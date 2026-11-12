using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUnaryScalarExpression : SqlScalarExpression
{
	private readonly SqlUnaryScalarOperatorType operatorType;

	private readonly SqlScalarExpression expression;

	internal override bool IsVisitable => true;

	public override IScalar BoundScalar => expression.BoundScalar;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Unary;

	internal override bool IsConstantExpression => expression.IsConstantExpression;

	public SqlUnaryScalarOperatorType Operator => operatorType;

	public SqlScalarExpression Expression => expression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return expression;
		}
	}

	internal sealed override bool IsValidFilterExpressionConstant
	{
		get
		{
			if (operatorType == SqlUnaryScalarOperatorType.Negative || operatorType == SqlUnaryScalarOperatorType.Positive)
			{
				return expression.IsValidFilterExpressionConstant;
			}
			return false;
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

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		else if (caretLocation.endTokenNumber < expression.Position.startTokenNumber)
		{
			result = RetrieveCompletionListItems(completionContext);
		}
		return result;
	}

	internal SqlUnaryScalarExpression(SqlUnaryScalarOperatorType type, SqlScalarExpression expression)
	{
		this.expression = expression;
		operatorType = type;
	}
}
