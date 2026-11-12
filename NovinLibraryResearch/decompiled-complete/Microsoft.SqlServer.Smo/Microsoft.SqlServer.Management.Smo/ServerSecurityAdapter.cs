using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public class ServerSecurityAdapter : ServerAdapterBase, IDmfAdapter, IServerSecurityFacet, IDmfFacet
{
	public bool CmdExecRightsForSystemAdminsOnly => base.Server.JobServer.SysAdminOnly;

	public bool ReplaceAlertTokensEnabled
	{
		get
		{
			return base.Server.JobServer.ReplaceAlertTokensEnabled;
		}
		set
		{
			base.Server.JobServer.ReplaceAlertTokensEnabled = value;
		}
	}

	public bool ProxyAccountIsGrantedToPublicRole => base.Server.Roles["public"].EnumAgentProxyAccounts().Rows.Count > 0;

	public bool ProxyAccountEnabled => base.Server.ProxyAccount.IsEnabled;

	public bool PublicServerRoleIsGrantedPermissions
	{
		get
		{
			bool result = false;
			ServerPermissionInfo[] array = base.Server.EnumServerPermissions("public");
			foreach (ServerPermissionInfo serverPermissionInfo in array)
			{
				if (serverPermissionInfo.PermissionState == PermissionState.Grant || serverPermissionInfo.PermissionState == PermissionState.GrantWithGrant)
				{
					return true;
				}
			}
			ObjectPermissionInfo[] array2 = base.Server.EnumObjectPermissions("public");
			foreach (ObjectPermissionInfo objectPermissionInfo in array2)
			{
				if (objectPermissionInfo.PermissionState == PermissionState.Grant || objectPermissionInfo.PermissionState == PermissionState.GrantWithGrant)
				{
					return true;
				}
			}
			return result;
		}
	}

	public ServerSecurityAdapter(Server obj)
		: base(obj)
	{
	}

	public override void Refresh()
	{
		base.Server.Information.Refresh();
		base.Server.Configuration.Refresh();
		base.Server.Settings.Refresh();
		base.Server.JobServer.Refresh();
	}

	public override void Alter()
	{
		base.Server.Configuration.Alter(overrideValueChecking: true);
		base.Server.JobServer.Alter();
	}
}
