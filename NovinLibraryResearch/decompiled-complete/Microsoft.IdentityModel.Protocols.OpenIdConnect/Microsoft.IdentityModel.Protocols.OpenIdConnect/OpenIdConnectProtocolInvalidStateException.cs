using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

[Serializable]
public class OpenIdConnectProtocolInvalidStateException : OpenIdConnectProtocolException
{
	public OpenIdConnectProtocolInvalidStateException()
	{
	}

	public OpenIdConnectProtocolInvalidStateException(string message)
		: base(message)
	{
	}

	public OpenIdConnectProtocolInvalidStateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected OpenIdConnectProtocolInvalidStateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
