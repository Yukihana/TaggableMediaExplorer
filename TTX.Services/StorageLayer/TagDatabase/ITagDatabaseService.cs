using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TTX.Data.ServerData.Entities;

namespace TTX.Services.StorageLayer.TagDatabase;

public interface ITagDatabaseService
{
    Task Repair(CancellationToken ctoken = default);

    Task Read(Func<DbSet<TagRecord>, CancellationToken, Task> readAction, CancellationToken ctoken = default);

    Task Write(Func<DbSet<TagRecord>, CancellationToken, Task<bool>> writeAction, CancellationToken ctoken = default);
}