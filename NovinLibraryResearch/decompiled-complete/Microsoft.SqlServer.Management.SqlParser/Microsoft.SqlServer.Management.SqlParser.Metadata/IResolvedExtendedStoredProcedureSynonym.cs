namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedExtendedStoredProcedureSynonym : IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IExtendedStoredProcedure, ICallableModule, IUserDefinedFunctionModuleBase, IFunctionModuleBase
{
	new IExtendedStoredProcedure TargetObject { get; }
}
