using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

[JsonObject(MemberSerialization.OptIn)]
public class EngineConfig
{
	private IRuleset defaultRuleset;

	public ConcurrentDictionary<string, ConcurrentDictionary<Version, IRuleset>> Rulesets { get; } = new ConcurrentDictionary<string, ConcurrentDictionary<Version, IRuleset>>();


	public IRuleset DefaultRuleset
	{
		get
		{
			return defaultRuleset;
		}
		internal set
		{
			defaultRuleset = value;
			AddRuleset(defaultRuleset);
		}
	}

	public List<string> CurrentRulesets { get; private set; } = new List<string>();


	[JsonProperty(Required = Required.Always)]
	private Version Version { get; set; }

	[JsonProperty(Required = Required.Always)]
	private string Name { get; set; }

	internal CharacteristicProviderSource CharacteristicProviderSource { get; } = new CharacteristicProviderSource();


	public void AddRuleset(IRuleset item, string alias = null)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		ConcurrentDictionary<Version, IRuleset> orAdd = Rulesets.GetOrAdd(alias ?? item.Name, (string n) => new ConcurrentDictionary<Version, IRuleset>());
		orAdd.AddOrUpdate(item.Version, item, (Version version, IRuleset factory) => item);
	}

	public IEnumerable<ICheck> GetChecks(ISqlObjectLocator target)
	{
		return GetChecksCore(target);
	}

	public IEnumerable<ICheck> GetChecks(ISqlObjectLocator target, IEnumerable<string> checkIdsOrTags)
	{
		return GetChecksCore(target, checkIdsOrTags);
	}

	public IProbe GetProbeImplementation(string probeId, ISqlObjectLocator target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (string.IsNullOrWhiteSpace(probeId))
		{
			throw new ArgumentException(Resources.ErrorValueCannotBeNullOrWhiteSpace, "probeId");
		}
		for (int num = CurrentRulesets.Count - 1; num >= 0; num--)
		{
			IRuleset ruleset = GetRuleset(CurrentRulesets[num]);
			if (ruleset is Ruleset ruleset2 && ruleset2.Probes.TryGetImplementation(probeId, target, out var value))
			{
				return value;
			}
		}
		throw new ProbeNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorImplementationWasNotFound, target));
	}

	private IEnumerable<ICheck> GetChecksCore(ISqlObjectLocator target, IEnumerable<string> checkIdsOrTags)
	{
		string[] selector = checkIdsOrTags.ToArray();
		foreach (ICheck item in GetChecksCore(target))
		{
			if (selector.Contains<string>(item.Id, StringComparer.OrdinalIgnoreCase) || item.Tags.Overlaps(selector))
			{
				yield return item;
			}
		}
	}

	private IEnumerable<ICheck> GetChecksCore(ISqlObjectLocator target)
	{
		HashSet<string> ids = new HashSet<string>(StringComparer.Ordinal);
		HashSet<string> tags = new HashSet<string>(StringComparer.Ordinal);
		for (int num = CurrentRulesets.Count - 1; num >= 0; num--)
		{
			GetRuleset(CurrentRulesets[num]).GetSuspects(target, ids, tags);
		}
		IEnumerable<ICheck> enumerable = Enumerable.Empty<ICheck>();
		foreach (string currentRuleset in CurrentRulesets)
		{
			enumerable = GetRuleset(currentRuleset).GetChecks(target, enumerable, ids, tags);
		}
		foreach (ICheck item in enumerable)
		{
			if (item.Target?.IsMatch(target) ?? true)
			{
				yield return item;
			}
		}
	}

	private IRuleset GetRuleset(string alias)
	{
		if (Rulesets.TryGetValue(alias, out var value))
		{
			Version version = value.Keys.Max();
			if (version != null && value.TryGetValue(version, out var value2))
			{
				return value2;
			}
		}
		throw new ArgumentOutOfRangeException("alias");
	}
}
