using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISyntaxParser : IParser, ILexer, IImport
{
	SyntaxOptions Options { get; set; }

	ISyntaxTree SyntaxTree { get; set; }

	ICodeCompletionRepository CompletionRepository { get; set; }

	ICodeSnippetsProvider CodeSnippets { get; }

	bool CaseSensitive { get; }

	bool UseScheme { get; set; }

	char[] CodeCompletionChars { get; set; }

	char[] CodeCompletionStopChars { get; set; }

	char[] AutoIndentChars { get; set; }

	char[] SmartFormatChars { get; set; }

	event EventHandler TextReparsed;

	void ReparseText();

	bool ReparseBlock(Point position);

	bool ReparseBlock(Point position, out ISyntaxNode node, CodeCompletionType completionType);

	int Outline(IList ranges);

	int GetSmartIndent(int index, bool autoIndent);

	string SmartFormatLine(int index, string text, short[] colorData);

	int GetSyntaxErrors(IList errors);

	void CodeCompletion(string text, Point position, CodeCompletionArgs e);

	bool ProcessAutoComplete(Point position, out string code);

	string GetSingleLineComment();

	ISyntaxNode GetBlock(Point position, out Point blockPt);

	ISyntaxNode GetNodeAt(Point position);

	bool IsDeclaration(ISyntaxNode node);

	object FindDeclaration(string text, Point position);

	int FindReferences(ISyntaxNode node, ISyntaxNodes references);

	void ResetOptions();

	void ResetCodeCompletionChars();

	void ResetCodeCompletionStopChars();

	void ResetAutoIndentChars();

	void ResetSmartFormatChars();

	bool IsContentDivider(int index);
}
