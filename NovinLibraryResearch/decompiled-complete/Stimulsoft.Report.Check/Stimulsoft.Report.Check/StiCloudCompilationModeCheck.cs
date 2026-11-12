using System.Collections.Generic;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCloudCompilationModeCheck : StiReportCheck
{
	public override bool PreviewVisible => false;

	public override string ShortMessage => StiLocalizationExt.Get("CheckReport", "StiCloudCompilationModeCheckShort");

	public override string LongMessage => StiLocalizationExt.Get("CheckReport", "StiCloudCompilationModeCheckLong");

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		List<StiCheck> result = null;
		if (report.CalculationMode == StiCalculationMode.Compilation)
		{
			StiCloudCompilationModeCheck stiCloudCompilationModeCheck = new StiCloudCompilationModeCheck();
			stiCloudCompilationModeCheck.Actions.Add(new StiChangeToInterpretationModeAction());
			result = new List<StiCheck> { stiCloudCompilationModeCheck };
		}
		return result;
	}
}
