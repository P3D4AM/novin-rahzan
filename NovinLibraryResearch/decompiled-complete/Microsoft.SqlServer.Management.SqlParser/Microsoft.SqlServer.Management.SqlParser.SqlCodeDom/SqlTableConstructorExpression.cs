using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableConstructorExpression : SqlQueryExpression
{
	private LocalBindingContext bindingContext;

	private readonly SqlRowConstructorExpressionCollection rows;

	internal override bool IsVisitable => true;

	internal override LocalBindingContext LocalBindingContext => bindingContext;

	internal override IBindingContext BindingContext => bindingContext;

	internal override bool HasUnionAll => false;

	public SqlRowConstructorExpressionCollection Rows => rows;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlRowConstructorExpression row in rows)
			{
				yield return row;
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
		LocalBindingContext localBindingContext = (this.bindingContext = bindingContext.CreateLocalBindingContext());
		base.Bind(localBindingContext);
		SqlRowConstructorExpression sqlRowConstructorExpression = rows[0];
		IColumnFactory column = bindingContext.MetadataFactory.Column;
		for (int i = 0; i < sqlRowConstructorExpression.Values.Count; i++)
		{
			IScalar boundScalar = sqlRowConstructorExpression.Values[i].BoundScalar;
			if (boundScalar != null)
			{
				localBindingContext.AddLocalColumnExpression(column.CreateAnonymousColumn(boundScalar.DataType, boundScalar.Nullable));
			}
			else
			{
				localBindingContext.AddLocalColumnExpression(column.Null);
			}
		}
		int count = sqlRowConstructorExpression.Values.Count;
		for (int j = 1; j < rows.Count; j++)
		{
			SqlRowConstructorExpression sqlRowConstructorExpression2 = rows[j];
			if (sqlRowConstructorExpression2.Values.Count != count)
			{
				sqlRowConstructorExpression2.ReportBindError(SR.NumberOfColumnsMustBeTheSame);
				base.BindingState |= BindingState.BindFailed;
			}
		}
	}

	internal override void Unbind()
	{
		base.Unbind();
		bindingContext = null;
	}

	internal override void ShallowBind(CteBindingContext bindingContext)
	{
		base.BindingState |= BindingState.BindFailed;
	}

	internal override void DeepBind(CteBindingContext bindingContext, SqlIdentifier cteName)
	{
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.Scalar | CompletionContext.LocalTableExpressionsOnly | CompletionContext.Columns | CompletionContext.Tables);
	}

	internal SqlTableConstructorExpression(List<SqlRowConstructorExpression> rows)
	{
		this.rows = new SqlRowConstructorExpressionCollection(rows);
	}

	internal override SqlSelectIntoClause GetIntoClause()
	{
		return null;
	}

	internal override void Validate(QueryExpressionValidateOption option)
	{
	}
}
