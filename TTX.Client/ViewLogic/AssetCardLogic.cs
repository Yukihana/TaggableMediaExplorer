using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using TTX.Client.ViewData;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Client.ViewLogic;

public partial class AssetCardLogic : ObservableObject
{
    // Data

    [ObservableProperty]
    private AssetCardData _dataModel = new();

    // ItemId for loading
    private string _itemIdString = string.Empty;

    public string ItemIdString
    {
        set
        {
            _itemIdString = value;
            Load();
        }
    }

    // Load
    private Task? _loadTask = null;

    private void Load()
    {
        _loadTask = Task.Run(async () =>
            await SessionContext.ApiConnectionService.LoadAssetCard(_itemIdString, LoadDataFrom, SessionContext.CancellationToken).ConfigureAwait(false));
        _loadTask.ContinueWith(t => _loadTask = null);
    }

    public void LoadDataFrom(AssetCardResponse card)
        => DataModel = card.CopyValues<AssetCardData>();
}