namespace Azure.Identity;

public class VisualStudioCredentialOptions : TokenCredentialOptions
{
	private string _tenantId;

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
}
