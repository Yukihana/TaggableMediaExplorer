using TTX.Data.Shared.Messages;

namespace TTX.Data.Shared.Types;

/// <summary>
/// Asset file metadata.
/// </summary>
public struct AssetFile : IMessage
{
    public string FullPath;
}