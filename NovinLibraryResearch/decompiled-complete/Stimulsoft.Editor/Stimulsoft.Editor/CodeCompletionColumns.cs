using System.Collections;

namespace Stimulsoft.Editor;

public class CodeCompletionColumns : ArrayList, ICodeCompletionColumns, IList, ICollection, IEnumerable
{
	public new ICodeCompletionColumn this[int index]
	{
		get
		{
			return base[index] as ICodeCompletionColumn;
		}
		set
		{
			base[index] = value;
		}
	}
}
