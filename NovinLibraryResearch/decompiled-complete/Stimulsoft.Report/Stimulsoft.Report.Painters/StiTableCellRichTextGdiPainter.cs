using System;
using System.Drawing;
using System.Drawing.Imaging;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Painters;

public class StiTableCellRichTextGdiPainter : StiComponentGdiPainter
{
	private int RoundValue(float value)
	{
		int num = (int)value;
		if ((float)num < value)
		{
			num++;
		}
		return num;
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		StiRichText stiRichText = (StiRichText)component;
		if (format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlTable)
		{
			zoom *= 2f;
		}
		if (stiRichText.Image == null)
		{
			stiRichText.RenderMetafile();
		}
		double val = stiRichText.Report.Unit.ConvertToHInches(stiRichText.Width);
		double val2 = stiRichText.Report.Unit.ConvertToHInches(stiRichText.Height);
		val = Math.Max(val, 1.0);
		val2 = Math.Max(val2, 1.0);
		RectangleD rect = default(RectangleD);
		((RectangleD)(ref rect))._002Ector(0.0, 0.0, val, val2);
		RectangleD val3 = stiRichText.ConvertTextMargins(rect, convert: false);
		RectangleF rectangleF = ((RectangleD)(ref val3)).ToRectangleF();
		rectangleF.Width = Math.Max(rectangleF.Width, 1f);
		rectangleF.Height = Math.Max(rectangleF.Height, 1f);
		Bitmap val4 = new Bitmap(RoundValue((float)val * zoom), RoundValue((float)val2 * zoom));
		Graphics val5 = Graphics.FromImage((Image)(object)val4);
		try
		{
			Color color = stiRichText.BackColor;
			if (color == Color.Transparent)
			{
				color = Color.White;
			}
			val5.Clear(color);
			if (StiOptions.Engine.FullTrust && stiRichText.Image != null)
			{
				val5.DrawImage(stiRichText.Image, new Rectangle(RoundValue(rectangleF.X * zoom), RoundValue(rectangleF.Y * zoom), RoundValue(rectangleF.Width * zoom), RoundValue(rectangleF.Height * zoom)), new Rectangle(RoundValue(0f), RoundValue(0f), RoundValue(rectangleF.Width + 2f), RoundValue(rectangleF.Height + 2f)), (GraphicsUnit)2);
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		return (Image)(object)val4;
	}

	public virtual void PaintBackground(StiTableCellRichText text, Graphics g, RectangleD rect)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (text.BackColor == Color.Transparent && text.Report.Info.FillComponent && text.IsDesigning)
		{
			Color color = Color.FromArgb(150, Color.White);
			StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		}
		else
		{
			StiDrawing.FillRectangle(g, text.BackColor, rect);
		}
		if (text.IsSelected)
		{
			Color color2 = Color.FromArgb(80, Color.FromArgb(168, 205, 241));
			StiDrawing.FillRectangle(g, color2, rect);
		}
	}

	private void DrawCellImage(Graphics g, RectangleD rect)
	{
		if (!(((RectangleD)(ref rect)).Width <= (double)StiScale.I9) && !(((RectangleD)(ref rect)).Height <= (double)StiScale.I9))
		{
			Image val = StiReportImages.Styles.RichText((StiImageSize)0);
			g.DrawImageUnscaled(val, new Rectangle((int)((RectangleD)(ref rect)).X, (int)((RectangleD)(ref rect)).Y, val.Width, val.Height));
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		StiTableCellRichText stiTableCellRichText = (StiTableCellRichText)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiTableCellRichText.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiTableCellRichText.InvokePainting(stiTableCellRichText, e);
		}
		if (!component.Enabled)
		{
			e.Cancel = false;
			stiTableCellRichText.InvokePainted(stiTableCellRichText, e);
			return;
		}
		if (!e.Cancel && (stiTableCellRichText.Enabled || stiTableCellRichText.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiTableCellRichText.GetPaintRectangle();
			RectangleD val = stiTableCellRichText.ConvertTextMargins(paintRectangle, convert: true);
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				if (e.DrawBorderFormatting)
				{
					PaintBackground(stiTableCellRichText, graphics, paintRectangle);
				}
				if (e.DrawBorderFormatting)
				{
					if (!string.IsNullOrEmpty(stiTableCellRichText.DataColumn) && stiTableCellRichText.IsDesigning)
					{
						Font val2 = new Font("Arial", (float)(9.0 * stiTableCellRichText.Page.Zoom));
						try
						{
							graphics.DrawString("Rtf from {" + stiTableCellRichText.DataColumn + "}", val2, Brushes.Black, ((RectangleD)(ref paintRectangle)).ToRectangleF());
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					else
					{
						stiTableCellRichText.RenderMetafile();
						PaintEvents(stiTableCellRichText, graphics, paintRectangle);
						PaintConditions(stiTableCellRichText, graphics, paintRectangle);
						PaintQuickButtons(stiTableCellRichText, graphics);
						if (stiTableCellRichText.Image != null)
						{
							float num = (float)(((RectangleD)(ref val)).Width / stiTableCellRichText.Page.Zoom) + 1f;
							float height = (float)(((RectangleD)(ref val)).Height / stiTableCellRichText.Page.Zoom) + 1f;
							if (stiTableCellRichText.Image is Metafile)
							{
								Image image = stiTableCellRichText.Image;
								Metafile val3 = (Metafile)(object)((image is Metafile) ? image : null);
								GraphicsUnit val4 = (GraphicsUnit)2;
								RectangleF bounds = ((Image)val3).GetBounds(ref val4);
								if (bounds.Width > num && stiTableCellRichText.WordWrap)
								{
									num = bounds.Width;
								}
							}
							try
							{
								RectangleF rectangleF = ((RectangleD)(ref val)).ToRectangleF();
								graphics.DrawImage(stiTableCellRichText.Image, rectangleF, new RectangleF(0f, 0f, num, height), (GraphicsUnit)2);
							}
							catch
							{
							}
						}
					}
				}
				if (e.DrawBorderFormatting)
				{
					PaintMarkers(stiTableCellRichText, graphics, paintRectangle);
				}
				if (stiTableCellRichText.HighlightState == StiHighlightState.Hide)
				{
					PaintBorder(stiTableCellRichText, graphics, paintRectangle, stiTableCellRichText.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiTableCellRichText.Border.Topmost);
				}
				if (e.DrawBorderFormatting && stiTableCellRichText.IsDesigning)
				{
					DrawCellImage(graphics, paintRectangle);
				}
				PaintQuickButtons(stiTableCellRichText, e.Graphics);
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiTableCellRichText.InvokePainted(stiTableCellRichText, e);
		}
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
