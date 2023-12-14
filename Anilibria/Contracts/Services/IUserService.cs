

using Anilibria.Core.Models;

namespace Anilibria.Contracts.Services;

public interface IUserService
{
    UserData? User
    {
        get;
    }
    event EventHandler? UserChanged;

    Task InitializeAsync();
    void Login(string username, string password);
    void LogOut();
}
