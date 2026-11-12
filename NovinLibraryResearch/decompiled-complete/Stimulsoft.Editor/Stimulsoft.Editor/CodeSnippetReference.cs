namespace Stimulsoft.Editor;

public class CodeSnippetReference : ICodeSnippetReference
{
	private string assembly = string.Empty;

	private string url = string.Empty;

	public string Assembly
	{
		get
		{
			return assembly;
		}
		set
		{
			assembly = value;
		}
	}

	public string Url
	{
		get
		{
			return url;
		}
		set
		{
			url = value;
		}
	}
}
