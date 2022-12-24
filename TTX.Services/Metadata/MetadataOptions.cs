namespace TTX.Services.Metadata;

public class MetadataOptions : IMetadataOptions
{
    public string MetadataSID { get; set; } = "mtd";
    public string AssetsIndexerSID { get; set; } = "aix";

    public void Initialize()
    {
    }
}