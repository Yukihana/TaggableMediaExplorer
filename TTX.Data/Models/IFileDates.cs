using System;

namespace TTX.Data.Models;

public interface IFileDates
{
    DateTime CreatedUtc { get; set; }
    DateTime ModifiedUtc { get; set; }
}