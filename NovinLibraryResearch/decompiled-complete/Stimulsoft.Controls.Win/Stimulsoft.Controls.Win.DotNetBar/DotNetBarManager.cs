using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(DotNetBarManager), "DotNetBarManager.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
[DefaultEvent("ItemClick")]
public class DotNetBarManager : Component, IExtenderProvider, IOwnerItemEvents, IOwnerMenuSupport, IOwner, IOwnerBarSupport, IOwnerAutoHideSupport, IMessageHandlerClient, IOwnerLocalize, ICustomSerialization
{
	public delegate void CustomizeContextMenuEventHandler(object sender, CustomizeContextMenuEventArgs e);

	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

	public delegate void PopupOpenEventHandler(object sender, PopupOpenEventArgs e);

	public delegate void DockTabChangeEventHandler(object sender, DockTabChangeEventArgs e);

	public delegate void BarClosingEventHandler(object sender, BarClosingEventArgs e);

	public delegate void AutoHideDisplayEventHandler(object sender, AutoHideDisplayEventArgs e);

	public delegate void LocalizeStringEventHandler(object sender, LocalizeEventArgs e);

	[CompilerGenerated]
	private MouseEventHandler m_ItemDoubleClick;

	[CompilerGenerated]
	private MouseEventHandler m_MouseDown;

	[CompilerGenerated]
	private MouseEventHandler m_MouseUp;

	[CompilerGenerated]
	private MouseEventHandler m_MouseMove;

	private static Hashtable m_MdiChildParentHandlers = new Hashtable(10);

	private License m_License;

	private DockSite m_TopDockSite;

	private DockSite m_BottomDockSite;

	private DockSite m_LeftDockSite;

	private DockSite m_RightDockSite;

	private DockSite m_FillDockSite;

	private DockSite m_ToolbarTopDockSite;

	private DockSite m_ToolbarBottomDockSite;

	private DockSite m_ToolbarLeftDockSite;

	private DockSite m_ToolbarRightDockSite;

	private Bars m_Bars;

	private ArrayList m_WereVisible;

	private Items m_Items;

	private Hashtable m_ShortcutTable;

	private bool m_FilterInstalled;

	private frmCustomize m_frmCustomize;

	private ArrayList m_DisabledControls;

	private BaseItem m_FocusItem;

	private BaseItem m_ExpandedItem;

	private ArrayList m_RegisteredPopups;

	private MDIClientMsgHandler m_MdiHandler;

	private Form m_ActiveMdiChild;

	private bool m_MdiChildMaximized;

	private bool m_IgnoreSysKeyUp;

	private bool m_EatSysKeyUp;

	private ImageList m_ImageList;

	private ImageList m_ImageListMedium;

	private ImageList m_ImageListLarge;

	private DotNetBarStreamer m_BarStreamer;

	private Form m_ParentForm;

	private bool m_Disposed;

	private bool m_ParentActivated;

	private bool m_PersonalizedAllVisible;

	private Timer m_ClickTimer;

	private BaseItem m_ClickRepeatItem;

	private bool m_ShowCustomizeContextMenu = true;

	private PopupItem m_ContextMenu;

	private ContextMenusCollection m_ContextMenus;

	private eMenuDropShadow m_MenuDropShadow;

	private bool m_AlphaBlendShadow = true;

	private bool m_UseHook;

	private Hook m_Hook;

	private bool m_ShowResetButton;

	private bool m_AlwaysShowFullMenus;

	private bool m_ShowFullMenusOnHover = true;

	private bool m_ShowToolTips = true;

	private bool m_ShowShortcutKeysInToolTips;

	private ePopupAnimation m_PopupAnimation = ePopupAnimation.SystemDefault;

	private Hashtable m_ContextExMenus = new Hashtable();

	private Hashtable m_ContextExHandlers = new Hashtable();

	private bool m_AllowUserBarCustomize = true;

	private bool m_EventControlAdded;

	private bool m_MdiChildActivateHandled;

	private bool m_MdiClientHandleCreatedHandled;

	private bool m_UseCustomCustomizeDialog;

	private bool m_MdiSystemItemVisible = true;

	private bool m_DispatchShortcuts;

	private ShortcutsCollection m_AutoDispatchShortcuts;

	private AutoHidePanel m_LeftAutoHidePanel;

	private AutoHidePanel m_RightAutoHidePanel;

	private AutoHidePanel m_TopAutoHidePanel;

	private AutoHidePanel m_BottomAutoHidePanel;

	private bool m_SuspendLayout;

	private string m_DefinitionName = "";

	private bool m_DefinitionLoaded;

	private bool m_IgnoreF10Key;

	private bool m_ThemeAware;

	private eDotNetBarStyle m_Style = eDotNetBarStyle.Office2003;

	private bool m_DefinitionCodeSerialize;

	private Bar m_FocusedBar;

	private Control m_ParentUserControl;

	private bool m_DockingHints = true;

	internal bool IgnoreLoadedControlDispose;

	private bool m_AlwaysDisplayKeyAccelerators;

	private bool m_EnableFullSizeDock = true;

	private bool m_LoadingDefinition;

	private bool m_LoadingLayout;

	private ColorScheme m_ColorScheme;

	private bool m_UseGlobalColorScheme;

	private bool m_ApplyDocumentBarStyle = true;

	private bool m_HideMdiSystemMenu;

	private bool m_IncludeDockDocumentsInDefinition;

	private Size m_MinimumClientSize = new Size(48, 48);

	private ContextMenuBar m_GlobalContextMenuBar;

	private bool _EnableDockCtrlTabKey = true;

	private DockContainerItem _ActiveDockContainerItem;

	private bool m_DisposeGCCollect = true;

	private DockingHint m_DockingHintLeft;

	private DockingHint m_DockingHintRight;

	private DockingHint m_DockingHintTop;

	private DockingHint m_DockingHintBottom;

	private DockingHint m_DockingHintMiddle;

	private bool m_DockingHintSetup;

	private bool m_ClosingHints;

	private bool m_DisabledImagesGrayScale = true;

	private bool m_AutoCreatedCategories;

	private bool m_Customizing;

	private Timer _DelayCheckTimer;

	private bool _ActivateOnLayoutLoad = true;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Ctrl+Tab keys cycles the currently selected dock tab")]
	public bool EnableDockCtrlTabKey
	{
		get
		{
			return _EnableDockCtrlTabKey;
		}
		set
		{
			_EnableDockCtrlTabKey = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates Context menu bar associated with the DotNetBarManager which is used as part of Global Items feature.")]
	[Category("Data")]
	public ContextMenuBar GlobalContextMenuBar
	{
		get
		{
			return m_GlobalContextMenuBar;
		}
		set
		{
			if (m_GlobalContextMenuBar != null)
			{
				m_GlobalContextMenuBar.GlobalParentComponent = null;
			}
			m_GlobalContextMenuBar = value;
			if (m_GlobalContextMenuBar != null)
			{
				m_GlobalContextMenuBar.GlobalParentComponent = this;
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
			return m_ParentForm;
		}
		set
		{
			if (m_ParentForm != null)
			{
				ReleaseParentFormHooks();
			}
			m_ParentForm = value;
			OnParentChanged();
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public Control ParentUserControl
	{
		get
		{
			return m_ParentUserControl;
		}
		set
		{
			m_ParentUserControl = value;
			SetupParentUserControl();
		}
	}

	[Browsable(false)]
	public DockContainerItem ActiveDockContainerItem => _ActiveDockContainerItem;

	private bool IsPopupProviderOnly
	{
		get
		{
			if (m_TopDockSite == null && m_BottomDockSite == null && m_LeftDockSite == null)
			{
				return m_RightDockSite == null;
			}
			return false;
		}
	}

	ArrayList IOwnerBarSupport.WereVisible => m_WereVisible;

	[Browsable(false)]
	public bool IsDisposed => m_Disposed;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool DisposeGCCollect
	{
		get
		{
			return m_DisposeGCCollect;
		}
		set
		{
			m_DisposeGCCollect = value;
		}
	}

	[Browsable(true)]
	[Category("Docking")]
	[Description("Indicates minimum client size that docking windows will try to maintain for the client area (not occupied by dock windows).")]
	public Size MinimumClientSize
	{
		get
		{
			return m_MinimumClientSize;
		}
		set
		{
			m_MinimumClientSize = value;
		}
	}

	[Obsolete]
	[Browsable(false)]
	[DefaultValue(true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Docking")]
	[Description("Indicates whether DotNetBar provides docking hints for easy docking of bars.")]
	public bool DockingHintsEnabled
	{
		get
		{
			return m_DockingHints;
		}
		set
		{
			m_DockingHints = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Indicates whether user can control how first bar is docked when using docking hints.")]
	public bool EnableFullSizeDock
	{
		get
		{
			return m_EnableFullSizeDock;
		}
		set
		{
			m_EnableFullSizeDock = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("")]
	public bool ApplyDocumentBarStyle
	{
		get
		{
			return m_ApplyDocumentBarStyle;
		}
		set
		{
			m_ApplyDocumentBarStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Bars Bars => m_Bars;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string DefinitionName
	{
		get
		{
			return m_DefinitionName;
		}
		set
		{
			m_DefinitionName = value;
			m_DefinitionLoaded = false;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool IsDefinitionLoaded => m_DefinitionLoaded;

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
			if (m_BarStreamer != null)
			{
				return m_BarStreamer;
			}
			return new DotNetBarStreamer(this);
		}
		set
		{
			m_BarStreamer = null;
			if (value != null && value.Data != null)
			{
				m_BarStreamer = value;
				m_DefinitionLoaded = false;
				BarStreamLoad();
			}
		}
	}

	[Browsable(false)]
	public Items Items => m_Items;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Obsolete("ContextMenus on DotNetBarManager is removed. Use ContextMenuBar control instead.")]
	public ContextMenusCollection ContextMenus => m_ContextMenus;

	private bool IsDocumentDockingEnabled => m_FillDockSite != null;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
	public bool DispatchShortcuts
	{
		get
		{
			return m_DispatchShortcuts;
		}
		set
		{
			m_DispatchShortcuts = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates list of shortcut keys that are automatically dispatched to the control that has focus even if they are handled and used by one of the items.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsDesigner, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ShortcutsCollection AutoDispatchShortcuts
	{
		get
		{
			return m_AutoDispatchShortcuts;
		}
		set
		{
			m_AutoDispatchShortcuts = value;
			m_AutoDispatchShortcuts.Parent = null;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether Reset buttons is shown that allows end-user to reset the toolbar state.")]
	public bool ShowResetButton
	{
		get
		{
			return m_ShowResetButton;
		}
		set
		{
			m_ShowResetButton = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Data")]
	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	public ImageList Images
	{
		get
		{
			return m_ImageList;
		}
		set
		{
			if (m_ImageList != null)
			{
				((Component)(object)m_ImageList).Disposed -= ImageListDisposed;
			}
			m_ImageList = value;
			if (m_ImageList != null)
			{
				((Component)(object)m_ImageList).Disposed += ImageListDisposed;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Data")]
	[Description("ImageList for medium-sized images used on Items.")]
	public ImageList ImagesMedium
	{
		get
		{
			return m_ImageListMedium;
		}
		set
		{
			if (m_ImageListMedium != null)
			{
				((Component)(object)m_ImageListMedium).Disposed -= ImageListDisposed;
			}
			m_ImageListMedium = value;
			if (m_ImageListMedium != null)
			{
				((Component)(object)m_ImageListMedium).Disposed += ImageListDisposed;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Data")]
	[Description("ImageList for large-sized images used on Items.")]
	public ImageList ImagesLarge
	{
		get
		{
			return m_ImageListLarge;
		}
		set
		{
			if (m_ImageListLarge != null)
			{
				((Component)(object)m_ImageListLarge).Disposed -= ImageListDisposed;
			}
			m_ImageListLarge = value;
			if (m_ImageListLarge != null)
			{
				((Component)(object)m_ImageListLarge).Disposed += ImageListDisposed;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool SuspendLayout
	{
		get
		{
			return m_SuspendLayout;
		}
		set
		{
			if (m_SuspendLayout == value)
			{
				return;
			}
			m_SuspendLayout = value;
			if (!m_SuspendLayout)
			{
				if (m_TopDockSite != null && m_TopDockSite.NeedsLayout)
				{
					m_TopDockSite.RecalcLayout();
				}
				if (m_BottomDockSite != null && m_BottomDockSite.NeedsLayout)
				{
					m_BottomDockSite.RecalcLayout();
				}
				if (m_LeftDockSite != null && m_LeftDockSite.NeedsLayout)
				{
					m_LeftDockSite.RecalcLayout();
				}
				if (m_RightDockSite != null && m_RightDockSite.NeedsLayout)
				{
					m_RightDockSite.RecalcLayout();
				}
				if (m_FillDockSite != null && m_FillDockSite.NeedsLayout)
				{
					m_FillDockSite.RecalcLayout();
				}
				if (m_ToolbarTopDockSite != null && m_ToolbarTopDockSite.NeedsLayout)
				{
					m_ToolbarTopDockSite.RecalcLayout();
				}
				if (m_ToolbarBottomDockSite != null && m_ToolbarBottomDockSite.NeedsLayout)
				{
					m_ToolbarBottomDockSite.RecalcLayout();
				}
				if (m_ToolbarLeftDockSite != null && m_ToolbarLeftDockSite.NeedsLayout)
				{
					m_ToolbarLeftDockSite.RecalcLayout();
				}
				if (m_ToolbarRightDockSite != null && m_ToolbarRightDockSite.NeedsLayout)
				{
					m_ToolbarRightDockSite.RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eMenuDropShadow.SystemDefault)]
	[Category("Behavior")]
	[Description("Specifes whether drop shadow is displayed for Menus and pop-up Bars. OfficeXP Style only.")]
	public eMenuDropShadow MenuDropShadow
	{
		get
		{
			return m_MenuDropShadow;
		}
		set
		{
			m_MenuDropShadow = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Specifes whether to use Alpha-Blending shadows for pop-up items if supported by target OS. Disabling Alpha-Blended shadows can improve performance.")]
	public bool AlphaBlendShadow
	{
		get
		{
			return m_AlphaBlendShadow;
		}
		set
		{
			m_AlphaBlendShadow = value;
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
			return m_DisabledImagesGrayScale;
		}
		set
		{
			m_DisabledImagesGrayScale = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
	public bool UseHook
	{
		get
		{
			return m_UseHook;
		}
		set
		{
			if (m_UseHook == value)
			{
				return;
			}
			m_UseHook = value;
			if (m_ParentForm == null)
			{
				return;
			}
			if (m_UseHook)
			{
				if (m_FilterInstalled)
				{
					MessageHandler.UnregisterMessageClient(this);
				}
				m_FilterInstalled = false;
				if (m_Hook == null)
				{
					m_Hook = new Hook(this);
				}
				return;
			}
			if (m_Hook != null)
			{
				m_Hook.Dispose();
				m_Hook = null;
			}
			if (!base.DesignMode)
			{
				if (!m_FilterInstalled)
				{
					MessageHandler.RegisterMessageClient(this);
				}
				m_FilterInstalled = true;
			}
		}
	}

	bool IOwnerMenuSupport.ShowPopupShadow
	{
		get
		{
			if (m_MenuDropShadow == eMenuDropShadow.Show)
			{
				return true;
			}
			if (m_MenuDropShadow == eMenuDropShadow.Hide)
			{
				return false;
			}
			if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor < 1) || Environment.OSVersion.Version.Major < 5)
			{
				return true;
			}
			return NativeFunctions.ShowDropShadow;
		}
	}

	bool ICustomSerialization.HasSerializeItemHandlers => this.SerializeItem != null;

	bool ICustomSerialization.HasDeserializeItemHandlers => this.DeserializeItem != null;

	internal bool IsLoadingDefinition => m_LoadingDefinition;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IncludeDockDocumentsInDefinition
	{
		get
		{
			return m_IncludeDockDocumentsInDefinition;
		}
		set
		{
			m_IncludeDockDocumentsInDefinition = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
	public bool IsLoadingLayout => m_LoadingLayout;

	internal bool IsCustomizing => m_Customizing;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Specifies that custom customize dialog will be used. Use EnterCustomize event to show your custom dialog box.")]
	public bool UseCustomCustomizeDialog
	{
		get
		{
			return m_UseCustomCustomizeDialog;
		}
		set
		{
			m_UseCustomCustomizeDialog = value;
		}
	}

	BaseItem IOwner.DragItem
	{
		get
		{
			if (m_frmCustomize != null)
			{
				return m_frmCustomize.DragItem;
			}
			return null;
		}
	}

	bool IOwner.DragInProgress
	{
		get
		{
			if (m_frmCustomize != null)
			{
				return m_frmCustomize.DragInProgress;
			}
			return false;
		}
	}

	internal Form CustomizeForm => (Form)(object)m_frmCustomize;

	[Browsable(false)]
	[DefaultValue(null)]
	public DockSite ToolbarTopDockSite
	{
		get
		{
			return m_ToolbarTopDockSite;
		}
		set
		{
			m_ToolbarTopDockSite = value;
			if (m_ToolbarTopDockSite != null)
			{
				((Control)m_ToolbarTopDockSite).Dock = (DockStyle)1;
				m_ToolbarTopDockSite.SetOwner(this);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DockSite ToolbarBottomDockSite
	{
		get
		{
			return m_ToolbarBottomDockSite;
		}
		set
		{
			m_ToolbarBottomDockSite = value;
			if (m_ToolbarBottomDockSite != null)
			{
				((Control)m_ToolbarBottomDockSite).Dock = (DockStyle)2;
				m_ToolbarBottomDockSite.SetOwner(this);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DockSite ToolbarLeftDockSite
	{
		get
		{
			return m_ToolbarLeftDockSite;
		}
		set
		{
			m_ToolbarLeftDockSite = value;
			if (m_ToolbarLeftDockSite != null)
			{
				((Control)m_ToolbarLeftDockSite).Dock = (DockStyle)3;
				m_ToolbarLeftDockSite.SetOwner(this);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public DockSite ToolbarRightDockSite
	{
		get
		{
			return m_ToolbarRightDockSite;
		}
		set
		{
			m_ToolbarRightDockSite = value;
			if (m_ToolbarRightDockSite != null)
			{
				((Control)m_ToolbarRightDockSite).Dock = (DockStyle)4;
				m_ToolbarRightDockSite.SetOwner(this);
			}
		}
	}

	[Browsable(false)]
	public DockSite TopDockSite
	{
		get
		{
			return m_TopDockSite;
		}
		set
		{
			m_TopDockSite = value;
			if (m_TopDockSite == null)
			{
				return;
			}
			((Control)m_TopDockSite).Dock = (DockStyle)1;
			m_TopDockSite.SetOwner(this);
			if (((Control)m_TopDockSite).Parent == null)
			{
				((Control)m_TopDockSite).ParentChanged += DockSiteParentChanged;
				return;
			}
			if (((Control)m_TopDockSite).Parent is UserControl)
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
			return m_BottomDockSite;
		}
		set
		{
			m_BottomDockSite = value;
			if (m_BottomDockSite != null)
			{
				((Control)m_BottomDockSite).Dock = (DockStyle)2;
				m_BottomDockSite.SetOwner(this);
				if (((Control)m_BottomDockSite).Parent == null)
				{
					((Control)m_BottomDockSite).ParentChanged += DockSiteParentChanged;
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
			return m_LeftDockSite;
		}
		set
		{
			m_LeftDockSite = value;
			if (m_LeftDockSite != null)
			{
				((Control)m_LeftDockSite).Dock = (DockStyle)3;
				m_LeftDockSite.SetOwner(this);
				if (((Control)m_LeftDockSite).Parent == null)
				{
					((Control)m_LeftDockSite).ParentChanged += DockSiteParentChanged;
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
			return m_RightDockSite;
		}
		set
		{
			m_RightDockSite = value;
			if (m_RightDockSite != null)
			{
				((Control)m_RightDockSite).Dock = (DockStyle)4;
				m_RightDockSite.SetOwner(this);
				if (((Control)m_RightDockSite).Parent == null)
				{
					((Control)m_RightDockSite).ParentChanged += DockSiteParentChanged;
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
			return m_FillDockSite;
		}
		set
		{
			m_FillDockSite = value;
			if (m_FillDockSite != null)
			{
				m_FillDockSite.SetOwner(this);
			}
		}
	}

	AutoHidePanel IOwnerAutoHideSupport.LeftAutoHidePanel
	{
		get
		{
			if (m_LeftAutoHidePanel == null)
			{
				if (((Control)m_LeftDockSite).Parent == null)
				{
					return null;
				}
				m_LeftAutoHidePanel = new AutoHidePanel();
				((Control)m_LeftAutoHidePanel).Font = SystemFonts.DefaultFont;
				m_LeftAutoHidePanel.SetOwner(this);
				((Control)m_LeftAutoHidePanel).Dock = (DockStyle)3;
				((Control)m_LeftAutoHidePanel).Width = 0;
				((Control)m_LeftAutoHidePanel).Visible = false;
				((Control)m_LeftDockSite).Parent.Controls.Add((Control)(object)m_LeftAutoHidePanel);
				if (m_LeftDockSite.HasFixedBars)
				{
					((Control)m_LeftDockSite).Parent.Controls.SetChildIndex((Control)(object)m_LeftAutoHidePanel, ((Control)m_LeftDockSite).Parent.Controls.GetChildIndex((Control)(object)m_LeftDockSite));
				}
				else
				{
					((Control)m_LeftDockSite).Parent.Controls.SetChildIndex((Control)(object)m_LeftAutoHidePanel, ((Control)m_LeftDockSite).Parent.Controls.GetChildIndex((Control)(object)m_LeftDockSite) + 1);
				}
			}
			return m_LeftAutoHidePanel;
		}
		set
		{
			m_LeftAutoHidePanel = value;
		}
	}

	bool IOwnerAutoHideSupport.HasLeftAutoHidePanel => m_LeftAutoHidePanel != null;

	AutoHidePanel IOwnerAutoHideSupport.RightAutoHidePanel
	{
		get
		{
			if (m_RightAutoHidePanel == null)
			{
				if (((Control)m_RightDockSite).Parent == null)
				{
					return null;
				}
				m_RightAutoHidePanel = new AutoHidePanel();
				m_RightAutoHidePanel.SetOwner(this);
				((Control)m_RightAutoHidePanel).Dock = (DockStyle)4;
				((Control)m_RightAutoHidePanel).Width = 0;
				((Control)m_RightAutoHidePanel).Visible = false;
				((Control)m_RightDockSite).Parent.Controls.Add((Control)(object)m_RightAutoHidePanel);
				if (m_RightDockSite.HasFixedBars)
				{
					((Control)m_RightDockSite).Parent.Controls.SetChildIndex((Control)(object)m_RightAutoHidePanel, ((Control)m_RightDockSite).Parent.Controls.GetChildIndex((Control)(object)m_RightDockSite));
				}
				else
				{
					((Control)m_RightDockSite).Parent.Controls.SetChildIndex((Control)(object)m_RightAutoHidePanel, ((Control)m_RightDockSite).Parent.Controls.GetChildIndex((Control)(object)m_RightDockSite) + 1);
				}
			}
			return m_RightAutoHidePanel;
		}
		set
		{
			m_RightAutoHidePanel = value;
		}
	}

	bool IOwnerAutoHideSupport.HasRightAutoHidePanel => m_RightAutoHidePanel != null;

	AutoHidePanel IOwnerAutoHideSupport.TopAutoHidePanel
	{
		get
		{
			if (m_TopAutoHidePanel == null)
			{
				if (((Control)m_TopDockSite).Parent == null)
				{
					return null;
				}
				m_TopAutoHidePanel = new AutoHidePanel();
				((Control)m_TopAutoHidePanel).Font = SystemFonts.DefaultFont;
				m_TopAutoHidePanel.SetOwner(this);
				((Control)m_TopAutoHidePanel).Dock = (DockStyle)1;
				((Control)m_TopAutoHidePanel).Width = 0;
				((Control)m_TopAutoHidePanel).Visible = false;
				((Control)m_TopDockSite).Parent.Controls.Add((Control)(object)m_TopAutoHidePanel);
				if (m_TopDockSite.HasFixedBars)
				{
					((Control)m_TopDockSite).Parent.Controls.SetChildIndex((Control)(object)m_TopAutoHidePanel, ((Control)m_TopDockSite).Parent.Controls.GetChildIndex((Control)(object)m_TopDockSite));
				}
				else
				{
					((Control)m_TopDockSite).Parent.Controls.SetChildIndex((Control)(object)m_TopAutoHidePanel, ((Control)m_TopDockSite).Parent.Controls.GetChildIndex((Control)(object)m_TopDockSite) + 1);
				}
			}
			return m_TopAutoHidePanel;
		}
		set
		{
			m_TopAutoHidePanel = value;
		}
	}

	bool IOwnerAutoHideSupport.HasTopAutoHidePanel => m_TopAutoHidePanel != null;

	AutoHidePanel IOwnerAutoHideSupport.BottomAutoHidePanel
	{
		get
		{
			if (m_BottomAutoHidePanel == null)
			{
				if (((Control)m_BottomDockSite).Parent == null)
				{
					return null;
				}
				m_BottomAutoHidePanel = new AutoHidePanel();
				((Control)m_BottomAutoHidePanel).Font = SystemFonts.DefaultFont;
				m_BottomAutoHidePanel.SetOwner(this);
				((Control)m_BottomAutoHidePanel).Dock = (DockStyle)2;
				((Control)m_BottomAutoHidePanel).Width = 0;
				((Control)m_BottomAutoHidePanel).Visible = false;
				((Control)m_BottomDockSite).Parent.Controls.Add((Control)(object)m_BottomAutoHidePanel);
				if (m_BottomDockSite.HasFixedBars)
				{
					((Control)m_BottomDockSite).Parent.Controls.SetChildIndex((Control)(object)m_BottomAutoHidePanel, ((Control)m_BottomDockSite).Parent.Controls.GetChildIndex((Control)(object)m_BottomDockSite));
				}
				else
				{
					((Control)m_BottomDockSite).Parent.Controls.SetChildIndex((Control)(object)m_BottomAutoHidePanel, ((Control)m_BottomDockSite).Parent.Controls.GetChildIndex((Control)(object)m_BottomDockSite) + 1);
				}
			}
			return m_BottomAutoHidePanel;
		}
		set
		{
			m_BottomAutoHidePanel = value;
		}
	}

	bool IOwnerAutoHideSupport.HasBottomAutoHidePanel => m_BottomAutoHidePanel != null;

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether new bars created are drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return m_ThemeAware;
		}
		set
		{
			foreach (Bar bar in m_Bars)
			{
				bar.ThemeAware = value;
			}
			m_ThemeAware = value;
			if (base.DesignMode)
			{
				if (m_TopDockSite != null)
				{
					m_TopDockSite.RecalcLayout();
				}
				if (m_BottomDockSite != null)
				{
					m_BottomDockSite.RecalcLayout();
				}
				if (m_LeftDockSite != null)
				{
					m_LeftDockSite.RecalcLayout();
				}
				if (m_RightDockSite != null)
				{
					m_RightDockSite.RecalcLayout();
				}
			}
			else
			{
				if (m_TopDockSite != null && ((ArrangedElementCollection)((Control)m_TopDockSite).Controls).Count > 0)
				{
					m_TopDockSite.NeedsLayout = true;
				}
				if (m_BottomDockSite != null && ((ArrangedElementCollection)((Control)m_BottomDockSite).Controls).Count > 0)
				{
					m_BottomDockSite.NeedsLayout = true;
				}
				if (m_LeftDockSite != null && ((ArrangedElementCollection)((Control)m_LeftDockSite).Controls).Count > 0)
				{
					m_LeftDockSite.NeedsLayout = true;
				}
				if (m_RightDockSite != null && ((ArrangedElementCollection)((Control)m_RightDockSite).Controls).Count > 0)
				{
					m_RightDockSite.NeedsLayout = true;
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Color Scheme")]
	[Description("Specifies the default visual style of the Bars.")]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			foreach (Bar bar in m_Bars)
			{
				TypeDescriptor.GetProperties(bar)["Style"].SetValue(bar, value);
			}
			foreach (BaseItem contextMenu in m_ContextMenus)
			{
				contextMenu.Style = value;
			}
			foreach (DictionaryEntry item in m_Items)
			{
				((BaseItem)item.Value).Style = value;
			}
			if (m_Style != value)
			{
				m_ColorScheme.SwitchStyle(value);
			}
			m_Style = value;
		}
	}

	[Editor(typeof(ColorSchemeVSEditor), typeof(UITypeEditor))]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Color Scheme")]
	[Description("Indicates Color Scheme for all bars.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorScheme ColorScheme
	{
		get
		{
			return m_ColorScheme;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			m_ColorScheme = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Color Scheme")]
	[Description("Indicates whether ColorScheme object on DotNetBarManager is used as a default ColorScheme for all bars managed by DotNetBarManager.")]
	[DefaultValue(false)]
	public bool UseGlobalColorScheme
	{
		get
		{
			return m_UseGlobalColorScheme;
		}
		set
		{
			m_UseGlobalColorScheme = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether the Personalized menu setting is ignored and full menus are always shown.")]
	public bool AlwaysShowFullMenus
	{
		get
		{
			return m_AlwaysShowFullMenus;
		}
		set
		{
			m_AlwaysShowFullMenus = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether accelerator letters for menu or toolbar commands are underlined regardless of current Windows settings.")]
	public bool AlwaysDisplayKeyAccelerators
	{
		get
		{
			return m_AlwaysDisplayKeyAccelerators;
		}
		set
		{
			m_AlwaysDisplayKeyAccelerators = value;
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
			return Themes.ThemesActive;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether the CustomizeItem (allows toolbar customization) is added for new Bars end users are creating.")]
	public bool AllowUserBarCustomize
	{
		get
		{
			return m_AllowUserBarCustomize;
		}
		set
		{
			m_AllowUserBarCustomize = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether DotNetBar ignores the F10 key which when pressed sets the focus to menu bar.")]
	public bool IgnoreF10Key
	{
		get
		{
			return m_IgnoreF10Key;
		}
		set
		{
			m_IgnoreF10Key = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether the items that are not most recenly used are shown after mouse hovers over the expand button.")]
	public bool ShowFullMenusOnHover
	{
		get
		{
			return m_ShowFullMenusOnHover;
		}
		set
		{
			m_ShowFullMenusOnHover = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether Tooltips are shown on Bars and menus.")]
	public bool ShowToolTips
	{
		get
		{
			return m_ShowToolTips;
		}
		set
		{
			m_ShowToolTips = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether item shortcut is displayed in Tooltips.")]
	public bool ShowShortcutKeysInToolTips
	{
		get
		{
			return m_ShowShortcutKeysInToolTips;
		}
		set
		{
			m_ShowShortcutKeysInToolTips = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(ePopupAnimation.SystemDefault)]
	[Category("Run-time Behavior")]
	[Description("Specifies the pop-up animation style.")]
	public ePopupAnimation PopupAnimation
	{
		get
		{
			return m_PopupAnimation;
		}
		set
		{
			m_PopupAnimation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Specifies whether the MDI system buttons are displayed in menu bar when MDI Child window is maximized.")]
	public bool MdiSystemItemVisible
	{
		get
		{
			return m_MdiSystemItemVisible;
		}
		set
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Invalid comparison between Unknown and I4
			m_MdiSystemItemVisible = value;
			if (base.DesignMode)
			{
				return;
			}
			Bar menuBar = GetMenuBar();
			if (menuBar == null)
			{
				return;
			}
			if (m_MdiSystemItemVisible)
			{
				if (m_ParentForm != null && m_ParentForm.IsMdiContainer && m_ParentForm.ActiveMdiChild != null && (int)m_ParentForm.ActiveMdiChild.WindowState == 2)
				{
					menuBar.ShowMDIChildSystemItems(m_ParentForm.ActiveMdiChild, bRecalcLayout: true);
					m_MdiChildMaximized = true;
				}
			}
			else
			{
				menuBar.ClearMDIChildSystemItems(bRecalcLayout: true);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether MDI Child form System Menu is hidden. Default value is false.")]
	public bool HideMdiSystemMenu
	{
		get
		{
			return m_HideMdiSystemMenu;
		}
		set
		{
			m_HideMdiSystemMenu = value;
		}
	}

	bool IOwner.DesignMode => base.DesignMode;

	Form IOwner.ActiveMdiChild => m_ActiveMdiChild;

	internal Bar FocusedBar
	{
		get
		{
			return m_FocusedBar;
		}
		set
		{
			m_FocusedBar = value;
		}
	}

	bool IMessageHandlerClient.IsModal
	{
		get
		{
			if (m_ParentForm != null && m_ParentActivated)
			{
				return m_ParentForm.Modal;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Gets or sets whether customize context menu is shown on all bars or dock sites.")]
	public bool ShowCustomizeContextMenu
	{
		get
		{
			return m_ShowCustomizeContextMenu;
		}
		set
		{
			m_ShowCustomizeContextMenu = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool PersonalizedAllVisible
	{
		get
		{
			return m_PersonalizedAllVisible;
		}
		set
		{
			m_PersonalizedAllVisible = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool ActivateOnLayoutLoad
	{
		get
		{
			return _ActivateOnLayoutLoad;
		}
		set
		{
			_ActivateOnLayoutLoad = value;
		}
	}

	[Description("Occurs after bar registered with DotNetBarManager state has changed, like selected dock tab has changed, bar has closed, bar has been docked or undocked etc.")]
	public event BarStateChangedEventHandler BarStateChanged;

	[Description("Occurs when focused (active) DockContainerItem has changed. You can use ActiveDockContainerItem property to get reference to currently focused DockContainerItem.")]
	public event ActiveDockContainerChangedEventHandler ActiveDockContainerChanged;

	[Description("Occurs just before customize popup menu is shown.")]
	[Category("Item")]
	public event CustomizeContextMenuEventHandler CustomizeContextMenu;

	[Description("Occurs when Item is clicked.")]
	[Category("Item")]
	public event EventHandler ItemClick;

	[Description("Occurs when Item is double-clicked.")]
	public event MouseEventHandler ItemDoubleClick
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_ItemDoubleClick;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_ItemDoubleClick, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_ItemDoubleClick;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_ItemDoubleClick, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when popup of type container is loading.")]
	[Category("Item")]
	public event EventHandler PopupContainerLoad;

	[Description("Occurs when popup of type container is unloading.")]
	[Category("Item")]
	public event EventHandler PopupContainerUnload;

	[Description("Occurs when popup item is about to open.")]
	[Category("Item")]
	public event PopupOpenEventHandler PopupOpen;

	[Description("Occurs when popup item is closing.")]
	[Category("Item")]
	public event EventHandler PopupClose;

	[Description("Occurs just before popup window is shown.")]
	[Category("Item")]
	public event EventHandler PopupShowing;

	[Description("Occurs when Item Expanded property has changed.")]
	[Category("Item")]
	public event EventHandler ExpandedChange;

	[Description("Occurs when Bar is docked.")]
	[Category("Bar")]
	public event EventHandler BarDock;

	[Description("Occurs when Bar is Undocked.")]
	[Category("Bar")]
	public event EventHandler BarUndock;

	[Description("Occurs before dock tab is displayed.")]
	[Category("Bar")]
	public event EventHandler BeforeDockTabDisplay;

	[Description("Occurs when Bar auto-hide state has changed.")]
	[Category("Bar")]
	public event EventHandler AutoHideChanged;

	[Description("Occurs when mouse button is pressed.")]
	[Category("Item")]
	public event MouseEventHandler MouseDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseDown, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse button is released.")]
	[Category("Item")]
	public event MouseEventHandler MouseUp
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseUp, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseUp, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse enters the item.")]
	[Category("Item")]
	public event EventHandler MouseEnter;

	[Description("Occurs when mouse leaves the item.")]
	[Category("Item")]
	public event EventHandler MouseLeave;

	[Description("Occurs when mouse moves over the item.")]
	[Category("Item")]
	public event MouseEventHandler MouseMove
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseMove, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseMove, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse remains still inside an item for an amount of time.")]
	[Category("Item")]
	public event EventHandler MouseHover;

	[Description("Occurs when item loses input focus.")]
	[Category("Item")]
	public event EventHandler LostFocus;

	[Description("Occurs when item receives input focus.")]
	[Category("Item")]
	public event EventHandler GotFocus;

	[Description("Occurs when user changes the item position, removes the item, adds new item or creates new bar.")]
	[Category("Item")]
	public event EventHandler UserCustomize;

	[Description("Occurs after DotNetBar definition is loaded.")]
	[Category("General")]
	public event EventHandler DefinitionLoaded;

	[Description("Occurs when users wants to reset the DotNetBar to default state.")]
	[Category("General")]
	public event EventHandler ResetDefinition;

	[Description("Occurs after an Item is removed from SubItemsCollection.")]
	[Category("Item")]
	public event ItemRemovedEventHandler ItemRemoved;

	[Description("Occurs after an Item has been added to the SubItemsCollection.")]
	[Category("Item")]
	public event EventHandler ItemAdded;

	[Description("Occurs when ControlContainerControl is created and contained control is needed.")]
	[Category("Item")]
	public event EventHandler ContainerLoadControl;

	[Description("Occurs when Text property of an Item has changed.")]
	[Category("Item")]
	public event EventHandler ItemTextChanged;

	[Description("Occurs when Customize Dialog is about to be shown.")]
	[Category("General")]
	public event EventHandler EnterCustomize;

	[Description("Occurs when Customize Dialog is closed.")]
	[Category("General")]
	public event EventHandler ExitCustomize;

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize;

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize;

	[Description("Occurs when current Dock tab has changed.")]
	[Category("Bar")]
	public event DockTabChangeEventHandler DockTabChange;

	[Description("Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar.")]
	[Category("Bar")]
	public event BarClosingEventHandler BarClosing;

	[Description("Occurs when Bar in auto-hide state is about to be displayed.")]
	[Category("Bar")]
	public event AutoHideDisplayEventHandler AutoHideDisplay;

	[Description("Occurs when user start to drag item when customize dialog is open.")]
	public event EventHandler CustomizeStartItemDrag;

	[Description("Occurs when users Tears-off the Tab from the Bar and new Bar is created as result of that action.")]
	public event EventHandler BarTearOff;

	public event LocalizeStringEventHandler LocalizeString;

	public event OptionGroupChangingEventHandler OptionGroupChanging;

	public event EventHandler ToolTipShowing;

	public event EndUserCustomizeEventHandler EndUserCustomize;

	[Description("Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.")]
	public event DockTabClosingEventHandler DockTabClosing;

	[Description("Occurs on dockable bars after DockContainerItem is closed. This event cannot be cancelled.")]
	public event DockTabClosingEventHandler DockTabClosed;

	public event SerializeItemEventHandler SerializeItem;

	public event SerializeItemEventHandler DeserializeItem;

	public event EventHandler TextBoxItemTextChanged;

	[Description("Occurs when color is choosen from drop-down color picker or from Custom Colors dialog box.")]
	public event EventHandler ColorPickerSelectedColorChanged;

	public event EventHandler ButtonCheckedChanged;

	protected virtual void OnBarStateChanged(BarStateChangedEventArgs e)
	{
		this.BarStateChanged?.Invoke(this, e);
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
		InitializeComponent(cont);
	}

	private void InitializeComponent(IContainer cont)
	{
		m_TopDockSite = null;
		m_BottomDockSite = null;
		m_LeftDockSite = null;
		m_RightDockSite = null;
		m_Bars = new Bars(this);
		m_WereVisible = new ArrayList();
		m_Items = new Items(this);
		m_ShortcutTable = new Hashtable();
		m_FilterInstalled = false;
		m_frmCustomize = null;
		m_DisabledControls = null;
		m_FocusItem = null;
		m_ExpandedItem = null;
		m_RegisteredPopups = new ArrayList();
		m_MdiHandler = null;
		m_ActiveMdiChild = null;
		m_IgnoreSysKeyUp = false;
		m_ImageList = null;
		m_ParentForm = null;
		m_ContextMenus = new ContextMenusCollection(this);
		cont?.Add(this);
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		m_AutoDispatchShortcuts = new ShortcutsCollection(null);
		m_ColorScheme = new ColorScheme(m_Style);
	}

	internal DotNetBarManager(IContainer cont, bool dnbe)
	{
		InitializeComponent(cont);
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
			referenceBar.InvokeBarDockEvents();
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
			bar.TearOffDockContainerItem(itemToDock, floatBar: true, initialFloatLocation);
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
		if (m_MdiChildParentHandlers.Contains(parentForm))
		{
			ParentMsgHandler parentMsgHandler = m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
			return parentMsgHandler.IsRegistered(manager);
		}
		return false;
	}

	internal static void RegisterParentMsgHandler(IOwner manager, Form parentForm)
	{
		if (m_MdiChildParentHandlers.Contains(parentForm))
		{
			ParentMsgHandler parentMsgHandler = m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
			if (!parentMsgHandler.IsRegistered(manager))
			{
				parentMsgHandler.Register(manager);
			}
		}
		else
		{
			ParentMsgHandler parentMsgHandler2 = new ParentMsgHandler(manager.DesignMode);
			((NativeWindow)parentMsgHandler2).AssignHandle(((Control)parentForm).Handle);
			parentMsgHandler2.Register(manager);
			m_MdiChildParentHandlers[parentForm] = parentMsgHandler2;
		}
	}

	internal static void UnRegisterParentMsgHandler(IOwner manager, Form parentForm)
	{
		if (parentForm == null)
		{
			return;
		}
		if (m_MdiChildParentHandlers.Contains(parentForm))
		{
			ParentMsgHandler parentMsgHandler = m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
			if (parentMsgHandler.IsRegistered(manager))
			{
				parentMsgHandler.Unregister(manager);
				if (parentMsgHandler.RegisteredCount == 0 && parentMsgHandler.RegisteredOwnersCount == 0)
				{
					((NativeWindow)parentMsgHandler).ReleaseHandle();
					m_MdiChildParentHandlers.Remove(parentForm);
				}
			}
			return;
		}
		foreach (DictionaryEntry mdiChildParentHandler in m_MdiChildParentHandlers)
		{
			ParentMsgHandler parentMsgHandler2 = mdiChildParentHandler.Value as ParentMsgHandler;
			if (parentMsgHandler2.IsRegistered(manager))
			{
				parentMsgHandler2.Unregister(manager);
				if (parentMsgHandler2.RegisteredCount == 0 && parentMsgHandler2.RegisteredOwnersCount == 0)
				{
					((NativeWindow)parentMsgHandler2).ReleaseHandle();
					m_MdiChildParentHandlers.Remove(parentForm);
				}
				break;
			}
		}
	}

	internal static void RegisterOwnerParentMsgHandler(IOwner owner, Form parentForm)
	{
		if (m_MdiChildParentHandlers.Contains(parentForm))
		{
			ParentMsgHandler parentMsgHandler = m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
			if (!parentMsgHandler.IsOwnerRegistered(owner))
			{
				parentMsgHandler.RegisterOwner(owner);
			}
		}
		else
		{
			ParentMsgHandler parentMsgHandler2 = new ParentMsgHandler(designmode: true);
			((NativeWindow)parentMsgHandler2).AssignHandle(((Control)parentForm).Handle);
			parentMsgHandler2.RegisterOwner(owner);
			m_MdiChildParentHandlers[parentForm] = parentMsgHandler2;
		}
	}

	internal static void UnRegisterOwnerParentMsgHandler(IOwner owner, Form parentForm)
	{
		if (parentForm == null)
		{
			return;
		}
		if (m_MdiChildParentHandlers.Contains(parentForm))
		{
			ParentMsgHandler parentMsgHandler = m_MdiChildParentHandlers[parentForm] as ParentMsgHandler;
			if (parentMsgHandler.IsOwnerRegistered(owner))
			{
				parentMsgHandler.UnregisterOwner(owner);
				if (parentMsgHandler.RegisteredCount == 0 && parentMsgHandler.RegisteredOwnersCount == 0)
				{
					((NativeWindow)parentMsgHandler).ReleaseHandle();
					m_MdiChildParentHandlers.Remove(parentForm);
				}
			}
			return;
		}
		foreach (DictionaryEntry mdiChildParentHandler in m_MdiChildParentHandlers)
		{
			ParentMsgHandler parentMsgHandler2 = mdiChildParentHandler.Value as ParentMsgHandler;
			if (parentMsgHandler2.IsOwnerRegistered(owner))
			{
				parentMsgHandler2.UnregisterOwner(owner);
				if (parentMsgHandler2.RegisteredCount == 0 && parentMsgHandler2.RegisteredOwnersCount == 0)
				{
					((NativeWindow)parentMsgHandler2).ReleaseHandle();
					m_MdiChildParentHandlers.Remove(parentForm);
				}
				break;
			}
		}
	}

	internal static void RelayApplicationActivate(bool bActivate)
	{
		ParentMsgHandler[] array = new ParentMsgHandler[m_MdiChildParentHandlers.Count];
		m_MdiChildParentHandlers.Values.CopyTo(array, 0);
		ParentMsgHandler[] array2 = array;
		foreach (ParentMsgHandler parentMsgHandler in array2)
		{
			if (bActivate)
			{
				parentMsgHandler.ApplicationActivate();
			}
			else
			{
				parentMsgHandler.ApplicationDeactivate();
			}
		}
	}

	internal void InternalDockContainerDeactivated(DockContainerItem item)
	{
		if (_ActiveDockContainerItem != null)
		{
			OnActiveDockContainerChanged(new ActiveDockContainerChangedEventArgs(item, isActive: false));
			_ActiveDockContainerItem = null;
		}
	}

	internal void InternalDockContainerActivated(DockContainerItem item)
	{
		if (_ActiveDockContainerItem != item)
		{
			if (_ActiveDockContainerItem != null)
			{
				InternalDockContainerDeactivated(_ActiveDockContainerItem);
			}
			_ActiveDockContainerItem = item;
			OnActiveDockContainerChanged(new ActiveDockContainerChangedEventArgs(item, isActive: true));
		}
	}

	protected virtual void OnActiveDockContainerChanged(ActiveDockContainerChangedEventArgs e)
	{
		this.ActiveDockContainerChanged?.Invoke(this, e);
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
		MouseEventHandler itemDoubleClick = this.ItemDoubleClick;
		if (itemDoubleClick != null)
		{
			itemDoubleClick.Invoke((object)objItem, e);
		}
	}

	void IOwnerItemEvents.InvokeItemClick(BaseItem objItem)
	{
		if (this.ItemClick != null)
		{
			this.ItemClick(objItem, new EventArgs());
		}
	}

	internal void InvokeDockTabClosing(Bar bar, DockTabClosingEventArgs e)
	{
		if (this.DockTabClosing != null)
		{
			this.DockTabClosing(this, e);
		}
	}

	internal void InvokeDockTabClosed(Bar bar, DockTabClosingEventArgs e)
	{
		if (this.DockTabClosed != null)
		{
			this.DockTabClosed(this, e);
		}
	}

	internal void InvokeTextBoxItemTextChanged(TextBoxItem ti)
	{
		if (this.TextBoxItemTextChanged != null)
		{
			this.TextBoxItemTextChanged(ti, new EventArgs());
		}
	}

	internal void InvokeColorPickerSelectedColorChanged(ColorPickerDropDown d)
	{
		if (this.ColorPickerSelectedColorChanged != null)
		{
			this.ColorPickerSelectedColorChanged(d, new EventArgs());
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
		if (m_ParentForm == null)
		{
			return;
		}
		if (!base.DesignMode)
		{
			UnRegisterParentMsgHandler(this, m_ParentForm);
		}
		if (((Control)m_ParentForm).IsHandleCreated && !base.DesignMode)
		{
			if (m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null)
			{
				RegisterParentMsgHandler(this, m_ParentForm.MdiParent);
			}
			else
			{
				RegisterParentMsgHandler(this, m_ParentForm);
			}
		}
		((Control)m_ParentForm).HandleDestroyed += ParentHandleDestroyed;
		((Control)m_ParentForm).HandleCreated += ParentHandleCreated;
		if (!base.DesignMode)
		{
			m_ParentForm.Activated += ParentActivated;
			m_ParentForm.Deactivate += ParentDeactivate;
			m_ParentForm.Load += ParentLoad;
			((Control)m_ParentForm).VisibleChanged += ParentVisibleChanged;
			((Control)m_ParentForm).Resize += ParentResize;
		}
		if (!base.DesignMode)
		{
			if (!m_FilterInstalled && !m_UseHook)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled = true;
			}
			else if (m_UseHook && m_Hook == null)
			{
				m_Hook = new Hook(this);
			}
		}
		if (m_MdiHandler != null)
		{
			m_MdiHandler.MdiSetMenu -= OnMdiSetMenu;
			((NativeWindow)m_MdiHandler).ReleaseHandle();
			m_MdiHandler = null;
		}
		MdiClient mdiClient = ((IOwner)this).GetMdiClient(m_ParentForm);
		if (mdiClient != null)
		{
			SetupMdiHandler(mdiClient);
		}
		else
		{
			((Control)m_ParentForm).ControlAdded += new ControlEventHandler(ParentControlAdded);
			m_EventControlAdded = true;
		}
		BarStreamLoad();
	}

	private void ParentHandleDestroyed(object sender, EventArgs e)
	{
		if (!base.DesignMode)
		{
			UnRegisterParentMsgHandler(this, m_ParentForm);
		}
	}

	private void ParentHandleCreated(object sender, EventArgs e)
	{
		if (!base.DesignMode)
		{
			if (m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null)
			{
				RegisterParentMsgHandler(this, m_ParentForm.MdiParent);
			}
			else
			{
				RegisterParentMsgHandler(this, m_ParentForm);
			}
			if (m_MdiHandler != null)
			{
				m_MdiHandler.MdiSetMenu -= OnMdiSetMenu;
				((NativeWindow)m_MdiHandler).ReleaseHandle();
				m_MdiHandler = null;
				MdiClient mdiClient = ((IOwner)this).GetMdiClient(m_ParentForm);
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
		else if (!m_MdiClientHandleCreatedHandled)
		{
			((Control)client).HandleCreated += MdiClientHandleCreates;
			m_MdiClientHandleCreatedHandled = true;
		}
		if (!m_MdiChildActivateHandled)
		{
			m_ParentForm.MdiChildActivate += OnMdiChildActivate;
			m_MdiChildActivateHandled = true;
			if (m_ParentForm.ActiveMdiChild != null)
			{
				OnMdiChildActivate(m_ParentForm, new EventArgs());
			}
		}
	}

	private void SetupClientMsgHandler(MdiClient client)
	{
		if (client != null)
		{
			if (m_MdiHandler != null)
			{
				m_MdiHandler.MdiSetMenu -= OnMdiSetMenu;
				((NativeWindow)m_MdiHandler).ReleaseHandle();
				m_MdiHandler = null;
			}
			m_MdiHandler = new MDIClientMsgHandler();
			m_MdiHandler.MdiSetMenu += OnMdiSetMenu;
			((NativeWindow)m_MdiHandler).AssignHandle(((Control)client).Handle);
			if (m_MdiClientHandleCreatedHandled)
			{
				((Control)client).HandleCreated -= MdiClientHandleCreates;
				m_MdiClientHandleCreatedHandled = false;
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
		if (m_ParentForm != null && (int)m_ParentForm.WindowState != 1 && m_ParentActivated && m_WereVisible.Count > 0)
		{
			((IOwner)this).OnApplicationActivate();
		}
		else if (m_ParentForm != null && (int)m_ParentForm.WindowState == 1 && m_WereVisible.Count == 0)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void ParentActivated(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		m_ParentActivated = true;
		if (m_WereVisible.Count > 0 && m_ParentForm != null && (int)m_ParentForm.WindowState != 1)
		{
			((IOwner)this).OnApplicationActivate();
		}
	}

	private void ParentDeactivate(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		m_ParentActivated = false;
		if (m_ParentForm != null && (int)m_ParentForm.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void ParentVisibleChanged(object sender, EventArgs e)
	{
		if (m_ParentForm == null)
		{
			return;
		}
		if (((Control)m_ParentForm).Visible)
		{
			if (!m_FilterInstalled && !m_UseHook)
			{
				m_FilterInstalled = true;
				MessageHandler.RegisterMessageClient(this);
			}
			else if (m_UseHook && m_Hook == null)
			{
				m_Hook = new Hook(this);
			}
		}
		else if (m_FilterInstalled)
		{
			MessageHandler.UnregisterMessageClient(this);
			m_FilterInstalled = false;
		}
		else if (m_Hook != null)
		{
			m_Hook.Dispose();
			m_Hook = null;
		}
	}

	private void ParentLoad(object sender, EventArgs e)
	{
		if (m_MdiHandler == null)
		{
			MdiClient mdiClient = ((IOwner)this).GetMdiClient(m_ParentForm);
			if (mdiClient != null)
			{
				SetupMdiHandler(mdiClient);
			}
		}
		if (((Control)m_ParentForm).IsHandleCreated && !base.DesignMode)
		{
			if (m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null)
			{
				if (!IsManagerRegistered(this, m_ParentForm.MdiParent))
				{
					UnRegisterParentMsgHandler(this, m_ParentForm);
					RegisterParentMsgHandler(this, m_ParentForm.MdiParent);
				}
			}
			else
			{
				RegisterParentMsgHandler(this, m_ParentForm);
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
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		if (m_DefinitionLoaded)
		{
			return;
		}
		if (m_DefinitionName != "" && ((m_TopDockSite != null && ((Control)m_TopDockSite).Parent != null && m_BottomDockSite != null && ((Control)m_BottomDockSite).Parent != null && m_LeftDockSite != null && ((Control)m_LeftDockSite).Parent != null && m_RightDockSite != null && ((Control)m_RightDockSite).Parent != null) || bForceLoad || (IsPopupProviderOnly && ParentForm != null && m_ParentUserControl != null)))
		{
			if (base.DesignMode)
			{
				string text = DesignTimeDte.GetDefinitionPath(m_DefinitionName, Site);
				if (text != null && text != "")
				{
					if (!text.EndsWith("\\"))
					{
						text += "\\";
					}
					if (File.Exists(text + m_DefinitionName))
					{
						LoadDefinition(text + m_DefinitionName);
					}
					else
					{
						MessageBox.Show("DotNetBar definition file not found: " + text + m_DefinitionName, "DotNetBar Designer", (MessageBoxButtons)0, (MessageBoxIcon)48);
						m_DefinitionName = "";
					}
				}
			}
			else if ((ParentForm != null && (((Control)ParentForm).IsHandleCreated || bForceLoad || IsPopupProviderOnly)) || (m_TopDockSite != null && ((Control)m_TopDockSite).Parent is UserControl))
			{
				Stream stream = null;
				Hashtable hashtable = new Hashtable();
				if (m_TopDockSite != null && ((Control)m_TopDockSite).Parent is UserControl)
				{
					hashtable.Add(((object)((Control)m_TopDockSite).Parent).GetType().Assembly.FullName, ((object)((Control)m_TopDockSite).Parent).GetType().Assembly);
					stream = ((object)((Control)m_TopDockSite).Parent).GetType().Assembly.GetManifestResourceStream(((object)((Control)m_TopDockSite).Parent).GetType().Namespace + "." + m_DefinitionName);
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
						stream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + m_DefinitionName);
						if (stream == null)
						{
							while (stack.Count > 0 && stream == null)
							{
								type = stack.Pop() as Type;
								stream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + m_DefinitionName);
							}
						}
						stack.Clear();
					}
					else
					{
						stream = ((object)ParentForm).GetType().Assembly.GetManifestResourceStream(((object)ParentForm).GetType().Namespace + "." + m_DefinitionName);
						hashtable.Add(((object)ParentForm).GetType().Assembly.FullName, ((object)ParentForm).GetType().Assembly);
					}
					if (m_ParentUserControl != null && !hashtable.Contains(((object)m_ParentUserControl).GetType().Assembly.FullName))
					{
						hashtable.Add(((object)m_ParentUserControl).GetType().Assembly.FullName, ((object)m_ParentUserControl).GetType().Assembly);
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
							if (text2.EndsWith(m_DefinitionName))
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
					m_DefinitionLoaded = true;
				}
			}
			m_BarStreamer = null;
		}
		else if (m_BarStreamer != null && ((m_TopDockSite != null && ((Control)m_TopDockSite).Parent != null && m_BottomDockSite != null && ((Control)m_BottomDockSite).Parent != null && m_LeftDockSite != null && ((Control)m_LeftDockSite).Parent != null && m_RightDockSite != null && ((Control)m_RightDockSite).Parent != null) || bForceLoad))
		{
			if (m_BarStreamer.Data != null)
			{
				LoadDefinition(m_BarStreamer.Data);
				m_DefinitionLoaded = true;
			}
			m_BarStreamer = null;
		}
	}

	void IOwner.OnApplicationDeactivate()
	{
		if (m_ParentForm != null && ((Control)m_ParentForm).InvokeRequired)
		{
			((Control)m_ParentForm).BeginInvoke((Delegate)new InvokeActivateDelegate(OnApplicationDeactivateInternal));
		}
		else
		{
			OnApplicationDeactivateInternal();
		}
	}

	private void OnApplicationDeactivateInternal()
	{
		CloseDockingHints();
		if (m_WereVisible.Count == 0)
		{
			if (m_Bars != null)
			{
				foreach (Bar bar3 in m_Bars)
				{
					if (bar3.DockingInProgress)
					{
						return;
					}
				}
				foreach (Bar bar4 in m_Bars)
				{
					bar4.AppLostFocus();
					if (bar4.BarState == eBarState.Floating && bar4.Visible && bar4.HideFloatingInactive)
					{
						bar4.HideBar();
						m_WereVisible.Add(bar4);
					}
				}
			}
			ArrayList arrayList = new ArrayList(m_RegisteredPopups);
			foreach (PopupItem item in arrayList)
			{
				item.ClosePopup();
			}
		}
		Bar menuBar = GetMenuBar();
		if (menuBar != null && menuBar.MenuFocus)
		{
			menuBar.MenuFocus = false;
		}
	}

	void IOwner.OnApplicationActivate()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (m_ParentForm != null && (int)m_ParentForm.WindowState != 1)
		{
			if (m_ParentForm != null && ((Control)m_ParentForm).InvokeRequired)
			{
				((Control)m_ParentForm).BeginInvoke((Delegate)new InvokeActivateDelegate(OnApplicationActivateInternal));
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
		if (m_ParentForm == null || (int)m_ParentForm.WindowState == 1)
		{
			return;
		}
		Form activeForm = Form.ActiveForm;
		if (activeForm == null || (activeForm.Modal && activeForm != m_ParentForm))
		{
			return;
		}
		try
		{
			foreach (Bar item in m_WereVisible)
			{
				item.ShowBar();
			}
		}
		finally
		{
			m_WereVisible.Clear();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (m_Disposed)
		{
			base.Dispose(disposing);
			return;
		}
		m_Disposed = true;
		if (disposing && m_License != null)
		{
			m_License.Dispose();
			m_License = null;
		}
		if (m_ActiveMdiChild != null)
		{
			((Control)m_ActiveMdiChild).Resize -= OnMdiChildResize;
			((Control)m_ActiveMdiChild).VisibleChanged -= OnMdiChildVisibleChanged;
			m_ActiveMdiChild = null;
		}
		Images = null;
		ImagesMedium = null;
		ImagesLarge = null;
		if (m_ClickTimer != null)
		{
			m_ClickTimer.Stop();
			((Component)(object)m_ClickTimer).Dispose();
			m_ClickTimer = null;
		}
		if (m_RegisteredPopups != null && m_RegisteredPopups.Count > 0)
		{
			BaseItem[] array;
			lock (this)
			{
				array = (BaseItem[])m_RegisteredPopups.ToArray(typeof(BaseItem));
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
		if (m_FilterInstalled)
		{
			MessageHandler.UnregisterMessageClient(this);
			m_FilterInstalled = false;
		}
		if (m_Hook != null)
		{
			m_Hook.Dispose();
			m_Hook = null;
		}
		if (m_frmCustomize != null)
		{
			((Form)m_frmCustomize).Close();
			if (m_frmCustomize != null)
			{
				((Component)(object)m_frmCustomize).Dispose();
			}
		}
		if (m_Bars != null)
		{
			m_Bars = null;
		}
		if (m_Items != null)
		{
			m_Items.Dispose();
			m_Items = null;
		}
		if (m_ContextMenus != null)
		{
			m_ContextMenus.SetOwner(null);
			m_ContextMenus = null;
		}
		if (m_DisposeGCCollect)
		{
			GC.Collect();
		}
		base.Dispose(disposing);
	}

	private void ReleaseParentFormHooks()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		if (m_ParentForm != null)
		{
			if (!base.DesignMode)
			{
				UnRegisterParentMsgHandler(this, m_ParentForm);
			}
			((Control)m_ParentForm).HandleDestroyed -= ParentHandleDestroyed;
			((Control)m_ParentForm).HandleCreated -= ParentHandleCreated;
			if (m_EventControlAdded)
			{
				((Control)m_ParentForm).ControlAdded -= new ControlEventHandler(ParentControlAdded);
				m_EventControlAdded = false;
			}
			if (!base.DesignMode)
			{
				m_ParentForm.Activated -= ParentActivated;
				m_ParentForm.Deactivate -= ParentDeactivate;
				m_ParentForm.Load -= ParentLoad;
				((Control)m_ParentForm).VisibleChanged -= ParentVisibleChanged;
				((Control)m_ParentForm).Resize -= ParentResize;
			}
			if (m_MdiChildActivateHandled)
			{
				m_ParentForm.MdiChildActivate -= OnMdiChildActivate;
				m_MdiChildActivateHandled = false;
			}
		}
	}

	private void ImageListDisposed(object sender, EventArgs e)
	{
		if (sender == m_ImageList)
		{
			Images = null;
		}
		else if (sender == m_ImageListLarge)
		{
			ImagesLarge = null;
		}
		else if (sender == m_ImageListMedium)
		{
			ImagesMedium = null;
		}
	}

	public bool ShouldSerializeMinimumClientSize()
	{
		if (m_MinimumClientSize.Width == 48)
		{
			return m_MinimumClientSize.Height != 48;
		}
		return true;
	}

	public void RemoveBar(Bar bar)
	{
		if (!m_Bars.Contains(bar))
		{
			throw new ArgumentException("Bar not part of Bars collections.");
		}
		m_Bars.Remove(bar);
		if (((Control)bar).Parent != null && ((Control)bar).Parent is DockSite)
		{
			if (((DockSite)(object)((Control)bar).Parent).IsDocumentDock || ((DockSite)(object)((Control)bar).Parent).DocumentDockContainer != null)
			{
				((DockSite)(object)((Control)bar).Parent).GetDocumentUIManager().UnDock(bar);
			}
			else
			{
				((DockSite)(object)((Control)bar).Parent).RemoveBar((Control)(object)bar);
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
		ShortcutTableEntry shortcutTableEntry = null;
		if (objItem.ShortcutString != "")
		{
			foreach (eShortcut shortcut in objItem.Shortcuts)
			{
				if (m_ShortcutTable.ContainsKey(shortcut))
				{
					shortcutTableEntry = (ShortcutTableEntry)m_ShortcutTable[objItem.Shortcuts[0]];
				}
				else
				{
					shortcutTableEntry = new ShortcutTableEntry(shortcut);
					m_ShortcutTable.Add(shortcutTableEntry.Shortcut, shortcutTableEntry);
				}
				try
				{
					if (!shortcutTableEntry.Items.ContainsKey(objItem.Id))
					{
						shortcutTableEntry.Items.Add(objItem.Id, objItem);
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
		ShortcutTableEntry shortcutTableEntry = null;
		if (objItem.ShortcutString != "")
		{
			foreach (eShortcut shortcut in objItem.Shortcuts)
			{
				if (!m_ShortcutTable.ContainsKey(shortcut))
				{
					continue;
				}
				shortcutTableEntry = (ShortcutTableEntry)m_ShortcutTable[shortcut];
				try
				{
					shortcutTableEntry.Items.Remove(objItem.Id);
					if (shortcutTableEntry.Items.Count == 0)
					{
						m_ShortcutTable.Remove(shortcutTableEntry.Shortcut);
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
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBars()
	{
		if (m_DefinitionCodeSerialize)
		{
			return true;
		}
		return false;
	}

	public void ForceDefinitionLoad()
	{
		if (!m_DefinitionLoaded)
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
			if (pDock.CanDockBottom || pDock.CanDockLeft || pDock.CanDockRight || pDock.CanDockTop || pDock.CanDockDocument)
			{
				DockingHintHandler(ref dockInfo, pDock, x, y);
				return dockInfo;
			}
			return dockInfo;
		}
		if (pDock.CanDockTop && m_ToolbarTopDockSite != null)
		{
			dockInfo = m_ToolbarTopDockSite.GetDockSiteInfo(pDock, x, y);
			if (dockInfo.objDockSite != null)
			{
				return dockInfo;
			}
		}
		if (pDock.CanDockBottom && m_ToolbarBottomDockSite != null)
		{
			dockInfo = m_ToolbarBottomDockSite.GetDockSiteInfo(pDock, x, y);
			if (dockInfo.objDockSite != null)
			{
				return dockInfo;
			}
		}
		if (pDock.CanDockLeft && m_ToolbarLeftDockSite != null)
		{
			dockInfo = m_ToolbarLeftDockSite.GetDockSiteInfo(pDock, x, y);
			if (dockInfo.objDockSite != null)
			{
				return dockInfo;
			}
		}
		if (pDock.CanDockRight && m_ToolbarRightDockSite != null)
		{
			dockInfo = m_ToolbarRightDockSite.GetDockSiteInfo(pDock, x, y);
			_ = dockInfo.objDockSite;
			return dockInfo;
		}
		return dockInfo;
	}

	void IOwnerBarSupport.DockComplete()
	{
		CloseDockingHints();
		m_DockingHintSetup = false;
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
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		if (!m_DockingHintSetup)
		{
			m_DockingHintSetup = true;
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
		eMouseOverHintSide eMouseOverHintSide2 = eMouseOverHintSide.None;
		if (m_DockingHintLeft != null)
		{
			eMouseOverHintSide2 = m_DockingHintLeft.ExMouseMove(x, y);
		}
		if (m_DockingHintRight != null)
		{
			eMouseOverHintSide eMouseOverHintSide3 = m_DockingHintRight.ExMouseMove(x, y);
			if (eMouseOverHintSide2 == eMouseOverHintSide.None)
			{
				eMouseOverHintSide2 = eMouseOverHintSide3;
			}
		}
		if (m_DockingHintTop != null)
		{
			eMouseOverHintSide eMouseOverHintSide4 = m_DockingHintTop.ExMouseMove(x, y);
			if (eMouseOverHintSide2 == eMouseOverHintSide.None)
			{
				eMouseOverHintSide2 = eMouseOverHintSide4;
			}
		}
		if (m_DockingHintBottom != null)
		{
			eMouseOverHintSide eMouseOverHintSide5 = m_DockingHintBottom.ExMouseMove(x, y);
			if (eMouseOverHintSide2 == eMouseOverHintSide.None)
			{
				eMouseOverHintSide2 = eMouseOverHintSide5;
			}
		}
		if (m_DockingHintMiddle != null)
		{
			eMouseOverHintSide eMouseOverHintSide6 = m_DockingHintMiddle.ExMouseMove(x, y);
			if (eMouseOverHintSide2 == eMouseOverHintSide.None)
			{
				eMouseOverHintSide2 = eMouseOverHintSide6;
				if (eMouseOverHintSide6 != 0)
				{
					flag = true;
				}
				if (flag && IsDocumentDockingEnabled && barDockInfo.CanDockDocument && bar == null && barDockInfo.CanDockDocument)
				{
					if (!barDockInfo.CanDockTop && eMouseOverHintSide6 == eMouseOverHintSide.Top)
					{
						eMouseOverHintSide2 = eMouseOverHintSide.DockTab;
					}
					else if (!barDockInfo.CanDockBottom && eMouseOverHintSide6 == eMouseOverHintSide.Bottom)
					{
						eMouseOverHintSide2 = eMouseOverHintSide.DockTab;
					}
					else if (!barDockInfo.CanDockLeft && eMouseOverHintSide6 == eMouseOverHintSide.Left)
					{
						eMouseOverHintSide2 = eMouseOverHintSide.DockTab;
					}
					else if (!barDockInfo.CanDockRight && eMouseOverHintSide6 == eMouseOverHintSide.Right)
					{
						eMouseOverHintSide2 = eMouseOverHintSide.DockTab;
					}
				}
			}
		}
		dockInfo.MouseOverBar = bar;
		if (bar == null || !flag)
		{
			if (m_EnableFullSizeDock)
			{
				dockInfo.FullSizeDock = !flag;
				dockInfo.PartialSizeDock = flag;
			}
			switch (eMouseOverHintSide2)
			{
			case eMouseOverHintSide.Left:
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
					dockInfo.InsertPosition = ((Control)m_LeftDockSite).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = m_LeftDockSite;
				break;
			case eMouseOverHintSide.Right:
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
					dockInfo.InsertPosition = ((Control)m_RightDockSite).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = m_RightDockSite;
				break;
			case eMouseOverHintSide.Top:
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
					dockInfo.InsertPosition = ((Control)m_TopDockSite).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = m_TopDockSite;
				break;
			case eMouseOverHintSide.Bottom:
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
					dockInfo.InsertPosition = ((Control)m_BottomDockSite).Controls.IndexOf((Control)((barDockInfo is Control) ? barDockInfo : null));
				}
				dockInfo.objDockSite = m_BottomDockSite;
				break;
			case eMouseOverHintSide.DockTab:
				if (IsDocumentDockingEnabled)
				{
					dockInfo.objDockSite = m_FillDockSite;
					dockInfo.DockSide = (DockStyle)5;
					dockInfo.MouseOverDockSide = eDockSide.Document;
				}
				break;
			}
		}
		else
		{
			switch (eMouseOverHintSide2)
			{
			case eMouseOverHintSide.DockTab:
				dockInfo.MouseOverDockSide = eDockSide.Document;
				if (bar != null)
				{
					dockInfo.TabDockContainer = bar;
					dockInfo.DockSide = ((Control)bar).Parent.Dock;
					dockInfo.objDockSite = ((Control)bar).Parent as DockSite;
				}
				break;
			case eMouseOverHintSide.Right:
			{
				dockInfo.MouseOverDockSide = eDockSide.Right;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				eDockSide dockSide4 = bar.DockSide;
				if ((uint)(dockSide4 - 3) <= 1u)
				{
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar) + 1;
					dockInfo.DockOffset = bar.DockOffset + 1;
				}
				else
				{
					dockInfo.DockLine = bar.DockLine + 1;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.InsertPosition++;
					dockInfo.NewLine = true;
				}
				break;
			}
			case eMouseOverHintSide.Left:
			{
				dockInfo.MouseOverDockSide = eDockSide.Left;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				eDockSide dockSide3 = bar.DockSide;
				if ((uint)(dockSide3 - 3) <= 1u)
				{
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.DockOffset = bar.DockOffset - 1;
				}
				else
				{
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.NewLine = true;
				}
				break;
			}
			case eMouseOverHintSide.Top:
			{
				dockInfo.MouseOverDockSide = eDockSide.Top;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				eDockSide dockSide2 = bar.DockSide;
				if ((uint)(dockSide2 - 3) <= 1u)
				{
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.NewLine = true;
				}
				else
				{
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					dockInfo.DockOffset = bar.DockOffset - 1;
				}
				break;
			}
			case eMouseOverHintSide.Bottom:
			{
				dockInfo.MouseOverDockSide = eDockSide.Bottom;
				dockInfo.DockSide = ((Control)bar).Parent.Dock;
				dockInfo.objDockSite = bar.DockedSite as DockSite;
				eDockSide dockSide = bar.DockSide;
				if ((uint)(dockSide - 3) <= 1u)
				{
					dockInfo.DockLine = bar.DockLine + 1;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar);
					if (bar.DockSide == eDockSide.Bottom)
					{
						dockInfo.InsertPosition++;
					}
					dockInfo.NewLine = true;
				}
				else
				{
					dockInfo.DockLine = 0;
					dockInfo.InsertPosition = ((Control)bar).Parent.Controls.IndexOf((Control)(object)bar) + 1;
					dockInfo.DockOffset = bar.DockOffset + 1;
				}
				break;
			}
			}
		}
		dockInfo.UseOutline = true;
		return true;
	}

	private void CloseDockingHints()
	{
		if (m_ClosingHints)
		{
			return;
		}
		try
		{
			m_ClosingHints = true;
			if (m_DockingHintLeft != null)
			{
				NativeFunctions.SetWindowPos(((Control)m_DockingHintLeft).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)m_DockingHintLeft).Close();
				((Component)(object)m_DockingHintLeft).Dispose();
				m_DockingHintLeft = null;
			}
			if (m_DockingHintRight != null)
			{
				NativeFunctions.SetWindowPos(((Control)m_DockingHintRight).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)m_DockingHintRight).Close();
				((Component)(object)m_DockingHintRight).Dispose();
				m_DockingHintRight = null;
			}
			if (m_DockingHintTop != null)
			{
				NativeFunctions.SetWindowPos(((Control)m_DockingHintTop).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)m_DockingHintTop).Close();
				((Component)(object)m_DockingHintTop).Dispose();
				m_DockingHintTop = null;
			}
			if (m_DockingHintBottom != null)
			{
				NativeFunctions.SetWindowPos(((Control)m_DockingHintBottom).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)m_DockingHintBottom).Close();
				((Component)(object)m_DockingHintBottom).Dispose();
				m_DockingHintBottom = null;
			}
			if (m_DockingHintMiddle != null)
			{
				NativeFunctions.SetWindowPos(((Control)m_DockingHintMiddle).Handle, new IntPtr(0), 0, 0, 0, 0, 147);
				((Form)m_DockingHintMiddle).Close();
				((Component)(object)m_DockingHintMiddle).Dispose();
				m_DockingHintMiddle = null;
			}
		}
		catch
		{
		}
		finally
		{
			m_ClosingHints = false;
		}
	}

	private eDotNetBarStyle GetEffectiveStyle()
	{
		if (m_Style == eDotNetBarStyle.StyleManagerControlled)
		{
			return StyleManager.GetEffectiveStyle();
		}
		return m_Style;
	}

	private void SetupDockingHintWindows(Bar barMouseOver, IDockInfo barDockInfo)
	{
		Rectangle empty = Rectangle.Empty;
		if (((Control)m_TopDockSite).Parent.Parent != null)
		{
			empty = new Rectangle(((Control)m_TopDockSite).Parent.PointToScreen(Point.Empty), ((Control)m_TopDockSite).Parent.Size);
			empty.Y += ((Control)m_TopDockSite).Height;
			empty.Height -= ((Control)m_TopDockSite).Height;
		}
		else
		{
			empty = ((Control)m_TopDockSite).Parent.Bounds;
			empty.Y += ((Control)m_TopDockSite).Height;
			empty.Height -= ((Control)m_TopDockSite).Height;
		}
		if (!base.DesignMode)
		{
			if (barDockInfo.CanDockLeft)
			{
				if (m_DockingHintLeft == null)
				{
					m_DockingHintLeft = new DockingHint(eDockingHintSide.Left, GetEffectiveStyle());
					((Form)m_DockingHintLeft).Location = new Point(empty.X + 32, empty.Y + (empty.Height - ((Control)m_DockingHintLeft).Height) / 2);
					m_DockingHintLeft.ShowFocusless();
				}
			}
			else if (m_DockingHintLeft != null)
			{
				((Form)m_DockingHintLeft).Close();
				((Component)(object)m_DockingHintLeft).Dispose();
				m_DockingHintLeft = null;
			}
			if (barDockInfo.CanDockRight)
			{
				if (m_DockingHintRight == null)
				{
					m_DockingHintRight = new DockingHint(eDockingHintSide.Right, GetEffectiveStyle());
					((Form)m_DockingHintRight).Location = new Point(empty.Right - 32 - ((Control)m_DockingHintRight).Width, empty.Y + (empty.Height - ((Control)m_DockingHintRight).Height) / 2);
					m_DockingHintRight.ShowFocusless();
				}
			}
			else if (m_DockingHintRight != null)
			{
				((Form)m_DockingHintRight).Close();
				((Component)(object)m_DockingHintRight).Dispose();
				m_DockingHintRight = null;
			}
			if (barDockInfo.CanDockTop)
			{
				if (m_DockingHintTop == null)
				{
					m_DockingHintTop = new DockingHint(eDockingHintSide.Top, GetEffectiveStyle());
					((Form)m_DockingHintTop).Location = new Point(empty.X + (empty.Width - ((Control)m_DockingHintTop).Width) / 2, empty.Y + 32);
					m_DockingHintTop.ShowFocusless();
				}
			}
			else if (m_DockingHintTop != null)
			{
				((Form)m_DockingHintTop).Close();
				((Component)(object)m_DockingHintTop).Dispose();
				m_DockingHintTop = null;
			}
			if (barDockInfo.CanDockBottom)
			{
				if (m_DockingHintBottom == null)
				{
					m_DockingHintBottom = new DockingHint(eDockingHintSide.Bottom, GetEffectiveStyle());
					((Form)m_DockingHintBottom).Location = new Point(empty.X + (empty.Width - ((Control)m_DockingHintBottom).Width) / 2, empty.Bottom - 32 - ((Control)m_DockingHintBottom).Height);
					m_DockingHintBottom.ShowFocusless();
				}
			}
			else if (m_DockingHintBottom != null)
			{
				((Form)m_DockingHintBottom).Close();
				((Component)(object)m_DockingHintBottom).Dispose();
				m_DockingHintBottom = null;
			}
		}
		bool flag = false;
		eDockingHintSide eDockingHintSide2 = eDockingHintSide.All;
		if (barMouseOver == null)
		{
			eDockingHintSide2 = eDockingHintSide.Top;
			if (barDockInfo.CanDockBottom)
			{
				eDockingHintSide2 = eDockingHintSide.Bottom;
			}
			if (barDockInfo.CanDockLeft)
			{
				eDockingHintSide2 |= eDockingHintSide.Left;
			}
			if (barDockInfo.CanDockRight)
			{
				eDockingHintSide2 |= eDockingHintSide.Right;
			}
			eDockingHintSide2 = ((!barDockInfo.CanDockTop) ? (eDockingHintSide2 & ~(eDockingHintSide2 & eDockingHintSide.Top)) : (eDockingHintSide2 | eDockingHintSide.Top));
			if (IsDocumentDockingEnabled && barDockInfo.CanDockDocument)
			{
				eDockingHintSide2 = eDockingHintSide.All;
			}
		}
		else if (!barDockInfo.CanDockTab)
		{
			eDockingHintSide2 &= ~(eDockingHintSide2 & eDockingHintSide.DockTab);
		}
		if (m_DockingHintMiddle == null)
		{
			if (barMouseOver != null)
			{
				m_DockingHintMiddle = new DockingHint(eDockingHintSide2, middleDockingHint: true, GetEffectiveStyle());
			}
			else
			{
				m_DockingHintMiddle = new DockingHint(eDockingHintSide2, middleDockingHint: true, GetEffectiveStyle());
			}
			flag = true;
		}
		else if (barMouseOver != null)
		{
			m_DockingHintMiddle.DockingHintSides = eDockingHintSide2;
		}
		else
		{
			m_DockingHintMiddle.DockingHintSides = eDockingHintSide2;
		}
		Point point = new Point(empty.X + (empty.Width - ((Control)m_DockingHintMiddle).Width) / 2, empty.Y + (empty.Height - ((Control)m_DockingHintMiddle).Height) / 2);
		if (barMouseOver != null)
		{
			Point point2 = ((Control)barMouseOver).PointToScreen(Point.Empty);
			point = new Point(point2.X + (((Control)barMouseOver).Width - ((Control)m_DockingHintMiddle).Width) / 2, point2.Y + (((Control)barMouseOver).Height - ((Control)m_DockingHintMiddle).Height) / 2);
		}
		Rectangle rectangle = new Rectangle(point, ((Form)m_DockingHintMiddle).Size);
		if (m_DockingHintLeft != null && rectangle.IntersectsWith(((Control)m_DockingHintLeft).Bounds))
		{
			if (Control.MousePosition.Y < ((Control)m_DockingHintLeft).Top)
			{
				point.Y = ((Control)m_DockingHintLeft).Top - ((Control)m_DockingHintMiddle).Height;
			}
			else
			{
				point.Y = ((Control)m_DockingHintLeft).Bottom;
			}
		}
		else if (m_DockingHintRight != null && rectangle.IntersectsWith(((Control)m_DockingHintRight).Bounds))
		{
			if (Control.MousePosition.Y < ((Control)m_DockingHintRight).Top)
			{
				point.Y = ((Control)m_DockingHintRight).Top - ((Control)m_DockingHintMiddle).Height;
			}
			else
			{
				point.Y = ((Control)m_DockingHintRight).Bottom;
			}
		}
		else if (m_DockingHintTop != null && rectangle.IntersectsWith(((Control)m_DockingHintTop).Bounds))
		{
			if (Control.MousePosition.X > ((Control)m_DockingHintTop).Left)
			{
				point.X = ((Control)m_DockingHintTop).Right;
			}
			else
			{
				point.X = ((Control)m_DockingHintTop).Left - ((Control)m_DockingHintMiddle).Width;
			}
		}
		else if (m_DockingHintBottom != null && rectangle.IntersectsWith(((Control)m_DockingHintBottom).Bounds))
		{
			if (Control.MousePosition.X > ((Control)m_DockingHintBottom).Left)
			{
				point.X = ((Control)m_DockingHintBottom).Right;
			}
			else
			{
				point.X = ((Control)m_DockingHintBottom).Left - ((Control)m_DockingHintMiddle).Width;
			}
		}
		if (((Form)m_DockingHintMiddle).Location != point)
		{
			((Form)m_DockingHintMiddle).Location = point;
		}
		if (flag)
		{
			m_DockingHintMiddle.ShowFocusless();
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
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Invalid comparison between Unknown and I4
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Invalid comparison between Unknown and I4
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Invalid comparison between Unknown and I4
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Invalid comparison between Unknown and I4
		XmlElement xmlElement = xmlDoc.FirstChild as XmlElement;
		if (xmlElement != null && (!xmlElement.HasAttribute(DocumentSerializationXml.Version) || XmlConvert.ToInt32(xmlElement.GetAttribute(DocumentSerializationXml.Version)) < 6))
		{
			return;
		}
		bool flag = false;
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		itemSerializationContext.DockControls = GetDockControls();
		try
		{
			m_LoadingDefinition = true;
			if (!SuspendLayout)
			{
				flag = true;
				SuspendLayout = true;
			}
			try
			{
				IgnoreLoadedControlDispose = true;
				m_Items.Clear();
				if (m_IncludeDockDocumentsInDefinition)
				{
					m_Bars.Clear();
				}
				else
				{
					m_Bars.ClearNonDocumentBars();
				}
				m_ContextMenus.Clear();
				m_ShortcutTable.Clear();
			}
			finally
			{
				IgnoreLoadedControlDispose = false;
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
				m_AlwaysShowFullMenus = XmlConvert.ToBoolean(xmlElement.GetAttribute("fullmenus"));
			}
			else
			{
				m_AlwaysShowFullMenus = false;
			}
			if (xmlElement.HasAttribute("fullmenushover"))
			{
				m_ShowFullMenusOnHover = XmlConvert.ToBoolean(xmlElement.GetAttribute("fullmenushover"));
			}
			else
			{
				m_ShowFullMenusOnHover = true;
			}
			if (xmlElement.HasAttribute("tooltips"))
			{
				m_ShowToolTips = XmlConvert.ToBoolean(xmlElement.GetAttribute("tooltips"));
			}
			else
			{
				m_ShowToolTips = true;
			}
			if (xmlElement.HasAttribute("scintooltip"))
			{
				m_ShowShortcutKeysInToolTips = XmlConvert.ToBoolean(xmlElement.GetAttribute("scintooltip"));
			}
			else
			{
				m_ShowShortcutKeysInToolTips = false;
			}
			if (xmlElement.HasAttribute("animation"))
			{
				if (XmlConvert.ToInt32(xmlElement.GetAttribute("animation")) != 6)
				{
					m_PopupAnimation = (ePopupAnimation)XmlConvert.ToInt32(xmlElement.GetAttribute("animation"));
				}
			}
			else
			{
				m_PopupAnimation = ePopupAnimation.SystemDefault;
			}
			foreach (XmlElement childNode in xmlElement.ChildNodes)
			{
				if (childNode.Name == "items")
				{
					foreach (XmlElement childNode2 in childNode.ChildNodes)
					{
						BaseItem baseItem = BarFunctions.CreateItemFromXml(childNode2);
						if (baseItem == null)
						{
							throw new InvalidOperationException("Invalid Item in file found (" + BarFunctions.GetItemErrorInfo(childNode2) + ").");
						}
						itemSerializationContext.ItemXmlElement = childNode2;
						baseItem.Deserialize(itemSerializationContext);
						m_Items.Add(baseItem);
					}
				}
				else if (childNode.Name == "bars")
				{
					foreach (XmlElement childNode3 in childNode.ChildNodes)
					{
						Bar bar = new Bar();
						bar.Visible = false;
						bar.SetDesignMode(base.DesignMode);
						m_Bars.Add(bar);
						bar.Deserialize(childNode3);
						((IOwnerBarSupport)this).AddShortcutsFromBar(bar);
					}
				}
				else if (childNode.Name == "popups")
				{
					foreach (XmlElement childNode4 in childNode.ChildNodes)
					{
						BaseItem baseItem2 = BarFunctions.CreateItemFromXml(childNode4);
						if (baseItem2 == null)
						{
							throw new InvalidOperationException("Invalid Item in file found (" + BarFunctions.GetItemErrorInfo(childNode4) + ").");
						}
						itemSerializationContext.ItemXmlElement = childNode4;
						baseItem2.Deserialize(itemSerializationContext);
						m_ContextMenus.Add(baseItem2);
					}
				}
				else if (childNode.Name == DocumentSerializationXml.Documents && m_IncludeDockDocumentsInDefinition)
				{
					itemSerializationContext.ItemXmlElement = childNode;
					if (m_FillDockSite != null)
					{
						m_FillDockSite.GetDocumentUIManager().DeserializeDefinition(itemSerializationContext);
					}
				}
				else
				{
					if (!(childNode.Name == DocumentSerializationXml.DockSite))
					{
						continue;
					}
					itemSerializationContext.ItemXmlElement = childNode;
					DockStyle val = (DockStyle)Enum.Parse(typeof(DockStyle), childNode.GetAttribute(DocumentSerializationXml.DockingSide));
					if ((int)val == 3)
					{
						if (m_LeftDockSite != null)
						{
							m_LeftDockSite.GetDocumentUIManager().DeserializeDefinition(itemSerializationContext);
						}
					}
					else if ((int)val == 4)
					{
						if (m_RightDockSite != null)
						{
							m_RightDockSite.GetDocumentUIManager().DeserializeDefinition(itemSerializationContext);
						}
					}
					else if ((int)val == 1)
					{
						if (m_TopDockSite != null)
						{
							m_TopDockSite.GetDocumentUIManager().DeserializeDefinition(itemSerializationContext);
						}
					}
					else if ((int)val == 2 && m_BottomDockSite != null)
					{
						m_BottomDockSite.GetDocumentUIManager().DeserializeDefinition(itemSerializationContext);
					}
				}
			}
		}
		finally
		{
			m_LoadingDefinition = false;
			if (flag)
			{
				SuspendLayout = false;
			}
		}
		if (itemSerializationContext.DockControls != null && itemSerializationContext.DockControls.Count > 0)
		{
			foreach (Control value in itemSerializationContext.DockControls.Values)
			{
				Control val2 = value;
				((Component)(object)val2).Dispose();
			}
		}
		((IOwner)this).InvokeDefinitionLoaded((object)this, new EventArgs());
		if (m_ActiveMdiChild != null && m_MdiChildMaximized)
		{
			m_MdiChildMaximized = false;
			OnMdiChildResize(m_ActiveMdiChild, new EventArgs());
		}
	}

	void ICustomSerialization.InvokeSerializeItem(SerializeItemEventArgs e)
	{
		if (this.SerializeItem != null)
		{
			this.SerializeItem(this, e);
		}
	}

	void ICustomSerialization.InvokeDeserializeItem(SerializeItemEventArgs e)
	{
		if (this.DeserializeItem != null)
		{
			this.DeserializeItem(this, e);
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
		xmlElement.SetAttribute(DocumentSerializationXml.Version, "6");
		xmlDoc.AppendChild(xmlElement);
		xmlElement.SetAttribute("fullmenus", XmlConvert.ToString(m_AlwaysShowFullMenus));
		xmlElement.SetAttribute("fullmenushover", XmlConvert.ToString(m_ShowFullMenusOnHover));
		xmlElement.SetAttribute("tooltips", XmlConvert.ToString(m_ShowToolTips));
		xmlElement.SetAttribute("scintooltip", XmlConvert.ToString(m_ShowShortcutKeysInToolTips));
		xmlElement.SetAttribute("animation", XmlConvert.ToString((int)m_PopupAnimation));
		XmlElement xmlElement2 = xmlDoc.CreateElement("items");
		xmlElement.AppendChild(xmlElement2);
		foreach (DictionaryEntry item in m_Items)
		{
			BaseItem baseItem = item.Value as BaseItem;
			XmlElement xmlElement3 = xmlDoc.CreateElement("item");
			xmlElement2.AppendChild(xmlElement3);
			itemSerializationContext.ItemXmlElement = xmlElement3;
			baseItem.Serialize(itemSerializationContext);
		}
		XmlElement xmlElement4 = xmlDoc.CreateElement("bars");
		xmlElement.AppendChild(xmlElement4);
		if (m_TopDockSite != null)
		{
			SerializeDockSite(m_TopDockSite, xmlElement);
		}
		if (m_BottomDockSite != null)
		{
			SerializeDockSite(m_BottomDockSite, xmlElement);
		}
		if (m_LeftDockSite != null)
		{
			SerializeDockSite(m_LeftDockSite, xmlElement);
		}
		if (m_RightDockSite != null)
		{
			SerializeDockSite(m_RightDockSite, xmlElement);
		}
		if (m_IncludeDockDocumentsInDefinition && m_FillDockSite != null)
		{
			SerializeDockSite(m_FillDockSite, xmlElement);
		}
		if (m_ToolbarTopDockSite != null)
		{
			SerializeDockSite(m_ToolbarTopDockSite, xmlElement4);
		}
		if (m_ToolbarBottomDockSite != null)
		{
			SerializeDockSite(m_ToolbarBottomDockSite, xmlElement4);
		}
		if (m_ToolbarLeftDockSite != null)
		{
			SerializeDockSite(m_ToolbarLeftDockSite, xmlElement4);
		}
		if (m_ToolbarRightDockSite != null)
		{
			SerializeDockSite(m_ToolbarRightDockSite, xmlElement4);
		}
		foreach (Bar bar in m_Bars)
		{
			if (bar.DockSide == eDockSide.None)
			{
				XmlElement xmlElement5 = xmlDoc.CreateElement("bar");
				xmlElement4.AppendChild(xmlElement5);
				bar.Serialize(xmlElement5);
			}
		}
		XmlElement xmlElement6 = xmlDoc.CreateElement("popups");
		xmlElement.AppendChild(xmlElement6);
		foreach (BaseItem contextMenu in m_ContextMenus)
		{
			XmlElement xmlElement7 = xmlDoc.CreateElement("item");
			xmlElement6.AppendChild(xmlElement7);
			itemSerializationContext.ItemXmlElement = xmlElement7;
			contextMenu.Serialize(itemSerializationContext);
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
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Invalid comparison between Unknown and I4
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Invalid comparison between Unknown and I4
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Invalid comparison between Unknown and I4
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Invalid comparison between Unknown and I4
		if (!(xmlDoc.FirstChild is XmlElement xmlElement) || !xmlElement.HasAttribute(DocumentSerializationXml.Version) || XmlConvert.ToInt32(xmlElement.GetAttribute(DocumentSerializationXml.Version)) < 6)
		{
			return;
		}
		DestroyAutoHidePanels();
		bool flag = false;
		m_LoadingLayout = true;
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
				if (childNode.Name == "bars")
				{
					foreach (XmlElement childNode2 in childNode.ChildNodes)
					{
						if (!(childNode2.Name != "bar"))
						{
							Bar bar = null;
							if (m_Bars.Contains(m_Bars[childNode2.GetAttribute("name")]))
							{
								bar = m_Bars[childNode2.GetAttribute("name")];
							}
							else
							{
								bar = new Bar();
								Bars.Add(bar);
							}
							bar.DeserializeLayout(childNode2);
						}
					}
				}
				else if (childNode.Name == DocumentSerializationXml.Documents)
				{
					if (m_FillDockSite != null)
					{
						m_FillDockSite.GetDocumentUIManager().DeserializeLayout(childNode);
					}
				}
				else
				{
					if (!(childNode.Name == DocumentSerializationXml.DockSite))
					{
						continue;
					}
					DockStyle val = (DockStyle)Enum.Parse(typeof(DockStyle), childNode.GetAttribute(DocumentSerializationXml.DockingSide));
					if ((int)val == 3)
					{
						if (m_LeftDockSite != null)
						{
							m_LeftDockSite.GetDocumentUIManager().DeserializeLayout(childNode);
						}
					}
					else if ((int)val == 4)
					{
						if (m_RightDockSite != null)
						{
							m_RightDockSite.GetDocumentUIManager().DeserializeLayout(childNode);
						}
					}
					else if ((int)val == 1)
					{
						if (m_TopDockSite != null)
						{
							m_TopDockSite.GetDocumentUIManager().DeserializeLayout(childNode);
						}
					}
					else if ((int)val == 2 && m_BottomDockSite != null)
					{
						m_BottomDockSite.GetDocumentUIManager().DeserializeLayout(childNode);
					}
				}
			}
			if (xmlElement.HasAttribute("zorder"))
			{
				RestoreDockSiteZOrder(xmlElement.GetAttribute("zorder"));
			}
			Bar[] array = new Bar[m_Bars.Count];
			m_Bars.CopyTo(array);
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
			m_LoadingLayout = false;
		}
		if (m_ActiveMdiChild != null && m_MdiChildMaximized)
		{
			m_MdiChildMaximized = false;
			OnMdiChildResize(m_ActiveMdiChild, new EventArgs());
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
		xmlElement.SetAttribute(DocumentSerializationXml.Version, "6");
		xmlDoc.AppendChild(xmlElement);
		xmlElement.SetAttribute("zorder", GetDockSitesZOrder());
		if (m_TopDockSite != null)
		{
			SerializeDockSiteLayout(m_TopDockSite, xmlElement);
		}
		if (m_BottomDockSite != null)
		{
			SerializeDockSiteLayout(m_BottomDockSite, xmlElement);
		}
		if (m_LeftDockSite != null)
		{
			SerializeDockSiteLayout(m_LeftDockSite, xmlElement);
		}
		if (m_RightDockSite != null)
		{
			SerializeDockSiteLayout(m_RightDockSite, xmlElement);
		}
		if (m_FillDockSite != null)
		{
			SerializeDockSiteLayout(m_FillDockSite, xmlElement);
		}
		XmlElement xmlElement2 = xmlDoc.CreateElement("bars");
		xmlElement.AppendChild(xmlElement2);
		if (m_ToolbarTopDockSite != null)
		{
			SerializeDockSiteLayout(m_ToolbarTopDockSite, xmlElement2);
		}
		if (m_ToolbarBottomDockSite != null)
		{
			SerializeDockSiteLayout(m_ToolbarBottomDockSite, xmlElement2);
		}
		if (m_ToolbarLeftDockSite != null)
		{
			SerializeDockSiteLayout(m_ToolbarLeftDockSite, xmlElement2);
		}
		if (m_ToolbarRightDockSite != null)
		{
			SerializeDockSiteLayout(m_ToolbarRightDockSite, xmlElement2);
		}
		foreach (Bar bar in m_Bars)
		{
			if (bar.DockSide == eDockSide.None && bar.Name != "" && bar.SaveLayoutChanges)
			{
				XmlElement xmlElement3 = xmlDoc.CreateElement("bar");
				xmlElement2.AppendChild(xmlElement3);
				bar.SerializeLayout(xmlElement3);
			}
		}
	}

	private string GetDockSitesZOrder()
	{
		string text = "";
		if (m_TopDockSite != null && ((Control)m_TopDockSite).Parent != null)
		{
			text += ((Control)m_TopDockSite).Parent.Controls.IndexOf((Control)(object)m_TopDockSite);
		}
		text += ",";
		if (m_BottomDockSite != null && ((Control)m_BottomDockSite).Parent != null)
		{
			text += ((Control)m_BottomDockSite).Parent.Controls.IndexOf((Control)(object)m_BottomDockSite);
		}
		text += ",";
		if (m_LeftDockSite != null && ((Control)m_LeftDockSite).Parent != null)
		{
			text += ((Control)m_LeftDockSite).Parent.Controls.IndexOf((Control)(object)m_LeftDockSite);
		}
		text += ",";
		if (m_RightDockSite != null)
		{
			text += ((Control)m_RightDockSite).Parent.Controls.IndexOf((Control)(object)m_RightDockSite);
		}
		return text;
	}

	private void RestoreDockSiteZOrder(string s)
	{
		try
		{
			string[] array = s.Split(new char[1] { ',' });
			if (array[0] != "" && m_TopDockSite != null && ((Control)m_TopDockSite).Parent != null)
			{
				((Control)m_TopDockSite).Parent.Controls.SetChildIndex((Control)(object)m_TopDockSite, int.Parse(array[0]));
			}
			if (array[1] != "" && m_BottomDockSite != null && ((Control)m_BottomDockSite).Parent != null)
			{
				((Control)m_BottomDockSite).Parent.Controls.SetChildIndex((Control)(object)m_BottomDockSite, int.Parse(array[1]));
			}
			if (array[2] != "" && m_LeftDockSite != null && ((Control)m_LeftDockSite).Parent != null)
			{
				((Control)m_LeftDockSite).Parent.Controls.SetChildIndex((Control)(object)m_LeftDockSite, int.Parse(array[2]));
			}
			if (array[3] != "" && m_RightDockSite != null && ((Control)m_RightDockSite).Parent != null)
			{
				((Control)m_RightDockSite).Parent.Controls.SetChildIndex((Control)(object)m_RightDockSite, int.Parse(array[3]));
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
		if (site.IsDocumentDock || site.DocumentDockContainer != null)
		{
			site.GetDocumentUIManager().SerializeDefinition(xmlBars);
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)site).Controls)
		{
			Control val = item;
			if (val is Bar bar)
			{
				XmlElement xmlElement = xmlBars.OwnerDocument.CreateElement("bar");
				xmlBars.AppendChild(xmlElement);
				bar.Serialize(xmlElement);
			}
		}
	}

	private void SerializeDockSiteLayout(DockSite site, XmlElement xmlBars)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (site.IsDocumentDock || site.DocumentDockContainer != null)
		{
			site.GetDocumentUIManager().SerializeLayout(xmlBars);
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)site).Controls)
		{
			Control val = item;
			if (val is Bar bar && bar.Name != "" && bar.SaveLayoutChanges)
			{
				XmlElement xmlElement = xmlBars.OwnerDocument.CreateElement("bar");
				xmlBars.AppendChild(xmlElement);
				bar.SerializeLayout(xmlElement);
			}
		}
	}

	internal void SetFloatingBarVisible(bool bVisible)
	{
		foreach (Bar bar in m_Bars)
		{
			if (bar.DockSide == eDockSide.None && bar.Visible != bVisible)
			{
				bar.Visible = bVisible;
			}
		}
	}

	public void Customize()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (m_DisabledControls == null)
		{
			m_DisabledControls = new ArrayList();
		}
		Control topLevelControl = ((Control)m_ParentForm).TopLevelControl;
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
				m_DisabledControls.Add(val2);
			}
		}
		m_Customizing = true;
		if (Items.Count == 0)
		{
			m_AutoCreatedCategories = true;
			RescanCategories();
		}
		if (!m_UseCustomCustomizeDialog)
		{
			if (m_frmCustomize == null)
			{
				m_frmCustomize = new frmCustomize(this);
			}
			if (this.EnterCustomize != null)
			{
				this.EnterCustomize(m_frmCustomize, new EventArgs());
			}
			((Control)m_frmCustomize).Show();
			((Form)m_frmCustomize).Owner = ParentForm;
		}
		else if (this.EnterCustomize != null)
		{
			this.EnterCustomize(null, new EventArgs());
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
		foreach (Bar bar in m_Bars)
		{
			if (!((Control)bar).IsDisposed)
			{
				GetSubItemsByName(bar.ItemsContainer, ItemName, arrayList);
			}
		}
		if (m_GlobalContextMenuBar != null)
		{
			GetSubItemsByName(m_GlobalContextMenuBar.ItemsContainer, ItemName, arrayList);
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
		foreach (Bar bar in m_Bars)
		{
			if (!((Control)bar).IsDisposed)
			{
				GetSubItemsByName(bar.ItemsContainer, ItemName, arrayList);
			}
		}
		foreach (DictionaryEntry item in m_Items)
		{
			BaseItem baseItem = (BaseItem)item.Value;
			if (baseItem.Name == ItemName)
			{
				arrayList.Add(baseItem);
			}
			GetSubItemsByName(baseItem, ItemName, arrayList);
		}
		foreach (BaseItem contextMenu in m_ContextMenus)
		{
			if (contextMenu.Name == ItemName)
			{
				arrayList.Add(contextMenu);
			}
			GetSubItemsByName(contextMenu, ItemName, arrayList);
		}
		return arrayList;
	}

	public ArrayList GetItems(string itemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		foreach (Bar bar in m_Bars)
		{
			if (!((Control)bar).IsDisposed)
			{
				BarFunctions.GetSubItemsByNameAndType(bar.ItemsContainer, itemName, arrayList, itemType, useGlobalName: false);
			}
		}
		if (m_GlobalContextMenuBar != null && !((Control)m_GlobalContextMenuBar).IsDisposed)
		{
			BarFunctions.GetSubItemsByNameAndType(m_GlobalContextMenuBar.ItemsContainer, itemName, arrayList, itemType, useGlobalName: false);
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
		if (m_Bars == null || IsDisposed)
		{
			return arrayList;
		}
		foreach (Bar bar in m_Bars)
		{
			if (!((Control)bar).IsDisposed)
			{
				BarFunctions.GetSubItemsByNameAndType(bar.ItemsContainer, itemName, arrayList, itemType, useGlobalName);
			}
		}
		if (m_GlobalContextMenuBar != null && !((Control)m_GlobalContextMenuBar).IsDisposed)
		{
			BarFunctions.GetSubItemsByNameAndType(m_GlobalContextMenuBar.ItemsContainer, itemName, arrayList, itemType, useGlobalName);
		}
		if (fullSearch)
		{
			foreach (DictionaryEntry item in m_Items)
			{
				BaseItem baseItem = (BaseItem)item.Value;
				if (((!useGlobalName && baseItem.Name == itemName) || (useGlobalName && baseItem.GlobalName == itemName)) && baseItem.GetType() == itemType)
				{
					arrayList.Add(baseItem);
				}
				BarFunctions.GetSubItemsByNameAndType(baseItem, itemName, arrayList, itemType, useGlobalName);
			}
			foreach (BaseItem contextMenu in m_ContextMenus)
			{
				if (((!useGlobalName && contextMenu.Name == itemName) || (useGlobalName && contextMenu.GlobalName == itemName)) && contextMenu.GetType() == itemType)
				{
					arrayList.Add(contextMenu);
				}
				BarFunctions.GetSubItemsByNameAndType(contextMenu, itemName, arrayList, itemType, useGlobalName);
			}
		}
		return arrayList;
	}

	public BaseItem GetItem(string ItemName)
	{
		foreach (Bar bar in m_Bars)
		{
			if (!((Control)bar).IsDisposed)
			{
				BaseItem subItemByName = GetSubItemByName(bar.ItemsContainer, ItemName);
				if (subItemByName != null)
				{
					return subItemByName;
				}
			}
		}
		if (m_GlobalContextMenuBar != null)
		{
			return GetSubItemByName(m_GlobalContextMenuBar.ItemsContainer, ItemName);
		}
		return null;
	}

	public BaseItem GetItem(string ItemName, bool FullSearch)
	{
		foreach (Bar bar in m_Bars)
		{
			if (!((Control)bar).IsDisposed)
			{
				BaseItem subItemByName = GetSubItemByName(bar.ItemsContainer, ItemName);
				if (subItemByName != null)
				{
					return subItemByName;
				}
			}
		}
		if (m_GlobalContextMenuBar != null && !((Control)m_GlobalContextMenuBar).IsDisposed)
		{
			BaseItem subItemByName2 = GetSubItemByName(m_GlobalContextMenuBar.ItemsContainer, ItemName);
			if (subItemByName2 != null)
			{
				return subItemByName2;
			}
		}
		foreach (DictionaryEntry item in m_Items)
		{
			BaseItem baseItem = (BaseItem)item.Value;
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
		foreach (BaseItem contextMenu in m_ContextMenus)
		{
			if (contextMenu.Name == ItemName)
			{
				return contextMenu;
			}
			BaseItem subItemByName4 = GetSubItemByName(contextMenu, ItemName);
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
			if (genericItemContainer.MoreItems != null)
			{
				GetSubItemsByName(genericItemContainer.MoreItems, ItemName, list);
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
			if (genericItemContainer.MoreItems != null)
			{
				GetSubItemsByNameAndType(genericItemContainer.MoreItems, ItemName, list, itemType);
			}
		}
		if (objParent.SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if (subItem.GetType() == itemType && subItem.Name == ItemName)
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
			if (genericItemContainer.MoreItems != null)
			{
				BaseItem subItemByName = GetSubItemByName(genericItemContainer.MoreItems, ItemName);
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
		if (IsDisposed || m_Bars == null)
		{
			return;
		}
		foreach (Bar bar in m_Bars)
		{
			if (bar.DockSide == eDockSide.None && ((Control)bar).Parent is Form && ((Control)bar).Parent.Visible)
			{
				((Form)((Control)bar).Parent).TopMost = false;
			}
		}
	}

	public void EndModalDisplay()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (IsDisposed || m_Bars == null)
		{
			return;
		}
		foreach (Bar bar in m_Bars)
		{
			if (bar.Visible && bar.DockSide == eDockSide.None && ((Control)bar).Parent is Form && !((Form)((Control)bar).Parent).TopMost)
			{
				((Form)((Control)bar).Parent).TopMost = true;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode(bool designmode)
	{
		foreach (Bar bar in m_Bars)
		{
			bar.SetDesignMode(designmode);
			bar.RecalcLayout();
		}
	}

	void IOwner.SetFocusItem(BaseItem objFocusItem)
	{
		if (m_FocusItem != null && m_FocusItem != objFocusItem)
		{
			m_FocusItem.OnLostFocus();
		}
		m_FocusItem = objFocusItem;
		if (m_FocusItem != null)
		{
			m_FocusItem.OnGotFocus();
		}
	}

	BaseItem IOwner.GetFocusItem()
	{
		return m_FocusItem;
	}

	void IOwner.SetExpandedItem(BaseItem objItem)
	{
		if (objItem != null && objItem.Parent is PopupItem)
		{
			return;
		}
		if (m_ExpandedItem != null)
		{
			if (m_ExpandedItem.Expanded)
			{
				m_ExpandedItem.Expanded = false;
			}
			m_ExpandedItem = null;
		}
		m_ExpandedItem = objItem;
	}

	BaseItem IOwner.GetExpandedItem()
	{
		return m_ExpandedItem;
	}

	[Browsable(false)]
	public void CustomizeClosing()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		m_Customizing = false;
		if (m_DisabledControls == null)
		{
			return;
		}
		foreach (Control disabledControl in m_DisabledControls)
		{
			Control val = disabledControl;
			val.Enabled = true;
		}
		m_DisabledControls.Clear();
		m_DisabledControls = null;
		m_frmCustomize = null;
		if (this.ExitCustomize != null)
		{
			this.ExitCustomize(this, new EventArgs());
		}
		if (m_AutoCreatedCategories)
		{
			m_AutoCreatedCategories = false;
			Items.Clear();
		}
	}

	void IOwner.DesignTimeContextMenu(BaseItem objItem)
	{
		if (m_frmCustomize != null)
		{
			m_frmCustomize.DesignTimeContextMenu(objItem);
		}
		else if (m_UseCustomCustomizeDialog)
		{
			OnCustomizeContextMenu(objItem, null);
		}
	}

	void IOwner.OnParentPositionChanging()
	{
		if (m_Bars == null)
		{
			return;
		}
		Bar[] array = new Bar[m_Bars.Count];
		m_Bars.CopyTo(array);
		Bar[] array2 = array;
		foreach (Bar bar in array2)
		{
			if (!((Control)bar).IsDisposed && ((Control)bar).IsHandleCreated && bar.BarState == eBarState.Floating)
			{
				NativeFunctions.PostMessage(((Control)bar).Handle, 1125, IntPtr.Zero, IntPtr.Zero);
			}
		}
	}

	void IOwner.StartItemDrag(BaseItem objItem)
	{
		if (this.CustomizeStartItemDrag != null)
		{
			this.CustomizeStartItemDrag(objItem, EventArgs.Empty);
		}
		if (m_frmCustomize != null)
		{
			objItem.Expanded = false;
			m_frmCustomize.DragItem = objItem;
			NativeFunctions.PostMessage(((Control)m_frmCustomize).Handle, 1731, IntPtr.Zero, IntPtr.Zero);
		}
	}

	internal void OnCustomizeContextMenu(object Sender, ButtonItem Parent)
	{
		if (this.CustomizeContextMenu != null)
		{
			this.CustomizeContextMenu(Sender, new CustomizeContextMenuEventArgs(Parent));
		}
	}

	public void RegisterPopup(PopupItem objPopup)
	{
		if (m_RegisteredPopups.Contains(objPopup))
		{
			return;
		}
		if (!base.DesignMode)
		{
			if (!m_FilterInstalled && !m_UseHook)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled = true;
			}
			else if (m_UseHook && m_Hook == null)
			{
				m_Hook = new Hook(this);
			}
		}
		else if (m_Hook == null)
		{
			m_Hook = new Hook(this);
		}
		m_RegisteredPopups.Add(objPopup);
		if (objPopup.GetOwner() == null)
		{
			objPopup.SetOwner(this);
		}
	}

	bool IOwnerMenuSupport.RelayMouseHover()
	{
		foreach (PopupItem registeredPopup in m_RegisteredPopups)
		{
			Control popupControl = registeredPopup.PopupControl;
			if (popupControl == null)
			{
				continue;
			}
			Point pt = popupControl.PointToClient(Control.MousePosition);
			if (popupControl.ClientRectangle.Contains(pt))
			{
				if (popupControl is MenuPanel)
				{
					((MenuPanel)(object)popupControl).InternalMouseHover();
				}
				else if (popupControl is Bar)
				{
					((Bar)(object)popupControl).InternalMouseHover();
				}
				return true;
			}
		}
		return false;
	}

	public void UnregisterPopup(PopupItem objPopup)
	{
		if (m_RegisteredPopups.Contains(objPopup))
		{
			m_RegisteredPopups.Remove(objPopup);
		}
		if (m_RegisteredPopups.Count == 0 && m_Hook != null && (!m_UseHook || base.DesignMode))
		{
			m_Hook.Dispose();
			m_Hook = null;
		}
	}

	private void ProcessDelayedCommands()
	{
		if (!base.DesignMode || m_TopDockSite == null || m_BottomDockSite == null || m_LeftDockSite == null || m_RightDockSite == null || ((Control)m_TopDockSite).Parent == null || ((Control)m_BottomDockSite).Parent == null || ((Control)m_LeftDockSite).Parent == null || ((Control)m_RightDockSite).Parent == null)
		{
			return;
		}
		foreach (Bar bar in Bars)
		{
			bar.ProcessDelayedCommands();
		}
	}

	private void DestroyAutoHidePanels()
	{
		if (m_LeftAutoHidePanel != null)
		{
			if (((Control)m_LeftAutoHidePanel).Parent != null)
			{
				((Control)m_LeftAutoHidePanel).Parent.Controls.Remove((Control)(object)m_LeftAutoHidePanel);
			}
			((Component)(object)m_LeftAutoHidePanel).Dispose();
			m_LeftAutoHidePanel = null;
		}
		if (m_RightAutoHidePanel != null)
		{
			if (((Control)m_RightAutoHidePanel).Parent != null)
			{
				((Control)m_RightAutoHidePanel).Parent.Controls.Remove((Control)(object)m_RightAutoHidePanel);
			}
			((Component)(object)m_RightAutoHidePanel).Dispose();
			m_RightAutoHidePanel = null;
		}
		if (m_TopAutoHidePanel != null)
		{
			if (((Control)m_TopAutoHidePanel).Parent != null)
			{
				((Control)m_TopAutoHidePanel).Parent.Controls.Remove((Control)(object)m_TopAutoHidePanel);
			}
			((Component)(object)m_TopAutoHidePanel).Dispose();
			m_TopAutoHidePanel = null;
		}
		if (m_BottomAutoHidePanel != null)
		{
			if (((Control)m_BottomAutoHidePanel).Parent != null)
			{
				((Control)m_BottomAutoHidePanel).Parent.Controls.Remove((Control)(object)m_BottomAutoHidePanel);
			}
			((Component)(object)m_BottomAutoHidePanel).Dispose();
			m_BottomAutoHidePanel = null;
		}
	}

	private void DockSiteParentChanged(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			if (val == m_TopDockSite && val.Parent is UserControl)
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
		if (((m_TopDockSite != null && ((Control)m_TopDockSite).Parent is UserControl) || m_ParentUserControl != null) && !base.DesignMode)
		{
			Control val = null;
			val = (Control)((m_TopDockSite == null) ? ((object)m_ParentUserControl) : ((object)/*isinst with value type is only supported in some contexts*/));
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
		val = (Control)((m_TopDockSite == null) ? ((object)m_ParentUserControl) : ((object)/*isinst with value type is only supported in some contexts*/));
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
						if (!m_DefinitionLoaded)
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
		return m_ColorScheme.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		m_ColorScheme.Refresh();
		foreach (Bar bar in m_Bars)
		{
			((Control)bar).Invalidate();
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
		if (_DelayCheckTimer == null)
		{
			_DelayCheckTimer = new Timer();
			_DelayCheckTimer.Interval = 200;
			_DelayCheckTimer.Tick += DelayCheckTimerTick;
		}
		_DelayCheckTimer.Start();
	}

	private void DelayCheckTimerTick(object sender, EventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		Timer delayCheckTimer = _DelayCheckTimer;
		_DelayCheckTimer = null;
		delayCheckTimer.Stop();
		delayCheckTimer.Tick -= DelayCheckTimerTick;
		((Component)(object)delayCheckTimer).Dispose();
		if (m_ActiveMdiChild != null && (int)m_ActiveMdiChild.WindowState == 2)
		{
			MdiChildActivated();
		}
	}

	private void OnMdiChildActivate(object sender, EventArgs e)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		if (m_ActiveMdiChild != null)
		{
			((Control)m_ActiveMdiChild).Resize -= OnMdiChildResize;
			((Control)m_ActiveMdiChild).VisibleChanged -= OnMdiChildVisibleChanged;
		}
		m_ActiveMdiChild = null;
		if (ParentForm.ActiveMdiChild != null)
		{
			if (GetMenuBar() != null)
			{
				m_ActiveMdiChild = ParentForm.ActiveMdiChild;
				((Control)m_ActiveMdiChild).Resize += OnMdiChildResize;
				((Control)m_ActiveMdiChild).VisibleChanged += OnMdiChildVisibleChanged;
				if ((int)m_ActiveMdiChild.WindowState == 2 || m_MdiChildMaximized)
				{
					OnMdiChildResize(m_ActiveMdiChild, null);
				}
				else
				{
					DelayCheckMaximizedState();
				}
			}
		}
		else if (m_MdiChildMaximized)
		{
			GetMenuBar()?.ClearMDIChildSystemItems(bRecalcLayout: true);
		}
	}

	private void OnMdiChildVisibleChanged(object sender, EventArgs e)
	{
		if (m_ActiveMdiChild != null && !((Control)m_ActiveMdiChild).Visible && m_MdiChildMaximized)
		{
			GetMenuBar()?.ClearMDIChildSystemItems(bRecalcLayout: true);
			m_MdiChildMaximized = false;
		}
	}

	private void OnMdiChildResize(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		Bar menuBar = GetMenuBar();
		bool flag = false;
		if (m_ActiveMdiChild != null && (int)m_ActiveMdiChild.WindowState != 2)
		{
			if (m_MdiChildMaximized && m_MdiSystemItemVisible)
			{
				if (e == null)
				{
					m_MdiChildMaximized = false;
					return;
				}
				menuBar?.ClearMDIChildSystemItems(bRecalcLayout: false);
				flag = true;
			}
			m_MdiChildMaximized = false;
			if (menuBar != null)
			{
				((Control)menuBar).Refresh();
			}
			return;
		}
		if (!m_MdiChildMaximized)
		{
			if (m_MdiSystemItemVisible)
			{
				if (menuBar == null)
				{
					return;
				}
				menuBar.ShowMDIChildSystemItems(m_ActiveMdiChild, bRecalcLayout: false);
				flag = true;
			}
			m_MdiChildMaximized = true;
		}
		if (menuBar != null && flag)
		{
			menuBar.RecalcLayout();
		}
	}

	private Bar GetMenuBar()
	{
		if (m_Bars == null)
		{
			return null;
		}
		try
		{
			foreach (Bar bar in m_Bars)
			{
				if (bar.MenuBar && bar.Visible)
				{
					return bar;
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
		MDIClientMsgHandler mDIClientMsgHandler = sender as MDIClientMsgHandler;
		if (GetMenuBar() != null || m_HideMdiSystemMenu)
		{
			mDIClientMsgHandler.EatMessage = true;
		}
	}

	bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Invalid comparison between Unknown and I4
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected I4, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Invalid comparison between Unknown and I4
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected I4, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		if (!m_ParentActivated || base.DesignMode)
		{
			return false;
		}
		if (m_ParentForm != null && m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null && !((Control)m_ParentForm.MdiParent).Focused)
		{
			return false;
		}
		Bar menuBar = GetMenuBar();
		int num = WinApi.ToInt(wParam);
		if (menuBar != null && menuBar.ItemsContainer != null && !menuBar.ItemsContainer.DesignMode)
		{
			GenericItemContainer itemsContainer = menuBar.ItemsContainer;
			if (itemsContainer == null)
			{
				return false;
			}
			if (num == 18 || (num == 121 && !m_IgnoreF10Key && ((int)Control.ModifierKeys == 0 || (int)Control.ModifierKeys == 262144)))
			{
				if (itemsContainer.ExpandedItem() != null && (((Control)menuBar).Focused || menuBar.MenuFocus))
				{
					menuBar.ReleaseFocus();
					menuBar.MenuFocus = false;
					m_IgnoreSysKeyUp = true;
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
				m_IgnoreSysKeyUp = true;
				if (num >= 27 && num <= 111)
				{
					int num2 = (int)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
					if (num2 == 0)
					{
						num2 = num;
					}
					if (itemsContainer.SysKeyDown(num2))
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
		if ((Control.ModifierKeys & 0x40000) == 262144 && num > 27 && m_Bars != null)
		{
			m_IgnoreSysKeyUp = true;
			int num3 = 0;
			if ((Control.ModifierKeys & 0x10000) == 65536)
			{
				try
				{
					byte[] array = new byte[256];
					if (NativeFunctions.GetKeyboardState(array))
					{
						byte[] array2 = new byte[2];
						if (NativeFunctions.ToAscii((uint)(int)wParam, 0u, array, array2, 0u) != 0)
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
				num3 = (int)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
			}
			if (num3 != 0)
			{
				foreach (Bar bar in m_Bars)
				{
					if (bar.ItemsContainer != null && !bar.ItemsContainer.DesignMode && ((Control)bar).Enabled && bar.Visible && bar.ItemsContainer.SysKeyDown(num3))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (!m_ParentActivated || base.DesignMode)
		{
			return false;
		}
		if (m_ParentForm != null && m_ParentForm.IsMdiChild && m_ParentForm.MdiParent != null && !((Control)m_ParentForm.MdiParent).Focused)
		{
			return false;
		}
		int num = WinApi.ToInt(wParam);
		if (num == 18 || num == 121)
		{
			if (m_IgnoreSysKeyUp)
			{
				m_IgnoreSysKeyUp = false;
				return false;
			}
			if (m_EatSysKeyUp)
			{
				m_EatSysKeyUp = false;
				return true;
			}
			foreach (Bar bar in Bars)
			{
				if (bar.Visible)
				{
					bar.HideAllToolTips();
				}
			}
			if (num == 18 || (num == 121 && !m_IgnoreF10Key))
			{
				Bar menuBar = GetMenuBar();
				if (menuBar != null && !menuBar.ItemsContainer.DesignMode)
				{
					if (menuBar.MenuFocus)
					{
						menuBar.MenuFocus = false;
					}
					else
					{
						menuBar.MenuFocus = true;
					}
					return true;
				}
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected I4, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		bool designMode = base.DesignMode;
		int num = WinApi.ToInt(wParam);
		if (m_RegisteredPopups.Count > 0 && ((BaseItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1]).Parent == null)
		{
			PopupItem popupItem = (PopupItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1];
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
				Keys val2 = (Keys)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
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
				Keys val4 = (Keys)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
				if ((int)val4 == 0)
				{
					val4 = (Keys)num;
				}
				FocusedBar.ExKeyDown(new KeyEventArgs(val4));
				return true;
			}
		}
		else if (num == 27)
		{
			foreach (Bar bar in Bars)
			{
				if (bar.IsBarMoving)
				{
					bar.OnEscapeKey();
					break;
				}
			}
		}
		if ((!m_ParentActivated && !(Form.ActiveForm is FloatingContainer)) || designMode)
		{
			return false;
		}
		if (num >= 112 || (int)Control.ModifierKeys != 0 || (WinApi.ToInt(lParam) & 0x1000000000L) != 0L || num == 46 || num == 45)
		{
			int key = Control.ModifierKeys | num;
			return ProcessShortcut((eShortcut)key);
		}
		return false;
	}

	private bool ProcessShortcut(eShortcut key)
	{
		if (ParentForm != null && ((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0 && (ParentForm.IsMdiContainer || ParentForm.IsMdiChild) && Application.OpenForms[((ReadOnlyCollectionBase)(object)Application.OpenForms).Count - 1] != ParentForm)
		{
			return false;
		}
		bool flag = BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
		if (m_AutoDispatchShortcuts != null && m_AutoDispatchShortcuts.Contains(key))
		{
			flag = false;
		}
		return !m_DispatchShortcuts && flag;
	}

	bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (m_RegisteredPopups == null || base.DesignMode)
		{
			return false;
		}
		if (m_RegisteredPopups.Count == 0)
		{
			Bar menuBar = GetMenuBar();
			if (menuBar != null && menuBar.MenuFocus && hWnd != ((Control)menuBar).Handle)
			{
				menuBar.MenuFocus = false;
			}
			return false;
		}
		for (int num = m_RegisteredPopups.Count - 1; num >= 0; num--)
		{
			PopupItem popupItem = m_RegisteredPopups[num] as PopupItem;
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
					string className = NativeFunctions.GetClassName(hWnd);
					className = className.ToLower();
					if (className.IndexOf("combolbox") >= 0)
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
			if (m_RegisteredPopups.Count == 0)
			{
				break;
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (m_RegisteredPopups.Count > 0)
		{
			bool flag = true;
			foreach (BaseItem registeredPopup in m_RegisteredPopups)
			{
				Control popupControl = ((PopupItem)registeredPopup).PopupControl;
				Control val = null;
				if (registeredPopup.Parent != null)
				{
					object containerControl = registeredPopup.Parent.ContainerControl;
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
				if (popupControl != null && val2 != null && popupControl != val2 && !registeredPopup.IsAnyOnHandle(val2.Handle) && (val == null || (val != null && val != val2)))
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
		if (!ShowCustomizeContextMenu || m_frmCustomize != null || base.DesignMode)
		{
			return;
		}
		m_ContextMenu = new ButtonItem("sys_customizecontextnmenu");
		eDotNetBarStyle style = eDotNetBarStyle.OfficeXP;
		if (m_Bars.Count > 0)
		{
			style = m_Bars[0].Style;
		}
		m_ContextMenu.Style = style;
		ButtonItem buttonItem;
		foreach (Bar bar2 in m_Bars)
		{
			if ((bar2.LayoutType != eLayoutType.DockContainer || bar2.Items.Count != 0) && bar2.CanHide && ((bar2.LayoutType == eLayoutType.DockContainer && bar2.CanCustomize) || bar2.LayoutType != eLayoutType.DockContainer) && ((Control)bar2).Text != "")
			{
				buttonItem = new ButtonItem();
				buttonItem.Text = ((Control)bar2).Text;
				buttonItem.Checked = bar2.Visible || bar2.AutoHide;
				buttonItem.SetSystemItem(b: true);
				buttonItem.Tag = bar2;
				buttonItem.Click += CustomizeItemClick;
				m_ContextMenu.SubItems.Add(buttonItem);
			}
		}
		buttonItem = new ButtonItem("customize");
		if (m_ContextMenu.SubItems.Count > 0)
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
		m_ContextMenu.SubItems.Add(buttonItem);
		OnCustomizeContextMenu(this, (ButtonItem)m_ContextMenu);
		RegisterPopup(m_ContextMenu);
		m_ContextMenu.PopupMenu(bar.PointToScreen(new Point(e.X, e.Y)));
	}

	private void CustomizeItemClick(object sender, EventArgs e)
	{
		BaseItem baseItem = sender as BaseItem;
		m_ContextMenu.Expanded = false;
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
			bar.InvokeUserVisibleChanged();
		}
		m_ContextMenu.Dispose();
		m_ContextMenu = null;
	}

	public void ResetUsageData()
	{
		foreach (Bar bar in m_Bars)
		{
			foreach (BaseItem item in bar.Items)
			{
				if (item is IPersonalizedMenuItem { MenuVisibility: eMenuVisibility.VisibleIfRecentlyUsed } personalizedMenuItem)
				{
					personalizedMenuItem.RecentlyUsed = false;
				}
				if (item.SubItems.Count > 0)
				{
					ResetItemUsageData(item);
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
		ArrayList arrayList = new ArrayList(m_RegisteredPopups);
		foreach (PopupItem item in arrayList)
		{
			item.ClosePopup();
		}
	}

	void IOwnerItemEvents.InvokeItemTextChanged(BaseItem item, EventArgs e)
	{
		if (this.ItemTextChanged != null)
		{
			this.ItemTextChanged(item, e);
		}
	}

	void IOwner.InvokeResetDefinition(BaseItem item, EventArgs e)
	{
		if (this.ResetDefinition != null)
		{
			this.ResetDefinition(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerLoad(PopupItem item, EventArgs e)
	{
		if (this.PopupContainerLoad != null)
		{
			this.PopupContainerLoad(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerUnload(PopupItem item, EventArgs e)
	{
		if (this.PopupContainerUnload != null)
		{
			this.PopupContainerUnload(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupOpen(PopupItem item, PopupOpenEventArgs e)
	{
		if (this.PopupOpen != null)
		{
			this.PopupOpen(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupClose(PopupItem item, EventArgs e)
	{
		if (this.PopupClose != null)
		{
			this.PopupClose(item, e);
		}
	}

	void IOwnerMenuSupport.InvokePopupShowing(PopupItem item, EventArgs e)
	{
		if (this.PopupShowing != null)
		{
			this.PopupShowing(item, e);
		}
	}

	void IOwnerItemEvents.InvokeExpandedChange(BaseItem item, EventArgs e)
	{
		if (this.ExpandedChange != null)
		{
			this.ExpandedChange(item, e);
		}
	}

	void IOwnerBarSupport.InvokeBarDock(Bar bar, EventArgs e)
	{
		if (this.BarDock != null)
		{
			this.BarDock(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBarUndock(Bar bar, EventArgs e)
	{
		if (this.BarUndock != null)
		{
			this.BarUndock(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBeforeDockTabDisplay(BaseItem item, EventArgs e)
	{
		if (this.BeforeDockTabDisplay != null)
		{
			this.BeforeDockTabDisplay(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseDown(BaseItem item, MouseEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (this.MouseDown != null)
		{
			this.MouseDown.Invoke((object)item, e);
		}
		if (item.ClickAutoRepeat && (int)e.Button == 1048576)
		{
			m_ClickRepeatItem = item;
			if (m_ClickTimer == null)
			{
				m_ClickTimer = new Timer();
			}
			m_ClickTimer.Interval = item.ClickRepeatInterval;
			m_ClickTimer.Tick += ClickTimerTick;
			m_ClickTimer.Start();
		}
	}

	void IOwnerItemEvents.InvokeMouseEnter(BaseItem item, EventArgs e)
	{
		if (this.MouseEnter != null)
		{
			this.MouseEnter(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseLeave(BaseItem item, EventArgs e)
	{
		if (this.MouseLeave != null)
		{
			this.MouseLeave(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseHover(BaseItem item, EventArgs e)
	{
		if (this.MouseHover != null)
		{
			this.MouseHover(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseMove(BaseItem item, MouseEventArgs e)
	{
		if (this.MouseMove != null)
		{
			this.MouseMove.Invoke((object)item, e);
		}
	}

	private void ClickTimerTick(object sender, EventArgs e)
	{
		if (m_ClickRepeatItem != null)
		{
			m_ClickRepeatItem.RaiseClick();
		}
		else
		{
			m_ClickTimer.Stop();
		}
	}

	void IOwnerItemEvents.InvokeMouseUp(BaseItem item, MouseEventArgs e)
	{
		if (this.MouseUp != null)
		{
			this.MouseUp.Invoke((object)item, e);
		}
		if (m_ClickTimer != null && m_ClickTimer.Enabled)
		{
			m_ClickTimer.Stop();
			m_ClickTimer.Enabled = false;
		}
	}

	void IOwnerItemEvents.InvokeGotFocus(BaseItem item, EventArgs e)
	{
		if (this.GotFocus != null)
		{
			this.GotFocus(item, e);
		}
	}

	void IOwnerItemEvents.InvokeLostFocus(BaseItem item, EventArgs e)
	{
		if (this.LostFocus != null)
		{
			this.LostFocus(item, e);
		}
	}

	void IOwner.InvokeUserCustomize(object sender, EventArgs e)
	{
		if (this.UserCustomize != null)
		{
			this.UserCustomize(sender, e);
		}
	}

	void IOwner.InvokeEndUserCustomize(object sender, EndUserCustomizeEventArgs e)
	{
		if (this.EndUserCustomize != null)
		{
			this.EndUserCustomize(sender, e);
		}
	}

	void IOwner.InvokeDefinitionLoaded(object sender, EventArgs e)
	{
		if (this.DefinitionLoaded != null)
		{
			this.DefinitionLoaded(sender, e);
		}
	}

	void IOwnerItemEvents.InvokeItemRemoved(BaseItem item, BaseItem parent)
	{
		if (this.ItemRemoved != null)
		{
			this.ItemRemoved(item, new ItemRemovedEventArgs(parent));
		}
	}

	void IOwnerItemEvents.InvokeItemAdded(BaseItem item, EventArgs e)
	{
		if (this.ItemAdded != null)
		{
			this.ItemAdded(item, e);
		}
	}

	void IOwnerItemEvents.InvokeContainerLoadControl(BaseItem item, EventArgs e)
	{
		if (this.ContainerLoadControl != null)
		{
			this.ContainerLoadControl(item, e);
		}
	}

	void IOwnerItemEvents.InvokeContainerControlDeserialize(BaseItem item, ControlContainerSerializationEventArgs e)
	{
		if (this.ContainerControlDeserialize != null)
		{
			this.ContainerControlDeserialize(item, e);
		}
	}

	void IOwnerItemEvents.InvokeContainerControlSerialize(BaseItem item, ControlContainerSerializationEventArgs e)
	{
		if (this.ContainerControlSerialize != null)
		{
			this.ContainerControlSerialize(item, e);
		}
	}

	void IOwnerItemEvents.InvokeOptionGroupChanging(BaseItem item, OptionGroupChangingEventArgs e)
	{
		if (this.OptionGroupChanging != null)
		{
			this.OptionGroupChanging(item, e);
		}
	}

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (this.ButtonCheckedChanged != null)
		{
			this.ButtonCheckedChanged(item, e);
		}
	}

	void IOwnerItemEvents.InvokeToolTipShowing(object item, EventArgs e)
	{
		if (this.ToolTipShowing != null)
		{
			this.ToolTipShowing(item, e);
		}
	}

	void IOwnerBarSupport.InvokeDockTabChange(Bar bar, DockTabChangeEventArgs e)
	{
		if (this.DockTabChange != null)
		{
			this.DockTabChange(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBarClosing(Bar bar, BarClosingEventArgs e)
	{
		if (this.BarClosing != null)
		{
			this.BarClosing(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeBarTearOff(Bar bar, EventArgs e)
	{
		if (this.BarTearOff != null)
		{
			this.BarTearOff(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeAutoHideChanged(Bar bar, EventArgs e)
	{
		if (this.AutoHideChanged != null)
		{
			this.AutoHideChanged(bar, e);
		}
	}

	void IOwnerBarSupport.InvokeAutoHideDisplay(Bar bar, AutoHideDisplayEventArgs e)
	{
		if (this.AutoHideDisplay != null)
		{
			this.AutoHideDisplay(bar, e);
		}
	}

	internal bool IsHandleOwnedByPopup(IntPtr handle)
	{
		if (m_RegisteredPopups.Count > 0)
		{
			foreach (BaseItem registeredPopup in m_RegisteredPopups)
			{
				Control popupControl = ((PopupItem)registeredPopup).PopupControl;
				if ((popupControl != null && popupControl.Handle == handle) || registeredPopup.IsAnyOnHandle(handle))
				{
					return true;
				}
			}
		}
		return false;
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
		}
	}

	public string GetContextMenuEx(Control control)
	{
		string text = (string)m_ContextExMenus[control];
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
			if (m_ContextExMenus.Contains(control))
			{
				m_ContextExMenus.Remove(control);
			}
		}
		else if (m_ContextExMenus.Contains(control))
		{
			m_ContextExMenus[control] = value;
		}
		else
		{
			m_ContextExMenus[control] = value;
		}
	}
}
