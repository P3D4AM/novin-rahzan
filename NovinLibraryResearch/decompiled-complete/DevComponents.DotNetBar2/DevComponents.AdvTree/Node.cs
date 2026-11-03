using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree;

[Designer("DevComponents.AdvTree.Design.NodeDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class Node : Component, IComparable
{
	private MouseEventHandler Ӗ;

	private MouseEventHandler ӗ;

	private MouseEventHandler Ә;

	private EventHandler ә;

	private EventHandler Ӛ;

	private EventHandler ӛ;

	private EventHandler Ӝ;

	private EventHandler ӝ;

	private MarkupLinkClickEventHandler Ӟ;

	private Rectangle ӟ = Rectangle.Empty;

	private Rectangle Ӡ = Rectangle.Empty;

	private Rectangle ӡ = Rectangle.Empty;

	private Rectangle Ӣ = Rectangle.Empty;

	private Rectangle ӣ = Rectangle.Empty;

	private NodeCollection Ӥ;

	private CellCollection ӥ = new CellCollection();

	private ColumnHeaderCollection Ӧ;

	private object ӧ;

	private object Ө;

	private string ө = "";

	private bool Ӫ;

	private eNodeExpandVisibility ӫ;

	private Rectangle Ӭ = Rectangle.Empty;

	private Node ӭ;

	private ElementStyle Ӯ;

	private ElementStyle ӯ;

	private ElementStyle Ӱ;

	private bool ӱ = true;

	private int Ӳ;

	private eMapPosition ӳ;

	private int Ӵ;

	internal AdvTree ӵ;

	private ElementStyle Ӷ;

	private _27AF ӷ;

	private NodeConnector Ӹ;

	private eCellLayout ӹ;

	private bool Ӻ = true;

	private bool ӻ;

	private bool Ӽ;

	private bool ӽ = true;

	private string Ӿ = "";

	private ConnectorPointsCollection ӿ;

	private bool Ԁ;

	private eNodeRenderMode ԁ;

	private TreeRenderer Ԃ;

	private object ԃ;

	private eVerticalAlign _Ԅ = eVerticalAlign.Middle;

	private bool _ԅ;

	private bool _Ԇ = true;

	private int _ԇ = -1;

	private eHorizontalAlign _Ԉ = eHorizontalAlign.Center;

	private bool _ԉ = true;

	private bool _Ԋ = true;

	private bool _ԋ;

	private bool _Ԍ = true;

	private int _ԍ;

	private bool _Ԏ = true;

	private bool _ԏ;

	private NodeAccessibleObject _Ԑ;

	private string _ԑ = "";

	private string _Ԓ = "";

	private string _ԓ = "";

	private AccessibleRole _Ԕ = (AccessibleRole)36;

	private bool _ԕ = true;

	[Browsable(false)]
	public bool HasChildNodes
	{
		get
		{
			if (Ӥ != null)
			{
				return Ӥ.Count > 0;
			}
			return false;
		}
	}

	[Description("Indicates the context menu assigned to this node.")]
	[DefaultValue(null)]
	[Editor("DevComponents.AdvTree.Design.NodeContextMenuTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	public object ContextMenu
	{
		get
		{
			return ԃ;
		}
		set
		{
			ԃ = value;
		}
	}

	[Browsable(false)]
	public bool HasHostedControls
	{
		get
		{
			foreach (Cell item in ӥ)
			{
				if (item.HostedControl != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	[Category("Style")]
	[Browsable(false)]
	[Description("Indicates render mode used to render the node.")]
	[DefaultValue(null)]
	internal TreeRenderer NodeRenderer
	{
		get
		{
			return Ԃ;
		}
		set
		{
			Ԃ = value;
			OnDisplayChanged();
		}
	}

	[Browsable(true)]
	[Description("Indicates render mode used to render the node.")]
	[Category("Style")]
	[DefaultValue(eNodeRenderMode.Default)]
	internal eNodeRenderMode RenderMode
	{
		get
		{
			return ԁ;
		}
		set
		{
			ԁ = value;
			OnDisplayChanged();
		}
	}

	[Browsable(true)]
	[Category("Node State")]
	[DevCoSerialize]
	[DefaultValue(false)]
	[Description("Indicates whether node is expanded.")]
	public bool Expanded
	{
		get
		{
			return Ӫ;
		}
		set
		{
			if (value)
			{
				Expand();
			}
			else
			{
				Collapse();
			}
		}
	}

	[Browsable(false)]
	[Description("Indicates the name used to identify node.")]
	[Category("Design")]
	[DevCoSerialize]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				Ӿ = Site.Name;
			}
			return Ӿ;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				Ӿ = "";
			}
			else
			{
				Ӿ = value;
			}
		}
	}

	[Category("Behavior")]
	[DevCoSerialize]
	[DefaultValue(true)]
	[Description("Indicates whether node can be dragged and dropped.")]
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

	[DefaultValue(eNodeExpandVisibility.Auto)]
	[Category("Appearance")]
	[Description("Indicates whether the expand button is always visible regardless of whether node contains child nodes or not.")]
	[Browsable(true)]
	[DevCoSerialize]
	public eNodeExpandVisibility ExpandVisibility
	{
		get
		{
			return ӫ;
		}
		set
		{
			ӫ = value;
			SizeChanged = true;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool SizeChanged
	{
		get
		{
			return ӱ;
		}
		set
		{
			ӱ = value;
			OnSizeChanged();
		}
	}

	[Browsable(false)]
	internal Rectangle BoundsRelative => ӟ;

	[Browsable(false)]
	public Rectangle Bounds
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, this, treeControl.NodeDisplay.Offset);
			}
			return Rectangle.Empty;
		}
	}

	internal Rectangle ContentBounds => Ӣ;

	[Browsable(false)]
	internal Rectangle CellsBoundsRelative => ӡ;

	[Browsable(false)]
	public Rectangle CellsBounds
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CellsBounds, this, treeControl.NodeDisplay.Offset);
			}
			return Rectangle.Empty;
		}
	}

	internal Rectangle ChildNodesBounds
	{
		get
		{
			return Ӡ;
		}
		set
		{
			Ӡ = value;
		}
	}

	[Browsable(false)]
	internal Rectangle ExpandPartRectangleRelative => Ӭ;

	[Browsable(false)]
	public Rectangle ExpandPartRectangle
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds, this, treeControl.NodeDisplay.Offset);
			}
			return Rectangle.Empty;
		}
	}

	[Category("Appearance")]
	[DefaultValue(eVerticalAlign.Middle)]
	[Description("Indicates vertical alignment within the node bounds of expand part of the node, if one is visible.")]
	public eVerticalAlign ExpandPartVerticalAlignment
	{
		get
		{
			return _Ԅ;
		}
		set
		{
			if (_Ԅ != value)
			{
				_Ԅ = value;
				InvalidateLayout();
			}
		}
	}

	[Browsable(false)]
	internal Rectangle CommandBounds
	{
		get
		{
			if (!ӣ.IsEmpty)
			{
				AdvTree treeControl = TreeControl;
				if (treeControl != null)
				{
					return NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CommandBounds, this, treeControl.NodeDisplay.Offset);
				}
			}
			return Rectangle.Empty;
		}
	}

	internal Rectangle CommandBoundsRelative
	{
		get
		{
			return ӣ;
		}
		set
		{
			ӣ = value;
		}
	}

	[Category("Appearance")]
	[DevCoSerialize]
	[Description("Indicates whether the tree node is in a checked state.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool Checked
	{
		get
		{
			return ӥ[0].Checked;
		}
		set
		{
			ӥ[0].Checked = value;
		}
	}

	[Description("Indicates checkbox alignment in relation to the text displayed by cell.")]
	[Browsable(true)]
	[DevCoSerialize]
	[Category("Check-box Properties")]
	[DefaultValue(eCellPartAlignment.Default)]
	public eCellPartAlignment CheckBoxAlignment
	{
		get
		{
			return ӥ[0].CheckBoxAlignment;
		}
		set
		{
			ӥ[0].CheckBoxAlignment = value;
		}
	}

	[Category("Check-box Properties")]
	[Description("Indicates whether check box is visible inside the cell.")]
	[DevCoSerialize]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool CheckBoxVisible
	{
		get
		{
			return ӥ[0].CheckBoxVisible;
		}
		set
		{
			ӥ[0].CheckBoxVisible = value;
		}
	}

	[Category("Check-box Properties")]
	[Browsable(true)]
	[DefaultValue(eCheckBoxStyle.CheckBox)]
	[Description("Indicates appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.")]
	public eCheckBoxStyle CheckBoxStyle
	{
		get
		{
			return ӥ[0].CheckBoxStyle;
		}
		set
		{
			ӥ[0].CheckBoxStyle = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether the CheckBox will allow three check states rather than two.")]
	[Category("Check-box Properties")]
	public bool CheckBoxThreeState
	{
		get
		{
			return ӥ[0].CheckBoxThreeState;
		}
		set
		{
			ӥ[0].CheckBoxThreeState = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state")]
	[Browsable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Check-box Properties")]
	public CheckState CheckState
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return ӥ[0].CheckState;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			ӥ[0].CheckState = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether first cell content is editable when cell editing is enabled on tree control.")]
	public bool Editable
	{
		get
		{
			return ӥ[0].Editable;
		}
		set
		{
			ӥ[0].Editable = value;
		}
	}

	[Browsable(false)]
	public string FullPath
	{
		get
		{
			if (TreeControl == null)
			{
				return "";
			}
			return _175C._175E(this, TreeControl.PathSeparator);
		}
	}

	[Browsable(false)]
	public int Index => _175C._176D(this);

	[Browsable(false)]
	public bool IsEditing => Ӽ;

	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			foreach (Cell cell in Cells)
			{
				if (cell.IsMouseDown)
				{
					return true;
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			foreach (Cell cell in Cells)
			{
				if (cell.IsMouseOver)
				{
					return true;
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			return _ԅ;
		}
		internal set
		{
			_ԅ = value;
		}
	}

	[Description("Indicates whether node can be selected by user by clicking it with the mouse or using keyboard.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool Selectable
	{
		get
		{
			return _Ԇ;
		}
		set
		{
			_Ԇ = value;
		}
	}

	[Browsable(false)]
	public bool CanSelect => Enabled & Visible & Selectable;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Cell SelectedCell
	{
		get
		{
			if (IsSelected)
			{
				return GetSelectedCell();
			}
			return null;
		}
		set
		{
			SetSelectedCell(value, eTreeAction.Code);
		}
	}

	[Browsable(false)]
	public bool IsVisible => _175C.ᝫ(this);

	[Browsable(false)]
	public bool IsDisplayed => _175C.ᝬ(this);

	[Browsable(false)]
	public Node LastNode => _175C.ᝠ(this);

	[Browsable(false)]
	public Node NextNode => _175C.ᝧ(this);

	[Browsable(false)]
	public Node NextVisibleNode => _175C.ᝩ(this);

	[Browsable(false)]
	public int Level
	{
		get
		{
			int num = 0;
			for (Node parent = Parent; parent != null; parent = parent.Parent)
			{
				num++;
			}
			return num;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Nodes")]
	[Browsable(true)]
	public NodeCollection Nodes
	{
		get
		{
			if (Ӥ == null)
			{
				Ӥ = new NodeCollection();
				Ӥ.Ԟ(this);
			}
			return Ӥ;
		}
	}

	[Browsable(false)]
	public bool AnyVisibleNodes => _175C._1776(this);

	[Browsable(false)]
	public Node Parent => ӭ;

	[Browsable(false)]
	public Node PrevNode => _175C.ᝮ(this);

	[Browsable(false)]
	public Node PrevVisibleNode => _175C.ᝯ(this);

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates text that contains data about the tree node.")]
	[Browsable(false)]
	[DefaultValue(null)]
	[Category("Data")]
	public object Tag
	{
		get
		{
			return ӥ[0].Tag;
		}
		set
		{
			ӥ[0].Tag = value;
		}
	}

	[DevCoSerialize]
	[DefaultValue("")]
	[Browsable(true)]
	[Description("Indicates text that contains data about the tree node.")]
	[Category("Data")]
	public string TagString
	{
		get
		{
			return ӥ[0].TagString;
		}
		set
		{
			ӥ[0].TagString = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int BindingIndex
	{
		get
		{
			return _ԇ;
		}
		set
		{
			_ԇ = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	[Description("Indicates text that contains data about the tree node.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Data")]
	public object DataKey
	{
		get
		{
			return Ө;
		}
		set
		{
			Ө = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Data")]
	[DevCoSerialize]
	[Description("Indicates text that contains data about the tree node.")]
	public string DataKeyString
	{
		get
		{
			if (Ө == null)
			{
				return "";
			}
			return Ө.ToString();
		}
		set
		{
			Ө = value;
		}
	}

	[Description("Indicates format that is applied to the value of Text property for display purposes.")]
	[Category("Appearance")]
	[DefaultValue("")]
	[Browsable(true)]
	public string TextDisplayFormat
	{
		get
		{
			return Cells[0].TextDisplayFormat;
		}
		set
		{
			Cells[0].TextDisplayFormat = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Browsable(true)]
	[Localizable(true)]
	[DevCoSerialize]
	[Description("Indicates text displayed in the tree node.")]
	[DefaultValue("")]
	public string Text
	{
		get
		{
			return Cells[0].Text;
		}
		set
		{
			Cells[0].Text = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates control hosted inside of the cell.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Control HostedControl
	{
		get
		{
			return Cells[0].HostedControl;
		}
		set
		{
			Cells[0].HostedControl = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates item hosted inside of the cell. Only items that do not generate popups are supported. Note that cell can only host either HostedItem or HostedControl but not both.")]
	[Category("Appearance")]
	[Editor("DevComponents.AdvTree.Design.HostedItemTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public BaseItem HostedItem
	{
		get
		{
			return Cells[0].HostedItem;
		}
		set
		{
			Cells[0].HostedItem = value;
		}
	}

	[Browsable(false)]
	public AdvTree TreeControl => GetTreeControl();

	[Category("Cells")]
	[DevCoSerialize]
	[Description("Indicates layout of the cells inside the node.")]
	[DefaultValue(eCellLayout.Default)]
	[Browsable(true)]
	public eCellLayout CellLayout
	{
		get
		{
			return ӹ;
		}
		set
		{
			ӹ = value;
			SizeChanged = true;
		}
	}

	[Category("Cells")]
	[Description("Indicates cell alignment when CellLayout=Vertical. Default value is center which means that cells are center aligned.")]
	[DefaultValue(eHorizontalAlign.Center)]
	public eHorizontalAlign VerticalCellLayoutAlignment
	{
		get
		{
			return _Ԉ;
		}
		set
		{
			_Ԉ = value;
			SizeChanged = true;
		}
	}

	[Description("Indicates the layout of the cell parts like check box, image and text.")]
	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(eCellPartLayout.Default)]
	[Category("Cells")]
	public eCellPartLayout CellPartLayout
	{
		get
		{
			return ӥ[0].Layout;
		}
		set
		{
			if (ӥ[0].Layout != value)
			{
				ӥ[0].Layout = value;
			}
		}
	}

	[Description("Indicates the text that is displayed when mouse hovers over the cell.")]
	[Localizable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue("")]
	public string Tooltip
	{
		get
		{
			return ӥ[0].Tooltip;
		}
		set
		{
			ӥ[0].Tooltip = value;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection of Cells assigned to this node.")]
	[Category("Cells")]
	public CellCollection Cells => ӥ;

	[Category("Columns")]
	[Description("Indicates whether column header for child nodes if defined is visible.")]
	[DefaultValue(true)]
	public bool NodesColumnsHeaderVisible
	{
		get
		{
			return _ԉ;
		}
		set
		{
			_ԉ = value;
			SizeChanged = true;
			OnDisplayChanged();
		}
	}

	[Browsable(false)]
	public bool HasColumns
	{
		get
		{
			if (Ӧ != null && Ӧ.Count > 0)
			{
				return true;
			}
			return false;
		}
	}

	[Description("Defines the columns for the child nodes.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Columns")]
	[Browsable(true)]
	public ColumnHeaderCollection NodesColumns
	{
		get
		{
			if (Ӧ == null)
			{
				Ӧ = new ColumnHeaderCollection();
				Ӧ.Ԟ(this);
			}
			return Ӧ;
		}
	}

	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates the style for the child nodes when node is expanded.")]
	public ElementStyle StyleExpanded
	{
		get
		{
			return Ӯ;
		}
		set
		{
			if (Ӯ != value)
			{
				if (Ӯ != null)
				{
					Ӯ.StyleChanged -= ElementStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ElementStyleChanged;
				}
				Ӯ = value;
				if (Expanded)
				{
					SizeChanged = true;
				}
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoSerialize]
	[DefaultValue("")]
	public string StyleExpandedName
	{
		get
		{
			if (Ӯ != null)
			{
				return Ӯ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleExpanded"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleExpanded"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[Category("Style")]
	[Description("Indicates the style for the nodes when node is selected.")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue(null)]
	public ElementStyle StyleSelected
	{
		get
		{
			return ӯ;
		}
		set
		{
			if (ӯ != value)
			{
				if (ӯ != null)
				{
					ӯ.StyleChanged -= ElementStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ElementStyleChanged;
				}
				ӯ = value;
				if (IsSelected)
				{
					SizeChanged = true;
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue("")]
	[DevCoSerialize]
	[Browsable(false)]
	public string StyleSelectedName
	{
		get
		{
			if (ӯ != null)
			{
				return ӯ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Description("Indicates the style for the nodes when node is selected.")]
	[Browsable(true)]
	[Category("Style")]
	public ElementStyle StyleMouseOver
	{
		get
		{
			return Ӱ;
		}
		set
		{
			if (Ӱ != value)
			{
				if (Ӱ != null)
				{
					Ӱ.StyleChanged -= ElementStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ElementStyleChanged;
				}
				Ӱ = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue("")]
	[DevCoSerialize]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string StyleMouseOverName
	{
		get
		{
			if (Ӱ != null)
			{
				return Ӱ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates the node style.")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public ElementStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			if (Ӷ != value)
			{
				if (Ӷ != null)
				{
					Ӷ.StyleChanged -= ElementStyleChanged;
				}
				if (value != null)
				{
					value.StyleChanged += ElementStyleChanged;
				}
				Ӷ = value;
				SizeChanged = true;
				Invalidate();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoSerialize]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue("")]
	public string StyleName
	{
		get
		{
			if (Ӷ != null)
			{
				return Ӷ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["Style"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["Style"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	internal _27AF MouseOverNodePart
	{
		get
		{
			return ӷ;
		}
		set
		{
			ӷ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(0)]
	internal int Offset
	{
		get
		{
			return Ӵ;
		}
		set
		{
			Ӵ = value;
			SizeChanged = true;
		}
	}

	[DefaultValue(eCellPartAlignment.Default)]
	[Browsable(true)]
	[Category("Image Properties")]
	[Description("Gets or sets the image alignment in relation to the text displayed by cell.")]
	[DevCoSerialize]
	public eCellPartAlignment ImageAlignment
	{
		get
		{
			return ӥ[0].ImageAlignment;
		}
		set
		{
			if (ӥ[0].ImageAlignment != value)
			{
				ӥ[0].ImageAlignment = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoSerialize]
	[Description("Gets or sets whether node is enabled.")]
	public bool Enabled
	{
		get
		{
			return _Ԋ;
		}
		set
		{
			_Ԋ = value;
			foreach (Cell item in ӥ)
			{
				item.Enabled = _Ԋ;
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Images")]
	[Description("Indicates image displayed when the tree node is disabled.")]
	public Image ImageDisabled
	{
		get
		{
			return ӥ[0].Images.ImageDisabled;
		}
		set
		{
			SizeChanged = true;
			ӥ[0].Images.ImageDisabled = value;
		}
	}

	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DevCoSerialize]
	[Description("Indicates the image-list index value of the disabled image")]
	[DefaultValue(-1)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Category("Images")]
	public int ImageDisabledIndex
	{
		get
		{
			return Cells[0].Images.ImageDisabledIndex;
		}
		set
		{
			SizeChanged = true;
			Cells[0].Images.ImageDisabledIndex = value;
		}
	}

	[DevCoSerialize]
	[DefaultValue(null)]
	[Description("Indicates image displayed when the tree node is in the unselected state.")]
	[Browsable(true)]
	[Category("Images")]
	public Image Image
	{
		get
		{
			return ӥ[0].Images.Image;
		}
		set
		{
			SizeChanged = true;
			ӥ[0].Images.Image = value;
		}
	}

	[Browsable(true)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Category("Images")]
	[Description("Indicates the image-list key value of the default image that is displayed by the tree nodes.")]
	[DevCoSerialize]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string ImageKey
	{
		get
		{
			return Cells[0].Images.ImageKey;
		}
		set
		{
			SizeChanged = true;
			Cells[0].Images.ImageKey = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(-1)]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DevCoSerialize]
	[Description("Indicates the image-list index value of the default image that is displayed by the tree nodes.")]
	[Category("Images")]
	public int ImageIndex
	{
		get
		{
			return Cells[0].Images.ImageIndex;
		}
		set
		{
			SizeChanged = true;
			Cells[0].Images.ImageIndex = value;
		}
	}

	[DevCoSerialize]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Images")]
	[Description("Indicates the image displayed when mouse is over the tree node.")]
	public Image ImageMouseOver
	{
		get
		{
			return Cells[0].Images.ImageMouseOver;
		}
		set
		{
			Cells[0].Images.ImageMouseOver = value;
			SizeChanged = true;
		}
	}

	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	[DevCoSerialize]
	[Category("Images")]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the image-list index value of the image that is displayed by the tree nodes when mouse is over the node.")]
	public int ImageMouseOverIndex
	{
		get
		{
			return Cells[0].Images.ImageMouseOverIndex;
		}
		set
		{
			SizeChanged = true;
			Cells[0].Images.ImageMouseOverIndex = value;
		}
	}

	[Description("Indicates the image-list key value of the image that is displayed by the tree nodes when mouse is over the node.")]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DevCoSerialize]
	[Category("Images")]
	[Browsable(true)]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string ImageMouseOverKey
	{
		get
		{
			return Cells[0].Images.ImageMouseOverKey;
		}
		set
		{
			SizeChanged = true;
			Cells[0].Images.ImageMouseOverKey = value;
		}
	}

	[Browsable(true)]
	[DevCoSerialize]
	[DefaultValue(null)]
	[Category("Images")]
	[Description("Indicates the image displayed when node is expanded.")]
	public Image ImageExpanded
	{
		get
		{
			return Cells[0].Images.ImageExpanded;
		}
		set
		{
			Cells[0].Images.ImageExpanded = value;
			SizeChanged = true;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	[DevCoSerialize]
	[Browsable(true)]
	[Description("Indicates the image-list index value of the image that is displayed by the tree nodes when node is expanded.")]
	[Category("Images")]
	public int ImageExpandedIndex
	{
		get
		{
			return Cells[0].Images.ImageExpandedIndex;
		}
		set
		{
			SizeChanged = true;
			Cells[0].Images.ImageExpandedIndex = value;
		}
	}

	[DefaultValue("")]
	[DevCoSerialize]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Description("Indicates the image-list key value of the image that is displayed by the tree nodes when node is expanded.")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[Category("Images")]
	public string ImageExpandedKey
	{
		get
		{
			return Cells[0].Images.ImageExpandedKey;
		}
		set
		{
			SizeChanged = true;
			Cells[0].Images.ImageExpandedKey = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImageList ImageList => TreeControl?.ImageList;

	[Editor("DevComponents.AdvTree.Design.NodeConnectorTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the nested nodes connector.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[Category("Connectors")]
	[DefaultValue(null)]
	public NodeConnector ParentConnector
	{
		get
		{
			return Ӹ;
		}
		set
		{
			if (Ӹ != value)
			{
				if (Ӹ != null)
				{
					Ӹ.AppearanceChanged -= ConnectorAppearanceChanged;
				}
				if (value != null)
				{
					value.AppearanceChanged += ConnectorAppearanceChanged;
				}
				Ӹ = value;
				OnDisplayChanged();
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicated whether node is visible")]
	[DevCoSerialize]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool Visible
	{
		get
		{
			return Ӻ;
		}
		set
		{
			Ӻ = value;
			OnVisibleChanged();
			SizeChanged = true;
		}
	}

	[DevCoSerialize]
	[DefaultValue(false)]
	[Category("Command Button")]
	[Browsable(true)]
	[Description("Indicates visibility of command button.")]
	internal bool CommandButton
	{
		get
		{
			return ӻ;
		}
		set
		{
			ӻ = value;
			OnDisplayChanged();
		}
	}

	internal bool SelectedConnectorMarker
	{
		get
		{
			return Ԁ;
		}
		set
		{
			Ԁ = value;
		}
	}

	[Browsable(false)]
	public bool IsDragNode
	{
		get
		{
			return _ԋ;
		}
		internal set
		{
			if (_ԋ == value)
			{
				return;
			}
			_ԋ = value;
			if (Ӥ == null)
			{
				return;
			}
			foreach (Node node in Nodes)
			{
				node.IsDragNode = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsSelectionVisible
	{
		get
		{
			return _Ԍ;
		}
		set
		{
			if (_Ԍ != value)
			{
				_Ԍ = value;
				Invalidate();
			}
		}
	}

	[Description("additional indent for the child nodes that is added to the AdvTree.Indent value when performing child node layout")]
	[DefaultValue(0)]
	[Category("Appearance")]
	public int NodesIndent
	{
		get
		{
			return _ԍ;
		}
		set
		{
			_ԍ = value;
			InvalidateLayout(invalidateChildNodes: true);
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public virtual bool CellNavigationEnabled
	{
		get
		{
			return _Ԏ;
		}
		set
		{
			_Ԏ = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether style background that is applied to the node is drawn across the width of the tree control instead of only behind the node content.")]
	[Category("Appearance")]
	public bool FullRowBackground
	{
		get
		{
			return _ԏ;
		}
		set
		{
			if (_ԏ != value)
			{
				_ԏ = value;
				SizeChanged = true;
				Invalidate();
			}
		}
	}

	internal int ColumnHeaderHeight
	{
		get
		{
			return Ӳ;
		}
		set
		{
			Ӳ = value;
		}
	}

	internal bool FireHoverEvent
	{
		get
		{
			if (ӛ == null)
			{
				return CellsHaveHostedItemOrTooltip;
			}
			return true;
		}
	}

	private bool CellsHaveHostedItemOrTooltip
	{
		get
		{
			foreach (Cell item in ӥ)
			{
				if (item.HostedItem != null || !string.IsNullOrEmpty(item.Tooltip))
				{
					return true;
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	public virtual AccessibleObject AccessibleObject => CreateAccessibilityInstance();

	[DefaultValue("")]
	[Browsable(true)]
	[Category("Accessibility")]
	[Description("Gets or sets the default action description of the control for use by accessibility client applications.")]
	public virtual string AccessibleDefaultActionDescription
	{
		get
		{
			return _ԑ;
		}
		set
		{
			_ԑ = value;
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[Category("Accessibility")]
	[Description("Gets or sets the description of the control used by accessibility client applications.")]
	public virtual string AccessibleDescription
	{
		get
		{
			return _Ԓ;
		}
		set
		{
			if (_Ԓ != value)
			{
				_Ԓ = value;
				GenerateAccessibilityEvent((AccessibleEvents)32781);
			}
		}
	}

	[Description("Gets or sets the name of the control used by accessibility client applications.")]
	[Category("Accessibility")]
	[Browsable(true)]
	[DefaultValue("")]
	public virtual string AccessibleName
	{
		get
		{
			return _ԓ;
		}
		set
		{
			if (_ԓ != value)
			{
				_ԓ = value;
				GenerateAccessibilityEvent((AccessibleEvents)32780);
			}
		}
	}

	[Category("Accessibility")]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the accessible role of the item.")]
	public virtual AccessibleRole AccessibleRole
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _Ԕ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_Ԕ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsAccessible
	{
		get
		{
			return _ԕ;
		}
		set
		{
			_ԕ = value;
		}
	}

	public event MouseEventHandler NodeMouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = Ӗ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref Ӗ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = Ӗ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref Ӗ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event MouseEventHandler NodeMouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ӗ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ӗ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ӗ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ӗ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event MouseEventHandler NodeMouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = Ә;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref Ә, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = Ә;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref Ә, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler NodeMouseEnter
	{
		add
		{
			EventHandler eventHandler = ә;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ә, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ә;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ә, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler NodeMouseLeave
	{
		add
		{
			EventHandler eventHandler = Ӛ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ӛ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ӛ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ӛ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler NodeMouseHover
	{
		add
		{
			EventHandler eventHandler = ӛ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ӛ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ӛ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ӛ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler NodeClick
	{
		add
		{
			EventHandler eventHandler = Ӝ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ӝ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ӝ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ӝ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler NodeDoubleClick
	{
		add
		{
			EventHandler eventHandler = ӝ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ӝ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ӝ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ӝ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
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

	public Node()
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		ӥ.Ԟ(this);
		ӿ = new ConnectorPointsCollection();
		ӿ.Ԟ(this);
		Cell cell = new Cell();
		Cells.Add(cell);
	}

	public Node(string text)
		: this()
	{
		Text = text;
	}

	public Node(string text, ElementStyle nodeStyle)
		: this()
	{
		Text = text;
		Style = nodeStyle;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (Ӥ != null)
			{
				Node[] array = new Node[Ӥ.Count];
				Ӥ.CopyTo(array);
				Node[] array2 = array;
				foreach (Node node in array2)
				{
					node.Dispose();
				}
			}
			foreach (Cell cell in Cells)
			{
				cell.Dispose();
			}
			if (Ӷ != null)
			{
				Ӷ.StyleChanged -= ElementStyleChanged;
			}
			if (ӯ != null)
			{
				ӯ.StyleChanged -= ElementStyleChanged;
			}
			if (Ӯ != null)
			{
				Ӯ.StyleChanged -= ElementStyleChanged;
			}
			if (Ӱ != null)
			{
				Ӱ.StyleChanged -= ElementStyleChanged;
			}
			if (Ӹ != null)
			{
				Ӹ.AppearanceChanged -= ConnectorAppearanceChanged;
			}
		}
		base.Dispose(disposing);
	}

	internal void SetBounds(Rectangle r)
	{
		ӟ = r;
	}

	internal void SetContentBounds(Rectangle r)
	{
		Ӣ = r;
	}

	internal void SetCellsBounds(Rectangle r)
	{
		ӡ = r;
	}

	internal void SetParent(Node parent)
	{
		if (parent == null)
		{
			RemoveHostedControls(enumChildren: true);
		}
		if (ӭ == parent)
		{
			return;
		}
		ӭ = parent;
		foreach (Cell item in ӥ)
		{
			if (item.HostedControl != null)
			{
				item.AddHostedControlToTree();
			}
		}
		OnParentChanged();
	}

	internal void InvokeOnParentChanged()
	{
		OnParentChanged();
	}

	protected virtual void OnParentChanged()
	{
	}

	internal void RemoveHostedControls(bool enumChildren)
	{
		foreach (Cell item in ӥ)
		{
			if (item.HostedControl != null && item.HostedControl.Parent is AdvTree)
			{
				item.RemoveHostedControlFromTree();
			}
		}
		if (!enumChildren || Ӥ == null)
		{
			return;
		}
		foreach (Node item2 in Ӥ)
		{
			item2.RemoveHostedControls(enumChildren: true);
		}
	}

	internal void SetExpandPartRectangle(Rectangle r)
	{
		Ӭ = r;
	}

	public void SetChecked(bool value, eTreeAction actionSource)
	{
		SetChecked((CheckState)(value ? 1 : 0), actionSource);
	}

	public void SetChecked(CheckState value, eTreeAction actionSource)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		ӥ[0].SetChecked(value, actionSource);
	}

	internal void SetEditing(bool b)
	{
		Ӽ = b;
	}

	public void SetSelectedCell(Cell value, eTreeAction actionSource)
	{
		AdvTree treeControl = TreeControl;
		if (!IsSelected && value != null && treeControl != null)
		{
			treeControl.SelectedNode = this;
		}
		Cell cell = null;
		Cell cell2 = null;
		foreach (Cell item in ӥ)
		{
			if (item.IsSelected)
			{
				cell = item;
			}
			if (item == value && item.CanSelect)
			{
				cell2 = item;
				item.SetSelected(selected: true, actionSource);
			}
			else
			{
				item.SetSelected(selected: false, actionSource);
			}
		}
		if (treeControl != null)
		{
			treeControl._0651(this);
			if (value == null || (cell != null && value != cell))
			{
				treeControl.ٳ(new AdvTreeCellEventArgs(actionSource, cell));
			}
			if (cell2 != null && cell != cell2)
			{
				treeControl.ٲ(new AdvTreeCellEventArgs(actionSource, cell2));
			}
		}
	}

	internal Cell GetSelectedCell()
	{
		foreach (Cell item in ӥ)
		{
			if (item.IsSelected)
			{
				return item;
			}
		}
		return null;
	}

	internal void SelectFirstCell(eTreeAction actionSource)
	{
		foreach (Cell item in ӥ)
		{
			if (item.CanSelect)
			{
				SetSelectedCell(item, actionSource);
				break;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageDisabled()
	{
		TypeDescriptor.GetProperties(this)["ImageDisabled"].SetValue(this, null);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImage()
	{
		TypeDescriptor.GetProperties(this)["Image"].SetValue(this, null);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageMouseOver()
	{
		TypeDescriptor.GetProperties(this)["ImageMouseOver"].SetValue(this, null);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageExpanded()
	{
		TypeDescriptor.GetProperties(this)["ImageExpanded"].SetValue(this, null);
	}

	public void InternalDeselected(eTreeAction action)
	{
		OnDeselected(action);
	}

	protected virtual void OnDeselected(eTreeAction action)
	{
	}

	protected virtual void OnSelected(eTreeAction action)
	{
	}

	public void InternalSelected(eTreeAction action)
	{
		OnSelected(action);
	}

	public Cell GetCellAt(int x, int y)
	{
		Cell result = null;
		foreach (Cell item in ӥ)
		{
			if (item.IsVisible && item.Bounds.Contains(x, y))
			{
				result = item;
				break;
			}
		}
		return result;
	}

	internal void InternalKeyDown(KeyEventArgs e)
	{
		OnKeyDown(e);
	}

	protected virtual void OnKeyDown(KeyEventArgs e)
	{
	}

	public void InvalidateLayout()
	{
		_175C.ព(this, ភ: false);
	}

	public void InvalidateLayout(bool invalidateChildNodes)
	{
		_175C.ព(this, invalidateChildNodes);
	}

	public void Invalidate()
	{
		TreeControl?._0651(this);
	}

	public void BeginEdit()
	{
		BeginEdit(0);
	}

	public void BeginEdit(string initialText)
	{
		BeginEdit(0, initialText);
	}

	public void BeginEdit(int iColumnIndex)
	{
		BeginEdit(iColumnIndex, null);
	}

	public void BeginEdit(int iColumnIndex, string initialText)
	{
		AdvTree treeControl = TreeControl;
		if (treeControl != null)
		{
			Cell cell = Cells[iColumnIndex];
			if (!cell.IsEditable)
			{
				throw new InvalidOperationException("Cell is not editable, either Cell.Editable=false or ColumnHeader.Editable=false.");
			}
			treeControl.ځ(Cells[iColumnIndex], eTreeAction.Code, initialText);
		}
	}

	protected virtual Node CreateCopyInstance()
	{
		return new Node();
	}

	public virtual Node Copy()
	{
		Node node = CreateCopyInstance();
		CopyTo(node);
		return node;
	}

	protected virtual void CopyTo(Node n)
	{
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		n.CellLayout = CellLayout;
		n.Cells.Clear();
		foreach (Cell cell in Cells)
		{
			n.Cells.Add(cell.Copy());
		}
		n.Checked = Checked;
		n.CommandButton = CommandButton;
		n.ExpandVisibility = ExpandVisibility;
		n.Image = Image;
		n.ImageDisabled = ImageDisabled;
		n.ImageDisabledIndex = ImageDisabledIndex;
		n.ImageExpanded = ImageExpanded;
		n.ImageExpandedIndex = ImageExpandedIndex;
		n.ImageIndex = ImageIndex;
		n.ImageMouseOver = ImageMouseOver;
		n.ImageMouseOverIndex = ImageMouseOverIndex;
		n.Expanded = Expanded;
		n.DragDropEnabled = DragDropEnabled;
		n.CellLayout = CellLayout;
		n.CellPartLayout = CellPartLayout;
		n.CheckBoxAlignment = CheckBoxAlignment;
		n.CheckBoxStyle = CheckBoxStyle;
		n.CheckBoxThreeState = CheckBoxThreeState;
		n.CheckBoxVisible = CheckBoxVisible;
		n.CheckState = CheckState;
		n.ContextMenu = ContextMenu;
		n.DataKey = DataKey;
		n.DragDropEnabled = DragDropEnabled;
		n.Enabled = Enabled;
		n.FullRowBackground = FullRowBackground;
		n.ImageAlignment = ImageAlignment;
		n.Selectable = Selectable;
		foreach (ColumnHeader nodesColumn in NodesColumns)
		{
			n.NodesColumns.Add(nodesColumn.Copy());
		}
		n.ParentConnector = ParentConnector;
		n.Style = Style;
		n.StyleExpanded = StyleExpanded;
		n.StyleMouseOver = StyleMouseOver;
		n.StyleSelected = StyleSelected;
		n.Tag = Tag;
		n.DataKey = DataKey;
		n.Text = Text;
		n.Visible = Visible;
		n.Name = Name;
	}

	public virtual Node DeepCopy()
	{
		Node node = Copy();
		foreach (Node node2 in Nodes)
		{
			node.Nodes.Add(node2.DeepCopy());
		}
		return node;
	}

	public void Collapse()
	{
		SetExpanded(e: false, eTreeAction.Code);
	}

	public void Collapse(eTreeAction action)
	{
		SetExpanded(e: false, action);
	}

	public void CollapseAll()
	{
		AdvTree treeControl = TreeControl;
		treeControl?.BeginUpdate();
		Collapse();
		foreach (Node node in Nodes)
		{
			node.CollapseAll();
		}
		treeControl?.EndUpdate();
	}

	public void EndEdit(bool cancelChanges)
	{
		if (!IsEditing)
		{
			return;
		}
		AdvTree treeControl = TreeControl;
		if (treeControl != null)
		{
			if (cancelChanges)
			{
				treeControl.ڇ(eTreeAction.Code);
			}
			else
			{
				treeControl.څ(eTreeAction.Code);
			}
		}
	}

	public void EnsureVisible()
	{
		_175C._1771(this, AdvTreeSettings.SelectedScrollIntoViewHorizontal);
	}

	public void EnsureVisible(eEnsureVisibleOption ensureVisibleOption)
	{
		_175C._1771(this, AdvTreeSettings.SelectedScrollIntoViewHorizontal, ensureVisibleOption);
	}

	public void Expand()
	{
		SetExpanded(e: true, eTreeAction.Code);
	}

	public void Expand(eTreeAction action)
	{
		SetExpanded(e: true, action);
	}

	public void ExpandAll()
	{
		ExpandAll(eTreeAction.Code);
	}

	public void ExpandAll(eTreeAction action)
	{
		AdvTree treeControl = TreeControl;
		treeControl?.BeginUpdate();
		try
		{
			if (!Expanded)
			{
				Expand(action);
			}
			foreach (Node node in Nodes)
			{
				node.Expand(action);
				node.ExpandAll(action);
			}
		}
		finally
		{
			treeControl?.EndUpdate();
		}
	}

	public void Remove()
	{
		if (Parent != null)
		{
			Parent.Nodes.Remove(this);
		}
		else if (TreeControl != null && TreeControl.Nodes.Contains(this))
		{
			TreeControl.Nodes.Remove(this);
		}
	}

	public void Remove(eTreeAction source)
	{
		if (Parent != null)
		{
			Parent.Nodes.Remove(this, source);
		}
		else if (TreeControl != null && TreeControl.Nodes.Contains(this))
		{
			TreeControl.Nodes.Remove(this, source);
		}
	}

	public void Toggle()
	{
		if (Expanded)
		{
			Collapse();
		}
		else
		{
			Expand();
		}
	}

	public void Toggle(eTreeAction action)
	{
		if (Expanded)
		{
			Collapse(action);
		}
		else
		{
			Expand(action);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int count = Cells.Count;
		for (int i = 0; i < count; i++)
		{
			stringBuilder.Append(Cells[i].Text);
			if (i + 1 < count)
			{
				stringBuilder.Append(", ");
			}
		}
		return stringBuilder.ToString();
	}

	internal void BeforeCellCheck(Cell cell, AdvTreeCellBeforeCheckEventArgs e)
	{
		OnBeforeCellCheck(cell, e);
	}

	protected virtual void OnBeforeCellCheck(Cell cell, AdvTreeCellBeforeCheckEventArgs e)
	{
	}

	internal void OnCellInserted(Cell cell)
	{
		SizeChanged = true;
		OnDisplayChanged();
	}

	internal void OnCellRemoved(Cell cell)
	{
		SizeChanged = true;
		OnDisplayChanged();
	}

	private void OnSizeChanged()
	{
		if (ӱ)
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null && !treeControl.IsUpdateSuspended)
			{
				treeControl._0654();
				((Control)treeControl).Invalidate();
			}
		}
	}

	internal void OnImageChanged()
	{
		if (!ӱ)
		{
			if (Parent != null)
			{
				Parent.OnChildNodesSizeChanged();
			}
			else
			{
				SizeChanged = true;
			}
			OnDisplayChanged();
		}
	}

	internal void OnChildNodesSizeChanged()
	{
		foreach (Node node in Nodes)
		{
			node.SizeChanged = true;
			node.OnChildNodesSizeChanged();
		}
	}

	private void OnColumnHeaderSizeChanged(object sender, EventArgs e)
	{
		if (Expanded)
		{
			SizeChanged = true;
		}
	}

	private void SetExpanded(bool e, eTreeAction action)
	{
		if (e == Ӫ)
		{
			return;
		}
		INodeNotify iNodeNotify = GetINodeNotify();
		if (iNodeNotify != null)
		{
			AdvTreeNodeCancelEventArgs advTreeNodeCancelEventArgs = new AdvTreeNodeCancelEventArgs(action, this);
			if (e)
			{
				iNodeNotify.OnBeforeExpand(advTreeNodeCancelEventArgs);
			}
			else
			{
				iNodeNotify.OnBeforeCollapse(advTreeNodeCancelEventArgs);
			}
			if (advTreeNodeCancelEventArgs.Cancel)
			{
				return;
			}
		}
		if (_175C._175D == 0)
		{
			return;
		}
		OnExpandChanging(e, action);
		Ӫ = e;
		iNodeNotify?.ExpandedChanged(this);
		if (iNodeNotify != null)
		{
			AdvTreeNodeEventArgs e2 = new AdvTreeNodeEventArgs(action, this);
			if (e)
			{
				iNodeNotify.OnAfterExpand(e2);
			}
			else
			{
				iNodeNotify.OnAfterCollapse(e2);
			}
		}
		foreach (Node node in Nodes)
		{
			node.OnParentExpandedChanged(Ӫ);
		}
		SizeChanged = true;
		OnDisplayChanged();
	}

	protected virtual void OnExpandChanging(bool expanded, eTreeAction action)
	{
	}

	internal void OnParentExpandedChanged(bool expanded)
	{
		foreach (Cell cell in Cells)
		{
			cell.OnParentExpandedChanged(expanded);
		}
		foreach (Node node in Nodes)
		{
			node.OnParentExpandedChanged(expanded);
		}
	}

	private AdvTree GetTreeControl()
	{
		Node node = this;
		while (node.Parent != null)
		{
			node = node.Parent;
		}
		return node.ӵ;
	}

	private INodeNotify GetINodeNotify()
	{
		AdvTree treeControl = TreeControl;
		if (treeControl != null)
		{
			return treeControl;
		}
		return null;
	}

	private void ElementStyleChanged(object sender, EventArgs e)
	{
		SizeChanged = true;
		OnDisplayChanged();
	}

	internal virtual void OnDisplayChanged()
	{
		AdvTree treeControl = TreeControl;
		if (treeControl == null || treeControl.SuspendPaint)
		{
			return;
		}
		if (SizeChanged)
		{
			if (!treeControl.Կ)
			{
				treeControl.RecalcLayout();
			}
			else
			{
				treeControl._0654();
			}
		}
		((Control)treeControl).Invalidate(true);
	}

	private void ConnectorAppearanceChanged(object sender, EventArgs e)
	{
		OnDisplayChanged();
	}

	internal void OnChildNodeInserted(Node node)
	{
		SizeChanged = true;
		if (node.Cells.Count > 1)
		{
			node.SizeChanged = true;
		}
	}

	internal void OnChildNodeRemoved(Node node)
	{
		SizeChanged = true;
		if (node.IsSelected && TreeControl != null)
		{
			TreeControl.SelectedNode = null;
		}
	}

	internal void OnVisibleChanged()
	{
		foreach (Cell cell in Cells)
		{
			cell.OnVisibleChanged();
		}
		foreach (Node node in Nodes)
		{
			node.OnVisibleChanged();
		}
	}

	public void CreateCells()
	{
		AdvTree treeControl = TreeControl;
		if (Parent == null && treeControl == null)
		{
			throw new NullReferenceException("Node must have a parent or be added to AdvTree.Nodes collection to use this method.");
		}
		Node parent = Parent;
		int num = 0;
		if (parent != null && parent.NodesColumns.Count > 0)
		{
			num = parent.NodesColumns.Count;
		}
		else if (treeControl != null && treeControl.Columns.Count > 0)
		{
			num = treeControl.Columns.Count;
		}
		if (num != 0)
		{
			for (int i = Cells.Count; i < num; i++)
			{
				Cells.Add(new Cell());
			}
		}
	}

	internal void OnNodesCleared()
	{
		SizeChanged = true;
	}

	internal bool IsKeyboardNavigationEnabled(KeyEventArgs e)
	{
		return CanKeyboardNavigate(e);
	}

	protected virtual bool CanKeyboardNavigate(KeyEventArgs e)
	{
		return true;
	}

	internal void InvokeKeyboardPaste(KeyEventArgs args)
	{
		OnKeyboardPaste(args);
	}

	protected virtual void OnKeyboardPaste(KeyEventArgs args)
	{
	}

	internal void InvokeKeyboardCopy(KeyEventArgs args)
	{
		OnKeyboardCopy(args);
	}

	protected virtual void OnKeyboardCopy(KeyEventArgs args)
	{
	}

	protected internal virtual void InvokeNodeMouseDown(object sender, MouseEventArgs e)
	{
		if (Ӗ != null)
		{
			Ӗ.Invoke((object)this, e);
		}
		foreach (Cell item in ӥ)
		{
			item.InvokeNodeMouseDown(sender, e);
		}
	}

	protected internal virtual void InvokeNodeMouseUp(object sender, MouseEventArgs e)
	{
		if (ӗ != null)
		{
			ӗ.Invoke((object)this, e);
		}
		foreach (Cell item in ӥ)
		{
			item.InvokeNodeMouseUp(sender, e);
		}
	}

	protected internal virtual void InvokeNodeMouseMove(object sender, MouseEventArgs e)
	{
		if (Ә != null)
		{
			Ә.Invoke((object)this, e);
		}
		foreach (Cell item in ӥ)
		{
			item.InvokeNodeMouseMove(sender, e);
		}
	}

	public void RaiseClick()
	{
		AdvTree treeControl = TreeControl;
		TreeNodeMouseEventArgs treeNodeMouseEventArgs = new TreeNodeMouseEventArgs(this, (MouseButtons)0, 0, 0, 0, 0);
		if (treeControl == null)
		{
			InvokeNodeClick(this, treeNodeMouseEventArgs);
		}
		else
		{
			treeControl.ڔ(treeNodeMouseEventArgs);
		}
	}

	protected internal virtual void InvokeNodeClick(object sender, EventArgs e)
	{
		if (Ӝ != null)
		{
			Ӝ(this, e);
		}
		foreach (Cell item in ӥ)
		{
			item.InvokeNodeClick(sender, e);
		}
	}

	protected internal virtual void InvokeNodeDoubleClick(object sender, EventArgs e)
	{
		OnNodeDoubleClick(e);
	}

	protected virtual void OnNodeDoubleClick(EventArgs e)
	{
		if (ӝ != null)
		{
			ӝ(this, e);
		}
	}

	protected internal virtual void InvokeNodeMouseEnter(object sender, EventArgs e)
	{
		if (ә != null)
		{
			ә(this, e);
		}
	}

	protected internal virtual void InvokeNodeMouseLeave(object sender, EventArgs e)
	{
		if (Ӛ != null)
		{
			Ӛ(this, e);
		}
		foreach (Cell item in ӥ)
		{
			item.InvokeNodeMouseLeave(sender, e);
		}
	}

	protected internal virtual void InvokeNodeMouseHover(object sender, EventArgs e)
	{
		if (ӛ != null)
		{
			ӛ(this, e);
		}
		foreach (Cell item in ӥ)
		{
			if (item.IsMouseOver)
			{
				item.InvokeNodeMouseHover(sender, e);
			}
		}
	}

	protected internal virtual void InvokeMarkupLinkClick(Cell cell, MarkupLinkClickEventArgs args)
	{
		if (Ӟ != null)
		{
			Ӟ(cell, args);
		}
		TreeControl?.ۃ(cell, args);
	}

	protected virtual AccessibleObject CreateAccessibilityInstance()
	{
		if (_Ԑ == null)
		{
			_Ԑ = new NodeAccessibleObject(this);
		}
		return (AccessibleObject)(object)_Ԑ;
	}

	internal void GenerateAccessibilityEvent(AccessibleEvents e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		AdvTree treeControl = TreeControl;
		if (treeControl != null && ((Control)treeControl).AccessibilityObject is AdvTree.AdvTreeAccessibleObject advTreeAccessibleObject)
		{
			advTreeAccessibleObject._၈(this, e);
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is Node)
		{
			Node node = obj as Node;
			return Utilities._17D5(Text, node.Text);
		}
		return -1;
	}
}
