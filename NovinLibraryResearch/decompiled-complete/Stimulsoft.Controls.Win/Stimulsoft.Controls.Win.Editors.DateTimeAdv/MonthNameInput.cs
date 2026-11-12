using System.Collections.Generic;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class MonthNameInput : VisualStringListInput, IDateTimePartInput
{
	private List<string> _Months;

	private int _MinValue = 1;

	private int _MaxValue = 12;

	private bool _UseAbbreviatedNames;

	int IDateTimePartInput.Value
	{
		get
		{
			return base.SelectedIndex + 1;
		}
		set
		{
			base.SelectedIndex = value - 1;
		}
	}

	int IDateTimePartInput.MinValue
	{
		get
		{
			return _MinValue;
		}
		set
		{
			if (_MinValue != value)
			{
				_MinValue = value;
				if (!IsEmpty && base.SelectedIndex < _MinValue)
				{
					base.SelectedIndex = _MinValue;
				}
			}
		}
	}

	int IDateTimePartInput.MaxValue
	{
		get
		{
			return _MaxValue;
		}
		set
		{
			if (_MaxValue != value)
			{
				_MaxValue = value;
				if (!IsEmpty && base.SelectedIndex > _MaxValue)
				{
					base.SelectedIndex = _MaxValue;
				}
			}
		}
	}

	eDateTimePart IDateTimePartInput.Part => eDateTimePart.Month;

	[DefaultValue(false)]
	public bool UseAbbreviatedNames
	{
		get
		{
			return _UseAbbreviatedNames;
		}
		set
		{
			if (_UseAbbreviatedNames != value)
			{
				_UseAbbreviatedNames = value;
				_Months = null;
				InvalidateArrange();
			}
		}
	}

	protected override List<string> GetItems()
	{
		if (base.Items.Count != 12)
		{
			if (_Months == null)
			{
				_Months = new List<string>(12);
				if (_UseAbbreviatedNames)
				{
					_Months.AddRange(DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedMonthNames);
				}
				else
				{
					_Months.AddRange(DateTimeInput.GetActiveCulture().DateTimeFormat.MonthNames);
				}
				if (_Months.Count == 13 && _Months[12] == "")
				{
					_Months.RemoveAt(12);
				}
			}
			return _Months;
		}
		return base.GetItems();
	}

	protected override bool ValidateNewInputStack(string s)
	{
		if (s.Length > 0)
		{
			int result = 0;
			int.TryParse(s, out result);
			if (result > 0 && result >= _MinValue && result <= _MaxValue)
			{
				List<string> items = GetItems();
				base.LastMatch = items[result - 1];
				base.LastValidatedInputStack = s;
				if (result > 1)
				{
					base.LastMatchComplete = true;
				}
				return true;
			}
		}
		bool flag = base.ValidateNewInputStack(s);
		if (flag && base.LastValidatedInputStack.Length > 0)
		{
			List<string> items2 = GetItems();
			int num = items2.IndexOf(base.LastMatch) + 1;
			if (num < _MinValue || num > _MaxValue)
			{
				return false;
			}
		}
		return flag;
	}
}
