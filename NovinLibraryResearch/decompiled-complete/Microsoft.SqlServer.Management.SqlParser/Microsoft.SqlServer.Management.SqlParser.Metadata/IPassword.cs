namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IPassword
{
	[ReadWrite]
	string Value { get; }

	[ReadWrite]
	bool IsHashed { get; }

	[ReadWrite]
	bool MustChange { get; }

	[ReadWrite]
	bool CheckPolicy { get; }

	[ReadWrite]
	bool CheckExpiration { get; }
}
