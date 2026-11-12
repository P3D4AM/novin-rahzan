using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

public delegate void IdTokenValidator(JwtSecurityToken idToken, OpenIdConnectProtocolValidationContext context);
