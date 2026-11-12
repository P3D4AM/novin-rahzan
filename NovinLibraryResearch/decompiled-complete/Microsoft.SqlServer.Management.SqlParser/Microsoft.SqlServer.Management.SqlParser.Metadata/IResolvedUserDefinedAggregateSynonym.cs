namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedUserDefinedAggregateSynonym : IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IUserDefinedAggregate, IScalarFunction, IFunction, IFunctionModuleBase, IScalar
{
	new IUserDefinedAggregate TargetObject { get; }
}
