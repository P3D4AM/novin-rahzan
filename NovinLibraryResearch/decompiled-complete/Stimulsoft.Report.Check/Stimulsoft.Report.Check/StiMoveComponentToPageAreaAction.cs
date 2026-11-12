using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiMoveComponentToPageAreaAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "StiMoveComponentToPageAreaActionShort");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiMoveComponentToPageAreaActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiComponent { Page: { } page } stiComponent)
		{
			RectangleD paintRectangle = stiComponent.GetPaintRectangle(convertToHInches: false, convertByZoom: false);
			RectangleD displayRectangle = page.DisplayRectangle;
			displayRectangle.X -= page.Margins.Left;
			displayRectangle.Y -= page.Margins.Top;
			if (paintRectangle.Left < displayRectangle.Left)
			{
				stiComponent.Left += displayRectangle.Left - paintRectangle.Left;
			}
			if (paintRectangle.Top < displayRectangle.Top)
			{
				stiComponent.Top += displayRectangle.Top - paintRectangle.Top;
			}
			if (paintRectangle.Right > displayRectangle.Right)
			{
				stiComponent.Left -= paintRectangle.Right - displayRectangle.Right;
			}
			if (paintRectangle.Bottom > displayRectangle.Bottom)
			{
				stiComponent.Top -= paintRectangle.Bottom - displayRectangle.Bottom;
			}
		}
	}
}
