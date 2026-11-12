using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[DisplayNameKey("IServerSelection_Name")]
[DisplayDescriptionKey("IServerSelection_Desc")]
public interface IServerSelectionFacet : IDmfFacet
{
	[DisplayNameKey("Server_BuildNumberName")]
	[DisplayDescriptionKey("Server_BuildNumberDesc")]
	int BuildNumber { get; }

	[DisplayNameKey("Server_CollationName")]
	[DisplayDescriptionKey("Server_CollationDesc")]
	string Collation { get; }

	[DisplayNameKey("Server_EditionName")]
	[DisplayDescriptionKey("Server_EditionDesc")]
	string Edition { get; }

	[DisplayNameKey("Server_IsCaseSensitiveName")]
	[DisplayDescriptionKey("Server_IsCaseSensitiveDesc")]
	bool IsCaseSensitive { get; }

	[DisplayNameKey("Server_LanguageName")]
	[DisplayDescriptionKey("Server_LanguageDesc")]
	string Language { get; }

	[DisplayNameKey("Server_NamedPipesEnabledName")]
	[DisplayDescriptionKey("Server_NamedPipesEnabledDesc")]
	bool NamedPipesEnabled { get; }

	[DisplayNameKey("Server_OSVersionName")]
	[DisplayDescriptionKey("Server_OSVersionDesc")]
	string OSVersion { get; }

	[DisplayNameKey("Server_PlatformName")]
	[DisplayDescriptionKey("Server_PlatformDesc")]
	string Platform { get; }

	[DisplayNameKey("Server_TcpIpProtocolEnabledName")]
	[DisplayDescriptionKey("Server_TcpEnabledDesc")]
	bool TcpEnabled { get; }

	[DisplayNameKey("Server_VersionMajorName")]
	[DisplayDescriptionKey("Server_VersionMajorDesc")]
	int VersionMajor { get; }

	[DisplayNameKey("Server_VersionMinorName")]
	[DisplayDescriptionKey("Server_VersionMinorDesc")]
	int VersionMinor { get; }
}
