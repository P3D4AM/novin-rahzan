using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal class ChannelPresenceProvider : ICharacteristicProvider
{
	public const string Key = "channel";

	private static readonly Regex StaticPattern = new Regex("^channel$", RegexOptions.Compiled);

	public Regex Pattern => StaticPattern;

	public int Priority => -100;

	public async Task UpdateCharacteristics(RunSpace runSpace, string key, Dictionary<string, string> args, HashSet<string> specification)
	{
		if (!runSpace.KnownCharacteristics.TryGetValue(key, out var result))
		{
			result = new HashSet<string>();
			runSpace.KnownCharacteristics.Add(key, result);
		}
		List<KeyValuePair<string, HashSet<string>>> tree = new List<KeyValuePair<string, HashSet<string>>>();
		Dictionary<string, ITool> cache = new Dictionary<string, ITool>();
		foreach (string item in specification)
		{
			if (!ToolKey.TryGetValue(item, out var channelType))
			{
				continue;
			}
			ITool tool = runSpace.GetTool(channelType);
			if (tool != null)
			{
				cache.Add(item, tool);
				ICharacteristicCollection dependencies = tool.GetDependencies(runSpace);
				HashSet<string> value = dependencies.FirstOrDefault((KeyValuePair<string, HashSet<string>> r) => string.CompareOrdinal(r.Key, key) == 0).Value;
				HashSet<string> value2 = ((value == null) ? null : new HashSet<string>(value));
				tree.Add(new KeyValuePair<string, HashSet<string>>(item, value2));
			}
		}
		for (int i = tree.FindIndex((KeyValuePair<string, HashSet<string>> p) => (p.Value?.Count ?? 0) == 0); i >= 0; i = tree.FindIndex((KeyValuePair<string, HashSet<string>> p) => (p.Value?.Count ?? 0) == 0))
		{
			string channelKey = tree[i].Key;
			if (await cache[channelKey].IsAvailable(runSpace).ConfigureAwait(continueOnCapturedContext: false))
			{
				result.Add(channelKey);
			}
			foreach (KeyValuePair<string, HashSet<string>> item2 in tree)
			{
				item2.Value?.Remove(channelKey);
			}
			tree.RemoveAt(i);
		}
	}

	public CharacteristicCollection GetDependencies(RunSpace runSpace, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		CharacteristicCollection characteristicCollection = new CharacteristicCollection();
		foreach (string item in specification)
		{
			characteristicCollection.UnionWith(GetDependencies(runSpace, item));
		}
		return characteristicCollection;
	}

	public string GetDisplayName(string key, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return "access to " + string.Join(", ", specification);
	}

	private static ICharacteristicCollection GetDependencies(RunSpace runSpace, string key)
	{
		if (!ToolKey.TryGetValue(key, out var channelType))
		{
			return CharacteristicCollection.Empty;
		}
		ITool tool = runSpace.GetTool(channelType);
		if (tool == null)
		{
			return CharacteristicCollection.Empty;
		}
		return tool.GetDependencies(runSpace);
	}
}
