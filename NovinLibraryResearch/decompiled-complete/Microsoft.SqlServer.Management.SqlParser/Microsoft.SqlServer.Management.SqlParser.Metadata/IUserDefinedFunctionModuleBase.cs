namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IUserDefinedFunctionModuleBase : ISchemaOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	bool IsEncrypted { get; }

	[ReadWrite]
	IExecutionContext ExecutionContext { get; }
}
