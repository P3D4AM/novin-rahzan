namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedTableValuedFunctionSynonym : IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, ITableValuedFunction, IDatabaseTable, ITabular, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase
{
	new ITableValuedFunction TargetObject { get; }
}
