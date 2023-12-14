using Newtonsoft.Json;

namespace Anilibria.Core.Models;

public class UserData
{
    public string Login
    {
        get; set;
    }
    public string Nickname
    {
        get; set;
    }
    public string Email
    {
        get; set;
    }
    public object Avatar
    {
        get; set;
    }
    [JsonProperty("vk_id")]
    public object VkId
    {
        get; set;
    }
    [JsonProperty("patreon_id")]
    public object PatreonId
    {
        get; set;
    }
    [JsonProperty("avatar_original")]
    public string AvatarOriginal
    {
        get; set;
    }
    [JsonProperty("avatar_thumbnail")]
    public string AvatarThumbnail
    {
        get; set;
    }
}

public partial class Session
{
    public string Err
    {
        get; set;
    }
    public string Mes
    {
        get; set;
    }
    public string Key
    {
        get; set;
    }
    public string SessionId
    {
        get; set;
    }
}