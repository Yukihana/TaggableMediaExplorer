namespace TTX.Data.Messages;

public struct ServiceCommand : IMessage
{
    public string TargetService { get; set; }
    public string CommandString { get; set; }
}