namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDatabaseObject : IDatabaseObject, IMetadataObject, IMutableMetadataObject
{
	new bool IsSystemObject { get; set; }
}
