namespace Microsoft.IdentityModel.Tokens;

public delegate string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters);
