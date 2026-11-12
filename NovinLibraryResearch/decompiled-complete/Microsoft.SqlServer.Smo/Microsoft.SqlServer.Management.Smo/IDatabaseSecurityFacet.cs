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
[DisplayNameKey("IDatabaseSecurityFacet_Name")]
[DisplayDescriptionKey("IDatabaseSecurityFacet_Desc")]
public interface IDatabaseSecurityFacet : IDmfFacet
{
	[DisplayNameKey("Database_TrustworthyName")]
	[DisplayDescriptionKey("Database_TrustworthyDesc")]
	bool Trustworthy { get; set; }

	[DisplayNameKey("IDatabaseSecurityFacet_IsOwnerSysadminName")]
	[DisplayDescriptionKey("IDatabaseSecurityFacet_IsOwnerSysadminDesc")]
	bool IsOwnerSysadmin { get; }
}
