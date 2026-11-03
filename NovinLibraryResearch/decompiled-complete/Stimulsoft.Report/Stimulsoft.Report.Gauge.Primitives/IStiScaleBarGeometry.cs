using System.Drawing;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Primitives;

public interface IStiScaleBarGeometry
{
	SizeF Size { get; }

	RectangleF RectGeometry { get; }

	PointF Center { get; }

	float Radius { get; }

	float Diameter { get; }

	void CheckRectGeometry(RectangleF rect);

	void DrawScaleGeometry(StiGaugeContextPainter context);

	float GetRestToLenght();

	StiGraphicsPathLinesGaugeGeom DrawGeometry(StiGaugeContextPainter context, float StartValue, float EndValue, float StartWidth, float EndWidth, float Offset, StiPlacement Placement, ref RectangleF rect, bool returnOnlyRect);
}
