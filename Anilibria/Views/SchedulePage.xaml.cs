using Anilibria.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Anilibria.Views;

public sealed partial class SchedulePage : Page
{
    public ScheduleViewModel ViewModel { get; }

    public SchedulePage()
    {
        ViewModel = App.GetService<ScheduleViewModel>();
        InitializeComponent();

        NavigationCacheMode = NavigationCacheMode.Enabled;
    }
}
