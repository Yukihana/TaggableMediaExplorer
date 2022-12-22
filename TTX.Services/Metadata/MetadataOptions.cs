namespace TTX.Services.Metadata;

public class MetadataOptions : IMetadataOptions
{
    public string MetadataSID { get; set; } = "mtd";
    public string IndexerSID { get; set; } = "idx";

    public void Initialize()
    {
    }
}