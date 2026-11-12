using System.Collections;

namespace Stimulsoft.Editor;

public class SyntaxAttributes : SortList, ISyntaxAttributes, IList, ICollection, IEnumerable
{
	public new ISyntaxAttribute this[int index]
	{
		get
		{
			return base[index] as ISyntaxAttribute;
		}
		set
		{
			base[index] = value;
		}
	}
}
