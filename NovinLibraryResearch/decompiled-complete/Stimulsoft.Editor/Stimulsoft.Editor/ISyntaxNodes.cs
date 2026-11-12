using System.Collections;

namespace Stimulsoft.Editor;

public interface ISyntaxNodes : IList, ICollection, IEnumerable
{
	new ISyntaxNode this[int index] { get; set; }
}
