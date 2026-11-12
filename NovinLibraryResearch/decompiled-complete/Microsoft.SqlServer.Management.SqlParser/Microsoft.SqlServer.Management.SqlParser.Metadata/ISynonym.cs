namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface ISynonym : ISchemaOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	SynonymBaseType BaseType { get; }

	[ReadWrite]
	string BaseObjectName { get; }
}
