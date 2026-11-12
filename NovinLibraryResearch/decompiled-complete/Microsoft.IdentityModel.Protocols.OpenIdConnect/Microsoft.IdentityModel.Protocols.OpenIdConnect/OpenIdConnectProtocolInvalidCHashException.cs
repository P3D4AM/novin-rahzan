using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

[Serializable]
public class OpenIdConnectProtocolInvalidCHashException : OpenIdConnectProtocolException
{
	public OpenIdConnectProtocolInvalidCHashException()
	{
	}

	public OpenIdConnectProtocolInvalidCHashException(string message)
		: base(message)
	{
	}

	public OpenIdConnectProtocolInvalidCHashException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected OpenIdConnectProtocolInvalidCHashException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
