using Microsoft.AspNetCore.Routing.Template;

namespace Microsoft.AspNetCore.Routing.Tree;

public class OutboundMatch
{
	public OutboundRouteEntry Entry { get; set; }

	public TemplateBinder TemplateBinder { get; set; }
}
