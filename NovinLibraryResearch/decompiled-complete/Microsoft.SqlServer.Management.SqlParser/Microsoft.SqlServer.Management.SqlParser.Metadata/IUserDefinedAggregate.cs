namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("ScalarType", ScalarType.ScalarFunction)]
[Constant("IsAggregateFunction", true)]
public interface IUserDefinedAggregate : IScalarFunction, IFunction, IMetadataObject, IFunctionModuleBase, IScalar, ISchemaOwnedObject, IDatabaseObject
{
}
