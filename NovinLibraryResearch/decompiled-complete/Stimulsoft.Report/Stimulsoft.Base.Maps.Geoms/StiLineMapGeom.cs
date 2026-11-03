using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiLineMapGeom : StiMapGeom
{
	public double X { get; set; }

	public double Y { get; set; }

	public override StiMapGeomType GeomType => StiMapGeomType.Line;

	public override PointD GetLastPoint()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new PointD(X, Y);
	}

	public override string ToString()
	{
		return $"Line={X},{Y}";
	}
}
