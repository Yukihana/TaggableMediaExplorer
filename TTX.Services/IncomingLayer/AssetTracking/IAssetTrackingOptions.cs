namespace TTX.Services.IncomingLayer.AssetTracking;

public interface IAssetTrackingOptions : IServiceProfile
{
    string AssetsPath { get; set; }
    string[] Whitelist { get; set; }
    string[] Blacklist { get; set; }
    string[] FinalAdds { get; set; }
}