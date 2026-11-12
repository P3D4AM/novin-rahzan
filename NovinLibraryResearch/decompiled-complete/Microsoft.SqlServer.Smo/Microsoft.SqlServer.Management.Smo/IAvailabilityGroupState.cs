using System.ComponentModel;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("AvailabilityGroupStateName")]
[DisplayDescriptionKey("AvailabilityGroupStateDesc")]
public interface IAvailabilityGroupState : IDmfFacet, IRefreshable
{
	[DisplayNameKey("AvailabilityGroupState_IsOnlineName")]
	[DisplayDescriptionKey("AvailabilityGroupState_IsOnlineDesc")]
	bool IsOnline { get; }

	[DisplayNameKey("AvailabilityGroupState_IsAutoFailoverName")]
	[DisplayDescriptionKey("AvailabilityGroupState_IsAutoFailoverDesc")]
	bool IsAutoFailover { get; }

	[DisplayNameKey("AvailabilityGroupState_NumberOfSynchronizedSecondaryReplicasName")]
	[DisplayDescriptionKey("AvailabilityGroupState_NumberOfSynchronizedSecondaryReplicasDesc")]
	int NumberOfSynchronizedSecondaryReplicas { get; }

	[DisplayNameKey("AvailabilityGroupState_NumberOfNotSynchronizingReplicasName")]
	[DisplayDescriptionKey("AvailabilityGroupState_NumberOfNotSynchronizingReplicasDesc")]
	int NumberOfNotSynchronizingReplicas { get; }

	[DisplayNameKey("AvailabilityGroupState_NumberOfNotSynchronizedReplicasName")]
	[DisplayDescriptionKey("AvailabilityGroupState_NumberOfNotSynchronizedReplicasDesc")]
	int NumberOfNotSynchronizedReplicas { get; }

	[DisplayNameKey("AvailabilityGroupState_NumberOfReplicasWithUnhealthyRoleName")]
	[DisplayDescriptionKey("AvailabilityGroupState_NumberOfReplicasWithUnhealthyRoleDesc")]
	int NumberOfReplicasWithUnhealthyRole { get; }

	[DisplayNameKey("AvailabilityGroupState_NumberOfDisconnectedReplicasName")]
	[DisplayDescriptionKey("AvailabilityGroupState_NumberOfDisconnectedReplicasDesc")]
	int NumberOfDisconnectedReplicas { get; }
}
