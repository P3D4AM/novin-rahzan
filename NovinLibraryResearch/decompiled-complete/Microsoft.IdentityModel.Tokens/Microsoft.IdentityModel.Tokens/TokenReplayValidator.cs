using System;

namespace Microsoft.IdentityModel.Tokens;

public delegate bool TokenReplayValidator(DateTime? expirationTime, string securityToken, TokenValidationParameters validationParameters);
