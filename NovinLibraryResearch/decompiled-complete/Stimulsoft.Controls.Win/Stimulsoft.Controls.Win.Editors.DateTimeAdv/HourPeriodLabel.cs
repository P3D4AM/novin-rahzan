using System;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class HourPeriodLabel : VisualLabel
{
	private NumericHourInput _HourInput;

	private string _AMText = "";

	private string _PMText = "";

	private bool _UseSingleLetterLabel;

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
			}
			_HourInput = value;
			if (_HourInput != null)
			{
				_HourInput.ValueChanged += HourChanged;
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

	private void HourChanged(object sender, EventArgs e)
	{
		if (_HourInput == null)
		{
			return;
		}
		string text = "";
		if (!_HourInput.IsEmpty)
		{
			text = ((_HourInput.Period == eHourPeriod.AM) ? ((_AMText == null || _AMText.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.AMDesignator : _AMText) : ((_PMText == null || _PMText.Length <= 0) ? DateTimeInput.GetActiveCulture().DateTimeFormat.PMDesignator : _PMText));
			if (_UseSingleLetterLabel && text.Length > 0)
			{
				text = text[0].ToString();
			}
		}
		base.Text = text;
	}
}
