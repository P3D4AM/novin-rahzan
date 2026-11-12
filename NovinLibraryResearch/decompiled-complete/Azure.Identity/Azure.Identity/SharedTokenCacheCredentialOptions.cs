namespace Azure.Identity;

public class SharedTokenCacheCredentialOptions : TokenCredentialOptions, ITokenCacheOptions
{
	private string _tenantId;

	public string Username { get; set; }

	public string TenantId
	{
		get
		{
			return _tenantId;
		}
		set
		{
			_tenantId = Validations.ValidateTenantId(value, null, allowNull: true);
		}
	}

	internal AuthenticationRecord AuthenticationRecord { get; set; }

	internal bool AllowUnencryptedCache { get; set; }

	bool ITokenCacheOptions.AllowUnencryptedCache => AllowUnencryptedCache;

	bool ITokenCacheOptions.EnablePersistentCache => true;
}
