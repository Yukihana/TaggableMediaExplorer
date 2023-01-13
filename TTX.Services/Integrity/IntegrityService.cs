using System.Threading;

namespace TTX.Services.Integrity;

/// <summary>
/// Class for verifying video integrity and identity.
/// </summary>
public partial class IntegrityService : IIntegrityService
{
    private readonly IntegrityOptions _options;

    private readonly SemaphoreSlim _semaphoreCrumbs;

    public IntegrityService(IOptionsSet options)
    {
        _options = options.ExtractValues<IntegrityOptions>();
        _semaphoreCrumbs = new(_options.CrumbsConcurrency);
    }
}