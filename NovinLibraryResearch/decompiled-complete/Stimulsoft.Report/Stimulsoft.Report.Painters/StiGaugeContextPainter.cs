using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.GaugeGeoms;

namespace Stimulsoft.Report.Painters;

public abstract class StiGaugeContextPainter
{
	public float Zoom { get; set; }

	public RectangleF Rect { get; set; }

	public StiGauge Gauge { get; set; }

	public List<StiGaugeGeom> Geoms { get; set; } = new List<StiGaugeGeom>();


	public List<StiAnimation> Animations { get; }

	public static Font ChangeFontSize(Font font, float zoom)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		float num = font.Size * zoom;
		if (num < 1f)
		{
			num = 1f;
		}
		return new Font(font.FontFamily.Name, num, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public void AddPieGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, float startAngle, float sweepAngle)
	{
		if (rect.Width > 0f && rect.Height > 0f)
		{
			Geoms.Add(new StiPieGaugeGeom(rect, background, borderBrush, borderWidth, startAngle, sweepAngle));
		}
	}

	public void AddEllipseGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth)
	{
		if (rect.Width > 0f && rect.Height > 0f)
		{
			Geoms.Add(new StiEllipseGaugeGeom(rect, background, borderBrush, borderWidth));
		}
	}

	public void AddGraphicsArcGeometryGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, float startAngle, float sweepAngle, float startWidth, float endWidth)
	{
		if (rect.Width > 0f && rect.Height > 0f)
		{
			Geoms.Add(new StiGraphicsArcGeometryGaugeGeom(rect, background, borderBrush, borderWidth, startAngle, sweepAngle, startWidth, endWidth));
		}
	}

	public void AddPopTranformGaugeGeom()
	{
		Geoms.Add(new StiPopTranformGaugeGeom());
	}

	public void AddPushMatrixGaugeGeom(float angle, PointF centerPoint)
	{
		Geoms.Add(new StiPushMatrixGaugeGeom(angle, centerPoint));
	}

	public void AddRadialRangeGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, PointF centerPoint, float startAngle, float sweepAngle, float radius1, float radius2, float radius3, float radius4)
	{
		if (rect.Width > 0f && rect.Height > 0f)
		{
			Geoms.Add(new StiRadialRangeGaugeGeom(rect, background, borderBrush, borderWidth, centerPoint, startAngle, sweepAngle, radius1, radius2, radius3, radius4));
		}
	}

	public void AddRectangleGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth)
	{
		if (rect.Width > 0f && rect.Height > 0f)
		{
			Geoms.Add(new StiRectangleGaugeGeom(rect, background, borderBrush, borderWidth));
		}
	}

	public void AddRoundedRectangleGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, int leftTop, int rightTop, int rightBottom, int leftBottom)
	{
		if (rect.Width > 0f && rect.Height > 0f)
		{
			Geoms.Add(new StiRoundedRectangleGaugeGeom(rect, background, borderBrush, borderWidth, leftTop, rightTop, rightBottom, leftBottom));
		}
	}

	public void AddTextGaugeGeom(string text, Font font, StiBrush foreground, RectangleF rect, StringFormat sf)
	{
		if (rect.Width > 0f && rect.Height > 0f)
		{
			Geoms.Add(new StiTextGaugeGeom(text, font, foreground, rect, sf));
		}
	}

	public void AddGraphicsPathGaugeGeom(StiGraphicsPathGaugeGeom geom)
	{
		if (geom.Rect.Width > 0f && geom.Rect.Height > 0f)
		{
			Animations.Add(geom.Animation);
			Geoms.Add(geom);
		}
	}

	public abstract void Render();

	public abstract SizeF MeasureString(string text, Font font);

	public StiGaugeContextPainter(StiGauge gauge, RectangleF rect, float zoom)
	{
		Gauge = gauge;
		Rect = rect;
		Zoom = zoom;
		Animations = new List<StiAnimation>();
	}
}
