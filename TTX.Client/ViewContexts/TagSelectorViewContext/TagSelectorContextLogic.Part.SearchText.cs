using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Client.ViewContexts.TagSelectorViewContext;

public partial class TagSelectorContextLogic
{
    private string _searchText = string.Empty;

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            OnSearchQueryChanged(_searchText, _guiSync.CancellationToken);
        }
    }

    private async void OnSearchQueryChanged(string searchText, CancellationToken ctoken = default)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            // call api
            RelatedTagsResponse response = await _apiConnection.GetRelatedTags(searchText, ctoken).ConfigureAwait(false);

            // update list
            await _guiSync.DispatchActionAsync(
                collection => MatchingTags = new ObservableCollection<string>(collection), response.TagIds,
                ctoken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "Encountered an error while searching for tags: {query}", searchText);
        }
    }
}