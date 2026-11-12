using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlColumnIdentity : SqlConstraint
{
	private readonly SqlLiteralExpression seed;

	private readonly SqlLiteralExpression increment;

	private bool notForReplicationClause;

	internal override bool IsVisitable => true;

	public int? Seed
	{
		get
		{
			if (seed == null)
			{
				return null;
			}
			return seed.AsInt32();
		}
	}

	public int? Increment
	{
		get
		{
			if (increment == null)
			{
				return null;
			}
			return increment.AsInt32();
		}
	}

	public bool NotForReplicationClause => notForReplicationClause;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.Name != null)
			{
				yield return base.Name;
			}
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

	internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
	}

	internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable, IMutableColumn boundColumn)
	{
		string columnName = boundColumn.Name;
		IScalarDataType dataType = boundColumn.DataType;
		if (!BindUtils.Column.IsComputed(boundColumn) && BindUtils.Column.BindColumnIdentitySpec(columnName, dataType, seed, increment, this))
		{
			long num = ((seed != null) ? seed.AsInt64().GetValueOrDefault(1L) : 1);
			long num2 = ((increment != null) ? increment.AsInt64().GetValueOrDefault(1L) : 1);
			bool notForReplication = notForReplicationClause;
			boundColumn.IdentityColumnInfo = new IdentityColumnInfo(num, num2, notForReplication);
			bindingHelper.NullableColumns[boundColumn] = false;
			base.BindingState = BindingState.BindSucceeded;
		}
		else
		{
			base.BindingState = BindingState.BindFailed;
		}
	}

	internal SqlColumnIdentity()
		: base(SqlConstraintType.Identity)
	{
	}

	internal SqlColumnIdentity(SqlLiteralExpression seed, SqlLiteralExpression increment)
		: this()
	{
		this.seed = seed;
		this.increment = increment;
	}

	internal void AddNotForReplicationClause()
	{
		notForReplicationClause = true;
	}
}
