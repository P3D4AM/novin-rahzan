using System.Runtime.InteropServices;

namespace System.IdentityModel.Tokens.Jwt;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct JwtRegisteredClaimNames
{
	public const string Actort = "actort";

	public const string Acr = "acr";

	public const string Amr = "amr";

	public const string Aud = "aud";

	public const string AuthTime = "auth_time";

	public const string Azp = "azp";

	public const string Birthdate = "birthdate";

	public const string CHash = "c_hash";

	public const string AtHash = "at_hash";

	public const string Email = "email";

	public const string Exp = "exp";

	public const string Gender = "gender";

	public const string FamilyName = "family_name";

	public const string GivenName = "given_name";

	public const string Iat = "iat";

	public const string Iss = "iss";

	public const string Jti = "jti";

	public const string NameId = "nameid";

	public const string Nonce = "nonce";

	public const string Nbf = "nbf";

	public const string Prn = "prn";

	public const string Sid = "sid";

	public const string Sub = "sub";

	public const string Typ = "typ";

	public const string UniqueName = "unique_name";

	public const string Website = "website";
}
