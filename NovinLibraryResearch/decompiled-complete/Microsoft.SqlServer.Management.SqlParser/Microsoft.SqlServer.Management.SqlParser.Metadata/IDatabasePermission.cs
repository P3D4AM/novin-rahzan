namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("Name", SpecialValue.RandomGuid)]
public interface IDatabasePermission : IMetadataObject
{
	[ReadOnly]
	[Parent]
	IDatabasePrincipal DatabasePrincipal { get; }

	[ReadOnly]
	DatabasePermissionType PermissionType { get; }

	[ReadOnly]
	[Reference]
	IMetadataObject TargetObject { get; }

	[ReadWrite]
	PermissionState PermissionState { get; }

	[ReadOnly]
	IDatabasePrincipal Grantor { get; }
}
