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
using System.Xml;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.ExplorerBarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[ComVisible(false)]
public class ExplorerBar : Control, IOwner, IOwnerMenuSupport, IOwnerItemEvents, IAccessibilitySupport, ል, ሎ, IBarDesignerServices, ICustomSerialization, ISupportInitialize
{
	public delegate void ItemRemovedEventHandler(object sender, ItemRemovedEventArgs e);

	public class ExplorerBarAccessibleObject : AccessibleObject
	{
		private ExplorerBar _0AC3;

		public override string Name
		{
			get
			{
				return ((Control)_0AC3).AccessibleName;
			}
			set
			{
				((Control)_0AC3).AccessibleName = value;
			}
		}

		public override string Description => ((Control)_0AC3).AccessibleDescription;

		public override AccessibleRole Role => ((Control)_0AC3).AccessibleRole;

		public override AccessibleObject Parent
		{
			get
			{
				if (((Control)_0AC3).Parent != null)
				{
					return ((Control)_0AC3).Parent.AccessibilityObject;
				}
				return null;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (((Control)_0AC3).Parent != null)
				{
					return ((Control)_0AC3).Parent.RectangleToScreen(((Control)_0AC3).Bounds);
				}
				return ((Control)_0AC3).DisplayRectangle;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				AccessibleStates result = (AccessibleStates)256;
				if (((Control)_0AC3).Focused)
				{
					result = (AccessibleStates)4;
				}
				return result;
			}
		}

		public ExplorerBarAccessibleObject(ExplorerBar owner)
		{
			_0AC3 = owner;
		}

		internal void _၈(BaseItem _٠, AccessibleEvents ؿ)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			int num = _0AC3.Groups.IndexOf(_٠);
			if (num >= 0)
			{
				((Control)_0AC3).AccessibilityNotifyClients(ؿ, num);
			}
		}

		public override int GetChildCount()
		{
			return _0AC3.Groups.Count;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			return _0AC3.Groups[iIndex].AccessibleObject;
		}
	}

	private const string ᇦ = "Right-click and choose Add New Group or use Groups collection to create new groups.";

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

	private OptionGroupChangingEventHandler _09D5;

	private EventHandler _09D6;

	private SerializeItemEventHandler _0B52;

	private SerializeItemEventHandler _0B53;

	private ExplorerBarContainerItem ଡ଼;

	private Timer _৮;

	private BaseItem _৯;

	private BaseItem _09D8;

	private BaseItem _09D9;

	private ImageList գ;

	private ImageList _09DB;

	private ImageList ড়;

	private bool _০ = true;

	private bool _১;

	private Hashtable _09DA = new Hashtable();

	private eBorderType ᇧ;

	private ExplorerBarGroupItem ᇨ;

	private BaseItem ঢ়;

	private bool _09E2;

	private Cursor _09E4;

	private Cursor ᇩ;

	private Cursor _09E5;

	private IDesignTimeProvider _৫;

	private int _৬;

	private bool _৭;

	private bool ᇪ = true;

	private bool _09F8;

	private _0A79 ৰ;

	private _0A7A ৱ;

	private _0A7B _09F2;

	private _0A7C _09F3;

	private _0A7D _09F4;

	private _0A7E _09F5;

	private _0A7F _09F6;

	private _0A80 _09F7;

	private bool ৡ;

	private bool ৠ;

	private bool _09E3;

	private bool য়;

	private VScrollBar ᇫ;

	private int ᇬ = 4;

	private ItemStyleMapper _0599;

	private ElementStyle ᇭ = new ElementStyle();

	private eExplorerBarStockStyle ᇮ = eExplorerBarStockStyle.Custom;

	private bool ᇯ = true;

	private int ᇰ = 100;

	private bool _৩;

	private bool _২;

	private bool _৪;

	private bool ᇱ;

	private ColorScheme լ;

	private bool ե = true;

	private IBarItemDesigner _09FD;

	private bool _0AC7;

	private Image ᇲ;

	private Image ᇳ;

	private Image ᇴ;

	private Image ᇵ;

	private Image ᇶ;

	private Image ᇷ;

	private BaseItem ਈ;

	private bool ৼ;

	private EventHandler ਞ;

	private bool ਖ = true;

	private ᒺ _0A0E;

	private ArrayList ਏ = new ArrayList();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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
	[DefaultValue(false)]
	public bool SuspendLayoutDisplay
	{
		get
		{
			return _0AC7;
		}
		set
		{
			_0AC7 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor(typeof(ColorSchemeVSEditor), typeof(UITypeEditor))]
	[Description("Gets or sets Color Scheme.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
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
			ᇱ = true;
			foreach (ExplorerBarGroupItem group in Groups)
			{
				group.ઐ();
			}
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	private bool ᇡ
	{
		get
		{
			if ((ᇭ.BackColor.IsEmpty && ᇭ.BackColor2.IsEmpty) || (ᇭ.BackColor == Color.Transparent && ᇭ.BackColor2 == Color.Transparent))
			{
				return true;
			}
			return false;
		}
	}

	protected bool IsThemed
	{
		get
		{
			if (_09F8 && (ଡ଼.EffectiveStyle == eDotNetBarStyle.OfficeXP || ଡ଼.EffectiveStyle == eDotNetBarStyle.Office2003) && BarFunctions.ThemedOS && _0EEE._0EF0)
			{
				return true;
			}
			return false;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Specifies whether ExplorerBar is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return _09F8;
		}
		set
		{
			_09F8 = value;
			ଡ଼.ThemeAware = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Description("Gets or sets bar background style.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public ElementStyle BackStyle => ᇭ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This property is obsolete. Use BackStyle property instead")]
	public ItemStyleMapper BackgroundStyle => _0599;

	[DefaultValue(15)]
	[Description("Specifies the vertical spacing between the group items.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int GroupSpacing
	{
		get
		{
			return ଡ଼.ᅣ;
		}
		set
		{
			ଡ଼.ᅣ = value;
			RecalcLayout();
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

	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(true)]
	[Category("Appearance")]
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
				((Control)this).Refresh();
			}
		}
	}

	[Category("Data")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Returns the collection of Explorer Bar Groups.")]
	[Editor("DevComponents.DotNetBar.Design.ExplorerBarPanelsEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public SubItemsCollection Groups => ଡ଼.SubItems;

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

	[Category("Layout")]
	[DefaultValue(4)]
	[Browsable(true)]
	[Description("Indicates margin in pixels between the explorer bar groups and the edge of the control.")]
	public int ContentMargin
	{
		get
		{
			return ᇬ;
		}
		set
		{
			ᇬ = value;
			RecalcLayout();
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Specifies whether end-user can rearrange the items inside the panels.")]
	[Browsable(true)]
	public bool AllowUserCustomize
	{
		get
		{
			return ᇪ;
		}
		set
		{
			ᇪ = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Specifies whether animation is enabled.")]
	public bool AnimationEnabled
	{
		get
		{
			return ᇯ;
		}
		set
		{
			ᇯ = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(100)]
	[Description("Indicates maximum animation time in milliseconds, default value is 100.")]
	[DevCoBrowsable(true)]
	public int AnimationTime
	{
		get
		{
			return ᇰ;
		}
		set
		{
			if (ᇰ > 0)
			{
				ᇰ = value;
			}
		}
	}

	[Category("Behavior")]
	[Description("Specifies whether native .NET Drag and Drop is used by side-bar to perform drag and drop operations.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool UseNativeDragDrop
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

	[Browsable(true)]
	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool AllowExternalDrop
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

	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(eExplorerBarStockStyle.Custom)]
	[Description("Applies the stock style to the object.")]
	[Browsable(true)]
	public eExplorerBarStockStyle StockStyle
	{
		get
		{
			return ᇮ;
		}
		set
		{
			BarFunctions.SetExplorerBarStyle(this, ᇮ);
			foreach (BaseItem subItem in ଡ଼.SubItems)
			{
				if (subItem is ExplorerBarGroupItem && ((ExplorerBarGroupItem)subItem).StockStyle != eExplorerBarStockStyle.Custom)
				{
					((ExplorerBarGroupItem)subItem).StockStyle = value;
				}
			}
			ᇮ = value;
			((Control)this).Refresh();
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
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

	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
	[Category("Group Expand Button")]
	public Image GroupButtonExpandNormal
	{
		get
		{
			return ᇲ;
		}
		set
		{
			ᇲ = value;
			((Control)this).Refresh();
		}
	}

	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Group Expand Button")]
	public Image GroupButtonExpandHot
	{
		get
		{
			return ᇳ;
		}
		set
		{
			ᇳ = value;
		}
	}

	[Category("Group Expand Button")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to expand the group.")]
	public Image GroupButtonExpandPressed
	{
		get
		{
			return ᇴ;
		}
		set
		{
			ᇴ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Group Expand Button")]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
	public Image GroupButtonCollapseNormal
	{
		get
		{
			return ᇵ;
		}
		set
		{
			ᇵ = value;
			((Control)this).Refresh();
		}
	}

	[DefaultValue(null)]
	[Category("Group Expand Button")]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
	[Browsable(true)]
	public Image GroupButtonCollapseHot
	{
		get
		{
			return ᇶ;
		}
		set
		{
			ᇶ = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Specifies custom image to be used as button on ExplorerBarGroup item to collapse the group.")]
	[Category("Group Expand Button")]
	public Image GroupButtonCollapsePressed
	{
		get
		{
			return ᇷ;
		}
		set
		{
			ᇷ = value;
		}
	}

	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	[Category("Data")]
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

	[Browsable(true)]
	[Category("Data")]
	[Description("ImageList for images displayed on the Group Item.")]
	public ImageList GroupImages
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

	ImageList IOwner.ᇣ
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

	ImageList IOwner.ᇤ
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

	bool IOwner.ଜ => _09E2;

	BaseItem IOwner.ଛ => ঢ়;

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

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Run-time Behavior")]
	[Description("Indicates whether item shortcut is displayed in Tooltips.")]
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

	bool IOwner.জ => ((Component)this).DesignMode;

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
		}
	}

	[Browsable(false)]
	public ExplorerBarContainerItem ItemsContainer => ଡ଼;

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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	bool ICustomSerialization.ଏ => _0B52 != null;

	bool ICustomSerialization.ଐ => _0B53 != null;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string Definition
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			ఠ(xmlDocument);
			return xmlDocument.OuterXml;
		}
		set
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			ఢ(xmlDocument);
			((Control)this).Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates whether shortucts handled by items are dispatched to the next handler or control.")]
	[DefaultValue(false)]
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

	private bool ᇥ
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

	public event SerializeItemEventHandler SerializeItem
	{
		add
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B52;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Combine(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B52, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
		remove
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B52;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Remove(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B52, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
	}

	public event SerializeItemEventHandler DeserializeItem
	{
		add
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B53;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Combine(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B53, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
		}
		remove
		{
			SerializeItemEventHandler serializeItemEventHandler = _0B53;
			SerializeItemEventHandler serializeItemEventHandler2;
			do
			{
				serializeItemEventHandler2 = serializeItemEventHandler;
				SerializeItemEventHandler value2 = (SerializeItemEventHandler)Delegate.Remove(serializeItemEventHandler2, value);
				serializeItemEventHandler = Interlocked.CompareExchange(ref _0B53, value2, serializeItemEventHandler2);
			}
			while ((object)serializeItemEventHandler != serializeItemEventHandler2);
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

	public ExplorerBar()
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		լ = new ColorScheme(eDotNetBarStyle.Office2003);
		ଡ଼ = new ExplorerBarContainerItem();
		ଡ଼.GlobalItem = false;
		ଡ଼.ContainerControl = this;
		ଡ଼.Stretch = false;
		ଡ଼.Displayed = true;
		ଡ଼.SetOwner(this);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)1, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).TabStop = true;
		Ӕ ӕ2 = new Ӕ();
		ӕ2.Ḕ();
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
		ApplyDefaultSettings();
		ᇭ.StyleChanged += ઐ;
		((Control)this).AccessibleRole = (AccessibleRole)22;
		_0599 = new ItemStyleMapper(ᇭ);
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
			if (ଡ଼ != null)
			{
				ଡ଼.Dispose();
			}
			ଡ଼ = null;
			ᇸ(ref _09E4);
			ᇸ(ref ᇩ);
			ᇸ(ref _09E5);
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.ᆟ(ref ᇶ);
			BarUtilities.ᆟ(ref ᇵ);
			BarUtilities.ᆟ(ref ᇷ);
			BarUtilities.ᆟ(ref ᇳ);
			BarUtilities.ᆟ(ref ᇲ);
			BarUtilities.ᆟ(ref ᇴ);
		}
		((Control)this).Dispose(disposing);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		if (ଡ଼ != null)
		{
			ଡ଼.NotifyScaleItem(factor);
		}
		((Control)this).ScaleControl(factor, specified);
	}

	private void ᇸ(ref Cursor ᇹ)
	{
		if (ᇹ != (Cursor)null)
		{
			ᇹ.Dispose();
			ᇹ = null;
		}
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (ଡ଼ != null)
		{
			ଡ଼.UpdateBindings();
		}
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new ExplorerBarAccessibleObject(this);
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		ᇱ = true;
		ColorScheme colorScheme = ColorScheme;
		ᇭ.SetColorScheme(colorScheme);
		if (ᇡ)
		{
			((Control)this).BackColor = Color.Transparent;
		}
		else
		{
			((Control)this).BackColor = SystemColors.Control;
		}
		if (((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ଡ଼.InternalClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ଡ଼.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_0A81(e);
		((Control)this).OnKeyDown(e);
	}

	internal void _0A81(KeyEventArgs ؿ)
	{
		ଡ଼.InternalKeyDown(ؿ);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		((IOwner)this).SetFocusItem((BaseItem)null);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (ଡ଼ != null)
		{
			ଡ଼.FocusNextItem();
		}
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		if (((Control)this).Focused)
		{
			if ((int)keyData == 40)
			{
				if (ଡ଼ != null)
				{
					ଡ଼.FocusNextItem();
				}
				if (((IOwner)this).GetFocusItem() != null)
				{
					return true;
				}
			}
			else if ((int)keyData == 38)
			{
				if (ଡ଼ != null)
				{
					ଡ଼.FocusPreviousItem();
				}
				if (((IOwner)this).GetFocusItem() != null)
				{
					return true;
				}
			}
			else if ((int)keyData == 13 && ((IOwner)this).GetFocusItem() != null)
			{
				BaseItem focusItem = ((IOwner)this).GetFocusItem();
				if (focusItem is ExplorerBarGroupItem)
				{
					focusItem.Expanded = !focusItem.Expanded;
				}
				else
				{
					focusItem.RaiseClick();
				}
				return true;
			}
		}
		return ((Control)this).ProcessDialogKey(keyData);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		ଡ଼.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		ଡ଼.InternalMouseHover();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		ଡ଼.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (_09E2)
		{
			if (!ৡ)
			{
				ᇺ(e.X, e.Y, null);
			}
		}
		else
		{
			ଡ଼.InternalMouseMove(e);
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((Control)this).OnMouseWheel(e);
		if (ᇫ == null)
		{
			return;
		}
		int num = Math.Abs(e.Delta) / 120 * ((ScrollBar)ᇫ).SmallChange;
		if (e.Delta < 0)
		{
			int num2 = ((ScrollBar)ᇫ).Value + num;
			if (num2 > ((ScrollBar)ᇫ).Maximum)
			{
				num2 = ((ScrollBar)ᇫ).Maximum;
			}
			((ScrollBar)ᇫ).Value = num2;
		}
		else
		{
			int num3 = ((ScrollBar)ᇫ).Value - num;
			if (num3 < 0)
			{
				num3 = 0;
			}
			((ScrollBar)ᇫ).Value = num3;
		}
		ሂ(((ScrollBar)ᇫ).Value);
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		if (_09E2 || _09E3)
		{
			Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
			ᇺ(point.X, point.Y, e);
			য় = false;
		}
		((Control)this).OnDragOver(e);
	}

	protected override void OnDragLeave(EventArgs e)
	{
		if (_09E2 || _09E3)
		{
			ᇺ(-1, -1, null);
		}
		য় = true;
		_09E3 = false;
		((Control)this).OnDragLeave(e);
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		((Control)this).OnDragEnter(e);
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
		if (_09E2)
		{
			Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
			ᇻ(point.X, point.Y, null);
		}
		else if (_09E3)
		{
			Point point2 = ((Control)this).PointToClient(new Point(e.X, e.Y));
			ᇻ(point2.X, point2.Y, e);
		}
		((Control)this).OnDragDrop(e);
	}

	protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if (_09E2 && ((য় && (int)e.Action == 1) || (int)e.Action == 2))
		{
			ᇻ(-1, -1, null);
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
		foreach (ExplorerBarGroupItem subItem in ଡ଼.SubItems)
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

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (!ৡ)
		{
			ᇻ(e.X, e.Y, null);
		}
		ଡ଼.InternalMouseUp(e);
	}

	private void ᇻ(int و, int ى, DragEventArgs _0A83)
	{
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		if (!_09E2 && !_09E3)
		{
			return;
		}
		BaseItem baseItem = ঢ়;
		if (_09E3)
		{
			baseItem = _0A83.Data.GetData(typeof(ButtonItem)) as BaseItem;
		}
		baseItem?.InternalMouseLeave();
		if (_৫ != null)
		{
			if (و == -1 && ى == -1)
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
		ଡ଼.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, و, ى, 0));
		if (baseItem != null)
		{
			baseItem._IgnoreClick = false;
		}
		ঢ় = null;
		RecalcLayout();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	public void ResetColorScheme()
	{
		լ = new ColorScheme(eDotNetBarStyle.Office2003);
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

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		if (ଡ଼ != null && !((Control)this).IsDisposed && !_0AC7)
		{
			SolidBrush val = new SolidBrush(Color.White);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Graphics graphics = e.Graphics;
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
			if (ե)
			{
				e.Graphics.SmoothingMode = (SmoothingMode)4;
				e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			graphics.PageUnit = (GraphicsUnit)2;
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			ColorScheme colorScheme = ColorScheme;
			if (ᇱ)
			{
				ᇭ.SetColorScheme(colorScheme);
				ᇱ = false;
			}
			Rectangle bounds = clientRectangle;
			bounds.Inflate(1, 1);
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(ᇭ, graphics, bounds);
			ElementStyleDisplay.Paint(e2);
			if (ᇧ == eBorderType.SingleLine)
			{
				clientRectangle.Inflate(-1, -1);
			}
			else if (ᇧ != 0)
			{
				clientRectangle.Inflate(-2, -2);
			}
			if (ᇡ)
			{
				((Control)this).OnPaintBackground(e);
			}
			Region clip = e.Graphics.Clip;
			e.Graphics.SetClip(clientRectangle);
			ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, e.Graphics, colorScheme);
			itemPaintArgs.ໝ = ဣ();
			itemPaintArgs.DesignerSelection = ৼ;
			itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && _0EFE.ষ;
			ଡ଼.Paint(itemPaintArgs);
			if (clip != null)
			{
				e.Graphics.SetClip(clip, (CombineMode)0);
			}
			else
			{
				e.Graphics.ResetClip();
			}
			clip.Dispose();
			if (ଡ଼.SubItems.Count == 0 && ((Component)this).DesignMode)
			{
				string text = "Right-click and choose Add New Group or use Groups collection to create new groups.";
				Rectangle clientRectangle2 = ((Control)this).ClientRectangle;
				clientRectangle.Inflate(-2, -2);
				eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				Font val2 = new Font(((Control)this).Font.FontFamily, 7f);
				TextDrawing.DrawString(graphics, text, val2, SystemColors.ControlText, clientRectangle2, format);
				val2.Dispose();
			}
			graphics.SmoothingMode = smoothingMode;
			graphics.TextRenderingHint = textRenderingHint;
		}
	}

	private BaseRenderer ဣ()
	{
		return GlobalManager.Renderer;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ApplyDefaultSettings()
	{
		ColorScheme colorScheme = ColorScheme;
		ᇭ.BackColorSchemePart = eColorSchemePart.ExplorerBarBackground;
		ᇭ.BackColor2SchemePart = eColorSchemePart.ExplorerBarBackground2;
		ᇭ.BackColorGradientAngle = colorScheme.ExplorerBarBackgroundGradientAngle;
		ᇭ.SetColorScheme(colorScheme);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		చ();
		ColorScheme.Refresh(null, bSystemColorEvent: true);
	}

	private void చ()
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
		if (_09F5 != null)
		{
			_09F5.Dispose();
			_09F5 = new _0A7E((Control)(object)this);
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = new _0A7F((Control)(object)this);
		}
		if (_09F7 != null)
		{
			_09F7.Dispose();
			_09F7 = new _0A80((Control)(object)this);
		}
		ᇼ();
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
		if (_09F7 != null)
		{
			_09F7.Dispose();
			_09F7 = null;
		}
	}

	private void ᇼ()
	{
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			if (subItem is ExplorerBarGroupItem { StockStyle: eExplorerBarStockStyle.SystemColors } explorerBarGroupItem)
			{
				explorerBarGroupItem.StockStyle = eExplorerBarStockStyle.SystemColors;
			}
		}
	}

	private eExplorerBarStockStyle ᇽ(eExplorerBarStockStyle _0657)
	{
		eExplorerBarStockStyle result = eExplorerBarStockStyle.Custom;
		switch (_0657)
		{
		case eExplorerBarStockStyle.Blue:
			result = eExplorerBarStockStyle.BlueSpecial;
			break;
		case eExplorerBarStockStyle.OliveGreen:
			result = eExplorerBarStockStyle.OliveGreenSpecial;
			break;
		case eExplorerBarStockStyle.Silver:
			result = eExplorerBarStockStyle.SilverSpecial;
			break;
		case eExplorerBarStockStyle.BlueSpecial:
		case eExplorerBarStockStyle.OliveGreenSpecial:
		case eExplorerBarStockStyle.SilverSpecial:
			result = _0657;
			break;
		}
		return result;
	}

	private bool ᇾ(eExplorerBarStockStyle _0657)
	{
		if (_0657 == eExplorerBarStockStyle.BlueSpecial || _0657 == eExplorerBarStockStyle.OliveGreenSpecial || _0657 == eExplorerBarStockStyle.SilverSpecial)
		{
			return true;
		}
		return false;
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

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (((Control)this).Visible)
		{
			ᇼ();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			RecalcSize();
		}
	}

	protected virtual void RecalcSize()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (!_0AC7 && BarFunctions.IsHandleValid((Control)(object)this))
		{
			if ((int)((Control)this).RightToLeft == 1)
			{
				ଡ଼.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
			}
			if (ᇧ == eBorderType.None)
			{
				ଡ଼.LeftInternal = ᇬ;
				ଡ଼.TopInternal = ᇬ;
				ଡ଼.WidthInternal = ((Control)this).Width - ᇬ * 2;
				ଡ଼.HeightInternal = ((Control)this).Height - ᇬ;
			}
			else
			{
				ଡ଼.LeftInternal = ᇬ;
				ଡ଼.TopInternal = ᇬ;
				ଡ଼.WidthInternal = ((Control)this).Width - 4 - ᇬ * 2;
				ଡ଼.HeightInternal = ((Control)this).Height - ᇬ - 2;
			}
			ଡ଼.RecalcSize();
			if (ଡ଼.HeightInternal > ((Control)this).ClientRectangle.Height || (ଡ଼.HeightInternal <= ((Control)this).ClientRectangle.Height && ᇫ != null))
			{
				ᇿ();
			}
			OnItemLayoutUpdated(EventArgs.Empty);
		}
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		ਞ?.Invoke(this, e);
	}

	internal void ᇿ()
	{
		if (ଡ଼.HeightInternal > ((Control)this).ClientRectangle.Height)
		{
			ሀ();
			if (ᇧ == eBorderType.None)
			{
				ଡ଼.LeftInternal = ᇬ;
				ଡ଼.TopInternal = ᇬ;
				ଡ଼.WidthInternal = ((Control)this).Width - ((Control)ᇫ).Width - ᇬ * 2;
				ଡ଼.HeightInternal = ((Control)this).Height - ᇬ;
			}
			else
			{
				ଡ଼.LeftInternal = ᇬ;
				ଡ଼.TopInternal = ᇬ;
				ଡ଼.WidthInternal = ((Control)this).Width - 4 - ((Control)ᇫ).Width - ᇬ * 2;
				ଡ଼.HeightInternal = ((Control)this).Height - ᇬ - 2;
			}
			ଡ଼.RecalcSize();
			ሀ();
			ଡ଼.TopInternal = -((ScrollBar)ᇫ).Value + ᇬ;
		}
		else if (ᇫ != null)
		{
			((Control)this).Controls.Remove((Control)(object)ᇫ);
			((Component)(object)ᇫ).Dispose();
			ᇫ = null;
			if (ᇧ == eBorderType.None)
			{
				ଡ଼.LeftInternal = ᇬ;
				ଡ଼.TopInternal = ᇬ;
				ଡ଼.WidthInternal = ((Control)this).Width - ᇬ * 2;
				ଡ଼.HeightInternal = ((Control)this).Height - ᇬ;
			}
			else
			{
				ଡ଼.LeftInternal = ᇬ;
				ଡ଼.TopInternal = ᇬ;
				ଡ଼.WidthInternal = ((Control)this).Width - 4 - ᇬ * 2;
				ଡ଼.HeightInternal = ((Control)this).Height - ᇬ - 2;
			}
			ଡ଼.RecalcSize();
		}
	}

	private void ሀ()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		if (ᇫ == null)
		{
			ᇫ = new VScrollBar();
			((Control)ᇫ).Width = SystemInformation.VerticalScrollBarWidth;
			((Control)ᇫ).Dock = (DockStyle)4;
			((Control)ᇫ).Visible = true;
			((Control)this).Controls.Add((Control)(object)ᇫ);
			((ScrollBar)ᇫ).Scroll += new ScrollEventHandler(ሁ);
			((Component)(object)ᇫ).Site = null;
		}
		((ScrollBar)ᇫ).Maximum = ଡ଼.HeightInternal;
		((ScrollBar)ᇫ).LargeChange = ((Control)this).Height / 2;
		((ScrollBar)ᇫ).SmallChange = ((ScrollBar)ᇫ).LargeChange / 5;
		int num = ଡ଼.HeightInternal - ((Control)this).Height + ((ScrollBar)ᇫ).LargeChange + ((ScrollBar)ᇫ).SmallChange;
		if (((ScrollBar)ᇫ).SmallChange > 0 && Math.Ceiling((float)num / (float)((ScrollBar)ᇫ).SmallChange) > (double)(num / ((ScrollBar)ᇫ).SmallChange))
		{
			num = (int)Math.Ceiling((float)num / (float)((ScrollBar)ᇫ).SmallChange) * ((ScrollBar)ᇫ).SmallChange;
		}
		if (((ScrollBar)ᇫ).Maximum != num)
		{
			((ScrollBar)ᇫ).Maximum = num;
		}
	}

	private void ሁ(object _٠, ScrollEventArgs ؿ)
	{
		ሂ(ؿ.NewValue);
	}

	private void ሂ(int ሃ)
	{
		ଡ଼.TopInternal = -ሃ + ᇬ;
		((Control)this).Invalidate(((Control)this).ClientRectangle, false);
	}

	public void RecalcLayout()
	{
		if (!_0AC7)
		{
			RecalcSize();
			((Control)this).Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		ଡ଼.SetDesignMode(b: true);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (((Component)this).DesignMode)
		{
			SetDesignMode();
		}
		if (!_২ && !((Component)this).DesignMode)
		{
			_1BE6._1BEA(this);
			_২ = true;
		}
		RecalcLayout();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (ᇨ != null)
		{
			if (ଡ଼.SubItems.Contains(ᇨ))
			{
				ᇨ.Expanded = true;
			}
			ᇨ = null;
		}
		if (Images != null || ((IOwner)this).ImagesLarge != null || ((IOwner)this).ImagesMedium != null)
		{
			foreach (ExplorerBarGroupItem subItem in ଡ଼.SubItems)
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
		RecalcLayout();
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

	public ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(ଡ଼, ItemName, arrayList);
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(ଡ଼, ItemName, arrayList, itemType);
		return arrayList;
	}

	public ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(ଡ଼, ItemName, arrayList, itemType, useGlobalName);
		return arrayList;
	}

	public BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(ଡ଼, ItemName);
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
		if (((Component)this).DesignMode && Groups.Contains(ਧ))
		{
			foreach (BaseItem group in Groups)
			{
				BaseItem.CollapseSubItems(group);
			}
		}
		_09D9 = ਧ;
		if (_09D9 != null)
		{
			_09D9.OnGotFocus();
		}
		if (!((Component)this).DesignMode)
		{
			EnsureVisible(_09D9);
		}
	}

	void IOwner.SetFocusItem(BaseItem ਧ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਦ
		this.ਦ(ਧ);
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

	public void EnsureVisible(BaseItem item)
	{
		if (item == null || item.ContainerControl != this)
		{
			return;
		}
		if (item.Parent is ExplorerBarGroupItem)
		{
			if (!item.Parent.Visible)
			{
				return;
			}
			if (!item.Parent.Expanded)
			{
				item.Parent.Expanded = true;
			}
		}
		if (ᇫ == null || !((Control)ᇫ).Visible)
		{
			return;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Rectangle rect = item.DisplayRectangle;
		if (item is ExplorerBarGroupItem)
		{
			rect = ((ExplorerBarGroupItem)item).PanelRect;
			rect.Y = item.TopInternal;
		}
		if (clientRectangle.Contains(rect))
		{
			return;
		}
		int num = 0;
		if (rect.Top < 0)
		{
			num = ((ScrollBar)ᇫ).Value + rect.Top - ᇬ;
			if (num < 0)
			{
				num = 0;
			}
		}
		else
		{
			num = Math.Abs(ଡ଼.TopInternal) + rect.Bottom - clientRectangle.Height + ᇬ;
		}
		((ScrollBar)ᇫ).Value = num;
		ሂ(((ScrollBar)ᇫ).Value);
	}

	private void ህ()
	{
	}

	void ISupportInitialize.BeginInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ህ
		this.ህ();
	}

	private void ሆ()
	{
		if (Images != null)
		{
			foreach (BaseItem subItem in ଡ଼.SubItems)
			{
				ሇ(subItem as ImageItem);
			}
		}
		RecalcLayout();
	}

	void ISupportInitialize.EndInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ሆ
		this.ሆ();
	}

	private void ሇ(ImageItem _06EC)
	{
		if (_06EC == null)
		{
			return;
		}
		_06EC.OnImageChanged();
		foreach (BaseItem subItem in _06EC.SubItems)
		{
			ሇ(subItem as ImageItem);
		}
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

	private void _0C77(BaseItem _06EC)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (ঢ় != null)
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
				ᇻ(-1, -1, null);
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

	public BaseItem HitTest(int clientX, int clientY)
	{
		BaseItem result = null;
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			if (!subItem.DisplayRectangle.Contains(clientX, clientY))
			{
				continue;
			}
			result = subItem;
			foreach (BaseItem subItem2 in subItem.SubItems)
			{
				if (subItem2.DisplayRectangle.Contains(clientX, clientY))
				{
					result = subItem2;
					break;
				}
			}
		}
		return result;
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 1131 && ਈ != null)
		{
			ਈ.DoAccesibleDefaultAction();
			ਈ = null;
		}
		((Control)this).WndProc(ref m);
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

	private void _0A3C(PopupItem _0A3D)
	{
		if (ਏ.Contains(_0A3D))
		{
			return;
		}
		if (!((Component)this).DesignMode)
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

	private void జ(SerializeItemEventArgs ؿ)
	{
		if (_0B52 != null)
		{
			_0B52(this, ؿ);
		}
	}

	void ICustomSerialization.InvokeSerializeItem(SerializeItemEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in జ
		this.జ(ؿ);
	}

	private void ఝ(SerializeItemEventArgs ؿ)
	{
		if (_0B53 != null)
		{
			_0B53(this, ؿ);
		}
	}

	void ICustomSerialization.InvokeDeserializeItem(SerializeItemEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ఝ
		this.ఝ(ؿ);
	}

	internal void ఠ(XmlDocument డ)
	{
		XmlElement xmlElement = డ.CreateElement("ExplorerBar");
		డ.AppendChild(xmlElement);
		ణ(xmlElement);
	}

	internal void ణ(XmlElement త)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected I4, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		త.SetAttribute("name", ((Control)this).Name);
		if (((Control)this).Font != null && (((Control)this).Font.Name != SystemInformation.MenuFont.Name || ((Control)this).Font.Size != SystemInformation.MenuFont.Size || ((Control)this).Font.Style != SystemInformation.MenuFont.Style))
		{
			త.SetAttribute("fontname", ((Control)this).Font.Name);
			త.SetAttribute("fontemsize", XmlConvert.ToString(((Control)this).Font.Size));
			త.SetAttribute("fontstyle", XmlConvert.ToString((int)((Control)this).Font.Style));
		}
		త.SetAttribute("width", XmlConvert.ToString(((Control)this).Width));
		త.SetAttribute("height", XmlConvert.ToString(((Control)this).Height));
		if (!ᇯ)
		{
			త.SetAttribute("animate", XmlConvert.ToString(ᇯ));
		}
		XmlElement xmlElement = త.OwnerDocument.CreateElement("items");
		త.AppendChild(xmlElement);
		foreach (BaseItem subItem in ଡ଼.SubItems)
		{
			if (subItem.ShouldSerialize)
			{
				XmlElement xmlElement2 = త.OwnerDocument.CreateElement("item");
				xmlElement.AppendChild(xmlElement2);
				itemSerializationContext.ItemXmlElement = xmlElement2;
				subItem.Serialize(itemSerializationContext);
			}
		}
		XmlElement xmlElement3 = త.OwnerDocument.CreateElement("backstyle");
		త.AppendChild(xmlElement3);
		ሊ(ᇭ, xmlElement3);
	}

	private void ሊ(ElementStyle _0657, XmlElement ላ)
	{
		ElementSerializer.Serialize(_0657, ላ);
	}

	private void ሌ(ElementStyle _0657, XmlElement ላ)
	{
		ElementSerializer.Deserialize(_0657, ላ);
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
		if (!ᇥ)
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

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
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
		if (!((Component)this).DesignMode)
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

	internal void ద(XmlElement త)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		ItemSerializationContext itemSerializationContext = new ItemSerializationContext();
		itemSerializationContext.Serializer = this;
		itemSerializationContext.HasDeserializeItemHandlers = ((ICustomSerialization)this).HasDeserializeItemHandlers;
		itemSerializationContext.HasSerializeItemHandlers = ((ICustomSerialization)this).HasSerializeItemHandlers;
		_09DA.Clear();
		((Control)this).Name = త.GetAttribute("name");
		((Control)this).Size = new Size(XmlConvert.ToInt32(త.GetAttribute("width")), XmlConvert.ToInt32(త.GetAttribute("height")));
		if (త.HasAttribute("animate"))
		{
			ᇯ = XmlConvert.ToBoolean(త.GetAttribute("animate"));
		}
		else
		{
			ᇯ = true;
		}
		if (త.HasAttribute("fontname"))
		{
			string attribute = త.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(త.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(త.GetAttribute("fontstyle"));
			try
			{
				((Control)this).Font = new Font(attribute, num, val);
			}
			catch (Exception)
			{
				((Control)this).Font = SystemFonts.DefaultFont;
			}
		}
		foreach (XmlElement childNode in త.ChildNodes)
		{
			switch (childNode.Name)
			{
			case "items":
				foreach (XmlElement childNode2 in childNode.ChildNodes)
				{
					BaseItem baseItem = BarFunctions._0953(childNode2);
					ଡ଼.SubItems.Add(baseItem);
					itemSerializationContext.ItemXmlElement = childNode2;
					baseItem.Deserialize(itemSerializationContext);
				}
				break;
			case "backstyle":
				ሌ(ᇭ, childNode);
				ᇱ = true;
				break;
			}
		}
	}

	public void LoadDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(FileName);
		ఢ(xmlDocument);
		((Control)this).Refresh();
	}

	internal void ఢ(XmlDocument డ)
	{
		if (డ.FirstChild.Name != "ExplorerBar")
		{
			throw new InvalidOperationException("XML Format not recognized");
		}
		ଡ଼.SubItems.Clear();
		ద(డ.FirstChild as XmlElement);
		((IOwner)this)?.AddShortcutsFromItem((BaseItem)ଡ଼);
		((IOwner)this).InvokeDefinitionLoaded((object)this, new EventArgs());
		RecalcSize();
	}

	public void SaveDefinition(string FileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		ఠ(xmlDocument);
		xmlDocument.Save(FileName);
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
}
