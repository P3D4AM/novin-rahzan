using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Painters;

public class StiTextInCellsGdiPainter : StiTextGdiPainter
{
	public override void PaintBorder(StiText textComp, Graphics g, RectangleD rect, bool drawBorderFormatting, bool drawTopmostBorderSides)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!textComp.IsDesigning || textComp.Report.IsPageDesigner)
		{
			base.PaintBorder(textComp, g, rect, drawBorderFormatting, drawTopmostBorderSides);
		}
	}

	public override void PaintBackground(StiText textComp, Graphics g, RectangleD rect)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (textComp.IsDesigning)
		{
			Color color = Color.FromArgb(150, Color.White);
			StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		}
		if (!textComp.IsDesigning || textComp.Report.IsPageDesigner)
		{
			base.PaintBackground(textComp, g, rect);
		}
	}

	public override void PaintText(StiText textComp, Graphics g, RectangleD rect)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Invalid comparison between Unknown and I4
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Invalid comparison between Unknown and I4
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		StiTextInCells stiTextInCells = textComp as StiTextInCells;
		if (!textComp.IsDesigning || textComp.Report.IsPageDesigner)
		{
			base.PaintText(textComp, g, rect);
			return;
		}
		float num = (float)(stiTextInCells.Border.Size / 2.0 * stiTextInCells.Page.Zoom);
		GraphicsState val = g.Save();
		g.SetClip(((RectangleD)(ref rect)).ToRectangleF());
		string text = stiTextInCells.GetTextForPaint();
		StiUnit unit = stiTextInCells.Page.Unit;
		float num2 = (float)unit.ConvertToHInches((double)stiTextInCells.HorSpacing * stiTextInCells.Page.Zoom);
		float num3 = (float)unit.ConvertToHInches((double)stiTextInCells.VertSpacing * stiTextInCells.Page.Zoom);
		float num4 = (float)unit.ConvertToHInches((double)stiTextInCells.CellWidth * stiTextInCells.Page.Zoom);
		float num5 = (float)unit.ConvertToHInches((double)stiTextInCells.CellHeight * stiTextInCells.Page.Zoom);
		if (stiTextInCells.CellWidth == 0f)
		{
			num4 = (float)((double)(stiTextInCells.Font.GetHeight() * 1.5f) * stiTextInCells.Page.Zoom);
		}
		if (stiTextInCells.CellHeight == 0f)
		{
			num5 = (float)((double)(stiTextInCells.Font.GetHeight() * 1.5f) * stiTextInCells.Page.Zoom);
		}
		float num6 = (float)((double)stiTextInCells.Font.Size * stiTextInCells.Page.Zoom);
		StringFormat val2 = new StringFormat();
		try
		{
			Font val3 = StiFontUtils.ChangeFontSize(stiTextInCells.Font, num6);
			try
			{
				val2.LineAlignment = (StringAlignment)1;
				val2.Alignment = (StringAlignment)1;
				if (!stiTextInCells.ContinuousText)
				{
					Size size = new Size(1, 1);
					float num7 = (float)(((RectangleD)(ref rect)).X + (double)num + (double)num4);
					while ((double)(num7 + num2 + num4) < ((RectangleD)(ref rect)).Right)
					{
						num7 += num2 + num4;
						size.Width++;
					}
					float num8 = (float)(((RectangleD)(ref rect)).Y + (double)num + (double)num5);
					while ((double)(num8 + num3 + num5) < ((RectangleD)(ref rect)).Bottom)
					{
						num8 += num3 + num5;
						size.Height++;
					}
					if (!stiTextInCells.WordWrap)
					{
						size.Height = 1;
					}
					ArrayList arrayList = new ArrayList();
					string text2 = string.Empty;
					if (text == null)
					{
						text = string.Empty;
					}
					string text3 = text;
					for (int i = 0; i < text3.Length; i++)
					{
						char c = text3[i];
						if (char.IsControl(c))
						{
							if (c == '\n')
							{
								arrayList.Add(StiTextInCellsHelper.TrimEndWhiteSpace(text2));
								text2 = string.Empty;
							}
						}
						else
						{
							text2 += c;
						}
					}
					if (text2 != string.Empty)
					{
						arrayList.Add(StiTextInCellsHelper.TrimEndWhiteSpace(text2));
					}
					if (arrayList.Count == 0)
					{
						arrayList.Add(text2);
					}
					if (stiTextInCells.WordWrap)
					{
						for (int j = 0; j < arrayList.Count; j++)
						{
							string text4 = (string)arrayList[j];
							if (text4.Length <= size.Width)
							{
								continue;
							}
							int[] array = new int[text4.Length];
							int num9 = 0;
							int k;
							for (k = 0; k < text4.Length && char.IsWhiteSpace(text4[k]); k++)
							{
								array[k] = num9;
							}
							for (int l = k; l < text4.Length; l++)
							{
								if (char.IsWhiteSpace(text4[l]))
								{
									num9++;
								}
								array[l] = num9;
							}
							int m = size.Width;
							int num10 = m - 1;
							if (array[m] > 0)
							{
								if (array[m] != array[num10])
								{
									for (; char.IsWhiteSpace(text4[m]); m++)
									{
									}
								}
								else
								{
									while (!char.IsWhiteSpace(text4[m]))
									{
										m--;
									}
									num10 = m++;
									while (char.IsWhiteSpace(text4[num10]))
									{
										num10--;
									}
								}
							}
							arrayList[j] = text4.Substring(0, num10 + 1);
							arrayList.Insert(j + 1, text4.Substring(m, text4.Length - m));
						}
					}
					num8 = (float)(((RectangleD)(ref rect)).Y + (double)num);
					for (int n = 0; n < size.Height; n++)
					{
						string text5 = ((n < arrayList.Count) ? ((string)arrayList[n]) : string.Empty);
						int num11 = 0;
						if ((int)stiTextInCells.HorAlignment == 1)
						{
							num11 = (size.Width - text5.Length) / 2;
						}
						if ((int)stiTextInCells.HorAlignment == 2)
						{
							num11 = size.Width - text5.Length;
						}
						if (num11 > 0)
						{
							text5 = new string(' ', num11) + text5;
						}
						num7 = (float)(((RectangleD)(ref rect)).X + (double)num);
						for (int num12 = 0; num12 < size.Width; num12++)
						{
							RectangleF rectangleF = new RectangleF(num7, num8, num4, num5);
							Brush brush = StiBrush.GetBrush(stiTextInCells.Brush, rectangleF);
							try
							{
								g.FillRectangle(brush, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
							}
							finally
							{
								((IDisposable)brush)?.Dispose();
							}
							int num13 = (stiTextInCells.RightToLeft ? (size.Width - num12 - 1) : num12);
							if (num13 < text5.Length)
							{
								Brush brush2 = StiBrush.GetBrush(stiTextInCells.TextBrush, rectangleF);
								try
								{
									g.DrawString(new string(text5[num13], 1), val3, brush2, rectangleF, val2);
								}
								finally
								{
									((IDisposable)brush2)?.Dispose();
								}
							}
							PaintBorder(stiTextInCells, g, RectangleD.CreateFromRectangle(rectangleF), stiTextInCells.Page.Zoom, drawBorderFormatting: true, drawBorderSides: true);
							num7 += num4 + num2;
						}
						num8 += num5 + num3;
					}
				}
				else
				{
					float num14 = (float)(((RectangleD)(ref rect)).X + (double)num);
					float num15 = (float)(((RectangleD)(ref rect)).Y + (double)num);
					bool flag = true;
					int num16 = 0;
					while (true)
					{
						RectangleF rectangleF2 = new RectangleF(num14, num15, num4, num5);
						if ((double)(rectangleF2.Right + num2) < ((RectangleD)(ref rect)).Right || flag)
						{
							Brush brush3 = StiBrush.GetBrush(stiTextInCells.Brush, rectangleF2);
							try
							{
								g.FillRectangle(brush3, rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
							}
							finally
							{
								((IDisposable)brush3)?.Dispose();
							}
							if (text != null && num16 < text.Length)
							{
								Brush brush4 = StiBrush.GetBrush(stiTextInCells.TextBrush, rectangleF2);
								try
								{
									g.DrawString(new string(text[num16], 1), val3, brush4, rectangleF2, val2);
								}
								finally
								{
									((IDisposable)brush4)?.Dispose();
								}
							}
							PaintBorder(stiTextInCells, g, RectangleD.CreateFromRectangle(rectangleF2), stiTextInCells.Page.Zoom, drawBorderFormatting: true, drawBorderSides: true);
							num14 += num4 + num2;
							num16++;
							flag = false;
						}
						else
						{
							num15 += num5 + num3;
							num14 = (float)((RectangleD)(ref rect)).X + num;
							flag = true;
							if (!stiTextInCells.WordWrap || ((RectangleD)(ref rect)).Bottom < (double)(num15 + num5 + num3))
							{
								break;
							}
						}
					}
				}
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
		g.Restore(val);
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		StiTextInCells stiTextInCells = component as StiTextInCells;
		double zoom2 = stiTextInCells.Report.Info.Zoom;
		stiTextInCells.Report.Info.Zoom = zoom;
		RectangleD rect = stiTextInCells.GetPaintRectangle();
		((RectangleD)(ref rect)).X = 0.0;
		((RectangleD)(ref rect)).Y = 0.0;
		int num = (int)((RectangleD)(ref rect)).Width;
		int num2 = (int)((RectangleD)(ref rect)).Height;
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.PageUnit = (GraphicsUnit)2;
			if (format != StiExportFormat.ImagePng)
			{
				val2.FillRectangle(Brushes.White, new Rectangle(0, 0, num, num2));
			}
			else
			{
				val2.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, num, num2));
			}
			rect = PaintIndicator(stiTextInCells, val2, rect);
			rect = stiTextInCells.ConvertTextMargins(rect, convert: true);
			rect = stiTextInCells.ConvertTextBorders(rect, convert: true);
			PaintText(stiTextInCells, val2, rect);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		stiTextInCells.Report.Info.Zoom = zoom2;
		return (Image)(object)val;
	}
}
