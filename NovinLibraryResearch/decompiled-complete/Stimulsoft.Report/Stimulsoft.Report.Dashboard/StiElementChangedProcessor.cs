using System.Linq;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard;

public static class StiElementChangedProcessor
{
	public static void ProcessElementChanging(object element, StiElementChangedArgs args)
	{
		switch (args.Action)
		{
		case StiElementMeterAction.Rename:
			ProcessElementRenaming(element, args);
			break;
		case StiElementMeterAction.ClearAll:
			ProcessElementClearing(element);
			break;
		case StiElementMeterAction.Delete:
			ProcessElementDeleting(element, args);
			break;
		}
	}

	private static void ProcessElementRenaming(object element, StiElementChangedArgs args)
	{
		IStiTransformActions val = (IStiTransformActions)((element is IStiTransformActions) ? element : null);
		if (val != null)
		{
			val.TransformActions?.Where((StiDataActionRule a) => a.Path == args.OldName).ToList().ForEach(delegate(StiDataActionRule a)
			{
				a.Path = args.NewName;
			});
		}
		IStiTransformFilters val2 = (IStiTransformFilters)((element is IStiTransformFilters) ? element : null);
		if (val2 != null)
		{
			val2.TransformFilters?.Where((StiDataFilterRule a) => a.Path == args.OldName).ToList().ForEach(delegate(StiDataFilterRule a)
			{
				a.Path = args.NewName;
			});
		}
	}

	private static void ProcessElementClearing(object element)
	{
		IStiTransformActions val = (IStiTransformActions)((element is IStiTransformActions) ? element : null);
		if (val != null)
		{
			val.TransformActions?.Clear();
		}
		IStiTransformFilters val2 = (IStiTransformFilters)((element is IStiTransformFilters) ? element : null);
		if (val2 != null)
		{
			val2.TransformFilters?.Clear();
		}
	}

	private static void ProcessElementDeleting(object element, StiElementChangedArgs args)
	{
		IStiTransformActions actions = (IStiTransformActions)((element is IStiTransformActions) ? element : null);
		IStiTransformActions obj = actions;
		if (obj != null)
		{
			obj.TransformActions?.Where((StiDataActionRule a) => a.Path == args.OldName).ToList().ForEach(delegate(StiDataActionRule a)
			{
				IStiTransformActions obj4 = actions;
				if (obj4 != null)
				{
					obj4.TransformActions.Remove(a);
				}
			});
		}
		IStiTransformFilters filters = (IStiTransformFilters)((element is IStiTransformFilters) ? element : null);
		IStiTransformFilters obj2 = filters;
		if (obj2 == null)
		{
			return;
		}
		obj2.TransformFilters?.Where((StiDataFilterRule a) => a.Path == args.OldName).ToList().ForEach(delegate(StiDataFilterRule a)
		{
			IStiTransformFilters obj3 = filters;
			if (obj3 != null)
			{
				obj3.TransformFilters.Remove(a);
			}
		});
	}
}
