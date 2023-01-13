using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Services.Integrity;

public partial class IntegrityService
{
    

    public async Task<byte[]> GetCrumbsAsync(string path, CancellationToken token = default)
    {
        using FileStream fs = new(path, FileMode.Open);
        var n = fs.Length;
        if (n <= _options.CrumbsCount)
        {
            byte[] smallcrumbs = new byte[n];
            await fs.ReadAsync(smallcrumbs, token);
            fs.Close();
            return smallcrumbs;
        }
        byte[] buffer = new byte[1];
        byte[] crumbs = new byte[_options.CrumbsCount];
        var spreadcount = _options.CrumbsCount - 1;
        long mult = n / spreadcount;
        for (int i = 0; i < spreadcount; i++)
        {
            fs.Position = i * mult;
            await fs.ReadAsync(buffer, token);
            crumbs[i] = buffer[0];
        }
        fs.Position = n - 1;
        await fs.ReadAsync(buffer, token);
        crumbs[spreadcount] = buffer[0];
        fs.Close();
        return crumbs;
    }
}