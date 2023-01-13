using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;
using TTX.Library.Helpers;

namespace TTX.Services.AssetsIndexer;

public partial class AssetsIndexerService
{
    private readonly SemaphoreSlim _semaphore = new(1);

    private readonly HashSet<AssetRecord> _records = new();
}