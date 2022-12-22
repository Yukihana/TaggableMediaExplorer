namespace TTX.Data.Messages;

public interface IMessage
{
    string TargetService { get; set; }
}