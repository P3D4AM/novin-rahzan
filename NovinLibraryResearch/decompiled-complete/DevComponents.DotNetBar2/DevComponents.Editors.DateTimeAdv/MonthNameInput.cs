using System.Collections.Generic;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv;

public class MonthNameInput : VisualStringListInput, IDateTimePartInput
{
	private List<string> __26A5;

	private int __234A = 1;

	private int __234B = 12;

	private bool __269F;

	int IDateTimePartInput._269B
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

	int IDateTimePartInput._269C
	{
		get
		{
			return __234A;
		}
		set
		{
			if (__234A != value)
			{
				__234A = value;
				if (!IsEmpty && base.SelectedIndex < __234A)
				{
					base.SelectedIndex = __234A;
				}
			}
		}
	}

	int IDateTimePartInput._269D
	{
		get
		{
			return __234B;
		}
		set
		{
			if (__234B != value)
			{
				__234B = value;
				if (!IsEmpty && base.SelectedIndex > __234B)
				{
					base.SelectedIndex = __234B;
				}
			}
		}
	}

	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.Month;

	[DefaultValue(false)]
	public bool UseAbbreviatedNames
	{
		get
		{
			return __269F;
		}
		set
		{
			if (__269F != value)
			{
				__269F = value;
				__26A5 = null;
				InvalidateArrange();
			}
		}
	}

	protected override List<string> GetItems()
	{
		if (base.Items.Count != 12)
		{
			if (__26A5 == null)
			{
				__26A5 = new List<string>(12);
				if (__269F)
				{
					__26A5.AddRange(DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedMonthNames);
				}
				else
				{
					__26A5.AddRange(DateTimeInput.GetActiveCulture().DateTimeFormat.MonthNames);
				}
				if (__26A5.Count == 13 && __26A5[12] == "")
				{
					__26A5.RemoveAt(12);
				}
			}
			return __26A5;
		}
		return base.GetItems();
	}

	protected override bool ValidateNewInputStack(string s)
	{
		if (s.Length > 0)
		{
			int result = 0;
			int.TryParse(s, out result);
			if (result > 0 && result >= __234A && result <= __234B)
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
			if (num < __234A || num > __234B)
			{
				return false;
			}
		}
		return flag;
	}
}
