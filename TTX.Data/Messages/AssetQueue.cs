namespace TTX.Data.Messages;

public struct AssetQueue : IMessage
{
    public string TargetService { get; set; }
    public string[] Paths { get; set; }
}