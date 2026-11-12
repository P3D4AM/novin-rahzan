using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public struct ValidatorControlPair
{
	public readonly ValidatorBase Validator;

	public readonly Control Control;

	public ValidatorControlPair(ValidatorBase validator, Control control)
	{
		Validator = validator;
		Control = control;
	}
}
