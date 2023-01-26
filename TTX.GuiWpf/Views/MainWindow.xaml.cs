using System.Windows;
using TTX.Client.ViewHandles;
using TTX.Client.ViewLogic;

namespace TTX.GuiWpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainView
{
    private readonly MainLogic _logic = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _logic;
    }

    public int ShowModal()
        => throw new System.NotImplementedException();

    public void ShowView()
        => Show();

    private void Window_Loaded(object sender, RoutedEventArgs e)
        => _logic.GuiLoaded();

    private void Window_Closed(object sender, System.EventArgs e)
        => _logic.GuiClosed();
}