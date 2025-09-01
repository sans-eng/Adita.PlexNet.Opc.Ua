namespace Adita.PlexNet.Opc.Ua.Samples.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
