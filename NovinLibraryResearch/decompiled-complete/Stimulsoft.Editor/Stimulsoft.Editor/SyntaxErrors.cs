using System.Collections;

namespace Stimulsoft.Editor;

public class SyntaxErrors : SortList, ISyntaxErrors, IList, ICollection, IEnumerable
{
	public new ISyntaxError this[int index]
	{
		get
		{
			return base[index] as ISyntaxError;
		}
		set
		{
			base[index] = value;
		}
	}
}
