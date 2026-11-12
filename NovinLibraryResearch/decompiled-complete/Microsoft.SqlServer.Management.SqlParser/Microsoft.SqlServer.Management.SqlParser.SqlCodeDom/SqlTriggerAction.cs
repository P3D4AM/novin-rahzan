using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTriggerAction : SqlCodeObject
{
	private readonly SqlTriggerActionType type;

	internal override bool IsVisitable => true;

	public SqlTriggerActionType Type => type;

	internal string DisplayName => GetActionName(type);

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

	internal SqlTriggerAction(SqlTriggerActionType type)
	{
		this.type = type;
	}

	private static string GetActionName(SqlTriggerActionType actionType)
	{
		return actionType switch
		{
			SqlTriggerActionType.Delete => "DELETE", 
			SqlTriggerActionType.Insert => "INSERT", 
			SqlTriggerActionType.Update => "UPDATE", 
			_ => null, 
		};
	}
}
