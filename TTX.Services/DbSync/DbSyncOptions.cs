namespace TTX.Services.DbSync;

public class DbSyncOptions : IDbSyncOptions
{
    public string DbSyncSID { get; set; } = "dbs";
    public string AssetsIndexerSID { get; set; } = "aix";
    public string TagsIndexerSID { get; set; } = "tix";

    public void Initialize()
    { }
}