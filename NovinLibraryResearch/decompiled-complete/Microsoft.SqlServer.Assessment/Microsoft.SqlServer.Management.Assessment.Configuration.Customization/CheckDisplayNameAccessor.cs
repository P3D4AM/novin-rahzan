using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckDisplayNameAccessor : IChangeAccessor<Check, string>
{
	public static CheckDisplayNameAccessor Instance { get; } = new CheckDisplayNameAccessor();


	private CheckDisplayNameAccessor()
	{
	}

	public void SetValue(Check target, string newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.DisplayName = newValue;
	}

	public string GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.DisplayName;
	}
}
