using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.RibbonBarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ComVisible(false)]
[ToolboxBitmap(typeof(RibbonBar), "Ribbon.RibbonBar.ico")]
public class RibbonBar : ItemControl
{
	private EventHandler ჿ;

	private OverflowButtonEventHandler ᄀ;

	private OverflowButtonEventHandler ᄁ;

	private OverflowButtonEventHandler ᄂ;

	private EventHandler ᄃ;

	private EventHandler ᄄ;

	private EventHandler ᄅ;

	private MouseEventHandler ᄆ;

	private ItemContainer ଡ଼;

	private Rectangle ᄇ = Rectangle.Empty;

	private ElementStyle ᄈ;

	private ElementStyle ᄉ;

	private _23DA ᄊ = new _23DA();

	private bool ᄋ;

	private Rectangle ᄌ = Rectangle.Empty;

	private Image ᄍ;

	private Image ᄎ;

	private bool ᄏ;

	private bool ᄐ = true;

	private _220F ᄑ;

	private RibbonBar ᄒ;

	private string ᄓ = "";

	private Image ᄔ;

	private int ᄕ;

	private bool ᄖ;

	private int ᄗ = 25;

	private Size ᄘ = Size.Empty;

	private bool ݡ;

	private ElementStyle ᄙ = new ElementStyle();

	private eRibbonTitlePosition ᄚ = eRibbonTitlePosition.Bottom;

	private bool ᄛ = true;

	private bool ᄜ;

	private ElementStyle ൿ = new ElementStyle();

	private ElementStyle ᄝ = new ElementStyle();

	private ElementStyle ᄞ = new ElementStyle();

	private ElementStyle ᄟ = new ElementStyle();

	private bool શ = true;

	private ArrayList ᄠ = new ArrayList();

	private bool ᄡ = true;

	private Size ᄢ = Size.Empty;

	private Size ᄣ = Size.Empty;

	private string ᄤ = "";

	private bool ᄥ;

	private GalleryContainer ᄦ;

	private Timer ᄧ;

	private bool ᄨ = true;

	private bool ᄩ;

	private ButtonItem _ᄪ;

	private bool _ᄫ;

	private eVerticalItemsAlignment ᄬ;

	private eOrientation ᄭ;

	private eHorizontalItemsAlignment ᄮ;

	private RibbonBar _ᄯ;

	private bool _ᄰ = true;

	private bool _ᄱ;

	private string _ᄲ = "";

	private Bitmap ᄳ;

	private int ᄴ = 1;

	private int ᄵ;

	private bool _0EB1;

	private ReaderWriterLock າ = new ReaderWriterLock();

	[Browsable(false)]
	internal ButtonItem ჴ
	{
		get
		{
			return _ᄪ;
		}
		set
		{
			_ᄪ = value;
		}
	}

	[Browsable(false)]
	internal bool ჵ
	{
		get
		{
			return _ᄫ;
		}
		set
		{
			_ᄫ = value;
		}
	}

	[Description("Indicates whether Office 2007 Design Guidelines specification for positioning KeyTips is used.")]
	[Category("KeyTips")]
	[DefaultValue(false)]
	public bool UseSpecKeyTipsPositioning
	{
		get
		{
			return ᄩ;
		}
		set
		{
			ᄩ = value;
		}
	}

	[Browsable(true)]
	[Category("Accessibility")]
	[DefaultValue("")]
	[Description("Indicates Accessible name for the Dialog Launcher button.")]
	public string DialogLauncherAccessibleName
	{
		get
		{
			return ᄤ;
		}
		set
		{
			ᄤ = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates mouse over background style.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	public ElementStyle BackgroundMouseOverStyle => ᄙ;

	[Description("Indicates image that is used as dialog luncher button in ribbon title bar.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Browsable(true)]
	public Image DialogLauncherButton
	{
		get
		{
			return ᄍ;
		}
		set
		{
			ᄍ = value;
			ᄾ();
		}
	}

	[Description("Indicates image that is used as dialog launcher button when mouse is over the button.")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Image DialogLauncherMouseOverButton
	{
		get
		{
			return ᄎ;
		}
		set
		{
			ᄎ = value;
		}
	}

	[Browsable(true)]
	[Category("Overflow")]
	[DefaultValue(25)]
	[Description("Indicates maximum text length for automatic overflow button text.")]
	public int MaximumOverflowTextLength
	{
		get
		{
			return ᄗ;
		}
		set
		{
			ᄗ = value;
		}
	}

	[Browsable(true)]
	[Category("Overflow")]
	[Description("Indicates resize order index of the control.")]
	[DefaultValue(0)]
	public int ResizeOrderIndex
	{
		get
		{
			return ᄕ;
		}
		set
		{
			ᄕ = value;
		}
	}

	public bool AutoOverflowEnabled
	{
		get
		{
			return ᄐ;
		}
		set
		{
			if (ᄐ != value)
			{
				ᄐ = value;
				ᄸ();
			}
		}
	}

	[Description("Indicates text for overflow button that is created when ribbon bar size is reduced so it cannot display all its content")]
	[Browsable(true)]
	[DefaultValue("")]
	[Category("Overflow")]
	public string OverflowButtonText
	{
		get
		{
			return ᄓ;
		}
		set
		{
			ᄓ = value;
			ᄹ();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates image for overflow button that is created when ribbon bar size is reduced so it cannot display all its content")]
	[Category("Overflow")]
	public Image OverflowButtonImage
	{
		get
		{
			return ᄔ;
		}
		set
		{
			ᄔ = value;
			ᄹ();
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether dialog launcher button is visible in title of the ribbon.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool DialogLauncherVisible
	{
		get
		{
			return ᄋ;
		}
		set
		{
			ᄋ = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public virtual eOrientation LayoutOrientation
	{
		get
		{
			return ଡ଼.LayoutOrientation;
		}
		set
		{
			ଡ଼.LayoutOrientation = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Description("Indicates spacing in pixels between items.")]
	[Category("Layout")]
	[Browsable(true)]
	[DefaultValue(1)]
	public int ItemSpacing
	{
		get
		{
			return ଡ଼.ItemSpacing;
		}
		set
		{
			ଡ଼.ItemSpacing = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	public bool ResizeItemsToFit
	{
		get
		{
			return ଡ଼.ResizeItemsToFit;
		}
		set
		{
			ଡ଼.ResizeItemsToFit = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Description("Indicates item alignment when container is in horizontal layout.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(eHorizontalItemsAlignment.Left)]
	[Category("Layout")]
	public eHorizontalItemsAlignment HorizontalItemAlignment
	{
		get
		{
			return ଡ଼.HorizontalItemAlignment;
		}
		set
		{
			ଡ଼.HorizontalItemAlignment = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates item item vertical alignment.")]
	[DefaultValue(eVerticalItemsAlignment.Top)]
	[DevCoBrowsable(true)]
	public eVerticalItemsAlignment VerticalItemAlignment
	{
		get
		{
			return ଡ଼.VerticalItemAlignment;
		}
		set
		{
			ଡ଼.VerticalItemAlignment = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items
	{
		get
		{
			if (OverflowState && !((Component)(object)this).DesignMode)
			{
				return ᄒ.Items;
			}
			return ଡ଼.SubItems;
		}
	}

	internal Rectangle ჶ => ᄌ;

	[DefaultValue(true)]
	[Description("Indicates whether ribbon bar can be customized by end user i.e. added to Quick Access Toolbar.")]
	[Browsable(true)]
	[Category("Behavior")]
	public virtual bool CanCustomize
	{
		get
		{
			return શ;
		}
		set
		{
			શ = value;
		}
	}

	internal GalleryContainer ჷ
	{
		get
		{
			return ᄦ;
		}
		set
		{
			ᄦ = value;
		}
	}

	internal Size ჸ => ᄘ;

	internal Size ჹ => ᄣ;

	internal bool ჺ => ᄠ.Count > 0;

	internal bool _10FB
	{
		get
		{
			return ᄥ;
		}
		set
		{
			ᄥ = value;
		}
	}

	internal RibbonBar ჼ
	{
		get
		{
			return _ᄯ;
		}
		set
		{
			_ᄯ = value;
		}
	}

	internal RibbonBar ჽ => ᄒ;

	[Browsable(false)]
	public bool OverflowState
	{
		get
		{
			if (ᄑ != null)
			{
				return true;
			}
			return false;
		}
	}

	[Description("Indicates whether ButtonItem objects hosted on control are resized to reduce the space consumed by ribbon bar when required.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool AutoSizeItems
	{
		get
		{
			return ᄡ;
		}
		set
		{
			ᄡ = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether auto-sizing for RibbonBar includes ribbon bar title width so in case when ribbon bar title width is wider than ribbon bar width the width of the ribbon bar will always be at least the title bar width so complete title text can be displayed.")]
	public bool AutoSizeIncludesTitle
	{
		get
		{
			return _ᄱ;
		}
		set
		{
			_ᄱ = value;
		}
	}

	public override Rectangle DisplayRectangle
	{
		get
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (!ᄇ.IsEmpty && ᄛ)
			{
				if (ᄚ == eRibbonTitlePosition.Top)
				{
					clientRectangle.Y += ᄇ.Height;
					clientRectangle.Height -= ᄇ.Height;
				}
				else
				{
					clientRectangle.Height -= ᄇ.Height;
				}
			}
			return clientRectangle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	public eDotNetBarStyle Style
	{
		get
		{
			return ଡ଼.Style;
		}
		set
		{
			ଡ଼.Style = value;
			base.ColorScheme.ߕ = ଡ଼.EffectiveStyle;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	internal eDotNetBarStyle _൨ => ଡ଼.EffectiveStyle;

	protected override bool NeedsTopLevelKeyTipCanvasParent
	{
		get
		{
			if (DialogLauncherVisible && DialogLauncherKeyTip != "")
			{
				return true;
			}
			return false;
		}
	}

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates KeyTip for the dialog launcher button.")]
	public string DialogLauncherKeyTip
	{
		get
		{
			return _ᄲ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_ᄲ = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether ribbon bar title is visible")]
	[Browsable(true)]
	public bool TitleVisible
	{
		get
		{
			return ᄛ;
		}
		set
		{
			ᄛ = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Description("Specifies the style of the title of the control")]
	public ElementStyle TitleStyle => ᄈ;

	[DevCoBrowsable(true)]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Specifies the style of the title of the control when mouse is over the control.")]
	[Browsable(true)]
	public ElementStyle TitleStyleMouseOver => ᄉ;

	internal Rectangle ჾ => ᄊ.Bounds;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether control changes its background when mouse is over the control.")]
	public bool BackgroundHoverEnabled
	{
		get
		{
			return ᄨ;
		}
		set
		{
			ᄨ = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	[Browsable(true)]
	[Category("Behavior")]
	public override bool AllowExternalDrop
	{
		get
		{
			return base.AllowExternalDrop;
		}
		set
		{
			base.AllowExternalDrop = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
	[Browsable(true)]
	public override bool UseNativeDragDrop
	{
		get
		{
			return base.UseNativeDragDrop;
		}
		set
		{
			base.UseNativeDragDrop = value;
		}
	}

	[Browsable(true)]
	[Description("Specifies whether automatic drag & drop support is enabled.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public virtual bool EnableDragDrop
	{
		get
		{
			return ᄜ;
		}
		set
		{
			ᄜ = value;
		}
	}

	[Description("Occurs when dialog launcher button in title bar is clicked. Use DialogLauncherVisible property to show the button in title bar.")]
	public event EventHandler LaunchDialog
	{
		add
		{
			EventHandler eventHandler = ჿ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ჿ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ჿ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ჿ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when overflow button for control is created because control size is below the minimum size required to display complete content of the control.")]
	public event OverflowButtonEventHandler OverflowButtonSetup
	{
		add
		{
			OverflowButtonEventHandler overflowButtonEventHandler = ᄀ;
			OverflowButtonEventHandler overflowButtonEventHandler2;
			do
			{
				overflowButtonEventHandler2 = overflowButtonEventHandler;
				OverflowButtonEventHandler value2 = (OverflowButtonEventHandler)Delegate.Combine(overflowButtonEventHandler2, value);
				overflowButtonEventHandler = Interlocked.CompareExchange(ref ᄀ, value2, overflowButtonEventHandler2);
			}
			while ((object)overflowButtonEventHandler != overflowButtonEventHandler2);
		}
		remove
		{
			OverflowButtonEventHandler overflowButtonEventHandler = ᄀ;
			OverflowButtonEventHandler overflowButtonEventHandler2;
			do
			{
				overflowButtonEventHandler2 = overflowButtonEventHandler;
				OverflowButtonEventHandler value2 = (OverflowButtonEventHandler)Delegate.Remove(overflowButtonEventHandler2, value);
				overflowButtonEventHandler = Interlocked.CompareExchange(ref ᄀ, value2, overflowButtonEventHandler2);
			}
			while ((object)overflowButtonEventHandler != overflowButtonEventHandler2);
		}
	}

	[Description("Occurs after overflow button setup is complete and all items contained by this control are moved to it.")]
	public event OverflowButtonEventHandler OverflowButtonSetupComplete
	{
		add
		{
			OverflowButtonEventHandler overflowButtonEventHandler = ᄁ;
			OverflowButtonEventHandler overflowButtonEventHandler2;
			do
			{
				overflowButtonEventHandler2 = overflowButtonEventHandler;
				OverflowButtonEventHandler value2 = (OverflowButtonEventHandler)Delegate.Combine(overflowButtonEventHandler2, value);
				overflowButtonEventHandler = Interlocked.CompareExchange(ref ᄁ, value2, overflowButtonEventHandler2);
			}
			while ((object)overflowButtonEventHandler != overflowButtonEventHandler2);
		}
		remove
		{
			OverflowButtonEventHandler overflowButtonEventHandler = ᄁ;
			OverflowButtonEventHandler overflowButtonEventHandler2;
			do
			{
				overflowButtonEventHandler2 = overflowButtonEventHandler;
				OverflowButtonEventHandler value2 = (OverflowButtonEventHandler)Delegate.Remove(overflowButtonEventHandler2, value);
				overflowButtonEventHandler = Interlocked.CompareExchange(ref ᄁ, value2, overflowButtonEventHandler2);
			}
			while ((object)overflowButtonEventHandler != overflowButtonEventHandler2);
		}
	}

	[Description("Occurs before overflow button is destroyed.")]
	public event OverflowButtonEventHandler OverflowButtonDestroy
	{
		add
		{
			OverflowButtonEventHandler overflowButtonEventHandler = ᄂ;
			OverflowButtonEventHandler overflowButtonEventHandler2;
			do
			{
				overflowButtonEventHandler2 = overflowButtonEventHandler;
				OverflowButtonEventHandler value2 = (OverflowButtonEventHandler)Delegate.Combine(overflowButtonEventHandler2, value);
				overflowButtonEventHandler = Interlocked.CompareExchange(ref ᄂ, value2, overflowButtonEventHandler2);
			}
			while ((object)overflowButtonEventHandler != overflowButtonEventHandler2);
		}
		remove
		{
			OverflowButtonEventHandler overflowButtonEventHandler = ᄂ;
			OverflowButtonEventHandler overflowButtonEventHandler2;
			do
			{
				overflowButtonEventHandler2 = overflowButtonEventHandler;
				OverflowButtonEventHandler value2 = (OverflowButtonEventHandler)Delegate.Remove(overflowButtonEventHandler2, value);
				overflowButtonEventHandler = Interlocked.CompareExchange(ref ᄂ, value2, overflowButtonEventHandler2);
			}
			while ((object)overflowButtonEventHandler != overflowButtonEventHandler2);
		}
	}

	[Description("")]
	public event EventHandler DialogLauncherMouseEnter
	{
		add
		{
			EventHandler eventHandler = ᄃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᄃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᄃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᄃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse leaves dialog launcher button.")]
	public event EventHandler DialogLauncherMouseLeave
	{
		add
		{
			EventHandler eventHandler = ᄄ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᄄ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᄄ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᄄ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse hovers over the dialog launcher button.")]
	public event EventHandler DialogLauncherMouseHover
	{
		add
		{
			EventHandler eventHandler = ᄅ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᄅ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᄅ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᄅ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse is pressed over the dialog launcher button.")]
	public event MouseEventHandler DialogLauncherMouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᄆ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᄆ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ᄆ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᄆ, value2, val2);
			}
			while (val != val2);
		}
	}

	public RibbonBar()
	{
		ଡ଼ = new ItemContainer();
		ଡ଼.GlobalItem = false;
		ଡ଼.ContainerControl = this;
		ଡ଼.Stretch = false;
		ଡ଼.Displayed = true;
		ଡ଼.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.ߕ = eDotNetBarStyle.StyleManagerControlled;
		ଡ଼.SetOwner(this);
		ଡ଼.ᆁ(_0BCF: true);
		SetBaseItemContainer(ଡ଼);
		DragDropSupport = true;
		ᄈ = new ElementStyle();
		ᄈ.SetColorScheme(GetColorScheme());
		ᄈ.StyleChanged += _065F;
		ᄉ = new ElementStyle();
		ᄉ.SetColorScheme(GetColorScheme());
		ᄉ.StyleChanged += _065F;
		ᄙ.SetColorScheme(GetColorScheme());
		ᄙ.StyleChanged += ઐ;
		base.ItemAdded += ᅀ;
		base.ItemRemoved += ᄿ;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			base.ItemAdded -= ᅀ;
			base.ItemRemoved -= ᄿ;
			ᄈ.StyleChanged -= _065F;
			ᄈ.Dispose();
			ᄉ.StyleChanged -= _065F;
			ᄉ.Dispose();
			ᄙ.StyleChanged -= ઐ;
			ᄙ.Dispose();
			ൿ.Dispose();
			ᄝ.Dispose();
			ᄞ.Dispose();
			ᄟ.Dispose();
			ᄼ();
			ᅎ();
			if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
			{
				BarUtilities.ᆟ(ref ᄍ);
				BarUtilities.ᆟ(ref ᄔ);
				BarUtilities.ᆟ(ref ᄎ);
			}
		}
		base.Dispose(disposing);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new RibbonBarAccessibleObject(this);
	}

	protected override void OnItemClick(BaseItem item)
	{
		base.OnItemClick(item);
		if (_10FB && ((Control)this).Parent is MenuPanel && item != null && item.AutoCollapseOnClick && item is ButtonItem && !item.Name.StartsWith("sysgallery"))
		{
			ButtonItem buttonItem = item as ButtonItem;
			if (!buttonItem.AutoExpandOnClick)
			{
				CloseOverflowPopup();
			}
		}
	}

	public void CloseOverflowPopup()
	{
		if (_10FB)
		{
			if (((Control)this).Parent is MenuPanel && ((MenuPanel)(object)((Control)this).Parent).ParentItem is _220F { Expanded: not false } _220F2)
			{
				_220F2.Expanded = false;
			}
		}
		else if (OverflowState && ᄑ != null && ᄑ.Expanded)
		{
			ᄑ.Expanded = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundMouseOverStyle()
	{
		ᄙ.StyleChanged -= ઐ;
		ᄙ = new ElementStyle();
		ᄙ.StyleChanged += ઐ;
		((Control)this).Invalidate();
	}

	protected virtual ElementStyle GetBackgroundMouseOverStyle()
	{
		if (!ᄨ)
		{
			return null;
		}
		if (OverflowState && ᄑ.Expanded)
		{
			return GetBackgroundStyle();
		}
		if (ᄙ.Custom)
		{
			ElementStyle elementStyle = ᄝ.Copy();
			elementStyle.ApplyStyle(ᄙ);
			return elementStyle;
		}
		return ᄝ;
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (OverflowState && ᄑ.Expanded && GetRenderer() is Office2007Renderer office2007Renderer)
		{
			return RibbonPredefinedColorSchemes._2104(office2007Renderer.ColorTable.RibbonBar.Expanded);
		}
		if (base.BackgroundStyle.Custom)
		{
			ElementStyle elementStyle = ൿ.Copy();
			elementStyle.ApplyStyle(base.BackgroundStyle);
			return elementStyle;
		}
		return ൿ;
	}

	private ElementStyle ᄶ()
	{
		if (ᄈ.Custom)
		{
			ElementStyle elementStyle = ᄞ.Copy();
			elementStyle.ApplyStyle(ᄈ);
			return elementStyle;
		}
		return ᄞ;
	}

	private ElementStyle ᄷ()
	{
		if (!ᄨ)
		{
			return null;
		}
		if (ᄉ.Custom)
		{
			ElementStyle elementStyle = ᄟ.Copy();
			elementStyle.ApplyStyle(ᄉ);
			return elementStyle;
		}
		return ᄟ;
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		if (GetDesignMode())
		{
			RecalcLayout();
		}
		else if (((Control)this).Parent != null && ((Component)(object)((Control)this).Parent).Site != null && ((Component)(object)((Control)this).Parent).Site.DesignMode)
		{
			RecalcLayout();
		}
	}

	private void ᄸ()
	{
		if (ᄑ != null)
		{
			ᅎ();
		}
		RecalcLayout();
	}

	private void ᄹ()
	{
		if (ᄑ != null)
		{
			ᄑ.Text = ᄓ;
			ᄑ.Image = ᄔ;
		}
	}

	public void DoLaunchDialog()
	{
		InvokeLaunchDialog(new EventArgs());
	}

	protected virtual void InvokeLaunchDialog(EventArgs e)
	{
		if (ჿ != null)
		{
			ჿ(this, e);
		}
	}

	protected override void OnPopupItemRightClick(BaseItem item)
	{
		if (item == null || item.CanCustomize)
		{
			GetRibbonControl()?.ᐆ(item, ᱩ: false);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		base.OnMouseDown(e);
		if (ᄌ.Contains(e.X, e.Y) && !ᄌ.IsEmpty && ᄛ && !OverflowState)
		{
			((Control)this).Invalidate(ᄌ);
			OnDialogLauncherMouseDown(e);
		}
		if ((int)e.Button == 2097152)
		{
			GetRibbonControl()?.ᱧ(this, e.X, e.Y);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		base.OnMouseUp(e);
		if (!ᄌ.Contains(e.X, e.Y) || (int)e.Button != 1048576)
		{
			return;
		}
		if (!ᄌ.IsEmpty)
		{
			((Control)this).Invalidate(ᄌ);
		}
		if (((Control)this).Parent is MenuPanel)
		{
			if (((MenuPanel)(object)((Control)this).Parent).ParentItem is PopupItem popupItem)
			{
				popupItem.ClosePopup();
			}
		}
		else if (((Control)this).Parent is Bar && ((Bar)(object)((Control)this).Parent).BarState == eBarState.Popup && ((Bar)(object)((Control)this).Parent).ParentItem is PopupItem popupItem2)
		{
			popupItem2.ClosePopup();
		}
		InvokeLaunchDialog(new EventArgs());
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		_0ECC(ԝ: true);
		if (((ᄉ != null && ᄉ.Custom) || ᄟ.Custom) && ᄛ && !OverflowState)
		{
			((Control)this).Invalidate(GetTitleRectangle());
		}
		ElementStyle backgroundMouseOverStyle = GetBackgroundMouseOverStyle();
		if (backgroundMouseOverStyle != null && backgroundMouseOverStyle.Custom)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(pt))
		{
			Form val = ((Control)this).FindForm();
			if (val != null && Form.ActiveForm == val)
			{
				ᄺ();
				return;
			}
		}
		_0ECC(ԝ: false);
		ᄽ(ԝ: false);
		if (((ᄉ != null && ᄉ.Custom) || ᄟ.Custom) && ᄛ && !OverflowState)
		{
			((Control)this).Invalidate(GetTitleRectangle());
		}
		ElementStyle backgroundMouseOverStyle = GetBackgroundMouseOverStyle();
		if (backgroundMouseOverStyle != null && backgroundMouseOverStyle.Custom)
		{
			((Control)this).Invalidate();
		}
	}

	private void ᄺ()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (ᄧ == null)
		{
			ᄧ = new Timer();
			ᄧ.Interval = 500;
			ᄧ.Tick += ᄻ;
			ᄧ.Enabled = true;
			ᄧ.Start();
		}
	}

	private void ᄻ(object _٠, EventArgs ؿ)
	{
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (!((Control)this).ClientRectangle.Contains(pt) || !((Control)this).Visible)
		{
			ᄧ.Stop();
			ᄧ.Enabled = false;
			_0ECC(ԝ: false);
			ᄼ();
		}
	}

	private void ᄼ()
	{
		if (ᄧ != null)
		{
			ᄧ.Stop();
			((Component)(object)ᄧ).Dispose();
			ᄧ = null;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		if (ᄋ)
		{
			if (ᄌ.Contains(e.X, e.Y))
			{
				ᄽ(ԝ: true);
			}
			else
			{
				ᄽ(ԝ: false);
			}
		}
	}

	protected override void InternalMouseMove(MouseEventArgs e)
	{
		if (ᄛ && !ᄇ.IsEmpty && ᄇ.Contains(e.X, e.Y))
		{
			if (ଡ଼.HotSubItem != null)
			{
				ଡ଼.InternalMouseLeave();
			}
		}
		else
		{
			base.InternalMouseMove(e);
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		base.OnMouseHover(e);
		if (ᄏ)
		{
			OnDialogLauncherMouseHover(new EventArgs());
		}
	}

	private void ᄽ(bool ԝ)
	{
		if (ᄏ != ԝ)
		{
			ᄏ = ԝ;
			((Control)this).Invalidate(ᄌ);
			if (ԝ)
			{
				OnDialogLauncherMouseEnter(new EventArgs());
			}
			else
			{
				OnDialogLauncherMouseLeave(new EventArgs());
			}
		}
	}

	protected virtual void OnDialogLauncherMouseEnter(EventArgs e)
	{
		if (ᄃ != null)
		{
			ᄃ(this, e);
		}
	}

	protected virtual void OnDialogLauncherMouseLeave(EventArgs e)
	{
		if (ᄄ != null)
		{
			ᄄ(this, e);
		}
	}

	protected virtual void OnDialogLauncherMouseHover(EventArgs e)
	{
		if (ᄅ != null)
		{
			ᄅ(this, e);
		}
	}

	protected virtual void OnDialogLauncherMouseDown(MouseEventArgs e)
	{
		if (ᄆ != null)
		{
			ᄆ.Invoke((object)this, e);
		}
	}

	public override ArrayList GetItems(string ItemName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItems(ItemName);
		}
		return ribbonControl.RibbonStrip.GetItems(ItemName);
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItems(ItemName, itemType);
		}
		return ribbonControl.RibbonStrip.GetItems(ItemName, itemType);
	}

	public override BaseItem GetItem(string ItemName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItem(ItemName);
		}
		return ribbonControl.RibbonStrip.GetItem(ItemName);
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItems(ItemName, itemType, useGlobalName);
		}
		return ribbonControl.RibbonStrip.GetItems(ItemName, itemType, useGlobalName);
	}

	protected override void OnShowKeyTipsChanged()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ShowKeyTips)
		{
			ribbonControl.RibbonStrip.ඟ(this);
		}
		base.OnShowKeyTipsChanged();
	}

	protected override BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys)
	{
		BaseItem itemForMnemonic = base.GetItemForMnemonic(container, charCode, deepScan, stackKeys);
		if (itemForMnemonic == null && OverflowState)
		{
			itemForMnemonic = ᄒ.GetItemForMnemonic(ᄒ.ଡ଼, charCode, deepScan, stackKeys);
		}
		return itemForMnemonic;
	}

	protected override void OnTextChanged(EventArgs e)
	{
		ᄊ.Text = ((Control)this).Text;
		if (ᄑ != null)
		{
			ᄑ.Text = ᅐ();
		}
		RecalcLayout();
		((Control)this).OnTextChanged(e);
	}

	private void ᄾ()
	{
		RecalcLayout();
	}

	private void ඕ()
	{
		ColorScheme colorScheme = GetColorScheme();
		ൿ.SetColorScheme(colorScheme);
		ᄝ.SetColorScheme(colorScheme);
		ᄞ.SetColorScheme(colorScheme);
		ᄞ.HideMnemonic = true;
		ᄟ.SetColorScheme(colorScheme);
		ᄟ.HideMnemonic = true;
		RibbonPredefinedColorSchemes._20FF(ൿ, ᄝ, ᄞ, ᄟ, this);
	}

	private void ᄿ(object _٠, ItemRemovedEventArgs ؿ)
	{
		if (_٠ == ᄦ)
		{
			ᄦ = null;
		}
	}

	private void ᅀ(object _٠, EventArgs ؿ)
	{
		if (_٠ is GalleryContainer)
		{
			GalleryContainer galleryContainer = _٠ as GalleryContainer;
			if (galleryContainer.StretchGallery && ᄦ == null)
			{
				ᄦ = galleryContainer;
			}
		}
	}

	private Size ᅁ()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (ᄛ)
		{
			Rectangle titleRectangle = GetTitleRectangle();
			if (ᄚ == eRibbonTitlePosition.Top)
			{
				titleRectangle.Height++;
				itemContainerRectangle.Y += titleRectangle.Height;
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
			else
			{
				titleRectangle.Height++;
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
		}
		return Dpi.ঔ(itemContainerRectangle.Size);
	}

	protected override void RecalcSize()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		if (!BarFunctions.IsHandleValid((Control)(object)this) || ᄖ)
		{
			return;
		}
		ඕ();
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (ᄛ)
		{
			_28F9 _28F92 = new _28F9();
			ᄊ.Image = ᄍ;
			if (ᄍ != null && ᄋ)
			{
				ᄊ.ImageAlignment = eSimplePartAlignment.FarCenter;
			}
			_28F92._28FA = ᄊ;
			_28F92._074B = ᄶ();
			_28F92._0745 = ((Control)this).Font;
			_28F92._074C = (int)((Control)this).RightToLeft != 1;
			_28F92.ݍ = false;
			_28F92._0749 = ((Control)this).ClientRectangle.X;
			_28F92._074A = ((Control)this).ClientRectangle.Y;
			ᄊ.FixedWidth = ((Control)this).ClientRectangle.Width;
			if (backgroundStyle != null)
			{
				if (backgroundStyle.ݿ)
				{
					ᄊ.FixedWidth -= backgroundStyle.BorderRightWidth;
				}
				if (backgroundStyle.ހ)
				{
					_28F92._074A += backgroundStyle.BorderTopWidth;
				}
			}
			Graphics val = CreateGraphics();
			try
			{
				_28F92.ܓ = val;
				_28FB._28FC(_28F92);
				if (ᄚ == eRibbonTitlePosition.Bottom)
				{
					_28F92._074A = ((Control)this).ClientRectangle.Bottom - ᄊ.Bounds.Height;
					if (backgroundStyle != null && backgroundStyle.ށ)
					{
						_28F92._074A -= backgroundStyle.BorderBottomWidth;
					}
					else if (StyleManager.IsMetro(StyleManager.Style))
					{
						_28F92._074A++;
					}
					_28FB._28FC(_28F92);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			ᄇ = ᄊ.Bounds;
		}
		else
		{
			ᄇ = Rectangle.Empty;
		}
		ᅉ();
		ᄢ = Size.Empty;
		if (((Control)this).Parent is RibbonPanel)
		{
			RibbonControl ribbonControl = GetRibbonControl();
			if (ribbonControl != null && !((Control)ribbonControl).AutoSize)
			{
				ଡ଼.MinimumSize = new Size(0, ᅁ().Height);
			}
		}
		else if (_10FB && !((Control)this).AutoSize)
		{
			ଡ଼.MinimumSize = new Size(0, ᅁ().Height);
		}
		else
		{
			ଡ଼.MinimumSize = Size.Empty;
		}
		base.RecalcSize();
		Size size = ᅑ();
		if (ᅂ(size.Width))
		{
			ᄣ = Size.Empty;
		}
		else
		{
			if (!AutoOverflowEnabled || ((Component)(object)this).DesignMode)
			{
				return;
			}
			if (size.Width > ((Control)this).Width)
			{
				if (ᄠ.Count == 0 && ᄡ && ((Control)this).Width > 1 && !OverflowState)
				{
					ᅆ(size.Width);
					base.RecalcSize();
					Size size2 = ᅑ();
					if (size2.Width > ((Control)this).Width)
					{
						ᅉ();
					}
					else
					{
						ᄣ = size2;
						ᄢ = size;
						size = size2;
					}
				}
				if (!OverflowState && size.Width > ((Control)this).Width)
				{
					ᄘ = size;
					ᅌ();
					base.RecalcSize();
				}
			}
			else if (OverflowState)
			{
				if (ᄘ.IsEmpty)
				{
					ᅎ();
					base.RecalcSize();
					if (ᅑ().Width > ((Control)this).Width)
					{
						ᅌ();
						base.RecalcSize();
					}
				}
				else if (ᄘ.Width <= ((Control)this).Width)
				{
					ᅎ();
					base.RecalcSize();
					if (ᅂ(ᅑ().Width))
					{
						ᄣ = Size.Empty;
					}
				}
				else if (!ᄣ.IsEmpty && ᄣ.Width <= ((Control)this).Width)
				{
					ᅎ();
					ᅆ(ᄣ.Width);
					base.RecalcSize();
					if (ᅂ(ᅑ().Width))
					{
						ᄣ = Size.Empty;
					}
				}
			}
			else
			{
				ᄣ = Size.Empty;
			}
		}
	}

	private bool ᅂ(int ᅃ)
	{
		if (ᄦ == null || !ᄦ.Visible)
		{
			return false;
		}
		int num = ((Control)this).Width - ᅃ;
		if (ᄦ.DisplayRectangle.Width + num >= ᄦ.MinimumSize.Width)
		{
			int width = ᄦ.DisplayRectangle.Width;
			ᄦ.ᥤ = new Size(ᄦ.DisplayRectangle.Width + num, Dpi.Height(ᄦ.DefaultSize.Height));
			ᄦ.RecalcSize();
			int num2 = ᄦ.DisplayRectangle.Width - width;
			ᅄ(ᄦ.Parent, ᄦ.Parent.SubItems.IndexOf(ᄦ) + 1, num2);
			return Math.Abs(num2) >= Math.Abs(num);
		}
		return false;
	}

	private void ᅄ(BaseItem ԟ, int ᅅ, int _066A)
	{
		int count = ԟ.SubItems.Count;
		for (int i = ᅅ; i < count; i++)
		{
			BaseItem baseItem = ԟ.SubItems[i];
			if (baseItem.Visible)
			{
				Rectangle bounds = baseItem.Bounds;
				bounds.Offset(_066A, 0);
				baseItem.Bounds = bounds;
			}
		}
		ԟ.SetDisplayRectangle(new Rectangle(ԟ.DisplayRectangle.X, ԟ.DisplayRectangle.Y, ԟ.DisplayRectangle.Width + _066A, ԟ.DisplayRectangle.Height));
	}

	private void ᅆ(int ᅃ)
	{
		if (ᄠ.Count <= 0)
		{
			if (ᄦ != null)
			{
				ᄦ.ᥤ = new Size(ᄦ.MinimumSize.Width, ᄦ.HeightInternal);
			}
			ᅇ(ଡ଼);
			if (ᄠ.Count > 0)
			{
				_ᄰ = false;
			}
		}
	}

	private void ᅇ(ItemContainer ᅈ)
	{
		if (ᅈ.LayoutOrientation == eOrientation.Vertical)
		{
			foreach (BaseItem subItem in ᅈ.SubItems)
			{
				if (subItem is ButtonItem { ButtonStyle: eButtonStyle.ImageAndText } buttonItem && (buttonItem.ImagePosition == eImagePosition.Left || buttonItem.ImagePosition == eImagePosition.Right))
				{
					_23D7 _23D72 = _2901._2902(buttonItem);
					_23D72.RecordSetting(buttonItem);
					bool globalItem = buttonItem.GlobalItem;
					buttonItem.GlobalItem = false;
					buttonItem.ButtonStyle = eButtonStyle.Default;
					buttonItem.ImageFixedSize = new Size(16, 16);
					buttonItem.UseSmallImage = true;
					buttonItem.GlobalItem = globalItem;
					ᄠ.Add(_23D72);
				}
				else if (subItem is ItemContainer && !(subItem is GalleryContainer))
				{
					ᅇ(subItem as ItemContainer);
				}
			}
			return;
		}
		ArrayList arrayList = new ArrayList();
		foreach (BaseItem subItem2 in ᅈ.SubItems)
		{
			if (subItem2 is ItemContainer && !(subItem2 is GalleryContainer))
			{
				ᅇ(subItem2 as ItemContainer);
			}
			else if (subItem2 is ButtonItem)
			{
				ButtonItem buttonItem2 = subItem2 as ButtonItem;
				if (buttonItem2.ImagePosition == eImagePosition.Top || buttonItem2.ImagePosition == eImagePosition.Bottom)
				{
					arrayList.Add(buttonItem2);
				}
			}
		}
		if (arrayList.Count <= 1)
		{
			return;
		}
		int num = arrayList.Count / 3;
		int num2 = arrayList.Count - num * 3;
		for (int num3 = arrayList.Count - 1; num3 >= num2; num3--)
		{
			ButtonItem buttonItem3 = arrayList[num3] as ButtonItem;
			_23D7 _23D73 = _2901._2902(buttonItem3);
			_23D73.RecordSetting(buttonItem3);
			bool globalItem2 = buttonItem3.GlobalItem;
			buttonItem3.GlobalItem = false;
			buttonItem3.ImagePosition = eImagePosition.Left;
			buttonItem3.ButtonStyle = eButtonStyle.ImageAndText;
			buttonItem3.ImageFixedSize = new Size(16, 16);
			if (buttonItem3.TextMarkupBody != null && buttonItem3.TextMarkupBody._0CBA)
			{
				buttonItem3.Text = ሒ.ሙ(buttonItem3.Text);
			}
			buttonItem3.GlobalItem = globalItem2;
			ᄠ.Add(_23D73);
		}
		_23D7 _23D74 = _2901._2902(ᅈ);
		ᄠ.Add(_23D74);
		_23D74.RecordSetting(ᅈ);
		ᅈ.LayoutOrientation = eOrientation.Vertical;
		ᅈ.MultiLine = true;
	}

	internal void ᅉ()
	{
		if (ᄦ != null)
		{
			ᄦ.ᥤ = Size.Empty;
		}
		if (ᄠ.Count != 0)
		{
			_ᄰ = true;
			_23D7[] array = new _23D7[ᄠ.Count];
			ᄠ.CopyTo(array);
			_23D7[] array2 = array;
			foreach (_23D7 _23D72 in array2)
			{
				_23D72.RestoreSettings();
			}
			ᄠ.Clear();
		}
	}

	private RibbonBar ᅊ()
	{
		return ᅊ(ᅋ: false);
	}

	internal RibbonBar ᅊ(bool ᅋ)
	{
		RibbonBar ribbonBar = new RibbonBar();
		ribbonBar.ShortcutsEnabled = false;
		ribbonBar.AutoOverflowEnabled = false;
		ribbonBar.Style = Style;
		ribbonBar.BackgroundStyle.ApplyStyle(base.BackgroundStyle);
		ribbonBar.BackgroundMouseOverStyle.ApplyStyle(BackgroundMouseOverStyle);
		((Control)ribbonBar).Text = ((Control)this).Text;
		ribbonBar.TitleStyle.ApplyStyle(TitleStyle);
		ribbonBar.TitleStyleMouseOver.ApplyStyle(TitleStyleMouseOver);
		ribbonBar.LayoutOrientation = LayoutOrientation;
		CopyIOwnerEvents(ribbonBar);
		ribbonBar.DialogLauncherVisible = DialogLauncherVisible;
		ribbonBar.ᄆ = ᄆ;
		ribbonBar.ᄃ = ᄃ;
		ribbonBar.ᄅ = ᄅ;
		ribbonBar.ᄄ = ᄄ;
		ribbonBar.DialogLauncherButton = DialogLauncherButton;
		ribbonBar.DialogLauncherMouseOverButton = DialogLauncherMouseOverButton;
		ribbonBar.Images = Images;
		ribbonBar.ImagesLarge = ImagesLarge;
		ribbonBar.ImagesMedium = ImagesMedium;
		ribbonBar.FadeEffect = base.FadeEffect;
		ribbonBar.ჿ = ჿ;
		ribbonBar.ItemSpacing = ItemSpacing;
		ribbonBar.HorizontalItemAlignment = HorizontalItemAlignment;
		ribbonBar._10FB = true;
		ribbonBar.AutoSizeItems = AutoSizeItems;
		ribbonBar.MaximumOverflowTextLength = MaximumOverflowTextLength;
		ribbonBar.VerticalItemAlignment = VerticalItemAlignment;
		if (ᅋ && !BarFunctions.IsHandleValid((Control)(object)this))
		{
			((Control)this).CreateHandle();
			RecalcLayout();
		}
		if (!ᄘ.IsEmpty)
		{
			Size size = ᄘ;
			((Control)ribbonBar).Size = size;
		}
		else
		{
			((Control)ribbonBar).Size = ᅔ();
		}
		ribbonBar.TitleVisible = TitleVisible;
		((Control)ribbonBar).Visible = true;
		return ribbonBar;
	}

	private void ᅌ()
	{
		if (ᄑ != null || ଡ଼.SubItems.Count == 0)
		{
			return;
		}
		ᄖ = true;
		_220F _220F2 = new _220F();
		_220F2.Name = "sysOverflowButton";
		_220F2.Text = ᅐ();
		_220F2._21F1 = this;
		_220F2.Image = ᅏ();
		_220F2.ButtonStyle = eButtonStyle.ImageAndText;
		_220F2.ImagePosition = eImagePosition.Top;
		_220F2.AutoExpandOnClick = true;
		_220F2.PopupType = ePopupType.Menu;
		_220F2.ImagePaddingHorizontal = 16;
		_220F2._0E62 = 12;
		_220F2.SubItemsExpandWidth = 14;
		_220F2.HotTrackingStyle = eHotTrackingStyle.None;
		_220F2.PopupOpen += ᅍ;
		if (ᄀ != null)
		{
			ᄀ(this, new OverflowButtonEventArgs(_220F2));
		}
		ArrayList arrayList = new ArrayList();
		ଡ଼.SubItems.CopyTo(arrayList);
		ଡ଼.SubItems.Clear();
		ଡ଼.SubItems.Add(_220F2);
		ᄒ = ᅊ();
		ᄒ.ჼ = this;
		ᄬ = ଡ଼.VerticalItemAlignment;
		ᄭ = ଡ଼.LayoutOrientation;
		ᄮ = ଡ଼.HorizontalItemAlignment;
		ଡ଼.VerticalItemAlignment = eVerticalItemsAlignment.Top;
		ଡ଼.LayoutOrientation = eOrientation.Horizontal;
		ଡ଼.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		foreach (BaseItem item in arrayList)
		{
			ᄒ.Items.Add(item);
			((IOwner)this).AddShortcutsFromItem(item);
		}
		ItemContainer itemContainer = new ItemContainer();
		itemContainer.Stretch = true;
		ControlContainerItem controlContainerItem = new ControlContainerItem();
		controlContainerItem.AllowItemResize = false;
		itemContainer.SubItems.Add(controlContainerItem);
		controlContainerItem.Control = (Control)(object)ᄒ;
		_220F2.SubItems.Add(itemContainer);
		ଡ଼.RefreshImageSize();
		ᄑ = _220F2;
		if (ᄁ != null)
		{
			ᄁ(this, new OverflowButtonEventArgs(ᄑ));
		}
		ᄖ = false;
	}

	protected override void OnClick(EventArgs e)
	{
		if (ᄑ != null)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (!ᄑ.DisplayRectangle.Contains(pt))
			{
				ᄑ.Expanded = !ᄑ.Expanded;
			}
		}
		base.OnClick(e);
	}

	private void ᅍ(object _٠, PopupOpenEventArgs ؿ)
	{
		if (ᄒ != null)
		{
			ᄒ.BackgroundStyle.ApplyStyle(base.BackgroundStyle);
			ᄒ.BackgroundMouseOverStyle.ApplyStyle(BackgroundMouseOverStyle);
			ᄒ.TitleStyle.ApplyStyle(TitleStyle);
			ᄒ.TitleStyleMouseOver.ApplyStyle(TitleStyleMouseOver);
			if (ShowKeyTips)
			{
				ᄒ.ShowKeyTips = true;
			}
		}
	}

	private void ᅎ()
	{
		if (ᄑ == null)
		{
			return;
		}
		ᄖ = true;
		if (ᄂ != null)
		{
			ᄂ(this, new OverflowButtonEventArgs(ᄑ));
		}
		ᄑ.PopupOpen -= ᅍ;
		ଡ଼.SubItems.Remove(ᄑ);
		ଡ଼.VerticalItemAlignment = ᄬ;
		ଡ଼.LayoutOrientation = ᄭ;
		ଡ଼.HorizontalItemAlignment = ᄮ;
		ArrayList arrayList = new ArrayList();
		ᄒ.Items.CopyTo(arrayList);
		ᄒ.Items.Clear();
		ᄑ.SubItems.Clear();
		foreach (BaseItem item in arrayList)
		{
			((IOwner)this).RemoveShortcutsFromItem(item);
			ଡ଼.SubItems.Add(item);
		}
		if (ᄔ == null)
		{
			ᄑ.Image.Dispose();
			ᄑ.Image = null;
		}
		ᄑ.Dispose();
		((Component)(object)ᄒ).Dispose();
		ᄑ = null;
		ᄒ = null;
		ᄖ = false;
		ᄘ = Size.Empty;
	}

	internal Image ᅏ()
	{
		if (ᄔ == null)
		{
			return (Image)(object)BarFunctions.ই("SystemImages.RibbonOverflow.png");
		}
		return ᄔ;
	}

	private string ᅐ()
	{
		string text = ᄓ;
		if (text == "")
		{
			text = ((Control)this).Text;
			if (ᄗ > 0 && text.Length >= ᄗ)
			{
				text = text.Substring(0, ᄗ) + "...";
			}
			else if (text.IndexOf(' ') > 0)
			{
				text += " <expand/>";
				text = "<div align=\"center\">" + text + "</div>";
			}
		}
		return text;
	}

	protected override void OnResize(EventArgs e)
	{
		if (OverflowState)
		{
			ᄑ.Bounds = GetItemContainerRectangle();
		}
		base.OnResize(e);
	}

	public void ResetCachedContentSize()
	{
		ᄘ = Size.Empty;
	}

	internal Size ᅑ()
	{
		return ᅒ(new Size(ଡ଼.WidthInternal, ଡ଼.HeightInternal));
	}

	private Size ᅒ(Size ᅓ)
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (_ᄱ && _ᄰ)
		{
			ᅓ.Width = Math.Max(ᅓ.Width, ᄊ.TextSize.Width + ((!ᄋ) ? 1 : 16));
		}
		Size result = new Size(((ᅓ.Width > 0) ? ᅓ.Width : 32) + ᝈ.ᝍ(backgroundStyle), ᅓ.Height + ᝈ.ᝏ(backgroundStyle));
		if (ᄛ && !OverflowState)
		{
			result.Height += ᄇ.Height;
		}
		if (BarFunctions.IsOffice2007Style(_൨))
		{
			result.Width++;
			result.Height += 4;
		}
		return result;
	}

	internal Size ᅔ()
	{
		if (!ᄢ.IsEmpty)
		{
			return ᅒ(ᄢ);
		}
		return ᅑ();
	}

	public override void AutoSyncSize()
	{
		if (((Control)this).AutoSize && !OverflowState && !ଡ଼.NeedRecalcSize)
		{
			((Control)this).Height = ᅑ().Height;
		}
	}

	public override int GetAutoSizeHeight()
	{
		return ᅑ().Height;
	}

	public new int GetAutoSizeWidth()
	{
		return ᅑ().Width;
	}

	protected override Rectangle GetItemContainerRectangle()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (ᄛ && !OverflowState)
		{
			Rectangle titleRectangle = GetTitleRectangle();
			if (ᄚ == eRibbonTitlePosition.Top)
			{
				titleRectangle.Height++;
				itemContainerRectangle.Y += titleRectangle.Height;
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
			else
			{
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
		}
		return itemContainerRectangle;
	}

	private Rectangle ᅕ()
	{
		return ((Control)this).ClientRectangle;
	}

	public Rectangle GetTitleRectangle()
	{
		return ᄇ;
	}

	protected override Font GetKeyTipFont()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			RibbonStrip ribbonStrip = ribbonControl.RibbonStrip;
			if (ribbonStrip.KeyTipsFont != null)
			{
				return ribbonStrip.KeyTipsFont;
			}
		}
		return base.GetKeyTipFont();
	}

	internal override void જ(BaseItem ઇ, BaseRenderer ઝ, KeyTipsRendererEventArgs ؿ)
	{
		base.જ(ઇ, ઝ, ؿ);
		if (DialogLauncherVisible && DialogLauncherKeyTip != "")
		{
			string keyTipsKeysStack = ((IKeyTipsControl)this).KeyTipsKeysStack;
			string dialogLauncherKeyTip = DialogLauncherKeyTip;
			if (!(keyTipsKeysStack != "") || dialogLauncherKeyTip.StartsWith(keyTipsKeysStack))
			{
				ؿ.ReferenceObject = this;
				ؿ.KeyTip = dialogLauncherKeyTip;
				Size _၂ = _၁._၂;
				Size size = TextDrawing.MeasureString(ؿ.Graphics, dialogLauncherKeyTip, ؿ.Font);
				size.Width += _၂.Width;
				size.Height += _၂.Height;
				Rectangle bounds = new Rectangle(((Control)this).Width - size.Width, ((Control)this).Height + 4, size.Width, size.Height);
				ؿ.Bounds = bounds;
				ઝ.DrawKeyTips(ؿ);
			}
		}
	}

	protected override Rectangle GetKeyTipCanvasBounds()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			Point point = ((Control)ribbonControl).PointToClient(((Control)this).PointToScreen(Point.Empty));
			return new Rectangle(point.X, point.Y - 8, ((Control)this).Bounds.Width + 12, ((Control)this).Bounds.Height + 20);
		}
		return new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Size _၂ = _၁._၂;
		Size size = TextDrawing.MeasureString(g, keyTip, font);
		size.Width += _၂.Width;
		size.Height += _၂.Height;
		Rectangle rectangle = ᅖ(item);
		Rectangle result = new Rectangle(rectangle.Right - size.Width - 1, rectangle.Bottom - size.Height + size.Height / 3 + 8, size.Width, size.Height);
		if (item == ᄑ)
		{
			result.Y = ((Control)this).Height - 2;
			result.X = rectangle.X + (rectangle.Width - size.Width - 2) / 2;
		}
		else if (ᄩ)
		{
			if ((double)item.HeightInternal * 1.2 >= (double)ଡ଼.HeightInternal || item is GalleryContainer)
			{
				result.Y = ((Control)this).Height - size.Height - 5;
				if (item is GalleryContainer)
				{
					result.X = rectangle.Right - (size.Width - 2) / 2;
				}
				else
				{
					result.X = rectangle.X + (rectangle.Width - size.Width - 2) / 2;
				}
			}
			else if ((double)rectangle.Bottom * 1.2 >= (double)ଡ଼.HeightInternal)
			{
				result.Y = ((Control)this).Height - size.Height - 5;
			}
			else if (item.Parent is ItemContainer && rectangle.Y >= 0 && rectangle.Y <= 12)
			{
				result.Y = 0;
			}
			else if (result.Bottom > ((Control)this).Height)
			{
				result.Y = ((Control)this).Height - size.Height;
			}
			else if ((double)rectangle.Y >= (double)((Control)this).Height * 0.25)
			{
				result.Y = (int)((double)((Control)this).Height * 0.4);
			}
		}
		else if (item is GalleryContainer)
		{
			result.X = rectangle.Right - (size.Width - 2) / 2;
		}
		return result;
	}

	private Rectangle ᅖ(BaseItem _06EC)
	{
		return _06EC.DisplayRectangle;
	}

	protected override bool ProcessMnemonic(BaseItem container, char charCode)
	{
		if (((Control)this).Parent is IKeyTipsControl && !((IKeyTipsControl)((Control)this).Parent).ShowKeyTips)
		{
			return false;
		}
		bool flag = base.ProcessMnemonic(container, charCode);
		if (!flag && DialogLauncherVisible && DialogLauncherKeyTip != "")
		{
			string keyTipsKeysStack = ((IKeyTipsControl)this).KeyTipsKeysStack;
			string text = keyTipsKeysStack + charCode;
			text = text.ToUpper();
			if (DialogLauncherKeyTip.ToUpper() == text)
			{
				DoLaunchDialog();
				flag = true;
			}
			else if (DialogLauncherKeyTip.ToUpper().StartsWith(text))
			{
				((IKeyTipsControl)this).KeyTipsKeysStack += charCode.ToString().ToUpper();
			}
		}
		return flag;
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Invalid comparison between Unknown and I4
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Invalid comparison between Unknown and I4
		bool flag = ݡ;
		bool flag2 = ᄳ != null;
		if (flag2)
		{
			flag = false;
		}
		GraphicsPath val = null;
		ElementStyle elementStyle = GetBackgroundStyle();
		bool flag3 = flag2;
		try
		{
			if (elementStyle != null)
			{
				Rectangle rectangle = ᅕ();
				pa.Graphics.SetClip(rectangle, (CombineMode)0);
				ElementStyle backgroundMouseOverStyle = GetBackgroundMouseOverStyle();
				if (flag && elementStyle != null && backgroundMouseOverStyle != null && backgroundMouseOverStyle.Custom)
				{
					elementStyle = elementStyle.Copy();
					flag3 = true;
					elementStyle.ApplyStyle(backgroundMouseOverStyle);
				}
				ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(elementStyle, pa.Graphics, rectangle, _൨ == eDotNetBarStyle.Office2007);
				ElementStyleDisplay.Paint(elementStyleDisplayInfo);
				pa.Graphics.ResetClip();
				elementStyleDisplayInfo.Bounds = ᅕ();
				elementStyleDisplayInfo.Bounds.X--;
				elementStyleDisplayInfo.Bounds.Width++;
				val = ElementStyleDisplay.GetInsideClip(elementStyleDisplayInfo);
				elementStyleDisplayInfo.Bounds.X++;
				elementStyleDisplayInfo.Bounds.Width--;
			}
			if (val != null)
			{
				pa.Graphics.SetClip(val, (CombineMode)0);
			}
			ᄌ = Rectangle.Empty;
			if (ᄛ && !OverflowState)
			{
				ElementStyle elementStyle2 = ᄶ();
				ElementStyle elementStyle3 = ᄷ();
				if (flag && elementStyle2 != null && elementStyle3 != null && elementStyle3.Custom)
				{
					elementStyle2 = elementStyle2.Copy();
					elementStyle2.ApplyStyle(elementStyle3);
				}
				if (elementStyle2 != null)
				{
					_2906 _29062 = new _2906(elementStyle2, pa.Graphics, ᄊ, ((Control)this).Font, (int)((Control)this).RightToLeft == 1);
					if (ᄋ)
					{
						if (ᄍ == null)
						{
							Rectangle textBounds = ᄊ.TextBounds;
							textBounds.Width -= ᄇ.Height;
							if ((int)((Control)this).RightToLeft == 1)
							{
								textBounds.X += ᄇ.Height;
							}
							_29062._175A = textBounds;
						}
						else if (ᄏ && ᄎ != null)
						{
							ᄊ.Image = ᄎ;
						}
						else
						{
							ᄊ.Image = ᄍ;
						}
					}
					_2908._139A(_29062);
					if (ᄋ && ᄊ.Image == null)
					{
						ᅗ(pa);
					}
					else
					{
						ᄌ = ᄊ.ImageBounds;
					}
				}
			}
			pa.Graphics.ResetClip();
			າ.AcquireReaderLock(-1);
			try
			{
				if (ᄳ != null)
				{
					Graphics graphics = pa.Graphics;
					Rectangle rectangle2 = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
					ColorMatrix val2 = new ColorMatrix();
					val2[3, 3] = (float)ᄵ / 255f;
					ImageAttributes val3 = new ImageAttributes();
					try
					{
						val3.SetColorMatrix(val2);
						graphics.DrawImage((Image)(object)ᄳ, rectangle2, 0, 0, rectangle2.Width, rectangle2.Height, (GraphicsUnit)2, val3);
						return;
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
			}
			finally
			{
				າ.ReleaseReaderLock();
			}
		}
		finally
		{
			if (val != null)
			{
				val.Dispose();
			}
			if (flag3)
			{
				elementStyle?.Dispose();
			}
		}
	}

	private void ᅗ(ItemPaintArgs _0BD2)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Invalid comparison between Unknown and I4
		Rectangle rectangle = new Rectangle(ᄇ.Right - (ᄇ.Height - 5) - 3, ᄇ.Y + 2, ᄇ.Height - 5, ᄇ.Height - 5);
		if ((int)((Control)this).RightToLeft == 1)
		{
			rectangle.X = ᄇ.X;
		}
		Graphics graphics = _0BD2.Graphics;
		if (BarFunctions.IsOffice2007Style(_൨))
		{
			int num = Dpi.Height(Math.Min(15, ᄇ.Height - 1));
			rectangle = new Rectangle(ᄇ.Right - num - 2, ᄇ.Y + (ᄇ.Height - num) / 2, num, num);
			if ((int)((Control)this).RightToLeft == 1)
			{
				rectangle.X = ᄇ.X + 2;
			}
			RibbonBarRendererEventArgs ribbonBarRendererEventArgs = new RibbonBarRendererEventArgs(graphics, rectangle, this);
			if (ᄏ)
			{
				if ((int)Control.MouseButtons == 1048576)
				{
					ribbonBarRendererEventArgs.Pressed = true;
				}
				else
				{
					ribbonBarRendererEventArgs.MouseOver = true;
				}
			}
			GetRenderer().DrawRibbonDialogLauncher(ribbonBarRendererEventArgs);
			ᄌ = rectangle;
			return;
		}
		if (base.AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)0;
		}
		LinearGradientBrush val = DisplayHelp.CreateLinearGradientBrush(rectangle, ControlPaint.LightLight(_0BD2.Colors.BarBackground), _0BD2.Colors.BarBackground, 90f);
		try
		{
			graphics.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Rectangle rectangle2 = new Rectangle(rectangle.X + (int)Math.Ceiling((float)(rectangle.Width - 8) / 2f), rectangle.Bottom - 4, 2, 2);
		graphics.FillRectangle(SystemBrushes.ControlText, rectangle2);
		rectangle2.Offset(rectangle2.Width + 1, 0);
		graphics.FillRectangle(SystemBrushes.ControlText, rectangle2);
		rectangle2.Offset(rectangle2.Width + 1, 0);
		graphics.FillRectangle(SystemBrushes.ControlText, rectangle2);
		if (base.AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		ᄌ = rectangle;
	}

	public void ResetTitleStyle()
	{
		ᄈ.StyleChanged -= _065F;
		ᄈ = new ElementStyle();
		ᄈ.StyleChanged += _065F;
		((Control)this).Invalidate();
	}

	public void ResetTitleStyleMouseOver()
	{
		ᄉ.StyleChanged -= _065F;
		ᄉ = new ElementStyle();
		ᄉ.StyleChanged += _065F;
		((Control)this).Invalidate();
	}

	private void _065F(object _٠, EventArgs ؿ)
	{
		if (((Component)(object)this).DesignMode)
		{
			RecalcLayout();
		}
	}

	private void _0ECC(bool ԝ)
	{
		if (ԝ == ݡ)
		{
			return;
		}
		if (base.স && ((Control)this).Enabled && ((Control)this).Visible && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				າ.AcquireReaderLock(-1);
				try
				{
					flag = ᄳ == null;
				}
				finally
				{
					າ.ReleaseReaderLock();
				}
				if (flag)
				{
					ݡ = true;
					bool isReaderLockHeld = າ.IsReaderLockHeld;
					LockCookie lockCookie = default(LockCookie);
					if (isReaderLockHeld)
					{
						lockCookie = າ.UpgradeToWriterLock(-1);
					}
					else
					{
						າ.AcquireWriterLock(-1);
					}
					try
					{
						ᄳ = _໒();
					}
					finally
					{
						if (isReaderLockHeld)
						{
							າ.DowngradeFromWriterLock(ref lockCookie);
						}
						else
						{
							າ.ReleaseWriterLock();
						}
					}
				}
			}
			catch (ApplicationException)
			{
				flag2 = true;
			}
			ᄴ = 1;
			ݡ = ԝ;
			if (!flag2)
			{
				if (!ݡ)
				{
					ᄴ = -1;
				}
				else
				{
					ᄵ = 10;
				}
				ᦵ.ᦺ(this, _໑);
				_0EB1 = true;
			}
		}
		else
		{
			ݡ = ԝ;
		}
	}

	private void _0ECF()
	{
		if (_0EB1)
		{
			_0EB1 = false;
			ᦵ._0ECF(this, _໑);
			bool flag = false;
			າ.AcquireReaderLock(-1);
			try
			{
				flag = ᄳ != null;
			}
			finally
			{
				າ.ReleaseReaderLock();
			}
			if (flag)
			{
				_໐();
			}
			if (ᄵ > 230)
			{
				ᄵ = 255;
			}
			else if (ᄵ < 0)
			{
				ᄵ = 0;
			}
		}
	}

	private void _໐()
	{
		bool isReaderLockHeld = າ.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = າ.UpgradeToWriterLock(-1);
		}
		else
		{
			າ.AcquireWriterLock(-1);
		}
		try
		{
			((Image)ᄳ).Dispose();
			ᄳ = null;
		}
		finally
		{
			if (isReaderLockHeld)
			{
				າ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				າ.ReleaseWriterLock();
			}
		}
	}

	private void _໑(object _٠, EventArgs ؿ)
	{
		ᄵ += ᄴ * 65;
		if ((ᄴ < 0 && ᄵ <= 0) || (ᄴ > 0 && ᄵ >= 255))
		{
			_0ECF();
		}
		((Control)this).Invalidate();
	}

	private Bitmap _໒()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height, (PixelFormat)925707);
		val.MakeTransparent();
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			bool antiAlias = base.AntiAlias;
			GetColorScheme();
			if (antiAlias)
			{
				val2.SmoothingMode = (SmoothingMode)4;
				val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			ItemPaintArgs pa = _0A92(val2);
			PaintControlBackground(pa);
			return val;
		}
		finally
		{
			val2.Dispose();
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0ECF();
		base.OnHandleDestroyed(e);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible)
		{
			_0ECF();
			if (_10FB)
			{
				((IOwner)this).OnApplicationDeactivate();
			}
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		ColorScheme colorScheme = GetColorScheme();
		ᄈ.SetColorScheme(colorScheme);
		ᄉ.SetColorScheme(colorScheme);
		ᄙ.SetColorScheme(colorScheme);
		ඕ();
		base.OnPaint(e);
		if (_089F.फ() || !_089F.थ)
		{
			e.Graphics.Clear(SystemColors.Control);
		}
	}
}
