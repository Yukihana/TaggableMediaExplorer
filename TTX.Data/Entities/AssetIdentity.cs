namespace TTX.Data.Entities;

public class AssetIdentity
{
    public int ID { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public byte[]? GUID { get; set; } = null;
}