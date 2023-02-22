using System;

namespace TTX.Data.Models;

public interface IAssetMetadata
{
    string FullPath { get; set; }
    DateTime CreatedUtc { get; set; }
    DateTime ModifiedUtc { get; set; }
    long SizeBytes { get; set; }
    byte[] Crumbs { get; set; }

    // Derived

    string LocalPath { get; set; }
}