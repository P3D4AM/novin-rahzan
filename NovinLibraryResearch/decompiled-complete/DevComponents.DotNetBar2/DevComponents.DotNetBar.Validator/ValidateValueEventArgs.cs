using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public class ValidateValueEventArgs : EventArgs
{
	public readonly Control ControlToValidate;

	public bool IsValid;

	public ValidateValueEventArgs(Control controlToValidate)
	{
		ControlToValidate = controlToValidate;
	}
}
