namespace Microsoft.Identity.Client;

public interface IAccount
{
	string Username { get; }

	string Environment { get; }

	AccountId HomeAccountId { get; }
}
