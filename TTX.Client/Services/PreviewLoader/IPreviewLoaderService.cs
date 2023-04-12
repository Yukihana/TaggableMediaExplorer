using System;
using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.PreviewLoader;

internal interface IPreviewLoaderService
{
    Task<string> GetDefaultPreview(string idString, DateTime lastUpdatedUtc, CancellationToken ctoken = default);
}