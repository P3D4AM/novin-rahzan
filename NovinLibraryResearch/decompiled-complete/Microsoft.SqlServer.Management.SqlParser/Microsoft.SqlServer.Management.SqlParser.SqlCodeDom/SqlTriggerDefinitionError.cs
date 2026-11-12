using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTriggerDefinitionError : SqlTriggerDefinition, ISqlErrorObject, IDdlTriggerDefinitionInfo, IDmlTriggerDefinitionInfo
{
	private readonly TriggerType triggerType;

	private readonly SqlObjectIdentifier targetName;

	private readonly SqlDdlTriggerTargetType targetType;

	private readonly SqlCollection<SqlTriggerAction> actions;

	private readonly SqlCollection<SqlTriggerEvent> events;

	private readonly bool isWithAppend;

	private readonly bool isNotForReplication;

	internal override bool IsVisitable => true;

	public bool IsNotForReplication => isNotForReplication;

	public bool IsWithAppend => isWithAppend;

	public SqlDdlTriggerTargetType TargetType => targetType;

	public SqlCollection<SqlTriggerEvent> Events => events;

	public SqlCollection<SqlTriggerAction> Actions => actions;

	public SqlObjectIdentifier TargetName => targetName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			if (targetName != null)
			{
				yield return targetName;
			}
			if (base.Options != null)
			{
				foreach (SqlModuleOption option in base.Options)
				{
					yield return option;
				}
			}
			if (actions != null)
			{
				foreach (SqlTriggerAction action in actions)
				{
					yield return action;
				}
			}
			if (events == null)
			{
				yield break;
			}
			foreach (SqlTriggerEvent @event in events)
			{
				yield return @event;
			}
		}
	}

	internal override TriggerType TriggerType => triggerType;

	internal override IDmlTriggerDefinitionInfo DmlDefinitionInfo => this;

	internal override IDdlTriggerDefinitionInfo DdlDefinitionInfo => this;

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal SqlTriggerDefinitionError(TriggerType triggerType, SqlObjectIdentifier name, SqlObjectIdentifier targetName, SqlDdlTriggerTargetType targetType, SqlCollection<SqlModuleOption> options, SqlTriggerActivationType activationType, List<SqlTriggerAction> actions, List<SqlTriggerEvent> events, bool isWithAppend, bool isNotForReplication)
		: base(name, options, activationType)
	{
		this.triggerType = triggerType;
		this.targetName = targetName;
		this.targetType = targetType;
		this.actions = ((actions != null) ? new SqlCollection<SqlTriggerAction>(actions) : null);
		this.events = ((events != null) ? new SqlCollection<SqlTriggerEvent>(events) : null);
		this.isWithAppend = isWithAppend;
		this.isNotForReplication = isNotForReplication;
	}
}
