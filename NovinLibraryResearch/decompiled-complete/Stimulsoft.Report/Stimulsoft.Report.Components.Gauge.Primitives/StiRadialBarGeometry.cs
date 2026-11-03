using System;
using System.Drawing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

internal sealed class StiRadialBarGeometry : IStiScaleBarGeometry
{
	private readonly StiRadialScale scale;

	private SizeF size;

	private RectangleF rectGeometry;

	private PointF center;

	public SizeF Size => size;

	public RectangleF RectGeometry => rectGeometry;

	public float Radius { get; private set; }

	public float Diameter { get; private set; }

	public PointF Center => center;

	public void CheckRectGeometry(RectangleF rect)
	{
		size = rect.Size;
		PointF pointF = scale.Center;
		pointF = new PointF(rect.X + size.Width * pointF.X, rect.Y + size.Height * pointF.Y);
		float num;
		float num2;
		if (scale.RadiusMode == StiRadiusMode.Auto)
		{
			num = Math.Min(size.Width, size.Height) * scale.GetRadius();
			num2 = num;
			Diameter = num;
			Radius = num / 2f;
		}
		else if (scale.RadiusMode == StiRadiusMode.Width)
		{
			num = size.Width * scale.GetRadius();
			num2 = size.Height;
			Diameter = num;
			Radius = num / 2f;
		}
		else
		{
			num = size.Width;
			num2 = (Diameter = size.Height * scale.GetRadius());
			Radius = num2 / 2f;
		}
		rectGeometry = new RectangleF(pointF.X - num / 2f, pointF.Y - num2 / 2f, num, num2);
		center = new PointF(rectGeometry.Left + rectGeometry.Width / 2f, rectGeometry.Top + rectGeometry.Height / 2f);
	}

	public void DrawScaleGeometry(StiGaugeContextPainter context)
	{
		float startWidth;
		float endWidth;
		if (scale.IsReversed)
		{
			startWidth = scale.GetEndWidth();
			endWidth = scale.GetStartWidth();
		}
		else
		{
			startWidth = scale.GetStartWidth();
			endWidth = scale.GetEndWidth();
		}
		if (rectGeometry.Width > 0f && rectGeometry.Height > 0f)
		{
			context.AddGraphicsArcGeometryGaugeGeom(rectGeometry, scale.Brush, scale.BorderBrush, 1f, scale.StartAngle, scale.GetSweepAngle(), startWidth, endWidth);
		}
	}

	public float GetRestToLenght()
	{
		throw new NotImplementedException();
	}

	public StiGraphicsPathLinesGaugeGeom DrawGeometry(StiGaugeContextPainter context, float StartValue, float EndValue, float StartWidth, float EndWidth, float Offset, StiPlacement Placement, ref RectangleF rect, bool returnOnlyRect)
	{
		throw new NotImplementedException();
	}

	public StiRadialBarGeometry(StiRadialScale scale)
	{
		size = new SizeF(0f, 0f);
		center = new PointF(0f, 0f);
		rectGeometry = RectangleF.Empty;
		Radius = 0f;
		Diameter = 0f;
		this.scale = scale;
	}
}
