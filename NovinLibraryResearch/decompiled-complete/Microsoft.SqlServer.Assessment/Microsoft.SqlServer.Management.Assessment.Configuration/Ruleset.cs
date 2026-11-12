using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Configuration.Customization;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

[DebuggerDisplay("{Name}{Version}")]
public class Ruleset : IRuleset
{
	[JsonProperty("name", Required = Required.Always)]
	public string Name { get; set; }

	[JsonProperty("version", Required = Required.Always)]
	public Version Version { get; set; }

	[JsonProperty("schemaVersion", Required = Required.Always)]
	public Version SchemaVersion { get; set; }

	[JsonProperty(TypeNameHandling = TypeNameHandling.None)]
	public ProbeCollection Probes { get; } = new ProbeCollection();


	[JsonProperty("rules")]
	internal RuleCollection Rules { get; set; }

	public void GetSuspects(ISqlObjectLocator target, HashSet<string> ids, HashSet<string> tags)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (ids == null)
		{
			throw new ArgumentNullException("ids");
		}
		if (tags == null)
		{
			throw new ArgumentNullException("tags");
		}
		foreach (CheckSpecialization item in Rules.CheckChanges.Concat(Rules.CheckSpecializations))
		{
			if (!IsSuspect(item, target, tags))
			{
				continue;
			}
			CheckSelector selector = item.Selector;
			if (!(selector is CheckByIdSelector checkByIdSelector))
			{
				if (selector is CheckByTagSelector checkByTagSelector)
				{
					tags.UnionWith(checkByTagSelector.Tags);
				}
			}
			else
			{
				ids.Add(checkByIdSelector.CheckId);
			}
		}
	}

	public IEnumerable<ICheck> GetChecks(ISqlObjectLocator target, IEnumerable<ICheck> baseChecks, HashSet<string> ids, HashSet<string> tags)
	{
		Check[] source = baseChecks.OfType<Check>().Concat(GetNewChecks(target, ids, tags)).ToArray();
		source = Change(source).ToArray();
		return Specialize(source, target).ToArray();
	}

	private static bool IsSuspect(CheckRule change, ISqlObjectLocator target, ISet<string> suspectedTags)
	{
		if (change.Target == null || !change.Target.IsMatch(target))
		{
			return suspectedTags.Overlaps(GetTags(change));
		}
		return true;
	}

	private static Check Apply(Check c, CheckSpecialization s)
	{
		s.ApplyTo(c);
		return c;
	}

	private static IEnumerable<string> GetTags(CheckInPlaceTransform creator)
	{
		return creator.Children.SelectMany(GetTags);
	}

	private static IEnumerable<string> GetTags(IItemChange<Check> itemChange)
	{
		if (!(itemChange is ItemChange<Check, string[]> itemChange2) || !(itemChange2.Accessor is CheckTagsAccessor))
		{
			yield break;
		}
		ChangeTransform<string[]> transform = itemChange2.Transform;
		if (transform is OverwriteTransform<string[]> { Value: var value })
		{
			for (int i = 0; i < value.Length; i++)
			{
				yield return value[i];
			}
		}
	}

	private static bool IsMatch(ISqlObjectLocator target, CheckDefinition checkDefinitionRule, HashSet<string> suspectIds, HashSet<string> suspectTags)
	{
		bool flag = checkDefinitionRule.IsMatch(target);
		bool flag2 = string.IsNullOrEmpty(checkDefinitionRule.CheckId);
		bool flag3 = suspectIds.Contains(checkDefinitionRule.CheckId);
		bool flag4 = !flag2 && flag3;
		string[] other = GetTags(checkDefinitionRule).ToArray();
		bool flag5 = suspectTags.Overlaps(other);
		return flag || flag4 || flag5;
	}

	private IEnumerable<Check> Change(IEnumerable<Check> source)
	{
		IEnumerable<Check> enumerable = source;
		foreach (CheckChange checkChange in Rules.CheckChanges)
		{
			enumerable = checkChange.Apply(enumerable);
		}
		return enumerable;
	}

	private IEnumerable<Check> GetNewChecks(ISqlObjectLocator target, HashSet<string> suspectIds, HashSet<string> suspectTags)
	{
		foreach (CheckDefinition checkDefinition in Rules.CheckDefinitions)
		{
			if (IsMatch(target, checkDefinition, suspectIds, suspectTags))
			{
				yield return checkDefinition.CreateCheck(Name, Version);
			}
		}
	}

	private IEnumerable<Check> Specialize(IEnumerable<Check> source, ISqlObjectLocator target)
	{
		foreach (Check check in source)
		{
			yield return Rules.CheckSpecializations.Where((CheckSpecialization s) => s.IsMatch(check, target)).Aggregate(check, Apply);
		}
	}
}
