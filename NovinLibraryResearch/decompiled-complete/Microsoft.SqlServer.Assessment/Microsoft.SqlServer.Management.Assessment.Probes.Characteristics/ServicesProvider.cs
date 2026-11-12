using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal class ServicesProvider : ICharacteristicProvider
{
	public const string Key = "service";

	private const string Hive = "HKEY_LOCAL_MACHINE";

	private const string ServicesKey = "SYSTEM\\CurrentControlSet\\Services";

	private static readonly CharacteristicCollection StaticRequirements = new CharacteristicCollection("channel", "Registry");

	private static readonly Regex StaticPattern = new Regex("^service$", RegexOptions.Compiled);

	private static readonly Dictionary<string, Func<string, string, Version, string>> Composers = new Dictionary<string, Func<string, string, Version, string>>(StringComparer.OrdinalIgnoreCase)
	{
		{ "MSSQL", ComposeMsSql },
		{ "SQLAgent", ComposeSqlAgent },
		{ "MSOLAP", ComposeMsOlap },
		{ "ReportServer", ComposeReportServer },
		{ "MsDtsServer", ComposeByVersion },
		{ "MSSQLFDLauncher", ComposeByInstance },
		{ "SQLBrowser", null }
	};

	public Regex Pattern { get; } = StaticPattern;


	public int Priority => -200;

	public async Task UpdateCharacteristics(RunSpace runSpace, string key, Dictionary<string, string> args, HashSet<string> specification)
	{
		if (!runSpace.Meets(StaticRequirements))
		{
			CharacteristicCollection characteristicCollection = new CharacteristicCollection(StaticRequirements);
			characteristicCollection.ExceptWith(runSpace.KnownCharacteristics);
			runSpace.FailedCharacteristics.Add(new CharacteristicFailure
			{
				Subject = new CharacteristicCollection(key, specification),
				Reason = characteristicCollection
			});
			return;
		}
		if (!runSpace.KnownCharacteristics.TryGetValue(key, out var confirmedServices))
		{
			confirmedServices = new HashSet<string>();
			runSpace.KnownCharacteristics.Add(key, confirmedServices);
		}
		IRegistryTool tool = runSpace.GetTool<IRegistryTool>();
		if (tool == null)
		{
			return;
		}
		HashSet<string> services = await GetServices(tool).ConfigureAwait(continueOnCapturedContext: false);
		string instanceName = GetInstanceName(runSpace.Target.ServerName);
		StringBuilder prefix = new StringBuilder("service");
		prefix.Append("::");
		foreach (string serviceKey in specification)
		{
			if (Composers.TryGetValue(serviceKey, out var value))
			{
				string text = value?.Invoke(serviceKey, instanceName, runSpace.Target.Version) ?? serviceKey;
				string svcName = null;
				string data = null;
				bool found = services.Contains(text);
				if (found)
				{
					svcName = text;
					data = await GetServiceAccount(tool, text).ConfigureAwait(continueOnCapturedContext: false);
					confirmedServices.Add(serviceKey);
				}
				prefix.Append(serviceKey);
				Publish(runSpace, prefix, found);
				Publish(runSpace, prefix, svcName, "name");
				Publish(runSpace, prefix, data, "account");
				prefix.Length = "service".Length + "::".Length;
			}
		}
	}

	public CharacteristicCollection GetDependencies(RunSpace runSpace, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return StaticRequirements;
	}

	public string GetDisplayName(string key, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return string.Join(", ", specification) + " service" + ((specification.Count == 1) ? string.Empty : "s") + " installed";
	}

	private static async Task<HashSet<string>> GetServices(IRegistryTool tool)
	{
		return new HashSet<string>((await tool.EnumKeys("HKEY_LOCAL_MACHINE", "SYSTEM\\CurrentControlSet\\Services", instance: false).ConfigureAwait(continueOnCapturedContext: false)).Keys, StringComparer.OrdinalIgnoreCase);
	}

	private static async Task<string> GetServiceAccount(IRegistryTool tool, string service)
	{
		List<Dictionary<string, object>> list = await tool.Execute("HKEY_LOCAL_MACHINE", "SYSTEM\\CurrentControlSet\\Services\\" + service, new string[1] { "ObjectName" }, instance: false).ConfigureAwait(continueOnCapturedContext: false);
		return (list.Count > 0 && list[0].ContainsKey("ObjectName")) ? ((string)list[0]["ObjectName"]) : null;
	}

	private static string ComposeMsSql(string spec, string instanceName, Version version)
	{
		return ComposeByInstance("MSSQLServer", "MSSQL", instanceName);
	}

	private static string ComposeSqlAgent(string spec, string instanceName, Version version)
	{
		return ComposeByInstance("SQLServerAgent", "SQLAgent", instanceName);
	}

	private static string ComposeMsOlap(string spec, string instanceName, Version version)
	{
		return ComposeByInstance("MSSQLServerOLAPService", "MSOLAP", instanceName);
	}

	private static string ComposeReportServer(string spec, string instanceName, Version version)
	{
		if (version.Major > 13)
		{
			return "SQLServerReportingServices";
		}
		return ComposeByInstance(spec, instanceName, version);
	}

	private static string ComposeByInstance(string spec, string instanceName, Version version)
	{
		if (string.IsNullOrEmpty(instanceName))
		{
			return spec;
		}
		return spec + "$" + instanceName;
	}

	private static string ComposeByInstance(string defaultName, string instancePrefix, string instanceName)
	{
		if (string.IsNullOrEmpty(instanceName))
		{
			return defaultName;
		}
		return instancePrefix + "$" + instanceName;
	}

	private static string ComposeByVersion(string spec, string instanceName, Version version)
	{
		return FormattableString.Invariant($"{spec}{version.Major}0");
	}

	private static string GetInstanceName(string dataSource)
	{
		if (string.IsNullOrWhiteSpace(dataSource))
		{
			return "";
		}
		int num = dataSource.IndexOf('\\');
		if (num < 0)
		{
			return "";
		}
		return dataSource.Substring(num + 1);
	}

	private static void Publish(RunSpace runSpace, StringBuilder prefix, object data, string var = null)
	{
		int length = prefix.Length;
		if (var != null)
		{
			prefix.Append('.').Append(var);
		}
		runSpace.EvaluationContext.Data[prefix.ToString()] = data;
		prefix.Length = length;
	}
}
