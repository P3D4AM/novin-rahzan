using System;

namespace Stimulsoft.Base.Wizards;

public static class StiCharterMapRunner
{
	private static Type windowType;

	private static bool isInit;

	private static void Init()
	{
		if (isInit)
		{
			return;
		}
		isInit = true;
		try
		{
			windowType = Type.GetType("Stimulsoft.Wizard.Wpf.Chartermap.StiChartermapWindow, Stimulsoft.Wizard.Wpf");
		}
		catch
		{
		}
	}

	public static bool AllowChartermapWindow()
	{
		Init();
		return windowType != null;
	}

	public static IStiCharterMapWindow StiWindow(string fontFamily)
	{
		Init();
		try
		{
			if (windowType != null)
			{
				return StiActivator.CreateObject(windowType, new object[1] { fontFamily }) as IStiCharterMapWindow;
			}
		}
		catch
		{
		}
		return null;
	}
}
