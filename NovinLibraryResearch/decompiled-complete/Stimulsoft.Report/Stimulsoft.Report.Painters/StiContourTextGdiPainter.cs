using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiContourTextGdiPainter : StiTextGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected I4, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		StiContourText stiContourText = (StiContourText)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiContourText.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiContourText.InvokePainting(stiContourText, e);
		}
		if (!e.Cancel && (stiContourText.Enabled || stiContourText.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiContourText.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						if (stiContourText.Brush is StiSolidBrush && ((StiSolidBrush)stiContourText.Brush).Color == Color.Transparent && stiContourText.Report.Info.FillComponent && stiContourText.IsDesigning)
						{
							Color color = Color.FromArgb(150, Color.White);
							StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						else
						{
							StiDrawing.FillRectangle(graphics, stiContourText.Brush, paintRectangle);
						}
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiContourText, graphics, paintRectangle);
					}
					if (stiContourText.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiContourText, graphics, paintRectangle, stiContourText.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiContourText.Border.Topmost);
					}
					if (e.DrawBorderFormatting)
					{
						string text = stiContourText.GetTextInternal();
						if (stiContourText.IsDesigning && !stiContourText.Report.Info.QuickInfoOverlay && stiContourText.Report.Info.QuickInfoType != 0)
						{
							text = stiContourText.GetQuickInfo();
						}
						if (!string.IsNullOrEmpty(text))
						{
							GraphicsState val = graphics.Save();
							double num = stiContourText.Border.Size / 2.0;
							RectangleD val2 = paintRectangle;
							((RectangleD)(ref val2)).X = ((RectangleD)(ref val2)).X + num;
							((RectangleD)(ref val2)).Y = ((RectangleD)(ref val2)).Y + num;
							((RectangleD)(ref val2)).Width = ((RectangleD)(ref val2)).Width - num;
							((RectangleD)(ref val2)).Height = ((RectangleD)(ref val2)).Height - num;
							graphics.SetClip(((RectangleD)(ref val2)).ToRectangleF(), (CombineMode)1);
							graphics.TranslateTransform((float)(((RectangleD)(ref val2)).X + ((RectangleD)(ref val2)).Width / 2.0), (float)(((RectangleD)(ref val2)).Y + ((RectangleD)(ref val2)).Height / 2.0));
							((RectangleD)(ref val2)).X = (0.0 - ((RectangleD)(ref val2)).Width) / 2.0;
							((RectangleD)(ref val2)).Y = (0.0 - ((RectangleD)(ref val2)).Height) / 2.0;
							if ((stiContourText.TextOptions.Angle > 45f && stiContourText.TextOptions.Angle < 135f) || (stiContourText.TextOptions.Angle > 225f && stiContourText.TextOptions.Angle < 315f))
							{
								((RectangleD)(ref val2))._002Ector(((RectangleD)(ref val2)).Y, ((RectangleD)(ref val2)).X, ((RectangleD)(ref val2)).Height, ((RectangleD)(ref val2)).Width);
							}
							GraphicsPath val3 = new GraphicsPath();
							try
							{
								StringFormat stringFormat = StiTextDrawing.GetStringFormat(stiContourText.TextOptions, stiContourText.HorAlignment, stiContourText.VertAlignment, (float)stiContourText.Page.Zoom);
								try
								{
									val3.AddString(text, stiContourText.Font.FontFamily, (int)stiContourText.Font.Style, (float)((double)stiContourText.Font.Size * stiContourText.Page.Zoom * 1.3 * StiScale.Factor), ((RectangleD)(ref val2)).ToRectangleF(), stringFormat);
									graphics.RotateTransform(stiContourText.TextOptions.Angle);
									Brush brush = StiBrush.GetBrush(stiContourText.TextBrush, ((RectangleD)(ref val2)).ToRectangleF());
									try
									{
										Pen val4 = new Pen(stiContourText.ContourColor);
										try
										{
											graphics.FillPath(brush, val3);
											val4.Width = (float)(stiContourText.Size * stiContourText.Page.Zoom * StiScale.Factor);
											graphics.DrawPath(val4, val3);
										}
										finally
										{
											((IDisposable)val4)?.Dispose();
										}
									}
									finally
									{
										((IDisposable)brush)?.Dispose();
									}
								}
								finally
								{
									((IDisposable)stringFormat)?.Dispose();
								}
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
							graphics.Restore(val);
						}
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiContourText, e.Graphics, paintRectangle);
						PaintConditions(stiContourText, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiContourText.InvokePainted(stiContourText, e);
		}
	}
}
