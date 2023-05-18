using System;

namespace TTX.Data.ServerData.Models;

public interface IFileDates
{
    DateTime CreatedUtc { get; set; }
    DateTime ModifiedUtc { get; set; }
}