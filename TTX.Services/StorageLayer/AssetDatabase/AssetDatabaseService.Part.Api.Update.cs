using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data;
using TTX.Data.Entities;
using TTX.Data.Extensions;

namespace TTX.Services.StorageLayer.AssetDatabase;

public partial class AssetDatabaseService
{
    public async partial Task<bool> Update(byte[] itemId, Action<AssetRecord> updateAction, CancellationToken ctoken)
    {
        try
        {
            ctoken.ThrowIfCancellationRequested();

            await UpdateInStorage(itemId, updateAction, ctoken).ConfigureAwait(false);
            UpdateInMemory(itemId, updateAction, ctoken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update database. Target ItemId: {id}", new Guid(itemId));
            return false;
        }
    }

    private async partial Task UpdateInStorage(byte[] itemId, Action<AssetRecord> action, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        using AssetsContext context = _dbContextFactory.CreateDbContext();
        var target = await SelectOneOrThrow(context, itemId, ctoken).ConfigureAwait(false);

        action.Invoke(target);
        await context.SaveChangesAsync(ctoken).ConfigureAwait(false);
    }

    private partial void UpdateInMemory(byte[] itemId, Action<AssetRecord> action, CancellationToken ctoken)
    {
        ctoken.ThrowIfCancellationRequested();

        AssetRecord target = SelectOneOrThrow(itemId);

        target.Write(action);
    }
}