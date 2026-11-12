using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Settings.KeyValuePairs;

internal class KeyValuePairSettings : ILoggerSettings
{
	internal class ConfigurationMethodCall
	{
		public string MethodName { get; }

		public string ArgumentName { get; }

		public string Value { get; }

		public ConfigurationMethodCall(string methodName, string argumentName, string value)
		{
			MethodName = methodName;
			ArgumentName = argumentName;
			Value = value;
		}
	}

	private const string UsingDirective = "using";

	private const string LevelSwitchDirective = "level-switch";

	private const string AuditToDirective = "audit-to";

	private const string WriteToDirective = "write-to";

	private const string MinimumLevelDirective = "minimum-level";

	private const string MinimumLevelControlledByDirective = "minimum-level:controlled-by";

	private const string EnrichWithDirective = "enrich";

	private const string EnrichWithPropertyDirective = "enrich:with-property";

	private const string FilterDirective = "filter";

	private const string DestructureDirective = "destructure";

	private const string UsingDirectiveFullFormPrefix = "using:";

	private const string EnrichWithPropertyDirectivePrefix = "enrich:with-property:";

	private const string MinimumLevelOverrideDirectivePrefix = "minimum-level:override:";

	private const string CallableDirectiveRegex = "^(?<directive>audit-to|write-to|enrich|filter|destructure):(?<method>[A-Za-z0-9]*)(\\.(?<argument>[A-Za-z0-9]*)){0,1}$";

	private const string LevelSwitchDeclarationDirectiveRegex = "^level-switch:(?<switchName>.*)$";

	private const string LevelSwitchNameRegex = "^\\$[A-Za-z]+[A-Za-z0-9]*$";

	private static readonly string[] _supportedDirectives = new string[10] { "using", "level-switch", "audit-to", "write-to", "minimum-level", "minimum-level:controlled-by", "enrich:with-property", "enrich", "filter", "destructure" };

	private static readonly Dictionary<string, Type> CallableDirectiveReceiverTypes = new Dictionary<string, Type>
	{
		["audit-to"] = typeof(LoggerAuditSinkConfiguration),
		["write-to"] = typeof(LoggerSinkConfiguration),
		["enrich"] = typeof(LoggerEnrichmentConfiguration),
		["filter"] = typeof(LoggerFilterConfiguration),
		["destructure"] = typeof(LoggerDestructuringConfiguration)
	};

	private static readonly Dictionary<Type, Func<LoggerConfiguration, object>> CallableDirectiveReceivers = new Dictionary<Type, Func<LoggerConfiguration, object>>
	{
		[typeof(LoggerAuditSinkConfiguration)] = (LoggerConfiguration lc) => lc.AuditTo,
		[typeof(LoggerSinkConfiguration)] = (LoggerConfiguration lc) => lc.WriteTo,
		[typeof(LoggerEnrichmentConfiguration)] = (LoggerConfiguration lc) => lc.Enrich,
		[typeof(LoggerFilterConfiguration)] = (LoggerConfiguration lc) => lc.Filter,
		[typeof(LoggerDestructuringConfiguration)] = (LoggerConfiguration lc) => lc.Destructure
	};

	private readonly IReadOnlyDictionary<string, string> _settings;

	public KeyValuePairSettings(IReadOnlyDictionary<string, string> settings)
	{
		_settings = Guard.AgainstNull(settings, "settings");
	}

	public void Configure(LoggerConfiguration loggerConfiguration)
	{
		Guard.AgainstNull(loggerConfiguration, "loggerConfiguration");
		Dictionary<string, string> dictionary = _settings.Where<KeyValuePair<string, string>>((KeyValuePair<string, string> kvp) => _supportedDirectives.Any(kvp.Key.StartsWith)).ToDictionary((KeyValuePair<string, string> kvp) => kvp.Key, (KeyValuePair<string, string> kvp) => kvp.Value);
		IReadOnlyDictionary<string, LoggingLevelSwitch> readOnlyDictionary = ParseNamedLevelSwitchDeclarationDirectives(dictionary);
		if (dictionary.TryGetValue("minimum-level", out var value) && Enum.TryParse<LogEventLevel>(value, out var result))
		{
			loggerConfiguration.MinimumLevel.Is(result);
		}
		foreach (KeyValuePair<string, string> item in dictionary.Where((KeyValuePair<string, string> dir) => dir.Key.StartsWith("enrich:with-property:") && dir.Key.Length > "enrich:with-property:".Length))
		{
			string name = item.Key.Substring("enrich:with-property:".Length);
			loggerConfiguration.Enrich.WithProperty(name, item.Value);
		}
		if (dictionary.TryGetValue("minimum-level:controlled-by", out var value2))
		{
			LoggingLevelSwitch levelSwitch = LookUpSwitchByName(value2, readOnlyDictionary);
			loggerConfiguration.MinimumLevel.ControlledBy(levelSwitch);
		}
		foreach (KeyValuePair<string, string> item2 in dictionary.Where((KeyValuePair<string, string> dir) => dir.Key.StartsWith("minimum-level:override:") && dir.Key.Length > "minimum-level:override:".Length))
		{
			string source = item2.Key.Substring("minimum-level:override:".Length);
			if (Enum.TryParse<LogEventLevel>(item2.Value, out var result2))
			{
				loggerConfiguration.MinimumLevel.Override(source, result2);
				continue;
			}
			LoggingLevelSwitch levelSwitch2 = LookUpSwitchByName(item2.Value, readOnlyDictionary);
			loggerConfiguration.MinimumLevel.Override(source, levelSwitch2);
		}
		Regex matchCallables = new Regex("^(?<directive>audit-to|write-to|enrich|filter|destructure):(?<method>[A-Za-z0-9]*)(\\.(?<argument>[A-Za-z0-9]*)){0,1}$");
		var source2 = (from _003C_003Eh__TransparentIdentifier0 in dictionary.Where(delegate(KeyValuePair<string, string> wt)
			{
				Regex regex2 = matchCallables;
				KeyValuePair<string, string> keyValuePair2 = wt;
				return regex2.IsMatch(keyValuePair2.Key);
			}).Select(delegate(KeyValuePair<string, string> wt)
			{
				Regex regex = matchCallables;
				KeyValuePair<string, string> keyValuePair = wt;
				return new
				{
					wt = wt,
					match = regex.Match(keyValuePair.Key)
				};
			})
			select new
			{
				ReceiverType = CallableDirectiveReceiverTypes[_003C_003Eh__TransparentIdentifier0.match.Groups["directive"].Value],
				Call = new ConfigurationMethodCall(_003C_003Eh__TransparentIdentifier0.match.Groups["method"].Value, _003C_003Eh__TransparentIdentifier0.match.Groups["argument"].Value, _003C_003Eh__TransparentIdentifier0.wt.Value)
			}).ToList();
		if (!source2.Any())
		{
			return;
		}
		List<Assembly> configurationAssemblies = LoadConfigurationAssemblies(dictionary).ToList();
		foreach (var item3 in from d in source2
			group d by d.ReceiverType)
		{
			IList<MethodInfo> configurationMethods = CallableConfigurationMethodFinder.FindConfigurationMethods(configurationAssemblies, item3.Key);
			ApplyDirectives((from d in item3
				select d.Call into call
				group call by call.MethodName).ToList(), configurationMethods, CallableDirectiveReceivers[item3.Key](loggerConfiguration), readOnlyDictionary);
		}
	}

	internal static bool IsValidSwitchName(string input)
	{
		return Regex.IsMatch(input, "^\\$[A-Za-z]+[A-Za-z0-9]*$");
	}

	private static IReadOnlyDictionary<string, LoggingLevelSwitch> ParseNamedLevelSwitchDeclarationDirectives(IReadOnlyDictionary<string, string> directives)
	{
		Regex matchLevelSwitchDeclarations = new Regex("^level-switch:(?<switchName>.*)$");
		var list = (from _003C_003Eh__TransparentIdentifier0 in directives.Where<KeyValuePair<string, string>>(delegate(KeyValuePair<string, string> wt)
			{
				Regex regex2 = matchLevelSwitchDeclarations;
				KeyValuePair<string, string> keyValuePair2 = wt;
				return regex2.IsMatch(keyValuePair2.Key);
			}).Select(delegate(KeyValuePair<string, string> wt)
			{
				Regex regex = matchLevelSwitchDeclarations;
				KeyValuePair<string, string> keyValuePair = wt;
				return new
				{
					wt = wt,
					match = regex.Match(keyValuePair.Key)
				};
			})
			select new
			{
				SwitchName = _003C_003Eh__TransparentIdentifier0.match.Groups["switchName"].Value,
				InitialSwitchLevel = _003C_003Eh__TransparentIdentifier0.wt.Value
			}).ToList();
		Dictionary<string, LoggingLevelSwitch> dictionary = new Dictionary<string, LoggingLevelSwitch>();
		foreach (var item in list)
		{
			string switchName = item.SwitchName;
			string initialSwitchLevel = item.InitialSwitchLevel;
			if (!IsValidSwitchName(switchName))
			{
				throw new FormatException("\"" + switchName + "\" is not a valid name for a Level Switch declaration. Level switch must be declared with a '$' sign, like \"level-switch:$switchName\"");
			}
			LoggingLevelSwitch value = ((!(initialSwitchLevel == string.Empty)) ? new LoggingLevelSwitch((LogEventLevel)Enum.Parse(typeof(LogEventLevel), initialSwitchLevel)) : new LoggingLevelSwitch());
			dictionary.Add(switchName, value);
		}
		return dictionary;
	}

	private static LoggingLevelSwitch LookUpSwitchByName(string switchName, IReadOnlyDictionary<string, LoggingLevelSwitch> declaredLevelSwitches)
	{
		if (declaredLevelSwitches.TryGetValue(switchName, out LoggingLevelSwitch value))
		{
			return value;
		}
		throw new InvalidOperationException("No LoggingLevelSwitch has been declared with name \"" + switchName + "\". You might be missing a key \"level-switch:" + switchName + "\"");
	}

	private static object ConvertOrLookupByName(string valueOrSwitchName, Type type, IReadOnlyDictionary<string, LoggingLevelSwitch> declaredSwitches)
	{
		if (type == typeof(LoggingLevelSwitch))
		{
			return LookUpSwitchByName(valueOrSwitchName, declaredSwitches);
		}
		return SettingValueConversions.ConvertToType(valueOrSwitchName, type);
	}

	private static void ApplyDirectives(List<IGrouping<string, ConfigurationMethodCall>> directives, IList<MethodInfo> configurationMethods, object loggerConfigMethod, IReadOnlyDictionary<string, LoggingLevelSwitch> declaredSwitches)
	{
		IReadOnlyDictionary<string, LoggingLevelSwitch> declaredSwitches2 = declaredSwitches;
		foreach (IGrouping<string, ConfigurationMethodCall> directiveInfo in directives)
		{
			MethodInfo methodInfo = SelectConfigurationMethod(configurationMethods, directiveInfo.Key, directiveInfo);
			if (methodInfo == null)
			{
				SelfLog.WriteLine("Setting \"{0}\" could not be matched to an implementation in any of the loaded assemblies. To use settings from additional assemblies, specify them with the \"serilog:using\" key.", directiveInfo.Key);
				continue;
			}
			List<object> list = (from p in methodInfo.GetParameters().Skip(1)
				let directive = directiveInfo.FirstOrDefault((ConfigurationMethodCall s) => s.ArgumentName == p.Name)
				select SuppressConvertCall(directive, p)).ToList();
			list.Insert(0, loggerConfigMethod);
			methodInfo.Invoke(null, list.ToArray());
		}
		object? SuppressConvertCall(ConfigurationMethodCall? directive, ParameterInfo p)
		{
			if (directive != null)
			{
				return ConvertOrLookupByName(directive.Value, p.ParameterType, declaredSwitches2);
			}
			return p.DefaultValue;
		}
	}

	internal static MethodInfo? SelectConfigurationMethod(IEnumerable<MethodInfo> candidateMethods, string name, IEnumerable<ConfigurationMethodCall> suppliedArgumentValues)
	{
		string name2 = name;
		IEnumerable<ConfigurationMethodCall> suppliedArgumentValues2 = suppliedArgumentValues;
		return (from m in candidateMethods
			where m.Name == name2 && m.GetParameters().Skip(1).All((ParameterInfo p) => p.HasDefaultValue || suppliedArgumentValues2.Any((ConfigurationMethodCall s) => s.ArgumentName == p.Name))
			orderby m.GetParameters().Count((ParameterInfo p) => suppliedArgumentValues2.Any((ConfigurationMethodCall s) => s.ArgumentName == p.Name)) descending
			select m).FirstOrDefault();
	}

	internal static IEnumerable<Assembly> LoadConfigurationAssemblies(IReadOnlyDictionary<string, string> directives)
	{
		List<Assembly> list = new List<Assembly> { typeof(ILogger).Assembly };
		foreach (KeyValuePair<string, string> item in directives.Where<KeyValuePair<string, string>>((KeyValuePair<string, string> d) => d.Key.Equals("using") || d.Key.StartsWith("using:")))
		{
			if (string.IsNullOrWhiteSpace(item.Value))
			{
				throw new InvalidOperationException("A zero-length or whitespace assembly name was supplied to a serilog:using configuration statement.");
			}
			AssemblyName assemblyRef = new AssemblyName(item.Value);
			list.Add(Assembly.Load(assemblyRef));
		}
		return list.Distinct();
	}
}
