using Stimulsoft.Report.Components;

namespace Stimulsoft.Report;

public class StiStimulsoftServerResource
{
	public delegate byte[] StiGetImageDelegate(StiImage image, string key);

	public delegate string StiGetRichTextDelegate(StiRichText richText, string key);

	public delegate StiReport StiGetSubReportDelegate(StiSubReport SubReport, string key);

	public delegate StiReport StiGetDrillDownReportDelegate(StiComponent component, string key);

	public static StiGetImageDelegate GetImageDelegate;

	public static StiGetRichTextDelegate GetRichTextDelegate;

	public static StiGetSubReportDelegate GetSubReportDelegate;

	public static StiGetDrillDownReportDelegate GetDrillDownReportDelegate;

	public static byte[] GetImage(StiImage component, string key)
	{
		if (GetImageDelegate == null)
		{
			return null;
		}
		return GetImageDelegate(component, key);
	}

	public static string GetRichText(StiRichText component, string key)
	{
		if (GetRichTextDelegate == null)
		{
			return null;
		}
		return GetRichTextDelegate(component, key);
	}

	public static StiReport GetSubReport(StiSubReport component, string key)
	{
		if (GetSubReportDelegate == null)
		{
			return null;
		}
		return GetSubReportDelegate(component, key);
	}

	public static StiReport GetDrillDownReport(StiComponent component, string key)
	{
		if (GetDrillDownReportDelegate == null)
		{
			return null;
		}
		return GetDrillDownReportDelegate(component, key);
	}
}
