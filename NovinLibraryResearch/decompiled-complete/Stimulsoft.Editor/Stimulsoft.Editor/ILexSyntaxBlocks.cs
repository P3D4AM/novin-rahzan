using System.Collections;

namespace Stimulsoft.Editor;

public interface ILexSyntaxBlocks : IList, ICollection, IEnumerable
{
	new ILexSyntaxBlock this[int index] { get; }

	ILexSyntaxBlock AddLexSyntaxBlock();

	ILexSyntaxBlock InsertLexSyntaxBlock(int index);

	ILexSyntaxBlock FindSyntaxBlock(string name);
}
