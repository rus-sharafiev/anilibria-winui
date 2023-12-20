

using Anilibria.Core.Models;

namespace Anilibria.Contracts.Services;

public interface IUserService
{
    UserData? User { get; }
    event EventHandler? UserChanged;

    LoginError? LoginError { get; }
    event EventHandler? LoginErrorChanged;

    Task InitializeAsync();
    void Login(string username, string password);
    void LogOut();
}
