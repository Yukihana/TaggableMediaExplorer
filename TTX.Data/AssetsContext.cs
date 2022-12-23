using Microsoft.EntityFrameworkCore;
using TTX.Data.Entities;

namespace TTX.Data;

public class AssetsContext : DbContext
{
    public AssetsContext(DbContextOptions<AssetsContext> options) : base(options)
    { }

    // Tables

    public DbSet<AssetInfo> Assets => Set<AssetInfo>();
    public DbSet<TagInfo> Tags => Set<TagInfo>();
}