using System;
using Stimulsoft.Base;
using Stimulsoft.Report.Dashboard.Visuals;

namespace Stimulsoft.Report.Dashboard;

public static class StiDashboardHelperCreator
{
	public static IStiTableElementAutoSizer CreateTableElementAutoSizer(IStiTableElement tableElement)
	{
		try
		{
			if (StiDashboardDrawingAssembly.Assembly == null)
			{
				return null;
			}
			Type type = StiDashboardDrawingAssembly.Assembly.GetType("Stimulsoft.Dashboard.Drawing.Helpers.StiTableElementAutoSizer");
			if (type == null)
			{
				return null;
			}
			return StiActivator.CreateObject(type) as IStiTableElementAutoSizer;
		}
		catch
		{
		}
		return null;
	}

	public static IStiProgressVisualSvgHelper CreateProgressVisualSvgHelper()
	{
		try
		{
			if (StiDashboardAssembly.Assembly == null)
			{
				return null;
			}
			Type type = StiDashboardAssembly.Assembly.GetType("Stimulsoft.Dashboard.Helpers.StiProgressVisualSvgHelper");
			if (type == null)
			{
				return null;
			}
			return StiActivator.CreateObject(type) as IStiProgressVisualSvgHelper;
		}
		catch
		{
		}
		return null;
	}

	public static IStiIndicatorVisualSvgHelper CreateIndicatorVisualSvgHelper()
	{
		try
		{
			if (StiDashboardAssembly.Assembly == null)
			{
				return null;
			}
			Type type = StiDashboardAssembly.Assembly.GetType("Stimulsoft.Dashboard.Helpers.StiIndicatorVisualSvgHelper");
			if (type == null)
			{
				return null;
			}
			return StiActivator.CreateObject(type) as IStiIndicatorVisualSvgHelper;
		}
		catch
		{
		}
		return null;
	}

	public static IStiGaugeVisualSvgHelper CreateGaugeVisualSvgHelper()
	{
		try
		{
			if (StiDashboardAssembly.Assembly == null)
			{
				return null;
			}
			Type type = StiDashboardAssembly.Assembly.GetType("Stimulsoft.Dashboard.Helpers.StiGaugeVisualSvgHelper");
			if (type == null)
			{
				return null;
			}
			return StiActivator.CreateObject(type) as IStiGaugeVisualSvgHelper;
		}
		catch
		{
		}
		return null;
	}

	internal static IStiCardsVisualSvgHelper CreateCardsVisualSvgHelper()
	{
		try
		{
			if (StiDashboardAssembly.Assembly == null)
			{
				return null;
			}
			Type type = StiDashboardAssembly.Assembly.GetType("Stimulsoft.Dashboard.Helpers.StiCardsVisualSvgHelper");
			if (type == null)
			{
				return null;
			}
			return StiActivator.CreateObject(type) as IStiCardsVisualSvgHelper;
		}
		catch
		{
		}
		return null;
	}
}
