using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class SecondaryCannotReadLocationException : HadrTaskBaseException
{
	public SecondaryCannotReadLocationException(string secondaryServerName, string backupLocation)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.SecondaryCannotReadLocation, secondaryServerName, backupLocation))
	{
	}

	public SecondaryCannotReadLocationException(string secondaryServerName, string backupLocation, Exception innerException)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.SecondaryCannotReadLocation, secondaryServerName, backupLocation), innerException)
	{
	}
}
