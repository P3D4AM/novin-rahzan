namespace Stimulsoft.Editor;

public interface ICodeSnippetDeclaration
{
	ICodeSnippetLiterals Literals { get; }

	ICodeSnippetObjects Objects { get; }
}
