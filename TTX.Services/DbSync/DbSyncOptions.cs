namespace TTX.Services.DbSync;

public class DbSyncOptions : IDbSyncOptions
{
    public string DbSyncSID { get; set; } = "dbs";
    public string IndexerSID { get; set; } = "idx";

    public void Initialize()
    { }
}