namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[ReadOnly("Name")]
public interface IDmlTrigger : ITrigger, IMetadataObject
{
	[ReadOnly]
	[Parent]
	ITableViewBase Parent { get; }

	[ReadWrite]
	bool NotForReplication { get; }

	[ReadWrite]
	bool InsteadOf { get; }

	[ReadWrite]
	bool Insert { get; }

	[ReadWrite]
	ActivationOrder InsertActivationOrder { get; }

	[ReadWrite]
	bool Update { get; }

	[ReadWrite]
	ActivationOrder UpdateActivationOrder { get; }

	[ReadWrite]
	bool Delete { get; }

	[ReadWrite]
	ActivationOrder DeleteActivationOrder { get; }

	[ReadWrite]
	bool IsQuotedIdentifierOn { get; }
}
