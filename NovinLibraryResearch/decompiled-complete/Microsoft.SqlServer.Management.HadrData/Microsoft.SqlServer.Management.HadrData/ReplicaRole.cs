using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.HadrData;

[LocalizedPropertyResources("Microsoft.SqlServer.Management.HadrData.Resource")]
public enum ReplicaRole
{
	[DisplayNameKey("Primary")]
	Primary,
	[DisplayNameKey("Secondary")]
	Secondary
}
