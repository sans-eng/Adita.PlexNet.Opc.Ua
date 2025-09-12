namespace Adita.PlexNet.Opc.Ua.Annotations;

[AttributeUsage(AttributeTargets.Class)]
public class NamespaceAttribute : Attribute
{
    #region Constructors
    public NamespaceAttribute(string uri)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uri);
        Uri = uri;
    }
    #endregion Constructors

    #region Public properties
    public string Uri
    {
        get;
    }
    #endregion Public properties
}
