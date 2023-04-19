using System.Windows;
using TTX.Client.Services.MainGui;
using TTX.Client.ViewContexts.MainViewContext;

namespace TTX.GuiWpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainView
{
    private MainContextLogic? _viewContext = null;

    public MainWindow()
        => InitializeComponent();

    public void SetViewContext(MainContextLogic viewContext)
        => DataContext = _viewContext = viewContext;

    public void ShowView()
        => ShowDialog();

    private void Window_Loaded(object sender, RoutedEventArgs e)
        => _viewContext?.GuiLoaded();
}