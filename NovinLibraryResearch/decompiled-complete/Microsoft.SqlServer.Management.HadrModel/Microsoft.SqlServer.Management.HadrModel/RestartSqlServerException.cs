using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class RestartSqlServerException : HadrTaskBaseException
{
	public RestartSqlServerException(string InstanceName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.RestartSqlServerException, InstanceName))
	{
	}
}
