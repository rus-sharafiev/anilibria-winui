using Newtonsoft.Json;

namespace Anilibria.Core.Models;

public class Title
{
    public long Id
    {
        get; set;
    }
    public string Code
    {
        get; set;
    }
    public Names Names
    {
        get; set;
    }
    public FranchiseElement[] Franchises
    {
        get; set;
    }
    public string Announce
    {
        get; set;
    }
    public Status Status
    {
        get; set;
    }
    public Posters Posters
    {
        get; set;
    }
    public long? Updated
    {
        get; set;
    }
    [JsonProperty("last_change")]
    public long LastChange
    {
        get; set;
    }
    public TypeClass Type
    {
        get; set;
    }
    public string[] Genres
    {
        get; set;
    }
    public Team Team
    {
        get; set;
    }
    public Season Season
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public long InFavorites
    {
        get; set;
    }
    public Blocked Blocked
    {
        get; set;
    }
    public Player Player
    {
        get; set;
    }
    public Torrents Torrents
    {
        get; set;
    }
}

public partial class Blocked
{
    public bool BlockedBlocked
    {
        get; set;
    }
    public bool Bakanim
    {
        get; set;
    }
}

public partial class FranchiseElement
{
    public Franchise Franchise
    {
        get; set;
    }
    public Release[] Releases
    {
        get; set;
    }
}

public partial class Franchise
{
    public Guid Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
}

public partial class Release
{
    public long Id
    {
        get; set;
    }
    public string Code
    {
        get; set;
    }
    public long Ordinal
    {
        get; set;
    }
    public Names Names
    {
        get; set;
    }
}

public partial class Names
{
    public string Ru
    {
        get; set;
    }
    public string En
    {
        get; set;
    }
    public string Alternative
    {
        get; set;
    }
}

public partial class Player
{
    public string AlternativePlayer
    {
        get; set;
    }
    public string Host
    {
        get; set;
    }
    public bool IsRutube
    {
        get; set;
    }
    public Episodes Episodes
    {
        get; set;
    }
    public Dictionary<string, ListValue> List
    {
        get; set;
    }
    public Dictionary<string, Rutube> Rutube
    {
        get; set;
    }
}

public partial class Episodes
{
    public long? First
    {
        get; set;
    }
    public long? Last
    {
        get; set;
    }
    public string String
    {
        get; set;
    }
}

public partial class ListValue
{
    public long Episode
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public Guid Uuid
    {
        get; set;
    }
    public long CreatedTimestamp
    {
        get; set;
    }
    public string Preview
    {
        get; set;
    }
    public Skips Skips
    {
        get; set;
    }
    public Hls Hls
    {
        get; set;
    }
}

public partial class Hls
{
    public string Fhd
    {
        get; set;
    }
    public string Hd
    {
        get; set;
    }
    public string Sd
    {
        get; set;
    }
}

public partial class Skips
{
    public long[] Opening
    {
        get; set;
    }
    public object[] Ending
    {
        get; set;
    }
}

public partial class Rutube
{
    public long Episode
    {
        get; set;
    }
    public long CreatedTimestamp
    {
        get; set;
    }
    public string RutubeId
    {
        get; set;
    }
}

public partial class Posters
{
    public Medium Small
    {
        get; set;
    }
    public Medium Medium
    {
        get; set;
    }
    public Medium Original
    {
        get; set;
    }
}

public partial class Medium
{
    public string Url
    {
        get; set;
    }
    public object RawBase64File
    {
        get; set;
    }
}

public partial class Season
{
    public SeasonString String
    {
        get; set;
    }
    public long Code
    {
        get; set;
    }
    public long Year
    {
        get; set;
    }
    public long WeekDay
    {
        get; set;
    }
}

public partial class Status
{
    public string String
    {
        get; set;
    }
    public long Code
    {
        get; set;
    }
}

public partial class Team
{
    public string[] Voice
    {
        get; set;
    }
    public string[] Translator
    {
        get; set;
    }
    public string[] Editing
    {
        get; set;
    }
    public string[] Decor
    {
        get; set;
    }
    public string[] Timing
    {
        get; set;
    }
}

public partial class Torrents
{
    public Episodes Episodes
    {
        get; set;
    }
    public TorrentsList[] List
    {
        get; set;
    }
}

public partial class TorrentsList
{
    public long TorrentId
    {
        get; set;
    }
    public Episodes Episodes
    {
        get; set;
    }
    public Quality Quality
    {
        get; set;
    }
    public long Leechers
    {
        get; set;
    }
    public long Seeders
    {
        get; set;
    }
    public long Downloads
    {
        get; set;
    }
    public long TotalSize
    {
        get; set;
    }
    public string SizeString
    {
        get; set;
    }
    public string Url
    {
        get; set;
    }
    public string Magnet
    {
        get; set;
    }
    public long UploadedTimestamp
    {
        get; set;
    }
    public string Hash
    {
        get; set;
    }
    public object Metadata
    {
        get; set;
    }
    public object RawBase64File
    {
        get; set;
    }
}

public partial class Quality
{
    public string String
    {
        get; set;
    }
    public string Type
    {
        get; set;
    }
    public string Resolution
    {
        get; set;
    }
    public string Encoder
    {
        get; set;
    }
    public object LqAudio
    {
        get; set;
    }
}

public partial class TypeClass
{
    public string FullString
    {
        get; set;
    }
    public long Code
    {
        get; set;
    }
    public string String
    {
        get; set;
    }
    public long? Episodes
    {
        get; set;
    }
    public long? Length
    {
        get; set;
    }
}

public enum SeasonString { Весна, Зима, Лето, Осень };
