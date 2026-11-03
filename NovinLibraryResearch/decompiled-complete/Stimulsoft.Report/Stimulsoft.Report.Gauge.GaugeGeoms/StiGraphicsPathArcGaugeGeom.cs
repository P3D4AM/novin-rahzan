namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiGraphicsPathArcGaugeGeom : StiGaugeGeom
{
	public float X { get; }

	public float Y { get; }

	public float Width { get; }

	public float Height { get; }

	public float StartAngle { get; }

	public float SweepAngle { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.GraphicsPathArc;

	public StiGraphicsPathArcGaugeGeom(float x, float y, float width, float height, float startAngle, float sweepAngle)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
	}
}
