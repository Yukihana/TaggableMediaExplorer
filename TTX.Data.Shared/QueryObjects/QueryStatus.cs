namespace TTX.Data.Shared.QueryObjects;

public enum QueryStatus : byte
{
    Unknown = 0,
    Success = 1,

    NotFound,
    AccessDenied,

    Queued,
    AlreadyQueued,
}