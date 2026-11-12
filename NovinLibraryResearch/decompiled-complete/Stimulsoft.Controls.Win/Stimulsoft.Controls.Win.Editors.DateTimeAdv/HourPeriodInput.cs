using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class HourPeriodInput : VisualStringListInput
{
	private NumericHourInput _HourInput;

	private bool _UseSingleLetterLabel;

	private string _AMText = "";

	private string _PMText = "";

	public NumericHourInput HourInput
	{
		get
		{
			return _HourInput;
		}
		set
		{
			if (_HourInput != null)
			{
				_HourInput.ValueChanged -= HourChanged;
				_HourInput.IsEmptyChanged -= HourIsEmptyChanged;
			}
			_HourInput = value;
			if (_HourInput != null)
			{
				_HourInput.ValueChanged += HourChanged;
				_HourInput.IsEmptyChanged += HourIsEmptyChanged;
			}
		}
	}

	public bool UseSingleLetterLabel
	{
		get
		{
			return _UseSingleLetterLabel;
		}
		set
		{
			if (_UseSingleLetterLabel != value)
			{
				_UseSingleLetterLabel = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue("")]
	public string AMText
	{
		get
		{
			return _AMText;
		}
		set
		{
			_AMText = value;
			InvalidateArrange();
		}
	}

	[DefaultValue("")]
	public string PMText
	{
		get
		{
			return _PMText;
		}
		set
		{
			_PMText = value;
			InvalidateArrange();
		}
	}

	private void HourIsEmptyChanged(object sender, EventArgs e)
	{
		if (_HourInput != null && _HourInput.IsEmpty)
		{
			IsEmpty = true;
		}
	}

	private void HourChanged(object sender, EventArgs e)
	{
		if (_HourInput == null)
		{
			return;
		}
		if (!_HourInput.IsEmpty)
		{
			if (_HourInput.Period == eHourPeriod.AM)
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
		list.Add(GetAMLabel());
		list.Add(GetPMLabel());
		return list;
	}

	private string GetPMLabel()
	{
		string text = "";
		text = ((_PMText == null || _PMText.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.PMDesignator : _PMText);
		if (_UseSingleLetterLabel && text.Length > 0)
		{
			text = text[0].ToString();
		}
		return text;
	}

	private string GetAMLabel()
	{
		string text = "";
		text = ((_AMText == null || _AMText.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.AMDesignator : _AMText);
		if (_UseSingleLetterLabel && text.Length > 0)
		{
			text = text[0].ToString();
		}
		return text;
	}

	protected override void OnSelectedIndexChanged(EventArgs eventArgs)
	{
		if (_HourInput != null && !_HourInput.IsEmpty)
		{
			eHourPeriod eHourPeriod = ((base.SelectedIndex == 1) ? eHourPeriod.PM : eHourPeriod.AM);
			if (_HourInput.Period != eHourPeriod)
			{
				_HourInput.Period = eHourPeriod;
			}
		}
		base.OnSelectedIndexChanged(eventArgs);
	}
}
