namespace TTX.Data.Messages;

public struct AssetQueue : IMessage
{
    public string[] Paths;
}