using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenNotYetValidException : SecurityTokenValidationException
{
	[NonSerialized]
	private const string _Prefix = "Microsoft.IdentityModel.SecurityTokenNotYetValidException.";

	[NonSerialized]
	private const string _NotBeforeKey = "Microsoft.IdentityModel.SecurityTokenNotYetValidException.NotBefore";

	public DateTime NotBefore { get; set; }

	public SecurityTokenNotYetValidException()
		: base("SecurityToken is not yet valid")
	{
	}

	public SecurityTokenNotYetValidException(string message)
		: base(message)
	{
	}

	public SecurityTokenNotYetValidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityTokenNotYetValidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Name == "Microsoft.IdentityModel.SecurityTokenNotYetValidException.NotBefore")
			{
				NotBefore = (DateTime)info.GetValue("Microsoft.IdentityModel.SecurityTokenNotYetValidException.NotBefore", typeof(DateTime));
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("Microsoft.IdentityModel.SecurityTokenNotYetValidException.NotBefore", NotBefore);
	}
}
