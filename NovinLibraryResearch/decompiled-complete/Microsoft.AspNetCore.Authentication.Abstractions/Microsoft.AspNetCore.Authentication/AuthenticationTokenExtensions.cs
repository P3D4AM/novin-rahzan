using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication;

public static class AuthenticationTokenExtensions
{
	private static string TokenNamesKey = ".TokenNames";

	private static string TokenKeyPrefix = ".Token.";

	public static void StoreTokens(this AuthenticationProperties properties, IEnumerable<AuthenticationToken> tokens)
	{
		if (properties == null)
		{
			throw new ArgumentNullException("properties");
		}
		if (tokens == null)
		{
			throw new ArgumentNullException("tokens");
		}
		foreach (AuthenticationToken token in properties.GetTokens())
		{
			properties.Items.Remove(TokenKeyPrefix + token.Name);
		}
		properties.Items.Remove(TokenNamesKey);
		List<string> list = new List<string>();
		foreach (AuthenticationToken token2 in tokens)
		{
			list.Add(token2.Name);
			properties.Items[TokenKeyPrefix + token2.Name] = token2.Value;
		}
		if (list.Count > 0)
		{
			properties.Items[TokenNamesKey] = string.Join(";", list.ToArray());
		}
	}

	public static string GetTokenValue(this AuthenticationProperties properties, string tokenName)
	{
		if (properties == null)
		{
			throw new ArgumentNullException("properties");
		}
		if (tokenName == null)
		{
			throw new ArgumentNullException("tokenName");
		}
		string key = TokenKeyPrefix + tokenName;
		if (!properties.Items.ContainsKey(key))
		{
			return null;
		}
		return properties.Items[key];
	}

	public static bool UpdateTokenValue(this AuthenticationProperties properties, string tokenName, string tokenValue)
	{
		if (properties == null)
		{
			throw new ArgumentNullException("properties");
		}
		if (tokenName == null)
		{
			throw new ArgumentNullException("tokenName");
		}
		string key = TokenKeyPrefix + tokenName;
		if (!properties.Items.ContainsKey(key))
		{
			return false;
		}
		properties.Items[key] = tokenValue;
		return true;
	}

	public static IEnumerable<AuthenticationToken> GetTokens(this AuthenticationProperties properties)
	{
		if (properties == null)
		{
			throw new ArgumentNullException("properties");
		}
		List<AuthenticationToken> list = new List<AuthenticationToken>();
		if (properties.Items.ContainsKey(TokenNamesKey))
		{
			string[] array = properties.Items[TokenNamesKey].Split(new char[1] { ';' });
			foreach (string text in array)
			{
				string tokenValue = properties.GetTokenValue(text);
				if (tokenValue != null)
				{
					list.Add(new AuthenticationToken
					{
						Name = text,
						Value = tokenValue
					});
				}
			}
		}
		return list;
	}

	public static Task<string> GetTokenAsync(this IAuthenticationService auth, HttpContext context, string tokenName)
	{
		return auth.GetTokenAsync(context, null, tokenName);
	}

	public static async Task<string> GetTokenAsync(this IAuthenticationService auth, HttpContext context, string scheme, string tokenName)
	{
		if (auth == null)
		{
			throw new ArgumentNullException("auth");
		}
		if (tokenName == null)
		{
			throw new ArgumentNullException("tokenName");
		}
		return (await auth.AuthenticateAsync(context, scheme))?.Properties?.GetTokenValue(tokenName);
	}
}
