using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Events;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public abstract class PopupItem : ImageItem, IDesignTimeProvider
{
	private MenuPanel m_PopupMenu;

	private Bar m_PopupBar;

	private PopupContainerControl m_PopupContainer;

	private bool m_FilterInstalled;

	protected SideBarImage m_SideBar;

	private ePopupType m_PopupType;

	private bool m_WasParentFocused;

	private Size m_OldSubItemsImageSize;

	private int m_PopupWidth = 200;

	private ePersonalizedMenus m_PersonalizedMenus;

	private Point m_PopupLocation = Point.Empty;

	private ePopupAnimation m_PopupAnimation = ePopupAnimation.ManagerControlled;

	private Font m_PopupFont;

	private Control m_SourceControl;

	private ePopupSide m_PopupSide;

	private bool m_PopupPositionAdjusted;

	private bool _VerifyPopupScreenPosition = true;

	private static readonly Point DefaultPopupOffset = Point.Empty;

	private Point _PopupOffset = DefaultPopupOffset;

	protected virtual bool IsRightHanded
	{
		get
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Invalid comparison between Unknown and I4
			if (NativeFunctions.RightHandedMenus)
			{
				return true;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				return (int)val.RightToLeft == 1;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size PopupSize
	{
		get
		{
			if (SubItemsImageSize.Width == ImageItem._InitalImageSize.Width && SubItemsImageSize.Height == ImageItem._InitalImageSize.Height)
			{
				m_OldSubItemsImageSize = SubItemsImageSize;
				SubItemsImageSize = new Size((int)Math.Ceiling(16.0 * StiScale2.X), (int)Math.Ceiling(16.0 * StiScale2.Y));
			}
			if (m_PopupType == ePopupType.Container)
			{
				CreatePopupContainer(fireLoadEvents: true);
				if (m_PopupContainer != null)
				{
					return ((Control)m_PopupContainer).Size;
				}
				return new Size(m_PopupWidth, 150);
			}
			foreach (BaseItem subItem in SubItems)
			{
				subItem.Orientation = eOrientation.Horizontal;
			}
			MenuPanel menuPanel = new MenuPanel();
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
			menuPanel.Owner = GetOwner();
			if (m_PopupFont != null)
			{
				((Control)menuPanel).Font = m_PopupFont;
			}
			else if (val != null && val.Font != null)
			{
				((Control)menuPanel).Font = val.Font;
			}
			if (iOwnerMenuSupport != null && iOwnerMenuSupport.AlwaysShowFullMenus)
			{
				menuPanel.PersonalizedMenus = ePersonalizedMenus.Disabled;
			}
			else
			{
				menuPanel.PersonalizedMenus = m_PersonalizedMenus;
			}
			if (iOwnerMenuSupport != null && !iOwnerMenuSupport.ShowFullMenusOnHover && (menuPanel.PersonalizedMenus == ePersonalizedMenus.DisplayOnHover || menuPanel.PersonalizedMenus == ePersonalizedMenus.Both))
			{
				menuPanel.PersonalizedMenus = ePersonalizedMenus.DisplayOnClick;
			}
			menuPanel.PopupAnimation = m_PopupAnimation;
			if (iOwnerMenuSupport != null && (IsOnMenuBar || base.IsOnMenu))
			{
				menuPanel.PersonalizedAllVisible = iOwnerMenuSupport.PersonalizedAllVisible;
			}
			if (Parent is CustomizeItem || (this is CustomizeItem && !(this is QatCustomizeItem)))
			{
				menuPanel.IsCustomizeMenu = true;
			}
			if (GetOwner() is DotNetBarManager && ((DotNetBarManager)GetOwner()).UseGlobalColorScheme)
			{
				menuPanel.ColorScheme = ((DotNetBarManager)GetOwner()).ColorScheme;
			}
			if (val is Bar)
			{
				menuPanel.ColorScheme = ((Bar)(object)val).ColorScheme;
			}
			else if (val is MenuPanel)
			{
				menuPanel.ColorScheme = ((MenuPanel)(object)val).ColorScheme;
			}
			else if (val is PopupItemControl)
			{
				menuPanel.ColorScheme = ((PopupItemControl)(object)val).GetColorScheme();
			}
			else if (BarFunctions.IsOffice2007Style(EffectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
			{
				menuPanel.ColorScheme = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			else
			{
				menuPanel.ColorScheme = new ColorScheme(EffectiveStyle);
			}
			menuPanel.SideBar = m_SideBar;
			menuPanel.ParentItem = this;
			((Control)menuPanel).CreateControl();
			menuPanel.RecalcSize();
			Size size = ((Control)menuPanel).Size;
			((Component)(object)menuPanel).Dispose();
			menuPanel = null;
			return size;
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool VerifyPopupScreenPosition
	{
		get
		{
			return _VerifyPopupScreenPosition;
		}
		set
		{
			_VerifyPopupScreenPosition = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
	public bool LockClosePopup { get; set; }

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether sub-items are shown on popup Bar or popup menu.")]
	[DefaultValue(ePopupType.Menu)]
	public virtual ePopupType PopupType
	{
		get
		{
			return m_PopupType;
		}
		set
		{
			if (m_PopupType != value)
			{
				m_PopupType = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "PopupType");
				}
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the font that will be used on the popup window.")]
	[DefaultValue(null)]
	public virtual Font PopupFont
	{
		get
		{
			return m_PopupFont;
		}
		set
		{
			if (m_PopupFont != value)
			{
				m_PopupFont = value;
			}
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Indicates side bar for pop-up window.")]
	public virtual SideBarImage PopUpSideBar
	{
		get
		{
			return m_SideBar;
		}
		set
		{
			m_SideBar = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(ePopupSide.Default)]
	[Description("Indicates location of popup in relation to it's parent.")]
	public virtual ePopupSide PopupSide
	{
		get
		{
			return m_PopupSide;
		}
		set
		{
			m_PopupSide = value;
			OnAppearanceChanged();
		}
	}

	[Category("Behavior")]
	[Description("Indicates popup offset.")]
	public Point PopupOffset
	{
		get
		{
			return _PopupOffset;
		}
		set
		{
			_PopupOffset = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point PopupLocation
	{
		get
		{
			if (m_PopupMenu != null)
			{
				return ((Control)m_PopupMenu).Location;
			}
			if (m_PopupBar != null)
			{
				return m_PopupBar.Location;
			}
			if (m_PopupContainer != null)
			{
				return ((Control)m_PopupContainer).Location;
			}
			return m_PopupLocation;
		}
		set
		{
			m_PopupLocation = value;
			if (m_PopupMenu != null)
			{
				((Control)m_PopupMenu).Location = value;
			}
			else if (m_PopupBar != null)
			{
				m_PopupBar.Location = value;
			}
			else if (m_PopupContainer != null)
			{
				((Control)m_PopupContainer).Location = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	[DefaultValue(ePersonalizedMenus.Disabled)]
	public virtual ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return m_PersonalizedMenus;
		}
		set
		{
			m_PersonalizedMenus = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates Animation type for Popups.")]
	[DefaultValue(ePopupAnimation.ManagerControlled)]
	public virtual ePopupAnimation PopupAnimation
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

	internal bool FilterInstalled
	{
		get
		{
			return m_FilterInstalled;
		}
		set
		{
			m_FilterInstalled = true;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.")]
	[DefaultValue(200)]
	public virtual int PopupWidth
	{
		get
		{
			return m_PopupWidth;
		}
		set
		{
			if (m_PopupWidth != value && value > 0)
			{
				m_PopupWidth = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "PopupWidth");
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Description("Gets the control that is displaying the context menu.")]
	public Control SourceControl => m_SourceControl;

	[Browsable(false)]
	public PopupContainerControl PopupContainerControl => m_PopupContainer;

	[Browsable(false)]
	public Control PopupControl
	{
		get
		{
			if (m_PopupMenu != null)
			{
				return (Control)(object)m_PopupMenu;
			}
			if (m_PopupBar != null)
			{
				return (Control)(object)m_PopupBar;
			}
			if (m_PopupContainer != null)
			{
				return (Control)(object)m_PopupContainer;
			}
			return null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool PopupPositionAdjusted
	{
		get
		{
			return m_PopupPositionAdjusted;
		}
		set
		{
			m_PopupPositionAdjusted = value;
		}
	}

	public event EventHandler PopupContainerLoad;

	public event EventHandler PopupContainerUnload;

	[Description("Occurs when popup item is about to open.")]
	public event DotNetBarManager.PopupOpenEventHandler PopupOpen;

	[Description("Occurs when popup item is closing.")]
	public event EventHandler PopupClose;

	[Description("Occurs before popup is closed and allows you to cancel closing.")]
	public event CancelableEventSourceHandler QueryPopupClose;

	[Description("Occurs after popup item has been closed.")]
	public event EventHandler PopupFinalized;

	[Description("Occurs just before popup window is shown.")]
	public event EventHandler PopupShowing;

	public PopupItem()
		: this("", "")
	{
	}

	public PopupItem(string sName)
		: this(sName, "")
	{
	}

	public PopupItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		m_PopupMenu = null;
		m_PopupBar = null;
		m_FilterInstalled = false;
		m_PopupType = ePopupType.Menu;
		m_SideBar = default(SideBarImage);
		m_WasParentFocused = false;
		m_OldSubItemsImageSize = Size.Empty;
	}

	protected override void Dispose(bool disposing)
	{
		if (Expanded)
		{
			ClosePopup();
		}
		base.Dispose(disposing);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		base.CopyToItem(copy);
		PopupItem popupItem = copy as PopupItem;
		popupItem.PopupContainerLoad = this.PopupContainerLoad;
		popupItem.PopupContainerUnload = this.PopupContainerUnload;
		popupItem.PersonalizedMenus = PersonalizedMenus;
		popupItem.PopUpSideBar = PopUpSideBar;
		popupItem.PopupAnimation = PopupAnimation;
		popupItem.PopupSide = PopupSide;
		popupItem.PopupWidth = PopupWidth;
		popupItem.PopupClose = this.PopupClose;
		popupItem.PopupContainerLoad = this.PopupContainerLoad;
		popupItem.PopupContainerUnload = this.PopupContainerUnload;
		popupItem.PopupFinalized = this.PopupFinalized;
		popupItem.PopupOpen = this.PopupOpen;
		popupItem.PopupShowing = this.PopupShowing;
		popupItem.PopupType = PopupType;
		popupItem.PopupWidth = PopupWidth;
	}

	protected internal override void OnItemAdded(BaseItem objItem)
	{
		base.OnItemAdded(objItem);
		if (SubItems.Count > 1)
		{
			m_NeedRecalcSize = false;
		}
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected I4, but got Unknown
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("PopupType", XmlConvert.ToString((int)m_PopupType));
		itemXmlElement.SetAttribute("PopupWidth", XmlConvert.ToString(m_PopupWidth));
		itemXmlElement.SetAttribute("PersonalizedMenus", XmlConvert.ToString((int)m_PersonalizedMenus));
		itemXmlElement.SetAttribute("panim", XmlConvert.ToString((int)m_PopupAnimation));
		if (m_PopupFont != null)
		{
			itemXmlElement.SetAttribute("fontname", m_PopupFont.Name);
			itemXmlElement.SetAttribute("fontemsize", XmlConvert.ToString(m_PopupFont.Size));
			itemXmlElement.SetAttribute("fontstyle", XmlConvert.ToString((int)m_PopupFont.Style));
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		m_PopupType = (ePopupType)XmlConvert.ToInt32(itemXmlElement.GetAttribute("PopupType"));
		m_PopupWidth = XmlConvert.ToInt32(itemXmlElement.GetAttribute("PopupWidth"));
		m_PersonalizedMenus = (ePersonalizedMenus)XmlConvert.ToInt32(itemXmlElement.GetAttribute("PersonalizedMenus"));
		if (itemXmlElement.HasAttribute("panim"))
		{
			m_PopupAnimation = (ePopupAnimation)XmlConvert.ToInt32(itemXmlElement.GetAttribute("panim"));
		}
		else
		{
			m_PopupAnimation = ePopupAnimation.SystemDefault;
		}
		m_PopupFont = null;
		if (itemXmlElement.HasAttribute("fontname"))
		{
			string attribute = itemXmlElement.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(itemXmlElement.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("fontstyle"));
			try
			{
				m_PopupFont = new Font(attribute, num, val);
			}
			catch (Exception)
			{
				m_PopupFont = null;
			}
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		if (Expanded)
		{
			if (m_PopupMenu != null)
			{
				if (m_PopupMenu.FocusedItem() != null)
				{
					m_PopupMenu.FocusedItem().InternalKeyDown(objArg);
				}
				if (m_PopupMenu != null)
				{
					m_PopupMenu.ExKeyDown(objArg);
				}
			}
			else if (m_PopupBar != null)
			{
				m_PopupBar.ExKeyDown(objArg);
			}
			else
			{
				_ = m_PopupContainer;
			}
		}
		else
		{
			base.InternalKeyDown(objArg);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		base.InternalMouseMove(objArg);
		if (Expanded && m_PopupMenu != null && !DesignMode)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			MenuPanel popupMenu = m_PopupMenu;
			if (val != null)
			{
				Point point = val.PointToScreen(new Point(objArg.X, objArg.Y));
				if (popupMenu != null && !((Control)popupMenu).IsDisposed)
				{
					point = ((Control)popupMenu).PointToClient(point);
					popupMenu.InternalMouseMove(new MouseEventArgs(objArg.Button, objArg.Clicks, point.X, point.Y, objArg.Delta));
				}
			}
		}
		else
		{
			if (!Expanded || m_PopupBar == null || DesignMode)
			{
				return;
			}
			object containerControl2 = ContainerControl;
			Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			Bar popupBar = m_PopupBar;
			if (val2 != null)
			{
				Point point2 = val2.PointToScreen(new Point(objArg.X, objArg.Y));
				if (popupBar != null && !((Control)popupBar).IsDisposed)
				{
					point2 = ((Control)popupBar).PointToClient(point2);
					popupBar.InternalMouseMove(new MouseEventArgs((MouseButtons)0, objArg.Clicks, point2.X, point2.Y, objArg.Delta));
				}
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		base.InternalMouseUp(objArg);
		if (Expanded && m_PopupMenu != null && !DesignMode)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Point point = val.PointToScreen(new Point(objArg.X, objArg.Y));
				point = ((Control)m_PopupMenu).PointToClient(point);
				m_PopupMenu.InternalMouseUp(new MouseEventArgs(objArg.Button, objArg.Clicks, point.X, point.Y, objArg.Delta));
			}
		}
		else if (Expanded && m_PopupBar != null && !DesignMode)
		{
			object containerControl2 = ContainerControl;
			Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			if (val2 != null)
			{
				Point point2 = val2.PointToScreen(new Point(objArg.X, objArg.Y));
				point2 = ((Control)m_PopupBar).PointToClient(point2);
				m_PopupBar.InternalMouseUp(new MouseEventArgs(objArg.Button, objArg.Clicks, point2.X, point2.Y, objArg.Delta));
			}
		}
	}

	protected override void OnSubItemGotFocus(BaseItem objItem)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is PopupItem { Expanded: not false } popupItem && objItem != subItem)
			{
				popupItem.Expanded = false;
			}
		}
		base.OnSubItemGotFocus(objItem);
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		if (Expanded)
		{
			Expanded = false;
			if (m_Parent != null)
			{
				m_Parent.AutoExpand = false;
			}
		}
	}

	public override void SubItemSizeChanged(BaseItem objChildItem)
	{
		if (m_PopupMenu != null)
		{
			m_PopupMenu.RecalcSize();
		}
		else if (m_PopupBar != null)
		{
			m_PopupBar.RecalcLayout();
		}
	}

	protected internal override void OnExpandChange()
	{
		base.OnExpandChange();
		if (!Expanded)
		{
			m_PopupLocation = Point.Empty;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsOnPopup(this) || Parent == null)
		{
			if (GetOwner() is IOwner owner && Name != "syscustomizepopupmenu")
			{
				if (Expanded)
				{
					owner.SetExpandedItem(this);
				}
				else if (owner.GetExpandedItem() == this)
				{
					owner.SetExpandedItem(null);
				}
				IOwner owner2 = null;
			}
			if (val is Bar && !DesignMode)
			{
				if (Expanded)
				{
					m_WasParentFocused = false;
					if (val.Focused)
					{
						m_WasParentFocused = true;
					}
					else if (val is Bar)
					{
						Bar bar = val as Bar;
						if (bar.BarState != eBarState.Floating || bar.MenuBar)
						{
							bar.MenuFocus = true;
						}
					}
					else
					{
						val.Focus();
					}
				}
				else
				{
					if (!m_WasParentFocused)
					{
						((Bar)(object)val).ReleaseFocus();
					}
					Bar bar2 = val as Bar;
					if (bar2.BarState != eBarState.Floating && !bar2.MenuBar)
					{
						bar2.MenuFocus = false;
					}
					m_WasParentFocused = false;
				}
			}
			else if (val is ItemControl && !DesignMode)
			{
				if (Expanded)
				{
					m_WasParentFocused = false;
					if (val.Focused)
					{
						m_WasParentFocused = true;
					}
					else
					{
						ItemControl itemControl = val as ItemControl;
						itemControl.MenuFocus = true;
					}
				}
				else
				{
					if (!m_WasParentFocused)
					{
						((ItemControl)(object)val).ReleaseFocus();
					}
					((ItemControl)(object)val).MenuFocus = false;
					m_WasParentFocused = false;
				}
			}
		}
		if (!Expanded && (m_PopupMenu != null || m_PopupBar != null || m_PopupContainer != null))
		{
			ClosePopup();
		}
		if (val == null)
		{
			return;
		}
		if (BaseItem.IsHandleValid(val))
		{
			Refresh();
		}
		if (Expanded && m_PopupMenu == null && m_PopupBar == null && m_PopupContainer == null && BaseItem.IsHandleValid(val))
		{
			if (m_PopupType == ePopupType.Menu)
			{
				Point point = ((!m_PopupLocation.IsEmpty) ? m_PopupLocation : ((m_PopupSide != 0) ? GetDisplayLocation() : ((Parent is CustomizeItem || this is CustomizeItem) ? ((!IsRightHanded) ? new Point(m_Rect.Right - 2, m_Rect.Top - 2) : new Point(m_Rect.Left + 3, m_Rect.Top)) : (base.IsOnMenu ? ((!IsRightHanded) ? new Point(m_Rect.Right, m_Rect.Top - 2) : new Point(m_Rect.Left + 2, m_Rect.Top - 2)) : ((m_Orientation != 0) ? new Point(m_Rect.Right, m_Rect.Top) : ((!IsRightHanded) ? new Point(m_Rect.Left, m_Rect.Bottom) : new Point(m_Rect.Right, m_Rect.Bottom)))))));
				Point p = val.PointToScreen(point);
				PopupMenu(p);
			}
			else if (m_PopupType == ePopupType.ToolBar)
			{
				Point point2 = ((!m_PopupLocation.IsEmpty) ? m_PopupLocation : ((m_PopupSide != 0) ? GetDisplayLocation() : (base.IsOnMenu ? ((!IsRightHanded) ? new Point(m_Rect.Right, m_Rect.Top - 2) : new Point(m_Rect.Left + 2, m_Rect.Top - 2)) : ((m_Orientation != 0) ? new Point(m_Rect.Right, m_Rect.Top) : ((!IsRightHanded) ? new Point(m_Rect.Left, m_Rect.Bottom) : new Point(m_Rect.Right, m_Rect.Bottom))))));
				Point p2 = val.PointToScreen(point2);
				PopupBar(p2);
			}
			else if (m_PopupType == ePopupType.Container)
			{
				Point point3 = ((!m_PopupLocation.IsEmpty) ? m_PopupLocation : ((m_PopupSide != 0) ? GetDisplayLocation() : (base.IsOnMenu ? ((!IsRightHanded) ? new Point(m_Rect.Right, m_Rect.Top - 2) : new Point(m_Rect.Left + 2, m_Rect.Top - 2)) : ((m_Orientation != 0) ? new Point(m_Rect.Right, m_Rect.Top) : ((!IsRightHanded) ? new Point(m_Rect.Left, m_Rect.Bottom) : new Point(m_Rect.Right, m_Rect.Bottom))))));
				Point p3 = val.PointToScreen(point3);
				PopupContainer(p3);
			}
		}
		val = null;
	}

	protected internal override void OnSubItemExpandChange(BaseItem objItem)
	{
		base.OnSubItemExpandChange(objItem);
		if (!objItem.Expanded)
		{
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Expanded && subItem != objItem)
			{
				subItem.Expanded = false;
			}
		}
	}

	public void PopupMenu(Point p)
	{
		PopupMenu(p.X, p.Y);
	}

	protected internal IOwnerMenuSupport GetIOwnerMenuSupport()
	{
		return GetOwner() as IOwnerMenuSupport;
	}

	protected virtual void OnPopupOpen(PopupOpenEventArgs e)
	{
		if (this.PopupOpen != null)
		{
			this.PopupOpen(this, e);
		}
		if (!e.Cancel)
		{
			GetIOwnerMenuSupport()?.InvokePopupOpen(this, e);
		}
	}

	public virtual void PopupMenu(int x, int y)
	{
		PopupMenu(x, y, _VerifyPopupScreenPosition);
	}

	public virtual void PopupMenu(int x, int y, bool verifyScreenPosition)
	{
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		PopupPositionAdjusted = false;
		IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (iOwnerMenuSupport != null)
		{
			PopupOpenEventArgs popupOpenEventArgs = new PopupOpenEventArgs();
			OnPopupOpen(popupOpenEventArgs);
			if (popupOpenEventArgs.Cancel)
			{
				Expanded = false;
				return;
			}
		}
		if (SubItemsImageSize.Width == ImageItem._InitalImageSize.Width && SubItemsImageSize.Height == ImageItem._InitalImageSize.Height)
		{
			m_OldSubItemsImageSize = SubItemsImageSize;
			SubItemsImageSize = new Size((int)Math.Ceiling(16.0 * StiScale2.X), (int)Math.Ceiling(16.0 * StiScale2.Y));
		}
		foreach (BaseItem subItem in SubItems)
		{
			subItem.Orientation = eOrientation.Horizontal;
		}
		if (m_PopupMenu == null)
		{
			m_PopupMenu = new MenuPanel();
			m_PopupMenu.Owner = GetOwner();
			m_PopupMenu.UseWholeScreenForSizeChecking = !verifyScreenPosition;
			if (m_PopupFont != null)
			{
				((Control)m_PopupMenu).Font = m_PopupFont;
			}
			else if (val != null && val.Font != null)
			{
				if (((object)val).GetType().Name.Contains("MetroTabStrip") && val.Parent != null && val.Parent.Font != null)
				{
					((Control)m_PopupMenu).Font = val.Parent.Font;
				}
				else
				{
					((Control)m_PopupMenu).Font = val.Font;
				}
			}
			if (iOwnerMenuSupport != null && iOwnerMenuSupport.AlwaysShowFullMenus)
			{
				m_PopupMenu.PersonalizedMenus = ePersonalizedMenus.Disabled;
			}
			else
			{
				m_PopupMenu.PersonalizedMenus = m_PersonalizedMenus;
			}
			if (iOwnerMenuSupport != null && !iOwnerMenuSupport.ShowFullMenusOnHover && (m_PopupMenu.PersonalizedMenus == ePersonalizedMenus.DisplayOnHover || m_PopupMenu.PersonalizedMenus == ePersonalizedMenus.Both))
			{
				m_PopupMenu.PersonalizedMenus = ePersonalizedMenus.DisplayOnClick;
			}
			m_PopupMenu.PopupAnimation = m_PopupAnimation;
			if (iOwnerMenuSupport != null && (IsOnMenuBar || base.IsOnMenu))
			{
				m_PopupMenu.PersonalizedAllVisible = iOwnerMenuSupport.PersonalizedAllVisible;
			}
			if (Parent is CustomizeItem || (this is CustomizeItem && !(this is QatCustomizeItem)))
			{
				m_PopupMenu.IsCustomizeMenu = true;
			}
			if (val is Bar)
			{
				Bar bar = (Bar)(object)val;
				m_PopupMenu.ColorScheme = bar.GetColorScheme();
				m_PopupMenu.ShowToolTips = bar.ShowToolTips;
				m_PopupMenu.AntiAlias = bar.AntiAlias;
				if (val is ContextMenuBar)
				{
					m_PopupMenu.UseWholeScreenForSizeChecking = true;
				}
			}
			else if (val is MenuPanel)
			{
				MenuPanel menuPanel = (MenuPanel)(object)val;
				m_PopupMenu.ColorScheme = menuPanel.ColorScheme;
				m_PopupMenu.ShowToolTips = menuPanel.ShowToolTips;
				m_PopupMenu.AntiAlias = menuPanel.AntiAlias;
			}
			else if (val is SideBar)
			{
				m_PopupMenu.ColorScheme = ((SideBar)(object)val).ColorScheme;
			}
			else if (val is ExplorerBar)
			{
				ExplorerBar explorerBar = val as ExplorerBar;
				m_PopupMenu.ColorScheme = explorerBar.ColorScheme;
				m_PopupMenu.AntiAlias = explorerBar.AntiAlias;
			}
			else if (val is BarBaseControl)
			{
				m_PopupMenu.ColorScheme = ((BarBaseControl)(object)val).ColorScheme;
			}
			else if (val is ItemControl)
			{
				ItemControl itemControl = (ItemControl)(object)val;
				m_PopupMenu.ColorScheme = itemControl.GetColorSchemeInternal();
				m_PopupMenu.AntiAlias = itemControl.AntiAlias;
			}
			else if (val is PopupItemControl)
			{
				PopupItemControl popupItemControl = (PopupItemControl)(object)val;
				m_PopupMenu.ColorScheme = popupItemControl.GetColorScheme();
				m_PopupMenu.AntiAlias = popupItemControl.AntiAlias;
			}
			else
			{
				if (Style == eDotNetBarStyle.StyleManagerControlled && GlobalManager.Renderer is Office2007Renderer)
				{
					m_PopupMenu.ColorScheme = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
				}
				else
				{
					m_PopupMenu.ColorScheme = new ColorScheme(EffectiveStyle);
				}
				object owner = GetOwner();
				if (owner is ItemControl)
				{
					m_PopupMenu.AntiAlias = ((ItemControl)owner).AntiAlias;
				}
				else if (owner is Bar)
				{
					m_PopupMenu.AntiAlias = ((Bar)owner).AntiAlias;
				}
			}
			if (val != null)
			{
				((Control)m_PopupMenu).RightToLeft = val.RightToLeft;
			}
			m_PopupMenu.SideBar = m_SideBar;
			m_PopupMenu.ParentItem = this;
			((Control)m_PopupMenu).CreateControl();
			m_PopupMenu.RecalcSize();
		}
		ScreenInformation screenInformation = BarFunctions.ScreenFromPoint(new Point(x, y));
		if (screenInformation == null && BaseItem.IsHandleValid(val))
		{
			screenInformation = BarFunctions.ScreenFromControl(val);
		}
		Rectangle rectangle = screenInformation.WorkingArea;
		if (Parent == null)
		{
			rectangle = screenInformation.Bounds;
			m_PopupMenu.UseWholeScreenForSizeChecking = true;
		}
		if (IsRightHanded)
		{
			if (m_Orientation == eOrientation.Horizontal)
			{
				x -= ((Control)m_PopupMenu).Width;
			}
			if (screenInformation != null && x < rectangle.Left && verifyScreenPosition)
			{
				PopupPositionAdjusted = true;
				if (Displayed && val != null && (m_PopupMenu.IsCustomizeMenu || base.IsOnMenu))
				{
					Point point = ((!m_PopupMenu.IsCustomizeMenu) ? new Point(m_Rect.Right, m_Rect.Top) : new Point(m_Rect.Right, m_Rect.Bottom));
					x = val.PointToScreen(point).X;
					if (x + ((Control)m_PopupMenu).Width > rectangle.Right)
					{
						x = rectangle.Right - ((Control)m_PopupMenu).Width;
					}
				}
				else
				{
					x = rectangle.Left;
				}
			}
		}
		else if (screenInformation != null && (x + ((Control)m_PopupMenu).Width > rectangle.Right || x < rectangle.Left) && verifyScreenPosition)
		{
			PopupPositionAdjusted = true;
			if (x + ((Control)m_PopupMenu).Width > rectangle.Right)
			{
				if (Displayed && val != null && !(val is ContextMenuBar))
				{
					Point point2 = (m_PopupMenu.IsCustomizeMenu ? new Point(m_Rect.Left, m_Rect.Bottom) : ((!base.IsOnMenu) ? new Point(m_Rect.Right, m_Rect.Top) : new Point(m_Rect.Left, m_Rect.Top)));
					x = val.PointToScreen(point2).X - ((Control)m_PopupMenu).Width;
					if (x + ((Control)m_PopupMenu).Width > rectangle.Right)
					{
						x = rectangle.Right - ((Control)m_PopupMenu).Width;
					}
				}
				else
				{
					x = rectangle.Right - ((Control)m_PopupMenu).Width;
				}
			}
			else if (x < rectangle.Left)
			{
				x = rectangle.Left;
			}
		}
		if (screenInformation != null && verifyScreenPosition && y + ((Control)m_PopupMenu).Height > rectangle.Bottom)
		{
			if (Displayed && val != null && val.Visible)
			{
				Point point3 = new Point(m_Rect.Left, m_Rect.Bottom - 1);
				Point point4 = val.PointToScreen(point3);
				point4.Y += 2;
				if (point4.Y - ((Control)m_PopupMenu).Height >= rectangle.Top)
				{
					y = Math.Min(rectangle.Bottom, point4.Y) - ((Control)m_PopupMenu).Height;
				}
				else if (this is ApplicationButton)
				{
					y = rectangle.Bottom - ((Control)m_PopupMenu).Height - 4;
				}
				else if (this is MetroAppButton)
				{
					y = rectangle.Bottom - ((Control)m_PopupMenu).Height - 4;
				}
			}
			else
			{
				y = rectangle.Bottom - ((Control)m_PopupMenu).Height;
			}
			if (y < rectangle.Top)
			{
				y = rectangle.Top;
			}
			PopupPositionAdjusted = true;
		}
		x += _PopupOffset.X;
		y += _PopupOffset.Y;
		((Control)m_PopupMenu).Location = new Point(x, y);
		OnPopupShowing(EventArgs.Empty);
		m_PopupMenu.Show();
		if (ContainerControl is IKeyTipsControl)
		{
			IKeyTipsControl keyTipsControl = ContainerControl as IKeyTipsControl;
			m_PopupMenu.ShowKeyTips = keyTipsControl.ShowKeyTips;
			keyTipsControl.ShowKeyTips = false;
		}
		Expanded = true;
		if (iOwnerMenuSupport == null)
		{
			return;
		}
		if (!(val is Bar) && !(val is MenuPanel))
		{
			iOwnerMenuSupport.RegisterPopup(this);
			m_FilterInstalled = true;
		}
		else if (val is Bar)
		{
			Bar bar2 = val as Bar;
			if (bar2.BarState != 0)
			{
				iOwnerMenuSupport.RegisterPopup(this);
				m_FilterInstalled = true;
			}
			bar2 = null;
		}
	}

	protected virtual void OnPopupShowing(EventArgs e)
	{
		if (this.PopupShowing != null)
		{
			this.PopupShowing(this, e);
		}
		GetIOwnerMenuSupport()?.InvokePopupShowing(this, e);
	}

	public void PopupBar(Point p)
	{
		PopupBar(p.X, p.Y);
	}

	public virtual void PopupBar(int x, int y)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (iOwnerMenuSupport != null)
		{
			PopupOpenEventArgs popupOpenEventArgs = new PopupOpenEventArgs();
			if (this.PopupOpen != null)
			{
				this.PopupOpen(this, popupOpenEventArgs);
			}
			if (!popupOpenEventArgs.Cancel)
			{
				iOwnerMenuSupport.InvokePopupOpen(this, popupOpenEventArgs);
			}
			if (popupOpenEventArgs.Cancel)
			{
				Expanded = false;
				return;
			}
		}
		foreach (BaseItem subItem in SubItems)
		{
			subItem.Orientation = eOrientation.Horizontal;
		}
		if (m_PopupBar == null)
		{
			m_PopupBar = new Bar();
			if (m_PopupFont != null)
			{
				((Control)m_PopupBar).Font = m_PopupFont;
			}
			else if (val != null && val.Font != null)
			{
				((Control)m_PopupBar).Font = (Font)val.Font.Clone();
			}
			if (val is Bar && ((Bar)(object)val).ColorScheme != null)
			{
				m_PopupBar.ColorScheme = ((Bar)(object)val).ColorScheme;
			}
			else if (val is MenuPanel && ((MenuPanel)(object)val).ColorScheme != null)
			{
				m_PopupBar.ColorScheme = ((MenuPanel)(object)val).ColorScheme;
			}
			else if (val is ItemControl && ((ItemControl)(object)val).ColorScheme != null)
			{
				m_PopupBar.ColorScheme = ((ItemControl)(object)val).ColorScheme;
			}
			else if (val is ButtonX && ((ButtonX)(object)val).ColorScheme != null)
			{
				m_PopupBar.ColorScheme = ((ButtonX)(object)val).ColorScheme;
			}
			else
			{
				m_PopupBar.ColorScheme = new ColorScheme(EffectiveStyle);
			}
			m_PopupBar.SetBarState(eBarState.Popup);
			m_PopupBar.PopupAnimation = m_PopupAnimation;
			m_PopupBar.Owner = GetOwner();
			m_PopupBar.ParentItem = this;
			if (val != null)
			{
				((Control)m_PopupBar).RightToLeft = val.RightToLeft;
			}
			((Control)m_PopupBar).CreateControl();
			m_PopupBar.SetDesignMode(DesignMode);
			m_PopupBar.ThemeAware = ThemeAware;
			if (val is IBarImageSize)
			{
				m_PopupBar.ImageSize = ((IBarImageSize)val).ImageSize;
			}
			m_PopupBar.PopupWidth = m_PopupWidth;
			m_PopupBar.RecalcSize();
		}
		ScreenInformation screenInformation = null;
		screenInformation = ((!BaseItem.IsHandleValid(val)) ? BarFunctions.ScreenFromPoint(new Point(x, y)) : BarFunctions.ScreenFromControl(val));
		if (IsRightHanded)
		{
			x -= ((Control)m_PopupBar).Width;
		}
		if (screenInformation != null && x + ((Control)m_PopupBar).Width > screenInformation.WorkingArea.Right)
		{
			x = screenInformation.WorkingArea.Right - ((Control)m_PopupBar).Width;
		}
		else if (screenInformation != null && x < screenInformation.WorkingArea.Left)
		{
			x = screenInformation.WorkingArea.Left;
		}
		if (screenInformation != null && y + ((Control)m_PopupBar).Height > screenInformation.WorkingArea.Bottom && Displayed && val != null)
		{
			Point point = new Point(m_Rect.Left, m_Rect.Bottom);
			Point point2 = val.PointToScreen(point);
			point2.Y += 2;
			if (point2.Y - ((Control)m_PopupBar).Height >= screenInformation.WorkingArea.Top)
			{
				y = point2.Y - ((Control)m_PopupBar).Height;
			}
		}
		x += _PopupOffset.X;
		y += _PopupOffset.Y;
		m_PopupBar.Location = new Point(x, y);
		iOwnerMenuSupport?.InvokePopupShowing(this, new EventArgs());
		m_PopupBar.ShowBar();
		Expanded = true;
		if (iOwnerMenuSupport == null)
		{
			return;
		}
		if (!(val is Bar) && !(val is MenuPanel))
		{
			iOwnerMenuSupport.RegisterPopup(this);
			m_FilterInstalled = true;
		}
		else if (val is Bar)
		{
			Bar bar = val as Bar;
			if (bar.BarState != 0)
			{
				iOwnerMenuSupport.RegisterPopup(this);
				m_FilterInstalled = true;
			}
			bar = null;
		}
	}

	public void PopupContainer(Point p)
	{
		PopupContainer(p.X, p.Y);
	}

	public virtual void CreatePopupContainer(bool fireLoadEvents)
	{
		if (PopupType != ePopupType.Container)
		{
			throw new InvalidOperationException("Method can only be called for PopupType set to ePopupType.Container");
		}
		if (m_PopupContainer != null)
		{
			return;
		}
		m_PopupContainer = new PopupContainerControl();
		m_PopupContainer.Owner = GetOwner();
		m_PopupContainer.ParentItem = this;
		((Control)m_PopupContainer).CreateControl();
		m_PopupContainer.SetDesignMode(DesignMode);
		((Control)m_PopupContainer).Width = m_PopupWidth;
		if (fireLoadEvents)
		{
			IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
			if (this.PopupContainerLoad != null)
			{
				this.PopupContainerLoad(this, new EventArgs());
			}
			iOwnerMenuSupport?.InvokePopupContainerLoad(this, new EventArgs());
			m_PopupContainer.RecalcSize();
		}
	}

	public virtual void PopupContainer(int x, int y)
	{
		IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
		if (iOwnerMenuSupport == null)
		{
			throw new InvalidOperationException("Current owner is not assigned or it does not have popup bar support.");
		}
		CreatePopupContainer(fireLoadEvents: true);
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		ScreenInformation screenInformation = null;
		screenInformation = ((!BaseItem.IsHandleValid(val)) ? BarFunctions.ScreenFromPoint(new Point(x, y)) : BarFunctions.ScreenFromControl(val));
		if (IsRightHanded)
		{
			x -= ((Control)m_PopupContainer).Width;
		}
		if (screenInformation != null && x + ((Control)m_PopupContainer).Width > screenInformation.WorkingArea.Right)
		{
			x = screenInformation.WorkingArea.Right - ((Control)m_PopupContainer).Width;
		}
		else if (screenInformation != null && x < screenInformation.WorkingArea.Left)
		{
			x = screenInformation.WorkingArea.Left;
		}
		if (screenInformation != null && y + ((Control)m_PopupContainer).Height > screenInformation.WorkingArea.Bottom && Displayed && val != null)
		{
			Point point = new Point(m_Rect.Left, m_Rect.Top);
			Point point2 = val.PointToScreen(point);
			point2.Y += 2;
			if (point2.Y - ((Control)m_PopupContainer).Height >= screenInformation.WorkingArea.Top)
			{
				y = point2.Y - ((Control)m_PopupContainer).Height;
			}
		}
		x += _PopupOffset.X;
		y += _PopupOffset.Y;
		((Control)m_PopupContainer).Location = new Point(x, y);
		iOwnerMenuSupport?.InvokePopupShowing(this, new EventArgs());
		m_PopupContainer.Show();
		Expanded = true;
		if (iOwnerMenuSupport == null)
		{
			return;
		}
		if (!(val is Bar) && !(val is MenuPanel))
		{
			iOwnerMenuSupport.RegisterPopup(this);
			m_FilterInstalled = true;
		}
		else if (val is Bar)
		{
			Bar bar = val as Bar;
			if (bar.BarState != 0)
			{
				iOwnerMenuSupport.RegisterPopup(this);
				m_FilterInstalled = true;
			}
			bar = null;
		}
	}

	public virtual void Popup(Point p)
	{
		Popup(p.X, p.Y);
	}

	public virtual void Popup(int x, int y)
	{
		if (m_PopupBar != null || m_PopupMenu != null || m_PopupContainer != null)
		{
			ClosePopup();
		}
		switch (m_PopupType)
		{
		case ePopupType.Container:
			PopupContainer(x, y);
			break;
		case ePopupType.Menu:
			PopupMenu(x, y);
			break;
		case ePopupType.ToolBar:
			PopupBar(x, y);
			break;
		}
	}

	protected virtual void OnPopupClose(EventArgs e)
	{
		IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
		this.PopupClose?.Invoke(this, e);
		iOwnerMenuSupport?.InvokePopupClose(this, e);
	}

	internal bool QueryPopupClosing(eEventSource eventSource)
	{
		CancelableEventSourceHandler queryPopupClose = this.QueryPopupClose;
		if (queryPopupClose != null)
		{
			CancelableEventSourceArgs cancelableEventSourceArgs = new CancelableEventSourceArgs(eventSource);
			queryPopupClose(this, cancelableEventSourceArgs);
			return !cancelableEventSourceArgs.Cancel;
		}
		return true;
	}

	public virtual void ClosePopup()
	{
		if (LockClosePopup)
		{
			return;
		}
		if (m_OldSubItemsImageSize != Size.Empty)
		{
			SubItemsImageSize = m_OldSubItemsImageSize;
			m_OldSubItemsImageSize = Size.Empty;
		}
		IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
		iOwnerMenuSupport?.UnregisterPopup(this);
		m_FilterInstalled = false;
		if (m_PopupContainer != null)
		{
			if (this.PopupContainerUnload != null)
			{
				this.PopupContainerUnload(this, new EventArgs());
			}
			iOwnerMenuSupport?.InvokePopupContainerUnload(this, new EventArgs());
		}
		if ((m_PopupBar != null || m_PopupMenu != null || m_PopupContainer != null) && iOwnerMenuSupport != null)
		{
			OnPopupClose(EventArgs.Empty);
		}
		if (m_PopupMenu != null)
		{
			if (ContainerControl is IKeyTipsControl)
			{
				IKeyTipsControl keyTipsControl = ContainerControl as IKeyTipsControl;
				keyTipsControl.ShowKeyTips = m_PopupMenu.ShowKeyTips;
				m_PopupMenu.ShowKeyTips = false;
			}
			m_PopupMenu.Close();
			((Component)(object)m_PopupMenu).Dispose();
			m_PopupMenu = null;
		}
		if (m_PopupBar != null)
		{
			m_PopupBar.Hide();
			((Component)(object)m_PopupBar).Dispose();
			m_PopupBar = null;
		}
		if (m_PopupContainer != null)
		{
			((Control)m_PopupContainer).Hide();
			((Component)(object)m_PopupContainer).Dispose();
			m_PopupContainer = null;
		}
		Expanded = false;
		if (GetOwner() is IOwner { ParentForm: not null } owner)
		{
			((Control)owner.ParentForm).Update();
		}
		OnPopupFinalized(EventArgs.Empty);
	}

	protected virtual void OnPopupFinalized(EventArgs e)
	{
		if (this.PopupFinalized != null)
		{
			this.PopupFinalized(this, e);
		}
	}

	private Point GetDisplayLocation()
	{
		Point empty = Point.Empty;
		if (Parent == null && !(ContainerControl is ButtonX))
		{
			return empty;
		}
		if (m_PopupSide == ePopupSide.Right)
		{
			empty.X = m_Rect.Right;
			empty.Y = m_Rect.Top;
		}
		else if (m_PopupSide == ePopupSide.Left)
		{
			Size popupSize = PopupSize;
			empty.X = m_Rect.Left - popupSize.Width;
			empty.Y = m_Rect.Top;
		}
		else if (m_PopupSide == ePopupSide.Top)
		{
			Size popupSize2 = PopupSize;
			empty.Y = m_Rect.Top - popupSize2.Height;
			if (popupSize2.Width > m_Rect.Width)
			{
				empty.X = m_Rect.Left;
			}
			else
			{
				empty.X = m_Rect.Right - popupSize2.Width;
			}
		}
		else if (m_PopupSide == ePopupSide.Bottom)
		{
			Size popupSize3 = PopupSize;
			empty.Y = m_Rect.Bottom;
			if (popupSize3.Width > m_Rect.Width)
			{
				empty.X = m_Rect.Left;
			}
			else
			{
				empty.X = m_Rect.Right - popupSize3.Width;
			}
		}
		return empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePopupOffset()
	{
		return _PopupOffset != DefaultPopupOffset;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPopupOffset()
	{
		PopupOffset = DefaultPopupOffset;
	}

	protected internal override void OnBeforeItemRemoved(BaseItem objItem)
	{
		base.OnBeforeItemRemoved(objItem);
		if (m_PopupBar != null && objItem != null)
		{
			m_PopupBar.Items.Remove(objItem);
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem objItem)
	{
		base.OnAfterItemRemoved(objItem);
		if (Expanded && objItem == null)
		{
			Expanded = false;
		}
		if (m_PopupMenu != null && !SuspendLayout)
		{
			m_PopupMenu.RecalcSize();
		}
		else if (m_PopupBar != null && !SuspendLayout)
		{
			m_PopupBar.RecalcLayout();
		}
	}

	protected internal override bool IsAnyOnHandle(IntPtr iHandle)
	{
		bool flag = base.IsAnyOnHandle(iHandle);
		if (!flag && m_PopupContainer != null && ((Control)m_PopupContainer).Handle == iHandle)
		{
			flag = true;
		}
		return flag;
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		if (!Expanded)
		{
			return null;
		}
		if (m_PopupMenu != null)
		{
			return m_PopupMenu.GetInsertPosition(pScreen, DragItem);
		}
		if (m_PopupBar != null)
		{
			return ((IDesignTimeProvider)m_PopupBar.ItemsContainer).GetInsertPosition(pScreen, DragItem);
		}
		return null;
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
		if (Expanded)
		{
			if (m_PopupMenu != null)
			{
				m_PopupMenu.DrawReversibleMarker(iPos, Before);
			}
			else if (m_PopupBar != null)
			{
				((IDesignTimeProvider)m_PopupBar.Items).DrawReversibleMarker(iPos, Before);
			}
		}
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		if (Expanded)
		{
			if (m_PopupMenu != null)
			{
				m_PopupMenu.InsertItemAt(objItem, iPos, Before);
			}
			else
			{
				((IDesignTimeProvider)m_PopupBar.Items).InsertItemAt(objItem, iPos, Before);
			}
		}
	}

	public void SetSourceControl(Control ctrl)
	{
		m_SourceControl = ctrl;
	}

	protected override void OnDisplayedChanged()
	{
		if (!Displayed && Expanded)
		{
			Expanded = false;
		}
	}
}
