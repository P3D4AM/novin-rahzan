using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visuals.Indicator;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiIndicatorAtom : StiElementAtom
{
	private StiIndicatorVisual visual;

	protected StiIndicatorElement IndicatorElement => base.Element as StiIndicatorElement;

	internal List<StiIndicatorIteration> Iterations { get; set; }

	public override void Paint(Graphics g)
	{
		if (Iterations != null && Iterations.Any())
		{
			visual.Draw(g, base.Location.ToRectangleF());
		}
	}

	public override void Render()
	{
		Iterations = new StiIndicatorElementBuilder().Render(IndicatorElement, DataTable)?.ToList();
		visual = new StiIndicatorVisual(IndicatorElement, Iterations.ToList());
	}

	public RectangleF GetContentRectangle(RectangleD rect)
	{
		if (Iterations == null || !Iterations.Any() || visual == null)
		{
			return rect.ToRectangleF();
		}
		return visual.GetContentRectangle(rect.ToRectangleF());
	}

	public StiIndicatorAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}
}
