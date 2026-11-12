using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal class FeatureCharacteristicProvider : ICharacteristicProvider
{
	private static readonly Regex StaticPattern = new Regex("^feature$", RegexOptions.Compiled);

	public Regex Pattern { get; } = StaticPattern;


	public int Priority => -10;

	private Dictionary<string, IFeatureProvider> Registry { get; } = new Dictionary<string, IFeatureProvider> { 
	{
		"HADR",
		new HadrProvider()
	} };


	public async Task UpdateCharacteristics(RunSpace runSpace, string key, Dictionary<string, string> args, HashSet<string> specification)
	{
		if (!runSpace.KnownCharacteristics.TryGetValue(key, out var result))
		{
			result = new HashSet<string>();
			runSpace.KnownCharacteristics.Add(key, result);
		}
		Dictionary<string, HashSet<string>> waitList = new Dictionary<string, HashSet<string>>();
		foreach (string item in specification)
		{
			if (!Registry.TryGetValue(item, out var value))
			{
				CharacteristicCollection characteristicCollection = new CharacteristicCollection(key, item);
				runSpace.FailedCharacteristics.Add(new CharacteristicFailure
				{
					Subject = characteristicCollection,
					Reason = characteristicCollection
				});
				continue;
			}
			CharacteristicCollection dependencies = value.GetDependencies();
			if (!runSpace.Meets(dependencies))
			{
				CharacteristicCollection subject = new CharacteristicCollection(key, item);
				dependencies.ExceptWith(runSpace.KnownCharacteristics);
				runSpace.FailedCharacteristics.Add(new CharacteristicFailure
				{
					Subject = subject,
					Reason = dependencies
				});
				continue;
			}
			foreach (KeyValuePair<string, HashSet<string>> item2 in dependencies)
			{
				if (Pattern.IsMatch(item2.Key))
				{
					waitList.Add(item, item2.Value);
					break;
				}
			}
		}
		List<string> done = new List<string>();
		while (waitList.Count > 0)
		{
			done.Clear();
			foreach (KeyValuePair<string, HashSet<string>> pair in waitList)
			{
				if (pair.Value.Count != 0)
				{
					continue;
				}
				if (await Registry[pair.Key].IsPresent(runSpace).ConfigureAwait(continueOnCapturedContext: false))
				{
					result.Add(pair.Key);
				}
				done.Add(pair.Key);
				foreach (KeyValuePair<string, HashSet<string>> item3 in waitList)
				{
					item3.Value.Remove(pair.Key);
				}
			}
			foreach (string item4 in done)
			{
				waitList.Remove(item4);
			}
		}
	}

	public CharacteristicCollection GetDependencies(RunSpace runSpace, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		CharacteristicCollection characteristicCollection = new CharacteristicCollection();
		foreach (string item in specification)
		{
			if (Registry.TryGetValue(item, out var value))
			{
				characteristicCollection.UnionWith(value.GetDependencies());
			}
		}
		return characteristicCollection;
	}

	public string GetDisplayName(string key, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return string.Join(", ", specification) + " enabled";
	}
}
