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

    private async void Window_Loaded(object sender, RoutedEventArgs e)
        => await _logic.GuiLoaded().ConfigureAwait(true);
}