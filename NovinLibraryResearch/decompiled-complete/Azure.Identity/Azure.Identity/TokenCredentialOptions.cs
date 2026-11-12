using System;
using Azure.Core;

namespace Azure.Identity;

public class TokenCredentialOptions : ClientOptions
{
	private Uri _authorityHost;

	public Uri AuthorityHost
	{
		get
		{
			return _authorityHost ?? AzureAuthorityHosts.GetDefault();
		}
		set
		{
			_authorityHost = Validations.ValidateAuthorityHost(value);
		}
	}
}
