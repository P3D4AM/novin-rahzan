using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Dashboard.Components.DatePicker;

public static class StiDatePickerHelper
{
	public static void CalculateRangeFromInitial(StiInitialDateRangeSelection selection, out DateTime start, out DateTime end)
	{
		switch (selection)
		{
		case StiInitialDateRangeSelection.DayTomorrow:
			start = DateTime.Today.AddDays(1.0);
			end = DateTime.Today.AddDays(1.0);
			break;
		case StiInitialDateRangeSelection.DayToday:
			start = DateTime.Today;
			end = DateTime.Today;
			break;
		case StiInitialDateRangeSelection.DayYesterday:
			start = DateTime.Today.AddDays(-1.0);
			end = DateTime.Today.AddDays(-1.0);
			break;
		case StiInitialDateRangeSelection.Last7Days:
			start = DateTime.Today.AddDays(-7.0);
			end = DateTime.Today;
			break;
		case StiInitialDateRangeSelection.Last14Days:
			start = DateTime.Today.AddDays(-14.0);
			end = DateTime.Today;
			break;
		case StiInitialDateRangeSelection.Last30Days:
			start = DateTime.Today.AddDays(-30.0);
			end = DateTime.Today;
			break;
		case StiInitialDateRangeSelection.WeekNext:
			start = DateTime.Today.AddDays(7.0).FirstDayOfWeek();
			end = DateTime.Today.AddDays(7.0).LastDayOfWeek();
			break;
		case StiInitialDateRangeSelection.WeekCurrent:
			start = DateTime.Today.FirstDayOfWeek();
			end = DateTime.Today.LastDayOfWeek();
			break;
		case StiInitialDateRangeSelection.WeekPrevious:
			start = DateTime.Today.AddDays(-7.0).FirstDayOfWeek();
			end = DateTime.Today.AddDays(-7.0).LastDayOfWeek();
			break;
		case StiInitialDateRangeSelection.MonthNext:
			start = DateTime.Today.AddMonths(1).FirstDayOfMonth();
			end = DateTime.Today.AddMonths(1).LastDayOfMonth();
			break;
		case StiInitialDateRangeSelection.MonthCurrent:
			start = DateTime.Today.FirstDayOfMonth();
			end = DateTime.Today.LastDayOfMonth();
			break;
		case StiInitialDateRangeSelection.MonthPrevious:
			start = DateTime.Today.AddMonths(-1).FirstDayOfMonth();
			end = DateTime.Today.AddMonths(-1).LastDayOfMonth();
			break;
		case StiInitialDateRangeSelection.QuarterNext:
			start = DateTime.Today.AddMonths(3).FirstDayOfQuarter();
			end = DateTime.Today.AddMonths(3).LastDayOfQuarter();
			break;
		case StiInitialDateRangeSelection.QuarterCurrent:
			start = DateTime.Today.FirstDayOfQuarter();
			end = DateTime.Today.LastDayOfQuarter();
			break;
		case StiInitialDateRangeSelection.QuarterPrevious:
			start = DateTime.Today.AddMonths(-3).FirstDayOfQuarter();
			end = DateTime.Today.AddMonths(-3).LastDayOfQuarter();
			break;
		case StiInitialDateRangeSelection.QuarterFirst:
			start = DateTime.Today.FirstDayOfFirthQuarter();
			end = DateTime.Today.LastDayOfFirthQuarter();
			break;
		case StiInitialDateRangeSelection.QuarterSecond:
			start = DateTime.Today.FirstDayOfSecondQuarter();
			end = DateTime.Today.LastDayOfSecondQuarter();
			break;
		case StiInitialDateRangeSelection.QuarterThird:
			start = DateTime.Today.FirstDayOfThirdQuarter();
			end = DateTime.Today.LastDayOfThirdQuarter();
			break;
		case StiInitialDateRangeSelection.QuarterFourth:
			start = DateTime.Today.FirstDayOfFourthQuarter();
			end = DateTime.Today.LastDayOfFourthQuarter();
			break;
		case StiInitialDateRangeSelection.YearNext:
			start = DateTime.Today.AddYears(1).FirstDayOfYear();
			end = DateTime.Today.AddYears(1).LastDayOfYear();
			break;
		case StiInitialDateRangeSelection.YearCurrent:
			start = DateTime.Today.FirstDayOfYear();
			end = DateTime.Today.LastDayOfYear();
			break;
		case StiInitialDateRangeSelection.YearPrevious:
			start = DateTime.Today.AddYears(-1).FirstDayOfYear();
			end = DateTime.Today.AddYears(-1).LastDayOfYear();
			break;
		case StiInitialDateRangeSelection.DateToWeek:
			start = DateTime.Today.FirstDayOfWeek();
			end = DateTime.Today;
			break;
		case StiInitialDateRangeSelection.DateToMonth:
			start = DateTime.Today.FirstDayOfMonth();
			end = DateTime.Today;
			break;
		case StiInitialDateRangeSelection.DateToQuarter:
			start = DateTime.Today.FirstDayOfQuarter();
			end = DateTime.Today;
			break;
		case StiInitialDateRangeSelection.DateToYear:
			start = DateTime.Today.FirstDayOfYear();
			end = DateTime.Today;
			break;
		default:
			start = DateTime.Now;
			end = DateTime.Now;
			break;
		}
	}

	public static List<StiDataFilterRule> FetchDefaultUserFilters(StiDatePickerElement datePickerElement)
	{
		string valueMeterExpression = GetValueMeterExpression(datePickerElement);
		if (datePickerElement.SelectionMode == StiDateSelectionMode.Single)
		{
			return GetSingleDefaultUserFilters(datePickerElement, valueMeterExpression);
		}
		if (datePickerElement.SelectionMode == StiDateSelectionMode.Range)
		{
			return GetRangeDefaultUserFilters(datePickerElement.InitialRangeSelection, valueMeterExpression);
		}
		if (datePickerElement.SelectionMode == StiDateSelectionMode.AutoRange)
		{
			return GetAutoRangeDefaultUserFilters(datePickerElement, valueMeterExpression);
		}
		return null;
	}

	private static List<StiDataFilterRule> GetSingleDefaultUserFilters(StiDatePickerElement datePickerElement, string columnName)
	{
		string formatedDate = GetFormatedDate(DateTime.Now);
		return new StiDataFilterRule(columnName, GetCondition(datePickerElement), formatedDate).ToList();
	}

	private static List<StiDataFilterRule> GetRangeDefaultUserFilters(StiInitialDateRangeSelection selection, string columnName)
	{
		CalculateRangeFromInitial(selection, out var start, out var end);
		string formatedDate = GetFormatedDate(start);
		string formatedDate2 = GetFormatedDate(end);
		return new StiDataFilterRule("", columnName, StiDataFilterCondition.Between, formatedDate, formatedDate2, isEnabled: true, isExpression: false).ToList();
	}

	private static List<StiDataFilterRule> GetAutoRangeDefaultUserFilters(StiDatePickerElement datePickerElement, string columnName)
	{
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(datePickerElement);
		if (orCreate == null)
		{
			return null;
		}
		IEnumerable<object> enumerable = from r in orCreate.Rows
			select r[0] into d
			where d is DateTime
			select d;
		if (enumerable == null || !enumerable.Any())
		{
			return null;
		}
		DateTime dateTime = enumerable.Where((object d) => d is DateTime).Cast<DateTime>().Min();
		DateTime dateTime2 = enumerable.Where((object d) => d is DateTime).Cast<DateTime>().Max();
		string formatedDate = GetFormatedDate(dateTime);
		string formatedDate2 = GetFormatedDate(dateTime2);
		return new StiDataFilterRule("", columnName, StiDataFilterCondition.Between, formatedDate, formatedDate2, isEnabled: true, isExpression: false).ToList();
	}

	internal static string GetValueMeterExpression(StiDatePickerElement datePickerElement)
	{
		if (datePickerElement.ValueMeter == null)
		{
			return null;
		}
		return datePickerElement.ValueMeter.Expression;
	}

	internal static StiDataFilterCondition GetCondition(StiDatePickerElement datePickerElement)
	{
		return datePickerElement.Condition switch
		{
			StiDateCondition.EqualTo => StiDataFilterCondition.EqualTo, 
			StiDateCondition.NotEqualTo => StiDataFilterCondition.NotEqualTo, 
			StiDateCondition.GreaterThan => StiDataFilterCondition.GreaterThan, 
			StiDateCondition.GreaterThanOrEqualTo => StiDataFilterCondition.GreaterThanOrEqualTo, 
			StiDateCondition.LessThan => StiDataFilterCondition.LessThan, 
			StiDateCondition.LessThanOrEqualTo => StiDataFilterCondition.LessThanOrEqualTo, 
			_ => StiDataFilterCondition.EqualTo, 
		};
	}

	internal static string GetFormatedDate(DateTime dateTime)
	{
		return dateTime.ToString("MM'/'dd'/'yyyy");
	}

	internal static IStiAppVariable GetVariableSpecifiedAsValue(IStiDatePickerElement element)
	{
		return StiVariableExpressionHelper.GetVariableSpecifiedAsExpression(element, element.GetValueMeter()?.Expression);
	}

	internal static bool IsVariableSpecifiedAsValue(IStiDatePickerElement element)
	{
		return StiVariableExpressionHelper.IsVariableSpecifiedAsExpression(element, element.GetValueMeter()?.Expression);
	}

	public static object GetDefaultValue(IStiDatePickerElement element)
	{
		if (GetVariableSpecifiedAsValue(element) is StiVariable stiVariable)
		{
			object obj = ((IStiAppDictionary)element.Report.Dictionary).GetVariableValueByName(stiVariable.Name);
			if (stiVariable.InitBy == StiVariableInitBy.Expression && !string.IsNullOrEmpty(obj as string))
			{
				obj = StiParser.ParseTextValue($"{{{obj}}}", element as StiComponent);
			}
			if (stiVariable.Type == typeof(DateTime) || stiVariable.Type == typeof(DateTime?))
			{
				return StiValueHelper.TryToDateTime(obj);
			}
			if (stiVariable.Type == typeof(DateTimeRange))
			{
				if (!(obj is DateTimeRange))
				{
					return new DateTimeRange();
				}
				return obj;
			}
		}
		return DateTime.Now;
	}
}
