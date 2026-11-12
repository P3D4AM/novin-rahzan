using System;

namespace Azure.Core;

public struct AccessToken
{
	public string Token { get; }

	public DateTimeOffset ExpiresOn { get; }

	public AccessToken(string accessToken, DateTimeOffset expiresOn)
	{
		Token = accessToken;
		ExpiresOn = expiresOn;
	}

	public override bool Equals(object? obj)
	{
		if (obj is AccessToken accessToken)
		{
			if (accessToken.ExpiresOn == ExpiresOn)
			{
				return accessToken.Token == Token;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Token.GetHashCode() ^ ExpiresOn.GetHashCode();
	}
}
