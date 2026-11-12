using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ProvideProperty("BalloonCaption", typeof(Control))]
[ProvideProperty("BalloonText", typeof(Control))]
[ComVisible(false)]
public class BalloonTip : Component, IExtenderProvider
{
	private class BalloonTipInfo
	{
		public string Caption;

		public string Text;
	}

	private bool m_BalloonFocus;

	private bool m_Enabled = true;

	private bool m_ShowAlways;

	private eBallonStyle m_Style;

	private int m_InitialDelay = 500;

	private bool m_AutoClose = true;

	private int m_AutoCloseTimeOut = 5;

	private eAlertAnimation m_AlertAnimation = eAlertAnimation.BottomToTop;

	private int m_AlertAnimationDuration = 200;

	private bool m_ShowBalloonOnFocus;

	private bool m_ShowCloseButton = true;

	private Balloon m_BalloonControl;

	private Image m_CaptionImage;

	private Icon m_CaptionIcon;

	private Hashtable m_BalloonsInfo = new Hashtable();

	private Control m_MouseOverControl;

	private Control m_FocusedControl;

	private Timer m_DelayTimer;

	private Control m_BalloonTriggerControl;

	private int m_DefaultBalloonWidth = 256;

	private bool m_AntiAlias = true;

	private int _MinimumBalloonWidth = 180;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return m_AntiAlias;
		}
		set
		{
			m_AntiAlias = value;
		}
	}

	[DefaultValue(true)]
	[Category("Misc")]
	[Description("Gets or sets a value indicating whether the BalloonTip is currently active.")]
	public bool Enabled
	{
		get
		{
			return m_Enabled;
		}
		set
		{
			m_Enabled = value;
		}
	}

	public Control BalloonTriggerControl => m_BalloonTriggerControl;

	[DefaultValue(180)]
	[Category("Appearance")]
	[Description("Indicates minimum balloon width when auto sizing balloon.")]
	public int MinimumBalloonWidth
	{
		get
		{
			return _MinimumBalloonWidth;
		}
		set
		{
			_MinimumBalloonWidth = value;
		}
	}

	[DefaultValue(false)]
	[Category("Misc")]
	[Description("Gets or sets a value indicating whether Balloon receives input focus when displayed.")]
	public bool BalloonFocus
	{
		get
		{
			return m_BalloonFocus;
		}
		set
		{
			m_BalloonFocus = value;
		}
	}

	[DefaultValue(500)]
	[Category("Misc")]
	[Description("Indicates the time (in milliseconds) that passes before the BalloonTip appears.")]
	public int InitialDelay
	{
		get
		{
			return m_InitialDelay;
		}
		set
		{
			m_InitialDelay = value;
		}
	}

	[DefaultValue(false)]
	[Category("Misc")]
	[Description("Indicates whether a Balloon window is displayed even when its parent form is not active.")]
	public bool ShowAlways
	{
		get
		{
			return m_ShowAlways;
		}
		set
		{
			m_ShowAlways = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Balloon BalloonControl
	{
		get
		{
			return m_BalloonControl;
		}
		set
		{
			m_BalloonControl = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(eBallonStyle.Balloon)]
	[Description("Specifies balloon style.")]
	public eBallonStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			if (m_Style != value)
			{
				m_Style = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Gets or sets the animation type used to display Alert type balloon.")]
	[Category("Behavior")]
	[DefaultValue(eAlertAnimation.BottomToTop)]
	public eAlertAnimation AlertAnimation
	{
		get
		{
			return m_AlertAnimation;
		}
		set
		{
			m_AlertAnimation = value;
		}
	}

	[Browsable(true)]
	[Description("Gets or sets the total time in milliseconds alert animation takes.")]
	[Category("Behavior")]
	[DefaultValue(200)]
	public int AlertAnimationDuration
	{
		get
		{
			return m_AlertAnimationDuration;
		}
		set
		{
			m_AlertAnimationDuration = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether balloon will close automatically when user click the close button.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool AutoClose
	{
		get
		{
			return m_AutoClose;
		}
		set
		{
			m_AutoClose = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates time period in seconds after balloon closes automatically.")]
	[Category("Behavior")]
	[DefaultValue(5)]
	public int AutoCloseTimeOut
	{
		get
		{
			return m_AutoCloseTimeOut;
		}
		set
		{
			m_AutoCloseTimeOut = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether Balloon is shown after control receives input focus.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool ShowBalloonOnFocus
	{
		get
		{
			return m_ShowBalloonOnFocus;
		}
		set
		{
			if (m_ShowBalloonOnFocus != value)
			{
				m_ShowBalloonOnFocus = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the Balloon Close button is displayed.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ShowCloseButton
	{
		get
		{
			return m_ShowCloseButton;
		}
		set
		{
			if (value != m_ShowCloseButton)
			{
				m_ShowCloseButton = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates default balloon width. Usually the width of the balloon is calculated based on the width of the caption text. If caption text is not set then this value will be used as default width of the balloon.")]
	[Category("Appearance")]
	[DefaultValue(256)]
	public int DefaultBalloonWidth
	{
		get
		{
			return m_DefaultBalloonWidth;
		}
		set
		{
			m_DefaultBalloonWidth = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates Balloon Caption image.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Image CaptionImage
	{
		get
		{
			return m_CaptionImage;
		}
		set
		{
			if (m_CaptionImage != value)
			{
				m_CaptionImage = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates Balloon Caption icon. Icon is used to provide support for alpha-blended images in caption.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Icon CaptionIcon
	{
		get
		{
			return m_CaptionIcon;
		}
		set
		{
			if (m_CaptionIcon != value)
			{
				m_CaptionIcon = value;
			}
		}
	}

	[Description("Occurs before balloon is displayed.")]
	public event EventHandler BalloonDisplaying;

	[Description("Occurs before balloon is closed and allows to cancel the action.")]
	public event CancelEventHandler BalloonClosing;

	protected override void Dispose(bool disposing)
	{
		RemoveAll();
		m_MouseOverControl = null;
		m_FocusedControl = null;
		m_BalloonTriggerControl = null;
		if (BarUtilities.DisposeItemImages && !base.DesignMode)
		{
			BarUtilities.DisposeImage(ref m_CaptionImage);
			BarUtilities.DisposeImage(ref m_CaptionIcon);
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
		if (m_BalloonsInfo.Contains(control) && m_BalloonsInfo[control] is BalloonTipInfo balloonTipInfo)
		{
			return balloonTipInfo.Caption;
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
		if (m_BalloonsInfo.Contains(control))
		{
			if (m_BalloonsInfo[control] is BalloonTipInfo balloonTipInfo)
			{
				balloonTipInfo.Caption = caption;
				if (balloonTipInfo.Caption == "" && balloonTipInfo.Text == "")
				{
					Remove(control);
				}
			}
		}
		else if (caption != "")
		{
			BalloonTipInfo balloonTipInfo2 = new BalloonTipInfo();
			balloonTipInfo2.Caption = caption;
			AddControl(control, balloonTipInfo2);
		}
	}

	[DefaultValue("")]
	[Localizable(true)]
	public string GetBalloonText(Control control)
	{
		if (m_BalloonsInfo.Contains(control) && m_BalloonsInfo[control] is BalloonTipInfo balloonTipInfo)
		{
			return balloonTipInfo.Text;
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
		if (m_BalloonsInfo.Contains(control))
		{
			if (m_BalloonsInfo[control] is BalloonTipInfo balloonTipInfo)
			{
				balloonTipInfo.Text = text;
				if (balloonTipInfo.Caption == "" && balloonTipInfo.Text == "")
				{
					Remove(control);
				}
			}
		}
		else if (text != "")
		{
			BalloonTipInfo balloonTipInfo2 = new BalloonTipInfo();
			balloonTipInfo2.Text = text;
			AddControl(control, balloonTipInfo2);
		}
	}

	public void RemoveAll()
	{
		Control[] array = (Control[])(object)new Control[m_BalloonsInfo.Keys.Count];
		m_BalloonsInfo.Keys.CopyTo(array, 0);
		Control[] array2 = array;
		foreach (Control control in array2)
		{
			Remove(control);
		}
		m_BalloonsInfo.Clear();
	}

	public void Remove(Control control)
	{
		if (m_BalloonsInfo.Contains(control))
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
			m_BalloonsInfo.Remove(control);
		}
	}

	private void AddControl(Control control, BalloonTipInfo info)
	{
		if (!m_BalloonsInfo.Contains(control))
		{
			m_BalloonsInfo.Add(control, info);
			control.MouseEnter += ControlMouseEnter;
			control.MouseLeave += ControlMouseLeave;
			control.Enter += ControlGotFocus;
			control.Leave += ControlLeave;
		}
	}

	public virtual void ShowBalloon(Control control)
	{
		if (!m_BalloonsInfo.Contains(control) || !(m_BalloonsInfo[control] is BalloonTipInfo info))
		{
			return;
		}
		CloseBalloon();
		if (m_BalloonControl != null)
		{
			return;
		}
		m_BalloonControl = CreateBalloonControl(info);
		((Form)m_BalloonControl).Location = new Point(0, 0);
		m_BalloonTriggerControl = control;
		if (this.BalloonDisplaying != null)
		{
			this.BalloonDisplaying(this, new EventArgs());
		}
		if (m_BalloonControl != null)
		{
			Form val = control.FindForm();
			if (val != null)
			{
				((Form)m_BalloonControl).Owner = val;
			}
			if (!((Form)m_BalloonControl).Location.IsEmpty)
			{
				m_BalloonControl.Show(m_BalloonFocus);
			}
			else
			{
				m_BalloonControl.Show(control, m_BalloonFocus);
			}
		}
	}

	private Balloon CreateBalloonControl(BalloonTipInfo info)
	{
		Balloon balloon = new Balloon();
		balloon.CaptionText = info.Caption;
		((Control)balloon).Text = info.Text;
		balloon.AutoClose = m_AutoClose;
		balloon.AutoCloseTimeOut = m_AutoCloseTimeOut;
		balloon.CaptionImage = CaptionImage;
		balloon.CaptionIcon = CaptionIcon;
		balloon.AlertAnimation = m_AlertAnimation;
		balloon.AlertAnimationDuration = m_AlertAnimationDuration;
		balloon.ShowCloseButton = m_ShowCloseButton;
		balloon.Style = m_Style;
		balloon.MinimumBalloonWidth = _MinimumBalloonWidth;
		if (info.Caption == "" || info.Caption == null)
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
		m_BalloonTriggerControl = null;
		if (m_BalloonControl == null)
		{
			return;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs(cancel: false);
		if (this.BalloonClosing != null)
		{
			this.BalloonClosing(this, cancelEventArgs);
		}
		if (!cancelEventArgs.Cancel)
		{
			if (m_BalloonControl.Visible)
			{
				m_BalloonControl.Hide();
			}
			((Form)m_BalloonControl).Close();
			((Component)(object)m_BalloonControl).Dispose();
			m_BalloonControl = null;
		}
	}

	private void DestroyDelayTimer()
	{
		if (m_DelayTimer != null)
		{
			m_DelayTimer.Stop();
			m_DelayTimer.Tick -= DelayTimerTick;
			((Component)(object)m_DelayTimer).Dispose();
			m_DelayTimer = null;
		}
	}

	private void DelayTimerTick(object sender, EventArgs e)
	{
		m_DelayTimer.Enabled = false;
		DestroyDelayTimer();
		if (m_Enabled)
		{
			if (m_ShowBalloonOnFocus && m_FocusedControl != null)
			{
				ShowBalloon(m_FocusedControl);
			}
			else if (!m_ShowBalloonOnFocus && m_MouseOverControl != null)
			{
				ShowBalloon(m_MouseOverControl);
			}
		}
	}

	private void ShowBalloonDelayed(Control control)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		if (control == null || !m_Enabled)
		{
			return;
		}
		if (!m_ShowAlways)
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
		if (m_InitialDelay == 0)
		{
			ShowBalloon(control);
		}
		else if (m_DelayTimer == null)
		{
			m_DelayTimer = new Timer();
			m_DelayTimer.Tick += DelayTimerTick;
			m_DelayTimer.Interval = m_InitialDelay;
			m_DelayTimer.Start();
		}
	}

	private void OnMouseEnter()
	{
		if (m_Enabled && m_MouseOverControl != null && !m_ShowBalloonOnFocus && m_BalloonsInfo.Contains(m_MouseOverControl))
		{
			ShowBalloonDelayed(m_MouseOverControl);
		}
	}

	private void OnMouseLeave()
	{
		if (!m_ShowBalloonOnFocus)
		{
			DestroyDelayTimer();
			CloseBalloon();
		}
	}

	private void OnControlGotFocus()
	{
		if (m_Enabled && m_FocusedControl != null && m_ShowBalloonOnFocus && m_BalloonsInfo.Contains(m_FocusedControl))
		{
			ShowBalloonDelayed(m_FocusedControl);
		}
	}

	private void OnControlLeave()
	{
		if (m_ShowBalloonOnFocus)
		{
			DestroyDelayTimer();
			CloseBalloon();
		}
	}

	private void ControlMouseEnter(object sender, EventArgs e)
	{
		m_MouseOverControl = (Control)((sender is Control) ? sender : null);
		OnMouseEnter();
	}

	private void ControlMouseLeave(object sender, EventArgs e)
	{
		m_MouseOverControl = null;
		OnMouseLeave();
	}

	private void ControlGotFocus(object sender, EventArgs e)
	{
		m_FocusedControl = (Control)((sender is Control) ? sender : null);
		OnControlGotFocus();
	}

	private void ControlLeave(object sender, EventArgs e)
	{
		m_FocusedControl = null;
		OnControlLeave();
	}
}
