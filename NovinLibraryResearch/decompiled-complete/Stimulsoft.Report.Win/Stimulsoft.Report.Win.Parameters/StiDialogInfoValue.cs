using System;
using System.Globalization;
using System.Threading;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Win.Parameters;

public class StiDialogInfoValue
{
	public object Key { get; set; }

	public string Value { get; set; }

	public object ValueBinding { get; set; }

	public bool IsDateTime { get; set; }

	public CultureInfo Culture { get; set; }

	public string GetString(StiVariable variable = null)
	{
		if (!string.IsNullOrEmpty(Value))
		{
			return Value;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			if (Culture != null)
			{
				Thread.CurrentThread.CurrentCulture = Culture;
			}
			if (Key == null)
			{
				return string.Empty;
			}
			if (IsDateTime && Key is DateTime && variable?.DialogInfo != null)
			{
				string dateTimeFormat = StiDateTimeFormatHelper.GetDateTimeFormat(variable.DialogInfo.DateTimeType);
				if (!string.IsNullOrWhiteSpace(dateTimeFormat))
				{
					return ((DateTime)Key).ToString(dateTimeFormat);
				}
			}
			return Key.ToString();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public override string ToString()
	{
		if (IsDateTime)
		{
			return GetString();
		}
		if (Key != null)
		{
			return Key.ToString();
		}
		return string.Empty;
	}

	public StiDialogInfoValue()
	{
	}

	public StiDialogInfoValue(object key, string value)
	{
		Key = key;
		Value = value;
	}

	public StiDialogInfoValue(object key, string value, object valueBinding)
	{
		Key = key;
		Value = value;
		ValueBinding = valueBinding;
	}

	public StiDialogInfoValue(object key, string value, object valueBinding, CultureInfo culture)
		: this(key, value, valueBinding)
	{
		Culture = culture;
	}
}
