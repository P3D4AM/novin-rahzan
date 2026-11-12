using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[CLSCompliant(false)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[DisplayNameKey("IDatabasePerformanceFacet_Name")]
[DisplayDescriptionKey("IDatabasePerformanceFacet_Desc")]
public interface IDatabasePerformanceFacet : IDmfFacet
{
	[DisplayNameKey("Database_AutoCloseName")]
	[DisplayDescriptionKey("Database_AutoCloseDesc")]
	bool AutoClose { get; set; }

	[DisplayNameKey("Database_AutoShrinkName")]
	[DisplayDescriptionKey("Database_AutoShrinkDesc")]
	bool AutoShrink { get; set; }

	[DisplayNameKey("Database_SizeName")]
	[DisplayDescriptionKey("Database_SizeDesc")]
	double Size { get; }

	[DisplayNameKey("IDatabasePerformanceFacet_DataAndLogFilesOnSeparateLogicalVolumesName")]
	[DisplayDescriptionKey("IDatabasePerformanceFacet_DataAndLogFilesOnSeparateLogicalVolumesDesc")]
	bool DataAndLogFilesOnSeparateLogicalVolumes { get; }

	[DisplayNameKey("IDatabasePerformanceFacet_CollationMatchesModelOrMasterName")]
	[DisplayDescriptionKey("IDatabasePerformanceFacet_CollationMatchesModelOrMasterDesc")]
	bool CollationMatchesModelOrMaster { get; }

	[DisplayNameKey("Database_IsSystemObjectName")]
	[DisplayDescriptionKey("Database_IsSystemObjectDesc")]
	bool IsSystemObject { get; }

	[DisplayNameKey("Database_StatusName")]
	[DisplayDescriptionKey("Database_StatusDesc")]
	DatabaseStatus Status { get; }
}
