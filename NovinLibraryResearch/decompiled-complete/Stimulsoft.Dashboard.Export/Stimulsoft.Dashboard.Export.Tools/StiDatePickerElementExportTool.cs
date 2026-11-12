using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiDatePickerElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		StiDatePickerElement stiDatePickerElement = (StiDatePickerElement)element;
		if (stiDatePickerElement.GetValueMeter() == null)
		{
			return;
		}
		Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
		Color backColor = StiDashboardStyleHelper.GetBackColor(element);
		CultureInfo cultureInfo = null;
		try
		{
			if (element.Report != null && !string.IsNullOrWhiteSpace(element.Report.Culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(element.Report.Culture);
			}
		}
		finally
		{
			destination.Components.Add(new StiText(rect)
			{
				Name = element.Name + "_Content",
				AllowHtmlTags = true,
				VertAlignment = StiVertAlignment.Center,
				TextBrush = new StiSolidBrush(foreColor),
				Brush = new StiSolidBrush(backColor),
				Text = GetDateTimeString(stiDatePickerElement)
			});
			if (cultureInfo != null)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo;
			}
		}
	}

	private string GetDateTimeString(StiDatePickerElement element)
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
		if (element.UserFilters.Any())
		{
			string text = Format(element, element.UserFilters[0].Value);
			if (element.SelectionMode == StiDateSelectionMode.AutoRange || element.SelectionMode == StiDateSelectionMode.Range)
			{
				text = text + " - " + Format(element, element.UserFilters[0].Value2);
			}
			return text;
		}
		if (element.SelectionMode == StiDateSelectionMode.AutoRange)
		{
			return GetAutoRangeColumnString(element);
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

	private string GetAutoRangeColumnString(StiDatePickerElement element)
	{
		StiDataTable stiDataTable = StiElementDataCache.TryToGetOrCreate(element);
		if (stiDataTable == null)
		{
			return string.Empty;
		}
		IEnumerable<object> enumerable = (from r in stiDataTable?.Rows
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
}
