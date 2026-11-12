using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.UI;

[JsonObject]
[Preserve(AllMembers = true)]
internal class AuthorizationResult
{
	public AuthorizationStatus Status { get; private set; }

	[JsonProperty]
	public string Code { get; set; }

	[JsonProperty]
	public string Error { get; set; }

	[JsonProperty]
	public string ErrorDescription { get; set; }

	[JsonProperty]
	public string CloudInstanceHost { get; set; }

	public string State { get; set; }

	public static AuthorizationResult FromUri(string webAuthenticationResult)
	{
		if (string.IsNullOrWhiteSpace(webAuthenticationResult))
		{
			return FromStatus(AuthorizationStatus.UnknownError, "authentication_failed", "The authorization server returned an invalid response");
		}
		string query = new Uri(webAuthenticationResult).Query;
		if (string.IsNullOrWhiteSpace(query))
		{
			return FromStatus(AuthorizationStatus.UnknownError, "authentication_failed", "The authorization server returned an invalid response");
		}
		Dictionary<string, string> dictionary = CoreHelpers.ParseKeyValueList(query.Substring(1), '&', urlDecode: true, null);
		if (dictionary.ContainsKey("error"))
		{
			return FromStatus(AuthorizationStatus.ProtocolError, dictionary["error"], dictionary.ContainsKey("error_description") ? dictionary["error_description"] : null);
		}
		AuthorizationResult authorizationResult = new AuthorizationResult();
		authorizationResult.Status = AuthorizationStatus.Success;
		if (dictionary.ContainsKey("state"))
		{
			authorizationResult.State = dictionary["state"];
		}
		if (dictionary.ContainsKey("cloud_instance_host_name"))
		{
			authorizationResult.CloudInstanceHost = dictionary["cloud_instance_host_name"];
		}
		if (dictionary.ContainsKey("code"))
		{
			authorizationResult.Code = dictionary["code"];
		}
		else
		{
			if (!webAuthenticationResult.StartsWith("msauth://", StringComparison.OrdinalIgnoreCase))
			{
				return FromStatus(AuthorizationStatus.UnknownError, "authentication_failed", "The authorization server returned an invalid response");
			}
			authorizationResult.Code = webAuthenticationResult;
		}
		return authorizationResult;
	}

	internal static AuthorizationResult FromStatus(AuthorizationStatus status)
	{
		if (status == AuthorizationStatus.Success)
		{
			throw new InvalidOperationException("Use the FromUri builder");
		}
		AuthorizationResult authorizationResult = new AuthorizationResult
		{
			Status = status
		};
		switch (status)
		{
		case AuthorizationStatus.UserCancel:
			authorizationResult.Error = "authentication_canceled";
			authorizationResult.ErrorDescription = "User canceled authentication.";
			break;
		case AuthorizationStatus.UnknownError:
			authorizationResult.Error = "unknown_error";
			authorizationResult.ErrorDescription = "Unknown error";
			break;
		}
		return authorizationResult;
	}

	public static AuthorizationResult FromStatus(AuthorizationStatus status, string error, string errorDescription)
	{
		return new AuthorizationResult
		{
			Status = status,
			Error = error,
			ErrorDescription = errorDescription
		};
	}
}
