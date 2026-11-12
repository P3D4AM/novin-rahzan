using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

internal class CollapsedList : RangeList
{
	private class RealComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point endPoint = ((SortRange)x).Range.EndPoint;
			Point point = (Point)y;
			if (point.Y < endPoint.Y || (point.Y == endPoint.Y && point.X < endPoint.X))
			{
				return 1;
			}
			return 0;
		}
	}

	private class DisplayComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			CollapsedRange collapsedRange = ((SortRange)x).Range as CollapsedRange;
			int num = collapsedRange.EndPoint.Y + collapsedRange.DeltaY;
			int num2 = collapsedRange.EndPoint.X + collapsedRange.DeltaX - (collapsedRange.DisplayText.Length - 1);
			Point point = (Point)y;
			if (point.Y < num || (point.Y == num && point.X < num2))
			{
				return 1;
			}
			return 0;
		}
	}

	private int collapsedCount;

	private IComparer realComparer;

	private IComparer displayComparer;

	public int CollapsedCount
	{
		get
		{
			return collapsedCount;
		}
		set
		{
			collapsedCount = value;
		}
	}

	public CollapsedList()
	{
		realComparer = new RealComparer();
		displayComparer = new DisplayComparer();
	}

	protected override bool UpdatePosition(IRange range, int x, int y, int deltaX, int deltaY)
	{
		int num = range.EndPoint.X - range.StartPoint.X;
		int num2 = range.EndPoint.Y - range.StartPoint.Y;
		if (base.UpdatePosition(range, x, y, deltaX, deltaY))
		{
			if (range.EndPoint.X - range.StartPoint.X == num)
			{
				return range.EndPoint.Y - range.StartPoint.Y != num2;
			}
			return true;
		}
		return false;
	}

	public override int Add(object value)
	{
		int num = base.Add(value);
		base.TopRange.Add(new SortRange((IRange)value, num));
		return num;
	}

	public void GetRealPoint(ref Point point, bool rangeStart)
	{
		if (!(FindRange(point, displayComparer) is CollapsedRange { EndPoint: var endPoint } collapsedRange))
		{
			return;
		}
		int num = endPoint.Y + collapsedRange.DeltaY;
		int num2 = collapsedRange.EndPoint.X + collapsedRange.DeltaX;
		if (point.Y == num && point.X != int.MaxValue)
		{
			int num3 = num2;
			if (point.X < num3 && rangeStart)
			{
				point.X = num3 - collapsedRange.DisplayText.Length;
				point.Y = collapsedRange.StartPoint.Y;
				return;
			}
			if (point.X < num3)
			{
				point.X = num3;
			}
			point.X += collapsedRange.EndPoint.X - num3;
		}
		if (point.Y != int.MaxValue)
		{
			point.Y += collapsedRange.EndPoint.Y - num;
		}
	}

	public void GetDisplayPoint(ref Point point)
	{
		if (FindRange(point, realComparer) is CollapsedRange collapsedRange)
		{
			if (point.Y == collapsedRange.EndPoint.Y && point.X != int.MaxValue)
			{
				point.X += collapsedRange.DeltaX;
			}
			if (point.Y != int.MaxValue)
			{
				point.Y += collapsedRange.DeltaY;
			}
		}
	}
}
