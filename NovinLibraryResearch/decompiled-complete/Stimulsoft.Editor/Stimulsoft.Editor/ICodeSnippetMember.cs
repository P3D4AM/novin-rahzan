using System;

namespace Stimulsoft.Editor;

public interface ICodeSnippetMember : IComparable
{
	string Name { get; set; }

	string EditPath { get; }

	string Path { get; set; }

	int ImageIndex { get; set; }

	ICodeSnippetMembers Members { get; }

	ICodeSnippets Snippets { get; }

	ICodeSnippetsProvider SnippetsAndMembers { get; }

	ICodeSnippetMembers Parent { get; set; }
}
