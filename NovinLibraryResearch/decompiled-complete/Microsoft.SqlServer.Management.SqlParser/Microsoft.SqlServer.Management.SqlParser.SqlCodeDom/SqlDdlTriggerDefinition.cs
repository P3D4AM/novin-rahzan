using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDdlTriggerDefinition : SqlTriggerDefinition, IDdlTriggerDefinitionInfo
{
	private readonly SqlDdlTriggerTargetType targetType;

	private readonly SqlCollection<SqlTriggerEvent> events;

	internal override bool IsVisitable => true;

	public SqlDdlTriggerTargetType TargetType => targetType;

	public SqlCollection<SqlTriggerEvent> Events => events;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			if (base.Options != null)
			{
				foreach (SqlModuleOption option in base.Options)
				{
					yield return option;
				}
			}
			foreach (SqlTriggerEvent @event in events)
			{
				yield return @event;
			}
		}
	}

	internal override TriggerType TriggerType => TriggerType.DdlTrigger;

	internal override IDmlTriggerDefinitionInfo DmlDefinitionInfo => null;

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

	internal SqlDdlTriggerDefinition(SqlObjectIdentifier name, SqlDdlTriggerTargetType targetType, SqlCollection<SqlModuleOption> options, SqlTriggerActivationType activationType, List<SqlTriggerEvent> events)
		: base(name, options, activationType)
	{
		this.targetType = targetType;
		this.events = new SqlCollection<SqlTriggerEvent>(events);
	}
}
