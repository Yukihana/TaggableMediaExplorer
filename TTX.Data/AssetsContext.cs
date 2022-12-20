using Microsoft.EntityFrameworkCore;
using TTX.Data.Entities;

namespace TTX.Data;

public class AssetsContext : DbContext
{
    public AssetsContext(DbContextOptions<AssetsContext> options) : base(options)
    { }

    // Assets

    public DbSet<AssetIdentity> Identities => Set<AssetIdentity>();
    public DbSet<AssetIntegrity> Hashes => Set<AssetIntegrity>();
    public DbSet<AssetMetadata> Metadatas => Set<AssetMetadata>();
    public DbSet<AssetDescription> Descriptions => Set<AssetDescription>();
    public DbSet<AssetMediaInfo> MediaInfos => Set<AssetMediaInfo>();

    // Tags

    public DbSet<TagInfo> Tags => Set<TagInfo>();
}