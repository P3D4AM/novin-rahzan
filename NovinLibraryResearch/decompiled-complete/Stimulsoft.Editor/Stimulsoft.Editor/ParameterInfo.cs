using System.Collections;

namespace Stimulsoft.Editor;

public class ParameterInfo : ListMembers, IParameterInfo, IListMembers, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	protected internal override bool ShouldFormatName()
	{
		return base.UseHtmlFormatting;
	}
}
