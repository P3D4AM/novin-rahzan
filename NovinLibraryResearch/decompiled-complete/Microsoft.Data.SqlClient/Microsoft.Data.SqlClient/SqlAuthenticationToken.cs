using System;
using System.Text;

namespace Microsoft.Data.SqlClient;

public class SqlAuthenticationToken
{
	public DateTimeOffset ExpiresOn { get; }

	public string AccessToken { get; }

	public SqlAuthenticationToken(string accessToken, DateTimeOffset expiresOn)
	{
		if (string.IsNullOrEmpty(accessToken))
		{
			throw SQL.ParameterCannotBeEmpty("AccessToken");
		}
		AccessToken = accessToken;
		ExpiresOn = expiresOn;
	}

	internal SqlAuthenticationToken(byte[] accessToken, DateTimeOffset expiresOn)
		: this(AccessTokenStringFromBytes(accessToken), expiresOn)
	{
	}

	internal SqlFedAuthToken ToSqlFedAuthToken()
	{
		byte[] array = AccessTokenBytesFromString(AccessToken);
		return new SqlFedAuthToken
		{
			accessToken = array,
			dataLen = (uint)array.Length,
			expirationFileTime = ExpiresOn.ToFileTime()
		};
	}

	internal static string AccessTokenStringFromBytes(byte[] bytes)
	{
		return Encoding.Unicode.GetString(bytes);
	}

	internal static byte[] AccessTokenBytesFromString(string token)
	{
		return Encoding.Unicode.GetBytes(token);
	}
}
