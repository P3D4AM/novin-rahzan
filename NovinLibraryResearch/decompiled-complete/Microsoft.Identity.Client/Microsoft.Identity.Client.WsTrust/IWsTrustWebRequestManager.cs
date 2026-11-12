using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.WsTrust;

internal interface IWsTrustWebRequestManager
{
	Task<MexDocument> GetMexDocumentAsync(string federationMetadataUrl, RequestContext requestContext);

	Task<WsTrustResponse> GetWsTrustResponseAsync(WsTrustEndpoint wsTrustEndpoint, string wsTrustRequest, RequestContext requestContext);

	Task<UserRealmDiscoveryResponse> GetUserRealmAsync(string userRealmUriPrefix, string userName, RequestContext requestContext);
}
