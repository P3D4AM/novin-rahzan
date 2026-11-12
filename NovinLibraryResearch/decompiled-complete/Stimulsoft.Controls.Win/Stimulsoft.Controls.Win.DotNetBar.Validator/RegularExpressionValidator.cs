using System.ComponentModel;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public class RegularExpressionValidator : ValidatorBase
{
	private string _ValidationExpression = "";

	private bool _EmptyValueIsValid;

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates regular expression used to validate controls value.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ValidationExpressionEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string ValidationExpression
	{
		get
		{
			return _ValidationExpression;
		}
		set
		{
			_ValidationExpression = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether empty value is considered valid value by the validator")]
	public bool EmptyValueIsValid
	{
		get
		{
			return _EmptyValueIsValid;
		}
		set
		{
			_EmptyValueIsValid = value;
		}
	}

	public RegularExpressionValidator()
	{
	}

	public RegularExpressionValidator(string errorMessage, string validationExpression)
	{
		_ValidationExpression = validationExpression;
		ErrorMessage = errorMessage;
	}

	public RegularExpressionValidator(string errorMessage, string validationExpression, bool emptyValueIsValid)
	{
		_ValidationExpression = validationExpression;
		ErrorMessage = errorMessage;
		EmptyValueIsValid = emptyValueIsValid;
	}

	public RegularExpressionValidator(string errorMessage, string validationExpression, string optionalValidationGroup, bool emptyValueIsValid)
	{
		_ValidationExpression = validationExpression;
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
	}

	public override bool Validate(Control input)
	{
		if (string.IsNullOrEmpty(_ValidationExpression))
		{
			base.LastValidationResult = true;
			return true;
		}
		object controlValue = GetControlValue(input);
		string text = "";
		if (controlValue != null)
		{
			text = controlValue.ToString();
		}
		if (_EmptyValueIsValid && string.IsNullOrEmpty(text))
		{
			base.LastValidationResult = true;
			return true;
		}
		Regex regex = new Regex(_ValidationExpression);
		base.LastValidationResult = regex.IsMatch(text);
		return base.LastValidationResult;
	}
}
