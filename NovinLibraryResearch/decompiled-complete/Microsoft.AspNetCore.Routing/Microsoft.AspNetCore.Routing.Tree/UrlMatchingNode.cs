using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.AspNetCore.Routing.Tree;

[DebuggerDisplay("{DebuggerToString(),nq}")]
public class UrlMatchingNode
{
	public int Depth { get; }

	public bool IsCatchAll { get; set; }

	public List<InboundMatch> Matches { get; }

	public Dictionary<string, UrlMatchingNode> Literals { get; }

	public UrlMatchingNode ConstrainedParameters { get; set; }

	public UrlMatchingNode Parameters { get; set; }

	public UrlMatchingNode ConstrainedCatchAlls { get; set; }

	public UrlMatchingNode CatchAlls { get; set; }

	public UrlMatchingNode(int length)
	{
		Depth = length;
		Matches = new List<InboundMatch>();
		Literals = new Dictionary<string, UrlMatchingNode>(StringComparer.OrdinalIgnoreCase);
	}

	private string DebuggerToString()
	{
		return string.Format("Length: {0}, Matches: {1}", Depth, string.Join(" | ", Matches?.Select((InboundMatch m) => $"({m.TemplateMatcher.Template.TemplateText})")));
	}
}
