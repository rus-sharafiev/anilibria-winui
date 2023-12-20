
namespace Anilibria.Core.Models;

internal class SearchBase
{
    public bool Status { get; set; }
    public Release[] Data { get; set; }
    public object Error { get; set; }
}
