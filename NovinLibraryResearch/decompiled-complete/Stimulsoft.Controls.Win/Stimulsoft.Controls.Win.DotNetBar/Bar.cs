using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using Microsoft.Win32;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(Bar), "Bar.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
[DefaultEvent("ItemClick")]
public class Bar : Control, IDockInfo, IBarImageSize, IOwner, IOwnerMenuSupport, IMessageHandlerClient, ISupportInitialize, IBarDesignerServices, ICustomSerialization, IRenderingSupport, IAccessibilitySupport, IOwnerLocalize
{
	private class System_Buttons
	{
		public Rectangle CloseButtonRect;

		public Rectangle CustomizeButtonRect;

		public Rectangle AutoHideButtonRect;

		public Rectangle CaptionButtonRect;

		public Size ButtonSize = new Size((int)(14.0 * StiScale2.Step), (int)(14.0 * StiScale2.Step));

		private bool m_MouseOverClose;

		private bool m_MouseOverCustomize;

		private bool m_MouseDownClose;

		private bool m_MouseDownCustomize;

		private bool m_MouseOverAutoHide;

		private bool m_MouseDownAutoHide;

		private bool m_MouseOverCaption;

		private bool m_MouseDownCaption;

		public bool MouseOverClose
		{
			get
			{
				return m_MouseOverClose;
			}
			set
			{
				if (m_MouseOverClose != value)
				{
					m_MouseOverClose = value;
					if (this.MouseOverCloseChanged != null)
					{
						this.MouseOverCloseChanged(this, new EventArgs());
					}
				}
			}
		}

		public bool MouseOverCaption
		{
			get
			{
				return m_MouseOverCaption;
			}
			set
			{
				if (m_MouseOverCaption != value)
				{
					m_MouseOverCaption = value;
					if (this.MouseOverCaptionChanged != null)
					{
						this.MouseOverCaptionChanged(this, new EventArgs());
					}
				}
			}
		}

		public bool MouseOverCustomize
		{
			get
			{
				return m_MouseOverCustomize;
			}
			set
			{
				if (m_MouseOverCustomize != value)
				{
					m_MouseOverCustomize = value;
					if (this.MouseOverCustomizeChanged != null)
					{
						this.MouseOverCustomizeChanged(this, new EventArgs());
					}
				}
			}
		}

		public bool MouseDownClose
		{
			get
			{
				return m_MouseDownClose;
			}
			set
			{
				if (m_MouseDownClose != value)
				{
					m_MouseDownClose = value;
					if (this.MouseDownCloseChanged != null)
					{
						this.MouseDownCloseChanged(this, new EventArgs());
					}
				}
			}
		}

		public bool MouseDownCaption
		{
			get
			{
				return m_MouseDownCaption;
			}
			set
			{
				if (m_MouseDownCaption != value)
				{
					m_MouseDownCaption = value;
					if (this.MouseDownCaptionChanged != null)
					{
						this.MouseDownCaptionChanged(this, new EventArgs());
					}
				}
			}
		}

		public bool MouseDownCustomize
		{
			get
			{
				return m_MouseDownCustomize;
			}
			set
			{
				if (m_MouseDownCustomize != value)
				{
					m_MouseDownCustomize = value;
					if (this.MouseDownCustomizeChanged != null)
					{
						this.MouseDownCustomizeChanged(this, new EventArgs());
					}
				}
			}
		}

		public bool MouseOverAutoHide
		{
			get
			{
				return m_MouseOverAutoHide;
			}
			set
			{
				if (m_MouseOverAutoHide != value)
				{
					m_MouseOverAutoHide = value;
					if (this.MouseOverAutoHideChanged != null)
					{
						this.MouseOverAutoHideChanged(this, new EventArgs());
					}
				}
			}
		}

		public bool MouseDownAutoHide
		{
			get
			{
				return m_MouseDownAutoHide;
			}
			set
			{
				if (m_MouseDownAutoHide != value)
				{
					m_MouseDownAutoHide = value;
					if (this.MouseDownAutoHideChanged != null)
					{
						this.MouseDownAutoHideChanged(this, new EventArgs());
					}
				}
			}
		}

		public event EventHandler MouseOverCloseChanged;

		public event EventHandler MouseOverCustomizeChanged;

		public event EventHandler MouseOverAutoHideChanged;

		public event EventHandler MouseDownCloseChanged;

		public event EventHandler MouseDownCustomizeChanged;

		public event EventHandler MouseDownAutoHideChanged;

		public event EventHandler MouseOverCaptionChanged;

		public event EventHandler MouseDownCaptionChanged;

		public System_Buttons(Rectangle closerect, Rectangle custrect, bool mouseoverclose, bool mouseovercustomize, bool mousednclose, bool mousedncust)
		{
			CloseButtonRect = closerect;
			CustomizeButtonRect = custrect;
			MouseOverClose = mouseoverclose;
			MouseOverCustomize = mouseovercustomize;
			MouseDownClose = mousednclose;
			MouseDownCustomize = mousedncust;
			MouseOverAutoHide = false;
			MouseDownAutoHide = false;
			AutoHideButtonRect = Rectangle.Empty;
		}
	}

	private struct ThemeMargin
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;

		public bool IsEmpty
		{
			get
			{
				if (Left == 0 && Top == 0 && Right == 0)
				{
					return Bottom == 0;
				}
				return false;
			}
		}
	}

	public class BarAccessibleObject : ControlAccessibleObject
	{
		private Bar m_Owner;

		public override AccessibleRole Role
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				if (m_Owner == null || ((Control)m_Owner).IsDisposed)
				{
					return (AccessibleRole)0;
				}
				return ((Control)m_Owner).AccessibleRole;
			}
		}

		public override AccessibleObject Parent
		{
			get
			{
				if (m_Owner != null && ((Control)m_Owner).Parent is DockSite)
				{
					return ((Control)m_Owner).Parent.AccessibilityObject;
				}
				return ((ControlAccessibleObject)this).Parent;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (m_Owner != null && !((Control)m_Owner).IsDisposed && ((Control)m_Owner).Parent != null)
				{
					return ((Control)m_Owner).Parent.RectangleToScreen(((Control)m_Owner).Bounds);
				}
				return Rectangle.Empty;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				if (m_Owner == null || ((Control)m_Owner).IsDisposed)
				{
					return (AccessibleStates)0;
				}
				AccessibleStates val;
				if (m_Owner.GrabHandleStyle != 0 && m_Owner.GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
				{
					val = (AccessibleStates)262144;
					if (m_Owner.DockSide == eDockSide.None)
					{
						val = (AccessibleStates)(val | 0x1000);
					}
				}
				else
				{
					val = (AccessibleStates)0;
				}
				return val;
			}
		}

		public BarAccessibleObject(Bar owner)
			: base((Control)(object)owner)
		{
			m_Owner = owner;
		}

		internal void GenerateEvent(BaseItem sender, AccessibleEvents e)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			int num = m_Owner.Items.IndexOf(sender);
			if (num >= 0 && m_Owner != null && !((Control)m_Owner).IsDisposed)
			{
				((Control)m_Owner).AccessibilityNotifyClients(e, num);
			}
		}

		public override int GetChildCount()
		{
			if (m_Owner != null && !((Control)m_Owner).IsDisposed && m_Owner.Items != null)
			{
				return m_Owner.Items.Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (m_Owner != null && !((Control)m_Owner).IsDisposed && m_Owner.Items != null)
			{
				return m_Owner.Items[iIndex].AccessibleObject;
			}
			return null;
		}

		public override AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Invalid comparison between Unknown and I4
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Invalid comparison between Unknown and I4
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Invalid comparison between Unknown and I4
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Invalid comparison between Unknown and I4
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Invalid comparison between Unknown and I4
			if ((int)navdir == 7)
			{
				if (m_Owner.Items.Count > 0)
				{
					return m_Owner.Items[0].AccessibleObject;
				}
			}
			else if ((int)navdir == 8)
			{
				if (m_Owner.Items.Count > 0)
				{
					return m_Owner.Items[m_Owner.Items.Count - 1].AccessibleObject;
				}
			}
			else
			{
				if ((int)navdir == 5)
				{
					return null;
				}
				if ((int)navdir == 6)
				{
					return null;
				}
				if ((int)navdir == 2)
				{
					return null;
				}
				if ((int)navdir == 1)
				{
					return null;
				}
			}
			return ((AccessibleObject)this).Navigate(navdir);
		}
	}

	private const int WM_MOUSEACTIVATE = 33;

	private const int MA_NOACTIVATE = 3;

	private const int MA_NOACTIVATEANDEAT = 4;

	private const long WS_POPUP = 2147483648L;

	private const long WS_CLIPSIBLINGS = 67108864L;

	private const long WS_CLIPCHILDREN = 33554432L;

	private const long WS_EX_TOPMOST = 8L;

	private const long WS_EX_TOOLWINDOW = 128L;

	private const long WS_VISIBLE = 268435456L;

	private const int WM_WINDOWPOSCHANGING = 70;

	private const int WM_WINDOWPOSCHANGED = 71;

	private const int SIZE_W = 1;

	private const int SIZE_E = 2;

	private const int SIZE_N = 3;

	private const int SIZE_S = 4;

	private const int SIZE_NWN = 5;

	private const int SIZE_NWS = 6;

	private const int SIZE_NEN = 7;

	private const int SIZE_NES = 8;

	private const int SIZE_HSPLITRIGHT = 9;

	private const int SIZE_HSPLITLEFT = 10;

	private const int SIZE_VSPLITTOP = 11;

	private const int SIZE_VSPLITBOTTOM = 12;

	private const int SIZE_HSPLIT = 13;

	private const int SIZE_VSPLIT = 14;

	private const int SIZE_PARENTRESIZE = 15;

	private const int DRAGRECTANGLE_WIDTH = 3;

	private int GrabHandleDotNetWidth = (int)Math.Ceiling(7.0 * StiScale2.X);

	private int GrabHandleOfficeWidth = (int)Math.Ceiling(7.0 * StiScale2.X);

	private int GrabHandleResizeWidth = (int)Math.Ceiling(17.0 * StiScale2.X);

	private int GrabHandleTaskPaneHeight = (int)Math.Ceiling(23.0 * StiScale2.Y);

	private int GrabHandleCaptionHeight = (int)Math.Ceiling(20.0 * StiScale2.Y);

	private const int DOCKTABSTRIP_HEIGHT = 25;

	private BaseItem m_ParentItem;

	private Point m_ParentItemScreenPos;

	private object m_OldContainer;

	private Rectangle m_ClientRect;

	private SideBarImage m_SideBarImage;

	private Rectangle m_SideBarRect;

	private GenericItemContainer m_ItemContainer;

	private int m_InitialContainerWidth;

	private eBarState m_BarState;

	private eGrabHandleStyle m_GrabHandleStyle;

	private Rectangle m_GrabHandleRect;

	private int m_DockOffset;

	private int m_DockLine;

	private object m_Owner;

	private Point m_MouseDownPt;

	private Size m_MouseDownSize;

	private bool m_MoveWindow;

	private int m_DockTabTearOffIndex = -1;

	private Rectangle m_FloatingRect;

	private Size m_DockedSizeH = Size.Empty;

	private Size m_DockedSizeV = Size.Empty;

	private int m_SizeWindow;

	private DockSiteInfo m_DragDockInfo = new DockSiteInfo();

	private Rectangle m_LastDragRect;

	internal DockSiteInfo m_LastDockSiteInfo = new DockSiteInfo();

	private bool m_WrapItemsDock;

	private bool m_WrapItemsFloat;

	private bool m_MenuBar;

	private bool m_DockStretch;

	private FloatingContainer m_Float;

	private bool m_DockingInProgress;

	private IntPtr m_LastFocusWindow;

	private bool m_CanDockLeft = true;

	private bool m_CanDockRight = true;

	private bool m_CanDockTop = true;

	private bool m_CanDockBottom = true;

	private bool m_CanUndock;

	private bool m_CanTearOffTabs = true;

	private bool m_CanReorderTabs = true;

	private bool m_CanDockTab = true;

	private bool m_CanDockDocument;

	private bool m_CanHide;

	private bool m_CloseSingleTab;

	private bool m_AcceptDropItems = true;

	private eBorderType m_DockedBorder;

	private Color m_SingleLineColor = SystemColors.ControlDark;

	private bool m_CustomBar;

	private System_Buttons m_SystemButtons = new System_Buttons(Rectangle.Empty, Rectangle.Empty, mouseoverclose: false, mouseovercustomize: false, mousednclose: false, mousedncust: false);

	private ePopupAnimation m_PopupAnimation = ePopupAnimation.ManagerControlled;

	private PopupShadow m_DropShadow;

	private eBarImageSize m_ImageSize;

	private Color m_CaptionBackColor = Color.Empty;

	private Color m_CaptionForeColor = Color.Empty;

	private PopupItem m_CustomizeMenu;

	private TabStrip m_TabDockItems;

	private bool m_AutoHideState;

	private bool m_CanAutoHide = true;

	private bool m_HasFocus;

	private bool m_SystemPrefChanged;

	private bool m_CustomFont;

	private bool m_LockDockPosition;

	private ColorScheme m_ColorScheme;

	internal bool PassiveBar;

	private bool m_ThemeAware;

	private ThemeMargin m_ThemeWindowMargins;

	private ThemeWindow m_ThemeWindow;

	private ThemeRebar m_ThemeRebar;

	private ThemeToolbar m_ThemeToolbar;

	private ThemeHeader m_ThemeHeader;

	private ThemeScrollBar m_ThemeScrollBar;

	private ThemeProgress m_ThemeProgress;

	private int m_SplitDockWidthPercent;

	private int m_SplitDockHeightPercent;

	private Bar m_TempTabBar;

	private eTabStripAlignment m_DockTabAlignment = eTabStripAlignment.Bottom;

	private bool m_HideFloatingInactive = true;

	private Point m_ResizeOffset = Point.Empty;

	private bool m_TabNavigation;

	internal bool m_AccessibleObjectCreated;

	private bool m_BarDefinitionLoading;

	private bool m_ParentMsgHandlerRegistered;

	private BaseItem m_DoDefaultActionItem;

	private int m_AutoHideAnimationTime = 100;

	private eBackgroundImagePosition m_BackgroundImagePosition;

	private byte m_BackgroundImageAlpha = byte.MaxValue;

	private bool m_ShowToolTips = true;

	private bool m_IgnoreAnimation = true;

	protected ToolTip m_ToolTipWnd;

	private int m_DockSideDelayed = -1;

	private bool m_AnimationInProgress;

	private bool m_EnableRedraw = true;

	private bool m_AlwaysDisplayDockTab;

	private bool m_MenuEventSupport;

	private bool m_MenuFocus;

	private int m_BarShowIndex = -1;

	private bool m_AlwaysDisplayKeyAccelerators;

	private bool m_AutoCreateCaptionMenu = true;

	private PopupItem m_CaptionMenu;

	private bool m_AutoSyncBarCaption;

	private bool m_SaveLayoutChanges = true;

	private bool m_TabsRearranged;

	private IBarItemDesigner m_BarDesigner;

	private bool m_LoadingHideFloating;

	private bool m_DesignerSelection;

	internal Hashtable PropertyBag = new Hashtable();

	private int m_CornerSize = 3;

	private bool m_FadeEffect;

	private bool m_AntiAlias;

	private eBarType m_BarType;

	private bool m_RoundCorners = true;

	private bool m_AutoHideTextAlwaysVisible;

	private bool m_DockTabCloseButtonVisible;

	private bool hotDragDrop;

	public ButtonItem LatestMouseOverItem;

	internal bool IsDisposing;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private bool m_RecalculatingSize;

	private int _CaptionHeight;

	private bool m_InMouseMove;

	private Rectangle m_OldOutlineRectangle = Rectangle.Empty;

	private Form m_OutlineForm;

	private bool m_IsVisible = true;

	private bool m_LayoutSuspended;

	private bool m_DesignerParent;

	private bool m_AutoHideStateDelayed;

	private Component m_GlobalParentComponent;

	private int _DockTabStripHeight = (int)Math.Ceiling(25.0 * StiScale2.Y);

	private bool m_RefreshingDockTab;

	private bool _CanMove = true;

	private bool m_DisabledImagesGrayScale = true;

	private eDockSide _LastDockSidePrivate;

	private BaseItem m_ExpandedItem;

	private BaseItem m_FocusItem;

	private Hashtable m_ShortcutTable = new Hashtable();

	private ImageList m_ImageList;

	private ImageList m_ImageListMedium;

	private ImageList m_ImageListLarge;

	private bool _UseHook;

	private ArrayList m_RegisteredPopups = new ArrayList();

	private bool m_FilterInstalled;

	private Hook m_Hook;

	private bool m_DispatchShortcuts;

	private bool m_IgnoreSysKeyUp;

	private bool m_EatSysKeyUp;

	private bool m_IgnoreF10Key;

	private bool m_DragDropInProgress;

	private int m_InsertPosition = -1;

	private bool m_InsertBefore;

	private IDesignTimeProvider m_DesignTimeProvider;

	private BaseItem m_DragItem;

	private IDesignTimeProvider m_DragDropDesignTimeProvider;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool HotDragDrop
	{
		get
		{
			return hotDragDrop;
		}
		set
		{
			hotDragDrop = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object Owner
	{
		get
		{
			if (m_ParentItem is CustomizeItem)
			{
				return m_ParentItem.GetOwner();
			}
			if ((((Control)this).Parent != null && !(((Control)this).Parent is FloatingContainer) && !(((Control)this).Parent is DockSite) && !AutoHide && !m_DesignerParent) || (((Control)this).Parent == null && m_Owner == null))
			{
				return this;
			}
			return m_Owner;
		}
		set
		{
			m_Owner = value;
			if (((Component)this).DesignMode && ((Component)(object)this).Site != null && m_TabDockItems != null)
			{
				RefreshDockTab(bFireEvents: false);
			}
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			if (PassiveBar)
			{
				return createParams;
			}
			if (m_BarState == eBarState.Popup && (m_ParentItem == null || m_ParentItem.Site == null || !m_ParentItem.Site.DesignMode))
			{
				createParams.Style = -2046820352;
				createParams.ExStyle = 136;
			}
			createParams.Caption = "";
			return createParams;
		}
	}

	internal bool HasFocus => m_HasFocus;

	internal bool EnableRedraw
	{
		get
		{
			return m_EnableRedraw;
		}
		set
		{
			if (m_EnableRedraw != value && Visible)
			{
				m_EnableRedraw = value;
				if (m_EnableRedraw)
				{
					NativeFunctions.SendMessage(((Control)this).Handle, 11, 1, 0);
				}
				else
				{
					NativeFunctions.SendMessage(((Control)this).Handle, 11, 0, 0);
				}
			}
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Control LastFocusControl
	{
		get
		{
			if (((Control)this).Focused && m_LastFocusWindow != IntPtr.Zero)
			{
				return Control.FromChildHandle(m_LastFocusWindow);
			}
			return null;
		}
	}

	private bool IsGradientStyle
	{
		get
		{
			if (Style != eDotNetBarStyle.Office2003 && Style != eDotNetBarStyle.VS2005)
			{
				return BarFunctions.IsOffice2007Style(Style);
			}
			return true;
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

	private bool HasPostRender => this.PostRender != null;

	private bool GradientBackground
	{
		get
		{
			if (Style == eDotNetBarStyle.VS2005 && LayoutType == eLayoutType.DockContainer)
			{
				return false;
			}
			return true;
		}
	}

	internal bool ThemedBackground
	{
		get
		{
			if (IsThemed && m_ItemContainer.BackgroundColor.IsEmpty)
			{
				return true;
			}
			return false;
		}
	}

	internal bool DrawThemedCaption
	{
		get
		{
			if (BarFunctions.IsOffice2007Style(Style) || !ThemeAware)
			{
				return false;
			}
			if (IsThemed || (m_BarState == eBarState.Floating && BarFunctions.ThemedOS && Themes.ThemesActive && LayoutType == eLayoutType.DockContainer))
			{
				return true;
			}
			return false;
		}
	}

	internal BaseItem ParentInternal => m_ParentItem;

	[Browsable(false)]
	public BaseItem ParentItem
	{
		get
		{
			if (m_ParentItem != null)
			{
				return m_ParentItem;
			}
			return m_ItemContainer;
		}
		set
		{
			RestoreContainer();
			if (m_ItemContainer.SubItems.Count > 0)
			{
				m_ItemContainer.SubItems.Clear();
			}
			m_ParentItem = value;
			if (m_ParentItem == null || m_ParentItem.SubItems.Count == 0)
			{
				return;
			}
			m_ItemContainer.Style = m_ParentItem.Style;
			BaseItem baseItem = m_ParentItem.SubItems[0];
			if (baseItem != null)
			{
				m_OldContainer = baseItem.ContainerControl;
			}
			else
			{
				m_OldContainer = null;
			}
			m_ItemContainer.SubItems.AllowParentRemove = false;
			foreach (BaseItem subItem in m_ParentItem.SubItems)
			{
				subItem.ContainerControl = this;
				m_ItemContainer.SubItems.Add(subItem);
			}
			m_ItemContainer.SubItems.AllowParentRemove = true;
			if (m_ParentItem.Displayed)
			{
				object containerControl = m_ParentItem.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (BaseItem.IsHandleValid(val))
				{
					m_ParentItemScreenPos = val.PointToScreen(new Point(m_ParentItem.LeftInternal, m_ParentItem.TopInternal));
					val = null;
				}
			}
		}
	}

	internal int SplitDockHeight
	{
		get
		{
			if (((Control)this).Parent != null && m_SplitDockHeightPercent > 0)
			{
				int num = ((Control)this).Parent.Height * m_SplitDockHeightPercent / 1000;
				Size adjustedFullSize = GetAdjustedFullSize(MinimumDockSize(eOrientation.Horizontal));
				if (num < adjustedFullSize.Height)
				{
					num = adjustedFullSize.Height;
				}
				return num;
			}
			return 0;
		}
		set
		{
			if (((Control)this).Parent != null && value > 0)
			{
				if (value > ((Control)this).Parent.Height)
				{
					value = ((Control)this).Parent.Height;
				}
				m_SplitDockHeightPercent = (int)((float)value / (float)((Control)this).Parent.Height * 1000f);
			}
			else
			{
				m_SplitDockHeightPercent = 0;
			}
		}
	}

	internal int SplitDockWidth
	{
		get
		{
			if (((Control)this).Parent != null && m_SplitDockWidthPercent > 0)
			{
				return ((Control)this).Parent.Width * m_SplitDockWidthPercent / 1000;
			}
			return 0;
		}
		set
		{
			if (((Control)this).Parent != null && value > 0)
			{
				if (value > ((Control)this).Parent.Width)
				{
					value = ((Control)this).Parent.Width;
				}
				m_SplitDockWidthPercent = (int)((float)value / (float)((Control)this).Parent.Width * 1000f);
			}
			else
			{
				m_SplitDockWidthPercent = 0;
			}
		}
	}

	internal int MinHeight
	{
		get
		{
			if (m_ItemContainer.MinHeight > 0)
			{
				return m_ItemContainer.MinHeight + GetNonClientHeight();
			}
			return 0;
		}
		set
		{
			if (m_ItemContainer != null)
			{
				if (value > 0)
				{
					m_ItemContainer.MinHeight = value - GetNonClientHeight();
				}
				else
				{
					m_ItemContainer.MinHeight = 0;
				}
			}
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates docked bar caption height. Default value is 0 which means system predefined height is used.")]
	public int CaptionHeight
	{
		get
		{
			return _CaptionHeight;
		}
		set
		{
			_CaptionHeight = value;
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LoadingHideFloating
	{
		get
		{
			return m_LoadingHideFloating;
		}
		set
		{
			m_LoadingHideFloating = value;
		}
	}

	internal Bar TempTabBar => m_TempTabBar;

	internal bool IsBarMoving => m_MoveWindow;

	internal bool IsSizingWindow => m_SizeWindow != 0;

	internal bool IsVisible => m_IsVisible;

	internal ThemeWindow ThemeWindow
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

	internal ThemeRebar ThemeRebar
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

	internal ThemeToolbar ThemeToolbar
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

	internal ThemeHeader ThemeHeader
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

	internal ThemeScrollBar ThemeScrollBar
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

	internal ThemeProgress ThemeProgress
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

	private int ClientMarginLeft
	{
		get
		{
			int num = 0;
			if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				return 1;
			}
			return 3;
		}
	}

	private int ClientMarginTop
	{
		get
		{
			int num = 0;
			if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				return 2;
			}
			return 3;
		}
	}

	private int ClientMarginRight
	{
		get
		{
			bool flag = true;
			int num = 0;
			if (m_Owner is IOwnerMenuSupport { ShowPopupShadow: false })
			{
				flag = false;
			}
			if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				if (AlphaShadow || !flag)
				{
					return 1;
				}
				return 3;
			}
			return 3;
		}
	}

	private int ClientMarginBottom
	{
		get
		{
			bool flag = true;
			int num = 0;
			if (m_Owner is IOwnerMenuSupport { ShowPopupShadow: false })
			{
				flag = false;
			}
			if (m_ParentItem != null && m_ParentItem.EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				if (AlphaShadow || !flag)
				{
					return 2;
				}
				return 4;
			}
			return 3;
		}
	}

	internal bool DisplayShadow
	{
		get
		{
			if (PassiveBar)
			{
				return false;
			}
			if (m_Owner is IOwnerMenuSupport ownerMenuSupport)
			{
				if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					if (ownerMenuSupport.MenuDropShadow == eMenuDropShadow.Show)
					{
						return true;
					}
					return false;
				}
				return ownerMenuSupport.ShowPopupShadow;
			}
			if (m_ParentItem != null && m_ParentItem.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return false;
			}
			return true;
		}
	}

	internal bool AlphaShadow
	{
		get
		{
			if (Environment.OSVersion.Version.Major < 5)
			{
				return false;
			}
			if (m_Owner is IOwnerMenuSupport { AlphaBlendShadow: false })
			{
				return false;
			}
			return NativeFunctions.CursorShadow;
		}
	}

	internal bool MenuFocus
	{
		get
		{
			return m_MenuFocus;
		}
		set
		{
			if (m_MenuFocus == value)
			{
				return;
			}
			m_MenuFocus = value;
			if (m_MenuFocus)
			{
				m_ItemContainer.SetSystemFocus();
				if (m_Owner is DotNetBarManager dotNetBarManager)
				{
					dotNetBarManager.FocusedBar = this;
				}
			}
			else
			{
				if (m_ItemContainer != null)
				{
					m_ItemContainer.AutoExpand = false;
					m_ItemContainer.ReleaseSystemFocus();
					m_ItemContainer.ContainerLostFocus(appLostFocus: false);
				}
				if (m_Owner is DotNetBarManager dotNetBarManager2)
				{
					dotNetBarManager2.FocusedBar = null;
				}
			}
			if (MenuBar)
			{
				((Control)this).Refresh();
			}
		}
	}

	internal int PopupWidth
	{
		get
		{
			return m_InitialContainerWidth;
		}
		set
		{
			m_InitialContainerWidth = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(ePopupAnimation.ManagerControlled)]
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

	bool ICustomSerialization.HasSerializeItemHandlers
	{
		get
		{
			bool flag = this.SerializeItem != null;
			if (Owner != this && Owner is ICustomSerialization)
			{
				flag |= ((ICustomSerialization)Owner).HasSerializeItemHandlers;
			}
			return flag;
		}
	}

	bool ICustomSerialization.HasDeserializeItemHandlers
	{
		get
		{
			bool flag = this.DeserializeItem != null;
			if (Owner != this && Owner is ICustomSerialization)
			{
				flag |= ((ICustomSerialization)Owner).HasDeserializeItemHandlers;
			}
			return flag;
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

	private bool CanSaveMinWidth
	{
		get
		{
			eDockSide dockSide = DockSide;
			if (dockSide == eDockSide.None || dockSide == eDockSide.Left || dockSide == eDockSide.Right)
			{
				return true;
			}
			return false;
		}
	}

	private bool CanSaveMinHeight
	{
		get
		{
			eDockSide dockSide = DockSide;
			if (dockSide == eDockSide.None || dockSide == eDockSide.Top || dockSide == eDockSide.Bottom)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether layout changes are saved for this bar")]
	public bool SaveLayoutChanges
	{
		get
		{
			return m_SaveLayoutChanges;
		}
		set
		{
			m_SaveLayoutChanges = value;
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(eBackgroundImagePosition.Stretch)]
	[Description("Specifies background image position when container is larger than image.")]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return m_BackgroundImagePosition;
		}
		set
		{
			if (m_BackgroundImagePosition != value)
			{
				m_BackgroundImagePosition = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(byte.MaxValue)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the transparency of background image.")]
	public byte BackgroundImageAlpha
	{
		get
		{
			return m_BackgroundImageAlpha;
		}
		set
		{
			if (m_BackgroundImageAlpha != value)
			{
				m_BackgroundImageAlpha = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SideBarImage SideBar
	{
		get
		{
			return m_SideBarImage;
		}
		set
		{
			m_SideBarImage = value;
		}
	}

	[DevCoBrowsable(true)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			if (((Control)this).Text != value)
			{
				((Control)this).Text = value;
				if (Visible && (m_BarState == eBarState.Floating || ((m_BarState == eBarState.Docked || m_BarState == eBarState.AutoHide) && (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted))))
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	public override Font Font
	{
		get
		{
			return ((Control)this).Font;
		}
		set
		{
			((Control)this).Font = value;
			if (((Control)this).Font != null)
			{
				m_CustomFont = ShouldSerializeFont();
			}
		}
	}

	protected internal bool IsThemed
	{
		get
		{
			if (m_ThemeAware && Style != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && Themes.ThemesActive)
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
		}
	}

	[Browsable(false)]
	public eBarState BarState => m_BarState;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items
	{
		get
		{
			if (m_ItemContainer == null)
			{
				return null;
			}
			return m_ItemContainer.SubItems;
		}
	}

	[Browsable(false)]
	public GenericItemContainer ItemsContainer => m_ItemContainer;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the items that could not be displayed on the non-wrap Bar are displayed on popup menu or popup Bar.")]
	[DefaultValue(false)]
	public bool DisplayMoreItemsOnMenu
	{
		get
		{
			return m_ItemContainer.MoreItemsOnMenu;
		}
		set
		{
			m_ItemContainer.MoreItemsOnMenu = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the spacing in pixels between the sub-items.")]
	[DefaultValue(0)]
	public int ItemSpacing
	{
		get
		{
			return m_ItemContainer.ItemSpacing;
		}
		set
		{
			if (m_ItemContainer.ItemSpacing != value)
			{
				m_ItemContainer.ItemSpacing = value;
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets/Sets the padding in pixels. This represents the spacing between the top of the bar and the top of the item.")]
	[DefaultValue(1)]
	public int PaddingTop
	{
		get
		{
			return m_ItemContainer.PaddingTop;
		}
		set
		{
			if (m_ItemContainer.PaddingTop != value)
			{
				m_ItemContainer.PaddingTop = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets/Sets the padding in pixels. This represents the spacing between the bottom edge of the bar and the bottom of the item.")]
	[DefaultValue(1)]
	public int PaddingBottom
	{
		get
		{
			return m_ItemContainer.PaddingBottom;
		}
		set
		{
			if (m_ItemContainer.PaddingBottom != value)
			{
				m_ItemContainer.PaddingBottom = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets/Sets the padding in pixels. This represents the spacing between the left edge of the bar and the left position of the first item.")]
	[DefaultValue(1)]
	public int PaddingLeft
	{
		get
		{
			return m_ItemContainer.PaddingLeft;
		}
		set
		{
			if (m_ItemContainer.PaddingLeft != value)
			{
				m_ItemContainer.PaddingLeft = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets/Sets the padding in pixels. This represents the spacing between the right edge of the bar and the right side of the last item.")]
	[DefaultValue(1)]
	public int PaddingRight
	{
		get
		{
			return m_ItemContainer.PaddingRight;
		}
		set
		{
			if (m_ItemContainer.PaddingRight != value)
			{
				m_ItemContainer.PaddingRight = value;
				if (((Component)this).DesignMode)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates Bar layout type.")]
	[DefaultValue(false)]
	public bool MenuBar
	{
		get
		{
			return m_MenuBar;
		}
		set
		{
			m_MenuBar = value;
			SetupAccessibility();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eBarType.Toolbar)]
	[Category("Appearance")]
	[Description("Indicates visual type of the bar. The type specified here is used to determine the appearance of the bar.")]
	public eBarType BarType
	{
		get
		{
			return m_BarType;
		}
		set
		{
			m_BarType = value;
			if (m_BarType == eBarType.StatusBar)
			{
				m_ItemContainer.ToolbarItemsAlign = eContainerVerticalAlignment.Middle;
			}
			else
			{
				m_ItemContainer.ToolbarItemsAlign = eContainerVerticalAlignment.Top;
			}
			if (BarType == eBarType.DockWindow)
			{
				ResetFont();
			}
			((Control)this).Invalidate();
		}
	}

	[Editor(typeof(ColorSchemeVSEditor), typeof(UITypeEditor))]
	[Browsable(true)]
	[DevCoBrowsable(true)]
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
			if (Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the background color of the caption (Title bar).")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color CaptionBackColor
	{
		get
		{
			return m_CaptionBackColor;
		}
		set
		{
			if (m_CaptionBackColor != value)
			{
				m_CaptionBackColor = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the caption (Title bar) text color.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color CaptionForeColor
	{
		get
		{
			return m_CaptionForeColor;
		}
		set
		{
			if (m_CaptionForeColor != value)
			{
				m_CaptionForeColor = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Indicates whether toolbars with appropriate style appear with rounded corners.")]
	[DefaultValue(true)]
	public bool RoundCorners
	{
		get
		{
			return m_RoundCorners;
		}
		set
		{
			m_RoundCorners = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the Bar.")]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_ItemContainer.Style;
		}
		set
		{
			m_ColorScheme.SwitchStyle(value);
			if (m_ItemContainer != null)
			{
				m_ItemContainer.Style = value;
				if ((m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle)) && LayoutType == eLayoutType.Toolbar && GrabHandleStyle != 0 && GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
				{
					m_GrabHandleStyle = eGrabHandleStyle.Office2003;
				}
				SetDockTabStyle(m_ItemContainer.EffectiveStyle);
			}
			if (AutoHide)
			{
				AutoHidePanel autoHidePanel = GetAutoHidePanel();
				if (autoHidePanel != null)
				{
					autoHidePanel.Style = value;
				}
			}
			else
			{
				((Control)this).Invalidate();
				RecalcLayout();
			}
		}
	}

	internal bool DockingInProgress => m_DockingInProgress;

	[Browsable(false)]
	public bool WrapItems => m_ItemContainer.WrapItems;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are docked.")]
	[DefaultValue(false)]
	public bool WrapItemsDock
	{
		get
		{
			return m_WrapItemsDock;
		}
		set
		{
			m_WrapItemsDock = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates whether the items will be wrapped into next line when Bar is full. Applies only to Bars that are floating.")]
	[DefaultValue(true)]
	public bool WrapItemsFloat
	{
		get
		{
			return m_WrapItemsFloat;
		}
		set
		{
			m_WrapItemsFloat = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the grab handle style of the docked Bars.")]
	[DefaultValue(eGrabHandleStyle.None)]
	public eGrabHandleStyle GrabHandleStyle
	{
		get
		{
			return m_GrabHandleStyle;
		}
		set
		{
			if (m_GrabHandleStyle != value)
			{
				m_GrabHandleStyle = value;
				SetupAccessibility();
				if (Visible && !m_LayoutSuspended)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public Rectangle GrabHandleRect => m_GrabHandleRect;

	private bool CanHideResolved
	{
		get
		{
			DockContainerItem selectedDockContainerItem = SelectedDockContainerItem;
			if (selectedDockContainerItem == null || selectedDockContainerItem.CanClose == eDockContainerClose.Inherit)
			{
				return CanHide;
			}
			return selectedDockContainerItem.CanClose == eDockContainerClose.Yes;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Determines whether bar can be hidden.")]
	[DefaultValue(false)]
	public bool CanHide
	{
		get
		{
			return m_CanHide;
		}
		set
		{
			if (m_CanHide == value)
			{
				return;
			}
			m_CanHide = value;
			if (m_BarState == eBarState.Floating || (m_BarState == eBarState.Docked && (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)))
			{
				((Control)this).Invalidate(m_GrabHandleRect);
			}
			UpdateDockTabSettings();
			if (m_TabDockItems != null && BarFunctions.IsHandleValid((Control)(object)this))
			{
				m_TabDockItems.RecalcSize();
				if (((Component)this).DesignMode)
				{
					((Control)m_TabDockItems).Invalidate();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates border style when Bar is docked.")]
	[DefaultValue(eBorderType.None)]
	public eBorderType DockedBorderStyle
	{
		get
		{
			return m_DockedBorder;
		}
		set
		{
			if (m_DockedBorder != value)
			{
				m_DockedBorder = value;
				if (m_BarState == eBarState.Docked && Visible)
				{
					RecalcLayout();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Indicates whether floating bar is hidden when application loses focus.")]
	public bool HideFloatingInactive
	{
		get
		{
			return m_HideFloatingInactive;
		}
		set
		{
			m_HideFloatingInactive = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Docking")]
	[Description("Indicates whether tab navigation buttons are shown for tabbed dockable bars.")]
	public bool TabNavigation
	{
		get
		{
			return m_TabNavigation;
		}
		set
		{
			if (m_TabNavigation != value)
			{
				m_TabNavigation = value;
				UpdateDockTabSettings();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the border line color when docked border is a single line.")]
	public Color SingleLineColor
	{
		get
		{
			return m_SingleLineColor;
		}
		set
		{
			if (m_SingleLineColor != value)
			{
				m_SingleLineColor = value;
				if (m_BarState == eBarState.Docked && Visible && m_DockedBorder == eBorderType.SingleLine)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	public bool Visible
	{
		get
		{
			if (m_BarState == eBarState.Floating && m_Float != null)
			{
				return ((Control)m_Float).Visible;
			}
			return ((Control)this).Visible;
		}
		set
		{
			m_IsVisible = value;
			if (((Control)this).Visible == value)
			{
				if (!value)
				{
					((Control)this).Visible = false;
				}
			}
			else if (value)
			{
				ShowBar();
			}
			else
			{
				HideBar();
			}
		}
	}

	[Browsable(false)]
	public int VisibleItemCount => m_ItemContainer.VisibleSubItems;

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Determines whether end-user can drag and drop items on the Bar.")]
	[DefaultValue(true)]
	public bool AcceptDropItems
	{
		get
		{
			return m_AcceptDropItems;
		}
		set
		{
			m_AcceptDropItems = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether items on the Bar can be customized.")]
	[DefaultValue(true)]
	public bool CanCustomize
	{
		get
		{
			if (m_ItemContainer != null)
			{
				return m_ItemContainer.CanCustomize;
			}
			return false;
		}
		set
		{
			if (!((Component)this).DesignMode && m_ItemContainer != null)
			{
				m_ItemContainer.CanCustomize = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CustomBar
	{
		get
		{
			return m_CustomBar;
		}
		set
		{
			m_CustomBar = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Design")]
	[Description("Bar name that can be used to identify Bar from code.")]
	public string Name
	{
		get
		{
			return ((Control)this).Name;
		}
		set
		{
			((Control)this).Name = value;
			if (((Component)(object)this).Site != null)
			{
				((Component)(object)this).Site.Name = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Specifies the Image size that will be used by items on this bar.")]
	[DefaultValue(eBarImageSize.Default)]
	public eBarImageSize ImageSize
	{
		get
		{
			return m_ImageSize;
		}
		set
		{
			if (m_ImageSize != value)
			{
				m_ImageSize = value;
				m_ItemContainer.RefreshImageSize();
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates Bar layout type.")]
	[DefaultValue(eLayoutType.Toolbar)]
	public eLayoutType LayoutType
	{
		get
		{
			if (m_ItemContainer == null)
			{
				return eLayoutType.Toolbar;
			}
			return m_ItemContainer.LayoutType;
		}
		set
		{
			if (m_ItemContainer.LayoutType != value)
			{
				m_ItemContainer.LayoutType = value;
				if (value == eLayoutType.DockContainer)
				{
					ResetFont();
				}
				SetupAccessibility();
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates that buttons will be automatically resized to match the size of the largest button on the bar.")]
	[DefaultValue(false)]
	public bool EqualButtonSize
	{
		get
		{
			return m_ItemContainer.EqualButtonSize;
		}
		set
		{
			if (m_ItemContainer.EqualButtonSize != value)
			{
				m_ItemContainer.EqualButtonSize = value;
				RecalcLayout();
			}
		}
	}

	internal int CornerSize
	{
		get
		{
			return m_CornerSize;
		}
		set
		{
			m_CornerSize = value;
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

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether mouse over fade effect for buttons is enabled.")]
	public bool FadeEffect
	{
		get
		{
			return m_FadeEffect;
		}
		set
		{
			m_FadeEffect = value;
		}
	}

	internal bool IsFadeEnabled
	{
		get
		{
			if (((Component)this).DesignMode || !BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle) || (m_FadeEffect && NativeFunctions.IsTerminalSession()) || IsThemed || TextDrawing.UseTextRenderer)
			{
				return false;
			}
			return m_FadeEffect;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies background color of the Bar.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override Color BackColor
	{
		get
		{
			if (m_ItemContainer != null)
			{
				return m_ItemContainer.BackColor;
			}
			return ((Control)this).BackColor;
		}
		set
		{
			if (m_ItemContainer != null)
			{
				m_ItemContainer.BackColor = value;
			}
			else
			{
				((Control)this).BackColor = value;
			}
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	[Description("Indicates the Customize popup menu.")]
	[DefaultValue(null)]
	public PopupItem CustomizeMenu
	{
		get
		{
			return m_CustomizeMenu;
		}
		set
		{
			m_CustomizeMenu = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Description("Indicates the auto-hide side of the parent form where bar is positioned.")]
	public eDockSide AutoHideSide
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected I4, but got Unknown
			eDockSide result = eDockSide.None;
			if (m_AutoHideState)
			{
				DockStyle dockSide = m_LastDockSiteInfo.DockSide;
				result = (dockSide - 1) switch
				{
					2 => eDockSide.Left, 
					3 => eDockSide.Right, 
					0 => eDockSide.Top, 
					_ => eDockSide.Bottom, 
				};
			}
			return result;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Auto-Hide")]
	[Description("")]
	public bool AutoHideTabTextAlwaysVisible
	{
		get
		{
			return m_AutoHideTextAlwaysVisible;
		}
		set
		{
			m_AutoHideTextAlwaysVisible = value;
			if (AutoHide && GetAutoHidePanel() != null)
			{
				((Control)GetAutoHidePanel()).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Category("Auto-Hide")]
	[Description("Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AutoHide
	{
		get
		{
			return m_AutoHideState;
		}
		set
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			if (!((Component)this).DesignMode)
			{
				if (m_LayoutSuspended)
				{
					m_AutoHideStateDelayed = value;
				}
				else if (m_CanAutoHide && LayoutType == eLayoutType.DockContainer && !(((Control)this).Parent != null && (int)((Control)this).Parent.Dock == 5 && value) && m_AutoHideState != value)
				{
					m_AutoHideState = value;
					AutoHideStateChanged();
					InvokeAutoHideChanged();
				}
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Auto-Hide")]
	[Description("Gets or sets the visibility of the bar when bar is in auto-hide state.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AutoHideVisible
	{
		get
		{
			return Visible;
		}
		set
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (AutoHide && Visible != value)
			{
				AutoHidePanel autoHidePanel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
				if (value && autoHidePanel != null && m_AutoHideState)
				{
					autoHidePanel.ShowBar(this);
				}
				else if (!value && autoHidePanel != null && m_AutoHideState)
				{
					autoHidePanel.HideBar(this);
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Auto-Hide")]
	[Description("Indicates whether Bar can be auto hidden.")]
	public bool CanAutoHide
	{
		get
		{
			return m_CanAutoHide;
		}
		set
		{
			m_CanAutoHide = value;
			if (!m_CanAutoHide)
			{
				m_SystemButtons.AutoHideButtonRect = Rectangle.Empty;
				m_SystemButtons.MouseOverAutoHide = false;
				m_SystemButtons.MouseDownAutoHide = false;
			}
			if (((Control)this).IsHandleCreated)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Component GlobalParentComponent
	{
		get
		{
			return m_GlobalParentComponent;
		}
		set
		{
			m_GlobalParentComponent = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(100)]
	[Category("Auto-Hide")]
	[Description("Specifies how long it takes to play the auto-hide animation, in milliseconds. Maximum value is 2000, 0 disables animation.")]
	public virtual int AutoHideAnimationTime
	{
		get
		{
			return m_AutoHideAnimationTime;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value > 2000)
			{
				value = 2000;
			}
			m_AutoHideAnimationTime = value;
		}
	}

	private bool ShowCustomizeMenuButton
	{
		get
		{
			if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted) && m_CustomizeMenu != null)
			{
				return true;
			}
			if (m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				if (m_ItemContainer.HaveCustomizeItem)
				{
					return true;
				}
				return m_CustomizeMenu != null;
			}
			return false;
		}
	}

	private bool ToolTipVisible => m_ToolTipWnd != null;

	[DefaultValue(25)]
	[Category("Appearance")]
	[Description("Indicates height of the docked bar tab strip which displays docked tabs.")]
	public int DockTabStripHeight
	{
		get
		{
			return _DockTabStripHeight;
		}
		set
		{
			if (value != _DockTabStripHeight)
			{
				int dockTabStripHeight = _DockTabStripHeight;
				_DockTabStripHeight = value;
				OnDockTabStripHeightChanged(dockTabStripHeight, value);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public TabStrip DockTabControl => m_TabDockItems;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockContainerItem SelectedDockContainerItem
	{
		get
		{
			if (m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				return null;
			}
			int selectedDockTab = SelectedDockTab;
			if (selectedDockTab >= 0)
			{
				return Items[selectedDockTab] as DockContainerItem;
			}
			if (VisibleItemCount > 0)
			{
				foreach (BaseItem item in Items)
				{
					if (item.Visible && item.Displayed)
					{
						return item as DockContainerItem;
					}
				}
			}
			return null;
		}
		set
		{
			if (m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				throw new InvalidOperationException("Bar type is not dockable window. LayoutType must be set to eLayoutType.DockContainer");
			}
			if (!Items.Contains(value))
			{
				throw new InvalidOperationException("Bar.Items collection does not contain the item");
			}
			if (Items.Contains(value))
			{
				SelectedDockTab = Items.IndexOf(value);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Gets or sets the tab (DockContainerItem) index for Bars with LayoutType set to eLayoutType.DockContainer.")]
	[DefaultValue(-1)]
	public int SelectedDockTab
	{
		get
		{
			if (m_ItemContainer.LayoutType != eLayoutType.DockContainer || m_TabDockItems == null)
			{
				return -1;
			}
			if (m_TabDockItems.SelectedTab != null)
			{
				return m_ItemContainer.SubItems.IndexOf(m_TabDockItems.SelectedTab.AttachedItem);
			}
			return -1;
		}
		set
		{
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			if (m_TabDockItems == null || (m_TabDockItems != null && AutoHide))
			{
				if (value < Items.Count)
				{
					for (int i = 0; i < Items.Count; i++)
					{
						if (i == value && Items[i].Visible)
						{
							Items[i].Displayed = true;
						}
						else
						{
							Items[i].Displayed = false;
						}
					}
					if (m_TabDockItems == null)
					{
						if (Items[value].Displayed)
						{
							InvokeDockTabChange(null, Items[value]);
						}
						RefreshAutoHidePanel();
					}
				}
				if (m_TabDockItems == null)
				{
					return;
				}
			}
			if (m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				throw new InvalidOperationException("SelectedDockTab property can be set only for LayoutType=eLayoutType.DockContainer.");
			}
			if (value < 0)
			{
				value = 0;
			}
			if (value >= m_ItemContainer.SubItems.Count)
			{
				throw new InvalidOperationException("Invalid tab index.");
			}
			BaseItem baseItem = m_ItemContainer.SubItems[value];
			foreach (TabItem tab in m_TabDockItems.Tabs)
			{
				if (tab.AttachedItem == baseItem)
				{
					if (m_TabDockItems.SelectedTab == tab && AutoHide)
					{
						InvokeBeforeDockTabDisplayed(baseItem);
					}
					m_TabDockItems.SelectedTab = tab;
					if (AutoHide)
					{
						AutoHidePanel autoHidePanel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
						autoHidePanel.SelectedDockContainerItem = baseItem as DockContainerItem;
					}
					break;
				}
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Cursor Cursor
	{
		get
		{
			return ((Control)this).Cursor;
		}
		set
		{
			((Control)this).Cursor = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates Bar background image.")]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).RightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RightToLeft = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override ContextMenuStrip ContextMenuStrip
	{
		get
		{
			return ((Control)this).ContextMenuStrip;
		}
		set
		{
			((Control)this).ContextMenuStrip = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).ImeMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).ImeMode = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public int TabIndex
	{
		get
		{
			return ((Control)this).TabIndex;
		}
		set
		{
			((Control)this).TabIndex = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool CausesValidation
	{
		get
		{
			return ((Control)this).CausesValidation;
		}
		set
		{
			((Control)this).CausesValidation = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override AnchorStyles Anchor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Anchor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Anchor = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (DockSide != eDockSide.Document || (int)value == 0 || !(((Control)this).Parent is DockSite))
			{
				((Control)this).Dock = value;
			}
		}
	}

	[Browsable(false)]
	public Point Location
	{
		get
		{
			return ((Control)this).Location;
		}
		set
		{
			if (m_BarState == eBarState.Floating)
			{
				if (m_Float != null)
				{
					((Form)m_Float).Location = value;
				}
			}
			else
			{
				((Control)this).Location = value;
			}
		}
	}

	[Browsable(false)]
	public Size Size
	{
		get
		{
			return ((Control)this).Size;
		}
		set
		{
			((Control)this).Size = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool AllowDrop
	{
		get
		{
			return ((Control)this).AllowDrop;
		}
		set
		{
			((Control)this).AllowDrop = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether caption button drop-down menu is automatically created")]
	public virtual bool AutoCreateCaptionMenu
	{
		get
		{
			return m_AutoCreateCaptionMenu;
		}
		set
		{
			m_AutoCreateCaptionMenu = value;
			if (Visible)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether caption is automatically set to the active dock container item text.")]
	public virtual bool AutoSyncBarCaption
	{
		get
		{
			return m_AutoSyncBarCaption;
		}
		set
		{
			m_AutoSyncBarCaption = value;
			SyncBarCaption();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Specifies whether Bar can be undocked.")]
	[DefaultValue(true)]
	public bool CanUndock
	{
		get
		{
			return m_CanUndock;
		}
		set
		{
			if (m_CanUndock != value)
			{
				m_CanUndock = value;
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Specifes whether end-user can tear-off (deattach) the tabs on dockable window.")]
	[DefaultValue(true)]
	public bool CanTearOffTabs
	{
		get
		{
			return m_CanTearOffTabs;
		}
		set
		{
			if (m_CanTearOffTabs != value)
			{
				m_CanTearOffTabs = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Specifes whether end-user can reorder the tabs on dockable window.")]
	[DefaultValue(true)]
	public bool CanReorderTabs
	{
		get
		{
			return m_CanReorderTabs;
		}
		set
		{
			if (m_CanReorderTabs != value)
			{
				m_CanReorderTabs = value;
				if (m_TabDockItems != null)
				{
					m_TabDockItems.CanReorderTabs = value;
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Indicates whether bar or DockContainerItem that is torn-off this bar can be docked as tab to another bar.")]
	public bool CanDockTab
	{
		get
		{
			return m_CanDockTab;
		}
		set
		{
			m_CanDockTab = value;
		}
	}

	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Indicates whether dock bar can be moved by dragging its caption using the mouse.")]
	public bool CanMove
	{
		get
		{
			return _CanMove;
		}
		set
		{
			_CanMove = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Determines whether bar can be docked to the top edge of container.")]
	public bool CanDockTop
	{
		get
		{
			return m_CanDockTop;
		}
		set
		{
			if (m_CanDockTop != value)
			{
				m_CanDockTop = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Determines whether bar can be docked to the bottom edge of container.")]
	public bool CanDockBottom
	{
		get
		{
			return m_CanDockBottom;
		}
		set
		{
			if (m_CanDockBottom != value)
			{
				m_CanDockBottom = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Determines whether bar can be docked to the left edge of container.")]
	public bool CanDockLeft
	{
		get
		{
			return m_CanDockLeft;
		}
		set
		{
			if (m_CanDockLeft != value)
			{
				m_CanDockLeft = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Docking")]
	[Description("Determines whether bar can be docked to the right edge of container.")]
	public bool CanDockRight
	{
		get
		{
			return m_CanDockRight;
		}
		set
		{
			if (m_CanDockRight != value)
			{
				m_CanDockRight = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Docking")]
	[Description("Specifies whether Bar can be docked as document.")]
	public bool CanDockDocument
	{
		get
		{
			return m_CanDockDocument;
		}
		set
		{
			m_CanDockDocument = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Specifies whether Bar will stretch to always fill the space in dock site.")]
	public bool Stretch
	{
		get
		{
			return m_DockStretch;
		}
		set
		{
			if (m_DockStretch != value)
			{
				m_DockStretch = value;
				RecalcLayout();
			}
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

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates the distance from the far left/top side of the docking site..")]
	[DefaultValue(0)]
	public int DockOffset
	{
		get
		{
			return m_DockOffset;
		}
		set
		{
			m_DockOffset = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates the docking line.")]
	[DefaultValue(0)]
	public int DockLine
	{
		get
		{
			return m_DockLine;
		}
		set
		{
			m_DockLine = value;
			if (m_BarState == eBarState.Docked && ((Control)this).Parent is DockSite && !m_LayoutSuspended)
			{
				((DockSite)(object)((Control)this).Parent).AdjustBarPosition((Control)(object)this);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Gets or sets the dock tab alignment.")]
	[DefaultValue(eTabStripAlignment.Bottom)]
	public eTabStripAlignment DockTabAlignment
	{
		get
		{
			return m_DockTabAlignment;
		}
		set
		{
			m_DockTabAlignment = value;
			if (m_TabDockItems != null)
			{
				m_TabDockItems.TabAlignment = m_DockTabAlignment;
				RecalcLayout();
				ResizeDockTab();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Docking")]
	[Description("Indicates whether selected dock tab is closed when Bar caption close button is pressed. Default value is false which indicates that whole bar will be hidden when bars close button is pressed.")]
	public bool CloseSingleTab
	{
		get
		{
			return m_CloseSingleTab;
		}
		set
		{
			m_CloseSingleTab = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether close button is displayed on each dock tab that allows closing of the tab.")]
	[Category("Docking")]
	public bool DockTabCloseButtonVisible
	{
		get
		{
			return m_DockTabCloseButtonVisible;
		}
		set
		{
			if (m_DockTabCloseButtonVisible != value)
			{
				m_DockTabCloseButtonVisible = value;
				if (m_TabDockItems != null)
				{
					m_TabDockItems.CloseButtonOnTabsVisible = m_DockTabCloseButtonVisible;
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates whether tab that shows all dock containers on the bar is visible all the time.")]
	[DefaultValue(false)]
	public virtual bool AlwaysDisplayDockTab
	{
		get
		{
			return m_AlwaysDisplayDockTab;
		}
		set
		{
			if (m_AlwaysDisplayDockTab != value)
			{
				m_AlwaysDisplayDockTab = value;
				RefreshDockTab(bFireEvents: false);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates whether bar is locked to prevent docking below it. Applies to undockable bars only.")]
	[DefaultValue(false)]
	public bool LockDockPosition
	{
		get
		{
			return m_LockDockPosition;
		}
		set
		{
			if (m_LockDockPosition != value)
			{
				m_LockDockPosition = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eOrientation.Horizontal)]
	[DevCoBrowsable(false)]
	public eOrientation DockOrientation
	{
		get
		{
			return m_ItemContainer.Orientation;
		}
		set
		{
			if (m_ItemContainer.Orientation != value)
			{
				m_ItemContainer.Orientation = value;
			}
			if (((Component)this).DesignMode && !(((Control)this).Parent is DockSite))
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	public bool Docked => m_BarState == eBarState.Docked;

	[Browsable(false)]
	public Control DockedSite => ((Control)this).Parent;

	internal eDockSide LastDockSide
	{
		get
		{
			return _LastDockSidePrivate;
		}
		set
		{
			_LastDockSidePrivate = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[Category("Docking")]
	[Description("Indicates the dock side for the Bar.")]
	public eDockSide DockSide
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Invalid comparison between Unknown and I4
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Invalid comparison between Unknown and I4
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Invalid comparison between Unknown and I4
			if (m_BarState != eBarState.Docked)
			{
				return eDockSide.None;
			}
			if (((Control)this).Parent == null)
			{
				return eDockSide.Top;
			}
			if ((int)((Control)this).Parent.Dock == 3)
			{
				return eDockSide.Left;
			}
			if ((int)((Control)this).Parent.Dock == 4)
			{
				return eDockSide.Right;
			}
			if ((int)((Control)this).Parent.Dock == 1)
			{
				return eDockSide.Top;
			}
			if ((int)((Control)this).Parent.Dock == 2)
			{
				return eDockSide.Bottom;
			}
			if ((int)((Control)this).Parent.Dock == 5)
			{
				return eDockSide.Document;
			}
			return eDockSide.None;
		}
		set
		{
			if (Owner == null || m_LayoutSuspended)
			{
				m_DockSideDelayed = (int)value;
				return;
			}
			if (AutoHide)
			{
				ChangeAutoHidePanel(value);
				return;
			}
			DockSiteInfo dockSiteInfo = new DockSiteInfo();
			if (!(m_Owner is IOwnerBarSupport ownerBarSupport))
			{
				m_DockSideDelayed = (int)value;
				return;
			}
			dockSiteInfo.InsertPosition = -10;
			dockSiteInfo.DockLine = m_DockLine;
			dockSiteInfo.DockOffset = m_DockOffset;
			if (!m_BarDefinitionLoading)
			{
				SplitDockHeight = 0;
				SplitDockWidth = 0;
			}
			switch (value)
			{
			case eDockSide.Left:
			{
				DockSite dockSite4 = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite4 = ownerBarSupport.ToolbarLeftDockSite;
					if (dockSite4 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarLeftDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite4 = ownerBarSupport.LeftDockSite;
					if (dockSite4 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.LeftDockSite dock-site is not set.");
					}
				}
				dockSiteInfo.objDockSite = dockSite4;
				break;
			}
			case eDockSide.Right:
			{
				DockSite dockSite3 = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite3 = ownerBarSupport.ToolbarRightDockSite;
					if (dockSite3 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarRightDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite3 = ownerBarSupport.RightDockSite;
					if (dockSite3 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.RightDockSite dock-site is not set.");
					}
				}
				dockSiteInfo.objDockSite = dockSite3;
				break;
			}
			case eDockSide.Top:
			{
				DockSite dockSite2 = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite2 = ownerBarSupport.ToolbarTopDockSite;
					if (dockSite2 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarTopDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite2 = ownerBarSupport.TopDockSite;
					if (dockSite2 == null)
					{
						throw new InvalidOperationException("DotNetBarManager.TopDockSite dock-site is not set.");
					}
				}
				dockSiteInfo.objDockSite = dockSite2;
				break;
			}
			case eDockSide.Bottom:
			{
				DockSite dockSite = null;
				if (LayoutType == eLayoutType.Toolbar)
				{
					dockSite = ownerBarSupport.ToolbarBottomDockSite;
					if (dockSite == null)
					{
						throw new InvalidOperationException("DotNetBarManager.ToolbarBottomDockSite dock-site is not set.");
					}
				}
				else
				{
					dockSite = ownerBarSupport.BottomDockSite;
					if (dockSite == null)
					{
						throw new InvalidOperationException("DotNetBarManager.BottomDockSite dock-site is not set.");
					}
				}
				if (dockSite == null)
				{
					throw new InvalidOperationException("Bottom dock-site is not set.");
				}
				dockSiteInfo.objDockSite = dockSite;
				break;
			}
			case eDockSide.Document:
				dockSiteInfo.objDockSite = ownerBarSupport.FillDockSite;
				break;
			default:
				dockSiteInfo.objDockSite = null;
				break;
			}
			DockingHandler(dockSiteInfo, m_FloatingRect.Location);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Description("Indicates the inital floating location.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point InitalFloatLocation
	{
		get
		{
			return m_FloatingRect.Location;
		}
		set
		{
			m_FloatingRect.Location = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether Tooltips are shown on Bar and it's sub-items.")]
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

	bool IOwner.DesignMode => ((Component)this).DesignMode;

	[Browsable(true)]
	[DevCoBrowsable(false)]
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
			if (m_AlwaysDisplayKeyAccelerators != value)
			{
				m_AlwaysDisplayKeyAccelerators = value;
				((Control)this).Refresh();
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

	[Browsable(true)]
	[DevCoBrowsable(false)]
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

	bool IOwner.ShowShortcutKeysInToolTips
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	BaseItem IOwner.DragItem => null;

	bool IOwner.DragInProgress => false;

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
	public bool UseHook
	{
		get
		{
			return _UseHook;
		}
		set
		{
			if (_UseHook != value)
			{
				_UseHook = value;
			}
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

	bool IOwnerMenuSupport.ShowPopupShadow => true;

	eMenuDropShadow IOwnerMenuSupport.MenuDropShadow
	{
		get
		{
			return eMenuDropShadow.SystemDefault;
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

	internal bool IgnoreSysKeyUp
	{
		get
		{
			return m_IgnoreSysKeyUp;
		}
		set
		{
			m_IgnoreSysKeyUp = value;
		}
	}

	internal bool EatSysKeyUp
	{
		get
		{
			return m_EatSysKeyUp;
		}
		set
		{
			m_EatSysKeyUp = value;
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

	private bool IsHandlingShortcuts
	{
		get
		{
			Form val = ((Control)this).FindForm();
			Form activeForm = Form.ActiveForm;
			if (val == null)
			{
				return false;
			}
			if (val.IsMdiChild && val.MdiParent != null)
			{
				if (val.MdiParent.ActiveMdiChild == val && (val == activeForm || val.MdiParent == activeForm))
				{
					return true;
				}
				return false;
			}
			return val == Form.ActiveForm;
		}
	}

	bool IDockInfo.CanDockTop => CanDockTop;

	bool IDockInfo.CanDockBottom => CanDockBottom;

	bool IDockInfo.CanDockLeft => CanDockLeft;

	bool IDockInfo.CanDockRight => CanDockRight;

	bool IDockInfo.CanDockDocument => CanDockDocument;

	bool IDockInfo.Stretch
	{
		get
		{
			return Stretch;
		}
		set
		{
			Stretch = value;
		}
	}

	int IDockInfo.DockOffset
	{
		get
		{
			return DockOffset;
		}
		set
		{
			DockOffset = value;
		}
	}

	int IDockInfo.DockLine
	{
		get
		{
			return DockLine;
		}
		set
		{
			DockLine = value;
		}
	}

	eOrientation IDockInfo.DockOrientation
	{
		get
		{
			return DockOrientation;
		}
		set
		{
			DockOrientation = value;
		}
	}

	bool IDockInfo.Docked => Docked;

	Control IDockInfo.DockedSite => DockedSite;

	eDockSide IDockInfo.DockSide
	{
		get
		{
			return DockSide;
		}
		set
		{
			DockSide = value;
		}
	}

	bool IDockInfo.LockDockPosition
	{
		get
		{
			return LockDockPosition;
		}
		set
		{
			LockDockPosition = value;
		}
	}

	[Description("Occurs after bar state has changed, like selected dock tab has changed, bar has closed, bar has been docked or undocked etc.")]
	public event BarStateChangedEventHandler BarStateChanged;

	[Description("Occurs when Item is clicked.")]
	[Category("Item")]
	public event EventHandler ItemClick;

	[Description("Occurs after Bar is docked.")]
	public event EventHandler BarDock;

	[Description("Occurs after Bar is undocked.")]
	public event EventHandler BarUndock;

	[Description("Occurs after Bar definition is loaded.")]
	public event EventHandler DefinitionLoaded;

	[Description("Occurs when current Dock tab has changed.")]
	public event DotNetBarManager.DockTabChangeEventHandler DockTabChange;

	[Description("Occurs when bar visibility has changed as a result of user action.")]
	public event EventHandler UserVisibleChanged;

	[Description("Occurs when bar auto hide state has changed.")]
	public event EventHandler AutoHideChanged;

	[Description("Occurs when Bar is about to be closed as a result of user clicking the Close button on the bar.")]
	public event DotNetBarManager.BarClosingEventHandler Closing;

	[Description("Occurs when Bar in auto-hide state is about to be displayed.")]
	public event DotNetBarManager.AutoHideDisplayEventHandler AutoHideDisplay;

	[Description("Occurs when popup item is closing.")]
	[Category("Item")]
	public event EventHandler PopupClose;

	[Description("Occurs when popup of type container is loading.")]
	[Category("Item")]
	public event EventHandler PopupContainerLoad;

	[Description("Occurs when popup of type container is unloading.")]
	[Category("Item")]
	public event EventHandler PopupContainerUnload;

	[Description("Occurs when popup item is about to open.")]
	[Category("Item")]
	public event DotNetBarManager.PopupOpenEventHandler PopupOpen;

	[Description("Occurs just before popup window is shown.")]
	[Category("Item")]
	public event EventHandler PopupShowing;

	[Description("Occurs before dock tab is displayed.")]
	public event EventHandler BeforeDockTabDisplayed;

	[Description("Occurs when caption button is clicked on bars with grab handle style task pane.")]
	public event EventHandler CaptionButtonClick;

	[Description("Occurs on dockable bars when end-user attempts to close the individual DockContainerItem objects using system buttons on dock tab.")]
	public event DockTabClosingEventHandler DockTabClosing;

	[Description("Occurs on dockable bars after DockContainerItem is closed by end-user. This action cannot be cancelled.")]
	public event DockTabClosingEventHandler DockTabClosed;

	public event SerializeItemEventHandler SerializeItem;

	public event SerializeItemEventHandler DeserializeItem;

	public event EventHandler TabStripStyleChanged;

	public event RenderBarEventHandler PreRender;

	public event RenderBarEventHandler PostRender;

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

	public event EventHandler StartDragDrop;

	public event EventHandler ItemLayoutUpdated;

	public event CancelEventHandler BeforeAutoHideDisplayed;

	public event CancelEventHandler BeforeAutoHideHidden;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler FocusItemChange;

	protected virtual void OnBarStateChanged(BarStateChangedEventArgs e)
	{
		this.BarStateChanged?.Invoke(this, e);
		if (Owner is DotNetBarManager dotNetBarManager)
		{
			dotNetBarManager.InvokeOnBarStateChanged(e);
		}
	}

	public void InvokeStartDragDrop(object item)
	{
		this.StartDragDrop(item, EventArgs.Empty);
	}

	public Bar()
	{
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		m_ColorScheme = new ColorScheme();
		m_ParentItem = null;
		m_OldContainer = null;
		m_ClientRect = Rectangle.Empty;
		m_SideBarRect = Rectangle.Empty;
		m_SideBarImage = default(SideBarImage);
		m_ItemContainer = new GenericItemContainer();
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.WrapItems = true;
		m_ItemContainer.Stretch = false;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.SystemContainer = true;
		m_InitialContainerWidth = 164;
		m_BarState = eBarState.Docked;
		m_DockOffset = 0;
		m_DockLine = 0;
		m_GrabHandleStyle = eGrabHandleStyle.None;
		m_GrabHandleRect = Rectangle.Empty;
		m_Owner = null;
		m_MouseDownPt = Point.Empty;
		m_MouseDownSize = new Size(0, 0);
		m_MoveWindow = false;
		m_FloatingRect = Rectangle.Empty;
		m_SizeWindow = 0;
		m_LastDragRect = Rectangle.Empty;
		m_WrapItemsDock = false;
		m_WrapItemsFloat = true;
		m_DockStretch = false;
		m_MenuBar = false;
		((Control)this).Text = "";
		m_Float = null;
		m_DockingInProgress = false;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		TabStop = false;
		((Control)this).Font = SystemFonts.MenuFont;
		m_CanDockLeft = true;
		m_CanDockRight = true;
		m_CanDockTop = true;
		m_CanDockBottom = true;
		m_CanUndock = true;
		SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(PreferenceChanged);
		((Control)this).IsAccessible = true;
		m_SystemButtons.MouseDownAutoHideChanged += SysButtonHideTooltip;
		m_SystemButtons.MouseDownCloseChanged += SysButtonHideTooltip;
		m_SystemButtons.MouseDownCustomizeChanged += SysButtonHideTooltip;
		m_SystemButtons.MouseOverAutoHideChanged += SysButtonMouseOverAutoHide;
		m_SystemButtons.MouseOverCustomizeChanged += SysButtonMouseOverCustomize;
		m_SystemButtons.MouseOverCloseChanged += SysButtonMouseOverClose;
	}

	private void PreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
	{
		if (!m_SystemPrefChanged && !m_CustomFont)
		{
			m_SystemPrefChanged = true;
			NativeFunctions.PostMessage(((Control)this).Handle, 1126, IntPtr.Zero, IntPtr.Zero);
		}
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new BarAccessibleObject(this);
	}

	private void SetupAccessibility()
	{
		if (((Control)this).Text != "")
		{
			((Control)this).AccessibleName = ((Control)this).Text;
		}
		else
		{
			((Control)this).AccessibleName = "DotNetBar Bar";
		}
		((Control)this).AccessibleDescription = ((Control)this).AccessibleName + " (" + Name + ")";
		if (MenuBar)
		{
			((Control)this).AccessibleRole = (AccessibleRole)2;
		}
		else if (GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
		{
			((Control)this).AccessibleRole = (AccessibleRole)23;
		}
		else
		{
			((Control)this).AccessibleRole = (AccessibleRole)22;
		}
	}

	public Bar(string BarCaption)
		: this()
	{
		((Control)this).Text = BarCaption;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		IsDisposing = true;
		if (m_ParentMsgHandlerRegistered)
		{
			DotNetBarManager.UnRegisterOwnerParentMsgHandler(this, null);
			m_ParentMsgHandlerRegistered = false;
		}
		if (m_Float != null && disposing)
		{
			try
			{
				if (((Control)m_Float).Controls.Contains((Control)(object)this))
				{
					((Control)m_Float).Controls.Remove((Control)(object)this);
				}
				m_Float.Close();
				((Component)(object)m_Float).Dispose();
				m_Float = null;
			}
			catch (Exception)
			{
			}
		}
		if (m_Owner is DotNetBarManager && ((DotNetBarManager)m_Owner).IgnoreLoadedControlDispose)
		{
			((Control)this).Controls.Clear();
		}
		if (((Control)this).Parent != null)
		{
			try
			{
				if (((Control)this).Parent.InvokeRequired)
				{
					((Control)this).Parent.Invoke((Delegate)(MethodInvoker)delegate
					{
						((Control)this).Parent.Controls.Remove((Control)(object)this);
					});
				}
				else
				{
					((Control)this).Parent.Controls.Remove((Control)(object)this);
				}
			}
			catch (Exception)
			{
			}
		}
		SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(PreferenceChanged);
		if (m_TabDockItems != null)
		{
			((Component)(object)m_TabDockItems).Dispose();
			m_TabDockItems = null;
		}
		if (m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		if (m_FilterInstalled)
		{
			MessageHandler.UnregisterMessageClient(this);
			m_FilterInstalled = false;
		}
		RestoreContainer();
		m_Owner = null;
		m_ParentItem = null;
		m_OldContainer = null;
		if (m_ItemContainer != null)
		{
			m_ItemContainer.Dispose();
		}
		m_ItemContainer = null;
		((Control)this).Dispose(disposing);
		IsDisposing = false;
	}

	internal void SetHasFocus(bool b)
	{
		m_HasFocus = b;
		if (DockSide == eDockSide.None && LayoutType == eLayoutType.DockContainer && Owner is DotNetBarManager)
		{
			DotNetBarManager dotNetBarManager = Owner as DotNetBarManager;
			if (b)
			{
				dotNetBarManager.InternalDockContainerActivated(SelectedDockContainerItem);
			}
			else
			{
				dotNetBarManager.InternalDockContainerDeactivated(SelectedDockContainerItem);
			}
		}
		((Control)this).Refresh();
	}

	protected override void OnLeave(EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnLeave(e);
		if (m_HasFocus)
		{
			m_HasFocus = false;
			if (m_GrabHandleStyle == eGrabHandleStyle.Caption && (m_BarState == eBarState.Docked || m_BarState == eBarState.AutoHide))
			{
				((Control)this).Invalidate();
			}
			if (m_BarState == eBarState.AutoHide)
			{
				GetAutoHidePanel(m_LastDockSiteInfo.DockSide).StartTimer();
			}
		}
	}

	protected override void OnEnter(EventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnEnter(e);
		if (m_HasFocus)
		{
			return;
		}
		m_HasFocus = true;
		if (m_GrabHandleStyle == eGrabHandleStyle.Caption && (m_BarState == eBarState.Docked || m_BarState == eBarState.AutoHide))
		{
			((Control)this).Invalidate();
		}
		if (m_BarState == eBarState.AutoHide)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (((Control)this).ClientRectangle.Contains(pt))
			{
				GetAutoHidePanel(m_LastDockSiteInfo.DockSide).StopTimer();
			}
		}
	}

	protected override bool IsInputKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 37 || (int)keyData == 39 || (int)keyData == 38 || (int)keyData == 40 || (int)keyData == 13 || (int)keyData == 13 || (int)keyData == 9 || (int)keyData == 27)
		{
			return true;
		}
		return ((Control)this).IsInputKey(keyData);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (m_ItemContainer != null)
		{
			m_ItemContainer.UpdateBindings();
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33 && (m_BarState == eBarState.Popup || (m_BarState == eBarState.Floating && m_ItemContainer.LayoutType == eLayoutType.TaskList)))
		{
			((Message)(ref m)).Result = new IntPtr(3);
			return;
		}
		if (((Message)(ref m)).Msg == 1125)
		{
			if (!m_DockingInProgress && m_BarState == eBarState.Floating)
			{
				NativeFunctions.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 19);
			}
		}
		else if (((Message)(ref m)).Msg == 1126)
		{
			if (Name == "StatusBar")
			{
				Name = Name;
			}
			if (LayoutType != eLayoutType.DockContainer)
			{
				((Control)this).Font = SystemFonts.MenuFont;
			}
			RecalcLayout();
			m_SystemPrefChanged = false;
		}
		else if (((Message)(ref m)).Msg == 1131)
		{
			if (m_DoDefaultActionItem != null)
			{
				m_DoDefaultActionItem.DoAccesibleDefaultAction();
				m_DoDefaultActionItem = null;
			}
		}
		else if (((Message)(ref m)).Msg == 7)
		{
			IntPtr wParam = ((Message)(ref m)).WParam;
			Control val = Control.FromChildHandle(wParam);
			if (val != null && !(val is Bar) && !(val is MenuPanel) && !(val is TextBoxX))
			{
				Form val2 = val.FindForm();
				if (val2 == ((Control)this).FindForm())
				{
					m_LastFocusWindow = ((Message)(ref m)).WParam;
				}
				else if (val2 != null)
				{
					m_LastFocusWindow = ((Message)(ref m)).WParam;
				}
			}
		}
		else if (((Message)(ref m)).Msg == 8)
		{
			bool flag = false;
			Control val3 = Control.FromChildHandle(((Message)(ref m)).WParam);
			if (val3 != null)
			{
				while (val3.Parent != null)
				{
					val3 = val3.Parent;
				}
				if (val3 != this && m_ItemContainer != null && !m_ItemContainer.IsAnyOnHandle(val3.Handle))
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && m_ItemContainer != null)
			{
				m_ItemContainer.ContainerLostFocus(appLostFocus: false);
			}
		}
		else if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
			RefreshThemeMargins();
			Themes.RefreshIsThemeActive();
		}
		((Control)this).WndProc(ref m);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		GetColorScheme().Refresh(null, bSystemColorEvent: true);
		((Control)this).Refresh();
	}

	public void ReleaseFocus()
	{
		if (!((Control)this).Focused || !(m_LastFocusWindow != IntPtr.Zero))
		{
			return;
		}
		Control val = Control.FromChildHandle(m_LastFocusWindow);
		if (val != null)
		{
			val.Select();
			if (!val.Focused)
			{
				NativeFunctions.SetFocus(m_LastFocusWindow);
			}
		}
		else
		{
			NativeFunctions.SetFocus(m_LastFocusWindow);
		}
		m_LastFocusWindow = IntPtr.Zero;
		m_ItemContainer.AutoExpand = false;
	}

	internal void SetSystemFocus()
	{
		if (!((Control)this).Focused)
		{
			m_ItemContainer.SetSystemFocus();
			((Control)this).Focus();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (m_ItemContainer != null && !((Control)this).IsDisposed)
		{
			if (((Control)this).BackColor == Color.Transparent)
			{
				((Control)this).OnPaintBackground(e);
			}
			Graphics graphics = e.Graphics;
			graphics.PageUnit = (GraphicsUnit)2;
			if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				PaintOffice(graphics);
			}
			else
			{
				PaintDotNet(e);
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (!m_AnimationInProgress && ((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			if (!(((Control)this).Parent is DockSite) && !(((Control)this).Parent is FloatingContainer) && m_BarState == eBarState.Docked && !m_LayoutSuspended)
			{
				RecalcLayout();
			}
			ResizeDockTab();
		}
	}

	private void PaintBackgroundImage(Graphics g)
	{
		if (((Control)this).BackgroundImage != null)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			BarFunctions.PaintBackgroundImage(g, clientRectangle, ((Control)this).BackgroundImage, m_BackgroundImagePosition, m_BackgroundImageAlpha);
		}
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
		if (m_DefaultRenderer == null && BarFunctions.IsOffice2007Style(Style))
		{
			m_DefaultRenderer = new Office2007Renderer();
		}
		return m_Renderer;
	}

	internal ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(m_Owner as IOwner, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.DesignerSelection = m_DesignerSelection;
		itemPaintArgs.Renderer = GetRenderer();
		if (m_BarState == eBarState.Popup && m_ParentItem != null && m_ParentItem.DesignMode)
		{
			ISite site = GetSite();
			if (site != null && site.DesignMode)
			{
				itemPaintArgs.DesignerSelection = true;
			}
		}
		return itemPaintArgs;
	}

	protected void PaintDotNet(PaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_113f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		//IL_117f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1186: Expected O, but got Unknown
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f89: Expected O, but got Unknown
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Expected O, but got Unknown
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0d: Expected O, but got Unknown
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e65: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Expected O, but got Unknown
		//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b1: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		RenderBarEventArgs renderBarEventArgs = null;
		if (m_AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		m_SystemButtons.CustomizeButtonRect = Rectangle.Empty;
		m_SystemButtons.CloseButtonRect = Rectangle.Empty;
		m_SystemButtons.AutoHideButtonRect = Rectangle.Empty;
		m_SystemButtons.CaptionButtonRect = Rectangle.Empty;
		ColorScheme colorScheme = GetColorScheme();
		ItemPaintArgs itemPaintArgs = GetItemPaintArgs(graphics);
		itemPaintArgs.ClipRectangle = e.ClipRectangle;
		Pen val = ((m_BarState != 0) ? new Pen(itemPaintArgs.Colors.BarFloatingBorder) : new Pen(itemPaintArgs.Colors.BarPopupBorder, 1f));
		if (DisplayShadow && !AlphaShadow && m_ParentItem != null && !BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
		{
			SetupRegion();
		}
		if (m_BarState == eBarState.Popup)
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).ClientRectangle);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && GetRenderer() != null)
				{
					ToolbarRendererEventArgs toolbarRendererEventArgs = new ToolbarRendererEventArgs(this, graphics, ((Control)this).DisplayRectangle);
					toolbarRendererEventArgs.ItemPaintArgs = itemPaintArgs;
					GetRenderer().DrawPopupToolbarBackground(toolbarRendererEventArgs);
				}
				else
				{
					SolidBrush val2 = new SolidBrush(itemPaintArgs.Colors.BarPopupBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					PaintBackgroundImage(itemPaintArgs.Graphics);
					PaintSideBar(graphics);
					Rectangle rectangle = ((Control)this).ClientRectangle;
					if (DisplayShadow && !AlphaShadow)
					{
						rectangle = new Rectangle(0, 0, ((Control)this).ClientSize.Width - 2, ((Control)this).ClientSize.Height - 2);
					}
					if (m_ParentItem != null && BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle))
					{
						DisplayHelp.DrawRoundedRectangle(graphics, val, rectangle, m_CornerSize);
					}
					else
					{
						NativeFunctions.DrawRectangle(graphics, val, rectangle);
					}
					if (DisplayShadow && !AlphaShadow)
					{
						val.Dispose();
						val = new Pen(SystemColors.ControlDark, 2f);
						Point[] array = new Point[3];
						array[0].X = 2;
						array[0].Y = ((Control)this).ClientSize.Height - 1;
						array[1].X = ((Control)this).ClientSize.Width - 1;
						array[1].Y = ((Control)this).ClientSize.Height - 1;
						array[2].X = ((Control)this).ClientSize.Width - 1;
						array[2].Y = 2;
						graphics.DrawLines(val, array);
					}
					if (m_ParentItem != null && !BarFunctions.IsOffice2007Style(m_ParentItem.EffectiveStyle) && m_ParentItem is ButtonItem && m_ParentItem.Displayed && m_ParentItemScreenPos.Y < Location.Y)
					{
						Point start = new Point(m_ParentItemScreenPos.X - Location.X + 1, 0);
						DisplayHelp.DrawLine(end: new Point(start.X + m_ParentItem.WidthInternal - 5, 0), g: graphics, start: start, color: itemPaintArgs.Colors.ItemExpandedBackground, penWidth: 1);
					}
				}
			}
		}
		else if (m_BarState == eBarState.Floating)
		{
			bool flag = true;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).ClientRectangle);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				if (BarFunctions.IsOffice2007Style(Style) && GetRenderer() != null)
				{
					ToolbarRendererEventArgs toolbarRendererEventArgs2 = new ToolbarRendererEventArgs(this, graphics, ((Control)this).DisplayRectangle);
					toolbarRendererEventArgs2.ItemPaintArgs = itemPaintArgs;
					GetRenderer().DrawToolbarBackground(toolbarRendererEventArgs2);
				}
				else
				{
					smoothingMode = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)0;
					if (MenuBar)
					{
						DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, itemPaintArgs.Colors.MenuBarBackground, itemPaintArgs.Colors.MenuBarBackground2, itemPaintArgs.Colors.MenuBarBackgroundGradientAngle);
					}
					else
					{
						DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, itemPaintArgs.Colors.BarBackground, itemPaintArgs.Colors.BarBackground2, itemPaintArgs.Colors.BarBackgroundGradientAngle);
					}
					PaintBackgroundImage(itemPaintArgs.Graphics);
					graphics.SmoothingMode = smoothingMode;
				}
			}
			Rectangle rectangle2 = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			ThemeWindow themeWindow = null;
			ThemeWindowParts part = ThemeWindowParts.SmallFrameLeft;
			ThemeWindowStates state = ThemeWindowStates.FrameActive;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Caption, Rectangle.Empty);
			if (DrawThemedCaption)
			{
				renderBarEventArgs.Bounds = new Rectangle(0, 0, ((Control)this).Width, m_ThemeWindowMargins.Top + 1);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					themeWindow = ThemeWindow;
					if (LayoutType == eLayoutType.DockContainer && !m_HasFocus)
					{
						state = ThemeWindowStates.FrameInactive;
					}
					themeWindow.DrawBackground(graphics, part, state, new Rectangle(0, 0, m_ThemeWindowMargins.Left, ((Control)this).Height));
					part = ThemeWindowParts.SmallFrameRight;
					themeWindow.DrawBackground(graphics, part, state, new Rectangle(((Control)this).Width - m_ThemeWindowMargins.Right, 0, m_ThemeWindowMargins.Left, ((Control)this).Height));
					part = ThemeWindowParts.SmallFrameBottom;
					themeWindow.DrawBackground(graphics, part, state, new Rectangle(0, ((Control)this).Height - m_ThemeWindowMargins.Bottom, ((Control)this).Width, m_ThemeWindowMargins.Bottom));
					if (LayoutType == eLayoutType.DockContainer && !m_HasFocus)
					{
						state = ThemeWindowStates.CaptionInactive;
					}
					part = ThemeWindowParts.SmallCaption;
				}
				rectangle2 = new Rectangle(0, 0, ((Control)this).Width, m_ThemeWindowMargins.Top + 1);
				themeWindow.DrawBackground(graphics, part, state, rectangle2);
				rectangle2.Offset(0, 1);
			}
			else if (m_GrabHandleStyle == eGrabHandleStyle.Caption && LayoutType == eLayoutType.DockContainer && Style == eDotNetBarStyle.Office2000)
			{
				Rectangle rectangle3 = (renderBarEventArgs.Bounds = new Rectangle(3, 3, ((Control)this).Width - 6, GetGrabHandleCaptionHeight()));
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					ControlPaint.DrawBorder3D(graphics, rectangle2, (Border3DStyle)5, (Border3DSide)2063);
					eDrawCaption eDrawCaption2 = eDrawCaption.DC_SMALLCAP | eDrawCaption.DC_TEXT | eDrawCaption.DC_GRADIENT;
					if (m_HasFocus)
					{
						eDrawCaption2 |= eDrawCaption.DC_ACTIVE;
					}
					IntPtr hdc = graphics.GetHdc();
					NativeFunctions.RECT lprc = new NativeFunctions.RECT(rectangle3.X, rectangle3.Y, rectangle3.Right, rectangle3.Bottom);
					try
					{
						NativeFunctions.DrawCaption(((Control)this).Handle, hdc, ref lprc, eDrawCaption2);
					}
					finally
					{
						graphics.ReleaseHdc(hdc);
					}
				}
				flag = false;
				rectangle2 = rectangle3;
			}
			else
			{
				Rectangle rectangle4 = (renderBarEventArgs.Bounds = new Rectangle(3, 3, ((Control)this).Width - 6, GetGrabHandleCaptionHeight()));
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					Pen val3 = new Pen(itemPaintArgs.Colors.BarFloatingBorder, 1f);
					NativeFunctions.DrawRectangle(graphics, val, rectangle2);
					rectangle2.Inflate(-1, -1);
					NativeFunctions.DrawRectangle(graphics, val, rectangle2);
					val3.Dispose();
					val3 = new Pen(itemPaintArgs.Colors.BarFloatingBorder, 1f);
					graphics.DrawLine(val3, 1, 2, 2, 2);
					graphics.DrawLine(val3, ((Control)this).Width - 3, 2, ((Control)this).Width - 2, 2);
					graphics.DrawLine(val3, 1, ((Control)this).Height - 3, 2, ((Control)this).Height - 3);
					graphics.DrawLine(val3, ((Control)this).Width - 3, ((Control)this).Height - 3, ((Control)this).Width - 2, ((Control)this).Height - 3);
					val3.Dispose();
					val3 = null;
					if (GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane)
					{
						smoothingMode = graphics.SmoothingMode;
						graphics.SmoothingMode = (SmoothingMode)0;
						if (m_CaptionBackColor.IsEmpty)
						{
							if (LayoutType == eLayoutType.Toolbar || m_HasFocus)
							{
								DisplayHelp.FillRectangle(graphics, rectangle4, itemPaintArgs.Colors.BarCaptionBackground, itemPaintArgs.Colors.BarCaptionBackground2, itemPaintArgs.Colors.BarCaptionBackgroundGradientAngle);
							}
							else
							{
								DisplayHelp.FillRectangle(graphics, rectangle4, itemPaintArgs.Colors.BarCaptionInactiveBackground, itemPaintArgs.Colors.BarCaptionInactiveBackground2, itemPaintArgs.Colors.BarCaptionInactiveBackgroundGAngle);
							}
						}
						else
						{
							DisplayHelp.FillRectangle(graphics, new Rectangle(3, 3, ((Control)this).Width - 6, GetGrabHandleCaptionHeight()), m_CaptionBackColor, Color.Empty);
						}
						graphics.SmoothingMode = smoothingMode;
					}
				}
				rectangle2 = rectangle4;
			}
			rectangle2.Inflate(-1, -1);
			if (GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane)
			{
				if (CanHideResolved)
				{
					m_SystemButtons.CloseButtonRect = new Rectangle(rectangle2.Right - (m_SystemButtons.ButtonSize.Width + 3), rectangle2.Y + (rectangle2.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CloseButton, m_SystemButtons.CloseButtonRect);
					OnPreRender(renderBarEventArgs);
					m_SystemButtons.CloseButtonRect = renderBarEventArgs.Bounds;
					rectangle2.Width -= m_SystemButtons.CloseButtonRect.Width + 3;
					if (!renderBarEventArgs.Cancel)
					{
						PaintCloseButton(itemPaintArgs);
					}
				}
				if (ShowCustomizeMenuButton)
				{
					m_SystemButtons.CustomizeButtonRect = new Rectangle(rectangle2.Right - (m_SystemButtons.ButtonSize.Width + 1), rectangle2.Y + (rectangle2.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CustomizeButton, m_SystemButtons.CustomizeButtonRect);
					OnPreRender(renderBarEventArgs);
					m_SystemButtons.CustomizeButtonRect = renderBarEventArgs.Bounds;
					rectangle2.Width -= m_SystemButtons.CustomizeButtonRect.Width + 2;
					if (!renderBarEventArgs.Cancel)
					{
						PaintCustomizeButton(itemPaintArgs);
					}
				}
				rectangle2.X += 2;
				rectangle2.Width -= 2;
				if (rectangle2.Width > 0 && flag)
				{
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CaptionText, rectangle2);
					OnPreRender(renderBarEventArgs);
					if (!renderBarEventArgs.Cancel)
					{
						Font val4 = null;
						try
						{
							val4 = new Font(((Control)this).Font, (FontStyle)1);
						}
						catch
						{
							val4 = SystemFonts.MenuFont;
						}
						eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
						if (m_CaptionForeColor.IsEmpty)
						{
							if (DrawThemedCaption)
							{
								if (m_HasFocus)
								{
									state = ThemeWindowStates.CaptionActive;
								}
								else
								{
									state = ThemeWindowStates.CaptionInactive;
								}
								rectangle2.Y++;
							}
							TextDrawing.DrawString(graphics, ((Control)this).Text, val4, (m_HasFocus || LayoutType == eLayoutType.Toolbar) ? itemPaintArgs.Colors.BarCaptionText : itemPaintArgs.Colors.BarCaptionInactiveText, rectangle2, format);
						}
						else
						{
							TextDrawing.DrawString(graphics, ((Control)this).Text, val4, m_CaptionForeColor, rectangle2, format);
						}
						val4.Dispose();
						val4 = null;
					}
				}
			}
			else
			{
				PaintGrabHandle(itemPaintArgs);
			}
		}
		else if (m_BarState == eBarState.Docked && BarFunctions.IsOffice2007Style(Style) && GetRenderer() != null && (!IsThemed || MenuBar))
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).DisplayRectangle);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel && ((Control)this).BackColor != Color.Transparent)
			{
				ToolbarRendererEventArgs toolbarRendererEventArgs3 = new ToolbarRendererEventArgs(this, graphics, ((Control)this).DisplayRectangle);
				toolbarRendererEventArgs3.ItemPaintArgs = itemPaintArgs;
				GetRenderer().DrawToolbarBackground(toolbarRendererEventArgs3);
			}
			else
			{
				PaintGrabHandle(itemPaintArgs);
			}
		}
		else
		{
			val.Dispose();
			val = null;
			bool flag2 = true;
			if (m_ItemContainer.BackgroundColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
			{
				if (IsThemed)
				{
					Rectangle r = new Rectangle(-Location.X, -Location.Y, ((Control)this).Parent.Width, ((Control)this).Parent.Height);
					ThemeRebar themeRebar = ThemeRebar;
					themeRebar.DrawBackground(graphics, ThemeRebarParts.Background, ThemeRebarStates.Normal, r);
				}
				else
				{
					smoothingMode = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)0;
					renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).ClientRectangle);
					OnPreRender(renderBarEventArgs);
					if (!renderBarEventArgs.Cancel)
					{
						if (MenuBar)
						{
							if (!itemPaintArgs.Colors.MenuBarBackground2.IsEmpty && itemPaintArgs.Colors.MenuBarBackground2.A > 0)
							{
								LinearGradientBrush val5 = BarFunctions.CreateLinearGradientBrush(((Control)this).ClientRectangle, itemPaintArgs.Colors.MenuBarBackground, itemPaintArgs.Colors.MenuBarBackground2, itemPaintArgs.Colors.MenuBarBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val5, ((Control)this).ClientRectangle);
								((Brush)val5).Dispose();
							}
							else if (IsGradientStyle && ((Control)this).Parent != null && !itemPaintArgs.Colors.DockSiteBackColor2.IsEmpty && !itemPaintArgs.Colors.DockSiteBackColor.IsEmpty)
							{
								LinearGradientBrush val6 = BarFunctions.CreateLinearGradientBrush(new Rectangle(-((Control)this).Left, -((Control)this).Top, ((Control)this).Parent.Width, ((Control)this).Parent.Height), itemPaintArgs.Colors.DockSiteBackColor, itemPaintArgs.Colors.DockSiteBackColor2, 0f);
								graphics.FillRectangle((Brush)(object)val6, -((Control)this).Left, -((Control)this).Top, ((Control)this).Parent.Width, ((Control)this).Parent.Height);
								((Brush)val6).Dispose();
							}
							else
							{
								SolidBrush val7 = new SolidBrush(itemPaintArgs.Colors.MenuBarBackground);
								try
								{
									graphics.FillRectangle((Brush)(object)val7, ((Control)this).DisplayRectangle);
								}
								finally
								{
									((IDisposable)val7)?.Dispose();
								}
							}
						}
						else if (Style == eDotNetBarStyle.VS2005 && LayoutType == eLayoutType.DockContainer && !((Control)this).BackColor.IsEmpty)
						{
							DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor, Color.Empty);
						}
						else if (GradientBackground)
						{
							DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, itemPaintArgs.Colors.BarBackground, itemPaintArgs.Colors.BarBackground2, itemPaintArgs.Colors.BarBackgroundGradientAngle);
						}
						else
						{
							SolidBrush val8 = new SolidBrush(itemPaintArgs.Colors.BarBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val8, ((Control)this).ClientRectangle);
							}
							finally
							{
								((IDisposable)val8)?.Dispose();
							}
						}
					}
					else
					{
						flag2 = false;
					}
					graphics.SmoothingMode = smoothingMode;
				}
			}
			else if (!m_ItemContainer.BackColor.IsEmpty)
			{
				smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Background, ((Control)this).DisplayRectangle);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					SolidBrush val9 = new SolidBrush(m_ItemContainer.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val9, ((Control)this).DisplayRectangle);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
				else
				{
					flag2 = false;
				}
				graphics.SmoothingMode = smoothingMode;
			}
			if (((Control)this).Parent != null && ((Control)this).Parent.BackgroundImage != null && ((Control)this).Parent is DockSite)
			{
				Rectangle targetRect = new Rectangle(-Location.X, -Location.Y, ((Control)this).Parent.Width, ((Control)this).Parent.Height);
				DockSite dockSite = ((Control)this).Parent as DockSite;
				BarFunctions.PaintBackgroundImage(graphics, targetRect, ((Control)dockSite).BackgroundImage, dockSite.BackgroundImagePosition, dockSite.BackgroundImageAlpha);
			}
			else
			{
				PaintBackgroundImage(itemPaintArgs.Graphics);
			}
			if (flag2)
			{
				if (IsGradientStyle && !IsThemed && LayoutType == eLayoutType.Toolbar && !MenuBar && BarType == eBarType.Toolbar && ((Control)this).BackColor != Color.Transparent)
				{
					if (val != null)
					{
						val.Dispose();
					}
					val = new Pen(itemPaintArgs.Colors.BarDockedBorder, 1f);
					graphics.DrawLine(val, 0, ((Control)this).Height - 1, ((Control)this).Width, ((Control)this).Height - 1);
					val.Dispose();
					val = null;
				}
				else
				{
					Rectangle clientRectangle = ((Control)this).ClientRectangle;
					clientRectangle.Inflate(-2, -2);
					BarFunctions.DrawBorder(graphics, m_DockedBorder, clientRectangle, m_SingleLineColor);
				}
			}
			PaintGrabHandle(itemPaintArgs);
		}
		m_ItemContainer.Paint(itemPaintArgs);
		if (m_BarType == eBarType.StatusBar && GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
		{
			PaintGrabHandle(itemPaintArgs);
		}
		if (val != null)
		{
			val.Dispose();
		}
		graphics.SmoothingMode = smoothingMode;
		graphics.TextRenderingHint = textRenderingHint;
		if (HasPostRender)
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.All, ((Control)this).ClientRectangle);
			OnPostRender(renderBarEventArgs);
		}
		Pen val10 = new Pen(Color.FromArgb(195, 195, 195));
		if (BarType == eBarType.Toolbar)
		{
			graphics.DrawRectangle(val10, 0, 0, ((Control)this).Width - 2, ((Control)this).Height - 1);
		}
		val10.Dispose();
	}

	protected virtual void OnPreRender(RenderBarEventArgs e)
	{
		if (this.PreRender != null)
		{
			this.PreRender(this, e);
		}
	}

	protected virtual void OnPostRender(RenderBarEventArgs e)
	{
		if (this.PostRender != null)
		{
			this.PostRender(this, e);
		}
	}

	internal void PaintGrabHandle(ItemPaintArgs pa)
	{
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Expected O, but got Unknown
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Invalid comparison between Unknown and I4
		//IL_0e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e84: Invalid comparison between Unknown and I4
		//IL_133e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1345: Expected O, but got Unknown
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Expected O, but got Unknown
		//IL_0f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Expected O, but got Unknown
		//IL_1377: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Expected O, but got Unknown
		//IL_1368: Unknown result type (might be due to invalid IL or missing references)
		//IL_136f: Expected O, but got Unknown
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Expected O, but got Unknown
		//IL_119a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a1: Expected O, but got Unknown
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Expected O, but got Unknown
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Expected O, but got Unknown
		RenderBarEventArgs renderBarEventArgs = null;
		Graphics graphics = pa.Graphics;
		if (m_GrabHandleStyle == eGrabHandleStyle.None)
		{
			return;
		}
		Rectangle empty = Rectangle.Empty;
		if (m_GrabHandleStyle != eGrabHandleStyle.Caption && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted && IsThemed && m_GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
		{
			ThemeRebar themeRebar = ThemeRebar;
			ThemeRebarParts part = ThemeRebarParts.Gripper;
			if (m_ItemContainer.Orientation == eOrientation.Vertical)
			{
				part = ThemeRebarParts.GripperVert;
				empty = new Rectangle(m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, m_GrabHandleRect.Width - 4, 5);
			}
			else
			{
				empty = new Rectangle(m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, 5, m_GrabHandleRect.Height - 4);
			}
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
			OnPreRender(renderBarEventArgs);
			empty = renderBarEventArgs.Bounds;
			if (!renderBarEventArgs.Cancel)
			{
				themeRebar.DrawBackground(graphics, part, ThemeRebarStates.Normal, empty);
			}
			return;
		}
		Color color = pa.Colors.BarBackground;
		if (!m_ItemContainer.BackgroundColor.IsEmpty)
		{
			color = m_ItemContainer.m_BackgroundColor;
		}
		switch (m_GrabHandleStyle)
		{
		case eGrabHandleStyle.Single:
			if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				empty = new Rectangle(m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, 3, m_GrabHandleRect.Height - 4);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			else
			{
				empty = new Rectangle(m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, m_GrabHandleRect.Width - 4, 3);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			break;
		case eGrabHandleStyle.Double:
			if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				empty = new Rectangle(m_GrabHandleRect.X + 1, m_GrabHandleRect.Top + 1, 3, m_GrabHandleRect.Height - 2);
				BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				empty.Offset(3, 0);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			else
			{
				empty = new Rectangle(m_GrabHandleRect.X + 1, m_GrabHandleRect.Top + 1, m_GrabHandleRect.Width - 2, 3);
				BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				empty.Offset(0, 3);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, color);
				}
			}
			break;
		case eGrabHandleStyle.DoubleThin:
			if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				empty = new Rectangle(m_GrabHandleRect.X + 1, m_GrabHandleRect.Top + 2, 3, m_GrabHandleRect.Height - 4);
				BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)5, color);
				empty.Offset(3, 0);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)5, color);
				}
			}
			else
			{
				empty = new Rectangle(m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 1, m_GrabHandleRect.Width - 4, 2);
				BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)10, color);
				empty.Offset(0, 3);
				renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, empty);
				OnPreRender(renderBarEventArgs);
				if (!renderBarEventArgs.Cancel)
				{
					BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)10, color);
				}
			}
			break;
		case eGrabHandleStyle.DoubleFlat:
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				Pen val10 = new Pen(ControlPaint.Dark(color), 1f);
				if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
				{
					graphics.DrawLine(val10, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, m_GrabHandleRect.X + 2, m_GrabHandleRect.Height - 4);
					graphics.DrawLine(val10, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + 2, m_GrabHandleRect.X + 5, m_GrabHandleRect.Height - 4);
				}
				else
				{
					graphics.DrawLine(val10, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 2, m_GrabHandleRect.Width - 4, m_GrabHandleRect.Y + 2);
					graphics.DrawLine(val10, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + 5, m_GrabHandleRect.Width - 4, m_GrabHandleRect.Y + 5);
				}
				val10.Dispose();
				val10 = null;
			}
			break;
		case eGrabHandleStyle.Dotted:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			Brush val13 = (Brush)new SolidBrush(ControlPaint.Dark(color));
			Brush val14 = (Brush)new SolidBrush(ControlPaint.Light(color));
			if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				for (int num7 = 0; num7 < m_GrabHandleRect.Height - 4; num7 += 4)
				{
					graphics.FillRectangle(val13, m_GrabHandleRect.X + 1, m_GrabHandleRect.Top + 2 + num7, 2, 2);
					graphics.FillRectangle(val14, m_GrabHandleRect.X + 1, m_GrabHandleRect.Top + 2 + num7, 1, 1);
					graphics.FillRectangle(val13, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + 2 + num7, 2, 2);
					graphics.FillRectangle(val14, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + 2 + num7, 1, 1);
				}
			}
			else
			{
				for (int num8 = 0; num8 < m_GrabHandleRect.Width - 4; num8 += 4)
				{
					graphics.FillRectangle(SystemBrushes.ControlDark, m_GrabHandleRect.Left + 2 + num8, m_GrabHandleRect.Y + 1, 2, 2);
					graphics.FillRectangle(SystemBrushes.ControlLight, m_GrabHandleRect.Left + 2 + num8, m_GrabHandleRect.Y + 1, 1, 1);
					graphics.FillRectangle(SystemBrushes.ControlDark, m_GrabHandleRect.Left + 2 + num8, m_GrabHandleRect.Y + 5, 2, 2);
					graphics.FillRectangle(SystemBrushes.ControlLight, m_GrabHandleRect.Left + 2 + num8, m_GrabHandleRect.Y + 5, 1, 1);
				}
			}
			val13.Dispose();
			val14.Dispose();
			break;
		}
		case eGrabHandleStyle.Stripe:
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				for (int n = 0; n < m_GrabHandleRect.Height - 4; n += 3)
				{
					BarFunctions.DrawBorder3D(graphics, m_GrabHandleRect.X + 2, m_GrabHandleRect.Top + n + 2, 5, 2, (Border3DStyle)4, (Border3DSide)10, color);
				}
			}
			else
			{
				for (int num5 = 0; num5 < m_GrabHandleRect.Width - 4; num5 += 3)
				{
					BarFunctions.DrawBorder3D(graphics, m_GrabHandleRect.Left + num5 + 2, m_GrabHandleRect.Y + 2, 2, 5, (Border3DStyle)4, (Border3DSide)5, color);
				}
			}
			break;
		case eGrabHandleStyle.StripeFlat:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			Pen val6 = new Pen(pa.Colors.BarStripeColor, 1f);
			if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
			{
				for (int l = 2; l < m_GrabHandleRect.Height - 6; l += 2)
				{
					graphics.DrawLine(val6, m_GrabHandleRect.X + 3, m_GrabHandleRect.Top + l + 3, m_GrabHandleRect.X + 5, m_GrabHandleRect.Top + l + 3);
				}
			}
			else
			{
				for (int m = 1; m < m_GrabHandleRect.Width - 6; m += 2)
				{
					graphics.DrawLine(val6, m_GrabHandleRect.Left + m + 3, m_GrabHandleRect.Y + 2, m_GrabHandleRect.Left + m + 3, m_GrabHandleRect.Y + 5);
				}
			}
			val6.Dispose();
			break;
		}
		case eGrabHandleStyle.CaptionTaskPane:
		case eGrabHandleStyle.CaptionDotted:
		{
			Rectangle targetRect = m_GrabHandleRect;
			Brush val3 = null;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Caption, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				if (m_BarState == eBarState.Floating)
				{
					DisplayHelp.FillRectangle(graphics, targetRect, pa.Colors.BarCaptionBackground, pa.Colors.BarCaptionBackground2, pa.Colors.BarCaptionBackgroundGradientAngle);
				}
				else
				{
					if (!CaptionBackColor.IsEmpty)
					{
						val3 = (Brush)new SolidBrush(CaptionBackColor);
					}
					val3 = (Brush)((!pa.Colors.BarBackground2.IsEmpty) ? ((object)BarFunctions.CreateLinearGradientBrush(targetRect, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle)) : ((object)((!m_HasFocus || m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted) ? new SolidBrush(pa.Colors.BarBackground) : new SolidBrush(pa.Colors.BarCaptionBackground))));
					graphics.FillRectangle(val3, targetRect);
					val3.Dispose();
					val3 = null;
				}
				Brush val4 = null;
				Brush val5 = (Brush)new SolidBrush(ControlPaint.Light(color));
				if (!pa.Colors.BarStripeColor.IsEmpty)
				{
					color = pa.Colors.BarStripeColor;
					val4 = (Brush)new SolidBrush(color);
				}
				else
				{
					val4 = (Brush)new SolidBrush(ControlPaint.Dark(color));
				}
				int num3 = m_GrabHandleRect.Top + 4;
				int num4 = m_GrabHandleRect.Left + 4;
				for (int k = num3; k <= m_GrabHandleRect.Bottom - 4; k += 5)
				{
					graphics.FillRectangle(val5, num4 + 1, k + 1, 2, 2);
					graphics.FillRectangle(val4, num4, k, 2, 2);
				}
				if (val5 != null)
				{
					val5.Dispose();
				}
				if (val4 != null)
				{
					val4.Dispose();
				}
			}
			targetRect.X += 8;
			targetRect.Width -= 8;
			targetRect.Inflate(-1, -1);
			PaintGrabHandleButtons(pa, ref targetRect);
			if (m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
			{
				PaintCaptionText(pa, targetRect);
			}
			break;
		}
		case eGrabHandleStyle.Caption:
		{
			Rectangle targetRect2 = m_GrabHandleRect;
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.Caption, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (!renderBarEventArgs.Cancel)
			{
				ThemeWindow themeWindow = null;
				ThemeWindowParts smallCaption = ThemeWindowParts.SmallCaption;
				ThemeWindowStates state = ThemeWindowStates.CaptionInactive;
				if (DrawThemedCaption)
				{
					themeWindow = ThemeWindow;
				}
				if (m_DockedBorder == eBorderType.None)
				{
					targetRect2.Inflate(-1, 0);
				}
				if (m_CaptionBackColor.IsEmpty)
				{
					if (Style == eDotNetBarStyle.Office2000)
					{
						if (m_HasFocus)
						{
							graphics.FillRectangle(SystemBrushes.ActiveCaption, targetRect2);
						}
						else
						{
							graphics.FillRectangle(SystemBrushes.InactiveCaption, targetRect2);
						}
					}
					else if (DrawThemedCaption)
					{
						if (m_HasFocus)
						{
							state = ThemeWindowStates.CaptionActive;
						}
						themeWindow.DrawBackground(graphics, smallCaption, state, targetRect2);
					}
					else if (m_HasFocus)
					{
						if (pa.Colors.BarCaptionBackground2.IsEmpty)
						{
							DisplayHelp.FillRectangle(graphics, targetRect2, pa.Colors.BarCaptionBackground);
						}
						else
						{
							LinearGradientBrush val7 = BarFunctions.CreateLinearGradientBrush(targetRect2, pa.Colors.BarCaptionBackground, pa.Colors.BarCaptionBackground2, pa.Colors.BarCaptionBackgroundGradientAngle);
							try
							{
								graphics.FillRectangle((Brush)(object)val7, targetRect2);
							}
							finally
							{
								((IDisposable)val7)?.Dispose();
							}
						}
					}
					else
					{
						if (pa.Colors.BarCaptionInactiveBackground2.IsEmpty)
						{
							DisplayHelp.FillRectangle(graphics, targetRect2, pa.Colors.BarCaptionInactiveBackground);
						}
						else
						{
							LinearGradientBrush val8 = BarFunctions.CreateLinearGradientBrush(targetRect2, pa.Colors.BarCaptionInactiveBackground, pa.Colors.BarCaptionInactiveBackground2, pa.Colors.BarCaptionInactiveBackgroundGAngle);
							try
							{
								graphics.FillRectangle((Brush)(object)val8, targetRect2);
							}
							finally
							{
								((IDisposable)val8)?.Dispose();
							}
						}
						eDotNetBarStyle effectiveStyle = m_ItemContainer.EffectiveStyle;
						if (effectiveStyle != eDotNetBarStyle.VS2005 && effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Metro)
						{
							Pen val9 = new Pen(pa.Colors.BarCaptionBackground, 1f);
							graphics.DrawLine(val9, targetRect2.X + 1, targetRect2.Y, targetRect2.Right - 2, targetRect2.Y);
							graphics.DrawLine(val9, targetRect2.X + 1, targetRect2.Bottom - 1, targetRect2.Right - 2, targetRect2.Bottom - 1);
							graphics.DrawLine(val9, targetRect2.X, targetRect2.Y + 1, targetRect2.X, targetRect2.Bottom - 2);
							graphics.DrawLine(val9, targetRect2.Right - 1, targetRect2.Y + 1, targetRect2.Right - 1, targetRect2.Bottom - 2);
							val9.Dispose();
						}
					}
				}
				else
				{
					DisplayHelp.FillRectangle(graphics, targetRect2, m_CaptionBackColor);
				}
			}
			targetRect2.Inflate(-1, -1);
			PaintGrabHandleButtons(pa, ref targetRect2);
			PaintCaptionText(pa, targetRect2);
			break;
		}
		case eGrabHandleStyle.ResizeHandle:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.ResizeHandle, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Metro)
			{
				break;
			}
			Form val11 = ((Control)this).FindForm();
			if (val11 == null || (int)val11.WindowState != 2)
			{
				int num6 = 1;
				Point point = new Point(((Control)this).ClientRectangle.Right, ((Control)this).ClientRectangle.Bottom);
				if ((int)((Control)this).RightToLeft == 1)
				{
					num6 = -1;
					point = new Point(0, ((Control)this).ClientRectangle.Bottom - 2);
				}
				ColorFunctions.HLSColor hLSColor = ColorFunctions.RGBToHSL(color);
				Color color2 = ColorFunctions.HLSToRGB(hLSColor.Hue, hLSColor.Lightness + 0.3, hLSColor.Saturation);
				Pen val12 = new Pen(color2, 1f);
				graphics.DrawLine(val12, point.X - 2 * num6, point.Y - 2, point.X - 3 * num6, point.Y - 2);
				graphics.DrawLine(val12, point.X - 6 * num6, point.Y - 2, point.X - 7 * num6, point.Y - 2);
				graphics.DrawLine(val12, point.X - 10 * num6, point.Y - 2, point.X - 11 * num6, point.Y - 2);
				graphics.DrawLine(val12, point.X - 2 * num6, point.Y - 2, point.X - 2 * num6, point.Y - 3);
				graphics.DrawLine(val12, point.X - 6 * num6, point.Y - 2, point.X - 6 * num6, point.Y - 3);
				graphics.DrawLine(val12, point.X - 10 * num6, point.Y - 2, point.X - 10 * num6, point.Y - 3);
				graphics.DrawLine(val12, point.X - 2 * num6, point.Y - 6, point.X - 3 * num6, point.Y - 6);
				graphics.DrawLine(val12, point.X - 6 * num6, point.Y - 6, point.X - 7 * num6, point.Y - 6);
				graphics.DrawLine(val12, point.X - 2 * num6, point.Y - 6, point.X - 2 * num6, point.Y - 7);
				graphics.DrawLine(val12, point.X - 6 * num6, point.Y - 6, point.X - 6 * num6, point.Y - 7);
				graphics.DrawLine(val12, point.X - 2 * num6, point.Y - 10, point.X - 3 * num6, point.Y - 10);
				graphics.DrawLine(val12, point.X - 2 * num6, point.Y - 10, point.X - 2 * num6, point.Y - 11);
				val12.Dispose();
				if (m_ItemContainer.EffectiveStyle != eDotNetBarStyle.Metro)
				{
					val12 = new Pen(pa.Colors.BarStripeColor, 1f);
					graphics.DrawRectangle(val12, point.X - 4 * num6, point.Y - 4, 1, 1);
					graphics.DrawRectangle(val12, point.X - 8 * num6, point.Y - 4, 1, 1);
					graphics.DrawRectangle(val12, point.X - 12 * num6, point.Y - 4, 1, 1);
					graphics.DrawRectangle(val12, point.X - 4 * num6, point.Y - 8, 1, 1);
					graphics.DrawRectangle(val12, point.X - 8 * num6, point.Y - 8, 1, 1);
					graphics.DrawRectangle(val12, point.X - 4 * num6, point.Y - 12, 1, 1);
					val12.Dispose();
					val12 = null;
				}
			}
			break;
		}
		case eGrabHandleStyle.Office2003:
		case eGrabHandleStyle.Office2003SingleDot:
		{
			renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.GrabHandle, m_GrabHandleRect);
			OnPreRender(renderBarEventArgs);
			if (renderBarEventArgs.Cancel)
			{
				break;
			}
			int num = m_GrabHandleRect.Left + (m_GrabHandleRect.Width - 3) / 2;
			int num2 = m_GrabHandleRect.Top + 4;
			if (m_GrabHandleStyle == eGrabHandleStyle.Office2003SingleDot)
			{
				num = m_GrabHandleRect.Left + (m_GrabHandleRect.Width - 3) / 2;
				num2 = m_GrabHandleRect.Top + (m_GrabHandleRect.Height - 3) / 2;
			}
			else if (m_ItemContainer.Orientation == eOrientation.Vertical)
			{
				num = m_GrabHandleRect.Left + 4;
				num2 = m_GrabHandleRect.Top + (m_GrabHandleRect.Height - 3) / 2;
			}
			Brush val = null;
			Brush val2 = (Brush)new SolidBrush(ControlPaint.Light(color));
			if (!pa.Colors.BarStripeColor.IsEmpty)
			{
				color = pa.Colors.BarStripeColor;
				val = (Brush)new SolidBrush(color);
			}
			else
			{
				val = (Brush)new SolidBrush(ControlPaint.Dark(color));
			}
			if (m_GrabHandleStyle == eGrabHandleStyle.Office2003SingleDot)
			{
				graphics.FillRectangle(val2, num + 1, num2 + 1, 2, 2);
				graphics.FillRectangle(val, num, num2, 2, 2);
			}
			else if (m_ItemContainer.Orientation == eOrientation.Vertical)
			{
				for (int i = num; i <= m_GrabHandleRect.Right - 4; i += 5)
				{
					graphics.FillRectangle(val2, i + 1, num2 + 1, 2, 2);
					graphics.FillRectangle(val, i, num2, 2, 2);
				}
			}
			else
			{
				for (int j = num2; j <= m_GrabHandleRect.Bottom - 4; j += 5)
				{
					graphics.FillRectangle(val2, num + 1, j + 1, 2, 2);
					graphics.FillRectangle(val, num, j, 2, 2);
				}
			}
			val.Dispose();
			val2.Dispose();
			break;
		}
		}
	}

	private void PaintCaptionText(ItemPaintArgs pa, Rectangle targetRect)
	{
		Graphics graphics = pa.Graphics;
		RenderBarEventArgs renderBarEventArgs = new RenderBarEventArgs(this, graphics, eBarRenderPart.CaptionText, targetRect);
		OnPreRender(renderBarEventArgs);
		if (renderBarEventArgs.Cancel)
		{
			return;
		}
		Font val = null;
		bool flag = false;
		try
		{
			object obj = ((Control)this).Font.Clone();
			val = (Font)((obj is Font) ? obj : null);
			flag = true;
		}
		catch
		{
			val = SystemFonts.MenuFont;
			flag = true;
		}
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		if (m_CaptionForeColor.IsEmpty)
		{
			if (IsThemed)
			{
				if (targetRect.Height < val.Height)
				{
					targetRect.Height = val.Height;
				}
				if (targetRect.Width > 0)
				{
					if (!m_HasFocus)
					{
						TextDrawing.DrawString(graphics, ((Control)this).Text, val, SystemColors.WindowText, targetRect, format);
					}
					else
					{
						TextDrawing.DrawString(graphics, ((Control)this).Text, val, SystemColors.ActiveCaptionText, targetRect, format);
					}
				}
			}
			else
			{
				if (targetRect.Height < val.Height)
				{
					targetRect.Height = val.Height;
				}
				if (targetRect.Width > 0)
				{
					Color color = pa.Colors.BarCaptionText;
					if (m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
					{
						color = pa.Colors.ItemText;
					}
					else if ((Style == eDotNetBarStyle.OfficeXP || Style == eDotNetBarStyle.Office2003 || Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(Style)) && !m_HasFocus)
					{
						color = pa.Colors.BarCaptionInactiveText;
					}
					TextDrawing.DrawString(graphics, ((Control)this).Text, val, color, targetRect, format);
				}
			}
		}
		else if (targetRect.Width > 0)
		{
			TextDrawing.DrawString(graphics, ((Control)this).Text, val, m_CaptionForeColor, targetRect, format);
		}
		if (flag)
		{
			val.Dispose();
		}
	}

	private bool GetPaintAutoHidePin()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		bool result = false;
		if (m_CanAutoHide && m_ItemContainer.LayoutType == eLayoutType.DockContainer && m_BarState != eBarState.Floating)
		{
			result = ((!(((Control)this).Parent is DockSite) || (int)((Control)this).Parent.Dock != 5) ? true : false);
		}
		return result;
	}

	private void PaintGrabHandleButtons(ItemPaintArgs pa, ref Rectangle targetRect)
	{
		RenderBarEventArgs renderBarEventArgs = null;
		if (CanHideResolved)
		{
			if (Style == eDotNetBarStyle.Office2000)
			{
				m_SystemButtons.CloseButtonRect = new Rectangle(targetRect.Right - 11, targetRect.Y + (targetRect.Height - m_SystemButtons.ButtonSize.Width) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
			}
			else
			{
				m_SystemButtons.CloseButtonRect = new Rectangle(targetRect.Right - (m_SystemButtons.ButtonSize.Width + 1), targetRect.Y + (targetRect.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
			}
			renderBarEventArgs = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.CloseButton, m_SystemButtons.CloseButtonRect);
			OnPreRender(renderBarEventArgs);
			m_SystemButtons.CloseButtonRect = renderBarEventArgs.Bounds;
			targetRect.Width -= m_SystemButtons.CloseButtonRect.Width + 3;
			if (!renderBarEventArgs.Cancel)
			{
				PaintCloseButton(pa);
			}
		}
		if (GetPaintAutoHidePin())
		{
			if (Style == eDotNetBarStyle.Office2000)
			{
				m_SystemButtons.AutoHideButtonRect = new Rectangle(targetRect.Right - (m_SystemButtons.ButtonSize.Width - 1), targetRect.Y + (targetRect.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
			}
			else
			{
				m_SystemButtons.AutoHideButtonRect = new Rectangle(targetRect.Right - (m_SystemButtons.ButtonSize.Width + 1), targetRect.Y + (targetRect.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
			}
			renderBarEventArgs = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.AutoHideButton, m_SystemButtons.AutoHideButtonRect);
			OnPreRender(renderBarEventArgs);
			m_SystemButtons.AutoHideButtonRect = renderBarEventArgs.Bounds;
			targetRect.Width -= m_SystemButtons.AutoHideButtonRect.Width + 3;
			if (!renderBarEventArgs.Cancel)
			{
				PaintAutoHideButton(pa);
			}
		}
		if (ShowCustomizeMenuButton)
		{
			renderBarEventArgs = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.CustomizeButton, Rectangle.Empty);
			if (Style == eDotNetBarStyle.Office2000)
			{
				m_SystemButtons.CustomizeButtonRect = new Rectangle(targetRect.X, targetRect.Y + (targetRect.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
				renderBarEventArgs.Bounds = m_SystemButtons.CustomizeButtonRect;
				OnPreRender(renderBarEventArgs);
				m_SystemButtons.CustomizeButtonRect = renderBarEventArgs.Bounds;
				targetRect.Width -= m_SystemButtons.CustomizeButtonRect.Width + 2;
				targetRect.X += m_SystemButtons.CustomizeButtonRect.Width + 2;
			}
			else
			{
				m_SystemButtons.CustomizeButtonRect = new Rectangle(targetRect.Right - (m_SystemButtons.ButtonSize.Width + 1), targetRect.Y + (targetRect.Height - m_SystemButtons.ButtonSize.Height) / 2, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
				renderBarEventArgs.Bounds = m_SystemButtons.CustomizeButtonRect;
				OnPreRender(renderBarEventArgs);
				m_SystemButtons.CustomizeButtonRect = renderBarEventArgs.Bounds;
				targetRect.Width -= m_SystemButtons.CustomizeButtonRect.Width + 2;
			}
			if (!renderBarEventArgs.Cancel)
			{
				PaintCustomizeButton(pa);
			}
		}
		if (GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane)
		{
			targetRect.X += 2;
			targetRect.Width -= 2;
			targetRect.Inflate(0, -2);
			m_SystemButtons.CaptionButtonRect = targetRect;
			renderBarEventArgs = new RenderBarEventArgs(this, pa.Graphics, eBarRenderPart.CaptionTaskPane, m_SystemButtons.CaptionButtonRect);
			OnPreRender(renderBarEventArgs);
			m_SystemButtons.CaptionButtonRect = renderBarEventArgs.Bounds;
			if (!renderBarEventArgs.Cancel)
			{
				PaintCaptionButton(pa);
			}
		}
		targetRect.X += 2;
		targetRect.Width -= 2;
	}

	protected void PaintOffice(Graphics g)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		m_SystemButtons.CustomizeButtonRect = Rectangle.Empty;
		m_SystemButtons.CloseButtonRect = Rectangle.Empty;
		if (m_ItemContainer.BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(SystemColors.Control);
			try
			{
				g.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(m_ItemContainer.BackColor);
			try
			{
				g.FillRectangle((Brush)(object)val2, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		ColorScheme colorScheme = GetColorScheme();
		PaintSideBar(g);
		ItemPaintArgs itemPaintArgs = GetItemPaintArgs(g);
		if (m_ItemContainer.SubItems.Count <= 0)
		{
			return;
		}
		if (m_BarState == eBarState.Popup)
		{
			ControlPaint.DrawBorder3D(g, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height, (Border3DStyle)5);
			PaintSideBar(g);
		}
		else if (m_BarState == eBarState.Floating)
		{
			ControlPaint.DrawBorder3D(g, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height, (Border3DStyle)5);
			if (m_CaptionBackColor.IsEmpty)
			{
				g.FillRectangle(SystemBrushes.ActiveCaption, 4, 4, ((Control)this).ClientSize.Width - 8, 15);
			}
			else
			{
				SolidBrush val3 = new SolidBrush(m_CaptionBackColor);
				try
				{
					g.FillRectangle((Brush)(object)val3, 4, 4, ((Control)this).ClientSize.Width - 8, 15);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			Font val4 = null;
			bool flag = false;
			try
			{
				val4 = new Font(((Control)this).Font, (FontStyle)1);
				flag = true;
			}
			catch
			{
				val4 = SystemFonts.MenuFont;
				flag = true;
			}
			Rectangle bounds = Rectangle.Empty;
			if (ShowCustomizeMenuButton)
			{
				bounds = new Rectangle(18, 4, ((Control)this).ClientSize.Width - 22, 15);
				m_SystemButtons.CustomizeButtonRect = new Rectangle(5, 6, 15, 14);
				PaintCustomizeButton(itemPaintArgs);
			}
			else
			{
				bounds = new Rectangle(4, 4, ((Control)this).ClientSize.Width - 8, 15);
			}
			if (CanHideResolved)
			{
				m_SystemButtons.CloseButtonRect = new Rectangle(bounds.Right - m_SystemButtons.ButtonSize.Width, 6, m_SystemButtons.ButtonSize.Width, m_SystemButtons.ButtonSize.Height);
				PaintCloseButton(itemPaintArgs);
				bounds.Width -= m_SystemButtons.ButtonSize.Width + 1;
			}
			if (m_CaptionForeColor.IsEmpty)
			{
				TextDrawing.DrawString(g, ((Control)this).Text, val4, SystemColors.ActiveCaptionText, bounds, eTextFormat.Default);
			}
			else
			{
				TextDrawing.DrawString(g, ((Control)this).Text, val4, m_CaptionForeColor, bounds, eTextFormat.Default);
			}
			if (flag)
			{
				val4.Dispose();
			}
		}
		else
		{
			BarFunctions.DrawBorder(g, m_DockedBorder, ((Control)this).ClientRectangle, m_SingleLineColor);
			PaintGrabHandle(itemPaintArgs);
		}
		m_ItemContainer.Paint(itemPaintArgs);
	}

	private void PaintCloseButton()
	{
		((Control)this).Invalidate(m_SystemButtons.CloseButtonRect);
		((Control)this).Update();
	}

	private void PaintCloseButton(ItemPaintArgs pa)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Expected O, but got Unknown
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		if (m_SystemButtons.CloseButtonRect.IsEmpty)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		if (DrawThemedCaption)
		{
			ThemeWindow themeWindow = ThemeWindow;
			ThemeWindowParts smallCloseButton = ThemeWindowParts.SmallCloseButton;
			ThemeWindowStates state = ThemeWindowStates.ButtonNormal;
			if (m_SystemButtons.MouseDownClose)
			{
				state = ThemeWindowStates.ButtonPushed;
			}
			else if (m_SystemButtons.MouseOverClose)
			{
				state = ThemeWindowStates.ButtonHot;
			}
			themeWindow.DrawBackground(graphics, smallCloseButton, state, m_SystemButtons.CloseButtonRect);
			return;
		}
		if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			ButtonState val = (ButtonState)0;
			if (m_SystemButtons.MouseDownClose)
			{
				val = (ButtonState)512;
			}
			ControlPaint.DrawCaptionButton(graphics, m_SystemButtons.CloseButtonRect, (CaptionButton)0, val);
			return;
		}
		Pen val2 = null;
		if (m_SystemButtons.MouseDownClose)
		{
			if (pa.Colors.ItemPressedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, m_SystemButtons.CloseButtonRect, pa.Colors.ItemPressedBackground);
			}
			else
			{
				LinearGradientBrush val3 = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CloseButtonRect, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val3, m_SystemButtons.CloseButtonRect);
				((Brush)val3).Dispose();
			}
			Pen val4 = new Pen(pa.Colors.ItemPressedBorder);
			try
			{
				NativeFunctions.DrawRectangle(graphics, val4, m_SystemButtons.CloseButtonRect);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		else if (m_SystemButtons.MouseOverClose)
		{
			val2 = new Pen(pa.Colors.ItemHotText);
			if (!pa.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val5 = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CloseButtonRect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val5, m_SystemButtons.CloseButtonRect);
				((Brush)val5).Dispose();
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, m_SystemButtons.CloseButtonRect, pa.Colors.ItemHotBackground);
			}
			val2.Dispose();
			val2 = null;
			Pen val6 = new Pen(pa.Colors.ItemHotBorder);
			try
			{
				NativeFunctions.DrawRectangle(graphics, val6, m_SystemButtons.CloseButtonRect);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		val2 = ((m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted) ? new Pen((m_HasFocus || LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText) : new Pen(pa.Colors.ItemText));
		if (m_SystemButtons.MouseOverClose)
		{
			val2 = new Pen(pa.Colors.ItemHotText, 1f);
		}
		else if (m_SystemButtons.MouseDownClose)
		{
			val2 = new Pen(pa.Colors.ItemPressedText, 1f);
		}
		else if (!m_CaptionForeColor.IsEmpty)
		{
			val2 = new Pen(m_CaptionForeColor, 1f);
		}
		else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
		{
			val2 = new Pen(pa.Colors.BarCaptionInactiveText);
		}
		Point[] array = new Point[2];
		Rectangle closeButtonRect = m_SystemButtons.CloseButtonRect;
		closeButtonRect.Inflate(-1, -1);
		array[0].X = closeButtonRect.Left + 2;
		array[0].Y = closeButtonRect.Top + 3;
		array[1].X = closeButtonRect.Right - 4;
		array[1].Y = closeButtonRect.Bottom - 4;
		graphics.DrawLine(val2, array[0], array[1]);
		array[0].X++;
		array[1].X++;
		graphics.DrawLine(val2, array[0], array[1]);
		array[0].X = closeButtonRect.Right - 3;
		array[0].Y = closeButtonRect.Top + 3;
		array[1].X = closeButtonRect.Left + 3;
		array[1].Y = closeButtonRect.Bottom - 4;
		graphics.DrawLine(val2, array[0], array[1]);
		array[0].X--;
		array[1].X--;
		graphics.DrawLine(val2, array[0], array[1]);
		val2.Dispose();
	}

	private void PaintCaptionButton()
	{
		((Control)this).Invalidate(m_SystemButtons.CaptionButtonRect);
		((Control)this).Update();
	}

	private void PaintCaptionButton(ItemPaintArgs pa)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		if (m_SystemButtons.CaptionButtonRect.IsEmpty)
		{
			return;
		}
		Rectangle captionButtonRect = m_SystemButtons.CaptionButtonRect;
		Graphics graphics = pa.Graphics;
		if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			ButtonState val = (ButtonState)0;
			if (m_SystemButtons.MouseDownCaption)
			{
				val = (ButtonState)512;
			}
			ControlPaint.DrawButton(graphics, captionButtonRect, val);
		}
		else if (m_CaptionMenu != null && m_CaptionMenu.Expanded)
		{
			if (pa.Colors.ItemExpandedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, captionButtonRect, pa.Colors.ItemExpandedBackground);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(captionButtonRect, pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, captionButtonRect);
				((Brush)val2).Dispose();
			}
			Pen val3 = new Pen(pa.Colors.ItemExpandedBorder);
			try
			{
				NativeFunctions.DrawRectangle(graphics, val3, captionButtonRect);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		else if (m_SystemButtons.MouseDownCaption)
		{
			if (pa.Colors.ItemPressedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, captionButtonRect, pa.Colors.ItemPressedBackground);
			}
			else
			{
				LinearGradientBrush val4 = BarFunctions.CreateLinearGradientBrush(captionButtonRect, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val4, captionButtonRect);
				((Brush)val4).Dispose();
			}
			Pen val5 = new Pen(pa.Colors.ItemPressedBorder);
			try
			{
				NativeFunctions.DrawRectangle(graphics, val5, captionButtonRect);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		else if (m_SystemButtons.MouseOverCaption)
		{
			Pen val6 = new Pen(pa.Colors.ItemHotText);
			if (!pa.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val7 = BarFunctions.CreateLinearGradientBrush(captionButtonRect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val7, captionButtonRect);
				((Brush)val7).Dispose();
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, captionButtonRect, pa.Colors.ItemHotBackground);
			}
			Pen val8 = new Pen(pa.Colors.ItemHotBorder);
			try
			{
				NativeFunctions.DrawRectangle(graphics, val8, captionButtonRect);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		SolidBrush val9 = null;
		Color itemText = pa.Colors.ItemText;
		itemText = (m_SystemButtons.MouseOverCaption ? pa.Colors.ItemHotText : (m_SystemButtons.MouseDownCaption ? pa.Colors.ItemPressedText : (m_CaptionForeColor.IsEmpty ? pa.Colors.ItemText : m_CaptionForeColor)));
		val9 = new SolidBrush(itemText);
		eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		Rectangle bounds = captionButtonRect;
		bounds.Width -= 12;
		bounds.X += 3;
		if (bounds.Width > 8)
		{
			TextDrawing.DrawString(graphics, ((Control)this).Text, ((Control)this).Font, itemText, bounds, format);
		}
		Point[] array = new Point[3];
		array[0].X = captionButtonRect.Right - 9;
		array[0].Y = captionButtonRect.Top + (captionButtonRect.Height + 6) / 2;
		array[1].X = array[0].X - 4;
		array[1].Y = array[0].Y - 5;
		array[2].X = array[1].X + 9;
		array[2].Y = array[1].Y;
		graphics.FillPolygon((Brush)(object)val9, array);
		((Brush)val9).Dispose();
	}

	private void PaintCustomizeButton()
	{
		((Control)this).Invalidate(m_SystemButtons.CustomizeButtonRect);
		((Control)this).Update();
	}

	private void PaintCustomizeButton(ItemPaintArgs pa)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Expected O, but got Unknown
		if (m_SystemButtons.CustomizeButtonRect.IsEmpty)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		Brush val = (Brush)new SolidBrush((m_HasFocus || LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText);
		if (!DrawThemedCaption)
		{
			if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				Point[] array = new Point[3];
				array[0].X = m_SystemButtons.CustomizeButtonRect.X + 4;
				array[0].Y = m_SystemButtons.CustomizeButtonRect.Y + 8;
				array[1].X = array[0].X - 3;
				array[1].Y = array[0].Y - 4;
				array[2].X = array[1].X + 7;
				array[2].Y = array[1].Y;
				graphics.FillPolygon(SystemBrushes.ActiveCaptionText, array);
				val.Dispose();
				return;
			}
			if (!m_CaptionForeColor.IsEmpty)
			{
				val.Dispose();
				val = (Brush)new SolidBrush(m_CaptionForeColor);
			}
			if (!m_HasFocus && m_BarState != eBarState.Floating)
			{
				val.Dispose();
				val = (Brush)new SolidBrush(pa.Colors.BarCaptionInactiveText);
			}
			if (m_SystemButtons.MouseDownCustomize)
			{
				if (pa.Colors.ItemPressedBackground2.IsEmpty)
				{
					SolidBrush val2 = new SolidBrush(pa.Colors.ItemPressedBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val2, m_SystemButtons.CustomizeButtonRect);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val3 = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CustomizeButtonRect, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
					graphics.FillRectangle((Brush)(object)val3, m_SystemButtons.CustomizeButtonRect);
					((Brush)val3).Dispose();
				}
				Pen val4 = new Pen(pa.Colors.ItemPressedBorder);
				try
				{
					NativeFunctions.DrawRectangle(graphics, val4, m_SystemButtons.CustomizeButtonRect);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else if (m_SystemButtons.MouseOverCustomize)
			{
				val.Dispose();
				val = (Brush)new SolidBrush(pa.Colors.BarCaptionText);
				if (!pa.Colors.ItemHotBackground2.IsEmpty)
				{
					LinearGradientBrush val5 = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.CustomizeButtonRect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
					graphics.FillRectangle((Brush)(object)val5, m_SystemButtons.CustomizeButtonRect);
					((Brush)val5).Dispose();
				}
				else
				{
					SolidBrush val6 = new SolidBrush(pa.Colors.ItemHotBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val6, m_SystemButtons.CustomizeButtonRect);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				Pen val7 = new Pen(pa.Colors.ItemHotBorder);
				try
				{
					NativeFunctions.DrawRectangle(graphics, val7, m_SystemButtons.CustomizeButtonRect);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
		}
		if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(pa.Colors.ItemText);
		}
		else
		{
			val.Dispose();
			val = (Brush)new SolidBrush((m_HasFocus || LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText);
		}
		if (m_SystemButtons.MouseOverCustomize)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(pa.Colors.ItemHotText);
		}
		else if (m_SystemButtons.MouseDownCustomize)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(pa.Colors.ItemPressedText);
		}
		else if (!m_CaptionForeColor.IsEmpty)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(m_CaptionForeColor);
		}
		else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
		{
			val.Dispose();
			val = (Brush)new SolidBrush(pa.Colors.BarCaptionInactiveText);
		}
		Point[] array2 = new Point[3];
		array2[0].X = m_SystemButtons.CustomizeButtonRect.Right - 7;
		array2[0].Y = m_SystemButtons.CustomizeButtonRect.Top + 8;
		array2[1].X = array2[0].X - 3;
		array2[1].Y = array2[0].Y - 4;
		array2[2].X = array2[1].X + 7;
		array2[2].Y = array2[1].Y;
		graphics.FillPolygon(val, array2);
		val.Dispose();
	}

	private void PaintAutoHideButton()
	{
		if (!m_SystemButtons.AutoHideButtonRect.IsEmpty)
		{
			Rectangle autoHideButtonRect = m_SystemButtons.AutoHideButtonRect;
			autoHideButtonRect.Inflate(1, 1);
			((Control)this).Invalidate(autoHideButtonRect);
			((Control)this).Update();
		}
	}

	private void PaintAutoHideButton(ItemPaintArgs pa)
	{
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Expected O, but got Unknown
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Expected O, but got Unknown
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		if (m_SystemButtons.AutoHideButtonRect.IsEmpty)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		if (DrawThemedCaption)
		{
			Image val = null;
			if ((m_SystemButtons.MouseDownAutoHide && !AutoHide) || (AutoHide && !m_SystemButtons.MouseDownAutoHide))
			{
				val = (Image)(object)((!m_SystemButtons.MouseOverAutoHide) ? BarFunctions.LoadBitmap("SystemImages.PinInactive.png") : BarFunctions.LoadBitmap("SystemImages.Pin.png"));
				graphics.DrawImage(val, m_SystemButtons.AutoHideButtonRect.X + (m_SystemButtons.AutoHideButtonRect.Width - val.Width) / 2, m_SystemButtons.AutoHideButtonRect.Y + (m_SystemButtons.AutoHideButtonRect.Height - val.Height) / 2, val.Width, val.Height);
			}
			else
			{
				val = (Image)(object)((!m_SystemButtons.MouseOverAutoHide) ? BarFunctions.LoadBitmap("SystemImages.PinPushedInactive.png") : BarFunctions.LoadBitmap("SystemImages.PinPushed.png"));
				graphics.DrawImage(val, m_SystemButtons.AutoHideButtonRect.X + (m_SystemButtons.AutoHideButtonRect.Width - val.Width) / 2, m_SystemButtons.AutoHideButtonRect.Y + (m_SystemButtons.AutoHideButtonRect.Height - val.Height) / 2, val.Width, val.Height);
			}
			return;
		}
		if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			Rectangle autoHideButtonRect = m_SystemButtons.AutoHideButtonRect;
			autoHideButtonRect.Inflate(1, 0);
			if (m_SystemButtons.MouseDownAutoHide)
			{
				ControlPaint.DrawBorder3D(graphics, autoHideButtonRect, (Border3DStyle)8, (Border3DSide)2063);
			}
			else
			{
				ControlPaint.DrawBorder3D(graphics, autoHideButtonRect, (Border3DStyle)5, (Border3DSide)2063);
			}
		}
		else if (m_SystemButtons.MouseDownAutoHide)
		{
			if (pa.Colors.ItemPressedBackground2.IsEmpty)
			{
				DisplayHelp.FillRectangle(graphics, m_SystemButtons.AutoHideButtonRect, pa.Colors.ItemPressedBackground);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.AutoHideButtonRect, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, m_SystemButtons.AutoHideButtonRect);
				((Brush)val2).Dispose();
			}
			Pen val3 = new Pen(pa.Colors.ItemPressedBorder);
			try
			{
				NativeFunctions.DrawRectangle(graphics, val3, m_SystemButtons.AutoHideButtonRect);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		else if (m_SystemButtons.MouseOverAutoHide)
		{
			if (!pa.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val4 = BarFunctions.CreateLinearGradientBrush(m_SystemButtons.AutoHideButtonRect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val4, m_SystemButtons.AutoHideButtonRect);
				((Brush)val4).Dispose();
			}
			else
			{
				DisplayHelp.FillRectangle(graphics, m_SystemButtons.AutoHideButtonRect, pa.Colors.ItemHotBackground);
			}
			Pen val5 = new Pen(pa.Colors.ItemHotBorder);
			try
			{
				NativeFunctions.DrawRectangle(graphics, val5, m_SystemButtons.AutoHideButtonRect);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if ((m_SystemButtons.MouseDownAutoHide && !AutoHide) || (AutoHide && !m_SystemButtons.MouseDownAutoHide))
		{
			Rectangle rectangle = new Rectangle(m_SystemButtons.AutoHideButtonRect.X + (m_SystemButtons.AutoHideButtonRect.Width - (int)Math.Ceiling(10.0 * StiScale2.X)) / 2, m_SystemButtons.AutoHideButtonRect.Y + (m_SystemButtons.AutoHideButtonRect.Height - (int)Math.Ceiling(7.0 * StiScale2.Y)) / 2, (int)Math.Ceiling(10.0 * StiScale2.X), (int)Math.Ceiling(7.0 * StiScale2.Y));
			Pen val6 = null;
			val6 = ((m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted) ? new Pen((m_HasFocus || LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText) : new Pen(pa.Colors.ItemText));
			if (m_SystemButtons.MouseOverAutoHide)
			{
				val6 = new Pen(pa.Colors.ItemHotText, 1f);
			}
			else if (m_SystemButtons.MouseDownAutoHide)
			{
				val6 = new Pen(pa.Colors.ItemPressedText, 1f);
			}
			else if (!m_CaptionForeColor.IsEmpty)
			{
				val6 = new Pen(m_CaptionForeColor, 1f);
			}
			else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
			{
				val6 = new Pen(pa.Colors.BarCaptionInactiveText);
			}
			graphics.DrawRectangle(val6, rectangle.X + (int)Math.Ceiling(4.0 * StiScale2.X), rectangle.Y + (int)Math.Ceiling(1.0 * StiScale2.Y), (int)Math.Ceiling(5.0 * StiScale2.X), (int)Math.Ceiling(4.0 * StiScale2.Y));
			graphics.FillRectangle((Brush)new SolidBrush(val6.Color), rectangle.X + (int)Math.Ceiling(4.0 * StiScale2.X), rectangle.Y + (int)Math.Ceiling(4.0 * StiScale2.Y), (int)Math.Ceiling(5.0 * StiScale2.X), (int)Math.Ceiling(1.0 * StiScale2.Y));
			graphics.DrawLine(val6, rectangle.X + (int)Math.Ceiling(4.0 * StiScale2.X), rectangle.Y, rectangle.X + (int)Math.Ceiling(4.0 * StiScale2.X), rectangle.Y + (int)Math.Ceiling(6.0 * StiScale2.Y));
			graphics.DrawLine(val6, rectangle.X, rectangle.Y + (int)Math.Ceiling(3.0 * StiScale2.Y), rectangle.X + (int)Math.Ceiling(3.0 * StiScale2.X), rectangle.Y + (int)Math.Ceiling(3.0 * StiScale2.Y));
		}
		else
		{
			Rectangle rectangle2 = new Rectangle(m_SystemButtons.AutoHideButtonRect.X + (m_SystemButtons.AutoHideButtonRect.Width - (int)Math.Ceiling(7.0 * StiScale2.X)) / 2, m_SystemButtons.AutoHideButtonRect.Y + (m_SystemButtons.AutoHideButtonRect.Height - (int)Math.Ceiling(9.0 * StiScale2.Y)) / 2, (int)Math.Ceiling(7.0 * StiScale2.X), (int)Math.Ceiling(9.0 * StiScale2.Y));
			Pen val7 = null;
			val7 = ((m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted) ? new Pen((m_HasFocus || LayoutType == eLayoutType.Toolbar) ? pa.Colors.BarCaptionText : pa.Colors.BarCaptionInactiveText) : new Pen(pa.Colors.ItemText));
			if (m_SystemButtons.MouseOverAutoHide)
			{
				val7 = new Pen(pa.Colors.ItemHotText, 1f);
			}
			else if (m_SystemButtons.MouseDownAutoHide)
			{
				val7 = new Pen(pa.Colors.ItemPressedText, 1f);
			}
			else if (!m_CaptionForeColor.IsEmpty)
			{
				val7 = new Pen(m_CaptionForeColor, 1f);
			}
			else if (!m_HasFocus && m_BarState != eBarState.Floating && m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted)
			{
				val7 = new Pen(pa.Colors.BarCaptionInactiveText);
			}
			graphics.DrawRectangle(val7, rectangle2.X + (int)Math.Ceiling(1.0 * StiScale2.X), rectangle2.Y, (int)Math.Ceiling(4.0 * StiScale2.X), (int)Math.Ceiling(5.0 * StiScale2.Y));
			graphics.FillRectangle((Brush)new SolidBrush(val7.Color), rectangle2.X + (int)Math.Ceiling(4.0 * StiScale2.X), rectangle2.Y, (int)Math.Ceiling(1.0 * StiScale2.X), (int)Math.Ceiling(5.0 * StiScale2.Y));
			graphics.DrawLine(val7, rectangle2.X, rectangle2.Y + (int)Math.Ceiling(5.0 * StiScale2.Y), rectangle2.X + (int)Math.Ceiling(6.0 * StiScale2.X), rectangle2.Y + (int)Math.Ceiling(5.0 * StiScale2.Y));
			graphics.DrawLine(val7, rectangle2.X + (int)Math.Ceiling(3.0 * StiScale2.X), rectangle2.Y + (int)Math.Ceiling(5.0 * StiScale2.Y), rectangle2.X + (int)Math.Ceiling(3.0 * StiScale2.X), rectangle2.Y + (int)Math.Ceiling(8.0 * StiScale2.Y));
		}
	}

	internal void PaintSideBar(Graphics g)
	{
		if (m_SideBarImage.Picture != null && m_BarState == eBarState.Popup)
		{
			if (!m_SideBarImage.GradientColor1.IsEmpty && !m_SideBarImage.GradientColor2.IsEmpty)
			{
				LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(m_SideBarRect, m_SideBarImage.GradientColor1, m_SideBarImage.GradientColor2, m_SideBarImage.GradientAngle);
				g.FillRectangle((Brush)(object)val, m_SideBarRect);
			}
			else if (!m_SideBarImage.BackColor.Equals((object?)Color.Empty))
			{
				DisplayHelp.FillRectangle(g, m_SideBarRect, m_SideBarImage.BackColor);
			}
			if (m_SideBarImage.StretchPicture)
			{
				g.DrawImage(m_SideBarImage.Picture, m_SideBarRect);
			}
			else if (m_SideBarImage.Alignment == eAlignment.Top)
			{
				g.DrawImage(m_SideBarImage.Picture, m_SideBarRect.X, m_SideBarRect.Top, m_SideBarRect.Width, m_SideBarImage.Picture.Height);
			}
			else if (m_SideBarImage.Alignment == eAlignment.Bottom)
			{
				g.DrawImage(m_SideBarImage.Picture, m_SideBarRect.Left, m_SideBarRect.Bottom - m_SideBarImage.Picture.Height, m_SideBarImage.Picture.Width, m_SideBarImage.Picture.Height);
			}
			else
			{
				g.DrawImage(m_SideBarImage.Picture, m_SideBarRect.Left, m_SideBarRect.Top + (m_SideBarRect.Height - m_SideBarImage.Picture.Height) / 2, m_SideBarImage.Picture.Width, m_SideBarImage.Picture.Height);
			}
		}
	}

	private void SetupRegion()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		if (m_BarState == eBarState.Popup)
		{
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			Region val = new Region(rectangle);
			rectangle.X = ((Control)this).Width - 2;
			rectangle.Y = 0;
			rectangle.Width = 2;
			rectangle.Height = 2;
			val.Xor(rectangle);
			rectangle.X = 0;
			rectangle.Y = ((Control)this).Height - 2;
			rectangle.Width = 2;
			rectangle.Height = 2;
			val.Xor(rectangle);
			((Control)this).Region = val;
		}
	}

	private void SetRoundRegion(Control c)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		Rectangle clientRectangle = c.ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(clientRectangle, m_CornerSize);
		Region val = new Region();
		val.MakeEmpty();
		val.Union(roundedRectanglePath);
		roundedRectanglePath.Widen(SystemPens.ControlText);
		val.Union(roundedRectanglePath);
		c.Region = val;
	}

	public void RecalcLayout()
	{
		if (m_LayoutSuspended || IsDisposing)
		{
			return;
		}
		if (m_BarState == eBarState.Docked)
		{
			if (((Control)this).Parent != null && !PassiveBar)
			{
				if (m_ItemContainer.LayoutType != eLayoutType.DockContainer)
				{
					((Control)this).Invalidate(((Control)this).Region, true);
				}
				if (((Control)this).Parent is DockSite)
				{
					((DockSite)(object)((Control)this).Parent).RecalcLayout();
				}
				else
				{
					RecalcSize();
					((Control)this).Refresh();
				}
				ResizeDockTab();
			}
		}
		else
		{
			RecalcSize();
			ResizeDockTab();
			((Control)this).Refresh();
		}
	}

	internal void OnDockContainerWidthChanged(DockContainerItem item, int width)
	{
		if (!m_LayoutSuspended)
		{
			if (DockSide == eDockSide.Document || (((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null))
			{
				((DockSite)(object)DockedSite).GetDocumentUIManager().SetBarWidth(this, width + 2);
			}
			else if (m_BarState == eBarState.AutoHide)
			{
				m_ItemContainer.MinWidth = width;
				m_LastDockSiteInfo.DockedWidth = width + 2;
			}
		}
	}

	internal void OnDockContainerHeightChanged(DockContainerItem item, int height)
	{
		if (!m_LayoutSuspended)
		{
			if (DockSide == eDockSide.Document || (((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null))
			{
				((DockSite)(object)DockedSite).GetDocumentUIManager().SetBarHeight(this, height + GetNonClientHeight());
			}
			else if (m_BarState == eBarState.AutoHide)
			{
				m_ItemContainer.MinHeight = height;
				m_LastDockSiteInfo.DockedHeight = height + 2;
			}
		}
	}

	internal void OnDockContainerMinimumSizeChanged(DockContainerItem item)
	{
		if (AutoHide)
		{
			m_LastDockSiteInfo.DockedHeight = 0;
			m_LastDockSiteInfo.DockedWidth = 0;
		}
	}

	private void ChangeSplitSize(Size size)
	{
		if (((Control)this).Parent == null || ((ArrangedElementCollection)((Control)this).Parent.Controls).Count <= 1)
		{
			return;
		}
		Bar bar = null;
		Bar bar2 = null;
		Bar previousVisibleBar = GetPreviousVisibleBar(this);
		Bar nextVisibleBar = GetNextVisibleBar(this);
		if (nextVisibleBar != null && nextVisibleBar.LayoutType == eLayoutType.DockContainer && nextVisibleBar.DockLine == DockLine)
		{
			bar = this;
			bar2 = nextVisibleBar;
		}
		else if (previousVisibleBar != null && previousVisibleBar.LayoutType == eLayoutType.DockContainer && previousVisibleBar.DockLine == DockLine)
		{
			bar = previousVisibleBar;
			bar2 = this;
			if (size.Width > 0)
			{
				size.Width = ((Control)bar).Width + (((Control)bar2).Width - size.Width);
			}
			if (size.Height > 0)
			{
				size.Height = ((Control)bar).Height + (((Control)bar2).Height - size.Height);
			}
		}
		if (DockSide == eDockSide.Left || DockSide == eDockSide.Right)
		{
			if (bar != null && bar2 != null && bar.DockLine == bar2.DockLine)
			{
				Size size2 = bar.MinimumDockSize(eOrientation.Horizontal);
				Size size3 = bar2.MinimumDockSize(eOrientation.Horizontal);
				int num = size.Height - ((Control)bar).Height;
				if (size.Height >= size2.Height && ((Control)bar2).Height - num >= size3.Height)
				{
					Size size4 = bar.Size;
					Size size5 = bar2.Size;
					Size size6 = bar.RecalcSizeOnly(new Size(((Control)bar).Width, size.Height));
					Size size7 = bar2.RecalcSizeOnly(new Size(((Control)bar2).Width, ((Control)bar2).Height - num));
					if (!size4.Equals((object?)size6) && !size5.Equals((object?)size7))
					{
						bar2.SplitDockHeight = 0;
						bar.SplitDockHeight = size.Height;
					}
				}
			}
			else
			{
				SplitDockHeight = 0;
				SplitDockWidth = 0;
			}
		}
		else
		{
			if (DockSide != eDockSide.Top && DockSide != eDockSide.Bottom)
			{
				return;
			}
			if (bar != null && bar2 != null && bar.DockLine == bar2.DockLine)
			{
				Size size8 = bar.MinimumDockSize(eOrientation.Horizontal);
				Size size9 = bar2.MinimumDockSize(eOrientation.Horizontal);
				int num2 = size.Width - ((Control)bar).Width;
				if (size.Width >= size8.Width && ((Control)bar2).Width - num2 >= size9.Width)
				{
					Size size10 = bar.Size;
					Size size11 = bar2.Size;
					Size size12 = bar.RecalcSizeOnly(new Size(size.Width, ((Control)bar).Height));
					Size size13 = bar2.RecalcSizeOnly(new Size(((Control)bar2).Width - num2, ((Control)bar2).Height));
					if (!size10.Equals((object?)size12) && !size11.Equals((object?)size13))
					{
						bar2.SplitDockWidth = 0;
						bar.SplitDockWidth = size.Width;
					}
				}
			}
			else
			{
				SplitDockHeight = 0;
				SplitDockWidth = 0;
			}
		}
	}

	internal int GetNonClientHeight()
	{
		return GetGrabHandleCaptionHeight() + 2;
	}

	public void RecalcSize()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		m_GrabHandleRect = Rectangle.Empty;
		if (m_RecalculatingSize || m_ItemContainer == null)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 1)
		{
			return;
		}
		m_RecalculatingSize = true;
		if (LayoutType == eLayoutType.DockContainer && m_TabDockItems == null)
		{
			RefreshDockTab(bFireEvents: false);
		}
		try
		{
			if (m_BarState == eBarState.Docked)
			{
				m_ItemContainer.WrapItems = m_WrapItemsDock;
				m_ItemContainer.Stretch = m_DockStretch;
			}
			else if (m_BarState == eBarState.Floating || m_BarState == eBarState.AutoHide)
			{
				m_ItemContainer.WrapItems = m_WrapItemsFloat;
				if (m_ItemContainer.LayoutType == eLayoutType.DockContainer)
				{
					m_ItemContainer.Stretch = m_DockStretch;
				}
				else
				{
					m_ItemContainer.Stretch = false;
				}
			}
			else
			{
				m_ItemContainer.WrapItems = true;
				m_ItemContainer.Stretch = false;
			}
			if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				if (m_BarState == eBarState.Docked && !(((Control)this).Parent is DockSite))
				{
					Size size = RecalcSizeOffice();
					if (m_ItemContainer.Orientation == eOrientation.Horizontal)
					{
						((Control)this).Height = size.Height;
					}
					else
					{
						((Control)this).Width = size.Width;
					}
				}
				else
				{
					((Control)this).ClientSize = RecalcSizeOffice();
				}
			}
			else if (m_BarState == eBarState.Docked && !(((Control)this).Parent is DockSite))
			{
				Size size2 = RecalcSizeDotNet();
				if (m_ItemContainer.Orientation == eOrientation.Horizontal)
				{
					((Control)this).Height = size2.Height;
				}
				else
				{
					((Control)this).Width = size2.Width;
				}
			}
			else
			{
				((Control)this).ClientSize = RecalcSizeDotNet();
			}
			if (m_DropShadow != null)
			{
				NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
			}
			if (m_BarState == eBarState.Docked)
			{
				if (m_ItemContainer.Orientation == eOrientation.Horizontal)
				{
					m_DockedSizeH = Size;
				}
				else
				{
					m_DockedSizeV = Size;
				}
				RefreshOffice2003Region();
			}
			else if (((Control)this).Region != null)
			{
				((Control)this).Region = null;
			}
			if (m_BarState == eBarState.Floating && !PassiveBar)
			{
				m_Float.RefreshRegion(((Control)this).ClientSize);
				((Form)m_Float).ClientSize = ((Control)this).ClientSize;
			}
		}
		finally
		{
			m_RecalculatingSize = false;
		}
		if (LayoutType == eLayoutType.DockContainer && m_TabDockItems != null && ((Component)this).DesignMode && ((Component)(object)this).Site != null)
		{
			RefreshDockTab(bFireEvents: false);
		}
		OnItemLayoutUpdated(EventArgs.Empty);
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		this.ItemLayoutUpdated?.Invoke(this, e);
	}

	private void RefreshOffice2003Region()
	{
		((Control)this).Region = null;
	}

	internal void RecalcSize(Size frameSize)
	{
		m_GrabHandleRect = Rectangle.Empty;
		if (m_RecalculatingSize || m_ItemContainer == null)
		{
			return;
		}
		m_RecalculatingSize = true;
		try
		{
			if (m_BarState == eBarState.Docked)
			{
				m_ItemContainer.WrapItems = m_WrapItemsDock;
				m_ItemContainer.Stretch = m_DockStretch;
			}
			else if (m_BarState == eBarState.Floating || m_BarState == eBarState.AutoHide)
			{
				m_ItemContainer.WrapItems = m_WrapItemsFloat;
				if (m_ItemContainer.LayoutType == eLayoutType.DockContainer)
				{
					m_ItemContainer.Stretch = m_DockStretch;
				}
				else
				{
					m_ItemContainer.Stretch = false;
				}
			}
			else
			{
				m_ItemContainer.WrapItems = true;
				m_ItemContainer.Stretch = false;
			}
			if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				((Control)this).ClientSize = RecalcSizeOffice(bCalculateOnly: false, frameSize, m_ItemContainer.Orientation, m_BarState, m_ItemContainer.WrapItems);
			}
			else
			{
				((Control)this).ClientSize = RecalcSizeDotNet(bCalculateOnly: false, frameSize, m_ItemContainer.Orientation, m_BarState, m_ItemContainer.WrapItems);
			}
			if (m_DropShadow != null)
			{
				NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
			}
			if (m_BarState == eBarState.Docked)
			{
				if (m_ItemContainer.Orientation == eOrientation.Horizontal)
				{
					m_DockedSizeH = Size;
				}
				else
				{
					m_DockedSizeV = Size;
				}
				RefreshOffice2003Region();
			}
			if (m_BarState == eBarState.Floating && !PassiveBar)
			{
				m_Float.RefreshRegion(((Control)this).ClientSize);
				((Form)m_Float).ClientSize = ((Control)this).ClientSize;
			}
		}
		finally
		{
			m_RecalculatingSize = false;
		}
	}

	private Size RecalcSizeOnly(Size objFrameSize)
	{
		return RecalcSizeOnly(objFrameSize, m_ItemContainer.Orientation, m_BarState, m_ItemContainer.WrapItems);
	}

	private Size RecalcSizeOnly(Size objFrameSize, eOrientation barOrientation, eBarState barState, bool bWrapItems)
	{
		if (m_ItemContainer.SubItems.Count > 0)
		{
			if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return RecalcSizeOffice(bCalculateOnly: true, objFrameSize, barOrientation, barState, bWrapItems);
			}
			return RecalcSizeDotNet(bCalculateOnly: true, objFrameSize, barOrientation, barState, bWrapItems);
		}
		return Size.Empty;
	}

	private Size RecalcSizeDotNet()
	{
		return RecalcSizeDotNet(bCalculateOnly: false, Size, m_ItemContainer.Orientation, m_BarState, m_ItemContainer.WrapItems);
	}

	private Size RecalcSizeDotNet(bool bCalculateOnly, Size objFrameSize, eOrientation barOrientation, eBarState barState, bool bWrapItems)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Invalid comparison between Unknown and I4
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Invalid comparison between Unknown and I4
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Invalid comparison between Unknown and I4
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Invalid comparison between Unknown and I4
		Size empty = Size.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int widthInternal = 0;
		int heightInternal = 0;
		eOrientation orientation = m_ItemContainer.Orientation;
		eBarState eBarState2 = m_BarState;
		bool stretch = m_ItemContainer.Stretch;
		bool wrapItems = m_ItemContainer.WrapItems;
		m_ItemContainer.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
		if (bCalculateOnly)
		{
			m_ItemContainer.Orientation = barOrientation;
			eBarState2 = barState;
			m_ItemContainer.WrapItems = bWrapItems;
			if (barState == eBarState.Docked)
			{
				m_ItemContainer.Stretch = m_DockStretch;
			}
			else if (m_ItemContainer.LayoutType == eLayoutType.DockContainer)
			{
				m_ItemContainer.Stretch = m_DockStretch;
			}
			else
			{
				m_ItemContainer.Stretch = false;
			}
		}
		switch (eBarState2)
		{
		case eBarState.Docked:
			if (m_DockedBorder != 0)
			{
				num = 3;
				num2 = 3;
				num3 = 3;
				num4 = 3;
			}
			break;
		case eBarState.Floating:
			if (IsThemed || DrawThemedCaption)
			{
				if (m_ThemeWindowMargins.IsEmpty)
				{
					RefreshThemeMargins();
				}
				num = m_ThemeWindowMargins.Top;
				num3 = m_ThemeWindowMargins.Left;
				num4 = m_ThemeWindowMargins.Right;
				num2 = m_ThemeWindowMargins.Bottom;
				if (GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane)
				{
					num = GetGrabHandleTaskPaneHeight() + m_ThemeWindowMargins.Bottom;
				}
			}
			else
			{
				num = ((GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane) ? GetNonClientHeight() : (GetGrabHandleTaskPaneHeight() + 4));
				num3 = 2;
				num4 = 2;
				num2 = 3;
			}
			break;
		default:
			num = ClientMarginTop;
			num3 = ClientMarginLeft;
			num4 = ClientMarginRight;
			num2 = ClientMarginBottom;
			break;
		}
		int num6 = num;
		int num7 = num3;
		if (BarType == eBarType.StatusBar && BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle))
		{
			num6 += 2;
		}
		if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && !m_AutoHideState && GetDockTabVisible() && (m_ItemContainer.VisibleSubItems > 1 || (m_AlwaysDisplayDockTab && m_ItemContainer.VisibleSubItems == 1 && m_BarState != eBarState.Floating)))
		{
			switch (m_DockTabAlignment)
			{
			case eTabStripAlignment.Top:
				num6 += DockTabStripHeight;
				break;
			case eTabStripAlignment.Left:
				num7 += DockTabStripHeight;
				break;
			case eTabStripAlignment.Right:
				num5 = DockTabStripHeight;
				break;
			default:
				num2 += DockTabStripHeight;
				break;
			}
		}
		if (eBarState2 == eBarState.Popup && m_SideBarImage.Picture != null)
		{
			num7 += m_SideBarImage.Picture.Width;
		}
		else if ((eBarState2 == eBarState.Docked || BarState == eBarState.AutoHide) && m_GrabHandleStyle != 0)
		{
			if (m_GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
			{
				if ((int)((Control)this).RightToLeft == 1)
				{
					num7 += GrabHandleResizeWidth;
				}
				else
				{
					num4 += GrabHandleResizeWidth;
				}
			}
			else if (m_ItemContainer.Orientation != 0 || m_ItemContainer.LayoutType != 0)
			{
				num6 = ((m_GrabHandleStyle == eGrabHandleStyle.Caption) ? (num6 + GetGrabHandleCaptionHeight()) : ((m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted) ? (num6 + GrabHandleDotNetWidth) : (num6 + GetGrabHandleTaskPaneHeight())));
			}
			else if (m_GrabHandleStyle == eGrabHandleStyle.Caption)
			{
				num7 += GetGrabHandleCaptionHeight();
			}
			else if ((int)((Control)this).RightToLeft == 1)
			{
				num4 += GrabHandleDotNetWidth;
			}
			else
			{
				num7 += GrabHandleDotNetWidth;
			}
		}
		if (!bCalculateOnly)
		{
			m_ItemContainer.TopInternal = num6;
			m_ItemContainer.LeftInternal = num7;
		}
		else
		{
			heightInternal = m_ItemContainer.HeightInternal;
			widthInternal = m_ItemContainer.WidthInternal;
		}
		m_ItemContainer.SuspendLayout = true;
		if (!bCalculateOnly)
		{
			if (eBarState2 == eBarState.Popup)
			{
				m_ItemContainer.WidthInternal = m_InitialContainerWidth;
			}
			else
			{
				m_ItemContainer.WidthInternal = objFrameSize.Width - num7 - num4 - num5;
				m_ItemContainer.HeightInternal = objFrameSize.Height - num6 - num2;
			}
		}
		else
		{
			m_ItemContainer.WidthInternal = objFrameSize.Width - num7 - num4 - num5;
			m_ItemContainer.HeightInternal = objFrameSize.Height - num6 - num2;
		}
		m_ItemContainer.SuspendLayout = false;
		if (m_ItemContainer.VisibleSubItems == 0 || (m_ItemContainer.SubItems.Count == 1 && m_ItemContainer.SubItems[0] is CustomizeItem && !m_ItemContainer.SubItems[0].Visible))
		{
			m_ItemContainer.RecalcSize();
			if (LayoutType != eLayoutType.DockContainer && DockSide != eDockSide.Document)
			{
				m_ItemContainer.WidthInternal = 36;
				m_ItemContainer.HeightInternal = 24;
			}
		}
		else
		{
			m_ItemContainer.RecalcSize();
		}
		num6 += m_ItemContainer.HeightInternal;
		if (IsGradientStyle && LayoutType == eLayoutType.Toolbar && !MenuBar && !IsThemed && BarType != eBarType.StatusBar)
		{
			num6++;
		}
		if (!bCalculateOnly)
		{
			if ((int)((Control)this).RightToLeft == 1)
			{
				if (LayoutType == eLayoutType.Toolbar && m_ItemContainer.WidthInternal < objFrameSize.Width - num7 - num4 - num5 && Stretch)
				{
					m_ItemContainer.LeftInternal = objFrameSize.Width - num4 - m_ItemContainer.WidthInternal;
					m_ItemContainer.RecalcSize();
				}
				m_ClientRect = new Rectangle(m_ItemContainer.LeftInternal, num, m_ItemContainer.WidthInternal, num6);
			}
			else
			{
				m_ClientRect = new Rectangle(num7, num, m_ItemContainer.WidthInternal, num6);
			}
		}
		if (!bCalculateOnly)
		{
			if ((eBarState2 == eBarState.Docked || eBarState2 == eBarState.AutoHide) && m_GrabHandleStyle != 0 && m_GrabHandleStyle != eGrabHandleStyle.ResizeHandle)
			{
				if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType == eLayoutType.Toolbar)
				{
					if (m_GrabHandleStyle == eGrabHandleStyle.Caption)
					{
						m_GrabHandleRect = new Rectangle(num3, num, GetGrabHandleCaptionHeight(), num6);
					}
					else if ((int)((Control)this).RightToLeft == 1)
					{
						m_GrabHandleRect = new Rectangle(m_ItemContainer.WidthInternal + num7 + num3, num, 10, num6);
					}
					else
					{
						m_GrabHandleRect = new Rectangle(num3, num, 10, num6);
					}
				}
				else if (m_GrabHandleStyle == eGrabHandleStyle.Caption)
				{
					m_GrabHandleRect = new Rectangle(num3, num, m_ItemContainer.WidthInternal + num7 - num3 + num5, GetGrabHandleCaptionHeight());
				}
				else if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
				{
					m_GrabHandleRect = new Rectangle(num3, num, m_ItemContainer.WidthInternal + num7 - num3 + num5, GetGrabHandleTaskPaneHeight());
				}
				else
				{
					m_GrabHandleRect = new Rectangle(num3, num, m_ItemContainer.WidthInternal + num7 - num3 + num5, 10);
				}
			}
			else if (eBarState2 == eBarState.Floating)
			{
				if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane)
				{
					m_GrabHandleRect = new Rectangle(num3, (IsThemed || DrawThemedCaption) ? m_ThemeWindowMargins.Bottom : 2, m_ItemContainer.WidthInternal + num7 - num3 + num5, GetGrabHandleTaskPaneHeight());
				}
				else
				{
					m_GrabHandleRect = new Rectangle(2, 2, m_ItemContainer.WidthInternal + num5, GetGrabHandleCaptionHeight());
					if (CanHideResolved)
					{
						m_GrabHandleRect.Width -= 14;
					}
				}
			}
			if (eBarState2 == eBarState.Popup && m_SideBarImage.Picture != null)
			{
				m_SideBarRect = new Rectangle(num7 - m_SideBarImage.Picture.Width, num, m_SideBarImage.Picture.Width, num6 - num);
			}
			empty = new Size(num7 + m_ItemContainer.WidthInternal + num4 + num5, num6 + num2);
		}
		else
		{
			empty = new Size(num7 + m_ItemContainer.WidthInternal + num4 + num5, num6 + num2);
			m_ItemContainer.WidthInternal = widthInternal;
			m_ItemContainer.HeightInternal = heightInternal;
			m_ItemContainer.Orientation = orientation;
			m_ItemContainer.WrapItems = wrapItems;
			m_ItemContainer.Stretch = stretch;
			m_ItemContainer.RecalcSize();
		}
		return empty;
	}

	private Size RecalcSizeOffice()
	{
		return RecalcSizeOffice(bCalculateOnly: false, Size, m_ItemContainer.Orientation, m_BarState, m_ItemContainer.WrapItems);
	}

	private Size RecalcSizeOffice(bool bCalculateOnly, Size objFrameSize, eOrientation barOrientation, eBarState barState, bool bWrapItems)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		Size empty = Size.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int widthInternal = 0;
		int heightInternal = 0;
		eOrientation orientation = m_ItemContainer.Orientation;
		eBarState eBarState2 = m_BarState;
		bool stretch = m_ItemContainer.Stretch;
		bool wrapItems = m_ItemContainer.WrapItems;
		m_ItemContainer.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
		if (bCalculateOnly)
		{
			m_ItemContainer.Orientation = barOrientation;
			eBarState2 = barState;
			m_ItemContainer.WrapItems = bWrapItems;
			if (barState == eBarState.Docked)
			{
				m_ItemContainer.Stretch = m_DockStretch;
			}
			else if (m_ItemContainer.LayoutType == eLayoutType.DockContainer)
			{
				m_ItemContainer.Stretch = m_DockStretch;
			}
			else
			{
				m_ItemContainer.Stretch = false;
			}
		}
		switch (eBarState2)
		{
		case eBarState.Docked:
			if (m_DockedBorder != 0)
			{
				num = 2;
				num2 = 2;
				num3 = 2;
				num4 = 2;
			}
			break;
		case eBarState.Floating:
			num = 22;
			num3 = 4;
			num4 = 4;
			num2 = 4;
			break;
		default:
			num = ClientMarginTop;
			num3 = ClientMarginLeft;
			num4 = ClientMarginRight;
			num2 = ClientMarginBottom;
			break;
		}
		int num6 = num;
		int num7 = num3;
		if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && !m_AutoHideState && GetDockTabVisible() && (m_ItemContainer.VisibleSubItems > 1 || (m_AlwaysDisplayDockTab && m_ItemContainer.VisibleSubItems == 1 && m_BarState != eBarState.Floating)))
		{
			switch (m_DockTabAlignment)
			{
			case eTabStripAlignment.Top:
				num6 += DockTabStripHeight;
				break;
			case eTabStripAlignment.Left:
				num7 += DockTabStripHeight;
				break;
			case eTabStripAlignment.Right:
				num5 = DockTabStripHeight;
				break;
			default:
				num2 += DockTabStripHeight;
				break;
			}
		}
		if (eBarState2 == eBarState.Popup && m_SideBarImage.Picture != null)
		{
			num7 += m_SideBarImage.Picture.Width;
		}
		else if ((eBarState2 == eBarState.Docked || BarState == eBarState.AutoHide) && m_GrabHandleStyle != 0)
		{
			if (m_ItemContainer.Orientation != 0 || m_ItemContainer.LayoutType == eLayoutType.DockContainer)
			{
				num6 = ((m_GrabHandleStyle == eGrabHandleStyle.Caption) ? (num6 + GetGrabHandleCaptionHeight()) : ((m_GrabHandleStyle != eGrabHandleStyle.CaptionTaskPane && m_GrabHandleStyle != eGrabHandleStyle.CaptionDotted) ? (num6 + GrabHandleOfficeWidth) : (num6 + GetGrabHandleTaskPaneHeight())));
			}
			else
			{
				num7 += GrabHandleOfficeWidth;
			}
		}
		if (!bCalculateOnly)
		{
			m_ItemContainer.TopInternal = num6;
			m_ItemContainer.LeftInternal = num7;
		}
		else
		{
			heightInternal = m_ItemContainer.HeightInternal;
			widthInternal = m_ItemContainer.WidthInternal;
		}
		if (!bCalculateOnly)
		{
			if (eBarState2 == eBarState.Popup)
			{
				m_ItemContainer.WidthInternal = m_InitialContainerWidth;
			}
			else
			{
				m_ItemContainer.WidthInternal = objFrameSize.Width - num7 - num4 - num5;
				m_ItemContainer.HeightInternal = objFrameSize.Height - num6 - num2;
			}
		}
		else
		{
			m_ItemContainer.WidthInternal = objFrameSize.Width - num7 - num4 - num5;
			m_ItemContainer.HeightInternal = objFrameSize.Height - num6 - num2;
		}
		if (m_ItemContainer.VisibleSubItems == 0)
		{
			m_ItemContainer.WidthInternal = 36;
			m_ItemContainer.HeightInternal = 24;
		}
		else
		{
			m_ItemContainer.RecalcSize();
		}
		num6 += m_ItemContainer.HeightInternal;
		if (!bCalculateOnly)
		{
			m_ClientRect = new Rectangle(num7, num, m_ItemContainer.WidthInternal, num6);
		}
		if (!bCalculateOnly)
		{
			if ((eBarState2 == eBarState.Docked || BarState == eBarState.AutoHide) && m_GrabHandleStyle != 0)
			{
				if (m_ItemContainer.Orientation == eOrientation.Horizontal && m_ItemContainer.LayoutType != eLayoutType.DockContainer)
				{
					m_GrabHandleRect = new Rectangle(num3, num, 12, num6);
				}
				else if (m_GrabHandleStyle == eGrabHandleStyle.Caption)
				{
					m_GrabHandleRect = new Rectangle(num3, num, m_ItemContainer.WidthInternal + num7 - num3 + num5, GetGrabHandleCaptionHeight());
				}
				else if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)
				{
					m_GrabHandleRect = new Rectangle(num3, num, m_ItemContainer.WidthInternal + num7 - num3 + num5, GetGrabHandleTaskPaneHeight());
				}
				else
				{
					m_GrabHandleRect = new Rectangle(num3, num, m_ItemContainer.WidthInternal + num7 - num3 + num5, 12);
				}
			}
			else if (eBarState2 == eBarState.Floating)
			{
				if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane)
				{
					m_GrabHandleRect = new Rectangle(4, 4, m_ItemContainer.WidthInternal + num5, GetGrabHandleTaskPaneHeight());
				}
				else
				{
					m_GrabHandleRect = new Rectangle(4, 4, m_ItemContainer.WidthInternal + num5, 15);
				}
				if (CanHideResolved)
				{
					m_GrabHandleRect.Width -= 14;
				}
			}
			if (eBarState2 == eBarState.Popup && m_SideBarImage.Picture != null)
			{
				m_SideBarRect = new Rectangle(num7 - m_SideBarImage.Picture.Width, num, m_SideBarImage.Picture.Width, num6 - num);
			}
			empty = new Size(num7 + m_ItemContainer.WidthInternal + num4 + num5, num6 + num2);
		}
		else
		{
			empty = new Size(num7 + m_ItemContainer.WidthInternal + num4 + num5, num6 + num2);
			m_ItemContainer.WidthInternal = widthInternal;
			m_ItemContainer.HeightInternal = heightInternal;
			m_ItemContainer.Orientation = orientation;
			m_ItemContainer.WrapItems = wrapItems;
			m_ItemContainer.Stretch = stretch;
			m_ItemContainer.RecalcSize();
		}
		return empty;
	}

	private void RestoreContainer()
	{
		if (m_ParentItem == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_ParentItem.SubItems)
		{
			subItem.Expanded = false;
			subItem.SetParent(m_ParentItem);
			subItem.ContainerControl = m_OldContainer;
			subItem.Displayed = false;
			subItem.Orientation = m_ParentItem.Orientation;
		}
		m_ItemContainer.SubItems._Clear();
	}

	internal void OnAddedToBars()
	{
		if (m_Owner != null && m_Owner is IOwner && ((IOwner)m_Owner).Images != null)
		{
			m_ItemContainer.RefreshImageSize();
		}
	}

	internal void ProcessDelayedCommands()
	{
		if (m_DockSideDelayed < 0)
		{
			return;
		}
		if (m_DockSideDelayed != 5 || DockSide != eDockSide.Document)
		{
			DockSide = (eDockSide)m_DockSideDelayed;
		}
		m_DockSideDelayed = -1;
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			subItem.OnProcessDelayedCommands();
		}
	}

	private bool IsAnyControl(Control ctrlParent, Control ctrlReference)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (ctrlParent == ctrlReference)
		{
			return true;
		}
		foreach (Control item in (ArrangedElementCollection)ctrlParent.Controls)
		{
			Control val = item;
			if (val == ctrlReference)
			{
				return true;
			}
			if (IsAnyControl(val, ctrlReference))
			{
				return true;
			}
		}
		return false;
	}

	private int GetGrabHandleTaskPaneHeight()
	{
		if (((Control)this).Font != null)
		{
			return Math.Max(GrabHandleTaskPaneHeight, ((Control)this).Font.Height + 2);
		}
		return GrabHandleTaskPaneHeight;
	}

	private int GetGrabHandleCaptionHeight()
	{
		if (_CaptionHeight > 0)
		{
			return _CaptionHeight;
		}
		if (((Control)this).Font != null)
		{
			return Math.Max(GrabHandleCaptionHeight, ((Control)this).Font.Height + 2);
		}
		return GrabHandleCaptionHeight;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DockDocumentManager(DockSiteInfo pDockInfo)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		DocumentDockUIManager documentUIManager = pDockInfo.objDockSite.GetDocumentUIManager();
		if ((int)((Control)pDockInfo.objDockSite).Dock == 5)
		{
			documentUIManager.Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
		}
		else if ((pDockInfo.DockLine == -1 || pDockInfo.DockLine == 999) && ((int)((Control)pDockInfo.objDockSite).Dock == 3 || (int)((Control)pDockInfo.objDockSite).Dock == 4))
		{
			documentUIManager.Dock(null, this, (pDockInfo.DockLine == -1) ? eDockSide.Left : eDockSide.None);
		}
		else if ((pDockInfo.DockLine == -1 || pDockInfo.DockLine == 999) && ((int)((Control)pDockInfo.objDockSite).Dock == 1 || (int)((Control)pDockInfo.objDockSite).Dock == 2))
		{
			documentUIManager.Dock(null, this, (pDockInfo.DockLine == -1) ? eDockSide.Top : eDockSide.None);
		}
		else
		{
			documentUIManager.Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
		}
	}

	internal void RemoveFromFloatingContainer()
	{
		if (m_Float != null)
		{
			m_FloatingRect = new Rectangle(((Form)m_Float).Location, Size);
			((Control)m_Float).Controls.Remove((Control)(object)this);
			((Control)m_Float).Hide();
			((Component)(object)m_Float).Dispose();
			m_Float = null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DockingHandler(DockSiteInfo pDockInfo, Point p)
	{
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected I4, but got Unknown
		IOwner owner = m_Owner as IOwner;
		if (m_TempTabBar != null && pDockInfo.TabDockContainer == m_TempTabBar)
		{
			return;
		}
		if (pDockInfo.objDockSite == null && (m_BarState != eBarState.Floating || m_Float == null))
		{
			m_DockingInProgress = true;
			if (m_TempTabBar != null)
			{
				RemoveTempTabBarItems();
				m_TempTabBar.RecalcLayout();
				m_TempTabBar = null;
			}
			DockSiteInfo lastDockSiteInfo = m_LastDockSiteInfo;
			m_LastDockSiteInfo = new DockSiteInfo();
			m_LastDockSiteInfo.LastRelativeDocumentId = lastDockSiteInfo.LastRelativeDocumentId;
			m_LastDockSiteInfo.LastRelativeDockToBar = lastDockSiteInfo.LastRelativeDockToBar;
			m_LastDockSiteInfo.DockedHeight = ((Control)this).Height;
			m_LastDockSiteInfo.DockedWidth = ((Control)this).Width;
			m_LastDockSiteInfo.DockLine = DockLine;
			m_LastDockSiteInfo.DockOffset = DockOffset;
			if (((Control)this).Parent != null)
			{
				m_LastDockSiteInfo.DockSide = ((Control)this).Parent.Dock;
			}
			else
			{
				m_LastDockSiteInfo.DockSide = (DockStyle)3;
			}
			if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
			{
				m_LastDockSiteInfo.InsertPosition = ((Control)(DockSite)(object)((Control)this).Parent).Controls.GetChildIndex((Control)(object)this);
				m_LastDockSiteInfo.objDockSite = (DockSite)(object)DockedSite;
			}
			if (m_LastDockSiteInfo.objDockSite == null && m_Owner is IOwnerBarSupport ownerBarSupport)
			{
				DockStyle dockSide = m_LastDockSiteInfo.DockSide;
				switch (dockSide - 1)
				{
				case 2:
					m_LastDockSiteInfo.objDockSite = ownerBarSupport.LeftDockSite;
					break;
				case 3:
					m_LastDockSiteInfo.objDockSite = ownerBarSupport.RightDockSite;
					break;
				case 0:
					m_LastDockSiteInfo.objDockSite = ownerBarSupport.TopDockSite;
					break;
				case 1:
					m_LastDockSiteInfo.objDockSite = ownerBarSupport.BottomDockSite;
					break;
				case 4:
					m_LastDockSiteInfo.objDockSite = ownerBarSupport.FillDockSite;
					break;
				}
			}
			m_BarState = eBarState.Floating;
			if (m_Float == null)
			{
				m_Float = new FloatingContainer(this);
				((Control)m_Float).CreateControl();
			}
			if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
			else if (owner.ParentForm != null && IsAnyControl((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
			if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
			{
				if (((DockSite)(object)((Control)this).Parent).IsDocumentDock || ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
				{
					((DockSite)(object)((Control)this).Parent).GetDocumentUIManager().UnDock(this);
				}
				else
				{
					((DockSite)(object)((Control)this).Parent).RemoveBar((Control)(object)this);
				}
			}
			((Control)this).Parent = null;
			((Control)m_Float).Controls.Add((Control)(object)this);
			if (!Visible)
			{
				((Control)this).Visible = true;
			}
			((Control)this).Location = new Point(0, 0);
			m_FloatingRect = new Rectangle(m_FloatingRect.Location, GetFloatingSize());
			Size = m_FloatingRect.Size;
			DockOrientation = eOrientation.Horizontal;
			if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && m_AlwaysDisplayDockTab)
			{
				RefreshDockTab(bFireEvents: true);
			}
			RecalcSize();
			m_FloatingRect.Size = Size;
			((Form)m_Float).Location = p;
			if (m_ItemContainer.LayoutType != 0)
			{
				m_MouseDownPt = new Point(((Control)this).Width / 2, 8);
			}
			else
			{
				m_MouseDownPt = new Point(8, 8);
			}
			if (m_LoadingHideFloating)
			{
				((Control)m_Float).Visible = false;
			}
			else
			{
				((Control)m_Float).Show();
			}
			m_FloatingRect.Location = ((Form)m_Float).Location;
			if (((Control)m_Float).Width != m_FloatingRect.Width)
			{
				((Control)m_Float).Width = m_FloatingRect.Width;
			}
			if (owner.ParentForm != null)
			{
				bool flag = true;
				if (m_BarDefinitionLoading && owner is DotNetBarManager && !((DotNetBarManager)owner).ActivateOnLayoutLoad)
				{
					flag = false;
				}
				if (flag)
				{
					owner.ParentForm.Activate();
				}
			}
			m_DockingInProgress = false;
			if (this.BarUndock != null)
			{
				this.BarUndock(this, new EventArgs());
			}
			if (m_Owner is IOwnerBarSupport ownerBarSupport2)
			{
				ownerBarSupport2.InvokeBarUndock(this, new EventArgs());
			}
			OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.BarUndocked));
			ResizeDockTab();
			return;
		}
		if ((pDockInfo.FullSizeDock || pDockInfo.PartialSizeDock) && pDockInfo.DockSiteZOrderIndex >= 0 && pDockInfo.objDockSite != null && ((Control)pDockInfo.objDockSite).Parent != null)
		{
			((Control)pDockInfo.objDockSite).Parent.Controls.SetChildIndex((Control)(object)pDockInfo.objDockSite, pDockInfo.DockSiteZOrderIndex);
		}
		if (m_Owner != null)
		{
			if (pDockInfo.TabDockContainer != null)
			{
				if (m_TempTabBar != null && pDockInfo.TabDockContainer != m_TempTabBar)
				{
					RemoveTempTabBarItems();
					m_TempTabBar.RecalcLayout();
					m_TempTabBar = null;
				}
				if (m_TempTabBar != pDockInfo.TabDockContainer)
				{
					m_DockingInProgress = true;
					if (m_DockTabTearOffIndex == -1)
					{
						if (m_Float != null && ((Control)m_Float).Visible)
						{
							if (owner.ParentForm != null)
							{
								owner.ParentForm.Activate();
							}
							m_FloatingRect = new Rectangle(((Form)m_Float).Location, Size);
							m_DockOffset = pDockInfo.DockOffset;
							m_DockLine = pDockInfo.DockLine;
							((Control)m_Float).Controls.Remove((Control)(object)this);
							((Control)m_Float).Hide();
							((Component)(object)m_Float).Dispose();
							m_Float = null;
						}
						else if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
						{
							((DockSite)(object)((Control)this).Parent).RemoveBar((Control)(object)this);
						}
						m_BarState = eBarState.Docked;
						if (!m_BarDefinitionLoading)
						{
							((Control)this).Visible = false;
						}
						foreach (BaseItem subItem in m_ItemContainer.SubItems)
						{
							if (subItem is DockContainerItem dockContainerItem)
							{
								DockContainerItem dockContainerItem2 = new DockContainerItem();
								dockContainerItem2.Displayed = false;
								dockContainerItem2.Text = subItem.Text;
								dockContainerItem2.Image = dockContainerItem.Image;
								dockContainerItem2.ImageIndex = dockContainerItem.ImageIndex;
								dockContainerItem2.Icon = dockContainerItem.Icon;
								dockContainerItem2.Tag = "systempdockitem";
								pDockInfo.TabDockContainer.Items.Add(dockContainerItem2);
							}
							m_TempTabBar = pDockInfo.TabDockContainer;
							m_TempTabBar.RecalcLayout();
							((Control)m_TempTabBar).Refresh();
						}
					}
					else
					{
						if (m_ItemContainer.SubItems[m_DockTabTearOffIndex] is DockContainerItem dockContainerItem3)
						{
							DockContainerItem dockContainerItem4 = new DockContainerItem();
							dockContainerItem4.Displayed = false;
							dockContainerItem4.Text = dockContainerItem3.Text;
							dockContainerItem4.Image = dockContainerItem3.Image;
							dockContainerItem4.ImageIndex = dockContainerItem3.ImageIndex;
							dockContainerItem4.Icon = dockContainerItem3.Icon;
							dockContainerItem4.Tag = "systempdockitem";
							pDockInfo.TabDockContainer.Items.Add(dockContainerItem4);
						}
						m_TempTabBar = pDockInfo.TabDockContainer;
						m_TempTabBar.RecalcLayout();
						((Control)m_TempTabBar).Refresh();
					}
					m_DockingInProgress = false;
				}
			}
			else
			{
				if (m_TempTabBar != null)
				{
					if (pDockInfo.objDockSite == ((Control)m_TempTabBar).Parent)
					{
						return;
					}
					RemoveTempTabBarItems();
					m_TempTabBar.RecalcLayout();
					m_TempTabBar = null;
				}
				if (!Visible && !m_BarDefinitionLoading)
				{
					Visible = true;
				}
				if (pDockInfo.LastRelativeDockToBar != null && pDockInfo.LastRelativeDockToBar.DockedSite != null && owner is DotNetBarManager)
				{
					DotNetBarManager dotNetBarManager = (DotNetBarManager)owner;
					if (pDockInfo.LastRelativeDockToBar.AutoHide)
					{
						if (pDockInfo.objDockSite != null)
						{
							pDockInfo.objDockSite.GetDocumentUIManager().Dock(this);
						}
						else
						{
							dotNetBarManager.Dock(this, eDockSide.Right);
						}
					}
					else if (!pDockInfo.LastRelativeDockToBar.Visible || pDockInfo.LastRelativeDockToBar.DockSide == eDockSide.None)
					{
						dotNetBarManager.Dock(this, m_LastDockSiteInfo.LastDockSiteSide);
					}
					else if (pDockInfo.LastRelativeDockToBar.DockSide != 0 && m_LastDockSiteInfo.LastDockSiteSide != pDockInfo.LastRelativeDockToBar.DockSide)
					{
						dotNetBarManager.Dock(this, m_LastDockSiteInfo.LastDockSiteSide);
					}
					else
					{
						dotNetBarManager.Dock(this, pDockInfo.LastRelativeDockToBar, LastDockSide);
					}
					if (((Control)this).IsDisposed)
					{
						return;
					}
					m_BarState = eBarState.Docked;
					pDockInfo.LastRelativeDockToBar = null;
				}
				else if (pDockInfo.objDockSite != null && m_BarState != eBarState.Docked)
				{
					m_DockingInProgress = true;
					if (m_Float != null && m_BarState == eBarState.Floating)
					{
						if (owner.ParentForm != null)
						{
							owner.ParentForm.Activate();
						}
						m_FloatingRect = new Rectangle(((Form)m_Float).Location, Size);
						m_DockOffset = pDockInfo.DockOffset;
						m_DockLine = pDockInfo.DockLine;
						((Control)m_Float).Controls.Remove((Control)(object)this);
						((Control)m_Float).Hide();
						((Component)(object)m_Float).Dispose();
						m_Float = null;
					}
					m_BarState = eBarState.Docked;
					if (pDockInfo.objDockSite.IsDocumentDock || pDockInfo.objDockSite.DocumentDockContainer != null)
					{
						DockDocumentManager(pDockInfo);
					}
					else if (pDockInfo.InsertPosition == -10)
					{
						pDockInfo.objDockSite.AddBar((Control)(object)this);
					}
					else
					{
						pDockInfo.objDockSite.AddBar((Control)(object)this, pDockInfo.InsertPosition);
					}
					m_DockingInProgress = false;
				}
				else if (pDockInfo.objDockSite != null && pDockInfo.objDockSite != ((Control)this).Parent)
				{
					m_DockingInProgress = true;
					if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
					{
						((ContainerControl)owner.ParentForm).ActiveControl = null;
						((Control)this).Focus();
					}
					else if (owner.ParentForm != null && IsAnyControl((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
					{
						((ContainerControl)owner.ParentForm).ActiveControl = null;
						((Control)this).Focus();
					}
					if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
					{
						if (((DockSite)(object)((Control)this).Parent).IsDocumentDock || ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
						{
							((DockSite)(object)((Control)this).Parent).GetDocumentUIManager().UnDock(this);
						}
						else
						{
							((DockSite)(object)((Control)this).Parent).RemoveBar((Control)(object)this);
						}
					}
					if (!Visible && !m_BarDefinitionLoading)
					{
						Visible = true;
					}
					m_DockOffset = pDockInfo.DockOffset;
					m_DockLine = pDockInfo.DockLine;
					if (pDockInfo.objDockSite.IsDocumentDock || pDockInfo.objDockSite.DocumentDockContainer != null)
					{
						pDockInfo.objDockSite.GetDocumentUIManager().Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
					}
					else if (pDockInfo.InsertPosition == -10)
					{
						pDockInfo.objDockSite.AddBar((Control)(object)this);
					}
					else
					{
						pDockInfo.objDockSite.AddBar((Control)(object)this, pDockInfo.InsertPosition);
					}
					m_DockingInProgress = false;
				}
				else if (((Control)this).Parent != null && pDockInfo.objDockSite == ((Control)this).Parent)
				{
					if (pDockInfo.objDockSite.IsDocumentDock || pDockInfo.objDockSite.DocumentDockContainer != null)
					{
						pDockInfo.objDockSite.GetDocumentUIManager().Dock(pDockInfo.MouseOverBar, this, pDockInfo.MouseOverDockSide);
					}
					else if (m_DockLine != pDockInfo.DockLine || (m_DockOffset != pDockInfo.DockOffset && !Stretch) || ((Control)pDockInfo.objDockSite).Controls.GetChildIndex((Control)(object)this) != pDockInfo.InsertPosition || pDockInfo.NewLine)
					{
						m_DockLine = pDockInfo.DockLine;
						m_DockOffset = pDockInfo.DockOffset;
						if (!Visible && !m_BarDefinitionLoading)
						{
							Visible = true;
						}
						if (pDockInfo.NewLine)
						{
							pDockInfo.objDockSite.SetBarPosition((Control)(object)this, pDockInfo.InsertPosition, bInNewLine: true);
						}
						else if (pDockInfo.InsertPosition == -10)
						{
							pDockInfo.objDockSite.AdjustBarPosition((Control)(object)this);
							pDockInfo.objDockSite.RecalcLayout();
						}
						else
						{
							pDockInfo.objDockSite.SetBarPosition((Control)(object)this, pDockInfo.InsertPosition);
						}
					}
				}
				else
				{
					Point location = new Point(p.X - m_MouseDownPt.X, p.Y - m_MouseDownPt.Y);
					ScreenInformation screenInformation = BarFunctions.ScreenFromControl((Control)(object)m_Float);
					if (screenInformation != null && location.Y + 8 >= screenInformation.WorkingArea.Bottom)
					{
						location.Y = screenInformation.WorkingArea.Bottom - 8;
					}
					((Form)m_Float).Location = location;
					if (((IOwner)m_Owner).ParentForm != null)
					{
						((Control)((IOwner)m_Owner).ParentForm).Update();
					}
				}
			}
		}
		if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && (m_TabDockItems == null || !((Control)m_TabDockItems).Visible))
		{
			int visibleSubItems = m_ItemContainer.VisibleSubItems;
			if ((m_BarState != eBarState.Floating || visibleSubItems > 1) && (m_AlwaysDisplayDockTab || visibleSubItems > 0))
			{
				eBarState barState = m_BarState;
				m_BarState = eBarState.Docked;
				RefreshDockTab(bFireEvents: false);
				m_BarState = barState;
			}
		}
		if (m_BarState != eBarState.Floating)
		{
			InvokeBarDockEvents();
		}
	}

	internal void InvokeBarDockEvents()
	{
		if (this.BarDock != null)
		{
			this.BarDock(this, new EventArgs());
		}
		if (m_Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeBarDock(this, new EventArgs());
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.BarDocked));
	}

	private Size GetFloatingSize()
	{
		Size result = m_FloatingRect.Size;
		if (result.IsEmpty)
		{
			if (m_ItemContainer.LayoutType == eLayoutType.DockContainer)
			{
				int selectedDockTab = SelectedDockTab;
				if (selectedDockTab >= 0 && m_ItemContainer.SubItems[selectedDockTab] is DockContainerItem)
				{
					DockContainerItem dockContainerItem = m_ItemContainer.SubItems[selectedDockTab] as DockContainerItem;
					result = dockContainerItem.DefaultFloatingSize;
				}
				else if (m_ItemContainer.SubItems.Count <= 0 || !(m_ItemContainer.SubItems[0] is DockContainerItem))
				{
					result = ((m_ItemContainer.Orientation != eOrientation.Vertical) ? new Size(m_ItemContainer.HeightInternal + 24, m_ItemContainer.HeightInternal + 24) : new Size(m_ItemContainer.WidthInternal + 16, m_ItemContainer.WidthInternal + 16));
				}
				else
				{
					DockContainerItem dockContainerItem2 = m_ItemContainer.SubItems[0] as DockContainerItem;
					result = dockContainerItem2.DefaultFloatingSize;
				}
			}
			else
			{
				result = Screen.FromControl((Control)(object)this).WorkingArea.Size;
			}
		}
		return result;
	}

	private void RemoveTempTabBarItems()
	{
		if (m_TempTabBar == null)
		{
			return;
		}
		ArrayList arrayList = new ArrayList(m_TempTabBar.Items.Count);
		m_TempTabBar.Items.CopyTo(arrayList);
		foreach (BaseItem item in arrayList)
		{
			if (item.Tag != null && item.Tag.ToString() == "systempdockitem")
			{
				m_TempTabBar.Items.Remove(item);
			}
		}
	}

	internal void DragMouseMove()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		((Control)this).OnMouseMove(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
	}

	internal void DragMouseUp()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		((Control)this).OnMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
	}

	internal void InternalMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	internal void OnEscapeKey()
	{
		EndDocking(revertLast: true, Point.Empty);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		InvalidateFontChange();
		((Control)this).OnFontChanged(e);
	}

	private void InvalidateFontChange()
	{
		if (m_ItemContainer != null)
		{
			BarUtilities.InvalidateFontChange(m_ItemContainer.SubItems);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_0f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1a: Invalid comparison between Unknown and I4
		//IL_0f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Invalid comparison between Unknown and I4
		//IL_1fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff6: Invalid comparison between Unknown and I4
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Invalid comparison between Unknown and I4
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Invalid comparison between Unknown and I4
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Invalid comparison between Unknown and I4
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Invalid comparison between Unknown and I4
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Invalid comparison between Unknown and I4
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Invalid comparison between Unknown and I4
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Invalid comparison between Unknown and I4
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Invalid comparison between Unknown and I4
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Invalid comparison between Unknown and I4
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Invalid comparison between Unknown and I4
		//IL_1d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3f: Expected O, but got Unknown
		//IL_1f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f53: Expected O, but got Unknown
		if ((int)e.Button == 1048576 && HotDragDrop && !((Control)this).ClientRectangle.Contains(new Point(e.X, e.Y)) && LatestMouseOverItem != null)
		{
			ButtonItem latestMouseOverItem = LatestMouseOverItem;
			LatestMouseOverItem = null;
			if (this.StartDragDrop != null && latestMouseOverItem.Tag != null)
			{
				this.StartDragDrop(latestMouseOverItem, EventArgs.Empty);
			}
			LatestMouseOverItem = null;
		}
		if (m_InMouseMove)
		{
			return;
		}
		m_InMouseMove = true;
		if (m_MoveWindow && m_BarState != 0 && (int)e.Button == 1048576)
		{
			if (((Control)this).Cursor != Cursors.SizeAll)
			{
				((Control)this).Cursor = Cursors.SizeAll;
			}
			Point mousePosition = Control.MousePosition;
			Point point = ((Control)this).PointToClient(mousePosition);
			if (!(m_Owner is IOwnerBarSupport ownerBarSupport) || (Math.Abs(point.X - m_MouseDownPt.X) <= 4 && Math.Abs(point.Y - m_MouseDownPt.Y) <= 4))
			{
				((Control)this).OnMouseMove(e);
				m_InMouseMove = false;
				return;
			}
			DockSiteInfo pDockInfo = ownerBarSupport.GetDockInfo(this, mousePosition.X, mousePosition.Y);
			if (pDockInfo.objDockSite == null && !m_CanUndock)
			{
				((Control)this).OnMouseMove(e);
				m_InMouseMove = false;
				return;
			}
			bool flag = true;
			if (!m_OldOutlineRectangle.IsEmpty)
			{
				NativeFunctions.DrawReversibleDesktopRect(m_OldOutlineRectangle, 3);
				m_OldOutlineRectangle = Rectangle.Empty;
			}
			if (pDockInfo.UseOutline && pDockInfo.objDockSite != null)
			{
				Rectangle barDockRectangle = pDockInfo.objDockSite.GetBarDockRectangle(this, ref pDockInfo);
				if (!barDockRectangle.IsEmpty)
				{
					flag = false;
					if (m_OutlineForm == null)
					{
						m_OutlineForm = CreateOutlineForm();
					}
					NativeFunctions.SetWindowPos(((Control)m_OutlineForm).Handle, new IntPtr(0), barDockRectangle.X, barDockRectangle.Y, barDockRectangle.Width, barDockRectangle.Height, 80);
				}
			}
			if (flag && m_OutlineForm != null)
			{
				((Control)m_OutlineForm).Visible = false;
			}
			m_DragDockInfo = pDockInfo;
			if (flag)
			{
				DockingHandler(pDockInfo, mousePosition);
			}
		}
		else if ((int)e.Button == 0)
		{
			if (m_BarState == eBarState.Floating)
			{
				if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && ((e.X <= 2 && e.Y <= 2) || (e.X >= ((Control)this).Width - 4 && e.Y >= ((Control)this).Height - 4)))
				{
					((Control)this).Cursor = Cursors.SizeNWSE;
				}
				else if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && ((e.X >= ((Control)this).Width - 2 && e.Y <= 2) || (e.X <= 4 && e.Y >= ((Control)this).Height - 4)))
				{
					((Control)this).Cursor = Cursors.SizeNESW;
				}
				else if ((e.X >= 0 && e.X <= 2) || (e.X >= ((Control)this).Width - 3 && e.X <= ((Control)this).Width))
				{
					((Control)this).Cursor = Cursors.SizeWE;
				}
				else if ((e.Y >= 0 && e.Y <= 2) || (e.Y >= ((Control)this).Height - 3 && e.Y <= ((Control)this).Height))
				{
					((Control)this).Cursor = Cursors.SizeNS;
				}
				else if (((Control)this).Cursor == Cursors.SizeWE || ((Control)this).Cursor == Cursors.SizeNS || ((Control)this).Cursor == Cursors.SizeNWSE || ((Control)this).Cursor == Cursors.SizeNESW)
				{
					((Control)this).Cursor = Cursors.Default;
				}
			}
			else if (m_BarState == eBarState.AutoHide && m_ItemContainer.LayoutType == eLayoutType.DockContainer && m_ItemContainer.Stretch)
			{
				if ((e.X <= 2 && (int)m_LastDockSiteInfo.DockSide == 4) || (e.X >= ((Control)this).Width - 2 && (int)m_LastDockSiteInfo.DockSide == 3))
				{
					((Control)this).Cursor = Cursors.VSplit;
				}
				else if ((e.Y <= 2 && (int)m_LastDockSiteInfo.DockSide == 2) || (e.Y >= ((Control)this).Height - 2 && (int)m_LastDockSiteInfo.DockSide == 1))
				{
					((Control)this).Cursor = Cursors.HSplit;
				}
				else if (e.X <= 2 && ((int)m_LastDockSiteInfo.DockSide == 2 || (int)m_LastDockSiteInfo.DockSide == 1) && ((Control)this).Left > 0)
				{
					((Control)this).Cursor = Cursors.VSplit;
				}
				else if (e.Y <= 2 && ((int)m_LastDockSiteInfo.DockSide == 3 || (int)m_LastDockSiteInfo.DockSide == 4) && ((Control)this).Top > 0)
				{
					((Control)this).Cursor = Cursors.HSplit;
				}
				else if (((Control)this).Cursor == Cursors.HSplit || ((Control)this).Cursor == Cursors.VSplit)
				{
					((Control)this).Cursor = Cursors.Default;
				}
			}
			else if (m_BarState == eBarState.Docked && m_GrabHandleStyle == eGrabHandleStyle.ResizeHandle)
			{
				if ((e.X > Location.X + ((Control)this).Width - GrabHandleResizeWidth && (int)((Control)this).RightToLeft == 0) || (e.X < Location.X + GrabHandleResizeWidth && (int)((Control)this).RightToLeft == 1))
				{
					Form val = ((Control)this).FindForm();
					if (val != null && (int)val.WindowState == 2)
					{
						if (((Control)this).Cursor == Cursors.SizeNESW || ((Control)this).Cursor == Cursors.SizeNWSE)
						{
							((Control)this).Cursor = Cursors.Default;
						}
					}
					else if ((int)((Control)this).RightToLeft == 1)
					{
						((Control)this).Cursor = Cursors.SizeNESW;
					}
					else
					{
						((Control)this).Cursor = Cursors.SizeNWSE;
					}
				}
				else if (((Control)this).Cursor == Cursors.SizeNESW || ((Control)this).Cursor == Cursors.SizeNWSE)
				{
					((Control)this).Cursor = Cursors.Default;
				}
			}
			if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.OfficeXP || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2003 || m_ItemContainer.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_ItemContainer.EffectiveStyle))
			{
				if (!m_SystemButtons.CloseButtonRect.IsEmpty && m_SystemButtons.CloseButtonRect.Contains(e.X, e.Y))
				{
					if (!m_SystemButtons.MouseOverClose)
					{
						m_SystemButtons.MouseOverClose = true;
						PaintCloseButton();
					}
				}
				else if (m_SystemButtons.MouseOverClose)
				{
					m_SystemButtons.MouseOverClose = false;
					PaintCloseButton();
				}
				if (!m_SystemButtons.CaptionButtonRect.IsEmpty && m_SystemButtons.CaptionButtonRect.Contains(e.X, e.Y))
				{
					if (!m_SystemButtons.MouseOverCaption)
					{
						m_SystemButtons.MouseOverCaption = true;
						PaintCaptionButton();
					}
				}
				else if (m_SystemButtons.MouseOverCaption)
				{
					m_SystemButtons.MouseOverCaption = false;
					PaintCaptionButton();
				}
				if (!m_SystemButtons.CustomizeButtonRect.IsEmpty && m_SystemButtons.CustomizeButtonRect.Contains(e.X, e.Y))
				{
					if (!m_SystemButtons.MouseOverCustomize)
					{
						m_SystemButtons.MouseOverCustomize = true;
						PaintCustomizeButton();
					}
				}
				else if (m_SystemButtons.MouseOverCustomize)
				{
					m_SystemButtons.MouseOverCustomize = false;
					PaintCustomizeButton();
				}
				if (!m_SystemButtons.AutoHideButtonRect.IsEmpty && m_SystemButtons.AutoHideButtonRect.Contains(e.X, e.Y))
				{
					if (!m_SystemButtons.MouseOverAutoHide)
					{
						m_SystemButtons.MouseOverAutoHide = true;
						PaintAutoHideButton();
					}
				}
				else if (m_SystemButtons.MouseOverAutoHide)
				{
					m_SystemButtons.MouseOverAutoHide = false;
					PaintAutoHideButton();
				}
			}
		}
		else if ((int)e.Button == 1048576 && m_SizeWindow != 0 && m_BarState == eBarState.Floating)
		{
			Size clientSize = ((Control)this).ClientSize;
			Size empty = Size.Empty;
			Size size = MinimumDockSize(m_ItemContainer.Orientation);
			if (m_SizeWindow == 5 || m_SizeWindow == 6)
			{
				empty = ((m_SizeWindow != 6) ? RecalcSizeOnly(new Size(((Control)this).Width - e.X, ((Control)this).Height - e.Y)) : RecalcSizeOnly(new Size(e.X, e.Y)));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					if (m_SizeWindow == 6)
					{
						((Control)this).ClientSize = empty;
					}
					else
					{
						((Form)m_Float).Location = new Point(((Control)m_Float).Left + (((Control)this).Width - empty.Width), ((Control)m_Float).Top + (((Control)this).Height - empty.Height));
						((Control)this).ClientSize = empty;
					}
					RecalcSize();
					((Control)this).Update();
					if (m_Owner is IOwner { ParentForm: not null } owner)
					{
						((Control)owner.ParentForm).Update();
					}
				}
			}
			else if (m_SizeWindow == 7 || m_SizeWindow == 8)
			{
				empty = ((m_SizeWindow != 8) ? RecalcSizeOnly(new Size(e.X, ((Control)this).Height - e.Y)) : RecalcSizeOnly(new Size(((Control)this).Width - e.X, e.Y)));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					if (m_SizeWindow == 8)
					{
						((Form)m_Float).Location = new Point(((Control)m_Float).Left + (((Control)this).Width - empty.Width), ((Control)m_Float).Top);
						((Control)this).ClientSize = empty;
					}
					else
					{
						((Form)m_Float).Location = new Point(((Control)m_Float).Left, ((Control)m_Float).Top + (((Control)this).Height - empty.Height));
						((Control)this).ClientSize = empty;
					}
					RecalcSize();
					((Control)this).Update();
					if (m_Owner is IOwner { ParentForm: not null } owner2)
					{
						((Control)owner2.ParentForm).Update();
					}
				}
			}
			else if (m_SizeWindow == 2)
			{
				empty = RecalcSizeOnly(new Size(e.X, ((Control)this).Height));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					((Control)this).ClientSize = empty;
					RecalcSize();
					((Control)this).Update();
					if (m_Owner is IOwner { ParentForm: not null } owner3)
					{
						((Control)owner3.ParentForm).Update();
					}
				}
			}
			else if (m_SizeWindow == 1)
			{
				empty = RecalcSizeOnly(new Size(((Control)this).Width - e.X, ((Control)this).Height));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					int right = ((Control)m_Float).Right;
					((Control)this).ClientSize = empty;
					RecalcSize();
					((Control)m_Float).Left = ((Control)m_Float).Left + right - ((Control)m_Float).Right;
					((Control)this).Update();
					if (m_Owner is IOwner { ParentForm: not null } owner4)
					{
						((Control)owner4.ParentForm).Update();
					}
				}
			}
			else if (m_SizeWindow == 4)
			{
				if (e.Y > 0)
				{
					empty = ((m_ItemContainer.LayoutType != eLayoutType.TaskList && m_ItemContainer.LayoutType != eLayoutType.DockContainer) ? RecalcSizeOnly(new Size((int)((float)m_MouseDownSize.Width * ((float)m_MouseDownSize.Height / (float)e.Y)), ((Control)this).Height)) : RecalcSizeOnly(new Size(((Control)this).Width, e.Y)));
					if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
					{
						((Control)this).ClientSize = empty;
						RecalcSize();
						((Control)this).Update();
						if (m_Owner is IOwner { ParentForm: not null } owner5)
						{
							((Control)owner5.ParentForm).Update();
						}
					}
				}
			}
			else if (m_SizeWindow == 3 && e.Y != 0)
			{
				empty = ((m_ItemContainer.LayoutType != eLayoutType.TaskList && m_ItemContainer.LayoutType != eLayoutType.DockContainer) ? RecalcSizeOnly(new Size((int)((float)m_MouseDownSize.Width * ((float)m_MouseDownSize.Height / (float)(((Control)this).Height - e.Y))), ((Control)this).Height)) : RecalcSizeOnly(new Size(((Control)this).Width, ((Control)this).Height - e.Y)));
				if (!clientSize.Equals((object?)empty) && empty.Width >= size.Width && empty.Height >= size.Height)
				{
					int bottom = ((Control)m_Float).Bottom;
					((Control)this).ClientSize = empty;
					RecalcSize();
					((Control)m_Float).Top = ((Control)m_Float).Top + bottom - ((Control)m_Float).Bottom;
					((Control)this).Update();
					if (m_Owner is IOwner { ParentForm: not null } owner6)
					{
						((Control)owner6.ParentForm).Update();
					}
				}
			}
			m_FloatingRect = new Rectangle(Location, Size);
		}
		else if (((int)e.Button == 1048576 && m_SizeWindow != 0 && m_BarState == eBarState.AutoHide) || (m_SizeWindow == 15 && m_BarState == eBarState.Docked))
		{
			Size clientSize2 = ((Control)this).ClientSize;
			Size empty2 = Size.Empty;
			if (m_SizeWindow == 15)
			{
				if ((int)((Control)this).RightToLeft == 0)
				{
					if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
					{
						Point point2 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
						if (((Control)this).Parent.Parent.Parent != null)
						{
							Point point3 = ((Control)this).Parent.Parent.Parent.PointToScreen(((Control)this).Parent.Parent.Location);
							((Control)this).Parent.Parent.Size = new Size(point2.X - point3.X + m_ResizeOffset.X, point2.Y - point3.Y + m_ResizeOffset.Y);
						}
						else
						{
							((Control)this).Parent.Parent.Size = new Size(point2.X - ((Control)this).Parent.Parent.Location.X + m_ResizeOffset.X, point2.Y - ((Control)this).Parent.Parent.Location.Y + m_ResizeOffset.Y);
						}
						if (m_Owner is IOwner { ParentForm: not null } owner7)
						{
							((Control)owner7.ParentForm).Update();
						}
					}
					else if (m_Owner == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is FloatingContainer))
					{
						Form val2 = ((Control)this).FindForm();
						if (val2 != null)
						{
							Point point4 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
							val2.Size = new Size(point4.X - val2.Location.X + m_ResizeOffset.X, point4.Y - val2.Location.Y + m_ResizeOffset.Y);
							((Control)val2).Update();
						}
					}
				}
				else if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
				{
					Point point5 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
					if (((Control)this).Parent.Parent.Parent != null)
					{
						Point point6 = ((Control)this).Parent.Parent.Parent.PointToScreen(((Control)this).Parent.Parent.Location);
						Rectangle bounds = ((Control)this).Parent.Parent.Bounds;
						bounds.X = point5.X - m_ResizeOffset.X;
						bounds.Width += ((Control)this).Parent.Parent.Left - bounds.X;
						bounds.Height = point5.Y - point6.Y + m_ResizeOffset.Y;
						((Control)this).Parent.Parent.Bounds = bounds;
					}
					else
					{
						Rectangle bounds2 = ((Control)this).Parent.Parent.Bounds;
						bounds2.X = point5.X - m_ResizeOffset.X;
						bounds2.Width += ((Control)this).Parent.Parent.Left - bounds2.X;
						bounds2.Height = point5.Y - ((Control)this).Parent.Parent.Location.Y + m_ResizeOffset.Y;
						((Control)this).Parent.Parent.Bounds = bounds2;
					}
					if (m_Owner is IOwner { ParentForm: not null } owner8)
					{
						((Control)owner8.ParentForm).Update();
					}
				}
				else if (m_Owner == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is FloatingContainer))
				{
					Form val3 = ((Control)this).FindForm();
					if (val3 != null)
					{
						Point point7 = ((Control)this).PointToScreen(new Point(e.X, e.Y));
						Rectangle bounds3 = ((Control)val3).Bounds;
						bounds3.X = point7.X - m_ResizeOffset.X;
						bounds3.Width += ((Control)val3).Left - bounds3.X;
						bounds3.Height = point7.Y - val3.Location.Y + m_ResizeOffset.Y;
						((Control)val3).Bounds = bounds3;
						((Control)val3).Update();
					}
				}
			}
			else if (m_SizeWindow == 9)
			{
				int formClientWidth = GetFormClientWidth();
				int num = e.X;
				int num2 = 32;
				if (m_Owner is DotNetBarManager)
				{
					num2 = ((DotNetBarManager)m_Owner).MinimumClientSize.Width;
				}
				if (num2 > 0 && formClientWidth + num < num2)
				{
					num = num2 - formClientWidth;
				}
				if (formClientWidth + num >= num2 || num2 == 0)
				{
					int minWidth = m_ItemContainer.MinWidth;
					m_ItemContainer.MinWidth = 0;
					empty2 = RecalcSizeOnly(new Size(((Control)this).Width - num, ((Control)this).Height));
					m_ItemContainer.MinWidth = minWidth;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (m_BarState == eBarState.AutoHide)
						{
							Rectangle bounds4 = ((Control)this).Bounds;
							EnableRedraw = false;
							try
							{
								m_ItemContainer.MinWidth = m_ItemContainer.WidthInternal - num;
								((Control)this).Width = ((Control)this).Width - num;
								RecalcSize();
								((Control)this).Left = ((Control)this).Left + (clientSize2.Width - Size.Width);
							}
							finally
							{
								EnableRedraw = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds4, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int minWidth2 = m_ItemContainer.MinWidth;
							m_ItemContainer.MinWidth = m_ItemContainer.WidthInternal - num;
							if (m_ItemContainer.MinWidth < minWidth2)
							{
								SyncLineMinWidth();
							}
							RecalcLayout();
						}
						if (m_BarState == eBarState.AutoHide)
						{
							m_LastDockSiteInfo.DockedWidth = ((Control)this).Width;
						}
					}
				}
			}
			else if (m_SizeWindow == 10)
			{
				int formClientWidth2 = GetFormClientWidth();
				int num3 = e.X;
				int num4 = 32;
				if (m_Owner is DotNetBarManager)
				{
					num4 = ((DotNetBarManager)m_Owner).MinimumClientSize.Width;
				}
				if (num4 > 0 && formClientWidth2 - (num3 - ((Control)this).Width) < num4)
				{
					num3 = formClientWidth2 + ((Control)this).Width - num4;
				}
				if (formClientWidth2 - (num3 - ((Control)this).Width) >= num4 || num4 == 0)
				{
					int minWidth3 = m_ItemContainer.MinWidth;
					m_ItemContainer.MinWidth = 0;
					empty2 = RecalcSizeOnly(new Size(num3, ((Control)this).Height));
					m_ItemContainer.MinWidth = minWidth3;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (m_BarState == eBarState.AutoHide)
						{
							Rectangle bounds5 = ((Control)this).Bounds;
							EnableRedraw = false;
							try
							{
								m_ItemContainer.MinWidth = m_ItemContainer.WidthInternal + (num3 - ((Control)this).Width);
								((Control)this).Width = ((Control)this).Width + (num3 - ((Control)this).Width);
								RecalcSize();
							}
							finally
							{
								EnableRedraw = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds5, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int minWidth4 = m_ItemContainer.MinWidth;
							m_ItemContainer.MinWidth = m_ItemContainer.WidthInternal + (num3 - ((Control)this).Width);
							if (m_ItemContainer.MinWidth < minWidth4)
							{
								SyncLineMinWidth();
							}
							RecalcLayout();
						}
						if (m_BarState == eBarState.AutoHide)
						{
							m_LastDockSiteInfo.DockedWidth = ((Control)this).Width;
						}
					}
				}
			}
			else if (m_SizeWindow == 12)
			{
				int formClientHeight = GetFormClientHeight();
				int num5 = e.Y;
				int num6 = 32;
				if (m_Owner is DotNetBarManager)
				{
					num6 = ((DotNetBarManager)m_Owner).MinimumClientSize.Height;
				}
				if (num6 > 0 && formClientHeight + num5 < num6)
				{
					num5 = num6 - formClientHeight;
				}
				if (formClientHeight + num5 >= num6 || num6 == 0)
				{
					int minHeight = m_ItemContainer.MinHeight;
					m_ItemContainer.MinHeight = 0;
					empty2 = RecalcSizeOnly(new Size(((Control)this).Width, ((Control)this).Height - num5));
					m_ItemContainer.MinHeight = minHeight;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (m_BarState == eBarState.AutoHide)
						{
							Rectangle bounds6 = ((Control)this).Bounds;
							EnableRedraw = false;
							try
							{
								m_ItemContainer.MinHeight = m_ItemContainer.HeightInternal - num5;
								((Control)this).Height = ((Control)this).Height - num5;
								RecalcSize();
								((Control)this).Top = ((Control)this).Top + (clientSize2.Height - ((Control)this).Height);
							}
							finally
							{
								EnableRedraw = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds6, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int minHeight2 = m_ItemContainer.MinHeight;
							Size size2 = MinimumDockSize(m_ItemContainer.Orientation);
							if (m_ItemContainer.HeightInternal - num5 >= size2.Height)
							{
								m_ItemContainer.MinHeight = m_ItemContainer.HeightInternal - num5;
							}
							if (m_ItemContainer.MinHeight < minHeight2)
							{
								SyncLineMinHeight();
							}
							RecalcLayout();
						}
						if (m_BarState == eBarState.AutoHide)
						{
							m_LastDockSiteInfo.DockedHeight = ((Control)this).Height;
						}
					}
				}
			}
			else if (m_SizeWindow == 11)
			{
				int formClientHeight2 = GetFormClientHeight();
				int num7 = e.Y;
				int num8 = 32;
				if (m_Owner is DotNetBarManager)
				{
					num8 = ((DotNetBarManager)m_Owner).MinimumClientSize.Height;
				}
				if (num8 > 0 && formClientHeight2 - (num7 - ((Control)this).Height) < num8)
				{
					num7 = formClientHeight2 + ((Control)this).Height - num8;
				}
				if (formClientHeight2 - (num7 - ((Control)this).Height) >= num8 || num8 == 0)
				{
					int minHeight3 = m_ItemContainer.MinHeight;
					m_ItemContainer.MinHeight = 0;
					empty2 = RecalcSizeOnly(new Size(((Control)this).Width, num7));
					m_ItemContainer.MinHeight = minHeight3;
					if (!clientSize2.Equals((object?)empty2))
					{
						if (m_BarState == eBarState.AutoHide)
						{
							Rectangle bounds7 = ((Control)this).Bounds;
							EnableRedraw = false;
							try
							{
								m_ItemContainer.MinHeight = m_ItemContainer.HeightInternal + (num7 - ((Control)this).Height);
								((Control)this).Height = num7;
								RecalcSize();
							}
							finally
							{
								EnableRedraw = true;
							}
							if (((Control)this).Parent != null)
							{
								((Control)this).Parent.Invalidate(bounds7, true);
								((Control)this).Parent.Invalidate(((Control)this).Bounds, true);
								((Control)this).Parent.Update();
							}
							else
							{
								((Control)this).Refresh();
							}
						}
						else
						{
							int minHeight4 = m_ItemContainer.MinHeight;
							m_ItemContainer.MinHeight = m_ItemContainer.HeightInternal + (num7 - ((Control)this).Height);
							if (m_ItemContainer.MinHeight < minHeight4)
							{
								SyncLineMinHeight();
							}
							RecalcLayout();
						}
						if (m_BarState == eBarState.AutoHide)
						{
							m_LastDockSiteInfo.DockedHeight = ((Control)this).Height;
						}
					}
				}
			}
			else if (m_SizeWindow == 13 && ((Control)this).Parent.Controls.IndexOf((Control)(object)this) > 0)
			{
				Bar previousVisibleBar = GetPreviousVisibleBar(this);
				if (previousVisibleBar != null && previousVisibleBar.DockLine == DockLine)
				{
					Size adjustedFullSize = GetAdjustedFullSize(previousVisibleBar.MinimumDockSize(eOrientation.Horizontal));
					Size adjustedFullSize2 = GetAdjustedFullSize(MinimumDockSize(eOrientation.Horizontal));
					int num9 = e.X;
					if (((Control)previousVisibleBar).Width + num9 < adjustedFullSize.Width)
					{
						num9 += adjustedFullSize.Width - (((Control)previousVisibleBar).Width + num9) - 1;
					}
					if (((Control)this).Width - num9 < adjustedFullSize2.Width)
					{
						num9 -= adjustedFullSize2.Width - (((Control)this).Width - num9) + 1;
					}
					if (((Control)previousVisibleBar).Width + num9 >= adjustedFullSize.Width && ((Control)this).Width - num9 >= adjustedFullSize2.Width)
					{
						Size size3 = previousVisibleBar.Size;
						Size size4 = previousVisibleBar.RecalcSizeOnly(new Size(((Control)previousVisibleBar).Width + num9, ((Control)this).Height));
						empty2 = RecalcSizeOnly(new Size(((Control)this).Width - num9, ((Control)this).Height));
						if (!size3.Equals((object?)size4) && !clientSize2.Equals((object?)empty2))
						{
							SplitDockWidth = 0;
							previousVisibleBar.SplitDockWidth = ((Control)previousVisibleBar).Width + num9;
							foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
							{
								Control val4 = item;
								if (val4 != this && val4 != previousVisibleBar && val4.Visible && val4 is Bar)
								{
									Bar bar = val4 as Bar;
									if (bar.DockLine == DockLine && bar.SplitDockHeight == 0)
									{
										bar.SplitDockWidth = ((Control)bar).Width;
									}
								}
							}
							RecalcLayout();
						}
					}
				}
			}
			else if (m_SizeWindow == 14 && ((Control)this).Parent.Controls.IndexOf((Control)(object)this) > 0)
			{
				Bar previousVisibleBar2 = GetPreviousVisibleBar(this);
				if (previousVisibleBar2 != null && previousVisibleBar2.DockLine == DockLine)
				{
					Size adjustedFullSize3 = GetAdjustedFullSize(previousVisibleBar2.MinimumDockSize(eOrientation.Horizontal));
					Size adjustedFullSize4 = GetAdjustedFullSize(MinimumDockSize(eOrientation.Horizontal));
					int num10 = e.Y;
					if (((Control)previousVisibleBar2).Height + num10 < adjustedFullSize3.Height)
					{
						num10 += adjustedFullSize3.Height - (((Control)previousVisibleBar2).Height + num10) - 1;
					}
					if (((Control)this).Height - num10 < adjustedFullSize4.Height)
					{
						num10 -= adjustedFullSize4.Height - (((Control)this).Height - num10) + 1;
					}
					if (((Control)previousVisibleBar2).Height + num10 >= adjustedFullSize3.Height && ((Control)this).Height - num10 >= adjustedFullSize4.Height)
					{
						Size size5 = previousVisibleBar2.Size;
						Size size6 = previousVisibleBar2.RecalcSizeOnly(new Size(((Control)previousVisibleBar2).Width, ((Control)this).Height + num10));
						empty2 = RecalcSizeOnly(new Size(((Control)this).Width, ((Control)this).Height - num10));
						if (!size5.Equals((object?)size6) && !clientSize2.Equals((object?)empty2))
						{
							SplitDockHeight = 0;
							previousVisibleBar2.SplitDockHeight = ((Control)previousVisibleBar2).Height + num10;
							foreach (Control item2 in (ArrangedElementCollection)((Control)this).Parent.Controls)
							{
								Control val5 = item2;
								if (val5 != this && val5 != previousVisibleBar2 && val5.Visible && val5 is Bar)
								{
									Bar bar2 = val5 as Bar;
									if (bar2.DockLine == DockLine && bar2.SplitDockHeight == 0)
									{
										bar2.SplitDockHeight = ((Control)bar2).Height;
									}
								}
							}
							RecalcLayout();
						}
					}
				}
			}
		}
		if (m_BarState == eBarState.Popup && m_ParentItem != null && m_ParentItem.DesignMode && (int)e.Button == 1048576 && (Math.Abs(e.X - m_MouseDownPt.X) >= 2 || Math.Abs(e.Y - m_MouseDownPt.Y) >= 2 || m_DragDropInProgress))
		{
			BaseItem focusItem = m_FocusItem;
			if (m_Owner is IOwner)
			{
				focusItem = ((IOwner)m_Owner).GetFocusItem();
			}
			ISite site = GetSite();
			if (site != null && focusItem != null)
			{
				DesignTimeMouseMove(e);
			}
		}
		((Control)this).OnMouseMove(e);
		if (m_ItemContainer.SubItems.Count == 0)
		{
			m_InMouseMove = false;
			return;
		}
		if (!m_MoveWindow && m_SizeWindow == 0)
		{
			m_ItemContainer.InternalMouseMove(e);
		}
		m_InMouseMove = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Form CreateOutlineForm()
	{
		return BarFunctions.CreateOutlineForm();
	}

	private Bar GetPreviousVisibleBar(Bar startBar)
	{
		if (((Control)this).Parent == null)
		{
			return null;
		}
		int num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this) - 1;
		if (num >= 0)
		{
			for (int num2 = num; num2 >= 0; num2--)
			{
				if (((Control)this).Parent.Controls[num2].Visible)
				{
					return ((Control)this).Parent.Controls[num2] as Bar;
				}
			}
		}
		return null;
	}

	private Bar GetNextVisibleBar(Bar startBar)
	{
		if (((Control)this).Parent == null)
		{
			return null;
		}
		int num = ((Control)this).Parent.Controls.IndexOf((Control)(object)this) + 1;
		if (num >= 0)
		{
			for (int i = num; i < ((ArrangedElementCollection)((Control)this).Parent.Controls).Count; i++)
			{
				if (((Control)this).Parent.Controls[i].Visible)
				{
					return ((Control)this).Parent.Controls[i] as Bar;
				}
			}
		}
		return null;
	}

	private int GetFormClientWidth()
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		if (!(m_Owner is IOwner owner) || ((Control)this).Parent == null || (((Control)this).Parent.Parent == null && m_BarState != eBarState.AutoHide))
		{
			return 0;
		}
		Control val = ((Control)this).Parent.Parent;
		if (m_BarState == eBarState.AutoHide)
		{
			val = (Control)(object)owner.ParentForm;
		}
		if (val == null && owner.ParentForm == null && m_Owner is DotNetBarManager)
		{
			val = ((DotNetBarManager)m_Owner).ParentUserControl;
		}
		if (val == null)
		{
			return 0;
		}
		int num = val.ClientSize.Width;
		foreach (Control item in (ArrangedElementCollection)val.Controls)
		{
			Control val2 = item;
			if (val2.Visible && ((int)val2.Dock == 3 || (int)val2.Dock == 4))
			{
				num -= val2.Width;
			}
		}
		if (m_BarState == eBarState.AutoHide && Visible)
		{
			num -= ((Control)this).Width;
		}
		return num;
	}

	private int GetFormClientHeight()
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		if (!(m_Owner is IOwner owner) || ((Control)this).Parent == null || (((Control)this).Parent.Parent == null && m_BarState != eBarState.AutoHide))
		{
			return 0;
		}
		Control val = null;
		val = (Control)((m_BarState != eBarState.AutoHide) ? ((object)((Control)this).Parent.Parent) : ((object)owner.ParentForm));
		if (val == null && owner.ParentForm == null && m_Owner is DotNetBarManager)
		{
			val = ((DotNetBarManager)m_Owner).ParentUserControl;
		}
		if (val == null)
		{
			return 0;
		}
		int num = val.ClientSize.Height;
		foreach (Control item in (ArrangedElementCollection)val.Controls)
		{
			Control val2 = item;
			if (val2.Visible && ((int)val2.Dock == 1 || (int)val2.Dock == 2))
			{
				num -= val2.Height;
			}
		}
		if (m_BarState == eBarState.AutoHide && Visible)
		{
			num -= ((Control)this).Height;
		}
		return num;
	}

	internal void SyncLineMinWidth()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val is Bar bar && bar != this && bar.DockLine == m_DockLine)
			{
				bar.ItemsContainer.MinWidth = m_ItemContainer.MinWidth;
			}
		}
	}

	internal void SyncLineMinHeight()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val is Bar bar && bar != this && bar.DockLine == m_DockLine)
			{
				bar.ItemsContainer.MinHeight = m_ItemContainer.MinHeight;
			}
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		InternalMouseHover();
	}

	internal void InternalMouseHover()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (m_MoveWindow)
		{
			return;
		}
		if ((int)Control.MouseButtons == 1048576)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (!((Control)this).ClientRectangle.Contains(pt) && Owner is IOwnerMenuSupport ownerMenuSupport && ownerMenuSupport.RelayMouseHover())
			{
				return;
			}
		}
		m_ItemContainer.InternalMouseHover();
		if (m_SystemButtons.MouseOverAutoHide)
		{
			using (LocalizationManager localizationManager = new LocalizationManager(m_Owner as IOwnerLocalize))
			{
				string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.BarAutoHideButtonTooltip);
				if (localizedString != "")
				{
					ShowToolTip(localizedString);
				}
				return;
			}
		}
		if (m_SystemButtons.MouseOverCustomize)
		{
			using (LocalizationManager localizationManager2 = new LocalizationManager(m_Owner as IOwnerLocalize))
			{
				string localizedString2 = localizationManager2.GetLocalizedString(LocalizationKeys.BarCustomizeButtonTooltip);
				if (localizedString2 != "")
				{
					ShowToolTip(localizedString2);
				}
				return;
			}
		}
		if (!m_SystemButtons.MouseOverClose)
		{
			return;
		}
		using LocalizationManager localizationManager3 = new LocalizationManager(m_Owner as IOwnerLocalize);
		string localizedString3 = localizationManager3.GetLocalizedString(LocalizationKeys.BarCloseButtonTooltip);
		if (localizedString3 != "")
		{
			ShowToolTip(localizedString3);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		if (m_SystemButtons.MouseOverClose)
		{
			m_SystemButtons.MouseOverClose = false;
			PaintCloseButton();
		}
		if (m_SystemButtons.MouseOverCaption)
		{
			m_SystemButtons.MouseOverCaption = false;
			PaintCaptionButton();
		}
		if (m_SystemButtons.MouseOverCustomize)
		{
			m_SystemButtons.MouseOverCustomize = false;
			PaintCustomizeButton();
		}
		if (m_SystemButtons.MouseOverAutoHide)
		{
			m_SystemButtons.MouseOverAutoHide = false;
			PaintAutoHideButton();
		}
		if (!m_MoveWindow)
		{
			m_ItemContainer.InternalMouseLeave();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Invalid comparison between Unknown and I4
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Invalid comparison between Unknown and I4
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Invalid comparison between Unknown and I4
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Invalid comparison between Unknown and I4
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Invalid comparison between Unknown and I4
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Invalid comparison between Unknown and I4
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Invalid comparison between Unknown and I4
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Invalid comparison between Unknown and I4
		//IL_09d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09db: Invalid comparison between Unknown and I4
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Invalid comparison between Unknown and I4
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Invalid comparison between Unknown and I4
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Invalid comparison between Unknown and I4
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Invalid comparison between Unknown and I4
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Invalid comparison between Unknown and I4
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Invalid comparison between Unknown and I4
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Invalid comparison between Unknown and I4
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Invalid comparison between Unknown and I4
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Invalid comparison between Unknown and I4
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Invalid comparison between Unknown and I4
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Invalid comparison between Unknown and I4
		if (HotDragDrop)
		{
			foreach (ButtonItem item in Items)
			{
				if (item.ScreenRectangle.Contains(((Control)this).PointToScreen(new Point(e.X, e.Y))))
				{
					LatestMouseOverItem = item;
					break;
				}
			}
		}
		m_MouseDownPt = new Point(e.X, e.Y);
		m_MouseDownSize = Size;
		HideToolTip();
		if ((int)e.Button == 1048576 && !m_SystemButtons.CustomizeButtonRect.IsEmpty && m_SystemButtons.CustomizeButtonRect.Contains(m_MouseDownPt) && !((Component)this).DesignMode && !m_ItemContainer.DesignMode)
		{
			if (m_CustomizeMenu != null)
			{
				if (m_CustomizeMenu.GetOwner() == null)
				{
					m_CustomizeMenu.SetOwner(m_Owner);
				}
				Point point = ((m_ItemContainer.EffectiveStyle != eDotNetBarStyle.Office2000) ? new Point(m_SystemButtons.CustomizeButtonRect.Left, m_SystemButtons.CustomizeButtonRect.Bottom) : new Point(0, 16));
				if (point.X < 0)
				{
					point.X = 0;
				}
				point = ((Control)this).PointToScreen(point);
				m_CustomizeMenu.SetSourceControl((Control)(object)this);
				m_CustomizeMenu.Popup(point);
				((Control)this).OnMouseDown(e);
				return;
			}
			foreach (BaseItem subItem in m_ItemContainer.SubItems)
			{
				if (subItem is CustomizeItem && !subItem.Expanded)
				{
					if (m_ItemContainer.EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						((CustomizeItem)subItem).PopupLocation = new Point(((Control)this).Left - 128, ((Control)this).Top + 16);
					}
					else
					{
						((CustomizeItem)subItem).PopupLocation = new Point(m_SystemButtons.CustomizeButtonRect.Left, m_SystemButtons.CustomizeButtonRect.Bottom);
					}
					subItem.Expanded = true;
					((Control)this).OnMouseDown(e);
					return;
				}
			}
		}
		else
		{
			if ((int)e.Button == 1048576 && !m_SystemButtons.CloseButtonRect.IsEmpty && m_SystemButtons.CloseButtonRect.Contains(e.X, e.Y))
			{
				m_SystemButtons.MouseDownClose = true;
				PaintCloseButton();
				((Control)this).OnMouseDown(e);
				return;
			}
			if ((int)e.Button == 1048576 && !m_SystemButtons.CaptionButtonRect.IsEmpty && m_SystemButtons.CaptionButtonRect.Contains(e.X, e.Y))
			{
				m_SystemButtons.MouseDownCaption = true;
				PaintCaptionButton();
				((Control)this).OnMouseDown(e);
				return;
			}
			if ((int)e.Button == 1048576 && !m_SystemButtons.AutoHideButtonRect.IsEmpty && m_SystemButtons.AutoHideButtonRect.Contains(e.X, e.Y))
			{
				m_SystemButtons.MouseDownAutoHide = true;
				PaintAutoHideButton();
				((Control)this).OnMouseDown(e);
				return;
			}
			if ((int)e.Button == 1048576 && m_BarState == eBarState.Floating)
			{
				if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && e.X <= 4 && e.Y <= 4)
				{
					m_SizeWindow = 5;
				}
				else if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && e.X >= ((Control)this).Width - 4 && e.Y >= ((Control)this).Height - 4)
				{
					m_SizeWindow = 6;
				}
				else if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && e.X >= ((Control)this).Width - 4 && e.Y <= 4)
				{
					m_SizeWindow = 7;
				}
				else if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && e.X <= 4 && e.Y >= ((Control)this).Height - 4)
				{
					m_SizeWindow = 8;
				}
				else if (e.X >= 0 && e.X <= 2)
				{
					m_SizeWindow = 1;
				}
				else if (e.X >= ((Control)this).Width - 3 && e.X <= ((Control)this).Width)
				{
					m_SizeWindow = 2;
				}
				else if (e.Y >= 0 && e.Y <= 2)
				{
					m_SizeWindow = 3;
				}
				else if (e.Y >= ((Control)this).Height - 3 && e.Y <= ((Control)this).Height)
				{
					m_SizeWindow = 4;
				}
			}
			else if ((int)e.Button == 1048576 && m_BarState == eBarState.AutoHide && m_ItemContainer.LayoutType == eLayoutType.DockContainer && m_ItemContainer.Stretch)
			{
				if ((int)m_LastDockSiteInfo.DockSide == 4 && e.X <= 2)
				{
					m_SizeWindow = 9;
				}
				else if ((int)m_LastDockSiteInfo.DockSide == 3 && e.X >= ((Control)this).Width - 2)
				{
					m_SizeWindow = 10;
				}
				else if ((int)m_LastDockSiteInfo.DockSide == 2 && e.Y <= 2)
				{
					m_SizeWindow = 12;
				}
				else if ((int)m_LastDockSiteInfo.DockSide == 1 && e.Y >= ((Control)this).Height - 2)
				{
					m_SizeWindow = 11;
				}
				else if (((int)m_LastDockSiteInfo.DockSide == 2 || (int)m_LastDockSiteInfo.DockSide == 1) && e.X <= 2 && ((Control)this).Left > 0)
				{
					m_SizeWindow = 13;
				}
				else if (((int)m_LastDockSiteInfo.DockSide == 3 || (int)m_LastDockSiteInfo.DockSide == 4) && e.Y <= 2 && ((Control)this).Top > 0)
				{
					m_SizeWindow = 14;
				}
			}
			else if ((int)e.Button == 2097152 && CanCustomize && !m_ItemContainer.DesignMode && m_BarState != 0 && !m_MoveWindow && m_Owner is DotNetBarManager && m_Owner is IOwnerBarSupport ownerBarSupport)
			{
				ownerBarSupport.BarContextMenu((Control)(object)this, e);
			}
		}
		if ((int)e.Button == 1048576 && m_BarState == eBarState.Docked && m_GrabHandleStyle == eGrabHandleStyle.ResizeHandle && m_MouseDownPt.X > Location.X + ((Control)this).Width - GrabHandleResizeWidth && (int)((Control)this).RightToLeft == 0)
		{
			Form val = ((Control)this).FindForm();
			if ((val != null && (int)val.WindowState != 2) || val == null)
			{
				((Control)this).Capture = true;
				((Control)this).Cursor = Cursors.SizeNWSE;
				m_SizeWindow = 15;
				Point point2 = ((Control)this).PointToScreen(m_MouseDownPt);
				if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
				{
					point2 = ((Control)this).Parent.Parent.PointToClient(point2);
					m_ResizeOffset = new Point(((Control)this).Parent.Parent.Width - point2.X, ((Control)this).Parent.Parent.ClientRectangle.Height - point2.Y);
				}
				else if (m_Owner == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is FloatingContainer) && val != null)
				{
					point2 = ((Control)val).PointToClient(point2);
					m_ResizeOffset = new Point(((Control)val).Width - point2.X, ((Control)val).ClientRectangle.Height - point2.Y);
				}
			}
		}
		else if ((int)e.Button == 1048576 && m_BarState == eBarState.Docked && m_GrabHandleStyle == eGrabHandleStyle.ResizeHandle && m_MouseDownPt.X < Location.X + GrabHandleResizeWidth && (int)((Control)this).RightToLeft == 1)
		{
			Form val2 = ((Control)this).FindForm();
			if ((val2 != null && (int)val2.WindowState != 2) || val2 == null)
			{
				((Control)this).Capture = true;
				((Control)this).Cursor = Cursors.SizeNESW;
				m_SizeWindow = 15;
				Point mouseDownPt = m_MouseDownPt;
				if (((object)val2).GetType().GetProperty("RightToLeftLayout") != null && (bool)TypeDescriptor.GetProperties(val2)["RightToLeftLayout"].GetValue(val2))
				{
					mouseDownPt.X = ((Control)this).Width - mouseDownPt.X;
				}
				Point point3 = ((Control)this).PointToScreen(mouseDownPt);
				if (((Control)this).Parent != null && ((Control)this).Parent.Parent != null)
				{
					point3 = ((Control)this).Parent.Parent.PointToClient(point3);
					m_ResizeOffset = new Point(point3.X, ((Control)this).Parent.Parent.ClientRectangle.Height - point3.Y);
				}
				else if (m_Owner == null && !(((Control)this).Parent is DockSite) && !(((Control)this).Parent is FloatingContainer) && val2 != null)
				{
					point3 = ((Control)val2).PointToClient(point3);
					m_ResizeOffset = new Point(point3.X, ((Control)val2).ClientRectangle.Height - point3.Y);
				}
			}
		}
		if ((int)e.Button == 1048576 && _CanMove && !m_GrabHandleRect.IsEmpty && m_GrabHandleRect.Contains(m_MouseDownPt) && m_SizeWindow == 0 && !m_AutoHideState && (m_CustomizeMenu == null || !m_CustomizeMenu.Expanded))
		{
			((Control)this).Cursor = Cursors.SizeAll;
			((Control)this).Capture = true;
			m_MoveWindow = true;
		}
		if (m_BarState == eBarState.Popup && m_ParentItem != null && m_ParentItem.DesignMode)
		{
			DesignTimeMouseDown(e);
		}
		else
		{
			m_ItemContainer.InternalMouseDown(e);
		}
		((Control)this).OnMouseDown(e);
	}

	internal void InternalMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	internal void CloseBar()
	{
		BarClosingEventArgs barClosingEventArgs = new BarClosingEventArgs();
		InvokeBarClosing(barClosingEventArgs);
		if (!barClosingEventArgs.Cancel)
		{
			if (AutoHide)
			{
				AutoHide = false;
			}
			HideBar();
			IOwner owner = Owner as IOwner;
			if (owner.ParentForm != null && !((Control)owner.ParentForm).ContainsFocus)
			{
				((Control)owner.ParentForm).Focus();
			}
			InvokeUserVisibleChanged();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Invalid comparison between Unknown and I4
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Invalid comparison between Unknown and I4
		if (m_MoveWindow || m_SizeWindow != 0)
		{
			((Control)this).Cursor = Cursors.Default;
		}
		if (m_DragDropInProgress)
		{
			DesignTimeMouseUp(e);
		}
		if (m_MoveWindow)
		{
			EndDocking(revertLast: false, new Point(e.X, e.Y));
		}
		else if ((int)e.Button == 1048576 && !m_SystemButtons.CloseButtonRect.IsEmpty && m_SystemButtons.CloseButtonRect.Contains(m_MouseDownPt))
		{
			m_SystemButtons.MouseDownClose = false;
			PaintCloseButton();
			if (m_SystemButtons.CloseButtonRect.Contains(e.X, e.Y))
			{
				if (m_CloseSingleTab && SelectedDockContainerItem != null)
				{
					CloseDockTab(SelectedDockContainerItem);
				}
				else
				{
					CloseBar();
				}
			}
		}
		else if ((int)e.Button == 1048576 && !m_SystemButtons.CaptionButtonRect.IsEmpty && m_SystemButtons.CaptionButtonRect.Contains(m_MouseDownPt))
		{
			m_SystemButtons.MouseDownCaption = false;
			PaintCaptionButton();
			if (m_SystemButtons.CaptionButtonRect.Contains(e.X, e.Y))
			{
				InvokeCaptionButtonClick();
				if (m_AutoCreateCaptionMenu)
				{
					ToggleCaptionMenu();
					PaintCaptionButton();
				}
			}
		}
		else if ((int)e.Button == 1048576 && !m_SystemButtons.AutoHideButtonRect.IsEmpty && m_SystemButtons.AutoHideButtonRect.Contains(m_MouseDownPt))
		{
			m_SystemButtons.MouseDownAutoHide = false;
			PaintAutoHideButton();
			if (m_SystemButtons.AutoHideButtonRect.Contains(e.X, e.Y))
			{
				m_IgnoreAnimation = false;
				AutoHide = !AutoHide;
			}
		}
		m_MoveWindow = false;
		m_SizeWindow = 0;
		if (m_ItemContainer != null)
		{
			m_ItemContainer.InternalMouseUp(e);
		}
		((Control)this).OnMouseUp(e);
	}

	private void EndDocking(bool revertLast, Point mousePos)
	{
		if (((Control)this).Capture)
		{
			((Control)this).Capture = false;
		}
		((Control)this).Cursor = Cursors.Default;
		if (Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.DockComplete();
		}
		if (m_DragDockInfo.IsEmpty())
		{
			m_MoveWindow = false;
			return;
		}
		if (m_TempTabBar != null || m_DragDockInfo.TabDockContainer != null)
		{
			DisposeDockingPreview();
			Bar bar = m_TempTabBar;
			if (bar == null)
			{
				bar = m_DragDockInfo.TabDockContainer;
			}
			RemoveTempTabBarItems();
			if (m_DragDockInfo.TabDockContainer != this)
			{
				if (m_DockTabTearOffIndex == -1)
				{
					ArrayList arrayList = new ArrayList(m_ItemContainer.SubItems.Count);
					m_ItemContainer.SubItems.CopyTo(arrayList);
					DockContainerItem dockContainerItem = null;
					Form val = ((Control)bar).FindForm();
					if (val != null)
					{
						((ContainerControl)val).ActiveControl = (Control)(object)bar;
					}
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
								dockContainerItem2.OriginalBarName = Name;
								dockContainerItem2.OriginalPosition = m_ItemContainer.SubItems.IndexOf(dockContainerItem2);
							}
							m_ItemContainer.SubItems.Remove(dockContainerItem2);
							bar.Items.Add(dockContainerItem2);
						}
					}
					bar.RecalcLayout();
					if (dockContainerItem != null)
					{
						bar.SelectedDockContainerItem = dockContainerItem;
					}
					bar.InvokeBarDockEvents();
					if (m_Owner is DotNetBarManager dotNetBarManager)
					{
						m_MoveWindow = false;
						bar = null;
						if (CustomBar)
						{
							dotNetBarManager.Bars.Remove(this);
							((Component)this).Dispose();
						}
						else
						{
							Visible = false;
						}
						if (dotNetBarManager.ParentForm != null)
						{
							dotNetBarManager.ParentForm.Activate();
						}
						return;
					}
				}
				else
				{
					Form val2 = ((Control)bar).FindForm();
					if (val2 != null)
					{
						((ContainerControl)val2).ActiveControl = (Control)(object)bar;
					}
					DockContainerItem dockContainerItem3 = m_ItemContainer.SubItems[m_DockTabTearOffIndex] as DockContainerItem;
					dockContainerItem3.Displayed = false;
					m_ItemContainer.SubItems.Remove(dockContainerItem3);
					bar.Items.Add(dockContainerItem3);
					bar.SelectedDockContainerItem = dockContainerItem3;
					bar.RecalcLayout();
					bar.InvokeBarDockEvents();
				}
			}
			m_TempTabBar = null;
			m_DockTabTearOffIndex = -1;
			m_MoveWindow = false;
			m_DragDockInfo = new DockSiteInfo();
			return;
		}
		if (m_DockTabTearOffIndex != -1)
		{
			DisposeDockingPreview();
			if (revertLast)
			{
				DockingHandler(m_LastDockSiteInfo, ((Control)this).PointToScreen(new Point(mousePos.X, mousePos.Y)));
			}
			else
			{
				DockContainerItem dockContainerItem4 = (DockContainerItem)Items[m_DockTabTearOffIndex];
				Bar bar2 = TearOffDockContainerItem(dockContainerItem4, floatBar: false);
				bar2.DockingHandler(m_DragDockInfo, ((Control)this).PointToScreen(new Point(mousePos.X, mousePos.Y)));
				Form val3 = ((Control)this).FindForm();
				if (val3 != null && dockContainerItem4 != null && dockContainerItem4.Control != null)
				{
					((ContainerControl)val3).ActiveControl = dockContainerItem4.Control;
				}
			}
			m_DragDockInfo = new DockSiteInfo();
			m_DockTabTearOffIndex = -1;
		}
		else if (!m_OldOutlineRectangle.IsEmpty || m_OutlineForm != null)
		{
			DisposeDockingPreview();
			if (revertLast)
			{
				if (LayoutType == eLayoutType.Toolbar)
				{
					DockingHandler(m_LastDockSiteInfo, ((Control)this).PointToScreen(new Point(mousePos.X, mousePos.Y)));
				}
			}
			else
			{
				DockingHandler(m_DragDockInfo, ((Control)this).PointToScreen(new Point(mousePos.X, mousePos.Y)));
			}
			m_DragDockInfo = new DockSiteInfo();
		}
		else if (revertLast)
		{
			DockingHandler(m_LastDockSiteInfo, ((Control)this).PointToScreen(new Point(mousePos.X, mousePos.Y)));
			m_DragDockInfo = new DockSiteInfo();
		}
		m_MoveWindow = false;
	}

	private void DisposeDockingPreview()
	{
		if (!m_OldOutlineRectangle.IsEmpty || m_OutlineForm != null)
		{
			if (!m_OldOutlineRectangle.IsEmpty)
			{
				NativeFunctions.DrawReversibleDesktopRect(m_OldOutlineRectangle, 3);
				m_OldOutlineRectangle = Rectangle.Empty;
			}
			if (m_OutlineForm != null)
			{
				((Control)m_OutlineForm).Visible = false;
				((Component)(object)m_OutlineForm).Dispose();
				m_OutlineForm = null;
			}
		}
	}

	internal Bar StartTabDrag()
	{
		if (m_TabDockItems.SelectedTab == null)
		{
			return null;
		}
		if (!(m_TabDockItems.SelectedTab.AttachedItem is DockContainerItem dockContainerItem))
		{
			return null;
		}
		if (!(m_Owner is DotNetBarManager))
		{
			return null;
		}
		if (VisibleItemCount == 1 && GrabHandleStyle == eGrabHandleStyle.None)
		{
			StartBarMove();
			return this;
		}
		if (!CanUndock)
		{
			m_DockTabTearOffIndex = Items.IndexOf(dockContainerItem);
			StartBarMove();
			return this;
		}
		Bar bar = TearOffDockContainerItem(dockContainerItem, floatBar: true);
		bar.InitalFloatLocation = Point.Empty;
		bar.StartBarMove();
		return bar;
	}

	internal Bar TearOffDockContainerItem(DockContainerItem item, bool floatBar)
	{
		return TearOffDockContainerItem(item, floatBar, new Point(Control.MousePosition.X - 32, Control.MousePosition.Y - 8));
	}

	internal Bar TearOffDockContainerItem(DockContainerItem item, bool floatBar, Point initialFloatLocation)
	{
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			((ContainerControl)val).ActiveControl = (Control)(object)this;
		}
		DotNetBarManager dotNetBarManager = m_Owner as DotNetBarManager;
		m_ItemContainer.SubItems.Remove(item);
		RecalcLayout();
		Bar bar = BarFunctions.CreateDuplicateDockBar(this);
		((Control)bar).Text = item.Text;
		bar.InitalFloatLocation = initialFloatLocation;
		if (dotNetBarManager.Bars.Contains(item.Name))
		{
			string name = item.Name;
			int num = 0;
			while (dotNetBarManager.Bars.Contains(name + num))
			{
				num++;
			}
			bar.Name = name + num;
		}
		else
		{
			bar.Name = item.Name;
		}
		bar.Items.Add(item);
		dotNetBarManager.Bars.Add(bar);
		if (floatBar)
		{
			bar.DockSide = eDockSide.None;
			bar.Location = initialFloatLocation;
		}
		bar.LastDockSide = DockSide;
		bar.m_LastDockSiteInfo.objDockSite = ((Control)this).Parent as DockSite;
		bar.m_LastDockSiteInfo.LastRelativeDockToBar = this;
		bar.m_LastDockSiteInfo.LastDockSiteSide = DockSide;
		bar.CustomBar = true;
		if (m_Owner is IOwnerBarSupport ownerBarSupport)
		{
			if (ownerBarSupport.ApplyDocumentBarStyle && DockSide == eDockSide.Document)
			{
				BarFunctions.RestoreAutoDocumentBarStyle(bar);
			}
			ownerBarSupport.InvokeBarTearOff(bar, new EventArgs());
		}
		return bar;
	}

	internal void StartBarMove()
	{
		m_MouseDownPt = ((Control)this).PointToClient(Control.MousePosition);
		m_MouseDownSize = Size;
		((Control)this).Cursor = Cursors.SizeAll;
		m_MoveWindow = true;
	}

	internal void InvokeUserVisibleChanged()
	{
		if (this.UserVisibleChanged != null)
		{
			this.UserVisibleChanged(this, new EventArgs());
		}
	}

	private void InvokeAutoHideChanged()
	{
		EventArgs e = new EventArgs();
		if (this.AutoHideChanged != null)
		{
			this.AutoHideChanged(this, e);
		}
		if (m_Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeAutoHideChanged(this, e);
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.AutoHideChanged));
	}

	public void CloseDockTab(DockContainerItem dockTab)
	{
		CloseDockTab(dockTab, eEventSource.Code);
	}

	public void CloseDockTab(DockContainerItem dockTab, eEventSource source)
	{
		DockTabClosingEventArgs dockTabClosingEventArgs = new DockTabClosingEventArgs(dockTab, source);
		InvokeDockTabClosing(dockTabClosingEventArgs);
		if (dockTabClosingEventArgs.Cancel)
		{
			return;
		}
		if (VisibleItemCount > 1)
		{
			if (dockTabClosingEventArgs.RemoveDockTab)
			{
				Items.Remove(dockTab);
			}
			else
			{
				BarUtilities.SetDockContainerVisible(dockTab, visible: false);
			}
		}
		else
		{
			if (dockTabClosingEventArgs.RemoveDockTab)
			{
				Items.Remove(dockTab);
			}
			CloseBar();
			if (!Visible)
			{
				dockTab.Visible = false;
			}
		}
		InvokeDockTabClosed(dockTabClosingEventArgs);
	}

	private void InvokeDockTabClosed(DockTabClosingEventArgs e)
	{
		if (this.DockTabClosed != null)
		{
			this.DockTabClosed(this, e);
		}
		if (Owner is DotNetBarManager)
		{
			((DotNetBarManager)Owner).InvokeDockTabClosed(this, e);
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.DockTabClosed, e.DockContainerItem));
	}

	internal void InvokeDockTabClosing(DockTabClosingEventArgs e)
	{
		if (this.DockTabClosing != null)
		{
			this.DockTabClosing(this, e);
		}
		if (Owner is DotNetBarManager)
		{
			((DotNetBarManager)Owner).InvokeDockTabClosing(this, e);
		}
	}

	private void InvokeBarClosing(BarClosingEventArgs e)
	{
		if (this.Closing != null)
		{
			this.Closing(this, e);
		}
		if (m_Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeBarClosing(this, e);
		}
	}

	protected virtual void OnItemClick(BaseItem item, EventArgs e)
	{
		if (this.ItemClick != null)
		{
			this.ItemClick(item, e);
		}
	}

	internal void InvokeItemClick(BaseItem item, EventArgs e)
	{
		OnItemClick(item, e);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_ItemContainer.InternalClick(Control.MouseButtons, ((Control)this).PointToClient(Control.MousePosition));
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		if (m_BarState == eBarState.Popup)
		{
			ISite site = GetSite();
			if (site != null && site.DesignMode)
			{
				ISelectionService selectionService = (ISelectionService)site.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					((IDesignerHost)site.GetService(typeof(IDesignerHost)))?.GetDesigner(selectionService.PrimarySelection as IComponent)?.DoDefaultAction();
				}
			}
		}
		if (m_GrabHandleRect.Contains(((Control)this).PointToClient(Control.MousePosition)))
		{
			if (m_BarState == eBarState.Floating)
			{
				ReDock();
			}
			else if (m_BarState == eBarState.Docked && CanUndock && ((Control)this).Parent is DockSite)
			{
				Point empty = Point.Empty;
				DockingHandler(p: (!m_FloatingRect.IsEmpty) ? m_FloatingRect.Location : Control.MousePosition, pDockInfo: new DockSiteInfo());
			}
		}
		if (!((Control)this).IsDisposed)
		{
			m_ItemContainer.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		}
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

	private void ResetHover()
	{
		NativeFunctions.TRACKMOUSEEVENT tme = default(NativeFunctions.TRACKMOUSEEVENT);
		tme.dwFlags = 1073741824u;
		tme.hwndTrack = ((Control)this).Handle;
		tme.cbSize = Marshal.SizeOf(tme);
		NativeFunctions.TrackMouseEvent(ref tme);
		tme.dwFlags |= 1u;
		NativeFunctions.TrackMouseEvent(ref tme);
	}

	public override string ToString()
	{
		return ((Control)this).Text;
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).IsHandleCreated)
		{
			m_IsVisible = ((Control)this).Visible;
		}
		if (m_BarShowIndex >= 0)
		{
			if (((Control)this).Parent is DockSite && ((Control)this).Parent.Controls.IndexOf((Control)(object)this) != m_BarShowIndex)
			{
				((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, m_BarShowIndex);
			}
			m_BarShowIndex = -1;
		}
		if (m_Owner is IOwner owner)
		{
			if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
			else if (owner.ParentForm != null && IsAnyControl((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
			{
				((ContainerControl)owner.ParentForm).ActiveControl = null;
				((Control)this).Focus();
			}
		}
		((Control)this).OnVisibleChanged(e);
		if (m_BarState == eBarState.Docked && !m_DockingInProgress && Visible)
		{
			if (LayoutType == eLayoutType.DockContainer && m_AlwaysDisplayDockTab)
			{
				RefreshDockTab(bFireEvents: false);
			}
			RecalcLayout();
		}
		if (!Visible && m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		if (!Visible && m_CustomizeMenu != null && m_CustomizeMenu.Expanded)
		{
			m_CustomizeMenu.Expanded = false;
		}
	}

	public void SuspendLayout()
	{
		m_LayoutSuspended = true;
		((Control)this).SuspendLayout();
	}

	public void ResumeLayout()
	{
		ResumeLayout(performLayout: true);
	}

	public void ResumeLayout(bool performLayout)
	{
		m_LayoutSuspended = false;
		((Control)this).ResumeLayout(true);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (((Control)this).Parent != null && !(((Control)this).Parent is FloatingContainer) && !(((Control)this).Parent is DockSite))
		{
			m_ItemContainer.SetOwner(this);
			if (!m_ParentMsgHandlerRegistered && ((Component)this).DesignMode && ((Control)this).FindForm() != null)
			{
				DotNetBarManager.RegisterOwnerParentMsgHandler(this, ((Control)this).FindForm());
				m_ParentMsgHandlerRegistered = true;
			}
			if (m_ShortcutTable.Count == 0)
			{
				foreach (BaseItem subItem in m_ItemContainer.SubItems)
				{
					((IOwner)this).AddShortcutsFromItem(subItem);
				}
			}
			if (((Control)this).Parent != null && !(((Control)this).Parent is FloatingContainer) && !(((Control)this).Parent is DockSite) && !AutoHide && !m_FilterInstalled)
			{
				MessageHandler.RegisterMessageClient(this);
				m_FilterInstalled = true;
			}
			if (!m_DockStretch)
			{
				Stretch = true;
			}
		}
		else if (m_FilterInstalled)
		{
			MessageHandler.UnregisterMessageClient(this);
			m_FilterInstalled = false;
		}
		if (((Component)this).DesignMode)
		{
			m_ItemContainer.SetDesignMode(((Component)this).DesignMode);
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (m_ThemeWindowMargins.IsEmpty)
		{
			RefreshThemeMargins();
		}
		if (((Control)this).Parent != null && !(((Control)this).Parent is FloatingContainer) && !(((Control)this).Parent is DockSite) && !AutoHide && !m_FilterInstalled)
		{
			MessageHandler.RegisterMessageClient(this);
			m_FilterInstalled = true;
		}
		RecalcSize();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DisposeThemes();
		MenuEventSupportUnhook();
		if (m_FilterInstalled)
		{
			MessageHandler.UnregisterMessageClient(this);
			m_FilterInstalled = false;
		}
		((Control)this).OnHandleDestroyed(e);
	}

	private void RefreshThemeMargins()
	{
		if (!BarFunctions.ThemedOS)
		{
			return;
		}
		ThemeWindow themeWindow = ThemeWindow;
		Graphics val = CreateGraphics();
		try
		{
			Size size = themeWindow.ThemeMinSize(val, ThemeWindowParts.SmallFrameLeft, ThemeWindowStates.FrameActive);
			m_ThemeWindowMargins.Left = size.Width;
			size = themeWindow.ThemeMinSize(val, ThemeWindowParts.SmallFrameRight, ThemeWindowStates.FrameActive);
			m_ThemeWindowMargins.Right = size.Width;
			size = themeWindow.ThemeMinSize(val, ThemeWindowParts.SmallFrameBottom, ThemeWindowStates.FrameActive);
			m_ThemeWindowMargins.Bottom = size.Height;
			size = themeWindow.ThemeTrueSize(val, ThemeWindowParts.SmallCaption, ThemeWindowStates.FrameActive);
			m_ThemeWindowMargins.Top = size.Height;
		}
		finally
		{
			val.Dispose();
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
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (MenuBar)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		if (MenuBar)
		{
			((Control)this).Refresh();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode(bool b)
	{
		m_ItemContainer.SetDesignMode(b);
	}

	internal bool GetDesignMode()
	{
		return ((Component)this).DesignMode;
	}

	void ICustomSerialization.InvokeSerializeItem(SerializeItemEventArgs e)
	{
		if (this.SerializeItem != null)
		{
			this.SerializeItem(this, e);
		}
		if (Owner != this && Owner is ICustomSerialization)
		{
			((ICustomSerialization)Owner).InvokeSerializeItem(e);
		}
	}

	void ICustomSerialization.InvokeDeserializeItem(SerializeItemEventArgs e)
	{
		if (this.DeserializeItem != null)
		{
			this.DeserializeItem(this, e);
		}
		if (Owner != this && Owner is ICustomSerialization)
		{
			((ICustomSerialization)Owner).InvokeDeserializeItem(e);
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
		XmlElement xmlElement = xmlDoc.CreateElement("bar");
		xmlDoc.AppendChild(xmlElement);
		Serialize(xmlElement);
	}

	public void LoadDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		LoadDefinition(xmlDocument);
	}

	internal void LoadDefinition(XmlDocument xmlDoc)
	{
		if (xmlDoc.FirstChild.Name != "bar")
		{
			throw new InvalidOperationException("XML Format not recognized");
		}
		m_ItemContainer.SubItems.Clear();
		Deserialize(xmlDoc.FirstChild as XmlElement);
		if (m_Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.AddShortcutsFromBar(this);
		}
		((IOwner)this).InvokeDefinitionLoaded((object)this, new EventArgs());
	}

	void IOwner.InvokeDefinitionLoaded(object sender, EventArgs e)
	{
		if (this.DefinitionLoaded != null)
		{
			this.DefinitionLoaded(sender, e);
		}
	}

	internal void Serialize(XmlElement xmlThisBar)
	{
		Serialize(xmlThisBar, bPropertiesOnly: false);
	}

	internal void Serialize(XmlElement xmlThisBar, bool bPropertiesOnly)
	{
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected I4, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected I4, but got Unknown
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		xmlThisBar.SetAttribute("name", Name);
		xmlThisBar.SetAttribute("candockleft", XmlConvert.ToString(m_CanDockLeft));
		xmlThisBar.SetAttribute("candockright", XmlConvert.ToString(m_CanDockRight));
		xmlThisBar.SetAttribute("candocktop", XmlConvert.ToString(m_CanDockTop));
		xmlThisBar.SetAttribute("candockbottom", XmlConvert.ToString(m_CanDockBottom));
		xmlThisBar.SetAttribute("candockdoc", XmlConvert.ToString(m_CanDockDocument));
		xmlThisBar.SetAttribute("candocktab", XmlConvert.ToString(m_CanDockTab));
		xmlThisBar.SetAttribute("text", ((Control)this).Text);
		xmlThisBar.SetAttribute("dockline", XmlConvert.ToString(m_DockLine));
		if (DockSide == eDockSide.Top || DockSide == eDockSide.Bottom)
		{
			xmlThisBar.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Left));
		}
		else if (DockSide == eDockSide.Left || DockSide == eDockSide.Right)
		{
			xmlThisBar.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Top));
		}
		xmlThisBar.SetAttribute("grabhandle", XmlConvert.ToString((int)m_GrabHandleStyle));
		xmlThisBar.SetAttribute("menubar", XmlConvert.ToString(m_MenuBar));
		xmlThisBar.SetAttribute("stretch", XmlConvert.ToString(m_DockStretch));
		xmlThisBar.SetAttribute("style", XmlConvert.ToString((int)m_ItemContainer.Style));
		xmlThisBar.SetAttribute("wrapdock", XmlConvert.ToString(m_WrapItemsDock));
		xmlThisBar.SetAttribute("wrapfloat", XmlConvert.ToString(m_WrapItemsFloat));
		if (m_LockDockPosition)
		{
			xmlThisBar.SetAttribute("lockdockpos", XmlConvert.ToString(m_LockDockPosition));
		}
		if (!m_CanUndock)
		{
			xmlThisBar.SetAttribute("canundock", XmlConvert.ToString(m_CanUndock));
		}
		if (m_TabNavigation)
		{
			xmlThisBar.SetAttribute("tabnav", XmlConvert.ToString(m_TabNavigation));
		}
		if (!m_ShowToolTips)
		{
			xmlThisBar.SetAttribute("tooltips", XmlConvert.ToString(m_ShowToolTips));
		}
		if (m_ItemContainer.MoreItemsOnMenu)
		{
			xmlThisBar.SetAttribute("overflowmenu", XmlConvert.ToString(m_ItemContainer.MoreItemsOnMenu));
		}
		if (m_AutoHideState)
		{
			xmlThisBar.SetAttribute("state", XmlConvert.ToString(2));
			eDockSide eDockSide2 = eDockSide.None;
			DockStyle dockSide = m_LastDockSiteInfo.DockSide;
			switch (dockSide - 1)
			{
			case 2:
				eDockSide2 = eDockSide.Left;
				break;
			case 3:
				eDockSide2 = eDockSide.Right;
				break;
			case 0:
				eDockSide2 = eDockSide.Top;
				break;
			case 1:
				eDockSide2 = eDockSide.Bottom;
				break;
			}
			xmlThisBar.SetAttribute("dockside", XmlConvert.ToString((int)eDockSide2));
		}
		else
		{
			xmlThisBar.SetAttribute("state", XmlConvert.ToString((int)m_BarState));
			xmlThisBar.SetAttribute("dockside", XmlConvert.ToString((int)DockSide));
			if (DockSide == eDockSide.None && m_Float != null)
			{
				xmlThisBar.SetAttribute("fpos", ((Form)m_Float).Location.X + "," + ((Form)m_Float).Location.Y + "," + ((Control)this).DisplayRectangle.Width + "," + ((Control)this).DisplayRectangle.Height);
			}
		}
		IOwnerBarSupport ownerBarSupport = m_Owner as IOwnerBarSupport;
		if (m_BarState == eBarState.Floating && !Visible && ownerBarSupport != null && ownerBarSupport.WereVisible.Count > 0 && ownerBarSupport.WereVisible.Contains(this))
		{
			xmlThisBar.SetAttribute("visible", XmlConvert.ToString(value: true));
		}
		else if (m_AutoHideState)
		{
			xmlThisBar.SetAttribute("visible", XmlConvert.ToString(value: true));
			xmlThisBar.SetAttribute("autohide", XmlConvert.ToString(value: true));
		}
		else
		{
			xmlThisBar.SetAttribute("visible", XmlConvert.ToString(Visible));
		}
		xmlThisBar.SetAttribute("custom", XmlConvert.ToString(m_CustomBar));
		xmlThisBar.SetAttribute("canhide", XmlConvert.ToString(m_CanHide));
		xmlThisBar.SetAttribute("imagesize", XmlConvert.ToString((int)m_ImageSize));
		xmlThisBar.SetAttribute("itemsp", XmlConvert.ToString(m_ItemContainer.ItemSpacing));
		xmlThisBar.SetAttribute("themes", XmlConvert.ToString(m_ThemeAware));
		if (!m_ItemContainer.CanCustomize)
		{
			xmlThisBar.SetAttribute("cancust", XmlConvert.ToString(m_ItemContainer.CanCustomize));
		}
		if (((Control)this).Font != null && m_CustomFont)
		{
			xmlThisBar.SetAttribute("fontname", ((Control)this).Font.Name);
			xmlThisBar.SetAttribute("fontemsize", XmlConvert.ToString(((Control)this).Font.Size));
			xmlThisBar.SetAttribute("fontstyle", XmlConvert.ToString((int)((Control)this).Font.Style));
		}
		if (!m_ItemContainer.BackgroundColor.IsEmpty)
		{
			xmlThisBar.SetAttribute("backcolor", BarFunctions.ColorToString(m_ItemContainer.BackColor));
		}
		xmlThisBar.SetAttribute("layout", XmlConvert.ToString((int)m_ItemContainer.LayoutType));
		xmlThisBar.SetAttribute("eqbutton", XmlConvert.ToString(m_ItemContainer.EqualButtonSize));
		if (m_DockedBorder != 0)
		{
			xmlThisBar.SetAttribute("dborder", XmlConvert.ToString((int)m_DockedBorder));
		}
		if (!m_AcceptDropItems)
		{
			xmlThisBar.SetAttribute("acceptdrop", XmlConvert.ToString(m_AcceptDropItems));
		}
		if (m_SingleLineColor != SystemColors.ControlDark)
		{
			xmlThisBar.SetAttribute("slcolor", BarFunctions.ColorToString(m_SingleLineColor));
		}
		if (!m_CaptionBackColor.IsEmpty)
		{
			xmlThisBar.SetAttribute("captionbc", BarFunctions.ColorToString(m_CaptionBackColor));
		}
		if (!m_CaptionForeColor.IsEmpty)
		{
			xmlThisBar.SetAttribute("captionfc", BarFunctions.ColorToString(m_CaptionForeColor));
		}
		if (m_AlwaysDisplayDockTab)
		{
			xmlThisBar.SetAttribute("showtab", XmlConvert.ToString(m_AlwaysDisplayDockTab));
		}
		if (AutoHide)
		{
			if (m_ItemContainer.MinWidth != 0)
			{
				xmlThisBar.SetAttribute("dockwidth", XmlConvert.ToString(m_ItemContainer.MinWidth));
			}
			else if (m_LastDockSiteInfo.DockedWidth != 0)
			{
				xmlThisBar.SetAttribute("dockwidth", XmlConvert.ToString(m_LastDockSiteInfo.DockedWidth));
			}
			if (m_ItemContainer.MinHeight != 0)
			{
				xmlThisBar.SetAttribute("dockheight", XmlConvert.ToString(m_ItemContainer.MinHeight));
			}
			else if (m_LastDockSiteInfo.DockedHeight != 0)
			{
				xmlThisBar.SetAttribute("dockheight", XmlConvert.ToString(m_LastDockSiteInfo.DockedHeight));
			}
		}
		else
		{
			if (m_ItemContainer.MinWidth != 0 && CanSaveMinWidth)
			{
				xmlThisBar.SetAttribute("dockwidth", XmlConvert.ToString(m_ItemContainer.MinWidth));
			}
			if (m_ItemContainer.MinHeight != 0 && CanSaveMinHeight)
			{
				xmlThisBar.SetAttribute("dockheight", XmlConvert.ToString(m_ItemContainer.MinHeight));
			}
		}
		if (m_SplitDockWidthPercent > 0)
		{
			xmlThisBar.SetAttribute("splitwidthpercent", XmlConvert.ToString(m_SplitDockWidthPercent));
		}
		if (m_SplitDockHeightPercent > 0)
		{
			xmlThisBar.SetAttribute("splitheightpercent", XmlConvert.ToString(m_SplitDockHeightPercent));
		}
		if (m_ItemContainer.PaddingBottom != 1)
		{
			xmlThisBar.SetAttribute("padbottom", XmlConvert.ToString(m_ItemContainer.PaddingBottom));
		}
		if (m_ItemContainer.PaddingLeft != 1)
		{
			xmlThisBar.SetAttribute("padleft", XmlConvert.ToString(m_ItemContainer.PaddingLeft));
		}
		if (m_ItemContainer.PaddingRight != 1)
		{
			xmlThisBar.SetAttribute("padright", XmlConvert.ToString(m_ItemContainer.PaddingRight));
		}
		if (m_ItemContainer.PaddingTop != 1)
		{
			xmlThisBar.SetAttribute("padtop", XmlConvert.ToString(m_ItemContainer.PaddingTop));
		}
		if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && SelectedDockTab >= 0)
		{
			xmlThisBar.SetAttribute("seldocktab", XmlConvert.ToString(SelectedDockTab));
		}
		if (!m_CanAutoHide)
		{
			xmlThisBar.SetAttribute("canautohide", XmlConvert.ToString(m_CanAutoHide));
		}
		if (!m_CanReorderTabs)
		{
			xmlThisBar.SetAttribute("canreordertabs", XmlConvert.ToString(m_CanReorderTabs));
		}
		if (!m_CanTearOffTabs)
		{
			xmlThisBar.SetAttribute("cantearoff", XmlConvert.ToString(m_CanTearOffTabs));
		}
		if (!m_HideFloatingInactive)
		{
			xmlThisBar.SetAttribute("hidein", XmlConvert.ToString(m_HideFloatingInactive));
		}
		if (m_DockTabAlignment != eTabStripAlignment.Bottom)
		{
			xmlThisBar.SetAttribute("tabalign", XmlConvert.ToString((int)m_DockTabAlignment));
		}
		if (m_AutoHideAnimationTime != 100)
		{
			xmlThisBar.SetAttribute("ahanim", XmlConvert.ToString(m_AutoHideAnimationTime));
		}
		if (!m_AutoCreateCaptionMenu)
		{
			xmlThisBar.SetAttribute("autocaptionmenu", XmlConvert.ToString(m_AutoCreateCaptionMenu));
		}
		if (m_AutoSyncBarCaption)
		{
			xmlThisBar.SetAttribute("autocaptionsync", XmlConvert.ToString(m_AutoSyncBarCaption));
		}
		if (!m_SaveLayoutChanges)
		{
			xmlThisBar.SetAttribute("savelayout", XmlConvert.ToString(m_SaveLayoutChanges));
		}
		if (m_ColorScheme.SchemeChanged)
		{
			XmlElement xmlElement = xmlThisBar.OwnerDocument.CreateElement("colorscheme");
			m_ColorScheme.Serialize(xmlElement);
			xmlThisBar.AppendChild(xmlElement);
		}
		if (((Control)this).BackgroundImage != null)
		{
			XmlElement xmlElement2 = xmlThisBar.OwnerDocument.CreateElement("backimage");
			xmlThisBar.AppendChild(xmlElement2);
			BarFunctions.SerializeImage(((Control)this).BackgroundImage, xmlElement2);
			int backgroundImagePosition = (int)m_BackgroundImagePosition;
			xmlElement2.SetAttribute("pos", backgroundImagePosition.ToString());
			xmlElement2.SetAttribute("alpha", m_BackgroundImageAlpha.ToString());
		}
		if (bPropertiesOnly)
		{
			return;
		}
		XmlElement xmlElement3 = xmlThisBar.OwnerDocument.CreateElement("items");
		xmlThisBar.AppendChild(xmlElement3);
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (subItem.ShouldSerialize)
			{
				XmlElement xmlElement4 = xmlThisBar.OwnerDocument.CreateElement("item");
				xmlElement3.AppendChild(xmlElement4);
				itemSerializationContext.ItemXmlElement = xmlElement4;
				subItem.Serialize(itemSerializationContext);
			}
		}
	}

	internal void Deserialize(XmlElement xmlThisBar)
	{
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		Deserialize(xmlThisBar, itemSerializationContext);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deserialize(XmlElement xmlThisBar, ItemSerializationContext context)
	{
		m_BarDefinitionLoading = true;
		m_IgnoreAnimation = true;
		try
		{
			LoadCommonProperties(xmlThisBar);
			((Control)this).BackgroundImage = null;
			foreach (XmlElement childNode in xmlThisBar.ChildNodes)
			{
				switch (childNode.Name)
				{
				case "items":
					foreach (XmlElement childNode2 in childNode.ChildNodes)
					{
						BaseItem baseItem = context.CreateItemFromXml(childNode2);
						m_ItemContainer.SubItems.Add(baseItem);
						context.ItemXmlElement = childNode2;
						baseItem.Deserialize(context);
					}
					break;
				case "colorscheme":
					m_ColorScheme.Deserialize(childNode);
					break;
				case "backimage":
					((Control)this).BackgroundImage = BarFunctions.DeserializeImage(childNode);
					m_BackgroundImagePosition = (eBackgroundImagePosition)XmlConvert.ToInt32(childNode.GetAttribute("pos"));
					m_BackgroundImageAlpha = XmlConvert.ToByte(childNode.GetAttribute("alpha"));
					break;
				}
			}
			if (XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockside")) == 0)
			{
				if (xmlThisBar.HasAttribute("fpos"))
				{
					string attribute = xmlThisBar.GetAttribute("fpos");
					string[] array = attribute.Split(new char[1] { ',' });
					if (array.Length == 4)
					{
						Rectangle floatingRect = new Rectangle(XmlConvert.ToInt32(array[0]), XmlConvert.ToInt32(array[1]), XmlConvert.ToInt32(array[2]), XmlConvert.ToInt32(array[3]));
						m_FloatingRect = floatingRect;
					}
				}
				if (!XmlConvert.ToBoolean(xmlThisBar.GetAttribute("visible")) || (xmlThisBar.HasAttribute("autohide") && XmlConvert.ToBoolean(xmlThisBar.GetAttribute("autohide"))))
				{
					m_LoadingHideFloating = true;
				}
			}
			if (xmlThisBar.HasAttribute("seldocktab"))
			{
				int num = XmlConvert.ToInt32(xmlThisBar.GetAttribute("seldocktab"));
				if (num >= 0 && num < m_ItemContainer.SubItems.Count)
				{
					foreach (BaseItem subItem in m_ItemContainer.SubItems)
					{
						subItem.Displayed = false;
					}
					m_ItemContainer.SubItems[num].Displayed = true;
				}
			}
			m_DockLine = XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockline"));
			if (m_ItemContainer.LayoutType == eLayoutType.DockContainer)
			{
				RefreshDockTab(bFireEvents: true);
			}
			DockSide = (eDockSide)XmlConvert.ToInt32(xmlThisBar.GetAttribute("dockside"));
			if (m_LoadingHideFloating)
			{
				m_LoadingHideFloating = false;
			}
			if (xmlThisBar.HasAttribute("autohide") && context._DesignerHost == null)
			{
				((Control)this).Visible = false;
				if (m_ItemContainer.MinWidth > 0)
				{
					((Control)this).Width = m_ItemContainer.MinWidth;
				}
				if (m_ItemContainer.MinHeight > 0)
				{
					((Control)this).Height = m_ItemContainer.MinHeight;
				}
				AutoHide = true;
				m_LastDockSiteInfo.DockedWidth = m_ItemContainer.MinWidth;
				m_LastDockSiteInfo.DockedHeight = m_ItemContainer.MinHeight;
			}
			else if (context._DesignerHost == null)
			{
				Visible = XmlConvert.ToBoolean(xmlThisBar.GetAttribute("visible"));
			}
			else
			{
				Visible = true;
			}
		}
		finally
		{
			m_BarDefinitionLoading = false;
		}
		m_TabsRearranged = false;
		SetupAccessibility();
	}

	private void LoadCommonProperties(XmlElement xmlBar)
	{
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Expected O, but got Unknown
		m_BarState = eBarState.Docked;
		Name = xmlBar.GetAttribute("name");
		if (xmlBar.HasAttribute("candockleft"))
		{
			m_CanDockLeft = XmlConvert.ToBoolean(xmlBar.GetAttribute("candockleft"));
		}
		if (xmlBar.HasAttribute("candockright"))
		{
			m_CanDockRight = XmlConvert.ToBoolean(xmlBar.GetAttribute("candockright"));
		}
		if (xmlBar.HasAttribute("candocktop"))
		{
			m_CanDockTop = XmlConvert.ToBoolean(xmlBar.GetAttribute("candocktop"));
		}
		if (xmlBar.HasAttribute("candockbottom"))
		{
			m_CanDockBottom = XmlConvert.ToBoolean(xmlBar.GetAttribute("candockbottom"));
		}
		if (xmlBar.HasAttribute("candockdoc"))
		{
			m_CanDockDocument = XmlConvert.ToBoolean(xmlBar.GetAttribute("candockdoc"));
		}
		if (xmlBar.HasAttribute("candocktab"))
		{
			m_CanDockTab = XmlConvert.ToBoolean(xmlBar.GetAttribute("candocktab"));
		}
		if (xmlBar.HasAttribute("text"))
		{
			((Control)this).Text = xmlBar.GetAttribute("text");
		}
		if (xmlBar.HasAttribute("dockline"))
		{
			m_DockLine = XmlConvert.ToInt32(xmlBar.GetAttribute("dockline"));
		}
		if (xmlBar.HasAttribute("dockoffset"))
		{
			m_DockOffset = XmlConvert.ToInt32(xmlBar.GetAttribute("dockoffset"));
		}
		if (xmlBar.HasAttribute("grabhandle"))
		{
			m_GrabHandleStyle = (eGrabHandleStyle)XmlConvert.ToInt32(xmlBar.GetAttribute("grabhandle"));
		}
		if (xmlBar.HasAttribute("menubar"))
		{
			m_MenuBar = XmlConvert.ToBoolean(xmlBar.GetAttribute("menubar"));
		}
		if (xmlBar.HasAttribute("stretch"))
		{
			m_DockStretch = XmlConvert.ToBoolean(xmlBar.GetAttribute("stretch"));
		}
		if (xmlBar.HasAttribute("wrapdock"))
		{
			m_WrapItemsDock = XmlConvert.ToBoolean(xmlBar.GetAttribute("wrapdock"));
		}
		if (xmlBar.HasAttribute("wrapfloat"))
		{
			m_WrapItemsFloat = XmlConvert.ToBoolean(xmlBar.GetAttribute("wrapfloat"));
		}
		if (xmlBar.HasAttribute("custom"))
		{
			m_CustomBar = XmlConvert.ToBoolean(xmlBar.GetAttribute("custom"));
		}
		if (xmlBar.HasAttribute("canhide"))
		{
			m_CanHide = XmlConvert.ToBoolean(xmlBar.GetAttribute("canhide"));
		}
		if (xmlBar.HasAttribute("imagesize"))
		{
			m_ImageSize = (eBarImageSize)XmlConvert.ToInt32(xmlBar.GetAttribute("imagesize"));
		}
		if (xmlBar.HasAttribute("itemsp"))
		{
			m_ItemContainer.ItemSpacing = XmlConvert.ToInt32(xmlBar.GetAttribute("itemsp"));
		}
		if (xmlBar.HasAttribute("backcolor"))
		{
			m_ItemContainer.BackColor = BarFunctions.ColorFromString(xmlBar.GetAttribute("backcolor"));
		}
		if (xmlBar.HasAttribute("layout"))
		{
			m_ItemContainer.LayoutType = (eLayoutType)XmlConvert.ToInt32(xmlBar.GetAttribute("layout"));
		}
		if (xmlBar.HasAttribute("eqbutton"))
		{
			m_ItemContainer.EqualButtonSize = XmlConvert.ToBoolean(xmlBar.GetAttribute("eqbutton"));
		}
		if (xmlBar.HasAttribute("dborder"))
		{
			m_DockedBorder = (eBorderType)XmlConvert.ToInt32(xmlBar.GetAttribute("dborder"));
		}
		if (xmlBar.HasAttribute("acceptdrop"))
		{
			m_AcceptDropItems = XmlConvert.ToBoolean(xmlBar.GetAttribute("acceptdrop"));
		}
		if (xmlBar.HasAttribute("slcolor"))
		{
			m_SingleLineColor = BarFunctions.ColorFromString(xmlBar.GetAttribute("slcolor"));
		}
		if (xmlBar.HasAttribute("captionbc"))
		{
			m_CaptionBackColor = BarFunctions.ColorFromString(xmlBar.GetAttribute("captionbc"));
		}
		if (xmlBar.HasAttribute("captionfc"))
		{
			m_CaptionForeColor = BarFunctions.ColorFromString(xmlBar.GetAttribute("captionfc"));
		}
		if (xmlBar.HasAttribute("dockwidth"))
		{
			m_ItemContainer.MinWidth = XmlConvert.ToInt32(xmlBar.GetAttribute("dockwidth"));
		}
		if (xmlBar.HasAttribute("dockheight"))
		{
			m_ItemContainer.MinHeight = XmlConvert.ToInt32(xmlBar.GetAttribute("dockheight"));
		}
		if (xmlBar.HasAttribute("splitwidthpercent"))
		{
			m_SplitDockWidthPercent = XmlConvert.ToInt32(xmlBar.GetAttribute("splitwidthpercent"));
		}
		else
		{
			m_SplitDockWidthPercent = 0;
		}
		if (xmlBar.HasAttribute("splitheightpercent"))
		{
			m_SplitDockHeightPercent = XmlConvert.ToInt32(xmlBar.GetAttribute("splitheightpercent"));
		}
		if (xmlBar.HasAttribute("padbottom"))
		{
			m_ItemContainer.PaddingBottom = XmlConvert.ToInt32(xmlBar.GetAttribute("padbottom"));
		}
		if (xmlBar.HasAttribute("padleft"))
		{
			m_ItemContainer.PaddingLeft = XmlConvert.ToInt32(xmlBar.GetAttribute("padleft"));
		}
		if (xmlBar.HasAttribute("padright"))
		{
			m_ItemContainer.PaddingRight = XmlConvert.ToInt32(xmlBar.GetAttribute("padright"));
		}
		if (xmlBar.HasAttribute("padtop"))
		{
			m_ItemContainer.PaddingTop = XmlConvert.ToInt32(xmlBar.GetAttribute("padtop"));
		}
		if (xmlBar.HasAttribute("lockdockpos"))
		{
			m_LockDockPosition = XmlConvert.ToBoolean(xmlBar.GetAttribute("lockdockpos"));
		}
		if (xmlBar.HasAttribute("canundock"))
		{
			m_CanUndock = XmlConvert.ToBoolean(xmlBar.GetAttribute("canundock"));
		}
		if (xmlBar.HasAttribute("canreordertabs"))
		{
			m_CanReorderTabs = XmlConvert.ToBoolean(xmlBar.GetAttribute("canreordertabs"));
		}
		if (xmlBar.HasAttribute("cantearoff"))
		{
			m_CanTearOffTabs = XmlConvert.ToBoolean(xmlBar.GetAttribute("cantearoff"));
		}
		if (xmlBar.HasAttribute("canautohide"))
		{
			m_CanAutoHide = XmlConvert.ToBoolean(xmlBar.GetAttribute("canautohide"));
		}
		if (xmlBar.HasAttribute("cancust"))
		{
			m_ItemContainer.CanCustomize = XmlConvert.ToBoolean(xmlBar.GetAttribute("cancust"));
		}
		if (xmlBar.HasAttribute("tabalign"))
		{
			m_DockTabAlignment = (eTabStripAlignment)XmlConvert.ToInt32(xmlBar.GetAttribute("tabalign"));
		}
		if (xmlBar.HasAttribute("showtab"))
		{
			m_AlwaysDisplayDockTab = XmlConvert.ToBoolean(xmlBar.GetAttribute("showtab"));
		}
		if (xmlBar.HasAttribute("hidein"))
		{
			m_HideFloatingInactive = XmlConvert.ToBoolean(xmlBar.GetAttribute("hidein"));
		}
		if (xmlBar.HasAttribute("themes"))
		{
			m_ThemeAware = XmlConvert.ToBoolean(xmlBar.GetAttribute("themes"));
			m_ItemContainer.ThemeAware = m_ThemeAware;
		}
		if (xmlBar.HasAttribute("tabnav"))
		{
			m_TabNavigation = XmlConvert.ToBoolean(xmlBar.GetAttribute("tabnav"));
		}
		if (xmlBar.HasAttribute("tooltips"))
		{
			m_ShowToolTips = XmlConvert.ToBoolean(xmlBar.GetAttribute("tooltips"));
		}
		if (xmlBar.HasAttribute("overflowmenu"))
		{
			m_ItemContainer.MoreItemsOnMenu = XmlConvert.ToBoolean(xmlBar.GetAttribute("overflowmenu"));
		}
		if (xmlBar.HasAttribute("autocaptionmenu"))
		{
			m_AutoCreateCaptionMenu = XmlConvert.ToBoolean(xmlBar.GetAttribute("autocaptionmenu"));
		}
		if (xmlBar.HasAttribute("autocaptionsync"))
		{
			AutoSyncBarCaption = XmlConvert.ToBoolean(xmlBar.GetAttribute("autocaptionsync"));
		}
		if (xmlBar.HasAttribute("style"))
		{
			string attribute = xmlBar.GetAttribute("style");
			Style = (eDotNetBarStyle)XmlConvert.ToInt32(attribute);
		}
		if (xmlBar.HasAttribute("fontname"))
		{
			string attribute2 = xmlBar.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(xmlBar.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(xmlBar.GetAttribute("fontstyle"));
			try
			{
				((Control)this).Font = new Font(attribute2, num, val);
			}
			catch (Exception)
			{
				((Control)this).Font = SystemFonts.MenuFont;
			}
			m_CustomFont = true;
		}
		if (xmlBar.HasAttribute("ahanim"))
		{
			m_AutoHideAnimationTime = XmlConvert.ToInt32(xmlBar.GetAttribute("ahanim"));
		}
		if (xmlBar.HasAttribute("savelayout"))
		{
			m_SaveLayoutChanges = XmlConvert.ToBoolean(xmlBar.GetAttribute("savelayout"));
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
		XmlElement xmlElement = xmlDoc.CreateElement("bar");
		xmlDoc.AppendChild(xmlElement);
		SerializeLayout(xmlElement);
	}

	public void LoadLayout(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		LoadLayout(xmlDocument);
	}

	internal void LoadLayout(XmlDocument xmlDoc)
	{
		if (xmlDoc.FirstChild.Name != "bar")
		{
			throw new InvalidOperationException("XML Format not recognized");
		}
		DeserializeLayout(xmlDoc.FirstChild as XmlElement);
	}

	internal void SerializeLayout(XmlElement xmlThisBar)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected I4, but got Unknown
		if (CustomBar)
		{
			Serialize(xmlThisBar, bPropertiesOnly: true);
		}
		else
		{
			xmlThisBar.SetAttribute("name", Name);
			xmlThisBar.SetAttribute("dockline", XmlConvert.ToString(m_DockLine));
			xmlThisBar.SetAttribute("layout", XmlConvert.ToString((int)LayoutType));
			if (m_DockOffset == 0 && ((Control)this).Left > 0 && (DockSide == eDockSide.Top || DockSide == eDockSide.Bottom))
			{
				xmlThisBar.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Left));
			}
			else if (m_DockOffset == 0 && ((Control)this).Top > 0 && (DockSide == eDockSide.Left || DockSide == eDockSide.Right))
			{
				xmlThisBar.SetAttribute("dockoffset", XmlConvert.ToString(((Control)this).Top));
			}
			else
			{
				xmlThisBar.SetAttribute("dockoffset", XmlConvert.ToString(m_DockOffset));
			}
			if (m_AutoHideState)
			{
				xmlThisBar.SetAttribute("state", XmlConvert.ToString(2));
				eDockSide eDockSide2 = eDockSide.None;
				DockStyle dockSide = m_LastDockSiteInfo.DockSide;
				switch (dockSide - 1)
				{
				case 2:
					eDockSide2 = eDockSide.Left;
					break;
				case 3:
					eDockSide2 = eDockSide.Right;
					break;
				case 0:
					eDockSide2 = eDockSide.Top;
					break;
				case 1:
					eDockSide2 = eDockSide.Bottom;
					break;
				}
				xmlThisBar.SetAttribute("dockside", XmlConvert.ToString((int)eDockSide2));
			}
			else
			{
				xmlThisBar.SetAttribute("state", XmlConvert.ToString((int)m_BarState));
				xmlThisBar.SetAttribute("dockside", XmlConvert.ToString((int)DockSide));
				if (DockSide == eDockSide.None && m_Float != null)
				{
					xmlThisBar.SetAttribute("fpos", ((Form)m_Float).Location.X + "," + ((Form)m_Float).Location.Y + "," + ((Control)this).DisplayRectangle.Width + "," + ((Control)this).DisplayRectangle.Height);
				}
				else if (!m_FloatingRect.IsEmpty)
				{
					xmlThisBar.SetAttribute("fpos", m_FloatingRect.X + "," + m_FloatingRect.Y + "," + m_FloatingRect.Width + "," + m_FloatingRect.Height);
				}
			}
			IOwnerBarSupport ownerBarSupport = m_Owner as IOwnerBarSupport;
			if (m_BarState == eBarState.Floating && !Visible && ownerBarSupport != null && ownerBarSupport.WereVisible.Count > 0 && ownerBarSupport.WereVisible.Contains(this))
			{
				xmlThisBar.SetAttribute("visible", XmlConvert.ToString(value: true));
			}
			else if (m_AutoHideState)
			{
				xmlThisBar.SetAttribute("visible", XmlConvert.ToString(value: true));
				xmlThisBar.SetAttribute("autohide", XmlConvert.ToString(value: true));
			}
			else
			{
				xmlThisBar.SetAttribute("visible", XmlConvert.ToString(Visible));
			}
			if (m_ItemContainer.MinWidth != 0)
			{
				xmlThisBar.SetAttribute("dockwidth", XmlConvert.ToString(m_ItemContainer.MinWidth));
			}
			if (m_ItemContainer.MinHeight != 0)
			{
				xmlThisBar.SetAttribute("dockheight", XmlConvert.ToString(m_ItemContainer.MinHeight));
			}
			if (m_SplitDockWidthPercent > 0)
			{
				xmlThisBar.SetAttribute("splitwidthpercent", XmlConvert.ToString(m_SplitDockWidthPercent));
			}
			if (m_SplitDockHeightPercent > 0)
			{
				xmlThisBar.SetAttribute("splitheightpercent", XmlConvert.ToString(m_SplitDockHeightPercent));
			}
			if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && SelectedDockTab >= 0)
			{
				xmlThisBar.SetAttribute("seldocktab", XmlConvert.ToString(SelectedDockTab));
			}
		}
		XmlElement xmlElement = xmlThisBar.OwnerDocument.CreateElement("items");
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (subItem.OriginalBarName != "" || subItem.OriginalPosition >= 0 || subItem.UserCustomized || CustomBar || m_TabsRearranged || LayoutType == eLayoutType.DockContainer)
			{
				XmlElement xmlElement2 = xmlThisBar.OwnerDocument.CreateElement("item");
				xmlElement2.SetAttribute("name", subItem.Name);
				xmlElement2.SetAttribute("origBar", subItem.OriginalBarName);
				xmlElement2.SetAttribute("origPos", XmlConvert.ToString(subItem.OriginalPosition));
				xmlElement2.SetAttribute("pos", XmlConvert.ToString(m_ItemContainer.SubItems.IndexOf(subItem)));
				if (!subItem.Visible)
				{
					xmlElement2.SetAttribute("visible", XmlConvert.ToString(subItem.Visible));
				}
				xmlElement.AppendChild(xmlElement2);
			}
		}
		if (xmlElement.ChildNodes.Count > 0)
		{
			xmlThisBar.AppendChild(xmlElement);
		}
	}

	internal void DeserializeLayout(XmlElement xmlBar)
	{
		m_BarDefinitionLoading = true;
		try
		{
			m_IgnoreAnimation = true;
			LoadCommonProperties(xmlBar);
			if (xmlBar.ChildNodes.Count > 0)
			{
				foreach (XmlElement childNode in xmlBar.ChildNodes)
				{
					if (childNode.Name == "items")
					{
						LoadLayoutItems(xmlBar);
						break;
					}
				}
			}
			if (xmlBar.HasAttribute("fpos"))
			{
				string attribute = xmlBar.GetAttribute("fpos");
				string[] array = attribute.Split(new char[1] { ',' });
				if (array.Length == 4)
				{
					Rectangle floatingRect = new Rectangle(XmlConvert.ToInt32(array[0]), XmlConvert.ToInt32(array[1]), XmlConvert.ToInt32(array[2]), XmlConvert.ToInt32(array[3]));
					m_FloatingRect = floatingRect;
				}
			}
			if (XmlConvert.ToInt32(xmlBar.GetAttribute("dockside")) == 0 && !XmlConvert.ToBoolean(xmlBar.GetAttribute("visible")))
			{
				m_LoadingHideFloating = true;
			}
			if (xmlBar.HasAttribute("seldocktab"))
			{
				int num = XmlConvert.ToInt32(xmlBar.GetAttribute("seldocktab"));
				if (num >= 0 && num < m_ItemContainer.SubItems.Count)
				{
					foreach (BaseItem subItem in m_ItemContainer.SubItems)
					{
						subItem.Displayed = false;
					}
					m_ItemContainer.SubItems[num].Displayed = true;
					if (m_AutoSyncBarCaption)
					{
						((Control)this).Text = m_ItemContainer.SubItems[num].Text;
					}
				}
			}
			if (m_ItemContainer.LayoutType == eLayoutType.DockContainer)
			{
				RefreshDockTab(bFireEvents: true);
			}
			if (AutoHide)
			{
				if (xmlBar.HasAttribute("autohide"))
				{
					int minWidth = m_ItemContainer.MinWidth;
					int minHeight = m_ItemContainer.MinHeight;
					m_ItemContainer.MinWidth = 0;
					m_ItemContainer.MinHeight = 0;
					AutoHide = false;
					m_ItemContainer.MinWidth = minWidth;
					m_ItemContainer.MinHeight = minHeight;
				}
				else
				{
					AutoHide = false;
				}
			}
			DockSide = (eDockSide)XmlConvert.ToInt32(xmlBar.GetAttribute("dockside"));
			if (m_LoadingHideFloating)
			{
				m_LoadingHideFloating = false;
			}
			if (xmlBar.HasAttribute("autohide"))
			{
				if (!AutoHide)
				{
					((Control)this).Visible = false;
					if (m_ItemContainer.MinWidth > 0)
					{
						((Control)this).Width = m_ItemContainer.MinWidth;
					}
					if (m_ItemContainer.MinHeight > 0)
					{
						((Control)this).Height = m_ItemContainer.MinHeight;
					}
					AutoHide = true;
					m_LastDockSiteInfo.DockedWidth = m_ItemContainer.MinWidth;
					m_LastDockSiteInfo.DockedHeight = m_ItemContainer.MinHeight;
				}
			}
			else
			{
				Visible = XmlConvert.ToBoolean(xmlBar.GetAttribute("visible"));
			}
		}
		finally
		{
			m_BarDefinitionLoading = false;
		}
	}

	private void LoadLayoutItems(XmlElement xmlBar)
	{
		bool flag = false;
		eLayoutType eLayoutType2 = eLayoutType.Toolbar;
		DotNetBarManager dotNetBarManager = null;
		if (m_Owner != null && m_Owner is DotNetBarManager)
		{
			dotNetBarManager = m_Owner as DotNetBarManager;
		}
		if (xmlBar.HasAttribute("layout"))
		{
			eLayoutType2 = (eLayoutType)XmlConvert.ToInt32(xmlBar.GetAttribute("layout"));
		}
		if (eLayoutType2 == eLayoutType.DockContainer)
		{
			m_TabsRearranged = true;
		}
		if (xmlBar.HasAttribute("custom"))
		{
			flag = XmlConvert.ToBoolean(xmlBar.GetAttribute("custom"));
		}
		foreach (XmlElement childNode in xmlBar.ChildNodes)
		{
			string name = childNode.Name;
			if (!(name == "items"))
			{
				continue;
			}
			for (int i = 0; i < childNode.ChildNodes.Count; i++)
			{
				XmlElement xmlElement2 = childNode.ChildNodes[i] as XmlElement;
				string attribute = xmlElement2.GetAttribute("name");
				string attribute2 = xmlElement2.GetAttribute("origBar");
				int originalPosition = XmlConvert.ToInt32(xmlElement2.GetAttribute("origPos"));
				int num = XmlConvert.ToInt32(xmlElement2.GetAttribute("pos"));
				bool flag2 = true;
				if (xmlElement2.HasAttribute("visible"))
				{
					flag2 = XmlConvert.ToBoolean(xmlElement2.GetAttribute("visible"));
				}
				if (eLayoutType2 == eLayoutType.DockContainer)
				{
					if (Items.Contains(attribute))
					{
						BaseItem baseItem = Items[attribute];
						if (baseItem.Visible != flag2)
						{
							baseItem.Visible = flag2;
						}
						if (Items.IndexOf(Items[attribute]) != num)
						{
							Items.Remove(baseItem);
							if (Items.Count > num)
							{
								Items.Insert(num, baseItem);
							}
							else
							{
								Items.Add(baseItem);
							}
						}
					}
					else
					{
						if (dotNetBarManager == null)
						{
							continue;
						}
						BaseItem item = dotNetBarManager.GetItem(attribute);
						if (item == null || !(item is DockContainerItem))
						{
							continue;
						}
						Bar bar = item.ContainerControl as Bar;
						item.Parent.SubItems.Remove(item);
						if (item.Visible != flag2)
						{
							item.Visible = flag2;
						}
						if (Items.Count > num)
						{
							Items.Insert(num, item);
						}
						else
						{
							Items.Add(item);
						}
						if (bar != null && bar.Items.Count == 0)
						{
							if (bar.AutoHide)
							{
								bar.AutoHide = false;
							}
							bar.Visible = false;
						}
						else if (bar != null && bar.VisibleItemCount == 0)
						{
							bar.Visible = false;
						}
					}
					continue;
				}
				BaseItem baseItem2 = null;
				if (Items.Contains(attribute))
				{
					baseItem2 = Items[attribute];
					if (Items.IndexOf(attribute) != num)
					{
						Items.Remove(baseItem2);
						if (Items.Count > num)
						{
							Items.Insert(num, baseItem2);
						}
						else
						{
							Items.Add(baseItem2);
						}
					}
				}
				else
				{
					if (attribute2 != "" && dotNetBarManager != null && dotNetBarManager.Bars.Contains(attribute2) && dotNetBarManager.Bars[attribute2].Items.Contains(attribute))
					{
						baseItem2 = dotNetBarManager.Bars[attribute2].Items[attribute];
					}
					if (baseItem2 == null)
					{
						baseItem2 = dotNetBarManager.GetItem(attribute, FullSearch: true);
					}
					if (baseItem2 != null)
					{
						if (baseItem2.ContainerControl == null || (attribute2 == "" && !(baseItem2.ContainerControl is Bar)))
						{
							baseItem2 = baseItem2.Copy();
						}
						else
						{
							baseItem2.Parent.SubItems.Remove(baseItem2);
						}
						if (Items.Count > num)
						{
							Items.Insert(num, baseItem2);
						}
						else
						{
							Items.Add(baseItem2);
						}
					}
				}
				if (baseItem2 != null)
				{
					if (baseItem2.Visible != flag2)
					{
						baseItem2.Visible = flag2;
					}
					baseItem2.OriginalBarName = attribute2;
					baseItem2.OriginalPosition = originalPosition;
					baseItem2.UserCustomized = true;
				}
			}
		}
	}

	[Obsolete("Method is obsolete. Use DocumentDockUIManager.SetBarWidth or DocumentDockUIManager.SetBarHeight instead. You can obtain DocumentDockUIManager using DockSite.GetDocumentUIManager method.")]
	public void SetSize(int width, int height)
	{
		if (m_BarState == eBarState.Docked)
		{
			m_ItemContainer.MinHeight = height;
			m_ItemContainer.MinWidth = width;
			SyncLineMinWidth();
			SyncLineMinHeight();
		}
		else
		{
			Size = new Size(width, height);
		}
		if (((Control)this).IsHandleCreated)
		{
			RecalcLayout();
		}
	}

	[Obsolete("Method is obsolete. Use DocumentDockUIManager.SetBarWidth or DocumentDockUIManager.SetBarHeight instead. You can obtain DocumentDockUIManager using DockSite.GetDocumentUIManager method.")]
	public void SetSize(Size size)
	{
		SetSize(size.Width, size.Height);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackgroundImageAlpha()
	{
		return m_BackgroundImageAlpha != byte.MaxValue;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFont()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).Font != null)
		{
			if (((Control)this).Font.Name != SystemInformation.MenuFont.Name || ((Control)this).Font.Size != SystemInformation.MenuFont.Size || ((Control)this).Font.Style != SystemInformation.MenuFont.Style)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFont()
	{
		if (LayoutType == eLayoutType.DockContainer)
		{
			((Control)this).ResetFont();
			return;
		}
		_003F val = this;
		object obj = SystemInformation.MenuFont.Clone();
		((Control)val).Font = (Font)((obj is Font) ? obj : null);
	}

	internal void SetBarState(eBarState state)
	{
		m_BarState = state;
		if (m_BarState != 0 && m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
	}

	internal ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(Style) && GetRenderer() is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		if (m_Owner is DotNetBarManager && ((DotNetBarManager)m_Owner).UseGlobalColorScheme)
		{
			return ((DotNetBarManager)m_Owner).ColorScheme;
		}
		return m_ColorScheme;
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
		((Control)this).Invalidate();
	}

	internal void ClearMDIChildSystemItems(bool bRecalcLayout)
	{
		if (m_ItemContainer == null)
		{
			return;
		}
		try
		{
			if (m_ItemContainer.SubItems.Contains("dotnetbarsysiconitem"))
			{
				m_ItemContainer.SubItems.Remove("dotnetbarsysiconitem");
			}
			if (m_ItemContainer.SubItems.Contains("dotnetbarsysmenuitem"))
			{
				m_ItemContainer.SubItems.Remove("dotnetbarsysmenuitem");
			}
			if (bRecalcLayout)
			{
				RecalcLayout();
			}
		}
		catch (Exception)
		{
		}
	}

	internal void ShowMDIChildSystemItems(Form objMdiChild, bool bRecalcLayout)
	{
		ClearMDIChildSystemItems(bRecalcLayout);
		if (objMdiChild != null)
		{
			MDISystemItem mDISystemItem = new MDISystemItem("dotnetbarsysiconitem");
			mDISystemItem.Icon = objMdiChild.Icon;
			if (!objMdiChild.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!objMdiChild.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			mDISystemItem.Click += MDISysItemClick;
			mDISystemItem.IsSystemIcon = true;
			m_ItemContainer.SubItems.Add(mDISystemItem, 0);
			mDISystemItem = new MDISystemItem("dotnetbarsysmenuitem");
			if (!objMdiChild.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!objMdiChild.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			mDISystemItem.ItemAlignment = eItemAlignment.Far;
			mDISystemItem.Click += MDISysItemClick;
			m_ItemContainer.SubItems.Add(mDISystemItem);
			if (bRecalcLayout)
			{
				RecalcLayout();
			}
		}
	}

	private void MDISysItemClick(object sender, EventArgs e)
	{
		MDISystemItem mDISystemItem = sender as MDISystemItem;
		IOwner owner = m_Owner as IOwner;
		Form val = null;
		if (owner != null)
		{
			val = owner.ActiveMdiChild;
		}
		if (val == null)
		{
			ClearMDIChildSystemItems(bRecalcLayout: true);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Minimize)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, new IntPtr(61472), IntPtr.Zero);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Restore)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, new IntPtr(61728), IntPtr.Zero);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Close)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, new IntPtr(61536), IntPtr.Zero);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.NextWindow)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, new IntPtr(61504), IntPtr.Zero);
		}
	}

	public void SetStyle()
	{
		ColorScheme.SetStyle();
	}

	internal bool IsPositionOnDockTab(int x, int y)
	{
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		if (m_GrabHandleRect.Contains(pt) || (m_TabDockItems != null && ((Control)m_TabDockItems).Bounds.Contains(pt)))
		{
			return true;
		}
		return false;
	}

	internal void AppLostFocus()
	{
		if (m_ItemContainer != null)
		{
			m_ItemContainer.ContainerLostFocus(appLostFocus: true);
		}
		if (!((Control)this).IsDisposed)
		{
			((Control)this).AccessibilityNotifyClients((AccessibleEvents)32778, 0);
		}
	}

	internal void AppActivate()
	{
		if (!((Control)this).IsDisposed)
		{
			((Control)this).AccessibilityNotifyClients((AccessibleEvents)32778, 0);
		}
	}

	internal void HideBar()
	{
		if (m_DesignerParent && ((Control)this).Parent != null)
		{
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			m_DesignerParent = false;
		}
		if (m_BarState == eBarState.Floating)
		{
			if (m_Float != null)
			{
				((Control)m_Float).Hide();
				((Control)this).OnVisibleChanged(new EventArgs());
			}
			return;
		}
		((Control)this).Hide();
		if (!m_DockingInProgress && !m_LayoutSuspended && ((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
		{
			((DockSite)(object)((Control)this).Parent).GetDocumentUIManager().AdjustContainerSize(this, visibleChanged: true);
		}
		if (m_Owner == null || !(m_Owner is DotNetBarManager) || !((DotNetBarManager)m_Owner).IsDisposed)
		{
			RecalcLayout();
		}
	}

	private bool AddtoDesignTimeContainer()
	{
		ISite site = GetSite();
		if (site == null)
		{
			return false;
		}
		if (!(site.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost))
		{
			return false;
		}
		IComponent rootComponent = designerHost.RootComponent;
		Control val = (Control)((rootComponent is Control) ? rootComponent : null);
		while (val != null)
		{
			val = val.Parent;
			if (val != null && ((object)val).GetType().Name.IndexOf("DesignerFrame") >= 0)
			{
				break;
			}
		}
		if (val == null || val.Parent == null)
		{
			return false;
		}
		Point location = val.PointToClient(Location);
		val.Controls.Add((Control)(object)this);
		Location = location;
		((Control)this).Show();
		((Control)this).Update();
		((Control)this).BringToFront();
		m_DesignerParent = true;
		return true;
	}

	internal void ShowBar()
	{
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Invalid comparison between Unknown and I4
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Invalid comparison between Unknown and I4
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Invalid comparison between Unknown and I4
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Invalid comparison between Unknown and I4
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Invalid comparison between Unknown and I4
		if (PassiveBar)
		{
			((Control)this).Show();
			((Control)this).Update();
			return;
		}
		if (m_BarState == eBarState.Floating)
		{
			if (m_Float == null)
			{
				m_Float = new FloatingContainer(this);
				((Control)m_Float).CreateControl();
				if (((Control)this).Parent != null)
				{
					if (((Control)this).Parent is DockSite)
					{
						((DockSite)(object)((Control)this).Parent).RemoveBar((Control)(object)this);
					}
					else
					{
						((Control)this).Parent.Controls.Remove((Control)(object)this);
					}
				}
				((Control)this).Parent = null;
				((Control)m_Float).Controls.Add((Control)(object)this);
				((Control)this).Location = new Point(0, 0);
			}
			if (m_Float != null)
			{
				((Form)m_Float).TopLevel = true;
				((Control)m_Float).Visible = true;
				if (!((Control)this).Visible)
				{
					((Control)this).Visible = true;
				}
				((Control)m_Float).Refresh();
			}
		}
		else if (m_BarState == eBarState.Popup)
		{
			if (m_ParentItem != null && ((m_ParentItem.Site != null && m_ParentItem.Site.DesignMode) || (m_ParentItem.Parent != null && m_ParentItem.Parent.Site != null && m_ParentItem.Parent.Site.DesignMode)) && AddtoDesignTimeContainer())
			{
				return;
			}
			ePopupAnimation ePopupAnimation2 = m_PopupAnimation;
			if (!BarFunctions.SupportsAnimation)
			{
				ePopupAnimation2 = ePopupAnimation.None;
			}
			else
			{
				IOwnerMenuSupport ownerMenuSupport = m_Owner as IOwnerMenuSupport;
				if (ePopupAnimation2 == ePopupAnimation.ManagerControlled)
				{
					if (ownerMenuSupport != null)
					{
						ePopupAnimation2 = ownerMenuSupport.PopupAnimation;
					}
					if (ePopupAnimation2 == ePopupAnimation.ManagerControlled)
					{
						ePopupAnimation2 = ePopupAnimation.SystemDefault;
					}
				}
				switch (ePopupAnimation2)
				{
				case ePopupAnimation.SystemDefault:
					ePopupAnimation2 = NativeFunctions.SystemMenuAnimation;
					break;
				case ePopupAnimation.Random:
				{
					Random random = new Random();
					int num = random.Next(2);
					ePopupAnimation2 = ePopupAnimation.Fade;
					switch (num)
					{
					case 1:
						ePopupAnimation2 = ePopupAnimation.Slide;
						break;
					case 2:
						ePopupAnimation2 = ePopupAnimation.Unfold;
						break;
					}
					break;
				}
				}
			}
			if (BarFunctions.IsOffice2007Style(Style) && BarType == eBarType.Toolbar)
			{
				SetRoundRegion((Control)(object)this);
			}
			if (ePopupAnimation2 == ePopupAnimation.Fade && Environment.OSVersion.Version.Major >= 5)
			{
				NativeFunctions.AnimateWindow(((Control)this).Handle, 100, 524288);
			}
			else
			{
				switch (ePopupAnimation2)
				{
				case ePopupAnimation.Slide:
					NativeFunctions.AnimateWindow(((Control)this).Handle, 100, 262149);
					break;
				case ePopupAnimation.Unfold:
					NativeFunctions.AnimateWindow(((Control)this).Handle, 100, 5);
					break;
				default:
					((Control)this).Show();
					break;
				}
			}
			if (ePopupAnimation2 != 0 && ((ArrangedElementCollection)((Control)this).Controls).Count > 0)
			{
				((Control)this).Refresh();
			}
			if (DisplayShadow && AlphaShadow)
			{
				if (m_DropShadow != null)
				{
					((Control)m_DropShadow).Hide();
					((Component)(object)m_DropShadow).Dispose();
				}
				m_DropShadow = new PopupShadow(bAlphaShadow: true);
				((Control)m_DropShadow).CreateControl();
				NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
				m_DropShadow.UpdateShadow();
			}
		}
		else
		{
			if (((Control)this).Parent is DockSite)
			{
				m_BarShowIndex = ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
			}
			else
			{
				m_BarShowIndex = -1;
			}
			if (!m_DockingInProgress && !m_LayoutSuspended && ((Control)this).Parent is DockSite && (int)((Control)this).Parent.Dock != 5 && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
			{
				DockSite dockSite = ((Control)this).Parent as DockSite;
				m_LayoutSuspended = true;
				((Control)this).Show();
				m_LayoutSuspended = false;
				dockSite.GetDocumentUIManager().AdjustContainerSize(this, visibleChanged: true);
				if (!m_BarDefinitionLoading)
				{
					DocumentBaseContainer documentFromBar = dockSite.GetDocumentUIManager().GetDocumentFromBar(this);
					if (documentFromBar != null && documentFromBar.Parent is DocumentDockContainer)
					{
						DocumentDockContainer documentDockContainer = documentFromBar.Parent as DocumentDockContainer;
						int visibleCount = documentDockContainer.Documents.VisibleCount;
						if (visibleCount > 1)
						{
							if (documentDockContainer.Orientation == eOrientation.Horizontal && ((int)((Control)dockSite).Dock == 1 || (int)((Control)dockSite).Dock == 2))
							{
								documentFromBar.SetLayoutBounds(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, documentDockContainer.LayoutBounds.Width / visibleCount, documentFromBar.LayoutBounds.Height));
							}
							else if (documentDockContainer.Orientation == eOrientation.Vertical && ((int)((Control)dockSite).Dock == 3 || (int)((Control)dockSite).Dock == 4))
							{
								documentFromBar.SetLayoutBounds(new Rectangle(documentFromBar.LayoutBounds.X, documentFromBar.LayoutBounds.Y, documentFromBar.LayoutBounds.Width, documentDockContainer.LayoutBounds.Height / visibleCount));
							}
						}
					}
				}
				if (Owner is DotNetBarManager { SuspendLayout: not false })
				{
					dockSite.NeedsLayout = true;
				}
			}
			else
			{
				((Control)this).Show();
			}
			m_BarShowIndex = -1;
		}
		((Control)this).Update();
	}

	internal void DockContainerItemCanCloseChanged(DockContainerItem item, eDockContainerClose oldValue, eDockContainerClose newValue)
	{
		if (SelectedDockContainerItem == item)
		{
			UpdateCloseButtonVisibility();
		}
	}

	private void UpdateCloseButtonVisibility()
	{
		bool canHideResolved = CanHideResolved;
		if (m_BarState == eBarState.Floating || (m_BarState == eBarState.Docked && (m_GrabHandleStyle == eGrabHandleStyle.Caption || m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane || m_GrabHandleStyle == eGrabHandleStyle.CaptionDotted)))
		{
			((Control)this).Invalidate(m_GrabHandleRect);
		}
		if (m_TabDockItems != null && m_TabDockItems._TabSystemBox != null)
		{
			m_TabDockItems._TabSystemBox.CloseVisible = canHideResolved;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return m_SingleLineColor != SystemColors.ControlDark;
	}

	private BaseItem GetFirstVisibleItem()
	{
		foreach (BaseItem item in Items)
		{
			if (item.Visible)
			{
				return item;
			}
		}
		return null;
	}

	public void Show()
	{
		Visible = true;
	}

	public void Hide()
	{
		if (AutoHide)
		{
			AutoHide = false;
		}
		Visible = false;
		RecalcLayout();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void ResetBackColor()
	{
		if (m_ItemContainer != null)
		{
			m_ItemContainer.BackColor = Color.Empty;
		}
		else
		{
			((Control)this).BackColor = SystemColors.Control;
		}
	}

	public bool ShouldSerializeBackColor()
	{
		if (m_ItemContainer != null && !m_ItemContainer.BackgroundColor.IsEmpty)
		{
			return true;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSize()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (LayoutType == eLayoutType.DockContainer && ((Control)this).Parent is DockSite && (int)((Control)this).Parent.Dock == 5)
		{
			return false;
		}
		return true;
	}

	public BaseItem GetItem(string name)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(m_ItemContainer, name);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		if (m_GlobalParentComponent != null)
		{
			if (m_GlobalParentComponent is RibbonControl)
			{
				return ((RibbonControl)(object)m_GlobalParentComponent).RibbonStrip.GetItem(name);
			}
			if (m_GlobalParentComponent is DotNetBarManager)
			{
				return ((DotNetBarManager)m_GlobalParentComponent).GetItem(name);
			}
		}
		return null;
	}

	public ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		if (m_GlobalParentComponent != null)
		{
			if (m_GlobalParentComponent is RibbonControl)
			{
				arrayList.AddRange(((RibbonControl)(object)m_GlobalParentComponent).RibbonStrip.GetItems(ItemName));
			}
			else if (m_GlobalParentComponent is DotNetBarManager)
			{
				arrayList.AddRange(((DotNetBarManager)m_GlobalParentComponent).GetItems(ItemName));
			}
		}
		else
		{
			BarFunctions.GetSubItemsByName(m_ItemContainer, ItemName, arrayList);
		}
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		if (m_GlobalParentComponent != null)
		{
			if (m_GlobalParentComponent is RibbonControl)
			{
				arrayList.AddRange(((RibbonControl)(object)m_GlobalParentComponent).RibbonStrip.GetItems(ItemName, itemType));
			}
			else if (m_GlobalParentComponent is DotNetBarManager)
			{
				arrayList.AddRange(((DotNetBarManager)m_GlobalParentComponent).GetItems(ItemName, itemType));
			}
		}
		else
		{
			BarFunctions.GetSubItemsByNameAndType(m_ItemContainer, ItemName, arrayList, itemType);
		}
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		if (m_GlobalParentComponent != null)
		{
			if (m_GlobalParentComponent is RibbonControl)
			{
				arrayList.AddRange(((RibbonControl)(object)m_GlobalParentComponent).RibbonStrip.GetItems(ItemName, itemType, useGlobalName));
			}
			else if (m_GlobalParentComponent is DotNetBarManager)
			{
				arrayList.AddRange(((DotNetBarManager)m_GlobalParentComponent).GetItems(ItemName, itemType, fullSearch: false, useGlobalName));
			}
		}
		else
		{
			BarFunctions.GetSubItemsByNameAndType(m_ItemContainer, ItemName, arrayList, itemType, useGlobalName);
		}
		return arrayList;
	}

	public AutoHidePanel GetAutoHidePanel()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (!AutoHide)
		{
			return null;
		}
		return GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
	}

	private AutoHidePanel GetAutoHidePanel(DockStyle dock)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected I4, but got Unknown
		if (!(m_Owner is IOwnerAutoHideSupport ownerAutoHideSupport))
		{
			return null;
		}
		AutoHidePanel autoHidePanel = null;
		DockStyle dockSide = m_LastDockSiteInfo.DockSide;
		autoHidePanel = (dockSide - 1) switch
		{
			2 => ownerAutoHideSupport.LeftAutoHidePanel, 
			3 => ownerAutoHideSupport.RightAutoHidePanel, 
			0 => ownerAutoHideSupport.TopAutoHidePanel, 
			_ => ownerAutoHideSupport.BottomAutoHidePanel, 
		};
		if (AntiAlias)
		{
			autoHidePanel.AntiAlias = true;
		}
		autoHidePanel.Style = Style;
		return autoHidePanel;
	}

	private Size GetLargestMinSize()
	{
		Size empty = Size.Empty;
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (subItem is DockContainerItem)
			{
				DockContainerItem dockContainerItem = subItem as DockContainerItem;
				if (dockContainerItem.MinimumSize.Width > empty.Width)
				{
					empty.Width = dockContainerItem.MinimumSize.Width;
				}
				if (dockContainerItem.MinimumSize.Height > empty.Height)
				{
					empty.Height = dockContainerItem.MinimumSize.Height;
				}
			}
		}
		return empty;
	}

	private Control GetOwnerControl()
	{
		Control result = null;
		IOwner owner = m_Owner as IOwner;
		if (owner is DotNetBarManager && ((DotNetBarManager)owner).TopDockSite != null)
		{
			result = ((Control)((DotNetBarManager)owner).TopDockSite).Parent;
		}
		else if (owner != null)
		{
			result = (Control)(object)owner.ParentForm;
		}
		return result;
	}

	private void AutoHideStateChanged()
	{
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Invalid comparison between Unknown and I4
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Invalid comparison between Unknown and I4
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Invalid comparison between Unknown and I4
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Invalid comparison between Unknown and I4
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Invalid comparison between Unknown and I4
		IOwner owner = m_Owner as IOwner;
		IOwnerAutoHideSupport ownerAutoHideSupport = m_Owner as IOwnerAutoHideSupport;
		if (owner == null)
		{
			return;
		}
		Control ownerControl = GetOwnerControl();
		if (ownerControl == null)
		{
			return;
		}
		if (m_AutoHideState)
		{
			if (m_BarState == eBarState.AutoHide)
			{
				return;
			}
			if (m_TabDockItems != null)
			{
				((Control)m_TabDockItems).Visible = false;
			}
			DockSiteInfo lastDockSiteInfo = m_LastDockSiteInfo;
			m_LastDockSiteInfo = new DockSiteInfo();
			m_LastDockSiteInfo.LastRelativeDocumentId = lastDockSiteInfo.LastRelativeDocumentId;
			m_LastDockSiteInfo.LastRelativeDockToBar = lastDockSiteInfo.LastRelativeDockToBar;
			if (!m_BarDefinitionLoading)
			{
				if (((Control)this).Height > 0)
				{
					m_LastDockSiteInfo.DockedHeight = ((Control)this).Height;
				}
				else if (m_ItemContainer != null && m_ItemContainer.MinHeight > 0)
				{
					m_LastDockSiteInfo.DockedHeight = m_ItemContainer.MinHeight + 22;
				}
				if (((Control)this).Width > 0)
				{
					m_LastDockSiteInfo.DockedWidth = ((Control)this).Width;
				}
				else if (m_ItemContainer != null && m_ItemContainer.MinWidth > 0)
				{
					m_LastDockSiteInfo.DockedWidth = m_ItemContainer.MinWidth + 22;
				}
				Size largestMinSize = GetLargestMinSize();
				if (largestMinSize.Width > m_LastDockSiteInfo.DockedWidth)
				{
					m_LastDockSiteInfo.DockedWidth = 0;
				}
				if (largestMinSize.Height > m_LastDockSiteInfo.DockedHeight)
				{
					m_LastDockSiteInfo.DockedHeight = 0;
				}
			}
			m_LastDockSiteInfo.DockLine = DockLine;
			m_LastDockSiteInfo.DockOffset = DockOffset;
			if (((Control)this).Parent != null && m_BarState == eBarState.Docked)
			{
				m_LastDockSiteInfo.DockSide = ((Control)this).Parent.Dock;
			}
			else
			{
				m_LastDockSiteInfo.DockSide = (DockStyle)3;
			}
			if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
			{
				m_LastDockSiteInfo.InsertPosition = ((Control)(DockSite)(object)((Control)this).Parent).Controls.GetChildIndex((Control)(object)this);
				m_LastDockSiteInfo.objDockSite = (DockSite)(object)DockedSite;
			}
			m_BarState = eBarState.AutoHide;
			ResetActiveControl();
			ownerControl.SuspendLayout();
			try
			{
				((Control)this).Visible = false;
				if (((Control)this).Parent != null && ((Control)this).Parent is DockSite)
				{
					DockSite dockSite = ((Control)this).Parent as DockSite;
					if (dockSite.DocumentDockContainer != null)
					{
						if ((int)((Control)dockSite).Dock != 5 && dockSite.GetDocumentUIManager().GetDocumentFromBar(this) is DocumentBarContainer documentBarContainer && documentBarContainer.Parent is DocumentDockContainer)
						{
							DocumentDockContainer documentDockContainer = documentBarContainer.Parent as DocumentDockContainer;
							if ((documentDockContainer.Orientation == eOrientation.Horizontal && ((int)((Control)dockSite).Dock == 1 || (int)((Control)dockSite).Dock == 2)) || (documentDockContainer.Orientation == eOrientation.Vertical && ((int)((Control)dockSite).Dock == 3 || (int)((Control)dockSite).Dock == 4) && documentDockContainer.Documents.Count > 1))
							{
								for (int i = 0; i < documentDockContainer.Documents.Count; i++)
								{
									if (documentDockContainer.Documents[i] is DocumentBarContainer && documentDockContainer.Documents[i].Visible && documentDockContainer.Documents[i] != documentBarContainer)
									{
										m_LastDockSiteInfo.MouseOverBar = ((DocumentBarContainer)documentDockContainer.Documents[i]).Bar;
										if (i < documentDockContainer.Documents.IndexOf(documentBarContainer))
										{
											m_LastDockSiteInfo.MouseOverDockSide = ((documentDockContainer.Orientation == eOrientation.Horizontal) ? eDockSide.Left : eDockSide.Bottom);
										}
										else
										{
											m_LastDockSiteInfo.MouseOverDockSide = ((documentDockContainer.Orientation == eOrientation.Horizontal) ? eDockSide.Right : eDockSide.Top);
										}
										break;
									}
								}
							}
						}
						dockSite.GetDocumentUIManager().UnDock(this);
					}
					else
					{
						((DockSite)(object)((Control)this).Parent).RemoveBar((Control)(object)this);
					}
				}
				((Control)this).Parent = null;
				ownerControl.Controls.Add((Control)(object)this);
				ownerControl.Controls.SetChildIndex((Control)(object)this, 0);
				AutoHidePanel autoHidePanel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
				autoHidePanel.AddBar(this);
				if (!m_BarDefinitionLoading)
				{
					SetAutoHideSize();
				}
				if (!m_IgnoreAnimation && m_AutoHideAnimationTime != 0)
				{
					Visible = true;
				}
			}
			finally
			{
				ownerControl.ResumeLayout();
			}
			if (!m_BarDefinitionLoading)
			{
				((Control)this).Update();
				AnimateHide();
			}
			return;
		}
		AutoHidePanel autoHidePanel2 = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
		autoHidePanel2.RemoveBar(this);
		if (m_LastDockSiteInfo.objDockSite != null)
		{
			if (m_LastDockSiteInfo.MouseOverBar != null && (m_LastDockSiteInfo.objDockSite.GetDocumentUIManager() == null || m_LastDockSiteInfo.objDockSite.GetDocumentUIManager().GetDocumentFromBar(m_LastDockSiteInfo.MouseOverBar) == null))
			{
				m_LastDockSiteInfo.MouseOverBar = null;
				m_LastDockSiteInfo.MouseOverDockSide = eDockSide.None;
				m_LastDockSiteInfo.NewLine = true;
			}
		}
		else
		{
			m_LastDockSiteInfo.MouseOverBar = null;
			m_LastDockSiteInfo.MouseOverDockSide = eDockSide.None;
			m_LastDockSiteInfo.NewLine = true;
		}
		DockingHandler(m_LastDockSiteInfo, Point.Empty);
		if (!IsVisible)
		{
			Visible = true;
		}
		RefreshDockTab(bFireEvents: true);
		ResizeDockTab();
		m_ItemContainer.MinHeight = 0;
		m_ItemContainer.MinWidth = 0;
	}

	internal void SetAutoHideSize()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected I4, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected I4, but got Unknown
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Invalid comparison between Unknown and I4
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Invalid comparison between Unknown and I4
		IOwner owner = m_Owner as IOwner;
		IOwnerBarSupport ownerBarSupport = m_Owner as IOwnerBarSupport;
		IOwnerAutoHideSupport ownerAutoHideSupport = m_Owner as IOwnerAutoHideSupport;
		Control ownerControl = GetOwnerControl();
		if (owner == null || ownerControl == null || ownerBarSupport == null || ownerAutoHideSupport == null)
		{
			return;
		}
		int width = ownerControl.ClientSize.Width;
		int height = ownerControl.ClientSize.Height;
		int num = 0;
		int num2 = 0;
		DockStyle dockSide = m_LastDockSiteInfo.DockSide;
		switch (dockSide - 1)
		{
		case 2:
		{
			width = ((Control)ownerBarSupport.LeftDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.LeftDockSite).Parent.Height;
			Point location4 = ((Control)ownerAutoHideSupport.LeftAutoHidePanel).Location;
			num = location4.X + ((Control)ownerAutoHideSupport.LeftAutoHidePanel).Width;
			num2 = location4.Y;
			break;
		}
		case 3:
		{
			width = ((Control)ownerBarSupport.RightDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.RightDockSite).Parent.Height;
			Point location3 = ((Control)ownerAutoHideSupport.RightAutoHidePanel).Location;
			num = location3.X;
			num2 = location3.Y;
			break;
		}
		case 0:
		{
			width = ((Control)ownerBarSupport.TopDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.TopDockSite).Parent.Height;
			Point location2 = ((Control)ownerAutoHideSupport.TopAutoHidePanel).Location;
			num = location2.X;
			num2 = location2.Y + ((Control)ownerAutoHideSupport.TopAutoHidePanel).Height;
			break;
		}
		default:
		{
			width = ((Control)ownerBarSupport.BottomDockSite).Parent.Width;
			height = ((Control)ownerBarSupport.BottomDockSite).Parent.Height;
			Point location = ((Control)ownerAutoHideSupport.BottomAutoHidePanel).Location;
			num = location.X;
			num2 = location.Y;
			break;
		}
		}
		DockStyle dockSide2 = m_LastDockSiteInfo.DockSide;
		switch (dockSide2 - 1)
		{
		case 2:
			height = ((Control)ownerAutoHideSupport.LeftAutoHidePanel).Height;
			width = m_LastDockSiteInfo.DockedWidth;
			if (width == 0)
			{
				int num6 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num6 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num6 = 0;
				}
				if (num6 >= 0)
				{
					width = Math.Max(((DockContainerItem)Items[num6]).MinimumSize.Width, ((DockContainerItem)Items[num6]).Width);
				}
				EnableRedraw = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					EnableRedraw = true;
				}
				width = ((Control)this).Width;
				m_LastDockSiteInfo.DockedWidth = width;
			}
			break;
		case 3:
			height = ((Control)ownerAutoHideSupport.RightAutoHidePanel).Height;
			width = m_LastDockSiteInfo.DockedWidth;
			if (width == 0)
			{
				int num4 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num4 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num4 = 0;
				}
				if (num4 >= 0)
				{
					width = Math.Max(((DockContainerItem)Items[num4]).MinimumSize.Width, ((DockContainerItem)Items[num4]).Width);
				}
				EnableRedraw = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					EnableRedraw = true;
				}
				width = ((Control)this).Width;
				m_LastDockSiteInfo.DockedWidth = width;
			}
			num -= width;
			break;
		case 0:
			width = ((Control)ownerAutoHideSupport.TopAutoHidePanel).Width;
			height = m_LastDockSiteInfo.DockedHeight;
			if (height == 0)
			{
				int num5 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num5 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num5 = 0;
				}
				if (num5 >= 0)
				{
					height = Math.Max(((DockContainerItem)Items[num5]).MinimumSize.Height, ((DockContainerItem)Items[num5]).Height);
				}
				EnableRedraw = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					EnableRedraw = true;
				}
				height = ((Control)this).Height;
				m_LastDockSiteInfo.DockedHeight = height;
			}
			break;
		default:
			width = ((Control)ownerAutoHideSupport.BottomAutoHidePanel).Width;
			height = m_LastDockSiteInfo.DockedHeight;
			if (height == 0)
			{
				int num3 = -1;
				if (SelectedDockTab >= 0 && Items[SelectedDockTab] is DockContainerItem)
				{
					num3 = SelectedDockTab;
				}
				else if (Items.Count > 0 && Items[0] is DockContainerItem)
				{
					num3 = 0;
				}
				if (num3 >= 0)
				{
					height = Math.Max(((DockContainerItem)Items[num3]).MinimumSize.Height, ((DockContainerItem)Items[num3]).Height);
				}
				EnableRedraw = false;
				try
				{
					Size = new Size(width, height);
					RecalcSize();
				}
				finally
				{
					EnableRedraw = true;
				}
				height = ((Control)this).Height;
				m_LastDockSiteInfo.DockedHeight = height;
				if (m_TabDockItems != null)
				{
					m_LastDockSiteInfo.DockedHeight += ((Control)m_TabDockItems).Height;
				}
			}
			num2 -= height;
			break;
		}
		Rectangle rectangle = new Rectangle(num, num2, width, height);
		AutoHideDisplayEventArgs autoHideDisplayEventArgs = new AutoHideDisplayEventArgs(rectangle);
		if (this.AutoHideDisplay != null)
		{
			this.AutoHideDisplay(this, autoHideDisplayEventArgs);
			rectangle = autoHideDisplayEventArgs.DisplayRectangle;
		}
		if (ownerBarSupport != null)
		{
			ownerBarSupport.InvokeAutoHideDisplay(this, autoHideDisplayEventArgs);
			rectangle = autoHideDisplayEventArgs.DisplayRectangle;
		}
		((Control)this).Bounds = rectangle;
		RecalcSize();
		if ((int)m_LastDockSiteInfo.DockSide == 2)
		{
			if (((Control)this).Height != height)
			{
				((Control)this).Top = ((Control)this).Top + (height - ((Control)this).Height);
			}
		}
		else if ((int)m_LastDockSiteInfo.DockSide == 4 && ((Control)this).Width != width)
		{
			((Control)this).Left = ((Control)this).Left + (width - ((Control)this).Width);
		}
	}

	internal bool AnimateShow()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected I4, but got Unknown
		if (Visible)
		{
			return false;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeAutoHideDisplayed(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return false;
		}
		SetAutoHideSize();
		Rectangle rectangle = new Rectangle(Location, Size);
		Rectangle rectStart = rectangle;
		((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, 0);
		if (!m_IgnoreAnimation && m_AutoHideAnimationTime > 0)
		{
			DockStyle dockSide = m_LastDockSiteInfo.DockSide;
			switch (dockSide - 1)
			{
			case 2:
				rectStart.Width = 1;
				break;
			case 3:
				rectStart.X = rectStart.Right - 1;
				rectStart.Width = 1;
				break;
			case 0:
				rectStart.Height = 1;
				break;
			default:
				rectStart.Y = rectStart.Bottom - 1;
				rectStart.Height = 1;
				break;
			}
		}
		try
		{
			m_AnimationInProgress = true;
			BarFunctions.AnimateControl((Control)(object)this, show: true, m_AutoHideAnimationTime, rectStart, rectangle);
		}
		finally
		{
			m_AnimationInProgress = false;
		}
		m_IgnoreAnimation = false;
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.AutoHideOpen));
		return true;
	}

	internal bool AnimateHide()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected I4, but got Unknown
		if (!Visible)
		{
			return true;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeAutoHideHidden(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return false;
		}
		((Control)this).Parent.Controls.SetChildIndex((Control)(object)this, 0);
		Rectangle rectangle = new Rectangle(Location, Size);
		Rectangle rectEnd = rectangle;
		if (!m_IgnoreAnimation && m_AutoHideAnimationTime > 0)
		{
			DockStyle dockSide = m_LastDockSiteInfo.DockSide;
			switch (dockSide - 1)
			{
			case 2:
				rectEnd.Width = 1;
				break;
			case 3:
				rectEnd.X = rectEnd.Right - 1;
				rectEnd.Width = 1;
				break;
			case 0:
				rectEnd.Height = 1;
				break;
			default:
				rectEnd.Y = rectEnd.Bottom - 1;
				rectEnd.Height = 1;
				break;
			}
			try
			{
				m_AnimationInProgress = true;
				BarFunctions.AnimateControl((Control)(object)this, show: false, m_AutoHideAnimationTime, rectangle, rectEnd);
			}
			finally
			{
				m_AnimationInProgress = false;
			}
		}
		else
		{
			Visible = false;
		}
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.AutoHideFolded));
		return true;
	}

	protected virtual void OnBeforeAutoHideDisplayed(CancelEventArgs e)
	{
		this.BeforeAutoHideDisplayed?.Invoke(this, e);
	}

	protected virtual void OnBeforeAutoHideHidden(CancelEventArgs e)
	{
		this.BeforeAutoHideHidden?.Invoke(this, e);
	}

	private void ResetActiveControl()
	{
		IOwner owner = m_Owner as IOwner;
		if (owner.ParentForm != null && ((ContainerControl)owner.ParentForm).ActiveControl == this)
		{
			((ContainerControl)owner.ParentForm).ActiveControl = null;
			((Control)this).Focus();
		}
		else if (owner.ParentForm != null && IsAnyControl((Control)(object)this, ((ContainerControl)owner.ParentForm).ActiveControl))
		{
			((ContainerControl)owner.ParentForm).ActiveControl = null;
			((Control)this).Focus();
		}
	}

	private void SysButtonMouseOverAutoHide(object sender, EventArgs e)
	{
		if (!m_SystemButtons.MouseOverAutoHide)
		{
			HideToolTip();
		}
		else
		{
			ResetHover();
		}
	}

	private void SysButtonMouseOverCustomize(object sender, EventArgs e)
	{
		if (!m_SystemButtons.MouseOverCustomize)
		{
			HideToolTip();
		}
		else
		{
			ResetHover();
		}
	}

	private void SysButtonMouseOverClose(object sender, EventArgs e)
	{
		if (!m_SystemButtons.MouseOverClose)
		{
			HideToolTip();
		}
		else
		{
			ResetHover();
		}
	}

	private void SysButtonHideTooltip(object sender, EventArgs e)
	{
		HideToolTip();
	}

	private void HideToolTip()
	{
		if (m_ToolTipWnd != null)
		{
			((Control)m_ToolTipWnd).Hide();
			((Component)(object)m_ToolTipWnd).Dispose();
			m_ToolTipWnd = null;
			ResetHover();
		}
	}

	internal void HideAllToolTips()
	{
		HideToolTip();
		if (m_ItemContainer == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			HideItemToolTips(subItem);
		}
	}

	private void HideItemToolTips(BaseItem item)
	{
		item.HideToolTip();
		foreach (BaseItem subItem in item.SubItems)
		{
			HideItemToolTips(subItem);
		}
	}

	private void ShowToolTip(string tipText)
	{
		if (!((Component)this).DesignMode && m_ShowToolTips && tipText != null && !(tipText == "") && Visible)
		{
			if (m_ToolTipWnd == null)
			{
				m_ToolTipWnd = new ToolTip();
			}
			m_ToolTipWnd.Text = tipText;
			if (Owner is IOwnerItemEvents ownerItemEvents)
			{
				ownerItemEvents.InvokeToolTipShowing(m_ToolTipWnd, new EventArgs());
			}
			m_ToolTipWnd.ShowToolTip();
		}
	}

	private void UpdateDockTabSettings()
	{
		if (m_TabDockItems != null && BarFunctions.IsHandleValid((Control)(object)this))
		{
			if (m_TabDockItems.CanReorderTabs != m_CanReorderTabs)
			{
				m_TabDockItems.CanReorderTabs = m_CanReorderTabs;
			}
			if (m_TabDockItems.TabAlignment != m_DockTabAlignment)
			{
				m_TabDockItems.TabAlignment = m_DockTabAlignment;
			}
			if (m_TabNavigation)
			{
				m_TabDockItems.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
			}
			else
			{
				m_TabDockItems.TabLayoutType = eTabLayoutType.FitContainer;
			}
			m_TabDockItems.ShowFocusRectangle = false;
			if (m_TabDockItems._TabSystemBox != null)
			{
				m_TabDockItems._TabSystemBox.CloseVisible = CanHideResolved;
			}
			if (m_AutoHideState)
			{
				((Control)m_TabDockItems).Visible = false;
			}
		}
	}

	internal void CreateDockTab()
	{
		if (m_TabDockItems == null && BarFunctions.IsHandleValid((Control)(object)this))
		{
			m_TabDockItems = new TabStrip();
			UpdateDockTabSettings();
			SuspendLayout();
			ResizeDockTab();
			m_TabDockItems.SelectedTabChanging += TabStripTabChanging;
			m_TabDockItems.SelectedTabChanged += TabDockItemsSelectedTabChanged;
			m_TabDockItems.BeforeTabDisplay += BeforeTabDisplay;
			m_TabDockItems.TabMoved += TabStripTabMoved;
			m_TabDockItems.TabItemClose += TabStripTabItemClose;
			m_TabDockItems.CloseButtonOnTabsVisible = m_DockTabCloseButtonVisible;
			((Control)this).Controls.Add((Control)(object)m_TabDockItems);
			SetDockTabStyle(m_ItemContainer.EffectiveStyle);
			((Control)m_TabDockItems).Visible = GetDockTabVisible();
			RefreshDockTabItems();
			((Control)this).Invalidate();
			ResumeLayout();
		}
	}

	private void TabDockItemsSelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
	{
		UpdateCloseButtonVisibility();
		OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.SelectedDockTabChanged, SelectedDockContainerItem));
	}

	private void TabStripTabItemClose(object sender, TabStripActionEventArgs e)
	{
		if (sender is TabItem { AttachedItem: DockContainerItem attachedItem })
		{
			CloseDockTab(attachedItem, eEventSource.Mouse);
		}
	}

	internal void SetDockTabStyle(eDotNetBarStyle style)
	{
		if (m_TabDockItems == null)
		{
			return;
		}
		if (style == eDotNetBarStyle.StyleManagerControlled)
		{
			style = StyleManager.GetEffectiveStyle();
		}
		switch (style)
		{
		case eDotNetBarStyle.Metro:
			m_TabDockItems.Style = eTabStripStyle.Metro;
			break;
		case eDotNetBarStyle.Office2003:
		{
			IOwnerBarSupport ownerBarSupport2 = m_Owner as IOwnerBarSupport;
			if (DockSide == eDockSide.Document && ownerBarSupport2 != null && ownerBarSupport2.ApplyDocumentBarStyle)
			{
				m_TabDockItems.Style = eTabStripStyle.OneNote;
			}
			else
			{
				m_TabDockItems.Style = eTabStripStyle.Office2003;
			}
			break;
		}
		case eDotNetBarStyle.VS2005:
		{
			IOwnerBarSupport ownerBarSupport3 = m_Owner as IOwnerBarSupport;
			if (DockSide == eDockSide.Document && ownerBarSupport3 != null && ownerBarSupport3.ApplyDocumentBarStyle)
			{
				m_TabDockItems.Style = eTabStripStyle.VS2005Document;
			}
			else
			{
				m_TabDockItems.Style = eTabStripStyle.VS2005;
			}
			break;
		}
		default:
			if (BarFunctions.IsOffice2007Style(style))
			{
				IOwnerBarSupport ownerBarSupport = m_Owner as IOwnerBarSupport;
				if (DockSide == eDockSide.Document && ownerBarSupport != null && ownerBarSupport.ApplyDocumentBarStyle)
				{
					m_TabDockItems.Style = eTabStripStyle.Office2007Document;
				}
				else
				{
					m_TabDockItems.Style = eTabStripStyle.Office2007Dock;
				}
			}
			else
			{
				m_TabDockItems.Style = eTabStripStyle.Flat;
			}
			break;
		}
		m_TabDockItems.ColorScheme.TabBorder = Color.Empty;
		if (this.TabStripStyleChanged != null)
		{
			this.TabStripStyleChanged(this, new EventArgs());
		}
	}

	private void BeforeTabDisplay(object sender, EventArgs e)
	{
		if (sender is TabItem)
		{
			InvokeBeforeDockTabDisplayed(((TabItem)sender).AttachedItem);
		}
	}

	private void DestroyDockTab()
	{
		if (m_TabDockItems != null)
		{
			if (m_TabDockItems.IsDraggingBar)
			{
				((Control)m_TabDockItems).Visible = false;
				m_TabDockItems.Tabs.Clear();
			}
			else
			{
				((Control)this).Controls.Remove((Control)(object)m_TabDockItems);
				((Component)(object)m_TabDockItems).Dispose();
				m_TabDockItems = null;
			}
		}
	}

	internal void RefreshTabStrip()
	{
		if (m_TabDockItems != null)
		{
			((Control)m_TabDockItems).Refresh();
		}
	}

	internal void RefreshDockTab(bool bFireEvents)
	{
		if (m_ItemContainer == null || !BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		SyncBarCaption();
		int visibleSubItems = m_ItemContainer.VisibleSubItems;
		if (visibleSubItems > 1 || (m_AlwaysDisplayDockTab && m_BarState != eBarState.Floating))
		{
			if (m_TabDockItems == null)
			{
				CreateDockTab();
				return;
			}
			if (!((Control)m_TabDockItems).Visible && m_BarState != eBarState.AutoHide)
			{
				((Control)m_TabDockItems).Visible = GetDockTabVisible();
			}
			BaseItem baseItem = null;
			if (bFireEvents && m_TabDockItems != null && m_TabDockItems.SelectedTab != null)
			{
				baseItem = m_TabDockItems.SelectedTab.AttachedItem;
			}
			RefreshDockTabItems();
			if (bFireEvents && m_TabDockItems != null && m_TabDockItems.SelectedTab != null && baseItem != m_TabDockItems.SelectedTab.AttachedItem)
			{
				InvokeDockTabChange(baseItem, m_TabDockItems.SelectedTab.AttachedItem);
			}
			return;
		}
		if (visibleSubItems == 1 && m_TabDockItems != null)
		{
			DockContainerItem dockContainerItem = null;
			foreach (BaseItem subItem in m_ItemContainer.SubItems)
			{
				if (subItem.Visible && subItem is DockContainerItem)
				{
					dockContainerItem = subItem as DockContainerItem;
					break;
				}
			}
			if (dockContainerItem != null && bFireEvents)
			{
				InvokeDockTabChange(null, dockContainerItem);
			}
		}
		if (((Component)this).DesignMode)
		{
			if (m_TabDockItems != null)
			{
				((Control)m_TabDockItems).Visible = false;
			}
		}
		else
		{
			DestroyDockTab();
		}
	}

	internal void RefreshAutoHidePanel()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!AutoHide)
		{
			return;
		}
		AutoHidePanel autoHidePanel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
		if (autoHidePanel != null)
		{
			autoHidePanel.RefreshBar(this);
			if (autoHidePanel.SelectedDockContainerItem != null)
			{
				InvokeDockTabChange(null, autoHidePanel.SelectedDockContainerItem);
			}
		}
	}

	internal void OnDockContainerVisibleChanged(DockContainerItem dockItem)
	{
		if (!AutoHide || dockItem.Visible)
		{
			return;
		}
		bool flag = true;
		foreach (BaseItem item in Items)
		{
			if (item.Visible && flag)
			{
				item.Displayed = true;
				flag = false;
			}
			else
			{
				item.Displayed = false;
			}
		}
	}

	public void ReDock()
	{
		if (m_BarState == eBarState.Floating && (CanDockBottom || CanDockTop || CanDockLeft || CanDockRight || CanDockDocument))
		{
			DockingHandler(m_LastDockSiteInfo, Point.Empty);
		}
	}

	internal void OnSubItemRemoved(BaseItem objItem)
	{
		if (objItem is DockContainerItem && LayoutType == eLayoutType.DockContainer)
		{
			if (objItem.Displayed)
			{
				bool flag = false;
				foreach (BaseItem item in Items)
				{
					if (item.Displayed)
					{
						flag = true;
					}
				}
				if (!flag && Items.Count > 0)
				{
					Items[0].Displayed = true;
				}
			}
			RefreshDockTab(bFireEvents: true);
			if (AutoHide)
			{
				GetAutoHidePanel()?.RefreshBar(this);
			}
		}
		SyncBarCaption();
	}

	protected void InvokeCaptionButtonClick()
	{
		if (this.CaptionButtonClick != null)
		{
			this.CaptionButtonClick(this, new EventArgs());
		}
	}

	private void ToggleCaptionMenu()
	{
		if (m_CaptionMenu != null)
		{
			bool expanded = m_CaptionMenu.Expanded;
			if (expanded)
			{
				m_CaptionMenu.ClosePopup();
			}
			m_CaptionMenu.Dispose();
			m_CaptionMenu = null;
			if (expanded)
			{
				return;
			}
		}
		if (Items.Count <= 0)
		{
			return;
		}
		m_CaptionMenu = new ButtonItem("sysCaptionButtonMenuParent");
		m_CaptionMenu.Style = Style;
		m_CaptionMenu.PopupShowing += CaptionMenuShowing;
		m_CaptionMenu.PopupFinalized += CaptionMenuClose;
		foreach (BaseItem item in Items)
		{
			ButtonItem buttonItem = new ButtonItem("sysCaption_" + item.Name);
			buttonItem.Tag = item;
			buttonItem.OptionGroup = "sysCaptionMenu";
			buttonItem.Text = item.Text;
			buttonItem.BeginGroup = item.BeginGroup;
			if (item is DockContainerItem)
			{
				DockContainerItem dockContainerItem = item as DockContainerItem;
				buttonItem.Checked = item.Displayed;
				if (dockContainerItem.Image != null)
				{
					object obj = dockContainerItem.Image.Clone();
					buttonItem.Image = (Image)((obj is Image) ? obj : null);
				}
				else if (dockContainerItem.Icon != null)
				{
					object obj2 = dockContainerItem.Icon.Clone();
					buttonItem.Icon = (Icon)((obj2 is Icon) ? obj2 : null);
				}
				else if (dockContainerItem.ImageIndex >= 0 && dockContainerItem.ImageList != null)
				{
					object obj3 = dockContainerItem.ImageList.Images[dockContainerItem.ImageIndex].Clone();
					buttonItem.Image = (Image)((obj3 is Image) ? obj3 : null);
				}
			}
			else
			{
				buttonItem.Checked = item.Visible;
			}
			buttonItem.Click += CaptionMenuItemClick;
			m_CaptionMenu.SubItems.Add(buttonItem);
		}
		if (m_CaptionMenu.GetOwner() == null)
		{
			m_CaptionMenu.SetOwner(m_Owner);
		}
		Size popupSize = m_CaptionMenu.PopupSize;
		Point point = new Point(m_SystemButtons.CaptionButtonRect.Right - popupSize.Width, m_SystemButtons.CaptionButtonRect.Bottom);
		if (point.X < 0)
		{
			point.X = 0;
		}
		point = ((Control)this).PointToScreen(point);
		m_CaptionMenu.Popup(point);
	}

	private void CaptionMenuItemClick(object sender, EventArgs e)
	{
		if (!(sender is ButtonItem buttonItem))
		{
			return;
		}
		if (LayoutType == eLayoutType.DockContainer)
		{
			SelectedDockTab = Items.IndexOf(buttonItem.Tag as BaseItem);
			return;
		}
		foreach (BaseItem item in Items)
		{
			if (item == buttonItem.Tag)
			{
				item.Visible = true;
			}
			else
			{
				item.Visible = false;
			}
		}
		RecalcLayout();
		m_CaptionMenu.Dispose();
		m_CaptionMenu = null;
	}

	private void CaptionMenuShowing(object sender, EventArgs e)
	{
		if (m_CaptionMenu != null)
		{
			((MenuPanel)(object)m_CaptionMenu.PopupControl).ColorScheme = GetColorScheme();
		}
	}

	private void CaptionMenuClose(object sender, EventArgs e)
	{
		PaintCaptionButton();
	}

	internal void RefreshDockContainerItem(DockContainerItem item)
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		if (m_ItemContainer == null || m_TabDockItems == null)
		{
			SyncBarCaption();
			return;
		}
		foreach (TabItem tab in m_TabDockItems.Tabs)
		{
			if (tab.AttachedItem == item)
			{
				tab.Text = item.Text;
				tab.AttachedItem = item;
				tab.Tooltip = item.Tooltip;
				tab.Name = item.Name;
				if (item.Icon != null)
				{
					object obj = item.Icon.Clone();
					tab.Icon = (Icon)((obj is Icon) ? obj : null);
				}
				else
				{
					tab.Image = item.TabImage;
					tab.Icon = null;
				}
				break;
			}
		}
		((Control)m_TabDockItems).Refresh();
		if (AutoHide)
		{
			AutoHidePanel autoHidePanel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
			if (autoHidePanel != null)
			{
				((Control)autoHidePanel).Refresh();
			}
		}
		SyncBarCaption();
	}

	private bool GetDockTabVisible()
	{
		if (m_GrabHandleStyle == eGrabHandleStyle.CaptionTaskPane && m_AutoCreateCaptionMenu)
		{
			return false;
		}
		return true;
	}

	protected virtual void OnDockTabStripHeightChanged(int oldValue, int newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			RecalcLayout();
		}
	}

	private void RefreshDockTabItems()
	{
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		if (m_RefreshingDockTab)
		{
			return;
		}
		if (!((Control)m_TabDockItems).Visible && m_BarState != eBarState.AutoHide)
		{
			((Control)m_TabDockItems).Visible = GetDockTabVisible();
			if (IsThemed && m_BarState == eBarState.Floating)
			{
				((Control)m_TabDockItems).Size = new Size(((Control)this).Width - (m_ThemeWindowMargins.Left + m_ThemeWindowMargins.Right), DockTabStripHeight);
				((Control)m_TabDockItems).Location = new Point(m_ThemeWindowMargins.Left, ((Control)this).Height - DockTabStripHeight - m_ThemeWindowMargins.Bottom);
			}
			else
			{
				((Control)m_TabDockItems).Size = new Size(((Control)this).Width - 4, DockTabStripHeight);
				((Control)m_TabDockItems).Location = new Point(2, ((Control)this).Height - DockTabStripHeight - 2);
			}
		}
		m_RefreshingDockTab = true;
		try
		{
			m_TabDockItems.Tabs.Clear();
			int num = 0;
			foreach (BaseItem subItem in m_ItemContainer.SubItems)
			{
				if (!subItem.Visible)
				{
					continue;
				}
				num++;
				TabItem tabItem = new TabItem();
				tabItem.Text = subItem.Text;
				tabItem.Tooltip = subItem.Tooltip;
				tabItem.Name = subItem.Name;
				if (subItem is DockContainerItem)
				{
					DockContainerItem dockContainerItem = subItem as DockContainerItem;
					if (dockContainerItem.Icon != null)
					{
						object obj = dockContainerItem.Icon.Clone();
						tabItem.Icon = (Icon)((obj is Icon) ? obj : null);
					}
					else
					{
						tabItem.Image = dockContainerItem.TabImage;
					}
					tabItem.PredefinedColor = dockContainerItem.PredefinedTabColor;
				}
				m_TabDockItems.Tabs.Add(tabItem);
				tabItem.AttachedItem = subItem;
				if (subItem.Displayed)
				{
					m_TabDockItems._IgnoreBeforeTabDisplayEvent = true;
					m_TabDockItems.SelectedTab = tabItem;
					m_TabDockItems._IgnoreBeforeTabDisplayEvent = false;
				}
			}
			if (num > 1 || (num == 1 && m_AlwaysDisplayDockTab && m_BarState != eBarState.Floating))
			{
				if (!((Control)m_TabDockItems).Visible && m_BarState != eBarState.AutoHide)
				{
					((Control)m_TabDockItems).Visible = GetDockTabVisible();
				}
				if (m_TabDockItems.SelectedTab == null)
				{
					m_TabDockItems.SelectedTab = m_TabDockItems.Tabs[0];
				}
				else if (m_TabDockItems.SelectedTab.AttachedItem != null && !m_TabDockItems.SelectedTab.AttachedItem.Displayed)
				{
					m_TabDockItems.SelectedTab.AttachedItem.Displayed = true;
				}
				((Control)m_TabDockItems).Refresh();
			}
			else if (((Control)m_TabDockItems).Visible)
			{
				((Control)m_TabDockItems).Visible = false;
			}
		}
		finally
		{
			m_RefreshingDockTab = false;
		}
		if (m_BarState == eBarState.AutoHide)
		{
			GetAutoHidePanel(m_LastDockSiteInfo.DockSide)?.RefreshBar(this);
		}
	}

	private void ResizeDockTab()
	{
		if (m_TabDockItems == null || !((Control)m_TabDockItems).Visible)
		{
			return;
		}
		Point point = Point.Empty;
		Size size = Size.Empty;
		switch (m_DockTabAlignment)
		{
		case eTabStripAlignment.Top:
			if (m_GrabHandleStyle != 0 && !m_GrabHandleRect.IsEmpty)
			{
				point = new Point(m_GrabHandleRect.X, m_GrabHandleRect.Bottom);
				size = new Size(((Control)this).Width - m_GrabHandleRect.Left * 2, DockTabStripHeight);
			}
			else if (m_BarState == eBarState.Docked)
			{
				point = new Point(1, 1);
				size = new Size(((Control)this).Width - 2, DockTabStripHeight);
			}
			else if (m_BarState == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(m_ThemeWindowMargins.Left, m_ThemeWindowMargins.Top);
					size = new Size(((Control)this).Width - (m_ThemeWindowMargins.Left + m_ThemeWindowMargins.Right), DockTabStripHeight);
				}
				else
				{
					point = new Point(3, 3);
					size = new Size(((Control)this).Width - 6, DockTabStripHeight);
				}
			}
			else
			{
				((Control)m_TabDockItems).Visible = false;
			}
			break;
		case eTabStripAlignment.Left:
			if (m_GrabHandleStyle != 0 && !m_GrabHandleRect.IsEmpty)
			{
				point = new Point(m_GrabHandleRect.X + 1, m_GrabHandleRect.Bottom + 1);
				size = new Size(DockTabStripHeight, m_ItemContainer.HeightInternal - 1);
			}
			else if (m_BarState == eBarState.Docked)
			{
				point = new Point(2, 1);
				size = new Size(DockTabStripHeight, ((Control)this).Height - 3);
			}
			else if (m_BarState == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(m_ThemeWindowMargins.Left, m_ThemeWindowMargins.Top);
					size = new Size(((Control)this).Width - (m_ThemeWindowMargins.Left + m_ThemeWindowMargins.Right), DockTabStripHeight);
				}
				else
				{
					point = new Point(3, 3);
					size = new Size(((Control)this).Width - 6, DockTabStripHeight);
				}
			}
			else
			{
				((Control)m_TabDockItems).Visible = false;
			}
			break;
		case eTabStripAlignment.Right:
			if (m_GrabHandleStyle != 0 && !m_GrabHandleRect.IsEmpty && m_BarState == eBarState.Docked)
			{
				point = new Point(((Control)this).ClientRectangle.Right - DockTabStripHeight, m_GrabHandleRect.Bottom + 1);
				size = new Size(DockTabStripHeight - 2, m_ItemContainer.HeightInternal);
			}
			else if (m_BarState == eBarState.Docked)
			{
				point = new Point(((Control)this).Width - DockTabStripHeight, 1);
				size = new Size(DockTabStripHeight, ((Control)this).Height - 2);
			}
			else if (m_BarState == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(((Control)this).Width - m_ThemeWindowMargins.Right - DockTabStripHeight, m_ThemeWindowMargins.Top);
					size = new Size(DockTabStripHeight, ((Control)this).Height - (m_ThemeWindowMargins.Top + m_ThemeWindowMargins.Bottom));
				}
				else
				{
					point = new Point(((Control)this).Width - 3 - DockTabStripHeight, m_ItemContainer.TopInternal);
					size = new Size(DockTabStripHeight, m_ItemContainer.HeightInternal);
				}
			}
			else
			{
				((Control)m_TabDockItems).Visible = false;
			}
			break;
		default:
			point = new Point(0, ((Control)this).Height - DockTabStripHeight);
			size = new Size(((Control)this).Width, DockTabStripHeight);
			if (DockSide == eDockSide.Top && m_BarState == eBarState.Docked)
			{
				point = new Point(0, ((Control)this).Height - DockTabStripHeight - 1);
				size = new Size(((Control)this).Width, DockTabStripHeight);
			}
			if (m_BarState == eBarState.Docked)
			{
				if (m_DockedBorder != 0)
				{
					point = new Point(2, ((Control)this).Height - DockTabStripHeight - 3);
					size = new Size(((Control)this).Width - 4, DockTabStripHeight);
				}
			}
			else if (m_BarState == eBarState.Floating)
			{
				if (IsThemed)
				{
					point = new Point(m_ThemeWindowMargins.Left, ((Control)this).Height - DockTabStripHeight - m_ThemeWindowMargins.Right);
					size = new Size(((Control)this).Width - (m_ThemeWindowMargins.Left + m_ThemeWindowMargins.Right), DockTabStripHeight);
				}
				else
				{
					point = new Point(3, ((Control)this).Height - DockTabStripHeight - 3);
					size = new Size(((Control)this).Width - 6, DockTabStripHeight);
				}
			}
			else
			{
				((Control)m_TabDockItems).Visible = false;
			}
			break;
		}
		((Control)m_TabDockItems).SetBounds(point.X, point.Y, size.Width, size.Height);
	}

	private void TabStripTabChanging(object sender, TabStripTabChangingEventArgs e)
	{
		if (!m_RefreshingDockTab)
		{
			e.Cancel = InvokeDockTabChange((e.OldTab == null) ? null : e.OldTab.AttachedItem, (e.NewTab == null) ? null : e.NewTab.AttachedItem);
		}
	}

	private void SyncBarCaption(BaseItem selected)
	{
		if (!m_AutoSyncBarCaption || LayoutType != eLayoutType.DockContainer)
		{
			return;
		}
		DockContainerItem selectedDockContainerItem = SelectedDockContainerItem;
		if ((selectedDockContainerItem != null && selectedDockContainerItem.Visible) || selected != null)
		{
			if (selected == null)
			{
				selected = selectedDockContainerItem;
			}
			((Control)this).Text = selected.Text;
			return;
		}
		foreach (BaseItem item in Items)
		{
			if (item.Visible && item.Displayed)
			{
				((Control)this).Text = item.Text;
				break;
			}
		}
	}

	internal void SyncBarCaption()
	{
		SyncBarCaption(null);
	}

	private bool InvokeDockTabChange(BaseItem oldItem, BaseItem newItem)
	{
		DockTabChangeEventArgs dockTabChangeEventArgs = null;
		IOwnerBarSupport ownerBarSupport = m_Owner as IOwnerBarSupport;
		bool flag = false;
		if (this.DockTabChange != null)
		{
			dockTabChangeEventArgs = new DockTabChangeEventArgs(oldItem, newItem);
			this.DockTabChange(this, dockTabChangeEventArgs);
			flag = dockTabChangeEventArgs.Cancel;
			if (flag)
			{
				return flag;
			}
		}
		if (m_Owner != null)
		{
			if (dockTabChangeEventArgs == null)
			{
				dockTabChangeEventArgs = new DockTabChangeEventArgs(oldItem, newItem);
			}
			ownerBarSupport?.InvokeDockTabChange(this, dockTabChangeEventArgs);
			flag = dockTabChangeEventArgs.Cancel;
		}
		if (!flag)
		{
			SyncBarCaption(newItem);
			UpdateCloseButtonVisibility();
		}
		if (!flag)
		{
			OnBarStateChanged(new BarStateChangedEventArgs(this, eBarStateChange.SelectedDockTabChanging, newItem));
		}
		return flag;
	}

	internal void InvokeBeforeDockTabDisplayed(BaseItem item)
	{
		if (this.BeforeDockTabDisplayed != null)
		{
			this.BeforeDockTabDisplayed(item, new EventArgs());
		}
		if (m_Owner is IOwnerBarSupport ownerBarSupport)
		{
			ownerBarSupport.InvokeBeforeDockTabDisplay(item, new EventArgs());
		}
	}

	private void TabStripTabMoved(object sender, TabStripTabMovedEventArgs e)
	{
		m_ItemContainer.SubItems.RemoveItem((DockContainerItem)e.Tab.AttachedItem);
		m_ItemContainer.SubItems._Add((DockContainerItem)e.Tab.AttachedItem, e.NewIndex);
		m_TabsRearranged = true;
	}

	public Size MinimumDockSize(eOrientation dockOrientation)
	{
		if (m_ItemContainer == null)
		{
			return new Size(32, 32);
		}
		if (m_ItemContainer.LayoutType == eLayoutType.DockContainer && (m_ItemContainer.Stretch || m_DockStretch))
		{
			if (m_ItemContainer.SubItems.Count > 0 && m_ItemContainer.SubItems[0] is DockContainerItem)
			{
				return ((DockContainerItem)m_ItemContainer.SubItems[0]).MinimumSize;
			}
			return new Size(32, 32);
		}
		if (m_ItemContainer.Stretch || m_DockStretch)
		{
			return PreferredDockSize(dockOrientation);
		}
		if (m_ItemContainer.SubItems.Count > 0)
		{
			BaseItem baseItem = m_ItemContainer.SubItems[0];
			if (baseItem != null)
			{
				return new Size(baseItem.WidthInternal + m_ClientRect.Left * 2, baseItem.HeightInternal);
			}
		}
		return new Size(0, 0);
	}

	internal Size GetAdjustedFullSize(Size size)
	{
		Size result = size;
		if (m_TabDockItems != null && ((Control)m_TabDockItems).Visible && (DockTabAlignment == eTabStripAlignment.Top || DockTabAlignment == eTabStripAlignment.Bottom))
		{
			result.Height += ((Control)m_TabDockItems).Height;
		}
		result.Height += 6 + SystemInformation.ToolWindowCaptionHeight;
		result.Width += 6;
		return result;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int GetBarDockedSize(eOrientation o)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		if (m_ItemContainer == null)
		{
			return 0;
		}
		if (o == eOrientation.Horizontal && m_LastDockSiteInfo.DockedHeight > 0 && ((int)m_LastDockSiteInfo.DockSide == 1 || (int)m_LastDockSiteInfo.DockSide == 2))
		{
			return m_LastDockSiteInfo.DockedHeight;
		}
		if (o == eOrientation.Vertical && m_LastDockSiteInfo.DockedWidth > 0 && ((int)m_LastDockSiteInfo.DockSide == 3 || (int)m_LastDockSiteInfo.DockSide == 4))
		{
			return m_LastDockSiteInfo.DockedWidth;
		}
		if (o == eOrientation.Horizontal && m_ItemContainer.MinHeight > 0 && m_ItemContainer.MinHeight + SystemInformation.ToolWindowCaptionHeight < GetFormClientHeight())
		{
			return m_ItemContainer.MinHeight + SystemInformation.ToolWindowCaptionHeight;
		}
		if (o == eOrientation.Vertical && m_ItemContainer.MinWidth > 0 && m_ItemContainer.MinWidth + SystemInformation.ToolWindowCaptionHeight < GetFormClientWidth())
		{
			return m_ItemContainer.MinWidth + SystemInformation.ToolWindowCaptionHeight;
		}
		if (SelectedDockTab >= 0 || (VisibleItemCount > 0 && LayoutType == eLayoutType.DockContainer))
		{
			DockContainerItem dockContainerItem = null;
			dockContainerItem = ((SelectedDockTab < 0) ? (GetFirstVisibleItem() as DockContainerItem) : (Items[SelectedDockTab] as DockContainerItem));
			if (dockContainerItem != null)
			{
				if (o == eOrientation.Horizontal)
				{
					if (dockContainerItem.MinimumSize.Height > 0)
					{
						int num = ((Control)this).Height;
						if ((((Control)this).Parent != null && (DockSide == eDockSide.Left || DockSide == eDockSide.Right)) || (((Control)this).Parent == null && (LastDockSide == eDockSide.Left || LastDockSide == eDockSide.Right)))
						{
							num = ((Control)this).Width;
						}
						if (num > dockContainerItem.MinimumSize.Height)
						{
							return num;
						}
						return dockContainerItem.MinimumSize.Height * 2 + DockTabStripHeight + 6;
					}
				}
				else if (dockContainerItem.MinimumSize.Width > 0)
				{
					int num2 = ((Control)this).Width;
					if ((((Control)this).Parent != null && (DockSide == eDockSide.Top || DockSide == eDockSide.Bottom)) || (((Control)this).Parent == null && (LastDockSide == eDockSide.Top || LastDockSide == eDockSide.Bottom)))
					{
						num2 = ((Control)this).Height;
					}
					if (num2 > dockContainerItem.MinimumSize.Width)
					{
						return num2;
					}
					return dockContainerItem.MinimumSize.Width * 2 + 6;
				}
			}
		}
		if (o == eOrientation.Horizontal)
		{
			return ((Control)this).Height;
		}
		return ((Control)this).Width;
	}

	public Size PreferredDockSize(eOrientation dockOrientation)
	{
		IOwner owner = m_Owner as IOwner;
		if (dockOrientation == eOrientation.Horizontal)
		{
			if (m_DockedSizeH.IsEmpty)
			{
				if (owner != null && owner.ParentForm != null)
				{
					m_DockedSizeH = RecalcSizeOnly(owner.ParentForm.Size, eOrientation.Horizontal, eBarState.Docked, m_WrapItemsDock);
				}
				else
				{
					m_DockedSizeH = RecalcSizeOnly(Screen.FromControl((Control)(object)this).WorkingArea.Size, eOrientation.Horizontal, eBarState.Docked, m_WrapItemsDock);
				}
			}
			return m_DockedSizeH;
		}
		if (m_DockedSizeV.IsEmpty)
		{
			Size empty = Size.Empty;
			empty = ((owner == null || owner.ParentForm == null) ? RecalcSizeOnly(Screen.FromControl((Control)(object)this).WorkingArea.Size, eOrientation.Horizontal, eBarState.Docked, m_WrapItemsDock) : RecalcSizeOnly(owner.ParentForm.Size, eOrientation.Horizontal, eBarState.Docked, m_WrapItemsDock));
			m_DockedSizeV = new Size(empty.Height, empty.Width);
		}
		return m_DockedSizeV;
	}

	protected override void OnDockChanged(EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnDockChanged(e);
		if ((int)((Control)this).Dock != 0)
		{
			m_DockStretch = true;
		}
		else
		{
			m_DockStretch = false;
		}
		RecalcLayout();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDockLine(int iLine)
	{
		m_DockLine = iLine;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		DotNetBarManager dotNetBarManager = Owner as DotNetBarManager;
		bool flag = false;
		if (dotNetBarManager != null)
		{
			flag = dotNetBarManager.EnableDockCtrlTabKey;
		}
		if (flag && LayoutType == eLayoutType.DockContainer && VisibleItemCount > 1 && (keyData & 0x20000) == 131072 && (keyData & 9) == 9 && WinApi.HIWORD(WinApi.GetKeyState(9)) != 0)
		{
			if ((keyData & 0x10000) == 65536)
			{
				SelectPreviousTab(cycle: true);
			}
			else
			{
				SelectNextTab(cycle: true);
			}
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	internal bool SelectPreviousTab(bool cycle)
	{
		int num = -1;
		if (SelectedDockTab <= 0)
		{
			if (!cycle)
			{
				return false;
			}
			num = GetTabIndex(Items.Count, -1);
		}
		else
		{
			num = GetTabIndex(SelectedDockTab, -1);
			if (num < 0 && cycle)
			{
				num = GetTabIndex(Items.Count, -1);
			}
		}
		if (num < 0)
		{
			return false;
		}
		SelectedDockTab = num;
		return true;
	}

	internal bool SelectNextTab(bool cycle)
	{
		int num = -1;
		if (SelectedDockTab >= Items.Count - 1)
		{
			if (!cycle)
			{
				return false;
			}
			num = GetTabIndex(-1, 1);
		}
		else
		{
			num = GetTabIndex(SelectedDockTab, 1);
			if (num < 0 && cycle)
			{
				num = GetTabIndex(-1, 1);
			}
		}
		if (num < 0)
		{
			return false;
		}
		SelectedDockTab = num;
		return true;
	}

	private int GetTabIndex(int start, int direction)
	{
		int num = start;
		int num2 = Items.Count - 1;
		int num3 = 1;
		if (direction < 0)
		{
			num2 = 0;
			num3 = -1;
			if (start <= 0)
			{
				return -1;
			}
		}
		else if (start >= num2)
		{
			return -1;
		}
		while (num2 != num)
		{
			num += num3;
			if (Items[num] is DockContainerItem { Visible: not false })
			{
				return num;
			}
		}
		return -1;
	}

	public bool ShouldSerializeDockSide()
	{
		if (((Control)this).Parent is DockSite && ((DockSite)(object)((Control)this).Parent).DocumentDockContainer != null)
		{
			return false;
		}
		return DockSide != eDockSide.None;
	}

	private void ChangeAutoHidePanel(eDockSide side)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected I4, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (!AutoHide)
		{
			return;
		}
		DockStyle val = (DockStyle)3;
		switch (side)
		{
		case eDockSide.Bottom:
			val = (DockStyle)2;
			break;
		case eDockSide.Right:
			val = (DockStyle)4;
			break;
		case eDockSide.Top:
			val = (DockStyle)1;
			break;
		}
		if (m_LastDockSiteInfo.DockSide == val || side == eDockSide.None)
		{
			return;
		}
		AutoHidePanel autoHidePanel = GetAutoHidePanel(m_LastDockSiteInfo.DockSide);
		if (autoHidePanel != null && m_Owner is IOwnerBarSupport ownerBarSupport)
		{
			AnimateHide();
			autoHidePanel.RemoveBar(this);
			m_LastDockSiteInfo.DockSide = val;
			switch (val - 1)
			{
			case 2:
				m_LastDockSiteInfo.objDockSite = ownerBarSupport.LeftDockSite;
				break;
			case 3:
				m_LastDockSiteInfo.objDockSite = ownerBarSupport.RightDockSite;
				break;
			case 0:
				m_LastDockSiteInfo.objDockSite = ownerBarSupport.TopDockSite;
				break;
			case 1:
				m_LastDockSiteInfo.objDockSite = ownerBarSupport.BottomDockSite;
				break;
			}
			autoHidePanel = GetAutoHidePanel(val);
			autoHidePanel.AddBar(this);
		}
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
		if (DockSide == eDockSide.Document && m_Owner != null)
		{
			((IOwner)m_Owner).SetFocusItem(objFocusItem);
			return;
		}
		if (m_FocusItem != null && m_FocusItem != objFocusItem)
		{
			m_FocusItem.OnLostFocus();
		}
		m_FocusItem = objFocusItem;
		if (m_FocusItem != null)
		{
			m_FocusItem.OnGotFocus();
		}
		if (this.FocusItemChange != null)
		{
			this.FocusItemChange(this, new EventArgs());
		}
	}

	BaseItem IOwner.GetFocusItem()
	{
		if (DockSide == eDockSide.Document && m_Owner != null && m_Owner is IOwner)
		{
			return ((IOwner)m_Owner).GetFocusItem();
		}
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

	MdiClient IOwner.GetMdiClient(Form MdiForm)
	{
		return BarFunctions.GetMdiClient(MdiForm);
	}

	void IOwner.Customize()
	{
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

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void BeginInit()
	{
		m_LayoutSuspended = true;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndInit()
	{
		m_LayoutSuspended = false;
		if (DockSide == eDockSide.Document && ((Control)this).Parent is DockSite)
		{
			((DockSite)(object)((Control)this).Parent).RecalcLayout();
		}
		else
		{
			RecalcSize();
		}
		if (AutoHide != m_AutoHideStateDelayed)
		{
			AutoHide = m_AutoHideStateDelayed;
		}
	}

	void IOwner.InvokeResetDefinition(BaseItem item, EventArgs e)
	{
	}

	void IOwner.InvokeUserCustomize(object sender, EventArgs e)
	{
	}

	void IOwner.InvokeEndUserCustomize(object sender, EndUserCustomizeEventArgs e)
	{
	}

	void IOwner.StartItemDrag(BaseItem item)
	{
		if (((Component)this).DesignMode && m_BarDesigner != null)
		{
			m_BarDesigner.StartExternalDrag(item);
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

	private void MenuEventSupportHook()
	{
		if (m_MenuEventSupport)
		{
			return;
		}
		m_MenuEventSupport = true;
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			m_MenuEventSupport = false;
			return;
		}
		if (val.IsMdiChild && val.MdiParent != null)
		{
			val = val.MdiParent;
		}
		((Control)val).Resize += ParentResize;
		val.Deactivate += ParentDeactivate;
		DotNetBarManager.RegisterParentMsgHandler(this, val);
	}

	private void MenuEventSupportUnhook()
	{
		if (!m_MenuEventSupport)
		{
			return;
		}
		m_MenuEventSupport = false;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if (val.IsMdiChild && val.MdiParent != null)
			{
				val = val.MdiParent;
			}
			DotNetBarManager.UnRegisterParentMsgHandler(this, val);
			((Control)val).Resize -= ParentResize;
			val.Deactivate -= ParentDeactivate;
		}
	}

	private void ParentResize(object sender, EventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val.IsMdiChild && val.MdiParent != null)
		{
			val = val.MdiParent;
		}
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void ParentDeactivate(object sender, EventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val.IsMdiChild && val.MdiParent != null)
		{
			val = val.MdiParent;
		}
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
		if (!((Component)this).DesignMode && !_UseHook)
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

	internal void ClosePopups()
	{
		ArrayList arrayList = new ArrayList(m_RegisteredPopups);
		foreach (PopupItem item in arrayList)
		{
			if (item.QueryPopupClosing(eEventSource.Mouse))
			{
				item.ClosePopup();
			}
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

	bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected I4, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		bool designMode = ((Component)this).DesignMode;
		int num = WinApi.ToInt(wParam);
		if (m_RegisteredPopups.Count > 0)
		{
			BaseItem baseItem = (BaseItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1];
			Keys val = (Keys)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
			if ((int)val == 0)
			{
				val = (Keys)num;
			}
			if (baseItem.Parent == null || IsContextPopup(baseItem) || (int)val == 27 || (int)val == 13 || (int)val == 37 || (int)val == 39 || (int)val == 40 || (int)val == 38)
			{
				PopupItem popupItem = (PopupItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1];
				Control popupControl = popupItem.PopupControl;
				Control val2 = Control.FromChildHandle(hWnd);
				if (val2 != null)
				{
					while (val2.Parent != null)
					{
						val2 = val2.Parent;
					}
				}
				bool flag = false;
				if (val2 != null && popupItem != null)
				{
					flag = popupItem.IsAnyOnHandle(val2.Handle);
				}
				bool flag2 = (popupControl != null && val2 != null && popupControl.Handle == val2.Handle) || flag;
				if (!flag)
				{
					popupItem.InternalKeyDown((KeyEventArgs)(object)new KeyEventArgsEx(val, WinApi.ToAscii((uint)(int)wParam)));
				}
				if (flag2)
				{
					return false;
				}
				return !designMode;
			}
		}
		Form val3 = ((Control)this).FindForm();
		if (val3 == null || (val3 != Form.ActiveForm && val3.MdiParent == null) || (val3.MdiParent != null && val3.MdiParent.ActiveMdiChild != val3))
		{
			return false;
		}
		if (num >= 112 || (int)Control.ModifierKeys != 0 || (WinApi.ToInt(lParam) & 0x1000000000L) != 0L || num == 46 || num == 45)
		{
			int key = Control.ModifierKeys | num;
			if (ProcessShortcut((eShortcut)key))
			{
				return !designMode;
			}
			return false;
		}
		return false;
	}

	protected virtual bool IsContextPopup(BaseItem popup)
	{
		return false;
	}

	private bool ProcessShortcut(eShortcut key)
	{
		if (!IsHandlingShortcuts)
		{
			return false;
		}
		bool flag = BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
		return !m_DispatchShortcuts && flag;
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
				if (popupItem.Expanded && popupItem.QueryPopupClosing(eEventSource.Mouse))
				{
					popupItem.Expanded = !popupItem.Expanded;
					if (!popupItem.Expanded && popupItem.Parent != null)
					{
						popupItem.Parent.AutoExpand = false;
					}
				}
			}
			else if (val == null && !flag && popupItem.QueryPopupClosing(eEventSource.Mouse))
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

	protected override bool ProcessMnemonic(char charCode)
	{
		if (!MenuBar)
		{
			foreach (BaseItem item in Items)
			{
				if (item.Visible && item.GetEnabled() && item.Displayed && item.AccessKey == charCode)
				{
					item.RaiseClick(eEventSource.Keyboard);
					return true;
				}
			}
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	protected virtual bool InternalSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Invalid comparison between Unknown and I4
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected I4, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected I4, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		int num = WinApi.ToInt(wParam);
		if (num == 18 && (int)Control.ModifierKeys == 262144)
		{
			ClosePopups();
		}
		Form val = ((Control)this).FindForm();
		if (val == null || !BarFunctions.IsFormActive(val))
		{
			return false;
		}
		Bar bar = null;
		if (MenuBar)
		{
			bar = this;
		}
		if (bar != null && bar.ItemsContainer != null && !bar.ItemsContainer.DesignMode)
		{
			GenericItemContainer itemsContainer = bar.ItemsContainer;
			if (itemsContainer == null)
			{
				return false;
			}
			if (num == 18 || (num == 121 && !m_IgnoreF10Key && ((int)Control.ModifierKeys == 0 || (int)Control.ModifierKeys == 262144)))
			{
				if (itemsContainer.ExpandedItem() != null && ((Control)bar).Focused)
				{
					bar.ReleaseFocus();
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
					if (num2 > 0 && itemsContainer.SysKeyDown(num2))
					{
						return true;
					}
				}
			}
		}
		else if (bar == null && !((Component)this).DesignMode && ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123)))
		{
			int key2 = Control.ModifierKeys | num;
			if (ProcessShortcut((eShortcut)key2))
			{
				return true;
			}
		}
		if ((int)Control.ModifierKeys == 262144 && num > 27)
		{
			m_IgnoreSysKeyUp = true;
			int num3 = (int)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
			if (num3 != 0 && !((Component)this).DesignMode && m_ItemContainer.SysKeyDown(num3))
			{
				return true;
			}
		}
		return false;
	}

	bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return InternalSysKeyDown(hWnd, wParam, lParam);
	}

	bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		Form val = ((Control)this).FindForm();
		if (val == null || ((Component)this).DesignMode || !BarFunctions.IsFormActive(val))
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
			if (num == 18 || (num == 121 && !m_IgnoreF10Key))
			{
				Bar bar = null;
				if (MenuBar)
				{
					bar = this;
				}
				if (bar != null && !bar.ItemsContainer.DesignMode)
				{
					if (((Control)bar).Focused)
					{
						bar.ReleaseFocus();
					}
					else
					{
						bar.SetSystemFocus();
					}
					return true;
				}
			}
		}
		return false;
	}

	Size IDockInfo.MinimumDockSize(eOrientation dockOrientation)
	{
		return MinimumDockSize(dockOrientation);
	}

	Size IDockInfo.PreferredDockSize(eOrientation dockOrientation)
	{
		return PreferredDockSize(dockOrientation);
	}

	void IDockInfo.SetDockLine(int iLine)
	{
		SetDockLine(iLine);
	}

	private ISite GetSite()
	{
		ISite site = null;
		IOwner owner = Owner as IOwner;
		Control val = null;
		if (owner is Control)
		{
			val = (Control)((owner is Control) ? owner : null);
		}
		if (m_ParentItem != null && m_ParentItem.ContainerControl is Control && (val == null || (val != null && ((Component)(object)val).Site == null)))
		{
			object containerControl = m_ParentItem.ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
		}
		if (val != null)
		{
			while (site == null && val != null)
			{
				if (((Component)(object)val).Site != null && ((Component)(object)val).Site.DesignMode)
				{
					site = ((Component)(object)val).Site;
				}
				else
				{
					val = val.Parent;
				}
			}
		}
		if (site == null && m_ParentItem != null)
		{
			BaseItem baseItem = m_ParentItem;
			while (site == null && baseItem != null)
			{
				if (baseItem.Site != null && baseItem.Site.DesignMode)
				{
					site = baseItem.Site;
				}
				else
				{
					baseItem = baseItem.Parent;
				}
			}
		}
		return site;
	}

	private void DesignTimeMouseMove(MouseEventArgs e)
	{
		if (m_DragDropInProgress)
		{
			try
			{
				if (m_DesignTimeProvider != null)
				{
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
					m_DesignTimeProvider = null;
				}
				InsertPosition insertPosition = m_DragDropDesignTimeProvider.GetInsertPosition(Control.MousePosition, m_DragItem);
				if (insertPosition != null)
				{
					if (insertPosition.TargetProvider == null)
					{
						Cursor.Current = Cursors.No;
					}
					else
					{
						insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
						m_InsertPosition = insertPosition.Position;
						m_InsertBefore = insertPosition.Before;
						m_DesignTimeProvider = insertPosition.TargetProvider;
						Cursor.Current = Cursors.Hand;
					}
				}
				else
				{
					Cursor.Current = Cursors.No;
				}
				return;
			}
			catch
			{
				m_DesignTimeProvider = null;
				return;
			}
		}
		if (m_Owner is IOwner)
		{
			m_DragItem = ((IOwner)m_Owner).GetFocusItem();
		}
		if (m_DragItem != null)
		{
			BaseItem baseItem = m_ParentItem;
			while (baseItem.Parent is IDesignTimeProvider)
			{
				baseItem = baseItem.Parent;
			}
			m_DragDropDesignTimeProvider = (IDesignTimeProvider)baseItem;
			m_DragDropInProgress = true;
			((Control)this).Capture = true;
		}
	}

	private void DesignTimeMouseUp(MouseEventArgs e)
	{
		ISite site = GetSite();
		if (site == null)
		{
			return;
		}
		IComponentChangeService componentChangeService = site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (m_DesignTimeProvider != null)
		{
			m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			BaseItem parent = m_DragItem.Parent;
			if (parent != null)
			{
				if (parent == (BaseItem)m_DesignTimeProvider && m_InsertPosition > 0 && parent.SubItems.IndexOf(m_DragItem) < m_InsertPosition)
				{
					m_InsertPosition--;
				}
				componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
				parent.SubItems.Remove(m_DragItem);
				componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
				object containerControl = parent.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is Bar)
				{
					((Bar)(object)val).RecalcLayout();
				}
				else if (val is MenuPanel)
				{
					((MenuPanel)(object)val).RecalcSize();
				}
			}
			m_DesignTimeProvider.InsertItemAt(m_DragItem, m_InsertPosition, m_InsertBefore);
			m_DesignTimeProvider = null;
		}
		m_DragDropDesignTimeProvider = null;
		m_DragItem = null;
		m_DragDropInProgress = false;
		((Control)this).Capture = false;
	}

	private void DesignTimeMouseDown(MouseEventArgs e)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		IOwner owner = Owner as IOwner;
		BaseItem baseItem = m_ItemContainer.ItemAtLocation(e.X, e.Y);
		if (baseItem != null && baseItem.SystemItem)
		{
			baseItem = null;
		}
		if (baseItem == null)
		{
			return;
		}
		if (owner != null)
		{
			owner.SetFocusItem(baseItem);
			ISite site = GetSite();
			if (site != null)
			{
				ISelectionService selectionService = (ISelectionService)site.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					ArrayList arrayList = new ArrayList(1);
					arrayList.Add(baseItem);
					selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
				}
				if ((int)e.Button == 2097152)
				{
					((IMenuCommandService)site.GetService(typeof(IMenuCommandService)))?.ShowContextMenu(new CommandID(new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}"), 1280), Control.MousePosition.X, Control.MousePosition.Y);
				}
			}
		}
		owner = null;
		baseItem?.InternalMouseDown(e);
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
		}
	}
}
