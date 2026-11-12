using System;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Identity;

public class DeviceCodeCredentialOptions : TokenCredentialOptions, ITokenCacheOptions
{
	private string _tenantId;

	internal bool DisableAutomaticAuthentication { get; set; }

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

	public string ClientId { get; set; } = "04b07795-8ddb-461a-bbee-02f9e1bf7b46";


	internal bool EnablePersistentCache { get; set; }

	internal bool AllowUnencryptedCache { get; set; }

	internal AuthenticationRecord AuthenticationRecord { get; set; }

	public Func<DeviceCodeInfo, CancellationToken, Task> DeviceCodeCallback { get; set; }

	bool ITokenCacheOptions.EnablePersistentCache => EnablePersistentCache;

	bool ITokenCacheOptions.AllowUnencryptedCache => AllowUnencryptedCache;
}
