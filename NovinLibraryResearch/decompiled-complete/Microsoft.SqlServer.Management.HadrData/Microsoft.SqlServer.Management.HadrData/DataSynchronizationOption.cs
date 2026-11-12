using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.HadrData;

[LocalizedPropertyResources("Microsoft.SqlServer.Management.HadrData.Resource")]
public enum DataSynchronizationOption
{
	[DisplayNameKey("FullDataSync")]
	Full,
	[DisplayNameKey("JoinOnlyDataSync")]
	JoinOnly,
	[DisplayNameKey("ManualDataSync")]
	Manual,
	[DisplayNameKey("AutomaticSeedingDataSync")]
	AutomaticSeeding
}
