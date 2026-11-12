using System.Collections;

namespace Stimulsoft.Editor;

public interface IQuickInfo : ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	string Text { get; set; }
}
