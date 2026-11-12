using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabItemLayoutManager : BlockLayoutManager
{
	private int _SelLine;

	private Size _CloseButtonSize = new Size(16, 16);

	private Size _FixedTabSize = Size.Empty;

	private Size _TabLayoutOffset = Size.Empty;

	private SuperTabStripItem _TabStripItem;

	private Dictionary<BaseItem, Size> _List;

	public Size FixedTabSize
	{
		get
		{
			return _FixedTabSize;
		}
		set
		{
			_FixedTabSize = value;
		}
	}

	public Size TabLayoutOffset
	{
		get
		{
			return _TabLayoutOffset;
		}
		set
		{
			_TabLayoutOffset = value;
		}
	}

	internal SuperTabStripBaseDisplay TabDisplay => _TabStripItem.TabDisplay;

	public SuperTabItemLayoutManager()
	{
	}

	public SuperTabItemLayoutManager(SuperTabStripItem tabStripItem)
	{
		_TabStripItem = tabStripItem;
		_List = new Dictionary<BaseItem, Size>();
	}

	public override void Layout(IBlock block, Size availableSize)
	{
		if (block.Visible)
		{
			if (block is SuperTabItem)
			{
				LayoutTab(block as SuperTabItem);
			}
			else if (block is BaseItem)
			{
				LayoutBaseItem(block as BaseItem);
			}
		}
	}

	private void LayoutTab(SuperTabItem tab)
	{
		Size size = default(Size);
		Rectangle empty = Rectangle.Empty;
		if (_FixedTabSize.Width <= 0 || _FixedTabSize.Height <= 0 || tab.TextMarkupBody != null)
		{
			size = MeasureTab(tab);
		}
		if (tab.FixedTabSize.Width > 0)
		{
			size.Width = tab.FixedTabSize.Width;
		}
		else if (_FixedTabSize.Width > 0)
		{
			size.Width = _FixedTabSize.Width;
		}
		if (tab.FixedTabSize.Height > 0)
		{
			size.Height = tab.FixedTabSize.Height;
		}
		else if (_FixedTabSize.Height > 0)
		{
			size.Height = _FixedTabSize.Height;
		}
		Size minTabSize = TabDisplay.MinTabSize;
		if (size.Width < minTabSize.Width)
		{
			size.Width = minTabSize.Width;
		}
		if (size.Height < minTabSize.Height)
		{
			size.Height = minTabSize.Height;
		}
		empty.Size = (tab.IsVertical ? new Size(size.Height, size.Width) : size);
		empty.Size = _TabStripItem.OnMeasureTabItem(tab, empty.Size, base.Graphics);
		tab.Bounds = empty;
		tab.RecalcSize();
	}

	private Size MeasureTab(SuperTabItem tab)
	{
		Size result = new Size(0, 0);
		Font tabFont = tab.GetTabFont();
		Size size = ((tab.TextMarkupBody != null) ? MeasureMarkup(tab, base.Graphics, tabFont) : MeasureText(tab, base.Graphics, tabFont));
		size.Width += _TabLayoutOffset.Width;
		size.Height += _TabLayoutOffset.Height + 10;
		bool flag = tab.IsSelected || !_TabStripItem.DisplaySelectedTextOnly;
		if (!_TabStripItem.IsVertical || !_TabStripItem.HorizontalText)
		{
			if (flag)
			{
				result.Width += size.Width + _TabStripItem.TabHorizontalSpacing;
			}
			if (size.Height > result.Height)
			{
				result.Height = size.Height;
			}
			result.Width += TabDisplay.TabOverlap + TabDisplay.TabSpacing;
		}
		else
		{
			if (flag && size.Height > result.Height)
			{
				result.Height = size.Height;
			}
			result.Height += TabDisplay.TabOverlap + TabDisplay.TabSpacing;
			result.Width += size.Width + _TabStripItem.TabHorizontalSpacing + TabDisplay.SelectedPaddingWidth;
		}
		CompositeImage tabImage = tab.GetTabImage();
		if (tabImage != null)
		{
			int num = _TabStripItem.TabVerticalSpacing + tab.ImagePadding.Vertical;
			int num2 = _TabStripItem.TabHorizontalSpacing + tab.ImagePadding.Horizontal;
			switch (tab.EffectiveImageAlignment)
			{
			case ImageAlignment.TopLeft:
			case ImageAlignment.TopCenter:
			case ImageAlignment.TopRight:
			case ImageAlignment.BottomLeft:
			case ImageAlignment.BottomCenter:
			case ImageAlignment.BottomRight:
				result.Height += tabImage.Height + num;
				break;
			case ImageAlignment.MiddleLeft:
			case ImageAlignment.MiddleRight:
				result.Width += tabImage.Width + num2;
				if (result.Height < tabImage.Height + num)
				{
					result.Height = tabImage.Height + num;
				}
				break;
			case ImageAlignment.MiddleCenter:
				if (result.Height < tabImage.Height + num)
				{
					result.Height = tabImage.Height + num;
				}
				break;
			}
			int num3 = tabImage.Height + _TabStripItem.TabDisplay.TabOverlap + _TabStripItem.TabDisplay.TabSpacing + 4;
			if (result.Height < num3)
			{
				result.Height = num3;
			}
		}
		if (TabDisplay.CloseButtonOnTabs && tab.CloseButtonVisible)
		{
			result.Width += _CloseButtonSize.Width + _TabStripItem.TabHorizontalSpacing;
			if (result.Height < _CloseButtonSize.Height + _TabStripItem.TabVerticalSpacing)
			{
				result.Height = _CloseButtonSize.Height + _TabStripItem.TabVerticalSpacing;
			}
			if (result.Height < _CloseButtonSize.Height + 4)
			{
				result.Height = _CloseButtonSize.Height + 4;
			}
		}
		result.Width += _TabStripItem.TabHorizontalSpacing * 2;
		result.Height += _TabStripItem.TabVerticalSpacing * 2;
		return result;
	}

	private Size MeasureMarkup(SuperTabItem tab, Graphics g, Font font)
	{
		MarkupDrawContext markupDrawContext = new MarkupDrawContext(g, font, Color.Black, rightToLeft: false);
		Size availableSize = new Size(5000, 5000);
		tab.TextMarkupBody.InvalidateElementsSize();
		tab.TextMarkupBody.Measure(availableSize, markupDrawContext);
		markupDrawContext.RightToLeft = _TabStripItem.IsRightToLeft;
		tab.TextMarkupBody.Arrange(new Rectangle(Point.Empty, tab.TextMarkupBody.Bounds.Size), markupDrawContext);
		return tab.TextMarkupBody.Bounds.Size;
	}

	private Size MeasureText(SuperTabItem tab, Graphics g, Font font)
	{
		string text = (string.IsNullOrEmpty(tab.Text) ? "M" : tab.Text);
		return tab.IsVertical ? TextDrawing.MeasureStringLegacy(g, text, font, Size.Empty, eTextFormat.Default) : TextDrawing.MeasureString(g, text, font, 0, eTextFormat.Default);
	}

	private void LayoutBaseItem(BaseItem item)
	{
		Size size = default(Size);
		Rectangle empty = Rectangle.Empty;
		if (_FixedTabSize.Width <= 0 || _FixedTabSize.Height <= 0 || (item is ButtonItem && !((ButtonItem)item).FixedSize.IsEmpty))
		{
			item.RecalcSize();
			size = item.Bounds.Size;
			size.Width += _TabLayoutOffset.Width;
			size.Height += _TabLayoutOffset.Height;
			size.Width += _TabStripItem.ItemPadding.Horizontal;
			size.Height += _TabStripItem.ItemPadding.Vertical;
		}
		if (_FixedTabSize.Width > 0)
		{
			size.Width = _FixedTabSize.Width;
		}
		if (_FixedTabSize.Height > 0)
		{
			size.Height = _FixedTabSize.Height;
		}
		if (!(item is ButtonItem) || ((ButtonItem)item).FixedSize.IsEmpty)
		{
			empty.Size = size;
			item.Bounds = empty;
		}
		if (!item.Stretch)
		{
			_List[item] = size;
		}
	}

	public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		if (lines.Count > 1)
		{
			int line = FindSelectedLine(lines);
			ReorderLines(lines, line);
			SetBlockDisplay(containerBounds, lines);
			ResizeMultiLine(lines);
		}
		else
		{
			SetBlockDisplay(containerBounds, lines);
			PromoteSelectedTab(containerBounds, lines);
			ResizeSingleLine(lines);
		}
		_TabStripItem.TabLines = lines.Count;
		_List.Clear();
		return blocksBounds;
	}

	private void ReorderLines(ArrayList lines, int line)
	{
		_SelLine = 0;
		eTabStripAlignment tabAlignment = _TabStripItem.TabAlignment;
		if (tabAlignment == eTabStripAlignment.Left || tabAlignment == eTabStripAlignment.Top)
		{
			line++;
			_SelLine = lines.Count - line;
		}
		else
		{
			_SelLine = line;
		}
		if (_SelLine > 0)
		{
			AdjustLines(lines, line);
		}
	}

	private void AdjustLines(ArrayList lines, int n)
	{
		Size[] lineSizes = GetLineSizes(lines);
		Point pt = default(Point);
		for (int i = 0; i < lines.Count; i++)
		{
			int num = (i + n) % lines.Count;
			if (lines[num] is SerialContentLayoutManager.BlockLineInfo info)
			{
				AdjustBlocks(info, pt);
				pt.X += lineSizes[num].Width;
				pt.Y += lineSizes[num].Height;
			}
		}
	}

	private Size[] GetLineSizes(ArrayList lines)
	{
		Size[] array = new Size[lines.Count];
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i] is SerialContentLayoutManager.BlockLineInfo blockLineInfo && blockLineInfo.Blocks[0] is IBlock block)
			{
				array[i] = block.Bounds.Size;
			}
		}
		return array;
	}

	private void AdjustBlocks(SerialContentLayoutManager.BlockLineInfo info, Point pt)
	{
		for (int i = 0; i < info.Blocks.Count; i++)
		{
			if (info.Blocks[i] is BaseItem baseItem)
			{
				eTabStripAlignment tabAlignment = _TabStripItem.TabAlignment;
				if ((uint)(tabAlignment - 2) <= 1u)
				{
					baseItem.TopInternal = pt.Y;
				}
				else
				{
					baseItem.LeftInternal = pt.X;
				}
			}
		}
	}

	private void SetBlockDisplay(Rectangle containerBounds, ArrayList lines)
	{
		for (int i = 0; i < lines.Count; i++)
		{
			if (!(lines[i] is SerialContentLayoutManager.BlockLineInfo blockLineInfo))
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < blockLineInfo.Blocks.Count; j++)
			{
				if (blockLineInfo.Blocks[j] is BaseItem { Visible: var displayed } baseItem)
				{
					if (blockLineInfo.Blocks.Count > 1 && !containerBounds.Contains(baseItem.Bounds))
					{
						flag = false;
						displayed = false;
					}
					baseItem.Displayed = displayed;
					if (baseItem.Displayed)
					{
						AlignBaseItem(baseItem);
					}
				}
			}
			if (!flag)
			{
				continue;
			}
			Size size;
			int breakCount = GetBreakCount(blockLineInfo, out size);
			if (breakCount > 0)
			{
				if (_TabStripItem.IsVertical)
				{
					ProcessBreaks(blockLineInfo, breakCount, containerBounds.Height - size.Height, vertical: true);
				}
				else
				{
					ProcessBreaks(blockLineInfo, breakCount, containerBounds.Width - size.Width, vertical: false);
				}
			}
		}
	}

	private void AlignBaseItem(BaseItem item)
	{
		Rectangle bounds = item.Bounds;
		if (!item.Stretch && _List.TryGetValue(item, out var value))
		{
			bounds.Size = value;
			bounds.Y = item.Bounds.Y + (item.Bounds.Height - bounds.Height) / 2;
			switch (item.ItemAlignment)
			{
			case eItemAlignment.Center:
				bounds.X += (item.Bounds.Width - bounds.Width) / 2;
				break;
			case eItemAlignment.Far:
				bounds.X = item.Bounds.Right - bounds.Width;
				break;
			}
		}
		if (!(item is SuperTabItem))
		{
			bounds.X += _TabStripItem.ItemPadding.Left;
			bounds.Y += _TabStripItem.ItemPadding.Top;
			bounds.Width -= _TabStripItem.ItemPadding.Horizontal;
			bounds.Height -= _TabStripItem.ItemPadding.Vertical;
		}
		item.Bounds = bounds;
	}

	private int GetBreakCount(SerialContentLayoutManager.BlockLineInfo info, out Size size)
	{
		int num = 0;
		size = Size.Empty;
		for (int i = 0; i < info.Blocks.Count; i++)
		{
			if (info.Blocks[i] is BaseItem { Displayed: not false } baseItem)
			{
				size.Width = baseItem.Bounds.Right;
				size.Height = baseItem.Bounds.Bottom;
				if (baseItem.BeginGroup)
				{
					num++;
				}
			}
		}
		return num;
	}

	private void ProcessBreaks(SerialContentLayoutManager.BlockLineInfo info, int count, int span, bool vertical)
	{
		int num = 0;
		float num2 = 0f;
		float num3 = (float)span / (float)count;
		for (int i = 0; i < info.Blocks.Count; i++)
		{
			if (!(info.Blocks[i] is BaseItem { Displayed: not false } baseItem))
			{
				continue;
			}
			if (baseItem.BeginGroup)
			{
				num2 = ((++num == count) ? ((float)span) : (num2 + num3));
			}
			if (num2 > 0f)
			{
				if (vertical)
				{
					baseItem.TopInternal += (int)num2;
				}
				else
				{
					baseItem.LeftInternal += (int)num2;
				}
			}
		}
	}

	private void PromoteSelectedTab(Rectangle containerBounds, ArrayList lines)
	{
		BaseItem visibleTab = _TabStripItem.VisibleTab;
		if (visibleTab == null || visibleTab.Displayed || !(lines[0] is SerialContentLayoutManager.BlockLineInfo blockLineInfo))
		{
			return;
		}
		for (int num = blockLineInfo.Blocks.Count - 1; num >= 0; num--)
		{
			if (blockLineInfo.Blocks[num] is BaseItem baseItem)
			{
				Rectangle rectangle = _TabStripItem.TabDisplay.NextBlockPosition(baseItem, visibleTab);
				if (containerBounds.Contains(rectangle))
				{
					visibleTab.Bounds = rectangle;
					visibleTab.Displayed = true;
					break;
				}
				baseItem.Displayed = false;
				if (num == 0)
				{
					rectangle = baseItem.Bounds;
					rectangle.Size = visibleTab.Size;
					visibleTab.Bounds = rectangle;
					visibleTab.Displayed = true;
				}
			}
		}
	}

	private int FindSelectedLine(ArrayList lines)
	{
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i] is SerialContentLayoutManager.BlockLineInfo blockLineInfo)
			{
				int num = FindSelectedBlock(blockLineInfo.Blocks);
				if (num >= 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int FindSelectedBlock(ArrayList blocks)
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			if (blocks[i] is SuperTabItem superTabItem && superTabItem == _TabStripItem.SelectedTab)
			{
				return i;
			}
		}
		return -1;
	}

	private void ResizeSingleLine(ArrayList lines)
	{
		if (lines[0] is SerialContentLayoutManager.BlockLineInfo info)
		{
			switch (_TabStripItem.TabAlignment)
			{
			case eTabStripAlignment.Top:
				ResizeSingleLine1(info, 0, _TabLayoutOffset.Height, 0, -_TabLayoutOffset.Height);
				break;
			case eTabStripAlignment.Left:
				ResizeSingleLine1(info, _TabLayoutOffset.Width, 0, -_TabLayoutOffset.Width, 0);
				break;
			case eTabStripAlignment.Bottom:
				ResizeSingleLine2(info, 0, 2, 0, -_TabLayoutOffset.Height);
				break;
			case eTabStripAlignment.Right:
				ResizeSingleLine2(info, 2, 0, -_TabLayoutOffset.Width, 0);
				break;
			}
		}
	}

	private void ResizeSingleLine1(SerialContentLayoutManager.BlockLineInfo info, int dx, int dy, int dw, int dh)
	{
		for (int i = 0; i < info.Blocks.Count; i++)
		{
			if (info.Blocks[i] is BaseItem baseItem && !(info.Blocks[i] is SuperTabItem { IsSelected: not false }))
			{
				Rectangle bounds = baseItem.Bounds;
				bounds.X += dx;
				bounds.Y += dy;
				bounds.Width += dw;
				bounds.Height += dh;
				baseItem.Bounds = bounds;
			}
		}
	}

	private void ResizeSingleLine2(SerialContentLayoutManager.BlockLineInfo info, int dx, int dy, int dw, int dh)
	{
		for (int i = 0; i < info.Blocks.Count; i++)
		{
			if (info.Blocks[i] is BaseItem { Bounds: var bounds } baseItem)
			{
				SuperTabItem superTabItem = info.Blocks[i] as SuperTabItem;
				bounds.X += dx;
				bounds.Y += dy;
				if (superTabItem == null || !superTabItem.IsSelected)
				{
					bounds.Width += dw;
					bounds.Height += dh;
				}
				baseItem.Bounds = bounds;
			}
		}
	}

	private void ResizeMultiLine(ArrayList lines)
	{
		switch (_TabStripItem.TabAlignment)
		{
		case eTabStripAlignment.Top:
			ResizeMultiLineTop();
			break;
		case eTabStripAlignment.Left:
			ResizeMultiLineLeft();
			break;
		case eTabStripAlignment.Bottom:
			ResizeMultiLineBottom(lines);
			break;
		case eTabStripAlignment.Right:
			ResizeMultiLineRight(lines);
			break;
		}
	}

	private void ResizeMultiLineTop()
	{
		if (_TabStripItem.SelectedTab != null)
		{
			Rectangle bounds = _TabStripItem.SelectedTab.Bounds;
			bounds.Y -= _TabLayoutOffset.Height;
			bounds.Height += _TabLayoutOffset.Height;
			_TabStripItem.SelectedTab.Bounds = bounds;
		}
	}

	private void ResizeMultiLineLeft()
	{
		if (_TabStripItem.SelectedTab != null)
		{
			Rectangle bounds = _TabStripItem.SelectedTab.Bounds;
			bounds.X -= _TabLayoutOffset.Width;
			bounds.Width += _TabLayoutOffset.Width;
			_TabStripItem.SelectedTab.Bounds = bounds;
		}
	}

	private void ResizeMultiLineBottom(ArrayList lines)
	{
		for (int i = 0; i < lines.Count; i++)
		{
			if (!(lines[i] is SerialContentLayoutManager.BlockLineInfo blockLineInfo))
			{
				continue;
			}
			for (int j = 0; j < blockLineInfo.Blocks.Count; j++)
			{
				if (blockLineInfo.Blocks[j] is BaseItem { Bounds: var bounds } baseItem)
				{
					bounds.Y += 2;
					if (blockLineInfo.Blocks[j] is SuperTabItem { IsSelected: not false })
					{
						bounds.Height += _TabLayoutOffset.Height;
					}
					baseItem.Bounds = bounds;
				}
			}
		}
	}

	private void ResizeMultiLineRight(ArrayList lines)
	{
		for (int i = 0; i < lines.Count; i++)
		{
			if (!(lines[i] is SerialContentLayoutManager.BlockLineInfo blockLineInfo))
			{
				continue;
			}
			for (int j = 0; j < blockLineInfo.Blocks.Count; j++)
			{
				if (blockLineInfo.Blocks[j] is BaseItem { Bounds: var bounds } baseItem)
				{
					bounds.X += 2;
					if (blockLineInfo.Blocks[j] is SuperTabItem { IsSelected: not false })
					{
						bounds.Width += _TabLayoutOffset.Width;
					}
					baseItem.Bounds = bounds;
				}
			}
		}
	}
}
