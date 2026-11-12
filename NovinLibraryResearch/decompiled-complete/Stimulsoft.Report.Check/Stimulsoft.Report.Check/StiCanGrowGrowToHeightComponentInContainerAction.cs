using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCanGrowGrowToHeightComponentInContainerAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "SetGrowToHeightToTrue");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiCanGrowGrowToHeightComponentInContainerLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (!(element is StiContainer stiContainer))
		{
			return;
		}
		foreach (StiComponent component in stiContainer.Components)
		{
			component.GrowToHeight = true;
		}
	}
}
