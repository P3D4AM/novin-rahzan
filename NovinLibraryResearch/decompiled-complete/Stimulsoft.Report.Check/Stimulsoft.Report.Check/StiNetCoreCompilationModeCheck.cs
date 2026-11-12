using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNetCoreCompilationModeCheck : StiReportCheck
{
	public override bool PreviewVisible => false;

	public override string ShortMessage => StiLocalizationExt.Get("CheckReport", "StiNetCoreCompilationModeCheckShort");

	public override string LongMessage => StiLocalizationExt.Get("CheckReport", "StiNetCoreCompilationModeCheckLong");

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool CheckFramework()
	{
		string text = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
		if (!string.IsNullOrEmpty(text) && text.IndexOf("=v") > 0 && text.StartsWith(".NETCoreApp"))
		{
			string text2 = text.Substring(text.IndexOf("=v") + 2);
			if ((StiDpiHelper.IsWindows && text2.StartsWith("2")) || (!StiDpiHelper.IsWindows && text2.StartsWith("3")))
			{
				return true;
			}
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		List<StiCheck> result = null;
		if (report.CalculationMode == StiCalculationMode.Compilation && (CheckFramework() || StiReport.CheckNeedForceInterpretationMode()))
		{
			StiNetCoreCompilationModeCheck stiNetCoreCompilationModeCheck = new StiNetCoreCompilationModeCheck();
			stiNetCoreCompilationModeCheck.Actions.Add(new StiChangeToInterpretationModeAction());
			result = new List<StiCheck> { stiNetCoreCompilationModeCheck };
		}
		return result;
	}
}
