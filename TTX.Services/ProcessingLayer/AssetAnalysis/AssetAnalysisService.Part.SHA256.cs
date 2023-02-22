using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.ProcessingLayer.AssetAnalysis;

public partial class AssetAnalysisService
{
    // Implement multiread
    // files below a certain size (100MB) will be fully read, then processed
    // files above, will be 1:1. Utilize Single concurrency semaphore for reads this way.

    private async partial Task<byte[]> ComputeSHA256Small(string path, CancellationToken token)
    {
        byte[] data;

        // IO Operation
        try
        {
            await _semaphoreIO.WaitAsync(token).ConfigureAwait(false);
            data = await File.ReadAllBytesAsync(path, token).ConfigureAwait(false);
        }
        finally { _semaphoreIO.Release(); }

        // Processing Operation
        try
        {
            await _semaphoreProc.WaitAsync(token).ConfigureAwait(false);
            return SHA256.HashData(data);
        }
        finally { _semaphoreProc.Release(); }
    }

    private async partial Task<byte[]> ComputeSHA256Big(string path, CancellationToken token)
    {
        try
        {
            await _semaphoreIO.WaitAsync(token).ConfigureAwait(false);

            using var fileStream = new FileStream(
                path: path,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.Read,
                bufferSize: _options.ReadBufferSize,
                useAsync: true);

            using var sha256 = SHA256.Create();
            return await sha256.ComputeHashAsync(fileStream, token).ConfigureAwait(false);
        }
        finally { _semaphoreIO.Release(); }
    }
}