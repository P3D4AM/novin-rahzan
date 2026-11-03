using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display;

public class NodeTreeDisplay : NodeDisplay
{
	private class _2798
	{
		public Graphics ܓ;

		public Font _2711;

		public Rectangle _2799 = Rectangle.Empty;

		public Point ܭ = Point.Empty;

		public ElementStyle _2717;

		public ElementStyle _279A;

		public ElementStyle _279B;

		public ElementStyle _279C;

		public ElementStyle _279D;

		public ElementStyle _279E;

		public ElementStyle _279F;

		public ElementStyle _27A0;

		public ElementStyle _27A1;

		public ElementStyleCollection _2719;

		public ConnectorRendererEventArgs _27A2;

		public NodeExpandPartRendererEventArgs _27A3;

		public SelectionRendererEventArgs _27A4;

		public bool _27A5;

		public TreeRenderer Ԧ;

		public NodeRendererEventArgs _27A6;

		public bool _27A7;

		public Rectangle ᶮ = Rectangle.Empty;

		public ColorScheme ԫ;

		public bool _27A8;

		public Color _27A9 = Color.Empty;

		public bool _27AA;

		public Brush _27AB;

		public bool _27AC;

		public bool _27AD;

		public eView ݎ;

		public Color _27AE = Color.Empty;
	}

	private ElementStyleDisplayInfo _2732 = new ElementStyleDisplayInfo();

	private NodeCellRendererEventArgs _2733 = new NodeCellRendererEventArgs();

	private NodeSystemRenderer _2734 = new NodeSystemRenderer();

	public NodeSystemRenderer SystemRenderer
	{
		get
		{
			return _2734;
		}
		set
		{
			_2734 = value;
		}
	}

	public bool IsBackgroundSelection => Tree.SelectionBoxStyle != eSelectionStyle.NodeMarker;

	public NodeTreeDisplay(AdvTree tree)
		: base(tree)
	{
		_2734.ColorTable = new TreeColorTable();
		_135B._135C(_2734.ColorTable, new ColorFactory());
	}

	public TreeRenderer GetTreeRenderer()
	{
		TreeRenderer ԧ = _2734;
		if (Tree.Ԧ != null && Tree.ԧ == eNodeRenderMode.Custom)
		{
			ԧ = Tree.Ԧ;
		}
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			ԧ._2760 = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			ԧ.ColorTable = ԧ._2760.AdvTree;
		}
		return ԧ;
	}

	private _2798 _2735(Graphics _٥, Rectangle _٩)
	{
		_2798 _2798 = new _2798();
		_2798.ܓ = _٥;
		_2798._2711 = ((Control)Tree).Font;
		_2798._2799 = _٩;
		_2798.ܭ = Offset;
		_2798._2719 = Tree.Styles;
		_2798._27A7 = IsBackgroundSelection;
		_2798.ᶮ = ᝈ._1756(Tree.BackgroundStyle, ((Control)Tree).ClientRectangle);
		_2798.ԫ = Tree.ԫ;
		_2798._27A8 = Tree.GridRowLines;
		_2798._27A9 = Tree.GridLinesColor;
		_2798._27AA = Tree.SelectionPerCell;
		_2798._27AB = ᙜ(Tree.AlternateRowColor);
		_2798._27AC = !Tree.AlternateRowColor.IsEmpty;
		_2798.ݎ = Tree.View;
		_2798._27AE = Tree.TileGroupLineColor;
		_2798.Ԧ = _2734;
		if (Tree.Ԧ != null && Tree.ԧ == eNodeRenderMode.Custom)
		{
			_2798.Ԧ = Tree.Ԧ;
		}
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			_2798.Ԧ._2760 = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			_2798.Ԧ.ColorTable = _2798.Ԧ._2760.AdvTree;
		}
		_2798._27A6 = new NodeRendererEventArgs();
		if (Tree.NodeStyle != null)
		{
			_2798._2717 = Tree.NodeStyle;
		}
		else
		{
			_2798._2717 = GetDefaultNodeStyle();
		}
		if (Tree.NodeStyleExpanded != null)
		{
			_2798._279A = Tree.NodeStyleExpanded;
		}
		if (Tree.NodeStyleSelected != null)
		{
			_2798._279B = Tree.NodeStyleSelected;
		}
		if (Tree.NodeStyleMouseOver != null)
		{
			_2798._279C = Tree.NodeStyleMouseOver;
		}
		if (Tree.CellStyleDefault != null)
		{
			_2798._279D = Tree.CellStyleDefault;
		}
		if (Tree.CellStyleDisabled != null)
		{
			_2798._27A1 = Tree.CellStyleDisabled;
		}
		else
		{
			_2798._27A1 = ElementStyle.GetDefaultDisabledCellStyle();
		}
		if (Tree.CellStyleMouseDown != null)
		{
			_2798._279E = Tree.CellStyleMouseDown;
		}
		if (Tree.CellStyleMouseOver != null)
		{
			_2798._279F = Tree.CellStyleMouseOver;
		}
		if (Tree.CellStyleSelected != null)
		{
			_2798._27A0 = Tree.CellStyleSelected;
		}
		else
		{
			_2798._27A0 = ElementStyle.GetDefaultSelectedCellStyle();
		}
		_2798._27A2 = new ConnectorRendererEventArgs();
		_2798._27A2.Graphics = _2798.ܓ;
		_2798._27A3 = new NodeExpandPartRendererEventArgs(_2798.ܓ);
		_2798._27A3.BackColor = Tree._065A(Tree.ExpandBackColor, Tree.ExpandBackColorSchemePart);
		_2798._27A3.BackColor2 = Tree._065A(Tree.ExpandBackColor2, Tree.ExpandBackColor2SchemePart);
		_2798._27A3.BackColorGradientAngle = Tree.ExpandBackColorGradientAngle;
		_2798._27A3.BorderColor = Tree._065A(Tree.ExpandBorderColor, Tree.ExpandBorderColorSchemePart);
		_2798._27A3.ExpandLineColor = Tree._065A(Tree.ExpandLineColor, Tree.ExpandLineColorSchemePart);
		_2798._27A3.Graphics = _2798.ܓ;
		_2798._27A3._276F = Tree.ExpandButtonType;
		_2798._27A3.ExpandImage = Tree.ExpandImage;
		_2798._27A3.ExpandImageCollapse = Tree.ExpandImageCollapse;
		_2798._27A4 = new SelectionRendererEventArgs();
		_2798._27A4.Graphics = _2798.ܓ;
		_2798._27A4.SelectionBoxStyle = Tree.SelectionBoxStyle;
		_2798._27A4.TreeActive = Tree.IsKeyboardFocusWithin || !Tree.SelectionFocusAware;
		return _2798;
	}

	private Brush ᙜ(Color _065B)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		if (_065B.IsEmpty)
		{
			return null;
		}
		return (Brush)new SolidBrush(_065B);
	}

	public override void Paint(Graphics g, Rectangle clipRectangle)
	{
		base.Paint(g, clipRectangle);
		_2733._2751 = Tree.CheckBoxImageChecked;
		_2733._2752 = Tree.CheckBoxImageUnChecked;
		_2733._2753 = Tree.CheckBoxImageIndeterminate;
		_2733.က = new ItemPaintArgs(null, (Control)(object)Tree, g, new ColorScheme());
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			_2734._2760 = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			_2733.က.Colors = _2734._2760.LegacyColors;
			_2733.က.ໝ = GlobalManager.Renderer;
		}
		_2798 _2798 = _2735(g, clipRectangle);
		if (Tree.SelectedPathConnector != null && Tree.SelectedNode != null)
		{
			for (Node node = Tree.SelectedNode; node != null; node = node.Parent)
			{
				node.SelectedConnectorMarker = true;
			}
		}
		if (_2798._27AC)
		{
			_2736(_2798);
		}
		if (Tree.GridColumnLines && Tree.Columns.Count > 0)
		{
			_2738(_2798);
		}
		if (Tree.ԥ != null && Tree.ԥ.Count > 0)
		{
			_273A(_2798, Tree.ԥ);
		}
		if (_2798._27A7)
		{
			_273E(_2798);
		}
		if (Tree.HotTracking)
		{
			_2740(_2798);
		}
		Rectangle rectangle = Tree._06D9();
		Rectangle clientRectangle = ((Control)Tree).ClientRectangle;
		if (rectangle == clipRectangle || clientRectangle == clipRectangle || clipRectangle.IsEmpty)
		{
			__0708.Clear();
		}
		if (Tree.DisplayRootNode != null)
		{
			_2742(Tree.DisplayRootNode, _2798);
		}
		else
		{
			_2741(Tree.Nodes, _2798);
		}
		Node dragNode = Tree.GetDragNode();
		if (dragNode != null && dragNode.Parent == null)
		{
			Point ܭ = _2798.ܭ;
			_2798.ܭ = Point.Empty;
			_2742(dragNode, _2798);
			_2798.ܭ = ܭ;
		}
		if (Tree.IsDragDropInProgress && Tree.PaintDragDropInsertMarker)
		{
			_273C(_2798, Tree.ۋ(Tree._06D7()));
		}
		if (!_2798._27A7)
		{
			_273E(_2798);
		}
		if (_2798._27AB != null)
		{
			_2798._27AB.Dispose();
			_2798._27AB = null;
		}
	}

	private void _2736(_2798 Ⴊ)
	{
		if (Tree.DisplayRootNode != null)
		{
			Ⴊ._27AD = true;
			_2736(Ⴊ, Tree.DisplayRootNode.Nodes);
		}
		else
		{
			_2736(Ⴊ, Tree.Nodes);
		}
	}

	private void _2736(_2798 Ⴊ, NodeCollection _2737)
	{
		foreach (Node item in _2737)
		{
			if (item.Visible)
			{
				Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, item, Ⴊ.ܭ);
				if (nodeRectangle.Y > Ⴊ.ᶮ.Bottom)
				{
					break;
				}
				if (nodeRectangle.Y >= Ⴊ.ᶮ.Y && Ⴊ._27AD)
				{
					Rectangle rectangle = new Rectangle(Ⴊ.ᶮ.X, nodeRectangle.Y - 1, Ⴊ.ᶮ.Width, nodeRectangle.Height + 1);
					Ⴊ.ܓ.FillRectangle(Ⴊ._27AB, rectangle);
				}
				Ⴊ._27AD = !Ⴊ._27AD;
				if (item.Expanded && item.HasChildNodes)
				{
					_2736(Ⴊ, item.Nodes);
				}
			}
		}
	}

	public void ExternalPaintNode(Node node, Graphics g, Rectangle bounds)
	{
		_2733._2751 = Tree.CheckBoxImageChecked;
		_2733._2752 = Tree.CheckBoxImageUnChecked;
		_2733._2753 = Tree.CheckBoxImageIndeterminate;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			_2734._2760 = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		}
		_2798 _2798 = _2735(g, bounds);
		_2798._27A3 = null;
		_2798._27A2 = null;
		_2798.ܭ = new Point(bounds.X - node.BoundsRelative.X, bounds.Y - node.BoundsRelative.Y);
		_2744(node, _2798);
	}

	private void _2738(_2798 Ⴊ)
	{
		_2738(Ⴊ, Tree.Columns, Ⴊ.ᶮ);
	}

	private void _2738(_2798 Ⴊ, ColumnHeaderCollection _2739, Rectangle ശ)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		Color color = (Tree.GridLinesColor.IsEmpty ? Ⴊ.Ԧ.ColorTable.GridLines : Tree.GridLinesColor);
		Graphics ܓ = Ⴊ.ܓ;
		for (int i = 0; i < _2739.Count; i++)
		{
			ColumnHeader columnHeader = _2739.ColumnAtDisplayIndex(i);
			if (!columnHeader.Visible || columnHeader.Bounds.Width <= 0)
			{
				continue;
			}
			Rectangle bounds = columnHeader.Bounds;
			bounds.Offset(Ⴊ.ܭ);
			if (_2739.ParentNode == null)
			{
				bounds.Offset(ശ.Location);
				if (!columnHeader.CellsBackColor.IsEmpty)
				{
					SmoothingMode smoothingMode = ܓ.SmoothingMode;
					ܓ.SmoothingMode = (SmoothingMode)3;
					Rectangle rectangle = new Rectangle(bounds.X - 5, Ⴊ.ᶮ.Y, bounds.Width + 4, Ⴊ.ᶮ.Height);
					SolidBrush val = new SolidBrush(columnHeader.CellsBackColor);
					try
					{
						ܓ.FillRectangle((Brush)(object)val, rectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					ܓ.SmoothingMode = smoothingMode;
				}
			}
			DisplayHelp.DrawLine(ܓ, bounds.Right - ((!columnHeader._171B) ? 1 : 0), ശ.Y, bounds.Right - ((!columnHeader._171B) ? 1 : 0), ശ.Bottom, color, 1);
		}
	}

	private void _273A(_2798 Ⴊ, ArrayList _273B)
	{
		Rectangle ᶮ = Ⴊ.ᶮ;
		if (Tree.VScrollBar != null)
		{
			ᶮ.Width -= ((Control)Tree.VScrollBar).Width;
		}
		foreach (Node item in _273B)
		{
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, item, Ⴊ.ܭ);
			if (!nodeRectangle.IsEmpty)
			{
				nodeRectangle.X = ᶮ.X;
				nodeRectangle.Width = ᶮ.Width;
				nodeRectangle.Inflate(0, 1);
			}
			if (nodeRectangle.IsEmpty || (!nodeRectangle.IntersectsWith(Ⴊ._2799) && !Ⴊ._2799.IsEmpty))
			{
				continue;
			}
			ElementStyle elementStyle = _2743(item, Ⴊ);
			if (elementStyle != null)
			{
				TreeRenderer treeRenderer = Ⴊ.Ԧ;
				if (item.NodeRenderer != null && item.RenderMode == eNodeRenderMode.Custom)
				{
					treeRenderer = item.NodeRenderer;
				}
				if (elementStyle != null)
				{
					Ⴊ._27A6.Graphics = Ⴊ.ܓ;
					Ⴊ._27A6.Node = item;
					Ⴊ._27A6.NodeBounds = nodeRectangle;
					Ⴊ._27A6.Style = elementStyle;
					treeRenderer.DrawNodeBackground(Ⴊ._27A6);
				}
			}
		}
	}

	private void _273C(_2798 Ⴊ, Rectangle _273D)
	{
		if (!_273D.IsEmpty)
		{
			Ⴊ.Ԧ.DrawDragDropMarker(new DragDropMarkerRendererEventArgs(Ⴊ.ܓ, _273D));
		}
	}

	private void _273E(_2798 Ⴊ)
	{
		if (!Tree.MultiSelect && Tree.SelectedNode != null && Tree.SelectedNode.Visible && Tree.SelectionBox && (!Tree.HideSelection || ((Control)Tree).Focused))
		{
			_273F(Tree.SelectedNode, Ⴊ);
		}
		else
		{
			if (!Tree.MultiSelect || Tree.SelectedNodes.Count <= 0 || !Tree.SelectionBox || (Tree.HideSelection && !((Control)Tree).Focused))
			{
				return;
			}
			foreach (Node selectedNode in Tree.SelectedNodes)
			{
				if (selectedNode.IsVisible)
				{
					_273F(selectedNode, Ⴊ);
				}
			}
		}
	}

	private void _273F(Node _0653, _2798 Ⴊ)
	{
		if (!_0653.IsSelectionVisible)
		{
			return;
		}
		Ⴊ._27A4.Node = _0653;
		Rectangle bounds;
		if (Tree.SelectionBoxStyle == eSelectionStyle.FullRowSelect)
		{
			if (Ⴊ._27AA)
			{
				Cell ڂ = ((Ⴊ._27A4.Node.SelectedCell != null) ? Ⴊ._27A4.Node.SelectedCell : Ⴊ._27A4.Node.Cells[0]);
				bounds = NodeDisplay._070A(_1757._175B, ڂ, Ⴊ.ܭ);
			}
			else
			{
				bounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, Ⴊ._27A4.Node, Ⴊ.ܭ);
				bounds.X = Ⴊ.ᶮ.X;
				bounds.Width = Ⴊ.ᶮ.Width;
			}
			if (Ⴊ._27A8)
			{
				bounds.Inflate(0, (int)Math.Floor((float)Tree.NodeSpacing / 2f));
			}
			else
			{
				bounds.Inflate(0, (int)Math.Ceiling((float)Tree.NodeSpacing / 2f));
			}
		}
		else if (Tree.SelectionBoxStyle == eSelectionStyle.HighlightCells)
		{
			if (Ⴊ._27AA)
			{
				Cell ڂ2 = ((Ⴊ._27A4.Node.SelectedCell != null) ? Ⴊ._27A4.Node.SelectedCell : Ⴊ._27A4.Node.Cells[0]);
				bounds = NodeDisplay._070A(_1757._175B, ڂ2, Ⴊ.ܭ);
			}
			else
			{
				bounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CellsBounds, Ⴊ._27A4.Node, Ⴊ.ܭ);
			}
			bounds.Inflate(2, 2);
			if (bounds.Right > Ⴊ.ᶮ.Right)
			{
				bounds.Width -= bounds.Right - Ⴊ.ᶮ.Right;
			}
		}
		else if (Ⴊ._27AA)
		{
			Cell ڂ3 = ((Ⴊ._27A4.Node.SelectedCell != null) ? Ⴊ._27A4.Node.SelectedCell : Ⴊ._27A4.Node.Cells[0]);
			bounds = NodeDisplay._070A(_1757._175B, ڂ3, Ⴊ.ܭ);
		}
		else
		{
			bounds = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, Ⴊ._27A4.Node, Ⴊ.ܭ);
		}
		Ⴊ._27A4.Bounds = bounds;
		Ⴊ.Ԧ.DrawSelection(Ⴊ._27A4);
	}

	private void _2740(_2798 Ⴊ)
	{
		Node mouseOverNode = Tree.MouseOverNode;
		if (mouseOverNode != null && !mouseOverNode.IsSelected && mouseOverNode.Selectable)
		{
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CellsBounds, mouseOverNode, Ⴊ.ܭ);
			nodeRectangle.Inflate(2, 2);
			Ⴊ._27A4.Node = mouseOverNode;
			Ⴊ._27A4.Bounds = nodeRectangle;
			Ⴊ.Ԧ.DrawHotTracking(Ⴊ._27A4);
		}
	}

	private Node _2741(NodeCollection _0652, _2798 Ⴊ)
	{
		Node result = null;
		int count = _0652.Count;
		int num = 0;
		if (Ⴊ.ݎ == eView.Tree && count > 99 && Ⴊ.ܭ.Y != 0)
		{
			int num2 = count / 2;
			int num3 = num2;
			while (num2 > 0 && num2 < count)
			{
				num3 /= 2;
				Node node = _0652[num2];
				if (!node.Visible)
				{
					int num4 = num2 + 1;
					while (num4 < count)
					{
						node = _0652[num4];
						num4++;
						if (node.Visible)
						{
							break;
						}
					}
					if (!node.Visible)
					{
						num4 = num2 - 1;
						while (num4 >= 0)
						{
							node = _0652[num4];
							num4--;
							if (node.Visible)
							{
								break;
							}
						}
					}
				}
				if (!node.Visible)
				{
					num = 0;
					break;
				}
				Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, Ⴊ.ܭ);
				if (num3 > 0 && nodeRectangle.Y > Ⴊ._2799.Bottom)
				{
					num2 -= num3;
					continue;
				}
				if (num3 > 0 && nodeRectangle.Y < Ⴊ._2799.Y)
				{
					num2 += num3;
					continue;
				}
				if (num2 == 0)
				{
					break;
				}
				num2--;
				for (int num5 = num2; num5 >= 0; num5--)
				{
					node = _0652[num5];
					if (node.Visible && NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, Ⴊ.ܭ).Y < Ⴊ._2799.Y)
					{
						num = num5;
						break;
					}
				}
				break;
			}
		}
		bool isEmpty = Ⴊ._2799.IsEmpty;
		for (int i = num; i < count; i++)
		{
			Node node2 = _0652[i];
			if (_2742(node2, Ⴊ))
			{
				result = node2;
				Rectangle nodeRectangle2 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node2, Ⴊ.ܭ);
				if (!isEmpty && nodeRectangle2.Y > Ⴊ._2799.Bottom && !Ⴊ.ᶮ.IsEmpty)
				{
					break;
				}
			}
		}
		return result;
	}

	private bool _2742(Node _0653, _2798 Ⴊ)
	{
		if (!_0653.Visible)
		{
			_0653.SelectedConnectorMarker = false;
			return false;
		}
		Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, _0653, Ⴊ.ܭ);
		if (nodeRectangle.IsEmpty)
		{
			_0653.SelectedConnectorMarker = false;
			return false;
		}
		if (Ⴊ._27A8 && !_0653.IsDragNode)
		{
			DisplayHelp.DrawLine(Ⴊ.ܓ, Ⴊ.ᶮ.X, nodeRectangle.Bottom + 1, Ⴊ.ᶮ.Right, nodeRectangle.Bottom + 1, Ⴊ._27A9, 1);
		}
		bool flag = _0653.Expanded && _0653.Nodes.Count > 0;
		if (!_0653.IsDragNode && (_0653.Parent != null || _0653.ExpandVisibility != eNodeExpandVisibility.Hidden))
		{
			_2745(null, _0653, Ⴊ, _2748: false);
		}
		if (flag)
		{
			Node node = _175C._177D(_0653);
			if (node != null)
			{
				_2745(_0653, node, Ⴊ, _2748: false);
			}
		}
		_0653.SelectedConnectorMarker = false;
		if (NodeDisplay._0709(_0653))
		{
			nodeRectangle.Height += _0653.ColumnHeaderHeight;
			nodeRectangle.Width = Math.Max(nodeRectangle.Width, _0653.NodesColumns.Bounds.Width);
		}
		if ((nodeRectangle.Y >= Ⴊ._2799.Y && nodeRectangle.Y <= Ⴊ._2799.Bottom) || (nodeRectangle.Bottom >= Ⴊ._2799.Y && nodeRectangle.Bottom <= Ⴊ._2799.Bottom) || (nodeRectangle.Y <= Ⴊ._2799.Y && nodeRectangle.Bottom > Ⴊ._2799.Y) || Ⴊ._2799.IsEmpty)
		{
			_2744(_0653, Ⴊ);
		}
		if (flag)
		{
			_2741(_0653.Nodes, Ⴊ);
		}
		return true;
	}

	private ElementStyle _2743(Node _0653, _2798 Ⴊ)
	{
		ElementStyle elementStyle = Ⴊ._2717.Copy();
		bool flag = true;
		bool flag2 = _0653.IsSelected && _0653.IsSelectionVisible;
		if (flag2 && Ⴊ._279B == null && _0653.StyleSelected == null && !_0653.FullRowBackground)
		{
			flag = false;
		}
		if (flag2 && (Ⴊ._279B == null || Ⴊ._279B.TextColor.IsEmpty) && (_0653.StyleSelected == null || _0653.StyleSelected.TextColor.IsEmpty))
		{
			eSelectionStyle selectionBoxStyle = Tree.SelectionBoxStyle;
			TreeSelectionColors selection = Ⴊ.Ԧ.ColorTable.Selection;
			if (Ⴊ._27A4.TreeActive)
			{
				if (selectionBoxStyle == eSelectionStyle.FullRowSelect && !selection.FullRowSelect.TextColor.IsEmpty)
				{
					elementStyle.TextColor = selection.FullRowSelect.TextColor;
				}
				else if (selectionBoxStyle == eSelectionStyle.HighlightCells && !selection.HighlightCells.TextColor.IsEmpty)
				{
					elementStyle.TextColor = selection.HighlightCells.TextColor;
				}
				else if (selectionBoxStyle == eSelectionStyle.NodeMarker && !selection.NodeMarker.TextColor.IsEmpty)
				{
					elementStyle.TextColor = selection.NodeMarker.TextColor;
				}
			}
			else if (selectionBoxStyle == eSelectionStyle.FullRowSelect && !selection.FullRowSelectInactive.TextColor.IsEmpty)
			{
				elementStyle.TextColor = selection.FullRowSelectInactive.TextColor;
			}
			else if (selectionBoxStyle == eSelectionStyle.HighlightCells && !selection.HighlightCellsInactive.TextColor.IsEmpty)
			{
				elementStyle.TextColor = selection.HighlightCellsInactive.TextColor;
			}
			else if (selectionBoxStyle == eSelectionStyle.NodeMarker && !selection.NodeMarkerInactive.TextColor.IsEmpty)
			{
				elementStyle.TextColor = selection.NodeMarkerInactive.TextColor;
			}
		}
		else if (Tree.HotTracking && !_0653.IsSelected && _0653.Selectable && Tree.MouseOverNode == _0653)
		{
			SelectionColorTable nodeHotTracking = Ⴊ.Ԧ.ColorTable.Selection.NodeHotTracking;
			if (!nodeHotTracking.TextColor.IsEmpty)
			{
				elementStyle.TextColor = nodeHotTracking.TextColor;
			}
		}
		if (_0653.Style != null)
		{
			if (flag)
			{
				elementStyle.ApplyStyle(_0653.Style);
			}
			else
			{
				elementStyle.ApplyFontStyle(_0653.Style);
			}
		}
		if (flag && _0653.MouseOverNodePart == _27AF._27B0 && elementStyle != null)
		{
			ElementStyle elementStyle2 = Ⴊ._279C;
			if (_0653.StyleMouseOver != null)
			{
				elementStyle2 = _0653.StyleMouseOver;
				flag = false;
			}
			if (elementStyle2 != null)
			{
				elementStyle.ApplyStyle(elementStyle2);
				flag = false;
			}
		}
		if (flag && _0653.Expanded && elementStyle != null)
		{
			ElementStyle elementStyle3 = Ⴊ._279A;
			if (_0653.StyleExpanded != null)
			{
				elementStyle3 = _0653.StyleExpanded;
			}
			if (elementStyle3 != null)
			{
				elementStyle.ApplyStyle(elementStyle3);
			}
		}
		if (flag && _0653.IsSelected && elementStyle != null)
		{
			ElementStyle elementStyle4 = Ⴊ._279B;
			if (_0653.StyleSelected != null)
			{
				elementStyle4 = _0653.StyleSelected;
			}
			if (elementStyle4 != null)
			{
				elementStyle.ApplyStyle(elementStyle4);
			}
		}
		if (elementStyle != null && elementStyle.Font == null)
		{
			elementStyle.Font = Ⴊ._2711;
		}
		return elementStyle;
	}

	private void _2744(Node _0653, _2798 Ⴊ)
	{
		__0708.Add(_0653);
		Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, _0653, Ⴊ.ܭ);
		TreeRenderer treeRenderer = Ⴊ.Ԧ;
		if (_0653.NodeRenderer != null && _0653.RenderMode == eNodeRenderMode.Custom)
		{
			treeRenderer = _0653.NodeRenderer;
		}
		bool flag = _0653.IsSelectionVisible && _0653.IsSelected;
		ElementStyle elementStyle = _2743(_0653, Ⴊ);
		Region val = null;
		if (elementStyle != null)
		{
			Ⴊ._27A6.Graphics = Ⴊ.ܓ;
			Ⴊ._27A6.Node = _0653;
			Ⴊ._27A6.NodeBounds = nodeRectangle;
			Ⴊ._27A6.Style = elementStyle;
			if (!_0653.FullRowBackground)
			{
				treeRenderer.DrawNodeBackground(Ⴊ._27A6);
			}
			ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(elementStyle, Ⴊ.ܓ, Ⴊ._27A6.NodeBounds);
			elementStyleDisplayInfo.Bounds.Inflate(1, 1);
			val = ElementStyleDisplay.GetStyleRegion(elementStyleDisplayInfo);
			elementStyleDisplayInfo.Bounds = nodeRectangle;
		}
		if (NodeDisplay._070C(_0653) && Ⴊ._27A3 != null)
		{
			nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds, _0653, Ⴊ.ܭ);
			Ⴊ._27A3.Node = _0653;
			Ⴊ._27A3.ExpandPartBounds = nodeRectangle;
			Ⴊ._27A3.IsMouseOver = _0653.MouseOverNodePart == _27AF._27B1;
			treeRenderer.DrawNodeExpandPart(Ⴊ._27A3);
		}
		if (NodeDisplay._0709(_0653))
		{
			_070D(_0653.NodesColumns, Ⴊ.ܓ, _070E: false);
		}
		Region val2 = null;
		if (val != null)
		{
			val2 = Ⴊ.ܓ.Clip;
			Ⴊ.ܓ.SetClip(val, (CombineMode)1);
		}
		ElementStyle elementStyle2 = null;
		if (Ⴊ._279D == null)
		{
			elementStyle2 = new ElementStyle();
			elementStyle2.TextColor = elementStyle.TextColor;
			elementStyle2.TextShadowColor = elementStyle.TextShadowColor;
			elementStyle2.TextShadowOffset = elementStyle.TextShadowOffset;
			elementStyle2.TextAlignment = elementStyle.TextAlignment;
			elementStyle2.TextLineAlignment = elementStyle.TextLineAlignment;
			elementStyle2.TextTrimming = elementStyle.TextTrimming;
			elementStyle2.WordWrap = elementStyle.WordWrap;
			elementStyle2.Font = elementStyle.Font;
			elementStyle2.UseMnemonic = elementStyle.UseMnemonic;
		}
		foreach (Cell cell in _0653.Cells)
		{
			if (!cell.IsVisible)
			{
				continue;
			}
			if (cell.StyleNormal == null)
			{
				elementStyle = ((Ⴊ._279D != null) ? Ⴊ._279D.Copy() : ((cell.Enabled && (!cell.IsMouseDown || (cell.StyleMouseDown == null && Ⴊ._279E == null)) && (!cell.IsSelected || (cell.StyleSelected == null && Ⴊ._27A0 == null)) && (!cell.IsMouseOver || (cell.StyleMouseOver == null && Ⴊ._279F == null))) ? elementStyle2 : elementStyle2.Copy()));
			}
			else
			{
				elementStyle = ((Ⴊ._279D == null) ? elementStyle2.Copy() : Ⴊ._279D.Copy());
				elementStyle.ApplyStyle(cell.StyleNormal);
			}
			if (!cell.Enabled && cell.StyleDisabled != null)
			{
				elementStyle.ApplyStyle(cell.StyleDisabled);
			}
			else if (!cell.Enabled && Ⴊ._27A1 != null)
			{
				elementStyle.ApplyStyle(Ⴊ._27A1);
			}
			else if (cell.IsMouseDown && cell.StyleMouseDown != null)
			{
				elementStyle.ApplyStyle(cell.StyleMouseDown);
			}
			else if (cell.IsMouseDown && Ⴊ._279E != null)
			{
				elementStyle.ApplyStyle(Ⴊ._279E);
			}
			else
			{
				if (cell.IsSelected && cell.StyleSelected != null)
				{
					elementStyle.ApplyStyle(cell.StyleSelected);
				}
				else if (cell.IsSelected && Ⴊ._27A0 != null && Ⴊ._27A0.Custom)
				{
					elementStyle.ApplyStyle(Ⴊ._27A0);
				}
				else if (flag && !cell.IsSelected && Tree.SelectionPerCell)
				{
					elementStyle.TextColor = Ⴊ._2717.TextColor;
				}
				if (cell.IsMouseOver && cell.StyleMouseOver != null)
				{
					elementStyle.ApplyStyle(cell.StyleMouseOver);
				}
				else if (cell.IsMouseOver && Ⴊ._279F != null)
				{
					elementStyle.ApplyStyle(Ⴊ._279F);
				}
			}
			nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, _0653, Ⴊ.ܭ);
			if (elementStyle != null)
			{
				if (elementStyle.Font == null)
				{
					elementStyle.Font = Ⴊ._2711;
				}
				if (flag)
				{
					elementStyle.BackColor = Color.Empty;
					elementStyle.BackColorSchemePart = eColorSchemePart.None;
					elementStyle.BackColor2 = Color.Empty;
					elementStyle.BackColor2SchemePart = eColorSchemePart.None;
				}
				Rectangle boundsRelative = cell.BoundsRelative;
				Rectangle textContentBounds = cell.TextContentBounds;
				boundsRelative.Offset(nodeRectangle.Location);
				textContentBounds.Offset(nodeRectangle.Location);
				ElementStyleDisplayInfo e = _274A(elementStyle, Ⴊ.ܓ, boundsRelative);
				ElementStyleDisplay.Paint(e);
				NodeCellRendererEventArgs nodeCellRendererEventArgs = _274B(elementStyle, Ⴊ.ܓ, cell, nodeRectangle.Location, Ⴊ.ԫ);
				if (nodeCellRendererEventArgs.Cell.CheckBoxVisible)
				{
					treeRenderer.DrawCellCheckBox(nodeCellRendererEventArgs);
				}
				if (!nodeCellRendererEventArgs.Cell.Images._26EE.IsEmpty)
				{
					treeRenderer.DrawCellImage(nodeCellRendererEventArgs);
				}
				treeRenderer.DrawCellText(nodeCellRendererEventArgs);
				if (Ⴊ.ݎ == eView.Tile && _0653.HasChildNodes && !Ⴊ._27AE.IsEmpty)
				{
					treeRenderer.DrawTileGroupLine(new NodeRendererEventArgs(Ⴊ.ܓ, _0653, nodeRectangle, elementStyle, Ⴊ._27AE));
				}
			}
		}
		if (val != null)
		{
			Ⴊ.ܓ.SetClip(val2, (CombineMode)0);
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	private void _2745(Node _2746, Node _2747, _2798 Ⴊ, bool _2748)
	{
		_2745(_2746, _2747, Ⴊ, _2748, null);
	}

	private void _2745(Node _2746, Node _2747, _2798 Ⴊ, bool _2748, ConnectorPointsCollection _2749)
	{
		bool flag = ((_2746 != null) ? IsRootNode(_2746) : IsRootNode(_2747));
		if (Ⴊ.ݎ != eView.Tile && (!flag || _2746 != null || _2747.Nodes.Count != 0 || _2747.ExpandVisibility == eNodeExpandVisibility.Visible) && Ⴊ._27A2 != null)
		{
			Ⴊ._27A2.FromNode = _2746;
			Ⴊ._27A2.ToNode = _2747;
			Ⴊ._27A2.IsRootNode = flag;
			Ⴊ._27A2.LinkConnector = _2748;
			if (_2746 != null && _2746.Style != null)
			{
				Ⴊ._27A2.StyleFromNode = _2746.Style;
			}
			else
			{
				Ⴊ._27A2.StyleFromNode = Ⴊ._2717;
			}
			if (_2747.Style != null)
			{
				Ⴊ._27A2.StyleToNode = _2747.Style;
			}
			else
			{
				Ⴊ._27A2.StyleToNode = Ⴊ._2717;
			}
			Ⴊ._27A2.Offset = Ⴊ.ܭ;
			if (_2747.SelectedConnectorMarker)
			{
				Ⴊ._27A2.NodeConnector = Tree.SelectedPathConnector;
			}
			else if (_2747.ParentConnector != null)
			{
				Ⴊ._27A2.NodeConnector = _2747.ParentConnector;
			}
			else
			{
				Ⴊ._27A2.NodeConnector = Tree.NodesConnector;
			}
			if (_2748)
			{
				Ⴊ._27A2.ConnectorPoints = _2749;
			}
			else if (_2749 != null)
			{
				Ⴊ._27A2.ConnectorPoints = _2749;
			}
			else
			{
				Ⴊ._27A2.ConnectorPoints = null;
			}
			Ⴊ.Ԧ.DrawConnector(Ⴊ._27A2);
		}
	}

	private void _2745(Node _2747, _2798 Ⴊ)
	{
		_2745(_2747.Parent, _2747, Ⴊ, _2748: false);
	}

	private ElementStyleDisplayInfo _274A(ElementStyle _0657, Graphics _٥, Rectangle ശ)
	{
		_2732.Style = _0657;
		_2732.Graphics = _٥;
		_2732.Bounds = ശ;
		return _2732;
	}

	private NodeCellRendererEventArgs _274B(ElementStyle _0657, Graphics _٥, Cell ڂ, Point _274C, ColorScheme ጎ)
	{
		_2733.Cell = ڂ;
		_2733.Graphics = _٥;
		_2733.Style = _0657;
		_2733._2750 = _274C;
		_2733.ԫ = ጎ;
		return _2733;
	}

	internal override void _070D(ColumnHeaderCollection _064B, Graphics _٥, bool _070E)
	{
		TreeRenderer ԧ = _2734;
		if (Tree.Ԧ != null && Tree.ԧ == eNodeRenderMode.Custom)
		{
			ԧ = Tree.Ԧ;
		}
		_070D(ԧ, _064B, _٥, _070E);
	}

	internal void _070D(TreeRenderer ઝ, ColumnHeaderCollection _064B, Graphics _٥, bool _070E)
	{
		ColumnHeaderRendererEventArgs columnHeaderRendererEventArgs = new ColumnHeaderRendererEventArgs();
		columnHeaderRendererEventArgs.Graphics = _٥;
		columnHeaderRendererEventArgs.Tree = Tree;
		columnHeaderRendererEventArgs.SortIndicatorColor = ઝ.ColorTable.ColumnSortIndicatorColor;
		ElementStyle elementStyle = _274F(ઝ);
		ElementStyle elementStyle2 = null;
		elementStyle2 = ((!_070E) ? ((Tree.NodesColumnsBackgroundStyle == null) ? _274E(ઝ) : Tree.NodesColumnsBackgroundStyle) : ((Tree.ColumnsBackgroundStyle == null) ? _274D(ઝ) : Tree.ColumnsBackgroundStyle));
		if (Tree.ColumnStyleNormal != null && Tree.ColumnStyleNormal.Custom)
		{
			elementStyle = Tree.ColumnStyleNormal;
		}
		Point pos = Point.Empty;
		if (Tree.AutoScroll)
		{
			pos = Tree._٧();
			if (_070E)
			{
				pos.Y = 0;
			}
		}
		Rectangle bounds = _064B.Bounds;
		if (!_070E)
		{
			bounds.Offset(pos);
		}
		ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(elementStyle2, _٥, bounds);
		ElementStyleDisplay.Paint(e);
		Color color = ((elementStyle2 != null && !elementStyle2.BorderColor.IsEmpty) ? elementStyle2.BorderColor : Color.Empty);
		for (int i = 0; i < _064B.Count; i++)
		{
			ColumnHeader columnHeader = _064B.ColumnAtDisplayIndex(i);
			if (!columnHeader.Visible)
			{
				continue;
			}
			ElementStyle elementStyle3 = null;
			elementStyle3 = ((!(columnHeader.StyleNormal != "")) ? elementStyle.Copy() : Tree.Styles[columnHeader.StyleNormal].Copy());
			if (columnHeader.IsMouseDown)
			{
				if (columnHeader.StyleMouseDown != "")
				{
					elementStyle3.ApplyStyle(Tree.Styles[columnHeader.StyleMouseDown]);
				}
				else if (Tree.ColumnStyleMouseDown != null)
				{
					elementStyle3.ApplyStyle(Tree.ColumnStyleMouseDown);
				}
			}
			else if (columnHeader.IsMouseOver)
			{
				if (columnHeader.StyleMouseOver != "")
				{
					elementStyle3.ApplyStyle(Tree.Styles[columnHeader.StyleMouseOver]);
				}
				else if (Tree.ColumnStyleMouseOver != null)
				{
					elementStyle3.ApplyStyle(Tree.ColumnStyleMouseOver);
				}
			}
			columnHeaderRendererEventArgs.ColumnHeader = columnHeader;
			Rectangle bounds2 = columnHeader.Bounds;
			bounds2.Offset(pos);
			columnHeaderRendererEventArgs.Bounds = bounds2;
			columnHeaderRendererEventArgs.Style = elementStyle3;
			ઝ.DrawColumnHeader(columnHeaderRendererEventArgs);
			if (!color.IsEmpty)
			{
				DisplayHelp.DrawLine(_٥, bounds2.Right - ((!columnHeader._171B) ? 1 : 0), bounds2.Y, bounds2.Right - ((!columnHeader._171B) ? 1 : 0), bounds2.Bottom - 1, color, 1);
			}
		}
	}

	private ElementStyle _274D(TreeRenderer ઝ)
	{
		if (ઝ != null && ઝ._2760 != null)
		{
			return ઝ._2760.StyleClasses[ElementStyleClassKeys.TreeColumnsHeaderKey] as ElementStyle;
		}
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.BackColor = ColorScheme.GetColor(16383229);
		elementStyle.BackColor2 = ColorScheme.GetColor(13884393);
		elementStyle.BackColorGradientAngle = 90;
		elementStyle.TextColor = ColorScheme.GetColor(0);
		elementStyle.BorderColor = ColorScheme.GetColor(10401486);
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		return elementStyle;
	}

	private ElementStyle _274E(TreeRenderer ઝ)
	{
		if (ઝ != null && ઝ._2760 != null)
		{
			return ઝ._2760.StyleClasses[ElementStyleClassKeys.TreeNodesColumnsHeaderKey] as ElementStyle;
		}
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.BackColor = ColorScheme.GetColor(16383229);
		elementStyle.BackColor2 = ColorScheme.GetColor(13884393);
		elementStyle.BackColorGradientAngle = 90;
		elementStyle.TextColor = ColorScheme.GetColor(0);
		elementStyle.BorderColor = ColorScheme.GetColor(10401486);
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.BorderLeft = eStyleBorderType.Solid;
		elementStyle.BorderLeftWidth = 1;
		elementStyle.BorderTop = eStyleBorderType.Solid;
		elementStyle.BorderTopWidth = 1;
		return elementStyle;
	}

	private ElementStyle _274F(TreeRenderer ઝ)
	{
		if (ઝ != null && ઝ._2760 != null)
		{
			return ઝ._2760.StyleClasses[ElementStyleClassKeys.TreeColumnKey] as ElementStyle;
		}
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.TextColor = ColorScheme.GetColor(0);
		return elementStyle;
	}
}
