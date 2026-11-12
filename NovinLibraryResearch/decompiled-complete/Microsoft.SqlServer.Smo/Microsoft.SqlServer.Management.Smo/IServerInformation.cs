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
[DisplayNameKey("IServerInformation_Name")]
[DisplayDescriptionKey("IServerInformation_Desc")]
[RootFacet(typeof(Server))]
public interface IServerInformation : IDmfFacet
{
	[DisplayNameKey("Server_CollationName")]
	[DisplayDescriptionKey("Server_CollationDesc")]
	string Collation { get; }

	[DisplayNameKey("Server_EditionName")]
	[DisplayDescriptionKey("Server_EditionDesc")]
	string Edition { get; }

	[DisplayNameKey("Server_Name")]
	[DisplayDescriptionKey("Server_Desc")]
	string ErrorLogPath { get; }

	[DisplayNameKey("Server_IsCaseSensitiveName")]
	[DisplayDescriptionKey("Server_IsCaseSensitiveDesc")]
	bool IsCaseSensitive { get; }

	[DisplayNameKey("Server_IsClusteredName")]
	[DisplayDescriptionKey("Server_IsClusteredDesc")]
	bool IsClustered { get; }

	[DisplayNameKey("Server_IsFullTextInstalledName")]
	[DisplayDescriptionKey("Server_IsFullTextInstalledDesc")]
	bool IsFullTextInstalled { get; }

	[DisplayNameKey("Server_IsPolyBaseInstalledName")]
	[DisplayDescriptionKey("Server_IsPolyBaseInstalledDesc")]
	bool IsPolyBaseInstalled { get; }

	[DisplayNameKey("Server_IsSingleUserName")]
	[DisplayDescriptionKey("Server_IsSingleUserDesc")]
	bool IsSingleUser { get; }

	[DisplayNameKey("Server_LanguageName")]
	[DisplayDescriptionKey("Server_LanguageDesc")]
	string Language { get; }

	[DisplayNameKey("Server_MasterDBLogPathName")]
	[DisplayDescriptionKey("Server_MasterDBLogPathDesc")]
	string MasterDBLogPath { get; }

	[DisplayNameKey("Server_MasterDBPathName")]
	[DisplayDescriptionKey("Server_MasterDBPathDesc")]
	string MasterDBPath { get; }

	[DisplayNameKey("Server_MaxPrecisionName")]
	[DisplayDescriptionKey("Server_MaxPrecisionDesc")]
	byte MaxPrecision { get; }

	[DisplayNameKey("Server_NetNameName")]
	[DisplayDescriptionKey("Server_NetNameDesc")]
	string NetName { get; }

	[DisplayNameKey("Server_OSVersionName")]
	[DisplayDescriptionKey("Server_OSVersionDesc")]
	string OSVersion { get; }

	[DisplayNameKey("Server_PhysicalMemoryName")]
	[DisplayDescriptionKey("Server_PhysicalMemoryDesc")]
	int PhysicalMemory { get; }

	[DisplayNameKey("Server_PlatformName")]
	[DisplayDescriptionKey("Server_PlatformDesc")]
	string Platform { get; }

	[DisplayNameKey("Server_ProcessorsName")]
	[DisplayDescriptionKey("Server_ProcessorsDesc")]
	int Processors { get; }

	[DisplayNameKey("Server_ProductName")]
	[DisplayDescriptionKey("Server_ProductDesc")]
	string Product { get; }

	[DisplayNameKey("Server_ProductLevelName")]
	[DisplayDescriptionKey("Server_ProductLevelDesc")]
	string ProductLevel { get; }

	[DisplayNameKey("Server_RootDirectoryName")]
	[DisplayDescriptionKey("Server_RootDirectoryDesc")]
	string RootDirectory { get; }

	[DisplayNameKey("Server_VersionStringName")]
	[DisplayDescriptionKey("Server_VersionStringDesc")]
	string VersionString { get; }

	[DisplayNameKey("Server_EngineEditionName")]
	[DisplayDescriptionKey("Server_EngineEditionDesc")]
	Edition EngineEdition { get; }

	[DisplayNameKey("Server_VersionMajorName")]
	[DisplayDescriptionKey("Server_VersionMajorDesc")]
	int VersionMajor { get; }

	[DisplayNameKey("Server_VersionMinorName")]
	[DisplayDescriptionKey("Server_VersionMinorDesc")]
	int VersionMinor { get; }

	[DisplayNameKey("Server_BuildClrVersionStringName")]
	[DisplayDescriptionKey("Server_BuildClrVersionStringDesc")]
	string BuildClrVersionString { get; }

	[DisplayNameKey("Server_BuildNumberName")]
	[DisplayDescriptionKey("Server_BuildNumberDesc")]
	int BuildNumber { get; }

	[DisplayNameKey("Server_CollationIDName")]
	[DisplayDescriptionKey("Server_CollationIDDesc")]
	int CollationID { get; }

	[DisplayNameKey("Server_ComparisonStyleName")]
	[DisplayDescriptionKey("Server_ComparisonStyleDesc")]
	int ComparisonStyle { get; }

	[DisplayNameKey("Server_ComputerNamePhysicalNetBIOSName")]
	[DisplayDescriptionKey("Server_ComputerNamePhysicalNetBIOSDesc")]
	string ComputerNamePhysicalNetBIOS { get; }

	[DisplayNameKey("Server_ResourceLastUpdateDateTimeName")]
	[DisplayDescriptionKey("Server_ResourceLastUpdateDateTimeDesc")]
	DateTime ResourceLastUpdateDateTime { get; }

	[DisplayNameKey("Server_ResourceVersionStringName")]
	[DisplayDescriptionKey("Server_ResourceVersionStringDesc")]
	string ResourceVersionString { get; }

	[DisplayNameKey("Server_SqlCharSetName")]
	[DisplayDescriptionKey("Server_SqlCharSetDesc")]
	short SqlCharSet { get; }

	[DisplayNameKey("Server_SqlCharSetNameName")]
	[DisplayDescriptionKey("Server_SqlCharSetNameDesc")]
	string SqlCharSetName { get; }

	[DisplayNameKey("Server_SqlSortOrderName")]
	[DisplayDescriptionKey("Server_SqlSortOrderDesc")]
	short SqlSortOrder { get; }

	[DisplayNameKey("Server_SqlSortOrderNameName")]
	[DisplayDescriptionKey("Server_SqlSortOrderNameDesc")]
	string SqlSortOrderName { get; }

	[DisplayNameKey("Server_IsHadrEnabledName")]
	[DisplayDescriptionKey("Server_IsHadrEnabledDesc")]
	bool IsHadrEnabled { get; }

	[DisplayNameKey("Server_IsXTPSupported")]
	[DisplayDescriptionKey("Server_IsXTPSupportedDesc")]
	bool IsXTPSupported { get; }
}
