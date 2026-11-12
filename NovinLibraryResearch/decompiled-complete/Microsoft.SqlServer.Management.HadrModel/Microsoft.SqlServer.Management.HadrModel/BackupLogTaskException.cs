using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class BackupLogTaskException : HadrTaskBaseException
{
	public BackupLogTaskException(string databaseName, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.BackupLogTaskException, databaseName), inner)
	{
	}

	public BackupLogTaskException(string databaseName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.BackupLogTaskException, databaseName))
	{
	}
}
