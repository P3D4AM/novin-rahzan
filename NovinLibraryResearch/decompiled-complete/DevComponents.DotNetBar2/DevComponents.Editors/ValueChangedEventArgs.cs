using System;

namespace DevComponents.Editors;

public class ValueChangedEventArgs : EventArgs
{
	private double _ᗸ;

	private string __232B;

	public string SValue => __232B;

	public double Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			_ᗸ = value;
		}
	}

	public ValueChangedEventArgs(string svalue, double value)
	{
		_ᗸ = value;
		__232B = svalue;
	}
}
