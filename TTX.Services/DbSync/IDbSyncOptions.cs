namespace TTX.Services.DbSync;

public interface IDbSyncOptions : IServiceOptions
{
    public string DbSyncSID { get; set; }
    public string IndexerSID { get; set; }
}