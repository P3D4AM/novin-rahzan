using System.Collections;

namespace Stimulsoft.Editor;

public interface ISyntaxAttributes : IList, ICollection, IEnumerable
{
	new ISyntaxAttribute this[int index] { get; set; }
}
