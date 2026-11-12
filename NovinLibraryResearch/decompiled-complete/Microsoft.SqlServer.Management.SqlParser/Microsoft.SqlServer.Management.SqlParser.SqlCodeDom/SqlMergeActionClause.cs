using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMergeActionClause : SqlCodeObject
{
	private readonly SqlMergeConditionType conditionType;

	private readonly SqlBooleanExpression mergeSearchCondition;

	private readonly SqlMergeAction mergeAction;

	internal override bool IsVisitable => true;

	public SqlMergeConditionType MergeConditionType => conditionType;

	public SqlBooleanExpression SearchCondition => mergeSearchCondition;

	public SqlMergeAction Action => mergeAction;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (mergeSearchCondition != null)
			{
				yield return mergeSearchCondition;
			}
			yield return mergeAction;
		}
	}

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

	internal SqlMergeActionClause(SqlMergeConditionType conditionType, SqlMergeAction mergeAction)
		: this(conditionType, null, mergeAction)
	{
	}

	internal SqlMergeActionClause(SqlMergeConditionType conditionType, SqlBooleanExpression mergeSearchCondition, SqlMergeAction mergeAction)
	{
		this.conditionType = conditionType;
		this.mergeSearchCondition = mergeSearchCondition;
		this.mergeAction = mergeAction;
	}
}
