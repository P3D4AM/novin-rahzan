using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiDatePickerElementViewHelper
{
	public static Hashtable GetAutoRangeValues(IStiDatePickerElement datePickerElement)
	{
		if (datePickerElement.SelectionMode == StiDateSelectionMode.AutoRange)
		{
			StiDataTable orCreate = StiElementDataCache.GetOrCreate(datePickerElement);
			if (orCreate == null || orCreate.Rows.Count == 0)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable();
			IEnumerable<object> source = from r in orCreate.Rows
				select r[0] into d
				where d is DateTime
				select d;
			if (source.Any())
			{
				DateTime dateTime = source.Where((object d) => d is DateTime).Cast<DateTime>().Min();
				DateTime dateTime2 = source.Where((object d) => d is DateTime).Cast<DateTime>().Max();
				hashtable["selectionStart"] = dateTime.ToString("MM'/'dd'/'yyyy");
				hashtable["selectionEnd"] = dateTime2.ToString("MM'/'dd'/'yyyy");
				return hashtable;
			}
		}
		return null;
	}

	public static Hashtable GetVariableRangeValues(IStiDatePickerElement datePickerElement)
	{
		if (StiVariableExpressionHelper.GetVariableSpecifiedAsExpression(datePickerElement, datePickerElement.GetValueMeter()?.Expression) is StiVariable stiVariable)
		{
			object obj = ((IStiAppDictionary)datePickerElement.Report.Dictionary).GetVariableValueByName(stiVariable.Name);
			if (stiVariable.InitBy == StiVariableInitBy.Expression && !string.IsNullOrEmpty(obj as string))
			{
				obj = StiReportParser.Parse($"{{{obj}}}", datePickerElement as StiComponent);
			}
			if (obj is DateTimeRange)
			{
				Hashtable hashtable = new Hashtable();
				hashtable["selectionStart"] = StiValueHelper.TryToDateTime(((DateTimeRange)obj).FromObject).ToString("MM'/'dd'/'yyyy");
				hashtable["selectionEnd"] = StiValueHelper.TryToDateTime(((DateTimeRange)obj).ToObject).ToString("MM'/'dd'/'yyyy");
				return hashtable;
			}
		}
		return null;
	}

	public static string GetVariableValue(IStiDatePickerElement datePickerElement)
	{
		if (StiVariableExpressionHelper.GetVariableSpecifiedAsExpression(datePickerElement, datePickerElement.GetValueMeter()?.Expression) is StiVariable stiVariable)
		{
			object obj = ((IStiAppDictionary)datePickerElement.Report.Dictionary).GetVariableValueByName(stiVariable.Name);
			if (stiVariable.InitBy == StiVariableInitBy.Expression && !string.IsNullOrEmpty(obj as string))
			{
				obj = StiReportParser.Parse($"{{{obj}}}", datePickerElement as StiComponent);
			}
			return StiValueHelper.TryToDateTime(obj).ToString("MM'/'dd'/'yyyy");
		}
		return DateTime.Now.ToString("MM'/'dd'/'yyyy");
	}

	public static bool IsVariablePresent(IStiDatePickerElement datePickerElement)
	{
		IStiMeter valueMeter = datePickerElement.GetValueMeter();
		if (valueMeter != null)
		{
			return StiVariableExpressionHelper.IsVariableSpecifiedAsExpression(datePickerElement, valueMeter?.Expression);
		}
		return false;
	}

	public static bool IsRangeVariablePresent(IStiDatePickerElement datePickerElement)
	{
		IStiMeter valueMeter = datePickerElement.GetValueMeter();
		if (valueMeter != null && IsVariablePresent(datePickerElement))
		{
			return Range.IsRangeType((StiVariableExpressionHelper.GetVariableSpecifiedAsExpression(datePickerElement, valueMeter?.Expression) as StiVariable)?.Type);
		}
		return false;
	}

	public static Hashtable GetFormattedValues(StiReport report, StiRequestParams requestParams)
	{
		Hashtable hashtable = requestParams.GetHashtable("dateValues");
		string @string = requestParams.GetString("datePickerElementName");
		IStiDatePickerElement stiDatePickerElement = ((report != null && @string != null) ? (report.Pages.GetComponentByName(@string) as IStiDatePickerElement) : null);
		if (hashtable != null && stiDatePickerElement != null)
		{
			CultureInfo cultureInfo = null;
			try
			{
				Hashtable hashtable2 = new Hashtable();
				if (!string.IsNullOrWhiteSpace(report.Culture))
				{
					cultureInfo = Thread.CurrentThread.CurrentCulture;
					Thread.CurrentThread.CurrentCulture = new CultureInfo(report.Culture);
				}
				if (hashtable["value1"] != null)
				{
					hashtable2["value1"] = StiDashboardElementViewHelper.Format(stiDatePickerElement, StiDataFiltersHelper.TryToDateTime((string)hashtable["value1"]));
				}
				if (hashtable["value2"] != null)
				{
					hashtable2["value2"] = StiDashboardElementViewHelper.Format(stiDatePickerElement, StiDataFiltersHelper.TryToDateTime((string)hashtable["value2"]));
				}
				if (cultureInfo != null)
				{
					Thread.CurrentThread.CurrentCulture = cultureInfo;
				}
				return hashtable2;
			}
			catch
			{
				return hashtable;
			}
		}
		return hashtable;
	}

	public static string GetColumnPath(IStiDatePickerElement datePickerElement)
	{
		return datePickerElement.GetValueMeter()?.Expression;
	}

	public static Hashtable GetSettings(IStiDatePickerElement datePickerElement)
	{
		Hashtable controlElementSettings = StiDashboardElementViewHelper.GetControlElementSettings(datePickerElement);
		controlElementSettings["itemHeight"] = StiElementConsts.ComboBox.ItemHeight;
		return controlElementSettings;
	}
}
