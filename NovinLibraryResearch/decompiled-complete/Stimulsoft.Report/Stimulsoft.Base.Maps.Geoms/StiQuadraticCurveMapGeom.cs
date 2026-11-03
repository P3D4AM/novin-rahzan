using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiQuadraticCurveMapGeom : StiMapGeom
{
	public double StartX { get; set; }

	public double StartY { get; set; }

	public double ControlPointX { get; set; }

	public double ControlPointY { get; set; }

	public double EndX { get; set; }

	public double EndY { get; set; }

	private PointF FirstControlPoint
	{
		get
		{
			double num = StartX + (ControlPointX - StartX) * 2.0 / 3.0;
			double num2 = StartY + (ControlPointY - StartY) * 2.0 / 3.0;
			return new PointF((float)num, (float)num2);
		}
	}

	private PointF SecondControlPoint
	{
		get
		{
			double num = ControlPointX + (EndX - ControlPointX) / 3.0;
			double num2 = ControlPointY + (EndY - ControlPointY) / 3.0;
			return new PointF((float)num, (float)num2);
		}
	}

	public override StiMapGeomType GeomType => StiMapGeomType.QuadraticCurve;

	public void AddToPath(GraphicsPath graphicsPath)
	{
		graphicsPath.AddBezier(new PointF((float)StartX, (float)StartY), FirstControlPoint, SecondControlPoint, new PointF((float)EndX, (float)EndY));
	}

	public override PointD GetLastPoint()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new PointD(EndX, EndY);
	}

	public override string ToString()
	{
		return $"QuadraticCurve={StartX},{StartX}";
	}
}
