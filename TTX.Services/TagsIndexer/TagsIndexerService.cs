using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Messages;
using TTX.Services.Communications;

namespace TTX.Services.TagsIndexer;

/// <summary>
/// Storage service for TagInfo entities.
/// </summary>
public class TagsIndexerService : ITagsIndexerService
{
    private readonly ITagsIndexerOptions _options;

    public TagsIndexerService(ITagsIndexerOptions options)
    {
        _options = options;
    }

    public async Task Reload()
    {
        await Invalidate();
        await Purge();
        await LoadRecords();
        await Validate();
    }

    private async Task Validate()
    {
        
    }

    private async Task LoadRecords()
    {
        
    }

    private async Task Purge()
    {
        
    }

    private async Task Invalidate()
    {
       
    }
}