using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

[ProvideProperty("Validator3", typeof(Control))]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.SuperValidatorDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ProvideProperty("Validator1", typeof(Control))]
[ProvideProperty("Validator2", typeof(Control))]
[ToolboxBitmap(typeof(SuperTooltip), "Validator.SuperValidator.ico")]
[ToolboxItem(true)]
public class SuperValidator : Component, IExtenderProvider
{
	private Dictionary<Control, ValidatorBase> __2FE5 = new Dictionary<Control, ValidatorBase>();

	private Dictionary<Control, ValidatorBase> __2FE6 = new Dictionary<Control, ValidatorBase>();

	private Dictionary<Control, ValidatorBase> __2FE7 = new Dictionary<Control, ValidatorBase>();

	private ValidatorGetValueEventHandler _239B;

	private ValidateValueEventHandler _2FE8;

	private bool __2FE9;

	private List<ValidatorControlPair> __2FEA = new List<ValidatorControlPair>();

	private int __2FEB = 2;

	private ErrorProvider __2FEC;

	private IErrorProvider __2FED;

	private Highlighter __2FEE;

	private eValidationType __2FEF = eValidationType.Manual;

	private Control __2FF0;

	private bool _Ԋ = true;

	private bool __2FF1 = true;

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether validation is performed in steps meaning that if first level validation fails, second level is not validated until first level validation succeeds")]
	public bool SteppedValidation
	{
		get
		{
			return __2FE9;
		}
		set
		{
			__2FE9 = value;
		}
	}

	[Browsable(false)]
	public ReadOnlyCollection<ValidatorControlPair> LastFailedValidationResults => new ReadOnlyCollection<ValidatorControlPair>(__2FEA);

	[Description("Indicates default ErrorProvider IconPadding when Highlighter is used with SuperValidator. Setting this property to 0 will allow you to set IconPadding for each control manually.")]
	[DefaultValue(2)]
	[Category("Behavior")]
	public int DefaultErrorProviderHighlighterIconPadding
	{
		get
		{
			return __2FEB;
		}
		set
		{
			__2FEB = value;
		}
	}

	[Description("Indicates error provider that is used by the validator to report validation errors.")]
	[Category("Behavior")]
	[DefaultValue(null)]
	public ErrorProvider ErrorProvider
	{
		get
		{
			return __2FEC;
		}
		set
		{
			__2FEC = value;
		}
	}

	[Browsable(false)]
	[Description("Indicates custom error provider that is used by validator to report errors. You can provide your own error validators by implementing IErrorProvider interface.")]
	[Category("Behavior")]
	[DefaultValue(null)]
	public IErrorProvider CustomErrorProvider
	{
		get
		{
			return __2FED;
		}
		set
		{
			__2FED = value;
		}
	}

	[Description("Highlighter component that is used to highlight validation errors.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Highlighter Highlighter
	{
		get
		{
			return __2FEE;
		}
		set
		{
			__2FEE = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates validation type performed by the control.")]
	[DefaultValue(eValidationType.Manual)]
	public eValidationType ValidationType
	{
		get
		{
			return __2FEF;
		}
		set
		{
			__2FEF = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates container control validator is bound to. Should be set to parent form.")]
	[DefaultValue(null)]
	public Control ContainerControl
	{
		get
		{
			return __2FF0;
		}
		set
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			if (__2FF0 == value)
			{
				return;
			}
			if (__2FF0 != null)
			{
				if (__2FF0 is Form)
				{
					((Form)__2FF0).FormClosing -= new FormClosingEventHandler(ContainerFormClosing);
				}
				else
				{
					__2FF0.Validating -= ContainerValidating;
				}
			}
			__2FF0 = value;
			if (__2FF0 != null)
			{
				if (__2FF0 is Form)
				{
					((Form)__2FF0).FormClosing += new FormClosingEventHandler(ContainerFormClosing);
				}
				else
				{
					__2FF0.Validating += ContainerValidating;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether validation is performed.")]
	[Category("Behavior")]
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

	[Description("Indicates whether Cancel argument in Validating event for validation type ValidatingEventPerControl is set to true when validation fails.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool CancelValidatingOnControl
	{
		get
		{
			return __2FF1;
		}
		set
		{
			__2FF1 = value;
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

	public event ValidateValueEventHandler CustomValidatorValidateValue
	{
		add
		{
			ValidateValueEventHandler validateValueEventHandler = _2FE8;
			ValidateValueEventHandler validateValueEventHandler2;
			do
			{
				validateValueEventHandler2 = validateValueEventHandler;
				ValidateValueEventHandler value2 = (ValidateValueEventHandler)Delegate.Combine(validateValueEventHandler2, value);
				validateValueEventHandler = Interlocked.CompareExchange(ref _2FE8, value2, validateValueEventHandler2);
			}
			while ((object)validateValueEventHandler != validateValueEventHandler2);
		}
		remove
		{
			ValidateValueEventHandler validateValueEventHandler = _2FE8;
			ValidateValueEventHandler validateValueEventHandler2;
			do
			{
				validateValueEventHandler2 = validateValueEventHandler;
				ValidateValueEventHandler value2 = (ValidateValueEventHandler)Delegate.Remove(validateValueEventHandler2, value);
				validateValueEventHandler = Interlocked.CompareExchange(ref _2FE8, value2, validateValueEventHandler2);
			}
			while ((object)validateValueEventHandler != validateValueEventHandler2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		ClearAllValidators();
		ContainerControl = null;
		base.Dispose(disposing);
	}

	private void ClearAllValidators()
	{
		ClearValidator(__2FE5);
		ClearValidator(__2FE6);
		ClearValidator(__2FE7);
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
		_2FE8?.Invoke(validator, e);
	}

	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.ValidatorEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public ValidatorBase GetValidator1(Control c)
	{
		if (__2FE5.ContainsKey(c))
		{
			return __2FE5[c];
		}
		return null;
	}

	public void SetValidator1(Control c, ValidatorBase info)
	{
		if (__2FE5.ContainsKey(c))
		{
			if (info == null)
			{
				ValidatorBase validator = __2FE5[c];
				RemoveValidator(__2FE5, c);
				DestroyValidator(validator);
				return;
			}
			ValidatorBase validatorBase = __2FE5[c];
			if (validatorBase != info)
			{
				DestroyValidator(validatorBase);
			}
			__2FE5[c] = info;
		}
		else if (info != null)
		{
			AddValidator(__2FE5, c, info);
		}
	}

	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.ValidatorEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public ValidatorBase GetValidator2(Control c)
	{
		if (__2FE6.ContainsKey(c))
		{
			return __2FE6[c];
		}
		return null;
	}

	public void SetValidator2(Control c, ValidatorBase info)
	{
		if (__2FE6.ContainsKey(c))
		{
			if (info == null)
			{
				ValidatorBase validator = __2FE6[c];
				RemoveValidator(__2FE6, c);
				DestroyValidator(validator);
				return;
			}
			ValidatorBase validatorBase = __2FE6[c];
			if (validatorBase != info)
			{
				DestroyValidator(validatorBase);
			}
			__2FE6[c] = info;
		}
		else if (info != null)
		{
			AddValidator(__2FE6, c, info);
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ValidatorEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	[DefaultValue(null)]
	public ValidatorBase GetValidator3(Control c)
	{
		if (__2FE7.ContainsKey(c))
		{
			return __2FE7[c];
		}
		return null;
	}

	public void SetValidator3(Control c, ValidatorBase info)
	{
		if (__2FE7.ContainsKey(c))
		{
			if (info == null)
			{
				ValidatorBase validator = __2FE7[c];
				RemoveValidator(__2FE7, c);
				DestroyValidator(validator);
				return;
			}
			ValidatorBase validatorBase = __2FE7[c];
			if (validatorBase != info)
			{
				DestroyValidator(validatorBase);
			}
			__2FE7[c] = info;
		}
		else if (info != null)
		{
			AddValidator(__2FE7, c, info);
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
		if (!Enabled || __2FEF == eValidationType.Manual || (__2FEF & eValidationType.ValidatingEventPerControl) != eValidationType.ValidatingEventPerControl)
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
		if (__2FE5.TryGetValue(val, out value))
		{
			bool cancelValidating = false;
			flag = ValidateSingleControl(value, val, out cancelValidating);
			if (!flag)
			{
				AddToLastFailedValidations(value, val);
			}
			e.Cancel = cancelValidating;
		}
		if (flag && __2FE6.TryGetValue(val, out value))
		{
			bool cancelValidating2 = false;
			flag = ValidateSingleControl(value, val, out cancelValidating2);
			if (!flag)
			{
				AddToLastFailedValidations(value, val);
			}
			e.Cancel = cancelValidating2;
		}
		if (flag && __2FE7.TryGetValue(val, out value))
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
		foreach (ValidatorControlPair item in __2FEA)
		{
			if (item.Control == control && item.Validator == validator)
			{
				return;
			}
		}
		__2FEA.Add(new ValidatorControlPair(validator, control));
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
				cancelValidating = __2FF1;
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
		FindOptionalValidators(__2FE5, optionalValidationGroup, list);
		FindOptionalValidators(__2FE6, optionalValidationGroup, list);
		FindOptionalValidators(__2FE7, optionalValidationGroup, list);
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
		_239B?.Invoke(this, e);
	}

	internal void InvokeGetValue(ValidatorGetValueEventArgs e)
	{
		OnGetValue(e);
	}

	public bool Validate()
	{
		bool flag = true;
		Dictionary<string, bool> table = new Dictionary<string, bool>();
		__2FEA.Clear();
		flag &= ValidateSingleTable(__2FE5, table);
		if (!flag && __2FE9)
		{
			return flag;
		}
		flag &= ValidateSingleTable(__2FE6, table);
		if (!flag && __2FE9)
		{
			return flag;
		}
		return flag & ValidateSingleTable(__2FE7, table);
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
		if (__2FE5.ContainsKey(controlToValidate))
		{
			validatorBase = __2FE5[controlToValidate];
			bool flag2 = ValidateSingleControl(validatorBase, controlToValidate, out cancelValidating);
			if (!flag2)
			{
				__2FEA.Add(new ValidatorControlPair(validatorBase, controlToValidate));
			}
			flag = flag && flag2;
		}
		if (!flag && __2FE9)
		{
			return flag;
		}
		if (__2FE6.ContainsKey(controlToValidate))
		{
			validatorBase = __2FE6[controlToValidate];
			bool flag3 = ValidateSingleControl(validatorBase, controlToValidate, out cancelValidating);
			if (!flag3)
			{
				__2FEA.Add(new ValidatorControlPair(validatorBase, controlToValidate));
			}
			flag = flag && flag3;
		}
		if (!flag && __2FE9)
		{
			return flag;
		}
		if (__2FE7.ContainsKey(controlToValidate))
		{
			validatorBase = __2FE7[controlToValidate];
			bool flag4 = ValidateSingleControl(validatorBase, controlToValidate, out cancelValidating);
			if (!flag4)
			{
				__2FEA.Add(new ValidatorControlPair(validatorBase, controlToValidate));
			}
			flag = flag && flag4;
		}
		if (!flag && __2FE9)
		{
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
				__2FEA.Add(new ValidatorControlPair(validator.Value, validator.Key));
			}
			flag = flag && flag2;
		}
		return flag;
	}

	private bool ControlFailedValidation(Control control)
	{
		foreach (ValidatorControlPair item in __2FEA)
		{
			if (item.Control == control)
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
		foreach (ValidatorControlPair item in __2FEA)
		{
			ClearError(item.Validator, item.Control);
		}
		__2FEA.Clear();
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
		if (__2FEC != null)
		{
			list.Add(new _3008(__2FEC, (__2FEE != null) ? __2FEB : 0));
		}
		if (__2FED != null)
		{
			list.Add(__2FED);
		}
		if (__2FEE != null)
		{
			list.Add(__2FEE);
		}
		return list;
	}

	private void ContainerFormClosing(object sender, FormClosingEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Enabled && (__2FEF & eValidationType.ValidatingEventOnContainer) == eValidationType.ValidatingEventOnContainer && ((Control)(Form)sender).CausesValidation)
		{
			((CancelEventArgs)(object)e).Cancel = !Validate();
		}
	}

	private void ContainerValidating(object sender, CancelEventArgs e)
	{
		if (Enabled && __2FEF == eValidationType.ValidatingEventOnContainer)
		{
			e.Cancel = !Validate();
		}
	}

	public bool CanExtend(object extendee)
	{
		return extendee is Control;
	}
}
