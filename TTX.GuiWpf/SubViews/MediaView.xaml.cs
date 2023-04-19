using System.Collections.Generic;
using System.Windows.Controls;
using TTX.Client.ViewContexts;
using TTX.Client.ViewContexts.MediaViewContext;

namespace TTX.GuiWpf.SubViews;

/// <summary>
/// Interaction logic for MediaView.xaml
/// </summary>
public partial class MediaView : UserControl
{
    public MediaView()
    {
        InitializeComponent();
    }

    private MediaContextLogic? ViewContext
        => DataContext as MediaContextLogic;

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MediaContextLogic viewContext && sender is ListView container)
        {
            List<QueueItemContext> selected = new();
            foreach (var item in container.SelectedItems)
            {
                if (item is QueueItemContext queueItem)
                    selected.Add(queueItem);
            }

            viewContext.ContextData.SelectedItems = selected.ToArray();
        }
    }
}