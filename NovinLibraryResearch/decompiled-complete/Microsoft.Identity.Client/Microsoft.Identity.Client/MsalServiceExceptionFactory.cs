using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client;

internal class MsalServiceExceptionFactory
{
	private static ISet<string> s_nonUiSubErrors = new HashSet<string>(new string[2] { "client_mismatch", "protection_policy_required" }, StringComparer.OrdinalIgnoreCase);

	internal static MsalServiceException FromHttpResponse(string errorCode, string errorMessage, HttpResponse httpResponse, Exception innerException = null)
	{
		MsalServiceException ex = null;
		OAuth2ResponseBase oAuth2ResponseBase = JsonHelper.TryToDeserializeFromJson<OAuth2ResponseBase>(httpResponse?.Body);
		if (IsInvalidGrant(oAuth2ResponseBase?.Error, oAuth2ResponseBase?.SubError) || IsInteractionRequired(oAuth2ResponseBase?.Error))
		{
			ex = new MsalUiRequiredException(errorCode, errorMessage, innerException);
		}
		if (string.Equals(oAuth2ResponseBase?.Error, "invalid_client", StringComparison.OrdinalIgnoreCase))
		{
			ex = new MsalServiceException("invalid_client", "A configuration issue is preventing authentication - check the error message from the server for details.You can modify the configuration in the application registration portal. See https://aka.ms/msal-net-invalid-client for details.  Original exception: " + oAuth2ResponseBase?.ErrorDescription, innerException);
		}
		if (ex == null)
		{
			ex = new MsalServiceException(errorCode, errorMessage, innerException);
		}
		ex.ResponseBody = httpResponse?.Body;
		ex.StatusCode = (int)(httpResponse?.StatusCode ?? ((HttpStatusCode)0));
		ex.Headers = httpResponse?.Headers;
		ex.Claims = oAuth2ResponseBase?.Claims;
		ex.CorrelationId = oAuth2ResponseBase?.CorrelationId;
		ex.SubError = oAuth2ResponseBase?.SubError;
		return ex;
	}

	internal static MsalServiceException FromBrokerResponse(string errorCode, string errorMessage, string subErrorCode, string correlationId, HttpResponse brokerHttpResponse)
	{
		MsalServiceException ex = null;
		if (IsInvalidGrant(errorCode, subErrorCode) || IsInteractionRequired(errorCode))
		{
			ex = new MsalUiRequiredException(errorCode, errorMessage);
		}
		if (string.Equals(errorCode, "invalid_client", StringComparison.OrdinalIgnoreCase))
		{
			ex = new MsalServiceException("invalid_client", "A configuration issue is preventing authentication - check the error message from the server for details.You can modify the configuration in the application registration portal. See https://aka.ms/msal-net-invalid-client for details.  Original exception: " + errorMessage);
		}
		if (ex == null)
		{
			ex = new MsalServiceException(errorCode, errorMessage);
		}
		if (brokerHttpResponse != null)
		{
			ex.ResponseBody = brokerHttpResponse.Body;
			ex.StatusCode = (int)brokerHttpResponse.StatusCode;
			ex.Headers = brokerHttpResponse.Headers;
		}
		ex.CorrelationId = correlationId;
		ex.SubError = subErrorCode;
		return ex;
	}

	private static bool IsInteractionRequired(string errorCode)
	{
		return string.Equals(errorCode, "interaction_required", StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsInvalidGrant(string errorCode, string subErrorCode)
	{
		if (string.Equals(errorCode, "invalid_grant", StringComparison.OrdinalIgnoreCase))
		{
			return IsInvalidGrantSubError(subErrorCode);
		}
		return false;
	}

	private static bool IsInvalidGrantSubError(string subError)
	{
		if (string.IsNullOrEmpty(subError))
		{
			return true;
		}
		return !s_nonUiSubErrors.Contains(subError);
	}
}
