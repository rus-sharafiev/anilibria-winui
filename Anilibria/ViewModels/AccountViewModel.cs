using Anilibria.Contracts.Services;
using Anilibria.Core.Models;
using Anilibria.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace Anilibria.ViewModels;

public partial class AccountViewModel : ObservableRecipient
{
    [ObservableProperty]
    private UserData? _user;

    [ObservableProperty]
    private string? _profilePicture;

    [ObservableProperty]
    private string? _userNameError;

    [ObservableProperty]
    private string? _passwordError;

    [ObservableProperty]
    private Visibility _formVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _userDataVisibility = Visibility.Collapsed;

    public IUserService UserService { get; }
    public AccountViewModel(IUserService userService)
    {
        UserService = userService;
        UserService.UserChanged += UserService_UserChanged;
        UserService.LoginErrorChanged += UserService_LoginErrorChanged;
        User = UserService.User;
        ProfilePicture = User?.Avatar ?? "/upload/avatars/noavatar.jpg";
        FormVisibility = User is null ? Visibility.Visible : Visibility.Collapsed;
        UserDataVisibility = User is not null ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UserService_LoginErrorChanged(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(UserService.LoginError?.Key);
        switch (UserService.LoginError?.Key)
        {
            case "invalidUser":
                UserNameError = "Account_InvalidUserError".GetLocalized();
                PasswordError = null;
                break;

            case "wrongPasswd":
                PasswordError = "Account_WrongPasswdError".GetLocalized();
                UserNameError = null;
                break;

            case "empty":
                UserNameError = "Account_EmptyFieldError".GetLocalized();
                PasswordError = null;
                break;

            default:
                UserNameError = null;
                PasswordError = null;
                break;
        }
    }

    private void UserService_UserChanged(object? sender, EventArgs e)
    {
        User = UserService.User;
        ProfilePicture = User?.Avatar ?? "/upload/avatars/noavatar.jpg";
        FormVisibility = User is null ? Visibility.Visible : Visibility.Collapsed;
        UserDataVisibility = User is not null ? Visibility.Visible : Visibility.Collapsed;
    }

    public void LogOut() => UserService?.LogOut();
}
