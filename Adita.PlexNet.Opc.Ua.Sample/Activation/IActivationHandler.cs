namespace Adita.PlexNet.Opc.Ua.Sample.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
