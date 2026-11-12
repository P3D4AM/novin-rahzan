namespace Microsoft.IdentityModel.Tokens;

public abstract class CryptoProviderCache
{
	protected abstract string GetCacheKey(SignatureProvider signatureProvider);

	protected abstract string GetCacheKey(SecurityKey securityKey, string algorithm, string typeofProvider);

	public abstract bool TryAdd(SignatureProvider signatureProvider);

	public abstract bool TryGetSignatureProvider(SecurityKey securityKey, string algorithm, string typeofProvider, bool willCreateSignatures, out SignatureProvider signatureProvider);

	public abstract bool TryRemove(SignatureProvider signatureProvider);
}
