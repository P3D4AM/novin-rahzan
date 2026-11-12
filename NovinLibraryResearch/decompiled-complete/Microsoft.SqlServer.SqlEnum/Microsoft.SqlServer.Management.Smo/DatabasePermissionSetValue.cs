namespace Microsoft.SqlServer.Management.Smo;

public enum DatabasePermissionSetValue
{
	[PermissionType("AL")]
	[PermissionName("ALTER")]
	Alter,
	[PermissionType("ALAK")]
	[PermissionName("ALTER ANY ASYMMETRIC KEY")]
	AlterAnyAsymmetricKey,
	[PermissionType("ALAR")]
	[PermissionName("ALTER ANY APPLICATION ROLE")]
	AlterAnyApplicationRole,
	[PermissionType("ALAS")]
	[PermissionName("ALTER ANY ASSEMBLY")]
	AlterAnyAssembly,
	[PermissionType("ALCF")]
	[PermissionName("ALTER ANY CERTIFICATE")]
	AlterAnyCertificate,
	[PermissionType("ALDS")]
	[PermissionName("ALTER ANY DATASPACE")]
	AlterAnyDataspace,
	[PermissionType("ALED")]
	[PermissionName("ALTER ANY DATABASE EVENT NOTIFICATION")]
	AlterAnyDatabaseEventNotification,
	[PermissionType("ALFT")]
	[PermissionName("ALTER ANY FULLTEXT CATALOG")]
	AlterAnyFulltextCatalog,
	[PermissionType("ALMT")]
	[PermissionName("ALTER ANY MESSAGE TYPE")]
	AlterAnyMessageType,
	[PermissionType("ALRL")]
	[PermissionName("ALTER ANY ROLE")]
	AlterAnyRole,
	[PermissionType("ALRT")]
	[PermissionName("ALTER ANY ROUTE")]
	AlterAnyRoute,
	[PermissionType("ALSB")]
	[PermissionName("ALTER ANY REMOTE SERVICE BINDING")]
	AlterAnyRemoteServiceBinding,
	[PermissionType("ALSC")]
	[PermissionName("ALTER ANY CONTRACT")]
	AlterAnyContract,
	[PermissionType("ALSK")]
	[PermissionName("ALTER ANY SYMMETRIC KEY")]
	AlterAnySymmetricKey,
	[PermissionType("ALSM")]
	[PermissionName("ALTER ANY SCHEMA")]
	AlterAnySchema,
	[PermissionType("ALSV")]
	[PermissionName("ALTER ANY SERVICE")]
	AlterAnyService,
	[PermissionType("ALTG")]
	[PermissionName("ALTER ANY DATABASE DDL TRIGGER")]
	AlterAnyDatabaseDdlTrigger,
	[PermissionType("ALUS")]
	[PermissionName("ALTER ANY USER")]
	AlterAnyUser,
	[PermissionType("AUTH")]
	[PermissionName("AUTHENTICATE")]
	Authenticate,
	[PermissionType("BADB")]
	[PermissionName("BACKUP DATABASE")]
	BackupDatabase,
	[PermissionType("BALO")]
	[PermissionName("BACKUP LOG")]
	BackupLog,
	[PermissionType("CL")]
	[PermissionName("CONTROL")]
	Control,
	[PermissionType("CO")]
	[PermissionName("CONNECT")]
	Connect,
	[PermissionType("CORP")]
	[PermissionName("CONNECT REPLICATION")]
	ConnectReplication,
	[PermissionType("CP")]
	[PermissionName("CHECKPOINT")]
	Checkpoint,
	[PermissionType("CRAG")]
	[PermissionName("CREATE AGGREGATE")]
	CreateAggregate,
	[PermissionType("CRAK")]
	[PermissionName("CREATE ASYMMETRIC KEY")]
	CreateAsymmetricKey,
	[PermissionType("CRAS")]
	[PermissionName("CREATE ASSEMBLY")]
	CreateAssembly,
	[PermissionType("CRCF")]
	[PermissionName("CREATE CERTIFICATE")]
	CreateCertificate,
	[PermissionType("CRDB")]
	[PermissionName("CREATE DATABASE")]
	CreateDatabase,
	[PermissionType("CRDF")]
	[PermissionName("CREATE DEFAULT")]
	CreateDefault,
	[PermissionType("CRED")]
	[PermissionName("CREATE DATABASE DDL EVENT NOTIFICATION")]
	CreateDatabaseDdlEventNotification,
	[PermissionType("CRFN")]
	[PermissionName("CREATE FUNCTION")]
	CreateFunction,
	[PermissionType("CRFT")]
	[PermissionName("CREATE FULLTEXT CATALOG")]
	CreateFulltextCatalog,
	[PermissionType("CRMT")]
	[PermissionName("CREATE MESSAGE TYPE")]
	CreateMessageType,
	[PermissionType("CRPR")]
	[PermissionName("CREATE PROCEDURE")]
	CreateProcedure,
	[PermissionType("CRQU")]
	[PermissionName("CREATE QUEUE")]
	CreateQueue,
	[PermissionType("CRRL")]
	[PermissionName("CREATE ROLE")]
	CreateRole,
	[PermissionType("CRRT")]
	[PermissionName("CREATE ROUTE")]
	CreateRoute,
	[PermissionType("CRRU")]
	[PermissionName("CREATE RULE")]
	CreateRule,
	[PermissionType("CRSB")]
	[PermissionName("CREATE REMOTE SERVICE BINDING")]
	CreateRemoteServiceBinding,
	[PermissionType("CRSC")]
	[PermissionName("CREATE CONTRACT")]
	CreateContract,
	[PermissionType("CRSK")]
	[PermissionName("CREATE SYMMETRIC KEY")]
	CreateSymmetricKey,
	[PermissionType("CRSM")]
	[PermissionName("CREATE SCHEMA")]
	CreateSchema,
	[PermissionType("CRSN")]
	[PermissionName("CREATE SYNONYM")]
	CreateSynonym,
	[PermissionType("CRSV")]
	[PermissionName("CREATE SERVICE")]
	CreateService,
	[PermissionType("CRTB")]
	[PermissionName("CREATE TABLE")]
	CreateTable,
	[PermissionType("CRTY")]
	[PermissionName("CREATE TYPE")]
	CreateType,
	[PermissionType("CRVW")]
	[PermissionName("CREATE VIEW")]
	CreateView,
	[PermissionType("CRXS")]
	[PermissionName("CREATE XML SCHEMA COLLECTION")]
	CreateXmlSchemaCollection,
	[PermissionType("DL")]
	[PermissionName("DELETE")]
	Delete,
	[PermissionType("EX")]
	[PermissionName("EXECUTE")]
	Execute,
	[PermissionType("IN")]
	[PermissionName("INSERT")]
	Insert,
	[PermissionType("RF")]
	[PermissionName("REFERENCES")]
	References,
	[PermissionType("SL")]
	[PermissionName("SELECT")]
	Select,
	[PermissionType("SPLN")]
	[PermissionName("SHOWPLAN")]
	Showplan,
	[PermissionType("SUQN")]
	[PermissionName("SUBSCRIBE QUERY NOTIFICATIONS")]
	SubscribeQueryNotifications,
	[PermissionType("TO")]
	[PermissionName("TAKE OWNERSHIP")]
	TakeOwnership,
	[PermissionType("UP")]
	[PermissionName("UPDATE")]
	Update,
	[PermissionType("VW")]
	[PermissionName("VIEW DEFINITION")]
	ViewDefinition,
	[PermissionType("VWDS")]
	[PermissionName("VIEW DATABASE STATE")]
	ViewDatabaseState,
	[PermissionType("ALDA")]
	[PermissionName("ALTER ANY DATABASE AUDIT")]
	AlterAnyDatabaseAudit,
	[PermissionType("ALSP")]
	[PermissionName("ALTER ANY SECURITY POLICY")]
	AlterAnySecurityPolicy,
	[PermissionType("AEDS")]
	[PermissionName("ALTER ANY EXTERNAL DATA SOURCE")]
	AlterAnyExternalDataSource,
	[PermissionType("AEFF")]
	[PermissionName("ALTER ANY EXTERNAL FILE FORMAT")]
	AlterAnyExternalFileFormat,
	[PermissionType("AAMK")]
	[PermissionName("ALTER ANY MASK")]
	AlterAnyMask,
	[PermissionType("UMSK")]
	[PermissionName("UNMASK")]
	Unmask,
	[PermissionType("VWCK")]
	[PermissionName("VIEW ANY COLUMN ENCRYPTION KEY DEFINITION")]
	ViewAnyColumnEncryptionKeyDefinition,
	[PermissionType("VWCM")]
	[PermissionName("VIEW ANY COLUMN MASTER KEY DEFINITION")]
	ViewAnyColumnMasterKeyDefinition,
	[PermissionType("AADS")]
	[PermissionName("ALTER ANY DATABASE EVENT SESSION")]
	AlterAnyDatabaseEventSession,
	[PermissionType("ALCK")]
	[PermissionName("ALTER ANY COLUMN ENCRYPTION KEY")]
	AlterAnyColumnEncryptionKey,
	[PermissionType("ALCM")]
	[PermissionName("ALTER ANY COLUMN MASTER KEY")]
	AlterAnyColumnMasterKey,
	[PermissionType("ALDC")]
	[PermissionName("ALTER ANY DATABASE SCOPED CONFIGURATION")]
	AlterAnyDatabaseScopedConfiguration,
	[PermissionType("ALEL")]
	[PermissionName("ALTER ANY EXTERNAL LIBRARY")]
	AlterAnyExternalLibrary,
	[PermissionType("DABO")]
	[PermissionName("ADMINISTER DATABASE BULK OPERATIONS")]
	AdministerDatabaseBulkOperations,
	[PermissionType("EAES")]
	[PermissionName("EXECUTE ANY EXTERNAL SCRIPT")]
	ExecuteAnyExternalScript,
	[PermissionType("KIDC")]
	[PermissionName("KILL DATABASE CONNECTION")]
	KillDatabaseConnection,
	[PermissionType("CREL")]
	[PermissionName("CREATE EXTERNAL LIBRARY")]
	CreateExternalLibrary,
	[PermissionType("AASC")]
	[PermissionName("ALTER ANY SENSITIVITY CLASSIFICATION")]
	AlterAnySensitivityClassification,
	[PermissionType("VASC")]
	[PermissionName("VIEW ANY SENSITIVITY CLASSIFICATION")]
	ViewAnySensitivityClassification,
	[PermissionType("ALLA")]
	[PermissionName("ALTER ANY EXTERNAL LANGUAGE")]
	AlterAnyExternalLanguage,
	[PermissionType("CRLA")]
	[PermissionName("CREATE EXTERNAL LANGUAGE")]
	CreateExternalLanguage
}
