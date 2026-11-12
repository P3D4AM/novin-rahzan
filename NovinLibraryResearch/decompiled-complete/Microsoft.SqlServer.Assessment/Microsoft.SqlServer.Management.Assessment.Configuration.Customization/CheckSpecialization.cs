using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class CheckSpecialization : CheckRule
{
	public CheckSelector Selector { get; set; }

	public CheckSpecialization(CheckSelector selector, SqlObjectLocatorPattern target, IEnumerable<IItemChange<Check>> changes)
		: base(target, changes)
	{
		Selector = selector;
	}

	public bool IsMatch(Check source, ISqlObjectLocator target)
	{
		if (Selector.IsMatch(source))
		{
			return base.Target.IsMatch(target);
		}
		return false;
	}
}
