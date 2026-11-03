using System.Collections.Generic;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public class StiSystemVariablesHelper
{
	protected static List<string> systemVariablesV1;

	protected static List<string> systemVariablesV2;

	public static string GetSystemVariableInfo(string variable)
	{
		return StiLocalization.Get("SystemVariables", variable);
	}

	public static List<string> GetSystemVariables(StiReport report)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		if (report == null || (int)report.EngineVersion != 2)
		{
			return GetSystemVariablesV1();
		}
		return GetSystemVariablesV2();
	}

	public static List<string> GetSystemVariablesV1()
	{
		object obj = systemVariablesV1;
		if (obj == null)
		{
			obj = new List<string>
			{
				"Column", "Line", "LineThrough", "LineABC", "LineRoman", "PageNumber", "PageNofM", "TotalPageCount", "IsFirstPage", "IsLastPage",
				"PageCopyNumber", "ReportAlias", "ReportAuthor", "ReportChanged", "ReportCreated", "ReportDescription", "ReportName", "Time", "Today"
			};
			systemVariablesV1 = (List<string>)obj;
		}
		return (List<string>)obj;
	}

	public static List<string> GetSystemVariablesV2()
	{
		object obj = systemVariablesV2;
		if (obj == null)
		{
			obj = new List<string>
			{
				"Column", "Line", "LineThrough", "LineABC", "LineRoman", "GroupLine", "PageNumber", "PageNumberThrough", "PageNofM", "PageNofMThrough",
				"TotalPageCount", "TotalPageCountThrough", "IsFirstPage", "IsFirstPageThrough", "IsLastPage", "IsLastPageThrough", "PageCopyNumber", "ReportAlias", "ReportAuthor", "ReportChanged",
				"ReportCreated", "ReportDescription", "ReportName", "Time", "Today"
			};
			systemVariablesV2 = (List<string>)obj;
		}
		return (List<string>)obj;
	}
}
