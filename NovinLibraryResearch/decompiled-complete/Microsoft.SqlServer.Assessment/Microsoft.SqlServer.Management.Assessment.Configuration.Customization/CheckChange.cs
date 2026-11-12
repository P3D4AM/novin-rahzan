using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class CheckChange : CheckSpecialization
{
	public CheckChange(CheckSelector selector, SqlObjectLocatorPattern target, IEnumerable<IItemChange<Check>> changes)
		: base(selector, target, changes)
	{
		if (target != null)
		{
			OverwriteTransform<IPattern<ISqlObjectLocator>> transform = new OverwriteTransform<IPattern<ISqlObjectLocator>>(target);
			ItemChange<Check, IPattern<ISqlObjectLocator>> item = new ItemChange<Check, IPattern<ISqlObjectLocator>>(CheckTargetAccessor.Instance, transform);
			base.Children.Add(item);
		}
	}

	public IEnumerable<Check> Apply(IEnumerable<Check> source)
	{
		foreach (Check item in source)
		{
			if (base.Selector.IsMatch(item))
			{
				ApplyTo(item);
			}
			yield return item;
		}
	}
}
