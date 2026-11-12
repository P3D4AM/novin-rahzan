using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiSwitchWidthAndHeightOfPageAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Change");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiSwitchWidthAndHeightOfPageActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiPage stiPage)
		{
			stiPage.Orientation = ((stiPage.Orientation != StiPageOrientation.Landscape) ? StiPageOrientation.Landscape : StiPageOrientation.Portrait);
			double pageWidth = stiPage.PageWidth;
			stiPage.PageWidth = stiPage.PageHeight;
			stiPage.PageHeight = pageWidth;
		}
	}
}
