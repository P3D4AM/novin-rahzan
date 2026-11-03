using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display;

public class NodeDisplay
{
	private Point Ӵ = Point.Empty;

	private Point _0706 = Point.Empty;

	private AdvTree _0707;

	internal ArrayList __0708 = new ArrayList(100);

	public virtual Point Offset
	{
		get
		{
			if (!_0706.IsEmpty)
			{
				return _0706;
			}
			Node node = _0707._06D6();
			if (node == null)
			{
				return Point.Empty;
			}
			_0707.GetScreenSize(new Size(_0707.Ԫ.ܐ, _0707.Ԫ._0711));
			return _0707.GetLayoutPosition(Ӵ);
		}
		set
		{
			Ӵ = value;
		}
	}

	public bool LockOffset
	{
		get
		{
			return !_0706.IsEmpty;
		}
		set
		{
			if (value)
			{
				_0706 = Offset;
			}
			else
			{
				_0706 = Point.Empty;
			}
		}
	}

	public virtual Point DefaultOffset
	{
		get
		{
			Node node = _0707._06D6();
			if (node == null)
			{
				return Point.Empty;
			}
			return ((Control)_0707).ClientRectangle.Location;
		}
	}

	protected virtual AdvTree Tree
	{
		get
		{
			return _0707;
		}
		set
		{
			_0707 = value;
		}
	}

	public ArrayList PaintedNodes => __0708;

	public NodeDisplay(AdvTree tree)
	{
		_0707 = tree;
	}

	public virtual void Paint(Graphics g, Rectangle clipRectangle)
	{
	}

	public void SetLockedOffset(Point p)
	{
		_0706 = p;
	}

	public Point GetLockedOffset()
	{
		return _0706;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Rectangle GetNodeRectangle(eNodeRectanglePart part, Node node, Point offset)
	{
		Rectangle result = Rectangle.Empty;
		switch (part)
		{
		case eNodeRectanglePart.CellsBounds:
			result = node.CellsBoundsRelative;
			if (!result.IsEmpty)
			{
				result.Offset(offset);
				result.Offset(node.BoundsRelative.Location);
			}
			break;
		case eNodeRectanglePart.ExpandHitTestBounds:
		{
			Rectangle nodeRectangle = GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, offset);
			result = node.ExpandPartRectangleRelative;
			if (!result.IsEmpty)
			{
				result.Offset(offset);
				result.Offset(node.BoundsRelative.Location);
			}
			result.Y = nodeRectangle.Y;
			result.Height = nodeRectangle.Height;
			result.Inflate(1, 0);
			break;
		}
		case eNodeRectanglePart.ExpandBounds:
			result = node.ExpandPartRectangleRelative;
			if (!result.IsEmpty)
			{
				result.Offset(offset);
				result.Offset(node.BoundsRelative.Location);
			}
			break;
		case eNodeRectanglePart.CommandBounds:
			result = node.CommandBoundsRelative;
			if (!result.IsEmpty)
			{
				result.Offset(offset);
				result.Offset(node.BoundsRelative.Location);
			}
			break;
		case eNodeRectanglePart.NodeContentBounds:
			result = node.ContentBounds;
			if (!result.IsEmpty)
			{
				result.Offset(offset);
				result.Offset(node.BoundsRelative.Location);
			}
			break;
		case eNodeRectanglePart.NodeBounds:
			result = node.BoundsRelative;
			if (!result.IsEmpty)
			{
				result.Offset(offset);
			}
			break;
		case eNodeRectanglePart.ChildNodeBounds:
			result = node.ChildNodesBounds;
			if (!result.IsEmpty)
			{
				result.Offset(offset);
			}
			break;
		case eNodeRectanglePart.ColumnsBounds:
			if (_0709(node))
			{
				result = node.NodesColumns.Bounds;
				if (!result.IsEmpty)
				{
					result.Offset(offset);
				}
			}
			break;
		}
		return result;
	}

	internal static bool _0709(Node _0653)
	{
		if (_0653.Expanded && _0653.HasColumns)
		{
			return _0653.NodesColumnsHeaderVisible;
		}
		return false;
	}

	internal static Rectangle _070A(_1757 _070B, Cell ڂ, Point _066A)
	{
		Rectangle result = Rectangle.Empty;
		if (ڂ.Parent == null)
		{
			return result;
		}
		switch (_070B)
		{
		case _1757._1758:
			result = ڂ.CheckBoxBoundsRelative;
			if (!result.IsEmpty)
			{
				result.Offset(_066A);
				result.Offset(ڂ.Parent.BoundsRelative.Location);
			}
			break;
		case _1757._1759:
			result = ڂ.ImageBoundsRelative;
			if (!result.IsEmpty)
			{
				result.Offset(_066A);
				result.Offset(ڂ.Parent.BoundsRelative.Location);
			}
			break;
		case _1757._175A:
			result = ڂ.TextContentBounds;
			if (!result.IsEmpty)
			{
				result.Offset(_066A);
				result.Offset(ڂ.Parent.BoundsRelative.Location);
			}
			break;
		case _1757._175B:
			result = ڂ.BoundsRelative;
			if (!result.IsEmpty)
			{
				result.Offset(_066A);
				result.Offset(ڂ.Parent.BoundsRelative.Location);
			}
			break;
		}
		return result;
	}

	internal static bool _070C(Node _0653)
	{
		if ((_0653.Nodes.Count > 0 && _0653.ExpandVisibility != eNodeExpandVisibility.Hidden) || _0653.ExpandVisibility == eNodeExpandVisibility.Visible)
		{
			return true;
		}
		return false;
	}

	protected NodeExpandDisplay GetExpandDisplay(eExpandButtonType e)
	{
		NodeExpandDisplay result = null;
		switch (e)
		{
		case eExpandButtonType.Rectangle:
			result = new NodeExpandRectDisplay();
			break;
		case eExpandButtonType.Triangle:
			result = new _2771();
			break;
		case eExpandButtonType.Ellipse:
			result = new NodeExpandEllipseDisplay();
			break;
		case eExpandButtonType.Image:
			result = new NodeExpandImageDisplay();
			break;
		}
		return result;
	}

	protected bool IsRootNode(Node node)
	{
		return _175C.ܢ(_0707, node);
	}

	protected ElementStyle GetDefaultNodeStyle()
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.TextColorSchemePart = eColorSchemePart.ItemText;
		return elementStyle;
	}

	public void MoveHostedControls()
	{
		Point offset = Offset;
		float zoom = Tree.Zoom;
		foreach (Cell item in Tree.Ծ)
		{
			Control hostedControl = item.HostedControl;
			if (hostedControl == null)
			{
				continue;
			}
			Rectangle r = _070A(_1757._175A, item, offset);
			Rectangle screenRectangle = Tree.GetScreenRectangle(r);
			if (r.IsEmpty || !(hostedControl.Bounds != screenRectangle))
			{
				continue;
			}
			if (zoom != 1f)
			{
				item.HostedControlSize = r.Size;
				item.IgnoreHostedControlSizeChange = true;
			}
			else
			{
				item.HostedControlSize = Size.Empty;
				if (screenRectangle.Height > hostedControl.Height && hostedControl.Height > 0)
				{
					screenRectangle.Y += (screenRectangle.Height - hostedControl.Height) / 2;
					screenRectangle.Height = hostedControl.Height;
				}
			}
			hostedControl.Bounds = screenRectangle;
			if (zoom != 1f)
			{
				item.IgnoreHostedControlSizeChange = false;
			}
			if (item.Parent != null)
			{
				bool flag = _175C.ᝫ(item.Parent) && item.IsVisible;
				if (flag != hostedControl.Visible)
				{
					hostedControl.Visible = flag;
				}
			}
		}
	}

	internal virtual void _070D(ColumnHeaderCollection _064B, Graphics _٥, bool _070E)
	{
	}
}
