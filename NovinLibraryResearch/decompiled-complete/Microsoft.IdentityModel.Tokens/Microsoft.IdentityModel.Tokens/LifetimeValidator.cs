using System;

namespace Microsoft.IdentityModel.Tokens;

public delegate bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters);
