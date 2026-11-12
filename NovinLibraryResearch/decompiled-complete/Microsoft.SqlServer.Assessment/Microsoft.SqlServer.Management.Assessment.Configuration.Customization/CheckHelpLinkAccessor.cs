using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckHelpLinkAccessor : IChangeAccessor<Check, string>
{
	public static CheckHelpLinkAccessor Instance { get; } = new CheckHelpLinkAccessor();


	private CheckHelpLinkAccessor()
	{
	}

	public void SetValue(Check target, string newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.HelpLink = newValue;
	}

	public string GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.HelpLink;
	}
}
