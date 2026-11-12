using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public class ExplorerBar : Control, IOwnerItemEvents, IOwner, IThemeCache, IMessageHandlerClient, IOwnerMenuSupport, ISupportInitialize, IBarDesignerServices, ICustomSerialization, IAccessibilitySupport
{
	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

	public class ExplorerBarAccessibleObject : AccessibleObject
	{
		private ExplorerBar m_Owner;

		public override string Name
		{
			get
			{
				return ((Control)m_Owner).AccessibleName;
			}
			set
			{
				((Control)m_Owner).AccessibleName = value;
			}
		}

		public override string Description => ((Control)m_Owner).AccessibleDescription;

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

		public ExplorerBarAccessibleObject(ExplorerBar owner)
		{
			m_Owner = owner;
		}

		internal void GenerateEvent(BaseItem sender, AccessibleEvents e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			int num = m_Owner.Groups.IndexOf(sender);
			if (num >= 0)
			{
				((Control)m_Owner).AccessibilityNotifyClients(e, num);
			}
		}

		public override int GetChildCount()
		{
			return m_Owner.Groups.Count;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			return m_Owner.Groups[iIndex].AccessibleObject;
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

	private ExplorerBarContainerItem m_ItemContainer;

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

	private eBorderType m_BorderStyle;

	private ExplorerBarGroupItem m_DelayedExpandedPanel;

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

	private VScrollBar m_ScrollBar;

	private int m_Margin = 4;

	private ItemStyleMapper m_BackgroundStyle;

	private ElementStyle m_BackStyle = new ElementStyle();

	private eExplorerBarStockStyle m_StockStyle = eExplorerBarStockStyle.Custom;

	private bool m_AnimationEnabled = true;

	private int m_AnimationTime = 100;

	private bool m_DispatchShortcuts;

	private bool m_FilterInstalled;

	private const string INFO_EMPTYEXPLORERBAR = "Right-click and choose Add New Group or use Groups collection to create new groups.";

	private bool m_MenuEventSupport;

	private bool m_VisualPropertyChanged;

	private ColorScheme m_ColorScheme;

	private bool m_AntiAlias = true;

	private IBarItemDesigner m_BarDesigner;

	private bool m_SuspendLayout;

	private Image m_GroupButtonExpandNormal;

	private Image m_GroupButtonExpandHot;

	private Image m_GroupButtonExpandPressed;

	private Image m_GroupButtonCollapseNormal;

	private Image m_GroupButtonCollapseHot;

	private Image m_GroupButtonCollapsePressed;

	private BaseItem m_DoDefaultActionItem;

	private bool m_DesignerSelection;

	private bool m_DisabledImagesGrayScale = true;

	private Hook m_Hook;

	private ArrayList m_RegisteredPopups = new ArrayList();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool DesignerSelection
	{
		get
		{
			return m_DesignerSelection;
		}
		set
		{
			if (m_DesignerSelection != value)
			{
				m_DesignerSelection = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool SuspendLayoutDisplay
	{
		get
		{
			return m_SuspendLayout;
		}
		set
		{
			m_SuspendLayout = value;
		}
	}

	[Editor(typeof(ColorSchemeVSEditor), typeof(UITypeEditor))]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets Color Scheme.")]
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
			m_VisualPropertyChanged = true;
			foreach (ExplorerBarGroupItem group in Groups)
			{
				group.VisualPropertyChanged();
			}
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	private bool IsTransparentBackground
	{
		get
		{
			if ((m_BackStyle.BackColor.IsEmpty && m_BackStyle.BackColor2.IsEmpty) || (m_BackStyle.BackColor == Color.Transparent && m_BackStyle.BackColor2 == Color.Transparent))
			{
				return true;
			}
			return false;
		}
	}

	protected bool IsThemed
	{
		get
		{
			if (m_ThemeAware && (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.OfficeXP || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003) && BarFunctions.ThemedOS && Themes.ThemesActive)
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
	[Description("Specifies whether ExplorerBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackStyle => m_BackStyle;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("This property is obsolete. Use BackStyle property instead")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ItemStyleMapper BackgroundStyle => m_BackgroundStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the vertical spacing between the group items.")]
	[DefaultValue(15)]
	public int GroupSpacing
	{
		get
		{
			return m_ItemContainer.m_ItemSpacing;
		}
		set
		{
			m_ItemContainer.m_ItemSpacing = value;
			RecalcLayout();
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

	[DefaultValue(true)]
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
				((Control)this).Refresh();
			}
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ExplorerBarPanelsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[Description("Returns the collection of Explorer Bar Groups.")]
	public SubItemsCollection Groups => m_ItemContainer.SubItems;

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

	[Browsable(true)]
	[DefaultValue(4)]
	[Category("Layout")]
	[Description("Indicates margin in pixels between the explorer bar groups and the edge of the control.")]
	public int ContentMargin
	{
		get
		{
			return m_Margin;
		}
		set
		{
			m_Margin = value;
			RecalcLayout();
		}
	}

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
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Specifies whether animation is enabled.")]
	public bool AnimationEnabled
	{
		get
		{
			return m_AnimationEnabled;
		}
		set
		{
			m_AnimationEnabled = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(100)]
	[Category("Behavior")]
	[Description("Indicates maximum animation time in milliseconds, default value is 100.")]
	public int AnimationTime
	{
		get
		{
			return m_AnimationTime;
		}
		set
		{
			if (m_AnimationTime > 0)
			{
				m_AnimationTime = value;
			}
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
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(eExplorerBarStockStyle.Custom)]
	[Description("Applies the stock style to the object.")]
	public eExplorerBarStockStyle StockStyle
	{
		get
		{
			return m_StockStyle;
		}
		set
		{
			BarFunctions.SetExplorerBarStyle(this, m_StockStyle);
			foreach (BaseItem subItem in m_ItemContainer.SubItems)
			{
				if (subItem is ExplorerBarGroupItem && ((ExplorerBarGroupItem)subItem).StockStyle != eExplorerBarStockStyle.Custom)
				{
					((ExplorerBarGroupItem)subItem).StockStyle = value;
				}
			}
			m_StockStyle = value;
			((Control)this).Refresh();
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
	[Category("Group Expand Button")]
	[DefaultValue(null)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
	public Image GroupButtonExpandNormal
	{
		get
		{
			return m_GroupButtonExpandNormal;
		}
		set
		{
			m_GroupButtonExpandNormal = value;
			((Control)this).Refresh();
		}
	}

	[Browsable(true)]
	[Category("Group Expand Button")]
	[DefaultValue(null)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
	public Image GroupButtonExpandHot
	{
		get
		{
			return m_GroupButtonExpandHot;
		}
		set
		{
			m_GroupButtonExpandHot = value;
		}
	}

	[Browsable(true)]
	[Category("Group Expand Button")]
	[DefaultValue(null)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
	public Image GroupButtonExpandPressed
	{
		get
		{
			return m_GroupButtonExpandPressed;
		}
		set
		{
			m_GroupButtonExpandPressed = value;
		}
	}

	[Browsable(true)]
	[Category("Group Expand Button")]
	[DefaultValue(null)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
	public Image GroupButtonCollapseNormal
	{
		get
		{
			return m_GroupButtonCollapseNormal;
		}
		set
		{
			m_GroupButtonCollapseNormal = value;
			((Control)this).Refresh();
		}
	}

	[Browsable(true)]
	[Category("Group Expand Button")]
	[DefaultValue(null)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
	public Image GroupButtonCollapseHot
	{
		get
		{
			return m_GroupButtonCollapseHot;
		}
		set
		{
			m_GroupButtonCollapseHot = value;
		}
	}

	[Browsable(true)]
	[Category("Group Expand Button")]
	[DefaultValue(null)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
	public Image GroupButtonCollapsePressed
	{
		get
		{
			return m_GroupButtonCollapsePressed;
		}
		set
		{
			m_GroupButtonCollapsePressed = value;
		}
	}

	[Browsable(true)]
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
	[Category("Data")]
	[Description("ImageList for images displayed on the Group Item.")]
	public ImageList GroupImages
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

	ImageList IOwner.ImagesMedium
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

	ImageList IOwner.ImagesLarge
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

	bool IOwner.DesignMode => ((Component)this).DesignMode;

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
	public ExplorerBarContainerItem ItemsContainer => m_ItemContainer;

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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
			((Control)this).Refresh();
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

	public event OptionGroupChangingEventHandler OptionGroupChanging;

	public event EventHandler ToolTipShowing;

	public event SerializeItemEventHandler SerializeItem;

	public event SerializeItemEventHandler DeserializeItem;

	public event EventHandler ItemLayoutUpdated;

	public ExplorerBar()
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		m_ItemContainer = new ExplorerBarContainerItem();
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
		((Control)this).SetStyle((ControlStyles)1, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).TabStop = true;
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
		ApplyDefaultSettings();
		m_BackStyle.StyleChanged += VisualPropertyChanged;
		((Control)this).AccessibleRole = (AccessibleRole)22;
		m_BackgroundStyle = new ItemStyleMapper(m_BackStyle);
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
			if (m_ItemContainer != null)
			{
				m_ItemContainer.Dispose();
			}
			m_ItemContainer = null;
			DisposeCursor(ref m_MoveCursor);
			DisposeCursor(ref m_CopyCursor);
			DisposeCursor(ref m_NACursor);
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_GroupButtonCollapseHot);
			BarUtilities.DisposeImage(ref m_GroupButtonCollapseNormal);
			BarUtilities.DisposeImage(ref m_GroupButtonCollapsePressed);
			BarUtilities.DisposeImage(ref m_GroupButtonExpandHot);
			BarUtilities.DisposeImage(ref m_GroupButtonExpandNormal);
			BarUtilities.DisposeImage(ref m_GroupButtonExpandPressed);
		}
		((Control)this).Dispose(disposing);
	}

	private void DisposeCursor(ref Cursor cursor)
	{
		if (cursor != (Cursor)null)
		{
			cursor.Dispose();
			cursor = null;
		}
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
		return (AccessibleObject)(object)new ExplorerBarAccessibleObject(this);
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		m_VisualPropertyChanged = true;
		ColorScheme colorScheme = ColorScheme;
		m_BackStyle.SetColorScheme(colorScheme);
		if (IsTransparentBackground)
		{
			((Control)this).BackColor = Color.Transparent;
		}
		else
		{
			((Control)this).BackColor = SystemColors.Control;
		}
		if (((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
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
				if (focusItem is ExplorerBarGroupItem)
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
		((Control)this).OnMouseWheel(e);
		if (m_ScrollBar == null)
		{
			return;
		}
		int num = Math.Abs(e.Delta) / 120 * ((ScrollBar)m_ScrollBar).SmallChange;
		if (e.Delta < 0)
		{
			int num2 = ((ScrollBar)m_ScrollBar).Value + num;
			if (num2 > ((ScrollBar)m_ScrollBar).Maximum)
			{
				num2 = ((ScrollBar)m_ScrollBar).Maximum;
			}
			((ScrollBar)m_ScrollBar).Value = num2;
		}
		else
		{
			int num3 = ((ScrollBar)m_ScrollBar).Value - num;
			if (num3 < 0)
			{
				num3 = 0;
			}
			((ScrollBar)m_ScrollBar).Value = num3;
		}
		UpdateScrollValue(((ScrollBar)m_ScrollBar).Value);
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
		foreach (ExplorerBarGroupItem subItem in m_ItemContainer.SubItems)
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
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
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
		RecalcLayout();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return m_ColorScheme.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
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

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		if (m_ItemContainer != null && !((Control)this).IsDisposed && !m_SuspendLayout)
		{
			SolidBrush val = new SolidBrush(Color.White);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Graphics graphics = e.Graphics;
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
			if (m_AntiAlias)
			{
				e.Graphics.SmoothingMode = (SmoothingMode)4;
				e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			graphics.PageUnit = (GraphicsUnit)2;
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			ColorScheme colorScheme = ColorScheme;
			if (m_VisualPropertyChanged)
			{
				m_BackStyle.SetColorScheme(colorScheme);
				m_VisualPropertyChanged = false;
			}
			Rectangle bounds = clientRectangle;
			bounds.Inflate(1, 1);
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(m_BackStyle, graphics, bounds);
			ElementStyleDisplay.Paint(e2);
			if (m_BorderStyle == eBorderType.SingleLine)
			{
				clientRectangle.Inflate(-1, -1);
			}
			else if (m_BorderStyle != 0)
			{
				clientRectangle.Inflate(-2, -2);
			}
			if (IsTransparentBackground)
			{
				((Control)this).OnPaintBackground(e);
			}
			Region clip = e.Graphics.Clip;
			e.Graphics.SetClip(clientRectangle);
			ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, e.Graphics, colorScheme);
			itemPaintArgs.Renderer = GetRenderer();
			itemPaintArgs.DesignerSelection = m_DesignerSelection;
			itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && WinApi.IsGlassEnabled;
			m_ItemContainer.Paint(itemPaintArgs);
			if (clip != null)
			{
				e.Graphics.SetClip(clip, (CombineMode)0);
			}
			else
			{
				e.Graphics.ResetClip();
			}
			clip.Dispose();
			if (m_ItemContainer.SubItems.Count == 0 && ((Component)this).DesignMode)
			{
				string text = "Right-click and choose Add New Group or use Groups collection to create new groups.";
				Rectangle clientRectangle2 = ((Control)this).ClientRectangle;
				clientRectangle.Inflate(-2, -2);
				eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				Font val2 = new Font(((Control)this).Font.FontFamily, 7f);
				TextDrawing.DrawString(graphics, text, val2, SystemColors.ControlText, clientRectangle2, format);
				val2.Dispose();
			}
			graphics.SmoothingMode = smoothingMode;
			graphics.TextRenderingHint = textRenderingHint;
		}
	}

	private BaseRenderer GetRenderer()
	{
		return GlobalManager.Renderer;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ApplyDefaultSettings()
	{
		ColorScheme colorScheme = ColorScheme;
		m_BackStyle.BackColorSchemePart = eColorSchemePart.ExplorerBarBackground;
		m_BackStyle.BackColor2SchemePart = eColorSchemePart.ExplorerBarBackground2;
		m_BackStyle.BackColorGradientAngle = colorScheme.ExplorerBarBackgroundGradientAngle;
		m_BackStyle.SetColorScheme(colorScheme);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		RefreshThemes();
		ColorScheme.Refresh(null, bSystemColorEvent: true);
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
		if (m_ThemeExplorerBar != null)
		{
			m_ThemeExplorerBar.Dispose();
			m_ThemeExplorerBar = new ThemeExplorerBar((Control)(object)this);
		}
		if (m_ThemeProgress != null)
		{
			m_ThemeProgress.Dispose();
			m_ThemeProgress = new ThemeProgress((Control)(object)this);
		}
		if (m_ThemeButton != null)
		{
			m_ThemeButton.Dispose();
			m_ThemeButton = new ThemeButton((Control)(object)this);
		}
		SyncGroupColors();
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
		if (m_ThemeButton != null)
		{
			m_ThemeButton.Dispose();
			m_ThemeButton = null;
		}
	}

	private void SyncGroupColors()
	{
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (subItem is ExplorerBarGroupItem { StockStyle: eExplorerBarStockStyle.SystemColors } explorerBarGroupItem)
			{
				explorerBarGroupItem.StockStyle = eExplorerBarStockStyle.SystemColors;
			}
		}
	}

	private eExplorerBarStockStyle GetSpecialStyle(eExplorerBarStockStyle style)
	{
		eExplorerBarStockStyle result = eExplorerBarStockStyle.Custom;
		switch (style)
		{
		case eExplorerBarStockStyle.Blue:
			result = eExplorerBarStockStyle.BlueSpecial;
			break;
		case eExplorerBarStockStyle.OliveGreen:
			result = eExplorerBarStockStyle.OliveGreenSpecial;
			break;
		case eExplorerBarStockStyle.Silver:
			result = eExplorerBarStockStyle.SilverSpecial;
			break;
		case eExplorerBarStockStyle.BlueSpecial:
		case eExplorerBarStockStyle.OliveGreenSpecial:
		case eExplorerBarStockStyle.SilverSpecial:
			result = style;
			break;
		}
		return result;
	}

	private bool IsStyleSpecial(eExplorerBarStockStyle style)
	{
		if (style == eExplorerBarStockStyle.BlueSpecial || style == eExplorerBarStockStyle.OliveGreenSpecial || style == eExplorerBarStockStyle.SilverSpecial)
		{
			return true;
		}
		return false;
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

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (((Control)this).Visible)
		{
			SyncGroupColors();
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (!m_SuspendLayout && BarFunctions.IsHandleValid((Control)(object)this))
		{
			if ((int)((Control)this).RightToLeft == 1)
			{
				m_ItemContainer.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
			}
			if (m_BorderStyle == eBorderType.None)
			{
				m_ItemContainer.LeftInternal = m_Margin;
				m_ItemContainer.TopInternal = m_Margin;
				m_ItemContainer.WidthInternal = ((Control)this).Width - m_Margin * 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - m_Margin;
			}
			else
			{
				m_ItemContainer.LeftInternal = m_Margin;
				m_ItemContainer.TopInternal = m_Margin;
				m_ItemContainer.WidthInternal = ((Control)this).Width - 4 - m_Margin * 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - m_Margin - 2;
			}
			m_ItemContainer.RecalcSize();
			if (m_ItemContainer.HeightInternal > ((Control)this).ClientRectangle.Height || (m_ItemContainer.HeightInternal <= ((Control)this).ClientRectangle.Height && m_ScrollBar != null))
			{
				ContainerSizeChanged();
			}
			OnItemLayoutUpdated(EventArgs.Empty);
		}
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		this.ItemLayoutUpdated?.Invoke(this, e);
	}

	internal void ContainerSizeChanged()
	{
		if (m_ItemContainer.HeightInternal > ((Control)this).ClientRectangle.Height)
		{
			CreateScrollBar();
			if (m_BorderStyle == eBorderType.None)
			{
				m_ItemContainer.LeftInternal = m_Margin;
				m_ItemContainer.TopInternal = m_Margin;
				m_ItemContainer.WidthInternal = ((Control)this).Width - ((Control)m_ScrollBar).Width - m_Margin * 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - m_Margin;
			}
			else
			{
				m_ItemContainer.LeftInternal = m_Margin;
				m_ItemContainer.TopInternal = m_Margin;
				m_ItemContainer.WidthInternal = ((Control)this).Width - 4 - ((Control)m_ScrollBar).Width - m_Margin * 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - m_Margin - 2;
			}
			m_ItemContainer.RecalcSize();
			CreateScrollBar();
			m_ItemContainer.TopInternal = -((ScrollBar)m_ScrollBar).Value + m_Margin;
		}
		else if (m_ScrollBar != null)
		{
			((Control)this).Controls.Remove((Control)(object)m_ScrollBar);
			((Component)(object)m_ScrollBar).Dispose();
			m_ScrollBar = null;
			if (m_BorderStyle == eBorderType.None)
			{
				m_ItemContainer.LeftInternal = m_Margin;
				m_ItemContainer.TopInternal = m_Margin;
				m_ItemContainer.WidthInternal = ((Control)this).Width - m_Margin * 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - m_Margin;
			}
			else
			{
				m_ItemContainer.LeftInternal = m_Margin;
				m_ItemContainer.TopInternal = m_Margin;
				m_ItemContainer.WidthInternal = ((Control)this).Width - 4 - m_Margin * 2;
				m_ItemContainer.HeightInternal = ((Control)this).Height - m_Margin - 2;
			}
			m_ItemContainer.RecalcSize();
		}
	}

	private void CreateScrollBar()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		if (m_ScrollBar == null)
		{
			m_ScrollBar = new VScrollBar();
			((Control)m_ScrollBar).Width = SystemInformation.VerticalScrollBarWidth;
			((Control)m_ScrollBar).Dock = (DockStyle)4;
			((Control)m_ScrollBar).Visible = true;
			((Control)this).Controls.Add((Control)(object)m_ScrollBar);
			((ScrollBar)m_ScrollBar).Scroll += new ScrollEventHandler(ScrollBarValueChanged);
			((Component)(object)m_ScrollBar).Site = null;
		}
		((ScrollBar)m_ScrollBar).Maximum = m_ItemContainer.HeightInternal;
		((ScrollBar)m_ScrollBar).LargeChange = ((Control)this).Height / 2;
		((ScrollBar)m_ScrollBar).SmallChange = ((ScrollBar)m_ScrollBar).LargeChange / 5;
		int num = m_ItemContainer.HeightInternal - ((Control)this).Height + ((ScrollBar)m_ScrollBar).LargeChange + ((ScrollBar)m_ScrollBar).SmallChange;
		if (((ScrollBar)m_ScrollBar).SmallChange > 0 && Math.Ceiling((float)num / (float)((ScrollBar)m_ScrollBar).SmallChange) > (double)(num / ((ScrollBar)m_ScrollBar).SmallChange))
		{
			num = (int)Math.Ceiling((float)num / (float)((ScrollBar)m_ScrollBar).SmallChange) * ((ScrollBar)m_ScrollBar).SmallChange;
		}
		if (((ScrollBar)m_ScrollBar).Maximum != num)
		{
			((ScrollBar)m_ScrollBar).Maximum = num;
		}
	}

	private void ScrollBarValueChanged(object sender, ScrollEventArgs e)
	{
		UpdateScrollValue(e.NewValue);
	}

	private void UpdateScrollValue(int iScrollValue)
	{
		m_ItemContainer.TopInternal = -iScrollValue + m_Margin;
		((Control)this).Invalidate(((Control)this).ClientRectangle, false);
	}

	public void RecalcLayout()
	{
		if (!m_SuspendLayout)
		{
			RecalcSize();
			((Control)this).Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		m_ItemContainer.SetDesignMode(b: true);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (((Component)this).DesignMode)
		{
			SetDesignMode();
		}
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
		if (Images != null || ((IOwner)this).ImagesLarge != null || ((IOwner)this).ImagesMedium != null)
		{
			foreach (ExplorerBarGroupItem subItem in m_ItemContainer.SubItems)
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
		RecalcLayout();
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
		if (((Component)this).DesignMode && Groups.Contains(objFocusItem))
		{
			foreach (BaseItem group in Groups)
			{
				BaseItem.CollapseSubItems(group);
			}
		}
		m_FocusItem = objFocusItem;
		if (m_FocusItem != null)
		{
			m_FocusItem.OnGotFocus();
		}
		if (!((Component)this).DesignMode)
		{
			EnsureVisible(m_FocusItem);
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

	public void EnsureVisible(BaseItem item)
	{
		if (item == null || item.ContainerControl != this)
		{
			return;
		}
		if (item.Parent is ExplorerBarGroupItem)
		{
			if (!item.Parent.Visible)
			{
				return;
			}
			if (!item.Parent.Expanded)
			{
				item.Parent.Expanded = true;
			}
		}
		if (m_ScrollBar == null || !((Control)m_ScrollBar).Visible)
		{
			return;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Rectangle rect = item.DisplayRectangle;
		if (item is ExplorerBarGroupItem)
		{
			rect = ((ExplorerBarGroupItem)item).PanelRect;
			rect.Y = item.TopInternal;
		}
		if (clientRectangle.Contains(rect))
		{
			return;
		}
		int num = 0;
		if (rect.Top < 0)
		{
			num = ((ScrollBar)m_ScrollBar).Value + rect.Top - m_Margin;
			if (num < 0)
			{
				num = 0;
			}
		}
		else
		{
			num = Math.Abs(m_ItemContainer.TopInternal) + rect.Bottom - clientRectangle.Height + m_Margin;
		}
		((ScrollBar)m_ScrollBar).Value = num;
		UpdateScrollValue(((ScrollBar)m_ScrollBar).Value);
	}

	void ISupportInitialize.BeginInit()
	{
	}

	void ISupportInitialize.EndInit()
	{
		if (Images != null)
		{
			foreach (BaseItem subItem in m_ItemContainer.SubItems)
			{
				FireOnImageChanged(subItem as ImageItem);
			}
		}
		RecalcLayout();
	}

	private void FireOnImageChanged(ImageItem item)
	{
		if (item == null)
		{
			return;
		}
		item.OnImageChanged();
		foreach (BaseItem subItem in item.SubItems)
		{
			FireOnImageChanged(subItem as ImageItem);
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

	public BaseItem HitTest(int clientX, int clientY)
	{
		BaseItem result = null;
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (!subItem.DisplayRectangle.Contains(clientX, clientY))
			{
				continue;
			}
			result = subItem;
			foreach (BaseItem subItem2 in subItem.SubItems)
			{
				if (subItem2.DisplayRectangle.Contains(clientX, clientY))
				{
					result = subItem2;
					break;
				}
			}
		}
		return result;
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 1131 && m_DoDefaultActionItem != null)
		{
			m_DoDefaultActionItem.DoAccesibleDefaultAction();
			m_DoDefaultActionItem = null;
		}
		((Control)this).WndProc(ref m);
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
		XmlElement xmlElement = xmlDoc.CreateElement("ExplorerBar");
		xmlDoc.AppendChild(xmlElement);
		Serialize(xmlElement);
	}

	internal void Serialize(XmlElement xmlThisBar)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected I4, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		xmlThisBar.SetAttribute("name", ((Control)this).Name);
		if (((Control)this).Font != null && (((Control)this).Font.Name != SystemInformation.MenuFont.Name || ((Control)this).Font.Size != SystemInformation.MenuFont.Size || ((Control)this).Font.Style != SystemInformation.MenuFont.Style))
		{
			xmlThisBar.SetAttribute("fontname", ((Control)this).Font.Name);
			xmlThisBar.SetAttribute("fontemsize", XmlConvert.ToString(((Control)this).Font.Size));
			xmlThisBar.SetAttribute("fontstyle", XmlConvert.ToString((int)((Control)this).Font.Style));
		}
		xmlThisBar.SetAttribute("width", XmlConvert.ToString(((Control)this).Width));
		xmlThisBar.SetAttribute("height", XmlConvert.ToString(((Control)this).Height));
		if (!m_AnimationEnabled)
		{
			xmlThisBar.SetAttribute("animate", XmlConvert.ToString(m_AnimationEnabled));
		}
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
		XmlElement xmlElement3 = xmlThisBar.OwnerDocument.CreateElement("backstyle");
		xmlThisBar.AppendChild(xmlElement3);
		SerializeElementStyle(m_BackStyle, xmlElement3);
	}

	private void SerializeElementStyle(ElementStyle style, XmlElement xmlElement)
	{
		ElementSerializer.Serialize(style, xmlElement);
	}

	private void DeserializeElementStyle(ElementStyle style, XmlElement xmlElement)
	{
		ElementSerializer.Deserialize(style, xmlElement);
	}

	private bool ProcessShortcut(eShortcut key)
	{
		bool flag = BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
		return !m_DispatchShortcuts && flag;
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

	bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
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

	internal void Deserialize(XmlElement xmlThisBar)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		m_ShortcutTable.Clear();
		((Control)this).Name = xmlThisBar.GetAttribute("name");
		((Control)this).Size = new Size(XmlConvert.ToInt32(xmlThisBar.GetAttribute("width")), XmlConvert.ToInt32(xmlThisBar.GetAttribute("height")));
		if (xmlThisBar.HasAttribute("animate"))
		{
			m_AnimationEnabled = XmlConvert.ToBoolean(xmlThisBar.GetAttribute("animate"));
		}
		else
		{
			m_AnimationEnabled = true;
		}
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
			string name = childNode.Name;
			if (!(name == "items"))
			{
				if (name == "backstyle")
				{
					DeserializeElementStyle(m_BackStyle, childNode);
					m_VisualPropertyChanged = true;
				}
				continue;
			}
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				BaseItem baseItem = BarFunctions.CreateItemFromXml(childNode2);
				m_ItemContainer.SubItems.Add(baseItem);
				itemSerializationContext.ItemXmlElement = childNode2;
				baseItem.Deserialize(itemSerializationContext);
			}
		}
	}

	public void LoadDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		LoadDefinition(xmlDocument);
		((Control)this).Refresh();
	}

	internal void LoadDefinition(XmlDocument xmlDoc)
	{
		if (xmlDoc.FirstChild.Name != "ExplorerBar")
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
}
