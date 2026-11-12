using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal sealed class TriggerActivationData
{
	public readonly TriggerType TriggerType;

	public readonly SqlTriggerActivationType ActivationType;

	public readonly List<SqlTriggerAction> Actions;

	public readonly List<SqlTriggerEvent> Events;

	private TriggerActivationData(TriggerType triggerType, SqlTriggerActivationType activationType, List<SqlTriggerAction> actions, List<SqlTriggerEvent> events)
	{
		TriggerType = triggerType;
		ActivationType = activationType;
		Actions = actions;
		Events = events;
	}

	public static TriggerActivationData CreateDmlActivation(SqlTriggerActivationType activationType, List<SqlTriggerAction> actions)
	{
		return new TriggerActivationData(TriggerType.DmlTrigger, activationType, actions, null);
	}

	public static TriggerActivationData CreateDdlActivation(SqlTriggerActivationType activationType, List<SqlTriggerEvent> events)
	{
		return new TriggerActivationData(TriggerType.DdlTrigger, activationType, null, events);
	}
}
