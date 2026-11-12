namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDatabaseDdlTrigger : IDatabaseDdlTrigger, ITrigger, IMetadataObject, IDatabaseOwnedObject, IDatabaseObject, IMutableTrigger, IMutableMetadataObject, IMutableDatabaseOwnedObject, IMutableDatabaseObject
{
	[ReadWrite]
	new IMutableTriggerEventTypeSet DatabaseDdlEvents { get; }

	new bool IsQuotedIdentifierOn { get; set; }
}
