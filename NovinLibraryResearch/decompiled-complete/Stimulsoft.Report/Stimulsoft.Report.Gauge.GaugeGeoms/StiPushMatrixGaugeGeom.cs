using System.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiPushMatrixGaugeGeom : StiGaugeGeom
{
	public float Angle { get; }

	public PointF CenterPoint { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.PushMatrix;

	public StiPushMatrixGaugeGeom(float angle, PointF centerPoint)
	{
		Angle = angle;
		CenterPoint = centerPoint;
	}
}
