using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiWordWrapCanGrowTextDoesNotFitAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "On");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiWordWrapCanGrowTextDoesNotFitActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiText)
		{
			((StiText)element).CanGrow = true;
		}
	}
}
