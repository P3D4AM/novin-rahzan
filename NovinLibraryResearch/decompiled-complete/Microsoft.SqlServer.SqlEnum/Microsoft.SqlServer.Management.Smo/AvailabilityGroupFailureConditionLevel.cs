using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(AvailabilityGroupFailureConditionLevelConverter))]
public enum AvailabilityGroupFailureConditionLevel
{
	[LocDisplayName("agfcOnServerDown")]
	OnServerDown = 1,
	[LocDisplayName("agfcOnServerUnresponsive")]
	OnServerUnresponsive,
	[LocDisplayName("agfcOnCriticalServerErrors")]
	OnCriticalServerErrors,
	[LocDisplayName("agfcOnModerateServerErrors")]
	OnModerateServerErrors,
	[LocDisplayName("agfcOnAnyQualifiedFailureCondition")]
	OnAnyQualifiedFailureCondition,
	[Browsable(false)]
	[LocDisplayName("Unknown")]
	Unknown
}
