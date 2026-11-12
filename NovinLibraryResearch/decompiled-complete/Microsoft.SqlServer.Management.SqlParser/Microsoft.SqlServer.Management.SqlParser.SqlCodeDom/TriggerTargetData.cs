namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal sealed class TriggerTargetData
{
	public readonly TriggerType TriggerType;

	public readonly SqlObjectIdentifier TargetName;

	public readonly SqlDdlTriggerTargetType TargetType;

	private TriggerTargetData(TriggerType triggerType, SqlObjectIdentifier targetName, SqlDdlTriggerTargetType targetType)
	{
		TriggerType = triggerType;
		TargetName = targetName;
		TargetType = targetType;
	}

	public static TriggerTargetData CreateDmlTarget(SqlObjectIdentifier targetName)
	{
		return new TriggerTargetData(TriggerType.DmlTrigger, targetName, SqlDdlTriggerTargetType.Database);
	}

	public static TriggerTargetData CreateDdlTarget(SqlDdlTriggerTargetType targetType)
	{
		return new TriggerTargetData(TriggerType.DdlTrigger, null, targetType);
	}
}
