﻿using System;

namespace TTX.Data.Shared.Entities;

public class AssetMetadata
{
    public int ID { get; set; } = 0;
    public byte[]? GUID { get; set; } = null;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get;set; } = DateTime.Now;
    public long SizeBytes { get; set; } = 0;
}