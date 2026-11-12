using System;

namespace Stimulsoft.Editor;

public interface ICodeSnippet : IComparable
{
	ICodeSnippetHeader Header { get; set; }

	ICodeSnippetDeclarations Declarations { get; }

	ICodeSnippetImports Imports { get; }

	ICodeSnippetReferences References { get; }

	ICodeSnippetCode Code { get; set; }

	string Description { get; }

	int ImageIndex { get; set; }

	object CustomData { get; set; }

	ICodeSnippets Parent { get; set; }
}
