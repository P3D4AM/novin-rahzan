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
[StateChangeEvent("CREATE_VIEW", "VIEW")]
[StateChangeEvent("ALTER_VIEW", "VIEW")]
[StateChangeEvent("RENAME", "VIEW")]
[StateChangeEvent("CREATE_FUNCTION", "FUNCTION")]
[StateChangeEvent("ALTER_FUNCTION", "FUNCTION")]
[StateChangeEvent("RENAME", "FUNCTION")]
[StateChangeEvent("CREATE_PROCEDURE", "PROCEDURE")]
[StateChangeEvent("ALTER_PROCEDURE", "PROCEDURE")]
[StateChangeEvent("RENAME", "PROCEDURE")]
[StateChangeEvent("CREATE_SYNONYM", "SYNONYM")]
[StateChangeEvent("CREATE_SEQUENCE", "SEQUENCE")]
[StateChangeEvent("ALTER_SEQUENCE", "SEQUENCE")]
[StateChangeEvent("RENAME", "SEQUENCE")]
[StateChangeEvent("CREATE_TYPE", "TYPE")]
[StateChangeEvent("RENAME", "TYPE")]
[StateChangeEvent("CREATE_XML_SCHEMA_COLLECTION", "XMLSCHEMACOLLECTION")]
[StateChangeEvent("ALTER_XML_SCHEMA_COLLECTION", "XMLSCHEMACOLLECTION")]
[StateChangeEvent("RENAME", "XMLSCHEMACOLLECTION")]
[StateChangeEvent("ALTER_SCHEMA", "TABLE")]
[StateChangeEvent("ALTER_SCHEMA", "VIEW")]
[StateChangeEvent("ALTER_SCHEMA", "FUNCTION")]
[StateChangeEvent("ALTER_SCHEMA", "PROCEDURE")]
[StateChangeEvent("ALTER_SCHEMA", "SYNONYM")]
[StateChangeEvent("ALTER_SCHEMA", "SEQUENCE")]
[StateChangeEvent("ALTER_SCHEMA", "TYPE")]
[StateChangeEvent("ALTER_SCHEMA", "XMLSCHEMACOLLECTION")]
[EvaluationMode(AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges | AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("MultipartNameName")]
[DisplayDescriptionKey("MultipartNameDesc")]
public interface IMultipartNameFacet : IDmfFacet
{
	[DisplayNameKey("NameName")]
	[DisplayDescriptionKey("NameDesc")]
	string Name { get; }

	[DisplayNameKey("SchemaName")]
	[DisplayDescriptionKey("SchemaDesc")]
	string Schema { get; }
}
