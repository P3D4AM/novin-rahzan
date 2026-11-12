using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class RestoreLogTaskException : HadrTaskBaseException
{
	public RestoreLogTaskException(string DatabaseName, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.RestoreLogTaskException, DatabaseName), inner)
	{
	}
}
