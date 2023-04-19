using LibVLCSharp.Shared;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TTX.Client.ViewContexts.MediaViewContext;

namespace TTX.GuiWpf.SubViews;

/// <summary>
/// Interaction logic for PlayerView.xaml
/// </summary>
public partial class PlayerView : UserControl
{
    public PlayerView()
    {
        InitializeComponent();

        Core.Initialize();
        _libVLC = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVLC);
        _mediaPlayer.EndReached += MediaPlayer_EndReached;
    }

    // Load

    private readonly LibVLC _libVLC;
    private readonly MediaPlayer _mediaPlayer;

    private void PlayerControl_Loaded(object sender, RoutedEventArgs e)
        => PlayerControl.MediaPlayer = _mediaPlayer;

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
            playerView._mediaPlayer.Play(new Media(playerView._libVLC, new Uri(item.CachePath)));
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
                _mediaPlayer.Play();
                break;

            case "stop":
                _mediaPlayer.Stop();
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

    private void MediaPlayer_EndReached(object? sender, EventArgs e)
        => Dispatcher.Invoke(() => MessageOutputPipe = "ended");

    private void PlayerControl_MediaOpened(object sender, RoutedEventArgs e)
        => MessageOutputPipe = "opened";

    private void PlayerControl_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        => MessageOutputPipe = "failed";
}