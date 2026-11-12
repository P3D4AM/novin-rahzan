using Azure.Core;

namespace Azure.Identity;

internal class DefaultAzureCredentialFactory
{
	public CredentialPipeline Pipeline { get; }

	public DefaultAzureCredentialFactory(TokenCredentialOptions options)
		: this(CredentialPipeline.GetInstance(options))
	{
	}

	protected DefaultAzureCredentialFactory(CredentialPipeline pipeline)
	{
		Pipeline = pipeline;
	}

	public virtual TokenCredential CreateEnvironmentCredential()
	{
		return new EnvironmentCredential(Pipeline);
	}

	public virtual TokenCredential CreateManagedIdentityCredential(string clientId)
	{
		return new ManagedIdentityCredential(clientId, Pipeline);
	}

	public virtual TokenCredential CreateSharedTokenCacheCredential(string tenantId, string username)
	{
		return new SharedTokenCacheCredential(tenantId, username, null, Pipeline);
	}

	public virtual TokenCredential CreateInteractiveBrowserCredential(string tenantId)
	{
		return new InteractiveBrowserCredential(tenantId, "04b07795-8ddb-461a-bbee-02f9e1bf7b46", new InteractiveBrowserCredentialOptions
		{
			EnablePersistentCache = true
		}, Pipeline);
	}

	public virtual TokenCredential CreateAzureCliCredential()
	{
		return new AzureCliCredential(Pipeline, null);
	}

	public virtual TokenCredential CreateVisualStudioCredential(string tenantId)
	{
		return new VisualStudioCredential(tenantId, Pipeline, null, null);
	}

	public virtual TokenCredential CreateVisualStudioCodeCredential(string tenantId)
	{
		return new VisualStudioCodeCredential(new VisualStudioCodeCredentialOptions
		{
			TenantId = tenantId
		}, Pipeline, null, null, null);
	}
}
