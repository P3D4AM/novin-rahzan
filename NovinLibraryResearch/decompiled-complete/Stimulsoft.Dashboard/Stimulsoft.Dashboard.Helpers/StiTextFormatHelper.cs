using System;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Helpers;

public static class StiTextFormatHelper
{
	public static StiGeneralFormatService DefaultGeneralFormat { get; } = new StiGeneralFormatService();


	public static StiCurrencyFormatService DefaultCurrencyFormat { get; } = new StiCurrencyFormatService();


	public static StiPercentageFormatService DefaultPercentageFormat { get; } = new StiPercentageFormatService
	{
		DecimalDigits = 0,
		GroupSize = 100,
		PositivePattern = 1,
		NegativePattern = 1,
		State = (Stimulsoft.Report.Components.StiTextFormatState.DecimalDigits | Stimulsoft.Report.Components.StiTextFormatState.GroupSize | Stimulsoft.Report.Components.StiTextFormatState.PositivePattern | Stimulsoft.Report.Components.StiTextFormatState.NegativePattern)
	};


	private static StiNumberFormatService DefaultNumberFormat { get; } = new StiNumberFormatService();


	private static StiNumberFormatService DefaultIntegerFormat { get; } = new StiNumberFormatService
	{
		UseLocalSetting = false,
		DecimalDigits = 0
	};


	private static StiDateFormatService DefaultDateFormat { get; } = new StiDateFormatService();


	private static StiBooleanFormatService DefaultBooleanFormat { get; } = new StiBooleanFormatService();


	public static string FormatBasedOnColumnType(StiComponent component, StiTableColumn column, object value)
	{
		return FormatBasedOnColumnType(component, column.TextFormat, column, value);
	}

	public static string FormatBasedOnColumnType(StiComponent component, StiFormatService formatService, IStiMeter column, object value)
	{
		CultureInfo culture = StiCultureHelper.Set(component?.Report);
		try
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (column == null)
			{
				return value.ToString();
			}
			if (StiExtValueConverter.IsEligable(value, component?.Report?.Culture))
			{
				return StiExtValueConverter.Convert(value, component?.Report?.Culture);
			}
			if (formatService == null || formatService is StiGeneralFormatService)
			{
				Type type = value.GetType();
				if (type != null)
				{
					if (type.IsDateType())
					{
						return DefaultDateFormat.Format(value);
					}
					if ((type.IsNumericType() || type.IsIntegerType()) && StiMoneyNameHelper.IsMoneyName(column.Label))
					{
						return DefaultCurrencyFormat.Format(value);
					}
					if (type.IsIntegerType())
					{
						return DefaultIntegerFormat.Format(value);
					}
					if (type.IsNumericType())
					{
						return DefaultNumberFormat.Format(value);
					}
					if (type == typeof(bool))
					{
						return DefaultBooleanFormat.Format(value);
					}
				}
			}
			return formatService.Format(value);
		}
		catch
		{
			return value.ToString();
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	public static string Format(StiReport report, StiFormatService format, object value)
	{
		CultureInfo culture = StiCultureHelper.Set(report);
		try
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (format != null)
			{
				return format.Format(value);
			}
			return value.ToString();
		}
		catch
		{
			return value.ToString();
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	public static string FormatAsPercentage(StiReport report, object value)
	{
		CultureInfo culture = StiCultureHelper.Set(report);
		try
		{
			if (value == null)
			{
				return string.Empty;
			}
			return DefaultPercentageFormat.Format(value);
		}
		catch
		{
			return value.ToString();
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	public static string FormatAsPercentage(StiReport report, StiFormatService format, object value)
	{
		CultureInfo culture = StiCultureHelper.Set(report);
		try
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (format == null || format is StiGeneralFormatService)
			{
				return DefaultPercentageFormat.Format(value);
			}
			return format.Format(value);
		}
		catch
		{
			return value.ToString();
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	internal static StiFormatService GetDefaultFormatForColumn(IStiAppDataCell cell)
	{
		Type type = StiMeterHelper.ToDataType(cell);
		if (type == null)
		{
			return DefaultGeneralFormat;
		}
		if (type.IsNumericType())
		{
			if (cell is StiDataColumn && StiMoneyNameHelper.IsMoneyName((cell as StiDataColumn).NameInSource))
			{
				return DefaultCurrencyFormat;
			}
			if (type.IsIntegerType())
			{
				return DefaultIntegerFormat;
			}
			return DefaultNumberFormat;
		}
		if (type.IsDateType())
		{
			return DefaultDateFormat;
		}
		if (type == typeof(bool))
		{
			return DefaultBooleanFormat;
		}
		return DefaultGeneralFormat;
	}
}
