namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface IDmlTriggerDefinitionInfo
{
	SqlTriggerActivationType ActivationType { get; }

	SqlObjectIdentifier TargetName { get; }

	SqlCollection<SqlTriggerAction> Actions { get; }

	bool IsWithAppend { get; }

	bool IsNotForReplication { get; }
}
