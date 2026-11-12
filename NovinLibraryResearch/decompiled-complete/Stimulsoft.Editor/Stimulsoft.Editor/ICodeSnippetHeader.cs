namespace Stimulsoft.Editor;

public interface ICodeSnippetHeader
{
	string Title { get; set; }

	string Description { get; set; }

	string Author { get; set; }

	string Shortcut { get; set; }

	ICodeSnippetTypes Types { get; }

	void Assign(ICodeSnippetHeader source);
}
