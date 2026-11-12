using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiDatePickerElementGdiPainter : StiControlElementGdiPainter
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
				PaintDatePicker(g, rect, element as StiDatePickerElement, dataTable);
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

	private void PaintDatePicker(Graphics g, RectangleD rect, StiDatePickerElement element, StiDataTable dataTable)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		float num = (float)StiElementScale.Factor((IStiElement)element);
		double num2 = (double)num * StiScale.Factor;
		Color foreColor = GetForeColor(element);
		Font font = GetFont(element);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = StiFontUtils.ChangeFontSize(font, font.Size * num);
			try
			{
				val.FormatFlags = (StringFormatFlags)4096;
				val.Trimming = (StringTrimming)3;
				val.LineAlignment = (StringAlignment)1;
				string dateTimeString = GetDateTimeString(element, dataTable);
				PaintItem(g, rect.ToRectangleF(), dateTimeString, val2, foreColor, val, (float)num2);
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

	private string GetDateTimeString(StiDatePickerElement element, StiDataTable dataTable)
	{
		object defaultValue = StiDatePickerHelper.GetDefaultValue(element);
		if (StiVariableExpressionHelper.IsVariableSpecifiedAsExpression(element, element?.ValueMeter?.Expression))
		{
			if (Stimulsoft.Report.Range.IsRangeType((StiDatePickerHelper.GetVariableSpecifiedAsValue(element) is StiVariable stiVariable) ? stiVariable.Type : null) && element.InitialRangeSelectionSource == StiInitialDateRangeSelectionSource.Selection)
			{
				StiDatePickerHelper.CalculateRangeFromInitial(element.InitialRangeSelection, out var start, out var end);
				return GetRangeVariableString(element, new DateTimeRange(start, end));
			}
			if (defaultValue is DateTimeRange)
			{
				return GetRangeVariableString(element, defaultValue);
			}
			return Format(element, defaultValue);
		}
		if (element.SelectionMode == StiDateSelectionMode.AutoRange)
		{
			return GetAutoRangeColumnString(element, dataTable);
		}
		if (element.SelectionMode == StiDateSelectionMode.Range)
		{
			return GetRangeColumnString(element);
		}
		return Format(element, defaultValue);
	}

	private string GetRangeVariableString(StiDatePickerElement element, object current)
	{
		DateTimeRange dateTimeRange = current as DateTimeRange;
		return Format(element, dateTimeRange.FromObject) + " - " + Format(element, dateTimeRange.ToObject);
	}

	private string GetAutoRangeColumnString(StiDatePickerElement element, StiDataTable dataTable)
	{
		IEnumerable<object> enumerable = (from r in dataTable?.Rows
			select r[0] into d
			where d is DateTime
			select d);
		if (enumerable == null || !enumerable.Any())
		{
			return string.Empty;
		}
		DateTime dateTime = enumerable.Where((object d) => d is DateTime).Cast<DateTime>().Min();
		DateTime dateTime2 = enumerable.Where((object d) => d is DateTime).Cast<DateTime>().Max();
		return Format(element, dateTime) + " - " + Format(element, dateTime2);
	}

	private string GetRangeColumnString(StiDatePickerElement element)
	{
		StiDatePickerHelper.CalculateRangeFromInitial(element.InitialRangeSelection, out var start, out var end);
		return Format(element, start) + " - " + Format(element, end);
	}

	private void PaintItem(Graphics g, RectangleF rect, string str, Font font, Color color, StringFormat sf, float scale)
	{
		RectangleF rect2 = new RectangleF(rect.X + 2f * scale, rect.Y + scale, rect.Width - 4f * scale, rect.Height);
		StiDrawing.DrawString(g, str, font, color, rect2, sf);
	}
}
