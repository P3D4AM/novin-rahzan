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
[DisplayNameKey("IDatabaseMaintenanceFacet_Name")]
[DisplayDescriptionKey("IDatabaseMaintenanceFacet_Desc")]
public interface IDatabaseMaintenanceFacet : IDmfFacet
{
	[DisplayNameKey("Database_RecoveryModelName")]
	[DisplayDescriptionKey("Database_RecoveryModelDesc")]
	RecoveryModel RecoveryModel { get; set; }

	[DisplayNameKey("Database_ReadOnlyName")]
	[DisplayDescriptionKey("Database_ReadOnlyDesc")]
	bool ReadOnly { get; set; }

	[DisplayNameKey("Database_PageVerifyName")]
	[DisplayDescriptionKey("Database_PageVerifyDesc")]
	PageVerify PageVerify { get; set; }

	[DisplayNameKey("Database_StatusName")]
	[DisplayDescriptionKey("Database_StatusDesc")]
	DatabaseStatus Status { get; }

	[DisplayNameKey("Database_LastBackupDateName")]
	[DisplayDescriptionKey("Database_LastBackupDateDesc")]
	DateTime LastBackupDate { get; }

	[DisplayNameKey("Database_LastLogBackupDateName")]
	[DisplayDescriptionKey("Database_LastLogBackupDateDesc")]
	DateTime LastLogBackupDate { get; }

	[DisplayNameKey("IDatabaseMaintenanceFacet_DataAndBackupOnSeparateLogicalVolumesName")]
	[DisplayDescriptionKey("IDatabaseMaintenanceFacet_DataAndBackupOnSeparateLogicalVolumesDesc")]
	bool DataAndBackupOnSeparateLogicalVolumes { get; }

	[DisplayNameKey("Database_TargetRecoveryTimeName")]
	[DisplayDescriptionKey("Database_TargetRecoveryTimeDesc")]
	int TargetRecoveryTime { get; set; }

	[DisplayNameKey("Database_DelayedDurabilityName")]
	[DisplayDescriptionKey("Database_DelayedDurabilityDesc")]
	DelayedDurability DelayedDurability { get; set; }
}
