using System;
using System.Drawing;
using System.Drawing.Imaging;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiTableCellTextGdiPainter : StiComponentGdiPainter
{
	public virtual void PaintText(StiTableCell textComp, Graphics g, RectangleD rect)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		string text = textComp.GetTextForPaint();
		if (textComp.IsDesigning && !textComp.Report.Info.QuickInfoOverlay && textComp.Report.Info.QuickInfoType != 0)
		{
			text = textComp.GetQuickInfo();
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (textComp.TextQuality == StiTextQuality.Wysiwyg && !StiTextGdiPainter.IsThumbnailsMode)
			{
				StiWysiwygTextRender.DrawString(g, rect, text, textComp);
			}
			else if (textComp.TextQuality == StiTextQuality.Typographic && !StiTextGdiPainter.IsThumbnailsMode)
			{
				StiTypographicTextRender.DrawString(g, rect, text, textComp);
			}
			else
			{
				StiStandardTextRenderer.DrawString(g, rect, text, textComp);
			}
		}
	}

	public virtual void PaintBackground(StiTableCell text, Graphics g, RectangleD rect)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		if (text.Brush is StiSolidBrush && ((StiSolidBrush)text.Brush).Color == Color.Transparent && text.Report.Info.FillComponent && text.IsDesigning)
		{
			Color color = Color.FromArgb(150, Color.White);
			StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		}
		else
		{
			StiDrawing.FillRectangle(g, text.Brush, rect);
		}
		if (text.IsSelected)
		{
			Color color2 = Color.FromArgb(80, Color.FromArgb(168, 205, 241));
			StiDrawing.FillRectangle(g, color2, rect);
		}
	}

	public virtual void PaintBorder(StiTableCell text, Graphics g, RectangleD rect, bool drawBorderFormatting, bool drawTopmostBorderSides)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if (text.HighlightState == StiHighlightState.Hide)
		{
			base.PaintBorder(text, g, rect, text.Page.Zoom, drawBorderFormatting, drawTopmostBorderSides);
		}
	}

	public virtual void PaintLinesOfUnderlining(StiTableCell text, Graphics g, RectangleD rect)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Invalid comparison between Unknown and I4
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Invalid comparison between Unknown and I4
		double num = (double)text.Font.GetHeight() * text.Page.Zoom;
		if ((int)text.LinesOfUnderline == 6)
		{
			return;
		}
		Pen val = new Pen(text.Border.Color);
		try
		{
			val.DashStyle = StiPenUtils.GetPenStyle(text.LinesOfUnderline);
			val.Width = (int)(text.Border.Size * text.Page.Zoom);
			if ((int)text.VertAlignment == 0)
			{
				for (double num2 = num + ((RectangleD)(ref rect)).Top; num2 < ((RectangleD)(ref rect)).Bottom; num2 += num)
				{
					PaintOneLineOfUnderline(text, g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Right, num2);
				}
			}
			else if ((int)text.VertAlignment == 1)
			{
				double num3 = ((RectangleD)(ref rect)).Top + ((RectangleD)(ref rect)).Height / 2.0 + num * 0.5 - num * 0.1;
				while (num3 > ((RectangleD)(ref rect)).Top && num3 < ((RectangleD)(ref rect)).Bottom)
				{
					PaintOneLineOfUnderline(text, g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Right, num3);
					num3 -= num;
				}
				for (num3 = ((RectangleD)(ref rect)).Top + ((RectangleD)(ref rect)).Height / 2.0 + num * 0.5 - num * 0.1; num3 > ((RectangleD)(ref rect)).Top && num3 < ((RectangleD)(ref rect)).Bottom; num3 += num)
				{
					PaintOneLineOfUnderline(text, g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Right, num3);
				}
			}
			else if ((int)text.VertAlignment == 2)
			{
				for (double num4 = ((RectangleD)(ref rect)).Bottom - num - num * 0.1; num4 > ((RectangleD)(ref rect)).Top; num4 -= num)
				{
					PaintOneLineOfUnderline(text, g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Right, num4);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintOneLineOfUnderline(StiTableCell text, Graphics g, Pen pen, double x1, double x2, double y)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)text.LinesOfUnderline == 5)
		{
			g.DrawLine(pen, (float)x1, (float)(y - 1.0), (float)x2, (float)(y - 1.0));
			g.DrawLine(pen, (float)x1, (float)(y + 1.0), (float)x2, (float)(y + 1.0));
		}
		else
		{
			g.DrawLine(pen, (float)x1, (float)y, (float)x2, (float)y);
		}
	}

	private void PaintMarkerFields(StiTableCell text, Graphics g, RectangleD rect)
	{
		if (text.GetMarkerFieldResult())
		{
			double zoom = text.Report.Info.Zoom;
			RectangleD val = default(RectangleD);
			((RectangleD)(ref val))._002Ector(((RectangleD)(ref rect)).Right - 8.0 * zoom, ((RectangleD)(ref rect)).Bottom - 8.0 * zoom, 6.0 * zoom, 6.0 * zoom);
			g.FillRectangle(Brushes.Red, ((RectangleD)(ref val)).ToRectangleF());
		}
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		StiTableCell stiTableCell = (StiTableCell)component;
		double zoom2 = stiTableCell.Report.Info.Zoom;
		stiTableCell.Report.Info.Zoom = zoom;
		RectangleD paintRectangle = stiTableCell.GetPaintRectangle();
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		int num = (int)((RectangleD)(ref paintRectangle)).Width;
		int num2 = (int)((RectangleD)(ref paintRectangle)).Height;
		Image val = null;
		if (format == StiExportFormat.Pdf && stiTableCell.CheckAllowHtmlTags() && !stiTableCell.IsExportAsImage(format))
		{
			Bitmap val2 = new Bitmap(1, 1);
			try
			{
				Graphics val3 = Graphics.FromImage((Image)(object)val2);
				try
				{
					IntPtr hdc = val3.GetHdc();
					val = (Image)new Metafile(hdc, (EmfType)3);
					val3.ReleaseHdc(hdc);
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
		}
		else
		{
			val = (Image)new Bitmap(num, num2);
		}
		Graphics val4 = Graphics.FromImage(val);
		try
		{
			val4.PageUnit = (GraphicsUnit)2;
			if (format != StiExportFormat.Pdf)
			{
				val4.FillRectangle(Brushes.White, new Rectangle(0, 0, num, num2));
			}
			else if (stiTableCell.Brush != null && stiTableCell.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color == Color.Transparent)
			{
				val4.FillRectangle((Brush)new SolidBrush(Color.FromArgb(1, 255, 255, 255)), new Rectangle(0, 0, num, num2));
			}
			StiDrawing.FillRectangle(val4, stiTableCell.Brush, paintRectangle);
			PaintText(stiTableCell, val4, paintRectangle);
			PaintLinesOfUnderlining(stiTableCell, val4, paintRectangle);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		stiTableCell.Report.Info.Zoom = zoom2;
		return val;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Invalid comparison between Unknown and I4
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiTableCell stiTableCell = (StiTableCell)component;
		if (!stiTableCell.Enabled)
		{
			e.Cancel = false;
			component.InvokePainted(component, e);
			return;
		}
		component.InvokePainting(component, e);
		if (!e.Cancel && (component.Enabled || component.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD val = component.GetPaintRectangle();
			if (((RectangleD)(ref val)).Width > 0.0 && ((RectangleD)(ref val)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref val)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						PaintBackground(stiTableCell, graphics, val);
					}
					if (e.DrawBorderFormatting && stiTableCell.HighlightState == StiHighlightState.Hide && (int)stiTableCell.Border.Side != 15)
					{
						PaintMarkers(stiTableCell, graphics, val);
					}
					RectangleD rect = val;
					val = stiTableCell.ConvertTextMargins(val, convert: true);
					val = stiTableCell.ConvertTextBorders(val, convert: true);
					if (e.DrawBorderFormatting)
					{
						PaintText(stiTableCell, graphics, val);
					}
					if (e.DrawBorderFormatting)
					{
						PaintLinesOfUnderlining(stiTableCell, graphics, val);
					}
					PaintBorder(stiTableCell, graphics, rect, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiTableCell.Border.Topmost);
					PaintMarkerFields(stiTableCell, e.Graphics, val);
				}
			}
			PaintEvents(component, e.Graphics, val);
			PaintConditions(component, e.Graphics, val);
			PaintQuickButtons(component, e.Graphics);
			PaintInteraction(component, e.Graphics);
		}
		e.Cancel = false;
		component.InvokePainted(component, e);
	}

	public override void PaintSelection(StiComponent component, StiPaintEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (component.IsDesigning && component.IsSelected && !component.Report.Info.IsComponentsMoving)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			int i = StiScale.I2;
			SolidBrush val = new SolidBrush(Color.DimGray);
			try
			{
				StiDrawing.DrawSelectedRectangle(graphics, i, (Brush)(object)val, paintRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
