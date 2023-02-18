namespace TTX.Services.IncomingLayer.AssetTracking;

public interface IAssetTrackingOptions : IServiceOptions
{
    string ServerRoot { get; set; }
    string AssetsPath { get; set; }
    string[] Whitelist { get; set; }
    string[] Blacklist { get; set; }
    string[] FinalAdds { get; set; }
}