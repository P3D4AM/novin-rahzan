using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

[Designer("DevComponents.DotNetBar.Design.CalculatorDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[DefaultProperty("Value")]
[ToolboxBitmap(typeof(DotNetBarManager), "Calculator.ico")]
[DefaultEvent("ValueChanged")]
public class Calculator : Control
{
	private EventHandler<ButtonClickEventArgs> _22E9;

	private EventHandler<ValueChangedEventArgs> ᗲ;

	private EventHandler _22EA;

	private string __22EB;

	private double _ᗸ;

	private double __22EC;

	private double __22ED;

	private Operators __22EE;

	private Operators __22EF;

	private bool __22F0;

	private bool __22F1;

	private DecimalKeyVisibility __22F2;

	private Timer __22F3;

	private string __22F4;

	private Size __21D2 = Size.Empty;

	private bool __22F5 = true;

	private bool __22F6 = true;

	private IContainer Պ;

	private PanelEx _22F7;

	private LabelX _22F8;

	private LabelX _22F9;

	private PanelEx _22FA;

	private PanelEx _22FB;

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

	private LabelX _22FC;

	[Browsable(false)]
	public double DisplayValue
	{
		get
		{
			if (string.IsNullOrEmpty(__22EB))
			{
				return _ᗸ;
			}
			return double.Parse(__22EB);
		}
	}

	[Browsable(false)]
	public string DisplaySValue
	{
		get
		{
			if (!string.IsNullOrEmpty(__22EB))
			{
				return __22EB;
			}
			return _ᗸ.ToString();
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether calculator displays only Integer values.")]
	public bool IsIntValue
	{
		get
		{
			return __22F0;
		}
		set
		{
			if (__22F0 != value)
			{
				__22F0 = value;
				ᘥ();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether memory keys are visible.")]
	[Category("Appearance")]
	public bool ShowMemKeys
	{
		get
		{
			return __22F1;
		}
		set
		{
			if (__22F1 == value)
			{
				return;
			}
			__22F1 = value;
			if (__22F1)
			{
				PanelEx panelEx = _22F7;
				((Control)panelEx).Height = ((Control)panelEx).Height + (((Control)_22FA).Height - 1);
				((Control)_22FB).Location = new Point(1, ((Control)_22FA).Bottom);
			}
			else
			{
				PanelEx panelEx2 = _22F7;
				((Control)panelEx2).Height = ((Control)panelEx2).Height - (((Control)_22FA).Height - 1);
				if (__22F6)
				{
					((Control)_22FB).Location = new Point(1, ((Control)_22FC).Bottom);
				}
				else
				{
					((Control)_22FB).Location = new Point(1, ((Control)_22FC).Top);
				}
			}
			InvalidateAutoSize();
		}
	}

	[Description("Indicates visibility of the decimal calculator key.")]
	[DefaultValue(DecimalKeyVisibility.Auto)]
	[Category("Appearance")]
	public DecimalKeyVisibility DecimalKeyVisibility
	{
		get
		{
			return __22F2;
		}
		set
		{
			if (__22F2 != value)
			{
				__22F2 = value;
				ᘥ();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string StringValue
	{
		get
		{
			return __22EB;
		}
		set
		{
			__22EB = value;
			OnValueChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(0.0)]
	public double Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			_ᗸ = value;
			OnValueChanged();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
	[DefaultValue(false)]
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
				_21DF();
			}
		}
	}

	internal bool _22E8
	{
		get
		{
			return __22F5;
		}
		set
		{
			if (value != __22F5)
			{
				bool oldValue = __22F5;
				__22F5 = value;
				OnFocusButtonsOnMouseDownChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether calculator display is visible.")]
	public bool DisplayVisible
	{
		get
		{
			return __22F6;
		}
		set
		{
			if (value != __22F6)
			{
				bool oldValue = __22F6;
				__22F6 = value;
				OnDisplayVisibleChanged(oldValue, value);
			}
		}
	}

	[Description("Occurs when a calc button has been clicked.")]
	public event EventHandler<ButtonClickEventArgs> ButtonClick
	{
		add
		{
			EventHandler<ButtonClickEventArgs> eventHandler = _22E9;
			EventHandler<ButtonClickEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ButtonClickEventArgs> value2 = (EventHandler<ButtonClickEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22E9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<ButtonClickEventArgs> eventHandler = _22E9;
			EventHandler<ButtonClickEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ButtonClickEventArgs> value2 = (EventHandler<ButtonClickEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22E9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the calculator value has changed.")]
	public event EventHandler<ValueChangedEventArgs> ValueChanged
	{
		add
		{
			EventHandler<ValueChangedEventArgs> eventHandler = ᗲ;
			EventHandler<ValueChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ValueChangedEventArgs> value2 = (EventHandler<ValueChangedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<ValueChangedEventArgs> eventHandler = ᗲ;
			EventHandler<ValueChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ValueChangedEventArgs> value2 = (EventHandler<ValueChangedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler CalculatorDisplayChanged
	{
		add
		{
			EventHandler eventHandler = _22EA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22EA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _22EA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22EA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnCalculatorDisplayChanged(EventArgs e)
	{
		_22EA?.Invoke(this, e);
	}

	public Calculator()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		ـ();
		__22F1 = true;
		__22F2 = DecimalKeyVisibility.Auto;
		((Control)this).SetStyle((ControlStyles)(DisplayHelp.DoubleBufferFlag | 0x2000 | 4), true);
		((Control)_22FC).BackColor = Color.White;
		((Control)_22F7).BackColor = Color.White;
		((Control)_22FB).BackColor = Color.White;
		__22F4 = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		((Control)BtnDecimal).Text = __22F4;
	}

	private void _22FD()
	{
		((Control)_22FC).Text = DisplaySValue;
	}

	internal void ᡡ(KeyPressEventArgs ؿ)
	{
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Invalid comparison between Unknown and I4
		ButtonX buttonX = null;
		switch (ؿ.KeyChar)
		{
		case '0':
			buttonX = _22FF(BtnDigit0);
			break;
		case '1':
			buttonX = _22FF(BtnDigit1);
			break;
		case '2':
			buttonX = _22FF(BtnDigit2);
			break;
		case '3':
			buttonX = _22FF(BtnDigit3);
			break;
		case '4':
			buttonX = _22FF(BtnDigit4);
			break;
		case '5':
			buttonX = _22FF(BtnDigit5);
			break;
		case '6':
			buttonX = _22FF(BtnDigit6);
			break;
		case '7':
			buttonX = _22FF(BtnDigit7);
			break;
		case '8':
			buttonX = _22FF(BtnDigit8);
			break;
		case '9':
			buttonX = _22FF(BtnDigit9);
			break;
		case '+':
			buttonX = _2324(BtnAdd, Operators.Add);
			break;
		case '-':
			buttonX = _2324(BtnSubtract, Operators.Subtract);
			break;
		case '*':
			buttonX = _2324(BtnMultiply, Operators.Multiply);
			break;
		case '/':
			buttonX = _2324(BtnDivide, Operators.Divide);
			break;
		case '%':
			buttonX = _2310();
			break;
		case 'r':
			buttonX = _230E();
			break;
		case '@':
			buttonX = _230C();
			break;
		case 'n':
			buttonX = _230A();
			break;
		case 'c':
			buttonX = _2316();
			break;
		case 'e':
			buttonX = _2314();
			break;
		case '=':
			buttonX = _2312();
			break;
		case '\b':
			buttonX = _2304();
			break;
		case '\r':
			buttonX = (((Control.ModifierKeys & 0x20000) == 131072) ? _2318() : _2312());
			break;
		case '\u0012':
			buttonX = _231A();
			break;
		case '\u0010':
			buttonX = _231E();
			break;
		case '\u0011':
			buttonX = _2320();
			break;
		case '\f':
			buttonX = _231C();
			break;
		}
		if (buttonX == null && ؿ.KeyChar.ToString() == __22F4)
		{
			buttonX = _2302();
		}
		if (buttonX != null)
		{
			FlashKey(buttonX);
			ؿ.Handled = true;
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
		if (!e.Handled)
		{
			ᡡ(e);
		}
	}

	private void _22FE(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_22FF((ButtonX)_٠);
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _22FF(ButtonX ख़)
	{
		_2300(ख़);
		OnValueChanged();
		return ख़;
	}

	private void _2300(object _٠)
	{
		if (!(_٠ is ButtonX buttonX))
		{
			return;
		}
		char c = ((Control)buttonX).Name[8];
		if (__22EB == null || __22EF == Operators.Set)
		{
			__22EB = "";
			if (__22EF == Operators.Calc || __22EF == Operators.Set)
			{
				_2322(Operators.None);
			}
		}
		__22EB += c;
		_22FD();
		OnCalculatorDisplayChanged(EventArgs.Empty);
	}

	private void _2301(object _٠, EventArgs ؿ)
	{
		if (!_2326(BtnDecimal))
		{
			_2302();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2302()
	{
		if (!__22F0)
		{
			if (__22EB == null || __22EF == Operators.Set)
			{
				__22EB = "0" + __22F4;
				if (__22EF == Operators.Calc || __22EF == Operators.Set)
				{
					_2322(Operators.None);
				}
			}
			else if (!__22EB.Contains(__22F4))
			{
				__22EB += __22F4;
			}
		}
		OnValueChanged();
		return BtnDecimal;
	}

	private void _2303(object _٠, EventArgs ؿ)
	{
		if (!_2326(BtnBack))
		{
			_2304();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2304()
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			__22EB = __22EB.Substring(0, __22EB.Length - 1);
			if (__22EB.Length == 0 || (__22EB.Length == 1 && __22EB[0] == '-'))
			{
				__22EB = "0";
			}
		}
		else
		{
			SystemSounds.Beep.Play();
		}
		OnValueChanged();
		return BtnBack;
	}

	private void _2305(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2324(BtnAdd, Operators.Add);
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private void _2306(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2324(BtnSubtract, Operators.Subtract);
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private void _2307(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2324(BtnMultiply, Operators.Multiply);
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private void _2308(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2324(BtnDivide, Operators.Divide);
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private void _2309(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_230A();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _230A()
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			__22EB = '-' + __22EB;
			__22EB = __22EB.Replace("--", "");
		}
		else
		{
			_ᗸ = 0.0 - _ᗸ;
		}
		OnValueChanged();
		return BtnNegate;
	}

	private void _230B(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_230C();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _230C()
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			double num = double.Parse(__22EB);
			if (num > 0.0)
			{
				num = Math.Sqrt(num);
				if (IsIntValue)
				{
					num = Math.Round(num);
				}
				__22EB = num.ToString();
			}
			else
			{
				SystemSounds.Beep.Play();
			}
		}
		else if (_ᗸ > 0.0)
		{
			_ᗸ = Math.Sqrt(_ᗸ);
		}
		else
		{
			SystemSounds.Beep.Play();
		}
		__22EF = Operators.Set;
		OnValueChanged();
		return BtnSqrt;
	}

	private void _230D(object _٠, EventArgs ؿ)
	{
		if (!_2326(BtnReciprocal))
		{
			_230E();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _230E()
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			double num = double.Parse(__22EB);
			if (num != 0.0)
			{
				num = 1.0 / num;
				if (IsIntValue)
				{
					num = Math.Round(num);
				}
				__22EB = num.ToString();
			}
			else
			{
				SystemSounds.Beep.Play();
			}
		}
		else if (_ᗸ != 0.0)
		{
			_ᗸ = 1.0 / _ᗸ;
		}
		else
		{
			SystemSounds.Beep.Play();
		}
		__22EF = Operators.Set;
		OnValueChanged();
		return BtnReciprocal;
	}

	private void _230F(object _٠, EventArgs ؿ)
	{
		if (!_2326(BtnPercent))
		{
			_2310();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2310()
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			double num = double.Parse(__22EB);
			num = _ᗸ * num / 100.0;
			if (IsIntValue)
			{
				num = Math.Round(num);
			}
			__22EB = num.ToString();
		}
		else
		{
			_ᗸ = 0.0;
		}
		OnValueChanged();
		return BtnPercent;
	}

	private void _2311(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2312();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2312()
	{
		_2325();
		__22EF = Operators.Calc;
		((Control)_22F9).Text = "";
		OnValueChanged();
		return BtnEquals;
	}

	private void _2313(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2314();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2314()
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			__22EB = "0";
		}
		else
		{
			_ᗸ = 0.0;
		}
		OnValueChanged();
		return BtnClearEntry;
	}

	private void _2315(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2316();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2316()
	{
		_ᗸ = 0.0;
		__22EC = 0.0;
		__22EB = null;
		_2322(Operators.None);
		OnValueChanged();
		return BtnClear;
	}

	private void _2317(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2318();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2318()
	{
		_2321(DisplayValue);
		return BtnMemStore;
	}

	private void _2319(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_231A();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _231A()
	{
		__22EB = __22ED.ToString();
		OnValueChanged();
		return BtnMemRestore;
	}

	private void _231B(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_231C();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _231C()
	{
		_2321(0.0);
		return BtnMemClear;
	}

	private void _231D(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_231E();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _231E()
	{
		_2321(__22ED + DisplayValue);
		return BtnMemAdd;
	}

	private void _231F(object _٠, EventArgs ؿ)
	{
		if (!_2326(_٠))
		{
			_2320();
		}
		if (__22F5)
		{
			((Control)this).Focus();
		}
	}

	private ButtonX _2320()
	{
		_2321(__22ED - DisplayValue);
		return BtnMemSubtract;
	}

	private void _2321(double ԝ)
	{
		__22ED = ԝ;
		((Control)_22F8).Text = ((ԝ == 0.0) ? "" : "M");
	}

	private void _2322(Operators _2323)
	{
		__22EE = _2323;
		__22EF = _2323;
		((Control)_22F9).Text = ((char)_2323).ToString();
	}

	private ButtonX _2324(ButtonX ख़, Operators _2323)
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			_2325();
		}
		_2322(_2323);
		OnValueChanged();
		return ख़;
	}

	private void _2325()
	{
		if (!string.IsNullOrEmpty(__22EB))
		{
			__22EC = double.Parse(__22EB);
		}
		switch (__22EE)
		{
		case Operators.None:
			_ᗸ = __22EC;
			break;
		case Operators.Add:
			_ᗸ += __22EC;
			break;
		case Operators.Subtract:
			_ᗸ -= __22EC;
			break;
		case Operators.Multiply:
			_ᗸ *= __22EC;
			break;
		case Operators.Divide:
			if (__22EC > 0.0)
			{
				_ᗸ /= __22EC;
			}
			else
			{
				SystemSounds.Beep.Play();
			}
			break;
		}
		__22EB = null;
		OnValueChanged();
	}

	private void ᘥ()
	{
		if (__22F2 switch
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

	private bool _2326(object ᤉ)
	{
		if (_22E9 != null)
		{
			ButtonClickEventArgs buttonClickEventArgs = new ButtonClickEventArgs(ᤉ);
			_22E9(this, buttonClickEventArgs);
			return buttonClickEventArgs.Cancel;
		}
		return false;
	}

	public void OnValueChanged()
	{
		if (ᗲ != null)
		{
			ᗲ(this, new ValueChangedEventArgs(DisplaySValue, DisplayValue));
		}
		_22FD();
		OnCalculatorDisplayChanged(EventArgs.Empty);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).Visible)
		{
			__22EB = null;
			__22EC = 0.0;
			_2321(0.0);
			_2322(Operators.Set);
			OnValueChanged();
		}
		((Control)this).OnVisibleChanged(e);
	}

	public void FlashKey(ButtonX keyButton)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (__22F3 == null)
		{
			__22F3 = new Timer();
			__22F3.Interval = 125;
			__22F3.Tick += _2329;
		}
		if (__22F3.Tag is ButtonX ख़)
		{
			__22F3.Stop();
			_2328(ख़);
		}
		_2327(keyButton);
		__22F3.Tag = keyButton;
		__22F3.Start();
	}

	private void _2327(ButtonX ख़)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		MouseEventArgs objArg = new MouseEventArgs((MouseButtons)1048576, 1, ((Control)ख़).Location.X, ((Control)ख़).Location.Y, 0);
		ख़.InternalItem.InternalMouseDown(objArg);
	}

	private void _2328(ButtonX ख़)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		MouseEventArgs objArg = new MouseEventArgs((MouseButtons)1048576, 1, ((Control)ख़).Location.X, ((Control)ख़).Location.Y, 0);
		ख़.InternalItem.InternalMouseUp(objArg);
	}

	private void _2329(object _٠, EventArgs ؿ)
	{
		if (__22F3.Tag is ButtonX ख़)
		{
			_2328(ख़);
		}
		__22F3.Stop();
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
		__21D2 = Size.Empty;
		_21DF();
	}

	private void _21DF()
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
		((Control)_22FC).Font = new Font(((Control)this).Font.FontFamily, ((Control)_22FC).Font.SizeInPoints);
		((Control)this).OnFontChanged(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!__21D2.IsEmpty)
		{
			return __21D2;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		__21D2 = ((Control)_22F7).Size;
		return __21D2;
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
			_21DF();
		}
		((Control)this).OnHandleCreated(e);
	}

	protected virtual void OnDisplayVisibleChanged(bool oldValue, bool newValue)
	{
		((Control)_22FC).Visible = newValue;
		if (newValue)
		{
			PanelEx panelEx = _22FB;
			((Control)panelEx).Top = ((Control)panelEx).Top + ((Control)_22FC).Height;
			PanelEx panelEx2 = _22FA;
			((Control)panelEx2).Top = ((Control)panelEx2).Top + ((Control)_22FC).Height;
			PanelEx panelEx3 = _22F7;
			((Control)panelEx3).Height = ((Control)panelEx3).Height + ((Control)_22FC).Height;
		}
		else
		{
			PanelEx panelEx4 = _22FB;
			((Control)panelEx4).Top = ((Control)panelEx4).Top - ((Control)_22FC).Height;
			PanelEx panelEx5 = _22FA;
			((Control)panelEx5).Top = ((Control)panelEx5).Top - ((Control)_22FC).Height;
			PanelEx panelEx6 = _22F7;
			((Control)panelEx6).Height = ((Control)panelEx6).Height - ((Control)_22FC).Height;
		}
		__21D2 = Size.Empty;
		_21DF();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void ـ()
	{
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Expected O, but got Unknown
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Expected O, but got Unknown
		//IL_0c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c25: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Expected O, but got Unknown
		//IL_0d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Expected O, but got Unknown
		//IL_0e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e34: Expected O, but got Unknown
		//IL_0ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee3: Expected O, but got Unknown
		//IL_1018: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Expected O, but got Unknown
		//IL_10c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d0: Expected O, but got Unknown
		//IL_1205: Unknown result type (might be due to invalid IL or missing references)
		//IL_120f: Expected O, but got Unknown
		//IL_12b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bd: Expected O, but got Unknown
		//IL_13f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1401: Expected O, but got Unknown
		//IL_1619: Unknown result type (might be due to invalid IL or missing references)
		//IL_1623: Expected O, but got Unknown
		//IL_16b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bb: Expected O, but got Unknown
		//IL_173a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1744: Expected O, but got Unknown
		//IL_17da: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e4: Expected O, but got Unknown
		//IL_187a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1884: Expected O, but got Unknown
		//IL_191a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1924: Expected O, but got Unknown
		//IL_19b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bf: Expected O, but got Unknown
		//IL_1a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9f: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Calculator));
		_22F7 = new PanelEx();
		_22FB = new PanelEx();
		BtnBack = new ButtonX();
		BtnDigit8 = new ButtonX();
		BtnReciprocal = new ButtonX();
		BtnDigit5 = new ButtonX();
		BtnSqrt = new ButtonX();
		BtnDivide = new ButtonX();
		BtnDigit9 = new ButtonX();
		BtnClearEntry = new ButtonX();
		BtnDecimal = new ButtonX();
		BtnAdd = new ButtonX();
		BtnEquals = new ButtonX();
		BtnDigit4 = new ButtonX();
		BtnSubtract = new ButtonX();
		BtnMultiply = new ButtonX();
		BtnDigit6 = new ButtonX();
		BtnClear = new ButtonX();
		BtnDigit1 = new ButtonX();
		BtnDigit2 = new ButtonX();
		BtnNegate = new ButtonX();
		BtnDigit0 = new ButtonX();
		BtnDigit3 = new ButtonX();
		BtnPercent = new ButtonX();
		BtnDigit7 = new ButtonX();
		_22FA = new PanelEx();
		BtnMemStore = new ButtonX();
		_22F9 = new LabelX();
		BtnMemRestore = new ButtonX();
		BtnMemAdd = new ButtonX();
		BtnMemClear = new ButtonX();
		BtnMemSubtract = new ButtonX();
		_22F8 = new LabelX();
		_22FC = new LabelX();
		((Control)_22F7).SuspendLayout();
		((Control)_22FB).SuspendLayout();
		((Control)_22FA).SuspendLayout();
		((Control)this).SuspendLayout();
		_22F7.CanvasColor = SystemColors.Control;
		_22F7.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)_22F7).Controls.Add((Control)(object)_22FB);
		((Control)_22F7).Controls.Add((Control)(object)_22FA);
		((Control)_22F7).Controls.Add((Control)(object)_22FC);
		((Control)_22F7).Location = new Point(0, 0);
		((Control)_22F7).Name = "pnlCalc";
		((Control)_22F7).Padding = new Padding(1);
		((Control)_22F7).Size = new Size(190, 211);
		_22F7.Style.Alignment = (StringAlignment)1;
		_22F7.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
		_22F7.Style.Border = eBorderType.SingleLine;
		_22F7.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		_22F7.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		_22F7.Style.GradientAngle = 90;
		((Control)_22F7).TabIndex = 0;
		_22FB.CanvasColor = SystemColors.Control;
		_22FB.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)_22FB).Controls.Add((Control)(object)BtnBack);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit8);
		((Control)_22FB).Controls.Add((Control)(object)BtnReciprocal);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit5);
		((Control)_22FB).Controls.Add((Control)(object)BtnSqrt);
		((Control)_22FB).Controls.Add((Control)(object)BtnDivide);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit9);
		((Control)_22FB).Controls.Add((Control)(object)BtnClearEntry);
		((Control)_22FB).Controls.Add((Control)(object)BtnDecimal);
		((Control)_22FB).Controls.Add((Control)(object)BtnAdd);
		((Control)_22FB).Controls.Add((Control)(object)BtnEquals);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit4);
		((Control)_22FB).Controls.Add((Control)(object)BtnSubtract);
		((Control)_22FB).Controls.Add((Control)(object)BtnMultiply);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit6);
		((Control)_22FB).Controls.Add((Control)(object)BtnClear);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit1);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit2);
		((Control)_22FB).Controls.Add((Control)(object)BtnNegate);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit0);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit3);
		((Control)_22FB).Controls.Add((Control)(object)BtnPercent);
		((Control)_22FB).Controls.Add((Control)(object)BtnDigit7);
		((Control)_22FB).Location = new Point(1, 58);
		((Control)_22FB).Name = "pnlPad";
		((Control)_22FB).Size = new Size(187, 151);
		_22FB.Style.Alignment = (StringAlignment)1;
		_22FB.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
		_22FB.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		_22FB.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		_22FB.Style.GradientAngle = 90;
		((Control)_22FB).TabIndex = 35;
		((Control)BtnBack).AccessibleRole = (AccessibleRole)43;
		BtnBack.ColorTable = eButtonColor.OrangeWithBackground;
		BtnBack.Image = (Image)componentResourceManager.GetObject("BtnBack.Image");
		((Control)BtnBack).Location = new Point(8, 6);
		((Control)BtnBack).Name = "BtnBack";
		((Control)BtnBack).Size = new Size(30, 23);
		BtnBack.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnBack).TabIndex = 0;
		((Control)BtnBack).Click += _2303;
		((Control)BtnDigit8).AccessibleRole = (AccessibleRole)43;
		BtnDigit8.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit8).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit8).Location = new Point(43, 36);
		((Control)BtnDigit8).Name = "BtnDigit8";
		((Control)BtnDigit8).Size = new Size(30, 23);
		BtnDigit8.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit8).TabIndex = 6;
		((Control)BtnDigit8).Text = "8";
		((Control)BtnDigit8).Click += _22FE;
		((Control)BtnReciprocal).AccessibleRole = (AccessibleRole)43;
		BtnReciprocal.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnReciprocal).Location = new Point(150, 65);
		((Control)BtnReciprocal).Name = "BtnReciprocal";
		((Control)BtnReciprocal).Size = new Size(30, 23);
		BtnReciprocal.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnReciprocal).TabIndex = 14;
		((Control)BtnReciprocal).Text = "1/x";
		((Control)BtnReciprocal).Click += _230D;
		((Control)BtnDigit5).AccessibleRole = (AccessibleRole)43;
		BtnDigit5.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit5).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit5).Location = new Point(43, 65);
		((Control)BtnDigit5).Name = "BtnDigit5";
		((Control)BtnDigit5).Size = new Size(30, 23);
		BtnDigit5.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit5).TabIndex = 11;
		((Control)BtnDigit5).Text = "5";
		((Control)BtnDigit5).Click += _22FE;
		((Control)BtnSqrt).AccessibleRole = (AccessibleRole)43;
		BtnSqrt.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnSqrt).Location = new Point(150, 6);
		((Control)BtnSqrt).Name = "BtnSqrt";
		((Control)BtnSqrt).Size = new Size(30, 23);
		BtnSqrt.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnSqrt).TabIndex = 4;
		((Control)BtnSqrt).Text = "√";
		((Control)BtnSqrt).Click += _230B;
		((Control)BtnDivide).AccessibleRole = (AccessibleRole)43;
		BtnDivide.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDivide).Location = new Point(114, 36);
		((Control)BtnDivide).Name = "BtnDivide";
		((Control)BtnDivide).Size = new Size(30, 23);
		BtnDivide.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDivide).TabIndex = 8;
		((Control)BtnDivide).Text = "/";
		((Control)BtnDivide).Click += _2308;
		((Control)BtnDigit9).AccessibleRole = (AccessibleRole)43;
		BtnDigit9.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit9).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit9).Location = new Point(78, 36);
		((Control)BtnDigit9).Name = "BtnDigit9";
		((Control)BtnDigit9).Size = new Size(30, 23);
		BtnDigit9.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit9).TabIndex = 7;
		((Control)BtnDigit9).Text = "9";
		((Control)BtnDigit9).Click += _22FE;
		((Control)BtnClearEntry).AccessibleRole = (AccessibleRole)43;
		BtnClearEntry.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnClearEntry).Location = new Point(43, 6);
		((Control)BtnClearEntry).Name = "BtnClearEntry";
		((Control)BtnClearEntry).Size = new Size(30, 23);
		BtnClearEntry.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnClearEntry).TabIndex = 1;
		((Control)BtnClearEntry).Text = "CE";
		((Control)BtnClearEntry).Click += _2313;
		((Control)BtnDecimal).AccessibleRole = (AccessibleRole)43;
		BtnDecimal.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDecimal).Font = new Font("Modern No. 20", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDecimal).Location = new Point(78, 123);
		((Control)BtnDecimal).Name = "BtnDecimal";
		((Control)BtnDecimal).Size = new Size(30, 23);
		BtnDecimal.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDecimal).TabIndex = 21;
		((Control)BtnDecimal).Text = ".";
		((Control)BtnDecimal).Click += _2301;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnAdd).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnAdd).Location = new Point(114, 123);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(30, 23);
		BtnAdd.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAdd).TabIndex = 22;
		((Control)BtnAdd).Text = "+";
		((Control)BtnAdd).Click += _2305;
		((Control)BtnEquals).AccessibleRole = (AccessibleRole)43;
		BtnEquals.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnEquals).Font = new Font("Microsoft Sans Serif", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnEquals).Location = new Point(150, 94);
		((Control)BtnEquals).Name = "BtnEquals";
		((Control)BtnEquals).Size = new Size(30, 52);
		BtnEquals.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnEquals).TabIndex = 19;
		((Control)BtnEquals).Text = "=";
		((Control)BtnEquals).Click += _2311;
		((Control)BtnDigit4).AccessibleRole = (AccessibleRole)43;
		BtnDigit4.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit4).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit4).Location = new Point(8, 65);
		((Control)BtnDigit4).Name = "BtnDigit4";
		((Control)BtnDigit4).Size = new Size(30, 23);
		BtnDigit4.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit4).TabIndex = 10;
		((Control)BtnDigit4).Text = "4";
		((Control)BtnDigit4).Click += _22FE;
		((Control)BtnSubtract).AccessibleRole = (AccessibleRole)43;
		BtnSubtract.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnSubtract).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnSubtract).Location = new Point(114, 94);
		((Control)BtnSubtract).Name = "BtnSubtract";
		((Control)BtnSubtract).Size = new Size(30, 23);
		BtnSubtract.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnSubtract).TabIndex = 18;
		((Control)BtnSubtract).Text = "-";
		((Control)BtnSubtract).Click += _2306;
		((Control)BtnMultiply).AccessibleRole = (AccessibleRole)43;
		BtnMultiply.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnMultiply).Font = new Font("Symbol", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)2);
		((Control)BtnMultiply).Location = new Point(114, 65);
		((Control)BtnMultiply).Name = "BtnMultiply";
		((Control)BtnMultiply).Size = new Size(30, 23);
		BtnMultiply.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnMultiply).TabIndex = 13;
		((Control)BtnMultiply).Text = "*";
		((Control)BtnMultiply).Click += _2307;
		((Control)BtnDigit6).AccessibleRole = (AccessibleRole)43;
		BtnDigit6.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit6).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit6).Location = new Point(78, 65);
		((Control)BtnDigit6).Name = "BtnDigit6";
		((Control)BtnDigit6).Size = new Size(30, 23);
		BtnDigit6.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit6).TabIndex = 12;
		((Control)BtnDigit6).Text = "6";
		((Control)BtnDigit6).Click += _22FE;
		((Control)BtnClear).AccessibleRole = (AccessibleRole)43;
		BtnClear.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnClear).Location = new Point(79, 6);
		((Control)BtnClear).Name = "BtnClear";
		((Control)BtnClear).Size = new Size(30, 23);
		BtnClear.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnClear).TabIndex = 2;
		((Control)BtnClear).Text = "C";
		((Control)BtnClear).Click += _2315;
		((Control)BtnDigit1).AccessibleRole = (AccessibleRole)43;
		BtnDigit1.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit1).Location = new Point(8, 94);
		((Control)BtnDigit1).Name = "BtnDigit1";
		((Control)BtnDigit1).Size = new Size(30, 23);
		BtnDigit1.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit1).TabIndex = 15;
		((Control)BtnDigit1).Text = "1";
		((Control)BtnDigit1).Click += _22FE;
		((Control)BtnDigit2).AccessibleRole = (AccessibleRole)43;
		BtnDigit2.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit2).Location = new Point(43, 94);
		((Control)BtnDigit2).Name = "BtnDigit2";
		((Control)BtnDigit2).Size = new Size(30, 23);
		BtnDigit2.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit2).TabIndex = 16;
		((Control)BtnDigit2).Text = "2";
		((Control)BtnDigit2).Click += _22FE;
		((Control)BtnNegate).AccessibleRole = (AccessibleRole)43;
		BtnNegate.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnNegate).Location = new Point(114, 6);
		((Control)BtnNegate).Name = "BtnNegate";
		((Control)BtnNegate).Size = new Size(30, 23);
		BtnNegate.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnNegate).TabIndex = 3;
		((Control)BtnNegate).Text = "±";
		((Control)BtnNegate).Click += _2309;
		((Control)BtnDigit0).AccessibleRole = (AccessibleRole)43;
		BtnDigit0.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit0).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit0).Location = new Point(8, 123);
		((Control)BtnDigit0).Name = "BtnDigit0";
		((Control)BtnDigit0).Size = new Size(65, 23);
		BtnDigit0.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit0).TabIndex = 20;
		((Control)BtnDigit0).Text = "0";
		((Control)BtnDigit0).Click += _22FE;
		((Control)BtnDigit3).AccessibleRole = (AccessibleRole)43;
		BtnDigit3.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit3).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit3).Location = new Point(78, 94);
		((Control)BtnDigit3).Name = "BtnDigit3";
		((Control)BtnDigit3).Size = new Size(30, 23);
		BtnDigit3.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit3).TabIndex = 17;
		((Control)BtnDigit3).Text = "3";
		((Control)BtnDigit3).Click += _22FE;
		((Control)BtnPercent).AccessibleRole = (AccessibleRole)43;
		BtnPercent.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnPercent).Location = new Point(150, 36);
		((Control)BtnPercent).Name = "BtnPercent";
		((Control)BtnPercent).Size = new Size(30, 23);
		BtnPercent.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnPercent).TabIndex = 9;
		((Control)BtnPercent).Text = "%";
		((Control)BtnPercent).Click += _230F;
		((Control)BtnDigit7).AccessibleRole = (AccessibleRole)43;
		BtnDigit7.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnDigit7).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnDigit7).Location = new Point(8, 36);
		((Control)BtnDigit7).Name = "BtnDigit7";
		((Control)BtnDigit7).Size = new Size(30, 23);
		BtnDigit7.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDigit7).TabIndex = 5;
		((Control)BtnDigit7).Text = "7";
		((Control)BtnDigit7).Click += _22FE;
		_22FA.CanvasColor = SystemColors.Control;
		_22FA.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)_22FA).Controls.Add((Control)(object)BtnMemStore);
		((Control)_22FA).Controls.Add((Control)(object)_22F9);
		((Control)_22FA).Controls.Add((Control)(object)BtnMemRestore);
		((Control)_22FA).Controls.Add((Control)(object)BtnMemAdd);
		((Control)_22FA).Controls.Add((Control)(object)BtnMemClear);
		((Control)_22FA).Controls.Add((Control)(object)BtnMemSubtract);
		((Control)_22FA).Controls.Add((Control)(object)_22F8);
		((Control)_22FA).Location = new Point(1, 33);
		((Control)_22FA).Name = "pnlMem";
		((Control)_22FA).Size = new Size(187, 26);
		_22FA.Style.Alignment = (StringAlignment)1;
		_22FA.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
		_22FA.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		_22FA.Style.BorderWidth = 0;
		_22FA.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		_22FA.Style.GradientAngle = 90;
		((Control)_22FA).TabIndex = 34;
		((Control)BtnMemStore).AccessibleRole = (AccessibleRole)43;
		BtnMemStore.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnMemStore).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnMemStore).Location = new Point(80, 6);
		((Control)BtnMemStore).Name = "BtnMemStore";
		((Control)BtnMemStore).Size = new Size(27, 18);
		((Control)BtnMemStore).TabIndex = 0;
		((Control)BtnMemStore).Text = "MS";
		((Control)BtnMemStore).Click += _2317;
		_22F9.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)_22F9).Font = new Font("Symbol", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)2);
		((Control)_22F9).Location = new Point(174, 6);
		((Control)_22F9).Name = "lbxOperator";
		((Control)_22F9).Size = new Size(12, 18);
		((Control)_22F9).TabIndex = 33;
		_22F9.TextAlignment = (StringAlignment)1;
		((Control)BtnMemRestore).AccessibleRole = (AccessibleRole)43;
		BtnMemRestore.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnMemRestore).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnMemRestore).Location = new Point(48, 6);
		((Control)BtnMemRestore).Name = "BtnMemRestore";
		((Control)BtnMemRestore).Size = new Size(27, 18);
		((Control)BtnMemRestore).TabIndex = 6;
		((Control)BtnMemRestore).Text = "MR";
		((Control)BtnMemRestore).Click += _2319;
		((Control)BtnMemAdd).AccessibleRole = (AccessibleRole)43;
		BtnMemAdd.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnMemAdd).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnMemAdd).Location = new Point(112, 6);
		((Control)BtnMemAdd).Name = "BtnMemAdd";
		((Control)BtnMemAdd).Size = new Size(27, 18);
		((Control)BtnMemAdd).TabIndex = 1;
		((Control)BtnMemAdd).Text = "M+";
		((Control)BtnMemAdd).Click += _231D;
		((Control)BtnMemClear).AccessibleRole = (AccessibleRole)43;
		BtnMemClear.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnMemClear).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnMemClear).Location = new Point(16, 6);
		((Control)BtnMemClear).Name = "BtnMemClear";
		((Control)BtnMemClear).Size = new Size(27, 18);
		((Control)BtnMemClear).TabIndex = 5;
		((Control)BtnMemClear).Text = "MC";
		((Control)BtnMemClear).Click += _231B;
		((Control)BtnMemSubtract).AccessibleRole = (AccessibleRole)43;
		BtnMemSubtract.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnMemSubtract).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)BtnMemSubtract).Location = new Point(144, 6);
		((Control)BtnMemSubtract).Name = "BtnMemSubtract";
		((Control)BtnMemSubtract).Size = new Size(27, 18);
		((Control)BtnMemSubtract).TabIndex = 2;
		((Control)BtnMemSubtract).Text = "M-";
		((Control)BtnMemSubtract).Click += _231F;
		_22F8.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)_22F8).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)_22F8).Location = new Point(4, 5);
		((Control)_22F8).Name = "lbxMemory";
		((Control)_22F8).Size = new Size(12, 18);
		((Control)_22F8).TabIndex = 31;
		_22F8.TextAlignment = (StringAlignment)1;
		_22FC.BackgroundStyle.BackColor2SchemePart = eColorSchemePart.MenuBackground;
		_22FC.BackgroundStyle.BorderBottom = eStyleBorderType.Solid;
		_22FC.BackgroundStyle.BorderBottomColorSchemePart = eColorSchemePart.PanelBorder;
		_22FC.BackgroundStyle.BorderBottomWidth = 1;
		_22FC.BackgroundStyle.CornerType = eCornerType.Square;
		_22FC.BackgroundStyle.PaddingRight = 6;
		((Control)_22FC).Dock = (DockStyle)1;
		((Control)_22FC).Font = new Font("Consolas", 18f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)_22FC).Location = new Point(1, 1);
		((Control)_22FC).Name = "labelValue";
		((Control)_22FC).Size = new Size(188, 30);
		((Control)_22FC).TabIndex = 1;
		((Control)_22FC).Text = "0";
		_22FC.TextAlignment = (StringAlignment)2;
		_22FC.BackgroundStyle.TextTrimming = eStyleTextTrimming.Character;
		((Control)this).Controls.Add((Control)(object)_22F7);
		((Control)this).Name = "Calculator";
		((Control)this).Size = new Size(190, 212);
		((Control)_22F7).ResumeLayout(false);
		((Control)_22FB).ResumeLayout(false);
		((Control)_22FA).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
