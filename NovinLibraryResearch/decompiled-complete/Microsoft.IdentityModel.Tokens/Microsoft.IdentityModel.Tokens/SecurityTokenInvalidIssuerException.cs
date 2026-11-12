using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenInvalidIssuerException : SecurityTokenValidationException
{
	[NonSerialized]
	private const string _Prefix = "Microsoft.IdentityModel.SecurityTokenInvalidIssuerException.";

	[NonSerialized]
	private const string _InvalidIssuerKey = "Microsoft.IdentityModel.SecurityTokenInvalidIssuerException.InvalidIssuer";

	public string InvalidIssuer { get; set; }

	public SecurityTokenInvalidIssuerException()
	{
	}

	public SecurityTokenInvalidIssuerException(string message)
		: base(message)
	{
	}

	public SecurityTokenInvalidIssuerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenInvalidIssuerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Name == "Microsoft.IdentityModel.SecurityTokenInvalidIssuerException.InvalidIssuer")
			{
				InvalidIssuer = info.GetString("Microsoft.IdentityModel.SecurityTokenInvalidIssuerException.InvalidIssuer");
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		if (!string.IsNullOrEmpty(InvalidIssuer))
		{
			info.AddValue("Microsoft.IdentityModel.SecurityTokenInvalidIssuerException.InvalidIssuer", InvalidIssuer);
		}
	}
}
