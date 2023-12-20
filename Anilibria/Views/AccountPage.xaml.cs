using Anilibria.Helpers;
using Anilibria.ViewModels;
using Microsoft.UI.Xaml;
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

    private void LoginSubmitButton_Click(object _, RoutedEventArgs __)
    {
        if (string.IsNullOrEmpty(LoginTextBox.Text))
            ViewModel.UserNameError = "Account_EmptyUsernameFieldError".GetLocalized();
        else
            ViewModel.UserNameError = string.Empty;

        if (string.IsNullOrEmpty(PasswordBox.Password))
            ViewModel.PasswordError = "Account_EmptyPasswordFieldError".GetLocalized();
        else
            ViewModel.PasswordError = string.Empty;

        if (!string.IsNullOrEmpty(LoginTextBox.Text) && !string.IsNullOrEmpty(PasswordBox.Password))
            ViewModel.UserService.Login(LoginTextBox.Text, PasswordBox.Password);
    }
}
