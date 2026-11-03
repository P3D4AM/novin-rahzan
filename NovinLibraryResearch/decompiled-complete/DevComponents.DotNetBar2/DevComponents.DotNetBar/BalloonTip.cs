using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[ComVisible(false)]
[ProvideProperty("BalloonCaption", typeof(Control))]
[ProvideProperty("BalloonText", typeof(Control))]
public class BalloonTip : Component, IExtenderProvider
{
	private class _2A8A
	{
		public string _༥;

		public string ᆺ;
	}

	private EventHandler _2A7D;

	private CancelEventHandler _2A7E;

	private bool _2A7F;

	private bool ݕ = true;

	private bool _2A80;

	private eBallonStyle Ӷ;

	private int _2A81 = 500;

	private bool _2434 = true;

	private int _2435 = 5;

	private eAlertAnimation _243A = eAlertAnimation.BottomToTop;

	private int _243B = 200;

	private bool _2A82;

	private bool _2427 = true;

	private Balloon _2A83;

	private Image _242C;

	private Icon _242D;

	private Hashtable _2A84 = new Hashtable();

	private Control _2A85;

	private Control _2A86;

	private Timer _2A87;

	private Control _2A88;

	private int _2A89 = 256;

	private bool ե = true;

	private int __243C = 180;

	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			ե = value;
		}
	}

	[DefaultValue(true)]
	[Category("Misc")]
	[Description("Gets or sets a value indicating whether the BalloonTip is currently active.")]
	public bool Enabled
	{
		get
		{
			return ݕ;
		}
		set
		{
			ݕ = value;
		}
	}

	public Control BalloonTriggerControl => _2A88;

	[Description("Indicates minimum balloon width when auto sizing balloon.")]
	[DefaultValue(180)]
	[Category("Appearance")]
	public int MinimumBalloonWidth
	{
		get
		{
			return __243C;
		}
		set
		{
			__243C = value;
		}
	}

	[Description("Gets or sets a value indicating whether Balloon receives input focus when displayed.")]
	[Category("Misc")]
	[DefaultValue(false)]
	public bool BalloonFocus
	{
		get
		{
			return _2A7F;
		}
		set
		{
			_2A7F = value;
		}
	}

	[Description("Indicates the time (in milliseconds) that passes before the BalloonTip appears.")]
	[Category("Misc")]
	[DefaultValue(500)]
	public int InitialDelay
	{
		get
		{
			return _2A81;
		}
		set
		{
			_2A81 = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether a Balloon window is displayed even when its parent form is not active.")]
	[Category("Misc")]
	public bool ShowAlways
	{
		get
		{
			return _2A80;
		}
		set
		{
			_2A80 = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Balloon BalloonControl
	{
		get
		{
			return _2A83;
		}
		set
		{
			_2A83 = value;
		}
	}

	[Category("Style")]
	[DevCoBrowsable(true)]
	[Description("Specifies balloon style.")]
	[DefaultValue(eBallonStyle.Balloon)]
	[Browsable(true)]
	public eBallonStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			if (Ӷ != value)
			{
				Ӷ = value;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(eAlertAnimation.BottomToTop)]
	[Description("Gets or sets the animation type used to display Alert type balloon.")]
	[Browsable(true)]
	public eAlertAnimation AlertAnimation
	{
		get
		{
			return _243A;
		}
		set
		{
			_243A = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(200)]
	[Description("Gets or sets the total time in milliseconds alert animation takes.")]
	public int AlertAnimationDuration
	{
		get
		{
			return _243B;
		}
		set
		{
			_243B = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether balloon will close automatically when user click the close button.")]
	[DefaultValue(true)]
	public bool AutoClose
	{
		get
		{
			return _2434;
		}
		set
		{
			_2434 = value;
		}
	}

	[Description("Indicates time period in seconds after balloon closes automatically.")]
	[DefaultValue(5)]
	[Browsable(true)]
	[Category("Behavior")]
	public int AutoCloseTimeOut
	{
		get
		{
			return _2435;
		}
		set
		{
			_2435 = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether Balloon is shown after control receives input focus.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool ShowBalloonOnFocus
	{
		get
		{
			return _2A82;
		}
		set
		{
			if (_2A82 != value)
			{
				_2A82 = value;
			}
		}
	}

	[Description("Indicates whether the Balloon Close button is displayed.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	public bool ShowCloseButton
	{
		get
		{
			return _2427;
		}
		set
		{
			if (value != _2427)
			{
				_2427 = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates default balloon width. Usually the width of the balloon is calculated based on the width of the caption text. If caption text is not set then this value will be used as default width of the balloon.")]
	[DefaultValue(256)]
	[Category("Appearance")]
	public int DefaultBalloonWidth
	{
		get
		{
			return _2A89;
		}
		set
		{
			_2A89 = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates Balloon Caption image.")]
	[DefaultValue(null)]
	public Image CaptionImage
	{
		get
		{
			return _242C;
		}
		set
		{
			if (_242C != value)
			{
				_242C = value;
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates Balloon Caption icon. Icon is used to provide support for alpha-blended images in caption.")]
	public Icon CaptionIcon
	{
		get
		{
			return _242D;
		}
		set
		{
			if (_242D != value)
			{
				_242D = value;
			}
		}
	}

	[Description("Occurs before balloon is displayed.")]
	public event EventHandler BalloonDisplaying
	{
		add
		{
			EventHandler eventHandler = _2A7D;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2A7D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2A7D;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2A7D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before balloon is closed and allows to cancel the action.")]
	public event CancelEventHandler BalloonClosing
	{
		add
		{
			CancelEventHandler cancelEventHandler = _2A7E;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2A7E, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _2A7E;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2A7E, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		RemoveAll();
		_2A85 = null;
		_2A86 = null;
		_2A88 = null;
		if (BarUtilities.DisposeItemImages && !base.DesignMode)
		{
			BarUtilities.ᆟ(ref _242C);
			BarUtilities.ᆟ(ref _242D);
		}
		DestroyDelayTimer();
		base.Dispose(disposing);
	}

	bool IExtenderProvider.CanExtend(object target)
	{
		if (target is Control)
		{
			return true;
		}
		return false;
	}

	[DefaultValue("")]
	[Localizable(true)]
	public string GetBalloonCaption(Control control)
	{
		if (_2A84.Contains(control) && _2A84[control] is _2A8A _2A8A)
		{
			return _2A8A._༥;
		}
		return "";
	}

	[Localizable(true)]
	public void SetBalloonCaption(Control control, string caption)
	{
		if (caption == null)
		{
			caption = "";
		}
		if (_2A84.Contains(control))
		{
			if (_2A84[control] is _2A8A _2A8A)
			{
				_2A8A._༥ = caption;
				if (_2A8A._༥ == "" && _2A8A.ᆺ == "")
				{
					Remove(control);
				}
			}
		}
		else if (caption != "")
		{
			_2A8A _2A8A2 = new _2A8A();
			_2A8A2._༥ = caption;
			AddControl(control, _2A8A2);
		}
	}

	[Localizable(true)]
	[DefaultValue("")]
	public string GetBalloonText(Control control)
	{
		if (_2A84.Contains(control) && _2A84[control] is _2A8A _2A8A)
		{
			return _2A8A.ᆺ;
		}
		return "";
	}

	[Localizable(true)]
	public void SetBalloonText(Control control, string text)
	{
		if (text == null)
		{
			text = "";
		}
		if (_2A84.Contains(control))
		{
			if (_2A84[control] is _2A8A _2A8A)
			{
				_2A8A.ᆺ = text;
				if (_2A8A._༥ == "" && _2A8A.ᆺ == "")
				{
					Remove(control);
				}
			}
		}
		else if (text != "")
		{
			_2A8A _2A8A2 = new _2A8A();
			_2A8A2.ᆺ = text;
			AddControl(control, _2A8A2);
		}
	}

	public void RemoveAll()
	{
		Control[] array = (Control[])(object)new Control[_2A84.Keys.Count];
		_2A84.Keys.CopyTo(array, 0);
		Control[] array2 = array;
		foreach (Control control in array2)
		{
			Remove(control);
		}
		_2A84.Clear();
	}

	public void Remove(Control control)
	{
		if (_2A84.Contains(control))
		{
			try
			{
				control.MouseEnter -= ControlMouseEnter;
				control.MouseLeave -= ControlMouseLeave;
				control.Enter -= ControlGotFocus;
				control.Leave -= ControlLeave;
			}
			catch
			{
			}
			_2A84.Remove(control);
		}
	}

	private void AddControl(Control control, _2A8A info)
	{
		if (!_2A84.Contains(control))
		{
			_2A84.Add(control, info);
			control.MouseEnter += ControlMouseEnter;
			control.MouseLeave += ControlMouseLeave;
			control.Enter += ControlGotFocus;
			control.Leave += ControlLeave;
		}
	}

	public virtual void ShowBalloon(Control control)
	{
		if (!_2A84.Contains(control) || !(_2A84[control] is _2A8A info))
		{
			return;
		}
		CloseBalloon();
		if (_2A83 != null)
		{
			return;
		}
		_2A83 = CreateBalloonControl(info);
		((Form)_2A83).Location = new Point(0, 0);
		_2A88 = control;
		if (_2A7D != null)
		{
			_2A7D(this, new EventArgs());
		}
		if (_2A83 != null)
		{
			Form val = control.FindForm();
			if (val != null)
			{
				((Form)_2A83).Owner = val;
			}
			if (!((Form)_2A83).Location.IsEmpty)
			{
				_2A83.Show(_2A7F);
			}
			else
			{
				_2A83.Show(control, _2A7F);
			}
		}
	}

	private Balloon CreateBalloonControl(_2A8A info)
	{
		Balloon balloon = new Balloon();
		balloon.CaptionText = info._༥;
		((Control)balloon).Text = info.ᆺ;
		balloon.AutoClose = _2434;
		balloon.AutoCloseTimeOut = _2435;
		balloon.CaptionImage = CaptionImage;
		balloon.CaptionIcon = CaptionIcon;
		balloon.AlertAnimation = _243A;
		balloon.AlertAnimationDuration = _243B;
		balloon.ShowCloseButton = _2427;
		balloon.Style = Ӷ;
		balloon.MinimumBalloonWidth = __243C;
		if (info._༥ == "" || info._༥ == null)
		{
			((Control)balloon).Width = DefaultBalloonWidth;
		}
		else
		{
			balloon.AutoResize();
		}
		return balloon;
	}

	public virtual void CloseBalloon()
	{
		_2A88 = null;
		if (_2A83 == null)
		{
			return;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs(cancel: false);
		if (_2A7E != null)
		{
			_2A7E(this, cancelEventArgs);
		}
		if (!cancelEventArgs.Cancel)
		{
			if (_2A83.Visible)
			{
				_2A83.Hide();
			}
			((Form)_2A83).Close();
			((Component)(object)_2A83).Dispose();
			_2A83 = null;
		}
	}

	private void DestroyDelayTimer()
	{
		if (_2A87 != null)
		{
			_2A87.Stop();
			_2A87.Tick -= DelayTimerTick;
			((Component)(object)_2A87).Dispose();
			_2A87 = null;
		}
	}

	private void DelayTimerTick(object sender, EventArgs e)
	{
		_2A87.Enabled = false;
		DestroyDelayTimer();
		if (ݕ)
		{
			if (_2A82 && _2A86 != null)
			{
				ShowBalloon(_2A86);
			}
			else if (!_2A82 && _2A85 != null)
			{
				ShowBalloon(_2A85);
			}
		}
	}

	private void ShowBalloonDelayed(Control control)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		if (control == null || !ݕ)
		{
			return;
		}
		if (!_2A80)
		{
			Form val = control.FindForm();
			if (val.IsMdiChild)
			{
				if (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)
				{
					return;
				}
			}
			else if (Form.ActiveForm != val)
			{
				return;
			}
		}
		if (_2A81 == 0)
		{
			ShowBalloon(control);
		}
		else if (_2A87 == null)
		{
			_2A87 = new Timer();
			_2A87.Tick += DelayTimerTick;
			_2A87.Interval = _2A81;
			_2A87.Start();
		}
	}

	private void OnMouseEnter()
	{
		if (ݕ && _2A85 != null && !_2A82 && _2A84.Contains(_2A85))
		{
			ShowBalloonDelayed(_2A85);
		}
	}

	private void OnMouseLeave()
	{
		if (!_2A82)
		{
			DestroyDelayTimer();
			CloseBalloon();
		}
	}

	private void OnControlGotFocus()
	{
		if (ݕ && _2A86 != null && _2A82 && _2A84.Contains(_2A86))
		{
			ShowBalloonDelayed(_2A86);
		}
	}

	private void OnControlLeave()
	{
		if (_2A82)
		{
			DestroyDelayTimer();
			CloseBalloon();
		}
	}

	private void ControlMouseEnter(object sender, EventArgs e)
	{
		_2A85 = (Control)((sender is Control) ? sender : null);
		OnMouseEnter();
	}

	private void ControlMouseLeave(object sender, EventArgs e)
	{
		_2A85 = null;
		OnMouseLeave();
	}

	private void ControlGotFocus(object sender, EventArgs e)
	{
		_2A86 = (Control)((sender is Control) ? sender : null);
		OnControlGotFocus();
	}

	private void ControlLeave(object sender, EventArgs e)
	{
		_2A86 = null;
		OnControlLeave();
	}
}
