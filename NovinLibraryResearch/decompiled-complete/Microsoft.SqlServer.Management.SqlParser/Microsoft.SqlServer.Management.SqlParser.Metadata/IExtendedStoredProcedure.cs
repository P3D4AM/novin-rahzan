namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("ModuleType", CallableModuleType.ExtendedStoredProcedure)]
[ReadWrite("ReturnType")]
[Reference("ReturnType")]
public interface IExtendedStoredProcedure : ICallableModule, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase
{
}
