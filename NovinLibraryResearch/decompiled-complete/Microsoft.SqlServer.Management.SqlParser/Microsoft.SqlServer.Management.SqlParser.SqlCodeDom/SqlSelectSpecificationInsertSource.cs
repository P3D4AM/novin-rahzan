using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSelectSpecificationInsertSource : SqlInsertSource
{
	private readonly SqlSelectSpecification selectSpecification;

	internal override bool IsVisitable => true;

	public SqlSelectSpecification SelectSpecification => selectSpecification;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return selectSpecification;
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

	internal SqlSelectSpecificationInsertSource(SqlSelectSpecification selectSpecification)
	{
		this.selectSpecification = selectSpecification;
		this.selectSpecification.QueryExpression.Validate(QueryExpressionValidateOption.IntoClauseSyntaxError);
	}
}
