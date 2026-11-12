namespace Azure.Identity;

internal class UsernamePasswordCredentialOptions : TokenCredentialOptions, ITokenCacheOptions
{
	public bool EnablePersistentCache { get; set; }

	public bool AllowUnencryptedCache { get; set; }
}
