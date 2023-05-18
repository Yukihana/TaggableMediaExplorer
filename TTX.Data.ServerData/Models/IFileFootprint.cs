namespace TTX.Data.ServerData.Models;

public interface IFileFootprint
{
    long SizeBytes { get; set; }
    byte[] Crumbs { get; set; }
}