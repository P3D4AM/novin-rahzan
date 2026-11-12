namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedScalarValuedFunctionSynonym : IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IScalarValuedFunction, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ICallableModule, IScalarFunction, IScalar
{
	new IScalarValuedFunction TargetObject { get; }
}
