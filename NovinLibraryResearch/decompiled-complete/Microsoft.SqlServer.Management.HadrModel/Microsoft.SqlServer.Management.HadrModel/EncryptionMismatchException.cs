using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class EncryptionMismatchException : HadrTaskBaseException
{
	public EncryptionMismatchException(string replicasString)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.EncryptionMismatchException, replicasString))
	{
	}
}
