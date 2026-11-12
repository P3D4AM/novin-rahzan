namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface ICallableModule : IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase
{
	[Abstract]
	CallableModuleType ModuleType { get; }

	[Abstract]
	IScalarDataType ReturnType { get; }
}
