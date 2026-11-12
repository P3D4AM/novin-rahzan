namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableServerDdlTrigger : IServerDdlTrigger, ITrigger, IMetadataObject, IServerOwnedObject, IDatabaseObject, IMutableTrigger, IMutableMetadataObject, IMutableServerOwnedObject, IMutableDatabaseObject
{
	new IMutableTriggerEventTypeSet ServerDdlEvents { get; }

	new bool IsQuotedIdentifierOn { get; set; }
}
