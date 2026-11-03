using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Validator;

[ToolboxItem(false)]
[TypeConverter("DevComponents.DotNetBar.Design.RequiredFieldValidatorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
[Localizable(true)]
public class RequiredFieldValidator : ValidatorBase
{
	private EvaluateIsEmptyEventHandler _301A;

	private bool __3017;

	[Description("Indicates whether empty string of zero length is considered valid input.")]
	[DefaultValue(false)]
	[Category("Behavior")]
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

	public event EvaluateIsEmptyEventHandler EvaluateIsEmpty
	{
		add
		{
			EvaluateIsEmptyEventHandler evaluateIsEmptyEventHandler = _301A;
			EvaluateIsEmptyEventHandler evaluateIsEmptyEventHandler2;
			do
			{
				evaluateIsEmptyEventHandler2 = evaluateIsEmptyEventHandler;
				EvaluateIsEmptyEventHandler value2 = (EvaluateIsEmptyEventHandler)Delegate.Combine(evaluateIsEmptyEventHandler2, value);
				evaluateIsEmptyEventHandler = Interlocked.CompareExchange(ref _301A, value2, evaluateIsEmptyEventHandler2);
			}
			while ((object)evaluateIsEmptyEventHandler != evaluateIsEmptyEventHandler2);
		}
		remove
		{
			EvaluateIsEmptyEventHandler evaluateIsEmptyEventHandler = _301A;
			EvaluateIsEmptyEventHandler evaluateIsEmptyEventHandler2;
			do
			{
				evaluateIsEmptyEventHandler2 = evaluateIsEmptyEventHandler;
				EvaluateIsEmptyEventHandler value2 = (EvaluateIsEmptyEventHandler)Delegate.Remove(evaluateIsEmptyEventHandler2, value);
				evaluateIsEmptyEventHandler = Interlocked.CompareExchange(ref _301A, value2, evaluateIsEmptyEventHandler2);
			}
			while ((object)evaluateIsEmptyEventHandler != evaluateIsEmptyEventHandler2);
		}
	}

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
		if (evaluateIsEmptyEventArgs._301B)
		{
			return evaluateIsEmptyEventArgs.IsEmpty;
		}
		if (value == null)
		{
			return true;
		}
		if (value is string)
		{
			if (__3017)
			{
				return value == null;
			}
			return string.IsNullOrEmpty((string)value);
		}
		if ((input is ComboBox || input is ComboTree) && value is int)
		{
			return (int)value < 0;
		}
		return false;
	}

	protected virtual void OnEvaluateIsEmpty(EvaluateIsEmptyEventArgs args)
	{
		_301A?.Invoke(this, args);
	}
}
