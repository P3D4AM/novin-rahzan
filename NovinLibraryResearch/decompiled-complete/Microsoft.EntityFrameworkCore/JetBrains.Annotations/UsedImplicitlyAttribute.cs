using System;

namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.All)]
internal sealed class UsedImplicitlyAttribute : Attribute
{
	public JetBrains.Annotations.ImplicitUseKindFlags UseKindFlags { get; }

	public JetBrains.Annotations.ImplicitUseTargetFlags TargetFlags { get; }

	public UsedImplicitlyAttribute()
		: this(JetBrains.Annotations.ImplicitUseKindFlags.Default, JetBrains.Annotations.ImplicitUseTargetFlags.Default)
	{
	}

	public UsedImplicitlyAttribute(JetBrains.Annotations.ImplicitUseKindFlags useKindFlags)
		: this(useKindFlags, JetBrains.Annotations.ImplicitUseTargetFlags.Default)
	{
	}

	public UsedImplicitlyAttribute(JetBrains.Annotations.ImplicitUseTargetFlags targetFlags)
		: this(JetBrains.Annotations.ImplicitUseKindFlags.Default, targetFlags)
	{
	}

	public UsedImplicitlyAttribute(JetBrains.Annotations.ImplicitUseKindFlags useKindFlags, JetBrains.Annotations.ImplicitUseTargetFlags targetFlags)
	{
		UseKindFlags = useKindFlags;
		TargetFlags = targetFlags;
	}
}
