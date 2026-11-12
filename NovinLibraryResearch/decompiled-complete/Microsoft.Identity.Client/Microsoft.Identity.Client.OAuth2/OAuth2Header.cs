namespace Microsoft.Identity.Client.OAuth2;

internal static class OAuth2Header
{
	public const string CorrelationId = "client-request-id";

	public const string RequestCorrelationIdInResponse = "return-client-request-id";

	public const string AppName = "x-app-name";

	public const string AppVer = "x-app-ver";
}
