using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Gauge.Painters;

public class StiGdiGaugeContextPainter : StiGaugeContextPainter, IStiAnimationContextPainter<StiGaugeGeom>
{
	private readonly Graphics g;

	private List<GraphicsState> states = new List<GraphicsState>();

	public StiAnimationEngine AnimationEngine { get; set; }

	public override SizeF MeasureString(string text, Font font)
	{
		return g.MeasureString(text, font);
	}

	public override void Render()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		g.TextRenderingHint = (TextRenderingHint)4;
		foreach (StiGaugeGeom geom in base.Geoms)
		{
			switch (geom.Type)
			{
			case StiGaugeGeomType.RoundedRectangle:
				DrawRoundedRectangle(geom as StiRoundedRectangleGaugeGeom);
				break;
			case StiGaugeGeomType.Rectangle:
				DrawRectangle(geom as StiRectangleGaugeGeom);
				break;
			case StiGaugeGeomType.GraphicsPath:
				DrawGraphicsPath(geom as StiGraphicsPathGaugeGeom);
				break;
			case StiGaugeGeomType.Pie:
				DrawPie(geom as StiPieGaugeGeom);
				break;
			case StiGaugeGeomType.Ellipse:
				DrawEllipse(geom as StiEllipseGaugeGeom);
				break;
			case StiGaugeGeomType.GraphicsArcGeometry:
				DrawGraphicsPathArcGeometry(geom as StiGraphicsArcGeometryGaugeGeom);
				break;
			case StiGaugeGeomType.PushMatrix:
				PushMatrix(geom as StiPushMatrixGaugeGeom);
				break;
			case StiGaugeGeomType.PopTranform:
				PopState();
				break;
			case StiGaugeGeomType.Text:
				DrawString(geom as StiTextGaugeGeom);
				break;
			case StiGaugeGeomType.RadialRange:
				RadialRange(geom as StiRadialRangeGaugeGeom);
				break;
			}
		}
		g.SmoothingMode = smoothingMode;
		g.TextRenderingHint = textRenderingHint;
	}

	private void DrawRoundedRectangle(StiRoundedRectangleGaugeGeom geom)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		GraphicsPath roundedPath = StiDrawingHelper.GetRoundedPath(geom.Rect, 1, geom.LeftTop, geom.RightTop, geom.RightBottom, geom.LeftBottom);
		if (geom.Background != null)
		{
			Brush brush = StiBrush.GetBrush(geom.Background, geom.Rect);
			try
			{
				g.FillPath(brush, roundedPath);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		if (geom.BorderBrush != null && geom.BorderWidth > 0f)
		{
			Pen val = new Pen(StiBrush.GetBrush(geom.BorderBrush, Rectangle.Empty), geom.BorderWidth);
			try
			{
				g.DrawPath(val, roundedPath);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		roundedPath.Dispose();
		roundedPath = null;
	}

	private void DrawRectangle(StiRectangleGaugeGeom geom)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		if (geom.Background != null)
		{
			Brush brush = StiBrush.GetBrush(geom.Background, geom.Rect);
			try
			{
				g.FillRectangle(brush, geom.Rect);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		if (geom.BorderBrush != null && geom.BorderWidth > 0f)
		{
			Pen val = new Pen(StiBrush.GetBrush(geom.BorderBrush, Rectangle.Empty), geom.BorderWidth);
			try
			{
				g.DrawRectangle(val, geom.Rect.X, geom.Rect.Y, geom.Rect.Width, geom.Rect.Height);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void DrawGraphicsPath(StiGraphicsPathGaugeGeom geom)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		foreach (StiGaugeGeom geom2 in geom.Geoms)
		{
			if (geom2.Type == StiGaugeGeomType.GraphicsPathArc)
			{
				StiGraphicsPathArcGaugeGeom stiGraphicsPathArcGaugeGeom = geom2 as StiGraphicsPathArcGaugeGeom;
				val.AddArc(stiGraphicsPathArcGaugeGeom.X, stiGraphicsPathArcGaugeGeom.Y, stiGraphicsPathArcGaugeGeom.Width, stiGraphicsPathArcGaugeGeom.Height, stiGraphicsPathArcGaugeGeom.StartAngle, stiGraphicsPathArcGaugeGeom.SweepAngle);
			}
			else if (geom2.Type == StiGaugeGeomType.GraphicsPathLines)
			{
				StiGraphicsPathLinesGaugeGeom stiGraphicsPathLinesGaugeGeom = geom2 as StiGraphicsPathLinesGaugeGeom;
				PointF[] array = stiGraphicsPathLinesGaugeGeom.Points;
				StiAnimation animation = geom.Animation;
				if (!(animation is StiTranslationAnimation animation2))
				{
					if (!(animation is StiScaleAnimation animation3))
					{
						if (animation is StiRotationAnimation animation4)
						{
							array = StiAnimationEngine.GetAnimationRotation(this, array, animation4);
						}
					}
					else
					{
						array = StiAnimationEngine.GetAnimationScale(this, array, animation3);
					}
				}
				else
				{
					array = StiAnimationEngine.GetAnimationTranslation(this, array, animation2);
				}
				val.AddLines(array);
			}
			else if (geom2.Type == StiGaugeGeomType.GraphicsPathLine)
			{
				StiGraphicsPathLineGaugeGeom stiGraphicsPathLineGaugeGeom = geom2 as StiGraphicsPathLineGaugeGeom;
				val.AddLine(stiGraphicsPathLineGaugeGeom.P1, stiGraphicsPathLineGaugeGeom.P2);
			}
			else if (geom2.Type == StiGaugeGeomType.GraphicsPathCloseFigure)
			{
				val.CloseFigure();
			}
		}
		if (geom.Background != null)
		{
			Brush brush = StiBrush.GetBrush(geom.Background, geom.Rect);
			try
			{
				g.FillPath(brush, val);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		if (geom.BorderBrush != null && geom.BorderWidth > 0f)
		{
			Pen val2 = new Pen(StiBrush.GetBrush(geom.BorderBrush, Rectangle.Empty), geom.BorderWidth);
			try
			{
				g.DrawPath(val2, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		val.Dispose();
		val = null;
	}

	private void DrawPie(StiPieGaugeGeom geom)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		if (geom.Background != null)
		{
			Brush brush = StiBrush.GetBrush(geom.Background, geom.Rect);
			try
			{
				g.FillPie(brush, geom.Rect.X, geom.Rect.Y, geom.Rect.Width, geom.Rect.Height, geom.StartAngle, geom.SweepAngle);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		if (geom.BorderBrush != null && geom.BorderWidth > 0f)
		{
			Pen val = new Pen(StiBrush.GetBrush(geom.BorderBrush, Rectangle.Empty), geom.BorderWidth);
			try
			{
				g.DrawPie(val, geom.Rect, geom.StartAngle, geom.SweepAngle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void DrawEllipse(StiEllipseGaugeGeom geom)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		if (geom.Background != null)
		{
			Brush brush = StiBrush.GetBrush(geom.Background, geom.Rect);
			try
			{
				g.FillEllipse(brush, geom.Rect);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		if (geom.BorderBrush != null && geom.BorderWidth > 0f)
		{
			Pen val = new Pen(StiBrush.GetBrush(geom.BorderBrush, Rectangle.Empty), geom.BorderWidth);
			try
			{
				g.DrawEllipse(val, geom.Rect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void DrawGraphicsPathArcGeometry(StiGraphicsArcGeometryGaugeGeom geom)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		GraphicsPath arcGeometry = StiDrawingHelper.GetArcGeometry(geom.Rect, geom.StartAngle, geom.SweepAngle, geom.StartWidth, geom.EndWidth);
		if (arcGeometry == null)
		{
			return;
		}
		if (geom.Background != null)
		{
			Brush brush = StiBrush.GetBrush(geom.Background, geom.Rect);
			try
			{
				g.FillPath(brush, arcGeometry);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		if (geom.BorderBrush != null)
		{
			Pen val = new Pen(StiBrush.GetBrush(geom.BorderBrush, Rectangle.Empty), geom.BorderWidth);
			try
			{
				g.DrawPath(val, arcGeometry);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void DrawString(StiTextGaugeGeom geom)
	{
		Brush brush = StiBrush.GetBrush(geom.Foreground, geom.Rect);
		try
		{
			g.DrawString(geom.Text, geom.Font, brush, geom.Rect, geom.StringFormat);
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
	}

	private void RadialRange(StiRadialRangeGaugeGeom geom)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		GraphicsPath radialRangeGeometry = StiDrawingHelper.GetRadialRangeGeometry(geom.CenterPoint, geom.StartAngle, geom.SweepAngle, geom.Radius1, geom.Radius2, geom.Radius3, geom.Radius4);
		if (radialRangeGeometry == null)
		{
			return;
		}
		if (geom.Background != null)
		{
			Brush brush = StiBrush.GetBrush(geom.Background, geom.Rect);
			try
			{
				g.FillPath(brush, radialRangeGeometry);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		if (geom.BorderBrush != null && geom.BorderWidth > 0f)
		{
			Pen val = new Pen(StiBrush.GetBrush(geom.BorderBrush, Rectangle.Empty), geom.BorderWidth);
			try
			{
				g.DrawPath(val, radialRangeGeometry);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void PushMatrix(StiPushMatrixGaugeGeom geom)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		PushState();
		Matrix val = new Matrix();
		val.RotateAt(geom.Angle, geom.CenterPoint);
		g.MultiplyTransform(val);
	}

	private void PushState()
	{
		states.Add(g.Save());
	}

	private void PopState()
	{
		g.Restore(states[states.Count - 1]);
		states.RemoveAt(states.Count - 1);
	}

	public StiGdiGaugeContextPainter(Graphics g, StiGauge gauge, RectangleF rect, float zoom)
		: base(gauge, rect, zoom)
	{
		this.g = g;
	}
}
