using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(ButtonX), "ButtonX.ButtonX.ico")]
[ToolboxItem(false)]
[DefaultEvent("Click")]
public class ButtonX : PopupItemControl, IButtonControl, ICommandSource
{
	private ButtonItem m_Button;

	private DialogResult m_DialogResult;

	private bool m_IsDefault;

	private bool m_FadeEffect = true;

	private eButtonTextAlignment m_TextAlignment = eButtonTextAlignment.Center;

	private Size m_PreferredSize = Size.Empty;

	private bool _CallBasePaintBackground = true;

	private bool _IsSpaceKeyDown;

	private Point _MouseDownPoint = Point.Empty;

	private bool _FocusOnLeftMouseButtonDown = true;

	private bool _EnableMnemonicWithAltKeyOnly;

	private static RoundRectangleShapeDescriptor _DefaultButtonShape = new RoundRectangleShapeDescriptor(2);

	private static RoundRectangleShapeDescriptor _DefaultMetroButtonShape = new RoundRectangleShapeDescriptor(0);

	private ShapeDescriptor _Shape;

	private bool _FocusCuesEnabled = true;

	private AutoSizeMode m_AutoSizeMode = (AutoSizeMode)1;

	private int _ImageTextSpacing;

	private static readonly Color DefaultTextColor = Color.Empty;

	private Color _TextColor = DefaultTextColor;

	private ICommand _Command;

	private object _CommandParameter;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool Is20Button
	{
		get
		{
			return m_Button.Is20Button;
		}
		set
		{
			m_Button.Is20Button = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsCenteredButton
	{
		get
		{
			return m_Button.IsCenteredButton;
		}
		set
		{
			m_Button.IsCenteredButton = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int DropArrowShift
	{
		get
		{
			return m_Button.DropArrowShift;
		}
		set
		{
			m_Button.DropArrowShift = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	protected internal ButtonItem ButtonItem => m_Button;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	protected bool IsMouseDown => m_Button.IsMouseDown;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return m_Button.EnableMarkup;
		}
		set
		{
			m_Button.EnableMarkup = value;
		}
	}

	[Browsable(false)]
	public bool IsPulsing => m_Button.IsPulsing;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether pulse effect started with Pulse method stops automatically when mouse moves over the button.")]
	public bool StopPulseOnMouseOver
	{
		get
		{
			return m_Button.StopPulseOnMouseOver;
		}
		set
		{
			m_Button.StopPulseOnMouseOver = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(12)]
	[Category("Behavior")]
	[Description("Indicates pulse speed. The value must be greater than 0 and less than 128.")]
	public int PulseSpeed
	{
		get
		{
			return m_Button.PulseSpeed;
		}
		set
		{
			m_Button.PulseSpeed = value;
		}
	}

	[Browsable(true)]
	public Size ImageFixedSize
	{
		get
		{
			return m_Button.ImageFixedSize;
		}
		set
		{
			m_Button.ImageFixedSize = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(eButtonTextAlignment.Center)]
	[Category("Appearance")]
	[Description("Indicates text alignment. Applies only when button text is not composed using text markup. Default value is center.")]
	public eButtonTextAlignment TextAlignment
	{
		get
		{
			return m_TextAlignment;
		}
		set
		{
			m_TextAlignment = value;
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CallBasePaintBackground
	{
		get
		{
			return _CallBasePaintBackground;
		}
		set
		{
			_CallBasePaintBackground = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FocusOnLeftMouseButtonDown
	{
		get
		{
			return _FocusOnLeftMouseButtonDown;
		}
		set
		{
			_FocusOnLeftMouseButtonDown = value;
		}
	}

	protected virtual bool ExecuteCommandOnClick => true;

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SymbolTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return m_Button.Symbol;
		}
		set
		{
			m_Button.Symbol = value;
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return m_Button.SymbolSize;
		}
		set
		{
			m_Button.SymbolSize = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the button.")]
	[DefaultValue(null)]
	public virtual Image Image
	{
		get
		{
			return m_Button.Image;
		}
		set
		{
			m_Button.Image = value;
			InvalidateAutoSize();
			AdjustSize();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when mouse hovers over the item.")]
	[DefaultValue(null)]
	public Image HoverImage
	{
		get
		{
			return m_Button.HoverImage;
		}
		set
		{
			m_Button.HoverImage = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is disabled.")]
	[DefaultValue(null)]
	public Image DisabledImage
	{
		get
		{
			return m_Button.DisabledImage;
		}
		set
		{
			m_Button.DisabledImage = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is pressed.")]
	[DefaultValue(null)]
	public Image PressedImage
	{
		get
		{
			return m_Button.PressedImage;
		}
		set
		{
			m_Button.PressedImage = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(ePopupSide.Default)]
	[Description("Indicates location of popup in relation to it's parent.")]
	public ePopupSide PopupSide
	{
		get
		{
			return m_Button.PopupSide;
		}
		set
		{
			m_Button.PopupSide = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ButtonItemEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Data")]
	[Description("Collection of sub items.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public virtual SubItemsCollection SubItems => m_Button.SubItems;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether button appears as split button.")]
	public bool SplitButton
	{
		get
		{
			return m_Button.SplitButton;
		}
		set
		{
			m_Button.SplitButton = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
	public bool ShowSubItems
	{
		get
		{
			return m_Button.ShowSubItems;
		}
		set
		{
			m_Button.ShowSubItems = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[DefaultValue(eImagePosition.Left)]
	public eImagePosition ImagePosition
	{
		get
		{
			return m_Button.ImagePosition;
		}
		set
		{
			m_Button.ImagePosition = value;
			InvalidateAutoSize();
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether mouse over fade effect is enabled")]
	public bool FadeEffect
	{
		get
		{
			return m_FadeEffect;
		}
		set
		{
			m_FadeEffect = value;
		}
	}

	internal bool IsFadeEnabled
	{
		get
		{
			if (((Component)(object)this).DesignMode || !BarFunctions.IsOffice2007Style(m_Button.EffectiveStyle) || (m_FadeEffect && NativeFunctions.IsTerminalSession()) || base.IsThemed || TextDrawing.UseTextRenderer)
			{
				return false;
			}
			return m_FadeEffect;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the button mouse over tracking style. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[DefaultValue(eHotTrackingStyle.Default)]
	public virtual eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return m_Button.HotTrackingStyle;
		}
		set
		{
			m_Button.HotTrackingStyle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates the width of the expand part of the button item.")]
	[DefaultValue(12)]
	public virtual int SubItemsExpandWidth
	{
		get
		{
			return m_Button.SubItemsExpandWidth;
		}
		set
		{
			m_Button.SubItemsExpandWidth = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("Indicates text associated with this button..")]
	[Localizable(true)]
	[DefaultValue("")]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("Indicates informational text (tooltip) for the button.")]
	[Localizable(true)]
	public virtual string Tooltip
	{
		get
		{
			return m_Button.Tooltip;
		}
		set
		{
			m_Button.Tooltip = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether mnemonic character assigned to button is processed only if Alt key is pressed")]
	public bool EnableMnemonicWithAltKeyOnly
	{
		get
		{
			return _EnableMnemonicWithAltKeyOnly;
		}
		set
		{
			_EnableMnemonicWithAltKeyOnly = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the button will auto-expand (display pop-up menu or toolbar) when clicked.")]
	public virtual bool AutoExpandOnClick
	{
		get
		{
			return m_Button.AutoExpandOnClick;
		}
		set
		{
			m_Button.AutoExpandOnClick = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
	public bool AutoCheckOnClick
	{
		get
		{
			return m_Button.AutoCheckOnClick;
		}
		set
		{
			m_Button.AutoCheckOnClick = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[DefaultValue(false)]
	public virtual bool Checked
	{
		get
		{
			return m_Button.Checked;
		}
		set
		{
			m_Button.Checked = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShapeTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShapeStringConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[MergableProperty(false)]
	public ShapeDescriptor Shape
	{
		get
		{
			return _Shape;
		}
		set
		{
			if (_Shape != value)
			{
				_Shape = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	public virtual bool FocusCuesEnabled
	{
		get
		{
			return _FocusCuesEnabled;
		}
		set
		{
			_FocusCuesEnabled = value;
			if (((Control)this).Focused)
			{
				((Control)this).Invalidate();
			}
		}
	}

	internal int CornerSize => 2;

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	public string CustomColorName
	{
		get
		{
			return m_Button.CustomColorName;
		}
		set
		{
			m_Button.CustomColorName = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(eButtonColor.BlueWithBackground)]
	[Category("Appearance")]
	[Description("Indicates predefined color of button when Office 2007 style is used.")]
	public eButtonColor ColorTable
	{
		get
		{
			return m_Button.ColorTable;
		}
		set
		{
			if (m_Button.ColorTable != value)
			{
				m_Button.ColorTable = value;
				((Control)this).Invalidate();
			}
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
				InvalidateAutoSize();
				AdjustSize();
			}
		}
	}

	[Localizable(true)]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Layout")]
	[Description("Indicates the mode by which the Button automatically resizes itself. ")]
	public AutoSizeMode AutoSizeMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_AutoSizeMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (m_AutoSizeMode != value)
			{
				m_AutoSizeMode = value;
				InvalidateAutoSize();
				AdjustSize();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates amount of spacing between button image if specified and text.")]
	public virtual int ImageTextSpacing
	{
		get
		{
			return _ImageTextSpacing;
		}
		set
		{
			_ImageTextSpacing = value;
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
			m_Button.ForeColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsCurrentSheduler
	{
		get
		{
			return m_Button.IsCurrentSheduler;
		}
		set
		{
			m_Button.IsCurrentSheduler = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the value returned to the parent form when the button is clicked.")]
	public DialogResult DialogResult
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_DialogResult;
		}
		set
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (Enum.IsDefined(typeof(DialogResult), value))
			{
				m_DialogResult = value;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool Expanded
	{
		get
		{
			return m_Button.Expanded;
		}
		set
		{
			m_Button.Expanded = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Design")]
	[Description("Indicates list of shortcut keys for this button.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsDesigner, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public virtual ShortcutsCollection Shortcuts
	{
		get
		{
			return m_Button.Shortcuts;
		}
		set
		{
			m_Button.Shortcuts = value;
		}
	}

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	ICommand ICommandSource.Command
	{
		get
		{
			return _Command;
		}
		set
		{
			bool flag = false;
			if (_Command != value)
			{
				flag = true;
			}
			if (_Command != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command);
			}
			_Command = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public object CommandParameter
	{
		get
		{
			return _CommandParameter;
		}
		set
		{
			_CommandParameter = value;
		}
	}

	[Description("Occurs when Checked property has changed.")]
	public event EventHandler CheckedChanged;

	public ButtonX()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).IsAccessible = true;
		((Control)this).AccessibleRole = (AccessibleRole)43;
		((Control)this).SetStyle((ControlStyles)4352, false);
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		Style = Style;
	}

	protected virtual ButtonItem CreateButtonItem()
	{
		return new ButtonItem();
	}

	protected override PopupItem CreatePopupItem()
	{
		m_Button = CreateButtonItem();
		m_Button.GlobalItem = false;
		m_Button.Displayed = true;
		m_Button.ContainerControl = this;
		m_Button.ColorTable = eButtonColor.BlueWithBackground;
		m_Button.ButtonStyle = eButtonStyle.ImageAndText;
		m_Button._FitContainer = true;
		m_Button.Style = eDotNetBarStyle.Office2007;
		m_Button.SetOwner(this);
		m_Button.CheckedChanged += OnCheckedChanged;
		return m_Button;
	}

	private void OnCheckedChanged(object sender, EventArgs e)
	{
		if (this.CheckedChanged != null)
		{
			this.CheckedChanged(this, e);
		}
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new ButtonXAccessibleObject(this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void StopFade()
	{
		m_Button.StopFade();
	}

	public void Pulse()
	{
		m_Button.Pulse();
	}

	public void Pulse(int pulseBeatCount)
	{
		m_Button.Pulse(pulseBeatCount);
	}

	public void StopPulse()
	{
		m_Button.StopPulse();
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (!((Control)this).Enabled && IsPulsing)
		{
			StopPulse();
		}
		base.OnEnabledChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageFixedSize()
	{
		return m_Button.ShouldSerializeImageFixedSize();
	}

	internal void InternalPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		if ((((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null) && _CallBasePaintBackground)
		{
			((Control)this).OnPaintBackground(e);
		}
		else
		{
			DisplayHelp.FillRectangle(e.Graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor, Color.Empty);
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Graphics graphics = e.Graphics;
		ColorScheme colorScheme = GetColorScheme();
		if (!base.IsThemed && !BarFunctions.IsOffice2007Style(m_Button.EffectiveStyle))
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
			DisplayHelp.DrawRectangle(graphics, colorScheme.BarDockedBorder, clientRectangle);
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		ItemPaintArgs itemPaintArgs = GetItemPaintArgs(graphics);
		if (base.AntiAlias)
		{
			itemPaintArgs.Graphics.SmoothingMode = (SmoothingMode)4;
			itemPaintArgs.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		IShapeDescriptor buttonShape = GetButtonShape();
		if (!(buttonShape is RoundRectangleShapeDescriptor))
		{
			Rectangle clientRectangle2 = ((Control)this).ClientRectangle;
			clientRectangle2.Y--;
			clientRectangle2.X--;
			clientRectangle2.Width++;
			clientRectangle2.Height++;
			if (buttonShape.CanDrawShape(clientRectangle2))
			{
				GraphicsPath shape = buttonShape.GetShape(clientRectangle2);
				try
				{
					graphics.SetClip(shape);
				}
				finally
				{
					((IDisposable)shape)?.Dispose();
				}
			}
		}
		m_Button.Paint(itemPaintArgs);
		if (IsCurrentSheduler)
		{
			SolidBrush val = new SolidBrush(Color.FromArgb(255, 231, 142, 70));
			try
			{
				graphics.FillRectangle((Brush)(object)val, clientRectangle.X, clientRectangle.Y, StiScale2.XXI(3.0), clientRectangle.Height);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		graphics.SmoothingMode = smoothingMode;
		graphics.TextRenderingHint = textRenderingHint;
		((Control)this).OnPaint(e);
	}

	protected override void OnResize(EventArgs e)
	{
		RecalcLayout();
		((Control)this).OnResize(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		m_Button.Text = ((Control)this).Text;
		InvalidateAutoSize();
		AdjustSize();
		RecalcLayout();
		((Control)this).OnTextChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		base.OnFontChanged(e);
		InvalidateAutoSize();
	}

	protected override void RecalcSize()
	{
		m_Button.Bounds = ((Control)this).ClientRectangle;
		m_Button.RecalcSize();
		m_Button.Bounds = ((Control)this).ClientRectangle;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && !m_Button.Expanded)
		{
			if (m_Button.GetShouldAutoExpandOnClick())
			{
				m_Button.Expanded = true;
			}
			else
			{
				m_Button.SetMouseDown(mouseDown: true);
			}
			((Control)this).Invalidate();
		}
		_IsSpaceKeyDown = (int)e.KeyCode == 32;
		((Control)this).OnKeyDown(e);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && _IsSpaceKeyDown && !m_Button.Expanded)
		{
			m_Button.SetMouseDown(mouseDown: false);
			((Control)this).Invalidate();
			PerformClick();
		}
		_IsSpaceKeyDown = false;
		((Control)this).OnKeyUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		m_Button.InternalMouseEnter();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		m_Button.InternalMouseMove(e);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		m_Button.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		m_Button.InternalMouseHover();
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			if (!((Control)this).Focused && ((Control)this).CanSelect && _FocusOnLeftMouseButtonDown && !((Control)this).Focus())
			{
				return;
			}
			_MouseDownPoint = new Point(e.X, e.Y);
		}
		m_Button.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		m_Button.InternalMouseUp(e);
		if ((int)e.Button == 1048576 && ((Control)this).ClientRectangle.Contains(e.X, e.Y) && !_MouseDownPoint.IsEmpty)
		{
			((Control)this).OnMouseClick(e);
			((Control)this).OnClick((EventArgs)(object)e);
		}
		_MouseDownPoint = Point.Empty;
		((Control)this).OnMouseUp(e);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (m_Button.IsMouseOver)
		{
			m_Button.InternalMouseLeave();
		}
		((Control)this).OnVisibleChanged(e);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (!m_Button.SubItemsRect.IsEmpty)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (m_Button.SubItemsRect.Contains(pt))
			{
				return;
			}
		}
		if (SplitButton && !m_Button.TextDrawRect.IsEmpty)
		{
			Point pt2 = ((Control)this).PointToClient(Control.MousePosition);
			if (m_Button.TextDrawRect.Contains(pt2))
			{
				return;
			}
		}
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val.DialogResult = DialogResult;
		}
		((Control)this).OnClick(e);
		if (ExecuteCommandOnClick)
		{
			ExecuteCommand();
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		m_Button.OnGotFocus();
		((Control)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		m_Button.OnLostFocus();
		((Control)this).OnLostFocus(e);
	}

	internal override ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = base.GetItemPaintArgs(g);
		itemPaintArgs.IsDefaultButton = m_IsDefault && _FocusCuesEnabled;
		return itemPaintArgs;
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		if (((Control)this).CanSelect && Control.IsMnemonic(charCode, ((Control)this).Text) && ((Control)this).Enabled && (!_EnableMnemonicWithAltKeyOnly || (int)Control.ModifierKeys == 262144 || ((Control)this).Focused) && !m_Button.IsUsingTextMarkup && ((Control)this).Focus())
		{
			PerformClick();
			return true;
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	internal IShapeDescriptor GetButtonShape()
	{
		if (Shape != null)
		{
			return Shape;
		}
		if (m_Button.EffectiveStyle == eDotNetBarStyle.Metro)
		{
			return _DefaultMetroButtonShape;
		}
		return _DefaultButtonShape;
	}

	protected override void InvalidateAutoSize()
	{
		m_PreferredSize = Size.Empty;
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Invalid comparison between Unknown and I4
		if (!m_PreferredSize.IsEmpty)
		{
			return m_PreferredSize;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		if (((Control)this).Text.Length == 0 && Image == null)
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		int widthInternal = m_Button.WidthInternal;
		int heightInternal = m_Button.HeightInternal;
		m_Button._FitContainer = false;
		m_Button.RecalcSize();
		Size size = m_Button.Size;
		if (size.Width < ((Control)this).MinimumSize.Width)
		{
			size.Width = ((Control)this).MinimumSize.Width;
		}
		if (size.Height < ((Control)this).MinimumSize.Height)
		{
			size.Height = ((Control)this).MinimumSize.Height;
		}
		if ((int)m_AutoSizeMode == 1)
		{
			if (size.Width < ((Control)this).Size.Width)
			{
				size.Width = ((Control)this).Size.Width;
			}
			if (size.Height < ((Control)this).Size.Height)
			{
				size.Height = ((Control)this).Size.Height;
			}
			if (proposedSize.Width > 0 && proposedSize.Width < 50000 && size.Width < proposedSize.Width)
			{
				size.Width = proposedSize.Width;
			}
			if (proposedSize.Height > 0 && proposedSize.Height < 50000 && size.Height < proposedSize.Height)
			{
				size.Height = proposedSize.Height;
			}
		}
		m_Button._FitContainer = true;
		RecalcSize();
		m_PreferredSize = size;
		return m_PreferredSize;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			if (preferredSize.Width > 0)
			{
				width = preferredSize.Width;
			}
			if (preferredSize.Height > 0)
			{
				height = preferredSize.Height;
			}
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected override void AdjustSize()
	{
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			if (preferredSize.Width > 0 && preferredSize.Height > 0)
			{
				((Control)this).Size = ((Control)this).PreferredSize;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return _TextColor != DefaultTextColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = DefaultTextColor;
	}

	public void NotifyDefault(bool value)
	{
		if (m_IsDefault != value)
		{
			m_IsDefault = value;
			((Control)this).Invalidate();
		}
	}

	public override void PerformClick()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).Enabled)
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				val.DialogResult = DialogResult;
			}
			if (AutoCheckOnClick)
			{
				Checked = !Checked;
			}
			if (ExecuteCommandOnClick)
			{
				ExecuteCommand();
			}
			((Control)this).OnClick(EventArgs.Empty);
		}
	}

	public virtual void Popup(Point p)
	{
		m_Button.Popup(p);
	}

	public virtual void Popup(int x, int y)
	{
		m_Button.Popup(x, y);
	}

	public virtual void ClosePopup()
	{
		m_Button.ClosePopup();
	}

	protected virtual void ExecuteCommand()
	{
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
