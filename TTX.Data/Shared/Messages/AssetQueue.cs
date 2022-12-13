namespace TTX.Data.Shared.Messages;

public struct AssetQueue : IMessage
{
    public string[] Paths;
}