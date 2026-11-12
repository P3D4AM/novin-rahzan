using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class CodeSnippetList : SortList
{
	private class SnippetPtComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point startPoint = ((IRange)x).StartPoint;
			Point point = (Point)y;
			int num = startPoint.Y - point.Y;
			if (num == 0)
			{
				num = startPoint.X - point.X;
			}
			return num;
		}
	}

	private class SnippetComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point startPoint = ((IRange)x).StartPoint;
			Point startPoint2 = ((IRange)y).StartPoint;
			int num = startPoint.Y - startPoint2.Y;
			if (num == 0)
			{
				num = startPoint.X - startPoint2.X;
			}
			return num;
		}
	}

	private IComparer snippetComparer;

	private IComparer snippetPtComparer;

	public CodeSnippetList()
	{
		snippetComparer = new SnippetComparer();
		snippetPtComparer = new SnippetPtComparer();
	}

	public bool NeedClear(int y)
	{
		if (Count != 0)
		{
			if (((IRange)this[0]).StartPoint.Y <= y)
			{
				return ((IRange)this[Count - 1]).EndPoint.Y < y;
			}
			return true;
		}
		return false;
	}

	public bool NeedClear(Rectangle rect)
	{
		if (!NeedClear(rect.Top))
		{
			return NeedClear(rect.Bottom);
		}
		return true;
	}

	public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, bool preserveBounds)
	{
		bool result = false;
		Point point = new Point(x, y);
		if (!FindLast(point, out var index, snippetPtComparer))
		{
			index--;
		}
		for (int i = Math.Max(index, 0); i < Count; i++)
		{
			SnippetRange snippetRange = (SnippetRange)this[i];
			point = snippetRange.StartPoint;
			Point pt = snippetRange.EndPoint;
			if (deltaY == 0 && point.Y > y)
			{
				break;
			}
			bool flag = snippetRange.StartPoint.Equals((object?)snippetRange.EndPoint);
			if (SortList.UpdatePos(x, y, deltaX, deltaY, ref point, !preserveBounds && snippetRange.Id != string.Empty && snippetRange.IsEditable))
			{
				snippetRange.StartPoint = point;
				if (flag)
				{
					snippetRange.EndPoint = point;
				}
				result = true;
			}
			if (!flag && SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, preserveBounds || snippetRange.Id == string.Empty || !snippetRange.IsEditable))
			{
				snippetRange.EndPoint = pt;
				result = true;
			}
		}
		return result;
	}

	public virtual bool BlockDeleting(Rectangle rect)
	{
		bool result = false;
		if (FindLast(new Point(rect.Right, rect.Bottom), out var index, snippetPtComparer))
		{
			index++;
		}
		for (int num = Math.Min(index, Count - 1); num >= 0; num--)
		{
			IRange range = (IRange)this[num];
			if (range.StartPoint.Y < rect.Top)
			{
				break;
			}
			if (SortList.InsideBlock(range.StartPoint, rect) && (SortList.InsideBlock(range.EndPoint, rect, checkMaxInt: true) || range.EndPoint.Equals((object?)new Point(rect.Right, rect.Bottom))))
			{
				RemoveAt(num);
				result = true;
			}
		}
		return result;
	}

	public int GetFirstSnippet()
	{
		for (int i = 0; i < Count; i++)
		{
			SnippetRange snippetRange = (SnippetRange)this[i];
			if (snippetRange.Id != string.Empty && snippetRange.IsEditable)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetNextSnippet(int index)
	{
		SnippetRange snippetRange = (SnippetRange)this[index];
		for (int i = index + 1; i < Count; i++)
		{
			SnippetRange snippetRange2 = (SnippetRange)this[i];
			if (snippetRange2.Id != string.Empty && snippetRange2.Id != snippetRange.Id && snippetRange2.IsEditable && IsFirstSnippet(i))
			{
				return i;
			}
		}
		for (int j = 0; j < index; j++)
		{
			SnippetRange snippetRange3 = (SnippetRange)this[j];
			if (snippetRange3.Id != string.Empty && snippetRange3.IsEditable && snippetRange3.Id != snippetRange.Id && IsFirstSnippet(j))
			{
				return j;
			}
		}
		return index;
	}

	public int GetPrevSnippet(int index)
	{
		SnippetRange snippetRange = (SnippetRange)this[index];
		for (int num = index - 1; num >= 0; num--)
		{
			SnippetRange snippetRange2 = (SnippetRange)this[num];
			if (snippetRange2.Id != string.Empty && snippetRange2.Id != snippetRange.Id && snippetRange2.IsEditable && IsFirstSnippet(num))
			{
				return num;
			}
		}
		for (int num2 = Count - 1; num2 > index; num2--)
		{
			SnippetRange snippetRange3 = (SnippetRange)this[num2];
			if (snippetRange3.Id != string.Empty && snippetRange3.IsEditable && snippetRange3.Id != snippetRange.Id && IsFirstSnippet(num2))
			{
				return num2;
			}
		}
		return index;
	}

	public bool IsFirstSnippet(IRange range)
	{
		int num = IndexOf(range);
		if (num >= 0)
		{
			return IsFirstSnippet(num);
		}
		return false;
	}

	public bool IsFirstSnippet(int index)
	{
		SnippetRange snippetRange = (SnippetRange)this[index];
		for (int num = index - 1; num >= 0; num--)
		{
			string id = ((SnippetRange)this[num]).Id;
			if (id == snippetRange.Id)
			{
				return false;
			}
		}
		return true;
	}

	public bool FindSnippet(Point position, bool exact, out int index)
	{
		bool flag = FindLast(position, out index, snippetPtComparer);
		if (!exact && !flag)
		{
			index--;
			if (index >= 0 && index < Count)
			{
				IRange range = this[index] as IRange;
				if ((range.StartPoint.Y < position.Y || (range.StartPoint.Y == position.Y && range.StartPoint.X <= position.X)) && (range.EndPoint.Y > position.Y || (range.EndPoint.Y == position.Y && range.EndPoint.X >= position.X)))
				{
					return true;
				}
			}
		}
		return flag;
	}

	public override void Sort()
	{
		Sort(snippetComparer);
	}
}
