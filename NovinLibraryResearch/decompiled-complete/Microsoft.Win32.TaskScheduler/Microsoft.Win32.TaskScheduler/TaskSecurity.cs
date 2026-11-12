using System;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using JetBrains.Annotations;

namespace Microsoft.Win32.TaskScheduler;

public sealed class TaskSecurity : CommonObjectSecurity
{
	public override Type AccessRightType => typeof(TaskRights);

	public override Type AccessRuleType => typeof(TaskAccessRule);

	public override Type AuditRuleType => typeof(TaskAuditRule);

	public static TaskSecurity DefaultTaskSecurity
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			TaskSecurity taskSecurity = new TaskSecurity();
			taskSecurity.AddAccessRule(new TaskAccessRule((IdentityReference)new SecurityIdentifier((WellKnownSidType)22, (SecurityIdentifier)null), TaskRights.FullControl, (AccessControlType)0));
			taskSecurity.AddAccessRule(new TaskAccessRule((IdentityReference)new SecurityIdentifier((WellKnownSidType)26, (SecurityIdentifier)null), TaskRights.Write | TaskRights.ReadAttributes | TaskRights.ReadExtendedAttributes | TaskRights.ReadData, (AccessControlType)0));
			taskSecurity.AddAccessRule(new TaskAccessRule((IdentityReference)new SecurityIdentifier((WellKnownSidType)23, (SecurityIdentifier)null), TaskRights.Read, (AccessControlType)0));
			taskSecurity.AddAccessRule(new TaskAccessRule((IdentityReference)new SecurityIdentifier((WellKnownSidType)17, (SecurityIdentifier)null), TaskRights.Read, (AccessControlType)0));
			taskSecurity.AddAccessRule(new TaskAccessRule((IdentityReference)new SecurityIdentifier((WellKnownSidType)24, (SecurityIdentifier)null), TaskRights.Read, (AccessControlType)0));
			return taskSecurity;
		}
	}

	public TaskSecurity()
		: base(false)
	{
	}

	public TaskSecurity([NotNull] Task task, AccessControlSections sections = 14)
		: base(false)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		((ObjectSecurity)this).SetSecurityDescriptorSddlForm(task.GetSecurityDescriptorSddlForm(Convert(sections)), sections);
		((ObjectSecurity)(object)this).CanonicalizeAccessRules();
	}

	public TaskSecurity([NotNull] TaskFolder folder, AccessControlSections sections = 14)
		: base(false)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		((ObjectSecurity)this).SetSecurityDescriptorSddlForm(folder.GetSecurityDescriptorSddlForm(Convert(sections)), sections);
		((ObjectSecurity)(object)this).CanonicalizeAccessRules();
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return (AccessRule)(object)new TaskAccessRule(identityReference, (TaskRights)accessMask, type);
	}

	public void AddAccessRule([NotNull] TaskAccessRule rule)
	{
		((CommonObjectSecurity)this).AddAccessRule((AccessRule)(object)rule);
	}

	public void AddAuditRule([NotNull] TaskAuditRule rule)
	{
		((CommonObjectSecurity)this).AddAuditRule((AuditRule)(object)rule);
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return (AuditRule)(object)new TaskAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
	}

	public bool RemoveAccessRule([NotNull] TaskAccessRule rule)
	{
		return ((CommonObjectSecurity)this).RemoveAccessRule((AccessRule)(object)rule);
	}

	public void RemoveAccessRuleAll([NotNull] TaskAccessRule rule)
	{
		((CommonObjectSecurity)this).RemoveAccessRuleAll((AccessRule)(object)rule);
	}

	public void RemoveAccessRuleSpecific([NotNull] TaskAccessRule rule)
	{
		((CommonObjectSecurity)this).RemoveAccessRuleSpecific((AccessRule)(object)rule);
	}

	public bool RemoveAuditRule([NotNull] TaskAuditRule rule)
	{
		return ((CommonObjectSecurity)this).RemoveAuditRule((AuditRule)(object)rule);
	}

	public void RemoveAuditRuleAll(TaskAuditRule rule)
	{
		((CommonObjectSecurity)this).RemoveAuditRuleAll((AuditRule)(object)rule);
	}

	public void RemoveAuditRuleSpecific([NotNull] TaskAuditRule rule)
	{
		((CommonObjectSecurity)this).RemoveAuditRuleSpecific((AuditRule)(object)rule);
	}

	public void ResetAccessRule([NotNull] TaskAccessRule rule)
	{
		((CommonObjectSecurity)this).ResetAccessRule((AccessRule)(object)rule);
	}

	public void SetAccessRule([NotNull] TaskAccessRule rule)
	{
		((CommonObjectSecurity)this).SetAccessRule((AccessRule)(object)rule);
	}

	public void SetAuditRule([NotNull] TaskAuditRule rule)
	{
		((CommonObjectSecurity)this).SetAuditRule((AuditRule)(object)rule);
	}

	public override string ToString()
	{
		return ((ObjectSecurity)this).GetSecurityDescriptorSddlForm((AccessControlSections)14);
	}

	private static SecurityInfos Convert(AccessControlSections si)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		SecurityInfos val = (SecurityInfos)0;
		if ((si & 1) != 0)
		{
			val = (SecurityInfos)(val | 8);
		}
		if ((si & 2) != 0)
		{
			val = (SecurityInfos)(val | 4);
		}
		if ((si & 8) != 0)
		{
			val = (SecurityInfos)(val | 2);
		}
		if ((si & 4) != 0)
		{
			val = (SecurityInfos)(val | 1);
		}
		return val;
	}

	private static AccessControlSections Convert(SecurityInfos si)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		AccessControlSections val = (AccessControlSections)0;
		if ((si & 8) != 0)
		{
			val = (AccessControlSections)(val | 1);
		}
		if ((si & 4) != 0)
		{
			val = (AccessControlSections)(val | 2);
		}
		if ((si & 2) != 0)
		{
			val = (AccessControlSections)(val | 8);
		}
		if ((si & 1) != 0)
		{
			val = (AccessControlSections)(val | 4);
		}
		return val;
	}

	private AccessControlSections GetAccessControlSectionsFromChanges()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		AccessControlSections val = (AccessControlSections)0;
		if (((ObjectSecurity)this).AccessRulesModified)
		{
			val = (AccessControlSections)2;
		}
		if (((ObjectSecurity)this).AuditRulesModified)
		{
			val = (AccessControlSections)(val | 1);
		}
		if (((ObjectSecurity)this).OwnerModified)
		{
			val = (AccessControlSections)(val | 4);
		}
		if (((ObjectSecurity)this).GroupModified)
		{
			val = (AccessControlSections)(val | 8);
		}
		return val;
	}

	[SecurityCritical]
	internal void Persist([NotNull] Task task, AccessControlSections includeSections = 14)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((ObjectSecurity)this).WriteLock();
		try
		{
			AccessControlSections accessControlSectionsFromChanges = GetAccessControlSectionsFromChanges();
			if ((int)accessControlSectionsFromChanges != 0)
			{
				task.SetSecurityDescriptorSddlForm(((ObjectSecurity)this).GetSecurityDescriptorSddlForm(accessControlSectionsFromChanges));
				bool flag2 = (((ObjectSecurity)this).AuditRulesModified = false);
				bool flag4 = (((ObjectSecurity)this).AccessRulesModified = flag2);
				bool ownerModified = (((ObjectSecurity)this).GroupModified = flag4);
				((ObjectSecurity)this).OwnerModified = ownerModified;
			}
		}
		finally
		{
			((ObjectSecurity)this).WriteUnlock();
		}
	}

	[SecurityCritical]
	internal void Persist([NotNull] TaskFolder folder, AccessControlSections includeSections = 14)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((ObjectSecurity)this).WriteLock();
		try
		{
			AccessControlSections accessControlSectionsFromChanges = GetAccessControlSectionsFromChanges();
			if ((int)accessControlSectionsFromChanges != 0)
			{
				folder.SetSecurityDescriptorSddlForm(((ObjectSecurity)this).GetSecurityDescriptorSddlForm(accessControlSectionsFromChanges));
				bool flag2 = (((ObjectSecurity)this).AuditRulesModified = false);
				bool flag4 = (((ObjectSecurity)this).AccessRulesModified = flag2);
				bool ownerModified = (((ObjectSecurity)this).GroupModified = flag4);
				((ObjectSecurity)this).OwnerModified = ownerModified;
			}
		}
		finally
		{
			((ObjectSecurity)this).WriteUnlock();
		}
	}

	protected override void Persist([NotNull] string name, AccessControlSections includeSections = 14)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		using TaskService taskService = new TaskService();
		Task task = taskService.GetTask(name);
		Persist(task, includeSections);
	}
}
