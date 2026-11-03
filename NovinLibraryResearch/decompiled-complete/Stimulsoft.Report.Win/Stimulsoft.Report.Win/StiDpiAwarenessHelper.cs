using System.Runtime.InteropServices;

namespace Stimulsoft.Report.Win;

public static class StiDpiAwarenessHelper
{
	private enum Type
	{
		DpiUnaware,
		SystemDpiAware,
		PerMonitorDpiAware
	}

	[DllImport("shcore.dll")]
	private static extern int SetProcessDpiAwareness(Type value);

	public static void SetDpiUnaware()
	{
		SetProcessDpiAwareness(Type.DpiUnaware);
	}

	public static void SetSystemDpiAware()
	{
		SetProcessDpiAwareness(Type.SystemDpiAware);
	}

	public static void SetPerMonitorDpiAware()
	{
		SetProcessDpiAwareness(Type.PerMonitorDpiAware);
	}
}
