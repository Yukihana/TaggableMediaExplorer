namespace TTX.Data.Entities;

public class AssetIdentity
{
    public int ID { get; set; } = 0;
    public byte[]? GUID { get; set; } = null;

    // Identity

    public string LastLocation { get; set; } = string.Empty;
}