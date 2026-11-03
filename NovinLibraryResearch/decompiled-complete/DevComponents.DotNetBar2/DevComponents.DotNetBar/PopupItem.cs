using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Events;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public abstract class PopupItem : ImageItem, IDesignTimeProvider
{
	private EventHandler _09BE;

	private EventHandler _09BF;

	private DotNetBarManager.PopupOpenEventHandler _09C0;

	private EventHandler _09C1;

	private CancelableEventSourceHandler ಒ;

	private EventHandler ಓ;

	private EventHandler _09C2;

	private MenuPanel ಔ;

	private Bar ಕ;

	private PopupContainerControl ಖ;

	private bool _২;

	protected SideBarImage m_SideBar;

	private ePopupType ಗ;

	private bool ಘ;

	private Size ಙ;

	private int ಚ = 200;

	private ePersonalizedMenus ಛ;

	private Point ಜ = Point.Empty;

	private ePopupAnimation ஃ = ePopupAnimation.ManagerControlled;

	private Font ಝ;

	private Control ಞ;

	private ePopupSide ಟ;

	private bool ಠ;

	private bool _ಡ = true;

	private static readonly Point ಢ = Point.Empty;

	private Point _ಣ = ಢ;

	protected virtual bool IsRightHanded => false;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Size PopupSize
	{
		get
		{
			if (SubItemsImageSize.Width == ImageItem._ನ.Width && SubItemsImageSize.Height == ImageItem._ನ.Height)
			{
				ಙ = SubItemsImageSize;
				SubItemsImageSize = new Size(16, 16);
			}
			if (ಗ == ePopupType.Container)
			{
				CreatePopupContainer(fireLoadEvents: true);
				if (ಖ != null)
				{
					return ((Control)ಖ).Size;
				}
				return new Size(ಚ, 150);
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
			if (ಝ != null)
			{
				((Control)menuPanel).Font = ಝ;
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
				menuPanel.PersonalizedMenus = ಛ;
			}
			if (iOwnerMenuSupport != null && !iOwnerMenuSupport.ShowFullMenusOnHover && (menuPanel.PersonalizedMenus == ePersonalizedMenus.DisplayOnHover || menuPanel.PersonalizedMenus == ePersonalizedMenus.Both))
			{
				menuPanel.PersonalizedMenus = ePersonalizedMenus.DisplayOnClick;
			}
			menuPanel.PopupAnimation = ஃ;
			if (iOwnerMenuSupport != null && (IsOnMenuBar || base.IsOnMenu))
			{
				menuPanel.PersonalizedAllVisible = iOwnerMenuSupport.PersonalizedAllVisible;
			}
			if (Parent is CustomizeItem || (this is CustomizeItem && !(this is QatCustomizeItem)))
			{
				menuPanel.ဒ = true;
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(true)]
	public bool VerifyPopupScreenPosition
	{
		get
		{
			return _ಡ;
		}
		set
		{
			_ಡ = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(ePopupType.Menu)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether sub-items are shown on popup Bar or popup menu.")]
	public virtual ePopupType PopupType
	{
		get
		{
			return ಗ;
		}
		set
		{
			if (ಗ != value)
			{
				ಗ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "PopupType");
				}
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Description("Indicates the font that will be used on the popup window.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	public virtual Font PopupFont
	{
		get
		{
			return ಝ;
		}
		set
		{
			if (ಝ != value)
			{
				ಝ = value;
			}
		}
	}

	[Description("Indicates side bar for pop-up window.")]
	[Category("Appearance")]
	[Browsable(false)]
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

	[DefaultValue(ePopupSide.Default)]
	[DevCoBrowsable(false)]
	[Description("Indicates location of popup in relation to it's parent.")]
	[Browsable(true)]
	public virtual ePopupSide PopupSide
	{
		get
		{
			return ಟ;
		}
		set
		{
			ಟ = value;
			OnAppearanceChanged();
		}
	}

	[Description("Indicates popup offset.")]
	[Category("Behavior")]
	public Point PopupOffset
	{
		get
		{
			return _ಣ;
		}
		set
		{
			_ಣ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point PopupLocation
	{
		get
		{
			if (ಔ != null)
			{
				return ((Control)ಔ).Location;
			}
			if (ಕ != null)
			{
				return ಕ.Location;
			}
			if (ಖ != null)
			{
				return ((Control)ಖ).Location;
			}
			return ಜ;
		}
		set
		{
			ಜ = value;
			if (ಔ != null)
			{
				((Control)ಔ).Location = value;
			}
			else if (ಕ != null)
			{
				ಕ.Location = value;
			}
			else if (ಖ != null)
			{
				((Control)ಖ).Location = value;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(ePersonalizedMenus.Disabled)]
	public virtual ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return ಛ;
		}
		set
		{
			ಛ = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(ePopupAnimation.ManagerControlled)]
	[Browsable(true)]
	[Description("Indicates Animation type for Popups.")]
	[Category("Behavior")]
	public virtual ePopupAnimation PopupAnimation
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

	internal bool ಐ
	{
		get
		{
			return _২;
		}
		set
		{
			_২ = true;
		}
	}

	[DefaultValue(200)]
	[Category("Layout")]
	[Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public virtual int PopupWidth
	{
		get
		{
			return ಚ;
		}
		set
		{
			if (ಚ != value && value > 0)
			{
				ಚ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "PopupWidth");
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Description("Gets the control that is displaying the context menu.")]
	public Control SourceControl => ಞ;

	[Browsable(false)]
	public PopupContainerControl PopupContainerControl => ಖ;

	[Browsable(false)]
	public Control PopupControl
	{
		get
		{
			if (ಔ != null)
			{
				return (Control)(object)ಔ;
			}
			if (ಕ != null)
			{
				return (Control)(object)ಕ;
			}
			if (ಖ != null)
			{
				return (Control)(object)ಖ;
			}
			return null;
		}
	}

	internal virtual bool _0C91
	{
		get
		{
			return ಠ;
		}
		set
		{
			ಠ = value;
		}
	}

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
	public event DotNetBarManager.PopupOpenEventHandler PopupOpen
	{
		add
		{
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler = _09C0;
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				DotNetBarManager.PopupOpenEventHandler value2 = (DotNetBarManager.PopupOpenEventHandler)Delegate.Combine(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _09C0, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
		remove
		{
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler = _09C0;
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				DotNetBarManager.PopupOpenEventHandler value2 = (DotNetBarManager.PopupOpenEventHandler)Delegate.Remove(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _09C0, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
	}

	[Description("Occurs when popup item is closing.")]
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

	[Description("Occurs before popup is closed and allows you to cancel closing.")]
	public event CancelableEventSourceHandler QueryPopupClose
	{
		add
		{
			CancelableEventSourceHandler cancelableEventSourceHandler = ಒ;
			CancelableEventSourceHandler cancelableEventSourceHandler2;
			do
			{
				cancelableEventSourceHandler2 = cancelableEventSourceHandler;
				CancelableEventSourceHandler value2 = (CancelableEventSourceHandler)Delegate.Combine(cancelableEventSourceHandler2, value);
				cancelableEventSourceHandler = Interlocked.CompareExchange(ref ಒ, value2, cancelableEventSourceHandler2);
			}
			while ((object)cancelableEventSourceHandler != cancelableEventSourceHandler2);
		}
		remove
		{
			CancelableEventSourceHandler cancelableEventSourceHandler = ಒ;
			CancelableEventSourceHandler cancelableEventSourceHandler2;
			do
			{
				cancelableEventSourceHandler2 = cancelableEventSourceHandler;
				CancelableEventSourceHandler value2 = (CancelableEventSourceHandler)Delegate.Remove(cancelableEventSourceHandler2, value);
				cancelableEventSourceHandler = Interlocked.CompareExchange(ref ಒ, value2, cancelableEventSourceHandler2);
			}
			while ((object)cancelableEventSourceHandler != cancelableEventSourceHandler2);
		}
	}

	[Description("Occurs after popup item has been closed.")]
	public event EventHandler PopupFinalized
	{
		add
		{
			EventHandler eventHandler = ಓ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ಓ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ಓ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ಓ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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
		ಔ = null;
		ಕ = null;
		_২ = false;
		ಗ = ePopupType.Menu;
		m_SideBar = default(SideBarImage);
		ಘ = false;
		ಙ = Size.Empty;
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
		popupItem.PersonalizedMenus = PersonalizedMenus;
		popupItem.PopUpSideBar = PopUpSideBar;
		popupItem.PopupAnimation = PopupAnimation;
		popupItem.PopupSide = PopupSide;
		popupItem.PopupWidth = PopupWidth;
		if (_09C1 != null)
		{
			popupItem._09C1 = (EventHandler)_09C1.Clone();
		}
		if (_09BE != null)
		{
			popupItem._09BE = (EventHandler)_09BE.Clone();
		}
		if (_09BF != null)
		{
			popupItem._09BF = (EventHandler)_09BF.Clone();
		}
		if (ಓ != null)
		{
			popupItem.ಓ = (EventHandler)ಓ.Clone();
		}
		if (_09C0 != null)
		{
			popupItem._09C0 = (DotNetBarManager.PopupOpenEventHandler)_09C0.Clone();
		}
		if (_09C2 != null)
		{
			popupItem._09C2 = (EventHandler)_09C2.Clone();
		}
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
		itemXmlElement.SetAttribute("PopupType", XmlConvert.ToString((int)ಗ));
		itemXmlElement.SetAttribute("PopupWidth", XmlConvert.ToString(ಚ));
		itemXmlElement.SetAttribute("PersonalizedMenus", XmlConvert.ToString((int)ಛ));
		itemXmlElement.SetAttribute("panim", XmlConvert.ToString((int)ஃ));
		if (ಝ != null)
		{
			itemXmlElement.SetAttribute("fontname", ಝ.Name);
			itemXmlElement.SetAttribute("fontemsize", XmlConvert.ToString(ಝ.Size));
			itemXmlElement.SetAttribute("fontstyle", XmlConvert.ToString((int)ಝ.Style));
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
		ಗ = (ePopupType)XmlConvert.ToInt32(itemXmlElement.GetAttribute("PopupType"));
		ಚ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("PopupWidth"));
		ಛ = (ePersonalizedMenus)XmlConvert.ToInt32(itemXmlElement.GetAttribute("PersonalizedMenus"));
		if (itemXmlElement.HasAttribute("panim"))
		{
			ஃ = (ePopupAnimation)XmlConvert.ToInt32(itemXmlElement.GetAttribute("panim"));
		}
		else
		{
			ஃ = ePopupAnimation.SystemDefault;
		}
		ಝ = null;
		if (itemXmlElement.HasAttribute("fontname"))
		{
			string attribute = itemXmlElement.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(itemXmlElement.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("fontstyle"));
			try
			{
				ಝ = new Font(attribute, num, val);
			}
			catch (Exception)
			{
				ಝ = null;
			}
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		if (Expanded)
		{
			if (ಔ != null)
			{
				if (ಔ.FocusedItem() != null)
				{
					ಔ.FocusedItem().InternalKeyDown(objArg);
				}
				if (ಔ != null)
				{
					ಔ._0A81(objArg);
				}
			}
			else if (ಕ != null)
			{
				ಕ._0A81(objArg);
			}
			else
			{
				_ = ಖ;
			}
		}
		else
		{
			base.InternalKeyDown(objArg);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		base.InternalMouseMove(objArg);
		if (Expanded && ಔ != null && !DesignMode)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			MenuPanel menuPanel = ಔ;
			if (val != null)
			{
				Point point = val.PointToScreen(new Point(objArg.X, objArg.Y));
				if (menuPanel != null && !((Control)menuPanel).IsDisposed)
				{
					point = ((Control)menuPanel).PointToClient(point);
					menuPanel._0BFD(new MouseEventArgs(objArg.Button, objArg.Clicks, point.X, point.Y, objArg.Delta));
				}
			}
		}
		else
		{
			if (!Expanded || ಕ == null || DesignMode)
			{
				return;
			}
			object containerControl2 = ContainerControl;
			Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			Bar bar = ಕ;
			if (val2 != null)
			{
				Point point2 = val2.PointToScreen(new Point(objArg.X, objArg.Y));
				if (bar != null && !((Control)bar).IsDisposed)
				{
					point2 = ((Control)bar).PointToClient(point2);
					bar._0BFD(new MouseEventArgs((MouseButtons)0, objArg.Clicks, point2.X, point2.Y, objArg.Delta));
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
		if (Expanded && ಔ != null && !DesignMode)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Point point = val.PointToScreen(new Point(objArg.X, objArg.Y));
				point = ((Control)ಔ).PointToClient(point);
				ಔ.ఇ(new MouseEventArgs(objArg.Button, objArg.Clicks, point.X, point.Y, objArg.Delta));
			}
		}
		else if (Expanded && ಕ != null && !DesignMode)
		{
			object containerControl2 = ContainerControl;
			Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			if (val2 != null)
			{
				Point point2 = val2.PointToScreen(new Point(objArg.X, objArg.Y));
				point2 = ((Control)ಕ).PointToClient(point2);
				ಕ.ఇ(new MouseEventArgs(objArg.Button, objArg.Clicks, point2.X, point2.Y, objArg.Delta));
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
		if (ಔ != null)
		{
			ಔ.RecalcSize();
		}
		else if (ಕ != null)
		{
			ಕ.RecalcLayout();
		}
	}

	protected internal override void OnExpandChange()
	{
		base.OnExpandChange();
		if (!Expanded)
		{
			ಜ = Point.Empty;
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
					ಘ = false;
					if (val.Focused)
					{
						ಘ = true;
					}
					else if (val is Bar)
					{
						Bar bar = val as Bar;
						if (bar.BarState != eBarState.Floating || bar.MenuBar)
						{
							bar._09A9 = true;
						}
					}
					else
					{
						val.Focus();
					}
				}
				else
				{
					if (!ಘ)
					{
						((Bar)(object)val).ReleaseFocus();
					}
					Bar bar2 = val as Bar;
					if (bar2.BarState != eBarState.Floating && !bar2.MenuBar)
					{
						bar2._09A9 = false;
					}
					ಘ = false;
				}
			}
			else if (val is ItemControl && !DesignMode)
			{
				if (Expanded)
				{
					ಘ = false;
					if (val.Focused)
					{
						ಘ = true;
					}
					else
					{
						ItemControl itemControl = val as ItemControl;
						itemControl._09A9 = true;
					}
				}
				else
				{
					if (!Expanded && (ಔ != null || ಕ != null || ಖ != null))
					{
						ClosePopup();
					}
					if (!ಘ)
					{
						((ItemControl)(object)val).ReleaseFocus();
					}
					((ItemControl)(object)val)._09A9 = false;
					ಘ = false;
				}
			}
		}
		if (!Expanded && (ಔ != null || ಕ != null || ಖ != null))
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
		if (Expanded && ಔ == null && ಕ == null && ಖ == null && BaseItem.IsHandleValid(val))
		{
			if (ಗ == ePopupType.Menu)
			{
				Point point = ((!ಜ.IsEmpty) ? ಜ : ((ಟ != 0) ? ದ() : ((Parent is CustomizeItem || this is CustomizeItem) ? ((!IsRightHanded) ? new Point(m_Rect.Right - 2, m_Rect.Top - 2) : new Point(m_Rect.Left + 3, m_Rect.Top)) : (base.IsOnMenu ? ((!IsRightHanded) ? new Point(m_Rect.Right, m_Rect.Top - 2) : new Point(m_Rect.Left + 2, m_Rect.Top - 2)) : ((m_Orientation != 0) ? new Point(m_Rect.Right, m_Rect.Top) : ((!IsRightHanded) ? new Point(m_Rect.Left, m_Rect.Bottom) : new Point(m_Rect.Right, m_Rect.Bottom)))))));
				Point p = val.PointToScreen(point);
				PopupMenu(p);
			}
			else if (ಗ == ePopupType.ToolBar)
			{
				Point point2 = ((!ಜ.IsEmpty) ? ಜ : ((ಟ != 0) ? ದ() : (base.IsOnMenu ? ((!IsRightHanded) ? new Point(m_Rect.Right, m_Rect.Top - 2) : new Point(m_Rect.Left + 2, m_Rect.Top - 2)) : ((m_Orientation != 0) ? new Point(m_Rect.Right, m_Rect.Top) : ((!IsRightHanded) ? new Point(m_Rect.Left, m_Rect.Bottom) : new Point(m_Rect.Right, m_Rect.Bottom))))));
				Point p2 = val.PointToScreen(point2);
				PopupBar(p2);
			}
			else if (ಗ == ePopupType.Container)
			{
				Point point3 = ((!ಜ.IsEmpty) ? ಜ : ((ಟ != 0) ? ದ() : (base.IsOnMenu ? ((!IsRightHanded) ? new Point(m_Rect.Right, m_Rect.Top - 2) : new Point(m_Rect.Left + 2, m_Rect.Top - 2)) : ((m_Orientation != 0) ? new Point(m_Rect.Right, m_Rect.Top) : ((!IsRightHanded) ? new Point(m_Rect.Left, m_Rect.Bottom) : new Point(m_Rect.Right, m_Rect.Bottom))))));
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
		if (_09C0 != null)
		{
			_09C0(this, e);
		}
		if (!e.Cancel)
		{
			GetIOwnerMenuSupport()?.InvokePopupOpen(this, e);
		}
	}

	public virtual void PopupMenu(int x, int y)
	{
		PopupMenu(x, y, _ಡ);
	}

	public virtual void PopupMenu(int x, int y, bool verifyScreenPosition)
	{
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		_0C91 = false;
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
		if (SubItemsImageSize.Width == ImageItem._ನ.Width && SubItemsImageSize.Height == ImageItem._ನ.Height)
		{
			ಙ = SubItemsImageSize;
			SubItemsImageSize = new Size(16, 16);
		}
		foreach (BaseItem subItem in SubItems)
		{
			subItem.Orientation = eOrientation.Horizontal;
		}
		if (ಔ == null)
		{
			ಔ = new MenuPanel();
			ಔ.Owner = GetOwner();
			ಔ.ဢ = !verifyScreenPosition;
			if (ಝ != null)
			{
				((Control)ಔ).Font = ಝ;
			}
			else if (val != null && val.Font != null)
			{
				if (((object)val).GetType().Name.Contains("MetroTabStrip") && val.Parent != null && val.Parent.Font != null)
				{
					((Control)ಔ).Font = val.Parent.Font;
				}
				else
				{
					((Control)ಔ).Font = val.Font;
				}
			}
			if (iOwnerMenuSupport != null && iOwnerMenuSupport.AlwaysShowFullMenus)
			{
				ಔ.PersonalizedMenus = ePersonalizedMenus.Disabled;
			}
			else
			{
				ಔ.PersonalizedMenus = ಛ;
			}
			if (iOwnerMenuSupport != null && !iOwnerMenuSupport.ShowFullMenusOnHover && (ಔ.PersonalizedMenus == ePersonalizedMenus.DisplayOnHover || ಔ.PersonalizedMenus == ePersonalizedMenus.Both))
			{
				ಔ.PersonalizedMenus = ePersonalizedMenus.DisplayOnClick;
			}
			ಔ.PopupAnimation = ஃ;
			if (iOwnerMenuSupport != null && (IsOnMenuBar || base.IsOnMenu))
			{
				ಔ.PersonalizedAllVisible = iOwnerMenuSupport.PersonalizedAllVisible;
			}
			if (Parent is CustomizeItem || (this is CustomizeItem && !(this is QatCustomizeItem)))
			{
				ಔ.ဒ = true;
			}
			if (val is Bar)
			{
				Bar bar = (Bar)(object)val;
				ಔ.ColorScheme = bar.GetColorScheme();
				ಔ.ShowToolTips = bar.ShowToolTips;
				ಔ.AntiAlias = bar.AntiAlias;
				if (val is ContextMenuBar)
				{
					ಔ.ဢ = true;
				}
			}
			else if (val is MenuPanel)
			{
				MenuPanel menuPanel = (MenuPanel)(object)val;
				ಔ.ColorScheme = menuPanel.ColorScheme;
				ಔ.ShowToolTips = menuPanel.ShowToolTips;
				ಔ.AntiAlias = menuPanel.AntiAlias;
			}
			else if (val is SideBar)
			{
				ಔ.ColorScheme = ((SideBar)(object)val).ColorScheme;
			}
			else if (val is ExplorerBar)
			{
				ExplorerBar explorerBar = val as ExplorerBar;
				ಔ.ColorScheme = explorerBar.ColorScheme;
				ಔ.AntiAlias = explorerBar.AntiAlias;
			}
			else if (val is BarBaseControl)
			{
				ಔ.ColorScheme = ((BarBaseControl)(object)val).ColorScheme;
			}
			else if (val is ItemControl)
			{
				ItemControl itemControl = (ItemControl)(object)val;
				ಔ.ColorScheme = itemControl.ઓ();
				ಔ.AntiAlias = itemControl.AntiAlias;
			}
			else if (val is PopupItemControl)
			{
				PopupItemControl popupItemControl = (PopupItemControl)(object)val;
				ಔ.ColorScheme = popupItemControl.GetColorScheme();
				ಔ.AntiAlias = popupItemControl.AntiAlias;
			}
			else
			{
				if (Style == eDotNetBarStyle.StyleManagerControlled && GlobalManager.Renderer is Office2007Renderer)
				{
					ಔ.ColorScheme = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
				}
				else
				{
					ಔ.ColorScheme = new ColorScheme(EffectiveStyle);
				}
				object owner = GetOwner();
				if (owner is ItemControl)
				{
					ಔ.AntiAlias = ((ItemControl)owner).AntiAlias;
				}
				else if (owner is Bar)
				{
					ಔ.AntiAlias = ((Bar)owner).AntiAlias;
				}
			}
			if (val != null)
			{
				((Control)ಔ).RightToLeft = val.RightToLeft;
			}
			ಔ.SideBar = m_SideBar;
			ಔ.ParentItem = this;
			((Control)ಔ).CreateControl();
			ಔ.RecalcSize();
		}
		ঊ ঊ2 = BarFunctions.ॹ(new Point(x, y));
		if (ঊ2 == null && BaseItem.IsHandleValid(val))
		{
			ঊ2 = BarFunctions.ॻ(val);
		}
		Rectangle rectangle = ঊ2.ঌ;
		if (Parent == null)
		{
			rectangle = ঊ2.ঋ;
			ಔ.ဢ = true;
		}
		if (IsRightHanded)
		{
			x -= ((Control)ಔ).Width;
			if (ঊ2 != null && x < rectangle.Left && verifyScreenPosition)
			{
				_0C91 = true;
				if (Displayed && val != null && (ಔ.ဒ || base.IsOnMenu))
				{
					Point point = ((!ಔ.ဒ) ? new Point(m_Rect.Right, m_Rect.Top) : new Point(m_Rect.Right, m_Rect.Bottom));
					x = val.PointToScreen(point).X;
					if (x + ((Control)ಔ).Width > rectangle.Right)
					{
						x = rectangle.Right - ((Control)ಔ).Width;
					}
				}
				else
				{
					x = rectangle.Left;
				}
			}
		}
		else if (ঊ2 != null && (x + ((Control)ಔ).Width > rectangle.Right || x < rectangle.Left) && verifyScreenPosition)
		{
			_0C91 = true;
			if (x + ((Control)ಔ).Width > rectangle.Right)
			{
				if (Displayed && val != null && !(val is ContextMenuBar))
				{
					Point point2 = (ಔ.ဒ ? new Point(m_Rect.Left, m_Rect.Bottom) : ((!base.IsOnMenu) ? new Point(m_Rect.Right, m_Rect.Top) : new Point(m_Rect.Left, m_Rect.Top)));
					x = val.PointToScreen(point2).X - ((Control)ಔ).Width;
					if (x + ((Control)ಔ).Width > rectangle.Right)
					{
						x = rectangle.Right - ((Control)ಔ).Width;
					}
				}
				else
				{
					x = rectangle.Right - ((Control)ಔ).Width;
				}
			}
			else if (x < rectangle.Left)
			{
				x = rectangle.Left;
			}
		}
		if (ঊ2 != null && verifyScreenPosition && y + ((Control)ಔ).Height > rectangle.Bottom)
		{
			if (Displayed && val != null && val.Visible)
			{
				Point point3 = new Point(m_Rect.Left, m_Rect.Bottom - 1);
				Point point4 = val.PointToScreen(point3);
				point4.Y += 2;
				if (point4.Y - ((Control)ಔ).Height >= rectangle.Top)
				{
					y = Math.Min(rectangle.Bottom, point4.Y) - ((Control)ಔ).Height;
				}
				else if (this is ApplicationButton)
				{
					y = rectangle.Bottom - ((Control)ಔ).Height - 4;
				}
				else if (this is MetroAppButton)
				{
					y = rectangle.Bottom - ((Control)ಔ).Height - 4;
				}
			}
			else
			{
				y = rectangle.Bottom - ((Control)ಔ).Height;
			}
			if (y < rectangle.Top)
			{
				y = rectangle.Top;
			}
			_0C91 = true;
		}
		x += _ಣ.X;
		y += _ಣ.Y;
		((Control)ಔ).Location = new Point(x, y);
		OnPopupShowing(EventArgs.Empty);
		ಔ.Show();
		if (ContainerControl is IKeyTipsControl)
		{
			IKeyTipsControl keyTipsControl = ContainerControl as IKeyTipsControl;
			ಔ.ShowKeyTips = keyTipsControl.ShowKeyTips;
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
			_২ = true;
		}
		else if (val is Bar)
		{
			Bar bar2 = val as Bar;
			if (bar2.BarState != 0)
			{
				iOwnerMenuSupport.RegisterPopup(this);
				_২ = true;
			}
			bar2 = null;
		}
	}

	protected virtual void OnPopupShowing(EventArgs e)
	{
		if (_09C2 != null)
		{
			_09C2(this, e);
		}
		GetIOwnerMenuSupport()?.InvokePopupShowing(this, e);
	}

	public void PopupBar(Point p)
	{
		PopupBar(p.X, p.Y);
	}

	public virtual void PopupBar(int x, int y)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (iOwnerMenuSupport != null)
		{
			PopupOpenEventArgs popupOpenEventArgs = new PopupOpenEventArgs();
			if (_09C0 != null)
			{
				_09C0(this, popupOpenEventArgs);
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
		if (ಕ == null)
		{
			ಕ = new Bar();
			if (ಝ != null)
			{
				((Control)ಕ).Font = ಝ;
			}
			else if (val != null && val.Font != null)
			{
				((Control)ಕ).Font = (Font)val.Font.Clone();
			}
			if (val is Bar && ((Bar)(object)val).ColorScheme != null)
			{
				ಕ.ColorScheme = ((Bar)(object)val).ColorScheme;
			}
			else if (val is MenuPanel && ((MenuPanel)(object)val).ColorScheme != null)
			{
				ಕ.ColorScheme = ((MenuPanel)(object)val).ColorScheme;
			}
			else if (val is ItemControl && ((ItemControl)(object)val).ColorScheme != null)
			{
				ಕ.ColorScheme = ((ItemControl)(object)val).ColorScheme;
			}
			else if (val is ButtonX && ((ButtonX)(object)val).ColorScheme != null)
			{
				ಕ.ColorScheme = ((ButtonX)(object)val).ColorScheme;
			}
			else
			{
				ಕ.ColorScheme = new ColorScheme(EffectiveStyle);
			}
			ಕ.మ(eBarState.Popup);
			ಕ.PopupAnimation = ஃ;
			ಕ.Owner = GetOwner();
			ಕ.ParentItem = this;
			if (val != null)
			{
				((Control)ಕ).RightToLeft = val.RightToLeft;
			}
			((Control)ಕ).CreateControl();
			ಕ.SetDesignMode(DesignMode);
			ಕ.ThemeAware = ThemeAware;
			if (val is IBarImageSize)
			{
				ಕ.ImageSize = ((IBarImageSize)val).ImageSize;
			}
			ಕ._0B0E = ಚ;
			ಕ.RecalcSize();
		}
		ঊ ঊ2 = null;
		ঊ2 = ((!BaseItem.IsHandleValid(val)) ? BarFunctions.ॹ(new Point(x, y)) : BarFunctions.ॻ(val));
		if (IsRightHanded)
		{
			x -= ((Control)ಕ).Width;
		}
		if (ঊ2 != null && x + ((Control)ಕ).Width > ঊ2.ঌ.Right)
		{
			x = ঊ2.ঌ.Right - ((Control)ಕ).Width;
		}
		else if (ঊ2 != null && x < ঊ2.ঌ.Left)
		{
			x = ঊ2.ঌ.Left;
		}
		if (ঊ2 != null && y + ((Control)ಕ).Height > ঊ2.ঌ.Bottom && Displayed && val != null)
		{
			Point point = new Point(m_Rect.Left, m_Rect.Bottom);
			Point point2 = val.PointToScreen(point);
			point2.Y += 2;
			if (point2.Y - ((Control)ಕ).Height >= ঊ2.ঌ.Top)
			{
				y = point2.Y - ((Control)ಕ).Height;
			}
		}
		x += _ಣ.X;
		y += _ಣ.Y;
		ಕ.Location = new Point(x, y);
		iOwnerMenuSupport?.InvokePopupShowing(this, new EventArgs());
		ಕ._0C3A();
		Expanded = true;
		if (iOwnerMenuSupport == null)
		{
			return;
		}
		if (!(val is Bar) && !(val is MenuPanel))
		{
			iOwnerMenuSupport.RegisterPopup(this);
			_২ = true;
		}
		else if (val is Bar)
		{
			Bar bar = val as Bar;
			if (bar.BarState != 0)
			{
				iOwnerMenuSupport.RegisterPopup(this);
				_২ = true;
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
		if (ಖ != null)
		{
			return;
		}
		ಖ = new PopupContainerControl();
		ಖ.Owner = GetOwner();
		ಖ.ParentItem = this;
		((Control)ಖ).CreateControl();
		ಖ.ᒶ(DesignMode);
		((Control)ಖ).Width = ಚ;
		if (fireLoadEvents)
		{
			IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
			if (_09BE != null)
			{
				_09BE(this, new EventArgs());
			}
			iOwnerMenuSupport?.InvokePopupContainerLoad(this, new EventArgs());
			ಖ.RecalcSize();
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
		ঊ ঊ2 = null;
		ঊ2 = ((!BaseItem.IsHandleValid(val)) ? BarFunctions.ॹ(new Point(x, y)) : BarFunctions.ॻ(val));
		if (IsRightHanded)
		{
			x -= ((Control)ಖ).Width;
		}
		if (ঊ2 != null && x + ((Control)ಖ).Width > ঊ2.ঌ.Right)
		{
			x = ঊ2.ঌ.Right - ((Control)ಖ).Width;
		}
		else if (ঊ2 != null && x < ঊ2.ঌ.Left)
		{
			x = ঊ2.ঌ.Left;
		}
		if (ঊ2 != null && y + ((Control)ಖ).Height > ঊ2.ঌ.Bottom && Displayed && val != null)
		{
			Point point = new Point(m_Rect.Left, m_Rect.Top);
			Point point2 = val.PointToScreen(point);
			point2.Y += 2;
			if (point2.Y - ((Control)ಖ).Height >= ঊ2.ঌ.Top)
			{
				y = point2.Y - ((Control)ಖ).Height;
			}
		}
		x += _ಣ.X;
		y += _ಣ.Y;
		((Control)ಖ).Location = new Point(x, y);
		iOwnerMenuSupport?.InvokePopupShowing(this, new EventArgs());
		ಖ.Show();
		Expanded = true;
		if (iOwnerMenuSupport == null)
		{
			return;
		}
		if (!(val is Bar) && !(val is MenuPanel))
		{
			iOwnerMenuSupport.RegisterPopup(this);
			_২ = true;
		}
		else if (val is Bar)
		{
			Bar bar = val as Bar;
			if (bar.BarState != 0)
			{
				iOwnerMenuSupport.RegisterPopup(this);
				_২ = true;
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
		if (ಕ != null || ಔ != null || ಖ != null)
		{
			ClosePopup();
		}
		switch (ಗ)
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
		if (_09C1 != null)
		{
			_09C1(this, e);
		}
		iOwnerMenuSupport?.InvokePopupClose(this, e);
	}

	internal bool ತ(eEventSource ಥ)
	{
		CancelableEventSourceHandler cancelableEventSourceHandler = ಒ;
		if (cancelableEventSourceHandler != null)
		{
			CancelableEventSourceArgs cancelableEventSourceArgs = new CancelableEventSourceArgs(ಥ);
			cancelableEventSourceHandler(this, cancelableEventSourceArgs);
			return !cancelableEventSourceArgs.Cancel;
		}
		return true;
	}

	public virtual void ClosePopup()
	{
		if (ಙ != Size.Empty)
		{
			SubItemsImageSize = ಙ;
			ಙ = Size.Empty;
		}
		IOwnerMenuSupport iOwnerMenuSupport = GetIOwnerMenuSupport();
		iOwnerMenuSupport?.UnregisterPopup(this);
		_২ = false;
		if (ಖ != null)
		{
			if (_09BF != null)
			{
				_09BF(this, new EventArgs());
			}
			iOwnerMenuSupport?.InvokePopupContainerUnload(this, new EventArgs());
		}
		if ((ಕ != null || ಔ != null || ಖ != null) && iOwnerMenuSupport != null)
		{
			OnPopupClose(EventArgs.Empty);
		}
		if (ಔ != null)
		{
			if (ContainerControl is IKeyTipsControl)
			{
				IKeyTipsControl keyTipsControl = ContainerControl as IKeyTipsControl;
				keyTipsControl.ShowKeyTips = ಔ.ShowKeyTips;
				ಔ.ShowKeyTips = false;
			}
			ಔ.Close();
			((Component)(object)ಔ).Dispose();
			ಔ = null;
		}
		if (ಕ != null)
		{
			ಕ.Hide();
			((Component)(object)ಕ).Dispose();
			ಕ = null;
		}
		if (ಖ != null)
		{
			((Control)ಖ).Hide();
			((Component)(object)ಖ).Dispose();
			ಖ = null;
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
		if (ಓ != null)
		{
			ಓ(this, e);
		}
	}

	private Point ದ()
	{
		Point empty = Point.Empty;
		if (Parent == null && !(ContainerControl is ButtonX))
		{
			return empty;
		}
		if (ಟ == ePopupSide.Right)
		{
			empty.X = m_Rect.Right;
			empty.Y = m_Rect.Top;
		}
		else if (ಟ == ePopupSide.Left)
		{
			Size popupSize = PopupSize;
			empty.X = m_Rect.Left - popupSize.Width;
			empty.Y = m_Rect.Top;
		}
		else if (ಟ == ePopupSide.Top)
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
		else if (ಟ == ePopupSide.Bottom)
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
		return _ಣ != ಢ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPopupOffset()
	{
		PopupOffset = ಢ;
	}

	protected internal override void OnBeforeItemRemoved(BaseItem objItem)
	{
		base.OnBeforeItemRemoved(objItem);
		if (ಕ != null && objItem != null)
		{
			ಕ.Items.Remove(objItem);
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem objItem, int itemIndex)
	{
		base.OnAfterItemRemoved(objItem, itemIndex);
		if (Expanded && objItem == null)
		{
			Expanded = false;
		}
		if (ಔ != null && !SuspendLayout)
		{
			ಔ.RecalcSize();
		}
		else if (ಕ != null && !SuspendLayout)
		{
			ಕ.RecalcLayout();
		}
	}

	protected internal override bool IsAnyOnHandle(IntPtr iHandle)
	{
		bool flag = base.IsAnyOnHandle(iHandle);
		if (!flag && ಖ != null && ((Control)ಖ).Handle == iHandle)
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
		if (ಔ != null)
		{
			return ಔ.GetInsertPosition(pScreen, DragItem);
		}
		if (ಕ != null)
		{
			return ((IDesignTimeProvider)ಕ.ItemsContainer).GetInsertPosition(pScreen, DragItem);
		}
		return null;
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
		if (Expanded)
		{
			if (ಔ != null)
			{
				ಔ.DrawReversibleMarker(iPos, Before);
			}
			else if (ಕ != null)
			{
				((IDesignTimeProvider)ಕ.Items).DrawReversibleMarker(iPos, Before);
			}
		}
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		if (Expanded)
		{
			if (ಔ != null)
			{
				ಔ.InsertItemAt(objItem, iPos, Before);
			}
			else
			{
				((IDesignTimeProvider)ಕ.Items).InsertItemAt(objItem, iPos, Before);
			}
		}
	}

	public void SetSourceControl(Control ctrl)
	{
		ಞ = ctrl;
	}

	protected override void OnDisplayedChanged()
	{
		if (!Displayed && Expanded)
		{
			Expanded = false;
		}
	}
}
