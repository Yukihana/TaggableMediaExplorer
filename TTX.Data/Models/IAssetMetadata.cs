using System;

namespace TTX.Data.Models;

public interface IAssetMetadata
{
    string LocalPath { get; set; }
    long SizeBytes { get; set; }
    DateTime CreatedUtc { get; set; }
    DateTime ModifiedUtc { get; set; }
    byte[] Crumbs { get; set; }
}