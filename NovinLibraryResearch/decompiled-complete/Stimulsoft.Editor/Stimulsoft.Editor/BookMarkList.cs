using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class BookMarkList : SortList
{
	private class BookMarkComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			IBookMark bookMark = (IBookMark)x;
			IBookMark bookMark2 = (IBookMark)y;
			int num = bookMark.Line - bookMark2.Line;
			if (num == 0)
			{
				num = bookMark.Pos - bookMark2.Pos;
			}
			if (num == 0)
			{
				num = bookMark.Index - bookMark2.Index;
			}
			return num;
		}
	}

	private class LineComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((IBookMark)x).Line - (int)y;
		}
	}

	private class PointComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			IBookMark bookMark = (IBookMark)x;
			Point point = (Point)y;
			int num = bookMark.Line - point.Y;
			if (num == 0)
			{
				num = bookMark.Pos - point.X;
			}
			return num;
		}
	}

	private class RangeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			IBookMark bookMark = (IBookMark)x;
			Point startPoint = ((IRange)y).StartPoint;
			Point endPoint = ((IRange)y).EndPoint;
			if (bookMark.Line < startPoint.Y || (bookMark.Line == startPoint.Y && bookMark.Pos < startPoint.X))
			{
				return -1;
			}
			if (bookMark.Line > endPoint.Y || (bookMark.Line == endPoint.Y && bookMark.Pos >= endPoint.X))
			{
				return 1;
			}
			return 0;
		}
	}

	protected IComparer lineComparer;

	protected IComparer pointComparer;

	protected IComparer bookMarkComparer;

	protected IComparer rangeComparer;

	public new IBookMark this[int index] => base[index] as IBookMark;

	public BookMarkList()
	{
		lineComparer = new LineComparer();
		pointComparer = new PointComparer();
		bookMarkComparer = new BookMarkComparer();
		rangeComparer = new RangeComparer();
	}

	protected internal virtual BookMark NewBookMark(int line, int pos, int index)
	{
		return new BookMark(line, pos, index);
	}

	protected virtual bool UpdatePosition(int x, int y, int deltaX, int deltaY, BookMark bookmark)
	{
		Point pt = new Point(bookmark.Pos, bookmark.Line);
		if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, pt.X != 0))
		{
			if (bookmark.Index != int.MaxValue)
			{
				bookmark.Pos = pt.X;
			}
			bookmark.Line = pt.Y;
			return true;
		}
		return false;
	}

	protected virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer lineComparer, IComparer sortComparer)
	{
		int index = 0;
		if (lineComparer != null)
		{
			FindFirst(y, out index, lineComparer);
		}
		if (index >= 0)
		{
			bool flag = false;
			for (int i = index; i < Count; i++)
			{
				if (UpdatePosition(x, y, deltaX, deltaY, (BookMark)this[i]))
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (sortComparer != null)
				{
					Sort(sortComparer);
				}
				return true;
			}
		}
		return false;
	}

	protected internal virtual bool ShouldDelete(IBookMark bm, Rectangle rect)
	{
		if (bm.Index == int.MaxValue)
		{
			if (SortList.InsideBlock(new Point(0, bm.Line), rect))
			{
				if (rect.Width != int.MaxValue)
				{
					return SortList.InsideBlock(new Point(int.MaxValue, bm.Line), rect);
				}
				return true;
			}
			return false;
		}
		return SortList.InsideBlock(new Point(bm.Pos, bm.Line), rect);
	}

	protected internal int InternalGet(int line)
	{
		if (FindFirst(line, out var index, lineComparer))
		{
			return this[index].Index;
		}
		return -1;
	}

	protected internal int InternalGet(Point startPoint, Point endPoint, IComparer comparer)
	{
		Range obj = new Range(startPoint, endPoint);
		if (FindFirst(obj, out var index, comparer))
		{
			return this[index].Index;
		}
		return -1;
	}

	protected internal int InternalGet(Point startPoint, Point endPoint, IList list, IComparer comparer)
	{
		list.Clear();
		Range range = new Range(startPoint, endPoint);
		if (FindFirst(range, out var index, comparer))
		{
			for (int i = index; i < Count && comparer.Compare(this[i], range) == 0; i++)
			{
				list.Add(this[i]);
			}
		}
		return list.Count;
	}

	protected internal void InternalSet(IBookMark bookMark)
	{
		if (FindFirst(new Point(bookMark.Pos, bookMark.Line), out var index, pointComparer))
		{
			RemoveAt(index);
		}
		Insert(index, bookMark);
	}

	protected internal void InternalSet(int line, int index)
	{
		if (FindFirst(line, out var index2, lineComparer))
		{
			RemoveAt(index2);
		}
		Insert(index2, NewBookMark(line, 0, index));
	}

	protected internal void InternalSet(int line, int pos, int index)
	{
		if (FindFirst(new Point(pos, line), out var index2, pointComparer))
		{
			RemoveAt(index2);
		}
		Insert(index2, NewBookMark(line, pos, index));
	}

	protected internal void InternalClear(int index)
	{
		int num = FindByIndex(index);
		if (num >= 0)
		{
			RemoveAt(num);
		}
	}

	protected internal void InternalClearLine(int line)
	{
		int index = 0;
		if (FindLast(line, out index, lineComparer))
		{
			int num = index;
			while (num >= 0 && this[index].Line == line)
			{
				RemoveAt(num);
				num--;
			}
		}
	}

	protected internal virtual bool BlockDeleting(Rectangle rect)
	{
		bool result = false;
		FindLast(rect.Bottom, out var index, lineComparer);
		if (index >= 0)
		{
			for (int num = Math.Min(index, Count - 1); num >= 0; num--)
			{
				IBookMark bookMark = this[num];
				if (bookMark.Line < rect.Top)
				{
					break;
				}
				if (ShouldDelete(bookMark, rect))
				{
					result = true;
					RemoveAt(num);
				}
			}
		}
		return result;
	}

	protected internal virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		return PositionChanged(x, y, deltaX, deltaY, lineComparer, bookMarkComparer);
	}

	public int FindByLine(int line)
	{
		if (FindFirst(line, out var index, lineComparer))
		{
			return index;
		}
		return -1;
	}

	public int FindByIndex(int index)
	{
		for (int i = 0; i < Count; i++)
		{
			if (this[i].Index == index)
			{
				return i;
			}
		}
		return -1;
	}
}
