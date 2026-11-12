using System;

namespace Stimulsoft.Controls.Win.Editors;

public class ValueChangedEventArgs : EventArgs
{
	private double _Value;

	private string _SValue;

	public string SValue => _SValue;

	public double Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
		}
	}

	public ValueChangedEventArgs(string svalue, double value)
	{
		_Value = value;
		_SValue = svalue;
	}
}
