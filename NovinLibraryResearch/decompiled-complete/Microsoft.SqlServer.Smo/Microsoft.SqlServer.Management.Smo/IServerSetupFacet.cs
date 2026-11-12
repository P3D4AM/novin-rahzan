using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[DisplayNameKey("IServerSetupFacet_Name")]
[DisplayDescriptionKey("IServerSetupFacet_Desc")]
[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
public interface IServerSetupFacet : IDmfFacet
{
	[DisplayNameKey("Server_ServiceNameName")]
	[DisplayDescriptionKey("Server_ServiceNameDesc")]
	string ServiceName { get; }

	[DisplayNameKey("Server_ServiceAccountName")]
	[DisplayDescriptionKey("Server_ServiceAccountDesc")]
	string EngineServiceAccount { get; }

	[DisplayNameKey("Server_ServiceStartModeName")]
	[DisplayDescriptionKey("Server_ServiceStartModeDesc")]
	ServiceStartMode ServiceStartMode { get; }

	[DisplayNameKey("Server_InstanceNameName")]
	[DisplayDescriptionKey("Server_InstanceNameDesc")]
	string InstanceName { get; }

	[DisplayNameKey("IServerSetupFacet_ServiceInstanceIdSuffixName")]
	[DisplayDescriptionKey("IServerSetupFacet_ServiceInstanceIdSuffixDesc")]
	string ServiceInstanceIdSuffix { get; }

	[DisplayNameKey("Server_FilestreamLevelName")]
	[DisplayDescriptionKey("Server_FilestreamLevelDesc")]
	FileStreamEffectiveLevel FilestreamLevel { get; }

	[DisplayNameKey("Server_FilestreamShareNameName")]
	[DisplayDescriptionKey("Server_FilestreamShareNameDesc")]
	string FilestreamShareName { get; }

	[DisplayNameKey("IServerConfigurationFacet_UserInstancesEnabledName")]
	[DisplayDescriptionKey("IServerConfigurationFacet_UserInstancesEnabledDesc")]
	bool UserInstancesEnabled { get; }

	[DisplayNameKey("Server_CollationName")]
	[DisplayDescriptionKey("Server_CollationDesc")]
	string Collation { get; }

	[DisplayNameKey("Server_SqlDomainGroupName")]
	[DisplayDescriptionKey("Server_SqlDomainGroupDesc")]
	string SqlDomainGroup { get; }

	[DisplayNameKey("IServerSetupFacet_WindowsUsersAndGroupsInSysadminRoleName")]
	[DisplayDescriptionKey("IServerSetupFacet_WindowsUsersAndGroupsInSysadminRoleDesc")]
	string[] WindowsUsersAndGroupsInSysadminRole { get; }

	[DisplayNameKey("Server_LoginModeName")]
	[DisplayDescriptionKey("Server_LoginModeDesc")]
	ServerLoginMode LoginMode { get; }

	[DisplayNameKey("Server_InstallDataDirectoryName")]
	[DisplayDescriptionKey("Server_InstallDataDirectoryDesc")]
	string InstallDataDirectory { get; }

	[DisplayNameKey("Server_BackupDirectoryName")]
	[DisplayDescriptionKey("Server_BackupDirectoryDesc")]
	string BackupDirectory { get; }

	[DisplayNameKey("Server_DefaultFileName")]
	[DisplayDescriptionKey("Server_DefaultFileDesc")]
	string DefaultFile { get; }

	[DisplayNameKey("Server_DefaultLogName")]
	[DisplayDescriptionKey("Server_DefaultLogDesc")]
	string DefaultLog { get; }

	[DisplayNameKey("IServerSetupFacet_TempdbPrimaryFilePathName")]
	[DisplayDescriptionKey("IServerSetupFacet_TempdbPrimaryFilePathDesc")]
	string TempdbPrimaryFilePath { get; }

	[DisplayNameKey("IServerSetupFacet_TempdbLogPathName")]
	[DisplayDescriptionKey("IServerSetupFacet_TempdbLogPathDesc")]
	string TempdbLogPath { get; }

	[DisplayNameKey("JobServer_ServiceStartModeName")]
	[DisplayDescriptionKey("JobServer_ServiceStartModeDesc")]
	ServiceStartMode AgentStartMode { get; }

	[DisplayNameKey("JobServer_ServiceAccountName")]
	[DisplayDescriptionKey("JobServer_ServiceAccountDesc")]
	string AgentServiceAccount { get; }

	[DisplayNameKey("JobServer_AgentDomainGroupName")]
	[DisplayDescriptionKey("JobServer_AgentDomainGroupDesc")]
	string AgentDomainGroup { get; }

	[DisplayNameKey("Server_NamedPipesEnabledName")]
	[DisplayDescriptionKey("Server_NamedPipesEnabledDesc")]
	bool NamedPipesEnabled { get; }

	[DisplayNameKey("Server_TcpEnabledName")]
	[DisplayDescriptionKey("Server_TcpEnabledDesc")]
	bool TcpEnabled { get; }

	[DisplayNameKey("Server_InstallSharedDirectoryName")]
	[DisplayDescriptionKey("Server_InstallSharedDirectoryDesc")]
	string InstallSharedDirectory { get; }

	[DisplayNameKey("Server_BrowserStartModeName")]
	[DisplayDescriptionKey("Server_InstallSharedDirectoryDesc")]
	ServiceStartMode BrowserStartMode { get; }

	[DisplayNameKey("Server_BrowserServiceAccountName")]
	[DisplayDescriptionKey("Server_BrowserServiceAccountDesc")]
	string BrowserServiceAccount { get; }
}
