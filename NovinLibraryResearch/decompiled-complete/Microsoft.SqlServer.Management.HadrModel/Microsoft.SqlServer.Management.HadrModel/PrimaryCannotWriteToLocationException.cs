using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class PrimaryCannotWriteToLocationException : HadrTaskBaseException
{
	public PrimaryCannotWriteToLocationException(string primaryServerName, string backupLocation)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.PrimaryCannotWriteToLocation, primaryServerName, backupLocation))
	{
	}

	public PrimaryCannotWriteToLocationException(string primaryServerName, string backupLocation, Exception innerException)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.PrimaryCannotWriteToLocation, primaryServerName, backupLocation), innerException)
	{
	}
}
