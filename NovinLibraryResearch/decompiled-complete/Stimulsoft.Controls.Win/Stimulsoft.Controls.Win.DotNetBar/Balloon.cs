using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class Balloon : Form
{
	private class ImageButton
	{
		private Rectangle m_Bounds = Rectangle.Empty;

		private Control m_Parent;

		private bool m_MouseOver;

		private bool m_MouseDown;

		public Image Normal;

		public Image Hover;

		public Image Pressed;

		public bool MouseDown => m_MouseDown;

		public bool MouseOver => m_MouseOver;

		public Rectangle Bounds
		{
			get
			{
				return m_Bounds;
			}
			set
			{
				m_Bounds = value;
			}
		}

		public Point Location
		{
			get
			{
				return m_Bounds.Location;
			}
			set
			{
				m_Bounds.Location = value;
			}
		}

		public Size Size
		{
			get
			{
				return m_Bounds.Size;
			}
			set
			{
				m_Bounds.Size = value;
			}
		}

		public event EventHandler Click;

		public ImageButton(Control parent, Rectangle bounds)
		{
			m_Parent = parent;
			m_Bounds = bounds;
		}

		public virtual void Paint(Graphics g)
		{
			if (m_MouseDown)
			{
				if (Pressed != null)
				{
					g.DrawImage(Pressed, m_Bounds);
				}
			}
			else if (m_MouseOver)
			{
				if (Hover != null)
				{
					g.DrawImage(Hover, m_Bounds);
				}
			}
			else if (Normal != null)
			{
				g.DrawImage(Normal, m_Bounds);
			}
		}

		public virtual void OnMouseMove(MouseEventArgs e)
		{
			if (m_MouseDown)
			{
				return;
			}
			if (m_Bounds.Contains(e.X, e.Y))
			{
				if (!m_MouseOver)
				{
					m_MouseOver = true;
					if (m_Parent != null)
					{
						m_Parent.Invalidate(m_Bounds);
					}
				}
			}
			else if (m_MouseOver)
			{
				m_MouseOver = false;
				if (m_Parent != null)
				{
					m_Parent.Invalidate(m_Bounds);
				}
			}
		}

		public virtual void OnMouseDown(MouseEventArgs e)
		{
			if (m_Bounds.Contains(e.X, e.Y) && !m_MouseDown)
			{
				m_MouseDown = true;
				if (m_Parent != null)
				{
					m_Parent.Invalidate(m_Bounds);
				}
			}
		}

		public virtual void OnMouseUp(MouseEventArgs e)
		{
			if (m_Bounds.Contains(e.X, e.Y) && m_MouseDown)
			{
				m_MouseDown = false;
				if (m_Parent != null)
				{
					m_Parent.Invalidate(m_Bounds);
				}
				if (this.Click != null)
				{
					this.Click(this, new EventArgs());
				}
			}
			else if (m_MouseOver || m_MouseDown)
			{
				m_MouseDown = false;
				m_MouseOver = false;
				if (m_Parent != null)
				{
					m_Parent.Invalidate(m_Bounds);
				}
			}
		}

		public virtual void OnMouseLeave(EventArgs e)
		{
			if (m_MouseOver && !m_MouseDown)
			{
				m_MouseOver = false;
				if (m_Parent != null)
				{
					m_Parent.Invalidate(m_Bounds);
				}
			}
		}
	}

	private IContainer components;

	private int m_TipLength = 16;

	private int m_CornerSize = StiScale2.I8;

	private int m_TipOffset = 32;

	private eTipPosition m_TipPosition;

	private Color m_BackColor2 = Color.Empty;

	private int m_BackColorGradientAngle = 90;

	private eBackgroundImagePosition m_BackgroundImagePosition;

	private int m_BackgroundImageAlpha = 255;

	private Color m_BorderColor = SystemColors.InfoText;

	private bool m_ShowCloseButton = true;

	private ImageButton m_CloseButton;

	private eBallonStyle m_Style;

	private Image m_CloseButtonNormal;

	private Image m_CloseButtonHot;

	private Image m_CloseButtonPressed;

	private Image m_CaptionImage;

	private Icon m_CaptionIcon;

	private Font m_CaptionFont;

	private string m_CaptionText = "";

	private StringAlignment m_CaptionAlignment;

	private Color m_CaptionColor = SystemColors.InfoText;

	private StringAlignment m_TextAlignment;

	private StringAlignment m_TextLineAlignment = (StringAlignment)1;

	private Rectangle m_CaptionImageRectangle = Rectangle.Empty;

	private Rectangle m_CaptionRectangle = Rectangle.Empty;

	private Rectangle m_TextRectangle = Rectangle.Empty;

	private bool m_AutoClose = true;

	private int m_AutoCloseTimeOut;

	private Timer m_Timer;

	internal int IMAGETEXT_SPACING = StiScale2.I4;

	internal int TEXTCLOSE_SPACING = StiScale2.I4;

	internal int CAPTIONTEXT_VSPACING = StiScale2.I1;

	internal int CAPTIONALERT_VSPACING = StiScale2.I1;

	private Color m_BorderColor1 = Color.FromArgb(241, 239, 226);

	private Color m_BorderColor2 = Color.White;

	private Color m_BorderColor3 = Color.FromArgb(236, 233, 216);

	private Color m_BorderColor4 = Color.FromArgb(172, 168, 153);

	private Color m_BorderColor5 = Color.FromArgb(113, 111, 100);

	private eAlertAnimation m_AlertAnimation = eAlertAnimation.BottomToTop;

	private int m_AlertAnimationDuration = 200;

	private bool m_AnimationInProgress;

	private bool m_AntiAlias = true;

	private int _MinimumBalloonWidth = 180;

	private const int WM_MOUSEACTIVATE = 33;

	private const int MA_NOACTIVATE = 3;

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
			return _MinimumBalloonWidth;
		}
		set
		{
			_MinimumBalloonWidth = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the target gradient background color.")]
	[Category("Style")]
	public Color BackColor2
	{
		get
		{
			return m_BackColor2;
		}
		set
		{
			m_BackColor2 = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates gradient fill angle.")]
	[Category("Style")]
	[DefaultValue(90)]
	public int BackColorGradientAngle
	{
		get
		{
			return m_BackColorGradientAngle;
		}
		set
		{
			if (value != m_BackColorGradientAngle)
			{
				m_BackColorGradientAngle = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(255)]
	[Description("Specifies the transparency of background image.")]
	public int BackgroundImageAlpha
	{
		get
		{
			return m_BackgroundImageAlpha;
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
			if (m_BackgroundImageAlpha != value)
			{
				m_BackgroundImageAlpha = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(eBackgroundImagePosition.Stretch)]
	[Description("Specifies background image position when container is larger than image.")]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return m_BackgroundImagePosition;
		}
		set
		{
			if (m_BackgroundImagePosition != value)
			{
				m_BackgroundImagePosition = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates border color.")]
	[Category("Style")]
	public Color BorderColor
	{
		get
		{
			return m_BorderColor;
		}
		set
		{
			m_BorderColor = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
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
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			m_Style = value;
			if (m_Style == eBallonStyle.Alert)
			{
				((Control)this).BackColor = Color.FromArgb(207, 221, 244);
				BackColor2 = Color.White;
				((Control)this).ForeColor = Color.FromArgb(102, 114, 196);
				m_TextAlignment = (StringAlignment)1;
			}
			else if (m_Style == eBallonStyle.Office2007Alert)
			{
				Office2007ColorTable office2007ColorTable = GetOffice2007ColorTable();
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
				m_BorderColor = legacyColors.PanelBorder;
				m_TextAlignment = (StringAlignment)0;
			}
			else
			{
				m_TextAlignment = (StringAlignment)0;
				m_BorderColor = SystemColors.InfoText;
			}
			if (m_ShowCloseButton)
			{
				CreateCloseButton();
			}
			RecalcLayout();
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the Close button is displayed.")]
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
				OnShowCloseButtonChanged();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
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
	[DefaultValue(0)]
	public int AutoCloseTimeOut
	{
		get
		{
			return m_AutoCloseTimeOut;
		}
		set
		{
			m_AutoCloseTimeOut = value;
			OnAutoCloseTimeOutChanged();
		}
	}

	[Browsable(true)]
	[Description("Indicates custom image for Close Button.")]
	[Category("Appearance")]
	public Image CloseButtonNormal
	{
		get
		{
			return m_CloseButtonNormal;
		}
		set
		{
			if (m_CloseButtonNormal != value)
			{
				m_CloseButtonNormal = value;
				OnShowCloseButtonChanged();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates custom image for Close Button when mouse is over the button.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Image CloseButtonHot
	{
		get
		{
			return m_CloseButtonHot;
		}
		set
		{
			if (m_CloseButtonHot != value)
			{
				m_CloseButtonHot = value;
				OnShowCloseButtonChanged();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates custom image for Close Button when button is pressed.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Image CloseButtonPressed
	{
		get
		{
			return m_CloseButtonPressed;
		}
		set
		{
			if (m_CloseButtonPressed != value)
			{
				m_CloseButtonPressed = value;
				OnShowCloseButtonChanged();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates Caption image.")]
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
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates Caption icon. Icon is used to provide support for alpha-blended images in caption.")]
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
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	private CompositeImage ImageInternal
	{
		get
		{
			if (m_CaptionIcon != null)
			{
				return new CompositeImage(m_CaptionIcon, dispose: false);
			}
			if (m_CaptionImage != null)
			{
				return new CompositeImage(m_CaptionImage, dispose: false);
			}
			return null;
		}
	}

	[Browsable(true)]
	[Description("Indicates Caption font.")]
	[Category("Appearance")]
	public Font CaptionFont
	{
		get
		{
			return m_CaptionFont;
		}
		set
		{
			if (m_CaptionFont != value)
			{
				m_CaptionFont = value;
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates text displayed in caption.")]
	[Category("Appearance")]
	[DefaultValue("")]
	[Localizable(true)]
	public string CaptionText
	{
		get
		{
			return m_CaptionText;
		}
		set
		{
			if (m_CaptionText != value)
			{
				m_CaptionText = value;
				RecalcLayout();
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates color of caption text")]
	[Category("Appearance")]
	public Color CaptionColor
	{
		get
		{
			return m_CaptionColor;
		}
		set
		{
			if (m_CaptionColor != value)
			{
				m_CaptionColor = value;
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
			return m_TipPosition;
		}
		set
		{
			if (m_TipPosition != value)
			{
				m_TipPosition = value;
				RecalcLayout();
				if (this.TipPositionChanged != null)
				{
					this.TipPositionChanged(this, new EventArgs());
				}
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates tip distance from the edge of the balloon.")]
	[Category("Appearance")]
	[DefaultValue(32)]
	public int TipOffset
	{
		get
		{
			return m_TipOffset;
		}
		set
		{
			if (m_TipOffset != value)
			{
				if (value < m_CornerSize)
				{
					value = m_CornerSize + 1;
				}
				else if ((m_TipPosition == eTipPosition.Top || m_TipPosition == eTipPosition.Bottom) && value > ((Control)this).Width - m_CornerSize - m_TipLength)
				{
					value = ((Control)this).Width - m_CornerSize - m_TipLength;
				}
				else if ((m_TipPosition == eTipPosition.Left || m_TipPosition == eTipPosition.Right) && value > ((Control)this).Height - m_CornerSize - m_TipLength)
				{
					value = ((Control)this).Height - m_CornerSize - m_TipLength;
				}
				m_TipOffset = value;
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
	public int TipLength => m_TipLength;

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

	public event CustomPaintEventHandler PaintBackground;

	public event CustomPaintEventHandler PaintCaptionImage;

	public event CustomPaintEventHandler PaintCaptionText;

	public event CustomPaintEventHandler PaintText;

	public event EventHandler CloseButtonClick;

	public event EventHandler TipPositionChanged;

	public Balloon()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		CreateCloseButton();
		InitializeComponent();
		if (components == null)
		{
			components = new Container();
		}
		m_CaptionFont = new Font(((Control)this).Font, (FontStyle)1);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)1, true);
	}

	protected override void Dispose(bool disposing)
	{
		DestroyAutoCloseTimer();
		if (disposing && components != null)
		{
			components.Dispose();
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_CaptionIcon);
			BarUtilities.DisposeImage(ref m_CaptionImage);
			BarUtilities.DisposeImage(ref m_CloseButtonHot);
			BarUtilities.DisposeImage(ref m_CloseButtonNormal);
			BarUtilities.DisposeImage(ref m_CloseButtonPressed);
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).AccessibleRole = (AccessibleRole)31;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
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
		if (!m_ShowCloseButton || !m_CloseButton.Bounds.Contains(((Control)this).PointToClient(Control.MousePosition)))
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
		if (m_AntiAlias)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)2;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		if (m_Style == eBallonStyle.Balloon)
		{
			PaintBalloon(e.Graphics);
		}
		else
		{
			PaintAlert(e.Graphics);
		}
		e.Graphics.SmoothingMode = smoothingMode;
		e.Graphics.TextRenderingHint = textRenderingHint;
	}

	private void PaintAlert(Graphics g)
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Invalid comparison between Unknown and I4
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Expected O, but got Unknown
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Invalid comparison between Unknown and I4
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Invalid comparison between Unknown and I4
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Invalid comparison between Unknown and I4
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Invalid comparison between Unknown and I4
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Invalid comparison between Unknown and I4
		Rectangle alertClientRect = GetAlertClientRect();
		if (this.PaintBackground != null)
		{
			this.PaintBackground(this, new CustomPaintEventArgs(g, alertClientRect));
		}
		else
		{
			PaintBackgroundInternal(g);
			if (m_Style == eBallonStyle.Alert)
			{
				Rectangle rectangle = alertClientRect;
				rectangle.Height = Math.Max(m_CaptionImageRectangle.Bottom, m_CaptionRectangle.Bottom) - rectangle.Top;
				rectangle.Height += CAPTIONALERT_VSPACING;
				if (rectangle.Width > 0 && rectangle.Height > 0)
				{
					LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(rectangle, ((Control)this).BackColor, m_BackColor2, m_BackColorGradientAngle);
					try
					{
						Blend val2 = new Blend(4);
						val2.Positions = new float[4] { 0f, 0.5f, 0.5f, 1f };
						val2.Factors = new float[4] { 0f, 1f, 1f, 0f };
						val.Blend = val2;
						g.FillRectangle((Brush)(object)val, rectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		if (m_Style == eBallonStyle.Office2007Alert)
		{
			DisplayHelp.DrawRectangle(g, m_BorderColor, displayRectangle);
		}
		else
		{
			displayRectangle.Width--;
			displayRectangle.Height--;
			Pen val3 = new Pen(m_BorderColor1, 1f);
			try
			{
				g.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.Right, displayRectangle.Y);
				g.DrawLine(val3, displayRectangle.X, displayRectangle.Y, displayRectangle.X, displayRectangle.Bottom);
				g.DrawLine(val3, displayRectangle.X + 4, displayRectangle.Bottom - 4, displayRectangle.Right - 4, displayRectangle.Bottom - 4);
				g.DrawLine(val3, displayRectangle.Right - 4, displayRectangle.Y + 4, displayRectangle.Right - 4, displayRectangle.Bottom - 4);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Pen val4 = new Pen(m_BorderColor2, 1f);
			try
			{
				g.DrawLine(val4, displayRectangle.X + 1, displayRectangle.Y + 1, displayRectangle.Right - 1, displayRectangle.Y + 1);
				g.DrawLine(val4, displayRectangle.X + 1, displayRectangle.Y + 1, displayRectangle.X + 1, displayRectangle.Bottom - 1);
				g.DrawLine(val4, displayRectangle.X + 3, displayRectangle.Bottom - 3, displayRectangle.Right - 3, displayRectangle.Bottom - 3);
				g.DrawLine(val4, displayRectangle.Right - 3, displayRectangle.Y + 3, displayRectangle.Right - 3, displayRectangle.Bottom - 3);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			Pen val5 = new Pen(m_BorderColor3, 1f);
			try
			{
				g.DrawLine(val5, displayRectangle.X + 2, displayRectangle.Y + 2, displayRectangle.Right - 2, displayRectangle.Y + 2);
				g.DrawLine(val5, displayRectangle.X + 2, displayRectangle.Y + 2, displayRectangle.X + 2, displayRectangle.Bottom - 2);
				g.DrawLine(val5, displayRectangle.X + 2, displayRectangle.Bottom - 2, displayRectangle.Right - 2, displayRectangle.Bottom - 2);
				g.DrawLine(val5, displayRectangle.Right - 2, displayRectangle.Y + 2, displayRectangle.Right - 2, displayRectangle.Bottom - 2);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			Pen val6 = new Pen(m_BorderColor4, 1f);
			try
			{
				g.DrawLine(val6, displayRectangle.X + 3, displayRectangle.Y + 3, displayRectangle.Right - 3, displayRectangle.Y + 3);
				g.DrawLine(val6, displayRectangle.X + 3, displayRectangle.Y + 3, displayRectangle.X + 3, displayRectangle.Bottom - 3);
				g.DrawLine(val6, displayRectangle.X + 1, displayRectangle.Bottom - 1, displayRectangle.Right - 1, displayRectangle.Bottom - 1);
				g.DrawLine(val6, displayRectangle.Right - 1, displayRectangle.Y + 1, displayRectangle.Right - 1, displayRectangle.Bottom - 1);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			Pen val7 = new Pen(m_BorderColor5, 1f);
			try
			{
				g.DrawLine(val7, displayRectangle.X + 4, displayRectangle.Y + 4, displayRectangle.Right - 4, displayRectangle.Y + 4);
				g.DrawLine(val7, displayRectangle.X + 4, displayRectangle.Y + 4, displayRectangle.X + 4, displayRectangle.Bottom - 4);
				g.DrawLine(val7, displayRectangle.X, displayRectangle.Bottom, displayRectangle.Right, displayRectangle.Bottom);
				g.DrawLine(val7, displayRectangle.Right, displayRectangle.Y, displayRectangle.Right, displayRectangle.Bottom);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (m_CloseButton != null)
		{
			m_CloseButton.Paint(g);
		}
		if (this.PaintCaptionImage != null)
		{
			this.PaintCaptionImage(this, new CustomPaintEventArgs(g, m_CaptionImageRectangle));
		}
		else
		{
			CompositeImage imageInternal = ImageInternal;
			if (imageInternal != null && !m_CaptionImageRectangle.IsEmpty)
			{
				imageInternal.DrawImage(g, m_CaptionImageRectangle);
			}
		}
		if (this.PaintCaptionText != null)
		{
			this.PaintCaptionText(this, new CustomPaintEventArgs(g, m_CaptionRectangle));
		}
		else if (m_CaptionText != "" && !m_CaptionRectangle.IsEmpty)
		{
			eTextFormat eTextFormat2 = eTextFormat.WordBreak;
			if ((int)m_CaptionAlignment == 1)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if ((int)m_CaptionAlignment == 2)
			{
				eTextFormat2 |= eTextFormat.Right;
			}
			Font val8 = m_CaptionFont;
			if (val8 == null)
			{
				val8 = ((Control)this).Font;
			}
			TextDrawing.DrawString(g, m_CaptionText, val8, m_CaptionColor, m_CaptionRectangle, eTextFormat2);
		}
		if (this.PaintText != null)
		{
			this.PaintText(this, new CustomPaintEventArgs(g, m_TextRectangle));
		}
		else if (((Control)this).Text != "")
		{
			eTextFormat eTextFormat3 = eTextFormat.WordBreak;
			if ((int)m_TextAlignment == 1)
			{
				eTextFormat3 |= eTextFormat.HorizontalCenter;
			}
			else if ((int)m_TextAlignment == 2)
			{
				eTextFormat3 |= eTextFormat.Right;
			}
			if ((int)m_TextLineAlignment == 1)
			{
				eTextFormat3 |= eTextFormat.VerticalCenter;
			}
			else if ((int)m_TextLineAlignment == 2)
			{
				eTextFormat3 |= eTextFormat.Bottom;
			}
			TextDrawing.DrawString(g, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, m_TextRectangle, eTextFormat3);
		}
	}

	private void PaintBalloon(Graphics g)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		GraphicsPath balloonPath = GetBalloonPath(rectangle);
		if (this.PaintBackground != null)
		{
			this.PaintBackground(this, new CustomPaintEventArgs(g, rectangle));
		}
		else
		{
			PaintBackgroundInternal(g);
		}
		Pen val = new Pen(m_BorderColor, 1f);
		try
		{
			val.Alignment = (PenAlignment)1;
			g.DrawPath(val, balloonPath);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (m_CloseButton != null)
		{
			m_CloseButton.Paint(g);
		}
		if (this.PaintCaptionImage != null)
		{
			this.PaintCaptionImage(this, new CustomPaintEventArgs(g, m_CaptionImageRectangle));
		}
		else
		{
			CompositeImage imageInternal = ImageInternal;
			if (imageInternal != null && !m_CaptionImageRectangle.IsEmpty)
			{
				imageInternal.DrawImage(g, m_CaptionImageRectangle);
			}
		}
		if (this.PaintCaptionText != null)
		{
			this.PaintCaptionText(this, new CustomPaintEventArgs(g, m_CaptionRectangle));
		}
		else if (m_CaptionText != "" && !m_CaptionRectangle.IsEmpty)
		{
			eTextFormat format = eTextFormat.Default | TextDrawing.TranslateHorizontal(m_CaptionAlignment) | eTextFormat.WordBreak;
			Font val2 = m_CaptionFont;
			if (val2 == null)
			{
				val2 = ((Control)this).Font;
			}
			TextDrawing.DrawString(g, m_CaptionText, val2, m_CaptionColor, m_CaptionRectangle, format);
		}
		if (this.PaintText != null)
		{
			this.PaintText(this, new CustomPaintEventArgs(g, m_TextRectangle));
		}
		else if (((Control)this).Text != "")
		{
			eTextFormat format2 = eTextFormat.Default | TextDrawing.TranslateHorizontal(m_TextAlignment) | TextDrawing.TranslateVertical(m_TextLineAlignment) | eTextFormat.WordBreak;
			TextDrawing.DrawString(g, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, m_TextRectangle, format2);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Form)this).OnResize(e);
		if (!m_AnimationInProgress)
		{
			RecalcLayout();
		}
	}

	public void AutoResize()
	{
		Size size = new Size(_MinimumBalloonWidth, 0);
		Size empty = Size.Empty;
		Font val = m_CaptionFont;
		if (val == null)
		{
			val = ((Control)this).Font;
		}
		Graphics val2 = ((Control)this).CreateGraphics();
		try
		{
			empty = TextDrawing.MeasureString(val2, (m_CaptionText != "") ? m_CaptionText : " ", val);
			size.Width = Math.Max(size.Width, empty.Width);
			if (m_CloseButton != null)
			{
				size.Width += m_CloseButton.Size.Width;
			}
			else
			{
				size.Width += 18;
			}
			size.Width += TEXTCLOSE_SPACING;
			int val3 = 0;
			if (m_CaptionIcon != null)
			{
				size.Width += m_CaptionIcon.Width + IMAGETEXT_SPACING;
				val3 = m_CaptionIcon.Height;
			}
			else if (m_CaptionImage != null)
			{
				size.Width += m_CaptionImage.Width + IMAGETEXT_SPACING;
				val3 = m_CaptionImage.Height;
			}
			size.Height += Math.Max(val3, empty.Height);
			size.Height += IMAGETEXT_SPACING * 2;
			empty = TextDrawing.MeasureString(val2, (((Control)this).Text != "") ? ((Control)this).Text : " ", ((Control)this).Font, size.Width, eTextFormat.WordBreak);
			size.Height += empty.Height;
			size.Width += m_CornerSize * 2;
			size.Height += m_CornerSize * 2;
			if (m_Style == eBallonStyle.Balloon)
			{
				if (m_TipPosition == eTipPosition.Left || m_TipPosition == eTipPosition.Right)
				{
					size.Width += m_TipLength;
				}
				else
				{
					size.Height += m_TipLength;
				}
			}
			else
			{
				size.Height += (int)((double)(float)size.Height * 0.3);
			}
		}
		finally
		{
			val2.Dispose();
		}
		((Form)this).Size = size;
	}

	public void RecalcLayout()
	{
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Invalid comparison between Unknown and I4
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Invalid comparison between Unknown and I4
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Invalid comparison between Unknown and I4
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Invalid comparison between Unknown and I4
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		if (((Control)this).DisplayRectangle.Width == 0 || ((Control)this).DisplayRectangle.Height == 0)
		{
			return;
		}
		if (m_Style == eBallonStyle.Balloon)
		{
			Rectangle captionRectangle = new Rectangle(m_CornerSize / 2, m_CornerSize / 2, ((Control)this).Width - m_CornerSize, ((Control)this).Height - m_CornerSize);
			if (m_TipPosition == eTipPosition.Bottom)
			{
				captionRectangle.Height -= m_TipLength;
			}
			else if (m_TipPosition == eTipPosition.Left)
			{
				captionRectangle.X += m_TipLength;
				captionRectangle.Width -= m_TipLength;
			}
			else if (m_TipPosition == eTipPosition.Right)
			{
				captionRectangle.Width -= m_TipLength;
			}
			else
			{
				captionRectangle.Y += m_TipLength;
				captionRectangle.Height -= m_TipLength;
			}
			Size size = Size.Empty;
			if (m_CaptionIcon != null)
			{
				size = m_CaptionIcon.Size;
			}
			else if (m_CaptionImage != null)
			{
				size = m_CaptionImage.Size;
			}
			m_CaptionRectangle = captionRectangle;
			if (!size.IsEmpty)
			{
				m_CaptionRectangle.X += size.Width + IMAGETEXT_SPACING;
				m_CaptionRectangle.Width -= size.Width + IMAGETEXT_SPACING;
			}
			if (m_CloseButton != null)
			{
				m_CloseButton.Location = new Point(captionRectangle.Right - m_CloseButton.Bounds.Width, captionRectangle.Top);
				m_CaptionRectangle.Width -= m_CloseButton.Size.Width + TEXTCLOSE_SPACING;
			}
			Font val = m_CaptionFont;
			if (val == null)
			{
				val = ((Control)this).Font;
			}
			Graphics val2 = ((Control)this).CreateGraphics();
			try
			{
				eTextFormat format = eTextFormat.WordBreak;
				if ((int)m_CaptionAlignment == 1)
				{
					format = eTextFormat.HorizontalCenter;
				}
				else if ((int)m_CaptionAlignment == 2)
				{
					format = eTextFormat.Right;
				}
				Size size2 = TextDrawing.MeasureString(val2, (m_CaptionText != "") ? m_CaptionText : " ", val, m_CaptionRectangle.Width, format);
				m_CaptionRectangle.Height = size2.Height;
			}
			finally
			{
				val2.Dispose();
			}
			if (!size.IsEmpty)
			{
				m_CaptionImageRectangle = new Rectangle(captionRectangle.X, captionRectangle.Y, size.Width, size.Height);
				if (m_CaptionRectangle.Height > size.Height)
				{
					m_CaptionImageRectangle.Y += (m_CaptionRectangle.Height - size.Height) / 2;
				}
				else if (size.Height > m_CaptionRectangle.Height)
				{
					m_CaptionRectangle.Y += (size.Height - m_CaptionRectangle.Height) / 2;
				}
			}
			else if (m_CloseButton != null && m_CloseButton.Size.Height > m_CaptionRectangle.Height)
			{
				m_CaptionRectangle.Y += (m_CloseButton.Size.Height - m_CaptionRectangle.Height) / 2;
			}
			if (m_CaptionRectangle.Bottom > m_CaptionImageRectangle.Bottom)
			{
				m_TextRectangle = new Rectangle(captionRectangle.X, m_CaptionRectangle.Bottom + CAPTIONTEXT_VSPACING, captionRectangle.Width, captionRectangle.Bottom - (m_CaptionRectangle.Bottom + CAPTIONTEXT_VSPACING));
			}
			else
			{
				m_TextRectangle = new Rectangle(captionRectangle.X, m_CaptionImageRectangle.Bottom + CAPTIONTEXT_VSPACING, captionRectangle.Width, captionRectangle.Bottom - (m_CaptionImageRectangle.Bottom + CAPTIONTEXT_VSPACING));
			}
			GraphicsPath balloonPath = GetBalloonPath(((Control)this).DisplayRectangle);
			GraphicsPath val3 = (GraphicsPath)balloonPath.Clone();
			val3.Widen(Pens.Black);
			Region val4 = new Region(val3);
			val4.Union(balloonPath);
			((Control)this).Region = val4;
			balloonPath.Dispose();
			val3.Dispose();
			return;
		}
		Rectangle alertClientRect = GetAlertClientRect();
		Size size3 = Size.Empty;
		if (m_CaptionIcon != null)
		{
			size3 = m_CaptionIcon.Size;
		}
		else if (m_CaptionImage != null)
		{
			size3 = m_CaptionImage.Size;
		}
		m_CaptionRectangle = alertClientRect;
		if (!size3.IsEmpty)
		{
			m_CaptionRectangle.X += size3.Width + IMAGETEXT_SPACING * 2;
			m_CaptionRectangle.Width -= size3.Width + IMAGETEXT_SPACING * 2;
		}
		if (m_CloseButton != null)
		{
			m_CloseButton.Location = new Point(alertClientRect.Right - m_CloseButton.Bounds.Width - CAPTIONALERT_VSPACING, alertClientRect.Top);
			m_CaptionRectangle.Width -= m_CloseButton.Size.Width + TEXTCLOSE_SPACING + CAPTIONALERT_VSPACING;
		}
		Font val5 = m_CaptionFont;
		if (val5 == null)
		{
			val5 = ((Control)this).Font;
		}
		Graphics val6 = ((Control)this).CreateGraphics();
		try
		{
			eTextFormat format2 = eTextFormat.WordBreak;
			if ((int)m_CaptionAlignment == 1)
			{
				format2 = eTextFormat.HorizontalCenter;
			}
			else if ((int)m_CaptionAlignment == 2)
			{
				format2 = eTextFormat.Right;
			}
			Size size4 = TextDrawing.MeasureString(val6, (m_CaptionText != "") ? m_CaptionText : " ", val5, m_CaptionRectangle.Width, format2);
			m_CaptionRectangle.Height = size4.Height;
		}
		finally
		{
			val6.Dispose();
		}
		if (!size3.IsEmpty)
		{
			m_CaptionImageRectangle = new Rectangle(alertClientRect.X + IMAGETEXT_SPACING, alertClientRect.Y, size3.Width, size3.Height);
			if (m_CaptionRectangle.Height > size3.Height)
			{
				m_CaptionImageRectangle.Y += (m_CaptionRectangle.Height - size3.Height) / 2;
			}
			else if (size3.Height > m_CaptionRectangle.Height)
			{
				m_CaptionRectangle.Y += (size3.Height + -m_CaptionRectangle.Height) / 2;
			}
		}
		else if (m_CloseButton != null && m_CloseButton.Size.Height > m_CaptionRectangle.Height)
		{
			m_CaptionRectangle.Y += (m_CloseButton.Size.Height - m_CaptionRectangle.Height) / 2;
		}
		m_CaptionRectangle.Offset(0, CAPTIONALERT_VSPACING);
		m_CaptionImageRectangle.Offset(0, CAPTIONALERT_VSPACING);
		if (m_CloseButton != null)
		{
			m_CloseButton.Location = new Point(m_CloseButton.Location.X, m_CaptionRectangle.Y + (m_CaptionRectangle.Height - m_CloseButton.Size.Height) / 2);
		}
		if (m_CaptionRectangle.Bottom > m_CaptionImageRectangle.Bottom)
		{
			m_TextRectangle = new Rectangle(alertClientRect.X, m_CaptionRectangle.Bottom + CAPTIONTEXT_VSPACING * 4, alertClientRect.Width, alertClientRect.Bottom - (m_CaptionRectangle.Bottom + CAPTIONTEXT_VSPACING * 4));
		}
		else
		{
			m_TextRectangle = new Rectangle(alertClientRect.X, m_CaptionImageRectangle.Bottom + CAPTIONTEXT_VSPACING * 4, alertClientRect.Width, alertClientRect.Bottom - (m_CaptionImageRectangle.Bottom + CAPTIONTEXT_VSPACING * 4));
		}
		((Control)this).Region = new Region(((Control)this).DisplayRectangle);
	}

	private Rectangle GetAlertClientRect()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.Inflate(-4, -4);
		return result;
	}

	private GraphicsPath GetBalloonPath(Rectangle rect)
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
		Rectangle rectangle = rect;
		int num = m_CornerSize * 2;
		Matrix val2 = new Matrix();
		Point[] array = new Point[3];
		int num2 = m_TipOffset;
		rectangle.Size = new Size(rectangle.Width - 1, rectangle.Height - 1);
		bool flag = false;
		if (m_TipPosition == eTipPosition.Bottom)
		{
			flag = true;
			val2.Translate((float)rectangle.Width, (float)rectangle.Height);
			val2.Rotate(180f);
		}
		else if ((m_TipPosition == eTipPosition.Left && (int)((Control)this).RightToLeft == 0) || (m_TipPosition == eTipPosition.Right && (int)((Control)this).RightToLeft == 1))
		{
			flag = true;
			rectangle.Size = new Size(rectangle.Height, rectangle.Width);
			val2.Translate(0f, (float)rectangle.Width);
			val2.Rotate(-90f);
		}
		else if ((m_TipPosition == eTipPosition.Right && (int)((Control)this).RightToLeft == 0) || (m_TipPosition == eTipPosition.Left && (int)((Control)this).RightToLeft == 1))
		{
			rectangle.Size = new Size(rectangle.Height, rectangle.Width);
			val2.Translate((float)rectangle.Height, 0f);
			val2.Rotate(90f);
		}
		int num3 = (rectangle.Width - m_CornerSize * 2) / 2 + 1;
		int cornerSize = m_CornerSize;
		if (flag)
		{
			num2 = rectangle.Width - cornerSize * 2 - m_TipOffset + 1;
		}
		if (num2 < num3)
		{
			array[0] = new Point(num2 + cornerSize, rectangle.Y + m_TipLength);
			array[1] = new Point(num2 + cornerSize, rectangle.Y);
			array[2] = new Point(num2 + m_TipLength + cornerSize, rectangle.Y + m_TipLength);
		}
		else if (num2 > num3)
		{
			array[0] = new Point(num2 - m_TipLength + cornerSize, rectangle.Y + m_TipLength);
			array[1] = new Point(num2 + cornerSize, rectangle.Y);
			array[2] = new Point(num2 + cornerSize, rectangle.Y + m_TipLength);
		}
		else
		{
			array[0] = new Point(num2 - m_TipLength + cornerSize, rectangle.Y + m_TipLength);
			array[1] = new Point(num2 + cornerSize, rectangle.Y);
			array[2] = new Point(num2 + m_TipLength + cornerSize, rectangle.Y + m_TipLength);
		}
		val.AddArc(rectangle.Left, rectangle.Top + m_TipLength, num, num, 180f, 90f);
		val.AddLine(array[0], array[1]);
		val.AddLine(array[1], array[2]);
		val.AddArc(rectangle.Width - num, rectangle.Top + m_TipLength, num, num, -90f, 90f);
		val.AddArc(rectangle.Width - num, rectangle.Bottom - num, num, num, 0f, 90f);
		val.AddArc(rectangle.Left, rectangle.Bottom - num, num, num, 90f, 90f);
		val.CloseFigure();
		val.Transform(val2);
		return val;
	}

	private void PaintBackgroundInternal(Graphics g)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (clientRectangle.Width == 0 || clientRectangle.Height == 0)
		{
			return;
		}
		if (m_BackColor2.IsEmpty)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				g.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(clientRectangle, ((Control)this).BackColor, m_BackColor2, m_BackColorGradientAngle);
			try
			{
				if (m_Style == eBallonStyle.Alert)
				{
					Blend val3 = new Blend(4);
					val3.Positions = new float[8] { 0f, 0.25f, 0.4f, 0.5f, 0.5f, 0.85f, 0.85f, 1f };
					val3.Factors = new float[8] { 0f, 1f, 1f, 0f, 0f, 0f, 0f, 1f };
					val2.Blend = val3;
				}
				g.FillRectangle((Brush)(object)val2, clientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (((Control)this).BackgroundImage != null)
		{
			BarFunctions.PaintBackgroundImage(g, clientRectangle, ((Control)this).BackgroundImage, m_BackgroundImagePosition, m_BackgroundImageAlpha);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !m_BackColor2.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		m_BackColor2 = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return m_BorderColor != SystemColors.InfoText;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		m_BorderColor = SystemColors.InfoText;
	}

	private Office2007ColorTable GetOffice2007ColorTable()
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
		if (m_CloseButton != null)
		{
			m_CloseButton.OnMouseDown(e);
		}
		if (((Control)this).Focused)
		{
			((Control)this).Focus();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (m_CloseButton != null)
		{
			m_CloseButton.OnMouseMove(e);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (m_CloseButton != null)
		{
			m_CloseButton.OnMouseUp(e);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (m_CloseButton != null)
		{
			m_CloseButton.OnMouseLeave(e);
		}
	}

	private void OnShowCloseButtonChanged()
	{
		if (m_ShowCloseButton)
		{
			CreateCloseButton();
			RecalcLayout();
		}
		else
		{
			m_CloseButton = null;
		}
	}

	private void CreateCloseButton()
	{
		if (m_CloseButtonNormal != null)
		{
			m_CloseButton = new ImageButton((Control)(object)this, new Rectangle(0, 0, StiScale2.XXI(m_CloseButtonNormal.Width), StiScale2.YYI(m_CloseButtonNormal.Height)));
			if (m_CloseButton.Normal != null)
			{
				m_CloseButton.Normal.Dispose();
				m_CloseButton.Normal = null;
			}
			m_CloseButton.Normal = m_CloseButtonNormal;
			if (m_CloseButton.Hover != null)
			{
				m_CloseButton.Hover.Dispose();
				m_CloseButton.Hover = null;
			}
			m_CloseButton.Hover = m_CloseButtonHot;
			if (m_CloseButton.Pressed != null)
			{
				m_CloseButton.Pressed.Dispose();
				m_CloseButton.Pressed = null;
			}
			m_CloseButton.Pressed = m_CloseButtonPressed;
		}
		else if (m_Style == eBallonStyle.Balloon)
		{
			m_CloseButton = new ImageButton((Control)(object)this, new Rectangle(0, 0, StiScale2.XXI(18.0), StiScale2.XXI(18.0)));
			m_CloseButton.Normal = StiImageResizer2.GetImage((Image)(object)BarFunctions.LoadBitmap("BalloonImages.BalloonNormal.png"));
			m_CloseButton.Hover = StiImageResizer2.GetImage((Image)(object)BarFunctions.LoadBitmap("BalloonImages.BalloonHot.png"));
			m_CloseButton.Pressed = StiImageResizer2.GetImage((Image)(object)BarFunctions.LoadBitmap("BalloonImages.BalloonPress.png"));
		}
		else
		{
			m_CloseButton = new ImageButton((Control)(object)this, new Rectangle(0, 0, StiScale2.XXI(13.0), StiScale2.XXI(13.0)));
			m_CloseButton.Normal = StiImageResizer2.GetImage((Image)(object)BarFunctions.LoadBitmap("BalloonImages.AlertNormal.png"));
			m_CloseButton.Hover = StiImageResizer2.GetImage((Image)(object)BarFunctions.LoadBitmap("BalloonImages.AlertHot.png"));
			m_CloseButton.Pressed = StiImageResizer2.GetImage((Image)(object)BarFunctions.LoadBitmap("BalloonImages.AlertPress.png"));
		}
		m_CloseButton.Click += CloseButtonClickInternal;
	}

	private void CloseButtonClickInternal(object sender, EventArgs e)
	{
		if (this.CloseButtonClick != null)
		{
			this.CloseButtonClick(this, new EventArgs());
		}
		if (m_AutoClose)
		{
			Hide();
			((Form)this).Close();
		}
	}

	protected void OnAutoCloseTimeOutChanged()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (m_AutoCloseTimeOut > 0 && !((Component)this).DesignMode)
		{
			if (m_Timer == null)
			{
				m_Timer = new Timer(components);
			}
			m_Timer.Enabled = false;
			m_Timer.Interval = m_AutoCloseTimeOut * 1000;
			m_Timer.Tick += AutoCloseTimeOutEllapsed;
			if (Visible)
			{
				m_Timer.Enabled = true;
				m_Timer.Start();
			}
		}
		else
		{
			DestroyAutoCloseTimer();
		}
	}

	protected virtual void AutoCloseTimeOutEllapsed(object sender, EventArgs e)
	{
		if (!((Control)this).IsDisposed)
		{
			DestroyAutoCloseTimer();
			Hide();
			((Form)this).Close();
		}
	}

	private void DestroyAutoCloseTimer()
	{
		if (m_Timer != null)
		{
			m_Timer.Enabled = false;
			m_Timer.Tick -= AutoCloseTimeOutEllapsed;
			components.Remove((IComponent?)m_Timer);
			((Component)(object)m_Timer).Dispose();
			m_Timer = null;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Form)this).OnVisibleChanged(e);
		if (Visible)
		{
			if (m_Timer != null && !m_Timer.Enabled)
			{
				m_Timer.Enabled = true;
				m_Timer.Start();
			}
		}
		else if (m_Timer != null && m_Timer.Enabled)
		{
			m_Timer.Stop();
			m_Timer.Enabled = false;
		}
	}

	private void InternalTipPositionChanged(eTipPosition newValue)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0 || Style != 0)
		{
			return;
		}
		if (newValue == eTipPosition.Top && m_TipPosition == eTipPosition.Bottom)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				val.Top += m_TipLength;
			}
			return;
		}
		if (newValue != eTipPosition.Bottom || m_TipPosition != 0)
		{
			return;
		}
		foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item2;
			val2.Top -= m_TipLength;
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
		int num = m_TipLength + m_CornerSize * 3;
		int tipOffset = m_TipLength / 2 + m_CornerSize * 3;
		Point empty = Point.Empty;
		ScreenInformation screenInformation = BarFunctions.ScreenFromPoint(referenceScreenRect.Location);
		if (referenceScreenRect.X + referenceScreenRect.Width / 2 + ((Control)this).Width > screenInformation.WorkingArea.Right)
		{
			empty.X = referenceScreenRect.X + referenceScreenRect.Width / 2 - ((Control)this).Width + num;
			tipOffset = ((Control)this).Width - num;
		}
		else
		{
			empty.X = referenceScreenRect.X + referenceScreenRect.Width / 2 - num;
		}
		if (referenceScreenRect.Y + referenceScreenRect.Height + ((Control)this).Height > screenInformation.WorkingArea.Bottom)
		{
			empty.Y = referenceScreenRect.Y - ((Control)this).Height;
			InternalTipPositionChanged(eTipPosition.Bottom);
			TipPosition = eTipPosition.Bottom;
		}
		else
		{
			empty.Y = referenceScreenRect.Y + referenceScreenRect.Height;
			InternalTipPositionChanged(eTipPosition.Top);
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
		Rectangle animationRectangle = GetAnimationRectangle();
		if (!balloonFocus)
		{
			if (((Form)this).TopMost)
			{
				((Form)this).TopMost = false;
				NativeFunctions.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
				((Form)this).TopMost = true;
			}
			else
			{
				NativeFunctions.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
			}
		}
		if (ShouldAnimate())
		{
			try
			{
				m_AnimationInProgress = true;
				BarFunctions.AnimateControl((Control)(object)this, show: true, m_AlertAnimationDuration, animationRectangle, rectEnd);
				return;
			}
			finally
			{
				m_AnimationInProgress = false;
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
		DestroyAutoCloseTimer();
		if (ShouldAnimate())
		{
			Rectangle rectStart = new Rectangle(((Form)this).Location, ((Form)this).Size);
			Rectangle animationRectangle = GetAnimationRectangle();
			try
			{
				m_AnimationInProgress = true;
				BarFunctions.AnimateControl((Control)(object)this, show: false, m_AlertAnimationDuration, rectStart, animationRectangle);
				return;
			}
			finally
			{
				m_AnimationInProgress = false;
			}
		}
		((Control)this).Hide();
	}

	public void Hide()
	{
		HideBalloon();
	}

	private bool ShouldAnimate()
	{
		if (m_Style != 0 && m_AlertAnimation != 0 && !((Component)this).DesignMode)
		{
			return true;
		}
		return false;
	}

	private Rectangle GetAnimationRectangle()
	{
		Rectangle result = new Rectangle(((Form)this).Location, ((Form)this).Size);
		if (m_AlertAnimation == eAlertAnimation.BottomToTop)
		{
			result.Y = result.Bottom - 1;
			result.Height = 1;
		}
		else if (m_AlertAnimation == eAlertAnimation.TopToBottom)
		{
			result.Height = 1;
		}
		else if (m_AlertAnimation == eAlertAnimation.LeftToRight)
		{
			result.Width = 2;
		}
		else if (m_AlertAnimation == eAlertAnimation.RightToLeft)
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
