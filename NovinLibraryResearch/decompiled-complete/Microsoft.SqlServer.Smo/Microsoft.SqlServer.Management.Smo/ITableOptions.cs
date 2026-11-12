using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[StateChangeEvent("CREATE_TABLE", "TABLE")]
[StateChangeEvent("ALTER_TABLE", "TABLE")]
[StateChangeEvent("RENAME", "TABLE")]
[StateChangeEvent("ALTER_AUTHORIZATION_DATABASE", "TABLE")]
[StateChangeEvent("ALTER_SCHEMA", "TABLE")]
[EvaluationMode(AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges | AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[TypeConverter(typeof(LocalizableTypeConverter))]
[DisplayNameKey("ITableOptions_Name")]
[DisplayDescriptionKey("ITableOptions_Desc")]
public interface ITableOptions : IDmfFacet
{
	[DisplayNameKey("Table_AnsiNullsStatusName")]
	[DisplayDescriptionKey("Table_AnsiNullsStatusDesc")]
	bool AnsiNullsStatus { get; set; }

	[DisplayNameKey("Table_ChangeTrackingEnabledName")]
	[DisplayDescriptionKey("Table_ChangeTrackingEnabledDesc")]
	bool ChangeTrackingEnabled { get; set; }

	[DisplayNameKey("Table_CreateDateName")]
	[DisplayDescriptionKey("Table_CreateDateDesc")]
	DateTime CreateDate { get; }

	[DisplayNameKey("Table_FakeSystemTableName")]
	[DisplayDescriptionKey("Table_FakeSystemTableDesc")]
	bool FakeSystemTable { get; }

	[DisplayNameKey("Table_IDName")]
	[DisplayDescriptionKey("Table_IDDesc")]
	int ID { get; }

	[DisplayNameKey("Table_IsSchemaOwnedName")]
	[DisplayDescriptionKey("Table_IsSchemaOwnedDesc")]
	bool IsSchemaOwned { get; }

	[DisplayNameKey("Table_IsSystemObjectName")]
	[DisplayDescriptionKey("Table_IsSystemObjectDesc")]
	bool IsSystemObject { get; }

	[DisplayNameKey("Table_LockEscalationName")]
	[DisplayDescriptionKey("Table_LockEscalationDesc")]
	LockEscalationType LockEscalation { get; set; }

	[DisplayNameKey("NamedSmoObject_NameName")]
	[DisplayDescriptionKey("NamedSmoObject_NameDesc")]
	string Name { get; }

	[DisplayNameKey("Table_OwnerName")]
	[DisplayDescriptionKey("Table_OwnerDesc")]
	string Owner { get; set; }

	[DisplayNameKey("Table_QuotedIdentifierStatusName")]
	[DisplayDescriptionKey("Table_QuotedIdentifierStatusDesc")]
	bool QuotedIdentifierStatus { get; }

	[DisplayNameKey("Table_RemoteDataArchiveEnabledName")]
	[DisplayDescriptionKey("Table_RemoteDataArchiveEnabledDesc")]
	bool RemoteDataArchiveEnabled { get; set; }

	[DisplayNameKey("Table_RemoteDataArchiveDataMigrationStateName")]
	[DisplayDescriptionKey("Table_RemoteDataArchiveDataMigrationStateDesc")]
	RemoteDataArchiveMigrationState RemoteDataArchiveDataMigrationState { get; set; }

	[DisplayNameKey("Table_RemoteTableNameName")]
	[DisplayDescriptionKey("Table_RemoteTableNameDesc")]
	string RemoteTableName { get; }

	[DisplayNameKey("Table_RemoteTableProvisionedName")]
	[DisplayDescriptionKey("Table_RemoteTableProvisionedDesc")]
	bool RemoteTableProvisioned { get; }

	[DisplayNameKey("Table_ReplicatedName")]
	[DisplayDescriptionKey("Table_ReplicatedDesc")]
	bool Replicated { get; }

	[DisplayNameKey("ScriptSchemaObjectBase_SchemaName")]
	[DisplayDescriptionKey("ScriptSchemaObjectBase_SchemaDesc")]
	string Schema { get; }

	[DisplayNameKey("Table_TrackColumnsUpdatedEnabledName")]
	[DisplayDescriptionKey("Table_TrackColumnsUpdatedEnabledDesc")]
	bool TrackColumnsUpdatedEnabled { get; set; }
}
