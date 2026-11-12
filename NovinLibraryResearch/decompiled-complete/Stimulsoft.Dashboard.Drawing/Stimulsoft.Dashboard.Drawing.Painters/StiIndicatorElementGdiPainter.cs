using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Visuals.Indicator;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiIndicatorElementGdiPainter : StiAtomElementGdiPainter
{
	public override void PaintCheckBoxTitle(Graphics g, RectangleD rect, IStiElement element)
	{
		base.PaintCheckBoxTitle(g, rect, element);
		StiSortElementGdiPainter.PaintSortButton(g, rect, element);
	}

	public override void PaintEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
		StiIndicatorElement stiIndicatorElement = ((StiIndicatorElement)element).Clone() as StiIndicatorElement;
		stiIndicatorElement.GlyphColor = Color.FromArgb(240, 240, 240);
		List<StiIndicatorIteration> iterations = new List<StiIndicatorIteration>
		{
			new StiIndicatorIteration
			{
				Value = 0m,
				Series = StiLocalization.Get("Dashboard", "Blank")
			}
		};
		StiIndicatorVisual stiIndicatorVisual = new StiIndicatorVisual(stiIndicatorElement, iterations);
		stiIndicatorVisual.Draw(g, rect.ToRectangleF());
	}
}
