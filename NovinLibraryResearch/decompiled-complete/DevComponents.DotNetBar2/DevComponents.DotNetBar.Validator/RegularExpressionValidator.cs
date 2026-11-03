using System.ComponentModel;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public class RegularExpressionValidator : ValidatorBase
{
	private string __3018 = "";

	private bool __3019;

	[Editor("DevComponents.DotNetBar.Design.ValidationExpressionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates regular expression used to validate controls value.")]
	[DefaultValue("")]
	[Category("Behavior")]
	public string ValidationExpression
	{
		get
		{
			return __3018;
		}
		set
		{
			__3018 = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether empty value is considered valid value by the validator")]
	public bool EmptyValueIsValid
	{
		get
		{
			return __3019;
		}
		set
		{
			__3019 = value;
		}
	}

	public RegularExpressionValidator()
	{
	}

	public RegularExpressionValidator(string errorMessage, string validationExpression)
	{
		__3018 = validationExpression;
		ErrorMessage = errorMessage;
	}

	public RegularExpressionValidator(string errorMessage, string validationExpression, bool emptyValueIsValid)
	{
		__3018 = validationExpression;
		ErrorMessage = errorMessage;
		EmptyValueIsValid = emptyValueIsValid;
	}

	public RegularExpressionValidator(string errorMessage, string validationExpression, string optionalValidationGroup, bool emptyValueIsValid)
	{
		__3018 = validationExpression;
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
	}

	public override bool Validate(Control input)
	{
		if (string.IsNullOrEmpty(__3018))
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
		if (__3019 && string.IsNullOrEmpty(text))
		{
			base.LastValidationResult = true;
			return true;
		}
		Regex regex = new Regex(__3018);
		base.LastValidationResult = regex.IsMatch(text);
		return base.LastValidationResult;
	}
}
