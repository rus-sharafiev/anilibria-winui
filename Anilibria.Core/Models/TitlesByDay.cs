using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Anilibria.Core.Models;

public class TitlesByDay
{
    public long Day
    {
        get; set;
    }

    public Title[] List
    {
        get; set;
    }
}

public class GroupedTitles
{
    public GroupedTitles() => Titles = new ObservableCollection<Title>();

    public string WeekDay
    {
        get; set;
    }
    public ObservableCollection<Title> Titles
    {
        get; set;
    }
}