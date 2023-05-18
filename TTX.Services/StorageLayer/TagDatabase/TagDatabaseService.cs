using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTX.Data.ServerData;

namespace TTX.Services.StorageLayer.TagDatabase;

public partial class TagDatabaseService : ITagDatabaseService
{
    private readonly IDbContextFactory<AssetsContext> _dbContextFactory;
    private readonly ILogger<TagDatabaseService> _logger;

    public TagDatabaseService(
        IDbContextFactory<AssetsContext> dbContextFactory,
        ILogger<TagDatabaseService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }
}