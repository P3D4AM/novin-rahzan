using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class DayLabelItem : VisualLabel, IDateTimePartInput
{
	private int _Day = -1;

	private List<string> _DayNames;

	private bool _UseAbbreviatedNames;

	[DefaultValue(-1)]
	public int Day
	{
		get
		{
			return _Day;
		}
		set
		{
			if (_Day != value)
			{
				if (_Day < -1 || _Day > 6)
				{
					throw new ArgumentException("Day must be value between -1 and 6");
				}
				_Day = value;
				OnDayChanged();
			}
		}
	}

	public List<string> DayNames
	{
		get
		{
			return _DayNames;
		}
		set
		{
			if (value != null && value.Count != 7)
			{
				throw new ArgumentException("DayNames must have exactly 7 items in collection.");
			}
			_DayNames = value;
		}
	}

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
				UpdateLabelText();
			}
		}
	}

	int IDateTimePartInput.Value
	{
		get
		{
			return _Day;
		}
		set
		{
			Day = value;
		}
	}

	int IDateTimePartInput.MinValue
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

	int IDateTimePartInput.MaxValue
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

	eDateTimePart IDateTimePartInput.Part => eDateTimePart.DayName;

	bool IDateTimePartInput.IsEmpty
	{
		get
		{
			return _Day == -1;
		}
		set
		{
			Day = -1;
		}
	}

	private void OnDayChanged()
	{
		UpdateLabelText();
	}

	private void UpdateLabelText()
	{
		if (_Day == -1)
		{
			base.Text = "";
		}
		else if (_DayNames != null)
		{
			base.Text = _DayNames[_Day];
		}
		else if (_UseAbbreviatedNames)
		{
			base.Text = DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedDayNames[_Day];
		}
		else
		{
			base.Text = DateTimeInput.GetActiveCulture().DateTimeFormat.DayNames[_Day];
		}
	}

	void IDateTimePartInput.UndoInput()
	{
	}
}
