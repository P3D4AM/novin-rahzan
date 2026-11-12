namespace Microsoft.IdentityModel.Tokens;

public delegate bool AlgorithmValidator(string algorithm, SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters);
