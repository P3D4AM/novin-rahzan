using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[StateChangeEvent("CREATE_DATABASE", "DATABASE")]
[StateChangeEvent("ALTER_DATABASE", "DATABASE")]
[StateChangeEvent("ALTER_AUTHORIZATION_DATABASE", "DATABASE")]
[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnChanges | AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[TypeConverter(typeof(LocalizableTypeConverter))]
[DisplayNameKey("IDatabaseOptions_Name")]
[DisplayDescriptionKey("IDatabaseOptions_Desc")]
public interface IDatabaseOptions : IDmfFacet
{
	[DisplayNameKey("Database_AnsiNullDefaultName")]
	[DisplayDescriptionKey("Database_AnsiNullDefaultDesc")]
	bool AnsiNullDefault { get; set; }

	[DisplayNameKey("Database_AnsiNullsEnabledName")]
	[DisplayDescriptionKey("Database_AnsiNullsEnabledDesc")]
	bool AnsiNullsEnabled { get; set; }

	[DisplayNameKey("Database_AnsiPaddingEnabledName")]
	[DisplayDescriptionKey("Database_AnsiPaddingEnabledDesc")]
	bool AnsiPaddingEnabled { get; set; }

	[DisplayNameKey("Database_AnsiWarningsEnabledName")]
	[DisplayDescriptionKey("Database_AnsiWarningsEnabledDesc")]
	bool AnsiWarningsEnabled { get; set; }

	[DisplayNameKey("Database_ArithmeticAbortEnabledName")]
	[DisplayDescriptionKey("Database_ArithmeticAbortEnabledDesc")]
	bool ArithmeticAbortEnabled { get; set; }

	[DisplayNameKey("Database_AutoCloseName")]
	[DisplayDescriptionKey("Database_AutoCloseDesc")]
	bool AutoClose { get; set; }

	[DisplayNameKey("Database_AutoCreateStatisticsEnabledName")]
	[DisplayDescriptionKey("Database_AutoCreateStatisticsEnabledDesc")]
	bool AutoCreateStatisticsEnabled { get; set; }

	[DisplayNameKey("Database_AutoCreateIncrementalStatisticsEnabledName")]
	[DisplayDescriptionKey("Database_AutoCreateIncrementalStatisticsEnabledDesc")]
	bool AutoCreateIncrementalStatisticsEnabled { get; set; }

	[DisplayNameKey("Database_AutoShrinkName")]
	[DisplayDescriptionKey("Database_AutoShrinkDesc")]
	bool AutoShrink { get; set; }

	[DisplayNameKey("Database_AutoUpdateStatisticsAsyncName")]
	[DisplayDescriptionKey("Database_AutoUpdateStatisticsAsyncDesc")]
	bool AutoUpdateStatisticsAsync { get; set; }

	[DisplayNameKey("Database_AutoUpdateStatisticsEnabledName")]
	[DisplayDescriptionKey("Database_AutoUpdateStatisticsEnabledDesc")]
	bool AutoUpdateStatisticsEnabled { get; set; }

	[DisplayNameKey("Database_BrokerEnabledName")]
	[DisplayDescriptionKey("Database_BrokerEnabledDesc")]
	bool BrokerEnabled { get; set; }

	[DisplayNameKey("Database_ChangeTrackingAutoCleanUpName")]
	[DisplayDescriptionKey("Database_ChangeTrackingAutoCleanUpDesc")]
	bool ChangeTrackingAutoCleanUp { get; set; }

	[DisplayNameKey("Database_ChangeTrackingEnabledName")]
	[DisplayDescriptionKey("Database_ChangeTrackingEnabledDesc")]
	bool ChangeTrackingEnabled { get; set; }

	[DisplayNameKey("Database_ChangeTrackingRetentionPeriodName")]
	[DisplayDescriptionKey("Database_ChangeTrackingRetentionPeriodDesc")]
	int ChangeTrackingRetentionPeriod { get; set; }

	[DisplayNameKey("Database_ChangeTrackingRetentionPeriodUnitsName")]
	[DisplayDescriptionKey("Database_ChangeTrackingRetentionPeriodUnitsDesc")]
	RetentionPeriodUnits ChangeTrackingRetentionPeriodUnits { get; set; }

	[DisplayNameKey("Database_CloseCursorsOnCommitEnabledName")]
	[DisplayDescriptionKey("Database_CloseCursorsOnCommitEnabledDesc")]
	bool CloseCursorsOnCommitEnabled { get; set; }

	[DisplayNameKey("Database_CollationName")]
	[DisplayDescriptionKey("Database_CollationDesc")]
	string Collation { get; set; }

	[DisplayNameKey("Database_CompatibilityLevelName")]
	[DisplayDescriptionKey("Database_CompatibilityLevelDesc")]
	CompatibilityLevel CompatibilityLevel { get; set; }

	[DisplayNameKey("Database_ConcatenateNullYieldsNullName")]
	[DisplayDescriptionKey("Database_ConcatenateNullYieldsNullDesc")]
	bool ConcatenateNullYieldsNull { get; set; }

	[DisplayNameKey("Database_CreateDateName")]
	[DisplayDescriptionKey("Database_CreateDateDesc")]
	DateTime CreateDate { get; }

	[DisplayNameKey("Database_DatabaseOwnershipChainingName")]
	[DisplayDescriptionKey("Database_DatabaseOwnershipChainingDesc")]
	bool DatabaseOwnershipChaining { get; set; }

	[DisplayNameKey("Database_DatabaseSnapshotBaseNameName")]
	[DisplayDescriptionKey("Database_DatabaseSnapshotBaseNameDesc")]
	string DatabaseSnapshotBaseName { get; }

	[DisplayNameKey("Database_DateCorrelationOptimizationName")]
	[DisplayDescriptionKey("Database_DateCorrelationOptimizationDesc")]
	bool DateCorrelationOptimization { get; set; }

	[DisplayNameKey("Database_DefaultFileGroupName")]
	[DisplayDescriptionKey("Database_DefaultFileGroupDesc")]
	string DefaultFileGroup { get; }

	[DisplayNameKey("Database_DefaultFileStreamFileGroupName")]
	[DisplayDescriptionKey("Database_DefaultFileStreamFileGroupDesc")]
	string DefaultFileStreamFileGroup { get; }

	[DisplayNameKey("Database_EncryptionEnabledName")]
	[DisplayDescriptionKey("Database_EncryptionEnabledDesc")]
	bool EncryptionEnabled { get; set; }

	[DisplayNameKey("Database_HonorBrokerPriorityName")]
	[DisplayDescriptionKey("Database_HonorBrokerPriorityDesc")]
	bool HonorBrokerPriority { get; set; }

	[DisplayNameKey("Database_IDName")]
	[DisplayDescriptionKey("Database_IDDesc")]
	int ID { get; }

	[DisplayNameKey("Database_IsParameterizationForcedName")]
	[DisplayDescriptionKey("Database_IsParameterizationForcedDesc")]
	bool IsParameterizationForced { get; set; }

	[DisplayNameKey("Database_IsReadCommittedSnapshotOnName")]
	[DisplayDescriptionKey("Database_IsReadCommittedSnapshotOnDesc")]
	bool IsReadCommittedSnapshotOn { get; set; }

	[DisplayNameKey("Database_IsSystemObjectName")]
	[DisplayDescriptionKey("Database_IsSystemObjectDesc")]
	bool IsSystemObject { get; }

	[DisplayNameKey("Database_IsUpdateableName")]
	[DisplayDescriptionKey("Database_IsUpdateableDesc")]
	bool IsUpdateable { get; }

	[DisplayNameKey("Database_LocalCursorsDefaultName")]
	[DisplayDescriptionKey("Database_LocalCursorsDefaultDesc")]
	bool LocalCursorsDefault { get; set; }

	[DisplayNameKey("NamedSmoObject_NameName")]
	[DisplayDescriptionKey("NamedSmoObject_NameDesc")]
	string Name { get; }

	[DisplayNameKey("Database_OwnerName")]
	[DisplayDescriptionKey("Database_OwnerDesc")]
	string Owner { get; }

	[DisplayNameKey("Database_NumericRoundAbortEnabledName")]
	[DisplayDescriptionKey("Database_NumericRoundAbortEnabledDesc")]
	bool NumericRoundAbortEnabled { get; set; }

	[DisplayNameKey("Database_MirroringTimeoutName")]
	[DisplayDescriptionKey("Database_MirroringTimeoutDesc")]
	int MirroringTimeout { get; set; }

	[DisplayNameKey("Database_PageVerifyName")]
	[DisplayDescriptionKey("Database_PageVerifyDesc")]
	PageVerify PageVerify { get; set; }

	[DisplayNameKey("Database_PrimaryFilePathName")]
	[DisplayDescriptionKey("Database_PrimaryFilePathDesc")]
	string PrimaryFilePath { get; }

	[DisplayNameKey("Database_QuotedIdentifiersEnabledName")]
	[DisplayDescriptionKey("Database_QuotedIdentifiersEnabledDesc")]
	bool QuotedIdentifiersEnabled { get; set; }

	[DisplayNameKey("Database_ReadOnlyName")]
	[DisplayDescriptionKey("Database_ReadOnlyDesc")]
	bool ReadOnly { get; set; }

	[DisplayNameKey("Database_RecoveryModelName")]
	[DisplayDescriptionKey("Database_RecoveryModelDesc")]
	RecoveryModel RecoveryModel { get; set; }

	[DisplayNameKey("Database_RecursiveTriggersEnabledName")]
	[DisplayDescriptionKey("Database_RecursiveTriggersEnabledDesc")]
	bool RecursiveTriggersEnabled { get; set; }

	[DisplayNameKey("Database_RemoteDataArchiveEnabledName")]
	[DisplayDescriptionKey("Database_RemoteDataArchiveEnabledDesc")]
	bool RemoteDataArchiveEnabled { get; set; }

	[DisplayNameKey("Database_RemoteDataArchiveEndpointName")]
	[DisplayDescriptionKey("Database_RemoteDataArchiveEndpointDesc")]
	string RemoteDataArchiveEndpoint { get; set; }

	[DisplayNameKey("Database_RemoteDataArchiveLinkedServerName")]
	[DisplayDescriptionKey("Database_RemoteDataArchiveLinkedServerDesc")]
	string RemoteDataArchiveLinkedServer { get; }

	[DisplayNameKey("Database_RemoteDatabaseNameName")]
	[DisplayDescriptionKey("Database_RemoteDatabaseNameDesc")]
	string RemoteDatabaseName { get; }

	[DisplayNameKey("Database_RemoteDataArchiveUseFederatedServiceAccount")]
	[DisplayDescriptionKey("Database_RemoteDataArchiveUseFederatedServiceAccountDesc")]
	bool RemoteDataArchiveUseFederatedServiceAccount { get; }

	[DisplayNameKey("Database_RemoteDataArchiveCredentialName")]
	[DisplayDescriptionKey("Database_RemoteDataArchiveCredentialDesc")]
	string RemoteDataArchiveCredential { get; }

	[DisplayNameKey("Database_TrustworthyName")]
	[DisplayDescriptionKey("Database_TrustworthyDesc")]
	bool Trustworthy { get; set; }

	[DisplayNameKey("Database_UserAccessName")]
	[DisplayDescriptionKey("Database_UserAccessDesc")]
	DatabaseUserAccess UserAccess { get; set; }

	[DisplayNameKey("Database_TargetRecoveryTimeName")]
	[DisplayDescriptionKey("Database_TargetRecoveryTimeDesc")]
	int TargetRecoveryTime { get; set; }

	[DisplayNameKey("Database_DelayedDurabilityName")]
	[DisplayDescriptionKey("Database_DelayedDurabilityDesc")]
	DelayedDurability DelayedDurability { get; set; }
}
