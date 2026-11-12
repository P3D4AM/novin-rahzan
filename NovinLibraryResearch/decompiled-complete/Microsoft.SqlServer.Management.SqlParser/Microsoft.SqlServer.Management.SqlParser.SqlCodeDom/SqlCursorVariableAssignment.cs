using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCursorVariableAssignment : SqlVariableAssignment
{
	private readonly SqlCursorVariableRefExpression variable;

	private readonly SqlSelectStatement selectStatement;

	private readonly List<SqlCursorOption> options;

	internal override bool IsVisitable => true;

	internal sealed override bool IsStatementLevelObject => true;

	public SqlCursorVariableRefExpression Variable => variable;

	public SqlSelectStatement SelectStatement => selectStatement;

	public IEnumerable<SqlCursorOption> Options => options;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return variable;
			if (options != null)
			{
				foreach (SqlCursorOption option in options)
				{
					yield return option;
				}
			}
			yield return selectStatement;
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

	internal override DatabaseContext GetDatabaseContext(LexicalScope lexScope)
	{
		return base.Parent.GetDatabaseContext(lexScope);
	}

	public SqlCursorVariableAssignment(SqlCursorVariableRefExpression variable, CursorDefinitionInfo definitionInfo)
		: base(SqlAssignmentOperatorType.Assignment)
	{
		this.variable = variable;
		selectStatement = definitionInfo.SelectStatement;
		if (definitionInfo.ExtendedCursorOptions.Count != 0)
		{
			options = definitionInfo.ExtendedCursorOptions;
		}
	}
}
