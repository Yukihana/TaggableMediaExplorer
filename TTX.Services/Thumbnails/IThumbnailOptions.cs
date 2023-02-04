namespace TTX.Services.Thumbnails;

public interface IThumbnailOptions : IServiceOptions
{
    string ServerRoot { get; set; }
    string ThumbsPath { get; set; }
    float ThumbnailTime { get; set; }
    string ThumbnailFormat { get; set; }
}