namespace Azure.Core;

public readonly struct TokenRequestContext
{
	public string[] Scopes { get; }

	public string? ParentRequestId { get; }

	public TokenRequestContext(string[] scopes, string? parentRequestId = null)
	{
		Scopes = scopes;
		ParentRequestId = parentRequestId;
	}
}
