using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class SuperTabStripItem : BaseItem, IDesignTimeProvider
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

	private EventHandler<SuperTabStripTabColorChangedEventArgs> Ἰ;

	private EventHandler<EventArgs> ὶ;

	private eSuperTabStyle _ί;

	private eSuperTabLayoutType _ὸ;

	private Image _ό;

	private Image _ὺ;

	private Image _ύ;

	private Size _ὼ = new Size(16, 16);

	private Size _ώ = new Size(16, 16);

	private int __1F7E = 5;

	private int __1F7F = 4;

	private bool _ᾀ;

	private bool _ᾁ = true;

	private bool _ᾂ;

	private bool _ᾃ;

	private Size __1F5C = Size.Empty;

	private bool _ᾄ = true;

	private bool _ᾅ = true;

	private bool _ᾆ;

	private bool _ᾇ = true;

	private bool _ᾈ;

	private Font _ὒ;

	private Font _ὓ;

	private SuperTabColorTable _ᾉ = new SuperTabColorTable();

	private SuperTabItem _ᾊ;

	private SuperTabItem _ᾋ;

	private BaseItem _ᾌ;

	private int _ᾍ;

	private SuperTabControlBox _ᾎ;

	private ῲ _ᾏ;

	private Rectangle _ᾐ;

	private SuperTabItem _ᾑ;

	private eTabStripAlignment _Ე = eTabStripAlignment.Top;

	private eTabCloseButtonPosition _ᾒ = eTabCloseButtonPosition.Right;

	private SuperTabStrip _Ꮺ;

	private int _ᾓ;

	private eItemAlignment _ᬭ;

	private eSuperTabArea _ᾔ;

	private SuperTabItem _ᾕ;

	private Point _ᾖ;

	private bool _ᾗ;

	private SuperTabDragWindow _ᾘ;

	private BaseItem _ᾙ;

	private bool _ᾚ;

	private bool _ᾛ;

	private Padding _ᾜ;

	private ImageAlignment __1712 = ImageAlignment.NotSet;

	public bool AutoCloseTabs
	{
		get
		{
			return _ᾅ;
		}
		set
		{
			_ᾅ = value;
		}
	}

	public bool AutoSelectAttachedControl
	{
		get
		{
			return _ᾆ;
		}
		set
		{
			_ᾆ = value;
		}
	}

	public Image TabCloseButtonNormal
	{
		get
		{
			return _ό;
		}
		set
		{
			_ό = value;
			_ὼ = ((_ό != null) ? _ό.Size : _ώ);
			ᾦ();
		}
	}

	public Image TabCloseButtonHot
	{
		get
		{
			return _ὺ;
		}
		set
		{
			_ὺ = value;
			ᾦ();
		}
	}

	public Image TabCloseButtonPressed
	{
		get
		{
			return _ύ;
		}
		set
		{
			_ύ = value;
			ᾦ();
		}
	}

	public int TabHorizontalSpacing
	{
		get
		{
			return __1F7E;
		}
		set
		{
			if (__1F7E != value)
			{
				__1F7E = value;
				ᾦ();
			}
		}
	}

	public int TabVerticalSpacing
	{
		get
		{
			return __1F7F;
		}
		set
		{
			if (__1F7F != value)
			{
				__1F7F = value;
				ᾦ();
			}
		}
	}

	public bool CloseButtonOnTabsVisible
	{
		get
		{
			return _ᾀ;
		}
		set
		{
			if (_ᾀ != value)
			{
				_ᾀ = value;
				ᾦ();
			}
		}
	}

	public bool CloseButtonOnTabsAlwaysDisplayed
	{
		get
		{
			return _ᾁ;
		}
		set
		{
			if (_ᾁ != value)
			{
				_ᾁ = value;
				ᾦ();
			}
		}
	}

	public eTabCloseButtonPosition CloseButtonPosition
	{
		get
		{
			return _ᾒ;
		}
		set
		{
			_ᾒ = value;
			ᾦ();
		}
	}

	public SuperTabControlBox ControlBox => _ᾎ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public SuperTabItem DesignTimeSelection
	{
		get
		{
			return _ᾑ;
		}
		set
		{
			_ᾑ = value;
			Refresh();
		}
	}

	public bool DisplaySelectedTextOnly
	{
		get
		{
			return _ᾂ;
		}
		set
		{
			_ᾂ = value;
			ᾦ();
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
			return _ὒ;
		}
		set
		{
			_ὒ = value;
			ᾝ();
			ᾦ();
		}
	}

	public Font SelectedTabFont
	{
		get
		{
			return _ὓ;
		}
		set
		{
			_ὓ = value;
			ᾝ();
			ᾦ();
		}
	}

	public bool HorizontalText
	{
		get
		{
			return _ᾄ;
		}
		set
		{
			_ᾄ = value;
			ᾦ();
		}
	}

	[Description("Indicates the default alignment of the Image within the tab.")]
	[DefaultValue(ImageAlignment.NotSet)]
	[Category("Appearance")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return __1712;
		}
		set
		{
			if (__1712 != value)
			{
				__1712 = value;
				ᾦ();
			}
		}
	}

	public Padding ItemPadding
	{
		get
		{
			return _ᾜ;
		}
		set
		{
			_ᾜ = value;
		}
	}

	public bool RotateVerticalText
	{
		get
		{
			return _ᾈ;
		}
		set
		{
			_ᾈ = value;
			ᾦ();
		}
	}

	public bool ShowFocusRectangle
	{
		get
		{
			return _ᾃ;
		}
		set
		{
			_ᾃ = value;
		}
	}

	public Size FixedTabSize
	{
		get
		{
			return __1F5C;
		}
		set
		{
			value.Width = Math.Max(0, value.Width);
			value.Height = Math.Max(0, value.Height);
			if ((value.Width > 0 || value.Height > 0) && _ᾏ != null)
			{
				Size ό = _ᾏ.Ό;
				if (value.Width > 0 && ό.Width > value.Width)
				{
					value.Width = ό.Width;
				}
				if (value.Height > 0 && ό.Height > value.Height)
				{
					value.Height = ό.Height;
				}
			}
			if (__1F5C != value)
			{
				__1F5C = value;
				ᾦ();
			}
		}
	}

	public bool IsTabDragging
	{
		get
		{
			return _ᾗ;
		}
		internal set
		{
			_ᾗ = value;
		}
	}

	public int MinTabStripHeight => _ᾏ._1FF5;

	public bool ReorderTabsEnabled
	{
		get
		{
			return _ᾇ;
		}
		set
		{
			_ᾇ = value;
		}
	}

	public SuperTabItem SelectedTab
	{
		get
		{
			return _ᾋ;
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
			if (_ᾋ != null)
			{
				return SubItems.IndexOf(_ᾋ);
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
			return _Ე;
		}
		set
		{
			if (_Ე != value)
			{
				_Ე = value;
				ᾞ();
				ᾦ();
			}
		}
	}

	public eSuperTabLayoutType TabLayoutType
	{
		get
		{
			return _ὸ;
		}
		set
		{
			_ὸ = value;
			_ᾎ.Bounds = Rectangle.Empty;
			ᕚ();
			ᾦ();
		}
	}

	public SuperTabColorTable TabStripColor
	{
		get
		{
			return _ᾉ;
		}
		set
		{
			if (!_ᾉ.Equals(value))
			{
				if (_ᾉ != null)
				{
					_ᾉ.ColorTableChanged -= Ὗ;
				}
				_ᾉ = value;
				if (value != null)
				{
					_ᾉ.ColorTableChanged += Ὗ;
				}
				Ὰ();
				ᾦ();
			}
		}
	}

	public eSuperTabStyle TabStyle
	{
		get
		{
			return _ί;
		}
		set
		{
			if (_ί != value)
			{
				_ί = value;
				switch (value)
				{
				case eSuperTabStyle.Office2007:
					_ᾏ = new _2082(this);
					break;
				case eSuperTabStyle.Office2010BackstageBlue:
					_ᾏ = new ₕ(this);
					break;
				case eSuperTabStyle.OneNote2007:
					_ᾏ = new ₘ(this);
					break;
				case eSuperTabStyle.OfficeMobile2014:
					_ᾏ = new _209D(this);
					break;
				case eSuperTabStyle.VisualStudio2008Dock:
					_ᾏ = new _20A1(this);
					break;
				case eSuperTabStyle.VisualStudio2008Document:
					_ᾏ = new _20A2(this);
					break;
				case eSuperTabStyle.WinMediaPlayer12:
					_ᾏ = new _20A4(this);
					break;
				}
				ᾠ();
			}
		}
	}

	public eItemAlignment TextAlignment
	{
		get
		{
			return _ᬭ;
		}
		set
		{
			if (_ᬭ != value)
			{
				_ᬭ = value;
				Refresh();
			}
		}
	}

	internal SuperTabItem Ὦ
	{
		get
		{
			for (int i = 0; i < SubItems.Count; i++)
			{
				BaseItem baseItem = SubItems[i];
				if (baseItem is SuperTabItem && baseItem.Visible && (baseItem.Enabled || DesignMode))
				{
					return (SuperTabItem)baseItem;
				}
			}
			return null;
		}
	}

	internal SuperTabItem Ὧ => _ᾊ;

	internal BaseItem ὰ => _ᾙ;

	internal bool ড => _ᾚ;

	internal SuperTabItem ᓤ => _ᾊ;

	internal bool ᓙ
	{
		get
		{
			if (_Ე != 0)
			{
				return _Ე == eTabStripAlignment.Right;
			}
			return true;
		}
	}

	internal SuperTabItem ά
	{
		get
		{
			for (int num = SubItems.Count - 1; num >= 0; num--)
			{
				BaseItem baseItem = SubItems[num];
				if (baseItem is SuperTabItem && baseItem.Visible && (baseItem.Enabled || DesignMode))
				{
					return (SuperTabItem)baseItem;
				}
			}
			return null;
		}
	}

	internal Size ὲ => _ὼ;

	internal ῲ έ => _ᾏ;

	internal Rectangle ᓚ
	{
		get
		{
			return _ᾐ;
		}
		set
		{
			_ᾐ = value;
		}
	}

	internal int ὴ
	{
		get
		{
			return _ᾓ;
		}
		set
		{
			_ᾓ = value;
		}
	}

	internal SuperTabStrip Ὁ => _Ꮺ;

	internal int ᓥ
	{
		get
		{
			int num = 0;
			for (int i = 0; i < SubItems.Count; i++)
			{
				if (SubItems[i].Parent != null && SubItems[i].Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	internal BaseItem ή
	{
		get
		{
			return _ᾌ ?? _ᾋ;
		}
		set
		{
			if (_ᾌ != value)
			{
				_ᾌ = value;
				ᾦ();
			}
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

	[Description("Occurs when the TabStrip TabColor has changed.")]
	public event EventHandler<EventArgs> TabStripColorChanged
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ὶ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ὶ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ὶ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ὶ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SuperTabStripItem(SuperTabStrip tabStrip)
	{
		_Ꮺ = tabStrip;
		_ᾎ = new SuperTabControlBox(this);
		_ᾏ = new _2082(this);
		_ᾜ = new Padding(0, 0, 0, 0);
		SetIsContainer(b: true);
		ἧ(Ἠ: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.ᆟ(ref _ό);
			BarUtilities.ᆟ(ref _ὺ);
			BarUtilities.ᆟ(ref _ύ);
		}
		if (disposing)
		{
			_ᾎ.Dispose();
		}
		base.Dispose(disposing);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabFont()
	{
		return _ὒ != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		TabFont = null;
	}

	private void ᾝ()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SuperTabItem superTabItem)
			{
				superTabItem._1F47 = null;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeSelectedTabFont()
	{
		return _ὓ != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		SelectedTabFont = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return !__1F5C.IsEmpty;
	}

	private void ᾞ()
	{
		eDesignMarkerOrientation designMarkerOrientation = ᾟ();
		foreach (BaseItem subItem in SubItems)
		{
			subItem.DesignMarkerOrientation = designMarkerOrientation;
		}
	}

	private eDesignMarkerOrientation ᾟ()
	{
		if (!ᓙ)
		{
			return eDesignMarkerOrientation.Horizontal;
		}
		return eDesignMarkerOrientation.Vertical;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		item.DesignMarkerOrientation = ᾟ();
		item.Style = ᾡ();
		base.OnItemAdded(item);
	}

	private void ᾠ()
	{
		eDotNetBarStyle style = ᾡ();
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SuperTabItem superTabItem)
			{
				superTabItem.TabStyle = _ί;
			}
			subItem.Style = style;
		}
		Ὁ.RecalcLayout();
	}

	private eDotNetBarStyle ᾡ()
	{
		eDotNetBarStyle result = eDotNetBarStyle.StyleManagerControlled;
		eSuperTabStyle ί = _ί;
		if (ί == eSuperTabStyle.VisualStudio2008Dock || ί == eSuperTabStyle.VisualStudio2008Document)
		{
			result = eDotNetBarStyle.VS2005;
		}
		return result;
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			base.SubItemsChanged += ᾢ;
		}
		else
		{
			base.SubItemsChanged -= ᾢ;
		}
	}

	private void ᾢ(object _٠, CollectionChangeEventArgs ؿ)
	{
		switch (ؿ.Action)
		{
		case CollectionChangeAction.Add:
			ᾣ(ؿ.Element as BaseItem);
			break;
		case CollectionChangeAction.Remove:
			ᾤ(ؿ.Element as BaseItem);
			break;
		case CollectionChangeAction.Refresh:
			ᾦ();
			break;
		}
	}

	internal void ᾣ(BaseItem _06EC)
	{
		if (_06EC is SuperTabItem superTabItem)
		{
			superTabItem.TabStripItem = this;
			superTabItem.TabColorChanged += ᾥ;
			if (ᓮ != null)
			{
				SuperTabStripTabItemOpenEventArgs e = new SuperTabStripTabItemOpenEventArgs(superTabItem);
				ᓮ(this, e);
			}
			if (superTabItem.Visible && SelectedTab == null && ᓥ == 1)
			{
				SelectedTab = superTabItem;
			}
		}
		ᾦ();
	}

	internal void ᾤ(BaseItem _06EC)
	{
		if (ᓯ != null)
		{
			SuperTabStripTabRemovedEventArgs e = new SuperTabStripTabRemovedEventArgs(_06EC);
			ᓯ(this, e);
		}
		if (_06EC is SuperTabItem superTabItem)
		{
			superTabItem.TabColorChanged -= ᾥ;
			if (superTabItem.AttachedControl is SuperTabControlPanel superTabControlPanel)
			{
				superTabControlPanel.PanelColorChanged -= ᾥ;
			}
			if (SelectedTab == superTabItem)
			{
				ᕐ(_ᾍ, superTabItem);
			}
			if (ᓥ == 0)
			{
				SelectedTab = null;
			}
		}
		Refresh();
	}

	private void ᾥ(object _٠, EventArgs ؿ)
	{
		if (Ἰ != null && _٠ is SuperTabItem tab)
		{
			Ἰ(this, new SuperTabStripTabColorChangedEventArgs(tab));
		}
		Refresh();
	}

	private void Ὗ(object _٠, EventArgs ؿ)
	{
		Ὰ();
	}

	private void ᾦ()
	{
		NeedRecalcSize = true;
		Refresh();
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout && BarFunctions.IsHandleValid((Control)/*isinst with value type is only supported in some contexts*/) && !_ᾛ)
		{
			try
			{
				_ᾛ = true;
				ᾧ();
			}
			finally
			{
				_ᾛ = false;
			}
		}
	}

	private void ᾧ()
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
			if (_ᾎ.Visible)
			{
				_ᾎ.RecalcSize();
				if (!DesignMode)
				{
					_ᾎ.Displayed = true;
				}
			}
			_ᾏ._0BE5(val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public void EnsureVisible(BaseItem item)
	{
		ή = item;
	}

	public void SelectTab(SuperTabItem tab, eEventSource eventSource)
	{
		if (tab != null && (tab.Enabled || DesignMode))
		{
			bool flag = _ᾌ != tab || _ᾋ != tab;
			if (_ᾋ != tab)
			{
				SuperTabControl superTabControl = ((Control)_Ꮺ).Parent as SuperTabControl;
				if (superTabControl != null && superTabControl.ValidateFormOnTabSelection)
				{
					Form val = ((Control)superTabControl).FindForm();
					if (val != null && !((ContainerControl)val).Validate())
					{
						return;
					}
				}
				SuperTabItem ᾃ = _ᾋ;
				if (!ᾴ(tab, eventSource))
				{
					_ᾌ = tab;
					Ᾰ(tab);
					Control val2 = null;
					if (_ᾋ != null && _ᾋ.AttachedControl != null)
					{
						val2 = _ᾋ.AttachedControl;
					}
					_ᾋ = tab;
					_ᾍ = SubItems.IndexOf(tab);
					if (_ᾋ.AttachedControl != null)
					{
						_ᾋ.AttachedControl.BringToFront();
						_ᾋ.AttachedControl.Visible = true;
						if (superTabControl != null)
						{
							bool flag2 = !((Control)superTabControl).TabStop || !((Control)superTabControl).Focused || eventSource != eEventSource.Keyboard;
							if (_ᾆ && flag2)
							{
								_ᾋ.AttachedControl.Select();
							}
						}
						if (val2 == _ᾋ.AttachedControl)
						{
							val2 = null;
						}
					}
					if (val2 != null)
					{
						val2.Visible = false;
					}
					_1FB5(ᾃ, _ᾋ, eventSource);
				}
			}
			if (flag)
			{
				ᾦ();
			}
		}
		else
		{
			_ᾋ = null;
			ᾦ();
		}
	}

	public bool SelectPreviousTab()
	{
		return _౯(eEventSource.Code);
	}

	internal bool _౯(eEventSource ಥ)
	{
		if (SelectedTab != null && SubItems.IndexOf(SelectedTab) > 0)
		{
			for (int num = SubItems.IndexOf(SelectedTab) - 1; num >= 0; num--)
			{
				BaseItem baseItem = SubItems[num];
				if (baseItem.Visible && (baseItem.Enabled || DesignMode) && baseItem is SuperTabItem)
				{
					SelectTab((SuperTabItem)baseItem, ಥ);
					return true;
				}
			}
		}
		return false;
	}

	public bool SelectNextTab()
	{
		return _0C71(eEventSource.Code);
	}

	internal bool _0C71(eEventSource ಥ)
	{
		if (SelectedTab != null && SubItems.IndexOf(SelectedTab) < SubItems.Count)
		{
			for (int i = SubItems.IndexOf(SelectedTab) + 1; i < SubItems.Count; i++)
			{
				BaseItem baseItem = SubItems[i];
				if (baseItem.Visible && (baseItem.Enabled || DesignMode) && baseItem is SuperTabItem)
				{
					SelectTab((SuperTabItem)baseItem, ಥ);
					return true;
				}
			}
		}
		return false;
	}

	internal void ᾨ(BaseItem _06EC)
	{
		ᾨ(Math.Max(0, SubItems.IndexOf(_06EC)));
	}

	internal void ᾨ(int ԡ)
	{
		SubItemsCollection subItems = SubItems;
		int num = ᾪ(subItems, ԡ);
		if (num < 0)
		{
			num = ᾫ(subItems, ԡ);
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

	internal SuperTabItem ᾩ(int ԡ)
	{
		SubItemsCollection subItems = SubItems;
		if ((uint)ԡ < subItems.Count)
		{
			int num = ᾪ(subItems, ԡ);
			if (num < 0)
			{
				num = ᾫ(subItems, ԡ);
			}
			if (num >= 0)
			{
				return subItems[num] as SuperTabItem;
			}
		}
		return null;
	}

	internal int ᾪ(SuperTabItem ᆰ)
	{
		SubItemsCollection subItems = SubItems;
		return ᾪ(subItems, subItems.IndexOf(ᆰ));
	}

	internal int ᾪ(SubItemsCollection ڴ, int _0F0C)
	{
		_0F0C = Math.Max(_0F0C, 0);
		for (int i = _0F0C; i < ڴ.Count; i++)
		{
			if (ڴ[i] is SuperTabItem { Parent: not null, Visible: not false, Enabled: not false })
			{
				return i;
			}
		}
		return -1;
	}

	internal int ᾫ(SuperTabItem ᆰ)
	{
		SubItemsCollection subItems = SubItems;
		return ᾫ(subItems, subItems.IndexOf(ᆰ));
	}

	internal int ᾫ(SubItemsCollection ڴ, int _0F0C)
	{
		_0F0C = Math.Min(_0F0C, ڴ.Count - 1);
		for (int num = _0F0C; num >= 0; num--)
		{
			if (ڴ[num] is SuperTabItem { Parent: not null, Visible: not false, Enabled: not false })
			{
				return num;
			}
		}
		return -1;
	}

	internal int ᾬ(SubItemsCollection ڴ, int _0F0C)
	{
		_0F0C = Math.Max(_0F0C, 0);
		for (int i = _0F0C; i < ڴ.Count; i++)
		{
			if (ڴ[i] is SuperTabItem { Parent: not null, Visible: not false })
			{
				return i;
			}
		}
		return -1;
	}

	internal int ᾭ(SubItemsCollection ڴ, int _0F0C)
	{
		_0F0C = Math.Min(_0F0C, ڴ.Count - 1);
		for (int num = _0F0C; num >= 0; num--)
		{
			if (ڴ[num] is SuperTabItem { Parent: not null, Visible: not false })
			{
				return num;
			}
		}
		return -1;
	}

	internal void ᾮ()
	{
		if (!DesignMode)
		{
			ᾨ(SelectedTab);
		}
		RecalcSize();
	}

	private void ᕚ()
	{
		ᾦ();
		EnsureVisible(SelectedTab);
	}

	internal Size ᾯ(SuperTabItem ᆰ, Size ߒ, Graphics _٥)
	{
		if (ᓱ != null)
		{
			SuperTabMeasureTabItemEventArgs superTabMeasureTabItemEventArgs = new SuperTabMeasureTabItemEventArgs(ᆰ, ߒ, _٥);
			ᓱ(this, superTabMeasureTabItemEventArgs);
			return superTabMeasureTabItemEventArgs.Size;
		}
		return ߒ;
	}

	internal bool ᾰ(SuperTabItem ᆰ, Graphics _٥)
	{
		if (ᓲ != null)
		{
			SuperTabPreRenderTabItemEventArgs superTabPreRenderTabItemEventArgs = new SuperTabPreRenderTabItemEventArgs(ᆰ, _٥);
			ᓲ(this, superTabPreRenderTabItemEventArgs);
			return superTabPreRenderTabItemEventArgs.Cancel;
		}
		return false;
	}

	internal void ᾱ(SuperTabItem ᆰ, Graphics _٥)
	{
		if (ᓳ != null)
		{
			SuperTabPostRenderTabItemEventArgs e = new SuperTabPostRenderTabItemEventArgs(ᆰ, _٥);
			ᓳ(this, e);
		}
	}

	internal Rectangle ᾲ(SuperTabItem ᆰ)
	{
		if (_1F16 != null)
		{
			SuperTabGetTabItemContentRectangleEventArgs superTabGetTabItemContentRectangleEventArgs = new SuperTabGetTabItemContentRectangleEventArgs(ᆰ);
			_1F16(this, superTabGetTabItemContentRectangleEventArgs);
			return superTabGetTabItemContentRectangleEventArgs.ContentRectangle;
		}
		return ᆰ.Ὀ._1FDC();
	}

	internal GraphicsPath ᾳ(SuperTabItem ᆰ)
	{
		if (ἕ != null)
		{
			SuperTabGetTabItemPathEventArgs superTabGetTabItemPathEventArgs = new SuperTabGetTabItemPathEventArgs(ᆰ);
			ἕ(this, superTabGetTabItemPathEventArgs);
			return superTabGetTabItemPathEventArgs.Path;
		}
		return null;
	}

	internal void ᕐ(int ԡ, SuperTabItem _06EC)
	{
		ᾨ(ԡ);
	}

	private bool ᾴ(SuperTabItem ԝ, eEventSource ಥ)
	{
		if (ᓩ != null)
		{
			SuperTabStripSelectedTabChangingEventArgs superTabStripSelectedTabChangingEventArgs = new SuperTabStripSelectedTabChangingEventArgs(_ᾋ, ԝ, ಥ);
			ᓩ(this, superTabStripSelectedTabChangingEventArgs);
			return superTabStripSelectedTabChangingEventArgs.Cancel;
		}
		return false;
	}

	private void _1FB5(SuperTabItem ᾶ, SuperTabItem ᾷ, eEventSource ಥ)
	{
		if (Ꮷ != null)
		{
			SuperTabStripSelectedTabChangedEventArgs e = new SuperTabStripSelectedTabChangedEventArgs(ᾶ, ᾷ, ಥ);
			Ꮷ(this, e);
		}
	}

	internal void Ᾰ(SuperTabItem _06EC)
	{
		if (_0C55 != null)
		{
			SuperTabStripBeforeTabDisplayEventArgs e = new SuperTabStripBeforeTabDisplayEventArgs(_06EC);
			_0C55(this, e);
		}
	}

	private bool Ᾱ(SuperTabItem ᆰ)
	{
		if (ᓭ != null)
		{
			SuperTabStripTabItemCloseEventArgs superTabStripTabItemCloseEventArgs = new SuperTabStripTabItemCloseEventArgs(ᆰ);
			superTabStripTabItemCloseEventArgs.Cancel = !AutoCloseTabs;
			ᓭ(this, superTabStripTabItemCloseEventArgs);
			return superTabStripTabItemCloseEventArgs.Cancel;
		}
		return !AutoCloseTabs;
	}

	private void Ὰ()
	{
		if (ὶ != null)
		{
			ὶ(this, EventArgs.Empty);
		}
		ᾦ();
	}

	internal void Ά(Graphics _٥, SuperTabColorTable ዅ)
	{
		if (Ἒ != null)
		{
			Ἒ(this, new SuperTabStripPaintBackgroundEventArgs(_٥, ዅ));
		}
	}

	internal Rectangle ᾼ(SuperTabItem ᆰ, Rectangle ശ)
	{
		if (_1F17 != null)
		{
			SuperTabGetTabTextBoundsEventArgs superTabGetTabTextBoundsEventArgs = new SuperTabGetTabTextBoundsEventArgs(ᆰ, ശ);
			_1F17(this, superTabGetTabTextBoundsEventArgs);
			ശ = superTabGetTabTextBoundsEventArgs.Bounds;
		}
		return ശ;
	}

	internal Rectangle _1FBD(SuperTabItem ᆰ, Rectangle ശ)
	{
		if (Ἐ != null)
		{
			SuperTabGetTabImageBoundsEventArgs superTabGetTabImageBoundsEventArgs = new SuperTabGetTabImageBoundsEventArgs(ᆰ, ശ);
			Ἐ(this, superTabGetTabImageBoundsEventArgs);
			ശ = superTabGetTabImageBoundsEventArgs.Bounds;
		}
		return ശ;
	}

	internal Rectangle ι(SuperTabItem ᆰ, Rectangle ശ)
	{
		if (Ἑ != null)
		{
			SuperTabGetTabCloseBoundsEventArgs superTabGetTabCloseBoundsEventArgs = new SuperTabGetTabCloseBoundsEventArgs(ᆰ, ശ);
			Ἑ(this, superTabGetTabCloseBoundsEventArgs);
			ശ = superTabGetTabCloseBoundsEventArgs.Bounds;
		}
		return ശ;
	}

	protected bool OnTabMoving(BaseItem moveTab, BaseItem insertTab, bool insertBefore)
	{
		if (ἔ != null)
		{
			SuperTabStripTabMovingEventArgs superTabStripTabMovingEventArgs = new SuperTabStripTabMovingEventArgs(moveTab, insertTab, insertBefore);
			ἔ(this, superTabStripTabMovingEventArgs);
			return superTabStripTabMovingEventArgs.CanMove;
		}
		return true;
	}

	protected bool OnTabMoved(SuperTabItem tab, List<BaseItem> oldOrder, ref List<BaseItem> newOrder)
	{
		if (ᓪ != null)
		{
			SuperTabStripTabMovedEventArgs superTabStripTabMovedEventArgs = new SuperTabStripTabMovedEventArgs(tab, oldOrder, newOrder);
			ᓪ(this, superTabStripTabMovedEventArgs);
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
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		HideToolTip();
		_ᾔ = eSuperTabArea.InNone;
		SuperTabItem superTabItem = (_ᾕ = GetTabFromPoint(objArg.Location));
		_ᾖ = objArg.Location;
		if (superTabItem != null)
		{
			if ((int)objArg.Button != 1048576)
			{
				return;
			}
			if (superTabItem.Enabled || DesignMode)
			{
				_ᾔ = superTabItem.GetTabAreaFromPoint(objArg.Location);
				if (_ᾔ == eSuperTabArea.InContent || _ᾔ == eSuperTabArea.InImage)
				{
					((Control)superTabItem.Ὁ).Focus();
					if (_ᾋ != superTabItem)
					{
						SelectTab(superTabItem, eEventSource.Mouse);
					}
				}
			}
			else
			{
				_ᾕ = null;
				SystemSounds.Beep.Play();
			}
		}
		else if (ControlBox.Visible && ControlBox.ᆼ)
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
		if (!_ᾗ)
		{
			ᡛ(objArg);
		}
		else
		{
			_1FBF();
		}
		_ᾙ = null;
		_ᾕ = null;
		_ᾗ = false;
	}

	private void ᡛ(MouseEventArgs ᨍ)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		SuperTabItem tabFromPoint = GetTabFromPoint(ᨍ.Location);
		if (tabFromPoint != null)
		{
			if ((int)ᨍ.Button == 1048576)
			{
				eSuperTabArea tabAreaFromPoint = tabFromPoint.GetTabAreaFromPoint(ᨍ.Location);
				if (_ᾔ == eSuperTabArea.InCloseBox && tabAreaFromPoint == eSuperTabArea.InCloseBox)
				{
					CloseTab(tabFromPoint);
				}
			}
		}
		else if (ControlBox.Visible && ControlBox._206D)
		{
			ControlBox.InternalMouseUp(ᨍ);
		}
	}

	private void _1FBF()
	{
		((Control)((Form)_ᾘ).Owner).Cursor = Cursors.Default;
		((Component)(object)_ᾘ).Dispose();
		_ᾘ = null;
		if (_ᾙ != null)
		{
			SuperTabItem ᾂ = _ᾊ;
			List<BaseItem> oldOrder = _1FC0();
			List<BaseItem> newOrder = _1FC1();
			if (!OnTabMoved(ᾂ, oldOrder, ref newOrder))
			{
				SubItems.Clear();
				foreach (BaseItem item in newOrder)
				{
					bool visible = item.Visible;
					SubItems.Add(item);
					if (!visible)
					{
						item.Visible = false;
					}
				}
				SelectedTab = ᾂ;
			}
		}
		Refresh();
	}

	private List<BaseItem> _1FC0()
	{
		List<BaseItem> list = new List<BaseItem>(SubItems.Count);
		foreach (BaseItem subItem in SubItems)
		{
			list.Add(subItem);
		}
		return list;
	}

	private List<BaseItem> _1FC1()
	{
		List<BaseItem> list = _1FC0();
		int num = list.IndexOf(_ᾙ);
		int num2 = list.IndexOf(_ᾊ);
		if (num >= 0 && num2 >= 0 && ((!_ᾚ && num + 1 != num2) || (_ᾚ && num - 1 != num2)))
		{
			list.Remove(_ᾊ);
			num = list.IndexOf(_ᾙ);
			if (!_ᾚ)
			{
				num++;
			}
			list.Insert(num, _ᾊ);
		}
		return list;
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (!_ᾗ)
		{
			base.InternalMouseMove(objArg);
		}
		InternalOnMouseMove(objArg);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalOnMouseMove(MouseEventArgs objArg)
	{
		ῂ();
		if (!_ᾗ)
		{
			ᡘ(objArg);
		}
		else
		{
			_1FC5(objArg);
		}
	}

	private void ῂ()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (_ᾕ != null && (int)Control.MouseButtons == 0)
		{
			if (_ᾘ != null)
			{
				((Component)(object)_ᾘ).Dispose();
				_ᾘ = null;
			}
			_ᾙ = null;
			_ᾕ = null;
			_ᾗ = false;
		}
	}

	private void ᡘ(MouseEventArgs ᨍ)
	{
		SuperTabItem tabFromPoint = GetTabFromPoint(ᨍ.Location);
		if (_ᾊ != tabFromPoint || _ᾊ != null)
		{
			ῃ(ᨍ, tabFromPoint);
		}
		else
		{
			ῄ(ᨍ);
		}
	}

	private void ῃ(MouseEventArgs ᨍ, SuperTabItem ᆰ)
	{
		if (_ᾊ != ᆰ)
		{
			if (_ᾊ != null)
			{
				_ᾊ.Refresh();
			}
			if (_ᾊ == null || !_ᾊ.ὄ)
			{
				_ᾊ = ᆰ;
			}
			if (_ᾊ != null)
			{
				_ᾊ.Refresh();
			}
			ResetHover();
			HideToolTip();
		}
		else if (_ᾊ != null)
		{
			ῆ(ᨍ);
		}
	}

	private void ῄ(MouseEventArgs ᨍ)
	{
		if (!ControlBox.Visible)
		{
			return;
		}
		if (ControlBox.Bounds.Contains(ᨍ.Location))
		{
			if (!ControlBox.ᆼ)
			{
				ControlBox.InternalMouseEnter();
			}
			ControlBox.InternalMouseMove(ᨍ);
		}
		else if (ControlBox.ᆼ)
		{
			ControlBox.InternalMouseLeave();
		}
	}

	private void _1FC5(MouseEventArgs ᨍ)
	{
		((Form)_ᾘ).Location = new Point(Control.MousePosition.X, Control.MousePosition.Y + ((Control)((Form)_ᾘ).Owner).Cursor.Size.Height / 2);
		_ᾙ = null;
		BaseItem itemFromPoint = GetItemFromPoint(ᨍ.Location);
		if (itemFromPoint != null && _ᾊ != null)
		{
			Point location = ᨍ.Location;
			switch (_Ꮺ.TabAlignment)
			{
			case eTabStripAlignment.Top:
			case eTabStripAlignment.Bottom:
				_ᾚ = location.X < itemFromPoint.Bounds.X + itemFromPoint.Bounds.Width / 2;
				break;
			default:
				_ᾚ = location.Y < itemFromPoint.Bounds.Y + itemFromPoint.Bounds.Height / 2;
				break;
			}
			int num = SubItems.IndexOf(itemFromPoint);
			int num2 = SubItems.IndexOf(_ᾊ);
			if (num2 != num && ((_ᾚ && num - 1 != num2) || (!_ᾚ && num + 1 != num2)))
			{
				_ᾙ = itemFromPoint;
			}
		}
		if (!OnTabMoving(_ᾊ, _ᾙ, _ᾚ))
		{
			_ᾙ = null;
		}
		((Control)((Form)_ᾘ).Owner).Cursor = ((_ᾙ != null) ? Cursors.Hand : Cursors.No);
		Refresh();
	}

	private bool ῆ(MouseEventArgs ᨍ)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		if (_ᾇ && _ᾊ == _ᾕ && !_ᾕ.ὄ && (Math.Abs(_ᾖ.X - ᨍ.Location.X) > 8 || Math.Abs(_ᾖ.Y - ᨍ.Location.Y) > 8))
		{
			_ᾗ = true;
			_ᾘ = new SuperTabDragWindow();
			((Form)_ᾘ).Owner = ((Control)Ὁ).FindForm();
			((Control)_ᾘ).Paint += new PaintEventHandler(Ὲ);
			ῇ();
			((Control)_ᾘ).Show();
			((Form)_ᾘ).Size = _ᾕ.Bounds.Size;
			return true;
		}
		return false;
	}

	private void ῇ()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		Rectangle bounds = _ᾕ.Bounds;
		Rectangle bounds2 = bounds;
		bounds2.Location = new Point(0, 0);
		try
		{
			_ᾕ.Bounds = bounds2;
			GraphicsPath tabItemPath = _ᾕ.GetTabItemPath();
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
				((Control)_ᾘ).Region = val3;
			}
			finally
			{
				((IDisposable)tabItemPath)?.Dispose();
			}
		}
		finally
		{
			_ᾕ.Bounds = bounds;
		}
	}

	private void Ὲ(object _٠, PaintEventArgs ؿ)
	{
		ItemPaintArgs p = new ItemPaintArgs(null, (Control)(object)_Ꮺ, ؿ.Graphics, new ColorScheme());
		Rectangle bounds = _ᾕ.Bounds;
		Rectangle bounds2 = bounds;
		bounds2.Location = new Point(0, 0);
		try
		{
			_ᾕ.Bounds = bounds2;
			_ᾕ.Paint(p);
		}
		finally
		{
			_ᾕ.Bounds = bounds;
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
		if (_ᾊ != null)
		{
			if (SubItems.Contains(_ᾊ))
			{
				_ᾊ.ὃ = false;
				_ᾊ.Refresh();
				_ᾊ = null;
				Refresh();
			}
			else
			{
				_ᾊ = null;
			}
		}
		else if (ControlBox.Visible && ControlBox.ᆼ)
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

	internal SuperTabItem ᾬ(int ԡ)
	{
		if (ԡ < 0 || ԡ >= SubItems.Count)
		{
			ԡ = 0;
		}
		for (int i = ԡ; i < SubItems.Count; i++)
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
		if (tab == null || !SubItems.Contains(tab) || Ᾱ(tab) || _Ꮺ.Ꮬ)
		{
			return;
		}
		SubItems.Remove(tab);
		Control attachedControl = tab.AttachedControl;
		if (attachedControl != null)
		{
			if (((Control)_Ꮺ).Controls.Contains(attachedControl))
			{
				((Control)_Ꮺ).Controls.Remove(attachedControl);
			}
			((Component)(object)attachedControl).Dispose();
		}
		_Ꮺ.RecalcLayout();
	}

	public override void Paint(ItemPaintArgs p)
	{
		if (_ᾏ != null)
		{
			_ᾏ._139A(p);
		}
	}

	public override void SubItemSizeChanged(BaseItem objChildItem)
	{
		base.SubItemSizeChanged(objChildItem);
		_Ꮺ.RecalcLayout();
		if (!DesignMode)
		{
			ᾨ(SelectedTab);
		}
	}

	public override BaseItem Copy()
	{
		SuperTabStripItem superTabStripItem = new SuperTabStripItem(_Ꮺ);
		CopyToItem(superTabStripItem);
		return superTabStripItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		if (copy is SuperTabStripItem superTabStripItem)
		{
			base.CopyToItem(superTabStripItem);
			superTabStripItem.AutoCloseTabs = _ᾅ;
			superTabStripItem.TabCloseButtonNormal = _ό;
			superTabStripItem.TabCloseButtonHot = _ὺ;
			superTabStripItem.TabHorizontalSpacing = __1F7E;
			superTabStripItem.TabVerticalSpacing = __1F7F;
			superTabStripItem.CloseButtonOnTabsVisible = _ᾀ;
			superTabStripItem.CloseButtonOnTabsAlwaysDisplayed = _ᾁ;
			superTabStripItem.CloseButtonPosition = _ᾒ;
			superTabStripItem.DisplaySelectedTextOnly = _ᾂ;
			superTabStripItem.TabFont = _ὒ;
			superTabStripItem.SelectedTabFont = _ὓ;
			superTabStripItem.HorizontalText = _ᾄ;
			superTabStripItem.ShowFocusRectangle = _ᾃ;
			superTabStripItem.FixedTabSize = __1F5C;
			superTabStripItem.ReorderTabsEnabled = _ᾇ;
			superTabStripItem.TabAlignment = _Ე;
			superTabStripItem.TabLayoutType = _ὸ;
			superTabStripItem.TabStripColor = _ᾉ;
			superTabStripItem.TabStyle = _ί;
			superTabStripItem.TextAlignment = _ᬭ;
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

	private InsertPosition ᆉ(Point ॺ, BaseItem ᆊ)
	{
		return GetContainerInsertPosition(ॺ, ᆊ);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem ᆊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, ᆊ);
	}

	private void ᆋ(int ᆌ, bool Έ)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, ᆌ, Έ);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool Έ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, Έ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool Έ)
	{
		DesignTimeProviderContainer.InsertItemAt(this, ਤ, ᆌ, Έ);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool Έ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, Έ);
	}
}
