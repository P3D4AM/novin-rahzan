using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv;

public class HourPeriodInput : VisualStringListInput
{
	private NumericHourInput __2536;

	private bool __254F;

	private string __254D = "";

	private string __254E = "";

	public NumericHourInput HourInput
	{
		get
		{
			return __2536;
		}
		set
		{
			if (__2536 != null)
			{
				__2536.ValueChanged -= _2550;
				__2536.IsEmptyChanged -= _2557;
			}
			__2536 = value;
			if (__2536 != null)
			{
				__2536.ValueChanged += _2550;
				__2536.IsEmptyChanged += _2557;
			}
		}
	}

	public bool UseSingleLetterLabel
	{
		get
		{
			return __254F;
		}
		set
		{
			if (__254F != value)
			{
				__254F = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue("")]
	public string AMText
	{
		get
		{
			return __254D;
		}
		set
		{
			__254D = value;
			InvalidateArrange();
		}
	}

	[DefaultValue("")]
	public string PMText
	{
		get
		{
			return __254E;
		}
		set
		{
			__254E = value;
			InvalidateArrange();
		}
	}

	private void _2557(object _٠, EventArgs ؿ)
	{
		if (__2536 != null && __2536.IsEmpty)
		{
			IsEmpty = true;
		}
	}

	private void _2550(object _٠, EventArgs ؿ)
	{
		if (__2536 == null)
		{
			return;
		}
		if (!__2536.IsEmpty)
		{
			if (__2536.Period == eHourPeriod.AM)
			{
				base.SelectedIndex = 0;
			}
			else
			{
				base.SelectedIndex = 1;
			}
		}
		else
		{
			base.SelectedIndex = -1;
		}
	}

	protected override List<string> GetItems()
	{
		List<string> list = new List<string>(2);
		list.Add(_2559());
		list.Add(_2558());
		return list;
	}

	private string _2558()
	{
		string text = "";
		text = ((__254E == null || __254E.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.PMDesignator : __254E);
		if (__254F && text.Length > 0)
		{
			text = text[0].ToString();
		}
		return text;
	}

	private string _2559()
	{
		string text = "";
		text = ((__254D == null || __254D.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.AMDesignator : __254D);
		if (__254F && text.Length > 0)
		{
			text = text[0].ToString();
		}
		return text;
	}

	protected override void OnSelectedIndexChanged(EventArgs eventArgs)
	{
		if (__2536 != null && !__2536.IsEmpty)
		{
			eHourPeriod eHourPeriod = ((base.SelectedIndex == 1) ? eHourPeriod.PM : eHourPeriod.AM);
			if (__2536.Period != eHourPeriod)
			{
				__2536.Period = eHourPeriod;
			}
		}
		base.OnSelectedIndexChanged(eventArgs);
	}
}
