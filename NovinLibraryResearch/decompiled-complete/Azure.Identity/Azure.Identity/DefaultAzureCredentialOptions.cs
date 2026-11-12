namespace Azure.Identity;

public class DefaultAzureCredentialOptions : TokenCredentialOptions
{
	public string InteractiveBrowserTenantId { get; set; } = EnvironmentVariables.TenantId;


	public string SharedTokenCacheTenantId { get; set; } = EnvironmentVariables.TenantId;


	public string VisualStudioTenantId { get; set; } = EnvironmentVariables.TenantId;


	public string VisualStudioCodeTenantId { get; set; } = EnvironmentVariables.TenantId;


	public string SharedTokenCacheUsername { get; set; } = EnvironmentVariables.Username;


	public string ManagedIdentityClientId { get; set; } = EnvironmentVariables.ClientId;


	public bool ExcludeEnvironmentCredential { get; set; }

	public bool ExcludeManagedIdentityCredential { get; set; }

	public bool ExcludeSharedTokenCacheCredential { get; set; }

	public bool ExcludeInteractiveBrowserCredential { get; set; } = true;


	public bool ExcludeAzureCliCredential { get; set; }

	public bool ExcludeVisualStudioCredential { get; set; }

	public bool ExcludeVisualStudioCodeCredential { get; set; }
}
