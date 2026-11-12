using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public class SideBar : Control, IOwnerItemEvents, IOwner, IThemeCache, IMessageHandlerClient, IOwnerMenuSupport, IBarDesignerServices, ICustomSerialization
{
	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

	public class SideBarAccessibleObject : ControlAccessibleObject
	{
		private SideBar m_Owner;

		public override AccessibleRole Role => ((Control)m_Owner).AccessibleRole;

		public override AccessibleObject Parent
		{
			get
			{
				if (((Control)m_Owner).Parent != null)
				{
					return ((Control)m_Owner).Parent.AccessibilityObject;
				}
				return null;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (((Control)m_Owner).Parent != null)
				{
					return ((Control)m_Owner).Parent.RectangleToScreen(((Control)m_Owner).Bounds);
				}
				return ((Control)m_Owner).DisplayRectangle;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				AccessibleStates result = (AccessibleStates)256;
				if (((Control)m_Owner).Focused)
				{
					result = (AccessibleStates)4;
				}
				return result;
			}
		}

		public SideBarAccessibleObject(SideBar owner)
			: base((Control)(object)owner)
		{
			m_Owner = owner;
		}

		internal void GenerateEvent(BaseItem sender, AccessibleEvents e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			int num = m_Owner.Panels.IndexOf(sender);
			if (num >= 0)
			{
				((Control)m_Owner).AccessibilityNotifyClients(e, num);
			}
		}

		public override int GetChildCount()
		{
			return m_Owner.Panels.Count;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			return m_Owner.Panels[iIndex].AccessibleObject;
		}
	}

	[CompilerGenerated]
	private MouseEventHandler m_ItemDoubleClick;

	private MouseEventHandler EventMouseDown;

	private MouseEventHandler EventMouseUp;

	private EventHandler EventMouseEnter;

	private EventHandler EventMouseLeave;

	private MouseEventHandler EventMouseMove;

	private EventHandler EventMouseHover;

	private EventHandler EventLostFocus;

	private EventHandler EventGotFocus;

	private SideBarContainerItem m_ItemContainer;

	private Timer m_ClickTimer;

	private BaseItem m_ClickRepeatItem;

	private BaseItem m_ExpandedItem;

	private BaseItem m_FocusItem;

	private ImageList m_ImageList;

	private ImageList m_ImageListMedium;

	private ImageList m_ImageListLarge;

	private bool m_ShowToolTips = true;

	private bool m_ShowShortcutKeysInToolTips;

	private Hashtable m_ShortcutTable = new Hashtable();

	private eBorderType m_BorderStyle = eBorderType.Sunken;

	private SideBarPanelItem m_DelayedExpandedPanel;

	private BaseItem m_DragItem;

	private bool m_DragInProgress;

	private Cursor m_MoveCursor;

	private Cursor m_CopyCursor;

	private Cursor m_NACursor;

	private IDesignTimeProvider m_DesignTimeProvider;

	private int m_InsertPosition;

	private bool m_InsertBefore;

	private bool m_AllowUserCustomize = true;

	private bool m_ThemeAware;

	private ThemeWindow m_ThemeWindow;

	private ThemeRebar m_ThemeRebar;

	private ThemeToolbar m_ThemeToolbar;

	private ThemeHeader m_ThemeHeader;

	private ThemeScrollBar m_ThemeScrollBar;

	private ThemeExplorerBar m_ThemeExplorerBar;

	private ThemeProgress m_ThemeProgress;

	private ThemeButton m_ThemeButton;

	private bool m_UseNativeDragDrop;

	private bool m_AllowExternalDrop;

	private bool m_ExternalDragInProgress;

	private bool m_DragLeft;

	private bool m_DispatchShortcuts;

	private bool m_FilterInstalled;

	private ColorScheme m_ColorScheme;

	private const string INFO_EMPTYSIDEBAR = "Right-click and choose Add New Panel or use Panels collection to create new panels. Right-click to Choose Color Scheme.";

	private eSideBarAppearance m_Appearance;

	private eSideBarColorScheme m_PredefinedColorScheme = eSideBarColorScheme.Blue;

	private bool m_UsingSystemColors;

	private bool m_MenuEventSupport;

	private IBarItemDesigner m_BarDesigner;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private bool m_DisabledImagesGrayScale = true;

	private ArrayList m_RegisteredPopups = new ArrayList();

	private Hook m_Hook;

	IBarItemDesigner IBarDesignerServices.Designer
	{
		get
		{
			return m_BarDesigner;
		}
		set
		{
			m_BarDesigner = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(eRenderMode.Global)]
	public eRenderMode RenderMode
	{
		get
		{
			return m_RenderMode;
		}
		set
		{
			if (m_RenderMode != value)
			{
				m_RenderMode = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public BaseRenderer Renderer
	{
		get
		{
			return m_Renderer;
		}
		set
		{
			m_Renderer = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eSideBarAppearance.Traditional)]
	[Category("Appearance")]
	[Description("Indicates visual appearance for the control.")]
	public eSideBarAppearance Appearance
	{
		get
		{
			return m_Appearance;
		}
		set
		{
			if (m_Appearance == value)
			{
				return;
			}
			m_Appearance = value;
			m_ItemContainer.Appearance = value;
			if (m_Appearance == eSideBarAppearance.Flat && Panels.Count > 0 && Panels[0] is SideBarPanelItem && !((SideBarPanelItem)Panels[0]).BackgroundStyle.Custom)
			{
				PredefinedColorScheme = eSideBarColorScheme.SystemColors;
				if (((Component)this).DesignMode)
				{
					ApplyPredefinedColorScheme(eSideBarColorScheme.SystemColors);
				}
			}
			((Control)this).Refresh();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool UsingSystemColors
	{
		get
		{
			return m_UsingSystemColors;
		}
		set
		{
			m_UsingSystemColors = value;
		}
	}

	[DesignOnly(true)]
	[Browsable(false)]
	public eSideBarColorScheme PredefinedColorScheme
	{
		get
		{
			return m_PredefinedColorScheme;
		}
		set
		{
			m_PredefinedColorScheme = value;
			if (m_PredefinedColorScheme == eSideBarColorScheme.SystemColors)
			{
				m_UsingSystemColors = true;
			}
			else
			{
				m_UsingSystemColors = false;
			}
			if (!((Component)this).DesignMode)
			{
				ApplyPredefinedColorScheme(m_PredefinedColorScheme);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Gets or sets Bar Color Scheme.")]
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
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the side bar.")]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_ItemContainer.Style;
		}
		set
		{
			if (m_ItemContainer.Style != value)
			{
				m_ColorScheme.Style = value;
				m_ItemContainer.Style = value;
				((Control)this).Invalidate();
				RecalcLayout();
			}
		}
	}

	protected bool IsThemed
	{
		get
		{
			if (m_ThemeAware && (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.OfficeXP || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle)) && BarFunctions.ThemedOS && Themes.ThemesActive)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return m_ThemeAware;
		}
		set
		{
			m_ThemeAware = value;
			m_ItemContainer.ThemeAware = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	ThemeWindow IThemeCache.ThemeWindow
	{
		get
		{
			if (m_ThemeWindow == null)
			{
				m_ThemeWindow = new ThemeWindow((Control)(object)this);
			}
			return m_ThemeWindow;
		}
	}

	ThemeRebar IThemeCache.ThemeRebar
	{
		get
		{
			if (m_ThemeRebar == null)
			{
				m_ThemeRebar = new ThemeRebar((Control)(object)this);
			}
			return m_ThemeRebar;
		}
	}

	ThemeToolbar IThemeCache.ThemeToolbar
	{
		get
		{
			if (m_ThemeToolbar == null)
			{
				m_ThemeToolbar = new ThemeToolbar((Control)(object)this);
			}
			return m_ThemeToolbar;
		}
	}

	ThemeHeader IThemeCache.ThemeHeader
	{
		get
		{
			if (m_ThemeHeader == null)
			{
				m_ThemeHeader = new ThemeHeader((Control)(object)this);
			}
			return m_ThemeHeader;
		}
	}

	ThemeScrollBar IThemeCache.ThemeScrollBar
	{
		get
		{
			if (m_ThemeScrollBar == null)
			{
				m_ThemeScrollBar = new ThemeScrollBar((Control)(object)this);
			}
			return m_ThemeScrollBar;
		}
	}

	ThemeExplorerBar IThemeCache.ThemeExplorerBar
	{
		get
		{
			if (m_ThemeExplorerBar == null)
			{
				m_ThemeExplorerBar = new ThemeExplorerBar((Control)(object)this);
			}
			return m_ThemeExplorerBar;
		}
	}

	ThemeProgress IThemeCache.ThemeProgress
	{
		get
		{
			if (m_ThemeProgress == null)
			{
				m_ThemeProgress = new ThemeProgress((Control)(object)this);
			}
			return m_ThemeProgress;
		}
	}

	ThemeButton IThemeCache.ThemeButton
	{
		get
		{
			if (m_ThemeButton == null)
			{
				m_ThemeButton = new ThemeButton((Control)(object)this);
			}
			return m_ThemeButton;
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SideBarPanelsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[Description("Returns the collection of side-bar Panels.")]
	public SubItemsCollection Panels => m_ItemContainer.SubItems;

	[Browsable(true)]
	[Category("Data")]
	[Description("Gets or sets the expanded panel. Only one panel can be expanded at a time.")]
	public SideBarPanelItem ExpandedPanel
	{
		get
		{
			return m_ItemContainer.ExpandedItem() as SideBarPanelItem;
		}
		set
		{
			if (!m_ItemContainer.SubItems.Contains(value))
			{
				m_DelayedExpandedPanel = value;
			}
			else
			{
				value.Expanded = true;
			}
		}
	}

	Form IOwner.ParentForm
	{
		get
		{
			return ((Control)this).FindForm();
		}
		set
		{
		}
	}

	bool IOwner.DesignMode => ((Component)this).DesignMode;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Specifies whether end-user can rearrange the items inside the panels.")]
	public bool AllowUserCustomize
	{
		get
		{
			return m_AllowUserCustomize;
		}
		set
		{
			m_AllowUserCustomize = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifies whether native .NET Drag and Drop is used by side-bar to perform drag and drop operations.")]
	public bool UseNativeDragDrop
	{
		get
		{
			return m_UseNativeDragDrop;
		}
		set
		{
			m_UseNativeDragDrop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	public bool AllowExternalDrop
	{
		get
		{
			return m_AllowExternalDrop;
		}
		set
		{
			m_AllowExternalDrop = value;
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
	[Category("Data")]
	[DefaultValue(null)]
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
	[Category("Data")]
	[DefaultValue(null)]
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
	[Category("Data")]
	[DefaultValue(null)]
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

	bool IOwner.DragInProgress => m_DragInProgress;

	BaseItem IOwner.DragItem => m_DragItem;

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

	bool IOwner.AlwaysDisplayKeyAccelerators
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	Form IOwner.ActiveMdiChild
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				return null;
			}
			if (val.IsMdiContainer)
			{
				return val.ActiveMdiChild;
			}
			return null;
		}
	}

	bool IOwner.ShowResetButton
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public SideBarContainerItem ItemsContainer => m_ItemContainer;

	[Browsable(true)]
	[DefaultValue(eBorderType.Sunken)]
	[Category("Appearance")]
	[Description("Indicates border style when Bar is docked.")]
	public eBorderType BorderStyle
	{
		get
		{
			return m_BorderStyle;
		}
		set
		{
			if (m_BorderStyle != value)
			{
				if (m_BorderStyle == eBorderType.None)
				{
					m_BorderStyle = value;
					RecalcSize();
				}
				else
				{
					m_BorderStyle = value;
				}
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	bool ICustomSerialization.HasSerializeItemHandlers => this.SerializeItem != null;

	bool ICustomSerialization.HasDeserializeItemHandlers => this.DeserializeItem != null;

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

	private bool IsParentFormActive
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				return false;
			}
			if (val.IsMdiChild)
			{
				if (val.MdiParent == null)
				{
					return false;
				}
				if (val.MdiParent.ActiveMdiChild != val)
				{
					return false;
				}
			}
			else if (val != Form.ActiveForm)
			{
				return false;
			}
			return true;
		}
	}

	bool IMessageHandlerClient.IsModal
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return val.Modal;
			}
			return false;
		}
	}

	bool IOwnerMenuSupport.PersonalizedAllVisible
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ShowFullMenusOnHover
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.AlwaysShowFullMenus
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ShowPopupShadow => false;

	eMenuDropShadow IOwnerMenuSupport.MenuDropShadow
	{
		get
		{
			return eMenuDropShadow.Hide;
		}
		set
		{
		}
	}

	ePopupAnimation IOwnerMenuSupport.PopupAnimation
	{
		get
		{
			return ePopupAnimation.SystemDefault;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.AlphaBlendShadow
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public event EventHandler ButtonCheckedChanged;

	[Description("Occurs when Item is clicked.")]
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
	public event EventHandler PopupContainerLoad;

	[Description("Occurs when popup of type container is unloading.")]
	public event EventHandler PopupContainerUnload;

	[Description("Occurs when popup item is about to open.")]
	public event EventHandler PopupOpen;

	[Description("Occurs when popup item is closing.")]
	public event EventHandler PopupClose;

	[Description("Occurs just before popup window is shown.")]
	public event EventHandler PopupShowing;

	[Description("Occurs when Item Expanded property has changed.")]
	public event EventHandler ExpandedChange;

	[Description("Occurs when mouse button is pressed.")]
	public event MouseEventHandler MouseDown
	{
		add
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			EventMouseDown = (MouseEventHandler)Delegate.Combine((Delegate?)(object)EventMouseDown, (Delegate?)(object)value);
		}
		remove
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			EventMouseDown = (MouseEventHandler)Delegate.Remove((Delegate?)(object)EventMouseDown, (Delegate?)(object)value);
		}
	}

	[Description("Occurs when mouse button is released.")]
	public event MouseEventHandler MouseUp
	{
		add
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			EventMouseUp = (MouseEventHandler)Delegate.Combine((Delegate?)(object)EventMouseUp, (Delegate?)(object)value);
		}
		remove
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			EventMouseUp = (MouseEventHandler)Delegate.Remove((Delegate?)(object)EventMouseUp, (Delegate?)(object)value);
		}
	}

	[Description("Occurs when mouse enters the item.")]
	public event EventHandler MouseEnter
	{
		add
		{
			EventMouseEnter = (EventHandler)Delegate.Combine(EventMouseEnter, value);
		}
		remove
		{
			EventMouseEnter = (EventHandler)Delegate.Remove(EventMouseEnter, value);
		}
	}

	[Description("Occurs when mouse leaves the item.")]
	public event EventHandler MouseLeave
	{
		add
		{
			EventMouseLeave = (EventHandler)Delegate.Combine(EventMouseLeave, value);
		}
		remove
		{
			EventMouseLeave = (EventHandler)Delegate.Remove(EventMouseLeave, value);
		}
	}

	[Description("Occurs when mouse moves over the item.")]
	public event MouseEventHandler MouseMove
	{
		add
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			EventMouseMove = (MouseEventHandler)Delegate.Combine((Delegate?)(object)EventMouseMove, (Delegate?)(object)value);
		}
		remove
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			EventMouseMove = (MouseEventHandler)Delegate.Remove((Delegate?)(object)EventMouseMove, (Delegate?)(object)value);
		}
	}

	[Description("Occurs when mouse remains still inside an item for an amount of time.")]
	public event EventHandler MouseHover
	{
		add
		{
			EventMouseHover = (EventHandler)Delegate.Combine(EventMouseHover, value);
		}
		remove
		{
			EventMouseHover = (EventHandler)Delegate.Remove(EventMouseHover, value);
		}
	}

	[Description("Occurs when item loses input focus.")]
	public event EventHandler LostFocus
	{
		add
		{
			EventLostFocus = (EventHandler)Delegate.Combine(EventLostFocus, value);
		}
		remove
		{
			EventLostFocus = (EventHandler)Delegate.Remove(EventLostFocus, value);
		}
	}

	[Description("Occurs when item receives input focus.")]
	public event EventHandler GotFocus
	{
		add
		{
			EventGotFocus = (EventHandler)Delegate.Combine(EventGotFocus, value);
		}
		remove
		{
			EventGotFocus = (EventHandler)Delegate.Remove(EventGotFocus, value);
		}
	}

	[Description("Occurs when user changes the item position.")]
	public event EventHandler UserCustomize;

	[Description("Occurs after an Item is removed from SubItemsCollection.")]
	public event ItemRemovedEventHandler ItemRemoved;

	[Description("Occurs after an Item has been added to the SubItemsCollection.")]
	public event EventHandler ItemAdded;

	[Description("Occurs when ControlContainerControl is created and contained control is needed.")]
	public event EventHandler ContainerLoadControl;

	[Description("Occurs when Text property of an Item has changed.")]
	public event EventHandler ItemTextChanged;

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize;

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize;

	[Description("Occurs after DotNetBar definition is loaded.")]
	public event EventHandler DefinitionLoaded;

	public event OptionGroupChangingEventHandler OptionGroupChanging;

	public event EventHandler ToolTipShowing;

	public event SerializeItemEventHandler SerializeItem;

	public event SerializeItemEventHandler DeserializeItem;

	public event EventHandler ItemLayoutUpdated;

	public SideBar()
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		m_ItemContainer = new SideBarContainerItem();
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.Stretch = false;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.SetOwner(this);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		try
		{
			m_MoveCursor = new Cursor(typeof(DotNetBarManager), "DRAGMOVE.CUR");
			m_CopyCursor = new Cursor(typeof(DotNetBarManager), "DRAGCOPY.CUR");
			m_NACursor = new Cursor(typeof(DotNetBarManager), "DRAGNONE.CUR");
		}
		catch (Exception)
		{
			m_MoveCursor = null;
			m_CopyCursor = null;
			m_NACursor = null;
		}
		((Control)this).AccessibleRole = (AccessibleRole)22;
		m_ColorScheme = new ColorScheme();
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (m_ClickTimer != null)
			{
				m_ClickTimer.Stop();
				((Component)(object)m_ClickTimer).Dispose();
				m_ClickTimer = null;
			}
			StyleManager.Unregister((Control)(object)this);
		}
		((Control)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (m_ItemContainer != null)
		{
			m_ItemContainer.UpdateBindings();
		}
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new SideBarAccessibleObject(this);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_ItemContainer.InternalClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_ItemContainer.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		ExKeyDown(e);
		((Control)this).OnKeyDown(e);
	}

	internal void ExKeyDown(KeyEventArgs e)
	{
		m_ItemContainer.InternalKeyDown(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		((IOwner)this).SetFocusItem((BaseItem)null);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (m_ItemContainer != null)
		{
			m_ItemContainer.FocusNextItem();
		}
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		if (((Control)this).Focused)
		{
			if ((int)keyData == 40)
			{
				if (m_ItemContainer != null)
				{
					m_ItemContainer.FocusNextItem();
				}
				if (((IOwner)this).GetFocusItem() != null)
				{
					return true;
				}
			}
			else if ((int)keyData == 38)
			{
				if (m_ItemContainer != null)
				{
					m_ItemContainer.FocusPreviousItem();
				}
				if (((IOwner)this).GetFocusItem() != null)
				{
					return true;
				}
			}
			else if ((int)keyData == 13 && ((IOwner)this).GetFocusItem() != null)
			{
				BaseItem focusItem = ((IOwner)this).GetFocusItem();
				if (focusItem is SideBarPanelItem)
				{
					focusItem.Expanded = !focusItem.Expanded;
				}
				else
				{
					focusItem.RaiseClick();
				}
				return true;
			}
		}
		return ((Control)this).ProcessDialogKey(keyData);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		m_ItemContainer.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		m_ItemContainer.InternalMouseHover();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		m_ItemContainer.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (m_DragInProgress)
		{
			if (!m_UseNativeDragDrop)
			{
				MouseDragOver(e.X, e.Y, null);
			}
		}
		else
		{
			m_ItemContainer.InternalMouseMove(e);
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		ExpandedPanel?.OnMouseWheel(e);
		((Control)this).OnMouseWheel(e);
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		if (m_DragInProgress || m_ExternalDragInProgress)
		{
			Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
			MouseDragOver(point.X, point.Y, e);
			m_DragLeft = false;
		}
		((Control)this).OnDragOver(e);
	}

	protected override void OnDragLeave(EventArgs e)
	{
		if (m_DragInProgress || m_ExternalDragInProgress)
		{
			MouseDragOver(-1, -1, null);
		}
		m_DragLeft = true;
		m_ExternalDragInProgress = false;
		((Control)this).OnDragLeave(e);
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		((Control)this).OnDragEnter(e);
		if (m_DragInProgress || !m_AllowExternalDrop)
		{
			return;
		}
		if (e.Data.GetData(typeof(ButtonItem)) == null)
		{
			if ((int)e.Effect != 0)
			{
				m_ExternalDragInProgress = true;
			}
			return;
		}
		if ((e.AllowedEffect & 2) == 2)
		{
			e.Effect = (DragDropEffects)2;
		}
		else if ((e.AllowedEffect & 1) == 1)
		{
			e.Effect = (DragDropEffects)2;
		}
		else
		{
			if ((e.AllowedEffect & 4) != 4)
			{
				return;
			}
			e.Effect = (DragDropEffects)2;
		}
		m_ExternalDragInProgress = true;
	}

	protected override void OnDragDrop(DragEventArgs e)
	{
		if (m_DragInProgress)
		{
			Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
			MouseDragDrop(point.X, point.Y, null);
		}
		else if (m_ExternalDragInProgress)
		{
			Point point2 = ((Control)this).PointToClient(new Point(e.X, e.Y));
			MouseDragDrop(point2.X, point2.Y, e);
		}
		((Control)this).OnDragDrop(e);
	}

	protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if (m_DragInProgress && ((m_DragLeft && (int)e.Action == 1) || (int)e.Action == 2))
		{
			MouseDragDrop(-1, -1, null);
		}
		((Control)this).OnQueryContinueDrag(e);
	}

	private void MouseDragOver(int x, int y, DragEventArgs dragArgs)
	{
		if (!m_DragInProgress && !m_ExternalDragInProgress)
		{
			return;
		}
		BaseItem baseItem = m_DragItem;
		if (m_ExternalDragInProgress && dragArgs != null)
		{
			baseItem = dragArgs.Data.GetData(typeof(ButtonItem)) as BaseItem;
		}
		if (m_DesignTimeProvider != null)
		{
			m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			m_DesignTimeProvider = null;
		}
		if (m_ExternalDragInProgress && baseItem == null)
		{
			return;
		}
		Point pScreen = ((Control)this).PointToScreen(new Point(x, y));
		foreach (SideBarPanelItem subItem in m_ItemContainer.SubItems)
		{
			if (!subItem.Visible)
			{
				continue;
			}
			InsertPosition insertPosition = ((IDesignTimeProvider)subItem).GetInsertPosition(pScreen, baseItem);
			if (insertPosition != null)
			{
				if (insertPosition.TargetProvider == null)
				{
					if (!m_UseNativeDragDrop)
					{
						if (m_NACursor != (Cursor)null)
						{
							Cursor.Current = m_NACursor;
						}
						else
						{
							Cursor.Current = Cursors.No;
						}
					}
					break;
				}
				insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
				m_InsertPosition = insertPosition.Position;
				m_InsertBefore = insertPosition.Before;
				m_DesignTimeProvider = insertPosition.TargetProvider;
				if (!m_UseNativeDragDrop)
				{
					if (m_MoveCursor != (Cursor)null)
					{
						Cursor.Current = m_MoveCursor;
					}
					else
					{
						Cursor.Current = Cursors.Hand;
					}
				}
				else if (dragArgs != null)
				{
					dragArgs.Effect = (DragDropEffects)2;
				}
				break;
			}
			if (!m_UseNativeDragDrop)
			{
				if (m_NACursor != (Cursor)null)
				{
					Cursor.Current = m_NACursor;
				}
				else
				{
					Cursor.Current = Cursors.No;
				}
			}
			else if (dragArgs != null)
			{
				dragArgs.Effect = (DragDropEffects)0;
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (!m_UseNativeDragDrop)
		{
			MouseDragDrop(e.X, e.Y, null);
		}
		m_ItemContainer.InternalMouseUp(e);
	}

	private void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		if (!m_DragInProgress && !m_ExternalDragInProgress)
		{
			return;
		}
		BaseItem baseItem = m_DragItem;
		if (m_ExternalDragInProgress)
		{
			baseItem = dragArgs.Data.GetData(typeof(ButtonItem)) as BaseItem;
		}
		baseItem?.InternalMouseLeave();
		if (m_DesignTimeProvider != null)
		{
			if (x == -1 && y == -1)
			{
				m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			}
			else
			{
				m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
				if (baseItem != null)
				{
					BaseItem parent = baseItem.Parent;
					if (parent != null)
					{
						if (parent == (BaseItem)m_DesignTimeProvider && m_InsertPosition > 0 && parent.SubItems.IndexOf(baseItem) < m_InsertPosition)
						{
							m_InsertPosition--;
						}
						parent.SubItems.Remove(baseItem);
						object containerControl = parent.ContainerControl;
						Control val = (Control)((containerControl is Control) ? containerControl : null);
						parent.Refresh();
					}
					m_DesignTimeProvider.InsertItemAt(baseItem, m_InsertPosition, m_InsertBefore);
					m_DesignTimeProvider = null;
					((IOwner)this).InvokeUserCustomize((object)baseItem, new EventArgs());
				}
			}
		}
		m_DesignTimeProvider = null;
		m_DragInProgress = false;
		m_ExternalDragInProgress = false;
		if (!m_UseNativeDragDrop)
		{
			Cursor.Current = Cursors.Default;
		}
		((Control)this).Capture = false;
		if (baseItem != null)
		{
			baseItem._IgnoreClick = true;
		}
		m_ItemContainer.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, x, y, 0));
		if (baseItem != null)
		{
			baseItem._IgnoreClick = false;
		}
		m_DragItem = null;
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (m_RenderMode == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
		{
			return m_Renderer;
		}
		if (m_DefaultRenderer == null)
		{
			m_DefaultRenderer = new Office2007Renderer();
		}
		return m_Renderer;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		if (m_ItemContainer == null || ((Control)this).IsDisposed)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		graphics.PageUnit = (GraphicsUnit)2;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (!BarFunctions.IsOffice2007Style(Style))
		{
			BarFunctions.DrawBorder(graphics, m_BorderStyle, clientRectangle, SystemColors.ControlText);
		}
		ColorScheme colorScheme = m_ColorScheme;
		if (!colorScheme.SchemeChanged && BarFunctions.IsOffice2007Style(Style) && GlobalManager.Renderer is Office2007Renderer)
		{
			colorScheme = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		}
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, e.Graphics, colorScheme);
		itemPaintArgs.Renderer = GetRenderer();
		if (BarFunctions.IsOffice2007Style(Style))
		{
			BaseRenderer renderer = itemPaintArgs.Renderer;
			SideBarRendererEventArgs sideBarRendererEventArgs = new SideBarRendererEventArgs(this, graphics);
			sideBarRendererEventArgs.ItemPaintArgs = itemPaintArgs;
			renderer.DrawSideBar(sideBarRendererEventArgs);
		}
		else if (((Control)this).BackColor == SystemColors.Control && IsThemed)
		{
			if (m_BorderStyle == eBorderType.SingleLine)
			{
				clientRectangle.Inflate(-1, -1);
			}
			else if (m_BorderStyle != 0)
			{
				clientRectangle.Inflate(-2, -2);
			}
			ThemeRebar themeRebar = ((IThemeCache)this).ThemeRebar;
			themeRebar.DrawBackground(e.Graphics, ThemeRebarParts.Background, ThemeRebarStates.Normal, clientRectangle);
		}
		else
		{
			if (m_BorderStyle == eBorderType.None)
			{
				SolidBrush val = new SolidBrush(((Control)this).BackColor);
				try
				{
					graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (m_BorderStyle == eBorderType.SingleLine)
			{
				clientRectangle.Inflate(-1, -1);
			}
			else
			{
				clientRectangle.Inflate(-2, -2);
			}
			graphics.FillRectangle((Brush)new SolidBrush(((Control)this).BackColor), clientRectangle);
		}
		m_ItemContainer.Paint(itemPaintArgs);
		if (m_ItemContainer.SubItems.Count == 0 && ((Component)this).DesignMode)
		{
			string text = "Right-click and choose Add New Panel or use Panels collection to create new panels. Right-click to Choose Color Scheme.";
			Rectangle clientRectangle2 = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			TextDrawing.DrawString(graphics, text, ((Control)this).Font, SystemColors.ControlDark, clientRectangle2, format);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return m_ColorScheme.SchemeChanged;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ApplyPredefinedColorScheme(eSideBarColorScheme scheme)
	{
		if (m_ItemContainer == null)
		{
			return;
		}
		ApplyColorScheme(ColorScheme, scheme);
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (subItem is SideBarPanelItem)
			{
				ApplyColorScheme((SideBarPanelItem)subItem, scheme);
			}
		}
		((Control)this).Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTraditional()
	{
		m_Appearance = eSideBarAppearance.Traditional;
		m_ColorScheme.Style = m_ItemContainer.Style;
		m_ItemContainer.Style = m_ItemContainer.Style;
		foreach (SideBarPanelItem panel in Panels)
		{
			panel.ResetHeaderHotStyle();
			panel.ResetHeaderMouseDownStyle();
			panel.ResetHeaderSideHotStyle();
			panel.ResetHeaderSideMouseDownStyle();
			panel.ResetHeaderSideStyle();
			panel.ResetHeaderStyle();
			panel.ResetBackgroundStyle();
		}
		((Control)this).Invalidate();
		RecalcLayout();
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		m_ColorScheme.Refresh(null, bSystemColorEvent: true);
		if (m_ItemContainer != null)
		{
			foreach (SideBarPanelItem subItem in m_ItemContainer.SubItems)
			{
				subItem.RefreshItemStyleSystemColors();
			}
		}
		if (m_UsingSystemColors && Appearance != 0)
		{
			PredefinedColorScheme = eSideBarColorScheme.SystemColors;
			((Control)this).Invalidate(true);
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		((Control)this).WndProc(ref m);
	}

	private void RefreshThemes()
	{
		if (m_ThemeWindow != null)
		{
			m_ThemeWindow.Dispose();
			m_ThemeWindow = new ThemeWindow((Control)(object)this);
		}
		if (m_ThemeRebar != null)
		{
			m_ThemeRebar.Dispose();
			m_ThemeRebar = new ThemeRebar((Control)(object)this);
		}
		if (m_ThemeToolbar != null)
		{
			m_ThemeToolbar.Dispose();
			m_ThemeToolbar = new ThemeToolbar((Control)(object)this);
		}
		if (m_ThemeHeader != null)
		{
			m_ThemeHeader.Dispose();
			m_ThemeHeader = new ThemeHeader((Control)(object)this);
		}
		if (m_ThemeScrollBar != null)
		{
			m_ThemeScrollBar.Dispose();
			m_ThemeScrollBar = new ThemeScrollBar((Control)(object)this);
		}
		if (m_ThemeProgress != null)
		{
			m_ThemeProgress.Dispose();
			m_ThemeProgress = new ThemeProgress((Control)(object)this);
		}
		if (m_ThemeExplorerBar != null)
		{
			m_ThemeExplorerBar.Dispose();
			m_ThemeExplorerBar = new ThemeExplorerBar((Control)(object)this);
		}
		if (m_ThemeButton != null)
		{
			m_ThemeButton.Dispose();
			m_ThemeButton = new ThemeButton((Control)(object)this);
		}
	}

	private void DisposeThemes()
	{
		if (m_ThemeWindow != null)
		{
			m_ThemeWindow.Dispose();
			m_ThemeWindow = null;
		}
		if (m_ThemeRebar != null)
		{
			m_ThemeRebar.Dispose();
			m_ThemeRebar = null;
		}
		if (m_ThemeToolbar != null)
		{
			m_ThemeToolbar.Dispose();
			m_ThemeToolbar = null;
		}
		if (m_ThemeHeader != null)
		{
			m_ThemeHeader.Dispose();
			m_ThemeHeader = null;
		}
		if (m_ThemeScrollBar != null)
		{
			m_ThemeScrollBar.Dispose();
			m_ThemeScrollBar = null;
		}
		if (m_ThemeProgress != null)
		{
			m_ThemeProgress.Dispose();
			m_ThemeProgress = null;
		}
		if (m_ThemeExplorerBar != null)
		{
			m_ThemeExplorerBar.Dispose();
			m_ThemeExplorerBar = null;
		}
		if (m_ThemeButton != null)
		{
			m_ThemeButton.Dispose();
			m_ThemeButton = null;
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DisposeThemes();
		MenuEventSupportUnhook();
		((Control)this).OnHandleDestroyed(e);
		if (m_FilterInstalled)
		{
			MessageHandler.UnregisterMessageClient(this);
			m_FilterInstalled = false;
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			RecalcSize();
		}
	}

	protected virtual void RecalcSize()
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		if (m_BorderStyle == eBorderType.None)
		{
			if (BarFunctions.IsOffice2007Style(Style))
			{
				m_ItemContainer.WidthInternal = ((Control)this).Width - 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - 2;
				m_ItemContainer.LeftInternal = 1;
				m_ItemContainer.TopInternal = 1;
			}
			else if (m_Appearance == eSideBarAppearance.Traditional)
			{
				m_ItemContainer.WidthInternal = ((Control)this).Width - 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - 2;
			}
			else
			{
				m_ItemContainer.WidthInternal = ((Control)this).Width;
				m_ItemContainer.HeightInternal = ((Control)this).Height - 2;
			}
		}
		else
		{
			m_ItemContainer.LeftInternal = 2;
			m_ItemContainer.TopInternal = 2;
			m_ItemContainer.WidthInternal = ((Control)this).Width - 4;
			m_ItemContainer.HeightInternal = ((Control)this).Height - 4;
		}
		m_ItemContainer.RecalcSize();
		OnItemLayoutUpdated(EventArgs.Empty);
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		this.ItemLayoutUpdated?.Invoke(this, e);
	}

	public void RecalcLayout()
	{
		RecalcSize();
		((Control)this).Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		m_ItemContainer.SetDesignMode(b: true);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Component)this).DesignMode)
		{
			SetDesignMode();
		}
		((Control)this).OnHandleCreated(e);
		if (!m_FilterInstalled && !((Component)this).DesignMode)
		{
			MessageHandler.RegisterMessageClient(this);
			m_FilterInstalled = true;
		}
		RecalcLayout();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (m_DelayedExpandedPanel != null)
		{
			if (m_ItemContainer.SubItems.Contains(m_DelayedExpandedPanel))
			{
				m_DelayedExpandedPanel.Expanded = true;
			}
			m_DelayedExpandedPanel = null;
		}
		if (((Control)this).Parent == null || (Images == null && ImagesLarge == null && ImagesMedium == null))
		{
			return;
		}
		foreach (SideBarPanelItem subItem in m_ItemContainer.SubItems)
		{
			foreach (BaseItem subItem2 in subItem.SubItems)
			{
				if (subItem2 is ImageItem)
				{
					((ImageItem)subItem2).OnImageChanged();
				}
			}
		}
	}

	void IOwnerItemEvents.InvokeItemAdded(BaseItem item, EventArgs e)
	{
		if (this.ItemAdded != null)
		{
			this.ItemAdded(item, e);
		}
	}

	void IOwnerItemEvents.InvokeItemRemoved(BaseItem item, BaseItem parent)
	{
		if (this.ItemRemoved != null)
		{
			this.ItemRemoved(item, new ItemRemovedEventArgs(parent));
		}
	}

	void IOwnerItemEvents.InvokeMouseEnter(BaseItem item, EventArgs e)
	{
		if (EventMouseEnter != null)
		{
			EventMouseEnter(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseHover(BaseItem item, EventArgs e)
	{
		if (EventMouseHover != null)
		{
			EventMouseHover(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseLeave(BaseItem item, EventArgs e)
	{
		if (EventMouseLeave != null)
		{
			EventMouseLeave(item, e);
		}
	}

	void IOwnerItemEvents.InvokeMouseDown(BaseItem item, MouseEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (EventMouseDown != null)
		{
			EventMouseDown.Invoke((object)item, e);
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

	void IOwnerItemEvents.InvokeMouseUp(BaseItem item, MouseEventArgs e)
	{
		if (EventMouseUp != null)
		{
			EventMouseUp.Invoke((object)item, e);
		}
		if (m_ClickTimer != null && m_ClickTimer.Enabled)
		{
			m_ClickTimer.Stop();
			m_ClickTimer.Enabled = false;
		}
	}

	void IOwnerItemEvents.InvokeMouseMove(BaseItem item, MouseEventArgs e)
	{
		if (EventMouseMove != null)
		{
			EventMouseMove.Invoke((object)item, e);
		}
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

	void IOwnerItemEvents.InvokeGotFocus(BaseItem item, EventArgs e)
	{
		if (EventGotFocus != null)
		{
			EventGotFocus(item, e);
		}
	}

	void IOwnerItemEvents.InvokeLostFocus(BaseItem item, EventArgs e)
	{
		if (EventLostFocus != null)
		{
			EventLostFocus(item, e);
		}
	}

	void IOwnerItemEvents.InvokeExpandedChange(BaseItem item, EventArgs e)
	{
		if (this.ExpandedChange != null)
		{
			this.ExpandedChange(item, e);
		}
	}

	void IOwnerItemEvents.InvokeItemTextChanged(BaseItem item, EventArgs e)
	{
		if (this.ItemTextChanged != null)
		{
			this.ItemTextChanged(item, e);
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

	void IOwnerItemEvents.InvokeContainerLoadControl(BaseItem item, EventArgs e)
	{
		if (this.ContainerLoadControl != null)
		{
			this.ContainerLoadControl(item, e);
		}
	}

	void IOwnerItemEvents.InvokeOptionGroupChanging(BaseItem item, OptionGroupChangingEventArgs e)
	{
		if (this.OptionGroupChanging != null)
		{
			this.OptionGroupChanging(item, e);
		}
	}

	void IOwnerItemEvents.InvokeToolTipShowing(object item, EventArgs e)
	{
		if (this.ToolTipShowing != null)
		{
			this.ToolTipShowing(item, e);
		}
	}

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (this.ButtonCheckedChanged != null)
		{
			this.ButtonCheckedChanged(item, e);
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

	public ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(m_ItemContainer, ItemName, arrayList);
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(m_ItemContainer, ItemName, arrayList, itemType);
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(m_ItemContainer, ItemName, arrayList, itemType, useGlobalName);
		return arrayList;
	}

	public BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(m_ItemContainer, ItemName);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		return null;
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

	void IOwner.SetFocusItem(BaseItem objFocusItem)
	{
		if (m_FocusItem != null && m_FocusItem != objFocusItem)
		{
			m_FocusItem.OnLostFocus();
		}
		if (objFocusItem == null)
		{
			objFocusItem = null;
		}
		if (((Component)this).DesignMode && Panels.Contains(objFocusItem))
		{
			foreach (BaseItem panel in Panels)
			{
				BaseItem.CollapseSubItems(panel);
			}
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

	void IOwner.DesignTimeContextMenu(BaseItem objItem)
	{
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
					shortcutTableEntry.Items.Add(objItem.Id, objItem);
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

	private void ImageListDisposed(object sender, EventArgs e)
	{
		if (sender == m_ImageList)
		{
			m_ImageList = null;
		}
		else if (sender == m_ImageListLarge)
		{
			m_ImageListLarge = null;
		}
		else if (sender == m_ImageListMedium)
		{
			m_ImageListMedium = null;
		}
	}

	void IOwner.StartItemDrag(BaseItem item)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (m_DragItem != null)
		{
			return;
		}
		m_DragItem = item;
		if (!m_UseNativeDragDrop)
		{
			((Control)this).Capture = true;
			if (m_MoveCursor != (Cursor)null)
			{
				Cursor.Current = m_MoveCursor;
			}
			else
			{
				Cursor.Current = Cursors.Hand;
			}
			m_DragInProgress = true;
		}
		else
		{
			m_DragInProgress = true;
			((Control)this).DoDragDrop((object)item, (DragDropEffects)(-2147483645));
			if (m_DragInProgress)
			{
				MouseDragDrop(-1, -1, null);
			}
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
	}

	MdiClient IOwner.GetMdiClient(Form MdiForm)
	{
		return BarFunctions.GetMdiClient(MdiForm);
	}

	void IOwner.Customize()
	{
	}

	void IOwner.InvokeResetDefinition(BaseItem item, EventArgs e)
	{
	}

	void IOwner.OnApplicationActivate()
	{
	}

	void IOwner.OnApplicationDeactivate()
	{
		ClosePopups();
	}

	void IOwner.OnParentPositionChanging()
	{
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

	internal void SaveDefinition(XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement("sidebar");
		xmlDoc.AppendChild(xmlElement);
		Serialize(xmlElement);
	}

	internal void Serialize(XmlElement xmlThisBar)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected I4, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		xmlThisBar.SetAttribute("name", ((Control)this).Name);
		xmlThisBar.SetAttribute("style", XmlConvert.ToString((int)m_ItemContainer.Style));
		xmlThisBar.SetAttribute("app", XmlConvert.ToString((int)m_Appearance));
		xmlThisBar.SetAttribute("psc", XmlConvert.ToString((int)m_PredefinedColorScheme));
		if (((Control)this).Font != null && (((Control)this).Font.Name != SystemInformation.MenuFont.Name || ((Control)this).Font.Size != SystemInformation.MenuFont.Size || ((Control)this).Font.Style != SystemInformation.MenuFont.Style))
		{
			xmlThisBar.SetAttribute("fontname", ((Control)this).Font.Name);
			xmlThisBar.SetAttribute("fontemsize", XmlConvert.ToString(((Control)this).Font.Size));
			xmlThisBar.SetAttribute("fontstyle", XmlConvert.ToString((int)((Control)this).Font.Style));
		}
		if (((Control)this).BackColor != SystemColors.Control)
		{
			xmlThisBar.SetAttribute("backcolor", BarFunctions.ColorToString(((Control)this).BackColor));
		}
		if (((Control)this).ForeColor != SystemColors.ControlText)
		{
			xmlThisBar.SetAttribute("forecolor", BarFunctions.ColorToString(((Control)this).ForeColor));
		}
		xmlThisBar.SetAttribute("width", XmlConvert.ToString(((Control)this).Width));
		xmlThisBar.SetAttribute("height", XmlConvert.ToString(((Control)this).Height));
		XmlElement xmlElement = xmlThisBar.OwnerDocument.CreateElement("items");
		xmlThisBar.AppendChild(xmlElement);
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (subItem.ShouldSerialize)
			{
				XmlElement xmlElement2 = xmlThisBar.OwnerDocument.CreateElement("item");
				xmlElement.AppendChild(xmlElement2);
				itemSerializationContext.ItemXmlElement = xmlElement2;
				subItem.Serialize(itemSerializationContext);
			}
		}
		if (m_ColorScheme.SchemeChanged)
		{
			XmlElement xmlElement3 = xmlThisBar.OwnerDocument.CreateElement("colorscheme");
			m_ColorScheme.Serialize(xmlElement3);
			xmlThisBar.AppendChild(xmlElement3);
		}
	}

	internal void Deserialize(XmlElement xmlThisBar)
	{
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		((Control)this).Name = xmlThisBar.GetAttribute("name");
		m_ItemContainer.Style = (eDotNetBarStyle)XmlConvert.ToInt32(xmlThisBar.GetAttribute("style"));
		if (xmlThisBar.HasAttribute("backcolor"))
		{
			((Control)this).BackColor = BarFunctions.ColorFromString(xmlThisBar.GetAttribute("backcolor"));
		}
		if (xmlThisBar.HasAttribute("forecolor"))
		{
			((Control)this).ForeColor = BarFunctions.ColorFromString(xmlThisBar.GetAttribute("forecolor"));
		}
		((Control)this).Size = new Size(XmlConvert.ToInt32(xmlThisBar.GetAttribute("width")), XmlConvert.ToInt32(xmlThisBar.GetAttribute("height")));
		if (xmlThisBar.HasAttribute("fontname"))
		{
			string attribute = xmlThisBar.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(xmlThisBar.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(xmlThisBar.GetAttribute("fontstyle"));
			try
			{
				((Control)this).Font = new Font(attribute, num, val);
			}
			catch (Exception)
			{
				((Control)this).Font = SystemFonts.DefaultFont;
			}
		}
		foreach (XmlElement childNode in xmlThisBar.ChildNodes)
		{
			if (childNode.Name == "items")
			{
				foreach (XmlElement childNode2 in childNode.ChildNodes)
				{
					BaseItem baseItem = BarFunctions.CreateItemFromXml(childNode2);
					m_ItemContainer.SubItems.Add(baseItem);
					itemSerializationContext.ItemXmlElement = childNode2;
					baseItem.Deserialize(itemSerializationContext);
				}
			}
			else if (childNode.Name == "colorscheme")
			{
				m_ColorScheme.Deserialize(childNode);
			}
		}
		if (xmlThisBar.HasAttribute("app"))
		{
			Appearance = (eSideBarAppearance)XmlConvert.ToInt32(xmlThisBar.GetAttribute("app"));
		}
		xmlThisBar.SetAttribute("app", XmlConvert.ToString((int)m_Appearance));
		if (xmlThisBar.HasAttribute("psc") && Appearance != 0)
		{
			PredefinedColorScheme = (eSideBarColorScheme)XmlConvert.ToInt32(xmlThisBar.GetAttribute("psc"));
		}
	}

	public void LoadDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		LoadDefinition(xmlDocument);
	}

	internal void LoadDefinition(XmlDocument xmlDoc)
	{
		if (xmlDoc.FirstChild.Name != "sidebar")
		{
			throw new InvalidOperationException("XML Format not recognized");
		}
		m_ItemContainer.SubItems.Clear();
		Deserialize(xmlDoc.FirstChild as XmlElement);
		((IOwner)this)?.AddShortcutsFromItem((BaseItem)m_ItemContainer);
		((IOwner)this).InvokeDefinitionLoaded((object)this, new EventArgs());
		RecalcSize();
	}

	public void SaveDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		SaveDefinition(xmlDocument);
		xmlDocument.Save(FileName);
	}

	void IOwner.InvokeDefinitionLoaded(object sender, EventArgs e)
	{
		if (this.DefinitionLoaded != null)
		{
			this.DefinitionLoaded(sender, e);
		}
	}

	private bool ProcessShortcut(eShortcut key)
	{
		bool flag = BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
		return !m_DispatchShortcuts && flag;
	}

	bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(pt))
		{
			SideBarPanelItem expandedPanel = ExpandedPanel;
			if (expandedPanel != null && expandedPanel.HasScrollButtons)
			{
				expandedPanel.OnMouseWheel(new MouseEventArgs(Control.MouseButtons, 0, pt.X, pt.Y, (wParam.ToInt64() <= 0) ? 1 : (-1)));
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected I4, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
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
			return true;
		}
		if (!IsParentFormActive)
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

	bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (m_RegisteredPopups.Count == 0 || ((Component)this).DesignMode)
		{
			return false;
		}
		for (int num = m_RegisteredPopups.Count - 1; num >= 0; num--)
		{
			PopupItem popupItem = m_RegisteredPopups[num] as PopupItem;
			bool flag = popupItem.IsAnyOnHandle(hWnd);
			if (!flag)
			{
				Control val = Control.FromChildHandle(hWnd);
				if (val != null)
				{
					if (val is MenuPanel)
					{
						flag = true;
					}
					else
					{
						while (val.Parent != null)
						{
							val = val.Parent;
							if (((object)val).GetType().FullName.IndexOf("DropDownHolder") >= 0 || val is MenuPanel || val is PopupContainerControl)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						flag = popupItem.IsAnyOnHandle(val.Handle);
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
				Control val2 = popupItem.PopupControl;
				if (val2 != null)
				{
					while (val2.Parent != null)
					{
						val2 = val2.Parent;
					}
				}
				if (val2 != null && val2.Bounds.Contains(Control.MousePosition))
				{
					flag = true;
				}
			}
			if (flag)
			{
				break;
			}
			if (popupItem.Displayed)
			{
				Point pt = ((Control)this).PointToClient(Control.MousePosition);
				if (popupItem.DisplayRectangle.Contains(pt))
				{
					break;
				}
			}
			popupItem.ClosePopup();
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
			foreach (BaseItem registeredPopup in m_RegisteredPopups)
			{
				if (registeredPopup.Parent == null)
				{
					Control popupControl = ((PopupItem)registeredPopup).PopupControl;
					if (popupControl != null && popupControl.Handle != hWnd && !registeredPopup.IsAnyOnHandle(hWnd) && (popupControl.Parent == null || !(popupControl.Parent.Handle != hWnd)))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		if (!((Component)this).DesignMode)
		{
			int num = WinApi.ToInt(wParam);
			if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
			{
				int key = Control.ModifierKeys | num;
				if (ProcessShortcut((eShortcut)key))
				{
					return true;
				}
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	private void MenuEventSupportHook()
	{
		if (!m_MenuEventSupport)
		{
			m_MenuEventSupport = true;
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				m_MenuEventSupport = false;
				return;
			}
			((Control)val).Resize += ParentResize;
			val.Deactivate += ParentDeactivate;
			DotNetBarManager.RegisterParentMsgHandler(this, val);
		}
	}

	private void MenuEventSupportUnhook()
	{
		if (m_MenuEventSupport)
		{
			m_MenuEventSupport = false;
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				DotNetBarManager.UnRegisterParentMsgHandler(this, val);
				((Control)val).Resize -= ParentResize;
				val.Deactivate -= ParentDeactivate;
			}
		}
	}

	private void ParentResize(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void ParentDeactivate(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
	{
		if (m_RegisteredPopups.Contains(objPopup))
		{
			return;
		}
		if (!((Component)this).DesignMode)
		{
			if (!m_FilterInstalled)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled = true;
			}
			if (!m_MenuEventSupport)
			{
				MenuEventSupportHook();
			}
		}
		else if (m_Hook == null)
		{
			m_Hook = new Hook(this);
		}
		m_RegisteredPopups.Add(objPopup);
		if (objPopup.GetOwner() != this)
		{
			objPopup.SetOwner(this);
		}
	}

	void IOwnerMenuSupport.UnregisterPopup(PopupItem objPopup)
	{
		if (m_RegisteredPopups.Contains(objPopup))
		{
			m_RegisteredPopups.Remove(objPopup);
		}
		if (m_RegisteredPopups.Count == 0)
		{
			MenuEventSupportUnhook();
			if (m_Hook != null)
			{
				m_Hook.Dispose();
				m_Hook = null;
			}
		}
	}

	bool IOwnerMenuSupport.RelayMouseHover()
	{
		foreach (PopupItem registeredPopup in m_RegisteredPopups)
		{
			Control popupControl = registeredPopup.PopupControl;
			if (popupControl != null && popupControl.DisplayRectangle.Contains(Control.MousePosition))
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

	void IOwnerMenuSupport.InvokePopupClose(PopupItem item, EventArgs e)
	{
		if (this.PopupClose != null)
		{
			this.PopupClose(item, e);
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

	void IOwnerMenuSupport.InvokePopupShowing(PopupItem item, EventArgs e)
	{
		if (this.PopupShowing != null)
		{
			this.PopupShowing(item, e);
		}
	}

	public static void ApplyColorScheme(SideBarPanelItem item, eSideBarColorScheme scheme)
	{
		SideBarColors colorScheme = GetColorScheme(scheme);
		colorScheme.Apply(item);
	}

	public static void ApplyColorScheme(ColorScheme cs, eSideBarColorScheme scheme)
	{
		SideBarColors colorScheme = GetColorScheme(scheme);
		colorScheme.Apply(cs);
	}

	private static SideBarColors GetColorScheme(eSideBarColorScheme scheme)
	{
		SideBarColors result = null;
		switch (scheme)
		{
		case eSideBarColorScheme.Brick:
		case eSideBarColorScheme.Wheat:
		case eSideBarColorScheme.Storm:
		case eSideBarColorScheme.Spruce:
		case eSideBarColorScheme.Slate:
		case eSideBarColorScheme.Rose:
		case eSideBarColorScheme.Fire:
		case eSideBarColorScheme.Pumpkin:
		case eSideBarColorScheme.Plum:
		case eSideBarColorScheme.Marine:
		case eSideBarColorScheme.Sunset:
			result = GetFlatColorScheme(scheme);
			break;
		case eSideBarColorScheme.Blue:
		case eSideBarColorScheme.Silver:
		case eSideBarColorScheme.Green:
		case eSideBarColorScheme.Orange:
		case eSideBarColorScheme.Red:
		case eSideBarColorScheme.LightBlue:
		case eSideBarColorScheme.Money:
			result = GetGradientColorScheme(scheme);
			break;
		case eSideBarColorScheme.SystemColors:
			result = GetSystemColorScheme();
			break;
		}
		return result;
	}

	private static SideBarColors GetGradientColorScheme(eSideBarColorScheme scheme)
	{
		Color border = Color.Empty;
		Color color = Color.Empty;
		Color color2 = Color.Empty;
		Color color3 = Color.Empty;
		Color color4 = Color.Empty;
		Color headerHotBackground = Color.Empty;
		Color headerText = Color.Empty;
		Color headerHotText = Color.Empty;
		Color color5 = Color.Empty;
		Color color6 = Color.Empty;
		Color color7 = Color.Empty;
		Color itemHotBackground = Color.Empty;
		Color itemHotBackground2 = Color.Empty;
		Color color8 = Color.Empty;
		Color itemHotText = Color.Empty;
		Color itemPressedBackground = Color.Empty;
		Color itemPressedBackground2 = Color.Empty;
		Color empty = Color.Empty;
		Color empty2 = Color.Empty;
		Color backgroundText = Color.Empty;
		switch (scheme)
		{
		case eSideBarColorScheme.Blue:
			border = Color.FromArgb(59, 97, 156);
			color = Color.FromArgb(232, 232, 232);
			color2 = Color.White;
			color5 = Color.FromArgb(200, 220, 248);
			color6 = Color.FromArgb(94, 137, 207);
			color3 = Color.FromArgb(221, 236, 254);
			color4 = Color.FromArgb(133, 171, 228);
			headerText = Color.FromArgb(0, 51, 102);
			color7 = Color.Black;
			itemHotBackground = Color.FromArgb(255, 244, 204);
			itemHotBackground2 = Color.FromArgb(255, 209, 147);
			color8 = Color.FromArgb(0, 0, 128);
			itemPressedBackground = Color.FromArgb(254, 142, 75);
			itemPressedBackground2 = Color.FromArgb(255, 207, 139);
			empty = Color.FromArgb(255, 213, 140);
			empty2 = Color.FromArgb(255, 173, 85);
			break;
		case eSideBarColorScheme.Silver:
			border = Color.FromArgb(87, 86, 113);
			color = Color.FromArgb(232, 232, 232);
			color2 = Color.White;
			color5 = Color.FromArgb(225, 226, 236);
			color6 = Color.FromArgb(126, 125, 157);
			color3 = Color.FromArgb(243, 244, 250);
			color4 = Color.FromArgb(155, 153, 183);
			headerText = Color.FromArgb(87, 86, 113);
			color7 = Color.Black;
			itemHotBackground = Color.FromArgb(255, 244, 204);
			itemHotBackground2 = Color.FromArgb(255, 209, 147);
			color8 = Color.FromArgb(87, 86, 113);
			itemPressedBackground = Color.FromArgb(254, 142, 75);
			itemPressedBackground2 = Color.FromArgb(255, 207, 139);
			empty = Color.FromArgb(255, 213, 140);
			empty2 = Color.FromArgb(255, 173, 85);
			break;
		case eSideBarColorScheme.Green:
			border = Color.FromArgb(96, 128, 88);
			color = Color.FromArgb(232, 232, 232);
			color2 = Color.White;
			color5 = Color.FromArgb(217, 225, 188);
			color6 = Color.FromArgb(151, 170, 111);
			color3 = Color.FromArgb(244, 247, 222);
			color4 = Color.FromArgb(183, 198, 145);
			headerText = Color.FromArgb(85, 114, 78);
			color7 = Color.Black;
			itemHotBackground = Color.FromArgb(255, 244, 204);
			itemHotBackground2 = Color.FromArgb(255, 209, 147);
			color8 = Color.FromArgb(96, 128, 88);
			itemPressedBackground = Color.FromArgb(254, 142, 75);
			itemPressedBackground2 = Color.FromArgb(255, 207, 139);
			empty = Color.FromArgb(255, 213, 140);
			empty2 = Color.FromArgb(255, 173, 85);
			break;
		case eSideBarColorScheme.Orange:
			border = Color.FromArgb(137, 105, 28);
			color = Color.FromArgb(232, 232, 232);
			color2 = Color.White;
			color5 = Color.FromArgb(249, 225, 164);
			color6 = Color.FromArgb(227, 185, 82);
			color3 = Color.FromArgb(255, 239, 201);
			color4 = Color.FromArgb(242, 210, 132);
			headerText = Color.FromArgb(117, 83, 2);
			color7 = Color.Black;
			itemHotBackground = Color.FromArgb(255, 244, 204);
			itemHotBackground2 = Color.FromArgb(255, 209, 147);
			color8 = Color.FromArgb(137, 105, 28);
			itemPressedBackground = Color.FromArgb(254, 142, 75);
			itemPressedBackground2 = Color.FromArgb(255, 207, 139);
			empty = Color.FromArgb(255, 213, 140);
			empty2 = Color.FromArgb(255, 173, 85);
			break;
		case eSideBarColorScheme.Red:
			border = Color.FromArgb(144, 0, 34);
			color = Color.FromArgb(232, 232, 232);
			color2 = Color.White;
			color5 = Color.FromArgb(255, 174, 193);
			color6 = Color.FromArgb(226, 78, 113);
			color3 = Color.FromArgb(252, 219, 227);
			color4 = Color.FromArgb(254, 150, 174);
			headerText = Color.FromArgb(144, 0, 34);
			color7 = Color.Black;
			itemHotBackground = Color.FromArgb(255, 244, 204);
			itemHotBackground2 = Color.FromArgb(255, 209, 147);
			color8 = Color.FromArgb(144, 0, 34);
			itemPressedBackground = Color.FromArgb(254, 142, 75);
			itemPressedBackground2 = Color.FromArgb(255, 207, 139);
			empty = Color.FromArgb(255, 213, 140);
			empty2 = Color.FromArgb(255, 173, 85);
			break;
		case eSideBarColorScheme.LightBlue:
			border = Color.FromArgb(81, 100, 136);
			color = Color.FromArgb(232, 232, 232);
			color2 = Color.White;
			color5 = Color.FromArgb(226, 235, 253);
			color6 = Color.FromArgb(175, 190, 218);
			color3 = Color.FromArgb(255, 255, 255);
			color4 = Color.FromArgb(210, 224, 252);
			headerText = Color.FromArgb(69, 84, 115);
			color7 = Color.Black;
			itemHotBackground = Color.FromArgb(255, 244, 204);
			itemHotBackground2 = Color.FromArgb(255, 209, 147);
			color8 = Color.FromArgb(81, 100, 136);
			itemPressedBackground = Color.FromArgb(254, 142, 75);
			itemPressedBackground2 = Color.FromArgb(255, 207, 139);
			empty = Color.FromArgb(255, 213, 140);
			empty2 = Color.FromArgb(255, 173, 85);
			break;
		case eSideBarColorScheme.Money:
			border = Color.FromArgb(44, 72, 112);
			color = Color.FromArgb(91, 91, 91);
			color2 = Color.FromArgb(127, 127, 127);
			backgroundText = Color.White;
			color5 = Color.FromArgb(163, 187, 224);
			color6 = Color.FromArgb(99, 131, 177);
			color3 = Color.FromArgb(77, 108, 152);
			color4 = Color.Empty;
			headerHotBackground = Color.FromArgb(55, 85, 128);
			headerText = Color.White;
			color7 = Color.White;
			itemHotText = Color.FromArgb(255, 223, 127);
			headerHotText = Color.FromArgb(255, 223, 127);
			itemHotBackground = Color.FromArgb(80, 80, 80);
			itemHotBackground2 = Color.FromArgb(60, 60, 60);
			color8 = Color.Black;
			itemPressedBackground = Color.FromArgb(110, 110, 110);
			itemPressedBackground2 = Color.FromArgb(80, 80, 80);
			empty = Color.FromArgb(60, 60, 60);
			empty2 = Color.FromArgb(80, 80, 80);
			break;
		}
		SideBarColors sideBarColors = new SideBarColors();
		sideBarColors.Background = color;
		sideBarColors.Background2 = color2;
		sideBarColors.Border = border;
		sideBarColors.HeaderBackground = color3;
		sideBarColors.HeaderBackground2 = color4;
		sideBarColors.HeaderSideBackground = color5;
		sideBarColors.HeaderSideBackground2 = color6;
		sideBarColors.HeaderText = headerText;
		sideBarColors.HeaderHotText = headerHotText;
		if (headerHotBackground.IsEmpty)
		{
			sideBarColors.HeaderHotBackground = color4;
			sideBarColors.HeaderHotBackground2 = color3;
		}
		else
		{
			sideBarColors.HeaderHotBackground = headerHotBackground;
		}
		sideBarColors.HeaderSideHotBackground = color4;
		sideBarColors.HeaderSideHotBackground2 = color3;
		sideBarColors.ItemCheckedBorder = color8;
		sideBarColors.ItemCheckedText = color7;
		if (itemHotText.IsEmpty)
		{
			sideBarColors.ItemHotText = color7;
		}
		else
		{
			sideBarColors.ItemHotText = itemHotText;
		}
		sideBarColors.ItemHotBackground = itemHotBackground;
		sideBarColors.ItemHotBackground2 = itemHotBackground2;
		sideBarColors.ItemHotBorder = color8;
		sideBarColors.ItemPressedText = color7;
		sideBarColors.ItemPressedBackground = itemPressedBackground;
		sideBarColors.ItemPressedBackground2 = itemPressedBackground2;
		sideBarColors.ItemPressedBorder = color8;
		sideBarColors.ItemText = color7;
		sideBarColors.MenuBackground = color;
		sideBarColors.MenuBackground2 = color2;
		sideBarColors.MenuBorder = color8;
		sideBarColors.MenuSide = color5;
		sideBarColors.MenuSide = color6;
		sideBarColors.BackgroundText = backgroundText;
		return sideBarColors;
	}

	private static SideBarColors GetFlatColorScheme(eSideBarColorScheme scheme)
	{
		Color color = Color.Empty;
		Color color2 = Color.Empty;
		Color color3 = Color.Empty;
		Color headerBackground = Color.Empty;
		Color color4 = Color.Empty;
		Color color5 = Color.Empty;
		Color color6 = Color.Empty;
		Color itemHotBorder = Color.Empty;
		Color empty = Color.Empty;
		switch (scheme)
		{
		case eSideBarColorScheme.Brick:
			color = Color.FromArgb(66, 0, 0);
			color2 = Color.White;
			color3 = Color.FromArgb(204, 102, 102);
			headerBackground = Color.FromArgb(227, 220, 198);
			color4 = Color.Black;
			color6 = Color.FromArgb(255, 153, 153);
			itemHotBorder = Color.FromArgb(132, 0, 0);
			color5 = Color.FromArgb(245, 238, 217);
			break;
		case eSideBarColorScheme.Wheat:
			color = Color.FromArgb(132, 130, 0);
			color2 = Color.White;
			color3 = Color.FromArgb(177, 174, 0);
			headerBackground = Color.FromArgb(239, 240, 120);
			color4 = Color.Black;
			color6 = Color.FromArgb(244, 245, 169);
			itemHotBorder = Color.FromArgb(132, 130, 0);
			color5 = Color.FromArgb(254, 255, 178);
			break;
		case eSideBarColorScheme.Storm:
			color = Color.FromArgb(132, 0, 132);
			color2 = Color.White;
			color3 = Color.FromArgb(162, 78, 162);
			headerBackground = Color.FromArgb(226, 189, 226);
			color4 = Color.Black;
			color6 = Color.FromArgb(226, 189, 226);
			itemHotBorder = Color.FromArgb(132, 0, 132);
			color5 = Color.FromArgb(244, 223, 244);
			break;
		case eSideBarColorScheme.Spruce:
			color = Color.FromArgb(51, 102, 51);
			color2 = Color.White;
			color3 = Color.FromArgb(90, 150, 99);
			headerBackground = Color.FromArgb(165, 228, 165);
			color4 = Color.Black;
			color6 = Color.FromArgb(204, 255, 204);
			itemHotBorder = Color.FromArgb(51, 102, 51);
			color5 = Color.FromArgb(204, 231, 204);
			break;
		case eSideBarColorScheme.Slate:
			color = Color.FromArgb(34, 82, 127);
			color2 = Color.White;
			color3 = Color.FromArgb(123, 167, 184);
			headerBackground = Color.FromArgb(186, 204, 216);
			color4 = Color.Black;
			color6 = Color.FromArgb(129, 191, 232);
			itemHotBorder = Color.FromArgb(34, 82, 127);
			color5 = Color.FromArgb(232, 232, 232);
			break;
		case eSideBarColorScheme.Rose:
			color = Color.FromArgb(102, 45, 63);
			color2 = Color.White;
			color3 = Color.FromArgb(182, 100, 125);
			headerBackground = Color.FromArgb(206, 174, 181);
			color4 = Color.Black;
			color6 = Color.FromArgb(241, 163, 180);
			itemHotBorder = Color.FromArgb(102, 45, 63);
			color5 = Color.FromArgb(242, 200, 209);
			break;
		case eSideBarColorScheme.Fire:
			color = Color.FromArgb(92, 0, 0);
			color2 = Color.White;
			color3 = Color.FromArgb(175, 32, 32);
			headerBackground = Color.FromArgb(198, 198, 198);
			color4 = Color.Black;
			color6 = Color.FromArgb(242, 84, 84);
			itemHotBorder = Color.FromArgb(92, 0, 0);
			color5 = Color.FromArgb(255, 142, 142);
			break;
		case eSideBarColorScheme.Pumpkin:
			color = Color.FromArgb(123, 96, 27);
			color2 = Color.White;
			color3 = Color.FromArgb(214, 166, 41);
			headerBackground = Color.FromArgb(239, 215, 156);
			color4 = Color.Black;
			color6 = Color.FromArgb(239, 191, 97);
			itemHotBorder = Color.FromArgb(123, 96, 27);
			color5 = Color.FromArgb(255, 239, 198);
			break;
		case eSideBarColorScheme.Plum:
			color = Color.FromArgb(74, 65, 99);
			color2 = Color.White;
			color3 = Color.FromArgb(119, 106, 154);
			headerBackground = Color.FromArgb(173, 154, 148);
			color4 = Color.Black;
			color6 = Color.FromArgb(205, 188, 182);
			itemHotBorder = Color.FromArgb(74, 65, 99);
			color5 = Color.FromArgb(219, 195, 188);
			break;
		case eSideBarColorScheme.Marine:
			color = Color.FromArgb(0, 0, 132);
			color2 = Color.White;
			color3 = Color.FromArgb(83, 168, 159);
			headerBackground = Color.FromArgb(154, 214, 207);
			color4 = Color.Black;
			color6 = Color.FromArgb(79, 198, 185);
			itemHotBorder = Color.FromArgb(0, 0, 132);
			color5 = Color.FromArgb(188, 231, 226);
			break;
		case eSideBarColorScheme.Sunset:
			color = Color.FromArgb(176, 87, 0);
			color2 = Color.White;
			color3 = Color.FromArgb(219, 155, 0);
			headerBackground = Color.FromArgb(255, 212, 110);
			color4 = Color.Black;
			color6 = Color.FromArgb(247, 193, 77);
			itemHotBorder = Color.FromArgb(176, 87, 0);
			color5 = Color.FromArgb(253, 226, 162);
			break;
		}
		empty = ControlPaint.Light(color6);
		SideBarColors sideBarColors = new SideBarColors();
		sideBarColors.Background = color2;
		sideBarColors.Border = color;
		sideBarColors.HeaderBackground = headerBackground;
		sideBarColors.HeaderSideBackground = color3;
		sideBarColors.HeaderHotBackground = color5;
		sideBarColors.HeaderSideHotBackground = color5;
		sideBarColors.HeaderText = color4;
		sideBarColors.ItemCheckedBorder = color;
		sideBarColors.ItemCheckedText = color4;
		sideBarColors.ItemHotText = color4;
		sideBarColors.ItemHotBackground = color6;
		sideBarColors.ItemHotBorder = itemHotBorder;
		sideBarColors.ItemPressedText = color4;
		sideBarColors.ItemPressedBackground = empty;
		sideBarColors.ItemPressedBorder = color;
		sideBarColors.ItemText = color4;
		sideBarColors.MenuBackground = color2;
		sideBarColors.MenuBorder = color;
		sideBarColors.MenuSide = color3;
		return sideBarColors;
	}

	private static SideBarColors GetSystemColorScheme()
	{
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		SideBarColors sideBarColors = new SideBarColors();
		sideBarColors.Background = colorScheme.MenuBackground;
		sideBarColors.Background2 = ControlPaint.LightLight(colorScheme.MenuBackground);
		sideBarColors.Border = colorScheme.BarPopupBorder;
		sideBarColors.HeaderBackground = colorScheme.MenuSide;
		sideBarColors.HeaderBackground2 = colorScheme.MenuSide2;
		sideBarColors.HeaderSideBackground = colorScheme.BarBackground;
		sideBarColors.HeaderSideBackground2 = colorScheme.BarBackground2;
		sideBarColors.HeaderText = colorScheme.ItemText;
		sideBarColors.HeaderHotText = colorScheme.ItemHotText;
		sideBarColors.HeaderHotBackground = sideBarColors.HeaderBackground2;
		sideBarColors.HeaderHotBackground2 = sideBarColors.HeaderBackground;
		sideBarColors.HeaderSideHotBackground = sideBarColors.HeaderHotBackground;
		sideBarColors.HeaderSideHotBackground2 = sideBarColors.HeaderHotBackground2;
		sideBarColors.ItemCheckedBorder = colorScheme.ItemCheckedBorder;
		sideBarColors.ItemCheckedText = colorScheme.ItemCheckedText;
		sideBarColors.ItemHotText = colorScheme.ItemHotText;
		sideBarColors.ItemHotBackground = colorScheme.ItemHotBackground;
		sideBarColors.ItemHotBackground2 = colorScheme.ItemHotBackground2;
		sideBarColors.ItemHotBorder = colorScheme.ItemHotBorder;
		sideBarColors.ItemPressedText = colorScheme.ItemPressedText;
		sideBarColors.ItemPressedBackground = colorScheme.ItemPressedBackground;
		sideBarColors.ItemPressedBackground2 = colorScheme.ItemPressedBackground2;
		sideBarColors.ItemPressedBorder = colorScheme.ItemPressedBorder;
		sideBarColors.ItemText = colorScheme.ItemText;
		sideBarColors.MenuBackground = colorScheme.MenuBackground;
		sideBarColors.MenuBackground2 = colorScheme.MenuBackground2;
		sideBarColors.MenuBorder = colorScheme.MenuBorder;
		sideBarColors.MenuSide = colorScheme.MenuSide;
		sideBarColors.MenuSide2 = colorScheme.MenuSide2;
		sideBarColors.BackgroundText = colorScheme.ItemText;
		return sideBarColors;
	}
}
