using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Shape;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiShapeAtom : StiElementAtom
{
	private StiShape shape;

	protected StiShapeElement ShapeElement => base.Element as StiShapeElement;

	public override void Paint(Graphics g)
	{
		RectangleF rect = base.Location.ToRectangleF();
		rect.Inflate(-1f, -1f);
		GraphicsState val = g.Save();
		g.SmoothingMode = (SmoothingMode)2;
		g.InterpolationMode = (InterpolationMode)7;
		g.CompositingQuality = (CompositingQuality)2;
		shape.ShapeType.Paint(g, shape, rect, (float)StiElementScale.Factor((IStiElement)ShapeElement));
		g.Restore(val);
	}

	public override void Render()
	{
		shape = new StiShapeElementBuilder().Render(ShapeElement);
	}

	public StiShapeAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}
}
