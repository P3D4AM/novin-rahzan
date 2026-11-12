using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSelectScalarExpression : SqlSelectExpression
{
	private sealed class SqlSelectIdentityFunctionExpression : SqlSelectScalarExpression
	{
		internal override bool IsIdentityFunction => true;

		public SqlSelectIdentityFunctionExpression(SqlIdentityFunctionCallExpression identityFunction, SqlIdentifier alias)
			: base(identityFunction, alias)
		{
		}
	}

	private readonly SqlIdentifier alias;

	private readonly SqlScalarExpression expression;

	internal override bool IsVisitable => true;

	internal override SelectExpressionType ExpressionType => SelectExpressionType.Scalar;

	internal override SqlSelectScalarExpression AsSelectScalarExpression => this;

	internal virtual bool IsIdentityFunction => false;

	public SqlIdentifier Alias => alias;

	public SqlScalarExpression Expression => expression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (alias != null)
			{
				if (alias.Position.StartsBefore(expression.Position))
				{
					yield return alias;
					yield return expression;
				}
				else
				{
					yield return expression;
					yield return alias;
				}
			}
			else
			{
				yield return expression;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		if (IsIdentityFunction)
		{
			((SqlIdentityFunctionCallExpression)expression).Bind(bindingContext, alias);
		}
		else
		{
			expression.Bind(bindingContext);
		}
		base.BindingState |= expression.BindingState;
		IScalar scalar = expression.BoundScalar;
		IColumnFactory column = bindingContext.MetadataFactory.Column;
		IColumn column2 = null;
		if (!SqlIdentifier.IsNullOrEmpty(alias))
		{
			if (scalar == null)
			{
				scalar = column.Null;
			}
			IColumn column3 = BindUtils.Scalar.AsColumn(scalar);
			column2 = ((column3 == null) ? column.CreateScalarAlias(scalar, alias.Value) : column.CreateColumnAlias(column3, alias.Value));
		}
		else if (scalar != null)
		{
			column2 = BindUtils.Scalar.AsColumn(scalar);
			if (column2 == null)
			{
				column2 = column.CreateAnonymousColumn(scalar.DataType, scalar.Nullable);
			}
		}
		else
		{
			column2 = column.Null;
		}
		bindingContext.AddLocalColumnExpression(column2);
	}

	internal override bool MayChildHaveCompletionList(SqlCodeObject child)
	{
		return child != alias;
	}

	internal SqlSelectScalarExpression(SqlScalarExpression expression)
		: this(expression, null)
	{
	}

	internal SqlSelectScalarExpression(SqlScalarExpression expression, SqlIdentifier alias)
	{
		this.expression = expression;
		this.alias = alias;
	}

	internal static SqlSelectScalarExpression IdentityFunction(SqlIdentityFunctionCallExpression identityFunction, SqlIdentifier alias)
	{
		return new SqlSelectIdentityFunctionExpression(identityFunction, alias);
	}
}
