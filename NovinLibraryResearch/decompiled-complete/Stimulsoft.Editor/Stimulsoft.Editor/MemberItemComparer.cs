using System;
using System.Collections;

namespace Stimulsoft.Editor;

internal class MemberItemComparer : IComparer
{
	public int Compare(object x, object y)
	{
		return string.Compare(x.ToString(), y.ToString(), ignoreCase: true);
	}

	public int PartialCompare(object x, object y)
	{
		string text = x.ToString();
		string text2 = y.ToString();
		return string.Compare(text, 0, text2, 0, Math.Min(text.Length, text2.Length), ignoreCase: true);
	}

	public bool FindPartial(SortList list, string name, out int idx)
	{
		list.FindFirst(name, out idx, this);
		if (idx >= 0 && idx < list.Count)
		{
			return PartialCompare(name, list[idx].ToString()) == 0;
		}
		return false;
	}
}
