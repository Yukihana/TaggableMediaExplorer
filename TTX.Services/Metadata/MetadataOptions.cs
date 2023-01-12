namespace TTX.Services.Metadata;

public class MetadataOptions : IServiceOptions
{
    public int MetadataConcurrency { get; set; } = 4;

    public void Initialize()
    {
    }
}