using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Instance.Validation;

internal class AadAuthorityValidator : IAuthorityValidator
{
	private readonly IServiceBundle _serviceBundle;

	public AadAuthorityValidator(IServiceBundle serviceBundle)
	{
		_serviceBundle = serviceBundle;
	}

	public async Task ValidateAuthorityAsync(AuthorityInfo authorityInfo, RequestContext requestContext)
	{
		Uri uri = new Uri(authorityInfo.CanonicalAuthority);
		if (authorityInfo.ValidateAuthority && !KnownMetadataProvider.IsKnownEnvironment(uri.Host))
		{
			await _serviceBundle.InstanceDiscoveryManager.GetMetadataEntryAsync(authorityInfo.CanonicalAuthority, requestContext).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
