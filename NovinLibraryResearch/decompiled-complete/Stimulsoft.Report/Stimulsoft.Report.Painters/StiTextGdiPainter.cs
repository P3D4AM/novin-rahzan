using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Painters;

public class StiTextGdiPainter : StiComponentGdiPainter
{
	public static bool IsThumbnailsMode { get; set; }

	public virtual RectangleD PaintIndicator(StiText textComp, Graphics g, RectangleD rect)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (textComp.Indicator == null)
		{
			return rect;
		}
		StiIndicatorTypePainter painter = StiIndicatorTypePainter.GetPainter(textComp.Indicator.GetType(), (StiGuiMode)0);
		return painter.Paint(g, textComp, rect);
	}

	public virtual void PaintText(StiText textComp, Graphics g, RectangleD rect)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		string text = textComp.GetTextForPaint();
		if (textComp.IsDesigning && !textComp.Report.Info.QuickInfoOverlay && textComp.Report.Info.QuickInfoType != 0)
		{
			text = textComp.GetQuickInfo();
		}
		if (!string.IsNullOrEmpty(text))
		{
			double num = (double)textComp.Font.Size * textComp.Page.Zoom * StiScale.Factor;
			if (num < 1.0 && !textComp.IsPrinting && StiOptions.Engine.TinyTextOptimization)
			{
				StiSolidBrush val = new StiSolidBrush(Color.FromArgb(128, StiBrush.ToColor(textComp.TextBrush)));
				StiDrawing.FillRectangle(g, (StiBrush)(object)val, rect);
			}
			else if (StiOptions.Engine.UseNewHtmlEngine && textComp.AllowHtmlTags)
			{
				StiHtmlTextRender.DrawString(g, rect, text, textComp);
			}
			else if (textComp.TextQuality == StiTextQuality.Wysiwyg && !IsThumbnailsMode)
			{
				StiWysiwygTextRender.DrawString(g, rect, text, textComp);
			}
			else if (textComp.TextQuality == StiTextQuality.Typographic && !IsThumbnailsMode)
			{
				StiTypographicTextRender.DrawString(g, rect, text, textComp);
			}
			else
			{
				StiStandardTextRenderer.DrawString(g, rect, text, textComp);
			}
		}
	}

	public virtual void PaintBackground(StiText text, Graphics g, RectangleD rect)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		PaintBackground(text, g, rect, paintExtendedMargins: false);
	}

	public virtual void PaintBackground(StiText text, Graphics g, RectangleD rect, bool paintExtendedMargins)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if ((text.ExceedMargins == StiExceedMargins.None || paintExtendedMargins) && !(text.ExceedMargins == StiExceedMargins.None && paintExtendedMargins))
		{
			if (text.ExceedMargins != StiExceedMargins.None && paintExtendedMargins)
			{
				rect = ApplyExceedMargins(text, rect);
			}
			if (text.Brush is StiSolidBrush && ((StiSolidBrush)text.Brush).Color.A == 0 && text.Report.Info.FillComponent && text.IsDesigning)
			{
				Color color = Color.FromArgb(150, Color.White);
				StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
			}
			else
			{
				StiDrawing.FillRectangle(g, text.Brush, rect);
			}
		}
	}

	private static RectangleD ApplyExceedMargins(StiText text, RectangleD rect)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		RectangleD paintRectangle = text.GetPaintRectangle(convertToHInches: true, convertByZoom: true);
		if ((text.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
		{
			double num = text.Page.Unit.ConvertToHInches(text.Page.Margins.Left) * text.Page.Zoom * StiScale.Factor;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + (((RectangleD)(ref paintRectangle)).Left + num);
			((RectangleD)(ref rect)).X = 0.0 - num;
		}
		if ((text.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
		{
			double num2 = text.Page.Unit.ConvertToHInches(text.Page.Margins.Top) * text.Page.Zoom * StiScale.Factor;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + (((RectangleD)(ref paintRectangle)).Top + num2);
			((RectangleD)(ref rect)).Y = 0.0 - num2;
		}
		if ((text.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
		{
			double num3 = text.Page.Unit.ConvertToHInches(text.Page.Width + text.Page.Margins.Right) * text.Page.Zoom * StiScale.Factor;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + (num3 - ((RectangleD)(ref paintRectangle)).Right);
		}
		if ((text.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
		{
			double num4 = text.Page.Unit.ConvertToHInches(text.Page.Height + text.Page.Margins.Bottom) * text.Page.Zoom * StiScale.Factor;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + (num4 - ((RectangleD)(ref paintRectangle)).Bottom);
		}
		return rect;
	}

	public virtual void PaintBorder(StiText text, Graphics g, RectangleD rect, bool drawBorderFormatting, bool drawTopmostBorderSides)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if (text.HighlightState == StiHighlightState.Hide)
		{
			base.PaintBorder(text, g, rect, text.Page.Zoom * StiScale.Factor, drawBorderFormatting, drawTopmostBorderSides);
		}
	}

	public virtual void PaintLinesOfUnderlining(StiText text, Graphics g, RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Invalid comparison between Unknown and I4
		if ((int)text.LinesOfUnderline == 6)
		{
			return;
		}
		double num = (double)text.Font.GetHeight() * text.Page.Zoom * StiScale.Factor;
		Pen val = new Pen(text.Border.Color);
		try
		{
			val.DashStyle = StiPenUtils.GetPenStyle(text.LinesOfUnderline);
			val.Width = (int)(text.Border.Size * text.Page.Zoom * StiScale.Factor);
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

	public virtual void PaintOneLineOfUnderline(StiText text, Graphics g, Pen pen, double x1, double x2, double y)
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

	private void PaintMarkerFields(StiText text, Graphics g, RectangleD rect)
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		StiText stiText = (StiText)component;
		if (stiText.Indicator != null)
		{
			zoom *= 2f;
		}
		double zoom2 = stiText.Report.Info.Zoom;
		stiText.Report.Info.Zoom = zoom;
		StiScale.Lock();
		RectangleD val = stiText.GetPaintRectangle();
		((RectangleD)(ref val)).X = 0.0;
		((RectangleD)(ref val)).Y = 0.0;
		if ((format == StiExportFormat.HtmlTable || format == StiExportFormat.ImagePng) && StiOptions.Export.Html.PrintLayoutOptimization && stiText.Border != null && (int)stiText.Border.Style != 6 && (int)stiText.Border.Side != 0)
		{
			((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height - stiText.Border.Size;
			((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - stiText.Border.Size;
			if (((RectangleD)(ref val)).Height < 0.0)
			{
				((RectangleD)(ref val)).Height = 0.0;
			}
			if (((RectangleD)(ref val)).Width < 0.0)
			{
				((RectangleD)(ref val)).Width = 0.0;
			}
		}
		int num = (int)((RectangleD)(ref val)).Width;
		int num2 = (int)((RectangleD)(ref val)).Height;
		Image val2 = (Image)((format != StiExportFormat.Pdf || !stiText.CheckAllowHtmlTags() || stiText.IsExportAsImage(format) || !StiOptions.Engine.FullTrust) ? ((object)new Bitmap(num, num2)) : ((object)CreateMetafileImage()));
		if (stiText.Brush != null && stiText.TextQuality == StiTextQuality.Wysiwyg)
		{
			StiExportUtils.DisableFontSmoothing(component.Report);
		}
		Graphics val3 = Graphics.FromImage(val2);
		try
		{
			val3.PageUnit = (GraphicsUnit)2;
			bool flag = stiText.Brush != null && ((stiText.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.A == 0) || stiText.Brush is StiEmptyBrush);
			if (flag)
			{
				val3.TextRenderingHint = (TextRenderingHint)3;
			}
			if (format == StiExportFormat.Pdf || format == StiExportFormat.ImagePng)
			{
				if (flag)
				{
					val3.FillRectangle((Brush)new SolidBrush(Color.FromArgb(1, 255, 255, 255)), new Rectangle(0, 0, num, num2));
				}
			}
			else
			{
				val3.FillRectangle(Brushes.White, new Rectangle(0, 0, num, num2));
			}
			StiDrawing.FillRectangle(val3, stiText.Brush, val);
			val = PaintIndicator(stiText, val3, val);
			val = stiText.ConvertTextMargins(val, convert: true);
			val = stiText.ConvertTextBorders(val, convert: true);
			PaintText(stiText, val3, val);
			PaintLinesOfUnderlining(stiText, val3, val);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		stiText.Report.Info.Zoom = zoom2;
		StiScale.Unlock();
		return val2;
	}

	private static Image CreateMetafileImage()
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Image val = null;
		Bitmap val2 = new Bitmap(1, 1);
		try
		{
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				IntPtr hdc = val3.GetHdc();
				val = (Image)new Metafile(hdc, (EmfType)3);
				val3.ReleaseHdc(hdc);
				return val;
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

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Invalid comparison between Unknown and I4
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		StiText stiText = (StiText)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiText.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			component.InvokePainting(component, e);
		}
		if (!e.Cancel && (component.Enabled || component.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD val = component.GetPaintRectangle();
			if (((RectangleD)(ref val)).Width > 0.0 && ((RectangleD)(ref val)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref val)).IntersectsWith(e.ClipRectangle))
				{
					if (component.Report != null && component.Report.IsExporting && stiText.TextQuality == StiTextQuality.Wysiwyg)
					{
						StiExportUtils.DisableFontSmoothing(component.Report);
					}
					if (e.DrawBorderFormatting)
					{
						PaintBackground(stiText, graphics, val);
					}
					if (e.DrawBorderFormatting && stiText.HighlightState == StiHighlightState.Hide && (int)stiText.Border.Side != 15)
					{
						PaintMarkers(stiText, graphics, val);
					}
					RectangleD rect = val;
					val = PaintIndicator(stiText, graphics, val);
					val = stiText.ConvertTextMargins(val, convert: true);
					val = stiText.ConvertTextBorders(val, convert: true);
					if (e.DrawBorderFormatting)
					{
						PaintText(stiText, graphics, val);
					}
					if (e.DrawBorderFormatting)
					{
						PaintLinesOfUnderlining(stiText, graphics, val);
					}
					PaintBorder(stiText, graphics, rect, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiText.Border.Topmost);
					if (e.DrawBorderFormatting)
					{
						PaintMarkerFields(stiText, e.Graphics, val);
					}
				}
			}
			if (e.DrawBorderFormatting)
			{
				PaintEvents(component, e.Graphics, val);
				PaintConditions(component, e.Graphics, val);
				PaintQuickButtons(component, e.Graphics);
				PaintInteraction(component, e.Graphics);
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			component.InvokePainted(component, e);
		}
	}
}
