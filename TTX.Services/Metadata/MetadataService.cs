using System.Collections.Generic;
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
        List<Task> tasks = new();

        if (message is AssetQueue queue)
        {
            foreach (string path in queue.Paths)
            {
                tasks.Add(ForwardMetadata(path, token));
            }
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private async Task ForwardMetadata(string path, CancellationToken token = default)
    {
        FileInfo info = new(path);

        AssetFile file = new()
        {
            TargetSID = _options.IndexerSID,
            FullPath = info.FullName,
            CreatedUtc = info.CreationTimeUtc,
            ModifiedUtc = info.LastWriteTimeUtc,
            SizeBytes = info.Length,
        };

        await SendMessage(file, token);
    }
}