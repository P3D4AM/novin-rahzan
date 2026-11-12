using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiComboBoxElementGdiPainter : StiControlElementGdiPainter
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
			}
			else
			{
				PaintComboBox(g, rect, element as StiComboBoxElement, dataTable);
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

	private void PaintComboBox(Graphics g, RectangleD rect, StiComboBoxElement element, StiDataTable dataTable)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return;
		}
		float num = (float)StiElementScale.Factor((IStiElement)element);
		double num2 = (double)num * StiScale.Factor;
		Font font = GetFont(element);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = StiFontUtils.ChangeFontSize(font, font.Size * num);
			try
			{
				val.FormatFlags = (StringFormatFlags)4096;
				val.LineAlignment = (StringAlignment)1;
				if (element.ShowAllValue)
				{
					Color foreColor;
					Color backColor;
					if (element.SelectionMode == StiItemSelectionMode.Multi)
					{
						foreColor = GetSelectedForeColor(element);
						backColor = GetSelectedBackColor(element);
						val.Alignment = (StringAlignment)1;
					}
					else
					{
						foreColor = GetForeColor(element);
						backColor = GetBackColor(element);
					}
					PaintItem(g, rect.ToRectangleF(), element, StiLocHelper.LocAll, StiLocHelper.LocAll, val2, foreColor, backColor, val, (float)num2);
				}
				else
				{
					int nameMeterIndex = GetNameMeterIndex(dataTable);
					int keyMeterIndex = GetKeyMeterIndex(dataTable);
					object[] array = dataTable.Rows.FirstOrDefault();
					if (array == null)
					{
						return;
					}
					object obj = ((nameMeterIndex != -1) ? array[nameMeterIndex] : null);
					object obj2 = ((keyMeterIndex != -1) ? array[keyMeterIndex] : null);
					if (obj == null && obj2 == null)
					{
						return;
					}
					RectangleD rectangleD = rect;
					rectangleD.Width -= 20.0 * num2;
					Color foreColor2 = GetForeColor(element);
					PaintItem(g, rect.ToRectangleF(), element, obj2, obj, val2, foreColor2, Color.Transparent, val, (float)num2);
				}
				PaintButton(g, rect.ToRectangleF(), element, (float)num2);
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

	private void PaintItem(Graphics g, RectangleF rect, IStiItemElement element, object key, object name, Font font, Color foreColor, Color backColor, StringFormat sf, float scale)
	{
		string text = Format(element as IStiControlElement, (name != null) ? name : key);
		RectangleF rect2 = new RectangleF(rect.X + 2f * scale, rect.Y + scale, rect.Width - 4f * scale, rect.Height);
		if (!string.IsNullOrWhiteSpace(text))
		{
			if (element.SelectionMode == StiItemSelectionMode.Multi)
			{
				int width = TextRenderer.MeasureText(text, font).Width;
				rect2.Width = (float)width + 2f * scale;
				StiDrawing.FillRectangle(g, backColor, rect2);
			}
			StiDrawing.DrawString(g, text, font, foreColor, rect2, sf);
		}
	}

	private static int GetNameMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiNameComboBoxMeter);
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
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiKeyComboBoxMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}
}
