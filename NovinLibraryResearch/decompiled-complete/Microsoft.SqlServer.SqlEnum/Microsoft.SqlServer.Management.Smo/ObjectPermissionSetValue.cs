namespace Microsoft.SqlServer.Management.Smo;

public enum ObjectPermissionSetValue
{
	[PermissionType("AL")]
	[PermissionName("ALTER")]
	Alter,
	[PermissionType("CL")]
	[PermissionName("CONTROL")]
	Control,
	[PermissionType("CO")]
	[PermissionName("CONNECT")]
	Connect,
	[PermissionType("DL")]
	[PermissionName("DELETE")]
	Delete,
	[PermissionType("EX")]
	[PermissionName("EXECUTE")]
	Execute,
	[PermissionType("IM")]
	[PermissionName("IMPERSONATE")]
	Impersonate,
	[PermissionType("IN")]
	[PermissionName("INSERT")]
	Insert,
	[PermissionType("RC")]
	[PermissionName("RECEIVE")]
	Receive,
	[PermissionType("RF")]
	[PermissionName("REFERENCES")]
	References,
	[PermissionType("SL")]
	[PermissionName("SELECT")]
	Select,
	[PermissionType("SN")]
	[PermissionName("SEND")]
	Send,
	[PermissionType("TO")]
	[PermissionName("TAKE OWNERSHIP")]
	TakeOwnership,
	[PermissionType("UP")]
	[PermissionName("UPDATE")]
	Update,
	[PermissionType("VW")]
	[PermissionName("VIEW DEFINITION")]
	ViewDefinition,
	[PermissionType("VWCT")]
	[PermissionName("VIEW CHANGE TRACKING")]
	ViewChangeTracking,
	[PermissionType("CRSO")]
	[PermissionName("CREATE SEQUENCE")]
	CreateSequence
}
