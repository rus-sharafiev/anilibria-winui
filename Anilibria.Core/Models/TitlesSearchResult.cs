using Newtonsoft.Json;

namespace Anilibria.Core.Models;

public class TitlesSearchResult
{
    public Title[] List
    {
        get; set;
    }
    public Pagination Pagination
    {
        get; set;
    }
}

public partial class Pagination
{
    [JsonProperty("pages")]
    public long Pages
    {
        get; set;
    }

    [JsonProperty("current_page")]
    public long CurrentPage
    {
        get; set;
    }

    [JsonProperty("items_per_page")]
    public long ItemsPerPage
    {
        get; set;
    }

    [JsonProperty("total_items")]
    public long TotalItems
    {
        get; set;
    }
}