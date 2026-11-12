namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IDatabaseDdlTrigger : ITrigger, IMetadataObject, IDatabaseOwnedObject, IDatabaseObject
{
	[ReadWrite]
	ITriggerEventTypeSet DatabaseDdlEvents { get; }

	[ReadWrite]
	bool IsQuotedIdentifierOn { get; }
}
