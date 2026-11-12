using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class ActionAggregator
{
	public static void AggregateActions(ActionPropertyBag targetAction, ActionPropertyBag childAction)
	{
		targetAction.IncrementCount();
		targetAction.Sum("count", 1);
		PropertyBagContents contents = childAction.GetContents();
		foreach (string intAggregationProperty in GetIntAggregationProperties())
		{
			AggregateMax(intAggregationProperty, targetAction, contents.IntProperties);
			AggregateMin(intAggregationProperty, targetAction, contents.IntProperties);
			AggregateSum(intAggregationProperty, targetAction, contents.IntProperties);
		}
		foreach (string int64AggregationProperty in GetInt64AggregationProperties())
		{
			AggregateMax(int64AggregationProperty, targetAction, contents.Int64Properties);
			AggregateMin(int64AggregationProperty, targetAction, contents.Int64Properties);
			AggregateSum(int64AggregationProperty, targetAction, contents.Int64Properties);
		}
	}

	private static void AggregateMax(string basePropertyName, ActionPropertyBag targetAction, ConcurrentDictionary<string, int> childMap)
	{
		string text = basePropertyName + "_max";
		if (ShouldAggregateProperty(text, childMap, out var childValue))
		{
			targetAction.Max(text, childValue);
		}
	}

	private static void AggregateMin(string basePropertyName, ActionPropertyBag targetAction, ConcurrentDictionary<string, int> childMap)
	{
		string text = basePropertyName + "_min";
		if (ShouldAggregateProperty(text, childMap, out var childValue))
		{
			targetAction.Min(text, childValue);
		}
	}

	private static void AggregateSum(string basePropertyName, ActionPropertyBag targetAction, ConcurrentDictionary<string, int> childMap)
	{
		string text = basePropertyName + "_sum";
		if (ShouldAggregateProperty(text, childMap, out var childValue))
		{
			targetAction.Sum(text, childValue);
		}
	}

	private static void AggregateMax(string basePropertyName, ActionPropertyBag targetAction, ConcurrentDictionary<string, long> childMap)
	{
		string text = basePropertyName + "_max";
		if (ShouldAggregateProperty(text, childMap, out var childValue))
		{
			targetAction.Max(text, childValue);
		}
	}

	private static void AggregateMin(string basePropertyName, ActionPropertyBag targetAction, ConcurrentDictionary<string, long> childMap)
	{
		string text = basePropertyName + "_min";
		if (ShouldAggregateProperty(text, childMap, out var childValue))
		{
			targetAction.Min(text, childValue);
		}
	}

	private static void AggregateSum(string basePropertyName, ActionPropertyBag targetAction, ConcurrentDictionary<string, long> childMap)
	{
		string text = basePropertyName + "_sum";
		if (ShouldAggregateProperty(text, childMap, out var childValue))
		{
			targetAction.Sum(text, childValue);
		}
	}

	private static bool ShouldAggregateProperty<T>(string propertyName, ConcurrentDictionary<string, T> childMap, out T childValue)
	{
		return childMap.TryGetValue(propertyName, out childValue);
	}

	private static List<string> GetIntAggregationProperties()
	{
		return new List<string> { "Microsoft_MSAL_cache_event_count", "Microsoft_MSAL_http_event_count", "Microsoft_MSAL_response_time" };
	}

	private static List<string> GetInt64AggregationProperties()
	{
		return new List<string> { "duration" };
	}
}
