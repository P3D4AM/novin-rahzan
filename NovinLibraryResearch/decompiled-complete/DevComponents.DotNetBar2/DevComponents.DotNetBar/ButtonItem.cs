using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DefaultEvent("Click")]
[DesignTimeVisible(false)]
[Designer("DevComponents.DotNetBar.Design.BaseItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ButtonItem : PopupItem, IPersonalizedMenuItem
{
	public class ButtonItemAccessibleObject : ItemAccessibleObject
	{
		private ButtonItemPartAccessibleObject _2922;

		private ButtonItemPartAccessibleObject _2923;

		internal ButtonItem _291E => base._၉ as ButtonItem;

		private ButtonItemPartAccessibleObject _291F
		{
			get
			{
				if (_2922 == null)
				{
					_2922 = new ButtonItemPartAccessibleObject(_291E, pushButtonPart: true);
				}
				return _2922;
			}
		}

		private ButtonItemPartAccessibleObject _2920
		{
			get
			{
				if (_2923 == null)
				{
					_2923 = new ButtonItemPartAccessibleObject(_291E, pushButtonPart: false);
				}
				return _2923;
			}
		}

		public override AccessibleRole Role
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				if (_2921)
				{
					return (AccessibleRole)20;
				}
				return base.Role;
			}
		}

		private bool _2921
		{
			get
			{
				ButtonItem buttonItem = _291E;
				if (buttonItem.VisibleSubItems > 0 && !buttonItem.AutoExpandOnClick && !buttonItem.IsOnMenu && !buttonItem.IsOnMenuBar)
				{
					return true;
				}
				return false;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				if (_2921)
				{
					if (base._၉ != null && base._၉.IsAccessible)
					{
						AccessibleStates val = (AccessibleStates)0;
						if (!base._၉.Displayed || !base._၉.Visible)
						{
							val = (AccessibleStates)(val | 0x8000);
						}
						else if (!base._၉.GetEnabled())
						{
							return (AccessibleStates)1;
						}
						return (AccessibleStates)256;
					}
					return (AccessibleStates)1;
				}
				AccessibleStates val2 = base.State;
				if (_291E.GetEnabled() && (_291E.Checked || _291E.IsMouseDown))
				{
					val2 = (AccessibleStates)(val2 | 8);
				}
				return val2;
			}
		}

		public override string DefaultAction
		{
			get
			{
				if (_2921)
				{
					return "";
				}
				return base.DefaultAction;
			}
		}

		public override string KeyboardShortcut => "";

		public ButtonItemAccessibleObject(BaseItem owner)
			: base(owner)
		{
		}

		public override int GetChildCount()
		{
			if (_2921)
			{
				return 2;
			}
			return base.GetChildCount();
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (_2921)
			{
				switch (iIndex)
				{
				case 0:
					return (AccessibleObject)(object)_291F;
				case 1:
					return (AccessibleObject)(object)_2920;
				}
			}
			return base.GetChild(iIndex);
		}

		public override AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (_2921)
			{
				return null;
			}
			return base.Navigate(navdir);
		}

		public override void DoDefaultAction()
		{
			if (!_2921)
			{
				base.DoDefaultAction();
			}
		}

		public override AccessibleObject GetSelected()
		{
			if (_2921)
			{
				if (_291E.IsMouseOverExpand)
				{
					return (AccessibleObject)(object)_2920;
				}
				if (_291E.IsMouseOver)
				{
					return (AccessibleObject)(object)_291F;
				}
				return null;
			}
			return base.GetSelected();
		}

		public override AccessibleObject HitTest(int x, int y)
		{
			if (_2921)
			{
				if (((AccessibleObject)_291F).Bounds.Contains(x, y))
				{
					return (AccessibleObject)(object)_291F;
				}
				if (((AccessibleObject)_2920).Bounds.Contains(x, y))
				{
					return (AccessibleObject)(object)_2920;
				}
				return null;
			}
			return base.HitTest(x, y);
		}
	}

	public class ButtonItemPartAccessibleObject : AccessibleObject
	{
		private ButtonItem ᒎ;

		private bool _2922 = true;

		public override string Name
		{
			get
			{
				if (!_2922)
				{
					return "Open";
				}
				if (ᒎ == null)
				{
					return "";
				}
				if (ᒎ.AccessibleName != "")
				{
					return ᒎ.AccessibleName;
				}
				if (ᒎ.Text != null)
				{
					return ᒎ.Text.Replace("&", "");
				}
				return ᒎ.Tooltip;
			}
			set
			{
				ᒎ.AccessibleName = value;
			}
		}

		public override string Description
		{
			get
			{
				if (ᒎ == null)
				{
					return "";
				}
				if (ᒎ.AccessibleDescription != "")
				{
					return ᒎ.AccessibleDescription;
				}
				return ((AccessibleObject)this).Name + " button";
			}
		}

		public override AccessibleRole Role
		{
			get
			{
				if (!_2922)
				{
					return (AccessibleRole)56;
				}
				return (AccessibleRole)62;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				if (ᒎ == null)
				{
					return (AccessibleStates)1;
				}
				AccessibleStates val = (AccessibleStates)0;
				if (!ᒎ.IsAccessible)
				{
					return (AccessibleStates)1;
				}
				if (!ᒎ.Displayed || !ᒎ.Visible)
				{
					return (AccessibleStates)(val | 0x8000);
				}
				if (!ᒎ.GetEnabled())
				{
					return (AccessibleStates)1;
				}
				if (_2922)
				{
					if (ᒎ.IsMouseOver && !ᒎ.IsMouseOverExpand)
					{
						return (AccessibleStates)(val | 0x84);
					}
					return (AccessibleStates)(val | 0x100000);
				}
				if (ᒎ.Expanded || ᒎ.IsMouseOverExpand)
				{
					val = (AccessibleStates)(val | 0x84);
				}
				if (ᒎ.Expanded)
				{
					return (AccessibleStates)(val | 0x200);
				}
				return (AccessibleStates)(val | 0x400);
			}
		}

		public override AccessibleObject Parent => ᒎ.AccessibleObject;

		public override Rectangle Bounds
		{
			get
			{
				object containerControl = ᒎ.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				Rectangle empty = Rectangle.Empty;
				if (_2922)
				{
					Rectangle subItemsRect = ᒎ.SubItemsRect;
					subItemsRect.Offset(ᒎ.Bounds.Location);
					empty = ᒎ.Bounds;
					if (subItemsRect.X == empty.X && subItemsRect.Y == empty.Y)
					{
						empty.X += subItemsRect.Width;
						empty.Width -= subItemsRect.Width;
					}
					else if (subItemsRect.X == empty.X && subItemsRect.Bottom == empty.Bottom && subItemsRect.Y > empty.Y)
					{
						empty.Y += subItemsRect.Height;
						empty.Height -= subItemsRect.Height;
					}
					else
					{
						empty.Width -= subItemsRect.Width;
					}
				}
				else
				{
					empty = ᒎ.SubItemsRect;
					empty.Offset(ᒎ.Bounds.Location);
				}
				if (val != null)
				{
					empty.Location = val.PointToScreen(empty.Location);
				}
				return empty;
			}
		}

		public override string DefaultAction
		{
			get
			{
				if (ᒎ.AccessibleDefaultActionDescription != "")
				{
					return ᒎ.AccessibleDefaultActionDescription;
				}
				if (_2922)
				{
					return "Press";
				}
				return "Open";
			}
		}

		public override string KeyboardShortcut => ᒎ.ShortcutString;

		public ButtonItemPartAccessibleObject(ButtonItem owner, bool pushButtonPart)
		{
			ᒎ = owner;
			_2922 = pushButtonPart;
		}

		public override int GetChildCount()
		{
			if (_2922)
			{
				return 0;
			}
			return ᒎ.SubItems.Count;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (ᒎ == null || _2922)
			{
				return null;
			}
			return ᒎ.SubItems[iIndex].AccessibleObject;
		}

		public override void DoDefaultAction()
		{
			if (ᒎ != null)
			{
				if (_2922)
				{
					ᒎ._ૡ = false;
				}
				else
				{
					ᒎ._ૡ = true;
				}
				object containerControl = ᒎ.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is MenuPanel && !(val is IAccessibilitySupport))
				{
					object containerControl2 = ((MenuPanel)(object)val).ParentItem.ContainerControl;
					val = (Control)((containerControl2 is Control) ? containerControl2 : null);
				}
				if (val is IAccessibilitySupport accessibilitySupport)
				{
					accessibilitySupport.DoDefaultActionItem = ᒎ;
					_089F.PostMessage(val.Handle, 1131, IntPtr.Zero, IntPtr.Zero);
				}
				((AccessibleObject)this).DoDefaultAction();
			}
		}

		public override AccessibleObject GetSelected()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Invalid comparison between Unknown and I4
			if (ᒎ == null || _2922)
			{
				return ((AccessibleObject)this).GetSelected();
			}
			foreach (BaseItem subItem in ᒎ.SubItems)
			{
				if ((subItem.AccessibleObject.State & 0x80) == 128)
				{
					return subItem.AccessibleObject;
				}
			}
			return ((AccessibleObject)this).GetSelected();
		}

		public override AccessibleObject HitTest(int x, int y)
		{
			if (ᒎ == null)
			{
				return ((AccessibleObject)this).HitTest(x, y);
			}
			Point point = new Point(x, y);
			foreach (BaseItem subItem in ᒎ.SubItems)
			{
				object containerControl = subItem.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					Point pt = val.PointToClient(point);
					if (subItem.DisplayRectangle.Contains(pt))
					{
						return subItem.AccessibleObject;
					}
				}
			}
			return ((AccessibleObject)this).HitTest(x, y);
		}

		public override AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Invalid comparison between Unknown and I4
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Invalid comparison between Unknown and I4
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Invalid comparison between Unknown and I4
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Invalid comparison between Unknown and I4
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Invalid comparison between Unknown and I4
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Invalid comparison between Unknown and I4
			if (ᒎ == null || _2922)
			{
				return ((AccessibleObject)this).Navigate(navdir);
			}
			BaseItem baseItem = null;
			if ((int)navdir == 2 || (int)navdir == 4 || (int)navdir == 5)
			{
				if (ᒎ.Parent != null)
				{
					BaseItem parent = ᒎ.Parent;
					baseItem = _104B(parent.SubItems, parent.SubItems.IndexOf(ᒎ) + 1);
				}
			}
			else if ((int)navdir == 7)
			{
				baseItem = _104B(ᒎ.SubItems, 0);
			}
			else if ((int)navdir == 8)
			{
				baseItem = _104E(ᒎ.SubItems, ᒎ.SubItems.Count - 1);
			}
			else if ((int)navdir == 1 || (int)navdir == 3 || (int)navdir == 6)
			{
				BaseItem parent2 = ᒎ.Parent;
				baseItem = _104E(ᒎ.SubItems, parent2.SubItems.IndexOf(ᒎ) - 1);
			}
			if (baseItem != null)
			{
				return baseItem.AccessibleObject;
			}
			return ((AccessibleObject)this).Navigate(navdir);
		}

		private BaseItem _104B(SubItemsCollection _104C, int _104D)
		{
			int count = _104C.Count;
			if (count == 0)
			{
				return null;
			}
			if (_104D >= _104C.Count)
			{
				_104D = _104C.Count - 1;
			}
			for (int i = _104D; i < count; i++)
			{
				if (_104C[i].Visible)
				{
					return _104C[i];
				}
			}
			return null;
		}

		private BaseItem _104E(SubItemsCollection _104C, int _104D)
		{
			if (_104C.Count == 0)
			{
				return null;
			}
			if (_104D >= _104C.Count)
			{
				_104D = _104C.Count - 1;
			}
			for (int num = _104D; num >= 0; num--)
			{
				if (_104C[num].Visible)
				{
					return _104C[num];
				}
			}
			return null;
		}
	}

	private const int _0E64 = 3;

	private EventHandler _0E65;

	private OptionGroupChangingEventHandler _09D5;

	private Image _0E66;

	private Image _0E67;

	private int դ;

	private Image _0E68;

	private int _0E69;

	private Image _0E6A;

	private bool _0E6B;

	private int _0E6C;

	private Icon _0E6D;

	private Image _0E6E;

	private int _0E6F;

	private eButtonStyle _0E70;

	private eImagePosition _0E71;

	private Font ޓ;

	private string _0E72;

	private bool ݡ;

	private bool _0E73;

	private bool ݠ;

	private bool ݟ;

	private Rectangle _0E74;

	private Rectangle _0E75;

	private Rectangle _0E76;

	private int _0E77;

	private int _0E78;

	private Color _0D02 = Color.Empty;

	private Color _0E79 = Color.Empty;

	private bool _0E7A;

	private bool _0E7B;

	private bool _0E7C;

	private bool _0E7D;

	private bool _0E7E;

	private eMenuVisibility _0E7F;

	private bool _0E80;

	private eHotTrackingStyle ກ;

	private Image ຂ;

	private ItemPaintArgs __0E83;

	private int ຄ = 12;

	private string _0E85 = "";

	private Size ຆ = Size.Empty;

	internal bool _ງ;

	private Icon ຈ;

	private bool ຉ;

	private eButtonColor ຊ = eButtonColor.Orange;

	private string _0E8B = "Orange";

	private int ຌ = 8;

	private int ຍ = 6;

	private string ൡ = "";

	private Size ຎ = Size.Empty;

	private bool ຏ;

	private bool ຐ;

	private int ຑ = 65;

	private int ຒ = 12;

	private bool ຓ;

	private int ດ;

	private int ຕ;

	private bool ຖ = true;

	private bool ທ = true;

	private bool ຘ;

	private bool ນ;

	internal bool _ບ;

	private ButtonItemAccessibleObject _ປ;

	private bool _ຜ;

	private string _ຝ = "";

	private eNotificationMarkPosition _ພ = eNotificationMarkPosition.TopRight;

	private int _ຟ;

	private Color _ຠ = Color.Empty;

	private bool _ມ;

	private static readonly Point ຢ = new Point(3, 3);

	private Point _ຣ = ຢ;

	private Color __0EA4 = Color.Empty;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ;

	private Image _ຩ;

	private eButtonImageListSelection _ສ = eButtonImageListSelection.NotSet;

	private bool _ຫ;

	private bool _ຬ;

	private Image _ອ;

	private Bitmap ຮ;

	private int ຯ = 1;

	private int ະ;

	private bool _0EB1;

	private ReaderWriterLock າ = new ReaderWriterLock();

	internal bool _ຳ = true;

	private bool __0EB4 = true;

	private Font __0EB5;

	private eColorSchemePart __0EB6 = eColorSchemePart.None;

	private ShapeDescriptor __0EB7;

	private bool __0EB8;

	private bool __0EB9 = true;

	private MarkupLinkClickEventHandler Ӟ;

	[DefaultValue(false)]
	[Description("Indicates whether images assigned to the button are disposed when button is disposed. Default value is false.")]
	[Category("Behavior")]
	public bool AutoDisposeImages
	{
		get
		{
			return _ຜ;
		}
		set
		{
			_ຜ = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Specifies maximum of 2 character text displayed inside of the notification mark on top of the button.")]
	public string NotificationMarkText
	{
		get
		{
			return _ຝ;
		}
		set
		{
			if (value != _ຝ)
			{
				string ຝ = _ຝ;
				_ຝ = value;
				OnNotificationMarkTextChanged(ຝ, value);
			}
		}
	}

	[Description("Indicates the position of the notification marker within the bounds of the button.")]
	[DefaultValue(eNotificationMarkPosition.TopRight)]
	[Category("Appearance")]
	public eNotificationMarkPosition NotificationMarkPosition
	{
		get
		{
			return _ພ;
		}
		set
		{
			if (value != _ພ)
			{
				eNotificationMarkPosition ພ = _ພ;
				_ພ = value;
				OnNotificationMarkPositionChanged(ພ, value);
			}
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Specifies diameter of notification mark. When set to 0 system default value is used.")]
	public int NotificationMarkSize
	{
		get
		{
			return _ຟ;
		}
		set
		{
			if (value != _ຟ)
			{
				int ຟ = _ຟ;
				_ຟ = value;
				OnNotificationMarkSizeChanged(ຟ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of the notification mark.")]
	public Color NotificationMarkColor
	{
		get
		{
			return _ຠ;
		}
		set
		{
			_ຠ = value;
			Refresh();
		}
	}

	[Description("Specifies the offset for the notification mark relative to its position.")]
	[Category("Appearance")]
	public Point NotificationMarkOffset
	{
		get
		{
			return _ຣ;
		}
		set
		{
			if (value != _ຣ)
			{
				Point ຣ = _ຣ;
				_ຣ = value;
				OnNotificationMarkOffsetChanged(ຣ, value);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eImagePosition.Left)]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[Category("Appearance")]
	public virtual eImagePosition ImagePosition
	{
		get
		{
			return _0E71;
		}
		set
		{
			if (_0E71 != value)
			{
				_0E71 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ImagePosition");
				}
				NeedRecalcSize = true;
				if (Parent != null)
				{
					Parent.NeedRecalcSize = true;
				}
				OnAppearanceChanged();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	[DevCoBrowsable(false)]
	[Browsable(true)]
	[DefaultValue("")]
	public virtual string CustomColorName
	{
		get
		{
			return ൡ;
		}
		set
		{
			ൡ = value;
			Refresh();
		}
	}

	[DevCoBrowsable(false)]
	[Description("Indicates predefined color of button when Office 2007 style is used.")]
	[Category("Appearance")]
	[DefaultValue(eButtonColor.Orange)]
	[Browsable(true)]
	public virtual eButtonColor ColorTable
	{
		get
		{
			return ຊ;
		}
		set
		{
			if (ຊ != value)
			{
				ຊ = value;
				_0E8B = Enum.GetName(typeof(eButtonColor), ຊ);
				Refresh();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Determines the style of the button.")]
	[Category("Appearance")]
	[DefaultValue(eButtonStyle.Default)]
	public virtual eButtonStyle ButtonStyle
	{
		get
		{
			return _0E70;
		}
		set
		{
			if (_0E70 != value)
			{
				_0E70 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ButtonStyle");
				}
				NeedRecalcSize = true;
				if (Displayed && m_Parent != null)
				{
					RecalcSize();
					m_Parent.SubItemSizeChanged(this);
				}
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether the item will auto-expand (display pop-up menu or toolbar) when clicked.")]
	[Category("Behavior")]
	[Browsable(true)]
	public virtual bool AutoExpandOnClick
	{
		get
		{
			return ຉ;
		}
		set
		{
			ຉ = value;
		}
	}

	[Description("Indicates color of the Symbol.")]
	[Category("Appearance")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			Refresh();
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(eSymbolSet.Awesome)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[Description("Indicates the size of the symbol in points.")]
	[DefaultValue(0f)]
	[Category("Appearance")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending.")]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public virtual Icon Icon
	{
		get
		{
			return ຈ;
		}
		set
		{
			NeedRecalcSize = true;
			ຈ = value;
			OnImageChanged();
			OnAppearanceChanged();
			Refresh();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Specifies the Button image which is used when button background is black or very dark. Image is used when button text is white. This is provided for Metro style applications.")]
	[Category("Appearance")]
	public Image ImageAlt
	{
		get
		{
			return _ຩ;
		}
		set
		{
			_ຩ = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("The image that will be displayed on the face of the item.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			NeedRecalcSize = true;
			_0E66 = value;
			OnImageChanged();
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("Specifies the small Button image used by Ribbon control when small image variant is needed")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(null)]
	public virtual Image ImageSmall
	{
		get
		{
			return _0E67;
		}
		set
		{
			_0E67 = value;
			if (!ຆ.IsEmpty || _๖)
			{
				NeedRecalcSize = true;
				OnAppearanceChanged();
				OnImageChanged();
				Refresh();
			}
		}
	}

	private bool _๖ => ນ;

	[Category("Image")]
	[DefaultValue(eButtonImageListSelection.NotSet)]
	[Description("Indicates image size that is used by the button when multiple ImageList controls are used as source for button image.")]
	public virtual eButtonImageListSelection ImageListSizeSelection
	{
		get
		{
			return _ສ;
		}
		set
		{
			_ສ = value;
			NeedRecalcSize = true;
			OnImageChanged();
			OnAppearanceChanged();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool UseSmallImage
	{
		get
		{
			return ນ;
		}
		set
		{
			if (ນ != value)
			{
				ນ = value;
				if (_0E67 != null)
				{
					NeedRecalcSize = true;
					OnImageChanged();
					OnAppearanceChanged();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("The image list image index of the image that will be displayed on the face of the item.")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(-1)]
	[TypeConverter(typeof(ImageIndexConverter))]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			if (ຂ != null)
			{
				ຂ.Dispose();
			}
			ຂ = null;
			if (դ != value)
			{
				դ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ImageIndex");
				}
				if (m_Parent != null)
				{
					OnImageChanged();
					NeedRecalcSize = true;
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	internal bool _๗ => _ຫ;

	internal bool _๘ => _ຬ;

	[Description("The image that will be displayed when mouse hovers over the item.")]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public virtual Image HoverImage
	{
		get
		{
			return _0E68;
		}
		set
		{
			_0E68 = value;
			OnAppearanceChanged();
		}
	}

	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(-1)]
	[Browsable(true)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DevCoBrowsable(true)]
	[Description("The image list image index of the image that will be displayed when mouse hovers over the item.")]
	public virtual int HoverImageIndex
	{
		get
		{
			return _0E69;
		}
		set
		{
			if (_0E69 != value)
			{
				_0E69 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HoverImageIndex");
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Description("The image that will be displayed when item is pressed.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(null)]
	public virtual Image PressedImage
	{
		get
		{
			return _0E6E;
		}
		set
		{
			_0E6E = value;
			OnAppearanceChanged();
		}
	}

	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	[Description("The image list image index of the image that will be displayed when item is pressed.")]
	public virtual int PressedImageIndex
	{
		get
		{
			return _0E6F;
		}
		set
		{
			if (_0E6F != value)
			{
				_0E6F = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "PressedImageIndex");
				}
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[DefaultValue(null)]
	[Description("The image that will be displayed when item is disabled.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public Image DisabledImage
	{
		get
		{
			if (!_0E6B)
			{
				return null;
			}
			return _0E6A;
		}
		set
		{
			_0E6A = value;
			if (value == null)
			{
				_0E6B = false;
			}
			else
			{
				_0E6B = true;
			}
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed when item is disabled.")]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(-1)]
	public int DisabledImageIndex
	{
		get
		{
			return _0E6C;
		}
		set
		{
			if (_0E6C != value)
			{
				if (_0E6A != null)
				{
					_0E6A.Dispose();
				}
				_0E6A = null;
				_0E6C = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "DisabledImageIndex");
				}
				Refresh();
			}
		}
	}

	internal bool _๙
	{
		get
		{
			return ຐ;
		}
		set
		{
			ຐ = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	public Size ImageFixedSize
	{
		get
		{
			return ຆ;
		}
		set
		{
			ຆ = value;
			OnImageChanged();
			if (m_Parent != null && m_Parent is ImageItem)
			{
				((ImageItem)m_Parent).RefreshImageSize();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates fixed size of the button.")]
	[DevCoBrowsable(false)]
	[Category("Layout")]
	public virtual Size FixedSize
	{
		get
		{
			return ຎ;
		}
		set
		{
			if (value.Width < 0)
			{
				value.Width = 0;
			}
			if (value.Height < 0)
			{
				value.Height = 0;
			}
			ຎ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	private Size _0E5A
	{
		get
		{
			Size result = new Size(16, 16);
			IBarImageSize barImageSize = null;
			if (__0E83 != null)
			{
				barImageSize = __0E83.ContainerControl as IBarImageSize;
			}
			if (barImageSize == null)
			{
				barImageSize = ContainerControl as IBarImageSize;
			}
			try
			{
				eBarImageSize eBarImageSize2 = _0EC7(barImageSize);
				if (barImageSize != null)
				{
					switch (eBarImageSize2)
					{
					case eBarImageSize.Medium:
						result = new Size(24, 24);
						return result;
					case eBarImageSize.Large:
						result = new Size(32, 32);
						return result;
					case eBarImageSize.Default:
						break;
					default:
						goto end_IL_0035;
					}
				}
				if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != 0)
				{
					switch (((SideBarPanelItem)m_Parent).ItemImageSize)
					{
					case eBarImageSize.Medium:
						result = new Size(24, 24);
						return result;
					case eBarImageSize.Large:
						result = new Size(32, 32);
						return result;
					}
				}
				end_IL_0035:;
			}
			catch (Exception)
			{
			}
			return result;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool UseParentSubItemsImageSize => true;

	protected virtual bool IsFadeEnabled
	{
		get
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (DesignMode || ກ == eHotTrackingStyle.None || !_ຳ || TextDrawing.UseTextRenderer || effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(effectiveStyle) || !BarUtilities.FadeAnimatorEnabled)
			{
				return false;
			}
			if (_0EFE.ষ && (Parent is ᱭ || (Parent is ය && effectiveStyle == eDotNetBarStyle.Office2010)))
			{
				return false;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				if (val is ItemControl)
				{
					return ((ItemControl)(object)val).স;
				}
				if (val is Bar)
				{
					return ((Bar)(object)val).স;
				}
				if (val is ButtonX)
				{
					return ((ButtonX)(object)val).স;
				}
			}
			return false;
		}
	}

	protected virtual bool IsPulseEnabed => true;

	[Browsable(false)]
	public bool IsPulsing => ຓ;

	[Category("Behavior")]
	[Description("Indicates whether pulse effect started with Pulse method stops automatically when mouse moves over the button.")]
	[Browsable(true)]
	[DefaultValue(true)]
	public virtual bool StopPulseOnMouseOver
	{
		get
		{
			return ຖ;
		}
		set
		{
			ຖ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(12)]
	[Description("Indicates pulse speed. The value must be greater than 0 and less than 128.")]
	[Category("Behavior")]
	public virtual int PulseSpeed
	{
		get
		{
			return ຒ;
		}
		set
		{
			if (value <= 0 || value >= 128)
			{
				throw new ArgumentOutOfRangeException("PulseSpeed value must be greater than 0 and less than 128");
			}
			ຒ = value;
		}
	}

	internal bool _0E5B
	{
		get
		{
			return ݡ;
		}
		set
		{
			ݡ = value;
		}
	}

	internal bool _0E5C
	{
		get
		{
			return _0E73;
		}
		set
		{
			_0E73 = value;
		}
	}

	internal bool _0E5D
	{
		get
		{
			return ݠ;
		}
		set
		{
			ݠ = value;
		}
	}

	internal bool _0E5E
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			m_Expanded = value;
		}
	}

	[Description("Indicates whether button auto-expands on mouse hover when button is used as menu-item and displayed on menu.")]
	[DefaultValue(true)]
	[Browsable(false)]
	[Category("Behavior")]
	public bool AutoExpandMenuItem
	{
		get
		{
			return __0EB4;
		}
		set
		{
			__0EB4 = value;
		}
	}

	protected virtual bool CanShowPopup
	{
		get
		{
			if (ShowSubItems || ShouldAutoExpandOnClick)
			{
				if (SubItems.Count <= 0)
				{
					return PopupType == ePopupType.Container;
				}
				return true;
			}
			return false;
		}
	}

	protected virtual bool ShouldAutoExpandOnClick => ຉ;

	[Browsable(true)]
	[Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public virtual bool AutoCheckOnClick
	{
		get
		{
			return ຘ;
		}
		set
		{
			ຘ = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Bindable(true)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[DefaultValue(false)]
	public virtual bool Checked
	{
		get
		{
			return ݟ;
		}
		set
		{
			if (ݟ == value)
			{
				return;
			}
			if (value && _0E85.Length > 0 && Parent != null)
			{
				ButtonItem buttonItem = null;
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this)
					{
						buttonItem = subItem as ButtonItem;
						if (buttonItem != null && buttonItem.OptionGroup == _0E85 && buttonItem.Checked)
						{
							break;
						}
					}
				}
				OptionGroupChangingEventArgs optionGroupChangingEventArgs = new OptionGroupChangingEventArgs(buttonItem, this);
				InvokeOptionGroupChanging(optionGroupChangingEventArgs);
				if (optionGroupChangingEventArgs.Cancel)
				{
					return;
				}
			}
			ݟ = value;
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "Checked");
			}
			OnCheckedChanged();
			if (Displayed)
			{
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts")]
	[Category("Design")]
	public virtual string AlternateShortCutText
	{
		get
		{
			return _0E72;
		}
		set
		{
			_0E72 = value;
			OnAppearanceChanged();
		}
	}

	internal string _0E5F
	{
		get
		{
			if (AlternateShortCutText != "")
			{
				return AlternateShortCutText;
			}
			return base.ShortcutString;
		}
	}

	internal Rectangle _0E60
	{
		get
		{
			return _0E74;
		}
		set
		{
			_0E74 = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle SubItemsRect
	{
		get
		{
			return _0E76;
		}
		set
		{
			_0E76 = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle TextDrawRect
	{
		get
		{
			return _0E75;
		}
		set
		{
			_0E75 = value;
		}
	}

	[Description("Gets or set the Group item belongs to. The groups allows a user to choose from mutually exclusive options within the group.")]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Browsable(true)]
	[Category("Behavior")]
	public virtual string OptionGroup
	{
		get
		{
			return _0E85;
		}
		set
		{
			if (!(_0E85 != value))
			{
				return;
			}
			_0E85 = value;
			if (_0E85 != "" && ݟ && Parent != null)
			{
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this && subItem is ButtonItem buttonItem && buttonItem.OptionGroup == _0E85 && buttonItem.Checked)
					{
						Checked = false;
					}
				}
			}
			OnAppearanceChanged();
		}
	}

	internal eColorSchemePart _0E61
	{
		get
		{
			return __0EB6;
		}
		set
		{
			__0EB6 = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("The foreground color used to display text.")]
	[Category("Appearance")]
	public virtual Color ForeColor
	{
		get
		{
			return _0D02;
		}
		set
		{
			if (_0D02 != value)
			{
				_0D02 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ForeColor");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("The foreground color used to display text when mouse is over the item.")]
	public virtual Color HotForeColor
	{
		get
		{
			return _0E79;
		}
		set
		{
			if (_0E79 != value)
			{
				_0E79 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HotForeColor");
				}
				if (Displayed && ݡ)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Specifies that text font is underlined when mouse is over the item.")]
	[DevCoBrowsable(true)]
	public virtual bool HotFontUnderline
	{
		get
		{
			return _0E7A;
		}
		set
		{
			if (_0E7A != value)
			{
				_0EDA();
				_0E7A = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HotFontUnderline");
				}
				if (Displayed && ݡ)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies that text font is bold when mouse is over the item.")]
	[DevCoBrowsable(true)]
	public virtual bool HotFontBold
	{
		get
		{
			return _0E7B;
		}
		set
		{
			if (_0E7B != value)
			{
				_0EDA();
				_0E7B = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HotFontBold");
				}
				if (Displayed && ݡ)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Specifies whether the font used to draw the item text is bold.")]
	public virtual bool FontBold
	{
		get
		{
			return _0E7C;
		}
		set
		{
			if (_0E7C != value)
			{
				_0E7C = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "FontBold");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Specifies whether the font used to draw the item text is italic.")]
	[DefaultValue(false)]
	public virtual bool FontItalic
	{
		get
		{
			return _0E7D;
		}
		set
		{
			if (_0E7D != value)
			{
				_0E7D = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "FontItalic");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Specifies whether the font used to draw the item text is underlined.")]
	public virtual bool FontUnderline
	{
		get
		{
			return _0E7E;
		}
		set
		{
			if (_0E7E != value)
			{
				_0E7E = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "FontUnderline");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates the width of the expand part of the button item.")]
	[DefaultValue(12)]
	public virtual int SubItemsExpandWidth
	{
		get
		{
			return ຄ;
		}
		set
		{
			ຄ = value;
			NeedRecalcSize = true;
			if (DesignMode)
			{
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[DevCoBrowsable(false)]
	[Editor("DevComponents.DotNetBar.Design.ButtonItemEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[Description("Collection of sub items.")]
	public override SubItemsCollection SubItems => base.SubItems;

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether button appears as split button. Use AutoExpandOnClick=true if you want to make complete surface of the button display popup when clicked.")]
	[Browsable(true)]
	public virtual bool SplitButton
	{
		get
		{
			return ຏ;
		}
		set
		{
			ຏ = value;
			OnAppearanceChanged();
		}
	}

	[Category("Appearance")]
	[Localizable(true)]
	[DefaultValue("")]
	[Browsable(true)]
	[Description("The text contained in the item.")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DevCoBrowsable(true)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	internal int _0E62
	{
		get
		{
			return _0E77;
		}
		set
		{
			_0E77 = value;
		}
	}

	internal int _0E63
	{
		get
		{
			return _0E78;
		}
		set
		{
			_0E78 = value;
		}
	}

	[Description("Indicates amount of padding added horizontally to the button images when not on menus")]
	[DefaultValue(8)]
	[Category("Layout")]
	[Browsable(true)]
	public virtual int ImagePaddingHorizontal
	{
		get
		{
			return ຌ;
		}
		set
		{
			ຌ = value;
			OnAppearanceChanged();
		}
	}

	[Description("Indicates amount of padding added vertically to the button images when not on menus")]
	[DefaultValue(6)]
	[Category("Layout")]
	[Browsable(true)]
	public virtual int ImagePaddingVertical
	{
		get
		{
			return ຍ;
		}
		set
		{
			ຍ = value;
			OnAppearanceChanged();
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ShapeTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[MergableProperty(false)]
	[TypeConverter("DevComponents.DotNetBar.Design.ShapeStringConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public virtual ShapeDescriptor Shape
	{
		get
		{
			return __0EB7;
		}
		set
		{
			if (__0EB7 != value)
			{
				__0EB7 = value;
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(eMenuVisibility.VisibleAlways)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	[Browsable(true)]
	public virtual eMenuVisibility MenuVisibility
	{
		get
		{
			return _0E7F;
		}
		set
		{
			if (_0E7F != value)
			{
				_0E7F = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "MenuVisibility");
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool RecentlyUsed
	{
		get
		{
			return _0E80;
		}
		set
		{
			if (_0E80 != value)
			{
				_0E80 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "RecentlyUsed");
				}
				OnAppearanceChanged();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsMouseOver => ݡ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOverExpand => _0E73;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsMouseDown => ݠ;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[DefaultValue(eHotTrackingStyle.Default)]
	public virtual eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return ກ;
		}
		set
		{
			if (ກ != value)
			{
				ກ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HotTrackingStyle");
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImageList ImageList
	{
		get
		{
			if (GetOwner() is IOwner owner)
			{
				return owner.Images;
			}
			return null;
		}
	}

	[Category("Ribbon")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Localizable(true)]
	[Description("Indicates whether the button text is automatically wrapped over multiple lines when button is used on RibbonBar control.")]
	public virtual bool RibbonWordWrap
	{
		get
		{
			return ທ;
		}
		set
		{
			if (ທ != value)
			{
				ທ = value;
				OnAppearanceChanged();
			}
		}
	}

	[Description("Indicates whether image animation is enabled.")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public bool EnableImageAnimation
	{
		get
		{
			return __0EB8;
		}
		set
		{
			if (__0EB8 != value)
			{
				__0EB8 = value;
				Refresh();
			}
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	public event EventHandler CheckedChanged
	{
		add
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before an item in option group is checked and provides opportunity to cancel that.")]
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

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public ButtonItem()
		: this("", "")
	{
	}

	public ButtonItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public ButtonItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		m_IsContainer = false;
		_0E66 = null;
		դ = -1;
		_0E68 = null;
		_0E69 = -1;
		_0E6A = null;
		_0E6C = -1;
		_0E6E = null;
		_0E6F = -1;
		ݡ = false;
		ݠ = false;
		_0E70 = eButtonStyle.Default;
		_0E71 = eImagePosition.Left;
		ޓ = null;
		_0E74 = Rectangle.Empty;
		_0E75 = Rectangle.Empty;
		_0E76 = Rectangle.Empty;
		_0E77 = 0;
		_0E78 = 0;
		_0E72 = "";
	}

	public override BaseItem Copy()
	{
		ButtonItem buttonItem = new ButtonItem(m_Name);
		CopyToItem(buttonItem);
		return buttonItem;
	}

	internal void _0EBA(ButtonItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		ButtonItem buttonItem = copy as ButtonItem;
		base.CopyToItem((BaseItem)buttonItem);
		if (_0E66 != null)
		{
			buttonItem.Image = (Image)_0E66.Clone();
		}
		if (_0E67 != null)
		{
			buttonItem.ImageSmall = (Image)_0E67.Clone();
		}
		if (_0E68 != null)
		{
			buttonItem.HoverImage = (Image)_0E68.Clone();
		}
		if (_0E6E != null)
		{
			buttonItem.PressedImage = (Image)_0E6E.Clone();
		}
		if (_0E6A != null && _0E6B)
		{
			buttonItem.DisabledImage = (Image)_0E6A.Clone();
		}
		if (ຈ != null)
		{
			object obj = ຈ.Clone();
			buttonItem.Icon = (Icon)((obj is Icon) ? obj : null);
		}
		buttonItem.ButtonStyle = _0E70;
		buttonItem.ImagePosition = _0E71;
		buttonItem.OptionGroup = _0E85;
		buttonItem.Checked = ݟ;
		buttonItem.PopupType = PopupType;
		buttonItem.AlternateShortCutText = AlternateShortCutText;
		if (դ >= 0)
		{
			buttonItem._0EBE(դ);
		}
		if (_0E6F >= 0)
		{
			buttonItem.PressedImageIndex = _0E6F;
		}
		if (_0E69 >= 0)
		{
			buttonItem.HoverImageIndex = _0E69;
		}
		if (_0E6C >= 0)
		{
			buttonItem.DisabledImageIndex = _0E6C;
		}
		if (!_0D02.IsEmpty)
		{
			buttonItem.ForeColor = _0D02;
		}
		buttonItem.HotFontBold = _0E7B;
		buttonItem.HotFontUnderline = _0E7A;
		if (!_0E79.IsEmpty)
		{
			buttonItem.HotForeColor = _0E79;
		}
		buttonItem.HotTrackingStyle = ກ;
		buttonItem.MenuVisibility = _0E7F;
		buttonItem.AutoExpandOnClick = AutoExpandOnClick;
		buttonItem.AlternateShortCutText = AlternateShortCutText;
		buttonItem.SubItemsExpandWidth = SubItemsExpandWidth;
		buttonItem._0E65 = _0E65;
		buttonItem._09D5 = _09D5;
		if (!ImageFixedSize.IsEmpty)
		{
			buttonItem.ImageFixedSize = ImageFixedSize;
		}
		buttonItem.ImagePaddingHorizontal = ຌ;
		buttonItem.ImagePaddingVertical = ຍ;
		buttonItem.FixedSize = FixedSize;
		buttonItem.RibbonWordWrap = RibbonWordWrap;
		buttonItem.AutoCheckOnClick = AutoCheckOnClick;
		if (Shape != null)
		{
			buttonItem.Shape = Shape;
		}
		buttonItem.ColorTable = ColorTable;
		buttonItem.CustomColorName = CustomColorName;
		buttonItem.AutoExpandMenuItem = AutoExpandMenuItem;
		buttonItem.EnableMarkup = EnableMarkup;
		buttonItem.SplitButton = SplitButton;
		buttonItem.NotificationMarkColor = NotificationMarkColor;
		buttonItem.NotificationMarkPosition = NotificationMarkPosition;
		buttonItem.NotificationMarkSize = NotificationMarkSize;
		buttonItem.NotificationMarkText = NotificationMarkText;
		buttonItem.Symbol = Symbol;
		buttonItem.SymbolSet = SymbolSet;
		buttonItem.SymbolColor = SymbolColor;
		buttonItem.SymbolSize = SymbolSize;
	}

	protected override void Dispose(bool disposing)
	{
		_0ECF();
		StopPulse();
		ໂ();
		_0EDA();
		_0EBC();
		if ((BarUtilities.DisposeItemImages && !DesignMode) || _ຜ)
		{
			BarUtilities.ᆟ(ref _0E66);
			BarUtilities.ᆟ(ref ຈ);
			BarUtilities.ᆟ(ref _0E67);
			BarUtilities.ᆟ(ref _0E68);
			BarUtilities.ᆟ(ref _0E6A);
			BarUtilities.ᆟ(ref _0E6E);
			BarUtilities.ᆟ(ref _0E6D);
		}
		base.Dispose(disposing);
	}

	private void _0EBC()
	{
		if (ຂ != null)
		{
			ຂ.Dispose();
		}
		ຂ = null;
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		if (_ປ == null)
		{
			_ປ = new ButtonItemAccessibleObject(this);
		}
		return (AccessibleObject)(object)_ປ;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("ImagePosition", XmlConvert.ToString((int)_0E71));
		itemXmlElement.SetAttribute("ButtonStyle", XmlConvert.ToString((int)_0E70));
		itemXmlElement.SetAttribute("Checked", XmlConvert.ToString(ݟ));
		itemXmlElement.SetAttribute("VerticalPadding", XmlConvert.ToString(_0E77));
		itemXmlElement.SetAttribute("HorizontalPadding", XmlConvert.ToString(_0E78));
		itemXmlElement.SetAttribute("MenuVisibility", XmlConvert.ToString((int)_0E7F));
		itemXmlElement.SetAttribute("RecentlyUsed", XmlConvert.ToString(_0E80));
		if (_0E72 != "")
		{
			itemXmlElement.SetAttribute("AlternateShortcutText", _0E72);
		}
		if (!_0D02.IsEmpty)
		{
			itemXmlElement.SetAttribute("forecolor", BarFunctions.ColorToString(_0D02));
		}
		if (ກ != 0)
		{
			itemXmlElement.SetAttribute("hottrack", XmlConvert.ToString((int)ກ));
		}
		if (_0E7B)
		{
			itemXmlElement.SetAttribute("hotfb", XmlConvert.ToString(_0E7B));
		}
		if (_0E7A)
		{
			itemXmlElement.SetAttribute("hotfu", XmlConvert.ToString(_0E7A));
		}
		if (!_0E79.IsEmpty)
		{
			itemXmlElement.SetAttribute("hotclr", BarFunctions.ColorToString(_0E79));
		}
		if (_0E85 != "")
		{
			itemXmlElement.SetAttribute("optiongroup", _0E85);
		}
		if (_0E7C)
		{
			itemXmlElement.SetAttribute("fontbold", XmlConvert.ToString(_0E7C));
		}
		if (_0E7D)
		{
			itemXmlElement.SetAttribute("fontitalic", XmlConvert.ToString(_0E7D));
		}
		if (_0E7E)
		{
			itemXmlElement.SetAttribute("fontunderline", XmlConvert.ToString(_0E7E));
		}
		if (ຉ)
		{
			itemXmlElement.SetAttribute("autoexpandclick", XmlConvert.ToString(ຉ));
		}
		if (ൡ != "")
		{
			itemXmlElement.SetAttribute("CustomColorName", ൡ);
		}
		if (ຊ != eButtonColor.Orange)
		{
			itemXmlElement.SetAttribute("ColorTable", Enum.GetName(ຊ.GetType(), ຊ));
		}
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (_0E66 != null || դ >= 0 || _0E68 != null || _0E69 >= 0 || _0E6A != null || _0E6C >= 0 || _0E6E != null || _0E6F >= 0 || ຈ != null || _0E67 != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (դ >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(դ));
			}
			if (_0E69 >= 0)
			{
				xmlElement.SetAttribute("hoverimageindex", XmlConvert.ToString(_0E69));
			}
			if (_0E6C >= 0)
			{
				xmlElement.SetAttribute("disabledimageindex", XmlConvert.ToString(_0E6C));
			}
			if (_0E6F >= 0)
			{
				xmlElement.SetAttribute("pressedimageindex", XmlConvert.ToString(_0E6F));
			}
			if (_0E66 != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E66, xmlElement2);
			}
			if (_0E67 != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("imagesmall");
				xmlElement2.SetAttribute("type", "small");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E67, xmlElement2);
			}
			if (_0E68 != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "hover");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E68, xmlElement2);
			}
			if (_0E6A != null && _0E6B)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "disabled");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E6A, xmlElement2);
			}
			if (_0E6E != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "pressed");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E6E, xmlElement2);
			}
			if (ຈ != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "icon");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeIcon(ຈ, xmlElement2);
			}
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		_0E71 = (eImagePosition)XmlConvert.ToInt32(itemXmlElement.GetAttribute("ImagePosition"));
		_0E70 = (eButtonStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("ButtonStyle"));
		ݟ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("Checked"));
		_0E77 = XmlConvert.ToInt32(itemXmlElement.GetAttribute("VerticalPadding"));
		_0E78 = XmlConvert.ToInt32(itemXmlElement.GetAttribute("HorizontalPadding"));
		_0E7F = (eMenuVisibility)XmlConvert.ToInt32(itemXmlElement.GetAttribute("MenuVisibility"));
		_0E80 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("RecentlyUsed"));
		if (itemXmlElement.HasAttribute("forecolor"))
		{
			_0D02 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("forecolor"));
		}
		else
		{
			_0D02 = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hottrack"))
		{
			ກ = (eHotTrackingStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("hottrack"));
		}
		else
		{
			ກ = eHotTrackingStyle.Default;
		}
		if (itemXmlElement.HasAttribute("hotclr"))
		{
			_0E79 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("hotclr"));
		}
		else
		{
			_0E79 = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hotfb"))
		{
			_0E7B = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfb"));
		}
		else
		{
			_0E7B = false;
		}
		if (itemXmlElement.HasAttribute("hotfu"))
		{
			_0E7A = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfu"));
		}
		else
		{
			_0E7A = false;
		}
		if (itemXmlElement.HasAttribute("optiongroup"))
		{
			_0E85 = itemXmlElement.GetAttribute("optiongroup");
		}
		else
		{
			_0E85 = "";
		}
		if (itemXmlElement.HasAttribute("fontbold"))
		{
			_0E7C = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("fontbold"));
		}
		else
		{
			_0E7C = false;
		}
		if (itemXmlElement.HasAttribute("fontitalic"))
		{
			_0E7D = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("fontitalic"));
		}
		else
		{
			_0E7D = false;
		}
		if (itemXmlElement.HasAttribute("fontunderline"))
		{
			_0E7E = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("fontunderline"));
		}
		else
		{
			_0E7E = false;
		}
		if (itemXmlElement.HasAttribute("AlternateShortcutText"))
		{
			_0E72 = itemXmlElement.GetAttribute("AlternateShortcutText");
		}
		else
		{
			_0E72 = "";
		}
		if (itemXmlElement.HasAttribute("autoexpandclick"))
		{
			ຉ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("autoexpandclick"));
		}
		else
		{
			ຉ = false;
		}
		if (itemXmlElement.HasAttribute("CustomColorName"))
		{
			ൡ = itemXmlElement.GetAttribute("CustomColorName");
		}
		else
		{
			ൡ = "";
		}
		if (itemXmlElement.HasAttribute("ColorTable"))
		{
			ຊ = (eButtonColor)Enum.Parse(ຊ.GetType(), itemXmlElement.GetAttribute("ColorTable"));
		}
		else
		{
			ຊ = eButtonColor.Orange;
		}
		դ = -1;
		_0E69 = -1;
		_0E6C = -1;
		_0E6F = -1;
		ຈ = null;
		_0E66 = null;
		_0E67 = null;
		_0E68 = null;
		_0E6A = null;
		_0E6B = false;
		_0E6E = null;
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			if (!(childNode.Name == "images"))
			{
				continue;
			}
			if (childNode.HasAttribute("imageindex"))
			{
				դ = XmlConvert.ToInt32(childNode.GetAttribute("imageindex"));
			}
			if (childNode.HasAttribute("hoverimageindex"))
			{
				_0E69 = XmlConvert.ToInt32(childNode.GetAttribute("hoverimageindex"));
			}
			if (childNode.HasAttribute("disabledimageindex"))
			{
				_0E6C = XmlConvert.ToInt32(childNode.GetAttribute("disabledimageindex"));
			}
			if (childNode.HasAttribute("pressedimageindex"))
			{
				_0E6F = XmlConvert.ToInt32(childNode.GetAttribute("pressedimageindex"));
			}
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				switch (childNode2.GetAttribute("type"))
				{
				case "default":
					_0E66 = BarFunctions.DeserializeImage(childNode2);
					դ = -1;
					break;
				case "icon":
					ຈ = BarFunctions.DeserializeIcon(childNode2);
					դ = -1;
					break;
				case "hover":
					_0E68 = BarFunctions.DeserializeImage(childNode2);
					_0E69 = -1;
					break;
				case "disabled":
					_0E6A = BarFunctions.DeserializeImage(childNode2);
					_0E6C = -1;
					_0E6B = true;
					break;
				case "pressed":
					_0E6E = BarFunctions.DeserializeImage(childNode2);
					_0E6F = -1;
					break;
				case "small":
					_0E67 = BarFunctions.DeserializeImage(childNode2);
					break;
				}
			}
			break;
		}
		OnImageChanged();
	}

	protected virtual void OnNotificationMarkTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_ມ = true;
		}
		Refresh();
	}

	protected virtual void OnNotificationMarkPositionChanged(eNotificationMarkPosition oldValue, eNotificationMarkPosition newValue)
	{
		if (!string.IsNullOrEmpty(_ຝ))
		{
			Refresh();
		}
	}

	protected virtual void OnNotificationMarkSizeChanged(int oldValue, int newValue)
	{
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNotificationMarkColor()
	{
		return !_ຠ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNotificationMarkColor()
	{
		NotificationMarkColor = Color.Empty;
	}

	protected override Rectangle GetInvalidateBounds()
	{
		if (_ມ || !string.IsNullOrEmpty(_ຝ))
		{
			_ມ = false;
			Rectangle invalidateBounds = base.GetInvalidateBounds();
			invalidateBounds.Inflate(6, 6);
			return invalidateBounds;
		}
		return base.GetInvalidateBounds();
	}

	protected virtual void OnNotificationMarkOffsetChanged(Point oldValue, Point newValue)
	{
		Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNotificationMarkOffset()
	{
		return _ຣ != ຢ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNotificationMarkOffset()
	{
		NotificationMarkOffset = ຢ;
	}

	internal virtual string ຽ()
	{
		return _0E8B;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !__0EA4.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	internal void _0EBE(int _0EBF)
	{
		դ = _0EBF;
	}

	protected override void ScaleItem(SizeF factor)
	{
		OnImageChanged();
		base.ScaleItem(factor);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void OnImageChanged()
	{
		base.OnImageChanged();
		StopPulse();
		ໂ();
		if (_0E6A != null && !_0E6B)
		{
			_0E6A.Dispose();
			_0E6A = null;
		}
		if (_0E6D != null)
		{
			_0E6D.Dispose();
			_0E6D = null;
		}
		ᇎ ᇎ2 = _0EC5(_1937.ሬ, Color.Black);
		if (ᇎ2 != null)
		{
			if (_0E66 != null)
			{
				IBarImageSize barImageSize = null;
				if (__0E83 != null)
				{
					barImageSize = __0E83.ContainerControl as IBarImageSize;
				}
				if (barImageSize == null)
				{
					barImageSize = ContainerControl as IBarImageSize;
				}
				eBarImageSize eBarImageSize2 = _0EC7(barImageSize);
				if (barImageSize != null && eBarImageSize2 != 0)
				{
					if (eBarImageSize2 == eBarImageSize.Medium)
					{
						ImageSize = new Size(32, 32);
					}
					else
					{
						ImageSize = new Size(48, 48);
					}
				}
				else
				{
					ImageSize = new Size(ᇎ2.ܐ, ᇎ2._0711);
				}
			}
			else
			{
				ImageSize = new Size(ᇎ2.ܐ, ᇎ2._0711);
			}
			if (ᇎ2.ᇑ == _0E66)
			{
				_ຫ = ImageAnimator.CanAnimate(_0E66);
			}
			else
			{
				_ຫ = false;
			}
			ᇎ2.Dispose();
		}
		else
		{
			ImageSize = Dpi.Size(ImageItem._ನ);
		}
		if (m_Parent != null && m_Parent is ImageItem imageItem)
		{
			imageItem.RefreshSubItemImageSize();
		}
	}

	internal void ເ(Image ແ)
	{
		if (ImageAnimator.CanAnimate(ແ))
		{
			_ຬ = true;
			_ອ = ແ;
			ImageAnimator.Animate(ແ, (EventHandler)ໃ);
		}
	}

	internal void ໂ()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		if (_ຬ)
		{
			_ຬ = false;
			Image ອ = _ອ;
			_ອ = null;
			if (ອ != null)
			{
				ImageAnimator.StopAnimate(ອ, (EventHandler)ໃ);
				FrameDimension val = new FrameDimension(ອ.FrameDimensionsList[0]);
				ອ.SelectActiveFrame(val, 0);
			}
		}
	}

	private void ໃ(object ໄ, EventArgs ؿ)
	{
		if (_ຬ && Visible)
		{
			Refresh();
			Update();
		}
		else
		{
			ໂ();
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		if (DesignMode || ((դ >= 0 || ຈ != null) && ImageSize.Width == ImageItem._ನ.Width && ImageSize.Height == ImageItem._ನ.Height))
		{
			OnImageChanged();
		}
		base.OnContainerChanged(objOldContainer);
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		bool flag = ݡ;
		າ.AcquireReaderLock(-1);
		try
		{
			if (ຮ != null)
			{
				ݡ = false;
			}
			try
			{
				__0E83 = p;
				if (EffectiveStyle == eDotNetBarStyle.Office2000 && !base.IsThemed)
				{
					_0DF5(p);
				}
				else
				{
					RenderButton(p);
					if (ຮ != null && !ຐ)
					{
						Graphics graphics = p.Graphics;
						Rectangle displayRectangle = DisplayRectangle;
						ColorMatrix val = new ColorMatrix();
						val[3, 3] = (float)ະ / 255f;
						ImageAttributes val2 = new ImageAttributes();
						try
						{
							val2.SetColorMatrix(val);
							graphics.DrawImage((Image)(object)ຮ, displayRectangle, 0, 0, displayRectangle.Width, displayRectangle.Height, (GraphicsUnit)2, val2);
							return;
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
				}
			}
			finally
			{
				__0E83 = null;
				ݡ = flag;
			}
		}
		finally
		{
			າ.ReleaseReaderLock();
		}
		DrawInsertMarker(p.Graphics);
	}

	protected virtual void RenderButton(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
			p.ButtonItemRendererEventArgs.ButtonItem = this;
			p.ButtonItemRendererEventArgs.က = p;
			ໝ.DrawButtonItem(p.ButtonItemRendererEventArgs);
		}
		else
		{
			_192F obj = ᣓ.ᤈ(this);
			if (obj != null)
			{
				obj.PaintButton(this, p);
			}
			else if (EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				_0DF5(p);
			}
			else
			{
				_0DF3(p);
			}
		}
		if (!string.IsNullOrEmpty(_ຝ))
		{
			_1A6E._139A(p.Graphics, Bounds, _ພ, _ຝ, new Size(_ຟ, _ຟ), _ຣ, _ຠ);
		}
	}

	private void _0DF3(ItemPaintArgs _0BD2)
	{
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_1a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a24: Expected O, but got Unknown
		//IL_0fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbc: Expected O, but got Unknown
		//IL_0fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Expected O, but got Unknown
		//IL_1284: Unknown result type (might be due to invalid IL or missing references)
		//IL_128b: Expected O, but got Unknown
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0f: Expected O, but got Unknown
		//IL_1b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b17: Expected O, but got Unknown
		//IL_14e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ee: Expected O, but got Unknown
		//IL_150b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1512: Expected O, but got Unknown
		//IL_0c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c37: Expected O, but got Unknown
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b24: Expected O, but got Unknown
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Expected O, but got Unknown
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Expected O, but got Unknown
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b54: Expected O, but got Unknown
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Expected O, but got Unknown
		//IL_1457: Unknown result type (might be due to invalid IL or missing references)
		//IL_145e: Expected O, but got Unknown
		//IL_1ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffc: Expected O, but got Unknown
		//IL_1b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b51: Expected O, but got Unknown
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eea: Expected O, but got Unknown
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Expected O, but got Unknown
		//IL_187a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1881: Expected O, but got Unknown
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_1809: Unknown result type (might be due to invalid IL or missing references)
		//IL_1810: Expected O, but got Unknown
		//IL_18b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bb: Expected O, but got Unknown
		//IL_11d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11df: Expected O, but got Unknown
		//IL_17c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cc: Expected O, but got Unknown
		//IL_2665: Unknown result type (might be due to invalid IL or missing references)
		//IL_266c: Expected O, but got Unknown
		//IL_263a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2641: Expected O, but got Unknown
		//IL_20dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e3: Expected O, but got Unknown
		//IL_207d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2084: Expected O, but got Unknown
		bool isOnMenu = _0BD2.IsOnMenu;
		bool isOnMenuBar = _0BD2.IsOnMenuBar;
		bool isThemed = base.IsThemed;
		if (!isOnMenu && !isOnMenuBar && isThemed)
		{
			ThemedButtonItemPainter.PaintButton(this, _0BD2);
			return;
		}
		bool flag = ݡ;
		if (isOnMenu && Expanded && _0BD2.ContainerControl != null && _0BD2.ContainerControl.Parent != null && !_0BD2.ContainerControl.Parent.Bounds.Contains(Control.MousePosition))
		{
			flag = true;
		}
		Graphics graphics = _0BD2.Graphics;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
		Color empty = Color.Empty;
		empty = ((flag && !_0E79.IsEmpty) ? _0E79 : ((!_0D02.IsEmpty) ? _0D02 : ((flag && !m_Expanded && ກ != eHotTrackingStyle.Image) ? _0BD2.Colors.ItemHotText : (m_Expanded ? _0BD2.Colors.ItemExpandedText : ((!isThemed || !isOnMenuBar || !(_0BD2.Colors.ItemText == SystemColors.ControlText)) ? _0BD2.Colors.ItemText : SystemColors.MenuText)))));
		Font val = null;
		eTextFormat eTextFormat2 = _0BD2.ButtonStringFormat;
		ᇎ ᇎ2 = _0EC5();
		Size empty2 = Size.Empty;
		val = ((ޓ == null) ? _໘(_0BD2, _໙: false) : ޓ);
		if (ᇎ2 != null)
		{
			empty2 = ImageSize;
			rectangle = ((isOnMenu || (_0E71 != eImagePosition.Top && _0E71 != eImagePosition.Bottom)) ? new Rectangle(_0E74.X, _0E74.Y, _0E74.Width, _0E74.Height) : new Rectangle(_0E74.X, _0E74.Y, rectangle2.Width, _0E74.Height));
			rectangle.Offset(rectangle2.Left, rectangle2.Top);
			rectangle.Offset((rectangle.Width - empty2.Width) / 2, (rectangle.Height - empty2.Height) / 2);
			rectangle.Width = empty2.Width;
			rectangle.Height = empty2.Height;
		}
		if (isOnMenu)
		{
			if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
			{
				if (!_0BD2.Colors.MenuUnusedSide2.IsEmpty)
				{
					LinearGradientBrush val2 = BarFunctions.ॴ(new Rectangle(m_Rect.Left, m_Rect.Top, _0E74.Right, m_Rect.Height), _0BD2.Colors.MenuUnusedSide, _0BD2.Colors.MenuUnusedSide2, _0BD2.Colors.MenuUnusedSideGradientAngle);
					graphics.FillRectangle((Brush)(object)val2, m_Rect.Left, m_Rect.Top, _0E74.Right, m_Rect.Height);
					((Brush)val2).Dispose();
				}
				else
				{
					SolidBrush val3 = new SolidBrush(_0BD2.Colors.MenuUnusedSide);
					try
					{
						graphics.FillRectangle((Brush)(object)val3, m_Rect.Left, m_Rect.Top, _0E74.Right, m_Rect.Height);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
			}
			else if (!_0BD2.Colors.MenuSide2.IsEmpty)
			{
				LinearGradientBrush val4 = BarFunctions.ॴ(new Rectangle(m_Rect.Left, m_Rect.Top, _0E74.Right, m_Rect.Height), _0BD2.Colors.MenuSide, _0BD2.Colors.MenuSide2, _0BD2.Colors.MenuSideGradientAngle);
				graphics.FillRectangle((Brush)(object)val4, m_Rect.Left, m_Rect.Top, _0E74.Right, m_Rect.Height);
				((Brush)val4).Dispose();
			}
			else
			{
				SolidBrush val5 = new SolidBrush(_0BD2.Colors.MenuSide);
				try
				{
					graphics.FillRectangle((Brush)(object)val5, m_Rect.Left, m_Rect.Top, _0E74.Right, m_Rect.Height);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
		}
		else if (!_0BD2.Colors.ItemBackground.IsEmpty)
		{
			if (_0BD2.Colors.ItemBackground2.IsEmpty)
			{
				SolidBrush val6 = new SolidBrush(_0BD2.Colors.ItemBackground);
				try
				{
					graphics.FillRectangle((Brush)(object)val6, m_Rect);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			else
			{
				LinearGradientBrush val7 = BarFunctions.ॴ(DisplayRectangle, _0BD2.Colors.ItemBackground, _0BD2.Colors.ItemBackground2, _0BD2.Colors.ItemBackgroundGradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val7, DisplayRectangle);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
		}
		else if (!GetEnabled(_0BD2.ContainerControl) && !_0BD2.Colors.ItemDisabledBackground.IsEmpty)
		{
			SolidBrush val8 = new SolidBrush(_0BD2.Colors.ItemDisabledBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val8, m_Rect);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		if (GetEnabled(_0BD2.ContainerControl) || DesignMode)
		{
			if (m_Expanded && !isOnMenu)
			{
				if (_0BD2.Colors.ItemExpandedBackground2.IsEmpty)
				{
					Rectangle rectangle3 = new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
					if (_0BD2.Colors.ItemExpandedShadow.IsEmpty)
					{
						rectangle3.Width -= 2;
					}
					SolidBrush val9 = new SolidBrush(_0BD2.Colors.ItemExpandedBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val9, rectangle3);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val10 = BarFunctions.ॴ(new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width - 2, rectangle2.Height), _0BD2.Colors.ItemExpandedBackground, _0BD2.Colors.ItemExpandedBackground2, _0BD2.Colors.ItemExpandedBackgroundGradientAngle);
					Rectangle rectangle4 = new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
					if (_0BD2.Colors.ItemExpandedShadow.IsEmpty)
					{
						rectangle4.Width -= 2;
					}
					graphics.FillRectangle((Brush)(object)val10, rectangle4);
					((Brush)val10).Dispose();
				}
				Point[] array = ((m_Orientation != 0 || PopupSide != 0) ? new Point[5] : new Point[4]);
				array[0].X = rectangle2.Left;
				array[0].Y = rectangle2.Top + rectangle2.Height - 1;
				array[1].X = rectangle2.Left;
				array[1].Y = rectangle2.Top;
				if (m_Orientation == eOrientation.Horizontal)
				{
					array[2].X = rectangle2.Left + rectangle2.Width - 3;
				}
				else
				{
					array[2].X = rectangle2.Left + rectangle2.Width - 1;
				}
				array[2].Y = rectangle2.Top;
				if (m_Orientation == eOrientation.Horizontal)
				{
					array[3].X = rectangle2.Left + rectangle2.Width - 3;
				}
				else
				{
					array[3].X = rectangle2.Left + rectangle2.Width - 1;
				}
				array[3].Y = rectangle2.Top + rectangle2.Height - 1;
				if (m_Orientation == eOrientation.Vertical || PopupSide != 0)
				{
					array[4].X = rectangle2.Left;
					array[4].Y = rectangle2.Top + rectangle2.Height - 1;
				}
				if (!_0BD2.Colors.ItemExpandedBorder.IsEmpty)
				{
					Pen val11 = new Pen(_0BD2.Colors.ItemExpandedBorder, 1f);
					try
					{
						graphics.DrawLines(val11, array);
					}
					finally
					{
						((IDisposable)val11)?.Dispose();
					}
				}
				if (!_0BD2.Colors.ItemExpandedShadow.IsEmpty && m_Orientation == eOrientation.Horizontal)
				{
					SolidBrush val12 = new SolidBrush(_0BD2.Colors.ItemExpandedShadow);
					try
					{
						graphics.FillRectangle((Brush)(object)val12, rectangle2.Left + rectangle2.Width - 2, rectangle2.Top + 2, 2, rectangle2.Height - 2);
					}
					finally
					{
						((IDisposable)val12)?.Dispose();
					}
				}
			}
			if ((flag && ກ != eHotTrackingStyle.None) || (m_Expanded && !isOnMenu))
			{
				if (!m_Expanded || isOnMenu)
				{
					Rectangle rectangle5 = rectangle2;
					if (isOnMenu)
					{
						rectangle5 = new Rectangle(rectangle2.Left + 1, rectangle2.Top, rectangle2.Width - 2, rectangle2.Height);
					}
					if (DesignMode && Focused)
					{
						rectangle5 = m_Rect;
						rectangle5.Inflate(-1, -1);
						_1A6B._1A6C(graphics, rectangle5, _0BD2.Colors.ItemDesignTimeBorder);
					}
					else if (ݠ)
					{
						if (ກ == eHotTrackingStyle.Image)
						{
							rectangle5 = rectangle;
							rectangle5.Inflate(2, 2);
						}
						if (_0BD2.Colors.ItemPressedBackground2.IsEmpty)
						{
							SolidBrush val13 = new SolidBrush(_0BD2.Colors.ItemPressedBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val13, rectangle5);
							}
							finally
							{
								((IDisposable)val13)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val14 = BarFunctions.ॴ(rectangle5, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val14, rectangle5);
							((Brush)val14).Dispose();
						}
						Pen val15 = new Pen(_0BD2.Colors.ItemPressedBorder, 1f);
						try
						{
							_089F.र(graphics, val15, rectangle5);
						}
						finally
						{
							((IDisposable)val15)?.Dispose();
						}
					}
					else if (ກ == eHotTrackingStyle.Image && !rectangle.IsEmpty)
					{
						Rectangle rectangle6 = rectangle;
						rectangle6.Inflate(2, 2);
						if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
						{
							LinearGradientBrush val16 = BarFunctions.ॴ(rectangle6, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val16, rectangle6);
							((Brush)val16).Dispose();
						}
						else
						{
							SolidBrush val17 = new SolidBrush(_0BD2.Colors.ItemHotBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val17, rectangle6);
							}
							finally
							{
								((IDisposable)val17)?.Dispose();
							}
						}
						Pen val18 = new Pen(_0BD2.Colors.ItemHotBorder, 1f);
						try
						{
							_089F.र(graphics, val18, rectangle6);
						}
						finally
						{
							((IDisposable)val18)?.Dispose();
						}
					}
					else
					{
						if (!_0BD2.Colors.ItemCheckedBackground2.IsEmpty && ݟ)
						{
							LinearGradientBrush val19 = BarFunctions.ॴ(rectangle5, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val19, rectangle5);
							((Brush)val19).Dispose();
						}
						else if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
						{
							LinearGradientBrush val20 = BarFunctions.ॴ(rectangle5, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val20, rectangle5);
							((Brush)val20).Dispose();
						}
						else
						{
							SolidBrush val21 = new SolidBrush(_0BD2.Colors.ItemHotBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val21, rectangle5);
							}
							finally
							{
								((IDisposable)val21)?.Dispose();
							}
						}
						Pen val22 = new Pen(_0BD2.Colors.ItemHotBorder, 1f);
						try
						{
							_089F.र(graphics, val22, rectangle5);
						}
						finally
						{
							((IDisposable)val22)?.Dispose();
						}
					}
				}
				if (ᇎ2 != null && _0E70 != eButtonStyle.TextOnlyAlways)
				{
					if (ݠ || (ݟ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog))
					{
						ᇎ2.ᇗ(graphics, rectangle);
					}
					else if (_089F.न >= 16 && EffectiveStyle != eDotNetBarStyle.Office2003)
					{
						float[][] array2 = new float[5][];
						float[] array3 = new float[5];
						array2[0] = array3;
						array3 = new float[5];
						array2[1] = array3;
						array3 = new float[5];
						array2[2] = array3;
						array2[3] = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f };
						array3 = new float[5];
						array2[4] = array3;
						ColorMatrix colorMatrix = new ColorMatrix(array2);
						ImageAttributes val23 = new ImageAttributes();
						val23.ClearColorKey();
						val23.SetColorMatrix(colorMatrix);
						rectangle.Offset(1, 1);
						ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val23);
						rectangle.Offset(-2, -2);
						ᇎ2.ᇗ(graphics, rectangle);
					}
					else
					{
						if (EffectiveStyle == eDotNetBarStyle.OfficeXP)
						{
							rectangle.Offset(-1, -1);
						}
						ᇎ2.ᇗ(graphics, rectangle);
					}
				}
				if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
				{
					Rectangle rectangle7 = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
					rectangle7.Inflate(-1, -1);
					Color color = _0BD2.Colors.ItemCheckedBackground;
					if (flag && !_0BD2.Colors.ItemHotBackground2.IsEmpty)
					{
						LinearGradientBrush val24 = BarFunctions.ॴ(rectangle7, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val24, rectangle7);
						((Brush)val24).Dispose();
					}
					else
					{
						if (flag)
						{
							color = _0BD2.Colors.ItemHotBackground;
						}
						if (!_0BD2.Colors.ItemCheckedBackground2.IsEmpty && !flag)
						{
							LinearGradientBrush val25 = BarFunctions.ॴ(rectangle7, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val25, rectangle7);
							((Brush)val25).Dispose();
						}
						else
						{
							SolidBrush val26 = new SolidBrush(color);
							graphics.FillRectangle((Brush)(object)val26, rectangle7);
							((Brush)val26).Dispose();
						}
					}
				}
			}
			else
			{
				if (ᇎ2 != null && _0E70 != eButtonStyle.TextOnlyAlways)
				{
					if (!ݟ || base.IsOnCustomizeMenu)
					{
						if (ກ != eHotTrackingStyle.Color)
						{
							ᇎ2.ᇗ(graphics, rectangle);
						}
						else
						{
							ColorMatrix colorMatrix2 = new ColorMatrix(new float[5][]
							{
								new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
								new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
								new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
								new float[5] { 0f, 0f, 0f, 1f, 0f },
								new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
							});
							ImageAttributes val27 = new ImageAttributes();
							val27.SetColorMatrix(colorMatrix2);
							ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val27);
						}
					}
					else
					{
						if (((ݟ && isOnMenu) || !Expanded) && !base.IsOnCustomizeDialog)
						{
							Rectangle rectangle8;
							if (isOnMenu)
							{
								rectangle8 = new Rectangle(m_Rect.X + 1, m_Rect.Y, _0E74.Width - 2, m_Rect.Height);
							}
							else if (ກ == eHotTrackingStyle.Image)
							{
								rectangle8 = rectangle;
								rectangle8.Inflate(2, 2);
							}
							else
							{
								rectangle8 = m_Rect;
							}
							if (isOnMenu)
							{
								rectangle8.Inflate(-1, -1);
							}
							Color color2;
							if (flag && ກ != eHotTrackingStyle.None)
							{
								if (ݟ && !_0BD2.Colors.ItemCheckedBackground2.IsEmpty)
								{
									LinearGradientBrush val28 = BarFunctions.ॴ(rectangle8, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
									graphics.FillRectangle((Brush)(object)val28, rectangle8);
									((Brush)val28).Dispose();
									color2 = Color.Empty;
								}
								else if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
								{
									LinearGradientBrush val29 = BarFunctions.ॴ(rectangle8, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
									graphics.FillRectangle((Brush)(object)val29, rectangle8);
									((Brush)val29).Dispose();
									color2 = Color.Empty;
								}
								else
								{
									color2 = ControlPaint.Dark(_0BD2.Colors.ItemHotBackground);
								}
							}
							else if (!_0BD2.Colors.ItemCheckedBackground2.IsEmpty)
							{
								LinearGradientBrush val30 = BarFunctions.ॴ(rectangle8, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val30, rectangle8);
								((Brush)val30).Dispose();
								color2 = Color.Empty;
							}
							else
							{
								color2 = _0BD2.Colors.ItemCheckedBackground;
							}
							if (!color2.IsEmpty)
							{
								SolidBrush val31 = new SolidBrush(color2);
								graphics.FillRectangle((Brush)(object)val31, rectangle8);
								((Brush)val31).Dispose();
							}
						}
						ᇎ2.ᇗ(graphics, rectangle);
					}
				}
				if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
				{
					Rectangle rectangle9 = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
					rectangle9.Inflate(-1, -1);
					if (_0BD2.Colors.ItemCheckedBackground2.IsEmpty)
					{
						Color itemCheckedBackground = _0BD2.Colors.ItemCheckedBackground;
						SolidBrush val32 = new SolidBrush(itemCheckedBackground);
						graphics.FillRectangle((Brush)(object)val32, rectangle9);
						((Brush)val32).Dispose();
					}
					else
					{
						LinearGradientBrush val33 = BarFunctions.ॴ(rectangle9, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val33, rectangle9);
						((Brush)val33).Dispose();
					}
				}
				else if (ݟ && !isOnMenu && ᇎ2 == null)
				{
					Rectangle rect = m_Rect;
					Color color3;
					if (flag && ກ != eHotTrackingStyle.None)
					{
						if (!_0BD2.Colors.ItemCheckedBackground2.IsEmpty)
						{
							LinearGradientBrush val34 = BarFunctions.ॴ(rect, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val34, rect);
							((Brush)val34).Dispose();
							color3 = Color.Empty;
						}
						else if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
						{
							LinearGradientBrush val35 = BarFunctions.ॴ(rect, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val35, rect);
							((Brush)val35).Dispose();
							color3 = Color.Empty;
						}
						else
						{
							color3 = _0BD2.Colors.ItemHotBackground;
						}
					}
					else if (!_0BD2.Colors.ItemCheckedBackground2.IsEmpty)
					{
						LinearGradientBrush val36 = BarFunctions.ॴ(rect, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val36, rect);
						((Brush)val36).Dispose();
						color3 = Color.Empty;
					}
					else
					{
						color3 = _0BD2.Colors.ItemCheckedBackground;
					}
					if (!color3.IsEmpty)
					{
						SolidBrush val37 = new SolidBrush(color3);
						graphics.FillRectangle((Brush)(object)val37, rect);
						((Brush)val37).Dispose();
					}
				}
			}
			if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
			{
				Rectangle ल = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
				ल.Inflate(-1, -1);
				Color itemCheckedBorder = _0BD2.Colors.ItemCheckedBorder;
				Pen val38 = new Pen(itemCheckedBorder, 1f);
				_089F.र(graphics, val38, ल);
				val38.Dispose();
				val38 = new Pen(_0BD2.Colors.ItemCheckedText);
				Point[] array4 = new Point[3];
				array4[0].X = ल.Left + (ल.Width - 5) / 2 - 1;
				array4[0].Y = ल.Top + (ल.Height - 6) / 2 + 3;
				array4[1].X = array4[0].X + 2;
				array4[1].Y = array4[0].Y + 2;
				array4[2].X = array4[1].X + 4;
				array4[2].Y = array4[1].Y - 4;
				graphics.DrawLines(val38, array4);
				array4[0].X++;
				array4[1].X++;
				array4[2].X++;
				graphics.DrawLines(val38, array4);
				val38.Dispose();
			}
			if (ݟ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
			{
				Rectangle rectangle10;
				if (isOnMenu)
				{
					rectangle10 = new Rectangle(m_Rect.X + 1, m_Rect.Y, _0E74.Width - 2, m_Rect.Height);
				}
				else if (ກ == eHotTrackingStyle.Image)
				{
					rectangle10 = rectangle;
					rectangle10.Inflate(2, 2);
				}
				else
				{
					rectangle10 = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
				}
				if (isOnMenu)
				{
					rectangle10.Inflate(-1, -1);
				}
				if (isOnMenu || !Expanded)
				{
					if (ᇎ2 == null || _0E70 == eButtonStyle.TextOnlyAlways)
					{
						if (flag)
						{
							if (ݟ && !_0BD2.Colors.ItemCheckedBackground2.IsEmpty)
							{
								LinearGradientBrush val39 = BarFunctions.ॴ(rectangle10, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val39, rectangle10);
								((Brush)val39).Dispose();
							}
							else if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
							{
								LinearGradientBrush val40 = BarFunctions.ॴ(rectangle10, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val40, rectangle10);
								((Brush)val40).Dispose();
							}
							else
							{
								SolidBrush val41 = new SolidBrush(_0BD2.Colors.ItemHotBackground);
								try
								{
									graphics.FillRectangle((Brush)(object)val41, rectangle10);
								}
								finally
								{
									((IDisposable)val41)?.Dispose();
								}
							}
						}
						else if (_0BD2.Colors.ItemCheckedBackground2.IsEmpty)
						{
							SolidBrush val42 = new SolidBrush(_0BD2.Colors.ItemCheckedBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val42, rectangle10);
							}
							finally
							{
								((IDisposable)val42)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val43 = BarFunctions.ॴ(rectangle10, _0BD2.Colors.ItemCheckedBackground, _0BD2.Colors.ItemCheckedBackground2, _0BD2.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val43, rectangle10);
							((Brush)val43).Dispose();
						}
					}
					Color itemCheckedBorder2 = _0BD2.Colors.ItemCheckedBorder;
					Pen val44 = new Pen(itemCheckedBorder2, 1f);
					_089F.र(graphics, val44, rectangle10);
					val44.Dispose();
				}
				if ((ᇎ2 == null || _0E70 == eButtonStyle.TextOnlyAlways) && isOnMenu)
				{
					Pen val45 = new Pen(_0BD2.Colors.ItemCheckedText);
					Point[] array5 = new Point[3];
					array5[0].X = rectangle10.Left + (rectangle10.Width - 5) / 2 - 1;
					array5[0].Y = rectangle10.Top + (rectangle10.Height - 6) / 2 + 3;
					array5[1].X = array5[0].X + 2;
					array5[1].Y = array5[0].Y + 2;
					array5[2].X = array5[1].X + 4;
					array5[2].Y = array5[1].Y - 4;
					graphics.DrawLines(val45, array5);
					array5[0].X++;
					array5[1].X++;
					array5[2].X++;
					graphics.DrawLines(val45, array5);
					val45.Dispose();
				}
			}
		}
		else
		{
			if (isOnMenu && flag && ກ == eHotTrackingStyle.Default)
			{
				Rectangle ल2 = new Rectangle(rectangle2.Left + 1, rectangle2.Top, rectangle2.Width - 2, rectangle2.Height);
				Pen val46 = new Pen(_0BD2.Colors.ItemHotBorder, 1f);
				try
				{
					_089F.र(graphics, val46, ल2);
				}
				finally
				{
					((IDisposable)val46)?.Dispose();
				}
			}
			if (ݟ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
			{
				Rectangle ल3;
				if (isOnMenu)
				{
					ल3 = new Rectangle(m_Rect.X + 1, m_Rect.Y, _0E74.Width - 2, m_Rect.Height);
				}
				else if (ກ == eHotTrackingStyle.Image)
				{
					ल3 = rectangle;
					ल3.Inflate(2, 2);
				}
				else
				{
					ल3 = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
				}
				if (isOnMenu)
				{
					ल3.Inflate(-1, -1);
				}
				if (isOnMenu || !Expanded)
				{
					Color itemDisabledText = _0BD2.Colors.ItemDisabledText;
					Pen val47 = new Pen(itemDisabledText, 1f);
					_089F.र(graphics, val47, ल3);
					val47.Dispose();
				}
				if ((ᇎ2 == null || _0E70 == eButtonStyle.TextOnlyAlways) && isOnMenu)
				{
					Pen val48 = new Pen(_0BD2.Colors.ItemDisabledText);
					Point[] array6 = new Point[3];
					array6[0].X = ल3.Left + (ल3.Width - 5) / 2 - 1;
					array6[0].Y = ल3.Top + (ल3.Height - 6) / 2 + 3;
					array6[1].X = array6[0].X + 2;
					array6[1].Y = array6[0].Y + 2;
					array6[2].X = array6[1].X + 4;
					array6[2].Y = array6[1].Y - 4;
					graphics.DrawLines(val48, array6);
					array6[0].X++;
					array6[1].X++;
					array6[2].X++;
					graphics.DrawLines(val48, array6);
					val48.Dispose();
				}
			}
			empty = _0BD2.Colors.ItemDisabledText;
			if (ᇎ2 != null && _0E70 != eButtonStyle.TextOnlyAlways)
			{
				ᇎ2.ᇗ(graphics, rectangle);
			}
		}
		if (isOnMenu || _0E70 != 0 || ᇎ2 == null || (!isOnMenu && (_0E71 == eImagePosition.Top || _0E71 == eImagePosition.Bottom)))
		{
			if (isOnMenu)
			{
				rectangle = new Rectangle(_0E75.X, _0E75.Y, rectangle2.Width - _0E74.Right - 26, _0E75.Height);
			}
			else
			{
				rectangle = _0E75;
				if (_0E71 == eImagePosition.Top || _0E71 == eImagePosition.Bottom)
				{
					if (m_Orientation == eOrientation.Vertical)
					{
						rectangle = new Rectangle(_0E75.X, _0E75.Y, _0E75.Width, _0E75.Height);
					}
					else
					{
						rectangle = new Rectangle(_0E75.X, _0E75.Y, rectangle2.Width, _0E75.Height);
						if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
						{
							rectangle.Width -= 10;
						}
					}
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
				else if (isOnMenuBar && ᇎ2 == null)
				{
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
				if (ݠ && ກ != eHotTrackingStyle.Image)
				{
					empty = ((!_0D02.IsEmpty) ? ControlPaint.Light(_0D02) : _0BD2.Colors.ItemPressedText);
				}
			}
			rectangle.Offset(rectangle2.Left, rectangle2.Top);
			if (m_Orientation == eOrientation.Vertical && !isOnMenu)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, m_Text, val, empty, new Rectangle(rectangle.Top, -rectangle.Right, rectangle.Height, rectangle.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (rectangle.Right > m_Rect.Right)
				{
					rectangle.Width = m_Rect.Right - rectangle.Left;
				}
				TextDrawing.DrawString(graphics, m_Text, val, empty, rectangle, eTextFormat2);
				if (!DesignMode && Focused && !isOnMenu && !isOnMenuBar)
				{
					Rectangle rectangle11 = rectangle;
					ControlPaint.DrawFocusRectangle(graphics, rectangle11);
				}
			}
		}
		if (_0E5F != "" && isOnMenu && !base.IsOnCustomizeDialog)
		{
			eTextFormat2 |= eTextFormat.HidePrefix | eTextFormat.Right;
			TextDrawing.DrawString(graphics, _0E5F, val, empty, rectangle, eTextFormat2);
		}
		if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
		{
			if (isOnMenu)
			{
				Point[] array7 = new Point[3];
				array7[0].X = rectangle2.Left + rectangle2.Width - 12;
				array7[0].Y = rectangle2.Top + (rectangle2.Height - 8) / 2;
				array7[1].X = array7[0].X;
				array7[1].Y = array7[0].Y + 8;
				array7[2].X = array7[0].X + 4;
				array7[2].Y = array7[0].Y + 4;
				SolidBrush val49 = new SolidBrush(empty);
				try
				{
					graphics.FillPolygon((Brush)(object)val49, array7);
				}
				finally
				{
					((IDisposable)val49)?.Dispose();
				}
			}
			else if (!_0E76.IsEmpty)
			{
				if (GetEnabled(_0BD2.ContainerControl) && (flag || ݟ) && !m_Expanded && ກ != eHotTrackingStyle.None && ກ != eHotTrackingStyle.Image)
				{
					if (m_Orientation == eOrientation.Horizontal)
					{
						Pen val50 = new Pen(_0BD2.Colors.ItemHotBorder);
						try
						{
							graphics.DrawLine(val50, rectangle2.Left + _0E76.Left, rectangle2.Top, rectangle2.Left + _0E76.Left, rectangle2.Bottom - 1);
						}
						finally
						{
							((IDisposable)val50)?.Dispose();
						}
					}
					else
					{
						Pen val51 = new Pen(_0BD2.Colors.ItemHotBorder);
						try
						{
							graphics.DrawLine(val51, rectangle2.Left, rectangle2.Top + _0E76.Top, rectangle2.Right - 2, rectangle2.Top + _0E76.Top);
						}
						finally
						{
							((IDisposable)val51)?.Dispose();
						}
					}
				}
				Point[] array8 = new Point[3];
				if (PopupSide == ePopupSide.Default)
				{
					if (m_Orientation == eOrientation.Horizontal)
					{
						array8[0].X = rectangle2.Left + _0E76.Left + (_0E76.Width - 5) / 2;
						array8[0].Y = rectangle2.Top + (_0E76.Height - 3) / 2 + 1;
						array8[1].X = array8[0].X + 5;
						array8[1].Y = array8[0].Y;
						array8[2].X = array8[0].X + 2;
						array8[2].Y = array8[0].Y + 3;
					}
					else
					{
						array8[0].X = rectangle2.Left + (_0E76.Width - 3) / 2 + 1;
						array8[0].Y = rectangle2.Top + _0E76.Top + (_0E76.Height - 5) / 2;
						array8[1].X = array8[0].X;
						array8[1].Y = array8[0].Y + 6;
						array8[2].X = array8[0].X - 3;
						array8[2].Y = array8[0].Y + 3;
					}
				}
				else
				{
					switch (PopupSide)
					{
					case ePopupSide.Left:
						array8[0].X = rectangle2.Left + _0E76.Left + _0E76.Width / 2;
						array8[0].Y = rectangle2.Top + _0E76.Height / 2 - 3;
						array8[1].X = array8[0].X;
						array8[1].Y = array8[0].Y + 6;
						array8[2].X = array8[0].X + 3;
						array8[2].Y = array8[0].Y + 3;
						break;
					case ePopupSide.Right:
						array8[0].X = rectangle2.Left + _0E76.Left + _0E76.Width / 2 + 3;
						array8[0].Y = rectangle2.Top + _0E76.Height / 2 - 3;
						array8[1].X = array8[0].X;
						array8[1].Y = array8[0].Y + 6;
						array8[2].X = array8[0].X - 3;
						array8[2].Y = array8[0].Y + 3;
						break;
					case ePopupSide.Top:
						array8[0].X = rectangle2.Left + _0E76.Left + (_0E76.Width - 5) / 2;
						array8[0].Y = rectangle2.Top + (_0E76.Height - 3) / 2 + 4;
						array8[1].X = array8[0].X + 6;
						array8[1].Y = array8[0].Y;
						array8[2].X = array8[0].X + 3;
						array8[2].Y = array8[0].Y - 4;
						break;
					case ePopupSide.Bottom:
						array8[0].X = rectangle2.Left + _0E76.Left + (_0E76.Width - 5) / 2 + 1;
						array8[0].Y = rectangle2.Top + (_0E76.Height - 3) / 2 + 1;
						array8[1].X = array8[0].X + 5;
						array8[1].Y = array8[0].Y;
						array8[2].X = array8[0].X + 2;
						array8[2].Y = array8[0].Y + 3;
						break;
					}
				}
				if (GetEnabled(_0BD2.ContainerControl))
				{
					SolidBrush val52 = new SolidBrush(_0BD2.Colors.ItemText);
					try
					{
						graphics.FillPolygon((Brush)(object)val52, array8);
					}
					finally
					{
						((IDisposable)val52)?.Dispose();
					}
				}
				else
				{
					SolidBrush val53 = new SolidBrush(_0BD2.Colors.ItemDisabledText);
					try
					{
						graphics.FillPolygon((Brush)(object)val53, array8);
					}
					finally
					{
						((IDisposable)val53)?.Dispose();
					}
				}
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle ल4 = rectangle2;
			ल4.Inflate(-1, -1);
			_1A6B._1A6C(graphics, ल4, _0BD2.Colors.ItemDesignTimeBorder);
		}
		ᇎ2?.Dispose();
	}

	private void _0DF4(ItemPaintArgs _0BD2)
	{
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		_0A7B obj = _0BD2._0A7B;
		_0F6F _070B = _0F6F._0F70;
		_0F76 obj2 = _0F76.ཊ;
		eTextFormat eTextFormat2 = _0BD2.ButtonStringFormat;
		Color controlText = SystemColors.ControlText;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rect = m_Rect;
		Font val = null;
		ᇎ ᇎ2 = _0EC5();
		val = ((ޓ == null) ? _໘(_0BD2, _໙: false) : ޓ);
		bool flag = (SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems && !_0E76.IsEmpty;
		if (flag)
		{
			_070B = _0F6F._0F72;
		}
		if (ᇎ2 != null)
		{
			rectangle = ((_0E71 != eImagePosition.Top && _0E71 != eImagePosition.Bottom) ? new Rectangle(_0E74.X, _0E74.Y, _0E74.Width, _0E74.Height) : new Rectangle(_0E74.X, _0E74.Y, rect.Width, _0E74.Height));
			rectangle.Offset(rect.Left, rect.Top);
			rectangle.Offset((rectangle.Width - ImageSize.Width) / 2, (rectangle.Height - ImageSize.Height) / 2);
			rectangle.Width = ImageSize.Width;
			rectangle.Height = ImageSize.Height;
		}
		if (!GetEnabled(_0BD2.ContainerControl))
		{
			obj2 = _0F76.ड़;
			controlText = _0BD2.Colors.ItemDisabledText;
		}
		else if (ݠ)
		{
			obj2 = _0F76._0F78;
			controlText = _0BD2.Colors.ItemPressedText;
		}
		else if (ݡ && ݟ)
		{
			obj2 = _0F76._0F7A;
			controlText = _0BD2.Colors.ItemHotText;
		}
		else if (ݡ || m_Expanded)
		{
			obj2 = _0F76._0F77;
			controlText = _0BD2.Colors.ItemHotText;
		}
		else if (ݟ)
		{
			obj2 = _0F76._0F79;
			controlText = _0BD2.Colors.ItemCheckedText;
		}
		else
		{
			controlText = _0BD2.Colors.ItemText;
		}
		Rectangle ल = m_Rect;
		if (ກ == eHotTrackingStyle.Image && ᇎ2 != null)
		{
			ल = rectangle;
			ल.Inflate(3, 3);
		}
		else if (flag)
		{
			ल.Width -= _0E76.Width;
		}
		if (ກ != eHotTrackingStyle.None)
		{
			obj._0EE5(graphics, _070B, obj2, ल);
		}
		if (ᇎ2 != null && _0E70 != eButtonStyle.TextOnlyAlways)
		{
			if (obj2 == _0F76.ཊ && ກ == eHotTrackingStyle.Color)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
					new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
					new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
				});
				ImageAttributes val2 = new ImageAttributes();
				val2.SetColorMatrix(colorMatrix);
				ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val2);
			}
			else if (obj2 == _0F76.ཊ && !ᇎ2.ᇏ)
			{
				ImageAttributes val3 = new ImageAttributes();
				val3.SetGamma(0.7f, (ColorAdjustType)1);
				ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val3);
			}
			else
			{
				ᇎ2.ᇗ(graphics, rectangle);
			}
		}
		if (_0E70 == eButtonStyle.ImageAndText || _0E70 == eButtonStyle.TextOnlyAlways || ᇎ2 == null)
		{
			Rectangle rectangle2 = _0E75;
			if (_0E71 == eImagePosition.Top || _0E71 == eImagePosition.Bottom)
			{
				if (m_Orientation == eOrientation.Vertical)
				{
					rectangle2 = new Rectangle(_0E75.X, _0E75.Y, _0E75.Width, _0E75.Height);
				}
				else
				{
					rectangle2 = new Rectangle(_0E75.X, _0E75.Y, rect.Width, _0E75.Height);
					if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
					{
						rectangle2.Width -= 10;
					}
				}
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			rectangle2.Offset(rect.Left, rect.Top);
			if (m_Orientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, m_Text, val, controlText, new Rectangle(rectangle2.Top, -rectangle2.Right, rectangle2.Height, rectangle2.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (rectangle2.Right > m_Rect.Right)
				{
					rectangle2.Width = m_Rect.Right - rectangle2.Left;
				}
				TextDrawing.DrawString(graphics, m_Text, val, controlText, rectangle2, eTextFormat2);
				if (!DesignMode && Focused && !_0BD2.IsOnMenu && !_0BD2.IsOnMenuBar)
				{
					Rectangle rectangle3 = rectangle2;
					ControlPaint.DrawFocusRectangle(graphics, rectangle3);
				}
			}
		}
		if (flag)
		{
			_070B = _0F6F._0F73;
			obj2 = (GetEnabled(_0BD2.ContainerControl) ? _0F76.ཊ : _0F76.ड़);
			if (ກ != eHotTrackingStyle.None && ກ != eHotTrackingStyle.Image && GetEnabled(_0BD2.ContainerControl))
			{
				if (m_Expanded || ݠ)
				{
					obj2 = _0F76._0F78;
				}
				else if (ݡ && ݟ)
				{
					obj2 = _0F76._0F7A;
				}
				else if (ݟ)
				{
					obj2 = _0F76._0F79;
				}
				else if (ݡ)
				{
					obj2 = _0F76._0F77;
				}
			}
			if (m_Orientation == eOrientation.Horizontal)
			{
				Rectangle ल2 = _0E76;
				ल2.Offset(rect.X, rect.Y);
				obj._0EE5(graphics, _070B, obj2, ल2);
			}
			else
			{
				Rectangle ल3 = _0E76;
				ल3.Offset(rect.X, rect.Y);
				obj._0EE5(graphics, _070B, obj2, ल3);
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle ल4 = rect;
			ल4.Inflate(-1, -1);
			_1A6B._1A6C(graphics, ल4, _0BD2.Colors.ItemDesignTimeBorder);
		}
		ᇎ2?.Dispose();
	}

	private void _0DF5(ItemPaintArgs _0BD2)
	{
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Expected O, but got Unknown
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Expected O, but got Unknown
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ece: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rect = m_Rect;
		Rectangle empty = Rectangle.Empty;
		Color color = SystemColors.ControlText;
		Color ॲ = SystemColors.Control;
		if (m_Parent is GenericItemContainer && !((GenericItemContainer)m_Parent).BackColor.IsEmpty)
		{
			ॲ = ((GenericItemContainer)m_Parent).BackColor;
		}
		else if (m_Parent is SideBarPanelItem && !((SideBarPanelItem)m_Parent).BackgroundStyle.BackColor1.IsEmpty)
		{
			ॲ = ((SideBarPanelItem)m_Parent).BackgroundStyle.BackColor1.GetCompositeColor();
		}
		if (ݡ && !_0E79.IsEmpty)
		{
			color = _0E79;
		}
		else if (!_0D02.IsEmpty)
		{
			color = _0D02;
		}
		Font val = null;
		bool isOnMenu = _0BD2.IsOnMenu;
		bool flag = _0BD2.ContainerControl is ButtonX;
		eTextFormat eTextFormat2 = _0BD2.ButtonStringFormat;
		ᇎ ᇎ2 = _0EC5();
		val = ((ޓ == null) ? _໘(_0BD2, _໙: false) : ޓ);
		if (ᇎ2 != null)
		{
			rectangle = ((isOnMenu || (_0E71 != eImagePosition.Top && _0E71 != eImagePosition.Bottom)) ? _0E74 : new Rectangle(_0E74.X, _0E74.Y + 1, rect.Width, _0E74.Height));
			rectangle.Offset(rect.Left, rect.Top);
			rectangle.Offset((rectangle.Width - ImageSize.Width) / 2, (rectangle.Height - ImageSize.Height) / 2);
			rectangle.Width = ImageSize.Width;
			rectangle.Height = ImageSize.Height;
		}
		if (isOnMenu && !DesignMode && MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
		{
			graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.RecentlyUsedOfficeBackColor()), m_Rect);
		}
		else if (flag)
		{
			ButtonState val2 = (ButtonState)0;
			if (ݠ)
			{
				val2 = (ButtonState)512;
			}
			else if (ݟ || m_Expanded)
			{
				val2 = (ButtonState)1024;
			}
			ControlPaint.DrawButton(graphics, rect, val2);
		}
		if (GetEnabled(_0BD2.ContainerControl) || DesignMode)
		{
			if (m_Expanded && !isOnMenu)
			{
				graphics.FillRectangle(SystemBrushes.Control, rect);
				BarFunctions._८(graphics, rect, (Border3DStyle)2, (Border3DSide)2063, ॲ);
			}
			if ((ݡ && ກ != eHotTrackingStyle.None) || m_Expanded || (ݟ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog))
			{
				if (isOnMenu || base.IsOnCustomizeDialog)
				{
					if (((ݡ && ກ != eHotTrackingStyle.None) || m_Expanded) && (!ݡ || !DesignMode || base.IsOnCustomizeDialog))
					{
						graphics.FillRectangle(SystemBrushes.Highlight, rect);
					}
				}
				else if (ݠ || (ݟ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog) || (Expanded && (!ShowSubItems || IsOnMenuBar)))
				{
					if (_0E76.IsEmpty)
					{
						if (!flag)
						{
							BarFunctions._८(graphics, rect, (Border3DStyle)2, (Border3DSide)2063, ॲ);
							if (ݟ && !ݡ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
							{
								Rectangle rectangle2 = rect;
								rectangle2.Inflate(-1, -1);
								graphics.FillRectangle(ColorFunctions.GetPushedBrush(this), rectangle2);
							}
						}
					}
					else if (!flag)
					{
						Rectangle rectangle3 = ((m_Orientation != 0) ? new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - _0E76.Height) : new Rectangle(rect.X, rect.Y, rect.Width - _0E76.Width, rect.Height));
						BarFunctions._८(graphics, rectangle3, (Border3DStyle)2, (Border3DSide)2063, ॲ);
						if (!ݡ)
						{
							rectangle3.Inflate(-1, -1);
							graphics.FillRectangle(ColorFunctions.GetPushedBrush(this), rectangle3);
						}
					}
				}
				else if (!DesignMode)
				{
					if (_0E76.IsEmpty)
					{
						if (!flag)
						{
							BarFunctions._८(graphics, rect, (Border3DStyle)4, (Border3DSide)2063, ॲ);
						}
					}
					else
					{
						Rectangle ल = ((m_Orientation != 0) ? new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - _0E76.Height) : new Rectangle(rect.X, rect.Y, rect.Width - _0E76.Width, rect.Height));
						BarFunctions._८(graphics, ल, (Border3DStyle)4, (Border3DSide)2063, ॲ);
					}
				}
				if (ᇎ2 != null && _0E70 != eButtonStyle.TextOnlyAlways)
				{
					empty = new Rectangle(_0E74.X, _0E74.Y, _0E74.Width, rect.Height);
					empty.Offset(rect.Left, rect.Top);
					if (isOnMenu)
					{
						if (ݟ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
						{
							BarFunctions._८(graphics, empty, (Border3DStyle)2, (Border3DSide)2063, ॲ);
							if (!ݡ)
							{
								empty.Inflate(-1, -1);
								graphics.FillRectangle(ColorFunctions.GetPushedBrush(this), empty);
							}
						}
						else
						{
							BarFunctions._८(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, ॲ);
						}
					}
					else if (ݠ)
					{
						rectangle.Offset(1, 1);
					}
					ᇎ2.ᇗ(graphics, rectangle);
				}
				else if ((ᇎ2 == null || _0E70 == eButtonStyle.TextOnlyAlways) && ݟ && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && isOnMenu)
				{
					empty = new Rectangle(_0E74.X, _0E74.Y, _0E74.Width, rect.Height);
					empty.Offset(rect.Left, rect.Top);
					_0ECB(graphics, empty);
				}
			}
			else if (ᇎ2 != null && _0E70 != eButtonStyle.TextOnlyAlways)
			{
				if (ກ != eHotTrackingStyle.Color)
				{
					ᇎ2.ᇗ(graphics, rectangle);
				}
				else if (ກ == eHotTrackingStyle.Color)
				{
					ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
					{
						new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
						new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
						new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
						new float[5] { 0f, 0f, 0f, 1f, 0f },
						new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
					});
					ImageAttributes val3 = new ImageAttributes();
					val3.SetColorMatrix(colorMatrix);
					ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val3);
				}
			}
			if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
			{
				Rectangle ल2 = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
				_0ECB(graphics, ल2);
			}
		}
		else
		{
			color = SystemColors.ControlDark;
			if (ᇎ2 != null && _0E70 != eButtonStyle.TextOnlyAlways)
			{
				ᇎ2.ᇗ(graphics, rectangle);
			}
		}
		if (isOnMenu || _0E70 != 0 || ᇎ2 == null || (!isOnMenu && (_0E71 == eImagePosition.Top || _0E71 == eImagePosition.Bottom)))
		{
			if (isOnMenu)
			{
				rectangle = new Rectangle(_0E75.X, _0E75.Y, rect.Width - _0E74.Right - 24, _0E75.Height);
			}
			else
			{
				rectangle = _0E75;
				if (_0E71 == eImagePosition.Top || _0E71 == eImagePosition.Bottom)
				{
					if (m_Orientation == eOrientation.Horizontal)
					{
						rectangle = new Rectangle(_0E75.X, _0E75.Y, rect.Width, _0E75.Height);
					}
					if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && m_Orientation == eOrientation.Horizontal && ShowSubItems)
					{
						rectangle.Width -= 12;
					}
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
			}
			if (((ݡ && ກ != eHotTrackingStyle.None) || m_Expanded) && isOnMenu && !DesignMode)
			{
				color = SystemColors.HighlightText;
			}
			rectangle.Offset(rect.Left, rect.Top);
			if (!isOnMenu && (ݠ || (Expanded && IsOnMenuBar)))
			{
				rectangle.Offset(1, 1);
			}
			if (flag)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
				rectangle.Height--;
			}
			if (GetEnabled(_0BD2.ContainerControl) || DesignMode)
			{
				if (m_Orientation == eOrientation.Vertical && !isOnMenu)
				{
					graphics.RotateTransform(90f);
					TextDrawing.DrawStringLegacy(graphics, m_Text, val, color, new Rectangle(rectangle.Top, -rectangle.Right, rectangle.Height, rectangle.Width), eTextFormat2);
					graphics.ResetTransform();
				}
				else
				{
					TextDrawing.DrawString(graphics, m_Text, val, color, rectangle, eTextFormat2);
					if (!DesignMode && Focused && !isOnMenu && !_0BD2.IsOnMenuBar)
					{
						Rectangle rectangle4 = rectangle;
						ControlPaint.DrawFocusRectangle(graphics, rectangle4);
					}
				}
			}
			else if (m_Orientation == eOrientation.Vertical && !isOnMenu)
			{
				graphics.RotateTransform(90f);
				ControlPaint.DrawStringDisabled(graphics, m_Text, val, SystemColors.Control, (RectangleF)new Rectangle(rectangle.Top, -rectangle.Right, rectangle.Height, rectangle.Width), TextDrawing.GetStringFormat(eTextFormat2));
				graphics.ResetTransform();
			}
			else
			{
				ControlPaint.DrawStringDisabled(graphics, m_Text, val, SystemColors.Control, (RectangleF)rectangle, TextDrawing.GetStringFormat(eTextFormat2));
			}
		}
		if (_0E5F != "" && isOnMenu)
		{
			eTextFormat2 |= eTextFormat.HidePrefix | eTextFormat.Right;
			if (GetEnabled(_0BD2.ContainerControl) || DesignMode)
			{
				TextDrawing.DrawString(graphics, _0E5F, val, color, rectangle, eTextFormat2);
			}
			else
			{
				ControlPaint.DrawStringDisabled(graphics, _0E5F, val, SystemColors.Control, (RectangleF)rectangle, TextDrawing.GetStringFormat(eTextFormat2));
			}
		}
		if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
		{
			if (isOnMenu)
			{
				Point[] array = new Point[3];
				array[0].X = rect.Left + rect.Width - 12;
				array[0].Y = rect.Top + (rect.Height - 8) / 2;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + 8;
				array[2].X = array[0].X + 4;
				array[2].Y = array[0].Y + 4;
				SolidBrush val4 = new SolidBrush(color);
				try
				{
					graphics.FillPolygon((Brush)(object)val4, array);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else if (!_0E76.IsEmpty)
			{
				if (ݡ && !m_Expanded && ກ != eHotTrackingStyle.None)
				{
					Rectangle ल3 = new Rectangle(_0E76.X, _0E76.Y, _0E76.Width, _0E76.Height);
					ल3.Offset(rect.X, rect.Y);
					BarFunctions._८(graphics, ल3, (Border3DStyle)4, (Border3DSide)2063, ॲ);
				}
				else if (m_Expanded)
				{
					Rectangle ल4 = new Rectangle(_0E76.X, _0E76.Y, _0E76.Width, _0E76.Height);
					ल4.Offset(rect.X, rect.Y);
					BarFunctions._८(graphics, ल4, (Border3DStyle)2, (Border3DSide)2063, ॲ);
				}
				Point[] array2 = new Point[3];
				if (m_Orientation == eOrientation.Horizontal)
				{
					array2[0].X = rect.Left + _0E76.Left + (_0E76.Width - 5) / 2;
					array2[0].Y = rect.Top + (_0E76.Height - 3) / 2 + 1;
					array2[1].X = array2[0].X + 5;
					array2[1].Y = array2[0].Y;
					array2[2].X = array2[0].X + 2;
					array2[2].Y = array2[0].Y + 3;
				}
				else
				{
					array2[0].X = rect.Left + (_0E76.Width - 3) / 2 + 1;
					array2[0].Y = rect.Top + _0E76.Top + (_0E76.Height - 5) / 2;
					array2[1].X = array2[0].X;
					array2[1].Y = array2[0].Y + 6;
					array2[2].X = array2[0].X - 3;
					array2[2].Y = array2[0].Y + 3;
				}
				graphics.FillPolygon(SystemBrushes.ControlText, array2);
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle rectangle5 = rect;
			rectangle5.Inflate(-1, -1);
			graphics.DrawRectangle(new Pen(SystemColors.ControlText, 2f), rectangle5);
		}
		ᇎ2?.Dispose();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Image GetItemImage()
	{
		return _0EC5()?.ᇑ;
	}

	internal ᇎ _0EC5(Color ໆ)
	{
		if (!GetEnabled() && !base.IsOnCustomizeDialog)
		{
			return _0EC5(_1937.ड़, ໆ);
		}
		if (ݠ || Checked || (Expanded && ກ == eHotTrackingStyle.Image))
		{
			return _0EC5(_1937._0F78, ໆ);
		}
		if (ݡ && !_0E73 && (!StyleManager.IsMetro(EffectiveStyle) || !Expanded))
		{
			return _0EC5(_1937._1938, ໆ);
		}
		return _0EC5(_1937.ሬ, ໆ);
	}

	internal ᇎ _0EC5()
	{
		return _0EC5(Color.Black);
	}

	internal ᇎ _0EC5(_1937 య, Color ໆ)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		Image val = null;
		if (య == _1937.ड़ && (_0E6A != null || _0E6C >= 0 || _0E6D != null || _0E66 != null || դ >= 0 || ຈ != null))
		{
			if (_0E6A != null)
			{
				return new ᇎ(_0E6A, dispose: false, ຆ);
			}
			if (_0E6D != null)
			{
				return new ᇎ(_0E6D, dispose: false, ຆ);
			}
			if (_0E6C >= 0)
			{
				_0EBC();
				val = _0EC9(_0E6C);
				if (val != null)
				{
					return new ᇎ(val, val != ຂ, ຆ);
				}
				return null;
			}
			_໖();
			if (_0E6A != null)
			{
				return new ᇎ(_0E6A, dispose: false, ຆ);
			}
			if (_0E6D != null)
			{
				return new ᇎ(_0E6D, dispose: false, ຆ);
			}
			return null;
		}
		if (ຈ != null)
		{
			Size size = _0E5A;
			Icon val2 = null;
			try
			{
				val2 = new Icon(ຈ, size);
			}
			catch
			{
				val2 = null;
			}
			if (val2 == null)
			{
				return new ᇎ(ຈ, dispose: false, ຆ);
			}
			return new ᇎ(val2, dispose: true, ຆ);
		}
		if (య == _1937._1938 && (_0E68 != null || _0E69 >= 0))
		{
			if (_0E68 != null)
			{
				return new ᇎ(_0E68, dispose: false, ຆ);
			}
			if (_0E69 >= 0)
			{
				_0EBC();
				val = _0EC9(_0E69);
				if (val != null)
				{
					return new ᇎ(val, val != ຂ, ຆ);
				}
				return null;
			}
		}
		if (య == _1937._0F78 && (_0E6E != null || _0E6F >= 0))
		{
			if (_0E6E != null)
			{
				return new ᇎ(_0E6E, dispose: false, ຆ);
			}
			if (_0E6F >= 0)
			{
				_0EBC();
				val = _0EC9(_0E6F);
				if (val != null)
				{
					return new ᇎ(val, val != ຂ, ຆ);
				}
				return null;
			}
		}
		if (_0E67 != null && (_๖ || (ຆ.Width == _0E67.Width && ຆ.Height == _0E67.Height)))
		{
			return new ᇎ(_0E67, dispose: false);
		}
		if (_0E66 != null)
		{
			if (_ຩ != null && ໆ == Color.White)
			{
				return new ᇎ(_ຩ, dispose: false, ຆ);
			}
			return new ᇎ(_0E66, dispose: false, ຆ);
		}
		if (դ >= 0)
		{
			if (_0E6C >= 0 || _0E69 >= 0 || _0E6F >= 0)
			{
				_0EBC();
			}
			val = _0EC9(դ);
			if (val != null)
			{
				return new ᇎ(val, val != ຂ, ຆ);
			}
		}
		return null;
	}

	private eBarImageSize _0EC7(IBarImageSize _0EC8)
	{
		eBarImageSize result = eBarImageSize.Default;
		if (_0EC8 != null)
		{
			result = _0EC8.ImageSize;
		}
		if (_ສ != eButtonImageListSelection.NotSet)
		{
			result = (eBarImageSize)_ສ;
		}
		return result;
	}

	private Image _0EC9(int _0ECA)
	{
		if (_0ECA >= 0)
		{
			IOwner owner = null;
			IBarImageSize barImageSize = null;
			if (__0E83 != null)
			{
				owner = __0E83.Owner;
				barImageSize = __0E83.ContainerControl as IBarImageSize;
			}
			if (owner == null)
			{
				owner = GetOwner() as IOwner;
			}
			if (barImageSize == null)
			{
				barImageSize = ContainerControl as IBarImageSize;
			}
			if (owner != null)
			{
				try
				{
					eBarImageSize eBarImageSize2 = _0EC7(barImageSize);
					if (eBarImageSize2 != 0)
					{
						if (eBarImageSize2 == eBarImageSize.Medium && owner.ImagesMedium != null && !_๖)
						{
							return owner.ImagesMedium.Images[_0ECA];
						}
						if (eBarImageSize2 == eBarImageSize.Large && owner.ImagesLarge != null && !_๖)
						{
							return owner.ImagesLarge.Images[_0ECA];
						}
						if (owner.Images != null)
						{
							if (_0ECA == դ)
							{
								if (ຂ == null)
								{
									ຂ = owner.Images.Images[_0ECA];
								}
								return ຂ;
							}
							return owner.Images.Images[_0ECA];
						}
					}
					else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != 0)
					{
						eBarImageSize itemImageSize = ((SideBarPanelItem)m_Parent).ItemImageSize;
						if (itemImageSize == eBarImageSize.Medium && owner.ImagesMedium != null)
						{
							return owner.ImagesMedium.Images[_0ECA];
						}
						if (itemImageSize == eBarImageSize.Large && owner.ImagesLarge != null)
						{
							return owner.ImagesLarge.Images[_0ECA];
						}
						if (owner.Images != null)
						{
							return owner.Images.Images[_0ECA];
						}
					}
					else if (owner.Images != null)
					{
						if (_0ECA == դ)
						{
							if (ຂ == null)
							{
								ຂ = owner.Images.Images[_0ECA];
							}
							return ຂ;
						}
						return owner.Images.Images[_0ECA];
					}
				}
				catch (Exception)
				{
					return null;
				}
			}
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageFixedSize()
	{
		ImageFixedSize = default(Size);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageFixedSize()
	{
		return !ຆ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedSize()
	{
		return !ຎ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedSize()
	{
		TypeDescriptor.GetProperties(this)["FixedSize"].SetValue(this, Size.Empty);
	}

	public override void RecalcSize()
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		bool flag = false;
		int pulseBeats = ດ;
		if (_0EB1)
		{
			_0ECF();
			flag = ຓ;
			ຓ = false;
		}
		if (Dpi.Factor.Width > 1f)
		{
			ᇎ ᇎ2 = _0EC5();
			if (ᇎ2 != null && ImageSize.Width != ᇎ2.ܐ)
			{
				OnImageChanged();
			}
		}
		if (EffectiveStyle == eDotNetBarStyle.Office2000 && !base.IsThemed)
		{
			_௮();
		}
		else
		{
			_௬();
		}
		base.RecalcSize();
		if (flag)
		{
			InvokeDelayed((MethodInvoker)delegate
			{
				Pulse(pulseBeats);
			}, 500);
		}
	}

	private void _௬()
	{
		_20B1._20B7(this);
	}

	private void _௮()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = Graphics.FromHwnd(val.Handle);
		val2.PageUnit = (GraphicsUnit)2;
		_0E75 = Rectangle.Empty;
		_0E74 = Rectangle.Empty;
		m_Rect = Rectangle.Empty;
		_0E76 = Rectangle.Empty;
		bool flag = false;
		if (_0EC5(_1937.ሬ, Color.Black) != null)
		{
			flag = true;
		}
		Size size = ((m_Parent == null) ? ImageSize : ((!(m_Parent is ImageItem imageItem)) ? ImageSize : ((flag && !base.IsOnMenu) ? ((Orientation != 0) ? new Size(imageItem.SubItemsImageSize.Width, ImageSize.Height) : new Size(ImageSize.Width, imageItem.SubItemsImageSize.Height)) : new Size(imageItem.SubItemsImageSize.Width, imageItem.SubItemsImageSize.Height))));
		Font val3 = null;
		val3 = ((ޓ == null) ? _໘(null, _໙: true) : ޓ);
		eTextFormat format = ಲ();
		Size size2 = ((m_Orientation != eOrientation.Vertical || base.IsOnMenu) ? TextDrawing.MeasureString(val2, m_Text, val3, 0, format) : TextDrawing.MeasureStringLegacy(val2, m_Text, val3, Size.Empty, format));
		if (base.IsOnMenu)
		{
			size.Width += 4;
			if (size2.Height > size.Height)
			{
				m_Rect.Height = size2.Height + 4;
			}
			else
			{
				m_Rect.Height = size.Height + 4;
			}
			m_Rect.Height += _0E77;
			if (base.IsOnCustomizeMenu)
			{
				_0E74 = new Rectangle(m_Rect.Height, 0, m_Rect.Height, m_Rect.Height);
			}
			else
			{
				_0E74 = new Rectangle(0, 0, size.Width, m_Rect.Height);
			}
			m_Rect.Width = size2.Width;
			if (_0E5F != "")
			{
				Size size3 = TextDrawing.MeasureString(val2, _0E5F, val3, 0, format);
				m_Rect.Width += size3.Width + 14;
			}
			_0E75 = new Rectangle(_0E74.Right + 2, (m_Rect.Height - size2.Height) / 2, m_Rect.Width, size2.Height);
			m_Rect.Width += _0E74.Right + 2 + 24;
			m_Rect.Width += _0E78;
		}
		else
		{
			if (m_Orientation == eOrientation.Horizontal && (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right))
			{
				size.Width += 10;
				if (size2.Height > size.Height)
				{
					m_Rect.Height = size2.Height + 6;
				}
				else
				{
					m_Rect.Height = size.Height + 6;
				}
				m_Rect.Height += _0E77;
				_0E74 = Rectangle.Empty;
				if (_0E70 != eButtonStyle.TextOnlyAlways && flag)
				{
					_0E74 = new Rectangle(0, (m_Rect.Height - size.Height) / 2, size.Width, size.Height);
				}
				_0E75 = Rectangle.Empty;
				if (_0E70 != 0 || !flag)
				{
					if (_0E74.Right > 0)
					{
						m_Rect.Width = size2.Width + 1;
						_0E75 = new Rectangle(_0E74.Right - 2, 2, m_Rect.Width, m_Rect.Height - 4);
					}
					else
					{
						m_Rect.Width = size2.Width + 6;
						_0E75 = new Rectangle(3, 2, m_Rect.Width, m_Rect.Height - 4);
					}
				}
				m_Rect.Width += _0E74.Right;
				m_Rect.Width += _0E78;
			}
			else if (m_Orientation == eOrientation.Horizontal)
			{
				if (size2.Width > size.Width)
				{
					m_Rect.Width = size2.Width + 6;
				}
				else
				{
					m_Rect.Width = size.Width + 6;
				}
				m_Rect.Height = size.Height + size2.Height + 10;
				m_Rect.Width += _0E78;
				m_Rect.Height += _0E77;
				if (_0E71 == eImagePosition.Top)
				{
					_0E74 = new Rectangle(0, _0E77 / 2, m_Rect.Width, size.Height + 2);
					_0E75 = new Rectangle((m_Rect.Width - size2.Width) / 2, _0E74.Bottom + 2, size2.Width, size2.Height + 5);
				}
				else
				{
					_0E75 = new Rectangle((m_Rect.Width - size2.Width) / 2, _0E77 / 2, size2.Width, size2.Height + 2);
					_0E74 = new Rectangle(0, _0E75.Bottom + 2, m_Rect.Width, size.Height + 5);
				}
			}
			else
			{
				if (size2.Height > size.Height && _0E70 != 0)
				{
					m_Rect.Width = size2.Height + 6;
				}
				else
				{
					m_Rect.Width = size.Width + 6;
				}
				m_Rect.Width += _0E78;
				if (_0E70 != 0 || !flag)
				{
					if (flag)
					{
						m_Rect.Height = size.Height + size2.Width + 12;
					}
					else
					{
						m_Rect.Height = size2.Width + 6;
					}
				}
				else
				{
					m_Rect.Height = size.Height + 6;
				}
				if (_0E71 == eImagePosition.Top || _0E71 == eImagePosition.Left)
				{
					if (flag)
					{
						_0E74 = new Rectangle(0, 0, m_Rect.Width, size.Height + 5);
					}
					_0E75 = new Rectangle((m_Rect.Width - size2.Height) / 2, _0E74.Bottom + 2, size2.Height, size2.Width + 5);
				}
				else
				{
					_0E75 = new Rectangle((m_Rect.Width - size2.Height) / 2, 0, size2.Height, size2.Width + 5);
					if (flag)
					{
						_0E74 = new Rectangle(0, _0E75.Bottom + 2, m_Rect.Width, size.Height + 5);
					}
				}
				m_Rect.Height += _0E77;
			}
			if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems && (!IsOnMenuBar || _0EC5() != null))
			{
				if (m_Orientation == eOrientation.Horizontal)
				{
					_0E76 = new Rectangle(m_Rect.Right, 0, 12, m_Rect.Height);
					m_Rect.Width += _0E76.Width;
				}
				else
				{
					_0E76 = new Rectangle(0, m_Rect.Bottom, m_Rect.Width, 12);
					m_Rect.Height += _0E76.Height;
				}
			}
		}
		val2.Dispose();
		val = null;
	}

	private void _0ECB(Graphics _٥, Rectangle ल)
	{
		ControlPaint.DrawBorder3D(_٥, ल, (Border3DStyle)2, (Border3DSide)2063);
		if (!ݡ)
		{
			ल.Inflate(-1, -1);
			_٥.FillRectangle((Brush)(object)ColorFunctions.GetPushedBrush(), ल);
		}
		Point[] array = new Point[3];
		array[0].X = ल.Left + (ल.Width - 6) / 2;
		array[0].Y = ल.Top + (ल.Height - 6) / 2 + 3;
		array[1].X = array[0].X + 2;
		array[1].Y = array[0].Y + 2;
		array[2].X = array[1].X + 4;
		array[2].Y = array[1].Y - 4;
		_٥.DrawLines(SystemPens.ControlText, array);
		array[0].X++;
		array[1].X++;
		array[2].X++;
		_٥.DrawLines(SystemPens.ControlText, array);
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		base.OnItemAdded(item);
		OnAppearanceChanged();
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		if (!ຎ.IsEmpty)
		{
			if (ຎ.Width > 0)
			{
				m_Rect.Width = Dpi.Width(ຎ.Width);
			}
			if (ຎ.Height > 0)
			{
				m_Rect.Height = Dpi.Height(ຎ.Height);
			}
		}
		_20B1._0CC6(this);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		if (m_Expanded)
		{
			Expanded = false;
			if (m_Parent != null)
			{
				m_Parent.AutoExpand = false;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		base.InternalKeyDown(objArg);
		if (Expanded || objArg.Handled)
		{
			return;
		}
		if ((int)objArg.KeyCode == 13 || (int)objArg.KeyCode == 13 || ((int)objArg.KeyCode == 39 && base.IsOnMenu))
		{
			if (SubItems.Count > 0 && GetEnabled())
			{
				if (Expanded)
				{
					if (m_Parent != null)
					{
						m_Parent.AutoExpand = false;
					}
					Expanded = false;
				}
				else
				{
					if (m_Parent != null)
					{
						m_Parent.AutoExpand = true;
					}
					Expanded = true;
					if (PopupType == ePopupType.Menu && base.PopupControl is MenuPanel)
					{
						((MenuPanel)(object)base.PopupControl)._1030();
					}
				}
				objArg.Handled = true;
				return;
			}
		}
		else if ((int)objArg.KeyCode == 27 && SubItems.Count > 0 && Expanded)
		{
			Expanded = false;
			if (m_Parent != null)
			{
				m_Parent.AutoExpand = false;
			}
			objArg.Handled = true;
			return;
		}
		base.InternalKeyDown(objArg);
	}

	private void _0ECC(bool ԝ)
	{
		if (ԝ == ݡ)
		{
			return;
		}
		if (ຓ)
		{
			if (ຖ)
			{
				ຓ = false;
			}
			_0ECF();
		}
		if ((IsFadeEnabled || ຓ) && GetEnabled() && Displayed && Visible && WidthInternal > 0 && HeightInternal > 0)
		{
			int num = (ԝ ? 1 : (-1));
			int num2 = (ԝ ? 10 : 255);
			ݡ = ԝ;
			_0ECD(num, num2);
		}
		else
		{
			ݡ = ԝ;
		}
	}

	private void _0ECD(int _0C74, int _0ECE)
	{
		bool flag = false;
		າ.AcquireReaderLock(-1);
		try
		{
			flag = ຮ == null;
		}
		finally
		{
			າ.ReleaseReaderLock();
		}
		if (flag)
		{
			bool flag2 = ݡ;
			ݡ = true;
			try
			{
				bool isReaderLockHeld = າ.IsReaderLockHeld;
				LockCookie lockCookie = default(LockCookie);
				if (isReaderLockHeld)
				{
					lockCookie = າ.UpgradeToWriterLock(-1);
				}
				else
				{
					າ.AcquireWriterLock(-1);
				}
				try
				{
					ຮ = _໒();
				}
				finally
				{
					if (isReaderLockHeld)
					{
						າ.DowngradeFromWriterLock(ref lockCookie);
					}
					else
					{
						າ.ReleaseWriterLock();
					}
				}
			}
			finally
			{
				ݡ = flag2;
			}
		}
		ຯ = _0C74;
		ະ = _0ECE;
		ᦵ.ᦺ(this, _໑);
		_0EB1 = true;
	}

	internal void _0ECF()
	{
		if (_0EB1)
		{
			_0EB1 = false;
			ᦵ._0ECF(this, _໑);
			bool flag = false;
			າ.AcquireReaderLock(-1);
			try
			{
				flag = ຮ != null;
			}
			finally
			{
				າ.ReleaseReaderLock();
			}
			if (flag)
			{
				_໐();
			}
			if (ະ > 230)
			{
				ະ = 255;
			}
			else if (ະ < 0)
			{
				ະ = 0;
			}
			Invalidate();
		}
	}

	private void _໐()
	{
		bool isReaderLockHeld = າ.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = າ.UpgradeToWriterLock(-1);
		}
		else
		{
			າ.AcquireWriterLock(-1);
		}
		try
		{
			((Image)ຮ).Dispose();
			ຮ = null;
		}
		finally
		{
			if (isReaderLockHeld)
			{
				າ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				າ.ReleaseWriterLock();
			}
		}
	}

	private void _໑(object _٠, EventArgs ؿ)
	{
		ະ += ຯ * (ຓ ? ຒ : ຑ);
		if ((ຯ < 0 && ະ <= 0) || (ຯ > 0 && ະ >= 255))
		{
			if (ຓ)
			{
				ຯ *= -1;
				if (ະ >= 255)
				{
					ະ = 255;
				}
				else
				{
					ະ = 0;
				}
				ຕ++;
				if (ດ > 0 && ຕ > ດ)
				{
					_0ECF();
					StopPulse();
				}
			}
			else
			{
				_0ECF();
			}
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && BarFunctions.IsHandleValid(val))
		{
			try
			{
				val.Invalidate(DisplayRectangle);
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Win32Exception)
			{
			}
		}
	}

	private Bitmap _໒()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = new Bitmap(WidthInternal, HeightInternal, (PixelFormat)925707);
		val.MakeTransparent();
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			object containerControl = ContainerControl;
			Control val3 = (Control)((containerControl is Control) ? containerControl : null);
			bool flag = false;
			ItemPaintArgs itemPaintArgs = null;
			if (val3 is ItemControl)
			{
				flag = ((ItemControl)(object)val3).AntiAlias;
				itemPaintArgs = ((ItemControl)(object)val3)._0A92(val2);
			}
			else if (val3 is Bar)
			{
				flag = ((Bar)(object)val3).AntiAlias;
				itemPaintArgs = ((Bar)(object)val3)._0A92(val2);
			}
			else if (val3 is ButtonX)
			{
				flag = ((ButtonX)(object)val3).AntiAlias;
				itemPaintArgs = ((ButtonX)(object)val3)._0A92(val2);
			}
			Matrix val4 = new Matrix();
			val4.Translate((float)(-DisplayRectangle.X), (float)(-DisplayRectangle.Y), (MatrixOrder)1);
			val2.Transform = val4;
			val4.Dispose();
			val4 = null;
			if (flag)
			{
				val2.SmoothingMode = (SmoothingMode)4;
				val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			if (itemPaintArgs == null)
			{
				((Image)val).Dispose();
				return null;
			}
			Paint(itemPaintArgs);
			return val;
		}
		finally
		{
			val2.Dispose();
		}
	}

	public void Pulse()
	{
		Pulse(0);
	}

	public void Pulse(int pulseBeatCount)
	{
		if (DesignMode || ກ == eHotTrackingStyle.None || !IsPulseEnabed)
		{
			return;
		}
		ດ = pulseBeatCount;
		ຕ = 0;
		if (!ຓ)
		{
			ຓ = true;
			if (!ݡ)
			{
				_0ECD(1, 0);
			}
		}
	}

	public void StopPulse()
	{
		if (ຓ)
		{
			ຓ = false;
			if (ݡ)
			{
				_0ECC(ԝ: false);
			}
			_0ECF();
		}
	}

	protected override void OnEnabledChanged()
	{
		if (!GetEnabled())
		{
			_0ECC(ԝ: false);
			_໔(_໕: false);
		}
		base.OnEnabledChanged();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		if (!DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			return;
		}
		bool flag = false;
		Rectangle rectangle = ໜ();
		if (!rectangle.IsEmpty && GetEnabled())
		{
			rectangle.Offset(DisplayRectangle.Location);
			if (rectangle.Contains(objArg.X, objArg.Y))
			{
				if (!_0E73)
				{
					_0E73 = true;
					flag = true;
				}
			}
			else if (_0E73)
			{
				_0E73 = false;
				flag = true;
			}
		}
		if (!ݡ)
		{
			_0ECC(ԝ: true);
			if (GetEnabled() || base.IsOnMenu)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_0E73 = false;
		_0ECC(ԝ: false);
		ݠ = false;
		if (GetEnabled() || base.IsOnMenu)
		{
			Refresh();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseHover()
	{
		base.InternalMouseHover();
		if ((SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && base.IsOnMenu && ShowSubItems && AutoExpandMenuItem && !Expanded && GetEnabled())
		{
			Expanded = true;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		base.InternalMouseDown(objArg);
		ButtonMouseDown(objArg);
	}

	protected virtual void ButtonMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button != 1048576 || !GetEnabled())
		{
			return;
		}
		ݠ = true;
		if ((IsOnMenuBar || ShouldAutoExpandOnClick) && !DesignMode)
		{
			if (objArg.Clicks == 2)
			{
				return;
			}
			if (CanShowPopup)
			{
				if (Expanded)
				{
					Expanded = false;
					if (m_Parent != null)
					{
						m_Parent.AutoExpand = false;
					}
				}
				else
				{
					if (IsOnMenuBar && m_Parent != null)
					{
						m_Parent.AutoExpand = true;
					}
					Expanded = true;
				}
			}
			else
			{
				Refresh();
			}
		}
		else if ((IsOnMenuBar || ShouldAutoExpandOnClick) && DesignMode)
		{
			Expanded = !Expanded;
		}
		else if (!base.IsOnMenu)
		{
			if ((SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && ShowSubItems)
			{
				if (!m_Expanded)
				{
					Rectangle rectangle = ໜ();
					rectangle.Width += 2;
					rectangle.Height += 2;
					rectangle.Offset(m_Rect.X, m_Rect.Y);
					if (rectangle.Contains(objArg.X, objArg.Y))
					{
						if (EffectiveStyle == eDotNetBarStyle.Office2000)
						{
							ݠ = false;
						}
						Expanded = true;
					}
				}
				else
				{
					Expanded = false;
				}
			}
			Refresh();
		}
		else if ((SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && ShowSubItems)
		{
			if (!base.IsOnMenu || !Expanded)
			{
				Expanded = !m_Expanded;
			}
		}
		else
		{
			Refresh();
		}
	}

	internal override void DoAccesibleDefaultAction()
	{
		if (base.VisibleSubItems > 0 && (base.IsOnMenu || IsOnMenuBar || _ૡ || ShouldAutoExpandOnClick))
		{
			if (Expanded)
			{
				Expanded = false;
				if (Parent != null && IsOnMenuBar)
				{
					Parent.AutoExpand = false;
				}
			}
			else
			{
				if (IsOnMenuBar && Parent != null)
				{
					Parent.AutoExpand = true;
				}
				Expanded = true;
			}
			Refresh();
			_ૡ = false;
		}
		else
		{
			RaiseClick(eEventSource.Keyboard);
		}
	}

	internal bool _໓()
	{
		return ShouldAutoExpandOnClick;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		bool flag = true;
		Rectangle rectangle = ໜ();
		rectangle.Width += 2;
		rectangle.Height += 2;
		rectangle.Offset(m_Rect.X, m_Rect.Y);
		if ((int)objArg.Button == 1048576)
		{
			if (GetEnabled() && !DesignMode && _0E7F == eMenuVisibility.VisibleIfRecentlyUsed && !_0E80 && base.IsOnMenu)
			{
				_0E80 = true;
				for (BaseItem parent = Parent; parent != null; parent = parent.Parent)
				{
					if (parent is IPersonalizedMenuItem personalizedMenuItem)
					{
						personalizedMenuItem.RecentlyUsed = true;
					}
				}
			}
			if (!base.IsOnMenu && (SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems && m_HotSubItem == null && !DesignMode && !IsOnMenuBar && !ShouldAutoExpandOnClick)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val == null)
				{
					return;
				}
				val = null;
				if (!rectangle.Contains(objArg.X, objArg.Y))
				{
					bool flag2 = true;
					if (PopupType == ePopupType.ToolBar || PopupType == ePopupType.Menu)
					{
						bool expanded = Expanded;
						base.InternalMouseUp(objArg);
						if (!expanded && Expanded)
						{
							flag2 = false;
						}
						flag = false;
					}
					if (flag2)
					{
						BaseItem.CollapseAll(this);
					}
				}
			}
		}
		if ((!base.IsOnMenu || Parent is ItemContainer) && (SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && ShowSubItems && rectangle.Contains(objArg.X, objArg.Y))
		{
			flag = false;
		}
		if (flag)
		{
			base.InternalMouseUp(objArg);
		}
		if (ݠ)
		{
			ݠ = false;
			if (!base.IsOnMenu || !(Parent is ButtonItem))
			{
				Refresh();
			}
		}
	}

	internal void _໔(bool _໕)
	{
		ݠ = _໕;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		base.InternalDoubleClick(mb, mpos);
		if (Expanded && IsOnMenuBar && m_Parent != null && !DesignMode && PopupType == ePopupType.Menu && (PersonalizedMenus == ePersonalizedMenus.Both || PersonalizedMenus == ePersonalizedMenus.DisplayOnClick) && base.PopupControl is MenuPanel)
		{
			((MenuPanel)(object)base.PopupControl)._1032();
		}
	}

	private void _໖()
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Invalid comparison between Unknown and I4
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Invalid comparison between Unknown and I4
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		if (_0E66 == null && դ < 0 && ຈ == null)
		{
			return;
		}
		if (_0E6A != null)
		{
			_0E6A.Dispose();
		}
		_0E6A = null;
		if (_0E6D != null)
		{
			_0E6D.Dispose();
		}
		_0E6D = null;
		ᇎ ᇎ2 = _0EC5(_1937.ሬ, Color.Black);
		if (ᇎ2 == null)
		{
			return;
		}
		if (GetOwner() is IOwner && ((IOwner)GetOwner()).DisabledImagesGrayScale)
		{
			if (ᇎ2.ᇏ)
			{
				_0E6D = BarFunctions.ॽ(ᇎ2.ᆻ);
			}
			else
			{
				ref Image reference = ref _0E6A;
				Image ᇑ = ᇎ2.ᇑ;
				reference = (Image)(object)ᶣ.ᶧ((ᇑ is Bitmap) ? ᇑ : null);
			}
		}
		if (_0E6D == null && _0E6A == null)
		{
			PixelFormat val = (PixelFormat)2498570;
			if (!ᇎ2.ᇏ && ᇎ2.ᇑ != null)
			{
				val = ᇎ2.ᇑ.PixelFormat;
			}
			if ((int)val == 196865 || (int)val == 197634 || (int)val == 198659)
			{
				val = (PixelFormat)2498570;
			}
			Bitmap val2 = new Bitmap(ᇎ2.ܐ, ᇎ2._0711, val);
			_0E6A = (Image)new Bitmap(ᇎ2.ܐ, ᇎ2._0711, val);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				val3.FillRectangle((Brush)(object)val4, 0, 0, ᇎ2.ܐ, ᇎ2._0711);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			ᇎ2.ᇗ(val3, new Rectangle(0, 0, ᇎ2.ܐ, ᇎ2._0711));
			val3.Dispose();
			val3 = Graphics.FromImage(_0E6A);
			val2.MakeTransparent(Color.White);
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if ((effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && _089F.न >= 8)
			{
				float[][] array = new float[5][];
				float[] array2 = new float[5];
				array[0] = array2;
				float[] array3 = new float[5];
				array[1] = array3;
				float[] array4 = new float[5];
				array[2] = array4;
				array[3] = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f };
				float[] array5 = new float[5];
				array[4] = array5;
				ColorMatrix colorMatrix = new ColorMatrix(array);
				ImageAttributes val5 = new ImageAttributes();
				val5.ClearColorKey();
				val5.SetColorMatrix(colorMatrix);
				val3.DrawImage((Image)(object)val2, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height), 0, 0, ((Image)val2).Width, ((Image)val2).Height, (GraphicsUnit)2, val5);
			}
			else
			{
				ControlPaint.DrawImageDisabled(val3, (Image)(object)val2, 0, 0, ColorFunctions.MenuBackColor(val3));
			}
			val3.Dispose();
			val3 = null;
			((Image)val2).Dispose();
			val2 = null;
			ᇎ2.Dispose();
		}
	}

	private eTextFormat ಲ()
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		eTextFormat2 = (_ງ ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		return eTextFormat2 | eTextFormat.VerticalCenter;
	}

	private ThemeTextFormat _໗()
	{
		return (ThemeTextFormat)1310724;
	}

	internal virtual Font _໘(ItemPaintArgs _0BD2, bool _໙)
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		Font val = null;
		if (_0BD2 != null)
		{
			val = _0BD2.Font;
		}
		if (val == null)
		{
			Control val2 = null;
			if (_0BD2 != null)
			{
				val2 = _0BD2.ContainerControl;
			}
			if (val2 == null)
			{
				object containerControl = ContainerControl;
				val2 = (Control)((containerControl is Control) ? containerControl : null);
			}
			if (val2 != null)
			{
				if (val2.Font != null)
				{
					val = val2.Font;
				}
			}
			else
			{
				val = SystemFonts.DefaultFont;
			}
		}
		if (ݡ || (_໙ && (_0E7B || _0E7A)))
		{
			if (__0EB5 != null && val != null && val.FontFamily.Name == __0EB5.FontFamily.Name && val.SizeInPoints == __0EB5.SizeInPoints)
			{
				return __0EB5;
			}
			try
			{
				Font val3 = null;
				FontStyle val4 = val.Style;
				if (_0E7B)
				{
					val4 = (FontStyle)(val4 | 1);
				}
				if (_0E7A)
				{
					val4 = (FontStyle)(val4 | 4);
				}
				val3 = new Font(val, val4);
				_0EDA();
				__0EB5 = val3;
				return __0EB5;
			}
			catch
			{
			}
		}
		if (_0E7C || _0E7D || _0E7E)
		{
			FontStyle val5 = val.Style;
			if (_0E7C)
			{
				val5 = (FontStyle)(val5 | 1);
			}
			if (_0E7D)
			{
				val5 = (FontStyle)(val5 | 2);
			}
			if (_0E7E)
			{
				val5 = (FontStyle)(val5 | 4);
			}
			Font val6 = null;
			try
			{
				val6 = new Font(val, val5);
			}
			catch
			{
			}
			if (val6 != null)
			{
				val = val6;
			}
		}
		return val;
	}

	private void _0EDA()
	{
		if (__0EB5 != null)
		{
			__0EB5.Dispose();
			__0EB5 = null;
		}
	}

	protected virtual void OnCheckedChanged()
	{
		if (_0E85 != "" && ݟ && Parent != null)
		{
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this && subItem is ButtonItem buttonItem && buttonItem.OptionGroup == _0E85 && buttonItem.Checked)
				{
					buttonItem.Checked = false;
				}
			}
		}
		InvokeCheckedChanged();
	}

	protected internal override void OnVisibleChanged(bool bVisible)
	{
		base.OnVisibleChanged(bVisible);
		if (!bVisible)
		{
			_0ECF();
			ໂ();
		}
		if (!bVisible && Checked && _0E85 != "")
		{
			Checked = false;
			if (Parent == null)
			{
				return;
			}
			{
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this && subItem.GetEnabled() && subItem.Visible && subItem is ButtonItem buttonItem && buttonItem.OptionGroup == _0E85)
					{
						buttonItem.Checked = true;
						break;
					}
				}
				return;
			}
		}
		if (!bVisible || !(_0E85 != "") || base.VisibleSubItems > 1)
		{
			return;
		}
		bool flag = true;
		if (Parent != null)
		{
			foreach (BaseItem subItem2 in Parent.SubItems)
			{
				if (subItem2 != this && subItem2.GetEnabled() && subItem2.Visible && subItem2 is ButtonItem buttonItem2 && buttonItem2.OptionGroup == _0E85 && buttonItem2.Checked)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			Checked = true;
		}
	}

	internal void _0EDB(bool _0BCF)
	{
		ݟ = _0BCF;
	}

	protected virtual void InvokeCheckedChanged()
	{
		if (_0E65 != null)
		{
			_0E65(this, new EventArgs());
		}
		GetIOwnerItemEvents()?.InvokeCheckedChanged(this, new EventArgs());
	}

	protected virtual void InvokeOptionGroupChanging(OptionGroupChangingEventArgs e)
	{
		if (_09D5 != null)
		{
			_09D5(this, e);
		}
		if (!e.Cancel)
		{
			GetIOwnerItemEvents()?.InvokeOptionGroupChanging(this, e);
		}
	}

	protected override void OnClick()
	{
		if (ຘ && _0E85 == "")
		{
			Checked = !Checked;
		}
		base.OnClick();
		if (_0E85 != "" && !ݟ)
		{
			Checked = true;
		}
		ExecuteCommand();
	}

	protected override void OnCommandChanged()
	{
		if (!DesignMode && Command == null)
		{
			Enabled = false;
		}
		base.OnCommandChanged();
	}

	protected override string GetTooltipShortcutString()
	{
		return _0E5F;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeForeColor()
	{
		if (_0D02.IsEmpty)
		{
			return false;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHotForeColor()
	{
		if (_0E79.IsEmpty)
		{
			return false;
		}
		return true;
	}

	internal Rectangle ໜ()
	{
		Rectangle subItemsRect = SubItemsRect;
		if (SplitButton && (ButtonStyle == eButtonStyle.ImageAndText || ImagePosition == eImagePosition.Top || ImagePosition == eImagePosition.Bottom) && (Image != null || ImageIndex >= 0 || !string.IsNullOrEmpty(SymbolRealized)) && Text.Length > 0)
		{
			Rectangle textDrawRect = TextDrawRect;
			textDrawRect.Inflate(0, -2);
			if (ImagePosition == eImagePosition.Left)
			{
				if (Orientation == eOrientation.Horizontal)
				{
					textDrawRect.X -= 3;
				}
				else
				{
					textDrawRect.Y -= 6;
				}
			}
			if (subItemsRect.IsEmpty)
			{
				subItemsRect = textDrawRect;
				if (_0E71 == eImagePosition.Top)
				{
					subItemsRect.X = 0;
					subItemsRect.Width = m_Rect.Width;
					subItemsRect.Height = DisplayRectangle.Bottom - subItemsRect.Y;
				}
				else if (_0E71 == eImagePosition.Bottom)
				{
					subItemsRect.X = 0;
					subItemsRect.Width = m_Rect.Width;
				}
			}
			else
			{
				subItemsRect = Rectangle.Union(subItemsRect, textDrawRect);
			}
			return subItemsRect;
		}
		return subItemsRect;
	}

	protected override void OnDisplayedChanged()
	{
		ݠ = false;
		ݡ = false;
		if (!Displayed && _ຬ)
		{
			ໂ();
		}
	}

	public void AddSubItems(string[] itemTexts)
	{
		AddSubItems(itemTexts, null);
	}

	public void AddSubItems(string[] itemTexts, EventHandler clickHandler)
	{
		if (itemTexts == null || itemTexts.Length == 0)
		{
			return;
		}
		for (int i = 0; i < itemTexts.Length; i++)
		{
			ButtonItem buttonItem = new ButtonItem();
			buttonItem.Text = itemTexts[i];
			if (clickHandler != null)
			{
				buttonItem.Click += clickHandler;
			}
			SubItems.Add(buttonItem);
		}
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}
}
