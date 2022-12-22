namespace TTX.Data.Messages;

/// <summary>
/// Asset file metadata.
/// </summary>
public struct AssetFile : IMessage
{
    public string TargetService { get; set; }
    public string FullPath { get; set; }
}