using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[CLSCompliant(false)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("ServerPerformanceName")]
[DisplayDescriptionKey("ServerPerformanceDesc")]
public interface IServerPerformanceFacet : IDmfFacet
{
	[DisplayNameKey("AffinityMaskName")]
	[DisplayDescriptionKey("AffinityMaskDesc")]
	int AffinityMask { get; }

	[DisplayNameKey("Affinity64MaskName")]
	[DisplayDescriptionKey("Affinity64MaskDesc")]
	int Affinity64Mask { get; }

	[DisplayNameKey("AffinityMaskIOName")]
	[DisplayDescriptionKey("AffinityMaskIODesc")]
	[PostConfigurationAction(PostConfigurationAction.RestartService)]
	int AffinityIOMask { get; set; }

	[DisplayNameKey("Affinity64IOMaskName")]
	[DisplayDescriptionKey("Affinity64IOMaskDesc")]
	int Affinity64IOMask { get; }

	[DisplayNameKey("BlockedProcessThresholdName")]
	[DisplayDescriptionKey("ServerPerformanceDesc")]
	int BlockedProcessThreshold { get; set; }

	[DisplayNameKey("DynamicLocksName")]
	[DisplayDescriptionKey("DynamicLocksDesc")]
	[PostConfigurationAction(PostConfigurationAction.RestartService)]
	int DynamicLocks { get; set; }

	[DisplayNameKey("LightweightPoolingEnabledName")]
	[DisplayDescriptionKey("LightweightPoolingEnabledDesc")]
	[PostConfigurationAction(PostConfigurationAction.RestartService)]
	bool LightweightPoolingEnabled { get; set; }

	[DisplayNameKey("ServerPerformanceName")]
	[DisplayDescriptionKey("ServerPerformanceDesc")]
	int MaxDegreeOfParallelism { get; set; }

	[DisplayNameKey("CostThresholdforParallelismName")]
	[DisplayDescriptionKey("CostThresholdforParallelismDesc")]
	int CostThresholdForParallelism { get; set; }

	[DisplayNameKey("MaxWorkerThreadsName")]
	[DisplayDescriptionKey("MaxWorkerThreadsDesc")]
	[PostConfigurationAction(PostConfigurationAction.RestartService)]
	int MaxWorkerThreads { get; set; }

	[DisplayNameKey("NetworkPacketSizeName")]
	[DisplayDescriptionKey("NetworkPacketSizeDesc")]
	int NetworkPacketSize { get; set; }

	[DisplayNameKey("OpenObjectsName")]
	[DisplayDescriptionKey("OpenObjectsDesc")]
	[PostConfigurationAction(PostConfigurationAction.RestartService)]
	int OpenObjects { get; set; }
}
