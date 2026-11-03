using System.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiGraphicsPathLinesGaugeGeom : StiGaugeGeom
{
	public PointF[] Points { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.GraphicsPathLines;

	public StiGraphicsPathLinesGaugeGeom(PointF[] points)
	{
		Points = points;
	}
}
