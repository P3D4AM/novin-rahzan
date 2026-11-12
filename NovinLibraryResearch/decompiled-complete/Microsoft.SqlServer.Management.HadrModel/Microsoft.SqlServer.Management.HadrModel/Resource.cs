using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.HadrModel;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resource
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Microsoft.SqlServer.Management.HadrModel.Resource", typeof(Resource).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string AddDatabaseCredentialText => ResourceManager.GetString("AddDatabaseCredentialText", resourceCulture);

	internal static string AddDatabaseToExistingAvailabilityGroupText => ResourceManager.GetString("AddDatabaseToExistingAvailabilityGroupText", resourceCulture);

	internal static string AddDomainUserInAdminGroupTaskException => ResourceManager.GetString("AddDomainUserInAdminGroupTaskException", resourceCulture);

	internal static string AddDomainUserInAdminGroupText => ResourceManager.GetString("AddDomainUserInAdminGroupText", resourceCulture);

	internal static string AddDomainUserInSQLAdminTaskAddDomainAccountToServiceAccountException => ResourceManager.GetString("AddDomainUserInSQLAdminTaskAddDomainAccountToServiceAccountException", resourceCulture);

	internal static string AddDomainUserInSQLAdminTaskAddUserToSqlAdminException => ResourceManager.GetString("AddDomainUserInSQLAdminTaskAddUserToSqlAdminException", resourceCulture);

	internal static string AddDomainUserInSQLAdminText => ResourceManager.GetString("AddDomainUserInSQLAdminText", resourceCulture);

	internal static string AddNodeInClusterTaskClusterNotOnlineException => ResourceManager.GetString("AddNodeInClusterTaskClusterNotOnlineException", resourceCulture);

	internal static string AddNodeInClusterText => ResourceManager.GetString("AddNodeInClusterText", resourceCulture);

	internal static string AddNodeInClusterVerificationError => ResourceManager.GetString("AddNodeInClusterVerificationError", resourceCulture);

	internal static string AddReplicaText => ResourceManager.GetString("AddReplicaText", resourceCulture);

	internal static string AvailabilityGroupNotExists => ResourceManager.GetString("AvailabilityGroupNotExists", resourceCulture);

	internal static string AvailabilityGroupNotJoined => ResourceManager.GetString("AvailabilityGroupNotJoined", resourceCulture);

	internal static string AvailabilityGroupQuorumValidatorException => ResourceManager.GetString("AvailabilityGroupQuorumValidatorException", resourceCulture);

	internal static string AvailabilityModeCompatibilityWarning => ResourceManager.GetString("AvailabilityModeCompatibilityWarning", resourceCulture);

	internal static string BackupDatabaseTaskException => ResourceManager.GetString("BackupDatabaseTaskException", resourceCulture);

	internal static string BackupDatabaseText => ResourceManager.GetString("BackupDatabaseText", resourceCulture);

	internal static string BackupLocationNotProvidedErrorMessage => ResourceManager.GetString("BackupLocationNotProvidedErrorMessage", resourceCulture);

	internal static string BackupLogTaskException => ResourceManager.GetString("BackupLogTaskException", resourceCulture);

	internal static string BackupLogText => ResourceManager.GetString("BackupLogText", resourceCulture);

	internal static string BasicAvailabilityGroupIncompatibleException => ResourceManager.GetString("BasicAvailabilityGroupIncompatibleException", resourceCulture);

	internal static string BasicBackupPriorityReason => ResourceManager.GetString("BasicBackupPriorityReason", resourceCulture);

	internal static string BasicListenerReason => ResourceManager.GetString("BasicListenerReason", resourceCulture);

	internal static string BasicSecondaryRoleReason => ResourceManager.GetString("BasicSecondaryRoleReason", resourceCulture);

	internal static string BasicTooManyDatabasesReason => ResourceManager.GetString("BasicTooManyDatabasesReason", resourceCulture);

	internal static string BasicTooManyReplicasReason => ResourceManager.GetString("BasicTooManyReplicasReason", resourceCulture);

	internal static string CheckingListenerConfiguration => ResourceManager.GetString("CheckingListenerConfiguration", resourceCulture);

	internal static string ClusterNameOnlineValidatorException => ResourceManager.GetString("ClusterNameOnlineValidatorException", resourceCulture);

	internal static string ConfigureEndpointsText => ResourceManager.GetString("ConfigureEndpointsText", resourceCulture);

	internal static string ConfigureSQLForAlwaysonTaskException => ResourceManager.GetString("ConfigureSQLForAlwaysonTaskException", resourceCulture);

	internal static string ConnectRemotePowersehllError => ResourceManager.GetString("ConnectRemotePowersehllError", resourceCulture);

	internal static string CreateAvailabilityGroupListenerText => ResourceManager.GetString("CreateAvailabilityGroupListenerText", resourceCulture);

	internal static string CreateAvailabilityGroupText => ResourceManager.GetString("CreateAvailabilityGroupText", resourceCulture);

	internal static string CreateLoginText => ResourceManager.GetString("CreateLoginText", resourceCulture);

	internal static string CreateStorageServiceNoStatusException => ResourceManager.GetString("CreateStorageServiceNoStatusException", resourceCulture);

	internal static string CreateStorageServiceStatusFailException => ResourceManager.GetString("CreateStorageServiceStatusFailException", resourceCulture);

	internal static string CreateVirtualDisk2 => ResourceManager.GetString("CreateVirtualDisk2", resourceCulture);

	internal static string CreatingAvailabilityGroup => ResourceManager.GetString("CreatingAvailabilityGroup", resourceCulture);

	internal static string CreatingLoginsOnReplica => ResourceManager.GetString("CreatingLoginsOnReplica", resourceCulture);

	internal static string DataBaseDiskSizeValidationException => ResourceManager.GetString("DataBaseDiskSizeValidationException", resourceCulture);

	internal static string DatabaseFileLocationMissingOnReplicaException => ResourceManager.GetString("DatabaseFileLocationMissingOnReplicaException", resourceCulture);

	internal static string DatabaseFileNotInDefaultDirectoryException => ResourceManager.GetString("DatabaseFileNotInDefaultDirectoryException", resourceCulture);

	internal static string DatabasesExistingOnReplica => ResourceManager.GetString("DatabasesExistingOnReplica", resourceCulture);

	internal static string DBMKPasswordIncorrect => ResourceManager.GetString("DBMKPasswordIncorrect", resourceCulture);

	internal static string DeleteDeploymentNoStatusException => ResourceManager.GetString("DeleteDeploymentNoStatusException", resourceCulture);

	internal static string DeleteDeploymentStatusFailException => ResourceManager.GetString("DeleteDeploymentStatusFailException", resourceCulture);

	internal static string DeleteHostedServiceNoStatusException => ResourceManager.GetString("DeleteHostedServiceNoStatusException", resourceCulture);

	internal static string DeleteHostedServiceStatusFailException => ResourceManager.GetString("DeleteHostedServiceStatusFailException", resourceCulture);

	internal static string DeleteStorageServiceNoStatusException => ResourceManager.GetString("DeleteStorageServiceNoStatusException", resourceCulture);

	internal static string DeleteStorageServiceStatusFailException => ResourceManager.GetString("DeleteStorageServiceStatusFailException", resourceCulture);

	internal static string DeleteVirtualMachineNoStatusException => ResourceManager.GetString("DeleteVirtualMachineNoStatusException", resourceCulture);

	internal static string DeleteVirtualMachineStatusException => ResourceManager.GetString("DeleteVirtualMachineStatusException", resourceCulture);

	internal static string DeploymentHealthyValidationDeploymentNotAvailableException => ResourceManager.GetString("DeploymentHealthyValidationDeploymentNotAvailableException", resourceCulture);

	internal static string DeploymentNotExistException => ResourceManager.GetString("DeploymentNotExistException", resourceCulture);

	internal static string DeploymentVirtualNetworkConfigurationVirtualNetworkNotMatchException => ResourceManager.GetString("DeploymentVirtualNetworkConfigurationVirtualNetworkNotMatchException", resourceCulture);

	internal static string DriveNotFoundOnReplica => ResourceManager.GetString("DriveNotFoundOnReplica", resourceCulture);

	internal static string EnableWindowsFailoverClusterTaskException => ResourceManager.GetString("EnableWindowsFailoverClusterTaskException", resourceCulture);

	internal static string EnablingXeventOnReplica => ResourceManager.GetString("EnablingXeventOnReplica", resourceCulture);

	internal static string EncryptionMismatchException => ResourceManager.GetString("EncryptionMismatchException", resourceCulture);

	internal static string EndpointAuthenticationValidatorException => ResourceManager.GetString("EndpointAuthenticationValidatorException", resourceCulture);

	internal static string FailoverValidationException => ResourceManager.GetString("FailoverValidationException", resourceCulture);

	internal static string FieldSetUpdate => ResourceManager.GetString("FieldSetUpdate", resourceCulture);

	internal static string ForcedFailoverTaskText => ResourceManager.GetString("ForcedFailoverTaskText", resourceCulture);

	internal static string FormatDataDiskTaskException => ResourceManager.GetString("FormatDataDiskTaskException", resourceCulture);

	internal static string FreeSpaceUnavailableForDataFileDrive => ResourceManager.GetString("FreeSpaceUnavailableForDataFileDrive", resourceCulture);

	internal static string FreeSpaceUnavailableForLogFileDrive => ResourceManager.GetString("FreeSpaceUnavailableForLogFileDrive", resourceCulture);

	internal static string HostedServiceAffinityGroupConfigurationAffinityGroupNotMatchException => ResourceManager.GetString("HostedServiceAffinityGroupConfigurationAffinityGroupNotMatchException", resourceCulture);

	internal static string ImageArgumentNullException => ResourceManager.GetString("ImageArgumentNullException", resourceCulture);

	internal static string ImageNameArgumentNullException => ResourceManager.GetString("ImageNameArgumentNullException", resourceCulture);

	internal static string InSufficientFreeSpaceForDataFiles => ResourceManager.GetString("InSufficientFreeSpaceForDataFiles", resourceCulture);

	internal static string InSufficientFreeSpaceForLogFiles => ResourceManager.GetString("InSufficientFreeSpaceForLogFiles", resourceCulture);

	internal static string InvalidAvailabilityGroupException => ResourceManager.GetString("InvalidAvailabilityGroupException", resourceCulture);

	internal static string InvalidShare => ResourceManager.GetString("InvalidShare", resourceCulture);

	internal static string JoinAvailabilityGroupError => ResourceManager.GetString("JoinAvailabilityGroupError", resourceCulture);

	internal static string JoinDatabaseToAvailabilityGroupText => ResourceManager.GetString("JoinDatabaseToAvailabilityGroupText", resourceCulture);

	internal static string JoinSecondariesText => ResourceManager.GetString("JoinSecondariesText", resourceCulture);

	internal static string ListenerConfigurationWarning => ResourceManager.GetString("ListenerConfigurationWarning", resourceCulture);

	internal static string ManualFailoverTaskText => ResourceManager.GetString("ManualFailoverTaskText", resourceCulture);

	internal static string MaxHostedServicesException => ResourceManager.GetString("MaxHostedServicesException", resourceCulture);

	internal static string NoAGSupportImageException => ResourceManager.GetString("NoAGSupportImageException", resourceCulture);

	internal static string NoStorageServiceWithServiceNameAndAffinityGroupNameException => ResourceManager.GetString("NoStorageServiceWithServiceNameAndAffinityGroupNameException", resourceCulture);

	internal static string NoStorageServiceWithServiceNameException => ResourceManager.GetString("NoStorageServiceWithServiceNameException", resourceCulture);

	internal static string NotSqlServerImageException => ResourceManager.GetString("NotSqlServerImageException", resourceCulture);

	internal static string OpenFireWallPortsException => ResourceManager.GetString("OpenFireWallPortsException", resourceCulture);

	internal static string PrimaryCannotWriteToLocation => ResourceManager.GetString("PrimaryCannotWriteToLocation", resourceCulture);

	internal static string PrimaryServerNotHadrEnabled => ResourceManager.GetString("PrimaryServerNotHadrEnabled", resourceCulture);

	internal static string QuorumHelperException => ResourceManager.GetString("QuorumHelperException", resourceCulture);

	internal static string QuorumValidationTaskText => ResourceManager.GetString("QuorumValidationTaskText", resourceCulture);

	internal static string ReplicaEndpointStringOutputFormat => ResourceManager.GetString("ReplicaEndpointStringOutputFormat", resourceCulture);

	internal static string RestartSqlServerException => ResourceManager.GetString("RestartSqlServerException", resourceCulture);

	internal static string RestoreDatabaseTaskExcption => ResourceManager.GetString("RestoreDatabaseTaskExcption", resourceCulture);

	internal static string RestoreDatabaseText => ResourceManager.GetString("RestoreDatabaseText", resourceCulture);

	internal static string RestoreLogTaskException => ResourceManager.GetString("RestoreLogTaskException", resourceCulture);

	internal static string RestoreLogText => ResourceManager.GetString("RestoreLogText", resourceCulture);

	internal static string SecondaryCannotReadLocation => ResourceManager.GetString("SecondaryCannotReadLocation", resourceCulture);

	internal static string ServiceOperationStateFailedMsg3 => ResourceManager.GetString("ServiceOperationStateFailedMsg3", resourceCulture);

	internal static string ServiceOperationStateFailedMsg4 => ResourceManager.GetString("ServiceOperationStateFailedMsg4", resourceCulture);

	internal static string ServiceOperationStatusArgumentNullException => ResourceManager.GetString("ServiceOperationStatusArgumentNullException", resourceCulture);

	internal static string StartAlwaysOnXeventSessionText => ResourceManager.GetString("StartAlwaysOnXeventSessionText", resourceCulture);

	internal static string SubscriptionArgumentNullException => ResourceManager.GetString("SubscriptionArgumentNullException", resourceCulture);

	internal static string TaskEventArgsTaskComplete => ResourceManager.GetString("TaskEventArgsTaskComplete", resourceCulture);

	internal static string TaskEventArgsTaskExecutionCancelled => ResourceManager.GetString("TaskEventArgsTaskExecutionCancelled", resourceCulture);

	internal static string TaskEventArgsTaskExecutionFailed => ResourceManager.GetString("TaskEventArgsTaskExecutionFailed", resourceCulture);

	internal static string TaskEventArgsTaskExecutionFailedWithRetry => ResourceManager.GetString("TaskEventArgsTaskExecutionFailedWithRetry", resourceCulture);

	internal static string TaskEventArgsTaskStarted => ResourceManager.GetString("TaskEventArgsTaskStarted", resourceCulture);

	internal static string TotalPrimaryDatabaseSizeValidationException => ResourceManager.GetString("TotalPrimaryDatabaseSizeValidationException", resourceCulture);

	internal static string UpdateAvailabilityGroupListenerText => ResourceManager.GetString("UpdateAvailabilityGroupListenerText", resourceCulture);

	internal static string UserDoesNotHaveViewServerStatePermission => ResourceManager.GetString("UserDoesNotHaveViewServerStatePermission", resourceCulture);

	internal static string ValidatingAvailabilityMode => ResourceManager.GetString("ValidatingAvailabilityMode", resourceCulture);

	internal static string ValidatingBackupLocation => ResourceManager.GetString("ValidatingBackupLocation", resourceCulture);

	internal static string ValidatingBasicAvailabilityGroupOptions => ResourceManager.GetString("ValidatingBasicAvailabilityGroupOptions", resourceCulture);

	internal static string ValidatingDatabaseFileExistingError => ResourceManager.GetString("ValidatingDatabaseFileExistingError", resourceCulture);

	internal static string ValidatingDatabaseFileLocationCompatibility => ResourceManager.GetString("ValidatingDatabaseFileLocationCompatibility", resourceCulture);

	internal static string ValidatingDatabaseFilesNotExistsOnSecondary => ResourceManager.GetString("ValidatingDatabaseFilesNotExistsOnSecondary", resourceCulture);

	internal static string ValidatingDatabaseMasterKey => ResourceManager.GetString("ValidatingDatabaseMasterKey", resourceCulture);

	internal static string ValidatingDatabaseNotExistOnSecondary => ResourceManager.GetString("ValidatingDatabaseNotExistOnSecondary", resourceCulture);

	internal static string ValidatingDiskSpace => ResourceManager.GetString("ValidatingDiskSpace", resourceCulture);

	internal static string ValidatingEndpointEncryption => ResourceManager.GetString("ValidatingEndpointEncryption", resourceCulture);

	internal static string ValidatingFailoverSettings => ResourceManager.GetString("ValidatingFailoverSettings", resourceCulture);

	internal static string ValidatingWSFCQuorumConfiguration => ResourceManager.GetString("ValidatingWSFCQuorumConfiguration", resourceCulture);

	internal static string ValidatorEventArgsValidationComplete => ResourceManager.GetString("ValidatorEventArgsValidationComplete", resourceCulture);

	internal static string ValidatorEventArgsValidationFailed => ResourceManager.GetString("ValidatorEventArgsValidationFailed", resourceCulture);

	internal static string ValidatorEventArgsValidationFailedWithRetry => ResourceManager.GetString("ValidatorEventArgsValidationFailedWithRetry", resourceCulture);

	internal static string ValidatorEventArgsValidationStarted => ResourceManager.GetString("ValidatorEventArgsValidationStarted", resourceCulture);

	internal static string VirtualNetworkLocationHashIDNullException => ResourceManager.GetString("VirtualNetworkLocationHashIDNullException", resourceCulture);

	internal static string WaitForAvailabilityGroupOnline => ResourceManager.GetString("WaitForAvailabilityGroupOnline", resourceCulture);

	internal static string WaitForAvailabilityGroupOnlineText => ResourceManager.GetString("WaitForAvailabilityGroupOnlineText", resourceCulture);

	internal static string WaitForRoleChange => ResourceManager.GetString("WaitForRoleChange", resourceCulture);

	internal static string WizardLaunchFailureQuorumLoss => ResourceManager.GetString("WizardLaunchFailureQuorumLoss", resourceCulture);

	internal Resource()
	{
	}
}
