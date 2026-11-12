using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckTargetAccessor : IChangeAccessor<Check, IPattern<ISqlObjectLocator>>
{
	public static CheckTargetAccessor Instance { get; } = new CheckTargetAccessor();


	private CheckTargetAccessor()
	{
	}

	public void SetValue(Check target, IPattern<ISqlObjectLocator> newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Target = newValue;
	}

	public IPattern<ISqlObjectLocator> GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Target;
	}
}
