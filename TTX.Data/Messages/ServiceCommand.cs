namespace TTX.Data.Messages;

public struct ServiceCommand : IMessage
{
    public string CommandString;
    public string TargetService;
}