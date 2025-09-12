using System.Reflection;

namespace Adita.PlexNet.Opc.Ua;
public class StructMemberInfo
{
    #region Constructors
    public StructMemberInfo(PropertyInfo targetPropertyInfo, string memberName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName);
        TargetPropertyInfo = targetPropertyInfo ?? throw new ArgumentNullException(nameof(targetPropertyInfo));
        MemberName = memberName;
    }
    #endregion Constructors

    #region Public properties
    public PropertyInfo TargetPropertyInfo { get;}
    public string MemberName
    {
        get;
    }
    #endregion Public properties
}
