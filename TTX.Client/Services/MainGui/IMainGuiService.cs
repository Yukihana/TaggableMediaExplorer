using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.MainGui;

internal interface IMainGuiService
{
    Task ShowAsync(CancellationToken ctoken = default);
}