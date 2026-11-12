using System.ComponentModel;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("SmartAdminStateName")]
[DisplayDescriptionKey("SmartAdminStateDesc")]
public interface ISmartAdminState : IDmfFacet, IRefreshable
{
	[DisplayNameKey("SmartAdminState_IsMasterSwitchEnabledName")]
	[DisplayDescriptionKey("SmartAdminState_IsMasterSwitchEnabledDesc")]
	bool IsMasterSwitchEnabled { get; }

	[DisplayNameKey("SmartAdminState_IsBackupEnabledName")]
	[DisplayDescriptionKey("SmartAdminState_IsBackupEnabledDesc")]
	bool IsBackupEnabled { get; }

	[DisplayNameKey("SmartAdminState_NumberOfStorageConnectivityErrorsName")]
	[DisplayDescriptionKey("SmartAdminState_NumberOfStorageConnectivityErrorsDesc")]
	int NumberOfStorageConnectivityErrors { get; }

	[DisplayNameKey("SmartAdminState_NumberOfSqlErrorsName")]
	[DisplayDescriptionKey("SmartAdminState_NumberOfSqlErrorsDesc")]
	int NumberOfSqlErrors { get; }

	[DisplayNameKey("SmartAdminState_NumberOfInvalidCredentialErrorsName")]
	[DisplayDescriptionKey("SmartAdminState_NumberOfInvalidCredentialErrorsDesc")]
	int NumberOfInvalidCredentialErrors { get; }

	[DisplayNameKey("SmartAdminState_NumberOfOtherErrorsName")]
	[DisplayDescriptionKey("SmartAdminState_NumberOfOtherErrorsDesc")]
	int NumberOfOtherErrors { get; }

	[DisplayNameKey("SmartAdminState_NumberOfCorruptedOrDeletedBackupsName")]
	[DisplayDescriptionKey("SmartAdminState_NumberOfCorruptedOrDeletedBackupsDesc")]
	int NumberOfCorruptedOrDeletedBackups { get; }

	[DisplayNameKey("SmartAdminState_NumberOfBackupLoopsName")]
	[DisplayDescriptionKey("SmartAdminState_NumberOfBackupLoopsDesc")]
	int NumberOfBackupLoops { get; }

	[DisplayNameKey("SmartAdminState_NumberOfRetentionLoopsName")]
	[DisplayDescriptionKey("SmartAdminState_NumberOfRetentionLoopsDesc")]
	int NumberOfRetentionLoops { get; }
}
