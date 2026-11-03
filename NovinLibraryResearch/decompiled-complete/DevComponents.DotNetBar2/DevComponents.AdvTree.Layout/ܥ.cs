using System;
using System.Drawing;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.AdvTree.Layout;

internal class ܥ
{
	private _070F __05A2;

	private Size _ܫ = new Size(12, 12);

	private _0744 ܬ;

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

	internal Size ܦ
	{
		get
		{
			return _ܫ;
		}
		set
		{
			_ܫ = value;
		}
	}

	protected int ܧ => 4;

	protected int ܨ => 4;

	public int ܩ => 1;

	public int ܪ => 1;

	public ܥ(_070F layoutSettings)
	{
		__05A2 = layoutSettings;
	}

	protected void ܭ(Cell ڂ, int و, int ى)
	{
		if (و != 0 || ى != 0)
		{
			ڂ.SetBounds(new Rectangle(ڂ.BoundsRelative.X + و, ڂ.BoundsRelative.Y + ى, ڂ.BoundsRelative.Width, ڂ.BoundsRelative.Height));
			if (!ڂ.CheckBoxBoundsRelative.IsEmpty)
			{
				ڂ.SetCheckBoxBounds(new Rectangle(ڂ.CheckBoxBoundsRelative.X + و, ڂ.CheckBoxBoundsRelative.Y + ى, ڂ.CheckBoxBoundsRelative.Width, ڂ.CheckBoxBoundsRelative.Height));
			}
			if (!ڂ.ImageBoundsRelative.IsEmpty)
			{
				ڂ.SetImageBounds(new Rectangle(ڂ.ImageBoundsRelative.X + و, ڂ.ImageBoundsRelative.Y + ى, ڂ.ImageBoundsRelative.Width, ڂ.ImageBoundsRelative.Height));
			}
			if (!ڂ.TextContentBounds.IsEmpty)
			{
				ڂ.TextContentBounds = new Rectangle(ڂ.TextContentBounds.X + و, ڂ.TextContentBounds.Y + ى, ڂ.TextContentBounds.Width, ڂ.TextContentBounds.Height);
			}
		}
	}

	protected virtual void LayoutSingleCell(_0744 info)
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
		Size ܯ2 = Dpi.Size(ܦ);
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
		else
		{
			string displayText = info._0746.DisplayText;
			if (info._0747 == 0)
			{
				if (info._0746.TextMarkupBody == null)
				{
					string text = displayText;
					if (text != "")
					{
						size = ((info._074B.WordWrap && info._074B.MaximumWidth > 0) ? TextDrawing.MeasureString(info.ܓ, text, val, info._074B.MaximumWidth) : ((info._0746.Parent == null || info._0746.Parent.Style == null || !info._0746.Parent.Style.WordWrap || info._0746.Parent.Style.MaximumWidth <= 0) ? TextDrawing.MeasureString(info.ܓ, text, val, 0, eTextFormat.LeftAndRightPadding | eTextFormat.NoPrefix) : TextDrawing.MeasureString(info.ܓ, text, val, info._0746.Parent.Style.MaximumWidth)));
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
				num4 -= ܯ.Width + ((ܯ.Width > 0) ? (ܨ * 2) : 0);
				if (info._0746.CheckBoxVisible)
				{
					num4 -= ܯ2.Width + ܨ * 2;
				}
				int height = num;
				if (info._074B.WordWrap || info._0746.TextMarkupBody != null)
				{
					height = info._074B.MaximumHeight - info._074B.MarginTop - info._074B.MarginBottom - info._074B.PaddingTop - info._074B.PaddingBottom;
					if (info._0746.TextMarkupBody == null)
					{
						if (num4 > 0)
						{
							if (height <= 0)
							{
								size = (string.IsNullOrEmpty(displayText) ? new Size(num4, num) : TextDrawing.MeasureString(info.ܓ, displayText, val, num4, info._074B.ݽ));
							}
							else
							{
								size = TextDrawing.MeasureString(info.ܓ, displayText, val, new Size(num4, height), info._074B.ݽ);
								if (size.Height == 0)
								{
									size.Height = height;
								}
							}
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
				num2 += ܯ.Height + ܨ;
			}
			if (info._0746.CheckBoxVisible)
			{
				num2 += ܯ2.Height + ܧ;
			}
		}
		else
		{
			if (ܯ.Height > num2)
			{
				num2 = ܯ.Height;
			}
			if (info._0746.CheckBoxVisible && ܯ2.Height > num2)
			{
				num2 = ܯ2.Height;
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
					_0730.Width = ܯ.Width + ܨ;
				}
				if (info._0746.CheckBoxVisible && ܯ2.Width > _0730.Width)
				{
					_0730.Width += ܯ2.Width + ܨ;
				}
			}
			else
			{
				_0730.Width = (int)Math.Ceiling((double)size.Width);
				if (ܯ.Width > 0)
				{
					_0730.Width += ܯ.Width + ܨ;
				}
				if (info._0746.CheckBoxVisible)
				{
					_0730.Width += ܯ2.Width + ܨ;
				}
			}
		}
		Rectangle bounds = new Rectangle(info._0749, info._074A, info._0747, _0730.Height + info._074B.MarginTop + info._074B.MarginBottom + info._074B.PaddingTop + info._074B.PaddingBottom);
		if (bounds.Width == 0)
		{
			bounds.Width = _0730.Width + ᝈ.ᝍ(info._074B);
		}
		info._0746.SetBounds(bounds);
		if (info._0746.CheckBoxVisible && bounds.Width >= ܯ2.Width)
		{
			eVerticalAlign eVerticalAlign = _073B(info._0746.CheckBoxAlignment, info.ݎ);
			eHorizontalAlign eHorizontalAlign = _073A(info._0746.CheckBoxAlignment, info._074C, info.ݎ);
			if (eHorizontalAlign == eHorizontalAlign.Center && (!string.IsNullOrEmpty(info._0746.Text) || !ܯ.IsEmpty))
			{
				eHorizontalAlign = eHorizontalAlign.Left;
			}
			if (info.ݍ)
			{
				info._0746.SetCheckBoxBounds(_0734(ܯ2, ref _0730, eHorizontalAlign, eVerticalAlign, ܨ));
			}
			else
			{
				info._0746.SetCheckBoxBounds(ܮ(ܯ2, ref _0730, eHorizontalAlign, eVerticalAlign, ܨ));
			}
		}
		else
		{
			info._0746.SetCheckBoxBounds(Rectangle.Empty);
		}
		if (!ܯ.IsEmpty && bounds.Width >= ܯ.Width)
		{
			eVerticalAlign eVerticalAlign2 = _0739(info._0746.ImageAlignment, info.ݎ);
			eHorizontalAlign eHorizontalAlign2 = _0735(info._0746.ImageAlignment, info._074C, info.ݎ);
			if (eHorizontalAlign2 == eHorizontalAlign.Center && (!string.IsNullOrEmpty(info._0746.Text) || info._0746.CheckBoxVisible) && !info.ݍ)
			{
				eHorizontalAlign2 = eHorizontalAlign.Left;
			}
			if (info.ݍ)
			{
				info._0746.SetImageBounds(_0734(ܯ, ref _0730, eHorizontalAlign2, eVerticalAlign2, ܨ));
			}
			else
			{
				info._0746.SetImageBounds(ܮ(ܯ, ref _0730, eHorizontalAlign2, eVerticalAlign2, ܨ));
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
				_0730.Width -= 2 + ᝈ.ᝍ(info._074B);
			}
			info._0746.TextContentBounds = _0730;
		}
		else
		{
			info._0746.TextContentBounds = Rectangle.Empty;
		}
	}

	protected Rectangle ܮ(Size ܯ, ref Rectangle _0730, eHorizontalAlign _0731, eVerticalAlign _0732, int _0733)
	{
		Rectangle result = new Rectangle(Point.Empty, ܯ);
		switch (_0731)
		{
		case eHorizontalAlign.Right:
			result.X = _0730.Right - result.Width;
			_0730.Width -= result.Width + _0733;
			break;
		case eHorizontalAlign.Center:
			result.X = _0730.X + (_0730.Width - result.Width) / 2;
			break;
		default:
			result.X = _0730.X;
			_0730.X = result.Right + _0733;
			_0730.Width -= result.Width + _0733;
			break;
		}
		switch (_0732)
		{
		case eVerticalAlign.Top:
			result.Y = _0730.Y;
			break;
		case eVerticalAlign.Middle:
			result.Y = _0730.Y + (_0730.Height - result.Height) / 2;
			break;
		case eVerticalAlign.Bottom:
			result.Y = _0730.Bottom - result.Height;
			break;
		}
		return result;
	}

	protected Rectangle _0734(Size ܯ, ref Rectangle _0730, eHorizontalAlign _0731, eVerticalAlign _0732, int _0733)
	{
		Rectangle result = new Rectangle(Point.Empty, ܯ);
		switch (_0731)
		{
		case eHorizontalAlign.Left:
			result.X = _0730.X;
			break;
		case eHorizontalAlign.Right:
			result.X = _0730.Right - result.Width;
			break;
		case eHorizontalAlign.Center:
			result.X = _0730.X + (_0730.Width - result.Width) / 2;
			break;
		}
		if (_0732 == eVerticalAlign.Bottom)
		{
			result.Y = _0730.Bottom - result.Height;
			_0730.Height -= result.Height + _0733;
		}
		else
		{
			result.Y = _0730.Y;
			_0730.Y = result.Bottom + _0733;
			_0730.Height -= result.Height + _0733;
		}
		return result;
	}

	public static eHorizontalAlign _0735(eCellPartAlignment _0736, bool _0737, eView _0738)
	{
		if (_0736 == eCellPartAlignment.Default)
		{
			switch (_0738)
			{
			case eView.Tree:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			case eView.Tile:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			}
		}
		if (((_0736 == eCellPartAlignment.NearBottom || _0736 == eCellPartAlignment.NearCenter || _0736 == eCellPartAlignment.NearTop) && _0737) || ((_0736 == eCellPartAlignment.FarBottom || _0736 == eCellPartAlignment.FarCenter || _0736 == eCellPartAlignment.FarTop) && !_0737))
		{
			return eHorizontalAlign.Left;
		}
		if (_0736 == eCellPartAlignment.CenterBottom || _0736 == eCellPartAlignment.CenterTop)
		{
			return eHorizontalAlign.Center;
		}
		return eHorizontalAlign.Right;
	}

	public static eVerticalAlign _0739(eCellPartAlignment _0736, eView _0738)
	{
		if (_0736 == eCellPartAlignment.Default)
		{
			switch (_0738)
			{
			case eView.Tree:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			case eView.Tile:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			}
		}
		eVerticalAlign result = eVerticalAlign.Middle;
		switch (_0736)
		{
		case eCellPartAlignment.NearBottom:
		case eCellPartAlignment.CenterBottom:
		case eCellPartAlignment.FarBottom:
			result = eVerticalAlign.Bottom;
			break;
		case eCellPartAlignment.NearTop:
		case eCellPartAlignment.CenterTop:
		case eCellPartAlignment.FarTop:
			result = eVerticalAlign.Top;
			break;
		}
		return result;
	}

	public static eHorizontalAlign _073A(eCellPartAlignment _0736, bool _0737, eView _0738)
	{
		if (_0736 == eCellPartAlignment.Default)
		{
			switch (_0738)
			{
			case eView.Tree:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			case eView.Tile:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			}
		}
		if (((_0736 == eCellPartAlignment.NearBottom || _0736 == eCellPartAlignment.NearCenter || _0736 == eCellPartAlignment.NearTop) && _0737) || ((_0736 == eCellPartAlignment.FarBottom || _0736 == eCellPartAlignment.FarCenter || _0736 == eCellPartAlignment.FarTop) && !_0737))
		{
			return eHorizontalAlign.Left;
		}
		if (_0736 == eCellPartAlignment.CenterBottom || _0736 == eCellPartAlignment.CenterTop)
		{
			return eHorizontalAlign.Center;
		}
		return eHorizontalAlign.Right;
	}

	public static eVerticalAlign _073B(eCellPartAlignment _0736, eView _0738)
	{
		if (_0736 == eCellPartAlignment.Default)
		{
			switch (_0738)
			{
			case eView.Tree:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			case eView.Tile:
				_0736 = eCellPartAlignment.NearCenter;
				break;
			}
		}
		eVerticalAlign result = eVerticalAlign.Middle;
		switch (_0736)
		{
		case eCellPartAlignment.NearBottom:
		case eCellPartAlignment.CenterBottom:
		case eCellPartAlignment.FarBottom:
			result = eVerticalAlign.Bottom;
			break;
		case eCellPartAlignment.NearTop:
		case eCellPartAlignment.CenterTop:
		case eCellPartAlignment.FarTop:
			result = eVerticalAlign.Top;
			break;
		}
		return result;
	}

	internal void ResetCheckBoxSize()
	{
		_ܫ = new Size(12, 12);
	}

	public virtual Size LayoutCells(_2710 layoutInfo, int x, int y)
	{
		eCellLayout cellLayout = layoutInfo.ܥ;
		if (layoutInfo._2712.CellLayout != layoutInfo.ܥ && layoutInfo._2712.CellLayout != 0)
		{
			cellLayout = layoutInfo._2712.CellLayout;
		}
		if (cellLayout == eCellLayout.Horizontal || (cellLayout == eCellLayout.Default && layoutInfo.ݎ == eView.Tree) || (layoutInfo.ݎ == eView.Tile && layoutInfo._2722 && layoutInfo._2712.HasChildNodes))
		{
			return LayoutCellsHorizontal(layoutInfo, x, y);
		}
		return LayoutCellsVertical(layoutInfo, x, y);
	}

	protected virtual Size LayoutCellsHorizontal(_2710 layoutInfo, int x, int y)
	{
		Node _2712 = layoutInfo._2712;
		int num = 0;
		int num2 = 0;
		bool flag = false;
		int count = _2712.Cells.Count;
		bool flag2 = true;
		int num3 = 0;
		for (int i = 0; i < count; i++)
		{
			Cell cell = null;
			cell = ((layoutInfo._2715 != null && layoutInfo._2715._2729.Count == count) ? _2712.Cells[layoutInfo._2715._2729[i]._272E] : ((layoutInfo._2714._2729.Count != count) ? _2712.Cells[i] : _2712.Cells[layoutInfo._2714._2729[i]._272E]));
			bool flag3 = true;
			_0744 layoutCellInfo = GetLayoutCellInfo();
			layoutCellInfo.ݎ = layoutInfo.ݎ;
			layoutCellInfo._074A = y;
			layoutCellInfo._0749 = x;
			layoutCellInfo._0747 = 0;
			layoutCellInfo._0748 = 0;
			layoutCellInfo._0746 = cell;
			layoutCellInfo.ܓ = layoutInfo.ܓ;
			layoutCellInfo._074C = layoutInfo._074C;
			layoutCellInfo._0745 = layoutInfo._2711;
			if (cell.Layout != 0)
			{
				layoutCellInfo.ݍ = cell.Layout == eCellPartLayout.Vertical;
			}
			else if (layoutInfo._271A != 0)
			{
				layoutCellInfo.ݍ = layoutInfo._271A == eCellPartLayout.Vertical;
			}
			_2729 _2729 = null;
			if ((layoutInfo._2714._2729.Count > 0 || (layoutInfo._2715 != null && layoutInfo._2715._2729.Count > 0)) && (_2712.Cells.Count > 1 || _2712.Cells.Count == layoutInfo._2714._2729.Count || (layoutInfo._2715 != null && layoutInfo._2715._2729.Count == _2712.Cells.Count)))
			{
				bool flag4 = false;
				if (layoutInfo._2715 != null && layoutInfo._2715._2729.Count > 0 && i < layoutInfo._2715._2729.Count)
				{
					_2729 = layoutInfo._2715._2729[i];
				}
				else if (i < layoutInfo._2714._2729.Count)
				{
					_2729 = layoutInfo._2714._2729[i];
					flag4 = true;
				}
				if (_2729 != null)
				{
					flag3 = _2729.ᆒ;
					if (_2729._272D && cell.HostedControl == null)
					{
						layoutCellInfo._0747 = 0;
					}
					else
					{
						layoutCellInfo._0747 = _2729.ܐ;
						if (flag4 && layoutCellInfo._0747 > 0 && num3 < layoutInfo._0749 + x)
						{
							layoutCellInfo._0747 = Math.Max(-1, layoutCellInfo._0747 - (layoutInfo._0749 + x - layoutInfo._2713 - num3));
							if (layoutCellInfo._0747 == 0)
							{
								layoutCellInfo._0747 = -1;
							}
						}
						num3 += _2729.ܐ + _070F._073C;
					}
				}
			}
			else if (layoutInfo.ݎ == eView.Tile)
			{
				if (layoutInfo._2722 && _2712.HasChildNodes)
				{
					layoutCellInfo._0747 = layoutInfo.ᶮ.Width - layoutInfo._0749 - layoutInfo._271E - 1;
				}
				else
				{
					layoutCellInfo._0747 = layoutInfo._2721.Width;
					layoutCellInfo._0748 = layoutInfo._2721.Height;
				}
			}
			if (cell.StyleNormal != null)
			{
				layoutCellInfo._074B = cell.StyleNormal;
			}
			else if (layoutInfo._2712.Style != null)
			{
				ElementStyle elementStyle = layoutInfo._2716.Copy();
				elementStyle.ApplyStyle(layoutInfo._2712.Style);
				layoutCellInfo._074B = elementStyle;
			}
			else
			{
				layoutCellInfo._074B = layoutInfo._2716;
			}
			LayoutSingleCell(layoutCellInfo);
			cell.SetVisible(flag3);
			if (!flag3)
			{
				continue;
			}
			if (_2729 != null && _2729._272D && cell.HostedControl == null)
			{
				if (_2729._272C.Parent.ParentNode == null && flag2)
				{
					_2729._272B = Math.Max(_2729._272B, cell.BoundsRelative.Width + (layoutInfo._0749 + x) + _070F._073C);
				}
				else
				{
					_2729._272B = Math.Max(_2729._272B, cell.BoundsRelative.Width + _070F._073C);
				}
			}
			x += Math.Max(0, cell.BoundsRelative.Width);
			num2 += Math.Max(0, cell.BoundsRelative.Width);
			x += _070F._073C;
			num2 += _070F._073C;
			if (cell.BoundsRelative.Height > num)
			{
				if (num != 0)
				{
					flag = true;
				}
				num = cell.BoundsRelative.Height;
			}
			else if (!flag2 && cell.BoundsRelative.Height < num && !cell.TextContentBounds.IsEmpty)
			{
				flag = true;
			}
			flag2 = false;
		}
		if (flag)
		{
			for (int j = 0; j < _2712.Cells.Count; j++)
			{
				Cell cell2 = _2712.Cells[j];
				if (cell2.BoundsRelative.Height == num || cell2.TextContentBounds.IsEmpty)
				{
					continue;
				}
				cell2.TextContentBounds = new Rectangle(cell2.TextContentBounds.X, cell2.TextContentBounds.Y, cell2.TextContentBounds.Width, cell2.TextContentBounds.Height + (num - cell2.BoundsRelative.Height));
				int num4 = num - cell2.BoundsRelative.Height;
				if (!cell2.CheckBoxBoundsRelative.IsEmpty)
				{
					eVerticalAlign eVerticalAlign = _073B(cell2.CheckBoxAlignment, layoutInfo.ݎ);
					if (eVerticalAlign == eVerticalAlign.Middle)
					{
						cell2.SetCheckBoxBounds(new Rectangle(cell2.CheckBoxBoundsRelative.X, cell2.CheckBoxBoundsRelative.Y + (int)Math.Ceiling((double)num4 / 2.0), cell2.CheckBoxBoundsRelative.Width, cell2.CheckBoxBoundsRelative.Height));
					}
					if (eVerticalAlign == eVerticalAlign.Bottom)
					{
						cell2.SetCheckBoxBounds(new Rectangle(cell2.CheckBoxBoundsRelative.X, cell2.CheckBoxBoundsRelative.Y + num4, cell2.CheckBoxBoundsRelative.Width, cell2.CheckBoxBoundsRelative.Height));
					}
				}
				if (!cell2.ImageBoundsRelative.IsEmpty)
				{
					switch (_0739(cell2.ImageAlignment, layoutInfo.ݎ))
					{
					case eVerticalAlign.Middle:
						cell2.SetImageBounds(new Rectangle(cell2.ImageBoundsRelative.X, cell2.ImageBoundsRelative.Y + (int)Math.Ceiling((double)num4 / 2.0), cell2.ImageBoundsRelative.Width, cell2.ImageBoundsRelative.Height));
						break;
					case eVerticalAlign.Bottom:
						cell2.SetImageBounds(new Rectangle(cell2.ImageBoundsRelative.X, cell2.ImageBoundsRelative.Y + num4, cell2.ImageBoundsRelative.Width, cell2.ImageBoundsRelative.Height));
						break;
					}
				}
				cell2.SetBounds(new Rectangle(cell2.BoundsRelative.X, cell2.BoundsRelative.Y, cell2.BoundsRelative.Width, num));
			}
		}
		x -= _070F._073C;
		num2 -= _070F._073C;
		return new Size(num2, num);
	}

	protected virtual Size LayoutCellsVertical(_2710 layoutInfo, int x, int y)
	{
		Node _2712 = layoutInfo._2712;
		int num = 0;
		int num2 = 0;
		eHorizontalAlign verticalCellLayoutAlignment = _2712.VerticalCellLayoutAlignment;
		int num3 = 0;
		int count = _2712.Cells.Count;
		for (int i = 0; i < count; i++)
		{
			Cell cell = null;
			cell = ((layoutInfo._2715 != null && layoutInfo._2715._2729.Count == count) ? _2712.Cells[layoutInfo._2715._2729[i]._272E] : ((layoutInfo._2714._2729.Count != count) ? _2712.Cells[i] : _2712.Cells[layoutInfo._2714._2729[i]._272E]));
			bool flag = true;
			_0744 layoutCellInfo = GetLayoutCellInfo();
			layoutCellInfo._074A = y;
			layoutCellInfo._0749 = x;
			layoutCellInfo._0747 = 0;
			layoutCellInfo._0746 = cell;
			layoutCellInfo.ܓ = layoutInfo.ܓ;
			layoutCellInfo._074C = layoutInfo._074C;
			layoutCellInfo._0745 = layoutInfo._2711;
			if (cell.Layout != 0)
			{
				layoutCellInfo.ݍ = cell.Layout == eCellPartLayout.Vertical;
			}
			else if (layoutInfo._271A != 0)
			{
				layoutCellInfo.ݍ = layoutInfo._271A == eCellPartLayout.Vertical;
			}
			if (layoutInfo._2714._2729.Count > 0 || (layoutInfo._2715 != null && layoutInfo._2715._2729.Count > 0))
			{
				_2729 _2729 = null;
				_2729 = ((layoutInfo._2715 == null || layoutInfo._2715._2729.Count <= 0) ? layoutInfo._2714._2729[i] : layoutInfo._2715._2729[i]);
				flag = _2729.ᆒ;
				layoutCellInfo._0747 = _2729.ܐ;
			}
			else if (layoutInfo.ݎ == eView.Tile)
			{
				layoutCellInfo._0747 = layoutInfo._2721.Width;
				layoutCellInfo._0748 = layoutInfo._2721.Height;
			}
			if (cell.StyleNormal != null)
			{
				layoutCellInfo._074B = cell.StyleNormal;
			}
			else
			{
				layoutCellInfo._074B = layoutInfo._2716;
			}
			LayoutSingleCell(layoutCellInfo);
			cell.SetVisible(flag);
			if (flag)
			{
				num3++;
				y += cell.BoundsRelative.Height;
				num += cell.BoundsRelative.Height;
				if (cell.BoundsRelative.Height > 0)
				{
					y += ܩ;
					num += ܩ;
				}
				if (cell.BoundsRelative.Width > num2)
				{
					num2 = cell.BoundsRelative.Width;
				}
			}
		}
		y -= ܩ;
		num -= ܩ;
		if (verticalCellLayoutAlignment != 0 && num3 > 1)
		{
			foreach (Cell cell2 in _2712.Cells)
			{
				if (cell2.IsVisible)
				{
					if (verticalCellLayoutAlignment == eHorizontalAlign.Center)
					{
						ܭ(cell2, (num2 - cell2.BoundsRelative.Width) / 2, 0);
					}
					else
					{
						ܭ(cell2, num2 - cell2.BoundsRelative.Width, 0);
					}
				}
			}
		}
		return new Size(num2, num);
	}

	protected virtual _0744 GetLayoutCellInfo()
	{
		if (ܬ == null)
		{
			ܬ = new _0744();
		}
		return ܬ;
	}

	protected virtual bool HasImage(Cell cell)
	{
		if (cell.Images.Image != null || cell.Images.ImageIndex >= 0 || !string.IsNullOrEmpty(cell.Images.ImageKey))
		{
			return true;
		}
		return false;
	}
}
