namespace Azure.Identity;

internal class ClientSecretCredentialOptions : TokenCredentialOptions, ITokenCacheOptions
{
	public bool EnablePersistentCache { get; set; }

	public bool AllowUnencryptedCache { get; set; }
}
