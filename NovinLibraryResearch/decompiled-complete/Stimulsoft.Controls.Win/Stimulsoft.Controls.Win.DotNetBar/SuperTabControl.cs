using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DefaultEvent("SelectedTabChanged")]
public class SuperTabControl : Control, ISupportInitialize
{
	private readonly SuperTabStrip _TabStrip;

	private bool _Initializing;

	private bool _TabsVisible = true;

	private int _LoadSelectedTabIndex = -1;

	public SuperTabStrip SuperTabStrip => _TabStrip;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether TabStrip will get focus when Tab key is used.")]
	public bool TabStripTabStop
	{
		get
		{
			return ((Control)TabStrip).TabStop;
		}
		set
		{
			((Control)TabStrip).TabStop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return _TabStrip.AntiAlias;
		}
		set
		{
			_TabStrip.AntiAlias = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Close Button")]
	[Description("Indicates whether tab is automatically closed when close button is clicked.")]
	public bool AutoCloseTabs
	{
		get
		{
			return _TabStrip.AutoCloseTabs;
		}
		set
		{
			_TabStrip.AutoCloseTabs = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether the tab's visible close button is displayed for every tab state.")]
	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return _TabStrip.CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			_TabStrip.CloseButtonOnTabsAlwaysDisplayed = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates whether close button is visible on each tab.")]
	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return _TabStrip.CloseButtonOnTabsVisible;
		}
		set
		{
			_TabStrip.CloseButtonOnTabsVisible = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabCloseButtonPosition.Right)]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[Description("Indicates the position of the tab close button.")]
	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return _TabStrip.CloseButtonPosition;
		}
		set
		{
			_TabStrip.CloseButtonPosition = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates the custom tab Close button image.")]
	public Image TabCloseButtonNormal
	{
		get
		{
			return _TabStrip.TabCloseButtonNormal;
		}
		set
		{
			_TabStrip.TabCloseButtonNormal = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom Close button image that is used on tabs when the mouse is over the close button.")]
	public Image TabCloseButtonHot
	{
		get
		{
			return _TabStrip.TabCloseButtonHot;
		}
		set
		{
			_TabStrip.TabCloseButtonHot = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Description("Indicates custom Close button image that is used on tabs when the button has been pressed.")]
	public Image TabCloseButtonPressed
	{
		get
		{
			return _TabStrip.TabCloseButtonPressed;
		}
		set
		{
			_TabStrip.TabCloseButtonPressed = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the TabStrip ControlBox.")]
	public SuperTabControlBox ControlBox => _TabStrip.ControlBox;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Specifies whether the only Text displayed is for the SelectedTab.")]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return _TabStrip.DisplaySelectedTextOnly;
		}
		set
		{
			_TabStrip.DisplaySelectedTextOnly = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text is drawn horizontally regardless of tab orientation.")]
	public bool HorizontalText
	{
		get
		{
			return _TabStrip.HorizontalText;
		}
		set
		{
			_TabStrip.HorizontalText = value;
		}
	}

	[DefaultValue(ImageAlignment.NotSet)]
	[Category("Appearance")]
	[Description("Indicates the default alignment of the Image within the tab.")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return _TabStrip.ImageAlignment;
		}
		set
		{
			_TabStrip.ImageAlignment = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates the ImageList used by the TabStrip and its tab items.")]
	public ImageList ImageList
	{
		get
		{
			return _TabStrip.ImageList;
		}
		set
		{
			_TabStrip.ImageList = value;
		}
	}

	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates BaseItem tab padding in pixels.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding ItemPadding
	{
		get
		{
			return _TabStrip.ItemPadding;
		}
		set
		{
			_TabStrip.ItemPadding = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether vertical tab text is rotated 180 degrees.")]
	public bool RotateVerticalText
	{
		get
		{
			return _TabStrip.RotateVerticalText;
		}
		set
		{
			_TabStrip.RotateVerticalText = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SuperTabControlPanel SelectedPanel
	{
		get
		{
			if (_TabStrip.SelectedTab != null)
			{
				return _TabStrip.SelectedTab.AttachedControl as SuperTabControlPanel;
			}
			return null;
		}
		set
		{
			if (value != null && ((Control)this).Controls.Contains((Control)(object)value) && value.TabItem != null && Tabs.Contains(value.TabItem))
			{
				_TabStrip.SelectedTab = value.TabItem;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether a focus rectangle is displayed when tab has input focus.")]
	public bool ShowFocusRectangle
	{
		get
		{
			return _TabStrip.ShowFocusRectangle;
		}
		set
		{
			_TabStrip.ShowFocusRectangle = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the fixed tab size in pixels. Either Height, Width, or both can be set.")]
	public Size FixedTabSize
	{
		get
		{
			return _TabStrip.FixedTabSize;
		}
		set
		{
			_TabStrip.FixedTabSize = value;
		}
	}

	[Browsable(false)]
	public bool IsTabDragging => _TabStrip.IsTabDragging;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control tabs can be reordered through the user interface.")]
	public bool ReorderTabsEnabled
	{
		get
		{
			return _TabStrip.ReorderTabsEnabled;
		}
		set
		{
			_TabStrip.ReorderTabsEnabled = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SuperTabItem SelectedTab
	{
		get
		{
			return _TabStrip.SelectedTab;
		}
		set
		{
			_TabStrip.SelectedTab = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Localizable(true)]
	[Category("Style")]
	[DefaultValue(null)]
	[Description("Indicates the selected tab Font")]
	public Font SelectedTabFont
	{
		get
		{
			return _TabStrip.SelectedTabFont;
		}
		set
		{
			_TabStrip.SelectedTabFont = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates the index of the selected tab.")]
	public int SelectedTabIndex
	{
		get
		{
			return _TabStrip.SelectedTabIndex;
		}
		set
		{
			if (_Initializing)
			{
				_LoadSelectedTabIndex = value;
			}
			else
			{
				_TabStrip.SelectedTabIndex = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabStripAlignment.Top)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the tab alignment within the Tab-Strip control.")]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			return _TabStrip.TabAlignment;
		}
		set
		{
			if (_TabStrip.TabAlignment != value)
			{
				_TabStrip.TabAlignment = value;
				SetTabDocking(value);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Localizable(true)]
	[Category("Style")]
	[DefaultValue(null)]
	[Description("Indicates the tab Font")]
	public Font TabFont
	{
		get
		{
			return _TabStrip.TabFont;
		}
		set
		{
			_TabStrip.TabFont = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(5)]
	[Category("Appearance")]
	[Description("Indicates the Horizontal spacing around tab elements.")]
	public int TabHorizontalSpacing
	{
		get
		{
			return _TabStrip.TabHorizontalSpacing;
		}
		set
		{
			_TabStrip.TabHorizontalSpacing = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eSuperTabLayoutType.SingleLine)]
	[Category("Appearance")]
	[Description("Indicates the type of the tab layout.")]
	public eSuperTabLayoutType TabLayoutType
	{
		get
		{
			return _TabStrip.TabLayoutType;
		}
		set
		{
			_TabStrip.TabLayoutType = value;
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SuperTabControlTabsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[Description("Indicates the collection of Tabs.")]
	public SubItemsCollection Tabs => _TabStrip.Tabs;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SuperTabStrip TabStrip => _TabStrip;

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the Color of the TabStrip.")]
	public SuperTabColorTable TabStripColor
	{
		get
		{
			return _TabStrip.TabStripColor;
		}
		set
		{
			_TabStrip.TabStripColor = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eSuperTabStyle.Office2007)]
	[Category("Appearance")]
	[Description("Indicates the tab style.")]
	public eSuperTabStyle TabStyle
	{
		get
		{
			return _TabStrip.TabStyle;
		}
		set
		{
			if (_TabStrip.TabStyle != value)
			{
				_TabStrip.TabStyle = value;
				RefreshPanelsStyle();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(4)]
	[Category("Appearance")]
	[Description("Indicates the Vertical spacing around tab elements.")]
	public int TabVerticalSpacing
	{
		get
		{
			return _TabStrip.TabVerticalSpacing;
		}
		set
		{
			_TabStrip.TabVerticalSpacing = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether tabs are visible")]
	public bool TabsVisible
	{
		get
		{
			return _TabsVisible;
		}
		set
		{
			if (_TabsVisible != value)
			{
				_TabsVisible = value;
				_TabStrip.Visible = value;
				RefreshPanelsStyle();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eItemAlignment.Near)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates tab text alignment.")]
	public eItemAlignment TextAlignment
	{
		get
		{
			return _TabStrip.TextAlignment;
		}
		set
		{
			_TabStrip.TextAlignment = value;
		}
	}

	protected override Size DefaultSize => new Size(200, 100);

	[Description("Occurs when a tab is added to the Tabs collection.")]
	public event EventHandler<SuperTabStripTabItemOpenEventArgs> TabItemOpen;

	[Description("Occurs when a tab is about to Close.")]
	public event EventHandler<SuperTabStripTabItemCloseEventArgs> TabItemClose;

	[Description("Occurs when a tab is removed from the Tabs collection.")]
	public event EventHandler<SuperTabStripTabRemovedEventArgs> TabRemoved;

	[Description("Occurs when a tab is about to be displayed.")]
	public event EventHandler<SuperTabStripBeforeTabDisplayEventArgs> BeforeTabDisplay;

	[Description("Occurs when a tab is being moved or dragged by the user.")]
	public event EventHandler<SuperTabStripTabMovingEventArgs> TabMoving;

	[Description("Occurs when a tab has been moved or dragged by the user.")]
	public event EventHandler<SuperTabStripTabMovedEventArgs> TabMoved;

	[Description("Occurs when the Selected tab is changing.")]
	public event EventHandler<SuperTabStripSelectedTabChangingEventArgs> SelectedTabChanging;

	[Description("Occurs when the Selected tab has changed.")]
	public event EventHandler<SuperTabStripSelectedTabChangedEventArgs> SelectedTabChanged;

	[Description("Occurs when the control needs a tab's bordering path.")]
	public event EventHandler<SuperTabGetTabItemPathEventArgs> GetTabItemPath;

	[Description("Occurs when the control needs a tab's Content Rectangle.")]
	public event EventHandler<SuperTabGetTabItemContentRectangleEventArgs> GetTabItemContentRectangle;

	[Description("Occurs when the control needs to measure a tab.")]
	public event EventHandler<SuperTabMeasureTabItemEventArgs> MeasureTabItem;

	[Description("Occurs before any tab rendering is done.")]
	public event EventHandler<SuperTabPreRenderTabItemEventArgs> PreRenderTabItem;

	[Description("Occurs After all tab rendering is complete.")]
	public event EventHandler<SuperTabPostRenderTabItemEventArgs> PostRenderTabItem;

	[Description("Occurs when the control needs to get the tab's Text Bounds.")]
	public event EventHandler<SuperTabGetTabTextBoundsEventArgs> GetTabTextBounds;

	[Description("Occurs when the control needs to get the tab's Image Bounds.")]
	public event EventHandler<SuperTabGetTabImageBoundsEventArgs> GetTabImageBounds;

	[Description("Occurs when the control needs to get the tab's Close Button Bounds.")]
	public event EventHandler<SuperTabGetTabCloseBoundsEventArgs> GetTabCloseBounds;

	[Description("Occurs when the TabStrip background needs painted.")]
	public event EventHandler<SuperTabStripPaintBackgroundEventArgs> TabStripPaintBackground;

	[Description("Occurs when a TabStrip MouseUp event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseUp;

	[Description("Occurs when a TabStrip MouseDown event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseDown;

	[Description("Occurs when a TabStrip MouseMove event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseMove;

	[Description("Occurs when a TabStrip MouseClick event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseClick;

	[Description("Occurs when a TabStrip MouseDoubleClick event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseDoubleClick;

	[Description("Occurs when a TabStrip MouseEnter event is raised.")]
	public event EventHandler<EventArgs> TabStripMouseEnter;

	[Description("Occurs when a TabStrip MouseHover event is raised.")]
	public event EventHandler<EventArgs> TabStripMouseHover;

	[Description("Occurs when a TabStrip MouseLeave event is raised.")]
	public event EventHandler<EventArgs> TabStripMouseLeave;

	public SuperTabControl()
	{
		_TabStrip = new SuperTabStrip();
		((Control)this).Controls.Add((Control)(object)_TabStrip);
		((Control)_TabStrip).Dock = (DockStyle)1;
		_TabStrip.AutoSelectAttachedControl = true;
		((Control)_TabStrip).SendToBack();
		HookEvents(hook: true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		StyleManager.Register((Control)(object)this);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		if (TabStripTabStop)
		{
			((Control)TabStrip).Focus();
		}
		((Control)this).OnGotFocus(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return _TabStrip.ShouldSerializeFixedTabSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedTabSize()
	{
		TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedTabFont()
	{
		return _TabStrip.ShouldSerializeSelectedTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		_TabStrip.ResetSelectedTabFont();
	}

	private void SetTabDocking(eTabStripAlignment value)
	{
		((Control)_TabStrip).SuspendLayout();
		try
		{
			switch (value)
			{
			case eTabStripAlignment.Top:
				((Control)_TabStrip).Dock = (DockStyle)1;
				break;
			case eTabStripAlignment.Bottom:
				((Control)_TabStrip).Dock = (DockStyle)2;
				break;
			case eTabStripAlignment.Left:
				((Control)_TabStrip).Dock = (DockStyle)3;
				break;
			case eTabStripAlignment.Right:
				((Control)_TabStrip).Dock = (DockStyle)4;
				break;
			}
			((Control)_TabStrip).SendToBack();
		}
		finally
		{
			((Control)_TabStrip).ResumeLayout(true);
		}
		RefreshPanelsStyle();
		RecalcLayout();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabFont()
	{
		return _TabStrip.ShouldSerializeTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		_TabStrip.ResetTabFont();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabStripColor()
	{
		return !_TabStrip.TabStripColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabStripColor()
	{
		_TabStrip.TabStripColor = new SuperTabColorTable();
	}

	public void ApplyStyle()
	{
		RefreshPanelsStyle();
	}

	private void HookEvents(bool hook)
	{
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		if (hook)
		{
			_TabStrip.TabItemOpen += TabStripTabItemOpen;
			_TabStrip.TabItemClose += TabStripTabItemClose;
			_TabStrip.TabRemoved += TabStripTabRemoved;
			_TabStrip.TabMoving += TabStripTabMoving;
			_TabStrip.TabMoved += TabStripTabMoved;
			_TabStrip.SelectedTabChanging += TabStripSelectedTabChanging;
			_TabStrip.SelectedTabChanged += TabStripSelectedTabChanged;
			_TabStrip.GetTabItemPath += TabStripItemGetTabItemPath;
			_TabStrip.GetTabItemContentRectangle += TabStripItemGetTabItemContentRectangle;
			_TabStrip.BeforeTabDisplay += TabStripBeforeTabDisplay;
			_TabStrip.MeasureTabItem += TabStripMeasureTabItem;
			_TabStrip.PreRenderTabItem += TabStripPreRenderTabItem;
			_TabStrip.PostRenderTabItem += TabStripPostRenderTabItem;
			_TabStrip.GetTabTextBounds += TabStripGetTabTextBounds;
			_TabStrip.GetTabImageBounds += TabStripGetTabImageBounds;
			_TabStrip.GetTabCloseBounds += TabStripGetTabCloseBounds;
			_TabStrip.TabStripPaintBackground += TabStripTabStripPaintBackground;
			_TabStrip.TabStripTabColorChanged += TabStripTabColorChanged;
			_TabStrip.MouseUp += new MouseEventHandler(TabStrip_MouseUp);
			_TabStrip.MouseDown += new MouseEventHandler(TabStrip_MouseDown);
			_TabStrip.MouseMove += new MouseEventHandler(TabStrip_MouseMove);
			((Control)_TabStrip).MouseClick += new MouseEventHandler(TabStrip_MouseClick);
			((Control)_TabStrip).MouseDoubleClick += new MouseEventHandler(TabStrip_MouseDoubleClick);
			_TabStrip.MouseEnter += TabStrip_MouseEnter;
			_TabStrip.MouseHover += TabStrip_MouseHover;
			_TabStrip.MouseLeave += TabStrip_MouseLeave;
		}
		else
		{
			_TabStrip.TabItemOpen -= TabStripTabItemOpen;
			_TabStrip.TabItemClose -= TabStripTabItemClose;
			_TabStrip.TabRemoved -= TabStripTabRemoved;
			_TabStrip.TabMoving -= TabStripTabMoving;
			_TabStrip.TabMoved -= TabStripTabMoved;
			_TabStrip.SelectedTabChanging -= TabStripSelectedTabChanging;
			_TabStrip.SelectedTabChanged -= TabStripSelectedTabChanged;
			_TabStrip.GetTabItemPath -= TabStripItemGetTabItemPath;
			_TabStrip.GetTabItemContentRectangle -= TabStripItemGetTabItemContentRectangle;
			_TabStrip.BeforeTabDisplay -= TabStripBeforeTabDisplay;
			_TabStrip.MeasureTabItem -= TabStripMeasureTabItem;
			_TabStrip.PreRenderTabItem -= TabStripPreRenderTabItem;
			_TabStrip.PostRenderTabItem -= TabStripPostRenderTabItem;
			_TabStrip.GetTabTextBounds -= TabStripGetTabTextBounds;
			_TabStrip.GetTabImageBounds -= TabStripGetTabImageBounds;
			_TabStrip.GetTabCloseBounds -= TabStripGetTabCloseBounds;
			_TabStrip.TabStripPaintBackground -= TabStripTabStripPaintBackground;
			_TabStrip.TabStripTabColorChanged -= TabStripTabColorChanged;
			_TabStrip.MouseUp -= new MouseEventHandler(TabStrip_MouseUp);
			_TabStrip.MouseDown -= new MouseEventHandler(TabStrip_MouseDown);
			_TabStrip.MouseMove -= new MouseEventHandler(TabStrip_MouseMove);
			((Control)_TabStrip).MouseClick -= new MouseEventHandler(TabStrip_MouseClick);
			((Control)_TabStrip).MouseDoubleClick -= new MouseEventHandler(TabStrip_MouseDoubleClick);
			_TabStrip.MouseEnter -= TabStrip_MouseEnter;
			_TabStrip.MouseHover -= TabStrip_MouseHover;
			_TabStrip.MouseLeave -= TabStrip_MouseLeave;
		}
	}

	private void TabStripTabItemOpen(object sender, SuperTabStripTabItemOpenEventArgs e)
	{
		if (this.TabItemOpen != null)
		{
			this.TabItemOpen(this, e);
		}
	}

	private void TabStripTabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
	{
		if (this.TabItemClose != null)
		{
			this.TabItemClose(this, e);
		}
	}

	private void TabStripTabMoving(object sender, SuperTabStripTabMovingEventArgs e)
	{
		if (this.TabMoving != null)
		{
			this.TabMoving(this, e);
		}
	}

	private void TabStripTabMoved(object sender, SuperTabStripTabMovedEventArgs e)
	{
		if (this.TabMoved != null)
		{
			this.TabMoved(this, e);
		}
	}

	private void TabStripTabRemoved(object sender, SuperTabStripTabRemovedEventArgs e)
	{
		if (this.TabRemoved != null)
		{
			this.TabRemoved(this, e);
		}
	}

	private void TabStripSelectedTabChanging(object sender, SuperTabStripSelectedTabChangingEventArgs e)
	{
		if (this.SelectedTabChanging != null)
		{
			this.SelectedTabChanging(this, e);
		}
	}

	private void TabStripSelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
	{
		if (this.SelectedTabChanged != null)
		{
			this.SelectedTabChanged(this, e);
		}
	}

	private void TabStripItemGetTabItemPath(object sender, SuperTabGetTabItemPathEventArgs e)
	{
		if (this.GetTabItemPath != null)
		{
			this.GetTabItemPath(this, e);
		}
	}

	private void TabStripItemGetTabItemContentRectangle(object sender, SuperTabGetTabItemContentRectangleEventArgs e)
	{
		if (this.GetTabItemContentRectangle != null)
		{
			this.GetTabItemContentRectangle(this, e);
		}
	}

	private void TabStripBeforeTabDisplay(object sender, SuperTabStripBeforeTabDisplayEventArgs e)
	{
		if (this.BeforeTabDisplay != null)
		{
			this.BeforeTabDisplay(this, e);
		}
	}

	private void TabStripMeasureTabItem(object sender, SuperTabMeasureTabItemEventArgs e)
	{
		if (this.MeasureTabItem != null)
		{
			this.MeasureTabItem(this, e);
		}
	}

	private void TabStripPreRenderTabItem(object sender, SuperTabPreRenderTabItemEventArgs e)
	{
		if (this.PreRenderTabItem != null)
		{
			this.PreRenderTabItem(this, e);
		}
	}

	private void TabStripPostRenderTabItem(object sender, SuperTabPostRenderTabItemEventArgs e)
	{
		if (this.PostRenderTabItem != null)
		{
			this.PostRenderTabItem(this, e);
		}
	}

	private void TabStripGetTabCloseBounds(object sender, SuperTabGetTabCloseBoundsEventArgs e)
	{
		if (this.GetTabCloseBounds != null)
		{
			this.GetTabCloseBounds(this, e);
		}
	}

	private void TabStripGetTabImageBounds(object sender, SuperTabGetTabImageBoundsEventArgs e)
	{
		if (this.GetTabImageBounds != null)
		{
			this.GetTabImageBounds(this, e);
		}
	}

	private void TabStripGetTabTextBounds(object sender, SuperTabGetTabTextBoundsEventArgs e)
	{
		if (this.GetTabTextBounds != null)
		{
			this.GetTabTextBounds(this, e);
		}
	}

	private void TabStripTabStripPaintBackground(object sender, SuperTabStripPaintBackgroundEventArgs e)
	{
		if (this.TabStripPaintBackground != null)
		{
			this.TabStripPaintBackground(this, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		RefreshPanelsStyle();
	}

	private void TabStripTabColorChanged(object sender, SuperTabStripTabColorChangedEventArgs e)
	{
		if (e.Tab.AttachedControl is SuperTabControlPanel panel)
		{
			ApplyPanelStyle(panel);
		}
	}

	private void TabStrip_MouseUp(object sender, MouseEventArgs e)
	{
		if (this.TabStripMouseUp != null)
		{
			this.TabStripMouseUp(sender, e);
		}
	}

	private void TabStrip_MouseDown(object sender, MouseEventArgs e)
	{
		if (this.TabStripMouseDown != null)
		{
			this.TabStripMouseDown(sender, e);
		}
	}

	private void TabStrip_MouseMove(object sender, MouseEventArgs e)
	{
		if (this.TabStripMouseMove != null)
		{
			this.TabStripMouseMove(sender, e);
		}
	}

	private void TabStrip_MouseClick(object sender, MouseEventArgs e)
	{
		if (this.TabStripMouseClick != null)
		{
			this.TabStripMouseClick(sender, e);
		}
	}

	private void TabStrip_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (this.TabStripMouseDoubleClick != null)
		{
			this.TabStripMouseDoubleClick(sender, e);
		}
	}

	private void TabStrip_MouseEnter(object sender, EventArgs e)
	{
		if (this.TabStripMouseEnter != null)
		{
			this.TabStripMouseEnter(sender, e);
		}
	}

	private void TabStrip_MouseHover(object sender, EventArgs e)
	{
		if (this.TabStripMouseHover != null)
		{
			this.TabStripMouseHover(sender, e);
		}
	}

	private void TabStrip_MouseLeave(object sender, EventArgs e)
	{
		if (this.TabStripMouseLeave != null)
		{
			this.TabStripMouseLeave(sender, e);
		}
	}

	public SuperTabItem CreateTab(string tabText)
	{
		return CreateTab(tabText, -1);
	}

	public SuperTabItem CreateTab(string tabText, int insertAt)
	{
		SuperTabItem superTabItem = new SuperTabItem();
		SuperTabControlPanel panel = new SuperTabControlPanel();
		superTabItem.Text = tabText;
		return CreateTab(superTabItem, panel, insertAt);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public SuperTabItem CreateTab(SuperTabItem tab, SuperTabControlPanel panel, int insertAt)
	{
		((Control)this).SuspendLayout();
		tab.AttachedControl = (Control)(object)panel;
		panel.TabItem = tab;
		((Control)this).Controls.Add((Control)(object)panel);
		if (insertAt < 0 || insertAt >= Tabs.Count)
		{
			Tabs.Add(tab);
		}
		else
		{
			Tabs.Insert(insertAt, tab);
		}
		ApplyPanelStyle(panel);
		((Control)panel).Size = Size.Empty;
		((Control)panel).Dock = (DockStyle)5;
		((Control)panel).BringToFront();
		((Control)this).ResumeLayout();
		return tab;
	}

	public bool SelectPreviousTab()
	{
		return _TabStrip.SelectPreviousTab();
	}

	public bool SelectNextTab()
	{
		return _TabStrip.SelectNextTab();
	}

	public SuperTabItem GetTabFromPoint(Point pt)
	{
		return _TabStrip.GetTabFromPoint(pt);
	}

	public BaseItem GetItemFromPoint(Point pt)
	{
		return _TabStrip.GetItemFromPoint(pt);
	}

	public void CloseTab(SuperTabItem tab)
	{
		_TabStrip.CloseTab(tab);
	}

	public void RecalcLayout()
	{
		_TabStrip.RecalcLayout();
	}

	internal void RefreshPanelsStyle()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (_Initializing)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is SuperTabControlPanel)
			{
				ApplyPanelStyle(val as SuperTabControlPanel);
			}
		}
	}

	public void ApplyPanelStyle(SuperTabControlPanel panel)
	{
		if (panel != null && panel.TabItem != null && panel.TabItem.TabStripItem != null)
		{
			ElementStyle elementStyle = new ElementStyle();
			ApplyPanelColor(panel, elementStyle);
			ApplyPanelBorder(elementStyle);
			elementStyle.BackgroundImage = ((Control)panel).BackgroundImage;
			elementStyle.BackgroundImagePosition = panel.BackgroundImagePosition;
			panel.PanelStyle = elementStyle;
		}
	}

	private void ApplyPanelColor(SuperTabControlPanel panel, ElementStyle style)
	{
		SuperTabPanelItemColorTable panelColorTable = panel.TabItem.GetPanelColorTable();
		SuperTabItemStateColorTable tabColorTable = panel.TabItem.GetTabColorTable(eTabState.Selected);
		int gradientAngle;
		bool? flag;
		if (!panelColorTable.Background.IsEmpty)
		{
			gradientAngle = panelColorTable.Background.GradientAngle;
			flag = panelColorTable.Background.AdaptiveGradient ?? tabColorTable.Background.AdaptiveGradient;
			style.BackColorBlend.Clear();
			if (panelColorTable.Background.Colors != null)
			{
				if (panelColorTable.Background.Colors.Length > 2)
				{
					style.BackColorBlend.CopyFrom(panelColorTable.Background.GetColorBlendCollection());
				}
				else
				{
					style.BackColor = panelColorTable.Background.Colors[0];
					style.BackColor2 = ((panelColorTable.Background.Colors.Length == 1) ? panelColorTable.Background.Colors[0] : panelColorTable.Background.Colors[1]);
				}
			}
		}
		else
		{
			gradientAngle = tabColorTable.Background.GradientAngle;
			flag = tabColorTable.Background.AdaptiveGradient;
			if (tabColorTable.Background.Colors != null)
			{
				style.BackColor = tabColorTable.Background.Colors[0];
				style.BackColor2 = ((tabColorTable.Background.Colors.Length == 1) ? tabColorTable.Background.Colors[0] : tabColorTable.Background.Colors[1]);
			}
		}
		if (flag != false)
		{
			switch (_TabStrip.TabAlignment)
			{
			case eTabStripAlignment.Top:
				style.BackColorGradientAngle = gradientAngle - 180;
				break;
			case eTabStripAlignment.Bottom:
				style.BackColorGradientAngle = gradientAngle;
				break;
			case eTabStripAlignment.Left:
				style.BackColorGradientAngle = gradientAngle + 90;
				break;
			case eTabStripAlignment.Right:
				style.BackColorGradientAngle = gradientAngle - 90;
				break;
			}
		}
		style.BorderColor = panelColorTable.OuterBorder;
		style.BorderColorLight = panelColorTable.InnerBorder;
	}

	private void ApplyPanelBorder(ElementStyle style)
	{
		if (TabsVisible)
		{
			style.BorderWidth = 0;
			if (!style.BorderColor.IsEmpty)
			{
				style.BorderWidth++;
			}
			if (style.BorderWidth == 0)
			{
				style.Border = eStyleBorderType.None;
			}
			else if (style.BorderColorLight.IsEmpty)
			{
				style.Border = eStyleBorderType.Solid;
			}
			else
			{
				style.Border = eStyleBorderType.Double;
			}
			if (_TabStrip.Available)
			{
				switch (_TabStrip.TabAlignment)
				{
				case eTabStripAlignment.Top:
					style.BorderTop = eStyleBorderType.None;
					break;
				case eTabStripAlignment.Bottom:
					style.BorderBottom = eStyleBorderType.None;
					break;
				case eTabStripAlignment.Left:
					style.BorderLeft = eStyleBorderType.None;
					break;
				case eTabStripAlignment.Right:
					style.BorderRight = eStyleBorderType.None;
					break;
				}
			}
		}
		else
		{
			style.Border = eStyleBorderType.None;
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		RefreshPanelsStyle();
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (_TabStrip != null)
		{
			((Control)_TabStrip).Size = ((Control)this).Size;
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode || !(e.Control is SuperTabControlPanel superTabControlPanel))
		{
			return;
		}
		if (superTabControlPanel.TabItem != null)
		{
			if (Tabs.Contains(superTabControlPanel.TabItem) && SelectedTab == superTabControlPanel.TabItem)
			{
				((Control)superTabControlPanel).Visible = true;
				((Control)superTabControlPanel).BringToFront();
			}
			else
			{
				((Control)superTabControlPanel).Visible = false;
			}
		}
		else
		{
			((Control)superTabControlPanel).Visible = false;
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (e.Control is SuperTabControlPanel { TabItem: not null } superTabControlPanel && Tabs.Contains(superTabControlPanel.TabItem))
		{
			Tabs.Remove(superTabControlPanel.TabItem);
		}
	}

	void ISupportInitialize.BeginInit()
	{
		_Initializing = true;
	}

	void ISupportInitialize.EndInit()
	{
		_Initializing = false;
		RefreshPanelsStyle();
		SuperTabItem newSelectTab = _TabStrip.TabStripItem.GetNewSelectTab(_LoadSelectedTabIndex);
		if (newSelectTab != null)
		{
			SelectedTab = newSelectTab;
			if (_TabStrip.SelectedTab != null && _TabStrip.SelectedTab.AttachedControl != null)
			{
				_TabStrip.SelectedTab.AttachedControl.Visible = true;
				_TabStrip.SelectedTab.AttachedControl.BringToFront();
			}
		}
		_LoadSelectedTabIndex = -1;
		((Control)this).ResumeLayout(true);
	}
}
