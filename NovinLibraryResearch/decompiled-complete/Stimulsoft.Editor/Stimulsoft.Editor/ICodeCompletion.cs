using System.Drawing;

namespace Stimulsoft.Editor;

public interface ICodeCompletion
{
	ICodeCompletionBox CodeCompletionBox { get; }

	ICodeCompletionHint CodeCompletionHint { get; }

	char[] CodeCompletionChars { get; set; }

	event CodeCompletionEvent NeedCodeCompletion;

	void ShowCodeCompletionBox(ICodeCompletionProvider provider);

	void ShowCodeCompletionBox(ICodeCompletionProvider provider, Point position);

	void ShowCodeCompletionHint(ICodeCompletionProvider provider);

	void ShowCodeCompletionHint(ICodeCompletionProvider provider, Point position);

	bool IsValidText(Point position);

	string RemovePlainText(int line);

	void CompleteWord();

	void ListMembers();

	void QuickInfo();

	void ParameterInfo();

	void CodeSnippets();
}
