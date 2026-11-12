using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class MaxHostedServicesException : HadrValidationBaseException
{
	public MaxHostedServicesException(string hostedServiceName, string currentHostedService)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.MaxHostedServicesException, hostedServiceName, currentHostedService))
	{
	}

	public MaxHostedServicesException(string hostedServiceName, string currentHostedService, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.MaxHostedServicesException, hostedServiceName, currentHostedService), inner)
	{
	}
}
