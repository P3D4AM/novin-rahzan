using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;
using Stimulsoft.Controls.Win.Editors.DateTimeAdv;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public abstract class ValidatorBase : Component
{
	private string _ErrorMessage = "";

	private bool _LastValidationResult = true;

	private SuperValidator _SuperValidator;

	private bool _DisplayError = true;

	private bool _Enabled = true;

	private string _OptionalValidationGroup = "";

	private eHighlightColor _HighlightColor;

	private string _ValuePropertyName = "";

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates error message that is displayed when validation fails.")]
	[Localizable(true)]
	public virtual string ErrorMessage
	{
		get
		{
			return _ErrorMessage;
		}
		set
		{
			_ErrorMessage = value;
		}
	}

	[Browsable(false)]
	public bool LastValidationResult
	{
		get
		{
			return _LastValidationResult;
		}
		internal set
		{
			_LastValidationResult = value;
		}
	}

	[Browsable(false)]
	public SuperValidator SuperValidator
	{
		get
		{
			return _SuperValidator;
		}
		internal set
		{
			_SuperValidator = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether error is displayed using the error provider on SuperValidator when validation fails.")]
	public bool DisplayError
	{
		get
		{
			return _DisplayError;
		}
		set
		{
			_DisplayError = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether validator is enabled.")]
	public bool Enabled
	{
		get
		{
			return _Enabled;
		}
		set
		{
			_Enabled = value;
		}
	}

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Specifies group name validation belongs to. When control belongs to optional validation group the validation is considered successful when any of the controls in the group validates.")]
	public string OptionalValidationGroup
	{
		get
		{
			return _OptionalValidationGroup;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_OptionalValidationGroup = value;
		}
	}

	[DefaultValue(eHighlightColor.None)]
	[Category("Appearance")]
	[Description("Indicates highlight color for control when validation fails if Highlighter component is used on SuperValidator.")]
	public eHighlightColor HighlightColor
	{
		get
		{
			return _HighlightColor;
		}
		set
		{
			_HighlightColor = value;
		}
	}

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates value property name for the control to validate.")]
	public string ValuePropertyName
	{
		get
		{
			return _ValuePropertyName;
		}
		set
		{
			_ValuePropertyName = value;
		}
	}

	public event ValidatorGetValueEventHandler GetValue;

	public abstract bool Validate(Control input);

	protected virtual object GetControlValue(Control input)
	{
		return GetControlValue(input, _ValuePropertyName);
	}

	protected virtual object GetControlValue(Control input, string valuePropertyName)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		ValidatorGetValueEventArgs validatorGetValueEventArgs = new ValidatorGetValueEventArgs(input, this);
		OnGetValue(validatorGetValueEventArgs);
		if (validatorGetValueEventArgs.ValueSet)
		{
			return validatorGetValueEventArgs.Value;
		}
		if (_SuperValidator != null)
		{
			_SuperValidator.InvokeGetValue(validatorGetValueEventArgs);
			if (validatorGetValueEventArgs.ValueSet)
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
		this.GetValue?.Invoke(this, e);
	}
}
