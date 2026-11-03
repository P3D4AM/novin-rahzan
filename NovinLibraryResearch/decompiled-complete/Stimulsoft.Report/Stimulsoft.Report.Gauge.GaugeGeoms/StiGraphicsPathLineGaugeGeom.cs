using System.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiGraphicsPathLineGaugeGeom : StiGaugeGeom
{
	public PointF P1 { get; }

	public PointF P2 { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.GraphicsPathLine;

	public StiGraphicsPathLineGaugeGeom(PointF p1, PointF p2)
	{
		P1 = p1;
		P2 = p2;
	}
}
