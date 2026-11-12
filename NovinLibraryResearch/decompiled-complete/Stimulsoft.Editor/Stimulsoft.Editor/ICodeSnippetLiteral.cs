namespace Stimulsoft.Editor;

public interface ICodeSnippetLiteral
{
	string Default { get; set; }

	string Function { get; set; }

	string ID { get; set; }

	string Type { get; set; }

	string ToolTip { get; set; }

	bool Editable { get; set; }
}
