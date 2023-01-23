using Microsoft.Extensions.Logging;

namespace TTX.Services.Auxiliary;

public partial class AuxiliaryService : IAuxiliaryService
{
    private readonly ILogger<AuxiliaryService> _logger;
    private readonly AuxiliaryOptions _options;

    public AuxiliaryService(ILogger<AuxiliaryService> logger, IOptionsSet options)
    {
        _logger = logger;
        _options = options.ExtractValues<AuxiliaryOptions>();
    }

    public void Summarize()
    {
        if (GetDuplicateFilesCount() is int dfCount && dfCount > 0)
            _logger.LogWarning("No of duplicate files: {count}", dfCount);

        if (GetDuplicateFilesCount() is int drCount && drCount > 0)
            _logger.LogWarning("No of duplicate records: {count}", drCount);

        if (GetDuplicateFilesCount() is int mfCount && mfCount > 0)
            _logger.LogWarning("No of modified files: {count}", mfCount);
    }
}