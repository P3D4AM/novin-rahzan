using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Report.Painters;

public class StiGdiContextPainter : StiContextPainter, IStiAnimationContextPainter<StiGeom>
{
	private bool isClone;

	private object lockObject = new object();

	private Graphics graphics;

	private List<GraphicsState> states = new List<GraphicsState>();

	private List<SmoothingMode> smothingModes = new List<SmoothingMode>();

	private List<TextRenderingHint> textRenderingHints = new List<TextRenderingHint>();

	internal StiContext Context { get; set; }

	public List<StiGeom> Geoms => Context.Geoms;

	public StiAnimationEngine AnimationEngine { get; set; }

	public override StiContextPainter Clone()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		isClone = true;
		Graphics val = Graphics.FromImage((Image)new Bitmap(1, 1));
		val.PageUnit = (GraphicsUnit)2;
		val.PageScale = 1f;
		return new StiGdiContextPainter(val);
	}

	public override void Dispose()
	{
		if (isClone)
		{
			graphics.Dispose();
		}
	}

	public override StiStringFormatGeom GetDefaultStringFormat()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		StringFormat val = new StringFormat();
		try
		{
			return new StiStringFormatGeom(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override StiStringFormatGeom GetGenericStringFormat()
	{
		object obj = StringFormat.GenericDefault.Clone();
		StringFormat val = (StringFormat)((obj is StringFormat) ? obj : null);
		try
		{
			StiStringFormatGeom stiStringFormatGeom = new StiStringFormatGeom(val);
			stiStringFormatGeom.IsGeneric = true;
			return stiStringFormatGeom;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override StiContext CreateShadowGraphics(bool isPrinting, float zoom)
	{
		return new StiContext(new StiGdiContextPainter(graphics), isGdi: true, isWpf: false, isPrinting, zoom);
	}

	public override RectangleF GetPathBounds(List<StiSegmentGeom> geoms)
	{
		GraphicsPath path = GetPath(geoms);
		try
		{
			return path.GetBounds();
		}
		finally
		{
			((IDisposable)path)?.Dispose();
		}
	}

	public override SizeF MeasureString(string text, StiFontGeom font)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		text = CheckSurrogatesAndSupplementaryCharacters(text);
		lock (lockObject)
		{
			Font font2 = GetFont(font);
			try
			{
				if (NeedHtmlTags(text))
				{
					return MeasureHtmlTags(graphics, text, new RectangleD(0.0, 0.0, 2147483647.0, 2147483647.0), font2, null, 0f);
				}
				return graphics.MeasureString(text, font2);
			}
			finally
			{
				((IDisposable)font2)?.Dispose();
			}
		}
	}

	public override SizeF MeasureString(string text, StiFontGeom font, int width, StiStringFormatGeom sf)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		text = CheckSurrogatesAndSupplementaryCharacters(text);
		Font font2 = GetFont(font);
		try
		{
			StringFormat stringFormat = GetStringFormat(sf);
			try
			{
				if (NeedHtmlTags(text))
				{
					return MeasureHtmlTags(graphics, text, new RectangleD(0.0, 0.0, (double)width, 2147483647.0), font2, sf, 0f);
				}
				return graphics.MeasureString(text, font2, width, stringFormat);
			}
			finally
			{
				((IDisposable)stringFormat)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)font2)?.Dispose();
		}
	}

	public override RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, float angle)
	{
		Font font2 = GetFont(font);
		try
		{
			StringFormat stringFormat = GetStringFormat(sf);
			try
			{
				return StiRotatedTextDrawing.MeasureString(graphics, CheckSurrogatesAndSupplementaryCharacters(text), font2, rect, stringFormat, (StiRotationMode)4, angle);
			}
			finally
			{
				((IDisposable)stringFormat)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)font2)?.Dispose();
		}
	}

	public override RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode, float angle)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Font font2 = GetFont(font);
		try
		{
			StringFormat stringFormat = GetStringFormat(sf);
			try
			{
				return StiRotatedTextDrawing.MeasureString(graphics, CheckSurrogatesAndSupplementaryCharacters(text), font2, rect, stringFormat, mode, angle);
			}
			finally
			{
				((IDisposable)stringFormat)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)font2)?.Dispose();
		}
	}

	public override RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle, int maximalWidth)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Font font2 = GetFont(font);
		try
		{
			StringFormat stringFormat = GetStringFormat(sf);
			try
			{
				return StiRotatedTextDrawing.MeasureString(graphics, CheckSurrogatesAndSupplementaryCharacters(text), font2, point, stringFormat, mode, angle, maximalWidth);
			}
			finally
			{
				((IDisposable)stringFormat)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)font2)?.Dispose();
		}
	}

	public override RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Font font2 = GetFont(font);
		try
		{
			StringFormat stringFormat = GetStringFormat(sf);
			try
			{
				return StiRotatedTextDrawing.MeasureString(graphics, CheckSurrogatesAndSupplementaryCharacters(text), font2, point, stringFormat, mode, angle);
			}
			finally
			{
				((IDisposable)stringFormat)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)font2)?.Dispose();
		}
	}

	public override void Render(RectangleF rect, List<StiGeom> geoms)
	{
		StiAnimationEngine.CorrectAnimationTimes(this, geoms);
		foreach (StiGeom geom in geoms)
		{
			try
			{
				switch (geom.Type)
				{
				case StiGeomType.Image:
					DrawImageGeom(geom as StiImageGeom);
					break;
				case StiGeomType.Border:
					DrawBorderGeom(geom as StiBorderGeom);
					break;
				case StiGeomType.Line:
					DrawLineGeom(geom as StiLineGeom);
					break;
				case StiGeomType.Lines:
					DrawLinesGeom(geom as StiLinesGeom);
					break;
				case StiGeomType.Curve:
					DrawCurveGeom(geom as StiCurveGeom);
					break;
				case StiGeomType.Ellipse:
					DrawEllipseGeom(geom as StiEllipseGeom);
					break;
				case StiGeomType.CachedShadow:
					DrawCachedShadowGeom(geom as StiCachedShadowGeom);
					break;
				case StiGeomType.Shadow:
					DrawShadowGeom(geom as StiShadowGeom);
					break;
				case StiGeomType.Path:
					DrawPathGeom(geom as StiPathGeom);
					break;
				case StiGeomType.Text:
					DrawTextGeom(geom as StiTextGeom);
					break;
				case StiGeomType.PushClipPath:
					PushClipPathGeom(geom as StiPushClipPathGeom);
					break;
				case StiGeomType.PushClip:
					PushClipGeom(geom as StiPushClipGeom);
					break;
				case StiGeomType.PushTranslateTransform:
					PushTranslateTransformGeom(geom as StiPushTranslateTransformGeom);
					break;
				case StiGeomType.PushRotateTransform:
					PushRotateTransformGeom(geom as StiPushRotateTransformGeom);
					break;
				case StiGeomType.PushSmothingModeToAntiAlias:
					PushSmothingModeToAntiAliasGeom(geom as StiPushSmothingModeToAntiAliasGeom);
					break;
				case StiGeomType.PushTextRenderingHintToAntiAlias:
					PushTextRenderingHintToAntiAliasGeom(geom as StiPushTextRenderingHintToAntiAliasGeom);
					break;
				case StiGeomType.PopSmothingMode:
					PopSmothingModeGeom(geom as StiPopSmothingModeGeom);
					break;
				case StiGeomType.PopTextRenderingHint:
					PopTextRenderingHintGeom(geom as StiPopTextRenderingHintGeom);
					break;
				case StiGeomType.PopTransform:
					PopTransformGeom(geom as StiPopTransformGeom);
					break;
				case StiGeomType.PopClip:
					PopClipGeom(geom as StiPopClipGeom);
					break;
				case StiGeomType.AnimationBar:
					DrawAnimationBar(geom as StiClusteredBarSeriesAnimationGeom);
					break;
				case StiGeomType.AnimationBorder:
					DrawAnimationBorder(geom as StiBorderAnimationGeom);
					break;
				case StiGeomType.AnimationColumn:
					DrawAnimationColumn(geom as StiClusteredColumnSeriesAnimationGeom);
					break;
				case StiGeomType.AnimationEllipse:
					DrawAnimationEllipse(geom as StiEllipseAnimationGeom);
					break;
				case StiGeomType.AnimationPath:
					DrawAnimationPath(geom as StiPathAnimationGeom);
					break;
				case StiGeomType.AnimationPathElement:
					DrawAnimationPathElement(geom as StiPathElementAnimationGeom);
					break;
				case StiGeomType.AnimationLines:
					DrawAnimationLines(geom as StiLinesAnimationGeom);
					break;
				case StiGeomType.AnimationCurve:
					DrawAnimationCurve(geom as StiCurveAnimationGeom);
					break;
				case StiGeomType.AnimationLabel:
					DrawAnimationLabel(geom as StiLabelAnimationGeom);
					break;
				case StiGeomType.AnimationShadow:
					DrawAnimationShadow(geom as StiShadowAnimationGeom);
					break;
				case StiGeomType.Font:
				case StiGeomType.Pen:
				case StiGeomType.PushSmothingMode:
				case StiGeomType.PushTextRenderingHint:
				case StiGeomType.Segment:
				case StiGeomType.StringFormat:
					break;
				}
			}
			catch
			{
			}
		}
	}

	private void DrawImageGeom(StiImageGeom imageGeom)
	{
		Image imageFromObject = StiImageHelper.GetImageFromObject(imageGeom.Image);
		try
		{
			RectangleF rect = imageGeom.Rect;
			float num = rect.Width / (float)imageFromObject.Width;
			float num2 = rect.Height / (float)imageFromObject.Height;
			num = (num2 = ((num < num2) ? num : num2));
			float num3 = (float)imageFromObject.Width * num;
			float num4 = (float)imageFromObject.Height * num2;
			float x = rect.X + (rect.Width - num3) / 2f;
			float y = rect.Y + (rect.Height - num4) / 2f;
			rect = new RectangleF(x, y, num3, num4);
			graphics.DrawImage(imageFromObject, rect);
		}
		finally
		{
			((IDisposable)imageFromObject)?.Dispose();
		}
	}

	private void DrawBorderGeom(StiBorderGeom borderGeom)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		object rect = GetRect(borderGeom.Rect);
		RectangleF rectangleF = ((!(rect is RectangleF)) ? ((RectangleF)(Rectangle)rect) : ((RectangleF)rect));
		Brush brush = GetBrush(borderGeom.Background, rect);
		Pen pen = GetPen(borderGeom.BorderPen);
		PointF pointF = new PointF(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
		if (borderGeom.Angle != 0f)
		{
			graphics.TranslateTransform(pointF.X, pointF.Y);
			graphics.RotateTransform(borderGeom.Angle);
			PointF startPoint = StiRotatedTextDrawing.GetStartPoint(borderGeom.RotationMode, new RectangleF(0f, 0f, rectangleF.Width, rectangleF.Height));
			rectangleF.X = 0f - startPoint.X;
			rectangleF.Y = 0f - startPoint.Y;
		}
		if (brush != null)
		{
			graphics.FillRectangle(brush, rectangleF);
		}
		if (pen != null)
		{
			graphics.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		if (borderGeom.Angle != 0f)
		{
			graphics.RotateTransform(0f - borderGeom.Angle);
			graphics.TranslateTransform(0f - pointF.X, 0f - pointF.Y);
		}
		if (brush != null)
		{
			brush.Dispose();
		}
		if (pen != null)
		{
			pen.Dispose();
		}
	}

	private void DrawLineGeom(StiLineGeom lineGeom)
	{
		Pen pen = GetPen(lineGeom.Pen);
		if (pen != null)
		{
			graphics.DrawLine(pen, lineGeom.X1, lineGeom.Y1, lineGeom.X2, lineGeom.Y2);
			if (pen != null)
			{
				pen.Dispose();
			}
		}
	}

	private void DrawLinesGeom(StiLinesGeom linesGeom)
	{
		Pen pen = GetPen(linesGeom.Pen);
		if (pen != null)
		{
			graphics.DrawLines(pen, linesGeom.Points);
			if (pen != null)
			{
				pen.Dispose();
			}
		}
	}

	private void DrawCurveGeom(StiCurveGeom curveGeom)
	{
		Pen pen = GetPen(curveGeom.Pen);
		if (pen != null)
		{
			graphics.DrawCurve(pen, curveGeom.Points, curveGeom.Tension);
			if (pen != null)
			{
				pen.Dispose();
			}
		}
	}

	private void DrawEllipseGeom(StiEllipseGeom ellipseGeom)
	{
		object rect = GetRect(ellipseGeom.Rect);
		Brush brush = GetBrush(ellipseGeom.Background, rect);
		Pen pen = GetPen(ellipseGeom.BorderPen);
		if (rect is Rectangle)
		{
			if (brush != null)
			{
				graphics.FillEllipse(brush, (Rectangle)rect);
			}
			if (pen != null)
			{
				graphics.DrawEllipse(pen, (Rectangle)rect);
			}
		}
		else if (rect is RectangleF rectangleF)
		{
			if (brush != null)
			{
				graphics.FillEllipse(brush, rectangleF);
			}
			if (pen != null)
			{
				graphics.DrawEllipse(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
			}
		}
		if (brush != null)
		{
			brush.Dispose();
		}
		if (pen != null)
		{
			pen.Dispose();
		}
	}

	private void DrawCachedShadowGeom(StiCachedShadowGeom cachedShadowGeom)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		object rect = GetRect(cachedShadowGeom.Rect);
		if (rect is Rectangle rectangle)
		{
			StiShadow.DrawCachedShadow(graphics, (RectangleF)rectangle, cachedShadowGeom.Sides, cachedShadowGeom.IsPrinting);
		}
		else if (rect is RectangleF rectangleF)
		{
			StiShadow.DrawCachedShadow(graphics, rectangleF, cachedShadowGeom.Sides, cachedShadowGeom.IsPrinting);
		}
	}

	private void DrawShadowGeom(StiShadowGeom shadowGeom)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		StiShadowGraphics val = new StiShadowGraphics(shadowGeom.Rect);
		try
		{
			((StiGdiContextPainter)shadowGeom.ShadowContext.ContextPainter).graphics = val.Graphics;
			shadowGeom.ShadowContext.Render(shadowGeom.Rect);
			val.DrawShadow(graphics, shadowGeom.Rect, shadowGeom.Radius);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawPathGeom(StiPathGeom pathGeom)
	{
		GraphicsPath path = GetPath(pathGeom.Geoms);
		try
		{
			object rect = ((pathGeom.Rect == StiPathGeom.GetBoundsState) ? ((object)path.GetBounds()) : GetRect(pathGeom.Rect));
			Brush brush = GetBrush(pathGeom.Background, rect);
			Pen pen = GetPen(pathGeom.Pen);
			if (brush != null)
			{
				graphics.FillPath(brush, path);
			}
			if (pen != null)
			{
				graphics.DrawPath(pen, path);
			}
			if (brush != null)
			{
				brush.Dispose();
			}
			if (pen != null)
			{
				pen.Dispose();
			}
		}
		finally
		{
			((IDisposable)path)?.Dispose();
		}
	}

	private void DrawTextGeom(StiTextGeom textGeom)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		object obj = textGeom.Brush;
		if (textGeom.Animation is StiOpacityAnimation animation)
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation);
		}
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		if (textGeom.Antialiasing)
		{
			graphics.TextRenderingHint = (TextRenderingHint)4;
		}
		string text = CheckSurrogatesAndSupplementaryCharacters(textGeom.Text);
		if (!textGeom.IsRotatedText)
		{
			object rect = GetRect(textGeom.Location);
			if (rect is Rectangle rectangle)
			{
				Brush brush = GetBrush(obj, rectangle);
				try
				{
					Font font = GetFont(textGeom.Font);
					try
					{
						StringFormat stringFormat = GetStringFormat(textGeom.StringFormat);
						try
						{
							if (NeedHtmlTags(text))
							{
								DrawHtmlTags(graphics, textGeom, RectangleD.CreateFromRectangle(rectangle), font);
							}
							else
							{
								graphics.DrawString(text, font, brush, (RectangleF)rectangle, stringFormat);
							}
						}
						finally
						{
							((IDisposable)stringFormat)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)font)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)brush)?.Dispose();
				}
			}
			else if (rect is RectangleF rectangleF)
			{
				Brush brush2 = GetBrush(obj, rectangleF);
				try
				{
					Font font2 = GetFont(textGeom.Font);
					try
					{
						StringFormat stringFormat2 = GetStringFormat(textGeom.StringFormat);
						try
						{
							if (NeedHtmlTags(text))
							{
								DrawHtmlTags(graphics, textGeom, RectangleD.CreateFromRectangle(rectangleF), font2);
							}
							else
							{
								graphics.DrawString(text, font2, brush2, rectangleF, stringFormat2);
							}
						}
						finally
						{
							((IDisposable)stringFormat2)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)font2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)brush2)?.Dispose();
				}
			}
		}
		else
		{
			object rect2 = GetRect(textGeom.Location);
			if ((rect2 is RectangleF || rect2 is Rectangle) && textGeom.IsRounded)
			{
				Rectangle empty = Rectangle.Empty;
				empty = ((!(rect2 is Rectangle)) ? Rectangle.Round((RectangleF)rect2) : ((Rectangle)rect2));
				Brush brush3 = GetBrush(obj, empty);
				try
				{
					Font font3 = GetFont(textGeom.Font);
					try
					{
						StringFormat stringFormat3 = GetStringFormat(textGeom.StringFormat);
						try
						{
							if (NeedHtmlTags(text))
							{
								DrawHtmlTags(graphics, textGeom, RectangleD.CreateFromRectangle(empty), font3);
							}
							else
							{
								StiRotatedTextDrawing.DrawString(graphics, text, font3, brush3, (RectangleF)empty, stringFormat3, textGeom.RotationMode.GetValueOrDefault(), textGeom.Angle, textGeom.Antialiasing);
							}
						}
						finally
						{
							((IDisposable)stringFormat3)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)font3)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)brush3)?.Dispose();
				}
			}
			else if (rect2 is RectangleF || rect2 is Rectangle)
			{
				RectangleF rectangleF2 = ((!(rect2 is RectangleF)) ? ((RectangleF)(Rectangle)rect2) : ((RectangleF)rect2));
				Brush brush4 = GetBrush(obj, rectangleF2);
				try
				{
					Font font4 = GetFont(textGeom.Font);
					try
					{
						StringFormat stringFormat4 = GetStringFormat(textGeom.StringFormat);
						try
						{
							if (NeedHtmlTags(text))
							{
								DrawHtmlTags(graphics, textGeom, RectangleD.CreateFromRectangle(rectangleF2), font4);
							}
							else if (textGeom.MaximalWidth.HasValue)
							{
								StiRotatedTextDrawing.DrawString(graphics, text, font4, brush4, rectangleF2, stringFormat4, textGeom.RotationMode.GetValueOrDefault(), textGeom.Angle, textGeom.Antialiasing, textGeom.MaximalWidth.GetValueOrDefault());
							}
							else
							{
								StiRotatedTextDrawing.DrawString(graphics, text, font4, brush4, rectangleF2, stringFormat4, textGeom.RotationMode.GetValueOrDefault(), textGeom.Angle, textGeom.Antialiasing);
							}
						}
						finally
						{
							((IDisposable)stringFormat4)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)font4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)brush4)?.Dispose();
				}
			}
			else if (textGeom.Location is PointF)
			{
				PointF pointF = (PointF)textGeom.Location;
				Brush brush5 = GetBrush(obj, RectangleF.Empty);
				try
				{
					Font font5 = GetFont(textGeom.Font);
					try
					{
						StringFormat stringFormat5 = GetStringFormat(textGeom.StringFormat);
						try
						{
							if (textGeom.MaximalWidth.HasValue)
							{
								StiRotatedTextDrawing.DrawString(graphics, text, font5, brush5, pointF, stringFormat5, textGeom.RotationMode.GetValueOrDefault(), textGeom.Angle, textGeom.Antialiasing, textGeom.MaximalWidth.GetValueOrDefault());
							}
							else
							{
								StiRotatedTextDrawing.DrawString(graphics, text, font5, brush5, pointF, stringFormat5, textGeom.RotationMode.GetValueOrDefault(), textGeom.Angle, textGeom.Antialiasing);
							}
						}
						finally
						{
							((IDisposable)stringFormat5)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)font5)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)brush5)?.Dispose();
				}
			}
		}
		if (textGeom.Antialiasing)
		{
			graphics.TextRenderingHint = textRenderingHint;
		}
	}

	private void PushTranslateTransformGeom(StiPushTranslateTransformGeom translate)
	{
		PushState();
		graphics.TranslateTransform(translate.X, translate.Y);
	}

	private void PushClipPathGeom(StiPushClipPathGeom clip)
	{
		PushState();
		graphics.SetClip(GetPath(clip.Geoms), (CombineMode)1);
	}

	private void PushClipGeom(StiPushClipGeom clip)
	{
		PushState();
		graphics.SetClip(clip.ClipRectangle, (CombineMode)1);
	}

	private void PushRotateTransformGeom(StiPushRotateTransformGeom rotate)
	{
		PushState();
		graphics.RotateTransform(rotate.Angle);
	}

	private void PopClipGeom(StiPopClipGeom clip)
	{
		PopState();
	}

	private void PopTransformGeom(StiPopTransformGeom translate)
	{
		PopState();
	}

	private void PushSmothingModeToAntiAliasGeom(StiPushSmothingModeToAntiAliasGeom smothing)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		PushState();
		smothingModes.Add(graphics.SmoothingMode);
		graphics.SmoothingMode = (SmoothingMode)4;
	}

	private void PopSmothingModeGeom(StiPopSmothingModeGeom smothing)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (smothingModes.Count > 0)
		{
			graphics.SmoothingMode = smothingModes[smothingModes.Count - 1];
			smothingModes.RemoveAt(smothingModes.Count - 1);
		}
		PopState();
	}

	private void PushTextRenderingHintToAntiAliasGeom(StiPushTextRenderingHintToAntiAliasGeom text)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		PushState();
		textRenderingHints.Add(graphics.TextRenderingHint);
		graphics.TextRenderingHint = (TextRenderingHint)4;
	}

	private void PopTextRenderingHintGeom(StiPopTextRenderingHintGeom text)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		graphics.TextRenderingHint = textRenderingHints[textRenderingHints.Count - 1];
		textRenderingHints.RemoveAt(textRenderingHints.Count - 1);
		PopState();
	}

	private void PushState()
	{
		states.Add(graphics.Save());
	}

	private void PopState()
	{
		if (states.Count > 0)
		{
			graphics.Restore(states[states.Count - 1]);
			states.RemoveAt(states.Count - 1);
		}
	}

	private void DrawAnimationBar(StiClusteredBarSeriesAnimationGeom animationGeom)
	{
		RectangleF rectangleF = (RectangleF)animationGeom.ColumnRect;
		object obj = animationGeom.Background;
		StiPenGeom stiPenGeom = animationGeom.BorderPen;
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			if (animation is StiColumnAnimation { RectFrom: var rectFrom } stiColumnAnimation)
			{
				rectangleF = StiAnimationEngine.GetAnimationRectangle(this, rectangleF, rectFrom, stiColumnAnimation);
			}
		}
		else
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation2);
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawBorderGeom(new StiBorderGeom(obj, null, stiPenGeom, rectangleF, animationGeom.Interaction, -1));
	}

	private void DrawAnimationColumn(StiClusteredColumnSeriesAnimationGeom animationGeom)
	{
		RectangleF rectangleF = (RectangleF)animationGeom.ColumnRect;
		object obj = animationGeom.Background;
		StiPenGeom stiPenGeom = animationGeom.BorderPen;
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			if (animation is StiColumnAnimation { RectFrom: var rectFrom } stiColumnAnimation)
			{
				rectangleF = StiAnimationEngine.GetAnimationRectangle(this, rectangleF, rectFrom, stiColumnAnimation);
			}
		}
		else
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation2);
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawBorderGeom(new StiBorderGeom(obj, null, stiPenGeom, rectangleF, animationGeom.Interaction, -1));
	}

	private void DrawAnimationBorder(StiBorderAnimationGeom animationGeom)
	{
		RectangleF rectangleF = (RectangleF)animationGeom.Rect;
		object obj = animationGeom.Background;
		StiPenGeom stiPenGeom = animationGeom.BorderPen;
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			if (animation is StiColumnAnimation stiColumnAnimation)
			{
				rectangleF = StiAnimationEngine.GetAnimationRectangle(this, rectangleF, stiColumnAnimation.RectFrom, stiColumnAnimation);
			}
		}
		else
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation2);
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawBorderGeom(new StiBorderGeom(obj, null, stiPenGeom, rectangleF, animationGeom.Interaction, -1));
	}

	private void DrawAnimationEllipse(StiEllipseAnimationGeom animationGeom)
	{
		RectangleF rectangleF = (RectangleF)animationGeom.Rect;
		object obj = animationGeom.Background;
		StiPenGeom stiPenGeom = animationGeom.BorderPen;
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			if (animation is StiScaleAnimation animation3)
			{
				rectangleF = StiAnimationEngine.GetAnimationScale(this, rectangleF, animation3);
			}
		}
		else
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation2);
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawEllipseGeom(new StiEllipseGeom(obj, stiPenGeom, rectangleF, animationGeom.Interaction, -1));
	}

	private void DrawAnimationPath(StiPathAnimationGeom animationGeom)
	{
		object obj = animationGeom.Background;
		StiPenGeom stiPenGeom = animationGeom.Pen;
		List<StiSegmentGeom> list = animationGeom.Geoms;
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			if (!(animation is StiScaleAnimation animation3))
			{
				if (animation == null)
				{
					list = list.Select(delegate(StiSegmentGeom segmentGeom)
					{
						if (segmentGeom is StiCurveSegmentGeom stiCurveSegmentGeom)
						{
							((StiPointsAnimation)stiCurveSegmentGeom.Animation).BeginTime = TimeSpan.Zero;
							return new StiCurveSegmentGeom(StiAnimationEngine.GetAnimationPoints(this, stiCurveSegmentGeom.Points, (StiPointsAnimation)stiCurveSegmentGeom.Animation), stiCurveSegmentGeom.Tension);
						}
						if (segmentGeom is StiLineSegmentGeom stiLineSegmentGeom)
						{
							((StiPointsAnimation)stiLineSegmentGeom.Animation).BeginTime = TimeSpan.Zero;
							PointF[] animationPoints = StiAnimationEngine.GetAnimationPoints(this, new PointF[2]
							{
								new PointF(stiLineSegmentGeom.X1, stiLineSegmentGeom.Y1),
								new PointF(stiLineSegmentGeom.X2, stiLineSegmentGeom.Y2)
							}, (StiPointsAnimation)stiLineSegmentGeom.Animation);
							return new StiLineSegmentGeom(animationPoints[0], animationPoints[1]);
						}
						if (segmentGeom is StiLinesSegmentGeom stiLinesSegmentGeom)
						{
							((StiPointsAnimation)stiLinesSegmentGeom.Animation).BeginTime = TimeSpan.Zero;
							return new StiLinesSegmentGeom(StiAnimationEngine.GetAnimationPoints(this, stiLinesSegmentGeom.Points, (StiPointsAnimation)stiLinesSegmentGeom.Animation));
						}
						return segmentGeom;
					}).ToList();
				}
			}
			else
			{
				list = StiAnimationEngine.GetAnimationScale(this, list, animation3);
			}
		}
		else
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation2);
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawPathGeom(new StiPathGeom(obj, stiPenGeom, list, animationGeom.Rect, animationGeom.Interaction, -1));
	}

	private void DrawAnimationPathElement(StiPathElementAnimationGeom animationGeom)
	{
		object obj = animationGeom.Background;
		StiPenGeom stiPenGeom = animationGeom.BorderPen;
		List<StiSegmentGeom> list = animationGeom.PathGeoms.ToList();
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			StiPieSegmentAnimation stiPieSegmentAnimation = animation as StiPieSegmentAnimation;
			if (stiPieSegmentAnimation != null)
			{
				if (stiPieSegmentAnimation.RectDtFrom.IsEmpty)
				{
					list = list.Select((StiSegmentGeom segmentGeom) => (segmentGeom is StiPieSegmentGeom geom) ? StiAnimationEngine.GetAnimationAngle(this, geom, stiPieSegmentAnimation) : segmentGeom).ToList();
				}
				else if (list.Count == 4 && list[0] is StiArcSegmentGeom && list[2] is StiArcSegmentGeom)
				{
					list = StiAnimationEngine.GetAnimationAngle(this, list, stiPieSegmentAnimation);
				}
			}
		}
		else
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation2);
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawPathGeom(new StiPathGeom(obj, stiPenGeom, list, animationGeom.Rect, animationGeom.Interaction, -1));
	}

	private void DrawAnimationLines(StiLinesAnimationGeom animationGeom)
	{
		StiPenGeom stiPenGeom = animationGeom.Pen;
		PointF[] array = animationGeom.Points;
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			if (!(animation is StiPointsAnimation animation3))
			{
				if (animation is StiTranslationAnimation animation4)
				{
					stiPenGeom = StiAnimationEngine.GetAnimationTranslation(this, array, stiPenGeom, animation4);
				}
			}
			else
			{
				array = StiAnimationEngine.GetAnimationPoints(this, array, animation3);
			}
		}
		else
		{
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawLinesGeom(new StiLinesGeom(stiPenGeom, array));
	}

	private void DrawAnimationCurve(StiCurveAnimationGeom animationGeom)
	{
		StiPenGeom stiPenGeom = animationGeom.Pen;
		PointF[] array = animationGeom.Points;
		StiAnimation animation = animationGeom.Animation;
		if (!(animation is StiOpacityAnimation animation2))
		{
			if (!(animation is StiPointsAnimation animation3))
			{
				if (animation is StiTranslationAnimation animation4)
				{
					stiPenGeom = StiAnimationEngine.GetAnimationTranslation(this, array, stiPenGeom, animation4);
				}
			}
			else
			{
				array = StiAnimationEngine.GetAnimationPoints(this, array, animation3);
			}
		}
		else
		{
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation2);
		}
		DrawCurveGeom(new StiCurveGeom(stiPenGeom, array, animationGeom.Tension));
	}

	private void DrawAnimationLabel(StiLabelAnimationGeom animationGeom)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		object obj = animationGeom.TextBrush;
		object obj2 = animationGeom.LabelBrush;
		StiPenGeom stiPenGeom = animationGeom.PenBorder;
		if (animationGeom.Animation is StiOpacityAnimation animation)
		{
			obj = StiAnimationEngine.GetAnimationOpacity(this, obj, animation);
			obj2 = StiAnimationEngine.GetAnimationOpacity(this, obj2, animation);
			stiPenGeom = StiAnimationEngine.GetAnimationOpacity(this, stiPenGeom, animation);
		}
		if (animationGeom.DrawBorder)
		{
			DrawBorderGeom(new StiBorderGeom(obj2, null, stiPenGeom, animationGeom.Rectangle, animationGeom.Interaction, -1, animationGeom.Angle, animationGeom.RotationMode.Value));
		}
		DrawTextGeom(new StiTextGeom(animationGeom.Text, animationGeom.Font, obj, animationGeom.Rectangle, animationGeom.StringFormat, animationGeom.Angle, antialiasing: true, null, animationGeom.RotationMode, animationGeom.Angle != 0f));
	}

	private void DrawAnimationShadow(StiShadowAnimationGeom animationGeom)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(Color.FromArgb(60, 150, 150, 150));
		RectangleF rect = animationGeom.Rect;
		for (int i = 1; i <= animationGeom.ShadowWidth; i++)
		{
			rect.X++;
			rect.Y++;
			val = new SolidBrush(Color.FromArgb((byte)(60 / animationGeom.ShadowWidth * (animationGeom.ShadowWidth - i)), 150, 150, 150));
			StiAnimation animation = animationGeom.Animation;
			if (animation is StiOpacityAnimation animation2)
			{
				val = (SolidBrush)StiAnimationEngine.GetAnimationOpacity(this, val, animation2);
			}
			GraphicsPath val2 = new GraphicsPath();
			if (animationGeom.RadiusX != 0.0)
			{
				val2.AddEllipse(rect);
			}
			else
			{
				val2.AddRectangle(rect);
			}
			graphics.FillPath((Brush)(object)val, val2);
		}
		((Brush)val).Dispose();
	}

	private PenAlignment GetPenAlignment(StiPenAlignment alignment)
	{
		return (PenAlignment)(alignment switch
		{
			StiPenAlignment.Inset => 1, 
			StiPenAlignment.Left => 3, 
			StiPenAlignment.Outset => 2, 
			StiPenAlignment.Right => 4, 
			StiPenAlignment.Center => 0, 
			_ => 0, 
		});
	}

	private object GetRect(object rect)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (rect is Rectangle)
		{
			return (Rectangle)rect;
		}
		if (rect is RectangleF)
		{
			return (RectangleF)rect;
		}
		if (rect is RectangleD val)
		{
			return ((RectangleD)(ref val)).ToRectangleF();
		}
		return null;
	}

	private GraphicsPath GetPath(List<StiSegmentGeom> geoms)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		foreach (StiSegmentGeom geom in geoms)
		{
			if (geom is StiArcSegmentGeom)
			{
				StiArcSegmentGeom stiArcSegmentGeom = geom as StiArcSegmentGeom;
				val.AddArc(stiArcSegmentGeom.Rect, stiArcSegmentGeom.StartAngle, stiArcSegmentGeom.SweepAngle);
			}
			else if (geom is StiCloseFigureSegmentGeom)
			{
				val.CloseAllFigures();
			}
			else if (geom is StiCurveSegmentGeom)
			{
				StiCurveSegmentGeom stiCurveSegmentGeom = geom as StiCurveSegmentGeom;
				val.AddCurve(stiCurveSegmentGeom.Points, stiCurveSegmentGeom.Tension);
			}
			else if (geom is StiLineSegmentGeom)
			{
				StiLineSegmentGeom stiLineSegmentGeom = geom as StiLineSegmentGeom;
				val.AddLine(stiLineSegmentGeom.X1, stiLineSegmentGeom.Y1, stiLineSegmentGeom.X2, stiLineSegmentGeom.Y2);
			}
			else if (geom is StiLinesSegmentGeom)
			{
				StiLinesSegmentGeom stiLinesSegmentGeom = geom as StiLinesSegmentGeom;
				val.AddLines(stiLinesSegmentGeom.Points);
			}
			else if (geom is StiPieSegmentGeom)
			{
				StiPieSegmentGeom stiPieSegmentGeom = geom as StiPieSegmentGeom;
				val.AddPie(stiPieSegmentGeom.Rect.X, stiPieSegmentGeom.Rect.Y, stiPieSegmentGeom.Rect.Width, stiPieSegmentGeom.Rect.Height, stiPieSegmentGeom.StartAngle, stiPieSegmentGeom.SweepAngle);
			}
		}
		return val;
	}

	private Brush GetBrush(object brush, object rect)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (brush == null)
		{
			return null;
		}
		if (brush is Color)
		{
			return (Brush)new SolidBrush((Color)brush);
		}
		if (brush is StiBrush)
		{
			if (rect is Rectangle)
			{
				return StiBrush.GetBrush((StiBrush)brush, (Rectangle)rect);
			}
			if (rect is RectangleF)
			{
				return StiBrush.GetBrush((StiBrush)brush, (RectangleF)rect);
			}
		}
		return (Brush)((brush is Brush) ? brush : null);
	}

	private Pen GetPen(StiPenGeom penGeom)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		if (penGeom == null)
		{
			return null;
		}
		if (penGeom.Brush == null)
		{
			return null;
		}
		if ((int)penGeom.PenStyle == 6)
		{
			return null;
		}
		if (penGeom.Brush is Color)
		{
			Pen val = new Pen((Color)penGeom.Brush, penGeom.Thickness);
			val.Alignment = GetPenAlignment(penGeom.Alignment);
			val.DashStyle = StiPenUtils.GetPenStyle(penGeom.PenStyle);
			val.StartCap = StiPenUtils.GetLineCap(penGeom.StartCap);
			val.EndCap = StiPenUtils.GetLineCap(penGeom.EndCap);
			return val;
		}
		if (penGeom.Brush is Brush)
		{
			return new Pen((Brush)penGeom.Brush, penGeom.Thickness)
			{
				Alignment = GetPenAlignment(penGeom.Alignment),
				DashStyle = StiPenUtils.GetPenStyle(penGeom.PenStyle),
				StartCap = StiPenUtils.GetLineCap(penGeom.StartCap),
				EndCap = StiPenUtils.GetLineCap(penGeom.EndCap)
			};
		}
		return null;
	}

	private Font GetFont(StiFontGeom fontGeom)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (fontGeom.FontFamily == null)
		{
			return new Font(StiFontCollection.GetFontFamily(fontGeom.FontName, false), (float)((double)fontGeom.FontSize * StiDpiHelper.DeviceCapsScale), fontGeom.FontStyle, fontGeom.Unit, fontGeom.GdiCharSet, fontGeom.GdiVerticalFont);
		}
		return new Font(fontGeom.FontFamily, (float)((double)fontGeom.FontSize * StiDpiHelper.DeviceCapsScale));
	}

	private StringFormat GetStringFormat(StiStringFormatGeom sfGeom)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		StringFormat val = (StringFormat)(sfGeom.IsGeneric ? ((object)/*isinst with value type is only supported in some contexts*/) : ((object)new StringFormat()));
		val.Alignment = sfGeom.Alignment;
		val.FormatFlags = sfGeom.FormatFlags;
		val.HotkeyPrefix = sfGeom.HotkeyPrefix;
		val.LineAlignment = sfGeom.LineAlignment;
		val.Trimming = sfGeom.Trimming;
		return val;
	}

	private bool NeedHtmlTags(string text)
	{
		if (StiOptions.Engine.AllowHtmlTagsInChart)
		{
			return StiTextRenderer.CheckTextForHtmlTags(text);
		}
		return false;
	}

	private void DrawHtmlTags(Graphics graphics, StiTextGeom textGeom, RectangleD rect, Font fontGdi)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		string text = textGeom.Text;
		object brush = textGeom.Brush;
		StiTextRenderer.DrawText(graphics, text, fontGdi, rect, StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null)), Color.Transparent, 1.0, (StiTextHorAlignment)textGeom.StringFormat.Alignment, (StiVertAlignment)textGeom.StringFormat.LineAlignment, (textGeom.StringFormat.FormatFlags & 0x1000) <= 0, (textGeom.StringFormat.FormatFlags & 1) > 0, 1.0, (double)textGeom.Angle, textGeom.StringFormat.Trimming, false, true, (StiTextOptions)null, (double?)null);
	}

	private SizeF MeasureHtmlTags(Graphics graphics, string text, RectangleD rect, Font fontGdi, StiStringFormatGeom sf, float angle)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		SizeD val = StiTextRenderer.MeasureText(graphics, text, fontGdi, rect, 1.0, sf != null && (sf.FormatFlags & 0x1000) <= 0, sf != null && (sf.FormatFlags & 1) > 0, 1.0, (double)angle, (StringTrimming)((sf != null) ? ((int)sf.Trimming) : 0), false, true, new StiTextOptions(), (double?)1.0);
		return ((SizeD)(ref val)).ToSizeF();
	}

	private static string CheckSurrogatesAndSupplementaryCharacters(string txt)
	{
		return txt;
	}

	public StiGdiContextPainter(Graphics graphics)
	{
		this.graphics = graphics;
	}
}
