using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class EncryptionAlgorithmMismatchException : HadrValidationErrorException
{
	public EncryptionAlgorithmMismatchException(string replicasString)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.EncryptionMismatchException, replicasString))
	{
	}
}
