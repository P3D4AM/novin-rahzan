using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDmlTriggerDefinition : SqlTriggerDefinition, IDmlTriggerDefinitionInfo
{
	private readonly SqlObjectIdentifier targetName;

	private readonly SqlCollection<SqlTriggerAction> actions;

	private readonly bool isWithAppend;

	private readonly bool isNotForReplication;

	internal override bool IsVisitable => true;

	public bool IsNotForReplication => isNotForReplication;

	public bool IsWithAppend => isWithAppend;

	public SqlCollection<SqlTriggerAction> Actions => actions;

	public SqlObjectIdentifier TargetName => targetName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			yield return targetName;
			if (base.Options != null)
			{
				foreach (SqlModuleOption option in base.Options)
				{
					yield return option;
				}
			}
			foreach (SqlTriggerAction action in actions)
			{
				yield return action;
			}
		}
	}

	internal override TriggerType TriggerType => TriggerType.DmlTrigger;

	internal override IDmlTriggerDefinitionInfo DmlDefinitionInfo => this;

	internal override IDdlTriggerDefinitionInfo DdlDefinitionInfo => null;

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

	internal SqlDmlTriggerDefinition(SqlObjectIdentifier name, SqlObjectIdentifier targetName, SqlCollection<SqlModuleOption> options, SqlTriggerActivationType activationType, List<SqlTriggerAction> actions, bool isWithAppend, bool isNotForReplication)
		: base(name, options, activationType)
	{
		this.targetName = targetName;
		this.actions = new SqlCollection<SqlTriggerAction>(actions);
		this.isWithAppend = isWithAppend;
		this.isNotForReplication = isNotForReplication;
		int num = 0;
		foreach (SqlTriggerAction action in actions)
		{
			int num2 = 1 << (int)action.Type;
			if ((num & num2) != 0)
			{
				action.ReportParseError(SR.DuplicateTriggerActionType(action.DisplayName));
			}
			num |= num2;
		}
		if (isWithAppend && activationType != SqlTriggerActivationType.For)
		{
			name.ReportParseError(SR.OptionNotRecognized("APPEND"));
		}
	}
}
