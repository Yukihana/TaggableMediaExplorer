using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.LoginGui;

internal interface ILoginGuiService
{
    Task<bool> ShowModalAsync(CancellationToken ctoken = default);
}