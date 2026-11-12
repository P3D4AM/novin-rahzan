using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxBitmap(typeof(DotNetBarManager), "Calculator.ico")]
[DefaultEvent("ValueChanged")]
[DefaultProperty("Value")]
[ToolboxItem(false)]
public class Calculator : Control
{
	private string _StringValue;

	private double _Value;

	private double _LValue;

	private double _MemValue;

	private Operators _Operator;

	private Operators _LastOp;

	private bool _IsIntValue;

	private bool _ShowMemKeys;

	private DecimalKeyVisibility _DecimalKeyVisibility;

	private Timer _KeyTimer;

	private string _NumberDecimalSeparator;

	private Size _PreferredSize = Size.Empty;

	private bool _FocusButtonsOnMouseDown = true;

	private bool _DisplayVisible = true;

	private IContainer components;

	private PanelEx pnlCalc;

	private LabelX lbxMemory;

	private LabelX lbxOperator;

	private PanelEx pnlMem;

	private PanelEx pnlPad;

	public ButtonX BtnNegate;

	public ButtonX BtnDecimal;

	public ButtonX BtnDigit7;

	public ButtonX BtnBack;

	public ButtonX BtnMemClear;

	public ButtonX BtnMemRestore;

	public ButtonX BtnMemStore;

	public ButtonX BtnMemSubtract;

	public ButtonX BtnSqrt;

	public ButtonX BtnPercent;

	public ButtonX BtnReciprocal;

	public ButtonX BtnDivide;

	public ButtonX BtnMultiply;

	public ButtonX BtnSubtract;

	public ButtonX BtnAdd;

	public ButtonX BtnClear;

	public ButtonX BtnDigit4;

	public ButtonX BtnDigit2;

	public ButtonX BtnClearEntry;

	public ButtonX BtnDigit9;

	public ButtonX BtnDigit6;

	public ButtonX BtnDigit3;

	public ButtonX BtnDigit1;

	public ButtonX BtnDigit8;

	public ButtonX BtnDigit5;

	public ButtonX BtnDigit0;

	public ButtonX BtnMemAdd;

	public ButtonX BtnEquals;

	private LabelX labelValue;

	[Browsable(false)]
	public double DisplayValue
	{
		get
		{
			if (string.IsNullOrEmpty(_StringValue))
			{
				return _Value;
			}
			return double.Parse(_StringValue);
		}
	}

	[Browsable(false)]
	public string DisplaySValue
	{
		get
		{
			if (!string.IsNullOrEmpty(_StringValue))
			{
				return _StringValue;
			}
			return _Value.ToString();
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether calculator displays only Integer values.")]
	public bool IsIntValue
	{
		get
		{
			return _IsIntValue;
		}
		set
		{
			if (_IsIntValue != value)
			{
				_IsIntValue = value;
				UpdateLayout();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether memory keys are visible.")]
	public bool ShowMemKeys
	{
		get
		{
			return _ShowMemKeys;
		}
		set
		{
			if (_ShowMemKeys == value)
			{
				return;
			}
			_ShowMemKeys = value;
			if (_ShowMemKeys)
			{
				PanelEx panelEx = pnlCalc;
				((Control)panelEx).Height = ((Control)panelEx).Height + (((Control)pnlMem).Height - 1);
				((Control)pnlPad).Location = new Point(1, ((Control)pnlMem).Bottom);
			}
			else
			{
				PanelEx panelEx2 = pnlCalc;
				((Control)panelEx2).Height = ((Control)panelEx2).Height - (((Control)pnlMem).Height - 1);
				if (_DisplayVisible)
				{
					((Control)pnlPad).Location = new Point(1, ((Control)labelValue).Bottom);
				}
				else
				{
					((Control)pnlPad).Location = new Point(1, ((Control)labelValue).Top);
				}
			}
			InvalidateAutoSize();
		}
	}

	[DefaultValue(DecimalKeyVisibility.Auto)]
	[Category("Appearance")]
	[Description("Indicates visibility of the decimal calculator key.")]
	public DecimalKeyVisibility DecimalKeyVisibility
	{
		get
		{
			return _DecimalKeyVisibility;
		}
		set
		{
			if (_DecimalKeyVisibility != value)
			{
				_DecimalKeyVisibility = value;
				UpdateLayout();
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string StringValue
	{
		get
		{
			return _StringValue;
		}
		set
		{
			_StringValue = value;
			OnValueChanged();
		}
	}

	[DefaultValue(0.0)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public double Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
			OnValueChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override bool AutoSize
	{
		get
		{
			return ((Control)this).AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				((Control)this).AutoSize = value;
				AdjustSize();
			}
		}
	}

	internal bool FocusButtonsOnMouseDown
	{
		get
		{
			return _FocusButtonsOnMouseDown;
		}
		set
		{
			if (value != _FocusButtonsOnMouseDown)
			{
				bool focusButtonsOnMouseDown = _FocusButtonsOnMouseDown;
				_FocusButtonsOnMouseDown = value;
				OnFocusButtonsOnMouseDownChanged(focusButtonsOnMouseDown, value);
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether calculator display is visible.")]
	public bool DisplayVisible
	{
		get
		{
			return _DisplayVisible;
		}
		set
		{
			if (value != _DisplayVisible)
			{
				bool displayVisible = _DisplayVisible;
				_DisplayVisible = value;
				OnDisplayVisibleChanged(displayVisible, value);
			}
		}
	}

	[Description("Occurs when a calc button has been clicked.")]
	public event EventHandler<ButtonClickEventArgs> ButtonClick;

	[Description("Occurs when the calculator value has changed.")]
	public event EventHandler<ValueChangedEventArgs> ValueChanged;

	public event EventHandler CalculatorDisplayChanged;

	protected virtual void OnCalculatorDisplayChanged(EventArgs e)
	{
		this.CalculatorDisplayChanged?.Invoke(this, e);
	}

	public Calculator()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_ShowMemKeys = true;
		_DecimalKeyVisibility = DecimalKeyVisibility.Auto;
		((Control)this).SetStyle((ControlStyles)(DisplayHelp.DoubleBufferFlag | 0x2000 | 4), true);
		((Control)labelValue).BackColor = Color.White;
		((Control)pnlCalc).BackColor = Color.White;
		((Control)pnlPad).BackColor = Color.White;
		_NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		((Control)BtnDecimal).Text = _NumberDecimalSeparator;
	}

	private void UpdateDisplay()
	{
		((Control)labelValue).Text = DisplaySValue;
	}

	internal void ProcessKeyPress(KeyPressEventArgs e)
	{
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Invalid comparison between Unknown and I4
		ButtonX buttonX = null;
		switch (e.KeyChar)
		{
		case '0':
			buttonX = OnDigitSelect(BtnDigit0);
			break;
		case '1':
			buttonX = OnDigitSelect(BtnDigit1);
			break;
		case '2':
			buttonX = OnDigitSelect(BtnDigit2);
			break;
		case '3':
			buttonX = OnDigitSelect(BtnDigit3);
			break;
		case '4':
			buttonX = OnDigitSelect(BtnDigit4);
			break;
		case '5':
			buttonX = OnDigitSelect(BtnDigit5);
			break;
		case '6':
			buttonX = OnDigitSelect(BtnDigit6);
			break;
		case '7':
			buttonX = OnDigitSelect(BtnDigit7);
			break;
		case '8':
			buttonX = OnDigitSelect(BtnDigit8);
			break;
		case '9':
			buttonX = OnDigitSelect(BtnDigit9);
			break;
		case '+':
			buttonX = OnOperatorSelect(BtnAdd, Operators.Add);
			break;
		case '-':
			buttonX = OnOperatorSelect(BtnSubtract, Operators.Subtract);
			break;
		case '*':
			buttonX = OnOperatorSelect(BtnMultiply, Operators.Multiply);
			break;
		case '/':
			buttonX = OnOperatorSelect(BtnDivide, Operators.Divide);
			break;
		case '%':
			buttonX = OnPercentSelect();
			break;
		case 'r':
			buttonX = OnReciprocalSelect();
			break;
		case '@':
			buttonX = OnSqrtSelect();
			break;
		case 'n':
			buttonX = OnNegateSelect();
			break;
		case 'c':
			buttonX = OnClearSelect();
			break;
		case 'e':
			buttonX = OnClearEntrySelect();
			break;
		case '=':
			buttonX = OnEqualsSelect();
			break;
		case '\b':
			buttonX = OnBackSelect();
			break;
		case '\r':
			buttonX = (((Control.ModifierKeys & 0x20000) == 131072) ? OnMemStoreSelect() : OnEqualsSelect());
			break;
		case '\u0012':
			buttonX = OnMemRestoreSelect();
			break;
		case '\u0010':
			buttonX = OnMemAddSelect();
			break;
		case '\u0011':
			buttonX = OnMemSubtractSelect();
			break;
		case '\f':
			buttonX = OnMemClearSelect();
			break;
		}
		if (buttonX == null && e.KeyChar.ToString() == _NumberDecimalSeparator)
		{
			buttonX = OnDecimalSelect();
		}
		if (buttonX != null)
		{
			FlashKey(buttonX);
			e.Handled = true;
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
		if (!e.Handled)
		{
			ProcessKeyPress(e);
		}
	}

	private void BtnDigitClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnDigitSelect((ButtonX)sender);
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnDigitSelect(ButtonX btn)
	{
		AddDigit(btn);
		OnValueChanged();
		return btn;
	}

	private void AddDigit(object sender)
	{
		if (!(sender is ButtonX buttonX))
		{
			return;
		}
		char c = ((Control)buttonX).Name[8];
		if (_StringValue == null || _LastOp == Operators.Set)
		{
			_StringValue = "";
			if (_LastOp == Operators.Calc || _LastOp == Operators.Set)
			{
				SetOperator(Operators.None);
			}
		}
		_StringValue += c;
		UpdateDisplay();
		OnCalculatorDisplayChanged(EventArgs.Empty);
	}

	private void BtnDecimalClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(BtnDecimal))
		{
			OnDecimalSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnDecimalSelect()
	{
		if (!_IsIntValue)
		{
			if (_StringValue == null || _LastOp == Operators.Set)
			{
				_StringValue = "0" + _NumberDecimalSeparator;
				if (_LastOp == Operators.Calc || _LastOp == Operators.Set)
				{
					SetOperator(Operators.None);
				}
			}
			else if (!_StringValue.Contains(_NumberDecimalSeparator))
			{
				_StringValue += _NumberDecimalSeparator;
			}
		}
		OnValueChanged();
		return BtnDecimal;
	}

	private void BtnBackClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(BtnBack))
		{
			OnBackSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnBackSelect()
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			_StringValue = _StringValue.Substring(0, _StringValue.Length - 1);
			if (_StringValue.Length == 0 || (_StringValue.Length == 1 && _StringValue[0] == '-'))
			{
				_StringValue = "0";
			}
		}
		else
		{
			SystemSounds.Beep.Play();
		}
		OnValueChanged();
		return BtnBack;
	}

	private void BtnAddClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnOperatorSelect(BtnAdd, Operators.Add);
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private void BtnSubtractClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnOperatorSelect(BtnSubtract, Operators.Subtract);
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private void BtnMultiplyClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnOperatorSelect(BtnMultiply, Operators.Multiply);
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private void BtnDivideClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnOperatorSelect(BtnDivide, Operators.Divide);
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private void BtnNegateClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnNegateSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnNegateSelect()
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			_StringValue = "-" + _StringValue;
			_StringValue = _StringValue.Replace("--", "");
		}
		else
		{
			_Value = 0.0 - _Value;
		}
		OnValueChanged();
		return BtnNegate;
	}

	private void BtnSqrtClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnSqrtSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnSqrtSelect()
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			double num = double.Parse(_StringValue);
			if (num > 0.0)
			{
				num = Math.Sqrt(num);
				if (IsIntValue)
				{
					num = Math.Round(num);
				}
				_StringValue = num.ToString();
			}
			else
			{
				SystemSounds.Beep.Play();
			}
		}
		else if (_Value > 0.0)
		{
			_Value = Math.Sqrt(_Value);
		}
		else
		{
			SystemSounds.Beep.Play();
		}
		_LastOp = Operators.Set;
		OnValueChanged();
		return BtnSqrt;
	}

	private void BtnReciprocalClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(BtnReciprocal))
		{
			OnReciprocalSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnReciprocalSelect()
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			double num = double.Parse(_StringValue);
			if (num != 0.0)
			{
				num = 1.0 / num;
				if (IsIntValue)
				{
					num = Math.Round(num);
				}
				_StringValue = num.ToString();
			}
			else
			{
				SystemSounds.Beep.Play();
			}
		}
		else if (_Value != 0.0)
		{
			_Value = 1.0 / _Value;
		}
		else
		{
			SystemSounds.Beep.Play();
		}
		_LastOp = Operators.Set;
		OnValueChanged();
		return BtnReciprocal;
	}

	private void BtnPercentClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(BtnPercent))
		{
			OnPercentSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnPercentSelect()
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			double num = double.Parse(_StringValue);
			num = _Value * num / 100.0;
			if (IsIntValue)
			{
				num = Math.Round(num);
			}
			_StringValue = num.ToString();
		}
		else
		{
			_Value = 0.0;
		}
		OnValueChanged();
		return BtnPercent;
	}

	private void BtnEqualsClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnEqualsSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnEqualsSelect()
	{
		UpdateTotal();
		_LastOp = Operators.Calc;
		((Control)lbxOperator).Text = "";
		OnValueChanged();
		return BtnEquals;
	}

	private void BtnClearEntryClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnClearEntrySelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnClearEntrySelect()
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			_StringValue = "0";
		}
		else
		{
			_Value = 0.0;
		}
		OnValueChanged();
		return BtnClearEntry;
	}

	private void BtnClearClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnClearSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnClearSelect()
	{
		_Value = 0.0;
		_LValue = 0.0;
		_StringValue = null;
		SetOperator(Operators.None);
		OnValueChanged();
		return BtnClear;
	}

	private void BtnMemStoreClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnMemStoreSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnMemStoreSelect()
	{
		StoreToMem(DisplayValue);
		return BtnMemStore;
	}

	private void BtnMemRestoreClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnMemRestoreSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnMemRestoreSelect()
	{
		_StringValue = _MemValue.ToString();
		OnValueChanged();
		return BtnMemRestore;
	}

	private void BtnMemClearClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnMemClearSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnMemClearSelect()
	{
		StoreToMem(0.0);
		return BtnMemClear;
	}

	private void BtnMemAddClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnMemAddSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnMemAddSelect()
	{
		StoreToMem(_MemValue + DisplayValue);
		return BtnMemAdd;
	}

	private void BtnMemSubtractClick(object sender, EventArgs e)
	{
		if (!OnButtonClick(sender))
		{
			OnMemSubtractSelect();
		}
		if (_FocusButtonsOnMouseDown)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX OnMemSubtractSelect()
	{
		StoreToMem(_MemValue - DisplayValue);
		return BtnMemSubtract;
	}

	private void StoreToMem(double value)
	{
		_MemValue = value;
		((Control)lbxMemory).Text = ((value == 0.0) ? "" : "M");
	}

	private void SetOperator(Operators op)
	{
		_Operator = op;
		_LastOp = op;
		((Control)lbxOperator).Text = ((char)op).ToString();
	}

	private ButtonX OnOperatorSelect(ButtonX btn, Operators op)
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			UpdateTotal();
		}
		SetOperator(op);
		OnValueChanged();
		return btn;
	}

	private void UpdateTotal()
	{
		if (!string.IsNullOrEmpty(_StringValue))
		{
			_LValue = double.Parse(_StringValue);
		}
		switch (_Operator)
		{
		case Operators.None:
			_Value = _LValue;
			break;
		case Operators.Add:
			_Value += _LValue;
			break;
		case Operators.Subtract:
			_Value -= _LValue;
			break;
		case Operators.Multiply:
			_Value *= _LValue;
			break;
		case Operators.Divide:
			if (_LValue > 0.0)
			{
				_Value /= _LValue;
			}
			else
			{
				SystemSounds.Beep.Play();
			}
			break;
		}
		_StringValue = null;
		OnValueChanged();
	}

	private void UpdateLayout()
	{
		if (_DecimalKeyVisibility switch
		{
			DecimalKeyVisibility.Always => true, 
			DecimalKeyVisibility.Auto => !IsIntValue, 
			_ => false, 
		})
		{
			((Control)BtnDecimal).Visible = true;
			((Control)BtnDigit0).Width = ((Control)BtnDigit2).Bounds.Right - ((Control)BtnDigit0).Bounds.X;
		}
		else
		{
			((Control)BtnDecimal).Visible = false;
			((Control)BtnDigit0).Width = ((Control)BtnDecimal).Bounds.Right - ((Control)BtnDigit0).Bounds.X;
		}
		InvalidateAutoSize();
	}

	private bool OnButtonClick(object button)
	{
		if (this.ButtonClick != null)
		{
			ButtonClickEventArgs buttonClickEventArgs = new ButtonClickEventArgs(button);
			this.ButtonClick(this, buttonClickEventArgs);
			return buttonClickEventArgs.Cancel;
		}
		return false;
	}

	public void OnValueChanged()
	{
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, new ValueChangedEventArgs(DisplaySValue, DisplayValue));
		}
		UpdateDisplay();
		OnCalculatorDisplayChanged(EventArgs.Empty);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).Visible)
		{
			_StringValue = null;
			_LValue = 0.0;
			StoreToMem(0.0);
			SetOperator(Operators.Set);
			OnValueChanged();
		}
		((Control)this).OnVisibleChanged(e);
	}

	public void FlashKey(ButtonX keyButton)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (_KeyTimer == null)
		{
			_KeyTimer = new Timer();
			_KeyTimer.Interval = 125;
			_KeyTimer.Tick += KeyTimerTick;
		}
		if (_KeyTimer.Tag is ButtonX btn)
		{
			_KeyTimer.Stop();
			DoKeyTimerUp(btn);
		}
		DoKeyTimerDown(keyButton);
		_KeyTimer.Tag = keyButton;
		_KeyTimer.Start();
	}

	private void DoKeyTimerDown(ButtonX btn)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		MouseEventArgs objArg = new MouseEventArgs((MouseButtons)1048576, 1, ((Control)btn).Location.X, ((Control)btn).Location.Y, 0);
		btn.InternalItem.InternalMouseDown(objArg);
	}

	private void DoKeyTimerUp(ButtonX btn)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		MouseEventArgs objArg = new MouseEventArgs((MouseButtons)1048576, 1, ((Control)btn).Location.X, ((Control)btn).Location.Y, 0);
		btn.InternalItem.InternalMouseUp(objArg);
	}

	private void KeyTimerTick(object sender, EventArgs e)
	{
		if (_KeyTimer.Tag is ButtonX btn)
		{
			DoKeyTimerUp(btn);
		}
		_KeyTimer.Stop();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((Control)this).OnPaint(e);
	}

	public void InvalidateAutoSize()
	{
		_PreferredSize = Size.Empty;
		AdjustSize();
	}

	private void AdjustSize()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected virtual void OnFocusButtonsOnMouseDownChanged(bool oldValue, bool newValue)
	{
		BtnBack.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit8.FocusOnLeftMouseButtonDown = newValue;
		BtnReciprocal.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit5.FocusOnLeftMouseButtonDown = newValue;
		BtnSqrt.FocusOnLeftMouseButtonDown = newValue;
		BtnDivide.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit9.FocusOnLeftMouseButtonDown = newValue;
		BtnClearEntry.FocusOnLeftMouseButtonDown = newValue;
		BtnDecimal.FocusOnLeftMouseButtonDown = newValue;
		BtnAdd.FocusOnLeftMouseButtonDown = newValue;
		BtnEquals.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit4.FocusOnLeftMouseButtonDown = newValue;
		BtnSubtract.FocusOnLeftMouseButtonDown = newValue;
		BtnMultiply.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit6.FocusOnLeftMouseButtonDown = newValue;
		BtnClear.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit1.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit2.FocusOnLeftMouseButtonDown = newValue;
		BtnNegate.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit0.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit3.FocusOnLeftMouseButtonDown = newValue;
		BtnPercent.FocusOnLeftMouseButtonDown = newValue;
		BtnDigit7.FocusOnLeftMouseButtonDown = newValue;
		BtnMemStore.FocusOnLeftMouseButtonDown = newValue;
		BtnMemRestore.FocusOnLeftMouseButtonDown = newValue;
		BtnMemAdd.FocusOnLeftMouseButtonDown = newValue;
		BtnMemClear.FocusOnLeftMouseButtonDown = newValue;
		BtnMemSubtract.FocusOnLeftMouseButtonDown = newValue;
	}

	protected override void OnFontChanged(EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		InvalidateAutoSize();
		((Control)BtnMemAdd).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnMemAdd).Font.SizeInPoints);
		((Control)BtnMemClear).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnMemClear).Font.SizeInPoints);
		((Control)BtnMemRestore).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnMemRestore).Font.SizeInPoints);
		((Control)BtnMemStore).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnMemStore).Font.SizeInPoints);
		((Control)BtnMemSubtract).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnMemSubtract).Font.SizeInPoints);
		((Control)BtnDigit0).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit0).Font.SizeInPoints);
		((Control)BtnDigit1).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit1).Font.SizeInPoints);
		((Control)BtnDigit2).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit2).Font.SizeInPoints);
		((Control)BtnDigit3).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit3).Font.SizeInPoints);
		((Control)BtnDigit4).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit4).Font.SizeInPoints);
		((Control)BtnDigit5).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit5).Font.SizeInPoints);
		((Control)BtnDigit6).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit6).Font.SizeInPoints);
		((Control)BtnDigit7).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit7).Font.SizeInPoints);
		((Control)BtnDigit8).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit8).Font.SizeInPoints);
		((Control)BtnDigit9).Font = new Font(((Control)this).Font.FontFamily, ((Control)BtnDigit9).Font.SizeInPoints);
		((Control)labelValue).Font = new Font(((Control)this).Font.FontFamily, ((Control)labelValue).Font.SizeInPoints);
		((Control)this).OnFontChanged(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!_PreferredSize.IsEmpty)
		{
			return _PreferredSize;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		_PreferredSize = ((Control)pnlCalc).Size;
		return _PreferredSize;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			width = preferredSize.Width;
			height = preferredSize.Height;
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Control)this).AutoSize)
		{
			AdjustSize();
		}
		((Control)this).OnHandleCreated(e);
	}

	protected virtual void OnDisplayVisibleChanged(bool oldValue, bool newValue)
	{
		((Control)labelValue).Visible = newValue;
		if (newValue)
		{
			PanelEx panelEx = pnlPad;
			((Control)panelEx).Top = ((Control)panelEx).Top + ((Control)labelValue).Height;
			PanelEx panelEx2 = pnlMem;
			((Control)panelEx2).Top = ((Control)panelEx2).Top + ((Control)labelValue).Height;
			PanelEx panelEx3 = pnlCalc;
			((Control)panelEx3).Height = ((Control)panelEx3).Height + ((Control)labelValue).Height;
		}
		else
		{
			PanelEx panelEx4 = pnlPad;
			((Control)panelEx4).Top = ((Control)panelEx4).Top - ((Control)labelValue).Height;
			PanelEx panelEx5 = pnlMem;
			((Control)panelEx5).Top = ((Control)panelEx5).Top - ((Control)labelValue).Height;
			PanelEx panelEx6 = pnlCalc;
			((Control)panelEx6).Height = ((Control)panelEx6).Height - ((Control)labelValue).Height;
		}
		_PreferredSize = Size.Empty;
		AdjustSize();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Expected O, but got Unknown
		//IL_0977: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Expected O, but got Unknown
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Expected O, but got Unknown
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Expected O, but got Unknown
		//IL_0c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c19: Expected O, but got Unknown
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cca: Expected O, but got Unknown
		//IL_0d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Expected O, but got Unknown
		//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e25: Expected O, but got Unknown
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed3: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_10b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bd: Expected O, but got Unknown
		//IL_11f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fa: Expected O, but got Unknown
		//IL_129d: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a7: Expected O, but got Unknown
		//IL_13df: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e9: Expected O, but got Unknown
		//IL_15ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1609: Expected O, but got Unknown
		//IL_1697: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a1: Expected O, but got Unknown
		//IL_1720: Unknown result type (might be due to invalid IL or missing references)
		//IL_172a: Expected O, but got Unknown
		//IL_17c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ca: Expected O, but got Unknown
		//IL_1860: Unknown result type (might be due to invalid IL or missing references)
		//IL_186a: Expected O, but got Unknown
		//IL_1900: Unknown result type (might be due to invalid IL or missing references)
		//IL_190a: Expected O, but got Unknown
		//IL_199b: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a5: Expected O, but got Unknown
		//IL_1a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a85: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stimulsoft.Controls.Win.Editors.Calculator));
		this.pnlCalc = new Stimulsoft.Controls.Win.DotNetBar.PanelEx();
		this.pnlPad = new Stimulsoft.Controls.Win.DotNetBar.PanelEx();
		this.BtnBack = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit8 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnReciprocal = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit5 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnSqrt = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDivide = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit9 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnClearEntry = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDecimal = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnAdd = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnEquals = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit4 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnSubtract = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnMultiply = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit6 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnClear = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit1 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit2 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnNegate = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit0 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit3 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnPercent = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnDigit7 = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.pnlMem = new Stimulsoft.Controls.Win.DotNetBar.PanelEx();
		this.BtnMemStore = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.lbxOperator = new Stimulsoft.Controls.Win.DotNetBar.LabelX();
		this.BtnMemRestore = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnMemAdd = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnMemClear = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.BtnMemSubtract = new Stimulsoft.Controls.Win.DotNetBar.ButtonX();
		this.lbxMemory = new Stimulsoft.Controls.Win.DotNetBar.LabelX();
		this.labelValue = new Stimulsoft.Controls.Win.DotNetBar.LabelX();
		((Control)this.pnlCalc).SuspendLayout();
		((Control)this.pnlPad).SuspendLayout();
		((Control)this.pnlMem).SuspendLayout();
		((Control)this).SuspendLayout();
		this.pnlCalc.CanvasColor = System.Drawing.SystemColors.Control;
		this.pnlCalc.ColorSchemeStyle = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.pnlCalc).Controls.Add((Control)(object)this.pnlPad);
		((Control)this.pnlCalc).Controls.Add((Control)(object)this.pnlMem);
		((Control)this.pnlCalc).Controls.Add((Control)(object)this.labelValue);
		((Control)this.pnlCalc).Location = new System.Drawing.Point(0, 0);
		((Control)this.pnlCalc).Name = "pnlCalc";
		((Control)this.pnlCalc).Padding = new Padding(1);
		((Control)this.pnlCalc).Size = new System.Drawing.Size(190, 211);
		this.pnlCalc.Style.Alignment = (StringAlignment)1;
		this.pnlCalc.Style.BackColor1.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.BarBackground;
		this.pnlCalc.Style.Border = Stimulsoft.Controls.Win.DotNetBar.eBorderType.SingleLine;
		this.pnlCalc.Style.BorderColor.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.BarDockedBorder;
		this.pnlCalc.Style.ForeColor.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.ItemText;
		this.pnlCalc.Style.GradientAngle = 90;
		((Control)this.pnlCalc).TabIndex = 0;
		this.pnlPad.CanvasColor = System.Drawing.SystemColors.Control;
		this.pnlPad.ColorSchemeStyle = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnBack);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit8);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnReciprocal);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit5);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnSqrt);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDivide);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit9);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnClearEntry);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDecimal);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnAdd);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnEquals);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit4);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnSubtract);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnMultiply);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit6);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnClear);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit1);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit2);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnNegate);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit0);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit3);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnPercent);
		((Control)this.pnlPad).Controls.Add((Control)(object)this.BtnDigit7);
		((Control)this.pnlPad).Location = new System.Drawing.Point(1, 58);
		((Control)this.pnlPad).Name = "pnlPad";
		((Control)this.pnlPad).Size = new System.Drawing.Size(187, 151);
		this.pnlPad.Style.Alignment = (StringAlignment)1;
		this.pnlPad.Style.BackColor1.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.BarBackground;
		this.pnlPad.Style.BorderColor.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.BarDockedBorder;
		this.pnlPad.Style.ForeColor.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.ItemText;
		this.pnlPad.Style.GradientAngle = 90;
		((Control)this.pnlPad).TabIndex = 35;
		((Control)this.BtnBack).AccessibleRole = (AccessibleRole)43;
		this.BtnBack.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		this.BtnBack.Image = (Image)resources.GetObject("BtnBack.Image");
		((Control)this.BtnBack).Location = new System.Drawing.Point(8, 6);
		((Control)this.BtnBack).Name = "BtnBack";
		((Control)this.BtnBack).Size = new System.Drawing.Size(30, 23);
		this.BtnBack.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnBack).TabIndex = 0;
		((Control)this.BtnBack).Click += new System.EventHandler(BtnBackClick);
		((Control)this.BtnDigit8).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit8.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit8).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit8).Location = new System.Drawing.Point(43, 36);
		((Control)this.BtnDigit8).Name = "BtnDigit8";
		((Control)this.BtnDigit8).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit8.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit8).TabIndex = 6;
		((Control)this.BtnDigit8).Text = "8";
		((Control)this.BtnDigit8).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnReciprocal).AccessibleRole = (AccessibleRole)43;
		this.BtnReciprocal.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnReciprocal).Location = new System.Drawing.Point(150, 65);
		((Control)this.BtnReciprocal).Name = "BtnReciprocal";
		((Control)this.BtnReciprocal).Size = new System.Drawing.Size(30, 23);
		this.BtnReciprocal.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnReciprocal).TabIndex = 14;
		((Control)this.BtnReciprocal).Text = "1 / x";
		((Control)this.BtnReciprocal).Click += new System.EventHandler(BtnReciprocalClick);
		((Control)this.BtnDigit5).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit5.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit5).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit5).Location = new System.Drawing.Point(43, 65);
		((Control)this.BtnDigit5).Name = "BtnDigit5";
		((Control)this.BtnDigit5).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit5.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit5).TabIndex = 11;
		((Control)this.BtnDigit5).Text = "5";
		((Control)this.BtnDigit5).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnSqrt).AccessibleRole = (AccessibleRole)43;
		this.BtnSqrt.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnSqrt).Location = new System.Drawing.Point(150, 6);
		((Control)this.BtnSqrt).Name = "BtnSqrt";
		((Control)this.BtnSqrt).Size = new System.Drawing.Size(30, 23);
		this.BtnSqrt.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnSqrt).TabIndex = 4;
		((Control)this.BtnSqrt).Text = "√";
		((Control)this.BtnSqrt).Click += new System.EventHandler(BtnSqrtClick);
		((Control)this.BtnDivide).AccessibleRole = (AccessibleRole)43;
		this.BtnDivide.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDivide).Location = new System.Drawing.Point(114, 36);
		((Control)this.BtnDivide).Name = "BtnDivide";
		((Control)this.BtnDivide).Size = new System.Drawing.Size(30, 23);
		this.BtnDivide.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDivide).TabIndex = 8;
		((Control)this.BtnDivide).Text = "/";
		((Control)this.BtnDivide).Click += new System.EventHandler(BtnDivideClick);
		((Control)this.BtnDigit9).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit9.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit9).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit9).Location = new System.Drawing.Point(78, 36);
		((Control)this.BtnDigit9).Name = "BtnDigit9";
		((Control)this.BtnDigit9).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit9.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit9).TabIndex = 7;
		((Control)this.BtnDigit9).Text = "9";
		((Control)this.BtnDigit9).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnClearEntry).AccessibleRole = (AccessibleRole)43;
		this.BtnClearEntry.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnClearEntry).Location = new System.Drawing.Point(43, 6);
		((Control)this.BtnClearEntry).Name = "BtnClearEntry";
		((Control)this.BtnClearEntry).Size = new System.Drawing.Size(30, 23);
		this.BtnClearEntry.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnClearEntry).TabIndex = 1;
		((Control)this.BtnClearEntry).Text = "CE";
		((Control)this.BtnClearEntry).Click += new System.EventHandler(BtnClearEntryClick);
		((Control)this.BtnDecimal).AccessibleRole = (AccessibleRole)43;
		this.BtnDecimal.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDecimal).Font = new Font("Modern No. 20", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDecimal).Location = new System.Drawing.Point(78, 123);
		((Control)this.BtnDecimal).Name = "BtnDecimal";
		((Control)this.BtnDecimal).Size = new System.Drawing.Size(30, 23);
		this.BtnDecimal.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDecimal).TabIndex = 21;
		((Control)this.BtnDecimal).Text = ".";
		((Control)this.BtnDecimal).Click += new System.EventHandler(BtnDecimalClick);
		((Control)this.BtnAdd).AccessibleRole = (AccessibleRole)43;
		this.BtnAdd.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnAdd).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnAdd).Location = new System.Drawing.Point(114, 123);
		((Control)this.BtnAdd).Name = "BtnAdd";
		((Control)this.BtnAdd).Size = new System.Drawing.Size(30, 23);
		this.BtnAdd.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnAdd).TabIndex = 22;
		((Control)this.BtnAdd).Text = "+";
		((Control)this.BtnAdd).Click += new System.EventHandler(BtnAddClick);
		((Control)this.BtnEquals).AccessibleRole = (AccessibleRole)43;
		this.BtnEquals.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnEquals).Font = new Font("Microsoft Sans Serif", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnEquals).Location = new System.Drawing.Point(150, 94);
		((Control)this.BtnEquals).Name = "BtnEquals";
		((Control)this.BtnEquals).Size = new System.Drawing.Size(30, 52);
		this.BtnEquals.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnEquals).TabIndex = 19;
		((Control)this.BtnEquals).Text = "=";
		((Control)this.BtnEquals).Click += new System.EventHandler(BtnEqualsClick);
		((Control)this.BtnDigit4).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit4.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit4).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit4).Location = new System.Drawing.Point(8, 65);
		((Control)this.BtnDigit4).Name = "BtnDigit4";
		((Control)this.BtnDigit4).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit4.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit4).TabIndex = 10;
		((Control)this.BtnDigit4).Text = "4";
		((Control)this.BtnDigit4).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnSubtract).AccessibleRole = (AccessibleRole)43;
		this.BtnSubtract.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnSubtract).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnSubtract).Location = new System.Drawing.Point(114, 94);
		((Control)this.BtnSubtract).Name = "BtnSubtract";
		((Control)this.BtnSubtract).Size = new System.Drawing.Size(30, 23);
		this.BtnSubtract.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnSubtract).TabIndex = 18;
		((Control)this.BtnSubtract).Text = "-";
		((Control)this.BtnSubtract).Click += new System.EventHandler(BtnSubtractClick);
		((Control)this.BtnMultiply).AccessibleRole = (AccessibleRole)43;
		this.BtnMultiply.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnMultiply).Font = new Font("Symbol", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)2);
		((Control)this.BtnMultiply).Location = new System.Drawing.Point(114, 65);
		((Control)this.BtnMultiply).Name = "BtnMultiply";
		((Control)this.BtnMultiply).Size = new System.Drawing.Size(30, 23);
		this.BtnMultiply.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnMultiply).TabIndex = 13;
		((Control)this.BtnMultiply).Text = "*";
		((Control)this.BtnMultiply).Click += new System.EventHandler(BtnMultiplyClick);
		((Control)this.BtnDigit6).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit6.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit6).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit6).Location = new System.Drawing.Point(78, 65);
		((Control)this.BtnDigit6).Name = "BtnDigit6";
		((Control)this.BtnDigit6).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit6.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit6).TabIndex = 12;
		((Control)this.BtnDigit6).Text = "6";
		((Control)this.BtnDigit6).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnClear).AccessibleRole = (AccessibleRole)43;
		this.BtnClear.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnClear).Location = new System.Drawing.Point(79, 6);
		((Control)this.BtnClear).Name = "BtnClear";
		((Control)this.BtnClear).Size = new System.Drawing.Size(30, 23);
		this.BtnClear.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnClear).TabIndex = 2;
		((Control)this.BtnClear).Text = "C";
		((Control)this.BtnClear).Click += new System.EventHandler(BtnClearClick);
		((Control)this.BtnDigit1).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit1.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit1).Location = new System.Drawing.Point(8, 94);
		((Control)this.BtnDigit1).Name = "BtnDigit1";
		((Control)this.BtnDigit1).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit1.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit1).TabIndex = 15;
		((Control)this.BtnDigit1).Text = "1";
		((Control)this.BtnDigit1).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnDigit2).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit2.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit2).Location = new System.Drawing.Point(43, 94);
		((Control)this.BtnDigit2).Name = "BtnDigit2";
		((Control)this.BtnDigit2).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit2.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit2).TabIndex = 16;
		((Control)this.BtnDigit2).Text = "2";
		((Control)this.BtnDigit2).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnNegate).AccessibleRole = (AccessibleRole)43;
		this.BtnNegate.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnNegate).Location = new System.Drawing.Point(114, 6);
		((Control)this.BtnNegate).Name = "BtnNegate";
		((Control)this.BtnNegate).Size = new System.Drawing.Size(30, 23);
		this.BtnNegate.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnNegate).TabIndex = 3;
		((Control)this.BtnNegate).Text = "±";
		((Control)this.BtnNegate).Click += new System.EventHandler(BtnNegateClick);
		((Control)this.BtnDigit0).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit0.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit0).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit0).Location = new System.Drawing.Point(8, 123);
		((Control)this.BtnDigit0).Name = "BtnDigit0";
		((Control)this.BtnDigit0).Size = new System.Drawing.Size(65, 23);
		this.BtnDigit0.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit0).TabIndex = 20;
		((Control)this.BtnDigit0).Text = "0";
		((Control)this.BtnDigit0).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnDigit3).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit3.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit3).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit3).Location = new System.Drawing.Point(78, 94);
		((Control)this.BtnDigit3).Name = "BtnDigit3";
		((Control)this.BtnDigit3).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit3.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit3).TabIndex = 17;
		((Control)this.BtnDigit3).Text = "3";
		((Control)this.BtnDigit3).Click += new System.EventHandler(BtnDigitClick);
		((Control)this.BtnPercent).AccessibleRole = (AccessibleRole)43;
		this.BtnPercent.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnPercent).Location = new System.Drawing.Point(150, 36);
		((Control)this.BtnPercent).Name = "BtnPercent";
		((Control)this.BtnPercent).Size = new System.Drawing.Size(30, 23);
		this.BtnPercent.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnPercent).TabIndex = 9;
		((Control)this.BtnPercent).Text = "%";
		((Control)this.BtnPercent).Click += new System.EventHandler(BtnPercentClick);
		((Control)this.BtnDigit7).AccessibleRole = (AccessibleRole)43;
		this.BtnDigit7.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnDigit7).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnDigit7).Location = new System.Drawing.Point(8, 36);
		((Control)this.BtnDigit7).Name = "BtnDigit7";
		((Control)this.BtnDigit7).Size = new System.Drawing.Size(30, 23);
		this.BtnDigit7.Style = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.BtnDigit7).TabIndex = 5;
		((Control)this.BtnDigit7).Text = "7";
		((Control)this.BtnDigit7).Click += new System.EventHandler(BtnDigitClick);
		this.pnlMem.CanvasColor = System.Drawing.SystemColors.Control;
		this.pnlMem.ColorSchemeStyle = Stimulsoft.Controls.Win.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
		((Control)this.pnlMem).Controls.Add((Control)(object)this.BtnMemStore);
		((Control)this.pnlMem).Controls.Add((Control)(object)this.lbxOperator);
		((Control)this.pnlMem).Controls.Add((Control)(object)this.BtnMemRestore);
		((Control)this.pnlMem).Controls.Add((Control)(object)this.BtnMemAdd);
		((Control)this.pnlMem).Controls.Add((Control)(object)this.BtnMemClear);
		((Control)this.pnlMem).Controls.Add((Control)(object)this.BtnMemSubtract);
		((Control)this.pnlMem).Controls.Add((Control)(object)this.lbxMemory);
		((Control)this.pnlMem).Location = new System.Drawing.Point(1, 33);
		((Control)this.pnlMem).Name = "pnlMem";
		((Control)this.pnlMem).Size = new System.Drawing.Size(187, 26);
		this.pnlMem.Style.Alignment = (StringAlignment)1;
		this.pnlMem.Style.BackColor1.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.BarBackground;
		this.pnlMem.Style.BorderColor.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.BarDockedBorder;
		this.pnlMem.Style.BorderWidth = 0;
		this.pnlMem.Style.ForeColor.ColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.ItemText;
		this.pnlMem.Style.GradientAngle = 90;
		((Control)this.pnlMem).TabIndex = 34;
		((Control)this.BtnMemStore).AccessibleRole = (AccessibleRole)43;
		this.BtnMemStore.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnMemStore).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnMemStore).Location = new System.Drawing.Point(80, 6);
		((Control)this.BtnMemStore).Name = "BtnMemStore";
		((Control)this.BtnMemStore).Size = new System.Drawing.Size(27, 18);
		((Control)this.BtnMemStore).TabIndex = 0;
		((Control)this.BtnMemStore).Text = "MS";
		((Control)this.BtnMemStore).Click += new System.EventHandler(BtnMemStoreClick);
		this.lbxOperator.BackgroundStyle.CornerType = Stimulsoft.Controls.Win.DotNetBar.eCornerType.Square;
		((Control)this.lbxOperator).Font = new Font("Symbol", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)2);
		((Control)this.lbxOperator).Location = new System.Drawing.Point(174, 6);
		((Control)this.lbxOperator).Name = "lbxOperator";
		((Control)this.lbxOperator).Size = new System.Drawing.Size(12, 18);
		((Control)this.lbxOperator).TabIndex = 33;
		this.lbxOperator.TextAlignment = (StringAlignment)1;
		((Control)this.BtnMemRestore).AccessibleRole = (AccessibleRole)43;
		this.BtnMemRestore.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnMemRestore).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnMemRestore).Location = new System.Drawing.Point(48, 6);
		((Control)this.BtnMemRestore).Name = "BtnMemRestore";
		((Control)this.BtnMemRestore).Size = new System.Drawing.Size(27, 18);
		((Control)this.BtnMemRestore).TabIndex = 6;
		((Control)this.BtnMemRestore).Text = "MR";
		((Control)this.BtnMemRestore).Click += new System.EventHandler(BtnMemRestoreClick);
		((Control)this.BtnMemAdd).AccessibleRole = (AccessibleRole)43;
		this.BtnMemAdd.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnMemAdd).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnMemAdd).Location = new System.Drawing.Point(112, 6);
		((Control)this.BtnMemAdd).Name = "BtnMemAdd";
		((Control)this.BtnMemAdd).Size = new System.Drawing.Size(27, 18);
		((Control)this.BtnMemAdd).TabIndex = 1;
		((Control)this.BtnMemAdd).Text = "M+";
		((Control)this.BtnMemAdd).Click += new System.EventHandler(BtnMemAddClick);
		((Control)this.BtnMemClear).AccessibleRole = (AccessibleRole)43;
		this.BtnMemClear.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnMemClear).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnMemClear).Location = new System.Drawing.Point(16, 6);
		((Control)this.BtnMemClear).Name = "BtnMemClear";
		((Control)this.BtnMemClear).Size = new System.Drawing.Size(27, 18);
		((Control)this.BtnMemClear).TabIndex = 5;
		((Control)this.BtnMemClear).Text = "MC";
		((Control)this.BtnMemClear).Click += new System.EventHandler(BtnMemClearClick);
		((Control)this.BtnMemSubtract).AccessibleRole = (AccessibleRole)43;
		this.BtnMemSubtract.ColorTable = Stimulsoft.Controls.Win.DotNetBar.eButtonColor.OrangeWithBackground;
		((Control)this.BtnMemSubtract).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.BtnMemSubtract).Location = new System.Drawing.Point(144, 6);
		((Control)this.BtnMemSubtract).Name = "BtnMemSubtract";
		((Control)this.BtnMemSubtract).Size = new System.Drawing.Size(27, 18);
		((Control)this.BtnMemSubtract).TabIndex = 2;
		((Control)this.BtnMemSubtract).Text = "M-";
		((Control)this.BtnMemSubtract).Click += new System.EventHandler(BtnMemSubtractClick);
		this.lbxMemory.BackgroundStyle.CornerType = Stimulsoft.Controls.Win.DotNetBar.eCornerType.Square;
		((Control)this.lbxMemory).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.lbxMemory).Location = new System.Drawing.Point(4, 5);
		((Control)this.lbxMemory).Name = "lbxMemory";
		((Control)this.lbxMemory).Size = new System.Drawing.Size(12, 18);
		((Control)this.lbxMemory).TabIndex = 31;
		this.lbxMemory.TextAlignment = (StringAlignment)1;
		this.labelValue.BackgroundStyle.BackColor2SchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.MenuBackground;
		this.labelValue.BackgroundStyle.BorderBottom = Stimulsoft.Controls.Win.DotNetBar.eStyleBorderType.Solid;
		this.labelValue.BackgroundStyle.BorderBottomColorSchemePart = Stimulsoft.Controls.Win.DotNetBar.eColorSchemePart.PanelBorder;
		this.labelValue.BackgroundStyle.BorderBottomWidth = 1;
		this.labelValue.BackgroundStyle.CornerType = Stimulsoft.Controls.Win.DotNetBar.eCornerType.Square;
		this.labelValue.BackgroundStyle.PaddingRight = 6;
		((Control)this.labelValue).Dock = (DockStyle)1;
		((Control)this.labelValue).Font = new Font("Consolas", 18f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this.labelValue).Location = new System.Drawing.Point(1, 1);
		((Control)this.labelValue).Name = "labelValue";
		((Control)this.labelValue).Size = new System.Drawing.Size(188, 27);
		((Control)this.labelValue).TabIndex = 1;
		((Control)this.labelValue).Text = "0";
		this.labelValue.TextAlignment = (StringAlignment)2;
		((Control)this).Controls.Add((Control)(object)this.pnlCalc);
		((Control)this).Name = "Calculator";
		((Control)this).Size = new System.Drawing.Size(190, 212);
		((Control)this.pnlCalc).ResumeLayout(false);
		((Control)this.pnlPad).ResumeLayout(false);
		((Control)this.pnlMem).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
