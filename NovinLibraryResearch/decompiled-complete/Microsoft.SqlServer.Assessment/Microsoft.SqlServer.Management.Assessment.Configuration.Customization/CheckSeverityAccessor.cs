using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckSeverityAccessor : IChangeAccessor<Check, SeverityLevel>
{
	public static CheckSeverityAccessor Instance { get; } = new CheckSeverityAccessor();


	private CheckSeverityAccessor()
	{
	}

	public void SetValue(Check target, SeverityLevel newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Level = newValue;
	}

	public SeverityLevel GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Level;
	}
}
