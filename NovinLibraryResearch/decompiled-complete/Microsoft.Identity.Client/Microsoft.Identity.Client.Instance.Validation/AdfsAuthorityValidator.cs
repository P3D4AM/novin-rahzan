using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Instance.Validation;

internal class AdfsAuthorityValidator : IAuthorityValidator
{
	private readonly IServiceBundle _serviceBundle;

	public AdfsAuthorityValidator(IServiceBundle serviceBundle)
	{
		_serviceBundle = serviceBundle;
	}

	public async Task ValidateAuthorityAsync(AuthorityInfo authorityInfo, RequestContext requestContext)
	{
		if (authorityInfo.ValidateAuthority)
		{
			string resource = string.Format(CultureInfo.InvariantCulture, "https://{0}", authorityInfo.Host);
			string uriString = Constants.FormatAdfsWebFingerUrl(authorityInfo.Host, resource);
			HttpResponse httpResponse = await _serviceBundle.HttpManager.SendGetAsync(new Uri(uriString), null, requestContext.Logger).ConfigureAwait(continueOnCapturedContext: false);
			if (httpResponse.StatusCode != HttpStatusCode.OK)
			{
				throw MsalServiceExceptionFactory.FromHttpResponse("invalid_authority", "Authority validation failed", httpResponse);
			}
			if (OAuth2Client.CreateResponse<AdfsWebFingerResponse>(httpResponse, requestContext).Links.FirstOrDefault((LinksList a) => a.Rel.Equals("http://schemas.microsoft.com/rel/trusted-realm", StringComparison.OrdinalIgnoreCase) && a.Href.Equals(resource)) == null)
			{
				throw new MsalClientException("invalid_authority", "invalid authority while getting the open id config endpoint");
			}
		}
	}
}
