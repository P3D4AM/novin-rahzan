using System;

namespace Microsoft.Identity.Client;

public static class MsalError
{
	public const string InvalidGrantError = "invalid_grant";

	public const string InteractionRequired = "interaction_required";

	public const string NoTokensFoundError = "no_tokens_found";

	public const string UserNullError = "user_null";

	public const string NoAccountForLoginHint = "no_account_for_login_hint";

	public const string MultipleAccountsForLoginHint = "multiple_accounts_for_login_hint";

	[Obsolete("This error code is not in use")]
	public const string TokenCacheNullError = "token_cache_null";

	public const string NoPromptFailedError = "no_prompt_failed";

	public const string ServiceNotAvailable = "service_not_available";

	public const string RequestTimeout = "request_timeout";

	public const string UpnRequired = "upn_required";

	public const string MissingPassiveAuthEndpoint = "missing_passive_auth_endpoint";

	public const string InvalidAuthority = "invalid_authority";

	public const string InvalidAuthorityType = "invalid_authority_type";

	public const string UnknownError = "unknown_error";

	public const string UnknownBrokerError = "unknown_broker_error";

	public const string AuthenticationFailed = "authentication_failed";

	public const string AuthorityValidationFailed = "authority_validation_failed";

	public const string InvalidOwnerWindowType = "invalid_owner_window_type";

	public const string EncodedTokenTooLong = "encoded_token_too_long";

	public const string UserMismatch = "user_mismatch";

	public const string FailedToRefreshToken = "failed_to_refresh_token";

	public const string FailedToAcquireTokenSilentlyFromBroker = "failed_to_acquire_token_silently_from_broker";

	public const string RedirectUriValidationFailed = "redirect_uri_validation_failed";

	public const string AuthenticationUiFailed = "authentication_ui_failed";

	public const string InternalError = "internal_error";

	public const string AccessingWsMetadataExchangeFailed = "accessing_ws_metadata_exchange_failed";

	public const string FederatedServiceReturnedError = "federated_service_returned_error";

	public const string UserRealmDiscoveryFailed = "user_realm_discovery_failed";

	public const string MissingFederationMetadataUrl = "missing_federation_metadata_url";

	public const string ParsingWsMetadataExchangeFailed = "parsing_ws_metadata_exchange_failed";

	public const string WsTrustEndpointNotFoundInMetadataDocument = "wstrust_endpoint_not_found";

	public const string ParsingWsTrustResponseFailed = "parsing_wstrust_response_failed";

	public const string UnknownUserType = "unknown_user_type";

	public const string UnknownUser = "unknown_user";

	public const string GetUserNameFailed = "get_user_name_failed";

	public const string PasswordRequiredForManagedUserError = "password_required_for_managed_user";

	public const string InvalidRequest = "invalid_request";

	public const string UapCannotFindDomainUser = "user_information_access_failed";

	public const string UapCannotFindUpn = "uap_cannot_find_upn";

	public const string NonParsableOAuthError = "non_parsable_oauth_error";

	public const string CodeExpired = "code_expired";

	public const string IntegratedWindowsAuthNotSupportedForManagedUser = "integrated_windows_auth_not_supported_managed_user";

	public const string ActivityRequired = "activity_required";

	public const string BrokerResponseHashMismatch = "broker_response_hash_mismatch";

	public const string BrokerResponseReturnedError = "broker_response_returned_error";

	public const string BrokerNonceMismatch = "broker_nonce_mismatch";

	public const string CannotInvokeBroker = "cannot_invoke_broker";

	public const string NoAndroidBrokerAccountFound = "no_broker_account_found";

	public const string NoAndroidBrokerInstalledOnDevice = "No_Broker_Installed_On_Device";

	public const string NullIntentReturnedFromAndroidBroker = "null_intent_returned_from_broker";

	public const string AndroidBrokerSignatureVerificationFailed = "broker_signature_verification_failed";

	public const string AndroidBrokerOperationFailed = "android_broker_operation_failed";

	public const string NoUsernameOrAccountIDProvidedForSilentAndroidBrokerAuthentication = "no_username_or_accountid_provided_for_silent_android_broker_authentication";

	public const string HttpStatusNotFound = "not_found";

	public const string HttpStatusCodeNotOk = "http_status_not_200";

	public const string CustomWebUiReturnedInvalidUri = "custom_webui_returned_invalid_uri";

	public const string CustomWebUiRedirectUriMismatch = "custom_webui_invalid_mismatch";

	public const string AccessDenied = "access_denied";

	public const string CannotAccessUserInformationOrUserNotDomainJoined = "user_information_access_failed";

	public const string DefaultRedirectUriIsInvalid = "redirect_uri_validation_failed";

	public const string NoRedirectUri = "no_redirect_uri";

	public const string MultipleTokensMatchedError = "multiple_matching_tokens_detected";

	public const string NonHttpsRedirectNotSupported = "non_https_redirect_failed";

	[Obsolete("MSAL no longer throws this error - it will allow the HttpClient exceptions to propagate. App developers may write their own logic for detecting access to the network issues, for example by using Xamarin.Essentials. ")]
	public const string NetworkNotAvailableError = "network_not_available";

	public const string B2CAuthorityHostMismatch = "B2C_authority_host_mismatch";

	public const string DuplicateQueryParameterError = "duplicate_query_parameter";

	public const string AuthenticationUiFailedError = "authentication_ui_failed";

	public const string AuthenticationCanceledError = "authentication_canceled";

	public const string JsonParseError = "json_parse_failed";

	public const string InvalidJwtError = "invalid_jwt";

	public const string StateMismatchError = "state_mismatch";

	public const string TenantDiscoveryFailedError = "tenant_discovery_failed";

	public const string PlatformNotSupported = "platform_not_supported";

	public const string InvalidAuthorizationUri = "invalid_authorization_uri";

	public const string LoopbackRedirectUri = "loopback_redirect_uri";

	public const string LoopbackResponseUriMismatch = "loopback_response_uri_mismatch";

	public const string LinuxXdgOpen = "linux_xdg_open_failed";

	public const string WebviewUnavailable = "no_system_webview";

	public const string SystemWebviewOptionsNotApplicable = "embedded_webview_not_compatible_default_browser";

	public const string ClientCredentialAuthenticationTypesAreMutuallyExclusive = "Client_Credential_Authentication_Types_Are_Mutually_Exclusive";

	public const string ClientCredentialAuthenticationTypeMustBeDefined = "Client_Credentials_Required_In_Confidential_Client_Application";

	internal const string BasicAction = "basic_action";

	internal const string AdditionalAction = "additional_action";

	internal const string MessageOnly = "message_only";

	internal const string UserPasswordExpired = "user_password_expired";

	internal const string ConsentRequired = "consent_required";

	internal const string BadToken = "bad_token";

	internal const string TokenExpired = "token_expired";

	internal const string ProtectionPolicyRequired = "protection_policy_required";

	internal const string ClientMismatch = "client_mismatch";

	internal const string DeviceAuthenticationFailed = "device_authentication_failed";

	public const string InvalidInstance = "invalid_instance";

	public const string InvalidUserInstanceMetadata = "invalid-custom-instance-metadata";

	public const string ValidateAuthorityOrCustomMetadata = "validate_authority_or_custom_instance_metadata";

	public const string NoClientId = "no_client_id";

	public const string ClientIdMustBeAGuid = "client_id_must_be_guid";

	public const string TelemetryConfigOrTelemetryCallback = "telemetry_config_or_telemetry_callback";

	public const string InvalidClient = "invalid_client";

	public const string SSHCertUsedAsHttpHeader = "ssh_cert_used_as_http_header";

	public const string WABError = "wab_error";

	public const string TokenTypeMismatch = "token_type_mismatch";

	public const string ExperimentalFeature = "experimental_feature";

	public const string BrokerApplicationRequired = "broker_application_required";

	public const string FailedToGetBrokerResponse = "failed_to_get_broker_response";

	public const string InvalidJsonClaimsFormat = "invalid_json_claims_format";

	public const string AuthorityTypeMismatch = "authority_type_mismatch";

	public const string CustomMetadataInstanceOrUri = "custom_metadata_instance_or_uri";

	public const string ScopesRequired = "scopes_required_client_credentials";

	public const string CertWithoutPrivateKey = "cert_without_private_key";

	public const string DeviceCertificateNotFound = "device_certificate_not_found";

	public const string InvalidAdalCacheMultipleRTs = "invalid_adal_cache";

	public const string RegionDiscoveryFailed = "region_discovery_failed";

	public const string RegionDiscoveryNotEnabled = "region_discovery_unavailable";

	public const string CryptoNet45 = "crypto_net45";

	public const string WamUiThread = "wam_ui_thread_only";

	public const string WamNoB2C = "wam_no_b2c";

	public const string WamInteractiveError = "wam_interactive_error";

	public const string WamPickerError = "wam_interactive_picker_error";
}
