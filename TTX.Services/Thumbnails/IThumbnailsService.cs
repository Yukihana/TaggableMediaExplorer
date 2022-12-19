using System.Threading.Tasks;

namespace TTX.Services.Thumbnails;

public interface IThumbnailsService
{
    /// <summary>
    /// Gets the associated thumbnail for the provided uuid.
    /// </summary>
    /// <param name="uuid">The UUID of the asset.</param>
    /// <returns>A byte array of the thumbnail's stream data.</returns>
    Task<byte[]?> GetThumbnail(string uuid);

    /// <summary>
    /// Gets the current state of the thumbnail for this asset.
    /// </summary>
    /// <param name="uuid">The UUID of the asset.</param>
    /// <returns>True = available, False = unavailable/queued, null = uuid unavailable.</returns>
    Task<bool?> IsThumbnailAvailable(string uuid);
}