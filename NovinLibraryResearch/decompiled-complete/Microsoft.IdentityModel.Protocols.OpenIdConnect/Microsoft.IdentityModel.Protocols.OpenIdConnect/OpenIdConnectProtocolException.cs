using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

[Serializable]
public class OpenIdConnectProtocolException : Exception
{
	public OpenIdConnectProtocolException()
	{
	}

	public OpenIdConnectProtocolException(string message)
		: base(message)
	{
	}

	public OpenIdConnectProtocolException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected OpenIdConnectProtocolException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
