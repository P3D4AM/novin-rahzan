using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiOrientationPageToPortraitAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "StiOrientationPageToPortraitActionShort");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiOrientationPageToPortraitActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiPage { Width: var width, Height: var height } stiPage)
		{
			stiPage.Orientation = StiPageOrientation.Portrait;
			stiPage.Width = width;
			stiPage.Height = height;
		}
	}
}
