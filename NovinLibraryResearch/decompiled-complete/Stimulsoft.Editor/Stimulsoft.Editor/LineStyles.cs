using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class LineStyles : BookMarkList, ILineStyles, IList, ICollection, IEnumerable
{
	private class StyleComparer : IComparer
	{
		public virtual int Compare(object x, object y)
		{
			LStyle lStyle = (LStyle)x;
			LStyle lStyle2 = (LStyle)y;
			int num = lStyle.Line - lStyle2.Line;
			if (num == 0)
			{
				num = lStyle.Pos - lStyle2.Pos;
			}
			if (num == 0)
			{
				num = lStyle.Priority - lStyle2.Priority;
			}
			return num;
		}
	}

	private class LineStyleComparer : StyleComparer
	{
		public override int Compare(object x, object y)
		{
			int num = base.Compare(x, y);
			if (num == 0)
			{
				num = ((LStyle)x).Index - ((LStyle)x).Index;
			}
			return num;
		}
	}

	private ITextSource owner;

	protected IComparer styleComparer;

	protected IComparer lineStyleComparer;

	public LineStyles()
	{
		styleComparer = new StyleComparer();
		lineStyleComparer = new LineStyleComparer();
	}

	public LineStyles(ITextSource owner)
		: this()
	{
		this.owner = owner;
	}

	public void Assign(ILineStyles source)
	{
		owner.BeginUpdate(UpdateReason.Other);
		try
		{
			Clear();
			foreach (LStyle item in source)
			{
				Add(NewLineStyle(item.Line, item.Pos, item.Index, item.Priority, item.Range));
			}
			owner.LinesChanged(0, int.MaxValue);
			owner.State |= NotifyState.BookMarkChanged;
		}
		finally
		{
			owner.EndUpdate();
		}
	}

	protected virtual LStyle NewLineStyle(int line, int ch, int index, int priority, IRange range)
	{
		return new LStyle(line, ch, index, priority, range);
	}

	protected internal override BookMark NewBookMark(int line, int ch, int index)
	{
		return NewLineStyle(line, ch, index, -1, null);
	}

	protected internal override bool PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		return PositionChanged(x, y, deltaX, deltaY, null, lineStyleComparer);
	}

	protected internal override bool ShouldDelete(IBookMark bm, Rectangle rect)
	{
		Point pt = new Point(Math.Max(bm.Pos, 0), bm.Line);
		Point pt2 = ((bm is LStyle && ((LStyle)bm).Range != null) ? ((LStyle)bm).Range.EndPoint : new Point(int.MaxValue, bm.Line));
		if (SortList.InsideBlock(pt, rect))
		{
			return SortList.InsideBlock(pt2, rect, checkMaxInt: true);
		}
		return false;
	}

	protected override bool UpdatePosition(int x, int y, int deltaX, int deltaY, BookMark bookmark)
	{
		bool result = true;
		LStyle lStyle = (LStyle)bookmark;
		Point pt = new Point(Math.Max(lStyle.Pos, 0), lStyle.Line);
		if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: false))
		{
			if (lStyle.Pos >= 0)
			{
				lStyle.Pos = pt.X;
			}
			lStyle.Line = pt.Y;
			if (lStyle.Range != null)
			{
				lStyle.Range.StartPoint = pt;
			}
			result = true;
		}
		if (lStyle.Range != null)
		{
			pt = lStyle.Range.EndPoint;
			if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: true))
			{
				lStyle.Range.EndPoint = pt;
				result = true;
			}
		}
		return result;
	}

	protected virtual void RemoveLineAt(int idx, int line)
	{
		if (owner != null)
		{
			owner.BeginUpdate(UpdateReason.Other);
			try
			{
				RemoveAt(idx);
				owner.State |= NotifyState.BookMarkChanged;
				owner.LinesChanged(line, line);
				return;
			}
			finally
			{
				owner.EndUpdate();
			}
		}
		RemoveAt(idx);
	}

	public void SetLineStyle(Point position, IRange range, int priority, int style)
	{
		if (owner != null)
		{
			owner.BeginUpdate(UpdateReason.Other);
		}
		try
		{
			LStyle lStyle = NewLineStyle(position.Y, position.X, style, priority, range);
			if (FindFirst(lStyle, out var index, styleComparer))
			{
				((LStyle)base[index]).Assign(lStyle);
			}
			else
			{
				Insert(index, lStyle);
			}
			if (owner != null)
			{
				if (range != null)
				{
					owner.LinesChanged(range.StartPoint.Y, range.EndPoint.Y);
				}
				else
				{
					owner.LinesChanged(position.Y, position.Y);
				}
				owner.State |= NotifyState.BookMarkChanged;
			}
		}
		finally
		{
			if (owner != null)
			{
				owner.EndUpdate();
			}
		}
	}

	public int GetLineStyle(int line)
	{
		return InternalGet(line);
	}

	public int GetLineStyles(int line, IList list)
	{
		list.Clear();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LStyle lStyle = (LStyle)enumerator.Current;
				int num = ((lStyle.Range != null) ? lStyle.Range.EndPoint.Y : lStyle.Line);
				if (line >= lStyle.Line && line <= num)
				{
					list.Add(lStyle);
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
		return list.Count;
	}

	public void SetLineStyle(int line, int priority, int style)
	{
		SetLineStyle(new Point(-1, line), null, priority, style);
	}

	public void SetLineStyle(IRange range, int priority, int style)
	{
		SetLineStyle(range.StartPoint, range, priority, style);
	}

	public void SetLineStyle(Point position, int priority, int style)
	{
		SetLineStyle(position, null, priority, style);
	}

	public void SetLineStyle(int line, int style)
	{
		SetLineStyle(new Point(-1, line), null, -1, style);
	}

	public void RemoveLineStyle(int line)
	{
		int num = FindByLine(line);
		if (num >= 0)
		{
			RemoveLineAt(num, line);
		}
	}

	public void ToggleLineStyle(int line, int style)
	{
		if (FindByLine(line) >= 0)
		{
			RemoveLineStyle(line);
		}
		else
		{
			SetLineStyle(line, style);
		}
	}

	public void ToggleLineStyle(int line, int priority, int style)
	{
		int num = FindByLine(line);
		if (num >= 0)
		{
			for (int i = num; i < Count && ((BookMark)base[i]).Line == line; i++)
			{
				if (((BookMark)base[i]).Index == style)
				{
					RemoveLineAt(i, line);
					return;
				}
			}
		}
		SetLineStyle(line, priority, style);
	}

	public LStyle GetLStyle(int index)
	{
		return base[index] as LStyle;
	}
}
