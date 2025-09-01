namespace Adita.PlexNet.Opc.Ua.Samples.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
