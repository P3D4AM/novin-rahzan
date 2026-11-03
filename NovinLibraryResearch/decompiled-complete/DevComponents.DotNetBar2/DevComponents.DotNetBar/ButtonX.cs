using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.ButtonXDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(ButtonX), "ButtonX.ButtonX.ico")]
[DefaultEvent("Click")]
public class ButtonX : PopupItemControl, ICommandSource, IButtonControl
{
	private EventHandler _0E65;

	private ButtonItem ᒎ;

	private ColorScheme լ;

	private DialogResult ᑭ;

	private bool ᑮ;

	private bool ਇ = true;

	private eButtonTextAlignment ޔ = eButtonTextAlignment.Center;

	private Size ᒏ = Size.Empty;

	private bool _ᒐ = true;

	private bool _ᒑ;

	private Point _ᐨ = Point.Empty;

	private bool _ᒒ = true;

	private bool _ᒓ;

	private bool _ߍ = true;

	private static RoundRectangleShapeDescriptor _ᒔ = new RoundRectangleShapeDescriptor(2);

	private static RoundRectangleShapeDescriptor _ᒕ = new RoundRectangleShapeDescriptor(0);

	private ShapeDescriptor __0EB7;

	private bool _ᒖ = true;

	private AutoSizeMode ᒗ = (AutoSizeMode)1;

	private int _ᒘ;

	private static readonly Color ᒙ = Color.Empty;

	private Color __0CD1 = ᒙ;

	private ICommand _૦;

	private object _૧;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected internal ButtonItem ButtonItem => ᒎ;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected bool IsMouseDown => ᒎ.IsMouseDown;

	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	[DefaultValue(true)]
	public bool EnableMarkup
	{
		get
		{
			return ᒎ.EnableMarkup;
		}
		set
		{
			ᒎ.EnableMarkup = value;
		}
	}

	[Browsable(false)]
	public bool IsPulsing => ᒎ.IsPulsing;

	[Description("Indicates whether pulse effect started with Pulse method stops automatically when mouse moves over the button.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool StopPulseOnMouseOver
	{
		get
		{
			return ᒎ.StopPulseOnMouseOver;
		}
		set
		{
			ᒎ.StopPulseOnMouseOver = value;
		}
	}

	[DefaultValue(12)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates pulse speed. The value must be greater than 0 and less than 128.")]
	public int PulseSpeed
	{
		get
		{
			return ᒎ.PulseSpeed;
		}
		set
		{
			ᒎ.PulseSpeed = value;
		}
	}

	[Browsable(true)]
	public Size ImageFixedSize
	{
		get
		{
			return ᒎ.ImageFixedSize;
		}
		set
		{
			ᒎ.ImageFixedSize = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Description("Indicates text alignment. Applies only when button text is not composed using text markup. Default value is center.")]
	[Category("Appearance")]
	[DefaultValue(eButtonTextAlignment.Center)]
	public eButtonTextAlignment TextAlignment
	{
		get
		{
			return ޔ;
		}
		set
		{
			ޔ = value;
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CallBasePaintBackground
	{
		get
		{
			return _ᒐ;
		}
		set
		{
			_ᒐ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FocusOnLeftMouseButtonDown
	{
		get
		{
			return _ᒒ;
		}
		set
		{
			_ᒒ = value;
		}
	}

	private bool ᒌ
	{
		get
		{
			if (!string.IsNullOrEmpty(Symbol))
			{
				return true;
			}
			ᇎ ᇎ2 = ᒎ._0EC5();
			bool result = ᇎ2 != null;
			ᇎ2?.Dispose();
			return result;
		}
	}

	protected virtual bool ExecuteCommandOnClick => true;

	[Description("Indicates color of the Symbol.")]
	[Category("Appearance")]
	public Color SymbolColor
	{
		get
		{
			return ᒎ.SymbolColor;
		}
		set
		{
			ᒎ.SymbolColor = value;
		}
	}

	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[DefaultValue("")]
	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return ᒎ.Symbol;
		}
		set
		{
			ᒎ.Symbol = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(eSymbolSet.Awesome)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return ᒎ.SymbolSet;
		}
		set
		{
			ᒎ.SymbolSet = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	[DefaultValue(0f)]
	public float SymbolSize
	{
		get
		{
			return ᒎ.SymbolSize;
		}
		set
		{
			ᒎ.SymbolSize = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the Button image which is used when button background is black or very dark. Image is used when button text is white. This is provided for Metro style applications.")]
	[DefaultValue(null)]
	public Image ImageAlt
	{
		get
		{
			return ᒎ.ImageAlt;
		}
		set
		{
			ᒎ.ImageAlt = value;
		}
	}

	[Description("The image that will be displayed on the face of the button.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return ᒎ.Image;
		}
		set
		{
			ᒎ.Image = value;
			InvalidateAutoSize();
			AdjustSize();
		}
	}

	[Description("The image that will be displayed when mouse hovers over the item.")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	public Image HoverImage
	{
		get
		{
			return ᒎ.HoverImage;
		}
		set
		{
			ᒎ.HoverImage = value;
		}
	}

	[Browsable(true)]
	[Description("The image that will be displayed when item is disabled.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	public Image DisabledImage
	{
		get
		{
			return ᒎ.DisabledImage;
		}
		set
		{
			ᒎ.DisabledImage = value;
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
			return ᒎ.PressedImage;
		}
		set
		{
			ᒎ.PressedImage = value;
		}
	}

	[DevCoBrowsable(false)]
	[Description("Indicates location of popup in relation to it's parent.")]
	[DefaultValue(ePopupSide.Default)]
	[Browsable(true)]
	public ePopupSide PopupSide
	{
		get
		{
			return ᒎ.PopupSide;
		}
		set
		{
			ᒎ.PopupSide = value;
			InvalidateAutoSize();
		}
	}

	[Category("Data")]
	[Description("Collection of sub items.")]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("DevComponents.DotNetBar.Design.ButtonItemEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	public virtual SubItemsCollection SubItems => ᒎ.SubItems;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether button appears as split button.")]
	public bool SplitButton
	{
		get
		{
			return ᒎ.SplitButton;
		}
		set
		{
			ᒎ.SplitButton = value;
			InvalidateAutoSize();
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Determines whether sub-items are displayed.")]
	[DevCoBrowsable(true)]
	public bool ShowSubItems
	{
		get
		{
			return ᒎ.ShowSubItems;
		}
		set
		{
			ᒎ.ShowSubItems = value;
		}
	}

	[DefaultValue(eImagePosition.Left)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public eImagePosition ImagePosition
	{
		get
		{
			return ᒎ.ImagePosition;
		}
		set
		{
			ᒎ.ImagePosition = value;
			InvalidateAutoSize();
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether mouse over fade effect is enabled")]
	public bool FadeEffect
	{
		get
		{
			return ਇ;
		}
		set
		{
			ਇ = value;
		}
	}

	internal bool স
	{
		get
		{
			if (((Component)(object)this).DesignMode || !BarFunctions.IsOffice2007Style(ᒎ.EffectiveStyle) || (ਇ && _089F.ष()) || base.IsThemed || TextDrawing.UseTextRenderer)
			{
				return false;
			}
			return ਇ;
		}
	}

	[DefaultValue(eHotTrackingStyle.Default)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the button mouse over tracking style. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	public virtual eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return ᒎ.HotTrackingStyle;
		}
		set
		{
			ᒎ.HotTrackingStyle = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(12)]
	[Description("Indicates the width of the expand part of the button item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public virtual int SubItemsExpandWidth
	{
		get
		{
			return ᒎ.SubItemsExpandWidth;
		}
		set
		{
			ᒎ.SubItemsExpandWidth = value;
			RecalcLayout();
		}
	}

	[Description("Indicates text associated with this button..")]
	[Category("Appearance")]
	[Localizable(true)]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
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

	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates informational text (tooltip) for the button.")]
	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	public virtual string Tooltip
	{
		get
		{
			return ᒎ.Tooltip;
		}
		set
		{
			ᒎ.Tooltip = value;
		}
	}

	[Description("Indicates whether mnemonic character assigned to button is processed only if Alt key is pressed")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool EnableMnemonicWithAltKeyOnly
	{
		get
		{
			return _ᒓ;
		}
		set
		{
			_ᒓ = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	[DefaultValue(true)]
	public bool UseMnemonic
	{
		get
		{
			return _ߍ;
		}
		set
		{
			_ߍ = value;
			if (((Control)this).IsHandleCreated)
			{
				ᒛ();
			}
		}
	}

	[Description("Indicates whether the button will auto-expand (display pop-up menu or toolbar) when clicked.")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Browsable(true)]
	public virtual bool AutoExpandOnClick
	{
		get
		{
			return InternalAutoExpandOnClick;
		}
		set
		{
			InternalAutoExpandOnClick = value;
		}
	}

	protected bool InternalAutoExpandOnClick
	{
		get
		{
			return ᒎ.AutoExpandOnClick;
		}
		set
		{
			ᒎ.AutoExpandOnClick = value;
			RecalcLayout();
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
	public bool AutoCheckOnClick
	{
		get
		{
			return ᒎ.AutoCheckOnClick;
		}
		set
		{
			ᒎ.AutoCheckOnClick = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether item is checked or not.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public virtual bool Checked
	{
		get
		{
			return ᒎ.Checked;
		}
		set
		{
			ᒎ.Checked = value;
			((Control)this).Invalidate();
		}
	}

	[TypeConverter("DevComponents.DotNetBar.Design.ShapeStringConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[MergableProperty(false)]
	[Editor("DevComponents.DotNetBar.Design.ShapeTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public ShapeDescriptor Shape
	{
		get
		{
			return __0EB7;
		}
		set
		{
			if (__0EB7 != value)
			{
				__0EB7 = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	[DefaultValue(true)]
	public virtual bool FocusCuesEnabled
	{
		get
		{
			return InternalFocusCuesEnabled;
		}
		set
		{
			InternalFocusCuesEnabled = value;
		}
	}

	protected bool InternalFocusCuesEnabled
	{
		get
		{
			return _ᒖ;
		}
		set
		{
			_ᒖ = value;
			if (((Control)this).Focused)
			{
				((Control)this).Invalidate();
			}
		}
	}

	internal int ଖ => 2;

	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	[DevCoBrowsable(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	public string CustomColorName
	{
		get
		{
			return ᒎ.CustomColorName;
		}
		set
		{
			ᒎ.CustomColorName = value;
			((Control)this).Invalidate();
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(true)]
	[DefaultValue(eButtonColor.BlueWithBackground)]
	[Category("Appearance")]
	[Description("Indicates predefined color of button when Office 2007 style is used.")]
	public eButtonColor ColorTable
	{
		get
		{
			return ᒎ.ColorTable;
		}
		set
		{
			if (ᒎ.ColorTable != value)
			{
				ᒎ.ColorTable = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DefaultValue(false)]
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

	[Description("Indicates the mode by which the Button automatically resizes itself. ")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Localizable(true)]
	[Browsable(true)]
	[Category("Layout")]
	public AutoSizeMode AutoSizeMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᒗ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (ᒗ != value)
			{
				ᒗ = value;
				InvalidateAutoSize();
				AdjustSize();
			}
		}
	}

	[Category("Layout")]
	[Description("Indicates amount of spacing between button image if specified and text.")]
	[DefaultValue(0)]
	[Browsable(true)]
	public virtual int ImageTextSpacing
	{
		get
		{
			return _ᒘ;
		}
		set
		{
			_ᒘ = value;
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

	[Description("Indicates text color.")]
	[Category("Appearance")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			ᒎ.ForeColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Behavior")]
	[Description("Gets or sets the value returned to the parent form when the button is clicked.")]
	public DialogResult DialogResult
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᑭ;
		}
		set
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (Enum.IsDefined(typeof(DialogResult), value))
			{
				ᑭ = value;
			}
		}
	}

	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool Expanded
	{
		get
		{
			return ᒎ.Expanded;
		}
		set
		{
			ᒎ.Expanded = value;
		}
	}

	[TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[DevCoBrowsable(true)]
	[Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates list of shortcut keys for this button.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Design")]
	public virtual ShortcutsCollection Shortcuts
	{
		get
		{
			return ᒎ.Shortcuts;
		}
		set
		{
			ᒎ.Shortcuts = value;
		}
	}

	[Description("Indicates the command assigned to the item.")]
	[Category("Commands")]
	[DefaultValue(null)]
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

	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
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
	[TypeConverter(typeof(StringConverter))]
	[Category("Commands")]
	[Localizable(true)]
	[DefaultValue(null)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
		}
	}

	[Description("Occurs when Checked property has changed.")]
	public event EventHandler CheckedChanged
	{
		add
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ButtonX()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
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
		ᒎ = CreateButtonItem();
		ᒎ.GlobalItem = false;
		ᒎ.Displayed = true;
		ᒎ.ContainerControl = this;
		ᒎ.ColorTable = eButtonColor.BlueWithBackground;
		ᒎ.ButtonStyle = eButtonStyle.ImageAndText;
		ᒎ._ງ = true;
		ᒎ.Style = eDotNetBarStyle.Office2007;
		ᒎ.SetOwner(this);
		ᒎ.CheckedChanged += ᒚ;
		return ᒎ;
	}

	private void ᒚ(object _٠, EventArgs ؿ)
	{
		if (_0E65 != null)
		{
			_0E65(this, ؿ);
		}
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new ButtonXAccessibleObject(this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void StopFade()
	{
		ᒎ._0ECF();
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		base.ScaleControl(factor, specified);
		ᒎ.NotifyScaleItem(factor);
	}

	public void Pulse()
	{
		ᒎ.Pulse();
	}

	public void Pulse(int pulseBeatCount)
	{
		ᒎ.Pulse(pulseBeatCount);
	}

	public void StopPulse()
	{
		ᒎ.StopPulse();
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
		return ᒎ.ShouldSerializeImageFixedSize();
	}

	internal void ઔ(PaintEventArgs ؿ)
	{
		((Control)this).OnPaint(ؿ);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		if ((((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null) && _ᒐ)
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
		if (!base.IsThemed && !BarFunctions.IsOffice2007Style(ᒎ.EffectiveStyle))
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
			DisplayHelp.DrawRectangle(graphics, colorScheme.BarDockedBorder, clientRectangle);
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		ItemPaintArgs itemPaintArgs = _0A92(graphics);
		if (base.AntiAlias)
		{
			itemPaintArgs.Graphics.SmoothingMode = (SmoothingMode)4;
			itemPaintArgs.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		IShapeDescriptor shapeDescriptor = ᒜ();
		if (!(shapeDescriptor is RoundRectangleShapeDescriptor))
		{
			Rectangle clientRectangle2 = ((Control)this).ClientRectangle;
			clientRectangle2.Y--;
			clientRectangle2.X--;
			clientRectangle2.Width++;
			clientRectangle2.Height++;
			if (shapeDescriptor.CanDrawShape(clientRectangle2))
			{
				GraphicsPath shape = shapeDescriptor.GetShape(clientRectangle2);
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
		ᒎ.Paint(itemPaintArgs);
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
		ᒎ.Text = ((Control)this).Text;
		ᒛ();
		((Control)this).OnTextChanged(e);
	}

	private void ᒛ()
	{
		InvalidateAutoSize();
		AdjustSize();
		RecalcLayout();
	}

	protected override void OnFontChanged(EventArgs e)
	{
		base.OnFontChanged(e);
		InvalidateAutoSize();
	}

	protected override void RecalcSize()
	{
		ᒎ.Bounds = ((Control)this).ClientRectangle;
		ᒎ.RecalcSize();
		ᒎ.Bounds = ((Control)this).ClientRectangle;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && !ᒎ.Expanded)
		{
			if (ᒎ._໓())
			{
				ᒎ.Expanded = true;
			}
			else
			{
				ᒎ._໔(_໕: true);
			}
			((Control)this).Invalidate();
		}
		_ᒑ = (int)e.KeyCode == 32;
		((Control)this).OnKeyDown(e);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && _ᒑ && !ᒎ.Expanded)
		{
			ᒎ._໔(_໕: false);
			((Control)this).Invalidate();
			PerformClick();
		}
		_ᒑ = false;
		((Control)this).OnKeyUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		ᒎ.InternalMouseEnter();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		ᒎ.InternalMouseMove(e);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		ᒎ.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		ᒎ.InternalMouseHover();
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			if (!((Control)this).Focused && ((Control)this).CanSelect && _ᒒ && !((Control)this).Focus())
			{
				return;
			}
			_ᐨ = new Point(e.X, e.Y);
		}
		ᒎ.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		ᒎ.InternalMouseUp(e);
		if ((int)e.Button == 1048576 && ((Control)this).ClientRectangle.Contains(e.X, e.Y) && !_ᐨ.IsEmpty)
		{
			((Control)this).OnMouseClick(e);
			((Control)this).OnClick((EventArgs)(object)e);
		}
		_ᐨ = Point.Empty;
		((Control)this).OnMouseUp(e);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (ᒎ.IsMouseOver)
		{
			ᒎ.InternalMouseLeave();
		}
		((Control)this).OnVisibleChanged(e);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		if (!ᒎ.SubItemsRect.IsEmpty)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (ᒎ.SubItemsRect.Contains(pt))
			{
				return;
			}
		}
		if (SplitButton && !ᒎ.TextDrawRect.IsEmpty && ᒌ)
		{
			Point pt2 = ((Control)this).PointToClient(Control.MousePosition);
			if (ᒎ.TextDrawRect.Contains(pt2))
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
		ᒎ.OnGotFocus();
		((Control)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		ᒎ.OnLostFocus();
		((Control)this).OnLostFocus(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return ᒎ.ShouldSerializeSymbolColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		ᒎ.ResetSymbolColor();
	}

	internal override ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = base._0A92(_٥);
		itemPaintArgs._0EE0 = ᑮ && _ᒖ;
		return itemPaintArgs;
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (((Control)this).CanSelect && Control.IsMnemonic(charCode, ((Control)this).Text) && ((Control)this).Enabled && _ߍ && (!_ᒓ || (int)Control.ModifierKeys == 262144 || ((Control)this).Focused) && !ᒎ.IsUsingTextMarkup && ((Control)this).Focus())
		{
			PerformClick();
			return true;
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	internal IShapeDescriptor ᒜ()
	{
		if (Shape != null)
		{
			return Shape;
		}
		if (StyleManager.IsMetro(ᒎ.EffectiveStyle))
		{
			return _ᒕ;
		}
		return _ᒔ;
	}

	protected override void InvalidateAutoSize()
	{
		ᒏ = Size.Empty;
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		if (!ᒏ.IsEmpty)
		{
			return ᒏ;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		if (((Control)this).Text.Length == 0 && Image == null)
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		_ = ᒎ.WidthInternal;
		_ = ᒎ.HeightInternal;
		ᒎ._ງ = false;
		ᒎ.RecalcSize();
		Size size = ᒎ.Size;
		if (size.Width < ((Control)this).MinimumSize.Width)
		{
			size.Width = ((Control)this).MinimumSize.Width;
		}
		if (size.Height < ((Control)this).MinimumSize.Height)
		{
			size.Height = ((Control)this).MinimumSize.Height;
		}
		if ((int)ᒗ == 1)
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
		ᒎ._ງ = true;
		RecalcSize();
		ᒏ = size;
		return ᒏ;
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
		return __0CD1 != ᒙ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = ᒙ;
	}

	public void NotifyDefault(bool value)
	{
		if (ᑮ != value)
		{
			ᑮ = value;
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
		ᒎ.Popup(p);
	}

	public virtual void Popup(int x, int y)
	{
		ᒎ.Popup(x, y);
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
