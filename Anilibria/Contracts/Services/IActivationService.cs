namespace Anilibria.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
