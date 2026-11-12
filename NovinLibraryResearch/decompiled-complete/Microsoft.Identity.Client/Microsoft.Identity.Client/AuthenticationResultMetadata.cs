namespace Microsoft.Identity.Client;

public class AuthenticationResultMetadata
{
	public TokenSource TokenSource { get; }

	public AuthenticationResultMetadata(TokenSource tokenSource)
	{
		TokenSource = tokenSource;
	}
}
