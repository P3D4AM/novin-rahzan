using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Routing.Tree;

public class TreeRouteBuilder
{
	private readonly ILogger _logger;

	private readonly ILogger _constraintLogger;

	private readonly UrlEncoder _urlEncoder;

	private readonly ObjectPool<UriBuildingContext> _objectPool;

	private readonly IInlineConstraintResolver _constraintResolver;

	public IList<InboundRouteEntry> InboundEntries { get; } = new List<InboundRouteEntry>();


	public IList<OutboundRouteEntry> OutboundEntries { get; } = new List<OutboundRouteEntry>();


	[Obsolete("This constructor is obsolete and will be removed in a future version. The recommended alternative is the overload that does not take a UrlEncoder.")]
	public TreeRouteBuilder(ILoggerFactory loggerFactory, UrlEncoder urlEncoder, ObjectPool<UriBuildingContext> objectPool, IInlineConstraintResolver constraintResolver)
		: this(loggerFactory, objectPool, constraintResolver)
	{
		if (urlEncoder == null)
		{
			throw new ArgumentNullException("urlEncoder");
		}
		_urlEncoder = urlEncoder;
	}

	public TreeRouteBuilder(ILoggerFactory loggerFactory, ObjectPool<UriBuildingContext> objectPool, IInlineConstraintResolver constraintResolver)
	{
		if (loggerFactory == null)
		{
			throw new ArgumentNullException("loggerFactory");
		}
		if (objectPool == null)
		{
			throw new ArgumentNullException("objectPool");
		}
		if (constraintResolver == null)
		{
			throw new ArgumentNullException("constraintResolver");
		}
		_urlEncoder = UrlEncoder.Default;
		_objectPool = objectPool;
		_constraintResolver = constraintResolver;
		_logger = loggerFactory.CreateLogger<TreeRouter>();
		_constraintLogger = loggerFactory.CreateLogger(typeof(RouteConstraintMatcher).FullName);
	}

	public InboundRouteEntry MapInbound(IRouter handler, RouteTemplate routeTemplate, string routeName, int order)
	{
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		if (routeTemplate == null)
		{
			throw new ArgumentNullException("routeTemplate");
		}
		InboundRouteEntry inboundRouteEntry = new InboundRouteEntry
		{
			Handler = handler,
			Order = order,
			Precedence = RoutePrecedence.ComputeInbound(routeTemplate),
			RouteName = routeName,
			RouteTemplate = routeTemplate
		};
		RouteConstraintBuilder routeConstraintBuilder = new RouteConstraintBuilder(_constraintResolver, routeTemplate.TemplateText);
		foreach (TemplatePart parameter in routeTemplate.Parameters)
		{
			if (parameter.InlineConstraints == null)
			{
				continue;
			}
			if (parameter.IsOptional)
			{
				routeConstraintBuilder.SetOptional(parameter.Name);
			}
			foreach (InlineConstraint inlineConstraint in parameter.InlineConstraints)
			{
				routeConstraintBuilder.AddResolvedConstraint(parameter.Name, inlineConstraint.Constraint);
			}
		}
		inboundRouteEntry.Constraints = routeConstraintBuilder.Build();
		inboundRouteEntry.Defaults = new RouteValueDictionary();
		foreach (TemplatePart parameter2 in inboundRouteEntry.RouteTemplate.Parameters)
		{
			if (parameter2.DefaultValue != null)
			{
				inboundRouteEntry.Defaults.Add(parameter2.Name, parameter2.DefaultValue);
			}
		}
		InboundEntries.Add(inboundRouteEntry);
		return inboundRouteEntry;
	}

	public OutboundRouteEntry MapOutbound(IRouter handler, RouteTemplate routeTemplate, RouteValueDictionary requiredLinkValues, string routeName, int order)
	{
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		if (routeTemplate == null)
		{
			throw new ArgumentNullException("routeTemplate");
		}
		if (requiredLinkValues == null)
		{
			throw new ArgumentNullException("requiredLinkValues");
		}
		OutboundRouteEntry outboundRouteEntry = new OutboundRouteEntry
		{
			Handler = handler,
			Order = order,
			Precedence = RoutePrecedence.ComputeOutbound(routeTemplate),
			RequiredLinkValues = requiredLinkValues,
			RouteName = routeName,
			RouteTemplate = routeTemplate
		};
		RouteConstraintBuilder routeConstraintBuilder = new RouteConstraintBuilder(_constraintResolver, routeTemplate.TemplateText);
		foreach (TemplatePart parameter in routeTemplate.Parameters)
		{
			if (parameter.InlineConstraints == null)
			{
				continue;
			}
			if (parameter.IsOptional)
			{
				routeConstraintBuilder.SetOptional(parameter.Name);
			}
			foreach (InlineConstraint inlineConstraint in parameter.InlineConstraints)
			{
				routeConstraintBuilder.AddResolvedConstraint(parameter.Name, inlineConstraint.Constraint);
			}
		}
		outboundRouteEntry.Constraints = routeConstraintBuilder.Build();
		outboundRouteEntry.Defaults = new RouteValueDictionary();
		foreach (TemplatePart parameter2 in outboundRouteEntry.RouteTemplate.Parameters)
		{
			if (parameter2.DefaultValue != null)
			{
				outboundRouteEntry.Defaults.Add(parameter2.Name, parameter2.DefaultValue);
			}
		}
		OutboundEntries.Add(outboundRouteEntry);
		return outboundRouteEntry;
	}

	public TreeRouter Build()
	{
		return Build(0);
	}

	public TreeRouter Build(int version)
	{
		Dictionary<int, UrlMatchingTree> dictionary = new Dictionary<int, UrlMatchingTree>();
		foreach (InboundRouteEntry inboundEntry in InboundEntries)
		{
			if (!dictionary.TryGetValue(inboundEntry.Order, out var value))
			{
				value = new UrlMatchingTree(inboundEntry.Order);
				dictionary.Add(inboundEntry.Order, value);
			}
			AddEntryToTree(value, inboundEntry);
		}
		return new TreeRouter(dictionary.Values.OrderBy((UrlMatchingTree tree) => tree.Order).ToArray(), OutboundEntries, _urlEncoder, _objectPool, _logger, _constraintLogger, version);
	}

	public void Clear()
	{
		InboundEntries.Clear();
		OutboundEntries.Clear();
	}

	private void AddEntryToTree(UrlMatchingTree tree, InboundRouteEntry entry)
	{
		UrlMatchingNode urlMatchingNode = tree.Root;
		TemplateMatcher templateMatcher = new TemplateMatcher(entry.RouteTemplate, entry.Defaults);
		for (int i = 0; i < entry.RouteTemplate.Segments.Count; i++)
		{
			TemplateSegment templateSegment = entry.RouteTemplate.Segments[i];
			if (!templateSegment.IsSimple)
			{
				if (urlMatchingNode.ConstrainedParameters == null)
				{
					urlMatchingNode.ConstrainedParameters = new UrlMatchingNode(i + 1);
				}
				urlMatchingNode = urlMatchingNode.ConstrainedParameters;
				continue;
			}
			TemplatePart templatePart = templateSegment.Parts[0];
			if (templatePart.IsLiteral)
			{
				if (!urlMatchingNode.Literals.TryGetValue(templatePart.Text, out var value))
				{
					value = new UrlMatchingNode(i + 1);
					urlMatchingNode.Literals.Add(templatePart.Text, value);
				}
				urlMatchingNode = value;
				continue;
			}
			if (templatePart.IsParameter && RemainingSegmentsAreOptional(entry.RouteTemplate.Segments, i))
			{
				urlMatchingNode.Matches.Add(new InboundMatch
				{
					Entry = entry,
					TemplateMatcher = templateMatcher
				});
			}
			if (templatePart.IsParameter && templatePart.InlineConstraints.Any() && !templatePart.IsCatchAll)
			{
				if (urlMatchingNode.ConstrainedParameters == null)
				{
					urlMatchingNode.ConstrainedParameters = new UrlMatchingNode(i + 1);
				}
				urlMatchingNode = urlMatchingNode.ConstrainedParameters;
			}
			else if (templatePart.IsParameter && !templatePart.IsCatchAll)
			{
				if (urlMatchingNode.Parameters == null)
				{
					urlMatchingNode.Parameters = new UrlMatchingNode(i + 1);
				}
				urlMatchingNode = urlMatchingNode.Parameters;
			}
			else if (templatePart.IsParameter && templatePart.InlineConstraints.Any() && templatePart.IsCatchAll)
			{
				if (urlMatchingNode.ConstrainedCatchAlls == null)
				{
					urlMatchingNode.ConstrainedCatchAlls = new UrlMatchingNode(i + 1)
					{
						IsCatchAll = true
					};
				}
				urlMatchingNode = urlMatchingNode.ConstrainedCatchAlls;
			}
			else if (templatePart.IsParameter && templatePart.IsCatchAll)
			{
				if (urlMatchingNode.CatchAlls == null)
				{
					urlMatchingNode.CatchAlls = new UrlMatchingNode(i + 1)
					{
						IsCatchAll = true
					};
				}
				urlMatchingNode = urlMatchingNode.CatchAlls;
			}
		}
		urlMatchingNode.Matches.Add(new InboundMatch
		{
			Entry = entry,
			TemplateMatcher = templateMatcher
		});
		urlMatchingNode.Matches.Sort(delegate(InboundMatch x, InboundMatch y)
		{
			int num = x.Entry.Precedence.CompareTo(y.Entry.Precedence);
			return (num != 0) ? num : x.Entry.RouteTemplate.TemplateText.CompareTo(y.Entry.RouteTemplate.TemplateText);
		});
	}

	private static bool RemainingSegmentsAreOptional(IList<TemplateSegment> segments, int currentParameterIndex)
	{
		for (int i = currentParameterIndex; i < segments.Count; i++)
		{
			if (!segments[i].IsSimple)
			{
				return false;
			}
			TemplatePart templatePart = segments[i].Parts[0];
			if (!templatePart.IsParameter)
			{
				return false;
			}
			if (!templatePart.IsOptional && !templatePart.IsCatchAll && templatePart.DefaultValue == null)
			{
				return false;
			}
		}
		return true;
	}
}
