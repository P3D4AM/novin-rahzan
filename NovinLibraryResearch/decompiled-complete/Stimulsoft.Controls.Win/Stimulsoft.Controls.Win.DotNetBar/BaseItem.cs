using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Events;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public abstract class BaseItem : Component, ICloneable, IBlock, ICommandSource, IComparable, IBindableComponent, IComponent, IDisposable
{
	public class ItemAccessibleObject : AccessibleObject
	{
		private BaseItem m_Owner;

		private bool m_Hot;

		internal BaseItem Owner => m_Owner;

		public override string Name
		{
			get
			{
				if (m_Owner == null)
				{
					return "";
				}
				if (m_Owner.AccessibleName != "")
				{
					return m_Owner.AccessibleName;
				}
				if (m_Owner.Text != null)
				{
					return m_Owner.Text.Replace("&", "");
				}
				return m_Owner.Tooltip;
			}
			set
			{
				m_Owner.AccessibleName = value;
			}
		}

		public override string Description
		{
			get
			{
				if (m_Owner == null)
				{
					return "";
				}
				if (m_Owner.AccessibleDescription != "")
				{
					return m_Owner.AccessibleDescription;
				}
				if (m_Owner.IsOnMenu)
				{
					return ((AccessibleObject)this).Name + " menu item";
				}
				if (m_Owner.IsOnMenuBar)
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
				if (m_Owner == null || !m_Owner.IsAccessible)
				{
					return (AccessibleRole)0;
				}
				if ((int)m_Owner.AccessibleRole != -1)
				{
					return m_Owner.AccessibleRole;
				}
				BaseItem baseItem = m_Owner;
				while (baseItem.Parent != null)
				{
					baseItem = baseItem.Parent;
				}
				if (!m_Owner.IsOnMenu && !m_Owner.IsOnMenuBar && !baseItem.IsOnMenuBar)
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
				if (m_Owner == null)
				{
					return (AccessibleStates)1;
				}
				AccessibleStates val = (AccessibleStates)0;
				if (!m_Owner.IsAccessible)
				{
					return (AccessibleStates)1;
				}
				if (!m_Owner.Displayed || !m_Owner.Visible)
				{
					val = (AccessibleStates)(val | 0x8000);
				}
				else
				{
					if (!m_Owner.GetEnabled())
					{
						val = (AccessibleStates)1;
						if (m_Owner.Expanded || m_Hot || (m_Owner is ButtonItem && ((ButtonItem)m_Owner).IsMouseOver))
						{
							val = (AccessibleStates)(val | 0x84);
						}
						return val;
					}
					val = (AccessibleStates)((!m_Owner.Expanded && !m_Hot && (!(m_Owner is ButtonItem) || !((ButtonItem)m_Owner).IsMouseOver)) ? (val | 0x100000) : (val | 0x84));
					if (m_Owner.ShowSubItems && m_Owner.SubItems.Count > 0)
					{
						val = (AccessibleStates)((!m_Owner.Expanded) ? (val | 0x400) : (val | 0x200));
					}
				}
				return val;
			}
		}

		public override AccessibleObject Parent
		{
			get
			{
				if (m_Owner == null)
				{
					return null;
				}
				if (m_Owner.Parent != null && (!(m_Owner.Parent is GenericItemContainer) || !((GenericItemContainer)m_Owner.Parent).SystemContainer))
				{
					return m_Owner.Parent.AccessibleObject;
				}
				object containerControl = m_Owner.ContainerControl;
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
				if (m_Owner == null)
				{
					return Rectangle.Empty;
				}
				object containerControl = m_Owner.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					return val.RectangleToScreen(m_Owner.DisplayRectangle);
				}
				return m_Owner.m_Rect;
			}
		}

		public override string DefaultAction
		{
			get
			{
				if (m_Owner.AccessibleDefaultActionDescription != "")
				{
					return m_Owner.AccessibleDefaultActionDescription;
				}
				return "Press";
			}
		}

		public override string KeyboardShortcut => m_Owner.ShortcutString;

		public ItemAccessibleObject(BaseItem owner)
		{
			m_Owner = owner;
			m_Owner.MouseEnter += OnMouseEnter;
			m_Owner.MouseLeave += OnMouseLeave;
		}

		protected virtual void OnMouseEnter(object sender, EventArgs e)
		{
			m_Hot = true;
		}

		protected virtual void OnMouseLeave(object sender, EventArgs e)
		{
			m_Hot = false;
		}

		public override int GetChildCount()
		{
			if (m_Owner == null)
			{
				return 0;
			}
			return m_Owner.SubItems.Count;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (m_Owner == null || iIndex < 0 || iIndex >= m_Owner.SubItems.Count)
			{
				return null;
			}
			return m_Owner.SubItems[iIndex].AccessibleObject;
		}

		public override void DoDefaultAction()
		{
			if (m_Owner != null)
			{
				object containerControl = m_Owner.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is MenuPanel && !(val is IAccessibilitySupport))
				{
					object containerControl2 = ((MenuPanel)(object)val).ParentItem.ContainerControl;
					val = (Control)((containerControl2 is Control) ? containerControl2 : null);
				}
				if (val is IAccessibilitySupport accessibilitySupport)
				{
					accessibilitySupport.DoDefaultActionItem = m_Owner;
					NativeFunctions.PostMessage(val.Handle, 1131, IntPtr.Zero, IntPtr.Zero);
				}
				((AccessibleObject)this).DoDefaultAction();
			}
		}

		public override AccessibleObject GetSelected()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Invalid comparison between Unknown and I4
			if (m_Owner == null)
			{
				return ((AccessibleObject)this).GetSelected();
			}
			foreach (BaseItem subItem in m_Owner.SubItems)
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
			if (m_Owner == null)
			{
				return ((AccessibleObject)this).HitTest(x, y);
			}
			Point point = new Point(x, y);
			foreach (BaseItem subItem in m_Owner.SubItems)
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
			if (m_Owner == null)
			{
				return ((AccessibleObject)this).Navigate(navdir);
			}
			BaseItem baseItem = null;
			if ((int)navdir == 2 || (int)navdir == 4)
			{
				if (m_Owner.Parent != null)
				{
					BaseItem parent = m_Owner.Parent;
					baseItem = GetFirstVisible(parent.SubItems, parent.SubItems.IndexOf(m_Owner) + 1);
				}
			}
			else if ((int)navdir == 5)
			{
				if (m_Owner != null && m_Owner.Parent != null)
				{
					int num = m_Owner.Parent.SubItems.IndexOf(m_Owner);
					if (num < m_Owner.Parent.SubItems.Count - 1)
					{
						baseItem = m_Owner.Parent.SubItems[num + 1];
					}
				}
			}
			else if ((int)navdir == 6)
			{
				if (m_Owner != null && m_Owner.Parent != null)
				{
					int num2 = m_Owner.Parent.SubItems.IndexOf(m_Owner);
					if (num2 > 0)
					{
						baseItem = m_Owner.Parent.SubItems[num2 - 1];
					}
				}
			}
			else if ((int)navdir == 7)
			{
				baseItem = GetFirstVisible(m_Owner.SubItems, 0);
			}
			else if ((int)navdir == 8)
			{
				baseItem = GetFirstVisibleReverse(m_Owner.SubItems, m_Owner.SubItems.Count - 1);
			}
			else if ((int)navdir == 1)
			{
				if (m_Owner != null && m_Owner.Parent.IsContainer && m_Owner.Parent.SystemItem)
				{
					object containerControl = m_Owner.Parent.ContainerControl;
					Control val = (Control)((containerControl is Control) ? containerControl : null);
					if (val != null)
					{
						return val.AccessibilityObject;
					}
				}
				if (m_Owner.Parent != null)
				{
					return m_Owner.Parent.AccessibleObject;
				}
			}
			else if ((int)navdir == 3)
			{
				BaseItem parent2 = m_Owner.Parent;
				baseItem = GetFirstVisibleReverse(m_Owner.SubItems, parent2.SubItems.IndexOf(m_Owner) - 1);
			}
			if (baseItem != null)
			{
				return baseItem.AccessibleObject;
			}
			return ((AccessibleObject)this).Navigate(navdir);
		}

		private BaseItem GetFirstVisible(SubItemsCollection col, int startIndex)
		{
			int count = col.Count;
			if (count == 0)
			{
				return null;
			}
			if (startIndex >= col.Count)
			{
				startIndex = col.Count - 1;
			}
			for (int i = startIndex; i < count; i++)
			{
				if (col[i].Visible)
				{
					return col[i];
				}
			}
			return null;
		}

		private BaseItem GetFirstVisibleReverse(SubItemsCollection col, int startIndex)
		{
			if (col.Count == 0)
			{
				return null;
			}
			if (startIndex >= col.Count)
			{
				startIndex = col.Count - 1;
			}
			for (int num = startIndex; num >= 0; num--)
			{
				if (col[num].Visible)
				{
					return col[num];
				}
			}
			return null;
		}
	}

	[CompilerGenerated]
	private MouseEventHandler m_MouseDown;

	[CompilerGenerated]
	private MouseEventHandler m_MouseUp;

	[CompilerGenerated]
	private MouseEventHandler m_MouseMove;

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

	private string m_GlobalName = "";

	protected string m_Category;

	protected eOrientation m_Orientation;

	protected bool m_SystemItem;

	protected eSupportedOrientation m_SupportedOrientation;

	protected bool m_RecalculatingSize;

	protected bool m_CheckMouseMovePressed;

	private bool m_CanCustomize;

	private int m_Id;

	private bool m_ShowSubItems;

	private bool m_Focused;

	private bool m_Stretch;

	internal Rectangle m_ViewRectangle;

	protected ToolTip m_ToolTipWnd;

	private static int s_IdCounter;

	private bool m_IsOnCustomizeMenu;

	private bool m_IsOnCustomizeDialog;

	protected char m_AccessKey;

	private bool m_DesignMode;

	private eItemAlignment m_ItemAlignment;

	private ShortcutsCollection m_Shortcuts;

	private string m_ShortcutString = "";

	private Point m_MouseDownPt;

	private object m_Owner;

	private bool m_ClickAutoRepeat;

	private int m_ClickRepeatInterval = 600;

	private bool m_AutoCollapseOnClick = true;

	private bool m_SuspendLayout;

	private bool m_IsRightToLeft;

	protected bool m_ThemeAware;

	private bool m_GlobalItem = true;

	private eDesignInsertPosition m_DesignInsertMarker;

	private Cursor m_Cursor;

	private Cursor m_ContainerCursor;

	protected bool m_ShouldSerialize = true;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool _IgnoreClick;

	protected bool m_AllowOnlyOneSubItemExpanded = true;

	private string m_AccessibleDefaultActionDescription = "";

	private AccessibleRole m_AccessibleRole = (AccessibleRole)(-1);

	private string m_AccessibleName = "";

	private string m_AccessibleDescription = "";

	private bool m_IsAccessible = true;

	private string m_OriginalBarName = "";

	private int m_OriginalPosition = -1;

	private string m_KeyTips = "";

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point DesignTimeMouseDownPoint = Point.Empty;

	internal bool RenderText = true;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MouseUpNotification;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MouseDownCapture;

	private bool m_UserCustomized;

	private bool m_IsDisposed;

	private bool _DesignTimeVisible = true;

	internal bool LockDisplayedChange;

	private bool m_Refreshing;

	private eDesignMarkerOrientation _DesignMarkerOrientation;

	private ItemAccessibleObject _ItemAccessibleObject;

	internal bool _AccessibleExpandAction;

	private eContainerState _ContainerState;

	private Point _DragStartPoint;

	private bool _AllowDrop = true;

	private Padding _Margin = new Padding(0);

	private ICommand _Command;

	private object _CommandParameter;

	private BodyElement m_TextMarkup;

	private BindingContext _BindingContext;

	private ControlBindingsCollection _DataBindings;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool ShowArrowIfNoSubItems { get; set; }

	internal bool DesignTimeVisible
	{
		get
		{
			return _DesignTimeVisible;
		}
		set
		{
			_DesignTimeVisible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether is item enabled.")]
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
					BarFunctions.SyncProperty(this, "Enabled");
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool WideBeginGroup { get; set; }

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
					BarFunctions.SyncProperty(this, "BeginGroup");
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual int WidthInternal
	{
		get
		{
			return m_Rect.Width;
		}
		set
		{
			m_Rect.Width = value;
			OnExternalSizeChange();
		}
	}

	protected internal virtual bool IsRightToLeft
	{
		get
		{
			return m_IsRightToLeft;
		}
		set
		{
			if (m_IsRightToLeft == value)
			{
				return;
			}
			m_IsRightToLeft = value;
			foreach (BaseItem subItem in SubItems)
			{
				subItem.IsRightToLeft = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether the item is visible or hidden.")]
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
					BarFunctions.SyncProperty(this, "Visible");
				}
				if (!m_Visible && m_Displayed && m_Parent != null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
				if (!m_IsOnCustomizeMenu)
				{
					Displayed = false;
				}
				OnVisibleChanged(value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	public virtual bool AutoCollapseOnClick
	{
		get
		{
			return m_AutoCollapseOnClick;
		}
		set
		{
			m_AutoCollapseOnClick = value;
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
					BarFunctions.SyncProperty(this, "AutoExpand");
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool Displayed
	{
		get
		{
			return m_Displayed;
		}
		set
		{
			if (m_Displayed != value && !LockDisplayedChange)
			{
				m_Displayed = value;
				OnDisplayedChanged();
			}
		}
	}

	internal Point MouseDownPt => m_MouseDownPt;

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
			return true;
		}
	}

	private BaseItem RootParentItem
	{
		get
		{
			BaseItem parent = m_Parent;
			while (parent != null && parent.Parent != null)
			{
				parent = parent.Parent;
			}
			return parent;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool Focused => m_Focused;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool IsRecalculatingSize => m_RecalculatingSize;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool SuspendLayout
	{
		get
		{
			return m_SuspendLayout;
		}
		set
		{
			m_SuspendLayout = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDisposed => m_IsDisposed;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

	[Browsable(false)]
	[Category("Design")]
	[Description("Specifies unique ID of the item.")]
	public long Id => m_Id;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Data")]
	[DefaultValue("")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	[Category("Appearance")]
	[Description("Determines the display of the item when shown.")]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
	public virtual bool ShowSubItems
	{
		get
		{
			return m_ShowSubItems;
		}
		set
		{
			if (m_ShowSubItems != value)
			{
				NeedRecalcSize = true;
				m_ShowSubItems = value;
				if (Displayed)
				{
					Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eItemAlignment.Near)]
	[Category("Appearance")]
	[Description("Determines alignment of the item inside the container.")]
	public virtual eItemAlignment ItemAlignment
	{
		get
		{
			return m_ItemAlignment;
		}
		set
		{
			if (m_ItemAlignment != value)
			{
				m_ItemAlignment = value;
				NeedRecalcSize = true;
				if (Displayed && Parent != null)
				{
					Parent.SubItemSizeChanged(this);
				}
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Category("Design")]
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
					BarFunctions.SyncProperty(this, "Description");
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	public virtual bool Stretch
	{
		get
		{
			return m_Stretch;
		}
		set
		{
			if (m_Stretch != value)
			{
				m_Stretch = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "Stretch");
				}
				NeedRecalcSize = true;
				if (Displayed && m_Parent != null && !m_SuspendLayout)
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
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[Localizable(true)]
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
				value = string.Empty;
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
				BarFunctions.SyncProperty(this, "Tooltip");
			}
			OnTooltipChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates item category used to group similar items at design-time.")]
	[Localizable(true)]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Design")]
	[Description("Indicates the name used to identify item.")]
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

	[Browsable(true)]
	[Category("Design")]
	[Description("Indicates global name of the item that is used to synchronize the Global properties for the item across all instances with same global name.")]
	[DefaultValue("")]
	public virtual string GlobalName
	{
		get
		{
			return m_GlobalName;
		}
		set
		{
			m_GlobalName = value;
		}
	}

	protected virtual bool ShouldSyncProperties
	{
		get
		{
			if (GlobalItem && (GlobalName.Length > 0 || Name.Length > 0))
			{
				return !m_IsOnCustomizeDialog;
			}
			return false;
		}
	}

	[Browsable(false)]
	public eSupportedOrientation SupportedOrientation => m_SupportedOrientation;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
	public virtual bool GlobalItem
	{
		get
		{
			return m_GlobalItem;
		}
		set
		{
			m_GlobalItem = value;
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

	public virtual Rectangle ScreenRectangle
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
			return m_OriginalBarName;
		}
		set
		{
			m_OriginalBarName = value;
		}
	}

	internal int OriginalPosition
	{
		get
		{
			return m_OriginalPosition;
		}
		set
		{
			m_OriginalPosition = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool UserCustomized
	{
		get
		{
			return m_UserCustomized;
		}
		set
		{
			m_UserCustomized = value;
		}
	}

	[Browsable(false)]
	public ToolTip ToolTipControl => m_ToolTipWnd;

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
	public virtual string KeyTips
	{
		get
		{
			return m_KeyTips;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			m_KeyTips = value.ToUpper();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[DefaultValue("")]
	[Bindable(true)]
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
				m_AccessKey = NativeFunctions.GetAccessKey(m_Text);
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "Text");
				}
				NeedRecalcSize = true;
				if (Displayed && m_Parent != null && !m_SuspendLayout)
				{
					RecalcSize();
					m_Parent.SubItemSizeChanged(this);
				}
				OnTextChanged();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether item can be customized by user.")]
	public virtual bool CanCustomize
	{
		get
		{
			return m_CanCustomize;
		}
		set
		{
			m_CanCustomize = value;
		}
	}

	protected internal bool IsOnCustomizeMenu => m_IsOnCustomizeMenu;

	protected internal bool IsOnCustomizeDialog => m_IsOnCustomizeDialog;

	[Browsable(false)]
	public bool IsOnMenu
	{
		get
		{
			if (ContainerControl is MenuPanel || ContainerControl is ItemsListBox)
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
	public new virtual bool DesignMode => m_DesignMode;

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

	protected internal char AccessKey => m_AccessKey;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Design")]
	[Description("Indicates list of shortcut keys for this item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsDesigner, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public virtual ShortcutsCollection Shortcuts
	{
		get
		{
			if (m_Shortcuts == null)
			{
				m_Shortcuts = new ShortcutsCollection(this);
			}
			return m_Shortcuts;
		}
		set
		{
			IOwner owner = GetOwner() as IOwner;
			if (m_Shortcuts != null)
			{
				owner?.RemoveShortcutsFromItem(this);
			}
			m_Shortcuts = value;
			m_Shortcuts.Parent = this;
			if (m_Shortcuts != null)
			{
				owner?.AddShortcutsFromItem(this);
			}
		}
	}

	protected internal string ShortcutString => m_ShortcutString;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	public virtual bool ClickAutoRepeat
	{
		get
		{
			return m_ClickAutoRepeat;
		}
		set
		{
			m_ClickAutoRepeat = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(600)]
	[Category("Behavior")]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	public virtual int ClickRepeatInterval
	{
		get
		{
			return m_ClickRepeatInterval;
		}
		set
		{
			m_ClickRepeatInterval = value;
		}
	}

	internal eDesignInsertPosition DesignInsertMarker
	{
		get
		{
			return m_DesignInsertMarker;
		}
		set
		{
			if (m_DesignInsertMarker != value)
			{
				m_DesignInsertMarker = value;
				Refresh();
			}
		}
	}

	protected internal virtual eDesignMarkerOrientation DesignMarkerOrientation
	{
		get
		{
			return _DesignMarkerOrientation;
		}
		set
		{
			_DesignMarkerOrientation = value;
		}
	}

	internal bool IsDesignMarkHorizontal
	{
		get
		{
			if (_DesignMarkerOrientation != 0)
			{
				return _DesignMarkerOrientation == eDesignMarkerOrientation.Horizontal;
			}
			if (Parent is ItemContainer)
			{
				if (((ItemContainer)Parent).LayoutOrientation == eOrientation.Horizontal)
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
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies the mouse cursor displayed when mouse is over the item.")]
	public virtual Cursor Cursor
	{
		get
		{
			return m_Cursor;
		}
		set
		{
			if (!(m_Cursor != value))
			{
				return;
			}
			m_Cursor = value;
			if (!(m_Cursor != (Cursor)null) || !m_Visible || !m_Displayed)
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
					val.Cursor = m_Cursor;
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
			if (m_ThemeAware && BarFunctions.ThemedOS && Themes.ThemesActive)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool IsWindowed => false;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
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
	[Category("Accessibility")]
	[DefaultValue("")]
	[Description("Gets or sets the default action description of the control for use by accessibility client applications.")]
	public virtual string AccessibleDefaultActionDescription
	{
		get
		{
			return m_AccessibleDefaultActionDescription;
		}
		set
		{
			m_AccessibleDefaultActionDescription = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Accessibility")]
	[DefaultValue("")]
	[Description("Gets or sets the description of the control used by accessibility client applications.")]
	public virtual string AccessibleDescription
	{
		get
		{
			return m_AccessibleDescription;
		}
		set
		{
			if (m_AccessibleDescription != value)
			{
				m_AccessibleDescription = value;
				GenerateAccessibilityEvent((AccessibleEvents)32781);
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Accessibility")]
	[Description("Gets or sets the name of the control used by accessibility client applications.")]
	[DefaultValue("")]
	public virtual string AccessibleName
	{
		get
		{
			return m_AccessibleName;
		}
		set
		{
			if (m_AccessibleName != value)
			{
				m_AccessibleName = value;
				GenerateAccessibilityEvent((AccessibleEvents)32780);
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Accessibility")]
	[Description("Gets or sets the accessible role of the item.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public virtual AccessibleRole AccessibleRole
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_AccessibleRole;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_AccessibleRole = value;
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsAccessible
	{
		get
		{
			return m_IsAccessible;
		}
		set
		{
			m_IsAccessible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eContainerState ContainerState
	{
		get
		{
			return _ContainerState;
		}
		internal set
		{
			_ContainerState = value;
		}
	}

	internal Point DragStartPoint
	{
		get
		{
			return _DragStartPoint;
		}
		set
		{
			if (value != _DragStartPoint)
			{
				Point dragStartPoint = _DragStartPoint;
				_DragStartPoint = value;
				OnDragStartPointChanged(dragStartPoint, value);
			}
		}
	}

	internal bool AllowDrop
	{
		get
		{
			return _AllowDrop;
		}
		set
		{
			_AllowDrop = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Padding Margin
	{
		get
		{
			return _Margin;
		}
		set
		{
			_Margin = value;
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
				BarFunctions.SyncProperty(this, "Command");
			}
		}
	}

	ICommand ICommandSource.Command
	{
		get
		{
			return _Command;
		}
		set
		{
			bool flag = false;
			if (_Command != value)
			{
				flag = true;
			}
			if (_Command != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command);
			}
			_Command = value;
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
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public virtual object CommandParameter
	{
		get
		{
			return _CommandParameter;
		}
		set
		{
			_CommandParameter = value;
			if (ShouldSyncProperties)
			{
				BarFunctions.SyncProperty(this, "CommandParameter");
			}
		}
	}

	internal BodyElement TextMarkupBody => m_TextMarkup;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle TextMarkupBodyBounds
	{
		get
		{
			return TextMarkupBody.Bounds;
		}
		set
		{
			TextMarkupBody.Bounds = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool IsUsingTextMarkup => m_TextMarkup != null;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string PlainText
	{
		get
		{
			if (m_TextMarkup == null)
			{
				return Text;
			}
			return m_TextMarkup.PlainText;
		}
	}

	protected virtual bool IsMarkupSupported => false;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BindingContext BindingContext
	{
		get
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (_BindingContext != null)
			{
				return _BindingContext;
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
			_BindingContext = value;
			UpdateBindings();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Data")]
	[ParenthesizePropertyName(true)]
	[RefreshProperties(RefreshProperties.All)]
	public ControlBindingsCollection DataBindings
	{
		get
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			if (_DataBindings == null)
			{
				_DataBindings = new ControlBindingsCollection((IBindableComponent)(object)this);
			}
			return _DataBindings;
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
	public event EventHandler Click;

	[Description("Occurs when Item is double-clicked.")]
	public event EventHandler DoubleClick;

	[Description("Occurs when Item Expanded property has changed.")]
	public event EventHandler ExpandChange;

	[Description("Occurs when item loses input focus.")]
	public event EventHandler LostFocus;

	[Description("Occurs when item receives input focus.")]
	public event EventHandler GotFocus;

	[Description("Occurs when mouse button is pressed.")]
	public event MouseEventHandler MouseDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseDown, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse button is released.")]
	public event MouseEventHandler MouseUp
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseUp, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseUp, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse enters the item.")]
	public event EventHandler MouseEnter;

	[Description("Occurs when mouse leaves the item.")]
	public event EventHandler MouseLeave;

	[Description("Occurs when mouse moves over the item.")]
	public event MouseEventHandler MouseMove
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseMove, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_MouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MouseMove, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse remains still inside an item for an amount of time.")]
	public event EventHandler MouseHover;

	[Description("Occurs when copy of the item is made.")]
	public event EventHandler ItemCopied;

	[Description("Occurs when Text property of an Item has changed.")]
	public event EventHandler TextChanged;

	[Description("Occurs when Visible property of an Item has changed.")]
	public event EventHandler VisibleChanged;

	[Description("Occurs when Enabled property of an Item has changed.")]
	public event EventHandler EnabledChanged;

	[Description("Occurs when item's tooltip visibility has changed.")]
	public event EventHandler ToolTipVisibleChanged;

	[Description("Occurs when content of SubItems collection has changed")]
	public event CollectionChangeEventHandler SubItemsChanged;

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
			m_AccessKey = NativeFunctions.GetAccessKey(m_Text);
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
		m_ItemData = "";
		m_Enabled = true;
		m_BeginGroup = false;
		m_Style = eDotNetBarStyle.OfficeXP;
		m_Description = "";
		m_Tooltip = "";
		m_Name = sItemName;
		m_Category = "";
		m_Orientation = eOrientation.Horizontal;
		m_ToolTipWnd = null;
		m_Id = ++s_IdCounter;
		m_IsOnCustomizeMenu = false;
		m_IsOnCustomizeDialog = false;
		m_SystemItem = false;
		m_Shortcuts = null;
		m_DesignMode = false;
		m_CanCustomize = true;
		m_ShowSubItems = true;
		m_ItemAlignment = eItemAlignment.Near;
		m_Focused = false;
		m_Owner = null;
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
		if (name.Name != "Stimulsoft.Controls.Win.DotNetBar")
		{
			itemXmlElement.SetAttribute("assembly", name.Name + ", PublicKeyToken=" + Encoding.ASCII.GetString(name.GetPublicKey()));
		}
		else
		{
			itemXmlElement.SetAttribute("assembly", name.Name);
		}
		itemXmlElement.SetAttribute("class", type.FullName);
		itemXmlElement.SetAttribute("name", m_Name);
		itemXmlElement.SetAttribute("globalname", m_GlobalName);
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
		itemXmlElement.SetAttribute("cancustomize", XmlConvert.ToString(m_CanCustomize));
		itemXmlElement.SetAttribute("showsubitems", XmlConvert.ToString(m_ShowSubItems));
		itemXmlElement.SetAttribute("itemalignment", XmlConvert.ToString((int)m_ItemAlignment));
		itemXmlElement.SetAttribute("stretch", XmlConvert.ToString(m_Stretch));
		itemXmlElement.SetAttribute("global", XmlConvert.ToString(m_GlobalItem));
		itemXmlElement.SetAttribute("themes", XmlConvert.ToString(m_ThemeAware));
		if (m_AccessibleDefaultActionDescription != "")
		{
			itemXmlElement.SetAttribute("adefdesc", m_AccessibleDefaultActionDescription);
		}
		if (m_AccessibleDescription != "")
		{
			itemXmlElement.SetAttribute("adesc", m_AccessibleDescription);
		}
		if (m_AccessibleName != "")
		{
			itemXmlElement.SetAttribute("aname", m_AccessibleName);
		}
		if ((int)m_AccessibleRole != -1)
		{
			itemXmlElement.SetAttribute("arole", XmlConvert.ToString((int)m_AccessibleRole));
		}
		if (!m_AutoCollapseOnClick)
		{
			itemXmlElement.SetAttribute("autocollapse", XmlConvert.ToString(m_AutoCollapseOnClick));
		}
		if (m_ClickAutoRepeat)
		{
			itemXmlElement.SetAttribute("autorepeat", XmlConvert.ToString(m_ClickAutoRepeat));
		}
		if (m_ClickRepeatInterval != 600)
		{
			itemXmlElement.SetAttribute("clickrepinterv", XmlConvert.ToString(m_ClickRepeatInterval));
		}
		if (m_Cursor != (Cursor)null)
		{
			if (m_Cursor == Cursors.Hand)
			{
				itemXmlElement.SetAttribute("cur", "4");
			}
			else if (m_Cursor == Cursors.AppStarting)
			{
				itemXmlElement.SetAttribute("cur", "1");
			}
			else if (m_Cursor == Cursors.Arrow)
			{
				itemXmlElement.SetAttribute("cur", "2");
			}
			else if (m_Cursor == Cursors.Cross)
			{
				itemXmlElement.SetAttribute("cur", "3");
			}
			else if (m_Cursor == Cursors.Help)
			{
				itemXmlElement.SetAttribute("cur", "5");
			}
			else if (m_Cursor == Cursors.HSplit)
			{
				itemXmlElement.SetAttribute("cur", "6");
			}
			else if (m_Cursor == Cursors.IBeam)
			{
				itemXmlElement.SetAttribute("cur", "7");
			}
			else if (m_Cursor == Cursors.No)
			{
				itemXmlElement.SetAttribute("cur", "8");
			}
			else if (m_Cursor == Cursors.NoMove2D)
			{
				itemXmlElement.SetAttribute("cur", "9");
			}
			else if (m_Cursor == Cursors.NoMoveHoriz)
			{
				itemXmlElement.SetAttribute("cur", "10");
			}
			else if (m_Cursor == Cursors.NoMoveVert)
			{
				itemXmlElement.SetAttribute("cur", "11");
			}
			else if (m_Cursor == Cursors.PanEast)
			{
				itemXmlElement.SetAttribute("cur", "12");
			}
			else if (m_Cursor == Cursors.PanNE)
			{
				itemXmlElement.SetAttribute("cur", "13");
			}
			else if (m_Cursor == Cursors.PanNorth)
			{
				itemXmlElement.SetAttribute("cur", "14");
			}
			else if (m_Cursor == Cursors.PanNW)
			{
				itemXmlElement.SetAttribute("cur", "15");
			}
			else if (m_Cursor == Cursors.PanSE)
			{
				itemXmlElement.SetAttribute("cur", "16");
			}
			else if (m_Cursor == Cursors.PanSouth)
			{
				itemXmlElement.SetAttribute("cur", "17");
			}
			else if (m_Cursor == Cursors.PanSW)
			{
				itemXmlElement.SetAttribute("cur", "18");
			}
			else if (m_Cursor == Cursors.PanWest)
			{
				itemXmlElement.SetAttribute("cur", "19");
			}
			else if (m_Cursor == Cursors.SizeAll)
			{
				itemXmlElement.SetAttribute("cur", "20");
			}
			else if (m_Cursor == Cursors.SizeNESW)
			{
				itemXmlElement.SetAttribute("cur", "21");
			}
			else if (m_Cursor == Cursors.SizeNS)
			{
				itemXmlElement.SetAttribute("cur", "22");
			}
			else if (m_Cursor == Cursors.SizeNWSE)
			{
				itemXmlElement.SetAttribute("cur", "23");
			}
			else if (m_Cursor == Cursors.SizeWE)
			{
				itemXmlElement.SetAttribute("cur", "24");
			}
			else if (m_Cursor == Cursors.UpArrow)
			{
				itemXmlElement.SetAttribute("cur", "25");
			}
			else if (m_Cursor == Cursors.VSplit)
			{
				itemXmlElement.SetAttribute("cur", "26");
			}
			else if (m_Cursor == Cursors.WaitCursor)
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
		if (m_Shortcuts != null && m_Shortcuts.Count > 0)
		{
			itemXmlElement.SetAttribute("shortcuts", m_Shortcuts.ToString(","));
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
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		XmlElement itemXmlElement = context.ItemXmlElement;
		m_Name = itemXmlElement.GetAttribute("name");
		if (itemXmlElement.HasAttribute("globalname"))
		{
			m_GlobalName = itemXmlElement.GetAttribute("globalname");
		}
		m_Text = itemXmlElement.GetAttribute("text");
		m_AccessKey = NativeFunctions.GetAccessKey(m_Text);
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
			m_ItemData = "";
		}
		m_Enabled = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("enabled"));
		m_BeginGroup = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("begingroup"));
		m_Style = (eDotNetBarStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("style"));
		m_Description = itemXmlElement.GetAttribute("desc");
		m_Tooltip = itemXmlElement.GetAttribute("tooltip");
		m_Category = itemXmlElement.GetAttribute("category");
		m_CanCustomize = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("cancustomize"));
		m_ShowSubItems = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("showsubitems"));
		m_ItemAlignment = (eItemAlignment)XmlConvert.ToInt32(itemXmlElement.GetAttribute("itemalignment"));
		if (itemXmlElement.HasAttribute("stretch"))
		{
			m_Stretch = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("stretch"));
		}
		else
		{
			m_Stretch = false;
		}
		if (itemXmlElement.HasAttribute("global"))
		{
			m_GlobalItem = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("global"));
		}
		if (itemXmlElement.HasAttribute("themes"))
		{
			m_ThemeAware = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("themes"));
		}
		if (itemXmlElement.HasAttribute("adefdesc"))
		{
			m_AccessibleDefaultActionDescription = itemXmlElement.GetAttribute("adefdesc");
		}
		if (itemXmlElement.HasAttribute("adesc"))
		{
			m_AccessibleDescription = itemXmlElement.GetAttribute("adesc");
		}
		if (itemXmlElement.HasAttribute("aname"))
		{
			m_AccessibleName = itemXmlElement.GetAttribute("aname");
		}
		if (itemXmlElement.HasAttribute("arole"))
		{
			m_AccessibleRole = (AccessibleRole)XmlConvert.ToInt32(itemXmlElement.GetAttribute("arole"));
		}
		if (itemXmlElement.HasAttribute("autocollapse"))
		{
			m_AutoCollapseOnClick = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("autocollapse"));
		}
		else
		{
			m_AutoCollapseOnClick = true;
		}
		if (itemXmlElement.HasAttribute("autorepeat"))
		{
			m_ClickAutoRepeat = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("autorepeat"));
		}
		else
		{
			m_ClickAutoRepeat = false;
		}
		if (itemXmlElement.HasAttribute("clickrepinterv"))
		{
			m_ClickRepeatInterval = XmlConvert.ToInt32(itemXmlElement.GetAttribute("clickrepinterv"));
		}
		else
		{
			m_ClickRepeatInterval = 600;
		}
		if (itemXmlElement.HasAttribute("cur"))
		{
			switch (itemXmlElement.GetAttribute("cur"))
			{
			case "4":
				m_Cursor = Cursors.Hand;
				break;
			case "1":
				m_Cursor = Cursors.AppStarting;
				break;
			case "2":
				m_Cursor = Cursors.Arrow;
				break;
			case "3":
				m_Cursor = Cursors.Cross;
				break;
			case "5":
				m_Cursor = Cursors.Help;
				break;
			case "6":
				m_Cursor = Cursors.HSplit;
				break;
			case "7":
				m_Cursor = Cursors.IBeam;
				break;
			case "8":
				m_Cursor = Cursors.No;
				break;
			case "9":
				m_Cursor = Cursors.NoMove2D;
				break;
			case "10":
				m_Cursor = Cursors.NoMoveHoriz;
				break;
			case "11":
				m_Cursor = Cursors.NoMoveVert;
				break;
			case "12":
				m_Cursor = Cursors.PanEast;
				break;
			case "13":
				m_Cursor = Cursors.PanNE;
				break;
			case "14":
				m_Cursor = Cursors.PanNorth;
				break;
			case "15":
				m_Cursor = Cursors.PanNW;
				break;
			case "16":
				m_Cursor = Cursors.PanSE;
				break;
			case "17":
				m_Cursor = Cursors.PanSouth;
				break;
			case "18":
				m_Cursor = Cursors.PanSW;
				break;
			case "19":
				m_Cursor = Cursors.PanWest;
				break;
			case "20":
				m_Cursor = Cursors.SizeAll;
				break;
			case "21":
				m_Cursor = Cursors.SizeNESW;
				break;
			case "22":
				m_Cursor = Cursors.SizeNS;
				break;
			case "23":
				m_Cursor = Cursors.SizeNWSE;
				break;
			case "24":
				m_Cursor = Cursors.SizeWE;
				break;
			case "25":
				m_Cursor = Cursors.UpArrow;
				break;
			case "26":
				m_Cursor = Cursors.VSplit;
				break;
			case "27":
				m_Cursor = Cursors.WaitCursor;
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
			if (m_Shortcuts == null)
			{
				m_Shortcuts = new ShortcutsCollection(this);
			}
			m_Shortcuts.FromString(itemXmlElement.GetAttribute("shortcuts"), ",");
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
		if (this.VisibleChanged != null)
		{
			this.VisibleChanged(this, new EventArgs());
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
		if (!m_Visible && !m_IsOnCustomizeMenu)
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
		m_ViewRectangle = r;
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
		this.SubItemsChanged?.Invoke(this, e);
	}

	protected internal virtual void OnItemAdded(BaseItem item)
	{
		if (this.SubItemsChanged != null)
		{
			OnSubItemsChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
		}
		GetIOwnerItemEvents()?.InvokeItemAdded(item, new EventArgs());
	}

	protected internal virtual void OnAfterItemRemoved(BaseItem item)
	{
		if (item != null)
		{
			if (this.SubItemsChanged != null)
			{
				OnSubItemsChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
			}
			GetIOwnerItemEvents()?.InvokeItemRemoved(item, this);
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
		if (this.EnabledChanged != null)
		{
			this.EnabledChanged(this, new EventArgs());
		}
	}

	protected virtual void OnTopLocationChanged(int oldValue)
	{
	}

	protected virtual void OnLeftLocationChanged(int oldValue)
	{
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseEnter()
	{
		if (!m_DesignMode)
		{
			if (this.MouseEnter != null)
			{
				this.MouseEnter(this, new EventArgs());
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
		if (m_Cursor != (Cursor)null && val != null)
		{
			m_ContainerCursor = val.Cursor;
			val.Cursor = m_Cursor;
		}
		if ((BarUtilities.AlwaysGenerateAccessibilityFocusEvent && !(val is MenuPanel)) || (val is Bar && IsOnMenuBar))
		{
			if (val.AccessibilityObject is Bar.BarAccessibleObject barAccessibleObject)
			{
				barAccessibleObject.GenerateEvent(this, (AccessibleEvents)32773);
			}
			else if (val.AccessibilityObject is ItemControlAccessibleObject itemControlAccessibleObject)
			{
				itemControlAccessibleObject.GenerateEvent(this, (AccessibleEvents)32773);
			}
		}
		else if (val is MenuPanel && !((Control)(MenuPanel)(object)val).IsDisposed && val.AccessibilityObject is MenuPanel.PopupMenuAccessibleObject popupMenuAccessibleObject)
		{
			popupMenuAccessibleObject.GenerateEvent(this, (AccessibleEvents)32773);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseHover()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (m_DesignMode)
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
		if (!m_DesignMode)
		{
			if (this.MouseHover != null)
			{
				this.MouseHover(this, new EventArgs());
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
		if (m_ContainerCursor != (Cursor)null)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Cursor = m_ContainerCursor;
			}
			m_ContainerCursor = null;
		}
		if (m_DesignMode)
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
			if (m_TextMarkup != null)
			{
				BodyElement textMarkup = m_TextMarkup;
				object containerControl2 = ContainerControl;
				textMarkup.MouseLeave((Control)((containerControl2 is Control) ? containerControl2 : null));
			}
			if (this.MouseLeave != null)
			{
				this.MouseLeave(this, new EventArgs());
			}
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseLeave(this, new EventArgs());
			}
		}
		HideToolTip();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Invalid comparison between Unknown and I4
		IOwner owner = null;
		m_MouseDownPt = new Point(objArg.X, objArg.Y);
		BaseItem baseItem = ExpandedItem();
		if (baseItem != null && baseItem != m_HotSubItem && !DesignMode && m_AllowOnlyOneSubItemExpanded)
		{
			baseItem.Expanded = false;
			m_AutoExpand = false;
		}
		if (m_DesignMode && (m_CanCustomize || (Site != null && Site.DesignMode)))
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
		else if (!m_DesignMode)
		{
			if (m_TextMarkup != null)
			{
				BodyElement textMarkup = m_TextMarkup;
				object containerControl = ContainerControl;
				textMarkup.MouseDown((Control)((containerControl is Control) ? containerControl : null), objArg);
			}
			if (this.MouseDown != null)
			{
				this.MouseDown.Invoke((object)this, objArg);
			}
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseDown(this, objArg);
			}
		}
		HideToolTip();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Invalid comparison between Unknown and I4
		if (m_HotSubItem != null)
		{
			if (m_HotSubItem.DisplayRectangle.Contains(objArg.X, objArg.Y) || m_HotSubItem.MouseUpNotification)
			{
				m_HotSubItem.InternalMouseUp(objArg);
			}
		}
		else if (!m_DesignMode)
		{
			if (m_TextMarkup != null)
			{
				BodyElement textMarkup = m_TextMarkup;
				object containerControl = ContainerControl;
				textMarkup.MouseUp((Control)((containerControl is Control) ? containerControl : null), objArg);
			}
			if (this.MouseUp != null)
			{
				this.MouseUp.Invoke((object)this, objArg);
			}
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeMouseUp(this, objArg);
			}
			if (((int)objArg.Button == 1048576 || ((int)objArg.Button == 2097152 && IsOnContextMenu)) && DisplayRectangle.Contains(objArg.X, objArg.Y) && ((DisplayRectangle.Contains(m_MouseDownPt) && GetEnabled()) || IsOnMenu))
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
		m_MouseDownPt = Point.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalKeyDown(KeyEventArgs objArg)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		if (m_DesignMode)
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
				((ItemControl)GetOwner()).MenuFocus = false;
			}
		}
	}

	protected virtual void LeaveHotSubItem(BaseItem newMouseOverItem)
	{
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			if (newMouseOverItem != null && m_HotSubItem.Expanded && (IsOnMenu || (ContainerControl is Bar && ((Bar)ContainerControl).BarType == eBarType.MenuBar)))
			{
				m_HotSubItem.Expanded = false;
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
		if (m_DesignMode && (int)objArg.Button == 1048576 && (Math.Abs(objArg.X - m_MouseDownPt.X) >= 2 || Math.Abs(objArg.Y - m_MouseDownPt.Y) >= 2))
		{
			BaseItem baseItem = FocusedItem();
			if (baseItem != null && baseItem.CanCustomize)
			{
				if (!baseItem.SystemItem && GetOwner() is IOwner owner)
				{
					owner.StartItemDrag(baseItem);
				}
			}
			else if (!m_IsContainer && !SystemItem && m_CanCustomize && GetOwner() is IOwner owner2)
			{
				owner2.StartItemDrag(this);
			}
		}
		else if (m_IsContainer && !m_DesignMode)
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
				if (this.MouseMove != null)
				{
					this.MouseMove.Invoke((object)this, objArg);
				}
			}
			else if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseMove(objArg);
			}
			else
			{
				if (this.MouseMove != null)
				{
					this.MouseMove.Invoke((object)this, objArg);
				}
				IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
				if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
				{
					iOwnerItemEvents.InvokeMouseMove(this, objArg);
				}
			}
			if (m_TextMarkup != null)
			{
				Control parentControl = GetParentControl();
				if (parentControl != null)
				{
					m_TextMarkup.MouseMove(parentControl, objArg);
				}
			}
		}
		else
		{
			if (m_DesignMode)
			{
				return;
			}
			if (m_TextMarkup != null)
			{
				Control parentControl2 = GetParentControl();
				if (parentControl2 != null)
				{
					m_TextMarkup.MouseMove(parentControl2, objArg);
				}
			}
			if (this.MouseMove != null)
			{
				this.MouseMove.Invoke((object)this, objArg);
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalClick(mb, mpos);
		}
		else if (m_TextMarkup != null)
		{
			BodyElement textMarkup = m_TextMarkup;
			object containerControl = ContainerControl;
			textMarkup.Click((Control)((containerControl is Control) ? containerControl : null));
		}
	}

	public virtual void RaiseClick(eEventSource source)
	{
		RaiseClick(source, eShortcut.None);
	}

	public virtual void RaiseClick(eEventSource source, eShortcut key)
	{
		if (!CanRaiseClick)
		{
			return;
		}
		IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
		BaseItem rootParentItem = RootParentItem;
		if (m_AutoCollapseOnClick)
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
		OnClick();
		if (this.Click != null)
		{
			this.Click(this, new EventSourceArgs(source, key));
		}
		if (rootParentItem != null && rootParentItem.ContainerControl is Bar bar)
		{
			bar.InvokeItemClick(this, new EventSourceArgs(source, key));
		}
		if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
		{
			iOwnerItemEvents.InvokeItemClick(this);
		}
	}

	public virtual void RaiseClick()
	{
		RaiseClick(eEventSource.Code);
	}

	internal void ClearClick()
	{
		this.Click = null;
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
		if (this.DoubleClick != null)
		{
			this.DoubleClick(this, new EventArgs());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void OnGotFocus()
	{
		m_Focused = true;
		if (!m_DesignMode)
		{
			IOwnerItemEvents iOwnerItemEvents = GetIOwnerItemEvents();
			if (iOwnerItemEvents != null && !SystemItem && !IsOnCustomizeMenu && !IsOnCustomizeDialog)
			{
				iOwnerItemEvents.InvokeGotFocus(this, new EventArgs());
			}
		}
		Refresh();
		if (this.GotFocus != null)
		{
			this.GotFocus(this, new EventArgs());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void OnLostFocus()
	{
		if (IsDisposed)
		{
			return;
		}
		if (!m_DesignMode)
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
		m_Focused = false;
		Refresh();
		if (this.LostFocus != null)
		{
			this.LostFocus(this, new EventArgs());
		}
	}

	public virtual void Focus()
	{
		if (IsOnPopup(this) && ContainerControl is MenuPanel menuPanel && menuPanel.ParentItem.Parent == null)
		{
			menuPanel.SetFocusItem(this);
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
		if (!m_Focused)
		{
			return;
		}
		if (IsOnPopup(this) && ContainerControl is MenuPanel menuPanel && menuPanel.ParentItem.Parent == null)
		{
			menuPanel.SetFocusItem(null);
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
				if ((subItem.Visible || m_IsOnCustomizeMenu) && subItem.Displayed && subItem.DisplayRectangle.Contains(x, y) && (m_ViewRectangle.IsEmpty || m_ViewRectangle.Contains(x, y)))
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
		if (m_Owner != null)
		{
			return m_Owner;
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
		if (m_Owner != owner)
		{
			m_Owner = owner;
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
		objCopy.GlobalName = m_GlobalName;
		objCopy.GlobalItem = m_GlobalItem;
		objCopy.Style = m_Style;
		objCopy.SetDesignMode(m_DesignMode);
		objCopy.ShowSubItems = m_ShowSubItems;
		objCopy.Stretch = m_Stretch;
		if (m_Shortcuts != null && m_Shortcuts.Count > 0)
		{
			objCopy.Shortcuts.FromString(m_Shortcuts.ToString(","), ",");
		}
		objCopy.Click = this.Click;
		objCopy.ExpandChange = this.ExpandChange;
		objCopy.LostFocus = this.LostFocus;
		objCopy.GotFocus = this.GotFocus;
		objCopy.MouseDown = this.MouseDown;
		objCopy.MouseEnter = this.MouseEnter;
		objCopy.MouseHover = this.MouseHover;
		objCopy.MouseLeave = this.MouseLeave;
		objCopy.MouseMove = this.MouseMove;
		objCopy.MouseUp = this.MouseUp;
		objCopy.VisibleChanged = this.VisibleChanged;
		objCopy.EnabledChanged = this.EnabledChanged;
		objCopy.TextChanged = this.TextChanged;
		objCopy.IsRightToLeft = IsRightToLeft;
		objCopy.Cursor = m_Cursor;
		objCopy.ThemeAware = m_ThemeAware;
		objCopy.AutoCollapseOnClick = AutoCollapseOnClick;
		objCopy.Tag = m_ItemData;
		objCopy.CanCustomize = m_CanCustomize;
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
		if (this.ItemCopied != null)
		{
			this.ItemCopied(objCopy, new EventArgs());
		}
	}

	public object Clone()
	{
		return Copy();
	}

	public virtual void Refresh()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		if (m_SuspendLayout || (!m_Visible && !m_IsOnCustomizeMenu) || !m_Displayed)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || !IsHandleValid(val) || val is ItemsListBox)
		{
			return;
		}
		if (val.InvokeRequired)
		{
			val.BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				Refresh();
			});
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
		Invalidate(val);
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
		WinApi.RECT lprcUpdate = new WinApi.RECT(0, -num, containerControl.Width, num);
		WinApi.RedrawWindow(containerControl.Handle, ref lprcUpdate, IntPtr.Zero, (WinApi.RedrawWindowFlags)1281u);
	}

	protected virtual void Invalidate(Control containerControl)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		if (DesignMode)
		{
			if (containerControl is Form)
			{
				if (IsHandleValid(containerControl))
				{
					InvalidateFormCaption(containerControl);
				}
				containerControl.Refresh();
			}
			else if (containerControl.InvokeRequired)
			{
				containerControl.BeginInvoke((Delegate)(MethodInvoker)delegate
				{
					containerControl.Invalidate();
				});
			}
			else
			{
				containerControl.Invalidate();
			}
		}
		else if (containerControl is Form)
		{
			if (IsHandleValid(containerControl))
			{
				InvalidateFormCaption(containerControl);
			}
		}
		else if (containerControl.InvokeRequired)
		{
			containerControl.BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				containerControl.Invalidate(m_Rect, true);
			});
		}
		else
		{
			containerControl.Invalidate(m_Rect, true);
		}
	}

	protected internal void OnAppearanceChanged()
	{
		if (m_Refreshing || !DesignMode)
		{
			return;
		}
		m_Refreshing = true;
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
			m_Refreshing = false;
		}
	}

	protected internal virtual void OnExpandChange()
	{
		HideToolTip();
		RaiseExpandChange(EventArgs.Empty);
	}

	protected virtual void RaiseExpandChange(EventArgs e)
	{
		if (this.ExpandChange != null)
		{
			this.ExpandChange(this, e);
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
				m_SubItems._Clear();
			}
			m_SubItems = null;
		}
		if (Site == null || (Site != null && !Site.DesignMode))
		{
			if (m_Shortcuts != null)
			{
				m_Shortcuts = null;
			}
			m_ContainerControl = null;
		}
		if (_Command != null)
		{
			Command = null;
		}
		if (m_TextMarkup != null)
		{
			m_TextMarkup.HyperLinkClick -= TextMarkupLinkClick;
			m_TextMarkup = null;
		}
		m_IsDisposed = true;
		base.Dispose(disposing);
	}

	protected virtual void OnStyleChanged()
	{
	}

	protected virtual void OnTextChanged()
	{
		MarkupTextChanged();
		if (this.TextChanged != null)
		{
			this.TextChanged(this, new EventArgs());
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
			NativeFunctions.TRACKMOUSEEVENT tme = default(NativeFunctions.TRACKMOUSEEVENT);
			tme.dwFlags = 1073741824u;
			tme.hwndTrack = val.Handle;
			tme.cbSize = Marshal.SizeOf(tme);
			NativeFunctions.TrackMouseEvent(ref tme);
			tme.dwFlags |= 1u;
			NativeFunctions.TrackMouseEvent(ref tme);
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
		EventHandler toolTipVisibleChanged = this.ToolTipVisibleChanged;
		if (toolTipVisibleChanged != null)
		{
			this.ToolTipVisibleChanged(this, eventArgs);
		}
	}

	protected virtual void OnHotSubItemChanged(BaseItem newValue, BaseItem oldValue)
	{
	}

	public virtual void ShowToolTip()
	{
		if (m_DesignMode || !m_Visible || !m_Displayed || m_Expanded)
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
			if (owner != null && owner.ShowShortcutKeysInToolTips && m_Shortcuts != null && m_Shortcuts.Count > 0)
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

	internal void SetIsOnCustomizeMenu(bool b)
	{
		if (m_IsOnCustomizeMenu != b)
		{
			m_IsOnCustomizeMenu = b;
			OnIsOnCustomizeMenuChanged();
		}
	}

	public override string ToString()
	{
		return m_Text;
	}

	internal void SetIsOnCustomizeDialog(bool b)
	{
		if (m_IsOnCustomizeDialog != b)
		{
			m_IsOnCustomizeDialog = b;
			OnIsOnCustomizeDialogChanged();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode(bool b)
	{
		if (m_DesignMode == b)
		{
			return;
		}
		m_DesignMode = b;
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				subItem.SetDesignMode(m_DesignMode);
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
		m_ShortcutString = "";
		if (m_Shortcuts != null && m_Shortcuts.Count != 0)
		{
			KeysConverter val = new KeysConverter();
			m_ShortcutString = ((TypeConverter)(object)val).ConvertToString((object)(Keys)m_Shortcuts[0]);
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
			}
			else if (val is Bar)
			{
				if (((Bar)(object)val).ParentInternal == null)
				{
					break;
				}
				objItem = ((Bar)(object)val).ParentItem;
			}
			else if (objItem.Parent != null)
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
		return false;
	}

	public override int GetHashCode()
	{
		return m_Id;
	}

	protected internal void DrawInsertMarker(Graphics g)
	{
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		IOwner owner = GetOwner() as IOwner;
		if (m_DesignInsertMarker == eDesignInsertPosition.None || !Visible || !Displayed || ((owner == null || !owner.DragInProgress) && !DesignMode))
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
			if (m_DesignInsertMarker == eDesignInsertPosition.After)
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
		if (m_DesignInsertMarker == eDesignInsertPosition.After)
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
		if (_ItemAccessibleObject == null)
		{
			_ItemAccessibleObject = new ItemAccessibleObject(this);
		}
		return (AccessibleObject)(object)_ItemAccessibleObject;
	}

	internal void GenerateAccessibilityEvent(AccessibleEvents e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (ContainerControl is Bar && ((Bar)ContainerControl).m_AccessibleObjectCreated && ((Control)(Bar)ContainerControl).AccessibilityObject is Bar.BarAccessibleObject barAccessibleObject)
		{
			barAccessibleObject.GenerateEvent(this, e);
		}
	}

	internal virtual void DoAccesibleDefaultAction()
	{
		if (VisibleSubItems > 0 && (IsOnMenu || IsOnMenuBar || _AccessibleExpandAction))
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
			_AccessibleExpandAction = false;
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
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
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
		if (m_TextMarkup != null)
		{
			m_TextMarkup.HyperLinkClick -= TextMarkupLinkClick;
		}
		m_TextMarkup = null;
		if (IsMarkupSupported && MarkupParser.IsMarkup(ref m_Text))
		{
			m_TextMarkup = MarkupParser.Parse(m_Text);
			if (m_TextMarkup != null)
			{
				m_TextMarkup.HyperLinkClick += TextMarkupLinkClick;
			}
		}
	}

	protected virtual void TextMarkupLinkClick(object sender, EventArgs e)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void TextMarkupBodyMeasure(Size size, MarkupDrawContext d)
	{
		TextMarkupBody.InvalidateElementsSize();
		TextMarkupBody.Measure(size, d);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void TextMarkupBodyArrange(Rectangle r, MarkupDrawContext d)
	{
		TextMarkupBody.Arrange(r, d);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void TextMarkupBodyRender(MarkupDrawContext d)
	{
		TextMarkupBody.Render(d);
	}

	public void UpdateBindings()
	{
		if (_DataBindings != null)
		{
			BindingContext bindingContext = BindingContext;
			for (int i = 0; i < ((BaseCollection)_DataBindings).Count; i++)
			{
				BindingContext.UpdateBinding(bindingContext, ((BindingsCollection)_DataBindings)[i]);
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
		if (val.IsDisposed)
		{
			return null;
		}
		return val.Invoke(method, args);
	}

	public object Invoke(Delegate method)
	{
		try
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val.IsDisposed || IsDisposed)
			{
				return null;
			}
			return val.Invoke(method);
		}
		catch
		{
			return null;
		}
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
