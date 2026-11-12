using System;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckConditionAccessor : IChangeAccessor<Check, Condition>
{
	public static CheckConditionAccessor Instance { get; } = new CheckConditionAccessor();


	private CheckConditionAccessor()
	{
	}

	public void SetValue(Check target, Condition newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Condition = newValue;
	}

	public Condition GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Condition;
	}
}
