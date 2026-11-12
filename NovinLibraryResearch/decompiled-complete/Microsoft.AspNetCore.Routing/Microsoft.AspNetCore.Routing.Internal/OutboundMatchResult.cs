using Microsoft.AspNetCore.Routing.Tree;

namespace Microsoft.AspNetCore.Routing.Internal;

public struct OutboundMatchResult
{
	public OutboundMatch Match { get; }

	public bool IsFallbackMatch { get; }

	public OutboundMatchResult(OutboundMatch match, bool isFallbackMatch)
	{
		Match = match;
		IsFallbackMatch = isFallbackMatch;
	}
}
