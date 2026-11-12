using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging;

internal sealed class LoggerFilterConfigureOptions : IConfigureOptions<LoggerFilterOptions>
{
	private const string LogLevelKey = "LogLevel";

	private const string DefaultCategory = "Default";

	private readonly IConfiguration _configuration;

	public LoggerFilterConfigureOptions(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void Configure(LoggerFilterOptions options)
	{
		LoadDefaultConfigValues(options);
	}

	private void LoadDefaultConfigValues(LoggerFilterOptions options)
	{
		if (_configuration == null)
		{
			return;
		}
		options.CaptureScopes = GetCaptureScopesValue(options);
		foreach (IConfigurationSection child in _configuration.GetChildren())
		{
			if (child.Key.Equals("LogLevel", StringComparison.OrdinalIgnoreCase))
			{
				LoadRules(options, child, null);
				continue;
			}
			IConfigurationSection section = child.GetSection("LogLevel");
			if (section != null)
			{
				string key = child.Key;
				LoadRules(options, section, key);
			}
		}
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "IConfiguration.GetValue is safe when T is a bool.")]
		bool GetCaptureScopesValue(LoggerFilterOptions options)
		{
			return _configuration.GetValue("CaptureScopes", options.CaptureScopes);
		}
	}

	private void LoadRules(LoggerFilterOptions options, IConfigurationSection configurationSection, string logger)
	{
		foreach (KeyValuePair<string, string> item2 in configurationSection.AsEnumerable(makePathsRelative: true))
		{
			if (TryGetSwitch(item2.Value, out var level))
			{
				string text = item2.Key;
				if (text.Equals("Default", StringComparison.OrdinalIgnoreCase))
				{
					text = null;
				}
				LoggerFilterRule item = new LoggerFilterRule(logger, text, level, null);
				options.Rules.Add(item);
			}
		}
	}

	private static bool TryGetSwitch(string value, out LogLevel level)
	{
		if (string.IsNullOrEmpty(value))
		{
			level = LogLevel.None;
			return false;
		}
		if (Enum.TryParse<LogLevel>(value, ignoreCase: true, out level))
		{
			return true;
		}
		throw new InvalidOperationException(System.SR.Format(System.SR.ValueNotSupported, value));
	}
}
