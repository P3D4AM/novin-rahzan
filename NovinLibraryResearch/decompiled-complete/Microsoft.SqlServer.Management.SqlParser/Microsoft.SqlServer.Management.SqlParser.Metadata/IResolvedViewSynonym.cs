namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedViewSynonym : IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IView, ITableViewBase, IDatabaseTable, ITabular
{
	new IView TargetObject { get; }
}
