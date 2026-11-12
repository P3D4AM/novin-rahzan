using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Dashboard.Visuals.Cards;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiCardsElementGdiPainter : StiAtomElementGdiPainter
{
	public override void PaintEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
		StiCardsElement element2 = ((StiCardsElement)element).Clone() as StiCardsElement;
		StiCardsVisual stiCardsVisual = new StiCardsVisual(element2, null);
		stiCardsVisual.DrawEmptyData(g, rect.ToRectangleF());
	}
}
