using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visuals.Progress;
using Stimulsoft.Dashboard.Visuals.Progress.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiProgressAtom : StiElementAtom
{
	internal List<StiProgressIteration> Iterations { get; set; }

	protected StiProgressElement ProgressElement => base.Element as StiProgressElement;

	public override void Paint(Graphics g)
	{
		if (Iterations != null && Iterations.Any())
		{
			StiProgressVisual stiProgressVisual = StiProgressVisualCreator.CreateProgressVisual(ProgressElement, Iterations);
			stiProgressVisual.Draw(g, base.Location.ToRectangleF());
		}
	}

	public override void Render()
	{
		Iterations = new StiProgressElementBuilder().Render(ProgressElement, DataTable)?.ToList();
	}

	public RectangleF GetContentRectangle(RectangleD rect, IStiElement element)
	{
		if (DataTable == null)
		{
			return rect.ToRectangleF();
		}
		IEnumerable<StiProgressIteration> enumerable = new StiProgressElementBuilder().Render(ProgressElement, DataTable);
		if (enumerable == null || !enumerable.Any())
		{
			return rect.ToRectangleF();
		}
		List<RectangleD> locationRectangles = StiElementLocationHelper.GetLocationRectangles(rect, enumerable.Count(), element);
		rect = StiElementLocationHelper.GetSumRectangle(locationRectangles);
		return rect.ToRectangleF();
	}

	public StiProgressAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}
}
