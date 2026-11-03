using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiMoveToMapGeom : StiMapGeom
{
	public double X { get; set; }

	public double Y { get; set; }

	public override StiMapGeomType GeomType => StiMapGeomType.MoveTo;

	public override PointD GetLastPoint()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new PointD(X, Y);
	}

	public override string ToString()
	{
		return $"MoveTo={X},{Y}";
	}
}
