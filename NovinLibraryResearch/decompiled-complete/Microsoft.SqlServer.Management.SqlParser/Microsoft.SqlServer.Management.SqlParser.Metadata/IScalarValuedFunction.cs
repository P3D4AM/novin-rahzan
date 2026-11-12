namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("ModuleType", CallableModuleType.ScalarFunction)]
[Constant("ScalarType", ScalarType.ScalarFunction)]
[Constant("IsAggregateFunction", false)]
[Duplicate("ReturnType", "DataType")]
public interface IScalarValuedFunction : IUserDefinedFunction, IFunction, IMetadataObject, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, ICallableModule, IScalarFunction, IScalar
{
	[ReadWrite]
	bool ReturnsNullOnNullInput { get; }
}
