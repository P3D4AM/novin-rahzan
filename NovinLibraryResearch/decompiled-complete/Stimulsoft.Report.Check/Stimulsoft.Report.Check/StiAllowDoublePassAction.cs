using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiAllowDoublePassAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "On");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiAllowOnDoublePassActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiReport)
		{
			((StiReport)element).NumberOfPass = StiNumberOfPass.DoublePass;
		}
		else if (element is StiComponent)
		{
			((StiComponent)element).Report.NumberOfPass = StiNumberOfPass.DoublePass;
		}
	}
}
