namespace TTX.Data.Messages;

/// <summary>
/// Asset file metadata with hash information.
/// </summary>
public struct HashedAssetFile : IMessage
{
    public string FullPath;
}