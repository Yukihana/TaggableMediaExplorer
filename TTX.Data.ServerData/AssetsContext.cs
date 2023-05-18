using Microsoft.EntityFrameworkCore;
using TTX.Data.ServerData.Entities;

namespace TTX.Data.ServerData;

public class AssetsContext : DbContext
{
    public AssetsContext(DbContextOptions<AssetsContext> options) : base(options)
    {
        //Assets = Set<AssetRecord>();
        //Tags = Set<TagRecord>();
    }

    // Tables

    public DbSet<AssetRecord> Assets { get; set; }
    public DbSet<TagRecord> Tags { get; set; }
}