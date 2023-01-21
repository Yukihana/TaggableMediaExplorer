namespace TTX.Data.Models;

public struct WatcherUpdate
{
    public string FullPath { get; set; }
    public WatcherUpdateType UpdateType { get; set; }
    public string OldPath { get; set; }
}