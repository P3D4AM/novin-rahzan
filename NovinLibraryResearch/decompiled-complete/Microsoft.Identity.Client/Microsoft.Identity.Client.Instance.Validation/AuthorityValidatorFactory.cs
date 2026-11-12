using System;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Instance.Validation;

internal static class AuthorityValidatorFactory
{
	public static IAuthorityValidator Create(AuthorityInfo authorityInfo, IServiceBundle serviceBundle)
	{
		return authorityInfo.AuthorityType switch
		{
			AuthorityType.Adfs => new AdfsAuthorityValidator(serviceBundle), 
			AuthorityType.Aad => new AadAuthorityValidator(serviceBundle), 
			AuthorityType.B2C => new B2CAuthorityValidator(), 
			_ => throw new InvalidOperationException("Invalid AuthorityType"), 
		};
	}
}
