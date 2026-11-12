using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens;

internal class JwtTokenDecryptionParameters
{
	public string Alg { get; set; }

	public string AuthenticationTag { get; set; }

	public string Ciphertext { get; set; }

	public Func<byte[], string, string> DecompressionFunction { get; set; }

	public string Enc { get; set; }

	public string EncodedHeader { get; set; }

	public string EncodedToken { get; set; }

	public string InitializationVector { get; set; }

	public IEnumerable<SecurityKey> Keys { get; set; }

	public string Zip { get; set; }
}
