namespace Microsoft.SqlServer.Management.Smo;

public enum ServerPermissionSetValue
{
	[PermissionType("ADBO")]
	[PermissionName("ADMINISTER BULK OPERATIONS")]
	AdministerBulkOperations,
	[PermissionType("ALCD")]
	[PermissionName("ALTER ANY CREDENTIAL")]
	AlterAnyCredential,
	[PermissionType("ALCO")]
	[PermissionName("ALTER ANY CONNECTION")]
	AlterAnyConnection,
	[PermissionType("ALDB")]
	[PermissionName("ALTER ANY DATABASE")]
	AlterAnyDatabase,
	[PermissionType("ALES")]
	[PermissionName("ALTER ANY EVENT NOTIFICATION")]
	AlterAnyEventNotification,
	[PermissionType("ALHE")]
	[PermissionName("ALTER ANY ENDPOINT")]
	AlterAnyEndpoint,
	[PermissionType("ALLG")]
	[PermissionName("ALTER ANY LOGIN")]
	AlterAnyLogin,
	[PermissionType("ALLS")]
	[PermissionName("ALTER ANY LINKED SERVER")]
	AlterAnyLinkedServer,
	[PermissionType("ALRS")]
	[PermissionName("ALTER RESOURCES")]
	AlterResources,
	[PermissionType("ALSS")]
	[PermissionName("ALTER SERVER STATE")]
	AlterServerState,
	[PermissionType("ALST")]
	[PermissionName("ALTER SETTINGS")]
	AlterSettings,
	[PermissionType("ALTR")]
	[PermissionName("ALTER TRACE")]
	AlterTrace,
	[PermissionType("AUTH")]
	[PermissionName("AUTHENTICATE SERVER")]
	AuthenticateServer,
	[PermissionType("CL")]
	[PermissionName("CONTROL SERVER")]
	ControlServer,
	[PermissionType("COSQ")]
	[PermissionName("CONNECT SQL")]
	ConnectSql,
	[PermissionType("CRDB")]
	[PermissionName("CREATE ANY DATABASE")]
	CreateAnyDatabase,
	[PermissionType("CRDE")]
	[PermissionName("CREATE DDL EVENT NOTIFICATION")]
	CreateDdlEventNotification,
	[PermissionType("CRHE")]
	[PermissionName("CREATE ENDPOINT")]
	CreateEndpoint,
	[PermissionType("CRTE")]
	[PermissionName("CREATE TRACE EVENT NOTIFICATION")]
	CreateTraceEventNotification,
	[PermissionType("SHDN")]
	[PermissionName("SHUTDOWN")]
	Shutdown,
	[PermissionType("VWAD")]
	[PermissionName("VIEW ANY DEFINITION")]
	ViewAnyDefinition,
	[PermissionType("VWDB")]
	[PermissionName("VIEW ANY DATABASE")]
	ViewAnyDatabase,
	[PermissionType("VWSS")]
	[PermissionName("VIEW SERVER STATE")]
	ViewServerState,
	[PermissionType("XA")]
	[PermissionName("EXTERNAL ACCESS ASSEMBLY")]
	ExternalAccessAssembly,
	[PermissionType("XU")]
	[PermissionName("UNSAFE ASSEMBLY")]
	UnsafeAssembly,
	[PermissionType("ALAA")]
	[PermissionName("ALTER ANY SERVER AUDIT")]
	AlterAnyServerAudit,
	[PermissionType("ALSR")]
	[PermissionName("ALTER ANY SERVER ROLE")]
	AlterAnyServerRole,
	[PermissionType("CRSR")]
	[PermissionName("CREATE SERVER ROLE")]
	CreateServerRole,
	[PermissionType("ALAG")]
	[PermissionName("ALTER ANY AVAILABILITY GROUP")]
	AlterAnyAvailabilityGroup,
	[PermissionType("CRAC")]
	[PermissionName("CREATE AVAILABILITY GROUP")]
	CreateAvailabilityGroup,
	[PermissionType("AAES")]
	[PermissionName("ALTER ANY EVENT SESSION")]
	AlterAnyEventSession,
	[PermissionType("SUS")]
	[PermissionName("SELECT ALL USER SECURABLES")]
	SelectAllUserSecurables,
	[PermissionType("CADB")]
	[PermissionName("CONNECT ANY DATABASE")]
	ConnectAnyDatabase,
	[PermissionType("IAL")]
	[PermissionName("IMPERSONATE ANY LOGIN")]
	ImpersonateAnyLogin
}
