using System.Windows;
using TTX.Client.Services.MainGui;

namespace TTX.GuiWpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainView
{
    private MainLogic? _logic = null;

    public MainWindow()
        => InitializeComponent();

    public void SetViewContext(MainLogic mainLogic)
        => DataContext = _logic = mainLogic;

    public void ShowView()
        => ShowDialog();

    private void Window_Loaded(object sender, RoutedEventArgs e)
        => _logic?.GuiLoaded();
}