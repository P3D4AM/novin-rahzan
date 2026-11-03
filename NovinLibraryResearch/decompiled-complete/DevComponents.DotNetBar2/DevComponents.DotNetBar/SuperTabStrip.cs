using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.SuperTabStripDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class SuperTabStrip : ItemControl, ISupportInitialize
{
	private enum _20A5
	{
		ᘶ,
		_20A6,
		_20A7
	}

	private EventHandler<SuperTabStripTabItemOpenEventArgs> ᓮ;

	private EventHandler<SuperTabStripTabItemCloseEventArgs> ᓭ;

	private EventHandler<SuperTabStripTabRemovedEventArgs> ᓯ;

	private EventHandler<SuperTabStripBeforeTabDisplayEventArgs> _0C55;

	private EventHandler<SuperTabStripTabMovingEventArgs> ἔ;

	private EventHandler<SuperTabStripTabMovedEventArgs> ᓪ;

	private EventHandler<SuperTabStripSelectedTabChangingEventArgs> ᓩ;

	private EventHandler<SuperTabStripSelectedTabChangedEventArgs> Ꮷ;

	private EventHandler<SuperTabGetTabItemPathEventArgs> ἕ;

	private EventHandler<SuperTabGetTabItemContentRectangleEventArgs> _1F16;

	private EventHandler<SuperTabMeasureTabItemEventArgs> ᓱ;

	private EventHandler<SuperTabPreRenderTabItemEventArgs> ᓲ;

	private EventHandler<SuperTabPostRenderTabItemEventArgs> ᓳ;

	private EventHandler<SuperTabGetTabTextBoundsEventArgs> _1F17;

	private EventHandler<SuperTabGetTabImageBoundsEventArgs> Ἐ;

	private EventHandler<SuperTabGetTabCloseBoundsEventArgs> Ἑ;

	private EventHandler<SuperTabStripPaintBackgroundEventArgs> Ἒ;

	private EventHandler<SuperTabStripTabColorChangedEventArgs> Ἰ;

	private BaseItem _ᾑ;

	private SuperTabStripItem _ὗ;

	private bool __2054;

	private bool __2055 = true;

	private BaseItem __2056;

	private bool __2057;

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

	[DefaultValue(true)]
	[Category("Close Button")]
	[Description("Indicates whether tab is automatically closed when close button is clicked.")]
	[Browsable(true)]
	public bool AutoCloseTabs
	{
		get
		{
			return _ὗ.AutoCloseTabs;
		}
		set
		{
			_ὗ.AutoCloseTabs = value;
		}
	}

	[Description("Indicates whether the tab's visible close button is displayed for every tab state.")]
	[DefaultValue(true)]
	[Category("Close Button")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return _ὗ.CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			_ὗ.CloseButtonOnTabsAlwaysDisplayed = value;
		}
	}

	[Description("Indicates whether close button is visible on each tab.")]
	[DevCoBrowsable(true)]
	[Category("Close Button")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return _ὗ.CloseButtonOnTabsVisible;
		}
		set
		{
			_ὗ.CloseButtonOnTabsVisible = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the position of the tab close button.")]
	[Category("Close Button")]
	[DevCoBrowsable(true)]
	[DefaultValue(eTabCloseButtonPosition.Right)]
	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return _ὗ.CloseButtonPosition;
		}
		set
		{
			_ὗ.CloseButtonPosition = value;
		}
	}

	[Description("Indicates the custom tab Close button image.")]
	[Category("Close Button")]
	[Browsable(true)]
	[DefaultValue(null)]
	public Image TabCloseButtonNormal
	{
		get
		{
			return _ὗ.TabCloseButtonNormal;
		}
		set
		{
			_ὗ.TabCloseButtonNormal = value;
		}
	}

	public Image TabCloseButtonHot
	{
		get
		{
			return _ὗ.TabCloseButtonHot;
		}
		set
		{
			_ὗ.TabCloseButtonHot = value;
		}
	}

	[Description("Indicates custom Close button image that is used on tabs when the button has been pressed.")]
	[DefaultValue(null)]
	[Category("Close Button")]
	[Browsable(true)]
	public Image TabCloseButtonPressed
	{
		get
		{
			return _ὗ.TabCloseButtonPressed;
		}
		set
		{
			_ὗ.TabCloseButtonPressed = value;
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
			__2055 = value;
			((Control)this).Visible = value;
			if (((Control)this).Parent is SuperTabControl superTabControl)
			{
				superTabControl.ᖐ();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the TabStrip ControlBox.")]
	[Category("Appearance")]
	[Browsable(true)]
	public SuperTabControlBox ControlBox => _ὗ.ControlBox;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public BaseItem DesignTimeSelection
	{
		get
		{
			return _ᾑ;
		}
		set
		{
			_ᾑ = value;
			((Control)this).Invalidate();
		}
	}

	[Description("Specifies whether the only Text displayed is for the SelectedTab.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return _ὗ.DisplaySelectedTextOnly;
		}
		set
		{
			_ὗ.DisplaySelectedTextOnly = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text is drawn horizontally regardless of tab orientation.")]
	[Browsable(true)]
	public bool HorizontalText
	{
		get
		{
			return _ὗ.HorizontalText;
		}
		set
		{
			_ὗ.HorizontalText = value;
		}
	}

	[DefaultValue(ImageAlignment.NotSet)]
	[Description("Indicates the default alignment of the Image within the tab.")]
	[Category("Appearance")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return _ὗ.ImageAlignment;
		}
		set
		{
			_ὗ.ImageAlignment = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Browsable(true)]
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

	[Category("Layout")]
	[Description("Indicates BaseItem tab padding in pixels.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public Padding ItemPadding
	{
		get
		{
			return _ὗ.ItemPadding;
		}
		set
		{
			_ὗ.ItemPadding = value;
		}
	}

	[Description("Indicates whether vertical tab text is rotated 180 degrees.")]
	[Category("Layout")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool RotateVerticalText
	{
		get
		{
			return _ὗ.RotateVerticalText;
		}
		set
		{
			_ὗ.RotateVerticalText = value;
		}
	}

	[Description("Indicates whether focus rectangle is displayed when tab has input focus.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool ShowFocusRectangle
	{
		get
		{
			return _ὗ.ShowFocusRectangle;
		}
		set
		{
			_ὗ.ShowFocusRectangle = value;
		}
	}

	[Description("Indicates the fixed tab size in pixels. Either Height, Width, or both can be set.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Size FixedTabSize
	{
		get
		{
			return _ὗ.FixedTabSize;
		}
		set
		{
			_ὗ.FixedTabSize = value;
		}
	}

	[Browsable(false)]
	public bool IsTabDragging => _ὗ.IsTabDragging;

	[Category("Behavior")]
	[Description("Indicates whether the control tabs can be reordered through the user interface.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public bool ReorderTabsEnabled
	{
		get
		{
			return _ὗ.ReorderTabsEnabled;
		}
		set
		{
			_ὗ.ReorderTabsEnabled = value;
		}
	}

	[Category("Behavior")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public SuperTabItem SelectedTab
	{
		get
		{
			return _ὗ.SelectedTab;
		}
		set
		{
			_ὗ.SelectedTab = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates the selected tab Font")]
	[Browsable(true)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	[Localizable(true)]
	public Font SelectedTabFont
	{
		get
		{
			return _ὗ.SelectedTabFont;
		}
		set
		{
			_ὗ.SelectedTabFont = value;
		}
	}

	[Description("Indicates the index of the selected tab.")]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int SelectedTabIndex
	{
		get
		{
			return _ὗ.SelectedTabIndex;
		}
		set
		{
			_ὗ.SelectedTabIndex = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates the tab alignment within the Tab-Strip control.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(eTabStripAlignment.Top)]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			return _ὗ.TabAlignment;
		}
		set
		{
			_ὗ.TabAlignment = value;
		}
	}

	[DevCoBrowsable(true)]
	[Localizable(true)]
	[DefaultValue(null)]
	[Category("Style")]
	[Description("Indicates the tab Font")]
	[Browsable(true)]
	public Font TabFont
	{
		get
		{
			return _ὗ.TabFont;
		}
		set
		{
			_ὗ.TabFont = value;
		}
	}

	[DefaultValue(5)]
	[Category("Appearance")]
	[Description("Indicates the Horizontal spacing around tab elements.")]
	[Browsable(true)]
	public int TabHorizontalSpacing
	{
		get
		{
			return _ὗ.TabHorizontalSpacing;
		}
		set
		{
			_ὗ.TabHorizontalSpacing = value;
		}
	}

	[DefaultValue(eSuperTabLayoutType.SingleLine)]
	[Description("Indicates the type of the tab layout.")]
	[Category("Appearance")]
	[Browsable(true)]
	public eSuperTabLayoutType TabLayoutType
	{
		get
		{
			return _ὗ.TabLayoutType;
		}
		set
		{
			_ὗ.TabLayoutType = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.SuperTabStripTabsEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Data")]
	[Description("Returns the collection of Tabs.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Tabs => _ὗ.SubItems;

	[Description("Indicates the Color of the TabStrip.")]
	[Browsable(true)]
	[Category("Appearance")]
	public SuperTabColorTable TabStripColor
	{
		get
		{
			return _ὗ.TabStripColor;
		}
		set
		{
			_ὗ.TabStripColor = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(eSuperTabStyle.Office2007)]
	[Description("Indicates the tab style.")]
	[Category("Appearance")]
	[Browsable(true)]
	public eSuperTabStyle TabStyle
	{
		get
		{
			return _ὗ.TabStyle;
		}
		set
		{
			_ὗ.TabStyle = value;
		}
	}

	[Description("Indicates the Vertical spacing around tab elements.")]
	[Category("Appearance")]
	[DefaultValue(4)]
	[Browsable(true)]
	public int TabVerticalSpacing
	{
		get
		{
			return _ὗ.TabVerticalSpacing;
		}
		set
		{
			_ὗ.TabVerticalSpacing = value;
		}
	}

	[Browsable(false)]
	public SuperTabStripItem TabStripItem => _ὗ;

	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates tab text alignment.")]
	[Browsable(true)]
	[DefaultValue(eItemAlignment.Near)]
	public eItemAlignment TextAlignment
	{
		get
		{
			return _ὗ.TextAlignment;
		}
		set
		{
			_ὗ.TextAlignment = value;
		}
	}

	internal bool _2052 => __2055;

	internal bool Ꮬ
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

	internal bool _2053 => __2054;

	internal ῲ έ => _ὗ.έ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BaseItem ApplicationButton
	{
		get
		{
			return __2056;
		}
		set
		{
			__2056 = value;
		}
	}

	protected override Size DefaultSize => new Size(236, 25);

	[Description("Occurs when a tab is added to the Tabs collection.")]
	public event EventHandler<SuperTabStripTabItemOpenEventArgs> TabItemOpen
	{
		add
		{
			EventHandler<SuperTabStripTabItemOpenEventArgs> eventHandler = ᓮ;
			EventHandler<SuperTabStripTabItemOpenEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabItemOpenEventArgs> value2 = (EventHandler<SuperTabStripTabItemOpenEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripTabItemOpenEventArgs> eventHandler = ᓮ;
			EventHandler<SuperTabStripTabItemOpenEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabItemOpenEventArgs> value2 = (EventHandler<SuperTabStripTabItemOpenEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a tab is about to Close.")]
	public event EventHandler<SuperTabStripTabItemCloseEventArgs> TabItemClose
	{
		add
		{
			EventHandler<SuperTabStripTabItemCloseEventArgs> eventHandler = ᓭ;
			EventHandler<SuperTabStripTabItemCloseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabItemCloseEventArgs> value2 = (EventHandler<SuperTabStripTabItemCloseEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓭ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripTabItemCloseEventArgs> eventHandler = ᓭ;
			EventHandler<SuperTabStripTabItemCloseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabItemCloseEventArgs> value2 = (EventHandler<SuperTabStripTabItemCloseEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓭ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a tab is removed from the Tabs collection.")]
	public event EventHandler<SuperTabStripTabRemovedEventArgs> TabRemoved
	{
		add
		{
			EventHandler<SuperTabStripTabRemovedEventArgs> eventHandler = ᓯ;
			EventHandler<SuperTabStripTabRemovedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabRemovedEventArgs> value2 = (EventHandler<SuperTabStripTabRemovedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripTabRemovedEventArgs> eventHandler = ᓯ;
			EventHandler<SuperTabStripTabRemovedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabRemovedEventArgs> value2 = (EventHandler<SuperTabStripTabRemovedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a tab is about to be displayed.")]
	public event EventHandler<SuperTabStripBeforeTabDisplayEventArgs> BeforeTabDisplay
	{
		add
		{
			EventHandler<SuperTabStripBeforeTabDisplayEventArgs> eventHandler = _0C55;
			EventHandler<SuperTabStripBeforeTabDisplayEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripBeforeTabDisplayEventArgs> value2 = (EventHandler<SuperTabStripBeforeTabDisplayEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0C55, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripBeforeTabDisplayEventArgs> eventHandler = _0C55;
			EventHandler<SuperTabStripBeforeTabDisplayEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripBeforeTabDisplayEventArgs> value2 = (EventHandler<SuperTabStripBeforeTabDisplayEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0C55, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a tab is being moved or dragged by the user.")]
	public event EventHandler<SuperTabStripTabMovingEventArgs> TabMoving
	{
		add
		{
			EventHandler<SuperTabStripTabMovingEventArgs> eventHandler = ἔ;
			EventHandler<SuperTabStripTabMovingEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabMovingEventArgs> value2 = (EventHandler<SuperTabStripTabMovingEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripTabMovingEventArgs> eventHandler = ἔ;
			EventHandler<SuperTabStripTabMovingEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabMovingEventArgs> value2 = (EventHandler<SuperTabStripTabMovingEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a tab has been moved or dragged by the user.")]
	public event EventHandler<SuperTabStripTabMovedEventArgs> TabMoved
	{
		add
		{
			EventHandler<SuperTabStripTabMovedEventArgs> eventHandler = ᓪ;
			EventHandler<SuperTabStripTabMovedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabMovedEventArgs> value2 = (EventHandler<SuperTabStripTabMovedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓪ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripTabMovedEventArgs> eventHandler = ᓪ;
			EventHandler<SuperTabStripTabMovedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabMovedEventArgs> value2 = (EventHandler<SuperTabStripTabMovedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓪ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the Selected tab is changing.")]
	public event EventHandler<SuperTabStripSelectedTabChangingEventArgs> SelectedTabChanging
	{
		add
		{
			EventHandler<SuperTabStripSelectedTabChangingEventArgs> eventHandler = ᓩ;
			EventHandler<SuperTabStripSelectedTabChangingEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripSelectedTabChangingEventArgs> value2 = (EventHandler<SuperTabStripSelectedTabChangingEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓩ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripSelectedTabChangingEventArgs> eventHandler = ᓩ;
			EventHandler<SuperTabStripSelectedTabChangingEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripSelectedTabChangingEventArgs> value2 = (EventHandler<SuperTabStripSelectedTabChangingEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓩ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the Selected tab has changed.")]
	public event EventHandler<SuperTabStripSelectedTabChangedEventArgs> SelectedTabChanged
	{
		add
		{
			EventHandler<SuperTabStripSelectedTabChangedEventArgs> eventHandler = Ꮷ;
			EventHandler<SuperTabStripSelectedTabChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripSelectedTabChangedEventArgs> value2 = (EventHandler<SuperTabStripSelectedTabChangedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripSelectedTabChangedEventArgs> eventHandler = Ꮷ;
			EventHandler<SuperTabStripSelectedTabChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripSelectedTabChangedEventArgs> value2 = (EventHandler<SuperTabStripSelectedTabChangedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ꮷ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control needs a tab's bordering path.")]
	public event EventHandler<SuperTabGetTabItemPathEventArgs> GetTabItemPath
	{
		add
		{
			EventHandler<SuperTabGetTabItemPathEventArgs> eventHandler = ἕ;
			EventHandler<SuperTabGetTabItemPathEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabItemPathEventArgs> value2 = (EventHandler<SuperTabGetTabItemPathEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἕ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabGetTabItemPathEventArgs> eventHandler = ἕ;
			EventHandler<SuperTabGetTabItemPathEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabItemPathEventArgs> value2 = (EventHandler<SuperTabGetTabItemPathEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἕ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control needs a tab's Content Rectangle.")]
	public event EventHandler<SuperTabGetTabItemContentRectangleEventArgs> GetTabItemContentRectangle
	{
		add
		{
			EventHandler<SuperTabGetTabItemContentRectangleEventArgs> eventHandler = _1F16;
			EventHandler<SuperTabGetTabItemContentRectangleEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabItemContentRectangleEventArgs> value2 = (EventHandler<SuperTabGetTabItemContentRectangleEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F16, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabGetTabItemContentRectangleEventArgs> eventHandler = _1F16;
			EventHandler<SuperTabGetTabItemContentRectangleEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabItemContentRectangleEventArgs> value2 = (EventHandler<SuperTabGetTabItemContentRectangleEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F16, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control needs to measure a tab.")]
	public event EventHandler<SuperTabMeasureTabItemEventArgs> MeasureTabItem
	{
		add
		{
			EventHandler<SuperTabMeasureTabItemEventArgs> eventHandler = ᓱ;
			EventHandler<SuperTabMeasureTabItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabMeasureTabItemEventArgs> value2 = (EventHandler<SuperTabMeasureTabItemEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓱ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabMeasureTabItemEventArgs> eventHandler = ᓱ;
			EventHandler<SuperTabMeasureTabItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabMeasureTabItemEventArgs> value2 = (EventHandler<SuperTabMeasureTabItemEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓱ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before any tab rendering is done.")]
	public event EventHandler<SuperTabPreRenderTabItemEventArgs> PreRenderTabItem
	{
		add
		{
			EventHandler<SuperTabPreRenderTabItemEventArgs> eventHandler = ᓲ;
			EventHandler<SuperTabPreRenderTabItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabPreRenderTabItemEventArgs> value2 = (EventHandler<SuperTabPreRenderTabItemEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabPreRenderTabItemEventArgs> eventHandler = ᓲ;
			EventHandler<SuperTabPreRenderTabItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabPreRenderTabItemEventArgs> value2 = (EventHandler<SuperTabPreRenderTabItemEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs After all tab rendering is complete.")]
	public event EventHandler<SuperTabPostRenderTabItemEventArgs> PostRenderTabItem
	{
		add
		{
			EventHandler<SuperTabPostRenderTabItemEventArgs> eventHandler = ᓳ;
			EventHandler<SuperTabPostRenderTabItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabPostRenderTabItemEventArgs> value2 = (EventHandler<SuperTabPostRenderTabItemEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabPostRenderTabItemEventArgs> eventHandler = ᓳ;
			EventHandler<SuperTabPostRenderTabItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabPostRenderTabItemEventArgs> value2 = (EventHandler<SuperTabPostRenderTabItemEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᓳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control needs to get the tab's Text Bounds.")]
	public event EventHandler<SuperTabGetTabTextBoundsEventArgs> GetTabTextBounds
	{
		add
		{
			EventHandler<SuperTabGetTabTextBoundsEventArgs> eventHandler = _1F17;
			EventHandler<SuperTabGetTabTextBoundsEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabTextBoundsEventArgs> value2 = (EventHandler<SuperTabGetTabTextBoundsEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F17, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabGetTabTextBoundsEventArgs> eventHandler = _1F17;
			EventHandler<SuperTabGetTabTextBoundsEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabTextBoundsEventArgs> value2 = (EventHandler<SuperTabGetTabTextBoundsEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F17, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control needs to get the tab's Image Bounds.")]
	public event EventHandler<SuperTabGetTabImageBoundsEventArgs> GetTabImageBounds
	{
		add
		{
			EventHandler<SuperTabGetTabImageBoundsEventArgs> eventHandler = Ἐ;
			EventHandler<SuperTabGetTabImageBoundsEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabImageBoundsEventArgs> value2 = (EventHandler<SuperTabGetTabImageBoundsEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἐ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabGetTabImageBoundsEventArgs> eventHandler = Ἐ;
			EventHandler<SuperTabGetTabImageBoundsEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabImageBoundsEventArgs> value2 = (EventHandler<SuperTabGetTabImageBoundsEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἐ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control needs to get the tab's Close Button Bounds.")]
	public event EventHandler<SuperTabGetTabCloseBoundsEventArgs> GetTabCloseBounds
	{
		add
		{
			EventHandler<SuperTabGetTabCloseBoundsEventArgs> eventHandler = Ἑ;
			EventHandler<SuperTabGetTabCloseBoundsEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabCloseBoundsEventArgs> value2 = (EventHandler<SuperTabGetTabCloseBoundsEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἑ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabGetTabCloseBoundsEventArgs> eventHandler = Ἑ;
			EventHandler<SuperTabGetTabCloseBoundsEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabGetTabCloseBoundsEventArgs> value2 = (EventHandler<SuperTabGetTabCloseBoundsEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἑ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the TabStrip background needs painted.")]
	public event EventHandler<SuperTabStripPaintBackgroundEventArgs> TabStripPaintBackground
	{
		add
		{
			EventHandler<SuperTabStripPaintBackgroundEventArgs> eventHandler = Ἒ;
			EventHandler<SuperTabStripPaintBackgroundEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripPaintBackgroundEventArgs> value2 = (EventHandler<SuperTabStripPaintBackgroundEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἒ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripPaintBackgroundEventArgs> eventHandler = Ἒ;
			EventHandler<SuperTabStripPaintBackgroundEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripPaintBackgroundEventArgs> value2 = (EventHandler<SuperTabStripPaintBackgroundEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἒ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the TabStrip TabColor has changed.")]
	public event EventHandler<SuperTabStripTabColorChangedEventArgs> TabStripTabColorChanged
	{
		add
		{
			EventHandler<SuperTabStripTabColorChangedEventArgs> eventHandler = Ἰ;
			EventHandler<SuperTabStripTabColorChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabColorChangedEventArgs> value2 = (EventHandler<SuperTabStripTabColorChangedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἰ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SuperTabStripTabColorChangedEventArgs> eventHandler = Ἰ;
			EventHandler<SuperTabStripTabColorChangedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SuperTabStripTabColorChangedEventArgs> value2 = (EventHandler<SuperTabStripTabColorChangedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἰ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SuperTabStrip()
	{
		_ὗ = new SuperTabStripItem(this);
		_ὗ.GlobalItem = false;
		_ὗ.ContainerControl = this;
		_ὗ.Stretch = false;
		_ὗ.Displayed = true;
		_ὗ.SetOwner(this);
		SetBaseItemContainer(_ὗ);
		((Control)this).SetStyle((ControlStyles)2048, true);
		ἧ(Ἠ: true);
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return _ὗ.ShouldSerializeFixedTabSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedTabSize()
	{
		TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeSelectedTabFont()
	{
		return _ὗ.ShouldSerializeSelectedTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		_ὗ.ResetSelectedTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeTabFont()
	{
		return _ὗ.ShouldSerializeTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		_ὗ.ResetTabFont();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabStripColor()
	{
		return !_ὗ.TabStripColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabStripColor()
	{
		_ὗ.TabStripColor = new SuperTabColorTable();
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			_ὗ.TabItemOpen += _2058;
			_ὗ.TabItemClose += _2059;
			_ὗ.TabRemoved += _205A;
			_ὗ.TabMoving += _205B;
			_ὗ.TabMoved += _205C;
			_ὗ.GetTabItemPath += _205D;
			_ὗ.GetTabItemContentRectangle += _205E;
			_ὗ.BeforeTabDisplay += _205F;
			_ὗ.MeasureTabItem += _2060;
			_ὗ.PreRenderTabItem += _2061;
			_ὗ.PostRenderTabItem += _2062;
			_ὗ.GetTabTextBounds += _2065;
			_ὗ.GetTabImageBounds += _2064;
			_ὗ.GetTabCloseBounds += _2063;
			_ὗ.TabStripPaintBackground += _2066;
			_ὗ.SelectedTabChanging += _2067;
			_ὗ.SelectedTabChanged += _2068;
			_ὗ.TabStripTabColorChanged += _2069;
		}
		else
		{
			_ὗ.TabItemOpen -= _2058;
			_ὗ.TabItemClose -= _2059;
			_ὗ.TabRemoved -= _205A;
			_ὗ.TabMoving -= _205B;
			_ὗ.TabMoved -= _205C;
			_ὗ.GetTabItemPath -= _205D;
			_ὗ.GetTabItemContentRectangle -= _205E;
			_ὗ.BeforeTabDisplay -= _205F;
			_ὗ.MeasureTabItem -= _2060;
			_ὗ.PreRenderTabItem -= _2061;
			_ὗ.PostRenderTabItem -= _2062;
			_ὗ.GetTabTextBounds -= _2065;
			_ὗ.GetTabImageBounds -= _2064;
			_ὗ.GetTabCloseBounds -= _2063;
			_ὗ.TabStripPaintBackground -= _2066;
			_ὗ.SelectedTabChanging -= _2067;
			_ὗ.SelectedTabChanged -= _2068;
			_ὗ.TabStripTabColorChanged -= _2069;
		}
	}

	private void _2058(object _٠, SuperTabStripTabItemOpenEventArgs ؿ)
	{
		if (ᓮ != null)
		{
			ᓮ(this, ؿ);
		}
	}

	private void _2059(object _٠, SuperTabStripTabItemCloseEventArgs ؿ)
	{
		if (ᓭ != null)
		{
			ᓭ(this, ؿ);
		}
	}

	private void _205A(object _٠, SuperTabStripTabRemovedEventArgs ؿ)
	{
		if (ᓯ != null)
		{
			ᓯ(this, ؿ);
		}
	}

	private void _205B(object _٠, SuperTabStripTabMovingEventArgs ؿ)
	{
		if (ἔ != null)
		{
			ἔ(this, ؿ);
		}
	}

	private void _205C(object _٠, SuperTabStripTabMovedEventArgs ؿ)
	{
		if (ᓪ != null)
		{
			ᓪ(this, ؿ);
		}
	}

	private void _205D(object _٠, SuperTabGetTabItemPathEventArgs ؿ)
	{
		if (ἕ != null)
		{
			ἕ(this, ؿ);
		}
	}

	private void _205E(object _٠, SuperTabGetTabItemContentRectangleEventArgs ؿ)
	{
		if (_1F16 != null)
		{
			_1F16(this, ؿ);
		}
	}

	private void _205F(object _٠, SuperTabStripBeforeTabDisplayEventArgs ؿ)
	{
		if (_0C55 != null)
		{
			_0C55(this, ؿ);
		}
	}

	private void _2060(object _٠, SuperTabMeasureTabItemEventArgs ؿ)
	{
		if (ᓱ != null)
		{
			ᓱ(this, ؿ);
		}
	}

	private void _2061(object _٠, SuperTabPreRenderTabItemEventArgs ؿ)
	{
		if (ᓲ != null)
		{
			ᓲ(this, ؿ);
		}
	}

	private void _2062(object _٠, SuperTabPostRenderTabItemEventArgs ؿ)
	{
		if (ᓳ != null)
		{
			ᓳ(this, ؿ);
		}
	}

	private void _2063(object _٠, SuperTabGetTabCloseBoundsEventArgs ؿ)
	{
		if (Ἑ != null)
		{
			Ἑ(this, ؿ);
		}
	}

	private void _2064(object _٠, SuperTabGetTabImageBoundsEventArgs ؿ)
	{
		if (Ἐ != null)
		{
			Ἐ(this, ؿ);
		}
	}

	private void _2065(object _٠, SuperTabGetTabTextBoundsEventArgs ؿ)
	{
		if (_1F17 != null)
		{
			_1F17(this, ؿ);
		}
	}

	private void _2066(object _٠, SuperTabStripPaintBackgroundEventArgs ؿ)
	{
		if (Ἒ != null)
		{
			Ἒ(this, ؿ);
		}
	}

	private void _2067(object _٠, SuperTabStripSelectedTabChangingEventArgs ؿ)
	{
		if (ᓩ != null)
		{
			ᓩ(this, ؿ);
		}
	}

	private void _2068(object _٠, SuperTabStripSelectedTabChangedEventArgs ؿ)
	{
		if (Ꮷ != null)
		{
			Ꮷ(this, ؿ);
		}
	}

	private void _2069(object _٠, SuperTabStripTabColorChangedEventArgs ؿ)
	{
		if (Ἰ != null)
		{
			Ἰ(this, ؿ);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			((Control)this).Invalidate(true);
		}
		έ._1FDF();
	}

	public override void RecalcLayout()
	{
		if (!((Control)this).IsHandleCreated || __2057)
		{
			return;
		}
		__2057 = true;
		try
		{
			base.RecalcLayout();
			if (!__2054 && _ὗ != null)
			{
				_ὗ.RecalcSize();
			}
		}
		finally
		{
			__2057 = false;
		}
	}

	public void EnsureVisible(SuperTabItem tab)
	{
		_ὗ.EnsureVisible(tab);
	}

	public SuperTabItem GetTabFromPoint(Point pt)
	{
		return _ὗ.GetTabFromPoint(pt);
	}

	public BaseItem GetItemFromPoint(Point pt)
	{
		return _ὗ.GetItemFromPoint(pt);
	}

	public void CloseTab(SuperTabItem tab)
	{
		_ὗ.CloseTab(tab);
	}

	public bool SelectPreviousTab()
	{
		return _ὗ.SelectPreviousTab();
	}

	public bool SelectNextTab()
	{
		return _ὗ.SelectNextTab();
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		if (_ὗ != null)
		{
			_ὗ.ή = null;
		}
		RecalcLayout();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (SelectedTab != null)
		{
			SelectedTab.Refresh();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		if (SelectedTab != null)
		{
			SelectedTab.Refresh();
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected I4, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		base.OnKeyDown(e);
		if (e.Handled)
		{
			return;
		}
		Keys keyData = e.KeyData;
		if ((int)keyData != 9)
		{
			switch (keyData - 35)
			{
			default:
				if ((int)keyData == 65545)
				{
					((Control)this).Parent.SelectNextControl((Control)(object)this, false, true, true, true);
					if (SelectedTab != null)
					{
						SelectedTab.Refresh();
					}
				}
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				switch (_206A(e.KeyData))
				{
				case _20A5._20A6:
					if (SelectPreviousTab())
					{
						e.Handled = true;
					}
					break;
				case _20A5._20A7:
					if (SelectNextTab())
					{
						e.Handled = true;
					}
					break;
				}
				break;
			case 1:
			{
				SuperTabItem ὦ = TabStripItem.Ὦ;
				if (ὦ != null)
				{
					TabStripItem.SelectTab(ὦ, eEventSource.Keyboard);
					e.Handled = true;
				}
				break;
			}
			case 0:
			{
				SuperTabItem ά = TabStripItem.ά;
				if (ά != null)
				{
					TabStripItem.SelectTab(ά, eEventSource.Keyboard);
					e.Handled = true;
				}
				break;
			}
			}
		}
		else
		{
			((Control)this).Parent.SelectNextControl((Control)(object)this, true, true, true, true);
			if (SelectedTab != null)
			{
				SelectedTab.Refresh();
			}
		}
		if (e.Handled)
		{
			((Control)this).Focus();
		}
	}

	private _20A5 _206A(Keys _206B)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected I4, but got Unknown
		switch (_206B - 37)
		{
		case 0:
			if (!TabStripItem.HorizontalText && ((TabStripItem.TabAlignment == eTabStripAlignment.Left && !TabStripItem.RotateVerticalText) || (TabStripItem.TabAlignment == eTabStripAlignment.Right && TabStripItem.RotateVerticalText)))
			{
				return _20A5._20A7;
			}
			return _20A5._20A6;
		case 2:
			if (!TabStripItem.HorizontalText && ((TabStripItem.TabAlignment == eTabStripAlignment.Left && !TabStripItem.RotateVerticalText) || (TabStripItem.TabAlignment == eTabStripAlignment.Right && TabStripItem.RotateVerticalText)))
			{
				return _20A5._20A6;
			}
			return _20A5._20A7;
		case 1:
			return _20A5._20A6;
		case 3:
			return _20A5._20A7;
		default:
			return _20A5.ᘶ;
		}
	}

	protected override bool IsInputKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected I4, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData != 9)
		{
			switch (keyData - 37)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				break;
			default:
				return ((Control)this).IsInputKey(keyData);
			}
		}
		return true;
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
				_ὗ.SelectTab((SuperTabItem)item, eEventSource.Keyboard);
				return true;
			}
			return false;
		}
		return base.ProcessItemMnemonicKey(item);
	}

	private void ህ()
	{
		__2054 = true;
	}

	void ISupportInitialize.BeginInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ህ
		this.ህ();
	}

	private void ሆ()
	{
		__2054 = false;
	}

	void ISupportInitialize.EndInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ሆ
		this.ሆ();
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
				((ApplicationButton)ApplicationButton).ᰇ(charCode);
			}
			else if (ApplicationButton is MetroAppButton)
			{
				((MetroAppButton)ApplicationButton).ᰇ(charCode);
			}
		}
		return flag;
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		if (ApplicationButton != null)
		{
			Size _၂ = _၁._၂;
			Size size = TextDrawing.MeasureString(g, keyTip, font);
			size.Width += _၂.Width;
			size.Height += _၂.Height;
			Rectangle displayRectangle = item.DisplayRectangle;
			return (!(item is SuperTabItem)) ? new Rectangle(displayRectangle.X + 24, displayRectangle.Y - 2, size.Width, size.Height) : new Rectangle(displayRectangle.X + 2, displayRectangle.Y, size.Width, size.Height);
		}
		return base.GetKeyTipRectangle(g, item, font, keyTip);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !((Control)this).IsDisposed)
		{
			ἧ(Ἠ: false);
		}
		base.Dispose(disposing);
	}
}
