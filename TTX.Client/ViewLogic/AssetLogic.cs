using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using TTX.Client.ViewData;
using TTX.Data.Shared.QueryObjects;
using TTX.Library.Helpers;

namespace TTX.Client.ViewLogic;

public partial class AssetLogic : ObservableObject
{
    // Data

    [ObservableProperty]
    private AssetData _data = new();

    // GUID
    private string _guid = string.Empty;

    public string GUID
    {
        get => _guid;
        set
        {
            if (_guid == value)
                return;

            _guid = value;

            _ = Task.Run(async ()
                => await SessionContext.DataLoader.LoadAssetCard(value, LoadDataFrom, SessionContext.CTS.Token).ConfigureAwait(false));
        }
    }

    public void LoadDataFrom(AssetCard card)
        => Data = card.CopyValues<AssetData>();
}