namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IServerDdlTrigger : ITrigger, IMetadataObject, IServerOwnedObject, IDatabaseObject
{
	[ReadWrite]
	ITriggerEventTypeSet ServerDdlEvents { get; }

	[ReadWrite]
	bool IsQuotedIdentifierOn { get; }
}
