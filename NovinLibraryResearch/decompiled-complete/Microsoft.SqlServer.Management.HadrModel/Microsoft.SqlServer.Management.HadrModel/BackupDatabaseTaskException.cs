using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class BackupDatabaseTaskException : HadrTaskBaseException
{
	public BackupDatabaseTaskException(string DatabaseName, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.BackupDatabaseTaskException, DatabaseName), inner)
	{
	}

	public BackupDatabaseTaskException(string DatabaseName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.BackupDatabaseTaskException, DatabaseName))
	{
	}
}
