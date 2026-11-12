using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class SyntaxErrorList : SortList
{
	private class ErrPtComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point position = ((ISyntaxError)x).Position;
			Point point = (Point)y;
			int num = position.Y - point.Y;
			if (num == 0)
			{
				num = position.X - point.X;
			}
			return num;
		}
	}

	private class ErrComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point position = ((ISyntaxError)x).Position;
			Point position2 = ((ISyntaxError)y).Position;
			int num = position.Y - position2.Y;
			if (num == 0)
			{
				num = position.X - position2.X;
			}
			return num;
		}
	}

	private IComparer errComparer;

	private IComparer errPtComparer;

	public SyntaxErrorList()
	{
		errComparer = new ErrComparer();
		errPtComparer = new ErrPtComparer();
	}

	public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		bool result = false;
		Point point = new Point(x, y);
		if (!FindLast(point, out var index, errPtComparer))
		{
			index--;
		}
		for (int i = Math.Max(index, 0); i < Count; i++)
		{
			ISyntaxError syntaxError = (ISyntaxError)this[i];
			point = syntaxError.Position;
			Point pt = syntaxError.Range.EndPoint;
			if (deltaY == 0 && point.Y > y)
			{
				break;
			}
			if (SortList.UpdatePos(x, y, deltaX, deltaY, ref point, endPos: false))
			{
				syntaxError.Position = point;
				result = true;
			}
			if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: true))
			{
				syntaxError.Range.EndPoint = pt;
				result = true;
			}
		}
		return result;
	}

	public virtual bool BlockDeleting(Rectangle rect)
	{
		bool result = false;
		if (FindLast(new Point(rect.Right, rect.Bottom), out var index, errPtComparer))
		{
			index++;
		}
		for (int num = Math.Min(index, Count - 1); num >= 0; num--)
		{
			ISyntaxError syntaxError = (ISyntaxError)this[num];
			if (syntaxError.Position.Y < rect.Top)
			{
				break;
			}
			if (SortList.InsideBlock(syntaxError.Position, rect) && SortList.InsideBlock(syntaxError.Range.EndPoint, rect, checkMaxInt: true))
			{
				RemoveAt(num);
				result = true;
			}
		}
		return result;
	}

	public override void Sort()
	{
		Sort(errComparer);
	}

	public bool FindError(Point position, bool exact, out int index)
	{
		bool flag = FindLast(position, out index, errPtComparer);
		if (!exact && !flag)
		{
			index--;
			if (index >= 0 && index < Count)
			{
				ISyntaxError syntaxError = this[index] as ISyntaxError;
				if ((syntaxError.Position.Y < position.Y || (syntaxError.Position.Y == position.Y && syntaxError.Position.X <= position.X)) && (syntaxError.Range.EndPoint.Y > position.Y || (syntaxError.Range.EndPoint.Y == position.Y && syntaxError.Range.EndPoint.X >= position.X)))
				{
					return true;
				}
			}
		}
		return flag;
	}
}
