using Anilibria.Contracts.Services;
using Anilibria.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace Anilibria.ViewModels;

public partial class AccountViewModel : ObservableRecipient
{
    [ObservableProperty]
    private UserData? _user;

    [ObservableProperty]
    private Visibility _formVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _userDataVisibility = Visibility.Collapsed;

    public IUserService UserService { get; }
    public AccountViewModel(IUserService userService)
    {
        UserService = userService;
        UserService.UserChanged += UserService_UserChanged;
        User = UserService.User;
        FormVisibility = User is null ? Visibility.Visible : Visibility.Collapsed;
        UserDataVisibility = User is not null ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UserService_UserChanged(object? sender, EventArgs e)
    {
        User = UserService.User;
        FormVisibility = User is null ? Visibility.Visible : Visibility.Collapsed;
        UserDataVisibility = User is not null ? Visibility.Visible : Visibility.Collapsed;
    }

    public void LogOut() => UserService?.LogOut();
}
