using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNegativeSizesOfComponentsAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Fix");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiNegativeSizesOfComponentsActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiComponent stiComponent)
		{
			if (stiComponent.Width < 0.0)
			{
				stiComponent.Width *= -1.0;
			}
			if (stiComponent.Height < 0.0)
			{
				stiComponent.Height *= -1.0;
			}
		}
	}
}
