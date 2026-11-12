using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public abstract class PopupItemControl : Control, IThemeCache, IMessageHandlerClient, IOwnerMenuSupport, IOwner, IRenderingSupport, IAccessibilitySupport, IOwnerLocalize
{
	private PopupItem m_PopupItem;

	private ColorScheme m_ColorScheme;

	private bool m_AntiAlias;

	private bool m_FilterInstalled;

	private bool m_MenuEventSupport;

	private bool m_MenuFocus;

	private Timer m_ActiveWindowTimer;

	private IntPtr m_ForegroundWindow = IntPtr.Zero;

	private IntPtr m_ActiveWindow = IntPtr.Zero;

	private Hashtable m_ShortcutTable = new Hashtable();

	private BaseItem m_ExpandedItem;

	private BaseItem m_FocusItem;

	private ImageList m_ImageList;

	private ImageList m_ImageListMedium;

	private ImageList m_ImageListLarge;

	private bool m_DisabledImagesGrayScale = true;

	private ThemeWindow m_ThemeWindow;

	private ThemeRebar m_ThemeRebar;

	private ThemeToolbar m_ThemeToolbar;

	private ThemeHeader m_ThemeHeader;

	private ThemeScrollBar m_ThemeScrollBar;

	private ThemeExplorerBar m_ThemeExplorerBar;

	private ThemeProgress m_ThemeProgress;

	private ThemeButton m_ThemeButton;

	private BaseItem m_DoDefaultActionItem;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private bool m_UseHook;

	private Hook m_Hook;

	private ArrayList m_RegisteredPopups = new ArrayList();

	private PopupDelayedClose m_DelayClose;

	[Category("Appearance")]
	[Description("Specifies the visual style of the button.")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	public virtual eDotNetBarStyle Style
	{
		get
		{
			return m_PopupItem.Style;
		}
		set
		{
			m_ColorScheme.Style = value;
			m_PopupItem.Style = value;
			OnDotNetBarStyleChanged();
			RecalcLayout();
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
				InvalidateAutoSize();
				((Control)this).Invalidate();
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
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether button is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return m_PopupItem.ThemeAware;
		}
		set
		{
			m_PopupItem.ThemeAware = value;
			RecalcLayout();
		}
	}

	protected bool IsThemed
	{
		get
		{
			if (ThemeAware && m_PopupItem.EffectiveStyle != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && Themes.ThemesActive)
			{
				return true;
			}
			return false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BaseItem InternalItem => m_PopupItem;

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

	internal bool MenuFocus
	{
		get
		{
			return m_MenuFocus;
		}
		set
		{
			if (m_MenuFocus != value)
			{
				m_MenuFocus = value;
				if (m_MenuFocus)
				{
					SetupActiveWindowTimer();
				}
				else
				{
					ReleaseActiveWindowTimer();
					ClosePopups();
				}
				((Control)this).Invalidate();
			}
		}
	}

	bool IMessageHandlerClient.IsModal
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val != null && val.Modal && Form.ActiveForm == val)
			{
				return true;
			}
			return false;
		}
	}

	protected virtual bool EatMenuFilterKeyDown => true;

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
			return true;
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

	bool IOwner.DragInProgress => false;

	BaseItem IOwner.DragItem => null;

	[Browsable(false)]
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

	[Browsable(false)]
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

	[Browsable(false)]
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

	bool IOwner.ShowToolTips
	{
		get
		{
			return true;
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

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
	public virtual bool DisabledImagesGrayScale
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

	public bool LockClosePopup
	{
		get
		{
			return m_PopupItem.LockClosePopup;
		}
		set
		{
			m_PopupItem.LockClosePopup = value;
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

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

	public PopupItemControl()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		PainterFactory.InitFactory();
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		m_PopupItem = CreatePopupItem();
		m_PopupItem.GlobalItem = false;
		m_PopupItem.ContainerControl = this;
		m_PopupItem.Style = eDotNetBarStyle.Office2007;
		m_PopupItem.SetOwner(this);
		m_ColorScheme = new ColorScheme(m_PopupItem.EffectiveStyle);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && m_PopupItem != null)
		{
			m_PopupItem.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	protected abstract PopupItem CreatePopupItem();

	internal void InternalAccessibilityNotifyClients(AccessibleEvents accEvent, int childID)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).AccessibilityNotifyClients(accEvent, childID);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (m_PopupItem != null)
		{
			m_PopupItem.UpdateBindings();
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		if (m_PopupItem != null)
		{
			BarUtilities.InvalidateFontChange(m_PopupItem);
		}
		((Control)this).OnFontChanged(e);
		RecalcLayout();
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (m_PopupItem != null)
		{
			m_PopupItem.Enabled = ((Control)this).Enabled;
		}
		((Control)this).OnEnabledChanged(e);
	}

	protected internal virtual ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(Style))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
			}
		}
		return m_ColorScheme;
	}

	public void RecalcLayout()
	{
		RecalcSize();
		((Control)this).Invalidate();
	}

	protected abstract void RecalcSize();

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode(bool value)
	{
		if (m_PopupItem != null)
		{
			m_PopupItem.SetDesignMode(value);
		}
	}

	protected virtual void OnDotNetBarStyleChanged()
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

	internal virtual ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		itemPaintArgs.ButtonStringFormat &= ~(itemPaintArgs.ButtonStringFormat & eTextFormat.SingleLine);
		itemPaintArgs.ButtonStringFormat |= eTextFormat.EndEllipsis | eTextFormat.WordBreak;
		return itemPaintArgs;
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

	protected virtual void InvalidateAutoSize()
	{
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private bool ShouldSerializeColorScheme()
	{
		return m_ColorScheme.SchemeChanged;
	}

	private bool GetDesignMode()
	{
		return ((Component)this).DesignMode;
	}

	void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
	{
		MenuFocus = true;
		if (m_RegisteredPopups.Contains(objPopup))
		{
			return;
		}
		if (!GetDesignMode() && !m_UseHook)
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
			MenuFocus = false;
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
		Control val = Control.FromChildHandle(activeWindow);
		if (val != null)
		{
			do
			{
				if (val is MenuPanel || val is Bar || val is PopupContainer || val is PopupContainerControl)
				{
					return;
				}
				val = val.Parent;
			}
			while (val != null && val.Parent != null);
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
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected I4, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
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
				KeyEventArgs val3 = new KeyEventArgs((Keys)(val2 | Control.ModifierKeys));
				popupItem.InternalKeyDown(val3);
				if (val3.Handled)
				{
					flag2 = false;
				}
				if (!EatMenuFilterKeyDown)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				return false;
			}
			return true;
		}
		if (MenuFocus)
		{
			bool flag3 = true;
			Control val4 = Control.FromChildHandle(hWnd);
			if (val4 != null)
			{
				while (val4.Parent != null)
				{
					val4 = val4.Parent;
				}
				if ((val4 is MenuPanel || val4 is ItemControl || val4 is PopupContainer || val4 is PopupContainerControl) && val4.Handle != hWnd)
				{
					flag3 = false;
				}
			}
			if (flag3)
			{
				Keys val5 = (Keys)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
				if ((int)val5 == 0)
				{
					val5 = (Keys)num;
				}
				m_PopupItem.InternalKeyDown(new KeyEventArgs(val5));
				return true;
			}
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
		foreach (eShortcut shortcut in m_PopupItem.Shortcuts)
		{
			if (shortcut == key)
			{
				PerformClick();
				return true;
			}
		}
		return BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
	}

	public abstract void PerformClick();

	private PopupDelayedClose GetDelayClose()
	{
		if (m_DelayClose == null)
		{
			m_DelayClose = new PopupDelayedClose();
		}
		return m_DelayClose;
	}

	internal void DesignerNewItemAdded()
	{
		GetDelayClose().EraseDelayClose();
	}

	bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (m_RegisteredPopups.Count == 0)
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
		if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		else if (((Message)(ref m)).Msg == 1131 && m_DoDefaultActionItem != null)
		{
			if (!m_DoDefaultActionItem._AccessibleExpandAction)
			{
				PerformClick();
			}
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
		if (((Control)this).AutoSize)
		{
			AdjustSize();
		}
		RecalcLayout();
	}

	protected virtual void AdjustSize()
	{
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

	ArrayList IOwner.GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(m_PopupItem, ItemName, arrayList);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(m_PopupItem, ItemName, arrayList, itemType);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(m_PopupItem, ItemName, arrayList, itemType, useGlobalName);
		return arrayList;
	}

	BaseItem IOwner.GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(m_PopupItem, ItemName);
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

	void IOwner.StartItemDrag(BaseItem item)
	{
	}

	void IOwner.InvokeUserCustomize(object sender, EventArgs e)
	{
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
		if (((Component)this).DesignMode)
		{
			m_PopupItem.SetDesignMode(((Component)this).DesignMode);
		}
	}

	void IOwner.InvokeDefinitionLoaded(object sender, EventArgs e)
	{
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
		}
	}
}
