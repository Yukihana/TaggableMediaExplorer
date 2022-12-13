﻿namespace TTX.Data.Services.Thumbnails;

internal class ThumbnailOptions
{
    public string DirectoryPath { get; set; } = "Thumbnails";
    public float ThumbnailTime { get; set; } = 0.2f;
    public string ThumbnailFormat { get; set; } = "PNG";
}