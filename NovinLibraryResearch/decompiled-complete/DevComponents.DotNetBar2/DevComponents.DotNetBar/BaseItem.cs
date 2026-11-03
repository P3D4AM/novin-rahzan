using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Events;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public abstract class BaseItem : Component, IBlock, ICommandSource, IComparable, IComponent, IDisposable, ICloneable, IBindableComponent
{
	public class ItemAccessibleObject : AccessibleObject
	{
		private BaseItem _0AC3;

		private bool _104A;

		internal BaseItem _၉ => _0AC3;

		public override string Name
		{
			get
			{
				if (_0AC3 == null)
				{
					return "";
				}
				if (_0AC3.AccessibleName != "")
				{
					return _0AC3.AccessibleName;
				}
				if (_0AC3.Text != null)
				{
					return _0AC3.Text.Replace("&", "");
				}
				return _0AC3.Tooltip;
			}
			set
			{
				_0AC3.AccessibleName = value;
			}
		}

		public override string Description
		{
			get
			{
				if (_0AC3 == null)
				{
					return "";
				}
				if (_0AC3.AccessibleDescription != "")
				{
					return _0AC3.AccessibleDescription;
				}
				if (_0AC3.IsOnMenu)
				{
					return ((AccessibleObject)this).Name + " menu item";
				}
				if (_0AC3.IsOnMenuBar)
				{
					return ((AccessibleObject)this).Name + " menu";
				}
				return ((AccessibleObject)this).Name + " button";
			}
		}

		public override AccessibleRole Role
		{
			get
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Invalid comparison between Unknown and I4
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				if (_0AC3 == null || !_0AC3.IsAccessible)
				{
					return (AccessibleRole)0;
				}
				if ((int)_0AC3.AccessibleRole != -1)
				{
					return _0AC3.AccessibleRole;
				}
				BaseItem baseItem = _0AC3;
				while (baseItem.Parent != null)
				{
					baseItem = baseItem.Parent;
				}
				if (!_0AC3.IsOnMenu && !_0AC3.IsOnMenuBar && !baseItem.IsOnMenuBar)
				{
					return (AccessibleRole)43;
				}
				return (AccessibleRole)12;
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
				//IL_0114: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_010c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0113: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				if (_0AC3 == null)
				{
					return (AccessibleStates)1;
				}
				AccessibleStates val = (AccessibleStates)0;
				if (!_0AC3.IsAccessible)
				{
					return (AccessibleStates)1;
				}
				if (!_0AC3.Displayed || !_0AC3.Visible)
				{
					val = (AccessibleStates)(val | 0x8000);
				}
				else
				{
					if (!_0AC3.GetEnabled())
					{
						val = (AccessibleStates)1;
						if (_0AC3.Expanded || _104A || (_0AC3 is ButtonItem && ((ButtonItem)_0AC3).IsMouseOver))
						{
							val = (AccessibleStates)(val | 0x84);
						}
						return val;
					}
					val = (AccessibleStates)((!_0AC3.Expanded && !_104A && (!(_0AC3 is ButtonItem) || !((ButtonItem)_0AC3).IsMouseOver)) ? (val | 0x100000) : (val | 0x84));
					if (_0AC3.ShowSubItems && _0AC3.SubItems.Count > 0)
					{
						val = (AccessibleStates)((!_0AC3.Expanded) ? (val | 0x400) : (val | 0x200));
					}
				}
				return val;
			}
		}

		public override AccessibleObject Parent
		{
			get
			{
				if (_0AC3 == null)
				{
					return null;
				}
				if (_0AC3.Parent != null && (!(_0AC3.Parent is GenericItemContainer) || !((GenericItemContainer)_0AC3.Parent).SystemContainer))
				{
					return _0AC3.Parent.AccessibleObject;
				}
				object containerControl = _0AC3.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					return val.AccessibilityObject;
				}
				return null;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				if (_0AC3 == null)
				{
					return Rectangle.Empty;
				}
				object containerControl = _0AC3.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					return val.RectangleToScreen(_0AC3.DisplayRectangle);
				}
				return _0AC3.m_Rect;
			}
		}

		public override string DefaultAction
		{
			get
			{
				if (_0AC3.AccessibleDefaultActionDescription != "")
				{
					return _0AC3.AccessibleDefaultActionDescription;
				}
				return "Press";
			}
		}

		public override string KeyboardShortcut => _0AC3.ShortcutString;

		public ItemAccessibleObject(BaseItem owner)
		{
			_0AC3 = owner;
			_0AC3.MouseEnter += OnMouseEnter;
			_0AC3.MouseLeave += OnMouseLeave;
		}

		protected virtual void OnMouseEnter(object sender, EventArgs e)
		{
			_104A = true;
		}

		protected virtual void OnMouseLeave(object sender, EventArgs e)
		{
			_104A = false;
		}

		public override int GetChildCount()
		{
			if (_0AC3 == null)
			{
				return 0;
			}
			return _0AC3.SubItems.Count;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (_0AC3 == null || iIndex < 0 || iIndex >= _0AC3.SubItems.Count)
			{
				return null;
			}
			return _0AC3.SubItems[iIndex].AccessibleObject;
		}

		public override void DoDefaultAction()
		{
			if (_0AC3 != null)
			{
				object containerControl = _0AC3.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is MenuPanel && !(val is IAccessibilitySupport))
				{
					object containerControl2 = ((MenuPanel)(object)val).ParentItem.ContainerControl;
					val = (Control)((containerControl2 is Control) ? containerControl2 : null);
				}
				if (val is IAccessibilitySupport accessibilitySupport)
				{
					accessibilitySupport.DoDefaultActionItem = _0AC3;
					_089F.PostMessage(val.Handle, 1131, IntPtr.Zero, IntPtr.Zero);
				}
				((AccessibleObject)this).DoDefaultAction();
			}
		}

		public override AccessibleObject GetSelected()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Invalid comparison between Unknown and I4
			if (_0AC3 == null)
			{
				return ((AccessibleObject)this).GetSelected();
			}
			foreach (BaseItem subItem in _0AC3.SubItems)
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
			if (_0AC3 == null)
			{
				return ((AccessibleObject)this).HitTest(x, y);
			}
			Point point = new Point(x, y);
			foreach (BaseItem subItem in _0AC3.SubItems)
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
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Invalid comparison between Unknown and I4
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Invalid comparison between Unknown and I4
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Invalid comparison between Unknown and I4
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Invalid comparison between Unknown and I4
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Invalid comparison between Unknown and I4
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Invalid comparison between Unknown and I4
			if (_0AC3 == null)
			{
				return ((AccessibleObject)this).Navigate(navdir);
			}
			BaseItem baseItem = null;
			if ((int)navdir == 2 || (int)navdir == 4)
			{
				if (_0AC3.Parent != null)
				{
					BaseItem parent = _0AC3.Parent;
					baseItem = _104B(parent.SubItems, parent.SubItems.IndexOf(_0AC3) + 1);
				}
			}
			else if ((int)navdir == 5)
			{
				if (_0AC3 != null && _0AC3.Parent != null)
				{
					int num = _0AC3.Parent.SubItems.IndexOf(_0AC3);
					if (num < _0AC3.Parent.SubItems.Count - 1)
					{
						baseItem = _0AC3.Parent.SubItems[num + 1];
					}
				}
			}
			else if ((int)navdir == 6)
			{
				if (_0AC3 != null && _0AC3.Parent != null)
				{
					int num2 = _0AC3.Parent.SubItems.IndexOf(_0AC3);
					if (num2 > 0)
					{
						baseItem = _0AC3.Parent.SubItems[num2 - 1];
					}
				}
			}
			else if ((int)navdir == 7)
			{
				baseItem = _104B(_0AC3.SubItems, 0);
			}
			else if ((int)navdir == 8)
			{
				baseItem = _104E(_0AC3.SubItems, _0AC3.SubItems.Count - 1);
			}
			else if ((int)navdir == 1)
			{
				if (_0AC3 != null && _0AC3.Parent.IsContainer && _0AC3.Parent.SystemItem)
				{
					object containerControl = _0AC3.Parent.ContainerControl;
					Control val = (Control)((containerControl is Control) ? containerControl : null);
					if (val != null)
					{
						return val.AccessibilityObject;
					}
				}
				if (_0AC3.Parent != null)
				{
					return _0AC3.Parent.AccessibleObject;
				}
			}
			else if ((int)navdir == 3)
			{
				BaseItem parent2 = _0AC3.Parent;
				baseItem = _104E(_0AC3.SubItems, parent2.SubItems.IndexOf(_0AC3) - 1);
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

	private EventHandler દ;

	private EventHandler ધ;

	private EventHandler ન;

	private EventHandler _0AA9;

	private EventHandler પ;

	private MouseEventHandler ग़;

	private MouseEventHandler ફ;

	private EventHandler બ;

	private EventHandler ભ;

	private MouseEventHandler મ;

	private EventHandler ય;

	private EventHandler ર;

	private EventHandler _0AB1;

	private EventHandler લ;

	private EventHandler ળ;

	private EventHandler ݫ;

	private CollectionChangeEventHandler _0AB4;

	protected string m_Text;

	protected Rectangle m_Rect;

	protected SubItemsCollection m_SubItems;

	protected bool m_Visible;

	protected BaseItem m_HotSubItem;

	protected bool m_IsContainer;

	protected BaseItem m_Parent;

	protected bool m_NeedRecalcSize;

	protected bool m_Expanded;

	protected bool m_AutoExpand;

	protected object m_ContainerControl;

	protected bool m_Displayed;

	protected object m_ItemData;

	protected bool m_Enabled;

	protected bool m_BeginGroup;

	protected eDotNetBarStyle m_Style;

	protected string m_Description;

	protected string m_Tooltip;

	protected string m_Name = "";

	private string વ = "";

	protected string m_Category;

	protected eOrientation m_Orientation;

	protected bool m_SystemItem;

	protected eSupportedOrientation m_SupportedOrientation;

	protected bool m_RecalculatingSize;

	protected bool m_CheckMouseMovePressed;

	private bool શ;

	private int ષ;

	private bool સ;

	private bool હ;

	private bool _0ABA;

	internal Rectangle _0ABB;

	protected ToolTip m_ToolTipWnd;

	private static int _0ABC;

	private bool ઽ;

	private bool _0ABE;

	protected char m_AccessKey;

	private bool _߈;

	private eItemAlignment _0ABF;

	private ShortcutsCollection _0AC0;

	private string _0AC1 = "";

	private Point _0AC2;

	private object _0AC3;

	private bool _0AC4;

	private int _0AC5 = 600;

	private bool _0AC6 = true;

	private bool _0AC7;

	private bool _0AC8;

	protected bool m_ThemeAware;

	private bool _0AC9 = true;

	private ᥡ _0ACA;

	private Cursor ݤ;

	private Cursor _0ACB;

	protected bool m_ShouldSerialize = true;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool _IgnoreClick;

	protected bool m_AllowOnlyOneSubItemExpanded = true;

	private string _0ACC = "";

	private AccessibleRole _0ACD = (AccessibleRole)(-1);

	private string _0ACE = "";

	private string _0ACF = "";

	private bool ૐ = true;

	private string _0AD1 = "";

	private int _0AD2 = -1;

	private string _0AD3 = "";

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Point DesignTimeMouseDownPoint = Point.Empty;

	internal bool _0AD4 = true;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MouseUpNotification;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool MouseDownCapture;

	private bool _0AD5;

	private bool _0AD6;

	private bool __0AD7 = true;

	private bool __0AD8 = true;

	internal bool _0AD9;

	private bool __0ADA = true;

	private bool _0ADB;

	private bool _0ADC;

	private bool __0ADD;

	private bool __0ADE = true;

	private eDesignMarkerOrientation __0ADF;

	private ItemAccessibleObject _ૠ;

	internal bool _ૡ;

	private eContainerState __0AE2;

	private Point __0AE3;

	private bool __0AE4 = true;

	private Padding __0AE5 = new Padding(0);

	private ICommand _૦;

	private object _૧;

	private _0CB4 ݶ;

	private BindingContext _૨;

	private ControlBindingsCollection _૩;

	internal bool IsRendered
	{
		get
		{
			return __0AD7;
		}
		set
		{
			__0AD7 = value;
		}
	}

	internal bool DesignTimeVisible
	{
		get
		{
			return __0AD8;
		}
		set
		{
			__0AD8 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual object ContainerControl
	{
		get
		{
			return GetContainerControl(checkIsContainer: true);
		}
		set
		{
			object containerControl = ContainerControl;
			m_ContainerControl = value;
			OnContainerChanged(containerControl);
		}
	}

	[Browsable(false)]
	public virtual BaseItem Parent
	{
		get
		{
			if (ContainerControl is Bar)
			{
				Bar bar = ContainerControl as Bar;
				if (bar.ParentItem == this)
				{
					return null;
				}
				if (m_Parent == bar.ItemsContainer)
				{
					return bar.ParentItem;
				}
			}
			return m_Parent;
		}
	}

	[Description("Indicates whether is item enabled.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public virtual bool Enabled
	{
		get
		{
			return m_Enabled;
		}
		set
		{
			if (m_Enabled != value)
			{
				m_Enabled = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "Enabled");
				}
				if (!m_Enabled && m_Expanded)
				{
					Expanded = false;
				}
				Refresh();
				OnEnabledChanged();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether this item is beginning of the group.")]
	public virtual bool BeginGroup
	{
		get
		{
			return m_BeginGroup;
		}
		set
		{
			if (m_BeginGroup != value)
			{
				NeedRecalcSize = true;
				m_BeginGroup = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "BeginGroup");
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual int LeftInternal
	{
		get
		{
			return m_Rect.Left;
		}
		set
		{
			if (m_Rect.X != value)
			{
				int x = m_Rect.X;
				m_Rect.X = value;
				OnLeftLocationChanged(x);
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int TopInternal
	{
		get
		{
			return m_Rect.Top;
		}
		set
		{
			if (m_Rect.Y != value)
			{
				int y = m_Rect.Y;
				m_Rect.Y = value;
				OnTopLocationChanged(y);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual int WidthInternal
	{
		get
		{
			return m_Rect.Width;
		}
		set
		{
			if (m_Rect.Width != value)
			{
				m_Rect.Width = value;
				OnExternalSizeChange();
			}
		}
	}

	protected internal virtual bool IsRightToLeft
	{
		get
		{
			return _0AC8;
		}
		set
		{
			if (_0AC8 == value)
			{
				return;
			}
			_0AC8 = value;
			foreach (BaseItem subItem in SubItems)
			{
				subItem.IsRightToLeft = value;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int HeightInternal
	{
		get
		{
			return m_Rect.Height;
		}
		set
		{
			if (m_Rect.Height != value)
			{
				m_Rect.Height = value;
				OnExternalSizeChange();
			}
		}
	}

	[Browsable(true)]
	[Description("Determines whether the item is visible or hidden.")]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	public virtual bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			if (m_Visible != value)
			{
				m_Visible = value;
				NeedRecalcSize = true;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "Visible");
				}
				if (!m_Visible && m_Displayed && m_Parent != null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
				if (!ઽ)
				{
					Displayed = false;
				}
				OnVisibleChanged(value);
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			if (m_Expanded != value)
			{
				m_Expanded = value;
				OnExpandChange();
				if (m_Parent != null)
				{
					m_Parent.OnSubItemExpandChange(this);
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[Browsable(true)]
	public virtual bool AutoCollapseOnClick
	{
		get
		{
			return _0AC6;
		}
		set
		{
			_0AC6 = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AutoExpand
	{
		get
		{
			return m_AutoExpand;
		}
		set
		{
			if (m_AutoExpand != value)
			{
				m_AutoExpand = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "AutoExpand");
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual Rectangle DisplayRectangle => m_Rect;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual Size Size
	{
		get
		{
			return m_Rect.Size;
		}
		set
		{
			if (m_Rect.Size != value)
			{
				m_Rect.Size = value;
				OnExternalSizeChange();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	public virtual Rectangle Bounds
	{
		get
		{
			return m_Rect;
		}
		set
		{
			if (m_Rect != value)
			{
				bool flag = m_Rect.Size != value.Size;
				m_Rect = value;
				if (flag)
				{
					OnExternalSizeChange();
				}
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsContainer => m_IsContainer;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool Displayed
	{
		get
		{
			return m_Displayed;
		}
		set
		{
			if (m_Displayed != value && !_0AD9)
			{
				m_Displayed = value;
				OnDisplayedChanged();
			}
		}
	}

	internal Point MouseDownPt => _0AC2;

	[Browsable(false)]
	public bool IsOnContextMenu => GetOwner() is ContextMenuBar;

	internal bool CanRaiseClick
	{
		get
		{
			if (DesignMode || (!m_Visible && !SystemItem && !IsOnCustomizeMenu) || _IgnoreClick || !GetEnabled())
			{
				return false;
			}
			if (ContainerControl is BaseItemControl && ((BaseItemControl)ContainerControl)._᭐)
			{
				return false;
			}
			if (_089F.फ())
			{
				return false;
			}
			return true;
		}
	}

	internal bool CollapseAllBeforeRaisingClick
	{
		get
		{
			return __0ADA;
		}
		set
		{
			__0ADA = value;
		}
	}

	private BaseItem RootParentItem
	{
		get
		{
			BaseItem parent = Parent;
			while (parent != null && parent.Parent != null)
			{
				parent = parent.Parent;
			}
			return parent;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool Focused => હ;

	protected object OwnerDirect => _0AC3;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool IsRecalculatingSize => m_RecalculatingSize;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool SuspendLayout
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsLayoutPass
	{
		get
		{
			return __0ADD;
		}
		set
		{
			__0ADD = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsLayoutPassInProgress
	{
		get
		{
			for (BaseItem parent = Parent; parent != null; parent = parent.Parent)
			{
				if (parent.IsLayoutPass)
				{
					return true;
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDisposed => _0AD6;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(false)]
	public virtual SubItemsCollection SubItems
	{
		get
		{
			if (m_SubItems == null)
			{
				m_SubItems = new SubItemsCollection(this);
			}
			return m_SubItems;
		}
	}

	[Browsable(false)]
	public int VisibleSubItems
	{
		get
		{
			int num = 0;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem.Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	[Category("Design")]
	[Browsable(false)]
	[Description("Specifies unique ID of the item.")]
	public long Id => ષ;

	[DevCoBrowsable(true)]
	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(null)]
	[Browsable(true)]
	[Localizable(true)]
	[Category("Data")]
	public virtual object Tag
	{
		get
		{
			return m_ItemData;
		}
		set
		{
			m_ItemData = value;
		}
	}

	[Description("Determines the display of the item when shown.")]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	public virtual eDotNetBarStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
			if (m_SubItems != null)
			{
				foreach (BaseItem subItem in m_SubItems)
				{
					subItem.Style = m_Style;
				}
			}
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public virtual eDotNetBarStyle EffectiveStyle
	{
		get
		{
			if (Style == eDotNetBarStyle.StyleManagerControlled)
			{
				return StyleManager.GetEffectiveStyle();
			}
			return Style;
		}
	}

	[Description("Determines whether sub-items are displayed.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public virtual bool ShowSubItems
	{
		get
		{
			return સ;
		}
		set
		{
			if (સ != value)
			{
				NeedRecalcSize = true;
				સ = value;
				if (Displayed)
				{
					Refresh();
				}
			}
		}
	}

	[Description("Determines alignment of the item inside the container.")]
	[DefaultValue(eItemAlignment.Near)]
	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	public virtual eItemAlignment ItemAlignment
	{
		get
		{
			return _0ABF;
		}
		set
		{
			if (_0ABF != value)
			{
				_0ABF = value;
				NeedRecalcSize = true;
				if (Displayed && Parent != null)
				{
					Parent.SubItemSizeChanged(this);
				}
				if (DesignMode)
				{
					OnAppearanceChanged();
					Refresh();
				}
			}
		}
	}

	[Category("Design")]
	[DefaultValue("")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates description of the item that is displayed during design.")]
	[Localizable(true)]
	public virtual string Description
	{
		get
		{
			return m_Description;
		}
		set
		{
			if (!(m_Description == value))
			{
				m_Description = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "Description");
				}
			}
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	[Browsable(true)]
	public virtual bool Stretch
	{
		get
		{
			return _0ABA;
		}
		set
		{
			if (_0ABA != value)
			{
				_0ABA = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "Stretch");
				}
				NeedRecalcSize = true;
				if (Displayed && m_Parent != null && !_0AC7)
				{
					m_Parent.SubItemSizeChanged(this);
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	public virtual string Tooltip
	{
		get
		{
			return m_Tooltip;
		}
		set
		{
			if (m_Tooltip == value)
			{
				return;
			}
			if (value == null)
			{
				value = "";
			}
			m_Tooltip = value;
			if (ToolTipVisible)
			{
				if (string.IsNullOrEmpty(m_Tooltip))
				{
					HideToolTip();
				}
				else
				{
					ToolTip toolTipWnd = m_ToolTipWnd;
					toolTipWnd.Text = m_Tooltip;
					toolTipWnd.ShowToolTip();
					((Control)toolTipWnd).Invalidate();
				}
			}
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "Tooltip");
			}
			OnTooltipChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Category("Design")]
	[Localizable(true)]
	[Description("Indicates item category used to group similar items at design-time.")]
	public virtual string Category
	{
		get
		{
			return m_Category;
		}
		set
		{
			m_Category = value;
		}
	}

	[Category("Design")]
	[Description("Indicates the name used to identify item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public virtual string Name
	{
		get
		{
			if (Site != null)
			{
				m_Name = Site.Name;
			}
			return m_Name;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				m_Name = "";
			}
			else
			{
				m_Name = value;
			}
		}
	}

	[Category("Design")]
	[Description("Indicates global name of the item that is used to synchronize the Global properties for the item across all instances with same global name.")]
	[DefaultValue("")]
	[Browsable(true)]
	public virtual string GlobalName
	{
		get
		{
			return વ;
		}
		set
		{
			વ = value;
		}
	}

	protected virtual bool ShouldSyncProperties
	{
		get
		{
			if (GlobalItem && (GlobalName.Length > 0 || Name.Length > 0))
			{
				return !_0ABE;
			}
			return false;
		}
	}

	[Browsable(false)]
	public eSupportedOrientation SupportedOrientation => m_SupportedOrientation;

	[DevCoBrowsable(true)]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	public virtual bool GlobalItem
	{
		get
		{
			return _0AC9;
		}
		set
		{
			_0AC9 = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual eOrientation Orientation
	{
		get
		{
			return m_Orientation;
		}
		set
		{
			if (m_Orientation != value)
			{
				SetOrientation(value);
				NeedRecalcSize = true;
			}
		}
	}

	internal BaseItem HotSubItem
	{
		get
		{
			return m_HotSubItem;
		}
		set
		{
			BaseItem hotSubItem = m_HotSubItem;
			m_HotSubItem = value;
			if (hotSubItem != value)
			{
				OnHotSubItemChanged(value, hotSubItem);
			}
		}
	}

	protected virtual bool ShowToolTips => true;

	internal virtual Rectangle ScreenRectangle
	{
		get
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(val.PointToScreen(DisplayRectangle.Location), DisplayRectangle.Size);
		}
	}

	protected internal bool ToolTipVisible => m_ToolTipWnd != null;

	internal string OriginalBarName
	{
		get
		{
			return _0AD1;
		}
		set
		{
			_0AD1 = value;
		}
	}

	internal int OriginalPosition
	{
		get
		{
			return _0AD2;
		}
		set
		{
			_0AD2 = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool UserCustomized
	{
		get
		{
			return _0AD5;
		}
		set
		{
			_0AD5 = value;
		}
	}

	[Browsable(false)]
	public ToolTip ToolTipControl => m_ToolTipWnd;

	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	public virtual string KeyTips
	{
		get
		{
			return _0AD3;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_0AD3 = value.ToUpper();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[Bindable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public virtual string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (!(m_Text == value))
			{
				if (value == null)
				{
					m_Text = "";
				}
				else
				{
					m_Text = value;
				}
				m_AccessKey = _089F.य(m_Text);
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "Text");
				}
				NeedRecalcSize = true;
				if (Displayed && m_Parent != null && !_0AC7)
				{
					RecalcSize();
					m_Parent.SubItemSizeChanged(this);
				}
				OnTextChanged();
				OnAppearanceChanged();
			}
		}
	}

	[Description("Indicates whether item can be customized by user.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public virtual bool CanCustomize
	{
		get
		{
			return શ;
		}
		set
		{
			શ = value;
		}
	}

	protected internal bool IsOnCustomizeMenu => ઽ;

	protected internal bool IsOnCustomizeDialog => _0ABE;

	[Browsable(false)]
	public bool IsOnMenu
	{
		get
		{
			if (ContainerControl is MenuPanel || ContainerControl is ᆓ)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	public virtual bool IsOnMenuBar
	{
		get
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null && val is Bar)
			{
				return ((Bar)(object)val).MenuBar;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsOnBar
	{
		get
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null && val is Bar)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public new virtual bool DesignMode => _߈;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool NeedRecalcSize
	{
		get
		{
			return m_NeedRecalcSize;
		}
		set
		{
			m_NeedRecalcSize = value;
			if (value && Parent != null && ContainerControl == Parent.ContainerControl)
			{
				Parent.NeedRecalcSize = true;
			}
		}
	}

	[Browsable(false)]
	public bool SystemItem => m_SystemItem;

	[DefaultValue(true)]
	[Description("Indicates whether access key processing set using ampersand key in Text is enabled. When enabled, and access key is pressed, item will raise Click event.")]
	[Category("Behavior")]
	public bool AccessKeyEnabled
	{
		get
		{
			return __0ADE;
		}
		set
		{
			__0ADE = value;
		}
	}

	protected internal char AccessKey => m_AccessKey;

	[Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Description("Indicates list of shortcut keys for this item.")]
	[Category("Design")]
	[TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[DevCoBrowsable(true)]
	public virtual ShortcutsCollection Shortcuts
	{
		get
		{
			if (_0AC0 == null)
			{
				_0AC0 = new ShortcutsCollection(this);
			}
			return _0AC0;
		}
		set
		{
			IOwner owner = GetOwner() as IOwner;
			if (_0AC0 != null)
			{
				owner?.RemoveShortcutsFromItem(this);
			}
			_0AC0 = value;
			_0AC0.Parent = this;
			if (_0AC0 != null)
			{
				owner?.AddShortcutsFromItem(this);
			}
		}
	}

	protected internal string ShortcutString => _0AC1;

	[Browsable(true)]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	public virtual bool ClickAutoRepeat
	{
		get
		{
			return _0AC4;
		}
		set
		{
			_0AC4 = value;
		}
	}

	[DefaultValue(600)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	public virtual int ClickRepeatInterval
	{
		get
		{
			return _0AC5;
		}
		set
		{
			_0AC5 = value;
		}
	}

	internal ᥡ DesignInsertMarker
	{
		get
		{
			return _0ACA;
		}
		set
		{
			if (_0ACA != value)
			{
				_0ACA = value;
				Refresh();
			}
		}
	}

	protected internal virtual eDesignMarkerOrientation DesignMarkerOrientation
	{
		get
		{
			return __0ADF;
		}
		set
		{
			__0ADF = value;
		}
	}

	internal bool IsDesignMarkHorizontal
	{
		get
		{
			if (__0ADF != 0)
			{
				return __0ADF == eDesignMarkerOrientation.Horizontal;
			}
			if (Parent is ItemContainer)
			{
				if (((ItemContainer)Parent).LayoutOrientation == eOrientation.Horizontal)
				{
					return true;
				}
				return false;
			}
			if (Parent is SimpleItemContainer)
			{
				if (((SimpleItemContainer)Parent).LayoutOrientation == eOrientation.Horizontal)
				{
					return true;
				}
				return false;
			}
			if (m_Orientation == eOrientation.Horizontal && !IsOnMenu && !(Parent is SideBarPanelItem) && !(Parent is ExplorerBarGroupItem))
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[Description("Specifies the mouse cursor displayed when mouse is over the item.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	public virtual Cursor Cursor
	{
		get
		{
			return ݤ;
		}
		set
		{
			if (!(ݤ != value))
			{
				return;
			}
			ݤ = value;
			if (!(ݤ != (Cursor)null) || !m_Visible || !m_Displayed)
			{
				return;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Point pt = val.PointToClient(Control.MousePosition);
				if (m_Rect.Contains(pt))
				{
					val.Cursor = ݤ;
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override ISite Site
	{
		get
		{
			return base.Site;
		}
		set
		{
			base.Site = value;
			OnSiteChanged();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerialize
	{
		get
		{
			return m_ShouldSerialize;
		}
		set
		{
			m_ShouldSerialize = value;
		}
	}

	internal bool IsThemed
	{
		get
		{
			if (m_ThemeAware && BarFunctions.ThemedOS && _0EEE._0EF0 && !BarFunctions.IsWindows8)
			{
				return true;
			}
			return false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual bool IsWindowed => false;

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[DefaultValue(false)]
	[Category("Appearance")]
	public virtual bool ThemeAware
	{
		get
		{
			return m_ThemeAware;
		}
		set
		{
			m_ThemeAware = value;
			if (m_SubItems == null)
			{
				return;
			}
			foreach (BaseItem subItem in m_SubItems)
			{
				subItem.ThemeAware = value;
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public virtual AccessibleObject AccessibleObject => CreateAccessibilityInstance();

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the default action description of the control for use by accessibility client applications.")]
	[Category("Accessibility")]
	[DefaultValue("")]
	public virtual string AccessibleDefaultActionDescription
	{
		get
		{
			return _0ACC;
		}
		set
		{
			_0ACC = value;
		}
	}

	[DefaultValue("")]
	[Category("Accessibility")]
	[Description("Gets or sets the description of the control used by accessibility client applications.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public virtual string AccessibleDescription
	{
		get
		{
			return _0ACF;
		}
		set
		{
			if (_0ACF != value)
			{
				_0ACF = value;
				GenerateAccessibilityEvent((AccessibleEvents)32781);
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Browsable(true)]
	[Description("Gets or sets the name of the control used by accessibility client applications.")]
	[Category("Accessibility")]
	public virtual string AccessibleName
	{
		get
		{
			return _0ACE;
		}
		set
		{
			if (_0ACE != value)
			{
				_0ACE = value;
				GenerateAccessibilityEvent((AccessibleEvents)32780);
			}
		}
	}

	[Category("Accessibility")]
	[Browsable(true)]
	[Description("Gets or sets the accessible role of the item.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	public virtual AccessibleRole AccessibleRole
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _0ACD;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_0ACD = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	public virtual bool IsAccessible
	{
		get
		{
			return ૐ;
		}
		set
		{
			ૐ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eContainerState ContainerState
	{
		get
		{
			return __0AE2;
		}
		internal set
		{
			__0AE2 = value;
		}
	}

	internal Point DragStartPoint
	{
		get
		{
			return __0AE3;
		}
		set
		{
			if (value != __0AE3)
			{
				Point oldValue = __0AE3;
				__0AE3 = value;
				OnDragStartPointChanged(oldValue, value);
			}
		}
	}

	internal bool AllowDrop
	{
		get
		{
			return __0AE4;
		}
		set
		{
			__0AE4 = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Padding Margin
	{
		get
		{
			return __0AE5;
		}
		set
		{
			__0AE5 = value;
		}
	}

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	public virtual Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "Command");
			}
		}
	}

	ICommand ICommandSource.Command
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Localizable(true)]
	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(null)]
	public virtual object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "CommandParameter");
			}
		}
	}

	internal _0CB4 TextMarkupBody => ݶ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public Rectangle TextMarkupBodyBounds
	{
		get
		{
			if (TextMarkupBody == null)
			{
				return Rectangle.Empty;
			}
			return TextMarkupBody.Bounds;
		}
		set
		{
			TextMarkupBody.Bounds = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool IsUsingTextMarkup => ݶ != null;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string PlainText
	{
		get
		{
			if (ݶ == null)
			{
				return Text;
			}
			return ݶ._0CBB;
		}
	}

	protected virtual bool IsMarkupSupported => false;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[Browsable(false)]
	public BindingContext BindingContext
	{
		get
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (_૨ != null)
			{
				return _૨;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				if (!(val is MenuPanel))
				{
					return val.BindingContext;
				}
				object owner = GetOwner();
				if (owner is DotNetBarManager && ((DotNetBarManager)owner).ParentForm != null)
				{
					return ((Control)((DotNetBarManager)owner).ParentForm).BindingContext;
				}
				if (owner is Control)
				{
					return ((Control)owner).BindingContext;
				}
			}
			else
			{
				object owner2 = GetOwner();
				if (owner2 is DotNetBarManager && ((DotNetBarManager)owner2).ParentForm != null)
				{
					return ((Control)((DotNetBarManager)owner2).ParentForm).BindingContext;
				}
				if (owner2 is Control)
				{
					return ((Control)owner2).BindingContext;
				}
			}
			return null;
		}
		set
		{
			_૨ = value;
			UpdateBindings();
		}
	}

	[ParenthesizePropertyName(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Data")]
	public ControlBindingsCollection DataBindings
	{
		get
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			if (_૩ == null)
			{
				_૩ = new ControlBindingsCollection((IBindableComponent)(object)this);
			}
			return _૩;
		}
	}

	[Browsable(false)]
	public bool InvokeRequired
	{
		get
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				return val.InvokeRequired;
			}
			return false;
		}
	}

	[Description("Occurs when Item is clicked.")]
	public event EventHandler Click
	{
		add
		{
			EventHandler eventHandler = દ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = દ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Item is double-clicked.")]
	public event EventHandler DoubleClick
	{
		add
		{
			EventHandler eventHandler = ધ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ધ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ધ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ધ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Item Expanded property has changed.")]
	public event EventHandler ExpandChange
	{
		add
		{
			EventHandler eventHandler = ન;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ન, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ન;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ન, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when item loses input focus.")]
	public event EventHandler LostFocus
	{
		add
		{
			EventHandler eventHandler = _0AA9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0AA9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0AA9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0AA9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when item receives input focus.")]
	public event EventHandler GotFocus
	{
		add
		{
			EventHandler eventHandler = પ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref પ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = પ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref પ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse button is pressed.")]
	public event MouseEventHandler MouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse button is released.")]
	public event MouseEventHandler MouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse enters the item.")]
	public event EventHandler MouseEnter
	{
		add
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse leaves the item.")]
	public event EventHandler MouseLeave
	{
		add
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse moves over the item.")]
	public event MouseEventHandler MouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse remains still inside an item for an amount of time.")]
	public event EventHandler MouseHover
	{
		add
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when copy of the item is made.")]
	public event EventHandler ItemCopied
	{
		add
		{
			EventHandler eventHandler = ર;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ર, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ર;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ર, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Text property of an Item has changed.")]
	public event EventHandler TextChanged
	{
		add
		{
			EventHandler eventHandler = _0AB1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0AB1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0AB1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0AB1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Visible property of an Item has changed.")]
	public event EventHandler VisibleChanged
	{
		add
		{
			EventHandler eventHandler = લ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref લ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = લ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref લ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Enabled property of an Item has changed.")]
	public event EventHandler EnabledChanged
	{
		add
		{
			EventHandler eventHandler = ળ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ળ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ળ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ળ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when item's tooltip visibility has changed.")]
	public event EventHandler ToolTipVisibleChanged
	{
		add
		{
			EventHandler eventHandler = ݫ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ݫ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ݫ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ݫ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when content of SubItems collection has changed")]
	public event CollectionChangeEventHandler SubItemsChanged
	{
		add
		{
			CollectionChangeEventHandler collectionChangeEventHandler = _0AB4;
			CollectionChangeEventHandler collectionChangeEventHandler2;
			do
			{
				collectionChangeEventHandler2 = collectionChangeEventHandler;
				CollectionChangeEventHandler value2 = (CollectionChangeEventHandler)Delegate.Combine(collectionChangeEventHandler2, value);
				collectionChangeEventHandler = Interlocked.CompareExchange(ref _0AB4, value2, collectionChangeEventHandler2);
			}
			while ((object)collectionChangeEventHandler != collectionChangeEventHandler2);
		}
		remove
		{
			CollectionChangeEventHandler collectionChangeEventHandler = _0AB4;
			CollectionChangeEventHandler collectionChangeEventHandler2;
			do
			{
				collectionChangeEventHandler2 = collectionChangeEventHandler;
				CollectionChangeEventHandler value2 = (CollectionChangeEventHandler)Delegate.Remove(collectionChangeEventHandler2, value);
				collectionChangeEventHandler = Interlocked.CompareExchange(ref _0AB4, value2, collectionChangeEventHandler2);
			}
			while ((object)collectionChangeEventHandler != collectionChangeEventHandler2);
		}
	}

	public BaseItem()
		: this("", "")
	{
	}

	public BaseItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public BaseItem(string sItemName, string ItemText)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (ItemText == null)
		{
			ItemText = "";
		}
		m_Text = ItemText;
		if (m_Text != "")
		{
			m_AccessKey = _089F.य(m_Text);
		}
		m_Rect = Rectangle.Empty;
		m_ContainerControl = null;
		m_HotSubItem = null;
		m_IsContainer = false;
		m_SubItems = null;
		m_Parent = null;
		m_NeedRecalcSize = true;
		m_Visible = true;
		m_Expanded = false;
		m_AutoExpand = false;
		m_ItemData = null;
		m_Enabled = true;
		m_BeginGroup = false;
		m_Style = eDotNetBarStyle.OfficeXP;
		m_Description = "";
		m_Tooltip = "";
		m_Name = sItemName;
		m_Category = "";
		m_Orientation = eOrientation.Horizontal;
		m_ToolTipWnd = null;
		ષ = ++_0ABC;
		ઽ = false;
		_0ABE = false;
		m_SystemItem = false;
		_0AC0 = null;
		_߈ = false;
		શ = true;
		સ = true;
		_0ABF = eItemAlignment.Near;
		હ = false;
		_0AC3 = null;
		MarkupTextChanged();
	}

	protected internal virtual void Serialize(ItemSerializationContext context)
	{
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Invalid comparison between Unknown and I4
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected I4, but got Unknown
		XmlElement itemXmlElement = context.ItemXmlElement;
		Type type = GetType();
		AssemblyName name = type.Assembly.GetName();
		if (name.Name != "DevComponents.DotNetBar")
		{
			itemXmlElement.SetAttribute("assembly", name.Name + ", PublicKeyToken=" + Encoding.ASCII.GetString(name.GetPublicKey()));
		}
		else
		{
			itemXmlElement.SetAttribute("assembly", name.Name);
		}
		itemXmlElement.SetAttribute("class", type.FullName);
		itemXmlElement.SetAttribute("name", m_Name);
		itemXmlElement.SetAttribute("globalname", વ);
		itemXmlElement.SetAttribute("text", m_Text);
		itemXmlElement.SetAttribute("iscontainer", XmlConvert.ToString(m_IsContainer));
		itemXmlElement.SetAttribute("visible", XmlConvert.ToString(m_Visible));
		if (m_ItemData != null && m_ItemData.ToString() != "")
		{
			itemXmlElement.SetAttribute("itemdata", m_ItemData.ToString());
		}
		else
		{
			itemXmlElement.SetAttribute("itemdata", "");
		}
		itemXmlElement.SetAttribute("enabled", XmlConvert.ToString(m_Enabled));
		itemXmlElement.SetAttribute("begingroup", XmlConvert.ToString(m_BeginGroup));
		itemXmlElement.SetAttribute("style", XmlConvert.ToString((int)m_Style));
		itemXmlElement.SetAttribute("desc", m_Description);
		itemXmlElement.SetAttribute("tooltip", m_Tooltip);
		itemXmlElement.SetAttribute("category", m_Category);
		itemXmlElement.SetAttribute("cancustomize", XmlConvert.ToString(શ));
		itemXmlElement.SetAttribute("showsubitems", XmlConvert.ToString(સ));
		itemXmlElement.SetAttribute("itemalignment", XmlConvert.ToString((int)_0ABF));
		itemXmlElement.SetAttribute("stretch", XmlConvert.ToString(_0ABA));
		itemXmlElement.SetAttribute("global", XmlConvert.ToString(_0AC9));
		itemXmlElement.SetAttribute("themes", XmlConvert.ToString(m_ThemeAware));
		if (_0ACC != "")
		{
			itemXmlElement.SetAttribute("adefdesc", _0ACC);
		}
		if (_0ACF != "")
		{
			itemXmlElement.SetAttribute("adesc", _0ACF);
		}
		if (_0ACE != "")
		{
			itemXmlElement.SetAttribute("aname", _0ACE);
		}
		if ((int)_0ACD != -1)
		{
			itemXmlElement.SetAttribute("arole", XmlConvert.ToString((int)_0ACD));
		}
		if (!_0AC6)
		{
			itemXmlElement.SetAttribute("autocollapse", XmlConvert.ToString(_0AC6));
		}
		if (_0AC4)
		{
			itemXmlElement.SetAttribute("autorepeat", XmlConvert.ToString(_0AC4));
		}
		if (_0AC5 != 600)
		{
			itemXmlElement.SetAttribute("clickrepinterv", XmlConvert.ToString(_0AC5));
		}
		if (ݤ != (Cursor)null)
		{
			if (ݤ == Cursors.Hand)
			{
				itemXmlElement.SetAttribute("cur", "4");
			}
			else if (ݤ == Cursors.AppStarting)
			{
				itemXmlElement.SetAttribute("cur", "1");
			}
			else if (ݤ == Cursors.Arrow)
			{
				itemXmlElement.SetAttribute("cur", "2");
			}
			else if (ݤ == Cursors.Cross)
			{
				itemXmlElement.SetAttribute("cur", "3");
			}
			else if (ݤ == Cursors.Help)
			{
				itemXmlElement.SetAttribute("cur", "5");
			}
			else if (ݤ == Cursors.HSplit)
			{
				itemXmlElement.SetAttribute("cur", "6");
			}
			else if (ݤ == Cursors.IBeam)
			{
				itemXmlElement.SetAttribute("cur", "7");
			}
			else if (ݤ == Cursors.No)
			{
				itemXmlElement.SetAttribute("cur", "8");
			}
			else if (ݤ == Cursors.NoMove2D)
			{
				itemXmlElement.SetAttribute("cur", "9");
			}
			else if (ݤ == Cursors.NoMoveHoriz)
			{
				itemXmlElement.SetAttribute("cur", "10");
			}
			else if (ݤ == Cursors.NoMoveVert)
			{
				itemXmlElement.SetAttribute("cur", "11");
			}
			else if (ݤ == Cursors.PanEast)
			{
				itemXmlElement.SetAttribute("cur", "12");
			}
			else if (ݤ == Cursors.PanNE)
			{
				itemXmlElement.SetAttribute("cur", "13");
			}
			else if (ݤ == Cursors.PanNorth)
			{
				itemXmlElement.SetAttribute("cur", "14");
			}
			else if (ݤ == Cursors.PanNW)
			{
				itemXmlElement.SetAttribute("cur", "15");
			}
			else if (ݤ == Cursors.PanSE)
			{
				itemXmlElement.SetAttribute("cur", "16");
			}
			else if (ݤ == Cursors.PanSouth)
			{
				itemXmlElement.SetAttribute("cur", "17");
			}
			else if (ݤ == Cursors.PanSW)
			{
				itemXmlElement.SetAttribute("cur", "18");
			}
			else if (ݤ == Cursors.PanWest)
			{
				itemXmlElement.SetAttribute("cur", "19");
			}
			else if (ݤ == Cursors.SizeAll)
			{
				itemXmlElement.SetAttribute("cur", "20");
			}
			else if (ݤ == Cursors.SizeNESW)
			{
				itemXmlElement.SetAttribute("cur", "21");
			}
			else if (ݤ == Cursors.SizeNS)
			{
				itemXmlElement.SetAttribute("cur", "22");
			}
			else if (ݤ == Cursors.SizeNWSE)
			{
				itemXmlElement.SetAttribute("cur", "23");
			}
			else if (ݤ == Cursors.SizeWE)
			{
				itemXmlElement.SetAttribute("cur", "24");
			}
			else if (ݤ == Cursors.UpArrow)
			{
				itemXmlElement.SetAttribute("cur", "25");
			}
			else if (ݤ == Cursors.VSplit)
			{
				itemXmlElement.SetAttribute("cur", "26");
			}
			else if (ݤ == Cursors.WaitCursor)
			{
				itemXmlElement.SetAttribute("cur", "27");
			}
		}
		if (SubItems.Count > 0 && ShouldSerializeSubItems())
		{
			XmlElement xmlElement = itemXmlElement.OwnerDocument.CreateElement("subitems");
			itemXmlElement.AppendChild(xmlElement);
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem.ShouldSerialize)
				{
					XmlElement xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("item");
					type = subItem.GetType();
					xmlElement.AppendChild(xmlElement2);
					context.ItemXmlElement = xmlElement2;
					subItem.Serialize(context);
					context.ItemXmlElement = itemXmlElement;
				}
			}
		}
		if (_0AC0 != null && _0AC0.Count > 0)
		{
			itemXmlElement.SetAttribute("shortcuts", _0AC0.ToString(","));
		}
		if (context.HasSerializeItemHandlers)
		{
			XmlElement xmlElement3 = itemXmlElement.OwnerDocument.CreateElement("customData");
			SerializeItemEventArgs e = new SerializeItemEventArgs(this, itemXmlElement, xmlElement3);
			context.Serializer.InvokeSerializeItem(e);
			if (xmlElement3.Attributes.Count > 0 || xmlElement3.ChildNodes.Count > 0)
			{
				itemXmlElement.AppendChild(xmlElement3);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Deserialize(ItemSerializationContext context)
	{
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		XmlElement itemXmlElement = context.ItemXmlElement;
		m_Name = itemXmlElement.GetAttribute("name");
		if (itemXmlElement.HasAttribute("globalname"))
		{
			વ = itemXmlElement.GetAttribute("globalname");
		}
		m_Text = itemXmlElement.GetAttribute("text");
		m_AccessKey = _089F.य(m_Text);
		m_IsContainer = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("iscontainer"));
		if (context._DesignerHost == null)
		{
			m_Visible = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("visible"));
		}
		else
		{
			TypeDescriptor.GetProperties(this)["Visible"].SetValue(this, XmlConvert.ToBoolean(itemXmlElement.GetAttribute("visible")));
		}
		if (itemXmlElement.GetAttribute("itemdata") != "")
		{
			m_ItemData = itemXmlElement.GetAttribute("itemdata");
		}
		else
		{
			m_ItemData = null;
		}
		m_Enabled = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("enabled"));
		m_BeginGroup = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("begingroup"));
		m_Style = (eDotNetBarStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("style"));
		m_Description = itemXmlElement.GetAttribute("desc");
		m_Tooltip = itemXmlElement.GetAttribute("tooltip");
		m_Category = itemXmlElement.GetAttribute("category");
		શ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("cancustomize"));
		સ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("showsubitems"));
		_0ABF = (eItemAlignment)XmlConvert.ToInt32(itemXmlElement.GetAttribute("itemalignment"));
		if (itemXmlElement.HasAttribute("stretch"))
		{
			_0ABA = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("stretch"));
		}
		else
		{
			_0ABA = false;
		}
		if (itemXmlElement.HasAttribute("global"))
		{
			_0AC9 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("global"));
		}
		if (itemXmlElement.HasAttribute("themes"))
		{
			m_ThemeAware = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("themes"));
		}
		if (itemXmlElement.HasAttribute("adefdesc"))
		{
			_0ACC = itemXmlElement.GetAttribute("adefdesc");
		}
		if (itemXmlElement.HasAttribute("adesc"))
		{
			_0ACF = itemXmlElement.GetAttribute("adesc");
		}
		if (itemXmlElement.HasAttribute("aname"))
		{
			_0ACE = itemXmlElement.GetAttribute("aname");
		}
		if (itemXmlElement.HasAttribute("arole"))
		{
			_0ACD = (AccessibleRole)XmlConvert.ToInt32(itemXmlElement.GetAttribute("arole"));
		}
		if (itemXmlElement.HasAttribute("autocollapse"))
		{
			_0AC6 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("autocollapse"));
		}
		else
		{
			_0AC6 = true;
		}
		if (itemXmlElement.HasAttribute("autorepeat"))
		{
			_0AC4 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("autorepeat"));
		}
		else
		{
			_0AC4 = false;
		}
		if (itemXmlElement.HasAttribute("clickrepinterv"))
		{
			_0AC5 = XmlConvert.ToInt32(itemXmlElement.GetAttribute("clickrepinterv"));
		}
		else
		{
			_0AC5 = 600;
		}
		if (itemXmlElement.HasAttribute("cur"))
		{
			switch (itemXmlElement.GetAttribute("cur"))
			{
			case "4":
				ݤ = Cursors.Hand;
				break;
			case "1":
				ݤ = Cursors.AppStarting;
				break;
			case "2":
				ݤ = Cursors.Arrow;
				break;
			case "3":
				ݤ = Cursors.Cross;
				break;
			case "5":
				ݤ = Cursors.Help;
				break;
			case "6":
				ݤ = Cursors.HSplit;
				break;
			case "7":
				ݤ = Cursors.IBeam;
				break;
			case "8":
				ݤ = Cursors.No;
				break;
			case "9":
				ݤ = Cursors.NoMove2D;
				break;
			case "10":
				ݤ = Cursors.NoMoveHoriz;
				break;
			case "11":
				ݤ = Cursors.NoMoveVert;
				break;
			case "12":
				ݤ = Cursors.PanEast;
				break;
			case "13":
				ݤ = Cursors.PanNE;
				break;
			case "14":
				ݤ = Cursors.PanNorth;
				break;
			case "15":
				ݤ = Cursors.PanNW;
				break;
			case "16":
				ݤ = Cursors.PanSE;
				break;
			case "17":
				ݤ = Cursors.PanSouth;
				break;
			case "18":
				ݤ = Cursors.PanSW;
				break;
			case "19":
				ݤ = Cursors.PanWest;
				break;
			case "20":
				ݤ = Cursors.SizeAll;
				break;
			case "21":
				ݤ = Cursors.SizeNESW;
				break;
			case "22":
				ݤ = Cursors.SizeNS;
				break;
			case "23":
				ݤ = Cursors.SizeNWSE;
				break;
			case "24":
				ݤ = Cursors.SizeWE;
				break;
			case "25":
				ݤ = Cursors.UpArrow;
				break;
			case "26":
				ݤ = Cursors.VSplit;
				break;
			case "27":
				ݤ = Cursors.WaitCursor;
				break;
			}
		}
		XmlNodeList elementsByTagName = itemXmlElement.GetElementsByTagName("subitems");
		if (elementsByTagName.Count > 0)
		{
			foreach (XmlElement childNode in elementsByTagName[0].ChildNodes)
			{
				BaseItem baseItem = context.CreateItemFromXml(childNode);
				if (baseItem != null)
				{
					SubItems.Add(baseItem);
					context.ItemXmlElement = childNode;
					baseItem.Deserialize(context);
					context.ItemXmlElement = itemXmlElement;
				}
			}
		}
		if (itemXmlElement.HasAttribute("shortcuts"))
		{
			if (_0AC0 == null)
			{
				_0AC0 = new ShortcutsCollection(this);
			}
			_0AC0.FromString(itemXmlElement.GetAttribute("shortcuts"), ",");
		}
		if (context.HasDeserializeItemHandlers)
		{
			XmlNodeList elementsByTagName2 = itemXmlElement.GetElementsByTagName("customData");
			if (elementsByTagName2.Count > 0)
			{
				XmlElement customXmlElement = elementsByTagName2[0] as XmlElement;
				SerializeItemEventArgs e = new SerializeItemEventArgs(this, itemXmlElement, customXmlElement);
				context.Serializer.InvokeDeserializeItem(e);
			}
		}
		MarkupTextChanged();
	}

	protected virtual bool ShouldSerializeSubItems()
	{
		return true;
	}

	internal void NotifyScaleItem(SizeF factor)
	{
		ScaleItem(factor);
	}

	protected virtual void ScaleItem(SizeF factor)
	{
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			subItem.NotifyScaleItem(factor);
		}
	}

	protected internal virtual void OnContainerChanged(object objOldContainer)
	{
		if (m_SubItems == null || !IsContainer)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			subItem.OnContainerChanged(objOldContainer);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public object GetContainerControl(bool checkIsContainer)
	{
		if (m_Parent == null)
		{
			return m_ContainerControl;
		}
		if (m_ContainerControl == null && (m_Parent.IsContainer || !checkIsContainer))
		{
			return m_Parent.GetContainerControl(checkIsContainer);
		}
		return m_ContainerControl;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void SetParent(BaseItem o)
	{
		m_Parent = o;
		m_NeedRecalcSize = true;
		OnParentChanged();
	}

	protected virtual void OnParentChanged()
	{
	}

	internal bool GetEnabled(Control container)
	{
		if (!m_Enabled || container == null)
		{
			return m_Enabled;
		}
		if (!container.Enabled)
		{
			return false;
		}
		if (m_Parent != null)
		{
			for (BaseItem parent = m_Parent; parent != null; parent = parent.Parent)
			{
				if (!parent.Enabled)
				{
					return false;
				}
			}
		}
		if (container.Enabled)
		{
			return m_Enabled;
		}
		return false;
	}

	internal bool GetEnabled()
	{
		object containerControl = GetContainerControl(checkIsContainer: false);
		return GetEnabled((Control)((containerControl is Control) ? containerControl : null));
	}

	protected virtual void OnExternalSizeChange()
	{
	}

	protected internal virtual void OnVisibleChanged(bool newValue)
	{
		if (લ != null)
		{
			લ(this, new EventArgs());
		}
		if (DesignMode)
		{
			if (Parent != null)
			{
				Parent.NeedRecalcSize = true;
				Parent.Refresh();
			}
		}
		else if (Parent != null && Parent is ItemContainer)
		{
			Parent.NeedRecalcSize = true;
		}
	}

	internal void SetVisibleDirect(bool b)
	{
		m_Visible = b;
		if (!m_Visible && !ઽ)
		{
			m_Displayed = false;
		}
	}

	internal void SetDisplayRectangle(Rectangle r)
	{
		m_Rect = r;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetViewRectangle(Rectangle r)
	{
		_0ABB = r;
	}

	protected internal void SetIsContainer(bool b)
	{
		m_IsContainer = b;
	}

	protected virtual void OnDisplayedChanged()
	{
		if (Displayed && Visible)
		{
			GenerateAccessibilityEvent((AccessibleEvents)32770);
		}
		else
		{
			GenerateAccessibilityEvent((AccessibleEvents)32771);
		}
	}

	protected internal virtual void OnProcessDelayedCommands()
	{
	}

	protected virtual void OnOwnerChanged()
	{
	}

	protected virtual void OnSubItemsChanged(CollectionChangeEventArgs e)
	{
		_0AB4?.Invoke(this, e);
	}

	protected virtual void OnSubItemsChanged(CollectionChangeAction action, BaseItem item)
	{
		if (_0AB4 != null)
		{
			OnSubItemsChanged(new CollectionChangeEventArgs(action, item));
		}
	}

	protected internal virtual void OnItemAdded(BaseItem item)
	{
		OnSubItemsChanged(CollectionChangeAction.Add, item);
		GetIOwnerItemEvents()?.InvokeItemAdded(item, new EventArgs());
	}

	protected internal virtual void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		if (item != null)
		{
			OnSubItemsChanged(CollectionChangeAction.Remove, item);
			GetIOwnerItemEvents()?.InvokeItemRemoved(item, this, itemIndex);
		}
	}

	protected virtual void OnClick()
	{
	}

	protected internal virtual void OnSubItemsClear()
	{
	}

	protected internal virtual void OnBeforeItemRemoved(BaseItem item)
	{
	}

	protected virtual void OnIsOnCustomizeMenuChanged()
	{
	}

	protected virtual void OnIsOnCustomizeDialogChanged()
	{
	}

	protected virtual void OnDesignModeChanged()
	{
	}

	protected virtual void OnTooltip(bool bShow)
	{
	}

	protected virtual void OnSubItemGotFocus(BaseItem item)
	{
	}

	protected virtual void OnSubItemLostFocus(BaseItem item)
	{
	}

	protected virtual void OnEnabledChanged()
	{
		if (ળ != null)
		{
			ળ(this, new EventArgs());
		}
	}

	protected virtual void OnTopLocationChanged(int oldValue)
	{
	}

	protected virtual void OnLeftLocationChanged(int oldValue)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void InternalMouseEnter()
	{
		if (!_߈)
		{
			if (બ != null)
			{
				બ(this, new EventArgs());
			}
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseEnter(this, new EventArgs());
			}
		}
		if (DesignMode)
		{
			return;
		}
		ResetHover();
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (ݤ != (Cursor)null && val != null)
		{
			_0ACB = val.Cursor;
			val.Cursor = ݤ;
		}
		if ((BarUtilities.AlwaysGenerateAccessibilityFocusEvent && !(val is MenuPanel)) || (val is Bar && IsOnMenuBar))
		{
			if (val.AccessibilityObject is Bar.BarAccessibleObject barAccessibleObject)
			{
				barAccessibleObject._၈(this, (AccessibleEvents)32773);
			}
			else if (val.AccessibilityObject is ItemControlAccessibleObject itemControlAccessibleObject)
			{
				itemControlAccessibleObject._၈(this, (AccessibleEvents)32773);
			}
		}
		else if (val is MenuPanel && !((Control)(MenuPanel)(object)val).IsDisposed && val.AccessibilityObject is MenuPanel.PopupMenuAccessibleObject popupMenuAccessibleObject)
		{
			popupMenuAccessibleObject._၈(this, (AccessibleEvents)32773);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void InternalMouseHover()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (_߈)
		{
			return;
		}
		if (m_HotSubItem != null)
		{
			if (!m_AutoExpand)
			{
				BaseItem baseItem = ExpandedItem();
				if (baseItem != null && m_HotSubItem != baseItem && baseItem.Visible && (IsOnMenu || ContainerControl is Bar || ContainerControl is RibbonBar))
				{
					baseItem.Expanded = false;
				}
			}
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseHover();
			}
			return;
		}
		if ((int)Control.MouseButtons == 0)
		{
			ShowToolTip();
		}
		if (!_߈)
		{
			if (ય != null)
			{
				ય(this, new EventArgs());
			}
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseHover(this, new EventArgs());
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseLeave()
	{
		if (_0ACB != (Cursor)null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Cursor = _0ACB;
			}
			_0ACB = null;
		}
		if (_߈)
		{
			return;
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			HotSubItem = null;
		}
		else
		{
			if (ݶ != null)
			{
				_0CB4 obj = ݶ;
				object containerControl2 = ContainerControl;
				obj.ભ((Control)((containerControl2 is Control) ? containerControl2 : null));
			}
			if (ભ != null)
			{
				ભ(this, new EventArgs());
			}
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseLeave(this, new EventArgs());
			}
		}
		HideToolTip();
	}

	protected virtual bool ShouldCollapseParentItem()
	{
		return true;
	}

	protected void RaiseMouseDown(MouseEventArgs objArg)
	{
		if (ग़ != null)
		{
			ग़.Invoke((object)this, objArg);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Invalid comparison between Unknown and I4
		IOwner owner = null;
		_0AC2 = new Point(objArg.X, objArg.Y);
		BaseItem baseItem = ExpandedItem();
		if (baseItem != null && baseItem != m_HotSubItem && !DesignMode && m_AllowOnlyOneSubItemExpanded && ShouldCollapseParentItem())
		{
			baseItem.Expanded = false;
			m_AutoExpand = false;
		}
		if (_߈ && (શ || (Site != null && Site.DesignMode)))
		{
			if (IsContainer && SubItems.Count > 0)
			{
				BaseItem baseItem2 = ItemAtLocation(objArg.X, objArg.Y);
				if (baseItem2 != null && (baseItem2.CanCustomize || (Site != null && Site.DesignMode) || (baseItem2.Site != null && baseItem2.Site.DesignMode)) && (!baseItem2.DesignMode || baseItem2.DesignTimeVisible))
				{
					if (GetOwner() is IOwner owner2)
					{
						owner2.SetFocusItem(baseItem2);
					}
				}
				else if (baseItem2 == null && GetOwner() is IOwner owner3)
				{
					owner3.SetFocusItem(null);
				}
				if (baseItem2 != null && baseItem2 != this)
				{
					baseItem2.InternalMouseDown(objArg);
				}
			}
			if ((int)objArg.Button == 2097152 && !m_IsContainer && GetOwner() is IOwner owner4)
			{
				owner4.DesignTimeContextMenu(this);
			}
		}
		if (!DesignMode && GetOwner() is IOwner owner5 && owner5.GetFocusItem() != null)
		{
			BaseItem baseItem3 = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem3 != owner5.GetFocusItem())
			{
				owner5.GetFocusItem().ReleaseFocus();
			}
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseDown(objArg);
		}
		else if (!_߈)
		{
			if (ݶ != null)
			{
				_0CB4 obj = ݶ;
				object containerControl = ContainerControl;
				obj.ग़((Control)((containerControl is Control) ? containerControl : null), objArg);
			}
			RaiseMouseDown(objArg);
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseDown(this, objArg);
			}
		}
		HideToolTip();
	}

	protected void RaiseMouseUp(MouseEventArgs objArg)
	{
		if (ફ != null)
		{
			ફ.Invoke((object)this, objArg);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Invalid comparison between Unknown and I4
		if (m_HotSubItem != null)
		{
			if (m_HotSubItem.DisplayRectangle.Contains(objArg.X, objArg.Y) || m_HotSubItem.MouseUpNotification)
			{
				m_HotSubItem.InternalMouseUp(objArg);
			}
		}
		else if (!_߈)
		{
			if (ݶ != null)
			{
				_0CB4 obj = ݶ;
				object containerControl = ContainerControl;
				obj.ફ((Control)((containerControl is Control) ? containerControl : null), objArg);
			}
			RaiseMouseUp(objArg);
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseUp(this, objArg);
			}
			if (((int)objArg.Button == 1048576 || ((int)objArg.Button == 2097152 && IsOnContextMenu)) && DisplayRectangle.Contains(objArg.X, objArg.Y) && ((DisplayRectangle.Contains(_0AC2) && GetEnabled()) || IsOnMenu))
			{
				RaiseClick(eEventSource.Mouse);
			}
		}
		else
		{
			BaseItem baseItem = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem != null && baseItem != this)
			{
				baseItem.InternalMouseUp(objArg);
			}
		}
		_0AC2 = Point.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void InternalKeyDown(KeyEventArgs objArg)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		if (_߈)
		{
			objArg.Handled = true;
			return;
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalKeyDown(objArg);
		}
		if (((int)objArg.KeyCode == 13 || (int)objArg.KeyCode == 13) && !IsOnMenuBar && ((!IsOnMenu && !ShowSubItems) || SubItems.Count <= 0) && GetEnabled())
		{
			RaiseClick(eEventSource.Keyboard);
			objArg.Handled = true;
			if (GetOwner() is RibbonStrip || GetOwner() is RibbonBar)
			{
				((ItemControl)GetOwner()).ReleaseFocus();
				((ItemControl)GetOwner())._09A9 = false;
			}
		}
	}

	protected virtual void LeaveHotSubItem(BaseItem newMouseOverItem)
	{
		BaseItem hotSubItem = m_HotSubItem;
		if (hotSubItem != null)
		{
			hotSubItem.InternalMouseLeave();
			if (newMouseOverItem != null && hotSubItem.Expanded && (IsOnMenu || (ContainerControl is Bar && ((Bar)ContainerControl).BarType == eBarType.MenuBar)))
			{
				hotSubItem.Expanded = false;
			}
			HotSubItem = null;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Invalid comparison between Unknown and I4
		if (_߈ && (int)objArg.Button == 1048576 && (Math.Abs(objArg.X - _0AC2.X) >= 2 || Math.Abs(objArg.Y - _0AC2.Y) >= 2))
		{
			BaseItem baseItem = FocusedItem();
			if (baseItem != null && baseItem.CanCustomize)
			{
				if (!baseItem.SystemItem && GetOwner() is IOwner owner)
				{
					owner.StartItemDrag(baseItem);
				}
			}
			else if (!m_IsContainer && !SystemItem && શ && GetOwner() is IOwner owner2)
			{
				owner2.StartItemDrag(this);
			}
		}
		else if (m_IsContainer && !_߈)
		{
			BaseItem baseItem2 = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem2 != m_HotSubItem && ((int)objArg.Button == 0 || m_HotSubItem == null || m_CheckMouseMovePressed) && (m_HotSubItem == null || !m_HotSubItem.MouseDownCapture || (int)objArg.Button != 1048576))
			{
				LeaveHotSubItem(baseItem2);
				if (baseItem2 != null)
				{
					if (m_AutoExpand)
					{
						BaseItem baseItem3 = ExpandedItem();
						if (baseItem3 != null && baseItem3 != baseItem2)
						{
							baseItem3.Expanded = false;
						}
					}
					if (baseItem2 != this)
					{
						baseItem2.InternalMouseEnter();
						baseItem2.InternalMouseMove(objArg);
						if (baseItem2.GetEnabled() && m_AutoExpand && baseItem2.ShowSubItems)
						{
							if (baseItem2 is PopupItem)
							{
								PopupItem popupItem = baseItem2 as PopupItem;
								ePopupAnimation popupAnimation = popupItem.PopupAnimation;
								popupItem.PopupAnimation = ePopupAnimation.None;
								if (baseItem2.SubItems.Count > 0)
								{
									baseItem2.Expanded = true;
								}
								popupItem.PopupAnimation = popupAnimation;
							}
							else
							{
								baseItem2.Expanded = true;
							}
						}
						HotSubItem = baseItem2;
					}
				}
				else
				{
					HotSubItem = null;
				}
				if (મ != null)
				{
					મ.Invoke((object)this, objArg);
				}
			}
			else if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseMove(objArg);
			}
			else
			{
				if (મ != null)
				{
					મ.Invoke((object)this, objArg);
				}
				IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
				if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
				{
					iOwnerItemEvents.InvokeMouseMove(this, objArg);
				}
			}
			if (ݶ != null)
			{
				Control parentControl = GetParentControl();
				if (parentControl != null)
				{
					ݶ.મ(parentControl, objArg);
				}
			}
		}
		else
		{
			if (_߈)
			{
				return;
			}
			if (ݶ != null)
			{
				Control parentControl2 = GetParentControl();
				if (parentControl2 != null)
				{
					ݶ.મ(parentControl2, objArg);
				}
			}
			if (મ != null)
			{
				મ.Invoke((object)this, objArg);
			}
			IOwnerItemEvents iOwnerItemEvents2 = GetIOwnerItemEvents();
			if (iOwnerItemEvents2 != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents2.InvokeMouseMove(this, objArg);
			}
		}
	}

	internal Control GetParentControl()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			for (BaseItem baseItem = ContainerControl as BaseItem; baseItem != null; baseItem = baseItem.Parent)
			{
				object containerControl2 = baseItem.ContainerControl;
				val = (Control)((containerControl2 is Control) ? containerControl2 : null);
				if (val != null)
				{
					break;
				}
			}
		}
		return val;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalClick(mb, mpos);
		}
		else if (ݶ != null)
		{
			_0CB4 obj = ݶ;
			object containerControl = ContainerControl;
			obj.દ((Control)((containerControl is Control) ? containerControl : null));
		}
	}

	private void ClickCollapseAll()
	{
		if (_0AC6)
		{
			if ((!SystemItem || (SystemItem && Name.StartsWith("mdi-"))) && IsOnPopup(this) && (m_Parent == null || !m_Parent.SystemItem || m_Parent is DisplayMoreItem) && (!(this is PopupItem) || !((PopupItem)this).ShowSubItems || (SubItems.Count <= 0 && ((PopupItem)this).PopupType != ePopupType.Container)))
			{
				CollapseAll(this);
			}
			if (m_Parent != null && m_Parent.AutoExpand && SubItems.Count == 0)
			{
				m_Parent.AutoExpand = false;
			}
		}
	}

	public virtual void RaiseClick(eEventSource source)
	{
		if (!CanRaiseClick)
		{
			return;
		}
		IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
		BaseItem rootParentItem = RootParentItem;
		if (__0ADA)
		{
			ClickCollapseAll();
		}
		if (_0ADB)
		{
			return;
		}
		_0ADB = true;
		try
		{
			OnClick();
			if (દ != null)
			{
				દ(this, new EventSourceArgs(source));
			}
			if (rootParentItem != null && rootParentItem.ContainerControl is Bar)
			{
				if (rootParentItem.ContainerControl is Bar bar)
				{
					bar.గ(this, new EventSourceArgs(source));
				}
			}
			else if (GetOwner() is Bar && iOwnerItemEvents == null)
			{
				((Bar)GetOwner()).గ(this, new EventSourceArgs(source));
			}
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeItemClick(this);
			}
		}
		finally
		{
			_0ADB = false;
		}
		if (!__0ADA)
		{
			ClickCollapseAll();
		}
	}

	public virtual void RaiseClick()
	{
		RaiseClick(eEventSource.Code);
	}

	internal void ClearClick()
	{
		દ = null;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalDoubleClick(mb, mpos);
			return;
		}
		InvokeDoubleClick();
		IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
		if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
		{
			iOwnerItemEvents.InvokeItemDoubleClick(this, new MouseEventArgs(mb, 2, mpos.X, mpos.Y, 0));
		}
	}

	protected virtual void InvokeDoubleClick()
	{
		if (ધ != null)
		{
			ધ(this, new EventArgs());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void OnGotFocus()
	{
		હ = true;
		if (!_߈)
		{
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeGotFocus(this, new EventArgs());
			}
		}
		Refresh();
		OnGotFocus(EventArgs.Empty);
	}

	protected virtual void OnGotFocus(EventArgs e)
	{
		if (પ != null)
		{
			પ(this, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void OnLostFocus()
	{
		if (IsDisposed)
		{
			return;
		}
		if (!_߈)
		{
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeLostFocus(this, new EventArgs());
			}
		}
		else
		{
			InternalMouseLeave();
		}
		હ = false;
		Refresh();
		OnLostFocus(EventArgs.Empty);
	}

	protected virtual void OnLostFocus(EventArgs e)
	{
		if (_0AA9 != null)
		{
			_0AA9(this, e);
		}
	}

	public virtual void Focus()
	{
		if (IsOnPopup(this) && ContainerControl is MenuPanel menuPanel && menuPanel.ParentItem.Parent == null)
		{
			menuPanel._102E(this);
			return;
		}
		if (GetOwner() is IOwner owner)
		{
			owner.SetFocusItem(this);
		}
		else
		{
			OnGotFocus();
		}
		if (Parent != null)
		{
			Parent.OnSubItemGotFocus(this);
		}
	}

	public virtual void ReleaseFocus()
	{
		if (!હ)
		{
			return;
		}
		if (IsOnPopup(this) && ContainerControl is MenuPanel menuPanel && menuPanel.ParentItem.Parent == null)
		{
			menuPanel._102E(null);
			return;
		}
		if (GetOwner() is IOwner owner)
		{
			owner.SetFocusItem(null);
		}
		else
		{
			OnLostFocus();
		}
		if (Parent != null)
		{
			Parent.OnSubItemLostFocus(this);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void ContainerLostFocus(bool appLostFocus)
	{
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				subItem.ContainerLostFocus(appLostFocus);
			}
		}
		if (m_HotSubItem != null)
		{
			bool flag = true;
			if (!appLostFocus)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					Point pt = val.PointToClient(Control.MousePosition);
					if (m_HotSubItem.DisplayRectangle.Contains(pt))
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				m_HotSubItem.InternalMouseLeave();
				HotSubItem = null;
			}
		}
		if (Focused && GetOwner() is IOwner owner)
		{
			owner.SetFocusItem(null);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void ContainerGotFocus()
	{
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			subItem.ContainerGotFocus();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void SubItemSizeChanged(BaseItem objChildItem)
	{
	}

	public virtual BaseItem ItemAtLocation(int x, int y)
	{
		if (!m_IsContainer)
		{
			return null;
		}
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				if ((subItem.Visible || ઽ) && subItem.Displayed && subItem.DisplayRectangle.Contains(x, y) && (_0ABB.IsEmpty || _0ABB.Contains(x, y)))
				{
					return subItem;
				}
			}
		}
		return null;
	}

	protected internal virtual BaseItem ExpandedItem()
	{
		if (!m_IsContainer)
		{
			return null;
		}
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				if (subItem is GalleryContainer && ((GalleryContainer)subItem).IsGalleryPopupOpen)
				{
					return subItem;
				}
				if (subItem is ItemContainer)
				{
					BaseItem baseItem2 = subItem.ExpandedItem();
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
				else if (subItem.Expanded)
				{
					return subItem;
				}
			}
		}
		return null;
	}

	protected internal BaseItem FocusedItem()
	{
		if (!m_IsContainer)
		{
			return null;
		}
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				if (subItem.Focused)
				{
					return subItem;
				}
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.FocusedItem();
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
			}
		}
		return null;
	}

	public object GetOwner()
	{
		if (_0AC3 != null)
		{
			return _0AC3;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val is Bar)
		{
			return ((Bar)(object)val).Owner;
		}
		if (val is MenuPanel)
		{
			return ((MenuPanel)(object)val).Owner;
		}
		if (m_Parent != null)
		{
			return m_Parent.GetOwner();
		}
		return null;
	}

	protected internal IOwnerItemEvents GetIOwnerItemEvents()
	{
		return GetOwner() as IOwnerItemEvents;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetOwner(object owner)
	{
		if (_0AC3 != owner)
		{
			_0AC3 = owner;
			OnOwnerChanged();
		}
	}

	public virtual void RecalcSize()
	{
		m_NeedRecalcSize = false;
	}

	public abstract void Paint(ItemPaintArgs p);

	public abstract BaseItem Copy();

	protected virtual void CopyToItem(BaseItem objCopy)
	{
		objCopy.Visible = m_Visible;
		objCopy.SetIsContainer(m_IsContainer);
		objCopy.AutoExpand = m_AutoExpand;
		objCopy.BeginGroup = m_BeginGroup;
		objCopy.Enabled = m_Enabled;
		objCopy.Description = m_Description;
		objCopy.Tooltip = m_Tooltip;
		objCopy.Category = m_Category;
		objCopy.Orientation = m_Orientation;
		objCopy.Text = m_Text;
		objCopy.Name = m_Name;
		objCopy.GlobalName = વ;
		objCopy.GlobalItem = _0AC9;
		objCopy.Style = m_Style;
		objCopy.SetDesignMode(_߈);
		objCopy.ShowSubItems = સ;
		objCopy.Stretch = _0ABA;
		if (_0AC0 != null && _0AC0.Count > 0)
		{
			objCopy.Shortcuts.FromString(_0AC0.ToString(","), ",");
		}
		objCopy.દ = દ;
		objCopy.ન = ન;
		objCopy._0AA9 = _0AA9;
		objCopy.પ = પ;
		objCopy.ग़ = ग़;
		objCopy.બ = બ;
		objCopy.ય = ય;
		objCopy.ભ = ભ;
		objCopy.મ = મ;
		objCopy.ફ = ફ;
		objCopy.લ = લ;
		objCopy.ળ = ળ;
		objCopy._0AB1 = _0AB1;
		objCopy.IsRightToLeft = IsRightToLeft;
		objCopy.Cursor = ݤ;
		objCopy.ThemeAware = m_ThemeAware;
		objCopy.AutoCollapseOnClick = AutoCollapseOnClick;
		objCopy.Tag = m_ItemData;
		objCopy.CanCustomize = શ;
		objCopy.Command = Command;
		objCopy.CommandParameter = CommandParameter;
		objCopy.KeyTips = KeyTips;
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				objCopy.SubItems.Add(subItem.Copy());
			}
		}
		if (ર != null)
		{
			ર(objCopy, new EventArgs());
		}
	}

	public object Clone()
	{
		return Copy();
	}

	public virtual void Refresh()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		MethodInvoker val = null;
		if (_0AC7 || (!m_Visible && !ઽ) || !m_Displayed)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		if (val2 == null || !IsHandleValid(val2) || val2 is ᆓ)
		{
			return;
		}
		if (val2.InvokeRequired)
		{
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					Refresh();
				};
			}
			val2.BeginInvoke((Delegate)(object)val);
			return;
		}
		if (m_NeedRecalcSize)
		{
			if (m_Parent is ItemContainer)
			{
				m_Parent.RecalcSize();
			}
			else
			{
				RecalcSize();
				if (m_Parent != null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
			}
		}
		Invalidate(val2);
	}

	protected virtual void Update()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			Update(val);
		}
	}

	protected virtual void Update(Control containerControl)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		if (containerControl.InvokeRequired)
		{
			containerControl.BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				containerControl.Update();
			});
		}
		else
		{
			containerControl.Update();
		}
	}

	protected virtual void Invalidate()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			Invalidate(val);
		}
	}

	private static void InvalidateFormCaption(Control containerControl)
	{
		int num = SystemInformation.Border3DSize.Height + SystemInformation.CaptionHeight;
		_0EFE.RECT lprcUpdate = new _0EFE.RECT(0, -num, containerControl.Width, num);
		_0EFE.RedrawWindow(containerControl.Handle, ref lprcUpdate, IntPtr.Zero, (_0EFE.RedrawWindowFlags)1281u);
	}

	protected virtual void Invalidate(Control containerControl)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		MethodInvoker val = null;
		MethodInvoker val2 = null;
		if (DesignMode)
		{
			if (containerControl is Form && !(containerControl is RadialMenuPopup))
			{
				if (IsHandleValid(containerControl))
				{
					InvalidateFormCaption(containerControl);
				}
				containerControl.Refresh();
			}
			else if (containerControl.InvokeRequired)
			{
				Control obj = containerControl;
				if (val == null)
				{
					val = (MethodInvoker)delegate
					{
						containerControl.Invalidate();
					};
				}
				obj.BeginInvoke((Delegate)(object)val);
			}
			else
			{
				containerControl.Invalidate();
			}
		}
		else if (containerControl is Form && !(containerControl is RadialMenuPopup))
		{
			if (IsHandleValid(containerControl))
			{
				InvalidateFormCaption(containerControl);
			}
		}
		else if (containerControl.InvokeRequired)
		{
			Control obj2 = containerControl;
			if (val2 == null)
			{
				val2 = (MethodInvoker)delegate
				{
					containerControl.Invalidate(GetInvalidateBounds(), true);
				};
			}
			obj2.BeginInvoke((Delegate)(object)val2);
		}
		else
		{
			containerControl.Invalidate(GetInvalidateBounds(), true);
		}
	}

	protected virtual Rectangle GetInvalidateBounds()
	{
		return m_Rect;
	}

	protected internal void OnAppearanceChanged()
	{
		if (_0ADC || !DesignMode)
		{
			return;
		}
		_0ADC = true;
		try
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				BarFunctions.InvokeRecalcLayout(val, invalidate: true);
			}
		}
		finally
		{
			_0ADC = false;
		}
	}

	protected internal virtual void OnExpandChange()
	{
		HideToolTip();
		RaiseExpandChange(EventArgs.Empty);
	}

	protected virtual void RaiseExpandChange(EventArgs e)
	{
		if (ન != null)
		{
			ન(this, e);
		}
		IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
		if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
		{
			iOwnerItemEvents.InvokeExpandedChange(this, e);
		}
	}

	protected internal virtual void OnSubItemExpandChange(BaseItem item)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (m_Expanded && ((Site != null && !Site.DesignMode) || Site == null))
		{
			Expanded = false;
		}
		HideToolTip();
		if (m_Parent != null)
		{
			m_Parent = null;
		}
		if (GetOwner() is IOwner owner)
		{
			owner.RemoveShortcutsFromItem(this);
		}
		if (Site == null || (Site != null && !Site.DesignMode))
		{
			if (m_SubItems != null && m_SubItems.Count > 0)
			{
				ArrayList arrayList = new ArrayList(m_SubItems.Count);
				m_SubItems.CopyTo(arrayList);
				foreach (BaseItem item in arrayList)
				{
					if (item.Parent == this)
					{
						item.Dispose();
					}
				}
				m_SubItems.__0AF5();
			}
			m_SubItems = null;
		}
		if (Site == null || (Site != null && !Site.DesignMode))
		{
			if (_0AC0 != null)
			{
				_0AC0 = null;
			}
			m_ContainerControl = null;
		}
		if (_૦ != null)
		{
			Command = null;
		}
		if (ݶ != null)
		{
			ݶ.ವ -= TextMarkupLinkClick;
			ݶ = null;
		}
		_0AD6 = true;
		base.Dispose(disposing);
	}

	protected virtual void OnStyleChanged()
	{
	}

	protected virtual void OnTextChanged()
	{
		MarkupTextChanged();
		if (_0AB1 != null)
		{
			_0AB1(this, new EventArgs());
		}
		GetIOwnerItemEvents()?.InvokeItemTextChanged(this, new EventArgs());
		GenerateAccessibilityEvent((AccessibleEvents)32780);
		OnAppearanceChanged();
	}

	protected internal static bool IsHandleValid(Control objCtrl)
	{
		if (objCtrl != null && !objCtrl.Disposing && !objCtrl.IsDisposed)
		{
			return objCtrl.IsHandleCreated;
		}
		return false;
	}

	protected virtual void ResetHover()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (IsHandleValid(val))
		{
			_089F.TRACKMOUSEEVENT tme = default(_089F.TRACKMOUSEEVENT);
			tme.dwFlags = 1073741824u;
			tme.hwndTrack = val.Handle;
			tme.cbSize = Marshal.SizeOf((object)tme);
			_089F.TrackMouseEvent(ref tme);
			tme.dwFlags |= 1u;
			_089F.TrackMouseEvent(ref tme);
			val = null;
		}
	}

	protected internal virtual bool IsAnyOnHandle(IntPtr iHandle)
	{
		bool result = false;
		if (m_SubItems == null || m_SubItems.Count == 0)
		{
			return result;
		}
		BaseItem baseItem = m_SubItems[0];
		Control val = null;
		if (baseItem != null)
		{
			object containerControl = baseItem.ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
			if (val is MenuPanel && ((MenuPanel)(object)val).PopupMenu)
			{
				val = val.Parent;
			}
			if (val != null && val.Handle == iHandle)
			{
				return true;
			}
			baseItem = null;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			if (subItem.Expanded && subItem.IsAnyOnHandle(iHandle))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	protected virtual void OnTooltipChanged()
	{
	}

	internal void SetOrientation(eOrientation o)
	{
		if (m_Orientation == o)
		{
			return;
		}
		m_Orientation = o;
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				subItem.Orientation = m_Orientation;
			}
		}
		m_NeedRecalcSize = true;
	}

	protected internal void HideToolTip()
	{
		if (m_ToolTipWnd == null)
		{
			return;
		}
		Rectangle bounds = ((Control)m_ToolTipWnd).Bounds;
		bounds.Width += 5;
		bounds.Height += 6;
		OnTooltip(bShow: false);
		OnToolTipVisibleChanged(new EventArgs());
		try
		{
			if (m_ToolTipWnd != null)
			{
				((Control)m_ToolTipWnd).Hide();
				((Component)(object)m_ToolTipWnd).Dispose();
				m_ToolTipWnd = null;
			}
		}
		catch
		{
		}
		if (ContainerControl != null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Invalidate(val.RectangleToClient(bounds), false);
			}
		}
	}

	private void OnToolTipVisibleChanged(EventArgs eventArgs)
	{
		EventHandler eventHandler = ݫ;
		if (eventHandler != null)
		{
			ݫ(this, eventArgs);
		}
	}

	protected virtual void OnHotSubItemChanged(BaseItem newValue, BaseItem oldValue)
	{
	}

	public virtual void ShowToolTip()
	{
		if (_߈ || !m_Visible || !m_Displayed || m_Expanded)
		{
			return;
		}
		IOwner owner = GetOwner() as IOwner;
		if ((owner != null && !owner.ShowToolTips) || !ShowToolTips)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if ((val is Bar && !((Bar)(object)val).ShowToolTips) || (val is MenuPanel && !((MenuPanel)(object)val).ShowToolTips))
		{
			return;
		}
		OnTooltip(bShow: true);
		if (m_Tooltip != "")
		{
			if (m_ToolTipWnd == null)
			{
				m_ToolTipWnd = new ToolTip();
			}
			m_ToolTipWnd.Style = EffectiveStyle;
			m_ToolTipWnd.Text = m_Tooltip;
			if (owner != null && owner.ShowShortcutKeysInToolTips && _0AC0 != null && _0AC0.Count > 0)
			{
				ToolTip toolTipWnd = m_ToolTipWnd;
				toolTipWnd.Text = toolTipWnd.Text + " (" + GetTooltipShortcutString() + ")";
			}
			GetIOwnerItemEvents()?.InvokeToolTipShowing(this, new EventArgs());
			m_ToolTipWnd.ReferenceRectangle = ScreenRectangle;
			OnToolTipVisibleChanged(new EventArgs());
			m_ToolTipWnd.ShowToolTip();
		}
	}

	protected virtual string GetTooltipShortcutString()
	{
		return ShortcutString;
	}

	public virtual Point PointToScreen(Point clientPoint)
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return clientPoint;
		}
		return val.PointToScreen(clientPoint);
	}

	internal void SetIsOnCustomizeMenu(bool b)
	{
		if (ઽ != b)
		{
			ઽ = b;
			OnIsOnCustomizeMenuChanged();
		}
	}

	public override string ToString()
	{
		return m_Text;
	}

	internal void SetIsOnCustomizeDialog(bool b)
	{
		if (_0ABE != b)
		{
			_0ABE = b;
			OnIsOnCustomizeDialogChanged();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode(bool b)
	{
		if (_߈ == b)
		{
			return;
		}
		_߈ = b;
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				subItem.SetDesignMode(_߈);
			}
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			HotSubItem = null;
		}
		OnDesignModeChanged();
	}

	internal void SetSystemItem(bool b)
	{
		m_SystemItem = b;
	}

	internal void RefreshShortcutString()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		_0AC1 = "";
		if (_0AC0 != null && _0AC0.Count != 0)
		{
			KeysConverter val = new KeysConverter();
			_0AC1 = ((TypeConverter)(object)val).ConvertToString((object)(Keys)_0AC0[0]);
		}
	}

	public static void CollapseSubItems(BaseItem item)
	{
		if (item == null && item.SubItems.Count == 0)
		{
			return;
		}
		BaseItem[] array = new BaseItem[item.SubItems.Count];
		item.SubItems.CopyTo(array, 0);
		BaseItem[] array2 = array;
		foreach (BaseItem baseItem in array2)
		{
			if (baseItem.Expanded)
			{
				baseItem.Expanded = false;
			}
		}
	}

	public static void CollapseSubItemsTree(BaseItem item)
	{
		if (item == null && item.SubItems.Count == 0)
		{
			return;
		}
		BaseItem[] array = new BaseItem[item.SubItems.Count];
		item.SubItems.CopyTo(array, 0);
		BaseItem[] array2 = array;
		foreach (BaseItem baseItem in array2)
		{
			CollapseSubItemsTree(baseItem);
			if (baseItem.Expanded)
			{
				baseItem.Expanded = false;
			}
		}
	}

	public static void CollapseAll(BaseItem objItem)
	{
		if (objItem == null)
		{
			return;
		}
		do
		{
			object containerControl = objItem.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val is MenuPanel)
			{
				if (objItem.Parent != null)
				{
					objItem = objItem.Parent;
				}
				continue;
			}
			if (val is Bar)
			{
				if (((Bar)(object)val)._0B00 == null)
				{
					break;
				}
				objItem = ((Bar)(object)val).ParentItem;
				continue;
			}
			if (objItem is RadialMenuContainer && objItem.Parent != null)
			{
				break;
			}
			if (objItem.Parent != null)
			{
				objItem = objItem.Parent;
			}
		}
		while (objItem != null && objItem.Parent != null);
		if (objItem != null)
		{
			objItem.Expanded = false;
			objItem.AutoExpand = false;
			if (objItem.Parent != null)
			{
				objItem.Parent.AutoExpand = false;
			}
		}
	}

	public static bool IsOnPopup(BaseItem item)
	{
		if (item == null)
		{
			return false;
		}
		object containerControl = item.ContainerControl;
		if (containerControl == null)
		{
			return false;
		}
		if (containerControl is MenuPanel)
		{
			return true;
		}
		if (containerControl is Bar && ((Bar)containerControl).BarState == eBarState.Popup)
		{
			return true;
		}
		if (containerControl is RadialMenuPopup)
		{
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ષ;
	}

	protected virtual bool ShouldDrawInsertMarker()
	{
		IOwner owner = GetOwner() as IOwner;
		if (_0ACA != 0 && Visible && Displayed)
		{
			if (owner != null && !owner.DragInProgress)
			{
				return !DesignMode;
			}
			return false;
		}
		return false;
	}

	protected internal void DrawInsertMarker(Graphics g)
	{
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		if (!ShouldDrawInsertMarker())
		{
			return;
		}
		Color color = ColorScheme.GetColor("834DD5");
		Color color2 = ColorScheme.GetColor("CCCFF8");
		int num = 4;
		int num2 = 1;
		int num3 = 2;
		if (IsDesignMarkHorizontal)
		{
			Point point = new Point(m_Rect.X, m_Rect.Y + num3);
			Point point2 = new Point(m_Rect.X, m_Rect.Bottom - (num3 + 1));
			if (_0ACA == ᥡ.ᥢ)
			{
				point = new Point(m_Rect.Right - num * 2, m_Rect.Y + num3);
				point2 = new Point(m_Rect.Right - num * 2, m_Rect.Bottom - (num3 + 1));
			}
			SolidBrush val = new SolidBrush(color2);
			try
			{
				Pen val2 = new Pen(color, 1f);
				try
				{
					GraphicsPath val3 = new GraphicsPath();
					try
					{
						val3.AddLine(point.X, point.Y + num, point.X + num, point.Y);
						val3.AddLine(point.X + num * 2, point.Y + num, point.X + (num * 2 - (num - num2)), point.Y + num);
						val3.AddLine(point2.X + (num * 2 - (num - num2)), point2.Y - num, point2.X + num * 2, point2.Y - num);
						val3.AddLine(point2.X + num, point2.Y, point2.X, point2.Y - num);
						val3.AddLine(point2.X + (num - num2), point2.Y - num, point.X + (num - num2), point.Y + num);
						val3.CloseAllFigures();
						g.FillPath((Brush)(object)val, val3);
						g.DrawPath(val2, val3);
						return;
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Point point3 = new Point(m_Rect.X + num3, m_Rect.Y);
		Point point4 = new Point(m_Rect.Right - (num3 + 1), m_Rect.Y);
		if (_0ACA == ᥡ.ᥢ)
		{
			point3 = new Point(m_Rect.X + num3, m_Rect.Bottom - (num * 2 + 1));
			point4 = new Point(m_Rect.Right - (num3 + 1), m_Rect.Bottom - (num * 2 + 1));
		}
		SolidBrush val4 = new SolidBrush(color2);
		try
		{
			Pen val5 = new Pen(color, 1f);
			try
			{
				GraphicsPath val6 = new GraphicsPath();
				try
				{
					val6.AddLine(point3.X, point3.Y + num, point3.X + num, point3.Y);
					val6.AddLine(point3.X + num, point3.Y + (num - num2), point4.X - num, point4.Y + (num - num2));
					val6.AddLine(point4.X - num, point4.Y, point4.X, point4.Y + num);
					val6.AddLine(point4.X - num, point4.Y + num * 2, point4.X - num, point4.Y + (num * 2 - (num - num3)));
					val6.AddLine(point3.X + num, point3.Y + (num * 2 - (num - num3)), point3.X + num, point3.Y + num * 2);
					val6.CloseAllFigures();
					g.FillPath((Brush)(object)val4, val6);
					g.DrawPath(val5, val6);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	protected virtual void OnSiteChanged()
	{
	}

	protected virtual AccessibleObject CreateAccessibilityInstance()
	{
		if (_ૠ == null)
		{
			_ૠ = new ItemAccessibleObject(this);
		}
		return (AccessibleObject)(object)_ૠ;
	}

	internal void GenerateAccessibilityEvent(AccessibleEvents e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (ContainerControl is Bar && ((Bar)ContainerControl)._0B98 && ((Control)(Bar)ContainerControl).AccessibilityObject is Bar.BarAccessibleObject barAccessibleObject)
		{
			barAccessibleObject._၈(this, e);
		}
	}

	internal virtual void DoAccesibleDefaultAction()
	{
		if (VisibleSubItems > 0 && (IsOnMenu || IsOnMenuBar || _ૡ))
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

	protected virtual void OnDragStartPointChanged(Point oldValue, Point newValue)
	{
	}

	protected void InvokeDelayed(MethodInvoker method)
	{
		InvokeDelayed(method, 10);
	}

	protected void InvokeDelayed(MethodInvoker method, int delayInterval)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = method;
		val.Interval = delayInterval;
		val.Tick += DelayedInvokeTimerTick;
		val.Start();
	}

	private void DelayedInvokeTimerTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)sender;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		if (!IsDisposed)
		{
			val2.Invoke();
		}
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null && (!(_૦ is Command) || !((Command)_૦).IsSyncingCommand))
		{
			CommandManager._૭(this);
		}
	}

	internal void ExecuteCommandInternal()
	{
		ExecuteCommand();
	}

	protected virtual void OnCommandChanged()
	{
	}

	private void MarkupTextChanged()
	{
		if (ݶ != null)
		{
			ݶ.ವ -= TextMarkupLinkClick;
		}
		ݶ = null;
		if (IsMarkupSupported && ሒ.መ(ref m_Text))
		{
			ݶ = ሒ.ሕ(m_Text);
			if (ݶ != null)
			{
				ݶ.ವ += TextMarkupLinkClick;
			}
		}
	}

	protected virtual void TextMarkupLinkClick(object sender, EventArgs e)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void TextMarkupBodyMeasure(Size size, MarkupDrawContext d)
	{
		TextMarkupBody._0CC4();
		TextMarkupBody.Measure(size, d);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void TextMarkupBodyArrange(Rectangle r, MarkupDrawContext d)
	{
		TextMarkupBody._0CC6(r, d);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void TextMarkupBodyRender(MarkupDrawContext d)
	{
		TextMarkupBody.Render(d);
	}

	public void UpdateBindings()
	{
		if (_૩ != null)
		{
			BindingContext bindingContext = BindingContext;
			for (int i = 0; i < ((BaseCollection)_૩).Count; i++)
			{
				BindingContext.UpdateBinding(bindingContext, ((BindingsCollection)_૩)[i]);
			}
		}
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			subItem.UpdateBindings();
		}
	}

	public object Invoke(Delegate method, object[] args)
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.IsDisposed)
		{
			return null;
		}
		return val.Invoke(method, args);
	}

	public object Invoke(Delegate method)
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.IsDisposed)
		{
			return null;
		}
		return val.Invoke(method);
	}

	public object BeginInvoke(Delegate method)
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.IsDisposed)
		{
			return null;
		}
		return val.BeginInvoke(method);
	}

	int IComparable.CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		if (obj is BaseItem baseItem)
		{
			return string.Compare(Text, baseItem.Text);
		}
		throw new ArgumentException("Object is not a BaseItem");
	}
}
