namespace Stimulsoft.Editor;

public interface ICodeSnippetCode
{
	string Delimiter { get; set; }

	string Kind { get; set; }

	string Language { get; set; }

	string Code { get; set; }

	void Assign(ICodeSnippetCode source);
}
