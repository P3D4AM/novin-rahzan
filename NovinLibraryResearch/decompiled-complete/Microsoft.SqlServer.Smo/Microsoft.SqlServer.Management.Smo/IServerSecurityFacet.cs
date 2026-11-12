using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[CLSCompliant(false)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("ServerSecurityName")]
[DisplayDescriptionKey("ServerSecurityDesc")]
public interface IServerSecurityFacet : IDmfFacet
{
	[DisplayNameKey("PublicServerRoleIsGrantedPermissionsName")]
	[DisplayDescriptionKey("PublicServerRoleIsGrantedPermissionsDesc")]
	bool PublicServerRoleIsGrantedPermissions { get; }

	[DisplayNameKey("LoginModeName")]
	[DisplayDescriptionKey("LoginModeDesc")]
	ServerLoginMode LoginMode { get; }

	[DisplayNameKey("XPCmdShellEnabledName")]
	[DisplayDescriptionKey("XPCmdShellEnabledDesc")]
	bool XPCmdShellEnabled { get; set; }

	[DisplayNameKey("CrossDBOwnershipChainingEnabledName")]
	[DisplayDescriptionKey("CrossDBOwnershipChainingEnabledDesc")]
	bool CrossDBOwnershipChainingEnabled { get; set; }

	[DisplayNameKey("CommonCriteriaComplianceEnabledName")]
	[DisplayDescriptionKey("CommonCriteriaComplianceEnabledDesc")]
	bool CommonCriteriaComplianceEnabled { get; }

	[DisplayNameKey("IServerSecurityFacet_CmdExecRightsForSystemAdminsOnlyName")]
	[DisplayDescriptionKey("IServerSecurityFacet_CmdExecRightsForSystemAdminsOnlyDesc")]
	bool CmdExecRightsForSystemAdminsOnly { get; }

	[DisplayNameKey("IServerSecurityFacet_ProxyAccountIsGrantedToPublicRoleName")]
	[DisplayDescriptionKey("IServerSecurityFacet_ProxyAccountIsGrantedToPublicRoleDesc")]
	bool ProxyAccountIsGrantedToPublicRole { get; }

	[DisplayNameKey("IServerSecurityFacet_ReplaceAlertTokensEnabledName")]
	[DisplayDescriptionKey("IServerSecurityFacet_ReplaceAlertTokensEnabledDesc")]
	bool ReplaceAlertTokensEnabled { get; set; }

	[DisplayNameKey("IServerSecurityFacet_ProxyAccountEnabledName")]
	[DisplayDescriptionKey("IServerSecurityFacet_ProxyAccountEnabledDesc")]
	bool ProxyAccountEnabled { get; }
}
