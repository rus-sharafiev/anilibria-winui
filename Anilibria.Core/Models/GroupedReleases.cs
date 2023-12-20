using System.Collections.ObjectModel;

namespace Anilibria.Core.Models;

public class GroupedReleases
{
    public GroupedReleases()
    {
        Releases = [];
    }

    public string GroupTitle { get; set; }
    public ObservableCollection<Release> Releases { get; set; }
}
