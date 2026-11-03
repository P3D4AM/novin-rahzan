namespace DevComponents.Editors.DateTimeAdv;

public class NumericHourInput : VisualIntegerInput, IDateTimePartInput
{
	private bool __2552 = true;

	private eHourPeriod __2553;

	public bool Is24HourFormat
	{
		get
		{
			return __2552;
		}
		set
		{
			if (__2552 != value)
			{
				__2552 = value;
				_2554();
			}
		}
	}

	public eHourPeriod Period
	{
		get
		{
			return __2553;
		}
		set
		{
			if (__2553 != value)
			{
				__2553 = value;
				if (!__2552)
				{
					_2555();
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
			if (!__2552)
			{
				if (value == 0)
				{
					value = 12;
					__2553 = eHourPeriod.AM;
				}
				else if (value > 12)
				{
					__2553 = eHourPeriod.PM;
					value -= 12;
				}
				_2555();
			}
			base.Value = value;
		}
	}

	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.Hour;

	public NumericHourInput()
	{
		_2555();
	}

	private void _2554()
	{
		if (!__2552 && Value > 12)
		{
			Value = Value;
		}
		_2555();
	}

	private void _2555()
	{
		if (__2552)
		{
			base.MinValue = 0;
			base.MaxValue = 23;
		}
		else if (__2553 == eHourPeriod.AM)
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
		if (__2552)
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
		if (!__2552 && s.Length > 0)
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
			__2553 = eHourPeriod.AM;
			_2555();
		}
		base.OnIsEmptyChanged();
	}
}
