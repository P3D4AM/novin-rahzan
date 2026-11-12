using System.Configuration;

namespace Microsoft.Data;

internal sealed class LocalDBConfigurationSection : ConfigurationSection
{
	[ConfigurationProperty("localdbinstances", IsRequired = true)]
	public LocalDBInstancesCollection LocalDbInstances => ((LocalDBInstancesCollection)((ConfigurationElement)this)["localdbinstances"]) ?? new LocalDBInstancesCollection();
}
