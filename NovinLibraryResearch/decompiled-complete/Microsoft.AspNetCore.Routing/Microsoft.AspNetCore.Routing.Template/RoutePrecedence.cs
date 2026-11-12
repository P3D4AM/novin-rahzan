using System;
using System.Linq;

namespace Microsoft.AspNetCore.Routing.Template;

public static class RoutePrecedence
{
	public static decimal ComputeInbound(RouteTemplate template)
	{
		decimal result = default(decimal);
		for (int i = 0; i < template.Segments.Count; i++)
		{
			int num = ComputeInboundPrecedenceDigit(template.Segments[i]);
			result += decimal.Divide(num, (decimal)Math.Pow(10.0, i));
		}
		return result;
	}

	public static decimal ComputeOutbound(RouteTemplate template)
	{
		decimal result = default(decimal);
		for (int i = 0; i < template.Segments.Count; i++)
		{
			int num = ComputeOutboundPrecedenceDigit(template.Segments[i]);
			result += decimal.Divide(num, (decimal)Math.Pow(10.0, i));
		}
		return result;
	}

	private static int ComputeOutboundPrecedenceDigit(TemplateSegment segment)
	{
		if (segment.Parts.Count > 1)
		{
			return 4;
		}
		TemplatePart templatePart = segment.Parts[0];
		if (templatePart.IsLiteral)
		{
			return 5;
		}
		int num = (templatePart.IsCatchAll ? 1 : 3);
		if (templatePart.InlineConstraints != null && templatePart.InlineConstraints.Any())
		{
			num++;
		}
		return num;
	}

	private static int ComputeInboundPrecedenceDigit(TemplateSegment segment)
	{
		if (segment.Parts.Count > 1)
		{
			return 2;
		}
		TemplatePart templatePart = segment.Parts[0];
		if (templatePart.IsLiteral)
		{
			return 1;
		}
		int num = (templatePart.IsCatchAll ? 5 : 3);
		if (templatePart.InlineConstraints != null && templatePart.InlineConstraints.Any())
		{
			num--;
		}
		return num;
	}
}
