using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public class ValidatorGetValueEventArgs : EventArgs
{
	public readonly Control Control;

	public readonly ValidatorBase Validator;

	private object _ᗸ;

	internal bool _2FE4;

	public object Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			_ᗸ = value;
			_2FE4 = true;
		}
	}

	public ValidatorGetValueEventArgs(Control control, ValidatorBase validator)
	{
		Control = control;
		Validator = validator;
	}

	public void ResetValue()
	{
		_ᗸ = null;
		_2FE4 = false;
	}
}
