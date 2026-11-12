using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckEnabledAccessor : IChangeAccessor<Check, bool>
{
	public static CheckEnabledAccessor Instance { get; } = new CheckEnabledAccessor();


	private CheckEnabledAccessor()
	{
	}

	public void SetValue(Check target, bool newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Enabled = newValue;
	}

	public bool GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Enabled;
	}
}
