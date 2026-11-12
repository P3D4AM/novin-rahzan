using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Visuals.Progress;
using Stimulsoft.Dashboard.Visuals.Progress.Helpers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiProgressElementGdiPainter : StiAtomElementGdiPainter
{
	public override void PaintCheckBoxTitle(Graphics g, RectangleD rect, IStiElement element)
	{
		base.PaintCheckBoxTitle(g, rect, element);
		StiSortElementGdiPainter.PaintSortButton(g, rect, element);
	}

	public override void PaintEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
		StiProgressElement element2 = ((StiProgressElement)element).Clone() as StiProgressElement;
		List<StiProgressIteration> iterations = new List<StiProgressIteration>
		{
			new StiProgressIteration
			{
				Value = 0m,
				Series = StiLocalization.Get("Dashboard", "Blank")
			}
		};
		StiProgressVisual stiProgressVisual = StiProgressVisualCreator.CreateProgressVisual(element2, iterations);
		stiProgressVisual.Draw(g, rect.ToRectangleF());
	}
}
