using Anilibria.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Anilibria.Views;

public sealed partial class AccountPage : Page
{
    public AccountViewModel ViewModel
    {
        get;
    }

    public AccountPage()
    {
        ViewModel = App.GetService<AccountViewModel>();
        InitializeComponent();
    }

    private void LoginSubmitButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.UserService.Login(Login.Text, Password.Password);
    }
}
