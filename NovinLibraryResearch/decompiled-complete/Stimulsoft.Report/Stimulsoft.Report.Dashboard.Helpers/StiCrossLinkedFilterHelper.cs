using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiCrossLinkedFilterHelper
{
	public static bool IsCrossLinkedFilter(IStiFilterElement filterElement)
	{
		if (filterElement == null)
		{
			return false;
		}
		List<StiComponent> list = (from e in (filterElement as StiComponent)?.Page.GetComponents().ToList()
			where e is IStiFilterElement
			select e).ToList();
		if (list == null)
		{
			return false;
		}
		List<IStiFilterElement> list2 = new List<IStiFilterElement>();
		while (true)
		{
			string parentKey = filterElement.GetParentKey();
			if (string.IsNullOrWhiteSpace(parentKey))
			{
				return false;
			}
			filterElement = list.Cast<IStiAppCell>().FirstOrDefault((Func<IStiAppCell, bool>)((IStiAppCell e) => e != null && e.GetKey() == parentKey)) as IStiFilterElement;
			if (filterElement == null)
			{
				return false;
			}
			if (list2.Contains(filterElement))
			{
				break;
			}
			list2.Add(filterElement);
		}
		return true;
	}
}
