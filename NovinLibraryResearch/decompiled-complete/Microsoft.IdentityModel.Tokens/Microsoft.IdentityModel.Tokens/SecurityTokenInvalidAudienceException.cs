using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenInvalidAudienceException : SecurityTokenValidationException
{
	[NonSerialized]
	private const string _Prefix = "Microsoft.IdentityModel.SecurityTokenInvalidAudienceException.";

	[NonSerialized]
	private const string _InvalidAudienceKey = "Microsoft.IdentityModel.SecurityTokenInvalidAudienceException.InvalidAudience";

	public string InvalidAudience { get; set; }

	public SecurityTokenInvalidAudienceException()
	{
	}

	public SecurityTokenInvalidAudienceException(string message)
		: base(message)
	{
	}

	public SecurityTokenInvalidAudienceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenInvalidAudienceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Name == "Microsoft.IdentityModel.SecurityTokenInvalidAudienceException.InvalidAudience")
			{
				InvalidAudience = info.GetString("Microsoft.IdentityModel.SecurityTokenInvalidAudienceException.InvalidAudience");
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		if (!string.IsNullOrEmpty(InvalidAudience))
		{
			info.AddValue("Microsoft.IdentityModel.SecurityTokenInvalidAudienceException.InvalidAudience", InvalidAudience);
		}
	}
}
