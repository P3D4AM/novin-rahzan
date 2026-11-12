using Microsoft.SqlServer.Management.Smo.Internal;

namespace Microsoft.SqlServer.Management.Smo;

internal class PostProcessCreateSqlSecureString : PostProcess
{
	public override object GetColumnData(string name, object data, DataProvider dp)
	{
		return new SqlSecureString(GetTriggeredObject(dp, 0).ToString());
	}
}
