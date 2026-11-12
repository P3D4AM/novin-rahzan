using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiListBoxElementGdiPainter : StiControlElementGdiPainter
{
	public override void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiErrorStack.SetOk(element.GetKey());
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(rect.ToRectangleF());
			rect = StiElementGdiPainter.ApplyCornerRadiusAndSetClip(g, rect, element);
			StiDataTable dataTable = (StiDbsThumbnaiModelHelper.AllowPage(element.Page) ? StiThumbnailDataHelper.Get(element) : StiElementDataCache.GetOrCreateWithProgress(element));
			if (IsNoResult(element, dataTable))
			{
				PaintNoResult(g, rect, element);
				return;
			}
			StiListBoxElement stiListBoxElement = element as StiListBoxElement;
			if (stiListBoxElement.Orientation == StiItemOrientation.Horizontal)
			{
				PaintHorizontalList(g, rect, stiListBoxElement, dataTable);
			}
			else
			{
				PaintVerticalList(g, rect, stiListBoxElement, dataTable);
			}
		}
		catch (Exception ex)
		{
			StiErrorStack.SetError(element.GetKey(), ex.Message);
			PaintError(g, rect.ToRectangleF(), element);
		}
		finally
		{
			g.Restore(val);
		}
	}

	private void PaintHorizontalList(Graphics g, RectangleD rect, StiListBoxElement element, StiDataTable dataTable)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return;
		}
		int num = GetNameMeterIndex(dataTable);
		int keyMeterIndex = GetKeyMeterIndex(dataTable);
		if (num == -1)
		{
			num = keyMeterIndex;
		}
		float num2 = (float)StiElementScale.Factor((IStiElement)element);
		Font font = GetFont(element);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = StiFontUtils.ChangeFontSize(font, font.Size * num2);
			try
			{
				val.FormatFlags = (StringFormatFlags)4096;
				val.Alignment = (StringAlignment)1;
				val.LineAlignment = (StringAlignment)1;
				val.Trimming = (StringTrimming)3;
				List<string> list = new List<string>();
				if (element.ShowAllValue)
				{
					list.Add(StiLocHelper.LocAll);
				}
				foreach (object[] row in dataTable.Rows)
				{
					object obj = ((keyMeterIndex != -1) ? row[keyMeterIndex] : null);
					if (obj != null)
					{
						object obj2 = ((num != -1) ? row[num] : null);
						string item = Format(element, obj2 ?? obj);
						if (element.ShowBlanks || !StiListBoxHelper.IsBlank(obj2))
						{
							list.Add(item);
						}
					}
				}
				List<int> list2 = new List<int>();
				int width = StiScaleUI.I(150);
				int num3 = 0;
				foreach (string item2 in list)
				{
					int num4 = (int)g.MeasureString(item2, val2, (SizeF)new Size(width, 1000000)).Width + StiScaleUI.I16;
					num3 += num4;
					list2.Add(num4);
					if ((double)num3 > rect.Width)
					{
						break;
					}
				}
				double num5 = (((double)num3 > rect.Width) ? 1.0 : (rect.Width / (double)num3));
				Color foreColor = GetForeColor(element);
				int num6 = 0;
				double num7 = rect.X;
				foreach (string item3 in list)
				{
					if (num6 >= list2.Count)
					{
						break;
					}
					int num8 = (int)((double)list2[num6] * num5);
					Rectangle rect2 = new Rectangle((int)num7, (int)rect.Y, num8, (int)rect.Height);
					StiDrawing.DrawString(g, item3, val2, foreColor, rect2, val);
					if (num6 == 0 && element.ShowAllValue)
					{
						Color separatorColor = GetSeparatorColor(element);
						StiDrawing.DrawLine(g, separatorColor, rect2.Right - 1, rect2.Y + StiScaleUI.I4, rect2.Right - 1, rect2.Bottom - StiScaleUI.I4);
					}
					num7 += (double)num8;
					if (num7 >= rect.Right)
					{
						break;
					}
					num6++;
				}
				if (num5 == 1.0)
				{
					PaintArrowMoreColumns(g, new RectangleD(rect.X, rect.Y + (rect.Height - (double)StiScale.I(25)) / 2.0, rect.Width, StiScale.I(25)));
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PaintVerticalList(Graphics g, RectangleD rect, StiListBoxElement element, StiDataTable dataTable)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return;
		}
		int num = GetNameMeterIndex(dataTable);
		int keyMeterIndex = GetKeyMeterIndex(dataTable);
		if (num == -1)
		{
			num = keyMeterIndex;
		}
		float num2 = (float)StiElementScale.Factor((IStiElement)element);
		double num3 = (double)num2 * StiScale.Factor;
		Color foreColor = GetForeColor(element);
		Font font = GetFont(element);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = StiFontUtils.ChangeFontSize(font, font.Size * num2);
			try
			{
				val.FormatFlags = (StringFormatFlags)4096;
				val.LineAlignment = (StringAlignment)1;
				val.Trimming = (StringTrimming)3;
				if (element.ShowAllValue)
				{
					string locAll = StiLocHelper.LocAll;
					PaintItem(g, rect.ToRectangleF(), element, locAll, locAll, val2, foreColor, val, (float)num3);
					Color separatorColor = GetSeparatorColor(element);
					StiDrawing.DrawLine(g, separatorColor, (float)rect.X, (float)(rect.Y + 30.0 * num3), (float)rect.Right, (float)(rect.Y + 30.0 * num3));
					rect.Y += 30.0 * num3;
					rect.Height -= 30.0 * num3;
					if (rect.Height < 0.0)
					{
						return;
					}
				}
				foreach (object[] row in dataTable.Rows)
				{
					object obj = ((keyMeterIndex != -1) ? row[keyMeterIndex] : null);
					if (obj != null)
					{
						object obj2 = ((num != -1) ? row[num] : null);
						if (element.ShowBlanks || !StiListBoxHelper.IsBlank(obj2))
						{
							PaintItem(g, rect.ToRectangleF(), element, obj, obj2, val2, foreColor, val, (float)num3);
							rect.Y += 30.0 * num3;
							rect.Height -= 30.0 * num3;
						}
						if (rect.Height < 0.0)
						{
							break;
						}
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static void PaintArrowMoreColumns(Graphics g, RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			float num = (float)(rect.Height / 10.0);
			RectangleF rectangleF = rect.ToRectangleF();
			PointF[] array = new PointF[3]
			{
				new PointF(rectangleF.Right - num * 5f, rectangleF.Y + num * 3f),
				new PointF(rectangleF.Right - num * 2f, rectangleF.Y + num * 5f),
				new PointF(rectangleF.Right - num * 5f, rectangleF.Y + num * 7f)
			};
			g.FillPolygon(Brushes.White, array);
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	internal void PaintItem(Graphics g, RectangleF rect, IStiItemElement element, object key, object name, Font font, Color foreColor, StringFormat sf, float scale)
	{
		float num = 16f * scale;
		string text = Format(element as IStiListBoxElement, (name != null) ? name : key);
		RectangleF rect2 = new RectangleF(rect.X + 2f * scale, rect.Y + scale, rect.Width - 4f * scale, 30f * scale);
		if (element.SelectionMode == StiItemSelectionMode.Multi)
		{
			rect2.X += num + 6f * scale;
			rect2.Width -= num + 6f * scale;
		}
		StiDrawing.DrawString(g, text, font, foreColor, rect2, sf);
		if (element.SelectionMode == StiItemSelectionMode.Multi)
		{
			RectangleD rectD = new RectangleD(rect.X + 4f * scale, rect.Y + (30f * scale - num) / 2f, num, num);
			StiCheckPainter.PaintCheckBox(g, rectD, StiCheckState.Checked, GetGlyphColor(element as IStiControlElement), scale);
		}
	}

	private static int GetNameMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiNameListBoxMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private static int GetKeyMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiKeyListBoxMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}
}
