using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public class CustomValidator : ValidatorBase
{
	private object _Tag;

	[DefaultValue(null)]
	[Localizable(false)]
	[TypeConverter(typeof(StringConverter))]
	[Category("Data")]
	[Description("Custom data associated with the validator")]
	public object Tag
	{
		get
		{
			return _Tag;
		}
		set
		{
			_Tag = value;
		}
	}

	public event ValidateValueEventHandler ValidateValue;

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
		this.ValidateValue?.Invoke(this, e);
	}
}
