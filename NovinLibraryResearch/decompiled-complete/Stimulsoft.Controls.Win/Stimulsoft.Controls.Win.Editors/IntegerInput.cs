using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxBitmap(typeof(DotNetBarManager), "IntegerInput.ico")]
[ToolboxItem(false)]
public class IntegerInput : NumericInputBase
{
	private VisualIntegerInput _IntegerInput;

	private VisualInputGroup _InputGroup;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel increases or decreases the input value when used.")]
	public bool MouseWheelValueChangeEnabled
	{
		get
		{
			return _IntegerInput.MouseWheelValueChangeEnabled;
		}
		set
		{
			_IntegerInput.MouseWheelValueChangeEnabled = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates value displayed in the control.")]
	public int Value
	{
		get
		{
			return _IntegerInput.Value;
		}
		set
		{
			if (Value != value || _IntegerInput.IsEmpty)
			{
				_IntegerInput.Value = value;
			}
		}
	}

	[Bindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[TypeConverter(typeof(StringConverter))]
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
			if (AcceptCustomValueObject(value))
			{
				return;
			}
			if (IsNull(value))
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

	[DefaultValue(int.MaxValue)]
	[Description("Indicates maximum value that can be entered.")]
	public int MaxValue
	{
		get
		{
			return _IntegerInput.MaxValue;
		}
		set
		{
			_IntegerInput.MaxValue = value;
		}
	}

	[DefaultValue(int.MinValue)]
	[Description("Indicates minimum value that can be entered.")]
	public int MinValue
	{
		get
		{
			return _IntegerInput.MinValue;
		}
		set
		{
			_IntegerInput.MinValue = value;
		}
	}

	[DefaultValue(1)]
	[Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
	public int Increment
	{
		get
		{
			return _IntegerInput.Increment;
		}
		set
		{
			_IntegerInput.Increment = value;
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (_FreeTextEntryBox == null || !((Control)_FreeTextEntryBox).Focused))
			{
				return _IntegerInput.IsEmpty;
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
			return _IntegerInput.Text;
		}
		set
		{
			ValueObject = value;
		}
	}

	public event ParseIntegerValueEventHandler ParseValue;

	public override void DecreaseValue()
	{
		if (base.FreeTextEntryMode)
		{
			ApplyFreeTextValue();
		}
		_IntegerInput.DecreaseValue();
		if (base.FreeTextEntryMode && _FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = ((Control)this).Text;
		}
	}

	public override void IncreaseValue()
	{
		if (base.FreeTextEntryMode)
		{
			ApplyFreeTextValue();
		}
		_IntegerInput.IncreaseValue();
		if (base.FreeTextEntryMode && _FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = ((Control)this).Text;
		}
	}

	public virtual void Copy()
	{
		if (_IntegerInput != null)
		{
			_IntegerInput.ProcessClipboardCopy();
		}
	}

	public virtual void Paste()
	{
		if (_IntegerInput != null)
		{
			_IntegerInput.ProcessClipboardPaste();
		}
	}

	public virtual void Cut()
	{
		if (_IntegerInput != null)
		{
			_IntegerInput.ProcessClipboardCut();
		}
	}

	protected override VisualItem CreateRootVisual()
	{
		VisualInputGroup visualInputGroup = new VisualInputGroup();
		VisualIntegerInput visualIntegerInput = new VisualIntegerInput();
		visualIntegerInput.ValueChanged += InputItemValueChanged;
		visualInputGroup.Items.Add(visualIntegerInput);
		_InputGroup = visualInputGroup;
		_IntegerInput = visualIntegerInput;
		return visualInputGroup;
	}

	protected override void UpdateInputFieldAlignment()
	{
		if (InputHorizontalAlignment == eHorizontalAlignment.Right)
		{
			if (!base.ButtonClear.Visible && !base.ButtonDropDown.Visible && !base.ButtonFreeText.Visible && !base.ButtonCustom.Visible && !base.ButtonCustom2.Visible && !base.ButtonCalculator.Visible && !base.ShowUpDown)
			{
				_IntegerInput.Alignment = eItemAlignment.Right;
			}
			else
			{
				_IntegerInput.Alignment = eItemAlignment.Left;
			}
		}
		else
		{
			_IntegerInput.Alignment = eItemAlignment.Left;
		}
		base.UpdateInputFieldAlignment();
	}

	private void InputItemValueChanged(object sender, EventArgs e)
	{
		OnValueChanged(e);
		if (base.FreeTextEntryMode && _FreeTextEntryBox != null && ((Control)_FreeTextEntryBox).Text != ((Control)this).Text)
		{
			((Control)_FreeTextEntryBox).Text = ((Control)this).Text;
		}
	}

	protected override void OnDisplayFormatChanged()
	{
		_IntegerInput.DisplayFormat = base.DisplayFormat;
	}

	private bool AcceptCustomValueObject(object value)
	{
		ParseIntegerValueEventArgs parseIntegerValueEventArgs = new ParseIntegerValueEventArgs(value);
		OnParseValue(parseIntegerValueEventArgs);
		if (parseIntegerValueEventArgs.IsParsed)
		{
			Value = parseIntegerValueEventArgs.ParsedValue;
		}
		return parseIntegerValueEventArgs.IsParsed;
	}

	protected virtual void OnParseValue(ParseIntegerValueEventArgs e)
	{
		if (this.ParseValue != null)
		{
			this.ParseValue(this, e);
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
		if (_FreeTextEntryBox == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(((Control)_FreeTextEntryBox).Text))
		{
			ValueObject = null;
			return;
		}
		if (int.TryParse(((Control)_FreeTextEntryBox).Text, out var result) && base.AutoResolveFreeTextEntries)
		{
			Value = result;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)_FreeTextEntryBox).Text);
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
