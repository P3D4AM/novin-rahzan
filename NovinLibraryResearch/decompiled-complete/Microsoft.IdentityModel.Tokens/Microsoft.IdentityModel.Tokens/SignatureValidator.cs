namespace Microsoft.IdentityModel.Tokens;

public delegate SecurityToken SignatureValidator(string token, TokenValidationParameters validationParameters);
