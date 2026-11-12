using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AvailabilityModeIncompatibleException : HadrValidationErrorException
{
	public AvailabilityModeIncompatibleException()
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AvailabilityModeCompatibilityWarning))
	{
	}
}
