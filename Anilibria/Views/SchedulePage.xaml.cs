using Anilibria.Contracts.Services;
using Anilibria.Core.Models;
using Anilibria.ViewModels;
using CommunityToolkit.WinUI.UI.Animations;
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
