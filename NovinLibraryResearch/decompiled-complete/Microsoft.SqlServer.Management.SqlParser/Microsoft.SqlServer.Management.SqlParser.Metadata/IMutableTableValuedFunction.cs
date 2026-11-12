namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableTableValuedFunction : ITableValuedFunction, IDatabaseTable, ITabular, IMetadataObject, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMutableDatabaseTable, IMutableTabular, IMutableMetadataObject, IMutableUserDefinedFunction, IMutableFunction, IMutableFunctionModuleBase, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	new bool IsInline { get; set; }

	new string TableVariableName { get; set; }
}
