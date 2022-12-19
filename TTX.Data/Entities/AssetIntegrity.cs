namespace TTX.Data.Entities;

public class AssetIntegrity
{
    public int ID { get; set; }
    public byte[]? GUID { get; set; } = null;
    public byte[]? SHA2 { get; set; } = null;
    public byte[]? FileCrumbs { get; set;} = null;
}