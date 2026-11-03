using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.RibbonControlDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[ComVisible(false)]
[ToolboxBitmap(typeof(RibbonControl), "Ribbon.RibbonControl.ico")]
public class RibbonControl : ContainerControl
{
	private const string Ꮰ = "syscustomizepopupmenu";

	private EventHandler _1C25;

	private CustomizeMenuPopupEventHandler Ꮱ;

	private CustomizeMenuPopupEventHandler Ꮲ;

	private CustomizeMenuPopupEventHandler Ꮳ;

	private DotNetBarManager.LocalizeStringEventHandler _09D4;

	private EventHandler _09BC;

	private QatCustomizeDialogEventHandler Ꮴ;

	private QatCustomizeDialogEventHandler Ꮵ;

	private EventHandler Ꮶ;

	private EventHandler _1C26;

	private CancelEventHandler _1C27;

	private EventHandler _1C28;

	private RibbonPopupCloseEventHandler _1C29;

	private EventHandler _1C2A;

	private MarkupLinkClickEventHandler _0D83;

	private EventHandler _1C2B;

	private RibbonStrip ඳ;

	private bool Ӫ = true;

	private bool ฬ = true;

	private Form ท;

	private bool ธ;

	private bool _09FB;

	private ShadowPaintInfo Ꮻ;

	private bool _1C2C = true;

	private bool _1C2D = true;

	private eCategorizeMode Ꮽ;

	private Ꮮ Ꮾ;

	private bool _1C2E;

	private int Ꮿ = 3;

	private Ყ Ᏸ;

	private bool Ᏹ;

	private RibbonLocalization Ᏺ = new RibbonLocalization();

	private Form _1C2F;

	private bool _1C30 = true;

	private bool _1C31;

	private bool _1C32;

	private int _1C33;

	private bool ᰗ;

	private RibbonPanel _1C34;

	private bool Ᏼ = true;

	private ContextMenuBar แ;

	private SubItemsCollection _1C35 = new SubItemsCollection(null);

	public static readonly string SysQatCustomizeItemName = "sysCustomizeQuickAccessToolbar";

	public static readonly string SysQatAddToItemName = "sysAddToQuickAccessToolbar";

	public static readonly string SysQatRemoveFromItemName = "sysRemoveFromQuickAccessToolbar";

	public static readonly string SysQatPlaceItemName = "sysPlaceQuickAccessToolbar";

	public static readonly string SysMinimizeRibbon = "sysMinimizeRibbon";

	public static readonly string SysMaximizeRibbon = "sysMaximizeRibbon";

	public static readonly string SysQatCustomizeLabelName = "sysCustomizeQuickAccessToolbarLabel";

	public static readonly string SysFrequentlyQatNamePart = "sysQatFrequent_";

	private int ਔ;

	private bool _1C36;

	private bool __1C37 = true;

	private eOffice2007ColorScheme _1C38;

	private bool _1C39;

	private Timer _0A04;

	private IntPtr ਅ = IntPtr.Zero;

	private IntPtr ਆ = IntPtr.Zero;

	private bool __13F6;

	private bool __13F7 = true;

	private bool _1C3A;

	private bool _1C3B;

	private int _1C3C;

	private eBackgroundImagePosition __1C3D = eBackgroundImagePosition.TopRight;

	private bool _1C3E;

	private Form __1C3F;

	private int _᱀;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public SubItemsCollection QatFrequentCommands => _1C35;

	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	[DefaultValue(true)]
	public bool KeyTipsEnabled
	{
		get
		{
			return ඳ.KeyTipsEnabled;
		}
		set
		{
			ඳ.KeyTipsEnabled = value;
		}
	}

	internal bool Ꮬ => ((Component)this).DesignMode;

	[Category("Behaviour")]
	[DefaultValue(true)]
	[Description("Indicates whether merge functionality is enabled for the control.")]
	[Browsable(true)]
	public bool AllowMerge
	{
		get
		{
			return _1C30;
		}
		set
		{
			_1C30 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates whether control height is set automatically.")]
	[DefaultValue(false)]
	[Category("Layout")]
	[Browsable(false)]
	public override bool AutoSize
	{
		get
		{
			return _09FB;
		}
		set
		{
			_09FB = value;
			_0A8E();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[Category("Appearance")]
	public bool AntiAlias
	{
		get
		{
			return ඳ.AntiAlias;
		}
		set
		{
			ඳ.AntiAlias = value;
			((Control)this).Invalidate();
		}
	}

	private bool ষ
	{
		get
		{
			if (((Component)this).DesignMode || !CanSupportGlass)
			{
				return false;
			}
			return _0EFE.ষ;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public string TitleText
	{
		get
		{
			return ඳ.TitleText;
		}
		set
		{
			ඳ.TitleText = value;
		}
	}

	[Category("Data")]
	[Description("Indicates Context menu bar associated with the ribbon control which is used as part of Global Items feature.")]
	[DefaultValue(null)]
	public ContextMenuBar GlobalContextMenuBar
	{
		get
		{
			return แ;
		}
		set
		{
			if (แ != null)
			{
				แ.GlobalParentComponent = null;
			}
			แ = value;
			if (แ != null)
			{
				แ.GlobalParentComponent = (Component)(object)this;
			}
		}
	}

	[Category("Caption")]
	[Description("Indicates whether custom caption and quick access toolbar provided by the control is visible.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool CaptionVisible
	{
		get
		{
			return ඳ.CaptionVisible;
		}
		set
		{
			ඳ.CaptionVisible = value;
			if (!value && ((Control)this).FindForm() is RibbonForm ribbonForm)
			{
				ribbonForm.ἃ();
			}
		}
	}

	[Description("Indicates font for the form caption text when CaptionVisible=true.")]
	[Category("Caption")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Font CaptionFont
	{
		get
		{
			return ඳ.CaptionFont;
		}
		set
		{
			ඳ.CaptionFont = value;
		}
	}

	[DefaultValue(0)]
	[Category("Caption")]
	[Description("Indicates explicit height of the caption provided by control")]
	[Browsable(true)]
	public int CaptionHeight
	{
		get
		{
			return ඳ.CaptionHeight;
		}
		set
		{
			ඳ.CaptionHeight = value;
			RecalcLayout();
		}
	}

	[Category("Layout")]
	[DefaultValue(46)]
	[Description("Indicates indent of the ribbon strip.")]
	[Browsable(true)]
	public int RibbonStripIndent
	{
		get
		{
			return ඳ.RibbonStripIndent;
		}
		set
		{
			ඳ.RibbonStripIndent = value;
		}
	}

	[Description("Gets or sets the font used by the ribbon strip.")]
	[Category("Appearance")]
	public Font RibbonStripFont
	{
		get
		{
			return ((Control)ඳ).Font;
		}
		set
		{
			((Control)ඳ).Font = value;
		}
	}

	[Description("Indicates whether mouse over fade effect is enabled")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool FadeEffect
	{
		get
		{
			return ඳ.FadeEffect;
		}
		set
		{
			ඳ.FadeEffect = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
	[DefaultValue(null)]
	public virtual Font KeyTipsFont
	{
		get
		{
			return ඳ.KeyTipsFont;
		}
		set
		{
			ඳ.KeyTipsFont = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Groups")]
	[Editor("DevComponents.DotNetBar.Design.RibbonTabItemGroupCollectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RibbonTabItemGroupCollection TabGroups => ඳ.TabGroups;

	[Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Tab Groups")]
	[DefaultValue(10)]
	public int TabGroupHeight
	{
		get
		{
			return ඳ.TabGroupHeight;
		}
		set
		{
			ඳ.TabGroupHeight = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Category("Tab Groups")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether tab group line that is displayed above the RibbonTabItem objects that have group assigned is visible.")]
	[DefaultValue(false)]
	public bool TabGroupsVisible
	{
		get
		{
			return ඳ.TabGroupsVisible;
		}
		set
		{
			ඳ.TabGroupsVisible = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Groups")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Font DefaultGroupFont
	{
		get
		{
			return ඳ.DefaultGroupFont;
		}
		set
		{
			ඳ.DefaultGroupFont = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets bar background style.")]
	[Browsable(true)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	public ElementStyle BackgroundStyle => ඳ.BackgroundStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RibbonTabItem SelectedRibbonTabItem
	{
		get
		{
			return ඳ.SelectedRibbonTabItem;
		}
		set
		{
			if (value != null)
			{
				value.Checked = true;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public RibbonStrip RibbonStrip => ඳ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => ඳ.Items;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection QuickToolbarItems => ᐂ();

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eDotNetBarStyle EffectiveStyle => ඳ._൨;

	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	[Browsable(false)]
	public eDotNetBarStyle Style
	{
		get
		{
			return ඳ.Style;
		}
		set
		{
			if (value == eDotNetBarStyle.StyleManagerControlled)
			{
				_᱐();
			}
			else
			{
				_᱒(value);
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether control automatically updates padding for the ribbon and RibbonPanel based on style")]
	public bool AutoUpdatePadding
	{
		get
		{
			return __1C37;
		}
		set
		{
			__1C37 = value;
		}
	}

	[DefaultValue(eOffice2007ColorScheme.Blue)]
	[Browsable(false)]
	[Category("Appearance")]
	[Obsolete("This property is obsolete and it does not do anything. It has been replaced by StyleManager")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates the Office 2007 Renderer global Color Table.")]
	public eOffice2007ColorScheme Office2007ColorTable
	{
		get
		{
			if (ඳ.GetRenderer() is Office2007Renderer office2007Renderer)
			{
				return office2007Renderer.ColorTable.InitialColorScheme;
			}
			return eOffice2007ColorScheme.Blue;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public bool IsPopupMode => ᰗ;

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether mouse wheel scrolls through the ribbon tabs.")]
	public bool MouseWheelTabScrollEnabled
	{
		get
		{
			return __13F7;
		}
		set
		{
			__13F7 = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Description("Gets or sets whether control is expanded or not. When control is expanded both the tabs and the tab ribbons are visible.")]
	public bool Expanded
	{
		get
		{
			return Ӫ;
		}
		set
		{
			if (Ӫ != value)
			{
				if (_1C3E || !BarFunctions.IsHandleValid((Control)(object)this))
				{
					_1C3A = true;
					_1C3B = value;
				}
				else
				{
					_1C3A = false;
					Ӫ = value;
					ᱞ();
				}
			}
		}
	}

	[Category("Layout")]
	[Description("Indicates whether control is collapsed when RibbonTabItem is double clicked and expanded when RibbonTabItem is clicked.")]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public virtual bool AutoExpand
	{
		get
		{
			return ඳ._൫;
		}
		set
		{
			ඳ._൫ = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether control is collapsed/expanded when Ctrl+F1 key is clicked.")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Layout")]
	public virtual bool AutoKeyboardExpand
	{
		get
		{
			return ඳ._൬;
		}
		set
		{
			ඳ._൬ = value;
		}
	}

	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	[DefaultValue(null)]
	[Category("Data")]
	[Browsable(true)]
	public ImageList Images
	{
		get
		{
			return ඳ.Images;
		}
		set
		{
			ඳ.Images = value;
		}
	}

	[DefaultValue(null)]
	[Description("ImageList for medium-sized images used on Items.")]
	[Category("Data")]
	[Browsable(true)]
	public ImageList ImagesMedium
	{
		get
		{
			return ඳ.ImagesMedium;
		}
		set
		{
			ඳ.ImagesMedium = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for large-sized images used on Items.")]
	public ImageList ImagesLarge
	{
		get
		{
			return ඳ.ImagesLarge;
		}
		set
		{
			ඳ.ImagesLarge = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the user can give the focus to this control using the TAB key.")]
	[Browsable(true)]
	public bool TabStop
	{
		get
		{
			return ((Control)this).TabStop;
		}
		set
		{
			((Control)this).TabStop = value;
		}
	}

	[DefaultValue(eBackgroundImagePosition.TopRight)]
	[Category("Appearance")]
	[Description("Indicates the position of the BackgroundImage")]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return __1C3D;
		}
		set
		{
			__1C3D = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).BackgroundImageLayout = value;
		}
	}

	[Description("Specifies whether the MDI system buttons are displayed in menu bar when MDI Child window is maximized.")]
	[Category("Run-time Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool MdiSystemItemVisible
	{
		get
		{
			return ฬ;
		}
		set
		{
			if (ฬ == value)
			{
				return;
			}
			ฬ = value;
			if (!((Component)this).DesignMode)
			{
				ᱦ();
				if (!ฬ && ท != null)
				{
					((Control)ท).Resize -= ᱥ;
					((Control)ท).VisibleChanged -= ᱤ;
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool CanSupportGlass
	{
		get
		{
			return ඳ.CanSupportGlass;
		}
		set
		{
			ඳ.CanSupportGlass = value;
		}
	}

	private bool Ꮭ
	{
		get
		{
			if (CanCustomize)
			{
				if (!CaptionVisible)
				{
					return _1C31;
				}
				return true;
			}
			return false;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates Quick Access Toolbar height when positioned below the Ribbon. Default value of 0 indicates auto-height.")]
	[Category("Appearance")]
	public int QuickAccessToolbarBelowRibbonHeight
	{
		get
		{
			return _᱀;
		}
		set
		{
			if (_᱀ == value)
			{
				return;
			}
			if (value < 0)
			{
				value = 0;
			}
			_᱀ = value;
			if (Ꮾ != null)
			{
				int height = ((Control)Ꮾ).Height;
				((Control)Ꮾ).Height = ((_᱀ > 0) ? _᱀ : Ꮾ.GetAutoSizeHeight());
				if (_09FB)
				{
					AutoSyncSize();
				}
				else
				{
					((Control)this).Height = ((Control)this).Height + (((Control)Ꮾ).Height - height);
				}
				((Control)this).Invalidate(true);
			}
		}
	}

	internal Ꮮ Ꮮ => Ꮾ;

	[DefaultValue(true)]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	[Category("Quick Access Toolbar")]
	[Browsable(true)]
	public bool CanCustomize
	{
		get
		{
			return ඳ.CanCustomize;
		}
		set
		{
			ඳ.CanCustomize = value;
		}
	}

	[Description("Indicates whether external implementation for ribbon bar and menu item customization will be used for customizing the ribbon control.")]
	[Browsable(true)]
	[Category("Quick Access Toolbar")]
	[DefaultValue(false)]
	public bool UseExternalCustomization
	{
		get
		{
			return _1C31;
		}
		set
		{
			_1C31 = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether end-user customization of the placement of the Quick Access Toolbar is enabled.")]
	[Category("Quick Access Toolbar")]
	[DefaultValue(true)]
	public bool EnableQatPlacement
	{
		get
		{
			return _1C2D;
		}
		set
		{
			_1C2D = value;
		}
	}

	[Category("Quick Access Toolbar")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether customize dialog is used to customize the quick access toolbar.")]
	public bool UseCustomizeDialog
	{
		get
		{
			return _1C2C;
		}
		set
		{
			_1C2C = value;
		}
	}

	[Description("Indicates categorization mode for the items on Quick Access Toolbar customize dialog box.")]
	[Browsable(true)]
	[DefaultValue(eCategorizeMode.RibbonBar)]
	[Category("Quick Access Toolbar")]
	public eCategorizeMode CategorizeMode
	{
		get
		{
			return Ꮽ;
		}
		set
		{
			Ꮽ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool QatPositionedBelowRibbon
	{
		get
		{
			return _1C2E;
		}
		set
		{
			if (_1C2E != value)
			{
				ᯃ();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string QatLayout
	{
		get
		{
			return ᐓ();
		}
		set
		{
			ᐔ(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool QatLayoutChanged
	{
		get
		{
			return Ᏹ;
		}
		set
		{
			Ᏹ = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Gets system text used by the component..")]
	[Browsable(true)]
	[Category("Localization")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[NotifyParentProperty(true)]
	public RibbonLocalization SystemText => Ᏺ;

	[Description("Occurs when Quick Access Toolbar placement is changed, i.e. below or above the Ribbon.")]
	public event EventHandler QatPlacementChanged
	{
		add
		{
			EventHandler eventHandler = _1C25;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C25, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1C25;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C25, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event CustomizeMenuPopupEventHandler BeforeCustomizeMenuPopup
	{
		add
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮱ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Combine(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮱ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
		remove
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮱ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Remove(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮱ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
	}

	public event CustomizeMenuPopupEventHandler BeforeAddItemToQuickAccessToolbar
	{
		add
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮲ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Combine(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮲ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
		remove
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮲ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Remove(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮲ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
	}

	public event CustomizeMenuPopupEventHandler BeforeRemoveItemFromQuickAccessToolbar
	{
		add
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮳ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Combine(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮳ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
		remove
		{
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler = Ꮳ;
			CustomizeMenuPopupEventHandler customizeMenuPopupEventHandler2;
			do
			{
				customizeMenuPopupEventHandler2 = customizeMenuPopupEventHandler;
				CustomizeMenuPopupEventHandler value2 = (CustomizeMenuPopupEventHandler)Delegate.Remove(customizeMenuPopupEventHandler2, value);
				customizeMenuPopupEventHandler = Interlocked.CompareExchange(ref Ꮳ, value2, customizeMenuPopupEventHandler2);
			}
			while ((object)customizeMenuPopupEventHandler != customizeMenuPopupEventHandler2);
		}
	}

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString
	{
		add
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Combine(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
		remove
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Remove(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
	}

	[Description("Occurs when Item on ribbon tab strip or quick access toolbar is clicked.")]
	public event EventHandler ItemClick
	{
		add
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event QatCustomizeDialogEventHandler BeforeQatCustomizeDialog
	{
		add
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮴ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Combine(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮴ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
		remove
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮴ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Remove(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮴ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
	}

	public event QatCustomizeDialogEventHandler AfterQatCustomizeDialog
	{
		add
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮵ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Combine(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮵ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
		remove
		{
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler = Ꮵ;
			QatCustomizeDialogEventHandler qatCustomizeDialogEventHandler2;
			do
			{
				qatCustomizeDialogEventHandler2 = qatCustomizeDialogEventHandler;
				QatCustomizeDialogEventHandler value2 = (QatCustomizeDialogEventHandler)Delegate.Remove(qatCustomizeDialogEventHandler2, value);
				qatCustomizeDialogEventHandler = Interlocked.CompareExchange(ref Ꮵ, value2, qatCustomizeDialogEventHandler2);
			}
			while ((object)qatCustomizeDialogEventHandler != qatCustomizeDialogEventHandler2);
		}
	}

	public event EventHandler AfterQatDialogChangesApplied
	{
		add
		{
			EventHandler eventHandler = Ꮶ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮶ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ꮶ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮶ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler SelectedRibbonTabChanged
	{
		add
		{
			EventHandler eventHandler = _1C26;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C26, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1C26;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C26, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before selected RibbonPanel is displayed on popup while ribbon is collapsed.")]
	public event CancelEventHandler BeforeRibbonPanelPopup
	{
		add
		{
			CancelEventHandler cancelEventHandler = _1C27;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _1C27, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _1C27;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _1C27, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs after selected RibbonPanel is displayed on popup while ribbon is collapsed.")]
	public event EventHandler AfterRibbonPanelPopup
	{
		add
		{
			EventHandler eventHandler = _1C28;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C28, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1C28;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C28, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before RibbonPanel popup is closed.")]
	public event RibbonPopupCloseEventHandler BeforeRibbonPanelPopupClose
	{
		add
		{
			RibbonPopupCloseEventHandler ribbonPopupCloseEventHandler = _1C29;
			RibbonPopupCloseEventHandler ribbonPopupCloseEventHandler2;
			do
			{
				ribbonPopupCloseEventHandler2 = ribbonPopupCloseEventHandler;
				RibbonPopupCloseEventHandler value2 = (RibbonPopupCloseEventHandler)Delegate.Combine(ribbonPopupCloseEventHandler2, value);
				ribbonPopupCloseEventHandler = Interlocked.CompareExchange(ref _1C29, value2, ribbonPopupCloseEventHandler2);
			}
			while ((object)ribbonPopupCloseEventHandler != ribbonPopupCloseEventHandler2);
		}
		remove
		{
			RibbonPopupCloseEventHandler ribbonPopupCloseEventHandler = _1C29;
			RibbonPopupCloseEventHandler ribbonPopupCloseEventHandler2;
			do
			{
				ribbonPopupCloseEventHandler2 = ribbonPopupCloseEventHandler;
				RibbonPopupCloseEventHandler value2 = (RibbonPopupCloseEventHandler)Delegate.Remove(ribbonPopupCloseEventHandler2, value);
				ribbonPopupCloseEventHandler = Interlocked.CompareExchange(ref _1C29, value2, ribbonPopupCloseEventHandler2);
			}
			while ((object)ribbonPopupCloseEventHandler != ribbonPopupCloseEventHandler2);
		}
	}

	[Description("Occurs after RibbonPanel popup is closed.")]
	public event EventHandler AfterRibbonPanelPopupClose
	{
		add
		{
			EventHandler eventHandler = _1C2A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1C2A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	[Description("Occurs after Expanded property has changed.")]
	public event EventHandler ExpandedChanged
	{
		add
		{
			EventHandler eventHandler = _1C2B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1C2B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public RibbonControl()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		_ = GlobalManager.Renderer;
		_1C35._0AF0 = true;
		_1C35.AllowParentRemove = false;
		((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2 | 4), true);
		ඳ = new RibbonStrip();
		((Control)ඳ).Dock = (DockStyle)1;
		((Control)ඳ).Height = 32;
		ඳ.ItemAdded += _1C4A;
		((Control)ඳ).SizeChanged += _᱉;
		ඳ.ItemRemoved += _1C4B;
		ඳ.LocalizeString += _᱃;
		ඳ.ItemClick += _᱂;
		ඳ.ButtonCheckedChanged += _᱁;
		ඳ.TitleTextMarkupLinkClick += ᱏ;
		((Control)this).Controls.Add((Control)(object)ඳ);
		TabStop = false;
		((ScrollableControl)this).DockPadding.Bottom = Ꮿ;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		ᱠ();
		((ContainerControl)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		_᱐();
	}

	protected virtual void OnSelectedRibbonTabChanged(EventArgs e)
	{
		if (_1C26 != null)
		{
			_1C26(this, e);
		}
	}

	private void _᱁(object _٠, EventArgs ؿ)
	{
		if (_٠ is RibbonTabItem && ((RibbonTabItem)_٠).Checked)
		{
			AutoSyncSize();
			OnSelectedRibbonTabChanged(new EventArgs());
		}
	}

	private void _᱂(object _٠, EventArgs ؿ)
	{
		if (_09BC != null)
		{
			_09BC(_٠, ؿ);
		}
	}

	private void _᱃(object _٠, LocalizeEventArgs ؿ)
	{
		if (_09D4 != null)
		{
			_09D4(this, ؿ);
		}
	}

	private void _0A8E()
	{
		AutoSyncSize();
	}

	public virtual void AutoSyncSize()
	{
		_᱄(_᱅: true);
	}

	private void _᱄(bool _᱅)
	{
		if (!_09FB || ਔ > 0 || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		ਔ++;
		try
		{
			_᱇(_᱅);
		}
		finally
		{
			ਔ--;
		}
	}

	private int _᱆(ItemControl _0BDE)
	{
		int num = _0BDE.GetAutoSizeHeight();
		if (!((Control)_0BDE).MinimumSize.IsEmpty && ((Control)_0BDE).MinimumSize.Height > num)
		{
			num = ((Control)_0BDE).MinimumSize.Height + ᝈ.ᝏ(_0BDE.એ());
		}
		else if (!((Control)_0BDE).MaximumSize.IsEmpty && num > ((Control)_0BDE).MaximumSize.Height)
		{
			num = ((Control)_0BDE).MaximumSize.Height + ᝈ.ᝏ(_0BDE.એ());
		}
		return num;
	}

	private void _᱇()
	{
		_᱇(_᱅: true);
	}

	private int _᱈(RibbonPanel Ꮔ)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		int num = 0;
		if (Ꮔ == null)
		{
			return num;
		}
		int num2 = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)Ꮔ).Controls)
		{
			Control val = item;
			if (!(val is ItemControl itemControl))
			{
				continue;
			}
			int autoSizeHeight = itemControl.GetAutoSizeHeight();
			if (itemControl != null && autoSizeHeight > num2)
			{
				int num3 = _᱆(itemControl);
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
		}
		if (num2 > 0)
		{
			num += num2;
		}
		return num;
	}

	private void _᱇(bool _᱅)
	{
		int height = ((Control)ඳ).Height;
		if (Expanded)
		{
			if (SelectedRibbonTabItem != null)
			{
				RibbonPanel panel = SelectedRibbonTabItem.Panel;
				if (panel != null)
				{
					int num = _᱈(panel);
					height = ((num > 0) ? (height + num) : 0);
				}
				else
				{
					height = 0;
				}
			}
			else
			{
				height = 0;
			}
		}
		else
		{
			height -= 4;
		}
		if (height > 0)
		{
			height += ((ScrollableControl)this).DockPadding.Bottom + ((ScrollableControl)this).DockPadding.Top;
			if (_1C2E)
			{
				height += ((Control)Ꮾ).Height;
			}
		}
		if (((Control)this).Height == height || height <= 0)
		{
			return;
		}
		RibbonPanel ribbonPanel = null;
		if (!_᱅)
		{
			RibbonTabItem selectedRibbonTabItem = SelectedRibbonTabItem;
			if (selectedRibbonTabItem != null && selectedRibbonTabItem.Panel != null)
			{
				ribbonPanel = selectedRibbonTabItem.Panel;
				((Control)ribbonPanel).SuspendLayout();
			}
		}
		((Control)this).Height = height;
		if (ribbonPanel != null)
		{
			((Control)ribbonPanel).ResumeLayout(false);
		}
	}

	internal Office2007ColorTable ጪ()
	{
		if (GlobalManager.Renderer is Office2007Renderer office2007Renderer)
		{
			return office2007Renderer.ColorTable;
		}
		return new Office2007ColorTable();
	}

	public void SetRibbonPanelStyle(RibbonPanel panel)
	{
		if (((Component)this).DesignMode)
		{
			TypeDescriptor.GetProperties(((ScrollableControl)panel).DockPadding)["Left"].SetValue(((ScrollableControl)panel).DockPadding, 3);
			TypeDescriptor.GetProperties(((ScrollableControl)panel).DockPadding)["Right"].SetValue(((ScrollableControl)panel).DockPadding, 3);
			TypeDescriptor.GetProperties(((ScrollableControl)panel).DockPadding)["Bottom"].SetValue(((ScrollableControl)panel).DockPadding, 3);
		}
		else
		{
			((ScrollableControl)panel).DockPadding.Left = 3;
			((ScrollableControl)panel).DockPadding.Right = 3;
			((ScrollableControl)panel).DockPadding.Bottom = 3;
		}
	}

	public RibbonTabItem CreateRibbonTab(string text, string name, int insertPosition)
	{
		RibbonTabItem ribbonTabItem = new RibbonTabItem();
		ribbonTabItem.Text = text;
		ribbonTabItem.Name = name;
		RibbonPanel ribbonPanel = new RibbonPanel();
		((Control)ribbonPanel).Dock = (DockStyle)5;
		((Control)ribbonPanel).Width = ((Control)this).Width - 4;
		SetRibbonPanelStyle(ribbonPanel);
		((Control)this).Controls.Add((Control)(object)ribbonPanel);
		((Control)ribbonPanel).SendToBack();
		ribbonTabItem.Panel = ribbonPanel;
		if (insertPosition < 0)
		{
			insertPosition = Items.Count;
			for (int i = 0; i < Items.Count; i++)
			{
				if (Items[i].ItemAlignment == eItemAlignment.Far)
				{
					insertPosition = i;
					break;
				}
			}
			if (insertPosition >= Items.Count)
			{
				Items.Add(ribbonTabItem);
			}
			else
			{
				Items.Insert(insertPosition, ribbonTabItem);
			}
		}
		else if (insertPosition > Items.Count - 1)
		{
			insertPosition = Items.Count;
			for (int j = 0; j < Items.Count; j++)
			{
				if (Items[j].ItemAlignment == eItemAlignment.Far)
				{
					insertPosition = j;
					break;
				}
			}
			if (insertPosition >= Items.Count)
			{
				Items.Add(ribbonTabItem);
			}
			else
			{
				Items.Insert(insertPosition, ribbonTabItem);
			}
		}
		else
		{
			Items.Insert(insertPosition, ribbonTabItem);
		}
		return ribbonTabItem;
	}

	public RibbonTabItem CreateRibbonTab(string text, string name)
	{
		return CreateRibbonTab(text, name, -1);
	}

	public void RecalcLayout()
	{
		ඳ.RecalcLayout();
		if (ඳ.SelectedRibbonTabItem != null && ඳ.SelectedRibbonTabItem.Panel != null)
		{
			((Control)ඳ.SelectedRibbonTabItem.Panel).PerformLayout();
		}
		if (Ꮾ != null && ((Control)Ꮾ).Visible)
		{
			Ꮾ.RecalcLayout();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((Control)this).OnHandleCreated(e);
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			_ = val.Handle;
			if (!(val is RibbonPanel))
			{
				continue;
			}
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				_ = val2.Handle;
			}
		}
		ᱎ();
		RecalcLayout();
		if (_1C3A)
		{
			Expanded = _1C3B;
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode || !(e.Control is RibbonPanel ribbonPanel))
		{
			return;
		}
		if (ribbonPanel.RibbonTabItem != null)
		{
			if (Items.Contains(ribbonPanel.RibbonTabItem) && SelectedRibbonTabItem == ribbonPanel.RibbonTabItem)
			{
				ribbonPanel.Visible = true;
				((Control)ribbonPanel).BringToFront();
			}
			else
			{
				ribbonPanel.Visible = false;
			}
		}
		else
		{
			ribbonPanel.Visible = false;
		}
	}

	private void _᱉(object _٠, EventArgs ؿ)
	{
		if (!_1C36 && _09FB)
		{
			AutoSyncSize();
		}
		_1C36 = false;
		if (!Ӫ && !_09FB)
		{
			((Control)this).Height = ᱟ();
		}
	}

	private void _1C4A(object _٠, EventArgs ؿ)
	{
		if (((Component)this).DesignMode || !(_٠ is RibbonTabItem))
		{
			return;
		}
		RibbonTabItem ribbonTabItem = _٠ as RibbonTabItem;
		if (ribbonTabItem.Panel != null)
		{
			if (((Control)this).Controls.Contains((Control)(object)ribbonTabItem.Panel) && ribbonTabItem.Checked)
			{
				ribbonTabItem.Panel.Visible = true;
				((Control)ribbonTabItem.Panel).BringToFront();
			}
			else
			{
				ribbonTabItem.Panel.Visible = false;
			}
			ribbonTabItem.CheckedChanged += _1C4C;
		}
	}

	private void _1C4B(object _٠, ItemRemovedEventArgs ؿ)
	{
		if (_٠ is RibbonTabItem)
		{
			((RibbonTabItem)_٠).CheckedChanged -= _1C4C;
		}
	}

	private void _1C4C(object _٠, EventArgs ؿ)
	{
		if (_٠ is RibbonTabItem && ((RibbonTabItem)_٠).Checked)
		{
			AutoSyncSize();
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (!ᰗ && e.Control is RibbonPanel { RibbonTabItem: not null } ribbonPanel && Items.Contains(ribbonPanel.RibbonTabItem))
		{
			Items.Remove(ribbonPanel.RibbonTabItem);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		ඳ.SetDesignMode(mode: true);
	}

	private ElementStyle _1400()
	{
		return ඳ.ඖ();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		ElementStyle elementStyle = _1400();
		if ((elementStyle.BackColor.A < byte.MaxValue && !elementStyle.BackColor.IsEmpty) || ((Control)this).BackColor == Color.Transparent)
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		else
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (ষ && ((Control)this).FindForm() is RibbonForm ribbonForm)
		{
			e.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, ribbonForm.Ủ), (CombineMode)4);
		}
		Office2007ColorTable office2007ColorTable = ጪ();
		if (office2007ColorTable != null)
		{
			SolidBrush val2 = new SolidBrush(office2007ColorTable.Form.BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		else
		{
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(elementStyle, e.Graphics, ((Control)this).ClientRectangle);
			ElementStyleDisplay.PaintBackground(e2);
		}
		if (!_1C2E && ඳ._0D63 && !StyleManager.IsMetro(EffectiveStyle))
		{
			ShadowPaintInfo shadowPaintInfo = ᱍ();
			shadowPaintInfo.Rectangle = new Rectangle(-6, 0, ((Control)this).Bounds.Width + 12, ((Control)this).Bounds.Height);
			shadowPaintInfo.Graphics = e.Graphics;
			ShadowPainter.Paint3(shadowPaintInfo, 2);
		}
	}

	private ShadowPaintInfo ᱍ()
	{
		if (Ꮻ == null)
		{
			Ꮻ = new ShadowPaintInfo();
		}
		Ꮻ.Size = 3;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		Ꮻ.IsSquare = effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(effectiveStyle);
		return Ꮻ;
	}

	protected override void OnResize(EventArgs e)
	{
		_1C36 = true;
		ᱎ();
		((Control)this).OnResize(e);
		AutoSyncSize();
	}

	protected override void NotifyInvalidate(Rectangle invalidatedArea)
	{
		((Control)this).NotifyInvalidate(invalidatedArea);
		ᱎ();
	}

	private void ᱎ()
	{
		if (((Control)this).Parent is RibbonForm && ඳ.CaptionVisible && !ষ)
		{
			((Control)this).Region = GetControlRegion();
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Invalid comparison between Unknown and I4
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).Msg == 132)
		{
			int x = _0EFE._0F0B(((Message)(ref m)).LParam);
			int y = _0EFE._0F0D(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (ষ && CaptionVisible)
			{
				Rectangle rectangle = new Rectangle(((Control)this).Width - BarFunctions.CaptionButtonSize.Width * 3, 0, BarFunctions.CaptionButtonSize.Width * 3, BarFunctions.CaptionButtonSize.Height);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (ඳ != null && !ඳ._൩ && new Rectangle(0, 0, ((Control)this).Width, Dpi.Height4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (BarFunctions.IsWindows7 && ඳ != null && ඳ._൩ && ඳ._0D65.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			else if (CaptionVisible && ඳ != null && !ඳ._൩ && new Rectangle(0, 0, ((Control)this).Width, Dpi.Height4).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if ((EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(EffectiveStyle)) && ((pt.X < Dpi.Width28 && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - Dpi.Width28 && (int)((Control)this).RightToLeft == 1)) && pt.Y < Dpi.Height28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (EffectiveStyle == eDotNetBarStyle.Windows7 && ((pt.X < Dpi.Width28 && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - Dpi.Width28 && (int)((Control)this).RightToLeft == 1)) && pt.Y < Dpi.Height28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		((ContainerControl)this).WndProc(ref m);
	}

	protected virtual Region GetControlRegion()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).Parent is RibbonForm && (int)((Form)((Control)this).Parent).WindowState == 2)
		{
			return null;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (StyleManager.IsMetro(effectiveStyle))
		{
			return null;
		}
		GraphicsPath val = new GraphicsPath();
		Rectangle ശ = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (effectiveStyle == eDotNetBarStyle.Office2010 && !BarFunctions.IsWindows8)
		{
			ശ.Y++;
		}
		bool flag = (int)((Control)this).RightToLeft == 1;
		int num = 2;
		int num2 = 2;
		if (((Control)this).Parent is RibbonForm)
		{
			RibbonForm ribbonForm = ((Control)this).Parent as RibbonForm;
			num = ribbonForm.TopLeftCornerSize - 2;
			num2 = ribbonForm.TopRightCornerSize - 1;
			if (num <= 0)
			{
				num = 0;
			}
			if (num2 <= 0)
			{
				num = 0;
			}
			if (flag)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			if (num > 0)
			{
				ᒇ ᒇ2 = ElementStyleDisplay.സ(ശ, num, eCornerArc.TopLeft);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
			}
			else
			{
				val.AddLine(ശ.X, ശ.Y + 2, ശ.X, ശ.Y);
				val.AddLine(ശ.X, ശ.Y, ശ.X + 2, ശ.Y);
			}
			if (num2 > 0)
			{
				ᒇ ᒇ2 = ElementStyleDisplay.സ(ശ, num2, eCornerArc.TopRight);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
			}
			else
			{
				val.AddLine(ശ.Right - 2, ശ.Y, ശ.Right, ശ.Y);
				val.AddLine(ശ.Right, ശ.Y, ശ.Right, ശ.Y + 2);
			}
			val.AddLine(ശ.Right, ശ.Bottom - 2, ശ.Right, ശ.Bottom);
			val.AddLine(ശ.Right, ശ.Bottom, ശ.Right - 2, ശ.Bottom);
			val.AddLine(ശ.X + 2, ശ.Bottom, ശ.X, ശ.Bottom);
			val.AddLine(ശ.X, ശ.Bottom, ശ.X, ശ.Bottom - 2);
			val.CloseAllFigures();
			Region val2 = new Region();
			val2.MakeEmpty();
			val2.Union(val);
			val.Widen(SystemPens.Control);
			new Region(val);
			val2.Union(val);
			return val2;
		}
		return null;
	}

	private void ᱏ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		if (_0D83 != null)
		{
			_0D83(this, new MarkupLinkClickEventArgs(ؿ.Name, ؿ.HRef));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRibbonStripFont()
	{
		MethodInfo method = ((object)ඳ).GetType().GetMethod("ShouldSerializeFont", BindingFlags.Instance | BindingFlags.NonPublic);
		return (bool)method.Invoke(ඳ, null);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRibbonStripFont()
	{
		((Control)ඳ).ResetFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefaultGroupFont()
	{
		TypeDescriptor.GetProperties(this)["DefaultGroupFont"].SetValue(this, null);
	}

	private SubItemsCollection ᐂ()
	{
		if (_1C2E)
		{
			if (Ᏸ == null)
			{
				Ᏸ = new Ყ(Ꮾ);
				BaseItem applicationButton = GetApplicationButton();
				if (applicationButton != null)
				{
					Ᏸ.__0AF3(applicationButton);
				}
				foreach (BaseItem item in Ꮾ.ᯥ)
				{
					Ᏸ.__0AF3(item);
				}
			}
			return Ᏸ;
		}
		return ඳ.QuickToolbarItems;
	}

	private static BaseItem ᐃ(RibbonControl ᤓ)
	{
		BaseItem result = null;
		for (int i = 0; i < ᤓ.QuickToolbarItems.Count; i++)
		{
			if (ᤓ.QuickToolbarItems[i] is ApplicationButton)
			{
				result = ᤓ.QuickToolbarItems[i];
				break;
			}
		}
		return result;
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
	}

	private void _᱐()
	{
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		eDotNetBarStyle effectiveStyle = StyleManager.GetEffectiveStyle();
		if (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(effectiveStyle))
		{
			BaseItem baseItem = ᐃ(this);
			if (baseItem != null)
			{
				if (baseItem is ButtonItem && ((ButtonItem)baseItem).ImageFixedSize.IsEmpty)
				{
					((ButtonItem)baseItem).ImageFixedSize = new Size(16, 16);
				}
				if (baseItem is ButtonItem)
				{
					ButtonItem buttonItem = (ButtonItem)baseItem;
					buttonItem.ImagePaddingHorizontal = 0;
					buttonItem.ImagePaddingVertical = 0;
				}
				if (baseItem.Parent != null)
				{
					baseItem.Parent.SubItems.Remove(baseItem);
				}
				Items.Insert(0, baseItem);
			}
		}
		else if (effectiveStyle == eDotNetBarStyle.Office2007 && Items.Count > 0 && Items[0] is ApplicationButton)
		{
			ApplicationButton applicationButton = (ApplicationButton)Items[0];
			if (!applicationButton.ImageFixedSize.IsEmpty)
			{
				applicationButton.ImageFixedSize = Size.Empty;
			}
			applicationButton.ImagePaddingHorizontal = 2;
			applicationButton.ImagePaddingVertical = 2;
			Items.Remove(applicationButton);
			if (_1C2E)
			{
				ඳ.CaptionContainerItem.SubItems.Insert(0, applicationButton);
			}
			else
			{
				QuickToolbarItems.Insert(0, applicationButton);
			}
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is RibbonPanel))
			{
				continue;
			}
			((RibbonPanel)(object)val).ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			RibbonPanel ribbonPanel = (RibbonPanel)(object)val;
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				if (val2 is RibbonBar)
				{
					RibbonBar ribbonBar = val2 as RibbonBar;
					if (ribbonBar.ჷ == null && !ribbonPanel.DefaultLayout && !((Component)this).DesignMode)
					{
						((Control)ribbonBar).Width = ((Control)ribbonBar).Width + 4;
					}
					TypeDescriptor.GetProperties(ribbonBar)["Style"].SetValue(ribbonBar, eDotNetBarStyle.StyleManagerControlled);
				}
			}
		}
		ඳ.Style = eDotNetBarStyle.StyleManagerControlled;
		_᱑();
		RibbonStrip.ඕ();
		((Control)this).Invalidate(true);
		if (((Control)this).IsHandleCreated && SelectedRibbonTabItem != null && SelectedRibbonTabItem.Panel != null)
		{
			((Control)SelectedRibbonTabItem.Panel).PerformLayout();
		}
		if (Ꮾ != null)
		{
			Ꮾ.ߕ = eDotNetBarStyle.StyleManagerControlled;
		}
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate(true);
			RecalcLayout();
		}
	}

	private void _᱑()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		if (!__1C37)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = StyleManager.GetEffectiveStyle();
		if (StyleManager.IsMetro(effectiveStyle))
		{
			Padding padding = ((Control)this).Padding;
			int left = ((Padding)(ref padding)).Left;
			Padding padding2 = ((Control)this).Padding;
			int top = ((Padding)(ref padding2)).Top;
			Padding padding3 = ((Control)this).Padding;
			((Control)this).Padding = new Padding(left, top, ((Padding)(ref padding3)).Right, 0);
		}
		else
		{
			Padding padding4 = ((Control)this).Padding;
			int left2 = ((Padding)(ref padding4)).Left;
			Padding padding5 = ((Control)this).Padding;
			int top2 = ((Padding)(ref padding5)).Top;
			Padding padding6 = ((Control)this).Padding;
			((Control)this).Padding = new Padding(left2, top2, ((Padding)(ref padding6)).Right, Ꮿ);
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is RibbonPanel ribbonPanel)
			{
				if (StyleManager.IsMetro(effectiveStyle))
				{
					Padding padding7 = ((Control)ribbonPanel).Padding;
					int left3 = ((Padding)(ref padding7)).Left;
					Padding padding8 = ((Control)ribbonPanel).Padding;
					int top3 = ((Padding)(ref padding8)).Top;
					Padding padding9 = ((Control)ribbonPanel).Padding;
					((Control)ribbonPanel).Padding = new Padding(left3, top3, ((Padding)(ref padding9)).Right, ((Component)this).DesignMode ? 2 : 0);
				}
				else
				{
					Padding padding10 = ((Control)ribbonPanel).Padding;
					int left4 = ((Padding)(ref padding10)).Left;
					Padding padding11 = ((Control)ribbonPanel).Padding;
					int top4 = ((Padding)(ref padding11)).Top;
					Padding padding12 = ((Control)ribbonPanel).Padding;
					((Control)ribbonPanel).Padding = new Padding(left4, top4, ((Padding)(ref padding12)).Right, Ꮿ);
				}
			}
		}
	}

	private void _᱒(eDotNetBarStyle ᥙ)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		switch (ᥙ)
		{
		case eDotNetBarStyle.Office2010:
			if (GlobalManager.Renderer is Office2007Renderer && !(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable();
			}
			break;
		case eDotNetBarStyle.Windows7:
			if (GlobalManager.Renderer is Office2007Renderer && !(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable();
			}
			break;
		case eDotNetBarStyle.Office2007:
			if (GlobalManager.Renderer is Office2007Renderer && (((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable || ((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(Office2007ColorTable);
			}
			break;
		}
		ඳ.Style = ᥙ;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is RibbonPanel))
			{
				continue;
			}
			((RibbonPanel)(object)val).ColorSchemeStyle = ᥙ;
			foreach (Control item2 in (ArrangedElementCollection)val.Controls)
			{
				Control val2 = item2;
				if (val2 is RibbonBar)
				{
					RibbonBar component = val2 as RibbonBar;
					TypeDescriptor.GetProperties(component)["Style"].SetValue(component, ᥙ);
				}
			}
		}
		if (Ꮾ != null)
		{
			Ꮾ.ߕ = ᥙ;
		}
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate(true);
			RecalcLayout();
		}
		if (!((Component)this).DesignMode && ((Control)this).FindForm() is RibbonForm ribbonForm)
		{
			ribbonForm.ἃ();
		}
	}

	protected virtual void OnBeforeRibbonPanelPopupClose(RibbonPopupCloseEventArgs e)
	{
		if (_1C29 != null)
		{
			_1C29(this, e);
		}
	}

	protected virtual void OnAfterRibbonPanelPopupClose(EventArgs e)
	{
		if (_1C2A != null)
		{
			_1C2A(this, e);
		}
	}

	protected virtual void OnBeforeRibbonPanelPopup(CancelEventArgs ce)
	{
		if (_1C27 != null)
		{
			_1C27(this, ce);
		}
	}

	protected virtual void OnAfterRibbonPanelPopup(EventArgs e)
	{
		if (_1C28 != null)
		{
			_1C28(this, e);
		}
	}

	internal void ට(BaseItem _06EC)
	{
		ඳ.ට(_06EC);
		if (ᰗ && _06EC != null && !(_06EC is RibbonTabItem) && !_06EC.IsContainer && !_06EC.Name.StartsWith("sysgallery") && !(_06EC is CheckBoxItem) && _06EC.AutoCollapseOnClick && (!(_06EC is PopupItem) || !_06EC.Expanded))
		{
			CloseRibbonMenu(_06EC, eEventSource.Mouse);
		}
	}

	private void _᱓()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_0A04 == null)
		{
			_0A04 = new Timer();
			_0A04.Interval = 100;
			_0A04.Tick += ણ;
			ਅ = _089F.GetForegroundWindow();
			ਆ = _089F.GetActiveWindow();
			_0A04.Start();
		}
	}

	private void ણ(object _٠, EventArgs ؿ)
	{
		if (_0A04 == null)
		{
			return;
		}
		IntPtr foregroundWindow = _089F.GetForegroundWindow();
		IntPtr activeWindow = _089F.GetActiveWindow();
		if (!(foregroundWindow != ਅ) && !(activeWindow != ਆ))
		{
			return;
		}
		if (activeWindow != IntPtr.Zero)
		{
			Control val = Control.FromHandle(activeWindow);
			if (val is PopupContainer || val is PopupContainerControl || val is Balloon)
			{
				return;
			}
		}
		_0A04.Stop();
		_᱔();
	}

	private void _᱔()
	{
		if (ᰗ)
		{
			CloseRibbonMenu(this, eEventSource.Code);
		}
		_᱕();
	}

	private void _᱕()
	{
		if (_0A04 != null)
		{
			Timer val = _0A04;
			_0A04 = null;
			val.Stop();
			val.Tick -= ણ;
			((Component)(object)val).Dispose();
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible)
		{
			_᱔();
		}
		else if (!((Component)this).DesignMode && !__13F6)
		{
			Ӕ ӕ2 = new Ӕ();
			ӕ2.Ḕ();
			((Component)(object)ӕ2).Dispose();
			__13F6 = true;
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	internal void _᱖()
	{
		_᱔();
	}

	internal void _᱗(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (!ᰗ)
		{
			return;
		}
		Control val = Control.FromChildHandle(_0A65);
		if (val == null)
		{
			string text = _089F.स(_0A65);
			text = text.ToLower();
			if (text.IndexOf("combolbox") < 0)
			{
				_᱔();
			}
			return;
		}
		do
		{
			if (val is MenuPanel || val is PopupContainer || val is PopupContainerControl || val is RibbonBar || val is RibbonStrip || val is RibbonPanel || val is Balloon)
			{
				return;
			}
			val = val.Parent;
		}
		while (val != null);
		_᱔();
	}

	public void PopupRibbon(object source, eEventSource eventSource)
	{
		if (Expanded || SelectedRibbonTabItem == null || SelectedRibbonTabItem.Panel == null || _1C34 == SelectedRibbonTabItem.Panel)
		{
			return;
		}
		Control val = (Control)(object)((Control)this).FindForm();
		if (val == null)
		{
			val = ((Control)this).Parent;
		}
		if (val == null)
		{
			return;
		}
		RibbonPanel ribbonPanel = _1C34;
		if (ribbonPanel != null)
		{
			RibbonPopupCloseEventArgs ribbonPopupCloseEventArgs = new RibbonPopupCloseEventArgs(source, eventSource);
			OnBeforeRibbonPanelPopupClose(ribbonPopupCloseEventArgs);
			if (ribbonPopupCloseEventArgs.Cancel)
			{
				return;
			}
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeRibbonPanelPopup(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			ᰗ = true;
			RibbonPanel panel = SelectedRibbonTabItem.Panel;
			((Control)panel).SuspendLayout();
			((Control)this).Controls.Remove((Control)(object)panel);
			panel.Visible = false;
			((Control)panel).Dock = (DockStyle)0;
			val.Controls.Add((Control)(object)panel);
			Point point = ((Control)ඳ).PointToScreen(new Point(0, ((Control)ඳ).Height));
			point = val.PointToClient(point);
			((Control)panel).Font = ((Control)this).Font;
			((Control)panel).Bounds = new Rectangle(point.X, point.Y - ((!StyleManager.IsMetro(EffectiveStyle)) ? Ꮿ : 0) - 1, ((Control)this).Width, _᱙(panel));
			panel.ᑞ(ᑟ: true, this);
			((Control)panel).ResumeLayout();
			panel.Visible = true;
			((Control)panel).BringToFront();
			_᱘(ribbonPanel);
			_᱓();
			_1C34 = panel;
			OnAfterRibbonPanelPopup(new EventArgs());
		}
	}

	private void _᱘(RibbonPanel Ꮔ)
	{
		if (Ꮔ == null || ((Control)Ꮔ).IsDisposed || ((Control)Ꮔ).Parent == null)
		{
			return;
		}
		SuspendLayout();
		Ꮔ.Visible = false;
		((Control)Ꮔ).Parent.Controls.Remove((Control)(object)Ꮔ);
		Ꮔ.ᑞ(ᑟ: false, this);
		((Control)Ꮔ).Dock = (DockStyle)5;
		((Control)this).Controls.Add((Control)(object)Ꮔ);
		if (SelectedRibbonTabItem != null)
		{
			SelectedRibbonTabItem.Refresh();
			if (SelectedRibbonTabItem.Panel == Ꮔ)
			{
				Ꮔ.Visible = true;
			}
		}
		((Control)Ꮔ).BringToFront();
		ResumeLayout(performLayout: true);
		OnAfterRibbonPanelPopupClose(new EventArgs());
	}

	public void CloseRibbonMenu()
	{
		CloseRibbonMenu(null, eEventSource.Code);
	}

	public void CloseRibbonMenu(object source, eEventSource eventSource)
	{
		if (ᰗ && _1C34 != null)
		{
			RibbonPopupCloseEventArgs ribbonPopupCloseEventArgs = new RibbonPopupCloseEventArgs(source, eventSource);
			OnBeforeRibbonPanelPopupClose(ribbonPopupCloseEventArgs);
			if (!ribbonPopupCloseEventArgs.Cancel)
			{
				_᱘(_1C34);
				_1C34 = null;
				ᰗ = false;
			}
		}
	}

	private int _᱙(RibbonPanel Ꮔ)
	{
		if (((Control)this).AutoSize)
		{
			return _᱈(Ꮔ);
		}
		int num = _1C3C - ((Control)ඳ).Height;
		if (_1C32)
		{
			num -= _1C33 + 1;
		}
		return num;
	}

	internal void ᱚ(RibbonTabItem ᱛ)
	{
		if (Expanded)
		{
			return;
		}
		if (Ᏼ)
		{
			if (ᱛ.Panel == _1C34)
			{
				CloseRibbonMenu(ᱛ, eEventSource.Mouse);
			}
			else
			{
				PopupRibbon(ᱛ, eEventSource.Mouse);
			}
		}
		else
		{
			Expanded = true;
		}
	}

	internal void ᱜ(RibbonTabItem ᱝ)
	{
		if (Ᏼ && !Expanded)
		{
			CloseRibbonMenu(ᱝ, eEventSource.Code);
			Expanded = true;
		}
		else
		{
			Expanded = false;
		}
	}

	private void ᱞ()
	{
		if (Ӫ && Ꮾ != null)
		{
			((Control)Ꮾ).SendToBack();
		}
		else if (!Ӫ && Ꮾ != null)
		{
			((Control)Ꮾ).BringToFront();
		}
		((Control)ඳ).Invalidate();
		if (((Control)this).AutoSize)
		{
			_᱇();
		}
		else
		{
			int num = 0;
			if (!Expanded)
			{
				num = ᱟ();
				_1C32 = _1C2E;
				if (_1C2E)
				{
					_1C33 = ((Control)Ꮾ).Height;
				}
			}
			if (Expanded)
			{
				num = _1C3C;
				if (!_1C2E && _1C32)
				{
					num -= _1C33;
				}
				else if (_1C2E && !_1C32)
				{
					num += ((Control)Ꮾ).Height;
				}
			}
			else
			{
				_1C3C = ((Control)this).Height;
			}
			if (((Control)this).Height != num && num > 0)
			{
				((Control)this).Height = num;
			}
		}
		if (StyleManager.IsMetro(EffectiveStyle) && ((Control)this).FindForm() is RibbonForm ribbonForm)
		{
			BaseItem applicationButton = GetApplicationButton();
			if (applicationButton != null)
			{
				((Control)ribbonForm).Invalidate(new Rectangle(0, applicationButton.TopInternal, ((Control)ribbonForm).Width, applicationButton.HeightInternal + 16));
			}
		}
		if (_1C2B != null)
		{
			_1C2B(this, new EventArgs());
		}
	}

	private int ᱟ()
	{
		int num = ((Control)ඳ).Height;
		if (_1C2E)
		{
			num += ((Control)Ꮾ).Height;
		}
		return num;
	}

	protected override void OnTabStopChanged(EventArgs e)
	{
		((Control)this).OnTabStopChanged(e);
		((Control)ඳ).TabStop = TabStop;
	}

	public void SuspendLayout()
	{
		((Control)this).SuspendLayout();
		_1C3E = true;
	}

	public void ResumeLayout()
	{
		_1C3E = false;
		if (_1C39)
		{
			_1C39 = false;
			Office2007ColorTable = _1C38;
		}
		((Control)this).ResumeLayout();
	}

	public void ResumeLayout(bool performLayout)
	{
		_1C3E = false;
		if (_1C39)
		{
			_1C39 = false;
			Office2007ColorTable = _1C38;
		}
		((Control)this).ResumeLayout(performLayout);
	}

	private void ᱠ()
	{
		Form val = __1C3F;
		__1C3F = null;
		if (val != null)
		{
			val.MdiChildActivate -= ᱡ;
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		ᱠ();
		((ContainerControl)this).OnParentChanged(e);
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val.MdiChildActivate += ᱡ;
			__1C3F = val;
		}
		if (_1C39 && !_1C3E)
		{
			_1C39 = false;
			Office2007ColorTable = _1C38;
		}
	}

	private void ᱡ(object _٠, EventArgs ؿ)
	{
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		Form activeMdiChild = val.ActiveMdiChild;
		if (ฬ)
		{
			if (activeMdiChild != null)
			{
				((Control)activeMdiChild).Resize += ᱥ;
				((Control)activeMdiChild).VisibleChanged += ᱤ;
			}
			if (ท != null)
			{
				((Control)ท).Resize -= ᱥ;
				((Control)ท).VisibleChanged -= ᱤ;
			}
			ธ = false;
			ท = activeMdiChild;
			ᱦ();
		}
		ᱢ();
	}

	private void ᱢ()
	{
		if (!_1C30)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		Form activeMdiChild = val.ActiveMdiChild;
		if (activeMdiChild == _1C2F)
		{
			return;
		}
		bool flag = false;
		SuspendLayout();
		ඳ.BeginUpdate();
		if (_1C2F != null)
		{
			ArrayList arrayList = ᱣ(_1C2F);
			foreach (RibbonBarMergeContainer item in arrayList)
			{
				if (item.RibbonTabItem == SelectedRibbonTabItem)
				{
					flag = true;
				}
				item.RemoveMergedRibbonBars(this);
			}
			_1C2F = null;
		}
		if (activeMdiChild != null)
		{
			ArrayList arrayList2 = ᱣ(activeMdiChild);
			if (arrayList2.Count > 0)
			{
				bool flag2 = true;
				foreach (RibbonBarMergeContainer item2 in arrayList2)
				{
					item2.MergeRibbonBars(this);
					if (flag2 && item2.RibbonTabItem != null && item2.AutoActivateTab)
					{
						item2.RibbonTabItem.Checked = true;
						flag = false;
					}
				}
				_1C2F = activeMdiChild;
			}
		}
		if (flag)
		{
			SelectFirstVisibleRibbonTab();
		}
		ඳ.EndUpdate(callRecalcLayout: false);
		ResumeLayout(performLayout: true);
		RecalcLayout();
	}

	public bool SelectFirstVisibleRibbonTab()
	{
		foreach (BaseItem item in Items)
		{
			if (item is RibbonTabItem && item.Visible)
			{
				((RibbonTabItem)item).Checked = true;
				return true;
			}
		}
		return false;
	}

	private ArrayList ᱣ(Form ᕧ)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		foreach (Control item in (ArrangedElementCollection)((Control)ᕧ).Controls)
		{
			Control val = item;
			if (val is RibbonBarMergeContainer && ((RibbonBarMergeContainer)(object)val).AllowMerge)
			{
				arrayList.Add(val);
			}
		}
		return arrayList;
	}

	private void ᱤ(object _٠, EventArgs ؿ)
	{
		ᱦ();
	}

	private void ᱥ(object _٠, EventArgs ؿ)
	{
		ᱦ();
	}

	private void ᱦ()
	{
		if (ฬ && ท != null && _൩(ท) && ((Control)ท).Visible)
		{
			if (!ธ)
			{
				ඳ.ల(ท, ఱ: true);
				ธ = true;
			}
		}
		else
		{
			ඳ.ర(ఱ: true);
			ธ = false;
		}
	}

	private bool _൩(Form ᕧ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)ᕧ.WindowState == 2)
		{
			if (ᕧ.IsMdiChild && !_0EFE.IsZoomed(((Control)ᕧ).Handle))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	internal void ᱧ(RibbonBar ዧ, int و, int ى)
	{
		if (!Ꮭ)
		{
			return;
		}
		if (ዧ.CanCustomize && (ዧ.ჾ.Contains(و, ى) || ዧ.OverflowState))
		{
			ᐆ(ዧ, ᱩ: false);
			return;
		}
		BaseItem baseItem = ዧ.HitTest(و, ى);
		if (baseItem != null && baseItem.CanCustomize && !baseItem.SystemItem)
		{
			ᐆ(baseItem, ᱩ: false);
		}
	}

	internal void ᱨ(RibbonStrip ᱩ, int و, int ى)
	{
		if (Ꮭ)
		{
			BaseItem baseItem = ᱩ.HitTest(و, ى);
			if (baseItem != null && baseItem.CanCustomize && !baseItem.SystemItem)
			{
				ᐆ(baseItem, ᱩ: true);
			}
		}
	}

	internal virtual void ᐆ(object ໄ, bool ᱩ)
	{
		if (ໄ == null || !_1C2C)
		{
			return;
		}
		ඳ._0A4A("syscustomizepopupmenu");
		ButtonItem buttonItem = new ButtonItem("syscustomizepopupmenu");
		buttonItem.Style = eDotNetBarStyle.Office2007;
		buttonItem.SetOwner(ඳ);
		if ((ᐌ(ໄ as BaseItem) || ໄ is RibbonBar) && !_1C31)
		{
			if (ໄ is BaseItem && QuickToolbarItems.Contains((BaseItem)ໄ))
			{
				ButtonItem buttonItem2 = new ButtonItem(SysQatRemoveFromItemName);
				buttonItem2.Text = SystemText.QatRemoveItemText;
				buttonItem2.Click += ᐎ;
				buttonItem2.Tag = ໄ;
				buttonItem.SubItems.Add(buttonItem2);
			}
			else
			{
				BaseItem baseItem = ໄ as BaseItem;
				ButtonItem buttonItem3 = new ButtonItem(SysQatAddToItemName);
				buttonItem3.Text = SystemText.QatAddItemText;
				buttonItem3.Click += ᐏ;
				buttonItem3.Tag = ໄ;
				buttonItem.SubItems.Add(buttonItem3);
				if ((baseItem != null && QuickToolbarItems.Contains(baseItem.Name)) || (ໄ is RibbonBar && QuickToolbarItems.Contains(ᐍ(ໄ as RibbonBar))))
				{
					buttonItem3.Enabled = false;
				}
				if (baseItem != null && BaseItem.IsOnPopup(baseItem) && baseItem.Parent != null)
				{
					object containerControl = baseItem.ContainerControl;
					Control val = (Control)((containerControl is Control) ? containerControl : null);
					if (val != null)
					{
						val.VisibleChanged += ᐈ;
					}
				}
			}
		}
		if (_1C2C)
		{
			ButtonItem buttonItem4 = new ButtonItem(SysQatCustomizeItemName);
			buttonItem4.Text = SystemText.QatCustomizeText;
			buttonItem4.BeginGroup = true;
			buttonItem4.Click += ᐋ;
			buttonItem.SubItems.Add(buttonItem4);
		}
		if (_1C2D && !_1C31)
		{
			ButtonItem buttonItem5 = new ButtonItem(SysQatPlaceItemName);
			if (_1C2E)
			{
				buttonItem5.Text = SystemText.QatPlaceAboveRibbonText;
			}
			else
			{
				buttonItem5.Text = SystemText.QatPlaceBelowRibbonText;
			}
			buttonItem5.Click += ᯃ;
			buttonItem.SubItems.Add(buttonItem5);
		}
		if (AutoExpand)
		{
			ButtonItem buttonItem6 = new ButtonItem(Expanded ? SysMinimizeRibbon : SysMaximizeRibbon, Expanded ? SystemText.MinimizeRibbonText : SystemText.MaximizeRibbonText);
			buttonItem6.Click += ᱪ;
			buttonItem6.BeginGroup = true;
			buttonItem.SubItems.Add(buttonItem6);
		}
		RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(ໄ, buttonItem);
		OnBeforeCustomizeMenuPopup(ribbonCustomizeEventArgs);
		if (ribbonCustomizeEventArgs.Cancel)
		{
			buttonItem.Dispose();
			return;
		}
		((IOwnerMenuSupport)ඳ).RegisterPopup((PopupItem)buttonItem);
		buttonItem.Popup(Control.MousePosition);
	}

	private void ᱪ(object _٠, EventArgs ؿ)
	{
		Expanded = !Expanded;
	}

	private void ᐈ(object _٠, EventArgs ؿ)
	{
		Control val = (Control)((_٠ is Control) ? _٠ : null);
		if (val != null)
		{
			val.VisibleChanged -= ᐈ;
			ඳ._0A4A("syscustomizepopupmenu");
		}
	}

	private void ᯃ(object _٠, EventArgs ؿ)
	{
		ᯃ();
	}

	internal void ᯃ()
	{
		ඳ.ClosePopups();
		if (_1C2E)
		{
			Ꮾ.ClosePopups();
			((Control)Ꮾ).Visible = false;
			ArrayList arrayList = new ArrayList(Ꮾ.ᯥ.Count);
			Ꮾ.ᯥ.CopyTo(arrayList);
			Ꮾ.ᯥ.Clear();
			GenericItemContainer captionContainerItem = ඳ.CaptionContainerItem;
			int num = captionContainerItem.SubItems.Count - 1;
			for (int i = 0; i < captionContainerItem.SubItems.Count; i++)
			{
				if (captionContainerItem.SubItems[i].ItemAlignment != 0)
				{
					num = Math.Max(0, i);
					break;
				}
			}
			foreach (BaseItem item3 in arrayList)
			{
				captionContainerItem.SubItems.Insert(num, item3);
				num++;
			}
			if (Ᏸ != null)
			{
				Ᏸ.__0AF5();
				Ᏸ = null;
			}
			((ScrollableControl)this).DockPadding.Bottom = Ꮿ;
			_1C2E = false;
			ඳ.RecalcLayout();
			if (_09FB)
			{
				AutoSyncSize();
			}
			else
			{
				((Control)this).Height = ((Control)this).Height - (((Control)Ꮾ).Height + ((ScrollableControl)Ꮾ).DockPadding.Top);
			}
			((Component)(object)Ꮾ).Dispose();
			Ꮾ = null;
		}
		else
		{
			if (Ꮾ == null)
			{
				ᱫ();
			}
			((ScrollableControl)this).DockPadding.Bottom = 1;
			Ꮾ.BeginUpdate();
			ArrayList arrayList2 = ᐉ(ᐊ: true);
			foreach (BaseItem item4 in arrayList2)
			{
				QuickToolbarItems.Remove(item4);
				Ꮾ.ᯥ.Add(item4);
			}
			Ꮾ.EndUpdate();
			((Control)Ꮾ).Height = ((_᱀ > 0) ? _᱀ : Ꮾ.GetAutoSizeHeight());
			_1C2E = true;
			if (_09FB)
			{
				AutoSyncSize();
			}
			else
			{
				((Control)this).Height = ((Control)this).Height + (((Control)Ꮾ).Height + ((ScrollableControl)Ꮾ).DockPadding.Top);
			}
		}
		RecalcLayout();
		Ᏹ = true;
		OnQatPlacementChanged(EventArgs.Empty);
	}

	protected virtual void OnQatPlacementChanged(EventArgs e)
	{
		_1C25?.Invoke(this, e);
	}

	private ArrayList ᐉ(bool ᐊ)
	{
		ArrayList arrayList = new ArrayList();
		foreach (BaseItem quickToolbarItem in QuickToolbarItems)
		{
			if (quickToolbarItem.ItemAlignment != 0)
			{
				break;
			}
			if (ᐒ(quickToolbarItem) || (quickToolbarItem is ButtonItem && ((ButtonItem)quickToolbarItem).HotTrackingStyle == eHotTrackingStyle.Image && QuickToolbarItems.IndexOf(quickToolbarItem) == 0))
			{
				if (ᐊ && quickToolbarItem is CustomizeItem)
				{
					arrayList.Add(quickToolbarItem);
				}
			}
			else
			{
				arrayList.Add(quickToolbarItem);
			}
		}
		return arrayList;
	}

	private void ᱫ()
	{
		Ꮾ = new Ꮮ();
		Ꮾ.FadeEffect = ඳ.FadeEffect;
		Ꮾ.ߕ = Style;
		((Control)Ꮾ).Dock = (DockStyle)2;
		((Control)this).Controls.Add((Control)(object)Ꮾ);
		if (!Expanded)
		{
			((Control)Ꮾ).BringToFront();
		}
	}

	private void ᐋ(object _٠, EventArgs ؿ)
	{
		ShowQatCustomizeDialog();
	}

	private bool ᐌ(BaseItem _06EC)
	{
		if (_06EC == null)
		{
			return false;
		}
		if (!_06EC.CanCustomize || _06EC.SystemItem)
		{
			return false;
		}
		if (_06EC is ItemContainer)
		{
			return false;
		}
		if (_06EC is ButtonItem && QuickToolbarItems.IndexOf(_06EC) == 0)
		{
			return false;
		}
		return true;
	}

	private string ᐍ(RibbonBar ච)
	{
		return "qt_" + ((Control)ච).Name;
	}

	private void ᐎ(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = _٠ as ButtonItem;
		if (buttonItem.Tag is BaseItem)
		{
			BaseItem item = buttonItem.Tag as BaseItem;
			RemoveItemFromQuickAccessToolbar(item);
		}
		buttonItem.Tag = null;
	}

	public void RemoveItemFromQuickAccessToolbar(BaseItem item)
	{
		RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(item, null);
		OnBeforeRemoveItemFromQuickAccessToolbar(ribbonCustomizeEventArgs);
		if (!ribbonCustomizeEventArgs.Cancel)
		{
			QuickToolbarItems.Remove(item);
			RecalcLayout();
			Ᏹ = true;
		}
	}

	private void ᐏ(object _٠, EventArgs ؿ)
	{
		ButtonItem buttonItem = _٠ as ButtonItem;
		AddItemToQuickAccessToolbar(buttonItem.Tag);
		buttonItem.Tag = null;
	}

	public void AddItemToQuickAccessToolbar(object originalItem)
	{
		BaseItem baseItem = ᐐ(originalItem);
		if (baseItem != null)
		{
			RibbonCustomizeEventArgs ribbonCustomizeEventArgs = new RibbonCustomizeEventArgs(baseItem, null);
			OnBeforeAddItemToQuickAccessToolbar(ribbonCustomizeEventArgs);
			if (!ribbonCustomizeEventArgs.Cancel)
			{
				QuickToolbarItems.Add(baseItem);
				RecalcLayout();
				Ᏹ = true;
			}
		}
	}

	private BaseItem ᐐ(object ໄ)
	{
		BaseItem baseItem = null;
		if (ໄ is ButtonItem)
		{
			ButtonItem buttonItem = ((ButtonItem)ໄ).Copy() as ButtonItem;
			buttonItem.KeyTips = "";
			buttonItem.ImageFixedSize = Size.Empty;
			buttonItem.FixedSize = Size.Empty;
			buttonItem.ButtonStyle = eButtonStyle.Default;
			buttonItem.ImagePosition = eImagePosition.Left;
			buttonItem.BeginGroup = false;
			buttonItem.UseSmallImage = true;
			if (buttonItem.TextMarkupBody != null && buttonItem.TextMarkupBody._0CBA)
			{
				buttonItem.Text = ሒ.ሙ(buttonItem.Text);
			}
			if (buttonItem.Image == null && buttonItem.ImageIndex == -1 && buttonItem.Icon == null)
			{
				buttonItem.Image = (Image)(object)BarFunctions.ই("SystemImages.GreenLight.png");
			}
			else
			{
				buttonItem.ImageFixedSize = new Size(16, 16);
			}
			baseItem = buttonItem;
		}
		else if (ໄ is BaseItem)
		{
			baseItem = ((BaseItem)ໄ).Copy();
			baseItem.KeyTips = "";
		}
		else if (ໄ is RibbonBar)
		{
			RibbonBar ribbonBar = ໄ as RibbonBar;
			ButtonItem buttonItem2 = new ButtonItem(ᐍ(ribbonBar));
			buttonItem2.Image = ribbonBar.ᅏ();
			buttonItem2.ImageFixedSize = new Size(16, 16);
			buttonItem2.AutoExpandOnClick = true;
			buttonItem2.Tooltip = ((Control)ribbonBar).Text;
			buttonItem2.Text = ((Control)ribbonBar).Text;
			RibbonBar ribbonBar2 = null;
			bool flag = false;
			if (ribbonBar.ჺ)
			{
				ribbonBar.ᅉ();
				ribbonBar2 = ribbonBar.ᅊ(ᅋ: true);
				flag = true;
			}
			else
			{
				ribbonBar2 = ribbonBar.ᅊ(ᅋ: true);
			}
			ribbonBar2.ჵ = true;
			ribbonBar2.ჴ = buttonItem2;
			foreach (BaseItem item2 in ribbonBar.Items)
			{
				BaseItem item = item2.Copy();
				ribbonBar2.Items.Add(item);
			}
			if (flag)
			{
				ribbonBar.RecalcLayout();
			}
			ItemContainer itemContainer = new ItemContainer();
			itemContainer.Stretch = true;
			ControlContainerItem controlContainerItem = new ControlContainerItem();
			controlContainerItem.AllowItemResize = false;
			itemContainer.SubItems.Add(controlContainerItem);
			controlContainerItem.Control = (Control)(object)ribbonBar2;
			buttonItem2.SubItems.Add(itemContainer);
			baseItem = buttonItem2;
		}
		return baseItem;
	}

	protected virtual void OnBeforeCustomizeMenuPopup(RibbonCustomizeEventArgs e)
	{
		if (Ꮱ != null)
		{
			Ꮱ(this, e);
		}
	}

	protected virtual void OnBeforeAddItemToQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (Ꮲ != null)
		{
			Ꮲ(this, e);
		}
	}

	protected virtual void OnBeforeRemoveItemFromQuickAccessToolbar(RibbonCustomizeEventArgs e)
	{
		if (Ꮳ != null)
		{
			Ꮳ(this, e);
		}
	}

	public virtual void ShowQatCustomizeDialog()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		Form val = (Form)(object)ᐑ();
		if (Ꮴ != null)
		{
			QatCustomizeDialogEventArgs qatCustomizeDialogEventArgs = new QatCustomizeDialogEventArgs(val);
			Ꮴ(this, qatCustomizeDialogEventArgs);
			if (qatCustomizeDialogEventArgs.Cancel || qatCustomizeDialogEventArgs.Dialog == null)
			{
				((Component)(object)val).Dispose();
				return;
			}
			val = qatCustomizeDialogEventArgs.Dialog;
		}
		if (val is QatCustomizeDialog)
		{
			((QatCustomizeDialog)(object)val).LoadItems(this);
		}
		Form val2 = ((Control)this).FindForm();
		val.StartPosition = (FormStartPosition)4;
		DialogResult val3 = val.ShowDialog((IWin32Window)(object)val2);
		if (Ꮵ != null)
		{
			QatCustomizeDialogEventArgs qatCustomizeDialogEventArgs2 = new QatCustomizeDialogEventArgs(val);
			Ꮵ(this, qatCustomizeDialogEventArgs2);
			if (qatCustomizeDialogEventArgs2.Cancel)
			{
				((Component)(object)val).Dispose();
				return;
			}
		}
		if ((int)val3 == 2)
		{
			((Component)(object)val).Dispose();
		}
		else if (val is QatCustomizeDialog qatCustomizeDialog && qatCustomizeDialog.QatCustomizePanel.DataChanged)
		{
			ApplyQatCustomizePanelChanges(qatCustomizeDialog.QatCustomizePanel);
			((Component)(object)val).Dispose();
		}
	}

	public void ApplyQatCustomizePanelChanges(QatCustomizePanel customizePanel)
	{
		if (customizePanel == null || !customizePanel.DataChanged)
		{
			return;
		}
		ඳ.BeginUpdate();
		try
		{
			ItemPanel itemPanelQat = customizePanel.itemPanelQat;
			int num = 0;
			BaseItem applicationButton = GetApplicationButton();
			if (applicationButton != null)
			{
				num = QuickToolbarItems.IndexOf(applicationButton) + 1;
			}
			ArrayList arrayList = new ArrayList();
			SubItemsCollection subItemsCollection = new SubItemsCollection(null);
			for (int i = num; i < QuickToolbarItems.Count; i++)
			{
				BaseItem baseItem = QuickToolbarItems[i];
				if (!ᐒ(baseItem))
				{
					if (!itemPanelQat.Items.Contains(baseItem.Name))
					{
						arrayList.Add(baseItem);
					}
					else
					{
						subItemsCollection.__0AF3(baseItem);
					}
				}
			}
			foreach (BaseItem item4 in arrayList)
			{
				QuickToolbarItems.Remove(item4);
			}
			foreach (BaseItem item5 in subItemsCollection)
			{
				QuickToolbarItems.Remove(item5);
			}
			foreach (BaseItem item6 in itemPanelQat.Items)
			{
				if (item6.Tag != null)
				{
					BaseItem item3 = ᐐ(item6.Tag as BaseItem);
					QuickToolbarItems.Add(item3);
					continue;
				}
				BaseItem baseItem3 = subItemsCollection[item6.Name];
				if (baseItem3 != null)
				{
					QuickToolbarItems.Add(baseItem3);
				}
			}
			Ᏹ = true;
		}
		finally
		{
			ඳ.EndUpdate();
		}
		if (customizePanel.checkQatBelowRibbon.Checked != _1C2E)
		{
			ᯃ();
		}
		OnAfterQatDialogChangesApplied();
	}

	protected virtual void OnAfterQatDialogChangesApplied()
	{
		if (Ꮶ != null)
		{
			Ꮶ(this, new EventArgs());
		}
	}

	private QatCustomizeDialog ᐑ()
	{
		QatCustomizeDialog qatCustomizeDialog = new QatCustomizeDialog();
		((Control)qatCustomizeDialog).Text = SystemText.QatDialogCaption;
		((Control)qatCustomizeDialog.ẘ).Text = SystemText.QatDialogCancelButton;
		((Control)qatCustomizeDialog.ẗ).Text = SystemText.QatDialogOkButton;
		((Control)qatCustomizeDialog.QatCustomizePanel.buttonAddToQat).Text = SystemText.QatDialogAddButton;
		((Control)qatCustomizeDialog.QatCustomizePanel.buttonRemoveFromQat).Text = SystemText.QatDialogRemoveButton;
		((Control)qatCustomizeDialog.QatCustomizePanel.labelCategories).Text = SystemText.QatDialogCategoriesLabel;
		((Control)qatCustomizeDialog.QatCustomizePanel.checkQatBelowRibbon).Text = SystemText.QatDialogPlacementCheckbox;
		return qatCustomizeDialog;
	}

	private bool ᐒ(BaseItem _06EC)
	{
		if (_06EC.SystemItem || _06EC is ItemContainer || _06EC is CustomizeItem || _06EC is SystemCaptionItem)
		{
			return true;
		}
		return false;
	}

	public BaseItem GetApplicationButton()
	{
		return ඳ.ඩ();
	}

	private string ᐓ()
	{
		ArrayList arrayList = ᐉ(ᐊ: false);
		StringBuilder stringBuilder = new StringBuilder();
		if (_1C2E)
		{
			stringBuilder.Append("1");
		}
		else
		{
			stringBuilder.Append("0");
		}
		foreach (BaseItem item in arrayList)
		{
			if (item.Name != "")
			{
				stringBuilder.Append("," + item.Name);
			}
		}
		return stringBuilder.ToString();
	}

	private void ᐔ(string ᐕ)
	{
		if (ᐕ == "" || ᐕ == null)
		{
			return;
		}
		string[] array = ᐕ.Split(new char[1] { ',' });
		if (array.Length == 0)
		{
			return;
		}
		bool flag = array[0] == "1";
		ArrayList arrayList = new ArrayList();
		for (int i = 1; i < array.Length; i++)
		{
			if (QuickToolbarItems.Contains(array[i]))
			{
				arrayList.Add(QuickToolbarItems[array[i]]);
			}
			else if (array[i].StartsWith("qt_"))
			{
				RibbonBar ribbonBar = ᱬ(array[i].Substring(3));
				if (ribbonBar != null)
				{
					arrayList.Add(ᐐ(ribbonBar));
				}
			}
			else
			{
				BaseItem item = ඳ.GetItem(array[i]);
				if (item != null && ᐌ(item))
				{
					arrayList.Add(ᐐ(item));
				}
			}
		}
		ArrayList arrayList2 = ᐉ(ᐊ: false);
		foreach (BaseItem item4 in arrayList2)
		{
			QuickToolbarItems.Remove(item4);
		}
		foreach (BaseItem item5 in arrayList)
		{
			QuickToolbarItems.Add(item5);
		}
		if (_1C2E != flag)
		{
			ᯃ();
		}
		Ᏹ = false;
	}

	private RibbonBar ᱬ(string _0956)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control ԟ = item;
			RibbonBar ribbonBar = ᱬ(ԟ, _0956);
			if (ribbonBar != null)
			{
				return ribbonBar;
			}
		}
		return null;
	}

	private RibbonBar ᱬ(Control ԟ, string _0956)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (ԟ is RibbonBar && ԟ.Name == _0956)
		{
			return ԟ as RibbonBar;
		}
		foreach (Control item in (ArrangedElementCollection)ԟ.Controls)
		{
			Control ԟ2 = item;
			RibbonBar ribbonBar = ᱬ(ԟ2, _0956);
			if (ribbonBar != null)
			{
				return ribbonBar;
			}
		}
		return null;
	}
}
