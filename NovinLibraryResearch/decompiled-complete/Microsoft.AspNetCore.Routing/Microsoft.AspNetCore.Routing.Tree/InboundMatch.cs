using System.Diagnostics;
using Microsoft.AspNetCore.Routing.Template;

namespace Microsoft.AspNetCore.Routing.Tree;

[DebuggerDisplay("{DebuggerToString(),nq}")]
public class InboundMatch
{
	public InboundRouteEntry Entry { get; set; }

	public TemplateMatcher TemplateMatcher { get; set; }

	private string DebuggerToString()
	{
		return TemplateMatcher?.Template?.TemplateText;
	}
}
