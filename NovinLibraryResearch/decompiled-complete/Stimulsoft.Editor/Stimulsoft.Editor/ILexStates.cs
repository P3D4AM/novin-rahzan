using System.Collections;

namespace Stimulsoft.Editor;

public interface ILexStates : IList, ICollection, IEnumerable
{
	new ILexState this[int index] { get; }

	ILexState AddLexState();

	ILexState InsertLexState(int index);

	ILexState FindLexState(string name);
}
