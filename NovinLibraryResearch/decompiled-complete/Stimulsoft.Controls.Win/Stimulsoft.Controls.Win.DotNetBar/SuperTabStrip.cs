using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public class SuperTabStrip : ItemControl, ISupportInitialize
{
	private BaseItem _DesignTimeSelection;

	private SuperTabStripItem _TabStripItem;

	private bool _IsInitializing;

	private bool _Available = true;

	private BaseItem _ApplicationButton;

	[Browsable(false)]
	public bool AutoSelectAttachedControl
	{
		get
		{
			return TabStripItem.AutoSelectAttachedControl;
		}
		set
		{
			TabStripItem.AutoSelectAttachedControl = value;
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
			return _TabStripItem.AutoCloseTabs;
		}
		set
		{
			_TabStripItem.AutoCloseTabs = value;
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
			return _TabStripItem.CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			_TabStripItem.CloseButtonOnTabsAlwaysDisplayed = value;
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
			return _TabStripItem.CloseButtonOnTabsVisible;
		}
		set
		{
			_TabStripItem.CloseButtonOnTabsVisible = value;
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
			return _TabStripItem.CloseButtonPosition;
		}
		set
		{
			_TabStripItem.CloseButtonPosition = value;
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
			return _TabStripItem.TabCloseButtonNormal;
		}
		set
		{
			_TabStripItem.TabCloseButtonNormal = value;
		}
	}

	public Image TabCloseButtonHot
	{
		get
		{
			return _TabStripItem.TabCloseButtonHot;
		}
		set
		{
			_TabStripItem.TabCloseButtonHot = value;
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
			return _TabStripItem.TabCloseButtonPressed;
		}
		set
		{
			_TabStripItem.TabCloseButtonPressed = value;
		}
	}

	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			_Available = value;
			((Control)this).Visible = value;
			if (((Control)this).Parent is SuperTabControl superTabControl)
			{
				superTabControl.RefreshPanelsStyle();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the TabStrip ControlBox.")]
	public SuperTabControlBox ControlBox => _TabStripItem.ControlBox;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BaseItem DesignTimeSelection
	{
		get
		{
			return _DesignTimeSelection;
		}
		set
		{
			_DesignTimeSelection = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Specifies whether the only Text displayed is for the SelectedTab.")]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return _TabStripItem.DisplaySelectedTextOnly;
		}
		set
		{
			_TabStripItem.DisplaySelectedTextOnly = value;
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
			return _TabStripItem.HorizontalText;
		}
		set
		{
			_TabStripItem.HorizontalText = value;
		}
	}

	[DefaultValue(ImageAlignment.NotSet)]
	[Category("Appearance")]
	[Description("Indicates the default alignment of the Image within the tab.")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return _TabStripItem.ImageAlignment;
		}
		set
		{
			_TabStripItem.ImageAlignment = value;
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
			return Images;
		}
		set
		{
			Images = value;
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
			return _TabStripItem.ItemPadding;
		}
		set
		{
			_TabStripItem.ItemPadding = value;
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
			return _TabStripItem.RotateVerticalText;
		}
		set
		{
			_TabStripItem.RotateVerticalText = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether focus rectangle is displayed when tab has input focus.")]
	public bool ShowFocusRectangle
	{
		get
		{
			return _TabStripItem.ShowFocusRectangle;
		}
		set
		{
			_TabStripItem.ShowFocusRectangle = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the fixed tab size in pixels. Either Height, Width, or both can be set.")]
	public Size FixedTabSize
	{
		get
		{
			return _TabStripItem.FixedTabSize;
		}
		set
		{
			_TabStripItem.FixedTabSize = value;
		}
	}

	[Browsable(false)]
	public bool IsTabDragging => _TabStripItem.IsTabDragging;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control tabs can be reordered through the user interface.")]
	public bool ReorderTabsEnabled
	{
		get
		{
			return _TabStripItem.ReorderTabsEnabled;
		}
		set
		{
			_TabStripItem.ReorderTabsEnabled = value;
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
			return _TabStripItem.SelectedTab;
		}
		set
		{
			_TabStripItem.SelectedTab = value;
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
			return _TabStripItem.SelectedTabFont;
		}
		set
		{
			_TabStripItem.SelectedTabFont = value;
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
			return _TabStripItem.SelectedTabIndex;
		}
		set
		{
			_TabStripItem.SelectedTabIndex = value;
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
			return _TabStripItem.TabAlignment;
		}
		set
		{
			_TabStripItem.TabAlignment = value;
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
			return _TabStripItem.TabFont;
		}
		set
		{
			_TabStripItem.TabFont = value;
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
			return _TabStripItem.TabHorizontalSpacing;
		}
		set
		{
			_TabStripItem.TabHorizontalSpacing = value;
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
			return _TabStripItem.TabLayoutType;
		}
		set
		{
			_TabStripItem.TabLayoutType = value;
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SuperTabStripTabsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[Description("Returns the collection of Tabs.")]
	public SubItemsCollection Tabs => _TabStripItem.SubItems;

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the Color of the TabStrip.")]
	public SuperTabColorTable TabStripColor
	{
		get
		{
			return _TabStripItem.TabStripColor;
		}
		set
		{
			_TabStripItem.TabStripColor = value;
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
			return _TabStripItem.TabStyle;
		}
		set
		{
			_TabStripItem.TabStyle = value;
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
			return _TabStripItem.TabVerticalSpacing;
		}
		set
		{
			_TabStripItem.TabVerticalSpacing = value;
		}
	}

	[Browsable(false)]
	public SuperTabStripItem TabStripItem => _TabStripItem;

	[Browsable(true)]
	[DefaultValue(eItemAlignment.Near)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates tab text alignment.")]
	public eItemAlignment TextAlignment
	{
		get
		{
			return _TabStripItem.TextAlignment;
		}
		set
		{
			_TabStripItem.TextAlignment = value;
		}
	}

	internal bool Available => _Available;

	internal bool IsDesignMode
	{
		get
		{
			if (((Control)this).Parent != null && ((Control)this).Parent is SuperTabControl && ((Component)(object)((Control)this).Parent).Site != null)
			{
				return ((Component)(object)((Control)this).Parent).Site.DesignMode;
			}
			return ((Component)(object)this).DesignMode;
		}
	}

	internal bool IsInitializing => _IsInitializing;

	internal SuperTabStripBaseDisplay TabDisplay => _TabStripItem.TabDisplay;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BaseItem ApplicationButton
	{
		get
		{
			return _ApplicationButton;
		}
		set
		{
			_ApplicationButton = value;
		}
	}

	protected override Size DefaultSize => new Size(236, 25);

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

	[Description("Occurs when the TabStrip TabColor has changed.")]
	public event EventHandler<SuperTabStripTabColorChangedEventArgs> TabStripTabColorChanged;

	public SuperTabStrip()
	{
		_TabStripItem = new SuperTabStripItem(this);
		_TabStripItem.GlobalItem = false;
		_TabStripItem.ContainerControl = this;
		_TabStripItem.Stretch = false;
		_TabStripItem.Displayed = true;
		_TabStripItem.SetOwner(this);
		SetBaseItemContainer(_TabStripItem);
		((Control)this).SetStyle((ControlStyles)2048, true);
		HookEvents(hook: true);
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return _TabStripItem.ShouldSerializeFixedTabSize();
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
		return _TabStripItem.ShouldSerializeSelectedTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		_TabStripItem.ResetSelectedTabFont();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabFont()
	{
		return _TabStripItem.ShouldSerializeTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		_TabStripItem.ResetTabFont();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabStripColor()
	{
		return !_TabStripItem.TabStripColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabStripColor()
	{
		_TabStripItem.TabStripColor = new SuperTabColorTable();
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			_TabStripItem.TabItemOpen += TabStripItemTabItemOpen;
			_TabStripItem.TabItemClose += TabStripItem_TabItemClose;
			_TabStripItem.TabRemoved += TabStripItem_TabRemoved;
			_TabStripItem.TabMoving += TabStripItem_TabMoving;
			_TabStripItem.TabMoved += TabStripItem_TabMoved;
			_TabStripItem.GetTabItemPath += TabStripItem_GetTabItemPath;
			_TabStripItem.GetTabItemContentRectangle += TabStripItem_GetTabItemContentRectangle;
			_TabStripItem.BeforeTabDisplay += TabStripItem_BeforeTabDisplay;
			_TabStripItem.MeasureTabItem += TabStripItem_MeasureTabItem;
			_TabStripItem.PreRenderTabItem += TabStripItem_PreRenderTabItem;
			_TabStripItem.PostRenderTabItem += TabStripItem_PostRenderTabItem;
			_TabStripItem.GetTabTextBounds += TabStripItem_GetTabTextBounds;
			_TabStripItem.GetTabImageBounds += TabStripItem_GetTabImageBounds;
			_TabStripItem.GetTabCloseBounds += TabStripItem_GetTabCloseBounds;
			_TabStripItem.TabStripPaintBackground += TabStripItem_TabStripPaintBackground;
			_TabStripItem.SelectedTabChanging += TabStripItem_SelectedTabChanging;
			_TabStripItem.SelectedTabChanged += TabStripItem_SelectedTabChanged;
			_TabStripItem.TabStripTabColorChanged += TabStripItem_TabStripTabColorChanged;
		}
		else
		{
			_TabStripItem.TabItemOpen -= TabStripItemTabItemOpen;
			_TabStripItem.TabItemClose -= TabStripItem_TabItemClose;
			_TabStripItem.TabRemoved -= TabStripItem_TabRemoved;
			_TabStripItem.TabMoving -= TabStripItem_TabMoving;
			_TabStripItem.TabMoved -= TabStripItem_TabMoved;
			_TabStripItem.GetTabItemPath -= TabStripItem_GetTabItemPath;
			_TabStripItem.GetTabItemContentRectangle -= TabStripItem_GetTabItemContentRectangle;
			_TabStripItem.BeforeTabDisplay -= TabStripItem_BeforeTabDisplay;
			_TabStripItem.MeasureTabItem -= TabStripItem_MeasureTabItem;
			_TabStripItem.PreRenderTabItem -= TabStripItem_PreRenderTabItem;
			_TabStripItem.PostRenderTabItem -= TabStripItem_PostRenderTabItem;
			_TabStripItem.GetTabTextBounds -= TabStripItem_GetTabTextBounds;
			_TabStripItem.GetTabImageBounds -= TabStripItem_GetTabImageBounds;
			_TabStripItem.GetTabCloseBounds -= TabStripItem_GetTabCloseBounds;
			_TabStripItem.TabStripPaintBackground -= TabStripItem_TabStripPaintBackground;
			_TabStripItem.SelectedTabChanging -= TabStripItem_SelectedTabChanging;
			_TabStripItem.SelectedTabChanged -= TabStripItem_SelectedTabChanged;
			_TabStripItem.TabStripTabColorChanged -= TabStripItem_TabStripTabColorChanged;
		}
	}

	private void TabStripItemTabItemOpen(object sender, SuperTabStripTabItemOpenEventArgs e)
	{
		if (this.TabItemOpen != null)
		{
			this.TabItemOpen(this, e);
		}
	}

	private void TabStripItem_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
	{
		if (this.TabItemClose != null)
		{
			this.TabItemClose(this, e);
		}
	}

	private void TabStripItem_TabRemoved(object sender, SuperTabStripTabRemovedEventArgs e)
	{
		if (this.TabRemoved != null)
		{
			this.TabRemoved(this, e);
		}
	}

	private void TabStripItem_TabMoving(object sender, SuperTabStripTabMovingEventArgs e)
	{
		if (this.TabMoving != null)
		{
			this.TabMoving(this, e);
		}
	}

	private void TabStripItem_TabMoved(object sender, SuperTabStripTabMovedEventArgs e)
	{
		if (this.TabMoved != null)
		{
			this.TabMoved(this, e);
		}
	}

	private void TabStripItem_GetTabItemPath(object sender, SuperTabGetTabItemPathEventArgs e)
	{
		if (this.GetTabItemPath != null)
		{
			this.GetTabItemPath(this, e);
		}
	}

	private void TabStripItem_GetTabItemContentRectangle(object sender, SuperTabGetTabItemContentRectangleEventArgs e)
	{
		if (this.GetTabItemContentRectangle != null)
		{
			this.GetTabItemContentRectangle(this, e);
		}
	}

	private void TabStripItem_BeforeTabDisplay(object sender, SuperTabStripBeforeTabDisplayEventArgs e)
	{
		if (this.BeforeTabDisplay != null)
		{
			this.BeforeTabDisplay(this, e);
		}
	}

	private void TabStripItem_MeasureTabItem(object sender, SuperTabMeasureTabItemEventArgs e)
	{
		if (this.MeasureTabItem != null)
		{
			this.MeasureTabItem(this, e);
		}
	}

	private void TabStripItem_PreRenderTabItem(object sender, SuperTabPreRenderTabItemEventArgs e)
	{
		if (this.PreRenderTabItem != null)
		{
			this.PreRenderTabItem(this, e);
		}
	}

	private void TabStripItem_PostRenderTabItem(object sender, SuperTabPostRenderTabItemEventArgs e)
	{
		if (this.PostRenderTabItem != null)
		{
			this.PostRenderTabItem(this, e);
		}
	}

	private void TabStripItem_GetTabCloseBounds(object sender, SuperTabGetTabCloseBoundsEventArgs e)
	{
		if (this.GetTabCloseBounds != null)
		{
			this.GetTabCloseBounds(this, e);
		}
	}

	private void TabStripItem_GetTabImageBounds(object sender, SuperTabGetTabImageBoundsEventArgs e)
	{
		if (this.GetTabImageBounds != null)
		{
			this.GetTabImageBounds(this, e);
		}
	}

	private void TabStripItem_GetTabTextBounds(object sender, SuperTabGetTabTextBoundsEventArgs e)
	{
		if (this.GetTabTextBounds != null)
		{
			this.GetTabTextBounds(this, e);
		}
	}

	private void TabStripItem_TabStripPaintBackground(object sender, SuperTabStripPaintBackgroundEventArgs e)
	{
		if (this.TabStripPaintBackground != null)
		{
			this.TabStripPaintBackground(this, e);
		}
	}

	private void TabStripItem_SelectedTabChanging(object sender, SuperTabStripSelectedTabChangingEventArgs e)
	{
		if (this.SelectedTabChanging != null)
		{
			this.SelectedTabChanging(this, e);
		}
	}

	private void TabStripItem_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
	{
		if (this.SelectedTabChanged != null)
		{
			this.SelectedTabChanged(this, e);
		}
	}

	private void TabStripItem_TabStripTabColorChanged(object sender, SuperTabStripTabColorChangedEventArgs e)
	{
		if (this.TabStripTabColorChanged != null)
		{
			this.TabStripTabColorChanged(this, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			((Control)this).Invalidate(true);
		}
		TabDisplay.SetDefaultColorTable();
	}

	public override void RecalcLayout()
	{
		base.RecalcLayout();
		if (!_IsInitializing && _TabStripItem != null)
		{
			_TabStripItem.RecalcSize();
		}
	}

	public void EnsureVisible(SuperTabItem tab)
	{
		_TabStripItem.EnsureVisible(tab);
	}

	public SuperTabItem GetTabFromPoint(Point pt)
	{
		return _TabStripItem.GetTabFromPoint(pt);
	}

	public BaseItem GetItemFromPoint(Point pt)
	{
		return _TabStripItem.GetItemFromPoint(pt);
	}

	public void CloseTab(SuperTabItem tab)
	{
		_TabStripItem.CloseTab(tab);
	}

	public bool SelectPreviousTab()
	{
		return _TabStripItem.SelectPreviousTab();
	}

	public bool SelectNextTab()
	{
		return _TabStripItem.SelectNextTab();
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		if (_TabStripItem != null)
		{
			_TabStripItem.VisibleTab = null;
		}
		RecalcLayout();
	}

	protected override bool ProcessItemMnemonicKey(BaseItem item)
	{
		if (item != null && item.Visible && item.GetEnabled() && item is SuperTabItem)
		{
			SuperTabItem superTabItem = (SuperTabItem)item;
			if (((Control)this).Parent is SuperTabControl superTabControl && !((Control)superTabControl).ContainsFocus)
			{
				return false;
			}
			if (!superTabItem.IsSelected)
			{
				_TabStripItem.SelectTab((SuperTabItem)item, eEventSource.Keyboard);
				return true;
			}
			return false;
		}
		return base.ProcessItemMnemonicKey(item);
	}

	void ISupportInitialize.BeginInit()
	{
		_IsInitializing = true;
	}

	void ISupportInitialize.EndInit()
	{
		_IsInitializing = false;
	}

	public override bool ProcessMnemonicEx(char charCode)
	{
		bool flag = base.ProcessMnemonicEx(charCode);
		if (flag)
		{
			if (ShowKeyTips)
			{
				ShowKeyTips = false;
			}
			if (ApplicationButton is ApplicationButton)
			{
				((ApplicationButton)ApplicationButton).BackstageMnemonicProcessed(charCode);
			}
			else if (ApplicationButton is MetroAppButton)
			{
				((MetroAppButton)ApplicationButton).BackstageMnemonicProcessed(charCode);
			}
		}
		return flag;
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		if (ApplicationButton != null)
		{
			Size keyTipsPadding = KeyTipsPainter.KeyTipsPadding;
			Size size = TextDrawing.MeasureString(g, keyTip, font);
			size.Width += keyTipsPadding.Width;
			size.Height += keyTipsPadding.Height;
			Rectangle displayRectangle = item.DisplayRectangle;
			return (!(item is SuperTabItem)) ? new Rectangle(displayRectangle.X + 24, displayRectangle.Y - 2, size.Width, size.Height) : new Rectangle(displayRectangle.X + 2, displayRectangle.Y, size.Width, size.Height);
		}
		return base.GetKeyTipRectangle(g, item, font, keyTip);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !((Control)this).IsDisposed)
		{
			HookEvents(hook: false);
		}
		base.Dispose(disposing);
	}
}
