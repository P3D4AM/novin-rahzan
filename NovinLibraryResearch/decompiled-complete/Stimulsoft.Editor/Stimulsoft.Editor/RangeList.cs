using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Stimulsoft.Editor;

public class RangeList : SortList
{
	private class RangeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			IRange range = (IRange)x;
			IRange range2 = (IRange)y;
			int num = range.StartPoint.Y - range2.StartPoint.Y;
			if (num == 0)
			{
				num = range.StartPoint.X - range2.StartPoint.X;
			}
			return num;
		}
	}

	private class PointComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point startPoint = ((SortRange)x).Range.StartPoint;
			Point point = (Point)y;
			int num = startPoint.Y - point.Y;
			if (num == 0)
			{
				num = startPoint.X - point.X;
			}
			return num;
		}
	}

	private class PointHitTest : IComparer
	{
		public int Compare(object x, object y)
		{
			SortRange sortRange = (SortRange)x;
			Point startPoint = sortRange.Range.StartPoint;
			Point endPoint = sortRange.Range.EndPoint;
			Point point = (Point)y;
			if (point.Y > startPoint.Y || (point.Y == startPoint.Y && point.X >= startPoint.X))
			{
				if (point.Y >= endPoint.Y && (point.Y != endPoint.Y || point.X >= endPoint.X))
				{
					return -1;
				}
				return 0;
			}
			return 1;
		}
	}

	private class InclusiveLineHitTest : IComparer
	{
		public int Compare(object x, object y)
		{
			SortRange sortRange = (SortRange)x;
			Point startPoint = sortRange.Range.StartPoint;
			Point endPoint = sortRange.Range.EndPoint;
			int num = (int)y;
			if (num > startPoint.Y)
			{
				if (num > endPoint.Y)
				{
					return -1;
				}
				return 0;
			}
			return 1;
		}
	}

	private class LineHitTest : IComparer
	{
		public int Compare(object x, object y)
		{
			SortRange sortRange = (SortRange)x;
			Point startPoint = sortRange.Range.StartPoint;
			Point endPoint = sortRange.Range.EndPoint;
			int num = (int)y;
			if (num >= startPoint.Y)
			{
				if (num > endPoint.Y)
				{
					return -1;
				}
				return 0;
			}
			return 1;
		}
	}

	protected class SortRange
	{
		private SortList ranges;

		private IRange range;

		private int index;

		public SortList Ranges => ranges;

		public IRange Range => range;

		public int Index => index;

		public SortRange(IRange range, int index)
		{
			this.index = index;
			this.range = range;
			ranges = new SortList();
		}

		public void Clear()
		{
			foreach (SortRange range in ranges)
			{
				range.Clear();
			}
			ranges.Clear();
		}

		public bool Contains(IRange range)
		{
			if (this.range != null)
			{
				if (this.range.StartPoint.Y < range.StartPoint.Y || (this.range.StartPoint.Y == range.StartPoint.Y && this.range.StartPoint.X < range.StartPoint.X))
				{
					if (this.range.EndPoint.Y <= range.EndPoint.Y)
					{
						if (this.range.EndPoint.Y == range.EndPoint.Y)
						{
							return this.range.EndPoint.X >= range.EndPoint.X;
						}
						return false;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		public int Add(SortRange range)
		{
			return ranges.Add(range);
		}

		public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, RangeList list, IComparer comparer)
		{
			bool result = false;
			Point point = new Point(x, y);
			if (!ranges.FindLast(point, out var num, comparer))
			{
				num--;
			}
			for (int i = Math.Max(num, 0); i < ranges.Count; i++)
			{
				SortRange sortRange = (SortRange)ranges[i];
				point = sortRange.Range.StartPoint;
				if (deltaY == 0 && point.Y > y)
				{
					break;
				}
				if (list.UpdatePosition(sortRange.Range, x, y, deltaX, deltaY))
				{
					result = true;
				}
				if (sortRange.PositionChanged(x, y, deltaX, deltaY, list, comparer))
				{
					result = true;
				}
			}
			return result;
		}

		public virtual bool BlockDeleting(Rectangle rect, RangeList list, IComparer comparer)
		{
			bool result = false;
			if (ranges.FindLast(new Point(rect.Right, rect.Bottom), out var num, comparer))
			{
				num++;
			}
			for (int num2 = Math.Min(num, ranges.Count - 1); num2 >= 0; num2--)
			{
				SortRange sortRange = (SortRange)ranges[num2];
				IRange range = sortRange.Range;
				if (range.EndPoint.Y < rect.Top)
				{
					break;
				}
				if (SortList.InsideBlock(range.StartPoint, rect) && SortList.InsideBlock(new Point(Math.Max(range.EndPoint.X - 1, 0), range.EndPoint.Y), rect))
				{
					if (sortRange.Index < list.Count)
					{
						list.RemoveAt(sortRange.Index);
					}
					ranges.RemoveAt(num2);
					result = true;
				}
				else if (sortRange.BlockDeleting(rect, list, comparer))
				{
					result = true;
				}
			}
			return result;
		}
	}

	private SortRange topRange;

	private IComparer rangeComparer;

	private IComparer pointComparer;

	private IComparer pointHitTest;

	private IComparer inclusiveLineHitTest;

	private IComparer lineHitTest;

	private int updateCount;

	private bool needSort;

	protected SortRange TopRange => topRange;

	public int UpdateCount => updateCount;

	public RangeList()
	{
		topRange = new SortRange(null, -1);
		rangeComparer = new RangeComparer();
		pointComparer = new PointComparer();
		pointHitTest = new PointHitTest();
		lineHitTest = new LineHitTest();
		inclusiveLineHitTest = new InclusiveLineHitTest();
	}

	protected void SortLevels()
	{
		topRange.Clear();
		List<SortRange> list = new List<SortRange>();
		SortRange sortRange = topRange;
		for (int i = 0; i < Count; i++)
		{
			IRange range = (IRange)this[i];
			SortRange sortRange2 = new SortRange(range, i);
			while (list.Count > 0 && !sortRange.Contains(range))
			{
				sortRange = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
			list.Add(sortRange);
			sortRange.Add(sortRange2);
			sortRange = sortRange2;
		}
	}

	protected int FindRangeIndex(Point point, IComparer comparer)
	{
		SortRange sortRange = topRange;
		int result = -1;
		int index;
		while (sortRange.Ranges.FindLast(point, out index, comparer))
		{
			sortRange = (SortRange)sortRange.Ranges[index];
			result = sortRange.Index;
		}
		return result;
	}

	protected int FindRangeIndex(int index, IComparer comparer)
	{
		SortRange sortRange = topRange;
		int result = -1;
		int index2;
		while (sortRange.Ranges.FindLast(index, out index2, comparer))
		{
			sortRange = (SortRange)sortRange.Ranges[index2];
			result = sortRange.Index;
		}
		return result;
	}

	protected IRange FindRange(Point point, IComparer comparer)
	{
		SortRange sortRange = topRange;
		IRange result = null;
		int index;
		while (sortRange.Ranges.FindLast(point, out index, comparer))
		{
			sortRange = (SortRange)sortRange.Ranges[index];
			result = sortRange.Range;
		}
		return result;
	}

	protected IRange FindRange(int index, IComparer comparer)
	{
		SortRange sortRange = topRange;
		IRange result = null;
		int index2;
		while (sortRange.Ranges.FindLast(index, out index2, comparer))
		{
			sortRange = (SortRange)sortRange.Ranges[index2];
			result = sortRange.Range;
		}
		return result;
	}

	protected virtual bool UpdatePosition(IRange range, int x, int y, int deltaX, int deltaY)
	{
		Point pt = range.StartPoint;
		bool result = false;
		if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: false))
		{
			range.StartPoint = pt;
			result = true;
		}
		pt = range.EndPoint;
		if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: true))
		{
			range.EndPoint = pt;
			result = true;
		}
		return result;
	}

	public virtual bool BlockDeleting(Rectangle rect)
	{
		bool flag = false;
		BeginUpdate();
		try
		{
			flag = topRange.BlockDeleting(rect, this, pointComparer);
			if (flag)
			{
				SortLevels();
			}
		}
		finally
		{
			EndUpdate(flag);
		}
		return flag;
	}

	public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		return topRange.PositionChanged(x, y, deltaX, deltaY, this, pointComparer);
	}

	public override void Clear()
	{
		base.Clear();
		topRange.Clear();
	}

	public void CopyFrom(IList ranges)
	{
		BeginUpdate();
		try
		{
			Clear();
			foreach (IRange range in ranges)
			{
				Add(range);
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	public override void RemoveAt(int index)
	{
		base.RemoveAt(index);
		if (updateCount == 0)
		{
			SortLevels();
		}
	}

	public override int Add(object value)
	{
		if (updateCount == 0)
		{
			IRange range = (IRange)value;
			if (FindLast(range, out var index, rangeComparer))
			{
				RemoveAt(index);
			}
			Insert(index, range);
			return index;
		}
		needSort = true;
		return base.Add(value);
	}

	public IRange FindRange(Point point)
	{
		return FindRange(point, pointHitTest);
	}

	public IRange FindInclusiveRange(int index)
	{
		return FindRange(index, inclusiveLineHitTest);
	}

	public IRange FindRange(int index)
	{
		return FindRange(index, lineHitTest);
	}

	public IRange FindExactRange(Point point)
	{
		IRange range = FindRange(point, pointHitTest);
		if (range == null || !range.StartPoint.Equals((object?)point))
		{
			return null;
		}
		return range;
	}

	public IRange FindExactRange(int index)
	{
		IRange range = FindRange(index, lineHitTest);
		if (range == null || range.StartPoint.Y != index)
		{
			return null;
		}
		return range;
	}

	public void RemoveRange(Point point)
	{
		List<int> list = new List<int>();
		SortRange sortRange = topRange;
		int index;
		while (sortRange.Ranges.FindLast(point, out index, pointHitTest))
		{
			if (sortRange.Index >= 0)
			{
				list.Add(sortRange.Index);
			}
			sortRange = (SortRange)sortRange.Ranges[index];
		}
		if (list.Count <= 0)
		{
			return;
		}
		BeginUpdate();
		try
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				RemoveAt(list[num]);
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	public void RemoveRange(int index)
	{
		List<int> list = new List<int>();
		SortRange sortRange = topRange;
		int index2;
		while (sortRange.Ranges.FindLast(index, out index2, lineHitTest))
		{
			if (sortRange.Index >= 0)
			{
				list.Add(sortRange.Index);
			}
			sortRange = (SortRange)sortRange.Ranges[index2];
		}
		if (list.Count <= 0)
		{
			return;
		}
		BeginUpdate();
		try
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				RemoveAt(list[num]);
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	public int GetRanges(IList ranges)
	{
		ranges.Clear();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IRange value = (IRange)enumerator.Current;
				ranges.Add(value);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return ranges.Count;
	}

	public IList GetRanges()
	{
		ArrayList arrayList = new ArrayList();
		GetRanges(arrayList);
		return arrayList;
	}

	public int GetRanges(IList ranges, Point startpoint, Point endpoint)
	{
		ranges.Clear();
		SortRange x = new SortRange(new Range(startpoint, endpoint), 0);
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IRange range = (IRange)enumerator.Current;
				if (pointHitTest.Compare(x, range.StartPoint) == 0 && pointHitTest.Compare(x, range.EndPoint) == 0)
				{
					ranges.Add(range);
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return ranges.Count;
	}

	public int GetRanges(IList ranges, Point point)
	{
		ranges.Clear();
		SortRange sortRange = topRange;
		int index;
		while (sortRange.Ranges.FindLast(point, out index, pointHitTest))
		{
			sortRange = (SortRange)sortRange.Ranges[index];
			if (sortRange.Range != null)
			{
				ranges.Add(sortRange.Range);
			}
		}
		return ranges.Count;
	}

	public int GetExactRanges(IList ranges, int index)
	{
		ranges.Clear();
		SortRange sortRange = topRange;
		int index2;
		while (sortRange.Ranges.FindLast(index, out index2, lineHitTest))
		{
			sortRange = (SortRange)sortRange.Ranges[index2];
			if (sortRange.Range != null && sortRange.Range.StartPoint.Y == index)
			{
				ranges.Add(sortRange.Range);
			}
		}
		return ranges.Count;
	}

	public int GetRanges(IList ranges, int index)
	{
		ranges.Clear();
		SortRange sortRange = topRange;
		int index2;
		while (sortRange.Ranges.FindLast(index, out index2, lineHitTest))
		{
			sortRange = (SortRange)sortRange.Ranges[index2];
			if (sortRange.Range != null)
			{
				ranges.Add(sortRange.Range);
			}
		}
		return ranges.Count;
	}

	public override void Insert(int index, object value)
	{
		base.Insert(index, value);
		if (updateCount == 0)
		{
			SortLevels();
		}
	}

	public int BeginUpdate()
	{
		updateCount++;
		return updateCount;
	}

	public int EndUpdate(bool update)
	{
		updateCount--;
		if (updateCount == 0 && update)
		{
			Update();
		}
		return updateCount;
	}

	public int EndUpdate()
	{
		return EndUpdate(update: true);
	}

	public virtual void Update()
	{
		if (needSort)
		{
			Sort();
		}
	}

	public override void Sort()
	{
		if (updateCount == 0)
		{
			needSort = false;
			Sort(rangeComparer);
			SortLevels();
		}
	}
}
