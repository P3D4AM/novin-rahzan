using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens;

public delegate bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters);
