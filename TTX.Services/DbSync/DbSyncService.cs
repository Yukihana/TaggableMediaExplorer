using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTX.Data;

namespace TTX.Services.DbSync;

/// <summary>
/// The service for handling database read/write operations.
/// </summary>
public partial class DbSyncService : IDbSyncService
{
    private readonly IDbContextFactory<AssetsContext> _contextFactory;
    private readonly ILogger<DbSyncService> _logger;
    private readonly DbSyncOptions _options;

    public DbSyncService(IDbContextFactory<AssetsContext> contextFactory, ILogger<DbSyncService> logger, IOptionsSet options)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _options = options.ExtractValues<DbSyncOptions>();
    }
}