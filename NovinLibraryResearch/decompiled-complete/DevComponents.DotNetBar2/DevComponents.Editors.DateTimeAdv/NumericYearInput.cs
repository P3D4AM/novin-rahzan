using System;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv;

public class NumericYearInput : VisualIntegerInput, IDateTimePartInput
{
	private eYearDisplayFormat __26A6;

	[DefaultValue(eYearDisplayFormat.FourDigit)]
	public eYearDisplayFormat YearDisplayFormat
	{
		get
		{
			return __26A6;
		}
		set
		{
			if (__26A6 != value)
			{
				__26A6 = value;
				InvalidateArrange();
			}
		}
	}

	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.Year;

	public NumericYearInput()
	{
		DateTime minValue = DateTime.MinValue;
		base.MinValue = minValue.Year;
		DateTime maxValue = DateTime.MaxValue;
		base.MaxValue = maxValue.Year;
	}

	protected override string GetMeasureString()
	{
		return _26A7(base.GetMeasureString());
	}

	protected override string GetRenderString()
	{
		return _26A7(base.GetRenderString());
	}

	private string _26A7(string ٮ)
	{
		if (base.IsFocused || ٮ.Length < 4 || __26A6 == eYearDisplayFormat.FourDigit)
		{
			return ٮ;
		}
		if (__26A6 == eYearDisplayFormat.TwoDigit)
		{
			return ٮ.Substring(2);
		}
		return ٮ.Substring(3);
	}

	protected override void InputComplete(bool sendNotification)
	{
		_26A8();
		base.InputComplete(sendNotification);
	}

	protected override void OnInputLostFocus()
	{
		_26A8();
		base.OnInputLostFocus();
	}

	private void _26A8()
	{
		if (!IsEmpty && Value < 100)
		{
			Value = int.Parse(DateTime.Now.Year.ToString().Substring(0, 2) + Value.ToString("00"));
		}
	}
}
