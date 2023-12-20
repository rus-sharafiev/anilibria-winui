using Newtonsoft.Json;

namespace Anilibria.Core.Models;

public partial class ReleaseBase
{
    public bool Status { get; set; }
    public Release Data { get; set; }
    public object Error { get; set; }
}

public partial class Release
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string[] Names { get; set; }
    public string Series { get; set; }
    public string Poster { get; set; }
    public long Last { get; set; }
    public string Moon { get; set; }
    public string Announce { get; set; }
    public string Status { get; set; }
    public long StatusCode { get; set; }
    public string Type { get; set; }
    public string[] Genres { get; set; }
    public string[] Voices { get; set; }
    public long Year { get; set; }
    public string Season { get; set; }
    public long Day { get; set; }
    public string Description { get; set; }
    public FranchiseElement[] Franchises { get; set; }
    public Members Members { get; set; }
    public BlockedInfo BlockedInfo { get; set; }
    public Playlist[] Playlist { get; set; }
    public ExternalPlaylist[] ExternalPlaylist { get; set; }
    public Favorite Favorite { get; set; }
    public Torrent[] Torrents { get; set; }
    public bool IsCurrentRelease {  get; set; }
    public bool IsAnimationAllowed { get; set; } = true;
}

public partial class BlockedInfo
{
    public bool Blocked { get; set; }
    public object Reason { get; set; }
    public bool Bakanim { get; set; }
    public bool Wakanim { get; set; }
    public bool Kinopoisk { get; set; }
}

public partial class ExternalPlaylist
{
    public string Tag { get; set; }
    public string Title { get; set; }
    public string ActionText { get; set; }
    public Episode[] Episodes { get; set; }
}

public partial class Episode
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
}

public partial class Favorite
{
    public long Rating { get; set; }
    public bool Added { get; set; }
}

public partial class FranchiseElement
{
    public Franchise Franchise { get; set; }
    public FranchiseRelease[] Releases { get; set; }
}

public partial class Franchise
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public partial class FranchiseRelease
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Ename { get; set; }
    public string Aname { get; set; }
    public string Alias { get; set; }
    public long Ordinal { get; set; }
}

public partial class Members
{
    public string[] Timing { get; set; }
    public string[] Voicing { get; set; }
    public string[] Editing { get; set; }
    public string[] Decorating { get; set; }
    public string[] Translating { get; set; }
}

public partial class Playlist
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public Uri SrcSd { get; set; }
    public Uri SrcHd { get; set; }
    public Skips Skips { get; set; }
    public string Poster { get; set; }
    public long Ordinal { get; set; }
    public Sources Sources { get; set; }
    [JsonProperty("rutube_id")]
    public string RutubeId { get; set; }
    [JsonProperty("updated_at")]
    public long UpdatedAt { get; set; }
    [JsonProperty("poster_thumbnail")]
    public string PosterThumbnail { get; set; }
    public Uri Sd { get; set; }
    public Uri Hd { get; set; }
    public Uri Fullhd { get; set; }
}

public partial class Skips
{
    public long[] Ending { get; set; }
    public long[] Opening { get; set; }
}

public partial class Sources
{
    [JsonProperty("is_rutube")]
    public bool IsRutube { get; set; }

    [JsonProperty("is_anilibria")]
    public bool IsAnilibria { get; set; }
}

public partial class Torrent
{
    public long Id { get; set; }
    public string Hash { get; set; }
    public long Leechers { get; set; }
    public long Seeders { get; set; }
    public long Completed { get; set; }
    public string Quality { get; set; }
    public string Series { get; set; }
    public long Size { get; set; }
    public string Url { get; set; }
    public long Ctime { get; set; }
}

public enum QltString { SD, HD, FHD }