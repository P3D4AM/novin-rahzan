using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.RequiredFieldValidatorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
[Localizable(true)]
public class RequiredFieldValidator : ValidatorBase
{
	private bool _IsEmptyStringValid;

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether empty string of zero length is considered valid input.")]
	public bool IsEmptyStringValid
	{
		get
		{
			return _IsEmptyStringValid;
		}
		set
		{
			_IsEmptyStringValid = value;
		}
	}

	public event EvaluateIsEmptyEventHandler EvaluateIsEmpty;

	public RequiredFieldValidator()
	{
	}

	public RequiredFieldValidator(string errorMessage)
	{
		ErrorMessage = errorMessage;
	}

	public RequiredFieldValidator(string errorMessage, string optionalValidationGroup)
	{
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
	}

	public override bool Validate(Control input)
	{
		object controlValue = GetControlValue(input);
		base.LastValidationResult = !IsEmpty(input, controlValue);
		return base.LastValidationResult;
	}

	protected virtual bool IsEmpty(Control input, object value)
	{
		EvaluateIsEmptyEventArgs evaluateIsEmptyEventArgs = new EvaluateIsEmptyEventArgs(input, value, this);
		OnEvaluateIsEmpty(evaluateIsEmptyEventArgs);
		if (evaluateIsEmptyEventArgs.IsEmptySet)
		{
			return evaluateIsEmptyEventArgs.IsEmpty;
		}
		if (value == null)
		{
			return true;
		}
		if (value is string)
		{
			if (_IsEmptyStringValid)
			{
				return value == null;
			}
			return string.IsNullOrEmpty((string)value);
		}
		return false;
	}

	protected virtual void OnEvaluateIsEmpty(EvaluateIsEmptyEventArgs args)
	{
		this.EvaluateIsEmpty?.Invoke(this, args);
	}
}
