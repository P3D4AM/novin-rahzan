using System;
using System.Globalization;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.TelemetryCore.Internal.Events;

internal class ApiEvent : EventBase
{
	public enum ApiIds
	{
		None = 0,
		AcquireTokenByAuthorizationCode = 1000,
		AcquireTokenByRefreshToken = 1001,
		AcquireTokenByIntegratedWindowsAuth = 1002,
		AcquireTokenByUsernamePassword = 1003,
		AcquireTokenForClient = 1004,
		AcquireTokenInteractive = 1005,
		AcquireTokenOnBehalfOf = 1006,
		AcquireTokenSilent = 1007,
		AcquireTokenByDeviceCode = 1008,
		GetAuthorizationRequestUrl = 1009,
		GetAccounts = 1010,
		GetAccountById = 1011,
		GetAccountsByUserFlow = 1012,
		RemoveAccount = 1013
	}

	public const string AuthorityKey = "msal.authority";

	public const string AuthorityTypeKey = "msal.authority_type";

	public const string PromptKey = "msal.ui_behavior";

	public const string TenantIdKey = "msal.tenant_id";

	public const string UserIdKey = "msal.user_id";

	public const string WasSuccessfulKey = "msal.was_successful";

	public const string IsConfidentialClientKey = "msal.is_confidential_client";

	public const string ApiErrorCodeKey = "msal.api_error_code";

	public const string LoginHintKey = "msal.login_hint";

	public const string IsAccessTokenCacheHitKey = "msal.at_cache_hit";

	public const string RegionDiscoveredKey = "msal.region_discovered";

	private readonly ICryptographyManager _cryptographyManager;

	private readonly ICoreLogger _logger;

	public ApiTelemetryId ApiTelemId
	{
		set
		{
			int num = (int)value;
			base["msal.api_telem_id"] = num.ToString(CultureInfo.InvariantCulture);
		}
	}

	public ApiIds ApiId
	{
		get
		{
			return (ApiIds)Enum.Parse(typeof(ApiIds), base["msal.api_id"]);
		}
		set
		{
			int num = (int)value;
			base["msal.api_id"] = num.ToString(CultureInfo.InvariantCulture);
		}
	}

	public string ApiIdString
	{
		get
		{
			if (!ContainsKey("msal.api_id"))
			{
				return null;
			}
			return base["msal.api_id"];
		}
	}

	public Uri Authority
	{
		set
		{
			base["msal.authority"] = EventBase.ScrubTenant(value)?.ToLowerInvariant();
		}
	}

	public string AuthorityType
	{
		set
		{
			base["msal.authority_type"] = value?.ToLowerInvariant();
		}
	}

	public string Prompt
	{
		set
		{
			base["msal.ui_behavior"] = value?.ToLowerInvariant();
		}
	}

	public string TenantId
	{
		set
		{
			base["msal.tenant_id"] = ((value != null && _logger.PiiLoggingEnabled) ? HashPersonalIdentifier(_cryptographyManager, value) : null);
		}
	}

	public string AccountId
	{
		set
		{
			base["msal.user_id"] = ((value != null && _logger.PiiLoggingEnabled) ? HashPersonalIdentifier(_cryptographyManager, value) : null);
		}
	}

	public bool WasSuccessful
	{
		get
		{
			return base["msal.was_successful"] == true.ToString().ToLowerInvariant();
		}
		set
		{
			base["msal.was_successful"] = value.ToString().ToLowerInvariant();
		}
	}

	public bool IsConfidentialClient
	{
		set
		{
			base["msal.is_confidential_client"] = value.ToString().ToLowerInvariant();
		}
	}

	public bool IsAccessTokenCacheHit
	{
		get
		{
			if (!ContainsKey("msal.at_cache_hit"))
			{
				return false;
			}
			return base["msal.at_cache_hit"] == true.ToString().ToLowerInvariant();
		}
		set
		{
			base["msal.at_cache_hit"] = value.ToString().ToLowerInvariant();
		}
	}

	public string ApiErrorCode
	{
		get
		{
			if (!ContainsKey("msal.api_error_code"))
			{
				return null;
			}
			return base["msal.api_error_code"];
		}
		set
		{
			base["msal.api_error_code"] = value;
		}
	}

	public string LoginHint
	{
		set
		{
			base["msal.login_hint"] = ((value != null && _logger.PiiLoggingEnabled) ? HashPersonalIdentifier(_cryptographyManager, value) : null);
		}
	}

	public string RegionDiscovered
	{
		get
		{
			if (!ContainsKey("msal.region_discovered"))
			{
				return null;
			}
			return base["msal.region_discovered"];
		}
		set
		{
			base["msal.region_discovered"] = value;
		}
	}

	public ApiEvent(ICoreLogger logger, ICryptographyManager cryptographyManager, string correlationId)
		: base("msal.api_event", correlationId)
	{
		_logger = logger;
		_cryptographyManager = cryptographyManager;
	}
}
