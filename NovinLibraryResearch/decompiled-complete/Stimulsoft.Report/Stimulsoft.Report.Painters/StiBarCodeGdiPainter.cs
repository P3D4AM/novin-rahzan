using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiBarCodeGdiPainter : StiComponentGdiPainter, IStiBarCodePainter
{
	public void BaseTransform(object context, float x, float y, float angle, float dx, float dy)
	{
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		val.TranslateTransform(x, y);
		val.RotateTransform(angle);
		val.TranslateTransform(dx, dy);
	}

	public void BaseRollbackTransform(object context)
	{
	}

	public void BaseFillRectangle(object context, StiBrush brush, float x, float y, float width, float height)
	{
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		Brush brush2 = StiBrush.GetBrush(brush, new RectangleF(x, y, width, height));
		try
		{
			val.FillRectangle(brush2, x, y, width, height);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public void BaseFillRectangle2D(object context, StiBrush brush, float x, float y, float width, float height)
	{
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		Brush brush2 = StiBrush.GetBrush(brush, new RectangleF(x, y, width, height));
		try
		{
			val.FillRectangle(brush2, x, y, width, height);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public void BaseFillPolygon(object context, StiBrush brush, PointF[] points)
	{
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		float x = points[0].X;
		float x2 = points[0].X;
		float y = points[0].Y;
		float y2 = points[0].Y;
		for (int i = 0; i < points.Length; i++)
		{
			PointF pointF = points[i];
			if (pointF.X < x)
			{
				x = pointF.X;
			}
			if (pointF.X > x2)
			{
				x2 = pointF.X;
			}
			if (pointF.Y < y)
			{
				y = pointF.Y;
			}
			if (pointF.Y > y2)
			{
				y2 = pointF.Y;
			}
		}
		Brush brush2 = StiBrush.GetBrush(brush, new RectangleF(x, y, x2 - x, y2 - y));
		try
		{
			val.FillPolygon(brush2, points);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public void BaseFillEllipse(object context, StiBrush brush, float x, float y, float width, float height)
	{
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		Brush brush2 = StiBrush.GetBrush(brush, new RectangleF(x, y, width, height));
		try
		{
			val.FillEllipse(brush2, x, y, width, height);
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public void BaseDrawRectangle(object context, Color penColor, float penSize, float x, float y, float width, float height)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		Pen val2 = new Pen(penColor, penSize);
		try
		{
			val.DrawRectangle(val2, x, y, width, height);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public void BaseDrawImage(object context, Image image, StiReport report, float x, float y, float width, float height)
	{
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (image != null)
		{
			Graphics val = (Graphics)((context is Graphics) ? context : null);
			val.DrawImage(image, x, y, width, height);
		}
	}

	public void BaseDrawString(object context, string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			TextRenderingHint textRenderingHint = val.TextRenderingHint;
			val.TextRenderingHint = (TextRenderingHint)4;
			val.DrawString(st, font, brush2, rect, sf);
			val.TextRenderingHint = textRenderingHint;
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public SizeF BaseMeasureString(object context, string st, Font font)
	{
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)((context is Graphics) ? context : null);
		return val.MeasureString(st, font);
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		StiBarCode stiBarCode = component as StiBarCode;
		zoom *= StiOptions.Engine.BarcodeDpiMultiplierFactor;
		double zoom2 = stiBarCode.Report.Info.Zoom;
		stiBarCode.Report.Info.Zoom = zoom;
		RectangleD val = component.ComponentToPage(component.ClientRectangle);
		RectangleD rect = ((RectangleD)(ref val)).Normalize();
		val = stiBarCode.Report.Unit.ConvertToHInches(rect);
		rect = ((RectangleD)(ref val)).Multiply((double)zoom);
		((RectangleD)(ref rect)).X = 0.0;
		((RectangleD)(ref rect)).Y = 0.0;
		stiBarCode.Report.Info.Zoom = zoom2;
		int num = (int)((RectangleD)(ref rect)).Width;
		int num2 = (int)((RectangleD)(ref rect)).Height;
		Bitmap val2 = new Bitmap(num, num2);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			val3.PageUnit = (GraphicsUnit)2;
			if (format != StiExportFormat.Pdf && format != StiExportFormat.ImagePng)
			{
				val3.Clear(Color.White);
			}
			else
			{
				val3.Clear(Color.FromArgb(1, 255, 255, 255));
			}
			if (!string.IsNullOrEmpty(stiBarCode.CodeValue))
			{
				stiBarCode.BarCodeType.Draw(val3, stiBarCode, ((RectangleD)(ref rect)).ToRectangleF(), zoom);
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		return (Image)(object)val2;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		StiBarCode stiBarCode = component as StiBarCode;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiBarCode.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiBarCode.InvokePainting(stiBarCode, e);
		}
		if (!e.Cancel && (component.Enabled || component.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiBarCode.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						GraphicsState val = graphics.Save();
						if (!stiBarCode.IsPrinting && !stiBarCode.IsExporting && !StiOptions.Engine.DisableAntialiasingInPainters)
						{
							graphics.SmoothingMode = (SmoothingMode)4;
						}
						graphics.SetClip(((RectangleD)(ref paintRectangle)).ToRectangleF(), (CombineMode)1);
						if (stiBarCode.Page != null && (stiBarCode.IsDesigning || !string.IsNullOrEmpty(stiBarCode.CodeValue)))
						{
							float zoom = (float)(stiBarCode.Page.Zoom * StiScale.Factor);
							if (stiBarCode.IsPrinting && StiOptions.Print.BarcodeAsBitmap)
							{
								Image image = GetImage(stiBarCode, ref zoom, StiExportFormat.ImagePng);
								graphics.DrawImage(image, ((RectangleD)(ref paintRectangle)).ToRectangleF());
							}
							else
							{
								stiBarCode.BarCodeType.Draw(graphics, stiBarCode, ((RectangleD)(ref paintRectangle)).ToRectangleF(), zoom);
							}
						}
						graphics.Restore(val);
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiBarCode, graphics, paintRectangle);
					}
					PaintBorder(stiBarCode, graphics, paintRectangle, (float)stiBarCode.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiBarCode.Border.Topmost);
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiBarCode, e.Graphics, paintRectangle);
						PaintConditions(stiBarCode, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiBarCode.InvokePainted(stiBarCode, e);
		}
	}
}
