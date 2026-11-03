using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(DotNetBarManager), "DotNetBarManager.ico")]
[DefaultEvent("ItemClick")]
[Designer("DevComponents.DotNetBar.Design.DotNetBarManagerDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ComVisible(false)]
[ToolboxItem(true)]
public class DotNetBarManager : Component, IOwner, IOwnerMenuSupport, IOwnerLocalize, IOwnerItemEvents, IOwnerBarSupport, ሎ, ICustomSerialization, IOwnerAutoHideSupport, IExtenderProvider
{
	public delegate void LocalizeStringEventHandler(object sender, LocalizeEventArgs e);

	public delegate void BarClosingEventHandler(object sender, BarClosingEventArgs e);

	public delegate void PopupOpenEventHandler(object sender, PopupOpenEventArgs e);

	public delegate void DockTabChangeEventHandler(object sender, DockTabChangeEventArgs e);

	public delegate void AutoHideDisplayEventHandler(object sender, AutoHideDisplayEventArgs e);

	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

	public delegate void CustomizeContextMenuEventHandler(object sender, CustomizeContextMenuEventArgs e);

	private BarStateChangedEventHandler _0B46;

	private ActiveDockContainerChangedEventHandler _0DFA;

	private CustomizeContextMenuEventHandler _0DFB;

	private EventHandler _09BC;

	private MouseEventHandler ঽ;

	private EventHandler _09BE;

	private EventHandler _09BF;

	private PopupOpenEventHandler _09C0;

	private EventHandler _09C1;

	private EventHandler _09C2;

	private EventHandler _09C3;

	private EventHandler _0B47;

	private EventHandler _0B48;

	private EventHandler _0DFC;

	private EventHandler _0B4B;

	private MouseEventHandler ग़;

	private MouseEventHandler ફ;

	private EventHandler બ;

	private EventHandler ભ;

	private MouseEventHandler મ;

	private EventHandler ય;

	private EventHandler _0AA9;

	private EventHandler પ;

	private EventHandler _09CC;

	private EventHandler _09D3;

	private EventHandler _0DFD;

	private ItemRemovedEventHandler _09CD;

	private EventHandler ৎ;

	private EventHandler _09CF;

	private EventHandler _09D0;

	private EventHandler _0DFE;

	private EventHandler _0DFF;

	private ControlContainerItem.ControlContainerSerializationEventHandler _09D1;

	private ControlContainerItem.ControlContainerSerializationEventHandler _09D2;

	private DockTabChangeEventHandler _0B49;

	private BarClosingEventHandler _0E00;

	private AutoHideDisplayEventHandler _0B4D;

	private EventHandler ก;

	private EventHandler ข;

	private LocalizeStringEventHandler _09D4;

	private OptionGroupChangingEventHandler _09D5;

	private EventHandler _09D6;

	private EndUserCustomizeEventHandler ฃ;

	private DockTabClosingEventHandler _0B50;

	private DockTabClosingEventHandler _0B51;

	private SerializeItemEventHandler _0B52;

	private SerializeItemEventHandler _0B53;

	private EventHandler ค;

	private EventHandler ฅ;

	private EventHandler _09BB;

	private static Hashtable ฆ = new Hashtable(10);

	private License ง;

	private DockSite จ;

	private DockSite ฉ;

	private DockSite ช;

	private DockSite ซ;

	private DockSite ฌ;

	private DockSite ญ;

	private DockSite ฎ;

	private DockSite ฏ;

	private DockSite ฐ;

	private Bars ฑ;

	private ArrayList ฒ;

	private Items ณ;

	private Hashtable _09DA;

	private bool _২;

	private frmCustomize ด;

	private ArrayList ต;

	private BaseItem _09D9;

	private BaseItem _09D8;

	private ArrayList ਏ;

	private _20EF ถ;

	private Form ท;

	private bool ธ;

	private bool _0BC7;

	private bool _0BC8;

	private ImageList գ;

	private ImageList _09DB;

	private ImageList ড়;

	private DotNetBarStreamer น;

	private Form บ;

	private bool ป;

	private bool ผ;

	private bool ฝ;

	private Timer _৮;

	private BaseItem _৯;

	private bool พ = true;

	private PopupItem ԃ;

	private ContextMenusCollection ฟ;

	private eMenuDropShadow ภ;

	private bool ม = true;

	private bool _0A0B;

	private ᒺ _0A0E;

	private bool ย;

	private bool ร;

	private bool ฤ = true;

	private bool _০ = true;

	private bool _১;

	private ePopupAnimation ஃ = ePopupAnimation.SystemDefault;

	private Hashtable ล = new Hashtable();

	private Hashtable ฦ = new Hashtable();

	private bool ว = true;

	private bool ศ;

	private bool ษ;

	private bool ส;

	private bool ห;

	private bool ฬ = true;

	private bool _৩;

	private ShortcutsCollection อ;

	private AutoHidePanel ฮ;

	private AutoHidePanel ฯ;

	private AutoHidePanel ะ;

	private AutoHidePanel _0E31;

	private bool _0AC7;

	private string า = "";

	private bool ำ;

	private bool _0BC9;

	private bool _09F8;

	private eDotNetBarStyle Ӷ = eDotNetBarStyle.Office2003;

	private bool _0E34;

	private Bar _0E35;

	private Control _0E36;

	private bool _0E37 = true;

	internal bool _0E38;

	private bool _0A0C;

	private bool _0E39 = true;

	private bool _0E3A;

	private bool _0E3B;

	private ColorScheme լ;

	private bool _0E3C;

	private bool _0E3D = true;

	private bool _0E3E;

	private bool _0E3F;

	private Size เ = new Size(48, 48);

	private ContextMenuBar แ;

	private int โ;

	private int ใ = 96;

	private int _ไ = 3;

	private static bool _ๅ = true;

	private bool _ๆ = true;

	private DockContainerItem __0E47;

	private bool _0E48 = true;

	private _2924 _0E49;

	private _2924 _0E4A;

	private _2924 _0E4B;

	private _2924 _0E4C;

	private _2924 _0E4D;

	private bool _0E4E;

	private bool _0E4F;

	private bool ਖ = true;

	private bool _๐;

	private bool _๑;

	private Timer _๒;

	private bool _๓;

	private bool _๔ = true;

	[Category("Appearance")]
	[DefaultValue(3)]
	[Description("Indicates docking splitter size")]
	public int DockSplitterSize
	{
		get
		{
			return _ไ;
		}
		set
		{
			_ไ = value;
		}
	}

	public static bool MouseDockingEnabled
	{
		get
		{
			return _ๅ;
		}
		set
		{
			_ๅ = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether Ctrl+Tab keys cycles the currently selected dock tab")]
	[Category("Behavior")]
	public bool EnableDockCtrlTabKey
	{
		get
		{
			return _ๆ;
		}
		set
		{
			_ๆ = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates Context menu bar associated with the DotNetBarManager which is used as part of Global Items feature.")]
	[Category("Data")]
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
				แ.GlobalParentComponent = this;
			}
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[Description("Gets or sets the form DotNetBarManager is attached to.")]
	public Form ParentForm
	{
		get
		{
			return บ;
		}
		set
		{
			if (บ != null)
			{
				ReleaseParentFormHooks();
			}
			บ = value;
			OnParentChanged();
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public Control ParentUserControl
	{
		get
		{
			return _0E36;
		}
		set
		{
			_0E36 = value;
			SetupParentUserControl();
		}
	}

	[Browsable(false)]
	public DockContainerItem ActiveDockContainerItem => __0E47;

	private bool IsPopupProviderOnly
	{
		get
		{
			if (จ == null && ฉ == null && ช == null)
			{
				return ซ == null;
			}
			return false;
		}
	}

	ArrayList IOwnerBarSupport.WereVisible => ฒ;

	[Browsable(false)]
	public bool IsDisposed => ป;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool DisposeGCCollect
	{
		get
		{
			return _0E48;
		}
		set
		{
			_0E48 = value;
		}
	}

	[Category("Docking")]
	[Description("Indicates minimum client size that docking windows will try to maintain for the client area (not occupied by dock windows).")]
	[Browsable(true)]
	public Size MinimumClientSize
	{
		get
		{
			return เ;
		}
		set
		{
			เ = value;
		}
	}

	[Description("Indicates whether DotNetBar provides docking hints for easy docking of bars.")]
	[Browsable(false)]
	[Category("Docking")]
	[Obsolete]
	[DefaultValue(true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool DockingHintsEnabled
	{
		get
		{
			return _0E37;
		}
		set
		{
			_0E37 = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether user can control how first bar is docked when using docking hints.")]
	[Browsable(true)]
	[Category("Docking")]
	public bool EnableFullSizeDock
	{
		get
		{
			return _0E39;
		}
		set
		{
			_0E39 = value;
		}
	}

	[Category("Docking")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("")]
	public bool ApplyDocumentBarStyle
	{
		get
		{
			return _0E3D;
		}
		set
		{
			_0E3D = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Bars Bars => ฑ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string DefinitionName
	{
		get
		{
			return า;
		}
		set
		{
			า = value;
			ำ = false;
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	public bool IsDefinitionLoaded => ำ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public DotNetBarStreamer BarStream
	{
		get
		{
			if (DefinitionName != "")
			{
				return null;
			}
			if (น != null)
			{
				return น;
			}
			return new DotNetBarStreamer(this);
		}
		set
		{
			น = null;
			if (value != null && value.Data != null)
			{
				น = value;
				ำ = false;
				BarStreamLoad();
			}
		}
	}

	[Browsable(false)]
	public Items Items => ณ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Obsolete("ContextMenus on DotNetBarManager is removed. Use ContextMenuBar control instead.")]
	public ContextMenusCollection ContextMenus => ฟ;

	private bool IsDocumentDockingEnabled => ฌ != null;

	[Category("Behavior")]
	[Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool DispatchShortcuts
	{
		get
		{
			return _৩;
		}
		set
		{
			_৩ = value;
		}
	}

	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Indicates list of shortcut keys that are automatically dispatched to the control that has focus even if they are handled and used by one of the items.")]
	[Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[Category("Behavior")]
	[TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public ShortcutsCollection AutoDispatchShortcuts
	{
		get
		{
			return อ;
		}
		set
		{
			อ = value;
			อ.Parent = null;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether Reset buttons is shown that allows end-user to reset the toolbar state.")]
	[Browsable(true)]
	[Category("Behavior")]
	public bool ShowResetButton
	{
		get
		{
			return ย;
		}
		set
		{
			ย = value;
		}
	}

	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	[Category("Data")]
	[DefaultValue(null)]
	[Browsable(true)]
	public ImageList Images
	{
		get
		{
			return գ;
		}
		set
		{
			if (գ != null)
			{
				((Component)(object)գ).Disposed -= ImageListDisposed;
			}
			գ = value;
			if (գ != null)
			{
				((Component)(object)գ).Disposed += ImageListDisposed;
			}
		}
	}

	[Category("Data")]
	[Browsable(true)]
	[Description("ImageList for medium-sized images used on Items.")]
	[DefaultValue(null)]
	public ImageList ImagesMedium
	{
		get
		{
			return _09DB;
		}
		set
		{
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed -= ImageListDisposed;
			}
			_09DB = value;
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed += ImageListDisposed;
			}
		}
	}

	[Category("Data")]
	[Browsable(true)]
	[Description("ImageList for large-sized images used on Items.")]
	[DefaultValue(null)]
	public ImageList ImagesLarge
	{
		get
		{
			return ড়;
		}
		set
		{
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed -= ImageListDisposed;
			}
			ড় = value;
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed += ImageListDisposed;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool SuspendLayout
	{
		get
		{
			return _0AC7;
		}
		set
		{
			if (_0AC7 == value)
			{
				return;
			}
			_0AC7 = value;
			if (!_0AC7)
			{
				if (จ != null && จ.NeedsLayout)
				{
					จ.RecalcLayout();
				}
				if (ฉ != null && ฉ.NeedsLayout)
				{
					ฉ.RecalcLayout();
				}
				if (ช != null && ช.NeedsLayout)
				{
					ช.RecalcLayout();
				}
				if (ซ != null && ซ.NeedsLayout)
				{
					ซ.RecalcLayout();
				}
				if (ฌ != null && ฌ.NeedsLayout)
				{
					ฌ.RecalcLayout();
				}
				if (ญ != null && ญ.NeedsLayout)
				{
					ญ.RecalcLayout();
				}
				if (ฎ != null && ฎ.NeedsLayout)
				{
					ฎ.RecalcLayout();
				}
				if (ฏ != null && ฏ.NeedsLayout)
				{
					ฏ.RecalcLayout();
				}
				if (ฐ != null && ฐ.NeedsLayout)
				{
					ฐ.RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Specifes whether drop shadow is displayed for Menus and pop-up Bars. OfficeXP Style only.")]
	[DefaultValue(eMenuDropShadow.SystemDefault)]
	[Category("Behavior")]
	public eMenuDropShadow MenuDropShadow
	{
		get
		{
			return ภ;
		}
		set
		{
			ภ = value;
		}
	}

	[Description("Specifes whether to use Alpha-Blending shadows for pop-up items if supported by target OS. Disabling Alpha-Blended shadows can improve performance.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool AlphaBlendShadow
	{
		get
		{
			return ม;
		}
		set
		{
			ม = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
	public bool DisabledImagesGrayScale
	{
		get
		{
			return ਖ;
		}
		set
		{
			ਖ = value;
		}
	}

	[Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool UseHook
	{
		get
		{
			return _0A0B;
		}
		set
		{
			if (_0A0B == value)
			{
				return;
			}
			_0A0B = value;
			if (บ == null)
			{
				return;
			}
			if (_0A0B)
			{
				if (_২)
				{
					_1BE6._1BEB(this);
				}
				_২ = false;
				if (_0A0E == null)
				{
					_0A0E = new ᒺ(this);
				}
				return;
			}
			if (_0A0E != null)
			{
				_0A0E.Dispose();
				_0A0E = null;
			}
			if (!base.DesignMode)
			{
				if (!_২)
				{
					_1BE6._1BEA(this);
				}
				_২ = true;
			}
		}
	}

	bool IOwnerMenuSupport.ShowPopupShadow
	{
		get
		{
			if (ภ == eMenuDropShadow.Show)
			{
				return true;
			}
			if (ภ == eMenuDropShadow.Hide)
			{
				return false;
			}
			if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor < 1) || Environment.OSVersion.Version.Major < 5)
			{
				return true;
			}
			return _089F.ࢣ;
		}
	}

	bool ICustomSerialization.HasSerializeItemHandlers => _0B52 != null;

	bool ICustomSerialization.HasDeserializeItemHandlers => _0B53 != null;

	internal bool IsLoadingDefinition => _0E3A;

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IncludeDockDocumentsInDefinition
	{
		get
		{
			return _0E3F;
		}
		set
		{
			_0E3F = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string Definition
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			SaveDefinition(xmlDocument);
			return xmlDocument.OuterXml;
		}
		set
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			LoadDefinition(xmlDocument);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LayoutDefinition
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			SaveLayout(xmlDocument);
			return xmlDocument.OuterXml;
		}
		set
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			LoadLayout(xmlDocument);
		}
	}

	[Browsable(false)]
	public bool IsLoadingLayout => _0E3B;

	internal bool IsCustomizing => _๑;

	[Description("Specifies that custom customize dialog will be used. Use EnterCustomize event to show your custom dialog box.")]
	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	public bool UseCustomCustomizeDialog
	{
		get
		{
			return ห;
		}
		set
		{
			ห = value;
		}
	}

	BaseItem IOwner.DragItem
	{
		get
		{
			if (ด != null)
			{
				return ด.DragItem;
			}
			return null;
		}
	}

	bool IOwner.DragInProgress
	{
		get
		{
			if (ด != null)
			{
				return ด.DragInProgress;
			}
			return false;
		}
	}

	internal Form CustomizeForm => (Form)(object)ด;

	[DefaultValue(null)]
	[Browsable(false)]
	public DockSite ToolbarTopDockSite
	{
		get
		{
			return ญ;
		}
		set
		{
			ญ = value;
			if (ญ != null)
			{
				((Control)ญ).Dock = (DockStyle)1;
				ญ.ၻ(this);
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public DockSite ToolbarBottomDockSite
	{
		get
		{
			return ฎ;
		}
		set
		{
			ฎ = value;
			if (ฎ != null)
			{
				((Control)ฎ).Dock = (DockStyle)2;
				ฎ.ၻ(this);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DockSite ToolbarLeftDockSite
	{
		get
		{
			return ฏ;
		}
		set
		{
			ฏ = value;
			if (ฏ != null)
			{
				((Control)ฏ).Dock = (DockStyle)3;
				ฏ.ၻ(this);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DockSite ToolbarRightDockSite
	{
		get
		{
			return ฐ;
		}
		set
		{
			ฐ = value;
			if (ฐ != null)
			{
				((Control)ฐ).Dock = (DockStyle)4;
				ฐ.ၻ(this);
			}
		}
	}

	[Browsable(false)]
	public DockSite TopDockSite
	{
		get
		{
			return จ;
		}
		set
		{
			จ = value;
			if (จ == null)
			{
				return;
			}
			((Control)จ).Dock = (DockStyle)1;
			จ.ၻ(this);
			if (((Control)จ).Parent == null)
			{
				((Control)จ).ParentChanged += DockSiteParentChanged;
				return;
			}
			if (((Control)จ).Parent is UserControl)
			{
				SetupParentUserControl();
			}
			BarStreamLoad();
			ProcessDelayedCommands();
		}
	}

	[Browsable(false)]
	public DockSite BottomDockSite
	{
		get
		{
			return ฉ;
		}
		set
		{
			ฉ = value;
			if (ฉ != null)
			{
				((Control)ฉ).Dock = (DockStyle)2;
				ฉ.ၻ(this);
				if (((Control)ฉ).Parent == null)
				{
					((Control)ฉ).ParentChanged += DockSiteParentChanged;
					return;
				}
				BarStreamLoad();
				ProcessDelayedCommands();
			}
		}
	}

	[Browsable(false)]
	public DockSite LeftDockSite
	{
		get
		{
			return ช;
		}
		set
		{
			ช = value;
			if (ช != null)
			{
				((Control)ช).Dock = (DockStyle)3;
				ช.ၻ(this);
				if (((Control)ช).Parent == null)
				{
					((Control)ช).ParentChanged += DockSiteParentChanged;
					return;
				}
				BarStreamLoad();
				ProcessDelayedCommands();
			}
		}
	}

	[Browsable(false)]
	public DockSite RightDockSite
	{
		get
		{
			return ซ;
		}
		set
		{
			ซ = value;
			if (ซ != null)
			{
				((Control)ซ).Dock = (DockStyle)4;
				ซ.ၻ(this);
				if (((Control)ซ).Parent == null)
				{
					((Control)ซ).ParentChanged += DockSiteParentChanged;
					return;
				}
				BarStreamLoad();
				ProcessDelayedCommands();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DockSite FillDockSite
	{
		get
		{
			return ฌ;
		}
		set
		{
			ฌ = value;
			if (ฌ != null)
			{
				ฌ.ၻ(this);
			}
		}
	}

	AutoHidePanel IOwnerAutoHideSupport.LeftAutoHidePanel
	{
		get
		{
			if (ฮ == null)
			{
				if (((Control)ช).Parent == null)
				{
					return null;
				}
				ฮ = new AutoHidePanel();
				((Control)ฮ).Font = SystemFonts.DefaultFont;
				ฮ.ၻ(this);
				((Control)ฮ).Dock = (DockStyle)3;
				((Control)ฮ).Width = 0;
				((Control)ฮ).Visible = false;
				((Control)ช).Parent.Controls.Add((Control)(object)ฮ);
				if (ช.ၔ)
				{
					((Control)ช).Parent.Controls.SetChildIndex((Control)(object)ฮ, ((Control)ช).Parent.Controls.GetChildIndex((Control)(object)ช));
				}
				else
				{
					((Control)ช).Parent.Controls.SetChildIndex((Control)(object)ฮ, ((Control)ช).Parent.Controls.GetChildIndex((Control)(object)ช) + 1);
				}
			}
			return ฮ;
		}
		set
		{
			ฮ = value;
		}
	}

	bool IOwnerAutoHideSupport.HasLeftAutoHidePanel => ฮ != null;

	AutoHidePanel IOwnerAutoHideSupport.RightAutoHidePanel
	{
		get
		{
			if (ฯ == null)
			{
				if (((Control)ซ).Parent == null)
				{
					return null;
				}
				ฯ = new AutoHidePanel();
				ฯ.ၻ(this);
				((Control)ฯ).Dock = (DockStyle)4;
				((Control)ฯ).Width = 0;
				((Control)ฯ).Visible = false;
				((Control)ซ).Parent.Controls.Add((Control)(object)ฯ);
				if (ซ.ၔ)
				{
					((Control)ซ).Parent.Controls.SetChildIndex((Control)(object)ฯ, ((Control)ซ).Parent.Controls.GetChildIndex((Control)(object)ซ));
				}
				else
				{
					((Control)ซ).Parent.Controls.SetChildIndex((Control)(object)ฯ, ((Control)ซ).Parent.Controls.GetChildIndex((Control)(object)ซ) + 1);
				}
			}
			return ฯ;
		}
		set
		{
			ฯ = value;
		}
	}

	bool IOwnerAutoHideSupport.HasRightAutoHidePanel => ฯ != null;

	AutoHidePanel IOwnerAutoHideSupport.TopAutoHidePanel
	{
		get
		{
			if (ะ == null)
			{
				if (((Control)จ).Parent == null)
				{
					return null;
				}
				ะ = new AutoHidePanel();
				((Control)ะ).Font = SystemFonts.DefaultFont;
				ะ.ၻ(this);
				((Control)ะ).Dock = (DockStyle)1;
				((Control)ะ).Width = 0;
				((Control)ะ).Visible = false;
				((Control)จ).Parent.Controls.Add((Control)(object)ะ);
				if (จ.ၔ)
				{
					((Control)จ).Parent.Controls.SetChildIndex((Control)(object)ะ, ((Control)จ).Parent.Controls.GetChildIndex((Control)(object)จ));
				}
				else
				{
					((Control)จ).Parent.Controls.SetChildIndex((Control)(object)ะ, ((Control)จ).Parent.Controls.GetChildIndex((Control)(object)จ) + 1);
				}
			}
			return ะ;
		}
		set
		{
			ะ = value;
		}
	}

	bool IOwnerAutoHideSupport.HasTopAutoHidePanel => ะ != null;

	AutoHidePanel IOwnerAutoHideSupport.BottomAutoHidePanel
	{
		get
		{
			if (_0E31 == null)
			{
				if (((Control)ฉ).Parent == null)
				{
					return null;
				}
				_0E31 = new AutoHidePanel();
				((Control)_0E31).Font = SystemFonts.DefaultFont;
				_0E31.ၻ(this);
				((Control)_0E31).Dock = (DockStyle)2;
				((Control)_0E31).Width = 0;
				((Control)_0E31).Visible = false;
				((Control)ฉ).Parent.Controls.Add((Control)(object)_0E31);
				if (ฉ.ၔ)
				{
					((Control)ฉ).Parent.Controls.SetChildIndex((Control)(object)_0E31, ((Control)ฉ).Parent.Controls.GetChildIndex((Control)(object)ฉ));
				}
				else
				{
					((Control)ฉ).Parent.Controls.SetChildIndex((Control)(object)_0E31, ((Control)ฉ).Parent.Controls.GetChildIndex((Control)(object)ฉ) + 1);
				}
			}
			return _0E31;
		}
		set
		{
			_0E31 = value;
		}
	}

	bool IOwnerAutoHideSupport.HasBottomAutoHidePanel => _0E31 != null;

	[DefaultValue(false)]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[Description("Specifies whether new bars created are drawn using Themes when running on OS that supports themes like Windows XP.")]
	[Browsable(true)]
	public virtual bool ThemeAware
	{
		get
		{
			return _09F8;
		}
		set
		{
			foreach (Bar item in ฑ)
			{
				item.ThemeAware = value;
			}
			_09F8 = value;
			if (base.DesignMode)
			{
				if (จ != null)
				{
					จ.RecalcLayout();
				}
				if (ฉ != null)
				{
					ฉ.RecalcLayout();
				}
				if (ช != null)
				{
					ช.RecalcLayout();
				}
				if (ซ != null)
				{
					ซ.RecalcLayout();
				}
			}
			else
			{
				if (จ != null && ((ArrangedElementCollection)((Control)จ).Controls).Count > 0)
				{
					จ.NeedsLayout = true;
				}
				if (ฉ != null && ((ArrangedElementCollection)((Control)ฉ).Controls).Count > 0)
				{
					ฉ.NeedsLayout = true;
				}
				if (ช != null && ((ArrangedElementCollection)((Control)ช).Controls).Count > 0)
				{
					ช.NeedsLayout = true;
				}
				if (ซ != null && ((ArrangedElementCollection)((Control)ซ).Controls).Count > 0)
				{
					ซ.NeedsLayout = true;
				}
			}
		}
	}

	[Description("Specifies the default visual style of the Bars.")]
	[Category("Color Scheme")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public eDotNetBarStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			foreach (Bar item in ฑ)
			{
				TypeDescriptor.GetProperties(item)["Style"].SetValue(item, value);
			}
			foreach (BaseItem item2 in ฟ)
			{
				item2.Style = value;
			}
			foreach (DictionaryEntry item3 in ณ)
			{
				((BaseItem)item3.Value).Style = value;
			}
			if (Ӷ != value)
			{
				լ._0875(value);
			}
			Ӷ = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Color Scheme")]
	[Description("Indicates Color Scheme for all bars.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor(typeof(ColorSchemeVSEditor), typeof(UITypeEditor))]
	[Browsable(true)]
	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			լ = value;
		}
	}

	[Description("Indicates whether ColorScheme object on DotNetBarManager is used as a default ColorScheme for all bars managed by DotNetBarManager.")]
	[DevCoBrowsable(true)]
	[Category("Color Scheme")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool UseGlobalColorScheme
	{
		get
		{
			return _0E3C;
		}
		set
		{
			_0E3C = value;
		}
	}

	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Browsable(true)]
	[Description("Indicates whether the Personalized menu setting is ignored and full menus are always shown.")]
	public bool AlwaysShowFullMenus
	{
		get
		{
			return ร;
		}
		set
		{
			ร = value;
		}
	}

	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Browsable(true)]
	[Description("Indicates whether accelerator letters for menu or toolbar commands are underlined regardless of current Windows settings.")]
	public bool AlwaysDisplayKeyAccelerators
	{
		get
		{
			return _0A0C;
		}
		set
		{
			_0A0C = value;
			Bar menuBar = GetMenuBar();
			if (menuBar != null)
			{
				((Control)menuBar).Refresh();
			}
		}
	}

	[Browsable(false)]
	public bool IsThemeActive
	{
		get
		{
			if (!BarFunctions.ThemedOS)
			{
				return false;
			}
			return _0EEE._0EF0;
		}
	}

	[Category("Run-time Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether the CustomizeItem (allows toolbar customization) is added for new Bars end users are creating.")]
	public bool AllowUserBarCustomize
	{
		get
		{
			return ว;
		}
		set
		{
			ว = value;
		}
	}

	[Browsable(true)]
	[Category("Run-time Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether DotNetBar ignores the F10 key which when pressed sets the focus to menu bar.")]
	public bool IgnoreF10Key
	{
		get
		{
			return _0BC9;
		}
		set
		{
			_0BC9 = value;
		}
	}

	[Browsable(true)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether the items that are not most recenly used are shown after mouse hovers over the expand button.")]
	[DefaultValue(true)]
	public bool ShowFullMenusOnHover
	{
		get
		{
			return ฤ;
		}
		set
		{
			ฤ = value;
		}
	}

	[Category("Run-time Behavior")]
	[Description("Indicates whether Tooltips are shown on Bars and menus.")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool ShowToolTips
	{
		get
		{
			return _০;
		}
		set
		{
			_০ = value;
		}
	}

	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Browsable(true)]
	[Description("Indicates whether item shortcut is displayed in Tooltips.")]
	public bool ShowShortcutKeysInToolTips
	{
		get
		{
			return _১;
		}
		set
		{
			_১ = value;
		}
	}

	[DefaultValue(ePopupAnimation.SystemDefault)]
	[Category("Run-time Behavior")]
	[Description("Specifies the pop-up animation style.")]
	[Browsable(true)]
	public ePopupAnimation PopupAnimation
	{
		get
		{
			return ஃ;
		}
		set
		{
			ஃ = value;
		}
	}

	[Description("Specifies whether the MDI system buttons are displayed in menu bar when MDI Child window is maximized.")]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Browsable(true)]
	public bool MdiSystemItemVisible
	{
		get
		{
			return ฬ;
		}
		set
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Invalid comparison between Unknown and I4
			ฬ = value;
			if (base.DesignMode)
			{
				return;
			}
			Bar menuBar = GetMenuBar();
			if (menuBar == null)
			{
				return;
			}
			if (ฬ)
			{
				if (บ != null && บ.IsMdiContainer && บ.ActiveMdiChild != null && (int)บ.ActiveMdiChild.WindowState == 2)
				{
					menuBar.ల(บ.ActiveMdiChild, ఱ: true);
					ธ = true;
				}
			}
			else
			{
				menuBar.ర(ఱ: true);
			}
		}
	}

	[Description("Indicates whether MDI Child form System Menu is hidden. Default value is false.")]
	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	public bool HideMdiSystemMenu
	{
		get
		{
			return _0E3E;
		}
		set
		{
			_0E3E = value;
		}
	}

	bool IOwner.DesignMode => base.DesignMode;

	Form IOwner.ActiveMdiChild => ท;

	internal Bar FocusedBar
	{
		get
		{
			return _0E35;
		}
		set
		{
			_0E35 = value;
		}
	}

	bool ሎ.IsModal
	{
		get
		{
			if (บ != null && ผ)
			{
				return บ.Modal;
			}
			return false;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Run-time Behavior")]
	[Description("Gets or sets whether customize context menu is shown on all bars or dock sites.")]
	public bool ShowCustomizeContextMenu
	{
		get
		{
			return พ;
		}
		set
		{
			พ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool PersonalizedAllVisible
	{
		get
		{
			return ฝ;
		}
		set
		{
			ฝ = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool ActivateOnLayoutLoad
	{
		get
		{
			return _๔;
		}
		set
		{
			_๔ = value;
		}
	}

	[Description("Occurs after bar registered with DotNetBarManager state has changed, like selected dock tab has changed, bar has closed, bar has been docked or undocked etc.")]
	public event BarStateChangedEventHandler BarStateChanged
	{
		add
		{
			BarStateChangedEventHandler barStateChangedEventHandler = _0B46;
			BarStateChangedEventHandler barStateChangedEventHandler2;
			do
			{
				barStateChangedEventHandler2 = barStateChangedEventHandler;
				BarStateChangedEventHandler value2 = (BarStateChangedEventHandler)Delegate.Combine(barStateChangedEventHandler2, value);
				barStateChangedEventHandler = Interlocked.CompareExchange(ref _0B46, value2, barStateChangedEventHandler2);
			}
			while ((object)barStateChangedEventHandler != barStateChangedEventHandler2);
		}
		remove
		{
			BarStateChangedEventHandler barStateChangedEventHandler = _0B46;
			BarStateChangedEventHandler barStateChangedEventHandler2;
			do
			{
				barStateChangedEventHandler2 = barStateChangedEventHandler;
				BarStateChangedEventHandler value2 = (BarStateChangedEventHandler)Delegate.Remove(barStateChangedEventHandler2, value);
				barStateChangedEventHandler = Interlocked.CompareExchange(ref _0B46, value2, barStateChangedEventHandler2);
			}
			while ((object)barStateChangedEventHandler != barStateChangedEventHandler2);
		}
	}

	[Description("Occurs when focused (active) DockContainerItem has changed. You can use ActiveDockContainerItem property to get reference to currently focused DockContainerItem.")]
	public event ActiveDockContainerChangedEventHandler ActiveDockContainerChanged
	{
		add
		{
			ActiveDockContainerChangedEventHandler activeDockContainerChangedEventHandler = _0DFA;
			ActiveDockContainerChangedEventHandler activeDockContainerChangedEventHandler2;
			do
			{
				activeDockContainerChangedEventHandler2 = activeDockContainerChangedEventHandler;
				ActiveDockContainerChangedEventHandler value2 = (ActiveDockContainerChangedEventHandler)Delegate.Combine(activeDockContainerChangedEventHandler2, value);
				activeDockContainerChangedEventHandler = Interlocked.CompareExchange(ref _0DFA, value2, activeDockContainerChangedEventHandler2);
			}
			while ((object)activeDockContainerChangedEventHandler != activeDockContainerChangedEventHandler2);
		}
		remove
		{
			ActiveDockContainerChangedEventHandler activeDockContainerChangedEventHandler = _0DFA;
			ActiveDockContainerChangedEventHandler activeDockContainerChangedEventHandler2;
			do
			{
				activeDockContainerChangedEventHandler2 = activeDockContainerChangedEventHandler;
				ActiveDockContainerChangedEventHandler value2 = (ActiveDockContainerChangedEventHandler)Delegate.Remove(activeDockContainerChangedEventHandler2, value);
				activeDockContainerChangedEventHandler = Interlocked.CompareExchange(ref _0DFA, value2, activeDockContainerChangedEventHandler2);
			}
			while ((object)activeDockContainerChangedEventHandler != activeDockContainerChangedEventHandler2);
		}
	}

	[Description("Occurs just before customize popup menu is shown.")]
	[Category("Item")]
	public event CustomizeContextMenuEventHandler CustomizeContextMenu
	{
		add
		{
			CustomizeContextMenuEventHandler customizeContextMenuEventHandler = _0DFB;
			CustomizeContextMenuEventHandler customizeContextMenuEventHandler2;
			do
			{
				customizeContextMenuEventHandler2 = customizeContextMenuEventHandler;
				CustomizeContextMenuEventHandler value2 = (CustomizeContextMenuEventHandler)Delegate.Combine(customizeContextMenuEventHandler2, value);
				customizeContextMenuEventHandler = Interlocked.CompareExchange(ref _0DFB, value2, customizeContextMenuEventHandler2);
			}
			while ((object)customizeContextMenuEventHandler != customizeContextMenuEventHandler2);
		}
		remove
		{
			CustomizeContextMenuEventHandler customizeContextMenuEventHandler = _0DFB;
			CustomizeContextMenuEventHandler customizeContextMenuEventHandler2;
			do
			{
				customizeContextMenuEventHandler2 = customizeContextMenuEventHandler;
				CustomizeContextMenuEventHandler value2 = (CustomizeContextMenuEventHandler)Delegate.Remove(customizeContextMenuEventHandler2, value);
				customizeContextMenuEventHandler = Interlocked.CompareExchange(ref _0DFB, value2, customizeContextMenuEventHandler2);
			}
			while ((object)customizeContextMenuEventHandler != customizeContextMenuEventHandler2);
		}
	}

	[Description("Occurs when Item is clicked.")]
	[Category("Item")]
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

	[Description("Occurs when Item is double-clicked.")]
	public event MouseEventHandler ItemDoubleClick
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ঽ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ঽ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ঽ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ঽ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when popup of type container is loading.")]
	[Category("Item")]
	public event EventHandler PopupContainerLoad
	{
		add
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs when popup of type container is unloading.")]
	public event EventHandler PopupContainerUnload
	{
		add
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when popup item is about to open.")]
	[Category("Item")]
	public event PopupOpenEventHandler PopupOpen
	{
		add
		{
			PopupOpenEventHandler popupOpenEventHandler = _09C0;
			PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				PopupOpenEventHandler value2 = (PopupOpenEventHandler)Delegate.Combine(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _09C0, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
		remove
		{
			PopupOpenEventHandler popupOpenEventHandler = _09C0;
			PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				PopupOpenEventHandler value2 = (PopupOpenEventHandler)Delegate.Remove(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _09C0, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
	}

	[Description("Occurs when popup item is closing.")]
	[Category("Item")]
	public event EventHandler PopupClose
	{
		add
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs just before popup window is shown.")]
	public event EventHandler PopupShowing
	{
		add
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Item Expanded property has changed.")]
	[Category("Item")]
	public event EventHandler ExpandedChange
	{
		add
		{
			EventHandler eventHandler = _09C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Bar is docked.")]
	[Category("Bar")]
	public event EventHandler BarDock
	{
		add
		{
			EventHandler eventHandler = _0B47;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B47, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B47;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B47, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Bar")]
	[Description("Occurs when Bar is Undocked.")]
	public event EventHandler BarUndock
	{
		add
		{
			EventHandler eventHandler = _0B48;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B48, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B48;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B48, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Bar")]
	[Description("Occurs before dock tab is displayed.")]
	public event EventHandler BeforeDockTabDisplay
	{
		add
		{
			EventHandler eventHandler = _0DFC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0DFC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Bar auto-hide state has changed.")]
	[Category("Bar")]
	public event EventHandler AutoHideChanged
	{
		add
		{
			EventHandler eventHandler = _0B4B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0B4B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0B4B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse button is pressed.")]
	[Category("Item")]
	public event MouseEventHandler MouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
	}

	[Category("Item")]
	[Description("Occurs when mouse button is released.")]
	public event MouseEventHandler MouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse enters the item.")]
	[Category("Item")]
	public event EventHandler MouseEnter
	{
		add
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs when mouse leaves the item.")]
	public event EventHandler MouseLeave
	{
		add
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs when mouse moves over the item.")]
	public event MouseEventHandler MouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Category("Item")]
	[Description("Occurs when mouse remains still inside an item for an amount of time.")]
	public event EventHandler MouseHover
	{
		add
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when item loses input focus.")]
	[Category("Item")]
	public event EventHandler LostFocus
	{
		add
		{
			EventHandler eventHandler = _0AA9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0AA9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0AA9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0AA9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when item receives input focus.")]
	[Category("Item")]
	public event EventHandler GotFocus
	{
		add
		{
			EventHandler eventHandler = પ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref પ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = પ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref પ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs when user changes the item position, removes the item, adds new item or creates new bar.")]
	public event EventHandler UserCustomize
	{
		add
		{
			EventHandler eventHandler = _09CC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09CC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after DotNetBar definition is loaded.")]
	[Category("General")]
	public event EventHandler DefinitionLoaded
	{
		add
		{
			EventHandler eventHandler = _09D3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09D3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when users wants to reset the DotNetBar to default state.")]
	[Category("General")]
	public event EventHandler ResetDefinition
	{
		add
		{
			EventHandler eventHandler = _0DFD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0DFD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs after an Item is removed from SubItemsCollection.")]
	public event ItemRemovedEventHandler ItemRemoved
	{
		add
		{
			ItemRemovedEventHandler itemRemovedEventHandler = _09CD;
			ItemRemovedEventHandler itemRemovedEventHandler2;
			do
			{
				itemRemovedEventHandler2 = itemRemovedEventHandler;
				ItemRemovedEventHandler value2 = (ItemRemovedEventHandler)Delegate.Combine(itemRemovedEventHandler2, value);
				itemRemovedEventHandler = Interlocked.CompareExchange(ref _09CD, value2, itemRemovedEventHandler2);
			}
			while ((object)itemRemovedEventHandler != itemRemovedEventHandler2);
		}
		remove
		{
			ItemRemovedEventHandler itemRemovedEventHandler = _09CD;
			ItemRemovedEventHandler itemRemovedEventHandler2;
			do
			{
				itemRemovedEventHandler2 = itemRemovedEventHandler;
				ItemRemovedEventHandler value2 = (ItemRemovedEventHandler)Delegate.Remove(itemRemovedEventHandler2, value);
				itemRemovedEventHandler = Interlocked.CompareExchange(ref _09CD, value2, itemRemovedEventHandler2);
			}
			while ((object)itemRemovedEventHandler != itemRemovedEventHandler2);
		}
	}

	[Category("Item")]
	[Description("Occurs after an Item has been added to the SubItemsCollection.")]
	public event EventHandler ItemAdded
	{
		add
		{
			EventHandler eventHandler = ৎ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ৎ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ৎ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ৎ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ControlContainerControl is created and contained control is needed.")]
	[Category("Item")]
	public event EventHandler ContainerLoadControl
	{
		add
		{
			EventHandler eventHandler = _09CF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09CF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Text property of an Item has changed.")]
	[Category("Item")]
	public event EventHandler ItemTextChanged
	{
		add
		{
			EventHandler eventHandler = _09D0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09D0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("General")]
	[Description("Occurs when Customize Dialog is about to be shown.")]
	public event EventHandler EnterCustomize
	{
		add
		{
			EventHandler eventHandler = _0DFE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0DFE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("General")]
	[Description("Occurs when Customize Dialog is closed.")]
	public event EventHandler ExitCustomize
	{
		add
		{
			EventHandler eventHandler = _0DFF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0DFF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0DFF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize
	{
		add
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize
	{
		add
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

	[Category("Bar")]
	[Description("Occurs when current Dock tab has changed.")]
	public event DockTabChangeEventHandler DockTabChange
	{
		add
		{
			DockTabChangeEventHandler dockTabChangeEventHandler = _0B49;
			DockTabChangeEventHandler dockTabChangeEventHandler2;
			do
			{
				dockTabChangeEventHandler2 = dockTabChangeEventHandler;
				DockTabChangeEventHandler value2 = (DockTabChangeEventHandler)Delegate.Combine(dockTabChangeEventHandler2, value);
				dockTabChangeEventHandler = Interlocked.CompareExchange(ref _0B49, value2, dockTabChangeEventHandler2);
			}
			while ((object)dockTabChangeEventHandler != dockTabChangeEventHandler2);
		}
		remove
		{
			DockTabChangeEventHandler dockTabChangeEventHandler = _0B49;
			DockTabChangeEventHandler dockTabChangeEventHandler2;
			do
			{
				dockTabChangeEventHandler2 = dockTabChangeEventHandler;
				DockTabChangeEventHandler value2 = (DockTabChangeEventHandler)Delegate.Remove(dockTabChangeEventHandler2, value);
				dockTabChangeEventHandler = Interlocked.CompareExchange(ref _0B49, value2, dockTabChangeEventHandler2);
			}
			while ((object)dockTabChangeEventHandler != dockTabChangeEventHandler2);
		}
	}

	[Description("Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar.")]
	[Category("Bar")]
	public event BarClosingEventHandler BarClosing
	{
		add
		{
			BarClosingEventHandler barClosingEventHandler = _0E00;
			BarClosingEventHandler barClosingEventHandler2;
			do
			{
				barClosingEventHandler2 = barClosingEventHandler;
				BarClosingEventHandler value2 = (BarClosingEventHandler)Delegate.Combine(barClosingEventHandler2, value);
				barClosingEventHandler = Interlocked.CompareExchange(ref _0E00, value2, barClosingEventHandler2);
			}
			while ((object)barClosingEventHandler != barClosingEventHandler2);
		}
		remove
		{
			BarClosingEventHandler barClosingEventHandler = _0E00;
			BarClosingEventHandler barClosingEventHandler2;
			do
			{
				barClosingEventHandler2 = barClosingEventHandler;
				BarClosingEventHandler value2 = (BarClosingEventHandler)Delegate.Remove(barClosingEventHandler2, value);
				barClosingEventHandler = Interlocked.CompareExchange(ref _0E00, value2, barClosingEventHandler2);
			}
			while ((object)barClosingEventHandler != barClosingEventHandler2);
		}
	}

	[Description("Occurs when Bar in auto-hide state is about to be displayed.")]
	[Category("Bar")]
	public event AutoHideDisplayEventHandler AutoHideDisplay
	{
		add
		{
			AutoHideDisplayEventHandler autoHideDisplayEventHandler = _0B4D;
			AutoHideDisplayEventHandler autoHideDisplayEventHandler2;
			do
			{
				autoHideDisplayEventHandler2 = autoHideDisplayEventHandler;
				AutoHideDisplayEventHandler value2 = (AutoHideDisplayEventHandler)Delegate.Combine(autoHideDisplayEventHandler2, value);
				autoHideDisplayEventHandler = Interlocked.CompareExchange(ref _0B4D, value2, autoHideDisplayEventHandler2);
			}
			while ((object)autoHideDisplayEventHandler != autoHideDisplayEventHandler2);
		}
		remove
		{
			AutoHideDisplayEventHandler autoHideDisplayEventHandler = _0B4D;
			AutoHideDisplayEventHandler autoHideDisplayEventHandler2;
			do
			{
				autoHideDisplayEventHandler2 = autoHideDisplayEventHandler;
				AutoHideDisplayEventHandler value2 = (AutoHideDisplayEventHandler)Delegate.Remove(autoHideDisplayEventHandler2, value);
				autoHideDisplayEventHandler = Interlocked.CompareExchange(ref _0B4D, value2, autoHideDisplayEventHandler2);
			}
			while ((object)autoHideDisplayEventHandler != autoHideDisplayEventHandler2);
		}
	}

	[Description("Occurs when user start to drag item when customize dialog is open.")]
	public event EventHandler CustomizeStartItemDrag
	{
		add
		{
			EventHandler eventHandler = ก;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ก, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ก;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ก, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when users Tears-off the Tab from the Bar and new Bar is created as result of that action.")]
	public event EventHandler BarTearOff
	{
		add
		{
			EventHandler eventHandler = ข;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ข, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ข;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ข, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event LocalizeStringEventHandler LocalizeString
	{
		add
		{
			LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				LocalizeStringEventHandler value2 = (LocalizeStringEventHandler)Delegate.Combine(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
		remove
		{
			LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				LocalizeStringEventHandler value2 = (LocalizeStringEventHandler)Delegate.Remove(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
	}

	public event OptionGroupChangingEventHandler OptionGroupChanging
	{
		add
		{
			OptionGroupChangingEventHandler optionGroupChangingEventHandler = _09D5;
			OptionGroupChangingEventHandler optionGroupChangingEventHandler2;
			do
			{
				optionGroupChangingEventHandler2 = optionGroupChangingEventHandler;
				OptionGroupChangingEventHandler value2 = (OptionGroupChangingEventHandler)Delegate.Combine(optionGroupChangingEventHandler2, value);
				optionGroupChangingEventHandler = Interlocked.CompareExchange(ref _09D5, value2, optionGroupChangingEventHandler2);
			}
			while ((object)optionGroupChangingEventHandler != optionGroupChangingEventHandler2);
		}
		remove
		{
			OptionGroupChangingEventHandler optionGroupChangingEventHandler = _09D5;
			OptionGroupChangingEventHandler optionGroupChangingEventHandler2;
			do
			{
				optionGroupChangingEventHandler2 = optionGroupChangingEventHandler;
				OptionGroupChangingEventHandler value2 = (OptionGroupChangingEventHandler)Delegate.Remove(optionGroupChangingEventHandler2, value);
				optionGroupChangingEventHandler = Interlocked.CompareExchange(ref _09D5, value2, optionGroupChangingEventHandler2);
			}
			while ((object)optionGroupChangingEventHandler != optionGroupChangingEventHandler2);
		}
	}

	public event EventHandler ToolTipShowing
	{
		add
		{
			EventHandler eventHandler = _09D6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09D6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EndUserCustomizeEventHandler EndUserCustomize
	{
		add
		{
			EndUserCustomizeEventHandler endUserCustomizeEventHandler = ฃ;
			EndUserCustomizeEventHandler endUserCustomizeEventHandler2;
			do
			{
				endUserCustomizeEventHandler2 = endUserCustomizeEventHandler;
				EndUserCustomizeEventHandler value2 = (EndUserCustomizeEventHandler)Delegate.Combine(endUserCustomizeEventHandler2, value);
				endUserCustomizeEventHandler = Interlocked.CompareExchange(ref ฃ, value2, endUserCustomizeEventHandler2);
			}
			while ((object)endUserCustomizeEventHandler != endUserCustomizeEventHandler2);
		}
		remove
		{
			EndUserCustomizeEventHandler endUserCustomizeEventHandler = ฃ;
			EndUserCustomizeEventHandler endUserCustomizeEventHandler2;
			do
			{
				endUserCustomizeEventHandler2 = endUserCustomizeEventHandler;
				EndUserCustomizeEventHandler value2 = (EndUserCustomizeEventHandler)Delegate.Remove(endUserCustomizeEventHandler2, value);
				endUserCustomizeEventHandler = Interlocked.CompareExchange(ref ฃ, value2, endUserCustomizeEventHandler2);
			}
			while ((object)endUserCustomizeEventHandler != endUserCustomizeEventHandler2);
		}
	}

	[Description("Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.")]
	public event DockTabClosingEventHandler DockTabClosing
	{
		add
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B50;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Combine(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B50, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
		remove
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B50;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Remove(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B50, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
	}

	[Description("Occurs on dockable bars after DockContainerItem is closed. This event cannot be cancelled.")]
	public event DockTabClosingEventHandler DockTabClosed
	{
		add
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B51;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Combine(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B51, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
		remove
		{
			DockTabClosingEventHandler dockTabClosingEventHandler = _0B51;
			DockTabClosingEventHandler dockTabClosingEventHandler2;
			do
			{
				dockTabClosingEventHandler2 = dockTabClosingEventHandler;
				DockTabClosingEventHandler value2 = (DockTabClosingEventHandler)Delegate.Remove(dockTabClosingEventHandler2, value);
				dockTabClosingEventHandler = Interlocked.CompareExchange(ref _0B51, value2, dockTabClosingEventHandler2);
			}
			while ((object)dockTabClosingEventHandler != dockTabClosingEventHandler2);
		}
	}

	public event SerializeItemEventHandler SerializeItem
	{
		add
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B52;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Combine(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B52, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
		remove
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B52;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Remove(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B52, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
	}

	public event SerializeItemEventHandler DeserializeItem
	{
		add
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B53;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Combine(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B53, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
		remove
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B53;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Remove(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B53, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
	}

	public event EventHandler TextBoxItemTextChanged
	{
		add
		{
			EventHandler eventHandler = ค;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ค, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ค;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ค, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when color is choosen from drop-down color picker or from Custom Colors dialog box.")]
	public event EventHandler ColorPickerSelectedColorChanged
	{
		add
		{
			EventHandler eventHandler = ฅ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ฅ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ฅ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ฅ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ButtonCheckedChanged
	{
		add
		{
			EventHandler eventHandler = _09BB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnBarStateChanged(BarStateChangedEventArgs e)
	{
		_0B46?.Invoke(this, e);
	}

	internal void InvokeOnBarStateChanged(BarStateChangedEventArgs e)
	{
		OnBarStateChanged(e);
	}

	public DotNetBarManager()
		: this(null)
	{
	}

	public DotNetBarManager(IContainer cont)
	{
		Ӕ ӕ2 = new Ӕ();
		ӕ2.Ḕ();
		((Component)(object)ӕ2).Dispose();
		InitializeComponent(cont);
	}

	private void InitializeComponent(IContainer cont)
	{
		จ = null;
		ฉ = null;
		ช = null;
		ซ = null;
		ฑ = new Bars(this);
		ฒ = new ArrayList();
		ณ = new Items(this);
		_09DA = new Hashtable();
		_২ = false;
		ด = null;
		ต = null;
		_09D9 = null;
		_09D8 = null;
		ਏ = new ArrayList();
		ถ = null;
		ท = null;
		_0BC7 = false;
		գ = null;
		บ = null;
		ฟ = new ContextMenusCollection(this);
		cont?.Add(this);
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		อ = new ShortcutsCollection(null);
		լ = new ColorScheme(Ӷ);
	}

	public void Dock(Bar barToDock, eDockSide side)
	{
		if (side == eDockSide.None)
		{
			Float(barToDock);
			return;
		}
		if (barToDock.LayoutType != eLayoutType.DockContainer)
		{
			barToDock.DockSide = side;
			return;
		}
		DocumentDockUIManager documentUIManager = GetDocumentUIManager(side);
		if (!Bars.Contains(barToDock))
		{
			Bars.Add(barToDock);
		}
		documentUIManager.Dock(barToDock);
	}

	public void Dock(Bar barToDock, Bar referenceBar, eDockSide dockToReferenceBarSide)
	{
		if (dockToReferenceBarSide == eDockSide.None)
		{
			throw new ArgumentException("eDockSide.None is not supported value for dockToReferenceBarSide parameter. Use Float method to make floating bar.");
		}
		if (referenceBar.DockSide == eDockSide.None)
		{
			throw new ArgumentException("referenceBar must be docked to be used as reference for docking.");
		}
		if (barToDock.LayoutType != eLayoutType.DockContainer)
		{
			barToDock.DockSide = dockToReferenceBarSide;
		}
		else if (dockToReferenceBarSide == eDockSide.Document)
		{
			ArrayList arrayList = new ArrayList(barToDock.Items.Count);
			barToDock.Items.CopyTo(arrayList);
			DockContainerItem dockContainerItem = null;
			foreach (BaseItem item in arrayList)
			{
				if (item is DockContainerItem dockContainerItem2)
				{
					if (dockContainerItem == null)
					{
						dockContainerItem = dockContainerItem2;
					}
					dockContainerItem2.Displayed = false;
					if (dockContainerItem2.OriginalBarName == "")
					{
						dockContainerItem2.OriginalBarName = barToDock.Name;
						dockContainerItem2.OriginalPosition = barToDock.Items.IndexOf(dockContainerItem2);
					}
					barToDock.Items.Remove(dockContainerItem2);
					referenceBar.Items.Add(dockContainerItem2);
				}
			}
			referenceBar.RecalcLayout();
			if (dockContainerItem != null)
			{
				referenceBar.SelectedDockContainerItem = dockContainerItem;
			}
			referenceBar._0BF8();
			if (barToDock.CustomBar)
			{
				Bars.Remove(barToDock);
				((Component)(object)barToDock).Dispose();
			}
			else
			{
				barToDock.Visible = false;
			}
		}
		else
		{
			DocumentDockUIManager documentUIManager = GetDocumentUIManager(referenceBar.DockSide);
			if (!Bars.Contains(barToDock))
			{
				Bars.Add(barToDock);
			}
			documentUIManager.Dock(referenceBar, barToDock, dockToReferenceBarSide);
		}
	}

	public void Dock(DockContainerItem itemToDock, eDockSide side)
	{
		Dock(itemToDock, null, side);
	}

	public void Dock(DockContainerItem itemToDock, Bar referenceBar, eDockSide dockToReferenceBarSide)
	{
		Dock(itemToDock, referenceBar, dockToReferenceBarSide, Point.Empty);
	}

	private void Dock(DockContainerItem itemToDock, Bar referenceBar, eDockSide dockToReferenceBarSide, Point initialFloatLocation)
	{
		Bar bar = itemToDock.ContainerControl as Bar;
		if (dockToReferenceBarSide == eDockSide.None && bar != null)
		{
			bar.ఎ(itemToDock, ఏ: true, initialFloatLocation);
			return;
		}
		Bar bar2 = null;
		if (bar != null)
		{
			bar2 = BarFunctions.CreateDuplicateDockBar(bar);
		}
		if (itemToDock.Parent != null)
		{
			if (bar != null)
			{
				bar.Items.Remove(itemToDock);
				if (bar.Items.Count == 0)
				{
					if (bar.CustomBar)
					{
						Bars.Remove(bar);
						((Component)(object)bar).Dispose();
					}
					else
					{
						bar.Visible = false;
					}
				}
				else if (bar.VisibleItemCount == 0)
				{
					bar.Visible = false;
				}
				else
				{
					bar.RecalcLayout();
				}
			}
			else
			{
				itemToDock.Parent.SubItems.Remove(itemToDock);
			}
		}
		if (bar2 == null)
		{
			bar2 = new Bar();
			bar2.Name = "customBar" + itemToDock.Name;
			bar2.LayoutType = eLayoutType.DockContainer;
			((Control)bar2).Text = itemToDock.Text;
			bar2.AutoSyncBarCaption = true;
			bar2.CustomBar = true;
			Bars.Add(bar2);
		}
		bar2.Items.Add(itemToDock);
		if (dockToReferenceBarSide == eDockSide.None)
		{
			bar2.DockSide = eDockSide.None;
			bar2.Location = initialFloatLocation;
		}
		else if (referenceBar == null)
		{
			Dock(bar2, dockToReferenceBarSide);
		}
		else
		{
			Dock(bar2, referenceBar, dockToReferenceBarSide);
		}
	}

	public void Float(DockContainerItem itemToFloat)
	{
		if (itemToFloat.ContainerControl is Bar bar && bar.Items.Count == 1)
		{
			Float(bar);
		}
		else
		{
			Dock(itemToFloat, eDockSide.None);
		}
	}

	public void Float(DockContainerItem itemToFloat, Point initialFloatLocation)
	{
		if (itemToFloat.ContainerControl is Bar bar && bar.Items.Count == 1)
		{
			Float(bar, initialFloatLocation);
		}
		else
		{
			Dock(itemToFloat, null, eDockSide.None, initialFloatLocation);
		}
	}

	public void Float(Bar barToFloat)
	{
		barToFloat.DockSide = eDockSide.None;
	}

	public void Float(Bar barToFloat, Point initialFloatingLocation)
	{
		barToFloat.InitalFloatLocation = initialFloatingLocation;
		barToFloat.DockSide = eDockSide.None;
	}

	private DocumentDockUIManager GetDocumentUIManager(eDockSide side)
	{
		return side switch
		{
			eDockSide.None => throw new ArgumentException("eDockSide.None is not supported parameter value"), 
			eDockSide.Bottom => BottomDockSite.GetDocumentUIManager(), 
			eDockSide.Left => LeftDockSite.GetDocumentUIManager(), 
			eDockSide.Right => RightDockSite.GetDocumentUIManager(), 
			eDockSide.Top => TopDockSite.GetDocumentUIManager(), 
			_ => FillDockSite.GetDocumentUIManager(), 
		};
	}

	internal static bool IsManagerRegistered(IOwner manager, Form parentForm)
	{
		if (ฆ.Contains(parentForm))
		{
			ᒦ ᒦ2 = ฆ[parentForm] as ᒦ;
			return ᒦ2.ᒮ(manager);
		}
		return false;
	}

	internal static void RegisterParentMsgHandler(IOwner manager, Form parentForm)
	{
		if (ฆ.Contains(parentForm))
		{
			ᒦ ᒦ2 = ฆ[parentForm] as ᒦ;
			if (!ᒦ2.ᒮ(manager))
			{
				ᒦ2.ᒫ(manager);
			}
		}
		else
		{
			ᒦ ᒦ3 = new ᒦ(manager.DesignMode);
			((NativeWindow)ᒦ3).AssignHandle(((Control)parentForm).Handle);
			ᒦ3.ᒫ(manager);
			ฆ[parentForm] = ᒦ3;
		}
	}

	internal static void UnRegisterParentMsgHandler(IOwner manager, Form parentForm)
	{
		if (parentForm == null)
		{
			return;
		}
		if (ฆ.Contains(parentForm))
		{
			ᒦ ᒦ2 = ฆ[parentForm] as ᒦ;
			if (ᒦ2.ᒮ(manager))
			{
				ᒦ2.ᒭ(manager);
				if (ᒦ2.ᒧ == 0 && ᒦ2.ᒨ == 0)
				{
					((NativeWindow)ᒦ2).ReleaseHandle();
					ฆ.Remove(parentForm);
				}
			}
			return;
		}
		foreach (DictionaryEntry item in ฆ)
		{
			ᒦ ᒦ3 = item.Value as ᒦ;
			if (ᒦ3.ᒮ(manager))
			{
				ᒦ3.ᒭ(manager);
				if (ᒦ3.ᒧ == 0 && ᒦ3.ᒨ == 0)
				{
					((NativeWindow)ᒦ3).ReleaseHandle();
					ฆ.Remove(parentForm);
				}
				break;
			}
		}
	}

	internal static void RegisterOwnerParentMsgHandler(IOwner owner, Form parentForm)
	{
		if (ฆ.Contains(parentForm))
		{
			ᒦ ᒦ2 = ฆ[parentForm] as ᒦ;
			if (!ᒦ2.ᒱ(owner))
			{
				ᒦ2.ᒯ(owner);
			}
		}
		else
		{
			ᒦ ᒦ3 = new ᒦ(designmode: true);
			((NativeWindow)ᒦ3).AssignHandle(((Control)parentForm).Handle);
			ᒦ3.ᒯ(owner);
			ฆ[parentForm] = ᒦ3;
		}
	}

	internal static void UnRegisterOwnerParentMsgHandler(IOwner owner, Form parentForm)
	{
		if (parentForm == null)
		{
			return;
		}
		if (ฆ.Contains(parentForm))
		{
			ᒦ ᒦ2 = ฆ[parentForm] as ᒦ;
			if (ᒦ2.ᒱ(owner))
			{
				ᒦ2.ᒰ(owner);
				if (ᒦ2.ᒧ == 0 && ᒦ2.ᒨ == 0)
				{
					((NativeWindow)ᒦ2).ReleaseHandle();
					ฆ.Remove(parentForm);
				}
			}
			return;
		}
		foreach (DictionaryEntry item in ฆ)
		{
			ᒦ ᒦ3 = item.Value as ᒦ;
			if (ᒦ3.ᒱ(owner))
			{
				ᒦ3.ᒰ(owner);
				if (ᒦ3.ᒧ == 0 && ᒦ3.ᒨ == 0)
				{
					((NativeWindow)ᒦ3).ReleaseHandle();
					ฆ.Remove(parentForm);
				}
				break;
			}
		}
	}

	internal static void RelayApplicationActivate(bool bActivate)
	{
		ᒦ[] array = new ᒦ[ฆ.Count];
		ฆ.Values.CopyTo(array, 0);
		ᒦ[] array2 = array;
		foreach (ᒦ ᒦ2 in array2)
		{
			if (bActivate)
			{
				ᒦ2.ᒲ();
			}
			else
			{
				ᒦ2.ᒳ();
			}
		}
	}

	internal void InternalDockContainerDeactivated(DockContainerItem item)
	{
		if (__0E47 != null)
		{
			OnActiveDockContainerChanged(new ActiveDockContainerChangedEventArgs(item, isActive: false));
			__0E47 = null;
		}
	}

	internal void InternalDockContainerActivated(DockContainerItem item)
	{
		if (__0E47 != item)
		{
			if (__0E47 != null)
			{
				InternalDockContainerDeactivated(__0E47);
			}
			__0E47 = item;
			OnActiveDockContainerChanged(new ActiveDockContainerChangedEventArgs(item, isActive: true));
		}
	}

	protected virtual void OnActiveDockContainerChanged(ActiveDockContainerChangedEventArgs e)
	{
		_0DFA?.Invoke(this, e);
	}

	bool IExtenderProvider.CanExtend(object target)
	{
		if (target is Control)
		{
			return true;
		}
		return false;
	}

	void IOwnerItemEvents.InvokeItemDoubleClick(BaseItem objItem, MouseEventArgs e)
	{
		OnItemDoubleClick(objItem, e);
	}

	protected virtual void OnItemDoubleClick(BaseItem objItem, MouseEventArgs e)
	{
		MouseEventHandler val = ঽ;
		if (val != null)
		{
			val.Invoke((object)objItem, e);
		}
	}

	void IOwnerItemEvents.InvokeItemClick(BaseItem objItem)
	{
		if (_09BC != null)
		{
			_09BC(objItem, new EventArgs());
		}
	}

	internal void InvokeDockTabClosing(Bar bar, DockTabClosingEventArgs e)
	{
		if (_0B50 != null)
		{
			_0B50(this, e);
		}
	}

	internal void InvokeDockTabClosed(Bar bar, DockTabClosingEventArgs e)
	{
		if (_0B51 != null)
		{
			_0B51(this, e);
		}
	}

	internal void InvokeTextBoxItemTextChanged(TextBoxItem ti)
	{
		if (ค != null)
		{
			ค(ti, new EventArgs());
		}
	}

	internal void InvokeColorPickerSelectedColorChanged(ColorPickerDropDown d)
	{
		if (ฅ != null)
		{
			ฅ(d, new EventArgs());
		}
	}

	[Browsable(false)]
	public Bar GetItemBar(BaseItem item)
	{
		while (item.Parent != null)
		{
			item = item.Parent;
		}
		return item.ContainerControl as Bar;
	}

	internal bool GetDesignMode()
	{
		return base.DesignMode;
	}

	private void OnParentChanged()
	{
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		if (บ == null)
		{
			return;
		}
		if (!base.DesignMode)
		{
			UnRegisterParentMsgHandler(this, บ);
		}
		if (((Control)บ).IsHandleCreated && !base.DesignMode)
		{
			if (บ.IsMdiChild && บ.MdiParent != null)
			{
				RegisterParentMsgHandler(this, บ.MdiParent);
			}
			else
			{
				RegisterParentMsgHandler(this, บ);
			}
		}
		((Control)บ).HandleDestroyed += ParentHandleDestroyed;
		((Control)บ).HandleCreated += ParentHandleCreated;
		if (!base.DesignMode)
		{
			บ.Activated += ParentActivated;
			บ.Deactivate += ParentDeactivate;
			บ.Load += ParentLoad;
			((Control)บ).VisibleChanged += ParentVisibleChanged;
			((Control)บ).Resize += ParentResize;
		}
		if (!base.DesignMode)
		{
			if (!_২ && !_0A0B)
			{
				_1BE6._1BEA(this);
				_২ = true;
			}
			else if (_0A0B && _0A0E == null)
			{
				_0A0E = new ᒺ(this);
			}
		}
		if (ถ != null)
		{
			ถ._20F0 -= OnMdiSetMenu;
			((NativeWindow)ถ).ReleaseHandle();
			ถ = null;
		}
		MdiClient mdiClient = ((IOwner)this).GetMdiClient(บ);
		if (mdiClient != null)
		{
			SetupMdiHandler(mdiClient);
		}
		else
		{
			((Control)บ).ControlAdded += new ControlEventHandler(ParentControlAdded);
			ศ = true;
		}
		BarStreamLoad();
	}

	private void ParentHandleDestroyed(object sender, EventArgs e)
	{
		if (!base.DesignMode)
		{
			UnRegisterParentMsgHandler(this, บ);
		}
	}

	private void ParentHandleCreated(object sender, EventArgs e)
	{
		if (!base.DesignMode)
		{
			if (บ.IsMdiChild && บ.MdiParent != null)
			{
				RegisterParentMsgHandler(this, บ.MdiParent);
			}
			else
			{
				RegisterParentMsgHandler(this, บ);
			}
			if (ถ != null)
			{
				ถ._20F0 -= OnMdiSetMenu;
				((NativeWindow)ถ).ReleaseHandle();
				ถ = null;
				MdiClient mdiClient = ((IOwner)this).GetMdiClient(บ);
				if (mdiClient != null)
				{
					SetupMdiHandler(mdiClient);
				}
			}
		}
		BarStreamLoad(bForceLoad: true);
	}

	private void SetupMdiHandler(MdiClient client)
	{
		if (((Control)client).IsHandleCreated)
		{
			SetupClientMsgHandler(client);
		}
		else if (!ส)
		{
			((Control)client).HandleCreated += MdiClientHandleCreates;
			ส = true;
		}
		if (!ษ)
		{
			บ.MdiChildActivate += OnMdiChildActivate;
			ษ = true;
			if (บ.ActiveMdiChild != null)
			{
				OnMdiChildActivate(บ, new EventArgs());
			}
		}
	}

	private void SetupClientMsgHandler(MdiClient client)
	{
		if (client != null)
		{
			if (ถ != null)
			{
				ถ._20F0 -= OnMdiSetMenu;
				((NativeWindow)ถ).ReleaseHandle();
				ถ = null;
			}
			ถ = new _20EF();
			ถ._20F0 += OnMdiSetMenu;
			((NativeWindow)ถ).AssignHandle(((Control)client).Handle);
			if (ส)
			{
				((Control)client).HandleCreated -= MdiClientHandleCreates;
				ส = false;
			}
		}
	}

	private void MdiClientHandleCreates(object sender, EventArgs e)
	{
		SetupClientMsgHandler((MdiClient)((sender is MdiClient) ? sender : null));
	}

	private void ParentResize(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		if (บ != null && (int)บ.WindowState != 1 && ผ && ฒ.Count > 0)
		{
			((IOwner)this).OnApplicationActivate();
		}
		else if (บ != null && (int)บ.WindowState == 1 && ฒ.Count == 0)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void ParentActivated(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		ผ = true;
		if (ฒ.Count > 0 && บ != null && (int)บ.WindowState != 1)
		{
			((IOwner)this).OnApplicationActivate();
		}
	}

	private void ParentDeactivate(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		ผ = false;
		if (บ != null && (int)บ.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void ParentVisibleChanged(object sender, EventArgs e)
	{
		if (บ == null)
		{
			return;
		}
		if (((Control)บ).Visible)
		{
			if (!_২ && !_0A0B)
			{
				_২ = true;
				_1BE6._1BEA(this);
			}
			else if (_0A0B && _0A0E == null)
			{
				_0A0E = new ᒺ(this);
			}
		}
		else if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
		else if (_0A0E != null)
		{
			_0A0E.Dispose();
			_0A0E = null;
		}
	}

	private void ParentLoad(object sender, EventArgs e)
	{
		if (ถ == null)
		{
			MdiClient mdiClient = ((IOwner)this).GetMdiClient(บ);
			if (mdiClient != null)
			{
				SetupMdiHandler(mdiClient);
			}
		}
		if (((Control)บ).IsHandleCreated && !base.DesignMode)
		{
			if (บ.IsMdiChild && บ.MdiParent != null)
			{
				if (!IsManagerRegistered(this, บ.MdiParent))
				{
					UnRegisterParentMsgHandler(this, บ);
					RegisterParentMsgHandler(this, บ.MdiParent);
				}
			}
			else
			{
				RegisterParentMsgHandler(this, บ);
			}
		}
		BarStreamLoad(bForceLoad: true);
	}

	private void ParentControlAdded(object sender, ControlEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (e.Control is MdiClient)
		{
			SetupMdiHandler((MdiClient)e.Control);
		}
	}

	private void BarStreamLoad()
	{
		BarStreamLoad(bForceLoad: false);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void BarStreamLoad(bool bForceLoad)
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		if (ำ)
		{
			return;
		}
		if (า != "" && ((จ != null && ((Control)จ).Parent != null && ฉ != null && ((Control)ฉ).Parent != null && ช != null && ((Control)ช).Parent != null && ซ != null && ((Control)ซ).Parent != null) || bForceLoad || (IsPopupProviderOnly && ParentForm != null && _0E36 != null)))
		{
			if (base.DesignMode)
			{
				string text = DesignTimeDte.GetDefinitionPath(า, Site);
				if (text != null && text != "")
				{
					if (!text.EndsWith("\\"))
					{
						text += "\\";
					}
					if (File.Exists(text + า))
					{
						LoadDefinition(text + า);
					}
					else
					{
						MessageBox.Show("DotNetBar definition file not found: " + text + า, "DotNetBar Designer", (MessageBoxButtons)0, (MessageBoxIcon)48);
						า = "";
					}
				}
			}
			else if ((ParentForm != null && (((Control)ParentForm).IsHandleCreated || bForceLoad || IsPopupProviderOnly)) || (จ != null && ((Control)จ).Parent is UserControl))
			{
				Stream stream = null;
				Hashtable hashtable = new Hashtable();
				if (จ != null && ((Control)จ).Parent is UserControl)
				{
					hashtable.Add(((object)((Control)จ).Parent).GetType().Assembly.FullName, ((object)((Control)จ).Parent).GetType().Assembly);
					stream = ((object)((Control)จ).Parent).GetType().Assembly.GetManifestResourceStream(((object)((Control)จ).Parent).GetType().Namespace + "." + า);
				}
				else
				{
					if (((object)ParentForm).GetType().BaseType.FullName != "System.Windows.Forms.Form")
					{
						Type type = ((object)ParentForm).GetType();
						Stack stack = new Stack(10);
						for (int i = 0; i < 64; i++)
						{
							stack.Push(type);
							if (!hashtable.ContainsKey(type.Assembly.FullName))
							{
								hashtable.Add(type.Assembly.FullName, type.Assembly);
							}
							if (type.BaseType.FullName == "System.Windows.Forms.Form")
							{
								break;
							}
							type = type.BaseType;
						}
						stream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + า);
						if (stream == null)
						{
							while (stack.Count > 0 && stream == null)
							{
								type = stack.Pop() as Type;
								stream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + า);
							}
						}
						stack.Clear();
					}
					else
					{
						stream = ((object)ParentForm).GetType().Assembly.GetManifestResourceStream(((object)ParentForm).GetType().Namespace + "." + า);
						hashtable.Add(((object)ParentForm).GetType().Assembly.FullName, ((object)ParentForm).GetType().Assembly);
					}
					if (_0E36 != null && !hashtable.Contains(((object)_0E36).GetType().Assembly.FullName))
					{
						hashtable.Add(((object)_0E36).GetType().Assembly.FullName, ((object)_0E36).GetType().Assembly);
					}
				}
				if (stream == null)
				{
					Assembly[] array = new Assembly[hashtable.Count];
					hashtable.Values.CopyTo(array, 0);
					for (int num = array.Length - 1; num >= 0; num--)
					{
						Assembly assembly = array[num];
						string[] manifestResourceNames = assembly.GetManifestResourceNames();
						string[] array2 = manifestResourceNames;
						foreach (string text2 in array2)
						{
							if (text2.EndsWith(า))
							{
								stream = assembly.GetManifestResourceStream(text2);
								break;
							}
						}
						if (stream != null)
						{
							break;
						}
					}
				}
				hashtable.Clear();
				if (stream != null)
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(stream);
					LoadDefinition(xmlDocument);
					ำ = true;
				}
			}
			น = null;
		}
		else if (น != null && ((จ != null && ((Control)จ).Parent != null && ฉ != null && ((Control)ฉ).Parent != null && ช != null && ((Control)ช).Parent != null && ซ != null && ((Control)ซ).Parent != null) || bForceLoad))
		{
			if (น.Data != null)
			{
				LoadDefinition(น.Data);
				ำ = true;
			}
			น = null;
		}
	}

	void IOwner.OnApplicationDeactivate()
	{
		if (บ != null && ((Control)บ).InvokeRequired)
		{
			((Control)บ).BeginInvoke((Delegate)new ᒴ(OnApplicationDeactivateInternal));
		}
		else
		{
			OnApplicationDeactivateInternal();
		}
	}

	private void OnApplicationDeactivateInternal()
	{
		CloseDockingHints();
		if (ฒ.Count == 0)
		{
			if (ฑ != null)
			{
				foreach (Bar item in ฑ)
				{
					if (item.ଓ)
					{
						return;
					}
				}
				foreach (Bar item2 in ฑ)
				{
					item2.శ();
					if (item2.BarState == eBarState.Floating && item2.Visible && item2.HideFloatingInactive)
					{
						item2.స();
						ฒ.Add(item2);
					}
				}
			}
			ArrayList arrayList = new ArrayList(ਏ);
			foreach (PopupItem item3 in arrayList)
			{
				item3.ClosePopup();
			}
		}
		Bar menuBar = GetMenuBar();
		if (menuBar != null && menuBar._09A9)
		{
			menuBar._09A9 = false;
		}
	}

	void IOwner.OnApplicationActivate()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (บ != null && (int)บ.WindowState != 1)
		{
			if (บ != null && ((Control)บ).InvokeRequired)
			{
				((Control)บ).BeginInvoke((Delegate)new ᒴ(OnApplicationActivateInternal));
			}
			else
			{
				OnApplicationActivateInternal();
			}
		}
	}

	private void OnApplicationActivateInternal()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (บ == null || (int)บ.WindowState == 1)
		{
			return;
		}
		Form activeForm = Form.ActiveForm;
		if (activeForm == null || (activeForm.Modal && activeForm != บ))
		{
			return;
		}
		try
		{
			foreach (Bar item in ฒ)
			{
				item._0C3A();
			}
		}
		finally
		{
			ฒ.Clear();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (ป)
		{
			base.Dispose(disposing);
			return;
		}
		ป = true;
		if (disposing && ง != null)
		{
			ง.Dispose();
			ง = null;
		}
		if (ท != null)
		{
			((Control)ท).Resize -= OnMdiChildResize;
			((Control)ท).VisibleChanged -= OnMdiChildVisibleChanged;
			ท = null;
		}
		Images = null;
		ImagesMedium = null;
		ImagesLarge = null;
		if (_৮ != null)
		{
			_৮.Stop();
			((Component)(object)_৮).Dispose();
			_৮ = null;
		}
		if (ਏ != null && ਏ.Count > 0)
		{
			BaseItem[] array;
			lock (this)
			{
				array = (BaseItem[])ਏ.ToArray(typeof(BaseItem));
			}
			BaseItem[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				PopupItem popupItem = (PopupItem)array2[i];
				if (popupItem.Expanded)
				{
					popupItem.ClosePopup();
				}
			}
		}
		ReleaseParentFormHooks();
		if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
		if (_0A0E != null)
		{
			_0A0E.Dispose();
			_0A0E = null;
		}
		if (ด != null)
		{
			((Form)ด).Close();
			if (ด != null)
			{
				((Component)(object)ด).Dispose();
			}
		}
		if (ฑ != null)
		{
			ฑ = null;
		}
		if (ณ != null)
		{
			ณ.Dispose();
			ณ = null;
		}
		if (ฟ != null)
		{
			ฟ.ၻ(null);
			ฟ = null;
		}
		if (_0E48)
		{
			GC.Collect();
		}
		base.Dispose(disposing);
	}

	private void ReleaseParentFormHooks()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		if (บ != null)
		{
			if (!base.DesignMode)
			{
				UnRegisterParentMsgHandler(this, บ);
			}
			((Control)บ).HandleDestroyed -= ParentHandleDestroyed;
			((Control)บ).HandleCreated -= ParentHandleCreated;
			if (ศ)
			{
				((Control)บ).ControlAdded -= new ControlEventHandler(ParentControlAdded);
				ศ = false;
			}
			if (!base.DesignMode)
			{
				บ.Activated -= ParentActivated;
				บ.Deactivate -= ParentDeactivate;
				บ.Load -= ParentLoad;
				((Control)บ).VisibleChanged -= ParentVisibleChanged;
				((Control)บ).Resize -= ParentResize;
			}
			if (ษ)
			{
				บ.MdiChildActivate -= OnMdiChildActivate;
				ษ = false;
			}
		}
	}

	private void ImageListDisposed(object sender, EventArgs e)
	{
		if (sender == գ)
		{
			Images = null;
		}
		else if (sender == ড়)
		{
			ImagesLarge = null;
		}
		else if (sender == _09DB)
		{
			ImagesMedium = null;
		}
	}

	public bool ShouldSerializeMinimumClientSize()
	{
		if (เ.Width == 48)
		{
			return เ.Height != 48;
		}
		return true;
	}

	public void RemoveBar(Bar bar)
	{
		if (!ฑ.Contains(bar))
		{
			throw new ArgumentException("Bar not part of Bars collections.");
		}
		ฑ.Remove(bar);
		if (((Control)bar).Parent != null && ((Control)bar).Parent is DockSite)
		{
			if (((DockSite)(object)((Control)bar).Parent).ၓ || ((DockSite)(object)((Control)bar).Parent).DocumentDockContainer != null)
			{
				((DockSite)(object)((Control)bar).Parent).GetDocumentUIManager().UnDock(bar);
			}
			else
			{
				((DockSite)(object)((Control)bar).Parent).ၹ((Control)(object)bar);
			}
		}
		((IOwnerBarSupport)this).RemoveShortcutsFromBar(bar);
	}

	void IOwnerBarSupport.AddShortcutsFromBar(Bar bar)
	{
		foreach (BaseItem item in bar.Items)
		{
			((IOwner)this).AddShortcutsFromItem(item);
		}
	}

	void IOwner.AddShortcutsFromItem(BaseItem objItem)
	{
		ᯣ ᯣ2 = null;
		if (objItem.ShortcutString != "")
		{
			foreach (eShortcut shortcut in objItem.Shortcuts)
			{
				if (_09DA.ContainsKey(shortcut))
				{
					ᯣ2 = (ᯣ)_09DA[objItem.Shortcuts[0]];
				}
				else
				{
					ᯣ2 = new ᯣ(shortcut);
					_09DA.Add(ᯣ2.ᯤ, ᯣ2);
				}
				try
				{
					if (!ᯣ2.ᯥ.ContainsKey(objItem.Id))
					{
						ᯣ2.ᯥ.Add(objItem.Id, objItem);
					}
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in objItem.SubItems)
		{
			((IOwner)this).AddShortcutsFromItem(subItem);
		}
	}

	void IOwnerBarSupport.RemoveShortcutsFromBar(Bar bar)
	{
		if (bar.Items == null)
		{
			return;
		}
		foreach (BaseItem item in bar.Items)
		{
			((IOwner)this).RemoveShortcutsFromItem(item);
		}
	}

	void IOwner.RemoveShortcutsFromItem(BaseItem objItem)
	{
		ᯣ ᯣ2 = null;
		if (objItem.ShortcutString != "")
		{
			foreach (eShortcut shortcut in objItem.Shortcuts)
			{
				if (!_09DA.ContainsKey(shortcut))
				{
					continue;
				}
				ᯣ2 = (ᯣ)_09DA[shortcut];
				try
				{
					ᯣ2.ᯥ.Remove(objItem.Id);
					if (ᯣ2.ᯥ.Count == 0)
					{
						_09DA.Remove(ᯣ2.ᯤ);
					}
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in objItem.SubItems)
		{
			((IOwner)this).RemoveShortcutsFromItem(subItem);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	public bool ShouldSerializeBars()
	{
		if (_0E34)
		{
			return true;
		}
		return false;
	}

	public void ForceDefinitionLoad()
	{
		if (!ำ)
		{
			BarStreamLoad(bForceLoad: true);
		}
	}

	protected bool ShouldSerializeBarStream()
	{
		return false;
	}

	internal bool IsDesignTime()
	{
		return base.DesignMode;
	}

	DockSiteInfo IOwnerBarSupport.GetDockInfo(IDockInfo pDock, int x, int y)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		DockSiteInfo dockInfo = new DockSiteInfo();
		if ((Control.ModifierKeys & 0x20000) != 0)
		{
			return dockInfo;
		}
		if (((Bar)pDock).LayoutType == eLayoutType.DockContainer)
		{
			if ((pDock.CanDockBottom || pDock.CanDockLeft || pDock.CanDockRight || pDock.CanDockTop || pDock.CanDockDocument) && DockingHintHandler(ref dockInfo, pDock, x, y))
			{
				return dockInfo;
			}
			return dockInfo;
		}
		if (pDock.CanDockTop && ญ != null)
		{
			dockInfo = ญ.GetDockSiteInfo(pDock, x, y);
			if (dockInfo.objDockSite != null)
			{
				return dockInfo;
			}
		}
		if (pDock.CanDockBottom && ฎ != null)
		{
			dockInfo = ฎ.GetDockSiteInfo(pDock, x, y);
			if (dockInfo.objDockSite != null)
			{
				return dockInfo;
			}
		}
		if (pDock.CanDockLeft && ฏ != null)
		{
			dockInfo = ฏ.GetDockSiteInfo(pDock, x, y);
			if (dockInfo.objDockSite != null)
			{
				return dockInfo;
			}
		}
		if (pDock.CanDockRight && ฐ != null)
		{
			dockInfo = ฐ.GetDockSiteInfo(pDock, x, y);
			if (dockInfo.objDockSite != null)
			{
				return dockInfo;
			}
		}
		return dockInfo;
	}

	void IOwnerBarSupport.DockComplete()
	{
		CloseDockingHints();
		_0E4E = false;
	}

	private bool CanDockToBar(IDockInfo barDockInfo, Bar referenceBar)
	{
		if (!referenceBar.AcceptDropItems)
		{
			return false;
		}
		if (referenceBar.DockSide == eDockSide.Bottom && !barDockInfo.CanDockBottom)
		{
			return false;
		}
		if (referenceBar.DockSide == eDockSide.Left && !barDockInfo.CanDockLeft)
		{
			return false;
		}
		if (referenceBar.DockSide == eDockSide.Right && !barDockInfo.CanDockRight)
		{
			return false;
		}
		if (referenceBar.DockSide == eDockSide.Document && !barDockInfo.CanDockDocument)
		{
			return false;
		}
		if (referenceBar.DockSide == eDockSide.Top && !barDockInfo.CanDockTop)
		{
			return false;
		}
		return true;
	}

	private bool DockingHintHandler(ref DockSiteInfo dockInfo, IDockInfo barDockInfo, int x, int y)
	{
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		if (!_0E4E)
		{
			_0E4E = true;
			return true;
		}
		Bar bar = null;
		foreach (Bar bar2 in Bars)
		{
			if (bar2.Visible && (bar2 != barDockInfo || base.DesignMode || bar2.DockSide == eDockSide.Document))
			{
				Point pt = ((Control)bar2).PointToClient(new Point(x, y));
				if (bar2.LayoutType == eLayoutType.DockContainer && ((Control)bar2).ClientRectangle.Contains(pt) && bar2.DockSide != 0 && CanDockToBar(barDockInfo, bar2))
				{
					bar = bar2;
					break;
				}
			}
		}
		SetupDockingHintWindows(bar, barDockInfo);
		bool flag = false;
		dockInfo.DockSiteZOrderIndex = -1;
		_295F _295F2 = _295F.ᘶ;
		if (_0E49 != null)
		{
			_295F2 = _0E49._295B(x, y);
		}
		if (_0E4A != null)
		{
			_295F _295F3 = _0E4A._295B(x, y);
			if (_295F2 == _295F.ᘶ)
			{
				_295F2 = _295F3;
			}
		}
		if (_0E4B != null)
		{
			_295F _295F4 = _0E4B._295B(x, y);
			if (_295F2 == _295F.ᘶ)
			{
				_295F2 = _295F4;
			}
		}
		if (_0E4C != null)
		{
			_295F _295F5 = _0E4C._295B(x, y);
			if (_295F2 == _295F.ᘶ)
			{
				_295F2 = _295F5;
			}
		}
		if (_0E4D != null)
		{
			_295F _295F6 = _0E4D._295B(x, y);
			if (_295F2 == _295F.ᘶ)
			{
				_295F2 = _295F6;
				if (_295F6 != 0)
				{
					flag = true;
				}
				if (flag && IsDocumentDockingEnabled && barDockInfo.CanDockDocument && bar == null && barDockInfo.CanDockDocument)
				{
					if (!barDockInfo.CanDockTop && _295F6 == _295F._074A)
					{
						_295F2 = _295F.ᵌ;
					}
					else if (!barDockInfo.CanDockBottom && _295F6 == _295F.ራ)
					{
						_295F2 = _295F.ᵌ;
					}
					else if (!barDockInfo.CanDockLeft && _295F6 == _295F._0749)
					{
						_295F2 = _295F.ᵌ;
					}
					else if (!barDockInfo.CanDockRight && _295F6 == _295F.ሪ)
					{
						_295F2 = _295F.ᵌ;
					}
				}
			}
		}
		dockInfo.MouseOverBar = bar;
		if (bar == null || !flag)
		{
			if (_0E39)
			{
				dockInfo.FullSizeDock = !flag;
				dockInfo.PartialSizeDock = flag;
			}
			switch (_295F2)
			{
			case _295F._0749:
				dockInfo.MouseOverDockSide = eDockSide.Left;
				if (barDockInfo.DockSide != eDockSide.Left)
				{
					dockInfo.DockSide = (DockStyle)3;
					if (flag)
					{
						dockInfo.DockLine = 999;
						dockInfo.InsertPosition = 999;
					}
					else
					{
						dockInfo.DockLine = -1;
						dockInfo.InsertPosition = 0;
					}
					dockInfo.NewLine = true;
				}
				else
				{
					dockInfo.DockSide = (DockStyle)3;
					dockInfo.DockLine = barDockInfo.DockLine;
					dockInfo.DockOffset = barDockInfo.DockOffset;
					dockInfo.InsertPosition = ((Control)ช).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = ช;
				break;
			case _295F.ሪ:
				dockInfo.MouseOverDockSide = eDockSide.Right;
				if (barDockInfo.DockSide != eDockSide.Right)
				{
					dockInfo.DockSide = (DockStyle)4;
					if (flag)
					{
						dockInfo.DockLine = -1;
						dockInfo.InsertPosition = 0;
					}
					else
					{
						dockInfo.DockLine = 999;
						dockInfo.InsertPosition = 999;
					}
					dockInfo.NewLine = true;
				}
				else
				{
					dockInfo.DockSide = (DockStyle)4;
					dockInfo.DockLine = barDockInfo.DockLine;
					dockInfo.DockOffset = barDockInfo.DockOffset;
					dockInfo.InsertPosition = ((Control)ซ).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = ซ;
				break;
			case _295F._074A:
				dockInfo.MouseOverDockSide = eDockSide.Top;
				if (barDockInfo.DockSide != eDockSide.Top)
				{
					dockInfo.DockSide = (DockStyle)1;
					if (flag)
					{
						dockInfo.DockLine = 999;
						dockInfo.InsertPosition = 999;
					}
					else
					{
						dockInfo.DockLine = -1;
						dockInfo.InsertPosition = 0;
					}
					dockInfo.NewLine = true;
				}
				else
				{
					dockInfo.DockSide = (DockStyle)1;
					dockInfo.DockLine = barDockInfo.DockLine;
					dockInfo.DockOffset = barDockInfo.DockOffset;
					dockInfo.InsertPosition = ((Control)จ).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = จ;
				break;
			case _295F.ራ:
				dockInfo.MouseOverDockSide = eDockSide.Bottom;
				if (barDockInfo.DockSide != eDockSide.Bottom)
				{
					dockInfo.DockSide = (DockStyle)2;
					if (flag)
					{
						dockInfo.DockLine = -1;
						dockInfo.InsertPosition = 0;
					}
					else
					{
						dockInfo.DockLine = 999;
						dockInfo.InsertPosition = 999;
					}
					dockInfo.NewLine = true;
				}
				else
				{
					dockInfo.DockSide = (DockStyle)2;
					dockInfo.DockLine = barDockInfo.DockLine;
					dockInfo.DockOffset = barDockInfo.DockOffset;
					dockInfo.InsertPosition = ((Control)ฉ).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = ฉ;
				break;
			case _295F.ᵌ:
				if (IsDocumentDockingEnabled)
				{
					dockInfo.objDockSite = ฌ;
					dockInfo.DockSide = (DockStyle)5;
					dockInfo.MouseOverDockSide = eDockSide.Document;
				}
				break;
			}
		}
		else
		{
			switch (_295F2)
			{
			case _295F.ᵌ:
				dockInfo.MouseOverDockSide = eDockSide.Document;
				if (bar != null)
				{
					dockInfo.TabDockContainer = bar;
					dockInfo.DockSide = ((Control)bar).Parent.Dock;
					dockInfo.objDockSite = ((Control)bar).Parent as DockSite;
				}
				break;
			case _295F.ሪ:
				dockInfo.MouseOverDockSide = eDockSide.Right;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				switch (bar.DockSide)
				{
				case eDockSide.Top:
				case eDockSide.Bottom:
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar) + 1;
					dockInfo.DockOffset = bar.DockOffset + 1;
					break;
				default:
					dockInfo.DockLine = bar.DockLine + 1;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.InsertPosition++;
					dockInfo.NewLine = true;
					break;
				}
				break;
			case _295F._0749:
				dockInfo.MouseOverDockSide = eDockSide.Left;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				switch (bar.DockSide)
				{
				case eDockSide.Top:
				case eDockSide.Bottom:
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.DockOffset = bar.DockOffset - 1;
					break;
				default:
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.NewLine = true;
					break;
				}
				break;
			case _295F._074A:
				dockInfo.MouseOverDockSide = eDockSide.Top;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				switch (bar.DockSide)
				{
				case eDockSide.Top:
				case eDockSide.Bottom:
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.NewLine = true;
					break;
				default:
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.DockOffset = bar.DockOffset - 1;
					break;
				}
				break;
			case _295F.ራ:
				dockInfo.MouseOverDockSide = eDockSide.Bottom;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				switch (bar.DockSide)
				{
				case eDockSide.Top:
				case eDockSide.Bottom:
					dockInfo.DockLine = bar.DockLine + 1;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					if (bar.DockSide == eDockSide.Bottom)
					{
						dockInfo.InsertPosition++;
					}
					dockInfo.NewLine = true;
					break;
				default:
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar) + 1;
					dockInfo.DockOffset = bar.DockOffset + 1;
					break;
				}
				break;
			}
		}
		dockInfo.UseOutline = true;
		return true;
	}

	private void CloseDockingHints()
	{
		if (_0E4F)
		{
			return;
		}
		try
		{
			_0E4F = true;
			if (_0E49 != null)
			{
				_089F.SetWindowPos(((Control)_0E49).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)_0E49).Close();
				((Component)(object)_0E49).Dispose();
				_0E49 = null;
			}
			if (_0E4A != null)
			{
				_089F.SetWindowPos(((Control)_0E4A).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)_0E4A).Close();
				((Component)(object)_0E4A).Dispose();
				_0E4A = null;
			}
			if (_0E4B != null)
			{
				_089F.SetWindowPos(((Control)_0E4B).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)_0E4B).Close();
				((Component)(object)_0E4B).Dispose();
				_0E4B = null;
			}
			if (_0E4C != null)
			{
				_089F.SetWindowPos(((Control)_0E4C).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)_0E4C).Close();
				((Component)(object)_0E4C).Dispose();
				_0E4C = null;
			}
			if (_0E4D != null)
			{
				_089F.SetWindowPos(((Control)_0E4D).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)_0E4D).Close();
				((Component)(object)_0E4D).Dispose();
				_0E4D = null;
			}
		}
		catch
		{
		}
		finally
		{
			_0E4F = false;
		}
	}

	private eDotNetBarStyle GetEffectiveStyle()
	{
		if (Ӷ == eDotNetBarStyle.StyleManagerControlled)
		{
			return StyleManager.GetEffectiveStyle();
		}
		return Ӷ;
	}

	private void SetupDockingHintWindows(Bar barMouseOver, IDockInfo barDockInfo)
	{
		Rectangle empty = Rectangle.Empty;
		if (((Control)จ).Parent.Parent != null)
		{
			empty = new Rectangle(((Control)จ).Parent.PointToScreen(Point.Empty), ((Control)จ).Parent.Size);
			empty.Y += ((Control)จ).Height;
			empty.Height -= ((Control)จ).Height;
		}
		else
		{
			empty = ((Control)จ).Parent.Bounds;
			empty.Y += ((Control)จ).Height;
			empty.Height -= ((Control)จ).Height;
		}
		if (!base.DesignMode)
		{
			if (barDockInfo.CanDockLeft)
			{
				if (_0E49 == null)
				{
					_0E49 = new _2924(_295D._0749, GetEffectiveStyle());
					((Form)_0E49).Location = new Point(empty.X + 32, empty.Y + (empty.Height - ((Control)_0E49).Height) / 2);
					_0E49._294D();
				}
			}
			else if (_0E49 != null)
			{
				((Form)_0E49).Close();
				((Component)(object)_0E49).Dispose();
				_0E49 = null;
			}
			if (barDockInfo.CanDockRight)
			{
				if (_0E4A == null)
				{
					_0E4A = new _2924(_295D.ሪ, GetEffectiveStyle());
					((Form)_0E4A).Location = new Point(empty.Right - 32 - ((Control)_0E4A).Width, empty.Y + (empty.Height - ((Control)_0E4A).Height) / 2);
					_0E4A._294D();
				}
			}
			else if (_0E4A != null)
			{
				((Form)_0E4A).Close();
				((Component)(object)_0E4A).Dispose();
				_0E4A = null;
			}
			if (barDockInfo.CanDockTop)
			{
				if (_0E4B == null)
				{
					_0E4B = new _2924(_295D._074A, GetEffectiveStyle());
					((Form)_0E4B).Location = new Point(empty.X + (empty.Width - ((Control)_0E4B).Width) / 2, empty.Y + 32);
					_0E4B._294D();
				}
			}
			else if (_0E4B != null)
			{
				((Form)_0E4B).Close();
				((Component)(object)_0E4B).Dispose();
				_0E4B = null;
			}
			if (barDockInfo.CanDockBottom)
			{
				if (_0E4C == null)
				{
					_0E4C = new _2924(_295D.ራ, GetEffectiveStyle());
					((Form)_0E4C).Location = new Point(empty.X + (empty.Width - ((Control)_0E4C).Width) / 2, empty.Bottom - 32 - ((Control)_0E4C).Height);
					_0E4C._294D();
				}
			}
			else if (_0E4C != null)
			{
				((Form)_0E4C).Close();
				((Component)(object)_0E4C).Dispose();
				_0E4C = null;
			}
		}
		bool flag = false;
		_295D _295D2 = _295D._26AF;
		if (barMouseOver == null)
		{
			_295D2 = _295D._074A;
			if (barDockInfo.CanDockBottom)
			{
				_295D2 = _295D.ራ;
			}
			if (barDockInfo.CanDockLeft)
			{
				_295D2 |= _295D._0749;
			}
			if (barDockInfo.CanDockRight)
			{
				_295D2 |= _295D.ሪ;
			}
			_295D2 = ((!barDockInfo.CanDockTop) ? (_295D2 & ~(_295D2 & _295D._074A)) : (_295D2 | _295D._074A));
			if (IsDocumentDockingEnabled && barDockInfo.CanDockDocument)
			{
				_295D2 = _295D._26AF;
			}
		}
		else if (!barDockInfo.CanDockTab)
		{
			_295D2 &= ~(_295D2 & _295D.ᵌ);
		}
		if (_0E4D == null)
		{
			if (barMouseOver != null)
			{
				_0E4D = new _2924(_295D2, middleDockingHint: true, GetEffectiveStyle());
			}
			else
			{
				_0E4D = new _2924(_295D2, middleDockingHint: true, GetEffectiveStyle());
			}
			flag = true;
		}
		else if (barMouseOver != null)
		{
			_0E4D._2934 = _295D2;
		}
		else
		{
			_0E4D._2934 = _295D2;
		}
		Point point = new Point(empty.X + (empty.Width - ((Control)_0E4D).Width) / 2, empty.Y + (empty.Height - ((Control)_0E4D).Height) / 2);
		if (barMouseOver != null)
		{
			Point point2 = ((Control)barMouseOver).PointToScreen(Point.Empty);
			point = new Point(point2.X + (((Control)barMouseOver).Width - ((Control)_0E4D).Width) / 2, point2.Y + (((Control)barMouseOver).Height - ((Control)_0E4D).Height) / 2);
		}
		Rectangle rectangle = new Rectangle(point, ((Form)_0E4D).Size);
		if (_0E49 != null && rectangle.IntersectsWith(((Control)_0E49).Bounds))
		{
			if (Control.MousePosition.Y < ((Control)_0E49).Top)
			{
				point.Y = ((Control)_0E49).Top - ((Control)_0E4D).Height;
			}
			else
			{
				point.Y = ((Control)_0E49).Bottom;
			}
		}
		else if (_0E4A != null && rectangle.IntersectsWith(((Control)_0E4A).Bounds))
		{
			if (Control.MousePosition.Y < ((Control)_0E4A).Top)
			{
				point.Y = ((Control)_0E4A).Top - ((Control)_0E4D).Height;
			}
			else
			{
				point.Y = ((Control)_0E4A).Bottom;
			}
		}
		else if (_0E4B != null && rectangle.IntersectsWith(((Control)_0E4B).Bounds))
		{
			if (Control.MousePosition.X > ((Control)_0E4B).Left)
			{
				point.X = ((Control)_0E4B).Right;
			}
			else
			{
				point.X = ((Control)_0E4B).Left - ((Control)_0E4D).Width;
			}
		}
		else if (_0E4C != null && rectangle.IntersectsWith(((Control)_0E4C).Bounds))
		{
			if (Control.MousePosition.X > ((Control)_0E4C).Left)
			{
				point.X = ((Control)_0E4C).Right;
			}
			else
			{
				point.X = ((Control)_0E4C).Left - ((Control)_0E4D).Width;
			}
		}
		if (((Form)_0E4D).Location != point)
		{
			((Form)_0E4D).Location = point;
		}
		if (flag)
		{
			_0E4D._294D();
		}
	}

	public void LoadDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		LoadDefinition(xmlDocument);
	}

	private Hashtable GetDockControls()
	{
		Hashtable hashtable = new Hashtable();
		foreach (Bar bar in Bars)
		{
			if (bar.LayoutType != eLayoutType.DockContainer)
			{
				continue;
			}
			foreach (BaseItem item in bar.Items)
			{
				if (!(item.Name != "") || !(item is DockContainerItem))
				{
					continue;
				}
				DockContainerItem dockContainerItem = item as DockContainerItem;
				if (dockContainerItem.Control != null)
				{
					try
					{
						hashtable.Add(dockContainerItem.Name, dockContainerItem.Control);
						dockContainerItem.Control = null;
					}
					catch
					{
					}
				}
			}
		}
		if (hashtable.Keys.Count == 0)
		{
			return null;
		}
		return hashtable;
	}

	internal void LoadDefinition(XmlDocument xmlDoc)
	{
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Invalid comparison between Unknown and I4
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Invalid comparison between Unknown and I4
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Invalid comparison between Unknown and I4
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Invalid comparison between Unknown and I4
		XmlElement xmlElement = xmlDoc.FirstChild as XmlElement;
		if (xmlElement != null && (!xmlElement.HasAttribute(_290D._2914) || XmlConvert.ToInt32(xmlElement.GetAttribute(_290D._2914)) < 6))
		{
			return;
		}
		bool flag = false;
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		itemSerializationContext.ᵠ = GetDockControls();
		try
		{
			_0E3A = true;
			if (!SuspendLayout)
			{
				flag = true;
				SuspendLayout = true;
			}
			try
			{
				_0E38 = true;
				ณ.Clear();
				if (_0E3F)
				{
					ฑ.Clear();
				}
				else
				{
					ฑ._2241();
				}
				ฟ.Clear();
				_09DA.Clear();
			}
			finally
			{
				_0E38 = false;
			}
			DestroyAutoHidePanels();
			if (xmlElement == null)
			{
				if (flag)
				{
					SuspendLayout = false;
				}
				return;
			}
			if (xmlElement.Name != "dotnetbar")
			{
				throw new InvalidOperationException("Invalid file format (dotnetbar).");
			}
			if (xmlElement.HasAttribute("fullmenus"))
			{
				ร = XmlConvert.ToBoolean(xmlElement.GetAttribute("fullmenus"));
			}
			else
			{
				ร = false;
			}
			if (xmlElement.HasAttribute("fullmenushover"))
			{
				ฤ = XmlConvert.ToBoolean(xmlElement.GetAttribute("fullmenushover"));
			}
			else
			{
				ฤ = true;
			}
			if (xmlElement.HasAttribute("tooltips"))
			{
				_০ = XmlConvert.ToBoolean(xmlElement.GetAttribute("tooltips"));
			}
			else
			{
				_০ = true;
			}
			if (xmlElement.HasAttribute("scintooltip"))
			{
				_১ = XmlConvert.ToBoolean(xmlElement.GetAttribute("scintooltip"));
			}
			else
			{
				_১ = false;
			}
			if (xmlElement.HasAttribute("animation"))
			{
				if (XmlConvert.ToInt32(xmlElement.GetAttribute("animation")) != 6)
				{
					ஃ = (ePopupAnimation)XmlConvert.ToInt32(xmlElement.GetAttribute("animation"));
				}
			}
			else
			{
				ஃ = ePopupAnimation.SystemDefault;
			}
			foreach (XmlElement childNode in xmlElement.ChildNodes)
			{
				if (childNode.Name == "items")
				{
					foreach (XmlElement childNode2 in childNode.ChildNodes)
					{
						BaseItem baseItem = BarFunctions._0953(childNode2);
						if (baseItem == null)
						{
							throw new InvalidOperationException("Invalid Item in file found (" + BarFunctions._0957(childNode2) + ").");
						}
						itemSerializationContext.ItemXmlElement = childNode2;
						baseItem.Deserialize(itemSerializationContext);
						ณ.Add(baseItem);
					}
				}
				else if (childNode.Name == "bars")
				{
					foreach (XmlElement childNode3 in childNode.ChildNodes)
					{
						Bar bar = new Bar();
						bar.Visible = false;
						bar.SetDesignMode(base.DesignMode);
						ฑ.Add(bar);
						bar.ద(childNode3);
						((IOwnerBarSupport)this).AddShortcutsFromBar(bar);
					}
				}
				else if (childNode.Name == "popups")
				{
					foreach (XmlElement childNode4 in childNode.ChildNodes)
					{
						BaseItem baseItem2 = BarFunctions._0953(childNode4);
						if (baseItem2 == null)
						{
							throw new InvalidOperationException("Invalid Item in file found (" + BarFunctions._0957(childNode4) + ").");
						}
						itemSerializationContext.ItemXmlElement = childNode4;
						baseItem2.Deserialize(itemSerializationContext);
						ฟ.Add(baseItem2);
					}
				}
				else if (childNode.Name == _290D._290E && _0E3F)
				{
					itemSerializationContext.ItemXmlElement = childNode;
					if (ฌ != null)
					{
						ฌ.GetDocumentUIManager().Ⴉ(itemSerializationContext);
					}
				}
				else
				{
					if (!(childNode.Name == _290D._2911))
					{
						continue;
					}
					itemSerializationContext.ItemXmlElement = childNode;
					DockStyle val = (DockStyle)Enum.Parse(typeof(DockStyle), childNode.GetAttribute(_290D._2912));
					if ((int)val == 3)
					{
						if (ช != null)
						{
							ช.GetDocumentUIManager().Ⴉ(itemSerializationContext);
						}
					}
					else if ((int)val == 4)
					{
						if (ซ != null)
						{
							ซ.GetDocumentUIManager().Ⴉ(itemSerializationContext);
						}
					}
					else if ((int)val == 1)
					{
						if (จ != null)
						{
							จ.GetDocumentUIManager().Ⴉ(itemSerializationContext);
						}
					}
					else if ((int)val == 2 && ฉ != null)
					{
						ฉ.GetDocumentUIManager().Ⴉ(itemSerializationContext);
					}
				}
			}
		}
		finally
		{
			_0E3A = false;
			if (flag)
			{
				SuspendLayout = false;
			}
		}
		if (itemSerializationContext.ᵠ != null && itemSerializationContext.ᵠ.Count > 0)
		{
			foreach (Control value in itemSerializationContext.ᵠ.Values)
			{
				Control val2 = value;
				((Component)(object)val2).Dispose();
			}
		}
		((IOwner)this).InvokeDefinitionLoaded((object)this, new EventArgs());
		if (ท != null && ธ)
		{
			ธ = false;
			OnMdiChildResize(ท, new EventArgs());
		}
	}

	void ICustomSerialization.InvokeSerializeItem(SerializeItemEventArgs e)
	{
		if (_0B52 != null)
		{
			_0B52(this, e);
		}
	}

	void ICustomSerialization.InvokeDeserializeItem(SerializeItemEventArgs e)
	{
		if (_0B53 != null)
		{
			_0B53(this, e);
		}
	}

	public void SaveDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		SaveDefinition(xmlDocument);
		xmlDocument.Save(FileName);
	}

	internal void SaveDefinition(XmlDocument xmlDoc)
	{
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		XmlElement xmlElement = xmlDoc.CreateElement("dotnetbar");
		xmlElement.SetAttribute(_290D._2914, "6");
		xmlDoc.AppendChild(xmlElement);
		xmlElement.SetAttribute("fullmenus", XmlConvert.ToString(ร));
		xmlElement.SetAttribute("fullmenushover", XmlConvert.ToString(ฤ));
		xmlElement.SetAttribute("tooltips", XmlConvert.ToString(_০));
		xmlElement.SetAttribute("scintooltip", XmlConvert.ToString(_১));
		xmlElement.SetAttribute("animation", XmlConvert.ToString((int)ஃ));
		XmlElement xmlElement2 = xmlDoc.CreateElement("items");
		xmlElement.AppendChild(xmlElement2);
		foreach (DictionaryEntry item in ณ)
		{
			BaseItem baseItem = item.Value as BaseItem;
			XmlElement xmlElement3 = xmlDoc.CreateElement("item");
			xmlElement2.AppendChild(xmlElement3);
			itemSerializationContext.ItemXmlElement = xmlElement3;
			baseItem.Serialize(itemSerializationContext);
		}
		XmlElement xmlElement4 = xmlDoc.CreateElement("bars");
		xmlElement.AppendChild(xmlElement4);
		if (จ != null)
		{
			SerializeDockSite(จ, xmlElement);
		}
		if (ฉ != null)
		{
			SerializeDockSite(ฉ, xmlElement);
		}
		if (ช != null)
		{
			SerializeDockSite(ช, xmlElement);
		}
		if (ซ != null)
		{
			SerializeDockSite(ซ, xmlElement);
		}
		if (_0E3F && ฌ != null)
		{
			SerializeDockSite(ฌ, xmlElement);
		}
		if (ญ != null)
		{
			SerializeDockSite(ญ, xmlElement4);
		}
		if (ฎ != null)
		{
			SerializeDockSite(ฎ, xmlElement4);
		}
		if (ฏ != null)
		{
			SerializeDockSite(ฏ, xmlElement4);
		}
		if (ฐ != null)
		{
			SerializeDockSite(ฐ, xmlElement4);
		}
		foreach (Bar item2 in ฑ)
		{
			if (item2.DockSide == eDockSide.None)
			{
				XmlElement xmlElement5 = xmlDoc.CreateElement("bar");
				xmlElement4.AppendChild(xmlElement5);
				item2.ణ(xmlElement5);
			}
		}
		XmlElement xmlElement6 = xmlDoc.CreateElement("popups");
		xmlElement.AppendChild(xmlElement6);
		foreach (BaseItem item3 in ฟ)
		{
			XmlElement xmlElement7 = xmlDoc.CreateElement("item");
			xmlElement6.AppendChild(xmlElement7);
			itemSerializationContext.ItemXmlElement = xmlElement7;
			item3.Serialize(itemSerializationContext);
		}
	}

	public void LoadLayout(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		LoadLayout(xmlDocument);
	}

	internal void LoadLayout(XmlDocument xmlDoc)
	{
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Invalid comparison between Unknown and I4
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Invalid comparison between Unknown and I4
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Invalid comparison between Unknown and I4
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Invalid comparison between Unknown and I4
		if (!(xmlDoc.FirstChild is XmlElement xmlElement) || !xmlElement.HasAttribute(_290D._2914) || XmlConvert.ToInt32(xmlElement.GetAttribute(_290D._2914)) < 6)
		{
			return;
		}
		DestroyAutoHidePanels();
		bool flag = false;
		_0E3B = true;
		try
		{
			if (xmlElement.Name != "dotnetbarlayout")
			{
				throw new InvalidOperationException("Invalid file format (dotnetbarlayout expected).");
			}
			if (!SuspendLayout)
			{
				flag = true;
				SuspendLayout = true;
			}
			foreach (XmlElement childNode in xmlElement.ChildNodes)
			{
				if (!(childNode.Name == "bars"))
				{
					continue;
				}
				foreach (XmlElement childNode2 in childNode.ChildNodes)
				{
					if (!(childNode2.Name != "bar"))
					{
						Bar bar = null;
						if (ฑ.Contains(ฑ[childNode2.GetAttribute("name")]))
						{
							bar = ฑ[childNode2.GetAttribute("name")];
						}
						else
						{
							bar = new Bar();
							Bars.Add(bar);
						}
						bar.బ(childNode2);
					}
				}
			}
			foreach (XmlElement childNode3 in xmlElement.ChildNodes)
			{
				if (childNode3.Name == _290D._290E)
				{
					if (ฌ != null)
					{
						ฌ.GetDocumentUIManager().బ(childNode3);
					}
				}
				else
				{
					if (!(childNode3.Name == _290D._2911))
					{
						continue;
					}
					DockStyle val = (DockStyle)Enum.Parse(typeof(DockStyle), childNode3.GetAttribute(_290D._2912));
					if ((int)val == 3)
					{
						if (ช != null)
						{
							ช.GetDocumentUIManager().బ(childNode3);
						}
					}
					else if ((int)val == 4)
					{
						if (ซ != null)
						{
							ซ.GetDocumentUIManager().బ(childNode3);
						}
					}
					else if ((int)val == 1)
					{
						if (จ != null)
						{
							จ.GetDocumentUIManager().బ(childNode3);
						}
					}
					else if ((int)val == 2 && ฉ != null)
					{
						ฉ.GetDocumentUIManager().బ(childNode3);
					}
				}
			}
			if (xmlElement.HasAttribute("zorder"))
			{
				RestoreDockSiteZOrder(xmlElement.GetAttribute("zorder"));
			}
			Bar[] array = new Bar[ฑ.Count];
			ฑ.CopyTo(array);
			Bar[] array2 = array;
			foreach (Bar bar2 in array2)
			{
				if (bar2.CustomBar && bar2.Items.Count == 0)
				{
					bar2.Visible = false;
					Bars.Remove(bar2);
					((Component)(object)bar2).Dispose();
				}
			}
		}
		finally
		{
			if (flag)
			{
				SuspendLayout = false;
			}
			_0E3B = false;
		}
		if (ท != null && ธ)
		{
			ธ = false;
			OnMdiChildResize(ท, new EventArgs());
		}
	}

	public void SaveLayout(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		SaveLayout(xmlDocument);
		xmlDocument.Save(FileName);
	}

	internal void SaveLayout(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("dotnetbarlayout");
		xmlElement.SetAttribute(_290D._2914, "6");
		xmlDoc.AppendChild(xmlElement);
		xmlElement.SetAttribute("zorder", GetDockSitesZOrder());
		if (จ != null)
		{
			SerializeDockSiteLayout(จ, xmlElement);
		}
		if (ฉ != null)
		{
			SerializeDockSiteLayout(ฉ, xmlElement);
		}
		if (ช != null)
		{
			SerializeDockSiteLayout(ช, xmlElement);
		}
		if (ซ != null)
		{
			SerializeDockSiteLayout(ซ, xmlElement);
		}
		if (ฌ != null)
		{
			SerializeDockSiteLayout(ฌ, xmlElement);
		}
		XmlElement xmlElement2 = xmlDoc.CreateElement("bars");
		xmlElement.AppendChild(xmlElement2);
		if (ญ != null)
		{
			SerializeDockSiteLayout(ญ, xmlElement2);
		}
		if (ฎ != null)
		{
			SerializeDockSiteLayout(ฎ, xmlElement2);
		}
		if (ฏ != null)
		{
			SerializeDockSiteLayout(ฏ, xmlElement2);
		}
		if (ฐ != null)
		{
			SerializeDockSiteLayout(ฐ, xmlElement2);
		}
		foreach (Bar item in ฑ)
		{
			if (item.DockSide == eDockSide.None && item.Name != "" && item.SaveLayoutChanges)
			{
				XmlElement xmlElement3 = xmlDoc.CreateElement("bar");
				xmlElement2.AppendChild(xmlElement3);
				item.ఫ(xmlElement3);
			}
		}
	}

	private string GetDockSitesZOrder()
	{
		string text = "";
		if (จ != null && ((Control)จ).Parent != null)
		{
			text += ((Control)จ).Parent.Controls.IndexOf((Control)(object)จ);
		}
		text += ",";
		if (ฉ != null && ((Control)ฉ).Parent != null)
		{
			text += ((Control)ฉ).Parent.Controls.IndexOf((Control)(object)ฉ);
		}
		text += ",";
		if (ช != null && ((Control)ช).Parent != null)
		{
			text += ((Control)ช).Parent.Controls.IndexOf((Control)(object)ช);
		}
		text += ",";
		if (ซ != null)
		{
			text += ((Control)ซ).Parent.Controls.IndexOf((Control)(object)ซ);
		}
		return text;
	}

	private void RestoreDockSiteZOrder(string s)
	{
		try
		{
			string[] array = s.Split(new char[1] { ',' });
			if (array[0] != "" && จ != null && ((Control)จ).Parent != null)
			{
				((Control)จ).Parent.Controls.SetChildIndex((Control)(object)จ, int.Parse(array[0]));
			}
			if (array[1] != "" && ฉ != null && ((Control)ฉ).Parent != null)
			{
				((Control)ฉ).Parent.Controls.SetChildIndex((Control)(object)ฉ, int.Parse(array[1]));
			}
			if (array[2] != "" && ช != null && ((Control)ช).Parent != null)
			{
				((Control)ช).Parent.Controls.SetChildIndex((Control)(object)ช, int.Parse(array[2]));
			}
			if (array[3] != "" && ซ != null && ((Control)ซ).Parent != null)
			{
				((Control)ซ).Parent.Controls.SetChildIndex((Control)(object)ซ, int.Parse(array[3]));
			}
		}
		catch
		{
		}
	}

	private void SerializeDockSite(DockSite site, XmlElement xmlBars)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (site.ၓ || site.DocumentDockContainer != null)
		{
			site.GetDocumentUIManager().Ⴃ(xmlBars);
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)site).Controls)
		{
			Control val = item;
			if (val is Bar bar)
			{
				XmlElement xmlElement = xmlBars.OwnerDocument.CreateElement("bar");
				xmlBars.AppendChild(xmlElement);
				bar.ణ(xmlElement);
			}
		}
	}

	private void SerializeDockSiteLayout(DockSite site, XmlElement xmlBars)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (site.ၓ || site.DocumentDockContainer != null)
		{
			site.GetDocumentUIManager().ఫ(xmlBars);
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)site).Controls)
		{
			Control val = item;
			if (val is Bar bar && bar.Name != "" && bar.SaveLayoutChanges)
			{
				XmlElement xmlElement = xmlBars.OwnerDocument.CreateElement("bar");
				xmlBars.AppendChild(xmlElement);
				bar.ఫ(xmlElement);
			}
		}
	}

	internal void SetFloatingBarVisible(bool bVisible)
	{
		foreach (Bar item in ฑ)
		{
			if (item.DockSide == eDockSide.None && item.Visible != bVisible)
			{
				item.Visible = bVisible;
			}
		}
	}

	public void Customize()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (ต == null)
		{
			ต = new ArrayList();
		}
		Control topLevelControl = ((Control)บ).TopLevelControl;
		Form val = (Form)(object)((topLevelControl is Form) ? topLevelControl : null);
		if (val == null)
		{
			val = ParentForm;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)val).Controls)
		{
			Control val2 = item;
			if (!(val2 is DockSite) && val2.Enabled)
			{
				val2.Enabled = false;
				ต.Add(val2);
			}
		}
		_๑ = true;
		if (Items.Count == 0)
		{
			_๐ = true;
			RescanCategories();
		}
		if (!ห)
		{
			if (ด == null)
			{
				ด = new frmCustomize(this);
			}
			if (_0DFE != null)
			{
				_0DFE(ด, new EventArgs());
			}
			((Control)ด).Show();
			((Form)ด).Owner = ParentForm;
		}
		else if (_0DFE != null)
		{
			_0DFE(null, new EventArgs());
		}
		else
		{
			MessageBox.Show("You need to add event handler for EnterCustomize event since your UseCustomCustomizeDialog property is set to true.");
		}
	}

	private void RescanCategories()
	{
		if (Bars.Count == 0)
		{
			return;
		}
		Items.Clear();
		foreach (Bar bar in Bars)
		{
			if (bar.LayoutType != 0)
			{
				continue;
			}
			foreach (BaseItem item in bar.Items)
			{
				AutoCategorizeItem(item);
			}
		}
	}

	private void AutoCategorizeItem(BaseItem item)
	{
		if (item.Category != "" && item.Name != "" && !Items.Contains(item.Name))
		{
			Items.Add(item.Copy());
		}
		foreach (BaseItem subItem in item.SubItems)
		{
			AutoCategorizeItem(subItem);
		}
	}

	public ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		foreach (Bar item in ฑ)
		{
			if (!((Control)item).IsDisposed)
			{
				GetSubItemsByName(item.ItemsContainer, ItemName, arrayList);
			}
		}
		if (แ != null)
		{
			GetSubItemsByName(แ.ItemsContainer, ItemName, arrayList);
		}
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, bool FullSearch)
	{
		if (!FullSearch)
		{
			return GetItems(ItemName);
		}
		ArrayList arrayList = new ArrayList(15);
		foreach (Bar item in ฑ)
		{
			if (!((Control)item).IsDisposed)
			{
				GetSubItemsByName(item.ItemsContainer, ItemName, arrayList);
			}
		}
		foreach (DictionaryEntry item2 in ณ)
		{
			BaseItem baseItem = (BaseItem)item2.Value;
			if (baseItem.Name == ItemName)
			{
				arrayList.Add(baseItem);
			}
			GetSubItemsByName(baseItem, ItemName, arrayList);
		}
		foreach (BaseItem item3 in ฟ)
		{
			if (item3.Name == ItemName)
			{
				arrayList.Add(item3);
			}
			GetSubItemsByName(item3, ItemName, arrayList);
		}
		return arrayList;
	}

	public ArrayList GetItems(string itemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		foreach (Bar item in ฑ)
		{
			if (!((Control)item).IsDisposed)
			{
				BarFunctions.GetSubItemsByNameAndType(item.ItemsContainer, itemName, arrayList, itemType, useGlobalName: false);
			}
		}
		if (แ != null && !((Control)แ).IsDisposed)
		{
			BarFunctions.GetSubItemsByNameAndType(แ.ItemsContainer, itemName, arrayList, itemType, useGlobalName: false);
		}
		return arrayList;
	}

	public ArrayList GetItems(string itemName, Type itemType, bool fullSearch)
	{
		return GetItems(itemName, itemType, fullSearch, useGlobalName: false);
	}

	public ArrayList GetItems(string itemName, Type itemType, bool fullSearch, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		if (ฑ == null || IsDisposed)
		{
			return arrayList;
		}
		foreach (Bar item in ฑ)
		{
			if (!((Control)item).IsDisposed)
			{
				BarFunctions.GetSubItemsByNameAndType(item.ItemsContainer, itemName, arrayList, itemType, useGlobalName);
			}
		}
		if (แ != null && !((Control)แ).IsDisposed)
		{
			BarFunctions.GetSubItemsByNameAndType(แ.ItemsContainer, itemName, arrayList, itemType, useGlobalName);
		}
		if (fullSearch)
		{
			foreach (DictionaryEntry item2 in ณ)
			{
				BaseItem baseItem = (BaseItem)item2.Value;
				if (((!useGlobalName && baseItem.Name == itemName) || (useGlobalName && baseItem.GlobalName == itemName)) && (object)baseItem.GetType() == itemType)
				{
					arrayList.Add(baseItem);
				}
				BarFunctions.GetSubItemsByNameAndType(baseItem, itemName, arrayList, itemType, useGlobalName);
			}
			foreach (BaseItem item3 in ฟ)
			{
				if (((!useGlobalName && item3.Name == itemName) || (useGlobalName && item3.GlobalName == itemName)) && (object)item3.GetType() == itemType)
				{
					arrayList.Add(item3);
				}
				BarFunctions.GetSubItemsByNameAndType(item3, itemName, arrayList, itemType, useGlobalName);
			}
		}
		return arrayList;
	}

	public BaseItem GetItem(string ItemName)
	{
		foreach (Bar item in ฑ)
		{
			if (!((Control)item).IsDisposed)
			{
				BaseItem subItemByName = GetSubItemByName(item.ItemsContainer, ItemName);
				if (subItemByName != null)
				{
					return subItemByName;
				}
			}
		}
		if (แ != null)
		{
			return GetSubItemByName(แ.ItemsContainer, ItemName);
		}
		return null;
	}

	public BaseItem GetItem(string ItemName, bool FullSearch)
	{
		foreach (Bar item in ฑ)
		{
			if (!((Control)item).IsDisposed)
			{
				BaseItem subItemByName = GetSubItemByName(item.ItemsContainer, ItemName);
				if (subItemByName != null)
				{
					return subItemByName;
				}
			}
		}
		if (แ != null && !((Control)แ).IsDisposed)
		{
			BaseItem subItemByName2 = GetSubItemByName(แ.ItemsContainer, ItemName);
			if (subItemByName2 != null)
			{
				return subItemByName2;
			}
		}
		foreach (DictionaryEntry item2 in ณ)
		{
			BaseItem baseItem = (BaseItem)item2.Value;
			if (baseItem.Name == ItemName)
			{
				return baseItem;
			}
			BaseItem subItemByName3 = GetSubItemByName(baseItem, ItemName);
			if (subItemByName3 != null)
			{
				return subItemByName3;
			}
		}
		foreach (BaseItem item3 in ฟ)
		{
			if (item3.Name == ItemName)
			{
				return item3;
			}
			BaseItem subItemByName4 = GetSubItemByName(item3, ItemName);
			if (subItemByName4 != null)
			{
				return subItemByName4;
			}
		}
		return null;
	}

	private void GetSubItemsByName(BaseItem objParent, string ItemName, ArrayList list)
	{
		if (objParent is GenericItemContainer)
		{
			GenericItemContainer genericItemContainer = objParent as GenericItemContainer;
			if (genericItemContainer._0DBC != null)
			{
				GetSubItemsByName(genericItemContainer._0DBC, ItemName, list);
			}
		}
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if (subItem.Name == ItemName)
			{
				list.Add(subItem);
			}
			if (subItem.SubItems.Count > 0)
			{
				GetSubItemsByName(subItem, ItemName, list);
			}
		}
	}

	private void GetSubItemsByNameAndType(BaseItem objParent, string ItemName, ArrayList list, Type itemType)
	{
		if (objParent is GenericItemContainer)
		{
			GenericItemContainer genericItemContainer = objParent as GenericItemContainer;
			if (genericItemContainer._0DBC != null)
			{
				GetSubItemsByNameAndType(genericItemContainer._0DBC, ItemName, list, itemType);
			}
		}
		if (objParent.SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if ((object)subItem.GetType() == itemType && subItem.Name == ItemName)
			{
				list.Add(subItem);
			}
			if (subItem.SubItems.Count > 0)
			{
				GetSubItemsByNameAndType(subItem, ItemName, list, itemType);
			}
		}
	}

	private BaseItem GetSubItemByName(BaseItem objParent, string ItemName)
	{
		if (objParent is GenericItemContainer)
		{
			GenericItemContainer genericItemContainer = objParent as GenericItemContainer;
			if (genericItemContainer._0DBC != null)
			{
				BaseItem subItemByName = GetSubItemByName(genericItemContainer._0DBC, ItemName);
				if (subItemByName != null)
				{
					return subItemByName;
				}
			}
		}
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if (subItem.Name == ItemName)
			{
				return subItem;
			}
			if (subItem.SubItems.Count > 0)
			{
				BaseItem subItemByName2 = GetSubItemByName(subItem, ItemName);
				if (subItemByName2 != null)
				{
					return subItemByName2;
				}
			}
		}
		return null;
	}

	public void BeginModalDisplay()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (IsDisposed || ฑ == null)
		{
			return;
		}
		foreach (Bar item in ฑ)
		{
			if (item.DockSide == eDockSide.None && ((Control)item).Parent is Form && ((Control)item).Parent.Visible)
			{
				((Form)((Control)item).Parent).TopMost = false;
			}
		}
	}

	public void EndModalDisplay()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (IsDisposed || ฑ == null)
		{
			return;
		}
		foreach (Bar item in ฑ)
		{
			if (item.Visible && item.DockSide == eDockSide.None && ((Control)item).Parent is Form && !((Form)((Control)item).Parent).TopMost)
			{
				((Form)((Control)item).Parent).TopMost = true;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void SetDesignMode(bool designmode)
	{
		foreach (Bar item in ฑ)
		{
			item.SetDesignMode(designmode);
			item.RecalcLayout();
		}
	}

	void IOwner.SetFocusItem(BaseItem objFocusItem)
	{
		if (_09D9 != null && _09D9 != objFocusItem)
		{
			_09D9.OnLostFocus();
		}
		_09D9 = objFocusItem;
		if (_09D9 != null)
		{
			_09D9.OnGotFocus();
		}
	}

	BaseItem IOwner.GetFocusItem()
	{
		return _09D9;
	}

	void IOwner.SetExpandedItem(BaseItem objItem)
	{
		if (objItem != null && objItem.Parent is PopupItem)
		{
			return;
		}
		if (_09D8 != null)
		{
			if (_09D8.Expanded)
			{
				_09D8.Expanded = false;
			}
			_09D8 = null;
		}
		_09D8 = objItem;
	}

	BaseItem IOwner.GetExpandedItem()
	{
		return _09D8;
	}

	[Browsable(false)]
	public void CustomizeClosing()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		_๑ = false;
		if (ต == null)
		{
			return;
		}
		foreach (Control item in ต)
		{
			Control val = item;
			val.Enabled = true;
		}
		ต.Clear();
		ต = null;
		ด = null;
		if (_0DFF != null)
		{
			_0DFF(this, new EventArgs());
		}
		if (_๐)
		{
			_๐ = false;
			Items.Clear();
		}
	}

	void IOwner.DesignTimeContextMenu(BaseItem objItem)
	{
		if (ด != null)
		{
			ด.DesignTimeContextMenu(objItem);
		}
		else if (ห)
		{
			OnCustomizeContextMenu(objItem, null);
		}
	}

	void IOwner.OnParentPositionChanging()
	{
		if (ฑ == null)
		{
			return;
		}
		Bar[] array = new Bar[ฑ.Count];
		ฑ.CopyTo(array);
		Bar[] array2 = array;
		foreach (Bar bar in array2)
		{
			if (!((Control)bar).IsDisposed && ((Control)bar).IsHandleCreated && bar.BarState == eBarState.Floating)
			{
				_089F.PostMessage(((Control)bar).Handle, 1125, IntPtr.Zero, IntPtr.Zero);
			}
		}
	}

	void IOwner.StartItemDrag(BaseItem objItem)
	{
		if (ก != null)
		{
			ก(objItem, EventArgs.Empty);
		}
		if (ด != null)
		{
			objItem.Expanded = false;
			ด.DragItem = objItem;
			_089F.PostMessage(((Control)ด).Handle, 1731, IntPtr.Zero, IntPtr.Zero);
		}
	}

	internal void OnCustomizeContextMenu(object Sender, ButtonItem Parent)
	{
		if (_0DFB != null)
		{
			_0DFB(Sender, new CustomizeContextMenuEventArgs(Parent));
		}
	}

	public void RegisterPopup(PopupItem objPopup)
	{
		if (ਏ.Contains(objPopup))
		{
			return;
		}
		if (!base.DesignMode)
		{
			if (!_২ && !_0A0B)
			{
				_1BE6._1BEA(this);
				_২ = true;
			}
			else if (_0A0B && _0A0E == null)
			{
				_0A0E = new ᒺ(this);
			}
		}
		else if (_0A0E == null)
		{
			_0A0E = new ᒺ(this);
		}
		if (ਏ.Count == 0)
		{
			PopupManager.RegisterPopup(this);
		}
		ਏ.Add(objPopup);
		if (objPopup.GetOwner() == null)
		{
			objPopup.SetOwner(this);
		}
	}

	bool IOwnerMenuSupport.RelayMouseHover()
	{
		foreach (PopupItem item in ਏ)
		{
			Control popupControl = item.PopupControl;
			if (popupControl == null)
			{
				continue;
			}
			Point pt = popupControl.PointToClient(Control.MousePosition);
			if (popupControl.ClientRectangle.Contains(pt))
			{
				if (popupControl is MenuPanel)
				{
					((MenuPanel)(object)popupControl).ఆ();
				}
				else if (popupControl is Bar)
				{
					((Bar)(object)popupControl).ఆ();
				}
				return true;
			}
		}
		return false;
	}

	public void UnregisterPopup(PopupItem objPopup)
	{
		if (ਏ.Contains(objPopup))
		{
			ਏ.Remove(objPopup);
		}
		if (ਏ.Count == 0 && _0A0E != null && (!_0A0B || base.DesignMode))
		{
			_0A0E.Dispose();
			_0A0E = null;
		}
		if (ਏ.Count == 0)
		{
			PopupManager.UnregisterPopup(this);
		}
	}

	private void ProcessDelayedCommands()
	{
		if (!base.DesignMode || จ == null || ฉ == null || ช == null || ซ == null || ((Control)จ).Parent == null || ((Control)ฉ).Parent == null || ((Control)ช).Parent == null || ((Control)ซ).Parent == null)
		{
			return;
		}
		foreach (Bar bar in Bars)
		{
			bar._0BF1();
		}
	}

	private void DestroyAutoHidePanels()
	{
		if (ฮ != null)
		{
			if (((Control)ฮ).Parent != null)
			{
				((Control)ฮ).Parent.Controls.Remove((Control)(object)ฮ);
			}
			((Component)(object)ฮ).Dispose();
			ฮ = null;
		}
		if (ฯ != null)
		{
			if (((Control)ฯ).Parent != null)
			{
				((Control)ฯ).Parent.Controls.Remove((Control)(object)ฯ);
			}
			((Component)(object)ฯ).Dispose();
			ฯ = null;
		}
		if (ะ != null)
		{
			if (((Control)ะ).Parent != null)
			{
				((Control)ะ).Parent.Controls.Remove((Control)(object)ะ);
			}
			((Component)(object)ะ).Dispose();
			ะ = null;
		}
		if (_0E31 != null)
		{
			if (((Control)_0E31).Parent != null)
			{
				((Control)_0E31).Parent.Controls.Remove((Control)(object)_0E31);
			}
			((Component)(object)_0E31).Dispose();
			_0E31 = null;
		}
	}

	private void DockSiteParentChanged(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			if (val == จ && val.Parent is UserControl)
			{
				SetupParentUserControl();
			}
			val.ParentChanged -= DockSiteParentChanged;
			if ((val.Parent != null && val.Parent.FindForm() != null) || base.DesignMode || !(val.Parent is UserControl))
			{
				BarStreamLoad();
				ProcessDelayedCommands();
			}
		}
	}

	private void SetupParentUserControl()
	{
		if (((จ != null && ((Control)จ).Parent is UserControl) || _0E36 != null) && !base.DesignMode)
		{
			Control val = null;
			val = (Control)((จ == null) ? ((object)_0E36) : ((object)/*isinst with value type is only supported in some contexts*/));
			Form val2 = val.FindForm();
			if (val2 != null)
			{
				ParentForm = val2;
			}
			else
			{
				val.ParentChanged += ParentUserControlParentChanged;
			}
		}
	}

	private void ParentUserControlParentChanged(object sender, EventArgs e)
	{
		Control val = null;
		val = (Control)((จ == null) ? ((object)_0E36) : ((object)/*isinst with value type is only supported in some contexts*/));
		Control val2 = (Control)((sender is Control) ? sender : null);
		if (val2 != null)
		{
			val2.ParentChanged -= ParentUserControlParentChanged;
		}
		Form val3 = val.FindForm();
		if (val3 != null)
		{
			ParentForm = val3;
		}
		else
		{
			if (val.Parent == null)
			{
				return;
			}
			while (val2.Parent != null)
			{
				val2 = val2.Parent;
			}
			if (val2 is PopupContainerControl)
			{
				PopupContainerControl popupContainerControl = val2 as PopupContainerControl;
				if (popupContainerControl.ParentItem != null && popupContainerControl.ParentItem.ContainerControl is Control)
				{
					object containerControl = popupContainerControl.ParentItem.ContainerControl;
					Control val4 = (Control)((containerControl is Control) ? containerControl : null);
					Form val5 = val4.FindForm();
					if (val5 != null)
					{
						ParentForm = val5;
						if (!ำ)
						{
							BarStreamLoad(bForceLoad: true);
						}
						return;
					}
				}
			}
			val2.ParentChanged += ParentUserControlParentChanged;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		լ.Refresh();
		foreach (Bar item in ฑ)
		{
			((Control)item).Invalidate();
		}
	}

	MdiClient IOwner.GetMdiClient(Form MdiForm)
	{
		return BarFunctions.GetMdiClient(MdiForm);
	}

	public void MdiChildActivated()
	{
		OnMdiChildActivate(ParentForm, new EventArgs());
	}

	private void DelayCheckMaximizedState()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (_๒ == null)
		{
			_๒ = new Timer();
			_๒.Interval = 200;
			_๒.Tick += DelayCheckTimerTick;
		}
		_๒.Start();
	}

	private void DelayCheckTimerTick(object sender, EventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		Timer val = _๒;
		_๒ = null;
		val.Stop();
		val.Tick -= DelayCheckTimerTick;
		((Component)(object)val).Dispose();
		if (ท != null && (int)ท.WindowState == 2)
		{
			MdiChildActivated();
		}
	}

	private void OnMdiChildActivate(object sender, EventArgs e)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		if (ท != null)
		{
			((Control)ท).Resize -= OnMdiChildResize;
			((Control)ท).VisibleChanged -= OnMdiChildVisibleChanged;
		}
		ท = null;
		if (ParentForm.ActiveMdiChild != null)
		{
			if (GetMenuBar() != null)
			{
				ท = ParentForm.ActiveMdiChild;
				((Control)ท).Resize += OnMdiChildResize;
				((Control)ท).VisibleChanged += OnMdiChildVisibleChanged;
				if ((int)ท.WindowState == 2 || ธ)
				{
					OnMdiChildResize(ท, null);
				}
				else
				{
					DelayCheckMaximizedState();
				}
			}
		}
		else if (ธ)
		{
			GetMenuBar()?.ర(ఱ: true);
		}
	}

	private void OnMdiChildVisibleChanged(object sender, EventArgs e)
	{
		if (ท != null && !((Control)ท).Visible && ธ)
		{
			GetMenuBar()?.ర(ఱ: true);
			ธ = false;
		}
	}

	private void OnMdiChildResize(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		Bar menuBar = GetMenuBar();
		bool flag = false;
		if (ท != null && (int)ท.WindowState != 2)
		{
			if (ธ && ฬ)
			{
				if (e == null)
				{
					ธ = false;
					return;
				}
				menuBar?.ర(ఱ: false);
				flag = true;
			}
			ธ = false;
			if (menuBar != null)
			{
				((Control)menuBar).Refresh();
			}
			return;
		}
		if (!ธ)
		{
			if (ฬ)
			{
				if (menuBar == null)
				{
					return;
				}
				menuBar.ల(ท, ఱ: false);
				flag = true;
			}
			ธ = true;
		}
		if (menuBar != null && flag)
		{
			menuBar.RecalcLayout();
		}
	}

	private Bar GetMenuBar()
	{
		if (ฑ == null)
		{
			return null;
		}
		try
		{
			foreach (Bar item in ฑ)
			{
				if (item.MenuBar && item.Visible)
				{
					return item;
				}
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	private void OnMdiSetMenu(object sender, EventArgs e)
	{
		_20EF obj = sender as _20EF;
		if (GetMenuBar() != null || _0E3E)
		{
			obj._20F3 = true;
		}
	}

	bool ሎ.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Invalid comparison between Unknown and I4
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected I4, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Invalid comparison between Unknown and I4
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected I4, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		if ((!ผ && !(Form.ActiveForm is ᆡ)) || base.DesignMode)
		{
			return false;
		}
		if (บ != null && บ.IsMdiChild && บ.MdiParent != null && !((Control)บ.MdiParent).Focused)
		{
			return false;
		}
		Bar menuBar = GetMenuBar();
		int num = _0EFE._0F11(wParam);
		if (menuBar != null && menuBar.ItemsContainer != null && !menuBar.ItemsContainer.DesignMode)
		{
			GenericItemContainer itemsContainer = menuBar.ItemsContainer;
			if (itemsContainer == null)
			{
				return false;
			}
			if (num == 18 || (num == 121 && !_0BC9 && ((int)Control.ModifierKeys == 0 || (int)Control.ModifierKeys == 262144)))
			{
				if (itemsContainer.ExpandedItem() != null && (((Control)menuBar).Focused || menuBar._09A9))
				{
					menuBar.ReleaseFocus();
					menuBar._09A9 = false;
					_0BC7 = true;
					return true;
				}
			}
			else
			{
				if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
				{
					int key = Control.ModifierKeys | num;
					if (ProcessShortcut((eShortcut)key))
					{
						return true;
					}
				}
				_0BC7 = true;
				if (num >= 27 && num <= 111)
				{
					int num2 = (int)_089F.MapVirtualKey((uint)(int)wParam, 2u);
					if (num2 == 0)
					{
						num2 = num;
					}
					if (itemsContainer._෫(num2))
					{
						return true;
					}
				}
			}
		}
		else if (menuBar == null && !base.DesignMode && ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123)))
		{
			int key2 = Control.ModifierKeys | num;
			if (ProcessShortcut((eShortcut)key2))
			{
				return true;
			}
		}
		if ((Control.ModifierKeys & 0x40000) == 262144 && num > 27 && ฑ != null)
		{
			_0BC7 = true;
			int num3 = 0;
			if ((Control.ModifierKeys & 0x10000) == 65536)
			{
				try
				{
					byte[] array = new byte[256];
					if (_089F.GetKeyboardState(array))
					{
						byte[] array2 = new byte[2];
						if (_089F.ToAscii((uint)(int)wParam, 0u, array, array2, 0u) != 0)
						{
							num3 = array2[0];
						}
					}
				}
				catch (Exception)
				{
					num3 = 0;
				}
			}
			if (num3 == 0)
			{
				num3 = (int)_089F.MapVirtualKey((uint)(int)wParam, 2u);
			}
			if (num3 != 0)
			{
				foreach (Bar item in ฑ)
				{
					if (item.ItemsContainer != null && !item.ItemsContainer.DesignMode && ((Control)item).Enabled && item.Visible && item.ItemsContainer._෫(num3))
					{
						return true;
					}
				}
			}
		}
		if (num == 18)
		{
			_๓ = true;
		}
		return false;
	}

	bool ሎ.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if ((!ผ && !(Form.ActiveForm is ᆡ)) || base.DesignMode)
		{
			return false;
		}
		if (บ != null && บ.IsMdiChild && บ.MdiParent != null && !((Control)บ.MdiParent).Focused)
		{
			return false;
		}
		int num = _0EFE._0F11(wParam);
		if ((num == 18 && _๓) || num == 121)
		{
			_๓ = false;
			if (_0BC7)
			{
				_0BC7 = false;
				return false;
			}
			if (_0BC8)
			{
				_0BC8 = false;
				return true;
			}
			foreach (Bar bar in Bars)
			{
				if (bar.Visible)
				{
					bar._0C4C();
				}
			}
			if (num == 18 || (num == 121 && !_0BC9))
			{
				Bar menuBar = GetMenuBar();
				if (menuBar != null && !menuBar.ItemsContainer.DesignMode)
				{
					if (menuBar._09A9)
					{
						menuBar._09A9 = false;
					}
					else
					{
						if (!ผ)
						{
							บ.Activate();
						}
						menuBar._09A9 = true;
					}
					return true;
				}
			}
		}
		return false;
	}

	bool ሎ.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool ሎ.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected I4, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		bool designMode = base.DesignMode;
		int num = _0EFE._0F11(wParam);
		if (ਏ.Count > 0 && ((BaseItem)ਏ[ਏ.Count - 1]).Parent == null)
		{
			PopupItem popupItem = (PopupItem)ਏ[ਏ.Count - 1];
			Control popupControl = popupItem.PopupControl;
			Control val = Control.FromChildHandle(hWnd);
			if (val != null)
			{
				while (val.Parent != null)
				{
					val = val.Parent;
				}
			}
			bool flag = false;
			if (val != null && popupItem != null)
			{
				flag = popupItem.IsAnyOnHandle(val.Handle);
			}
			bool flag2 = (popupControl != null && val != null && popupControl.Handle == val.Handle) || flag;
			if (!flag)
			{
				Keys val2 = (Keys)_089F.MapVirtualKey((uint)(int)wParam, 2u);
				if ((int)val2 == 0)
				{
					val2 = (Keys)num;
				}
				popupItem.InternalKeyDown(new KeyEventArgs(val2));
			}
			if (flag2)
			{
				return false;
			}
			return !designMode;
		}
		if (FocusedBar != null && !designMode)
		{
			bool flag3 = true;
			Control val3 = Control.FromChildHandle(hWnd);
			if (val3 != null)
			{
				while (val3.Parent != null)
				{
					val3 = val3.Parent;
				}
				if ((val3 is MenuPanel || val3 is Bar || val3 is PopupContainer || val3 is PopupContainerControl) && val3.Handle != hWnd)
				{
					flag3 = false;
				}
			}
			if (flag3)
			{
				Keys val4 = (Keys)_089F.MapVirtualKey((uint)(int)wParam, 2u);
				if ((int)val4 == 0)
				{
					val4 = (Keys)num;
				}
				FocusedBar._0A81(new KeyEventArgs(val4));
				return true;
			}
		}
		else if (num == 27)
		{
			foreach (Bar bar in Bars)
			{
				if (bar.ଅ)
				{
					bar._0BFE();
					break;
				}
			}
		}
		if ((!ผ && !(Form.ActiveForm is ᆡ)) || designMode)
		{
			return false;
		}
		if (num >= 112 || (int)Control.ModifierKeys != 0 || (_0EFE._0F11(lParam) & 0x1000000000L) != 0 || num == 46 || num == 45)
		{
			int key = Control.ModifierKeys | num;
			return ProcessShortcut((eShortcut)key);
		}
		return false;
	}

	private bool ProcessShortcut(eShortcut key)
	{
		bool result = BarFunctions.ProcessItemsShortcuts(key, _09DA);
		if (อ != null && อ.Contains(key))
		{
			result = false;
		}
		if (!_৩)
		{
			return result;
		}
		return false;
	}

	bool ሎ.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (_๓)
		{
			_0BC7 = true;
		}
		if (ਏ == null || base.DesignMode)
		{
			return false;
		}
		if (ਏ.Count == 0)
		{
			Bar menuBar = GetMenuBar();
			if (menuBar != null && menuBar._09A9 && hWnd != ((Control)menuBar).Handle)
			{
				menuBar._09A9 = false;
			}
			return false;
		}
		for (int num = ਏ.Count - 1; num >= 0; num--)
		{
			PopupItem popupItem = ਏ[num] as PopupItem;
			object containerControl = popupItem.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			bool flag = popupItem.IsAnyOnHandle(hWnd);
			if (!flag)
			{
				Control val2 = Control.FromChildHandle(hWnd);
				if (val2 != null)
				{
					if (val2 is MenuPanel)
					{
						flag = true;
					}
					else
					{
						while (val2.Parent != null)
						{
							val2 = val2.Parent;
							if (((object)val2).GetType().FullName.IndexOf("DropDownHolder") >= 0 || val2 is MenuPanel || val2 is PopupContainerControl)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						flag = popupItem.IsAnyOnHandle(val2.Handle);
					}
				}
				else
				{
					string text = _089F.स(hWnd);
					text = text.ToLower();
					if (text.IndexOf("combolbox") >= 0)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Control val3 = popupItem.PopupControl;
				if (val3 != null)
				{
					while (val3.Parent != null)
					{
						val3 = val3.Parent;
					}
				}
				if (val3 != null && val3.Bounds.Contains(Control.MousePosition))
				{
					flag = true;
				}
			}
			if (flag)
			{
				break;
			}
			if (val != null && hWnd != val.Handle && !flag)
			{
				if (popupItem.Parent != null)
				{
					_ = base.DesignMode;
				}
				popupItem.Expanded = !popupItem.Expanded;
				if (!popupItem.Expanded && popupItem.Parent != null)
				{
					popupItem.Parent.AutoExpand = false;
				}
			}
			else if (val == null && !flag)
			{
				popupItem.ClosePopup();
			}
			if (ਏ.Count == 0)
			{
				break;
			}
		}
		return false;
	}

	bool ሎ.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (ਏ.Count > 0)
		{
			bool flag = true;
			foreach (BaseItem item in ਏ)
			{
				Control popupControl = ((PopupItem)item).PopupControl;
				Control val = null;
				if (item.Parent != null)
				{
					object containerControl = item.Parent.ContainerControl;
					val = (Control)((containerControl is Control) ? containerControl : null);
				}
				Control val2 = Control.FromChildHandle(hWnd);
				if (val2 is MenuPanel && val2.Parent != null)
				{
					val2 = val2.Parent;
				}
				else if (val2 != null)
				{
					Control val3 = val2;
					while (val3.Parent != null)
					{
						val3 = val3.Parent;
					}
					if (val3 is Bar || val3 is PopupContainerControl || val3 is PopupContainer)
					{
						val2 = val3;
					}
				}
				if (popupControl != null && val2 != null && popupControl != val2 && !item.IsAnyOnHandle(val2.Handle) && (val == null || (val != null && val != val2)))
				{
					flag = flag;
					continue;
				}
				flag = false;
				break;
			}
			return flag;
		}
		return false;
	}

	void IOwnerBarSupport.BarContextMenu(Control bar, MouseEventArgs e)
	{
		if (!ShowCustomizeContextMenu || ด != null || base.DesignMode)
		{
			return;
		}
		ԃ = new ButtonItem("sys_customizecontextnmenu");
		eDotNetBarStyle style = eDotNetBarStyle.OfficeXP;
		if (ฑ.Count > 0)
		{
			style = ฑ[0].Style;
		}
		ԃ.Style = style;
		ButtonItem buttonItem;
		foreach (Bar item in ฑ)
		{
			if ((item.LayoutType != eLayoutType.DockContainer || item.Items.Count != 0) && item.CanHide && ((item.LayoutType == eLayoutType.DockContainer && item.CanCustomize) || item.LayoutType != eLayoutType.DockContainer) && ((Control)item).Text != "")
			{
				buttonItem = new ButtonItem();
				buttonItem.Text = ((Control)item).Text;
				buttonItem.Checked = item.Visible || item.AutoHide;
				buttonItem.SetSystemItem(b: true);
				buttonItem.Tag = item;
				buttonItem.Click += CustomizeItemClick;
				ԃ.SubItems.Add(buttonItem);
			}
		}
		buttonItem = new ButtonItem("customize");
		if (ԃ.SubItems.Count > 0)
		{
			buttonItem.BeginGroup = true;
		}
		using (LocalizationManager localizationManager = new LocalizationManager(this))
		{
			buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.CustomizeItemCustomize);
		}
		buttonItem.SetSystemItem(b: true);
		buttonItem.Style = style;
		buttonItem.Click += CustomizeItemClick;
		ԃ.SubItems.Add(buttonItem);
		OnCustomizeContextMenu(this, (ButtonItem)ԃ);
		RegisterPopup(ԃ);
		ԃ.PopupMenu(bar.PointToScreen(new Point(e.X, e.Y)));
	}

	private void CustomizeItemClick(object sender, EventArgs e)
	{
		BaseItem baseItem = sender as BaseItem;
		ԃ.Expanded = false;
		if (baseItem.Name == "customize")
		{
			Customize();
		}
		else
		{
			Bar bar = baseItem.Tag as Bar;
			if (bar.AutoHide)
			{
				bar.AutoHide = false;
			}
			if (!bar.Visible && bar.LayoutType == eLayoutType.DockContainer && bar.VisibleItemCount == 0 && bar.Items.Count > 0)
			{
				bar.Items[0].Visible = true;
			}
			bar.Visible = !bar.Visible;
			bar.RecalcLayout();
			bar.ఒ();
		}
		ԃ.Dispose();
		ԃ = null;
	}

	public void ResetUsageData()
	{
		foreach (Bar item in ฑ)
		{
			foreach (BaseItem item2 in item.Items)
			{
				if (item2 is IPersonalizedMenuItem { MenuVisibility: eMenuVisibility.VisibleIfRecentlyUsed } personalizedMenuItem)
				{
					personalizedMenuItem.RecentlyUsed = false;
				}
				if (item2.SubItems.Count > 0)
				{
					ResetItemUsageData(item2);
				}
			}
		}
	}

	private void ResetItemUsageData(BaseItem item)
	{
		foreach (BaseItem subItem in item.SubItems)
		{
			if (subItem is IPersonalizedMenuItem { MenuVisibility: eMenuVisibility.VisibleIfRecentlyUsed } personalizedMenuItem)
			{
				personalizedMenuItem.RecentlyUsed = false;
			}
			if (subItem.SubItems.Count > 0)
			{
				ResetItemUsageData(subItem);
			}
		}
	}

	void IOwnerMenuSupport.ClosePopups()
	{
		ClosePopups();
	}

	private void ClosePopups()
	{
		ArrayList arrayList = new ArrayList(ਏ);
		foreach (PopupItem item in arrayList)
		{
			item.ClosePopup();
		}
	}

	void IOwnerItemEvents.InvokeItemTextChanged(BaseItem item, EventArgs e)
	{
		if (_09D0 != null)
		{
			_09D0(item, e);
		}
	}

	void IOwner.InvokeResetDefinition(BaseItem item, EventArgs e)
	{
		if (_0DFD != null)
		{
			_0DFD(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerLoad(PopupItem item, EventArgs e)
	{
		if (_09BE != null)
		{
			_09BE(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerUnload(PopupItem item, EventArgs e)
	{
		if (_09BF != null)
		{
			_09BF(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupOpen(PopupItem item, PopupOpenEventArgs e)
	{
		if (_09C0 != null)
		{
			_09C0(item, e);
		}
		โ++;
	}

	void IOwnerMenuSupport.InvokePopupClose(PopupItem item, EventArgs e)
	{
		if (_09C1 != null)
		{
			_09C1(item, e);
		}
		if (โ > ใ || ใ <= 0)
		{
			โ = 0;
			Ӕ ӕ2 = new Ӕ();
			ӕ2.Ḕ();
		}
	}

	void IOwnerMenuSupport.InvokePopupShowing(PopupItem item, EventArgs e)
	{
		if (_09C2 != null)
		{
			_09C2(item, e);
		}
	}

	void IOwnerItemEvents.InvokeExpandedChange(BaseItem item, EventArgs e)
	{
		if (_09C3 != null)
		{
			_09C3(item, e);
		}
	}

	void IOwnerBarSupport.InvokeBarDock(Bar bar, EventArgs e)
	{
		if (_0B47 != null)
		{
			_0B47(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBarUndock(Bar bar, EventArgs e)
	{
		if (_0B48 != null)
		{
			_0B48(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBeforeDockTabDisplay(BaseItem item, EventArgs e)
	{
		if (_0DFC != null)
		{
			_0DFC(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseDown(BaseItem item, MouseEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (ग़ != null)
		{
			ग़.Invoke((object)item, e);
		}
		if (item.ClickAutoRepeat && (int)e.Button == 1048576)
		{
			_৯ = item;
			if (_৮ == null)
			{
				_৮ = new Timer();
			}
			_৮.Interval = item.ClickRepeatInterval;
			_৮.Tick += ClickTimerTick;
			_৮.Start();
		}
	}

	void IOwnerItemEvents.InvokeMouseEnter(BaseItem item, EventArgs e)
	{
		if (બ != null)
		{
			બ(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseLeave(BaseItem item, EventArgs e)
	{
		if (ભ != null)
		{
			ભ(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseHover(BaseItem item, EventArgs e)
	{
		if (ય != null)
		{
			ય(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseMove(BaseItem item, MouseEventArgs e)
	{
		if (મ != null)
		{
			મ.Invoke((object)item, e);
		}
	}

	private void ClickTimerTick(object sender, EventArgs e)
	{
		if (_৯ != null)
		{
			_৯.RaiseClick();
		}
		else
		{
			_৮.Stop();
		}
	}

	void IOwnerItemEvents.InvokeMouseUp(BaseItem item, MouseEventArgs e)
	{
		if (ફ != null)
		{
			ફ.Invoke((object)item, e);
		}
		if (_৮ != null && _৮.Enabled)
		{
			_৮.Stop();
			_৮.Enabled = false;
		}
	}

	void IOwnerItemEvents.InvokeGotFocus(BaseItem item, EventArgs e)
	{
		if (પ != null)
		{
			પ(item, e);
		}
	}

	void IOwnerItemEvents.InvokeLostFocus(BaseItem item, EventArgs e)
	{
		if (_0AA9 != null)
		{
			_0AA9(item, e);
		}
	}

	void IOwner.InvokeUserCustomize(object sender, EventArgs e)
	{
		if (_09CC != null)
		{
			_09CC(sender, e);
		}
	}

	void IOwner.InvokeEndUserCustomize(object sender, EndUserCustomizeEventArgs e)
	{
		if (ฃ != null)
		{
			ฃ(sender, e);
		}
	}

	void IOwner.InvokeDefinitionLoaded(object sender, EventArgs e)
	{
		if (_09D3 != null)
		{
			_09D3(sender, e);
		}
	}

	void IOwnerItemEvents.InvokeItemRemoved(BaseItem item, BaseItem parent, int itemIndex)
	{
		if (_09CD != null)
		{
			_09CD(item, new ItemRemovedEventArgs(parent, itemIndex));
		}
	}

	void IOwnerItemEvents.InvokeItemAdded(BaseItem item, EventArgs e)
	{
		if (ৎ != null)
		{
			ৎ(item, e);
		}
	}

	void IOwnerItemEvents.InvokeContainerLoadControl(BaseItem item, EventArgs e)
	{
		if (_09CF != null)
		{
			_09CF(item, e);
		}
	}

	void IOwnerItemEvents.InvokeContainerControlDeserialize(BaseItem item, ControlContainerSerializationEventArgs e)
	{
		if (_09D2 != null)
		{
			_09D2(item, e);
		}
	}

	void IOwnerItemEvents.InvokeContainerControlSerialize(BaseItem item, ControlContainerSerializationEventArgs e)
	{
		if (_09D1 != null)
		{
			_09D1(item, e);
		}
	}

	void IOwnerItemEvents.InvokeOptionGroupChanging(BaseItem item, OptionGroupChangingEventArgs e)
	{
		if (_09D5 != null)
		{
			_09D5(item, e);
		}
	}

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (_09BB != null)
		{
			_09BB(item, e);
		}
	}

	void IOwnerItemEvents.InvokeToolTipShowing(object item, EventArgs e)
	{
		if (_09D6 != null)
		{
			_09D6(item, e);
		}
	}

	void IOwnerBarSupport.InvokeDockTabChange(Bar bar, DockTabChangeEventArgs e)
	{
		if (_0B49 != null)
		{
			_0B49(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBarClosing(Bar bar, BarClosingEventArgs e)
	{
		if (_0E00 != null)
		{
			_0E00(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBarTearOff(Bar bar, EventArgs e)
	{
		if (ข != null)
		{
			ข(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeAutoHideChanged(Bar bar, EventArgs e)
	{
		if (_0B4B != null)
		{
			_0B4B(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeAutoHideDisplay(Bar bar, AutoHideDisplayEventArgs e)
	{
		if (_0B4D != null)
		{
			_0B4D(bar, e);
		}
	}

	internal bool IsHandleOwnedByPopup(IntPtr handle)
	{
		if (ਏ.Count > 0)
		{
			foreach (BaseItem item in ਏ)
			{
				Control popupControl = ((PopupItem)item).PopupControl;
				if ((popupControl != null && popupControl.Handle == handle) || item.IsAnyOnHandle(handle))
				{
					return true;
				}
			}
		}
		return false;
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
	{
		if (_09D4 != null)
		{
			_09D4(this, e);
		}
	}

	public string GetContextMenuEx(Control control)
	{
		string text = (string)ล[control];
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}

	public void SetContextMenuEx(Control control, string value)
	{
		if (value == null)
		{
			value = string.Empty;
		}
		if (value.Length == 0)
		{
			if (ล.Contains(control))
			{
				ล.Remove(control);
			}
		}
		else if (ล.Contains(control))
		{
			ล[control] = value;
		}
		else
		{
			ล[control] = value;
		}
	}
}
