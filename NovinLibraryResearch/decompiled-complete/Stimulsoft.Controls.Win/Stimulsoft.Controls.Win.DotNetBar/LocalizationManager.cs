using System;
using System.ComponentModel;
using System.Resources;

namespace Stimulsoft.Controls.Win.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public class LocalizationManager : IDisposable
{
	public LocalizationManager()
		: this(null)
	{
	}

	public LocalizationManager(IOwnerLocalize manager)
	{
	}

	public void Dispose()
	{
	}

	public string GetDefaultLocalizedString(string key)
	{
		string text = GetLocalizedString(key);
		if (text == "" || text == null)
		{
			ResourceManager resourceManager = BarFunctions.GetResourceManager(bDefault: true);
			text = resourceManager.GetString(key);
		}
		if (text == null)
		{
			text = "";
		}
		return text;
	}

	public static string GetLocalizedString(string key, string defaultValue)
	{
		LocalizeEventArgs localizeEventArgs = new LocalizeEventArgs();
		localizeEventArgs.Key = key;
		localizeEventArgs.LocalizedValue = defaultValue;
		LocalizationKeys.InvokeLocalizeString(localizeEventArgs);
		if (localizeEventArgs.Handled)
		{
			return localizeEventArgs.LocalizedValue;
		}
		return defaultValue;
	}

	public string GetLocalizedString(string key)
	{
		return key switch
		{
			"qat_customizetooltip" => "qat_customizetooltip", 
			"ClickAndDragPickColor" => StiLocalization.Get("Gui", "ClickAndDragPickColor"), 
			"TimeSelectorOkButton" => StiLocalization.Get("Buttons", "Ok"), 
			"TimeSelectorClearButton" => StiLocalization.Get("Gui", "monthcalendar_clearbutton"), 
			"SuperGrid_FilterShowAll" => "Show All", 
			"SuperGrid_FilterShowNull" => "Show Null", 
			"SuperGrid_FilterShowNotnull" => "Show Not Null", 
			"SuperGrid_FilterCustom" => "Custom", 
			"SuperGrid_FilterEnterExpr" => "Enter custom filter expression", 
			"SuperGrid_FilterSampleExpr" => "Sample", 
			"SuperGrid_FilterCustomHelp" => "Click the Title Bar help button for syntax help", 
			"SuperGrid_FilterApply" => "Apply", 
			"SuperGrid_FilterOk" => "OK", 
			"SuperGrid_FilterCancel" => "Cancel", 
			"SuperGrid_FilterClose" => "Close", 
			"SuperGrid_FilterClear" => "Clear", 
			"SuperGrid_FilterReset" => "Reset", 
			"SuperGrid_FilterByRelativeDate" => "Filter by Relative Date", 
			"SuperGrid_FilterBySpecificDate" => "Filter by Specific Date", 
			"SuperGrid_FilterByDateRange" => "Filter by Date Range", 
			"SuperGrid_FilterCurrentMonth" => "Current Month", 
			"SuperGrid_FilterCurrentYear" => "Current Year", 
			"SuperGrid_FilterLastMonthPeriod" => "Last Month Period", 
			"SuperGrid_FilterLast3MonthPeriod" => "Last 3 Month Period", 
			"SuperGrid_FilterLast6MonthPeriod" => "Last 6 Month Period", 
			"SuperGridFilter_Last9MonthPeriod" => "Last 9 Month Period", 
			"SuperGrid_FilterLastYear" => "Last Year", 
			"SuperGrid_FilterLast5YearPeriod" => "Last 5 Year Period", 
			"SuperGrid_FilterLast10YearPeriod" => "Last 10 Year Period", 
			"SuperGrid_FilterExprError" => "Error", 
			"SuperGrid_FilterMissingParen" => "Missing Paren", 
			"SuperGrid_FilterMissingQuote" => "Missing Quote", 
			"SuperGrid_FilterInvalidArgCount" => "Invalid Argument Count", 
			"SuperGrid_FilterInvalidArg" => "Invalid Argument", 
			"SuperGrid_FilterInvalidEmptyOp" => "Invalid Empty Operation", 
			"SuperGrid_FilterInvalidDateTimeOp" => "Invalid Date Time Operation", 
			"SuperGrid_FilterInvalidBoolOp" => "Invalid Boolean Operation", 
			"SuperGrid_FilterInvalidStringOp" => "Invalid String Operation", 
			"SuperGrid_FilterInvalidNumericOp" => "Invalid Numeric Operation", 
			"SuperGrid_FilterInvalidEval" => "Invalid Eval", 
			"SuperGrid_FilterUndefinedFunction" => "Undefined Function", 
			"SuperGrid_FilterCustomFilter" => "Custom Filter", 
			"SuperGrid_NewFilterExpr" => "New Filter Expression", 
			"SuperGrid_NewFilter" => "New Filter", 
			"SuperGrid_DeleteFilter" => "Delete Filter", 
			"SuperGrid_FilterName" => "Filter Name:", 
			"SuperGrid_FilterDescription" => "Description", 
			"SuperGrid_ShowInFilterPopup" => "Show in FilterPopup", 
			_ => StiLocalization.Get("Gui", key), 
		};
	}
}
