using System;
using System.Globalization;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class MatsConverter
{
	public static string AsString(AccountType accountType)
	{
		return accountType switch
		{
			AccountType.MSA => "msa", 
			AccountType.AAD => "aad", 
			AccountType.B2C => "b2c", 
			_ => "unknown", 
		};
	}

	public static string AsString(ActionType actionType)
	{
		return actionType switch
		{
			ActionType.Adal => "adal", 
			ActionType.CustomInteractive => "custominteractive", 
			ActionType.MsaInteractive => "msainteractive", 
			ActionType.MsaNonInteractive => "msanoninteractive", 
			ActionType.Wam => "wam", 
			ActionType.Msal => "msal", 
			_ => "unknown", 
		};
	}

	public static string AsString(AuthOutcome outcome)
	{
		return outcome switch
		{
			AuthOutcome.Cancelled => "canceled", 
			AuthOutcome.Failed => "failed", 
			AuthOutcome.Incomplete => "incomplete", 
			AuthOutcome.Succeeded => "succeeded", 
			_ => "unknown", 
		};
	}

	public static string AsString(ErrorSource errorSource)
	{
		return errorSource switch
		{
			ErrorSource.AuthSdk => "authsdk", 
			ErrorSource.Client => "client", 
			ErrorSource.None => "none", 
			ErrorSource.Service => "service", 
			_ => "unknown", 
		};
	}

	public static string AsString(EventType eventType)
	{
		return eventType switch
		{
			EventType.Scenario => "scenario", 
			EventType.Action => "action", 
			EventType.LibraryError => "error", 
			_ => "unknown", 
		};
	}

	public static string AsString(IdentityService service)
	{
		return service switch
		{
			IdentityService.AAD => "aad", 
			IdentityService.MSA => "msa", 
			_ => "unknown", 
		};
	}

	public static string AsString(TelemetryAudienceType audience)
	{
		return audience switch
		{
			TelemetryAudienceType.PreProduction => "preproduction", 
			TelemetryAudienceType.Production => "production", 
			_ => "unknown", 
		};
	}

	public static string AsString(OsPlatform osPlatform)
	{
		return osPlatform switch
		{
			OsPlatform.Win32 => "win32", 
			OsPlatform.Android => "android", 
			OsPlatform.Ios => "ios", 
			OsPlatform.Mac => "mac", 
			OsPlatform.Winrt => "winrt", 
			_ => "unknown", 
		};
	}

	public static int AsInt(OsPlatform osPlatform)
	{
		return Convert.ToInt32(osPlatform, CultureInfo.InvariantCulture);
	}

	public static string AsString(ApiTelemetryFeature apiTelemetryFeature)
	{
		return apiTelemetryFeature switch
		{
			ApiTelemetryFeature.WithAccount => "api_with_account", 
			ApiTelemetryFeature.WithRedirectUri => "api_with_redirect_uri", 
			ApiTelemetryFeature.WithForceRefresh => "api_with_force_refresh", 
			ApiTelemetryFeature.WithLoginHint => "api_with_login_hint", 
			ApiTelemetryFeature.WithExtraScopesToConsent => "api_with_extra_scopes_to_consent", 
			ApiTelemetryFeature.WithUserAssertion => "api_with_user_assertion", 
			ApiTelemetryFeature.WithSendX5C => "api_with_send_x5c", 
			ApiTelemetryFeature.WithCurrentSynchronizationContext => "api_with_current_sync_context", 
			ApiTelemetryFeature.WithEmbeddedWebView => "api_with_embedded_web_view", 
			ApiTelemetryFeature.WithParent => "api_with_parent", 
			ApiTelemetryFeature.WithPrompt => "api_with_prompt", 
			ApiTelemetryFeature.WithUsername => "api_with_username", 
			ApiTelemetryFeature.WithClaims => "api_with_claims", 
			ApiTelemetryFeature.WithExtraQueryParameters => "api_with_extra_query_parameters", 
			ApiTelemetryFeature.WithAuthority => "api_with_authority", 
			ApiTelemetryFeature.WithValidateAuthority => "api_with_validate_authority", 
			ApiTelemetryFeature.WithAdfsAuthority => "api_with_adfs_authority", 
			ApiTelemetryFeature.WithB2CAuthority => "api_with_b2c_authority", 
			ApiTelemetryFeature.WithCustomWebUi => "api_with_custom_webui", 
			_ => "api_feature_unknown", 
		};
	}
}
