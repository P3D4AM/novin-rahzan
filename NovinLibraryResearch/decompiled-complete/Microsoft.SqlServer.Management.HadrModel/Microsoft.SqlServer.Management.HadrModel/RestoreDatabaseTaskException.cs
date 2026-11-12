using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class RestoreDatabaseTaskException : HadrTaskBaseException
{
	public RestoreDatabaseTaskException(string DatabaseName, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.RestoreDatabaseTaskExcption, DatabaseName), inner)
	{
	}
}
