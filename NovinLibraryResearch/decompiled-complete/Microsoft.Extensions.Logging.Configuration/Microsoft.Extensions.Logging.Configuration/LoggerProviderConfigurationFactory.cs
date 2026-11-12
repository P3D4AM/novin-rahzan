using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Logging.Configuration;

internal sealed class LoggerProviderConfigurationFactory : ILoggerProviderConfigurationFactory
{
	private readonly IEnumerable<LoggingConfiguration> _configurations;

	public LoggerProviderConfigurationFactory(IEnumerable<LoggingConfiguration> configurations)
	{
		_configurations = configurations;
	}

	public IConfiguration GetConfiguration(Type providerType)
	{
		if (providerType == null)
		{
			throw new ArgumentNullException("providerType");
		}
		string fullName = providerType.FullName;
		string alias = Microsoft.Extensions.Logging.ProviderAliasUtilities.GetAlias(providerType);
		ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
		foreach (LoggingConfiguration configuration in _configurations)
		{
			IConfigurationSection section = configuration.Configuration.GetSection(fullName);
			configurationBuilder.AddConfiguration(section);
			if (!string.IsNullOrWhiteSpace(alias))
			{
				IConfigurationSection section2 = configuration.Configuration.GetSection(alias);
				configurationBuilder.AddConfiguration(section2);
			}
		}
		return configurationBuilder.Build();
	}
}
