using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public class CompareValidator : ValidatorBase
{
	private Control __300C;

	private string __300D = "";

	private object __300E;

	private eValidationCompareOperator __22EE;

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates control to compare validated control to.")]
	public Control ControlToCompare
	{
		get
		{
			return __300C;
		}
		set
		{
			__300C = value;
		}
	}

	[DefaultValue("")]
	[Description("Indicates Value property name for the ControlToCompare control.")]
	[Category("Behavior")]
	public string ControlToCompareValuePropertyName
	{
		get
		{
			return __300D;
		}
		set
		{
			__300D = value;
		}
	}

	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(null)]
	[Description("Indicates value to compare to the validation control.")]
	[Category("Behavior")]
	public object ValueToCompare
	{
		get
		{
			return __300E;
		}
		set
		{
			__300E = value;
		}
	}

	[Description("Indicates operator used for comparison")]
	[DefaultValue(eValidationCompareOperator.Equal)]
	[Category("Behavior")]
	public eValidationCompareOperator Operator
	{
		get
		{
			return __22EE;
		}
		set
		{
			__22EE = value;
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
		obj = ((__300C == null) ? __300E : GetControlValue(__300C));
		base.LastValidationResult = _300F(controlValue, obj, __22EE);
		return base.LastValidationResult;
	}

	internal static bool _300F(object _3010, object _3011, eValidationCompareOperator _3012)
	{
		if (_3012 == eValidationCompareOperator.DataTypeCheck)
		{
			if (_3010 != null && _3011 != null)
			{
				return _3010.GetType().Equals(_3011.GetType());
			}
			return _3010 == _3011;
		}
		if (_3010 == null && _3011 == null)
		{
			return true;
		}
		if (_3010 == null || _3011 == null)
		{
			return false;
		}
		if (!_3010.GetType().Equals(_3011.GetType()) && _3011 is string)
		{
			object _3014 = null;
			if (_3013(_3010, _3011, out _3014))
			{
				_3011 = _3014;
			}
		}
		if (!_3010.GetType().Equals(_3011.GetType()))
		{
			return false;
		}
		int num = 0;
		if (_3010 is string)
		{
			num = ((string)_3010).CompareTo(_3011);
		}
		else if (_3010 is int num2)
		{
			num = num2.CompareTo(_3011);
		}
		else if (_3010 is double num3)
		{
			num = num3.CompareTo(_3011);
		}
		else if (_3010 is long num4)
		{
			num = num4.CompareTo(_3011);
		}
		else if (_3010 is DateTime dateTime)
		{
			num = dateTime.CompareTo(_3011);
		}
		else if (_3010 is decimal num5)
		{
			num = num5.CompareTo(_3011);
		}
		else if (_3010 is float num6)
		{
			num = num6.CompareTo(_3011);
		}
		else if (_3010 is bool flag)
		{
			num = flag.CompareTo(_3011);
		}
		else if (_3010 is TimeSpan timeSpan)
		{
			num = timeSpan.CompareTo(_3011);
		}
		else if (_3010 is byte b)
		{
			num = b.CompareTo(_3011);
		}
		else if (_3010 is char c)
		{
			num = c.CompareTo(_3011);
		}
		else
		{
			if (!(_3010 is Guid guid))
			{
				return true;
			}
			num = guid.CompareTo(_3011);
		}
		return _3012 switch
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

	internal static bool _3013(object _3010, object _3011, out object _3014)
	{
		_3014 = null;
		if (_3011 == null || _3010 == null || !(_3011 is string))
		{
			return false;
		}
		if (_3011.GetType().Equals(_3010.GetType()))
		{
			_3014 = _3011;
			return true;
		}
		Type type = _3010.GetType();
		string text = (string)_3011;
		char result9;
		if ((object)type == typeof(int))
		{
			int result = 0;
			if (int.TryParse(text, out result))
			{
				_3014 = result;
				return true;
			}
		}
		else if ((object)type == typeof(long))
		{
			long result2 = 0L;
			if (long.TryParse(text, out result2))
			{
				_3014 = result2;
				return true;
			}
		}
		else if ((object)type == typeof(double))
		{
			double result3 = 0.0;
			if (double.TryParse(text, out result3))
			{
				_3014 = result3;
				return true;
			}
		}
		else if ((object)type == typeof(float))
		{
			float result4 = 0f;
			if (float.TryParse(text, out result4))
			{
				_3014 = result4;
				return true;
			}
		}
		else if ((object)type == typeof(DateTime))
		{
			if (DateTime.TryParse(text, out var result5))
			{
				_3014 = result5;
				return true;
			}
		}
		else if ((object)type == typeof(TimeSpan))
		{
			if (TimeSpan.TryParse(text, out var result6))
			{
				_3014 = result6;
				return true;
			}
		}
		else if ((object)type == typeof(bool))
		{
			if (bool.TryParse(text, out var result7))
			{
				_3014 = result7;
				return true;
			}
		}
		else if ((object)type == typeof(byte))
		{
			if (byte.TryParse(text, out var result8))
			{
				_3014 = result8;
				return true;
			}
		}
		else if ((object)type == typeof(char) && char.TryParse(text, out result9))
		{
			_3014 = result9;
			return true;
		}
		return false;
	}
}
