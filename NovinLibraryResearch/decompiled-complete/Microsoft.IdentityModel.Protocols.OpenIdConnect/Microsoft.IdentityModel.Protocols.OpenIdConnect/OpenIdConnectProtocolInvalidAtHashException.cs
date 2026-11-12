using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

[Serializable]
public class OpenIdConnectProtocolInvalidAtHashException : OpenIdConnectProtocolException
{
	public OpenIdConnectProtocolInvalidAtHashException()
	{
	}

	public OpenIdConnectProtocolInvalidAtHashException(string message)
		: base(message)
	{
	}

	public OpenIdConnectProtocolInvalidAtHashException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected OpenIdConnectProtocolInvalidAtHashException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
