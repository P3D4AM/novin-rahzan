using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenInvalidAlgorithmException : SecurityTokenValidationException
{
	[NonSerialized]
	private const string _Prefix = "Microsoft.IdentityModel.SecurityTokenInvalidAlgorithmException.";

	[NonSerialized]
	private const string _InvalidAlgorithmKey = "Microsoft.IdentityModel.SecurityTokenInvalidAlgorithmException.InvalidAlgorithm";

	public string InvalidAlgorithm { get; set; }

	public SecurityTokenInvalidAlgorithmException()
	{
	}

	public SecurityTokenInvalidAlgorithmException(string message)
		: base(message)
	{
	}

	public SecurityTokenInvalidAlgorithmException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenInvalidAlgorithmException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Name == "Microsoft.IdentityModel.SecurityTokenInvalidAlgorithmException.InvalidAlgorithm")
			{
				InvalidAlgorithm = info.GetString("Microsoft.IdentityModel.SecurityTokenInvalidAlgorithmException.InvalidAlgorithm");
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		if (!string.IsNullOrEmpty(InvalidAlgorithm))
		{
			info.AddValue("Microsoft.IdentityModel.SecurityTokenInvalidAlgorithmException.InvalidAlgorithm", InvalidAlgorithm);
		}
	}
}
