using System.Threading;
using System.Threading.Tasks;

namespace TTX.Client.Services.TagSelectorGui;

internal interface ITagSelectorGuiService
{
    Task<string?> ShowModalAsync(CancellationToken ctoken = default);
}