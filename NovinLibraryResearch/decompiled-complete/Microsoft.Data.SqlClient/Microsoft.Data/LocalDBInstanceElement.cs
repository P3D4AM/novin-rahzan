using System.Configuration;

namespace Microsoft.Data;

internal sealed class LocalDBInstanceElement : ConfigurationElement
{
	[ConfigurationProperty("name", IsRequired = true)]
	public string Name => ((ConfigurationElement)this)["name"] as string;

	[ConfigurationProperty("version", IsRequired = true)]
	public string Version => ((ConfigurationElement)this)["version"] as string;
}
