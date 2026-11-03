using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.AdvTree;

internal class _0744
{
	public Cell _0746;

	public int _0747;

	public int _0748;

	public Graphics ܓ;

	public int _0749;

	public int _074A;

	public ElementStyle _074B;

	public bool _074C = true;

	public bool ݍ;

	public eView ݎ;

	public int ݏ;

	private Font _ݐ;

	public int ݑ;

	public Font _0745
	{
		get
		{
			return _ݐ;
		}
		set
		{
			if (_ݐ != value)
			{
				_ݐ = value;
				if (_ݐ != null)
				{
					ݑ = _ݐ.Height;
				}
				else
				{
					ݑ = 0;
				}
			}
		}
	}
}
internal enum _1757
{
	_1758,
	_1759,
	_175A,
	_175B
}
internal class _175C
{
	private static Color त = Color.Empty;

	internal static int _175D = 0;

	public static string _175E(Node _0653, string _175F)
	{
		if (_0653 == null)
		{
			throw new ArgumentNullException("node");
		}
		StringBuilder stringBuilder = new StringBuilder(_0653.Text);
		for (_0653 = _0653.Parent; _0653 != null; _0653 = _0653.Parent)
		{
			stringBuilder.Insert(0, _0653.Text + _175F);
		}
		return stringBuilder.ToString();
	}

	public static Node ᝠ(Node _0653)
	{
		if (_0653.Nodes.Count > 0)
		{
			return _0653.Nodes[_0653.Nodes.Count - 1];
		}
		return null;
	}

	public static Node ᝡ(AdvTree ᝢ)
	{
		Rectangle ܒ = ᝢ.Ԫ.ܒ;
		Node node = ᝢ.SelectedNode;
		if (node == null)
		{
			node = ᝤ(ᝢ);
		}
		Point offset = Point.Empty;
		if (ᝢ.AutoScroll)
		{
			offset = ᝢ._٧();
		}
		Node result = null;
		if (ܒ.IntersectsWith(node.Bounds))
		{
			result = node;
		}
		while (node != null)
		{
			node = ᝩ(node);
			if (node != null && node.Selectable)
			{
				Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, offset);
				if (nodeRectangle.Bottom > ܒ.Bottom)
				{
					break;
				}
				if (ܒ.IntersectsWith(nodeRectangle))
				{
					result = node;
				}
			}
		}
		return result;
	}

	public static Node ᝣ(AdvTree ᝢ)
	{
		Rectangle rectangle = ᝢ._٦();
		rectangle.Y += ᝢ.Ձ;
		rectangle.Height -= ᝢ.Ձ;
		Node node = ᝢ.SelectedNode;
		if (node == null)
		{
			node = ᝤ(ᝢ);
		}
		Point offset = Point.Empty;
		if (ᝢ.AutoScroll)
		{
			offset = ᝢ._٧();
		}
		Node result = null;
		if (rectangle.IntersectsWith(node.Bounds))
		{
			result = node;
		}
		while (node != null)
		{
			node = ᝯ(node);
			if (node != null && node.Selectable)
			{
				Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, offset);
				if (rectangle.IntersectsWith(nodeRectangle))
				{
					result = node;
				}
				else if (nodeRectangle.Y < rectangle.Y)
				{
					break;
				}
			}
		}
		return result;
	}

	public static Node ᝤ(AdvTree ᝢ)
	{
		if (ᝢ.Nodes.Count == 0)
		{
			return null;
		}
		Node node = ((ᝢ.DisplayRootNode == null) ? ᝢ.Nodes[0] : ᝢ.DisplayRootNode);
		if (node.Visible)
		{
			return node;
		}
		return ᝩ(node);
	}

	public static Node ᝥ(AdvTree ᝢ)
	{
		if (ᝢ.Nodes.Count == 0)
		{
			return null;
		}
		Node node = ((ᝢ.DisplayRootNode == null) ? ᝢ.Nodes[ᝢ.Nodes.Count - 1] : ᝢ.DisplayRootNode);
		if (node.Visible)
		{
			if (node.Nodes.Count > 0 && node.Expanded)
			{
				Node node2 = _177D(node);
				if (node2 != null)
				{
					while (node2.Nodes.Count > 0 && node2.Expanded)
					{
						Node node3 = _177D(node2);
						if (node3 == null)
						{
							return node2;
						}
						node2 = node3;
					}
					return node2;
				}
			}
			return node;
		}
		return ᝯ(node);
	}

	public static Node ᝦ(AdvTree ᝢ)
	{
		if (ᝢ.Nodes.Count == 0)
		{
			return null;
		}
		NodeCollection nodes = ᝢ.Nodes;
		if (ᝢ.DisplayRootNode != null)
		{
			nodes = ᝢ.DisplayRootNode.Nodes;
		}
		for (int num = nodes.Count - 1; num >= 0; num--)
		{
			if (nodes[num].Visible)
			{
				return nodes[num];
			}
		}
		return null;
	}

	public static Node ᝧ(Node _0653)
	{
		if (_0653 == null)
		{
			return null;
		}
		NodeCollection nodeCollection = null;
		if (_0653.Parent != null)
		{
			nodeCollection = _0653.Parent.Nodes;
		}
		else if (_0653.ӵ != null)
		{
			nodeCollection = _0653.ӵ.Nodes;
		}
		if (nodeCollection != null)
		{
			int num = nodeCollection.IndexOf(_0653);
			if (num < nodeCollection.Count - 1)
			{
				return nodeCollection[num + 1];
			}
		}
		return null;
	}

	public static Node ᝨ(Node _0653)
	{
		if (_0653 == null)
		{
			return null;
		}
		Node node = ᝧ(_0653);
		while (node != null && !node.Visible)
		{
			node = ᝧ(node);
		}
		return node;
	}

	public static Node ᝩ(Node _0653)
	{
		Node node = null;
		Node parent = _0653.Parent;
		Node node2 = null;
		while (parent != null)
		{
			if (!parent.Expanded)
			{
				node2 = parent;
			}
			parent = parent.Parent;
		}
		if (node2 != null)
		{
			if (node2.Parent != null)
			{
				node2 = node2.Parent;
			}
			while (node2 != null)
			{
				node2 = ᝧ(node2);
				if (node2 != null && node2.Visible)
				{
					return node2;
				}
			}
		}
		if (_0653.Expanded && _0653.Nodes.Count > 0)
		{
			foreach (Node node3 in _0653.Nodes)
			{
				if (node3.Visible)
				{
					return node3;
				}
			}
		}
		for (node = ᝪ(_0653); node != null; node = ᝪ(node))
		{
			if (node.Visible && node.IsVisible)
			{
				return node;
			}
		}
		return node;
	}

	public static Node ᝪ(Node _0653)
	{
		Node node = null;
		if (_0653.Nodes.Count > 0)
		{
			return _0653.Nodes[0];
		}
		node = ᝧ(_0653);
		if (node == null)
		{
			while (node == null && _0653 != null)
			{
				_0653 = _0653.Parent;
				node = ᝧ(_0653);
			}
		}
		return node;
	}

	public static bool ᝫ(Node _0653)
	{
		if (!_0653.Visible)
		{
			return false;
		}
		Node node = _0653;
		while (node.Parent != null)
		{
			node = node.Parent;
			if (!node.Expanded || !node.Visible)
			{
				return false;
			}
		}
		return true;
	}

	public static bool ᝬ(Node _0653)
	{
		if (!_0653.Visible || !ᝫ(_0653))
		{
			return false;
		}
		AdvTree treeControl = _0653.TreeControl;
		if (treeControl != null)
		{
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, _0653, treeControl.NodeDisplay.Offset);
			return ((Control)treeControl).ClientRectangle.IntersectsWith(nodeRectangle);
		}
		return false;
	}

	public static int _176D(Node _0653)
	{
		if (_0653.Parent == null)
		{
			return _0653.TreeControl?.Nodes.IndexOf(_0653) ?? (-1);
		}
		return _0653.Parent.Nodes.IndexOf(_0653);
	}

	public static Node ᝮ(Node _0653)
	{
		if (_0653 == null)
		{
			return null;
		}
		NodeCollection nodeCollection = null;
		if (_0653.Parent != null)
		{
			nodeCollection = _0653.Parent.Nodes;
		}
		else if (_0653.ӵ != null)
		{
			nodeCollection = _0653.ӵ.Nodes;
		}
		if (nodeCollection != null)
		{
			int num = nodeCollection.IndexOf(_0653);
			if (num > 0)
			{
				return nodeCollection[num - 1];
			}
		}
		return null;
	}

	public static Node ᝯ(Node _0653)
	{
		Node node = null;
		Node parent = _0653.Parent;
		Node node2 = null;
		while (parent != null)
		{
			if (!parent.Expanded)
			{
				node2 = parent;
			}
			parent = parent.Parent;
		}
		if (node2 != null)
		{
			if (node2.IsVisible)
			{
				return node2;
			}
			parent = node2;
		}
		else
		{
			parent = _0653;
		}
		if (parent.Parent != null)
		{
			if (parent.Parent.Nodes.IndexOf(parent) > 0)
			{
				node = ᝰ(parent);
				while (node != null && !node.IsVisible)
				{
					node = ᝰ(node);
				}
				if (node != null)
				{
					return node;
				}
			}
			else
			{
				while (parent.Parent != null)
				{
					if (parent.Parent.IsVisible)
					{
						return parent.Parent;
					}
					parent = parent.Parent;
				}
				if (parent.TreeControl != null)
				{
					node = ᝰ(parent);
				}
			}
		}
		else
		{
			if (parent.TreeControl == null)
			{
				return null;
			}
			node = ᝮ(parent);
			if (node != null && (!node.Visible || !node.Selectable))
			{
				while (node != null && (!node.Visible || !node.Selectable))
				{
					node = ᝮ(node);
				}
			}
		}
		if (node == null)
		{
			return null;
		}
		while (node.Expanded && node.Nodes.Count > 0)
		{
			node = node.Nodes[node.Nodes.Count - 1];
		}
		return node;
	}

	public static Node ᝰ(Node _0653)
	{
		Node node = null;
		if (_0653.Parent != null)
		{
			if (_0653.Parent.Nodes.IndexOf(_0653) <= 0)
			{
				return _0653.Parent;
			}
			node = ᝮ(_0653);
		}
		else
		{
			if (_0653.TreeControl == null)
			{
				return null;
			}
			node = ᝮ(_0653);
		}
		if (node == null)
		{
			return null;
		}
		while (node.Nodes.Count > 0)
		{
			node = node.Nodes[node.Nodes.Count - 1];
		}
		return node;
	}

	public static bool ܢ(AdvTree ᝢ, Node _0653)
	{
		if (_0653.Parent == null || _0653 == ᝢ.DisplayRootNode)
		{
			return true;
		}
		return false;
	}

	public static void _1771(Cell ڂ)
	{
		Node parent = ڂ.Parent;
		if (parent == null)
		{
			return;
		}
		AdvTree treeControl = parent.TreeControl;
		if (treeControl == null)
		{
			return;
		}
		_1771(parent, _1773: false);
		Rectangle rectangle = NodeDisplay._070A(_1757._175B, ڂ, treeControl.NodeDisplay.Offset);
		if (treeControl == null || !treeControl.AutoScroll)
		{
			return;
		}
		if (treeControl.Zoom != 1f)
		{
			rectangle = treeControl.GetScreenRectangle(rectangle);
		}
		Rectangle rectangle2 = treeControl._٦();
		if (treeControl.VScrollBar != null)
		{
			rectangle2.Width -= ((Control)treeControl.VScrollBar).Width;
		}
		if (treeControl.HScrollBar != null)
		{
			rectangle2.Height -= ((Control)treeControl.HScrollBar).Height;
		}
		if (!rectangle2.Contains(rectangle))
		{
			Point offset = treeControl.NodeDisplay.Offset;
			offset.Y = treeControl.AutoScrollPosition.Y;
			if (rectangle.X < rectangle2.X)
			{
				offset.X -= rectangle.X;
			}
			else if (rectangle.X > rectangle2.Right)
			{
				offset.X += rectangle.Right - rectangle2.Right;
			}
			if (!treeControl.IsLayoutPending)
			{
				treeControl.AutoScrollPosition = offset;
			}
		}
	}

	public static void _1771(Node _1772, bool _1773)
	{
		_1771(_1772, _1773, eEnsureVisibleOption.Middle);
	}

	public static void _1771(Node _1772, bool _1773, eEnsureVisibleOption _1774)
	{
		AdvTree treeControl = _1772.TreeControl;
		if (treeControl == null || treeControl.IsUpdateSuspended)
		{
			return;
		}
		treeControl.SuspendPaint = true;
		try
		{
			Node node = _1772;
			bool flag = false;
			while (node.Parent != null)
			{
				if (!node.Parent.Expanded)
				{
					node.Parent.Expand();
					flag = true;
				}
				node = node.Parent;
			}
			if (flag || treeControl.IsLayoutPending)
			{
				treeControl.ښ();
			}
			Point offset = treeControl.NodeDisplay.Offset;
			if (offset.IsEmpty && treeControl.AutoScroll && !treeControl.AutoScrollPosition.IsEmpty)
			{
				offset = treeControl._٧();
			}
			Rectangle rectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, _1772, offset);
			if (treeControl == null || !treeControl.AutoScroll)
			{
				return;
			}
			if (treeControl.Zoom != 1f)
			{
				rectangle = treeControl.GetScreenRectangle(rectangle);
			}
			Rectangle rectangle2 = treeControl._٦();
			if (treeControl.VScrollBar != null)
			{
				rectangle2.Width -= ((Control)treeControl.VScrollBar).Width;
			}
			if (treeControl.HScrollBar != null)
			{
				rectangle2.Height -= ((Control)treeControl.HScrollBar).Height;
			}
			int ձ = treeControl.Ձ;
			rectangle2.Y += ձ;
			rectangle2.Height -= ձ;
			if (rectangle2.Contains(rectangle))
			{
				return;
			}
			Point autoScrollPosition = treeControl.AutoScrollPosition;
			if (_1773)
			{
				if (rectangle.X < rectangle2.X)
				{
					autoScrollPosition.X -= rectangle.X;
				}
				if (autoScrollPosition.X > 0)
				{
					autoScrollPosition.X = -autoScrollPosition.X;
				}
				if ((autoScrollPosition.X > -10 && autoScrollPosition.X < 0) || (autoScrollPosition.X < 11 && autoScrollPosition.X > 0))
				{
					autoScrollPosition.X = 0;
				}
			}
			if ((double)rectangle.Height > (double)rectangle2.Height * 0.8)
			{
				autoScrollPosition.Y = -Math.Max(0, rectangle.Y - 4);
			}
			else if (rectangle.Bottom > rectangle2.Bottom)
			{
				switch (_1774)
				{
				case eEnsureVisibleOption.Bottom:
					autoScrollPosition.Y -= rectangle.Bottom - rectangle2.Bottom + rectangle.Height;
					break;
				case eEnsureVisibleOption.Middle:
					autoScrollPosition.Y -= rectangle.Bottom - (rectangle2.Bottom - rectangle2.Height / 2) + rectangle.Height / 2;
					break;
				case eEnsureVisibleOption.Top:
					autoScrollPosition.Y -= rectangle.Top - (rectangle2.Bottom - rectangle2.Height);
					break;
				}
			}
			else if (rectangle.Y < rectangle2.Y)
			{
				switch (_1774)
				{
				case eEnsureVisibleOption.Top:
					autoScrollPosition.Y -= rectangle.Y - ձ;
					break;
				case eEnsureVisibleOption.Middle:
					autoScrollPosition.Y -= rectangle.Y - (ձ + rectangle2.Height / 2 + rectangle.Height / 2);
					break;
				case eEnsureVisibleOption.Bottom:
					autoScrollPosition.Y -= rectangle.Y - (ձ - rectangle2.Height + rectangle.Height);
					break;
				}
			}
			if ((autoScrollPosition.Y > -10 && autoScrollPosition.Y < 0) || autoScrollPosition.Y > 0)
			{
				autoScrollPosition.Y = 0;
			}
			if (!treeControl.IsLayoutPending)
			{
				treeControl.AutoScrollPosition = autoScrollPosition;
			}
		}
		finally
		{
			if (treeControl != null)
			{
				treeControl.SuspendPaint = false;
			}
		}
	}

	public static int _1775(Node _0653)
	{
		if (_0653.Nodes.Count == 0)
		{
			return 0;
		}
		int num = 0;
		foreach (Node node in _0653.Nodes)
		{
			if (node.Visible)
			{
				num++;
			}
		}
		return num;
	}

	public static bool _1776(Node _0653)
	{
		if (!_0653.HasChildNodes)
		{
			return false;
		}
		foreach (Node node in _0653.Nodes)
		{
			if (node.Visible)
			{
				return true;
			}
		}
		return false;
	}

	public static _17C6 _1777(AdvTree ᝢ, Point _065C)
	{
		return _1777(ᝢ, _065C.X, _065C.Y);
	}

	public static _17C6 _1777(AdvTree ᝢ, int و, int ى)
	{
		return _1777(ᝢ, و, ى, _1778: false);
	}

	public static _17C6 _1777(AdvTree ᝢ, int و, int ى, bool _1778)
	{
		return _1777(ᝢ, و, ى, _1778, _1779: false);
	}

	public static _17C6 _1777(AdvTree ᝢ, int و, int ى, bool _1778, bool _1779)
	{
		_17C6 obj = new _17C6();
		if (ᝢ.Nodes.Count == 0 || (!((Control)ᝢ).DisplayRectangle.Contains(و, ى) && ᝢ.Zoom == 1f))
		{
			return obj;
		}
		Node node = ᝢ.Nodes[0];
		Node node2 = null;
		bool flag = false;
		if (ᝢ.DisplayRootNode != null)
		{
			node = ᝢ.DisplayRootNode;
			flag = true;
		}
		Point offset = ᝢ.NodeDisplay.Offset;
		if (((Control)ᝢ).DisplayRectangle.Contains(و, ى))
		{
			ArrayList paintedNodes = ᝢ.NodeDisplay.PaintedNodes;
			foreach (Node item in paintedNodes)
			{
				if (item.IsDisplayed)
				{
					if (NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, item, offset).Contains(و, ى))
					{
						obj._17C7 = item;
						return obj;
					}
					if (NodeDisplay._0709(item) && NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, item, offset).Contains(و, ى))
					{
						obj._17C8 = item.NodesColumns;
						return obj;
					}
				}
			}
		}
		if (_1778)
		{
			return obj;
		}
		while (node != null)
		{
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, offset);
			if (nodeRectangle.Contains(و, ى))
			{
				node2 = node;
				break;
			}
			if (NodeDisplay._0709(node) && NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, node, offset).Contains(و, ى))
			{
				obj._17C8 = node.NodesColumns;
				return obj;
			}
			if (nodeRectangle.Y > ى)
			{
				break;
			}
			node = (NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds, node, offset).Contains(و, ى) ? node.NextVisibleNode : ᝨ(node));
			if (flag && !_177E(ᝢ.DisplayRootNode, node))
			{
				break;
			}
		}
		obj._17C7 = node2;
		return obj;
	}

	public static _17C6 _1777(AdvTree ᝢ, int ى)
	{
		return _1777(ᝢ, ى, _1778: false);
	}

	public static _17C6 _1777(AdvTree ᝢ, int ى, bool _1778)
	{
		return _1777(ᝢ, ى, _1778, _1779: false);
	}

	public static _17C6 _1777(AdvTree ᝢ, int ى, bool _1778, bool _1779)
	{
		_17C6 obj = new _17C6();
		if (ᝢ.Nodes.Count == 0 || (!((Control)ᝢ).DisplayRectangle.Contains(1, ى) && ᝢ.Zoom == 1f))
		{
			return obj;
		}
		Point offset = ᝢ.NodeDisplay.Offset;
		if (((Control)ᝢ).DisplayRectangle.Contains(1, ى))
		{
			ArrayList paintedNodes = ᝢ.NodeDisplay.PaintedNodes;
			foreach (Node item in paintedNodes)
			{
				if (!item.IsDisplayed && (!item.Expanded || !item.HasColumns))
				{
					continue;
				}
				Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, item, offset);
				if (ى >= nodeRectangle.Y && ى <= nodeRectangle.Bottom)
				{
					obj._17C7 = item;
					return obj;
				}
				if (NodeDisplay._0709(item))
				{
					Rectangle nodeRectangle2 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, item, offset);
					if (ى >= nodeRectangle2.Y && ى <= nodeRectangle2.Bottom)
					{
						obj._17C8 = item.NodesColumns;
						return obj;
					}
				}
			}
		}
		if (_1778)
		{
			return obj;
		}
		Node node2 = ᝢ.Nodes[0];
		Node node3 = null;
		if (ᝢ.DisplayRootNode != null)
		{
			node2 = ᝢ.DisplayRootNode;
		}
		while (node2 != null)
		{
			Rectangle nodeRectangle3 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node2, offset);
			if (node2.Visible && ى >= nodeRectangle3.Y && ى <= nodeRectangle3.Bottom)
			{
				node3 = node2;
				break;
			}
			if (NodeDisplay._0709(node2))
			{
				Rectangle nodeRectangle4 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ColumnsBounds, node2, offset);
				if (ى >= nodeRectangle4.Y && ى <= nodeRectangle4.Bottom)
				{
					obj._17C8 = node2.NodesColumns;
					return obj;
				}
			}
			if (_1779 && (nodeRectangle3.Bottom + 16 < ى || nodeRectangle3.Y + 16 > ى))
			{
				break;
			}
			nodeRectangle3 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ChildNodeBounds, node2, offset);
			node2 = ((ى >= nodeRectangle3.Y && ى <= nodeRectangle3.Bottom) ? node2.NextVisibleNode : ᝨ(node2));
		}
		obj._17C7 = node3;
		return obj;
	}

	public static _17C9 _177A(AdvTree ᝢ, int و, int ى)
	{
		_17C9 obj = new _17C9();
		if (ᝢ.Nodes.Count == 0 || (!((Control)ᝢ).DisplayRectangle.Contains(و, ى) && ᝢ.Zoom == 1f))
		{
			return null;
		}
		Node node = ᝢ.Nodes[0];
		if (ᝢ.DisplayRootNode != null)
		{
			node = ᝢ.DisplayRootNode;
		}
		if (ᝢ.DisplayRootNode != null)
		{
			node = ᝢ.DisplayRootNode;
		}
		Node node2 = null;
		Rectangle rectangle = Rectangle.Empty;
		if (ᝢ.View == eView.Tile)
		{
			while (node != null)
			{
				if (!node.IsDisplayed)
				{
					node = node.NextVisibleNode;
					continue;
				}
				Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, ᝢ.NodeDisplay.Offset);
				if (nodeRectangle.Contains(و, ى))
				{
					obj._17C7 = node;
					break;
				}
				if (ى >= nodeRectangle.Y && ى <= nodeRectangle.Bottom && node.HasChildNodes)
				{
					obj._17C7 = node;
					break;
				}
				if (node2 != null && ى > rectangle.Bottom && ى < nodeRectangle.Y)
				{
					obj._17C7 = node2;
					break;
				}
				node2 = node;
				rectangle = nodeRectangle;
				node = node.NextVisibleNode;
			}
		}
		else
		{
			while (node != null)
			{
				if (!node.IsDisplayed)
				{
					node = node.NextVisibleNode;
					continue;
				}
				Rectangle nodeRectangle2 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, ᝢ.NodeDisplay.Offset);
				if (ى >= nodeRectangle2.Y && ى <= nodeRectangle2.Bottom)
				{
					obj._17C7 = node;
					break;
				}
				if (node2 != null && ى > rectangle.Bottom && ى < nodeRectangle2.Y)
				{
					obj._17C7 = node2;
					break;
				}
				node2 = node;
				rectangle = nodeRectangle2;
				node = node.NextVisibleNode;
			}
		}
		return obj;
	}

	public static int _177B(Node ԟ)
	{
		int num = 0;
		foreach (Node node in ԟ.Nodes)
		{
			if (node.Visible)
			{
				num++;
			}
		}
		return num;
	}

	public static Node _177C(Node ԟ)
	{
		foreach (Node node in ԟ.Nodes)
		{
			if (node.Visible)
			{
				return node;
			}
		}
		return null;
	}

	public static Node _177D(Node ԟ)
	{
		NodeCollection nodes = ԟ.Nodes;
		int num = nodes.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			Node node = nodes[num2];
			if (node.Visible)
			{
				return node;
			}
		}
		return null;
	}

	public static bool _177E(Node ԟ, Node[] _177F)
	{
		foreach (Node ក in _177F)
		{
			if (_177E(ԟ, ក))
			{
				return true;
			}
		}
		return false;
	}

	public static bool _177E(Node ԟ, Node ក)
	{
		if (ԟ == ក || ក == null || ԟ == null)
		{
			return false;
		}
		while (ក.Parent != null)
		{
			if (ក.Parent == ԟ)
			{
				return true;
			}
			ក = ក.Parent;
		}
		return false;
	}

	public static bool ខ(Node[] គ, Node ក)
	{
		foreach (Node ԟ in គ)
		{
			if (_177E(ԟ, ក))
			{
				return true;
			}
		}
		return false;
	}

	internal static bool फ()
	{
		//Discarded unreachable code: IL_0051, IL_0063, IL_01a9
		Color control = SystemColors.Control;
		Color color = control;
		color.ToArgb();
		control.ToArgb();
		_175D = control.A;
		त = Color.Black;
		return false;
	}

	public static Node ឃ(AdvTree ង, string _0956)
	{
		return ឃ(ង.Nodes, _0956);
	}

	public static Node ឃ(NodeCollection _104C, string _0956)
	{
		foreach (Node item in _104C)
		{
			if (item.Name == _0956)
			{
				return item;
			}
			if (item.HasChildNodes)
			{
				Node node2 = ឃ(item.Nodes, _0956);
				if (node2 != null)
				{
					return node2;
				}
			}
		}
		return null;
	}

	public static void ច(NodeCollection _104C, string _0956, bool ឆ, ArrayList ជ)
	{
		foreach (Node item in _104C)
		{
			if (item.Name == _0956)
			{
				ជ.Add(item);
			}
			if (ឆ && item.HasChildNodes)
			{
				ច(item.Nodes, _0956, ឆ: true, ជ);
			}
		}
	}

	public static Node ឈ(AdvTree ង, object _੬)
	{
		return ឈ(ង.Nodes, _੬);
	}

	public static Node ឈ(NodeCollection _104C, object _੬)
	{
		foreach (Node item in _104C)
		{
			if (object.Equals(item.DataKey, _੬))
			{
				return item;
			}
			if (item.HasChildNodes)
			{
				Node node2 = ឈ(item.Nodes, _੬);
				if (node2 != null)
				{
					return node2;
				}
			}
		}
		return null;
	}

	public static Node ញ(AdvTree ង, int _۰)
	{
		return ញ(ង.Nodes, _۰);
	}

	public static Node ញ(NodeCollection _104C, int _۰)
	{
		foreach (Node item in _104C)
		{
			if (item.BindingIndex == _۰)
			{
				return item;
			}
			if (item.HasChildNodes)
			{
				Node node2 = ញ(item.Nodes, _۰);
				if (node2 != null)
				{
					return node2;
				}
			}
		}
		return null;
	}

	public static Cell ដ(Node _0653, int _104D)
	{
		return ឌ(_0653, _104D, 1);
	}

	public static Cell ឋ(Node _0653, int _104D)
	{
		return ឌ(_0653, _104D, -1);
	}

	public static Cell ឌ(Node _0653, int _104D, int _0C74)
	{
		AdvTree treeControl = _0653.TreeControl;
		if (treeControl != null && treeControl.AllowUserToReorderColumns && treeControl.Columns.Count > 0)
		{
			_104D = treeControl.Columns.GetDisplayIndex(treeControl.Columns[_104D]);
			_104D += _0C74;
			if (_0C74 > 0)
			{
				_ = treeControl.Columns.Count;
			}
			int num = _104D;
			int num2 = 0;
			while (num2 < 2)
			{
				if (num < 0)
				{
					num = treeControl.Columns.Count - 1;
					num2++;
				}
				else if (num >= treeControl.Columns.Count)
				{
					num = 0;
					num2++;
				}
				int num3 = treeControl.Columns.ᜭ[num];
				if (_0653.Cells.Count > num3 && _0653.Cells[num3].IsVisible)
				{
					return _0653.Cells[num3];
				}
				num += _0C74;
			}
		}
		else
		{
			_104D += _0C74;
			if (_0C74 > 0)
			{
				_ = _0653.Cells.Count;
			}
			int num4 = _104D;
			int num5 = 0;
			while (num5 < 2)
			{
				if (num4 < 0)
				{
					num4 = _0653.Cells.Count - 1;
					num5++;
				}
				else if (num4 >= _0653.Cells.Count)
				{
					num4 = 0;
					num5++;
				}
				if (_0653.Cells[num4].IsVisible)
				{
					return _0653.Cells[num4];
				}
				num4 += _0C74;
			}
		}
		return null;
	}

	internal static int ឍ(AdvTree ᝢ, Node _0653)
	{
		if (_0653 == null)
		{
			throw new NullReferenceException("Parameter node cannot be null");
		}
		int num = 0;
		for (Node node = ᝰ(_0653); node != null; node = ᝰ(node))
		{
			num++;
		}
		return num;
	}

	internal static Node ណ(AdvTree ង, int ԡ)
	{
		if (ԡ == -1 || ង.Nodes.Count == 0)
		{
			return null;
		}
		Node node = ង.Nodes[0];
		int num = 0;
		while (node != null)
		{
			if (ԡ == num)
			{
				return node;
			}
			node = ᝪ(node);
			num++;
		}
		return null;
	}

	internal static Node ត(AdvTree ង, string _06EA, Node ថ, bool ទ)
	{
		ថ = ((ថ != null) ? ᝪ(ថ) : ᝤ(ង));
		if (ថ == null)
		{
			return null;
		}
		Node node = ថ;
		StringComparison comparisonType = (ទ ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
		while (node != null)
		{
			if (node.Text.StartsWith(_06EA, comparisonType))
			{
				return node;
			}
			node = ᝪ(node);
		}
		return null;
	}

	internal static Node ធ(AdvTree ង, string _06EA, Node ថ, bool ទ)
	{
		if (string.IsNullOrEmpty(_06EA))
		{
			return null;
		}
		ថ = ((ថ == null) ? ᝤ(ង) : ᝪ(ថ));
		if (ថ == null)
		{
			return null;
		}
		string[] array = _06EA.Split(new char[1] { ',' });
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
		}
		Node node = ថ;
		StringComparison ប = (ទ ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
		while (node != null)
		{
			if (ន(node, array, ប))
			{
				return node;
			}
			node = ᝪ(node);
		}
		return null;
	}

	private static bool ន(Node _0653, string[] ٮ, StringComparison ប)
	{
		for (int i = 0; i < ٮ.Length && i < _0653.Cells.Count; i++)
		{
			Cell cell = _0653.Cells[i];
			if (!cell.Text.StartsWith(ٮ[i], ប))
			{
				return false;
			}
		}
		return true;
	}

	internal static ColumnHeader ផ(AdvTree ᝢ, Cell ڂ)
	{
		int num = -1;
		if (ڂ.Parent != null)
		{
			num = ڂ.Parent.Cells.IndexOf(ڂ);
		}
		if (ڂ.Parent != null && ڂ.Parent.Parent != null && ڂ.Parent.Parent.NodesColumns.Count > 0 && num < ڂ.Parent.Parent.NodesColumns.Count)
		{
			return ڂ.Parent.Parent.NodesColumns[num];
		}
		if (ᝢ != null && ᝢ.Columns.Count > 0 && num < ᝢ.Columns.Count)
		{
			return ᝢ.Columns[num];
		}
		return null;
	}

	public static void ព(Node _0653, bool ភ)
	{
		_0653.SizeChanged = true;
		if (!ភ || !_0653.HasChildNodes)
		{
			return;
		}
		foreach (Node node in _0653.Nodes)
		{
			ព(node, ភ: true);
		}
	}
}
[Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxItem(false)]
internal class _17C1 : TextBox, ICellEditControl
{
	private bool ݥ;

	private EventHandler _17C3;

	private EventHandler _17C4;

	private bool __17C5;

	public bool EditWordWrap
	{
		get
		{
			return ݥ;
		}
		set
		{
			ݥ = value;
			if (ݥ)
			{
				((TextBoxBase)this).Multiline = true;
				((TextBox)this).ScrollBars = (ScrollBars)0;
			}
			else
			{
				((TextBoxBase)this).Multiline = false;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets whether control prevents Beep sound when Enter key is pressed.")]
	public bool _17C2
	{
		get
		{
			return __17C5;
		}
		set
		{
			__17C5 = value;
		}
	}

	public object CurrentValue
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value.ToString();
		}
	}

	public event EventHandler EditComplete
	{
		add
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler CancelEdit
	{
		add
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public _17C1()
	{
		((Control)this).AutoSize = false;
		((TextBoxBase)this).BorderStyle = (BorderStyle)0;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if (((int)e.KeyCode == 13 && !ݥ) || ((int)e.KeyCode == 13 && (int)e.Modifiers == 131072))
		{
			if (_17C3 != null)
			{
				_17C3(this, new EventArgs());
			}
		}
		else if ((int)e.KeyCode == 27 && _17C4 != null)
		{
			_17C4(this, new EventArgs());
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (!ݥ && __17C5 && e.KeyChar == '\r')
		{
			e.Handled = true;
		}
		((Control)this).OnKeyPress(e);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 9 && _17C3 != null)
		{
			_17C3(this, new EventArgs());
		}
		return ((TextBoxBase)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((TextBoxBase)this).OnTextChanged(e);
		ლ();
	}

	private void ლ()
	{
		Graphics val = ((Control)this).CreateGraphics();
		SizeF sizeF = val.MeasureString(((Control)this).Text, ((Control)this).Font);
		int num = (int)Math.Ceiling(sizeF.Width);
		int num2 = (int)Math.Ceiling(sizeF.Height);
		if (((Control)this).Parent == null || ((Control)this).Right + (num - ((Control)this).Width) <= ((Control)this).Parent.ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2)
		{
			if (num > ((Control)this).Width)
			{
				((Control)this).Width = num;
			}
			if (ݥ && (((Control)this).Parent == null || ((Control)this).Bottom + (num2 - ((Control)this).Height) <= ((Control)this).Parent.Bottom) && num2 > ((Control)this).Height)
			{
				((Control)this).Height = num2;
			}
		}
	}

	public void BeginEdit()
	{
	}

	public void EndEdit()
	{
	}
}
internal class _17C6
{
	public Node _17C7;

	public ColumnHeaderCollection _17C8;
}
internal class _17C9
{
	public Node _17CA;

	public Node _17C7;

	public Node _17CB;

	public Node _17CC;
}
internal class _2729
{
	public bool ᆒ;

	public int ܐ;

	public int _272B;

	public ColumnHeader _272C;

	public bool _272D;

	public int _272E = -1;

	public _2729(int width, bool visible, ColumnHeader h, int absoluteIndex)
	{
		ܐ = width;
		ᆒ = visible;
		_272C = h;
		_272D = h.Width.AutoSize;
		_272E = absoluteIndex;
	}
}
internal enum _27AF
{
	ᘶ,
	_27B0,
	_27B1,
	_27B2,
	ᒝ
}
internal class _27B3
{
	public Node ކ;

	public int _27B4 = -1;

	public _27B3()
	{
	}

	public _27B3(Node parent, int insertIndex)
	{
		ކ = parent;
		_27B4 = insertIndex;
	}

	public override string ToString()
	{
		return $"NodeDragInfo-> Parent={ކ}, InsertIndex={_27B4}";
	}
}
internal class _29B8
{
	public static void ᵢ(AdvTree ᝢ, KeyEventArgs ؿ)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected I4, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		Node selectedNode = ᝢ.SelectedNode;
		if (selectedNode != null)
		{
			selectedNode.InternalKeyDown(ؿ);
			if (ؿ.Handled)
			{
				return;
			}
		}
		Keys keyCode = ؿ.KeyCode;
		if ((int)keyCode != 32)
		{
			switch (keyCode - 107)
			{
			default:
				if ((int)keyCode != 113)
				{
					break;
				}
				_29BD(ᝢ, ؿ);
				return;
			case 0:
				ؿ.Handled = _29BA(ᝢ, ؿ);
				return;
			case 2:
				ؿ.Handled = _29B9(ᝢ, ؿ);
				return;
			case 1:
				break;
			}
			if (ؿ.Control && (int)ؿ.KeyCode == 67)
			{
				_29BC(ᝢ, ؿ);
			}
			else if (ؿ.Control && (int)ؿ.KeyCode == 86)
			{
				_29BB(ᝢ, ؿ);
			}
		}
		else
		{
			_29BE(ᝢ, ؿ);
			ؿ.Handled = true;
		}
	}

	private static bool _29B9(AdvTree ᝢ, KeyEventArgs ؿ)
	{
		Node selectedNode = ᝢ.SelectedNode;
		if (selectedNode != null && ᝢ.SelectedNodes.Count == 1 && selectedNode.Expanded)
		{
			selectedNode.Collapse();
			return true;
		}
		return false;
	}

	private static bool _29BA(AdvTree ᝢ, KeyEventArgs ؿ)
	{
		Node selectedNode = ᝢ.SelectedNode;
		if (selectedNode != null && ᝢ.SelectedNodes.Count == 1 && !selectedNode.Expanded)
		{
			selectedNode.Expand();
			return true;
		}
		return false;
	}

	private static void _29BB(AdvTree ᝢ, KeyEventArgs ھ)
	{
		if (ᝢ.SelectedNode != null)
		{
			ᝢ.SelectedNode.InvokeKeyboardPaste(ھ);
		}
	}

	private static void _29BC(AdvTree ᝢ, KeyEventArgs ھ)
	{
		if (ᝢ.SelectedNode != null)
		{
			ᝢ.SelectedNode.InvokeKeyboardCopy(ھ);
		}
	}

	private static void _29BD(AdvTree ᝢ, KeyEventArgs ؿ)
	{
		if (ᝢ.ٿ(eTreeAction.Keyboard))
		{
			ؿ.Handled = true;
		}
	}

	public static void _29BE(AdvTree ᝢ, KeyEventArgs ؿ)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		Node selectedNode = ᝢ.SelectedNode;
		if (selectedNode == null || !selectedNode.CheckBoxVisible || !selectedNode.Enabled)
		{
			return;
		}
		if (selectedNode.CheckBoxThreeState)
		{
			if ((int)selectedNode.CheckState == 1)
			{
				selectedNode.SetChecked((CheckState)2, eTreeAction.Keyboard);
			}
			else if ((int)selectedNode.CheckState == 0)
			{
				selectedNode.SetChecked((CheckState)1, eTreeAction.Keyboard);
			}
			else if ((int)selectedNode.CheckState == 2)
			{
				selectedNode.SetChecked((CheckState)0, eTreeAction.Keyboard);
			}
		}
		else
		{
			selectedNode.SetChecked(!selectedNode.Checked, eTreeAction.Keyboard);
		}
		ؿ.Handled = true;
	}

	public static void _29BF(AdvTree ᝢ, KeyEventArgs ؿ)
	{
		if (ᝢ.SelectedNode != null && ᝢ.SelectedNode.Nodes.Count > 0)
		{
			ᝢ.SelectedNode.Toggle(eTreeAction.Keyboard);
		}
	}

	public static bool _29C0(AdvTree ᝢ, KeyEventArgs ؿ)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Invalid comparison between Unknown and I4
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Invalid comparison between Unknown and I4
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Invalid comparison between Unknown and I4
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Invalid comparison between Unknown and I4
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Invalid comparison between Unknown and I4
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Invalid comparison between Unknown and I4
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Invalid comparison between Unknown and I4
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Invalid comparison between Unknown and I4
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Invalid comparison between Unknown and I4
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Invalid comparison between Unknown and I4
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Invalid comparison between Unknown and I4
		if (ᝢ.SelectedNode == null)
		{
			if (ᝢ.DisplayRootNode != null)
			{
				ᝢ.SelectNode(ᝢ.DisplayRootNode, eTreeAction.Keyboard);
			}
			else if (ᝢ.Nodes.Count > 0)
			{
				Node node = _175C.ᝤ(ᝢ);
				if (node != null)
				{
					while (node != null && !node.Selectable)
					{
						node = _175C.ᝩ(node);
					}
					if (node != null)
					{
						ᝢ.SelectNode(node, eTreeAction.Keyboard);
					}
				}
			}
			return true;
		}
		Node selectedNode = ᝢ.SelectedNode;
		if (selectedNode != null && !selectedNode.IsKeyboardNavigationEnabled(ؿ))
		{
			return false;
		}
		if ((int)ؿ.KeyCode == 39)
		{
			if (selectedNode != null && selectedNode.Cells.Count > 1 && ᝢ.SelectionPerCell && selectedNode.CellNavigationEnabled)
			{
				if (selectedNode.SelectedCell == null)
				{
					selectedNode.SetSelectedCell(_175C.ដ(selectedNode, -1), eTreeAction.Keyboard);
				}
				else
				{
					selectedNode.SetSelectedCell(_175C.ដ(selectedNode, selectedNode.Cells.IndexOf(selectedNode.SelectedCell)), eTreeAction.Keyboard);
				}
				return true;
			}
			if (ᝢ.View == eView.Tile)
			{
				Node node2 = _175C.ᝩ(selectedNode);
				if (node2 != null)
				{
					if (selectedNode.Expanded || !selectedNode.HasChildNodes)
					{
						ᝢ.SelectNode(node2, eTreeAction.Keyboard);
					}
					else
					{
						selectedNode.Expand(eTreeAction.Keyboard);
					}
				}
				else if (selectedNode != null && selectedNode.ExpandVisibility == eNodeExpandVisibility.Visible && !selectedNode.Expanded)
				{
					selectedNode.Expand(eTreeAction.Keyboard);
				}
			}
			else
			{
				Node node3 = _175C._177C(selectedNode);
				if (node3 != null)
				{
					if (selectedNode.Expanded)
					{
						ᝢ.SelectNode(node3, eTreeAction.Keyboard);
					}
					else
					{
						selectedNode.Expand(eTreeAction.Keyboard);
					}
				}
				else if (selectedNode != null && selectedNode.ExpandVisibility == eNodeExpandVisibility.Visible && !selectedNode.Expanded)
				{
					selectedNode.Expand(eTreeAction.Keyboard);
				}
			}
		}
		else if ((int)ؿ.KeyCode == 37)
		{
			if (selectedNode != null && selectedNode.Cells.Count > 1 && ᝢ.SelectionPerCell && selectedNode.CellNavigationEnabled)
			{
				if (selectedNode.SelectedCell == null)
				{
					selectedNode.SetSelectedCell(_175C.ឋ(selectedNode, selectedNode.Cells.Count - 1), eTreeAction.Keyboard);
				}
				else
				{
					selectedNode.SetSelectedCell(_175C.ឋ(selectedNode, selectedNode.Cells.IndexOf(selectedNode.SelectedCell)), eTreeAction.Keyboard);
				}
				return true;
			}
			if (ᝢ.View == eView.Tile)
			{
				Node node4 = _175C.ᝯ(selectedNode);
				if (node4 != null)
				{
					ᝢ.SelectNode(node4, eTreeAction.Keyboard);
				}
			}
			else if (selectedNode.Expanded && selectedNode.IsSelected && _175C._177C(selectedNode) != null)
			{
				selectedNode.Collapse(eTreeAction.Keyboard);
			}
			else if (selectedNode.Parent != null)
			{
				ᝢ.SelectNode(selectedNode.Parent, eTreeAction.Keyboard);
			}
		}
		else if ((int)ؿ.KeyCode == 35)
		{
			Node node5 = _175C.ᝥ(ᝢ);
			if (node5 != null)
			{
				if (!node5.Selectable)
				{
					while (node5 != null)
					{
						node5 = _175C.ᝯ(node5);
						if (node5 != null && node5.Selectable)
						{
							break;
						}
					}
				}
				if (node5 != null)
				{
					ᝢ.SelectNode(node5, eTreeAction.Keyboard);
				}
			}
		}
		else if ((int)ؿ.KeyCode == 36 || ((int)ؿ.KeyCode == 34 && selectedNode == null))
		{
			Node node6 = _175C.ᝤ(ᝢ);
			if (node6 != null)
			{
				if (!node6.Selectable)
				{
					while (node6 != null)
					{
						node6 = _175C.ᝩ(node6);
						if (node6 != null && node6.Selectable)
						{
							break;
						}
					}
				}
				if (node6 != null)
				{
					ᝢ.SelectNode(node6, eTreeAction.Keyboard);
				}
			}
		}
		else if ((int)ؿ.KeyCode == 34)
		{
			Node node7 = _175C.ᝡ(ᝢ);
			if (node7 != null && ᝢ.SelectedNode == node7 && ᝢ.VScrollBar != null && ᝢ.AutoScroll)
			{
				ᝢ.AutoScrollPosition = new Point(ᝢ.AutoScrollPosition.X, Math.Max(ᝢ.AutoScrollPosition.Y - ᝢ.VScrollBar.LargeChange, -(ᝢ.VScrollBar.Maximum - ᝢ.VScrollBar.LargeChange)));
				node7 = _175C.ᝡ(ᝢ);
			}
			if (node7 != null)
			{
				ᝢ.SelectNode(node7, eTreeAction.Keyboard);
			}
		}
		else if ((int)ؿ.KeyCode == 33)
		{
			Node node8 = _175C.ᝣ(ᝢ);
			if (node8 != null && ᝢ.SelectedNode == node8 && ᝢ.VScrollBar != null && ᝢ.AutoScroll && ᝢ.AutoScrollPosition.Y < 0)
			{
				ᝢ.AutoScrollPosition = new Point(ᝢ.AutoScrollPosition.X, Math.Min(0, ᝢ.AutoScrollPosition.Y + ᝢ.VScrollBar.LargeChange));
				node8 = _175C.ᝣ(ᝢ);
			}
			if (node8 != null)
			{
				ᝢ.SelectNode(node8, eTreeAction.Keyboard);
			}
		}
		else if ((ؿ.KeyCode & 0x28) == 40)
		{
			int num = 0;
			if (selectedNode != null && selectedNode.SelectedCell != null)
			{
				num = selectedNode.Cells.IndexOf(selectedNode.SelectedCell);
			}
			Node node9 = _175C.ᝩ(selectedNode);
			if ((ؿ.KeyData & 0x10000) == 65536 && ᝢ.MultiSelect && ᝢ.SelectedNodes.Count > 1 && ᝢ.SelectedNodes[0].Bounds.Y > ᝢ.SelectedNodes[ᝢ.SelectedNodes.Count - 1].Bounds.Y)
			{
				node9 = ᝢ.SelectedNodes[ᝢ.SelectedNodes.Count - 1];
			}
			if (node9 != null)
			{
				if (!node9.CanSelect)
				{
					int num2 = 0;
					while (node9 != null && num2 < 100)
					{
						node9 = _175C.ᝩ(node9);
						if (node9 != null && node9.CanSelect)
						{
							break;
						}
					}
				}
				if (node9 != null)
				{
					if ((ؿ.KeyData & 0x10000) == 65536 && ᝢ.MultiSelect && ᝢ.SelectedNodes.Count > 0)
					{
						if (ᝢ.MultiSelectRule == eMultiSelectRule.SameParent && ᝢ.SelectedNodes[0].Parent != node9.Parent)
						{
							return true;
						}
						if (node9.IsSelected)
						{
							ᝢ.SelectedNodes.Remove(node9, eTreeAction.Keyboard);
						}
						else
						{
							ᝢ.SelectedNodes.Add(node9, eTreeAction.Keyboard);
						}
						node9.EnsureVisible();
					}
					else
					{
						ᝢ.SelectNode(node9, eTreeAction.Keyboard);
						if (ᝢ.SelectionPerCell && num < node9.Cells.Count && num > 0)
						{
							node9.SetSelectedCell(node9.Cells[num], eTreeAction.Keyboard);
						}
					}
				}
			}
		}
		else if ((ؿ.KeyCode & 0x26) == 38)
		{
			int num3 = 0;
			if (selectedNode != null && selectedNode.SelectedCell != null)
			{
				num3 = selectedNode.Cells.IndexOf(selectedNode.SelectedCell);
			}
			Node node10 = _175C.ᝯ(selectedNode);
			if (node10 != null)
			{
				if (!node10.CanSelect)
				{
					int num4 = 0;
					while (node10 != null && num4 < 100)
					{
						node10 = _175C.ᝯ(node10);
						if (node10 != null && node10.CanSelect)
						{
							break;
						}
					}
				}
				if (node10 != null)
				{
					if ((ؿ.KeyData & 0x10000) == 65536 && ᝢ.MultiSelect && ᝢ.SelectedNodes.Count > 0)
					{
						if (ᝢ.MultiSelectRule == eMultiSelectRule.SameParent && ᝢ.SelectedNodes[0].Parent != node10.Parent)
						{
							return true;
						}
						if (node10.IsSelected)
						{
							ᝢ.SelectedNodes.Remove(ᝢ.SelectedNodes[ᝢ.SelectedNodes.Count - 1], eTreeAction.Keyboard);
						}
						else
						{
							ᝢ.SelectedNodes.Add(node10, eTreeAction.Keyboard);
						}
						node10.EnsureVisible();
					}
					else if (node10 != null)
					{
						ᝢ.SelectNode(node10, eTreeAction.Keyboard);
						if (ᝢ.SelectionPerCell && num3 < node10.Cells.Count && num3 > 0)
						{
							node10.SetSelectedCell(node10.Cells[num3], eTreeAction.Keyboard);
						}
					}
				}
			}
		}
		return true;
	}
}
internal class _29C1 : IntegerInput, ICellEditControl
{
	private EventHandler _17C3;

	private EventHandler _17C4;

	public object CurrentValue
	{
		get
		{
			if (base.FreeTextEntryMode && base.IsKeyboardFocusWithin)
			{
				ApplyFreeTextValue();
			}
			return ((Control)this).Text;
		}
		set
		{
			if (value is int)
			{
				base.Value = (int)value;
				return;
			}
			if (value == null)
			{
				base.ValueObject = null;
				return;
			}
			string s = Utilities.ៗ(value.ToString());
			int result = 0;
			int.TryParse(s, out result);
			base.Value = result;
		}
	}

	public bool EditWordWrap
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public event EventHandler EditComplete
	{
		add
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler CancelEdit
	{
		add
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public void BeginEdit()
	{
		((Control)this).MinimumSize = new Size(32, 10);
	}

	public void EndEdit()
	{
		((Component)(object)this).Dispose();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 13 || (int)keyData == 9)
		{
			if (_17C3 != null)
			{
				_17C3(this, new EventArgs());
			}
			return true;
		}
		if ((int)keyData == 27)
		{
			if (_17C4 != null)
			{
				_17C4(this, new EventArgs());
			}
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}
}
internal class _29C2 : DoubleInput, ICellEditControl
{
	private EventHandler _17C3;

	private EventHandler _17C4;

	public object CurrentValue
	{
		get
		{
			if (base.FreeTextEntryMode && base.IsKeyboardFocusWithin)
			{
				ApplyFreeTextValue();
			}
			return ((Control)this).Text;
		}
		set
		{
			if (value is int)
			{
				base.Value = (int)value;
				return;
			}
			if (value == null)
			{
				base.ValueObject = null;
				return;
			}
			string s = Utilities.ៗ(value.ToString());
			double result = 0.0;
			double.TryParse(s, out result);
			base.Value = result;
		}
	}

	public bool EditWordWrap
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public event EventHandler EditComplete
	{
		add
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler CancelEdit
	{
		add
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public void BeginEdit()
	{
		((Control)this).MinimumSize = new Size(32, 10);
	}

	public void EndEdit()
	{
		((Component)(object)this).Dispose();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 13 || (int)keyData == 9)
		{
			if (_17C3 != null)
			{
				_17C3(this, new EventArgs());
			}
			return true;
		}
		if ((int)keyData == 27)
		{
			if (_17C4 != null)
			{
				_17C4(this, new EventArgs());
			}
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}
}
internal class _29CA : DateTimeInput, ICellEditControl
{
	private EventHandler _17C3;

	private EventHandler _17C4;

	public object CurrentValue
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			if (value is DateTime)
			{
				base.Value = (DateTime)value;
			}
			else if (value == null)
			{
				base.ValueObject = null;
			}
			else if (value is string)
			{
				DateTime result = DateTime.MinValue;
				if (DateTime.TryParse((string)value, out result))
				{
					base.Value = result;
				}
			}
		}
	}

	public bool EditWordWrap
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public event EventHandler EditComplete
	{
		add
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler CancelEdit
	{
		add
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _17C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public _29CA()
	{
		base.BackgroundStyle.BorderColor = Color.Transparent;
	}

	public void BeginEdit()
	{
		((Control)this).MinimumSize = new Size(70, 10);
	}

	public void EndEdit()
	{
		((Component)(object)this).Dispose();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 13 || (int)keyData == 9)
		{
			if (_17C3 != null)
			{
				_17C3(this, new EventArgs());
			}
			return true;
		}
		if ((int)keyData == 27)
		{
			if (_17C4 != null)
			{
				_17C4(this, new EventArgs());
			}
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}
}
internal class _29DC
{
	public Point[] _29DD;

	public Point[] _29DE;
}
internal enum _0883
{
	_0884,
	_0885,
	_0886,
	_0887
}
