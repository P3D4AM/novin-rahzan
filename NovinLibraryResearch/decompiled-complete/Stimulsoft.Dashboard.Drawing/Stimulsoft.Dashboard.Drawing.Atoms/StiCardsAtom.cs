using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Dashboard.Visuals.Cards;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiCardsAtom : StiElementAtom
{
	private StiCardsVisual visual;

	protected StiCardsElement CardsElement => base.Element as StiCardsElement;

	public override void Paint(Graphics g)
	{
		visual.Draw(g, base.Location.ToRectangleF());
	}

	public override void Render()
	{
		visual = new StiCardsVisual(CardsElement, DataTable);
	}

	public RectangleF GetContentRectangle(RectangleD rect)
	{
		return visual.GetContentRectangle(rect.ToRectangleF());
	}

	public StiCardsAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}
}
