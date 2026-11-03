using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.SuperTabControlDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("SelectedTabChanged")]
[ToolboxItem(true)]
public class SuperTabControl : Control, ISupportInitialize
{
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

	private EventHandler<MouseEventArgs> Ἓ;

	private EventHandler<MouseEventArgs> Ἔ;

	private EventHandler<MouseEventArgs> Ἕ;

	private EventHandler<MouseEventArgs> _1F1E;

	private EventHandler<MouseEventArgs> _1F1F;

	private EventHandler<EventArgs> ἠ;

	private EventHandler<EventArgs> ἡ;

	private EventHandler<EventArgs> ἢ;

	private readonly SuperTabStrip _Ꮺ;

	private bool _ᠭ;

	private bool _ἣ = true;

	private int _ἤ = -1;

	private bool _ἥ = true;

	[Category("Behavior")]
	[DefaultValue(true)]
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

	[Category("Appearance")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return _Ꮺ.AntiAlias;
		}
		set
		{
			_Ꮺ.AntiAlias = value;
		}
	}

	[Category("Close Button")]
	[Browsable(true)]
	[Description("Indicates whether tab is automatically closed when close button is clicked.")]
	[DefaultValue(true)]
	public bool AutoCloseTabs
	{
		get
		{
			return _Ꮺ.AutoCloseTabs;
		}
		set
		{
			_Ꮺ.AutoCloseTabs = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates whether the tab's visible close button is displayed for every tab state.")]
	[DefaultValue(true)]
	[Category("Close Button")]
	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return _Ꮺ.CloseButtonOnTabsAlwaysDisplayed;
		}
		set
		{
			_Ꮺ.CloseButtonOnTabsAlwaysDisplayed = value;
		}
	}

	[DefaultValue(false)]
	[Category("Close Button")]
	[Description("Indicates whether close button is visible on each tab.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return _Ꮺ.CloseButtonOnTabsVisible;
		}
		set
		{
			_Ꮺ.CloseButtonOnTabsVisible = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates the position of the tab close button.")]
	[Category("Close Button")]
	[Browsable(true)]
	[DefaultValue(eTabCloseButtonPosition.Right)]
	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return _Ꮺ.CloseButtonPosition;
		}
		set
		{
			_Ꮺ.CloseButtonPosition = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Indicates the custom tab Close button image.")]
	[Category("Close Button")]
	public Image TabCloseButtonNormal
	{
		get
		{
			return _Ꮺ.TabCloseButtonNormal;
		}
		set
		{
			_Ꮺ.TabCloseButtonNormal = value;
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
			return _Ꮺ.TabCloseButtonHot;
		}
		set
		{
			_Ꮺ.TabCloseButtonHot = value;
		}
	}

	[Category("Close Button")]
	[DefaultValue(null)]
	[Description("Indicates custom Close button image that is used on tabs when the button has been pressed.")]
	[Browsable(true)]
	public Image TabCloseButtonPressed
	{
		get
		{
			return _Ꮺ.TabCloseButtonPressed;
		}
		set
		{
			_Ꮺ.TabCloseButtonPressed = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets the TabStrip ControlBox.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SuperTabControlBox ControlBox => _Ꮺ.ControlBox;

	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Description("Specifies whether the only Text displayed is for the SelectedTab.")]
	public bool DisplaySelectedTextOnly
	{
		get
		{
			return _Ꮺ.DisplaySelectedTextOnly;
		}
		set
		{
			_Ꮺ.DisplaySelectedTextOnly = value;
		}
	}

	[Description("Indicates whether text is drawn horizontally regardless of tab orientation.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	public bool HorizontalText
	{
		get
		{
			return _Ꮺ.HorizontalText;
		}
		set
		{
			_Ꮺ.HorizontalText = value;
		}
	}

	[DefaultValue(ImageAlignment.NotSet)]
	[Category("Appearance")]
	[Description("Indicates the default alignment of the Image within the tab.")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return _Ꮺ.ImageAlignment;
		}
		set
		{
			_Ꮺ.ImageAlignment = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the ImageList used by the TabStrip and its tab items.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	public ImageList ImageList
	{
		get
		{
			return _Ꮺ.ImageList;
		}
		set
		{
			_Ꮺ.ImageList = value;
		}
	}

	[Description("Indicates BaseItem tab padding in pixels.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Layout")]
	public Padding ItemPadding
	{
		get
		{
			return _Ꮺ.ItemPadding;
		}
		set
		{
			_Ꮺ.ItemPadding = value;
		}
	}

	[Description("Indicates whether vertical tab text is rotated 180 degrees.")]
	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(false)]
	public bool RotateVerticalText
	{
		get
		{
			return _Ꮺ.RotateVerticalText;
		}
		set
		{
			_Ꮺ.RotateVerticalText = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public SuperTabControlPanel SelectedPanel
	{
		get
		{
			if (_Ꮺ.SelectedTab != null)
			{
				return _Ꮺ.SelectedTab.AttachedControl as SuperTabControlPanel;
			}
			return null;
		}
		set
		{
			if (value != null && ((Control)this).Controls.Contains((Control)(object)value) && value.TabItem != null && Tabs.Contains(value.TabItem))
			{
				_Ꮺ.SelectedTab = value.TabItem;
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether a focus rectangle is displayed when tab has input focus.")]
	[DefaultValue(false)]
	public bool ShowFocusRectangle
	{
		get
		{
			return _Ꮺ.ShowFocusRectangle;
		}
		set
		{
			_Ꮺ.ShowFocusRectangle = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the fixed tab size in pixels. Either Height, Width, or both can be set.")]
	public Size FixedTabSize
	{
		get
		{
			return _Ꮺ.FixedTabSize;
		}
		set
		{
			_Ꮺ.FixedTabSize = value;
		}
	}

	[Browsable(false)]
	public bool IsTabDragging => _Ꮺ.IsTabDragging;

	[DevCoBrowsable(true)]
	[Description("Indicates whether the control tabs can be reordered through the user interface.")]
	[Browsable(true)]
	[Category("Behavior")]
	public bool ReorderTabsEnabled
	{
		get
		{
			return _Ꮺ.ReorderTabsEnabled;
		}
		set
		{
			_Ꮺ.ReorderTabsEnabled = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	public SuperTabItem SelectedTab
	{
		get
		{
			return _Ꮺ.SelectedTab;
		}
		set
		{
			_Ꮺ.SelectedTab = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Style")]
	[Localizable(true)]
	[Description("Indicates the selected tab Font")]
	[DefaultValue(null)]
	public Font SelectedTabFont
	{
		get
		{
			return _Ꮺ.SelectedTabFont;
		}
		set
		{
			_Ꮺ.SelectedTabFont = value;
		}
	}

	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates the index of the selected tab.")]
	public int SelectedTabIndex
	{
		get
		{
			return _Ꮺ.SelectedTabIndex;
		}
		set
		{
			if (_ᠭ)
			{
				_ἤ = value;
			}
			else
			{
				_Ꮺ.SelectedTabIndex = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabStripAlignment.Top)]
	[Description("Indicates the tab alignment within the Tab-Strip control.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			return _Ꮺ.TabAlignment;
		}
		set
		{
			if (_Ꮺ.TabAlignment != value)
			{
				_Ꮺ.TabAlignment = value;
				ἦ(value);
			}
		}
	}

	[DevCoBrowsable(true)]
	[Localizable(true)]
	[Category("Style")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates the tab Font")]
	public Font TabFont
	{
		get
		{
			return _Ꮺ.TabFont;
		}
		set
		{
			_Ꮺ.TabFont = value;
		}
	}

	[DefaultValue(5)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the Horizontal spacing around tab elements.")]
	public int TabHorizontalSpacing
	{
		get
		{
			return _Ꮺ.TabHorizontalSpacing;
		}
		set
		{
			_Ꮺ.TabHorizontalSpacing = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eSuperTabLayoutType.SingleLine)]
	[Description("Indicates the type of the tab layout.")]
	[Category("Appearance")]
	public eSuperTabLayoutType TabLayoutType
	{
		get
		{
			return _Ꮺ.TabLayoutType;
		}
		set
		{
			_Ꮺ.TabLayoutType = value;
		}
	}

	[Category("Data")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Indicates the collection of Tabs.")]
	[Editor("DevComponents.DotNetBar.Design.SuperTabControlTabsEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public SubItemsCollection Tabs => _Ꮺ.Tabs;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SuperTabStrip TabStrip => _Ꮺ;

	[Category("Appearance")]
	[Description("Indicates the Color of the TabStrip.")]
	[Browsable(true)]
	public SuperTabColorTable TabStripColor
	{
		get
		{
			return _Ꮺ.TabStripColor;
		}
		set
		{
			_Ꮺ.TabStripColor = value;
		}
	}

	[Description("Indicates the tab style.")]
	[DefaultValue(eSuperTabStyle.Office2007)]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public eSuperTabStyle TabStyle
	{
		get
		{
			return _Ꮺ.TabStyle;
		}
		set
		{
			if (_Ꮺ.TabStyle != value)
			{
				_Ꮺ.TabStyle = value;
				ᖐ();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(4)]
	[Description("Indicates the Vertical spacing around tab elements.")]
	public int TabVerticalSpacing
	{
		get
		{
			return _Ꮺ.TabVerticalSpacing;
		}
		set
		{
			_Ꮺ.TabVerticalSpacing = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether tabs are visible")]
	public bool TabsVisible
	{
		get
		{
			return _ἣ;
		}
		set
		{
			if (_ἣ != value)
			{
				_ἣ = value;
				_Ꮺ.Visible = value;
				ᖐ();
			}
		}
	}

	[DefaultValue(eItemAlignment.Near)]
	[Category("Layout")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates tab text alignment.")]
	public eItemAlignment TextAlignment
	{
		get
		{
			return _Ꮺ.TextAlignment;
		}
		set
		{
			_Ꮺ.TextAlignment = value;
		}
	}

	protected override Size DefaultSize => new Size(200, 100);

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether Form.Validate is called and checked before selected tab is changed.")]
	public bool ValidateFormOnTabSelection
	{
		get
		{
			return _ἥ;
		}
		set
		{
			_ἥ = value;
		}
	}

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

	[Description("Occurs when a TabStrip MouseUp event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseUp
	{
		add
		{
			EventHandler<MouseEventArgs> eventHandler = Ἓ;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἓ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<MouseEventArgs> eventHandler = Ἓ;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἓ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a TabStrip MouseDown event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseDown
	{
		add
		{
			EventHandler<MouseEventArgs> eventHandler = Ἔ;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<MouseEventArgs> eventHandler = Ἔ;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἔ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a TabStrip MouseMove event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseMove
	{
		add
		{
			EventHandler<MouseEventArgs> eventHandler = Ἕ;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἕ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<MouseEventArgs> eventHandler = Ἕ;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ἕ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a TabStrip MouseClick event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseClick
	{
		add
		{
			EventHandler<MouseEventArgs> eventHandler = _1F1E;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F1E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<MouseEventArgs> eventHandler = _1F1E;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F1E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a TabStrip MouseDoubleClick event is raised.")]
	public event EventHandler<MouseEventArgs> TabStripMouseDoubleClick
	{
		add
		{
			EventHandler<MouseEventArgs> eventHandler = _1F1F;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F1F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<MouseEventArgs> eventHandler = _1F1F;
			EventHandler<MouseEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1F1F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a TabStrip MouseEnter event is raised.")]
	public event EventHandler<EventArgs> TabStripMouseEnter
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ἠ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἠ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ἠ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἠ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a TabStrip MouseHover event is raised.")]
	public event EventHandler<EventArgs> TabStripMouseHover
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ἡ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἡ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ἡ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἡ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a TabStrip MouseLeave event is raised.")]
	public event EventHandler<EventArgs> TabStripMouseLeave
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ἢ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἢ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ἢ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ἢ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SuperTabControl()
	{
		_Ꮺ = new SuperTabStrip();
		((Control)this).Controls.Add((Control)(object)_Ꮺ);
		((Control)_Ꮺ).Dock = (DockStyle)1;
		_Ꮺ.AutoSelectAttachedControl = true;
		((Control)_Ꮺ).SendToBack();
		ἧ(Ἠ: true);
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
		return _Ꮺ.ShouldSerializeFixedTabSize();
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
		return _Ꮺ.ShouldSerializeSelectedTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		_Ꮺ.ResetSelectedTabFont();
	}

	private void ἦ(eTabStripAlignment ԝ)
	{
		((Control)_Ꮺ).SuspendLayout();
		try
		{
			switch (ԝ)
			{
			case eTabStripAlignment.Top:
				((Control)_Ꮺ).Dock = (DockStyle)1;
				break;
			case eTabStripAlignment.Bottom:
				((Control)_Ꮺ).Dock = (DockStyle)2;
				break;
			case eTabStripAlignment.Left:
				((Control)_Ꮺ).Dock = (DockStyle)3;
				break;
			case eTabStripAlignment.Right:
				((Control)_Ꮺ).Dock = (DockStyle)4;
				break;
			}
			((Control)_Ꮺ).SendToBack();
		}
		finally
		{
			((Control)_Ꮺ).ResumeLayout(true);
		}
		ᖐ();
		RecalcLayout();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeTabFont()
	{
		return _Ꮺ.ShouldSerializeTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		_Ꮺ.ResetTabFont();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeTabStripColor()
	{
		return !_Ꮺ.TabStripColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabStripColor()
	{
		_Ꮺ.TabStripColor = new SuperTabColorTable();
	}

	private void ἧ(bool Ἠ)
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
		if (Ἠ)
		{
			_Ꮺ.TabItemOpen += Ἡ;
			_Ꮺ.TabItemClose += _0C53;
			_Ꮺ.TabRemoved += Ἣ;
			_Ꮺ.TabMoving += Ἢ;
			_Ꮺ.TabMoved += _౭;
			_Ꮺ.SelectedTabChanging += Ἤ;
			_Ꮺ.SelectedTabChanged += Ἥ;
			_Ꮺ.GetTabItemPath += Ἦ;
			_Ꮺ.GetTabItemContentRectangle += Ἧ;
			_Ꮺ.BeforeTabDisplay += ἰ;
			_Ꮺ.MeasureTabItem += ἱ;
			_Ꮺ.PreRenderTabItem += ἲ;
			_Ꮺ.PostRenderTabItem += ἳ;
			_Ꮺ.GetTabTextBounds += ἶ;
			_Ꮺ.GetTabImageBounds += ἵ;
			_Ꮺ.GetTabCloseBounds += ἴ;
			_Ꮺ.TabStripPaintBackground += ἷ;
			_Ꮺ.TabStripTabColorChanged += Ἰ;
			_Ꮺ.MouseUp += new MouseEventHandler(Ἱ);
			_Ꮺ.MouseDown += new MouseEventHandler(Ἲ);
			_Ꮺ.MouseMove += new MouseEventHandler(Ἳ);
			((Control)_Ꮺ).MouseClick += new MouseEventHandler(Ἴ);
			((Control)_Ꮺ).MouseDoubleClick += new MouseEventHandler(Ἵ);
			_Ꮺ.MouseEnter += Ἶ;
			_Ꮺ.MouseHover += Ἷ;
			_Ꮺ.MouseLeave += ὀ;
		}
		else
		{
			_Ꮺ.TabItemOpen -= Ἡ;
			_Ꮺ.TabItemClose -= _0C53;
			_Ꮺ.TabRemoved -= Ἣ;
			_Ꮺ.TabMoving -= Ἢ;
			_Ꮺ.TabMoved -= _౭;
			_Ꮺ.SelectedTabChanging -= Ἤ;
			_Ꮺ.SelectedTabChanged -= Ἥ;
			_Ꮺ.GetTabItemPath -= Ἦ;
			_Ꮺ.GetTabItemContentRectangle -= Ἧ;
			_Ꮺ.BeforeTabDisplay -= ἰ;
			_Ꮺ.MeasureTabItem -= ἱ;
			_Ꮺ.PreRenderTabItem -= ἲ;
			_Ꮺ.PostRenderTabItem -= ἳ;
			_Ꮺ.GetTabTextBounds -= ἶ;
			_Ꮺ.GetTabImageBounds -= ἵ;
			_Ꮺ.GetTabCloseBounds -= ἴ;
			_Ꮺ.TabStripPaintBackground -= ἷ;
			_Ꮺ.TabStripTabColorChanged -= Ἰ;
			_Ꮺ.MouseUp -= new MouseEventHandler(Ἱ);
			_Ꮺ.MouseDown -= new MouseEventHandler(Ἲ);
			_Ꮺ.MouseMove -= new MouseEventHandler(Ἳ);
			((Control)_Ꮺ).MouseClick -= new MouseEventHandler(Ἴ);
			((Control)_Ꮺ).MouseDoubleClick -= new MouseEventHandler(Ἵ);
			_Ꮺ.MouseEnter -= Ἶ;
			_Ꮺ.MouseHover -= Ἷ;
			_Ꮺ.MouseLeave -= ὀ;
		}
	}

	private void Ἡ(object _٠, SuperTabStripTabItemOpenEventArgs ؿ)
	{
		if (ᓮ != null)
		{
			ᓮ(this, ؿ);
		}
	}

	private void _0C53(object _٠, SuperTabStripTabItemCloseEventArgs ؿ)
	{
		if (ᓭ != null)
		{
			ᓭ(this, ؿ);
		}
	}

	private void Ἢ(object _٠, SuperTabStripTabMovingEventArgs ؿ)
	{
		if (ἔ != null)
		{
			ἔ(this, ؿ);
		}
	}

	private void _౭(object _٠, SuperTabStripTabMovedEventArgs ؿ)
	{
		if (ᓪ != null)
		{
			ᓪ(this, ؿ);
		}
	}

	private void Ἣ(object _٠, SuperTabStripTabRemovedEventArgs ؿ)
	{
		if (ᓯ != null)
		{
			ᓯ(this, ؿ);
		}
	}

	private void Ἤ(object _٠, SuperTabStripSelectedTabChangingEventArgs ؿ)
	{
		if (ᓩ != null)
		{
			ᓩ(this, ؿ);
		}
	}

	private void Ἥ(object _٠, SuperTabStripSelectedTabChangedEventArgs ؿ)
	{
		if (Ꮷ != null)
		{
			Ꮷ(this, ؿ);
		}
	}

	private void Ἦ(object _٠, SuperTabGetTabItemPathEventArgs ؿ)
	{
		if (ἕ != null)
		{
			ἕ(this, ؿ);
		}
	}

	private void Ἧ(object _٠, SuperTabGetTabItemContentRectangleEventArgs ؿ)
	{
		if (_1F16 != null)
		{
			_1F16(this, ؿ);
		}
	}

	private void ἰ(object _٠, SuperTabStripBeforeTabDisplayEventArgs ؿ)
	{
		if (_0C55 != null)
		{
			_0C55(this, ؿ);
		}
	}

	private void ἱ(object _٠, SuperTabMeasureTabItemEventArgs ؿ)
	{
		if (ᓱ != null)
		{
			ᓱ(this, ؿ);
		}
	}

	private void ἲ(object _٠, SuperTabPreRenderTabItemEventArgs ؿ)
	{
		if (ᓲ != null)
		{
			ᓲ(this, ؿ);
		}
	}

	private void ἳ(object _٠, SuperTabPostRenderTabItemEventArgs ؿ)
	{
		if (ᓳ != null)
		{
			ᓳ(this, ؿ);
		}
	}

	private void ἴ(object _٠, SuperTabGetTabCloseBoundsEventArgs ؿ)
	{
		if (Ἑ != null)
		{
			Ἑ(this, ؿ);
		}
	}

	private void ἵ(object _٠, SuperTabGetTabImageBoundsEventArgs ؿ)
	{
		if (Ἐ != null)
		{
			Ἐ(this, ؿ);
		}
	}

	private void ἶ(object _٠, SuperTabGetTabTextBoundsEventArgs ؿ)
	{
		if (_1F17 != null)
		{
			_1F17(this, ؿ);
		}
	}

	private void ἷ(object _٠, SuperTabStripPaintBackgroundEventArgs ؿ)
	{
		if (Ἒ != null)
		{
			Ἒ(this, ؿ);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		ᖐ();
	}

	private void Ἰ(object _٠, SuperTabStripTabColorChangedEventArgs ؿ)
	{
		if (ؿ.Tab.AttachedControl is SuperTabControlPanel panel)
		{
			ApplyPanelStyle(panel);
		}
	}

	private void Ἱ(object _٠, MouseEventArgs ؿ)
	{
		if (Ἓ != null)
		{
			Ἓ(_٠, ؿ);
		}
	}

	private void Ἲ(object _٠, MouseEventArgs ؿ)
	{
		if (Ἔ != null)
		{
			Ἔ(_٠, ؿ);
		}
	}

	private void Ἳ(object _٠, MouseEventArgs ؿ)
	{
		if (Ἕ != null)
		{
			Ἕ(_٠, ؿ);
		}
	}

	private void Ἴ(object _٠, MouseEventArgs ؿ)
	{
		if (_1F1E != null)
		{
			_1F1E(_٠, ؿ);
		}
	}

	private void Ἵ(object _٠, MouseEventArgs ؿ)
	{
		if (_1F1F != null)
		{
			_1F1F(_٠, ؿ);
		}
	}

	private void Ἶ(object _٠, EventArgs ؿ)
	{
		if (ἠ != null)
		{
			ἠ(_٠, ؿ);
		}
	}

	private void Ἷ(object _٠, EventArgs ؿ)
	{
		if (ἡ != null)
		{
			ἡ(_٠, ؿ);
		}
	}

	private void ὀ(object _٠, EventArgs ؿ)
	{
		if (ἢ != null)
		{
			ἢ(_٠, ؿ);
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
		return _Ꮺ.SelectPreviousTab();
	}

	public bool SelectNextTab()
	{
		return _Ꮺ.SelectNextTab();
	}

	public SuperTabItem GetTabFromPoint(Point pt)
	{
		return _Ꮺ.GetTabFromPoint(pt);
	}

	public BaseItem GetItemFromPoint(Point pt)
	{
		return _Ꮺ.GetItemFromPoint(pt);
	}

	public void CloseTab(SuperTabItem tab)
	{
		_Ꮺ.CloseTab(tab);
	}

	public void RecalcLayout()
	{
		_Ꮺ.RecalcLayout();
	}

	internal void ᖐ()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (_ᠭ)
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
			ὁ(panel, elementStyle);
			ὂ(elementStyle);
			elementStyle.BackgroundImage = ((Control)panel).BackgroundImage;
			elementStyle.BackgroundImagePosition = panel.BackgroundImagePosition;
			panel.PanelStyle = elementStyle;
		}
	}

	private void ὁ(SuperTabControlPanel Ꮔ, ElementStyle _0657)
	{
		SuperTabPanelItemColorTable superTabPanelItemColorTable = Ꮔ.TabItem.ጩ();
		SuperTabItemStateColorTable superTabItemStateColorTable = Ꮔ.TabItem.ὠ(eTabState.Selected);
		int gradientAngle;
		bool? flag;
		if (!superTabPanelItemColorTable.Background.IsEmpty)
		{
			gradientAngle = superTabPanelItemColorTable.Background.GradientAngle;
			flag = superTabPanelItemColorTable.Background.AdaptiveGradient ?? superTabItemStateColorTable.Background.AdaptiveGradient;
			_0657.BackColorBlend.Clear();
			if (superTabPanelItemColorTable.Background.Colors != null)
			{
				if (superTabPanelItemColorTable.Background.Colors.Length > 2)
				{
					_0657.BackColorBlend.CopyFrom(superTabPanelItemColorTable.Background.GetColorBlendCollection());
				}
				else
				{
					_0657.BackColor = superTabPanelItemColorTable.Background.Colors[0];
					_0657.BackColor2 = ((superTabPanelItemColorTable.Background.Colors.Length == 1) ? superTabPanelItemColorTable.Background.Colors[0] : superTabPanelItemColorTable.Background.Colors[1]);
				}
			}
		}
		else
		{
			gradientAngle = superTabItemStateColorTable.Background.GradientAngle;
			flag = superTabItemStateColorTable.Background.AdaptiveGradient;
			if (superTabItemStateColorTable.Background.Colors != null)
			{
				_0657.BackColor = superTabItemStateColorTable.Background.Colors[0];
				_0657.BackColor2 = ((superTabItemStateColorTable.Background.Colors.Length == 1) ? superTabItemStateColorTable.Background.Colors[0] : superTabItemStateColorTable.Background.Colors[1]);
			}
		}
		if (flag != false)
		{
			switch (_Ꮺ.TabAlignment)
			{
			case eTabStripAlignment.Top:
				_0657.BackColorGradientAngle = gradientAngle - 180;
				break;
			case eTabStripAlignment.Bottom:
				_0657.BackColorGradientAngle = gradientAngle;
				break;
			case eTabStripAlignment.Left:
				_0657.BackColorGradientAngle = gradientAngle + 90;
				break;
			case eTabStripAlignment.Right:
				_0657.BackColorGradientAngle = gradientAngle - 90;
				break;
			}
		}
		_0657.BorderColor = superTabPanelItemColorTable.OuterBorder;
		_0657.BorderColorLight = superTabPanelItemColorTable.InnerBorder;
	}

	private void ὂ(ElementStyle _0657)
	{
		if (TabsVisible)
		{
			_0657.BorderWidth = 0;
			if (!_0657.BorderColor.IsEmpty)
			{
				_0657.BorderWidth++;
			}
			if (_0657.BorderWidth == 0)
			{
				_0657.Border = eStyleBorderType.None;
			}
			else if (_0657.BorderColorLight.IsEmpty)
			{
				_0657.Border = eStyleBorderType.Solid;
			}
			else
			{
				_0657.Border = eStyleBorderType.Double;
			}
			if (_Ꮺ._2052)
			{
				switch (_Ꮺ.TabAlignment)
				{
				case eTabStripAlignment.Top:
					_0657.BorderTop = eStyleBorderType.None;
					break;
				case eTabStripAlignment.Bottom:
					_0657.BorderBottom = eStyleBorderType.None;
					break;
				case eTabStripAlignment.Left:
					_0657.BorderLeft = eStyleBorderType.None;
					break;
				case eTabStripAlignment.Right:
					_0657.BorderRight = eStyleBorderType.None;
					break;
				}
			}
		}
		else
		{
			_0657.Border = eStyleBorderType.None;
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		ᖐ();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		BarUtilities.InvokeDelayed((MethodInvoker)delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Invalid comparison between Unknown and I4
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Invalid comparison between Unknown and I4
			if ((int)((Control)_Ꮺ).Dock == 1 || (int)((Control)_Ꮺ).Dock == 2)
			{
				((Control)_Ꮺ).Width = ((Control)this).ClientRectangle.Width;
			}
			else if ((int)((Control)_Ꮺ).Dock == 3 || (int)((Control)_Ꮺ).Dock == 4)
			{
				((Control)_Ꮺ).Height = ((Control)this).ClientRectangle.Height;
			}
		}, 100);
		((Control)this).OnHandleCreated(e);
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

	private void ህ()
	{
		_ᠭ = true;
	}

	void ISupportInitialize.BeginInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ህ
		this.ህ();
	}

	private void ሆ()
	{
		_ᠭ = false;
		ᖐ();
		SuperTabItem superTabItem = _Ꮺ.TabStripItem.ᾩ(_ἤ);
		if (superTabItem != null)
		{
			SelectedTab = superTabItem;
			if (_Ꮺ.SelectedTab != null && _Ꮺ.SelectedTab.AttachedControl != null)
			{
				_Ꮺ.SelectedTab.AttachedControl.Visible = true;
				_Ꮺ.SelectedTab.AttachedControl.BringToFront();
			}
		}
		if (((Component)this).DesignMode && SelectedTab != null)
		{
			foreach (BaseItem tab in _Ꮺ.Tabs)
			{
				if (tab is SuperTabItem superTabItem2 && superTabItem2 != SelectedTab && superTabItem2.AttachedControl != null)
				{
					superTabItem2.AttachedControl.Visible = false;
				}
			}
		}
		_ἤ = -1;
	}

	void ISupportInitialize.EndInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ሆ
		this.ሆ();
	}

	[CompilerGenerated]
	private void ᖒ()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		if ((int)((Control)_Ꮺ).Dock == 1 || (int)((Control)_Ꮺ).Dock == 2)
		{
			((Control)_Ꮺ).Width = ((Control)this).ClientRectangle.Width;
		}
		else if ((int)((Control)_Ꮺ).Dock == 3 || (int)((Control)_Ꮺ).Dock == 4)
		{
			((Control)_Ꮺ).Height = ((Control)this).ClientRectangle.Height;
		}
	}
}
