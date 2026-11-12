namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutablePassword : IPassword
{
	new string Value { get; set; }

	new bool IsHashed { get; set; }

	new bool MustChange { get; set; }

	new bool CheckPolicy { get; set; }

	new bool CheckExpiration { get; set; }
}
