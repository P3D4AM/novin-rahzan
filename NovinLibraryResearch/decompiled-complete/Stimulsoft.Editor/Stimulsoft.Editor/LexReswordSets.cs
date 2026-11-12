using System.Collections;

namespace Stimulsoft.Editor;

public class LexReswordSets : ArrayList, ILexReswordSets, IList, ICollection, IEnumerable
{
	public new ILexReswordSet this[int index] => (ILexReswordSet)base[index];

	public ILexReswordSet AddLexReswordSet()
	{
		LexReswordSet lexReswordSet = new LexReswordSet();
		base.Add(lexReswordSet);
		return lexReswordSet;
	}

	public ILexReswordSet InsertLexReswordSet(int index)
	{
		ILexReswordSet lexReswordSet = new LexReswordSet();
		base.Insert(index, lexReswordSet);
		return lexReswordSet;
	}

	public int FindResword(string resword)
	{
		for (int i = 0; i < Count; i++)
		{
			if (this[i].FindResword(resword))
			{
				return i;
			}
		}
		return -1;
	}
}
