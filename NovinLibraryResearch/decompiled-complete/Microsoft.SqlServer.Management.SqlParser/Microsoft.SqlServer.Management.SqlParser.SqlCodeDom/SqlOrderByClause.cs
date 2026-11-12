using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlOrderByClause : SqlCodeObject
{
	private class OrderByClauseBindingContext : LocalBindingContext
	{
		private readonly LocalBindingContext m_baseBindingContext;

		public OrderByClauseBindingContext(LocalBindingContext bindingContext)
			: base(bindingContext)
		{
			m_baseBindingContext = bindingContext;
		}

		public override IColumn FindColumn(string name, out bool isAmbiguous)
		{
			IMetadataOrderedCollection<IColumn> localColumnExpressions = m_baseBindingContext.LocalColumnExpressions;
			IColumn column = null;
			isAmbiguous = false;
			foreach (IColumn item in localColumnExpressions.FindAll(name))
			{
				if (column != null)
				{
					isAmbiguous = true;
					column = null;
					break;
				}
				column = item;
			}
			if (column == null && !isAmbiguous)
			{
				column = m_baseBindingContext.FindColumn(name, out isAmbiguous);
			}
			return column;
		}
	}

	private OrderByClauseBindingContext orderByBindingContext;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => orderByBindingContext;

	public SqlOrderByItemCollection Items { get; private set; }

	public SqlOffsetFetchClause OffsetFetchClause { get; internal set; }

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlOrderByItem item in Items)
			{
				yield return item;
			}
			if (OffsetFetchClause != null)
			{
				yield return OffsetFetchClause;
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
		OrderByClauseBindingContext bindingContext2 = (orderByBindingContext = new OrderByClauseBindingContext(bindingContext.AsLocalBindingContext));
		IMetadataOrderedCollection<IColumn> localColumnExpressions = bindingContext.LocalColumnExpressions;
		for (int i = 0; i < Items.Count; i++)
		{
			SqlScalarExpression expression = Items[i].Expression;
			int position = i + 1;
			if (expression.IsConstantExpression)
			{
				bool flag = false;
				while (expression.IsUnary)
				{
					SqlUnaryScalarExpression sqlUnaryScalarExpression = expression as SqlUnaryScalarExpression;
					bool flag2 = false;
					switch (sqlUnaryScalarExpression.Operator)
					{
					case SqlUnaryScalarOperatorType.Positive:
						expression = sqlUnaryScalarExpression.Expression;
						break;
					case SqlUnaryScalarOperatorType.Negative:
						expression = sqlUnaryScalarExpression.Expression;
						flag = !flag;
						break;
					case SqlUnaryScalarOperatorType.BitwiseNot:
						flag2 = true;
						break;
					}
					if (flag2)
					{
						break;
					}
				}
				if (expression.IsLiteral && !expression.IsCollate)
				{
					SqlLiteralExpression sqlLiteralExpression = expression as SqlLiteralExpression;
					if (sqlLiteralExpression.Type == LiteralValueType.Integer)
					{
						int? num = sqlLiteralExpression.AsInt32();
						int num2 = (flag ? (-num.Value) : num.Value);
						if (num2 <= 0 || num2 > localColumnExpressions.Count)
						{
							ReportBindError(SR.OrderByPositionNumberIsOutOfRange(num2));
						}
					}
					else if (sqlLiteralExpression.Type == LiteralValueType.String || sqlLiteralExpression.Type == LiteralValueType.UnicodeString)
					{
						if (!localColumnExpressions.Contains(sqlLiteralExpression.Value))
						{
							ReportBindError(SR.OrderByListHasConstantExpression(position));
						}
					}
					else
					{
						ReportBindError(SR.OrderByListHasConstantExpression(position));
					}
				}
				else
				{
					ReportBindError(SR.OrderByListHasConstantExpression(position));
				}
			}
			else
			{
				while (expression.IsUnary)
				{
					SqlUnaryScalarExpression sqlUnaryScalarExpression2 = expression as SqlUnaryScalarExpression;
					if (sqlUnaryScalarExpression2.Operator != SqlUnaryScalarOperatorType.Positive)
					{
						break;
					}
					expression = sqlUnaryScalarExpression2.Expression;
				}
				if (expression.IsCollate)
				{
					expression.Bind(bindingContext);
				}
				else if (expression.IsColumnReference)
				{
					expression.Bind(bindingContext2);
				}
				else if (expression.IsVariableReference)
				{
					ReportBindError(SR.OrderByItemContainsVariable(position));
					expression.BindingState |= BindingState.BindFailed;
				}
				else
				{
					expression.Bind(bindingContext);
				}
			}
			base.BindingState |= expression.BindingState;
		}
	}

	internal override void Unbind()
	{
		base.Unbind();
		orderByBindingContext = null;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.ScalarPlusAliases);
	}

	internal SqlOrderByClause(List<SqlOrderByItem> items)
	{
		Items = new SqlOrderByItemCollection(items);
	}

	internal SqlOrderByClause(List<SqlOrderByItem> items, SqlOffsetFetchClause offsetFetchClause)
		: this(items)
	{
		OffsetFetchClause = offsetFetchClause;
	}
}
