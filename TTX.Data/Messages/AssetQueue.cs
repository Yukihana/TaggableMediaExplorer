namespace TTX.Data.Messages;

public struct AssetQueue : IMessage
{
    public string TargetSID { get; set; }
    public string[] Paths { get; set; }
}