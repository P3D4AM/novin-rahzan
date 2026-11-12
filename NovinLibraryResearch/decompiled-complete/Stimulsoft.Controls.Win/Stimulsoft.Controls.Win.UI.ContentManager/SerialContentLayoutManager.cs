using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Controls.Win.UI.ContentManager;

public class SerialContentLayoutManager : IContentLayout
{
	private struct SizeExtended
	{
		public int Width;

		public int Height;

		public float WidthReduction;

		public float HeightReduction;

		public bool UseAbsoluteWidth;
	}

	internal class BlockLineInfo
	{
		public ArrayList Blocks = new ArrayList();

		public Size LineSize = Size.Empty;

		public int Line;

		public int VisibleItemsCount;
	}

	private int m_BlockSpacing;

	private bool m_FitContainerOversize;

	private bool m_FitContainer;

	private bool m_VerticalFitContainerWidth;

	private bool m_HorizontalFitContainerHeight;

	private eContentOrientation m_ContentOrientation;

	private eContentAlignment m_ContentAlignment;

	private eContentVerticalAlignment m_ContentVerticalAlignment = eContentVerticalAlignment.Middle;

	private eContentVerticalAlignment m_BlockLineAlignment = eContentVerticalAlignment.Middle;

	private bool m_EvenHeight;

	private bool m_MultiLine;

	private bool m_RightToLeft;

	private bool m_OversizeDistribute;

	public virtual int BlockSpacing
	{
		get
		{
			return m_BlockSpacing;
		}
		set
		{
			m_BlockSpacing = value;
		}
	}

	public virtual bool FitContainerOversize
	{
		get
		{
			return m_FitContainerOversize;
		}
		set
		{
			m_FitContainerOversize = value;
		}
	}

	public virtual bool FitContainer
	{
		get
		{
			return m_FitContainer;
		}
		set
		{
			m_FitContainer = value;
		}
	}

	public virtual bool VerticalFitContainerWidth
	{
		get
		{
			return m_VerticalFitContainerWidth;
		}
		set
		{
			m_VerticalFitContainerWidth = value;
		}
	}

	public virtual bool HorizontalFitContainerHeight
	{
		get
		{
			return m_HorizontalFitContainerHeight;
		}
		set
		{
			m_HorizontalFitContainerHeight = value;
		}
	}

	public virtual eContentOrientation ContentOrientation
	{
		get
		{
			return m_ContentOrientation;
		}
		set
		{
			m_ContentOrientation = value;
		}
	}

	public virtual eContentVerticalAlignment ContentVerticalAlignment
	{
		get
		{
			return m_ContentVerticalAlignment;
		}
		set
		{
			m_ContentVerticalAlignment = value;
		}
	}

	public virtual eContentVerticalAlignment BlockLineAlignment
	{
		get
		{
			return m_BlockLineAlignment;
		}
		set
		{
			m_BlockLineAlignment = value;
		}
	}

	public virtual eContentAlignment ContentAlignment
	{
		get
		{
			return m_ContentAlignment;
		}
		set
		{
			m_ContentAlignment = value;
		}
	}

	public virtual bool EvenHeight
	{
		get
		{
			return m_EvenHeight;
		}
		set
		{
			m_EvenHeight = value;
		}
	}

	public virtual bool OversizeDistribute
	{
		get
		{
			return m_OversizeDistribute;
		}
		set
		{
			m_OversizeDistribute = value;
		}
	}

	public bool MultiLine
	{
		get
		{
			return m_MultiLine;
		}
		set
		{
			m_MultiLine = value;
		}
	}

	public bool RightToLeft
	{
		get
		{
			return m_RightToLeft;
		}
		set
		{
			m_RightToLeft = value;
		}
	}

	public event LayoutManagerPositionEventHandler NextPosition;

	public event LayoutManagerLayoutEventHandler BeforeNewBlockLayout;

	public virtual Rectangle Layout(Rectangle containerBounds, IBlock[] contentBlocks, BlockLayoutManager blockLayout)
	{
		Rectangle rectangle = Rectangle.Empty;
		Point point = containerBounds.Location;
		ArrayList arrayList = new ArrayList();
		arrayList.Add(new BlockLineInfo());
		BlockLineInfo blockLineInfo = arrayList[0] as BlockLineInfo;
		bool flag = false;
		bool flag2 = true;
		int num = 0;
		foreach (IBlock block in contentBlocks)
		{
			if (!block.Visible)
			{
				block.Bounds = Rectangle.Empty;
				continue;
			}
			if (this.BeforeNewBlockLayout != null)
			{
				LayoutManagerLayoutEventArgs layoutManagerLayoutEventArgs = new LayoutManagerLayoutEventArgs(block, point, num);
				this.BeforeNewBlockLayout(this, layoutManagerLayoutEventArgs);
				point = layoutManagerLayoutEventArgs.CurrentPosition;
				if (layoutManagerLayoutEventArgs.CancelLayout)
				{
					continue;
				}
			}
			num++;
			Size size = containerBounds.Size;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (block is IBlockExtended)
			{
				IBlockExtended blockExtended = block as IBlockExtended;
				flag3 = blockExtended.IsBlockElement;
				flag4 = blockExtended.IsNewLineAfterElement;
				flag2 = blockExtended.CanStartNewLine;
				flag5 = blockExtended.IsBlockContainer;
			}
			else
			{
				flag2 = true;
			}
			if (!flag3 && !flag5)
			{
				if (m_ContentOrientation == eContentOrientation.Horizontal)
				{
					size.Width = containerBounds.Right - point.X;
				}
				else
				{
					size.Height = containerBounds.Bottom - point.Y;
				}
			}
			blockLayout.Layout(block, size);
			if (m_MultiLine && blockLineInfo.Blocks.Count > 0)
			{
				if (m_ContentOrientation == eContentOrientation.Horizontal && ((point.X + block.Bounds.Width > containerBounds.Right && flag2) || flag3 || flag))
				{
					point.X = containerBounds.X;
					point.Y += blockLineInfo.LineSize.Height + m_BlockSpacing;
					blockLineInfo = new BlockLineInfo();
					blockLineInfo.Line = arrayList.Count;
					arrayList.Add(blockLineInfo);
				}
				else if (m_ContentOrientation == eContentOrientation.Vertical && ((point.Y + block.Bounds.Height > containerBounds.Bottom && flag2) || flag3 || flag))
				{
					point.Y = containerBounds.Y;
					point.X += blockLineInfo.LineSize.Width + m_BlockSpacing;
					blockLineInfo = new BlockLineInfo();
					blockLineInfo.Line = arrayList.Count;
					arrayList.Add(blockLineInfo);
				}
			}
			if (m_ContentOrientation == eContentOrientation.Horizontal)
			{
				if (block.Bounds.Height > blockLineInfo.LineSize.Height)
				{
					blockLineInfo.LineSize.Height = block.Bounds.Height;
				}
				blockLineInfo.LineSize.Width = point.X + block.Bounds.Width - containerBounds.X;
			}
			else if (m_ContentOrientation == eContentOrientation.Vertical)
			{
				if (block.Bounds.Width > blockLineInfo.LineSize.Width)
				{
					blockLineInfo.LineSize.Width = block.Bounds.Width;
				}
				blockLineInfo.LineSize.Height = point.Y + block.Bounds.Height - containerBounds.Y;
			}
			blockLineInfo.Blocks.Add(block);
			if (block.Visible)
			{
				blockLineInfo.VisibleItemsCount++;
			}
			Rectangle rectangle2 = new Rectangle(point, block.Bounds.Size);
			rectangle2.X += block.Margin.Left;
			rectangle2.Y += block.Margin.Top;
			block.Bounds = rectangle2;
			if (rectangle.IsEmpty)
			{
				rectangle2 = block.Bounds;
				rectangle2.X -= block.Margin.Left;
				rectangle2.Y -= block.Margin.Top;
				rectangle2.Width += block.Margin.Horizontal;
				rectangle2.Height += block.Margin.Vertical;
				rectangle = rectangle2;
			}
			else
			{
				rectangle = Rectangle.Union(rectangle, block.Bounds);
			}
			flag = flag3 || flag4;
			point = GetNextPosition(block, point);
		}
		rectangle = AlignResizeBlocks(containerBounds, rectangle, arrayList);
		if (m_RightToLeft)
		{
			rectangle = MirrorContent(containerBounds, rectangle, contentBlocks);
		}
		return blockLayout.FinalizeLayout(containerBounds, rectangle, arrayList);
	}

	private Rectangle AlignResizeBlocks(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		Rectangle rectangle = Rectangle.Empty;
		if (containerBounds.IsEmpty || blocksBounds.IsEmpty || ((BlockLineInfo)lines[0]).Blocks.Count == 0)
		{
			return rectangle;
		}
		if (m_ContentAlignment == eContentAlignment.Left && m_ContentVerticalAlignment == eContentVerticalAlignment.Top && !m_FitContainer && !m_FitContainerOversize && !m_EvenHeight && m_BlockLineAlignment == eContentVerticalAlignment.Top)
		{
			return blocksBounds;
		}
		Point[] array = new Point[lines.Count];
		SizeExtended[] array2 = new SizeExtended[lines.Count];
		foreach (BlockLineInfo line in lines)
		{
			if (m_ContentOrientation == eContentOrientation.Horizontal)
			{
				if ((m_FitContainer && containerBounds.Width > line.LineSize.Width) || (m_FitContainerOversize && line.LineSize.Width > containerBounds.Width))
				{
					if (m_OversizeDistribute && (double)containerBounds.Width < (double)line.LineSize.Width * 0.75)
					{
						array2[line.Line].Width = (int)Math.Floor((float)(containerBounds.Width - line.VisibleItemsCount * m_BlockSpacing) / (float)line.VisibleItemsCount);
						array2[line.Line].UseAbsoluteWidth = true;
					}
					else
					{
						array2[line.Line].Width = (containerBounds.Width - line.VisibleItemsCount * m_BlockSpacing - line.LineSize.Width) / line.VisibleItemsCount;
					}
					array2[line.Line].WidthReduction = (float)(containerBounds.Width - line.VisibleItemsCount * m_BlockSpacing) / (float)line.LineSize.Width;
					blocksBounds.Width = containerBounds.Width;
				}
				if (m_HorizontalFitContainerHeight && containerBounds.Height > blocksBounds.Height)
				{
					array2[line.Line].Height = (containerBounds.Height - line.LineSize.Height) / lines.Count;
				}
			}
			else
			{
				if ((m_FitContainer && containerBounds.Height > line.LineSize.Height) || (m_FitContainerOversize && line.LineSize.Height > containerBounds.Height))
				{
					if (m_OversizeDistribute && (double)containerBounds.Width < (double)line.LineSize.Width * 0.75)
					{
						array2[line.Line].Height = (int)Math.Floor((float)(containerBounds.Height - line.VisibleItemsCount * m_BlockSpacing) / (float)line.VisibleItemsCount);
						array2[line.Line].UseAbsoluteWidth = true;
					}
					else
					{
						array2[line.Line].Height = (containerBounds.Height - line.VisibleItemsCount * m_BlockSpacing - line.LineSize.Height) / line.VisibleItemsCount;
					}
					array2[line.Line].HeightReduction = (float)(containerBounds.Height - line.VisibleItemsCount * m_BlockSpacing) / (float)line.LineSize.Height;
					blocksBounds.Height = containerBounds.Height;
				}
				if (m_VerticalFitContainerWidth && containerBounds.Width > blocksBounds.Width)
				{
					array2[line.Line].Width = (containerBounds.Width - line.LineSize.Width) / lines.Count;
				}
			}
			if (m_ContentOrientation == eContentOrientation.Horizontal && !m_FitContainer && ((containerBounds.Width > blocksBounds.Width && m_FitContainerOversize) || !m_FitContainerOversize))
			{
				switch (m_ContentAlignment)
				{
				case eContentAlignment.Right:
					if (containerBounds.Width > line.LineSize.Width)
					{
						array[line.Line].X = containerBounds.Width - line.LineSize.Width;
					}
					break;
				case eContentAlignment.Center:
					if (containerBounds.Width > line.LineSize.Width)
					{
						array[line.Line].X = (containerBounds.Width - line.LineSize.Width) / 2;
					}
					break;
				}
			}
			if (m_ContentOrientation != eContentOrientation.Vertical || m_FitContainer || ((containerBounds.Height <= blocksBounds.Height || !m_FitContainerOversize) && m_FitContainerOversize))
			{
				continue;
			}
			switch (m_ContentVerticalAlignment)
			{
			case eContentVerticalAlignment.Bottom:
				if (containerBounds.Height > line.LineSize.Height)
				{
					array[line.Line].Y = containerBounds.Height - line.LineSize.Height;
				}
				break;
			case eContentVerticalAlignment.Middle:
				if (containerBounds.Height > line.LineSize.Height)
				{
					array[line.Line].Y = (containerBounds.Height - line.LineSize.Height) / 2;
				}
				break;
			}
		}
		if (m_VerticalFitContainerWidth && containerBounds.Width > blocksBounds.Width && m_ContentOrientation == eContentOrientation.Vertical)
		{
			blocksBounds.Width = containerBounds.Width;
		}
		else if (m_HorizontalFitContainerHeight && containerBounds.Height > blocksBounds.Height && m_ContentOrientation == eContentOrientation.Horizontal)
		{
			blocksBounds.Height = containerBounds.Height;
		}
		if (m_ContentOrientation == eContentOrientation.Horizontal)
		{
			foreach (BlockLineInfo line2 in lines)
			{
				foreach (IBlock block3 in line2.Blocks)
				{
					if (!block3.Visible)
					{
						continue;
					}
					Rectangle bounds = block3.Bounds;
					if (m_EvenHeight && line2.LineSize.Height > 0)
					{
						bounds.Height = line2.LineSize.Height;
					}
					bounds.Offset(array[line2.Line]);
					if (m_ContentVerticalAlignment == eContentVerticalAlignment.Middle)
					{
						if (m_BlockLineAlignment == eContentVerticalAlignment.Middle)
						{
							bounds.Offset(0, (containerBounds.Height - blocksBounds.Height + (line2.LineSize.Height - bounds.Height)) / 2);
						}
						else
						{
							bounds.Offset(0, (containerBounds.Height - blocksBounds.Height) / 2);
						}
						if (m_BlockLineAlignment == eContentVerticalAlignment.Bottom)
						{
							bounds.Offset(0, line2.LineSize.Height - bounds.Height);
						}
					}
					else if (m_ContentVerticalAlignment == eContentVerticalAlignment.Bottom)
					{
						bounds.Offset(0, containerBounds.Height - blocksBounds.Height);
					}
					if (m_ContentVerticalAlignment != eContentVerticalAlignment.Middle)
					{
						if (m_BlockLineAlignment == eContentVerticalAlignment.Middle)
						{
							bounds.Offset(0, (line2.LineSize.Height - bounds.Height) / 2);
						}
						else if (m_BlockLineAlignment == eContentVerticalAlignment.Bottom)
						{
							bounds.Offset(0, line2.LineSize.Height - bounds.Height);
						}
					}
					if (array2[line2.Line].Width != 0)
					{
						if (m_OversizeDistribute)
						{
							int num = (array2[line2.Line].UseAbsoluteWidth ? array2[line2.Line].Width : ((int)Math.Floor((float)bounds.Width * array2[line2.Line].WidthReduction)));
							array[line2.Line].X += num - bounds.Width;
							bounds.Width = num;
						}
						else
						{
							bounds.Width += array2[line2.Line].Width;
							array[line2.Line].X += array2[line2.Line].Width;
						}
					}
					bounds.Height += array2[line2.Line].Height;
					block3.Bounds = bounds;
					rectangle = ((!rectangle.IsEmpty) ? Rectangle.Union(rectangle, block3.Bounds) : block3.Bounds);
				}
				if (!m_OversizeDistribute && array2[line2.Line].Width != 0 && containerBounds.Width - (line2.LineSize.Width + array2[line2.Line].Width * line2.Blocks.Count) != 0)
				{
					Rectangle bounds2 = ((IBlock)line2.Blocks[line2.Blocks.Count - 1]).Bounds;
					bounds2.Width += containerBounds.Width - (line2.LineSize.Width + array2[line2.Line].Width * line2.Blocks.Count);
					((IBlock)line2.Blocks[line2.Blocks.Count - 1]).Bounds = bounds2;
				}
			}
		}
		else
		{
			foreach (BlockLineInfo line3 in lines)
			{
				foreach (IBlock block4 in line3.Blocks)
				{
					if (!block4.Visible)
					{
						continue;
					}
					Rectangle bounds3 = block4.Bounds;
					if (m_EvenHeight && line3.LineSize.Width > 0)
					{
						bounds3.Width = line3.LineSize.Width - block4.Margin.Horizontal;
					}
					bounds3.Offset(array[line3.Line]);
					if (m_ContentAlignment == eContentAlignment.Center)
					{
						bounds3.Offset((containerBounds.Width - blocksBounds.Width + (line3.LineSize.Width - bounds3.Width)) / 2, 0);
					}
					else if (m_ContentAlignment == eContentAlignment.Right)
					{
						bounds3.Offset(containerBounds.Width - blocksBounds.Width + line3.LineSize.Width - bounds3.Width, 0);
					}
					bounds3.Width += array2[line3.Line].Width;
					if (array2[line3.Line].Height != 0)
					{
						if (m_OversizeDistribute)
						{
							int num2 = (array2[line3.Line].UseAbsoluteWidth ? array2[line3.Line].Height : ((int)Math.Floor((float)bounds3.Height * array2[line3.Line].HeightReduction)));
							array[line3.Line].Y += num2 - bounds3.Height;
							bounds3.Height = num2;
						}
						else
						{
							bounds3.Height += array2[line3.Line].Height;
							array[line3.Line].Y += array2[line3.Line].Height;
						}
					}
					block4.Bounds = bounds3;
					if (rectangle.IsEmpty)
					{
						bounds3.Y -= block4.Margin.Top;
						bounds3.X -= block4.Margin.Left;
						bounds3.Width += block4.Margin.Horizontal;
						bounds3.Height += block4.Margin.Vertical;
						rectangle = bounds3;
					}
					else
					{
						rectangle = Rectangle.Union(rectangle, block4.Bounds);
					}
				}
				if (!m_OversizeDistribute && array2[line3.Line].Height != 0 && containerBounds.Height - (line3.LineSize.Height + array2[line3.Line].Height * line3.Blocks.Count) != 0)
				{
					Rectangle bounds4 = ((IBlock)line3.Blocks[line3.Blocks.Count - 1]).Bounds;
					bounds4.Height += containerBounds.Height - (line3.LineSize.Height + array2[line3.Line].Height * line3.Blocks.Count);
					((IBlock)line3.Blocks[line3.Blocks.Count - 1]).Bounds = bounds4;
				}
			}
		}
		return rectangle;
	}

	private Point GetNextPosition(IBlock block, Point position)
	{
		if (this.NextPosition != null)
		{
			LayoutManagerPositionEventArgs layoutManagerPositionEventArgs = new LayoutManagerPositionEventArgs();
			layoutManagerPositionEventArgs.Block = block;
			layoutManagerPositionEventArgs.CurrentPosition = position;
			this.NextPosition(this, layoutManagerPositionEventArgs);
			if (layoutManagerPositionEventArgs.Cancel)
			{
				return layoutManagerPositionEventArgs.NextPosition;
			}
		}
		if (m_ContentOrientation == eContentOrientation.Horizontal)
		{
			position.X += block.Bounds.Width + m_BlockSpacing + block.Margin.Horizontal;
		}
		else
		{
			position.Y += block.Bounds.Height + m_BlockSpacing + block.Margin.Vertical;
		}
		return position;
	}

	private Rectangle MirrorContent(Rectangle containerBounds, Rectangle blockBounds, IBlock[] contentBlocks)
	{
		int num = blockBounds.X - containerBounds.X;
		if (blockBounds.Width < containerBounds.Width)
		{
			blockBounds.X = containerBounds.Right - (blockBounds.X - containerBounds.X + blockBounds.Width);
		}
		else if (blockBounds.Width > containerBounds.Width)
		{
			containerBounds.Width = blockBounds.Width;
		}
		foreach (IBlock block in contentBlocks)
		{
			if (block.Visible)
			{
				Rectangle bounds = block.Bounds;
				block.Bounds = new Rectangle(containerBounds.Right - (bounds.X - containerBounds.X + bounds.Width), bounds.Y, bounds.Width, bounds.Height);
			}
		}
		return blockBounds;
	}
}
