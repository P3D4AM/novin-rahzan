using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

[Serializable]
public class OpenIdConnectProtocolInvalidNonceException : OpenIdConnectProtocolException
{
	public OpenIdConnectProtocolInvalidNonceException()
	{
	}

	public OpenIdConnectProtocolInvalidNonceException(string message)
		: base(message)
	{
	}

	public OpenIdConnectProtocolInvalidNonceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected OpenIdConnectProtocolInvalidNonceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
