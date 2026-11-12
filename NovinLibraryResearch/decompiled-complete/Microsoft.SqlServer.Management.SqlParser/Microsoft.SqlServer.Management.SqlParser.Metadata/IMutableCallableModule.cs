namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableCallableModule : ICallableModule, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableMetadataObject, IMutableFunctionModuleBase
{
	new IScalarDataType ReturnType { get; set; }
}
