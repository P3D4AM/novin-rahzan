using System.Security.AccessControl;
using System.Security.Principal;
using JetBrains.Annotations;

namespace Microsoft.Win32.TaskScheduler;

public sealed class TaskAccessRule : AccessRule
{
	public TaskRights TaskRights => (TaskRights)((AuthorizationRule)this).AccessMask;

	public TaskAccessRule([NotNull] IdentityReference identity, TaskRights eventRights, AccessControlType type)
		: this(identity, (int)eventRights, isInherited: false, (InheritanceFlags)0, (PropagationFlags)0, type)
	{
	}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


	public TaskAccessRule([NotNull] string identity, TaskRights eventRights, AccessControlType type)
		: this((IdentityReference)new NTAccount(identity), (int)eventRights, isInherited: false, (InheritanceFlags)0, (PropagationFlags)0, type)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
	//IL_000b: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Expected O, but got Unknown


	private TaskAccessRule([NotNull] IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
	{
	}//IL_0004: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_0008: Unknown result type (might be due to invalid IL or missing references)

}
