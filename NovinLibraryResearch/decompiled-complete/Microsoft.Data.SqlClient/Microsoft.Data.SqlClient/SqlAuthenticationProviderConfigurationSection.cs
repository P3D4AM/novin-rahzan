using System.Configuration;

namespace Microsoft.Data.SqlClient;

internal class SqlAuthenticationProviderConfigurationSection : ConfigurationSection
{
	public const string Name = "SqlAuthenticationProviders";

	[ConfigurationProperty("providers")]
	public ProviderSettingsCollection Providers => (ProviderSettingsCollection)((ConfigurationElement)this)["providers"];

	[ConfigurationProperty("initializerType")]
	public string InitializerType => ((ConfigurationElement)this)["initializerType"] as string;

	[ConfigurationProperty("applicationClientId", IsRequired = false)]
	public string ApplicationClientId => ((ConfigurationElement)this)["applicationClientId"] as string;
}
