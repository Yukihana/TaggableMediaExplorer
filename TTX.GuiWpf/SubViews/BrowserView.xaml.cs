using System.Collections.Generic;
using System.Windows.Controls;
using TTX.Client.ViewContexts;
using TTX.Client.ViewContexts.BrowserViewContext;

namespace TTX.GuiWpf.SubViews;

/// <summary>
/// Interaction logic for BrowserView.xaml
/// </summary>
public partial class BrowserView : UserControl
{
    public BrowserView()
    {
        InitializeComponent();
    }

    private void FileView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is BrowserContextLogic viewContext &&
            sender is ListView container)
        {
            List<AssetCardContext> selected = new();
            foreach (var item in container.SelectedItems)
            {
                if (item is AssetCardContext cardData)
                    selected.Add(cardData);
            }

            viewContext.ContextData.SelectedItems = selected.ToArray();
        }
    }
}