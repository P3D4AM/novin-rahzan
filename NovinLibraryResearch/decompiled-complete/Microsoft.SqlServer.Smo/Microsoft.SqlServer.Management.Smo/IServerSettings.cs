using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[DisplayNameKey("IServerSettings_Name")]
[DisplayDescriptionKey("IServerSettings_Desc")]
[RootFacet(typeof(Server))]
public interface IServerSettings : IDmfFacet
{
	[DisplayNameKey("Server_AuditLevelName")]
	[DisplayDescriptionKey("Server_AuditLevelDesc")]
	AuditLevel AuditLevel { get; set; }

	[DisplayNameKey("Server_BackupDirectoryName")]
	[DisplayDescriptionKey("Server_BackupDirectoryDesc")]
	string BackupDirectory { get; set; }

	[DisplayNameKey("Server_DefaultFileName")]
	[DisplayDescriptionKey("Server_DefaultFileDesc")]
	string DefaultFile { get; set; }

	[DisplayNameKey("Server_DefaultLogName")]
	[DisplayDescriptionKey("Server_DefaultLogDesc")]
	string DefaultLog { get; set; }

	[DisplayNameKey("Server_LoginModeName")]
	[DisplayDescriptionKey("Server_LoginModeDesc")]
	ServerLoginMode LoginMode { get; }

	[DisplayNameKey("Server_MailProfileName")]
	[DisplayDescriptionKey("Server_MailProfileDesc")]
	string MailProfile { get; set; }

	[DisplayNameKey("Server_NumberOfLogFilesName")]
	[DisplayDescriptionKey("Server_NumberOfLogFilesDesc")]
	int NumberOfLogFiles { get; set; }

	[DisplayNameKey("Server_PerfMonModeName")]
	[DisplayDescriptionKey("Server_PerfMonModeDesc")]
	PerfMonMode PerfMonMode { get; set; }

	[DisplayNameKey("Server_TapeLoadWaitTimeName")]
	[DisplayDescriptionKey("Server_TapeLoadWaitTimeDesc")]
	int TapeLoadWaitTime { get; set; }
}
