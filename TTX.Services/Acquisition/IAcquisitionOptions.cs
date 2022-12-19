namespace TTX.Services.Acquisition;

public interface IAcquisitionOptions : IServiceOptions
{
    // SID

    string AcquisitionSID { get; set; }

    // Base options

    string ServerRoot { get; set; }
    string AssetsPath { get; set; }
    string[] Whitelist { get; set; }
    string[] Blacklist { get; set; }
    string[] FinalAdds { get; set; }

    // Derived

    string AssetsPathFull { get; set; }
}