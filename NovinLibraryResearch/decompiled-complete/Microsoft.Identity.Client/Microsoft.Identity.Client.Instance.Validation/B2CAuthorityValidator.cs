using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Instance.Validation;

internal class B2CAuthorityValidator : IAuthorityValidator
{
	public Task ValidateAuthorityAsync(AuthorityInfo authorityInfo, RequestContext requestContext)
	{
		return Task.FromResult(0);
	}
}
