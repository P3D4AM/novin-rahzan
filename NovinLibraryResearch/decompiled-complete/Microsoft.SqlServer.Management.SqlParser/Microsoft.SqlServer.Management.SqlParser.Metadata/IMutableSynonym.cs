namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IMutableSynonym : ISynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	new SynonymBaseType BaseType { get; set; }

	[ReadWrite]
	new string BaseObjectName { get; set; }
}
