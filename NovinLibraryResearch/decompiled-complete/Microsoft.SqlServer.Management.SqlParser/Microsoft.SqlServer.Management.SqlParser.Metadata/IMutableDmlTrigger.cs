namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDmlTrigger : IDmlTrigger, ITrigger, IMetadataObject, IMutableTrigger, IMutableMetadataObject
{
	new bool NotForReplication { get; set; }

	new bool InsteadOf { get; set; }

	new bool Insert { get; set; }

	new ActivationOrder InsertActivationOrder { get; set; }

	new bool Update { get; set; }

	new ActivationOrder UpdateActivationOrder { get; set; }

	new bool Delete { get; set; }

	new ActivationOrder DeleteActivationOrder { get; set; }

	new bool IsQuotedIdentifierOn { get; set; }
}
