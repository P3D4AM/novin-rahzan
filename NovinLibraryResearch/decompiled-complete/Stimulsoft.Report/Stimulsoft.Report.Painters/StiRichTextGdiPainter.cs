using System;
using System.Drawing;
using System.Drawing.Imaging;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Painters;

public class StiRichTextGdiPainter : StiComponentGdiPainter
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
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		StiRichText stiRichText = (StiRichText)component;
		if (format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlTable)
		{
			zoom *= 2f;
		}
		if (stiRichText.Image == null && StiOptions.Engine.FullTrust)
		{
			stiRichText.RenderMetafile(stiRichText.BackColor.A > 0);
		}
		StiExportUtils.DisableFontSmoothing(component.Report);
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
			if (color.A == 0)
			{
				color = ((format != StiExportFormat.ImagePng && format != StiExportFormat.Pdf) ? Color.White : Color.FromArgb(1, 255, 255, 255));
			}
			val5.Clear(color);
			RectangleF rectangleF2 = default(RectangleF);
			if (stiRichText.Image is Metafile)
			{
				Image image = stiRichText.Image;
				Metafile val6 = (Metafile)(object)((image is Metafile) ? image : null);
				GraphicsUnit val7 = (GraphicsUnit)2;
				try
				{
					rectangleF2 = ((Image)val6).GetBounds(ref val7);
				}
				catch
				{
					((Image)val6).Dispose();
					stiRichText.RenderMetafile();
					Image image2 = stiRichText.Image;
					val6 = (Metafile)(object)((image2 is Metafile) ? image2 : null);
					rectangleF2 = ((Image)val6).GetBounds(ref val7);
				}
			}
			RectangleF rectangleF3 = new RectangleF(0f, 0f, rectangleF.Width + 2f, rectangleF.Height + 2f);
			if ((stiRichText.Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale) && rectangleF2.Width > 0f)
			{
				rectangleF3 = rectangleF2;
			}
			if (StiOptions.Engine.FullTrust && stiRichText.Image != null)
			{
				val5.DrawImage(stiRichText.Image, (RectangleF)new Rectangle(RoundValue(rectangleF.X * zoom), RoundValue(rectangleF.Y * zoom), RoundValue(rectangleF.Width * zoom), RoundValue(rectangleF.Height * zoom)), rectangleF3, (GraphicsUnit)2);
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		return (Image)(object)val4;
	}

	public virtual void PaintBackground(StiRichText text, Graphics g, RectangleD rect)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (text.BackColor.A == 0 && text.Report.Info.FillComponent && text.IsDesigning)
		{
			Color color = Color.FromArgb(150, Color.White);
			StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		}
		else
		{
			StiDrawing.FillRectangle(g, text.BackColor, rect);
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		StiRichText stiRichText = (StiRichText)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiRichText.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiRichText.InvokePainting(stiRichText, e);
		}
		if (!e.Cancel && (stiRichText.Enabled || stiRichText.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiRichText.GetPaintRectangle();
			RectangleD val = stiRichText.ConvertTextMargins(paintRectangle, convert: true);
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				if (component.Report.IsExporting && stiRichText.BackColor.A == 0)
				{
					StiExportUtils.DisableFontSmoothing(component.Report);
				}
				if (e.DrawBorderFormatting)
				{
					PaintBackground(stiRichText, graphics, paintRectangle);
				}
				if (e.DrawBorderFormatting)
				{
					if (stiRichText.Image == null)
					{
						stiRichText.RenderMetafile();
					}
					if (stiRichText.Image == null)
					{
						if (!string.IsNullOrEmpty(stiRichText.DataColumn) || !string.IsNullOrEmpty(stiRichText.DataUrl.Value))
						{
							Font val2 = new Font("Segoe UI", (float)(10.0 * stiRichText.Page.Zoom));
							try
							{
								string text = null;
								string text2 = ((stiRichText.DataUrl != null) ? stiRichText.DataUrl.Value : null);
								if (!string.IsNullOrEmpty(text2))
								{
									text = StiHyperlinkProcessor.HyperlinkToString(text2);
								}
								else if (!string.IsNullOrEmpty(stiRichText.DataColumn))
								{
									text = string.Format("{0} {1}", StiLocalization.Get("Report", "LabelDataColumn"), stiRichText.DataColumn);
								}
								graphics.DrawString(text, val2, Brushes.Black, ((RectangleD)(ref paintRectangle)).ToRectangleF());
							}
							finally
							{
								((IDisposable)val2)?.Dispose();
							}
						}
					}
					else
					{
						PaintEvents(stiRichText, graphics, paintRectangle);
						PaintConditions(stiRichText, e.Graphics, paintRectangle);
						if (stiRichText.Image != null)
						{
							float num = (float)(((RectangleD)(ref val)).Width / stiRichText.Page.Zoom * (double)(StiOptions.Engine.RichTextScale / 96f)) + 1f;
							float height = (float)(((RectangleD)(ref val)).Height / stiRichText.Page.Zoom * (double)(StiOptions.Engine.RichTextScale / 96f)) + 1f;
							RectangleF rectangleF = default(RectangleF);
							if (stiRichText.Image is Metafile)
							{
								Image image = stiRichText.Image;
								Metafile val3 = (Metafile)(object)((image is Metafile) ? image : null);
								GraphicsUnit val4 = (GraphicsUnit)2;
								try
								{
									rectangleF = ((Image)val3).GetBounds(ref val4);
								}
								catch
								{
									((Image)val3).Dispose();
									stiRichText.RenderMetafile();
									Image image2 = stiRichText.Image;
									val3 = (Metafile)(object)((image2 is Metafile) ? image2 : null);
									rectangleF = ((Image)val3).GetBounds(ref val4);
								}
								if (rectangleF.Width > num && stiRichText.WordWrap)
								{
									num = rectangleF.Width;
								}
								rectangleF.Width *= 96f / StiOptions.Engine.RichTextScale;
								rectangleF.Height *= 96f / StiOptions.Engine.RichTextScale;
							}
							try
							{
								RectangleF rectangleF2 = ((RectangleD)(ref val)).ToRectangleF();
								RectangleF rectangleF3 = new RectangleF(0f, 0f, num, height);
								if (stiRichText.Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale)
								{
									rectangleF3 = rectangleF;
								}
								graphics.DrawImage(stiRichText.Image, rectangleF2, rectangleF3, (GraphicsUnit)2);
							}
							catch
							{
							}
						}
					}
				}
				if (e.DrawBorderFormatting)
				{
					PaintMarkers(stiRichText, graphics, paintRectangle);
				}
				if (stiRichText.HighlightState == StiHighlightState.Hide)
				{
					PaintBorder(stiRichText, graphics, paintRectangle, stiRichText.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiRichText.Border.Topmost);
				}
			}
			if (e.DrawBorderFormatting)
			{
				PaintQuickButtons(stiRichText, e.Graphics);
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiRichText.InvokePainted(stiRichText, e);
		}
	}
}
