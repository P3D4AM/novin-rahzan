using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGrowToHeightOverlappingAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "SetGrowToHeightToFalse");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiGrowToHeightOverlappingLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiComponent stiComponent)
		{
			stiComponent.GrowToHeight = false;
		}
	}
}
