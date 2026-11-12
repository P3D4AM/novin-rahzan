using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxBitmap(typeof(DotNetBarManager), "DoubleInput.ico")]
[ToolboxItem(false)]
public class DoubleInput : NumericInputBase
{
	private VisualDoubleInput _DoubleInput;

	private VisualInputGroup _InputGroup;

	internal VisualInputGroup InputGroup => _InputGroup;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel increases or decreases the input value when used.")]
	public bool MouseWheelValueChangeEnabled
	{
		get
		{
			return _DoubleInput.MouseWheelValueChangeEnabled;
		}
		set
		{
			_DoubleInput.MouseWheelValueChangeEnabled = value;
		}
	}

	[DefaultValue(0.0)]
	[Description("Indicates value displayed in the control.")]
	public double Value
	{
		get
		{
			return _DoubleInput.Value;
		}
		set
		{
			if (Value != value || _DoubleInput.IsEmpty)
			{
				_DoubleInput.Value = value;
			}
		}
	}

	[Bindable(true)]
	[RefreshProperties(RefreshProperties.All)]
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
			if (value is double)
			{
				Value = (double)value;
				return;
			}
			if (value is int)
			{
				Value = (int)value;
				return;
			}
			if (value is float)
			{
				Value = (float)value;
				return;
			}
			if (value is decimal)
			{
				Value = decimal.ToDouble((decimal)value);
				return;
			}
			if (value is long)
			{
				string s = value.ToString();
				Value = double.Parse(s);
				return;
			}
			if (value is string)
			{
				double result = 0.0;
				if (double.TryParse(value.ToString(), out result))
				{
					Value = result;
					return;
				}
				throw new ArgumentException("ValueObject property expects either null/nothing value or double type.");
			}
			throw new ArgumentException("ValueObject property expects either null/nothing value or double type.");
		}
	}

	[DefaultValue(double.MaxValue)]
	[Description("Indicates maximum value that can be entered.")]
	public double MaxValue
	{
		get
		{
			return _DoubleInput.MaxValue;
		}
		set
		{
			_DoubleInput.MaxValue = value;
		}
	}

	[DefaultValue(double.MinValue)]
	[Description("Indicates minimum value that can be entered.")]
	public double MinValue
	{
		get
		{
			return _DoubleInput.MinValue;
		}
		set
		{
			_DoubleInput.MinValue = value;
		}
	}

	[DefaultValue(1)]
	[Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
	public double Increment
	{
		get
		{
			return _DoubleInput.Increment;
		}
		set
		{
			_DoubleInput.Increment = value;
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (_FreeTextEntryBox == null || !((Control)_FreeTextEntryBox).Focused))
			{
				return _DoubleInput.IsEmpty;
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
			return _DoubleInput.Text;
		}
		set
		{
			ValueObject = value;
		}
	}

	internal VisualDoubleInput VisualDoubleInput => _DoubleInput;

	public event ParseDoubleValueEventHandler ParseValue;

	public virtual void Copy()
	{
		if (_DoubleInput != null)
		{
			_DoubleInput.ProcessClipboardCopy();
		}
	}

	public virtual void Paste()
	{
		if (_DoubleInput != null)
		{
			_DoubleInput.ProcessClipboardPaste();
		}
	}

	public virtual void Cut()
	{
		if (_DoubleInput != null)
		{
			_DoubleInput.ProcessClipboardCut();
		}
	}

	protected override VisualItem CreateRootVisual()
	{
		VisualInputGroup visualInputGroup = new VisualInputGroup();
		VisualDoubleInput visualDoubleInput = new VisualDoubleInput();
		visualDoubleInput.ValueChanged += InputItemValueChanged;
		visualInputGroup.Items.Add(visualDoubleInput);
		_InputGroup = visualInputGroup;
		_DoubleInput = visualDoubleInput;
		return visualInputGroup;
	}

	protected override void UpdateInputFieldAlignment()
	{
		if (InputHorizontalAlignment == eHorizontalAlignment.Right)
		{
			if (!base.ButtonClear.Visible && !base.ButtonDropDown.Visible && !base.ButtonFreeText.Visible && !base.ButtonCustom.Visible && !base.ButtonCustom2.Visible && !base.ButtonCalculator.Visible && !base.ShowUpDown)
			{
				_DoubleInput.Alignment = eItemAlignment.Right;
			}
			else
			{
				_DoubleInput.Alignment = eItemAlignment.Left;
			}
		}
		else
		{
			_DoubleInput.Alignment = eItemAlignment.Left;
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
		_DoubleInput.DisplayFormat = base.DisplayFormat;
	}

	private bool AcceptCustomValueObject(object value)
	{
		ParseDoubleValueEventArgs parseDoubleValueEventArgs = new ParseDoubleValueEventArgs(value);
		OnParseValue(parseDoubleValueEventArgs);
		if (parseDoubleValueEventArgs.IsParsed)
		{
			Value = parseDoubleValueEventArgs.ParsedValue;
		}
		return parseDoubleValueEventArgs.IsParsed;
	}

	protected virtual void OnParseValue(ParseDoubleValueEventArgs e)
	{
		if (this.ParseValue != null)
		{
			this.ParseValue(this, e);
		}
	}

	public override void DecreaseValue()
	{
		if (base.FreeTextEntryMode)
		{
			ApplyFreeTextValue();
		}
		_DoubleInput.DecreaseValue();
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
		_DoubleInput.IncreaseValue();
		if (base.FreeTextEntryMode && _FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = ((Control)this).Text;
		}
	}

	protected override void DropDownCalculatorValueChanged(object sender, ValueChangedEventArgs e)
	{
		Value = e.Value;
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
		if (double.TryParse(((Control)_FreeTextEntryBox).Text, out var result) && base.AutoResolveFreeTextEntries)
		{
			Value = result;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)_FreeTextEntryBox).Text);
		OnConvertFreeTextEntry(freeTextEntryConversionEventArgs);
		if (freeTextEntryConversionEventArgs.IsValueConverted)
		{
			if (freeTextEntryConversionEventArgs.ControlValue is double)
			{
				Value = (double)freeTextEntryConversionEventArgs.ControlValue;
				return;
			}
			if (freeTextEntryConversionEventArgs.ControlValue != null)
			{
				throw new ArgumentException("ControlValue assigned is not double type.");
			}
			ValueObject = null;
		}
		else
		{
			Value = 0.0;
		}
	}
}
