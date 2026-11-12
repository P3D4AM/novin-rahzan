using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiMoveComponentToPrintablePageAreaAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "StiMoveComponentToPrintablePageAreaActionShort");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiMoveComponentToPrintablePageAreaActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (!(element is StiComponent { Page: { } page } stiComponent))
		{
			return;
		}
		RectangleD paintRectangle = stiComponent.GetPaintRectangle(convertToHInches: false, convertByZoom: false);
		RectangleD clientRectangle = page.ClientRectangle;
		if (paintRectangle.Left < clientRectangle.Left)
		{
			stiComponent.Left = 0.0;
			paintRectangle.X = 0.0;
		}
		if (paintRectangle.Top < clientRectangle.Top)
		{
			stiComponent.Top = 0.0;
			paintRectangle.Y = 0.0;
		}
		if (paintRectangle.Right > clientRectangle.Right)
		{
			double num = paintRectangle.Right - clientRectangle.Right;
			stiComponent.Left -= num;
			paintRectangle.X -= num;
			if (paintRectangle.Left < clientRectangle.Left)
			{
				stiComponent.Width -= clientRectangle.Left - paintRectangle.Left;
				stiComponent.Left = 0.0;
			}
		}
		if (paintRectangle.Bottom > clientRectangle.Bottom)
		{
			double num2 = paintRectangle.Bottom - clientRectangle.Bottom;
			stiComponent.Top -= num2;
			paintRectangle.Y -= num2;
			if (paintRectangle.Top < clientRectangle.Top)
			{
				stiComponent.Height -= clientRectangle.Top - paintRectangle.Top;
				stiComponent.Top = 0.0;
			}
		}
	}
}
