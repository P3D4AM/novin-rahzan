using System;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class NumericYearInput : VisualIntegerInput, IDateTimePartInput
{
	private eYearDisplayFormat _YearDisplayFormat;

	[DefaultValue(eYearDisplayFormat.FourDigit)]
	public eYearDisplayFormat YearDisplayFormat
	{
		get
		{
			return _YearDisplayFormat;
		}
		set
		{
			if (_YearDisplayFormat != value)
			{
				_YearDisplayFormat = value;
				InvalidateArrange();
			}
		}
	}

	eDateTimePart IDateTimePartInput.Part => eDateTimePart.Year;

	public NumericYearInput()
	{
		DateTime minValue = DateTime.MinValue;
		base.MinValue = minValue.Year;
		minValue = DateTime.MaxValue;
		base.MaxValue = minValue.Year;
	}

	protected override string GetMeasureString()
	{
		return GetFormattedYear(base.GetMeasureString());
	}

	protected override string GetRenderString()
	{
		return GetFormattedYear(base.GetRenderString());
	}

	private string GetFormattedYear(string s)
	{
		if (base.IsFocused || s.Length < 4 || _YearDisplayFormat == eYearDisplayFormat.FourDigit)
		{
			return s;
		}
		if (_YearDisplayFormat == eYearDisplayFormat.TwoDigit)
		{
			return s.Substring(2);
		}
		return s.Substring(3);
	}

	protected override void InputComplete(bool sendNotification)
	{
		UpdateYearValue();
		base.InputComplete(sendNotification);
	}

	protected override void OnInputLostFocus()
	{
		UpdateYearValue();
		base.OnInputLostFocus();
	}

	private void UpdateYearValue()
	{
		if (!IsEmpty && Value < 100)
		{
			Value = int.Parse(DateTime.Now.Year.ToString().Substring(0, 2) + Value.ToString("00"));
		}
	}
}
