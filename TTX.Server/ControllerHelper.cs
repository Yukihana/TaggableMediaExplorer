using Microsoft.Extensions.Logging;
using System;

namespace TTX.Server;

public static class ControllerHelper
{
    internal static void LogRequestError<T, TEx, TReq>(this T logger, TEx ex, TReq request)
    where T : ILogger
    where TEx : Exception
    => logger.LogError(ex, "An error has occured trying to process the request: {request}", request);
}