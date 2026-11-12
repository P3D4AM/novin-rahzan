using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public interface IClaimsTransformation
{
	Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal);
}
