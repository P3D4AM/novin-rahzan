using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class AvailabilityGroupQuorumValidationTaskException : HadrTaskBaseException
{
	public AvailabilityGroupQuorumValidationTaskException(string availabilityGroupName)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AvailabilityGroupQuorumValidatorException, availabilityGroupName))
	{
	}

	public AvailabilityGroupQuorumValidationTaskException(string availabilityGroupName, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.AvailabilityGroupQuorumValidatorException, availabilityGroupName), inner)
	{
	}
}
