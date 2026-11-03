using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public class EvaluateIsEmptyEventArgs : EventArgs
{
	public readonly object Control;

	public readonly object Value;

	public readonly ValidatorBase Validator;

	private bool __22CE;

	internal bool _301B;

	public bool IsEmpty
	{
		get
		{
			return __22CE;
		}
		set
		{
			__22CE = value;
			_301B = true;
		}
	}

	public EvaluateIsEmptyEventArgs(Control control, object value, ValidatorBase validator)
	{
		Control = control;
		Validator = validator;
		Value = value;
	}

	public void ResetIsEmpty()
	{
		__22CE = false;
		_301B = false;
	}
}
