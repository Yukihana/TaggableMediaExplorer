using Microsoft.EntityFrameworkCore;
using TTX.Data;

namespace TTX.Services.DbSync;

/// <summary>
/// The service for handling database read/write operations.
/// </summary>
public partial class DbSyncService : IDbSyncService
{
    private readonly DbSyncOptions _options;
    private readonly IDbContextFactory<AssetsContext> _contextFactory;

    public DbSyncService(IDbContextFactory<AssetsContext> contextFactory, IOptionsSet options)
    {
        _contextFactory = contextFactory;
        _options = options.ExtractValues<DbSyncOptions>();
    }
}