namespace Stimulsoft.Editor;

public class CodeSnippetImport : ICodeSnippetImport
{
	private string nspace = string.Empty;

	public string Namespace
	{
		get
		{
			return nspace;
		}
		set
		{
			nspace = value;
		}
	}
}
