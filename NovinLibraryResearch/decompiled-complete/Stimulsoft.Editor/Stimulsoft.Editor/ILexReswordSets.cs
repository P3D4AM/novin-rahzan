using System.Collections;

namespace Stimulsoft.Editor;

public interface ILexReswordSets : IList, ICollection, IEnumerable
{
	new ILexReswordSet this[int index] { get; }

	ILexReswordSet AddLexReswordSet();

	ILexReswordSet InsertLexReswordSet(int index);

	int FindResword(string resword);
}
