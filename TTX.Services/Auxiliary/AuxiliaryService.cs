namespace TTX.Services.Auxiliary;

public partial class AuxiliaryService : IAuxiliaryService
{
    private readonly AuxiliaryOptions _options;

    public AuxiliaryService(IOptionsSet options)
    {
        _options = options.ExtractValues<AuxiliaryOptions>();
    }
}