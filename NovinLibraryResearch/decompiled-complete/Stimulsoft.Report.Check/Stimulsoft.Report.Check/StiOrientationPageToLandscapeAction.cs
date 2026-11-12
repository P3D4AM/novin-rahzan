using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiOrientationPageToLandscapeAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "StiOrientationPageToLandscapeActionShort");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiOrientationPageToLandscapeActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiPage { Width: var width, Height: var height } stiPage)
		{
			stiPage.Orientation = StiPageOrientation.Landscape;
			stiPage.Width = width;
			stiPage.Height = height;
		}
	}
}
