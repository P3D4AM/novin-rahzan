using System;
using System.Collections;
using System.Drawing;
using System.Threading;

namespace DevComponents.UI.ContentManager;

public class SerialContentLayoutManager : IContentLayout
{
	internal class _0CF0
	{
		public ArrayList ೱ = new ArrayList();

		public Size ೲ = Size.Empty;

		public int _0CF3;

		public int _0CF4;
	}

	private struct _0CF5
	{
		public int ܐ;

		public int _0711;

		public float _0CF6;

		public float _0CF7;

		public bool _0CF8;
	}

	private LayoutManagerPositionEventHandler _0CD4;

	private LayoutManagerLayoutEventHandler _0CD5;

	private int _0CD6;

	private bool _0CD7;

	private bool _0CD8;

	private bool _0CD9;

	private bool _0CDA;

	private eContentOrientation _0CDB;

	private eContentAlignment _0CDC;

	private eContentVerticalAlignment _0CDD = eContentVerticalAlignment.Middle;

	private eContentVerticalAlignment ೞ = eContentVerticalAlignment.Middle;

	private bool _0CDF;

	private bool ೠ;

	private bool ೡ;

	private bool _0CE2;

	private bool __0CE3;

	private bool __0CE4;

	public bool ReserveLeftSpace
	{
		get
		{
			return __0CE3;
		}
		set
		{
			__0CE3 = value;
		}
	}

	public virtual int BlockSpacing
	{
		get
		{
			return _0CD6;
		}
		set
		{
			_0CD6 = value;
		}
	}

	public virtual bool FitContainerOversize
	{
		get
		{
			return _0CD7;
		}
		set
		{
			_0CD7 = value;
		}
	}

	public virtual bool FitContainer
	{
		get
		{
			return _0CD8;
		}
		set
		{
			_0CD8 = value;
		}
	}

	public virtual bool VerticalFitContainerWidth
	{
		get
		{
			return _0CD9;
		}
		set
		{
			_0CD9 = value;
		}
	}

	public virtual bool HorizontalFitContainerHeight
	{
		get
		{
			return _0CDA;
		}
		set
		{
			_0CDA = value;
		}
	}

	public virtual eContentOrientation ContentOrientation
	{
		get
		{
			return _0CDB;
		}
		set
		{
			_0CDB = value;
		}
	}

	public virtual eContentVerticalAlignment ContentVerticalAlignment
	{
		get
		{
			return _0CDD;
		}
		set
		{
			_0CDD = value;
		}
	}

	public virtual eContentVerticalAlignment BlockLineAlignment
	{
		get
		{
			return ೞ;
		}
		set
		{
			ೞ = value;
		}
	}

	public virtual eContentAlignment ContentAlignment
	{
		get
		{
			return _0CDC;
		}
		set
		{
			_0CDC = value;
		}
	}

	public virtual bool EvenHeight
	{
		get
		{
			return _0CDF;
		}
		set
		{
			_0CDF = value;
		}
	}

	public virtual bool OversizeDistribute
	{
		get
		{
			return _0CE2;
		}
		set
		{
			_0CE2 = value;
		}
	}

	public bool MultiLine
	{
		get
		{
			return ೠ;
		}
		set
		{
			ೠ = value;
		}
	}

	public bool RightToLeft
	{
		get
		{
			return ೡ;
		}
		set
		{
			ೡ = value;
		}
	}

	public bool EqualItemSize
	{
		get
		{
			return __0CE4;
		}
		set
		{
			__0CE4 = value;
		}
	}

	public event LayoutManagerPositionEventHandler NextPosition
	{
		add
		{
			LayoutManagerPositionEventHandler layoutManagerPositionEventHandler = _0CD4;
			LayoutManagerPositionEventHandler layoutManagerPositionEventHandler2;
			do
			{
				layoutManagerPositionEventHandler2 = layoutManagerPositionEventHandler;
				LayoutManagerPositionEventHandler value2 = (LayoutManagerPositionEventHandler)Delegate.Combine(layoutManagerPositionEventHandler2, value);
				layoutManagerPositionEventHandler = Interlocked.CompareExchange(ref _0CD4, value2, layoutManagerPositionEventHandler2);
			}
			while ((object)layoutManagerPositionEventHandler != layoutManagerPositionEventHandler2);
		}
		remove
		{
			LayoutManagerPositionEventHandler layoutManagerPositionEventHandler = _0CD4;
			LayoutManagerPositionEventHandler layoutManagerPositionEventHandler2;
			do
			{
				layoutManagerPositionEventHandler2 = layoutManagerPositionEventHandler;
				LayoutManagerPositionEventHandler value2 = (LayoutManagerPositionEventHandler)Delegate.Remove(layoutManagerPositionEventHandler2, value);
				layoutManagerPositionEventHandler = Interlocked.CompareExchange(ref _0CD4, value2, layoutManagerPositionEventHandler2);
			}
			while ((object)layoutManagerPositionEventHandler != layoutManagerPositionEventHandler2);
		}
	}

	public event LayoutManagerLayoutEventHandler BeforeNewBlockLayout
	{
		add
		{
			LayoutManagerLayoutEventHandler layoutManagerLayoutEventHandler = _0CD5;
			LayoutManagerLayoutEventHandler layoutManagerLayoutEventHandler2;
			do
			{
				layoutManagerLayoutEventHandler2 = layoutManagerLayoutEventHandler;
				LayoutManagerLayoutEventHandler value2 = (LayoutManagerLayoutEventHandler)Delegate.Combine(layoutManagerLayoutEventHandler2, value);
				layoutManagerLayoutEventHandler = Interlocked.CompareExchange(ref _0CD5, value2, layoutManagerLayoutEventHandler2);
			}
			while ((object)layoutManagerLayoutEventHandler != layoutManagerLayoutEventHandler2);
		}
		remove
		{
			LayoutManagerLayoutEventHandler layoutManagerLayoutEventHandler = _0CD5;
			LayoutManagerLayoutEventHandler layoutManagerLayoutEventHandler2;
			do
			{
				layoutManagerLayoutEventHandler2 = layoutManagerLayoutEventHandler;
				LayoutManagerLayoutEventHandler value2 = (LayoutManagerLayoutEventHandler)Delegate.Remove(layoutManagerLayoutEventHandler2, value);
				layoutManagerLayoutEventHandler = Interlocked.CompareExchange(ref _0CD5, value2, layoutManagerLayoutEventHandler2);
			}
			while ((object)layoutManagerLayoutEventHandler != layoutManagerLayoutEventHandler2);
		}
	}

	public virtual Rectangle Layout(Rectangle containerBounds, IBlock[] contentBlocks, BlockLayoutManager blockLayout)
	{
		Rectangle rectangle = Rectangle.Empty;
		Point point = containerBounds.Location;
		ArrayList arrayList = new ArrayList();
		arrayList.Add(new _0CF0());
		_0CF0 obj = arrayList[0] as _0CF0;
		bool _೬ = false;
		bool flag = true;
		int num = 0;
		bool flag2 = true;
		if (__0CE4)
		{
			Size empty = Size.Empty;
			foreach (IBlock block in contentBlocks)
			{
				if (!block.Visible)
				{
					block.Bounds = Rectangle.Empty;
					continue;
				}
				if (_0CD5 != null)
				{
					LayoutManagerLayoutEventArgs layoutManagerLayoutEventArgs = new LayoutManagerLayoutEventArgs(block, point, num);
					_0CD5(this, layoutManagerLayoutEventArgs);
					point = layoutManagerLayoutEventArgs.CurrentPosition;
					if (layoutManagerLayoutEventArgs.CancelLayout)
					{
						continue;
					}
				}
				num++;
				Size size = containerBounds.Size;
				blockLayout.Layout(block, size);
				if (block.Bounds.Width > empty.Width)
				{
					empty.Width = block.Bounds.Width;
				}
				if (block.Bounds.Height > empty.Height)
				{
					empty.Height = block.Bounds.Height;
				}
			}
			foreach (IBlock block2 in contentBlocks)
			{
				if (block2.Visible)
				{
					block2.Bounds = new Rectangle(block2.Bounds.Location, empty);
				}
			}
			flag2 = false;
		}
		foreach (IBlock block3 in contentBlocks)
		{
			if (!block3.Visible)
			{
				block3.Bounds = Rectangle.Empty;
				continue;
			}
			if (_0CD5 != null)
			{
				LayoutManagerLayoutEventArgs layoutManagerLayoutEventArgs2 = new LayoutManagerLayoutEventArgs(block3, point, num);
				_0CD5(this, layoutManagerLayoutEventArgs2);
				point = layoutManagerLayoutEventArgs2.CurrentPosition;
				if (layoutManagerLayoutEventArgs2.CancelLayout)
				{
					continue;
				}
			}
			num++;
			Size size2 = containerBounds.Size;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (block3 is IBlockExtended)
			{
				IBlockExtended blockExtended = block3 as IBlockExtended;
				flag3 = blockExtended.IsBlockElement;
				flag4 = blockExtended.IsNewLineAfterElement;
				flag = blockExtended.CanStartNewLine;
				flag5 = blockExtended.IsBlockContainer;
			}
			else
			{
				flag = true;
			}
			if (!flag3 && !flag5)
			{
				if (_0CDB == eContentOrientation.Horizontal)
				{
					size2.Width = containerBounds.Right - point.X;
				}
				else
				{
					size2.Height = containerBounds.Bottom - point.Y;
				}
			}
			if (flag2)
			{
				blockLayout.Layout(block3, size2);
			}
			if (ೠ && obj.ೱ.Count > 0)
			{
				if (_0CDB == eContentOrientation.Horizontal && ((point.X + block3.Bounds.Width > containerBounds.Right && flag) || flag3 || _೬))
				{
					point.X = containerBounds.X;
					point.Y += obj.ೲ.Height + _0CD6;
					obj = new _0CF0();
					obj._0CF3 = arrayList.Count;
					arrayList.Add(obj);
				}
				else if (_0CDB == eContentOrientation.Vertical && ((point.Y + block3.Bounds.Height > containerBounds.Bottom && flag) || flag3 || _೬))
				{
					point.Y = containerBounds.Y;
					point.X += obj.ೲ.Width + _0CD6;
					obj = new _0CF0();
					obj._0CF3 = arrayList.Count;
					arrayList.Add(obj);
				}
			}
			if (_0CDB == eContentOrientation.Horizontal)
			{
				if (block3.Bounds.Height > obj.ೲ.Height)
				{
					obj.ೲ.Height = block3.Bounds.Height;
				}
				obj.ೲ.Width = point.X + block3.Bounds.Width - containerBounds.X;
			}
			else if (_0CDB == eContentOrientation.Vertical)
			{
				if (block3.Bounds.Width > obj.ೲ.Width)
				{
					obj.ೲ.Width = block3.Bounds.Width;
				}
				obj.ೲ.Height = point.Y + block3.Bounds.Height - containerBounds.Y;
			}
			obj.ೱ.Add(block3);
			if (block3.Visible)
			{
				obj._0CF4++;
			}
			Rectangle rectangle2 = new Rectangle(point, block3.Bounds.Size);
			rectangle2.X += block3.Margin.Left;
			rectangle2.Y += block3.Margin.Top;
			block3.Bounds = rectangle2;
			if (rectangle.IsEmpty)
			{
				rectangle2 = block3.Bounds;
				rectangle2.X -= block3.Margin.Left;
				rectangle2.Y -= block3.Margin.Top;
				rectangle2.Width += block3.Margin.Horizontal;
				rectangle2.Height += block3.Margin.Vertical;
				rectangle = rectangle2;
			}
			else
			{
				rectangle = Rectangle.Union(rectangle, block3.Bounds);
			}
			_೬ = flag3 || flag4;
			point = _೩(block3, point, ref _೬);
		}
		rectangle = _0CE5(containerBounds, rectangle, arrayList);
		if (ೡ)
		{
			rectangle = _೭(containerBounds, rectangle, contentBlocks);
		}
		return blockLayout.FinalizeLayout(containerBounds, rectangle, arrayList);
	}

	private Rectangle _0CE5(Rectangle _೦, Rectangle _೧, ArrayList _೨)
	{
		Rectangle rectangle = Rectangle.Empty;
		if (_೦.IsEmpty || _೧.IsEmpty || ((_0CF0)_೨[0]).ೱ.Count == 0)
		{
			return rectangle;
		}
		if (_0CDC == eContentAlignment.Left && _0CDD == eContentVerticalAlignment.Top && !_0CD8 && !_0CD7 && !_0CDF && ೞ == eContentVerticalAlignment.Top && (_0CDB != eContentOrientation.Vertical || (!_0CD9 && !_0CDF) || ೠ))
		{
			return _೧;
		}
		Point[] array = new Point[_೨.Count];
		_0CF5[] array2 = new _0CF5[_೨.Count];
		foreach (_0CF0 item in _೨)
		{
			if (_0CDB == eContentOrientation.Horizontal)
			{
				if ((_0CD8 && _೦.Width > item.ೲ.Width) || (_0CD7 && item.ೲ.Width > _೦.Width))
				{
					if (_0CE2 && (double)_೦.Width < (double)item.ೲ.Width * 0.75)
					{
						array2[item._0CF3].ܐ = (int)Math.Floor((float)(_೦.Width - item._0CF4 * _0CD6) / (float)item._0CF4);
						array2[item._0CF3]._0CF8 = true;
					}
					else
					{
						array2[item._0CF3].ܐ = (_೦.Width - item._0CF4 * _0CD6 - item.ೲ.Width) / item._0CF4;
					}
					array2[item._0CF3]._0CF6 = (float)(_೦.Width - item._0CF4 * _0CD6) / (float)item.ೲ.Width;
					_೧.Width = _೦.Width;
				}
				if (_0CDA && _೦.Height > _೧.Height && _೨.Count == 1)
				{
					array2[item._0CF3]._0711 = (_೦.Height - item.ೲ.Height) / _೨.Count;
				}
			}
			else
			{
				if ((_0CD8 && _೦.Height > item.ೲ.Height) || (_0CD7 && item.ೲ.Height > _೦.Height))
				{
					if (_0CE2 && (double)_೦.Width < (double)item.ೲ.Width * 0.75)
					{
						array2[item._0CF3]._0711 = (int)Math.Floor((float)(_೦.Height - item._0CF4 * _0CD6) / (float)item._0CF4);
						array2[item._0CF3]._0CF8 = true;
					}
					else
					{
						array2[item._0CF3]._0711 = (_೦.Height - item._0CF4 * _0CD6 - item.ೲ.Height) / item._0CF4;
					}
					array2[item._0CF3]._0CF7 = (float)(_೦.Height - item._0CF4 * _0CD6) / (float)item.ೲ.Height;
					_೧.Height = _೦.Height;
				}
				if (_0CD9 && _೦.Width > _೧.Width && _೨.Count == 1)
				{
					array2[item._0CF3].ܐ = (_೦.Width - item.ೲ.Width) / _೨.Count;
				}
			}
			if (_0CDB == eContentOrientation.Horizontal && !_0CD8 && ((_೦.Width > _೧.Width && _0CD7) || !_0CD7))
			{
				switch (_0CDC)
				{
				case eContentAlignment.Right:
					if (_೦.Width > item.ೲ.Width)
					{
						array[item._0CF3].X = _೦.Width - item.ೲ.Width;
					}
					break;
				case eContentAlignment.Center:
					if (_೦.Width > item.ೲ.Width)
					{
						array[item._0CF3].X = (_೦.Width - item.ೲ.Width) / 2;
					}
					break;
				}
			}
			if (_0CDB != eContentOrientation.Vertical || _0CD8 || ((_೦.Height <= _೧.Height || !_0CD7) && _0CD7))
			{
				continue;
			}
			switch (_0CDD)
			{
			case eContentVerticalAlignment.Bottom:
				if (_೦.Height > item.ೲ.Height)
				{
					array[item._0CF3].Y = _೦.Height - item.ೲ.Height;
				}
				break;
			case eContentVerticalAlignment.Middle:
				if (_೦.Height > item.ೲ.Height)
				{
					array[item._0CF3].Y = (_೦.Height - item.ೲ.Height) / 2;
				}
				break;
			}
		}
		if (_0CD9 && _೦.Width > _೧.Width && _0CDB == eContentOrientation.Vertical)
		{
			_೧.Width = _೦.Width;
		}
		else if (_0CDA && _೦.Height > _೧.Height && _0CDB == eContentOrientation.Horizontal)
		{
			_೧.Height = _೦.Height;
		}
		if (_0CDB == eContentOrientation.Horizontal)
		{
			foreach (_0CF0 item2 in _೨)
			{
				foreach (IBlock item3 in item2.ೱ)
				{
					if (!item3.Visible)
					{
						continue;
					}
					Rectangle bounds = item3.Bounds;
					if (_0CDF && item2.ೲ.Height > 0)
					{
						bounds.Height = item2.ೲ.Height;
					}
					bounds.Offset(array[item2._0CF3]);
					if (_0CDD == eContentVerticalAlignment.Middle)
					{
						if (ೞ == eContentVerticalAlignment.Middle)
						{
							bounds.Offset(0, (_೦.Height - _೧.Height + (item2.ೲ.Height - bounds.Height)) / 2);
						}
						else
						{
							bounds.Offset(0, (_೦.Height - _೧.Height) / 2);
						}
						if (ೞ == eContentVerticalAlignment.Bottom)
						{
							bounds.Offset(0, item2.ೲ.Height - bounds.Height);
						}
					}
					else if (_0CDD == eContentVerticalAlignment.Bottom)
					{
						bounds.Offset(0, _೦.Height - _೧.Height);
					}
					if (_0CDD != eContentVerticalAlignment.Middle)
					{
						if (ೞ == eContentVerticalAlignment.Middle)
						{
							bounds.Offset(0, (item2.ೲ.Height - bounds.Height) / 2);
						}
						else if (ೞ == eContentVerticalAlignment.Bottom)
						{
							bounds.Offset(0, item2.ೲ.Height - bounds.Height);
						}
					}
					if (array2[item2._0CF3].ܐ != 0)
					{
						if (_0CE2)
						{
							int num = (array2[item2._0CF3]._0CF8 ? array2[item2._0CF3].ܐ : ((int)Math.Floor((float)bounds.Width * array2[item2._0CF3]._0CF6)));
							array[item2._0CF3].X += num - bounds.Width;
							bounds.Width = num;
						}
						else
						{
							bounds.Width += array2[item2._0CF3].ܐ;
							array[item2._0CF3].X += array2[item2._0CF3].ܐ;
						}
					}
					bounds.Height += array2[item2._0CF3]._0711;
					item3.Bounds = bounds;
					rectangle = ((!rectangle.IsEmpty) ? Rectangle.Union(rectangle, item3.Bounds) : item3.Bounds);
				}
				if (!_0CE2 && array2[item2._0CF3].ܐ != 0 && _೦.Width - (item2.ೲ.Width + array2[item2._0CF3].ܐ * item2.ೱ.Count) != 0)
				{
					Rectangle bounds2 = ((IBlock)item2.ೱ[item2.ೱ.Count - 1]).Bounds;
					bounds2.Width += _೦.Width - (item2.ೲ.Width + array2[item2._0CF3].ܐ * item2.ೱ.Count);
					((IBlock)item2.ೱ[item2.ೱ.Count - 1]).Bounds = bounds2;
				}
				if (!_0CD8 && _0CDC != 0 && __0CE3)
				{
					rectangle.Width += rectangle.X - _೦.X;
					rectangle.X = _೦.X;
				}
			}
		}
		else
		{
			foreach (_0CF0 item4 in _೨)
			{
				foreach (IBlock item5 in item4.ೱ)
				{
					if (!item5.Visible)
					{
						continue;
					}
					Rectangle bounds3 = item5.Bounds;
					if (_0CDF && item4.ೲ.Width > 0)
					{
						bounds3.Width = item4.ೲ.Width - item5.Margin.Horizontal;
					}
					bounds3.Offset(array[item4._0CF3]);
					if (_0CDC == eContentAlignment.Center)
					{
						bounds3.Offset((_೦.Width - _೧.Width + (item4.ೲ.Width - bounds3.Width)) / 2, 0);
					}
					else if (_0CDC == eContentAlignment.Right)
					{
						bounds3.Offset(_೦.Width - _೧.Width + item4.ೲ.Width - bounds3.Width, 0);
					}
					bounds3.Width += array2[item4._0CF3].ܐ;
					if (array2[item4._0CF3]._0711 != 0)
					{
						if (_0CE2)
						{
							int num2 = (array2[item4._0CF3]._0CF8 ? array2[item4._0CF3]._0711 : ((int)Math.Floor((float)bounds3.Height * array2[item4._0CF3]._0CF7)));
							array[item4._0CF3].Y += num2 - bounds3.Height;
							bounds3.Height = num2;
						}
						else
						{
							bounds3.Height += array2[item4._0CF3]._0711;
							array[item4._0CF3].Y += array2[item4._0CF3]._0711;
						}
					}
					item5.Bounds = bounds3;
					if (rectangle.IsEmpty)
					{
						bounds3.Y -= item5.Margin.Top;
						bounds3.X -= item5.Margin.Left;
						bounds3.Width += item5.Margin.Horizontal;
						bounds3.Height += item5.Margin.Vertical;
						rectangle = bounds3;
					}
					else
					{
						rectangle = Rectangle.Union(rectangle, item5.Bounds);
					}
				}
				if (!_0CE2 && array2[item4._0CF3]._0711 != 0 && _೦.Height - (item4.ೲ.Height + array2[item4._0CF3]._0711 * item4.ೱ.Count) != 0)
				{
					Rectangle bounds4 = ((IBlock)item4.ೱ[item4.ೱ.Count - 1]).Bounds;
					bounds4.Height += _೦.Height - (item4.ೲ.Height + array2[item4._0CF3]._0711 * item4.ೱ.Count);
					((IBlock)item4.ೱ[item4.ೱ.Count - 1]).Bounds = bounds4;
				}
			}
		}
		return rectangle;
	}

	private Point _೩(IBlock _೪, Point _೫, ref bool _೬)
	{
		if (_0CD4 != null)
		{
			LayoutManagerPositionEventArgs layoutManagerPositionEventArgs = new LayoutManagerPositionEventArgs();
			layoutManagerPositionEventArgs.Block = _೪;
			layoutManagerPositionEventArgs.CurrentPosition = _೫;
			layoutManagerPositionEventArgs.SwitchToNewLine = _೬;
			_0CD4(this, layoutManagerPositionEventArgs);
			_೬ = layoutManagerPositionEventArgs.SwitchToNewLine;
			if (layoutManagerPositionEventArgs.Cancel)
			{
				return layoutManagerPositionEventArgs.NextPosition;
			}
		}
		if (_0CDB == eContentOrientation.Horizontal)
		{
			_೫.X += _೪.Bounds.Width + _0CD6 + _೪.Margin.Horizontal;
		}
		else
		{
			_೫.Y += _೪.Bounds.Height + _0CD6 + _೪.Margin.Vertical;
		}
		return _೫;
	}

	private Rectangle _೭(Rectangle _೦, Rectangle _೮, IBlock[] _೯)
	{
		_ = _೮.X;
		_ = _೦.X;
		if (_೮.Width < _೦.Width)
		{
			_೮.X = _೦.Right - (_೮.X - _೦.X + _೮.Width);
		}
		else if (_೮.Width > _೦.Width)
		{
			_೦.Width = _೮.Width;
		}
		foreach (IBlock block in _೯)
		{
			if (block.Visible)
			{
				Rectangle bounds = block.Bounds;
				block.Bounds = new Rectangle(_೦.Right - (bounds.X - _೦.X + bounds.Width), bounds.Y, bounds.Width, bounds.Height);
			}
		}
		return _೮;
	}
}
