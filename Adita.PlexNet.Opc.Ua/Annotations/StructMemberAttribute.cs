using CommunityToolkit.Mvvm.ComponentModel;

namespace Adita.PlexNet.Opc.Ua.Annotations;

/// <summary>
/// Marks that a property or a field is the member of a structure.
/// </summary>
/// <remarks>
/// To use this in a field, the field should be marked also using <see cref="ObservablePropertyAttribute"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class StructMemberAttribute : Attribute
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of <see cref="StructMemberAttribute"/> using specified <paramref name="memberName"/>
    /// </summary>
    /// <param name="memberName">The member name of the structure on server side.</param>
    public StructMemberAttribute(string memberName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName);
        MemberName = memberName;
    }
    #endregion Constructors

    #region Public properties
    /// <summary>
    /// Gets the member name of the structure on server side.
    /// </summary>
    public string MemberName
    {
        get;
    } = string.Empty;
    #endregion Public properties
}
