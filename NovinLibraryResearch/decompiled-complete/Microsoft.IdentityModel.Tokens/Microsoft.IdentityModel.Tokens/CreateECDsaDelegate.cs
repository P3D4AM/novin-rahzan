using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Tokens;

internal delegate ECDsa CreateECDsaDelegate(JsonWebKey jsonWebKey, bool usePrivateKey);
