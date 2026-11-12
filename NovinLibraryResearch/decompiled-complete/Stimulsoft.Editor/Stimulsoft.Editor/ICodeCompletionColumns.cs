using System.Collections;

namespace Stimulsoft.Editor;

public interface ICodeCompletionColumns : IList, ICollection, IEnumerable
{
	new ICodeCompletionColumn this[int index] { get; set; }
}
