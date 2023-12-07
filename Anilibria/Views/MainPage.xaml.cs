using Anilibria.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Anilibria.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
