using TTX.Data.Shared.Messages;

namespace TTX.Data.Shared.Types;

/// <summary>
/// Asset file metadata with hash information.
/// </summary>
public struct HashedAssetFile : IMessage
{
    public string FullPath;
}