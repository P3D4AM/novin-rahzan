namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IFileGroup : IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadOnly]
	bool IsDefault { get; }

	[ReadOnly]
	bool IsFileStream { get; }

	[ReadOnly]
	bool IsReadOnly { get; }
}
