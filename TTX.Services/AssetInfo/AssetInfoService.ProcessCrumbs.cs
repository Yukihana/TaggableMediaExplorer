using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.AssetInfo;

public partial class AssetInfoService
{
    private static long[] GetSpreadIndices(long length, int count)
    {
        if (length <= count)
        {
            long[] indiseq = new long[length];
            for (int i = 0; i < length; i++)
                indiseq[i] = i;
            return indiseq;
        }

        long[] indices = new long[count];
        var spreadcount = count - 1;
        long mult = length / spreadcount;
        for (int i = 0; i < spreadcount; i++)
            indices[i] = i * mult;
        indices[count - 1] = length - 1;

        return indices;
    }

    private async Task<byte[]> GetCrumbsAsync(string path, long[] indices, CancellationToken token = default)
    {
        try
        {
            await _semaphoreCrumbs.WaitAsync(token);

            byte[] buffer = new byte[1];
            byte[] crumbs = new byte[indices.Length];

            using FileStream fs = new(path, FileMode.Open);

            for (int i = 0; i < indices.Length; i++)
            {
                fs.Position = indices[i];
                await fs.ReadAsync(buffer, token);
                crumbs[i] = buffer[0];
            }

            fs.Close();

            return crumbs;
        }
        finally { _semaphoreCrumbs.Release(); }
    }
}