using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Layout;

internal abstract class Ԫ
{
	protected int ܗ;

	protected int ܘ;

	protected AdvTree _0707;

	protected Rectangle ܙ;

	protected Size ܚ = new Size(8, 8);

	private Size _ܛ = Size.Empty;

	private int ܜ = 10;

	private int ܝ = 16;

	private LeftRightAlignment ܞ;

	private int _0596 = 3;

	private int _0595;

	private ܥ ӹ;

	private Graphics ܟ;

	private _070F __05A2;

	private int _ܠ = 16;

	public _070F _070F
	{
		get
		{
			return __05A2;
		}
		set
		{
			__05A2 = value;
		}
	}

	public int ܐ => ܘ;

	public int _0711 => ܗ;

	public Rectangle ܒ
	{
		get
		{
			return ܙ;
		}
		set
		{
			ܙ = value;
		}
	}

	public Graphics ܓ
	{
		get
		{
			return ܟ;
		}
		set
		{
			ܟ = value;
		}
	}

	internal bool ܔ => ܟ == null;

	public virtual int TreeLayoutChildNodeIndent
	{
		get
		{
			return ܝ;
		}
		set
		{
			ܝ = value;
		}
	}

	protected virtual bool RootHasExpandedPart => true;

	protected virtual bool ReserveExpandPartSpace => false;

	public AdvTree ܕ => _0707;

	internal int ܖ
	{
		get
		{
			return _ܠ;
		}
		set
		{
			_ܠ = value;
		}
	}

	public virtual LeftRightAlignment LeftRight
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ܞ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			ܞ = value;
		}
	}

	public Ԫ(AdvTree treeControl, Rectangle clientArea, _070F layoutSettings)
	{
		_0707 = treeControl;
		ܙ = clientArea;
		__05A2 = layoutSettings;
	}

	public virtual void PerformLayout()
	{
	}

	public virtual void UpdateTopLevelColumnsWidth()
	{
	}

	public virtual void PerformSingleNodeLayout(Node node)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		if (node == null)
		{
			return;
		}
		PrepareStyles();
		Graphics graphics = GetGraphics();
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		_ = graphics.TextRenderingHint;
		if (_0707.AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		try
		{
			_2710 defaultNodeLayoutContextInfo = GetDefaultNodeLayoutContextInfo(graphics);
			defaultNodeLayoutContextInfo._2712 = node;
			if (node.IsDragNode)
			{
				defaultNodeLayoutContextInfo._2714 = new _2728(new List<_2729>(), hasAutoSizeColumn: false);
			}
			LayoutNode(defaultNodeLayoutContextInfo);
		}
		finally
		{
			if (_0707.AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
			graphics.Dispose();
		}
	}

	protected virtual Graphics GetGraphics()
	{
		if (ܟ != null)
		{
			return ܟ;
		}
		return _0707.CreateGraphics();
	}

	protected virtual void PrepareStyles()
	{
		foreach (ElementStyle style in _0707.Styles)
		{
			if (style.ݺ)
			{
				ᝈ.ᝋ(style, ((Control)_0707).Font);
			}
		}
		if (__05A2 != null)
		{
			_ܛ = Dpi.Size(__05A2._073D);
		}
		else
		{
			_ܛ = Dpi.Size(ܚ);
		}
	}

	protected virtual _2728 GetDefaultColumnInfo()
	{
		List<_2729> list = new List<_2729>();
		_2728 _27282 = new _2728(list, hasAutoSizeColumn: false);
		ColumnHeaderCollection columns = _0707.Columns;
		if (columns != null)
		{
			for (int i = 0; i < columns.Count; i++)
			{
				int num = columns.ᜭ[i];
				ColumnHeader columnHeader = columns[num];
				_2729 _2729 = new _2729(columnHeader.Bounds.Width, columnHeader.Visible, columnHeader, num);
				list.Add(_2729);
				_27282._272A |= _2729._272D;
			}
		}
		return _27282;
	}

	protected virtual _2728 GetNodeColumnInfo(Node node)
	{
		if (!node.HasColumns)
		{
			return null;
		}
		List<_2729> list = new List<_2729>();
		_2728 _27282 = new _2728(list, hasAutoSizeColumn: false);
		ColumnHeaderCollection nodesColumns = node.NodesColumns;
		for (int i = 0; i < nodesColumns.Count; i++)
		{
			int num = nodesColumns.ᜭ[i];
			ColumnHeader columnHeader = nodesColumns[num];
			_2729 _2729 = new _2729(columnHeader.Bounds.Width, columnHeader.Visible, columnHeader, num);
			list.Add(_2729);
			_27282._272A |= _2729._272D;
		}
		return _27282;
	}

	public virtual ColumnHeaderCollection GetColumnHeader(string name)
	{
		if (name == "" || name == null)
		{
			return null;
		}
		return _0707.ԩ.GetByName(name).Columns;
	}

	protected virtual void LayoutCommandPart(_2710 layoutInfo, ElementStyle nodeStyle)
	{
		Rectangle commandBoundsRelative = new Rectangle(layoutInfo._2712.ContentBounds.Right - _070F.CommandAreaWidth - ᝈ._1755(nodeStyle, eSpacePart.Border, eStyleSide.Right), layoutInfo._2712.ContentBounds.Y + ᝈ._1755(nodeStyle, eSpacePart.Border, eStyleSide.Top), _070F.CommandAreaWidth, layoutInfo._2712.ContentBounds.Height - ᝈ._1755(nodeStyle, eSpacePart.Border, eStyleSide.Top) - ᝈ._1755(nodeStyle, eSpacePart.Border, eStyleSide.Bottom));
		layoutInfo._2712.CommandBoundsRelative = commandBoundsRelative;
	}

	protected virtual void LayoutExpandPart(_2710 layoutInfo, bool bLeftNode, int x)
	{
		Node _2712 = layoutInfo._2712;
		Size expandPartSize = GetExpandPartSize();
		Rectangle expandPartRectangle = new Rectangle(x, 0, expandPartSize.Width, expandPartSize.Height);
		if (_2712.ExpandPartVerticalAlignment == eVerticalAlign.Middle)
		{
			expandPartRectangle.Y = (_2712.BoundsRelative.Height - expandPartRectangle.Height) / 2;
		}
		else if (_2712.ExpandPartVerticalAlignment == eVerticalAlign.Top)
		{
			expandPartRectangle.Y = Dpi.Height3;
		}
		else
		{
			expandPartRectangle.Y = _2712.BoundsRelative.Height - expandPartSize.Height - Dpi.Height3;
		}
		if (bLeftNode || (layoutInfo._271C && layoutInfo._074C))
		{
			expandPartRectangle.X += (layoutInfo._2726 - expandPartRectangle.Width) / 2;
		}
		else
		{
			expandPartRectangle.X = _2712.BoundsRelative.Right - layoutInfo._2726 + (layoutInfo._2726 - expandPartSize.Width) / 2;
		}
		_2712.SetExpandPartRectangle(expandPartRectangle);
	}

	protected virtual Size GetExpandPartSize()
	{
		return _ܛ;
	}

	protected virtual void LayoutNode(_2710 layoutInfo)
	{
		bool flag = HasExpandPart(layoutInfo);
		bool flag2 = HasCommandPart(layoutInfo);
		Node _2712 = layoutInfo._2712;
		Rectangle bounds = new Rectangle(layoutInfo._0749, layoutInfo._074A, 0, 0);
		Rectangle empty = Rectangle.Empty;
		int num = 0;
		int num2 = 0;
		bool flag3 = layoutInfo._074C;
		layoutInfo._2724 = 0;
		if ((flag3 && flag) || ReserveExpandPartSpace)
		{
			layoutInfo._2724 = layoutInfo._2726 + GetCellLayout().ܪ;
			num2 += layoutInfo._2726 + GetCellLayout().ܪ;
		}
		int num3 = num2;
		int num4 = 0;
		ElementStyle elementStyle = null;
		elementStyle = ((_2712.Expanded && _2712.StyleExpanded != null) ? _2712.StyleExpanded : ((_2712.Style == null) ? layoutInfo._2717 : _2712.Style));
		empty.X = num3;
		if (elementStyle != null)
		{
			num3 += ᝈ.ᝐ(elementStyle);
			num4 += ᝈ._1753(elementStyle);
			empty.X += elementStyle.MarginLeft;
			empty.Y += elementStyle.MarginTop;
		}
		Size size = GetCellLayout().LayoutCells(layoutInfo, num3, num4);
		_2712.SetCellsBounds(new Rectangle(num3, num4, size.Width, size.Height));
		num = size.Height;
		num2 += size.Width;
		empty.Width = size.Width;
		empty.Height = size.Height;
		if (elementStyle != null)
		{
			empty.Width += ᝈ._1755(elementStyle, eSpacePart.Padding | eSpacePart.Border, eStyleSide.Left) + ᝈ._1755(elementStyle, eSpacePart.Padding | eSpacePart.Border, eStyleSide.Right);
			empty.Height += ᝈ._1755(elementStyle, eSpacePart.Padding | eSpacePart.Border, eStyleSide.Top) + ᝈ._1755(elementStyle, eSpacePart.Padding | eSpacePart.Border, eStyleSide.Bottom);
			num2 += ᝈ.ᝍ(elementStyle);
			num += ᝈ.ᝏ(elementStyle);
		}
		if (!flag3 && flag && !ReserveExpandPartSpace)
		{
			num2 += layoutInfo._2726;
		}
		if (flag2)
		{
			num2 += _070F.CommandAreaWidth;
			empty.Width += _070F.CommandAreaWidth;
		}
		bounds.Height = num;
		bounds.Width = num2;
		_2712.SetBounds(bounds);
		_2712.SetContentBounds(empty);
		if (flag2)
		{
			LayoutCommandPart(layoutInfo, elementStyle);
		}
		else
		{
			_2712.CommandBoundsRelative = Rectangle.Empty;
		}
		if (flag || ReserveExpandPartSpace)
		{
			LayoutExpandPart(layoutInfo, flag3, 0);
		}
		else
		{
			_2712.SetExpandPartRectangle(Rectangle.Empty);
		}
		_2712.SizeChanged = false;
		ܣ(layoutInfo);
	}

	protected virtual bool HasExpandPart(_2710 layoutInfo)
	{
		Node _2712 = layoutInfo._2712;
		if (_2712.ExpandVisibility == eNodeExpandVisibility.Auto)
		{
			if ((ܢ(_2712) && !RootHasExpandedPart) || !_175C._1776(_2712))
			{
				return false;
			}
			return true;
		}
		return _2712.ExpandVisibility == eNodeExpandVisibility.Visible;
	}

	protected virtual bool HasCommandPart(_2710 layoutInfo)
	{
		return layoutInfo._2712.CommandButton;
	}

	protected internal virtual ܥ GetCellLayout()
	{
		if (ӹ == null)
		{
			ӹ = new ܥ(_070F);
		}
		return ӹ;
	}

	protected virtual void OffsetNodeLocation(Node node, int x, int y)
	{
		node.SetBounds(new Rectangle(node.BoundsRelative.X + x, node.BoundsRelative.Y + y, node.BoundsRelative.Width, node.BoundsRelative.Height));
		if (node.Expanded)
		{
			node.ChildNodesBounds = new Rectangle(node.ChildNodesBounds.X + x, node.ChildNodesBounds.Y + y, node.ChildNodesBounds.Width, node.ChildNodesBounds.Height);
		}
	}

	protected virtual _2710 GetDefaultNodeLayoutContextInfo(Graphics graphics)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		_2710 _27102 = new _2710();
		_27102.ᶮ = ܙ;
		_27102._2714 = GetDefaultColumnInfo();
		_27102._2715 = null;
		_27102._2725 = _0707.Indent;
		_27102._0749 = 0;
		_27102._074A = 0;
		_27102._2713 = _0707.BackgroundStyle.PaddingLeft;
		_27102._2711 = ((Control)_0707).Font;
		_27102._074C = (int)LeftRight == 0;
		_27102.ܓ = graphics;
		_27102._2719 = _0707.Styles;
		_27102.ԥ = new ArrayList();
		if (_0707.CellLayout != 0)
		{
			_27102.ܥ = _0707.CellLayout;
		}
		if (_0707.CellPartLayout != 0)
		{
			_27102._271A = _0707.CellPartLayout;
		}
		if (_0707.NodeStyle != null)
		{
			_27102._2717 = _0707.NodeStyle;
		}
		if (_0707.CellStyleDefault != null)
		{
			_27102._2716 = _0707.CellStyleDefault;
		}
		else
		{
			_27102._2716 = ElementStyle.GetDefaultCellStyle(_27102._2717);
		}
		if (_0707.ColumnStyleNormal != null)
		{
			ᝈ.ᝋ(_0707.ColumnStyleNormal, _27102._2711);
			_27102._2718 = _0707.ColumnStyleNormal.Size;
		}
		if (_27102._2718.IsEmpty)
		{
			_27102._2718.Height = _27102._2711.Height + 4;
		}
		_27102._271E = Dpi.Width(ܕ.ExpandWidth);
		_27102.ݎ = ܕ.View;
		_27102._2721 = Dpi.Size(ܕ.TileSize);
		_27102._2723 = ܕ.ColumnStyleNormal;
		_27102._2726 = Dpi.Width(_070F.ExpandAreaWidth);
		return _27102;
	}

	protected Node[] ܡ()
	{
		if (_0707.DisplayRootNode != null)
		{
			return new Node[1] { _0707.DisplayRootNode };
		}
		Node[] array = new Node[_0707.Nodes.Count];
		_0707.Nodes.CopyTo(array);
		return array;
	}

	protected bool ܢ(Node _0653)
	{
		return _175C.ܢ(_0707, _0653);
	}

	protected virtual void EmptyBoundsUnusedNodes(Node[] topLevelNodes)
	{
		if (_0707.DisplayRootNode != null)
		{
			Node prevVisibleNode;
			for (prevVisibleNode = _0707.DisplayRootNode.PrevVisibleNode; prevVisibleNode != null; prevVisibleNode = prevVisibleNode.PrevVisibleNode)
			{
				prevVisibleNode.SetBounds(Rectangle.Empty);
			}
			prevVisibleNode = _0707.DisplayRootNode.NextNode;
			if (prevVisibleNode == null)
			{
				for (prevVisibleNode = _0707.DisplayRootNode.Parent; prevVisibleNode != null; prevVisibleNode = prevVisibleNode.Parent)
				{
					if (prevVisibleNode.NextNode != null)
					{
						prevVisibleNode = prevVisibleNode.NextNode;
						break;
					}
				}
			}
			while (prevVisibleNode != null)
			{
				prevVisibleNode.SetBounds(Rectangle.Empty);
				prevVisibleNode = prevVisibleNode.NextVisibleNode;
			}
		}
		else
		{
			for (int i = 1; i < topLevelNodes.Length; i++)
			{
				topLevelNodes[i].SetBounds(Rectangle.Empty);
			}
		}
	}

	public void ܣ(_2710 ܤ)
	{
		Node _2712 = ܤ._2712;
		if (!_2712.HasColumns || !_2712.Expanded)
		{
			_2712.ColumnHeaderHeight = 0;
			return;
		}
		int num = 2;
		int num2 = ܤ._0749 + ܖ + _2712.NodesIndent;
		int ى = ܤ._2712.BoundsRelative.Bottom + num;
		bool flag = ܤ._271B && ܤ._074C;
		int _2726 = ܤ._2726;
		int ܪ = GetCellLayout().ܪ;
		if (!flag)
		{
			num2 += _2726 + ܪ;
		}
		int num3 = ܤ.ᶮ.Width - (ܤ._0749 + _2726);
		if (num3 <= 0)
		{
			num3 = ܤ.ᶮ.Width;
		}
		_2712.ColumnHeaderHeight = _272F.ܣ(ܤ, num2, ى, num3, GetCellLayout()._070F._073C) + num;
		if (!_2712.NodesColumnsHeaderVisible)
		{
			_2712.ColumnHeaderHeight = 0;
		}
	}
}
