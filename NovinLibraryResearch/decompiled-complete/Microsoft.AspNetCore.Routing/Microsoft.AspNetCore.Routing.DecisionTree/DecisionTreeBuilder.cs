using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Routing.DecisionTree;

internal static class DecisionTreeBuilder<TItem>
{
	private class TreeBuilderContext
	{
		public HashSet<string> CurrentCriteria { get; private set; }

		public HashSet<ItemDescriptor<TItem>> MatchedItems { get; private set; }

		public TreeBuilderContext()
		{
			CurrentCriteria = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			MatchedItems = new HashSet<ItemDescriptor<TItem>>();
		}

		public TreeBuilderContext(TreeBuilderContext other)
		{
			CurrentCriteria = new HashSet<string>(other.CurrentCriteria, StringComparer.OrdinalIgnoreCase);
			MatchedItems = new HashSet<ItemDescriptor<TItem>>();
		}
	}

	private class Criterion : Dictionary<DecisionCriterionValue, List<ItemDescriptor<TItem>>>
	{
		public Criterion(DecisionCriterionValueEqualityComparer comparer)
			: base((IEqualityComparer<DecisionCriterionValue>?)comparer)
		{
		}
	}

	public static DecisionTreeNode<TItem> GenerateTree(IReadOnlyList<TItem> items, IClassifier<TItem> classifier)
	{
		List<ItemDescriptor<TItem>> list = new List<ItemDescriptor<TItem>>();
		for (int i = 0; i < items.Count; i++)
		{
			list.Add(new ItemDescriptor<TItem>
			{
				Criteria = classifier.GetCriteria(items[i]),
				Index = i,
				Item = items[i]
			});
		}
		DecisionCriterionValueEqualityComparer comparer = new DecisionCriterionValueEqualityComparer(classifier.ValueComparer);
		return GenerateNode(new TreeBuilderContext(), comparer, list);
	}

	private static DecisionTreeNode<TItem> GenerateNode(TreeBuilderContext context, DecisionCriterionValueEqualityComparer comparer, IList<ItemDescriptor<TItem>> items)
	{
		Dictionary<string, Criterion> dictionary = new Dictionary<string, Criterion>(StringComparer.OrdinalIgnoreCase);
		List<TItem> list = new List<TItem>();
		foreach (ItemDescriptor<TItem> item2 in items)
		{
			int num = 0;
			foreach (KeyValuePair<string, DecisionCriterionValue> criterion in item2.Criteria)
			{
				if (!context.CurrentCriteria.Contains(criterion.Key))
				{
					num++;
					if (!dictionary.TryGetValue(criterion.Key, out var value))
					{
						value = new Criterion(comparer);
						dictionary.Add(criterion.Key, value);
					}
					if (!value.TryGetValue(criterion.Value, out var value2))
					{
						value2 = new List<ItemDescriptor<TItem>>();
						value.Add(criterion.Value, value2);
					}
					value2.Add(item2);
				}
			}
			if (num == 0)
			{
				list.Add(item2.Item);
			}
		}
		List<DecisionCriterion<TItem>> list2 = new List<DecisionCriterion<TItem>>();
		foreach (KeyValuePair<string, Criterion> item3 in dictionary.OrderByDescending((KeyValuePair<string, Criterion> c) => c.Value.Count))
		{
			Dictionary<object, DecisionTreeNode<TItem>> dictionary2 = new Dictionary<object, DecisionTreeNode<TItem>>(comparer.InnerComparer);
			foreach (KeyValuePair<DecisionCriterionValue, List<ItemDescriptor<TItem>>> item4 in item3.Value)
			{
				List<ItemDescriptor<TItem>> list3 = new List<ItemDescriptor<TItem>>();
				foreach (ItemDescriptor<TItem> item5 in item4.Value)
				{
					if (context.MatchedItems.Add(item5))
					{
						list3.Add(item5);
					}
				}
				if (list3.Count > 0)
				{
					DecisionTreeNode<TItem> value3 = GenerateNode(new TreeBuilderContext(context)
					{
						CurrentCriteria = { item3.Key }
					}, comparer, item4.Value);
					dictionary2.Add(item4.Key.Value, value3);
				}
			}
			if (dictionary2.Count > 0)
			{
				DecisionCriterion<TItem> item = new DecisionCriterion<TItem>
				{
					Key = item3.Key,
					Branches = dictionary2
				};
				list2.Add(item);
			}
		}
		return new DecisionTreeNode<TItem>
		{
			Criteria = list2.ToList(),
			Matches = list
		};
	}
}
