using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Viewer;

internal static class StiDashboardViewerHelper
{
	internal static StiDashboardViewerControl FindViewer(Control control)
	{
		do
		{
			if (control is StiDashboardViewerControl)
			{
				return control as StiDashboardViewerControl;
			}
			control = control.Parent;
		}
		while (control != null && !(control is Form));
		return null;
	}

	internal static ControlCollection GetViewerControls(StiDashboardViewerControl viewer)
	{
		if (viewer == null)
		{
			return null;
		}
		return ((Control)viewer.GetElementsPanel()).Controls;
	}

	public static List<Control> GetAllControls(ControlCollection controls)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		List<Control> list = new List<Control>();
		if (controls == null)
		{
			return list;
		}
		foreach (Control item in (ArrangedElementCollection)controls)
		{
			Control val = item;
			list.Add(val);
			list.AddRange(GetAllControls(val.Controls));
		}
		return list;
	}

	public static StiDashboardViewerControl CreateViewerControl(StiReport report, int startPage = -1)
	{
		return new StiDashboardViewerControl(report, startPage);
	}

	public static List<StiDashboard> GetDashboardList(StiReport report)
	{
		return (from d in report?.Pages.OfType<StiDashboard>()
			where d.IsEnabled
			select d).ToList();
	}

	public static List<IStiRefreshControl> GetRefreshControlList(StiDashboardViewerControl viewer, IStiElement filterByElement = null)
	{
		return (from e in GetElementList(viewer.Dashboard)
			where filterByElement == null || e == filterByElement
			select e into c
			select viewer.ElementToControl[c] into c
			where c is IStiRefreshControl
			select c).Cast<IStiRefreshControl>().ToList();
	}

	internal static void EnableRefreshViewerProcessing(StiDashboardViewerControl viewer)
	{
		(from e in GetElementList(viewer.Dashboard)
			where e is IStiRefreshViewerElement
			select e).Cast<IStiRefreshViewerElement>().ToList().ForEach(delegate(IStiRefreshViewerElement c)
		{
			c.AllowRefreshControl = true;
		});
	}

	internal static void DisableRefreshViewerProcessing(StiDashboardViewerControl viewer)
	{
		(from e in GetElementList(viewer.Dashboard)
			where e is IStiRefreshViewerElement
			select e).Cast<IStiRefreshViewerElement>().ToList().ForEach(delegate(IStiRefreshViewerElement c)
		{
			c.AllowRefreshControl = false;
		});
	}

	public static List<IStiElement> GetElementList(StiDashboard dashboard, bool includesDisabled = false)
	{
		if (includesDisabled)
		{
			return dashboard.GetElements();
		}
		return (from c in dashboard.GetElements()
			where c.IsEnabled
			select c).ToList();
	}

	public static List<IStiFilterElement> GetFilterElementList(StiDashboard dashboard)
	{
		List<IStiFilterElement> filterElements = (from e in GetElementList(dashboard)
			where e is IStiFilterElement
			select e).Cast<IStiFilterElement>().ToList();
		return filterElements.OrderBy((IStiFilterElement f) => GetLevel(f, filterElements)).ToList();
	}

	public static List<StiHolderElementControl> GetHolderControlList(IStiDashboardViewer viewer)
	{
		return (from c in GetControlList(viewer)
			where c is StiHolderElementControl
			select c).Cast<StiHolderElementControl>().ToList();
	}

	public static List<StiElementControl> GetElementControlList(IStiDashboardViewer viewer)
	{
		return (from c in GetHolderControlList(viewer)
			select c.Child into c
			where c is StiElementControl
			select c).Cast<StiElementControl>().ToList();
	}

	public static List<IStiSeriesControl> GetSeriesControlList(IStiDashboardViewer viewer)
	{
		return (from c in GetControlList(viewer)
			where c is IStiSeriesControl
			select c).Cast<IStiSeriesControl>().ToList();
	}

	public static List<Control> GetControlList(IStiDashboardViewer viewer)
	{
		return GetControlList((IEnumerable)((Control)viewer.GetElementsPanel()).Controls);
	}

	public static List<Control> GetControlList(IEnumerable controls)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		List<Control> list = new List<Control>();
		foreach (Control control in controls)
		{
			Control val = control;
			list.Add(val);
			list.AddRange(GetControlList((IEnumerable)val.Controls));
		}
		return list;
	}

	public static Control GetControlByElement(StiDashboardViewerControl viewer, IStiElement element)
	{
		object? obj = (from c in GetElementList(viewer.Dashboard, includesDisabled: true)
			where c == element
			select viewer.ElementToControl[c]).FirstOrDefault();
		return (Control)((obj is Control) ? obj : null);
	}

	private static int GetLevel(IStiFilterElement element, List<IStiFilterElement> elements)
	{
		if (StiCrossLinkedFilterHelper.IsCrossLinkedFilter(element))
		{
			return 0;
		}
		int num = 0;
		while (true)
		{
			if (string.IsNullOrWhiteSpace(element.GetParentKey()))
			{
				return num;
			}
			element = elements.FirstOrDefault((IStiFilterElement e) => e.GetKey() == element.GetParentKey());
			if (element == null)
			{
				break;
			}
			num++;
		}
		return num;
	}
}
