using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TTX.Client.ViewContexts.MediaViewContext;
using TTX.Client.ViewContexts.PlayerViewContext;

namespace TTX.GuiWpf.SubViews;

/// <summary>
/// Interaction logic for PlayerView.xaml
/// </summary>
public partial class PlayerView : UserControl
{
    public PlayerView()
    {
        InitializeComponent();
    }

    private void PerformContextAction(Action<PlayerContextLogic> contextAction)
    {
        if (DataContext is PlayerContextLogic contextLogic)
            contextAction(contextLogic);
    }

    // Active Media - Source change handler

    public QueueItemContext ActiveMedia
    {
        get { return (QueueItemContext)GetValue(ActiveMediaProperty); }
        set { SetValue(ActiveMediaProperty, value); }
    }

    public static readonly DependencyProperty ActiveMediaProperty =
        DependencyProperty.Register(
            "ActiveMedia",
            typeof(QueueItemContext),
            typeof(PlayerView),
            new PropertyMetadata(null, ActiveMediaChanged));

    private static void ActiveMediaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PlayerView playerView &&
            e.NewValue is QueueItemContext item
            && !string.IsNullOrEmpty(item.CachePath)
            && File.Exists(item.CachePath))
        {
            // FFME
            //_ = Task.Run(async () => await playerView.PlayerControl.Open(new Uri(path)));

            // Inbuilt ME
            playerView.PlayerControl.Source = new Uri(item.CachePath);
            playerView.PlayerControl.Play();
        }
    }

    // Event Input

    public string MessageInputPipe
    {
        get { return (string)GetValue(MessageInputPipeProperty); }
        set { SetValue(MessageInputPipeProperty, value); }
    }

    public static readonly DependencyProperty MessageInputPipeProperty =
        DependencyProperty.Register(
            "MessageInputPipe",
            typeof(string),
            typeof(PlayerView),
            new PropertyMetadata(string.Empty, MessageRecieved));

    private static void MessageRecieved(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PlayerView playerView &&
            e.NewValue is string input &&
            !string.IsNullOrEmpty(input))
        {
            playerView.ExecuteInput(input);
        }
    }

    // Inputs

    private void ExecuteInput(string input)
    {
        switch (input)
        {
            case "play":
                PlayerControl.Play();
                break;

            case "stop":
                PlayerControl.Stop();
                break;

            default:
                break;
        }
    }

    // Event Output

    public string MessageOutputPipe
    {
        get { return (string)GetValue(MessageOutputPipeProperty); }
        set { SetValue(MessageOutputPipeProperty, value); }
    }

    public static readonly DependencyProperty MessageOutputPipeProperty =
        DependencyProperty.Register(
            "MessageOutputPipe",
            typeof(string),
            typeof(PlayerView),
            new PropertyMetadata(string.Empty));

    // Outputs

    private void PlayerControl_MediaEnded(object sender, EventArgs e)
        => MessageOutputPipe = "ended";

    private void PlayerControl_MediaOpened(object sender, RoutedEventArgs e)
        => MessageOutputPipe = "opened";

    private void PlayerControl_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        => MessageOutputPipe = "failed";
}