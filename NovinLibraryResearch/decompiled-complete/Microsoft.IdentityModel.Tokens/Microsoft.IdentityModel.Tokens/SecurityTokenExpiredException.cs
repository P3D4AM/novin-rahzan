using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenExpiredException : SecurityTokenValidationException
{
	[NonSerialized]
	private const string _Prefix = "Microsoft.IdentityModel.SecurityTokenExpiredException.";

	[NonSerialized]
	private const string _ExpiresKey = "Microsoft.IdentityModel.SecurityTokenExpiredException.Expires";

	public DateTime Expires { get; set; }

	public SecurityTokenExpiredException()
		: base("SecurityToken has Expired")
	{
	}

	public SecurityTokenExpiredException(string message)
		: base(message)
	{
	}

	public SecurityTokenExpiredException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityTokenExpiredException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Name == "Microsoft.IdentityModel.SecurityTokenExpiredException.Expires")
			{
				Expires = (DateTime)info.GetValue("Microsoft.IdentityModel.SecurityTokenExpiredException.Expires", typeof(DateTime));
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("Microsoft.IdentityModel.SecurityTokenExpiredException.Expires", Expires);
	}
}
