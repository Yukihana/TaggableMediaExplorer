using TTX.Data.Services.Acquisition;

namespace TTX.Data.Shared.Messages;

public struct AcquisitionCommand : IMessage
{
    public AcquisitionCommands CommandValue;
}