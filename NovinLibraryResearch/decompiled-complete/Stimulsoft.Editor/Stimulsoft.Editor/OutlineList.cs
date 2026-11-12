using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

internal class OutlineList : RangeList
{
	private CollapsedList collapsedList;

	private DisplayStrings owner;

	public DisplayStrings Owner => owner;

	public IList CollapsedList => collapsedList;

	public int CollapsedCount => collapsedList.CollapsedCount;

	public OutlineList(DisplayStrings owner)
	{
		this.owner = owner;
		collapsedList = new CollapsedList();
	}

	public override bool BlockDeleting(Rectangle rect)
	{
		bool flag = base.BlockDeleting(rect);
		if (flag)
		{
			UpdateRange(null, update: false);
		}
		return flag;
	}

	public override bool PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		bool flag = base.PositionChanged(x, y, deltaX, deltaY);
		if (flag && collapsedList.PositionChanged(x, y, deltaX, deltaY))
		{
			UpdateRange(null, update: false);
		}
		return flag;
	}

	public bool IsVisible(Point point)
	{
		return collapsedList.FindRange(point) == null;
	}

	public bool IsVisible(int index)
	{
		return collapsedList.FindInclusiveRange(index) == null;
	}

	public void CheckVisible(ref int index)
	{
		IRange range = collapsedList.FindInclusiveRange(index);
		if (range != null)
		{
			index = range.StartPoint.Y;
		}
	}

	public override void Clear()
	{
		base.Clear();
		collapsedList.Clear();
		UpdateRange(null, update: true);
	}

	public override void Update()
	{
		base.Update();
		UpdateRange(null, update: true);
	}

	public void UpdateRange(IOutlineRange rng, bool update)
	{
		if (base.UpdateCount > 0)
		{
			return;
		}
		Point point = Point.Empty;
		int num = 0;
		int num2 = 0;
		collapsedList.Clear();
		int count = owner.Lines.Count;
		collapsedList.BeginUpdate();
		try
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IOutlineRange outlineRange = (IOutlineRange)enumerator.Current;
					if (!outlineRange.Visible && !outlineRange.IsEmpty && IsVisible(outlineRange.StartPoint))
					{
						num2 += Math.Min(outlineRange.EndPoint.Y, count - 1) - Math.Min(outlineRange.StartPoint.Y, count - 1);
						if (outlineRange.StartPoint.Y != point.Y)
						{
							num = 0;
						}
						num += outlineRange.EndPoint.X - outlineRange.StartPoint.X - outlineRange.DisplayText.Length;
						collapsedList.Add(new CollapsedRange(null, outlineRange.StartPoint, outlineRange.EndPoint, -num, -num2, 0, outlineRange.DisplayText));
						point = outlineRange.EndPoint;
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
		}
		finally
		{
			collapsedList.EndUpdate();
		}
		collapsedList.CollapsedCount = num2;
		if (update)
		{
			owner.CollapseChanged(rng);
		}
	}

	public void GetRealPoint(ref Point point, bool rangeStart)
	{
		collapsedList.GetRealPoint(ref point, rangeStart);
	}

	public void GetDisplayPoint(ref Point point)
	{
		IRange range = collapsedList.FindRange(point);
		if (range != null)
		{
			point = range.StartPoint;
		}
		collapsedList.GetDisplayPoint(ref point);
	}

	public IRange FindCollapsedRange(Point point)
	{
		return collapsedList.FindExactRange(point);
	}

	public int GetCollapsedRanges(int index, IList ranges)
	{
		return collapsedList.GetExactRanges(ranges, index);
	}

	public IRange FindCollapsedRange(int index)
	{
		return collapsedList.FindExactRange(index);
	}

	public int LineToDisplayLine(int index)
	{
		Point point = new Point(0, index);
		GetDisplayPoint(ref point);
		return point.Y;
	}

	public int DisplayLineToLine(int index)
	{
		Point point = new Point(0, index);
		GetRealPoint(ref point, rangeStart: false);
		return point.Y;
	}
}
