namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedStoredProcedureSynonym : IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IStoredProcedure, ICallableModule, IUserDefinedFunctionModuleBase, IFunctionModuleBase
{
	new IStoredProcedure TargetObject { get; }
}
