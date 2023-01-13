using Microsoft.EntityFrameworkCore;
using TTX.Data.Entities;

namespace TTX.Data;

public class AssetsContext : DbContext
{
    public AssetsContext(DbContextOptions<AssetsContext> options) : base(options)
    { }

    // Tables

    public DbSet<AssetRecord> Assets => Set<AssetRecord>();
    public DbSet<TagRecord> Tags => Set<TagRecord>();
}