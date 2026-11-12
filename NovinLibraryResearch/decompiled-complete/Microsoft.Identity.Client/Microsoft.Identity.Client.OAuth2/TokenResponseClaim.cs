namespace Microsoft.Identity.Client.OAuth2;

internal class TokenResponseClaim : OAuth2ResponseBaseClaim
{
	public const string Code = "code";

	public const string TokenType = "token_type";

	public const string AccessToken = "access_token";

	public const string RefreshToken = "refresh_token";

	public const string IdToken = "id_token";

	public const string Scope = "scope";

	public const string ClientInfo = "client_info";

	public const string ExpiresIn = "expires_in";

	public const string CloudInstanceHost = "cloud_instance_host_name";

	public const string CreatedOn = "created_on";

	public const string ExtendedExpiresIn = "ext_expires_in";

	public const string Authority = "authority";

	public const string FamilyId = "foci";

	public const string RefreshIn = "refresh_in";
}
