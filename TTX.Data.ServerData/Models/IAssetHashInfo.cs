using System;

namespace TTX.Data.ServerData.Models;

public interface IAssetHashInfo
{
    byte[] SHA256 { get; set; }
    DateTime VerifiedUtc { get; set; }
}