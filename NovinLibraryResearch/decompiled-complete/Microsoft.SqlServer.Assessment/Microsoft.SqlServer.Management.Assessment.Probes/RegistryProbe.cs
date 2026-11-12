using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class RegistryProbe : SingleToolProbe<IRegistryTool>
{
	private static readonly Regex ParamPattern = new Regex("(?<!@)@{(.*)}", RegexOptions.Compiled);

	[JsonProperty]
	public Dictionary<string, Dictionary<string, List<string>>> Query { get; set; }

	[JsonProperty]
	public bool Instance { get; set; }

	protected override async Task<IDataView> GetDataImpl(RunSpace runSpace, IRegistryTool tool, IReadOnlyDictionary<string, object> parameters)
	{
		if (Query == null)
		{
			return new MeasurementCollection();
		}
		List<List<Dictionary<string, object>>> heap = new List<List<Dictionary<string, object>>>();
		foreach (KeyValuePair<string, Dictionary<string, List<string>>> hiveQuery in Query)
		{
			foreach (KeyValuePair<string, List<string>> item in hiveQuery.Value)
			{
				string hive = Substitute(hiveQuery.Key, parameters);
				string key = Substitute(item.Key, parameters);
				IReadOnlyList<string> valueNames = Substitute(item.Value, parameters);
				List<Dictionary<string, object>> list = await tool.Execute(hive, key, valueNames, Instance).ConfigureAwait(continueOnCapturedContext: false);
				if (list.Count > 0)
				{
					heap.Add(list);
				}
			}
		}
		Func<Dictionary<string, object>, Dictionary<string, object>, Dictionary<string, object>> aggregator = CollectionUtil.UnionWith;
		IEnumerable<Dictionary<string, object>> source = from r in AlgebraicUtil.GetCombinations(heap, aggregator)
			where r.Count > 0
			select r;
		return new MeasurementCollection(source.Select((Dictionary<string, object> d) => new Measurement(d)));
	}

	private static string Substitute(string pattern, IReadOnlyDictionary<string, object> parameters)
	{
		object value;
		return ParamPattern.Replace(pattern, (Match m) => (m.Groups.Count <= 1 || !parameters.TryGetValue(m.Groups[1].Value, out value)) ? m.Value : value.Normalize<string>());
	}

	private static IReadOnlyList<string> Substitute(IReadOnlyList<string> list, IReadOnlyDictionary<string, object> parameters)
	{
		if (list.Any(ParamPattern.IsMatch))
		{
			list = list.Select((string i) => Substitute(i, parameters)).ToArray();
		}
		return list;
	}
}
