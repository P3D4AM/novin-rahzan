using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public class CustomValidator : ValidatorBase
{
	private ValidateValueEventHandler _300B;

	private object __1714;

	[Description("Custom data associated with the validator")]
	[Category("Data")]
	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(null)]
	[Localizable(false)]
	public object Tag
	{
		get
		{
			return __1714;
		}
		set
		{
			__1714 = value;
		}
	}

	public event ValidateValueEventHandler ValidateValue
	{
		add
		{
			ValidateValueEventHandler validateValueEventHandler = _300B;
			ValidateValueEventHandler validateValueEventHandler2;
			do
			{
				validateValueEventHandler2 = validateValueEventHandler;
				ValidateValueEventHandler value2 = (ValidateValueEventHandler)Delegate.Combine(validateValueEventHandler2, value);
				validateValueEventHandler = Interlocked.CompareExchange(ref _300B, value2, validateValueEventHandler2);
			}
			while ((object)validateValueEventHandler != validateValueEventHandler2);
		}
		remove
		{
			ValidateValueEventHandler validateValueEventHandler = _300B;
			ValidateValueEventHandler validateValueEventHandler2;
			do
			{
				validateValueEventHandler2 = validateValueEventHandler;
				ValidateValueEventHandler value2 = (ValidateValueEventHandler)Delegate.Remove(validateValueEventHandler2, value);
				validateValueEventHandler = Interlocked.CompareExchange(ref _300B, value2, validateValueEventHandler2);
			}
			while ((object)validateValueEventHandler != validateValueEventHandler2);
		}
	}

	public override bool Validate(Control input)
	{
		ValidateValueEventArgs validateValueEventArgs = new ValidateValueEventArgs(input);
		OnValidateValue(validateValueEventArgs);
		if (base.SuperValidator != null)
		{
			base.SuperValidator.InvokeCustomValidatorValidateValue(this, validateValueEventArgs);
		}
		base.LastValidationResult = validateValueEventArgs.IsValid;
		return base.LastValidationResult;
	}

	protected virtual void OnValidateValue(ValidateValueEventArgs e)
	{
		_300B?.Invoke(this, e);
	}
}
