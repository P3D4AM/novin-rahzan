using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Validator;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public abstract class ValidatorBase : Component
{
	private ValidatorGetValueEventHandler _239B;

	private string __2FDD = "";

	private bool __2FDE = true;

	private SuperValidator __2FDF;

	private bool __2FE0 = true;

	private bool _Ԋ = true;

	private string __2FE1 = "";

	private eHighlightColor __2FE2;

	private string __2FE3 = "";

	[Category("Appearance")]
	[Description("Indicates error message that is displayed when validation fails.")]
	[Localizable(true)]
	[DefaultValue("")]
	public virtual string ErrorMessage
	{
		get
		{
			return __2FDD;
		}
		set
		{
			__2FDD = value;
		}
	}

	[Browsable(false)]
	public bool LastValidationResult
	{
		get
		{
			return __2FDE;
		}
		internal set
		{
			__2FDE = value;
		}
	}

	[Browsable(false)]
	public SuperValidator SuperValidator
	{
		get
		{
			return __2FDF;
		}
		internal set
		{
			__2FDF = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether error is displayed using the error provider on SuperValidator when validation fails.")]
	public bool DisplayError
	{
		get
		{
			return __2FE0;
		}
		set
		{
			__2FE0 = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether validator is enabled.")]
	public bool Enabled
	{
		get
		{
			return _Ԋ;
		}
		set
		{
			_Ԋ = value;
		}
	}

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Specifies group name validation belongs to. When control belongs to optional validation group the validation is considered successful when any of the controls in the group validates.")]
	public string OptionalValidationGroup
	{
		get
		{
			return __2FE1;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			__2FE1 = value;
		}
	}

	[DefaultValue(eHighlightColor.None)]
	[Description("Indicates highlight color for control when validation fails if Highlighter component is used on SuperValidator.")]
	[Category("Appearance")]
	public eHighlightColor HighlightColor
	{
		get
		{
			return __2FE2;
		}
		set
		{
			__2FE2 = value;
		}
	}

	[Description("Indicates value property name for the control to validate.")]
	[DefaultValue("")]
	[Category("Behavior")]
	public string ValuePropertyName
	{
		get
		{
			return __2FE3;
		}
		set
		{
			__2FE3 = value;
		}
	}

	public event ValidatorGetValueEventHandler GetValue
	{
		add
		{
			ValidatorGetValueEventHandler validatorGetValueEventHandler = _239B;
			ValidatorGetValueEventHandler validatorGetValueEventHandler2;
			do
			{
				validatorGetValueEventHandler2 = validatorGetValueEventHandler;
				ValidatorGetValueEventHandler value2 = (ValidatorGetValueEventHandler)Delegate.Combine(validatorGetValueEventHandler2, value);
				validatorGetValueEventHandler = Interlocked.CompareExchange(ref _239B, value2, validatorGetValueEventHandler2);
			}
			while ((object)validatorGetValueEventHandler != validatorGetValueEventHandler2);
		}
		remove
		{
			ValidatorGetValueEventHandler validatorGetValueEventHandler = _239B;
			ValidatorGetValueEventHandler validatorGetValueEventHandler2;
			do
			{
				validatorGetValueEventHandler2 = validatorGetValueEventHandler;
				ValidatorGetValueEventHandler value2 = (ValidatorGetValueEventHandler)Delegate.Remove(validatorGetValueEventHandler2, value);
				validatorGetValueEventHandler = Interlocked.CompareExchange(ref _239B, value2, validatorGetValueEventHandler2);
			}
			while ((object)validatorGetValueEventHandler != validatorGetValueEventHandler2);
		}
	}

	public abstract bool Validate(Control input);

	protected virtual object GetControlValue(Control input)
	{
		return GetControlValue(input, __2FE3);
	}

	protected virtual object GetControlValue(Control input, string valuePropertyName)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		ValidatorGetValueEventArgs validatorGetValueEventArgs = new ValidatorGetValueEventArgs(input, this);
		OnGetValue(validatorGetValueEventArgs);
		if (validatorGetValueEventArgs._2FE4)
		{
			return validatorGetValueEventArgs.Value;
		}
		if (__2FDF != null)
		{
			__2FDF.InvokeGetValue(validatorGetValueEventArgs);
			if (validatorGetValueEventArgs._2FE4)
			{
				return validatorGetValueEventArgs.Value;
			}
		}
		if (!string.IsNullOrEmpty(valuePropertyName))
		{
			return TypeDescriptor.GetProperties(input)[valuePropertyName].GetValue(input);
		}
		if (input is ComboBox)
		{
			ComboBox val = (ComboBox)input;
			return ((ListControl)val).SelectedIndex;
		}
		if (input is ComboTree)
		{
			ComboTree comboTree = (ComboTree)(object)input;
			return comboTree.SelectedIndex;
		}
		if (input is DoubleInput)
		{
			return ((DoubleInput)(object)input).ValueObject;
		}
		if (input is IntegerInput)
		{
			return ((IntegerInput)(object)input).ValueObject;
		}
		if (input is DateTimeInput)
		{
			return ((DateTimeInput)(object)input).ValueObject;
		}
		object result = input.Text;
		string text = input.Text;
		long result2 = 0L;
		int result3 = 0;
		double result4 = 0.0;
		if (long.TryParse(text, out result2))
		{
			result = result2;
		}
		else if (int.TryParse(text, out result3))
		{
			result = result3;
		}
		else if (double.TryParse(text, out result4))
		{
			result = result4;
		}
		return result;
	}

	protected virtual void OnGetValue(ValidatorGetValueEventArgs e)
	{
		_239B?.Invoke(this, e);
	}
}
