using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class SortList : ArrayList
{
	public int CompareIndex;

	public bool FindExact(object obj, out int index, IComparer comparer)
	{
		index = 0;
		if (comparer == null)
		{
			return false;
		}
		int num = 0;
		int num2 = Count - 1;
		int num3 = num;
		bool result = false;
		while (num <= num2)
		{
			num3 = (CompareIndex = num + num2 >> 1);
			int num4 = comparer.Compare(this[num3], obj);
			if (num4 < 0)
			{
				num = num3 + 1;
				continue;
			}
			num2 = num3 - 1;
			if (num4 == 0)
			{
				result = true;
				num = num3;
			}
		}
		index = num;
		return result;
	}

	public bool FindFirst(object obj, out int index, IComparer comparer)
	{
		int num = 0;
		int num2 = Count - 1;
		int num3 = num;
		bool result = false;
		while (num <= num2)
		{
			num3 = (CompareIndex = num + num2 >> 1);
			int num4 = comparer.Compare(this[num3], obj);
			if (num4 < 0)
			{
				num = num3 + 1;
				continue;
			}
			num2 = num3 - 1;
			if (num4 == 0)
			{
				result = true;
			}
		}
		index = num;
		return result;
	}

	public bool FindLast(object obj, out int index, IComparer comparer)
	{
		int num = 0;
		int num2 = Count - 1;
		int num3 = num;
		int num4 = 0;
		bool flag = false;
		while (num <= num2)
		{
			num3 = (CompareIndex = num + num2 >> 1);
			int num5 = comparer.Compare(this[num3], obj);
			if (num5 > 0)
			{
				num2 = num3 - 1;
				continue;
			}
			num = num3 + 1;
			if (num5 == 0)
			{
				num4 = num3;
				flag = true;
			}
		}
		index = (flag ? num4 : num);
		return flag;
	}

	public static bool UpdatePos(int x, int y, int deltaX, int deltaY, ref Point pt, bool endPos)
	{
		int ch = pt.X;
		int ln = pt.Y;
		bool result = UpdatePos(x, y, deltaX, deltaY, ref ch, ref ln, endPos);
		pt.X = ch;
		pt.Y = ln;
		return result;
	}

	public static bool UpdatePos(int x, int y, int deltaX, int deltaY, ref int ch, ref int ln, bool endPos)
	{
		int num = ch;
		int num2 = ln;
		if (deltaY == 0)
		{
			bool flag = deltaX < 0 || endPos;
			if (ln == y && ((ch >= x && !flag) || (ch > x && flag)))
			{
				if (ch != int.MaxValue)
				{
					ch += deltaX;
				}
				ch = Math.Max(ch, 0);
			}
		}
		else
		{
			bool flag2 = endPos;
			if (ln > y || (ln == y && ((ch >= x && !flag2) || (ch > x && flag2))))
			{
				if ((ln == y && deltaY >= 0) || (ln + deltaY == y && deltaY < 0))
				{
					if (ch != int.MaxValue)
					{
						ch += deltaX;
					}
					ch = Math.Max(ch, 0);
				}
				if (ln != int.MaxValue)
				{
					ln += deltaY;
				}
				ln = Math.Max(ln, 0);
			}
		}
		if (ch == num)
		{
			return ln != num2;
		}
		return true;
	}

	public static bool InsideBlock(Point pt, Rectangle rect)
	{
		return InsideBlock(pt, rect, checkMaxInt: false);
	}

	public static bool InsideBlock(Point pt, Rectangle rect, bool checkMaxInt)
	{
		if (pt.Y <= rect.Bottom && pt.Y >= rect.Top)
		{
			if (pt.Y == rect.Top)
			{
				if (pt.Y == rect.Bottom)
				{
					if (pt.X >= rect.Left)
					{
						if (pt.X >= rect.Right)
						{
							if (checkMaxInt && pt.X == rect.Right)
							{
								return pt.X == int.MaxValue;
							}
							return false;
						}
						return true;
					}
					return false;
				}
				return pt.X >= rect.Left;
			}
			if (pt.Y == rect.Bottom)
			{
				if (pt.X >= rect.Right)
				{
					if (checkMaxInt && pt.X == rect.Right)
					{
						return pt.X == int.MaxValue;
					}
					return false;
				}
				return true;
			}
			return true;
		}
		return false;
	}
}
