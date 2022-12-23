namespace TTX.Data.Messages;

public struct ServiceCommand : IMessage
{
    public string TargetSID { get; set; }
    public string CommandString { get; set; }
}