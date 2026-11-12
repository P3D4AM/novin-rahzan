using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeSnippetsProvider : ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	ICodeSnippet FindByShortcut(string shortcut, bool caseSensitive);
}
