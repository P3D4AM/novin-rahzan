using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public class ValidatorGetValueEventArgs : EventArgs
{
	public readonly Control Control;

	public readonly ValidatorBase Validator;

	private object _Value;

	internal bool ValueSet;

	public object Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
			ValueSet = true;
		}
	}

	public ValidatorGetValueEventArgs(Control control, ValidatorBase validator)
	{
		Control = control;
		Validator = validator;
	}

	public void ResetValue()
	{
		_Value = null;
		ValueSet = false;
	}
}
