namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDatabasePermission : IDatabasePermission, IMetadataObject
{
	new PermissionState PermissionState { get; set; }
}
