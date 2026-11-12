using System;

namespace Microsoft.IdentityModel.Tokens;

[Obsolete("The 'TokenContext' property is obsolete. Please use 'CallContext' instead.")]
public class TokenContext : CallContext
{
	public TokenContext()
	{
	}

	public TokenContext(Guid activityId)
		: base(activityId)
	{
	}
}
