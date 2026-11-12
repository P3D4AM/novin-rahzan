namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableScalarValuedFunction : IScalarValuedFunction, IUserDefinedFunction, IFunction, IMetadataObject, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, ICallableModule, IScalarFunction, IScalar, IMutableUserDefinedFunction, IMutableFunction, IMutableFunctionModuleBase, IMutableMetadataObject, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableCallableModule, IMutableScalarFunction, IMutableScalar
{
	new bool ReturnsNullOnNullInput { get; set; }
}
