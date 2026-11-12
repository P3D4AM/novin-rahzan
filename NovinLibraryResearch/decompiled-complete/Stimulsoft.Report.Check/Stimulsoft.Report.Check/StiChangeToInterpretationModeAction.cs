using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiChangeToInterpretationModeAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Change");

	public override string Description => StiLocalizationExt.Get("CheckActions", "ChangeReportToInterpretationMode");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		report.CalculationMode = StiCalculationMode.Interpretation;
	}
}
