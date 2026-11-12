namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableUserDefinedFunctionModuleBase : IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new bool IsEncrypted { get; set; }

	new IExecutionContext ExecutionContext { get; set; }
}
