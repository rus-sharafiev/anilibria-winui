using Anilibria.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Anilibria.Views;

public sealed partial class ReleasesPage : Page
{
    public ReleasesViewModel ViewModel
    {
        get;
    }

    public ReleasesPage()
    {
        ViewModel = App.GetService<ReleasesViewModel>();
        InitializeComponent();
    }
}
