using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

internal class TokenUtilities
{
	internal const string Json = "JSON";

	internal const string JsonArray = "JSON_ARRAY";

	internal const string JsonNull = "JSON_NULL";

	internal static IDictionary<string, object> CreateDictionaryFromClaims(IEnumerable<Claim> claims)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (claims == null)
		{
			return dictionary;
		}
		foreach (Claim claim in claims)
		{
			if (claim == null)
			{
				continue;
			}
			string type = claim.Type;
			object obj = (claim.ValueType.Equals("http://www.w3.org/2001/XMLSchema#string", StringComparison.Ordinal) ? claim.Value : GetClaimValueUsingValueType(claim));
			if (dictionary.TryGetValue(type, out var value))
			{
				IList<object> list = value as IList<object>;
				if (list == null)
				{
					list = new List<object>();
					list.Add(value);
					dictionary[type] = list;
				}
				list.Add(obj);
			}
			else
			{
				dictionary[type] = obj;
			}
		}
		return dictionary;
	}

	internal static object GetClaimValueUsingValueType(Claim claim)
	{
		if (claim.ValueType == "http://www.w3.org/2001/XMLSchema#string")
		{
			return claim.Value;
		}
		if (claim.ValueType == "http://www.w3.org/2001/XMLSchema#boolean" && bool.TryParse(claim.Value, out var result))
		{
			return result;
		}
		if (claim.ValueType == "http://www.w3.org/2001/XMLSchema#double" && double.TryParse(claim.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result2))
		{
			return result2;
		}
		if ((claim.ValueType == "http://www.w3.org/2001/XMLSchema#integer" || claim.ValueType == "http://www.w3.org/2001/XMLSchema#integer32") && int.TryParse(claim.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result3))
		{
			return result3;
		}
		if (claim.ValueType == "http://www.w3.org/2001/XMLSchema#integer64" && long.TryParse(claim.Value, out var result4))
		{
			return result4;
		}
		if (claim.ValueType == "http://www.w3.org/2001/XMLSchema#dateTime" && DateTime.TryParse(claim.Value, out var result5))
		{
			return result5;
		}
		if (claim.ValueType == "JSON")
		{
			return JObject.Parse(claim.Value);
		}
		if (claim.ValueType == "JSON_ARRAY")
		{
			return JArray.Parse(claim.Value);
		}
		if (claim.ValueType == "JSON_NULL")
		{
			return string.Empty;
		}
		return claim.Value;
	}

	internal static IEnumerable<SecurityKey> GetAllSigningKeys(TokenValidationParameters validationParameters)
	{
		LogHelper.LogInformation("IDX10243: Reading issuer signing keys from validation parameters.");
		if (validationParameters.IssuerSigningKey != null)
		{
			yield return validationParameters.IssuerSigningKey;
		}
		if (validationParameters.IssuerSigningKeys == null)
		{
			yield break;
		}
		foreach (SecurityKey issuerSigningKey in validationParameters.IssuerSigningKeys)
		{
			yield return issuerSigningKey;
		}
	}

	internal static IEnumerable<Claim> MergeClaims(IEnumerable<Claim> claims, IEnumerable<Claim> subjectClaims)
	{
		if (claims == null)
		{
			return subjectClaims;
		}
		if (subjectClaims == null)
		{
			return claims;
		}
		List<Claim> list = claims.ToList();
		foreach (Claim claim in subjectClaims)
		{
			if (!claims.Any((Claim i) => i.Type == claim.Type))
			{
				list.Add(claim);
			}
		}
		return list;
	}
}
