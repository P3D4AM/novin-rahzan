namespace Azure.Identity;

internal interface ITokenCacheOptions
{
	bool EnablePersistentCache { get; }

	bool AllowUnencryptedCache { get; }
}
