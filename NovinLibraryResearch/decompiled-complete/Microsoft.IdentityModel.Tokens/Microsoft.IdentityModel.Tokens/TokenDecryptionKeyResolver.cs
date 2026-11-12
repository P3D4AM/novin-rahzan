using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens;

public delegate IEnumerable<SecurityKey> TokenDecryptionKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);
