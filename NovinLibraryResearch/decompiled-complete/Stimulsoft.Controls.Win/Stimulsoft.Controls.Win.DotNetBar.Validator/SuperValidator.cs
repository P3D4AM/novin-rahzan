using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

[ToolboxBitmap(typeof(SuperTooltip), "Validator.SuperValidator.ico")]
[ToolboxItem(false)]
[ProvideProperty("Validator1", typeof(Control))]
[ProvideProperty("Validator2", typeof(Control))]
[ProvideProperty("Validator3", typeof(Control))]
[ComVisible(false)]
public class SuperValidator : Component, IExtenderProvider
{
	private Dictionary<Control, ValidatorBase> _Validators1 = new Dictionary<Control, ValidatorBase>();

	private Dictionary<Control, ValidatorBase> _Validators2 = new Dictionary<Control, ValidatorBase>();

	private Dictionary<Control, ValidatorBase> _Validators3 = new Dictionary<Control, ValidatorBase>();

	private bool _SteppedValidation;

	private List<ValidatorControlPair> _LastFailedValidations = new List<ValidatorControlPair>();

	private ErrorProvider _ErrorProvider;

	private IErrorProvider _CustomErrorProvider;

	private Highlighter _Highlighter;

	private eValidationType _ValidationType = eValidationType.Manual;

	private Control _ContainerControl;

	private bool _Enabled = true;

	private bool _CancelValidatingOnControl = true;

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether validation is performed in steps meaning that if first level validation fails, second level is not validated until first level validation succeeds")]
	public bool SteppedValidation
	{
		get
		{
			return _SteppedValidation;
		}
		set
		{
			_SteppedValidation = value;
		}
	}

	[Browsable(false)]
	public ReadOnlyCollection<ValidatorControlPair> LastFailedValidationResults => new ReadOnlyCollection<ValidatorControlPair>(_LastFailedValidations);

	[DefaultValue(null)]
	[Category("Behavior")]
	[Description("Indicates error provider that is used by the validator to report validation errors.")]
	public ErrorProvider ErrorProvider
	{
		get
		{
			return _ErrorProvider;
		}
		set
		{
			_ErrorProvider = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	[Category("Behavior")]
	[Description("Indicates custom error provider that is used by validator to report errors. You can provide your own error validators by implementing IErrorProvider interface.")]
	public IErrorProvider CustomErrorProvider
	{
		get
		{
			return _CustomErrorProvider;
		}
		set
		{
			_CustomErrorProvider = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Highlighter component that is used to highlight validation errors.")]
	public Highlighter Highlighter
	{
		get
		{
			return _Highlighter;
		}
		set
		{
			_Highlighter = value;
		}
	}

	[DefaultValue(eValidationType.Manual)]
	[Category("Behavior")]
	[Description("Indicates validation type performed by the control.")]
	public eValidationType ValidationType
	{
		get
		{
			return _ValidationType;
		}
		set
		{
			_ValidationType = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates container control validator is bound to. Should be set to parent form.")]
	[Category("Behavior")]
	public Control ContainerControl
	{
		get
		{
			return _ContainerControl;
		}
		set
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			if (_ContainerControl == value)
			{
				return;
			}
			if (_ContainerControl != null)
			{
				if (_ContainerControl is Form)
				{
					((Form)_ContainerControl).FormClosing -= new FormClosingEventHandler(ContainerFormClosing);
				}
				else
				{
					_ContainerControl.Validating -= ContainerValidating;
				}
			}
			_ContainerControl = value;
			if (_ContainerControl != null)
			{
				if (_ContainerControl is Form)
				{
					((Form)_ContainerControl).FormClosing += new FormClosingEventHandler(ContainerFormClosing);
				}
				else
				{
					_ContainerControl.Validating += ContainerValidating;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether validation is performed.")]
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

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Cancel argument in Validating event for validation type ValidatingEventPerControl is set to true when validation fails.")]
	public bool CancelValidatingOnControl
	{
		get
		{
			return _CancelValidatingOnControl;
		}
		set
		{
			_CancelValidatingOnControl = value;
		}
	}

	public event ValidatorGetValueEventHandler GetValue;

	public event ValidateValueEventHandler CustomValidatorValidateValue;

	protected override void Dispose(bool disposing)
	{
		ClearAllValidators();
		ContainerControl = null;
		base.Dispose(disposing);
	}

	private void ClearAllValidators()
	{
		ClearValidator(_Validators1);
		ClearValidator(_Validators2);
		ClearValidator(_Validators3);
	}

	private void ClearValidator(Dictionary<Control, ValidatorBase> validators)
	{
		if (validators == null || validators.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<Control, ValidatorBase> validator in validators)
		{
			validator.Key.Validating -= ControlValidating;
		}
		validators.Clear();
	}

	internal void InvokeCustomValidatorValidateValue(CustomValidator validator, ValidateValueEventArgs e)
	{
		OnCustomValidatorValidateValue(validator, e);
	}

	protected virtual void OnCustomValidatorValidateValue(CustomValidator validator, ValidateValueEventArgs e)
	{
		this.CustomValidatorValidateValue?.Invoke(validator, e);
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ValidatorEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Localizable(true)]
	public ValidatorBase GetValidator1(Control c)
	{
		if (_Validators1.ContainsKey(c))
		{
			return _Validators1[c];
		}
		return null;
	}

	public void SetValidator1(Control c, ValidatorBase info)
	{
		if (_Validators1.ContainsKey(c))
		{
			if (info == null)
			{
				ValidatorBase validator = _Validators1[c];
				RemoveValidator(_Validators1, c);
				DestroyValidator(validator);
				return;
			}
			ValidatorBase validatorBase = _Validators1[c];
			if (validatorBase != info)
			{
				DestroyValidator(validatorBase);
			}
			_Validators1[c] = info;
		}
		else if (info != null)
		{
			AddValidator(_Validators1, c, info);
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ValidatorEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Localizable(true)]
	public ValidatorBase GetValidator2(Control c)
	{
		if (_Validators2.ContainsKey(c))
		{
			return _Validators2[c];
		}
		return null;
	}

	public void SetValidator2(Control c, ValidatorBase info)
	{
		if (_Validators2.ContainsKey(c))
		{
			if (info == null)
			{
				ValidatorBase validator = _Validators2[c];
				RemoveValidator(_Validators2, c);
				DestroyValidator(validator);
				return;
			}
			ValidatorBase validatorBase = _Validators2[c];
			if (validatorBase != info)
			{
				DestroyValidator(validatorBase);
			}
			_Validators2[c] = info;
		}
		else if (info != null)
		{
			AddValidator(_Validators2, c, info);
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ValidatorEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Localizable(true)]
	public ValidatorBase GetValidator3(Control c)
	{
		if (_Validators3.ContainsKey(c))
		{
			return _Validators3[c];
		}
		return null;
	}

	public void SetValidator3(Control c, ValidatorBase info)
	{
		if (_Validators3.ContainsKey(c))
		{
			if (info == null)
			{
				ValidatorBase validator = _Validators3[c];
				RemoveValidator(_Validators3, c);
				DestroyValidator(validator);
				return;
			}
			ValidatorBase validatorBase = _Validators3[c];
			if (validatorBase != info)
			{
				DestroyValidator(validatorBase);
			}
			_Validators3[c] = info;
		}
		else if (info != null)
		{
			AddValidator(_Validators3, c, info);
		}
	}

	private void DestroyValidator(ValidatorBase validator)
	{
		if (Site != null && base.DesignMode && Site.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.DestroyComponent(validator);
		}
	}

	private void RemoveValidator(Dictionary<Control, ValidatorBase> validatorCollection, Control control)
	{
		ValidatorBase validatorBase = validatorCollection[control];
		validatorBase.SuperValidator = null;
		validatorCollection.Remove(control);
		control.Validating -= ControlValidating;
	}

	private void AddValidator(Dictionary<Control, ValidatorBase> validatorCollection, Control control, ValidatorBase info)
	{
		info.SuperValidator = this;
		validatorCollection.Add(control, info);
		control.Validating += ControlValidating;
	}

	private void ControlValidating(object sender, CancelEventArgs e)
	{
		if (!Enabled || _ValidationType == eValidationType.Manual || (_ValidationType & eValidationType.ValidatingEventPerControl) != eValidationType.ValidatingEventPerControl)
		{
			return;
		}
		Control val = (Control)((sender is Control) ? sender : null);
		if (val == null)
		{
			return;
		}
		ValidatorBase value = null;
		bool flag = true;
		if (_Validators1.TryGetValue(val, out value))
		{
			bool cancelValidating = false;
			flag = ValidateSingleControl(value, val, out cancelValidating);
			if (!flag)
			{
				AddToLastFailedValidations(value, val);
			}
			e.Cancel = cancelValidating;
		}
		if (flag && _Validators2.TryGetValue(val, out value))
		{
			bool cancelValidating2 = false;
			flag = ValidateSingleControl(value, val, out cancelValidating2);
			if (!flag)
			{
				AddToLastFailedValidations(value, val);
			}
			e.Cancel = cancelValidating2;
		}
		if (flag && _Validators3.TryGetValue(val, out value))
		{
			bool cancelValidating3 = false;
			if (!ValidateSingleControl(value, val, out cancelValidating3))
			{
				AddToLastFailedValidations(value, val);
			}
			e.Cancel = cancelValidating3;
		}
	}

	private void AddToLastFailedValidations(ValidatorBase validator, Control control)
	{
		foreach (ValidatorControlPair lastFailedValidation in _LastFailedValidations)
		{
			if (lastFailedValidation.Control == control && lastFailedValidation.Validator == validator)
			{
				return;
			}
		}
		_LastFailedValidations.Add(new ValidatorControlPair(validator, control));
	}

	private bool ValidateSingleControl(ValidatorBase validator, Control control, out bool cancelValidating)
	{
		cancelValidating = false;
		if (!validator.Enabled)
		{
			return true;
		}
		bool flag = true;
		bool flag2 = true;
		if (!string.IsNullOrEmpty(validator.OptionalValidationGroup))
		{
			List<ValidatorControlPair> optionalValidators = GetOptionalValidators(validator.OptionalValidationGroup);
			if (optionalValidators.Count > 1)
			{
				flag = false;
				foreach (ValidatorControlPair item in optionalValidators)
				{
					flag |= item.Validator.Validate(item.Control);
				}
				if (!flag)
				{
					foreach (ValidatorControlPair item2 in optionalValidators)
					{
						if (item2.Validator.DisplayError)
						{
							SetError(item2.Validator, item2.Control);
						}
					}
				}
				else
				{
					foreach (ValidatorControlPair item3 in optionalValidators)
					{
						ClearError(item3.Validator, item3.Control);
					}
				}
				flag2 = false;
			}
		}
		if (flag2)
		{
			flag = validator.Validate(control);
			if (!flag)
			{
				if (validator.DisplayError)
				{
					SetError(validator, control);
				}
				cancelValidating = _CancelValidatingOnControl;
			}
			else
			{
				ClearError(validator, control);
			}
		}
		return flag;
	}

	private List<ValidatorControlPair> GetOptionalValidators(string optionalValidationGroup)
	{
		List<ValidatorControlPair> list = new List<ValidatorControlPair>();
		FindOptionalValidators(_Validators1, optionalValidationGroup, list);
		FindOptionalValidators(_Validators2, optionalValidationGroup, list);
		FindOptionalValidators(_Validators3, optionalValidationGroup, list);
		return list;
	}

	private void FindOptionalValidators(Dictionary<Control, ValidatorBase> validators, string optionalValidationGroup, List<ValidatorControlPair> list)
	{
		foreach (KeyValuePair<Control, ValidatorBase> validator in validators)
		{
			if (validator.Value.OptionalValidationGroup.Equals(optionalValidationGroup, StringComparison.CurrentCultureIgnoreCase))
			{
				list.Add(new ValidatorControlPair(validator.Value, validator.Key));
			}
		}
	}

	protected virtual void OnGetValue(ValidatorGetValueEventArgs e)
	{
		this.GetValue?.Invoke(this, e);
	}

	internal void InvokeGetValue(ValidatorGetValueEventArgs e)
	{
		OnGetValue(e);
	}

	public bool Validate()
	{
		bool flag = true;
		Dictionary<string, bool> table = new Dictionary<string, bool>();
		_LastFailedValidations.Clear();
		flag &= ValidateSingleTable(_Validators1, table);
		if (!flag && _SteppedValidation)
		{
			return flag;
		}
		flag &= ValidateSingleTable(_Validators2, table);
		if (!flag && _SteppedValidation)
		{
			return flag;
		}
		return flag & ValidateSingleTable(_Validators3, table);
	}

	public bool Validate(Control controlToValidate)
	{
		return Validate(controlToValidate, clearLastFailedValidations: true);
	}

	public bool Validate(Control controlToValidate, bool clearLastFailedValidations)
	{
		bool flag = true;
		bool cancelValidating = false;
		if (clearLastFailedValidations)
		{
			ClearFailedValidations();
		}
		ValidatorBase validatorBase = null;
		if (_Validators1.ContainsKey(controlToValidate))
		{
			validatorBase = _Validators1[controlToValidate];
			bool flag2 = ValidateSingleControl(validatorBase, controlToValidate, out cancelValidating);
			if (!flag2)
			{
				_LastFailedValidations.Add(new ValidatorControlPair(validatorBase, controlToValidate));
			}
			flag = flag && flag2;
		}
		if (!flag && _SteppedValidation)
		{
			return flag;
		}
		if (_Validators2.ContainsKey(controlToValidate))
		{
			validatorBase = _Validators2[controlToValidate];
			bool flag3 = ValidateSingleControl(validatorBase, controlToValidate, out cancelValidating);
			if (!flag3)
			{
				_LastFailedValidations.Add(new ValidatorControlPair(validatorBase, controlToValidate));
			}
			flag = flag && flag3;
		}
		if (!flag && _SteppedValidation)
		{
			return flag;
		}
		if (_Validators3.ContainsKey(controlToValidate))
		{
			validatorBase = _Validators3[controlToValidate];
			bool flag4 = ValidateSingleControl(validatorBase, controlToValidate, out cancelValidating);
			if (!flag4)
			{
				_LastFailedValidations.Add(new ValidatorControlPair(validatorBase, controlToValidate));
			}
			flag = flag && flag4;
		}
		if (!flag)
		{
			_ = _SteppedValidation;
			return flag;
		}
		return flag;
	}

	private bool ValidateSingleTable(Dictionary<Control, ValidatorBase> validators, Dictionary<string, bool> table)
	{
		bool flag = true;
		foreach (KeyValuePair<Control, ValidatorBase> validator in validators)
		{
			if (ControlFailedValidation(validator.Key))
			{
				continue;
			}
			string optionalValidationGroup = validator.Value.OptionalValidationGroup;
			if (!string.IsNullOrEmpty(optionalValidationGroup))
			{
				if (table.ContainsKey(optionalValidationGroup))
				{
					continue;
				}
				table.Add(optionalValidationGroup, value: true);
			}
			bool cancelValidating = false;
			bool flag2 = ValidateSingleControl(validator.Value, validator.Key, out cancelValidating);
			if (!flag2)
			{
				_LastFailedValidations.Add(new ValidatorControlPair(validator.Value, validator.Key));
			}
			flag = flag && flag2;
		}
		return flag;
	}

	private bool ControlFailedValidation(Control control)
	{
		foreach (ValidatorControlPair lastFailedValidation in _LastFailedValidations)
		{
			if (lastFailedValidation.Control == control)
			{
				return true;
			}
		}
		return false;
	}

	private void SetError(ValidatorBase validator, Control controlToValidate)
	{
		List<IErrorProvider> errorProviders = GetErrorProviders();
		if (errorProviders == null || errorProviders.Count == 0)
		{
			return;
		}
		foreach (IErrorProvider item in errorProviders)
		{
			if (item is Highlighter)
			{
				((Highlighter)item).SetHighlightColor(controlToValidate, validator.HighlightColor);
			}
			else
			{
				item.SetError(controlToValidate, validator.ErrorMessage);
			}
		}
	}

	public void ClearFailedValidations()
	{
		foreach (ValidatorControlPair lastFailedValidation in _LastFailedValidations)
		{
			ClearError(lastFailedValidation.Validator, lastFailedValidation.Control);
		}
		_LastFailedValidations.Clear();
	}

	private void ClearError(ValidatorBase validator, Control controlToValidate)
	{
		List<IErrorProvider> errorProviders = GetErrorProviders();
		if (errorProviders == null || errorProviders.Count == 0)
		{
			return;
		}
		foreach (IErrorProvider item in errorProviders)
		{
			item.ClearError(controlToValidate);
		}
	}

	private List<IErrorProvider> GetErrorProviders()
	{
		List<IErrorProvider> list = new List<IErrorProvider>();
		if (_ErrorProvider != null)
		{
			list.Add(new ErrorProviderWrapper(_ErrorProvider, (_Highlighter != null) ? 2 : 0));
		}
		if (_CustomErrorProvider != null)
		{
			list.Add(_CustomErrorProvider);
		}
		if (_Highlighter != null)
		{
			list.Add(_Highlighter);
		}
		return list;
	}

	private void ContainerFormClosing(object sender, FormClosingEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Enabled && (_ValidationType & eValidationType.ValidatingEventOnContainer) == eValidationType.ValidatingEventOnContainer && ((Control)(Form)sender).CausesValidation)
		{
			((CancelEventArgs)(object)e).Cancel = !Validate();
		}
	}

	private void ContainerValidating(object sender, CancelEventArgs e)
	{
		if (Enabled && _ValidationType == eValidationType.ValidatingEventOnContainer)
		{
			e.Cancel = !Validate();
		}
	}

	public bool CanExtend(object extendee)
	{
		return extendee is Control;
	}
}
