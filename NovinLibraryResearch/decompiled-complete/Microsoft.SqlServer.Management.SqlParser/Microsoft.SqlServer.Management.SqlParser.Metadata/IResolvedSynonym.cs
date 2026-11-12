namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IResolvedSynonym : ISchemaOwnedObject, IDatabaseObject, IMetadataObject
{
	ISynonym Synonym { get; }

	ISchemaOwnedObject TargetObject { get; }
}
