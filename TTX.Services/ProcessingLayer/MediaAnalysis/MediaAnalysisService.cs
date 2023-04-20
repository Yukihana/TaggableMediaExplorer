using Microsoft.Extensions.Logging;

namespace TTX.Services.ProcessingLayer.MediaAnalysis;

public partial class MediaAnalysisService : IMediaAnalysisService
{
    private readonly ILogger _logger;
    private readonly MediaAnalysisOptions _options;

    public MediaAnalysisService(
        ILogger<MediaAnalysisService> logger,
        IWorkspaceProfile profile)
    {
        _logger = logger;
        _options = profile.InitializeServiceOptions<MediaAnalysisOptions>();
    }
}