using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context;

public class StiContext
{
	public StiContextPainter ContextPainter { get; }

	public StiContextOptions Options { get; }

	public List<StiGeom> Geoms { get; } = new List<StiGeom>();


	public List<StiAnimation> Animations { get; }

	public void Render(RectangleF rect)
	{
		ContextPainter.Render(rect, Geoms);
	}

	public StiStringFormatGeom GetDefaultStringFormat()
	{
		return ContextPainter.GetDefaultStringFormat();
	}

	public StiStringFormatGeom GetGenericStringFormat()
	{
		return ContextPainter.GetGenericStringFormat();
	}

	public void DrawImage(byte[] image, RectangleF rect)
	{
		Geoms.Add(new StiImageGeom(rect, image));
	}

	public StiTextGeom DrawString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf)
	{
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, isRotatedText: false);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, int elementIndex = -1)
	{
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, isRotatedText: false, elementIndex);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, bool antialiasing, string toolTip)
	{
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, 0f, antialiasing, null, null, isRotatedText: false, toolTip);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf, float angle, bool antialiasing)
	{
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), sf, angle, antialiasing, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, float angle, bool antialiasing)
	{
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, PointF pos, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool antialiasing)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, pos, sf, angle, antialiasing, mode, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool antialiasing)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, mode, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool antialiasing, int maximalWidth)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, maximalWidth, mode, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool antialiasing, int maximalWidth)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, maximalWidth, mode, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool antialiasing)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, mode, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, PointF pos, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool antialiasing, int maximalWidth)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, pos, sf, angle, antialiasing, maximalWidth, mode, isRotatedText: true);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public SizeF MeasureString(string text, StiFontGeom font)
	{
		return ContextPainter.MeasureString(text, font);
	}

	public SizeF MeasureString(string text, StiFontGeom font, int width, StiStringFormatGeom sf)
	{
		return ContextPainter.MeasureString(text, font, width, sf);
	}

	public RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, float angle)
	{
		return ContextPainter.MeasureRotatedString(text, font, rect, sf, angle);
	}

	public RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode, float angle)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return ContextPainter.MeasureRotatedString(text, font, rect, sf, mode, angle);
	}

	public RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle, int maximalWidth)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return ContextPainter.MeasureRotatedString(text, font, point, sf, mode, angle, maximalWidth);
	}

	public RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return ContextPainter.MeasureRotatedString(text, font, point, sf, mode, angle);
	}

	public void DrawShadow(StiContext sg, RectangleF rect, float radius)
	{
		Geoms.Add(new StiShadowGeom(sg, rect, radius));
	}

	public void DrawCachedShadow(RectangleF rect, StiShadowSides sides, bool isPrinting)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Geoms.Add(new StiCachedShadowGeom(rect, sides, isPrinting));
	}

	public void DrawCachedShadow(RectangleF rect, StiShadowSides sides, bool isPrinting, RectangleF clipRect)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Geoms.Add(new StiCachedShadowGeom(rect, sides, isPrinting, clipRect));
	}

	public StiContext CreateShadowGraphics()
	{
		return ContextPainter.CreateShadowGraphics(Options.IsPrinting, Options.Zoom);
	}

	public void PushTranslateTransform(float x, float y)
	{
		Geoms.Add(new StiPushTranslateTransformGeom(x, y));
	}

	public void PushRotateTransform(float angle)
	{
		Geoms.Add(new StiPushRotateTransformGeom(angle));
	}

	public void PopTransform()
	{
		Geoms.Add(new StiPopTransformGeom());
	}

	public void PushClipPath(List<StiSegmentGeom> listGeoms)
	{
		Geoms.Add(new StiPushClipPathGeom(listGeoms));
	}

	public void PushClip(RectangleF clipRect)
	{
		Geoms.Add(new StiPushClipGeom(clipRect));
	}

	public void PopClip()
	{
		Geoms.Add(new StiPopClipGeom());
	}

	public StiTextGeom DrawAnimationText(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool antialiasing, int maximalWidth, StiAnimation animation)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Animations.Add(animation);
		StiTextGeom stiTextGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, maximalWidth, mode, isRotatedText: true, null, animation);
		Geoms.Add(stiTextGeom);
		return stiTextGeom;
	}

	public void DrawAnimationColumn(StiBrush wpfBackColor, object brush, StiPenGeom borderPen, object rect, double? value, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiClusteredColumnSeriesAnimationGeom(wpfBackColor, brush, borderPen, rect, value, toolTip, tag, animation, interaction));
	}

	public void DrawAnimationColumn(object brush, StiPenGeom borderPen, object rect, double? value, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiClusteredColumnSeriesAnimationGeom(brush, borderPen, rect, value, toolTip, tag, animation, interaction));
	}

	public void DrawAnimationBar(StiBrush wpfBackColor, object brush, StiPenGeom borderPen, object columnRect, double? value, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiClusteredBarSeriesAnimationGeom(wpfBackColor, brush, borderPen, columnRect, value, toolTip, tag, animation, interaction));
	}

	public void DrawAnimationBar(object brush, StiPenGeom borderPen, object columnRect, double? value, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiClusteredBarSeriesAnimationGeom(brush, borderPen, columnRect, value, toolTip, tag, animation, interaction));
	}

	public void DrawAnimationRectangle(StiBrush wpfBackColor, object brush, StiPenGeom pen, RectangleF rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null, string toolTip = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiBorderAnimationGeom(wpfBackColor, brush, pen, rect, tag, animation, interaction, toolTip));
	}

	public void DrawAnimationRectangle(object brush, StiPenGeom pen, RectangleF rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null, string toolTip = null)
	{
		Geoms.Add(new StiBorderAnimationGeom(brush, pen, rect, tag, animation, interaction, toolTip));
	}

	public void DrawAnimationPathElement(StiBrush wpfBackColor, object brush, StiPenGeom borderPen, List<StiSegmentGeom> path, object rect, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiPathElementAnimationGeom(wpfBackColor, brush, borderPen, path, rect, toolTip, tag, animation, interaction));
	}

	public void DrawAnimationPathElement(object brush, StiPenGeom borderPen, List<StiSegmentGeom> path, object rect, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiPathElementAnimationGeom(brush, borderPen, path, rect, toolTip, tag, animation, interaction));
	}

	public void DrawAnimationLabel(string text, StiFontGeom font, object textBrush, object labelBrush, StiPenGeom penBorder, Rectangle rect, StiStringFormatGeom sf, StiRotationMode mode, float angle, bool drawBorder, StiAnimation animation)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Geoms.Add(new StiLabelAnimationGeom(text, font, textBrush, labelBrush, penBorder, rect, sf, mode, angle, drawBorder, animation));
	}

	public void DrawAnimationLines(StiPenGeom pen, PointF[] points, StiAnimation animation)
	{
		Animations.Add(animation);
		Geoms.Add(new StiLinesAnimationGeom(pen, points, animation));
	}

	public void DrawAnimationCurve(StiPenGeom pen, PointF[] points, float tension, StiAnimation animation)
	{
		Animations.Add(animation);
		Geoms.Add(new StiCurveAnimationGeom(pen, points, tension, animation));
	}

	public void FillDrawAnimationPath(StiBrush wpfBackColor, object brush, StiPenGeom pen, List<StiSegmentGeom> path, object rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiPathAnimationGeom(wpfBackColor, brush, pen, path, rect, tag, animation, interaction));
	}

	public void FillDrawAnimationPath(object brush, StiPenGeom pen, List<StiSegmentGeom> path, object rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animations.Add(animation);
		Geoms.Add(new StiPathAnimationGeom(brush, pen, path, rect, tag, animation, interaction));
	}

	public void FillDrawAnimationEllipse(StiBrush wpfBackColor, object brush, StiPenGeom pen, float x, float y, float width, float height, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Geoms.Add(new StiEllipseAnimationGeom(wpfBackColor, brush, pen, new RectangleF(x, y, width, height), toolTip, tag, animation, interaction));
	}

	public void FillDrawAnimationEllipse(object brush, StiPenGeom pen, float x, float y, float width, float height, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Geoms.Add(new StiEllipseAnimationGeom(brush, pen, new RectangleF(x, y, width, height), toolTip, tag, animation, interaction));
	}

	public void DrawLine(StiPenGeom pen, float x1, float y1, float x2, float y2)
	{
		Geoms.Add(new StiLineGeom(pen, x1, y1, x2, y2));
	}

	public void DrawLines(StiPenGeom pen, PointF[] points)
	{
		for (int i = 0; i < points.Length; i++)
		{
			PointF pointF = points[i];
			if (double.IsNaN(pointF.X))
			{
				pointF.X = 0f;
			}
			if (double.IsNaN(pointF.Y))
			{
				pointF.Y = 0f;
			}
			points[i] = pointF;
		}
		Geoms.Add(new StiLinesGeom(pen, points));
	}

	public void DrawRectangle(StiPenGeom pen, Rectangle rect)
	{
		Geoms.Add(new StiBorderGeom(null, null, pen, rect, null, -1));
	}

	public void DrawRectangle(StiPenGeom pen, RectangleF rect)
	{
		Geoms.Add(new StiBorderGeom(null, null, pen, rect, null, -1));
	}

	public void DrawRectangle(StiPenGeom pen, float x, float y, float width, float height)
	{
		Geoms.Add(new StiBorderGeom(null, null, pen, new RectangleF(x, y, width, height), null, -1));
	}

	public void DrawEllipse(StiPenGeom pen, float x, float y, float width, float height)
	{
		Geoms.Add(new StiEllipseGeom(null, pen, new RectangleF(x, y, width, height), null, -1));
	}

	public void DrawEllipse(StiPenGeom pen, RectangleF rect)
	{
		Geoms.Add(new StiEllipseGeom(null, pen, rect, null, -1));
	}

	public void FillEllipse(object brush, float x, float y, float width, float height, StiInteractionDataGeom interaction = null, int elementIndex = -1)
	{
		Geoms.Add(new StiEllipseGeom(brush, null, new RectangleF(x, y, width, height), interaction, elementIndex));
	}

	public void FillEllipse(object brush, float x, float y, float width, float height, string toolTip, StiInteractionDataGeom interaction = null, int elementIndex = -1)
	{
		Geoms.Add(new StiEllipseGeom(brush, null, new RectangleF(x, y, width, height), interaction, elementIndex, toolTip));
	}

	public void FillEllipse(object brush, RectangleF rect, StiInteractionDataGeom interaction = null, int elementIndex = -1)
	{
		Geoms.Add(new StiEllipseGeom(brush, null, rect, interaction, elementIndex));
	}

	public void DrawPath(StiPenGeom pen, List<StiSegmentGeom> path, object rect)
	{
		Geoms.Add(new StiPathGeom(null, pen, path, rect, null, -1));
	}

	public void FillPath(object brush, List<StiSegmentGeom> path, object rect, StiInteractionDataGeom interaction = null, int elementIndex = -1, string toolTip = null)
	{
		Geoms.Add(new StiPathGeom(brush, null, path, rect, interaction, elementIndex, toolTip));
	}

	public void DrawCurve(StiPenGeom pen, PointF[] points, float tension)
	{
		Geoms.Add(new StiCurveGeom(pen, points, tension));
	}

	public void FillRectangle(object brush, object brushMouseOver, Rectangle rect, StiInteractionDataGeom interaction = null, int elementIndex = -1)
	{
		Geoms.Add(new StiBorderGeom(brush, brushMouseOver, null, rect, interaction, elementIndex));
	}

	public void FillRectangle(object brush, Rectangle rect, StiInteractionDataGeom interaction = null, int elementIndex = -1)
	{
		Geoms.Add(new StiBorderGeom(brush, null, null, rect, interaction, elementIndex));
	}

	public void FillRectangle(object brush, float x, float y, float width, float height, StiInteractionDataGeom interaction = null, int elementIndex = -1)
	{
		Geoms.Add(new StiBorderGeom(brush, null, null, new RectangleF(x, y, width, height), interaction, elementIndex));
	}

	public void PushSmoothingModeToAntiAlias()
	{
		Geoms.Add(new StiPushSmothingModeToAntiAliasGeom());
	}

	public void PopSmoothingMode()
	{
		Geoms.Add(new StiPopSmothingModeGeom());
	}

	public void PushTextRenderingHintToAntiAlias()
	{
		Geoms.Add(new StiPushTextRenderingHintToAntiAliasGeom());
	}

	public void PopTextRenderingHint()
	{
		Geoms.Add(new StiPopTextRenderingHintGeom());
	}

	public RectangleF GetPathBounds(List<StiSegmentGeom> geoms)
	{
		return ContextPainter.GetPathBounds(geoms);
	}

	public void DrawShadowRect(RectangleF rect, int shadowWidth, StiAnimation animation)
	{
		Geoms.Add(new StiShadowAnimationGeom(rect, shadowWidth, animation));
	}

	public void DrawShadowRect(RectangleF rect, double radiusX, double radiusY, int shadowWidth, StiAnimation animation)
	{
		Geoms.Add(new StiShadowAnimationGeom(rect, radiusX, radiusY, shadowWidth, animation));
	}

	public StiContext(StiContextPainter contextPainter, bool isGdi, bool isWpf, bool isPrinting, float zoom)
	{
		ContextPainter = contextPainter;
		Options = new StiContextOptions(isGdi, isWpf, isPrinting, zoom);
		Animations = new List<StiAnimation>();
	}
}
