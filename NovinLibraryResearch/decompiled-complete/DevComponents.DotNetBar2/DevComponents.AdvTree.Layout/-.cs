using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.AdvTree.Layout;

internal class _070F
{
	private int __073E = 3;

	private int __073F = 4;

	private int __0740 = 5;

	private int __0741 = 24;

	protected Size __0742 = new Size(8, 8);

	private int __0743 = 10;

	public virtual int NodeVerticalSpacing
	{
		get
		{
			return __073E;
		}
		set
		{
			__073E = value;
		}
	}

	public virtual int NodeHorizontalSpacing
	{
		get
		{
			return __073F;
		}
		set
		{
			__073F = value;
		}
	}

	public int _073C
	{
		get
		{
			return __0740;
		}
		set
		{
			__0740 = value;
		}
	}

	public virtual int ExpandAreaWidth
	{
		get
		{
			return __0741;
		}
		set
		{
			__0741 = value;
		}
	}

	public Size _073D
	{
		get
		{
			return __0742;
		}
		set
		{
			__0742 = value;
		}
	}

	public virtual int CommandAreaWidth
	{
		get
		{
			return __0743;
		}
		set
		{
			__0743 = value;
		}
	}
}
internal class _2706 : Ԫ
{
	protected override bool ReserveExpandPartSpace => true;

	public _2706(AdvTree treeControl, Rectangle clientArea, _070F layoutSettings)
		: base(treeControl, clientArea, layoutSettings)
	{
	}

	public override void UpdateTopLevelColumnsWidth()
	{
		if (base.ܕ.Columns.Count > 0)
		{
			Rectangle ᝆ = ᝈ._1756(base.ܕ.BackgroundStyle, ((Control)base.ܕ).ClientRectangle);
			if (base.ܕ.VScrollBar != null)
			{
				ᝆ.Width -= ((Control)base.ܕ.VScrollBar).Width;
			}
			ᝆ.Height = base.ܕ.Columns.Bounds.Height;
			if (base.ܕ.Columns.Bounds.Width < ᝆ.Width)
			{
				base.ܕ.Columns.ᝅ(ᝆ);
			}
		}
	}

	public override void PerformLayout()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		PrepareStyles();
		Rectangle _2709 = Rectangle.Empty;
		Graphics graphics = GetGraphics();
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		_ = graphics.TextRenderingHint;
		if (_0707.AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		_2710 defaultNodeLayoutContextInfo = GetDefaultNodeLayoutContextInfo(graphics);
		defaultNodeLayoutContextInfo._271C = true;
		defaultNodeLayoutContextInfo._0749 = base.ܒ.X;
		defaultNodeLayoutContextInfo._074A = base.ܒ.Y;
		ܥ cellLayout = GetCellLayout();
		cellLayout.ResetCheckBoxSize();
		if (base.ܕ.CheckBoxImageChecked != null)
		{
			cellLayout.ܦ = base.ܕ.CheckBoxImageChecked.Size;
		}
		_2707(defaultNodeLayoutContextInfo);
		_2728 _27282 = (defaultNodeLayoutContextInfo._2714 = GetDefaultColumnInfo());
		try
		{
			Node[] _270A = ܡ();
			int num = defaultNodeLayoutContextInfo._074A;
			_2709 = _2708(_2709, defaultNodeLayoutContextInfo, _270A);
			bool flag = false;
			bool flag2 = false;
			if (_27282._272A)
			{
				foreach (_2729 item in _27282._2729)
				{
					if (item._272D)
					{
						item._272D = false;
						item.ܐ = item._272B;
						item._272C.Width.ᜬ(item._272B);
						item._272B = 0;
						if (item._272C.Width.AutoSizeMinHeader)
						{
							flag = true;
						}
						if (item._272C.StretchToFill)
						{
							flag2 = true;
						}
					}
				}
				defaultNodeLayoutContextInfo._2712 = null;
				_2707(defaultNodeLayoutContextInfo);
				if (flag || flag2)
				{
					foreach (_2729 item2 in _27282._2729)
					{
						if ((item2._272C.Width.AutoSize && item2._272C.Width.AutoSizeMinHeader) || item2._272C.StretchToFill)
						{
							item2.ܐ = item2._272C.Bounds.Width;
						}
					}
				}
				defaultNodeLayoutContextInfo._074A = num;
				_2709 = _2708(Rectangle.Empty, defaultNodeLayoutContextInfo, _270A);
			}
		}
		finally
		{
			if (_0707.AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
			if (base.ܔ)
			{
				graphics.Dispose();
			}
		}
		if (defaultNodeLayoutContextInfo.ԥ.Count > 0)
		{
			base.ܕ.ԥ = defaultNodeLayoutContextInfo.ԥ;
		}
		else
		{
			base.ܕ.ԥ = null;
		}
		ܘ = _2709.Width;
		ܗ = _2709.Height;
	}

	private void _2707(_2710 ܤ)
	{
		if (base.ܕ.Columns.Count > 0)
		{
			Rectangle rectangle = ܙ;
			ܤ._271D = base.ܕ.Columns;
			int num = _272F.ܣ(ܤ, 0, 0, rectangle.Width, GetCellLayout()._070F._073C);
			num += base._070F.NodeVerticalSpacing;
			if (base.ܕ.ColumnsVisible)
			{
				Rectangle bounds = ܤ._271D.Bounds;
				if (bounds.Width > 0 && bounds.Width < rectangle.Width)
				{
					bounds.Width = rectangle.Width;
					ܤ._271D.ᝅ(bounds);
				}
				ܤ._074A += num;
				base.ܕ.م(ن: true);
			}
			else
			{
				base.ܕ.م(ن: false);
			}
			ܤ._271D = null;
		}
		else
		{
			base.ܕ.م(ن: false);
		}
	}

	private Rectangle _2708(Rectangle _2709, _2710 ܤ, Node[] _270A)
	{
		for (int i = 0; i < _270A.Length; i++)
		{
			Node node = (ܤ._2712 = _270A[i]);
			_270B(ܤ);
			if (node.Visible)
			{
				_2709 = Rectangle.Union(_2709, node.BoundsRelative);
				if (node.Expanded)
				{
					_2709 = Rectangle.Union(_2709, node.ChildNodesBounds);
				}
			}
		}
		return _2709;
	}

	private void _270B(_2710 ܤ)
	{
		Node _2712 = ܤ._2712;
		if (!_2712.Visible)
		{
			return;
		}
		int num = ܤ._074A;
		if (_2712.SizeChanged || _2712.HasColumns || (ܤ._2714 != null && ܤ._2714._272A) || (ܤ._2715 != null && ܤ._2715._272A))
		{
			LayoutNode(ܤ);
		}
		if (_2712.FullRowBackground)
		{
			ܤ.ԥ.Add(_2712);
		}
		if (_2712.BoundsRelative.X != ܤ._0749 || _2712.BoundsRelative.Y != ܤ._074A)
		{
			_2712.SetBounds(new Rectangle(ܤ._0749, ܤ._074A, _2712.BoundsRelative.Width, _2712.BoundsRelative.Height));
		}
		int nodeVerticalSpacing = base._070F.NodeVerticalSpacing;
		ܤ._074A += _2712.BoundsRelative.Height + nodeVerticalSpacing;
		if (NodeDisplay._0709(_2712))
		{
			ܤ._074A += _2712.ColumnHeaderHeight;
		}
		if (!_2712.Expanded)
		{
			return;
		}
		int num2 = ܤ._0749;
		int num3 = ܤ._074A;
		ܤ._0749 += base.ܖ + _2712.NodesIndent;
		_2728 _2715 = ܤ._2715;
		_2728 nodeColumnInfo = GetNodeColumnInfo(_2712);
		Rectangle childNodesBounds = _270C(ܤ, _2712, nodeVerticalSpacing, nodeColumnInfo);
		if (nodeColumnInfo != null && nodeColumnInfo._272A)
		{
			bool flag = false;
			foreach (_2729 item in nodeColumnInfo._2729)
			{
				if (item._272D)
				{
					item.ܐ = item._272B;
					item._272C.Width.ᜬ(item._272B);
					item._272D = false;
					item._272B = 0;
					if (item._272C.Width.AutoSizeMinHeader)
					{
						flag = true;
					}
				}
			}
			ܤ._074A = num;
			ܤ._0749 = num2;
			ܤ._2712 = _2712;
			ܤ._2715 = _2715;
			LayoutNode(ܤ);
			ܤ._074A = num3;
			ܤ._0749 += base.ܖ + _2712.NodesIndent;
			if (flag)
			{
				foreach (_2729 item2 in nodeColumnInfo._2729)
				{
					if (item2._272C.Width.AutoSize && item2._272C.Width.AutoSizeMinHeader)
					{
						item2.ܐ = item2._272C.Bounds.Width;
					}
				}
			}
			childNodesBounds = _270C(ܤ, _2712, nodeVerticalSpacing, nodeColumnInfo);
		}
		_2712.ChildNodesBounds = childNodesBounds;
		ܤ._2715 = _2715;
		ܤ._2712 = _2712;
		ܤ._0749 = num2;
	}

	private Rectangle _270C(_2710 ܤ, Node _0653, int _270D, _2728 _270E)
	{
		Rectangle result = new Rectangle(ܤ._0749, ܤ._074A, 0, 0);
		foreach (Node node in _0653.Nodes)
		{
			if (node.Visible)
			{
				ܤ._2712 = node;
				ܤ._2715 = _270E;
				_270B(ܤ);
				result.Width = Math.Max(result.Width, Math.Max(node.BoundsRelative.Width, (node.Expanded && node.ChildNodesBounds.Width > 0) ? (node.ChildNodesBounds.Right - result.X) : 0));
				result.Height += node.BoundsRelative.Height + (node.Expanded ? (node.ChildNodesBounds.Height + node.ColumnHeaderHeight) : 0) + _270D;
			}
		}
		return result;
	}

	private bool _270F(Node _0653)
	{
		return true;
	}
}
internal class _2710
{
	public Node _2712;

	public Rectangle ᶮ = Rectangle.Empty;

	public int _0749;

	public int _074A;

	public int _2713;

	public _2728 _2714;

	public _2728 _2715;

	public ElementStyle _2716;

	public ElementStyle _2717;

	public Size _2718 = Size.Empty;

	public bool _074C = true;

	public bool _20BA = true;

	public Graphics ܓ;

	public ElementStyleCollection _2719;

	public eCellLayout ܥ;

	public eCellPartLayout _271A = eCellPartLayout.Horizontal;

	public bool _271B;

	public bool _271C;

	public ColumnHeaderCollection _271D;

	public ArrayList ԥ;

	public int _271E;

	public int _271F;

	public int _2720;

	public eView ݎ;

	public Size _2721 = Size.Empty;

	public bool _2722;

	public ElementStyle _2723;

	public int _2724;

	public int _2725;

	public int _2726;

	private Font __187D;

	public int _2727;

	public Font _2711
	{
		get
		{
			return __187D;
		}
		set
		{
			if (__187D != value)
			{
				__187D = value;
				if (__187D != null)
				{
					_2727 = __187D.Height;
				}
				else
				{
					_2727 = 0;
				}
			}
		}
	}
}
internal class _2728
{
	public List<_2729> _2729;

	public bool _272A;

	public _2728(List<_2729> columnInfo, bool hasAutoSizeColumn)
	{
		_2729 = columnInfo;
		_272A = hasAutoSizeColumn;
	}
}
internal class _272F
{
	public static int ܣ(_2710 ܤ, int و, int ى, int _2730, int _2731)
	{
		ColumnHeaderCollection columnHeaderCollection = null;
		Node _2712 = ܤ._2712;
		columnHeaderCollection = ((_2712 != null) ? _2712.NodesColumns : ܤ._271D);
		columnHeaderCollection._1735 = false;
		int num = 0;
		bool flag = false;
		Rectangle rectangle = Rectangle.Empty;
		ColumnHeader columnHeader = null;
		ColumnHeader columnHeader2 = null;
		bool flag2 = true;
		bool flag3 = true;
		for (int i = 0; i < columnHeaderCollection.Count; i++)
		{
			ColumnHeader columnHeader3 = columnHeaderCollection.ColumnAtDisplayIndex(i);
			columnHeader3._171B = false;
			if (!columnHeader3.Visible)
			{
				continue;
			}
			columnHeader3._171C = flag3;
			flag3 = false;
			Rectangle empty = Rectangle.Empty;
			empty.X = و;
			empty.Y = ى;
			if (columnHeader3.Width.AutoSize)
			{
				int num2 = columnHeader3.Width.AutoSizeWidth;
				if (columnHeader3.Width.AutoSizeMinHeader)
				{
					Font val = ܤ._2711;
					if (!string.IsNullOrEmpty(columnHeader3.StyleNormal))
					{
						ElementStyle elementStyle = ܤ._2719[columnHeader3.StyleNormal];
						if (elementStyle != null && elementStyle.Font != null)
						{
							val = elementStyle.Font;
						}
					}
					else if (ܤ._2723 != null && ܤ._2723.Font != null)
					{
						val = ܤ._2723.Font;
					}
					if (val != null)
					{
						int val2 = (int)Math.Ceiling(ܤ.ܓ.MeasureString(columnHeader3.Text, val).Width) + 2;
						num2 = Math.Max(num2, val2);
						columnHeader3.Width.ᜬ(num2);
					}
				}
				empty.Width = num2;
				flag2 = false;
			}
			else if (columnHeader3.Width.Absolute > 0)
			{
				empty.Width = columnHeader3.Width.Absolute;
				flag2 = false;
			}
			else if (columnHeader3.Width.Absolute == -1)
			{
				empty.Width = 0;
				flag2 = false;
			}
			else if (columnHeader3.Width.Relative > 0)
			{
				if (columnHeader3._171C)
				{
					_2730 -= ܤ._271E;
					empty.Width = _2730 * columnHeader3.Width.Relative / 100 - _2731;
					empty.Width += ܤ._271E;
				}
				else
				{
					empty.Width = _2730 * columnHeader3.Width.Relative / 100 - _2731;
				}
				columnHeaderCollection._1735 = true;
			}
			columnHeader = columnHeader3;
			if (columnHeader3.StretchToFill)
			{
				columnHeader2 = columnHeader3;
				columnHeaderCollection._1735 = true;
			}
			if (columnHeader3.StyleNormal == "" && columnHeader3.StyleMouseDown == "" && columnHeader3.StyleMouseOver == "")
			{
				empty.Height = ܤ._2718.Height;
			}
			else
			{
				Size size = Size.Empty;
				if (columnHeader3.StyleNormal != "")
				{
					ᝈ.ᝋ(ܤ._2719[columnHeader3.StyleNormal], ܤ._2711);
					size = ܤ._2719[columnHeader3.StyleNormal].Size;
				}
				if (size.Height == 0)
				{
					empty.Height = ܤ._2718.Height;
				}
				else
				{
					empty.Height = size.Height;
				}
			}
			if (columnHeader3.Image != null && columnHeader3.Image.Height + 4 > empty.Height)
			{
				empty.Height = columnHeader3.Image.Height + 4;
			}
			columnHeader3.SetBounds(empty);
			columnHeader3.SizeChanged = false;
			و += empty.Width + _2731;
			if (empty.Height > num)
			{
				if (num > 0)
				{
					flag = true;
				}
				num = empty.Height;
			}
			else if (empty.Height < num)
			{
				flag = true;
			}
			rectangle = ((!rectangle.IsEmpty) ? Rectangle.Union(rectangle, columnHeader3.Bounds) : columnHeader3.Bounds);
		}
		if (flag)
		{
			foreach (ColumnHeader item in columnHeaderCollection)
			{
				item.SetBounds(new Rectangle(item.Bounds.X, item.Bounds.Y, item.Bounds.Width, num));
			}
		}
		if (columnHeader != null && flag2)
		{
			columnHeader.SetBounds(new Rectangle(columnHeader.Bounds.X, columnHeader.Bounds.Y, columnHeader.Bounds.Width + _2731, columnHeader.Bounds.Height));
			rectangle = Rectangle.Union(rectangle, columnHeader.Bounds);
		}
		if (columnHeader != null)
		{
			columnHeader._171B = true;
		}
		if (columnHeader2 != null && rectangle.Width < _2730)
		{
			int num3 = _2730 - rectangle.Width;
			if (columnHeader2._171C && columnHeader2._171B)
			{
				num3 -= ܤ._2725;
			}
			else if (columnHeader2.Parent != null && columnHeader2.Parent.ParentNode != null)
			{
				num3 -= ܤ._2725;
			}
			columnHeader2.SetBounds(new Rectangle(columnHeader2.Bounds.X, columnHeader2.Bounds.Y, columnHeader2.Bounds.Width + num3, columnHeader2.Bounds.Height));
			rectangle = Rectangle.Union(rectangle, columnHeader2.Bounds);
			if (!columnHeader2._171B)
			{
				int num4 = columnHeaderCollection.GetDisplayIndex(columnHeader2) + 1;
				for (int j = num4; j < columnHeaderCollection.Count; j++)
				{
					ColumnHeader columnHeader5 = columnHeaderCollection.ColumnAtDisplayIndex(j);
					if (columnHeader5.Visible)
					{
						columnHeader5.SetBounds(new Rectangle(columnHeader5.Bounds.X + num3, columnHeader5.Bounds.Y, columnHeader5.Bounds.Width, columnHeader5.Bounds.Height));
						rectangle = Rectangle.Union(rectangle, columnHeader5.Bounds);
					}
				}
			}
		}
		columnHeaderCollection.ᝅ(rectangle);
		return num;
	}
}
internal class _29CB : Ԫ
{
	private bool __29CD = true;

	private _29CF __29CE;

	protected override bool ReserveExpandPartSpace => false;

	public bool _29CC
	{
		get
		{
			return __29CD;
		}
		set
		{
			__29CD = value;
		}
	}

	public _29CB(AdvTree treeControl, Rectangle clientArea, _070F layoutSettings)
		: base(treeControl, clientArea, layoutSettings)
	{
	}

	public override void UpdateTopLevelColumnsWidth()
	{
		base.ܕ.Columns.ᝅ(Rectangle.Empty);
	}

	protected override _2728 GetDefaultColumnInfo()
	{
		List<_2729> columnInfo = new List<_2729>();
		return new _2728(columnInfo, hasAutoSizeColumn: false);
	}

	protected override _2710 GetDefaultNodeLayoutContextInfo(Graphics graphics)
	{
		_2710 defaultNodeLayoutContextInfo = base.GetDefaultNodeLayoutContextInfo(graphics);
		defaultNodeLayoutContextInfo._2722 = _29CC;
		return defaultNodeLayoutContextInfo;
	}

	public override void PerformLayout()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		PrepareStyles();
		Rectangle _2709 = Rectangle.Empty;
		Graphics graphics = GetGraphics();
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		_ = graphics.TextRenderingHint;
		if (_0707.AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		_2710 defaultNodeLayoutContextInfo = GetDefaultNodeLayoutContextInfo(graphics);
		defaultNodeLayoutContextInfo._271C = true;
		defaultNodeLayoutContextInfo._0749 = base.ܒ.X;
		defaultNodeLayoutContextInfo._074A = base.ܒ.Y;
		ܥ cellLayout = GetCellLayout();
		cellLayout.ResetCheckBoxSize();
		if (base.ܕ.CheckBoxImageChecked != null)
		{
			cellLayout.ܦ = base.ܕ.CheckBoxImageChecked.Size;
		}
		_2707(defaultNodeLayoutContextInfo);
		_2728 defaultColumnInfo = GetDefaultColumnInfo();
		defaultNodeLayoutContextInfo._2714 = defaultColumnInfo;
		try
		{
			Node[] _270A = ܡ();
			_ = defaultNodeLayoutContextInfo._074A;
			_2709 = _2708(_2709, defaultNodeLayoutContextInfo, _270A);
		}
		finally
		{
			if (_0707.AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
			if (base.ܔ)
			{
				graphics.Dispose();
			}
		}
		if (defaultNodeLayoutContextInfo.ԥ.Count > 0)
		{
			base.ܕ.ԥ = defaultNodeLayoutContextInfo.ԥ;
		}
		else
		{
			base.ܕ.ԥ = null;
		}
		ܘ = _2709.Width;
		ܗ = _2709.Height;
	}

	private void _2707(_2710 ܤ)
	{
		base.ܕ.م(ن: false);
	}

	private Rectangle _2708(Rectangle _2709, _2710 ܤ, Node[] _270A)
	{
		ܤ._2720 = ܤ._0749;
		bool flag = false;
		for (int i = 0; i < _270A.Length; i++)
		{
			Node node = (ܤ._2712 = _270A[i]);
			if (node.Visible)
			{
				if (__29CD && node.HasChildNodes)
				{
					if (ܤ._271F > 0)
					{
						ܤ._074A += ܤ._271F + base._070F.NodeVerticalSpacing;
					}
					ܤ._271F = 0;
					ܤ._0749 = ܤ._2720;
					flag = true;
				}
				else
				{
					if (flag)
					{
						if (ܤ._271F > 0)
						{
							ܤ._074A += ܤ._271F + base._070F.NodeVerticalSpacing;
						}
						ܤ._271F = 0;
						ܤ._0749 = ܤ._2720;
					}
					flag = false;
				}
			}
			_270B(ܤ);
			if (node.Visible)
			{
				_2709 = Rectangle.Union(_2709, node.BoundsRelative);
				if (node.Expanded && node.HasChildNodes)
				{
					_2709 = Rectangle.Union(_2709, node.ChildNodesBounds);
				}
				if (!__29CD || !node.HasChildNodes)
				{
					ܤ._0749 += node.BoundsRelative.Width + base._070F.NodeHorizontalSpacing;
				}
			}
		}
		return _2709;
	}

	private Rectangle _270C(_2710 ܤ, Node _0653, int _270D, _2728 _270E)
	{
		Rectangle rectangle = new Rectangle(ܤ._0749, ܤ._074A, 0, 0);
		bool flag = false;
		foreach (Node node in _0653.Nodes)
		{
			if (!node.Visible)
			{
				continue;
			}
			if (__29CD && node.HasChildNodes)
			{
				if (ܤ._271F > 0)
				{
					ܤ._074A += ܤ._271F + base._070F.NodeVerticalSpacing;
				}
				ܤ._271F = 0;
				ܤ._0749 = ܤ._2720;
				flag = true;
			}
			else if (flag)
			{
				if (ܤ._271F > 0)
				{
					ܤ._074A += ܤ._271F + base._070F.NodeVerticalSpacing;
				}
				ܤ._271F = 0;
				ܤ._0749 = ܤ._2720;
				flag = false;
			}
			ܤ._2712 = node;
			ܤ._2715 = _270E;
			_270B(ܤ);
			if (!__29CD || !node.HasChildNodes)
			{
				ܤ._0749 += node.BoundsRelative.Width + base._070F.NodeHorizontalSpacing;
			}
			if (flag)
			{
				rectangle.Width = Math.Max(rectangle.Width, Math.Max(node.BoundsRelative.Width, (node.Expanded && node.ChildNodesBounds.Width > 0) ? (node.ChildNodesBounds.Right - rectangle.X) : 0));
				rectangle.Height += node.BoundsRelative.Height + (node.Expanded ? (node.ChildNodesBounds.Height + node.ColumnHeaderHeight) : 0) + _270D;
			}
			else
			{
				rectangle = Rectangle.Union(rectangle, node.BoundsRelative);
				rectangle.Height += _270D;
			}
		}
		return rectangle;
	}

	private void _270B(_2710 ܤ)
	{
		Node _2712 = ܤ._2712;
		if (!_2712.Visible || _2712.Cells.Count == 0)
		{
			return;
		}
		if (_2712.SizeChanged || (__29CD && _2712.HasChildNodes))
		{
			LayoutNode(ܤ);
		}
		if (_2712.FullRowBackground)
		{
			ܤ.ԥ.Add(_2712);
		}
		if (__29CD && _2712.HasChildNodes)
		{
			if (ܤ._271F > 0)
			{
				ܤ._074A += ܤ._271F + base._070F.NodeVerticalSpacing;
			}
			ܤ._271F = 0;
		}
		else if (ܤ._0749 + _2712.BoundsRelative.Width > base.ܒ.Right)
		{
			ܤ._0749 = ܤ._2720;
			ܤ._074A += ܤ._271F + base._070F.NodeVerticalSpacing;
			ܤ._271F = 0;
		}
		ܤ._271F = Math.Max(ܤ._271F, _2712.BoundsRelative.Height);
		if (_2712.BoundsRelative.X != ܤ._0749 || _2712.BoundsRelative.Y != ܤ._074A)
		{
			_2712.SetBounds(new Rectangle(ܤ._0749, ܤ._074A, _2712.BoundsRelative.Width, _2712.BoundsRelative.Height));
		}
		if (__29CD && _2712.HasChildNodes)
		{
			if (ܤ._271F > 0)
			{
				ܤ._074A += ܤ._271F + base._070F.NodeVerticalSpacing;
			}
			ܤ._271F = 0;
		}
		int nodeVerticalSpacing = base._070F.NodeVerticalSpacing;
		if (__29CD && _2712.HasChildNodes && _2712.Expanded)
		{
			int num = ܤ._0749;
			int _2720 = ܤ._2720;
			_ = ܤ._074A;
			ܤ._0749 += base.ܖ + _2712.NodesIndent;
			ܤ._2720 = ܤ._0749;
			_2728 _2715 = ܤ._2715;
			_2728 nodeColumnInfo = GetNodeColumnInfo(_2712);
			Rectangle childNodesBounds = _270C(ܤ, _2712, nodeVerticalSpacing, nodeColumnInfo);
			_2712.ChildNodesBounds = childNodesBounds;
			ܤ._2715 = _2715;
			ܤ._2712 = _2712;
			ܤ._0749 = num;
			ܤ._2720 = _2720;
		}
		else
		{
			_2712.ChildNodesBounds = Rectangle.Empty;
		}
	}

	private bool _270F(Node _0653)
	{
		return true;
	}

	protected override _2728 GetNodeColumnInfo(Node node)
	{
		return null;
	}

	protected override bool HasExpandPart(_2710 layoutInfo)
	{
		Node _2712 = layoutInfo._2712;
		if (_2712.ExpandVisibility == eNodeExpandVisibility.Auto)
		{
			if (!__29CD || !_175C._1776(_2712))
			{
				return false;
			}
			return true;
		}
		return _2712.ExpandVisibility == eNodeExpandVisibility.Visible;
	}

	protected internal override ܥ GetCellLayout()
	{
		if (__29CE == null)
		{
			__29CE = new _29CF(base._070F);
		}
		return __29CE;
	}
}
internal class _29CF : ܥ
{
	public _29CF(_070F layoutSettings)
		: base(layoutSettings)
	{
	}

	public override Size LayoutCells(_2710 layoutInfo, int x, int y)
	{
		eCellLayout cellLayout = layoutInfo.ܥ;
		if (layoutInfo._2712.CellLayout != layoutInfo.ܥ && layoutInfo._2712.CellLayout != 0)
		{
			cellLayout = layoutInfo._2712.CellLayout;
		}
		if (cellLayout == eCellLayout.Default && (!layoutInfo._2712.HasChildNodes || !layoutInfo._2722) && layoutInfo._2712.ImageAlignment == eCellPartAlignment.Default)
		{
			return _29D0(layoutInfo, x, y);
		}
		return base.LayoutCells(layoutInfo, x, y);
	}

	private Size _29D0(_2710 ܤ, int و, int ى)
	{
		Node _2712 = ܤ._2712;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		Size _2721 = ܤ._2721;
		int num4 = 0;
		int count = _2712.Cells.Count;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			Cell cell = _2712.Cells[i];
			bool flag2 = !flag;
			_0744 layoutCellInfo = GetLayoutCellInfo();
			layoutCellInfo._074A = ى;
			layoutCellInfo._0749 = و;
			layoutCellInfo._0747 = _2721.Width;
			layoutCellInfo._0746 = cell;
			layoutCellInfo.ܓ = ܤ.ܓ;
			layoutCellInfo._074C = ܤ._074C;
			layoutCellInfo._0745 = ܤ._2711;
			layoutCellInfo.ݎ = ܤ.ݎ;
			layoutCellInfo.ݏ = i;
			if (cell.Layout != 0)
			{
				layoutCellInfo.ݍ = cell.Layout == eCellPartLayout.Vertical;
			}
			else if (ܤ._271A != 0)
			{
				layoutCellInfo.ݍ = ܤ._271A == eCellPartLayout.Vertical;
			}
			if (cell.StyleNormal != null)
			{
				layoutCellInfo._074B = cell.StyleNormal;
			}
			else
			{
				layoutCellInfo._074B = ܤ._2716;
			}
			LayoutSingleTileCell(layoutCellInfo);
			if (flag2 && ى + cell.BoundsRelative.Height > _2721.Height && i > 0)
			{
				flag = true;
				flag2 = false;
			}
			cell.SetVisible(flag2);
			if (!flag2)
			{
				continue;
			}
			num4++;
			ى += cell.BoundsRelative.Height;
			num += cell.BoundsRelative.Height;
			if (cell.BoundsRelative.Height > 0)
			{
				ى += base.ܩ;
				num += base.ܩ;
			}
			if (cell.BoundsRelative.Width > num2)
			{
				num2 = cell.BoundsRelative.Width;
			}
			if (i == 0)
			{
				num3 += cell.BoundsRelative.Height;
				if (cell.BoundsRelative.Height > 0)
				{
					num3 += base.ܩ;
				}
			}
			else
			{
				num3 = Math.Max(num3, num);
			}
			if (i == 0 && !cell.Images._26EE.IsEmpty && count > 1)
			{
				Size size = Dpi.ImageSize(cell.Images._26EE);
				و = ((!cell.TextContentBounds.IsEmpty) ? (و + (cell.TextContentBounds.X - و)) : (و + (size.Width + base.ܨ)));
				_2721.Width = cell.TextContentBounds.Width;
				num -= cell.BoundsRelative.Height;
				num += cell.TextContentBounds.Height;
				ى -= cell.BoundsRelative.Height;
				ى += cell.TextContentBounds.Height;
			}
		}
		ى -= base.ܩ;
		num -= base.ܩ;
		num3 -= base.ܩ;
		if (_2712.Cells[0].BoundsRelative.Height > num && !_2712.Cells[0].Images._26EE.IsEmpty)
		{
			int num5 = (num3 - num) / num4 / 2;
			if (num5 > 0)
			{
				foreach (Cell cell2 in _2712.Cells)
				{
					if (cell2.IsVisible)
					{
						cell2.TextContentBounds = new Rectangle(cell2.TextContentBounds.X, cell2.TextContentBounds.Y + num5, cell2.TextContentBounds.Width, cell2.TextContentBounds.Height);
					}
				}
			}
		}
		else if (num4 == 1)
		{
			Rectangle textContentBounds = _2712.Cells[0].TextContentBounds;
			_2712.Cells[0].TextContentBounds = new Rectangle(textContentBounds.X, textContentBounds.Y, textContentBounds.Width, _2712.Cells[0].BoundsRelative.Height);
		}
		if (num2 < ܤ._2721.Width)
		{
			num2 = ܤ._2721.Width;
		}
		if (num3 < ܤ._2721.Height)
		{
			num3 = ܤ._2721.Height;
		}
		return new Size(num2, num3);
	}

	protected virtual void LayoutSingleTileCell(_0744 info)
	{
		Size size = Size.Empty;
		Font val = info._0745;
		int num = info.ݑ;
		int num2 = 0;
		if (info._074B.Font != null)
		{
			val = info._074B.Font;
			num = val.Height;
		}
		info._0746.OnLayoutCell();
		if (info._0746.Images._26EE.IsEmpty && HasImage(info._0746))
		{
			info._0746.Images._2701();
		}
		Size ܯ = Dpi.ImageSize(info._0746.Images._26EE);
		if (info._0746.HostedControl != null)
		{
			Size size2 = info._0746.HostedControl.Size;
			if (!info._0746.HostedControlSize.IsEmpty)
			{
				size2 = info._0746.HostedControlSize;
			}
			if (info._0747 == 0)
			{
				size = new Size(size2.Width, size2.Height);
			}
			else
			{
				int width = info._0747 - ᝈ.ᝍ(info._074B);
				size = new Size(width, size2.Height);
			}
		}
		else if (info._0746.HostedItem != null)
		{
			if (info._0747 != 0)
			{
				info._0746.HostedItem.WidthInternal = info._0747;
			}
			info._0746.HostedItem.RecalcSize();
			Size size3 = info._0746.HostedItem.Size;
			if (info._0747 == 0)
			{
				size = new Size(size3.Width, size3.Height);
			}
			else
			{
				int num3 = info._0747 - ᝈ.ᝍ(info._074B);
				size = new Size(num3, size3.Height);
				info._0746.HostedItem.WidthInternal = num3;
			}
		}
		else if (info._0747 == 0)
		{
			if (info._0746.TextMarkupBody == null)
			{
				string displayText = info._0746.DisplayText;
				if (displayText != "")
				{
					size = ((info._074B.WordWrap && info._074B.MaximumWidth > 0) ? TextDrawing.MeasureString(info.ܓ, displayText, val, info._074B.MaximumWidth) : ((info._0746.Parent == null || info._0746.Parent.Style == null || !info._0746.Parent.Style.WordWrap || info._0746.Parent.Style.MaximumWidth <= 0) ? TextDrawing.MeasureString(info.ܓ, displayText, val, 0, eTextFormat.LeftAndRightPadding | eTextFormat.NoPrefix) : TextDrawing.MeasureString(info.ܓ, displayText, val, info._0746.Parent.Style.MaximumWidth)));
					if (!BarFunctions.IsVista && BarUtilities.UseTextRenderer)
					{
						size.Width += 4;
					}
				}
				else if (ܯ.IsEmpty && !info._0746.CheckBoxVisible)
				{
					size = new Size(5, num);
				}
			}
			else
			{
				Size availableSize = new Size(1600, 1);
				if (info._074B.WordWrap && info._074B.MaximumWidth > 0)
				{
					availableSize.Width = info._074B.MaximumWidth;
				}
				else if (info._0746.Parent != null && info._0746.Parent.Style != null && info._0746.Parent.Style.WordWrap && info._0746.Parent.Style.MaximumWidth > 0)
				{
					availableSize.Width = info._0746.Parent.Style.MaximumWidth;
				}
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(info.ܓ, val, Color.Empty, rightToLeft: false);
				info._0746.TextMarkupBody.Measure(availableSize, markupDrawContext);
				availableSize = info._0746.TextMarkupBody.Bounds.Size;
				markupDrawContext.RightToLeft = !info._074C;
				info._0746.TextMarkupBody._0CC6(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
				size = info._0746.TextMarkupBody.Bounds.Size;
			}
		}
		else
		{
			int num4 = info._0747 - ᝈ.ᝍ(info._074B);
			num4 -= ܯ.Width + ((ܯ.Width > 0) ? (base.ܨ * 2) : 0);
			if (info._0746.CheckBoxVisible)
			{
				num4 -= base.ܦ.Width + base.ܨ * 2;
			}
			int height = num;
			if (info._074B.WordWrap || info._0746.TextMarkupBody != null)
			{
				height = info._074B.MaximumHeight - info._074B.MarginTop - info._074B.MarginBottom - info._074B.PaddingTop - info._074B.PaddingBottom;
				if (info._0746.TextMarkupBody == null)
				{
					if (num4 > 0)
					{
						size = ((height <= 0) ? TextDrawing.MeasureString(info.ܓ, info._0746.DisplayText, val, num4, info._074B.ݽ) : TextDrawing.MeasureString(info.ܓ, info._0746.DisplayText, val, new Size(num4, height), info._074B.ݽ));
					}
				}
				else
				{
					Size availableSize2 = new Size(num4, 1);
					MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(info.ܓ, val, Color.Empty, rightToLeft: false);
					info._0746.TextMarkupBody.Measure(availableSize2, markupDrawContext2);
					availableSize2 = info._0746.TextMarkupBody.Bounds.Size;
					availableSize2.Width = num4;
					markupDrawContext2.RightToLeft = !info._074C;
					info._0746.TextMarkupBody._0CC6(new Rectangle(0, 0, availableSize2.Width, availableSize2.Height), markupDrawContext2);
					size = info._0746.TextMarkupBody.Bounds.Size;
				}
			}
			else
			{
				size = new Size(num4, height);
			}
		}
		if (info._074B.WordWrap)
		{
			info._0746.WordWrap = true;
		}
		else
		{
			info._0746.WordWrap = false;
		}
		num2 = (int)Math.Max(num2, Math.Ceiling((double)size.Height));
		if (info.ݍ)
		{
			if (ܯ.Height > 0)
			{
				num2 += ܯ.Height + base.ܨ;
			}
			if (info._0746.CheckBoxVisible)
			{
				num2 += base.ܦ.Height + base.ܧ;
			}
		}
		else
		{
			if (ܯ.Height > num2)
			{
				num2 = ܯ.Height;
			}
			if (info._0746.CheckBoxVisible && base.ܦ.Height > num2)
			{
				num2 = base.ܦ.Height;
			}
		}
		Rectangle _0730 = new Rectangle(info._0749 + ᝈ.ᝐ(info._074B), info._074A + ᝈ._1753(info._074B), info._0747, num2);
		if (_0730.Width == 0)
		{
			if (info.ݍ)
			{
				_0730.Width = (int)Math.Ceiling((double)size.Width);
				if (ܯ.Width > _0730.Width)
				{
					_0730.Width = ܯ.Width + base.ܨ;
				}
				if (info._0746.CheckBoxVisible && base.ܦ.Width > _0730.Width)
				{
					_0730.Width += base.ܦ.Width + base.ܨ;
				}
			}
			else
			{
				_0730.Width = (int)Math.Ceiling((double)size.Width);
				if (ܯ.Width > 0)
				{
					_0730.Width += ܯ.Width + base.ܨ;
				}
				if (info._0746.CheckBoxVisible)
				{
					_0730.Width += base.ܦ.Width + base.ܨ;
				}
			}
		}
		Rectangle bounds = new Rectangle(info._0749, info._074A, info._0747, _0730.Height + info._074B.MarginTop + info._074B.MarginBottom + info._074B.PaddingTop + info._074B.PaddingBottom);
		if (bounds.Width == 0)
		{
			bounds.Width = _0730.Width + ᝈ.ᝍ(info._074B);
		}
		info._0746.SetBounds(bounds);
		if (info._0746.CheckBoxVisible && bounds.Width >= base.ܦ.Width)
		{
			eVerticalAlign eVerticalAlign = ܥ._073B(info._0746.CheckBoxAlignment, info.ݎ);
			eHorizontalAlign eHorizontalAlign = ܥ._073A(info._0746.CheckBoxAlignment, info._074C, info.ݎ);
			if (eHorizontalAlign == eHorizontalAlign.Center && (!string.IsNullOrEmpty(info._0746.Text) || !ܯ.IsEmpty))
			{
				eHorizontalAlign = eHorizontalAlign.Left;
			}
			if (info.ݍ)
			{
				info._0746.SetCheckBoxBounds(_0734(base.ܦ, ref _0730, eHorizontalAlign, eVerticalAlign, base.ܨ));
			}
			else
			{
				info._0746.SetCheckBoxBounds(ܮ(base.ܦ, ref _0730, eHorizontalAlign, eVerticalAlign, base.ܨ));
			}
		}
		else
		{
			info._0746.SetCheckBoxBounds(Rectangle.Empty);
		}
		if (!ܯ.IsEmpty && bounds.Width >= ܯ.Width)
		{
			eVerticalAlign eVerticalAlign2 = ܥ._0739(info._0746.ImageAlignment, info.ݎ);
			eHorizontalAlign eHorizontalAlign2 = ܥ._0735(info._0746.ImageAlignment, info._074C, info.ݎ);
			if (eHorizontalAlign2 == eHorizontalAlign.Center && (!string.IsNullOrEmpty(info._0746.Text) || info._0746.CheckBoxVisible))
			{
				eHorizontalAlign2 = eHorizontalAlign.Left;
			}
			if (info.ݍ)
			{
				info._0746.SetImageBounds(_0734(ܯ, ref _0730, eHorizontalAlign2, eVerticalAlign2, base.ܨ));
			}
			else
			{
				info._0746.SetImageBounds(ܮ(ܯ, ref _0730, eHorizontalAlign2, eVerticalAlign2, base.ܨ));
			}
		}
		else
		{
			info._0746.SetImageBounds(Rectangle.Empty);
		}
		if (!size.IsEmpty)
		{
			if (info._0747 > 0)
			{
				_0730.Width -= 2;
			}
			if (info.ݎ == eView.Tile && info.ݏ == 0)
			{
				info._0746.TextContentBounds = new Rectangle(_0730.X, _0730.Y, size.Width, size.Height + 1);
			}
			else
			{
				info._0746.TextContentBounds = _0730;
			}
		}
		else
		{
			info._0746.TextContentBounds = Rectangle.Empty;
		}
	}
}
