using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Entities;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    public async partial Task<bool> Delete(byte[] itemId, CancellationToken ctoken)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            await DeleteInStorage(itemId, ctoken).ConfigureAwait(false);
            await DeleteInMemory(itemId, ctoken).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update the database. ItemId: {id}", new Guid(itemId));
            return false;
        }
    }

    private async partial Task DeleteInStorage(byte[] itemId, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        using var context = _dbContextFactory.CreateDbContext();
        AssetRecord target = await SelectOneOrThrow(context, itemId, ctoken).ConfigureAwait(false);

        if (!_options.EnableAssetDeletion)
            throw new InvalidOperationException("Deletion simulation succeeded. Actual deletion is disabled.");

        context.Assets.Remove(target);
        await context.SaveChangesAsync(ctoken).ConfigureAwait(false);
        _logger.LogWarning("Asset deleted from the database. Item ID: {id}", new Guid(itemId));
    }

    private async partial Task DeleteInMemory(byte[] itemId, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        AssetRecord target = SelectOneOrThrow(itemId);

        await ModifyCacheSafely(() => _cache.Remove(target), ctoken).ConfigureAwait(false);
    }
}