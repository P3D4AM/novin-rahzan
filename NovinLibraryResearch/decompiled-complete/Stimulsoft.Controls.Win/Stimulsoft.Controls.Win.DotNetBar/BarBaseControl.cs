using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public abstract class BarBaseControl : Control, IOwner, IOwnerMenuSupport, IMessageHandlerClient, IOwnerItemEvents, IThemeCache, IOwnerLocalize, IBarDesignerServices, IRenderingSupport, IAccessibilitySupport
{
	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

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

	private BaseItem m_BaseItemContainer;

	private BaseItem m_ExpandedItem;

	private BaseItem m_FocusItem;

	private Hashtable m_ShortcutTable = new Hashtable();

	private ImageList m_ImageList;

	private ImageList m_ImageListMedium;

	private ImageList m_ImageListLarge;

	private BaseItem m_DragItem;

	private bool m_DragDropSupport;

	private bool m_DragLeft;

	private bool m_AllowExternalDrop;

	private bool m_UseNativeDragDrop;

	private bool m_DragInProgress;

	private bool m_ExternalDragInProgress;

	private Cursor m_MoveCursor;

	private Cursor m_CopyCursor;

	private Cursor m_NACursor;

	private bool m_ShowToolTips = true;

	private bool m_ShowShortcutKeysInToolTips;

	private bool m_FilterInstalled;

	private bool m_DispatchShortcuts;

	private bool m_MenuEventSupport;

	private IDesignTimeProvider m_DesignTimeProvider;

	private int m_InsertPosition;

	private bool m_InsertBefore;

	private Timer m_ClickTimer;

	private BaseItem m_ClickRepeatItem;

	private ThemeWindow m_ThemeWindow;

	private ThemeRebar m_ThemeRebar;

	private ThemeToolbar m_ThemeToolbar;

	private ThemeHeader m_ThemeHeader;

	private ThemeScrollBar m_ThemeScrollBar;

	private ThemeExplorerBar m_ThemeExplorerBar;

	private ThemeProgress m_ThemeProgress;

	private ThemeButton m_ThemeButton;

	private ColorScheme m_ColorScheme;

	private bool m_ThemeAware;

	private string m_EmptyContainerDesignTimeHint = "Right-click to add more items...";

	private ItemStyle m_BackgroundStyle = new ItemStyle();

	private bool m_DesignModeInternal;

	private IBarItemDesigner m_BarDesigner;

	private BaseItem m_DoDefaultActionItem;

	private bool m_AntiAlias;

	private Hook m_Hook;

	private ArrayList m_RegisteredPopups = new ArrayList();

	private bool m_DisabledImagesGrayScale = true;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

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

	bool IOwner.DesignMode => GetDesignMode();

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

	bool IOwner.DragInProgress => m_DragInProgress;

	BaseItem IOwner.DragItem => m_DragItem;

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

	protected bool IsParentFormActive
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

	BaseItem IAccessibilitySupport.DoDefaultActionItem
	{
		get
		{
			return m_DoDefaultActionItem;
		}
		set
		{
			m_DoDefaultActionItem = value;
		}
	}

	protected virtual string EmptyContainerDesignTimeHint
	{
		get
		{
			return m_EmptyContainerDesignTimeHint;
		}
		set
		{
			m_EmptyContainerDesignTimeHint = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle BackgroundStyle => m_BackgroundStyle;

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

	protected bool IsThemed
	{
		get
		{
			if (m_ThemeAware && m_BaseItemContainer.EffectiveStyle != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && Themes.ThemesActive)
			{
				return true;
			}
			return false;
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
			m_BaseItemContainer.ThemeAware = value;
			if (GetDesignMode())
			{
				((Control)this).Refresh();
			}
		}
	}

	[DefaultValue(false)]
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
			if (m_AntiAlias != value)
			{
				m_AntiAlias = value;
				((Control)this).Invalidate();
			}
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

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

	public event OptionGroupChangingEventHandler OptionGroupChanging;

	public event EventHandler ToolTipShowing;

	internal event EventHandler ApplicationActivate;

	internal event EventHandler ApplicationDeactivate;

	internal event HandlerMessageEventHandler ApplicationMouseDown;

	public event EventHandler ItemLayoutUpdated;

	public BarBaseControl()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
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
		m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		m_BackgroundStyle.BackColor1.Color = SystemColors.Control;
		m_BackgroundStyle.VisualPropertyChanged += VisualPropertyChanged;
		((Control)this).IsAccessible = true;
	}

	protected bool GetDesignMode()
	{
		if (!m_DesignModeInternal)
		{
			return ((Component)this).DesignMode;
		}
		return m_DesignModeInternal;
	}

	internal void SetDesignMode(bool mode)
	{
		m_DesignModeInternal = mode;
		m_BaseItemContainer.SetDesignMode(mode);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new BarBaseControlAccessibleObject(this);
	}

	internal void InternalAccessibilityNotifyClients(AccessibleEvents accEvent, int childID)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).AccessibilityNotifyClients(accEvent, childID);
	}

	public virtual ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(m_BaseItemContainer, ItemName, arrayList);
		return arrayList;
	}

	public virtual ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(m_BaseItemContainer, ItemName, arrayList, itemType);
		return arrayList;
	}

	public virtual ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(m_BaseItemContainer, ItemName, arrayList, itemType, useGlobalName);
		return arrayList;
	}

	public virtual BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(m_BaseItemContainer, ItemName);
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
		OnSetFocusItem(objFocusItem);
		m_FocusItem = objFocusItem;
		if (m_FocusItem != null)
		{
			m_FocusItem.OnGotFocus();
		}
	}

	protected virtual void OnSetFocusItem(BaseItem objFocusItem)
	{
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

	void IOwner.Customize()
	{
	}

	void IOwner.InvokeResetDefinition(BaseItem item, EventArgs e)
	{
	}

	void IOwner.OnApplicationActivate()
	{
		if (this.ApplicationActivate != null)
		{
			this.ApplicationActivate(this, new EventArgs());
		}
	}

	void IOwner.OnApplicationDeactivate()
	{
		ClosePopups();
		if (this.ApplicationDeactivate != null)
		{
			this.ApplicationDeactivate(this, new EventArgs());
		}
	}

	void IOwner.OnParentPositionChanging()
	{
	}

	void IOwner.StartItemDrag(BaseItem item)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (!m_DragDropSupport || m_DragItem != null)
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

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (((Control)this).Parent != null && (Images != null || ImagesLarge != null || ImagesMedium != null))
		{
			foreach (BaseItem subItem in m_BaseItemContainer.SubItems)
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
		if (((Component)this).DesignMode)
		{
			m_BaseItemContainer.SetDesignMode(((Component)this).DesignMode);
		}
	}

	void IOwner.InvokeDefinitionLoaded(object sender, EventArgs e)
	{
		if (this.DefinitionLoaded != null)
		{
			this.DefinitionLoaded(sender, e);
		}
	}

	void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
	{
		if (m_RegisteredPopups.Contains(objPopup))
		{
			return;
		}
		if (!GetDesignMode())
		{
			if (!m_FilterInstalled)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled = true;
			}
		}
		else if (m_Hook == null)
		{
			m_Hook = new Hook(this);
		}
		if (!m_MenuEventSupport)
		{
			MenuEventSupportHook();
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

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (this.ButtonCheckedChanged != null)
		{
			this.ButtonCheckedChanged(item, e);
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

	bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
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

	private bool ProcessShortcut(eShortcut key)
	{
		bool flag = BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
		return !m_DispatchShortcuts && flag;
	}

	bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (this.ApplicationMouseDown != null)
		{
			this.ApplicationMouseDown(this, new HandlerMessageEventArgs(hWnd, wParam, lParam));
		}
		if (m_RegisteredPopups.Count == 0)
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
		if (!GetDesignMode())
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

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		else if (((Message)(ref m)).Msg == 1131 && m_DoDefaultActionItem != null)
		{
			m_DoDefaultActionItem.DoAccesibleDefaultAction();
			m_DoDefaultActionItem = null;
		}
		((Control)this).WndProc(ref m);
	}

	protected void RefreshThemes()
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

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (!m_FilterInstalled && !((Component)this).DesignMode)
		{
			MessageHandler.RegisterMessageClient(this);
			m_FilterInstalled = true;
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

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_BaseItemContainer.InternalClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_BaseItemContainer.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		ExKeyDown(e);
		((Control)this).OnKeyDown(e);
	}

	internal void ExKeyDown(KeyEventArgs e)
	{
		m_BaseItemContainer.InternalKeyDown(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		m_BaseItemContainer.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		m_BaseItemContainer.InternalMouseHover();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		m_BaseItemContainer.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (m_DragDropSupport && !m_UseNativeDragDrop)
		{
			MouseDragDrop(e.X, e.Y, null);
		}
		m_BaseItemContainer.InternalMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (m_DragDropSupport && m_DragInProgress)
		{
			if (!m_UseNativeDragDrop)
			{
				MouseDragOver(e.X, e.Y, null);
			}
		}
		else
		{
			m_BaseItemContainer.InternalMouseMove(e);
		}
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		if (m_DragDropSupport && (m_DragInProgress || m_ExternalDragInProgress))
		{
			Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
			MouseDragOver(point.X, point.Y, e);
			m_DragLeft = false;
		}
		((Control)this).OnDragOver(e);
	}

	protected override void OnDragLeave(EventArgs e)
	{
		if (m_DragDropSupport)
		{
			if (m_DragInProgress || m_ExternalDragInProgress)
			{
				MouseDragOver(-1, -1, null);
			}
			m_DragLeft = true;
			m_ExternalDragInProgress = false;
		}
		((Control)this).OnDragLeave(e);
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		((Control)this).OnDragEnter(e);
		if (!m_DragDropSupport || m_DragInProgress || !m_AllowExternalDrop)
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
		if (m_DragDropSupport)
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
		}
		((Control)this).OnDragDrop(e);
	}

	protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		if (m_DragDropSupport && m_DragInProgress && ((m_DragLeft && (int)e.Action == 1) || (int)e.Action == 2))
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
		foreach (SideBarPanelItem subItem in m_BaseItemContainer.SubItems)
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

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
		}
	}

	protected void SetBaseItemContainer(BaseItem item)
	{
		m_BaseItemContainer = item;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public BaseItem GetBaseItemContainer()
	{
		return m_BaseItemContainer;
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (m_BaseItemContainer != null)
		{
			m_BaseItemContainer.UpdateBindings();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && m_ClickTimer != null)
		{
			m_ClickTimer.Stop();
			((Component)(object)m_ClickTimer).Dispose();
			m_ClickTimer = null;
		}
		((Control)this).Dispose(disposing);
	}

	public void ResetBackgroundStyle()
	{
		m_BackgroundStyle.VisualPropertyChanged -= VisualPropertyChanged;
		m_BackgroundStyle = new ItemStyle();
		m_BackgroundStyle.VisualPropertyChanged += VisualPropertyChanged;
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		if (m_BackgroundStyle != null)
		{
			m_BackgroundStyle.ApplyColorScheme(ColorScheme);
		}
		if (GetDesignMode())
		{
			((Control)this).Refresh();
		}
	}

	protected virtual void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
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
		m_BaseItemContainer.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, x, y, 0));
		if (baseItem != null)
		{
			baseItem._IgnoreClick = false;
		}
		m_DragItem = null;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return m_ColorScheme.SchemeChanged;
	}

	public virtual void SetDesignTimeDefaults()
	{
	}

	public Graphics CreateGraphics()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = ((Control)this).CreateGraphics();
		if (m_AntiAlias)
		{
			val.SmoothingMode = (SmoothingMode)4;
			if (!SystemInformation.IsFontSmoothingEnabled)
			{
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
		}
		return val;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(m_BaseItemContainer.EffectiveStyle) && GetRenderer() is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		return m_ColorScheme;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (m_BaseItemContainer != null && !((Control)this).IsDisposed)
		{
			ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, e.Graphics, GetColorScheme());
			itemPaintArgs.Renderer = GetRenderer();
			if (m_AntiAlias)
			{
				itemPaintArgs.Graphics.SmoothingMode = (SmoothingMode)4;
				itemPaintArgs.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			PaintControlBackground(itemPaintArgs);
			itemPaintArgs.Graphics.SetClip(GetItemContainerRectangle());
			PaintItemContainer(itemPaintArgs);
			itemPaintArgs.Graphics.ResetClip();
			if (m_BaseItemContainer.SubItems.Count == 0 && GetDesignMode())
			{
				PaintDesignTimeEmptyHint(itemPaintArgs);
			}
			PaintContentOnTop(itemPaintArgs);
		}
	}

	protected virtual void PaintContentOnTop(ItemPaintArgs pa)
	{
	}

	protected virtual void PaintControlBackground(ItemPaintArgs pa)
	{
		if (m_BackgroundStyle != null)
		{
			m_BackgroundStyle.ApplyColorScheme(pa.Colors);
			m_BackgroundStyle.Paint(pa.Graphics, ((Control)this).ClientRectangle);
		}
	}

	protected virtual void PaintItemContainer(ItemPaintArgs pa)
	{
		m_BaseItemContainer.Paint(pa);
	}

	protected virtual void PaintDesignTimeEmptyHint(ItemPaintArgs pa)
	{
		string emptyContainerDesignTimeHint = m_EmptyContainerDesignTimeHint;
		Rectangle itemContainerRectangle = GetItemContainerRectangle();
		itemContainerRectangle.Inflate(-2, -2);
		if (itemContainerRectangle.Width >= 0 && itemContainerRectangle.Height >= 0)
		{
			eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			TextDrawing.DrawString(pa.Graphics, emptyContainerDesignTimeHint, ((Control)this).Font, SystemColors.ControlDark, itemContainerRectangle, format);
		}
	}

	protected virtual Rectangle GetItemContainerRectangle()
	{
		if (m_BackgroundStyle == null)
		{
			return ((Control)this).ClientRectangle;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (m_BackgroundStyle.Border == eBorderType.SingleLine)
		{
			if (m_BackgroundStyle.BorderSide == eBorderSide.All)
			{
				clientRectangle.Inflate(-1, -1);
			}
			else
			{
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Left) != 0)
				{
					clientRectangle.X++;
				}
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Right) != 0)
				{
					clientRectangle.Width--;
				}
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Top) != 0)
				{
					clientRectangle.Y++;
				}
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Bottom) != 0)
				{
					clientRectangle.Height--;
				}
			}
		}
		else if (m_BackgroundStyle.Border != 0)
		{
			if (m_BackgroundStyle.BorderSide == eBorderSide.All)
			{
				clientRectangle.Inflate(-2, -2);
			}
			else
			{
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Left) != 0)
				{
					clientRectangle.X += 2;
				}
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Right) != 0)
				{
					clientRectangle.Width -= 2;
				}
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Top) != 0)
				{
					clientRectangle.Y += 2;
				}
				if ((m_BackgroundStyle.BorderSide & eBorderSide.Bottom) != 0)
				{
					clientRectangle.Height -= 2;
				}
			}
		}
		return clientRectangle;
	}

	protected virtual void RecalcSize()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (!m_BaseItemContainer.IsRecalculatingSize)
		{
			Rectangle itemContainerRectangle = GetItemContainerRectangle();
			m_BaseItemContainer.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
			m_BaseItemContainer.LeftInternal = itemContainerRectangle.X;
			m_BaseItemContainer.TopInternal = itemContainerRectangle.Y;
			m_BaseItemContainer.WidthInternal = itemContainerRectangle.Width;
			m_BaseItemContainer.HeightInternal = itemContainerRectangle.Height;
			m_BaseItemContainer.RecalcSize();
			OnItemLayoutUpdated(EventArgs.Empty);
		}
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		this.ItemLayoutUpdated?.Invoke(this, e);
	}

	public virtual void RecalcLayout()
	{
		if (!m_BaseItemContainer.IsRecalculatingSize)
		{
			RecalcSize();
			((Control)this).Invalidate();
		}
	}
}
