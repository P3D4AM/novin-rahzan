using System.Collections;

namespace Stimulsoft.Editor;

public interface ISyntaxErrors : IList, ICollection, IEnumerable
{
	new ISyntaxError this[int index] { get; set; }
}
