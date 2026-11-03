using System.Reflection;

namespace Stimulsoft.Report;

public static class StiStimulsoftApps
{
	public const string StimulsoftReportsFile = "Stimulsoft-Reports";

	public const string StimulsoftUltimateFile = "Stimulsoft-Ultimate";

	public static bool? IsDesignerRunFromDesktop { get; set; }

	public static bool IsDesktopApp
	{
		get
		{
			if (IsDesignerRunFromDesktop.HasValue)
			{
				return IsDesignerRunFromDesktop.GetValueOrDefault();
			}
			string text = Assembly.GetExecutingAssembly().Location.ToLowerInvariant();
			return text.Contains("desktop");
		}
	}

	public static string StimulsoftCurrentAppName
	{
		get
		{
			if (!IsDesktopApp)
			{
				return "Stimulsoft Designer";
			}
			return "Stimulsoft Desktop";
		}
	}

	public static string StimulsoftCurrentAppFile
	{
		get
		{
			if (!IsDesktopApp)
			{
				return "Stimulsoft-Designer";
			}
			return "Stimulsoft-Desktop";
		}
	}
}
