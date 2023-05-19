using System;
using System.Windows;
using System.Windows.Controls;
using TTX.Client.Services.TagSelectorGui;
using TTX.Client.ViewContexts.TagSelectorViewContext;

namespace TTX.GuiWpf.Views;

/// <summary>
/// Interaction logic for TagSelectorWindow.xaml
/// </summary>
public partial class TagSelectorWindow : Window, ITagSelectorView
{
    private TagSelectorContextLogic? _viewContext = null;

    public TagSelectorWindow()
        => InitializeComponent();

    public void SetViewContext(TagSelectorContextLogic viewContext)
        => DataContext = _viewContext = viewContext;

    public string? ShowModal()
    {
        if (_viewContext is null)
            throw new InvalidOperationException("Cannot start this view without a view context.");

        return ShowDialog() == true ? _viewContext.SearchText : null;
    }

    // Events

    private void Window_Loaded(object sender, RoutedEventArgs e)
        => SearchBox.Focus();

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender.Equals(ConfirmButton))
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                MessageBox.Show("The tag cannot be an empty string or whitespace.");
                return;
            }
            DialogResult = true;
        }
        Close();
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox &&
            listBox.SelectedItem is null &&
            listBox.Items.Count > 0)
        {
            listBox.SelectedIndex = 0;
        }
    }
}