namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IPartitionScheme : IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	[Child]
	IMetadataCollection<IFileGroup> FileGroups { get; }
}
