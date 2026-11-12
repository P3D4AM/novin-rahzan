using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class WmiRequest : Request
{
	private bool bRequestingTheWhereClause;

	internal bool RequestingTheWhereClause
	{
		get
		{
			return bRequestingTheWhereClause;
		}
		set
		{
			bRequestingTheWhereClause = value;
		}
	}
}
