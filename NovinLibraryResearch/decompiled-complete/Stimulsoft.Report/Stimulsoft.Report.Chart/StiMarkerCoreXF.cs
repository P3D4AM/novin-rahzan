using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiMarkerCoreXF : ICloneable
{
	private IStiMarker marker;

	public IStiMarker Marker
	{
		get
		{
			return marker;
		}
		set
		{
			marker = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void DrawMarkers(StiContext context, PointF[] points, bool showShadow)
	{
		if (points.Length != 0 && marker != null && marker.Visible && marker.Size > 0f)
		{
			context.PushSmoothingModeToAntiAlias();
			float zoom = context.Options.Zoom;
			foreach (PointF position in points)
			{
				marker.Core.Draw(context, marker, position, zoom, showShadow, isMouseOver: false, isTooltipMode: false, isAnimation: false, string.Empty, null, null);
			}
			context.PopSmoothingMode();
		}
	}

	public static RectangleF GetMarkerRect(PointF position, float markerSize, float zoom)
	{
		float num = markerSize * zoom / 2f;
		return new RectangleF(position.X - num, position.Y - num, num * 2f, num * 2f);
	}

	public void Draw(StiContext context, IStiMarker marker, PointF position, float zoom, bool showShadow, bool isMouseOver, bool isTooltipMode, bool isAnimation, string toolTip, object tag, StiInteractionDataGeom interaction)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		StiSolidBrush brush = new StiSolidBrush(Color.FromArgb(55, Color.Black));
		if (!(marker.Size < 0f))
		{
			RectangleF markerRect = GetMarkerRect(position, marker.Size, zoom);
			if (showShadow && !isTooltipMode)
			{
				markerRect.X += 2f * zoom;
				markerRect.Y += 2f * zoom;
				DrawPoint(context, markerRect.X + markerRect.Width / 2f, markerRect.Y + markerRect.Height / 2f, zoom, brush, null, marker.Type, marker.Icon, marker.Size, marker.Angle, isMouseOver: false, isAnimation, toolTip, null, null);
				markerRect.X -= 2f * zoom;
				markerRect.Y -= 2f * zoom;
			}
			StiBrush brush2 = marker.Brush;
			if (isAnimation && isTooltipMode && !marker.Visible)
			{
				Color color = ((marker.Brush == null) ? Color.LightGray : StiBrush.ToColor(marker.Brush));
				brush2 = (StiBrush)new StiSolidBrush(Color.FromArgb(0, color.R, color.G, color.B));
				isAnimation = false;
			}
			StiPenGeom pen = new StiPenGeom(marker.BorderColor);
			if (marker.Visible || isTooltipMode)
			{
				DrawPoint(context, markerRect.X + markerRect.Width / 2f, markerRect.Y + markerRect.Height / 2f, zoom, brush2, pen, marker.Type, marker.Icon, marker.Size, marker.Angle, isMouseOver, isAnimation, toolTip, tag, interaction);
			}
		}
	}

	public void DrawLine(StiContext context, float x1, float y1, float x2, float y2, float scale, StiBrush brushMarker, StiPenGeom penMarker, StiMarkerType markerType, float markerStep, float markerSize, float angle)
	{
		DrawLines(context, new PointF?[2]
		{
			new PointF(x1, y1),
			new PointF(x2, y2)
		}, scale, brushMarker, penMarker, markerType, markerStep, markerSize, angle);
	}

	public void DrawLines(StiContext context, PointF?[] points, float scale, object brushMarker, StiPenGeom penMarker, StiMarkerType markerType, float markerStep, float markerSize, float angle)
	{
		context.PushSmoothingModeToAntiAlias();
		if (points.Length < 2)
		{
			return;
		}
		float num = 0f;
		for (int i = 0; i < points.Length - 1; i++)
		{
			PointF? pointF = points[i];
			PointF? pointF2 = points[i + 1];
			if (pointF.HasValue && pointF2.HasValue)
			{
				float x = pointF.Value.X;
				float y = pointF.Value.Y;
				float x2 = pointF2.Value.X;
				float y2 = pointF2.Value.Y;
				float num2 = x2 - x;
				float num3 = y2 - y;
				float num4;
				for (num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3); num < num4; num += markerStep * scale)
				{
					float x3 = x + num2 * num / num4;
					float y3 = y + num3 * num / num4;
					DrawPoint(context, x3, y3, scale, brushMarker, penMarker, markerType, marker.Icon, markerSize, angle, isMouseOver: false, isAnimation: false, string.Empty, null, null);
				}
				num -= num4;
			}
		}
		context.PopSmoothingMode();
	}

	public void DrawPoint(StiContext context, float x, float y, float scale, object brush, StiPenGeom pen, StiMarkerType markerType, StiFontIcons? icon, float markerSize, float angle, bool isMouseOver, bool isAnimation, string toolTip, object tag, StiInteractionDataGeom interaction)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		float num = markerSize * scale / 2f;
		float num2 = x - num;
		float num3 = x + num;
		float num4 = y - num;
		float num5 = y + num;
		if (icon.HasValue)
		{
			RectangleF rect = new RectangleF(num2, num4, markerSize * scale, markerSize * scale);
			if (rect.Height > 0f && rect.Width > 0f)
			{
				StiFontIconsExHelper.DrawDirectionIcons(context, brush, rect, new SizeF(rect.Height, rect.Height), icon.GetValueOrDefault(), toolTip, verticalDirection: false);
			}
			return;
		}
		switch (markerType)
		{
		case StiMarkerType.Star5:
			DrawPolygon(context, brush, pen, x, y, num, 10, angle, isStar: true, isMouseOver, isAnimation, toolTip, tag, interaction);
			break;
		case StiMarkerType.Star6:
			DrawPolygon(context, brush, pen, x, y, num, 12, angle, isStar: true, isMouseOver, isAnimation, toolTip, tag, interaction);
			break;
		case StiMarkerType.Star7:
			DrawPolygon(context, brush, pen, x, y, num, 14, angle, isStar: true, isMouseOver, isAnimation, toolTip, tag, interaction);
			break;
		case StiMarkerType.Star8:
			DrawPolygon(context, brush, pen, x, y, num, 16, angle, isStar: true, isMouseOver, isAnimation, toolTip, tag, interaction);
			break;
		case StiMarkerType.Hexagon:
			DrawPolygon(context, brush, pen, x, y, num, 6, 30f + angle, isStar: false, isMouseOver, isAnimation, toolTip, tag, interaction);
			break;
		case StiMarkerType.Rectangle:
			DrawPolygon(context, brush, pen, x, y, num, 4, 45f + angle, isStar: false, isMouseOver, isAnimation, toolTip, tag, interaction);
			break;
		case StiMarkerType.Circle:
			if (isAnimation)
			{
				StiScaleAnimation animation2 = new StiScaleAnimation(StiChartHelper.GlobalDurationElement, TimeSpan.Zero);
				context.FillDrawAnimationEllipse(brush, pen, num2, num4, num3 - num2, num5 - num4, toolTip, tag, animation2, interaction);
				break;
			}
			if (brush != null)
			{
				context.FillEllipse(brush, num2, num4, num3 - num2, num5 - num4, toolTip, interaction);
			}
			if (isMouseOver)
			{
				context.FillEllipse(StiMouseOverHelper.GetMouseOverColor(), num2, num4, num3 - num2, num5 - num4, toolTip);
			}
			if (pen != null)
			{
				context.DrawEllipse(pen, num2, num4, num3 - num2, num5 - num4);
			}
			break;
		case StiMarkerType.HalfCircle:
		{
			RectangleF rectangleF = new RectangleF(num2, num4, num3 - num2, num5 - num4);
			RectangleF rectangleF2 = new RectangleF(0f, 0f, num3 - num2, num5 - num4);
			List<StiSegmentGeom> list = new List<StiSegmentGeom>();
			list.Add(new StiArcSegmentGeom(new RectangleF(rectangleF2.X, rectangleF2.Top + rectangleF2.Height / 4f, rectangleF2.Width, rectangleF2.Height), 360f, -180f));
			list.Add(new StiLineSegmentGeom(new PointF(rectangleF2.X, rectangleF2.Bottom - rectangleF2.Height / 4f), new PointF(rectangleF2.Right, rectangleF2.Bottom - rectangleF2.Height / 4f)));
			context.PushTranslateTransform(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
			context.PushRotateTransform(angle);
			context.PushTranslateTransform((0f - rectangleF.Width) / 2f, (0f - rectangleF.Height) / 2f);
			if (isAnimation)
			{
				StiScaleAnimation animation = new StiScaleAnimation(StiChartHelper.GlobalDurationElement, TimeSpan.Zero);
				context.FillDrawAnimationPath(brush, pen, list, rectangleF, tag, animation, interaction);
			}
			else
			{
				if (brush != null)
				{
					context.FillPath(brush, list, rectangleF2);
				}
				if (isMouseOver)
				{
					context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list, rectangleF);
				}
				if (pen != null)
				{
					context.DrawPath(pen, list, rectangleF2);
				}
			}
			context.PopTransform();
			context.PopTransform();
			context.PopTransform();
			break;
		}
		case StiMarkerType.Triangle:
			DrawPolygon(context, brush, pen, x, y, num, 3, angle, isStar: false, isMouseOver, isAnimation, toolTip, tag, interaction);
			break;
		}
	}

	private void DrawPolygon(StiContext context, object fillBrush, StiPenGeom borderPen, float centerX, float centerY, float radius, int count, float startAngle, bool isStar, bool isMouseOver, bool isAnimation, string toolTip, object tag, StiInteractionDataGeom interaction)
	{
		startAngle = 0f - startAngle;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		float num = -90f + startAngle;
		PointF[] array = new PointF[count + 1];
		if (isStar)
		{
			for (int i = 0; i < count; i += 2)
			{
				array[i].X = centerX + radius * (float)Math.Cos((double)num * Math.PI / 180.0);
				array[i].Y = centerY + radius * (float)Math.Sin((double)num * Math.PI / 180.0);
				num += 360f / (float)count;
				array[i + 1].X = centerX + radius / 2f * (float)Math.Cos((double)num * Math.PI / 180.0);
				array[i + 1].Y = centerY + radius / 2f * (float)Math.Sin((double)num * Math.PI / 180.0);
				num += 360f / (float)count;
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				array[j].X = centerX + radius * (float)Math.Cos((double)num * Math.PI / 180.0);
				array[j].Y = centerY + radius * (float)Math.Sin((double)num * Math.PI / 180.0);
				num += 360f / (float)count;
			}
		}
		array[count] = array[0];
		list.Add(new StiLinesSegmentGeom(array));
		if (isAnimation)
		{
			StiScaleAnimation animation = new StiScaleAnimation(StiChartHelper.GlobalDurationElement, TimeSpan.Zero);
			context.FillDrawAnimationPath(fillBrush, borderPen, list, StiPathGeom.GetBoundsState, tag, animation, interaction);
			return;
		}
		if (fillBrush != null)
		{
			context.FillPath(fillBrush, list, StiPathGeom.GetBoundsState, null, -1, toolTip);
		}
		if (isMouseOver)
		{
			context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list, StiPathGeom.GetBoundsState, null, -1, toolTip);
		}
		if (borderPen != null)
		{
			context.DrawPath(borderPen, list, null);
		}
	}

	public StiMarkerCoreXF(IStiMarker marker)
	{
		this.marker = marker;
	}
}
