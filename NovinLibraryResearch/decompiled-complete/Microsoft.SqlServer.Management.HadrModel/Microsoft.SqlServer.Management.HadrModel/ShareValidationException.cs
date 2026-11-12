using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class ShareValidationException : HadrTaskBaseException
{
	public ShareValidationException(string backupLocation)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.InvalidShare, backupLocation))
	{
	}

	public ShareValidationException(string backupLocation, Exception innerException)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.InvalidShare, backupLocation), innerException)
	{
	}
}
