namespace TTX.Data.Models;

public interface IFileFootprint
{
    long SizeBytes { get; set; }
    byte[] Crumbs { get; set; }
}