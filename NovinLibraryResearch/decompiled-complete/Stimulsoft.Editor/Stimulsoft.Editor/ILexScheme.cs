namespace Stimulsoft.Editor;

public interface ILexScheme
{
	string Author { get; set; }

	string Name { get; set; }

	string Desc { get; set; }

	string Copyright { get; set; }

	string FileExtension { get; set; }

	string FileType { get; set; }

	string Version { get; set; }

	ILexStyles LexStyles { get; }

	ILexStates LexStates { get; }

	bool IsEmpty();

	bool IsPlainText(int style);

	void ClearScheme();
}
