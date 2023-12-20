using Newtonsoft.Json;

namespace Anilibria.Core.Models;

public partial class UserBase
{
    public bool Status { get; set; }
    public UserData Data { get; set; }
    public object Error { get; set; }
}

public partial class UserData
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Avatar { get; set; }
}