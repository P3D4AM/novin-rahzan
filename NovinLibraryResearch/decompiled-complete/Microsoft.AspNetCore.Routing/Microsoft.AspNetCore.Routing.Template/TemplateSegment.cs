using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.AspNetCore.Routing.Template;

[DebuggerDisplay("{DebuggerToString()}")]
public class TemplateSegment
{
	public bool IsSimple => Parts.Count == 1;

	public List<TemplatePart> Parts { get; } = new List<TemplatePart>();


	internal string DebuggerToString()
	{
		return string.Join(string.Empty, Parts.Select((TemplatePart p) => p.DebuggerToString()));
	}
}
