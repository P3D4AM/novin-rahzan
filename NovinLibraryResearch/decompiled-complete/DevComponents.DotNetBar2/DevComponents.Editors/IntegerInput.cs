using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

[DefaultBindingProperty("ValueObject")]
[DefaultProperty("ValueObject")]
[ToolboxItem(true)]
[ToolboxBitmap(typeof(DotNetBarManager), "IntegerInput.ico")]
[Designer("DevComponents.DotNetBar.Design.NumericInputBaseDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class IntegerInput : NumericInputBase
{
	private VisualIntegerInput __22B7;

	private VisualInputGroup __22B8;

	private ParseIntegerValueEventHandler _៨;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel increases or decreases the input value when used.")]
	public bool MouseWheelValueChangeEnabled
	{
		get
		{
			return __22B7.MouseWheelValueChangeEnabled;
		}
		set
		{
			__22B7.MouseWheelValueChangeEnabled = value;
		}
	}

	[Description("Indicates value displayed in the control.")]
	[DefaultValue(0)]
	public int Value
	{
		get
		{
			return __22B7.Value;
		}
		set
		{
			if (Value != value || __22B7.IsEmpty)
			{
				__22B7.Value = value;
			}
		}
	}

	[Bindable(true)]
	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[RefreshProperties(RefreshProperties.All)]
	public object ValueObject
	{
		get
		{
			if (base.IsEmpty)
			{
				return null;
			}
			return Value;
		}
		set
		{
			if (_1805(value))
			{
				return;
			}
			if (IsNull(value) || (value is string && string.IsNullOrEmpty((string)value)))
			{
				base.IsEmpty = true;
				return;
			}
			if (value is int)
			{
				Value = (int)value;
				return;
			}
			if (value is byte || value is short)
			{
				Value = (int)value;
				return;
			}
			if (value is string)
			{
				int result = 0;
				if (int.TryParse(value.ToString(), out result))
				{
					Value = result;
					return;
				}
				throw new ArgumentException("ValueObject property expects either null/nothing value or int type.");
			}
			throw new ArgumentException("ValueObject property expects either null/nothing value or int type.");
		}
	}

	[Description("Indicates maximum value that can be entered.")]
	[DefaultValue(int.MaxValue)]
	public int MaxValue
	{
		get
		{
			return __22B7.MaxValue;
		}
		set
		{
			__22B7.MaxValue = value;
		}
	}

	[Description("Indicates minimum value that can be entered.")]
	[DefaultValue(int.MinValue)]
	public int MinValue
	{
		get
		{
			return __22B7.MinValue;
		}
		set
		{
			__22B7.MinValue = value;
		}
	}

	[DefaultValue(1)]
	[Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
	public int Increment
	{
		get
		{
			return __22B7.Increment;
		}
		set
		{
			__22B7.Increment = value;
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (__1800 == null || !((Control)__1800).Focused))
			{
				return __22B7.IsEmpty;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Text
	{
		get
		{
			return __22B7.Text;
		}
		set
		{
			ValueObject = value;
		}
	}

	public event ParseIntegerValueEventHandler ParseValue
	{
		add
		{
			ParseIntegerValueEventHandler parseIntegerValueEventHandler = _៨;
			ParseIntegerValueEventHandler parseIntegerValueEventHandler2;
			do
			{
				parseIntegerValueEventHandler2 = parseIntegerValueEventHandler;
				ParseIntegerValueEventHandler value2 = (ParseIntegerValueEventHandler)Delegate.Combine(parseIntegerValueEventHandler2, value);
				parseIntegerValueEventHandler = Interlocked.CompareExchange(ref _៨, value2, parseIntegerValueEventHandler2);
			}
			while ((object)parseIntegerValueEventHandler != parseIntegerValueEventHandler2);
		}
		remove
		{
			ParseIntegerValueEventHandler parseIntegerValueEventHandler = _៨;
			ParseIntegerValueEventHandler parseIntegerValueEventHandler2;
			do
			{
				parseIntegerValueEventHandler2 = parseIntegerValueEventHandler;
				ParseIntegerValueEventHandler value2 = (ParseIntegerValueEventHandler)Delegate.Remove(parseIntegerValueEventHandler2, value);
				parseIntegerValueEventHandler = Interlocked.CompareExchange(ref _៨, value2, parseIntegerValueEventHandler2);
			}
			while ((object)parseIntegerValueEventHandler != parseIntegerValueEventHandler2);
		}
	}

	public override void DecreaseValue()
	{
		if (base.FreeTextEntryMode)
		{
			ApplyFreeTextValue();
		}
		__22B7.DecreaseValue();
		if (base.FreeTextEntryMode && __1800 != null)
		{
			((Control)__1800).Text = ((Control)this).Text;
		}
	}

	public override void IncreaseValue()
	{
		if (base.FreeTextEntryMode)
		{
			ApplyFreeTextValue();
		}
		__22B7.IncreaseValue();
		if (base.FreeTextEntryMode && __1800 != null)
		{
			((Control)__1800).Text = ((Control)this).Text;
		}
	}

	public virtual void Copy()
	{
		if (__22B7 != null)
		{
			__22B7._22E1();
		}
	}

	public virtual void Paste()
	{
		if (__22B7 != null)
		{
			__22B7._22E3();
		}
	}

	public virtual void Cut()
	{
		if (__22B7 != null)
		{
			__22B7._22E2();
		}
	}

	protected override VisualItem CreateRootVisual()
	{
		VisualInputGroup visualInputGroup = new VisualInputGroup();
		VisualIntegerInput visualIntegerInput = new VisualIntegerInput();
		visualIntegerInput.ValueChanged += _22B9;
		visualInputGroup.Items.Add(visualIntegerInput);
		__22B8 = visualInputGroup;
		__22B7 = visualIntegerInput;
		return visualInputGroup;
	}

	protected override void UpdateInputFieldAlignment()
	{
		if (InputHorizontalAlignment == eHorizontalAlignment.Right)
		{
			if (!base.ButtonClear.Visible && !base.ButtonDropDown.Visible && !base.ButtonFreeText.Visible && !base.ButtonCustom.Visible && !base.ButtonCustom2.Visible && !base.ButtonCalculator.Visible && !base.ShowUpDown)
			{
				__22B7.Alignment = eItemAlignment.Right;
			}
			else
			{
				__22B7.Alignment = eItemAlignment.Left;
			}
		}
		else
		{
			__22B7.Alignment = eItemAlignment.Left;
		}
		base.UpdateInputFieldAlignment();
	}

	private void _22B9(object _٠, EventArgs ؿ)
	{
		OnValueChanged(ؿ);
		if (base.FreeTextEntryMode && __1800 != null && ((Control)__1800).Text != ((Control)this).Text)
		{
			((Control)__1800).Text = ((Control)this).Text;
		}
	}

	protected override void OnDisplayFormatChanged()
	{
		__22B7.DisplayFormat = base.DisplayFormat;
	}

	private bool _1805(object ԝ)
	{
		ParseIntegerValueEventArgs parseIntegerValueEventArgs = new ParseIntegerValueEventArgs(ԝ);
		OnParseValue(parseIntegerValueEventArgs);
		if (parseIntegerValueEventArgs.IsParsed)
		{
			Value = parseIntegerValueEventArgs.ParsedValue;
		}
		return parseIntegerValueEventArgs.IsParsed;
	}

	protected virtual void OnParseValue(ParseIntegerValueEventArgs e)
	{
		if (_៨ != null)
		{
			_៨(this, e);
		}
	}

	protected override void OnDropDownCalculatorCreated(Calculator calculator)
	{
		base.OnDropDownCalculatorCreated(calculator);
		calculator.IsIntValue = true;
	}

	protected override void DropDownCalculatorValueChanged(object sender, ValueChangedEventArgs e)
	{
		Value = (int)e.Value;
		base.DropDownCalculatorValueChanged(sender, e);
	}

	protected override void UpdateCalculatorValue(Calculator calculator)
	{
		calculator.Value = Value;
	}

	protected override void ApplyFreeTextValue()
	{
		if (__1800 == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(((Control)__1800).Text))
		{
			ValueObject = null;
			return;
		}
		if (int.TryParse(((Control)__1800).Text, out var result) && base.AutoResolveFreeTextEntries)
		{
			Value = result;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)__1800).Text);
		OnConvertFreeTextEntry(freeTextEntryConversionEventArgs);
		if (freeTextEntryConversionEventArgs.IsValueConverted)
		{
			if (freeTextEntryConversionEventArgs.ControlValue is int)
			{
				Value = (int)freeTextEntryConversionEventArgs.ControlValue;
				return;
			}
			if (freeTextEntryConversionEventArgs.ControlValue != null)
			{
				throw new ArgumentException("ControlValue assigned is not int type.");
			}
			ValueObject = null;
		}
		else
		{
			Value = 0;
		}
	}
}
