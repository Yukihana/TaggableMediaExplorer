using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Services.Communications;

namespace TTX.Services.Metadata;

public class MetadataService : ServiceBase, IMetadataService
{
    private readonly IMetadataOptions _options;

    public override string Identifier => _options.MetadataSID;


    public MetadataService(IMessageBus bus, IMetadataOptions options) : base(bus, 5)
    {
        _options = options;
    }


    protected override async Task ProcessMessage(IMessage message, CancellationToken token = default)
    {
        if(message is AssetQueue queue)
        {
            foreach(string path in queue.Paths)
            {
                Console.WriteLine(path);
            }
        }
    }
}