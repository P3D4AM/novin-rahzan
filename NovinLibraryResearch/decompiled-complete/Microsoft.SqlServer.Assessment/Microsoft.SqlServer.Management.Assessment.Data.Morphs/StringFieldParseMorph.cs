using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Assessment.Configuration.Json;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class StringFieldParseMorph : MultidimensionalCachedMorph
{
	private static readonly List<SortedList<string, object>> EmptyList = new List<SortedList<string, object>>();

	[JsonProperty]
	public Dictionary<string, List<Regex>> Map { get; set; }

	[JsonProperty]
	public bool Flatten { get; set; }

	[JsonProperty]
	public string Join { get; set; }

	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		return TransformRows(source);
	}

	internal IEnumerable<IDataRow> TransformRows(IEnumerable<IDataRow> source)
	{
		IEnumerable<IDataRow> enumerable = ((Join == null) ? source.SelectMany(Transform) : Transform(JoinRows(source)));
		if (!Flatten)
		{
			return enumerable;
		}
		return GetFlattener().TransformRows(enumerable.ToList());
	}

	private IDataRow JoinRows(IEnumerable<IDataRow> dataRows)
	{
		Measurement measurement = new Measurement();
		foreach (IDataRow dataRow in dataRows)
		{
			foreach (string key in Map.Keys)
			{
				if (dataRow.TryGetData(key, out var value))
				{
					if (!measurement.TryGetData(key, out var value2))
					{
						value2 = new List<string>();
						measurement.Add(new KeyValuePair<string, object>(key, value2));
					}
					((IList<string>)value2).Add(value.Normalize<string>());
				}
			}
		}
		foreach (KeyValuePair<string, object> item in measurement.ToList())
		{
			measurement[item.Key] = string.Join(Join, (IEnumerable<string>)item.Value);
		}
		return measurement;
	}

	protected IEnumerable<IDataRow> Transform(IDataRow sourceRow)
	{
		Func<SortedList<string, object>, SortedList<string, object>, SortedList<string, object>> aggregator = CollectionUtil.UnionWith;
		return from c in AlgebraicUtil.GetCombinations(Map.Select(ParseColumn).ToList(), aggregator)
			select new ExtValueRow(sourceRow, c);
		List<SortedList<string, object>> ParseColumn(KeyValuePair<string, List<Regex>> pair)
		{
			if (!sourceRow.TryGetData(pair.Key, out var value) || value == null)
			{
				return EmptyList;
			}
			string rawData = value.Normalize<string>();
			return pair.Value.SelectMany(ApplyPattern).ToList();
			IEnumerable<SortedList<string, object>> ApplyPattern(Regex pattern)
			{
				List<string> groupNames = pattern.GetGroupNames().Skip(1).ToList();
				MatchCollection source = pattern.Matches(rawData);
				return source.OfType<Match>().SelectMany(MatchToDictionary);
				IEnumerable<SortedList<string, object>> MatchToDictionary(Match match)
				{
					Func<SortedList<string, object>, KeyValuePair<string, string>, SortedList<string, object>> aggregator2 = CollectionUtil.Add;
					return AlgebraicUtil.GetCombinations((from cap in groupNames.Select(CapturesToDictionary)
						where cap.Count > 0
						select cap).ToList(), aggregator2).Select(pair.Key.PrefixKeys<SortedList<string, object>>);
					List<KeyValuePair<string, string>> CapturesToDictionary(string groupName)
					{
						return (from c in match.Groups[groupName].Captures.OfType<Capture>()
							select new KeyValuePair<string, string>(groupName, c.Value)).ToList();
					}
				}
			}
		}
	}

	private FlattenDataMorph GetFlattener()
	{
		FlattenDataMorph flattenDataMorph = new FlattenDataMorph
		{
			Columns = new List<string>()
		};
		foreach (KeyValuePair<string, List<Regex>> item in Map)
		{
			foreach (Regex item2 in item.Value)
			{
				foreach (string item3 in item2.GetGroupNames().Skip(1))
				{
					flattenDataMorph.Columns.Add(item.Key + "." + item3);
				}
			}
		}
		return flattenDataMorph;
	}
}
