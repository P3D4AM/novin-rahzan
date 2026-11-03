using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Events;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[DefaultEvent("ItemClick")]
[ToolboxBitmap(typeof(ToolboxIconResFinder), "RadialMenu.ico")]
[ComVisible(false)]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.RadialMenuDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class RadialMenu : Control, IOwner, IOwnerItemEvents, ሎ
{
	public class RadialMenuAccessibleObject : ControlAccessibleObject
	{
		private RadialMenu __2487;

		public override AccessibleRole Role
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				if (__2487 == null || ((Control)__2487).IsDisposed)
				{
					return (AccessibleRole)0;
				}
				return ((Control)__2487).AccessibleRole;
			}
		}

		public override AccessibleObject Parent
		{
			get
			{
				if (__2487 != null && !((Control)__2487).IsDisposed)
				{
					return ((Control)__2487).Parent.AccessibilityObject;
				}
				return null;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (__2487 != null && !((Control)__2487).IsDisposed && ((Control)__2487).Parent != null)
				{
					return ((Control)__2487).Parent.RectangleToScreen(((Control)__2487).Bounds);
				}
				return Rectangle.Empty;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				AccessibleStates result = (AccessibleStates)256;
				if (__2487 == null || ((Control)__2487).IsDisposed)
				{
					return (AccessibleStates)0;
				}
				if (((Control)__2487).Focused)
				{
					result = (AccessibleStates)4;
				}
				if (__2487.IsOpen)
				{
					result = (AccessibleStates)512;
				}
				return result;
			}
		}

		public override string DefaultAction
		{
			get
			{
				if (!string.IsNullOrEmpty(((Control)__2487).AccessibleDefaultActionDescription))
				{
					return ((Control)__2487).AccessibleDefaultActionDescription;
				}
				return "Open";
			}
		}

		public override string Description
		{
			get
			{
				if (!string.IsNullOrEmpty(((Control)__2487).AccessibleDescription))
				{
					return ((Control)__2487).AccessibleDescription;
				}
				return "Radial Menu";
			}
		}

		public override string Help
		{
			get
			{
				if (!string.IsNullOrEmpty(__2487.AccessibleHelp))
				{
					return __2487.AccessibleHelp;
				}
				return "";
			}
		}

		public override string Name
		{
			get
			{
				return ((Control)__2487).Name;
			}
			set
			{
				((Control)__2487).Name = value;
			}
		}

		public override string Value
		{
			get
			{
				return ((Control)__2487).Text;
			}
			set
			{
				((Control)__2487).Text = value;
			}
		}

		public override string KeyboardShortcut => "";

		public RadialMenuAccessibleObject(RadialMenu owner)
			: base((Control)(object)owner)
		{
			__2487 = owner;
		}

		internal void _၈(BaseItem _٠, AccessibleEvents ؿ)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			int num = __2487.Items.IndexOf(_٠);
			if (num >= 0 && __2487 != null && !((Control)__2487).IsDisposed)
			{
				((Control)__2487).AccessibilityNotifyClients(ؿ, num);
			}
		}

		internal void _၈(AccessibleEvents ؿ)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (__2487 != null)
			{
				((Control)__2487).AccessibilityNotifyClients(ؿ, -1);
			}
		}

		public override int GetChildCount()
		{
			if (__2487 != null && !((Control)__2487).IsDisposed && __2487.Items != null)
			{
				return __2487.Items.Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (__2487 != null && !((Control)__2487).IsDisposed && __2487.Items != null)
			{
				return __2487.Items[iIndex].AccessibleObject;
			}
			return null;
		}

		public override void DoDefaultAction()
		{
			if (__2487 != null && !((Control)__2487).IsDisposed)
			{
				__2487.IsOpen = !__2487.IsOpen;
			}
		}
	}

	private RadialMenuContainer __2465;

	private bool __05F5;

	private CancelableEventSourceHandler _2466;

	private CancelableEventSourceHandler _2467;

	private EventHandler _2468;

	private EventHandler _2469;

	private Image _ነ;

	private bool __20D1;

	private Size _ᨀ = Size.Empty;

	private bool __19DA;

	private bool __246A = true;

	private bool __246B = true;

	private bool __246C = true;

	private bool __0BC6;

	private bool __246D;

	private ᒺ __246E;

	private bool __246F;

	private bool __2470 = true;

	private string __2471 = "";

	private bool __2472;

	private bool __2473;

	private _20E7 ਐ;

	private ArrayList _ᓓ = new ArrayList();

	private BaseItem __2474;

	private BaseItem __1B34;

	private Hashtable __2475 = new Hashtable();

	private bool _ו = true;

	private bool __2476;

	private bool __2477;

	private bool __2478 = true;

	private EventHandler ৎ;

	private ItemControl.ItemRemovedEventHandler _09CD;

	private EventHandler _2479;

	private EventHandler _247A;

	private EventHandler _247B;

	private MouseEventHandler _247C;

	private MouseEventHandler _247D;

	private MouseEventHandler _247E;

	private EventHandler _09BC;

	private MouseEventHandler ঽ;

	private EventHandler _247F;

	private EventHandler _09D0;

	private EventHandler _2480;

	[Category("Appearance")]
	[Description("Specifies the width of the sub-menu edge around the radial menu.")]
	[DefaultValue(18)]
	public int SubMenuEdgeWidth
	{
		get
		{
			return __2465.SubMenuEdgeWidth;
		}
		set
		{
			__2465.SubMenuEdgeWidth = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates spacing between sub-menu marking edge and the item.")]
	[DefaultValue(3)]
	public int SubMenuEdgeItemSpacing
	{
		get
		{
			return __2465.SubMenuEdgeItemSpacing;
		}
		set
		{
			__2465.SubMenuEdgeItemSpacing = value;
		}
	}

	[Description("Radial menu diameter.")]
	[Category("Appearance")]
	[DefaultValue(180)]
	public int Diameter
	{
		get
		{
			return __2465.Diameter;
		}
		set
		{
			__2465.Diameter = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets reference to colors used by the radial menu.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RadialMenuColorTable Colors => __2465.Colors;

	[DefaultValue(18)]
	[Description("Indicates the size of the back symbol that is displayed on center of radial menu")]
	[Browsable(false)]
	public int BackButtonSymbolSize
	{
		get
		{
			return __2465.BackButtonSymbolSize;
		}
		set
		{
			__2465.BackButtonSymbolSize = value;
		}
	}

	[Category("Appearance")]
	[Description("Specifies the back button symbol.")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("\uf060")]
	public string BackButtonSymbol
	{
		get
		{
			return __2465.BackButtonSymbol;
		}
		set
		{
			__2465.BackButtonSymbol = value;
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet BackButtonSymbolSet
	{
		get
		{
			return __2465.BackButtonSymbolSet;
		}
		set
		{
			__2465.BackButtonSymbolSet = value;
		}
	}

	[DefaultValue(32)]
	[Description("Indicates diameter of center button of radial menu.")]
	[Category("Appearance")]
	public int CenterButtonDiameter
	{
		get
		{
			return __2465.CenterButtonDiameter;
		}
		set
		{
			__2465.CenterButtonDiameter = value;
		}
	}

	[DefaultValue(90)]
	[Description("Specifies the maximum pie part angle an item will occupy. Maximum is 180, minimum is 1 degree.")]
	[Category("Appearance")]
	public int MaxItemPieAngle
	{
		get
		{
			return __2465.MaxItemPieAngle;
		}
		set
		{
			__2465.MaxItemPieAngle = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates maximum radial angle single item in menu can consume. Max value is 180.")]
	[Category("Appearance")]
	public int MaxItemRadialAngle
	{
		get
		{
			return __2465.MaxItemRadialAngle;
		}
		set
		{
			__2465.MaxItemRadialAngle = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Behavior")]
	[Browsable(false)]
	[Description("Indicates the position of top-left corner of radial menu when shown in screen coordinates")]
	public Point MenuLocation
	{
		get
		{
			return __2465.MenuLocation;
		}
		set
		{
			__2465.MenuLocation = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates image displayed in center of the radial menu.")]
	[Category("Appearance")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (value != _ነ)
			{
				Image ነ = _ነ;
				_ነ = value;
				OnImageChanged(ነ, value);
			}
		}
	}

	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("Indicates image displayed in center of the radial menu.  When set, it overrides any Image settings.")]
	public string Symbol
	{
		get
		{
			return __2465.Symbol;
		}
		set
		{
			__2465.Symbol = value;
			_ᨀ = Size.Empty;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return __2465.SymbolSet;
		}
		set
		{
			__2465.SymbolSet = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	[DefaultValue(0f)]
	public float SymbolSize
	{
		get
		{
			return __2465.SymbolSize;
		}
		set
		{
			__2465.SymbolSize = value;
			_ᨀ = Size.Empty;
			((Control)this).Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsOpen
	{
		get
		{
			return __20D1;
		}
		set
		{
			if (value != __20D1)
			{
				SetIsOpen(value, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => __2465.SubItems;

	[Description("Indicates whether border, a circle is rendered around the selected symbol which represents radial menu.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool SymbolBorderEnabled
	{
		get
		{
			return __246A;
		}
		set
		{
			__246A = value;
			((Control)this).Invalidate();
		}
	}

	protected override Size DefaultSize => new Size(28, 28);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShortcutsEnabled
	{
		get
		{
			return __246B;
		}
		set
		{
			__246B = value;
		}
	}

	[Category("Behavior")]
	[Browsable(false)]
	[DefaultValue(true)]
	[Description("Indicates whether radial menu is closed automatically when app is deactivated")]
	public bool CloseMenuOnAppDeactivate
	{
		get
		{
			return __246C;
		}
		set
		{
			__246C = value;
		}
	}

	[Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(false)]
	public bool UseHook
	{
		get
		{
			return __0BC6;
		}
		set
		{
			__0BC6 = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(eRadialMenuType.Segment)]
	[Description("Indicates the radial menu type.")]
	public eRadialMenuType MenuType
	{
		get
		{
			return __2465.MenuType;
		}
		set
		{
			__2465.MenuType = value;
		}
	}

	[Description("Indicates whether control automatically scales the items if the parent Form performs scaling due to AutoScale settings.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool AutoScale
	{
		get
		{
			return __2470;
		}
		set
		{
			__2470 = value;
			_2485();
		}
	}

	[Description("Indicates accessibility help string")]
	[Category("Accessibility")]
	[DefaultValue("")]
	public string AccessibleHelp
	{
		get
		{
			return __2471;
		}
		set
		{
			__2471 = value;
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
	public bool HasRegisteredPopups => __2465.Expanded;

	[Category("Behavior")]
	[Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
	[Browsable(false)]
	[DefaultValue(false)]
	public bool DispatchShortcuts
	{
		get
		{
			return __2473;
		}
		set
		{
			__2473 = value;
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

	ImageList IOwner._2464
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	ImageList IOwner.ᇣ
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	ImageList IOwner.ᇤ
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	BaseItem IOwner.ଛ
	{
		get
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

	bool IOwner.ଜ
	{
		get
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

	[Description("Indicates whether items show tooltips.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ShowToolTips
	{
		get
		{
			return _ו;
		}
		set
		{
			_ו = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether item shortcut is displayed in Tooltips.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public virtual bool ShowShortcutKeysInToolTips
	{
		get
		{
			return __2476;
		}
		set
		{
			__2476 = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether accelerator letters for buttons are underlined regardless of current Windows settings.")]
	[Browsable(true)]
	public bool AlwaysDisplayKeyAccelerators
	{
		get
		{
			return __2477;
		}
		set
		{
			if (__2477 != value)
			{
				__2477 = value;
				((Control)this).Invalidate();
			}
		}
	}

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

	bool IOwner.ঞ
	{
		get
		{
			return false;
		}
		set
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

	bool IOwner.জ => ((Component)this).DesignMode;

	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool DisabledImagesGrayScale
	{
		get
		{
			return __2478;
		}
		set
		{
			__2478 = value;
		}
	}

	[Description("Occurs before menu is closed and allows you to cancel closing.")]
	public event CancelableEventSourceHandler BeforeMenuClose
	{
		add
		{
			CancelableEventSourceHandler cancelableEventSourceHandler = _2466;
			CancelableEventSourceHandler cancelableEventSourceHandler2;
			do
			{
				cancelableEventSourceHandler2 = cancelableEventSourceHandler;
				CancelableEventSourceHandler value2 = (CancelableEventSourceHandler)Delegate.Combine(cancelableEventSourceHandler2, value);
				cancelableEventSourceHandler = Interlocked.CompareExchange(ref _2466, value2, cancelableEventSourceHandler2);
			}
			while ((object)cancelableEventSourceHandler != cancelableEventSourceHandler2);
		}
		remove
		{
			CancelableEventSourceHandler cancelableEventSourceHandler = _2466;
			CancelableEventSourceHandler cancelableEventSourceHandler2;
			do
			{
				cancelableEventSourceHandler2 = cancelableEventSourceHandler;
				CancelableEventSourceHandler value2 = (CancelableEventSourceHandler)Delegate.Remove(cancelableEventSourceHandler2, value);
				cancelableEventSourceHandler = Interlocked.CompareExchange(ref _2466, value2, cancelableEventSourceHandler2);
			}
			while ((object)cancelableEventSourceHandler != cancelableEventSourceHandler2);
		}
	}

	[Description("Occurs before menu has been opened and allows you to cancel opening.")]
	public event CancelableEventSourceHandler BeforeMenuOpen
	{
		add
		{
			CancelableEventSourceHandler cancelableEventSourceHandler = _2467;
			CancelableEventSourceHandler cancelableEventSourceHandler2;
			do
			{
				cancelableEventSourceHandler2 = cancelableEventSourceHandler;
				CancelableEventSourceHandler value2 = (CancelableEventSourceHandler)Delegate.Combine(cancelableEventSourceHandler2, value);
				cancelableEventSourceHandler = Interlocked.CompareExchange(ref _2467, value2, cancelableEventSourceHandler2);
			}
			while ((object)cancelableEventSourceHandler != cancelableEventSourceHandler2);
		}
		remove
		{
			CancelableEventSourceHandler cancelableEventSourceHandler = _2467;
			CancelableEventSourceHandler cancelableEventSourceHandler2;
			do
			{
				cancelableEventSourceHandler2 = cancelableEventSourceHandler;
				CancelableEventSourceHandler value2 = (CancelableEventSourceHandler)Delegate.Remove(cancelableEventSourceHandler2, value);
				cancelableEventSourceHandler = Interlocked.CompareExchange(ref _2467, value2, cancelableEventSourceHandler2);
			}
			while ((object)cancelableEventSourceHandler != cancelableEventSourceHandler2);
		}
	}

	[Description("Occurs after menu has been opened.")]
	public event EventHandler MenuOpened
	{
		add
		{
			EventHandler eventHandler = _2468;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2468, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2468;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2468, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after menu is closed.")]
	public event EventHandler MenuClosed
	{
		add
		{
			EventHandler eventHandler = _2469;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2469, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2469;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2469, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after an item has been added to items collection.")]
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

	[Description("Occurs after an item has been removed from items collection.")]
	public event ItemControl.ItemRemovedEventHandler ItemRemoved
	{
		add
		{
			ItemControl.ItemRemovedEventHandler itemRemovedEventHandler = _09CD;
			ItemControl.ItemRemovedEventHandler itemRemovedEventHandler2;
			do
			{
				itemRemovedEventHandler2 = itemRemovedEventHandler;
				ItemControl.ItemRemovedEventHandler value2 = (ItemControl.ItemRemovedEventHandler)Delegate.Combine(itemRemovedEventHandler2, value);
				itemRemovedEventHandler = Interlocked.CompareExchange(ref _09CD, value2, itemRemovedEventHandler2);
			}
			while ((object)itemRemovedEventHandler != itemRemovedEventHandler2);
		}
		remove
		{
			ItemControl.ItemRemovedEventHandler itemRemovedEventHandler = _09CD;
			ItemControl.ItemRemovedEventHandler itemRemovedEventHandler2;
			do
			{
				itemRemovedEventHandler2 = itemRemovedEventHandler;
				ItemControl.ItemRemovedEventHandler value2 = (ItemControl.ItemRemovedEventHandler)Delegate.Remove(itemRemovedEventHandler2, value);
				itemRemovedEventHandler = Interlocked.CompareExchange(ref _09CD, value2, itemRemovedEventHandler2);
			}
			while ((object)itemRemovedEventHandler != itemRemovedEventHandler2);
		}
	}

	[Description("Occurs when mouse pointer enters boundaries of an item.")]
	public event EventHandler ItemMouseEnter
	{
		add
		{
			EventHandler eventHandler = _2479;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2479, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2479;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2479, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse pointer hovers over an item.")]
	public event EventHandler ItemMouseHover
	{
		add
		{
			EventHandler eventHandler = _247A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _247A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _247A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _247A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse pointer leaves boundaries of an item.")]
	public event EventHandler ItemMouseLeave
	{
		add
		{
			EventHandler eventHandler = _247B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _247B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _247B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _247B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse button is pressed on item.")]
	public event MouseEventHandler ItemMouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _247C;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _247C, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _247C;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _247C, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse button is released on item.")]
	public event MouseEventHandler ItemMouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _247D;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _247D, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _247D;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _247D, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse moves over an item.")]
	public event MouseEventHandler ItemMouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _247E;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _247E, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _247E;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _247E, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when an item is clicked.")]
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

	[Description("Occurs when an item is double-clicked.")]
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

	[Description("Occurs when an item Expanded property value has changed.")]
	public event EventHandler ItemExpandedChanged
	{
		add
		{
			EventHandler eventHandler = _247F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _247F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _247F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _247F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when an item Text property value has changed.")]
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

	[Description("Occurs when tooltip for an item is about to be displayed.")]
	public event EventHandler ItemTooltipShowing
	{
		add
		{
			EventHandler eventHandler = _2480;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2480, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2480;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2480, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public RadialMenu()
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		__2465 = new RadialMenuContainer();
		__2465._19E1 = this;
		__2465.Opened += _2483;
		__2465.Closed += _2482;
		((Control)this).SetStyle((ControlStyles)(0x2016 | DisplayHelp.DoubleBufferFlag | 0x800), true);
		__2465.SetOwner(this);
	}

	protected override void Dispose(bool disposing)
	{
		__2465.Dispose();
		((Control)this).Dispose(disposing);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		__05F5 = true;
		return (AccessibleObject)(object)new RadialMenuAccessibleObject(this);
	}

	protected virtual void OnBeforeMenuClose(CancelableEventSourceArgs e)
	{
		_2466?.Invoke(this, e);
	}

	protected virtual void OnBeforeMenuOpen(CancelableEventSourceArgs e)
	{
		_2467?.Invoke(this, e);
	}

	protected virtual void OnMenuOpened(EventArgs e)
	{
		_2468?.Invoke(this, e);
	}

	protected virtual void OnMenuClosed(EventArgs e)
	{
		_2469?.Invoke(this, e);
	}

	protected virtual void OnImageChanged(Image oldValue, Image newValue)
	{
		((Control)this).Invalidate();
	}

	protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
	{
	}

	public void SetIsOpen(bool isOpen, eEventSource source)
	{
		if (isOpen == __20D1)
		{
			return;
		}
		if (isOpen)
		{
			CancelableEventSourceArgs cancelableEventSourceArgs = new CancelableEventSourceArgs(source);
			OnBeforeMenuOpen(cancelableEventSourceArgs);
			if (cancelableEventSourceArgs.Cancel)
			{
				return;
			}
		}
		else
		{
			CancelableEventSourceArgs cancelableEventSourceArgs2 = new CancelableEventSourceArgs(source);
			OnBeforeMenuClose(cancelableEventSourceArgs2);
			if (cancelableEventSourceArgs2.Cancel)
			{
				return;
			}
		}
		bool oldValue = __20D1;
		__20D1 = isOpen;
		__2465.Expanded = isOpen;
		OnIsOpenChanged(oldValue, isOpen);
	}

	protected override void OnClick(EventArgs e)
	{
		SetIsOpen(!IsOpen, eEventSource.Mouse);
		((Control)this).OnClick(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		if (__19DA)
		{
			return;
		}
		__19DA = true;
		try
		{
			RadialMenuColorTable radialMenuColorTable = RadialMenuContainer.ጧ();
			RadialMenuColorTable colors = __2465.Colors;
			Color color = ColorScheme.GetColor(2840474);
			Color color2 = Color.White;
			if (!colors.RadialMenuButtonBorder.IsEmpty)
			{
				color = colors.RadialMenuButtonBorder;
			}
			else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuButtonBorder.IsEmpty)
			{
				color = radialMenuColorTable.RadialMenuButtonBorder;
			}
			if (!colors.RadialMenuButtonBackground.IsEmpty)
			{
				color2 = colors.RadialMenuButtonBackground;
			}
			else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuButtonBackground.IsEmpty)
			{
				color2 = radialMenuColorTable.RadialMenuButtonBackground;
			}
			SolidBrush val = new SolidBrush(color2);
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = (SmoothingMode)4;
			graphics.TextRenderingHint = (TextRenderingHint)3;
			graphics.InterpolationMode = (InterpolationMode)7;
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(1, 1);
			if (!((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
			{
				SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, clientRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else if (((Control)this).BackColor == Color.Transparent)
			{
				_2481(e.Graphics);
			}
			clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			Rectangle rectangle = clientRectangle;
			rectangle.Inflate(-1, -1);
			graphics.FillEllipse((Brush)(object)val, rectangle);
			if (__246A)
			{
				GraphicsPath val3 = new GraphicsPath();
				try
				{
					val3.AddEllipse(clientRectangle);
					clientRectangle.Inflate(-2, -2);
					val3.AddEllipse(clientRectangle);
					SolidBrush val4 = new SolidBrush(color);
					try
					{
						graphics.FillPath((Brush)(object)val4, val3);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			if (!string.IsNullOrEmpty(__2465.SymbolRealized))
			{
				float symbolSize = __2465.SymbolSize;
				Font font = Symbols.GetFont(symbolSize, __2465.SymbolSet);
				if (_ᨀ.IsEmpty)
				{
					_ᨀ = TextDrawing.MeasureStringLegacy(graphics, __2465.SymbolRealized, Symbols.GetFont(symbolSize, __2465.SymbolSet), Size.Empty, eTextFormat.Default);
					int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
					_ᨀ.Height -= num;
				}
				TextDrawing.DrawStringLegacy(graphics, __2465.SymbolRealized, Symbols.GetFont(symbolSize, __2465.SymbolSet), color, new Rectangle(((Control)this).ClientRectangle.Width / 2, clientRectangle.Y + (clientRectangle.Height - _ᨀ.Height) / 2, 0, 0), eTextFormat.HorizontalCenter);
			}
			else if (_ነ != null)
			{
				clientRectangle = ((Control)this).ClientRectangle;
				ᶣ.ᶤ(graphics, _ነ, clientRectangle);
			}
			((Brush)val).Dispose();
			val = null;
		}
		finally
		{
			__19DA = false;
		}
		((Control)this).OnPaint(e);
	}

	private void _2481(Graphics _٥)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Control parent = ((Control)this).Parent;
		if (parent != null)
		{
			_٥.SmoothingMode = (SmoothingMode)4;
			_٥.TextRenderingHint = (TextRenderingHint)3;
			_٥.InterpolationMode = (InterpolationMode)7;
			Bitmap val = new Bitmap(parent.Width, parent.Height, _٥);
			try
			{
				parent.DrawToBitmap(val, parent.ClientRectangle);
				_٥.TranslateTransform((float)(-((Control)this).Left), (float)(-((Control)this).Top));
				_٥.DrawImage((Image)(object)val, Point.Empty);
				_٥.TranslateTransform((float)((Control)this).Left, (float)((Control)this).Top);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (proposedSize.Width <= 0)
		{
			return new Size(26, 26);
		}
		return new Size(proposedSize.Width, proposedSize.Height);
	}

	private void _2482(object _٠, EventArgs ؿ)
	{
		__20D1 = false;
		_2484((AccessibleEvents)7);
		_2484((AccessibleEvents)5);
		OnMenuClosed(EventArgs.Empty);
	}

	private void _2483(object _٠, EventArgs ؿ)
	{
		_2484((AccessibleEvents)4);
		_2484((AccessibleEvents)6);
		OnMenuOpened(EventArgs.Empty);
	}

	private void _2484(AccessibleEvents ؿ)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (__05F5)
		{
			RadialMenuAccessibleObject radialMenuAccessibleObject = (RadialMenuAccessibleObject)(object)((Control)this).AccessibilityObject;
			radialMenuAccessibleObject._၈(ؿ);
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (!__246F && !((Component)this).DesignMode)
		{
			_1BE6._1BEA(this);
			__246F = true;
		}
		if (!GetDesignMode() && !__0BC6)
		{
			if (!__246D)
			{
				ੴ();
			}
		}
		else if (__246E == null)
		{
			__246E = new ᒺ(this);
		}
		_2485();
		((Control)this).OnHandleCreated(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A75();
		((Control)this).OnHandleDestroyed(e);
		if (__246F)
		{
			_1BE6._1BEB(this);
			__246F = false;
		}
		if (__246E != null)
		{
			__246E.Dispose();
			__246E = null;
		}
	}

	private void _0A75()
	{
		if (!__246D)
		{
			return;
		}
		__246D = false;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if (val.IsMdiChild && val.MdiParent != null)
			{
				val = val.MdiParent;
			}
			DotNetBarManager.UnRegisterParentMsgHandler(this, val);
			((Control)val).Resize -= _0A76;
			val.Deactivate -= _0A77;
		}
	}

	private void _2485()
	{
		if (!((Control)this).IsHandleCreated)
		{
			return;
		}
		if (__2470)
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				SizeF scaleFactor = new SizeF(val.DpiX / 96f, val.DpiY / 96f);
				if (scaleFactor.Width != 1f)
				{
					__2465.ScaleFactor = scaleFactor;
				}
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		__2465.ScaleFactor = new SizeF(1f, 1f);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 736)
		{
			_2485();
		}
		((Control)this).WndProc(ref m);
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

	protected bool GetDesignMode()
	{
		if (!__2472)
		{
			return ((Component)this).DesignMode;
		}
		return __2472;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public void SetDesignMode(bool mode)
	{
		__2472 = mode;
		__2465.SetDesignMode(mode);
	}

	protected virtual bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected I4, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		bool designMode = GetDesignMode();
		int num = _0EFE._0F11(_੦);
		if (__2465.Expanded && num == 27)
		{
			SetIsOpen(isOpen: false, eEventSource.Keyboard);
			return true;
		}
		if (__2465.Expanded && __2465.Popup != null && ((Control)__2465.Popup).Handle == _0A65)
		{
			Keys val = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
			if ((int)val == 0)
			{
				val = (Keys)num;
			}
			__2465.InternalKeyDown(new KeyEventArgs(val));
			return false;
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
		if (!__246B || !((Control)this).Enabled)
		{
			return false;
		}
		Form val = ((Control)this).FindForm();
		if (val == null || (((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)) && !val.IsMdiContainer) || (Form.ActiveForm != null && Form.ActiveForm.Modal && Form.ActiveForm != val))
		{
			return false;
		}
		bool result = BarFunctions.ProcessItemsShortcuts(_੬, __2475);
		if (!__2473)
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
		if (!__20D1 || GetDesignMode() || __2465.Popup == null)
		{
			return false;
		}
		if (!((Control)__2465.Popup).Bounds.Contains(Control.MousePosition))
		{
			SetIsOpen(isOpen: false, eEventSource.Mouse);
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
		if (__20D1 && __2465.Popup != null && ((Control)__2465.Popup).Handle != _0A65)
		{
			return true;
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected I4, but got Unknown
		if (!GetDesignMode())
		{
			int num = _0EFE._0F11(_੦);
			if (num == 18 && (int)Control.ModifierKeys == 262144)
			{
				SetIsOpen(isOpen: false, eEventSource.Keyboard);
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
		if (__246D)
		{
			return;
		}
		__246D = true;
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			__246D = false;
			return;
		}
		if (val.IsMdiChild && val.MdiParent != null)
		{
			val = val.MdiParent;
		}
		((Control)val).Resize += _0A76;
		val.Deactivate += _0A77;
		DotNetBarManager.RegisterParentMsgHandler(this, val);
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

	private ArrayList ᒡ(string ᒢ)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(__2465, ᒢ, arrayList);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ᒢ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒡ
		return this.ᒡ(ᒢ);
	}

	private ArrayList ᒡ(string ᒢ, Type _0962)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(__2465, ᒢ, arrayList, _0962);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ᒢ, Type _0962)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒡ
		return this.ᒡ(ᒢ, _0962);
	}

	private ArrayList ᒡ(string ᒢ, Type _0962, bool ᒣ)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(__2465, ᒢ, arrayList, _0962, ᒣ);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ᒢ, Type _0962, bool ᒣ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒡ
		return this.ᒡ(ᒢ, _0962, ᒣ);
	}

	private BaseItem ᒤ(string ᒢ)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(__2465, ᒢ);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		return null;
	}

	BaseItem IOwner.GetItem(string ᒢ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒤ
		return this.ᒤ(ᒢ);
	}

	private void ਣ(BaseItem ਤ)
	{
		if (ਤ != null)
		{
			if (ਤ.Parent is RadialMenuContainer)
			{
				return;
			}
			_ = ਤ.Parent;
		}
		if (__2474 != null)
		{
			if (__2474.Expanded)
			{
				__2474.Expanded = false;
			}
			__2474 = null;
		}
		__2474 = ਤ;
	}

	void IOwner.SetExpandedItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਣ
		this.ਣ(ਤ);
	}

	private BaseItem ਥ()
	{
		return __2474;
	}

	BaseItem IOwner.GetExpandedItem()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਥ
		return this.ਥ();
	}

	private void ਦ(BaseItem ਧ)
	{
		if (__1B34 != null && __1B34 != ਧ)
		{
			__1B34.OnLostFocus();
		}
		OnSetFocusItem(ਧ);
		__1B34 = ਧ;
		if (__1B34 != null)
		{
			__1B34.OnGotFocus();
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
		return __1B34;
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
				if (!__2475.ContainsKey(shortcut))
				{
					continue;
				}
				ᯣ2 = (ᯣ)__2475[shortcut];
				try
				{
					ᯣ2.ᯥ.Remove(ਤ.Id);
					if (ᯣ2.ᯥ.Count == 0)
					{
						__2475.Remove(ᯣ2.ᯤ);
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
				if (__2475.ContainsKey(shortcut))
				{
					ᯣ2 = (ᯣ)__2475[ਤ.Shortcuts[0]];
				}
				else
				{
					ᯣ2 = new ᯣ(shortcut);
					__2475.Add(ᯣ2.ᯤ, ᯣ2);
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

	private void _0C77(BaseItem ਤ)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	void IOwner.StartItemDrag(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ౷
		this._0C77(ਤ);
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

	private void ਭ()
	{
		throw new Exception("The method or operation is not implemented.");
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

	private void _0A37(object _٠, EventArgs ؿ)
	{
	}

	void IOwner.InvokeDefinitionLoaded(object _٠, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਷
		this._0A37(_٠, ؿ);
	}

	private void ਲ਼(object _٠, EventArgs ؿ)
	{
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
		if (IsOpen && __246C)
		{
			IsOpen = false;
		}
	}

	void IOwner.OnApplicationDeactivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਱
		this._0A31();
	}

	private void ਲ()
	{
		if (IsOpen)
		{
			IsOpen = false;
		}
	}

	void IOwner.OnParentPositionChanging()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਲ
		this.ਲ();
	}

	protected virtual void OnItemAdded(BaseItem source, EventArgs e)
	{
		ৎ?.Invoke(source, e);
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

	protected virtual void OnItemRemoved(BaseItem source, ItemRemovedEventArgs e)
	{
		_09CD?.Invoke(source, e);
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

	protected virtual void OnItemMouseEnter(BaseItem source, EventArgs e)
	{
		_2479?.Invoke(source, e);
	}

	private void _0A50(BaseItem _06EC, EventArgs ؿ)
	{
		OnItemMouseEnter(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeMouseEnter(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੐
		this._0A50(_06EC, ؿ);
	}

	protected virtual void OnItemMouseHover(BaseItem source, EventArgs e)
	{
		_247A?.Invoke(source, e);
	}

	private void _0A51(BaseItem _06EC, EventArgs ؿ)
	{
		OnItemMouseHover(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeMouseHover(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੑ
		this._0A51(_06EC, ؿ);
	}

	protected virtual void OnItemMouseLeave(BaseItem source, EventArgs e)
	{
		_247B?.Invoke(source, e);
	}

	private void _0A52(BaseItem _06EC, EventArgs ؿ)
	{
		OnItemMouseLeave(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeMouseLeave(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੒
		this._0A52(_06EC, ؿ);
	}

	protected virtual void OnItemMouseDown(BaseItem source, MouseEventArgs e)
	{
		MouseEventHandler val = _247C;
		if (val != null)
		{
			val.Invoke((object)source, e);
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

	protected virtual void OnItemMouseUp(BaseItem source, MouseEventArgs e)
	{
		MouseEventHandler val = _247D;
		if (val != null)
		{
			val.Invoke((object)source, e);
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

	protected virtual void OnItemMouseMove(BaseItem source, MouseEventArgs e)
	{
		MouseEventHandler val = _247E;
		if (val != null)
		{
			val.Invoke((object)source, e);
		}
	}

	private void _0A56(BaseItem _06EC, MouseEventArgs ؿ)
	{
		OnItemMouseMove(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeMouseMove(BaseItem _06EC, MouseEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੖
		this._0A56(_06EC, ؿ);
	}

	protected virtual void OnItemClick(BaseItem source, EventArgs e)
	{
		_09BC?.Invoke(source, e);
	}

	private void _0A58(BaseItem ਤ)
	{
		OnItemClick(ਤ, EventArgs.Empty);
	}

	void IOwnerItemEvents.InvokeItemClick(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੘
		this._0A58(ਤ);
	}

	protected virtual void OnItemDoubleClick(BaseItem source, MouseEventArgs e)
	{
		MouseEventHandler val = ঽ;
		if (val != null)
		{
			val.Invoke((object)source, e);
		}
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

	private void ਖ਼(BaseItem _06EC, EventArgs ؿ)
	{
	}

	void IOwnerItemEvents.InvokeGotFocus(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਖ਼
		this.ਖ਼(_06EC, ؿ);
	}

	private void ਗ਼(BaseItem _06EC, EventArgs ؿ)
	{
	}

	void IOwnerItemEvents.InvokeLostFocus(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਗ਼
		this.ਗ਼(_06EC, ؿ);
	}

	protected virtual void OnItemExpandedChanged(BaseItem source, EventArgs e)
	{
		_247F?.Invoke(source, e);
	}

	private void ਜ਼(BaseItem _06EC, EventArgs ؿ)
	{
		OnItemExpandedChanged(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeExpandedChange(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਜ਼
		this.ਜ਼(_06EC, ؿ);
	}

	protected virtual void OnItemTextChanged(BaseItem source, EventArgs e)
	{
		_09D0?.Invoke(source, e);
	}

	private void ੜ(BaseItem _06EC, EventArgs ؿ)
	{
		OnItemTextChanged(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeItemTextChanged(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੜ
		this.ੜ(_06EC, ؿ);
	}

	private void _0A5D(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
	}

	void IOwnerItemEvents.InvokeContainerControlDeserialize(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੝
		this._0A5D(_06EC, ؿ);
	}

	private void ਫ਼(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
	}

	void IOwnerItemEvents.InvokeContainerControlSerialize(BaseItem _06EC, ControlContainerSerializationEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਫ਼
		this.ਫ਼(_06EC, ؿ);
	}

	private void _0A5F(BaseItem _06EC, EventArgs ؿ)
	{
	}

	void IOwnerItemEvents.InvokeContainerLoadControl(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੟
		this._0A5F(_06EC, ؿ);
	}

	private void _0A60(BaseItem _06EC, OptionGroupChangingEventArgs ؿ)
	{
	}

	void IOwnerItemEvents.InvokeOptionGroupChanging(BaseItem _06EC, OptionGroupChangingEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੠
		this._0A60(_06EC, ؿ);
	}

	protected virtual void OnItemTooltipShowing(object source, EventArgs e)
	{
		_2480?.Invoke(source, e);
	}

	private void _0A61(object _06EC, EventArgs ؿ)
	{
		OnItemTooltipShowing(_06EC, ؿ);
	}

	void IOwnerItemEvents.InvokeToolTipShowing(object _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੡
		this._0A61(_06EC, ؿ);
	}

	private void _0A62(ButtonItem _06EC, EventArgs ؿ)
	{
	}

	void IOwnerItemEvents.InvokeCheckedChanged(ButtonItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੢
		this._0A62(_06EC, ؿ);
	}
}
