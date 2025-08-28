namespace Adita.PlexNet.Opc.Ua.Sample.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
