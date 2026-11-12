namespace Azure.Identity;

internal class ManagedIdentityClientOptions
{
	public TokenCredentialOptions Options { get; set; }

	public string ClientId { get; set; }

	public bool PreserveTransport { get; set; }

	public CredentialPipeline Pipeline { get; set; }
}
