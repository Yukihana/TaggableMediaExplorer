namespace TTX.Data.ServerData.Models;

public class AssetFullSyncQueueItem
{
    public string LocalPath { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public bool Handled { get; set; } = false;
    public int FailedAttempts { get; set; } = 0;
    public int CyclesToSkipTillNextRetry { get; set; } = 0;
}