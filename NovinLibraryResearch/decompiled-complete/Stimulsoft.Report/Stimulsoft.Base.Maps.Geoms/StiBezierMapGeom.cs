using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiBezierMapGeom : StiMapGeom
{
	public double X1 { get; set; }

	public double Y1 { get; set; }

	public double X2 { get; set; }

	public double Y2 { get; set; }

	public double X3 { get; set; }

	public double Y3 { get; set; }

	public override StiMapGeomType GeomType => StiMapGeomType.Bezier;

	public override PointD GetLastPoint()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new PointD(X3, Y3);
	}

	public override string ToString()
	{
		return $"Bezier={X1},{Y1}, {X2},{Y2}, {X3},{Y3}";
	}
}
