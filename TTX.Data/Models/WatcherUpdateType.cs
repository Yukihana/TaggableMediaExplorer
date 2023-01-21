namespace TTX.Data.Models;

public enum WatcherUpdateType : byte
{
    Unknown = 0,
    Created,
    Moved,
    Modified,
    Deleted,
}