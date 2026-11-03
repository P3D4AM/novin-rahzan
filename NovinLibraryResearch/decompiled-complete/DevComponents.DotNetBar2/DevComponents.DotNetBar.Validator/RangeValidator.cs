using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public class RangeValidator : ValidatorBase
{
	private object __3015;

	private object __3016;

	private bool __3017;

	[Description("Indicates maximum value control may have.")]
	[TypeConverter(typeof(StringConverter))]
	[Category("Behavior")]
	[DefaultValue(null)]
	public object MaximumValue
	{
		get
		{
			return __3015;
		}
		set
		{
			__3015 = value;
		}
	}

	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Indicates minimum value control may have.")]
	[DefaultValue(null)]
	public object MinimumValue
	{
		get
		{
			return __3016;
		}
		set
		{
			__3016 = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether empty string of zero length is considered valid input.")]
	public bool IsEmptyStringValid
	{
		get
		{
			return __3017;
		}
		set
		{
			__3017 = value;
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
		if (__3015 == null && __3016 == null)
		{
			return true;
		}
		object controlValue = GetControlValue(input);
		if (controlValue == null)
		{
			if (__3016 == null)
			{
				return true;
			}
			return false;
		}
		if (controlValue == "" && __3017)
		{
			return true;
		}
		bool flag = true;
		if (__3016 != null)
		{
			flag = CompareValidator._300F(controlValue, __3016, eValidationCompareOperator.GreaterThanEqual);
			if (!flag)
			{
				return false;
			}
		}
		bool flag2 = true;
		if (__3015 != null)
		{
			flag2 = CompareValidator._300F(controlValue, __3015, eValidationCompareOperator.LessThanEqual);
		}
		base.LastValidationResult = flag && flag2;
		return base.LastValidationResult;
	}
}
