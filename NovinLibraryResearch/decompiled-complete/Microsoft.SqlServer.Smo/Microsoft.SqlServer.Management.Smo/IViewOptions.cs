using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[StateChangeEvent("CREATE_VIEW", "VIEW")]
[StateChangeEvent("ALTER_VIEW", "VIEW")]
[StateChangeEvent("RENAME", "VIEW")]
[StateChangeEvent("ALTER_AUTHORIZATION_DATABASE", "VIEW")]
[StateChangeEvent("ALTER_SCHEMA", "VIEW")]
[EvaluationMode(AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges | AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[TypeConverter(typeof(LocalizableTypeConverter))]
[DisplayNameKey("IViewOptions_Name")]
[DisplayDescriptionKey("IViewOptions_Desc")]
public interface IViewOptions : IDmfFacet
{
	[DisplayNameKey("View_AnsiNullsStatusName")]
	[DisplayDescriptionKey("View_AnsiNullsStatusDesc")]
	bool AnsiNullsStatus { get; }

	[DisplayNameKey("View_CreateDateName")]
	[DisplayDescriptionKey("View_CreateDateDesc")]
	DateTime CreateDate { get; }

	[DisplayNameKey("View_IDName")]
	[DisplayDescriptionKey("View_IDDesc")]
	int ID { get; }

	[DisplayNameKey("View_IsEncryptedName")]
	[DisplayDescriptionKey("View_IsEncryptedDesc")]
	bool IsEncrypted { get; }

	[DisplayNameKey("View_IsSchemaBoundName")]
	[DisplayDescriptionKey("View_IsSchemaBoundDesc")]
	bool IsSchemaBound { get; }

	[DisplayNameKey("View_IsSchemaOwnedName")]
	[DisplayDescriptionKey("View_IsSchemaOwnedDesc")]
	bool IsSchemaOwned { get; }

	[DisplayNameKey("View_IsSystemObjectName")]
	[DisplayDescriptionKey("View_IsSystemObjectDesc")]
	bool IsSystemObject { get; }

	[DisplayNameKey("NamedSmoObject_NameName")]
	[DisplayDescriptionKey("NamedSmoObject_NameDesc")]
	string Name { get; }

	[DisplayNameKey("View_OwnerName")]
	[DisplayDescriptionKey("View_OwnerDesc")]
	string Owner { get; }

	[DisplayNameKey("ScriptSchemaObjectBase_SchemaName")]
	[DisplayDescriptionKey("ScriptSchemaObjectBase_SchemaDesc")]
	string Schema { get; }

	[DisplayNameKey("View_QuotedIdentifierStatusName")]
	[DisplayDescriptionKey("View_QuotedIdentifierStatusDesc")]
	bool QuotedIdentifierStatus { get; }

	[DisplayNameKey("View_ReturnsViewMetadataName")]
	[DisplayDescriptionKey("View_ReturnsViewMetadataDesc")]
	bool ReturnsViewMetadata { get; }
}
