using System.Threading.Tasks;

namespace TTX.Services.Legacy.Thumbnails;

/// <summary>
/// Service to extract, cache and deliver thumbnails.
/// </summary>
public class ThumbnailsService : IThumbnailsService
{
    public async Task<byte[]?> GetThumbnail(string uuid)
    {
        await Task.Delay(1);
        throw new System.NotImplementedException();
    }

    public async Task<bool?> IsThumbnailAvailable(string uuid)
    {
        await Task.Delay(1);
        throw new System.NotImplementedException();
    }
}