using System.Collections;

namespace Stimulsoft.Editor;

public class SyntaxNodes : SortList, ISyntaxNodes, IList, ICollection, IEnumerable
{
	public new ISyntaxNode this[int index]
	{
		get
		{
			return base[index] as ISyntaxNode;
		}
		set
		{
			base[index] = value;
		}
	}
}
