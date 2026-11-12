namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IApplicationRole : IDatabasePrincipal, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	[Reference]
	ISchema DefaultSchema { get; }
}
