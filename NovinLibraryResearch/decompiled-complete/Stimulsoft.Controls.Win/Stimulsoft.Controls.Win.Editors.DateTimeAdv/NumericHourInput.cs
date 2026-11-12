namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class NumericHourInput : VisualIntegerInput, IDateTimePartInput
{
	private bool _Is24HourFormat = true;

	private eHourPeriod _Period;

	public bool Is24HourFormat
	{
		get
		{
			return _Is24HourFormat;
		}
		set
		{
			if (_Is24HourFormat != value)
			{
				_Is24HourFormat = value;
				OnIs24HourFormatChanged();
			}
		}
	}

	public eHourPeriod Period
	{
		get
		{
			return _Period;
		}
		set
		{
			if (_Period != value)
			{
				_Period = value;
				if (!_Is24HourFormat)
				{
					UpdateRange();
					Value = Value;
				}
			}
		}
	}

	public override int Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			if (!_Is24HourFormat)
			{
				if (value == 0)
				{
					value = 12;
					_Period = eHourPeriod.AM;
				}
				else if (value > 12)
				{
					_Period = eHourPeriod.PM;
					value -= 12;
				}
				UpdateRange();
			}
			base.Value = value;
		}
	}

	eDateTimePart IDateTimePartInput.Part => eDateTimePart.Hour;

	public NumericHourInput()
	{
		UpdateRange();
	}

	private void OnIs24HourFormatChanged()
	{
		if (!_Is24HourFormat && Value > 12)
		{
			Value = Value;
		}
		UpdateRange();
	}

	private void UpdateRange()
	{
		if (_Is24HourFormat)
		{
			base.MinValue = 0;
			base.MaxValue = 23;
		}
		else if (_Period == eHourPeriod.AM)
		{
			base.MinValue = 1;
			base.MaxValue = 12;
		}
		else
		{
			base.MinValue = 1;
			base.MaxValue = 12;
		}
	}

	protected override void OnValueChanged()
	{
		if (_Is24HourFormat)
		{
			if (Value >= 0 && Value <= 12)
			{
				Period = eHourPeriod.AM;
			}
			else
			{
				Period = eHourPeriod.PM;
			}
		}
		base.OnValueChanged();
	}

	protected override bool ValidateNewInputStack(string s)
	{
		if (!_Is24HourFormat && s.Length > 0)
		{
			int result = 0;
			if (int.TryParse(s, out result) && result > 12 && result < 24)
			{
				SetInputStack("");
				SetInputPosition(0);
				Value = result;
				return false;
			}
		}
		return base.ValidateNewInputStack(s);
	}

	protected override void OnIsEmptyChanged()
	{
		if (IsEmpty)
		{
			_Period = eHourPeriod.AM;
			UpdateRange();
		}
		base.OnIsEmptyChanged();
	}
}
