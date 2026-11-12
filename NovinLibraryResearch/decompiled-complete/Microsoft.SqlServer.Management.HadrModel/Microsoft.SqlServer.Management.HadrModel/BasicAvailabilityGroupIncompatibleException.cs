using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class BasicAvailabilityGroupIncompatibleException : HadrValidationErrorException
{
	public BasicAvailabilityGroupIncompatibleException(string reason)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.BasicAvailabilityGroupIncompatibleException, reason))
	{
	}
}
