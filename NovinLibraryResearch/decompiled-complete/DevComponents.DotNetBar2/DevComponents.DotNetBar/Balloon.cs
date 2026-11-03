using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxItem(false)]
public class Balloon : Form
{
	private class _244A
	{
		private Rectangle ݗ = Rectangle.Empty;

		private Control ӭ;

		private bool ݡ;

		private bool ݠ;

		public Image ཊ;

		public Image _1938;

		public Image _0F78;

		public bool ग़ => ݠ;

		public bool ज़ => ݡ;

		public Rectangle ঋ
		{
			get
			{
				return ݗ;
			}
			set
			{
				ݗ = value;
			}
		}

		public Point ᙞ
		{
			get
			{
				return ݗ.Location;
			}
			set
			{
				ݗ.Location = value;
			}
		}

		public Size ᇐ
		{
			get
			{
				return ݗ.Size;
			}
			set
			{
				ݗ.Size = value;
			}
		}

		public event EventHandler દ;

		public _244A(Control parent, Rectangle bounds)
		{
			ӭ = parent;
			ݗ = bounds;
		}

		public virtual void Paint(Graphics g)
		{
			Rectangle rectangle = ݗ;
			if (ݠ)
			{
				if (_0F78 != null)
				{
					g.DrawImage(_0F78, rectangle);
				}
			}
			else if (ݡ)
			{
				if (_1938 != null)
				{
					g.DrawImage(_1938, rectangle);
				}
			}
			else if (ཊ != null)
			{
				g.DrawImage(ཊ, rectangle);
			}
		}

		public virtual void OnMouseMove(MouseEventArgs e)
		{
			if (ݠ)
			{
				return;
			}
			if (ݗ.Contains(e.X, e.Y))
			{
				if (!ݡ)
				{
					ݡ = true;
					if (ӭ != null)
					{
						ӭ.Invalidate(ݗ);
					}
				}
			}
			else if (ݡ)
			{
				ݡ = false;
				if (ӭ != null)
				{
					ӭ.Invalidate(ݗ);
				}
			}
		}

		public virtual void OnMouseDown(MouseEventArgs e)
		{
			if (ݗ.Contains(e.X, e.Y) && !ݠ)
			{
				ݠ = true;
				if (ӭ != null)
				{
					ӭ.Invalidate(ݗ);
				}
			}
		}

		public virtual void OnMouseUp(MouseEventArgs e)
		{
			if (ݗ.Contains(e.X, e.Y) && ݠ)
			{
				ݠ = false;
				if (ӭ != null)
				{
					ӭ.Invalidate(ݗ);
				}
				if (this.દ != null)
				{
					this.દ(this, new EventArgs());
				}
			}
			else if (ݡ || ݠ)
			{
				ݠ = false;
				ݡ = false;
				if (ӭ != null)
				{
					ӭ.Invalidate(ݗ);
				}
			}
		}

		public virtual void OnMouseLeave(EventArgs e)
		{
			if (ݡ && !ݠ)
			{
				ݡ = false;
				if (ӭ != null)
				{
					ӭ.Invalidate(ݗ);
				}
			}
		}
	}

	private const int _241D = 4;

	private const int _241E = 4;

	private const int _241F = 1;

	private const int _2420 = 1;

	private const int ग = 33;

	private const int घ = 3;

	private IContainer Պ;

	private CustomPaintEventHandler ᯓ;

	private CustomPaintEventHandler _2421;

	private CustomPaintEventHandler _0BD3;

	private CustomPaintEventHandler _2422;

	private EventHandler _20DE;

	private EventHandler _2423;

	private int _2424 = 16;

	private int ன = 8;

	private int _2425 = 32;

	private eTipPosition _2426;

	private Color ތ = Color.Empty;

	private int ގ = 90;

	private eBackgroundImagePosition ސ;

	private int ޑ = 255;

	private Color _07A8 = SystemColors.InfoText;

	private bool _2427 = true;

	private _244A _2428;

	private eBallonStyle Ӷ;

	private Image _2429;

	private Image _242A;

	private Image _242B;

	private Image _242C;

	private Icon _242D;

	private Font ൾ;

	private string _242E = "";

	private StringAlignment _242F;

	private Color _2430 = SystemColors.InfoText;

	private StringAlignment ޔ;

	private StringAlignment ޕ = (StringAlignment)1;

	private Rectangle _2431 = Rectangle.Empty;

	private Rectangle _2432 = Rectangle.Empty;

	private Rectangle _2433 = Rectangle.Empty;

	private bool _2434 = true;

	private int _2435;

	private Timer ᆪ;

	private Color _2436 = Color.FromArgb(241, 239, 226);

	private Color _07AA = Color.White;

	private Color _2437 = Color.FromArgb(236, 233, 216);

	private Color _2438 = Color.FromArgb(172, 168, 153);

	private Color _2439 = Color.FromArgb(113, 111, 100);

	private eAlertAnimation _243A = eAlertAnimation.BottomToTop;

	private int _243B = 200;

	private bool ஞ;

	private bool ե = true;

	private int __243C = 180;

	private _0CB4 __1A56;

	private MarkupLinkClickEventHandler Ӟ;

	private _0CB4 __243D;

	[Category("Appearance")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			ե = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(180)]
	[Category("Appearance")]
	[Description("Indicates minimum balloon width when auto sizing balloon.")]
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

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates the target gradient background color.")]
	public Color BackColor2
	{
		get
		{
			return ތ;
		}
		set
		{
			ތ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates gradient fill angle.")]
	[DefaultValue(90)]
	public int BackColorGradientAngle
	{
		get
		{
			return ގ;
		}
		set
		{
			if (value != ގ)
			{
				ގ = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(255)]
	[Description("Specifies the transparency of background image.")]
	[Browsable(true)]
	[Category("Appearance")]
	public int BackgroundImageAlpha
	{
		get
		{
			return ޑ;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			else if (value > 255)
			{
				value = 255;
			}
			if (ޑ != value)
			{
				ޑ = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DefaultValue(eBackgroundImagePosition.Stretch)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Specifies background image position when container is larger than image.")]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return ސ;
		}
		set
		{
			if (ސ != value)
			{
				ސ = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates border color.")]
	public Color BorderColor
	{
		get
		{
			return _07A8;
		}
		set
		{
			_07A8 = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(eBallonStyle.Balloon)]
	[Description("Specifies balloon style.")]
	[Browsable(true)]
	public eBallonStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			Ӷ = value;
			if (Ӷ == eBallonStyle.Alert)
			{
				((Control)this).BackColor = Color.FromArgb(207, 221, 244);
				BackColor2 = Color.White;
				((Control)this).ForeColor = Color.FromArgb(102, 114, 196);
				ޔ = (StringAlignment)1;
			}
			else if (Ӷ == eBallonStyle.Office2007Alert)
			{
				Office2007ColorTable office2007ColorTable = ጪ();
				ColorScheme legacyColors = office2007ColorTable.LegacyColors;
				if (office2007ColorTable.SuperTooltip.BackgroundColors.IsEmpty)
				{
					((Control)this).BackColor = legacyColors.PanelBackground;
					BackColor2 = legacyColors.PanelBackground2;
					((Control)this).ForeColor = legacyColors.PanelText;
				}
				else
				{
					((Control)this).BackColor = office2007ColorTable.SuperTooltip.BackgroundColors.Start;
					BackColor2 = office2007ColorTable.SuperTooltip.BackgroundColors.End;
					((Control)this).ForeColor = office2007ColorTable.SuperTooltip.TextColor;
				}
				_07A8 = legacyColors.PanelBorder;
				ޔ = (StringAlignment)0;
			}
			else
			{
				ޔ = (StringAlignment)0;
				_07A8 = SystemColors.InfoText;
			}
			if (_2427)
			{
				_2444();
			}
			RecalcLayout();
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Description("Indicates whether the Close button is displayed.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Browsable(true)]
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
				_2443();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Description("Gets or sets the animation type used to display Alert type balloon.")]
	[Browsable(true)]
	[DefaultValue(eAlertAnimation.BottomToTop)]
	[Category("Behavior")]
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

	[Browsable(true)]
	[DefaultValue(200)]
	[Description("Gets or sets the total time in milliseconds alert animation takes.")]
	[Category("Behavior")]
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

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether balloon will close automatically when user click the close button.")]
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

	[DefaultValue(0)]
	[Description("Indicates time period in seconds after balloon closes automatically.")]
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
			OnAutoCloseTimeOutChanged();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates custom image for Close Button.")]
	public Image CloseButtonNormal
	{
		get
		{
			return _2429;
		}
		set
		{
			if (_2429 != value)
			{
				_2429 = value;
				_2443();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates custom image for Close Button when mouse is over the button.")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Image CloseButtonHot
	{
		get
		{
			return _242A;
		}
		set
		{
			if (_242A != value)
			{
				_242A = value;
				_2443();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates custom image for Close Button when button is pressed.")]
	[DefaultValue(null)]
	public Image CloseButtonPressed
	{
		get
		{
			return _242B;
		}
		set
		{
			if (_242B != value)
			{
				_242B = value;
				_2443();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates Caption image.")]
	[Category("Appearance")]
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
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates Caption icon. Icon is used to provide support for alpha-blended images in caption.")]
	[DefaultValue(null)]
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
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	private ᇎ _241C
	{
		get
		{
			if (_242D != null)
			{
				return new ᇎ(_242D, dispose: false);
			}
			if (_242C != null)
			{
				return new ᇎ(_242C, dispose: false);
			}
			return null;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates Caption font.")]
	public Font CaptionFont
	{
		get
		{
			return ൾ;
		}
		set
		{
			if (ൾ != value)
			{
				ൾ = value;
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DefaultValue("")]
	[Localizable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates text displayed in caption.")]
	public string CaptionText
	{
		get
		{
			return _242E;
		}
		set
		{
			if (_242E != value)
			{
				_242E = value;
				if (__243D != null)
				{
					__243D.ભ((Control)(object)this);
					__243D.ವ -= ᑶ;
					__243D = null;
				}
				if (ሒ.መ(ref _242E))
				{
					__243D = ሒ.ሕ(_242E);
				}
				if (__243D != null)
				{
					__243D.ವ += ᑶ;
				}
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Description("Indicates color of caption text")]
	[Browsable(true)]
	[Category("Appearance")]
	public Color CaptionColor
	{
		get
		{
			return _2430;
		}
		set
		{
			if (_2430 != value)
			{
				_2430 = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates position of the balloon tip.")]
	[Category("Appearance")]
	[DefaultValue(eTipPosition.Top)]
	public eTipPosition TipPosition
	{
		get
		{
			return _2426;
		}
		set
		{
			if (_2426 != value)
			{
				_2426 = value;
				RecalcLayout();
				if (_2423 != null)
				{
					_2423(this, new EventArgs());
				}
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DefaultValue(32)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates tip distance from the edge of the balloon.")]
	public int TipOffset
	{
		get
		{
			return _2425;
		}
		set
		{
			if (_2425 != value)
			{
				if (value < ன)
				{
					value = ன + 1;
				}
				else if ((_2426 == eTipPosition.Top || _2426 == eTipPosition.Bottom) && value > ((Control)this).Width - ன - _2424)
				{
					value = ((Control)this).Width - ன - _2424;
				}
				else if ((_2426 == eTipPosition.Left || _2426 == eTipPosition.Right) && value > ((Control)this).Height - ன - _2424)
				{
					value = ((Control)this).Height - ன - _2424;
				}
				_2425 = value;
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(false)]
	[Description("Returns tip length")]
	[Category("Appearance")]
	public int TipLength => _2424;

	[DevCoBrowsable(true)]
	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			if (value)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}
	}

	public event CustomPaintEventHandler PaintBackground
	{
		add
		{
			CustomPaintEventHandler customPaintEventHandler = ᯓ;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Combine(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref ᯓ, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
		remove
		{
			CustomPaintEventHandler customPaintEventHandler = ᯓ;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Remove(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref ᯓ, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
	}

	public event CustomPaintEventHandler PaintCaptionImage
	{
		add
		{
			CustomPaintEventHandler customPaintEventHandler = _2421;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Combine(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref _2421, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
		remove
		{
			CustomPaintEventHandler customPaintEventHandler = _2421;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Remove(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref _2421, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
	}

	public event CustomPaintEventHandler PaintCaptionText
	{
		add
		{
			CustomPaintEventHandler customPaintEventHandler = _0BD3;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Combine(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref _0BD3, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
		remove
		{
			CustomPaintEventHandler customPaintEventHandler = _0BD3;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Remove(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref _0BD3, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
	}

	public event CustomPaintEventHandler PaintText
	{
		add
		{
			CustomPaintEventHandler customPaintEventHandler = _2422;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Combine(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref _2422, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
		remove
		{
			CustomPaintEventHandler customPaintEventHandler = _2422;
			CustomPaintEventHandler customPaintEventHandler2;
			do
			{
				customPaintEventHandler2 = customPaintEventHandler;
				CustomPaintEventHandler value2 = (CustomPaintEventHandler)Delegate.Remove(customPaintEventHandler2, value);
				customPaintEventHandler = Interlocked.CompareExchange(ref _2422, value2, customPaintEventHandler2);
			}
			while ((object)customPaintEventHandler != customPaintEventHandler2);
		}
	}

	public event EventHandler CloseButtonClick
	{
		add
		{
			EventHandler eventHandler = _20DE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20DE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _20DE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20DE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler TipPositionChanged
	{
		add
		{
			EventHandler eventHandler = _2423;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2423, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2423;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2423, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when text markup link is clicked. Markup links can be created using <a> tag.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public Balloon()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		_2444();
		ـ();
		if (Պ == null)
		{
			Պ = new Container();
		}
		ൾ = new Font(((Control)this).Font, (FontStyle)1);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)1, true);
	}

	protected override void Dispose(bool disposing)
	{
		_2286();
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.ᆟ(ref _242D);
			BarUtilities.ᆟ(ref _242C);
			BarUtilities.ᆟ(ref _242A);
			BarUtilities.ᆟ(ref _2429);
			BarUtilities.ᆟ(ref _242B);
		}
		((Form)this).Dispose(disposing);
	}

	private void ـ()
	{
		((Control)this).AccessibleRole = (AccessibleRole)31;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackColor = SystemColors.Info;
		((Form)this).ClientSize = new Size(192, 80);
		((Form)this).ControlBox = false;
		((Control)this).ForeColor = SystemColors.InfoText;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "Balloon";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
	}

	protected override void OnClick(EventArgs e)
	{
		if (!_2427 || !_2428.ঋ.Contains(((Control)this).PointToClient(Control.MousePosition)))
		{
			((Control)this).OnClick(e);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		if (ե)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)2;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		if (Ӷ == eBallonStyle.Balloon)
		{
			_243F(e.Graphics);
		}
		else
		{
			_243E(e.Graphics);
		}
		e.Graphics.SmoothingMode = smoothingMode;
		e.Graphics.TextRenderingHint = textRenderingHint;
	}

	private void _243E(Graphics _٥)
	{
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Expected O, but got Unknown
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Invalid comparison between Unknown and I4
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Invalid comparison between Unknown and I4
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Invalid comparison between Unknown and I4
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Invalid comparison between Unknown and I4
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Invalid comparison between Unknown and I4
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Invalid comparison between Unknown and I4
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Invalid comparison between Unknown and I4
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Invalid comparison between Unknown and I4
		Rectangle rectangle = _2440();
		if (ᯓ != null)
		{
			ᯓ(this, new CustomPaintEventArgs(_٥, rectangle));
		}
		else
		{
			_2442(_٥);
			if (Ӷ == eBallonStyle.Alert)
			{
				Rectangle rectangle2 = rectangle;
				rectangle2.Height = Math.Max(_2431.Bottom, _2432.Bottom) - rectangle2.Top;
				rectangle2.Height++;
				if (rectangle2.Width > 0 && rectangle2.Height > 0)
				{
					LinearGradientBrush val = BarFunctions.ॴ(rectangle2, ((Control)this).BackColor, ތ, ގ);
					try
					{
						Blend val2 = new Blend(4);
						val2.Positions = new float[4] { 0f, 0.5f, 0.5f, 1f };
						val2.Factors = new float[4] { 0f, 1f, 1f, 0f };
						val.Blend = val2;
						_٥.FillRectangle((Brush)(object)val, rectangle2);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		if (Ӷ == eBallonStyle.Office2007Alert)
		{
			DisplayHelp.DrawRectangle(_٥, _07A8, displayRectangle);
		}
		else
		{
			displayRectangle.Width--;
			displayRectangle.Height--;
			Pen val3 = new Pen(_2436, 1f);
			try
			{
				_٥.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				_٥.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.X, displayRectangle.Bottom);
				_٥.DrawLine(val3, displayRectangle.X + 4, displayRectangle.Bottom - 4, displayRectangle.Right - 4, displayRectangle.Bottom - 4);
				_٥.DrawLine(val3, displayRectangle.Right - 4, displayRectangle.Y + 4, displayRectangle.Right - 4, displayRectangle.Bottom - 4);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Pen val4 = new Pen(_07AA, 1f);
			try
			{
				_٥.DrawLine(val4, displayRectangle.X + 1, displayRectangle.Y + 1, displayRectangle.Right - 1, displayRectangle.Y + 1);
				_٥.DrawLine(val4, displayRectangle.X + 1, displayRectangle.Y + 1, displayRectangle.X + 1, displayRectangle.Bottom - 1);
				_٥.DrawLine(val4, displayRectangle.X + 3, displayRectangle.Bottom - 3, displayRectangle.Right - 3, displayRectangle.Bottom - 3);
				_٥.DrawLine(val4, displayRectangle.Right - 3, displayRectangle.Y + 3, displayRectangle.Right - 3, displayRectangle.Bottom - 3);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			Pen val5 = new Pen(_2437, 1f);
			try
			{
				_٥.DrawLine(val5, displayRectangle.X + 2, displayRectangle.Y + 2, displayRectangle.Right - 2, displayRectangle.Y + 2);
				_٥.DrawLine(val5, displayRectangle.X + 2, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Bottom - 2);
				_٥.DrawLine(val5, displayRectangle.X + 2, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom - 2);
				_٥.DrawLine(val5, displayRectangle.Right - 2, displayRectangle.Y + 2, displayRectangle.Right - 2, displayRectangle.Bottom - 2);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			Pen val6 = new Pen(_2438, 1f);
			try
			{
				_٥.DrawLine(val6, displayRectangle.X + 3, displayRectangle.Y + 3, displayRectangle.Right - 3, displayRectangle.Y + 3);
				_٥.DrawLine(val6, displayRectangle.X + 3, displayRectangle.Y + 3, displayRectangle.X + 3, displayRectangle.Bottom - 3);
				_٥.DrawLine(val6, displayRectangle.X + 1, displayRectangle.Bottom - 1, displayRectangle.Right - 1, displayRectangle.Bottom - 1);
				_٥.DrawLine(val6, displayRectangle.Right - 1, displayRectangle.Y + 1, displayRectangle.Right - 1, displayRectangle.Bottom - 1);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			Pen val7 = new Pen(_2439, 1f);
			try
			{
				_٥.DrawLine(val7, displayRectangle.X + 4, displayRectangle.Y + 4, displayRectangle.Right - 4, displayRectangle.Y + 4);
				_٥.DrawLine(val7, displayRectangle.X + 4, displayRectangle.Y + 4, displayRectangle.X + 4, displayRectangle.Bottom - 4);
				_٥.DrawLine(val7, displayRectangle.X, displayRectangle.Bottom, displayRectangle.Right, displayRectangle.Bottom);
				_٥.DrawLine(val7, displayRectangle.Right, displayRectangle.Y, displayRectangle.Right, displayRectangle.Bottom);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (_2428 != null)
		{
			_2428.Paint(_٥);
		}
		if (_2421 != null)
		{
			_2421(this, new CustomPaintEventArgs(_٥, _2431));
		}
		else
		{
			ᇎ ᇎ2 = _241C;
			if (ᇎ2 != null && !_2431.IsEmpty)
			{
				ᇎ2.ᇗ(_٥, _2431);
			}
		}
		if (_0BD3 != null)
		{
			_0BD3(this, new CustomPaintEventArgs(_٥, _2432));
		}
		else if (_242E != "" && !_2432.IsEmpty)
		{
			eTextFormat eTextFormat2 = eTextFormat.WordBreak;
			if ((int)_242F == 1)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if ((int)_242F == 2)
			{
				eTextFormat2 |= eTextFormat.Right;
			}
			Font font = ൾ;
			if (font == null)
			{
				font = ((Control)this).Font;
			}
			if (__243D != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(_٥, font, _2430, (int)((Control)this).RightToLeft == 1, displayRectangle, hotKeyPrefixVisible: true);
				__243D._0CC6(_2432, markupDrawContext);
				__243D.Render(markupDrawContext);
			}
			else
			{
				TextDrawing.DrawString(_٥, _242E, font, _2430, _2432, eTextFormat2);
			}
		}
		if (_2422 != null)
		{
			_2422(this, new CustomPaintEventArgs(_٥, _2433));
		}
		else if (((Control)this).Text != "")
		{
			eTextFormat eTextFormat3 = eTextFormat.WordBreak;
			if ((int)ޔ == 1)
			{
				eTextFormat3 |= eTextFormat.HorizontalCenter;
			}
			else if ((int)ޔ == 2)
			{
				eTextFormat3 |= eTextFormat.Right;
			}
			if ((int)ޕ == 1)
			{
				eTextFormat3 |= eTextFormat.VerticalCenter;
			}
			else if ((int)ޕ == 2)
			{
				eTextFormat3 |= eTextFormat.Bottom;
			}
			if (__1A56 != null)
			{
				MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(_٥, ((Control)this).Font, ((Control)this).ForeColor, (int)((Control)this).RightToLeft == 1, displayRectangle, hotKeyPrefixVisible: true);
				__1A56._0CC6(_2433, markupDrawContext2);
				__1A56.Render(markupDrawContext2);
			}
			else
			{
				TextDrawing.DrawString(_٥, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, _2433, eTextFormat3);
			}
		}
	}

	private void _243F(Graphics _٥)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Invalid comparison between Unknown and I4
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Invalid comparison between Unknown and I4
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		GraphicsPath val = _2441(rectangle);
		if (ᯓ != null)
		{
			ᯓ(this, new CustomPaintEventArgs(_٥, rectangle));
		}
		else
		{
			_2442(_٥);
		}
		Pen val2 = new Pen(_07A8, 1f);
		try
		{
			val2.Alignment = (PenAlignment)1;
			_٥.DrawPath(val2, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (_2428 != null)
		{
			_2428.Paint(_٥);
		}
		if (_2421 != null)
		{
			_2421(this, new CustomPaintEventArgs(_٥, _2431));
		}
		else
		{
			ᇎ ᇎ2 = _241C;
			if (ᇎ2 != null && !_2431.IsEmpty)
			{
				ᇎ2.ᇗ(_٥, _2431);
			}
		}
		if (_0BD3 != null)
		{
			_0BD3(this, new CustomPaintEventArgs(_٥, _2432));
		}
		else if (_242E != "" && !_2432.IsEmpty)
		{
			eTextFormat format = TextDrawing.TranslateHorizontal(_242F) | eTextFormat.WordBreak;
			Font font = ൾ;
			if (font == null)
			{
				font = ((Control)this).Font;
			}
			if (__243D != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(_٥, font, _2430, (int)((Control)this).RightToLeft == 1, rectangle, hotKeyPrefixVisible: true);
				__243D._0CC6(_2432, markupDrawContext);
				__243D.Render(markupDrawContext);
			}
			else
			{
				TextDrawing.DrawString(_٥, _242E, font, _2430, _2432, format);
			}
		}
		if (_2422 != null)
		{
			_2422(this, new CustomPaintEventArgs(_٥, _2433));
		}
		else if (((Control)this).Text != "")
		{
			eTextFormat format2 = TextDrawing.TranslateHorizontal(ޔ) | TextDrawing.TranslateVertical(ޕ) | eTextFormat.WordBreak;
			if (__1A56 != null)
			{
				MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(_٥, ((Control)this).Font, ((Control)this).ForeColor, (int)((Control)this).RightToLeft == 1, rectangle, hotKeyPrefixVisible: true);
				__1A56._0CC6(_2433, markupDrawContext2);
				__1A56.Render(markupDrawContext2);
			}
			else
			{
				TextDrawing.DrawString(_٥, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, _2433, format2);
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Form)this).OnResize(e);
		if (!ஞ)
		{
			RecalcLayout();
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		string _06EA = ((Control)this).Text;
		if (__1A56 != null)
		{
			__1A56.ભ((Control)(object)this);
			__1A56.ವ -= ᑶ;
			__1A56 = null;
		}
		if (ሒ.መ(ref _06EA))
		{
			__1A56 = ሒ.ሕ(_06EA);
		}
		if (__1A56 != null)
		{
			__1A56.ವ += ᑶ;
		}
		((Control)this).Invalidate();
		((Form)this).OnTextChanged(e);
	}

	private void ᑶ(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	protected virtual Size ResizeMarkup(Graphics g, int containerWidth)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if (__1A56 != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, containerWidth, 1);
			MarkupDrawContext d = new MarkupDrawContext(g, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			__1A56.Measure(rectangle.Size, d);
			return __1A56.Bounds.Size;
		}
		return Size.Empty;
	}

	protected virtual Size ResizeCaptionMarkup(Graphics g, int containerWidth)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if (__243D != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, containerWidth, 1);
			MarkupDrawContext d = new MarkupDrawContext(g, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			__243D.Measure(rectangle.Size, d);
			return __243D.Bounds.Size;
		}
		return Size.Empty;
	}

	public void AutoResize()
	{
		Size size = new Size(Dpi.Width(__243C), 0);
		Size empty = Size.Empty;
		Font font = ൾ;
		if (font == null)
		{
			font = ((Control)this).Font;
		}
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			empty = ((__243D == null) ? TextDrawing.MeasureString(val, (_242E != "") ? _242E : " ", font) : ResizeCaptionMarkup(val, 800));
			size.Width = Math.Max(size.Width, empty.Width);
			if (_2428 != null)
			{
				size.Width += Dpi.Width(_2428.ᇐ.Width);
			}
			else
			{
				size.Width += Dpi.Width18;
			}
			size.Width += 4;
			int val2 = 0;
			if (_242D != null)
			{
				size.Width += Dpi.Width(_242D.Width + 4);
				val2 = Dpi.Height(_242D.Height);
			}
			else if (_242C != null)
			{
				size.Width += Dpi.Width(_242C.Width + 4);
				val2 = Dpi.Height(_242C.Height);
			}
			size.Height += Math.Max(val2, empty.Height);
			size.Height += 8;
			empty = ((__1A56 != null) ? ResizeMarkup(val, size.Width) : TextDrawing.MeasureString(val, (((Control)this).Text != "") ? ((Control)this).Text : " ", ((Control)this).Font, size.Width, eTextFormat.WordBreak));
			size.Height += empty.Height;
			size.Width += ன * 2;
			size.Height += ன * 2;
			if (Ӷ == eBallonStyle.Balloon)
			{
				if (_2426 == eTipPosition.Left || _2426 == eTipPosition.Right)
				{
					size.Width += Dpi.Width(_2424);
				}
				else
				{
					size.Height += Dpi.Height(_2424);
				}
			}
			else
			{
				size.Height += (int)((double)(float)size.Height * 0.3);
			}
		}
		finally
		{
			val.Dispose();
		}
		((Form)this).Size = size;
	}

	public void RecalcLayout()
	{
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Invalid comparison between Unknown and I4
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Invalid comparison between Unknown and I4
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Invalid comparison between Unknown and I4
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Invalid comparison between Unknown and I4
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Expected O, but got Unknown
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Expected O, but got Unknown
		if (((Control)this).DisplayRectangle.Width == 0 || ((Control)this).DisplayRectangle.Height == 0)
		{
			return;
		}
		Size size = Size.Empty;
		if (_2428 != null)
		{
			size = _2428.ᇐ;
		}
		if (Ӷ == eBallonStyle.Balloon)
		{
			Rectangle rectangle = new Rectangle(ன / 2, ன / 2, ((Control)this).Width - ன, ((Control)this).Height - ன);
			if (_2426 == eTipPosition.Bottom)
			{
				rectangle.Height -= _2424;
			}
			else if (_2426 == eTipPosition.Left)
			{
				rectangle.X += _2424;
				rectangle.Width -= _2424;
			}
			else if (_2426 == eTipPosition.Right)
			{
				rectangle.Width -= _2424;
			}
			else
			{
				rectangle.Y += _2424;
				rectangle.Height -= _2424;
			}
			Size size2 = Size.Empty;
			if (_242D != null)
			{
				size2 = Dpi.ImageSize(_242D.Size);
			}
			else if (_242C != null)
			{
				size2 = Dpi.ImageSize(_242C.Size);
			}
			_2432 = rectangle;
			if (!size2.IsEmpty)
			{
				_2432.X += size2.Width + 4;
				_2432.Width -= size2.Width + 4;
			}
			if (_2428 != null)
			{
				_2428.ᙞ = new Point(rectangle.Right - size.Width, rectangle.Top);
				_2432.Width -= size.Width + 4;
			}
			Font font = ൾ;
			if (font == null)
			{
				font = ((Control)this).Font;
			}
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				eTextFormat format = eTextFormat.WordBreak;
				if ((int)_242F == 1)
				{
					format = eTextFormat.HorizontalCenter;
				}
				else if ((int)_242F == 2)
				{
					format = eTextFormat.Right;
				}
				Size empty = Size.Empty;
				empty = ((__243D == null) ? TextDrawing.MeasureString(val, (_242E != "") ? _242E : " ", font, _2432.Width, format) : ResizeCaptionMarkup(val, _2432.Width));
				_2432.Height = empty.Height;
			}
			finally
			{
				val.Dispose();
			}
			if (!size2.IsEmpty)
			{
				_2431 = new Rectangle(rectangle.X, rectangle.Y, size2.Width, size2.Height);
				if (_2432.Height > size2.Height)
				{
					_2431.Y += (_2432.Height - size2.Height) / 2;
				}
				else if (size2.Height > _2432.Height)
				{
					_2432.Y += (size2.Height - _2432.Height) / 2;
				}
			}
			else if (_2428 != null && size.Height > _2432.Height)
			{
				_2432.Y += (size.Height - _2432.Height) / 2;
			}
			if (_2432.Bottom > _2431.Bottom)
			{
				_2433 = new Rectangle(rectangle.X, _2432.Bottom + 1, rectangle.Width, rectangle.Bottom - (_2432.Bottom + 1));
			}
			else
			{
				_2433 = new Rectangle(rectangle.X, _2431.Bottom + 1, rectangle.Width, rectangle.Bottom - (_2431.Bottom + 1));
			}
			GraphicsPath val2 = _2441(((Control)this).DisplayRectangle);
			GraphicsPath val3 = (GraphicsPath)val2.Clone();
			val3.Widen(Pens.Black);
			Region val4 = new Region(val3);
			val4.Union(val2);
			((Control)this).Region = val4;
			val2.Dispose();
			val3.Dispose();
			return;
		}
		Rectangle rectangle2 = _2440();
		Size size3 = Size.Empty;
		if (_242D != null)
		{
			size3 = Dpi.ImageSize(_242D.Size);
		}
		else if (_242C != null)
		{
			size3 = Dpi.ImageSize(_242C.Size);
		}
		_2432 = rectangle2;
		if (!size3.IsEmpty)
		{
			_2432.X += size3.Width + 8;
			_2432.Width -= size3.Width + 8;
		}
		if (_2428 != null)
		{
			_2428.ᙞ = new Point(rectangle2.Right - _2428.ঋ.Width - 1, rectangle2.Top);
			_2432.Width -= size.Width + 4 + 1;
		}
		Font font2 = ൾ;
		if (font2 == null)
		{
			font2 = ((Control)this).Font;
		}
		Graphics val5 = ((Control)this).CreateGraphics();
		try
		{
			eTextFormat format2 = eTextFormat.WordBreak;
			if ((int)_242F == 1)
			{
				format2 = eTextFormat.HorizontalCenter;
			}
			else if ((int)_242F == 2)
			{
				format2 = eTextFormat.Right;
			}
			Size size4 = TextDrawing.MeasureString(val5, (_242E != "") ? _242E : " ", font2, _2432.Width, format2);
			_2432.Height = size4.Height;
		}
		finally
		{
			val5.Dispose();
		}
		if (!size3.IsEmpty)
		{
			_2431 = new Rectangle(rectangle2.X + 4, rectangle2.Y, size3.Width, size3.Height);
			if (_2432.Height > size3.Height)
			{
				_2431.Y += (_2432.Height - size3.Height) / 2;
			}
			else if (size3.Height > _2432.Height)
			{
				_2432.Y += (size3.Height + -_2432.Height) / 2;
			}
		}
		else if (_2428 != null && size.Height > _2432.Height)
		{
			_2432.Y += (size.Height - _2432.Height) / 2;
		}
		_2432.Offset(0, 1);
		_2431.Offset(0, 1);
		if (_2428 != null)
		{
			_2428.ᙞ = new Point(_2428.ᙞ.X, _2432.Y + (_2432.Height - size.Height) / 2);
		}
		if (_2432.Bottom > _2431.Bottom)
		{
			_2433 = new Rectangle(rectangle2.X, _2432.Bottom + 4, rectangle2.Width, rectangle2.Bottom - (_2432.Bottom + 4));
		}
		else
		{
			_2433 = new Rectangle(rectangle2.X, _2431.Bottom + 4, rectangle2.Width, rectangle2.Bottom - (_2431.Bottom + 4));
		}
		((Control)this).Region = new Region(((Control)this).DisplayRectangle);
	}

	private Rectangle _2440()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.Inflate(-4, -4);
		return result;
	}

	private GraphicsPath _2441(Rectangle भ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Invalid comparison between Unknown and I4
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = भ;
		int num = ன * 2;
		Matrix val2 = new Matrix();
		Point[] array = new Point[3];
		int num2 = _2425;
		rectangle.Size = new Size(rectangle.Width - 1, rectangle.Height - 1);
		bool flag = false;
		if (_2426 == eTipPosition.Bottom)
		{
			flag = true;
			val2.Translate((float)rectangle.Width, (float)rectangle.Height);
			val2.Rotate(180f);
		}
		else if ((_2426 == eTipPosition.Left && (int)((Control)this).RightToLeft == 0) || (_2426 == eTipPosition.Right && (int)((Control)this).RightToLeft == 1))
		{
			flag = true;
			rectangle.Size = new Size(rectangle.Height, rectangle.Width);
			val2.Translate(0f, (float)rectangle.Width);
			val2.Rotate(-90f);
		}
		else if ((_2426 == eTipPosition.Right && (int)((Control)this).RightToLeft == 0) || (_2426 == eTipPosition.Left && (int)((Control)this).RightToLeft == 1))
		{
			rectangle.Size = new Size(rectangle.Height, rectangle.Width);
			val2.Translate((float)rectangle.Height, 0f);
			val2.Rotate(90f);
		}
		int num3 = (rectangle.Width - ன * 2) / 2 + 1;
		int num4 = ன;
		if (flag)
		{
			num2 = rectangle.Width - num4 * 2 - _2425 + 1;
		}
		if (num2 < num3)
		{
			ref Point reference = ref array[0];
			reference = new Point(num2 + num4, rectangle.Y + _2424);
			ref Point reference2 = ref array[1];
			reference2 = new Point(num2 + num4, rectangle.Y);
			ref Point reference3 = ref array[2];
			reference3 = new Point(num2 + _2424 + num4, rectangle.Y + _2424);
		}
		else if (num2 > num3)
		{
			ref Point reference4 = ref array[0];
			reference4 = new Point(num2 - _2424 + num4, rectangle.Y + _2424);
			ref Point reference5 = ref array[1];
			reference5 = new Point(num2 + num4, rectangle.Y);
			ref Point reference6 = ref array[2];
			reference6 = new Point(num2 + num4, rectangle.Y + _2424);
		}
		else
		{
			ref Point reference7 = ref array[0];
			reference7 = new Point(num2 - _2424 + num4, rectangle.Y + _2424);
			ref Point reference8 = ref array[1];
			reference8 = new Point(num2 + num4, rectangle.Y);
			ref Point reference9 = ref array[2];
			reference9 = new Point(num2 + _2424 + num4, rectangle.Y + _2424);
		}
		val.AddArc(rectangle.Left, rectangle.Top + _2424, num, num, 180f, 90f);
		val.AddLine(array[0], array[1]);
		val.AddLine(array[1], array[2]);
		val.AddArc(rectangle.Width - num, rectangle.Top + _2424, num, num, -90f, 90f);
		val.AddArc(rectangle.Width - num, rectangle.Bottom - num, num, num, 0f, 90f);
		val.AddArc(rectangle.Left, rectangle.Bottom - num, num, num, 90f, 90f);
		val.CloseFigure();
		val.Transform(val2);
		return val;
	}

	private void _2442(Graphics _٥)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (clientRectangle.Width == 0 || clientRectangle.Height == 0)
		{
			return;
		}
		if (ތ.IsEmpty)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				_٥.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			LinearGradientBrush val2 = BarFunctions.ॴ(clientRectangle, ((Control)this).BackColor, ތ, ގ);
			try
			{
				if (Ӷ == eBallonStyle.Alert)
				{
					Blend val3 = new Blend(4);
					val3.Positions = new float[8] { 0f, 0.25f, 0.4f, 0.5f, 0.5f, 0.85f, 0.85f, 1f };
					val3.Factors = new float[8] { 0f, 1f, 1f, 0f, 0f, 0f, 0f, 1f };
					val2.Blend = val3;
				}
				_٥.FillRectangle((Brush)(object)val2, clientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (((Control)this).BackgroundImage != null)
		{
			BarFunctions._0982(_٥, clientRectangle, ((Control)this).BackgroundImage, ސ, ޑ);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBackColor2()
	{
		return !ތ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		ތ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBorderColor()
	{
		return _07A8 != SystemColors.InfoText;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		_07A8 = SystemColors.InfoText;
	}

	private Office2007ColorTable ጪ()
	{
		if (GlobalManager.Renderer is Office2007Renderer office2007Renderer)
		{
			return office2007Renderer.ColorTable;
		}
		return new Office2007ColorTable();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		if (_2428 != null)
		{
			_2428.OnMouseDown(e);
		}
		if (((Control)this).Focused)
		{
			((Control)this).Focus();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (_2428 != null)
		{
			_2428.OnMouseMove(e);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (_2428 != null)
		{
			_2428.OnMouseUp(e);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (_2428 != null)
		{
			_2428.OnMouseLeave(e);
		}
	}

	private void _2443()
	{
		if (_2427)
		{
			_2444();
			RecalcLayout();
		}
		else
		{
			_2428 = null;
		}
	}

	private void _2444()
	{
		if (_2429 != null)
		{
			_2428 = new _244A((Control)(object)this, new Rectangle(0, 0, Dpi.Width(_2429.Width), Dpi.Height(_2429.Height)));
			if (_2428.ཊ != null)
			{
				_2428.ཊ.Dispose();
				_2428.ཊ = null;
			}
			_2428.ཊ = _2429;
			if (_2428._1938 != null)
			{
				_2428._1938.Dispose();
				_2428._1938 = null;
			}
			_2428._1938 = _242A;
			if (_2428._0F78 != null)
			{
				_2428._0F78.Dispose();
				_2428._0F78 = null;
			}
			_2428._0F78 = _242B;
		}
		else if (Ӷ == eBallonStyle.Balloon)
		{
			_2428 = new _244A((Control)(object)this, new Rectangle(0, 0, Dpi.Width18, Dpi.Height18));
			_2428.ཊ = (Image)(object)BarFunctions.ই("BalloonImages.BalloonNormal.png");
			_2428._1938 = (Image)(object)BarFunctions.ই("BalloonImages.BalloonHot.png");
			_2428._0F78 = (Image)(object)BarFunctions.ই("BalloonImages.BalloonPress.png");
		}
		else
		{
			_2428 = new _244A((Control)(object)this, new Rectangle(0, 0, Dpi.Width13, Dpi.Height13));
			_2428.ཊ = (Image)(object)BarFunctions.ই("BalloonImages.AlertNormal.png");
			_2428._1938 = (Image)(object)BarFunctions.ই("BalloonImages.AlertHot.png");
			_2428._0F78 = (Image)(object)BarFunctions.ই("BalloonImages.AlertPress.png");
		}
		_2428.દ += _2445;
	}

	private void _2445(object _٠, EventArgs ؿ)
	{
		if (_20DE != null)
		{
			_20DE(this, new EventArgs());
		}
		if (_2434)
		{
			Hide();
			((Form)this).Close();
		}
	}

	protected void OnAutoCloseTimeOutChanged()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		if (_2435 > 0 && !((Component)this).DesignMode)
		{
			if (ᆪ == null)
			{
				ᆪ = new Timer(Պ);
			}
			ᆪ.Enabled = false;
			ᆪ.Interval = _2435 * 1000;
			ᆪ.Tick += AutoCloseTimeOutEllapsed;
			if (Visible)
			{
				ᆪ.Enabled = true;
				ᆪ.Start();
			}
		}
		else
		{
			_2286();
		}
	}

	protected virtual void AutoCloseTimeOutEllapsed(object sender, EventArgs e)
	{
		if (!((Control)this).IsDisposed)
		{
			_2286();
			Hide();
			((Form)this).Close();
		}
	}

	private void _2286()
	{
		if (ᆪ != null)
		{
			ᆪ.Enabled = false;
			ᆪ.Tick -= AutoCloseTimeOutEllapsed;
			Պ.Remove((IComponent?)ᆪ);
			((Component)(object)ᆪ).Dispose();
			ᆪ = null;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Form)this).OnVisibleChanged(e);
		if (Visible)
		{
			if (ᆪ != null && !ᆪ.Enabled)
			{
				ᆪ.Enabled = true;
				ᆪ.Start();
			}
		}
		else if (ᆪ != null && ᆪ.Enabled)
		{
			ᆪ.Stop();
			ᆪ.Enabled = false;
		}
	}

	private void _2446(eTipPosition ل)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0 || Style != 0)
		{
			return;
		}
		if (ل == eTipPosition.Top && _2426 == eTipPosition.Bottom)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				val.Top += _2424;
			}
			return;
		}
		if (ل != eTipPosition.Bottom || _2426 != 0)
		{
			return;
		}
		foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item2;
			val2.Top -= _2424;
		}
	}

	public void Show(Control referenceControl)
	{
		Show(referenceControl, balloonFocus: true);
	}

	public void Show(Control referenceControl, bool balloonFocus)
	{
		if (referenceControl == null)
		{
			Show();
			return;
		}
		Point empty = Point.Empty;
		empty = ((referenceControl.Parent == null) ? referenceControl.Location : referenceControl.Parent.PointToScreen(referenceControl.Location));
		Show(new Rectangle(empty, referenceControl.Size), balloonFocus);
	}

	public void Show(Rectangle referenceScreenRect, bool balloonFocus)
	{
		if (referenceScreenRect.IsEmpty)
		{
			Show();
			return;
		}
		int num = _2424 + ன * 3;
		int tipOffset = _2424 / 2 + ன * 3;
		Point empty = Point.Empty;
		ঊ ঊ2 = BarFunctions.ॹ(referenceScreenRect.Location);
		if (referenceScreenRect.X + referenceScreenRect.Width / 2 + ((Control)this).Width > ঊ2.ঌ.Right)
		{
			empty.X = referenceScreenRect.X + referenceScreenRect.Width / 2 - ((Control)this).Width + num;
			tipOffset = ((Control)this).Width - num;
		}
		else
		{
			empty.X = referenceScreenRect.X + referenceScreenRect.Width / 2 - num;
		}
		if (referenceScreenRect.Y + referenceScreenRect.Height + ((Control)this).Height > ঊ2.ঌ.Bottom)
		{
			empty.Y = referenceScreenRect.Y - ((Control)this).Height;
			_2446(eTipPosition.Bottom);
			TipPosition = eTipPosition.Bottom;
		}
		else
		{
			empty.Y = referenceScreenRect.Y + referenceScreenRect.Height;
			_2446(eTipPosition.Top);
			TipPosition = eTipPosition.Top;
		}
		TipOffset = tipOffset;
		((Form)this).Location = empty;
		Show(balloonFocus);
	}

	public void Show(BaseItem item, bool balloonFocus)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (item == null || item.ContainerControl == null)
		{
			Show();
			return;
		}
		Point location = ((Control)item.ContainerControl).PointToScreen(item.DisplayRectangle.Location);
		Show(new Rectangle(location, item.DisplayRectangle.Size), balloonFocus);
	}

	public void Show(bool balloonFocus)
	{
		Rectangle rectEnd = new Rectangle(((Form)this).Location, ((Form)this).Size);
		Rectangle rectStart = _2448();
		if (!balloonFocus)
		{
			if (((Form)this).TopMost)
			{
				((Form)this).TopMost = false;
				_089F.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
				((Form)this).TopMost = true;
			}
			else
			{
				_089F.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
			}
		}
		if (_2447())
		{
			try
			{
				ஞ = true;
				BarFunctions.AnimateControl((Control)(object)this, show: true, _243B, rectStart, rectEnd);
				return;
			}
			finally
			{
				ஞ = false;
			}
		}
		((Control)this).Show();
	}

	public void Show()
	{
		Show(balloonFocus: true);
	}

	protected virtual void HideBalloon()
	{
		_2286();
		if (_2447())
		{
			Rectangle rectStart = new Rectangle(((Form)this).Location, ((Form)this).Size);
			Rectangle rectEnd = _2448();
			try
			{
				ஞ = true;
				BarFunctions.AnimateControl((Control)(object)this, show: false, _243B, rectStart, rectEnd);
				return;
			}
			finally
			{
				ஞ = false;
			}
		}
		((Control)this).Hide();
	}

	public void Hide()
	{
		HideBalloon();
	}

	private bool _2447()
	{
		if (Ӷ != 0 && _243A != 0 && !((Component)this).DesignMode)
		{
			return true;
		}
		return false;
	}

	private Rectangle _2448()
	{
		Rectangle result = new Rectangle(((Form)this).Location, ((Form)this).Size);
		if (_243A == eAlertAnimation.BottomToTop)
		{
			result.Y = result.Bottom - 1;
			result.Height = 1;
		}
		else if (_243A == eAlertAnimation.TopToBottom)
		{
			result.Height = 1;
		}
		else if (_243A == eAlertAnimation.LeftToRight)
		{
			result.Width = 2;
		}
		else if (_243A == eAlertAnimation.RightToLeft)
		{
			result.X = result.Right - 1;
			result.Width = 1;
		}
		return result;
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Form)this).WndProc(ref m);
		}
	}
}
