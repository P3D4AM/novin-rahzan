using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

[DefaultBindingProperty("ValueObject")]
[ToolboxItem(true)]
[DefaultProperty("ValueObject")]
[Designer("DevComponents.DotNetBar.Design.NumericInputBaseDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(DotNetBarManager), "DoubleInput.ico")]
public class DoubleInput : NumericInputBase
{
	private VisualDoubleInput __29C5;

	private VisualInputGroup __22B8;

	private ParseDoubleValueEventHandler _៨;

	internal VisualInputGroup _29C3 => __22B8;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel increases or decreases the input value when used.")]
	public bool MouseWheelValueChangeEnabled
	{
		get
		{
			return __29C5.MouseWheelValueChangeEnabled;
		}
		set
		{
			__29C5.MouseWheelValueChangeEnabled = value;
		}
	}

	[DefaultValue(0.0)]
	[Description("Indicates value displayed in the control.")]
	public double Value
	{
		get
		{
			return __29C5.Value;
		}
		set
		{
			if (Value != value || __29C5.IsEmpty)
			{
				__29C5.Value = value;
			}
		}
	}

	[TypeConverter(typeof(StringConverter))]
	[Bindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
			return __29C5.MaxValue;
		}
		set
		{
			__29C5.MaxValue = value;
		}
	}

	[Description("Indicates minimum value that can be entered.")]
	[DefaultValue(double.MinValue)]
	public double MinValue
	{
		get
		{
			return __29C5.MinValue;
		}
		set
		{
			__29C5.MinValue = value;
		}
	}

	[Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
	[DefaultValue(1)]
	public double Increment
	{
		get
		{
			return __29C5.Increment;
		}
		set
		{
			__29C5.Increment = value;
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (__1800 == null || !((Control)__1800).Focused))
			{
				return __29C5.IsEmpty;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override string Text
	{
		get
		{
			return __29C5.Text;
		}
		set
		{
			ValueObject = value;
		}
	}

	internal VisualDoubleInput _29C4 => __29C5;

	public event ParseDoubleValueEventHandler ParseValue
	{
		add
		{
			ParseDoubleValueEventHandler parseDoubleValueEventHandler = _៨;
			ParseDoubleValueEventHandler parseDoubleValueEventHandler2;
			do
			{
				parseDoubleValueEventHandler2 = parseDoubleValueEventHandler;
				ParseDoubleValueEventHandler value2 = (ParseDoubleValueEventHandler)Delegate.Combine(parseDoubleValueEventHandler2, value);
				parseDoubleValueEventHandler = Interlocked.CompareExchange(ref _៨, value2, parseDoubleValueEventHandler2);
			}
			while ((object)parseDoubleValueEventHandler != parseDoubleValueEventHandler2);
		}
		remove
		{
			ParseDoubleValueEventHandler parseDoubleValueEventHandler = _៨;
			ParseDoubleValueEventHandler parseDoubleValueEventHandler2;
			do
			{
				parseDoubleValueEventHandler2 = parseDoubleValueEventHandler;
				ParseDoubleValueEventHandler value2 = (ParseDoubleValueEventHandler)Delegate.Remove(parseDoubleValueEventHandler2, value);
				parseDoubleValueEventHandler = Interlocked.CompareExchange(ref _៨, value2, parseDoubleValueEventHandler2);
			}
			while ((object)parseDoubleValueEventHandler != parseDoubleValueEventHandler2);
		}
	}

	public virtual void Copy()
	{
		if (__29C5 != null)
		{
			__29C5._22E1();
		}
	}

	public virtual void Paste()
	{
		if (__29C5 != null)
		{
			__29C5._22E3();
		}
	}

	public virtual void Cut()
	{
		if (__29C5 != null)
		{
			__29C5._22E2();
		}
	}

	protected override VisualItem CreateRootVisual()
	{
		VisualInputGroup visualInputGroup = new VisualInputGroup();
		VisualDoubleInput visualDoubleInput = new VisualDoubleInput();
		visualDoubleInput.ValueChanged += _22B9;
		visualInputGroup.Items.Add(visualDoubleInput);
		__22B8 = visualInputGroup;
		__29C5 = visualDoubleInput;
		return visualInputGroup;
	}

	protected override void UpdateInputFieldAlignment()
	{
		if (InputHorizontalAlignment == eHorizontalAlignment.Right)
		{
			if (!base.ButtonClear.Visible && !base.ButtonDropDown.Visible && !base.ButtonFreeText.Visible && !base.ButtonCustom.Visible && !base.ButtonCustom2.Visible && !base.ButtonCalculator.Visible && !base.ShowUpDown)
			{
				__29C5.Alignment = eItemAlignment.Right;
			}
			else
			{
				__29C5.Alignment = eItemAlignment.Left;
			}
		}
		else
		{
			__29C5.Alignment = eItemAlignment.Left;
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
		__29C5.DisplayFormat = base.DisplayFormat;
	}

	private bool _1805(object ԝ)
	{
		ParseDoubleValueEventArgs parseDoubleValueEventArgs = new ParseDoubleValueEventArgs(ԝ);
		OnParseValue(parseDoubleValueEventArgs);
		if (parseDoubleValueEventArgs.IsParsed)
		{
			Value = parseDoubleValueEventArgs.ParsedValue;
		}
		return parseDoubleValueEventArgs.IsParsed;
	}

	protected virtual void OnParseValue(ParseDoubleValueEventArgs e)
	{
		if (_៨ != null)
		{
			_៨(this, e);
		}
	}

	public override void DecreaseValue()
	{
		if (base.FreeTextEntryMode)
		{
			ApplyFreeTextValue();
		}
		__29C5.DecreaseValue();
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
		__29C5.IncreaseValue();
		if (base.FreeTextEntryMode && __1800 != null)
		{
			((Control)__1800).Text = ((Control)this).Text;
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
		if (__1800 == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(((Control)__1800).Text))
		{
			ValueObject = null;
			return;
		}
		if (double.TryParse(((Control)__1800).Text, out var result) && base.AutoResolveFreeTextEntries)
		{
			Value = result;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)__1800).Text);
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
