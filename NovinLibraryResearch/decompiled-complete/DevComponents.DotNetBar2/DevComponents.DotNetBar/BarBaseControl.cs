using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxItem(false)]
public abstract class BarBaseControl : Control, IOwner, IOwnerMenuSupport, IOwnerLocalize, IOwnerItemEvents, IAccessibilitySupport, IRenderingSupport, ል, ሎ, IBarDesignerServices
{
	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

	private EventHandler _09BB;

	private EventHandler _09BC;

	private MouseEventHandler ঽ;

	private EventHandler _09BE;

	private EventHandler _09BF;

	private EventHandler _09C0;

	private EventHandler _09C1;

	private EventHandler _09C2;

	private EventHandler _09C3;

	private MouseEventHandler _09C4;

	private MouseEventHandler _09C5;

	private EventHandler _09C6;

	private EventHandler _09C7;

	private MouseEventHandler _09C8;

	private EventHandler _09C9;

	private EventHandler _09CA;

	private EventHandler _09CB;

	private EventHandler _09CC;

	private ItemRemovedEventHandler _09CD;

	private EventHandler ৎ;

	private EventHandler _09CF;

	private EventHandler _09D0;

	private ControlContainerItem.ControlContainerSerializationEventHandler _09D1;

	private ControlContainerItem.ControlContainerSerializationEventHandler _09D2;

	private EventHandler _09D3;

	private DotNetBarManager.LocalizeStringEventHandler _09D4;

	private OptionGroupChangingEventHandler _09D5;

	private EventHandler _09D6;

	private BaseItem _09D7;

	private BaseItem _09D8;

	private BaseItem _09D9;

	private Hashtable _09DA = new Hashtable();

	private ImageList գ;

	private ImageList _09DB;

	private ImageList ড়;

	private BaseItem ঢ়;

	private bool _09DE;

	private bool য়;

	private bool ৠ;

	private bool ৡ;

	private bool _09E2;

	private bool _09E3;

	private Cursor _09E4;

	private Cursor ᇩ;

	private Cursor _09E5;

	private bool _০ = true;

	private bool _১;

	private bool _২;

	private bool _৩;

	private bool _৪;

	private IDesignTimeProvider _৫;

	private int _৬;

	private bool _৭;

	private Timer _৮;

	private BaseItem _৯;

	private _0A79 ৰ;

	private _0A7A ৱ;

	private _0A7B _09F2;

	private _0A7C _09F3;

	private _0A7D _09F4;

	private _0A7E _09F5;

	private _0A7F _09F6;

	private _0A80 _09F7;

	private ColorScheme լ;

	private bool _09F8;

	private string _09F9 = "Right-click to add more items...";

	private ItemStyle _0599 = new ItemStyle();

	private bool _09FA;

	private IBarItemDesigner _09FD;

	private BaseItem ਈ;

	private bool ե;

	private ᒺ _0A0E;

	private ArrayList ਏ = new ArrayList();

	private bool ਖ = true;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private EventHandler ਞ;

	Form IOwner.ছ
	{
		get
		{
			return ((Control)this).FindForm();
		}
		set
		{
		}
	}

	bool IOwner.জ => GetDesignMode();

	Form IOwner.ঝ
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

	bool IOwner.ᇢ
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwner.ঞ
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwner.ଜ => _09E2;

	BaseItem IOwner.ଛ => ঢ়;

	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	[Category("Data")]
	[DefaultValue(null)]
	[Browsable(true)]
	public ImageList Images
	{
		get
		{
			return գ;
		}
		set
		{
			if (գ != null)
			{
				((Component)(object)գ).Disposed -= ٶ;
			}
			գ = value;
			if (գ != null)
			{
				((Component)(object)գ).Disposed += ٶ;
			}
		}
	}

	[Description("ImageList for medium-sized images used on Items.")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Data")]
	public ImageList ImagesMedium
	{
		get
		{
			return _09DB;
		}
		set
		{
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed -= ٶ;
			}
			_09DB = value;
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed += ٶ;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("ImageList for large-sized images used on Items.")]
	[Category("Data")]
	public ImageList ImagesLarge
	{
		get
		{
			return ড়;
		}
		set
		{
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed -= ٶ;
			}
			ড় = value;
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed += ٶ;
			}
		}
	}

	[Description("Indicates whether Tooltips are shown on Bars and menus.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Run-time Behavior")]
	public bool ShowToolTips
	{
		get
		{
			return _০;
		}
		set
		{
			_০ = value;
		}
	}

	[Description("Indicates whether item shortcut is displayed in Tooltips.")]
	[Browsable(true)]
	[Category("Run-time Behavior")]
	[DefaultValue(false)]
	public bool ShowShortcutKeysInToolTips
	{
		get
		{
			return _১;
		}
		set
		{
			_১ = value;
		}
	}

	bool IOwnerMenuSupport.ঢ
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ণ
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ত
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.থ => false;

	eMenuDropShadow IOwnerMenuSupport.দ
	{
		get
		{
			return eMenuDropShadow.Hide;
		}
		set
		{
		}
	}

	ePopupAnimation IOwnerMenuSupport.ধ
	{
		get
		{
			return ePopupAnimation.SystemDefault;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ন
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	bool ሎ.প
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

	_0A79 ል.ফ
	{
		get
		{
			if (ৰ == null)
			{
				ৰ = new _0A79((Control)(object)this);
			}
			return ৰ;
		}
	}

	_0A7A ል.ব
	{
		get
		{
			if (ৱ == null)
			{
				ৱ = new _0A7A((Control)(object)this);
			}
			return ৱ;
		}
	}

	_0A7B ል.ভ
	{
		get
		{
			if (_09F2 == null)
			{
				_09F2 = new _0A7B((Control)(object)this);
			}
			return _09F2;
		}
	}

	_0A7C ል.ম
	{
		get
		{
			if (_09F3 == null)
			{
				_09F3 = new _0A7C((Control)(object)this);
			}
			return _09F3;
		}
	}

	_0A7D ል.য
	{
		get
		{
			if (_09F4 == null)
			{
				_09F4 = new _0A7D((Control)(object)this);
			}
			return _09F4;
		}
	}

	_0A7E ል.র
	{
		get
		{
			if (_09F5 == null)
			{
				_09F5 = new _0A7E((Control)(object)this);
			}
			return _09F5;
		}
	}

	_0A7F ል._09B1
	{
		get
		{
			if (_09F6 == null)
			{
				_09F6 = new _0A7F((Control)(object)this);
			}
			return _09F6;
		}
	}

	_0A80 ል.ল
	{
		get
		{
			if (_09F7 == null)
			{
				_09F7 = new _0A80((Control)(object)this);
			}
			return _09F7;
		}
	}

	BaseItem IAccessibilitySupport._09B3
	{
		get
		{
			return ਈ;
		}
		set
		{
			ਈ = value;
		}
	}

	protected virtual string EmptyContainerDesignTimeHint
	{
		get
		{
			return _09F9;
		}
		set
		{
			_09F9 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	public ItemStyle BackgroundStyle => _0599;

	[Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Behavior")]
	public bool DispatchShortcuts
	{
		get
		{
			return _৩;
		}
		set
		{
			_৩ = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets Bar Color Scheme.")]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			լ = value;
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	protected bool IsThemed
	{
		get
		{
			if (_09F8 && _09D7.EffectiveStyle != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && _0EEE._0EF0)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool DisabledImagesGrayScale
	{
		get
		{
			return ਖ;
		}
		set
		{
			ਖ = value;
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[Browsable(true)]
	public virtual bool ThemeAware
	{
		get
		{
			return _09F8;
		}
		set
		{
			_09F8 = value;
			_09D7.ThemeAware = value;
			if (GetDesignMode())
			{
				((Control)this).Refresh();
			}
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(eRenderMode.Global)]
	public eRenderMode RenderMode
	{
		get
		{
			return ԁ;
		}
		set
		{
			if (ԁ != value)
			{
				ԁ = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public BaseRenderer Renderer
	{
		get
		{
			return ਜ;
		}
		set
		{
			ਜ = value;
		}
	}

	IBarItemDesigner IBarDesignerServices.হ
	{
		get
		{
			return _09FD;
		}
		set
		{
			_09FD = value;
		}
	}

	public event EventHandler ButtonCheckedChanged
	{
		add
		{
			EventHandler eventHandler = _09BB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Item is clicked.")]
	public event EventHandler ItemClick
	{
		add
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Item is double-clicked.")]
	public event MouseEventHandler ItemDoubleClick
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ঽ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ঽ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ঽ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ঽ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when popup of type container is loading.")]
	public event EventHandler PopupContainerLoad
	{
		add
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when popup of type container is unloading.")]
	public event EventHandler PopupContainerUnload
	{
		add
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when popup item is about to open.")]
	public event EventHandler PopupOpen
	{
		add
		{
			EventHandler eventHandler = _09C0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when popup item is closing.")]
	public event EventHandler PopupClose
	{
		add
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs just before popup window is shown.")]
	public event EventHandler PopupShowing
	{
		add
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Item Expanded property has changed.")]
	public event EventHandler ExpandedChange
	{
		add
		{
			EventHandler eventHandler = _09C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse button is pressed.")]
	public event MouseEventHandler MouseDown
	{
		add
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			_09C4 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)_09C4, (Delegate?)(object)value);
		}
		remove
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			_09C4 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)_09C4, (Delegate?)(object)value);
		}
	}

	[Description("Occurs when mouse button is released.")]
	public event MouseEventHandler MouseUp
	{
		add
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			_09C5 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)_09C5, (Delegate?)(object)value);
		}
		remove
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			_09C5 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)_09C5, (Delegate?)(object)value);
		}
	}

	[Description("Occurs when mouse enters the item.")]
	public event EventHandler MouseEnter
	{
		add
		{
			_09C6 = (EventHandler)Delegate.Combine(_09C6, value);
		}
		remove
		{
			_09C6 = (EventHandler)Delegate.Remove(_09C6, value);
		}
	}

	[Description("Occurs when mouse leaves the item.")]
	public event EventHandler MouseLeave
	{
		add
		{
			_09C7 = (EventHandler)Delegate.Combine(_09C7, value);
		}
		remove
		{
			_09C7 = (EventHandler)Delegate.Remove(_09C7, value);
		}
	}

	[Description("Occurs when mouse moves over the item.")]
	public event MouseEventHandler MouseMove
	{
		add
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			_09C8 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)_09C8, (Delegate?)(object)value);
		}
		remove
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			_09C8 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)_09C8, (Delegate?)(object)value);
		}
	}

	[Description("Occurs when mouse remains still inside an item for an amount of time.")]
	public event EventHandler MouseHover
	{
		add
		{
			_09C9 = (EventHandler)Delegate.Combine(_09C9, value);
		}
		remove
		{
			_09C9 = (EventHandler)Delegate.Remove(_09C9, value);
		}
	}

	[Description("Occurs when item loses input focus.")]
	public event EventHandler LostFocus
	{
		add
		{
			_09CA = (EventHandler)Delegate.Combine(_09CA, value);
		}
		remove
		{
			_09CA = (EventHandler)Delegate.Remove(_09CA, value);
		}
	}

	[Description("Occurs when item receives input focus.")]
	public event EventHandler GotFocus
	{
		add
		{
			_09CB = (EventHandler)Delegate.Combine(_09CB, value);
		}
		remove
		{
			_09CB = (EventHandler)Delegate.Remove(_09CB, value);
		}
	}

	[Description("Occurs when user changes the item position.")]
	public event EventHandler UserCustomize
	{
		add
		{
			EventHandler eventHandler = _09CC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09CC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after an Item is removed from SubItemsCollection.")]
	public event ItemRemovedEventHandler ItemRemoved
	{
		add
		{
			ItemRemovedEventHandler itemRemovedEventHandler = _09CD;
			ItemRemovedEventHandler itemRemovedEventHandler2;
			do
			{
				itemRemovedEventHandler2 = itemRemovedEventHandler;
				ItemRemovedEventHandler value2 = (ItemRemovedEventHandler)Delegate.Combine(itemRemovedEventHandler2, value);
				itemRemovedEventHandler = Interlocked.CompareExchange(ref _09CD, value2, itemRemovedEventHandler2);
			}
			while ((object)itemRemovedEventHandler != itemRemovedEventHandler2);
		}
		remove
		{
			ItemRemovedEventHandler itemRemovedEventHandler = _09CD;
			ItemRemovedEventHandler itemRemovedEventHandler2;
			do
			{
				itemRemovedEventHandler2 = itemRemovedEventHandler;
				ItemRemovedEventHandler value2 = (ItemRemovedEventHandler)Delegate.Remove(itemRemovedEventHandler2, value);
				itemRemovedEventHandler = Interlocked.CompareExchange(ref _09CD, value2, itemRemovedEventHandler2);
			}
			while ((object)itemRemovedEventHandler != itemRemovedEventHandler2);
		}
	}

	[Description("Occurs after an Item has been added to the SubItemsCollection.")]
	public event EventHandler ItemAdded
	{
		add
		{
			EventHandler eventHandler = ৎ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ৎ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ৎ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ৎ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ControlContainerControl is created and contained control is needed.")]
	public event EventHandler ContainerLoadControl
	{
		add
		{
			EventHandler eventHandler = _09CF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09CF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09CF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Text property of an Item has changed.")]
	public event EventHandler ItemTextChanged
	{
		add
		{
			EventHandler eventHandler = _09D0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09D0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize
	{
		add
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D1;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D1, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

	public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize
	{
		add
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Combine(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
		remove
		{
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler = _09D2;
			ControlContainerItem.ControlContainerSerializationEventHandler controlContainerSerializationEventHandler2;
			do
			{
				controlContainerSerializationEventHandler2 = controlContainerSerializationEventHandler;
				ControlContainerItem.ControlContainerSerializationEventHandler value2 = (ControlContainerItem.ControlContainerSerializationEventHandler)Delegate.Remove(controlContainerSerializationEventHandler2, value);
				controlContainerSerializationEventHandler = Interlocked.CompareExchange(ref _09D2, value2, controlContainerSerializationEventHandler2);
			}
			while ((object)controlContainerSerializationEventHandler != controlContainerSerializationEventHandler2);
		}
	}

	[Description("Occurs after DotNetBar definition is loaded.")]
	public event EventHandler DefinitionLoaded
	{
		add
		{
			EventHandler eventHandler = _09D3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09D3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString
	{
		add
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Combine(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
		remove
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Remove(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
	}

	public event OptionGroupChangingEventHandler OptionGroupChanging
	{
		add
		{
			OptionGroupChangingEventHandler optionGroupChangingEventHandler = _09D5;
			OptionGroupChangingEventHandler optionGroupChangingEventHandler2;
			do
			{
				optionGroupChangingEventHandler2 = optionGroupChangingEventHandler;
				OptionGroupChangingEventHandler value2 = (OptionGroupChangingEventHandler)Delegate.Combine(optionGroupChangingEventHandler2, value);
				optionGroupChangingEventHandler = Interlocked.CompareExchange(ref _09D5, value2, optionGroupChangingEventHandler2);
			}
			while ((object)optionGroupChangingEventHandler != optionGroupChangingEventHandler2);
		}
		remove
		{
			OptionGroupChangingEventHandler optionGroupChangingEventHandler = _09D5;
			OptionGroupChangingEventHandler optionGroupChangingEventHandler2;
			do
			{
				optionGroupChangingEventHandler2 = optionGroupChangingEventHandler;
				OptionGroupChangingEventHandler value2 = (OptionGroupChangingEventHandler)Delegate.Remove(optionGroupChangingEventHandler2, value);
				optionGroupChangingEventHandler = Interlocked.CompareExchange(ref _09D5, value2, optionGroupChangingEventHandler2);
			}
			while ((object)optionGroupChangingEventHandler != optionGroupChangingEventHandler2);
		}
	}

	public event EventHandler ToolTipShowing
	{
		add
		{
			EventHandler eventHandler = _09D6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09D6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09D6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	internal event EventHandler ᒲ;

	internal event EventHandler ᒳ;

	internal event _218D ᯢ;

	public event EventHandler ItemLayoutUpdated
	{
		add
		{
			EventHandler eventHandler = ਞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ਞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ਞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ਞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public BarBaseControl()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		try
		{
			_09E4 = new Cursor(typeof(DotNetBarManager), "DRAGMOVE.CUR");
			ᇩ = new Cursor(typeof(DotNetBarManager), "DRAGCOPY.CUR");
			_09E5 = new Cursor(typeof(DotNetBarManager), "DRAGNONE.CUR");
		}
		catch (Exception)
		{
			_09E4 = null;
			ᇩ = null;
			_09E5 = null;
		}
		լ = new ColorScheme(eDotNetBarStyle.Office2003);
		_0599.BackColor1.Color = SystemColors.Control;
		_0599.ઐ += ઐ;
		((Control)this).IsAccessible = true;
	}

	protected bool GetDesignMode()
	{
		if (!_09FA)
		{
			return ((Component)this).DesignMode;
		}
		return _09FA;
	}

	internal void ᒶ(bool _᭓)
	{
		_09FA = _᭓;
		_09D7.SetDesignMode(_᭓);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new BarBaseControlAccessibleObject(this);
	}

	internal void ਟ(AccessibleEvents ਠ, int ਡ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).AccessibilityNotifyClients(ਠ, ਡ);
	}

	public virtual ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(_09D7, ItemName, arrayList);
		return arrayList;
	}

	public virtual ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(_09D7, ItemName, arrayList, itemType);
		return arrayList;
	}

	public virtual ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(_09D7, ItemName, arrayList, itemType, useGlobalName);
		return arrayList;
	}

	public virtual BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(_09D7, ItemName);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		return null;
	}

	private void ਣ(BaseItem ਤ)
	{
		if (ਤ != null && ਤ.Parent is PopupItem)
		{
			return;
		}
		if (_09D8 != null)
		{
			if (_09D8.Expanded)
			{
				_09D8.Expanded = false;
			}
			_09D8 = null;
		}
		_09D8 = ਤ;
	}

	void IOwner.SetExpandedItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਣ
		this.ਣ(ਤ);
	}

	private BaseItem ਥ()
	{
		return _09D8;
	}

	BaseItem IOwner.GetExpandedItem()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਥ
		return this.ਥ();
	}

	private void ਦ(BaseItem ਧ)
	{
		if (_09D9 != null && _09D9 != ਧ)
		{
			_09D9.OnLostFocus();
		}
		OnSetFocusItem(ਧ);
		_09D9 = ਧ;
		if (_09D9 != null)
		{
			_09D9.OnGotFocus();
		}
	}

	void IOwner.SetFocusItem(BaseItem ਧ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਦ
		this.ਦ(ਧ);
	}

	protected virtual void OnSetFocusItem(BaseItem objFocusItem)
	{
	}

	private BaseItem ਨ()
	{
		return _09D9;
	}

	BaseItem IOwner.GetFocusItem()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਨ
		return this.ਨ();
	}

	private void _0A29(BaseItem ਤ)
	{
	}

	void IOwner.DesignTimeContextMenu(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਩
		this._0A29(ਤ);
	}

	private void ਪ(BaseItem ਤ)
	{
		ᯣ ᯣ2 = null;
		if (ਤ.ShortcutString != "")
		{
			foreach (eShortcut shortcut in ਤ.Shortcuts)
			{
				if (!_09DA.ContainsKey(shortcut))
				{
					continue;
				}
				ᯣ2 = (ᯣ)_09DA[shortcut];
				try
				{
					ᯣ2.ᯥ.Remove(ਤ.Id);
					if (ᯣ2.ᯥ.Count == 0)
					{
						_09DA.Remove(ᯣ2.ᯤ);
					}
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in ਤ.SubItems)
		{
			((IOwner)this).RemoveShortcutsFromItem(subItem);
		}
	}

	void IOwner.RemoveShortcutsFromItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਪ
		this.ਪ(ਤ);
	}

	private void ਫ(BaseItem ਤ)
	{
		ᯣ ᯣ2 = null;
		if (ਤ.ShortcutString != "")
		{
			foreach (eShortcut shortcut in ਤ.Shortcuts)
			{
				if (_09DA.ContainsKey(shortcut))
				{
					ᯣ2 = (ᯣ)_09DA[ਤ.Shortcuts[0]];
				}
				else
				{
					ᯣ2 = new ᯣ(shortcut);
					_09DA.Add(ᯣ2.ᯤ, ᯣ2);
				}
				try
				{
					ᯣ2.ᯥ.Add(ਤ.Id, ਤ);
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in ਤ.SubItems)
		{
			((IOwner)this).AddShortcutsFromItem(subItem);
		}
	}

	void IOwner.AddShortcutsFromItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਫ
		this.ਫ(ਤ);
	}

	private void ਭ()
	{
	}

	void IOwner.Customize()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਭ
		this.ਭ();
	}

	private void ਮ(BaseItem _06EC, EventArgs ؿ)
	{
	}

	void IOwner.InvokeResetDefinition(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਮ
		this.ਮ(_06EC, ؿ);
	}

	private void ਰ()
	{
		if (this.ᒲ != null)
		{
			this.ᒲ(this, new EventArgs());
		}
	}

	void IOwner.OnApplicationActivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਰ
		this.ਰ();
	}

	private void _0A31()
	{
		ClosePopups();
		if (this.ᒳ != null)
		{
			this.ᒳ(this, new EventArgs());
		}
	}

	void IOwner.OnApplicationDeactivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਱
		this._0A31();
	}

	private void ਲ()
	{
	}

	void IOwner.OnParentPositionChanging()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਲ
		this.ਲ();
	}

	private void _0C77(BaseItem _06EC)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (!_09DE || ঢ় != null)
		{
			return;
		}
		ঢ় = _06EC;
		if (!ৡ)
		{
			((Control)this).Capture = true;
			if (_09E4 != (Cursor)null)
			{
				Cursor.Current = _09E4;
			}
			else
			{
				Cursor.Current = Cursors.Hand;
			}
			_09E2 = true;
		}
		else
		{
			_09E2 = true;
			((Control)this).DoDragDrop((object)_06EC, (DragDropEffects)(-2147483645));
			if (_09E2)
			{
				MouseDragDrop(-1, -1, null);
			}
		}
	}

	void IOwner.StartItemDrag(BaseItem _06EC)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ౷
		this._0C77(_06EC);
	}

	private void ਲ਼(object _٠, EventArgs ؿ)
	{
		if (_09CC != null)
		{
			_09CC(_٠, ؿ);
		}
	}

	void IOwner.InvokeUserCustomize(object _٠, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਲ਼
		this.ਲ਼(_٠, ؿ);
	}

	private void _0A34(object _٠, EndUserCustomizeEventArgs ؿ)
	{
	}

	void IOwner.InvokeEndUserCustomize(object _٠, EndUserCustomizeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਴
		this._0A34(_٠, ؿ);
	}

	private MdiClient ਵ(Form ਸ਼)
	{
		return BarFunctions.GetMdiClient(ਸ਼);
	}

	MdiClient IOwner.GetMdiClient(Form ਸ਼)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਵ
		return this.ਵ(ਸ਼);
	}

	private void ٶ(object _٠, EventArgs ؿ)
	{
		if (_٠ == գ)
		{
			գ = null;
		}
		else if (_٠ == ড়)
		{
			ড় = null;
		}
		else if (_٠ == _09DB)
		{
			_09DB = null;
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (((Control)this).Parent != null && (Images != null || ImagesLarge != null || ImagesMedium != null))
		{
			foreach (BaseItem subItem in _09D7.SubItems)
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
			_09D7.SetDesignMode(((Component)this).DesignMode);
		}
	}

	private void _0A37(object _٠, EventArgs ؿ)
	{
		if (_09D3 != null)
		{
			_09D3(_٠, ؿ);
		}
	}

	void IOwner.InvokeDefinitionLoaded(object _٠, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਷
		this._0A37(_٠, ؿ);
	}

	private void _0A3C(PopupItem _0A3D)
	{
		if (ਏ.Contains(_0A3D))
		{
			return;
		}
		if (!GetDesignMode())
		{
			if (!_২)
			{
				_1BE6._1BEA(this);
				_২ = true;
			}
		}
		else if (_0A0E == null)
		{
			_0A0E = new ᒺ(this);
		}
		if (!_৪)
		{
			ੴ();
		}
		if (ਏ.Count == 0)
		{
			PopupManager.RegisterPopup(this);
		}
		ਏ.Add(_0A3D);
		if (_0A3D.GetOwner() != this)
		{
			_0A3D.SetOwner(this);
		}
	}

	void IOwnerMenuSupport.RegisterPopup(PopupItem _0A3D)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਼
		this._0A3C(_0A3D);
	}

	private void _0A3E(PopupItem _0A3D)
	{
		if (ਏ.Contains(_0A3D))
		{
			ਏ.Remove(_0A3D);
		}
		if (ਏ.Count == 0)
		{
			_0A75();
			if (_0A0E != null)
			{
				_0A0E.Dispose();
				_0A0E = null;
			}
			PopupManager.UnregisterPopup(this);
		}
	}

	void IOwnerMenuSupport.UnregisterPopup(PopupItem _0A3D)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਾ
		this._0A3E(_0A3D);
	}

	private bool _0A3F()
	{
		foreach (PopupItem item in ਏ)
		{
			Control popupControl = item.PopupControl;
			if (popupControl != null && popupControl.DisplayRectangle.Contains(Control.MousePosition))
			{
				if (popupControl is MenuPanel)
				{
					((MenuPanel)(object)popupControl).ఆ();
				}
				else if (popupControl is Bar)
				{
					((Bar)(object)popupControl).ఆ();
				}
				return true;
			}
		}
		return false;
	}

	bool IOwnerMenuSupport.RelayMouseHover()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਿ
		return this._0A3F();
	}

	private void _0A49()
	{
		ClosePopups();
	}

	void IOwnerMenuSupport.ClosePopups()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੉
		this._0A49();
	}

	private void ClosePopups()
	{
		ArrayList arrayList = new ArrayList(ਏ);
		foreach (PopupItem item in arrayList)
		{
			item.ClosePopup();
		}
	}

	private void _0A40(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09C1 != null)
		{
			_09C1(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupClose(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੀ
		this._0A40(_06EC, ؿ);
	}

	private void _0A41(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09BE != null)
		{
			_09BE(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerLoad(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੁ
		this._0A41(_06EC, ؿ);
	}

	private void _0A42(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09BF != null)
		{
			_09BF(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerUnload(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੂ
		this._0A42(_06EC, ؿ);
	}

	private void _0A43(PopupItem _06EC, PopupOpenEventArgs ؿ)
	{
		if (_09C0 != null)
		{
			_09C0(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupOpen(PopupItem _06EC, PopupOpenEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੃
		this._0A43(_06EC, ؿ);
	}

	private void _0A44(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09C2 != null)
		{
			_09C2(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupShowing(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੄
		this._0A44(_06EC, ؿ);
	}

	private void _0A62(ButtonItem _06EC, EventArgs ؿ)
	{
		if (_09BB != null)
		{
			_09BB(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੢
		this._0A62(_06EC, ؿ);
	}

	private void _0A4E(BaseItem _06EC, EventArgs ؿ)
	{
		if (ৎ != null)
		{
			ৎ(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeItemAdded(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੎
		this._0A4E(_06EC, ؿ);
	}

	private void _0A4F(BaseItem _06EC, BaseItem ԟ, int _06ED)
	{
		if (_09CD != null)
		{
			_09CD(_06EC, new ItemRemovedEventArgs(ԟ, _06ED));
		}
	}

	void IOwnerItemEvents.InvokeItemRemoved(BaseItem _06EC, BaseItem ԟ, int _06ED)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੏
		this._0A4F(_06EC, ԟ, _06ED);
	}

	private void _0A50(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09C6 != null)
		{
			_09C6(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeMouseEnter(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੐
		this._0A50(_06EC, ؿ);
	}

	private void _0A51(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09C9 != null)
		{
			_09C9(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeMouseHover(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੑ
		this._0A51(_06EC, ؿ);
	}

	private void _0A52(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09C7 != null)
		{
			_09C7(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeMouseLeave(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੒
		this._0A52(_06EC, ؿ);
	}

	private void _0A53(BaseItem _06EC, MouseEventArgs ؿ)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (_09C4 != null)
		{
			_09C4.Invoke((object)_06EC, ؿ);
		}
		if (_06EC.ClickAutoRepeat && (int)ؿ.Button == 1048576)
		{
			_৯ = _06EC;
			if (_৮ == null)
			{
				_৮ = new Timer();
			}
			_৮.Interval = _06EC.ClickRepeatInterval;
			_৮.Tick += _0A55;
			_৮.Start();
		}
	}

	void IOwnerItemEvents.InvokeMouseDown(BaseItem _06EC, MouseEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੓
		this._0A53(_06EC, ؿ);
	}

	private void _0A54(BaseItem _06EC, MouseEventArgs ؿ)
	{
		if (_09C5 != null)
		{
			_09C5.Invoke((object)_06EC, ؿ);
		}
		if (_৮ != null && _৮.Enabled)
		{
			_৮.Stop();
			_৮.Enabled = false;
		}
	}

	void IOwnerItemEvents.InvokeMouseUp(BaseItem _06EC, MouseEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੔
		this._0A54(_06EC, ؿ);
	}

	private void _0A55(object _٠, EventArgs ؿ)
	{
		if (_৯ != null)
		{
			_৯.RaiseClick();
		}
		else
		{
			_৮.Stop();
		}
	}

	private void _0A56(BaseItem _06EC, MouseEventArgs ؿ)
	{
		if (_09C8 != null)
		{
			_09C8.Invoke((object)_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeMouseMove(BaseItem _06EC, MouseEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੖
		this._0A56(_06EC, ؿ);
	}

	private void _0A57(BaseItem ਤ, MouseEventArgs ؿ)
	{
		OnItemDoubleClick(ਤ, ؿ);
	}

	void IOwnerItemEvents.InvokeItemDoubleClick(BaseItem ਤ, MouseEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੗
		this._0A57(ਤ, ؿ);
	}

	protected virtual void OnItemDoubleClick(BaseItem objItem, MouseEventArgs e)
	{
		MouseEventHandler val = ঽ;
		if (val != null)
		{
			val.Invoke((object)objItem, e);
		}
	}

	private void _0A58(BaseItem ਤ)
	{
		if (_09BC != null)
		{
			_09BC(ਤ, new EventArgs());
		}
	}

	void IOwnerItemEvents.InvokeItemClick(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੘
		this._0A58(ਤ);
	}

	private void ਖ਼(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09CB != null)
		{
			_09CB(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeGotFocus(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਖ਼
		this.ਖ਼(_06EC, ؿ);
	}

	private void ਗ਼(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09CA != null)
		{
			_09CA(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeLostFocus(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਗ਼
		this.ਗ਼(_06EC, ؿ);
	}

	private void ਜ਼(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09C3 != null)
		{
			_09C3(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeExpandedChange(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਜ਼
		this.ਜ਼(_06EC, ؿ);
	}

	private void ੜ(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09D0 != null)
		{
			_09D0(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeItemTextChanged(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੜ
		this.ੜ(_06EC, ؿ);
	}

	private void _0A5D(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
		if (_09D2 != null)
		{
			_09D2(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeContainerControlDeserialize(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੝
		this._0A5D(_06EC, ؿ);
	}

	private void ਫ਼(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
		if (_09D1 != null)
		{
			_09D1(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeContainerControlSerialize(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਫ਼
		this.ਫ਼(_06EC, ؿ);
	}

	private void _0A5F(BaseItem _06EC, EventArgs ؿ)
	{
		if (_09CF != null)
		{
			_09CF(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeContainerLoadControl(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੟
		this._0A5F(_06EC, ؿ);
	}

	private void _0A60(BaseItem _06EC, OptionGroupChangingEventArgs ؿ)
	{
		if (_09D5 != null)
		{
			_09D5(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeOptionGroupChanging(BaseItem _06EC, OptionGroupChangingEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੠
		this._0A60(_06EC, ؿ);
	}

	private void _0A61(object _06EC, EventArgs ؿ)
	{
		if (_09D6 != null)
		{
			_09D6(_06EC, ؿ);
		}
	}

	void IOwnerItemEvents.InvokeToolTipShowing(object _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੡
		this._0A61(_06EC, ؿ);
	}

	private bool _0A64(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ._੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੤
		return this._0A64(_0A65, _੦, _੧);
	}

	private bool _੩(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected I4, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		int num = _0EFE._0F11(_੦);
		if (ਏ.Count > 0 && ((BaseItem)ਏ[ਏ.Count - 1]).Parent == null)
		{
			PopupItem popupItem = (PopupItem)ਏ[ਏ.Count - 1];
			Control popupControl = popupItem.PopupControl;
			Control val = Control.FromChildHandle(_0A65);
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
				Keys val2 = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
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
			return true;
		}
		if (!IsParentFormActive)
		{
			return false;
		}
		if (num >= 112 || (int)Control.ModifierKeys != 0 || (_0EFE._0F11(_੧) & 0x1000000000L) != 0 || num == 46 || num == 45)
		{
			int _੬ = Control.ModifierKeys | num;
			return _੫((eShortcut)_੬);
		}
		return false;
	}

	bool ሎ._੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੩
		return this._੩(_0A65, _੦, _੧);
	}

	private bool _੫(eShortcut _੬)
	{
		bool result = BarFunctions.ProcessItemsShortcuts(_੬, _09DA);
		if (!_৩)
		{
			return result;
		}
		return false;
	}

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (this.ᯢ != null)
		{
			this.ᯢ(this, new _218C(_0A65, _੦, _੧));
		}
		if (ਏ.Count == 0)
		{
			return false;
		}
		for (int num = ਏ.Count - 1; num >= 0; num--)
		{
			PopupItem popupItem = ਏ[num] as PopupItem;
			bool flag = popupItem.IsAnyOnHandle(_0A65);
			if (!flag)
			{
				Control val = Control.FromChildHandle(_0A65);
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
					string text = _089F.स(_0A65);
					text = text.ToLower();
					if (text.IndexOf("combolbox") >= 0)
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
			popupItem.ClosePopup();
			if (ਏ.Count == 0)
			{
				break;
			}
		}
		return false;
	}

	bool ሎ.ሏ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੮
		return this._੮(_0A65, _੦, _੧);
	}

	private bool _੯(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (ਏ.Count > 0)
		{
			foreach (BaseItem item in ਏ)
			{
				if (item.Parent == null)
				{
					Control popupControl = ((PopupItem)item).PopupControl;
					if (popupControl != null && popupControl.Handle != _0A65 && !item.IsAnyOnHandle(_0A65) && (popupControl.Parent == null || !(popupControl.Parent.Handle != _0A65)))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	bool ሎ.ሐ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੯
		return this._੯(_0A65, _੦, _੧);
	}

	private bool _0A70(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		if (!GetDesignMode())
		{
			int num = _0EFE._0F11(_੦);
			if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
			{
				int _੬ = Control.ModifierKeys | num;
				if (_੫((eShortcut)_੬))
				{
					return true;
				}
			}
		}
		return false;
	}

	bool ሎ._0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੰ
		return this._0A70(_0A65, _੦, _੧);
	}

	private bool ੲ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੲ
		return this.ੲ(_0A65, _੦, _੧);
	}

	private void ੴ()
	{
		if (!_৪)
		{
			_৪ = true;
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				_৪ = false;
				return;
			}
			((Control)val).Resize += _0A76;
			val.Deactivate += _0A77;
			DotNetBarManager.RegisterParentMsgHandler(this, val);
		}
	}

	private void _0A75()
	{
		if (_৪)
		{
			_৪ = false;
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				DotNetBarManager.UnRegisterParentMsgHandler(this, val);
				((Control)val).Resize -= _0A76;
				val.Deactivate -= _0A77;
			}
		}
	}

	private void _0A76(object _٠, EventArgs ؿ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void _0A77(object _٠, EventArgs ؿ)
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
		else if (((Message)(ref m)).Msg == 1131 && ਈ != null)
		{
			ਈ.DoAccesibleDefaultAction();
			ਈ = null;
		}
		((Control)this).WndProc(ref m);
	}

	protected void RefreshThemes()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = new _0A79((Control)(object)this);
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = new _0A7A((Control)(object)this);
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = new _0A7B((Control)(object)this);
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = new _0A7C((Control)(object)this);
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = new _0A7D((Control)(object)this);
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = new _0A7F((Control)(object)this);
		}
		if (_09F5 != null)
		{
			_09F5.Dispose();
			_09F5 = new _0A7E((Control)(object)this);
		}
		if (_09F7 != null)
		{
			_09F7.Dispose();
			_09F7 = new _0A80((Control)(object)this);
		}
	}

	private void _0A78()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = null;
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = null;
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = null;
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = null;
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = null;
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = null;
		}
		if (_09F5 != null)
		{
			_09F5.Dispose();
			_09F5 = null;
		}
		if (_09F7 != null)
		{
			_09F7.Dispose();
			_09F7 = null;
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (!_২ && !((Component)this).DesignMode)
		{
			_1BE6._1BEA(this);
			_২ = true;
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		_0A75();
		((Control)this).OnHandleDestroyed(e);
		if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_09D7.InternalClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_09D7.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_0A81(e);
		((Control)this).OnKeyDown(e);
	}

	internal void _0A81(KeyEventArgs ؿ)
	{
		_09D7.InternalKeyDown(ؿ);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		_09D7.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		_09D7.InternalMouseHover();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		_09D7.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (_09DE && !ৡ)
		{
			MouseDragDrop(e.X, e.Y, null);
		}
		_09D7.InternalMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (_09DE && _09E2)
		{
			if (!ৡ)
			{
				ᇺ(e.X, e.Y, null);
			}
		}
		else
		{
			_09D7.InternalMouseMove(e);
		}
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		if (_09DE && (_09E2 || _09E3))
		{
			Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
			ᇺ(point.X, point.Y, e);
			য় = false;
		}
		((Control)this).OnDragOver(e);
	}

	protected override void OnDragLeave(EventArgs e)
	{
		if (_09DE)
		{
			if (_09E2 || _09E3)
			{
				ᇺ(-1, -1, null);
			}
			য় = true;
			_09E3 = false;
		}
		((Control)this).OnDragLeave(e);
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		((Control)this).OnDragEnter(e);
		if (!_09DE || _09E2 || !ৠ)
		{
			return;
		}
		if (e.Data.GetData(typeof(ButtonItem)) == null)
		{
			if ((int)e.Effect != 0)
			{
				_09E3 = true;
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
		_09E3 = true;
	}

	protected override void OnDragDrop(DragEventArgs e)
	{
		if (_09DE)
		{
			if (_09E2)
			{
				Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
				MouseDragDrop(point.X, point.Y, null);
			}
			else if (_09E3)
			{
				Point point2 = ((Control)this).PointToClient(new Point(e.X, e.Y));
				MouseDragDrop(point2.X, point2.Y, e);
			}
		}
		((Control)this).OnDragDrop(e);
	}

	protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		if (_09DE && _09E2 && ((য় && (int)e.Action == 1) || (int)e.Action == 2))
		{
			MouseDragDrop(-1, -1, null);
		}
		((Control)this).OnQueryContinueDrag(e);
	}

	private void ᇺ(int و, int ى, DragEventArgs _0A83)
	{
		if (!_09E2 && !_09E3)
		{
			return;
		}
		BaseItem baseItem = ঢ়;
		if (_09E3 && _0A83 != null)
		{
			baseItem = _0A83.Data.GetData(typeof(ButtonItem)) as BaseItem;
		}
		if (_৫ != null)
		{
			_৫.DrawReversibleMarker(_৬, _৭);
			_৫ = null;
		}
		if (_09E3 && baseItem == null)
		{
			return;
		}
		Point pScreen = ((Control)this).PointToScreen(new Point(و, ى));
		foreach (SideBarPanelItem subItem in _09D7.SubItems)
		{
			if (!subItem.Visible)
			{
				continue;
			}
			InsertPosition insertPosition = ((IDesignTimeProvider)subItem).GetInsertPosition(pScreen, baseItem);
			if (insertPosition != null)
			{
				if (insertPosition.TargetProvider == null)
				{
					if (!ৡ)
					{
						if (_09E5 != (Cursor)null)
						{
							Cursor.Current = _09E5;
						}
						else
						{
							Cursor.Current = Cursors.No;
						}
					}
					break;
				}
				insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
				_৬ = insertPosition.Position;
				_৭ = insertPosition.Before;
				_৫ = insertPosition.TargetProvider;
				if (!ৡ)
				{
					if (_09E4 != (Cursor)null)
					{
						Cursor.Current = _09E4;
					}
					else
					{
						Cursor.Current = Cursors.Hand;
					}
				}
				else if (_0A83 != null)
				{
					_0A83.Effect = (DragDropEffects)2;
				}
				break;
			}
			if (!ৡ)
			{
				if (_09E5 != (Cursor)null)
				{
					Cursor.Current = _09E5;
				}
				else
				{
					Cursor.Current = Cursors.No;
				}
			}
			else if (_0A83 != null)
			{
				_0A83.Effect = (DragDropEffects)0;
			}
		}
	}

	private void ઌ(LocalizeEventArgs ؿ)
	{
		if (_09D4 != null)
		{
			_09D4(this, ؿ);
		}
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ઌ
		this.ઌ(ؿ);
	}

	protected void SetBaseItemContainer(BaseItem item)
	{
		_09D7 = item;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public BaseItem GetBaseItemContainer()
	{
		return _09D7;
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (_09D7 != null)
		{
			_09D7.UpdateBindings();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _৮ != null)
		{
			_৮.Stop();
			((Component)(object)_৮).Dispose();
			_৮ = null;
		}
		((Control)this).Dispose(disposing);
	}

	public void ResetBackgroundStyle()
	{
		_0599.ઐ -= ઐ;
		_0599 = new ItemStyle();
		_0599.ઐ += ઐ;
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		if (_0599 != null)
		{
			_0599.ᗅ(ColorScheme);
		}
		if (GetDesignMode())
		{
			((Control)this).Refresh();
		}
	}

	protected virtual void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		if (!_09E2 && !_09E3)
		{
			return;
		}
		BaseItem baseItem = ঢ়;
		if (_09E3)
		{
			baseItem = dragArgs.Data.GetData(typeof(ButtonItem)) as BaseItem;
		}
		baseItem?.InternalMouseLeave();
		if (_৫ != null)
		{
			if (x == -1 && y == -1)
			{
				_৫.DrawReversibleMarker(_৬, _৭);
			}
			else
			{
				_৫.DrawReversibleMarker(_৬, _৭);
				if (baseItem != null)
				{
					BaseItem parent = baseItem.Parent;
					if (parent != null)
					{
						if (parent == (BaseItem)_৫ && _৬ > 0 && parent.SubItems.IndexOf(baseItem) < _৬)
						{
							_৬--;
						}
						parent.SubItems.Remove(baseItem);
						_ = parent.ContainerControl;
						parent.Refresh();
					}
					_৫.InsertItemAt(baseItem, _৬, _৭);
					_৫ = null;
					((IOwner)this).InvokeUserCustomize((object)baseItem, new EventArgs());
				}
			}
		}
		_৫ = null;
		_09E2 = false;
		_09E3 = false;
		if (!ৡ)
		{
			Cursor.Current = Cursors.Default;
		}
		((Control)this).Capture = false;
		if (baseItem != null)
		{
			baseItem._IgnoreClick = true;
		}
		_09D7.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, x, y, 0));
		if (baseItem != null)
		{
			baseItem._IgnoreClick = false;
		}
		ঢ় = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	public virtual void SetDesignTimeDefaults()
	{
	}

	public Graphics CreateGraphics()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = ((Control)this).CreateGraphics();
		if (ե)
		{
			val.SmoothingMode = (SmoothingMode)4;
			if (!SystemInformation.IsFontSmoothingEnabled)
			{
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
		}
		return val;
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (ԁ == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (ԁ == eRenderMode.Custom && ਜ != null)
		{
			return ਜ;
		}
		if (ਛ == null)
		{
			ਛ = new Office2007Renderer();
		}
		return ਜ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(_09D7.EffectiveStyle) && GetRenderer() is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		return լ;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (_09D7 != null && !((Control)this).IsDisposed)
		{
			ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, e.Graphics, GetColorScheme());
			itemPaintArgs.ໝ = GetRenderer();
			if (ե)
			{
				itemPaintArgs.Graphics.SmoothingMode = (SmoothingMode)4;
				itemPaintArgs.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			PaintControlBackground(itemPaintArgs);
			itemPaintArgs.Graphics.SetClip(GetItemContainerRectangle());
			PaintItemContainer(itemPaintArgs);
			itemPaintArgs.Graphics.ResetClip();
			if (_09D7.SubItems.Count == 0 && GetDesignMode())
			{
				PaintDesignTimeEmptyHint(itemPaintArgs);
			}
			PaintContentOnTop(itemPaintArgs);
		}
	}

	protected virtual void PaintContentOnTop(ItemPaintArgs pa)
	{
	}

	protected virtual void PaintControlBackground(ItemPaintArgs pa)
	{
		if (_0599 != null)
		{
			_0599.ᗅ(pa.Colors);
			_0599.Paint(pa.Graphics, ((Control)this).ClientRectangle);
		}
	}

	protected virtual void PaintItemContainer(ItemPaintArgs pa)
	{
		_09D7.Paint(pa);
	}

	protected virtual void PaintDesignTimeEmptyHint(ItemPaintArgs pa)
	{
		string text = _09F9;
		Rectangle itemContainerRectangle = GetItemContainerRectangle();
		itemContainerRectangle.Inflate(-2, -2);
		if (itemContainerRectangle.Width >= 0 && itemContainerRectangle.Height >= 0)
		{
			eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			TextDrawing.DrawString(pa.Graphics, text, ((Control)this).Font, SystemColors.ControlDark, itemContainerRectangle, format);
		}
	}

	protected virtual Rectangle GetItemContainerRectangle()
	{
		if (_0599 == null)
		{
			return ((Control)this).ClientRectangle;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (_0599.Border == eBorderType.SingleLine)
		{
			if (_0599.BorderSide == eBorderSide.All)
			{
				clientRectangle.Inflate(-1, -1);
			}
			else
			{
				if ((_0599.BorderSide & eBorderSide.Left) != 0)
				{
					clientRectangle.X++;
				}
				if ((_0599.BorderSide & eBorderSide.Right) != 0)
				{
					clientRectangle.Width--;
				}
				if ((_0599.BorderSide & eBorderSide.Top) != 0)
				{
					clientRectangle.Y++;
				}
				if ((_0599.BorderSide & eBorderSide.Bottom) != 0)
				{
					clientRectangle.Height--;
				}
			}
		}
		else if (_0599.Border != 0)
		{
			if (_0599.BorderSide == eBorderSide.All)
			{
				clientRectangle.Inflate(-2, -2);
			}
			else
			{
				if ((_0599.BorderSide & eBorderSide.Left) != 0)
				{
					clientRectangle.X += 2;
				}
				if ((_0599.BorderSide & eBorderSide.Right) != 0)
				{
					clientRectangle.Width -= 2;
				}
				if ((_0599.BorderSide & eBorderSide.Top) != 0)
				{
					clientRectangle.Y += 2;
				}
				if ((_0599.BorderSide & eBorderSide.Bottom) != 0)
				{
					clientRectangle.Height -= 2;
				}
			}
		}
		return clientRectangle;
	}

	protected virtual void RecalcSize()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (!_09D7.IsRecalculatingSize)
		{
			Rectangle itemContainerRectangle = GetItemContainerRectangle();
			_09D7.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
			_09D7.LeftInternal = itemContainerRectangle.X;
			_09D7.TopInternal = itemContainerRectangle.Y;
			_09D7.WidthInternal = itemContainerRectangle.Width;
			_09D7.HeightInternal = itemContainerRectangle.Height;
			_09D7.RecalcSize();
			OnItemLayoutUpdated(EventArgs.Empty);
		}
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		ਞ?.Invoke(this, e);
	}

	public virtual void RecalcLayout()
	{
		if (!_09D7.IsRecalculatingSize)
		{
			RecalcSize();
			((Control)this).Invalidate();
		}
	}
}
