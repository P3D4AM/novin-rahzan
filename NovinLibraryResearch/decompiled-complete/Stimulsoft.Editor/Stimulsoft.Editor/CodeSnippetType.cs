namespace Stimulsoft.Editor;

public class CodeSnippetType : ICodeSnippetType
{
	private SnippetType snippetType;

	public SnippetType SnippetType
	{
		get
		{
			return snippetType;
		}
		set
		{
			snippetType = value;
		}
	}
}
