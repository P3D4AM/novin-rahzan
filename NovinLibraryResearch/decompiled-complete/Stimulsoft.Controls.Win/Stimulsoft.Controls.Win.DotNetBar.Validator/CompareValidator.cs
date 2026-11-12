using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public class CompareValidator : ValidatorBase
{
	private Control _ControlToCompare;

	private string _ControlToCompareValuePropertyName = "";

	private object _ValueToCompare;

	private eValidationCompareOperator _Operator;

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates control to compare validated control to.")]
	public Control ControlToCompare
	{
		get
		{
			return _ControlToCompare;
		}
		set
		{
			_ControlToCompare = value;
		}
	}

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates Value property name for the ControlToCompare control.")]
	public string ControlToCompareValuePropertyName
	{
		get
		{
			return _ControlToCompareValuePropertyName;
		}
		set
		{
			_ControlToCompareValuePropertyName = value;
		}
	}

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates value to compare to the validation control.")]
	[TypeConverter(typeof(StringConverter))]
	public object ValueToCompare
	{
		get
		{
			return _ValueToCompare;
		}
		set
		{
			_ValueToCompare = value;
		}
	}

	[DefaultValue(eValidationCompareOperator.Equal)]
	[Category("Behavior")]
	[Description("Indicates operator used for comparison")]
	public eValidationCompareOperator Operator
	{
		get
		{
			return _Operator;
		}
		set
		{
			_Operator = value;
		}
	}

	public CompareValidator()
	{
	}

	public CompareValidator(string errorMessage)
	{
		ErrorMessage = errorMessage;
	}

	public CompareValidator(string errorMessage, string optionalValidationGroup)
	{
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
	}

	public CompareValidator(string errorMessage, string optionalValidationGroup, Control controlToCompare, eValidationCompareOperator compareOperator)
	{
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
		ControlToCompare = controlToCompare;
		Operator = compareOperator;
	}

	public CompareValidator(string errorMessage, Control controlToCompare, eValidationCompareOperator compareOperator)
	{
		ErrorMessage = errorMessage;
		ControlToCompare = controlToCompare;
		Operator = compareOperator;
	}

	public CompareValidator(string errorMessage, string optionalValidationGroup, object valueToCompare, eValidationCompareOperator compareOperator)
	{
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
		ValueToCompare = valueToCompare;
		Operator = compareOperator;
	}

	public CompareValidator(string errorMessage, object valueToCompare, eValidationCompareOperator compareOperator)
	{
		ErrorMessage = errorMessage;
		ValueToCompare = valueToCompare;
		Operator = compareOperator;
	}

	public CompareValidator(string errorMessage, string optionalValidationGroup, Control controlToCompare, object valueToCompare, eValidationCompareOperator compareOperator)
	{
		ErrorMessage = errorMessage;
		base.OptionalValidationGroup = optionalValidationGroup;
		ControlToCompare = controlToCompare;
		Operator = compareOperator;
		ValueToCompare = valueToCompare;
	}

	public override bool Validate(Control input)
	{
		object controlValue = GetControlValue(input);
		object obj = null;
		obj = ((_ControlToCompare == null) ? _ValueToCompare : GetControlValue(_ControlToCompare));
		base.LastValidationResult = Compare(controlValue, obj, _Operator);
		return base.LastValidationResult;
	}

	internal static bool Compare(object leftValue, object rightValue, eValidationCompareOperator compareOperator)
	{
		if (compareOperator == eValidationCompareOperator.DataTypeCheck)
		{
			if (leftValue != null && rightValue != null)
			{
				return leftValue.GetType().Equals(rightValue.GetType());
			}
			return leftValue == rightValue;
		}
		if (leftValue == null && rightValue == null)
		{
			return true;
		}
		if (leftValue == null || rightValue == null)
		{
			return false;
		}
		if (!leftValue.GetType().Equals(rightValue.GetType()) && rightValue is string)
		{
			object temp = null;
			if (ConvertRightToLeftType(leftValue, rightValue, out temp))
			{
				rightValue = temp;
			}
		}
		if (!leftValue.GetType().Equals(rightValue.GetType()))
		{
			return false;
		}
		int num = 0;
		if (leftValue is string)
		{
			num = ((string)leftValue).CompareTo(rightValue);
		}
		else if (leftValue is int num2)
		{
			num = num2.CompareTo(rightValue);
		}
		else if (leftValue is double num3)
		{
			num = num3.CompareTo(rightValue);
		}
		else if (leftValue is long num4)
		{
			num = num4.CompareTo(rightValue);
		}
		else if (leftValue is DateTime dateTime)
		{
			num = dateTime.CompareTo(rightValue);
		}
		else if (leftValue is decimal num5)
		{
			num = num5.CompareTo(rightValue);
		}
		else if (leftValue is float num6)
		{
			num = num6.CompareTo(rightValue);
		}
		else if (leftValue is bool flag)
		{
			num = flag.CompareTo(rightValue);
		}
		else if (leftValue is TimeSpan timeSpan)
		{
			num = timeSpan.CompareTo(rightValue);
		}
		else if (leftValue is byte b)
		{
			num = b.CompareTo(rightValue);
		}
		else if (leftValue is char c)
		{
			num = c.CompareTo(rightValue);
		}
		else
		{
			if (!(leftValue is Guid guid))
			{
				return true;
			}
			num = guid.CompareTo(rightValue);
		}
		return compareOperator switch
		{
			eValidationCompareOperator.Equal => num == 0, 
			eValidationCompareOperator.NotEqual => num != 0, 
			eValidationCompareOperator.GreaterThan => num > 0, 
			eValidationCompareOperator.GreaterThanEqual => num >= 0, 
			eValidationCompareOperator.LessThan => num < 0, 
			eValidationCompareOperator.LessThanEqual => num <= 0, 
			_ => true, 
		};
	}

	internal static bool ConvertRightToLeftType(object leftValue, object rightValue, out object temp)
	{
		temp = null;
		if (rightValue == null || leftValue == null || !(rightValue is string))
		{
			return false;
		}
		if (rightValue.GetType().Equals(leftValue.GetType()))
		{
			temp = rightValue;
			return true;
		}
		Type type = leftValue.GetType();
		string text = (string)rightValue;
		char result9;
		if (type == typeof(int))
		{
			int result = 0;
			if (int.TryParse(text, out result))
			{
				temp = result;
				return true;
			}
		}
		else if (type == typeof(long))
		{
			long result2 = 0L;
			if (long.TryParse(text, out result2))
			{
				temp = result2;
				return true;
			}
		}
		else if (type == typeof(double))
		{
			double result3 = 0.0;
			if (double.TryParse(text, out result3))
			{
				temp = result3;
				return true;
			}
		}
		else if (type == typeof(float))
		{
			float result4 = 0f;
			if (float.TryParse(text, out result4))
			{
				temp = result4;
				return true;
			}
		}
		else if (type == typeof(DateTime))
		{
			if (DateTime.TryParse(text, out var result5))
			{
				temp = result5;
				return true;
			}
		}
		else if (type == typeof(TimeSpan))
		{
			if (TimeSpan.TryParse(text, out var result6))
			{
				temp = result6;
				return true;
			}
		}
		else if (type == typeof(bool))
		{
			if (bool.TryParse(text, out var result7))
			{
				temp = result7;
				return true;
			}
		}
		else if (type == typeof(byte))
		{
			if (byte.TryParse(text, out var result8))
			{
				temp = result8;
				return true;
			}
		}
		else if (type == typeof(char) && char.TryParse(text, out result9))
		{
			temp = result9;
			return true;
		}
		return false;
	}
}
