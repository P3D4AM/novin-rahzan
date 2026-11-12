using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens;

public interface ISecurityTokenValidator
{
	bool CanValidateToken { get; }

	int MaximumTokenSizeInBytes { get; set; }

	bool CanReadToken(string securityToken);

	ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken);
}
