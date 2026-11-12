namespace Microsoft.Identity.Client.TelemetryCore.Internal.Constants;

internal static class MsalTelemetryBlobEventNames
{
	public const string MsalCorrelationIdConstStrKey = "Microsoft.MSAL.correlation_id";

	public const string ApiTelemIdConstStrKey = "msal.api_telem_id";

	public const string ApiIdConstStrKey = "msal.api_id";

	public const string BrokerAppConstStrKey = "Microsoft_MSAL_broker_app";

	public const string CacheEventCountConstStrKey = "Microsoft_MSAL_cache_event_count";

	public const string HttpEventCountTelemetryBatchKey = "Microsoft_MSAL_http_event_count";

	public const string IdpConstStrKey = "Microsoft_MSAL_idp";

	public const string IsSilentTelemetryBatchKey = "";

	public const string IsSuccessfulConstStrKey = "Microsoft_MSAL_is_successful";

	public const string ResponseTimeConstStrKey = "Microsoft_MSAL_response_time";

	public const string TenantIdConstStrKey = "Microsoft_MSAL_tenant_id";

	public const string UiEventCountTelemetryBatchKey = "Microsoft_MSAL_ui_event_count";

	public const string ApiErrorCodeConstStrKey = "msal.api_error_code";

	public const string ForceRefreshId = "api_with_force_refresh";

	public const string RegionDiscovered = "msal.region_discovered";
}
