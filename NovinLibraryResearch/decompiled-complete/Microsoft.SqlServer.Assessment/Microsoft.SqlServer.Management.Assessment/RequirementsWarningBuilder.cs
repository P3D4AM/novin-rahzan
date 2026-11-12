using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

internal class RequirementsWarningBuilder
{
	private Dictionary<string, SpecGroupList> Errors { get; } = new Dictionary<string, SpecGroupList>();


	public void Append(ProbeRequirementsError err)
	{
		foreach (KeyValuePair<string, HashSet<string>> missingDependency in err.MissingDependencies)
		{
			if (!Errors.TryGetValue(missingDependency.Key, out var value))
			{
				value = new SpecGroupList();
				Errors.Add(missingDependency.Key, value);
			}
			value.Add(missingDependency.Value, err);
		}
	}

	public IEnumerable<IAssessmentResult> ToWarning(CharacteristicProviderSource providerSource)
	{
		if (Errors.Count == 0)
		{
			yield break;
		}
		foreach (KeyValuePair<string, SpecGroupList> errorGroup in Errors)
		{
			foreach (KeyValuePair<HashSet<string>, IReadOnlyList<ProbeRequirementsError>> item in errorGroup.Value)
			{
				string[] checkIds = item.Value.Select((ProbeRequirementsError e) => e.Check.Id).ToArray();
				string message = BuildMessage(providerSource, errorGroup.Key, item);
				yield return new RequirementsWarning(item.Value[0].TargetType, item.Value[0].TargetPath, message, checkIds);
			}
		}
	}

	private static string BuildMessage(CharacteristicProviderSource providerSource, string key, KeyValuePair<HashSet<string>, IReadOnlyList<ProbeRequirementsError>> subGroup)
	{
		IReadOnlyList<ProbeRequirementsError> value = subGroup.Value;
		return value.Count switch
		{
			1 => string.Format(CultureInfo.InvariantCulture, Resources.WarningCheckSkipped1, GetDisplayName(providerSource, key, subGroup.Key), value[0].Check.Id), 
			2 => string.Format(CultureInfo.InvariantCulture, Resources.WarningCheckSkipped2, GetDisplayName(providerSource, key, subGroup.Key), value[0].Check.Id, value[1].Check.Id), 
			3 => string.Format(CultureInfo.InvariantCulture, Resources.WarningCheckSkipped3, GetDisplayName(providerSource, key, subGroup.Key), value[0].Check.Id, value[1].Check.Id, value[2].Check.Id), 
			_ => string.Format(CultureInfo.InvariantCulture, Resources.WarningCheckSkipped3Plus, GetDisplayName(providerSource, key, subGroup.Key), value[0].Check.Id, value[1].Check.Id, value[2].Check.Id, value.Count - 3), 
		};
	}

	private static string GetDisplayName(CharacteristicProviderSource providerSource, string key, HashSet<string> specification)
	{
		if (!providerSource.TryGetProvider(key, out var provider, out var arguments))
		{
			return key;
		}
		return provider.GetDisplayName(key, arguments, specification);
	}
}
