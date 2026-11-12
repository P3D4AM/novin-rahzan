using System;

namespace Microsoft.AspNetCore.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute, IAuthorizeData
{
	public string Policy { get; set; }

	public string Roles { get; set; }

	public string AuthenticationSchemes { get; set; }

	[Obsolete("Use AuthenticationSchemes instead.", false)]
	public string ActiveAuthenticationSchemes
	{
		get
		{
			return AuthenticationSchemes;
		}
		set
		{
			AuthenticationSchemes = value;
		}
	}

	public AuthorizeAttribute()
	{
	}

	public AuthorizeAttribute(string policy)
	{
		Policy = policy;
	}
}
