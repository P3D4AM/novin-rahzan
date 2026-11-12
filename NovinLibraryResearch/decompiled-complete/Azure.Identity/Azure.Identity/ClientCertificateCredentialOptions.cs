namespace Azure.Identity;

public class ClientCertificateCredentialOptions : TokenCredentialOptions, ITokenCacheOptions
{
	internal bool EnablePersistentCache { get; set; }

	internal bool AllowUnencryptedCache { get; set; }

	public bool SendCertificateChain { get; set; }

	bool ITokenCacheOptions.EnablePersistentCache => EnablePersistentCache;

	bool ITokenCacheOptions.AllowUnencryptedCache => AllowUnencryptedCache;
}
