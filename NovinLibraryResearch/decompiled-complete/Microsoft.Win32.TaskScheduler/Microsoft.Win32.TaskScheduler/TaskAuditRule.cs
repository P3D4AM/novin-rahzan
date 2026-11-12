using System.Security.AccessControl;
using System.Security.Principal;
using JetBrains.Annotations;

namespace Microsoft.Win32.TaskScheduler;

public sealed class TaskAuditRule : AuditRule
{
	public TaskRights TaskRights => (TaskRights)((AuthorizationRule)this).AccessMask;

	public TaskAuditRule([NotNull] IdentityReference identity, TaskRights eventRights, AuditFlags flags)
		: this(identity, (int)eventRights, isInherited: false, (InheritanceFlags)0, (PropagationFlags)0, flags)
	{
	}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


	internal TaskAuditRule([NotNull] IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
	{
	}//IL_0004: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_0008: Unknown result type (might be due to invalid IL or missing references)

}
