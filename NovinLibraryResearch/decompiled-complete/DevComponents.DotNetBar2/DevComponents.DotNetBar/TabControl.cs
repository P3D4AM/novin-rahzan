using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.TabControlDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class TabControl : Control, ISupportInitialize
{
	private TabStrip.SelectedTabChangedEventHandler Ꮷ;

	private TabStrip.SelectedTabChangingEventHandler ᓩ;

	private TabStrip.TabMovedEventHandler ᓪ;

	private TabStrip.UserActionEventHandler ᓫ;

	private TabStrip.UserActionEventHandler ᓬ;

	private TabStrip.UserActionEventHandler ᓭ;

	private EventHandler ᓮ;

	private EventHandler _0C55;

	private EventHandler ᓯ;

	private TabStrip ᕺ;

	private Timer _৮;

	private bool ᕻ;

	private bool ᕼ = true;

	private int ᕽ = -1;

	private bool ᕾ;

	private Container Պ;

	private bool ᕿ;

	private bool _ᖀ = true;

	private bool ᖁ;

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
			if (ᕺ != null)
			{
				((Control)ᕺ).BackColor = value;
			}
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TabStripTabsEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[Browsable(false)]
	[Description("Returns the collection of Tabs.")]
	public TabsCollection Tabs => ᕺ.Tabs;

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether tabs are visible")]
	public bool TabsVisible
	{
		get
		{
			return ᕼ;
		}
		set
		{
			ᕼ = value;
			((Control)ᕺ).Visible = value;
			ᖐ();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public TabStrip TabStrip => ᕺ;

	[Description("Gets or sets whether TabStrip will get focus when Tab key is used.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool TabStripTabStop
	{
		get
		{
			return ((Control)ᕺ).TabStop;
		}
		set
		{
			((Control)ᕺ).TabStop = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[Category("Behavior")]
	[Description("Indicates whether pressing Ctrl+Tab keys will cycle through the selected tabs.")]
	[DefaultValue(true)]
	public bool ControlTabNavigationEnabled
	{
		get
		{
			return _ᖀ;
		}
		set
		{
			_ᖀ = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Gets or sets whether tabs use anti-aliasing when painted.")]
	[DefaultValue(false)]
	public bool AntiAlias
	{
		get
		{
			return ᕺ.AntiAlias;
		}
		set
		{
			ᕺ.AntiAlias = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates whether the tab scrolling is animanted.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	public bool Animate
	{
		get
		{
			return ᕺ.Animate;
		}
		set
		{
			ᕺ.Animate = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.")]
	[Browsable(true)]
	[Category("Behavior")]
	public bool AutoHideSystemBox
	{
		get
		{
			return ᕺ.AutoHideSystemBox;
		}
		set
		{
			ᕺ.AutoHideSystemBox = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Description("Specifes whether end-user can reorder the tabs.")]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public bool CanReorderTabs
	{
		get
		{
			return ᕺ.CanReorderTabs;
		}
		set
		{
			ᕺ.CanReorderTabs = value;
		}
	}

	[Browsable(true)]
	[Category("Close Button")]
	[DefaultValue(false)]
	[Description("Indicates whether tab is automatically closed when close button is clicked.")]
	public bool AutoCloseTabs
	{
		get
		{
			return ᕻ;
		}
		set
		{
			ᕻ = value;
		}
	}

	[Category("Close Button")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether the Close button that closes the active tab is visible.")]
	[DefaultValue(false)]
	public bool CloseButtonVisible
	{
		get
		{
			return ᕺ.CloseButtonVisible;
		}
		set
		{
			ᕺ.CloseButtonVisible = value;
		}
	}

	[Browsable(true)]
	[Category("Close Button")]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether close button is visible on each tab instead of in system box.")]
	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return ᕺ.CloseButtonOnTabsVisible;
		}
		set
		{
			ᕺ.CloseButtonOnTabsVisible = value;
			((Control)this).Refresh();
		}
	}

	[Category("Close Button")]
	[Description("Indicates whether close button on tabs when visible is displayed for every tab state.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return ᕺ.CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			ᕺ.CloseButtonOnTabsAlwaysDisplayed = value;
		}
	}

	[Description("Indicates position of the close button displayed on each tab.")]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Browsable(true)]
	[DefaultValue(eTabCloseButtonPosition.Left)]
	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return ᕺ.CloseButtonPosition;
		}
		set
		{
			ᕺ.CloseButtonPosition = value;
		}
	}

	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom image that is used on tabs as Close button that allows user to close the tab.")]
	[Browsable(true)]
	public Image TabCloseButtonNormal
	{
		get
		{
			return ᕺ.TabCloseButtonNormal;
		}
		set
		{
			ᕺ.TabCloseButtonNormal = value;
		}
	}

	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom image that is used on tabs as Close button whem mouse is over the close button.")]
	[Browsable(true)]
	public Image TabCloseButtonHot
	{
		get
		{
			return ᕺ.TabCloseButtonHot;
		}
		set
		{
			ᕺ.TabCloseButtonHot = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifes whether only selected tab is displaying it's text.")]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return ᕺ.DisplaySelectedTextOnly;
		}
		set
		{
			ᕺ.DisplaySelectedTextOnly = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(null)]
	public ImageList ImageList
	{
		get
		{
			return ᕺ.ImageList;
		}
		set
		{
			ᕺ.ImageList = value;
		}
	}

	[DevCoBrowsable(false)]
	[DefaultValue(0)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ScrollOffset
	{
		get
		{
			return ᕺ.ScrollOffset;
		}
		set
		{
			ᕺ.ScrollOffset = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabStripStyle.OneNote)]
	[DevCoBrowsable(true)]
	[Description("Specifies the Tab Control style.")]
	[Category("Appearance")]
	public eTabStripStyle Style
	{
		get
		{
			return ᕺ.Style;
		}
		set
		{
			ᕺ.Style = value;
			SyncTabStripSize();
			ᖐ();
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return ᕺ.ThemeAware;
		}
		set
		{
			ᕺ.ThemeAware = value;
			ᖐ();
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.")]
	[Category("Behavior")]
	[Browsable(true)]
	public virtual bool TabScrollAutoRepeat
	{
		get
		{
			return ᕺ.TabScrollAutoRepeat;
		}
		set
		{
			ᕺ.TabScrollAutoRepeat = value;
		}
	}

	[DefaultValue(300)]
	[Browsable(true)]
	[Description("Indicates auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.")]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public virtual int TabScrollRepeatInterval
	{
		get
		{
			return ᕺ.TabScrollRepeatInterval;
		}
		set
		{
			ᕺ.TabScrollRepeatInterval = value;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets Tab Color Scheme.")]
	public TabColorScheme ColorScheme
	{
		get
		{
			return ᕺ.ColorScheme;
		}
		set
		{
			ᕺ.ColorScheme = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the tab alignment within the Tab-Strip control.")]
	[DefaultValue(eTabStripAlignment.Top)]
	[Browsable(true)]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			return ᕺ.TabAlignment;
		}
		set
		{
			if (ᕺ.TabAlignment == value)
			{
				return;
			}
			((Control)this).SuspendLayout();
			try
			{
				switch (value)
				{
				case eTabStripAlignment.Top:
					((Control)ᕺ).Dock = (DockStyle)1;
					break;
				case eTabStripAlignment.Bottom:
					((Control)ᕺ).Dock = (DockStyle)2;
					break;
				case eTabStripAlignment.Left:
					((Control)ᕺ).Dock = (DockStyle)3;
					break;
				case eTabStripAlignment.Right:
					((Control)ᕺ).Dock = (DockStyle)4;
					break;
				}
				ᕺ.TabAlignment = value;
				ᖐ();
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

	[Category("Behavior")]
	[Description("Indicates selected tab.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabItem SelectedTab
	{
		get
		{
			return ᕺ.SelectedTab;
		}
		set
		{
			ᕺ.SelectedTab = value;
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the selected tab Font")]
	[Browsable(true)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	public Font SelectedTabFont
	{
		get
		{
			return ᕺ.SelectedTabFont;
		}
		set
		{
			ᕺ.SelectedTabFont = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether focus rectangle is displayed on the tab when tab has input focus.")]
	public bool ShowFocusRectangle
	{
		get
		{
			return ᕺ.ShowFocusRectangle;
		}
		set
		{
			ᕺ.ShowFocusRectangle = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether keyboard navigation using Left and Right arrow keys to select tabs is enabled.")]
	public bool KeyboardNavigationEnabled
	{
		get
		{
			return ᕺ.KeyboardNavigationEnabled;
		}
		set
		{
			ᕺ.KeyboardNavigationEnabled = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates the index of selected tab.")]
	[Category("Behavior")]
	[Browsable(true)]
	public int SelectedTabIndex
	{
		get
		{
			return ᕺ.SelectedTabIndex;
		}
		set
		{
			if (ᖁ)
			{
				ᕽ = value;
			}
			else
			{
				ᕺ.SelectedTabIndex = value;
			}
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public TabControlPanel SelectedPanel
	{
		get
		{
			if (ᕺ.SelectedTab != null)
			{
				return ᕺ.SelectedTab.AttachedControl as TabControlPanel;
			}
			return null;
		}
		set
		{
			if (value != null && ((Control)this).Controls.Contains((Control)(object)value) && value.TabItem != null && Tabs.Contains(value.TabItem))
			{
				ᕺ.SelectedTab = value.TabItem;
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates the type of the tab layout.")]
	[DefaultValue(eTabLayoutType.FitContainer)]
	[Category("Appearance")]
	public eTabLayoutType TabLayoutType
	{
		get
		{
			return ᕺ.TabLayoutType;
		}
		set
		{
			ᕺ.TabLayoutType = value;
		}
	}

	[Description("Indicates whether selected tab is aligned with the panel in multiple lines use case.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool MultiLinePanelAlignSelectedTab
	{
		get
		{
			return ᕺ.MultiLinePanelAlignSelectedTab;
		}
		set
		{
			ᕺ.MultiLinePanelAlignSelectedTab = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.")]
	[Browsable(true)]
	public Size FixedTabSize
	{
		get
		{
			return ᕺ.FixedTabSize;
		}
		set
		{
			ᕺ.FixedTabSize = value;
			RecalcLayout();
		}
	}

	public event TabStrip.SelectedTabChangedEventHandler SelectedTabChanged
	{
		add
		{
			TabStrip.SelectedTabChangedEventHandler selectedTabChangedEventHandler = Ꮷ;
			TabStrip.SelectedTabChangedEventHandler selectedTabChangedEventHandler2;
			do
			{
				selectedTabChangedEventHandler2 = selectedTabChangedEventHandler;
				TabStrip.SelectedTabChangedEventHandler value2 = (TabStrip.SelectedTabChangedEventHandler)Delegate.Combine(selectedTabChangedEventHandler2, value);
				selectedTabChangedEventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, selectedTabChangedEventHandler2);
			}
			while ((object)selectedTabChangedEventHandler != selectedTabChangedEventHandler2);
		}
		remove
		{
			TabStrip.SelectedTabChangedEventHandler selectedTabChangedEventHandler = Ꮷ;
			TabStrip.SelectedTabChangedEventHandler selectedTabChangedEventHandler2;
			do
			{
				selectedTabChangedEventHandler2 = selectedTabChangedEventHandler;
				TabStrip.SelectedTabChangedEventHandler value2 = (TabStrip.SelectedTabChangedEventHandler)Delegate.Remove(selectedTabChangedEventHandler2, value);
				selectedTabChangedEventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, selectedTabChangedEventHandler2);
			}
			while ((object)selectedTabChangedEventHandler != selectedTabChangedEventHandler2);
		}
	}

	public event TabStrip.SelectedTabChangingEventHandler SelectedTabChanging
	{
		add
		{
			TabStrip.SelectedTabChangingEventHandler selectedTabChangingEventHandler = ᓩ;
			TabStrip.SelectedTabChangingEventHandler selectedTabChangingEventHandler2;
			do
			{
				selectedTabChangingEventHandler2 = selectedTabChangingEventHandler;
				TabStrip.SelectedTabChangingEventHandler value2 = (TabStrip.SelectedTabChangingEventHandler)Delegate.Combine(selectedTabChangingEventHandler2, value);
				selectedTabChangingEventHandler = Interlocked.CompareExchange(ref ᓩ, value2, selectedTabChangingEventHandler2);
			}
			while ((object)selectedTabChangingEventHandler != selectedTabChangingEventHandler2);
		}
		remove
		{
			TabStrip.SelectedTabChangingEventHandler selectedTabChangingEventHandler = ᓩ;
			TabStrip.SelectedTabChangingEventHandler selectedTabChangingEventHandler2;
			do
			{
				selectedTabChangingEventHandler2 = selectedTabChangingEventHandler;
				TabStrip.SelectedTabChangingEventHandler value2 = (TabStrip.SelectedTabChangingEventHandler)Delegate.Remove(selectedTabChangingEventHandler2, value);
				selectedTabChangingEventHandler = Interlocked.CompareExchange(ref ᓩ, value2, selectedTabChangingEventHandler2);
			}
			while ((object)selectedTabChangingEventHandler != selectedTabChangingEventHandler2);
		}
	}

	public event TabStrip.TabMovedEventHandler TabMoved
	{
		add
		{
			TabStrip.TabMovedEventHandler tabMovedEventHandler = ᓪ;
			TabStrip.TabMovedEventHandler tabMovedEventHandler2;
			do
			{
				tabMovedEventHandler2 = tabMovedEventHandler;
				TabStrip.TabMovedEventHandler value2 = (TabStrip.TabMovedEventHandler)Delegate.Combine(tabMovedEventHandler2, value);
				tabMovedEventHandler = Interlocked.CompareExchange(ref ᓪ, value2, tabMovedEventHandler2);
			}
			while ((object)tabMovedEventHandler != tabMovedEventHandler2);
		}
		remove
		{
			TabStrip.TabMovedEventHandler tabMovedEventHandler = ᓪ;
			TabStrip.TabMovedEventHandler tabMovedEventHandler2;
			do
			{
				tabMovedEventHandler2 = tabMovedEventHandler;
				TabStrip.TabMovedEventHandler value2 = (TabStrip.TabMovedEventHandler)Delegate.Remove(tabMovedEventHandler2, value);
				tabMovedEventHandler = Interlocked.CompareExchange(ref ᓪ, value2, tabMovedEventHandler2);
			}
			while ((object)tabMovedEventHandler != tabMovedEventHandler2);
		}
	}

	public event TabStrip.UserActionEventHandler NavigateBack
	{
		add
		{
			TabStrip.UserActionEventHandler userActionEventHandler = ᓫ;
			TabStrip.UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				TabStrip.UserActionEventHandler value2 = (TabStrip.UserActionEventHandler)Delegate.Combine(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓫ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
		remove
		{
			TabStrip.UserActionEventHandler userActionEventHandler = ᓫ;
			TabStrip.UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				TabStrip.UserActionEventHandler value2 = (TabStrip.UserActionEventHandler)Delegate.Remove(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓫ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
	}

	public event TabStrip.UserActionEventHandler NavigateForward
	{
		add
		{
			TabStrip.UserActionEventHandler userActionEventHandler = ᓬ;
			TabStrip.UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				TabStrip.UserActionEventHandler value2 = (TabStrip.UserActionEventHandler)Delegate.Combine(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓬ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
		remove
		{
			TabStrip.UserActionEventHandler userActionEventHandler = ᓬ;
			TabStrip.UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				TabStrip.UserActionEventHandler value2 = (TabStrip.UserActionEventHandler)Delegate.Remove(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓬ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
	}

	public event TabStrip.UserActionEventHandler TabItemClose
	{
		add
		{
			TabStrip.UserActionEventHandler userActionEventHandler = ᓭ;
			TabStrip.UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				TabStrip.UserActionEventHandler value2 = (TabStrip.UserActionEventHandler)Delegate.Combine(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓭ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
		remove
		{
			TabStrip.UserActionEventHandler userActionEventHandler = ᓭ;
			TabStrip.UserActionEventHandler userActionEventHandler2;
			do
			{
				userActionEventHandler2 = userActionEventHandler;
				TabStrip.UserActionEventHandler value2 = (TabStrip.UserActionEventHandler)Delegate.Remove(userActionEventHandler2, value);
				userActionEventHandler = Interlocked.CompareExchange(ref ᓭ, value2, userActionEventHandler2);
			}
			while ((object)userActionEventHandler != userActionEventHandler2);
		}
	}

	public event EventHandler TabItemOpen
	{
		add
		{
			EventHandler eventHandler = ᓮ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓮ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler BeforeTabDisplay
	{
		add
		{
			EventHandler eventHandler = _0C55;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0C55, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0C55;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0C55, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler TabRemoved
	{
		add
		{
			EventHandler eventHandler = ᓯ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᓯ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public TabControl()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		ـ();
		ᕺ.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		ᕺ.ᓗ += ᖑ;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (Պ != null)
			{
				Պ.Dispose();
			}
			StyleManager.Unregister((Control)(object)this);
		}
		((Control)this).Dispose(disposing);
	}

	private void ـ()
	{
		ᕺ = new TabStrip();
		((Control)this).SuspendLayout();
		ᕺ.CanReorderTabs = true;
		ᕺ.CloseButtonVisible = true;
		((Control)ᕺ).Dock = (DockStyle)1;
		ᕺ.ImageList = null;
		((Control)ᕺ).Name = "tabStrip1";
		ᕺ.SelectedTab = null;
		((Control)ᕺ).Size = new Size(256, 26);
		ᕺ.Style = eTabStripStyle.OneNote;
		ᕺ.TabAlignment = eTabStripAlignment.Top;
		((Control)ᕺ).TabIndex = 0;
		((Control)ᕺ).Text = "tabStrip1";
		ᕺ.AutoHideSystemBox = true;
		ᕺ.CloseButtonVisible = false;
		ᕺ.TabRemoved += ᖊ;
		ᕺ.TabItemOpen += ᖋ;
		ᕺ.SelectedTabChanged += ᖂ;
		ᕺ.SelectedTabChanging += ᖃ;
		ᕺ.TabMoved += ᖄ;
		ᕺ.NavigateBack += ᖅ;
		ᕺ.NavigateForward += ᖆ;
		ᕺ.TabItemClose += ᖇ;
		ᕺ.TabItemOpen += ᖈ;
		ᕺ.TabsCleared += ᖌ;
		ᕺ.BeforeTabDisplay += ᖉ;
		((Control)ᕺ).TabStop = false;
		((Control)this).Controls.AddRange((Control[])(object)new Control[1] { ᕺ });
		((Control)this).Name = "TabControl";
		((Control)this).Size = new Size(256, 152);
		ResumeLayout(performLayout: false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return ((Control)this).BackColor != SystemColors.Control;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		ᖏ();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		if (TabStripTabStop)
		{
			((Control)ᕺ).Focus();
		}
		((Control)this).OnGotFocus(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if ((int)validationConstraints < 0 || (int)validationConstraints > 31)
		{
			throw new InvalidEnumArgumentException("validationConstraints", (int)validationConstraints, typeof(ValidationConstraints));
		}
		Type typeFromHandle = typeof(Control);
		MethodInfo method = typeFromHandle.GetMethod("PerformContainerValidation", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
		if ((object)method != null)
		{
			bool flag = (bool)method.Invoke(this, new object[1] { validationConstraints });
			return !flag;
		}
		return true;
	}

	public void RecalcLayout()
	{
		((Control)ᕺ).Refresh();
		SyncTabStripSize();
	}

	private void ᖂ(object _٠, TabStripTabChangedEventArgs ؿ)
	{
		if (ؿ.NewTab != null && ؿ.NewTab.AttachedControl != null)
		{
			ؿ.NewTab.AttachedControl.BringToFront();
		}
		InvokeSelectedTabChanged(ؿ);
	}

	protected virtual void InvokeSelectedTabChanged(TabStripTabChangedEventArgs e)
	{
		if (Ꮷ != null)
		{
			Ꮷ(this, e);
		}
	}

	private void ᖃ(object _٠, TabStripTabChangingEventArgs ؿ)
	{
		InvokeSelectedTabChanging(ؿ);
	}

	protected virtual void InvokeSelectedTabChanging(TabStripTabChangingEventArgs e)
	{
		if (ᓩ != null)
		{
			ᓩ(this, e);
		}
	}

	private void ᖄ(object _٠, TabStripTabMovedEventArgs ؿ)
	{
		InvokeTabMoved(ؿ);
	}

	protected virtual void InvokeTabMoved(TabStripTabMovedEventArgs e)
	{
		if (ᓪ != null)
		{
			ᓪ(this, e);
		}
	}

	private void ᖅ(object _٠, TabStripActionEventArgs ؿ)
	{
		InvokeNavigateBack(ؿ);
	}

	protected virtual void InvokeNavigateBack(TabStripActionEventArgs e)
	{
		if (ᓫ != null)
		{
			ᓫ(this, e);
		}
	}

	private void ᖆ(object _٠, TabStripActionEventArgs ؿ)
	{
		InvokeNavigateForward(ؿ);
	}

	protected virtual void InvokeNavigateForward(TabStripActionEventArgs e)
	{
		if (ᓬ != null)
		{
			ᓬ(this, e);
		}
	}

	private void ᖇ(object _٠, TabStripActionEventArgs ؿ)
	{
		InvokeTabItemClose(ؿ);
		if (ؿ.Cancel || !ᕻ || ((Component)this).DesignMode)
		{
			return;
		}
		TabItem selectedTab = ᕺ.SelectedTab;
		if (selectedTab != null)
		{
			ᕺ.Tabs.Remove(selectedTab);
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
		if (ᓭ != null)
		{
			ᓭ(this, e);
		}
	}

	private void ᖈ(object _٠, EventArgs ؿ)
	{
		InvokeTabItemOpen(ؿ);
	}

	protected virtual void InvokeTabItemOpen(EventArgs e)
	{
		if (ᓮ != null)
		{
			ᓮ(this, e);
		}
	}

	private void ᖉ(object _٠, EventArgs ؿ)
	{
		InvokeBeforeTabDisplay(ؿ);
	}

	protected virtual void InvokeBeforeTabDisplay(EventArgs e)
	{
		if (_0C55 != null)
		{
			_0C55(this, e);
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

	private void ᖊ(object _٠, EventArgs ؿ)
	{
		if (_٠ is TabItem)
		{
			TabItem tabItem = _٠ as TabItem;
			if (tabItem.AttachedControl != null && ((Control)this).Controls.Contains(tabItem.AttachedControl))
			{
				((Control)this).Controls.Remove(tabItem.AttachedControl);
			}
		}
		InvokeTabRemoved(ؿ);
	}

	private void ᖋ(object _٠, EventArgs ؿ)
	{
		TabItem tabItem = _٠ as TabItem;
		if (!((Component)this).DesignMode && !ᖁ && tabItem != null && tabItem.AttachedControl != null && !((Control)this).Controls.Contains(tabItem.AttachedControl))
		{
			((Control)this).Controls.Add(tabItem.AttachedControl);
		}
	}

	protected virtual void InvokeTabRemoved(EventArgs e)
	{
		if (ᓯ != null)
		{
			ᓯ(this, e);
		}
	}

	private void ᖌ(object _٠, EventArgs ؿ)
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
		if (((Control)this).Width != 0 && ((Control)this).Height != 0 && ᕺ.ᓡ)
		{
			ᖍ();
		}
	}

	private void ᖍ()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_৮ == null)
		{
			_৮ = new Timer();
			_৮.Interval = 10;
			_৮.Tick += ᖎ;
			_৮.Enabled = true;
			_৮.Start();
		}
	}

	private void ᖎ(object _٠, EventArgs ؿ)
	{
		_৮.Stop();
		_৮.Enabled = false;
		_৮ = null;
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
				switch (ᕺ.TabAlignment)
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
			panel.Style.BackColor1.Color = ᕺ.ColorScheme.TabPanelBackground;
			panel.Style.BackColor2.Color = ᕺ.ColorScheme.TabPanelBackground2;
			switch (ᕺ.TabAlignment)
			{
			case eTabStripAlignment.Top:
				panel.Style.GradientAngle = ᕺ.ColorScheme.TabPanelBackgroundGradientAngle;
				break;
			case eTabStripAlignment.Bottom:
				panel.Style.GradientAngle = ᕺ.ColorScheme.TabPanelBackgroundGradientAngle - 180;
				break;
			case eTabStripAlignment.Left:
				panel.Style.GradientAngle = ᕺ.ColorScheme.TabPanelBackgroundGradientAngle - 90;
				break;
			case eTabStripAlignment.Right:
				panel.Style.GradientAngle = ᕺ.ColorScheme.TabPanelBackgroundGradientAngle + 90;
				break;
			}
			panel.Style.BorderColor.Color = ᕺ.ColorScheme.TabPanelBorder;
		}
		panel.Style.Border = eBorderType.SingleLine;
		if (ᕼ)
		{
			switch (ᕺ.TabAlignment)
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
		if (ᕺ.ᓨ)
		{
			panel.ᑲ = true;
		}
		else
		{
			panel.ᑲ = false;
		}
	}

	private void ᖏ()
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
		ᖏ();
	}

	public void ResumeLayout(bool performLayout)
	{
		((Control)this).ResumeLayout(performLayout);
		if (!((Control)this).IsHandleCreated)
		{
			ᕿ = true;
			return;
		}
		ᕺ.ResetTabHeight();
		SyncTabStripSize();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		MethodInvoker val = null;
		if (ᕿ || ((Component)this).DesignMode)
		{
			ᕿ = false;
			ᕺ.ResetTabHeight();
			SyncTabStripSize();
			if (((Component)this).DesignMode)
			{
				if (val == null)
				{
					val = (MethodInvoker)delegate
					{
						((Control)this).PerformLayout();
					};
				}
				ڣ(val);
			}
		}
		((Control)this).OnHandleCreated(e);
	}

	private void ᖐ()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (ᖁ)
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

	private void ᖑ(object _٠, EventArgs ؿ)
	{
		SyncTabStripSize();
	}

	public void SyncTabStripSize()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected I4, but got Unknown
		if (ᕾ)
		{
			return;
		}
		ᕾ = true;
		try
		{
			DockStyle dock = ((Control)ᕺ).Dock;
			switch (dock - 3)
			{
			case 0:
			case 1:
				((Control)ᕺ).Width = ᕺ.MinTabStripHeight;
				break;
			default:
				((Control)ᕺ).Height = ᕺ.MinTabStripHeight;
				break;
			}
			((Control)ᕺ).SendToBack();
		}
		finally
		{
			ᕾ = false;
		}
	}

	private void ڣ(MethodInvoker ڤ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = ڤ;
		val.Interval = 10;
		val.Tick += ڥ;
		val.Start();
	}

	private void ڥ(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)_٠;
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
		if (insertAt < 0 || insertAt >= ᕺ.Tabs.Count)
		{
			ᕺ.Tabs.Add(tabItem);
			((Control)this).Controls.Add((Control)(object)tabControlPanel);
		}
		else
		{
			ᕺ.Tabs.Insert(insertAt, tabItem);
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
		if (_ᖀ)
		{
			if ((int)keyData == 131081)
			{
				ᕺ._0C71(eEventSource.Keyboard, _0C70: true);
				return true;
			}
			if ((int)keyData == 196617)
			{
				ᕺ._౯(eEventSource.Keyboard, _0C70: true);
				return true;
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	private void ህ()
	{
		ᖁ = true;
	}

	void ISupportInitialize.BeginInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ህ
		this.ህ();
	}

	private void ሆ()
	{
		ᖁ = false;
		ᖐ();
		if (ᕽ >= 0)
		{
			ᕺ.SelectedTabIndex = ᕽ;
		}
		ᕽ = -1;
		if (ᕺ.SelectedTab != null && ᕺ.SelectedTab.AttachedControl != null)
		{
			ᕺ.SelectedTab.AttachedControl.Visible = true;
		}
	}

	void ISupportInitialize.EndInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ሆ
		this.ሆ();
	}

	public bool SelectPreviousTab()
	{
		return ᕺ.SelectPreviousTab();
	}

	public bool SelectNextTab()
	{
		return ᕺ.SelectNextTab();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (Style == eTabStripStyle.Office2007Dock || Style == eTabStripStyle.Office2007Document)
		{
			ᕺ.Style = Style;
			ᖐ();
		}
		((Control)this).OnParentChanged(e);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return ᕺ.ColorScheme.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		ᕺ.ResetColorScheme();
		if (((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		ᕺ.ResetSelectedTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return ᕺ.ShouldSerializeFixedTabSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedTabSize()
	{
		TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
	}

	[CompilerGenerated]
	private void ᖒ()
	{
		((Control)this).PerformLayout();
	}
}
