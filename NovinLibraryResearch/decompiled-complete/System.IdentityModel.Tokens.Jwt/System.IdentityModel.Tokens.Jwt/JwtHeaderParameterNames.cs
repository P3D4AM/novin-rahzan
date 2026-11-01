using System.Runtime.InteropServices;

namespace System.IdentityModel.Tokens.Jwt;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct JwtHeaderParameterNames
{
	public const string Alg = "alg";

	public const string Cty = "cty";

	public const string Enc = "enc";

	public const string IV = "iv";

	public const string Jku = "jku";

	public const string Jwk = "jwk";

	public const string Kid = "kid";

	public const string Typ = "typ";

	public const string X5c = "x5c";

	public const string X5t = "x5t";

	public const string X5u = "x5u";

	public const string Zip = "zip";
}
