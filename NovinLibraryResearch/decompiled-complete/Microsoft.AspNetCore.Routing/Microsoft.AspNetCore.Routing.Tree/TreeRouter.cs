using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.AspNetCore.Routing.Logging;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Routing.Tree;

public class TreeRouter : IRouter
{
	private struct TreeEnumerator : IEnumerator<UrlMatchingNode>, IEnumerator, IDisposable
	{
		private readonly Stack<UrlMatchingNode> _stack;

		private readonly PathTokenizer _tokenizer;

		public UrlMatchingNode Current { get; private set; }

		object IEnumerator.Current => Current;

		public TreeEnumerator(UrlMatchingNode root, PathTokenizer tokenizer)
		{
			_stack = new Stack<UrlMatchingNode>();
			_tokenizer = tokenizer;
			Current = null;
			_stack.Push(root);
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (_stack == null)
			{
				return false;
			}
			while (_stack.Count > 0)
			{
				UrlMatchingNode urlMatchingNode = _stack.Pop();
				if (urlMatchingNode.IsCatchAll && urlMatchingNode.Matches.Count > 0)
				{
					Current = urlMatchingNode;
					return true;
				}
				if (urlMatchingNode.Depth == _tokenizer.Count)
				{
					if (urlMatchingNode.Matches.Count > 0)
					{
						Current = urlMatchingNode;
						return true;
					}
					continue;
				}
				if (urlMatchingNode.CatchAlls != null)
				{
					_stack.Push(urlMatchingNode.CatchAlls);
				}
				if (urlMatchingNode.ConstrainedCatchAlls != null)
				{
					_stack.Push(urlMatchingNode.ConstrainedCatchAlls);
				}
				if (urlMatchingNode.Parameters != null)
				{
					_stack.Push(urlMatchingNode.Parameters);
				}
				if (urlMatchingNode.ConstrainedParameters != null)
				{
					_stack.Push(urlMatchingNode.ConstrainedParameters);
				}
				if (urlMatchingNode.Literals.Count > 0 && urlMatchingNode.Literals.TryGetValue(_tokenizer[urlMatchingNode.Depth].Value, out var value))
				{
					_stack.Push(value);
				}
			}
			return false;
		}

		public void Reset()
		{
			_stack.Clear();
			Current = null;
		}
	}

	public static readonly string RouteGroupKey = "!__route_group";

	private readonly LinkGenerationDecisionTree _linkGenerationTree;

	private readonly UrlMatchingTree[] _trees;

	private readonly IDictionary<string, OutboundMatch> _namedEntries;

	private readonly ILogger _logger;

	private readonly ILogger _constraintLogger;

	public int Version { get; }

	internal IEnumerable<UrlMatchingTree> MatchingTrees => _trees;

	public TreeRouter(UrlMatchingTree[] trees, IEnumerable<OutboundRouteEntry> linkGenerationEntries, UrlEncoder urlEncoder, ObjectPool<UriBuildingContext> objectPool, ILogger routeLogger, ILogger constraintLogger, int version)
	{
		if (trees == null)
		{
			throw new ArgumentNullException("trees");
		}
		if (linkGenerationEntries == null)
		{
			throw new ArgumentNullException("linkGenerationEntries");
		}
		if (urlEncoder == null)
		{
			throw new ArgumentNullException("urlEncoder");
		}
		if (objectPool == null)
		{
			throw new ArgumentNullException("objectPool");
		}
		if (routeLogger == null)
		{
			throw new ArgumentNullException("routeLogger");
		}
		if (constraintLogger == null)
		{
			throw new ArgumentNullException("constraintLogger");
		}
		_trees = trees;
		_logger = routeLogger;
		_constraintLogger = constraintLogger;
		_namedEntries = new Dictionary<string, OutboundMatch>(StringComparer.OrdinalIgnoreCase);
		List<OutboundMatch> list = new List<OutboundMatch>();
		foreach (OutboundRouteEntry linkGenerationEntry in linkGenerationEntries)
		{
			TemplateBinder templateBinder = new TemplateBinder(urlEncoder, objectPool, linkGenerationEntry.RouteTemplate, linkGenerationEntry.Defaults);
			OutboundMatch outboundMatch = new OutboundMatch
			{
				Entry = linkGenerationEntry,
				TemplateBinder = templateBinder
			};
			list.Add(outboundMatch);
			if (linkGenerationEntry.RouteName != null)
			{
				if (_namedEntries.TryGetValue(linkGenerationEntry.RouteName, out var value) && !string.Equals(value.Entry.RouteTemplate.TemplateText, linkGenerationEntry.RouteTemplate.TemplateText, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(Resources.FormatAttributeRoute_DifferentLinkGenerationEntries_SameName(linkGenerationEntry.RouteName), "linkGenerationEntries");
				}
				if (value == null)
				{
					_namedEntries.Add(linkGenerationEntry.RouteName, outboundMatch);
				}
			}
		}
		_linkGenerationTree = new LinkGenerationDecisionTree(list.ToArray());
		Version = version;
	}

	public VirtualPathData GetVirtualPath(VirtualPathContext context)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (context.RouteName != null)
		{
			return GetVirtualPathForNamedRoute(context);
		}
		IList<OutboundMatchResult> matches = _linkGenerationTree.GetMatches(context);
		if (matches == null)
		{
			return null;
		}
		for (int i = 0; i < matches.Count; i++)
		{
			VirtualPathData virtualPathData = GenerateVirtualPath(context, matches[i].Match.Entry, matches[i].Match.TemplateBinder);
			if (virtualPathData != null)
			{
				return virtualPathData;
			}
		}
		return null;
	}

	public async Task RouteAsync(RouteContext context)
	{
		UrlMatchingTree[] trees = _trees;
		foreach (UrlMatchingTree obj in trees)
		{
			PathTokenizer tokenizer = new PathTokenizer(context.HttpContext.Request.Path);
			UrlMatchingNode root = obj.Root;
			TreeEnumerator treeEnumerator = new TreeEnumerator(root, tokenizer);
			RouteData.RouteDataSnapshot snapshot = context.RouteData.PushState(null, null, null);
			while (treeEnumerator.MoveNext())
			{
				UrlMatchingNode current = treeEnumerator.Current;
				foreach (InboundMatch match in current.Matches)
				{
					InboundRouteEntry entry = match.Entry;
					TemplateMatcher templateMatcher = match.TemplateMatcher;
					try
					{
						if (templateMatcher.TryMatch(context.HttpContext.Request.Path, context.RouteData.Values) && RouteConstraintMatcher.Match(entry.Constraints, context.RouteData.Values, context.HttpContext, this, RouteDirection.IncomingRequest, _constraintLogger))
						{
							_logger.MatchedRoute(entry.RouteName, entry.RouteTemplate.TemplateText);
							context.RouteData.Routers.Add(entry.Handler);
							await entry.Handler.RouteAsync(context);
							if (context.Handler != null)
							{
								return;
							}
						}
					}
					finally
					{
						if (context.Handler == null)
						{
							snapshot.Restore();
						}
					}
				}
			}
		}
	}

	private VirtualPathData GetVirtualPathForNamedRoute(VirtualPathContext context)
	{
		if (_namedEntries.TryGetValue(context.RouteName, out var value))
		{
			VirtualPathData virtualPathData = GenerateVirtualPath(context, value.Entry, value.TemplateBinder);
			if (virtualPathData != null)
			{
				return virtualPathData;
			}
		}
		return null;
	}

	private VirtualPathData GenerateVirtualPath(VirtualPathContext context, OutboundRouteEntry entry, TemplateBinder binder)
	{
		RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
		foreach (KeyValuePair<string, object> value in context.Values)
		{
			if (!entry.RequiredLinkValues.ContainsKey(value.Key) || entry.RouteTemplate.GetParameter(value.Key) != null)
			{
				routeValueDictionary.Add(value.Key, value.Value);
			}
		}
		TemplateValuesResult values = binder.GetValues(context.AmbientValues, routeValueDictionary);
		if (values == null)
		{
			return null;
		}
		if (!RouteConstraintMatcher.Match(entry.Constraints, values.CombinedValues, context.HttpContext, this, RouteDirection.UrlGeneration, _constraintLogger))
		{
			return null;
		}
		VirtualPathData virtualPath = entry.Handler.GetVirtualPath(context);
		if (virtualPath != null)
		{
			return virtualPath;
		}
		string text = binder.BindValues(values.AcceptedValues);
		if (text == null)
		{
			return null;
		}
		return new VirtualPathData(this, text);
	}
}
