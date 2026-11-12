using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class EndpointAuthenticationValidationException : HadrValidationErrorException
{
	public EndpointAuthenticationValidationException(string endpointName, string authenticationType)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.EndpointAuthenticationValidatorException, endpointName))
	{
	}

	public EndpointAuthenticationValidationException(string endpointName, string authenticationType, Exception inner)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.EndpointAuthenticationValidatorException, endpointName, authenticationType), inner)
	{
	}
}
