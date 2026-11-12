namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableUserDefinedFunction : IUserDefinedFunction, IFunction, IMetadataObject, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMutableFunction, IMutableFunctionModuleBase, IMutableMetadataObject, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	new string BodyText { get; set; }

	new bool IsSchemaBound { get; set; }

	new bool IsSqlClr { get; set; }

	new bool IsQuotedIdentifierOn { get; set; }
}
