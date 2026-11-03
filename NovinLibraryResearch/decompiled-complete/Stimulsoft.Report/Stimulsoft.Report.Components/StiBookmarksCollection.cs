using System.Collections;

namespace Stimulsoft.Report.Components;

public class StiBookmarksCollection : CollectionBase
{
	public StiBookmark this[string name]
	{
		get
		{
			int num = IndexOf(name);
			StiBookmark stiBookmark;
			if (num == -1)
			{
				stiBookmark = new StiBookmark(name, this);
				Add(stiBookmark);
			}
			else
			{
				stiBookmark = base.List[num] as StiBookmark;
			}
			return stiBookmark;
		}
	}

	public StiBookmark this[int index]
	{
		get
		{
			return (StiBookmark)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public void Add(StiBookmark bookmark)
	{
		base.List.Add(bookmark);
	}

	public void AddRange(StiBookmarksCollection bookmarks)
	{
		foreach (StiBookmark bookmark in bookmarks)
		{
			Add(bookmark);
		}
	}

	public void AddRange(StiBookmark[] bookmarks)
	{
		foreach (StiBookmark bookmark in bookmarks)
		{
			Add(bookmark);
		}
	}

	public bool Contains(StiBookmark bookmark)
	{
		return base.List.Contains(bookmark);
	}

	public int IndexOf(StiBookmark bookmark)
	{
		return base.List.IndexOf(bookmark);
	}

	public int IndexOf(string name)
	{
		int num = 0;
		foreach (StiBookmark item in base.List)
		{
			if (item.Text == name)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	public void Insert(int index, StiBookmark bookmark)
	{
		base.List.Insert(index, bookmark);
	}

	public void Remove(StiBookmark bookmark)
	{
		base.List.Remove(bookmark);
	}
}
