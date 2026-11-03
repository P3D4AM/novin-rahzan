using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiShapeGdiPainter : StiComponentGdiPainter
{
	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		StiShape stiShape = (StiShape)component;
		double zoom2 = stiShape.Report.Info.Zoom;
		stiShape.Report.Info.Zoom = zoom;
		StiScale.Lock();
		int num = (int)Math.Round(stiShape.Size * zoom / 2f);
		RectangleD val = component.ComponentToPage(component.ClientRectangle);
		RectangleD rect = ((RectangleD)(ref val)).Normalize();
		val = stiShape.Report.Unit.ConvertToHInches(rect);
		rect = ((RectangleD)(ref val)).Multiply((double)zoom);
		((RectangleD)(ref rect)).X = 0.0;
		((RectangleD)(ref rect)).Y = 0.0;
		int num2 = (int)(((RectangleD)(ref rect)).Width + 0.5) + 1 + (int)(stiShape.Size * zoom);
		int num3 = (int)(((RectangleD)(ref rect)).Height + 0.5) + 1 + (int)(stiShape.Size * zoom);
		Bitmap val2 = new Bitmap(num2, num3);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			val3.PageUnit = (GraphicsUnit)2;
			if (format == StiExportFormat.ImagePng)
			{
				val3.Clear(Color.Transparent);
			}
			else
			{
				Color color = Color.White;
				if (component.Page != null)
				{
					color = StiBrush.ToColor(component.Page.Brush);
				}
				if (color.Equals((object?)Color.Transparent) || color.Equals((object?)Color.Empty))
				{
					color = Color.White;
				}
				val3.Clear(color);
			}
			val3.TranslateTransform((float)num, (float)num);
			GraphicsState val4 = val3.Save();
			val3.SmoothingMode = (SmoothingMode)2;
			val3.InterpolationMode = (InterpolationMode)7;
			val3.CompositingQuality = (CompositingQuality)2;
			stiShape.ShapeType.Paint(val3, stiShape, ((RectangleD)(ref rect)).ToRectangleF(), zoom);
			DrawText(val3, stiShape, rect);
			val3.Restore(val4);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		stiShape.Report.Info.Zoom = zoom2;
		StiScale.Unlock();
		return (Image)(object)val2;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides)
		{
			return;
		}
		StiShape stiShape = (StiShape)component;
		stiShape.InvokePainting(stiShape, e);
		if (!e.Cancel && (stiShape.Enabled || stiShape.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			GraphicsState val = graphics.Save();
			graphics.SmoothingMode = (SmoothingMode)2;
			graphics.InterpolationMode = (InterpolationMode)7;
			graphics.CompositingQuality = (CompositingQuality)2;
			RectangleD paintRectangle = stiShape.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					stiShape.ShapeType.Paint(graphics, stiShape, ((RectangleD)(ref paintRectangle)).ToRectangleF(), (float)(stiShape.Page.Zoom * StiScale.Factor));
					DrawText(graphics, stiShape, paintRectangle);
					PaintMarkers(stiShape, graphics, paintRectangle);
					PaintEvents(stiShape, e.Graphics, paintRectangle);
					PaintConditions(stiShape, e.Graphics, paintRectangle);
				}
			}
			graphics.Restore(val);
		}
		e.Cancel = false;
		stiShape.InvokePainted(stiShape, e);
	}

	public void DrawText(Graphics g, StiShape shape, RectangleD rect)
	{
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Invalid comparison between Unknown and I4
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Invalid comparison between Unknown and I4
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Invalid comparison between Unknown and I4
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Invalid comparison between Unknown and I4
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Invalid comparison between Unknown and I4
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Invalid comparison between Unknown and I4
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		string parsedText = shape.GetParsedText();
		if (string.IsNullOrEmpty(parsedText))
		{
			return;
		}
		g.SetClip(((RectangleD)(ref rect)).ToRectangle());
		double num = shape.Page.Zoom * StiScale.Factor;
		if (((RectangleD)(ref rect)).Width - shape.Margins.Left * num - shape.Margins.Right * num <= 0.0 || ((RectangleD)(ref rect)).Height - shape.Margins.Top * num - shape.Margins.Bottom * num <= 0.0)
		{
			return;
		}
		((RectangleD)(ref rect))._002Ector(((RectangleD)(ref rect)).X + shape.Margins.Left * num, ((RectangleD)(ref rect)).Y + shape.Margins.Top * num, ((RectangleD)(ref rect)).Width - (shape.Margins.Left + shape.Margins.Right) * num, ((RectangleD)(ref rect)).Height - (shape.Margins.Top + shape.Margins.Bottom) * num);
		double num2 = (double)shape.Font.Size * shape.Page.Zoom;
		if (num2 < 1.0 && !shape.IsPrinting && StiOptions.Engine.TinyTextOptimization)
		{
			StiSolidBrush val = new StiSolidBrush(Color.FromArgb(128, shape.ForeColor));
			StiDrawing.FillRectangle(g, (StiBrush)(object)val, rect);
			return;
		}
		num = shape.Page.Zoom;
		if (StiScale.IsNoScaling && !shape.IsPrinting && StiDpiHelper.NeedDeviceCapsScale)
		{
			num *= (double)(float)StiDpiHelper.DeviceCapsScale;
		}
		num2 = (float)((double)shape.Font.Size * num);
		if (shape.Report.IsPrinting)
		{
			if ((int)shape.Font.Unit != 2 && (int)shape.Font.Unit != 0)
			{
				num2 *= (double)(StiOptions.Engine.TypographicTextQualityScale / 100f);
			}
		}
		else if ((int)shape.Font.Unit != 2 && (int)shape.Font.Unit != 0)
		{
			num2 *= (double)(StiOptions.Engine.StandardTextQualityScale / 96f);
		}
		Font val2 = StiFontUtils.ChangeFontSize(shape.Font, (float)num2);
		try
		{
			SolidBrush val3 = new SolidBrush(shape.ForeColor);
			try
			{
				if (shape.BackgroundColor != Color.Transparent)
				{
					SizeF sizeF = g.MeasureString(parsedText, val2, (int)((RectangleD)(ref rect)).Width);
					double num3 = ((RectangleD)(ref rect)).X;
					double num4 = ((RectangleD)(ref rect)).Y;
					StiTextHorAlignment horAlignment = shape.HorAlignment;
					if ((int)horAlignment != 1)
					{
						if ((int)horAlignment == 2)
						{
							num3 = ((RectangleD)(ref rect)).Right - (double)sizeF.Width;
						}
					}
					else
					{
						num3 = ((RectangleD)(ref rect)).X + ((RectangleD)(ref rect)).Width / 2.0 - (double)(sizeF.Width / 2f);
					}
					StiVertAlignment vertAlignment = shape.VertAlignment;
					if ((int)vertAlignment != 1)
					{
						if ((int)vertAlignment == 2)
						{
							num4 = ((RectangleD)(ref rect)).Bottom - (double)sizeF.Height;
						}
					}
					else
					{
						num4 = ((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0 - (double)(sizeF.Height / 2f);
					}
					RectangleF rectangleF = new RectangleF((float)num3, (float)num4, sizeF.Width, sizeF.Height);
					SolidBrush val4 = new SolidBrush(shape.BackgroundColor);
					try
					{
						g.FillRectangle((Brush)(object)val4, rectangleF);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				TextRenderingHint textRenderingHint = g.TextRenderingHint;
				g.TextRenderingHint = (TextRenderingHint)4;
				StiTextDrawing.DrawString(g, parsedText, val2, (Brush)(object)val3, rect, new StiTextOptions
				{
					WordWrap = true
				}, shape.HorAlignment, shape.VertAlignment, true, (float)shape.Page.Zoom, (double)StiOptions.Engine.TextLineSpacingScale);
				g.TextRenderingHint = textRenderingHint;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		g.ResetClip();
	}
}
