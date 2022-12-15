using System;
using TTX.Data.Shared.BaseClasses;

namespace TTX.Data.Services.Communications;

internal class MessageProcessor
{
    public ServiceBase Service;
    public Type[] MessageTypes;

    public MessageProcessor(ServiceBase service, Type[] messageTypes)
    {
        Service = service;
        MessageTypes = messageTypes;
    }
}