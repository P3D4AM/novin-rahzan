namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface IDdlTriggerDefinitionInfo
{
	SqlTriggerActivationType ActivationType { get; }

	SqlDdlTriggerTargetType TargetType { get; }

	SqlCollection<SqlTriggerEvent> Events { get; }
}
