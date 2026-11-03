using System;
using System.ComponentModel;

namespace DevComponents.Editors.DateTimeAdv;

public class HourPeriodLabel : VisualLabel
{
	private NumericHourInput __2536;

	private string __254D = "";

	private string __254E = "";

	private bool __254F;

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
			}
			__2536 = value;
			if (__2536 != null)
			{
				__2536.ValueChanged += _2550;
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

	private void _2550(object _٠, EventArgs ؿ)
	{
		if (__2536 == null)
		{
			return;
		}
		string text = "";
		if (!__2536.IsEmpty)
		{
			text = ((__2536.Period == eHourPeriod.AM) ? ((__254D == null || __254D.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.AMDesignator : __254D) : ((__254E == null || __254E.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.PMDesignator : __254E));
			if (__254F && text.Length > 0)
			{
				text = text[0].ToString();
			}
		}
		base.Text = text;
	}
}
