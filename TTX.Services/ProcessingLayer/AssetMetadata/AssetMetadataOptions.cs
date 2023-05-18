using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TTX.Services.ProcessingLayer.AssetMetadata;

internal class AssetMetadataOptions : IAssetMetadataOptions
{
    // Tags

    public bool IgnoreTagIdCasing { get; set; } = true;
    public char TagSeparator { get; set; } = ' ';

    // Derived

    [JsonIgnore]
    public IEqualityComparer<string> TagComparer { get; set; } = StringComparer.Ordinal;

    // Init

    public void Initialize()
    {
        TagComparer = IgnoreTagIdCasing ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    }

    public void Initialize(IRuntimeConfig runtimeConfig)
        => throw new NotImplementedException();
}