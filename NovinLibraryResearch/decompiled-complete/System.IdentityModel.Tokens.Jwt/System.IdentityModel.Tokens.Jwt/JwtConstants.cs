namespace System.IdentityModel.Tokens.Jwt;

public static class JwtConstants
{
	public const string HeaderType = "JWT";

	public const string HeaderTypeAlt = "http://openid.net/specs/jwt/1.0";

	public const string TokenType = "JWT";

	public const string TokenTypeAlt = "urn:ietf:params:oauth:token-type:jwt";

	public const string JsonCompactSerializationRegex = "^[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]*$";

	public const string JweCompactSerializationRegex = "^[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]*\\.[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]+$";

	internal const int JweSegmentCount = 5;

	internal const int JwsSegmentCount = 3;

	internal const int MaxJwtSegmentCount = 5;

	public const string DirectKeyUseAlg = "dir";
}
