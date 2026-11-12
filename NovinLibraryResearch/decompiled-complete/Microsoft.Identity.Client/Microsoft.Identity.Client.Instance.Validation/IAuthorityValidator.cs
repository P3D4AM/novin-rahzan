using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Instance.Validation;

internal interface IAuthorityValidator
{
	Task ValidateAuthorityAsync(AuthorityInfo authorityInfo, RequestContext requestContext);
}
