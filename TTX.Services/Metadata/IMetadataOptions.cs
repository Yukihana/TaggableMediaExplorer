namespace TTX.Services.Metadata;

public interface IMetadataOptions : IServiceOptions
{
    string MetadataSID { get; set; }
    string IndexerSID { get; set; }
}