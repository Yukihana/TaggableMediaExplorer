namespace TTX.Data.Entities;

public class AssetMediaInfo
{
    public int Height { get; set; } = 0;
    public int Width { get; set; } = 0;
    public int Length { get; set; } = 0;

    public string Container { get; set; } = string.Empty;
    public string DefaultVideoTrackCodec { get; set; } = string.Empty;
    public string DefaultAudioTrackCodec { get; set; } = string.Empty;
    public string DefaultSubtitlesFormat { get; set; } = string.Empty;
}