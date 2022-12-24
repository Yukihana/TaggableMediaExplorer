namespace TTX.Services.DbSync;

public interface IDbSyncOptions : IServiceOptions
{
    string DbSyncSID { get; set; }
    string AssetsIndexerSID { get; set; }
    string TagsIndexerSID { get; set; }
}