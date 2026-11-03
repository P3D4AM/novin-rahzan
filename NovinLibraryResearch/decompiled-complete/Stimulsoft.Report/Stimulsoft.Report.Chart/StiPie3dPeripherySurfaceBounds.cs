using System.Drawing;

namespace Stimulsoft.Report.Chart;

public struct StiPie3dPeripherySurfaceBounds
{
	public float StartAngle { get; }

	public float EndAngle { get; }

	public PointF StartPoint { get; }

	public PointF EndPoint { get; }

	public float RealStartAngle { get; }

	public float RealEndAngle { get; }

	public StiPie3dPeripherySurfaceBounds(float startAngle, float endAngle, PointF startPoint, PointF endPoint, float realStartAngle, float realEndAngle)
	{
		StartAngle = startAngle;
		EndAngle = endAngle;
		StartPoint = startPoint;
		EndPoint = endPoint;
		RealStartAngle = realStartAngle;
		RealEndAngle = realEndAngle;
	}
}
