using System.ComponentModel;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

[TypeConverter(typeof(LocalizedEnumConverter))]
public enum DatabaseObjectType
{
	Table,
	View,
	StoredProcedure,
	UserDefinedFunction,
	UserDefinedDataType,
	User,
	Default,
	Rule,
	DatabaseRole,
	ApplicationRole,
	SqlAssembly,
	DdlTrigger,
	Synonym,
	XmlSchemaCollection,
	Schema,
	SecurityPolicy,
	PlanGuide,
	UserDefinedType,
	UserDefinedAggregate,
	FullTextCatalog,
	UserDefinedTableType
}
