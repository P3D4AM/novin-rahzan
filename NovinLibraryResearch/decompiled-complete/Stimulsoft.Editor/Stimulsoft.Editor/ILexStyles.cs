using System.Collections;

namespace Stimulsoft.Editor;

public interface ILexStyles : IList, ICollection, IEnumerable
{
	new ILexStyle this[int index] { get; }

	ILexStyle AddLexStyle();

	ILexStyle InsertLexStyle(int index);

	ILexStyle FindLexStyle(string name);
}
