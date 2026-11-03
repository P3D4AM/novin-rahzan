using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiGraphicsPathGaugeGeom : StiGaugeGeom
{
	public RectangleF Rect { get; }

	public StiBrush Background { get; }

	public StiBrush BorderBrush { get; }

	public float BorderWidth { get; }

	public PointF StartPoint { get; }

	public List<StiGaugeGeom> Geoms { get; } = new List<StiGaugeGeom>();


	public override StiGaugeGeomType Type => StiGaugeGeomType.GraphicsPath;

	public void AddGraphicsPathArcGaugeGeom(float x, float y, float width, float height, float startAngle, float sweepAngle)
	{
		if (width > 0f && height > 0f)
		{
			Geoms.Add(new StiGraphicsPathArcGaugeGeom(x, y, width, height, startAngle, sweepAngle));
		}
	}

	public void AddGraphicsPathCloseFigureGaugeGeom()
	{
		Geoms.Add(new StiGraphicsPathCloseFigureGaugeGeom());
	}

	public void AddGraphicsPathLinesGaugeGeom(PointF[] points)
	{
		Geoms.Add(new StiGraphicsPathLinesGaugeGeom(points));
	}

	public void AddGraphicsPathLineGaugeGeom(PointF p1, PointF p2)
	{
		Geoms.Add(new StiGraphicsPathLineGaugeGeom(p1, p2));
	}

	public StiGraphicsPathGaugeGeom(RectangleF rect, PointF startPoint, StiBrush background, StiBrush borderBrush, float borderWidth)
	{
		Rect = rect;
		StartPoint = startPoint;
		Background = background;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
		StartPoint = startPoint;
	}
}
