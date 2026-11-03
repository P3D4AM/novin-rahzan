using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Painters;

public class StiCrossHeaderGdiPainter : StiTextGdiPainter
{
	public override void PaintInteraction(StiComponent component, Graphics g)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if (component.IsDesigning || !(component is StiCrossHeader) || component.Report.Info.Zoom < 0.5 || !component.Report.Info.ShowInteractive || (int)component.Report.EngineVersion == 1)
		{
			return;
		}
		StiCrossHeader masterHeader = component as StiCrossHeader;
		if (component != null && ((IStiInteraction)component).Interaction is StiCrossHeaderInteraction && ((StiCrossHeaderInteraction)((IStiInteraction)component).Interaction).CollapsingEnabled)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			RectangleF rectangleF = ((RectangleD)(ref paintRectangle)).ToRectangleF();
			Image val = (StiCrossTabV2Builder.IsCollapsed(masterHeader) ? StiDataBuilder.ImageCollapsed : StiDataBuilder.ImageExpanded);
			int num = (int)rectangleF.Left + StiScale.I3;
			if (StiOptions.Viewer.Pins.EventsRightToLeft)
			{
				num = (int)rectangleF.Right - StiScale.I3 - val.Width;
			}
			g.DrawImage(val, num, (int)rectangleF.Y + StiScale.I3, val.Width, val.Height);
		}
	}
}
