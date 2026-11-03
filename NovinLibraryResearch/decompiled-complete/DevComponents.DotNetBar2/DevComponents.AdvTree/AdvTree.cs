using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.AdvTree.Display;
using DevComponents.AdvTree.Interop;
using DevComponents.AdvTree.Layout;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.ScrollBar;
using DevComponents.DotNetBar.Touch;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.AdvTree;

[ToolboxBitmap(typeof(ToolboxIconResFinder), "AdvTree.ico")]
[ComVisible(false)]
[ToolboxItem(true)]
[DefaultEvent("Click")]
[Designer("DevComponents.AdvTree.Design.AdvTreeDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class AdvTree : ScrollableControl, INodeNotify, ISupportInitialize
{
	private delegate void ᝇ();

	public class AdvTreeAccessibleObject : ControlAccessibleObject
	{
		private AdvTree __2487;

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
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				AccessibleStates result = (AccessibleStates)256;
				if (__2487 == null || ((Control)__2487).IsDisposed)
				{
					return (AccessibleStates)0;
				}
				if (((Control)__2487).Focused)
				{
					result = (AccessibleStates)4;
				}
				return result;
			}
		}

		public AdvTreeAccessibleObject(AdvTree owner)
			: base((Control)(object)owner)
		{
			__2487 = owner;
		}

		internal void _၈(Node _٠, AccessibleEvents ؿ)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			int num = __2487.Nodes.IndexOf(_٠);
			if (num >= 0 && __2487 != null && !((Control)__2487).IsDisposed)
			{
				((Control)__2487).AccessibilityNotifyClients(ؿ, num);
			}
		}

		public override int GetChildCount()
		{
			if (__2487 != null && !((Control)__2487).IsDisposed && __2487.Nodes != null)
			{
				return __2487.Nodes.Count;
			}
			return 0;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (__2487 != null && !((Control)__2487).IsDisposed && __2487.Nodes != null)
			{
				return __2487.Nodes[iIndex].AccessibleObject;
			}
			return null;
		}
	}

	private bool _Շ = true;

	private ElementStyleCollection Ո = new ElementStyleCollection();

	private ColumnHeaderCollection Չ = new ColumnHeaderCollection();

	private NodeCollection Ӥ = new NodeCollection();

	private Container Պ;

	private ElementStyle Ջ;

	private ElementStyle Ռ;

	private ElementStyle Ս;

	private ElementStyle Վ;

	private ElementStyle Տ;

	private ElementStyle Ր;

	private ElementStyle Ց;

	private ElementStyle Ւ;

	private ElementStyle Փ;

	private ElementStyle Ք;

	private ElementStyle Օ;

	private ElementStyle Ֆ;

	private Ԫ _0557;

	private NodeDisplay _0558;

	private HeadersCollection ՙ;

	private string _055A;

	private int _055B;

	private bool _055C;

	private bool _055D;

	private Node _055E;

	private SelectedNodesCollection _055F;

	private Node ՠ;

	private Cell ա;

	private int բ;

	private ImageList գ;

	private int դ;

	private bool ե;

	private NodeConnector զ;

	private NodeConnector է;

	private NodeConnector ը;

	private eCellLayout ӹ;

	private eCellPartLayout թ;

	private Cursor ժ;

	private Cursor ի;

	private ColorScheme լ;

	private bool խ;

	private bool ծ;

	private int կ;

	private Size հ;

	private Size ձ;

	private Color ղ;

	private eColorSchemePart ճ;

	private Color մ;

	private eColorSchemePart յ;

	private Color ն;

	private eColorSchemePart շ;

	private int ո;

	private Color չ;

	private eColorSchemePart պ;

	private Image ջ;

	private Image ռ;

	private eExpandButtonType ս;

	private Node վ;

	private int տ;

	private Color ր;

	private eColorSchemePart ց;

	private Color ւ;

	private eColorSchemePart փ;

	private Color ք;

	private eColorSchemePart օ;

	private int ֆ;

	private Color և;

	private eColorSchemePart ֈ;

	private Color _0589;

	private eColorSchemePart _058A;

	private Color _058B;

	private eColorSchemePart _058C;

	private int _058D;

	private bool _058E;

	private bool _058F;

	private Cell _0590;

	private _17C1 _0591;

	private bool ӽ;

	private Node _0592;

	private Point _0593;

	private eNodeLayout _0594;

	private int _0595;

	private int _0596;

	private eDiagramFlow _0597;

	private eMapFlow _0598;

	private ElementStyle _0599;

	private eNodeRenderMode ԁ;

	private TreeRenderer Ԃ;

	private ArrayList _059A;

	private float _059B;

	private object _059C;

	private VScrollBarAdv __059D;

	private HScrollBarAdv __059E;

	private Control __059F;

	private Ԥ __05A0;

	private ArrayList __05A1;

	private _070F __05A2;

	private PrepareCellEditorEventHandler _05A3;

	private MouseEventHandler _05A4;

	private MouseEventHandler _05A5;

	private AdvTreeCellEventHandler _05A6;

	private AdvTreeCellBeforeCheckEventHandler _05A7;

	private AdvTreeNodeEventHandler _05A8;

	private AdvTreeNodeCancelEventHandler _05A9;

	private AdvTreeNodeEventHandler _05AA;

	private AdvTreeNodeCancelEventHandler _05AB;

	private CommandButtonEventHandler _05AC;

	private CellEditEventHandler _05AD;

	private CellEditEventHandler _05AE;

	private CellEditEventHandler _05AF;

	private CellEditEventHandler _05B0;

	private EventHandler _05B1;

	private AdvTreeNodeCancelEventHandler _05B2;

	private AdvTreeNodeEventHandler _05B3;

	private AdvTreeNodeEventHandler _05B4;

	private TreeNodeCollectionEventHandler _05B5;

	private TreeNodeCollectionEventHandler _05B6;

	private TreeNodeCollectionEventHandler _05B7;

	private TreeNodeCollectionEventHandler _05B8;

	private EventHandler _05B9;

	private AdvTreeNodeCancelEventHandler _05BA;

	private TreeDragDropEventHandler _05BB;

	private TreeDragFeedbackEventHander _05BC;

	private TreeDragDropEventHandler _05BD;

	private TreeNodeMouseEventHandler Ӗ;

	private TreeNodeMouseEventHandler ӗ;

	private TreeNodeMouseEventHandler Ә;

	private TreeNodeMouseEventHandler ә;

	private TreeNodeMouseEventHandler Ӛ;

	private TreeNodeMouseEventHandler ӛ;

	private TreeNodeMouseEventHandler Ӝ;

	private TreeNodeMouseEventHandler ӝ;

	private SerializeNodeEventHandler _05BE;

	private SerializeNodeEventHandler _05BF;

	private MarkupLinkClickEventHandler Ӟ;

	private CustomCellEditorEventHandler _05C0;

	private EventHandler _05C1;

	private EventHandler _05C2;

	private TreeConvertEventHandler _05C3;

	private EventHandler _05C4;

	private EventHandler _05C5;

	private EventHandler _05C6;

	private DataNodeEventHandler _05C7;

	private DataNodeEventHandler _05C8;

	private EventHandler _05C9;

	private EventHandler _05CA;

	private EventHandler _05CB;

	private DataColumnEventHandler _05CC;

	private EventHandler _05CD;

	private EventHandler _05CE;

	private AdvTreeCellEventHandler _05CF;

	private AdvTreeCellEventHandler א;

	private ColumnMovedHandler ב;

	private AdvTreeRenderEventHandler ג;

	private bool _ד;

	private bool _ה;

	private bool _ו;

	private eScrollBarAppearance _ז;

	private Size _ח;

	private bool _ט;

	private bool _י;

	private bool _ך;

	private ContextMenuBar _כ;

	private bool _ל;

	private bool _ם;

	private bool _מ;

	private ColumnHeader _ן;

	private Cursor _נ;

	private int _ס;

	private ColumnHeader _ע;

	private int _ף;

	private bool _פ;

	private bool _ץ;

	private bool _צ;

	private Color _ק;

	private Color _ר;

	private ElementStyle _ש;

	private ElementStyle _ת;

	private bool __05EB;

	private eMultiSelectRule __05EC;

	private Image __05ED;

	private Image __05EE;

	private Image _ׯ;

	private bool _װ;

	private bool _ױ;

	private eSelectionStyle _ײ;

	private bool __05F3;

	private int __05F4;

	private bool __05F5;

	private int __05F6;

	private bool __05F7;

	private Timer __05F8;

	private int __05F9;

	private bool __05FA;

	private string __05FB;

	private ColumnHeader __05FC;

	private DateTime __05FD;

	private DateTime __05FE;

	private bool __05FF;

	private bool __0600;

	private ColumnHeader __0601;

	private ColumnHeaderCollection __0602;

	private static readonly string _0603 = "DotNetBar.";

	private bool __0604;

	private bool __0605;

	private ICellEditControl __0606;

	private bool __0607;

	private eView __0608;

	private static readonly Color _0609 = ColorScheme.GetColor(14869218);

	private Color __060A;

	private bool __060B;

	private List<Node> __060C;

	private bool __060D;

	private bool __060E;

	private bool __060F;

	private bool __0610;

	private PanelControl __0611;

	private bool __0612;

	private bool __0613;

	private bool __0614;

	private bool __0615;

	private bool __0616;

	private Size __0617;

	private Point __0618;

	private Point __0619;

	private bool __061A;

	private Timer __061B;

	private int __061C;

	private bool __061D;

	private bool __061E;

	private _27B3 __061F;

	internal static readonly int ؠ = 9;

	private int _ء;

	private bool _آ;

	private bool _أ;

	private bool _ؤ;

	private bool _إ;

	private List<BindingMemberInfo> _ئ;

	private object _ا;

	private bool _ب;

	private string _ة;

	private IFormatProvider _ت;

	private bool _ث;

	private Hashtable _ج;

	private bool _ح;

	private string _خ;

	private string _د;

	private ElementStyle _ذ;

	private CurrencyManager _ر;

	private bool _ز;

	private bool _س;

	private static TypeConverter ش;

	private Dictionary<string, PropertyDescriptor> _ص;

	private BindingMemberInfo _ض;

	private bool _ط;

	private TouchHandler _ظ;

	private bool _ع;

	private bool _غ;

	private Point _ػ;

	private Point _ؼ;

	private Rectangle _ؽ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool DisposeNodes
	{
		get
		{
			return _Շ;
		}
		set
		{
			_Շ = value;
		}
	}

	[DefaultValue(true)]
	public override bool AllowDrop
	{
		get
		{
			return ((Control)this).AllowDrop;
		}
		set
		{
			((Control)this).AllowDrop = value;
		}
	}

	[Description("Indicates whether sorting of node collection sorts child nodes as well. Default value is false.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(false)]
	public bool DeepSort
	{
		get
		{
			return _ה;
		}
		set
		{
			_ה = value;
		}
	}

	[Description("Indicates whether tooltips are shown when mouse is over the cell when Tooltip property is set.")]
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

	internal Ԥ Ԥ => __05A0;

	[Category("Appearance")]
	[Description("Gets or sets the scroll-bar visual style.")]
	[DefaultValue(eScrollBarAppearance.Default)]
	public eScrollBarAppearance ScrollBarAppearance
	{
		get
		{
			return _ז;
		}
		set
		{
			_ז = value;
			ف();
		}
	}

	[Category("Appearance")]
	[Description("Indicates size of the tile in Tile view.")]
	public Size TileSize
	{
		get
		{
			return _ח;
		}
		set
		{
			if (value != _ח)
			{
				Size ח = _ח;
				_ח = value;
				ق(ח, value);
			}
		}
	}

	[Description("Indicates whether the selected tree node remains highlighted even when the tree control has lost the focus.")]
	[Category("Selection")]
	[DefaultValue(false)]
	public bool HideSelection
	{
		get
		{
			return _ט;
		}
		set
		{
			_ט = value;
			if (SelectedNode != null && !IsKeyboardFocusWithin)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public bool IsKeyboardFocusWithin
	{
		get
		{
			return _י;
		}
		internal set
		{
			_י = value;
			OnIsKeyboardFocusWithinChanged();
		}
	}

	[Description("Indicates whether cell editing is completed when control loses input focus. Default value is true.")]
	[Category("Editing")]
	[DefaultValue(true)]
	public bool EndCellEditingOnLostFocus
	{
		get
		{
			return _ך;
		}
		set
		{
			_ך = value;
		}
	}

	internal ArrayList ԥ
	{
		get
		{
			return __05A1;
		}
		set
		{
			__05A1 = value;
		}
	}

	[DefaultValue(null)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ContextMenuBar ContextMenuBar
	{
		get
		{
			return _כ;
		}
		set
		{
			_כ = value;
		}
	}

	[Browsable(false)]
	[Category("Layout")]
	[DefaultValue(1f)]
	[Description("Indicates zoom factor for the control.")]
	public float Zoom
	{
		get
		{
			return _059B;
		}
		set
		{
			if (!((double)value <= 0.1))
			{
				_059B = value;
				RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	public Size TreeSize => new Size(_0557.ܐ, _0557._0711);

	[Browsable(false)]
	[DefaultValue(null)]
	[Category("Style")]
	[Description("Indicates render mode used to render the node.")]
	internal TreeRenderer Ԧ
	{
		get
		{
			return Ԃ;
		}
		set
		{
			Ԃ = value;
			_١();
		}
	}

	[Description("Indicates render mode used to render the node.")]
	[Browsable(true)]
	[Category("Style")]
	[DefaultValue(eNodeRenderMode.Default)]
	internal eNodeRenderMode ԧ
	{
		get
		{
			return ԁ;
		}
		set
		{
			ԁ = value;
			_١();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets the style for the background of the control.")]
	public ElementStyle BackgroundStyle => _0599;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int CellHorizontalSpacing
	{
		get
		{
			return __05A2._073C;
		}
		set
		{
			__05A2._073C = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether automatic drag and drop is enabled.")]
	[Category("Behavior")]
	[Browsable(true)]
	public bool DragDropEnabled
	{
		get
		{
			return ӽ;
		}
		set
		{
			ӽ = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting the tree.")]
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

	[Browsable(false)]
	[DefaultValue("\\")]
	[Description("Indicates the delimiter string that the tree node path uses.")]
	public string PathSeparator
	{
		get
		{
			return _055A;
		}
		set
		{
			_055A = value;
		}
	}

	[DefaultValue(true)]
	[Category("Columns")]
	[Description("Indicates whether user can resize the columns.")]
	public bool AllowUserToResizeColumns
	{
		get
		{
			return _ל;
		}
		set
		{
			_ל = value;
		}
	}

	[Description("Indicates whether user can reorder the columns.")]
	[Category("Columns")]
	[DefaultValue(false)]
	public bool AllowUserToReorderColumns
	{
		get
		{
			return _ם;
		}
		set
		{
			_ם = value;
		}
	}

	[Category("Columns")]
	[Description("Gets collection of column headers that appear in the tree.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColumnHeaderCollection Columns => Չ;

	[Description("Indicates whether column headers are visible if they are defined through Columns collection.")]
	[Category("Columns")]
	[DefaultValue(true)]
	public bool ColumnsVisible
	{
		get
		{
			return _מ;
		}
		set
		{
			if (_מ != value)
			{
				_מ = value;
				if (Չ.Count > 0)
				{
					RecalcLayout();
				}
			}
		}
	}

	private int Ԩ
	{
		get
		{
			return _ס;
		}
		set
		{
			if (_ן == null)
			{
				throw new ArgumentException("Column is not being reordered");
			}
			_ס = value;
			if (_ן.Parent == Չ)
			{
				__05A0.Ԩ = value;
			}
			else
			{
				((Control)this).Invalidate(_ן.Parent.Bounds);
			}
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyleCollection Styles => Ո;

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates default style for the node cell.")]
	[Category("Node Style")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle CellStyleDefault
	{
		get
		{
			return Ջ;
		}
		set
		{
			if (Ջ != value)
			{
				if (Ջ != null)
				{
					Ջ.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Ջ = value;
				ٴ();
			}
		}
	}

	[DefaultValue(null)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[Category("Node Style")]
	[Description("Indicates default style for the node cell when mouse is pressed.")]
	public ElementStyle CellStyleMouseDown
	{
		get
		{
			return Ռ;
		}
		set
		{
			if (Ռ != value)
			{
				if (Ռ != null)
				{
					Ռ.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Ռ = value;
				ٴ();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates default style for the node cell when mouse is over the cell.")]
	[Category("Node Style")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	public ElementStyle CellStyleMouseOver
	{
		get
		{
			return Ս;
		}
		set
		{
			if (Ս != value)
			{
				if (Ս != null)
				{
					Ս.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Ս = value;
				ٴ();
			}
		}
	}

	[Browsable(true)]
	[Category("Node Style")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates default style for the node cell when cell is selected.")]
	[DefaultValue(null)]
	public ElementStyle CellStyleSelected
	{
		get
		{
			return Վ;
		}
		set
		{
			if (Վ != value)
			{
				if (Վ != null)
				{
					Վ.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Վ = value;
				ٴ();
			}
		}
	}

	[DefaultValue(null)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Node Style")]
	[Description("Indicates default style for the node cell when cell is disabled.")]
	[Browsable(true)]
	public ElementStyle CellStyleDisabled
	{
		get
		{
			return Տ;
		}
		set
		{
			if (Տ != value)
			{
				if (Տ != null)
				{
					Տ.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Տ = value;
				ٴ();
			}
		}
	}

	[Browsable(true)]
	[Category("Node Style")]
	[Description("Gets or sets default style for the node cell when node that cell belongs to is expanded.")]
	[DefaultValue(null)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle NodeStyleExpanded
	{
		get
		{
			return Ր;
		}
		set
		{
			if (Ր != value)
			{
				if (Ր != null)
				{
					Ր.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Ր = value;
				ٴ();
			}
		}
	}

	[Description("Gets or sets default style for the node.")]
	[Category("Node Style")]
	[Browsable(true)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public ElementStyle NodeStyle
	{
		get
		{
			return Ց;
		}
		set
		{
			if (Ց != value)
			{
				if (Ց != null)
				{
					Ց.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Ց = value;
				ٴ();
			}
		}
	}

	[Description("Gets or sets style of the node when node is selected.")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Category("Node Style")]
	[Browsable(true)]
	public ElementStyle NodeStyleSelected
	{
		get
		{
			return Ւ;
		}
		set
		{
			if (Ւ != value)
			{
				if (Ւ != null)
				{
					Ւ.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Ւ = value;
				ٴ();
			}
		}
	}

	[Description("Gets or sets style of the node when mouse is over node.")]
	[Browsable(true)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Node Style")]
	[DefaultValue(null)]
	public ElementStyle NodeStyleMouseOver
	{
		get
		{
			return Փ;
		}
		set
		{
			if (Փ != value)
			{
				if (Փ != null)
				{
					Փ.StyleChanged -= _065F;
				}
				if (value != null)
				{
					value.StyleChanged += _065F;
				}
				Փ = value;
				ٴ();
			}
		}
	}

	[Description("Gets the collection of tree nodes that are assigned to the tree control.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public NodeCollection Nodes => Ӥ;

	[Category("Layout")]
	[DefaultValue(3)]
	[Description("Indicates vertical spacing between nodes in pixels.")]
	public int NodeSpacing
	{
		get
		{
			return _ף;
		}
		set
		{
			_ף = value;
			__05A2.NodeVerticalSpacing = value;
			InvalidateNodesSize();
			RecalcLayout();
		}
	}

	[Category("Layout")]
	[DefaultValue(4)]
	[Description("Indicates horizontal spacing between nodes in pixels when control is in Tile layout.")]
	public int NodeHorizontalSpacing
	{
		get
		{
			return __05A2.NodeHorizontalSpacing;
		}
		set
		{
			if (value != __05A2.NodeHorizontalSpacing)
			{
				__05A2.NodeHorizontalSpacing = value;
				InvalidateNodesSize();
				RecalcLayout();
			}
		}
	}

	[DefaultValue(false)]
	[Description("")]
	[Category("Columns")]
	public bool GridRowLines
	{
		get
		{
			return _פ;
		}
		set
		{
			if (_פ != value)
			{
				_פ = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether column can be resized when mouse is over the column grid line and outside of the column header.")]
	[Category("Columns")]
	public bool GridColumnLineResizeEnabled
	{
		get
		{
			return _ץ;
		}
		set
		{
			_ץ = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates whether grid lines are displayed when columns are defined.")]
	[DefaultValue(true)]
	public bool GridColumnLines
	{
		get
		{
			return _צ;
		}
		set
		{
			if (_צ != value)
			{
				_צ = value;
				if (Չ.Count > 0 && _מ)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates grid lines color.")]
	public Color GridLinesColor
	{
		get
		{
			return _ק;
		}
		set
		{
			_ק = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Columns")]
	[Description("Indicates alternate row color applied to every other row.")]
	public Color AlternateRowColor
	{
		get
		{
			return _ר;
		}
		set
		{
			_ר = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Category("Column Header Style")]
	[Description("Indicates the style class assigned to the child nodes columns.")]
	[Browsable(true)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle NodesColumnsBackgroundStyle
	{
		get
		{
			return _ש;
		}
		set
		{
			if (_ש != null)
			{
				_ש.StyleChanged -= _065F;
			}
			if (value != null)
			{
				value.StyleChanged += _065F;
			}
			_ש = value;
		}
	}

	[Browsable(true)]
	[Category("Column Header Style")]
	[Description("Indicates the style class assigned to the column.")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public ElementStyle ColumnsBackgroundStyle
	{
		get
		{
			return _ת;
		}
		set
		{
			if (_ת != null)
			{
				_ת.StyleChanged -= _065F;
			}
			if (value != null)
			{
				value.StyleChanged += _065F;
			}
			_ת = value;
		}
	}

	[DefaultValue(null)]
	[Category("Column Header Style")]
	[Browsable(true)]
	[Description("Indicates the style class assigned to the column.")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle ColumnStyleNormal
	{
		get
		{
			return Ք;
		}
		set
		{
			if (Ք != null)
			{
				Ք.StyleChanged -= _065F;
			}
			if (value != null)
			{
				value.StyleChanged += _065F;
			}
			Ք = value;
		}
	}

	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Column Header Style")]
	[Description("Indicates the style class assigned to the column when mouse is down.")]
	public ElementStyle ColumnStyleMouseDown
	{
		get
		{
			return Ֆ;
		}
		set
		{
			if (Ֆ != null)
			{
				Ֆ.StyleChanged -= _065F;
			}
			if (value != null)
			{
				value.StyleChanged += _065F;
			}
			Ֆ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Column Header Style")]
	[Description("Indicates the style class assigned to the cell when mouse is over the column.")]
	public ElementStyle ColumnStyleMouseOver
	{
		get
		{
			return Օ;
		}
		set
		{
			if (Օ != null)
			{
				Օ.StyleChanged -= _065F;
			}
			if (value != null)
			{
				value.StyleChanged += _065F;
			}
			Օ = value;
		}
	}

	[Description("Collection that holds definition of column headers associated with nodes.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	internal HeadersCollection ԩ => ՙ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Node SelectedNode
	{
		get
		{
			if (__05EB && _055F.Count > 0)
			{
				return _055F[_055F.Count - 1];
			}
			return _055E;
		}
		set
		{
			if (_055E != value)
			{
				SelectNode(value, eTreeAction.Code);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public NodeCollection SelectedNodes => _055F;

	[DefaultValue(false)]
	[Description("Indicates whether multi-node selection is enabled.")]
	[Category("Selection")]
	public bool MultiSelect
	{
		get
		{
			return __05EB;
		}
		set
		{
			if (__05EB != value)
			{
				if (__05EB)
				{
					_055F.Clear();
				}
				__05EB = value;
			}
		}
	}

	[Category("Selection")]
	[Description("Indicates rule that governs the multiple node selection.")]
	[DefaultValue(eMultiSelectRule.SameParent)]
	public eMultiSelectRule MultiSelectRule
	{
		get
		{
			return __05EC;
		}
		set
		{
			__05EC = value;
		}
	}

	internal Ԫ Ԫ => _0557;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public NodeDisplay NodeDisplay => _0558;

	[Browsable(false)]
	public bool IsUpdateSuspended => _055B > 0;

	[Browsable(false)]
	public bool IsLayoutPending => _055C;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool SuspendPaint
	{
		get
		{
			return _055D;
		}
		set
		{
			_055D = value;
		}
	}

	[Category("Images")]
	[Description("Indicates the ImageList that contains the Image objects used by the tree nodes.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public ImageList ImageList
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

	[TypeConverter(typeof(ImageIndexConverter))]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue(-1)]
	[Category("Images")]
	[Description("Indicates the image-list index value of the default image that is displayed by the tree nodes.")]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			if (դ != value)
			{
				դ = value;
			}
		}
	}

	[Category("CheckBox Images")]
	[DefaultValue(null)]
	[Description("Indicates custom image that is displayed instead default check box representation when check box in cell is checked")]
	public Image CheckBoxImageChecked
	{
		get
		{
			return __05ED;
		}
		set
		{
			__05ED = value;
			_0655();
		}
	}

	[Description("Indicates custom image that is displayed instead default check box representation when check box in cell is unchecked")]
	[DefaultValue(null)]
	[Category("CheckBox Images")]
	public Image CheckBoxImageUnChecked
	{
		get
		{
			return __05EE;
		}
		set
		{
			__05EE = value;
			_0655();
		}
	}

	[Description("Indicates custom image that is displayed instead default check box representation when check box in cell is in indeterminate state")]
	[Category("CheckBox Images")]
	[DefaultValue(null)]
	public Image CheckBoxImageIndeterminate
	{
		get
		{
			return _ׯ;
		}
		set
		{
			_ׯ = value;
			_0655();
		}
	}

	[Browsable(true)]
	[Category("Connectors")]
	[Description("Indicates the nested nodes connector.")]
	[Editor("DevComponents.AdvTree.Design.NodeConnectorTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public NodeConnector NodesConnector
	{
		get
		{
			return զ;
		}
		set
		{
			if (զ != value)
			{
				if (զ != null)
				{
					զ.AppearanceChanged -= _٣;
				}
				if (value != null)
				{
					value.AppearanceChanged += _٣;
				}
				զ = value;
				_١();
			}
		}
	}

	[Description("Indicates the linked nodes connector.")]
	[Category("Connectors")]
	[DefaultValue(null)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[Editor("DevComponents.AdvTree.Design.NodeConnectorTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public NodeConnector SelectedPathConnector
	{
		get
		{
			return ը;
		}
		set
		{
			if (ը != value)
			{
				if (ը != null)
				{
					ը.AppearanceChanged -= _٣;
				}
				if (value != null)
				{
					value.AppearanceChanged += _٣;
				}
				ը = value;
				_١();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eCellLayout.Default)]
	[Description("Indicates layout of the cells inside the node.")]
	[Category("Appearance")]
	public eCellLayout CellLayout
	{
		get
		{
			return ӹ;
		}
		set
		{
			ӹ = value;
			InvalidateNodesSize();
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[Description("Indicates layout of the cells inside the node.")]
	[Browsable(true)]
	[DefaultValue(eCellPartLayout.Default)]
	public eCellPartLayout CellPartLayout
	{
		get
		{
			return թ;
		}
		set
		{
			թ = value;
			InvalidateNodesSize();
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[Description("Indicates the color scheme style.")]
	[Browsable(true)]
	[DefaultValue(eColorSchemeStyle.Office2007)]
	public eColorSchemeStyle ColorSchemeStyle
	{
		get
		{
			return _0656(լ.ߕ);
		}
		set
		{
			լ.ߕ = _0656(value);
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	internal ColorScheme ԫ
	{
		get
		{
			if (ColorSchemeStyle == eColorSchemeStyle.Office2007 && GlobalManager.Renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			return լ;
		}
	}

	[Description("Indicates whether the content of the control is centered within the bounds of control.")]
	[Category("Layout")]
	[DefaultValue(true)]
	[Browsable(true)]
	internal bool Ԭ
	{
		get
		{
			return խ;
		}
		set
		{
			խ = value;
			_٢();
		}
	}

	[Description("Indicates whether selection is drawn for selected cell only instead of all cells inside of node")]
	[DefaultValue(false)]
	[Category("Selection")]
	public bool SelectionPerCell
	{
		get
		{
			return _װ;
		}
		set
		{
			if (_װ != value)
			{
				_װ = value;
				if (SelectedNode != null)
				{
					_0651(SelectedNode);
				}
			}
		}
	}

	[Category("Selection")]
	[Description("Indicates whether selection appearance changes depending on whether control has input focus.")]
	[DefaultValue(true)]
	public bool SelectionFocusAware
	{
		get
		{
			return _ױ;
		}
		set
		{
			if (_ױ != value)
			{
				_ױ = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Indicates node selection box style.")]
	[DefaultValue(eSelectionStyle.HighlightCells)]
	[Category("Selection")]
	public eSelectionStyle SelectionBoxStyle
	{
		get
		{
			return _ײ;
		}
		set
		{
			_ײ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Selection")]
	[Description("Indicates whether selection box is drawn around selected node.")]
	public bool SelectionBox
	{
		get
		{
			return ծ;
		}
		set
		{
			if (ծ != value)
			{
				ծ = value;
				ڏ();
			}
		}
	}

	[Browsable(true)]
	[Category("Selection")]
	[Description("Indicates the size in pixels of the selection box drawn around selected node.")]
	[DefaultValue(4)]
	internal int ԭ
	{
		get
		{
			return կ;
		}
		set
		{
			if (կ != value)
			{
				կ = value;
				ڏ();
			}
		}
	}

	[Category("Selection")]
	[Description("Indicates whether node is selected when mouse is pressed anywhere within node vertical bounds.")]
	[DefaultValue(true)]
	public bool FullRowSelect
	{
		get
		{
			return __05F3;
		}
		set
		{
			__05F3 = value;
		}
	}

	[Description("Indicates total node expand area width in pixels.")]
	[Category("Expand Button")]
	[DefaultValue(24)]
	public int ExpandWidth
	{
		get
		{
			return __05F4;
		}
		set
		{
			if (__05F4 != value)
			{
				__05F4 = value;
				__05A2.ExpandAreaWidth = value;
				InvalidateNodesSize();
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Expand Button")]
	[Description("Indicates size of the expand button that is used to expand/collapse node.")]
	[Browsable(true)]
	public Size ExpandButtonSize
	{
		get
		{
			return ձ;
		}
		set
		{
			if (ձ != value)
			{
				ձ = value;
				_065D();
			}
		}
	}

	[Description("Indicates expand button border color.")]
	[Browsable(true)]
	[Category("Expand Button")]
	public Color ExpandBorderColor
	{
		get
		{
			return ղ;
		}
		set
		{
			ղ = value;
			_065D();
		}
	}

	[Description("Indicates expand button border color.")]
	[DefaultValue(eColorSchemePart.None)]
	[Category("Expand Button")]
	[Browsable(true)]
	public eColorSchemePart ExpandBorderColorSchemePart
	{
		get
		{
			return ճ;
		}
		set
		{
			ճ = value;
			_065D();
		}
	}

	[Description("Indicates expand button back color.")]
	[Browsable(true)]
	[Category("Expand Button")]
	public Color ExpandBackColor
	{
		get
		{
			return մ;
		}
		set
		{
			մ = value;
			_065D();
		}
	}

	[Description("Indicates expand button back color.")]
	[DefaultValue(eColorSchemePart.None)]
	[Browsable(true)]
	[Category("Expand Button")]
	public eColorSchemePart ExpandBackColorSchemePart
	{
		get
		{
			return յ;
		}
		set
		{
			յ = value;
			_065D();
		}
	}

	[Description("Indicates expand button target gradient back color.")]
	[Category("Expand Button")]
	[Browsable(true)]
	public Color ExpandBackColor2
	{
		get
		{
			return ն;
		}
		set
		{
			ն = value;
			_065D();
		}
	}

	[Category("Expand Button")]
	[Description("Indicates expand button target gradient back color.")]
	[Browsable(true)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart ExpandBackColor2SchemePart
	{
		get
		{
			return շ;
		}
		set
		{
			շ = value;
			_065D();
		}
	}

	[Category("Expand Button")]
	[Browsable(true)]
	[Description("Indicates expand button line color.")]
	public Color ExpandLineColor
	{
		get
		{
			return չ;
		}
		set
		{
			չ = value;
			_065D();
		}
	}

	[Browsable(true)]
	[DefaultValue(eColorSchemePart.None)]
	[Category("Expand Button")]
	[Description("Indicates expand button line color.")]
	public eColorSchemePart ExpandLineColorSchemePart
	{
		get
		{
			return պ;
		}
		set
		{
			պ = value;
			_065D();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Expand Button")]
	[Description("Indicates expand button background gradient angle.")]
	public int ExpandBackColorGradientAngle
	{
		get
		{
			return ո;
		}
		set
		{
			if (ո != value)
			{
				ո = value;
				_065D();
			}
		}
	}

	[Description("Indicates expand button image which is used to indicate that node will be expanded.")]
	[DefaultValue(null)]
	[Category("Expand Button")]
	[Browsable(true)]
	public Image ExpandImage
	{
		get
		{
			return ջ;
		}
		set
		{
			if (ջ != value)
			{
				ջ = value;
				if (ս == eExpandButtonType.Image)
				{
					_065D();
				}
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Indicates expand button image which is used to indicate that node will be collapsed.")]
	[Category("Expand Button")]
	public Image ExpandImageCollapse
	{
		get
		{
			return ռ;
		}
		set
		{
			if (ռ != value)
			{
				ռ = value;
				if (ս == eExpandButtonType.Image)
				{
					_065D();
				}
			}
		}
	}

	[Description("Indicates type of the expand button used to expand/collapse nodes.")]
	[DefaultValue(eExpandButtonType.Rectangle)]
	[Category("Expand Button")]
	[Browsable(true)]
	public eExpandButtonType ExpandButtonType
	{
		get
		{
			return ս;
		}
		set
		{
			if (ս != value)
			{
				ս = value;
				_065D();
			}
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates display root node.")]
	[Browsable(true)]
	public Node DisplayRootNode
	{
		get
		{
			return վ;
		}
		set
		{
			if (վ != value)
			{
				վ = value;
				if (_0558 != null)
				{
					_0558.PaintedNodes.Clear();
				}
				InvalidateNodesSize();
				RecalcLayout();
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates width of the command button.")]
	[DefaultValue(10)]
	[Category("Command Button")]
	internal int Ԯ
	{
		get
		{
			return տ;
		}
		set
		{
			տ = value;
			ڐ();
		}
	}

	[Browsable(true)]
	[Category("Command Button")]
	[Description("Indicates command button back color.")]
	internal Color ԯ
	{
		get
		{
			return ր;
		}
		set
		{
			ր = value;
			ڐ();
		}
	}

	[Description("Indicates command button back color.")]
	[DefaultValue(eColorSchemePart.CustomizeBackground)]
	[Category("Command Button")]
	[Browsable(true)]
	internal eColorSchemePart _0530
	{
		get
		{
			return ց;
		}
		set
		{
			ց = value;
			ڐ();
		}
	}

	[Category("Command Button")]
	[Description("Indicates command button target gradient back color.")]
	[Browsable(true)]
	internal Color Ա
	{
		get
		{
			return ւ;
		}
		set
		{
			ւ = value;
			ڐ();
		}
	}

	[DefaultValue(eColorSchemePart.CustomizeBackground2)]
	[Category("Command Button")]
	[Description("Indicates command button target gradient back color.")]
	[Browsable(true)]
	internal eColorSchemePart Բ
	{
		get
		{
			return փ;
		}
		set
		{
			փ = value;
			ڐ();
		}
	}

	[Description("Indicates command button fore color.")]
	[Browsable(true)]
	[Category("Command Button")]
	internal Color Գ
	{
		get
		{
			return ք;
		}
		set
		{
			ք = value;
			ڐ();
		}
	}

	[Browsable(true)]
	[Category("Command Button")]
	[Description("Indicates command button foreground color.")]
	[DefaultValue(eColorSchemePart.CustomizeText)]
	internal eColorSchemePart Դ
	{
		get
		{
			return օ;
		}
		set
		{
			օ = value;
			ڐ();
		}
	}

	[Category("Expand Button")]
	[Description("Indicates command button background gradient angle.")]
	[DefaultValue(0)]
	[Browsable(true)]
	internal int Ե
	{
		get
		{
			return ֆ;
		}
		set
		{
			if (ֆ != value)
			{
				ֆ = value;
				_065D();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates command button mouse over back color.")]
	[Category("Command Button")]
	internal Color Զ
	{
		get
		{
			return և;
		}
		set
		{
			և = value;
			ڐ();
		}
	}

	[Category("Command Button")]
	[Description("Indicates command button mouse over back color.")]
	[Browsable(true)]
	[DefaultValue(eColorSchemePart.ItemHotBackground)]
	internal eColorSchemePart Է
	{
		get
		{
			return ֈ;
		}
		set
		{
			ֈ = value;
			ڐ();
		}
	}

	[Description("Indicates command button mouse over target gradient back color.")]
	[Browsable(true)]
	[Category("Command Button")]
	internal Color Ը
	{
		get
		{
			return _0589;
		}
		set
		{
			_0589 = value;
			ڐ();
		}
	}

	[Category("Command Button")]
	[Description("Indicates command button mouse over target gradient back color.")]
	[DefaultValue(eColorSchemePart.ItemPressedBackground2)]
	[Browsable(true)]
	internal eColorSchemePart Թ
	{
		get
		{
			return _058A;
		}
		set
		{
			_058A = value;
			ڐ();
		}
	}

	[Category("Command Button")]
	[Browsable(true)]
	[Description("Indicates command button mouse over fore color.")]
	internal Color Ժ
	{
		get
		{
			return _058B;
		}
		set
		{
			_058B = value;
			ڐ();
		}
	}

	[Description("Indicates command button mouse over foreground color.")]
	[DefaultValue(eColorSchemePart.ItemHotText)]
	[Category("Command Button")]
	[Browsable(true)]
	internal eColorSchemePart Ի
	{
		get
		{
			return _058C;
		}
		set
		{
			_058C = value;
			ڐ();
		}
	}

	[DefaultValue(0)]
	[Browsable(true)]
	[Description("Indicates command button mouse over background gradient angle.")]
	[Category("Expand Button")]
	internal int Լ
	{
		get
		{
			return _058D;
		}
		set
		{
			if (_058D != value)
			{
				_058D = value;
				_065D();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Editing")]
	[Description("Indicates whether the label text of the node cells can be edited.")]
	public bool CellEdit
	{
		get
		{
			return _058E;
		}
		set
		{
			_058E = value;
		}
	}

	[Browsable(false)]
	public bool IsCellEditing => _058F;

	protected override bool ScaleChildren => false;

	[Description("Indicates whether keyboard incremental search is enabled.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool KeyboardSearchEnabled
	{
		get
		{
			return __05F7;
		}
		set
		{
			__05F7 = value;
		}
	}

	[DefaultValue(1000)]
	[Category("Behavior")]
	[Description("Indicates keyboard search buffer expiration timeout.")]
	public int SearchBufferExpireTimeout
	{
		get
		{
			return __05F9;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			__05F9 = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether search buffer is auto-cleared regardless of the expiration timeout if node is not found using current buffer content.")]
	[DefaultValue(true)]
	public bool SearchBufferAutoClearNotFound
	{
		get
		{
			return __05FA;
		}
		set
		{
			__05FA = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether double-clicking the node will toggle its expanded state.")]
	public bool DoubleClickTogglesNode
	{
		get
		{
			return __0600;
		}
		set
		{
			__0600 = value;
		}
	}

	[Description("Indicates whether node is highlighted when mouse enters the node.")]
	[DefaultValue(false)]
	[Category("Appearance")]
	public bool HotTracking
	{
		get
		{
			return __0605;
		}
		set
		{
			__0605 = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public bool IsMovingColumn => __05FF;

	private bool Խ => ӛ != null;

	[Description("Indicates how control positions the items.")]
	[DefaultValue(eView.Tree)]
	[Category("Appearance")]
	public eView View
	{
		get
		{
			return __0608;
		}
		set
		{
			if (value != __0608)
			{
				eView ك = __0608;
				__0608 = value;
				ږ(ك, value);
			}
		}
	}

	[Description("Indicates color of the group divider line when in tile view.")]
	[Category("Columns")]
	public Color TileGroupLineColor
	{
		get
		{
			return __060A;
		}
		set
		{
			__060A = value;
			if (__0608 == eView.Tile)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public Node MouseOverNode => ՠ;

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Specifies the default mouse cursor displayed when mouse is over the cell.")]
	[Browsable(true)]
	public Cursor DefaultCellCursor
	{
		get
		{
			return ի;
		}
		set
		{
			if (ի != value)
			{
				ի = value;
			}
		}
	}

	internal ArrayList Ծ => _059A;

	[DefaultValue(true)]
	[Browsable(false)]
	public bool ClipHostedControls
	{
		get
		{
			return __060F;
		}
		set
		{
			__060F = value;
			ڠ();
		}
	}

	internal bool Կ => __0610;

	internal bool Հ
	{
		get
		{
			return __0612;
		}
		set
		{
			__0612 = value;
		}
	}

	[Browsable(false)]
	public VScrollBarAdv VScrollBar => __059D;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Vertical Scroll-bar is shown if needed because content of the control exceeds available height.")]
	public bool VScrollBarVisible
	{
		get
		{
			return __0614;
		}
		set
		{
			__0614 = value;
			if (!__0614 && __059D != null)
			{
				((Control)__059D).Visible = false;
			}
		}
	}

	[Description("Indicates whether Vertical Scroll-bar is shown if needed because content of the control exceeds available height.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool HScrollBarVisible
	{
		get
		{
			return __0615;
		}
		set
		{
			__0615 = value;
			if (!__0615 && __059E != null)
			{
				((Control)__059E).Visible = false;
			}
		}
	}

	[Browsable(false)]
	public HScrollBarAdv HScrollBar => __059E;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Size AutoScrollMargin
	{
		get
		{
			return ((ScrollableControl)this).AutoScrollMargin;
		}
		set
		{
			((ScrollableControl)this).AutoScrollMargin = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AutoScroll
	{
		get
		{
			return __0616;
		}
		set
		{
			if (__0616 != value)
			{
				__0616 = value;
				ڦ();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMinSize
	{
		get
		{
			return __0617;
		}
		set
		{
			__0617 = value;
			ڦ();
		}
	}

	[Description("Indicates location of the auto-scroll position.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Point AutoScrollPosition
	{
		get
		{
			return __0619;
		}
		set
		{
			if (!(__0619 != value))
			{
				return;
			}
			__0618 = new Point(value.X - __0619.X, value.Y - __0619.Y);
			__0619 = value;
			if (__0616)
			{
				if (__059D != null && __059D.Value != -__0619.Y)
				{
					__059D.Value = Math.Min(__059D.Maximum, Math.Max(__059D.Minimum, -__0619.Y));
				}
				if (__059E != null && __059E.Value != -__0619.X)
				{
					__059E.Value = Math.Min(__059E.Maximum, Math.Max(__059E.Minimum, -__0619.X));
				}
				ڡ(ڢ: false);
				((Control)this).Invalidate();
				_0558.Offset = Point.Empty;
				if (__05A0 != null)
				{
					((Control)__05A0).Invalidate();
				}
			}
		}
	}

	internal int Ձ
	{
		get
		{
			if (__05A0 == null)
			{
				return 0;
			}
			return ((Control)__05A0).Height;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public List<Node> CheckedNodes
	{
		get
		{
			List<Node> list = new List<Node>();
			NodeCollection nodes = Nodes;
			ڳ(list, nodes);
			return list;
		}
	}

	internal bool Ղ => _05BE != null;

	internal bool Ճ => _05BF != null;

	[DefaultValue(24)]
	[Description("Indicates offset in pixels from node's X position that is used during drag & drop operation to indicate that dragged node is dropped as child node of the parent's node.")]
	[Category("Behavior")]
	public int DropAsChildOffset
	{
		get
		{
			return __061C;
		}
		set
		{
			__061C = value;
		}
	}

	[Description("Indicates whether drag & drop internal implementation allows the copying of the node being dragged when CTRL key is pressed.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool DragDropNodeCopyEnabled
	{
		get
		{
			return __061D;
		}
		set
		{
			__061D = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether control accepts the dragged nodes from other AdvTree controls.")]
	public bool AllowExternalDrop
	{
		get
		{
			return __061E;
		}
		set
		{
			__061E = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates distance to indent each of the child tree node levels.")]
	[DefaultValue(16)]
	public int Indent
	{
		get
		{
			return _ء;
		}
		set
		{
			if (_ء != value)
			{
				_ء = value;
				if (_0557 is _2706)
				{
					((_2706)_0557).ܖ = value;
				}
				InvalidateNodesSize();
				RecalcLayout();
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether drop marker is displayed during drag-drop operation.")]
	public bool PaintDragDropInsertMarker
	{
		get
		{
			return _آ;
		}
		set
		{
			_آ = value;
			if (IsDragDropInProgress && __061F != null)
			{
				ۈ(__061F);
			}
		}
	}

	[Browsable(false)]
	public bool IsDragDropInProgress => _0592 != null;

	[DefaultValue(true)]
	[Description("Indicates whether number of nodes being dragged is displayed on drag node preview.")]
	[Category("Selection")]
	public bool MultiNodeDragCountVisible
	{
		get
		{
			return _أ;
		}
		set
		{
			_أ = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether multiple nodes drag & drop is enabled.")]
	[Category("Selection")]
	public bool MultiNodeDragDropAllowed
	{
		get
		{
			return _ؤ;
		}
		set
		{
			_ؤ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
		}
	}

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
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).BackgroundImageLayout = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool ResetAutoScrollPositionOnClearedNodes
	{
		get
		{
			return _إ;
		}
		set
		{
			_إ = value;
		}
	}

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates comma separated list of property or column names to display on popup tree control")]
	[Editor("DevComponents.DotNetBar.Design.DataMembersSelector, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string DisplayMembers
	{
		get
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (_ئ == null || _ئ.Count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < _ئ.Count; i++)
			{
				BindingMemberInfo val = _ئ[i];
				stringBuilder.Append(((BindingMemberInfo)(ref val)).BindingMember);
				if (i + 1 < _ئ.Count)
				{
					stringBuilder.Append(',');
				}
			}
			return stringBuilder.ToString();
		}
		set
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			List<BindingMemberInfo> ئ = _ئ;
			List<BindingMemberInfo> list = null;
			if (!string.IsNullOrEmpty(value))
			{
				list = new List<BindingMemberInfo>();
				string[] array = value.Split(new char[1] { ',' });
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(new BindingMemberInfo(array[i].Trim()));
				}
			}
			try
			{
				_06DA(_ا, list, _06DD: false);
			}
			catch
			{
				_ئ = ئ;
			}
		}
	}

	[RefreshProperties(RefreshProperties.Repaint)]
	[AttributeProvider(typeof(IListSource))]
	[Description("Indicates data source for the ComboTree.")]
	[DefaultValue(null)]
	[Category("Data")]
	public object DataSource
	{
		get
		{
			return _ا;
		}
		set
		{
			if (value != null && !(value is IList) && !(value is IListSource))
			{
				throw new ArgumentException("Data type is not supported for complex data binding");
			}
			ClearDescriptorsCache();
			try
			{
				_06DA(value, _ئ, _06DD: true);
			}
			catch
			{
				DisplayMembers = "";
			}
			if (value == null)
			{
				DisplayMembers = "";
			}
		}
	}

	[Description("Indicates whether formatting is applied to the DisplayMembers property of the control.")]
	[DefaultValue(false)]
	public bool FormattingEnabled
	{
		get
		{
			return _ب;
		}
		set
		{
			if (value != _ب)
			{
				_ب = value;
				RefreshItems();
				OnFormattingEnabledChanged(EventArgs.Empty);
			}
		}
	}

	[Description("Indicates format-specifier characters that indicate how a value is to be displayed.")]
	[MergableProperty(false)]
	[Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[DefaultValue("")]
	public string FormatString
	{
		get
		{
			return _ة;
		}
		set
		{
			if (value == null)
			{
				value = string.Empty;
			}
			if (!value.Equals(_ة))
			{
				_ة = value;
				RefreshItems();
				OnFormatStringChanged(EventArgs.Empty);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IFormatProvider FormatInfo
	{
		get
		{
			return _ت;
		}
		set
		{
			if (value != _ت)
			{
				_ت = value;
				RefreshItems();
				OnFormatInfoChanged(EventArgs.Empty);
			}
		}
	}

	[Description("Indicates comma separated field or property names that holds the value that is used to identify node and parent node. Format expected is: FieldNodeId,ParentNodeFieldId. For example if your table represents departments, you have DepartmentId field which uniquely identifies a department and ParentDepartmentId field which identifies parent of the department if any you would set this property to DepartmentId,ParentDepartmentId.")]
	[DefaultValue("")]
	[Category("Data")]
	public string ParentFieldNames
	{
		get
		{
			return _خ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (!string.IsNullOrEmpty(value))
			{
				string[] array = value.Split(new char[1] { ',' });
				if (array.Length != 2)
				{
					throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character.");
				}
				if (string.IsNullOrEmpty(array[0]))
				{
					throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. First field name is empty.");
				}
				if (string.IsNullOrEmpty(array[1]))
				{
					throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. Second field name is empty.");
				}
			}
			_خ = value;
			OnParentFieldNamesChanged();
		}
	}

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates comma separated list of field or property names that are used for grouping when data-binding is used")]
	public string GroupingMembers
	{
		get
		{
			return _د;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_د = value;
			OnGroupingMembersChanged();
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets default style for the node.")]
	[Category("Node Style")]
	[Browsable(true)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle GroupNodeStyle
	{
		get
		{
			return _ذ;
		}
		set
		{
			if (_ذ != value)
			{
				_ذ = value;
				if (_ر != null && Nodes != null)
				{
					RefreshItems();
				}
			}
		}
	}

	private bool Մ => true;

	protected CurrencyManager DataManager => _ر;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Description("Gets or sets the index specifying the currently selected item.")]
	public int SelectedIndex
	{
		get
		{
			if (_ر != null)
			{
				if (SelectedNode == null || SelectedNode.BindingIndex < 0)
				{
					return -1;
				}
				return SelectedNode.BindingIndex;
			}
			if (SelectedNode == null)
			{
				return -1;
			}
			return GetNodeFlatIndex(SelectedNode);
		}
		set
		{
			_۷(value);
		}
	}

	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Description("property to use as the actual value for the items in the control. Applies to data-binding scenarios. SelectedValue property will return the value of selected node as indicated by this property.")]
	[DefaultValue("")]
	[Category("Data")]
	public string ValueMember
	{
		get
		{
			return ((BindingMemberInfo)(ref _ض)).BindingMember;
		}
		set
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (value == null)
			{
				value = "";
			}
			BindingMemberInfo val = default(BindingMemberInfo);
			((BindingMemberInfo)(ref val))._002Ector(value);
			if (!((object)(BindingMemberInfo)(ref val)).Equals((object?)_ض))
			{
				if (DisplayMembers.Length == 0)
				{
					List<BindingMemberInfo> list = new List<BindingMemberInfo>();
					list.Add(val);
					_06DA(DataSource, list, _06DD: false);
				}
				if (_ر != null && value != null && value.Length != 0 && !_06E0(val))
				{
					throw new ArgumentException("Invalid value for ValueMember", "value");
				}
				_ض = val;
				OnValueMemberChanged(EventArgs.Empty);
				OnSelectedValueChanged(EventArgs.Empty);
			}
		}
	}

	[Category("Data")]
	[Bindable(true)]
	[Description("Indicates value of the member property specified by the ValueMember property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(null)]
	[Browsable(false)]
	public object SelectedValue
	{
		get
		{
			if (_ر != null && SelectedIndex != -1)
			{
				object item = _ر.List[SelectedIndex];
				return GetPropertyValue(item, ((BindingMemberInfo)(ref _ض)).BindingField);
			}
			return null;
		}
		set
		{
			if (_ر != null)
			{
				string bindingField = ((BindingMemberInfo)(ref _ض)).BindingField;
				if (string.IsNullOrEmpty(bindingField))
				{
					throw new InvalidOperationException("ValueMember property must be set to be able to set SelectedValue");
				}
				PropertyDescriptor propertyDescriptor = ((BindingManagerBase)_ر).GetItemProperties().Find(bindingField, ignoreCase: true);
				MethodInfo method = ((object)_ر).GetType().GetMethod("Find", BindingFlags.Instance | BindingFlags.NonPublic);
				int selectedIndex = -1;
				if ((object)method != null)
				{
					selectedIndex = (int)method.Invoke(_ر, new object[3] { propertyDescriptor, value, true });
				}
				SelectedIndex = selectedIndex;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool EnableDataPositionChange
	{
		get
		{
			return _ط;
		}
		set
		{
			_ط = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether touch support for scrolling is enabled.")]
	public bool TouchEnabled
	{
		get
		{
			return _ع;
		}
		set
		{
			if (value != _ع)
			{
				bool ع = _ع;
				_ع = value;
				OnTouchEnabledChanged(ع, value);
			}
		}
	}

	private int Յ => 32;

	private int Ն => 0;

	[Description(" Occurs just before cell editor is released for editing. It allows you to customize any properties on edit control.")]
	public event PrepareCellEditorEventHandler PrepareCellEditorControl
	{
		add
		{
			PrepareCellEditorEventHandler prepareCellEditorEventHandler = _05A3;
			PrepareCellEditorEventHandler prepareCellEditorEventHandler2;
			do
			{
				prepareCellEditorEventHandler2 = prepareCellEditorEventHandler;
				PrepareCellEditorEventHandler value2 = (PrepareCellEditorEventHandler)Delegate.Combine(prepareCellEditorEventHandler2, value);
				prepareCellEditorEventHandler = Interlocked.CompareExchange(ref _05A3, value2, prepareCellEditorEventHandler2);
			}
			while ((object)prepareCellEditorEventHandler != prepareCellEditorEventHandler2);
		}
		remove
		{
			PrepareCellEditorEventHandler prepareCellEditorEventHandler = _05A3;
			PrepareCellEditorEventHandler prepareCellEditorEventHandler2;
			do
			{
				prepareCellEditorEventHandler2 = prepareCellEditorEventHandler;
				PrepareCellEditorEventHandler value2 = (PrepareCellEditorEventHandler)Delegate.Remove(prepareCellEditorEventHandler2, value);
				prepareCellEditorEventHandler = Interlocked.CompareExchange(ref _05A3, value2, prepareCellEditorEventHandler2);
			}
			while ((object)prepareCellEditorEventHandler != prepareCellEditorEventHandler2);
		}
	}

	public event MouseEventHandler ColumnHeaderMouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _05A4;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _05A4, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _05A4;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _05A4, value2, val2);
			}
			while (val != val2);
		}
	}

	public event MouseEventHandler ColumnHeaderMouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _05A5;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _05A5, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _05A5;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _05A5, value2, val2);
			}
			while (val != val2);
		}
	}

	public event AdvTreeCellEventHandler AfterCheck
	{
		add
		{
			AdvTreeCellEventHandler advTreeCellEventHandler = _05A6;
			AdvTreeCellEventHandler advTreeCellEventHandler2;
			do
			{
				advTreeCellEventHandler2 = advTreeCellEventHandler;
				AdvTreeCellEventHandler value2 = (AdvTreeCellEventHandler)Delegate.Combine(advTreeCellEventHandler2, value);
				advTreeCellEventHandler = Interlocked.CompareExchange(ref _05A6, value2, advTreeCellEventHandler2);
			}
			while ((object)advTreeCellEventHandler != advTreeCellEventHandler2);
		}
		remove
		{
			AdvTreeCellEventHandler advTreeCellEventHandler = _05A6;
			AdvTreeCellEventHandler advTreeCellEventHandler2;
			do
			{
				advTreeCellEventHandler2 = advTreeCellEventHandler;
				AdvTreeCellEventHandler value2 = (AdvTreeCellEventHandler)Delegate.Remove(advTreeCellEventHandler2, value);
				advTreeCellEventHandler = Interlocked.CompareExchange(ref _05A6, value2, advTreeCellEventHandler2);
			}
			while ((object)advTreeCellEventHandler != advTreeCellEventHandler2);
		}
	}

	public event AdvTreeCellBeforeCheckEventHandler BeforeCheck
	{
		add
		{
			AdvTreeCellBeforeCheckEventHandler advTreeCellBeforeCheckEventHandler = _05A7;
			AdvTreeCellBeforeCheckEventHandler advTreeCellBeforeCheckEventHandler2;
			do
			{
				advTreeCellBeforeCheckEventHandler2 = advTreeCellBeforeCheckEventHandler;
				AdvTreeCellBeforeCheckEventHandler value2 = (AdvTreeCellBeforeCheckEventHandler)Delegate.Combine(advTreeCellBeforeCheckEventHandler2, value);
				advTreeCellBeforeCheckEventHandler = Interlocked.CompareExchange(ref _05A7, value2, advTreeCellBeforeCheckEventHandler2);
			}
			while ((object)advTreeCellBeforeCheckEventHandler != advTreeCellBeforeCheckEventHandler2);
		}
		remove
		{
			AdvTreeCellBeforeCheckEventHandler advTreeCellBeforeCheckEventHandler = _05A7;
			AdvTreeCellBeforeCheckEventHandler advTreeCellBeforeCheckEventHandler2;
			do
			{
				advTreeCellBeforeCheckEventHandler2 = advTreeCellBeforeCheckEventHandler;
				AdvTreeCellBeforeCheckEventHandler value2 = (AdvTreeCellBeforeCheckEventHandler)Delegate.Remove(advTreeCellBeforeCheckEventHandler2, value);
				advTreeCellBeforeCheckEventHandler = Interlocked.CompareExchange(ref _05A7, value2, advTreeCellBeforeCheckEventHandler2);
			}
			while ((object)advTreeCellBeforeCheckEventHandler != advTreeCellBeforeCheckEventHandler2);
		}
	}

	public event AdvTreeNodeEventHandler AfterCollapse
	{
		add
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05A8;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Combine(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05A8, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
		remove
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05A8;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Remove(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05A8, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
	}

	public event AdvTreeNodeCancelEventHandler BeforeCollapse
	{
		add
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05A9;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Combine(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05A9, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
		remove
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05A9;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Remove(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05A9, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
	}

	public event AdvTreeNodeEventHandler AfterExpand
	{
		add
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05AA;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Combine(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05AA, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
		remove
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05AA;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Remove(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05AA, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
	}

	public event AdvTreeNodeCancelEventHandler BeforeExpand
	{
		add
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05AB;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Combine(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05AB, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
		remove
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05AB;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Remove(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05AB, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
	}

	public event CommandButtonEventHandler CommandButtonClick
	{
		add
		{
			CommandButtonEventHandler commandButtonEventHandler = _05AC;
			CommandButtonEventHandler commandButtonEventHandler2;
			do
			{
				commandButtonEventHandler2 = commandButtonEventHandler;
				CommandButtonEventHandler value2 = (CommandButtonEventHandler)Delegate.Combine(commandButtonEventHandler2, value);
				commandButtonEventHandler = Interlocked.CompareExchange(ref _05AC, value2, commandButtonEventHandler2);
			}
			while ((object)commandButtonEventHandler != commandButtonEventHandler2);
		}
		remove
		{
			CommandButtonEventHandler commandButtonEventHandler = _05AC;
			CommandButtonEventHandler commandButtonEventHandler2;
			do
			{
				commandButtonEventHandler2 = commandButtonEventHandler;
				CommandButtonEventHandler value2 = (CommandButtonEventHandler)Delegate.Remove(commandButtonEventHandler2, value);
				commandButtonEventHandler = Interlocked.CompareExchange(ref _05AC, value2, commandButtonEventHandler2);
			}
			while ((object)commandButtonEventHandler != commandButtonEventHandler2);
		}
	}

	public event CellEditEventHandler BeforeCellEdit
	{
		add
		{
			CellEditEventHandler cellEditEventHandler = _05AD;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Combine(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05AD, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
		remove
		{
			CellEditEventHandler cellEditEventHandler = _05AD;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Remove(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05AD, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
	}

	public event CellEditEventHandler CellEditEnding
	{
		add
		{
			CellEditEventHandler cellEditEventHandler = _05AE;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Combine(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05AE, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
		remove
		{
			CellEditEventHandler cellEditEventHandler = _05AE;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Remove(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05AE, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
	}

	public event CellEditEventHandler AfterCellEdit
	{
		add
		{
			CellEditEventHandler cellEditEventHandler = _05AF;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Combine(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05AF, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
		remove
		{
			CellEditEventHandler cellEditEventHandler = _05AF;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Remove(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05AF, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
	}

	public event CellEditEventHandler AfterCellEditComplete
	{
		add
		{
			CellEditEventHandler cellEditEventHandler = _05B0;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Combine(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05B0, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
		remove
		{
			CellEditEventHandler cellEditEventHandler = _05B0;
			CellEditEventHandler cellEditEventHandler2;
			do
			{
				cellEditEventHandler2 = cellEditEventHandler;
				CellEditEventHandler value2 = (CellEditEventHandler)Delegate.Remove(cellEditEventHandler2, value);
				cellEditEventHandler = Interlocked.CompareExchange(ref _05B0, value2, cellEditEventHandler2);
			}
			while ((object)cellEditEventHandler != cellEditEventHandler2);
		}
	}

	public event EventHandler SelectionChanged
	{
		add
		{
			EventHandler eventHandler = _05B1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05B1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05B1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05B1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event AdvTreeNodeCancelEventHandler BeforeNodeSelect
	{
		add
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05B2;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Combine(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05B2, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
		remove
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05B2;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Remove(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05B2, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
	}

	public event AdvTreeNodeEventHandler AfterNodeSelect
	{
		add
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05B3;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Combine(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05B3, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
		remove
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05B3;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Remove(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05B3, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
	}

	public event AdvTreeNodeEventHandler AfterNodeDeselect
	{
		add
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05B4;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Combine(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05B4, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
		remove
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05B4;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Remove(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05B4, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
	}

	public event TreeNodeCollectionEventHandler BeforeNodeRemove
	{
		add
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B5;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Combine(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B5, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
		remove
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B5;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Remove(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B5, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
	}

	public event TreeNodeCollectionEventHandler AfterNodeRemove
	{
		add
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B6;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Combine(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B6, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
		remove
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B6;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Remove(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B6, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
	}

	public event TreeNodeCollectionEventHandler BeforeNodeInsert
	{
		add
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B7;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Combine(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B7, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
		remove
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B7;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Remove(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B7, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
	}

	public event TreeNodeCollectionEventHandler AfterNodeInsert
	{
		add
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B8;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Combine(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B8, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
		remove
		{
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler = _05B8;
			TreeNodeCollectionEventHandler treeNodeCollectionEventHandler2;
			do
			{
				treeNodeCollectionEventHandler2 = treeNodeCollectionEventHandler;
				TreeNodeCollectionEventHandler value2 = (TreeNodeCollectionEventHandler)Delegate.Remove(treeNodeCollectionEventHandler2, value);
				treeNodeCollectionEventHandler = Interlocked.CompareExchange(ref _05B8, value2, treeNodeCollectionEventHandler2);
			}
			while ((object)treeNodeCollectionEventHandler != treeNodeCollectionEventHandler2);
		}
	}

	[Description("Occurs when node drag & drop operation is initiated.")]
	public event EventHandler NodeDragStart
	{
		add
		{
			EventHandler eventHandler = _05B9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05B9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05B9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05B9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event AdvTreeNodeCancelEventHandler BeforeNodeDragStart
	{
		add
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05BA;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Combine(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05BA, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
		remove
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = _05BA;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Remove(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref _05BA, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
	}

	public event TreeDragDropEventHandler BeforeNodeDrop
	{
		add
		{
			TreeDragDropEventHandler treeDragDropEventHandler = _05BB;
			TreeDragDropEventHandler treeDragDropEventHandler2;
			do
			{
				treeDragDropEventHandler2 = treeDragDropEventHandler;
				TreeDragDropEventHandler value2 = (TreeDragDropEventHandler)Delegate.Combine(treeDragDropEventHandler2, value);
				treeDragDropEventHandler = Interlocked.CompareExchange(ref _05BB, value2, treeDragDropEventHandler2);
			}
			while ((object)treeDragDropEventHandler != treeDragDropEventHandler2);
		}
		remove
		{
			TreeDragDropEventHandler treeDragDropEventHandler = _05BB;
			TreeDragDropEventHandler treeDragDropEventHandler2;
			do
			{
				treeDragDropEventHandler2 = treeDragDropEventHandler;
				TreeDragDropEventHandler value2 = (TreeDragDropEventHandler)Delegate.Remove(treeDragDropEventHandler2, value);
				treeDragDropEventHandler = Interlocked.CompareExchange(ref _05BB, value2, treeDragDropEventHandler2);
			}
			while ((object)treeDragDropEventHandler != treeDragDropEventHandler2);
		}
	}

	public event TreeDragFeedbackEventHander NodeDragFeedback
	{
		add
		{
			TreeDragFeedbackEventHander treeDragFeedbackEventHander = _05BC;
			TreeDragFeedbackEventHander treeDragFeedbackEventHander2;
			do
			{
				treeDragFeedbackEventHander2 = treeDragFeedbackEventHander;
				TreeDragFeedbackEventHander value2 = (TreeDragFeedbackEventHander)Delegate.Combine(treeDragFeedbackEventHander2, value);
				treeDragFeedbackEventHander = Interlocked.CompareExchange(ref _05BC, value2, treeDragFeedbackEventHander2);
			}
			while ((object)treeDragFeedbackEventHander != treeDragFeedbackEventHander2);
		}
		remove
		{
			TreeDragFeedbackEventHander treeDragFeedbackEventHander = _05BC;
			TreeDragFeedbackEventHander treeDragFeedbackEventHander2;
			do
			{
				treeDragFeedbackEventHander2 = treeDragFeedbackEventHander;
				TreeDragFeedbackEventHander value2 = (TreeDragFeedbackEventHander)Delegate.Remove(treeDragFeedbackEventHander2, value);
				treeDragFeedbackEventHander = Interlocked.CompareExchange(ref _05BC, value2, treeDragFeedbackEventHander2);
			}
			while ((object)treeDragFeedbackEventHander != treeDragFeedbackEventHander2);
		}
	}

	public event TreeDragDropEventHandler AfterNodeDrop
	{
		add
		{
			TreeDragDropEventHandler treeDragDropEventHandler = _05BD;
			TreeDragDropEventHandler treeDragDropEventHandler2;
			do
			{
				treeDragDropEventHandler2 = treeDragDropEventHandler;
				TreeDragDropEventHandler value2 = (TreeDragDropEventHandler)Delegate.Combine(treeDragDropEventHandler2, value);
				treeDragDropEventHandler = Interlocked.CompareExchange(ref _05BD, value2, treeDragDropEventHandler2);
			}
			while ((object)treeDragDropEventHandler != treeDragDropEventHandler2);
		}
		remove
		{
			TreeDragDropEventHandler treeDragDropEventHandler = _05BD;
			TreeDragDropEventHandler treeDragDropEventHandler2;
			do
			{
				treeDragDropEventHandler2 = treeDragDropEventHandler;
				TreeDragDropEventHandler value2 = (TreeDragDropEventHandler)Delegate.Remove(treeDragDropEventHandler2, value);
				treeDragDropEventHandler = Interlocked.CompareExchange(ref _05BD, value2, treeDragDropEventHandler2);
			}
			while ((object)treeDragDropEventHandler != treeDragDropEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeMouseDown
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ӗ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ӗ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ӗ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ӗ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeMouseUp
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ӗ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ӗ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ӗ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ӗ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeMouseMove
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ә;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ә, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ә;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ә, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeMouseEnter
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ә;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ә, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ә;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ә, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeMouseLeave
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ӛ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ӛ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ӛ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ӛ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeMouseHover
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ӛ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ӛ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ӛ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ӛ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeClick
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ӝ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ӝ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = Ӝ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref Ӝ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event TreeNodeMouseEventHandler NodeDoubleClick
	{
		add
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ӝ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Combine(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ӝ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
		remove
		{
			TreeNodeMouseEventHandler treeNodeMouseEventHandler = ӝ;
			TreeNodeMouseEventHandler treeNodeMouseEventHandler2;
			do
			{
				treeNodeMouseEventHandler2 = treeNodeMouseEventHandler;
				TreeNodeMouseEventHandler value2 = (TreeNodeMouseEventHandler)Delegate.Remove(treeNodeMouseEventHandler2, value);
				treeNodeMouseEventHandler = Interlocked.CompareExchange(ref ӝ, value2, treeNodeMouseEventHandler2);
			}
			while ((object)treeNodeMouseEventHandler != treeNodeMouseEventHandler2);
		}
	}

	public event SerializeNodeEventHandler SerializeNode
	{
		add
		{
			SerializeNodeEventHandler serializeNodeEventHandler = _05BE;
			SerializeNodeEventHandler serializeNodeEventHandler2;
			do
			{
				serializeNodeEventHandler2 = serializeNodeEventHandler;
				SerializeNodeEventHandler value2 = (SerializeNodeEventHandler)Delegate.Combine(serializeNodeEventHandler2, value);
				serializeNodeEventHandler = Interlocked.CompareExchange(ref _05BE, value2, serializeNodeEventHandler2);
			}
			while ((object)serializeNodeEventHandler != serializeNodeEventHandler2);
		}
		remove
		{
			SerializeNodeEventHandler serializeNodeEventHandler = _05BE;
			SerializeNodeEventHandler serializeNodeEventHandler2;
			do
			{
				serializeNodeEventHandler2 = serializeNodeEventHandler;
				SerializeNodeEventHandler value2 = (SerializeNodeEventHandler)Delegate.Remove(serializeNodeEventHandler2, value);
				serializeNodeEventHandler = Interlocked.CompareExchange(ref _05BE, value2, serializeNodeEventHandler2);
			}
			while ((object)serializeNodeEventHandler != serializeNodeEventHandler2);
		}
	}

	public event SerializeNodeEventHandler DeserializeNode
	{
		add
		{
			SerializeNodeEventHandler serializeNodeEventHandler = _05BF;
			SerializeNodeEventHandler serializeNodeEventHandler2;
			do
			{
				serializeNodeEventHandler2 = serializeNodeEventHandler;
				SerializeNodeEventHandler value2 = (SerializeNodeEventHandler)Delegate.Combine(serializeNodeEventHandler2, value);
				serializeNodeEventHandler = Interlocked.CompareExchange(ref _05BF, value2, serializeNodeEventHandler2);
			}
			while ((object)serializeNodeEventHandler != serializeNodeEventHandler2);
		}
		remove
		{
			SerializeNodeEventHandler serializeNodeEventHandler = _05BF;
			SerializeNodeEventHandler serializeNodeEventHandler2;
			do
			{
				serializeNodeEventHandler2 = serializeNodeEventHandler;
				SerializeNodeEventHandler value2 = (SerializeNodeEventHandler)Delegate.Remove(serializeNodeEventHandler2, value);
				serializeNodeEventHandler = Interlocked.CompareExchange(ref _05BF, value2, serializeNodeEventHandler2);
			}
			while ((object)serializeNodeEventHandler != serializeNodeEventHandler2);
		}
	}

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

	public event CustomCellEditorEventHandler ProvideCustomCellEditor
	{
		add
		{
			CustomCellEditorEventHandler customCellEditorEventHandler = _05C0;
			CustomCellEditorEventHandler customCellEditorEventHandler2;
			do
			{
				customCellEditorEventHandler2 = customCellEditorEventHandler;
				CustomCellEditorEventHandler value2 = (CustomCellEditorEventHandler)Delegate.Combine(customCellEditorEventHandler2, value);
				customCellEditorEventHandler = Interlocked.CompareExchange(ref _05C0, value2, customCellEditorEventHandler2);
			}
			while ((object)customCellEditorEventHandler != customCellEditorEventHandler2);
		}
		remove
		{
			CustomCellEditorEventHandler customCellEditorEventHandler = _05C0;
			CustomCellEditorEventHandler customCellEditorEventHandler2;
			do
			{
				customCellEditorEventHandler2 = customCellEditorEventHandler;
				CustomCellEditorEventHandler value2 = (CustomCellEditorEventHandler)Delegate.Remove(customCellEditorEventHandler2, value);
				customCellEditorEventHandler = Interlocked.CompareExchange(ref _05C0, value2, customCellEditorEventHandler2);
			}
			while ((object)customCellEditorEventHandler != customCellEditorEventHandler2);
		}
	}

	[Description("Occurs when the DataSource changes.")]
	public event EventHandler DataSourceChanged
	{
		add
		{
			EventHandler eventHandler = _05C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the DisplayMembers property changes")]
	public event EventHandler DisplayMembersChanged
	{
		add
		{
			EventHandler eventHandler = _05C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control is bound to a data value that need to be converted.")]
	public event TreeConvertEventHandler Format
	{
		add
		{
			TreeConvertEventHandler treeConvertEventHandler = _05C3;
			TreeConvertEventHandler treeConvertEventHandler2;
			do
			{
				treeConvertEventHandler2 = treeConvertEventHandler;
				TreeConvertEventHandler value2 = (TreeConvertEventHandler)Delegate.Combine(treeConvertEventHandler2, value);
				treeConvertEventHandler = Interlocked.CompareExchange(ref _05C3, value2, treeConvertEventHandler2);
			}
			while ((object)treeConvertEventHandler != treeConvertEventHandler2);
		}
		remove
		{
			TreeConvertEventHandler treeConvertEventHandler = _05C3;
			TreeConvertEventHandler treeConvertEventHandler2;
			do
			{
				treeConvertEventHandler2 = treeConvertEventHandler;
				TreeConvertEventHandler value2 = (TreeConvertEventHandler)Delegate.Remove(treeConvertEventHandler2, value);
				treeConvertEventHandler = Interlocked.CompareExchange(ref _05C3, value2, treeConvertEventHandler2);
			}
			while ((object)treeConvertEventHandler != treeConvertEventHandler2);
		}
	}

	[Description("Occurs when FormattingEnabled property changes.")]
	public event EventHandler FormattingEnabledChanged
	{
		add
		{
			EventHandler eventHandler = _05C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when FormatString property changes.")]
	public event EventHandler FormatStringChanged
	{
		add
		{
			EventHandler eventHandler = _05C5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when FormatInfo property has changed.")]
	public event EventHandler FormatInfoChanged
	{
		add
		{
			EventHandler eventHandler = _05C6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a Node for an data-bound object item has been created and provides you with opportunity to modify the node")]
	public event DataNodeEventHandler DataNodeCreated
	{
		add
		{
			DataNodeEventHandler dataNodeEventHandler = _05C7;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Combine(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C7, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
		remove
		{
			DataNodeEventHandler dataNodeEventHandler = _05C7;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Remove(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C7, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
	}

	[Description("Occurs when a group Node is created as result of GroupingMembers property setting and provides you with opportunity to modify the node")]
	public event DataNodeEventHandler GroupNodeCreated
	{
		add
		{
			DataNodeEventHandler dataNodeEventHandler = _05C8;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Combine(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C8, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
		remove
		{
			DataNodeEventHandler dataNodeEventHandler = _05C8;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Remove(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C8, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
	}

	[Description("Occurs when value of ValueMember property has changed.")]
	public event EventHandler ValueMemberChanged
	{
		add
		{
			EventHandler eventHandler = _05C9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when value of SelectedValue property has changed.")]
	public event EventHandler SelectedValueChanged
	{
		add
		{
			EventHandler eventHandler = _05CA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when value of SelectedValue property has changed.")]
	public event EventHandler SelectedIndexChanged
	{
		add
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ColumnHeader is automatically created by control as result of data binding and provides you with opportunity to modify it.")]
	public event DataColumnEventHandler DataColumnCreated
	{
		add
		{
			DataColumnEventHandler dataColumnEventHandler = _05CC;
			DataColumnEventHandler dataColumnEventHandler2;
			do
			{
				dataColumnEventHandler2 = dataColumnEventHandler;
				DataColumnEventHandler value2 = (DataColumnEventHandler)Delegate.Combine(dataColumnEventHandler2, value);
				dataColumnEventHandler = Interlocked.CompareExchange(ref _05CC, value2, dataColumnEventHandler2);
			}
			while ((object)dataColumnEventHandler != dataColumnEventHandler2);
		}
		remove
		{
			DataColumnEventHandler dataColumnEventHandler = _05CC;
			DataColumnEventHandler dataColumnEventHandler2;
			do
			{
				dataColumnEventHandler2 = dataColumnEventHandler;
				DataColumnEventHandler value2 = (DataColumnEventHandler)Delegate.Remove(dataColumnEventHandler2, value);
				dataColumnEventHandler = Interlocked.CompareExchange(ref _05CC, value2, dataColumnEventHandler2);
			}
			while ((object)dataColumnEventHandler != dataColumnEventHandler2);
		}
	}

	[Description("Occurs after column has been resized by end-user")]
	public event EventHandler ColumnResized
	{
		add
		{
			EventHandler eventHandler = _05CD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs while column is being resized by end-user.")]
	public event EventHandler ColumnResizing
	{
		add
		{
			EventHandler eventHandler = _05CE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after cell has been selected.")]
	public event AdvTreeCellEventHandler CellSelected
	{
		add
		{
			AdvTreeCellEventHandler advTreeCellEventHandler = _05CF;
			AdvTreeCellEventHandler advTreeCellEventHandler2;
			do
			{
				advTreeCellEventHandler2 = advTreeCellEventHandler;
				AdvTreeCellEventHandler value2 = (AdvTreeCellEventHandler)Delegate.Combine(advTreeCellEventHandler2, value);
				advTreeCellEventHandler = Interlocked.CompareExchange(ref _05CF, value2, advTreeCellEventHandler2);
			}
			while ((object)advTreeCellEventHandler != advTreeCellEventHandler2);
		}
		remove
		{
			AdvTreeCellEventHandler advTreeCellEventHandler = _05CF;
			AdvTreeCellEventHandler advTreeCellEventHandler2;
			do
			{
				advTreeCellEventHandler2 = advTreeCellEventHandler;
				AdvTreeCellEventHandler value2 = (AdvTreeCellEventHandler)Delegate.Remove(advTreeCellEventHandler2, value);
				advTreeCellEventHandler = Interlocked.CompareExchange(ref _05CF, value2, advTreeCellEventHandler2);
			}
			while ((object)advTreeCellEventHandler != advTreeCellEventHandler2);
		}
	}

	[Description("Occurs after cell has been unselected.")]
	public event AdvTreeCellEventHandler CellUnselected
	{
		add
		{
			AdvTreeCellEventHandler advTreeCellEventHandler = א;
			AdvTreeCellEventHandler advTreeCellEventHandler2;
			do
			{
				advTreeCellEventHandler2 = advTreeCellEventHandler;
				AdvTreeCellEventHandler value2 = (AdvTreeCellEventHandler)Delegate.Combine(advTreeCellEventHandler2, value);
				advTreeCellEventHandler = Interlocked.CompareExchange(ref א, value2, advTreeCellEventHandler2);
			}
			while ((object)advTreeCellEventHandler != advTreeCellEventHandler2);
		}
		remove
		{
			AdvTreeCellEventHandler advTreeCellEventHandler = א;
			AdvTreeCellEventHandler advTreeCellEventHandler2;
			do
			{
				advTreeCellEventHandler2 = advTreeCellEventHandler;
				AdvTreeCellEventHandler value2 = (AdvTreeCellEventHandler)Delegate.Remove(advTreeCellEventHandler2, value);
				advTreeCellEventHandler = Interlocked.CompareExchange(ref א, value2, advTreeCellEventHandler2);
			}
			while ((object)advTreeCellEventHandler != advTreeCellEventHandler2);
		}
	}

	[Description("Occurs after users has moved the column.")]
	public event ColumnMovedHandler ColumnMoved
	{
		add
		{
			ColumnMovedHandler columnMovedHandler = ב;
			ColumnMovedHandler columnMovedHandler2;
			do
			{
				columnMovedHandler2 = columnMovedHandler;
				ColumnMovedHandler value2 = (ColumnMovedHandler)Delegate.Combine(columnMovedHandler2, value);
				columnMovedHandler = Interlocked.CompareExchange(ref ב, value2, columnMovedHandler2);
			}
			while ((object)columnMovedHandler != columnMovedHandler2);
		}
		remove
		{
			ColumnMovedHandler columnMovedHandler = ב;
			ColumnMovedHandler columnMovedHandler2;
			do
			{
				columnMovedHandler2 = columnMovedHandler;
				ColumnMovedHandler value2 = (ColumnMovedHandler)Delegate.Remove(columnMovedHandler2, value);
				columnMovedHandler = Interlocked.CompareExchange(ref ב, value2, columnMovedHandler2);
			}
			while ((object)columnMovedHandler != columnMovedHandler2);
		}
	}

	[Description("Occurs while tree control is being rendered.")]
	public event AdvTreeRenderEventHandler Render
	{
		add
		{
			AdvTreeRenderEventHandler advTreeRenderEventHandler = ג;
			AdvTreeRenderEventHandler advTreeRenderEventHandler2;
			do
			{
				advTreeRenderEventHandler2 = advTreeRenderEventHandler;
				AdvTreeRenderEventHandler value2 = (AdvTreeRenderEventHandler)Delegate.Combine(advTreeRenderEventHandler2, value);
				advTreeRenderEventHandler = Interlocked.CompareExchange(ref ג, value2, advTreeRenderEventHandler2);
			}
			while ((object)advTreeRenderEventHandler != advTreeRenderEventHandler2);
		}
		remove
		{
			AdvTreeRenderEventHandler advTreeRenderEventHandler = ג;
			AdvTreeRenderEventHandler advTreeRenderEventHandler2;
			do
			{
				advTreeRenderEventHandler2 = advTreeRenderEventHandler;
				AdvTreeRenderEventHandler value2 = (AdvTreeRenderEventHandler)Delegate.Remove(advTreeRenderEventHandler2, value);
				advTreeRenderEventHandler = Interlocked.CompareExchange(ref ג, value2, advTreeRenderEventHandler2);
			}
			while ((object)advTreeRenderEventHandler != advTreeRenderEventHandler2);
		}
	}

	protected virtual void OnRender(AdvTreeRenderEventArgs e)
	{
		ג?.Invoke(this, e);
	}

	internal void ؾ(AdvTreeRenderEventArgs ؿ)
	{
		OnRender(ؿ);
	}

	public AdvTree()
	{
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		char pathSeparator = Path.PathSeparator;
		_055A = pathSeparator.ToString();
		_055F = new SelectedNodesCollection();
		դ = -1;
		ե = true;
		խ = true;
		ծ = true;
		կ = 4;
		հ = new Size(8, 8);
		ձ = Size.Empty;
		ղ = Color.Empty;
		ճ = eColorSchemePart.None;
		մ = Color.Empty;
		յ = eColorSchemePart.None;
		ն = Color.Empty;
		շ = eColorSchemePart.None;
		չ = Color.Empty;
		պ = eColorSchemePart.None;
		ս = eExpandButtonType.Rectangle;
		տ = 10;
		ր = Color.Empty;
		ց = eColorSchemePart.CustomizeBackground;
		ւ = Color.Empty;
		փ = eColorSchemePart.CustomizeBackground2;
		ք = Color.Empty;
		օ = eColorSchemePart.CustomizeText;
		ֆ = 90;
		և = Color.Empty;
		ֈ = eColorSchemePart.ItemHotBackground;
		_0589 = Color.Empty;
		_058A = eColorSchemePart.ItemHotBackground2;
		_058B = Color.Empty;
		_058C = eColorSchemePart.ItemHotText;
		_058D = 90;
		ӽ = true;
		_0593 = Point.Empty;
		_0595 = 32;
		_0596 = 32;
		_0599 = new ElementStyle();
		_059A = new ArrayList();
		_059B = 1f;
		__05A2 = new _070F();
		_ו = true;
		_ח = new Size(184, 30);
		_ך = true;
		_ל = true;
		_מ = true;
		_ס = -1;
		_ף = 3;
		_צ = true;
		_ק = Color.Empty;
		_ר = Color.Empty;
		_ױ = true;
		_ײ = eSelectionStyle.HighlightCells;
		__05F3 = true;
		__05F4 = 24;
		__05F7 = true;
		__05F9 = 1000;
		__05FA = true;
		__05FB = "";
		__05FD = DateTime.MinValue;
		__05FE = DateTime.MinValue;
		__0600 = true;
		__060A = _0609;
		__060C = new List<Node>();
		__060E = true;
		__060F = true;
		__0612 = true;
		__0614 = true;
		__0615 = true;
		__0617 = Size.Empty;
		__0618 = Point.Empty;
		__0619 = Point.Empty;
		__061C = 24;
		__061D = true;
		__061E = true;
		_ء = 16;
		_آ = true;
		_أ = true;
		_ؤ = true;
		_إ = true;
		_ة = "";
		_ج = new Hashtable();
		_خ = "";
		_د = "";
		_ص = new Dictionary<string, PropertyDescriptor>();
		_ض = default(BindingMemberInfo);
		_ط = true;
		_ع = true;
		_ػ = Point.Empty;
		_ؼ = Point.Empty;
		_ؽ = Rectangle.Empty;
		((ScrollableControl)this)._002Ector();
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		_055F._0700 = this;
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		ـ();
		_0599.StyleChanged += _065F;
		_0599.Ԙ = this;
		Չ.ކ = this;
		_0557 = new _2706(this, ((Control)this).ClientRectangle, __05A2);
		__05A2.NodeVerticalSpacing = _ף;
		_0557.LeftRight = ((Control)this).RtlTranslateLeftRight((LeftRightAlignment)0);
		_175C.फ();
		_0558 = new NodeTreeDisplay(this);
		ՙ = new HeadersCollection();
		Ӥ.Ԙ = this;
		Ո.Ԙ = this;
		լ = new ColorScheme(eColorSchemeStyle.Office2007);
		ձ = _065E();
		((Control)this).AllowDrop = true;
		((Control)this).IsAccessible = true;
		if (BarFunctions.IsWindows7 && TouchHandler.IsTouchEnabled)
		{
			_ظ = new TouchHandler((Control)(object)this, eTouchHandlerType.Gesture);
			_ظ.PanBegin += _۸;
			_ظ.Pan += ۻ;
			_ظ.PanEnd += _۹;
		}
	}

	protected override void Dispose(bool disposing)
	{
		_ד = true;
		if (disposing)
		{
			if (Պ != null)
			{
				Պ.Dispose();
			}
			Timer val = __05F8;
			__05F8 = null;
			if (val != null)
			{
				val.Stop();
				((Component)(object)val).Dispose();
			}
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.ᆟ(ref __05ED);
			BarUtilities.ᆟ(ref _ׯ);
			BarUtilities.ᆟ(ref __05EE);
			BarUtilities.ᆟ(ref ջ);
			BarUtilities.ᆟ(ref ռ);
		}
		if (Ӥ != null && disposing && _Շ)
		{
			Node[] array = new Node[Ӥ.Count];
			Ӥ.CopyTo(array);
			Node[] array2 = array;
			foreach (Node node in array2)
			{
				node.Dispose();
			}
		}
		((Control)this).Dispose(disposing);
	}

	private void ـ()
	{
		Պ = new Container();
	}

	private void ف()
	{
		if (__059D != null)
		{
			__059D.Appearance = _ז;
		}
		if (__059E != null)
		{
			__059E.Appearance = _ז;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTileSize()
	{
		if (_ח.Width == 184)
		{
			return _ח.Height != 30;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTileSize()
	{
		TileSize = new Size(184, 30);
	}

	private void ق(Size ك, Size ل)
	{
		if (__0608 == eView.Tile)
		{
			InvalidateNodesSize();
			RecalcLayout();
		}
	}

	protected virtual void OnIsKeyboardFocusWithinChanged()
	{
		if (_י)
		{
			if (__05EB)
			{
				foreach (Node item in _055F)
				{
					_0651(item);
				}
				return;
			}
			if (SelectedNode != null)
			{
				_0651(SelectedNode);
			}
			return;
		}
		if (__05EB)
		{
			foreach (Node item2 in _055F)
			{
				_0651(item2);
			}
		}
		else if (SelectedNode != null)
		{
			_0651(SelectedNode);
		}
		if (_058F && _ך)
		{
			څ(eTreeAction.Keyboard);
		}
	}

	protected override void OnEnter(EventArgs e)
	{
		IsKeyboardFocusWithin = true;
		((Control)this).OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		IsKeyboardFocusWithin = false;
		((Control)this).OnLeave(e);
	}

	public TreeRenderer GetRenderer()
	{
		if (ԁ == eNodeRenderMode.Default)
		{
			if (_0558 is NodeTreeDisplay)
			{
				return ((NodeTreeDisplay)_0558).SystemRenderer;
			}
			return null;
		}
		return Ԃ;
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

	internal void م(bool ن)
	{
		if (ن)
		{
			if (__05A0 == null)
			{
				__05A0 = new Ԥ();
				__05A0.យ = Columns;
				((Control)this).Controls.Add((Control)(object)__05A0);
			}
		}
		else if (__05A0 != null)
		{
			__05A0.យ = null;
			((Control)this).Controls.Remove((Control)(object)__05A0);
			((Component)(object)__05A0).Dispose();
			__05A0 = null;
		}
		ڬ();
	}

	internal bool ه(int و, int ى)
	{
		if (!Չ.Bounds.Contains(و, ى))
		{
			return false;
		}
		ColumnHeader columnHeader = _064C(و, ى, Չ, _064D: false);
		return columnHeader != null;
	}

	internal ColumnHeader ي(int و, int ى, ColumnHeaderCollection _064B)
	{
		Point pt = new Point(و, ى);
		Point pos = Point.Empty;
		if (AutoScroll)
		{
			pos = _٧();
			if (_064B.ParentNode == null)
			{
				pos.Y = 0;
			}
		}
		Rectangle bounds = _064B.Bounds;
		bounds.Offset(pos);
		if (!bounds.Contains(pt))
		{
			return null;
		}
		foreach (ColumnHeader item in _064B)
		{
			if (item.Visible)
			{
				Rectangle bounds2 = item.Bounds;
				bounds2.Offset(pos);
				if (bounds2.Contains(pt))
				{
					return item;
				}
			}
		}
		return null;
	}

	private ColumnHeader _064C(int و, int ى, ColumnHeaderCollection _064B, bool _064D)
	{
		return _064C(و, ى, _064B, _064D, _064E: false);
	}

	private ColumnHeader _064C(int و, int ى, ColumnHeaderCollection _064B, bool _064D, bool _064E)
	{
		Point pt = new Point(و, ى);
		Point pos = Point.Empty;
		if (AutoScroll)
		{
			pos = _٧();
			if (_064B.ParentNode == null)
			{
				pos.Y = 0;
			}
		}
		Rectangle bounds = _064B.Bounds;
		bounds.Offset(pos);
		if (!bounds.Contains(pt) && !_064D)
		{
			return null;
		}
		int num = _064B.Count - (_064E ? 1 : 0);
		for (int i = 0; i < num; i++)
		{
			ColumnHeader columnHeader = _064B[i];
			Rectangle rectangle = new Rectangle(columnHeader.Bounds.Right - 4, columnHeader.Bounds.Y, 6, columnHeader.Bounds.Height);
			rectangle.Offset(pos);
			if (rectangle.Contains(pt) || (_064D && pt.X >= rectangle.X && pt.X <= rectangle.Right))
			{
				return columnHeader;
			}
		}
		return null;
	}

	internal void _064F(int و, int ى)
	{
		_064F(و, ى, Չ);
	}

	private void _064F(int و, int ى, ColumnHeaderCollection _064B)
	{
		ColumnHeader columnHeader = ي(و, ى, _064B);
		if (columnHeader != null)
		{
			څ(eTreeAction.Mouse);
			_נ = ((Control)this).Cursor;
			((Control)this).Cursor = Cursors.Hand;
			((Control)this).Capture = true;
			_ן = columnHeader;
			Ԩ = _064B.IndexOf(columnHeader);
			((Control)this).Invalidate();
		}
	}

	internal void _0650(int و, int ى)
	{
		_0650(و, ى, Չ);
	}

	private void _0650(int و, int ى, ColumnHeaderCollection _064B)
	{
		ColumnHeader columnHeader = _064C(و, ى, _064B, _ץ, _064E: false);
		if (columnHeader != null)
		{
			((Control)this).Capture = true;
			_ע = columnHeader;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeGridLinesColor()
	{
		return !_ק.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGridLinesColor()
	{
		GridLinesColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAlternateRowColor()
	{
		return !_ר.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAlternateRowColor()
	{
		AlternateRowColor = Color.Empty;
	}

	internal void _0651(Node[] _0652)
	{
		foreach (Node node in _0652)
		{
			_0651(node);
		}
	}

	internal void _0651(Node _0653)
	{
		if (_0653 != null)
		{
			Rectangle r = ((_0653 != _0592 || _0592.Parent != null) ? NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, _0653, _0558.Offset) : _0592.BoundsRelative);
			if (_0653.FullRowBackground)
			{
				r.Width = ((Control)this).Width;
				r.X = 0;
			}
			r.Inflate(կ, կ);
			if (_ײ == eSelectionStyle.FullRowSelect)
			{
				r.X = ((Control)this).ClientRectangle.X;
				r.Width = ((Control)this).ClientRectangle.Width;
			}
			if (NodeStyleMouseOver != null || CellStyleMouseOver != null || _0653.StyleMouseOver != null || NodeStyleMouseOver != null || __0605)
			{
				r.Inflate(1, 1);
			}
			r = GetScreenRectangle(r);
			((Control)this).Invalidate(r);
		}
	}

	public Node FindNodeByName(string name)
	{
		return _175C.ឃ(this, name);
	}

	public Node FindNodeByDataKey(object key)
	{
		return _175C.ឈ(this, key);
	}

	public Node FindNodeByBindingIndex(int bindingIndex)
	{
		return _175C.ញ(this, bindingIndex);
	}

	public Node FindNodeByText(string text)
	{
		return _175C.ត(this, text, null, ទ: true);
	}

	public Node FindNodeByCellText(string text)
	{
		return _175C.ធ(this, text, null, ទ: true);
	}

	public Node FindNodeByText(string text, bool ignoreCase)
	{
		return _175C.ត(this, text, null, ignoreCase);
	}

	public Node FindNodeByCellText(string text, bool ignoreCase)
	{
		return _175C.ធ(this, text, null, ignoreCase);
	}

	public Node FindNodeByText(string text, Node startFromNode, bool ignoreCase)
	{
		return _175C.ត(this, text, startFromNode, ignoreCase);
	}

	public Node FindNodeByCellText(string text, Node startFromNode, bool ignoreCase)
	{
		return _175C.ធ(this, text, startFromNode, ignoreCase);
	}

	internal void _0654()
	{
		_055C = true;
	}

	private void _0655()
	{
		InvalidateNodesSize();
		_0654();
		((Control)this).Invalidate();
	}

	internal static eDotNetBarStyle _0656(eColorSchemeStyle _0657)
	{
		return _0657 switch
		{
			eColorSchemeStyle.Office2003 => eDotNetBarStyle.Office2003, 
			eColorSchemeStyle.VS2005 => eDotNetBarStyle.VS2005, 
			_ => eDotNetBarStyle.Office2007, 
		};
	}

	internal static eColorSchemeStyle _0656(eDotNetBarStyle _0657)
	{
		return _0657 switch
		{
			eDotNetBarStyle.Office2003 => eColorSchemeStyle.Office2003, 
			eDotNetBarStyle.VS2005 => eColorSchemeStyle.VS2005, 
			_ => eColorSchemeStyle.Office2007, 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandButtonSize()
	{
		return ձ != _065E();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandButtonSize()
	{
		ExpandButtonSize = _065E();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandBorderColor()
	{
		if (!ղ.IsEmpty)
		{
			return ճ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandBorderColor()
	{
		ExpandBorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandBackColor()
	{
		if (!մ.IsEmpty)
		{
			return յ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandBackColor()
	{
		ExpandBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandBackColor2()
	{
		if (!ն.IsEmpty)
		{
			return շ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandBackColor2()
	{
		ExpandBackColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandLineColor()
	{
		if (!չ.IsEmpty)
		{
			return պ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandLineColor()
	{
		ExpandLineColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeCommandBackColor()
	{
		if (!ր.IsEmpty)
		{
			return ց == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal void ResetCommandBackColor()
	{
		ԯ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeCommandBackColor2()
	{
		if (!ւ.IsEmpty)
		{
			return փ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal void ResetCommandBackColor2()
	{
		Ա = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeCommandForeColor()
	{
		if (!ք.IsEmpty)
		{
			return օ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal void ResetCommandForeColor()
	{
		Գ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeCommandMouseOverBackColor()
	{
		if (!և.IsEmpty)
		{
			return ֈ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal void ResetCommandMouseOverBackColor()
	{
		Զ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeCommandMouseOverBackColor2()
	{
		if (!_0589.IsEmpty)
		{
			return _058A == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal void ResetCommandMouseOverBackColor2()
	{
		Ը = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeCommandMouseOverForeColor()
	{
		if (!_058B.IsEmpty)
		{
			return _058C == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal void ResetCommandMouseOverForeColor()
	{
		Ժ = Color.Empty;
	}

	public int GetNodeFlatIndex(Node node)
	{
		return _175C.ឍ(this, node);
	}

	public Node GetNodeByFlatIndex(int index)
	{
		return _175C.ណ(this, index);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		__05F5 = true;
		return (AccessibleObject)(object)new AdvTreeAccessibleObject(this);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		_0658(factor);
		InvalidateNodesSize();
		if (__059D != null)
		{
			__059D.ᗺ(factor);
		}
		if (__059E != null)
		{
			__059E.ᗺ(factor);
		}
		((ScrollableControl)this).ScaleControl(factor, specified);
	}

	private void _0658(SizeF _0659)
	{
		foreach (Cell item in _059A)
		{
			Control hostedControl = item.HostedControl;
			if (hostedControl != null)
			{
				hostedControl.Scale(_0659);
			}
		}
	}

	internal Color _065A(Color _065B, eColorSchemePart _065C)
	{
		if (_065C == eColorSchemePart.None || _065C == eColorSchemePart.Custom)
		{
			return _065B;
		}
		ColorScheme colorScheme = ԫ;
		if (colorScheme == null)
		{
			return _065B;
		}
		return (Color)colorScheme.GetType().GetProperty(_065C.ToString()).GetValue(colorScheme, null);
	}

	private void _065D()
	{
		__05A2._073D = ձ;
		InvalidateNodesSize();
		RecalcLayout();
		((Control)this).Refresh();
	}

	private Size _065E()
	{
		return հ;
	}

	private void _065F(object _٠, EventArgs ؿ)
	{
		InvalidateNodesSize();
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
		}
		_١();
	}

	private void _١()
	{
		if (((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	private void _٢()
	{
		__05A2.NodeHorizontalSpacing = _0595;
		__05A2.NodeVerticalSpacing = _0596;
		RecalcLayout();
		_١();
	}

	private void _٣(object _٠, EventArgs ؿ)
	{
		_١();
	}

	public void CollapseAll()
	{
		BeginUpdate();
		try
		{
			foreach (Node node in Nodes)
			{
				node.CollapseAll();
			}
		}
		finally
		{
			EndUpdate(performLayoutAndRefresh: true);
		}
	}

	public void ExpandAll()
	{
		BeginUpdate();
		try
		{
			foreach (Node node in Nodes)
			{
				node.Expand();
				node.ExpandAll();
			}
		}
		finally
		{
			EndUpdate(performLayoutAndRefresh: true);
		}
	}

	private void _٤(Graphics _٥)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		TreeRenderer treeRenderer = Ԧ;
		if (treeRenderer != null)
		{
			Ԧ.DrawTreeBackground(new TreeBackgroundRendererEventArgs(_٥, this));
			return;
		}
		if (!((Control)this).BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				_٥.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo();
		elementStyleDisplayInfo.Bounds = ((Control)this).DisplayRectangle;
		elementStyleDisplayInfo.Graphics = _٥;
		bool ല = false;
		ElementStyle elementStyle = (elementStyleDisplayInfo.Style = ElementStyleDisplay.റ(_0599, out ല));
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		if (ല)
		{
			elementStyle.Dispose();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		if (SuspendPaint)
		{
			return;
		}
		try
		{
			if (_055C)
			{
				RecalcLayout();
			}
			if (((Control)this).BackColor.A < byte.MaxValue)
			{
				((ScrollableControl)this).OnPaintBackground(e);
			}
			Graphics graphics = e.Graphics;
			_٤(graphics);
			Point _066A = Point.Empty;
			bool _066B = false;
			if (AutoScroll)
			{
				_066A = _٧();
				_066B = true;
			}
			else if (SelectionBoxStyle == eSelectionStyle.NodeMarker)
			{
				_066A.X += ԭ;
				_066A.Y += ԭ;
				_066B = true;
			}
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
			if (ե)
			{
				graphics.SmoothingMode = (SmoothingMode)4;
				graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			Rectangle clip = _٦();
			if (__059D != null)
			{
				clip.Width -= ((Control)__059D).Width;
			}
			if (__059E != null)
			{
				clip.Height -= ((Control)__059E).Height;
			}
			graphics.SetClip(clip);
			_٨(graphics, e.ClipRectangle, _066A, _066B, _059B);
			if (_ס >= 0 && _ן != null && _ן.Parent != Չ)
			{
				ស.អ(graphics, this, _ס, _ן.Parent);
			}
			if (ե)
			{
				graphics.SmoothingMode = smoothingMode;
				graphics.TextRenderingHint = textRenderingHint;
			}
		}
		finally
		{
			((Control)this).OnPaint(e);
		}
	}

	internal Rectangle _٦()
	{
		return ᝈ._1756(BackgroundStyle, ((Control)this).ClientRectangle);
	}

	internal Point _٧()
	{
		Point autoScrollPosition = AutoScrollPosition;
		if (SelectionBoxStyle == eSelectionStyle.NodeMarker)
		{
			autoScrollPosition.Y += ԭ;
			autoScrollPosition.X += ԭ;
		}
		return autoScrollPosition;
	}

	private void _٨(Graphics _٥, Rectangle _٩, Point _066A, bool _066B, float _066C)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		if (_175C.फ())
		{
			StringFormat val = new StringFormat(StringFormat.GenericDefault);
			val.Alignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags & ~(val.FormatFlags & 0x1000));
			_٥.DrawString("Thank you very much for trying AdvTree. Unfortunately your trial period is over. To continue using AdvTree you should purchase license at http://www.devcomponents.com", new Font(((Control)this).Font.FontFamily, 12f), SystemBrushes.Highlight, (RectangleF)((Control)this).ClientRectangle, val);
			val.Dispose();
			return;
		}
		if (_066C != 1f)
		{
			Matrix transform = گ(_066C);
			_٥.Transform = transform;
			_٩ = GetLayoutRectangle(_٩);
		}
		if (_066B)
		{
			_0558.Offset = _066A;
		}
		OnRender(new AdvTreeRenderEventArgs(eAdvTreeRenderType.Background, _٥));
		_0558.Paint(_٥, _٩);
		Font val2 = new Font(((Control)this).Font.FontFamily, 7f);
		SolidBrush val3 = new SolidBrush(Color.FromArgb(220, SystemColors.Highlight));
		StringFormat val4 = new StringFormat(StringFormat.GenericDefault);
		val4.Alignment = (StringAlignment)1;
		_٥.DrawString("", val2, (Brush)(object)val3, (float)(((Control)this).DisplayRectangle.X + ((Control)this).DisplayRectangle.Width / 2), (float)(((Control)this).DisplayRectangle.Bottom - 14), val4);
		((Brush)val3).Dispose();
		val4.Dispose();
		val2.Dispose();
	}

	public void PaintTo(Graphics g, bool background)
	{
		PaintTo(g, background, Rectangle.Empty);
	}

	public void PaintTo(Graphics g, bool background, Rectangle clipRectangle)
	{
		if (background)
		{
			_٤(g);
		}
		Point lockedOffset = _0558.GetLockedOffset();
		Point offset = _0558.Offset;
		Point empty = Point.Empty;
		_0557.ܓ = g;
		try
		{
			_0557.PerformLayout();
		}
		finally
		{
			_0557.ܓ = null;
		}
		_06D6();
		_0558.SetLockedOffset(empty);
		try
		{
			_٨(g, clipRectangle, Point.Empty, _066B: true, 1f);
		}
		finally
		{
			_0557.PerformLayout();
			_0558.SetLockedOffset(lockedOffset);
			if (lockedOffset.IsEmpty)
			{
				_0558.Offset = offset;
			}
		}
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		RecalcLayout();
		((ScrollableControl)this).OnRightToLeftChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (!((Component)this).DesignMode)
		{
			Ӕ ӕ = new Ӕ();
			ӕ.Ḕ();
			((Component)(object)ӕ).Dispose();
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		ۄ();
		((Control)this).OnHandleDestroyed(e);
	}

	protected override void OnResize(EventArgs e)
	{
		__05F6++;
		try
		{
			((Control)this).OnResize(e);
			if (((Control)this).Size.Width == 0 || ((Control)this).Size.Height == 0)
			{
				return;
			}
			ګ();
			if (((Չ.Count > 0 && Չ._1735) || __0608 == eView.Tile) && !__060E)
			{
				InvalidateNodesSize();
				if (Nodes.Count == 0 && Չ._1735)
				{
					_0654();
				}
			}
			else
			{
				_0654();
			}
			ڠ();
		}
		finally
		{
			__05F6--;
		}
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		try
		{
			__05F6++;
			((ScrollableControl)this).OnLayout(levent);
		}
		finally
		{
			__05F6--;
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (__05F7 && char.IsLetterOrDigit(e.KeyChar))
		{
			Node selectedNode = SelectedNode;
			if (selectedNode == null || !selectedNode.CheckBoxVisible || !selectedNode.Enabled || e.KeyChar != ' ' || selectedNode == null)
			{
				e.Handled = ProcessKeyboardCharacter(e.KeyChar);
			}
		}
		((Control)this).OnKeyPress(e);
	}

	public virtual bool ProcessKeyboardCharacter(char p)
	{
		string text = _066D(p.ToString());
		Node node = FindNodeByText(text, SelectedNode, ignoreCase: true);
		if (node == null && SelectedNode != null)
		{
			node = FindNodeByText(text, ignoreCase: true);
		}
		else
		{
			while (node != null && !node.Selectable)
			{
				node = FindNodeByText(text, node, ignoreCase: true);
			}
		}
		if (node != null)
		{
			SelectNode(node, eTreeAction.Keyboard);
		}
		else if (text.Length > 1 && __05FA)
		{
			text = (__05FB = p.ToString());
			node = FindNodeByText(text, SelectedNode, ignoreCase: true);
			if (node == null && SelectedNode != null)
			{
				node = FindNodeByText(text, ignoreCase: true);
			}
			if (node != null)
			{
				SelectNode(node, eTreeAction.Keyboard);
			}
		}
		return false;
	}

	private string _066D(string ٮ)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (__05F9 <= 0)
		{
			return ٮ;
		}
		if (__05F8 == null)
		{
			__05F8 = new Timer();
			__05F8.Interval = __05F9;
			__05F8.Tick += ٯ;
			__05F8.Start();
		}
		else
		{
			__05F8.Start();
		}
		__05FB += ٮ;
		return __05FB;
	}

	private void ٯ(object _٠, EventArgs ؿ)
	{
		__05F8.Stop();
		__05FB = "";
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		((Control)this).OnKeyDown(e);
		if (!e.Handled)
		{
			_29B8.ᵢ(this, e);
		}
	}

	protected override bool IsInputChar(char charCode)
	{
		if (char.IsLetterOrDigit(charCode))
		{
			return true;
		}
		return ((Control)this).IsInputChar(charCode);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Invalid comparison between Unknown and I4
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Invalid comparison between Unknown and I4
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Invalid comparison between Unknown and I4
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Invalid comparison between Unknown and I4
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Invalid comparison between Unknown and I4
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Invalid comparison between Unknown and I4
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Invalid comparison between Unknown and I4
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Invalid comparison between Unknown and I4
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Invalid comparison between Unknown and I4
		if (IsKeyboardFocusWithin && !((Control)this).Focused)
		{
			foreach (Cell item in _059A)
			{
				if (item.HostedControl != null && (item.HostedControl.Focused || item.HostedControl.ContainsFocus))
				{
					return ((Control)this).ProcessCmdKey(ref msg, keyData);
				}
			}
		}
		if (!_058F)
		{
			if (AutoScroll && AutoScrollMinSize.Height > ((Control)this).ClientRectangle.Height && __059D != null)
			{
				if ((int)keyData == 131112)
				{
					AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Max(AutoScrollPosition.Y - __059D.SmallChange, -(__059D.Maximum - __059D.LargeChange)));
					return true;
				}
				if ((int)keyData == 131110)
				{
					AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Min(0, AutoScrollPosition.Y + __059D.SmallChange));
					return true;
				}
			}
			if ((int)keyData == 37 || (int)keyData == 39 || (int)keyData == 38 || (int)keyData == 65574 || (int)keyData == 40 || (int)keyData == 65576 || (int)keyData == 35 || (int)keyData == 36 || (int)keyData == 34 || (int)keyData == 33)
			{
				return _29B8._29C0(this, new KeyEventArgs(keyData));
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseDown(e);
		if (!((Control)this).Focused)
		{
			((Control)this).Focus();
		}
		__0618 = Point.Empty;
		Point layoutPosition = GetLayoutPosition(e);
		if (_058F)
		{
			բ = 0;
			if (!څ(eTreeAction.Mouse))
			{
				return;
			}
		}
		_17C6 obj = (((int)e.Button == 1048576 && __05F3 && __0608 != eView.Tile) ? _175C._1777(this, layoutPosition.Y, _1778: false, _1779: true) : _175C._1777(this, layoutPosition.X, layoutPosition.Y, _1778: false, _1779: true));
		if (__0601 != null && (int)e.Button == 1048576 && AllowUserToResizeColumns)
		{
			if (obj._17C8 != null)
			{
				_0650(layoutPosition.X, layoutPosition.Y, obj._17C8);
			}
			else if (_ץ)
			{
				_0650(layoutPosition.X, layoutPosition.Y, Չ);
			}
		}
		else if (obj._17C8 != null)
		{
			ColumnHeader columnHeader = ي(layoutPosition.X, layoutPosition.Y, obj._17C8);
			if (columnHeader != null)
			{
				__05FC = columnHeader;
				columnHeader.OnMouseDown(e);
			}
		}
		Node node = obj._17C7;
		if (node != null)
		{
			ټ(node, e, _0558.Offset);
		}
		else if (__05EB && SelectedNodes.Count > 1 && (Control.ModifierKeys & 0x30000) == 0)
		{
			SelectNode(null, eTreeAction.Mouse);
		}
		_0593 = layoutPosition;
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (__05FC != null)
		{
			__05FC.OnMouseUp(e);
		}
		__05FC = null;
		if (_ע != null)
		{
			DateTime now = DateTime.Now;
			ColumnHeader ע = _ע;
			_ע = null;
			((Control)this).Capture = false;
			if (__05FD == DateTime.MinValue || now.Subtract(__05FD).TotalMilliseconds > (double)SystemInformation.DoubleClickTime)
			{
				__05FD = DateTime.Now;
			}
			else if (now.Subtract(__05FD).TotalMilliseconds <= (double)SystemInformation.DoubleClickTime)
			{
				__05FD = DateTime.MinValue;
				ע.OnDoubleClick(EventArgs.Empty);
				if (ע.DoubleClickAutoSize)
				{
					ע.AutoSize();
				}
				if (ע.Parent != null && ע.Parent.ParentNode == null)
				{
					if (__05A0 != null)
					{
						__05A0.វ = now;
					}
				}
				else if (ע.Parent != null && ע.Parent.ކ != null)
				{
					__05FE = now;
				}
				else
				{
					__05FE = DateTime.MinValue;
				}
			}
			else
			{
				__05FD = DateTime.MinValue;
			}
			OnColumnResized(ע, EventArgs.Empty);
			InvalidateNodesSize();
			return;
		}
		ColumnHeader ן = _ן;
		if (ן != null)
		{
			((Control)this).Cursor = _נ;
			_נ = null;
			ColumnHeaderCollection parent = ן.Parent;
			if (ן.Parent == parent)
			{
				int num = Ԩ;
				int num2 = -1;
				Ԩ = -1;
				if (num >= -1)
				{
					__05FF = true;
					څ(eTreeAction.Mouse);
					if (num == parent.Count)
					{
						num2 = parent.GetDisplayIndex(parent.LastVisibleColumn) + 1;
					}
					else if (num >= 0)
					{
						int displayIndex = parent.GetDisplayIndex(parent[num]);
						num2 = ((displayIndex >= 0) ? displayIndex : 0);
					}
					List<int> ᜭ = parent.ᜭ;
					int displayIndex2 = parent.GetDisplayIndex(ן);
					ᜭ.RemoveAt(displayIndex2);
					if (displayIndex2 < num2)
					{
						num2--;
					}
					ᜭ.Insert(num2, parent.IndexOf(ן));
					parent.ᜮ = true;
					for (int i = 0; i < ᜭ.Count; i++)
					{
						parent[ᜭ[i]].DisplayIndex = i;
					}
					parent.ᜮ = false;
					__05FF = false;
					OnColumnMoved(new ColumnMovedEventArgs(ן, displayIndex2, num2));
					InvalidateNodesSize();
					((Control)this).Invalidate();
					RecalcLayout();
				}
			}
			_ן = null;
		}
		__05FD = DateTime.MinValue;
		_0670();
		ٽ(e);
	}

	protected virtual void OnColumnResized(ColumnHeader header, EventArgs e)
	{
		_05CD?.Invoke(header, e);
	}

	protected virtual void OnColumnMoved(ColumnMovedEventArgs e)
	{
		ב?.Invoke(this, e);
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		پ(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnDoubleClick(e);
		MouseEventArgs val = (MouseEventArgs)(object)((e is MouseEventArgs) ? e : null);
		Node selectedNode = SelectedNode;
		if (selectedNode == null)
		{
			return;
		}
		Point mousePosition = ((Control)this).PointToClient(Control.MousePosition);
		mousePosition = GetLayoutPosition(mousePosition);
		if (selectedNode.Bounds.Contains(mousePosition) || (_ײ == eSelectionStyle.FullRowSelect && mousePosition.Y >= selectedNode.Bounds.Y && mousePosition.Y <= selectedNode.Bounds.Bottom))
		{
			Cell cellAt = GetCellAt(mousePosition);
			bool flag = true;
			if (cellAt != null)
			{
				Rectangle rectangle = NodeDisplay._070A(_1757._1758, cellAt, _0558.Offset);
				if (!rectangle.IsEmpty && rectangle.Contains(mousePosition))
				{
					flag = false;
				}
			}
			Node node = selectedNode;
			if (__0600 && flag && (node.HasChildNodes || node.ExpandVisibility == eNodeExpandVisibility.Visible))
			{
				node.Toggle(eTreeAction.Mouse);
			}
			InvokeNodeDoubleClick(new TreeNodeMouseEventArgs(node, (MouseButtons)((val == null) ? 1048576 : ((int)val.Button)), 2, 0, mousePosition.X, mousePosition.Y));
		}
		if (!selectedNode.HasColumns || !selectedNode.NodesColumnsHeaderVisible || !selectedNode.CommandBounds.Contains(mousePosition))
		{
			return;
		}
		ColumnHeader columnHeader = ي(mousePosition.X, mousePosition.Y, selectedNode.NodesColumns);
		if (columnHeader != null)
		{
			DateTime now = DateTime.Now;
			if (__05FE != DateTime.MinValue && now.Subtract(__05FE).TotalMilliseconds <= (double)SystemInformation.DoubleClickTime)
			{
				__05FE = DateTime.MinValue;
				return;
			}
			__05FE = DateTime.MinValue;
			columnHeader.OnDoubleClick(e);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (ՠ != null)
		{
			ٺ(null);
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseHover(e);
		if (ՠ != null && (Խ || ՠ.FireHoverEvent))
		{
			Point mousePosition = ((Control)this).PointToClient(Control.MousePosition);
			mousePosition = GetLayoutPosition(mousePosition);
			InvokeNodeMouseHover(new TreeNodeMouseEventArgs(ՠ, Control.MouseButtons, 0, 0, mousePosition.X, mousePosition.Y));
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Invalid comparison between Unknown and I4
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		((Control)this).OnMouseMove(e);
		bool flag = false;
		if (ՠ != null && ՠ.Parent == null && ՠ.TreeControl == null)
		{
			ٺ(null);
		}
		Point layoutPosition = GetLayoutPosition(e);
		if (__05FC != null && _ן == null && Math.Abs(layoutPosition.X - _0593.X) >= 2 && _ם)
		{
			_064F(e.X, e.Y, __05FC.Parent);
			return;
		}
		if (_ע != null)
		{
			Point pos = Point.Empty;
			if (AutoScroll)
			{
				pos = _٧();
				if (__0602 == null)
				{
					pos.Y = 0;
				}
			}
			Rectangle bounds = _ע.Bounds;
			if (_0599.PaddingLeft > 0 || _0599.MarginLeft > 0)
			{
				bounds.X += _0599.PaddingLeft + _0599.MarginLeft;
			}
			bounds.Offset(pos);
			bounds = GetScreenRectangle(bounds);
			int num = Math.Max(Math.Max(2, _ע.MinimumWidth), layoutPosition.X - bounds.X);
			if (num != _ע.Width.Absolute)
			{
				_ע.Width.AutoSize = false;
				_ע.Width.Absolute = num;
				OnColumnResizing(_ע, EventArgs.Empty);
				if (__0602 != null)
				{
					InvalidateNodeSize(__0602.ParentNode);
				}
				((Control)this).Update();
				if (__05A0 != null)
				{
					((Control)__05A0).Refresh();
				}
				if (_058F && __0606 is Control)
				{
					Control val = (Control)__0606;
					Rectangle r = NodeDisplay._070A(_1757._175A, _0590, _0558.Offset);
					r = GetScreenRectangle(r);
					val.Bounds = r;
				}
			}
			return;
		}
		if (_ן != null)
		{
			ColumnHeader columnHeader = ي(layoutPosition.X, layoutPosition.Y, _ן.Parent);
			int num2 = -1;
			if (columnHeader != null)
			{
				num2 = columnHeader.Parent.IndexOf(columnHeader);
			}
			else if (columnHeader == null)
			{
				ColumnHeader lastVisibleColumn = _ן.Parent.LastVisibleColumn;
				if (lastVisibleColumn != null && layoutPosition.X >= lastVisibleColumn.Bounds.Right)
				{
					num2 = _ן.Parent.Count;
				}
			}
			if (num2 >= 0)
			{
				Ԩ = num2;
			}
			return;
		}
		if ((int)e.Button == 1048576 && !_058F && ӽ && ՠ != null && _0592 == null && ՠ.DragDropEnabled && ՠ.IsSelected && (Math.Abs(_0593.X - layoutPosition.X) > SystemInformation.DragSize.Width || Math.Abs(_0593.Y - layoutPosition.Y) > SystemInformation.DragSize.Height))
		{
			ە();
		}
		Rectangle rectangle = Rectangle.Empty;
		if (ՠ != null)
		{
			rectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, ՠ, _0558.Offset);
		}
		bool flag2 = true;
		if (_ל && _צ && _ץ && Չ.Count > 0)
		{
			ColumnHeader columnHeader2 = _064C(layoutPosition.X, layoutPosition.Y, Չ, _064D: true, _064E: false);
			if (columnHeader2 != null)
			{
				if (__0601 == null)
				{
					ժ = ((Control)this).Cursor;
					((Control)this).Cursor = Cursors.VSplit;
				}
				__0601 = columnHeader2;
				flag2 = false;
			}
			else
			{
				_0670();
			}
		}
		if (!rectangle.IsEmpty && rectangle.Contains(layoutPosition))
		{
			flag = ڎ(ՠ, e, _0558.Offset);
		}
		else
		{
			Node node = null;
			_17C6 obj = null;
			obj = ((!__05F3 || __0608 != 0) ? _175C._1777(this, layoutPosition.X, layoutPosition.Y, _1778: true) : _175C._1777(this, layoutPosition.Y, _1778: true));
			node = obj._17C7;
			if (node != ՠ)
			{
				flag = ٺ(node);
			}
			if (flag2)
			{
				if (node == null && obj._17C8 != null && _ל)
				{
					ColumnHeader columnHeader3 = _064C(layoutPosition.X, layoutPosition.Y, obj._17C8, _064D: false);
					if (columnHeader3 != null)
					{
						if (__0601 == null)
						{
							ժ = ((Control)this).Cursor;
							((Control)this).Cursor = Cursors.VSplit;
						}
						__0601 = columnHeader3;
						__0602 = obj._17C8;
					}
					else
					{
						_0670();
					}
				}
				else
				{
					_0670();
				}
			}
			if (ՠ != null)
			{
				flag |= ڎ(ՠ, e, _0558.Offset);
			}
		}
		if (flag)
		{
			((Control)this).Update();
		}
	}

	protected virtual void OnColumnResizing(ColumnHeader column, EventArgs e)
	{
		_05CE?.Invoke(column, e);
	}

	private void _0670()
	{
		if (__0601 != null)
		{
			__0601 = null;
			if (ժ != (Cursor)null)
			{
				((Control)this).Cursor = ժ;
			}
			else
			{
				((Control)this).Cursor = null;
			}
			ժ = null;
		}
		__0602 = null;
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (AutoScroll && __059D != null && !IsCellEditing)
		{
			if (e.Delta < 0)
			{
				AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Max(AutoScrollPosition.Y - __059D.SmallChange * SystemInformation.MouseWheelScrollLines, -(__059D.Maximum - __059D.LargeChange)));
			}
			else
			{
				AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Min(0, AutoScrollPosition.Y + __059D.SmallChange * SystemInformation.MouseWheelScrollLines));
			}
		}
		((ScrollableControl)this).OnMouseWheel(e);
	}

	public void DeselectNode(Node node, eTreeAction action)
	{
		if (!__05EB || node == null)
		{
			SelectNode(null, action);
		}
		else if (node.IsSelected)
		{
			_055F.Remove(node, action);
		}
	}

	internal void ٱ(EventArgs ؿ)
	{
		OnSelectionChanged(ؿ);
	}

	protected virtual void OnSelectionChanged(EventArgs e)
	{
		_05B1?.Invoke(this, e);
	}

	internal void ٲ(AdvTreeCellEventArgs ؿ)
	{
		OnCellSelected(ؿ);
	}

	protected virtual void OnCellSelected(AdvTreeCellEventArgs e)
	{
		_05CF?.Invoke(this, e);
	}

	internal void ٳ(AdvTreeCellEventArgs ؿ)
	{
		OnCellUnselected(ؿ);
	}

	protected virtual void OnCellUnselected(AdvTreeCellEventArgs e)
	{
		א?.Invoke(this, e);
	}

	public void SelectNode(Node node, eTreeAction action)
	{
		if ((node != null && !node.Selectable && !((Component)this).DesignMode) || (!__05EB && node == _055E))
		{
			return;
		}
		AdvTreeNodeCancelEventArgs advTreeNodeCancelEventArgs = new AdvTreeNodeCancelEventArgs(action, node);
		OnBeforeNodeSelect(advTreeNodeCancelEventArgs);
		if (advTreeNodeCancelEventArgs.Cancel)
		{
			return;
		}
		__0618 = Point.Empty;
		if (__05EB && !((Component)this).DesignMode)
		{
			if (_055F.Count > 0)
			{
				_055F.Ԗ = action;
				_055F.Clear();
				_055F.Ԗ = eTreeAction.Code;
			}
			if (node != null)
			{
				_055F._0701 = true;
				try
				{
					if (_055F.Add(node, action) != -1)
					{
						node.EnsureVisible();
					}
				}
				finally
				{
					_055F._0701 = false;
				}
			}
			else
			{
				OnSelectionChanged(EventArgs.Empty);
			}
			((Control)this).Invalidate();
		}
		else
		{
			if (_058F && !څ(eTreeAction.Code))
			{
				return;
			}
			Node node2 = _055E;
			_055E = node;
			if (node2 != null)
			{
				node2.IsSelected = false;
				_0651(node2);
				if (node2.GetSelectedCell() != null && node2 != node)
				{
					node2.SetSelectedCell(null, action);
				}
				if (_055F.Contains(node2))
				{
					_055F.Remove(node2);
				}
				OnAfterNodeDeselect(new AdvTreeNodeEventArgs(action, node2));
				node2.InternalDeselected(action);
			}
			if (_055E != null)
			{
				_055E.IsSelected = true;
				_055F.Add(_055E);
				_0651(_055E);
				if (_055E.SelectedCell == null)
				{
					_055E.SelectFirstCell(action);
				}
				_055E.EnsureVisible();
			}
			if (SelectedPathConnector != null)
			{
				((Control)this).Invalidate();
			}
			if (_ر != null && _ط && node != null && node.BindingIndex > -1 && ((BindingManagerBase)_ر).Position != node.BindingIndex)
			{
				((BindingManagerBase)_ر).Position = node.BindingIndex;
			}
			AdvTreeNodeEventArgs args = new AdvTreeNodeEventArgs(action, node);
			OnAfterNodeSelect(args);
			if (_055E != null)
			{
				_055E.InternalSelected(action);
			}
			OnSelectedIndexChanged(EventArgs.Empty);
			if (!string.IsNullOrEmpty(ValueMember))
			{
				OnSelectedValueChanged(EventArgs.Empty);
			}
			OnSelectionChanged(EventArgs.Empty);
		}
	}

	private void ٴ()
	{
		InvalidateNodesSize();
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	private void ٵ(Node _0653)
	{
		_0653.SizeChanged = true;
		foreach (Node node in _0653.Nodes)
		{
			ٵ(node);
		}
	}

	public void InvalidateNodesSize()
	{
		if (_ע != null)
		{
			ArrayList paintedNodes = NodeDisplay.PaintedNodes;
			{
				foreach (Node item in paintedNodes)
				{
					ٵ(item);
				}
				return;
			}
		}
		foreach (Node item2 in Ӥ)
		{
			ٵ(item2);
		}
	}

	public void InvalidateNodeSize(Node node)
	{
		ٵ(node);
	}

	private void ٶ(object _٠, EventArgs ؿ)
	{
		if (_٠ == գ)
		{
			ImageList = null;
		}
	}

	internal void ٷ()
	{
		Node node = SelectedNode;
		Node node2 = node;
		if (node == null)
		{
			return;
		}
		if (node.TreeControl != this)
		{
			node2 = null;
		}
		else
		{
			while (node != null)
			{
				node = node.Parent;
				if (node != null && !node.Expanded && node.Selectable)
				{
					node2 = node;
				}
			}
		}
		if (node2 == null || !node2.IsVisible)
		{
			if (!ړ(eTreeAction.Code))
			{
				SelectedNode = null;
			}
		}
		else if (__05EB)
		{
			if (!SelectedNodes.Contains(node2))
			{
				SelectedNode = node2;
			}
		}
		else if (SelectedNode != node2)
		{
			SelectedNode = node2;
		}
	}

	private bool ٸ(Cell ٹ)
	{
		bool result = false;
		if (ٹ == ա)
		{
			return result;
		}
		if (ա != null && ա != ٹ)
		{
			if (CellStyleMouseOver != null || ա.StyleMouseOver != null || ԁ != 0 || (ա.Parent != null && ա.Parent.RenderMode != 0))
			{
				result = true;
			}
			ա.SetMouseOver(over: false);
			if (ա.Tooltip != null)
			{
				DevComponents.AdvTree.Interop._0EFE.ఘ((Control)(object)this);
			}
		}
		ա = ٹ;
		if (ա != null)
		{
			ա.SetMouseOver(over: true);
			if (CellStyleMouseOver != null || ա.StyleMouseOver != null || ԁ != 0 || (ա.Parent != null && ա.Parent.RenderMode != 0))
			{
				result = true;
			}
		}
		ڌ();
		return result;
	}

	private bool ٺ(Node ٻ)
	{
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (ՠ != null)
		{
			flag |= ٸ(null);
			if (ՠ.MouseOverNodePart == _27AF._27B1)
			{
				flag = true;
			}
			ՠ.MouseOverNodePart = _27AF.ᘶ;
			if (NodeStyleMouseOver != null || CellStyleMouseOver != null || ՠ.StyleMouseOver != null || NodeStyleMouseOver != null || ԁ != 0 || ՠ.RenderMode != 0 || ՠ.CommandButton || flag || __0605)
			{
				_0651(ՠ);
				flag = true;
			}
			if (ՠ != ٻ && ՠ != null)
			{
				Point point = ((Control)this).PointToClient(Control.MousePosition);
				InvokeNodeMouseLeave(new TreeNodeMouseEventArgs(ՠ, Control.MouseButtons, 0, 0, point.X, point.Y));
			}
		}
		bool flag2 = ՠ != ٻ;
		ՠ = ٻ;
		if (ٻ != null)
		{
			ٻ.MouseOverNodePart = _27AF._27B0;
			if (ٻ.StyleMouseOver != null || NodeStyleMouseOver != null || ԁ != 0 || ٻ.RenderMode != 0 || ٻ.CommandButton || __0605)
			{
				_0651(ٻ);
				flag = true;
			}
			Point point2 = ((Control)this).PointToClient(Control.MousePosition);
			InvokeNodeMouseEnter(new TreeNodeMouseEventArgs(ٻ, Control.MouseButtons, 0, 0, point2.X, point2.Y));
			if (flag2 && (Խ || ٻ.FireHoverEvent))
			{
				DevComponents.AdvTree.Interop._0EFE.ఘ((Control)(object)this);
			}
		}
		return flag;
	}

	private void ټ(Node _0653, MouseEventArgs ؿ, Point _066A)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Invalid comparison between Unknown and I4
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Invalid comparison between Unknown and I4
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Invalid comparison between Unknown and I4
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Invalid comparison between Unknown and I4
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Invalid comparison between Unknown and I4
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Invalid comparison between Unknown and I4
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Invalid comparison between Unknown and I4
		Point layoutPosition = GetLayoutPosition(ؿ);
		InvokeNodeMouseDown(new TreeNodeMouseEventArgs(_0653, ؿ.Button, ؿ.Clicks, ؿ.Delta, layoutPosition.X, layoutPosition.Y));
		if ((int)ؿ.Button == 1048576)
		{
			if (NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandHitTestBounds, _0653, _066A).Contains(layoutPosition) && ؿ.Clicks == 1 && _0653.ExpandVisibility != eNodeExpandVisibility.Hidden)
			{
				բ = 0;
				_0653.Toggle(eTreeAction.Mouse);
				return;
			}
			if (_0653.CommandButton)
			{
				բ = 0;
				if (NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CommandBounds, _0653, _066A).Contains(layoutPosition))
				{
					ڸ(_0653, new CommandButtonEventArgs(eTreeAction.Mouse, _0653));
					return;
				}
			}
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, _0653, _066A);
			if ((nodeRectangle.Contains(layoutPosition) || (__05F3 && layoutPosition.Y >= nodeRectangle.Y && layoutPosition.Y <= nodeRectangle.Bottom)) && _0653.TreeControl != null)
			{
				if (_0653.TreeControl.SelectedNode != _0653)
				{
					բ = 0;
				}
				if (_0653.Selectable)
				{
					if (__05EB && _055F.Count > 0 && (int)Control.ModifierKeys == 0 && (int)ؿ.Button == 1048576)
					{
						__0604 = true;
					}
					else
					{
						__0604 = false;
					}
					if (__05EB && _055F.Count > 0 && ((int)Control.ModifierKeys == 65536 || (int)Control.ModifierKeys == 131072))
					{
						բ = 0;
						if (__05EC == eMultiSelectRule.SameParent && _055F[0].Parent != _0653.Parent)
						{
							return;
						}
						if ((int)Control.ModifierKeys == 65536 && _055F.Count > 0)
						{
							Node node = _055F[0];
							Node node2 = _0653;
							bool flag = false;
							_055F.ۿ = true;
							try
							{
								while (_055F.Count > 1)
								{
									_055F.Remove(_055F[_055F.Count - 1], eTreeAction.Mouse);
									flag = true;
								}
							}
							finally
							{
								_055F.ۿ = false;
							}
							if (node2 != node)
							{
								if (node2.Bounds.Y > node.Bounds.Y)
								{
									_055F.ۿ = true;
									try
									{
										do
										{
											if (!node2.IsSelected && node2.Selectable && (__05EC == eMultiSelectRule.AnyNode || (__05EC == eMultiSelectRule.SameParent && _055F.Count > 0 && _055F[0].Parent == node2.Parent)))
											{
												_055F.Add(node2, eTreeAction.Mouse);
											}
											node2 = _175C.ᝯ(node2);
										}
										while (node != node2 && node2 != null);
										return;
									}
									finally
									{
										_055F.ۿ = false;
										ٱ(EventArgs.Empty);
									}
								}
								_055F.ۿ = true;
								try
								{
									do
									{
										if (!node2.IsSelected && node2.Selectable && (__05EC == eMultiSelectRule.AnyNode || (__05EC == eMultiSelectRule.SameParent && _055F.Count > 0 && _055F[0].Parent == node2.Parent)))
										{
											_055F.Add(node2, eTreeAction.Mouse);
										}
										node2 = _175C.ᝩ(node2);
									}
									while (node != node2 && node2 != null);
									return;
								}
								finally
								{
									_055F.ۿ = false;
									ٱ(EventArgs.Empty);
								}
							}
							if (flag)
							{
								ٱ(EventArgs.Empty);
							}
						}
						else if (_0653.IsSelected)
						{
							_055F.Remove(_0653, eTreeAction.Mouse);
						}
						else
						{
							_055F.Add(_0653, eTreeAction.Mouse);
						}
						return;
					}
					if (!_0653.IsSelected)
					{
						SelectNode(_0653, eTreeAction.Mouse);
						if (_0653.TreeControl == null || _0653.TreeControl.SelectedNode != _0653)
						{
							return;
						}
					}
				}
				Cell cell = ڍ(_0653, layoutPosition.X, layoutPosition.Y, _066A);
				if (cell == null)
				{
					return;
				}
				bool flag2 = false;
				if (cell.CheckBoxVisible && cell.GetEnabled())
				{
					Rectangle checkBoxBoundsRelative = cell.CheckBoxBoundsRelative;
					checkBoxBoundsRelative.Offset(NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, _0653, _066A).Location);
					if (checkBoxBoundsRelative.Contains(layoutPosition))
					{
						if (cell.CheckBoxThreeState)
						{
							if ((int)cell.CheckState == 1)
							{
								cell.SetChecked((CheckState)2, eTreeAction.Mouse);
							}
							else if ((int)cell.CheckState == 0)
							{
								cell.SetChecked((CheckState)1, eTreeAction.Mouse);
							}
							else if ((int)cell.CheckState == 2)
							{
								cell.SetChecked((CheckState)0, eTreeAction.Mouse);
							}
						}
						else
						{
							cell.SetChecked(!cell.Checked, eTreeAction.Mouse);
						}
						flag2 = true;
						բ = 0;
					}
				}
				if (_0653.SelectedCell != cell)
				{
					բ = 1;
				}
				else if (!flag2)
				{
					բ++;
				}
				_0653.SetSelectedCell(cell, eTreeAction.Mouse);
				cell.SetMouseDown(over: true);
			}
			else
			{
				բ = 0;
			}
		}
		else
		{
			if ((int)ؿ.Button != 2097152 || _0653.TreeControl == null)
			{
				return;
			}
			if (!_0653.IsSelected)
			{
				SelectNode(_0653, eTreeAction.Mouse);
			}
			if ((!MultiSelect && _0653.TreeControl.SelectedNode != _0653) || _0653.ContextMenu == null)
			{
				return;
			}
			if (_0653.ContextMenu is ContextMenu)
			{
				object contextMenu = _0653.ContextMenu;
				ContextMenu val = (ContextMenu)((contextMenu is ContextMenu) ? contextMenu : null);
				val.Show((Control)(object)this, new Point(ؿ.X, ؿ.Y));
			}
			else if (_0653.ContextMenu.GetType().FullName == "System.Windows.Forms.ContextMenuStrip")
			{
				_0653.ContextMenu.GetType().InvokeMember("Show", BindingFlags.InvokeMethod, null, _0653.ContextMenu, new object[2]
				{
					this,
					new Point(ؿ.X, ؿ.Y)
				});
			}
			else if (_0653.ContextMenu.GetType().FullName == "DevComponents.DotNetBar.ButtonItem")
			{
				Point point = ((Control)this).PointToScreen(new Point(ؿ.X, ؿ.Y));
				((PopupItem)_0653.ContextMenu).SetSourceControl((Control)(object)this);
				_0653.ContextMenu.GetType().InvokeMember("Popup", BindingFlags.InvokeMethod, null, _0653.ContextMenu, new object[1] { point });
			}
			else
			{
				if (!_0653.ContextMenu.ToString().StartsWith(_0603) || _059C == null)
				{
					return;
				}
				string text = _0653.ContextMenu.ToString().Substring(_0603.Length);
				object obj = _059C.GetType().InvokeMember("ContextMenus", BindingFlags.GetProperty, null, _059C, null);
				int num = (int)obj.GetType().InvokeMember("IndexOf", BindingFlags.InvokeMethod, null, obj, new string[1] { text });
				if (num >= 0)
				{
					IList list = obj as IList;
					object obj2 = list[num];
					try
					{
						obj2.GetType().InvokeMember("SetSourceControl", BindingFlags.InvokeMethod, null, obj2, new object[1] { this });
					}
					catch
					{
					}
					Point point2 = ((Control)this).PointToScreen(new Point(ؿ.X, ؿ.Y));
					obj2.GetType().InvokeMember("Popup", BindingFlags.InvokeMethod, null, obj2, new object[1] { point2 });
				}
			}
		}
	}

	private void ٽ(MouseEventArgs ؿ)
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		Point layoutPosition = GetLayoutPosition(ؿ);
		if (SelectedNode != null && SelectedNode.SelectedCell != null && SelectedNode.SelectedCell.IsMouseDown)
		{
			SelectedNode.SelectedCell.SetMouseDown(over: false);
			_0651(SelectedNode);
			flag = true;
		}
		if (flag)
		{
			((Control)this).Update();
		}
		Node node = null;
		node = ((SelectedNode == null || !SelectedNode.Bounds.Contains(layoutPosition)) ? _175C._1777(this, layoutPosition.X, layoutPosition.Y, _1778: true)._17C7 : SelectedNode);
		if (__0604 && node != null && node.IsSelected && __05EB && _055F.Count > 1 && (int)Control.ModifierKeys == 0)
		{
			SelectNode(node, eTreeAction.Mouse);
		}
		__0604 = false;
		if (node != null)
		{
			InvokeNodeMouseUp(new TreeNodeMouseEventArgs(node, ؿ.Button, ؿ.Clicks, ؿ.Delta, layoutPosition.X, layoutPosition.Y));
		}
	}

	private void پ(EventArgs ؿ)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Invalid comparison between Unknown and I4
		if (SelectedNode == null)
		{
			return;
		}
		MouseEventArgs val = (MouseEventArgs)(object)((ؿ is MouseEventArgs) ? ؿ : null);
		if (val == null)
		{
			Point point = ((Control)this).PointToClient(Control.MousePosition);
			val = new MouseEventArgs((MouseButtons)1048576, 1, point.X, point.Y, 0);
		}
		Point layoutPosition = GetLayoutPosition(val);
		_17C6 obj = (((int)val.Button == 1048576 && __05F3 && __0608 == eView.Tree) ? _175C._1777(this, layoutPosition.Y + __0618.Y, _1778: true, _1779: true) : _175C._1777(this, layoutPosition.X, layoutPosition.Y, _1778: true, _1779: true));
		__0618 = Point.Empty;
		if (obj._17C7 != null && (obj._17C7 == SelectedNode || (obj._17C7.IsSelected && (int)val.Button == 2097152)) && !obj._17C7.ExpandPartRectangle.Contains(layoutPosition))
		{
			InvokeNodeClick(new TreeNodeMouseEventArgs(SelectedNode, val.Button, val.Clicks, val.Delta, val.X, val.Y));
			if (բ > 1 && (int)val.Button == 1048576)
			{
				ٿ(eTreeAction.Mouse);
			}
		}
		if (obj._17C8 != null)
		{
			ي(layoutPosition.X, layoutPosition.Y, obj._17C8)?.OnClick(ؿ);
		}
	}

	internal bool ٿ(eTreeAction ڀ)
	{
		if (CellEdit && SelectedNode != null && SelectedNode.SelectedCell != null && SelectedNode.SelectedCell.IsEditable)
		{
			ځ(SelectedNode.SelectedCell, ڀ);
			return true;
		}
		return false;
	}

	internal void ځ(Cell ڂ, eTreeAction ڃ)
	{
		ځ(ڂ, ڃ, null);
	}

	internal void ځ(Cell ڂ, eTreeAction ڃ, string ڄ)
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		if (ڂ == null || !ڂ.GetEnabled() || (_058F && !څ(ڃ)))
		{
			return;
		}
		if (__05FF)
		{
			throw new InvalidOperationException("Cannot initiate cell edit while column is being moved.");
		}
		CellEditEventArgs cellEditEventArgs = new CellEditEventArgs(ڂ, ڃ, "");
		OnBeforeCellEdit(cellEditEventArgs);
		if (cellEditEventArgs.Cancel || (_ر != null && ڂ.Parent.DataKey == null))
		{
			return;
		}
		ICellEditControl cellEditControl = ڈ(ڂ);
		if (cellEditControl == null)
		{
			return;
		}
		_175C._1771(ڂ);
		((Control)this).Update();
		_17C1 obj = cellEditControl as _17C1;
		Control val = (Control)((cellEditControl is Control) ? cellEditControl : null);
		Rectangle r = NodeDisplay._070A(_1757._175A, ڂ, _0558.Offset);
		if (r.IsEmpty)
		{
			r = NodeDisplay._070A(_1757._175B, ڂ, _0558.Offset);
			if (string.IsNullOrEmpty(ڂ.Text))
			{
				r.X += r.Width;
			}
		}
		r = GetScreenRectangle(r);
		cellEditControl.CurrentValue = ڂ.Text;
		cellEditControl.EditWordWrap = ڂ.WordWrap;
		Font val2 = _2791._2797(this, ڂ);
		if (_059B != 1f && val2 != null)
		{
			val2 = new Font(val2.FontFamily, val2.SizeInPoints * _059B);
		}
		val.Font = val2;
		val.Location = r.Location;
		val.Size = r.Size;
		val.Visible = true;
		val.Focus();
		if (obj != null)
		{
			if (ڄ == null)
			{
				((TextBoxBase)obj).SelectAll();
			}
			else
			{
				((Control)obj).Text = ڄ;
				((TextBoxBase)obj).Select(((Control)obj).Text.Length, 0);
			}
		}
		PrepareCellEditor(ڂ, cellEditControl);
		ڂ.Parent.SetEditing(b: true);
		_0590 = ڂ;
		_058F = true;
		__0606 = cellEditControl;
		cellEditControl.BeginEdit();
	}

	protected virtual void PrepareCellEditor(Cell cell, ICellEditControl editControl)
	{
		OnPrepareCellEditorControl(new PrepareCellEditorEventArgs(cell, editControl));
	}

	protected virtual void OnPrepareCellEditorControl(PrepareCellEditorEventArgs e)
	{
		_05A3?.Invoke(this, e);
	}

	protected virtual void OnProvideCustomCellEditor(CustomCellEditorEventArgs e)
	{
		_05C0?.Invoke(this, e);
	}

	internal bool څ(eTreeAction ڃ)
	{
		return څ(ڃ, چ: false);
	}

	internal bool څ(eTreeAction ڃ, bool چ)
	{
		Cell cell = _0590;
		if (cell == null || __0606 == null)
		{
			_058F = false;
			return true;
		}
		ICellEditControl cellEditControl = __0606;
		Control val = (Control)((cellEditControl is Control) ? cellEditControl : null);
		string newText = cellEditControl.CurrentValue.ToString();
		CellEditEventArgs cellEditEventArgs = new CellEditEventArgs(cell, ڃ, newText, چ, cellEditControl);
		ڑ(cellEditEventArgs);
		if (cellEditEventArgs.Cancel)
		{
			return false;
		}
		((Control)this).Focus();
		val.Visible = false;
		newText = cellEditEventArgs.NewText;
		ڒ(cellEditEventArgs);
		newText = cellEditEventArgs.NewText;
		if (!cellEditEventArgs.Cancel && cell.Text != newText && !چ)
		{
			if (((Component)this).DesignMode && cell.Parent.Cells[0] == cell)
			{
				TypeDescriptor.GetProperties(cell.Parent)["Text"].SetValue(cell.Parent, newText);
			}
			else
			{
				TypeDescriptor.GetProperties(cell)["Text"].SetValue(cell, newText);
			}
			BeginUpdate();
			RecalcLayout();
			EndUpdate();
		}
		cell.Parent.SetEditing(b: false);
		if (_ا != null)
		{
			object dataKey = cell.Parent.DataKey;
			string dataFieldName = Columns[cell.Parent.Cells.IndexOf(cell)].DataFieldName;
			if (dataKey != null)
			{
				IEditableObject editableObject = dataKey as IEditableObject;
				PropertyDescriptor propertyDescriptor = ((_ر == null) ? TypeDescriptor.GetProperties(dataKey).Find(dataFieldName, ignoreCase: true) : ((BindingManagerBase)_ر).GetItemProperties().Find(dataFieldName, ignoreCase: true));
				if (propertyDescriptor != null)
				{
					if (_ط && _ر != null && ((BindingManagerBase)_ر).Position != cell.Parent.BindingIndex)
					{
						((BindingManagerBase)_ر).Position = cell.Parent.BindingIndex;
					}
					editableObject?.BeginEdit();
					propertyDescriptor.SetValue(dataKey, newText);
					editableObject?.EndEdit();
				}
			}
		}
		_0590 = null;
		_058F = false;
		OnAfterCellEditComplete(cellEditEventArgs);
		if (cellEditControl != _0591)
		{
			((Control)this).Controls.Remove(val);
			cellEditControl.EditComplete -= ڊ;
			cellEditControl.CancelEdit -= ڋ;
		}
		cellEditControl.EndEdit();
		return true;
	}

	protected virtual void OnAfterCellEditComplete(CellEditEventArgs e)
	{
		_05B0?.Invoke(this, e);
	}

	internal void ڇ(eTreeAction ڃ)
	{
		if (_0590 == null)
		{
			return;
		}
		if (_0591 != null)
		{
			((Control)_0591).Text = _0590.Text;
		}
		else
		{
			ICellEditControl cellEditControl = __0606;
			if (cellEditControl != null)
			{
				cellEditControl.CurrentValue = _0590.Text;
			}
		}
		څ(ڃ, چ: true);
	}

	private ICellEditControl ڈ(Cell ڂ)
	{
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		eCellEditorType effectiveEditorType = ڂ.GetEffectiveEditorType();
		ICellEditControl cellEditControl = null;
		switch (effectiveEditorType)
		{
		case eCellEditorType.Default:
			cellEditControl = ډ();
			if (cellEditControl is TextBox)
			{
				ColumnHeader columnHeader = _175C.ផ(this, ڂ);
				if (columnHeader != null)
				{
					((TextBoxBase)(TextBox)cellEditControl).MaxLength = columnHeader.MaxInputLength;
				}
				else
				{
					((TextBoxBase)(TextBox)cellEditControl).MaxLength = 0;
				}
			}
			break;
		case eCellEditorType.NumericInteger:
		{
			_29C1 _29C12 = new _29C1();
			_29C12.ShowUpDown = true;
			cellEditControl = _29C12;
			break;
		}
		case eCellEditorType.NumericDouble:
		{
			_29C2 _29C23 = new _29C2();
			_29C23.ShowUpDown = true;
			cellEditControl = _29C23;
			break;
		}
		case eCellEditorType.NumericCurrency:
		{
			_29C2 _29C22 = new _29C2();
			_29C22.DisplayFormat = "C";
			_29C22.ShowUpDown = true;
			cellEditControl = _29C22;
			break;
		}
		case eCellEditorType.Date:
		{
			_29CA _29CA4 = new _29CA();
			_29CA4.Format = eDateTimePickerFormat.Short;
			_29CA4.ButtonDropDown.Visible = true;
			cellEditControl = _29CA4;
			break;
		}
		case eCellEditorType.Time:
		{
			_29CA _29CA3 = new _29CA();
			_29CA3.Format = eDateTimePickerFormat.ShortTime;
			_29CA3.ButtonDropDown.Visible = true;
			cellEditControl = _29CA3;
			break;
		}
		case eCellEditorType.DateTime:
		{
			_29CA _29CA2 = new _29CA();
			_29CA2.Format = eDateTimePickerFormat.Custom;
			CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
			_29CA2.CustomFormat = activeCulture.DateTimeFormat.ShortDatePattern + " " + activeCulture.DateTimeFormat.ShortTimePattern;
			_29CA2.DateTimeSelectorVisibility = eDateTimeSelectorVisibility.Both;
			_29CA2.ButtonDropDown.Visible = true;
			cellEditControl = _29CA2;
			break;
		}
		case eCellEditorType.Custom:
		{
			CustomCellEditorEventArgs customCellEditorEventArgs = new CustomCellEditorEventArgs(ڂ);
			OnProvideCustomCellEditor(customCellEditorEventArgs);
			if (customCellEditorEventArgs.EditControl == null)
			{
				throw new ArgumentNullException("CustomCellEditorEventArgs.EditControl must be set to the custom cell editor.");
			}
			cellEditControl = customCellEditorEventArgs.EditControl;
			break;
		}
		}
		if (cellEditControl == null)
		{
			throw new NotImplementedException("Editor type " + effectiveEditorType.ToString() + " not implemented.");
		}
		if (((Control)cellEditControl).Parent != this)
		{
			((Control)this).Controls.Add((Control)cellEditControl);
			cellEditControl.EditComplete += ڊ;
			cellEditControl.CancelEdit += ڋ;
		}
		return cellEditControl;
	}

	private _17C1 ډ()
	{
		if (_0591 == null)
		{
			_0591 = new _17C1();
			((Control)_0591).Name = "DefaultCellEditor";
			((Control)_0591).AutoSize = false;
			_0591._17C2 = true;
			((Control)this).Controls.Add((Control)(object)_0591);
			_0591.EditComplete += ڊ;
			_0591.CancelEdit += ڋ;
		}
		return _0591;
	}

	private void ڊ(object _٠, EventArgs ؿ)
	{
		if (_0590 != null)
		{
			څ(eTreeAction.Keyboard);
		}
	}

	private void ڋ(object _٠, EventArgs ؿ)
	{
		ڇ(eTreeAction.Keyboard);
	}

	private void ڌ()
	{
		if (ա != null)
		{
			if (ա.Cursor != (Cursor)null && ((Control)this).Cursor != ա.Cursor)
			{
				if (ժ == (Cursor)null)
				{
					ժ = ((Control)this).Cursor;
				}
				((Control)this).Cursor = ա.Cursor;
			}
			else if (ի != (Cursor)null && ((Control)this).Cursor != ի)
			{
				if (ժ == (Cursor)null)
				{
					ժ = ((Control)this).Cursor;
				}
				((Control)this).Cursor = ի;
			}
		}
		else if (ժ != (Cursor)null)
		{
			((Control)this).Cursor = ժ;
			ժ = null;
		}
	}

	private Cell ڍ(Node _0653, int و, int ى, Point _066A)
	{
		Cell result = null;
		Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, _0653, _066A);
		foreach (Cell cell in _0653.Cells)
		{
			if (cell.IsVisible)
			{
				Rectangle boundsRelative = cell.BoundsRelative;
				boundsRelative.Offset(nodeRectangle.Location);
				if (boundsRelative.Contains(و, ى))
				{
					result = cell;
					break;
				}
			}
		}
		return result;
	}

	private bool ڎ(Node _0653, MouseEventArgs ؿ, Point _066A)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Point layoutPosition = GetLayoutPosition(ؿ);
		InvokeNodeMouseMove(new TreeNodeMouseEventArgs(_0653, ؿ.Button, ؿ.Clicks, ؿ.Delta, layoutPosition.X, layoutPosition.Y));
		Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandHitTestBounds, _0653, _066A);
		bool flag = false;
		if (nodeRectangle.Contains(layoutPosition) && _0653.ExpandVisibility != eNodeExpandVisibility.Hidden)
		{
			_0653.MouseOverNodePart = _27AF._27B1;
			if (ա != null)
			{
				flag |= ٸ(null);
			}
			flag = true;
		}
		else if (_0653.MouseOverNodePart == _27AF._27B1)
		{
			_0653.MouseOverNodePart = _27AF._27B0;
			flag = true;
		}
		if (NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CommandBounds, _0653, _066A).Contains(layoutPosition))
		{
			_0653.MouseOverNodePart = _27AF.ᒝ;
			if (ա != null)
			{
				flag |= ٸ(null);
			}
			flag = true;
		}
		else if (_0653.MouseOverNodePart != _27AF._27B1)
		{
			if (_0653.MouseOverNodePart != _27AF._27B0)
			{
				_0653.MouseOverNodePart = _27AF._27B0;
				flag = true;
			}
			Cell cell = ڍ(_0653, layoutPosition.X, layoutPosition.Y, _066A);
			if (cell != null)
			{
				flag |= ٸ(cell);
			}
		}
		if (flag)
		{
			_0651(_0653);
		}
		return flag;
	}

	private void ڏ()
	{
		if (SelectedNode != null)
		{
			_0651(SelectedNode);
			((Control)this).Update();
		}
	}

	private void ڐ()
	{
		__05A2.CommandAreaWidth = տ;
		RecalcLayout();
	}

	private void ڑ(CellEditEventArgs ؿ)
	{
		if (_05AE != null)
		{
			_05AE(this, ؿ);
		}
	}

	private void ڒ(CellEditEventArgs ؿ)
	{
		if (_05AF != null)
		{
			_05AF(this, ؿ);
		}
	}

	protected internal virtual void InvokeBeforeNodeInsert(eTreeAction action, Node node, Node parentNode)
	{
		if (_05B7 != null)
		{
			TreeNodeCollectionEventArgs e = new TreeNodeCollectionEventArgs(action, node, parentNode);
			_05B7(this, e);
		}
	}

	protected internal virtual void InvokeAfterNodeInsert(eTreeAction action, Node node, Node parentNode)
	{
		if (_05B8 != null)
		{
			TreeNodeCollectionEventArgs e = new TreeNodeCollectionEventArgs(action, node, parentNode);
			_05B8(this, e);
		}
	}

	protected internal virtual void InvokeBeforeNodeRemove(eTreeAction action, Node node, Node parentNode)
	{
		if (_05B5 != null)
		{
			TreeNodeCollectionEventArgs e = new TreeNodeCollectionEventArgs(action, node, parentNode);
			_05B5(this, e);
		}
	}

	protected internal virtual void InvokeAfterNodeRemove(eTreeAction action, Node node, Node parentNode)
	{
		if (_05B6 != null)
		{
			TreeNodeCollectionEventArgs e = new TreeNodeCollectionEventArgs(action, node, parentNode);
			_05B6(this, e);
		}
	}

	protected internal virtual void NodeRemoved(eTreeAction action, Node node, Node parentNode, int indexOfRemovedNode)
	{
		InvokeAfterNodeRemove(action, node, parentNode);
		if (_0558 != null)
		{
			_0558.PaintedNodes.Clear();
		}
		if (!((Control)this).IsDisposed)
		{
			RecalcLayout();
		}
		if (!IsUpdateSuspended)
		{
			bool flag = false;
			if (node.IsSelected)
			{
				flag = true;
			}
			else
			{
				for (Node node2 = SelectedNode; node2 != null; node2 = node2.Parent)
				{
					if (node2 != null && node2 == node)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				Node node3 = parentNode;
				bool flag2 = true;
				if (parentNode != null)
				{
					if (parentNode.Nodes.Count > 0)
					{
						if (indexOfRemovedNode >= parentNode.Nodes.Count)
						{
							indexOfRemovedNode = parentNode.Nodes.Count - 1;
						}
						node3 = parentNode.Nodes[indexOfRemovedNode];
						if (node3.CanSelect)
						{
							SelectNode(node3, action);
							flag2 = false;
							node3 = null;
						}
					}
					else if (node3.CanSelect)
					{
						SelectNode(node3, action);
						flag2 = false;
						node3 = null;
					}
					if (node3 != null)
					{
						while (node3 != null)
						{
							node3 = _175C.ᝯ(node3);
							if (node3 != null && node3.CanSelect)
							{
								SelectNode(node3, action);
								flag2 = false;
								break;
							}
						}
					}
				}
				else
				{
					int num = indexOfRemovedNode;
					if (Nodes.Count == indexOfRemovedNode)
					{
						num--;
					}
					if (num > 0 && Nodes.Count > num)
					{
						for (int num2 = num; num2 >= 0; num2--)
						{
							node3 = Nodes[num2];
							if (node3.CanSelect && node3.Visible && node3.Enabled)
							{
								SelectNode(node3, action);
								flag2 = false;
								break;
							}
						}
					}
				}
				if (flag2 && !ړ(action))
				{
					SelectNode(null, action);
				}
			}
			((Control)this).Invalidate();
		}
		else if (node.IsSelected)
		{
			if (MultiSelect)
			{
				SelectedNodes.Remove(node, action);
			}
			else
			{
				SelectNode(null, action);
			}
		}
	}

	private bool ړ(eTreeAction ڃ)
	{
		foreach (Node node in Nodes)
		{
			if (node.Selectable && node.Visible && node.Enabled)
			{
				SelectNode(node, ڃ);
				if (SelectedNode == node)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected virtual void InvokeBeforeNodeDrop(TreeDragDropEventArgs e)
	{
		if (_05BB != null)
		{
			_05BB(this, e);
		}
	}

	protected virtual void InvokeAfterNodeDrop(TreeDragDropEventArgs e)
	{
		if (_05BD != null)
		{
			_05BD(this, e);
		}
	}

	protected virtual void InvokeNodeMouseDown(TreeNodeMouseEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (e.Node != null)
		{
			e.Node.InvokeNodeMouseDown(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
		}
		if (Ӗ != null)
		{
			Ӗ(this, e);
		}
	}

	protected virtual void InvokeNodeMouseUp(TreeNodeMouseEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (e.Node != null)
		{
			e.Node.InvokeNodeMouseUp(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
		}
		if (ӗ != null)
		{
			ӗ(this, e);
		}
	}

	protected virtual void InvokeNodeMouseMove(TreeNodeMouseEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (e.Node != null)
		{
			e.Node.InvokeNodeMouseMove(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
		}
		if (Ә != null)
		{
			Ә(this, e);
		}
	}

	internal void ڔ(TreeNodeMouseEventArgs ؿ)
	{
		InvokeNodeClick(ؿ);
	}

	protected virtual void InvokeNodeClick(TreeNodeMouseEventArgs e)
	{
		if (e.Node != null)
		{
			e.Node.InvokeNodeClick(this, e);
		}
		if (Ӝ != null)
		{
			Ӝ(this, e);
		}
	}

	protected virtual void InvokeNodeDoubleClick(TreeNodeMouseEventArgs e)
	{
		if (e.Node != null)
		{
			e.Node.InvokeNodeDoubleClick(this, e);
		}
		if (ӝ != null)
		{
			ӝ(this, e);
		}
	}

	protected virtual void InvokeNodeMouseEnter(TreeNodeMouseEventArgs e)
	{
		if (e.Node != null)
		{
			e.Node.InvokeNodeMouseEnter(this, e);
		}
		if (ә != null)
		{
			ә(this, e);
		}
	}

	protected virtual void InvokeNodeMouseLeave(TreeNodeMouseEventArgs e)
	{
		if (e.Node != null)
		{
			e.Node.InvokeNodeMouseLeave(this, e);
		}
		if (Ӛ != null)
		{
			Ӛ(this, e);
		}
	}

	protected virtual void InvokeNodeMouseHover(TreeNodeMouseEventArgs e)
	{
		if (e.Node != null)
		{
			e.Node.InvokeNodeMouseHover(this, e);
		}
		if (ӛ != null)
		{
			ӛ(this, e);
		}
	}

	private void ڕ(Node _0653)
	{
		if (!_0653.Expanded)
		{
			ٷ();
		}
		if (!IsUpdateSuspended)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	void INodeNotify.ExpandedChanged(Node _0653)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ڕ
		this.ڕ(_0653);
	}

	private void ږ(eView ك, eView ل)
	{
		switch (ل)
		{
		case eView.Tree:
			_0557 = new _2706(this, ((Control)this).ClientRectangle, __05A2);
			break;
		case eView.Tile:
			_0557 = new _29CB(this, ((Control)this).ClientRectangle, __05A2);
			break;
		}
		InvalidateNodesSize();
		RecalcLayout();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTileGroupLineColor()
	{
		return !ColorFunctions.IsEqual(_0609, __060A);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTileGroupLineColor()
	{
		TileGroupLineColor = _0609;
	}

	public void Save(XmlDocument document)
	{
		TreeSerializer.Save(this, document);
	}

	public void Save(string fileName)
	{
		TreeSerializer.Save(this, fileName);
	}

	public void Save(Stream outStream)
	{
		TreeSerializer.Save(this, outStream);
	}

	public void Save(TextWriter writer)
	{
		TreeSerializer.Save(this, writer);
	}

	public void Save(XmlWriter writer)
	{
		TreeSerializer.Save(this, writer);
	}

	public void Load(string fileName)
	{
		TreeSerializer.Load(this, fileName);
	}

	public void Load(Stream inStream)
	{
		TreeSerializer.Load(this, inStream);
	}

	public void Load(XmlReader reader)
	{
		TreeSerializer.Load(this, reader);
	}

	public void Load(TextReader reader)
	{
		TreeSerializer.Load(this, reader);
	}

	public void Load(XmlDocument document)
	{
		TreeSerializer.Load(this, document);
	}

	public override void Refresh()
	{
		if (!IsUpdateSuspended && !SuspendPaint)
		{
			((Control)this).Refresh();
		}
	}

	private void ڗ()
	{
		if (__060B)
		{
			return;
		}
		__060B = true;
		try
		{
			if (__060D)
			{
				__060D = false;
				if (Չ.IsSorted)
				{
					Չ._173E();
				}
			}
			if (__060C.Count <= 0)
			{
				return;
			}
			foreach (Node item in __060C)
			{
				if (item.NodesColumns.IsSorted)
				{
					item.NodesColumns._173E();
				}
			}
			__060C.Clear();
		}
		finally
		{
			__060B = false;
		}
	}

	internal void ژ()
	{
		ژ(null);
	}

	internal void ژ(Node ڙ)
	{
		if (ڙ == null)
		{
			__060D = true;
		}
		else if (!__060C.Contains(ڙ))
		{
			__060C.Add(ڙ);
		}
	}

	public void BeginUpdate()
	{
		_055B++;
	}

	public void EndUpdate()
	{
		EndUpdate(performLayoutAndRefresh: true);
	}

	public void EndUpdate(bool performLayoutAndRefresh)
	{
		if (_055B > 0)
		{
			_055B--;
		}
		if (_055B == 0 && performLayoutAndRefresh)
		{
			RecalcLayout();
			((Control)this).Invalidate(true);
		}
	}

	public Node GetNodeAt(Point p)
	{
		return GetNodeAt(p.X, p.Y);
	}

	public Node GetNodeAt(int x, int y)
	{
		return _175C._1777(this, x, y)._17C7;
	}

	public Node GetNodeAt(int x, int y, bool displayedOnly)
	{
		return _175C._1777(this, x, y, displayedOnly)._17C7;
	}

	public Node GetNodeAt(int y)
	{
		return _175C._1777(this, y)._17C7;
	}

	public Cell GetCellAt(Point p)
	{
		return GetCellAt(p.X, p.Y);
	}

	public Cell GetCellAt(int x, int y)
	{
		return GetCellAt(x, y, displayedOnly: false);
	}

	public Cell GetCellAt(int x, int y, bool displayedOnly)
	{
		Node nodeAt = GetNodeAt(x, y, displayedOnly);
		if (nodeAt != null)
		{
			return ڍ(nodeAt, x, y, _0558.Offset);
		}
		return null;
	}

	public void RecalcLayout()
	{
		if (IsUpdateSuspended)
		{
			_055C = true;
		}
		else
		{
			ښ();
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		InvalidateNodesSize();
		RecalcLayout();
		((Control)this).OnFontChanged(e);
	}

	internal void ښ()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).Bounds.IsEmpty || ((Control)this).IsDisposed)
		{
			return;
		}
		if (((Control)this).InvokeRequired)
		{
			((Control)this).Invoke((Delegate)new ᝇ(ښ));
			return;
		}
		__060E = false;
		ڗ();
		Rectangle ڜ = Rectangle.Empty;
		Rectangle ڝ = Rectangle.Empty;
		ڛ(ref ڜ, ref ڝ);
		_0557.ܒ = ڜ;
		_0557.LeftRight = ((Control)this).RtlTranslateLeftRight((LeftRightAlignment)0);
		_0557.PerformLayout();
		_055C = false;
		ڞ(ڜ, ڝ);
		if (__0612)
		{
			ڠ();
		}
		ڡ(ڢ: true);
		ڬ();
		ڦ();
		((Control)this).Invalidate(true);
	}

	private void ڛ(ref Rectangle ڜ, ref Rectangle ڝ)
	{
		ڜ = _٦();
		if (((Չ.Count > 0 && Չ._1735) || __0608 == eView.Tile) && __059D != null)
		{
			ڜ.Width -= ((Control)__059D).Width;
		}
		ڝ = ڜ;
		if (SelectionBoxStyle == eSelectionStyle.NodeMarker)
		{
			ڜ.Inflate(-ԭ, -ԭ);
		}
		else if ((DisplayRootNode != null && !DisplayRootNode.Selectable) || (Nodes.Count > 0 && !Nodes[0].Selectable))
		{
			ڜ.Height -= 2;
		}
		else
		{
			ڜ.Inflate(-1, -2);
		}
	}

	private void ڞ()
	{
		Rectangle ڜ = Rectangle.Empty;
		Rectangle ڝ = Rectangle.Empty;
		ڛ(ref ڜ, ref ڝ);
		ڞ(ڜ, ڝ);
	}

	private void ڞ(Rectangle ڜ, Rectangle ڝ)
	{
		Size size = GetScreenRectangle(new Rectangle(0, 0, _0557.ܐ, _0557._0711)).Size;
		if (size.Width > ڝ.Width || size.Height > ڝ.Height)
		{
			Size size2 = size;
			if (SelectionBoxStyle == eSelectionStyle.NodeMarker)
			{
				size2.Width += ԭ * 2;
				size2.Height += ԭ * 2;
			}
			else
			{
				size2.Height += 2;
			}
			Rectangle rectangle = ڜ;
			if ((size.Height > rectangle.Height && __0614) || (size.Width > rectangle.Width && size.Height > rectangle.Height - SystemInformation.HorizontalScrollBarHeight))
			{
				size2.Width += SystemInformation.VerticalScrollBarWidth;
			}
			if (__0615)
			{
				size2.Height += SystemInformation.HorizontalScrollBarHeight;
			}
			if (!AutoScroll)
			{
				BeginUpdate();
				((Control)this).Invalidate();
				AutoScroll = true;
				AutoScrollMinSize = size2;
				AutoScrollPosition = _0558.DefaultOffset;
				if ((Չ.Count > 0 && Չ._1735) || __0608 == eView.Tile)
				{
					InvalidateNodesSize();
					_0654();
				}
				EndUpdate(performLayoutAndRefresh: false);
			}
			else if (AutoScrollMinSize != size2)
			{
				BeginUpdate();
				AutoScrollMinSize = size2;
				if (size.Width <= ((Control)this).Bounds.Width && AutoScrollPosition.X != 0)
				{
					AutoScrollPosition = new Point(0, AutoScrollPosition.Y);
				}
				Point autoScrollPosition = AutoScrollPosition;
				if (Math.Abs(autoScrollPosition.Y) > size2.Height)
				{
					if (size2.Height > rectangle.Height)
					{
						autoScrollPosition.Y = -(size2.Height - rectangle.Height);
					}
					else
					{
						autoScrollPosition.Y = 0;
					}
				}
				else if (size.Height <= ((Control)this).Bounds.Height)
				{
					autoScrollPosition.Y = 0;
				}
				else if (autoScrollPosition.Y < 0 && size2.Height - Math.Abs(autoScrollPosition.Y) < ڜ.Height)
				{
					autoScrollPosition.Y = -(size2.Height - rectangle.Height);
				}
				if (Math.Abs(autoScrollPosition.X) > size2.Width)
				{
					autoScrollPosition.X = -(size2.Width - rectangle.Width);
				}
				else if (size.Width <= ((Control)this).Bounds.Width)
				{
					autoScrollPosition.X = 0;
				}
				if (AutoScrollPosition != autoScrollPosition)
				{
					AutoScrollPosition = autoScrollPosition;
				}
				EndUpdate(performLayoutAndRefresh: false);
			}
			else if (__0619.X != 0 && size.Width <= ڝ.Width)
			{
				AutoScrollPosition = new Point(0, AutoScrollPosition.Y);
			}
		}
		else if (AutoScroll)
		{
			BeginUpdate();
			bool flag = false;
			if (Չ.Count > 0 && __059D != null)
			{
				if (Չ._1735)
				{
					InvalidateNodesSize();
				}
				else
				{
					flag = true;
				}
			}
			AutoScroll = false;
			_0558.Offset = _0558.DefaultOffset;
			if (flag)
			{
				_0557.UpdateTopLevelColumnsWidth();
			}
			EndUpdate(performLayoutAndRefresh: false);
		}
		else
		{
			_0558.Offset = _0558.DefaultOffset;
		}
	}

	private void ڟ()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		MethodInvoker val = null;
		if (__0611 == null)
		{
			return;
		}
		if (__05F6 > 0)
		{
			if (__0613)
			{
				return;
			}
			__0613 = true;
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					ڟ();
				};
			}
			ڣ(val);
		}
		else
		{
			__0613 = false;
			PanelControl panelControl = __0611;
			__0611 = null;
			((Control)this).Controls.Remove((Control)(object)panelControl);
			((Component)(object)panelControl).Dispose();
		}
	}

	private void ڠ()
	{
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		MethodInvoker val = null;
		if (!__060F)
		{
			ڟ();
			return;
		}
		if (_059A.Count > 0)
		{
			Rectangle rectangle = _٦();
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (clientRectangle.Contains(rectangle) && rectangle != clientRectangle && rectangle.X > clientRectangle.X && rectangle.Right < clientRectangle.Right && rectangle.Y > clientRectangle.Y && rectangle.Bottom < clientRectangle.Bottom)
			{
				if (__0611 == null)
				{
					if (__05F6 > 0)
					{
						if (__0613)
						{
							return;
						}
						__0613 = true;
						if (val == null)
						{
							val = (MethodInvoker)delegate
							{
								ڠ();
							};
						}
						ڣ(val);
						return;
					}
					__0613 = false;
					__0611 = new PanelControl();
					__0611.ᑱ = false;
					__0611.Style.ApplyStyle(BackgroundStyle);
					__0611.Style.Class = BackgroundStyle.Class;
					((Control)this).Controls.Add((Control)(object)__0611);
				}
				((Control)__0611).Bounds = clientRectangle;
				Region val2 = new Region(clientRectangle);
				val2.Exclude(rectangle);
				((Control)__0611).Region = val2;
				((Control)__0611).BringToFront();
			}
			else
			{
				ڟ();
			}
		}
		else
		{
			ڟ();
		}
		__0612 = false;
	}

	private void ڡ(bool ڢ)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		if (__0610)
		{
			return;
		}
		try
		{
			__0610 = true;
			if (_059A.Count > 0)
			{
				((Control)this).SuspendLayout();
				if (AutoScroll)
				{
					_0558.Offset = _٧();
				}
				_0558.MoveHostedControls();
				((Control)this).ResumeLayout(ڢ);
			}
			else if (__0611 != null)
			{
				ڟ();
			}
			if (_058F && __0606 is Control && _0590 != null)
			{
				Point _066A = _0558.Offset;
				if (AutoScroll)
				{
					_066A = _٧();
				}
				Control val = (Control)__0606;
				Rectangle r = NodeDisplay._070A(_1757._175A, _0590, _066A);
				val.Location = GetScreenRectangle(r).Location;
			}
		}
		finally
		{
			__0610 = false;
		}
	}

	private void ڣ(MethodInvoker ڤ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = ڤ;
		val.Interval = 10;
		val.Tick += ڥ;
		val.Start();
	}

	private void ڥ(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)_٠;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}

	private void ڦ()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		MethodInvoker val = null;
		if (__05F6 > 0)
		{
			if (__061A)
			{
				return;
			}
			__061A = true;
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					ڦ();
				};
			}
			ڣ(val);
			return;
		}
		__061A = false;
		if (!__0616)
		{
			bool flag = __059E != null || __059D != null;
			ڭ();
			ڮ();
			if (__059F != null)
			{
				((Control)this).Controls.Remove(__059F);
				((Component)(object)__059F).Dispose();
				__059F = null;
			}
			if (flag)
			{
				((Control)this).Update();
			}
			return;
		}
		Rectangle rectangle = ᝈ._1756(BackgroundStyle, ((Control)this).ClientRectangle);
		Size size = __0617;
		if (__0614 && size.Height > rectangle.Height)
		{
			if (__059D == null)
			{
				__059D = new VScrollBarAdv();
				__059D.Appearance = _ז;
				((Control)__059D).Width = ڨ();
				((Control)this).Controls.Add((Control)(object)__059D);
				((Control)__059D).BringToFront();
				__059D.Scroll += new ScrollEventHandler(ک);
			}
			if (__059D.Minimum != 0)
			{
				__059D.Minimum = 0;
			}
			int num = rectangle.Height - 12;
			if (__05A0 != null && ((Control)__05A0).Visible)
			{
				num -= ((Control)__05A0).Height + 2;
			}
			if (__059D.LargeChange != num && num > 0)
			{
				__059D.LargeChange = num;
			}
			__059D.SmallChange = (int)Math.Ceiling(((Control)this).Font.GetHeight());
			if (__059D.Maximum != __0617.Height)
			{
				__059D.Maximum = __0617.Height;
			}
			if (__059D.Value != -__0619.Y)
			{
				__059D.Value = Math.Min(__059D.Maximum, Math.Abs(__0619.Y));
			}
		}
		else
		{
			ڮ();
		}
		if (__0615 && size.Width > rectangle.Width)
		{
			if (__059E == null)
			{
				__059E = new HScrollBarAdv();
				__059E.Appearance = _ז;
				((Control)__059E).Height = ڧ();
				((Control)this).Controls.Add((Control)(object)__059E);
				((Control)__059E).BringToFront();
				__059E.Scroll += new ScrollEventHandler(ڪ);
			}
			if (__059E.Minimum != 0)
			{
				__059E.Minimum = 0;
			}
			if (__059E.LargeChange != rectangle.Width && rectangle.Width > 0)
			{
				__059E.LargeChange = rectangle.Width;
			}
			if (__059E.Maximum != __0617.Width)
			{
				__059E.Maximum = __0617.Width;
			}
			if (__059E.Value != -__0619.X)
			{
				__059E.Value = Math.Min(__059E.Maximum, Math.Abs(__0619.X));
			}
			__059E.SmallChange = (int)Math.Ceiling(((Control)this).Font.GetHeight());
		}
		else
		{
			ڭ();
		}
		ګ();
		if (__059F != null)
		{
			__059F.BringToFront();
		}
	}

	private static int ڧ()
	{
		return SystemInformation.HorizontalScrollBarHeight;
	}

	private static int ڨ()
	{
		return SystemInformation.VerticalScrollBarWidth;
	}

	private void ک(object _٠, ScrollEventArgs ؿ)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		if (ؿ.NewValue != ؿ.OldValue)
		{
			__0619.Y = -ؿ.NewValue;
			ڡ(ڢ: false);
			((Control)this).Invalidate();
			if (__05A0 != null)
			{
				((Control)__05A0).Invalidate();
			}
			if ((int)ؿ.Type == 5 && _059A.Count > 0)
			{
				((Control)this).Update();
			}
		}
		((ScrollableControl)this).OnScroll(new ScrollEventArgs(ؿ.Type, ؿ.OldValue, ؿ.NewValue, (ScrollOrientation)1));
	}

	private void ڪ(object _٠, ScrollEventArgs ؿ)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		__0619.X = -ؿ.NewValue;
		ڡ(ڢ: false);
		((Control)this).Invalidate();
		if (__05A0 != null)
		{
			((Control)__05A0).Invalidate();
		}
		if ((int)ؿ.Type == 5 && _059A.Count > 0)
		{
			((Control)this).Update();
		}
		((ScrollableControl)this).OnScroll(new ScrollEventArgs(ؿ.Type, ؿ.OldValue, ؿ.NewValue, (ScrollOrientation)0));
	}

	private void ګ()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		Rectangle rectangle = ᝈ._1756(BackgroundStyle, ((Control)this).ClientRectangle);
		if (__059E != null)
		{
			int num = rectangle.Width;
			if (__059D != null)
			{
				num -= ((Control)__059D).Width;
			}
			((Control)__059E).Bounds = new Rectangle(rectangle.X, rectangle.Height - ڧ() + 1, num, ڧ());
		}
		if (__059D != null)
		{
			int num2 = rectangle.Height;
			if (__059E != null)
			{
				num2 -= ((Control)__059E).Height;
			}
			((Control)__059D).Bounds = new Rectangle(rectangle.Right - ڨ(), rectangle.Y, ڨ(), num2);
		}
		if (__059D != null && __059E != null)
		{
			if (__059F == null)
			{
				__059F = new Control();
				__059F.BackColor = ((Control)this).BackColor;
				((Control)this).Controls.Add(__059F);
			}
			__059F.Bounds = new Rectangle(((Control)__059E).Bounds.Right, ((Control)__059D).Bounds.Bottom, ((Control)__059D).Width, ((Control)__059E).Height);
		}
		else if (__059F != null)
		{
			((Control)this).Controls.Remove(__059F);
			((Component)(object)__059F).Dispose();
			__059F = null;
		}
		ڬ();
	}

	private void ڬ()
	{
		Rectangle rectangle = ᝈ._1756(BackgroundStyle, ((Control)this).ClientRectangle);
		if (__05A0 != null)
		{
			Rectangle screenRectangle = GetScreenRectangle(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - ((__059D != null) ? ((Control)__059D).Width : 0), __05A0.យ.Bounds.Height));
			screenRectangle.X = rectangle.X;
			screenRectangle.Y = rectangle.Y;
			screenRectangle.Width = Math.Max(0, rectangle.Width - ((__059D != null) ? ((Control)__059D).Width : 0));
			screenRectangle.Height = Math.Max(0, screenRectangle.Height);
			((Control)__05A0).Bounds = screenRectangle;
			((Control)__05A0).BringToFront();
		}
	}

	private void ڭ()
	{
		if (__059E != null)
		{
			Rectangle bounds = ((Control)__059E).Bounds;
			((Control)this).Controls.Remove((Control)(object)__059E);
			((Component)(object)__059E).Dispose();
			__059E = null;
			((Control)this).Invalidate(bounds);
		}
	}

	private void ڮ()
	{
		if (__059D != null)
		{
			Rectangle bounds = ((Control)__059D).Bounds;
			((Control)this).Controls.Remove((Control)(object)__059D);
			((Component)(object)__059D).Dispose();
			__059D = null;
			((Control)this).Invalidate(bounds);
		}
	}

	private Matrix گ(float ڰ)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Matrix val = new Matrix(ڰ, 0f, 0f, ڰ, 0f, 0f);
		if (Ԭ)
		{
			float num = 0f;
			float num2 = 0f;
			if (AutoScroll)
			{
				if (((Control)this).Width > Ԫ.ܐ)
				{
					num = ((!((float)((Control)this).Width < (float)Ԫ.ܐ * ڰ)) ? (((float)((Control)this).Width * (1f / ڰ) - (float)((Control)this).Width) / 2f) : ((float)(-(((Control)this).Width - Ԫ.ܐ) / 2)));
				}
				if (((Control)this).Height > Ԫ._0711)
				{
					num2 = ((!((float)((Control)this).Height < (float)Ԫ._0711 * ڰ)) ? (((float)((Control)this).Height * (1f / ڰ) - (float)((Control)this).Height) / 2f) : ((float)(-(((Control)this).Height - Ԫ._0711) / 2)));
				}
			}
			else
			{
				num = ((float)((Control)this).Width * (1f / ڰ) - (float)((Control)this).Width) / 2f;
				num2 = ((float)((Control)this).Height * (1f / ڰ) - (float)((Control)this).Height) / 2f;
			}
			val.Translate(num, num2);
		}
		return val;
	}

	public Matrix GetTranslationMatrix()
	{
		return گ(_059B);
	}

	public virtual Rectangle GetLayoutRectangle(Rectangle r)
	{
		if (_059B == 1f)
		{
			return r;
		}
		Point[] array = new Point[2]
		{
			new Point(r.X, r.Y),
			new Point(r.Right, r.Bottom)
		};
		Matrix translationMatrix = GetTranslationMatrix();
		try
		{
			translationMatrix.Invert();
			translationMatrix.TransformPoints(array);
		}
		finally
		{
			((IDisposable)translationMatrix)?.Dispose();
		}
		return new Rectangle(array[0].X, array[0].Y, array[1].X - array[0].X, array[1].Y - array[0].Y);
	}

	protected virtual Point GetLayoutPosition(MouseEventArgs e)
	{
		return GetLayoutPosition(e.X, e.Y);
	}

	public virtual Point GetLayoutPosition(Point mousePosition)
	{
		return GetLayoutPosition(mousePosition.X, mousePosition.Y);
	}

	public virtual Point GetLayoutPosition(int x, int y)
	{
		if (_059B == 1f)
		{
			return new Point(x, y);
		}
		Point[] array = new Point[1]
		{
			new Point(x, y)
		};
		Matrix translationMatrix = GetTranslationMatrix();
		try
		{
			translationMatrix.Invert();
			translationMatrix.TransformPoints(array);
		}
		finally
		{
			((IDisposable)translationMatrix)?.Dispose();
		}
		return array[0];
	}

	public virtual Rectangle GetScreenRectangle(Rectangle r)
	{
		if (_059B == 1f)
		{
			return r;
		}
		Point[] array = new Point[2]
		{
			new Point(r.X, r.Y),
			new Point(r.Right, r.Bottom)
		};
		Matrix translationMatrix = GetTranslationMatrix();
		try
		{
			translationMatrix.TransformPoints(array);
		}
		finally
		{
			((IDisposable)translationMatrix)?.Dispose();
		}
		return new Rectangle(array[0].X, array[0].Y, array[1].X - array[0].X, array[1].Y - array[0].Y);
	}

	public virtual Size GetScreenSize(Size s)
	{
		return GetScreenRectangle(new Rectangle(Point.Empty, s)).Size;
	}

	internal void ڱ(object _٠, MouseEventArgs ؿ)
	{
		OnColumnHeaderMouseUp(_٠, ؿ);
	}

	protected virtual void OnColumnHeaderMouseUp(object sender, MouseEventArgs e)
	{
		if (_05A5 != null)
		{
			_05A5.Invoke(sender, e);
		}
	}

	internal void ڲ(object _٠, MouseEventArgs ؿ)
	{
		OnColumnHeaderMouseDown(_٠, ؿ);
	}

	protected virtual void OnColumnHeaderMouseDown(object sender, MouseEventArgs e)
	{
		if (_05A4 != null)
		{
			_05A4.Invoke(sender, e);
		}
	}

	private static void ڳ(List<Node> ڴ, NodeCollection ڵ)
	{
		foreach (Node item in ڵ)
		{
			if (item.Checked)
			{
				ڴ.Add(item);
			}
			if (item.HasChildNodes)
			{
				ڳ(ڴ, item.Nodes);
			}
		}
	}

	internal void ڶ(AdvTreeCellBeforeCheckEventArgs ؿ)
	{
		OnBeforeCheck(ؿ);
	}

	protected virtual void OnBeforeCheck(AdvTreeCellBeforeCheckEventArgs e)
	{
		if (_05A7 != null)
		{
			_05A7(this, e);
		}
	}

	internal void ڷ(AdvTreeCellEventArgs ؿ)
	{
		OnAfterCheck(ؿ);
	}

	protected virtual void OnAfterCheck(AdvTreeCellEventArgs e)
	{
		if (_05A6 != null)
		{
			_05A6(this, e);
		}
	}

	internal void ڸ(Node _0653, CommandButtonEventArgs ؿ)
	{
		if (_05AC != null)
		{
			_05AC(_0653, ؿ);
		}
	}

	protected virtual void OnBeforeCellEdit(CellEditEventArgs e)
	{
		if (_05AD != null)
		{
			_05AD(this, e);
		}
	}

	private void ڹ(AdvTreeNodeCancelEventArgs ؿ)
	{
		if (_05A9 != null)
		{
			_05A9(this, ؿ);
		}
	}

	void INodeNotify.OnBeforeCollapse(AdvTreeNodeCancelEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ڹ
		this.ڹ(ؿ);
	}

	private void ں(AdvTreeNodeEventArgs ؿ)
	{
		if (_05A8 != null)
		{
			_05A8(this, ؿ);
		}
	}

	void INodeNotify.OnAfterCollapse(AdvTreeNodeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ں
		this.ں(ؿ);
	}

	private void ڻ(AdvTreeNodeCancelEventArgs ؿ)
	{
		if (_05AB != null)
		{
			_05AB(this, ؿ);
		}
	}

	void INodeNotify.OnBeforeExpand(AdvTreeNodeCancelEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ڻ
		this.ڻ(ؿ);
	}

	private void ڼ(AdvTreeNodeEventArgs ؿ)
	{
		if (_05AA != null)
		{
			_05AA(this, ؿ);
		}
	}

	void INodeNotify.OnAfterExpand(AdvTreeNodeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ڼ
		this.ڼ(ؿ);
	}

	internal void ڽ(AdvTreeNodeEventArgs ھ)
	{
		OnAfterNodeDeselect(ھ);
	}

	protected virtual void OnAfterNodeDeselect(AdvTreeNodeEventArgs args)
	{
		if (_05B4 != null)
		{
			_05B4(this, args);
		}
	}

	internal void ڿ(AdvTreeNodeEventArgs ھ)
	{
		OnAfterNodeSelect(ھ);
	}

	protected virtual void OnAfterNodeSelect(AdvTreeNodeEventArgs args)
	{
		if (_05B3 != null)
		{
			_05B3(this, args);
		}
	}

	internal void ۀ(AdvTreeNodeCancelEventArgs ھ)
	{
		OnBeforeNodeSelect(ھ);
	}

	protected virtual void OnBeforeNodeSelect(AdvTreeNodeCancelEventArgs args)
	{
		if (_05B2 != null)
		{
			_05B2(this, args);
		}
	}

	protected virtual void OnDeserializeNode(SerializeNodeEventArgs e)
	{
		if (_05BF != null)
		{
			_05BF(this, e);
		}
	}

	protected virtual void OnSerializeNode(SerializeNodeEventArgs e)
	{
		if (_05BE != null)
		{
			_05BE(this, e);
		}
	}

	internal void ہ(SerializeNodeEventArgs ؿ)
	{
		OnSerializeNode(ؿ);
	}

	internal void ۂ(SerializeNodeEventArgs ؿ)
	{
		OnDeserializeNode(ؿ);
	}

	internal void ۃ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		OnMarkupLinkClick(_٠, ؿ);
		բ = 0;
	}

	protected virtual void OnMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(sender, e);
		}
	}

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetCursorPos(int X, int Y);

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		((Control)this).OnDragDrop(drgevent);
		InternalDragDrop(drgevent);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalDragDrop(DragEventArgs drgevent)
	{
		ێ(ۏ: true);
	}

	protected override void OnDragLeave(EventArgs e)
	{
		((Control)this).OnDragLeave(e);
		InternalDragLeave();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalDragLeave()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		ێ(ۏ: false);
		if (AutoScroll && __061B == null)
		{
			__061B = new Timer();
			__061B.Interval = 100;
			__061B.Tick += ۅ;
			__061B.Start();
		}
	}

	private void ۄ()
	{
		Timer val = __061B;
		if (val != null)
		{
			__061B = null;
			val.Stop();
			val.Tick -= ۅ;
			((Component)(object)val).Dispose();
		}
	}

	private void ۅ(object _٠, EventArgs ؿ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		if ((int)Control.MouseButtons == 1048576 && ((Control)this).IsHandleCreated && AutoScroll)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(32, 32);
			if (clientRectangle.Contains(pt))
			{
				if (pt.Y < 0 && AutoScrollPosition.Y != 0 && __059D != null)
				{
					AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Min(0, AutoScrollPosition.Y + __059D.SmallChange));
				}
				else if (__059D != null && pt.Y > ((Control)this).ClientRectangle.Bottom && -(AutoScrollPosition.Y - __059D.SmallChange) < __059D.Maximum - __059D.LargeChange)
				{
					AutoScrollPosition = new Point(AutoScrollPosition.X, Math.Min(0, AutoScrollPosition.Y - __059D.SmallChange));
				}
			}
		}
		else
		{
			ۄ();
		}
	}

	protected override void OnDragEnter(DragEventArgs drgevent)
	{
		ۄ();
		((Control)this).OnDragEnter(drgevent);
	}

	protected override void OnDragOver(DragEventArgs drgevent)
	{
		((Control)this).OnDragOver(drgevent);
		InternalDragOver(drgevent);
	}

	protected virtual void OnNodeDragFeedback(TreeDragFeedbackEventArgs e)
	{
		_05BC?.Invoke(this, e);
	}

	private bool ۆ(Node[] _0652, Node ۇ)
	{
		foreach (Node node in _0652)
		{
			if (node == ۇ)
			{
				return true;
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InternalDragOver(DragEventArgs drgevent)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		if (!ӽ)
		{
			return;
		}
		if (!__061E && ې(drgevent))
		{
			drgevent.Effect = (DragDropEffects)0;
			return;
		}
		if (_0592 == null)
		{
			ۑ(drgevent);
		}
		if (_0592 == null)
		{
			drgevent.Effect = (DragDropEffects)0;
			return;
		}
		drgevent.Effect = (DragDropEffects)(((int)Control.ModifierKeys == 131072 && __061D) ? 1 : 2);
		_0651(_0592);
		Point mousePosition = ((Control)this).PointToClient(new Point(drgevent.X, drgevent.Y));
		mousePosition = GetLayoutPosition(mousePosition);
		_17C9 obj = _175C._177A(this, mousePosition.X, mousePosition.Y);
		_ = Point.Empty;
		_27B3 _27B32 = null;
		Node node = null;
		if (obj != null && obj._17C7 == null && ((Control)this).ClientRectangle.Contains(mousePosition.X, mousePosition.Y))
		{
			node = _175C.ᝦ(this);
			if (node != null && mousePosition.Y >= node.Bounds.Y)
			{
				obj._17C7 = node;
			}
		}
		if (obj != null && obj._17C7 != null && !ۆ((Node[])_0592.Tag, obj._17C7) && !_175C.ខ((Node[])_0592.Tag, obj._17C7))
		{
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, obj._17C7, _0558.Offset);
			Rectangle nodeRectangle2 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, obj._17C7, _0558.Offset);
			if (__0608 == eView.Tile)
			{
				if (obj._17C7 == node)
				{
					MultiNodeTreeDragFeedbackEventArgs multiNodeTreeDragFeedbackEventArgs = new MultiNodeTreeDragFeedbackEventArgs(null, Nodes.Count, (Node[])_0592.Tag, drgevent.Effect);
					OnNodeDragFeedback(multiNodeTreeDragFeedbackEventArgs);
					if (multiNodeTreeDragFeedbackEventArgs.AllowDrop)
					{
						_27B32 = new _27B3(multiNodeTreeDragFeedbackEventArgs.ParentNode, multiNodeTreeDragFeedbackEventArgs.InsertPosition);
						if (multiNodeTreeDragFeedbackEventArgs._29B6)
						{
							drgevent.Effect = multiNodeTreeDragFeedbackEventArgs.Effect;
						}
					}
					else
					{
						drgevent.Effect = (DragDropEffects)0;
					}
				}
				else if (obj._17C7.HasChildNodes || obj._17C7.Parent == null)
				{
					MultiNodeTreeDragFeedbackEventArgs multiNodeTreeDragFeedbackEventArgs2 = new MultiNodeTreeDragFeedbackEventArgs(obj._17C7, -1, (Node[])_0592.Tag, drgevent.Effect);
					OnNodeDragFeedback(multiNodeTreeDragFeedbackEventArgs2);
					if (multiNodeTreeDragFeedbackEventArgs2.AllowDrop)
					{
						_27B32 = new _27B3(multiNodeTreeDragFeedbackEventArgs2.ParentNode, multiNodeTreeDragFeedbackEventArgs2.InsertPosition);
						if (multiNodeTreeDragFeedbackEventArgs2._29B6)
						{
							drgevent.Effect = multiNodeTreeDragFeedbackEventArgs2.Effect;
						}
					}
					else
					{
						drgevent.Effect = (DragDropEffects)0;
					}
				}
				else
				{
					TreeDragFeedbackEventArgs treeDragFeedbackEventArgs = null;
					treeDragFeedbackEventArgs = ((mousePosition.X < nodeRectangle.X + nodeRectangle.Width / 3) ? new MultiNodeTreeDragFeedbackEventArgs(obj._17C7.Parent, obj._17C7.Index - 1, (Node[])_0592.Tag, drgevent.Effect) : ((mousePosition.Y < nodeRectangle2.Y) ? new MultiNodeTreeDragFeedbackEventArgs(obj._17C7.Parent, obj._17C7.Index + 1, (Node[])_0592.Tag, drgevent.Effect) : ((!obj._17C7.Expanded || obj._17C7.Nodes.Count <= 0) ? new MultiNodeTreeDragFeedbackEventArgs(obj._17C7.Parent, obj._17C7.Index, (Node[])_0592.Tag, drgevent.Effect) : new MultiNodeTreeDragFeedbackEventArgs(obj._17C7, -1, (Node[])_0592.Tag))));
					OnNodeDragFeedback(treeDragFeedbackEventArgs);
					if (treeDragFeedbackEventArgs.AllowDrop)
					{
						_27B32 = new _27B3(treeDragFeedbackEventArgs.ParentNode, treeDragFeedbackEventArgs.InsertPosition);
						if (treeDragFeedbackEventArgs._29B6)
						{
							drgevent.Effect = treeDragFeedbackEventArgs.Effect;
						}
					}
					else
					{
						drgevent.Effect = (DragDropEffects)0;
					}
				}
			}
			else if (mousePosition.X > nodeRectangle2.X + __061C)
			{
				MultiNodeTreeDragFeedbackEventArgs multiNodeTreeDragFeedbackEventArgs3 = new MultiNodeTreeDragFeedbackEventArgs(obj._17C7, -1, (Node[])_0592.Tag, drgevent.Effect);
				OnNodeDragFeedback(multiNodeTreeDragFeedbackEventArgs3);
				if (multiNodeTreeDragFeedbackEventArgs3.AllowDrop)
				{
					_27B32 = new _27B3(multiNodeTreeDragFeedbackEventArgs3.ParentNode, multiNodeTreeDragFeedbackEventArgs3.InsertPosition);
					if (multiNodeTreeDragFeedbackEventArgs3._29B6)
					{
						drgevent.Effect = multiNodeTreeDragFeedbackEventArgs3.Effect;
					}
				}
				else
				{
					drgevent.Effect = (DragDropEffects)0;
				}
			}
			else
			{
				TreeDragFeedbackEventArgs treeDragFeedbackEventArgs2 = null;
				treeDragFeedbackEventArgs2 = ((mousePosition.Y < nodeRectangle.Y + nodeRectangle.Height / 3) ? new MultiNodeTreeDragFeedbackEventArgs(obj._17C7.Parent, obj._17C7.Index - 1, (Node[])_0592.Tag, drgevent.Effect) : ((mousePosition.X < nodeRectangle2.X) ? new MultiNodeTreeDragFeedbackEventArgs(obj._17C7.Parent, obj._17C7.Index + 1, (Node[])_0592.Tag, drgevent.Effect) : ((!obj._17C7.Expanded || obj._17C7.Nodes.Count <= 0) ? new MultiNodeTreeDragFeedbackEventArgs(obj._17C7.Parent, obj._17C7.Index, (Node[])_0592.Tag, drgevent.Effect) : new MultiNodeTreeDragFeedbackEventArgs(obj._17C7, -1, (Node[])_0592.Tag))));
				OnNodeDragFeedback(treeDragFeedbackEventArgs2);
				if (treeDragFeedbackEventArgs2.AllowDrop)
				{
					_27B32 = new _27B3(treeDragFeedbackEventArgs2.ParentNode, treeDragFeedbackEventArgs2.InsertPosition);
					if (treeDragFeedbackEventArgs2._29B6)
					{
						drgevent.Effect = treeDragFeedbackEventArgs2.Effect;
					}
				}
				else
				{
					drgevent.Effect = (DragDropEffects)0;
				}
			}
		}
		else if (Ӥ.Count == 0)
		{
			MultiNodeTreeDragFeedbackEventArgs multiNodeTreeDragFeedbackEventArgs4 = new MultiNodeTreeDragFeedbackEventArgs(null, 0, (Node[])_0592.Tag, drgevent.Effect);
			OnNodeDragFeedback(multiNodeTreeDragFeedbackEventArgs4);
			if (multiNodeTreeDragFeedbackEventArgs4.AllowDrop)
			{
				_27B32 = new _27B3(null, 0);
				if (multiNodeTreeDragFeedbackEventArgs4._29B6)
				{
					drgevent.Effect = multiNodeTreeDragFeedbackEventArgs4.Effect;
				}
			}
			else
			{
				drgevent.Effect = (DragDropEffects)0;
			}
		}
		else
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(32, 32);
			if (AutoScroll && clientRectangle.Contains(mousePosition))
			{
				if (mousePosition.Y < 0)
				{
					if (__059D != null && __059D.Value > 0)
					{
						__059D.Value -= __059D.SmallChange;
					}
				}
				else if (mousePosition.Y > ((Control)this).ClientRectangle.Bottom && __059D != null && __059D.Value + __059D.SmallChange < __059D.Maximum)
				{
					__059D.Value += __059D.SmallChange;
				}
			}
			drgevent.Effect = (DragDropEffects)0;
		}
		bool flag = false;
		if (_27B32 != null)
		{
			if (__061F == null)
			{
				__061F = _27B32;
				ۈ(__061F);
				flag = true;
			}
			else if (__061F.ކ != _27B32.ކ || __061F._27B4 != _27B32._27B4)
			{
				ۈ(__061F);
				ۈ(_27B32);
				__061F = _27B32;
				flag = true;
			}
		}
		else if (_27B32 != __061F)
		{
			ۈ(__061F);
			__061F = null;
			flag = true;
		}
		_0592.SetBounds(new Rectangle(mousePosition.X, mousePosition.Y, _0592.BoundsRelative.Width, _0592.BoundsRelative.Height));
		_0592.Visible = true;
		_0651(_0592);
		if (flag)
		{
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
			else
			{
				((Control)this).Update();
			}
		}
	}

	private void ۈ(_27B3 ۉ)
	{
		if (ۉ != null)
		{
			Rectangle rectangle = ۋ(ۉ);
			if (!rectangle.IsEmpty)
			{
				rectangle.Inflate(1, 1);
				((Control)this).Invalidate(rectangle);
			}
		}
	}

	private Node ۊ(_27B3 ۉ)
	{
		Node result = null;
		NodeCollection nodeCollection = null;
		nodeCollection = ((ۉ.ކ == null) ? Nodes : ۉ.ކ.Nodes);
		if (nodeCollection.Count > 0)
		{
			result = ((ۉ._27B4 <= 0) ? nodeCollection[0] : ((ۉ._27B4 < nodeCollection.Count) ? nodeCollection[ۉ._27B4] : nodeCollection[nodeCollection.Count - 1]));
		}
		return result;
	}

	internal Rectangle ۋ(_27B3 ۉ)
	{
		if (ۉ == null)
		{
			return Rectangle.Empty;
		}
		Rectangle result = Rectangle.Empty;
		Node node = ۊ(ۉ);
		Node ކ = ۉ.ކ;
		if (__0608 == eView.Tile)
		{
			if (node != null)
			{
				result = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, _0558.Offset);
				if (ۉ._27B4 == -1)
				{
					result.X -= 4;
				}
				else
				{
					result.X = result.Right - ؠ;
				}
				result.Height = node.Bounds.Height;
				result.Width = ؠ;
			}
			else if (ކ != null)
			{
				result = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, ކ, _0558.Offset);
				result.X += Indent;
				if (!ކ.HasChildNodes)
				{
					result.Y = result.Bottom - ؠ;
				}
				result.Width = ކ.Bounds.Width;
				result.Height = ؠ;
			}
		}
		else
		{
			if (node != null)
			{
				result = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, _0558.Offset);
				result.Width = node.Bounds.Width;
				if (node.Expanded && ۉ._27B4 >= 0)
				{
					result.Y = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds, node, _0558.Offset).Bottom - result.Height;
				}
			}
			else if (ކ != null)
			{
				result = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, ކ, _0558.Offset);
				result.X += Indent;
				result.Width = ކ.Bounds.Width;
			}
			if (ۉ._27B4 < 0 && node != null)
			{
				result.Y -= 4;
			}
			else
			{
				result.Y = result.Bottom - 2;
			}
			result.Height = ؠ;
		}
		return result;
	}

	private Node[] ی(Node[] ۍ)
	{
		ArrayList arrayList = new ArrayList(ۍ);
		arrayList.Sort(new NodeFlatIndexComparer(this));
		arrayList.CopyTo(ۍ);
		return ۍ;
	}

	private void ێ(bool ۏ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (_0592 == null)
		{
			return;
		}
		bool isCopy = (int)Control.ModifierKeys == 131072 && __061D;
		BeginUpdate();
		try
		{
			if (_0558.LockOffset)
			{
				_0558.LockOffset = false;
			}
			Node[] array = _0592.Tag as Node[];
			Node[] array2 = array;
			foreach (Node node in array2)
			{
				if (!node.Visible)
				{
					node.Visible = true;
				}
			}
			if (ۏ && __061F != null)
			{
				Node ކ = __061F.ކ;
				TreeDragDropEventArgs treeDragDropEventArgs = new TreeDragDropEventArgs(eTreeAction.Mouse, array, array[0].Parent, ކ, isCopy, __061F._27B4);
				InvokeBeforeNodeDrop(treeDragDropEventArgs);
				if (!treeDragDropEventArgs.Cancel && treeDragDropEventArgs.Nodes != null && treeDragDropEventArgs.Nodes.Length > 0)
				{
					isCopy = treeDragDropEventArgs.IsCopy;
					array = treeDragDropEventArgs.Nodes;
					int num = treeDragDropEventArgs.InsertPosition + 1;
					if (array.Length > 1)
					{
						array = ی(array);
					}
					Node[] array3 = new Node[array.Length];
					for (int j = 0; j < array.Length; j++)
					{
						Node node2 = array[j];
						Node node3 = node2;
						if (_175C.ខ(array, node3))
						{
							continue;
						}
						if (isCopy)
						{
							node3 = node3.DeepCopy();
						}
						else
						{
							if (ކ != null)
							{
								if (node3.Parent == ކ && ކ.Nodes.IndexOf(node3) < num)
								{
									num--;
								}
							}
							else if (node3.Parent == null && node3.TreeControl == this && Nodes.IndexOf(node3) < num)
							{
								num--;
							}
							node3.Remove(eTreeAction.Mouse);
						}
						array3[j] = node3;
						if (num < 0)
						{
							num = 0;
						}
						else if (ކ != null && num > ކ.Nodes.Count)
						{
							num = ކ.Nodes.Count;
						}
						else if (ކ == null && num > Nodes.Count)
						{
							num = Nodes.Count;
						}
						if (ކ == null)
						{
							Nodes.Insert(num, node3, eTreeAction.Mouse);
						}
						else
						{
							ކ.Nodes.Insert(num, node3, eTreeAction.Mouse);
							if (!ކ.Expanded)
							{
								ކ.Expanded = true;
							}
						}
						num++;
					}
					treeDragDropEventArgs.Nodes = array3;
					InvokeAfterNodeDrop(treeDragDropEventArgs);
				}
			}
			__061F = null;
		}
		finally
		{
			_0592 = null;
			if (!_055C)
			{
				EndUpdate();
				RecalcLayout();
				((Control)this).Refresh();
			}
			else
			{
				EndUpdate();
			}
		}
	}

	protected virtual void OnNodeDragStart(object sender, EventArgs e)
	{
		_05B9?.Invoke(sender, e);
	}

	private bool ې(DragEventArgs ؿ)
	{
		if (ؿ.Data != null)
		{
			Node node = null;
			Node[] array = null;
			if (ؿ.Data.GetDataPresent(typeof(Node)))
			{
				node = ؿ.Data.GetData(typeof(Node)) as Node;
			}
			else if (ؿ.Data.GetDataPresent(typeof(Node[])))
			{
				array = ؿ.Data.GetData(typeof(Node[])) as Node[];
			}
			else if (ؿ.Data.GetFormats().Length > 0)
			{
				node = ؿ.Data.GetData(ؿ.Data.GetFormats()[0]) as Node;
				array = ؿ.Data.GetData(ؿ.Data.GetFormats()[0]) as Node[];
			}
			if (node != null && node.TreeControl != this)
			{
				return true;
			}
			if (array != null && array.Length > 0 && array[0].TreeControl != this)
			{
				return true;
			}
		}
		return false;
	}

	private void ۑ(DragEventArgs ؿ)
	{
		if (ؿ.Data != null)
		{
			Node node = null;
			Node[] array = null;
			if (ؿ.Data.GetDataPresent(typeof(Node)))
			{
				node = ؿ.Data.GetData(typeof(Node)) as Node;
			}
			else if (ؿ.Data.GetDataPresent(typeof(Node[])))
			{
				array = ؿ.Data.GetData(typeof(Node[])) as Node[];
			}
			else if (ؿ.Data.GetFormats().Length > 0)
			{
				node = ؿ.Data.GetData(ؿ.Data.GetFormats()[0]) as Node;
				array = ؿ.Data.GetData(ؿ.Data.GetFormats()[0]) as Node[];
			}
			if (node != null)
			{
				ۑ(new Node[1] { node });
			}
			else if (array != null && array.Length > 0)
			{
				ۑ(array);
			}
		}
	}

	private void ۑ(Node[] _0652)
	{
		AdvTreeMultiNodeCancelEventArgs advTreeMultiNodeCancelEventArgs = new AdvTreeMultiNodeCancelEventArgs(eTreeAction.Mouse, _0652);
		OnBeforeNodeDragStart(advTreeMultiNodeCancelEventArgs);
		if (!advTreeMultiNodeCancelEventArgs.Cancel)
		{
			_0592 = _0652[0].Copy();
			_0592.Tag = _0652;
			_0592.IsDragNode = true;
			ے(_0592, _0652[0]);
			if (_0652.Length > 1 && MultiNodeDragCountVisible)
			{
				Cell cell = new Cell();
				cell.Text = _0652.Length.ToString();
				cell.StyleNormal = new ElementStyle();
				cell.StyleNormal.BackColor = ColorScheme.GetColor(4210828);
				cell.StyleNormal.BackColor2 = ColorScheme.GetColor(37881);
				cell.StyleNormal.BackColorGradientAngle = 90;
				cell.StyleNormal.TextColor = Color.White;
				cell.StyleNormal.Padding = 3;
				cell.Name = "sysNodeCount";
				cell.Tag = _0592.Tag;
				_0592.Tag = null;
				_0592.Cells.Insert(0, cell);
			}
			_0592.Visible = false;
			_0557.PerformSingleNodeLayout(_0592);
			((Control)this).Refresh();
			OnNodeDragStart(_0652[0], new EventArgs());
		}
	}

	protected virtual void OnBeforeNodeDragStart(AdvTreeNodeCancelEventArgs e)
	{
		if (_05BA != null)
		{
			_05BA(this, e);
		}
	}

	private void ے(Node ۓ, Node _06D4)
	{
		int alpha = 128;
		ElementStyle elementStyle = null;
		if (_06D4.Style != null)
		{
			elementStyle = _06D4.Style.Copy();
		}
		else if (NodeStyle != null)
		{
			elementStyle = NodeStyle.Copy();
		}
		else
		{
			elementStyle = new ElementStyle();
			elementStyle.TextColorSchemePart = eColorSchemePart.ItemText;
		}
		ElementStyle.SetColorsAlpha(elementStyle, alpha);
		ۓ.Style = elementStyle;
		elementStyle = ((CellStyleDefault == null) ? ElementStyle.GetDefaultCellStyle(ۓ.Style) : CellStyleDefault.Copy());
		ElementStyle.SetColorsAlpha(elementStyle, alpha);
		foreach (Cell cell in ۓ.Cells)
		{
			cell.StyleNormal = elementStyle;
		}
		for (int i = 0; i < ۓ.Cells.Count; i++)
		{
			if (_06D4.Cells[i].StyleNormal != null)
			{
				ElementStyle styleNormal = _06D4.Cells[i].StyleNormal.Copy();
				ElementStyle.SetColorsAlpha(elementStyle, alpha);
				ۓ.Cells[i].StyleNormal = styleNormal;
			}
			else
			{
				ۓ.Cells[i].StyleNormal = elementStyle;
			}
		}
	}

	private bool ە()
	{
		if (ՠ == null)
		{
			return false;
		}
		if (MultiSelect && SelectedNodes.Count > 1 && MultiNodeDragDropAllowed)
		{
			Node[] array = new Node[SelectedNodes.Count];
			SelectedNodes.CopyTo(array);
			return ە(array);
		}
		return StartDragDrop(ՠ);
	}

	private bool ە(Node[] _0652)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (IsDragDropInProgress)
		{
			return false;
		}
		__0604 = false;
		foreach (Node node in _0652)
		{
			if (!node.DragDropEnabled || node == վ)
			{
				return false;
			}
			if (node.IsMouseDown)
			{
				ٽ(new MouseEventArgs(Control.MouseButtons, 0, 0, 0, 0));
			}
		}
		څ(eTreeAction.Mouse);
		_ = Control.MousePosition;
		if (((Component)this).DesignMode)
		{
			ۑ(_0652);
		}
		else
		{
			((Control)this).DoDragDrop((object)_0652, (DragDropEffects)(-2147483641));
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool StartDragDrop(Node node)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (IsDragDropInProgress || !node.DragDropEnabled)
		{
			return false;
		}
		څ(eTreeAction.Mouse);
		if (node.IsMouseDown)
		{
			ٽ(new MouseEventArgs(Control.MouseButtons, 0, 0, 0, 0));
		}
		if (node == վ)
		{
			return false;
		}
		_ = Control.MousePosition;
		if (((Component)this).DesignMode)
		{
			ۑ(new Node[1] { node });
		}
		else
		{
			((Control)this).DoDragDrop((object)node, (DragDropEffects)(-2147483641));
		}
		return true;
	}

	internal Node _06D6()
	{
		if (վ != null)
		{
			return վ;
		}
		if (Nodes.Count > 0)
		{
			return Nodes[0];
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Node GetDragNode()
	{
		return _0592;
	}

	internal _27B3 _06D7()
	{
		return __061F;
	}

	public void BeginInit()
	{
		BeginUpdate();
	}

	public void EndInit()
	{
		EndUpdate();
	}

	internal void _06D8()
	{
		if (_0558 != null)
		{
			_0558.PaintedNodes.Clear();
		}
		if (__05A1 != null)
		{
			__05A1.Clear();
		}
		if (MultiSelect && SelectedNodes.Count > 0)
		{
			SelectedNodes.Clear();
		}
		if (_إ)
		{
			AutoScrollPosition = Point.Empty;
		}
		SelectedNode = null;
		if (((Control)this).IsHandleCreated)
		{
			RecalcLayout();
		}
		else
		{
			_0654();
		}
	}

	internal Rectangle _06D9()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (__05A0 != null && ((Control)__05A0).Visible)
		{
			clientRectangle.Y += ((Control)__05A0).Height;
			clientRectangle.Height -= ((Control)__05A0).Height;
		}
		if (__059D != null && ((Control)__059D).Visible)
		{
			clientRectangle.Width -= ((Control)__059D).Width;
		}
		if (__059E != null && ((Control)__059E).Visible)
		{
			clientRectangle.Height -= ((Control)__059E).Height;
		}
		return clientRectangle;
	}

	protected virtual void OnFormattingEnabledChanged(EventArgs e)
	{
		_05C4?.Invoke(this, e);
	}

	protected virtual void OnFormatStringChanged(EventArgs e)
	{
		_05C5?.Invoke(this, e);
	}

	protected virtual void OnFormatInfoChanged(EventArgs e)
	{
		_05C6?.Invoke(this, e);
	}

	private void _06DA(object _06DB, List<BindingMemberInfo> _06DC, bool _06DD)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		bool flag = _ا != _06DB;
		bool flag2 = _ئ != _06DC;
		if (_ث)
		{
			return;
		}
		try
		{
			if (_06DD || flag || flag2)
			{
				_ث = true;
				IList list = ((DataManager != null) ? DataManager.List : null);
				bool flag3 = DataManager == null;
				_۱();
				_ا = _06DB;
				_ئ = _06DC;
				_۳();
				if (_س)
				{
					CurrencyManager val = null;
					if (_06DB != null && ((Control)this).BindingContext != null && _06DB != Convert.DBNull)
					{
						string text = "";
						if (_ئ != null && _ئ.Count > 0)
						{
							BindingMemberInfo val2 = _06DC[0];
							text = ((BindingMemberInfo)(ref val2)).BindingPath;
						}
						val = (CurrencyManager)((Control)this).BindingContext[_06DB, text];
					}
					if (_ر != val)
					{
						if (_ر != null)
						{
							_ر.ItemChanged -= new ItemChangedEventHandler(_06E4);
							((BindingManagerBase)_ر).PositionChanged -= ۥ;
						}
						_ر = val;
						if (_ر != null)
						{
							_ر.ItemChanged += new ItemChangedEventHandler(_06E4);
							((BindingManagerBase)_ر).PositionChanged += ۥ;
						}
						else if (_06DB == null)
						{
							Nodes.Clear();
						}
					}
					if (_ر != null && (flag2 || flag) && !_06DE(_ئ))
					{
						throw new ArgumentException("Wrong DisplayMembers parameter", "newDisplayMember");
					}
					if (_ر != null && (flag || flag2 || _06DD) && (flag2 || flag || (_06DD && (list != _ر.List || flag3))))
					{
						_06E4(_ر, null);
					}
				}
				_ج.Clear();
			}
			if (flag)
			{
				OnDataSourceChanged(EventArgs.Empty);
			}
			if (flag2)
			{
				OnDisplayMembersChanged(EventArgs.Empty);
			}
		}
		finally
		{
			_ث = false;
		}
	}

	private bool _06DE(List<BindingMemberInfo> _06DF)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (_06DF == null || _06DF.Count == 0)
		{
			return true;
		}
		foreach (BindingMemberInfo item in _06DF)
		{
			BindingMemberInfo current = item;
			if (((BindingMemberInfo)(ref current)).BindingMember != null && !_06E0(current))
			{
				return false;
			}
		}
		return true;
	}

	private bool _06E0(BindingMemberInfo _06E1)
	{
		if (_ر != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_ر).GetItemProperties();
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(((BindingMemberInfo)(ref _06E1)).BindingField))
				{
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && string.Compare(itemProperties[j].Name, ((BindingMemberInfo)(ref _06E1)).BindingField, ignoreCase: true, CultureInfo.CurrentCulture) == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected virtual void OnDataSourceChanged(EventArgs e)
	{
		_05C1?.Invoke(this, e);
	}

	protected virtual void OnDisplayMembersChanged(EventArgs e)
	{
		_05C2?.Invoke(this, e);
	}

	private TypeConverter _06E2(string _06E3)
	{
		if (_ج.ContainsKey(_06E3))
		{
			return (TypeConverter)_ج[_06E3];
		}
		if (DataManager != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)DataManager).GetItemProperties();
			if (itemProperties != null)
			{
				PropertyDescriptor propertyDescriptor = itemProperties.Find(_06E3, ignoreCase: true);
				if (propertyDescriptor != null)
				{
					_ج.Add(_06E3, propertyDescriptor.Converter);
					return propertyDescriptor.Converter;
				}
			}
		}
		return null;
	}

	private void _06E4(object _٠, ItemChangedEventArgs ؿ)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (((Control)this).InvokeRequired)
		{
			((Control)this).Invoke((Delegate)new ItemChangedEventHandler(_06E4), new object[2] { _٠, ؿ });
		}
		else
		{
			if (_ر == null)
			{
				return;
			}
			if (ؿ == null || ؿ.Index == -1)
			{
				SetItemsCore(_ر.List);
				if (Մ)
				{
					if (((BindingManagerBase)_ر).Position == -1)
					{
						SelectedNode = null;
					}
					else
					{
						SelectedNode = FindNodeByBindingIndex(((BindingManagerBase)_ر).Position);
					}
				}
			}
			else
			{
				SetItemCore(ؿ.Index, _ر.List[ؿ.Index]);
			}
		}
	}

	private void ۥ(object _٠, EventArgs ؿ)
	{
		if (_ر != null && Մ)
		{
			if (((BindingManagerBase)_ر).Position == -1)
			{
				SelectedNode = null;
			}
			else
			{
				SelectedNode = FindNodeByBindingIndex(((BindingManagerBase)_ر).Position);
			}
		}
	}

	public virtual void RefreshItems()
	{
		if (_ر == null)
		{
			return;
		}
		SetItemsCore(_ر.List);
		if (Մ)
		{
			if (((BindingManagerBase)_ر).Position == -1)
			{
				SelectedNode = null;
			}
			else
			{
				SelectedNode = FindNodeByBindingIndex(((BindingManagerBase)_ر).Position);
			}
		}
	}

	public void ClearAndDisposeAllNodes()
	{
		List<Control> list = new List<Control>();
		if (_059A.Count > 0)
		{
			foreach (Cell item in _059A)
			{
				Control hostedControl = item.HostedControl;
				if (hostedControl != null)
				{
					list.Add(hostedControl);
				}
			}
		}
		foreach (Node node in Nodes)
		{
			node.Dispose();
		}
		foreach (Control item2 in list)
		{
			((Control)this).Controls.Remove(item2);
			((Component)(object)item2).Dispose();
		}
		Nodes.Clear();
	}

	private bool ۦ(object _06E7)
	{
		if (_06E7 == null)
		{
			return false;
		}
		if (!(_06E7 is string))
		{
			return _06E7.GetType().IsPrimitive;
		}
		return true;
	}

	protected virtual void SetItemsCore(IList items)
	{
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		BeginUpdate();
		SuspendPaint = true;
		_ح = true;
		try
		{
			ClearAndDisposeAllNodes();
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(DisplayMembers))
			{
				if (Columns.Count > 0)
				{
					foreach (ColumnHeader column in Columns)
					{
						if (!string.IsNullOrEmpty(column.DataFieldName))
						{
							list.Add(column.DataFieldName);
						}
					}
				}
				if (list.Count == 0)
				{
					Columns.Clear();
					if (_ر.List is Array)
					{
						ColumnHeader columnHeader2 = new ColumnHeader("Items");
						columnHeader2.Width.Relative = 100;
						Columns.Add(columnHeader2);
						OnDataColumnCreated(new DataColumnEventArgs(columnHeader2));
					}
					else if (_ر != null)
					{
						if (((BindingManagerBase)_ر).Position >= 0 && ((BindingManagerBase)_ر).Current != null && ۦ(((BindingManagerBase)_ر).Current))
						{
							ColumnHeader columnHeader3 = new ColumnHeader();
							columnHeader3.Width.Relative = 100;
							Columns.Add(columnHeader3);
							OnDataColumnCreated(new DataColumnEventArgs(columnHeader3));
						}
						else
						{
							PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_ر).GetItemProperties();
							foreach (PropertyDescriptor item in itemProperties)
							{
								list.Add(item.Name);
								ColumnHeader columnHeader4 = new ColumnHeader(_26E6._26E8(item.Name));
								columnHeader4.DataFieldName = item.Name;
								columnHeader4.Width.Relative = Math.Max(15, 100 / itemProperties.Count);
								Columns.Add(columnHeader4);
								OnDataColumnCreated(new DataColumnEventArgs(columnHeader4));
							}
						}
					}
				}
			}
			else
			{
				Columns.Clear();
				if (_ئ != null && _ئ.Count > 0)
				{
					foreach (BindingMemberInfo item2 in _ئ)
					{
						BindingMemberInfo current = item2;
						ColumnHeader columnHeader5 = new ColumnHeader(_26E6._26E8(((BindingMemberInfo)(ref current)).BindingMember));
						columnHeader5.DataFieldName = ((BindingMemberInfo)(ref current)).BindingMember;
						columnHeader5.Tag = current;
						columnHeader5.Width.Relative = Math.Max(15, 100 / _ئ.Count);
						Columns.Add(columnHeader5);
						list.Add(((BindingMemberInfo)(ref current)).BindingMember);
						OnDataColumnCreated(new DataColumnEventArgs(columnHeader5));
					}
				}
			}
			if (string.IsNullOrEmpty(_د) && string.IsNullOrEmpty(_خ))
			{
				for (int i = 0; i < items.Count; i++)
				{
					object obj = items[i];
					_06EB(Nodes, obj, i, list);
				}
				return;
			}
			if (!string.IsNullOrEmpty(_خ))
			{
				Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
				Dictionary<string, List<Node>> dictionary2 = new Dictionary<string, List<Node>>();
				string[] array = _خ.Split(new char[1] { ',' });
				int position = ((BindingManagerBase)_ر).Position;
				for (int j = 0; j < items.Count; j++)
				{
					object obj2 = items[j];
					string itemText = GetItemText(obj2, array[0].Trim());
					string itemText2 = GetItemText(obj2, array[1].Trim());
					Node value = null;
					if (dictionary.TryGetValue(itemText2, out value))
					{
						Node node = _06EB(value.Nodes, obj2, j, list);
						dictionary.Add(itemText, node);
						if (node.BindingIndex == position)
						{
							SelectNode(node, eTreeAction.Code);
						}
						continue;
					}
					Node node2 = _06EB(Nodes, obj2, j, list);
					List<Node> value2 = null;
					if (!dictionary2.TryGetValue(itemText2, out value2))
					{
						value2 = new List<Node>();
						dictionary2.Add(itemText2, value2);
					}
					value2.Add(node2);
					dictionary.Add(itemText, node2);
				}
				{
					foreach (KeyValuePair<string, List<Node>> item3 in dictionary2)
					{
						Node value3 = null;
						if (!dictionary.TryGetValue(item3.Key, out value3))
						{
							continue;
						}
						foreach (Node item4 in item3.Value)
						{
							item4.Remove();
							value3.Nodes.Add(item4);
							if (item4.BindingIndex == position)
							{
								SelectNode(item4, eTreeAction.Code);
							}
						}
					}
					return;
				}
			}
			string[] array2 = _د.Split(new char[1] { ',' });
			for (int k = 0; k < array2.Length; k++)
			{
				array2[k] = array2[k].Trim();
			}
			Dictionary<string, Node> dictionary3 = new Dictionary<string, Node>();
			for (int l = 0; l < items.Count; l++)
			{
				object obj3 = items[l];
				NodeCollection nodes = Nodes;
				string text = "";
				for (int m = 0; m < array2.Length; m++)
				{
					string itemText3 = GetItemText(obj3, array2[m]);
					text = text + itemText3.ToLower() + "/";
					Node value4 = null;
					if (!dictionary3.TryGetValue(text, out value4))
					{
						value4 = _06E8(nodes, itemText3);
						dictionary3.Add(text, value4);
					}
					nodes = value4.Nodes;
				}
				_06EB(nodes, obj3, l, list);
			}
		}
		finally
		{
			SuspendPaint = false;
			EndUpdate();
			_ح = false;
		}
	}

	protected virtual void OnDataColumnCreated(DataColumnEventArgs args)
	{
		if (_05CC != null)
		{
			_05CC(this, args);
		}
	}

	private Node _06E8(NodeCollection _06E9, string _06EA)
	{
		Node node = new Node();
		node.Text = _06EA;
		node.Style = _ذ;
		node.Expanded = true;
		node.Selectable = false;
		_06E9.Add(node);
		DataNodeEventArgs dataNodeEventArgs = new DataNodeEventArgs(node, null);
		OnGroupNodeCreated(dataNodeEventArgs);
		return node;
	}

	protected virtual void OnGroupNodeCreated(DataNodeEventArgs dataNodeEventArgs)
	{
		if (_05C8 != null)
		{
			_05C8(this, dataNodeEventArgs);
		}
	}

	private Node _06EB(NodeCollection _06E9, object _06EC, int _06ED, List<string> ۮ)
	{
		Node node = new Node();
		_06E9.Add(node);
		ۯ(node, _06EC, ۮ, _06ED);
		DataNodeEventArgs dataNodeEventArgs = new DataNodeEventArgs(node, _06EC);
		OnDataNodeCreated(dataNodeEventArgs);
		return dataNodeEventArgs.Node;
	}

	private void ۯ(Node _0653, object _06EC, List<string> ۮ, int _۰)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		_0653.DataKey = _06EC;
		_0653.BindingIndex = _۰;
		_0653.CreateCells();
		if (ۮ.Count > 0)
		{
			for (int i = 0; i < ۮ.Count; i++)
			{
				object propertyValue = GetPropertyValue(_06EC, ۮ[i]);
				if (propertyValue is Image)
				{
					_0653.Cells[i].Images.Image = (Image)propertyValue;
				}
				else
				{
					_0653.Cells[i].Text = GetItemText(_06EC, ۮ[i]);
				}
			}
		}
		else if (_06EC != null)
		{
			_0653.Text = _06EC.ToString();
		}
	}

	protected virtual void OnDataNodeCreated(DataNodeEventArgs dataNodeEventArgs)
	{
		if (_05C7 != null)
		{
			_05C7(this, dataNodeEventArgs);
		}
	}

	protected virtual void SetItemCore(int index, object value)
	{
		Node node = FindNodeByBindingIndex(index);
		if (node == null)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (ColumnHeader column in Columns)
		{
			if (!string.IsNullOrEmpty(column.DataFieldName))
			{
				list.Add(column.DataFieldName);
			}
		}
		ۯ(node, value, list, index);
	}

	protected virtual void OnParentFieldNamesChanged()
	{
		RefreshItems();
	}

	protected virtual void OnGroupingMembersChanged()
	{
		RefreshItems();
	}

	private void _۱()
	{
		if (_ا is IComponent)
		{
			((IComponent)_ا).Disposed -= _۲;
		}
		if (_ا is ISupportInitializeNotification supportInitializeNotification && _ز)
		{
			supportInitializeNotification.Initialized -= _۴;
			_ز = false;
		}
	}

	private void _۲(object _٠, EventArgs ؿ)
	{
		_06DA(null, null, _06DD: true);
	}

	private void _۳()
	{
		if (_ا is IComponent)
		{
			((IComponent)_ا).Disposed += _۲;
		}
		if (_ا is ISupportInitializeNotification { IsInitialized: false } supportInitializeNotification)
		{
			supportInitializeNotification.Initialized += _۴;
			_ز = true;
			_س = false;
		}
		else
		{
			_س = true;
		}
	}

	private void _۴(object _٠, EventArgs ؿ)
	{
		_06DA(_ا, _ئ, _06DD: true);
	}

	protected virtual void OnFormat(TreeConvertEventArgs e)
	{
		_05C3?.Invoke(this, e);
	}

	public string GetItemText(object item, string fieldName)
	{
		object propertyValue = GetPropertyValue(item, fieldName);
		if (!_ب)
		{
			if (item == null)
			{
				return string.Empty;
			}
			if (propertyValue == null)
			{
				return "";
			}
			return Convert.ToString(propertyValue, CultureInfo.CurrentCulture);
		}
		TreeConvertEventArgs treeConvertEventArgs = new TreeConvertEventArgs(propertyValue, typeof(string), item, fieldName);
		OnFormat(treeConvertEventArgs);
		if (((ConvertEventArgs)treeConvertEventArgs).Value != item && ((ConvertEventArgs)treeConvertEventArgs).Value is string)
		{
			return (string)((ConvertEventArgs)treeConvertEventArgs).Value;
		}
		if (ش == null)
		{
			ش = TypeDescriptor.GetConverter(typeof(string));
		}
		try
		{
			return (string)ᬒ.ᬖ(propertyValue, typeof(string), _06E2(fieldName), ش, _ة, _ت, null, DBNull.Value);
		}
		catch (Exception ex)
		{
			if (ex is SecurityException || _۵(ex))
			{
				throw;
			}
			return (propertyValue != null) ? Convert.ToString(item, CultureInfo.CurrentCulture) : "";
		}
	}

	private static bool _۵(Exception _۶)
	{
		if (!(_۶ is NullReferenceException) && !(_۶ is StackOverflowException) && !(_۶ is OutOfMemoryException) && !(_۶ is ThreadAbortException) && !(_۶ is ExecutionEngineException) && !(_۶ is IndexOutOfRangeException))
		{
			return _۶ is AccessViolationException;
		}
		return true;
	}

	public void ClearDescriptorsCache()
	{
		_ص.Clear();
	}

	protected object GetPropertyValue(object item, string fieldName)
	{
		if (item != null && fieldName.Length > 0)
		{
			try
			{
				PropertyDescriptor value = null;
				string key = item.GetType().ToString() + ":" + fieldName;
				if (!_ص.TryGetValue(key, out value))
				{
					value = ((_ر == null) ? TypeDescriptor.GetProperties(item).Find(fieldName, ignoreCase: true) : ((BindingManagerBase)_ر).GetItemProperties().Find(fieldName, ignoreCase: true));
					if (value != null)
					{
						_ص.Add(key, value);
					}
				}
				if (value != null)
				{
					item = value.GetValue(item);
				}
			}
			catch
			{
			}
		}
		return item;
	}

	private void _۷(int ԝ)
	{
		if (ԝ == -1)
		{
			SelectedNode = null;
			return;
		}
		Node node = null;
		node = ((_ر == null) ? GetNodeByFlatIndex(ԝ) : FindNodeByBindingIndex(ԝ));
		SelectedNode = node;
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
		_05CB?.Invoke(this, e);
	}

	protected virtual void OnValueMemberChanged(EventArgs e)
	{
		_05C9?.Invoke(this, e);
	}

	protected virtual void OnSelectedValueChanged(EventArgs e)
	{
		_05CA?.Invoke(this, e);
	}

	protected virtual void OnTouchEnabledChanged(bool oldValue, bool newValue)
	{
	}

	private void _۸(object _٠, GestureEventArgs ؿ)
	{
		if (_ع)
		{
			_ؽ = _٦();
			_ػ = ؿ.Location;
			_ؼ = __0619;
			_غ = true;
			ؿ.Handled = true;
		}
	}

	private void _۹(object _٠, GestureEventArgs ؿ)
	{
		if (_غ)
		{
			ۺ();
			ؿ.Handled = true;
		}
	}

	private void ۺ()
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		_غ = false;
		Point point = AutoScrollPosition;
		if (__0617.Width > _ؽ.Width)
		{
			if (__0617.Width - _ؽ.Width < -__0619.X)
			{
				point = new Point(__0617.Width - _ؽ.Width, point.Y);
			}
			else if (-__0619.X < 0)
			{
				point = new Point(0, point.Y);
			}
		}
		if (__0617.Height > _ؽ.Height)
		{
			if (__0617.Height - _ؽ.Height < -__0619.Y)
			{
				point = new Point(point.X, __0617.Height - _ؽ.Height);
			}
			else if (-__0619.Y < 0)
			{
				point = new Point(point.X, 0);
			}
		}
		if (AutoScrollPosition != point)
		{
			AutoScrollPosition = point;
			((ScrollableControl)this).OnScroll(new ScrollEventArgs((ScrollEventType)8, point.Y));
		}
	}

	private void ۻ(object _٠, GestureEventArgs ؿ)
	{
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		if (!_غ)
		{
			return;
		}
		Point autoScrollPosition = AutoScrollPosition;
		int num = ؿ.Location.X - _ػ.X;
		int num2 = num + _ؼ.X;
		bool flag = false;
		if (__0617.Width > _ؽ.Width)
		{
			if (-num2 + Ն > __0617.Width - _ؽ.Width)
			{
				autoScrollPosition.X = -(__0617.Width + Ն - _ؽ.Width);
				flag = true;
			}
			else if (num2 > Ն)
			{
				autoScrollPosition.X = Ն;
				flag = true;
			}
			else
			{
				autoScrollPosition.X = num2;
			}
		}
		bool flag2 = false;
		if (__0617.Height > _ؽ.Height)
		{
			num = ؿ.Location.Y - _ػ.Y;
			num2 = num + _ؼ.Y;
			if (-num2 + Ն > __0617.Height - _ؽ.Height)
			{
				autoScrollPosition.Y = -(__0617.Height + Ն - _ؽ.Height);
				flag2 = true;
			}
			else if (num2 > Ն)
			{
				autoScrollPosition.Y = Ն;
				flag2 = true;
			}
			else
			{
				autoScrollPosition.Y = num2;
			}
		}
		if (AutoScrollPosition != autoScrollPosition)
		{
			AutoScrollPosition = autoScrollPosition;
			((Control)this).Update();
			((ScrollableControl)this).OnScroll(new ScrollEventArgs((ScrollEventType)5, autoScrollPosition.Y));
		}
		if (flag && flag2 && ؿ.IsInertia)
		{
			ۺ();
		}
		ؿ.Handled = true;
	}

	[CompilerGenerated]
	private void ۼ()
	{
		ڟ();
	}

	[CompilerGenerated]
	private void _06FD()
	{
		ڠ();
	}

	[CompilerGenerated]
	private void _06FE()
	{
		ڦ();
	}
}
