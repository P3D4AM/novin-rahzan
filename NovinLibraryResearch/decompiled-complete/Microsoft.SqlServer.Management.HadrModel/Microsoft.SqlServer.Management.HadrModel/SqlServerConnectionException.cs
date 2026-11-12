using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class SqlServerConnectionException : HadrTaskBaseException
{
	public SqlServerConnectionException(string VMIPAddress)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AddDomainUserInAdminGroupTaskException, VMIPAddress))
	{
	}
}
