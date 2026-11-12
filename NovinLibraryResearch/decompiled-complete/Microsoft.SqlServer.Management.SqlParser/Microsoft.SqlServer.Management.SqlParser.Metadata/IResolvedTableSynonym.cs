namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedTableSynonym : IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, ITable, ITableViewBase, IDatabaseTable, ITabular
{
	new ITable TargetObject { get; }
}
