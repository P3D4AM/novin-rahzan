using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public class RangeValidator : ValidatorBase
{
	private object _MaximumValue;

	private object _MinimumValue;

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates maximum value control may have.")]
	[TypeConverter(typeof(StringConverter))]
	public object MaximumValue
	{
		get
		{
			return _MaximumValue;
		}
		set
		{
			_MaximumValue = value;
		}
	}

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates minimum value control may have.")]
	[TypeConverter(typeof(StringConverter))]
	public object MinimumValue
	{
		get
		{
			return _MinimumValue;
		}
		set
		{
			_MinimumValue = value;
		}
	}

	public RangeValidator()
	{
	}

	public RangeValidator(string errorMessage, object minimumValue, object maximumValue)
	{
		ErrorMessage = errorMessage;
		MinimumValue = minimumValue;
		MaximumValue = maximumValue;
	}

	public RangeValidator(string errorMessage, string optionalValidationGroup, object minimumValue, object maximumValue)
	{
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
		MinimumValue = minimumValue;
		MaximumValue = maximumValue;
	}

	public override bool Validate(Control input)
	{
		if (_MaximumValue == null && _MinimumValue == null)
		{
			return true;
		}
		object controlValue = GetControlValue(input);
		if (controlValue == null)
		{
			if (_MinimumValue == null)
			{
				return true;
			}
			return false;
		}
		bool flag = true;
		if (_MinimumValue != null)
		{
			flag = CompareValidator.Compare(controlValue, _MinimumValue, eValidationCompareOperator.GreaterThanEqual);
			if (!flag)
			{
				return false;
			}
		}
		bool flag2 = true;
		if (_MaximumValue != null)
		{
			flag2 = CompareValidator.Compare(controlValue, _MaximumValue, eValidationCompareOperator.LessThanEqual);
		}
		base.LastValidationResult = flag && flag2;
		return base.LastValidationResult;
	}
}
