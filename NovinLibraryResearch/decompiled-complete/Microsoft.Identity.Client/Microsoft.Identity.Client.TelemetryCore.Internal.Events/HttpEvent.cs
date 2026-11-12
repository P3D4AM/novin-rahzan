using System;
using System.Globalization;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.TelemetryCore.Internal.Events;

internal class HttpEvent : EventBase
{
	public const string HttpPathKey = "msal.http_path";

	public const string UserAgentKey = "msal.user_agent";

	public const string QueryParametersKey = "msal.query_parameters";

	public const string ApiVersionKey = "msal.api_version";

	public const string ResponseCodeKey = "msal.response_code";

	public const string OauthErrorCodeKey = "msal.oauth_error_code";

	public const string HttpMethodKey = "msal.http_method";

	public const string RequestIdHeaderKey = "msal.request_id_header";

	public const string TokenAgeKey = "msal.token_age";

	public const string SpeInfoKey = "msal.spe_info";

	public const string ServerErrorCodeKey = "msal.server_error_code";

	public const string ServerSubErrorCodeKey = "msal.server_sub_error_code";

	public Uri HttpPath
	{
		set
		{
			base["msal.http_path"] = EventBase.ScrubTenant(value);
		}
	}

	public string UserAgent
	{
		set
		{
			base["msal.user_agent"] = value;
		}
	}

	public string QueryParams
	{
		set
		{
			base["msal.query_parameters"] = string.Join("&", CoreHelpers.ParseKeyValueList(value, '&', urlDecode: false, lowercaseKeys: true, null).Keys);
		}
	}

	public string ApiVersion
	{
		set
		{
			base["msal.api_version"] = value?.ToLowerInvariant();
		}
	}

	public int HttpResponseStatus
	{
		set
		{
			base["msal.response_code"] = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	public string OauthErrorCode
	{
		set
		{
			base["msal.oauth_error_code"] = value;
		}
	}

	public string HttpMethod
	{
		set
		{
			base["msal.http_method"] = value;
		}
	}

	public string RequestIdHeader
	{
		set
		{
			base["msal.request_id_header"] = value;
		}
	}

	public string TokenAge
	{
		set
		{
			base["msal.token_age"] = value;
		}
	}

	public string SpeInfo
	{
		set
		{
			base["msal.spe_info"] = value;
		}
	}

	public string ServerErrorCode
	{
		set
		{
			base["msal.server_error_code"] = value;
		}
	}

	public string ServerSubErrorCode
	{
		set
		{
			base["msal.server_sub_error_code"] = value;
		}
	}

	public HttpEvent(string correlationId)
		: base("msal.http_event", correlationId)
	{
	}
}
