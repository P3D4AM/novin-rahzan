using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenInvalidLifetimeException : SecurityTokenValidationException
{
	[NonSerialized]
	private const string _Prefix = "Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.";

	[NonSerialized]
	private const string _NotBeforeKey = "Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.NotBefore";

	[NonSerialized]
	private const string _ExpiresKey = "Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.Expires";

	public DateTime? NotBefore { get; set; }

	public DateTime? Expires { get; set; }

	public SecurityTokenInvalidLifetimeException()
	{
	}

	public SecurityTokenInvalidLifetimeException(string message)
		: base(message)
	{
	}

	public SecurityTokenInvalidLifetimeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenInvalidLifetimeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string name = enumerator.Name;
			if (!(name == "Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.NotBefore"))
			{
				if (name == "Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.Expires")
				{
					Expires = (DateTime)info.GetValue("Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.Expires", typeof(DateTime));
				}
			}
			else
			{
				NotBefore = (DateTime)info.GetValue("Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.NotBefore", typeof(DateTime));
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		if (NotBefore.HasValue)
		{
			info.AddValue("Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.NotBefore", NotBefore.Value);
		}
		if (Expires.HasValue)
		{
			info.AddValue("Microsoft.IdentityModel.SecurityTokenInvalidLifetimeException.Expires", Expires.Value);
		}
	}
}
