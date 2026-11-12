using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

internal class BraceList : SortList
{
	private class ExactComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((BraceLine)x).Line - ((BraceLine)y).Line;
		}
	}

	private class LineComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			if ((int)y < ((BraceLine)x).Line)
			{
				return 1;
			}
			return 0;
		}
	}

	private class ExactLineComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((BraceLine)x).Line - (int)y;
		}
	}

	private class ExactBraceComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((BraceItem)x).X - (int)y;
		}
	}

	private class BraceComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			if ((int)y <= ((BraceItem)x).X)
			{
				return 1;
			}
			return 0;
		}
	}

	private class BraceItem
	{
		public int X;

		public bool Open;

		public int BraceIndex;

		public BraceItem(int x, int braceIndex, bool open)
		{
			X = x;
			BraceIndex = braceIndex;
			Open = open;
		}
	}

	private class BraceLine
	{
		private SortList braces;

		private int line;

		public int Line
		{
			get
			{
				return line;
			}
			set
			{
				line = value;
			}
		}

		public SortList Braces => braces;

		public BraceLine(int line)
		{
			braces = new SortList();
			this.line = line;
		}

		~BraceLine()
		{
			braces.Clear();
		}

		public bool FindBrace(int x, out int index, IComparer comparer)
		{
			return braces.FindExact(x, out index, comparer);
		}

		public void AddBrace(BraceItem brace, IComparer comparer)
		{
			braces.FindLast(brace.X, out var index, comparer);
			braces.Insert(index, brace);
		}
	}

	private IComparer exactComparer;

	private IComparer lineComparer;

	private IComparer braceComparer;

	private IComparer exactLineComparer;

	private IComparer exactBraceComparer;

	private new BraceLine this[int index] => base[index] as BraceLine;

	public BraceList()
	{
		exactComparer = new ExactComparer();
		lineComparer = new LineComparer();
		braceComparer = new BraceComparer();
		exactLineComparer = new ExactLineComparer();
		exactBraceComparer = new ExactBraceComparer();
	}

	public bool BlockDeleting(Rectangle rect)
	{
		bool result = false;
		FindFirst(rect.Bottom, out var index, exactLineComparer);
		if (index >= 0)
		{
			for (int num = Math.Min(index, Count - 1); num >= 0; num--)
			{
				BraceLine braceLine = this[num];
				if (braceLine.Line >= rect.Top && braceLine.Line <= rect.Bottom)
				{
					result = true;
					RemoveAt(num);
				}
				else if (braceLine.Line < rect.Top)
				{
					break;
				}
			}
		}
		return result;
	}

	public bool PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		if (deltaY == 0)
		{
			return false;
		}
		FindFirst(y, out var index, exactLineComparer);
		if (index >= 0)
		{
			bool flag = false;
			for (int i = index; i < Count; i++)
			{
				BraceLine braceLine = this[i];
				Point pt = new Point(0, braceLine.Line);
				if (!SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: true))
				{
					continue;
				}
				if (deltaX != 0 && deltaY != 0)
				{
					foreach (BraceItem brace in braceLine.Braces)
					{
						Point pt2 = new Point(brace.X, braceLine.Line);
						if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt2, endPos: true))
						{
							brace.X = pt2.X;
						}
					}
				}
				braceLine.Line = pt.Y;
				flag = true;
			}
			if (flag)
			{
				Sort(exactComparer);
				return true;
			}
		}
		return false;
	}

	public void AddBrace(int y, int x, int braceIndex, bool open)
	{
		BraceLine braceLine;
		if (FindLast(y, out var index, exactLineComparer))
		{
			braceLine = this[index];
		}
		else
		{
			braceLine = new BraceLine(y);
			Insert(index, braceLine);
		}
		braceLine.AddBrace(new BraceItem(x, braceIndex, open), exactBraceComparer);
	}

	public void RemoveBraces(int line)
	{
		if (FindLast(line, out var index, exactLineComparer))
		{
			RemoveAt(index);
		}
	}

	public bool FindBrace(Point position, out bool open)
	{
		open = false;
		if (FindLast(position.Y, out var index, exactLineComparer))
		{
			BraceLine braceLine = this[index];
			if (braceLine.FindBrace(position.X, out index, exactBraceComparer))
			{
				open = ((BraceItem)braceLine.Braces[index]).Open;
				return true;
			}
		}
		return false;
	}

	public bool FindClosingBrace(ref Point position, ref int braceIndex, int bracesLen)
	{
		if (FindLast(position.Y, out var index, lineComparer))
		{
			int[] array = new int[bracesLen];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 1;
			}
			for (int j = index; j < Count; j++)
			{
				BraceLine braceLine = this[j];
				int num = 0;
				if (braceLine.Line == position.Y)
				{
					num = (braceLine.Braces.FindLast(position.X, out index, braceComparer) ? (index + 1) : braceLine.Braces.Count);
				}
				for (int k = num; k < braceLine.Braces.Count; k++)
				{
					BraceItem braceItem = (BraceItem)braceLine.Braces[k];
					if (braceItem.Open)
					{
						array[braceItem.BraceIndex]++;
						continue;
					}
					array[braceItem.BraceIndex]--;
					if (array[braceItem.BraceIndex] == 0 && (braceIndex < 0 || braceIndex == braceItem.BraceIndex))
					{
						position = new Point(braceItem.X, braceLine.Line);
						braceIndex = braceItem.BraceIndex;
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool FindOpenBrace(ref Point position, ref int braceIndex, int bracesLen)
	{
		if (FindLast(position.Y, out var index, lineComparer))
		{
			int[] array = new int[bracesLen];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 1;
			}
			for (int num = index; num >= 0; num--)
			{
				BraceLine braceLine = this[num];
				int num2 = braceLine.Braces.Count - 1;
				if (braceLine.Line == position.Y)
				{
					num2 = (braceLine.Braces.FindLast(position.X, out index, braceComparer) ? index : (-1));
				}
				for (int num3 = num2; num3 >= 0; num3--)
				{
					BraceItem braceItem = (BraceItem)braceLine.Braces[num3];
					if (!braceItem.Open)
					{
						array[braceItem.BraceIndex]++;
					}
					else
					{
						array[braceItem.BraceIndex]--;
						if (array[braceItem.BraceIndex] == 0 && (braceIndex < 0 || braceIndex == braceItem.BraceIndex))
						{
							position = new Point(braceItem.X, braceLine.Line);
							braceIndex = braceItem.BraceIndex;
							return true;
						}
					}
				}
			}
		}
		return false;
	}
}
