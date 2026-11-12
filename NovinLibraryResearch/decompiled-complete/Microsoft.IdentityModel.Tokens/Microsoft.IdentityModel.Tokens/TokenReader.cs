namespace Microsoft.IdentityModel.Tokens;

public delegate SecurityToken TokenReader(string token, TokenValidationParameters validationParameters);
