using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabStripItem : BaseItem, IDesignTimeProvider
{
	private eSuperTabStyle _TabStyle;

	private eSuperTabLayoutType _TabLayoutType;

	private Image _TabCloseButtonNormal;

	private Image _TabCloseButtonHot;

	private Image _TabCloseButtonPressed;

	private Size _TabCloseButtonSize = new Size(16, 16);

	private Size _DefaultTabCloseButtonSize = new Size(16, 16);

	private int _TabHorizontalSpacing = 5;

	private int _TabVerticalSpacing = 4;

	private bool _CloseButtonOnTabsVisible;

	private bool _CloseButtonOnTabsAlwaysDisplayed = true;

	private bool _DisplaySelectedTextOnly;

	private bool _ShowFocusRectangle;

	private Size _FixedTabSize = Size.Empty;

	private bool _HorizontalText = true;

	private bool _AutoCloseTabs = true;

	private bool _AutoSelectAttachedControl;

	private bool _ReorderTabsEnabled = true;

	private bool _RotateVerticalText;

	private Font _TabFont;

	private Font _SelectedTabFont;

	private SuperTabColorTable _TabStripColor = new SuperTabColorTable();

	private SuperTabItem _HotTab;

	private SuperTabItem _SelectedTab;

	private BaseItem _VisibleTab;

	private int _SelectedTabIndex;

	private SuperTabControlBox _TabControlBox;

	private SuperTabStripBaseDisplay _TabDisplay;

	private Rectangle _TabItemsBounds;

	private SuperTabItem _DesignTimeSelection;

	private eTabStripAlignment _TabAlignment = eTabStripAlignment.Top;

	private eTabCloseButtonPosition _CloseButtonPosition = eTabCloseButtonPosition.Right;

	private SuperTabStrip _TabStrip;

	private int _TabLines;

	private eItemAlignment _TextAlignment;

	private eSuperTabArea _TabArea;

	private SuperTabItem _MouseDownTab;

	private Point _MouseDownLocation;

	private bool _IsTabDragging;

	private SuperTabDragWindow _TabDragWindow;

	private BaseItem _InsertTab;

	private bool _InsertBefore;

	private bool _RecalcInProgress;

	private Padding _ItemPadding;

	private ImageAlignment _ImageAlignment = ImageAlignment.NotSet;

	public bool AutoCloseTabs
	{
		get
		{
			return _AutoCloseTabs;
		}
		set
		{
			_AutoCloseTabs = value;
		}
	}

	public bool AutoSelectAttachedControl
	{
		get
		{
			return _AutoSelectAttachedControl;
		}
		set
		{
			_AutoSelectAttachedControl = value;
		}
	}

	public Image TabCloseButtonNormal
	{
		get
		{
			return _TabCloseButtonNormal;
		}
		set
		{
			_TabCloseButtonNormal = value;
			_TabCloseButtonSize = ((_TabCloseButtonNormal != null) ? _TabCloseButtonNormal.Size : _DefaultTabCloseButtonSize);
			MyRefresh();
		}
	}

	public Image TabCloseButtonHot
	{
		get
		{
			return _TabCloseButtonHot;
		}
		set
		{
			_TabCloseButtonHot = value;
			MyRefresh();
		}
	}

	public Image TabCloseButtonPressed
	{
		get
		{
			return _TabCloseButtonPressed;
		}
		set
		{
			_TabCloseButtonPressed = value;
			MyRefresh();
		}
	}

	public int TabHorizontalSpacing
	{
		get
		{
			return _TabHorizontalSpacing;
		}
		set
		{
			if (_TabHorizontalSpacing != value)
			{
				_TabHorizontalSpacing = value;
				MyRefresh();
			}
		}
	}

	public int TabVerticalSpacing
	{
		get
		{
			return _TabVerticalSpacing;
		}
		set
		{
			if (_TabVerticalSpacing != value)
			{
				_TabVerticalSpacing = value;
				MyRefresh();
			}
		}
	}

	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return _CloseButtonOnTabsVisible;
		}
		set
		{
			if (_CloseButtonOnTabsVisible != value)
			{
				_CloseButtonOnTabsVisible = value;
				MyRefresh();
			}
		}
	}

	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return _CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			if (_CloseButtonOnTabsAlwaysDisplayed != value)
			{
				_CloseButtonOnTabsAlwaysDisplayed = value;
				MyRefresh();
			}
		}
	}

	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return _CloseButtonPosition;
		}
		set
		{
			_CloseButtonPosition = value;
			MyRefresh();
		}
	}

	public SuperTabControlBox ControlBox => _TabControlBox;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SuperTabItem DesignTimeSelection
	{
		get
		{
			return _DesignTimeSelection;
		}
		set
		{
			_DesignTimeSelection = value;
			Refresh();
		}
	}

	public bool DisplaySelectedTextOnly
	{
		get
		{
			return _DisplaySelectedTextOnly;
		}
		set
		{
			_DisplaySelectedTextOnly = value;
			MyRefresh();
		}
	}

	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
			for (int i = 0; i < SubItems.Count; i++)
			{
				SubItems[i].Expanded = value;
			}
			if (ControlBox.Visible)
			{
				ControlBox.Expanded = value;
				for (int j = 0; j < ControlBox.SubItems.Count; j++)
				{
					ControlBox.SubItems[j].Expanded = value;
				}
			}
		}
	}

	public Font TabFont
	{
		get
		{
			return _TabFont;
		}
		set
		{
			_TabFont = value;
			MyRefresh();
		}
	}

	public Font SelectedTabFont
	{
		get
		{
			return _SelectedTabFont;
		}
		set
		{
			_SelectedTabFont = value;
			MyRefresh();
		}
	}

	public bool HorizontalText
	{
		get
		{
			return _HorizontalText;
		}
		set
		{
			_HorizontalText = value;
			MyRefresh();
		}
	}

	[DefaultValue(ImageAlignment.NotSet)]
	[Category("Appearance")]
	[Description("Indicates the default alignment of the Image within the tab.")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return _ImageAlignment;
		}
		set
		{
			if (_ImageAlignment != value)
			{
				_ImageAlignment = value;
				MyRefresh();
			}
		}
	}

	public Padding ItemPadding
	{
		get
		{
			return _ItemPadding;
		}
		set
		{
			_ItemPadding = value;
		}
	}

	public bool RotateVerticalText
	{
		get
		{
			return _RotateVerticalText;
		}
		set
		{
			_RotateVerticalText = value;
			MyRefresh();
		}
	}

	public bool ShowFocusRectangle
	{
		get
		{
			return _ShowFocusRectangle;
		}
		set
		{
			_ShowFocusRectangle = value;
		}
	}

	public Size FixedTabSize
	{
		get
		{
			return _FixedTabSize;
		}
		set
		{
			value.Width = Math.Max(0, value.Width);
			value.Height = Math.Max(0, value.Height);
			if ((value.Width > 0 || value.Height > 0) && _TabDisplay != null)
			{
				Size minTabSize = _TabDisplay.MinTabSize;
				if (value.Width > 0 && minTabSize.Width > value.Width)
				{
					value.Width = minTabSize.Width;
				}
				if (value.Height > 0 && minTabSize.Height > value.Height)
				{
					value.Height = minTabSize.Height;
				}
			}
			if (_FixedTabSize != value)
			{
				_FixedTabSize = value;
				MyRefresh();
			}
		}
	}

	public bool IsTabDragging
	{
		get
		{
			return _IsTabDragging;
		}
		internal set
		{
			_IsTabDragging = value;
		}
	}

	public int MinTabStripHeight => _TabDisplay.MinTabStripHeight;

	public bool ReorderTabsEnabled
	{
		get
		{
			return _ReorderTabsEnabled;
		}
		set
		{
			_ReorderTabsEnabled = value;
		}
	}

	public SuperTabItem SelectedTab
	{
		get
		{
			return _SelectedTab;
		}
		set
		{
			if (value != null)
			{
				if (value.Visible)
				{
					SelectTab(value, eEventSource.Code);
				}
			}
			else
			{
				SelectTab(null, eEventSource.Code);
			}
		}
	}

	public int SelectedTabIndex
	{
		get
		{
			if (_SelectedTab != null)
			{
				return SubItems.IndexOf(_SelectedTab);
			}
			return -1;
		}
		set
		{
			if (value < 0 || value >= SubItems.Count)
			{
				return;
			}
			if (SubItems[value] is SuperTabItem superTabItem)
			{
				if (superTabItem.Visible)
				{
					SelectedTab = superTabItem;
				}
			}
			else
			{
				SelectedTab = null;
			}
		}
	}

	public eTabStripAlignment TabAlignment
	{
		get
		{
			return _TabAlignment;
		}
		set
		{
			if (_TabAlignment != value)
			{
				_TabAlignment = value;
				SyncOrientation();
				MyRefresh();
			}
		}
	}

	public eSuperTabLayoutType TabLayoutType
	{
		get
		{
			return _TabLayoutType;
		}
		set
		{
			_TabLayoutType = value;
			_TabControlBox.Bounds = Rectangle.Empty;
			OnTabStyleChanged();
			MyRefresh();
		}
	}

	public SuperTabColorTable TabStripColor
	{
		get
		{
			return _TabStripColor;
		}
		set
		{
			if (!_TabStripColor.Equals(value))
			{
				if (_TabStripColor != null)
				{
					_TabStripColor.ColorTableChanged -= ColorTableColorTableChanged;
				}
				_TabStripColor = value;
				if (value != null)
				{
					_TabStripColor.ColorTableChanged += ColorTableColorTableChanged;
				}
				OnTabStripColorChanged();
				MyRefresh();
			}
		}
	}

	public eSuperTabStyle TabStyle
	{
		get
		{
			return _TabStyle;
		}
		set
		{
			if (_TabStyle != value)
			{
				_TabStyle = value;
				switch (value)
				{
				case eSuperTabStyle.Office2007:
					_TabDisplay = new Office2007SuperTabStrip(this);
					break;
				case eSuperTabStyle.Office2010BackstageBlue:
					_TabDisplay = new Office2010BackstageSuperTabStrip(this);
					break;
				case eSuperTabStyle.OneNote2007:
					_TabDisplay = new OneNote2007SuperTabStrip(this);
					break;
				case eSuperTabStyle.VisualStudio2008Dock:
					_TabDisplay = new VS2008DockSuperTabStrip(this);
					break;
				case eSuperTabStyle.VisualStudio2008Document:
					_TabDisplay = new VS2008DocumentSuperTabStrip(this);
					break;
				case eSuperTabStyle.WinMediaPlayer12:
					_TabDisplay = new WinMediaPlayer12SuperTabStrip(this);
					break;
				}
				ApplyTabStyle();
			}
		}
	}

	public eItemAlignment TextAlignment
	{
		get
		{
			return _TextAlignment;
		}
		set
		{
			if (_TextAlignment != value)
			{
				_TextAlignment = value;
				Refresh();
			}
		}
	}

	internal SuperTabItem HotTab => _HotTab;

	internal BaseItem InsertTab => _InsertTab;

	internal bool InsertBefore => _InsertBefore;

	internal SuperTabItem MouseOverTab => _HotTab;

	internal bool IsVertical
	{
		get
		{
			if (_TabAlignment != 0)
			{
				return _TabAlignment == eTabStripAlignment.Right;
			}
			return true;
		}
	}

	internal Size TabCloseButtonSize => _TabCloseButtonSize;

	internal SuperTabStripBaseDisplay TabDisplay => _TabDisplay;

	internal Rectangle TabItemsBounds
	{
		get
		{
			return _TabItemsBounds;
		}
		set
		{
			_TabItemsBounds = value;
		}
	}

	internal int TabLines
	{
		get
		{
			return _TabLines;
		}
		set
		{
			_TabLines = value;
		}
	}

	internal SuperTabStrip TabStrip => _TabStrip;

	internal int VisibleTabCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < SubItems.Count; i++)
			{
				if (SubItems[i].Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	internal BaseItem VisibleTab
	{
		get
		{
			return _VisibleTab ?? _SelectedTab;
		}
		set
		{
			if (_VisibleTab != value)
			{
				_VisibleTab = value;
				MyRefresh();
			}
		}
	}

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

	[Description("Occurs when the TabStrip TabColor has changed.")]
	public event EventHandler<EventArgs> TabStripColorChanged;

	public SuperTabStripItem(SuperTabStrip tabStrip)
	{
		_TabStrip = tabStrip;
		_TabControlBox = new SuperTabControlBox(this);
		_TabDisplay = new Office2007SuperTabStrip(this);
		_ItemPadding = new Padding(0, 0, 0, 0);
		SetIsContainer(b: true);
		HookEvents(hook: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref _TabCloseButtonNormal);
			BarUtilities.DisposeImage(ref _TabCloseButtonHot);
			BarUtilities.DisposeImage(ref _TabCloseButtonPressed);
		}
		if (disposing)
		{
			_TabControlBox.Dispose();
		}
		base.Dispose(disposing);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabFont()
	{
		return _TabFont != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		TabFont = null;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedTabFont()
	{
		return _SelectedTabFont != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		SelectedTabFont = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return !_FixedTabSize.IsEmpty;
	}

	private void SyncOrientation()
	{
		eDesignMarkerOrientation tabAlignmentBasedOrientation = GetTabAlignmentBasedOrientation();
		foreach (BaseItem subItem in SubItems)
		{
			subItem.DesignMarkerOrientation = tabAlignmentBasedOrientation;
		}
	}

	private eDesignMarkerOrientation GetTabAlignmentBasedOrientation()
	{
		if (!IsVertical)
		{
			return eDesignMarkerOrientation.Horizontal;
		}
		return eDesignMarkerOrientation.Vertical;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		item.DesignMarkerOrientation = GetTabAlignmentBasedOrientation();
		item.Style = GetStyleFromTabStyle();
		base.OnItemAdded(item);
	}

	private void ApplyTabStyle()
	{
		eDotNetBarStyle styleFromTabStyle = GetStyleFromTabStyle();
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SuperTabItem superTabItem)
			{
				superTabItem.TabStyle = _TabStyle;
			}
			subItem.Style = styleFromTabStyle;
		}
		TabStrip.RecalcLayout();
	}

	private eDotNetBarStyle GetStyleFromTabStyle()
	{
		eDotNetBarStyle result = eDotNetBarStyle.StyleManagerControlled;
		eSuperTabStyle tabStyle = _TabStyle;
		if (tabStyle == eSuperTabStyle.VisualStudio2008Dock || tabStyle == eSuperTabStyle.VisualStudio2008Document)
		{
			result = eDotNetBarStyle.VS2005;
		}
		return result;
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			base.SubItemsChanged += SuperTabStripItemSubItemsChanged;
		}
		else
		{
			base.SubItemsChanged -= SuperTabStripItemSubItemsChanged;
		}
	}

	private void SuperTabStripItemSubItemsChanged(object sender, CollectionChangeEventArgs e)
	{
		switch (e.Action)
		{
		case CollectionChangeAction.Add:
			TabItemAdded(e.Element as BaseItem);
			break;
		case CollectionChangeAction.Remove:
			TabItemRemoved(e.Element as BaseItem);
			break;
		case CollectionChangeAction.Refresh:
			MyRefresh();
			break;
		}
	}

	internal void TabItemAdded(BaseItem item)
	{
		if (item is SuperTabItem superTabItem)
		{
			superTabItem.TabStripItem = this;
			superTabItem.Visible = true;
			superTabItem.TabColorChanged += TabTabColorChanged;
			if (this.TabItemOpen != null)
			{
				SuperTabStripTabItemOpenEventArgs e = new SuperTabStripTabItemOpenEventArgs(superTabItem);
				this.TabItemOpen(this, e);
			}
			if (VisibleTabCount == 1 && SelectedTab == null)
			{
				SelectedTab = superTabItem;
			}
		}
		MyRefresh();
	}

	internal void TabItemRemoved(BaseItem item)
	{
		if (this.TabRemoved != null)
		{
			SuperTabStripTabRemovedEventArgs e = new SuperTabStripTabRemovedEventArgs(item);
			this.TabRemoved(this, e);
		}
		if (item is SuperTabItem superTabItem)
		{
			superTabItem.TabColorChanged -= TabTabColorChanged;
			if (superTabItem.AttachedControl is SuperTabControlPanel superTabControlPanel)
			{
				superTabControlPanel.PanelColorChanged -= TabTabColorChanged;
			}
			if (SelectedTab == superTabItem)
			{
				OnSelectedTabRemoved(_SelectedTabIndex, superTabItem);
			}
			if (VisibleTabCount == 0)
			{
				SelectedTab = null;
			}
		}
		Refresh();
	}

	private void TabTabColorChanged(object sender, EventArgs e)
	{
		if (this.TabStripTabColorChanged != null && sender is SuperTabItem tab)
		{
			this.TabStripTabColorChanged(this, new SuperTabStripTabColorChangedEventArgs(tab));
		}
		Refresh();
	}

	private void ColorTableColorTableChanged(object sender, EventArgs e)
	{
		OnTabStripColorChanged();
	}

	private void MyRefresh()
	{
		NeedRecalcSize = true;
		Refresh();
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout && !_RecalcInProgress)
		{
			try
			{
				_RecalcInProgress = true;
				RecalcDisplaySize();
			}
			finally
			{
				_RecalcInProgress = false;
			}
		}
	}

	private void RecalcDisplaySize()
	{
		base.RecalcSize();
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			if (_TabControlBox.Visible)
			{
				_TabControlBox.RecalcSize();
				if (!DesignMode)
				{
					_TabControlBox.Displayed = true;
				}
			}
			_TabDisplay.RecalcSize(val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public void EnsureVisible(BaseItem item)
	{
		VisibleTab = item;
	}

	public void SelectTab(SuperTabItem tab, eEventSource eventSource)
	{
		if (tab != null && (tab.Enabled || DesignMode))
		{
			bool flag = _VisibleTab != tab || _SelectedTab != tab;
			_VisibleTab = tab;
			if (_SelectedTab != tab)
			{
				SuperTabItem selectedTab = _SelectedTab;
				if (!OnSelectedTabChanging(tab, eventSource))
				{
					OnBeforeTabDisplay(tab);
					Control val = null;
					if (_SelectedTab != null && _SelectedTab.AttachedControl != null)
					{
						val = _SelectedTab.AttachedControl;
					}
					_SelectedTab = tab;
					_SelectedTabIndex = SubItems.IndexOf(tab);
					if (_SelectedTab.AttachedControl != null)
					{
						_SelectedTab.AttachedControl.BringToFront();
						_SelectedTab.AttachedControl.Visible = true;
						if (((Control)_TabStrip).Parent is SuperTabControl superTabControl)
						{
							bool flag2 = !((Control)superTabControl).TabStop || !((Control)superTabControl).Focused || eventSource != eEventSource.Keyboard;
							if (_AutoSelectAttachedControl && flag2)
							{
								_SelectedTab.AttachedControl.Select();
							}
						}
						if (val == _SelectedTab.AttachedControl)
						{
							val = null;
						}
					}
					if (val != null)
					{
						val.Visible = false;
					}
					OnSelectedTabChanged(selectedTab, _SelectedTab, eventSource);
				}
			}
			if (flag)
			{
				MyRefresh();
			}
		}
		else
		{
			_SelectedTab = null;
			MyRefresh();
		}
	}

	public bool SelectPreviousTab()
	{
		return SelectPreviousTab(eEventSource.Code);
	}

	private bool SelectPreviousTab(eEventSource eventSource)
	{
		if (SelectedTab != null && SubItems.IndexOf(SelectedTab) > 0)
		{
			for (int num = SubItems.IndexOf(SelectedTab) - 1; num >= 0; num--)
			{
				BaseItem baseItem = SubItems[num];
				if (baseItem.Visible && (baseItem.Enabled || DesignMode))
				{
					SelectTab(SubItems[num] as SuperTabItem, eventSource);
					return true;
				}
			}
		}
		return false;
	}

	public bool SelectNextTab()
	{
		return SelectNextTab(eEventSource.Code);
	}

	private bool SelectNextTab(eEventSource eventSource)
	{
		if (SelectedTab != null && SubItems.IndexOf(SelectedTab) < SubItems.Count)
		{
			for (int i = SubItems.IndexOf(SelectedTab) + 1; i < SubItems.Count; i++)
			{
				BaseItem baseItem = SubItems[i];
				if (baseItem.Visible && (baseItem.Enabled || DesignMode))
				{
					SelectTab(baseItem as SuperTabItem, eventSource);
					return true;
				}
			}
		}
		return false;
	}

	internal void SelectNewTab(BaseItem item)
	{
		SelectNewTab(Math.Max(0, SubItems.IndexOf(item)));
	}

	internal void SelectNewTab(int index)
	{
		SubItemsCollection subItems = SubItems;
		int num = GetNextSelectableTab(subItems, index);
		if (num < 0)
		{
			num = GetPrevSelectableTab(subItems, index);
		}
		if (num >= 0)
		{
			SelectTab(subItems[num] as SuperTabItem, eEventSource.Code);
			RecalcSize();
		}
		else
		{
			SelectedTab = null;
		}
	}

	internal SuperTabItem GetNewSelectTab(int index)
	{
		SubItemsCollection subItems = SubItems;
		if ((uint)index < subItems.Count)
		{
			int num = GetNextSelectableTab(subItems, index);
			if (num < 0)
			{
				num = GetPrevSelectableTab(subItems, index);
			}
			if (num >= 0)
			{
				return subItems[num] as SuperTabItem;
			}
		}
		return null;
	}

	internal int GetNextSelectableTab(SuperTabItem tab)
	{
		SubItemsCollection subItems = SubItems;
		return GetNextSelectableTab(subItems, subItems.IndexOf(tab));
	}

	internal int GetNextSelectableTab(SubItemsCollection items, int n)
	{
		n = Math.Max(n, 0);
		for (int i = n; i < items.Count; i++)
		{
			if (items[i] is SuperTabItem && items[i].Visible && items[i].Enabled)
			{
				return i;
			}
		}
		return -1;
	}

	internal int GetPrevSelectableTab(SuperTabItem tab)
	{
		SubItemsCollection subItems = SubItems;
		return GetPrevSelectableTab(subItems, subItems.IndexOf(tab));
	}

	internal int GetPrevSelectableTab(SubItemsCollection items, int n)
	{
		n = Math.Min(n, items.Count - 1);
		for (int num = n; num >= 0; num--)
		{
			if (items[num] is SuperTabItem && items[num].Visible && items[num].Enabled)
			{
				return num;
			}
		}
		return -1;
	}

	internal void UpdateSelectedTab()
	{
		if (!DesignMode)
		{
			SelectNewTab(SelectedTab);
		}
		RecalcSize();
	}

	private void OnTabStyleChanged()
	{
		MyRefresh();
		EnsureVisible(SelectedTab);
	}

	internal Size OnMeasureTabItem(SuperTabItem tab, Size size, Graphics g)
	{
		if (this.MeasureTabItem != null)
		{
			SuperTabMeasureTabItemEventArgs superTabMeasureTabItemEventArgs = new SuperTabMeasureTabItemEventArgs(tab, size, g);
			this.MeasureTabItem(this, superTabMeasureTabItemEventArgs);
			return superTabMeasureTabItemEventArgs.Size;
		}
		return size;
	}

	internal bool OnPreRenderTabItem(SuperTabItem tab, Graphics g)
	{
		if (this.PreRenderTabItem != null)
		{
			SuperTabPreRenderTabItemEventArgs superTabPreRenderTabItemEventArgs = new SuperTabPreRenderTabItemEventArgs(tab, g);
			this.PreRenderTabItem(this, superTabPreRenderTabItemEventArgs);
			return superTabPreRenderTabItemEventArgs.Cancel;
		}
		return false;
	}

	internal void OnPostRenderTabItem(SuperTabItem tab, Graphics g)
	{
		if (this.PostRenderTabItem != null)
		{
			SuperTabPostRenderTabItemEventArgs e = new SuperTabPostRenderTabItemEventArgs(tab, g);
			this.PostRenderTabItem(this, e);
		}
	}

	internal Rectangle OnGetTabItemContentRectangle(SuperTabItem tab)
	{
		if (this.GetTabItemContentRectangle != null)
		{
			SuperTabGetTabItemContentRectangleEventArgs superTabGetTabItemContentRectangleEventArgs = new SuperTabGetTabItemContentRectangleEventArgs(tab);
			this.GetTabItemContentRectangle(this, superTabGetTabItemContentRectangleEventArgs);
			return superTabGetTabItemContentRectangleEventArgs.ContentRectangle;
		}
		return tab.TabItemDisplay.ContentRectangle();
	}

	internal GraphicsPath OnGetTabItemPath(SuperTabItem tab)
	{
		if (this.GetTabItemPath != null)
		{
			SuperTabGetTabItemPathEventArgs superTabGetTabItemPathEventArgs = new SuperTabGetTabItemPathEventArgs(tab);
			this.GetTabItemPath(this, superTabGetTabItemPathEventArgs);
			return superTabGetTabItemPathEventArgs.Path;
		}
		return null;
	}

	internal void OnSelectedTabRemoved(int index, SuperTabItem item)
	{
		SelectNewTab(index);
	}

	private bool OnSelectedTabChanging(SuperTabItem value, eEventSource eventSource)
	{
		if (this.SelectedTabChanging != null)
		{
			SuperTabStripSelectedTabChangingEventArgs superTabStripSelectedTabChangingEventArgs = new SuperTabStripSelectedTabChangingEventArgs(_SelectedTab, value, eventSource);
			this.SelectedTabChanging(this, superTabStripSelectedTabChangingEventArgs);
			return superTabStripSelectedTabChangingEventArgs.Cancel;
		}
		return false;
	}

	private void OnSelectedTabChanged(SuperTabItem oldTab, SuperTabItem newTab, eEventSource eventSource)
	{
		if (this.SelectedTabChanged != null)
		{
			SuperTabStripSelectedTabChangedEventArgs e = new SuperTabStripSelectedTabChangedEventArgs(oldTab, newTab, eventSource);
			this.SelectedTabChanged(this, e);
		}
	}

	internal void OnBeforeTabDisplay(SuperTabItem item)
	{
		if (this.BeforeTabDisplay != null)
		{
			SuperTabStripBeforeTabDisplayEventArgs e = new SuperTabStripBeforeTabDisplayEventArgs(item);
			this.BeforeTabDisplay(this, e);
		}
	}

	private bool OnTabItemClose(SuperTabItem tab)
	{
		if (this.TabItemClose != null)
		{
			SuperTabStripTabItemCloseEventArgs superTabStripTabItemCloseEventArgs = new SuperTabStripTabItemCloseEventArgs(tab);
			superTabStripTabItemCloseEventArgs.Cancel = !AutoCloseTabs;
			this.TabItemClose(this, superTabStripTabItemCloseEventArgs);
			return superTabStripTabItemCloseEventArgs.Cancel;
		}
		return !AutoCloseTabs;
	}

	private void OnTabStripColorChanged()
	{
		if (this.TabStripColorChanged != null)
		{
			this.TabStripColorChanged(this, EventArgs.Empty);
		}
		MyRefresh();
	}

	internal void OnPaintBackground(Graphics g, SuperTabColorTable ct)
	{
		if (this.TabStripPaintBackground != null)
		{
			this.TabStripPaintBackground(this, new SuperTabStripPaintBackgroundEventArgs(g, ct));
		}
	}

	internal Rectangle OnGetTextBounds(SuperTabItem tab, Rectangle bounds)
	{
		if (this.GetTabTextBounds != null)
		{
			SuperTabGetTabTextBoundsEventArgs superTabGetTabTextBoundsEventArgs = new SuperTabGetTabTextBoundsEventArgs(tab, bounds);
			this.GetTabTextBounds(this, superTabGetTabTextBoundsEventArgs);
			bounds = superTabGetTabTextBoundsEventArgs.Bounds;
		}
		return bounds;
	}

	internal Rectangle OnGetImageBounds(SuperTabItem tab, Rectangle bounds)
	{
		if (this.GetTabImageBounds != null)
		{
			SuperTabGetTabImageBoundsEventArgs superTabGetTabImageBoundsEventArgs = new SuperTabGetTabImageBoundsEventArgs(tab, bounds);
			this.GetTabImageBounds(this, superTabGetTabImageBoundsEventArgs);
			bounds = superTabGetTabImageBoundsEventArgs.Bounds;
		}
		return bounds;
	}

	internal Rectangle OnGetCloseBounds(SuperTabItem tab, Rectangle bounds)
	{
		if (this.GetTabCloseBounds != null)
		{
			SuperTabGetTabCloseBoundsEventArgs superTabGetTabCloseBoundsEventArgs = new SuperTabGetTabCloseBoundsEventArgs(tab, bounds);
			this.GetTabCloseBounds(this, superTabGetTabCloseBoundsEventArgs);
			bounds = superTabGetTabCloseBoundsEventArgs.Bounds;
		}
		return bounds;
	}

	protected bool OnTabMoving(BaseItem moveTab, BaseItem insertTab, bool insertBefore)
	{
		if (this.TabMoving != null)
		{
			SuperTabStripTabMovingEventArgs superTabStripTabMovingEventArgs = new SuperTabStripTabMovingEventArgs(moveTab, insertTab, insertBefore);
			this.TabMoving(this, superTabStripTabMovingEventArgs);
			return superTabStripTabMovingEventArgs.CanMove;
		}
		return true;
	}

	protected bool OnTabMoved(SuperTabItem tab, List<BaseItem> oldOrder, ref List<BaseItem> newOrder)
	{
		if (this.TabMoved != null)
		{
			SuperTabStripTabMovedEventArgs superTabStripTabMovedEventArgs = new SuperTabStripTabMovedEventArgs(tab, oldOrder, newOrder);
			this.TabMoved(this, superTabStripTabMovedEventArgs);
			newOrder = superTabStripTabMovedEventArgs.NewOrder;
			return superTabStripTabMovedEventArgs.Cancel;
		}
		return false;
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		Point pt = new Point(x, y);
		return GetTabFromPoint(pt) ?? GetItemFromPoint(pt);
	}

	public override void InternalClick(MouseButtons mb, Point pt)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		base.InternalClick(mb, pt);
		GetTabFromPoint(pt)?.InternalClick(mb, pt);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		base.InternalMouseDown(objArg);
		InternalOnMouseDown(objArg);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseDown(MouseEventArgs objArg)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		HideToolTip();
		_TabArea = eSuperTabArea.InNone;
		SuperTabItem superTabItem = (_MouseDownTab = GetTabFromPoint(objArg.Location));
		_MouseDownLocation = objArg.Location;
		if (superTabItem != null)
		{
			if ((int)objArg.Button != 1048576)
			{
				return;
			}
			if (superTabItem.Enabled || DesignMode)
			{
				_TabArea = superTabItem.GetTabAreaFromPoint(objArg.Location);
				if ((_TabArea == eSuperTabArea.InContent || _TabArea == eSuperTabArea.InImage) && _SelectedTab != superTabItem)
				{
					SelectedTab = superTabItem;
				}
			}
			else
			{
				_MouseDownTab = null;
				SystemSounds.Beep.Play();
			}
		}
		else if (ControlBox.Visible && ControlBox.IsMouseOver)
		{
			ControlBox.InternalMouseDown(objArg);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		InternalOnMouseUp(objArg);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseUp(MouseEventArgs objArg)
	{
		if (!_IsTabDragging)
		{
			ProcessMouseUp(objArg);
		}
		else
		{
			ProcessTabUp();
		}
		_InsertTab = null;
		_MouseDownTab = null;
		_IsTabDragging = false;
	}

	private void ProcessMouseUp(MouseEventArgs objArg)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		SuperTabItem tabFromPoint = GetTabFromPoint(objArg.Location);
		if (tabFromPoint != null)
		{
			if ((int)objArg.Button == 1048576)
			{
				eSuperTabArea tabAreaFromPoint = tabFromPoint.GetTabAreaFromPoint(objArg.Location);
				if (_TabArea == eSuperTabArea.InCloseBox && tabAreaFromPoint == eSuperTabArea.InCloseBox)
				{
					CloseTab(tabFromPoint);
				}
			}
		}
		else if (ControlBox.Visible && ControlBox.IsMouseDown)
		{
			ControlBox.InternalMouseUp(objArg);
		}
	}

	private void ProcessTabUp()
	{
		((Control)((Form)_TabDragWindow).Owner).Cursor = Cursors.Default;
		((Component)(object)_TabDragWindow).Dispose();
		_TabDragWindow = null;
		if (_InsertTab != null)
		{
			SuperTabItem hotTab = _HotTab;
			List<BaseItem> oldOrder = GetOldOrder();
			List<BaseItem> newOrder = GetNewOrder();
			if (!OnTabMoved(hotTab, oldOrder, ref newOrder))
			{
				SubItems.Clear();
				foreach (BaseItem item in newOrder)
				{
					SubItems.Add(item);
				}
				SelectedTab = hotTab;
			}
		}
		Refresh();
	}

	private List<BaseItem> GetOldOrder()
	{
		List<BaseItem> list = new List<BaseItem>(SubItems.Count);
		foreach (BaseItem subItem in SubItems)
		{
			list.Add(subItem);
		}
		return list;
	}

	private List<BaseItem> GetNewOrder()
	{
		List<BaseItem> oldOrder = GetOldOrder();
		int num = oldOrder.IndexOf(_InsertTab);
		int num2 = oldOrder.IndexOf(_HotTab);
		if (num >= 0 && num2 >= 0 && ((!_InsertBefore && num + 1 != num2) || (_InsertBefore && num - 1 != num2)))
		{
			oldOrder.Remove(_HotTab);
			num = oldOrder.IndexOf(_InsertTab);
			if (!_InsertBefore)
			{
				num++;
			}
			oldOrder.Insert(num, _HotTab);
		}
		return oldOrder;
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (!_IsTabDragging)
		{
			base.InternalMouseMove(objArg);
		}
		InternalOnMouseMove(objArg);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseMove(MouseEventArgs objArg)
	{
		SyncMouseState();
		if (!_IsTabDragging)
		{
			ProcessMouseMove(objArg);
		}
		else
		{
			ProcessTabMove(objArg);
		}
	}

	private void SyncMouseState()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (_MouseDownTab != null && (int)Control.MouseButtons == 0)
		{
			if (_TabDragWindow != null)
			{
				((Component)(object)_TabDragWindow).Dispose();
				_TabDragWindow = null;
			}
			_InsertTab = null;
			_MouseDownTab = null;
			_IsTabDragging = false;
		}
	}

	private void ProcessMouseMove(MouseEventArgs objArg)
	{
		SuperTabItem tabFromPoint = GetTabFromPoint(objArg.Location);
		if (_HotTab != tabFromPoint || _HotTab != null)
		{
			HotTabMouseMove(objArg, tabFromPoint);
		}
		else
		{
			ControlBoxMouseMove(objArg);
		}
	}

	private void HotTabMouseMove(MouseEventArgs objArg, SuperTabItem tab)
	{
		if (_HotTab != tab)
		{
			if (_HotTab != null)
			{
				_HotTab.Refresh();
			}
			if (_HotTab == null || !_HotTab.CloseButtonPressed)
			{
				_HotTab = tab;
				_ = _HotTab;
			}
			if (_HotTab != null)
			{
				_HotTab.Refresh();
			}
			ResetHover();
			HideToolTip();
		}
		else if (_HotTab != null)
		{
			StartTabMove(objArg);
		}
	}

	private void ControlBoxMouseMove(MouseEventArgs objArg)
	{
		if (!ControlBox.Visible)
		{
			return;
		}
		if (ControlBox.Bounds.Contains(objArg.Location))
		{
			if (!ControlBox.IsMouseOver)
			{
				ControlBox.InternalMouseEnter();
			}
			ControlBox.InternalMouseMove(objArg);
		}
		else if (ControlBox.IsMouseOver)
		{
			ControlBox.InternalMouseLeave();
		}
	}

	private void ProcessTabMove(MouseEventArgs objArg)
	{
		((Form)_TabDragWindow).Location = new Point(Control.MousePosition.X, Control.MousePosition.Y + ((Control)((Form)_TabDragWindow).Owner).Cursor.Size.Height / 2);
		_InsertTab = null;
		BaseItem itemFromPoint = GetItemFromPoint(objArg.Location);
		if (itemFromPoint != null && _HotTab != null)
		{
			Point location = objArg.Location;
			eTabStripAlignment tabAlignment = _TabStrip.TabAlignment;
			if ((uint)(tabAlignment - 2) <= 1u)
			{
				_InsertBefore = location.X < itemFromPoint.Bounds.X + itemFromPoint.Bounds.Width / 2;
			}
			else
			{
				_InsertBefore = location.Y < itemFromPoint.Bounds.Y + itemFromPoint.Bounds.Height / 2;
			}
			int num = SubItems.IndexOf(itemFromPoint);
			int num2 = SubItems.IndexOf(_HotTab);
			if (num2 != num && ((_InsertBefore && num - 1 != num2) || (!_InsertBefore && num + 1 != num2)))
			{
				_InsertTab = itemFromPoint;
			}
		}
		if (!OnTabMoving(_HotTab, _InsertTab, _InsertBefore))
		{
			_InsertTab = null;
		}
		((Control)((Form)_TabDragWindow).Owner).Cursor = ((_InsertTab != null) ? Cursors.Hand : Cursors.No);
		Refresh();
	}

	private bool StartTabMove(MouseEventArgs objArg)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		if (_ReorderTabsEnabled && _HotTab == _MouseDownTab && !_MouseDownTab.CloseButtonPressed && (Math.Abs(_MouseDownLocation.X - objArg.Location.X) > 8 || Math.Abs(_MouseDownLocation.Y - objArg.Location.Y) > 8))
		{
			_IsTabDragging = true;
			_TabDragWindow = new SuperTabDragWindow();
			((Form)_TabDragWindow).Owner = ((Control)TabStrip).FindForm();
			((Control)_TabDragWindow).Paint += new PaintEventHandler(TabDragWindowPaint);
			SetDragWindowRegion();
			((Control)_TabDragWindow).Show();
			((Form)_TabDragWindow).Size = _MouseDownTab.Bounds.Size;
			return true;
		}
		return false;
	}

	private void SetDragWindowRegion()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		Rectangle bounds = _MouseDownTab.Bounds;
		Rectangle bounds2 = bounds;
		bounds2.Location = new Point(0, 0);
		try
		{
			_MouseDownTab.Bounds = bounds2;
			GraphicsPath tabItemPath = _MouseDownTab.GetTabItemPath();
			try
			{
				GraphicsPath val = (GraphicsPath)tabItemPath.Clone();
				Pen val2 = new Pen(Color.Black, 2f);
				try
				{
					val.Widen(val2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				Region val3 = new Region(tabItemPath);
				val3.Union(val);
				((Control)_TabDragWindow).Region = val3;
			}
			finally
			{
				((IDisposable)tabItemPath)?.Dispose();
			}
		}
		finally
		{
			_MouseDownTab.Bounds = bounds;
		}
	}

	private void TabDragWindowPaint(object sender, PaintEventArgs e)
	{
		ItemPaintArgs p = new ItemPaintArgs(null, (Control)(object)_TabStrip, e.Graphics, new ColorScheme());
		Rectangle bounds = _MouseDownTab.Bounds;
		Rectangle bounds2 = bounds;
		bounds2.Location = new Point(0, 0);
		try
		{
			_MouseDownTab.Bounds = bounds2;
			_MouseDownTab.Paint(p);
		}
		finally
		{
			_MouseDownTab.Bounds = bounds;
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		InternalOnMouseLeave();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseLeave()
	{
		HideToolTip();
		if (_HotTab != null)
		{
			if (SubItems.Contains(_HotTab))
			{
				_HotTab.CloseButtonMouseOver = false;
				_HotTab.Refresh();
				_HotTab = null;
				Refresh();
			}
			else
			{
				_HotTab = null;
			}
		}
		else if (ControlBox.Visible && ControlBox.IsMouseOver)
		{
			ControlBox.InternalMouseLeave();
		}
	}

	public SuperTabItem GetTabFromPoint(Point pt)
	{
		if (SelectedTab != null && SelectedTab.PointInTab(pt))
		{
			return SelectedTab;
		}
		for (int i = 0; i < SubItems.Count; i++)
		{
			if (SubItems[i] is SuperTabItem { Visible: not false, Displayed: not false } superTabItem && superTabItem.PointInTab(pt))
			{
				return superTabItem;
			}
		}
		return null;
	}

	public BaseItem GetItemFromPoint(Point pt)
	{
		for (int i = 0; i < SubItems.Count; i++)
		{
			BaseItem baseItem = SubItems[i];
			if (baseItem.Visible && baseItem.Displayed && baseItem.DisplayRectangle.Contains(pt))
			{
				return baseItem;
			}
		}
		return null;
	}

	internal SuperTabItem GetNextVisibleTab(int index)
	{
		if (index < 0 || index >= SubItems.Count)
		{
			index = 0;
		}
		for (int i = index; i < SubItems.Count; i++)
		{
			if (SubItems[i] is SuperTabItem { Visible: not false } superTabItem)
			{
				return superTabItem;
			}
		}
		return null;
	}

	public void CloseTab(SuperTabItem tab)
	{
		if (tab == null || !SubItems.Contains(tab) || OnTabItemClose(tab) || _TabStrip.IsDesignMode)
		{
			return;
		}
		SubItems.Remove(tab);
		Control attachedControl = tab.AttachedControl;
		if (attachedControl != null)
		{
			if (((Control)_TabStrip).Controls.Contains(attachedControl))
			{
				((Control)_TabStrip).Controls.Remove(attachedControl);
			}
			((Component)(object)attachedControl).Dispose();
		}
		_TabStrip.RecalcLayout();
	}

	public override void Paint(ItemPaintArgs p)
	{
		if (_TabDisplay != null)
		{
			_TabDisplay.Paint(p);
		}
	}

	public override void SubItemSizeChanged(BaseItem objChildItem)
	{
		base.SubItemSizeChanged(objChildItem);
		_TabStrip.RecalcLayout();
		if (!DesignMode)
		{
			SelectNewTab(SelectedTab);
		}
	}

	public override BaseItem Copy()
	{
		SuperTabStripItem superTabStripItem = new SuperTabStripItem(_TabStrip);
		CopyToItem(superTabStripItem);
		return superTabStripItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		if (copy is SuperTabStripItem superTabStripItem)
		{
			base.CopyToItem(superTabStripItem);
			superTabStripItem.AutoCloseTabs = _AutoCloseTabs;
			superTabStripItem.TabCloseButtonNormal = _TabCloseButtonNormal;
			superTabStripItem.TabCloseButtonHot = _TabCloseButtonHot;
			superTabStripItem.TabHorizontalSpacing = _TabHorizontalSpacing;
			superTabStripItem.TabVerticalSpacing = _TabVerticalSpacing;
			superTabStripItem.CloseButtonOnTabsVisible = _CloseButtonOnTabsVisible;
			superTabStripItem.CloseButtonOnTabsAlwaysDisplayed = _CloseButtonOnTabsAlwaysDisplayed;
			superTabStripItem.CloseButtonPosition = _CloseButtonPosition;
			superTabStripItem.DisplaySelectedTextOnly = _DisplaySelectedTextOnly;
			superTabStripItem.TabFont = _TabFont;
			superTabStripItem.SelectedTabFont = _SelectedTabFont;
			superTabStripItem.HorizontalText = _HorizontalText;
			superTabStripItem.ShowFocusRectangle = _ShowFocusRectangle;
			superTabStripItem.FixedTabSize = _FixedTabSize;
			superTabStripItem.ReorderTabsEnabled = _ReorderTabsEnabled;
			superTabStripItem.TabAlignment = _TabAlignment;
			superTabStripItem.TabLayoutType = _TabLayoutType;
			superTabStripItem.TabStripColor = _TabStripColor;
			superTabStripItem.TabStyle = _TabStyle;
			superTabStripItem.TextAlignment = _TextAlignment;
		}
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		InsertPosition insertPosition = DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
		if (insertPosition != null && insertPosition.TargetProvider != this && dragItem is SuperTabItem)
		{
			return null;
		}
		return insertPosition;
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return GetContainerInsertPosition(pScreen, dragItem);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, before);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, before);
	}
}
