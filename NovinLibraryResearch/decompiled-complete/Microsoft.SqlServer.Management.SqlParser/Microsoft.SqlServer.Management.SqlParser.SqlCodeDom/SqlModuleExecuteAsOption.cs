using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public sealed class SqlModuleExecuteAsOption : SqlModuleOption
{
	private SqlExecuteAsClause executeAsClause;

	internal override bool IsVisitable => true;

	internal override SqlExecuteAsClause ExecuteAsClause => executeAsClause;

	internal override string DisplayName => "EXECUTE AS";

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return executeAsClause;
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

	internal SqlModuleExecuteAsOption(SqlExecuteAsClause executeAsClause)
		: base(SqlModuleOptionType.ExecuteAs)
	{
		this.executeAsClause = executeAsClause;
	}
}
