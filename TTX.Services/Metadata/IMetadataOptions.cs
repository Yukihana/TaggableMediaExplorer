namespace TTX.Services.Metadata;

public interface IMetadataOptions : IServiceOptions
{
    string MetadataSID { get; set; }
    string AssetsIndexerSID { get; set; }
}