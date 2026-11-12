using System;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckTagsAccessor : IChangeAccessor<Check, string[]>
{
	public static CheckTagsAccessor Instance { get; } = new CheckTagsAccessor();


	private CheckTagsAccessor()
	{
	}

	public void SetValue(Check target, string[] newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Tags.Clear();
		if (newValue != null && newValue.Length != 0)
		{
			target.Tags.UnionWith(newValue);
		}
	}

	public string[] GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Tags.ToArray();
	}
}
