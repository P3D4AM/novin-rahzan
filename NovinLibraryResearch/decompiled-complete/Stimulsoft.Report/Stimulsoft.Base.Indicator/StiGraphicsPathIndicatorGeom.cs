using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Indicator;

public class StiGraphicsPathIndicatorGeom : StiIndicatorGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public PointF StartPoint { get; }

	public List<StiIndicatorGeom> Geoms { get; }

	public override StiIndicatorGeomType Type => StiIndicatorGeomType.GraphicsPath;

	public void AddGraphicsPathCloseFigureGaugeGeom()
	{
		Geoms.Add(new StiGraphicsPathCloseFigureIndicatorGeom());
	}

	public void AddGraphicsPathLinesGaugeGeom(PointF[] points)
	{
		Geoms.Add(new StiGraphicsPathLinesIndicatorGeom(points));
	}

	public StiGraphicsPathIndicatorGeom(RectangleF rect, PointF startPoint, StiBrush background, StiBrush borderBrush, float borderWidth)
	{
		Rect = rect;
		StartPoint = startPoint;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
		StartPoint = startPoint;
	}
}
