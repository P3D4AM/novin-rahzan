using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Touch;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
[DefaultEvent("ItemClick")]
public abstract class ItemControl : ContainerControl, IOwner, IBarImageSize, IOwnerMenuSupport, IOwnerLocalize, IOwnerItemEvents, IAccessibilitySupport, IKeyTipsControl, _103D, IRenderingSupport, ል, ሎ, IBarDesignerServices, IRibbonCustomize
{
	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

	private const uint _09BA = 282u;

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

	private ElementStyle _0599 = new ElementStyle();

	private bool _09FA;

	private bool ե = true;

	private bool _09FB;

	private bool ৼ;

	private IBarItemDesigner _09FD;

	private bool _09FE;

	private Font _09FF;

	private string _0A00 = "";

	private _103E _0A01;

	private bool _0A02;

	private IntPtr _0A03 = IntPtr.Zero;

	private Timer _0A04;

	private IntPtr ਅ = IntPtr.Zero;

	private IntPtr ਆ = IntPtr.Zero;

	private bool ਇ = true;

	private BaseItem ਈ;

	private eBarImageSize ਉ;

	private bool ਊ = true;

	private bool _0A0B;

	private bool _0A0C;

	private CancelEventHandler _0A0D;

	private ᒺ _0A0E;

	private ArrayList ਏ = new ArrayList();

	private _20E7 ਐ;

	private bool __0A11;

	private List<Type> __0A12 = new List<Type>();

	private bool _ਓ = true;

	private int ਔ;

	private eOrientation _ਕ = eOrientation.Vertical;

	private bool ਖ = true;

	private bool _ਗ;

	private Timer ਘ;

	private bool ਙ = true;

	private bool _ਚ;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private int ਝ;

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

	[Category("Run-time Behavior")]
	[Description("Indicates whether accelerator letters for buttons are underlined regardless of current Windows settings.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool AlwaysDisplayKeyAccelerators
	{
		get
		{
			return _0A0C;
		}
		set
		{
			if (_0A0C != value)
			{
				_0A0C = value;
				((Control)this).Invalidate();
			}
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

	[Browsable(false)]
	public bool DragInProgress
	{
		get
		{
			return _09E2;
		}
		internal set
		{
			if (_09E2 != value)
			{
				_09E2 = value;
				OnDragInProgressChanged();
			}
		}
	}

	[Browsable(false)]
	public bool ExternalDragInProgress
	{
		get
		{
			return _09E3;
		}
		internal set
		{
			if (ExternalDragInProgress != value)
			{
				ExternalDragInProgress = value;
			}
		}
	}

	[Browsable(false)]
	public BaseItem DragItem
	{
		get
		{
			return ঢ়;
		}
		internal set
		{
			ঢ় = value;
		}
	}

	internal IDesignTimeProvider ট
	{
		get
		{
			return _৫;
		}
		set
		{
			_৫ = value;
		}
	}

	internal int ঠ
	{
		get
		{
			return _৬;
		}
		set
		{
			_৬ = value;
		}
	}

	internal bool ড
	{
		get
		{
			return _৭;
		}
		set
		{
			_৭ = value;
		}
	}

	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	[Browsable(true)]
	public virtual ImageList Images
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

	[Browsable(true)]
	[Category("Data")]
	[Description("ImageList for medium-sized images used on Items.")]
	[DefaultValue(null)]
	public virtual ImageList ImagesMedium
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

	[DefaultValue(null)]
	[Category("Data")]
	[Browsable(true)]
	[Description("ImageList for large-sized images used on Items.")]
	public virtual ImageList ImagesLarge
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

	[DefaultValue(true)]
	[Category("Run-time Behavior")]
	[Browsable(true)]
	[Description("Indicates whether Tooltips are shown on Bars and menus.")]
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
	[Category("Run-time Behavior")]
	[DefaultValue(false)]
	[Browsable(true)]
	public virtual bool ShowShortcutKeysInToolTips
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

	[Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(false)]
	public bool UseHook
	{
		get
		{
			return _0A0B;
		}
		set
		{
			_0A0B = value;
		}
	}

	bool IOwnerMenuSupport.থ => true;

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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool HasMenuFocus => _09A9;

	internal virtual bool _09A9
	{
		get
		{
			return _0A02;
		}
		set
		{
			if (_0A02 == value)
			{
				return;
			}
			_0A4C(value);
			if (_0A02)
			{
				if (_09D7 is GenericItemContainer)
				{
					((GenericItemContainer)_09D7).ௐ();
				}
				else if (_09D7 is RibbonStripContainerItem)
				{
					((RibbonStripContainerItem)_09D7).ௐ();
				}
				SetupActiveWindowTimer();
			}
			else
			{
				ReleaseActiveWindowTimer();
				if (_09D7 is GenericItemContainer)
				{
					((GenericItemContainer)_09D7).AutoExpand = false;
					((GenericItemContainer)_09D7).ඹ();
					((GenericItemContainer)_09D7).ContainerLostFocus(appLostFocus: false);
				}
				else if (_09D7 is RibbonStripContainerItem)
				{
					((RibbonStripContainerItem)_09D7).AutoExpand = false;
					((RibbonStripContainerItem)_09D7).ඹ();
					((RibbonStripContainerItem)_09D7).ContainerLostFocus(appLostFocus: false);
				}
			}
			((Control)this).Refresh();
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Control LastFocusControl
	{
		get
		{
			if (((Control)this).Focused && _0A03 != IntPtr.Zero)
			{
				return Control.FromChildHandle(_0A03);
			}
			return null;
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool HasRegisteredPopups => ਏ.Count > 0;

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

	[DefaultValue(false)]
	[Browsable(false)]
	public bool EnableGestureNotifyHandler
	{
		get
		{
			return __0A11;
		}
		set
		{
			__0A11 = value;
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Type> DragDropAllowedContainerTypes
	{
		get
		{
			return __0A12;
		}
		internal set
		{
			__0A12 = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool ContainerControlProcessDialogKey
	{
		get
		{
			return _ਓ;
		}
		set
		{
			_ਓ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShortcutsEnabled
	{
		get
		{
			return ਊ;
		}
		set
		{
			ਊ = value;
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

	[Description("Indicates whether control height is set automatically.")]
	[Category("Layout")]
	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	public override bool AutoSize
	{
		get
		{
			return _09FB;
		}
		set
		{
			_09FB = value;
			_0A8E();
		}
	}

	internal eOrientation _09B4
	{
		get
		{
			return _ਕ;
		}
		set
		{
			_ਕ = value;
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

	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public ElementStyle BackgroundStyle => _0599;

	[Category("Behavior")]
	[Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
	[DefaultValue(false)]
	[Browsable(true)]
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

	[Browsable(false)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(false)]
	[Description("Gets or sets Bar Color Scheme.")]
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

	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(true)]
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

	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
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

	[Category("Appearance")]
	[Browsable(false)]
	[Description("Specifies whether SideBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[DefaultValue(false)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool DesignerSelection
	{
		get
		{
			return ৼ;
		}
		set
		{
			if (ৼ != value)
			{
				ৼ = value;
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool ShowKeyTips
	{
		get
		{
			return _09FE;
		}
		set
		{
			if (_09FE != value)
			{
				_09FE = value;
				OnShowKeyTipsChanged();
			}
		}
	}

	string IKeyTipsControl._09B5
	{
		get
		{
			return _0A00;
		}
		set
		{
			_0A00 = value;
			if (_0A01 != null && ((Control)_0A01).Parent != null)
			{
				((Control)_0A01).Parent.Invalidate(true);
			}
		}
	}

	[Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(null)]
	public virtual Font KeyTipsFont
	{
		get
		{
			return _09FF;
		}
		set
		{
			_09FF = value;
		}
	}

	internal bool শ
	{
		get
		{
			return _ਗ;
		}
		set
		{
			_ਗ = value;
		}
	}

	internal virtual bool ষ
	{
		get
		{
			if (GetDesignMode())
			{
				return false;
			}
			return _0EFE.ষ;
		}
	}

	[DefaultValue(eRenderMode.Global)]
	[Browsable(false)]
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

	protected virtual bool NeedsTopLevelKeyTipCanvasParent => false;

	[Category("Appearance")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether mouse over fade effect is enabled")]
	public bool FadeEffect
	{
		get
		{
			return ਇ;
		}
		set
		{
			ਇ = value;
		}
	}

	internal bool স
	{
		get
		{
			if (((Component)this).DesignMode || !BarFunctions.IsOffice2007Style(_09D7.EffectiveStyle) || (ਇ && _089F.ष()) || TextDrawing.UseTextRenderer || _09D7.EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_09D7.EffectiveStyle))
			{
				return false;
			}
			return ਇ;
		}
	}

	[Browsable(false)]
	public bool IsUpdateSuspended => ਝ > 0;

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

	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Description("Specifies the Image size that will be used by items on this bar.")]
	[Browsable(true)]
	[DefaultValue(eBarImageSize.Default)]
	public virtual eBarImageSize ImageSize
	{
		get
		{
			return ਉ;
		}
		set
		{
			ਉ = value;
			if (_09D7 is ImageItem)
			{
				((ImageItem)_09D7).RefreshImageSize();
			}
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	[DefaultValue(false)]
	public virtual bool AllowExternalDrop
	{
		get
		{
			return ৠ;
		}
		set
		{
			ৠ = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(false)]
	[Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
	public virtual bool UseNativeDragDrop
	{
		get
		{
			return ৡ;
		}
		set
		{
			ৡ = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[Description("Indicates whether internal automatic drag & drop support is enabled")]
	[Category("Behavior")]
	public virtual bool DragDropSupport
	{
		get
		{
			return _09DE;
		}
		set
		{
			_09DE = value;
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

	[Description("Occurs before an item drag & drop operation is started and allows cancellation")]
	public event CancelEventHandler BeforeItemDrag
	{
		add
		{
			CancelEventHandler cancelEventHandler = _0A0D;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0A0D, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _0A0D;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0A0D, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

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

	public ItemControl()
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		_ = GlobalManager.Renderer;
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
		((Control)this).SetStyle((ControlStyles)2048, true);
		_09E4 = null;
		_09E5 = null;
		լ = new ColorScheme(eDotNetBarStyle.Office2003);
		_0599.SetColorScheme(լ);
		_0599.StyleChanged += ઐ;
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode(bool mode)
	{
		_09FA = mode;
		_09D7.SetDesignMode(mode);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new ItemControlAccessibleObject(this);
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
	}

	void IOwner.OnApplicationActivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਰ
		this.ਰ();
	}

	private void _0A31()
	{
		ClosePopups();
		if (_09D7 is GenericItemContainer)
		{
			((GenericItemContainer)_09D7).ContainerLostFocus(appLostFocus: true);
		}
		else if (_09D7 is RibbonStripContainerItem)
		{
			((RibbonStripContainerItem)_09D7).ContainerLostFocus(appLostFocus: true);
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

	protected virtual void OnBeforeItemDrag(BaseItem itemSource, CancelEventArgs e)
	{
		_0A0D?.Invoke(itemSource, e);
	}

	public void StartItemDrag(BaseItem item)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		if (!_09DE)
		{
			return;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeItemDrag(item, cancelEventArgs);
		if (cancelEventArgs.Cancel || ঢ় != null)
		{
			return;
		}
		if (!ৡ)
		{
			ঢ় = item;
			Point dragStartPoint = ((Control)this).PointToClient(Control.MousePosition);
			dragStartPoint.X -= ঢ়.LeftInternal;
			dragStartPoint.Y -= ঢ়.TopInternal;
			if (dragStartPoint.IsEmpty)
			{
				dragStartPoint.Offset(1, 1);
			}
			ঢ়.DragStartPoint = dragStartPoint;
			((Control)this).Capture = true;
			if (_09E4 != (Cursor)null)
			{
				Cursor.Current = _09E4;
			}
			else
			{
				Cursor.Current = Cursors.Hand;
			}
			DragInProgress = true;
		}
		else
		{
			if (((Control)this).AllowDrop)
			{
				DragInProgress = true;
			}
			((Control)this).DoDragDrop((object)item, (DragDropEffects)(-2147483645));
		}
	}

	protected virtual void OnDragInProgressChanged()
	{
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
		((ContainerControl)this).OnParentChanged(e);
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

	internal void _0A3B()
	{
		if (!_২)
		{
			_1BE6._1BEA(this);
			_২ = true;
		}
	}

	private void _0A3C(PopupItem _0A3D)
	{
		if (ਏ.Contains(_0A3D))
		{
			return;
		}
		if (!GetDesignMode() && !_0A0B)
		{
			_0A3B();
			if (!_৪)
			{
				ੴ();
			}
		}
		else if (_0A0E == null)
		{
			_0A0E = new ᒺ(this);
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

	private void _0A49()
	{
		ClosePopups();
	}

	void IOwnerMenuSupport.ClosePopups()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੉
		this._0A49();
	}

	internal void ClosePopups()
	{
		ArrayList arrayList = new ArrayList(ਏ);
		foreach (PopupItem item in arrayList)
		{
			item.ClosePopup();
		}
	}

	internal void _0A4A(string _0A4B)
	{
		ArrayList arrayList = new ArrayList(ਏ);
		foreach (PopupItem item in arrayList)
		{
			if (item.Name == _0A4B)
			{
				item.ClosePopup();
			}
		}
	}

	internal void _0A4C(bool _0A4D)
	{
		_0A02 = _0A4D;
	}

	public void ReleaseFocus()
	{
		if (!((Control)this).Focused || !(_0A03 != IntPtr.Zero))
		{
			return;
		}
		Control val = Control.FromChildHandle(_0A03);
		if (val != null)
		{
			val.Select();
			if (!val.Focused)
			{
				_089F.SetFocus(_0A03);
			}
		}
		else
		{
			_089F.SetFocus(_0A03);
		}
		_0A03 = IntPtr.Zero;
		if (_09D7 is GenericItemContainer)
		{
			((GenericItemContainer)_09D7).AutoExpand = false;
		}
	}

	protected void CopyIOwnerEvents(ItemControl target)
	{
		target._09BC = _09BC;
	}

	protected virtual void OnItemAdded(BaseItem item, EventArgs e)
	{
		if (ৎ != null)
		{
			ৎ(item, e);
		}
	}

	private void _0A4E(BaseItem _06EC, EventArgs ؿ)
	{
		OnItemAdded(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeItemAdded(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੎
		this._0A4E(_06EC, ؿ);
	}

	protected virtual void OnItemRemoved(BaseItem item, ItemRemovedEventArgs e)
	{
		if (_09CD != null)
		{
			_09CD(item, e);
		}
	}

	private void _0A4F(BaseItem _06EC, BaseItem ԟ, int _06ED)
	{
		OnItemRemoved(_06EC, new ItemRemovedEventArgs(ԟ, _06ED));
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

	protected virtual void OnItemMouseDown(BaseItem item, MouseEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (_09C4 != null)
		{
			_09C4.Invoke((object)item, e);
		}
		if (item.ClickAutoRepeat && (int)e.Button == 1048576)
		{
			_৯ = item;
			if (_৮ == null)
			{
				_৮ = new Timer();
			}
			_৮.Interval = item.ClickRepeatInterval;
			_৮.Tick += _0A55;
			_৮.Start();
		}
	}

	private void _0A53(BaseItem _06EC, MouseEventArgs ؿ)
	{
		OnItemMouseDown(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeMouseDown(BaseItem _06EC, MouseEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੓
		this._0A53(_06EC, ؿ);
	}

	protected virtual void OnItemMouseUp(BaseItem item, MouseEventArgs e)
	{
		if (_09C5 != null)
		{
			_09C5.Invoke((object)item, e);
		}
		if (_৮ != null && _৮.Enabled)
		{
			_৮.Stop();
			_৮.Enabled = false;
		}
	}

	private void _0A54(BaseItem _06EC, MouseEventArgs ؿ)
	{
		OnItemMouseUp(_06EC, ؿ);
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
		OnItemClick(ਤ);
	}

	void IOwnerItemEvents.InvokeItemClick(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੘
		this._0A58(ਤ);
	}

	protected virtual void OnItemClick(BaseItem item)
	{
		GetRibbonControl()?.ට(item);
		if (ShowKeyTips)
		{
			ShowKeyTips = false;
		}
		if (_09BC != null)
		{
			_09BC(item, new EventArgs());
		}
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

	protected virtual void OnButtonCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (_09BB != null)
		{
			_09BB(item, e);
		}
	}

	private void _0A62(ButtonItem _06EC, EventArgs ؿ)
	{
		OnButtonCheckedChanged(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੢
		this._0A62(_06EC, ؿ);
	}

	private bool _0A64(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return _੨(_0A65, _੦, _੧);
	}

	bool ሎ._੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੤
		return this._0A64(_0A65, _੦, _੧);
	}

	protected virtual bool _੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	private bool _੩(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return _੪(_0A65, _੦, _੧);
	}

	bool ሎ._੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੩
		return this._੩(_0A65, _੦, _੧);
	}

	protected virtual bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected I4, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		bool designMode = GetDesignMode();
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
			return !designMode;
		}
		if (_09A9 && !designMode)
		{
			bool flag3 = true;
			Control val3 = Control.FromChildHandle(_0A65);
			if (val3 != null)
			{
				while (val3.Parent != null)
				{
					val3 = val3.Parent;
				}
				if ((val3 is MenuPanel || val3 is ItemControl || val3 is PopupContainer || val3 is PopupContainerControl) && val3.Handle != _0A65)
				{
					flag3 = false;
				}
			}
			if (flag3)
			{
				Keys val4 = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
				if ((int)val4 == 0)
				{
					val4 = (Keys)num;
				}
				_0A81(new KeyEventArgs(val4));
				return !designMode;
			}
		}
		if (!IsParentFormActive)
		{
			return false;
		}
		if (num >= 112 || (int)Control.ModifierKeys != 0 || (_0EFE._0F11(_੧) & 0x1000000000L) != 0 || num == 46 || num == 45)
		{
			int _੬ = Control.ModifierKeys | num;
			if (_੫((eShortcut)_੬))
			{
				return !designMode;
			}
			return false;
		}
		return false;
	}

	private bool _੫(eShortcut _੬)
	{
		if (!ਊ || !((Control)this).Enabled)
		{
			return false;
		}
		Form val = ((Control)this).FindForm();
		try
		{
			if (val == null || (((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)) && !val.IsMdiContainer) || (Form.ActiveForm != null && Form.ActiveForm.Modal && Form.ActiveForm != val))
			{
				return false;
			}
		}
		catch (NullReferenceException)
		{
			return false;
		}
		bool result = BarFunctions.ProcessItemsShortcuts(_੬, _09DA);
		if (!_৩)
		{
			return result;
		}
		return false;
	}

	private _20E7 _੭()
	{
		if (ਐ == null)
		{
			ਐ = new _20E7();
		}
		return ਐ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DesignerNewItemAdded()
	{
		_੭()._20ED();
	}

	protected virtual bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (ਏ.Count == 0 || GetDesignMode())
		{
			return false;
		}
		BaseItem[] array = new BaseItem[ਏ.Count];
		ਏ.CopyTo(array);
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
						if (!flag)
						{
							flag = popupItem.IsAnyOnHandle(val.Handle);
						}
					}
				}
				else
				{
					string text = _089F.स(hWnd);
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
				if (val2 != null && val2.Parent != null)
				{
					val2 = val2.Parent;
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
			if (popupItem.Displayed && !((Control)this).IsDisposed)
			{
				Point pt = ((Control)this).PointToClient(Control.MousePosition);
				if (popupItem.DisplayRectangle.Contains(pt))
				{
					break;
				}
			}
			if (popupItem.PopupControl is IKeyTipsControl)
			{
				((IKeyTipsControl)popupItem.PopupControl).ShowKeyTips = false;
			}
			if (GetDesignMode())
			{
				_੭()._20EC(popupItem);
			}
			else
			{
				popupItem.ClosePopup();
			}
			if (ਏ.Count == 0)
			{
				break;
			}
		}
		if (ਏ.Count == 0)
		{
			_09A9 = false;
		}
		return false;
	}

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return OnSysMouseDown(_0A65, _੦, _੧);
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
		return _0A71(_0A65, _੦, _੧);
	}

	bool ሎ._0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੰ
		return this._0A70(_0A65, _੦, _੧);
	}

	protected virtual bool _0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected I4, but got Unknown
		if (!GetDesignMode())
		{
			int num = _0EFE._0F11(_੦);
			if (num == 18 && (int)Control.ModifierKeys == 262144)
			{
				ClosePopups();
			}
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

	private bool ੲ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return ੳ(_0A65, _੦, _੧);
	}

	bool ሎ.ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੲ
		return this.ੲ(_0A65, _੦, _੧);
	}

	protected virtual bool ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
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
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		if (((Message)(ref m)).Msg == 7)
		{
			IntPtr wParam = ((Message)(ref m)).WParam;
			Control val = Control.FromChildHandle(wParam);
			if (val != null && !(val is Bar) && !(val is MenuPanel) && !(val is TextBoxX) && !(val is ComboBoxEx))
			{
				Form val2 = val.FindForm();
				if (val2 == ((Control)this).FindForm())
				{
					_0A03 = ((Message)(ref m)).WParam;
				}
				else if (val2 != null)
				{
					_0A03 = ((Message)(ref m)).WParam;
				}
			}
		}
		else if (((Message)(ref m)).Msg == 1131)
		{
			if (ਈ != null)
			{
				ਈ.DoAccesibleDefaultAction();
				ਈ = null;
			}
		}
		else if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		else if ((long)((Message)(ref m)).Msg == 282 && __0A11)
		{
			DevComponents.DotNetBar.Touch._0EFE.GESTURENOTIFYSTRUCT gESTURENOTIFYSTRUCT = (DevComponents.DotNetBar.Touch._0EFE.GESTURENOTIFYSTRUCT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(DevComponents.DotNetBar.Touch._0EFE.GESTURENOTIFYSTRUCT));
			Point point = ((Control)this).PointToClient(new Point(gESTURENOTIFYSTRUCT.ptsLocation.x, gESTURENOTIFYSTRUCT.ptsLocation.y));
			_09D7.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, point.X, point.Y, 0));
			_09D7.InternalMouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		((ContainerControl)this).WndProc(ref m);
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
		RecalcLayout();
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
		if (_0A0E != null)
		{
			_0A0E.Dispose();
			_0A0E = null;
		}
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Point mousePos = ((Control)this).PointToClient(Control.MousePosition);
		InternalOnClick(Control.MouseButtons, mousePos);
		((Control)this).OnClick(e);
	}

	protected virtual void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_09D7.InternalClick(mb, mousePos);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		MouseButtons mb = Control.MouseButtons;
		if (e is MouseEventArgs)
		{
			mb = ((MouseEventArgs)e).Button;
		}
		_09D7.InternalDoubleClick(mb, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_0A81(e);
		((Control)this).OnKeyDown(e);
	}

	internal virtual void _0A81(KeyEventArgs ؿ)
	{
		_09D7.InternalKeyDown(ؿ);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_09C6?.Invoke(this, e);
		((Control)this).OnMouseEnter(e);
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
		InternalMouseMove(e);
	}

	protected virtual void InternalMouseMove(MouseEventArgs e)
	{
		if (_09DE && _09E2)
		{
			if (!ৡ)
			{
				MouseDragOver(e.X, e.Y, null);
			}
		}
		else
		{
			_09D7.InternalMouseMove(e);
		}
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (_09DE && (_09E2 || _09E3))
		{
			e.Effect = (DragDropEffects)2;
			((Control)this).OnDragOver(e);
			if ((int)e.Effect != 0)
			{
				Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
				MouseDragOver(point.X, point.Y, e);
			}
			য় = false;
		}
		else
		{
			((Control)this).OnDragOver(e);
		}
	}

	protected override void OnDragLeave(EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (_09DE)
		{
			if (_09E2 || _09E3)
			{
				if ((int)Control.MouseButtons == 0)
				{
					MouseDragDrop(-1, -1, null);
				}
				else
				{
					MouseDragOver(-1, -1, null);
				}
				if (!ৡ)
				{
					((Control)this).Capture = true;
				}
			}
			য় = true;
			_09E3 = false;
		}
		((Control)this).OnDragLeave(e);
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		((Control)this).OnDragEnter(e);
		if (!_09DE)
		{
			return;
		}
		if (_09E2 && !ৠ)
		{
			BaseItem baseItem = _0A82(e);
			if (baseItem != null && baseItem.ContainerControl == this)
			{
				if ((e.AllowedEffect & 2) == 2)
				{
					e.Effect = (DragDropEffects)2;
				}
				return;
			}
		}
		if (_09E2 || !ৠ)
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
				MouseDragDrop(point.X, point.Y, e);
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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		((Control)this).OnQueryContinueDrag(e);
		if (_09DE && _09E2 && ((য় && (int)e.Action == 1) || (int)e.Action == 2))
		{
			MouseDragDrop(-1, -1, null);
		}
	}

	private BaseItem _0A82(DragEventArgs _0A83)
	{
		if (_0A83 == null)
		{
			return null;
		}
		string[] formats = _0A83.Data.GetFormats();
		string[] array = formats;
		foreach (string text in array)
		{
			if (_0A83.Data.GetData(text) is BaseItem result)
			{
				return result;
			}
		}
		return null;
	}

	private bool _0A84(object અ)
	{
		if (__0A12.Count == 0 || અ == null)
		{
			return true;
		}
		Type type = અ.GetType();
		foreach (Type item in __0A12)
		{
			if (item.Equals(type))
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void MouseDragOver(int x, int y, DragEventArgs dragArgs)
	{
		if (!_09E2 && !_09E3)
		{
			return;
		}
		BaseItem baseItem = ঢ়;
		if (dragArgs != null)
		{
			baseItem = _0A82(dragArgs);
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
		Point pScreen = ((Control)this).PointToScreen(new Point(x, y));
		InsertPosition insertPosition = ((IDesignTimeProvider)_09D7).GetInsertPosition(pScreen, baseItem);
		if (insertPosition != null)
		{
			if (insertPosition.TargetProvider == null || !_0A84(insertPosition.TargetProvider))
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
				else if (dragArgs != null)
				{
					dragArgs.Effect = (DragDropEffects)0;
				}
			}
			else
			{
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
				else if (dragArgs != null)
				{
					dragArgs.Effect = (DragDropEffects)2;
				}
			}
		}
		else if (!ৡ)
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
		else if (dragArgs != null)
		{
			dragArgs.Effect = (DragDropEffects)0;
		}
		OnMouseDragOverProcessed(x, y, dragArgs);
	}

	protected virtual void OnMouseDragOverProcessed(int x, int y, DragEventArgs dragArgs)
	{
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (((Control)this).Visible && ProcessMnemonicEx(charCode))
		{
			return true;
		}
		return ((ContainerControl)this).ProcessMnemonic(charCode);
	}

	public virtual bool ProcessMnemonicEx(char charCode)
	{
		if (ProcessMnemonic(_09D7, charCode))
		{
			return true;
		}
		return false;
	}

	protected virtual bool ProcessMnemonic(BaseItem container, char charCode)
	{
		BaseItem itemForMnemonic = GetItemForMnemonic(container, charCode, deepScan: true, stackKeys: true);
		return ProcessItemMnemonicKey(itemForMnemonic);
	}

	protected virtual bool ProcessItemMnemonicKey(BaseItem item)
	{
		if (item != null && item.Visible && item.GetEnabled())
		{
			if (item is QatOverflowItem)
			{
				((QatOverflowItem)item).Expanded = true;
			}
			else if (item is ButtonItem && (item.ShowSubItems || ((ButtonItem)item).AutoExpandOnClick) && ((item.SubItems.Count > 0 && item.VisibleSubItems > 0) || ((ButtonItem)item).PopupType == ePopupType.Container) && !item.Expanded)
			{
				item.Expanded = true;
				PopupItem popupItem = item as PopupItem;
				if (popupItem.PopupType == ePopupType.Menu && popupItem.PopupControl is MenuPanel)
				{
					((MenuPanel)(object)popupItem.PopupControl)._1030();
				}
				if (_09D7 is GenericItemContainer)
				{
					_09D7.AutoExpand = true;
				}
			}
			else if (item is ComboBoxItem)
			{
				((Control)((ComboBoxItem)item).ComboBoxEx).Focus();
				((ComboBox)((ComboBoxItem)item).ComboBoxEx).DroppedDown = true;
			}
			else if (item is TextBoxItem)
			{
				((Control)((TextBoxItem)item).TextBox).Focus();
			}
			else if (item is ControlContainerItem && ((ControlContainerItem)item).Control != null)
			{
				((ControlContainerItem)item).Control.Focus();
			}
			else if (item is GalleryContainer)
			{
				((GalleryContainer)item).PopupGallery();
			}
			else
			{
				item.RaiseClick();
			}
			return true;
		}
		return false;
	}

	protected virtual BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys)
	{
		bool ઋ = false;
		return આ(container, charCode, deepScan, stackKeys, ref ઋ);
	}

	private BaseItem આ(BaseItem ઇ, char ઈ, bool ઉ, bool ઊ, ref bool ઋ)
	{
		string text = _0A00 + ઈ;
		text = text.ToUpper();
		ᱭ ᱭ2 = ઇ as ᱭ;
		int num = ઇ.SubItems.Count;
		if (ᱭ2 != null && ᱭ2._0DBC != null && ᱭ2._0DBC.Visible)
		{
			num++;
		}
		BaseItem[] array = new BaseItem[num];
		ઇ.SubItems.CopyTo(array, 0);
		if (ᱭ2 != null && ᱭ2._0DBC != null && ᱭ2._0DBC.Visible)
		{
			array[num - 1] = ᱭ2._0DBC;
		}
		BaseItem[] array2 = array;
		foreach (BaseItem baseItem in array2)
		{
			if (baseItem.KeyTips != "" || _0A00 != "")
			{
				if (baseItem.KeyTips != "")
				{
					if (baseItem.KeyTips == text)
					{
						if (baseItem.Visible && baseItem.GetEnabled())
						{
							return baseItem;
						}
					}
					else if (baseItem.KeyTips.StartsWith(text))
					{
						ઋ = true;
					}
				}
			}
			else if (Control.IsMnemonic(ઈ, baseItem.Text) && baseItem.Visible && baseItem.GetEnabled())
			{
				return baseItem;
			}
			if (ઉ && baseItem.IsContainer)
			{
				BaseItem baseItem2 = આ(baseItem, ઈ, ઉ, ઊ: false, ref ઋ);
				if (baseItem2 != null)
				{
					return baseItem2;
				}
			}
		}
		if (ઋ && ઊ)
		{
			((IKeyTipsControl)this).KeyTipsKeysStack += ઈ.ToString().ToUpper();
		}
		return null;
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

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		BaseItem baseItemContainer = GetBaseItemContainer();
		if (baseItemContainer != null)
		{
			baseItemContainer.NotifyScaleItem(factor);
			baseItemContainer.NeedRecalcSize = true;
		}
		((ScrollableControl)this).ScaleControl(factor, specified);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (_ਓ)
		{
			return ((ContainerControl)this).ProcessDialogKey(keyData);
		}
		return false;
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (_09D7 != null)
		{
			_09D7.UpdateBindings();
		}
	}

	protected virtual RibbonControl GetRibbonControl()
	{
		Control parent = ((Control)this).Parent;
		while (parent != null && !(parent is RibbonControl))
		{
			if (parent is RibbonPanel && ((RibbonPanel)(object)parent).ᯇ() != null)
			{
				return ((RibbonPanel)(object)parent).ᯇ();
			}
			parent = parent.Parent;
		}
		return parent as RibbonControl;
	}

	public virtual BaseItem HitTest(Point p)
	{
		return HitTest(p.X, p.Y);
	}

	public virtual BaseItem HitTest(int x, int y)
	{
		return _09D7.ItemAtLocation(x, y);
	}

	private void _0A8E()
	{
		AutoSyncSize();
	}

	public virtual void AutoSyncSize()
	{
		if (!_09FB || ਔ > 2 || _09D7 == null || _09D7.NeedRecalcSize)
		{
			return;
		}
		ਔ++;
		try
		{
			if (_ਕ == eOrientation.Vertical)
			{
				int autoSizeHeight = GetAutoSizeHeight();
				if (((Control)this).Height != autoSizeHeight && _09D7.HeightInternal > 0)
				{
					((Control)this).Height = autoSizeHeight;
					RecalcSize();
				}
			}
			else
			{
				int autoSizeWidth = GetAutoSizeWidth();
				if (((Control)this).Width != autoSizeWidth && _09D7.WidthInternal > 0)
				{
					((Control)this).Width = autoSizeWidth;
					RecalcSize();
				}
			}
		}
		finally
		{
			ਔ--;
		}
	}

	protected virtual ElementStyle GetBackgroundStyle()
	{
		return _0599;
	}

	internal ElementStyle એ()
	{
		return GetBackgroundStyle();
	}

	public virtual int GetAutoSizeHeight()
	{
		return _09D7.HeightInternal + ᝈ.ᝏ(GetBackgroundStyle());
	}

	public virtual int GetAutoSizeWidth()
	{
		return _09D7.WidthInternal + ᝈ.ᝍ(GetBackgroundStyle());
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

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_৮ != null)
			{
				_৮.Stop();
				((Component)(object)_৮).Dispose();
				_৮ = null;
			}
			if (ਐ != null)
			{
				ਐ._1A17();
				ਐ = null;
			}
			if (_09D7 != null)
			{
				_09D7.Dispose();
			}
			if (գ != null)
			{
				((Component)(object)գ).Disposed -= ٶ;
			}
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed -= ٶ;
			}
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed -= ٶ;
			}
		}
		((ContainerControl)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_0599.StyleChanged -= ઐ;
		_0599 = new ElementStyle();
		_0599.StyleChanged += ઐ;
		((Control)this).Invalidate();
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		OnVisualPropertyChanged();
	}

	protected virtual void OnVisualPropertyChanged()
	{
		if (GetDesignMode() || (((Control)this).Parent != null && ((Component)(object)((Control)this).Parent).Site != null && ((Component)(object)((Control)this).Parent).Site.DesignMode))
		{
			RecalcLayout();
		}
	}

	protected virtual void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
	{
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		if (!_09E2 && !_09E3)
		{
			return;
		}
		BaseItem baseItem = ঢ়;
		if (baseItem == null)
		{
			baseItem = _0A82(dragArgs);
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
						parent.Refresh();
						if (parent.ContainerControl != this)
						{
							object containerControl = parent.ContainerControl;
							BarUtilities.ᆞ((Control)((containerControl is Control) ? containerControl : null));
						}
					}
					_৫.InsertItemAt(baseItem, _৬, _৭);
					OnItemDragAndDropped(baseItem);
					_৫ = null;
					((IOwner)this).InvokeUserCustomize((object)baseItem, new EventArgs());
				}
			}
		}
		_৫ = null;
		DragInProgress = false;
		_09E3 = false;
		if (!ৡ)
		{
			Cursor.Current = Cursors.Default;
		}
		((Control)this).Capture = false;
		if (baseItem != null)
		{
			baseItem._IgnoreClick = true;
			baseItem.DragStartPoint = Point.Empty;
		}
		_09D7.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, x, y, 0));
		if (baseItem != null)
		{
			baseItem._IgnoreClick = false;
		}
		ঢ় = null;
		OnUserCustomize(EventArgs.Empty);
	}

	protected virtual void OnUserCustomize(EventArgs e)
	{
		if (_09CC != null)
		{
			_09CC(this, e);
		}
	}

	protected virtual void OnItemDragAndDropped(BaseItem dragItem)
	{
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

	protected virtual void OnShowKeyTipsChanged()
	{
		_0A00 = "";
		if (ShowKeyTips)
		{
			CreateKeyTipCanvas();
		}
		else
		{
			DestroyKeyTipCanvas();
		}
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

	internal virtual ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.DesignerSelection = ৼ;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && ষ;
		itemPaintArgs.DragInProgress = DragInProgress;
		itemPaintArgs._0EE1 = _ਗ;
		if (_ਗ)
		{
			itemPaintArgs._0EE2 = new List<ခ>();
		}
		_ਗ = false;
		return itemPaintArgs;
	}

	internal ColorScheme ઓ()
	{
		return GetColorScheme();
	}

	protected virtual ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(_09D7.EffectiveStyle))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
			}
		}
		return լ;
	}

	internal void ઔ(PaintEventArgs ؿ)
	{
		ઘ(ؿ, ઙ: true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		ઘ(e, ઙ: false);
		if (_0A01 != null)
		{
			ગ();
			ક();
		}
		((Control)this).OnPaint(e);
	}

	private void ક()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (ਘ == null)
		{
			ਘ = new Timer();
			ਘ.Tick += ખ;
			ਘ.Interval = 100;
			ਘ.Start();
		}
	}

	private void ખ(object _٠, EventArgs ؿ)
	{
		Timer val = ਘ;
		ਘ = null;
		val.Stop();
		val.Tick -= ખ;
		((Component)(object)val).Dispose();
		ગ();
	}

	internal void ગ()
	{
		if (_0A01 != null)
		{
			((Control)_0A01).Invalidate();
		}
	}

	private void ઘ(PaintEventArgs ؿ, bool ઙ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = ؿ.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = ؿ.Graphics.TextRenderingHint;
		if (((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent)
		{
			((ScrollableControl)this).OnPaintBackground(ؿ);
		}
		if (_0599 != null)
		{
			_0599.SetColorScheme(GetColorScheme());
		}
		ItemPaintArgs itemPaintArgs = _0A92(ؿ.Graphics);
		itemPaintArgs.PaintEventArgs = ؿ;
		itemPaintArgs.ClipRectangle = ؿ.ClipRectangle;
		if (_ਚ)
		{
			itemPaintArgs._0EE1 = true;
			itemPaintArgs._0EE2 = new List<ခ>();
		}
		if (ਙ && ((ScrollableControl)this).AutoScroll && ((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			itemPaintArgs.ClipRectangle = Rectangle.Empty;
			ਙ = false;
		}
		itemPaintArgs.ໟ = ઙ;
		PaintControl(itemPaintArgs);
		ؿ.Graphics.SmoothingMode = smoothingMode;
		ؿ.Graphics.TextRenderingHint = textRenderingHint;
		if (itemPaintArgs._0EE1 && itemPaintArgs._0EE2.Count > 0)
		{
			_20EE obj = new _20EE(itemPaintArgs._0EE2.ToArray(), AnimationEasing.EaseOutExpo, 500);
			obj.AnimationUpdateControl = (Control)(object)this;
			_ਚ = false;
			obj.AnimationCompleted += ચ;
			obj.Start();
		}
	}

	private void ચ(object _٠, EventArgs ؿ)
	{
		_1A18 obj = (_1A18)_٠;
		obj.AnimationCompleted -= ચ;
		obj.Dispose();
	}

	protected virtual void ClearBackground(ItemPaintArgs pa)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			pa.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void PaintControl(ItemPaintArgs pa)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (_09D7 == null || ((Control)this).IsDisposed)
		{
			return;
		}
		if (_09D7.NeedRecalcSize)
		{
			RecalcSize();
		}
		if (!((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
		{
			ClearBackground(pa);
		}
		if (_089F.फ())
		{
			pa.Graphics.DrawString("Your DotNetBar trial has expired :-(", ((Control)this).Font, SystemBrushes.ControlText, 0f, 0f);
			return;
		}
		if (ե)
		{
			pa.Graphics.SmoothingMode = (SmoothingMode)4;
			pa.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		PaintControlBackground(pa);
		Region clip = pa.Graphics.Clip;
		pa.ContainerClipRectangle = GetInnerPaintBounds();
		pa.Graphics.SetClip(pa.ContainerClipRectangle, (CombineMode)1);
		PaintItemContainer(pa);
		pa.Graphics.Clip = clip;
		if (clip != null)
		{
			clip.Dispose();
		}
		if (_09D7.SubItems.Count == 0 && GetDesignMode())
		{
			PaintDesignTimeEmptyHint(pa);
		}
	}

	protected virtual Rectangle GetInnerPaintBounds()
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (backgroundStyle == null)
		{
			return ((Control)this).ClientRectangle;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ᝈ._1755(backgroundStyle, eSpacePart.Border, eStyleSide.Left);
		clientRectangle.Width -= ᝈ._1755(backgroundStyle, eSpacePart.Border, eStyleSide.Left) + ᝈ._1755(backgroundStyle, eSpacePart.Border, eStyleSide.Right);
		clientRectangle.Y += ᝈ._1755(backgroundStyle, eSpacePart.Border, eStyleSide.Top);
		clientRectangle.Height -= ᝈ._1755(backgroundStyle, eSpacePart.Border, eStyleSide.Top) + ᝈ._1755(backgroundStyle, eSpacePart.Border, eStyleSide.Bottom);
		return clientRectangle;
	}

	protected virtual Rectangle GetPaintClipRectangle()
	{
		return GetItemContainerRectangle();
	}

	protected virtual Rectangle GetPaintControlBackgroundRectangle()
	{
		return ((Control)this).ClientRectangle;
	}

	protected virtual void PaintControlBackground(ItemPaintArgs pa)
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (backgroundStyle != null)
		{
			Rectangle paintControlBackgroundRectangle = GetPaintControlBackgroundRectangle();
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(backgroundStyle, pa.Graphics, paintControlBackgroundRectangle);
			ElementStyleDisplay.Paint(e);
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
			StringFormat val = BarFunctions.CreateStringFormat();
			val.Alignment = (StringAlignment)1;
			val.LineAlignment = (StringAlignment)1;
			pa.Graphics.DrawString(text, ((Control)this).Font, SystemBrushes.ControlDark, (RectangleF)itemContainerRectangle, val);
		}
	}

	protected virtual void છ(Graphics _٥)
	{
		if (_09FE)
		{
			KeyTipsRendererEventArgs ؿ = new KeyTipsRendererEventArgs(_٥, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer renderer = GetRenderer();
			જ(_09D7, renderer, ؿ);
		}
	}

	protected virtual Font GetKeyTipFont()
	{
		Font result = ((Control)this).Font;
		if (_09FF != null)
		{
			result = _09FF;
		}
		return result;
	}

	internal virtual void જ(BaseItem ઇ, BaseRenderer ઝ, KeyTipsRendererEventArgs ؿ)
	{
		foreach (BaseItem subItem in ઇ.SubItems)
		{
			ઞ(subItem, ઝ, ؿ);
		}
		if (ઇ is ᱭ { _0DBC: not null } ᱭ2 && ᱭ2._0DBC.Visible)
		{
			ઞ(ᱭ2._0DBC, ઝ, ؿ);
		}
	}

	private void ઞ(BaseItem _06EC, BaseRenderer ઝ, KeyTipsRendererEventArgs ؿ)
	{
		if (!_06EC.Visible || !_06EC.Displayed)
		{
			return;
		}
		if (_06EC.IsContainer)
		{
			જ(_06EC, ઝ, ؿ);
		}
		if ((_06EC.AccessKey != 0 || !(_06EC.KeyTips == "")) && (!(_0A00 != "") || _06EC.KeyTips.StartsWith(_0A00)) && (!(_06EC.KeyTips == "") || !(_0A00 != "")))
		{
			if (_06EC.KeyTips != "")
			{
				ؿ.KeyTip = _06EC.KeyTips;
			}
			else
			{
				ؿ.KeyTip = _06EC.AccessKey.ToString().ToUpper();
			}
			ؿ.Bounds = GetKeyTipRectangle(ؿ.Graphics, _06EC, ؿ.Font, ؿ.KeyTip);
			ؿ.ReferenceObject = _06EC;
			ઝ.DrawKeyTips(ؿ);
		}
	}

	protected virtual Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Size _၂ = _၁._၂;
		Size size = TextDrawing.MeasureString(g, keyTip, font);
		size.Width += _၂.Width;
		size.Height += _၂.Height;
		Rectangle displayRectangle = item.DisplayRectangle;
		Rectangle result = new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Bottom - size.Height, size.Width, size.Height);
		if (item is RibbonTabItem)
		{
			result.Y = displayRectangle.Bottom - 7;
		}
		return result;
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

	protected virtual Rectangle GetKeyTipCanvasBounds()
	{
		if (((Control)this).Parent != null)
		{
			return new Rectangle(((Control)this).Bounds.X, ((Control)this).Bounds.Y, ((Control)this).Bounds.Width, ((Control)this).Bounds.Height + 16);
		}
		return new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
	}

	protected virtual void CreateKeyTipCanvas()
	{
		if (_0A01 != null)
		{
			((Control)_0A01).BringToFront();
			if (((Control)_0A01).Parent != null)
			{
				((Control)_0A01).Parent.Invalidate(((Control)_0A01).Bounds);
			}
			return;
		}
		_0A01 = new _103E(this);
		((Control)_0A01).Bounds = GetKeyTipCanvasBounds();
		((Control)_0A01).Visible = true;
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			if (ribbonControl.Expanded && !NeedsTopLevelKeyTipCanvasParent)
			{
				((Control)ribbonControl).Controls.Add((Control)(object)_0A01);
			}
			else
			{
				Form val = ((Control)ribbonControl).FindForm();
				if (val != null)
				{
					((Control)val).Controls.Add((Control)(object)_0A01);
				}
				else
				{
					((Control)ribbonControl).Controls.Add((Control)(object)_0A01);
				}
			}
		}
		else
		{
			((Control)this).Controls.Add((Control)(object)_0A01);
		}
		((Control)_0A01).BringToFront();
	}

	internal void ટ()
	{
		if (_0A01 != null)
		{
			((Control)_0A01).Refresh();
		}
	}

	protected virtual void DestroyKeyTipCanvas()
	{
		if (_0A01 != null)
		{
			((Control)_0A01).Visible = false;
			if (((Control)_0A01).Parent != null)
			{
				((Control)_0A01).Parent.Controls.Remove((Control)(object)_0A01);
			}
			((Component)(object)_0A01).Dispose();
			_0A01 = null;
		}
	}

	private void ઠ(Graphics _٥)
	{
		છ(_٥);
	}

	void _103D.છ(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ઠ
		this.ઠ(_٥);
	}

	public void BeginUpdate()
	{
		ਝ++;
	}

	public void EndUpdate()
	{
		EndUpdate(callRecalcLayout: true);
	}

	public void EndUpdate(bool callRecalcLayout)
	{
		if (ਝ > 0)
		{
			ਝ--;
			if (ਝ == 0 && callRecalcLayout)
			{
				RecalcLayout();
			}
		}
	}

	protected virtual Rectangle GetItemContainerRectangle()
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (backgroundStyle == null)
		{
			return ((Control)this).ClientRectangle;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ᝈ.ᝐ(backgroundStyle);
		clientRectangle.Width -= ᝈ.ᝍ(backgroundStyle);
		clientRectangle.Y += ᝈ._1753(backgroundStyle);
		clientRectangle.Height -= ᝈ.ᝏ(backgroundStyle);
		return clientRectangle;
	}

	protected virtual void RecalcSize()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		if (!BarFunctions.IsHandleValid((Control)(object)this) || IsUpdateSuspended || _09D7.IsRecalculatingSize)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val == null || (int)val.WindowState != 1)
		{
			if (_09D7.NeedRecalcSize)
			{
				Rectangle itemContainerRectangle = GetItemContainerRectangle();
				_09D7.SuspendLayout = true;
				_09D7.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
				_09D7.LeftInternal = itemContainerRectangle.X;
				_09D7.TopInternal = itemContainerRectangle.Y;
				_09D7.WidthInternal = itemContainerRectangle.Width;
				_09D7.HeightInternal = itemContainerRectangle.Height;
				_09D7.SuspendLayout = false;
				_09D7.RecalcSize();
				OnItemLayoutUpdated(EventArgs.Empty);
			}
			AutoSyncSize();
		}
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		ਞ?.Invoke(this, e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		ડ();
		InvalidateLayout();
		RecalcLayout();
		((ContainerControl)this).OnFontChanged(e);
	}

	private void ડ()
	{
		BarUtilities.ડ(_09D7.SubItems);
	}

	public void InvalidateLayout()
	{
		_09D7.NeedRecalcSize = true;
	}

	public virtual void RecalcLayout()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		MethodInvoker val = null;
		if (((Control)this).InvokeRequired)
		{
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					RecalcLayout();
				};
			}
			((Control)this).BeginInvoke((Delegate)(object)val);
		}
		else if (!_09D7.IsRecalculatingSize && !IsUpdateSuspended)
		{
			RecalcSize();
			((Control)this).Invalidate();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			Form val = ((Control)this).FindForm();
			if (val == null || (int)val.WindowState != 1)
			{
				_09D7.NeedRecalcSize = true;
				RecalcSize();
			}
		}
	}

	protected virtual void SetupActiveWindowTimer()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_0A04 == null)
		{
			_0A04 = new Timer();
			_0A04.Interval = 100;
			_0A04.Tick += ણ;
			ਅ = _089F.GetForegroundWindow();
			ਆ = _089F.GetActiveWindow();
			_0A04.Start();
		}
	}

	private void ણ(object _٠, EventArgs ؿ)
	{
		if (_0A04 == null)
		{
			return;
		}
		IntPtr foregroundWindow = _089F.GetForegroundWindow();
		IntPtr activeWindow = _089F.GetActiveWindow();
		if (!(foregroundWindow != ਅ) && !(activeWindow != ਆ))
		{
			return;
		}
		if (activeWindow != IntPtr.Zero)
		{
			Control val = Control.FromHandle(activeWindow);
			if (val is PopupContainer || val is PopupContainerControl)
			{
				return;
			}
		}
		_0A04.Stop();
		OnActiveWindowChanged();
	}

	protected virtual void OnActiveWindowChanged()
	{
		if (_09A9)
		{
			_09A9 = false;
		}
	}

	protected virtual void ReleaseActiveWindowTimer()
	{
		if (_0A04 != null)
		{
			Timer val = _0A04;
			_0A04 = null;
			val.Stop();
			val.Tick -= ણ;
			((Component)(object)val).Dispose();
		}
	}

	private void ત(BaseItem _06EC)
	{
		OnPopupItemRightClick(_06EC);
	}

	void IRibbonCustomize.ItemRightClick(BaseItem _06EC)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ત
		this.ત(_06EC);
	}

	protected virtual void OnPopupItemRightClick(BaseItem item)
	{
	}

	[CompilerGenerated]
	private void થ()
	{
		RecalcLayout();
	}
}
