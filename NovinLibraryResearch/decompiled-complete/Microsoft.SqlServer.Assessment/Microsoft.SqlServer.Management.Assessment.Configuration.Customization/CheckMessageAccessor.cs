using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckMessageAccessor : IChangeAccessor<Check, string>
{
	public static CheckMessageAccessor Instance { get; } = new CheckMessageAccessor();


	private CheckMessageAccessor()
	{
	}

	public void SetValue(Check target, string newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Message = newValue;
	}

	public string GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Message;
	}
}
