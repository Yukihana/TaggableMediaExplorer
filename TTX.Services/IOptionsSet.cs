using TTX.Services.AssetInfo;
using TTX.Services.AssetsIndexer;
using TTX.Services.Auxiliary;
using TTX.Services.DbSync;
using TTX.Services.TagsIndexer;
using TTX.Services.Thumbnails;
using TTX.Services.Watcher;

namespace TTX.Services;

public interface IOptionsSet :
    IAssetInfoOptions,
    IAssetsIndexerOptions,
    IAuxiliaryOptions,
    IDbSyncOptions,
    ITagsIndexerOptions,
    IThumbnailOptions,
    IWatcherOptions
{
}