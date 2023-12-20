using Newtonsoft.Json;

namespace Anilibria.Core.Models;

public partial class ScheduleBase
{
    public bool Status { get; set; }
    public Day[] Data { get; set; }
    public object Error { get; set; }
}

public partial class Day
{
    [JsonProperty("day")]
    public string Number { get; set; }
    public Release[] Items { get; set; }
}
