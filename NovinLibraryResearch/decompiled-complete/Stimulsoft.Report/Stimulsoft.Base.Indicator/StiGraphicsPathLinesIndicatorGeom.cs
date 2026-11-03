using System.Drawing;

namespace Stimulsoft.Base.Indicator;

public class StiGraphicsPathLinesIndicatorGeom : StiIndicatorGeom
{
	public PointF[] Points { get; }

	public override StiIndicatorGeomType Type => StiIndicatorGeomType.GraphicsPathLines;

	public StiGraphicsPathLinesIndicatorGeom(PointF[] points)
	{
		Points = points;
	}
}
