namespace TTX.Services.Acquisition;

public interface IAcquisitionOptions : IServiceOptions
{
    // Base options

    string ServerRoot { get; set; }
    string AssetsPath { get; set; }
    string[] Whitelist { get; set; }
    string[] Blacklist { get; set; }
    string[] FinalAdds { get; set; }

    // Service IDs

    string AcquisitionSID { get; set; }
    string MetadataSID { get; set; }

    // Derived

    string AssetsPathFull { get; set; }
}