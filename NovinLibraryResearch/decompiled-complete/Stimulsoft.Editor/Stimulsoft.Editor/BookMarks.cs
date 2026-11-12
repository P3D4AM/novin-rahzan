using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class BookMarks : BookMarkList, IBookMarks, IList, ICollection, IEnumerable
{
	private ITextSource owner;

	public BookMarks(ITextSource owner)
	{
		this.owner = owner;
	}

	private void GotoBookMarkIndex(bool direction)
	{
		int num = -1;
		if (direction)
		{
			for (int i = 0; i < Count; i++)
			{
				IBookMark bookMark = base[i];
				if (bookMark.Index == int.MaxValue && bookMark.Line > owner.Position.Y)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				for (int j = 0; j < Count; j++)
				{
					if (base[j].Index == int.MaxValue)
					{
						num = j;
						break;
					}
				}
			}
		}
		else
		{
			for (int num2 = Count - 1; num2 >= 0; num2--)
			{
				IBookMark bookMark2 = base[num2];
				if (bookMark2.Index == int.MaxValue && bookMark2.Line < owner.Position.Y)
				{
					num = num2;
					break;
				}
			}
			if (num == -1)
			{
				for (int num3 = Count - 1; num3 >= 0; num3--)
				{
					if (base[num3].Index == int.MaxValue)
					{
						num = num3;
						break;
					}
				}
			}
		}
		if (num >= 0 && num < Count)
		{
			IBookMark bookMark3 = base[num];
			owner.BeginUpdate(UpdateReason.Navigate);
			try
			{
				owner.State |= NotifyState.GotoBookMark | NotifyState.CenterLine;
				owner.MoveTo(bookMark3.Pos, bookMark3.Line);
			}
			finally
			{
				owner.EndUpdate();
			}
		}
	}

	private void SetBookMark(int line, int pos, int bookMark)
	{
		owner.BeginUpdate(UpdateReason.Other);
		try
		{
			if (bookMark != int.MaxValue)
			{
				ClearBookMark(bookMark);
			}
			InternalSet(line, pos, bookMark);
			owner.LinesChanged(line, line);
			owner.State |= NotifyState.BookMarkChanged;
		}
		finally
		{
			owner.EndUpdate();
		}
	}

	public void Assign(IBookMarks source)
	{
		owner.BeginUpdate(UpdateReason.Other);
		try
		{
			Clear();
			foreach (IBookMark item in source)
			{
				Add(NewBookMark(item.Line, item.Pos, item.Index));
			}
			owner.LinesChanged(0, int.MaxValue);
			owner.State |= NotifyState.BookMarkChanged;
		}
		finally
		{
			owner.EndUpdate();
		}
	}

	public int GetBookMark(int line)
	{
		return InternalGet(line);
	}

	public int GetBookMark(Point startPoint, Point endPoint)
	{
		return InternalGet(startPoint, endPoint, rangeComparer);
	}

	public int GetBookMarks(Point startPoint, Point endPoint, IList list)
	{
		return InternalGet(startPoint, endPoint, list, rangeComparer);
	}

	public IBookMark FindBookMark(int bookMark)
	{
		int num = FindByIndex(bookMark);
		if (num >= 0)
		{
			return base[num];
		}
		return null;
	}

	public IBookMark FindBookMark(string name)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IBookMark bookMark = (IBookMark)enumerator.Current;
				if (bookMark is IBookMarkEx && ((IBookMarkEx)bookMark).Name == name)
				{
					return bookMark;
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
		return null;
	}

	public void SetBookMark(int line, int bookMark)
	{
		SetBookMark(line, 0, bookMark);
	}

	public void SetBookMark(Point position, int bookMark)
	{
		SetBookMark(position.Y, position.X, bookMark);
	}

	public void SetBookMark(Point position, int bookMark, string name, string description, string url)
	{
		owner.BeginUpdate(UpdateReason.Other);
		try
		{
			if (bookMark != int.MaxValue)
			{
				ClearBookMark(bookMark);
			}
			InternalSet(new BookMarkEx(position.Y, position.X, bookMark, name, description, url));
			owner.LinesChanged(position.Y, position.Y);
			owner.State |= NotifyState.BookMarkChanged;
		}
		finally
		{
			owner.EndUpdate();
		}
	}

	public void SetBookMark(IBookMark bookMark)
	{
		if (bookMark is IBookMarkEx)
		{
			IBookMarkEx bookMarkEx = (IBookMarkEx)bookMark;
			SetBookMark(bookMarkEx.Position, bookMarkEx.Index, bookMarkEx.Name, bookMarkEx.Description, bookMarkEx.Url);
		}
		else
		{
			SetBookMark(bookMark.Line, bookMark.Pos, bookMark.Index);
		}
	}

	public int FindBookMark(int bookMark, int line)
	{
		int i;
		if (bookMark == int.MaxValue)
		{
			for (i = FindByLine(line); i >= 0 && i < Count && base[i].Line == line && base[i].Index != bookMark; i++)
			{
			}
		}
		else
		{
			i = FindByIndex(bookMark);
		}
		if (i >= 0 && i < Count && base[i].Line == line)
		{
			return i;
		}
		return -1;
	}

	public bool FindBookMark(int bookMark, out Point position)
	{
		int num = FindByIndex(bookMark);
		if (num >= 0)
		{
			IBookMark bookMark2 = base[num];
			position = new Point(bookMark2.Pos, bookMark2.Line);
			return true;
		}
		position = new Point(0, 0);
		return false;
	}

	public void ToggleBookMark(Point position, int bookMark)
	{
		int num = FindBookMark(bookMark, position.Y);
		if (num >= 0)
		{
			ClearBookMarkByIndex(num);
		}
		else
		{
			SetBookMark(position.Y, position.X, bookMark);
		}
	}

	public void ToggleBookMark(int line, int bookMark)
	{
		int num = FindBookMark(bookMark, line);
		if (num >= 0)
		{
			ClearBookMarkByIndex(num);
		}
		else
		{
			SetBookMark(line, bookMark);
		}
	}

	public void ToggleBookMark(int bookMark)
	{
		ToggleBookMark(owner.Position, bookMark);
	}

	public void ToggleBookMark()
	{
		ToggleBookMark(owner.Position.Y, int.MaxValue);
	}

	public void ToggleBookMark(IBookMark bookMark)
	{
		if (bookMark is IBookMarkEx)
		{
			IBookMarkEx bookMarkEx = (IBookMarkEx)bookMark;
			ToggleBookMark(bookMarkEx.Position, bookMarkEx.Index, bookMarkEx.Name, bookMarkEx.Description, bookMarkEx.Url);
		}
		else
		{
			ToggleBookMark(bookMark.Position, bookMark.Index);
		}
	}

	public void ToggleBookMark(Point position, int bookMark, string name, string description, string url)
	{
		int num = FindBookMark(bookMark, position.Y);
		if (num >= 0)
		{
			ClearBookMarkByIndex(num);
		}
		else
		{
			SetBookMark(position, bookMark, name, description, url);
		}
	}

	public void ClearBookMark(int bookMark)
	{
		int num = FindByIndex(bookMark);
		if (num >= 0)
		{
			ClearBookMarkByIndex(num);
		}
	}

	public void ClearBookMark(int line, int bookmark)
	{
		int index = 0;
		if (!FindLast(line, out index, lineComparer))
		{
			return;
		}
		int num = index;
		while (num >= 0 && base[num].Line == line)
		{
			if (base[num].Index == bookmark)
			{
				ClearBookMarkByIndex(num);
				break;
			}
			num--;
		}
	}

	public void ClearBookMarkByIndex(int index)
	{
		if (index >= 0)
		{
			owner.BeginUpdate(UpdateReason.Other);
			try
			{
				int line = base[index].Line;
				RemoveAt(index);
				owner.State |= NotifyState.BookMarkChanged;
				owner.LinesChanged(line, line);
			}
			finally
			{
				owner.EndUpdate();
			}
		}
	}

	public override void Clear()
	{
		if (Count > 0)
		{
			owner.BeginUpdate(UpdateReason.Other);
			try
			{
				base.Clear();
				owner.LinesChanged(0, int.MaxValue);
				owner.State |= NotifyState.BookMarkChanged;
				return;
			}
			finally
			{
				owner.EndUpdate();
			}
		}
		base.Clear();
	}

	public void ClearBookMarks(int line)
	{
		int index = 0;
		if (!FindLast(line, out index, lineComparer))
		{
			return;
		}
		owner.BeginUpdate(UpdateReason.Other);
		try
		{
			int num = index;
			while (num >= 0 && base[num].Line == line)
			{
				RemoveAt(num);
				num--;
			}
			owner.State |= NotifyState.BookMarkChanged;
			owner.LinesChanged(line, line);
		}
		finally
		{
			owner.EndUpdate();
		}
	}

	public void ClearAllBookMarks()
	{
		Clear();
	}

	public void ClearAllUnnumberedBookmarks()
	{
		owner.BeginUpdate(UpdateReason.Other);
		try
		{
			for (int num = Count - 1; num > 0; num--)
			{
				IBookMark bookMark = base[num];
				if (bookMark.Index == int.MaxValue)
				{
					owner.LinesChanged(bookMark.Line, bookMark.Line);
					Remove(bookMark);
				}
			}
			owner.State |= NotifyState.BookMarkChanged;
		}
		finally
		{
			owner.EndUpdate();
		}
	}

	public int NextBookMark()
	{
		int num = -1;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				BookMark bookMark = (BookMark)enumerator.Current;
				if (bookMark.Index > num)
				{
					num = bookMark.Index;
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
		return num + 1;
	}

	public void GotoBookMark(int bookMark)
	{
		int num = FindByIndex(bookMark);
		if (num >= 0)
		{
			IBookMark bookMark2 = base[num];
			owner.BeginUpdate(UpdateReason.Navigate);
			try
			{
				owner.State |= NotifyState.GotoBookMark | NotifyState.CenterLine;
				owner.MoveTo(bookMark2.Pos, bookMark2.Line);
			}
			finally
			{
				owner.EndUpdate();
			}
		}
	}

	public void GotoPrevBookMark()
	{
		GotoBookMarkIndex(direction: false);
	}

	public void GotoNextBookMark()
	{
		GotoBookMarkIndex(direction: true);
	}
}
