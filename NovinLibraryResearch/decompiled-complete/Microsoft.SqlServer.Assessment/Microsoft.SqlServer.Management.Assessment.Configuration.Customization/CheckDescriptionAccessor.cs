using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckDescriptionAccessor : IChangeAccessor<Check, string>
{
	public static CheckDescriptionAccessor Instance { get; } = new CheckDescriptionAccessor();


	private CheckDescriptionAccessor()
	{
	}

	public void SetValue(Check target, string newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Description = newValue;
	}

	public string GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Description;
	}
}
