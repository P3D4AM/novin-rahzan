using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenInvalidTypeException : SecurityTokenValidationException
{
	[NonSerialized]
	private const string _Prefix = "Microsoft.IdentityModel.SecurityTokenInvalidTypeException.";

	[NonSerialized]
	private const string _InvalidTypeKey = "Microsoft.IdentityModel.SecurityTokenInvalidTypeException.InvalidType";

	public string InvalidType { get; set; }

	public SecurityTokenInvalidTypeException()
	{
	}

	public SecurityTokenInvalidTypeException(string message)
		: base(message)
	{
	}

	public SecurityTokenInvalidTypeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenInvalidTypeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Name == "Microsoft.IdentityModel.SecurityTokenInvalidTypeException.InvalidType")
			{
				InvalidType = info.GetString("Microsoft.IdentityModel.SecurityTokenInvalidTypeException.InvalidType");
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		if (!string.IsNullOrEmpty(InvalidType))
		{
			info.AddValue("Microsoft.IdentityModel.SecurityTokenInvalidTypeException.InvalidType", InvalidType);
		}
	}
}
