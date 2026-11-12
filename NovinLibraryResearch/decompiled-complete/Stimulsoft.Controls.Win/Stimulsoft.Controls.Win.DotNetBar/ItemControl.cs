using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Animation;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Touch;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
[DefaultEvent("ItemClick")]
public abstract class ItemControl : ContainerControl, IOwner, IOwnerMenuSupport, IMessageHandlerClient, IOwnerItemEvents, IThemeCache, IOwnerLocalize, IBarDesignerServices, IKeyTipsRenderer, IRenderingSupport, IAccessibilitySupport, IRibbonCustomize, IBarImageSize, IKeyTipsControl
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

	private ElementStyle m_BackgroundStyle = new ElementStyle();

	private bool m_DesignModeInternal;

	private bool m_AntiAlias = true;

	private bool m_AutoSize;

	private bool m_DesignerSelection;

	private IBarItemDesigner m_BarDesigner;

	private bool m_ShowKeyTips;

	private Font m_KeyTipsFont;

	private string m_KeyTipsKeysStack = "";

	private KeyTipsCanvasControl m_KeyTipsCanvas;

	private bool m_MenuFocus;

	private IntPtr m_LastFocusWindow = IntPtr.Zero;

	private Timer m_ActiveWindowTimer;

	private IntPtr m_ForegroundWindow = IntPtr.Zero;

	private IntPtr m_ActiveWindow = IntPtr.Zero;

	private bool m_FadeEffect = true;

	private BaseItem m_DoDefaultActionItem;

	private eBarImageSize m_ImageSize;

	private bool m_ShortcutsEnabled = true;

	private bool m_UseHook;

	private bool m_AlwaysDisplayKeyAccelerators;

	private Hook m_Hook;

	private ArrayList m_RegisteredPopups = new ArrayList();

	private PopupDelayedClose m_DelayClose;

	private const uint WM_GESTURENOTIFY = 282u;

	private bool _EnableGestureNotifyHandler;

	private bool _ContainerControlProcessDialogKey = true;

	private int m_SyncingSize;

	private bool m_DisabledImagesGrayScale = true;

	private bool _AnimateNextPaintRequest;

	private Timer m_KeyTipDelayRefreshTimer;

	private bool m_FirstScrollPaint = true;

	private bool _CanAnimate;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private int m_UpdateSuspendCount;

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

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether accelerator letters for buttons are underlined regardless of current Windows settings.")]
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
				((Control)this).Invalidate();
			}
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
	public bool DragInProgress
	{
		get
		{
			return m_DragInProgress;
		}
		internal set
		{
			if (m_DragInProgress != value)
			{
				m_DragInProgress = value;
				OnDragInProgressChanged();
			}
		}
	}

	BaseItem IOwner.DragItem => m_DragItem;

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	public virtual ImageList Images
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
	public virtual ImageList ImagesMedium
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
	public virtual ImageList ImagesLarge
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
	public virtual bool ShowShortcutKeysInToolTips
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

	[Browsable(false)]
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
			m_UseHook = value;
		}
	}

	bool IOwnerMenuSupport.ShowPopupShadow => true;

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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool HasMenuFocus => MenuFocus;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool MenuFocus
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
			SetMenuFocus(value);
			if (m_MenuFocus)
			{
				if (m_BaseItemContainer is GenericItemContainer)
				{
					((GenericItemContainer)m_BaseItemContainer).SetSystemFocus();
				}
				else if (m_BaseItemContainer is RibbonStripContainerItem)
				{
					((RibbonStripContainerItem)m_BaseItemContainer).SetSystemFocus();
				}
				SetupActiveWindowTimer();
			}
			else
			{
				ReleaseActiveWindowTimer();
				if (m_BaseItemContainer is GenericItemContainer)
				{
					((GenericItemContainer)m_BaseItemContainer).AutoExpand = false;
					((GenericItemContainer)m_BaseItemContainer).ReleaseSystemFocus();
					((GenericItemContainer)m_BaseItemContainer).ContainerLostFocus(appLostFocus: false);
				}
				else if (m_BaseItemContainer is RibbonStripContainerItem)
				{
					((RibbonStripContainerItem)m_BaseItemContainer).AutoExpand = false;
					((RibbonStripContainerItem)m_BaseItemContainer).ReleaseSystemFocus();
					((RibbonStripContainerItem)m_BaseItemContainer).ContainerLostFocus(appLostFocus: false);
				}
			}
			((Control)this).Refresh();
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool HasRegisteredPopups => m_RegisteredPopups.Count > 0;

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

	[Browsable(false)]
	[DefaultValue(false)]
	public bool EnableGestureNotifyHandler
	{
		get
		{
			return _EnableGestureNotifyHandler;
		}
		set
		{
			_EnableGestureNotifyHandler = value;
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

	[DefaultValue(false)]
	[Browsable(false)]
	public bool ContainerControlProcessDialogKey
	{
		get
		{
			return _ContainerControlProcessDialogKey;
		}
		set
		{
			_ContainerControlProcessDialogKey = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShortcutsEnabled
	{
		get
		{
			return m_ShortcutsEnabled;
		}
		set
		{
			m_ShortcutsEnabled = value;
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates whether control height is set automatically.")]
	[DefaultValue(false)]
	public override bool AutoSize
	{
		get
		{
			return m_AutoSize;
		}
		set
		{
			m_AutoSize = value;
			OnAutoSizeChanged();
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
	public ElementStyle BackgroundStyle
	{
		get
		{
			return m_BackgroundStyle;
		}
		set
		{
			if (value == null)
			{
				throw new InvalidOperationException("Null is not valid value for this property.");
			}
			m_BackgroundStyle = value;
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
				((Control)this).Invalidate();
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool ShowKeyTips
	{
		get
		{
			return m_ShowKeyTips;
		}
		set
		{
			if (m_ShowKeyTips != value)
			{
				m_ShowKeyTips = value;
				OnShowKeyTipsChanged();
			}
		}
	}

	string IKeyTipsControl.KeyTipsKeysStack
	{
		get
		{
			return m_KeyTipsKeysStack;
		}
		set
		{
			m_KeyTipsKeysStack = value;
			if (m_KeyTipsCanvas != null && ((Control)m_KeyTipsCanvas).Parent != null)
			{
				((Control)m_KeyTipsCanvas).Parent.Invalidate(true);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
	public virtual Font KeyTipsFont
	{
		get
		{
			return m_KeyTipsFont;
		}
		set
		{
			m_KeyTipsFont = value;
		}
	}

	internal bool AnimateNextPaintRequest
	{
		get
		{
			return _AnimateNextPaintRequest;
		}
		set
		{
			_AnimateNextPaintRequest = value;
		}
	}

	internal virtual bool IsGlassEnabled
	{
		get
		{
			if (GetDesignMode())
			{
				return false;
			}
			return WinApi.IsGlassEnabled;
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

	protected virtual bool NeedsTopLevelKeyTipCanvasParent => false;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether mouse over fade effect is enabled")]
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
			if (((Component)this).DesignMode || !BarFunctions.IsOffice2007Style(m_BaseItemContainer.EffectiveStyle) || (m_FadeEffect && NativeFunctions.IsTerminalSession()) || TextDrawing.UseTextRenderer || m_BaseItemContainer.EffectiveStyle == eDotNetBarStyle.Office2010 || m_BaseItemContainer.EffectiveStyle == eDotNetBarStyle.Metro)
			{
				return false;
			}
			return m_FadeEffect;
		}
	}

	[Browsable(false)]
	public bool IsUpdateSuspended => m_UpdateSuspendCount > 0;

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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Specifies the Image size that will be used by items on this bar.")]
	[DefaultValue(eBarImageSize.Default)]
	public virtual eBarImageSize ImageSize
	{
		get
		{
			return m_ImageSize;
		}
		set
		{
			m_ImageSize = value;
			if (m_BaseItemContainer is ImageItem)
			{
				((ImageItem)m_BaseItemContainer).RefreshImageSize();
			}
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	public virtual bool AllowExternalDrop
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

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
	public virtual bool UseNativeDragDrop
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

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether internal automatic drag & drop support is enabled")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	protected virtual bool DragDropSupport
	{
		get
		{
			return m_DragDropSupport;
		}
		set
		{
			m_DragDropSupport = value;
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

	[Description("Occurs before an item drag & drop operation is started and allows cancellation")]
	public event CancelEventHandler BeforeItemDrag;

	public event EventHandler ItemLayoutUpdated;

	public ItemControl()
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		BaseRenderer renderer = GlobalManager.Renderer;
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
		((Control)this).SetStyle((ControlStyles)2048, true);
		m_MoveCursor = null;
		m_NACursor = null;
		m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		m_BackgroundStyle.SetColorScheme(m_ColorScheme);
		m_BackgroundStyle.StyleChanged += VisualPropertyChanged;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public void SetDesignMode(bool mode)
	{
		m_DesignModeInternal = mode;
		m_BaseItemContainer.SetDesignMode(mode);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new ItemControlAccessibleObject(this);
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
	}

	void IOwner.OnApplicationDeactivate()
	{
		ClosePopups();
		if (m_BaseItemContainer is GenericItemContainer)
		{
			((GenericItemContainer)m_BaseItemContainer).ContainerLostFocus(appLostFocus: true);
		}
		else if (m_BaseItemContainer is RibbonStripContainerItem)
		{
			((RibbonStripContainerItem)m_BaseItemContainer).ContainerLostFocus(appLostFocus: true);
		}
	}

	void IOwner.OnParentPositionChanging()
	{
	}

	protected virtual void OnBeforeItemDrag(BaseItem itemSource, CancelEventArgs e)
	{
		this.BeforeItemDrag?.Invoke(itemSource, e);
	}

	public void StartItemDrag(BaseItem item)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (!m_DragDropSupport)
		{
			return;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeItemDrag(item, cancelEventArgs);
		if (cancelEventArgs.Cancel || m_DragItem != null)
		{
			return;
		}
		m_DragItem = item;
		Point dragStartPoint = ((Control)this).PointToClient(Control.MousePosition);
		dragStartPoint.X -= m_DragItem.LeftInternal;
		dragStartPoint.Y -= m_DragItem.TopInternal;
		if (dragStartPoint.IsEmpty)
		{
			dragStartPoint.Offset(1, 1);
		}
		m_DragItem.DragStartPoint = dragStartPoint;
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
			DragInProgress = true;
		}
		else
		{
			DragInProgress = true;
			((Control)this).DoDragDrop((object)item, (DragDropEffects)(-2147483641));
		}
	}

	protected virtual void OnDragInProgressChanged()
	{
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
		((ContainerControl)this).OnParentChanged(e);
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

	internal void InstallMessageFilter()
	{
		if (!m_FilterInstalled)
		{
			MessageHandler.RegisterMessageClient(this);
			m_FilterInstalled = true;
		}
	}

	void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
	{
		if (m_RegisteredPopups.Contains(objPopup))
		{
			return;
		}
		if (!GetDesignMode() && !m_UseHook)
		{
			InstallMessageFilter();
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

	void IOwnerMenuSupport.ClosePopups()
	{
		ClosePopups();
	}

	internal void ClosePopups()
	{
		ArrayList arrayList = new ArrayList(m_RegisteredPopups);
		foreach (PopupItem item in arrayList)
		{
			item.ClosePopup();
		}
	}

	internal void ClosePopup(string popupName)
	{
		ArrayList arrayList = new ArrayList(m_RegisteredPopups);
		foreach (PopupItem item in arrayList)
		{
			if (item.Name == popupName)
			{
				item.ClosePopup();
			}
		}
	}

	internal void SetMenuFocus(bool focus)
	{
		m_MenuFocus = focus;
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
		if (m_BaseItemContainer is GenericItemContainer)
		{
			((GenericItemContainer)m_BaseItemContainer).AutoExpand = false;
		}
	}

	protected void CopyIOwnerEvents(ItemControl target)
	{
		target.ItemClick = this.ItemClick;
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

	protected virtual void OnItemMouseDown(BaseItem item, MouseEventArgs e)
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

	void IOwnerItemEvents.InvokeMouseDown(BaseItem item, MouseEventArgs e)
	{
		OnItemMouseDown(item, e);
	}

	protected virtual void OnItemMouseUp(BaseItem item, MouseEventArgs e)
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

	void IOwnerItemEvents.InvokeMouseUp(BaseItem item, MouseEventArgs e)
	{
		OnItemMouseUp(item, e);
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
		OnItemClick(objItem);
	}

	protected virtual void OnItemClick(BaseItem item)
	{
		GetRibbonControl()?.OnChildItemClick(item);
		if (ShowKeyTips)
		{
			ShowKeyTips = false;
		}
		if (this.ItemClick != null)
		{
			this.ItemClick(item, new EventArgs());
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

	protected virtual void OnButtonCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (this.ButtonCheckedChanged != null)
		{
			this.ButtonCheckedChanged(item, e);
		}
	}

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem item, EventArgs e)
	{
		OnButtonCheckedChanged(item, e);
	}

	bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return OnMouseWheel(hWnd, wParam, lParam);
	}

	protected virtual bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return OnKeyDown(hWnd, wParam, lParam);
	}

	protected virtual bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected I4, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		bool designMode = GetDesignMode();
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
		if (MenuFocus && !designMode)
		{
			bool flag3 = true;
			Control val3 = Control.FromChildHandle(hWnd);
			if (val3 != null)
			{
				while (val3.Parent != null)
				{
					val3 = val3.Parent;
				}
				if ((val3 is MenuPanel || val3 is ItemControl || val3 is PopupContainer || val3 is PopupContainerControl) && val3.Handle != hWnd)
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
				ExKeyDown(new KeyEventArgs(val4));
				return !designMode;
			}
		}
		if (!IsParentFormActive)
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

	private bool ProcessShortcut(eShortcut key)
	{
		if (!m_ShortcutsEnabled || !((Control)this).Enabled)
		{
			return false;
		}
		Form val = ((Control)this).FindForm();
		if (val == null || (((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)) && !val.IsMdiContainer) || (Form.ActiveForm != null && Form.ActiveForm.Modal && Form.ActiveForm != val))
		{
			return false;
		}
		bool flag = BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
		return !m_DispatchShortcuts && flag;
	}

	private PopupDelayedClose GetDelayClose()
	{
		if (m_DelayClose == null)
		{
			m_DelayClose = new PopupDelayedClose();
		}
		return m_DelayClose;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DesignerNewItemAdded()
	{
		GetDelayClose().EraseDelayClose();
	}

	protected virtual bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (m_RegisteredPopups.Count == 0 || GetDesignMode())
		{
			return false;
		}
		BaseItem[] array = new BaseItem[m_RegisteredPopups.Count];
		m_RegisteredPopups.CopyTo(array);
		for (int num = array.Length - 1; num >= 0; num--)
		{
			PopupItem popupItem = array[num] as PopupItem;
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
						if (!flag)
						{
							flag = popupItem.IsAnyOnHandle(val.Handle);
						}
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
				if (val2 != null && val2.Parent != null)
				{
					val2 = val2.Parent;
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
			if (popupItem.Displayed && !((Control)this).IsDisposed)
			{
				Point pt = ((Control)this).PointToClient(Control.MousePosition);
				if (popupItem.DisplayRectangle.Contains(pt))
				{
					break;
				}
			}
			if (popupItem.PopupControl is IKeyTipsControl)
			{
				((IKeyTipsControl)popupItem.PopupControl).ShowKeyTips = false;
			}
			if (GetDesignMode())
			{
				GetDelayClose().DelayClose(popupItem);
			}
			else
			{
				popupItem.ClosePopup();
			}
			if (m_RegisteredPopups.Count == 0)
			{
				break;
			}
		}
		if (m_RegisteredPopups.Count == 0)
		{
			MenuFocus = false;
		}
		return false;
	}

	bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return OnSysMouseDown(hWnd, wParam, lParam);
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
		return OnSysKeyDown(hWnd, wParam, lParam);
	}

	protected virtual bool OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected I4, but got Unknown
		if (!GetDesignMode())
		{
			int num = WinApi.ToInt(wParam);
			if (num == 18 && (int)Control.ModifierKeys == 262144)
			{
				ClosePopups();
			}
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
		return OnSysKeyUp(hWnd, wParam, lParam);
	}

	protected virtual bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
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
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		if (((Message)(ref m)).Msg == 7)
		{
			IntPtr wParam = ((Message)(ref m)).WParam;
			Control val = Control.FromChildHandle(wParam);
			if (val != null && !(val is Bar) && !(val is MenuPanel) && !(val is TextBoxX) && !(val is ComboBoxEx))
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
		else if (((Message)(ref m)).Msg == 1131)
		{
			if (m_DoDefaultActionItem != null)
			{
				m_DoDefaultActionItem.DoAccesibleDefaultAction();
				m_DoDefaultActionItem = null;
			}
		}
		else if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		else if ((long)((Message)(ref m)).Msg == 282 && _EnableGestureNotifyHandler)
		{
			Stimulsoft.Controls.Win.DotNetBar.Touch.WinApi.GESTURENOTIFYSTRUCT gESTURENOTIFYSTRUCT = (Stimulsoft.Controls.Win.DotNetBar.Touch.WinApi.GESTURENOTIFYSTRUCT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(Stimulsoft.Controls.Win.DotNetBar.Touch.WinApi.GESTURENOTIFYSTRUCT));
			Point point = ((Control)this).PointToClient(new Point(gESTURENOTIFYSTRUCT.PtsLocation.x, gESTURENOTIFYSTRUCT.PtsLocation.y));
			m_BaseItemContainer.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, point.X, point.Y, 0));
			m_BaseItemContainer.InternalMouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		((ContainerControl)this).WndProc(ref m);
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
		RecalcLayout();
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
		if (m_Hook != null)
		{
			m_Hook.Dispose();
			m_Hook = null;
		}
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Point mousePos = ((Control)this).PointToClient(Control.MousePosition);
		InternalOnClick(Control.MouseButtons, mousePos);
		((Control)this).OnClick(e);
	}

	protected virtual void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_BaseItemContainer.InternalClick(mb, mousePos);
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

	internal virtual void ExKeyDown(KeyEventArgs e)
	{
		m_BaseItemContainer.InternalKeyDown(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		EventMouseEnter?.Invoke(this, e);
		((Control)this).OnMouseEnter(e);
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
		InternalMouseMove(e);
	}

	protected virtual void InternalMouseMove(MouseEventArgs e)
	{
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (m_DragDropSupport && (m_DragInProgress || m_ExternalDragInProgress))
		{
			e.Effect = (DragDropEffects)2;
			((Control)this).OnDragOver(e);
			if ((int)e.Effect != 0)
			{
				Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
				MouseDragOver(point.X, point.Y, e);
			}
			m_DragLeft = false;
		}
		else
		{
			((Control)this).OnDragOver(e);
		}
	}

	protected override void OnDragLeave(EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (m_DragDropSupport)
		{
			if (m_DragInProgress || m_ExternalDragInProgress)
			{
				if ((int)Control.MouseButtons == 0)
				{
					MouseDragDrop(-1, -1, null);
				}
				else
				{
					MouseDragOver(-1, -1, null);
				}
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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		((Control)this).OnQueryContinueDrag(e);
		if (m_DragDropSupport && m_DragInProgress && ((m_DragLeft && (int)e.Action == 1) || (int)e.Action == 2))
		{
			MouseDragDrop(-1, -1, null);
		}
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
		InsertPosition insertPosition = ((IDesignTimeProvider)m_BaseItemContainer).GetInsertPosition(pScreen, baseItem);
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
			}
			else
			{
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
			}
		}
		else if (!m_UseNativeDragDrop)
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
		OnMouseDragOverProcessed(x, y, dragArgs);
	}

	protected virtual void OnMouseDragOverProcessed(int x, int y, DragEventArgs dragArgs)
	{
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (((Control)this).Visible && ProcessMnemonicEx(charCode))
		{
			return true;
		}
		return ((ContainerControl)this).ProcessMnemonic(charCode);
	}

	public virtual bool ProcessMnemonicEx(char charCode)
	{
		if (ProcessMnemonic(m_BaseItemContainer, charCode))
		{
			return true;
		}
		return false;
	}

	protected virtual bool ProcessMnemonic(BaseItem container, char charCode)
	{
		BaseItem itemForMnemonic = GetItemForMnemonic(container, charCode, deepScan: true, stackKeys: true);
		return ProcessItemMnemonicKey(itemForMnemonic);
	}

	protected virtual bool ProcessItemMnemonicKey(BaseItem item)
	{
		if (item != null && item.Visible && item.GetEnabled())
		{
			if (item is QatOverflowItem)
			{
				((QatOverflowItem)item).Expanded = true;
			}
			else if (item is ButtonItem && (item.ShowSubItems || ((ButtonItem)item).AutoExpandOnClick) && ((item.SubItems.Count > 0 && item.VisibleSubItems > 0) || ((ButtonItem)item).PopupType == ePopupType.Container) && !item.Expanded)
			{
				item.Expanded = true;
				PopupItem popupItem = item as PopupItem;
				if (popupItem.PopupType == ePopupType.Menu && popupItem.PopupControl is MenuPanel)
				{
					((MenuPanel)(object)popupItem.PopupControl).SelectFirstItem();
				}
				if (m_BaseItemContainer is GenericItemContainer)
				{
					m_BaseItemContainer.AutoExpand = true;
				}
			}
			else if (item is ComboBoxItem)
			{
				((Control)((ComboBoxItem)item).ComboBoxEx).Focus();
				((ComboBox)((ComboBoxItem)item).ComboBoxEx).DroppedDown = true;
			}
			else if (item is TextBoxItem)
			{
				((Control)((TextBoxItem)item).TextBox).Focus();
			}
			else if (item is ControlContainerItem && ((ControlContainerItem)item).Control != null)
			{
				((ControlContainerItem)item).Control.Focus();
			}
			else if (item is GalleryContainer)
			{
				((GalleryContainer)item).PopupGallery();
			}
			else
			{
				item.RaiseClick();
			}
			return true;
		}
		return false;
	}

	protected virtual BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys)
	{
		bool partialMatch = false;
		return GetItemForMnemonic(container, charCode, deepScan, stackKeys, ref partialMatch);
	}

	private BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys, ref bool partialMatch)
	{
		string text = m_KeyTipsKeysStack + charCode;
		text = text.ToUpper();
		CaptionItemContainer captionItemContainer = container as CaptionItemContainer;
		int num = container.SubItems.Count;
		if (captionItemContainer != null && captionItemContainer.MoreItems != null && captionItemContainer.MoreItems.Visible)
		{
			num++;
		}
		BaseItem[] array = new BaseItem[num];
		container.SubItems.CopyTo(array, 0);
		if (captionItemContainer != null && captionItemContainer.MoreItems != null && captionItemContainer.MoreItems.Visible)
		{
			array[num - 1] = captionItemContainer.MoreItems;
		}
		BaseItem[] array2 = array;
		foreach (BaseItem baseItem in array2)
		{
			if (baseItem.KeyTips != "" || m_KeyTipsKeysStack != "")
			{
				if (baseItem.KeyTips != "")
				{
					if (baseItem.KeyTips == text)
					{
						if (baseItem.Visible && baseItem.GetEnabled())
						{
							return baseItem;
						}
					}
					else if (baseItem.KeyTips.StartsWith(text))
					{
						partialMatch = true;
					}
				}
			}
			else if (Control.IsMnemonic(charCode, baseItem.Text) && baseItem.Visible && baseItem.GetEnabled())
			{
				return baseItem;
			}
			if (deepScan && baseItem.IsContainer)
			{
				BaseItem itemForMnemonic = GetItemForMnemonic(baseItem, charCode, deepScan, stackKeys: false, ref partialMatch);
				if (itemForMnemonic != null)
				{
					return itemForMnemonic;
				}
			}
		}
		if (partialMatch && stackKeys)
		{
			((IKeyTipsControl)this).KeyTipsKeysStack += charCode.ToString().ToUpper();
		}
		return null;
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
		}
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (_ContainerControlProcessDialogKey)
		{
			return ((ContainerControl)this).ProcessDialogKey(keyData);
		}
		return false;
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (m_BaseItemContainer != null)
		{
			m_BaseItemContainer.UpdateBindings();
		}
	}

	protected virtual RibbonControl GetRibbonControl()
	{
		Control parent = ((Control)this).Parent;
		while (parent != null && !(parent is RibbonControl))
		{
			if (parent is RibbonPanel && ((RibbonPanel)(object)parent).GetRibbonControl() != null)
			{
				return ((RibbonPanel)(object)parent).GetRibbonControl();
			}
			parent = parent.Parent;
		}
		return parent as RibbonControl;
	}

	public virtual BaseItem HitTest(int x, int y)
	{
		return m_BaseItemContainer.ItemAtLocation(x, y);
	}

	private void OnAutoSizeChanged()
	{
		AutoSyncSize();
	}

	public virtual void AutoSyncSize()
	{
		if (!m_AutoSize || m_SyncingSize > 2 || m_BaseItemContainer == null || m_BaseItemContainer.NeedRecalcSize)
		{
			return;
		}
		m_SyncingSize++;
		try
		{
			int autoSizeHeight = GetAutoSizeHeight();
			if (((Control)this).Height != autoSizeHeight && m_BaseItemContainer.HeightInternal > 0)
			{
				((Control)this).Height = autoSizeHeight;
				RecalcSize();
			}
		}
		finally
		{
			m_SyncingSize--;
		}
	}

	protected virtual ElementStyle GetBackgroundStyle()
	{
		return m_BackgroundStyle;
	}

	internal ElementStyle GetPaintBackgroundStyle()
	{
		return GetBackgroundStyle();
	}

	public virtual int GetAutoSizeHeight()
	{
		return m_BaseItemContainer.HeightInternal + ElementStyleLayout.VerticalStyleWhiteSpace(GetBackgroundStyle());
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
			if (m_DelayClose != null)
			{
				m_DelayClose.Dispose();
				m_DelayClose = null;
			}
			if (m_BaseItemContainer != null)
			{
				m_BaseItemContainer.Dispose();
			}
			if (m_ImageList != null)
			{
				((Component)(object)m_ImageList).Disposed -= ImageListDisposed;
			}
			if (m_ImageListMedium != null)
			{
				((Component)(object)m_ImageListMedium).Disposed -= ImageListDisposed;
			}
			if (m_ImageListLarge != null)
			{
				((Component)(object)m_ImageListLarge).Disposed -= ImageListDisposed;
			}
		}
		((ContainerControl)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		m_BackgroundStyle.StyleChanged -= VisualPropertyChanged;
		m_BackgroundStyle = new ElementStyle();
		m_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		((Control)this).Invalidate();
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		OnVisualPropertyChanged();
	}

	protected virtual void OnVisualPropertyChanged()
	{
		if (GetDesignMode() || (((Control)this).Parent != null && ((Component)(object)((Control)this).Parent).Site != null && ((Component)(object)((Control)this).Parent).Site.DesignMode))
		{
			RecalcLayout();
		}
	}

	protected virtual void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
	{
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
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
						parent.Refresh();
						if (parent.ContainerControl != this)
						{
							object containerControl = parent.ContainerControl;
							BarUtilities.InvokeRecalcLayout((Control)((containerControl is Control) ? containerControl : null));
						}
					}
					m_DesignTimeProvider.InsertItemAt(baseItem, m_InsertPosition, m_InsertBefore);
					m_DesignTimeProvider = null;
					((IOwner)this).InvokeUserCustomize((object)baseItem, new EventArgs());
				}
			}
		}
		m_DesignTimeProvider = null;
		DragInProgress = false;
		m_ExternalDragInProgress = false;
		if (!m_UseNativeDragDrop)
		{
			Cursor.Current = Cursors.Default;
		}
		((Control)this).Capture = false;
		if (baseItem != null)
		{
			baseItem._IgnoreClick = true;
			baseItem.DragStartPoint = Point.Empty;
		}
		m_BaseItemContainer.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, x, y, 0));
		if (baseItem != null)
		{
			baseItem._IgnoreClick = false;
		}
		m_DragItem = null;
		if (this.UserCustomize != null)
		{
			this.UserCustomize(this, new EventArgs());
		}
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

	protected virtual void OnShowKeyTipsChanged()
	{
		m_KeyTipsKeysStack = "";
		if (ShowKeyTips)
		{
			CreateKeyTipCanvas();
		}
		else
		{
			DestroyKeyTipCanvas();
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

	internal virtual ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		itemPaintArgs.DesignerSelection = m_DesignerSelection;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && IsGlassEnabled;
		itemPaintArgs.DragInProgress = DragInProgress;
		itemPaintArgs.AnimationEnabled = _AnimateNextPaintRequest;
		if (_AnimateNextPaintRequest)
		{
			itemPaintArgs.AnimationRequests = new List<AnimationRequest>();
		}
		_AnimateNextPaintRequest = false;
		return itemPaintArgs;
	}

	internal ColorScheme GetColorSchemeInternal()
	{
		return GetColorScheme();
	}

	protected virtual ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(m_BaseItemContainer.EffectiveStyle))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
			}
		}
		return m_ColorScheme;
	}

	internal void InternalPaint(PaintEventArgs e)
	{
		OnPaint(e, cashedPaint: true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		OnPaint(e, cashedPaint: false);
		if (m_KeyTipsCanvas != null)
		{
			InvalidateKeyTipsCanvas();
			InvalidateKeyTipsDelayed();
		}
		((Control)this).OnPaint(e);
	}

	private void InvalidateKeyTipsDelayed()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (m_KeyTipDelayRefreshTimer == null)
		{
			m_KeyTipDelayRefreshTimer = new Timer();
			m_KeyTipDelayRefreshTimer.Tick += KeyTipDelayRefreshTimerTick;
			m_KeyTipDelayRefreshTimer.Interval = 100;
			m_KeyTipDelayRefreshTimer.Start();
		}
	}

	private void KeyTipDelayRefreshTimerTick(object sender, EventArgs e)
	{
		Timer keyTipDelayRefreshTimer = m_KeyTipDelayRefreshTimer;
		m_KeyTipDelayRefreshTimer = null;
		keyTipDelayRefreshTimer.Stop();
		keyTipDelayRefreshTimer.Tick -= KeyTipDelayRefreshTimerTick;
		((Component)(object)keyTipDelayRefreshTimer).Dispose();
		InvalidateKeyTipsCanvas();
	}

	internal void InvalidateKeyTipsCanvas()
	{
		if (m_KeyTipsCanvas != null)
		{
			((Control)m_KeyTipsCanvas).Invalidate();
		}
	}

	private void OnPaint(PaintEventArgs e, bool cashedPaint)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		if (((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent)
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		if (m_BackgroundStyle != null)
		{
			m_BackgroundStyle.SetColorScheme(GetColorScheme());
		}
		ItemPaintArgs itemPaintArgs = GetItemPaintArgs(e.Graphics);
		itemPaintArgs.PaintEventArgs = e;
		itemPaintArgs.ClipRectangle = e.ClipRectangle;
		if (_CanAnimate)
		{
			itemPaintArgs.AnimationEnabled = true;
			itemPaintArgs.AnimationRequests = new List<AnimationRequest>();
		}
		if (m_FirstScrollPaint && ((ScrollableControl)this).AutoScroll && ((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			itemPaintArgs.ClipRectangle = Rectangle.Empty;
			m_FirstScrollPaint = false;
		}
		itemPaintArgs.CachedPaint = cashedPaint;
		PaintControl(itemPaintArgs);
		e.Graphics.SmoothingMode = smoothingMode;
		e.Graphics.TextRenderingHint = textRenderingHint;
		if (itemPaintArgs.AnimationEnabled && itemPaintArgs.AnimationRequests.Count > 0)
		{
			AnimationRectangle animationRectangle = new AnimationRectangle(itemPaintArgs.AnimationRequests.ToArray(), AnimationEasing.EaseOutExpo, 500);
			animationRectangle.AnimationUpdateControl = (Control)(object)this;
			_CanAnimate = false;
			animationRectangle.AnimationCompleted += AnimationCompleted;
			animationRectangle.Start();
		}
	}

	private void AnimationCompleted(object sender, EventArgs e)
	{
		Stimulsoft.Controls.Win.DotNetBar.Animation.Animation animation = (Stimulsoft.Controls.Win.DotNetBar.Animation.Animation)sender;
		animation.AnimationCompleted -= AnimationCompleted;
		animation.Dispose();
	}

	protected virtual void ClearBackground(ItemPaintArgs pa)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			pa.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void PaintControl(ItemPaintArgs pa)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (m_BaseItemContainer != null && !((Control)this).IsDisposed)
		{
			if (m_BaseItemContainer.NeedRecalcSize)
			{
				RecalcSize();
			}
			if (!((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
			{
				ClearBackground(pa);
			}
			if (m_AntiAlias)
			{
				pa.Graphics.SmoothingMode = (SmoothingMode)4;
				pa.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			PaintControlBackground(pa);
			Region clip = pa.Graphics.Clip;
			pa.Graphics.SetClip(GetPaintClipRectangle(), (CombineMode)1);
			PaintItemContainer(pa);
			pa.Graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
			if (m_BaseItemContainer.SubItems.Count == 0 && GetDesignMode())
			{
				PaintDesignTimeEmptyHint(pa);
			}
		}
	}

	protected virtual Rectangle GetPaintClipRectangle()
	{
		return GetItemContainerRectangle();
	}

	protected virtual Rectangle GetPaintControlBackgroundRectangle()
	{
		return ((Control)this).ClientRectangle;
	}

	protected virtual void PaintControlBackground(ItemPaintArgs pa)
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (backgroundStyle != null)
		{
			Rectangle paintControlBackgroundRectangle = GetPaintControlBackgroundRectangle();
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(backgroundStyle, pa.Graphics, paintControlBackgroundRectangle);
			ElementStyleDisplay.Paint(e);
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
			StringFormat val = BarFunctions.CreateStringFormat();
			val.Alignment = (StringAlignment)1;
			val.LineAlignment = (StringAlignment)1;
			pa.Graphics.DrawString(emptyContainerDesignTimeHint, ((Control)this).Font, SystemBrushes.ControlDark, (RectangleF)itemContainerRectangle, val);
		}
	}

	protected virtual void PaintKeyTips(Graphics g)
	{
		if (m_ShowKeyTips)
		{
			KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer renderer = GetRenderer();
			PaintContainerKeyTips(m_BaseItemContainer, renderer, e);
		}
	}

	protected virtual Font GetKeyTipFont()
	{
		Font result = ((Control)this).Font;
		if (m_KeyTipsFont != null)
		{
			result = m_KeyTipsFont;
		}
		return result;
	}

	internal virtual void PaintContainerKeyTips(BaseItem container, BaseRenderer renderer, KeyTipsRendererEventArgs e)
	{
		foreach (BaseItem subItem in container.SubItems)
		{
			PaintItemKeyTip(subItem, renderer, e);
		}
		if (container is CaptionItemContainer { MoreItems: not null } captionItemContainer && captionItemContainer.MoreItems.Visible)
		{
			PaintItemKeyTip(captionItemContainer.MoreItems, renderer, e);
		}
	}

	private void PaintItemKeyTip(BaseItem item, BaseRenderer renderer, KeyTipsRendererEventArgs e)
	{
		if (!item.Visible || !item.Displayed)
		{
			return;
		}
		if (item.IsContainer)
		{
			PaintContainerKeyTips(item, renderer, e);
		}
		if ((item.AccessKey != 0 || !(item.KeyTips == "")) && (!(m_KeyTipsKeysStack != "") || item.KeyTips.StartsWith(m_KeyTipsKeysStack)) && (!(item.KeyTips == "") || !(m_KeyTipsKeysStack != "")))
		{
			if (item.KeyTips != "")
			{
				e.KeyTip = item.KeyTips;
			}
			else
			{
				e.KeyTip = item.AccessKey.ToString().ToUpper();
			}
			e.Bounds = GetKeyTipRectangle(e.Graphics, item, e.Font, e.KeyTip);
			e.ReferenceObject = item;
			renderer.DrawKeyTips(e);
		}
	}

	protected virtual Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Size keyTipsPadding = KeyTipsPainter.KeyTipsPadding;
		Size size = TextDrawing.MeasureString(g, keyTip, font);
		size.Width += keyTipsPadding.Width;
		size.Height += keyTipsPadding.Height;
		Rectangle displayRectangle = item.DisplayRectangle;
		Rectangle result = new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Bottom - size.Height, size.Width, size.Height);
		if (item is RibbonTabItem)
		{
			result.Y = displayRectangle.Bottom - 7;
		}
		return result;
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

	protected virtual Rectangle GetKeyTipCanvasBounds()
	{
		if (((Control)this).Parent != null)
		{
			return new Rectangle(((Control)this).Bounds.X, ((Control)this).Bounds.Y, ((Control)this).Bounds.Width, ((Control)this).Bounds.Height + 16);
		}
		return new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
	}

	protected virtual void CreateKeyTipCanvas()
	{
		if (m_KeyTipsCanvas != null)
		{
			((Control)m_KeyTipsCanvas).BringToFront();
			if (((Control)m_KeyTipsCanvas).Parent != null)
			{
				((Control)m_KeyTipsCanvas).Parent.Invalidate(((Control)m_KeyTipsCanvas).Bounds);
			}
			return;
		}
		m_KeyTipsCanvas = new KeyTipsCanvasControl(this);
		((Control)m_KeyTipsCanvas).Bounds = GetKeyTipCanvasBounds();
		((Control)m_KeyTipsCanvas).Visible = true;
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			if (ribbonControl.Expanded && !NeedsTopLevelKeyTipCanvasParent)
			{
				((Control)ribbonControl).Controls.Add((Control)(object)m_KeyTipsCanvas);
			}
			else
			{
				Form val = ((Control)ribbonControl).FindForm();
				if (val != null)
				{
					((Control)val).Controls.Add((Control)(object)m_KeyTipsCanvas);
				}
				else
				{
					((Control)ribbonControl).Controls.Add((Control)(object)m_KeyTipsCanvas);
				}
			}
		}
		else
		{
			((Control)this).Controls.Add((Control)(object)m_KeyTipsCanvas);
		}
		((Control)m_KeyTipsCanvas).BringToFront();
	}

	internal void UpdateKeyTipsCanvas()
	{
		if (m_KeyTipsCanvas != null)
		{
			((Control)m_KeyTipsCanvas).Refresh();
		}
	}

	protected virtual void DestroyKeyTipCanvas()
	{
		if (m_KeyTipsCanvas != null)
		{
			((Control)m_KeyTipsCanvas).Visible = false;
			if (((Control)m_KeyTipsCanvas).Parent != null)
			{
				((Control)m_KeyTipsCanvas).Parent.Controls.Remove((Control)(object)m_KeyTipsCanvas);
			}
			((Component)(object)m_KeyTipsCanvas).Dispose();
			m_KeyTipsCanvas = null;
		}
	}

	void IKeyTipsRenderer.PaintKeyTips(Graphics g)
	{
		PaintKeyTips(g);
	}

	public void BeginUpdate()
	{
		m_UpdateSuspendCount++;
	}

	public void EndUpdate()
	{
		EndUpdate(callRecalcLayout: true);
	}

	public void EndUpdate(bool callRecalcLayout)
	{
		if (m_UpdateSuspendCount > 0)
		{
			m_UpdateSuspendCount--;
			if (m_UpdateSuspendCount == 0 && callRecalcLayout)
			{
				RecalcLayout();
			}
		}
	}

	protected virtual Rectangle GetItemContainerRectangle()
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (backgroundStyle == null)
		{
			return ((Control)this).ClientRectangle;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ElementStyleLayout.LeftWhiteSpace(backgroundStyle);
		clientRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(backgroundStyle);
		clientRectangle.Y += ElementStyleLayout.TopWhiteSpace(backgroundStyle);
		clientRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
		return clientRectangle;
	}

	protected virtual void RecalcSize()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		if (!BarFunctions.IsHandleValid((Control)(object)this) || IsUpdateSuspended || m_BaseItemContainer.IsRecalculatingSize)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val == null || (int)val.WindowState != 1)
		{
			if (m_BaseItemContainer.NeedRecalcSize)
			{
				Rectangle itemContainerRectangle = GetItemContainerRectangle();
				m_BaseItemContainer.SuspendLayout = true;
				m_BaseItemContainer.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
				m_BaseItemContainer.LeftInternal = itemContainerRectangle.X;
				m_BaseItemContainer.TopInternal = itemContainerRectangle.Y;
				m_BaseItemContainer.WidthInternal = itemContainerRectangle.Width;
				m_BaseItemContainer.HeightInternal = itemContainerRectangle.Height;
				m_BaseItemContainer.SuspendLayout = false;
				m_BaseItemContainer.RecalcSize();
				OnItemLayoutUpdated(EventArgs.Empty);
			}
			AutoSyncSize();
		}
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		this.ItemLayoutUpdated?.Invoke(this, e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		InvalidateFontChange();
		InvalidateLayout();
		RecalcLayout();
		((ContainerControl)this).OnFontChanged(e);
	}

	private void InvalidateFontChange()
	{
		BarUtilities.InvalidateFontChange(m_BaseItemContainer.SubItems);
	}

	public void InvalidateLayout()
	{
		m_BaseItemContainer.NeedRecalcSize = true;
	}

	public virtual void RecalcLayout()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if (((Control)this).InvokeRequired)
		{
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				RecalcLayout();
			});
		}
		else if (!m_BaseItemContainer.IsRecalculatingSize && !IsUpdateSuspended)
		{
			RecalcSize();
			((Control)this).Invalidate();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			Form val = ((Control)this).FindForm();
			if (val == null || (int)val.WindowState != 1)
			{
				m_BaseItemContainer.NeedRecalcSize = true;
				RecalcSize();
			}
		}
	}

	protected virtual void SetupActiveWindowTimer()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (m_ActiveWindowTimer == null)
		{
			m_ActiveWindowTimer = new Timer();
			m_ActiveWindowTimer.Interval = 100;
			m_ActiveWindowTimer.Tick += ActiveWindowTimer_Tick;
			m_ForegroundWindow = NativeFunctions.GetForegroundWindow();
			m_ActiveWindow = NativeFunctions.GetActiveWindow();
			m_ActiveWindowTimer.Start();
		}
	}

	private void ActiveWindowTimer_Tick(object sender, EventArgs e)
	{
		if (m_ActiveWindowTimer == null)
		{
			return;
		}
		IntPtr foregroundWindow = NativeFunctions.GetForegroundWindow();
		IntPtr activeWindow = NativeFunctions.GetActiveWindow();
		if (!(foregroundWindow != m_ForegroundWindow) && !(activeWindow != m_ActiveWindow))
		{
			return;
		}
		if (activeWindow != IntPtr.Zero)
		{
			Control val = Control.FromHandle(activeWindow);
			if (val is PopupContainer || val is PopupContainerControl)
			{
				return;
			}
		}
		m_ActiveWindowTimer.Stop();
		OnActiveWindowChanged();
	}

	protected virtual void OnActiveWindowChanged()
	{
		if (MenuFocus)
		{
			MenuFocus = false;
		}
	}

	protected virtual void ReleaseActiveWindowTimer()
	{
		if (m_ActiveWindowTimer != null)
		{
			Timer activeWindowTimer = m_ActiveWindowTimer;
			m_ActiveWindowTimer = null;
			activeWindowTimer.Stop();
			activeWindowTimer.Tick -= ActiveWindowTimer_Tick;
			((Component)(object)activeWindowTimer).Dispose();
		}
	}

	void IRibbonCustomize.ItemRightClick(BaseItem item)
	{
		OnPopupItemRightClick(item);
	}

	protected virtual void OnPopupItemRightClick(BaseItem item)
	{
	}
}
