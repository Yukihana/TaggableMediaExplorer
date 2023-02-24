using System;
using System.Collections.Generic;
using TTX.Data.Entities;

namespace TTX.Services.AbstractionLayer.AssetQuery;

public interface IAssetQueryService
{
    TOutput PerformQuery<TInput, TOutput>(TInput input, Func<TInput, IEnumerable<AssetRecord>, TOutput> func);
}