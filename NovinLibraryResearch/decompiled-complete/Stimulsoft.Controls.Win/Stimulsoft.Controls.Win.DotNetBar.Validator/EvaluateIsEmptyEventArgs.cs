using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public class EvaluateIsEmptyEventArgs : EventArgs
{
	public readonly object Control;

	public readonly object Value;

	public readonly ValidatorBase Validator;

	private bool _IsEmpty;

	internal bool IsEmptySet;

	public bool IsEmpty
	{
		get
		{
			return _IsEmpty;
		}
		set
		{
			_IsEmpty = value;
			IsEmptySet = true;
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
		_IsEmpty = false;
		IsEmptySet = false;
	}
}
