using System.Windows;
using TTX.Client.Services.LoginGui;

namespace TTX.GuiWpf.Views;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window, ILoginView
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    public int ShowModal()
    {
        var result = ShowDialog();
        return result switch
        {
            true => 1,
            _ => 0
        };
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender.Equals(LoginBtn))
            DialogResult = true;
        Close();
    }
}