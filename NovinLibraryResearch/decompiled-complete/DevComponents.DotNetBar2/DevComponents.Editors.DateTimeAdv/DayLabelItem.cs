using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv;

public class DayLabelItem : VisualLabel, IDateTimePartInput
{
	private int __2643 = -1;

	private List<string> __25D8;

	private bool __269F;

	[DefaultValue(-1)]
	public int Day
	{
		get
		{
			return __2643;
		}
		set
		{
			if (__2643 != value)
			{
				if (__2643 < -1 || __2643 > 6)
				{
					throw new ArgumentException("Day must be value between -1 and 6");
				}
				__2643 = value;
				_26A0();
			}
		}
	}

	public List<string> DayNames
	{
		get
		{
			return __25D8;
		}
		set
		{
			if (value != null && value.Count != 7)
			{
				throw new ArgumentException("DayNames must have exactly 7 items in collection.");
			}
			__25D8 = value;
		}
	}

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
				_26A1();
			}
		}
	}

	int IDateTimePartInput._269B
	{
		get
		{
			return __2643;
		}
		set
		{
			Day = value;
		}
	}

	int IDateTimePartInput._269C
	{
		get
		{
			return 0;
		}
		set
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

	int IDateTimePartInput._269D
	{
		get
		{
			return 6;
		}
		set
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.DayName;

	bool IDateTimePartInput._269E
	{
		get
		{
			return __2643 == -1;
		}
		set
		{
			Day = -1;
		}
	}

	private void _26A0()
	{
		_26A1();
	}

	private void _26A1()
	{
		if (__2643 == -1)
		{
			base.Text = "";
		}
		else if (__25D8 != null)
		{
			base.Text = __25D8[__2643];
		}
		else if (__269F)
		{
			base.Text = DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedDayNames[__2643];
		}
		else
		{
			base.Text = DateTimeInput.GetActiveCulture().DateTimeFormat.DayNames[__2643];
		}
	}

	private void _26A4()
	{
	}

	void IDateTimePartInput.UndoInput()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ⚤
		this._26A4();
	}
}
