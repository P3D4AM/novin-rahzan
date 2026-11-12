using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Smo;

[CompilerGenerated]
internal class WmiMgmtImpl
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(WmiMgmtImpl));

		private static CultureInfo _culture = null;

		public const string LoginPropertyNotSet = "LoginPropertyNotSet";

		public const string LoginEnable = "LoginEnable";

		public const string LoginDisable = "LoginDisable";

		public const string CannotSetPrivilege = "CannotSetPrivilege";

		public const string CollectionNotAvailable = "CollectionNotAvailable";

		public const string TriggerNotSupported = "TriggerNotSupported";

		public const string CannotEnableViewTrigger = "CannotEnableViewTrigger";

		public const string UnknownRecoveryModel = "UnknownRecoveryModel";

		public const string UnknownUserAccess = "UnknownUserAccess";

		public const string CannotRenameObject = "CannotRenameObject";

		public const string PropertyNotSet = "PropertyNotSet";

		public const string ColumnAlreadyHasDefault = "ColumnAlreadyHasDefault";

		public const string ColumnHasNoDefault = "ColumnHasNoDefault";

		public const string MismatchingServerName = "MismatchingServerName";

		public const string MissingBackupDevices = "MissingBackupDevices";

		public const string MissingBackupDeviceType = "MissingBackupDeviceType";

		public const string MismatchingNumberOfMirrors = "MismatchingNumberOfMirrors";

		public const string BackupToPipesNotSupported = "BackupToPipesNotSupported";

		public const string LoginHasUser = "LoginHasUser";

		public const string LoginHasAlias = "LoginHasAlias";

		public const string InvalidLogin = "InvalidLogin";

		public const string UnknownShrinkType = "UnknownShrinkType";

		public const string InvalidShrinkMethod = "InvalidShrinkMethod";

		public const string CannotChangePrimary = "CannotChangePrimary";

		public const string OnlyOnePrimaryFile = "OnlyOnePrimaryFile";

		public const string MustSpecifyGrowth = "MustSpecifyGrowth";

		public const string WrongPercentageGrowth = "WrongPercentageGrowth";

		public const string WrongSize = "WrongSize";

		public const string ObjectRefsNonexCol = "ObjectRefsNonexCol";

		public const string CannotAddObject = "CannotAddObject";

		public const string UnknownObjectType = "UnknownObjectType";

		public const string TooFewFiles = "TooFewFiles";

		public const string UnknownProperty = "UnknownProperty";

		public const string ObjectNotUnderServer = "ObjectNotUnderServer";

		public const string UnknownChild = "UnknownChild";

		public const string UnknownChildCollection = "UnknownChildCollection";

		public const string CantCreateType = "CantCreateType";

		public const string ObjectAlreadyExists = "ObjectAlreadyExists";

		public const string NoSqlGen = "NoSqlGen";

		public const string InvalidType = "InvalidType";

		public const string WrongUrn = "WrongUrn";

		public const string NoDepForSysObjects = "NoDepForSysObjects";

		public const string UrnMissing = "UrnMissing";

		public const string InvalidGranteeList = "InvalidGranteeList";

		public const string DatabaseAlreadyExists = "DatabaseAlreadyExists";

		public const string ObjectWithNoChildren = "ObjectWithNoChildren";

		public const string ColumnBeforeNotExisting = "ColumnBeforeNotExisting";

		public const string PropNotModifiable = "PropNotModifiable";

		public const string UnsupportedLoginMode = "UnsupportedLoginMode";

		public const string CannotSetDefInitFlds = "CannotSetDefInitFlds";

		public const string ReasonTextIsEncrypted = "ReasonTextIsEncrypted";

		public const string ReasonPropertyIsNotSupportedOnCurrentServerVersion = "ReasonPropertyIsNotSupportedOnCurrentServerVersion";

		public const string InvalidPathChildCollectionNotFound = "InvalidPathChildCollectionNotFound";

		public const string SqlInnerException = "SqlInnerException";

		public const string InnerException = "InnerException";

		public const string InnerWmiException = "InnerWmiException";

		public const string UnknownError = "UnknownError";

		public const string WMIException = "WMIException";

		public const string CallingInitChildLevelWithWrongUrn = "CallingInitChildLevelWithWrongUrn";

		public const string UnsupportedBackupDeviceType = "UnsupportedBackupDeviceType";

		public const string UnsupportedVersion = "UnsupportedVersion";

		public const string ObjectDoesNotExist = "ObjectDoesNotExist";

		public const string UnsupportedFeature = "UnsupportedFeature";

		public const string OneFilePageSupported = "OneFilePageSupported";

		public const string NoObjectWithoutColumns = "NoObjectWithoutColumns";

		public const string ConflictingScriptingOptions = "ConflictingScriptingOptions";

		public const string SupportedOnlyBelow90 = "SupportedOnlyBelow90";

		public const string SupportedOnlyOn90 = "SupportedOnlyOn90";

		public const string SupportedOnlyOn80 = "SupportedOnlyOn80";

		public const string EmptyInputParam = "EmptyInputParam";

		public const string MustSpecifyOneParameter = "MustSpecifyOneParameter";

		public const string MutuallyExclusiveProperties = "MutuallyExclusiveProperties";

		public const string NeedPSParams = "NeedPSParams";

		public const string IndexMustBeClustered = "IndexMustBeClustered";

		public const string ParentMustExist = "ParentMustExist";

		public const string ViewCannotHaveKeys = "ViewCannotHaveKeys";

		public const string InvalidAcctName = "InvalidAcctName";

		public const string OperationOnlyInPendingState = "OperationOnlyInPendingState";

		public const string OperationNotInPendingState = "OperationNotInPendingState";

		public const string OperationNotInPendingState1 = "OperationNotInPendingState1";

		public const string OperationNotInPendingState2 = "OperationNotInPendingState2";

		public const string OperationNotInPendingState3 = "OperationNotInPendingState3";

		public const string NotXmlIndexOnView = "NotXmlIndexOnView";

		public const string OneColumnInXmlIndex = "OneColumnInXmlIndex";

		public const string NoXmlIndexUnique = "NoXmlIndexUnique";

		public const string NoXmlIndexClustered = "NoXmlIndexClustered";

		public const string Version80SP3Only = "Version80SP3Only";

		public const string CantSetDefaultFalse = "CantSetDefaultFalse";

		public const string TimeoutMustBePositive = "TimeoutMustBePositive";

		public const string NoPropertyChangeForDotNet = "NoPropertyChangeForDotNet";

		public const string ClrNotSupported = "ClrNotSupported";

		public const string InvalidPropertyNumberRange = "InvalidPropertyNumberRange";

		public const string WrongPropertyValueException = "WrongPropertyValueException";

		public const string InvalidCollation = "InvalidCollation";

		public const string ViewColumnsCannotBeModified = "ViewColumnsCannotBeModified";

		public const string NeedToPassObject = "NeedToPassObject";

		public const string InexistentDir = "InexistentDir";

		public const string CantCreateTempFile = "CantCreateTempFile";

		public const string UnknownFilter = "UnknownFilter";

		public const string SystemMessageReadOnly = "SystemMessageReadOnly";

		public const string MessageIDTooSmall = "MessageIDTooSmall";

		public const string DataTypeUnsupported = "DataTypeUnsupported";

		public const string CantSetTypeName = "CantSetTypeName";

		public const string CantSetTypeSchema = "CantSetTypeSchema";

		public const string UnknownSqlDataType = "UnknownSqlDataType";

		public const string NeedExistingObjForDataType = "NeedExistingObjForDataType";

		public const string NoPendingObjForDataType = "NoPendingObjForDataType";

		public const string ViewFragInfoNotInV7 = "ViewFragInfoNotInV7";

		public const string InvalidOptionForVersion = "InvalidOptionForVersion";

		public const string InvalidParamForVersion = "InvalidParamForVersion";

		public const string InvalidPropertyValueForVersion = "InvalidPropertyValueForVersion";

		public const string UnknownLanguageId = "UnknownLanguageId";

		public const string UnknownEnumeration = "UnknownEnumeration";

		public const string MissingConfigVariable = "MissingConfigVariable";

		public const string DtsNotInstalled = "DtsNotInstalled";

		public const string NSNotInstalled = "NSNotInstalled";

		public const string InvalidNSOperation = "InvalidNSOperation";

		public const string NSNotConnectedToServer = "NSNotConnectedToServer";

		public const string InvalidThreadPoolSize = "InvalidThreadPoolSize";

		public const string InvalidQuantumLimit = "InvalidQuantumLimit";

		public const string InvalidQuantumDuration = "InvalidQuantumDuration";

		public const string InvalidThrottle = "InvalidThrottle";

		public const string InvalidFailuresBeforeLoggingEvent = "InvalidFailuresBeforeLoggingEvent";

		public const string InvalidFailuresBeforeAbort = "InvalidFailuresBeforeAbort";

		public const string InvalidMulticastRecipientLimit = "InvalidMulticastRecipientLimit";

		public const string InvalidNotificationClassBatchSize = "InvalidNotificationClassBatchSize";

		public const string ThreadPoolSizeNotValidForEdition = "ThreadPoolSizeNotValidForEdition";

		public const string StartTimeGreaterThanADay = "StartTimeGreaterThanADay";

		public const string InvalidPropertySetForExistingObject = "InvalidPropertySetForExistingObject";

		public const string InvalidEncryptArgumentsAndArgumentKey = "InvalidEncryptArgumentsAndArgumentKey";

		public const string RequiredChildMissingFromParent = "RequiredChildMissingFromParent";

		public const string NoPermissions = "NoPermissions";

		public const string WrongParent = "WrongParent";

		public const string VerifyFailed = "VerifyFailed";

		public const string VerifyFailed0 = "VerifyFailed0";

		public const string PrimaryFgMustHaveFiles = "PrimaryFgMustHaveFiles";

		public const string NeedToSetParent = "NeedToSetParent";

		public const string InvalidVersion = "InvalidVersion";

		public const string ErrorInCreatingState = "ErrorInCreatingState";

		public const string ConflictingSwitches = "ConflictingSwitches";

		public const string BackupFailed = "BackupFailed";

		public const string RestoreFailed = "RestoreFailed";

		public const string BadCompatLevel = "BadCompatLevel";

		public const string SetPasswordError = "SetPasswordError";

		public const string PassPhraseAndIdentityNotSpecified = "PassPhraseAndIdentityNotSpecified";

		public const string PassPhraseNotSpecified = "PassPhraseNotSpecified";

		public const string NotEnoughRights = "NotEnoughRights";

		public const string IncludeHeader = "IncludeHeader";

		public const string NoUrnSuffix = "NoUrnSuffix";

		public const string FullPropertyBag = "FullPropertyBag";

		public const string MultipleRowsForUrn = "MultipleRowsForUrn";

		public const string GetParentFailed = "GetParentFailed";

		public const string ParentNull = "ParentNull";

		public const string CouldNotFindKey = "CouldNotFindKey";

		public const string UnsupportedUrnFilter = "UnsupportedUrnFilter";

		public const string UnsupportedUrnAttrib = "UnsupportedUrnAttrib";

		public const string MappingObjectIdMissing = "MappingObjectIdMissing";

		public const string EmptyMapping = "EmptyMapping";

		public const string ExecutionContextPrincipalIsNotSpecified = "ExecutionContextPrincipalIsNotSpecified";

		public const string ServiceError0 = "ServiceError0";

		public const string ServiceError1 = "ServiceError1";

		public const string ServiceError2 = "ServiceError2";

		public const string ServiceError3 = "ServiceError3";

		public const string ServiceError4 = "ServiceError4";

		public const string ServiceError5 = "ServiceError5";

		public const string ServiceError6 = "ServiceError6";

		public const string ServiceError7 = "ServiceError7";

		public const string ServiceError8 = "ServiceError8";

		public const string ServiceError9 = "ServiceError9";

		public const string ServiceError10 = "ServiceError10";

		public const string ServiceError11 = "ServiceError11";

		public const string ServiceError12 = "ServiceError12";

		public const string ServiceError13 = "ServiceError13";

		public const string ServiceError14 = "ServiceError14";

		public const string ServiceError15 = "ServiceError15";

		public const string ServiceError16 = "ServiceError16";

		public const string ServiceError17 = "ServiceError17";

		public const string ServiceError18 = "ServiceError18";

		public const string ServiceError19 = "ServiceError19";

		public const string ServiceError20 = "ServiceError20";

		public const string ServiceError21 = "ServiceError21";

		public const string ServiceError22 = "ServiceError22";

		public const string ServiceError23 = "ServiceError23";

		public const string ServiceError24 = "ServiceError24";

		public const string UnknownCategoryName = "UnknownCategoryName";

		public const string UnknownCategoryType = "UnknownCategoryType";

		public const string UnknownOperator = "UnknownOperator";

		public const string UnsupportedFeatureSqlAgent = "UnsupportedFeatureSqlAgent";

		public const string UnsupportedFeatureSqlMail = "UnsupportedFeatureSqlMail";

		public const string UnsupportedFeatureServiceBroker = "UnsupportedFeatureServiceBroker";

		public const string UnsupportedFeatureFullText = "UnsupportedFeatureFullText";

		public const string InvalidServerUrn = "InvalidServerUrn";

		public const string InvalidUrn = "InvalidUrn";

		public const string WMIProviderNotInstalled = "WMIProviderNotInstalled";

		public const string PropertyCannotBeChangedAfterConnection = "PropertyCannotBeChangedAfterConnection";

		public const string CouldNotFindManagementObject = "CouldNotFindManagementObject";

		public const string CannotSubscribe = "CannotSubscribe";

		public const string CannotStartSubscription = "CannotStartSubscription";

		public const string NotSupportedNotification = "NotSupportedNotification";

		public const string InnerRegSvrException = "InnerRegSvrException";

		public const string SqlServerTypeName = "SqlServerTypeName";

		public const string MissingObjectExceptionText = "MissingObjectExceptionText";

		public const string PropertyNotSetExceptionText = "PropertyNotSetExceptionText";

		public const string WrongPropertyValueExceptionText = "WrongPropertyValueExceptionText";

		public const string PropertyTypeMismatchExceptionText = "PropertyTypeMismatchExceptionText";

		public const string MissingPropertyExceptionText = "MissingPropertyExceptionText";

		public const string UnknownPropertyExceptionText = "UnknownPropertyExceptionText";

		public const string PropertyReadOnlyExceptionText = "PropertyReadOnlyExceptionText";

		public const string InvalidSmoOperationExceptionText = "InvalidSmoOperationExceptionText";

		public const string PropertyCannotBeRetrievedExceptionText = "PropertyCannotBeRetrievedExceptionText";

		public const string ObjectDroppedExceptionText = "ObjectDroppedExceptionText";

		public const string UnsupportedObjectNameExceptionText = "UnsupportedObjectNameExceptionText";

		public const string FailedtoInitialize = "FailedtoInitialize";

		public const string PropertyMustBeSpecifiedInUrn = "PropertyMustBeSpecifiedInUrn";

		public const string InvalidScanType = "InvalidScanType";

		public const string ColumnsMustBeSpecified = "ColumnsMustBeSpecified";

		public const string PasswdModiOnlyForStandardLogin = "PasswdModiOnlyForStandardLogin";

		public const string DenyLoginModiNotForStandardLogin = "DenyLoginModiNotForStandardLogin";

		public const string CannotCreateExtendedPropertyWithoutSchema = "CannotCreateExtendedPropertyWithoutSchema";

		public const string InvalidSchema = "InvalidSchema";

		public const string FailedToChangeSchema = "FailedToChangeSchema";

		public const string InvalidUrnServerLevel = "InvalidUrnServerLevel";

		public const string ServerLevelMustBePresent = "ServerLevelMustBePresent";

		public const string TempTablesNotSupported = "TempTablesNotSupported";

		public const string TypeSchemaMustBeDbo = "TypeSchemaMustBeDbo";

		public const string OperationInProgress = "OperationInProgress";

		public const string UnssuportedPermission = "UnssuportedPermission";

		public const string InvalidVersionSmoOperation = "InvalidVersionSmoOperation";

		public const string CannotChangePassword = "CannotChangePassword";

		public const string OperationNotSupportedWhenPartOfAUDF = "OperationNotSupportedWhenPartOfAUDF";

		public const string FailedToWriteProperty = "FailedToWriteProperty";

		public const string ReasonIntextMode = "ReasonIntextMode";

		public const string ReasonNotIntextMode = "ReasonNotIntextMode";

		public const string SyntaxErrorInTextHeader = "SyntaxErrorInTextHeader";

		public const string IncorrectTextHeader = "IncorrectTextHeader";

		public const string CollectionCannotBeModified = "CollectionCannotBeModified";

		public const string KeyOptionsIncorrect = "KeyOptionsIncorrect";

		public const string PropertyIsInvalidInUrn = "PropertyIsInvalidInUrn";

		public const string TableOrViewParentForUpdateStatistics = "TableOrViewParentForUpdateStatistics";

		public const string ReasonObjectAlreadyCreated = "ReasonObjectAlreadyCreated";

		public const string ServerSphinx = "ServerSphinx";

		public const string ServerShiloh = "ServerShiloh";

		public const string ServerYukon = "ServerYukon";

		public const string PropertyAvailable = "PropertyAvailable";

		public const string ServerPropertyMustBeSetForOlap = "ServerPropertyMustBeSetForOlap";

		public const string InvalidInstanceName = "InvalidInstanceName";

		public const string TransferCtorErr = "TransferCtorErr";

		public const string UnsupportedVersionException = "UnsupportedVersionException";

		public const string CantScriptObject = "CantScriptObject";

		public const string FailedOperationExceptionText = "FailedOperationExceptionText";

		public const string FailedOperationExceptionText2 = "FailedOperationExceptionText2";

		public const string FailedOperationExceptionText3 = "FailedOperationExceptionText3";

		public const string FailedOperationExceptionTextColl = "FailedOperationExceptionTextColl";

		public const string FailedOperationExceptionTextScript = "FailedOperationExceptionTextScript";

		public const string FailedOperationMessageNotSupportedTempdb = "FailedOperationMessageNotSupportedTempdb";

		public const string SetAccount = "SetAccount";

		public const string Create = "Create";

		public const string Alter = "Alter";

		public const string Drop = "Drop";

		public const string Rename = "Rename";

		public const string Script = "Script";

		public const string Grant = "Grant";

		public const string Revoke = "Revoke";

		public const string Deny = "Deny";

		public const string GrantWithGrant = "GrantWithGrant";

		public const string Bind = "Bind";

		public const string Unbind = "Unbind";

		public const string AddDefaultConstraint = "AddDefaultConstraint";

		public const string TestMailProfile = "TestMailProfile";

		public const string TestNetSend = "TestNetSend";

		public const string SetState = "SetState";

		public const string DropAndMove = "DropAndMove";

		public const string GetSmoObject = "GetSmoObject";

		public const string AdvancedProperties = "AdvancedProperties";

		public const string AddCollection = "AddCollection";

		public const string RemoveCollection = "RemoveCollection";

		public const string This = "This";

		public const string Abort = "Abort";

		public const string AddMember = "AddMember";

		public const string DropMember = "DropMember";

		public const string EnumMembers = "EnumMembers";

		public const string EnumPermissions = "EnumPermissions";

		public const string AddMemberServer = "AddMemberServer";

		public const string EnumMemberServers = "EnumMemberServers";

		public const string RemoveMemberServer = "RemoveMemberServer";

		public const string ResetOccurrenceCount = "ResetOccurrenceCount";

		public const string AddNotification = "AddNotification";

		public const string RemoveNotification = "RemoveNotification";

		public const string UpdateNotification = "UpdateNotification";

		public const string EnumNotifications = "EnumNotifications";

		public const string ApplyToTargetServer = "ApplyToTargetServer";

		public const string ApplyToTargetServerGroup = "ApplyToTargetServerGroup";

		public const string EnumAlerts = "EnumAlerts";

		public const string EnumHistory = "EnumHistory";

		public const string EnumJobStepOutputLogs = "EnumJobStepOutputLogs";

		public const string EnumTargetServers = "EnumTargetServers";

		public const string EnumJobSteps = "EnumJobSteps";

		public const string Invoke = "Invoke";

		public const string PurgeHistory = "PurgeHistory";

		public const string AddSharedSchedule = "AddSharedSchedule";

		public const string RemoveSharedSchedule = "RemoveSharedSchedule";

		public const string RemoveAllJobSchedules = "RemoveAllJobSchedules";

		public const string RemoveAllJobSteps = "RemoveAllJobSteps";

		public const string RemoveFromTargetServer = "RemoveFromTargetServer";

		public const string RemoveFromTargetServerGroup = "RemoveFromTargetServerGroup";

		public const string Start = "Start";

		public const string Stop = "Stop";

		public const string SetPassword = "SetPassword";

		public const string ChangePassword = "ChangePassword";

		public const string EnumDatabaseMappings = "EnumDatabaseMappings";

		public const string GetDatabaseUser = "GetDatabaseUser";

		public const string AddToRole = "AddToRole";

		public const string AttachDatabase = "AttachDatabase";

		public const string DetachDatabase = "DetachDatabase";

		public const string EnumCollations = "EnumCollations";

		public const string EnumPerformanceCounters = "EnumPerformanceCounters";

		public const string EnumErrorLogs = "EnumErrorLogs";

		public const string EnumDatabaseMirrorWitnessRoles = "EnumDatabaseMirrorWitnessRoles";

		public const string ReadErrorLog = "ReadErrorLog";

		public const string KillDatabase = "KillDatabase";

		public const string KillProcess = "KillProcess";

		public const string GetActiveDBConnectionCount = "GetActiveDBConnectionCount";

		public const string DropAllActiveDBConnections = "DropAllActiveDBConnections";

		public const string EnumDirectories = "EnumDirectories";

		public const string EnumLocks = "EnumLocks";

		public const string AddPrivateKey = "AddPrivateKey";

		public const string ExportCertificate = "ExportCertificate";

		public const string ChangePrivateKeyPassword = "ChangePrivateKeyPassword";

		public const string RemovePrivateKey = "RemovePrivateKey";

		public const string AddKeyEncryption = "AddKeyEncryption";

		public const string DropKeyEncryption = "DropKeyEncryption";

		public const string SymmetricKeyOpen = "SymmetricKeyOpen";

		public const string SymmetricKeyClose = "SymmetricKeyClose";

		public const string EnumLogins = "EnumLogins";

		public const string EnumWindowsDomainGroups = "EnumWindowsDomainGroups";

		public const string EnumProcesses = "EnumProcesses";

		public const string EnumStartupProcedures = "EnumStartupProcedures";

		public const string EnumWindowsUserInfo = "EnumWindowsUserInfo";

		public const string EnumWindowsGroupInfo = "EnumWindowsGroupInfo";

		public const string EnumAvailableMedia = "EnumAvailableMedia";

		public const string EnumServerAttributes = "EnumServerAttributes";

		public const string DeleteBackupHistory = "DeleteBackupHistory";

		public const string Refresh = "Refresh";

		public const string EnumBoundColumns = "EnumBoundColumns";

		public const string EnumBoundDataTypes = "EnumBoundDataTypes";

		public const string CheckAllocations = "CheckAllocations";

		public const string CheckCatalog = "CheckCatalog";

		public const string CheckIdentityValues = "CheckIdentityValues";

		public const string CheckTables = "CheckTables";

		public const string CheckTable = "CheckTable";

		public const string Shrink = "Shrink";

		public const string RecalculateSpaceUsage = "RecalculateSpaceUsage";

		public const string PrefetchObjects = "PrefetchObjects";

		public const string EnumTransactions = "EnumTransactions";

		public const string GetTransactionCount = "GetTransactionCount";

		public const string ImportXmlSchema = "ImportXmlSchema";

		public const string ExtendXmlSchema = "ExtendXmlSchema";

		public const string RemoveFullTextCatalogs = "RemoveFullTextCatalogs";

		public const string SetDefaultFullTextCatalog = "SetDefaultFullTextCatalog";

		public const string SetDefaultFileGroup = "SetDefaultFileGroup";

		public const string CheckFileGroup = "CheckFileGroup";

		public const string CheckIndex = "CheckIndex";

		public const string Checkpoint = "Checkpoint";

		public const string Cleanup = "Cleanup";

		public const string UpdateLanguageResources = "UpdateLanguageResources";

		public const string EnumLanguages = "EnumLanguages";

		public const string ClearHostLoginAccount = "ClearHostLoginAccount";

		public const string SetProxyAccount = "SetProxyAccount";

		public const string ClearProxyAccount = "ClearProxyAccount";

		public const string SetMsxAccount = "SetMsxAccount";

		public const string ClearMsxAccount = "ClearMsxAccount";

		public const string CycleErrorLog = "CycleErrorLog";

		public const string EnumJobHistory = "EnumJobHistory";

		public const string EnumJobs = "EnumJobs";

		public const string MoreThanOneProxyAccountIsNotSupported = "MoreThanOneProxyAccountIsNotSupported";

		public const string AddSubSystems = "AddSubSystems";

		public const string DeleteJobStepLogs = "DeleteJobStepLogs";

		public const string RemoveSubSystems = "RemoveSubSystems";

		public const string EnumSubSystems = "EnumSubSystems";

		public const string AddMailAccountToProfile = "AddMailAccountToProfile";

		public const string RemoveMailAccountFromProfile = "RemoveMailAccountFromProfile";

		public const string EnumMailAccountsForProfile = "EnumMailAccountsForProfile";

		public const string AddPrincipalToMailProfile = "AddPrincipalToMailProfile";

		public const string RemovePrincipalFromMailProfile = "RemovePrincipalFromMailProfile";

		public const string EnumPrincipalsForMailProfile = "EnumPrincipalsForMailProfile";

		public const string AddLoginToProxyAccount = "AddLoginToProxyAccount";

		public const string RemoveLoginFromProxyAccount = "RemoveLoginFromProxyAccount";

		public const string EnumLoginsOfProxyAccount = "EnumLoginsOfProxyAccount";

		public const string AddServerRoleToProxyAccount = "AddServerRoleToProxyAccount";

		public const string RemoveServerRoleFromProxyAccount = "RemoveServerRoleFromProxyAccount";

		public const string EnumServerRolesOfProxyAccount = "EnumServerRolesOfProxyAccount";

		public const string AddMSDBRoleToProxyAccount = "AddMSDBRoleToProxyAccount";

		public const string RemoveMSDBRoleFromProxyAccount = "RemoveMSDBRoleFromProxyAccount";

		public const string EnumMSDBRolesOfProxyAccount = "EnumMSDBRolesOfProxyAccount";

		public const string MsxDefect = "MsxDefect";

		public const string MsxEnlist = "MsxEnlist";

		public const string PurgeJobHistory = "PurgeJobHistory";

		public const string ReassignJobsByLogin = "ReassignJobsByLogin";

		public const string DropJobsByLogin = "DropJobsByLogin";

		public const string StartMonitor = "StartMonitor";

		public const string StopMonitor = "StopMonitor";

		public const string EnumProxies = "EnumProxies";

		public const string DropJobsByServer = "DropJobsByServer";

		public const string CompareUrn = "CompareUrn";

		public const string Disable = "Disable";

		public const string DisableAllIndexes = "DisableAllIndexes";

		public const string EnableAllIndexes = "EnableAllIndexes";

		public const string DiscoverDependencies = "DiscoverDependencies";

		public const string DropBackupHistory = "DropBackupHistory";

		public const string ChangeMirroringState = "ChangeMirroringState";

		public const string IsMember = "IsMember";

		public const string Recreate = "Recreate";

		public const string Enable = "Enable";

		public const string EnumColumns = "EnumColumns";

		public const string EnumForeignKeys = "EnumForeignKeys";

		public const string EnumIndexes = "EnumIndexes";

		public const string EnumFragmentation = "EnumFragmentation";

		public const string EnumReferences = "EnumReferences";

		public const string SetOffline = "SetOffline";

		public const string SetOwner = "SetOwner";

		public const string StartPopulation = "StartPopulation";

		public const string StopPopulation = "StopPopulation";

		public const string Rebuild = "Rebuild";

		public const string Reorganize = "Reorganize";

		public const string UpdateStatistics = "UpdateStatistics";

		public const string SetHostLoginAccount = "SetHostLoginAccount";

		public const string SetMailServerAccount = "SetMailServerAccount";

		public const string SetMailServerPassword = "SetMailServerPassword";

		public const string EnumLastStatisticsUpdates = "EnumLastStatisticsUpdates";

		public const string RebuildIndexes = "RebuildIndexes";

		public const string ReCompileReferences = "ReCompileReferences";

		public const string TruncateData = "TruncateData";

		public const string TruncateLog = "TruncateLog";

		public const string SwitchPartition = "SwitchPartition";

		public const string MergeHashPartition = "MergeHashPartition";

		public const string MergeRangePartition = "MergeRangePartition";

		public const string SplitHashPartition = "SplitHashPartition";

		public const string SplitRangePartition = "SplitRangePartition";

		public const string GetRangeValues = "GetRangeValues";

		public const string ResetNextUsed = "ResetNextUsed";

		public const string GetFileGroups = "GetFileGroups";

		public const string GetDefaultInitFields = "GetDefaultInitFields";

		public const string SetDefaultInitFields = "SetDefaultInitFields";

		public const string GetPropertyNames = "GetPropertyNames";

		public const string SetParent = "SetParent";

		public const string SetName = "SetName";

		public const string SetNamespace = "SetNamespace";

		public const string SetSchema = "SetSchema";

		public const string ExecuteNonQuery = "ExecuteNonQuery";

		public const string SetSnapshotIsolation = "SetSnapshotIsolation";

		public const string EnumNamespaces = "EnumNamespaces";

		public const string EnumTypes = "EnumTypes";

		public const string AddSchemaDocument = "AddSchemaDocument";

		public const string ScriptTransfer = "ScriptTransfer";

		public const string SetIdentityPhrase = "SetIdentityPhrase";

		public const string SetEncryptionOptions = "SetEncryptionOptions";

		public const string EnumStatistics = "EnumStatistics";

		public const string GetJobByID = "GetJobByID";

		public const string RemoveJobByID = "RemoveJobByID";

		public const string RemoveJobsByLogin = "RemoveJobsByLogin";

		public const string EnumCandidateKeys = "EnumCandidateKeys";

		public const string ExecuteWithResults = "ExecuteWithResults";

		public const string UpdateIndexStatistics = "UpdateIndexStatistics";

		public const string EnumMatchingSPs = "EnumMatchingSPs";

		public const string EnumObjects = "EnumObjects";

		public const string ReadBackupHeader = "ReadBackupHeader";

		public const string ReadMediaHeader = "ReadMediaHeader";

		public const string DetachedDatabaseInfo = "DetachedDatabaseInfo";

		public const string IsDetachedPrimaryFile = "IsDetachedPrimaryFile";

		public const string IsWindowsGroupMember = "IsWindowsGroupMember";

		public const string EnumDetachedDatabaseFiles = "EnumDetachedDatabaseFiles";

		public const string EnumDetachedLogFiles = "EnumDetachedLogFiles";

		public const string ServerEnumMembers = "ServerEnumMembers";

		public const string Contains = "Contains";

		public const string PingSqlServerVersion = "PingSqlServerVersion";

		public const string SetServiceAccount = "SetServiceAccount";

		public const string ChangeServicePassword = "ChangeServicePassword";

		public const string ServerActiveDirectoryRegister = "ServerActiveDirectoryRegister";

		public const string ServerActiveDirectoryUpdateRegistration = "ServerActiveDirectoryUpdateRegistration";

		public const string ServerActiveDirectoryUnregister = "ServerActiveDirectoryUnregister";

		public const string DatabaseActiveDirectoryRegister = "DatabaseActiveDirectoryRegister";

		public const string DatabaseActiveDirectoryUpdateRegistration = "DatabaseActiveDirectoryUpdateRegistration";

		public const string DatabaseActiveDirectoryUnregister = "DatabaseActiveDirectoryUnregister";

		public const string RecoverMasterKey = "RecoverMasterKey";

		public const string RegenerateMasterKey = "RegenerateMasterKey";

		public const string ImportMasterKey = "ImportMasterKey";

		public const string ExportMasterKey = "ExportMasterKey";

		public const string ChangeAcctMasterKey = "ChangeAcctMasterKey";

		public const string AddEncryptionMasterKey = "AddEncryptionMasterKey";

		public const string DropEncryptionMasterKey = "DropEncryptionMasterKey";

		public const string Close = "Close";

		public const string Open = "Open";

		public const string EnumKeyEncryptions = "EnumKeyEncryptions";

		public const string Compare = "Compare";

		public const string Insert = "Insert";

		public const string AddRange = "AddRange";

		public const string SetRange = "SetRange";

		public const string AddDevice = "AddDevice";

		public const string SetMirrors = "SetMirrors";

		public const string SetDatabase = "SetDatabase";

		public const string SqlManagement = "SqlManagement";

		public const string Table = "Table";

		public const string View = "View";

		public const string Server = "Server";

		public const string Database = "Database";

		public const string ExtendedProperty = "ExtendedProperty";

		public const string DatabaseOptions = "DatabaseOptions";

		public const string Synonym = "Synonym";

		public const string FullTextIndex = "FullTextIndex";

		public const string FullTextIndexColumn = "FullTextIndexColumn";

		public const string Check = "Check";

		public const string ForeignKey = "ForeignKey";

		public const string ForeignKeyColumn = "ForeignKeyColumn";

		public const string PartitionSchemeParameter = "PartitionSchemeParameter";

		public const string Trigger = "Trigger";

		public const string Index = "Index";

		public const string IndexedColumn = "IndexedColumn";

		public const string Statistic = "Statistic";

		public const string StatisticColumn = "StatisticColumn";

		public const string Column = "Column";

		public const string DefaultConstraint = "DefaultConstraint";

		public const string StoredProcedure = "StoredProcedure";

		public const string StoredProcedureParameter = "StoredProcedureParameter";

		public const string SqlAssembly = "SqlAssembly";

		public const string SqlAssemblyFile = "SqlAssemblyFile";

		public const string UserDefinedType = "UserDefinedType";

		public const string UserDefinedAggregate = "UserDefinedAggregate";

		public const string UserDefinedAggregateParameter = "UserDefinedAggregateParameter";

		public const string FullTextCatalog = "FullTextCatalog";

		public const string ExtendedStoredProcedure = "ExtendedStoredProcedure";

		public const string UserDefinedFunction = "UserDefinedFunction";

		public const string UserDefinedFunctionParameter = "UserDefinedFunctionParameter";

		public const string User = "User";

		public const string Schema = "Schema";

		public const string DatabaseRole = "DatabaseRole";

		public const string ApplicationRole = "ApplicationRole";

		public const string LogFile = "LogFile";

		public const string FileGroup = "FileGroup";

		public const string DataFile = "DataFile";

		public const string Default = "Default";

		public const string Rule = "Rule";

		public const string UserDefinedDataType = "UserDefinedDataType";

		public const string PartitionFunction = "PartitionFunction";

		public const string PartitionScheme = "PartitionScheme";

		public const string DatabaseActiveDirectory = "DatabaseActiveDirectory";

		public const string Language = "Language";

		public const string Login = "Login";

		public const string ServerRole = "ServerRole";

		public const string LinkedServer = "LinkedServer";

		public const string LinkedServerLogin = "LinkedServerLogin";

		public const string SystemDataType = "SystemDataType";

		public const string JobServer = "JobServer";

		public const string Category = "Category";

		public const string AlertSystem = "AlertSystem";

		public const string Alert = "Alert";

		public const string Operator = "Operator";

		public const string TargetServer = "TargetServer";

		public const string TargetServerGroup = "TargetServerGroup";

		public const string Job = "Job";

		public const string JobStep = "JobStep";

		public const string JobSchedule = "JobSchedule";

		public const string Settings = "Settings";

		public const string Information = "Information";

		public const string UserOptions = "UserOptions";

		public const string BackupDevice = "BackupDevice";

		public const string FullTextService = "FullTextService";

		public const string ServerActiveDirectory = "ServerActiveDirectory";

		public const string HttpEndpoint = "HttpEndpoint";

		public const string SoapConfiguration = "SoapConfiguration";

		public const string SoapMethod = "SoapMethod";

		public const string ServerAlias = "ServerAlias";

		public const string ManagedComputer = "ManagedComputer";

		public const string Service = "Service";

		public const string XmlSchemaCollection = "XmlSchemaCollection";

		public const string Restore = "Restore";

		public const string RestoreAsync = "RestoreAsync";

		public const string EnumAvailableSqlServers = "EnumAvailableSqlServers";

		public const string GetDataType = "GetDataType";

		public const string SetDataType = "SetDataType";

		public const string Backup = "Backup";

		public static CultureInfo Culture
		{
			get
			{
				return _culture;
			}
			set
			{
				_culture = value;
			}
		}

		private Keys()
		{
		}

		public static string GetString(string key)
		{
			return resourceManager.GetString(key, _culture);
		}

		public static string GetString(string key, object arg0)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0);
		}

		public static string GetString(string key, object arg0, object arg1)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1);
		}

		public static string GetString(string key, object arg0, object arg1, object arg2)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1, arg2);
		}

		public static string GetString(string key, object arg0, object arg1, object arg2, object arg3)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1, arg2, arg3);
		}
	}

	public static CultureInfo Culture
	{
		get
		{
			return Keys.Culture;
		}
		set
		{
			Keys.Culture = value;
		}
	}

	public static string LoginPropertyNotSet => Keys.GetString("LoginPropertyNotSet");

	public static string LoginEnable => Keys.GetString("LoginEnable");

	public static string LoginDisable => Keys.GetString("LoginDisable");

	public static string CannotEnableViewTrigger => Keys.GetString("CannotEnableViewTrigger");

	public static string MissingBackupDevices => Keys.GetString("MissingBackupDevices");

	public static string MissingBackupDeviceType => Keys.GetString("MissingBackupDeviceType");

	public static string UnknownShrinkType => Keys.GetString("UnknownShrinkType");

	public static string CannotChangePrimary => Keys.GetString("CannotChangePrimary");

	public static string OnlyOnePrimaryFile => Keys.GetString("OnlyOnePrimaryFile");

	public static string MustSpecifyGrowth => Keys.GetString("MustSpecifyGrowth");

	public static string WrongPercentageGrowth => Keys.GetString("WrongPercentageGrowth");

	public static string WrongSize => Keys.GetString("WrongSize");

	public static string TooFewFiles => Keys.GetString("TooFewFiles");

	public static string UnknownChild => Keys.GetString("UnknownChild");

	public static string InvalidGranteeList => Keys.GetString("InvalidGranteeList");

	public static string DatabaseAlreadyExists => Keys.GetString("DatabaseAlreadyExists");

	public static string ReasonTextIsEncrypted => Keys.GetString("ReasonTextIsEncrypted");

	public static string ReasonPropertyIsNotSupportedOnCurrentServerVersion => Keys.GetString("ReasonPropertyIsNotSupportedOnCurrentServerVersion");

	public static string SqlInnerException => Keys.GetString("SqlInnerException");

	public static string InnerException => Keys.GetString("InnerException");

	public static string InnerWmiException => Keys.GetString("InnerWmiException");

	public static string UnknownError => Keys.GetString("UnknownError");

	public static string OneFilePageSupported => Keys.GetString("OneFilePageSupported");

	public static string SupportedOnlyBelow90 => Keys.GetString("SupportedOnlyBelow90");

	public static string SupportedOnlyOn90 => Keys.GetString("SupportedOnlyOn90");

	public static string SupportedOnlyOn80 => Keys.GetString("SupportedOnlyOn80");

	public static string MustSpecifyOneParameter => Keys.GetString("MustSpecifyOneParameter");

	public static string NeedPSParams => Keys.GetString("NeedPSParams");

	public static string ViewCannotHaveKeys => Keys.GetString("ViewCannotHaveKeys");

	public static string InvalidAcctName => Keys.GetString("InvalidAcctName");

	public static string OperationOnlyInPendingState => Keys.GetString("OperationOnlyInPendingState");

	public static string OperationNotInPendingState => Keys.GetString("OperationNotInPendingState");

	public static string NotXmlIndexOnView => Keys.GetString("NotXmlIndexOnView");

	public static string OneColumnInXmlIndex => Keys.GetString("OneColumnInXmlIndex");

	public static string NoXmlIndexUnique => Keys.GetString("NoXmlIndexUnique");

	public static string NoXmlIndexClustered => Keys.GetString("NoXmlIndexClustered");

	public static string Version80SP3Only => Keys.GetString("Version80SP3Only");

	public static string CantSetDefaultFalse => Keys.GetString("CantSetDefaultFalse");

	public static string TimeoutMustBePositive => Keys.GetString("TimeoutMustBePositive");

	public static string ViewColumnsCannotBeModified => Keys.GetString("ViewColumnsCannotBeModified");

	public static string SystemMessageReadOnly => Keys.GetString("SystemMessageReadOnly");

	public static string ViewFragInfoNotInV7 => Keys.GetString("ViewFragInfoNotInV7");

	public static string DtsNotInstalled => Keys.GetString("DtsNotInstalled");

	public static string NSNotInstalled => Keys.GetString("NSNotInstalled");

	public static string InvalidNSOperation => Keys.GetString("InvalidNSOperation");

	public static string NSNotConnectedToServer => Keys.GetString("NSNotConnectedToServer");

	public static string InvalidThreadPoolSize => Keys.GetString("InvalidThreadPoolSize");

	public static string InvalidQuantumLimit => Keys.GetString("InvalidQuantumLimit");

	public static string InvalidQuantumDuration => Keys.GetString("InvalidQuantumDuration");

	public static string InvalidThrottle => Keys.GetString("InvalidThrottle");

	public static string InvalidFailuresBeforeLoggingEvent => Keys.GetString("InvalidFailuresBeforeLoggingEvent");

	public static string InvalidFailuresBeforeAbort => Keys.GetString("InvalidFailuresBeforeAbort");

	public static string InvalidMulticastRecipientLimit => Keys.GetString("InvalidMulticastRecipientLimit");

	public static string InvalidNotificationClassBatchSize => Keys.GetString("InvalidNotificationClassBatchSize");

	public static string StartTimeGreaterThanADay => Keys.GetString("StartTimeGreaterThanADay");

	public static string NoPermissions => Keys.GetString("NoPermissions");

	public static string PrimaryFgMustHaveFiles => Keys.GetString("PrimaryFgMustHaveFiles");

	public static string NeedToSetParent => Keys.GetString("NeedToSetParent");

	public static string ErrorInCreatingState => Keys.GetString("ErrorInCreatingState");

	public static string BackupFailed => Keys.GetString("BackupFailed");

	public static string RestoreFailed => Keys.GetString("RestoreFailed");

	public static string SetPasswordError => Keys.GetString("SetPasswordError");

	public static string PassPhraseAndIdentityNotSpecified => Keys.GetString("PassPhraseAndIdentityNotSpecified");

	public static string PassPhraseNotSpecified => Keys.GetString("PassPhraseNotSpecified");

	public static string NotEnoughRights => Keys.GetString("NotEnoughRights");

	public static string NoUrnSuffix => Keys.GetString("NoUrnSuffix");

	public static string GetParentFailed => Keys.GetString("GetParentFailed");

	public static string ParentNull => Keys.GetString("ParentNull");

	public static string ExecutionContextPrincipalIsNotSpecified => Keys.GetString("ExecutionContextPrincipalIsNotSpecified");

	public static string ServiceError0 => Keys.GetString("ServiceError0");

	public static string ServiceError1 => Keys.GetString("ServiceError1");

	public static string ServiceError2 => Keys.GetString("ServiceError2");

	public static string ServiceError3 => Keys.GetString("ServiceError3");

	public static string ServiceError4 => Keys.GetString("ServiceError4");

	public static string ServiceError5 => Keys.GetString("ServiceError5");

	public static string ServiceError6 => Keys.GetString("ServiceError6");

	public static string ServiceError7 => Keys.GetString("ServiceError7");

	public static string ServiceError8 => Keys.GetString("ServiceError8");

	public static string ServiceError9 => Keys.GetString("ServiceError9");

	public static string ServiceError10 => Keys.GetString("ServiceError10");

	public static string ServiceError11 => Keys.GetString("ServiceError11");

	public static string ServiceError12 => Keys.GetString("ServiceError12");

	public static string ServiceError13 => Keys.GetString("ServiceError13");

	public static string ServiceError14 => Keys.GetString("ServiceError14");

	public static string ServiceError15 => Keys.GetString("ServiceError15");

	public static string ServiceError16 => Keys.GetString("ServiceError16");

	public static string ServiceError17 => Keys.GetString("ServiceError17");

	public static string ServiceError18 => Keys.GetString("ServiceError18");

	public static string ServiceError19 => Keys.GetString("ServiceError19");

	public static string ServiceError20 => Keys.GetString("ServiceError20");

	public static string ServiceError21 => Keys.GetString("ServiceError21");

	public static string ServiceError22 => Keys.GetString("ServiceError22");

	public static string ServiceError23 => Keys.GetString("ServiceError23");

	public static string ServiceError24 => Keys.GetString("ServiceError24");

	public static string UnsupportedFeatureSqlAgent => Keys.GetString("UnsupportedFeatureSqlAgent");

	public static string UnsupportedFeatureSqlMail => Keys.GetString("UnsupportedFeatureSqlMail");

	public static string UnsupportedFeatureServiceBroker => Keys.GetString("UnsupportedFeatureServiceBroker");

	public static string UnsupportedFeatureFullText => Keys.GetString("UnsupportedFeatureFullText");

	public static string CannotSubscribe => Keys.GetString("CannotSubscribe");

	public static string CannotStartSubscription => Keys.GetString("CannotStartSubscription");

	public static string InnerRegSvrException => Keys.GetString("InnerRegSvrException");

	public static string SqlServerTypeName => Keys.GetString("SqlServerTypeName");

	public static string ColumnsMustBeSpecified => Keys.GetString("ColumnsMustBeSpecified");

	public static string PasswdModiOnlyForStandardLogin => Keys.GetString("PasswdModiOnlyForStandardLogin");

	public static string DenyLoginModiNotForStandardLogin => Keys.GetString("DenyLoginModiNotForStandardLogin");

	public static string CannotCreateExtendedPropertyWithoutSchema => Keys.GetString("CannotCreateExtendedPropertyWithoutSchema");

	public static string InvalidSchema => Keys.GetString("InvalidSchema");

	public static string FailedToChangeSchema => Keys.GetString("FailedToChangeSchema");

	public static string InvalidUrnServerLevel => Keys.GetString("InvalidUrnServerLevel");

	public static string ServerLevelMustBePresent => Keys.GetString("ServerLevelMustBePresent");

	public static string OperationInProgress => Keys.GetString("OperationInProgress");

	public static string CannotChangePassword => Keys.GetString("CannotChangePassword");

	public static string OperationNotSupportedWhenPartOfAUDF => Keys.GetString("OperationNotSupportedWhenPartOfAUDF");

	public static string ReasonIntextMode => Keys.GetString("ReasonIntextMode");

	public static string ReasonNotIntextMode => Keys.GetString("ReasonNotIntextMode");

	public static string CollectionCannotBeModified => Keys.GetString("CollectionCannotBeModified");

	public static string KeyOptionsIncorrect => Keys.GetString("KeyOptionsIncorrect");

	public static string TableOrViewParentForUpdateStatistics => Keys.GetString("TableOrViewParentForUpdateStatistics");

	public static string ServerSphinx => Keys.GetString("ServerSphinx");

	public static string ServerShiloh => Keys.GetString("ServerShiloh");

	public static string ServerYukon => Keys.GetString("ServerYukon");

	public static string PropertyAvailable => Keys.GetString("PropertyAvailable");

	public static string ServerPropertyMustBeSetForOlap => Keys.GetString("ServerPropertyMustBeSetForOlap");

	public static string InvalidInstanceName => Keys.GetString("InvalidInstanceName");

	public static string TransferCtorErr => Keys.GetString("TransferCtorErr");

	public static string UnsupportedVersionException => Keys.GetString("UnsupportedVersionException");

	public static string FailedOperationMessageNotSupportedTempdb => Keys.GetString("FailedOperationMessageNotSupportedTempdb");

	public static string SetAccount => Keys.GetString("SetAccount");

	public static string Create => Keys.GetString("Create");

	public static string Alter => Keys.GetString("Alter");

	public static string Drop => Keys.GetString("Drop");

	public static string Rename => Keys.GetString("Rename");

	public static string Script => Keys.GetString("Script");

	public static string Grant => Keys.GetString("Grant");

	public static string Revoke => Keys.GetString("Revoke");

	public static string Deny => Keys.GetString("Deny");

	public static string GrantWithGrant => Keys.GetString("GrantWithGrant");

	public static string Bind => Keys.GetString("Bind");

	public static string Unbind => Keys.GetString("Unbind");

	public static string AddDefaultConstraint => Keys.GetString("AddDefaultConstraint");

	public static string TestMailProfile => Keys.GetString("TestMailProfile");

	public static string TestNetSend => Keys.GetString("TestNetSend");

	public static string SetState => Keys.GetString("SetState");

	public static string DropAndMove => Keys.GetString("DropAndMove");

	public static string GetSmoObject => Keys.GetString("GetSmoObject");

	public static string AdvancedProperties => Keys.GetString("AdvancedProperties");

	public static string AddCollection => Keys.GetString("AddCollection");

	public static string RemoveCollection => Keys.GetString("RemoveCollection");

	public static string This => Keys.GetString("This");

	public static string Abort => Keys.GetString("Abort");

	public static string AddMember => Keys.GetString("AddMember");

	public static string DropMember => Keys.GetString("DropMember");

	public static string EnumMembers => Keys.GetString("EnumMembers");

	public static string EnumPermissions => Keys.GetString("EnumPermissions");

	public static string AddMemberServer => Keys.GetString("AddMemberServer");

	public static string EnumMemberServers => Keys.GetString("EnumMemberServers");

	public static string RemoveMemberServer => Keys.GetString("RemoveMemberServer");

	public static string ResetOccurrenceCount => Keys.GetString("ResetOccurrenceCount");

	public static string AddNotification => Keys.GetString("AddNotification");

	public static string RemoveNotification => Keys.GetString("RemoveNotification");

	public static string UpdateNotification => Keys.GetString("UpdateNotification");

	public static string EnumNotifications => Keys.GetString("EnumNotifications");

	public static string ApplyToTargetServer => Keys.GetString("ApplyToTargetServer");

	public static string ApplyToTargetServerGroup => Keys.GetString("ApplyToTargetServerGroup");

	public static string EnumAlerts => Keys.GetString("EnumAlerts");

	public static string EnumHistory => Keys.GetString("EnumHistory");

	public static string EnumJobStepOutputLogs => Keys.GetString("EnumJobStepOutputLogs");

	public static string EnumTargetServers => Keys.GetString("EnumTargetServers");

	public static string EnumJobSteps => Keys.GetString("EnumJobSteps");

	public static string Invoke => Keys.GetString("Invoke");

	public static string PurgeHistory => Keys.GetString("PurgeHistory");

	public static string AddSharedSchedule => Keys.GetString("AddSharedSchedule");

	public static string RemoveSharedSchedule => Keys.GetString("RemoveSharedSchedule");

	public static string RemoveAllJobSchedules => Keys.GetString("RemoveAllJobSchedules");

	public static string RemoveAllJobSteps => Keys.GetString("RemoveAllJobSteps");

	public static string RemoveFromTargetServer => Keys.GetString("RemoveFromTargetServer");

	public static string RemoveFromTargetServerGroup => Keys.GetString("RemoveFromTargetServerGroup");

	public static string Start => Keys.GetString("Start");

	public static string Stop => Keys.GetString("Stop");

	public static string SetPassword => Keys.GetString("SetPassword");

	public static string ChangePassword => Keys.GetString("ChangePassword");

	public static string EnumDatabaseMappings => Keys.GetString("EnumDatabaseMappings");

	public static string GetDatabaseUser => Keys.GetString("GetDatabaseUser");

	public static string AddToRole => Keys.GetString("AddToRole");

	public static string AttachDatabase => Keys.GetString("AttachDatabase");

	public static string DetachDatabase => Keys.GetString("DetachDatabase");

	public static string EnumCollations => Keys.GetString("EnumCollations");

	public static string EnumPerformanceCounters => Keys.GetString("EnumPerformanceCounters");

	public static string EnumErrorLogs => Keys.GetString("EnumErrorLogs");

	public static string EnumDatabaseMirrorWitnessRoles => Keys.GetString("EnumDatabaseMirrorWitnessRoles");

	public static string ReadErrorLog => Keys.GetString("ReadErrorLog");

	public static string KillDatabase => Keys.GetString("KillDatabase");

	public static string KillProcess => Keys.GetString("KillProcess");

	public static string GetActiveDBConnectionCount => Keys.GetString("GetActiveDBConnectionCount");

	public static string DropAllActiveDBConnections => Keys.GetString("DropAllActiveDBConnections");

	public static string EnumDirectories => Keys.GetString("EnumDirectories");

	public static string EnumLocks => Keys.GetString("EnumLocks");

	public static string AddPrivateKey => Keys.GetString("AddPrivateKey");

	public static string ExportCertificate => Keys.GetString("ExportCertificate");

	public static string ChangePrivateKeyPassword => Keys.GetString("ChangePrivateKeyPassword");

	public static string RemovePrivateKey => Keys.GetString("RemovePrivateKey");

	public static string AddKeyEncryption => Keys.GetString("AddKeyEncryption");

	public static string DropKeyEncryption => Keys.GetString("DropKeyEncryption");

	public static string SymmetricKeyOpen => Keys.GetString("SymmetricKeyOpen");

	public static string SymmetricKeyClose => Keys.GetString("SymmetricKeyClose");

	public static string EnumLogins => Keys.GetString("EnumLogins");

	public static string EnumWindowsDomainGroups => Keys.GetString("EnumWindowsDomainGroups");

	public static string EnumProcesses => Keys.GetString("EnumProcesses");

	public static string EnumStartupProcedures => Keys.GetString("EnumStartupProcedures");

	public static string EnumWindowsUserInfo => Keys.GetString("EnumWindowsUserInfo");

	public static string EnumWindowsGroupInfo => Keys.GetString("EnumWindowsGroupInfo");

	public static string EnumAvailableMedia => Keys.GetString("EnumAvailableMedia");

	public static string EnumServerAttributes => Keys.GetString("EnumServerAttributes");

	public static string DeleteBackupHistory => Keys.GetString("DeleteBackupHistory");

	public static string Refresh => Keys.GetString("Refresh");

	public static string EnumBoundColumns => Keys.GetString("EnumBoundColumns");

	public static string EnumBoundDataTypes => Keys.GetString("EnumBoundDataTypes");

	public static string CheckAllocations => Keys.GetString("CheckAllocations");

	public static string CheckCatalog => Keys.GetString("CheckCatalog");

	public static string CheckIdentityValues => Keys.GetString("CheckIdentityValues");

	public static string CheckTables => Keys.GetString("CheckTables");

	public static string CheckTable => Keys.GetString("CheckTable");

	public static string Shrink => Keys.GetString("Shrink");

	public static string RecalculateSpaceUsage => Keys.GetString("RecalculateSpaceUsage");

	public static string PrefetchObjects => Keys.GetString("PrefetchObjects");

	public static string EnumTransactions => Keys.GetString("EnumTransactions");

	public static string GetTransactionCount => Keys.GetString("GetTransactionCount");

	public static string ImportXmlSchema => Keys.GetString("ImportXmlSchema");

	public static string ExtendXmlSchema => Keys.GetString("ExtendXmlSchema");

	public static string RemoveFullTextCatalogs => Keys.GetString("RemoveFullTextCatalogs");

	public static string SetDefaultFullTextCatalog => Keys.GetString("SetDefaultFullTextCatalog");

	public static string SetDefaultFileGroup => Keys.GetString("SetDefaultFileGroup");

	public static string CheckFileGroup => Keys.GetString("CheckFileGroup");

	public static string CheckIndex => Keys.GetString("CheckIndex");

	public static string Checkpoint => Keys.GetString("Checkpoint");

	public static string Cleanup => Keys.GetString("Cleanup");

	public static string UpdateLanguageResources => Keys.GetString("UpdateLanguageResources");

	public static string EnumLanguages => Keys.GetString("EnumLanguages");

	public static string ClearHostLoginAccount => Keys.GetString("ClearHostLoginAccount");

	public static string SetProxyAccount => Keys.GetString("SetProxyAccount");

	public static string ClearProxyAccount => Keys.GetString("ClearProxyAccount");

	public static string SetMsxAccount => Keys.GetString("SetMsxAccount");

	public static string ClearMsxAccount => Keys.GetString("ClearMsxAccount");

	public static string CycleErrorLog => Keys.GetString("CycleErrorLog");

	public static string EnumJobHistory => Keys.GetString("EnumJobHistory");

	public static string EnumJobs => Keys.GetString("EnumJobs");

	public static string MoreThanOneProxyAccountIsNotSupported => Keys.GetString("MoreThanOneProxyAccountIsNotSupported");

	public static string AddSubSystems => Keys.GetString("AddSubSystems");

	public static string DeleteJobStepLogs => Keys.GetString("DeleteJobStepLogs");

	public static string RemoveSubSystems => Keys.GetString("RemoveSubSystems");

	public static string EnumSubSystems => Keys.GetString("EnumSubSystems");

	public static string AddMailAccountToProfile => Keys.GetString("AddMailAccountToProfile");

	public static string RemoveMailAccountFromProfile => Keys.GetString("RemoveMailAccountFromProfile");

	public static string EnumMailAccountsForProfile => Keys.GetString("EnumMailAccountsForProfile");

	public static string AddPrincipalToMailProfile => Keys.GetString("AddPrincipalToMailProfile");

	public static string RemovePrincipalFromMailProfile => Keys.GetString("RemovePrincipalFromMailProfile");

	public static string EnumPrincipalsForMailProfile => Keys.GetString("EnumPrincipalsForMailProfile");

	public static string AddLoginToProxyAccount => Keys.GetString("AddLoginToProxyAccount");

	public static string RemoveLoginFromProxyAccount => Keys.GetString("RemoveLoginFromProxyAccount");

	public static string EnumLoginsOfProxyAccount => Keys.GetString("EnumLoginsOfProxyAccount");

	public static string AddServerRoleToProxyAccount => Keys.GetString("AddServerRoleToProxyAccount");

	public static string RemoveServerRoleFromProxyAccount => Keys.GetString("RemoveServerRoleFromProxyAccount");

	public static string EnumServerRolesOfProxyAccount => Keys.GetString("EnumServerRolesOfProxyAccount");

	public static string AddMSDBRoleToProxyAccount => Keys.GetString("AddMSDBRoleToProxyAccount");

	public static string RemoveMSDBRoleFromProxyAccount => Keys.GetString("RemoveMSDBRoleFromProxyAccount");

	public static string EnumMSDBRolesOfProxyAccount => Keys.GetString("EnumMSDBRolesOfProxyAccount");

	public static string MsxDefect => Keys.GetString("MsxDefect");

	public static string MsxEnlist => Keys.GetString("MsxEnlist");

	public static string PurgeJobHistory => Keys.GetString("PurgeJobHistory");

	public static string ReassignJobsByLogin => Keys.GetString("ReassignJobsByLogin");

	public static string DropJobsByLogin => Keys.GetString("DropJobsByLogin");

	public static string StartMonitor => Keys.GetString("StartMonitor");

	public static string StopMonitor => Keys.GetString("StopMonitor");

	public static string EnumProxies => Keys.GetString("EnumProxies");

	public static string DropJobsByServer => Keys.GetString("DropJobsByServer");

	public static string CompareUrn => Keys.GetString("CompareUrn");

	public static string Disable => Keys.GetString("Disable");

	public static string DisableAllIndexes => Keys.GetString("DisableAllIndexes");

	public static string EnableAllIndexes => Keys.GetString("EnableAllIndexes");

	public static string DiscoverDependencies => Keys.GetString("DiscoverDependencies");

	public static string DropBackupHistory => Keys.GetString("DropBackupHistory");

	public static string ChangeMirroringState => Keys.GetString("ChangeMirroringState");

	public static string IsMember => Keys.GetString("IsMember");

	public static string Recreate => Keys.GetString("Recreate");

	public static string Enable => Keys.GetString("Enable");

	public static string EnumColumns => Keys.GetString("EnumColumns");

	public static string EnumForeignKeys => Keys.GetString("EnumForeignKeys");

	public static string EnumIndexes => Keys.GetString("EnumIndexes");

	public static string EnumFragmentation => Keys.GetString("EnumFragmentation");

	public static string EnumReferences => Keys.GetString("EnumReferences");

	public static string SetOffline => Keys.GetString("SetOffline");

	public static string SetOwner => Keys.GetString("SetOwner");

	public static string StartPopulation => Keys.GetString("StartPopulation");

	public static string StopPopulation => Keys.GetString("StopPopulation");

	public static string Rebuild => Keys.GetString("Rebuild");

	public static string Reorganize => Keys.GetString("Reorganize");

	public static string UpdateStatistics => Keys.GetString("UpdateStatistics");

	public static string SetHostLoginAccount => Keys.GetString("SetHostLoginAccount");

	public static string SetMailServerAccount => Keys.GetString("SetMailServerAccount");

	public static string SetMailServerPassword => Keys.GetString("SetMailServerPassword");

	public static string EnumLastStatisticsUpdates => Keys.GetString("EnumLastStatisticsUpdates");

	public static string RebuildIndexes => Keys.GetString("RebuildIndexes");

	public static string ReCompileReferences => Keys.GetString("ReCompileReferences");

	public static string TruncateData => Keys.GetString("TruncateData");

	public static string TruncateLog => Keys.GetString("TruncateLog");

	public static string SwitchPartition => Keys.GetString("SwitchPartition");

	public static string MergeHashPartition => Keys.GetString("MergeHashPartition");

	public static string MergeRangePartition => Keys.GetString("MergeRangePartition");

	public static string SplitHashPartition => Keys.GetString("SplitHashPartition");

	public static string SplitRangePartition => Keys.GetString("SplitRangePartition");

	public static string GetRangeValues => Keys.GetString("GetRangeValues");

	public static string ResetNextUsed => Keys.GetString("ResetNextUsed");

	public static string GetFileGroups => Keys.GetString("GetFileGroups");

	public static string GetDefaultInitFields => Keys.GetString("GetDefaultInitFields");

	public static string SetDefaultInitFields => Keys.GetString("SetDefaultInitFields");

	public static string GetPropertyNames => Keys.GetString("GetPropertyNames");

	public static string SetParent => Keys.GetString("SetParent");

	public static string SetName => Keys.GetString("SetName");

	public static string SetNamespace => Keys.GetString("SetNamespace");

	public static string SetSchema => Keys.GetString("SetSchema");

	public static string ExecuteNonQuery => Keys.GetString("ExecuteNonQuery");

	public static string SetSnapshotIsolation => Keys.GetString("SetSnapshotIsolation");

	public static string EnumNamespaces => Keys.GetString("EnumNamespaces");

	public static string EnumTypes => Keys.GetString("EnumTypes");

	public static string AddSchemaDocument => Keys.GetString("AddSchemaDocument");

	public static string ScriptTransfer => Keys.GetString("ScriptTransfer");

	public static string SetIdentityPhrase => Keys.GetString("SetIdentityPhrase");

	public static string SetEncryptionOptions => Keys.GetString("SetEncryptionOptions");

	public static string EnumStatistics => Keys.GetString("EnumStatistics");

	public static string GetJobByID => Keys.GetString("GetJobByID");

	public static string RemoveJobByID => Keys.GetString("RemoveJobByID");

	public static string RemoveJobsByLogin => Keys.GetString("RemoveJobsByLogin");

	public static string EnumCandidateKeys => Keys.GetString("EnumCandidateKeys");

	public static string ExecuteWithResults => Keys.GetString("ExecuteWithResults");

	public static string UpdateIndexStatistics => Keys.GetString("UpdateIndexStatistics");

	public static string EnumMatchingSPs => Keys.GetString("EnumMatchingSPs");

	public static string EnumObjects => Keys.GetString("EnumObjects");

	public static string ReadBackupHeader => Keys.GetString("ReadBackupHeader");

	public static string ReadMediaHeader => Keys.GetString("ReadMediaHeader");

	public static string DetachedDatabaseInfo => Keys.GetString("DetachedDatabaseInfo");

	public static string IsDetachedPrimaryFile => Keys.GetString("IsDetachedPrimaryFile");

	public static string IsWindowsGroupMember => Keys.GetString("IsWindowsGroupMember");

	public static string EnumDetachedDatabaseFiles => Keys.GetString("EnumDetachedDatabaseFiles");

	public static string EnumDetachedLogFiles => Keys.GetString("EnumDetachedLogFiles");

	public static string ServerEnumMembers => Keys.GetString("ServerEnumMembers");

	public static string Contains => Keys.GetString("Contains");

	public static string PingSqlServerVersion => Keys.GetString("PingSqlServerVersion");

	public static string SetServiceAccount => Keys.GetString("SetServiceAccount");

	public static string ChangeServicePassword => Keys.GetString("ChangeServicePassword");

	public static string ServerActiveDirectoryRegister => Keys.GetString("ServerActiveDirectoryRegister");

	public static string ServerActiveDirectoryUpdateRegistration => Keys.GetString("ServerActiveDirectoryUpdateRegistration");

	public static string ServerActiveDirectoryUnregister => Keys.GetString("ServerActiveDirectoryUnregister");

	public static string DatabaseActiveDirectoryRegister => Keys.GetString("DatabaseActiveDirectoryRegister");

	public static string DatabaseActiveDirectoryUpdateRegistration => Keys.GetString("DatabaseActiveDirectoryUpdateRegistration");

	public static string DatabaseActiveDirectoryUnregister => Keys.GetString("DatabaseActiveDirectoryUnregister");

	public static string RecoverMasterKey => Keys.GetString("RecoverMasterKey");

	public static string RegenerateMasterKey => Keys.GetString("RegenerateMasterKey");

	public static string ImportMasterKey => Keys.GetString("ImportMasterKey");

	public static string ExportMasterKey => Keys.GetString("ExportMasterKey");

	public static string ChangeAcctMasterKey => Keys.GetString("ChangeAcctMasterKey");

	public static string AddEncryptionMasterKey => Keys.GetString("AddEncryptionMasterKey");

	public static string DropEncryptionMasterKey => Keys.GetString("DropEncryptionMasterKey");

	public static string Close => Keys.GetString("Close");

	public static string Open => Keys.GetString("Open");

	public static string EnumKeyEncryptions => Keys.GetString("EnumKeyEncryptions");

	public static string Compare => Keys.GetString("Compare");

	public static string Insert => Keys.GetString("Insert");

	public static string AddRange => Keys.GetString("AddRange");

	public static string SetRange => Keys.GetString("SetRange");

	public static string AddDevice => Keys.GetString("AddDevice");

	public static string SetMirrors => Keys.GetString("SetMirrors");

	public static string SetDatabase => Keys.GetString("SetDatabase");

	public static string SqlManagement => Keys.GetString("SqlManagement");

	public static string Table => Keys.GetString("Table");

	public static string View => Keys.GetString("View");

	public static string Server => Keys.GetString("Server");

	public static string Database => Keys.GetString("Database");

	public static string ExtendedProperty => Keys.GetString("ExtendedProperty");

	public static string DatabaseOptions => Keys.GetString("DatabaseOptions");

	public static string Synonym => Keys.GetString("Synonym");

	public static string FullTextIndex => Keys.GetString("FullTextIndex");

	public static string FullTextIndexColumn => Keys.GetString("FullTextIndexColumn");

	public static string Check => Keys.GetString("Check");

	public static string ForeignKey => Keys.GetString("ForeignKey");

	public static string ForeignKeyColumn => Keys.GetString("ForeignKeyColumn");

	public static string PartitionSchemeParameter => Keys.GetString("PartitionSchemeParameter");

	public static string Trigger => Keys.GetString("Trigger");

	public static string Index => Keys.GetString("Index");

	public static string IndexedColumn => Keys.GetString("IndexedColumn");

	public static string Statistic => Keys.GetString("Statistic");

	public static string StatisticColumn => Keys.GetString("StatisticColumn");

	public static string Column => Keys.GetString("Column");

	public static string DefaultConstraint => Keys.GetString("DefaultConstraint");

	public static string StoredProcedure => Keys.GetString("StoredProcedure");

	public static string StoredProcedureParameter => Keys.GetString("StoredProcedureParameter");

	public static string SqlAssembly => Keys.GetString("SqlAssembly");

	public static string SqlAssemblyFile => Keys.GetString("SqlAssemblyFile");

	public static string UserDefinedType => Keys.GetString("UserDefinedType");

	public static string UserDefinedAggregate => Keys.GetString("UserDefinedAggregate");

	public static string UserDefinedAggregateParameter => Keys.GetString("UserDefinedAggregateParameter");

	public static string FullTextCatalog => Keys.GetString("FullTextCatalog");

	public static string ExtendedStoredProcedure => Keys.GetString("ExtendedStoredProcedure");

	public static string UserDefinedFunction => Keys.GetString("UserDefinedFunction");

	public static string UserDefinedFunctionParameter => Keys.GetString("UserDefinedFunctionParameter");

	public static string User => Keys.GetString("User");

	public static string Schema => Keys.GetString("Schema");

	public static string DatabaseRole => Keys.GetString("DatabaseRole");

	public static string ApplicationRole => Keys.GetString("ApplicationRole");

	public static string LogFile => Keys.GetString("LogFile");

	public static string FileGroup => Keys.GetString("FileGroup");

	public static string DataFile => Keys.GetString("DataFile");

	public static string Default => Keys.GetString("Default");

	public static string Rule => Keys.GetString("Rule");

	public static string UserDefinedDataType => Keys.GetString("UserDefinedDataType");

	public static string PartitionFunction => Keys.GetString("PartitionFunction");

	public static string PartitionScheme => Keys.GetString("PartitionScheme");

	public static string DatabaseActiveDirectory => Keys.GetString("DatabaseActiveDirectory");

	public static string Language => Keys.GetString("Language");

	public static string Login => Keys.GetString("Login");

	public static string ServerRole => Keys.GetString("ServerRole");

	public static string LinkedServer => Keys.GetString("LinkedServer");

	public static string LinkedServerLogin => Keys.GetString("LinkedServerLogin");

	public static string SystemDataType => Keys.GetString("SystemDataType");

	public static string JobServer => Keys.GetString("JobServer");

	public static string Category => Keys.GetString("Category");

	public static string AlertSystem => Keys.GetString("AlertSystem");

	public static string Alert => Keys.GetString("Alert");

	public static string Operator => Keys.GetString("Operator");

	public static string TargetServer => Keys.GetString("TargetServer");

	public static string TargetServerGroup => Keys.GetString("TargetServerGroup");

	public static string Job => Keys.GetString("Job");

	public static string JobStep => Keys.GetString("JobStep");

	public static string JobSchedule => Keys.GetString("JobSchedule");

	public static string Settings => Keys.GetString("Settings");

	public static string Information => Keys.GetString("Information");

	public static string UserOptions => Keys.GetString("UserOptions");

	public static string BackupDevice => Keys.GetString("BackupDevice");

	public static string FullTextService => Keys.GetString("FullTextService");

	public static string ServerActiveDirectory => Keys.GetString("ServerActiveDirectory");

	public static string HttpEndpoint => Keys.GetString("HttpEndpoint");

	public static string SoapConfiguration => Keys.GetString("SoapConfiguration");

	public static string SoapMethod => Keys.GetString("SoapMethod");

	public static string ServerAlias => Keys.GetString("ServerAlias");

	public static string ManagedComputer => Keys.GetString("ManagedComputer");

	public static string Service => Keys.GetString("Service");

	public static string XmlSchemaCollection => Keys.GetString("XmlSchemaCollection");

	public static string Restore => Keys.GetString("Restore");

	public static string RestoreAsync => Keys.GetString("RestoreAsync");

	public static string EnumAvailableSqlServers => Keys.GetString("EnumAvailableSqlServers");

	public static string GetDataType => Keys.GetString("GetDataType");

	public static string SetDataType => Keys.GetString("SetDataType");

	public static string Backup => Keys.GetString("Backup");

	protected WmiMgmtImpl()
	{
	}

	public static string CannotSetPrivilege(string privName)
	{
		return Keys.GetString("CannotSetPrivilege", privName);
	}

	public static string CollectionNotAvailable(string objectname, string serverversion)
	{
		return Keys.GetString("CollectionNotAvailable", objectname, serverversion);
	}

	public static string TriggerNotSupported(string serverversion)
	{
		return Keys.GetString("TriggerNotSupported", serverversion);
	}

	public static string UnknownRecoveryModel(string modelname)
	{
		return Keys.GetString("UnknownRecoveryModel", modelname);
	}

	public static string UnknownUserAccess(string modelname)
	{
		return Keys.GetString("UnknownUserAccess", modelname);
	}

	public static string CannotRenameObject(string objectname, string serverversion)
	{
		return Keys.GetString("CannotRenameObject", objectname, serverversion);
	}

	public static string PropertyNotSet(string propname, string objtype)
	{
		return Keys.GetString("PropertyNotSet", propname, objtype);
	}

	public static string ColumnAlreadyHasDefault(string name)
	{
		return Keys.GetString("ColumnAlreadyHasDefault", name);
	}

	public static string ColumnHasNoDefault(string colname, string defname)
	{
		return Keys.GetString("ColumnHasNoDefault", colname, defname);
	}

	public static string MismatchingServerName(string srvname, string urnsrvname)
	{
		return Keys.GetString("MismatchingServerName", srvname, urnsrvname);
	}

	public static string MismatchingNumberOfMirrors(int i1, int i2)
	{
		return Keys.GetString("MismatchingNumberOfMirrors", i1, i2);
	}

	public static string BackupToPipesNotSupported(string serverversion)
	{
		return Keys.GetString("BackupToPipesNotSupported", serverversion);
	}

	public static string LoginHasUser(string name, string dbname)
	{
		return Keys.GetString("LoginHasUser", name, dbname);
	}

	public static string LoginHasAlias(string name)
	{
		return Keys.GetString("LoginHasAlias", name);
	}

	public static string InvalidLogin(string loginname)
	{
		return Keys.GetString("InvalidLogin", loginname);
	}

	public static string InvalidShrinkMethod(string name)
	{
		return Keys.GetString("InvalidShrinkMethod", name);
	}

	public static string ObjectRefsNonexCol(string objname, string key, string col)
	{
		return Keys.GetString("ObjectRefsNonexCol", objname, key, col);
	}

	public static string CannotAddObject(string typename, string objectName)
	{
		return Keys.GetString("CannotAddObject", typename, objectName);
	}

	public static string UnknownObjectType(string name)
	{
		return Keys.GetString("UnknownObjectType", name);
	}

	public static string UnknownProperty(string name, string typename)
	{
		return Keys.GetString("UnknownProperty", name, typename);
	}

	public static string ObjectNotUnderServer(string name)
	{
		return Keys.GetString("ObjectNotUnderServer", name);
	}

	public static string UnknownChildCollection(string objtypename, string childname)
	{
		return Keys.GetString("UnknownChildCollection", objtypename, childname);
	}

	public static string CantCreateType(string name)
	{
		return Keys.GetString("CantCreateType", name);
	}

	public static string ObjectAlreadyExists(string type, string name)
	{
		return Keys.GetString("ObjectAlreadyExists", type, name);
	}

	public static string NoSqlGen(string objname)
	{
		return Keys.GetString("NoSqlGen", objname);
	}

	public static string InvalidType(string typename)
	{
		return Keys.GetString("InvalidType", typename);
	}

	public static string WrongUrn(string objectType, string urn)
	{
		return Keys.GetString("WrongUrn", objectType, urn);
	}

	public static string NoDepForSysObjects(string objUrn)
	{
		return Keys.GetString("NoDepForSysObjects", objUrn);
	}

	public static string UrnMissing(string urn)
	{
		return Keys.GetString("UrnMissing", urn);
	}

	public static string ObjectWithNoChildren(string objectName, string childName)
	{
		return Keys.GetString("ObjectWithNoChildren", objectName, childName);
	}

	public static string ColumnBeforeNotExisting(string colname)
	{
		return Keys.GetString("ColumnBeforeNotExisting", colname);
	}

	public static string PropNotModifiable(string propName, string objectType)
	{
		return Keys.GetString("PropNotModifiable", propName, objectType);
	}

	public static string UnsupportedLoginMode(string loginmode)
	{
		return Keys.GetString("UnsupportedLoginMode", loginmode);
	}

	public static string CannotSetDefInitFlds(string typename)
	{
		return Keys.GetString("CannotSetDefInitFlds", typename);
	}

	public static string InvalidPathChildCollectionNotFound(string child, string parent)
	{
		return Keys.GetString("InvalidPathChildCollectionNotFound", child, parent);
	}

	public static string WMIException(string errorCode)
	{
		return Keys.GetString("WMIException", errorCode);
	}

	public static string CallingInitChildLevelWithWrongUrn(string urn)
	{
		return Keys.GetString("CallingInitChildLevelWithWrongUrn", urn);
	}

	public static string UnsupportedBackupDeviceType(string name)
	{
		return Keys.GetString("UnsupportedBackupDeviceType", name);
	}

	public static string UnsupportedVersion(string version)
	{
		return Keys.GetString("UnsupportedVersion", version);
	}

	public static string ObjectDoesNotExist(string type, string name)
	{
		return Keys.GetString("ObjectDoesNotExist", type, name);
	}

	public static string UnsupportedFeature(string type)
	{
		return Keys.GetString("UnsupportedFeature", type);
	}

	public static string NoObjectWithoutColumns(string objName)
	{
		return Keys.GetString("NoObjectWithoutColumns", objName);
	}

	public static string ConflictingScriptingOptions(string opt1, string opt2)
	{
		return Keys.GetString("ConflictingScriptingOptions", opt1, opt2);
	}

	public static string EmptyInputParam(string paramName, string paramKind)
	{
		return Keys.GetString("EmptyInputParam", paramName, paramKind);
	}

	public static string MutuallyExclusiveProperties(string prop1, string prop2)
	{
		return Keys.GetString("MutuallyExclusiveProperties", prop1, prop2);
	}

	public static string IndexMustBeClustered(string tableName, string indexName)
	{
		return Keys.GetString("IndexMustBeClustered", tableName, indexName);
	}

	public static string ParentMustExist(string objectType, string objectName)
	{
		return Keys.GetString("ParentMustExist", objectType, objectName);
	}

	public static string OperationNotInPendingState1(string prop1)
	{
		return Keys.GetString("OperationNotInPendingState1", prop1);
	}

	public static string OperationNotInPendingState2(string prop1, string prop2)
	{
		return Keys.GetString("OperationNotInPendingState2", prop1, prop2);
	}

	public static string OperationNotInPendingState3(string prop1, string prop2, string prop3)
	{
		return Keys.GetString("OperationNotInPendingState3", prop1, prop2, prop3);
	}

	public static string NoPropertyChangeForDotNet(string propertyName)
	{
		return Keys.GetString("NoPropertyChangeForDotNet", propertyName);
	}

	public static string ClrNotSupported(string propName, string version)
	{
		return Keys.GetString("ClrNotSupported", propName, version);
	}

	public static string InvalidPropertyNumberRange(string propName, string numberMin, string numberMax)
	{
		return Keys.GetString("InvalidPropertyNumberRange", propName, numberMin, numberMax);
	}

	public static string WrongPropertyValueException(string PropName, string PropValue)
	{
		return Keys.GetString("WrongPropertyValueException", PropName, PropValue);
	}

	public static string InvalidCollation(string name)
	{
		return Keys.GetString("InvalidCollation", name);
	}

	public static string NeedToPassObject(string objtype)
	{
		return Keys.GetString("NeedToPassObject", objtype);
	}

	public static string InexistentDir(string dir)
	{
		return Keys.GetString("InexistentDir", dir);
	}

	public static string CantCreateTempFile(string dir)
	{
		return Keys.GetString("CantCreateTempFile", dir);
	}

	public static string UnknownFilter(string filter)
	{
		return Keys.GetString("UnknownFilter", filter);
	}

	public static string MessageIDTooSmall(int id)
	{
		return Keys.GetString("MessageIDTooSmall", id);
	}

	public static string DataTypeUnsupported(string dt)
	{
		return Keys.GetString("DataTypeUnsupported", dt);
	}

	public static string CantSetTypeName(string type)
	{
		return Keys.GetString("CantSetTypeName", type);
	}

	public static string CantSetTypeSchema(string type)
	{
		return Keys.GetString("CantSetTypeSchema", type);
	}

	public static string UnknownSqlDataType(string type)
	{
		return Keys.GetString("UnknownSqlDataType", type);
	}

	public static string NeedExistingObjForDataType(string objName)
	{
		return Keys.GetString("NeedExistingObjForDataType", objName);
	}

	public static string NoPendingObjForDataType(string state)
	{
		return Keys.GetString("NoPendingObjForDataType", state);
	}

	public static string InvalidOptionForVersion(string method, string optionName, string SqlVersionName)
	{
		return Keys.GetString("InvalidOptionForVersion", method, optionName, SqlVersionName);
	}

	public static string InvalidParamForVersion(string method, string paramName, string SqlVersionName)
	{
		return Keys.GetString("InvalidParamForVersion", method, paramName, SqlVersionName);
	}

	public static string InvalidPropertyValueForVersion(string typeName, string propertyName, string value, string sqlVersionName)
	{
		return Keys.GetString("InvalidPropertyValueForVersion", typeName, propertyName, value, sqlVersionName);
	}

	public static string UnknownLanguageId(string langid)
	{
		return Keys.GetString("UnknownLanguageId", langid);
	}

	public static string UnknownEnumeration(string type)
	{
		return Keys.GetString("UnknownEnumeration", type);
	}

	public static string MissingConfigVariable(string fnName, string varName)
	{
		return Keys.GetString("MissingConfigVariable", fnName, varName);
	}

	public static string ThreadPoolSizeNotValidForEdition(string componentName, int threadPoolSize)
	{
		return Keys.GetString("ThreadPoolSizeNotValidForEdition", componentName, threadPoolSize);
	}

	public static string InvalidPropertySetForExistingObject(string propertyName)
	{
		return Keys.GetString("InvalidPropertySetForExistingObject", propertyName);
	}

	public static string InvalidEncryptArgumentsAndArgumentKey(string EncryptArguments, string ArgumentKey)
	{
		return Keys.GetString("InvalidEncryptArgumentsAndArgumentKey", EncryptArguments, ArgumentKey);
	}

	public static string RequiredChildMissingFromParent(string requiredChildType, string parentName, string parentType)
	{
		return Keys.GetString("RequiredChildMissingFromParent", requiredChildType, parentName, parentType);
	}

	public static string WrongParent(string objectName)
	{
		return Keys.GetString("WrongParent", objectName);
	}

	public static string VerifyFailed(string database, string backupType)
	{
		return Keys.GetString("VerifyFailed", database, backupType);
	}

	public static string VerifyFailed0(string database)
	{
		return Keys.GetString("VerifyFailed0", database);
	}

	public static string InvalidVersion(string version)
	{
		return Keys.GetString("InvalidVersion", version);
	}

	public static string ConflictingSwitches(string prop1, string prop2)
	{
		return Keys.GetString("ConflictingSwitches", prop1, prop2);
	}

	public static string BadCompatLevel(string level)
	{
		return Keys.GetString("BadCompatLevel", level);
	}

	public static string IncludeHeader(string objectType, string name, string dateString)
	{
		return Keys.GetString("IncludeHeader", objectType, name, dateString);
	}

	public static string FullPropertyBag(string propName)
	{
		return Keys.GetString("FullPropertyBag", propName);
	}

	public static string MultipleRowsForUrn(string urnName)
	{
		return Keys.GetString("MultipleRowsForUrn", urnName);
	}

	public static string CouldNotFindKey(string keyName)
	{
		return Keys.GetString("CouldNotFindKey", keyName);
	}

	public static string UnsupportedUrnFilter(string attrib, string functionType)
	{
		return Keys.GetString("UnsupportedUrnFilter", attrib, functionType);
	}

	public static string UnsupportedUrnAttrib(string attrib)
	{
		return Keys.GetString("UnsupportedUrnAttrib", attrib);
	}

	public static string MappingObjectIdMissing(string typename, int id)
	{
		return Keys.GetString("MappingObjectIdMissing", typename, id);
	}

	public static string EmptyMapping(string parent, string mappingname)
	{
		return Keys.GetString("EmptyMapping", parent, mappingname);
	}

	public static string UnknownCategoryName(string name)
	{
		return Keys.GetString("UnknownCategoryName", name);
	}

	public static string UnknownCategoryType(string typename)
	{
		return Keys.GetString("UnknownCategoryType", typename);
	}

	public static string UnknownOperator(string name)
	{
		return Keys.GetString("UnknownOperator", name);
	}

	public static string InvalidServerUrn(string serverName)
	{
		return Keys.GetString("InvalidServerUrn", serverName);
	}

	public static string InvalidUrn(string type)
	{
		return Keys.GetString("InvalidUrn", type);
	}

	public static string WMIProviderNotInstalled(string machineName)
	{
		return Keys.GetString("WMIProviderNotInstalled", machineName);
	}

	public static string PropertyCannotBeChangedAfterConnection(string propertyName)
	{
		return Keys.GetString("PropertyCannotBeChangedAfterConnection", propertyName);
	}

	public static string CouldNotFindManagementObject(string type, string name)
	{
		return Keys.GetString("CouldNotFindManagementObject", type, name);
	}

	public static string NotSupportedNotification(string className, string eventType)
	{
		return Keys.GetString("NotSupportedNotification", className, eventType);
	}

	public static string MissingObjectExceptionText(string parentName, string objectName, string serverVersion)
	{
		return Keys.GetString("MissingObjectExceptionText", parentName, objectName, serverVersion);
	}

	public static string PropertyNotSetExceptionText(string propertyName)
	{
		return Keys.GetString("PropertyNotSetExceptionText", propertyName);
	}

	public static string WrongPropertyValueExceptionText(string propName, string propValue)
	{
		return Keys.GetString("WrongPropertyValueExceptionText", propName, propValue);
	}

	public static string PropertyTypeMismatchExceptionText(string propname, string received, string expected)
	{
		return Keys.GetString("PropertyTypeMismatchExceptionText", propname, received, expected);
	}

	public static string MissingPropertyExceptionText(string propertyName, string serverVersion)
	{
		return Keys.GetString("MissingPropertyExceptionText", propertyName, serverVersion);
	}

	public static string UnknownPropertyExceptionText(string propertyName)
	{
		return Keys.GetString("UnknownPropertyExceptionText", propertyName);
	}

	public static string PropertyReadOnlyExceptionText(string name)
	{
		return Keys.GetString("PropertyReadOnlyExceptionText", name);
	}

	public static string InvalidSmoOperationExceptionText(string opName, string state)
	{
		return Keys.GetString("InvalidSmoOperationExceptionText", opName, state);
	}

	public static string PropertyCannotBeRetrievedExceptionText(string objType, string propname, string objName)
	{
		return Keys.GetString("PropertyCannotBeRetrievedExceptionText", objType, propname, objName);
	}

	public static string ObjectDroppedExceptionText(string type, string name)
	{
		return Keys.GetString("ObjectDroppedExceptionText", type, name);
	}

	public static string UnsupportedObjectNameExceptionText(string objectType)
	{
		return Keys.GetString("UnsupportedObjectNameExceptionText", objectType);
	}

	public static string FailedtoInitialize(string urn)
	{
		return Keys.GetString("FailedtoInitialize", urn);
	}

	public static string PropertyMustBeSpecifiedInUrn(string propName, string nodeType)
	{
		return Keys.GetString("PropertyMustBeSpecifiedInUrn", propName, nodeType);
	}

	public static string InvalidScanType(string scanType)
	{
		return Keys.GetString("InvalidScanType", scanType);
	}

	public static string TempTablesNotSupported(string tableName)
	{
		return Keys.GetString("TempTablesNotSupported", tableName);
	}

	public static string TypeSchemaMustBeDbo(string prop, string value)
	{
		return Keys.GetString("TypeSchemaMustBeDbo", prop, value);
	}

	public static string UnssuportedPermission(string permName)
	{
		return Keys.GetString("UnssuportedPermission", permName);
	}

	public static string InvalidVersionSmoOperation(string version)
	{
		return Keys.GetString("InvalidVersionSmoOperation", version);
	}

	public static string FailedToWriteProperty(string propName, string objectType, string objectName, string reason)
	{
		return Keys.GetString("FailedToWriteProperty", propName, objectType, objectName, reason);
	}

	public static string SyntaxErrorInTextHeader(string objectType, string objectName)
	{
		return Keys.GetString("SyntaxErrorInTextHeader", objectType, objectName);
	}

	public static string IncorrectTextHeader(string objectType, string objectName, string propNameSmall, string propName)
	{
		return Keys.GetString("IncorrectTextHeader", objectType, objectName, propNameSmall, propName);
	}

	public static string PropertyIsInvalidInUrn(string propName, string nodeType)
	{
		return Keys.GetString("PropertyIsInvalidInUrn", propName, nodeType);
	}

	public static string ReasonObjectAlreadyCreated(string objName)
	{
		return Keys.GetString("ReasonObjectAlreadyCreated", objName);
	}

	public static string CantScriptObject(string urn)
	{
		return Keys.GetString("CantScriptObject", urn);
	}

	public static string FailedOperationExceptionText(string opName, string objType, string objName)
	{
		return Keys.GetString("FailedOperationExceptionText", opName, objType, objName);
	}

	public static string FailedOperationExceptionText2(string opName)
	{
		return Keys.GetString("FailedOperationExceptionText2", opName);
	}

	public static string FailedOperationExceptionText3(string opName, string objType, string objName, string Reason)
	{
		return Keys.GetString("FailedOperationExceptionText3", opName, objType, objName, Reason);
	}

	public static string FailedOperationExceptionTextColl(string opName, string coll, string objType, string parent)
	{
		return Keys.GetString("FailedOperationExceptionTextColl", opName, coll, objType, parent);
	}

	public static string FailedOperationExceptionTextScript(string objType, string objName)
	{
		return Keys.GetString("FailedOperationExceptionTextScript", objType, objName);
	}
}
