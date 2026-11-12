using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class TabControl : Control, ISupportInitialize
{
	private TabStrip tabStrip1;

	private Timer m_ClickTimer;

	private bool m_AutoCloseTabs;

	private bool m_TabStripVisible = true;

	private int m_LoadSelectedTabIndex = -1;

	private bool m_SyncingTabStripSize;

	private Container components;

	private bool m_DelayedSync;

	private bool _ControlTabNavigationEnabled = true;

	private bool m_Initializing;

	[Browsable(true)]
	[Description("Indicates the background color.")]
	[Category("Style")]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
			if (tabStrip1 != null)
			{
				((Control)tabStrip1).BackColor = value;
			}
		}
	}

	[Browsable(false)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TabStripTabsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[Description("Returns the collection of Tabs.")]
	public TabsCollection Tabs => tabStrip1.Tabs;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether tabs are visible")]
	public bool TabsVisible
	{
		get
		{
			return m_TabStripVisible;
		}
		set
		{
			m_TabStripVisible = value;
			((Control)tabStrip1).Visible = value;
			RefreshPanelsStyle();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabStrip TabStrip => tabStrip1;

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether TabStrip will get focus when Tab key is used.")]
	public bool TabStripTabStop
	{
		get
		{
			return ((Control)tabStrip1).TabStop;
		}
		set
		{
			((Control)tabStrip1).TabStop = value;
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

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether pressing Ctrl+Tab keys will cycle through the selected tabs.")]
	public bool ControlTabNavigationEnabled
	{
		get
		{
			return _ControlTabNavigationEnabled;
		}
		set
		{
			_ControlTabNavigationEnabled = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether tabs use anti-aliasing when painted.")]
	public bool AntiAlias
	{
		get
		{
			return tabStrip1.AntiAlias;
		}
		set
		{
			tabStrip1.AntiAlias = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether the tab scrolling is animanted.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool Animate
	{
		get
		{
			return tabStrip1.Animate;
		}
		set
		{
			tabStrip1.Animate = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.")]
	public bool AutoHideSystemBox
	{
		get
		{
			return tabStrip1.AutoHideSystemBox;
		}
		set
		{
			tabStrip1.AutoHideSystemBox = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Specifes whether end-user can reorder the tabs.")]
	public bool CanReorderTabs
	{
		get
		{
			return tabStrip1.CanReorderTabs;
		}
		set
		{
			tabStrip1.CanReorderTabs = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Close Button")]
	[Description("Indicates whether tab is automatically closed when close button is clicked.")]
	public bool AutoCloseTabs
	{
		get
		{
			return m_AutoCloseTabs;
		}
		set
		{
			m_AutoCloseTabs = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether the Close button that closes the active tab is visible.")]
	[DefaultValue(false)]
	public bool CloseButtonVisible
	{
		get
		{
			return tabStrip1.CloseButtonVisible;
		}
		set
		{
			tabStrip1.CloseButtonVisible = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether close button is visible on each tab instead of in system box.")]
	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return tabStrip1.CloseButtonOnTabsVisible;
		}
		set
		{
			tabStrip1.CloseButtonOnTabsVisible = value;
			((Control)this).Refresh();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether close button on tabs when visible is displayed for every tab state.")]
	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return tabStrip1.CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			tabStrip1.CloseButtonOnTabsAlwaysDisplayed = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabCloseButtonPosition.Left)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates position of the close button displayed on each tab.")]
	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return tabStrip1.CloseButtonPosition;
		}
		set
		{
			tabStrip1.CloseButtonPosition = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom image that is used on tabs as Close button that allows user to close the tab.")]
	public Image TabCloseButtonNormal
	{
		get
		{
			return tabStrip1.TabCloseButtonNormal;
		}
		set
		{
			tabStrip1.TabCloseButtonNormal = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom image that is used on tabs as Close button whem mouse is over the close button.")]
	public Image TabCloseButtonHot
	{
		get
		{
			return tabStrip1.TabCloseButtonHot;
		}
		set
		{
			tabStrip1.TabCloseButtonHot = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Specifes whether only selected tab is displaying it's text.")]
	[DefaultValue(false)]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return tabStrip1.DisplaySelectedTextOnly;
		}
		set
		{
			tabStrip1.DisplaySelectedTextOnly = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	public ImageList ImageList
	{
		get
		{
			return tabStrip1.ImageList;
		}
		set
		{
			tabStrip1.ImageList = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(0)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ScrollOffset
	{
		get
		{
			return tabStrip1.ScrollOffset;
		}
		set
		{
			tabStrip1.ScrollOffset = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the Tab Control style.")]
	[DefaultValue(eTabStripStyle.OneNote)]
	public eTabStripStyle Style
	{
		get
		{
			return tabStrip1.Style;
		}
		set
		{
			tabStrip1.Style = value;
			SyncTabStripSize();
			RefreshPanelsStyle();
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return tabStrip1.ThemeAware;
		}
		set
		{
			tabStrip1.ThemeAware = value;
			RefreshPanelsStyle();
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.")]
	public virtual bool TabScrollAutoRepeat
	{
		get
		{
			return tabStrip1.TabScrollAutoRepeat;
		}
		set
		{
			tabStrip1.TabScrollAutoRepeat = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(300)]
	[Category("Behavior")]
	[Description("Indicates auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.")]
	public virtual int TabScrollRepeatInterval
	{
		get
		{
			return tabStrip1.TabScrollRepeatInterval;
		}
		set
		{
			tabStrip1.TabScrollRepeatInterval = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets Tab Color Scheme.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public TabColorScheme ColorScheme
	{
		get
		{
			return tabStrip1.ColorScheme;
		}
		set
		{
			tabStrip1.ColorScheme = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[DefaultValue(eTabStripAlignment.Top)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the tab alignment within the Tab-Strip control.")]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			return tabStrip1.TabAlignment;
		}
		set
		{
			if (tabStrip1.TabAlignment == value)
			{
				return;
			}
			((Control)this).SuspendLayout();
			try
			{
				switch (value)
				{
				case eTabStripAlignment.Top:
					((Control)tabStrip1).Dock = (DockStyle)1;
					break;
				case eTabStripAlignment.Bottom:
					((Control)tabStrip1).Dock = (DockStyle)2;
					break;
				case eTabStripAlignment.Left:
					((Control)tabStrip1).Dock = (DockStyle)3;
					break;
				case eTabStripAlignment.Right:
					((Control)tabStrip1).Dock = (DockStyle)4;
					break;
				}
				tabStrip1.TabAlignment = value;
				RefreshPanelsStyle();
			}
			finally
			{
				ResumeLayout(performLayout: true);
			}
			if (((Control)this).Visible)
			{
				SyncTabStripSize();
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[Description("Indicates selected tab.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabItem SelectedTab
	{
		get
		{
			return tabStrip1.SelectedTab;
		}
		set
		{
			tabStrip1.SelectedTab = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(null)]
	[Description("Gets or sets the selected tab Font")]
	public Font SelectedTabFont
	{
		get
		{
			return tabStrip1.SelectedTabFont;
		}
		set
		{
			tabStrip1.SelectedTabFont = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether focus rectangle is displayed on the tab when tab has input focus.")]
	public bool ShowFocusRectangle
	{
		get
		{
			return tabStrip1.ShowFocusRectangle;
		}
		set
		{
			tabStrip1.ShowFocusRectangle = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether keyboard navigation using Left and Right arrow keys to select tabs is enabled.")]
	public bool KeyboardNavigationEnabled
	{
		get
		{
			return tabStrip1.KeyboardNavigationEnabled;
		}
		set
		{
			tabStrip1.KeyboardNavigationEnabled = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates the index of selected tab.")]
	public int SelectedTabIndex
	{
		get
		{
			return tabStrip1.SelectedTabIndex;
		}
		set
		{
			if (m_Initializing)
			{
				m_LoadSelectedTabIndex = value;
			}
			else
			{
				tabStrip1.SelectedTabIndex = value;
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabControlPanel SelectedPanel
	{
		get
		{
			if (tabStrip1.SelectedTab != null)
			{
				return tabStrip1.SelectedTab.AttachedControl as TabControlPanel;
			}
			return null;
		}
		set
		{
			if (value != null && ((Control)this).Controls.Contains((Control)(object)value) && value.TabItem != null && Tabs.Contains(value.TabItem))
			{
				tabStrip1.SelectedTab = value.TabItem;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabLayoutType.FitContainer)]
	[Category("Appearance")]
	[Description("Indicates the type of the tab layout.")]
	public eTabLayoutType TabLayoutType
	{
		get
		{
			return tabStrip1.TabLayoutType;
		}
		set
		{
			tabStrip1.TabLayoutType = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.")]
	public Size FixedTabSize
	{
		get
		{
			return tabStrip1.FixedTabSize;
		}
		set
		{
			tabStrip1.FixedTabSize = value;
			RecalcLayout();
		}
	}

	public event TabStrip.SelectedTabChangedEventHandler SelectedTabChanged;

	public event TabStrip.SelectedTabChangingEventHandler SelectedTabChanging;

	public event TabStrip.TabMovedEventHandler TabMoved;

	public event TabStrip.UserActionEventHandler NavigateBack;

	public event TabStrip.UserActionEventHandler NavigateForward;

	public event TabStrip.UserActionEventHandler TabItemClose;

	public event EventHandler TabItemOpen;

	public event EventHandler BeforeTabDisplay;

	public event EventHandler TabRemoved;

	public TabControl()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		InitializeComponent();
		tabStrip1.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		tabStrip1.SizeRecalculated += TabStripSizeRecalculated;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (components != null)
			{
				components.Dispose();
			}
			StyleManager.Unregister((Control)(object)this);
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.tabStrip1 = new Stimulsoft.Controls.Win.DotNetBar.TabStrip();
		((Control)this).SuspendLayout();
		this.tabStrip1.CanReorderTabs = true;
		this.tabStrip1.CloseButtonVisible = true;
		((Control)this.tabStrip1).Dock = (DockStyle)1;
		this.tabStrip1.ImageList = null;
		((Control)this.tabStrip1).Name = "tabStrip1";
		this.tabStrip1.SelectedTab = null;
		((Control)this.tabStrip1).Size = new System.Drawing.Size(256, 26);
		this.tabStrip1.Style = Stimulsoft.Controls.Win.DotNetBar.eTabStripStyle.OneNote;
		this.tabStrip1.TabAlignment = Stimulsoft.Controls.Win.DotNetBar.eTabStripAlignment.Top;
		((Control)this.tabStrip1).TabIndex = 0;
		((Control)this.tabStrip1).Text = "tabStrip1";
		this.tabStrip1.AutoHideSystemBox = true;
		this.tabStrip1.CloseButtonVisible = false;
		this.tabStrip1.TabRemoved += new System.EventHandler(tabStrip1_TabRemoved);
		this.tabStrip1.TabItemOpen += new System.EventHandler(tabStrip1_TabItemAdded);
		this.tabStrip1.SelectedTabChanged += new Stimulsoft.Controls.Win.DotNetBar.TabStrip.SelectedTabChangedEventHandler(tabStrip1_SelectedTabChanged);
		this.tabStrip1.SelectedTabChanging += new Stimulsoft.Controls.Win.DotNetBar.TabStrip.SelectedTabChangingEventHandler(tabStrip1_SelectedTabChanging);
		this.tabStrip1.TabMoved += new Stimulsoft.Controls.Win.DotNetBar.TabStrip.TabMovedEventHandler(tabStrip1_TabMoved);
		this.tabStrip1.NavigateBack += new Stimulsoft.Controls.Win.DotNetBar.TabStrip.UserActionEventHandler(tabStrip1_NavigateBack);
		this.tabStrip1.NavigateForward += new Stimulsoft.Controls.Win.DotNetBar.TabStrip.UserActionEventHandler(tabStrip1_NavigateForward);
		this.tabStrip1.TabItemClose += new Stimulsoft.Controls.Win.DotNetBar.TabStrip.UserActionEventHandler(tabStrip1_TabItemClose);
		this.tabStrip1.TabItemOpen += new System.EventHandler(tabStrip1_TabItemOpen);
		this.tabStrip1.TabsCleared += new System.EventHandler(tabStrip1_TabsCleared);
		this.tabStrip1.BeforeTabDisplay += new System.EventHandler(tabStrip1_BeforeTabDisplay);
		((Control)this.tabStrip1).TabStop = false;
		((Control)this).Controls.AddRange((Control[])(object)new Control[1] { this.tabStrip1 });
		((Control)this).Name = "TabControl";
		((Control)this).Size = new System.Drawing.Size(256, 152);
		this.ResumeLayout(false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return ((Control)this).BackColor != SystemColors.Control;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateTabPanelStyle();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		if (TabStripTabStop)
		{
			((Control)tabStrip1).Focus();
		}
		((Control)this).OnGotFocus(e);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool ValidateChildren()
	{
		return ValidateChildren((ValidationConstraints)1);
	}

	[Browsable(false)]
	public virtual bool ValidateChildren(ValidationConstraints validationConstraints)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if ((int)validationConstraints < 0 || (int)validationConstraints > 31)
		{
			throw new InvalidEnumArgumentException("validationConstraints", (int)validationConstraints, typeof(ValidationConstraints));
		}
		Type typeFromHandle = typeof(Control);
		MethodInfo method = typeFromHandle.GetMethod("PerformContainerValidation", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
		if (method != null)
		{
			bool flag = (bool)method.Invoke(this, new object[1] { validationConstraints });
			return !flag;
		}
		return true;
	}

	public void RecalcLayout()
	{
		((Control)tabStrip1).Refresh();
		SyncTabStripSize();
	}

	private void tabStrip1_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
	{
		if (e.NewTab != null && e.NewTab.AttachedControl != null)
		{
			e.NewTab.AttachedControl.BringToFront();
		}
		InvokeSelectedTabChanged(e);
	}

	protected virtual void InvokeSelectedTabChanged(TabStripTabChangedEventArgs e)
	{
		if (this.SelectedTabChanged != null)
		{
			this.SelectedTabChanged(this, e);
		}
	}

	private void tabStrip1_SelectedTabChanging(object sender, TabStripTabChangingEventArgs e)
	{
		InvokeSelectedTabChanging(e);
	}

	protected virtual void InvokeSelectedTabChanging(TabStripTabChangingEventArgs e)
	{
		if (this.SelectedTabChanging != null)
		{
			this.SelectedTabChanging(this, e);
		}
	}

	private void tabStrip1_TabMoved(object sender, TabStripTabMovedEventArgs e)
	{
		InvokeTabMoved(e);
	}

	protected virtual void InvokeTabMoved(TabStripTabMovedEventArgs e)
	{
		if (this.TabMoved != null)
		{
			this.TabMoved(this, e);
		}
	}

	private void tabStrip1_NavigateBack(object sender, TabStripActionEventArgs e)
	{
		InvokeNavigateBack(e);
	}

	protected virtual void InvokeNavigateBack(TabStripActionEventArgs e)
	{
		if (this.NavigateBack != null)
		{
			this.NavigateBack(this, e);
		}
	}

	private void tabStrip1_NavigateForward(object sender, TabStripActionEventArgs e)
	{
		InvokeNavigateForward(e);
	}

	protected virtual void InvokeNavigateForward(TabStripActionEventArgs e)
	{
		if (this.NavigateForward != null)
		{
			this.NavigateForward(this, e);
		}
	}

	private void tabStrip1_TabItemClose(object sender, TabStripActionEventArgs e)
	{
		InvokeTabItemClose(e);
		if (e.Cancel || !m_AutoCloseTabs || ((Component)this).DesignMode)
		{
			return;
		}
		TabItem selectedTab = tabStrip1.SelectedTab;
		if (selectedTab != null)
		{
			tabStrip1.Tabs.Remove(selectedTab);
			Control attachedControl = selectedTab.AttachedControl;
			if (attachedControl != null)
			{
				if (((Control)this).Controls.Contains(attachedControl))
				{
					((Control)this).Controls.Remove(attachedControl);
				}
				((Component)(object)attachedControl).Dispose();
			}
		}
		RecalcLayout();
	}

	protected virtual void InvokeTabItemClose(TabStripActionEventArgs e)
	{
		if (this.TabItemClose != null)
		{
			this.TabItemClose(this, e);
		}
	}

	private void tabStrip1_TabItemOpen(object sender, EventArgs e)
	{
		InvokeTabItemOpen(e);
	}

	protected virtual void InvokeTabItemOpen(EventArgs e)
	{
		if (this.TabItemOpen != null)
		{
			this.TabItemOpen(this, e);
		}
	}

	private void tabStrip1_BeforeTabDisplay(object sender, EventArgs e)
	{
		InvokeBeforeTabDisplay(e);
	}

	protected virtual void InvokeBeforeTabDisplay(EventArgs e)
	{
		if (this.BeforeTabDisplay != null)
		{
			this.BeforeTabDisplay(this, e);
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode || !(e.Control is TabControlPanel tabControlPanel))
		{
			return;
		}
		if (tabControlPanel.TabItem != null)
		{
			if (Tabs.Contains(tabControlPanel.TabItem) && SelectedTab == tabControlPanel.TabItem)
			{
				((Control)tabControlPanel).Visible = true;
				((Control)tabControlPanel).BringToFront();
			}
			else
			{
				((Control)tabControlPanel).Visible = false;
			}
		}
		else
		{
			((Control)tabControlPanel).Visible = false;
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (e.Control is TabControlPanel { TabItem: not null } tabControlPanel && Tabs.Contains(tabControlPanel.TabItem))
		{
			Tabs.Remove(tabControlPanel.TabItem);
		}
	}

	private void tabStrip1_TabRemoved(object sender, EventArgs e)
	{
		if (sender is TabItem)
		{
			TabItem tabItem = sender as TabItem;
			if (tabItem.AttachedControl != null && ((Control)this).Controls.Contains(tabItem.AttachedControl))
			{
				((Control)this).Controls.Remove(tabItem.AttachedControl);
			}
		}
		InvokeTabRemoved(e);
	}

	private void tabStrip1_TabItemAdded(object sender, EventArgs e)
	{
		TabItem tabItem = sender as TabItem;
		if (!((Component)this).DesignMode && !m_Initializing && tabItem != null && tabItem.AttachedControl != null && !((Control)this).Controls.Contains(tabItem.AttachedControl))
		{
			((Control)this).Controls.Add(tabItem.AttachedControl);
		}
	}

	protected virtual void InvokeTabRemoved(EventArgs e)
	{
		if (this.TabRemoved != null)
		{
			this.TabRemoved(this, e);
		}
	}

	private void tabStrip1_TabsCleared(object sender, EventArgs e)
	{
		if (((Component)this).DesignMode)
		{
			return;
		}
		Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
		((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
		Control[] array2 = array;
		foreach (Control val in array2)
		{
			if (val is TabControlPanel)
			{
				((Control)this).Controls.Remove(val);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		if (((Control)this).BackColor == Color.Transparent || ColorScheme.TabBackground == Color.Transparent)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count <= 1)
		{
			SolidBrush val = new SolidBrush(SystemColors.Control);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0 && tabStrip1.IsMultiLine)
		{
			CreateSyncSizeTimer();
		}
	}

	private void CreateSyncSizeTimer()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (m_ClickTimer == null)
		{
			m_ClickTimer = new Timer();
			m_ClickTimer.Interval = 10;
			m_ClickTimer.Tick += SyncSizeTimerTick;
			m_ClickTimer.Enabled = true;
			m_ClickTimer.Start();
		}
	}

	private void SyncSizeTimerTick(object sender, EventArgs e)
	{
		m_ClickTimer.Stop();
		m_ClickTimer.Enabled = false;
		m_ClickTimer = null;
		try
		{
			((Control)this).PerformLayout();
		}
		catch
		{
		}
	}

	public void ApplyDefaultPanelStyle(TabControlPanel panel)
	{
		if (panel == null || panel.UseCustomStyle)
		{
			return;
		}
		if (panel.TabItem != null && !panel.TabItem.BackColor.IsEmpty)
		{
			if (!panel.TabItem.BackColor2.IsEmpty)
			{
				panel.Style.BackColor1.Color = panel.TabItem.BackColor;
				panel.Style.BackColor2.Color = panel.TabItem.BackColor2;
				switch (tabStrip1.TabAlignment)
				{
				case eTabStripAlignment.Top:
					panel.Style.GradientAngle = panel.TabItem.BackColorGradientAngle - 180;
					break;
				case eTabStripAlignment.Bottom:
					panel.Style.GradientAngle = panel.TabItem.BackColorGradientAngle;
					break;
				case eTabStripAlignment.Left:
					panel.Style.GradientAngle = panel.TabItem.BackColorGradientAngle - 90;
					break;
				case eTabStripAlignment.Right:
					panel.Style.GradientAngle = panel.TabItem.BackColorGradientAngle + 90;
					break;
				}
			}
		}
		else
		{
			panel.Style.BackColor1.Color = tabStrip1.ColorScheme.TabPanelBackground;
			panel.Style.BackColor2.Color = tabStrip1.ColorScheme.TabPanelBackground2;
			switch (tabStrip1.TabAlignment)
			{
			case eTabStripAlignment.Top:
				panel.Style.GradientAngle = tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle;
				break;
			case eTabStripAlignment.Bottom:
				panel.Style.GradientAngle = tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle - 180;
				break;
			case eTabStripAlignment.Left:
				panel.Style.GradientAngle = tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle - 90;
				break;
			case eTabStripAlignment.Right:
				panel.Style.GradientAngle = tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle + 90;
				break;
			}
			panel.Style.BorderColor.Color = tabStrip1.ColorScheme.TabPanelBorder;
		}
		panel.Style.Border = eBorderType.SingleLine;
		if (m_TabStripVisible)
		{
			switch (tabStrip1.TabAlignment)
			{
			case eTabStripAlignment.Top:
				panel.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
				break;
			case eTabStripAlignment.Bottom:
				panel.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Top;
				break;
			case eTabStripAlignment.Left:
				panel.Style.BorderSide = eBorderSide.Right | eBorderSide.Top | eBorderSide.Bottom;
				break;
			case eTabStripAlignment.Right:
				panel.Style.BorderSide = eBorderSide.Left | eBorderSide.Top | eBorderSide.Bottom;
				break;
			}
		}
		else
		{
			panel.Style.BorderSide = eBorderSide.All;
		}
		if (tabStrip1.IsThemed)
		{
			panel.DrawThemedPane = true;
		}
		else
		{
			panel.DrawThemedPane = false;
		}
	}

	public void SetStyle()
	{
		UpdateTabPanelStyle();
	}

	private void UpdateTabPanelStyle()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is TabControlPanel)
			{
				TabControlPanel panel = val as TabControlPanel;
				ApplyDefaultPanelStyle(panel);
			}
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		UpdateTabPanelStyle();
	}

	public void ResumeLayout(bool performLayout)
	{
		((Control)this).ResumeLayout(performLayout);
		if (!((Control)this).IsHandleCreated)
		{
			m_DelayedSync = true;
			return;
		}
		tabStrip1.ResetTabHeight();
		SyncTabStripSize();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		if (m_DelayedSync || ((Component)this).DesignMode)
		{
			m_DelayedSync = false;
			tabStrip1.ResetTabHeight();
			SyncTabStripSize();
			if (((Component)this).DesignMode)
			{
				InvokeDelayed((MethodInvoker)delegate
				{
					((Control)this).PerformLayout();
				});
			}
		}
		((Control)this).OnHandleCreated(e);
	}

	private void RefreshPanelsStyle()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (m_Initializing)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is TabControlPanel)
			{
				ApplyDefaultPanelStyle(val as TabControlPanel);
			}
		}
	}

	private void TabStripSizeRecalculated(object sender, EventArgs e)
	{
		SyncTabStripSize();
	}

	public void SyncTabStripSize()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		if (m_SyncingTabStripSize)
		{
			return;
		}
		m_SyncingTabStripSize = true;
		try
		{
			DockStyle dock = ((Control)tabStrip1).Dock;
			if (dock - 3 <= 1)
			{
				((Control)tabStrip1).Width = tabStrip1.MinTabStripHeight;
			}
			else
			{
				((Control)tabStrip1).Height = tabStrip1.MinTabStripHeight;
			}
			((Control)tabStrip1).SendToBack();
		}
		finally
		{
			m_SyncingTabStripSize = false;
		}
	}

	private void InvokeDelayed(MethodInvoker method)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = method;
		val.Interval = 10;
		val.Tick += DelayedInvokeTimerTick;
		val.Start();
	}

	private void DelayedInvokeTimerTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)sender;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}

	public TabItem CreateTab(string tabText)
	{
		return CreateTab(tabText, -1);
	}

	public TabItem CreateTab(string tabText, int insertAt)
	{
		TabItem tabItem = new TabItem();
		tabItem.Text = tabText;
		TabControlPanel tabControlPanel = new TabControlPanel();
		ApplyDefaultPanelStyle(tabControlPanel);
		tabItem.AttachedControl = (Control)(object)tabControlPanel;
		tabControlPanel.TabItem = tabItem;
		if (insertAt < 0 || insertAt >= tabStrip1.Tabs.Count)
		{
			tabStrip1.Tabs.Add(tabItem);
			((Control)this).Controls.Add((Control)(object)tabControlPanel);
		}
		else
		{
			tabStrip1.Tabs.Insert(insertAt, tabItem);
			((Control)this).Controls.Add((Control)(object)tabControlPanel);
		}
		((Control)tabControlPanel).Dock = (DockStyle)5;
		((Control)tabControlPanel).SendToBack();
		RecalcLayout();
		return tabItem;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if (_ControlTabNavigationEnabled)
		{
			if ((int)keyData == 131081)
			{
				tabStrip1.SelectNextTab(eEventSource.Keyboard, cycle: true);
				return true;
			}
			if ((int)keyData == 196617)
			{
				tabStrip1.SelectPreviousTab(eEventSource.Keyboard, cycle: true);
				return true;
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	void ISupportInitialize.BeginInit()
	{
		m_Initializing = true;
	}

	void ISupportInitialize.EndInit()
	{
		m_Initializing = false;
		RefreshPanelsStyle();
		if (m_LoadSelectedTabIndex >= 0)
		{
			tabStrip1.SelectedTabIndex = m_LoadSelectedTabIndex;
		}
		m_LoadSelectedTabIndex = -1;
		if (tabStrip1.SelectedTab != null && tabStrip1.SelectedTab.AttachedControl != null)
		{
			tabStrip1.SelectedTab.AttachedControl.Visible = true;
		}
	}

	public bool SelectPreviousTab()
	{
		return tabStrip1.SelectPreviousTab();
	}

	public bool SelectNextTab()
	{
		return tabStrip1.SelectNextTab();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (Style == eTabStripStyle.Office2007Dock || Style == eTabStripStyle.Office2007Document)
		{
			tabStrip1.Style = Style;
			RefreshPanelsStyle();
		}
		((Control)this).OnParentChanged(e);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return tabStrip1.ColorScheme.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		tabStrip1.ResetColorScheme();
		if (((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		tabStrip1.ResetSelectedTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return tabStrip1.ShouldSerializeFixedTabSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedTabSize()
	{
		TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
	}
}
