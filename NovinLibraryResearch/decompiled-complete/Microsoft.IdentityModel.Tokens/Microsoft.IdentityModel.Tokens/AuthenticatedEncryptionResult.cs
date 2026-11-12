namespace Microsoft.IdentityModel.Tokens;

public class AuthenticatedEncryptionResult
{
	public SecurityKey Key { get; private set; }

	public byte[] Ciphertext { get; private set; }

	public byte[] IV { get; private set; }

	public byte[] AuthenticationTag { get; private set; }

	public AuthenticatedEncryptionResult(SecurityKey key, byte[] ciphertext, byte[] iv, byte[] authenticationTag)
	{
		Key = key;
		Ciphertext = ciphertext;
		IV = iv;
		AuthenticationTag = authenticationTag;
	}
}
