namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IDatabaseRole : IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	bool IsFixedRole { get; }

	[ReadWrite]
	[Reference]
	IDatabasePrincipal Owner { get; }
}
