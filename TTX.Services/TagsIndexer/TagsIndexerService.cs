using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Services.Communications;

namespace TTX.Services.TagsIndexer;

/// <summary>
/// Storage service for TagInfo entities.
/// </summary>
public class TagsIndexerService : ServiceBase, ITagsIndexerService
{
    private readonly ITagsIndexerOptions _options;
    public override string Identifier => _options.TagsIndexerSID;

    public TagsIndexerService(IMessageBus bus, ITagsIndexerOptions options) : base(bus, 1)
    {
        _options = options;
    }

    protected override Task ProcessMessage(IMessage message, CancellationToken token = default)
    {
        throw new System.NotImplementedException();
    }
}