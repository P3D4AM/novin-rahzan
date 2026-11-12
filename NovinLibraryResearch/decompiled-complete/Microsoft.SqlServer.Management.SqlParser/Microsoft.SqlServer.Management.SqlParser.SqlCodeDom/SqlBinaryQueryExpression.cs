using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBinaryQueryExpression : SqlQueryExpression
{
	private readonly SqlBinaryQueryOperatorType operatorType;

	private readonly SqlQueryExpression left;

	private readonly SqlQueryExpression right;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => left.BindingContext;

	internal override LocalBindingContext LocalBindingContext => left.LocalBindingContext;

	internal override bool HasUnionAll
	{
		get
		{
			if (operatorType != SqlBinaryQueryOperatorType.UnionAll && !left.HasUnionAll)
			{
				return right.HasUnionAll;
			}
			return true;
		}
	}

	public SqlBinaryQueryOperatorType Operator => operatorType;

	public SqlQueryExpression Left => left;

	public SqlQueryExpression Right => right;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return left;
			yield return right;
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

	internal override void ShallowBind(CteBindingContext bindingContext)
	{
		left.ShallowBind(bindingContext);
	}

	internal override void DeepBind(CteBindingContext bindingContext, SqlIdentifier cteName)
	{
		left.DeepBind(bindingContext, cteName);
		if (operatorType == SqlBinaryQueryOperatorType.UnionAll)
		{
			bindingContext.CteBindMode |= CteBindMode.Recursive;
		}
		right.Bind(bindingContext);
		bool flag = (bindingContext.QueryFlags & CteQueryFlags.HasSelfReference) == CteQueryFlags.HasSelfReference;
		if (bindingContext.CteBindMode == CteBindMode.Recursive && !flag)
		{
			right.ReportBindError(SR.AnchorMemberFoundInRecursiveQuery(cteName.Value));
		}
		if (operatorType == SqlBinaryQueryOperatorType.UnionAll && flag)
		{
			bindingContext.CteBindMode = CteBindMode.Recursive;
		}
		bindingContext.QueryFlags &= ~CteQueryFlags.HasSelfReference;
	}

	internal SqlBinaryQueryExpression(SqlQueryExpression left, SqlBinaryQueryOperatorType expressionType, SqlQueryExpression right)
		: this(left, expressionType, isAll: false, right)
	{
	}

	internal SqlBinaryQueryExpression(SqlQueryExpression left, SqlBinaryQueryOperatorType expressionType, bool isAll, SqlQueryExpression right)
	{
		this.left = left;
		operatorType = expressionType;
		if (isAll)
		{
			if (expressionType == SqlBinaryQueryOperatorType.Union)
			{
				operatorType = SqlBinaryQueryOperatorType.UnionAll;
			}
			else
			{
				ReportParseError(SR.OperatorNotSupported(expressionType.ToString()));
			}
		}
		this.right = right;
	}

	internal override SqlSelectIntoClause GetIntoClause()
	{
		return left.GetIntoClause();
	}

	internal override void Validate(QueryExpressionValidateOption option)
	{
		switch (option)
		{
		case QueryExpressionValidateOption.IntoClauseSyntaxError:
			left.Validate(option);
			right.Validate(option);
			break;
		case QueryExpressionValidateOption.IntoClauseAllowLeftmost:
		case QueryExpressionValidateOption.IntoClauseDisallow:
			left.Validate(option);
			right.Validate(QueryExpressionValidateOption.IntoClauseDisallow);
			break;
		}
	}
}
