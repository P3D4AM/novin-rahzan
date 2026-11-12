namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IDatabasePrincipal : IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	[Child]
	IMetadataCollection<IDatabasePermission> Permissions { get; }

	[ReadWrite]
	[Reference]
	IMetadataCollection<IDatabaseRole> MemberOfRoles { get; }
}
