using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.DecisionTree;
using Microsoft.AspNetCore.Routing.Tree;

namespace Microsoft.AspNetCore.Routing.Internal;

public class LinkGenerationDecisionTree
{
	private class OutboundMatchClassifier : IClassifier<OutboundMatch>
	{
		public IEqualityComparer<object> ValueComparer { get; private set; }

		public OutboundMatchClassifier()
		{
			ValueComparer = new RouteValueEqualityComparer();
		}

		public IDictionary<string, DecisionCriterionValue> GetCriteria(OutboundMatch item)
		{
			Dictionary<string, DecisionCriterionValue> dictionary = new Dictionary<string, DecisionCriterionValue>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, object> requiredLinkValue in item.Entry.RequiredLinkValues)
			{
				dictionary.Add(requiredLinkValue.Key, new DecisionCriterionValue(requiredLinkValue.Value ?? string.Empty));
			}
			return dictionary;
		}
	}

	private class OutboundMatchResultComparer : IComparer<OutboundMatchResult>
	{
		public static readonly OutboundMatchResultComparer Instance = new OutboundMatchResultComparer();

		public int Compare(OutboundMatchResult x, OutboundMatchResult y)
		{
			if (x.Match.Entry.Order != y.Match.Entry.Order)
			{
				return x.Match.Entry.Order.CompareTo(y.Match.Entry.Order);
			}
			if (x.Match.Entry.Precedence != y.Match.Entry.Precedence)
			{
				return y.Match.Entry.Precedence.CompareTo(x.Match.Entry.Precedence);
			}
			if (x.IsFallbackMatch != y.IsFallbackMatch)
			{
				return x.IsFallbackMatch.CompareTo(y.IsFallbackMatch);
			}
			return StringComparer.Ordinal.Compare(x.Match.Entry.RouteTemplate.TemplateText, y.Match.Entry.RouteTemplate.TemplateText);
		}
	}

	private readonly DecisionTreeNode<OutboundMatch> _root;

	public LinkGenerationDecisionTree(IReadOnlyList<OutboundMatch> entries)
	{
		_root = DecisionTreeBuilder<OutboundMatch>.GenerateTree(entries, new OutboundMatchClassifier());
	}

	public IList<OutboundMatchResult> GetMatches(VirtualPathContext context)
	{
		if (_root.Matches.Count > 0 || _root.Criteria.Count > 0)
		{
			List<OutboundMatchResult> list = new List<OutboundMatchResult>();
			Walk(list, context, _root, isFallbackPath: false);
			list.Sort(OutboundMatchResultComparer.Instance);
			return list;
		}
		return null;
	}

	private void Walk(List<OutboundMatchResult> results, VirtualPathContext context, DecisionTreeNode<OutboundMatch> node, bool isFallbackPath)
	{
		for (int i = 0; i < node.Matches.Count; i++)
		{
			results.Add(new OutboundMatchResult(node.Matches[i], isFallbackPath));
		}
		for (int j = 0; j < node.Criteria.Count; j++)
		{
			DecisionCriterion<OutboundMatch> decisionCriterion = node.Criteria[j];
			string key = decisionCriterion.Key;
			if (context.Values.TryGetValue(key, out var value))
			{
				if (decisionCriterion.Branches.TryGetValue(value ?? string.Empty, out var value2))
				{
					Walk(results, context, value2, isFallbackPath);
				}
				continue;
			}
			if (context.AmbientValues.TryGetValue(key, out value) && !decisionCriterion.Branches.Comparer.Equals(value, string.Empty) && decisionCriterion.Branches.TryGetValue(value, out var value3))
			{
				Walk(results, context, value3, isFallbackPath);
			}
			if (decisionCriterion.Branches.TryGetValue(string.Empty, out value3))
			{
				Walk(results, context, value3, isFallbackPath: true);
			}
		}
	}
}
