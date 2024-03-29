﻿using System.Threading;
using System.Threading.Tasks;
using TTX.Data.Shared.QueryObjects;

namespace TTX.Services.ApiLayer.AssetCardData;

public interface IAssetCardDataService
{
    Task<AssetCardResponse> GetAssetCardData(string id, CancellationToken ctoken = default);
}