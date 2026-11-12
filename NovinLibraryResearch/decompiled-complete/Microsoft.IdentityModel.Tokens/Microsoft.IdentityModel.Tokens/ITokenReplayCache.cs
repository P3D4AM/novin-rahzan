using System;

namespace Microsoft.IdentityModel.Tokens;

public interface ITokenReplayCache
{
	bool TryAdd(string securityToken, DateTime expiresOn);

	bool TryFind(string securityToken);
}
