using Microsoft.Extensions.Logging;
using System;
using System.Text;
using TTX.Data.ServerData.Entities;
using TTX.Library.Helpers.StringHelpers;
using TTX.Services.StorageLayer.TagDatabase;

namespace TTX.Services.ApiLayer.TagData;

public partial class TagDataService : ITagDataService
{
    private readonly ITagDatabaseService _tagDatabase;
    private readonly ILogger<TagDataService> _logger;

    private readonly Random _random = new();

    public TagDataService(
        ITagDatabaseService tagDatabase,
        ILogger<TagDataService> logger)
    {
        _tagDatabase = tagDatabase;
        _logger = logger;
    }

    private TagRecord GenerateTagRecord(string key) => new()
    {
        TagId = key,
        Title = key.ToTitleFormat(),
        Description = "No description added yet.",
        Color = RandomColor(100, 200),
    };

    private string RandomColor(byte lowerLimit, byte upperLimit)
    {
        int colorCount = 3;
        int skip = _random.Next(0, colorCount * 1000) % colorCount;

        StringBuilder result = new("#");

        for (int i = 0; i < colorCount; i++)
        {
            result.Append(
                (i == skip ? 0 : (byte)_random.Next(lowerLimit, upperLimit))
                .ToString("X2"));
        }

        return result.ToString();
    }
}